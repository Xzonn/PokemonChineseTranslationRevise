#!/usr/bin/env dotnet-script

var originalPath = Args[0];
var originalLength = (int)(new FileInfo(originalPath).Length);

var newPath = Args[1];
var newBytes = File.ReadAllBytes(newPath);

var padding = new byte[originalLength - newBytes.Length];
File.WriteAllBytes(newPath, newBytes.Concat(padding).ToArray());
