using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class ImportSetup
    {
        private DataSourceNameID _dataSourceNameID;
        private ImportMethod _importMethod;
        private ImportFormat _importFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportSetup"/> class.
        /// Sugandh - The data initialized in importMethod and ImportFormat is dummy as of now. 
        /// </summary>
        public ImportSetup()
        {
            DataSourceNameID = new DataSourceNameID();

            //Sugandh =  For getting dummy data For Now.
            ImportMethod = new ImportMethod(1, "ftp");
            ImportFormat = new ImportFormat(1, "xls");

        }

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public DataSourceNameID DataSourceNameID
        {
            get
            {
                if (_dataSourceNameID == null)
                {
                    _dataSourceNameID = new DataSourceNameID();
                }
                return _dataSourceNameID;
            }
            set
            {
                _dataSourceNameID = value;
            }
        }

        /// <summary>
        /// Gets or sets the import method.
        /// </summary>
        /// <value>The import method.</value>
        public ImportMethod ImportMethod
        {
            get
            {
                if (_importMethod == null)
                {
                    _importMethod = new ImportMethod();
                }
                return _importMethod;
            }
            set
            {
                _importMethod = value;
            }
        }

        public ImportFormat ImportFormat
        {
            get
            {
                if (_importFormat == null)
                {
                    _importFormat = new ImportFormat();
                }
                return _importFormat;
            }
            set
            {
                _importFormat = value;
            }
        }


    }
}
