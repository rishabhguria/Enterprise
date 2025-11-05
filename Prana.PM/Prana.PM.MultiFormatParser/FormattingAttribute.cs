using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.PM.MultiFormatParser
{

    /// <summary>
    /// Enum keeping all the type of file layouts which we can get. 
    /// </summary>
    public enum DataSourceFileFormat : ushort
    {
        Excel = 1,
        Csv = 2,
        Text = 3
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FormattingAttribute : Attribute
    {
        private DataSourceFileFormat _dataSourceFileFormat;

        public FormattingAttribute(DataSourceFileFormat dataSourceFileFormat)
        {
            this._dataSourceFileFormat = dataSourceFileFormat;
        }

        public DataSourceFileFormat DataSourceFileFormat
        {
            get { return _dataSourceFileFormat; }
        }
    }
}
