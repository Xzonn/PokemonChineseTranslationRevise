#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.10.0"

var banner = new NitroHelper.Banner("files/HG/banner.bin");
var title = "宝可梦 心金\nNintendo";
banner.japaneseTitle = title;
banner.englishTitle = title;
banner.frenchTitle = title;
banner.germanTitle = title;
banner.italianTitle = title;
banner.spanishTitle = title;
banner.WriteTo("out/HG/banner.bin");
Console.WriteLine("Banner saved.");

CopyFolder("textures/DP/battle/graphic/batt_obj.narc/", "textures/HGSS/a/0/0/8/");
File.Delete("textures/HGSS/a/0/0/8/0077.bin");
for (int from = 155; from <= 177; from++)   
{
  MoveFile($"textures/HGSS/a/0/0/8/{from:d04}.bin", $"textures/HGSS/a/0/0/8/{from + 64:d04}.bin");
}

CopyFolder("textures/DP/graphic/pst_gra.narc/", "textures/HGSS/a/0/3/9/");
CopyFolder("textures/DP/graphic/zukan.narc/", "textures/HGSS/a/0/6/7/");

void CopyFolder(string from, string to) {
  if (!Directory.Exists(to))
  {
    Directory.CreateDirectory(to);
  }
  foreach (var file in Directory.EnumerateFiles(from))
  {
    File.Copy(file, Path.Combine(to, Path.GetFileName(file)), true);
  }
  Console.WriteLine($"Copied: {from} -> {to}");
}

void MoveFile(string from, string to)
{
  if (File.Exists(to))
  {
    File.Delete(to);
  }
  if (!File.Exists(from))
  {
    return;
  }
  File.Move(from, to);
  Console.WriteLine($"Moved: {from} -> {to}");
}
