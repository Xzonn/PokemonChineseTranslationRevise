#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.11.1"

void EditBinary(ref byte[] bytes, int offset, string newHexString)
{
  var newBytes = newHexString.Split(' ').Select(x => Convert.ToByte(x, 16)).ToArray();
  Array.Copy(newBytes, 0, bytes, offset, newBytes.Length);
  Console.WriteLine($"Edited binary at 0x{offset:X06} (length: 0x{newBytes.Length:X}): {newHexString}");
}

void EditBanner(string gameCode, string newTitle)
{
  var banner = new NitroHelper.Banner($"files/{gameCode}/banner.bin");
  banner.japaneseTitle = newTitle;
  banner.englishTitle = newTitle;
  banner.frenchTitle = newTitle;
  banner.germanTitle = newTitle;
  banner.italianTitle = newTitle;
  banner.spanishTitle = newTitle;
  banner.WriteTo($"out/{gameCode}/banner.bin");
  Console.WriteLine($"Banner title edited: {newTitle.Split('\n')[0]}");
}

void CopyFolder(string from, string to, string pattern = "*")
{
  if (!Directory.Exists(to))
  {
    Directory.CreateDirectory(to);
  }
  foreach (var file in Directory.GetFiles(from, pattern))
  {
    var fileTo = Path.Combine(to, Path.GetFileName(file));
    File.Copy(file, fileTo, true);
    Console.WriteLine($"Copied: {file} -> {fileTo}");
  }
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
