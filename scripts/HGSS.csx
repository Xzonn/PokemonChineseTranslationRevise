#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.2"
#r "nuget: Xzonn.BlzHelper, 0.9.0"
#load "lib.csx"

using NitroHelper;
using Xzonn.BlzHelper;

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

  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

  // Edit overarm9.bin
  var overarm9Stream = File.OpenRead($"original_files/HGSS/{gameCode}/overarm9.bin");
  var overarm9 = new OverlayTable(overarm9Stream, 0, (uint)overarm9Stream.Length, true);
  overarm9.overlayTable[74].ramSize = (uint)File.ReadAllBytes($"out/{gameCode}/overlay/overlay_0074.bin").Length;
  overarm9.WriteTo($"out/{gameCode}/overarm9.bin");
  Console.WriteLine($"Edited: overarm9.bin");

  EditBanner("HGSS", gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"original_files/HGSS/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}
