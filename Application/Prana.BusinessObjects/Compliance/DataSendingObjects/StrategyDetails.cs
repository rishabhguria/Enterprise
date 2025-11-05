namespace Prana.BusinessObjects.Compliance.DataSendingObjects
{
    /// <summary>
    /// Definition of StrategyDetails
    /// Used to send data to esper
    /// </summary>
    public class StrategyDetails
    {
        int _strategyId;

        public int StrategyId
        {
            get { return _strategyId; }
            set { _strategyId = value; }
        }
        string _strategyName = string.Empty;

        public string StrategyName
        {
            get { return _strategyName; }
            set { _strategyName = value; }
        }
        string _StrategyFullName = string.Empty;

        public string StrategyFullName
        {
            get { return _StrategyFullName; }
            set { _StrategyFullName = value; }
        }

        int _masterStrategyId;

        public int MasterStrategyId
        {
            get { return _masterStrategyId; }
            set { _masterStrategyId = value; }
        }
        string _masterStrategyName = string.Empty;

        public string MasterStrategyName
        {
            get { return _masterStrategyName; }
            set { _masterStrategyName = value; }
        }
        int companyId;

        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }
    }
}
