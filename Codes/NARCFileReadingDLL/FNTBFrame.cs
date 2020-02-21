// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.FNTBFrame
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace NARCFileReadingDLL
{
  public class FNTBFrame : NARCFileFrame
  {
    public const string MAGIC = "FNTB";
    private int m_nDirectoryStart;
    private short m_sFirstFile;
    private short m_sDirectoryCount;
    private List<FNTBFrame.FileNameTableEntry> m_lstfntbeEntries;

    public FNTBFrame(BinaryReader brrReader, int nSize, params object[] args)
    {
      if (nSize < 0)
        throw new FormatException();
      if (nSize < 8)
        throw new FormatException();
            m_nDirectoryStart = brrReader.ReadInt32();
      nSize -= 4;
            m_sFirstFile = brrReader.ReadInt16();
      nSize -= 2;
            m_sDirectoryCount = brrReader.ReadInt16();
      nSize -= 2;
      switch (m_nDirectoryStart)
      {
        case 4:
          if (nSize > 0)
            throw new FormatException();
                    m_lstfntbeEntries = null;
          break;
        case 8:
          while (nSize > 0)
          {
                        m_lstfntbeEntries = new List<FNTBFrame.FileNameTableEntry>();
                        m_lstfntbeEntries.Add(new FNTBFrame.FileNameTableEntry(brrReader, ref nSize));
          }
          break;
        default:
          throw new FormatException();
      }
    }

    public override string Magic
    {
      get
      {
        return "FNTB";
      }
    }

    protected override int ContentSize
    {
      get
      {
        int num = 8;
        if (m_nDirectoryStart == 8)
        {
          foreach (FNTBFrame.FileNameTableEntry lstfntbeEntry in m_lstfntbeEntries)
            num += lstfntbeEntry.Size;
        }
        return num;
      }
    }

    public bool ContainsNames
    {
      get
      {
        return m_nDirectoryStart == 8;
      }
    }

    public FNTBFrame.FileNameTableEntry[] Entries
    {
      get
      {
        return m_lstfntbeEntries.ToArray();
      }
    }

    protected override void WriteContentTo(BinaryWriter brwWriter)
    {
      brwWriter.Write(m_nDirectoryStart);
      brwWriter.Write(m_sFirstFile);
      brwWriter.Write(m_sDirectoryCount);
      if (m_nDirectoryStart != 8)
        return;
      foreach (FNTBFrame.FileNameTableEntry lstfntbeEntry in m_lstfntbeEntries)
        lstfntbeEntry.WriteTo(brwWriter);
    }

    public class FileNameTableEntry : INintendoItem
    {
      private string m_strFileName;

      public FileNameTableEntry(BinaryReader brrReader, ref int nMaxSize)
      {
        byte num = brrReader.ReadByte();
        --nMaxSize;
        if (num > nMaxSize)
          throw new FormatException();
                m_strFileName = new string(brrReader.ReadChars(num));
        nMaxSize -= num;
      }

      public int Size
      {
        get
        {
          return 1 + m_strFileName.Length;
        }
      }

      public string FileName
      {
        get
        {
          return m_strFileName;
        }
        set
        {
          if (value == null)
            throw new FormatException();
          if (value.Length > byte.MaxValue)
            throw new FormatException();
                    m_strFileName = value;
        }
      }

      public void WriteTo(BinaryWriter brwWriter)
      {
        brwWriter.Write((byte)m_strFileName.Length);
        brwWriter.Write(m_strFileName.ToCharArray());
      }
    }
  }
}
