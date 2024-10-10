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

  EditBanner("Pt", gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"original_files/Pt/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}
