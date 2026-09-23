#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.2"
#load "build_common.csx"

using NitroHelper;

const uint PINYIN_OVERLAY_ADDRESS = 0x02264840;
const int PINYIN_OVERLAY_ID = 0;
const uint PINYIN_LOADER_ADDRESS = 0x022647C0;
const uint ARENA_LO_POINTER_ADDRESS = 0x020CE978;

var GAME_CODE_TO_TITLE = new Dictionary<string, string>
{
  ["D"] = "宝可梦 钻石\nNintendo",
  ["P"] = "宝可梦 珍珠\nNintendo",
};

var messages = LoadMessages("temp/DP/messages.txt");
var easyChatWordsIds = new int[] {
  356, // monsname
  575, // wazaname
  555, // typename
  544, // tokusei
  380, // pms_word06
  381, // pms_word07
  382, // pms_word08
  383, // pms_word09
  384, // pms_word10
  385, // pms_word11
  386, // pms_word12
};
IEnumerable<string> easyChatWords = [];
foreach (var id in easyChatWordsIds)
{
  easyChatWords = easyChatWords.Concat(messages[id].Values);
}

foreach (var gameCode in GAME_CODE_TO_TITLE.Keys)
{
  BuildPinyinKeyboard(gameCode);
  var (pinyinOverlaySymbols, pinyinLoaderSymbols, pinyinOverlayBinary, pinyinLoaderBinary) =
    LoadPinyinKeyboard($"{gameCode}_overlay_0000", $"{gameCode}_overlay_ldr");

  PrepareBuild("DP", gameCode);

  // Edit arm9.bin
  var arm9 = File.ReadAllBytes($"original_files/DP/{gameCode}/arm9.bin");
  Dictionary<string, string> symbols = new();

  CompileArm9(ref arm9, ref symbols, "DP", gameCode);

  // Sort easy chat words
  var aikotobaList = SortEasyChatWords(ref arm9, (uint)(gameCode == "D" ? 0x1001b4 : 0x1001b8), easyChatWords.ToArray());
  if (gameCode == "D") { File.WriteAllLines($"out/Aikotoba-DP.txt", aikotobaList); }

  InstallPinyinMainHook(arm9, pinyinOverlaySymbols, pinyinLoaderSymbols);

  // Route A, touch, B, and R decisions through the semantic input hook.
  uint shift = gameCode == "P" ? 4u : 0;
  var decideHook = pinyinOverlaySymbols["NativeNameIn_DecideMainButton"];
  WriteThumbBl(arm9, 0x0207E5AA + shift, decideHook, new byte[] { 0xFE, 0xF7, (byte)(gameCode == "D" ? 0x2D : 0x2B), 0xF8 });
  WriteThumbBl(arm9, 0x0207E5D2 + shift, decideHook, new byte[] { 0xFE, 0xF7, (byte)(gameCode == "D" ? 0x19 : 0x17), 0xF8 });
  WriteThumbBl(arm9, 0x0207E5EC + shift, decideHook, new byte[] { 0xFE, 0xF7, (byte)(gameCode == "D" ? 0x0C : 0x0A), 0xF8 });
  WriteThumbBl(arm9, 0x0207E600 + shift, decideHook, new byte[] { 0xFE, 0xF7, (byte)(gameCode == "D" ? 0x02 : 0x00), 0xF8 });

  // Reserve the loader, overlay, and its BSS at the bottom of the SDK arena.
  ReservePinyinArena(arm9, ARENA_LO_POINTER_ADDRESS + shift, PINYIN_LOADER_ADDRESS,
    PINYIN_OVERLAY_ADDRESS, pinyinOverlaySymbols);

  // Install the loader through a third SDK autoload entry; DP has no verified padding.
  // Preserve both retail autoload destinations, their bytes, and static BSS.
  if (pinyinLoaderBinary.Length > PINYIN_OVERLAY_ADDRESS - PINYIN_LOADER_ADDRESS ||
      (pinyinLoaderBinary.Length & 3) != 0 ||
      pinyinLoaderSymbols["__bss_size__"] != 0 ||
      pinyinLoaderSymbols["NativeNameIn_LazyMain"] != PINYIN_LOADER_ADDRESS)
  {
    throw new Exception("DP pinyin loader does not fit the reserved autoload region.");
  }
  if (arm9.Length != 0x1090A4 ||
      BitConverter.ToUInt32(arm9, 0xB5C) != 0x02109080 ||
      BitConverter.ToUInt32(arm9, 0xB60) != 0x02109098 ||
      BitConverter.ToUInt32(arm9, 0xB64) != 0x02108940)
  {
    throw new Exception("Unexpected DP SDK autoload layout.");
  }

  var expanded = new byte[arm9.Length + pinyinLoaderBinary.Length + 12];
  Array.Copy(arm9, expanded, 0x109080);
  Array.Copy(pinyinLoaderBinary, 0, expanded, 0x109080, pinyinLoaderBinary.Length);
  int tableOffset = 0x109080 + pinyinLoaderBinary.Length;
  Array.Copy(arm9, 0x109080, expanded, tableOffset, 24);
  WriteUInt32(expanded, tableOffset + 24, PINYIN_LOADER_ADDRESS);
  WriteUInt32(expanded, tableOffset + 28, (uint)pinyinLoaderBinary.Length);
  WriteUInt32(expanded, tableOffset + 32, 0);
  Array.Copy(arm9, 0x109098, expanded, tableOffset + 36, 12);
  WriteUInt32(expanded, 0xB5C, ARM9_RAM_ADDRESS + (uint)tableOffset);
  WriteUInt32(expanded, 0xB60, ARM9_RAM_ADDRESS + (uint)tableOffset + 36);
  arm9 = expanded;
  File.WriteAllBytes($"out/{gameCode}/arm9.bin", arm9);
  Console.WriteLine($"Edited: arm9.bin");

  // Edit overlay files
  CompileOverlays(ref symbols, "DP", gameCode);
  WritePinyinOverlay(gameCode, PINYIN_OVERLAY_ID, pinyinOverlayBinary);

  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

  // Edit overarm9.bin
  var overarm9Bytes = ReadOverlayTable("DP", gameCode, 83);
  for (int i = 32; i < overarm9Bytes.Length; i += 32)
  {
    uint end = BitConverter.ToUInt32(overarm9Bytes, i + 4) + BitConverter.ToUInt32(overarm9Bytes, i + 8) +
      BitConverter.ToUInt32(overarm9Bytes, i + 12);
    if (end > PINYIN_LOADER_ADDRESS)
    {
      throw new Exception("A retail DP overlay overlaps the pinyin loader.");
    }
  }
  WritePinyinOverlayTable(overarm9Bytes, PINYIN_OVERLAY_ID, PINYIN_OVERLAY_ADDRESS, pinyinOverlaySymbols);
  File.WriteAllBytes($"out/{gameCode}/overarm9.bin", overarm9Bytes);
  Console.WriteLine($"Edited: overarm9.bin");

  EditBanner("DP", gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy preprocessing
  CopyFolder($"original_files/DP/{gameCode}/preprocessing", $"out/{gameCode}/preprocessing");

  // Copy md5.txt
  File.Copy($"original_files/DP/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}
