/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

using System.IO;

namespace Prana.Utilities.IO
{
    public class OutputStream : StreamAdaptor, IOutputStream
    {
        public OutputStream(Stream stream)
            : base(stream)
        {
        }

        public void Write(int i)
        {
            _stream.WriteByte((byte)i);
        }

        public void Write(byte[] bytes)
        {
            _stream.Write(bytes, 0, bytes.Length);
        }

        public void Write(byte[] bytes, int offset, int length)
        {
            _stream.Write(bytes, offset, length);
        }

        public void Flush()
        {
            _stream.Flush();
        }

        #region IOutputStream Members


        public Stream getBaseStream()
        {
            return _stream;
        }

        #endregion
    }
}
