#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.0"
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
  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

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

  // Decopress and Edit overlay_0074.bin
  var overlay_0074 = BLZ.Decompress(File.ReadAllBytes($"original_files/HGSS/{gameCode}/overlay/overlay_0074.bin"));

  // chinese from gen3 to gen4
  // Ref: https://github.com/Wokann/Pokemon_PalParkMigratation_For_GEN34Chinese/blob/main/src/HG/patch.asm
  // Ref: https://github.com/Wokann/Pokemon_PalParkMigratation_For_GEN34Chinese/blob/main/src/SS/patch.asm

  // expand overlay_0074.bin for conversion table chinese
  var conversion_table_chinese = File.ReadAllBytes("files/gen3_to_gen4_chinese_char/CharTable_3to4.bin");
  var overlay_0074_expand = new byte[overlay_0074.Length + 0x1980 + conversion_table_chinese.Length];
  Array.Copy(overlay_0074, 0, overlay_0074_expand, 0, overlay_0074.Length);
  Array.Copy(conversion_table_chinese, 0, overlay_0074_expand, overlay_0074.Length + 0x1980, conversion_table_chinese.Length);
  // Remove language restrictions
  // Ref: https://bbs.oldmantvg.net/thread-31283.htm
  EditBinary(ref overlay_0074_expand, 0x0638, "FF D1");
  // conversion table quote trans redirect
  var conversion_table_quote = File.ReadAllBytes("files/gen3_to_gen4_chinese_char/CharTable_3to4_quote.bin");
  Array.Copy(conversion_table_quote, 0, overlay_0074_expand, (uint)(gameCode == "HG" ? 0xFF9C : 0xFFA0), conversion_table_quote.Length);
  // conversion table change for space(0x00) trans
  EditBinary(ref overlay_0074_expand, (gameCode == "HG" ? 0x010E8E : 0x010E92), "DE 01");
  // chinese trans core code
  var rs_migrate_string = File.ReadAllBytes("files/gen3_to_gen4_chinese_char/rs_migrate_string.bin");
  EditBinary(ref rs_migrate_string, 0xA4, (gameCode == "HG" ? "2C 74 23 02 3C 65 23 02 40 A6 23 02" :"30 74 23 02 40 65 23 02 40 A6 23 02"));
  Array.Copy(rs_migrate_string, 0, overlay_0074_expand, (gameCode == "HG" ? 0x010010 : 0x010014), rs_migrate_string.Length);

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0074.bin", BLZ.Compress(overlay_0074_expand));
  Console.WriteLine($"Edited: overlay_0074.bin");

  // Edit overarm9.bin
  var overarm9 = File.ReadAllBytes($"original_files/HGSS/{gameCode}/overarm9.bin");

  // update ram size and compressed size for overlay_0074 expand
  Array.Copy(BitConverter.GetBytes((uint)overlay_0074_expand.Length), 0, overarm9, 74*0x20+8, 4);
  Array.Copy(BitConverter.GetBytes((uint)(BLZ.Compress(overlay_0074_expand).Length|(1<<24))), 0, overarm9, 74*0x20+0x1C, 4);

  File.WriteAllBytes($"out/{gameCode}/overarm9.bin", overarm9);
  Console.WriteLine($"Edited: overarm9.bin");

  EditBanner("HGSS", gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"original_files/HGSS/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}
