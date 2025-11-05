using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Prana.PM.MultiFormatParser
{
    [Formatting(DataSourceFileFormat.Text)]
    class TextReadingStrategy : FileFormatStrategy
    {
        public override DataTable GetDataTableFromUploadedDataFile(string fileName)
        {
            DataTable result = new DataTable();

            return result;
        }
    }
}
