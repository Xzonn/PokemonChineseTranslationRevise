#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.0"
#load "lib.csx"

var GAME_CODE_TO_TITLE = new Dictionary<string, string>
{
  ["Pt"] = "宝可梦 白金\nNintendo",
};
var messages = LoadMessages("Pt");
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
  var arm9 = File.ReadAllBytes($"files/{gameCode}/arm9.bin");

  // Replace `DecompressGlyphTiles_FromPreloaded` with `DecompressGlyphTiles_LazyFromNarc`
  EditBinary(ref arm9, 0x02311C, "19 32 02 02");
  // `TryLoadGlyph()` NOP
  EditBinary(ref arm9, 0x02315A, "C0 46");
  // `GetGlyphWidth_VariableWidth()` Change 01AC `?` to 01FB `　`
  EditBinary(ref arm9, 0x023378, "FA 01");
  // Sort easy chat words
  var easyChatWordsArray = easyChatWords.ToArray();
  var aikotobaList = new List<string>();
  using (var br = new BinaryReader(File.OpenRead($"files/Pt/pms_aikotoba.bin")))
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

  // Edit overlay_0016.bin
  var overlay_0016 = File.ReadAllBytes($"files/{gameCode}/overlay/overlay_0016.bin");

  // Status condition icons
  EditBinary(ref overlay_0016, 0x034A80, "FF 88 88 E8 83 88 EE EE 83 88 8E 8E 82 88 EE EE 82 88 8E 8E 73 88 8E EE 33 88 EE 8E FF E7 77 7E 88 88 88 88 EE 8E E8 EE 8E 88 EE E8 EE 8E E8 E8 EE 88 EE E8 EE 88 E8 EE 8E 8E E8 88 7E 77 7E E7 8E 88 88 FF EE EE 88 38 E8 E8 88 38 EE EE 88 28 E8 E8 88 28 EE EE 88 37 8E 8E 88 33 77 7E 77 FF FF EC CC EC C3 CC CE EC C3 CC EC EE C2 CC CE EE C2 CC CE EE B3 EC EC EC 33 EC CC EC FF BB BB EE CC CC CE CC CC CE EC EE EC CC CC CC CE CC EC EC EC CC EC EC CC CE CE CC CC CE CE EC BB BB BE BE CE CC CC FF EE EE CC 3C CE CC CC 3C EC CC CC 2C EE EE CC 2C EC CC CC 3B EC EC CC 33 EE EB BB FF FF 11 11 11 13 E1 EE E1 13 E1 E1 EE 12 E1 EE E1 12 E1 E1 EE 23 E1 EE E1 33 E1 E1 EE FF E2 EE E2 E1 1E EE 1E 1E 11 1E 1E EE 1E EE 1E E1 11 1E 1E EE 1E EE 1E E1 11 1E 1E EE 1E EE 1E EE 22 22 22 EE EE 11 FF 1E E1 11 31 EE EE 11 31 1E 1E 11 21 EE EE 11 21 1E 1E 11 32 1E 1E 11 33 EE E2 22 FF FF DD DD DE D3 DD DD DE D3 ED EE EE D2 ED DD DE D2 ED DD DE 93 ED EE EE 33 DD DD DE FF 99 99 9E DD DD DD ED DD DD EE EE EE DD DD ED ED DD EE EE ED DD ED ED EE DD EE EE DD DD ED DD 99 99 E9 EE DD DD DD FF EE DE DD 3D DD DD DD 3D EE EE DD 2D DD DE DD 2D EE EE DD 39 DE DE DD 33 EE 9E 99 FF FF AA AE EA A3 AA AE EE A3 EA EE AA A2 EA AE EA A2 AA AE AA 93 AA AE AA 33 EA EA AA 33 E9 E9 99 AA AA AA AE EE AE EA AA AA AE EE EA AA AE EA AE AE AE EA EA AE AE EA AA AA AE EA AA E9 9E E9 E9 AE AA AA FF EE EE AA 3A AA AA AA 3A EA AA AA 2A EE EE AA 2A EA EA AA 39 AE EA AA 33 99 EE 99 33");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0016.bin", overlay_0016);
  Console.WriteLine($"Edited: overlay_0016.bin");

  // Edit overlay_0062.bin
  var overlay_0062 = File.ReadAllBytes($"files/{gameCode}/overlay/overlay_0062.bin");

  // GTS Pokemon name sorting
  EditBinary(ref overlay_0062, 0x01A514, "00 00 25 00 76 00 90 00 CE 00 0B 01 3C 01 60 01 93 01 BD 01 ED 01");
  EditBinary(ref overlay_0062, 0x01A688, "00 00 25 00 76 00 90 00 CE 00 0B 01 3C 01 60 01 93 01 BD 01 ED 01");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0062.bin", overlay_0062);
  Console.WriteLine($"Edited: overlay_0062.bin");

  // Edit overlay_0088.bin
  var overlay_0088 = File.ReadAllBytes($"files/{gameCode}/overlay/overlay_0088.bin");

  // やめる -> 取消
  EditBinary(ref overlay_0088, 0x003CEC, "86 03 54 08 FF FF");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0088.bin", overlay_0088);
  Console.WriteLine($"Edited: overlay_0088.bin");

  // Edit overlay_0094.bin
  var overlay_0094 = File.ReadAllBytes($"files/{gameCode}/overlay/overlay_0094.bin");

  // GTS Pokemon name sorting
  EditBinary(ref overlay_0094, 0x00B7C8, "00 00 25 00 76 00 90 00 CE 00 0B 01 3C 01 60 01 93 01 BD 01 ED 01");

  File.WriteAllBytes($"out/{gameCode}/overlay/overlay_0094.bin", overlay_0094);
  Console.WriteLine($"Edited: overlay_0094.bin");

  EditBanner(gameCode, GAME_CODE_TO_TITLE[gameCode]);

  // Copy md5.txt
  File.Copy($"files/{gameCode}/md5.txt", $"out/{gameCode}/md5.txt", true);
}

