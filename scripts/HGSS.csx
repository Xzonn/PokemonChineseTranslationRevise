#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.2"
#r "nuget: Xzonn.BlzHelper, 0.9.0"
#load "lib.csx"

using NitroHelper;
using Xzonn.BlzHelper;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

const uint ARM9_RAM_ADDRESS = 0x02000000;
const uint PINYIN_OVERLAY_ADDRESS = 0x0226E140;
const int PINYIN_OVERLAY_ID = 11;
const uint PINYIN_LOADER_ADDRESS = 0x021082C0;
const uint ARENA_LO_POINTER_ADDRESS = 0x020D24D4;

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
    Arguments = "-C asm/pinyin_keyboard GAME=HGSS",
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
    throw new Exception($"HGSS pinyin keyboard build failed with exit code {process.ExitCode}.");
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
    throw new Exception($"Thumb BL target out of range: 0x{sourceAddress:X8} -> 0x{targetAddress:X8}.");
  }
  var upper = (ushort)(0xF000 | ((delta >> 12) & 0x7FF));
  var lower = (ushort)(0xF800 | ((delta >> 1) & 0x7FF));
  Array.Copy(BitConverter.GetBytes(upper), 0, data, sourceOffset, sizeof(ushort));
  Array.Copy(BitConverter.GetBytes(lower), 0, data, sourceOffset + 2, sizeof(ushort));
}

BuildPinyinKeyboard();
var pinyinOverlaySymbols = ReadNmSymbols("asm/pinyin_keyboard/hgss_overlay_0011.sym");
var pinyinLoaderSymbols = ReadNmSymbols("asm/pinyin_keyboard/hgss_overlay_ldr.sym");
var pinyinOverlayBinary = File.ReadAllBytes("asm/pinyin_keyboard/hgss_overlay_0011.bin");
var pinyinLoaderBinary = File.ReadAllBytes("asm/pinyin_keyboard/hgss_overlay_ldr.bin");
if (pinyinOverlayBinary.Length != pinyinOverlaySymbols["__ram_size__"])
{
  throw new Exception("HGSS pinyin overlay file/RAM size mismatch.");
}

var GAME_CODE_TO_TITLE = new Dictionary<string, string>
{
  ["HG"] = "宝可梦 心金\nNintendo",
  ["SS"] = "宝可梦 魂银\nNintendo",
};

