namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for DefaultAlert.
    /// </summary>
    public class DefaultAlert
    {
        #region Private methods

        private int _rMDefaultID = int.MinValue;
        private int _alertTypeID = int.MinValue;
        private int _refreshRateCalculation = 0;
        private int _companyID = int.MinValue;

        #endregion

        #region Constructors
        public DefaultAlert()
        {

        }
        #endregion

        #region Properties
        public int RMDefaultID
        {
            get { return _rMDefaultID; }
            set { _rMDefaultID = value; }
        }
        public int AlertTypeID
        {
            get { return _alertTypeID; }
            set { _alertTypeID = value; }
        }
        public int RefreshRateCalculation
        {
            get { return _refreshRateCalculation; }
            set { _refreshRateCalculation = value; }
        }
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        #endregion
    }
}
