#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.2"
#load "build_common.csx"

using NitroHelper;
using Xzonn.BlzHelper;

const uint PINYIN_OVERLAY_ADDRESS = 0x0226E140;
const int PINYIN_OVERLAY_ID = 11;
const uint PINYIN_LOADER_ADDRESS = 0x021082C0;
const uint ARENA_LO_POINTER_ADDRESS = 0x020D24D4;

BuildPinyinKeyboard("HGSS");
var (pinyinOverlaySymbols, pinyinLoaderSymbols, pinyinOverlayBinary, pinyinLoaderBinary) =
  LoadPinyinKeyboard("hgss_overlay_0011", "hgss_overlay_ldr");

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
  PrepareBuild("HGSS", gameCode);

  // Build import.o
  RunMake("TARGET=import SOURCES=import", "asm/HGSS");

  // Decopress and edit arm9.bin
  var arm9Comp = File.ReadAllBytes($"original_files/HGSS/{gameCode}/arm9.bin");
  var nitroCode = arm9Comp.Skip(arm9Comp.Length - 12).ToArray();
  var arm9 = BLZ.Decompress(arm9Comp.Take(arm9Comp.Length - 12).ToArray());
  Dictionary<string, string> symbols = new();

  CompileArm9(ref arm9, ref symbols, "HGSS", gameCode);

  // Install a lazy loader in verified ARM9 padding. The first name-input
  // frame loads the persistent pinyin overlay, then dispatches to its hook.
  InstallPinyinLoader(arm9, PINYIN_LOADER_ADDRESS, pinyinLoaderBinary);

  InstallPinyinMainHook(arm9, pinyinOverlaySymbols, pinyinLoaderSymbols);

  var decideHook = pinyinOverlaySymbols["NativeNameIn_DecideMainButton"];
  WriteThumbBl(arm9, 0x02082802, decideHook, new byte[] { 0x01, 0xF0, 0x8F, 0xFD });
  WriteThumbBl(arm9, 0x0208282A, decideHook, new byte[] { 0x01, 0xF0, 0x7B, 0xFD });
  WriteThumbBl(arm9, 0x0208283E, decideHook, new byte[] { 0x01, 0xF0, 0x71, 0xFD });
  WriteThumbBl(arm9, 0x02082850, decideHook, new byte[] { 0x01, 0xF0, 0x68, 0xFD });

  ReservePinyinArena(arm9, ARENA_LO_POINTER_ADDRESS, PINYIN_OVERLAY_ADDRESS,
    PINYIN_OVERLAY_ADDRESS, pinyinOverlaySymbols);

  SortEasyChatWords(ref arm9, 0x1068f0, easyChatWords.ToArray());

  var arm9New = arm9.Take(0x4000).Concat(BLZ.Compress(arm9.Skip(0x4000).ToArray())).Concat(nitroCode).ToArray();
  uint newSize = (uint)(0x2000000 + arm9New.Length - 12);
  Array.Copy(BitConverter.GetBytes(newSize), 0, arm9New, 0x0BB4, 4);

  File.WriteAllBytes($"out/{gameCode}/arm9.bin", arm9New);
  Console.WriteLine($"Edited: arm9.bin");

  // Edit overlay files
  CompileOverlays(ref symbols, "HGSS", gameCode, true);
  WritePinyinOverlay(gameCode, PINYIN_OVERLAY_ID, pinyinOverlayBinary);

  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

  // Edit overarm9.bin
  var overarm9Bytes = ReadOverlayTable("HGSS", gameCode, 74);
  WritePinyinOverlayTable(overarm9Bytes, PINYIN_OVERLAY_ID, PINYIN_OVERLAY_ADDRESS, pinyinOverlaySymbols);
  File.WriteAllBytes($"out/{gameCode}/overarm9.bin", overarm9Bytes);
  Console.WriteLine($"Edited: overarm9.bin");

  EditBanner("HGSS", gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"original_files/HGSS/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}