var messages = LoadMessages("temp/HGSS/messages.txt");
var easyChatWordsIds = new int[] {
  232, // monsname
  739, // wazaname
  724, // typename
  711, // tokusei
  278, // pms_word06
  279, // pms_word07
  280, // pms_word08
  281, // pms_word09
  282, // pms_word10
  283, // pms_word11
  284, // pms_word12
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
  if (Directory.Exists("asm/HGSS/build"))
  {
    while (true)
    {
      try
      {
        Directory.Delete("asm/HGSS/build", true);
        break;
      }
      catch { }
    }
  }
  foreach (var path in Directory.EnumerateFiles("asm/HGSS", "repl_*"))
  {
    File.Delete(path);
  }

  // Build import.o
  ProcessStartInfo psi = new()
  {
    FileName = "make",
    Arguments = $"TARGET=import SOURCES=import",
    WorkingDirectory = $"asm/HGSS",
    UseShellExecute = false,
    RedirectStandardError = true,
    RedirectStandardOutput = true,
  };
  Process p = new() { StartInfo = psi };
  static void func(object sender, DataReceivedEventArgs e)
  {
    if (!string.IsNullOrEmpty(e.Data))
    {
      Console.WriteLine(e.Data);
    }
  }
  p.OutputDataReceived += func;
  p.ErrorDataReceived += func;
  p.Start();
  p.BeginOutputReadLine();
  p.BeginErrorReadLine();
  p.WaitForExit();

  // Decopress and edit arm9.bin
  var arm9Comp = File.ReadAllBytes($"original_files/HGSS/{gameCode}/arm9.bin");
  var nitroCode = arm9Comp.Skip(arm9Comp.Length - 12).ToArray();
  var arm9 = BLZ.Decompress(arm9Comp.Take(arm9Comp.Length - 12).ToArray());
  Dictionary<string, string> symbols = new();

  foreach (var folder in Directory.EnumerateDirectories("asm/HGSS/replSource/"))
  {
    int address = Convert.ToInt32(Path.GetFileName(folder), 16);
    Compile(ref arm9, ref symbols, address, "HGSS", gameCode);
  }

  // Install a lazy loader in verified ARM9 padding. The first name-input
  // frame loads the persistent pinyin overlay, then dispatches to its hook.
  var loaderOffset = checked((int)(PINYIN_LOADER_ADDRESS - ARM9_RAM_ADDRESS));
  if (pinyinLoaderBinary.Length > 0x12C ||
      arm9.Skip(loaderOffset).Take(pinyinLoaderBinary.Length).Any(x => x != 0))
  {
    throw new Exception("HGSS pinyin loader target is not the verified free region.");
  }
  Array.Copy(pinyinLoaderBinary, 0, arm9, loaderOffset, pinyinLoaderBinary.Length);

  var mainSlotAddress = pinyinOverlaySymbols["NameInProcMainSlot"];
  var expectedMain = pinyinOverlaySymbols["NameInProc_Main"] | 1;
  var lazyMain = pinyinLoaderSymbols["NativeNameIn_LazyMain"] | 1;
  var expectedPreHook = pinyinOverlaySymbols["NameInProcMainPreHook"];
  if (lazyMain != expectedPreHook)
  {
    throw new Exception(
      $"HGSS lazy-loader address mismatch: loader=0x{lazyMain:X8}, overlay=0x{expectedPreHook:X8}."
    );
  }
  var mainSlotOffset = checked((int)(mainSlotAddress - ARM9_RAM_ADDRESS));
  if (BitConverter.ToUInt32(arm9, mainSlotOffset) != expectedMain)
  {
    throw new Exception("Unexpected HGSS NameInProc_Main procedure slot.");
  }
  WriteUInt32(arm9, mainSlotOffset, lazyMain);

  var decideHook = pinyinOverlaySymbols["NativeNameIn_DecideMainButton"];
  WriteThumbBl(arm9, 0x02082802, decideHook, new byte[] { 0x01, 0xF0, 0x8F, 0xFD });
  WriteThumbBl(arm9, 0x0208282A, decideHook, new byte[] { 0x01, 0xF0, 0x7B, 0xFD });
  WriteThumbBl(arm9, 0x0208283E, decideHook, new byte[] { 0x01, 0xF0, 0x71, 0xFD });
  WriteThumbBl(arm9, 0x02082850, decideHook, new byte[] { 0x01, 0xF0, 0x68, 0xFD });

  var arenaLoOffset = checked((int)(ARENA_LO_POINTER_ADDRESS - ARM9_RAM_ADDRESS));
  if (BitConverter.ToUInt32(arm9, arenaLoOffset) != PINYIN_OVERLAY_ADDRESS)
  {
    throw new Exception("Unexpected HGSS main-arena low address.");
  }
  var pinyinOverlayEnd = PINYIN_OVERLAY_ADDRESS + pinyinOverlaySymbols["__size__"];
  WriteUInt32(arm9, arenaLoOffset, (pinyinOverlayEnd + 0xF) & ~0xFu);

  SortEasyChatWords(ref arm9, 0x1068f0, easyChatWords.ToArray());

  var arm9New = arm9.Take(0x4000).Concat(BLZ.Compress(arm9.Skip(0x4000).ToArray())).Concat(nitroCode).ToArray();
  uint newSize = (uint)(0x2000000 + arm9New.Length - 12);
  Array.Copy(BitConverter.GetBytes(newSize), 0, arm9New, 0x0BB4, 4);

  File.WriteAllBytes($"out/{gameCode}/arm9.bin", arm9New);
  Console.WriteLine($"Edited: arm9.bin");

  // Edit overlay files
  var overlay9Table = new OverlayTable($"original_files/HGSS/{gameCode}/overarm9.bin", 0, (uint)new FileInfo($"original_files/HGSS/{gameCode}/overarm9.bin").Length, true);
  for (int i = 0; i < overlay9Table.overlayTable.Count; i++)
  {
    if (!Directory.Exists($"asm/HGSS/overlay_{i:D4}")) { continue; }
    var overlay = File.ReadAllBytes($"original_files/HGSS/{gameCode}/overlay/overlay_{i:D4}.bin");
    var overlayItem = overlay9Table.overlayTable[i];
    var compressed = overlay.Length < overlayItem.ramSize + overlayItem.bssSize;
    if (compressed) { overlay = BLZ.Decompress(overlay); }
    var ramAddress = overlayItem.ramAddress;
    foreach (var folder in Directory.EnumerateDirectories($"asm/HGSS/overlay_{i:D4}/"))
    {
      var address = Convert.ToInt32(Path.GetFileName(folder), 16);
      Compile(ref overlay, ref symbols, address, "HGSS", gameCode, $"overlay_{i:D4}", ramAddress);
    }
    if (compressed) { overlay = BLZ.Compress(overlay); }
    File.WriteAllBytes($"out/{gameCode}/overlay/overlay_{i:D4}.bin", overlay);
    Console.WriteLine($"Edited: overlay_{i:D4}.bin");
  }
  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_{PINYIN_OVERLAY_ID:D4}.bin", pinyinOverlayBinary);
  Console.WriteLine($"Added: overlay_{PINYIN_OVERLAY_ID:D4}.bin (pinyin keyboard)");

  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

  // Edit overarm9.bin
  using var overarm9Stream = File.OpenRead($"original_files/HGSS/{gameCode}/overarm9.bin");
  var overarm9 = new OverlayTable(overarm9Stream, 0, (uint)overarm9Stream.Length, true);
  overarm9.overlayTable[74].ramSize = (uint)File.ReadAllBytes($"out/{gameCode}/overlay/overlay_0074.bin").Length;
  using var outputStream = new MemoryStream();
  overarm9.WriteTo(outputStream);
  var overarm9Bytes = outputStream.ToArray()[..(0x20 * overarm9.overlayTable.Count)];
  var pinyinEntryOffset = PINYIN_OVERLAY_ID * 0x20;
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x04, PINYIN_OVERLAY_ADDRESS);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x08, pinyinOverlaySymbols["__ram_size__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x0C, pinyinOverlaySymbols["__bss_size__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x10, pinyinOverlaySymbols["__init_functions_begin__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x14, pinyinOverlaySymbols["__init_functions_end__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x1C, 0);
  File.WriteAllBytes($"out/{gameCode}/overarm9.bin", overarm9Bytes);
  Console.WriteLine($"Edited: overarm9.bin");

  EditBanner("HGSS", gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"original_files/HGSS/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}
