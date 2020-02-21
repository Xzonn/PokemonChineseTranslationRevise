// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.NARCFile
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace NARCFileReadingDLL
{
  public class NARCFile : INintendoItem
  {
    private string m_strMagic;
    private short m_shBom;
    private short m_shUnknown1;
    private short m_shUnknown2;
    private short m_shFramesCount;
    private FATBFrame m_fatfFATB;
    private FNTBFrame m_fntfFNTB;
    private FIMGFrame m_fimgfFIMG;

        public NARCFile(BinaryReader brrReader)
        {
            m_strMagic = new string(brrReader.ReadChars(4));
            if (!m_strMagic.Contains('N'.ToString()) || !m_strMagic.Contains('A'.ToString()) || (!m_strMagic.Contains('R'.ToString()) || !m_strMagic.Contains('C'.ToString())))
                throw new FormatException();
            m_shBom = brrReader.ReadInt16();
            switch (m_shBom)
            {
                case -4097:
                    if (m_strMagic != "CNAR")
                        throw new FormatException();
                    break;
                case -257:
                    if (m_strMagic != "RCNA")
                        throw new FormatException();
                    break;
                case -17:
                    if (m_strMagic != "ARCN")
                        throw new FormatException();
                    break;
                case -2:
                    if (m_strMagic != "NARC")
                        throw new FormatException();
                    break;
                default:
                    throw new FormatException();
            }
            m_shUnknown1 = brrReader.ReadInt16();
            if (brrReader.ReadInt32() != brrReader.BaseStream.Length)
                throw new FormatException();
            m_shUnknown2 = brrReader.ReadInt16();
            m_shFramesCount = brrReader.ReadInt16();
            if (m_shFramesCount != 3)
                throw new FormatException();
            m_fatfFATB = (FATBFrame)NARCFileFrame.ReadFrom(brrReader, null);
            m_fntfFNTB = (FNTBFrame)NARCFileFrame.ReadFrom(brrReader, null);
            m_fimgfFIMG = (FIMGFrame)NARCFileFrame.ReadFrom(brrReader, m_fatfFATB);
            /*if (brrReader.BaseStream.Position != brrReader.BaseStream.Length)
              throw new FormatException();*/
        }

    public int Size
    {
      get
      {
        return 16 + m_fatfFATB.Size + m_fntfFNTB.Size + m_fimgfFIMG.Size;
      }
    }

    public int FilesCount
    {
      get
      {
        return m_fatfFATB.FilesCount;
      }
    }

    public List<FIMGFrame.FileImageEntryBase> Files
    {
      get
      {
        return m_fimgfFIMG.Entries;
      }
    }

    public void WriteTo(BinaryWriter brwWriter)
    {
      brwWriter.Write(m_strMagic.ToCharArray());
      brwWriter.Write(m_shBom);
      brwWriter.Write(m_shUnknown1);
      brwWriter.Write(Size);
      brwWriter.Write(m_shUnknown2);
      brwWriter.Write(m_shFramesCount);
      int num = 0;
      for (int index = 0; index < m_fimgfFIMG.Entries.Count; ++index)
      {
                m_fatfFATB.Entries[index].Start = num;
                m_fatfFATB.Entries[index].End = num + m_fimgfFIMG.Entries[index].Size;
        num += m_fimgfFIMG.Entries[index].Size + (4 - m_fimgfFIMG.Entries[index].Size % 4) % 4;
      }
            m_fatfFATB.WriteTo(brwWriter);
            m_fntfFNTB.WriteTo(brwWriter);
            m_fimgfFIMG.WriteTo(brwWriter);
    }
  }
}
