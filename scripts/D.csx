#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.10.0"

var banner = new NitroHelper.Banner("files/D/banner.bin");
var title = "宝可梦 钻石\nNintendo";
banner.japaneseTitle = title;
banner.englishTitle = title;
banner.frenchTitle = title;
banner.germanTitle = title;
banner.italianTitle = title;
banner.spanishTitle = title;
banner.WriteTo("out/D/banner.bin");
Console.WriteLine("Banner saved.");
