namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ThirdPartyAccountCommissionRule.
    /// </summary>
    public class ThirdPartyAccountCommissionRule
    {
        #region Private Members
        private int _companyAccountID = int.MinValue;
        private string _accountName = string.Empty;
        private int _companyCounterPartyCVID = int.MinValue;
        private int _cVAUECID = int.MinValue;
        private int _singleRuleID = int.MinValue;
        private int _basketRuleID = int.MinValue;

        private string _singleRuleDescription = string.Empty;
        private string _basketRuleDescription = string.Empty;
        #endregion

        #region Constructors
        public ThirdPartyAccountCommissionRule()
        {
        }
        #endregion

        #region Public Properties
        public int CompanyAccountID
        {
            get { return _companyAccountID; }
            set { _companyAccountID = value; }
        }
        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }
        public int CompanyCounterPartyCVID
        {
            get { return _companyCounterPartyCVID; }
            set { _companyCounterPartyCVID = value; }
        }
        public int CVAUECID
        {
            get { return _cVAUECID; }
            set { _cVAUECID = value; }
        }
        public int SingleRuleID
        {
            get { return _singleRuleID; }
            set { _singleRuleID = value; }
        }
        public int BasketRuleID
        {
            get { return _basketRuleID; }
            set { _basketRuleID = value; }
        }
        public string SingleRuleDescription
        {
            get { return _singleRuleDescription; }
            set { _singleRuleDescription = value; }
        }
        public string BasketRuleDescription
        {
            get { return _basketRuleDescription; }
            set { _basketRuleDescription = value; }
        }
        #endregion
    }
}
