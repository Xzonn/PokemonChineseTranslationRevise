// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.FATBFrame
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace NARCFileReadingDLL
{
  public class FATBFrame : NARCFileFrame
  {
    public const string MAGIC = "FATB";
    private List<FATBFrame.FileAllocationTableEntry> m_lstfateEntries;

        public FATBFrame(BinaryReader brrReader, int nSize, params object[] args)
        {
            if (nSize < 4 || nSize % 8 != 4)
                throw new FormatException();
            int num = brrReader.ReadInt32();
            nSize -= 4;
            if (num < 0 || num * 8 != nSize)
                throw new FormatException();
            m_lstfateEntries = new List<FATBFrame.FileAllocationTableEntry>();
            FATBFrame.FileAllocationTableEntry allocationTableEntry1;
            m_lstfateEntries.Add(allocationTableEntry1 = new FATBFrame.FileAllocationTableEntry(brrReader));
            for (int index = num - 1; index > 0; --index)
            {
                FATBFrame.FileAllocationTableEntry allocationTableEntry2 = new FATBFrame.FileAllocationTableEntry(brrReader);
                if (allocationTableEntry2.Start < allocationTableEntry1.End)
                    throw new FormatException();
                allocationTableEntry1.Changed += new FATBFrame.FileAllocationTableEntry.FileSizeChanged(allocationTableEntry2.SizeChanged);
                m_lstfateEntries.Add(allocationTableEntry1 = allocationTableEntry2);
            }
        }

    public override string Magic
    {
      get
      {
        return "FATB";
      }
    }

    protected override int ContentSize
    {
      get
      {
        return 4 + FilesCount * 8;
      }
    }

    public FATBFrame.FileAllocationTableEntry[] Entries
    {
      get
      {
        return m_lstfateEntries.ToArray();
      }
    }

    public int FilesCount
    {
      get
      {
        return m_lstfateEntries.Count;
      }
    }

    protected override void WriteContentTo(BinaryWriter brwWriter)
    {
      brwWriter.Write(FilesCount);
      foreach (FATBFrame.FileAllocationTableEntry lstfateEntry in m_lstfateEntries)
        lstfateEntry.WriteTo(brwWriter);
    }

    public class FileAllocationTableEntry : INintendoItem
    {
      private FATBFrame.FileAllocationTableEntry.FileSizeChanged m_fscChanged;
      private int m_nStart;
      private int m_nEnd;

      public FileAllocationTableEntry(BinaryReader brrReader)
      {
                m_nStart = brrReader.ReadInt32();
        if (m_nStart < 0 || m_nStart % 4 != 0)
          throw new FormatException();
                m_nEnd = brrReader.ReadInt32();
        if (m_nEnd < m_nStart)
          throw new FormatException();
      }

      public event FATBFrame.FileAllocationTableEntry.FileSizeChanged Changed
      {
        add
        {
                    m_fscChanged += value;
        }
        remove
        {
                    m_fscChanged -= value;
        }
      }

      public int Size
      {
        get
        {
          return 8;
        }
      }

      public int Start
      {
        get
        {
          return m_nStart;
        }
        set
        {
          if (value == Start)
            return;
          if (value < 0 || value % 4 != 0)
            throw new FormatException();
          int num = value - m_nStart;
                    m_nStart = value;
                    End += num;
        }
      }

      public int End
      {
        get
        {
          return m_nEnd;
        }
        set
        {
          if (value == End)
            return;
          if (value < Start)
            throw new FormatException();
                    m_nEnd = value;
          if (m_fscChanged == null)
            return;
                    m_fscChanged(this);
        }
      }

      public void FileChanged(FIMGFrame.FileImageEntryBase fimgeEntry)
      {
                End = Start + fimgeEntry.Size;
      }

      public void SizeChanged(FATBFrame.FileAllocationTableEntry fateEntry)
      {
        int end = fateEntry.End;
        while (end % 4 != 0)
          ++end;
                Start = end;
      }

      public void WriteTo(BinaryWriter brwWriter)
      {
        brwWriter.Write(Start);
        brwWriter.Write(End);
      }

      public delegate void FileSizeChanged(FATBFrame.FileAllocationTableEntry fateEntry);
    }
  }
}
