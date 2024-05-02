import os
from helper import GAMES, load_game_data

os.chdir(os.path.join(os.path.dirname(__file__), "../"))

for game_info in GAMES:
  game: str = game_info["game"]
  folder_name: str = game_info["folder_name"]
  file_name: str = game_info["file_name"]
  languages: list[str] = game_info["languages"]

  if not os.path.exists(f"files/{game}/{file_name}"):
    print(f"{file_name} not found, skipped")
    continue

  game_data: dict[str, dict[int, dict[int, str]]] = {
    language: {} for language in languages
  }
  with open(f"files/{game}/{file_name}", "r", -1, "utf8") as reader:
    lines = reader.read().splitlines()
    for line in lines:
      if not line or line.startswith("#"):
        continue
      if not "\t" in line:
        if "-" in line:
          file_id, lang_i = line.split("-")
          file_id = int(file_id)
          lang_i = int(lang_i)
          language = languages[lang_i]
        else:
          file_id = int(line)
          language = languages[0]
        if not file_id in game_data[language]:
          game_data[language][file_id] = {}
      else:
        line_id, content = line.split("\t")
        line_id = int(line_id)
        game_data[language][file_id][line_id] = content

  file_list_name: str = game_info["file_list"]
  with open(f"files/{game}/{file_list_name}", "r", -1, "utf8") as reader:
    file_list = reader.read().splitlines()
  for line in file_list:
    if not line or line.startswith("#") or "\t" not in line:
      continue
    file_id, *file_paths = line.split("\t")
    file_id = int(file_id)
    for language, file_path in zip(languages, file_paths):
      if (not file_id in game_data[language]) or (not file_path):
        continue
      with open(file_path, "w", -1, "utf8", newline="\n") as writer:
        for line_id, content in game_data[language][file_id].items():
          writer.write(f"{line_id}\t{content}\n")
