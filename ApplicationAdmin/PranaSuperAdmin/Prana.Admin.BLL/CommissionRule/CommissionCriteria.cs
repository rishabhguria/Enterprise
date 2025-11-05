namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CommissionCriteria.
    /// </summary>
    public class CommissionCriteria
    {
        #region Private Members
        private int _commCriteriaID = int.MinValue;
        private int _ruleID = int.MinValue;
        private int _commissionCalculationID = int.MinValue;
        private double _minCommissionRate = double.MinValue;
        #endregion
        public CommissionCriteria()
        {

        }
        #region Public Members
        public int CommissionCriteriaID
        {
            get { return _commCriteriaID; }
            set { _commCriteriaID = value; }
        }

        public int RuleID_FK
        {
            get { return _ruleID; }
            set { _ruleID = value; }
        }
        public int CommissionCalculationID_FK
        {
            get { return _commissionCalculationID; }
            set { _commissionCalculationID = value; }
        }

        public double MinimumCommissionRate
        {
            get { return _minCommissionRate; }
            set { _minCommissionRate = value; }
        }
        #endregion
        public CommissionCriteria(int commCriteriaID, int ruleID, int commissionCalculationID, double minCommissionRate)
        {
            _commCriteriaID = commCriteriaID;
            _ruleID = ruleID;
            _commissionCalculationID = commissionCalculationID;
            _minCommissionRate = minCommissionRate;

        }
    }
}