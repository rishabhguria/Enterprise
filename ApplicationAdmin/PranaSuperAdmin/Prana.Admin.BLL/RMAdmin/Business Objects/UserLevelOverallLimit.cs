using System;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for UserLevelOverallLimit.
    /// </summary>
    public class UserLevelOverallLimit
    {
        #region Private methods

        private int _rMCompanyUserID = int.MinValue;
        private int _companyUserID = int.MinValue;
        private Int64 _userExposureLimit = Int64.MinValue;
        private Int64 _maximumPNLLoss = Int64.MinValue;
        private Int64 _maximumSizePerOrder = Int64.MinValue;
        private Int64 _maximumSizePerBasket = Int64.MinValue;
        private int _companyID = int.MinValue;
        private string _shortName = string.Empty;

        #endregion

        #region Constructors

        public UserLevelOverallLimit(int rMCompanyUserID, int companyUserID, int userExposureLimit,
                                                int maximumPNLLoss, int maximumSizePerOrder, int maximumSizePerBasket,
                                                int companyID, string shortName)
        {
            _rMCompanyUserID = rMCompanyUserID;
            _companyUserID = companyUserID;
            _userExposureLimit = userExposureLimit;
            _maximumPNLLoss = maximumPNLLoss;
            _maximumSizePerOrder = maximumSizePerOrder;
            _maximumSizePerBasket = maximumSizePerBasket;
            _companyID = companyID;
            _shortName = shortName;
        }

        public UserLevelOverallLimit()
        {

        }

        #endregion

        #region Properties
        public int RMCompanyUserID
        {
            get { return _rMCompanyUserID; }
            set { _rMCompanyUserID = value; }
        }
        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }
        public Int64 UserExposureLimit
        {
            get { return _userExposureLimit; }
            set { _userExposureLimit = value; }
        }
        public Int64 MaximumPNLLoss
        {
            get { return _maximumPNLLoss; }
            set { _maximumPNLLoss = value; }
        }
        public Int64 MaximumSizePerOrder
        {
            get { return _maximumSizePerOrder; }
            set { _maximumSizePerOrder = value; }
        }
        public Int64 MaximumSizePerBasket
        {
            get { return _maximumSizePerBasket; }
            set { _maximumSizePerBasket = value; }
        }
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        public string ShortName
        {
            get { return this._shortName; }
            set { this._shortName = value; }
        }
        #endregion
    }
}
