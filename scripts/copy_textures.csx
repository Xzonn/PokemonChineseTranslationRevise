#!/usr/bin/env dotnet-script
#load "lib.csx"

// DP
CopyFile("files/utility.bin", "out/D/data/data/utility.bin");
CopyFile("files/utility.bin", "out/D/data/dwc/utility.bin");

// Pt
CopyFile("files/utility.bin", "out/Pt/data/data/utility.bin");
CopyFile("files/utility.bin", "out/Pt/data/dwc/utility.bin");

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

// HGSS
CopyFile("files/utility.bin", "out/HG/data/data/utility.bin");
CopyFile("files/utility.bin", "out/HG/data/dwc/utility.bin");

CopyFolder("textures/DP/battle/graphic/batt_obj.narc/", "textures/HGSS/a/0/0/8/");
File.Delete("textures/HGSS/a/0/0/8/0077.bin");
for (int from = 155; from <= 177; from++)
{
  MoveFile($"textures/HGSS/a/0/0/8/{from:d04}.bin", $"textures/HGSS/a/0/0/8/{from + 64:d04}.bin");
}

CopyFolder("textures/DP/graphic/pst_gra.narc/", "textures/HGSS/a/0/3/9/");
CopyFolder("textures/DP/graphic/zukan.narc/", "textures/HGSS/a/0/6/7/");
CopyFile("textures/DP/graphic/mystery.narc/0026.bin", "textures/HGSS/a/1/1/3/0030.bin");

// CopyFile("textures/DP/application/zukanlist/zkn_data/zukan_data.narc/0013.bin", "textures/HGSS/a/0/7/4/0013.bin");
