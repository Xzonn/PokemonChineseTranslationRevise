#!/usr/bin/python3
# -*- coding: UTF-8 -*-

import os
import re

LINE_PATTERN = re.compile(r"^\| (\d+) \| ([^\|]*) \| ([^\|]*) |$", re.M)
GAME_NAMES = {
  "DP": "珍珠／钻石",
  "Pt": "白金",
  "HGSS": "心金／魂银"
}

for game_key in GAME_NAMES:
  with open(os.path.abspath(os.path.join("dist/src", game_key + ".txt")), "w", encoding="utf-8") as file:
    file.write("#3\n")
    file_list = sorted(os.listdir(game_key + "/"))
    for file_name in file_list:
      file.write(str(int(file_name[:3])) + "\n")
      file_path = os.path.join(game_key + "/", file_name)
      with open(file_path, encoding="utf-8") as f:
        text = f.read()
      result = list(filter(lambda x: len("".join(x)) > 0, LINE_PATTERN.findall(text)))
      file.write("\n".join(map(lambda x: "\t".join(x[:2]), result)) + "\n")