#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.0"
#r "nuget: Xzonn.BlzHelper, 0.9.0"
#load "lib.csx"
using Xzonn.BlzHelper;

var GAME_CODE_TO_TITLE = new Dictionary<string, string>
{
  ["HG"] = "宝可梦 心金\nNintendo",
  ["SS"] = "宝可梦 魂银\nNintendo",
};

var text = File.ReadAllText("temp/HGSS/messages.txt");
var version = Environment.GetEnvironmentVariable("XZ_PATCH_VERSION");
text = text.Replace("${VERSION}", string.IsNullOrEmpty(version) ? "UNKNOWN" : version[..7]);
File.WriteAllText("temp/HGSS/messages.txt", text);

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
    CompileArm9(ref arm9, address, "HGSS", gameCode, ref symbols);
  }
  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

  SortEasyChatWords(ref arm9, 0x1068f0, easyChatWords.ToArray());

  var arm9New = arm9.Take(0x4000).Concat(BLZ.Compress(arm9.Skip(0x4000).ToArray())).Concat(nitroCode).ToArray();
  uint newSize = (uint)(0x2000000 + arm9New.Length - 12);
  Array.Copy(BitConverter.GetBytes(newSize), 0, arm9New, 0x0BB4, 4);

  File.WriteAllBytes($"out/{gameCode}/arm9.bin", arm9New);
  Console.WriteLine($"Edited: arm9.bin");

  // Decopress and Edit overlay_0001.bin
  var overlay_0001 = BLZ.Decompress(File.ReadAllBytes($"original_files/HGSS/{gameCode}/overlay/overlay_0001.bin"));

  // AP Fix
  EditBinary(ref overlay_0001, 0x000276, "0A E0");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0001.bin", BLZ.Compress(overlay_0001));
  Console.WriteLine($"Edited: overlay_0001.bin");

  // Decopress and Edit overlay_0012.bin
  var overlay_0012 = BLZ.Decompress(File.ReadAllBytes($"original_files/HGSS/{gameCode}/overlay/overlay_0012.bin"));

  // Status condition icons
  EditBinary(ref overlay_0012, 0x0362FC, "FF 88 88 48 82 88 44 44 8F 88 84 84 82 88 44 44 82 88 84 84 82 88 44 44 82 48 88 84 22 88 88 88 88 88 88 88 44 88 48 44 84 88 44 48 44 88 48 48 84 88 44 88 44 88 48 44 84 88 84 48 88 88 88 88 84 88 88 FF 44 84 88 28 44 88 88 F8 44 88 88 28 88 88 88 28 44 84 88 28 48 88 88 28 88 88 88 22 FF CC C4 CC C2 CC C4 CC CF CC CC 44 C2 CC CC 4C C2 CC C4 C4 C2 CC C4 CC C2 CC C4 4C 22 CC CC CC C4 CC 4C CC C4 C4 4C 4C 44 CC CC CC C4 CC CC 4C 44 CC CC CC C4 C4 4C 4C C4 CC 4C CC CC CC CC CC 4C CC CC FF 44 44 CC 2C C4 C4 CC FC 44 44 CC 2C C4 C4 CC 2C CC 44 CC 2C 4C C4 CC 2C CC CC CC 22 FF 33 44 34 32 33 34 34 3F 33 44 44 32 33 34 34 32 33 44 44 32 33 34 34 32 33 44 34 22 33 33 33 44 34 43 34 43 33 43 43 44 34 43 44 44 33 43 43 44 34 43 44 43 33 43 43 44 34 43 34 33 33 33 33 44 44 33 FF 34 43 33 23 44 44 33 F3 34 34 33 23 44 44 33 23 34 34 33 23 44 43 33 23 33 33 33 22 FF DD DD 4D D2 DD 4D 44 DF DD D4 4D D2 DD D4 4D D2 DD 44 44 D2 DD DD 4D D2 DD DD 4D 22 DD DD DD DD DD DD DD 44 D4 DD 44 DD D4 DD DD DD D4 4D 44 44 D4 DD D4 DD DD 4D 44 DD DD DD DD DD DD DD DD D4 DD DD FF 44 D4 DD 2D D4 DD DD FD 44 44 DD 2D D4 D4 DD 2D 44 44 DD 2D DD D4 DD 2D DD DD DD 22 FF AA 4A AA A2 AA 4A A4 AF AA 44 AA A2 AA 4A AA A2 AA 4A AA A2 AA A4 A4 A2 AA A4 AA 22 AA AA AA A4 AA AA A4 44 A4 AA A4 AA A4 4A 44 A4 A4 AA A4 A4 A4 AA A4 AA A4 AA A4 4A AA AA A4 AA AA AA AA A4 AA AA FF 44 44 AA 2A 4A AA AA FA 44 44 AA 2A 4A 4A AA 2A 4A 4A AA 2A A4 44 AA 2A AA AA AA 22");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0012.bin", BLZ.Compress(overlay_0012));
  Console.WriteLine($"Edited: overlay_0012.bin");

  // Decopress and Edit overlay_0065.bin
  var overlay_0065 = BLZ.Decompress(File.ReadAllBytes($"original_files/HGSS/{gameCode}/overlay/overlay_0065.bin"));

  // やめる -> 取消
  EditBinary(ref overlay_0065, 0x003F00, "86 03 54 08 FF FF");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0065.bin", BLZ.Compress(overlay_0065));
  Console.WriteLine($"Edited: overlay_0065.bin");

  // Decopress and Edit overlay_0074.bin
  var overlay_0074 = BLZ.Decompress(File.ReadAllBytes($"original_files/HGSS/{gameCode}/overlay/overlay_0074.bin"));
  var conversion_table_chinese = File.ReadAllBytes("files/gen3_to_gen4_chinese_char/CharTable_3to4.bin");
  var overlay_0074_expand = new byte[overlay_0074.Length + 0x1980 + conversion_table_chinese.Length]
  Array.Copy(overlay_0074, 0, overlay_0074_expand, 0, overlay_0074.Length);
  // chinese from gen3 to gen4
  // conversion table for chinese
  Array.Copy(conversion_table_chinese, 0, overlay_0074_expand, overlay_0074.Length + 0x1980, conversion_table_chinese.Length);
  // Remove language restrictions
  // Ref: https://bbs.oldmantvg.net/thread-31283.htm
  EditBinary(ref overlay_0074_expand, 0x0638, "FF D1");
  // quote trans redirect
  var conversion_table_quote = File.ReadAllBytes("files/gen3_to_gen4_chinese_char/CharTable_3to4_quote.bin");
  Array.Copy(conversion_table_quote, 0, overlay_0074_expand, (uint)(gameCode == "HG" ? 0xFF9C : 0xFFA0), conversion_table_quote.Length);
  // conversion table change for space(0x00) trans
  EditBinary(ref overlay_0074_expand, (uint)(gameCode == "HG" ? 0x010E8E : 0x010E92), "DE 01");
  // chinese trans core code
  var rs_migrate_string = (gameCode == "HG")
      ? File.ReadAllBytes("files/gen3_to_gen4_chinese_char/HG_overlay_0074_0x010010.bin")
      : File.ReadAllBytes("files/gen3_to_gen4_chinese_char/SS_overlay_0074_0x010014.bin");
  Array.Copy(rs_migrate_string, 0, overlay_0074_expand, (uint)(gameCode == "HG" ? 0x010010 : 0x010014), rs_migrate_string.Length);

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0074.bin", BLZ.Compress(overlay_0074_expand));
  Console.WriteLine($"Edited: overlay_0074.bin");

  // Decopress and Edit overlay_0112.bin
  var overlay_0112 = BLZ.Decompress(File.ReadAllBytes($"original_files/HGSS/{gameCode}/overlay/overlay_0112.bin"));

  // RAM Address Offset: 0x021E4E40;
  // https://github.com/enler/HGSS_PokeWalkerChineseLocalization/blob/master/Patch/HG.asm
  EditBinary(ref overlay_0112, 0x002D5C, "00 20");
  EditBinary(ref overlay_0112, 0x002D62, "EC F6 96 EA");
  EditBinary(ref overlay_0112, 0x00678E, "0F F0 B5 F9 C0 46 C0 46 C0 46 C0 46 C0 46");
  var _0x01059E = File.ReadAllBytes("files/HGSS_overlay_0112_0x01059E.bin");
  Array.Copy(_0x01059E, 0, overlay_0112, 0x01059E, _0x01059E.Length);

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0112.bin", BLZ.Compress(overlay_0112));
  Console.WriteLine($"Edited: overlay_0112.bin");

  // Decopress and Edit overlay_0122.bin
  var overlay_0122 = BLZ.Decompress(File.ReadAllBytes($"original_files/HGSS/{gameCode}/overlay/overlay_0122.bin"));

  // AP Fix
  EditBinary(ref overlay_0122, 0x000670, "36 00");
  EditBinary(ref overlay_0122, 0x000778, "36 00");
  EditBinary(ref overlay_0122, 0x000904, "7B 42");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0122.bin", BLZ.Compress(overlay_0122));
  Console.WriteLine($"Edited: overlay_0122.bin");

  EditBanner("HGSS", gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"original_files/HGSS/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}
