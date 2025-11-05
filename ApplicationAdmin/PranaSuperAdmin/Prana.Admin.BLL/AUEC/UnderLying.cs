namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for UnderLying.
    /// </summary>
    public class UnderLying
    {
        #region Private members

        private int _underlyingID = int.MinValue;
        private string _underlyingName = string.Empty;
        private string _comment = string.Empty;
        private int _assetID = int.MinValue;
        private int _companyID = int.MinValue;

        #endregion

        #region Constructors

        public UnderLying()
        {
        }

        public UnderLying(int underlyingID, string underlyingName)
        {
            _underlyingID = underlyingID;
            _underlyingName = underlyingName;
        }

        public UnderLying(int underlyingID, string underlyingName, int assetID)
        {
            _underlyingID = underlyingID;
            _underlyingName = underlyingName;
            _assetID = assetID;
        }

        #endregion

        #region Properties

        public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }

        public string Name
        {
            get { return _underlyingName; }
            set { _underlyingName = value; }
        }

        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
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

        public string Asset
        {
            get
            {
                string result = string.Empty;
                Asset asset = AssetManager.GetAssets(_assetID);
                if (asset != null)
                {
                    result = asset.Name;
                }
                else
                {
                    result = "";
                }
                return result;
            }
        }

        #endregion
    }
}
