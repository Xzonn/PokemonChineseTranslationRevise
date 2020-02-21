// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.FontTableItem
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.IO;

namespace NARCFileReadingDLL
{
    public class FontTableItem : IFontTableItem
    {
        private FontTableItem.Pallete[,] m_arrpltParts;
        private byte m_bWidth;

        public FontTableItem()
        {
            m_arrpltParts = new FontTableItem.Pallete[2, 2];
            for (int index1 = 0; index1 < 2; ++index1)
            {
                for (int index2 = 0; index2 < 2; ++index2)
                    m_arrpltParts[index1, index2] = new FontTableItem.Pallete();
            }
        }

        public FontTableItem(BinaryReader brrReader)
        {
            m_arrpltParts = new FontTableItem.Pallete[2, 2];
            for (int index1 = 0; index1 < 2; ++index1)
            {
                for (int index2 = 0; index2 < 2; ++index2)
                    m_arrpltParts[index1, index2] = new FontTableItem.Pallete(brrReader);
            }
        }

        public VALUE[,] Item
        {
            get
            {
                VALUE[,] objArray = new VALUE[16, 16];
                for (int index1 = 0; index1 < 2; ++index1)
                {
                    for (int index2 = 0; index2 < 2; ++index2)
                    {
                        for (int index3 = 0; index3 < 8; ++index3)
                        {
                            for (int index4 = 0; index4 < 8; ++index4)
                                objArray[index1 * 8 + index3, index2 * 8 + index4] = m_arrpltParts[index1, index2].Values[index3, index4];
                        }
                    }
                }
                return objArray;
            }
            set
            {
                if (value == null)
                    throw new FormatException();
                if (value.GetLength(0) != 16 || value.GetLength(1) != 16)
                    throw new FormatException();
                for (int index1 = 0; index1 < 2; ++index1)
                {
                    for (int index2 = 0; index2 < 2; ++index2)
                    {
                        for (int index3 = 0; index3 < 8; ++index3)
                        {
                            for (int index4 = 0; index4 < 8; ++index4)
                                m_arrpltParts[index1, index2].Values[index3, index4] = value[index1 * 8 + index3, index2 * 8 + index4];
                        }
                    }
                }
            }
        }

        public byte Height
        {
            get
            {
                return 16;
            }
        }

        public byte Width
        {
            get
            {
                return m_bWidth;
            }
            set
            {
                if (value > 16 || value < 0)
                    return;
                m_bWidth = value;
            }
        }

        public void ReadWidthFrom(BinaryReader brrReader)
        {
            m_bWidth = brrReader.ReadByte();
        }

        public void WriteWidthTo(BinaryWriter brwWriter)
        {
            brwWriter.Write(m_bWidth);
        }

        public void WriteTo(BinaryWriter brwWriter)
        {
            FontTableItem.Pallete[,] arrpltParts = m_arrpltParts;
            int upperBound1 = arrpltParts.GetUpperBound(0);
            int upperBound2 = arrpltParts.GetUpperBound(1);
            for (int lowerBound1 = arrpltParts.GetLowerBound(0); lowerBound1 <= upperBound1; ++lowerBound1)
            {
                for (int lowerBound2 = arrpltParts.GetLowerBound(1); lowerBound2 <= upperBound2; ++lowerBound2)
                    arrpltParts[lowerBound1, lowerBound2].WriteTo(brwWriter);
            }
        }

        private class Pallete
        {
            private VALUE[,] m_arrvValues;

            public Pallete()
            {
                m_arrvValues = new VALUE[8, 8];
                for (int index1 = 0; index1 < 8; ++index1)
                {
                    for (int index2 = 7; index2 >= 0; --index2)
                        m_arrvValues[index1, index2] = VALUE.VALUE_0;
                }
            }

            public Pallete(BinaryReader brrReader)
            {
                m_arrvValues = new VALUE[8, 8];
                for (int index1 = 0; index1 < 8; ++index1)
                {
                    ushort num = brrReader.ReadUInt16();
                    for (int index2 = 7; index2 >= 0; --index2)
                    {
                        m_arrvValues[index1, index2] = (VALUE)(num & 3);
                        num /= 4;
                    }
                }
            }

            public VALUE[,] Values
            {
                get
                {
                    return m_arrvValues;
                }
            }

            public void WriteTo(BinaryWriter brwWriter)
            {
                for (int index1 = 0; index1 < 8; ++index1)
                {
                    ushort num = (byte)m_arrvValues[index1, 0];
                    for (int index2 = 1; index2 < 8; ++index2)
                        num = (ushort)((ushort)(num * 4U) + (uint)(byte)m_arrvValues[index1, index2]);
                    brwWriter.Write(num);
                }
            }
        }
    }
}
