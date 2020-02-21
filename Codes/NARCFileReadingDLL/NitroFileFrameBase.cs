// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.NitroFileFrameBase
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.IO;

namespace NARCFileReadingDLL
{
  public abstract class NitroFileFrameBase : INintendoDataItem, INintendoItem
  {
    private EventHandler m_ehContentChanged;

    public NitroFileFrameBase(BinaryReader brrReader)
    {
            ReadFrom(brrReader);
    }

    public abstract string Magic { get; set; }

    protected abstract byte[] GetContent();

    protected abstract void SetContent(byte[] arrbContent);

    public event EventHandler ContentChanged
    {
      add
      {
                m_ehContentChanged += value;
      }
      remove
      {
                m_ehContentChanged -= value;
      }
    }

    public int Size
    {
      get
      {
        return 8 + ContentSize;
      }
    }

    private int ContentSize
    {
      get
      {
        return GetContent().Length;
      }
    }

    private byte[] Content
    {
      get
      {
        return (byte[])GetContent().Clone();
      }
      set
      {
        if (value == null)
          return;
                SetContent((byte[]) value.Clone());
      }
    }

    public byte[] File
    {
      get
      {
        ByteArrayStream byteArrayStream = new ByteArrayStream();
                WriteTo(new BinaryWriter(byteArrayStream));
        byteArrayStream.Position = 0L;
        return new BinaryReader(byteArrayStream).ReadBytes((int) byteArrayStream.Length);
      }
      set
      {
                ReadFrom(new BinaryReader(new ByteArrayStream(value)));
      }
    }

    public static NitroFileFrameBase CreateFrom(BinaryReader brrReader)
    {
      string str = Global.ReplaceOrder(new string(brrReader.ReadChars(4)));
      foreach (char ch in str)
      {
        if (ch < 'A' || ch > 'Z')
          throw new FormatException();
      }
      brrReader.BaseStream.Position -= 4L;
      Type type = Type.GetType("NARCFileReadingDLL." + str + "Frame");
      NitroFileFrameBase nitroFileFrameBase;
      if (type == null)
        nitroFileFrameBase = new SimpleNitroFileFrame(brrReader);
      else
        nitroFileFrameBase = (NitroFileFrameBase) Activator.CreateInstance(type, new object[1]
        {
           brrReader
        });
      return nitroFileFrameBase;
    }

    public void ReadFrom(BinaryReader brrReader)
    {
      string str = Global.ReplaceOrder(new string(brrReader.ReadChars(4)));
      foreach (char ch in str)
      {
        if (ch < 'A' || ch > 'Z')
          throw new FormatException();
      }
            Magic = str;
            SetContent(brrReader.ReadBytes(brrReader.ReadInt32() - 8));
      if (m_ehContentChanged == null)
        return;
            m_ehContentChanged(this, new EventArgs());
    }

    public void WriteTo(BinaryWriter brwWriter)
    {
      brwWriter.Write(Global.ReplaceOrder(Magic).ToCharArray());
      brwWriter.Write(Size);
      brwWriter.Write(Content);
    }
  }
}
