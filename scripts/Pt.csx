#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.0"
#load "lib.csx"

using NitroHelper;

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

  // Edit arm9.bin
  var arm9 = File.ReadAllBytes($"original_files/Pt/{gameCode}/arm9.bin");
  Dictionary<string, string> symbols = new();

  foreach (var folder in Directory.EnumerateDirectories("asm/Pt/replSource/"))
  {
    int address = Convert.ToInt32(Path.GetFileName(folder), 16);
    Compile(ref arm9, ref symbols, address, "Pt", gameCode);
  }
  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

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

  File.WriteAllText($"out/{gameCode}/symbols.txt", string.Join('\n', symbols.Select(x => $"{x.Key} = 0x{x.Value};")));

  // Edit overlay_0097.bin
  var overlay_0097 = File.ReadAllBytes($"original_files/Pt/{gameCode}/overlay/overlay_0097.bin");

  // chinese from gen3 to gen4
  // Ref: https://github.com/Wokann/Pokemon_PalParkMigratation_For_GEN34Chinese/blob/main/src/Pt/patch.asm

  // expand overlay_0097.bin for conversion table chinese
  var conversion_table_chinese = File.ReadAllBytes("files/gen3_to_gen4_chinese_char/CharTable_3to4.bin");
  var overlay_0097_expand = new byte[overlay_0097.Length + 0x1980 + conversion_table_chinese.Length];
  Array.Copy(overlay_0097, 0, overlay_0097_expand, 0, overlay_0097.Length);
  Array.Copy(conversion_table_chinese, 0, overlay_0097_expand, overlay_0097.Length + 0x1980, conversion_table_chinese.Length);
  // Remove language restrictions
  // Ref: https://bbs.oldmantvg.net/thread-31283.htm
  EditBinary(ref overlay_0097_expand, 0x0118, "FF D1");
  // Remove 24 hour restrictions
  EditBinary(ref overlay_0097_expand, 0xA6CC, "1E E0");
  // conversion table quote trans redirect
  var conversion_table_quote = File.ReadAllBytes("files/gen3_to_gen4_chinese_char/CharTable_3to4_quote.bin");
  Array.Copy(conversion_table_quote, 0, overlay_0097_expand, 0xE588, conversion_table_quote.Length);
  // conversion table change for space(0x00) trans
  EditBinary(ref overlay_0097_expand, 0xF44E, "DE 01");
  // chinese trans core code
  var rs_migrate_string = File.ReadAllBytes("files/gen3_to_gen4_chinese_char/rs_migrate_string.bin");
  EditBinary(ref rs_migrate_string, 0xA4, "AC 96 23 02 E8 87 23 02 40 C6 23 02");
  Array.Copy(rs_migrate_string, 0, overlay_0097_expand, 0xE5FC, rs_migrate_string.Length);

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0097.bin", overlay_0097_expand);
  Console.WriteLine($"Edited: overlay_0097.bin");

  // Edit overarm9.bin
  var overarm9 = File.ReadAllBytes($"original_files/Pt/{gameCode}/overarm9.bin");

  // update ram size for overlay_0097 expand
  Array.Copy(BitConverter.GetBytes((uint)overlay_0097_expand.Length), 0, overarm9, 97*0x20+8, 4);

  File.WriteAllBytes($"out/{gameCode}/overarm9.bin", overarm9);
  Console.WriteLine($"Edited: overarm9.bin");

  EditBanner("Pt", gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"original_files/Pt/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}
