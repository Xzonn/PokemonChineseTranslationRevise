// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.NitroFileBase
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace NARCFileReadingDLL
{
  public abstract class NitroFileBase : FIMGFrame.FileImageEntryBase
  {
    private short m_shBom;
    private short m_shUnknown1;
    private int m_nFileSize;
    private short m_shHeaderSize;
    private List<NitroFileFrameBase> m_lstnffFrames;

    public NitroFileBase()
    {
            Magic = "AAAA";
            m_shBom = -257;
            m_shUnknown1 = 256;
            m_shHeaderSize = 16;
    }

    public NitroFileBase(BinaryReader brrReader)
    {
            ReadFrom(brrReader);
    }

    public abstract string Magic { get; set; }

    public short Bom
    {
      get
      {
        return m_shBom;
      }
    }

    public short Unknown1
    {
      get
      {
        return m_shUnknown1;
      }
    }

    public short Unknown2
    {
      get
      {
        return m_shHeaderSize;
      }
    }

    public ushort FramesCount
    {
      get
      {
        return (ushort)m_lstnffFrames.Count;
      }
    }

    public NitroFileFrameBase[] Frames
    {
      get
      {
        return m_lstnffFrames.ToArray();
      }
    }

    private void NitroFile_ContentChanged(object sender, EventArgs e)
    {
      if (m_fcFileChanged == null)
        return;
            m_fcFileChanged(this);
    }

    public static NitroFileBase ReadFrom(BinaryReader brrReader, int nSize, ref int nMaxSize)
    {
      if (nSize < 0 || nSize > nMaxSize || nSize > brrReader.BaseStream.Length - brrReader.BaseStream.Position)
        throw new FormatException();
      long position = brrReader.BaseStream.Position;
      NitroFileBase nitroFileBase;
      try
      {
        Type type = Type.GetType("NARCFileReadingDLL." + Global.ReplaceOrder(new string(brrReader.ReadChars(4))) + "NitroFile");
        brrReader.BaseStream.Position -= 4L;
        if (type == null)
          nitroFileBase = new SimpleNitroFile(brrReader);
        else
          nitroFileBase = (NitroFileBase) Activator.CreateInstance(type, new object[1]
          {
             brrReader
          });
        if (nitroFileBase.Size != nSize)
          throw new FormatException();
      }
      catch
      {
        brrReader.BaseStream.Position = position;
        throw new FormatException();
      }
      nMaxSize -= nSize;
      return nitroFileBase;
    }

    public override void ReadFrom(BinaryReader brrReader)
    {
            Magic = Global.ReplaceOrder(new string(brrReader.ReadChars(4)));
      foreach (char ch in Magic)
      {
        if (ch < 'A' || ch > 'Z')
          throw new FormatException();
      }
            m_shBom = brrReader.ReadInt16();
            m_shUnknown1 = brrReader.ReadInt16();
            m_nFileSize = brrReader.ReadInt32();
      if (m_nFileSize > brrReader.BaseStream.Length - brrReader.BaseStream.Position + 12L)
        throw new FormatException();
            m_shHeaderSize = brrReader.ReadInt16();
            m_lstnffFrames = new List<NitroFileFrameBase>(brrReader.ReadUInt16());
      for (ushort index = 0; index < m_lstnffFrames.Capacity - 1; ++index)
      {
                m_lstnffFrames.Add(NitroFileFrameBase.CreateFrom(brrReader));
                m_lstnffFrames[index].ContentChanged += new EventHandler(NitroFile_ContentChanged);
        if (m_lstnffFrames[m_lstnffFrames.Count - 1].Size % 2 == 1 && brrReader.ReadByte() != 0)
          throw new FormatException();
      }
            m_lstnffFrames.Add(NitroFileFrameBase.CreateFrom(brrReader));
            m_lstnffFrames[m_lstnffFrames.Count - 1].ContentChanged += new EventHandler(NitroFile_ContentChanged);
    }

    public override void WriteTo(BinaryWriter brwWriter)
    {
      brwWriter.Write(Global.ReplaceOrder(Magic).ToCharArray());
      brwWriter.Write(m_shBom);
      brwWriter.Write(m_shUnknown1);
      brwWriter.Write(m_nFileSize);
      brwWriter.Write(m_shHeaderSize);
      brwWriter.Write((ushort)m_lstnffFrames.Count);
      for (short index = 0; index < m_lstnffFrames.Count - 1; ++index)
      {
                m_lstnffFrames[index].WriteTo(brwWriter);
        if (m_lstnffFrames[index].Size % 2 == 1)
          brwWriter.Write((byte) 0);
      }
            m_lstnffFrames[m_lstnffFrames.Count - 1].WriteTo(brwWriter);
    }

    public void AddFrame(NitroFileFrameBase nffbFrame)
    {
      nffbFrame.ContentChanged += new EventHandler(NitroFile_ContentChanged);
            m_lstnffFrames.Add(nffbFrame);
      if (m_fcFileChanged == null)
        return;
            m_fcFileChanged(this);
    }

    public void InsertFrame(int nIndex, NitroFileFrameBase nffbFrame)
    {
      nffbFrame.ContentChanged += new EventHandler(NitroFile_ContentChanged);
            m_lstnffFrames.Insert(nIndex, nffbFrame);
      if (m_fcFileChanged == null)
        return;
            m_fcFileChanged(this);
    }

    public void RemoveFrame(NitroFileFrameBase nffbFrame)
    {
      if (!m_lstnffFrames.Contains(nffbFrame))
        return;
            m_lstnffFrames.Remove(nffbFrame);
      nffbFrame.ContentChanged -= new EventHandler(NitroFile_ContentChanged);
      if (m_fcFileChanged == null)
        return;
            m_fcFileChanged(this);
    }

    public void RemoveFrameAt(int nIndex)
    {
      if (nIndex >= m_lstnffFrames.Count)
        return;
      NitroFileFrameBase lstnffFrame = m_lstnffFrames[nIndex];
            m_lstnffFrames.RemoveAt(nIndex);
      lstnffFrame.ContentChanged -= new EventHandler(NitroFile_ContentChanged);
      if (m_fcFileChanged == null)
        return;
            m_fcFileChanged(this);
    }
  }
}
