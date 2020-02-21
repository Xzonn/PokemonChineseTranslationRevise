// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.SimpleFontTable
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace NARCFileReadingDLL
{
    public class SimpleFontTable : FIMGFrame.FileImageEntryBase, IFontTable
    {
        private int chineseMaxChar = 0x119D;
        private int originalMaxChar = 0x01FD;
        private const int MAX_HEIGHT = 16;
        private int m_nHeaderSize;
        private int m_nTableSize;
        private int m_nItemsCount;
        private byte m_bMaxWidth;
        private byte m_bHeight;
        private byte m_bBitsPerPixel;
        private byte m_bOrientation;
        private List<FontTableItem> m_lstftiItems = new List<FontTableItem>();

        public SimpleFontTable(BinaryReader brrReader)
        {
            ReadFrom(brrReader);
        }

        public IFontTableItem[] Items
        {
            get
            {
                return m_lstftiItems.ToArray();
            }
        }

        public byte Height
        {
            get
            {
                return m_bHeight;
            }
        }

        public byte MaxWidth
        {
            get
            {
                return m_bMaxWidth;
            }
        }

        public byte MaxHeight
        {
            get
            {
                return 16;
            }
        }

        public override void ReadFrom(BinaryReader brrReader)
        {
            m_nHeaderSize = brrReader.ReadInt32();
            if (m_nHeaderSize != 16)
                throw new FormatException();
            m_nTableSize = brrReader.ReadInt32();
            if (m_nTableSize < 0)
                throw new FormatException();
            m_nItemsCount = brrReader.ReadInt32();
            m_bMaxWidth = brrReader.ReadByte();
            m_bHeight = brrReader.ReadByte();
            m_bBitsPerPixel = brrReader.ReadByte();
            m_bOrientation = brrReader.ReadByte();
            for (int i = 0; i < chineseMaxChar; i++)
            {
                m_lstftiItems.Add(new FontTableItem(brrReader));
            }
            brrReader.BaseStream.Position = m_nTableSize;
            for (int i = 0; i < originalMaxChar; i++)
            {
                m_lstftiItems[i].ReadWidthFrom(brrReader);
            }
            for(int i = originalMaxChar; i < chineseMaxChar; i++)
            {
                m_lstftiItems[i].Width = 12;
            }
        }

        public override void WriteTo(BinaryWriter brwWriter)
        {
            brwWriter.Write(m_nHeaderSize);
            brwWriter.Write(m_nTableSize);
            brwWriter.Write(originalMaxChar);
            brwWriter.Write(m_bMaxWidth);
            brwWriter.Write(m_bHeight);
            brwWriter.Write(m_bBitsPerPixel);
            brwWriter.Write(m_bOrientation);
            foreach (FontTableItem lstftiItem in m_lstftiItems)
                lstftiItem.WriteTo(brwWriter);
            int TableSize = (int)brwWriter.BaseStream.Position;
            int pos = 0;
            for (pos = 0; pos < originalMaxChar; pos++)
            {
                m_lstftiItems[pos].WriteWidthTo(brwWriter);
            }
            brwWriter.Write(new byte[] { 255, 255, 255 });
            brwWriter.BaseStream.Position = 4;
            brwWriter.Write(TableSize);
        }

        public void AddNewItem()
        {
            m_lstftiItems.Add(new FontTableItem());
            if (m_fcFileChanged == null)
                return;
            m_fcFileChanged(this);
        }
    }
}
