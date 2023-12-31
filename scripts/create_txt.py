import json
import os

os.chdir(os.path.join(os.path.dirname(__file__), "../"))

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

for game_info in GAMES:
  original_style = {}
  game_data = {
    "zh_Hans": {}
  }
  game = game_info["game"]
  file_name = game_info["file_name"]
  json_folder = game_info["json_folder"]
  languages = game_info["languages"]

  with open(f"master/files/{game}/{file_name}", "r", -1, "utf8") as reader:
    lines = reader.read().split("\n")
  
  current_file = None
  current_data = None
  for line in lines:
    if not line or line.startswith("#"):
      continue
    if "\t" not in line:
      if "-" in line:
        if line.split("-")[1] != "0":
          break
        current_file = int(line.split("-")[0])
      else:
        current_file = int(line)
      original_style[current_file] = {}
      current_data = original_style[current_file]
      continue
    else:
      line_id, line_content = line.split("\t")
      line_id = int(line_id)
      assert line_id == len(current_data)
      original_style[current_file][line_id] = line_content

  for language in languages:
    if language not in game_data:
      game_data[language] = {}
    if not os.path.exists(f"{json_folder}/{language}/"):
      continue

    for json_name in os.listdir(f"{json_folder}/{language}/"):
      json_id = int(json_name.removesuffix(".json"))
      if not json_name.endswith(".json"):
        continue

      if (json_folder == "B2W2_common" and 113 <= json_id <= 157) or \
         (json_folder == "BW_common" and 93 <= json_id <= 137):
        path = f"{json_folder}/ja/{json_name}"
      else:
        path = f"{json_folder}/{language}/{json_name}"
      with open(path, "r", -1, "utf8") as reader:
        raw_data = json.load(reader)

      for key, chinese in raw_data.items():
        k_game, file_id, line_id = key.split(".")
        if json_folder != k_game:
          continue
        else:
          file_id = int(file_id)
        if file_id not in original_style:
          continue
        if file_id not in game_data[language]:
          game_data[language][file_id] = {}
        
        line_id = int(line_id)
        original_line = original_style[file_id][line_id]
        if not ("\\r\\n" in original_line or "\\f\\n" in original_line):
          chinese = chinese.replace("\\r\n", "\\r").replace("\\f\n", "\\f")
        if original_line.endswith("\\r\\n"):
          chinese = chinese + "\\n"
        elif original_line.endswith("\\f\\n"):
          chinese = chinese + "\\n"
        chinese = chinese.replace("\n", "\\n")
        game_data[language][file_id][line_id] = chinese
  
  with open(f"master/files/{game}/{file_name}", "w", -1, "utf8", newline="\n") as writer:
    if game not in ["BW", "B2W2"]:
      writer.write("#3\n")
    for i, language in enumerate(languages):
      for file_id in sorted(game_data[language].keys()):
        if game in ["BW", "B2W2"]:
          writer.write(f"{file_id}-{i}\n")
        else:
          writer.write(f"{file_id}\n")
        for line_id in sorted(game_data[language][file_id].keys()):
          writer.write(f"{line_id}\t{game_data[language][file_id][line_id]}\n")