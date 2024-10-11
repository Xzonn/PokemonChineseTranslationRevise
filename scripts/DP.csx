#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.0"
#load "lib.csx"

using NitroHelper;

var GAME_CODE_TO_TITLE = new Dictionary<string, string>
{
  ["D"] = "宝可梦 钻石\nNintendo",
  ["P"] = "宝可梦 珍珠\nNintendo",
};

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
  if (Directory.Exists("asm/DP/build"))
  {
    Directory.Delete("asm/DP/build", true);
  }
  foreach (var path in Directory.EnumerateFiles("asm/DP", "repl_*"))
  {
    File.Delete(path);
  }

  // Edit arm9.bin
  var arm9 = File.ReadAllBytes($"original_files/DP/{gameCode}/arm9.bin");
  Dictionary<string, string> symbols = new();

  foreach (var folder in Directory.EnumerateDirectories("asm/DP/replSource/"))
  {
    var address = Convert.ToInt32(Path.GetFileName(folder), 16);
    Compile(ref arm9, ref symbols, address, "DP", gameCode);
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

  // Edit overlay files
  var overlay9Table = new OverlayTable($"original_files/DP/{gameCode}/overarm9.bin", 0, (uint)new FileInfo($"original_files/DP/{gameCode}/overarm9.bin").Length, true);
  for (int i = 0; i < overlay9Table.overlayTable.Count; i++)
  {
    if (!Directory.Exists($"asm/DP/overlay_{i:D4}")) { continue; }
    var overlay = File.ReadAllBytes($"original_files/DP/{gameCode}/overlay/overlay_{i:D4}.bin");
    var ramAddress = overlay9Table.overlayTable[i].ramAddress;
    foreach (var folder in Directory.EnumerateDirectories($"asm/DP/overlay_{i:D4}/"))
    {
      var address = Convert.ToInt32(Path.GetFileName(folder), 16);
      Compile(ref overlay, ref symbols, address, "DP", gameCode, $"overlay_{i:D4}", ramAddress);
    }
    File.WriteAllBytes($"out/{gameCode}/overlay/overlay_{i:D4}.bin", overlay);
    Console.WriteLine($"Edited: overlay_{i:D4}.bin");
  }

  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

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
