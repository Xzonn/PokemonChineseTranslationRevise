// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.NFTRNitroFile
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.IO;

namespace NARCFileReadingDLL
{
  public class NFTRNitroFile : NitroFileBase, IFontTable
  {
    public const string MAGIC = "NFTR";
    private const int FINF_INDEX = 0;
    private const int CGLP_INDEX = 1;
    private const int CWDH_INDEX = 2;
    private const int CMAP_FIRST_INDEX = 3;
    private int m_nCMAP_Type_1_FirstIndex;

    public NFTRNitroFile(BinaryReader brrReader)
      : base(brrReader)
    {
            m_nCMAP_Type_1_FirstIndex = 3;
      for (int index = 3; index < FramesCount - 1; ++index)
      {
        if (((CMAPFrame)Frames[index]).Type == 1U)
        {
                    m_nCMAP_Type_1_FirstIndex = index;
          break;
        }
      }
    }

    public override string Magic
    {
      get
      {
        return "NFTR";
      }
      set
      {
        if (value != "NFTR")
          throw new FormatException();
      }
    }

    public IFontTableItem[] Items
    {
      get
      {
        return ((CGLPFrame)Frames[1]).Items;
      }
    }

    public byte Height
    {
      get
      {
        return ((CGLPFrame)Frames[1]).Height;
      }
    }

    public byte MaxWidth
    {
      get
      {
        return ((CGLPFrame)Frames[1]).MaxWidth;
      }
    }

    public byte MaxHeight
    {
      get
      {
        return ((CGLPFrame)Frames[1]).MaxHeight;
      }
    }

    public ushort this[char cValue]
    {
      get
      {
        for (int index = 3; index < FramesCount - 1; ++index)
        {
          if (((CMAPFrame)Frames[index]).ContainsValue(cValue))
            return ((CMAPFrame)Frames[index])[cValue];
        }
        return ((CMAPFrame)Frames[FramesCount - 1])[cValue];
      }
      set
      {
        for (int index = 3; index < FramesCount - 1; ++index)
        {
          if (((CMAPFrame)Frames[index]).ContainsValue(cValue))
          {
            ((CMAPFrame)Frames[index])[cValue] = value;
            break;
          }
        }
        ((CMAPFrame)Frames[FramesCount - 1])[cValue] = value;
      }
    }

    public void AddCMAPFrame(CMAPFrame cmapFrame)
    {
      if (cmapFrame.Type == 0U)
                InsertFrame(m_nCMAP_Type_1_FirstIndex++, cmapFrame);
      else if (cmapFrame.Type == 1U)
                InsertFrame(FramesCount - 1, cmapFrame);
      cmapFrame.UpdateFromNFTRNitroFile(this);
    }

    public void AddNewItem()
    {
      ((CGLPFrame)Frames[1]).AddNewItem();
      if (m_fcFileChanged == null)
        return;
            m_fcFileChanged(this);
    }
  }
}
