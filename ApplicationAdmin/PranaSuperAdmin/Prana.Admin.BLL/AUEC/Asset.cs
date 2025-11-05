namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Asset.
    /// </summary>
    public class Asset
    {
        #region Private members

        private int _assetID = int.MinValue;
        private string _assetName = string.Empty;
        private string _comment = string.Empty;
        private int _companyID = int.MinValue;


        private UnderLyings _underLyings = null;

        #endregion

        #region Constructors

        public Asset()
        {
        }

        public Asset(int assetID, string assetName)
        {
            _assetID = assetID;
            _assetName = assetName;
        }

        #endregion

        #region Properties

        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        public string Name
        {
            get { return _assetName; }
            set { _assetName = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        public UnderLyings UnderLyings
        {
            get
            {
                if (_underLyings == null)
                {
                    _underLyings = AssetManager.GetUnderLyings(_assetID);
                }
                return _underLyings;
            }
            set { _underLyings = value; }
        }
        #endregion
    }
}
