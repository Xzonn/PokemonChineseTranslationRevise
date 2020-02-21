// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.SimpleFileImageEntry
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.IO;

namespace NARCFileReadingDLL
{
    public class SimpleFileImageEntry : FIMGFrame.FileImageEntryBase
    {
        protected byte[] m_arrbContent;

        public SimpleFileImageEntry()
        {
        }

        public SimpleFileImageEntry(BinaryReader brrReader)
        {
            ReadFrom(brrReader);
        }

        public SimpleFileImageEntry(byte[] arrbFile)
        {
            ReadFrom(new BinaryReader(new ByteArrayStream(arrbFile)));
        }

        public static FIMGFrame.FileImageEntryBase ReadFrom(BinaryReader brrReader, int nSize, ref int nMaxSize)
        {
            if (nSize < 0 || nSize > nMaxSize || nSize > brrReader.BaseStream.Length - brrReader.BaseStream.Position)
                throw new FormatException();
            byte[] arrbFile = brrReader.ReadBytes(nSize);
            nMaxSize -= nSize;
            return new SimpleFileImageEntry(arrbFile);
        }

        public override void ReadFrom(BinaryReader brrReader)
        {
            m_arrbContent = brrReader.ReadBytes((int)brrReader.BaseStream.Length);
            if (m_fcFileChanged == null)
                return;
            m_fcFileChanged(this);
        }

        public override void WriteTo(BinaryWriter brwWriter)
        {
            brwWriter.Write(m_arrbContent);
            char[] chArray = new char[m_arrbContent.Length];
            for (int index = 0; index < chArray.Length; ++index)
                chArray[index] = (char)m_arrbContent[index];
        }
    }
}