CopyFolder("textures/DP/application/zukanlist/zkn_data/zukan_data.narc/", "textures/Pt/application/zukanlist/zkn_data/zukan_data.narc/");
CopyFolder("textures/DP/application/zukanlist/zkn_data/zukan_data.narc/", "textures/Pt/application/zukanlist/zkn_data/zukan_data_gira.narc/");

CopyFolder("textures/DP/battle/graphic/batt_obj.narc/", "textures/Pt/battle/graphic/batt_obj.narc/");
CopyFolder("textures/DP/battle/graphic/batt_obj.narc/", "textures/Pt/battle/graphic/pl_batt_obj.narc/");
File.Delete("textures/Pt/battle/graphic/pl_batt_obj.narc/0077.bin");
for (int from = 155; from <= 177; from++)
{
  MoveFile($"textures/Pt/battle/graphic/pl_batt_obj.narc/{from:d04}.bin", $"textures/Pt/battle/graphic/pl_batt_obj.narc/{from + 64:d04}.bin");
}

CopyFolder("textures/DP/contest/graphic/contest_bg.narc/", "textures/Pt/contest/graphic/contest_bg.narc/");
CopyFolder("textures/DP/contest/graphic/contest_obj.narc/", "textures/Pt/contest/graphic/contest_obj.narc/");

CopyFolder("textures/DP/data/namein.narc/", "textures/Pt/data/namein.narc/");
CopyFolder("textures/DP/data/slot.narc/", "textures/Pt/data/slot.narc/");

CopyFolder("textures/DP/graphic/bag_gra.narc/", "textures/Pt/graphic/bag_gra.narc/");
CopyFolder("textures/DP/graphic/bag_gra.narc/", "textures/Pt/graphic/pl_bag_gra.narc/");
CopyFolder("textures/DP/graphic/box.narc/", "textures/Pt/graphic/box.narc/");
CopyFolder("textures/DP/graphic/mystery.narc/", "textures/Pt/graphic/mystery.narc/");
CopyFolder("textures/DP/graphic/nutmixer.narc/", "textures/Pt/graphic/nutmixer.narc/");
CopyFolder("textures/DP/graphic/pst_gra.narc/", "textures/Pt/graphic/pst_gra.narc/");
CopyFolder("textures/DP/graphic/pst_gra.narc/", "textures/Pt/graphic/pl_pst_gra.narc/");
CopyFolder("textures/DP/graphic/touch_subwindow.narc/", "textures/Pt/graphic/touch_subwindow.narc/");
CopyFolder("textures/DP/graphic/trainer_case.narc/", "textures/Pt/graphic/trainer_case.narc/");
CopyFolder("textures/DP/graphic/zukan.narc/", "textures/Pt/graphic/zukan.narc/");

MoveFile("textures/Pt/graphic/pl_bag_gra.narc/0007.bin", "textures/Pt/graphic/pl_bag_gra.narc/0011.bin");
MoveFile("textures/Pt/graphic/box.narc/0103.bin", "textures/Pt/graphic/box.narc/0127.bin");
MoveFile("textures/Pt/graphic/box.narc/0108.bin", "textures/Pt/graphic/box.narc/0132.bin");
MoveFile("textures/Pt/graphic/trainer_case.narc/0023.bin", "textures/Pt/graphic/trainer_case.narc/0027.bin");

var text = File.ReadAllText("files/Pt/Messages.txt");
var version = Environment.GetEnvironmentVariable("XZ_PATCH_VERSION");
text = text.Replace("${VERSION}", string.IsNullOrEmpty(version) ? "UNKNOWN" : version[..7]);
File.WriteAllText("out/Messages_Pt.txt", text);
