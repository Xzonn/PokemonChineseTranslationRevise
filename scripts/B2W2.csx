#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.0"
#load "lib.csx"

var GAME_CODE_TO_TITLE = new Dictionary<string, string>
{
  ["B2"] = "宝可梦 黑2\nNintendo",
  ["W2"] = "宝可梦 白2\nNintendo",
};

foreach (var gameCode in GAME_CODE_TO_TITLE.Keys)
{
  Directory.CreateDirectory($"out/{gameCode}/data/");
  Directory.CreateDirectory($"out/{gameCode}/overlay/");

  EditBanner(gameCode, GAME_CODE_TO_TITLE[gameCode]);
}