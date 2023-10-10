import json
import os

os.chdir(os.path.join(os.path.dirname(__file__), "../"))

for game in ["DP", "Pt", "HGSS"]:
  game_data = {}
  with open(f"files/{game}/Messages.txt", "r", -1, "utf8") as reader:
    lines = reader.read().split("\n")
  
  current_file = None
  current_data = None
  for line in lines:
    if not line or line.startswith("#"):
      continue
    if "\t" not in line:
      current_file = int(line)
      game_data[current_file] = {}
      current_data = game_data[current_file]
      continue
    else:
      line_id, line_content = line.split("\t")
      line_id = int(line_id)
      assert line_id == len(current_data)
      current_data[f"{game}.{current_file:03d}.{line_id:04d}"] = line_content.replace("\\n", "\n").replace("\\r", "\\r\n").replace("\\f", "\\f\n")
  
  for file in game_data:
    with open(f"weblate/{game}/zh_Hans/{file:03d}.json", "w", -1, "utf8", None, "\n") as writer:
      json.dump(game_data[file], writer, indent=4, ensure_ascii=False)
      writer.write("\n")
  
  print(f"Converted: {game}")
