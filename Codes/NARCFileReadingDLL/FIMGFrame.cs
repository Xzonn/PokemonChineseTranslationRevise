// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.FIMGFrame
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace NARCFileReadingDLL
{
    public class FIMGFrame : NARCFileFrame
    {
        public const string MAGIC = "FIMG";
        private List<FIMGFrame.FileImageEntryBase> m_lstfimgeEntries;

        public FIMGFrame(BinaryReader brrReader, int nSize, params object[] args)
        {
            FATBFrame fatbFrame = (FATBFrame)args[0];
            if (nSize < 0 || nSize > 0 && fatbFrame.FilesCount == 0)
                throw new FormatException();
            m_lstfimgeEntries = new List<FIMGFrame.FileImageEntryBase>(fatbFrame.FilesCount);
            int position = (int)brrReader.BaseStream.Position;
            int num = 0;
            int tempNum = 0;
            foreach (FATBFrame.FileAllocationTableEntry entry in fatbFrame.Entries)
            {
                tempNum++;
                //if (tempNum > 4) break;
                if (entry.Start != brrReader.BaseStream.Position - position)
                    throw new FormatException();
                FIMGFrame.FileImageEntryBase fileImageEntryBase = null;
                foreach (byte readByte in brrReader.ReadBytes(4))
                {
                    if (readByte < 65 || readByte > 90)
                    {
                        brrReader.BaseStream.Position -= 4L;
                        m_lstfimgeEntries.Add(fileImageEntryBase = SimpleFileImageEntry.ReadFrom(brrReader, entry.End - entry.Start, ref nSize));
                        break;
                    }
                }
                if (fileImageEntryBase == null)
                {
                    brrReader.ReadInt32();
                    bool flag = brrReader.ReadInt32() > brrReader.BaseStream.Length - brrReader.BaseStream.Position + 12L;
                    brrReader.BaseStream.Position -= 12L;
                    try
                    {
                        if (flag)
                            m_lstfimgeEntries.Add(fileImageEntryBase = SimpleFileImageEntry.ReadFrom(brrReader, entry.End - entry.Start, ref nSize));
                        else
                            m_lstfimgeEntries.Add(fileImageEntryBase = NitroFileBase.ReadFrom(brrReader, entry.End - entry.Start, ref nSize));
                    }
                    catch
                    {
                        try
                        {
                            if (flag)
                                m_lstfimgeEntries.Add(fileImageEntryBase = SimpleFileImageEntry.ReadFrom(brrReader, entry.End - entry.Start - 2, ref nSize));
                            else
                                m_lstfimgeEntries.Add(fileImageEntryBase = NitroFileBase.ReadFrom(brrReader, entry.End - entry.Start - 2, ref nSize));
                        }
                        catch { }
                    }
                    ++num;
                }
                fileImageEntryBase.Changed += new FIMGFrame.FileImageEntryBase.FileChanged(entry.FileChanged);
                while ((brrReader.BaseStream.Position - position) % 4L != 0L)
                {
                    if (brrReader.ReadByte() != byte.MaxValue)
                        throw new FormatException();
                    --nSize;
                }
            }
            /*if (nSize != 0)
                throw new FormatException();*/
            //this.Entries[3] = this.Entries[2];
        }

        public override string Magic
        {
            get
            {
                return "FIMG";
            }
        }

        protected override int ContentSize
        {
            get
            {
                int num = 0;
                for (int index = 0; index < m_lstfimgeEntries.Count; ++index)
                {
                    num += m_lstfimgeEntries[index].Size;
                    while (num % 4 != 0)
                        ++num;
                }
                return num;
            }
        }

        public List<FIMGFrame.FileImageEntryBase> Entries
        {
            get
            {
                return m_lstfimgeEntries;
            }
        }

        protected override void WriteContentTo(BinaryWriter brwWriter)
        {
            int position = (int)brwWriter.BaseStream.Position;
            foreach (FIMGFrame.FileImageEntryBase lstfimgeEntry in m_lstfimgeEntries)
            {
                lstfimgeEntry.WriteTo(brwWriter);
                while ((brwWriter.BaseStream.Length - position) % 4L != 0L)
                    brwWriter.Write(byte.MaxValue);
            }
        }

        public abstract class FileImageEntryBase : INintendoDataItem, INintendoItem
        {
            protected FIMGFrame.FileImageEntryBase.FileChanged m_fcFileChanged;

            public event FIMGFrame.FileImageEntryBase.FileChanged Changed
            {
                add
                {
                    m_fcFileChanged += value;
                }
                remove
                {
                    m_fcFileChanged -= value;
                }
            }

            public virtual byte[] File
            {
                get
                {
                    ByteArrayStream byteArrayStream = new ByteArrayStream();
                    WriteTo(new BinaryWriter(byteArrayStream));
                    byteArrayStream.Position = 0L;
                    return new BinaryReader(byteArrayStream).ReadBytes((int)byteArrayStream.Length);
                }
                set
                {
                    ReadFrom(new BinaryReader(new ByteArrayStream(value)));
                }
            }

            public int Size
            {
                get
                {
                    return File.Length;
                }
            }

            public void Copy(FIMGFrame.FileImageEntryBase fimgEntry)
            {
                ReadFrom(new BinaryReader(new ByteArrayStream(fimgEntry.File)));
            }

            public abstract void ReadFrom(BinaryReader brrReader);

            public abstract void WriteTo(BinaryWriter brwWriter);

            public delegate void FileChanged(FIMGFrame.FileImageEntryBase fimgeEntry);
        }
    }
}
