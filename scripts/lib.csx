#!/usr/bin/env dotnet-script
#r "nuget: NitroHelper, 0.12.0"
using System.Globalization;

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

void CopyFile(string from, string to, string pattern = "*")
{
  if (!Directory.Exists(Path.GetDirectoryName(to)))
  {
    Directory.CreateDirectory(Path.GetDirectoryName(to));
  }
  File.Copy(from, to, true);
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

string[] SortEasyChatWords(ref byte[] arm9, uint offset, string[] words)
{
  const int CATEGORIES = 12;
  var comparer = StringComparer.Create(new CultureInfo("zh-CN"), true);
  var comparerReplacement = new Dictionary<string, string>()
  {
    { "彷徨夜灵", "旁徨夜灵" }, // Fanghuang Yeling -> Panghuang Yeling
    { "聒噪鸟", "锅噪鸟" }, // Guazao Niao -> Guozao Niao
  };

  var easyChatWordsByCategory = new Dictionary<int, Dictionary<string, ushort>>();
  var offsets = new uint[CATEGORIES];
  var lengths = new uint[CATEGORIES];
  using var ms = new MemoryStream(arm9);
  var br = new BinaryReader(ms);
  IEnumerable<string> aikotobaList = [];
  for (int i = 0; i < CATEGORIES; i++)
  {
    ms.Position = offset + i * 12;
    easyChatWordsByCategory.Add(i, []);

    br.ReadUInt32();
    var data_offset = br.ReadUInt32() - 0x2000000;
    var data_length = br.ReadUInt32();
    offsets[i] = data_offset;
    lengths[i] = data_length;

    var aikotobaIds = easyChatWordsByCategory[i - ((i == 1 || i == 3) ? 1 : 0)]; // Pokemon and moves are in the same category
    ms.Position = data_offset;
    for (int j = 0; j < data_length; j++)
    {
      var id = br.ReadUInt16();
      aikotobaIds.Add(words[id], id);
    }
  }
  var bw = new BinaryWriter(ms);
  for (int i = 0; i < CATEGORIES; i++)
  {
    if (i == 1 || i == 3) { continue; }
    var _words = easyChatWordsByCategory[i].Keys.ToList();
    _words.Sort((a, b) =>
    {
      if (comparerReplacement.TryGetValue(a, out string value1)) { a = value1; }
      if (comparerReplacement.TryGetValue(b, out string value2)) { b = value2; }
      return comparer.Compare(a, b);
    });
    switch (i)
    {
      case 4: case 5: case 6: case 8: case 9: aikotobaList = aikotobaList.Concat(_words); break;
    }
    var sortedIds = _words.Select(word => easyChatWordsByCategory[i][word]).ToArray();
    if (i == 0 || i == 2)
    {
      var firstPart = sortedIds.Take((int)lengths[i]).ToArray();
      var secondPart = sortedIds.Skip((int)lengths[i]).ToArray();
      if (secondPart.Length != (int)lengths[i + 1])
      {
        throw new Exception("Easy chat words count mismatch");
      }
      ms.Position = offsets[i];
      foreach (var id in firstPart) { bw.Write(id); }
      ms.Position = offsets[i + 1];
      foreach (var id in secondPart) { bw.Write(id); }
    }
    else
    {
      if (sortedIds.Length != (int)lengths[i])
      {
        throw new Exception("Easy chat words count mismatch");
      }
      ms.Position = offsets[i];
      foreach (var id in sortedIds) { bw.Write(id); }
    }
  }
  arm9 = ms.ToArray();
  return aikotobaList.ToArray();
}

Dictionary<int, Dictionary<int, string>> LoadMessages(string game)
{
  var messages = new Dictionary<int, Dictionary<int, string>>();
  var fileList = File.ReadAllLines($"files/{game}/messages_list.txt");
  foreach (var file in fileList)
  {
    if (file.StartsWith('#') || !file.Contains('\t')) { continue; }
    var fileSplit = file.Split('\t');
    var fileID = int.Parse(fileSplit[0]);
    var filePath = fileSplit[1];
    var lines = File.ReadAllLines(filePath);
    if (!messages.TryGetValue(fileID, out Dictionary<int, string> value))
    {
      value = [];
      messages.Add(fileID, value);
    }
    foreach (var line in lines)
    {
      if (line.StartsWith('#') || !line.Contains('\t')) { continue; }
      var lineSplit = line.Split('\t');
      var lineID = int.Parse(lineSplit[0]);
      var lineText = lineSplit[1];
      value.Add(lineID, lineText);
    }
  }

  return messages;
}