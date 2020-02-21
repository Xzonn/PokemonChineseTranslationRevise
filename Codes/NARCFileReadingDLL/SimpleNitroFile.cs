// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.SimpleNitroFile
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.IO;

namespace NARCFileReadingDLL
{
  public class SimpleNitroFile : NitroFileBase
  {
    private string m_strMagic;

    public SimpleNitroFile()
    {
    }

    public SimpleNitroFile(BinaryReader brrReader)
      : base(brrReader)
    {
    }

    public override string Magic
    {
      get
      {
        return m_strMagic;
      }
      set
      {
        if (value == null)
          throw new FormatException();
        if (value.Length != 4)
          throw new FormatException();
        foreach (char ch in value)
        {
          if (ch < 'A' || ch > 'Z')
            throw new FormatException();
        }
                m_strMagic = value;
      }
    }
  }
}
