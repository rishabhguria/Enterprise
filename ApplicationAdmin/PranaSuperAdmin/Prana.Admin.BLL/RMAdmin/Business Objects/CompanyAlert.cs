namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyAlert.
    /// </summary>
    public class CompanyAlert
    {
        #region Private methods

        private int _rMCompanyAlertID = int.MinValue;
        private int _companyExposureLower = 3;
        private int _companyExposureUpper = 4;
        private int _alertTypeID = int.MinValue;
        private int _refreshRateCalculation = 0;
        private int _rMCompanyOverallLimitID = int.MinValue;
        private string _alertMessage = string.Empty;
        private string _emailAddress = string.Empty;
        private int _blockTrading = int.MinValue;
        private int _companyID = int.MinValue;
        private int _rank = int.MinValue;
        private bool _checkBox = false;

        #endregion

        #region Constructors
        public CompanyAlert()
        {

        }
        #endregion

        #region Properties
        public int RMCompanyAlertID
        {
            get { return _rMCompanyAlertID; }
            set { _rMCompanyAlertID = value; }
        }
        public int CompanyExposureLower
        {
            get { return _companyExposureLower; }
            set { _companyExposureLower = value; }
        }
        public int CompanyExposureUpper
        {
            get { return _companyExposureUpper; }
            set { _companyExposureUpper = value; }
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
        public int RMCompanyOverallLimitID
        {
            get { return _rMCompanyOverallLimitID; }
            set { _rMCompanyOverallLimitID = value; }
        }
        public string AlertMessage
        {
            get { return _alertMessage; }
            set { _alertMessage = value; }
        }
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }
        public int BlockTrading
        {
            get { return _blockTrading; }
            set { _blockTrading = value; }
        }
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        public int Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }
        public bool CheckBox
        {
            get { return _checkBox; }
            set { _checkBox = value; }
        }

        #endregion
    }
}
