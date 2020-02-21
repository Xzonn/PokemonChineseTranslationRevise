// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.NARCFileFrame
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.IO;

namespace NARCFileReadingDLL
{
  public abstract class NARCFileFrame : INintendoItem
  {
    public abstract string Magic { get; }

    protected abstract int ContentSize { get; }

    public int Size
    {
      get
      {
        return 8 + ContentSize;
      }
    }

        public static NARCFileFrame ReadFrom(BinaryReader brrReader, FATBFrame fatfFATB = null)
        {
            string str = Global.ReplaceOrder(new string(brrReader.ReadChars(4)));
            foreach (char ch in str)
            {
                if (ch < 'A' || ch > 'Z')
                    throw new FormatException();
            }
            int num = brrReader.ReadInt32() - 4 - 4;
            if (num < 0)
                throw new FormatException();
            return (NARCFileFrame)Activator.CreateInstance(Type.GetType("NARCFileReadingDLL." + str + "Frame"), brrReader, num, fatfFATB);
        }

    protected abstract void WriteContentTo(BinaryWriter brwWriter);

    public void WriteTo(BinaryWriter brwWriter)
    {
      brwWriter.Write(Global.ReplaceOrder(Magic).ToCharArray());
      brwWriter.Write(Size);
            WriteContentTo(brwWriter);
    }
  }
}
