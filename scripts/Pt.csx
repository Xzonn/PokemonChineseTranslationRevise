#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.2"
#load "lib.csx"

using NitroHelper;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

const uint ARM9_RAM_ADDRESS = 0x02000000;
const uint PINYIN_OVERLAY_ADDRESS = 0x0226FFC0;
const int PINYIN_OVERLAY_ID = 0;
const int PINYIN_LOADER_HOST_OVERLAY_ID = 60;
const uint PINYIN_LOADER_ADDRESS = 0x020F8834;
const uint ARENA_LO_POINTER_ADDRESS = 0x020C27AC;

Dictionary<string, uint> ReadNmSymbols(string path)
{
  var symbols = new Dictionary<string, uint>();
  var pattern = new Regex(@"^(?<address>[0-9a-fA-F]+)\s+\w\s+(?<name>\S+)$");
  foreach (var line in File.ReadLines(path))
  {
    var match = pattern.Match(line.Trim());
    if (!match.Success) { continue; }
    symbols[match.Groups["name"].Value] =
      uint.Parse(match.Groups["address"].Value, NumberStyles.HexNumber);
  }
  return symbols;
}

void BuildPinyinKeyboard()
{
  ProcessStartInfo psi = new()
  {
    FileName = "make",
    Arguments = "-C asm/pinyin_keyboard GAME=Pt",
    UseShellExecute = false,
    RedirectStandardError = true,
    RedirectStandardOutput = true,
  };
  using Process process = new() { StartInfo = psi };
  process.OutputDataReceived += (_, e) => { if (!string.IsNullOrEmpty(e.Data)) Console.WriteLine(e.Data); };
  process.ErrorDataReceived += (_, e) => { if (!string.IsNullOrEmpty(e.Data)) Console.WriteLine(e.Data); };
  process.Start();
  process.BeginOutputReadLine();
  process.BeginErrorReadLine();
  process.WaitForExit();
  if (process.ExitCode != 0)
  {
    throw new Exception($"Pinyin keyboard build failed with exit code {process.ExitCode}.");
  }
}

void WriteUInt32(byte[] data, int offset, uint value)
{
  Array.Copy(BitConverter.GetBytes(value), 0, data, offset, sizeof(uint));
}

void WriteThumbBl(byte[] data, uint sourceAddress, uint targetAddress, byte[] expected)
{
  var sourceOffset = checked((int)(sourceAddress - ARM9_RAM_ADDRESS));
  if (!data.Skip(sourceOffset).Take(4).SequenceEqual(expected))
  {
    throw new Exception($"Unexpected Thumb BL bytes at 0x{sourceAddress:X8}.");
  }

  var delta = checked((int)targetAddress - (int)(sourceAddress + 4));
  if ((delta & 1) != 0 || delta < -0x400000 || delta > 0x3FFFFE)
  {
    throw new Exception(
      $"Thumb BL target out of range: 0x{sourceAddress:X8} -> 0x{targetAddress:X8}."
    );
  }

  var upper = (ushort)(0xF000 | ((delta >> 12) & 0x7FF));
  var lower = (ushort)(0xF800 | ((delta >> 1) & 0x7FF));
  Array.Copy(BitConverter.GetBytes(upper), 0, data, sourceOffset, sizeof(ushort));
  Array.Copy(BitConverter.GetBytes(lower), 0, data, sourceOffset + 2, sizeof(ushort));
}

