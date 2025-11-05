using System;

namespace Prana.Utilities.IO
{
    public class DataOutputStream : IOutputStream, IDisposable
    {
        private IOutputStream _stream;
        System.IO.BinaryWriter bw;
        public DataOutputStream(IOutputStream stream)
        {
            bw = new System.IO.BinaryWriter(stream.getBaseStream());
            _stream = stream;
        }

        //public DataOutputStream(IOutputStream stream, int bufferSize)
        //{
        //    _stream = stream;
        //}

        public void Write(int i)
        {
            bw.Write(i);
        }

        public void Write(byte[] bytes)
        {
            bw.Write(bytes);
        }

        public void Write(byte[] bytes, int offset, int length)
        {
            bw.Write(bytes, offset, length);
        }

        public void Flush()
        {
            bw.Flush();
        }

        public void Close()
        {
            bw.Close();


        }

        public void writeInt64(Int64 value)
        {
            bw.Write(value);
        }

        public void writeInt(int size)
        {
            bw.Write(size);
        }

        public void writeLong(long offset)
        {
            bw.Write(offset);

        }

        #region IOutputStream Members


        public System.IO.Stream getBaseStream()
        {
            return _stream.getBaseStream();
        }

        #endregion
        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                bw.Close();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
