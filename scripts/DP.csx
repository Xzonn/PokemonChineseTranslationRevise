#!/usr/bin/env dotnet-script
#load "lib.csx"

var GAME_CODE_TO_TITLE = new Dictionary<string, string>
{
  ["D"] = "宝可梦 钻石\nNintendo",
  ["P"] = "宝可梦 珍珠\nNintendo",
};

foreach (var gameCode in GAME_CODE_TO_TITLE.Keys)
{
  Directory.CreateDirectory($"out/{gameCode}/data/");
  Directory.CreateDirectory($"out/{gameCode}/overlay/");

  // Edit arm9.bin
  var arm9 = File.ReadAllBytes($"files/{gameCode}/arm9.bin");

  // Replace `DecompressGlyphTiles_FromPreloaded()` with `GetGlyphWidth_VariableWidth()`, from Pt and HGSS
  EditBinary(ref arm9, 0x0228C4, "42 6E 91 42 02 D2 40 6F 40 5C 70 47 41 6F 01 48 08 5C 70 47 FB 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00");
  // `TryLoadGlyph()` NOP
  EditBinary(ref arm9, 0x022974, "C0 46");
  // Replace `DecompressGlyphTiles_FromPreloaded` with `DecompressGlyphTiles_LazyFromNarc`
  EditBinary(ref arm9, 0x0229FC, "05 28 02 02");
  // Change offset
  EditBinary(ref arm9, 0x022AE8, "C5 28 02 02");

  File.WriteAllBytes($"out/{gameCode}/arm9.bin", arm9);
  Console.WriteLine($"Edited: arm9.bin");

  // Edit overlay_0011.bin
  var overlay_0011 = File.ReadAllBytes($"files/{gameCode}/overlay/overlay_0011.bin");

  // Status condition icons
  EditBinary(ref overlay_0011, 0x0342CC, "1F 33 33 3F 31 F3 FF FF 33 F3 33 33 33 FF FF FF 33 F3 33 3F 31 FF FF FF 1F F3 3F 3F FF 1F FF F1 33 33 33 F3 FF 33 FF FF 33 33 33 F3 FF 33 FF FF 33 33 F3 F3 FF 33 FF FF F3 33 F3 33 F1 11 F1 FF 33 33 F1 FF FF 3F 13 FF 33 33 33 FF FF FF 33 FF 33 3F 33 FF FF FF 13 FF 3F 3F F1 FF FF 1F FF FF 1F 88 F8 88 81 FF FF FF 88 8F 8F 8F 88 FF FF FF 88 8F 8F FF 81 8F FF FF 1F FF 8F 8F FF 11 1F 1F 88 88 88 8F 8F F8 FF FF 88 FF F8 F8 8F F8 F8 FF 88 FF F8 F8 88 F8 FF FF 8F F8 88 8F 11 1F F1 11 88 F1 FF FF FF 18 FF FF F8 88 FF FF FF 88 FF FF F8 88 FF FF FF 18 FF FF 8F F1 FF FF 1F FF FF FF FF 00 F0 00 01 0F F0 00 00 F0 FF F0 00 0F FF 0F 00 0F FF F0 F1 F0 F0 00 FF 00 F0 00 FF 11 FF 11 00 0F 00 0F 0F F0 FF FF 00 00 00 0F 00 F0 F0 F0 00 F0 F0 FF 0F 0F 00 F0 0F 0F F0 F0 11 1F 1F FF 00 F1 FF FF FF 10 FF FF 00 00 FF FF 00 00 FF FF FF 00 FF FF 00 10 FF FF F0 F1 FF FF F1 FF FF FF 1F 33 33 F3 F1 FF F3 3F F3 F3 FF FF F3 FF F3 F3 F3 F3 FF FF F1 FF F3 F3 FF F3 FF FF FF FF F1 FF 3F FF 3F FF 33 3F 3F 3F 3F FF 3F FF 33 3F 3F 3F 3F FF 3F FF 33 3F 3F 3F 3F FF 3F 3F 11 11 11 FF FF F1 FF FF F3 13 FF FF FF 33 FF FF 3F 33 FF FF FF 33 FF FF 3F 13 FF FF 3F F1 FF FF F1 FF FF FF 1F DD FD DD D1 DD FD DD DD FF FF FF DD DF FD DD DD DF FD DD D1 FF FF FF 1F DD FD DD FF 11 F1 11 DD DD FD DD DD FF FF FF DF DD FD DD DF FF FF FF DF FD FD DD DF FF FF FF DD FD DD DF 11 F1 FF FF DD F1 FF FF DF 1D FF FF DD DD FF FF FF DD FF FF DF DD FF FF FF 1D FF FF DF F1 FF FF 1F FF FF FF 1F AF FA AA A1 AF FF FF FA FF AA AA FA AF FA AA AA AF AA AF A1 AF AA AF FF FA AA AA FF F1 11 F1 AA AA AF AF AF FA AA FF AF FF FA AA AF FA AF FA AF FA FA FF AF FA AA FA AF FA AA AF 1F F1 F1 11 AA F1 FF FF FF 1A FF FF AA AA FF FF AA AA FF FF FF AA FF FF FA 1A FF FF FA F1 FF FF 11 FF FF FF");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0011.bin", overlay_0011);
  Console.WriteLine($"Edited: overlay_0011.bin");

  // Edit overlay_0071.bin
  var overlay_0071 = File.ReadAllBytes($"files/{gameCode}/overlay/overlay_0071.bin");

  // やめる -> 取消
  EditBinary(ref overlay_0071, 0x003D54, "86 03 54 08 FF FF");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0071.bin", overlay_0071);
  Console.WriteLine($"Edited: overlay_0071.bin");

  EditBanner(gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"files/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}

var text = File.ReadAllText("files/DP/Messages.txt");
var version = Environment.GetEnvironmentVariable("XZ_PATCH_VERSION");
text = text.Replace("${VERSION}", string.IsNullOrEmpty(version) ? "UNKNOWN" : version[..7]);
File.WriteAllText("out/Messages_DP.txt", text);
