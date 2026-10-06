import csv
import struct

from helper import PATH_CHAR_TABLE, PATH_PINYIN_TABLE, PATH_PINYIN_TABLE_DATA


def read_pinyin_table(filename):
  with open(filename, "r", -1, "utf16") as reader:
    return dict(csv.reader(reader, delimiter=" "))


def read_encoding_table_reverse(filename):
  encoding_table = {}
  with open(filename, "r", -1, "utf8") as reader:
    for row in csv.reader(reader, delimiter="\t", quoting=csv.QUOTE_NONE):
      if len(row) > 1:
        encoding_table.setdefault(row[1][0], int(row[0], 16))
  return encoding_table


def encode_pinyin(pinyin):
  letter_map = {chr(i + ord("a") - 1): i for i in range(1, 27)}
  code = 0
  for i, letter in enumerate(pinyin):
    code |= letter_map[letter] << (25 - 5 * i)
  return code


def create_binary_pinyin_table(pinyin_table, output_filename, encoding_table):
  pinyin_entries = []
  all_codes = []
  offset = 0
  max_num_chars = 0
  for pinyin, characters in pinyin_table.items():
    num_chars = len(characters)
    max_num_chars = max(max_num_chars, num_chars)
    pinyin_entries.append((encode_pinyin(pinyin), offset, num_chars))
    all_codes.extend(encoding_table[character] for character in characters)
    offset += num_chars
  pinyin_entries.sort()

  header_size = 0x10
  index_table_size = len(pinyin_entries) * 8
  third_part_offset = (header_size + index_table_size + 0xF) & ~0xF
  with open(output_filename, "wb") as writer:
    writer.write(struct.pack("<4I", len(pinyin_entries), third_part_offset, max_num_chars, 0))
    writer.writelines(struct.pack("<IHH", *entry) for entry in pinyin_entries)
    writer.write(bytes(third_part_offset - header_size - index_table_size))
    writer.writelines(struct.pack("<H", code) for code in all_codes)


def main():
  pinyin_table = read_pinyin_table(PATH_PINYIN_TABLE)
  encoding_table = read_encoding_table_reverse(PATH_CHAR_TABLE)
  create_binary_pinyin_table(pinyin_table, PATH_PINYIN_TABLE_DATA, encoding_table)


if __name__ == "__main__":
  main()
