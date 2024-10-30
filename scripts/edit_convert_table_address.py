import os
import re
import struct
import sys

from helper import PATH_CHAR_TABLE_3TO4

PATTERN = re.compile(r"^(.+) = 0x(.+);$", re.MULTILINE)


def edit_convert_table_address(arm9_path: str, msg_path: str, symbol_path: str):
  with open(arm9_path, "rb") as reader:
    arm9 = reader.read()

  msg_file_size = os.path.getsize(msg_path)
  conversion_table_size = os.path.getsize(PATH_CHAR_TABLE_3TO4)
  conversion_table_offset = msg_file_size - conversion_table_size

  with open(symbol_path, "r", -1, "utf8") as reader:
    symbol = reader.read()

  symbol_dict = {name: int(address, 16) - 0x2000000 for name, address in PATTERN.findall(symbol)}

  arm9 = (
    arm9[: symbol_dict["conversion_table_origin"]]
    + struct.pack("<I", conversion_table_offset)
    + arm9[symbol_dict["conversion_table_origin"] + 4 :]
  )
  arm9 = (
    arm9[: symbol_dict["conversion_table_chinese"]]
    + struct.pack("<I", conversion_table_offset + 0x3E0)
    + arm9[symbol_dict["conversion_table_chinese"] + 4 :]
  )

  with open(arm9_path, "wb") as writer:
    writer.write(arm9)


if __name__ == "__main__":
  edit_convert_table_address(*sys.argv[1:])
