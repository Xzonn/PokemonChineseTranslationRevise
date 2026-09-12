#!/usr/bin/env dotnet-script
#r "nuget: Xzonn.BlzHelper, 0.9.0"
#load "lib.csx"

using NitroHelper;
using Xzonn.BlzHelper;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

const uint ARM9_RAM_ADDRESS = 0x02000000;

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

int RunMake(string arguments, string workingDirectory = ".")
{
  ProcessStartInfo psi = new()
  {
    FileName = "make",
    Arguments = arguments,
    WorkingDirectory = workingDirectory,
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
  return process.ExitCode;
}

void BuildPinyinKeyboard(string gameCode)
{
  var exitCode = RunMake($"-C asm/pinyin_keyboard GAME={gameCode}");
  if (exitCode != 0)
  {
    throw new Exception($"Pinyin keyboard build failed with exit code {exitCode}.");
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

(Dictionary<string, uint> overlaySymbols, Dictionary<string, uint> loaderSymbols,
 byte[] overlayBinary, byte[] loaderBinary) LoadPinyinKeyboard(string overlayName, string loaderName)
{
  var pinyinOverlaySymbols = ReadNmSymbols($"asm/pinyin_keyboard/build/{overlayName}.sym");
  var pinyinLoaderSymbols = ReadNmSymbols($"asm/pinyin_keyboard/build/{loaderName}.sym");
  var pinyinOverlayBinary = File.ReadAllBytes($"asm/pinyin_keyboard/build/{overlayName}.bin");
  var pinyinLoaderBinary = File.ReadAllBytes($"asm/pinyin_keyboard/build/{loaderName}.bin");
  if (pinyinOverlayBinary.Length != pinyinOverlaySymbols["__ram_size__"])
  {
    throw new Exception("Pinyin overlay file/RAM size mismatch.");
  }
  return (pinyinOverlaySymbols, pinyinLoaderSymbols, pinyinOverlayBinary, pinyinLoaderBinary);
}

void PrepareBuild(string parentGame, string gameCode)
{
  Directory.CreateDirectory($"out/{gameCode}/data/");
  Directory.CreateDirectory($"out/{gameCode}/overlay/");
  if (Directory.Exists($"asm/{parentGame}/build"))
  {
    while (true)
    {
      try
      {
        Directory.Delete($"asm/{parentGame}/build", true);
        break;
      }
      catch { }
    }
  }
}

void CompileArm9(ref byte[] arm9, ref Dictionary<string, string> symbols, string parentGame, string gameCode)
{
  foreach (var folder in Directory.EnumerateDirectories($"asm/{parentGame}/replSource/"))
  {
    int address = Convert.ToInt32(Path.GetFileName(folder), 16);
    Compile(ref arm9, ref symbols, address, parentGame, gameCode);
  }
}

void CompileOverlays(ref Dictionary<string, string> symbols, string parentGame, string gameCode, bool compressedOverlays = false)
{
  var overlay9Table = new OverlayTable($"original_files/{parentGame}/{gameCode}/overarm9.bin", 0, (uint)new FileInfo($"original_files/{parentGame}/{gameCode}/overarm9.bin").Length, true);
  for (int i = 0; i < overlay9Table.overlayTable.Count; i++)
  {
    if (!Directory.Exists($"asm/{parentGame}/overlay_{i:D4}")) { continue; }
    var overlay = File.ReadAllBytes($"original_files/{parentGame}/{gameCode}/overlay/overlay_{i:D4}.bin");
    var overlayItem = overlay9Table.overlayTable[i];
    var compressed = compressedOverlays && overlay.Length < overlayItem.ramSize + overlayItem.bssSize;
    if (compressed) { overlay = BLZ.Decompress(overlay); }
    var ramAddress = overlayItem.ramAddress;
    foreach (var folder in Directory.EnumerateDirectories($"asm/{parentGame}/overlay_{i:D4}/"))
    {
      var address = Convert.ToInt32(Path.GetFileName(folder), 16);
      Compile(ref overlay, ref symbols, address, parentGame, gameCode, $"overlay_{i:D4}", ramAddress);
    }
    if (compressed) { overlay = BLZ.Compress(overlay); }
    File.WriteAllBytes($"out/{gameCode}/overlay/overlay_{i:D4}.bin", overlay);
    Console.WriteLine($"Edited: overlay_{i:D4}.bin");
  }
}

void WritePinyinOverlay(string gameCode, int overlayId, byte[] pinyinOverlayBinary)
{
  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_{overlayId:D4}.bin", pinyinOverlayBinary);
  Console.WriteLine($"Added: overlay_{overlayId:D4}.bin (pinyin keyboard)");
}

byte[] ReadOverlayTable(string parentGame, string gameCode, int expandedOverlayId)
{
  using var overarm9Stream = File.OpenRead($"original_files/{parentGame}/{gameCode}/overarm9.bin");
  var overarm9 = new OverlayTable(overarm9Stream, 0, (uint)overarm9Stream.Length, true);
  overarm9.overlayTable[expandedOverlayId].ramSize =
    (uint)File.ReadAllBytes($"out/{gameCode}/overlay/overlay_{expandedOverlayId:D4}.bin").Length;
  using var outputStream = new MemoryStream();
  overarm9.WriteTo(outputStream);
  return outputStream.ToArray()[..(0x20 * overarm9.overlayTable.Count)];
}

void WritePinyinOverlayTable(byte[] overarm9Bytes, int overlayId, uint overlayAddress,
    Dictionary<string, uint> pinyinOverlaySymbols)
{
  var pinyinEntryOffset = overlayId * 0x20;
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x04, overlayAddress);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x08, pinyinOverlaySymbols["__ram_size__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x0C, pinyinOverlaySymbols["__bss_size__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x10, pinyinOverlaySymbols["__init_functions_begin__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x14, pinyinOverlaySymbols["__init_functions_end__"]);
  WriteUInt32(overarm9Bytes, pinyinEntryOffset + 0x1C, 0);
}

void InstallPinyinLoader(byte[] arm9, uint loaderAddress, byte[] pinyinLoaderBinary)
{
  var loaderOffset = checked((int)(loaderAddress - ARM9_RAM_ADDRESS));
  if (pinyinLoaderBinary.Length > 0x12C ||
      arm9.Skip(loaderOffset).Take(pinyinLoaderBinary.Length).Any(x => x != 0))
  {
    throw new Exception("Pinyin loader target is not the verified free region.");
  }
  Array.Copy(pinyinLoaderBinary, 0, arm9, loaderOffset, pinyinLoaderBinary.Length);
}

void InstallPinyinMainHook(byte[] arm9, Dictionary<string, uint> pinyinOverlaySymbols,
    Dictionary<string, uint> pinyinLoaderSymbols)
{
  var mainSlotAddress = pinyinOverlaySymbols["NameInProcMainSlot"];
  var expectedMain = pinyinOverlaySymbols["NameInProc_Main"] | 1;
  var lazyMain = pinyinLoaderSymbols["NativeNameIn_LazyMain"] | 1;
  var expectedPreHook = pinyinOverlaySymbols["NameInProcMainPreHook"];
  if (lazyMain != expectedPreHook)
  {
    throw new Exception(
      $"lazy-loader address mismatch: loader=0x{lazyMain:X8}, overlay=0x{expectedPreHook:X8}."
    );
  }
  var mainSlotOffset = checked((int)(mainSlotAddress - ARM9_RAM_ADDRESS));
  if (BitConverter.ToUInt32(arm9, mainSlotOffset) != expectedMain)
  {
    throw new Exception("Unexpected NameInProc_Main procedure slot.");
  }
  WriteUInt32(arm9, mainSlotOffset, lazyMain);
}

void ReservePinyinArena(byte[] arm9, uint arenaLoPointerAddress, uint expectedArenaLo,
    uint overlayAddress, Dictionary<string, uint> pinyinOverlaySymbols)
{
  var arenaLoOffset = checked((int)(arenaLoPointerAddress - ARM9_RAM_ADDRESS));
  if (BitConverter.ToUInt32(arm9, arenaLoOffset) != expectedArenaLo)
  {
    throw new Exception("Unexpected main-arena low address.");
  }
  var pinyinOverlayEnd = overlayAddress + pinyinOverlaySymbols["__size__"];
  WriteUInt32(arm9, arenaLoOffset, (pinyinOverlayEnd + 0xF) & ~0xFu);
}
