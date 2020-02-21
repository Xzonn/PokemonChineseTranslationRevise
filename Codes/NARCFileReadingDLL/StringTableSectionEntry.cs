// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.StringTableSectionEntry
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.Globalization;
using System.IO;

namespace NARCFileReadingDLL
{
  public class StringTableSectionEntry : IStringTableSectionEntry
  {
    private static readonly StringTableSectionEntry.BinaryString[] BINARY_STRINGS = new StringTableSectionEntry.BinaryString[28]
    {
      new StringTableSectionEntry.BinaryString("\\FFFE\\", "\\n"),
      new StringTableSectionEntry.BinaryString("\\F000\\Ā\\0001\\\\0000\\", "\\Name0\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\Ā\\0001\\\\0001\\", "\\Name1\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\Ă\\0001\\\\0000\\", "\\Pokemon1\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\ā\\0001\\\\0000\\", "\\Pokemon2\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\ć\\0001\\\\0000\\", "\\Move0\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\ć\\0001\\\\0001\\", "\\Move1\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\ć\\0001\\\\0002\\", "\\Move2\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\븅\\0001\\\\0001\\", "\\Speed1\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\븅\\0001\\\\0003\\", "\\Speed3\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\븅\\0001\\\\0005\\", "\\Speed5\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\븅\\0001\\\\0006\\", "\\Speed6\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\븁\\0000\\", "\\(A) Dialog\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\븀\\0000\\", "\\(A)\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\봂\\0001\\\\0000\\", "\\Center\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\봃\\0001\\\\0000\\", "\\Right\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\봀\\0003\\\\0001\\\\0002\\\\0000\\", "\\Default Shadow\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\봀\\0003\\\\0003\\\\0004\\\\0000\\", "\\Cyan Shadow\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\봀\\0003\\\\0005\\\\0006\\\\0000\\", "\\Red Shadow\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\봀\\0003\\\\0007\\\\0008\\\\0000\\", "\\Black Shadow\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\\\FF00\\\\0001\\\\0000\\", "\\Black\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\\\FF00\\\\0001\\\\0001\\", "\\Red\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\\\FF00\\\\0001\\\\0002\\", "\\Blue\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\\\FF00\\\\0001\\\\0003\\", "\\Yellow\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\\\FF00\\\\0001\\\\0004\\", "\\Green\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\븂\\0001\\\\0014\\", "\\Small Delay\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\븉\\0001\\\\0000\\", "\\Unknown0\\"),
      new StringTableSectionEntry.BinaryString("\\F000\\븉\\0001\\\\0001\\", "\\Unknown1\\")
    };
    private string m_strText;
    private ushort m_ushKey;
    private EventHandler m_ehChanged;

    public StringTableSectionEntry(BinaryReader brrReader, ushort ushCharacterCount)
    {
            ReadFrom(brrReader, ushCharacterCount);
    }

    public int Size
    {
      get
      {
        return Length * 2;
      }
    }

