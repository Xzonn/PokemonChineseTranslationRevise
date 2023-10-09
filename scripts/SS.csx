#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.10.0"

var banner = new NitroHelper.Banner("files/SS/banner.bin");
var title = "宝可梦 魂银\nNintendo";
banner.japaneseTitle = title;
banner.englishTitle = title;
banner.frenchTitle = title;
banner.germanTitle = title;
banner.italianTitle = title;
banner.spanishTitle = title;
banner.WriteTo("out/SS/banner.bin");
Console.WriteLine("Banner saved.");
