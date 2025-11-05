using System;


namespace Prana.Admin.BLL
{
    public class UserTradingAccount
    {
        #region Private Methods

        private int _rMUserTradingAccntID = int.MinValue;
        private int _companyID = int.MinValue;
        private int _companyUserID = int.MinValue;
        private int _userTradingAccntID = int.MinValue;
        private Int64 _userTAExposureLimit = Int64.MinValue;
        private string _userShortName = string.Empty;
        private string _tradingAccntName = string.Empty;

        #endregion Private Methods

        #region Constructors

        public UserTradingAccount()
        {

        }

        public UserTradingAccount(int rMUserTradingAccntID, int companyID, int companyUserID, int userTradingAccntID,
           Int64 userTAExposureLimit, string userShortName, string tradingAccntName)
        {

            _rMUserTradingAccntID = rMUserTradingAccntID;
            _companyID = companyID;
            _companyUserID = companyUserID;
            _userTradingAccntID = userTradingAccntID;
            _userTAExposureLimit = userTAExposureLimit;
            _userShortName = userShortName;
            _tradingAccntName = tradingAccntName;
        }
        #endregion Constructors

        #region Properties
        public int RMUserTradingAccntID
        {
            get { return _rMUserTradingAccntID; }
            set { _rMUserTradingAccntID = value; }
        }
        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }
        public int UserTradingAccntID
        {
            get { return _userTradingAccntID; }
            set { _userTradingAccntID = value; }
        }
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        public Int64 UserTAExposureLimit
        {
            get { return _userTAExposureLimit; }
            set { _userTAExposureLimit = value; }
        }

        public string TradingAccount
        {
            get { return _tradingAccntName; }
            set { _tradingAccntName = value; }
        }

        public string User
        {
            get { return _userShortName; }
            set { _userShortName = value; }
        }
        #endregion Properties
    }
}
