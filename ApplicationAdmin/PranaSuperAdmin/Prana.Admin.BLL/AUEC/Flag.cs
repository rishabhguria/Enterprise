namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Flag.
    /// </summary>
    public class Flag
    {
        #region Private members

        private int _countryFlagID = int.MinValue;
        private string _countryFlagName = string.Empty;
        private byte[] _countryFlagImage = null;

        #endregion

        #region Counstructors
        public Flag()
        {
        }
        public Flag(int countryFlagID, string countryFlagName, byte[] countryFlagImage)
        {
            _countryFlagID = countryFlagID;
            _countryFlagName = countryFlagName;
            _countryFlagImage = countryFlagImage;
        }
        #endregion

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
