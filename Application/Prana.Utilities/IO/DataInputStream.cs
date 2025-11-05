using System;

namespace Prana.Utilities.IO
{
    public class DataInputStream : IInputStream, IDisposable
    {
        private IInputStream _stream;
        private System.IO.BinaryReader br;

        public DataInputStream(IInputStream stream)
        {

            _stream = stream;
            br = new System.IO.BinaryReader(stream.getBaseStream());
        }



        public int Read()
        {
            return br.Read();
        }

        public int Read(byte[] bytes)
        {
            return br.Read(bytes, 0, bytes.Length);
        }

        public int Read(byte[] bytes, int offset, int length)
        {
            return br.Read(bytes, offset, length);
        }

        public void Close()
        {
            br.Close();
        }

        public int readInt()
        {
            return br.ReadInt32();
        }

        public long readLong()
        {
            return br.ReadInt64();
        }

        public long available()
        {
            return (_stream.getBaseStream().Length - _stream.getBaseStream().Position);
        }

        #region IInputStream Members


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
                br.Close();
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
