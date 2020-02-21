// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.StringTable
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.IO;

namespace NARCFileReadingDLL
{
  public class StringTable : FIMGFrame.FileImageEntryBase, IStringTable
  {
    private StringTableSection[] m_arrStringTableSections;
    private uint m_unUnknown1;
    private uint m_unUnknown2;

    public StringTable(BinaryReader brrReader)
    {
            ReadFrom(brrReader);
    }

    public IStringTableSection[] StringTableSections
    {
      get
      {
        return m_arrStringTableSections;
      }
    }

    public override void ReadFrom(BinaryReader brrReader)
    {
      if (brrReader.BaseStream.Length - brrReader.BaseStream.Position < 12L)
        throw new FormatException();
            m_arrStringTableSections = new StringTableSection[(brrReader.ReadUInt16())];
      ushort ushEntriesCount = brrReader.ReadUInt16();
      uint num1 = brrReader.ReadUInt32();
            m_unUnknown1 = brrReader.ReadUInt32();
            m_unUnknown2 = brrReader.ReadUInt32();
      if (num1 < (uint) (8 * ushEntriesCount * m_arrStringTableSections.Length))
        throw new FormatException();
      if (brrReader.BaseStream.Length != num1 + (uint)(12 + 4 * m_arrStringTableSections.Length))
        throw new FormatException();
      uint num2 = (uint) (12 + 4 * m_arrStringTableSections.Length);
      for (int index = 0; index < m_arrStringTableSections.Length; ++index)
      {
        brrReader.BaseStream.Position = 16L + index * (uint)m_arrStringTableSections.Length;
        if (brrReader.BaseStream.Position != num2)
          throw new FormatException();
                m_arrStringTableSections[index] = new StringTableSection(brrReader, ushEntriesCount);
        num2 += (uint) brrReader.BaseStream.Position;
        if (m_arrStringTableSections[index].Size != num1)
          throw new FormatException();
                m_arrStringTableSections[index].Changed += new EventHandler(StringTableSection_Changed);
      }
    }

    private void StringTableSection_Changed(object sender, EventArgs args)
    {
      if (m_fcFileChanged == null)
        return;
            m_fcFileChanged(this);
    }

    public override void WriteTo(BinaryWriter brwWriter)
    {
      brwWriter.Write((ushort)m_arrStringTableSections.Length);
      brwWriter.Write((ushort)m_arrStringTableSections[0].Entries.Length);
      uint num = 0;
      foreach (StringTableSection stringTableSection in m_arrStringTableSections)
        num += (uint) stringTableSection.Size;
      brwWriter.Write(num);
      brwWriter.Write(m_unUnknown1);
      brwWriter.Write(m_unUnknown2);
      foreach (StringTableSection stringTableSection in m_arrStringTableSections)
        stringTableSection.WriteTo(brwWriter);
    }
  }
}
