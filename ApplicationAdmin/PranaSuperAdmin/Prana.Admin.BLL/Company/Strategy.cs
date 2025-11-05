namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Strategy.
    /// </summary>
    public class Strategy
    {
        #region Private members

        private int _strategyID = int.MinValue;
        private string _strategyName = string.Empty;
        private string _strategyShortName = string.Empty;
        private int _companyID = int.MinValue;

        private int _companyStrategyID = int.MinValue;
        private int _companyUserStrategyID = int.MinValue;

        #endregion

        #region Constructors
        public Strategy()
        {
        }

        public Strategy(int strategyID, string strategyName)
        {
            _strategyID = strategyID;
            _strategyName = strategyName;
        }
        #endregion

        #region Properties

        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        public string StrategyName
        {
            get { return _strategyName; }
            set { _strategyName = value; }
        }

        public string StrategyShortName
        {
            get { return _strategyShortName; }
            set { _strategyShortName = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        public int CompanyStrategyID
        {
            get { return _companyStrategyID; }
            set { _companyStrategyID = value; }
        }

        public int CompanyUserStrategyID
        {
            get { return _companyUserStrategyID; }
            set { _companyUserStrategyID = value; }
        }
        #endregion
    }
}
