import json
import os

os.chdir(os.path.join(os.path.dirname(__file__), "../"))

GAMES = [
  {
    "game": "DP",
    "file_name": "Messages.txt",
    "output_folder": "DP",
    "languages": ["zh_Hans"],
  },
  {
    "game": "Pt",
    "file_name": "Messages.txt",
    "output_folder": "Pt",
    "languages": ["zh_Hans"],
  },
  {
    "game": "HGSS",
    "file_name": "Messages.txt",
    "output_folder": "HGSS",
    "languages": ["zh_Hans"],
  },
  {
    "game": "BW",
    "file_name": "Messages_common.txt",
    "output_folder": "BW_common",
    "languages": ["zh_Hans", "zh_Hant"],
  },
  {
    "game": "BW",
    "file_name": "Messages_script.txt",
    "output_folder": "BW_script",
    "languages": ["zh_Hans", "zh_Hant"],
  },
  {
    "game": "B2W2",
    "file_name": "Messages_common.txt",
    "output_folder": "B2W2_common",
    "languages": ["zh_Hans", "zh_Hant"],
  },
  {
    "game": "B2W2",
    "file_name": "Messages_script.txt",
    "output_folder": "B2W2_script",
    "languages": ["zh_Hans", "zh_Hant"],
  },
]

for game_info in GAMES:
  game_data = {
    "zh_Hans": {}
  }
  game = game_info["game"]
  file_name = game_info["file_name"]
  output_folder = game_info["output_folder"]
  languages = game_info["languages"]
  with open(f"files/{game}/{file_name}", "r", -1, "utf8") as reader:
    lines = reader.read().split("\n")
  
  current_file = None
  current_data = None
  language = languages[0]
  for line in lines:
    if not line or line.startswith("#"):
      continue
    if "\t" not in line:
      if "-" in line:
        current_file = int(line.split("-")[0])
        language = languages[int(line.split("-")[1])]
        if language not in game_data:
          game_data[language] = {}
      else:
        current_file = int(line)
      game_data[language][current_file] = {}
      current_data = game_data[language][current_file]
      continue
    else:
      line_id, line_content = line.split("\t")
      line_id = int(line_id)
      assert line_id == len(current_data)
      line_content = line_content.replace("\\n", "\n")
      if not ("\\r\n" in line_content or "\\f\n" in line_content):
        line_content = line_content.replace("\\r", "\\r\n").replace("\\f", "\\f\n")
      if line_content.endswith("\\f\n"):
        line_content = line_content.removesuffix("\n")
      elif line_content.endswith("\\r\n"):
        line_content = line_content.removesuffix("\n")
      current_data[f"{output_folder}.{current_file:03d}.{line_id:04d}"] = line_content

  for language in game_data:
    os.makedirs(f"weblate/{output_folder}/{language}/", exist_ok=True)
    for file in game_data[language]:
      with open(f"weblate/{output_folder}/{language}/{file:03d}.json", "w", -1, "utf8", None, "\n") as writer:
        json.dump(game_data[language][file], writer, indent=4, ensure_ascii=False)
        writer.write("\n")
  
  print(f"Converted: {output_folder}")
