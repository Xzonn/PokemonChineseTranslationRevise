// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.CMAPFrame
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace NARCFileReadingDLL
{
  public class CMAPFrame : NitroFileFrameBase
  {
    private const string MAGIC = "CMAP";
    private char m_cFirstValue;
    private char m_cLastValue;
    private uint m_unType;
    private uint m_unNextCMAPFrameIndex;
    private Dictionary<char, ushort> m_dicushushIndexes;

    public CMAPFrame(BinaryReader brrReader)
      : base(brrReader)
    {
    }

    public override string Magic
    {
      get
      {
        return "CMAP";
      }
      set
      {
        if (value != "CMAP")
          throw new FormatException();
      }
    }

    public char FirstValue
    {
      get
      {
        return m_cFirstValue;
      }
    }

    public char LastValue
    {
      get
      {
        return m_cLastValue;
      }
    }

    public uint Type
    {
      get
      {
        return m_unType;
      }
    }

    public ushort this[char cValue]
    {
      get
      {
        return m_dicushushIndexes[cValue];
      }
      set
      {
        if (m_unType == 2U)
        {
          if (!m_dicushushIndexes.ContainsKey(cValue))
                        m_unNextCMAPFrameIndex += 4U;
                    m_dicushushIndexes[cValue] = value;
        }
        else
        {
          if (cValue < m_cFirstValue || cValue > m_cLastValue)
            return;
                    m_dicushushIndexes[cValue] = value;
        }
      }
    }

    public static CMAPFrame CreateType0(char cFirstValue, char cLastValue, ushort ushFirstIndex)
    {
      BinaryWriter binaryWriter = new BinaryWriter(new ByteArrayStream());
      binaryWriter.Write(Global.ReplaceOrder("CMAP").ToCharArray());
      binaryWriter.Write(24U);
      binaryWriter.Write((ushort) cFirstValue);
      binaryWriter.Write((ushort) cLastValue);
      binaryWriter.Write(0U);
      binaryWriter.Write(0U);
      binaryWriter.Write(ushFirstIndex);
      binaryWriter.Write((ushort) 0);
      binaryWriter.BaseStream.Position = 0L;
      return new CMAPFrame(new BinaryReader(binaryWriter.BaseStream));
    }

    public static CMAPFrame CreateType1(char cFirstValue, char cLastValue, ushort[] arrushIndexes)
    {
      BinaryWriter binaryWriter = new BinaryWriter(new ByteArrayStream());
      binaryWriter.Write(Global.ReplaceOrder("CMAP").ToCharArray());
      binaryWriter.Write((uint) (20 + (arrushIndexes.Length + arrushIndexes.Length % 2) * 2));
      binaryWriter.Write((ushort) cFirstValue);
      binaryWriter.Write((ushort) cLastValue);
      binaryWriter.Write(1U);
      binaryWriter.Write(0U);
      foreach (ushort arrushIndex in arrushIndexes)
        binaryWriter.Write(arrushIndex);
      while (binaryWriter.BaseStream.Length % 4L != 0L)
        binaryWriter.Write((byte) 0);
      binaryWriter.BaseStream.Position = 0L;
      return new CMAPFrame(new BinaryReader(binaryWriter.BaseStream));
    }

    protected override byte[] GetContent()
    {
      BinaryWriter binaryWriter = new BinaryWriter(new ByteArrayStream());
      binaryWriter.Write((ushort)m_cFirstValue);
      binaryWriter.Write((ushort)m_cLastValue);
      binaryWriter.Write(m_unType);
      binaryWriter.Write(m_unNextCMAPFrameIndex);
      switch (m_unType)
      {
        case 0:
          binaryWriter.Write(m_dicushushIndexes[m_cFirstValue]);
          break;
        case 1:
          for (char cFirstValue = m_cFirstValue; cFirstValue <= m_cLastValue; ++cFirstValue)
            binaryWriter.Write(m_dicushushIndexes[cFirstValue]);
          break;
        case 2:
          binaryWriter.Write((ushort)m_dicushushIndexes.Count);
          using (Dictionary<char, ushort>.Enumerator enumerator = m_dicushushIndexes.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<char, ushort> current = enumerator.Current;
              binaryWriter.Write((ushort) current.Key);
              binaryWriter.Write(current.Value);
            }
            break;
          }
      }
      while (binaryWriter.BaseStream.Length % 4L != 0L)
        binaryWriter.Write((byte) 0);
      binaryWriter.BaseStream.Position = 0L;
      return new BinaryReader(binaryWriter.BaseStream).ReadBytes((int) binaryWriter.BaseStream.Length);
    }

    protected override void SetContent(byte[] arrbContent)
    {
      if (arrbContent.Length % 4 != 0)
        throw new FormatException();
      if (arrbContent.Length < 16)
        throw new FormatException();
      BinaryReader binaryReader = new BinaryReader(new ByteArrayStream(arrbContent));
            m_cFirstValue = (char) binaryReader.ReadUInt16();
            m_cLastValue = (char) binaryReader.ReadUInt16();
      if (m_cFirstValue > m_cLastValue)
        throw new FormatException();
            m_unType = binaryReader.ReadUInt32();
            m_unNextCMAPFrameIndex = binaryReader.ReadUInt32();
            m_dicushushIndexes = new Dictionary<char, ushort>();
      switch (m_unType)
      {
        case 0:
          if (arrbContent.Length != 16)
            throw new FormatException();
          ushort num1 = binaryReader.ReadUInt16();
          if (num1 - m_cFirstValue + m_cLastValue > ushort.MaxValue)
            throw new FormatException();
          for (char cFirstValue = m_cFirstValue; cFirstValue <= m_cLastValue; ++cFirstValue)
          {
                        m_dicushushIndexes[cFirstValue] = num1;
            ++num1;
          }
          break;
        case 1:
          int num2 = 12 + 2 * (m_cLastValue - m_cFirstValue + 1);
          if (num2 + (4 - num2 % 4) % 4 != arrbContent.Length)
            throw new FormatException();
          for (char cFirstValue = m_cFirstValue; cFirstValue <= m_cLastValue; ++cFirstValue)
          {
            ushort num3 = binaryReader.ReadUInt16();
                        m_dicushushIndexes[cFirstValue] = num3;
          }
          break;
        case 2:
          if (m_cFirstValue != char.MinValue || m_cLastValue != char.MaxValue)
            throw new FormatException();
          ushort num4 = binaryReader.ReadUInt16();
          int num5 = 14 + 4 * num4;
          if (num5 + (4 - num5 % 4) % 4 != arrbContent.Length)
            throw new FormatException();
          for (int index = 0; index < num4; ++index)
                        m_dicushushIndexes[(char) binaryReader.ReadUInt16()] = binaryReader.ReadUInt16();
          break;
        default:
          throw new FormatException();
      }
      while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
      {
        if (binaryReader.ReadByte() != 0)
          throw new FormatException();
      }
    }

    public bool ContainsValue(char cValue)
    {
      if (m_unType == 2U)
        return m_dicushushIndexes.ContainsKey(cValue);
      if (cValue >= m_cFirstValue)
        return cValue <= m_cLastValue;
      return false;
    }

    public void UpdateFromNFTRNitroFile(NFTRNitroFile nftrNitro)
    {
      int index1 = 3;
      while (index1 < nftrNitro.FramesCount - 1 && nftrNitro.Frames[index1] != this)
        ++index1;
      if (index1 == nftrNitro.FramesCount - 1)
        return;
      uint size = (uint)Size;
            m_unNextCMAPFrameIndex = index1 != 3 ? ((CMAPFrame) nftrNitro.Frames[index1 - 1]).m_unNextCMAPFrameIndex + size : ((FINFFrame) nftrNitro.Frames[0]).CMAPOffset + size;
      uint nextCmapFrameIndex = m_unNextCMAPFrameIndex;
      for (int index2 = index1 + 1; index2 < nftrNitro.FramesCount; ++index2)
      {
        nextCmapFrameIndex += (uint) nftrNitro.Frames[index2].Size;
        ((CMAPFrame) nftrNitro.Frames[index2]).m_unNextCMAPFrameIndex = nextCmapFrameIndex;
      }
    }
  }
}
