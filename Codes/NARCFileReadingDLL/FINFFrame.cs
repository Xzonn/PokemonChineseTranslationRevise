// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.FINFFrame
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.IO;

namespace NARCFileReadingDLL
{
  public class FINFFrame : NitroFileFrameBase
  {
    public const string MAGIC = "FINF";
    private byte m_bUnknown1;
    private byte m_bSpacing;
    private short m_shUnknown2;
    private byte m_bUnknown3;
    private byte m_bUnknown4;
    private byte m_bUnknown5;
    private FINFFrame.Encoding m_encEncodingCode;
    private uint m_unCGLPOffset;
    private uint m_unCWDHOffset;
    private uint m_unCMAPOffset;

    public FINFFrame(BinaryReader brrReader)
      : base(brrReader)
    {
    }

    public override string Magic
    {
      get
      {
        return "FINF";
      }
      set
      {
        if (value != "FINF")
          throw new FormatException();
      }
    }

    public byte Unknown1
    {
      get
      {
        return m_bUnknown1;
      }
    }

    public byte Spacing
    {
      get
      {
        return m_bSpacing;
      }
    }

    public short Unknown2
    {
      get
      {
        return m_shUnknown2;
      }
    }

    public byte Unknown3
    {
      get
      {
        return m_bUnknown3;
      }
    }

    public byte Unknown4
    {
      get
      {
        return m_bUnknown4;
      }
    }

    public byte Unknown5
    {
      get
      {
        return m_bUnknown5;
      }
    }

    public FINFFrame.Encoding EncodingCode
    {
      get
      {
        return m_encEncodingCode;
      }
    }

    public uint CGLPOffset
    {
      get
      {
        return m_unCGLPOffset;
      }
    }

    public uint CWDHOffset
    {
      get
      {
        return m_unCWDHOffset;
      }
    }

    public uint CMAPOffset
    {
      get
      {
        return m_unCMAPOffset;
      }
    }

    protected override byte[] GetContent()
    {
      BinaryWriter binaryWriter = new BinaryWriter(new ByteArrayStream());
      binaryWriter.Write(m_bUnknown1);
      binaryWriter.Write(m_bSpacing);
      binaryWriter.Write(m_shUnknown2);
      binaryWriter.Write(m_bUnknown3);
      binaryWriter.Write(m_bUnknown4);
      binaryWriter.Write(m_bUnknown5);
      binaryWriter.Write((byte)m_encEncodingCode);
      binaryWriter.Write(m_unCGLPOffset);
      binaryWriter.Write(m_unCWDHOffset);
      binaryWriter.Write(m_unCMAPOffset);
      binaryWriter.BaseStream.Position = 0L;
      return new BinaryReader(binaryWriter.BaseStream).ReadBytes((int) binaryWriter.BaseStream.Length);
    }

    protected override void SetContent(byte[] arrbContent)
    {
      BinaryReader binaryReader = new BinaryReader(new ByteArrayStream(arrbContent));
            m_bUnknown1 = binaryReader.ReadByte();
            m_bSpacing = binaryReader.ReadByte();
            m_shUnknown2 = binaryReader.ReadInt16();
            m_bUnknown3 = binaryReader.ReadByte();
            m_bUnknown4 = binaryReader.ReadByte();
            m_bUnknown5 = binaryReader.ReadByte();
            m_encEncodingCode = (FINFFrame.Encoding) binaryReader.ReadByte();
            m_unCGLPOffset = binaryReader.ReadUInt32();
            m_unCWDHOffset = binaryReader.ReadUInt32();
            m_unCMAPOffset = binaryReader.ReadUInt32();
    }

    public enum Encoding
    {
      UTF_8,
      UTF_16,
      Shift_JIS,
    }
  }
}
