using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Utilities.ImportExportUtilities
{

    /// <summary>
    /// Enum keeping all the type of file layouts which we can get. 
    /// </summary>
    public enum DataSourceFileFormat : ushort
    {
        Excel = 0,
        Csv = 1,
        Text = 2,
        FixPlace = 3,
        Default = 4         
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
