using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonCTR
{
    class Narc
    {
        private class NarcHeader
        {
            public uint id;
            public ushort id0;
            public ushort id1;
            public uint file_size;
            public ushort length;
            public ushort num_sections;
        }

        private class FatbHeader
        {
            public uint id;
            public uint length;
            public uint num_entries;
        }

        private class FatbEntry
        {
            public uint file_from_offset;
            public uint file_to_offset;
        }

        private class FntbHeader
        {
            public uint id;
            public uint length;
            public uint unknown0;
            public uint unknown1;
        }

        private class FntbEntry
        {
            public byte name_length;
            public char[] name;
        }

        private class FimgHeader
        {
            public uint id;
            public uint length;
        }

        public readonly List<byte[]> Files = new List<byte[]>();
        private readonly NarcHeader narcHeader;
        private readonly FatbHeader fatbHeader;
        private readonly List<FatbEntry> fatbEntries;
        private readonly FntbHeader fntbHeader;
        private readonly List<FntbEntry> fntbEntries;
        private readonly FimgHeader fimgHeader;

        /// <summary>
        /// 从 narc 文件路径读取 narc 文件。
        /// </summary>
        /// <param name="path">narc 文件路径</param>
        public Narc (string path)
        {
            if (File.Exists(path))
            {
                BinaryReader br = new BinaryReader(File.OpenRead(path));
                int i;
                narcHeader = new NarcHeader {
                    id = br.ReadUInt32(),
                    id0 = br.ReadUInt16(),
                    id1 = br.ReadUInt16(),
                    file_size = br.ReadUInt32(),
                    length = br.ReadUInt16(),
                    num_sections = br.ReadUInt16()
                };
                fatbHeader = new FatbHeader
                {
                    id = br.ReadUInt32(),
                    length = br.ReadUInt32(),
                    num_entries = br.ReadUInt32()
                };
                fatbEntries = new List<FatbEntry>();
                for (i = 0; i < fatbHeader.num_entries; i++)
                {
                    fatbEntries.Add(new FatbEntry
                    {
                        file_from_offset = br.ReadUInt32(),
                        file_to_offset = br.ReadUInt32()
                    });
                }
                fntbHeader = new FntbHeader
                {
                    id = br.ReadUInt32(),
                    length = br.ReadUInt32(),
                    unknown0 = br.ReadUInt32(),
                    unknown1 = br.ReadUInt32()
                };
                fntbEntries = new List<FntbEntry>();
                if (fntbHeader.unknown0 == 8) // 原文为 unknown1
                {
                    for (i = 0; i < fatbHeader.num_entries; i++)
                    {
                        FntbEntry tempEntry = new FntbEntry
                        {
                            name_length = br.ReadByte()
                        };
                        tempEntry.name = br.ReadChars(tempEntry.name_length);
                        fntbEntries.Add(tempEntry);
                    }
                }
                br.BaseStream.Position = narcHeader.length + fatbHeader.length + fntbHeader.length;
                fimgHeader = new FimgHeader
                {
                    id = br.ReadUInt32(),
                    length = br.ReadUInt32()
                };
                long positon = br.BaseStream.Position;
                for (i = 0; i < fatbHeader.num_entries; i++)
                {
                    uint fileLength = fatbEntries[i].file_to_offset - fatbEntries[i].file_from_offset;
                    if (fntbHeader.unknown0 == 8)
                    {
                        br.BaseStream.Position = positon + fatbEntries[i].file_from_offset;
                        Files.Add(br.ReadBytes((int)fileLength));
                    }
                }
                br.Close();
            }
        }
    }
}
