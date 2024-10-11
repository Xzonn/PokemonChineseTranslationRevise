#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.2"
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
    while (true)
    {
      try
      {
        Directory.Delete("asm/DP/build", true);
        break;
      }
      catch { }
    }
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

  // Sort easy chat words
  var aikotobaList = SortEasyChatWords(ref arm9, (uint)(gameCode == "D" ? 0x1001b4 : 0x1001b8), easyChatWords.ToArray());
  if (gameCode == "D") { File.WriteAllLines($"out/Aikotoba-DP.txt", aikotobaList); }

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

  // Edit overarm9.bin
  var overarm9Stream = File.OpenRead($"original_files/DP/{gameCode}/overarm9.bin");
  var overarm9 = new OverlayTable(overarm9Stream, 0, (uint)overarm9Stream.Length, true);
  overarm9.overlayTable[83].ramSize = (uint)File.ReadAllBytes($"out/{gameCode}/overlay/overlay_0083.bin").Length;
  overarm9.WriteTo($"out/{gameCode}/overarm9.bin");
  Console.WriteLine($"Edited: overarm9.bin");

  EditBanner("DP", gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"original_files/DP/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}
