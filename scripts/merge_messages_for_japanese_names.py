import os

from helper import DIR_TEMP, DIR_TEXTS, GAMES, GameInfo, load_game_data


def merge_messages_for_japanese_names(games: list[GameInfo], texts_root: str, output_root: str):
  version = os.environ.get("XZ_PATCH_VERSION", "UNKNOWN")[:7]

  for game_info in games:
    game = game_info["game"]
    file_name = game_info["file_name"].replace(".txt", "_J.txt")
    languages = game_info["languages"]
    game_data = load_game_data(game_info, texts_root)

    file_ids = sorted(game_data[languages[0]].keys())
    os.makedirs(f"{output_root}/{game}", exist_ok=True)
    with open(f"{output_root}/{game}/{file_name}", "w", -1, "utf8", None, "\n") as writer:
      writer.write("#4\n")
      for lang_i, language in enumerate(languages):
        for file_id in file_ids:
          if f"{game}-{file_id}" in {"DP-324", "DP-356", "Pt-369", "Pt-408", "HGSS-198", "HGSS-232"}:
            continue
          lines = game_data[language].get(file_id, game_data[languages[0]][file_id])
          if len(lines) == 0:
            continue

          if len(languages) == 1:
            writer.write(f"{file_id}\n")
          else:
            writer.write(f"{file_id}-{lang_i}\n")

          for i, line in lines.items():
            line = (
              line.replace("\\f\n", "\\f")
              .replace("\\r\n", "\\r")
              .replace("\n", "\\n")
              .replace("${VERSION}", f"{version}+J")
            )
            if f"{game}-{file_id}" in {"DP-13", "DP-23", "Pt-17"} and line.startswith("盒子"):
              continue
            writer.write(f"{i}\t{line}\n")

    print(f"Merged: {game}/{file_name}")


if __name__ == "__main__":
  merge_messages_for_japanese_names(GAMES, DIR_TEXTS, DIR_TEMP)
