// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.INintendoDataItem
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System.IO;

namespace NARCFileReadingDLL
{
  public interface INintendoDataItem : INintendoItem
  {
    void ReadFrom(BinaryReader brrReader);
  }
}
