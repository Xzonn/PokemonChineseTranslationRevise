#!/usr/bin/env dotnet-script
#load "lib.csx"

var GAME_CODE_TO_TITLE = new Dictionary<string, string>
{
  ["HG"] = "宝可梦 心金\nNintendo",
  ["SS"] = "宝可梦 魂银\nNintendo",
};

foreach (var gameCode in GAME_CODE_TO_TITLE.Keys)
{
  Directory.CreateDirectory($"out/{gameCode}/data/");
  Directory.CreateDirectory($"out/{gameCode}/overlay/");

  // TODO: Edit arm9.bin and overlays
  // arm9.bin and overlays are compressed in HGSS
  CopyFolder($"files/HGSS/overlay/", $"out/{gameCode}/overlay/");
  CopyFolder($"files/{gameCode}/", $"out/{gameCode}/", "arm9.bin");
  CopyFolder($"files/{gameCode}/overlay/", $"out/{gameCode}/overlay/");

  EditBanner(gameCode, GAME_CODE_TO_TITLE[gameCode]);
}

CopyFolder("textures/DP/battle/graphic/batt_obj.narc/", "textures/HGSS/a/0/0/8/");
File.Delete("textures/HGSS/a/0/0/8/0077.bin");
for (int from = 155; from <= 177; from++)   
{
  MoveFile($"textures/HGSS/a/0/0/8/{from:d04}.bin", $"textures/HGSS/a/0/0/8/{from + 64:d04}.bin");
}

CopyFolder("textures/DP/graphic/pst_gra.narc/", "textures/HGSS/a/0/3/9/");
CopyFolder("textures/DP/graphic/zukan.narc/", "textures/HGSS/a/0/6/7/");

var text = File.ReadAllText("files/HGSS/Messages.txt");
var version = Environment.GetEnvironmentVariable("XZ_PATCH_VERSION");
text = text.Replace("${VERSION}", string.IsNullOrEmpty(version) ? "UNKNOWN" : version[..7]);
File.WriteAllText("out/Messages_HGSS.txt", text);