BuildPinyinKeyboard();
var pinyinOverlaySymbols = ReadNmSymbols("asm/pinyin_keyboard/pt_overlay_0000.sym");
var pinyinLoaderSymbols = ReadNmSymbols("asm/pinyin_keyboard/pt_overlay_ldr.sym");
var pinyinOverlayBinary = File.ReadAllBytes("asm/pinyin_keyboard/pt_overlay_0000.bin");
if ((pinyinOverlaySymbols["NameInProc_Main"] | 1) !=
    pinyinOverlaySymbols["NameInProcMainPreHook"])
{
  throw new Exception("Pt pre-hook symbol does not match the retail NameInProc_Main callback.");
}
if (pinyinOverlayBinary.Length != pinyinOverlaySymbols["__ram_size__"])
{
  throw new Exception(
    $"Pinyin overlay size mismatch: bin=0x{pinyinOverlayBinary.Length:X}, " +
    $"ELF=0x{pinyinOverlaySymbols["__ram_size__"]:X}."
  );
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
  Directory.CreateDirectory($"out/{gameCode}/data/");
  Directory.CreateDirectory($"out/{gameCode}/overlay/");
  if (Directory.Exists("asm/Pt/build"))
  {
    while (true)
    {
      try
      {
        Directory.Delete("asm/Pt/build", true);
        break;
      }
      catch { }
    }
  }
  foreach (var path in Directory.EnumerateFiles("asm/Pt", "repl_*"))
  {
    File.Delete(path);
  }

  // Edit arm9.bin
  var arm9 = File.ReadAllBytes($"original_files/Pt/{gameCode}/arm9.bin");
  Dictionary<string, string> symbols = new();

  foreach (var folder in Directory.EnumerateDirectories("asm/Pt/replSource/"))
  {
    int address = Convert.ToInt32(Path.GetFileName(folder), 16);
    Compile(ref arm9, ref symbols, address, "Pt", gameCode);
  }

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
  var pinyinLoader = File.ReadAllBytes("asm/pinyin_keyboard/pt_overlay_ldr.bin");
  if (pinyinLoader.Length > 0x12C)
  {
    throw new Exception($"Pinyin overlay loader is too large: 0x{pinyinLoader.Length:X} > 0x12C.");
  }
  var loaderOffset = checked((int)(PINYIN_LOADER_ADDRESS - ARM9_RAM_ADDRESS));
  if (arm9.Skip(loaderOffset).Take(pinyinLoader.Length).Any(x => x != 0))
  {
    throw new Exception("Pinyin loader target is no longer empty; re-analyze the ARM9 layout.");
  }
  Array.Copy(pinyinLoader, 0, arm9, loaderOffset, pinyinLoader.Length);

  // Route A, touch, B, and R decisions through the semantic input hook.
  var decideHook = pinyinOverlaySymbols["NativeNameIn_DecideMainButton"];
  WriteThumbBl(arm9, 0x020866B2, decideHook, new byte[] { 0x01, 0xF0, 0x7F, 0xFD });
  WriteThumbBl(arm9, 0x020866DA, decideHook, new byte[] { 0x01, 0xF0, 0x6B, 0xFD });
  WriteThumbBl(arm9, 0x020866F2, decideHook, new byte[] { 0x01, 0xF0, 0x5F, 0xFD });
  WriteThumbBl(arm9, 0x02086704, decideHook, new byte[] { 0x01, 0xF0, 0x56, 0xFD });

  // Reserve the overlay and its BSS at the bottom of the SDK arena.
  var pinyinOverlayEnd = PINYIN_OVERLAY_ADDRESS + pinyinOverlaySymbols["__size__"];
  var newArenaLo = (pinyinOverlayEnd + 0xF) & ~0xFu;
  var arenaLoOffset = checked((int)(ARENA_LO_POINTER_ADDRESS - ARM9_RAM_ADDRESS));
  var originalArenaLo = BitConverter.ToUInt32(arm9, arenaLoOffset);
  if (originalArenaLo != PINYIN_OVERLAY_ADDRESS)
  {
    throw new Exception(
      $"Unexpected ArenaLo 0x{originalArenaLo:X8}; expected 0x{PINYIN_OVERLAY_ADDRESS:X8}."
    );
  }
  WriteUInt32(arm9, arenaLoOffset, newArenaLo);

  File.WriteAllBytes($"out/{gameCode}/arm9.bin", arm9);
  Console.WriteLine($"Edited: arm9.bin");

  // Edit overlay files
  var overlay9Table = new OverlayTable($"original_files/Pt/{gameCode}/overarm9.bin", 0, (uint)new FileInfo($"original_files/Pt/{gameCode}/overarm9.bin").Length, true);
  for (int i = 0; i < overlay9Table.overlayTable.Count; i++)
  {
    if (!Directory.Exists($"asm/Pt/overlay_{i:D4}")) { continue; }
    var overlay = File.ReadAllBytes($"original_files/Pt/{gameCode}/overlay/overlay_{i:D4}.bin");
    var ramAddress = overlay9Table.overlayTable[i].ramAddress;
    foreach (var folder in Directory.EnumerateDirectories($"asm/Pt/overlay_{i:D4}/"))
    {
      var address = Convert.ToInt32(Path.GetFileName(folder), 16);
      Compile(ref overlay, ref symbols, address, "Pt", gameCode, $"overlay_{i:D4}", ramAddress);
    }
    File.WriteAllBytes($"out/{gameCode}/overlay/overlay_{i:D4}.bin", overlay);
    Console.WriteLine($"Edited: overlay_{i:D4}.bin");
  }

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0000.bin", pinyinOverlayBinary);
  Console.WriteLine("Added: overlay_0000.bin (pinyin keyboard)");

  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

  // Edit overarm9.bin
  using var overarm9Stream = File.OpenRead($"original_files/Pt/{gameCode}/overarm9.bin");
  var overarm9 = new OverlayTable(overarm9Stream, 0, (uint)overarm9Stream.Length, true);
  overarm9.overlayTable[97].ramSize = (uint)File.ReadAllBytes($"out/{gameCode}/overlay/overlay_0097.bin").Length;
  using var outputStream = new MemoryStream();
  overarm9.WriteTo(outputStream);
  var overarm9Bytes = outputStream.ToArray()[..(0x20 * overarm9.overlayTable.Count)];

  // Reuse the retail test overlay (ID 0), so the .xzp only replaces existing ROM files.
  var pinyinEntryOffset = PINYIN_OVERLAY_ID * 0x20;
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x04, PINYIN_OVERLAY_ADDRESS);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x08, pinyinOverlaySymbols["__ram_size__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x0C, pinyinOverlaySymbols["__bss_size__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x10, pinyinOverlaySymbols["__init_functions_begin__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x14, pinyinOverlaySymbols["__init_functions_end__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x1C, 0); // uncompressed

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
