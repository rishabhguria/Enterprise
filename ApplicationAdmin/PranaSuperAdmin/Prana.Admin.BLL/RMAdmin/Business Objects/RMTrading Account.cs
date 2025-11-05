using System;

namespace Prana.Admin.BLL
{
    public class RMTradingAccount
    {
        #region Private Method

        private int _companyTradAccntRMID = int.MinValue;
        private int _companyID = int.MinValue;
        private int _companyTradAccntID = int.MinValue;
        private Int64 _tAExposureLimit = Int64.MinValue;
        private Int64 _tAPositivePNL = Int64.MinValue;
        private Int64 _tANegativePNL = Int64.MinValue;
        private string _tradingAccount = string.Empty;

        #endregion Private Method

        #region Constructor

        public RMTradingAccount()
        {

        }

        public RMTradingAccount(int companyTradAccntRMID, int companyID,
            int companyTradAccntID, Int64 tAExposureLimit, Int64 tAPositivePNL, Int64 tANegativePNL, string tradingAccount)
        {
            _companyTradAccntRMID = companyTradAccntRMID;
            _companyID = companyID;
            _companyTradAccntID = companyTradAccntID;
            _tAExposureLimit = tAExposureLimit;
            _tAPositivePNL = tAPositivePNL;
            _tANegativePNL = tANegativePNL;
            _tradingAccount = tradingAccount;
        }
        #endregion Constructor

        #region Properties
        public int CompanyTradAccntRMID
        {
            get { return _companyTradAccntRMID; }
            set { _companyTradAccntRMID = value; }
        }
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        public int CompanyTradingAccountID
        {
            get { return _companyTradAccntID; }
            set { _companyTradAccntID = value; }
        }

        public Int64 ExposureLimit
        {
            get { return _tAExposureLimit; }
            set { _tAExposureLimit = Convert.ToInt64(value); }
        }
        public Int64 PositivePNL
        {
            get { return _tAPositivePNL; }
            set { _tAPositivePNL = Convert.ToInt64(value); }
        }
        public Int64 NegativePNL
        {
            get { return _tANegativePNL; }
            set { _tANegativePNL = Convert.ToInt64(value); }
        }
        public string TradingAccount
        {
            get { return _tradingAccount; }
            set { _tradingAccount = value; }
        }
        #endregion Properties

    }
}
