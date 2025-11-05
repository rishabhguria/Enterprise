namespace Prana.BusinessObjects
{
    public class PriceSymbolValidation
    {
        public PriceSymbolValidation()
        {
            //TODO:Add Constructor Logic  
        }

        #region Private Variables
        private bool _riskControlChecked = false;
        private double _riskValue = double.MinValue;
        private bool _validateSymbolChecked = false;
        private int _companyUserID = int.MinValue;
        private bool _limitPriceChecked = false;
        private bool _setExecutedQtytoZero = false;
        #endregion

        #region Public Properties
        public bool RiskCtrlCheck
        {
            get { return _riskControlChecked; }
            set { _riskControlChecked = value; }
        }
        public double RiskValue
        {
            get { return _riskValue; }
            set { _riskValue = value; }
        }
        public bool ValidateSymbolCheck
        {
            get { return _validateSymbolChecked; }
            set { _validateSymbolChecked = value; }
        }
        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }
        public bool LimitPriceCheck
        {
            get { return _limitPriceChecked; }
            set { _limitPriceChecked = value; }
        }
        public bool SetExecutedQtytoZero
        {
            get { return _setExecutedQtytoZero; }
            set { _setExecutedQtytoZero = value; }
        }
        #endregion

    }
}
