using System;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyOverallLimit.
    /// </summary>
    public class CompanyOverallLimit
    {
        #region Private members

        private int _rMCompanyOverallLimitID = int.MinValue;
        private int _companyID = int.MinValue;
        private int _rMBaseCurrencyID = int.MinValue;
        private int _calculateRiskLimit = 0;
        private Int64 _exposureLimit = Int64.MinValue;
        private Int64 _positivePNL = Int64.MinValue;
        private Int64 _negativePNL = Int64.MinValue;

        #endregion


        #region Constructors

        public CompanyOverallLimit()
        {

        }
        #endregion


        #region Properties

        public int RMCompanyOverallLimitID
        {
            get { return _rMCompanyOverallLimitID; }
            set { _rMCompanyOverallLimitID = value; }
        }
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        public int RMBaseCurrencyID
        {
            get { return _rMBaseCurrencyID; }
            set { _rMBaseCurrencyID = value; }
        }
        public int CalculateRiskLimit
        {
            get { return _calculateRiskLimit; }
            set { _calculateRiskLimit = value; }
        }
        public Int64 ExposureLimit
        {
            get { return _exposureLimit; }
            set { _exposureLimit = value; }
        }
        public Int64 PositivePNL
        {
            get { return _positivePNL; }
            set { _positivePNL = value; }
        }
        public Int64 NegativePNL
        {
            get { return _negativePNL; }
            set { _negativePNL = value; }
        }

        #endregion
    }
}
