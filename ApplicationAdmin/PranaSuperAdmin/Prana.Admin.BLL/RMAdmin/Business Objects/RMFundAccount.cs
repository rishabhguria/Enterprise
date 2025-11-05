using System;


namespace Prana.Admin.BLL
{
    public class RMFundAccount
    {
        #region Private Methods

        private int _companyFundAccntRMID = int.MinValue;

        private Int64 _exposureLimitRMBaseCurrency = Int64.MinValue;
        private int _companyID = int.MinValue;
        private Int64 _fAPositivePNL = Int64.MinValue;
        private Int64 _fANegativePNL = Int64.MinValue;
        private int _companyFundAccntID = int.MinValue;
        private string _fundAccount = string.Empty;

        #endregion Private Methods

        #region Constructors

        public RMFundAccount()
        {

        }

        public RMFundAccount(int companyAccountAccntRMID, Int64 exposureLimitRMBaseCurrency, int companyID,
            Int64 fAPositivePNL, Int64 fANegativePNL, int companyAccountAccntID)
        {
            _companyFundAccntRMID = companyAccountAccntRMID;
            _exposureLimitRMBaseCurrency = exposureLimitRMBaseCurrency;
            _companyID = companyID;
            _fAPositivePNL = fAPositivePNL;
            _fANegativePNL = fANegativePNL;
            _companyFundAccntID = companyAccountAccntID;

        }
        #endregion Constructors

        #region Properties

        public int CompanyFundAccntRMID
        {
            get { return _companyFundAccntRMID; }
            set { _companyFundAccntRMID = value; }
        }

        public Int64 ExposureLimit_RMBaseCurrency
        {
            get { return _exposureLimitRMBaseCurrency; }
            set { _exposureLimitRMBaseCurrency = value; }
        }
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        public Int64 Positive_PNL_Loss
        {
            get { return _fAPositivePNL; }
            set { _fAPositivePNL = value; }
        }
        public Int64 Negative_PNL_Loss
        {
            get { return _fANegativePNL; }
            set { _fANegativePNL = value; }
        }
        public int CompanyFundAccntID
        {
            get { return _companyFundAccntID; }
            set { _companyFundAccntID = value; }
        }
        public string FundAccount
        {
            get { return _fundAccount; }
            set { _fundAccount = value; }
        }

        #endregion Properties
    }
}
