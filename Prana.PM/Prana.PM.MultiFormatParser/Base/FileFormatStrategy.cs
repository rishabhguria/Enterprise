using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Prana.PM.MultiFormatParser
{
    public abstract class FileFormatStrategy
    {
        public abstract DataTable GetDataTableFromUploadedDataFile(string fileName);
    }
}
