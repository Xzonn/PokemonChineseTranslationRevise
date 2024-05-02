import json
import os
from helper import GAMES, load_game_data

os.chdir(os.path.join(os.path.dirname(__file__), "../"))

for game_info in GAMES:
  game: str = game_info["game"]
  folder_name: str = game_info["json_folder"]
  file_name: str = game_info["file_name"]
  languages: list[str] = game_info["languages"]
  game_data = load_game_data(game_info)

  for language in game_data:
    os.makedirs(f"weblate/{folder_name}/{language}/", exist_ok=True)
    for file in game_data[language]:
      with open(f"weblate/{folder_name}/{language}/{file:03d}.json", "w", -1, "utf8", None, "\n") as writer:
        data = {}
        for key, value in game_data[language][file].items():
          value = value.replace("\\n", "\n")
          if not ("\\r\n" in value or "\\f\n" in value):
            value = value.replace("\\r", "\\r\n").replace("\\f", "\\f\n")
          if value.endswith("\\f\n"):
            value = value.removesuffix("\n")
          elif value.endswith("\\r\n"):
            value = value.removesuffix("\n")
          data[f"{folder_name}.{file:03d}.{key:04d}"] = value
        json.dump(data, writer, indent=4, ensure_ascii=False)
        writer.write("\n")

  print(f"Converted: {folder_name}")
