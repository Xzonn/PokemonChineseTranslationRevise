// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.Global
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

namespace NARCFileReadingDLL
{
  public static class Global
  {
    public static string ReplaceOrder(string strInput)
    {
      string str = string.Empty;
      foreach (char ch in strInput)
        str = str.Insert(0, ch.ToString());
      return str;
    }
  }
}
