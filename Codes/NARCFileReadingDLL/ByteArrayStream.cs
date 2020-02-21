// Decompiled with JetBrains decompiler
// Type: NARCFileReadingDLL.ByteArrayStream
// Assembly: NARCFileReadingDLL, Version=2.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1D310F7-093C-48EB-B9DF-91020A139DAF
// Assembly location: C:\Users\CHEMI6DER\Downloads\pokefonts\NARCFileReadingDLL.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace NARCFileReadingDLL
{
    public class ByteArrayStream : Stream
    {
        private List<byte[]> m_array;
        private long m_length;
        private long m_position;
        private int m_positionArrayIndex;
        private int m_positionByteIndex;

        public ByteArrayStream()
        {
            m_array = new List<byte[]>(0);
            m_length = 0L;
            m_position = 0L;
            m_positionArrayIndex = 0;
            m_positionByteIndex = 0;
        }

        public ByteArrayStream(byte[] array)
          : this()
        {
            m_array.Add(array);
            m_length = array.Length;
        }

        public override long Position
        {
            get
            {
                if (m_array == null)
                    throw new ObjectDisposedException("m_array");
                return m_position;
            }
            set
            {
                if (m_array == null)
                    throw new ObjectDisposedException("m_array");
                if (value < 0L || value > Length)
                    throw new ArgumentException();
                if (m_position < value)
                {
                    for (; m_position < value && m_position + (m_array[m_positionArrayIndex].Length - m_positionByteIndex) <= value; m_positionByteIndex = 0)
                    {
                        m_position += m_array[m_positionArrayIndex].Length - m_positionByteIndex;
                        ++m_positionArrayIndex;
                    }
                }
                else
                {
                    for (; m_position - m_positionByteIndex > value; m_positionByteIndex = m_array[m_positionArrayIndex].Length)
                    {
                        m_position -= m_positionByteIndex;
                        --m_positionArrayIndex;
                    }
                }
                m_positionByteIndex += (int)(value - m_position);
                m_position = value;
            }
        }

        public override bool CanRead
        {
            get
            {
                return m_array != null;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return m_array != null;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return m_array != null;
            }
        }

        public override long Length
        {
            get
            {
                if (m_array == null)
                    throw new ObjectDisposedException("m_array");
                return m_length;
            }
        }

        protected override void Dispose(bool disposing)
        {
            m_array = null;
            base.Dispose(disposing);
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException();
            if (offset < 0 || count < 0)
                throw new ArgumentException();
            if (offset > buffer.LongLength || buffer.LongLength - offset < count)
                throw new ArgumentOutOfRangeException();
            for (int index = 0; index < count; ++index)
            {
                if (Position == Length)
                    return index;
                buffer[offset + index] = m_array[m_positionArrayIndex][m_positionByteIndex];
                ++Position;
            }
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    Position = Length + offset;
                    break;
            }
            return Position;
        }

        public override void SetLength(long value)
        {
            if (value < 0L)
                throw new ArgumentException();
            if (m_length < value)
            {
                m_array.Add(new byte[value - m_length]);
            }
            else
            {
                while (m_length - m_array[m_array.Count - 1].Length >= value)
                {
                    m_length -= m_array[m_array.Count - 1].Length;
                    m_array.RemoveAt(m_array.Count - 1);
                }
                byte[] numArray = new byte[m_length - value];
                for (int index = 0; index < numArray.Length; ++index)
                    numArray[index] = m_array[m_array.Count - 1][index];
                m_array.RemoveAt(m_array.Count - 1);
                m_array.Add(numArray);
                if (m_position > value)
                {
                    m_position = value;
                    m_positionArrayIndex = m_array.Count;
                    m_positionByteIndex = 0;
                }
            }
            m_length = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (m_array == null)
                throw new ObjectDisposedException("m_array");
            if (buffer == null)
                throw new ArgumentNullException();
            if (offset < 0 || count < 0)
                throw new ArgumentException();
            if (offset > buffer.LongLength || buffer.LongLength - offset < count)
                throw new ArgumentOutOfRangeException();
            if (count > Length - Position)
                SetLength(Position + count);
            for (int index = 0; index < count; ++index)
            {
                m_array[m_positionArrayIndex][m_positionByteIndex] = buffer[offset + index];
                ++Position;
            }
        }
    }
}
