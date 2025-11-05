/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

using System.IO;

namespace Prana.Utilities.IO
{

    public class FileOutputStream : OutputStream
    {

        public FileOutputStream(File file)
            : base(new FileStream(file.GetPath(), FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
        {
        }
        public FileOutputStream(string fileName)
            : base(new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
        {
        }


        internal FileOutputStream getFD()
        {
            return this;
        }

        internal void sync()
        {

        }
    }
}
