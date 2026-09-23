#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.2"
#load "build_common.csx"

using NitroHelper;

const uint PINYIN_OVERLAY_ADDRESS = 0x0226FFC0;
const int PINYIN_OVERLAY_ID = 0;
const int PINYIN_LOADER_HOST_OVERLAY_ID = 60;
const uint PINYIN_LOADER_ADDRESS = 0x020F8834;
const uint ARENA_LO_POINTER_ADDRESS = 0x020C27AC;

BuildPinyinKeyboard("Pt");
var (pinyinOverlaySymbols, pinyinLoaderSymbols, pinyinOverlayBinary, pinyinLoaderBinary) =
  LoadPinyinKeyboard("pt_overlay_0000", "pt_overlay_ldr");
if ((pinyinOverlaySymbols["NameInProc_Main"] | 1) !=
    pinyinOverlaySymbols["NameInProcMainPreHook"])
{
  throw new Exception("Pt pre-hook symbol does not match the retail NameInProc_Main callback.");
}

var GAME_CODE_TO_TITLE = new Dictionary<string, string>
{
  ["Pt"] = "宝可梦 白金\nNintendo",
};

var messages = LoadMessages("temp/Pt/messages.txt");
var easyChatWordsIds = new int[] {
  408, // monsname
  636, // wazaname
  616, // typename
  604, // tokusei
  433, // pms_word06
  434, // pms_word07
  435, // pms_word08
  436, // pms_word09
  437, // pms_word10
  438, // pms_word11
  439, // pms_word12
};
IEnumerable<string> easyChatWords = [];
foreach (var id in easyChatWordsIds)
{
  easyChatWords = easyChatWords.Concat(messages[id].Values);
}

foreach (var gameCode in GAME_CODE_TO_TITLE.Keys)
{
  PrepareBuild("Pt", gameCode);

  // Edit arm9.bin
  var arm9 = File.ReadAllBytes($"original_files/Pt/{gameCode}/arm9.bin");
  Dictionary<string, string> symbols = new();

  CompileArm9(ref arm9, ref symbols, "Pt", gameCode);

  // Sort easy chat words
  var easyChatWordsArray = easyChatWords.ToArray();
  var aikotobaList = new List<string>();
  using (var br = new BinaryReader(File.OpenRead($"original_files/Pt/{gameCode}/pms_aikotoba.bin")))
  {
    while (br.BaseStream.Position != br.BaseStream.Length)
    {
      aikotobaList.Add(easyChatWordsArray[br.ReadInt32()]);
    }
  }
  File.WriteAllLines($"out/Aikotoba-Pt.txt", aikotobaList);
  SortEasyChatWords(ref arm9, 0xf7044, easyChatWordsArray);

  // Install the pinyin overlay loader into a verified free area in ARM9.
  InstallPinyinLoader(arm9, PINYIN_LOADER_ADDRESS, pinyinLoaderBinary);

  // Route A, touch, B, and R decisions through the semantic input hook.
  var decideHook = pinyinOverlaySymbols["NativeNameIn_DecideMainButton"];
  WriteThumbBl(arm9, 0x020866B2, decideHook, new byte[] { 0x01, 0xF0, 0x7F, 0xFD });
  WriteThumbBl(arm9, 0x020866DA, decideHook, new byte[] { 0x01, 0xF0, 0x6B, 0xFD });
  WriteThumbBl(arm9, 0x020866F2, decideHook, new byte[] { 0x01, 0xF0, 0x5F, 0xFD });
  WriteThumbBl(arm9, 0x02086704, decideHook, new byte[] { 0x01, 0xF0, 0x56, 0xFD });

  // Reserve the overlay and its BSS at the bottom of the SDK arena.
  ReservePinyinArena(arm9, ARENA_LO_POINTER_ADDRESS, PINYIN_OVERLAY_ADDRESS,
    PINYIN_OVERLAY_ADDRESS, pinyinOverlaySymbols);

  File.WriteAllBytes($"out/{gameCode}/arm9.bin", arm9);
  Console.WriteLine($"Edited: arm9.bin");

  // Edit overlay files
  CompileOverlays(ref symbols, "Pt", gameCode);
  WritePinyinOverlay(gameCode, PINYIN_OVERLAY_ID, pinyinOverlayBinary);

  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

  // Edit overarm9.bin
  var overarm9Bytes = ReadOverlayTable("Pt", gameCode, 97);

  // Reuse the retail test overlay (ID 0), so the .xzp only replaces existing ROM files.
  WritePinyinOverlayTable(overarm9Bytes, PINYIN_OVERLAY_ID, PINYIN_OVERLAY_ADDRESS, pinyinOverlaySymbols);

  // Run the loader from the game-start overlay, then preserve its original initializer.
  var hostEntryOffset = PINYIN_LOADER_HOST_OVERLAY_ID * 0x20;
  var loaderInit = pinyinLoaderSymbols["OverlayStaticInitFunc"];
  WriteUInt32(overarm9Bytes, hostEntryOffset + 0x10, loaderInit);
  WriteUInt32(overarm9Bytes, hostEntryOffset + 0x14, loaderInit + sizeof(uint));

  File.WriteAllBytes($"out/{gameCode}/overarm9.bin", overarm9Bytes);
  Console.WriteLine($"Edited: overarm9.bin");

  EditBanner("Pt", gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"original_files/Pt/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}
