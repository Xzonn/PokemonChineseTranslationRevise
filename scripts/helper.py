import json
import os
from typing import TypedDict

DIR_FILES = "original_files"
DIR_TEXTS = "texts"
DIR_TEMP = "temp"

PATH_CHAR_TABLE = "files/CharTable.txt"


class GameInfo(TypedDict):
  game: str
  folder_name: str
  file_list: str
  file_name: str
  json_folder: str
  languages: list[str]


GAMES: list[GameInfo] = [
  {
    "game": "DP",
    "folder_name": "messages",
    "file_list": "messages_list.txt",
    "file_name": "messages.txt",
    "json_folder": "DP",
    "languages": ["zh_Hans"],
  },
  {
    "game": "Pt",
    "folder_name": "messages",
    "file_list": "messages_list.txt",
    "file_name": "messages.txt",
    "json_folder": "Pt",
    "languages": ["zh_Hans"],
  },
  {
    "game": "HGSS",
    "folder_name": "messages",
    "file_list": "messages_list.txt",
    "file_name": "messages.txt",
    "json_folder": "HGSS",
    "languages": ["zh_Hans"],
  },
]


def load_game_data(game_info: GameInfo, texts_root: str) -> dict[str, dict[int, dict[int, str]]]:
  game = game_info["game"]
  file_list_name = game_info["file_list"]
  languages = game_info["languages"]
  game_data: dict[str, dict[int, dict[int, str]]] = {language: {} for language in languages}
  with open(f"{texts_root}/{game}/{file_list_name}", "r", -1, "utf8") as reader:
    file_list = reader.read().splitlines()

  language: str = languages[0]
  for line in file_list:
    if not line or line.startswith("#") or "\t" not in line:
      continue
    _1, line_game, file_path = line.split("\t")
    file_id = int(_1)
    line_game: str
    for language in languages:
      full_path = f"{texts_root}/{line_game}/{language}/{file_path.removesuffix(".txt")}.json"
      if file_id not in game_data[language]:
        game_data[language][file_id] = {}
      if not os.path.exists(full_path):
        continue

      with open(full_path, "r", -1, "utf8") as reader:
        lines = json.load(reader)
      for line in lines:
        line_id = line["index"]
        line_content = line["translation"]
        game_data[language][file_id][line_id] = line_content

  return game_data
