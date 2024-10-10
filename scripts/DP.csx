#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.0"
#load "lib.csx"

var GAME_CODE_TO_TITLE = new Dictionary<string, string>
{
  ["D"] = "宝可梦 钻石\nNintendo",
  ["P"] = "宝可梦 珍珠\nNintendo",
};

var text = File.ReadAllText("temp/DP/messages.txt");
var version = Environment.GetEnvironmentVariable("XZ_PATCH_VERSION");
text = text.Replace("${VERSION}", string.IsNullOrEmpty(version) ? "UNKNOWN" : version[..7]);
File.WriteAllText("temp/DP/messages.txt", text);

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
  Directory.CreateDirectory($"out/{gameCode}/data/");
  Directory.CreateDirectory($"out/{gameCode}/overlay/");

  // Edit arm9.bin
  var arm9 = File.ReadAllBytes($"original_files/DP/{gameCode}/arm9.bin");
  Dictionary<string, string> symbols = new();

  foreach (var folder in Directory.EnumerateDirectories("asm/DP/replSource/"))
  {
    int address = Convert.ToInt32(Path.GetFileName(folder), 16);
    CompileArm9(ref arm9, address, "DP", gameCode, ref symbols);
  }
  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

  // Sort easy chat words
  var aikotobaList = SortEasyChatWords(ref arm9, (uint)(gameCode == "D" ? 0x1001b4 : 0x1001b8), easyChatWords.ToArray());
  if (gameCode == "D") { File.WriteAllLines($"out/Aikotoba-DP.txt", aikotobaList); }

  // chinese from gen3 to gen4
  // Ref: https://github.com/Wokann/Pokemon_PalParkMigratation_For_GEN34Chinese/blob/main/src/D/patch.asm
  // Ref: https://github.com/Wokann/Pokemon_PalParkMigratation_For_GEN34Chinese/blob/main/src/P/patch.asm
  // conversion table quote trans redirect
  var conversion_table_quote = File.ReadAllBytes("files/gen3_to_gen4_chinese_char/CharTable_3to4_quote.bin");
  Array.Copy(conversion_table_quote, 0, arm9, 0x016574, conversion_table_quote.Length);
  // conversion table change for space(0x00) trans
  EditBinary(ref arm9, (gameCode == "D" ? 0x0EF7D6 : 0x0EF7DA), "DE 01");
  // chinese trans core code
  var rs_migrate_string = File.ReadAllBytes("files/gen3_to_gen4_chinese_char/rs_migrate_string.bin");
  EditBinary(ref rs_migrate_string, 0xA4, (gameCode == "D" ? "D4 F7 0E 02 74 65 01 02 00 06 24 02" : "D8 F7 0E 02 74 65 01 02 00 06 24 02"));
  Array.Copy(rs_migrate_string, 0, arm9, 0x0164C0, rs_migrate_string.Length);

  File.WriteAllBytes($"out/{gameCode}/arm9.bin", arm9);
  Console.WriteLine($"Edited: arm9.bin");

  // Edit overlay_0011.bin
  var overlay_0011 = File.ReadAllBytes($"original_files/DP/{gameCode}/overlay/overlay_0011.bin");

  // Status condition icons
  EditBinary(ref overlay_0011, 0x0342CC, "1F 33 33 3F 31 F3 FF FF 33 F3 33 33 33 FF FF FF 33 F3 33 3F 31 FF FF FF 1F F3 3F 3F FF 1F FF F1 33 33 33 F3 FF 33 FF FF 33 33 33 F3 FF 33 FF FF 33 33 F3 F3 FF 33 FF FF F3 33 F3 33 F1 11 F1 FF 33 33 F1 FF FF 3F 13 FF 33 33 33 FF FF FF 33 FF 33 3F 33 FF FF FF 13 FF 3F 3F F1 FF FF 1F FF FF 1F 88 F8 88 81 FF FF FF 88 8F 8F 8F 88 FF FF FF 88 8F 8F FF 81 8F FF FF 1F FF 8F 8F FF 11 1F 1F 88 88 88 8F 8F F8 FF FF 88 FF F8 F8 8F F8 F8 FF 88 FF F8 F8 88 F8 FF FF 8F F8 88 8F 11 1F F1 11 88 F1 FF FF FF 18 FF FF F8 88 FF FF FF 88 FF FF F8 88 FF FF FF 18 FF FF 8F F1 FF FF 1F FF FF FF FF 00 F0 00 01 0F F0 00 00 F0 FF F0 00 0F FF 0F 00 0F FF F0 F1 F0 F0 00 FF 00 F0 00 FF 11 FF 11 00 0F 00 0F 0F F0 FF FF 00 00 00 0F 00 F0 F0 F0 00 F0 F0 FF 0F 0F 00 F0 0F 0F F0 F0 11 1F 1F FF 00 F1 FF FF FF 10 FF FF 00 00 FF FF 00 00 FF FF FF 00 FF FF 00 10 FF FF F0 F1 FF FF F1 FF FF FF 1F 33 33 F3 F1 FF F3 3F F3 F3 FF FF F3 FF F3 F3 F3 F3 FF FF F1 FF F3 F3 FF F3 FF FF FF FF F1 FF 3F FF 3F FF 33 3F 3F 3F 3F FF 3F FF 33 3F 3F 3F 3F FF 3F FF 33 3F 3F 3F 3F FF 3F 3F 11 11 11 FF FF F1 FF FF F3 13 FF FF FF 33 FF FF 3F 33 FF FF FF 33 FF FF 3F 13 FF FF 3F F1 FF FF F1 FF FF FF 1F DD FD DD D1 DD FD DD DD FF FF FF DD DF FD DD DD DF FD DD D1 FF FF FF 1F DD FD DD FF 11 F1 11 DD DD FD DD DD FF FF FF DF DD FD DD DF FF FF FF DF FD FD DD DF FF FF FF DD FD DD DF 11 F1 FF FF DD F1 FF FF DF 1D FF FF DD DD FF FF FF DD FF FF DF DD FF FF FF 1D FF FF DF F1 FF FF 1F FF FF FF 1F AF FA AA A1 AF FF FF FA FF AA AA FA AF FA AA AA AF AA AF A1 AF AA AF FF FA AA AA FF F1 11 F1 AA AA AF AF AF FA AA FF AF FF FA AA AF FA AF FA AF FA FA FF AF FA AA FA AF FA AA AF 1F F1 F1 11 AA F1 FF FF FF 1A FF FF AA AA FF FF AA AA FF FF FF AA FF FF FA 1A FF FF FA F1 FF FF 11 FF FF FF");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0011.bin", overlay_0011);
  Console.WriteLine($"Edited: overlay_0011.bin");

  // Edit overlay_0071.bin
  var overlay_0071 = File.ReadAllBytes($"original_files/DP/{gameCode}/overlay/overlay_0071.bin");

  // やめる -> 取消
  EditBinary(ref overlay_0071, 0x003D54, "86 03 54 08 FF FF");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0071.bin", overlay_0071);
  Console.WriteLine($"Edited: overlay_0071.bin");

  // Edit overlay_0080.bin
  var overlay_0080 = File.ReadAllBytes($"original_files/DP/{gameCode}/overlay/overlay_0080.bin");

  // GTS Pokemon name sorting
  EditBinary(ref overlay_0080, 0x00A9D0, "00 00 25 00 76 00 90 00 CE 00 0B 01 3C 01 60 01 93 01 BD 01 ED 01");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0080.bin", overlay_0080);
  Console.WriteLine($"Edited: overlay_0080.bin");

  // Edit overlay_0083.bin
  var overlay_0083 = File.ReadAllBytes($"original_files/DP/{gameCode}/overlay/overlay_0083.bin");

  // chinese from gen3 to gen4
  // Ref: https://github.com/Wokann/Pokemon_PalParkMigratation_For_GEN34Chinese/blob/main/src/D/patch.asm
  // Ref: https://github.com/Wokann/Pokemon_PalParkMigratation_For_GEN34Chinese/blob/main/src/P/patch.asm

  // expand overlay_0083.bin for conversion table chinese
  var conversion_table_chinese = File.ReadAllBytes("files/gen3_to_gen4_chinese_char/CharTable_3to4.bin");
  var overlay_0083_expand = new byte[overlay_0083.Length + 0x1980 + conversion_table_chinese.Length];
  Array.Copy(overlay_0083, 0, overlay_0083_expand, 0, overlay_0083.Length);
  Array.Copy(conversion_table_chinese, 0, overlay_0083_expand, overlay_0083.Length + 0x1980, conversion_table_chinese.Length);
  // Remove language restrictions
  // Ref: https://bbs.oldmantvg.net/thread-31283.htm
  EditBinary(ref overlay_0083_expand, 0x00129A, "FF D1");
  // Remove 24 hour restrictions
  EditBinary(ref overlay_0083_expand, 0x00839C, "1E E0");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0083.bin", overlay_0083_expand);
  Console.WriteLine($"Edited: overlay_0083.bin");

  // Edit overarm9.bin
  var overarm9 = File.ReadAllBytes($"original_files/DP/{gameCode}/overarm9.bin");

  // update ram size for overlay_0083 expand
  Array.Copy(BitConverter.GetBytes((uint)overlay_0083_expand.Length), 0, overarm9, 83*0x20+8, 4);

  File.WriteAllBytes($"out/{gameCode}/overarm9.bin", overarm9);
  Console.WriteLine($"Edited: overarm9.bin");

  EditBanner("DP", gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"original_files/DP/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}
