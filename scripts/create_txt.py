import json
import os

os.chdir(os.path.join(os.path.dirname(__file__), "../"))

for game in ["DP", "Pt", "HGSS"]:
  if False:
    with open("scripts/DP_convert_table.txt", "r", -1, "utf8") as reader:
      lines = reader.read().split("\n")

    dp_convert_table = {}
    for line in lines:
      if not line:
        continue
      ja, en = line.split("\t")
      if not ja.isdigit():
        continue
      dp_convert_table[int(ja)] = int(en)

  game_data = {}
  for file_name in os.listdir(f"{game}/zh_Hans/"):
    if not file_name.endswith(".json"):
      continue
    with open(f"{game}/zh_Hans/{file_name}", "r", -1, "utf8") as reader:
      raw_data = json.load(reader)

    for key, value in raw_data.items():
      k_game, k_file_id, k_line_id = key.split(".")
      if game != k_game:
        continue
      if False:
        if k_game == "DP":
          assert int(k_file_id) in dp_convert_table
          k_file_id = dp_convert_table[int(k_file_id)]
        else:
          k_file_id = int(k_file_id)
      else:
        k_file_id = int(k_file_id)
      if k_file_id not in game_data:
        game_data[k_file_id] = {}
      game_data[k_file_id][int(k_line_id)] = value.replace("\\r\n", "\\r").replace("\\f\n", "\\f").replace("\n", "\\n")
  
  with open(f"master/files/{game}/Messages.txt", "w", -1, "utf8", newline="\n") as writer:
    writer.write("#3\n")
    for file_id in sorted(game_data.keys()):
      writer.write(f"{file_id}\n")
      for line_id in sorted(game_data[file_id].keys()):
        writer.write(f"{line_id}\t{game_data[file_id][line_id]}\n")