    public int Length
    {
      get
      {
        string strText = m_strText;
        int num = strText.Length + 1;
        for (int startIndex = strText.IndexOf('\\'); startIndex >= 0; startIndex = strText.IndexOf('\\', startIndex))
        {
          if (strText[startIndex + 1] == '\\')
          {
            --num;
            startIndex += 2;
          }
          else
          {
            bool flag = false;
            foreach (StringTableSectionEntry.BinaryString binaryString in StringTableSectionEntry.BINARY_STRINGS)
            {
              if (strText.Length >= startIndex + binaryString.String.Length && strText.Substring(startIndex, binaryString.String.Length) == binaryString.String)
              {
                num = num - binaryString.String.Length + binaryString.BinaryLength;
                startIndex += binaryString.String.Length;
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              num -= 5;
              startIndex += 6;
            }
          }
        }
        return num;
      }
    }

    public string Text
    {
      get
      {
        return m_strText;
      }
      set
      {
        if (value == null)
          throw new FormatException();
        foreach (char c in value)
        {
          if ((ushort) char.GetUnicodeCategory(c) == ushort.MaxValue)
            throw new FormatException();
        }
                m_strText = value;
        if (m_ehChanged == null)
          return;
                m_ehChanged(this, new EventArgs());
      }
    }

    public ushort Key
    {
      get
      {
        return m_ushKey;
      }
    }

    public event EventHandler Changed
    {
      add
      {
                m_ehChanged += value;
      }
      remove
      {
                m_ehChanged -= value;
      }
    }

    public void ReadFrom(BinaryReader brrReader, ushort ushCharacterCount)
    {
      if (brrReader.BaseStream.Length - brrReader.BaseStream.Position < ushCharacterCount)
        throw new FormatException();
      ushort[] numArray = new ushort[ushCharacterCount];
      for (int index = 0; index < ushCharacterCount; ++index)
        numArray[index] = brrReader.ReadUInt16();
      ushort num1 = (ushort) (numArray[ushCharacterCount - 1] ^ (uint) ushort.MaxValue);
      for (int index = ushCharacterCount - 1; index > 0; --index)
      {
        numArray[index] ^= num1;
        num1 = (ushort) ((num1 >> 3 | num1 << 13) & ushort.MaxValue);
      }
      numArray[0] ^= num1;
            m_ushKey = num1;
            m_strText = string.Empty;
      for (int index1 = 0; index1 < ushCharacterCount - 1; ++index1)
      {
        if (numArray[index1] == ushort.MaxValue)
          throw new FormatException();
        if (numArray[index1] > 20 && numArray[index1] <= 65520 && (numArray[index1] != 61440 && char.GetUnicodeCategory((char) numArray[index1]) != UnicodeCategory.OtherNotAssigned))
        {
          if (((char) numArray[index1]).ToString() == "\\")
                        m_strText += "\\";
                    m_strText += ((char) numArray[index1]).ToString();
        }
        else
        {
                    m_strText += "\\";
          ushort num2 = numArray[index1];
          for (int index2 = 0; index2 < 4; ++index2)
          {
                        m_strText = num2 % 16 <= 9 ? m_strText.Insert(m_strText.Length - index2, ((char) (num2 % 16 + 48)).ToString()) : m_strText.Insert(m_strText.Length - index2, ((char) (num2 % 16 - 10 + 65)).ToString());
            num2 /= 16;
          }
                    m_strText += "\\";
          foreach (StringTableSectionEntry.BinaryString binaryString in StringTableSectionEntry.BINARY_STRINGS)
          {
            if (m_strText.EndsWith(binaryString.Binary))
            {
                            m_strText = m_strText.Remove(m_strText.Length - binaryString.Binary.Length) + binaryString.String;
              break;
            }
          }
        }
      }
    }

    public void WriteTo(BinaryWriter brwWriter)
    {
      ushort num1 = m_ushKey;
      string str = m_strText;
      for (int startIndex = 0; startIndex < str.Length; ++startIndex)
      {
        ushort num2;
        if (str[startIndex] == '\\')
        {
          ++startIndex;
          if (str[startIndex] == '\\')
          {
            num2 = 92;
          }
          else
          {
            foreach (StringTableSectionEntry.BinaryString binaryString in StringTableSectionEntry.BINARY_STRINGS)
            {
              if (str.Length >= startIndex - 1 + binaryString.String.Length && str.Substring(startIndex - 1, binaryString.String.Length) == binaryString.String)
              {
                str = str.Remove(startIndex - 1, binaryString.String.Length).Insert(startIndex - 1, binaryString.Binary);
                break;
              }
            }
            str = str.Remove(str.IndexOf('\\', startIndex), 1);
            ushort num3 = 0;
            for (int index = 0; index < 3; ++index)
            {
              num3 = (ushort) ((str[startIndex] <= '9' ? (ushort)(num3 + (uint)(ushort)(str[startIndex] - 48U)) : (ushort)(num3 + (uint)(ushort)(str[startIndex] - 65 + 10))) * 16U);
              ++startIndex;
            }
            num2 = str[startIndex] <= '9' ? (ushort) (num3 + (uint) (ushort) (str[startIndex] - 48U)) : (ushort) (num3 + (uint) (ushort) (str[startIndex] - 65 + 10));
          }
        }
        else
          num2 = str[startIndex];
        ushort num4 = (ushort) (num2 ^ (uint) num1);
        num1 = (ushort) ((num1 << 3 | num1 >> 13) & ushort.MaxValue);
        brwWriter.Write(num4);
      }
      brwWriter.Write((ushort) (ushort.MaxValue ^ (uint) num1));
    }

    private struct BinaryString
    {
      private string m_strBinary;
      private string m_strString;

      public BinaryString(string strBinary, string strString)
      {
                m_strBinary = strBinary;
                m_strString = strString;
      }

      public string Binary
      {
        get
        {
          return m_strBinary;
        }
      }

      public string String
      {
        get
        {
          return m_strString;
        }
      }

      public int BinaryLength
      {
        get
        {
          string strBinary = m_strBinary;
          int length = strBinary.Length;
          int startIndex;
          for (int index = strBinary.IndexOf('\\'); index >= 0; index = strBinary.IndexOf('\\', startIndex))
          {
            if (strBinary[index + 1] == '\\')
            {
              --length;
              startIndex = index + 2;
            }
            else
            {
              length -= 5;
              startIndex = index + 6;
            }
          }
          return length;
        }
      }
    }
  }
}
