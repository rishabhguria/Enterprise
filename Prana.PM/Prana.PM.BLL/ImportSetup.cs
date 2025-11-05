
using Csla;
using Prana.BusinessObjects.PositionManagement;

namespace Prana.PM.BLL
{
    public class ImportSetup : BusinessBase<ImportSetup>
    {
        private ThirdPartyNameID _dataSourceNameID;
        //private ImportMethod _importMethod;
        //private ImportFormat _importFormat;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportSetup"/> class.
        /// Sugandh - The data initialized in importMethod and ImportFormat is dummy as of now. 
        /// </summary>
        public ImportSetup()
        {
            MarkAsChild();
        }

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public ThirdPartyNameID DataSourceNameID
        {
            get
            {
                if (_dataSourceNameID == null)
                {
                    _dataSourceNameID = new ThirdPartyNameID();
                }
                return _dataSourceNameID;
            }
            set
            {
                _dataSourceNameID = value;
            }
        }

        private ImportMethod _importMethod = ImportMethod.ftp;

        /// <summary>
        /// Gets or sets the import method.
        /// </summary>
        /// <value>The import method.</value>
        public ImportMethod ImportMethod
        {
            get
            {
                return _importMethod;
            }
            set
            {
                _importMethod = value;
            }
        }

        private ImportFormat _importFormat = ImportFormat.xls;

        /// <summary>
        /// Gets or sets the import format.
        /// </summary>
        /// <value>The import format.</value>
        public ImportFormat ImportFormat
        {
            get
            {
                return _importFormat;
            }
            set
            {
                _importFormat = value;
            }
        }

        private int _id;
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The ID.</value>
        //[Browsable(false)]
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                PropertyHasChanged();
            }
        }

        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _id;
        }

    }
}
