namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ThirdPartyAccountCommissionRule.
    /// </summary>

    public class CVAUECAccountCommissionRule
    {
        #region Private Members
        private int _accountID = int.MinValue;
        private string _accountName = string.Empty;

        private int _cvID = int.MinValue;
        string _cvName = string.Empty;
        private int _auecID = int.MinValue;
        string _auecName = string.Empty;

        #endregion

        #region Public Properties

        public int CVID
        {
            get { return _cvID; }
            set { _cvID = value; }
        }

        public string CVName
        {
            get { return _cvName; }
            set { _cvName = value; }
        }

        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        public string AUECName
        {
            get { return _auecName; }
            set { _auecName = value; }
        }


        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }

        private CommissionRule _singleRule = new CommissionRule();

        public CommissionRule SingleRule
        {
            get { return _singleRule; }
            set { _singleRule = value; }
        }

        private CommissionRule _basketRule = new CommissionRule();

        public CommissionRule BasketRule
        {
            get { return _basketRule; }
            set { _basketRule = value; }
        }

        private int _companyID;
        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        private int _cvAUECRuleID = int.MinValue;
        public int CVAUECRuleID
        {
            get { return _cvAUECRuleID; }
            set { _cvAUECRuleID = value; }
        }
        private int _counterPartyId = int.MinValue;
        public int CounterPartyId
        {
            get { return _counterPartyId; }
            set { _counterPartyId = value; }
        }
        private int _venueId = int.MinValue;
        public int VenueId
        {
            get { return _venueId; }
            set { _venueId = value; }
        }

        //public override Guid ToGuid()
        //{
        //    return _ruleName;
        //}

        #endregion
    }
}
