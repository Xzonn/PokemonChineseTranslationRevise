GAMES: list[dict[str, str | list]] = [
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
  {
    "game": "BW",
    "folder_name": "messages_common",
    "file_list": "messages_common_list.txt",
    "file_name": "messages_common.txt",
    "json_folder": "BW_common",
    "languages": ["zh_Hans", "zh_Hant"],
  },
  {
    "game": "BW",
    "folder_name": "messages_script",
    "file_list": "messages_script_list.txt",
    "file_name": "messages_script.txt",
    "json_folder": "BW_script",
    "languages": ["zh_Hans", "zh_Hant"],
  },
  {
    "game": "B2W2",
    "folder_name": "messages_common",
    "file_list": "messages_common_list.txt",
    "file_name": "messages_common.txt",
    "json_folder": "B2W2_common",
    "languages": ["zh_Hans", "zh_Hant"],
  },
  {
    "game": "B2W2",
    "folder_name": "messages_script",
    "file_list": "messages_script_list.txt",
    "file_name": "messages_script.txt",
    "json_folder": "B2W2_script",
    "languages": ["zh_Hans", "zh_Hant"],
  },
]

def load_game_data(game_info: dict[str, str | dict]) -> dict[str, dict[int, dict[int, str]]]:
  game: str = game_info["game"]
  file_list_name: str = game_info["file_list"]
  languages: list[str] = game_info["languages"]
  game_data: dict[str, dict[int, dict[int, str]]] = {
    language: {} for language in languages
  }
  with open(f"files/{game}/{file_list_name}", "r", -1, "utf8") as reader:
    file_list = reader.read().splitlines()

  language: str = languages[0]
  for line in file_list:
    if not line or line.startswith("#") or "\t" not in line:
      continue
    file_id, *file_paths = line.split("\t")
    file_id = int(file_id)
    for language, file_path in zip(languages, file_paths):
      if file_id not in game_data[language]:
        game_data[language][file_id] = {}

      with open(file_path, "r", -1, "utf8") as reader:
        lines = reader.read().splitlines()
      for line in lines:
        if not line or line.startswith("#") or "\t" not in line:
          continue
        line_id, line_content = line.split("\t")
        line_id = int(line_id)
        game_data[language][file_id][line_id] = line_content
  
  return game_data