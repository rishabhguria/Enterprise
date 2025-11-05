/* Copyright (C) 2004 - 2007  db4objects Inc.   http://www.db4o.com */

using System;
using System.IO;
//using Db4objects.Db4o;
//using Db4objects.Db4o.Ext;

namespace Prana.Utilities.IO
{
    public class RandomAccessFile : IDisposable
    {
        private FileStream _stream;

#if NET || NET_2_0
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern int FlushFileBuffers(IntPtr fileHandle);
#endif

        public RandomAccessFile(String file, String fileMode)
        {
            try
            {
                _stream = new FileStream(file, FileMode.OpenOrCreate,
                    fileMode.Equals("rwd") ? FileAccess.ReadWrite : FileAccess.Read, FileShare.ReadWrite);
            }
            catch //(IOException x)
            {
                // throw new DatabaseFileLockedException(file,x);
                throw;
            }
        }

        public FileStream Stream
        {
            get { return _stream; }
        }

        public void Close()
        {
            _stream.Close();
        }

        public long Length()
        {
            return _stream.Length;
        }

        public int Read(byte[] bytes, int offset, int length)
        {
            return _stream.Read(bytes, offset, length);
        }

        public int Read(byte[] bytes)
        {
            return _stream.Read(bytes, 0, bytes.Length);
        }

        public void Seek(long pos)
        {
            _stream.Seek(pos, SeekOrigin.Begin);
        }

        public void Sync()
        {
            _stream.Flush();

#if NET_2_0 
            FlushFileBuffers(_stream.SafeFileHandle.DangerousGetHandle());
#elif NET
            FlushFileBuffers(_stream.Handle);
#endif

        }

        public RandomAccessFile GetFD()
        {
            return this;
        }

        public void Write(byte[] bytes)
        {
            Write(bytes, 0, bytes.Length);
            _stream.Flush();
        }

        public void Write(byte[] bytes, int offset, int length)
        {
            try
            {
                _stream.Write(bytes, offset, length);
                _stream.Flush();
            }
            catch //(System.NotSupportedException e)
            {
                throw;
            }
        }

        public long getFilePointer()
        {

            return _stream.Position;
        }



        public void writeUTF(string msg)
        {
            byte[] data = StringByteConversion.GetBytes(msg);
            Write(data);
        }

        public string readUTF()
        {
            byte[] data = new byte[_stream.Length];
            Read(data);
            return StringByteConversion.GetString(data);

        }
        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                _stream.Close();
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
