import os
from helper import GAMES, load_game_data

os.chdir(os.path.join(os.path.dirname(__file__), "../"))

for game_info in GAMES:
  game: str = game_info["game"]
  folder_name: str = game_info["folder_name"]
  file_name: str = game_info["file_name"]
  languages: list[str] = game_info["languages"]
  game_data = load_game_data(game_info)

  file_ids = sorted(game_data[languages[0]].keys())
  with open(f"files/{game}/{file_name}", "w", -1, "utf8") as writer:
    for lang_i, language in enumerate(languages):
      for file_id in file_ids:
        if len(languages) == 1:
          writer.write(f"{file_id}\n")
        else:
          writer.write(f"{file_id}-{lang_i}\n")

        lines = game_data[language].get(file_id, game_data[languages[0]][file_id])
        for i, line in lines.items():
          writer.write(f"{i}\t{line}\n")

  print(f"Merged: {game}/{file_name}")