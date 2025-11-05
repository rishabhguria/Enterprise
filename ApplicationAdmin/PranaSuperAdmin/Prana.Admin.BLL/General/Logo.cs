namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Logo.
    /// </summary>
    public class Logo
    {
        #region Private
        int _logoID = int.MinValue;
        string _logoName = string.Empty;
        byte[] _logoImage = null;
        #endregion
        public Logo()
        {
        }
        public Logo(int logoID, string logoName, byte[] logoImage)
        {
            _logoID = logoID;
            _logoName = logoName;
            _logoImage = logoImage;
        }
        #region Properties

        public int LogoID
        {
            get { return _logoID; }
            set { _logoID = value; }
        }

        public string LogoName
        {
            get { return _logoName; }
            set { _logoName = value; }
        }

        public byte[] LogoImage
        {
            get { return _logoImage; }
            set { _logoImage = value; }
        }
        #endregion
    }
}
