namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Rule.
    /// </summary>
    public class Rule
    {
        public Rule()
        {
        }
        public Rule(int applyruleID, string tradeType)
        {
            _applyruleID = applyruleID;
            _tradeType = tradeType;
        }


        #region Private Members
        private int _applyruleID = int.MinValue;

        private string _tradeType = string.Empty;
        #endregion


        #region Public Properties
        public int ApplyRuleID
        {
            get { return _applyruleID; }
            set { _applyruleID = value; }
        }

        public string TradeType
        {
            get { return _tradeType; }
            set { _tradeType = value; }
        }
        #endregion

    }
}
