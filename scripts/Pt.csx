#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.10.0"

var banner = new NitroHelper.Banner("files/Pt/banner.bin");
var title = "宝可梦 白金\nNintendo";
banner.japaneseTitle = title;
banner.englishTitle = title;
banner.frenchTitle = title;
banner.germanTitle = title;
banner.italianTitle = title;
banner.spanishTitle = title;
banner.WriteTo("out/Pt/banner.bin");
Console.WriteLine("Banner saved.");

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
