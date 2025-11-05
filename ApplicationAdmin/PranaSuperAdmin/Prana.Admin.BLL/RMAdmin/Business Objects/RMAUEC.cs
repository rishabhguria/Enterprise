using System;

namespace Prana.Admin.BLL
{
    public class RMAUEC
    {
        #region Private Methods

        private int _rMAUECID = int.MinValue;
        private int _aUECID = int.MinValue;
        private Int64 _exposureLimitRMBaseCurrency = Int64.MinValue;
        private Int64 _exposureLimitBaseCurrency = Int64.MinValue;
        private Int64 _maximumPNLLossRMBaseCurrency = Int64.MinValue;
        private Int64 _maximumPNLLossBaseCurrency = Int64.MinValue;
        private int _companyID = int.MinValue;
        private string _auec = string.Empty;


        #endregion Private Methods

        #region Constructors
        public RMAUEC()
        {

        }

        public RMAUEC(int rMAUECID, int aUECID, int exposureLimitRMBaseCurrency, int exposureLimitBaseCurrency,
            int maximumPNLLossRMBaseCurrency, int maximumPNLLossBaseCurrency, int companyID)
        {
            _rMAUECID = rMAUECID;
            _aUECID = aUECID;
            _exposureLimitRMBaseCurrency = exposureLimitRMBaseCurrency;
            _exposureLimitBaseCurrency = exposureLimitBaseCurrency;
            _maximumPNLLossRMBaseCurrency = maximumPNLLossRMBaseCurrency;
            _maximumPNLLossBaseCurrency = maximumPNLLossBaseCurrency;
            _companyID = companyID;

        }
        #endregion Constructors

        #region Properties

        public int RMAUECID
        {
            get { return _rMAUECID; }
            set { _rMAUECID = value; }
        }
        public int AUECID
        {
            get { return _aUECID; }
            set { _aUECID = value; }
        }
        public Int64 ExposureLimit_RMBaseCurrency
        {
            get { return _exposureLimitRMBaseCurrency; }
            set { _exposureLimitRMBaseCurrency = Convert.ToInt64(value); }
        }
        public Int64 ExposureLimit_BaseCurrency
        {
            get { return _exposureLimitBaseCurrency; }
            set { _exposureLimitBaseCurrency = Convert.ToInt64(value); }
        }
        public Int64 MaximumPNLLoss_RMBaseCurrency
        {
            get { return _maximumPNLLossRMBaseCurrency; }
            set { _maximumPNLLossRMBaseCurrency = Convert.ToInt64(value); }
        }
        public Int64 MaximumPNLLoss_BaseCurrency
        {
            get { return _maximumPNLLossBaseCurrency; }
            set { _maximumPNLLossBaseCurrency = Convert.ToInt64(value); }
        }
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        public string AUEC
        {
            get { return _auec; }
            set { _auec = value; }
        }

        #endregion Properties
    }
}
