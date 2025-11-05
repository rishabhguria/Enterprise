/* Copyright (C) 2004   db4objects Inc.   http://www.db4o.com */

using System.IO;

namespace Prana.Utilities.IO
{

    public class FileInputStream : InputStream
    {

        public FileInputStream(File file)
            : base(new FileStream(file.GetPath(), FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
        {
        }

    }
}
