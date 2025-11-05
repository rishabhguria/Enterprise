namespace Prana.Admin.BLL.General
{
    /// <summary>
    /// Summary description for Flag.
    /// </summary>
    public class Flag
    {
        #region Private
        int _countryFlagID = int.MinValue;
        string _countryFlagName = string.Empty;
        byte[] _countryFlagImage = null;
        #endregion

        public Flag()
        {
        }

        #region Properties

        public int CountryFlagID
        {
            get { return _countryFlagID; }
            set { _countryFlagID = value; }
        }

        public string CountryFlagName
        {
            get { return _countryFlagName; }
            set { _countryFlagName = value; }
        }

        public byte[] CountryFlagImage
        {
            get { return _countryFlagImage; }
            set { _countryFlagImage = value; }
        }
        #endregion
    }
}
