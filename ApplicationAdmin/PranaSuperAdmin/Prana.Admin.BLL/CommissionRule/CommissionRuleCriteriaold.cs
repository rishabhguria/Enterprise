namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CommissionRuleCriteria.
    /// </summary>
    public class CommissionRuleCriteriaold
    {
        #region Private Members
        private int _rankID = int.MinValue;
        private int _commRulecriteriaID = int.MinValue;
        private int _commCriteriaID = int.MinValue;
        private int _operatorID1 = int.MinValue;
        private int _value1 = 0;
        private int _commissionRateID1 = int.MinValue;
        private double _commissionRate1 = 0;
        private int _operatorID2 = int.MinValue;
        private int _value2 = 0;
        private int _commissionRateID2 = int.MinValue;
        private float _commissionRate2 = 0;
        private int _operatorID3 = int.MinValue;
        private int _value3 = 0;
        private int _commissionRateID3 = int.MinValue;
        private float _commissionRate3 = 0;
        #endregion
        public CommissionRuleCriteriaold()
        {

        }
        #region Public Members
        public int RankID
        {
            get { return _rankID; }
            set { _rankID = value; }
        }
        public int CommissionRuleCriteriaID
        {
            get { return _commRulecriteriaID; }
            set { _commRulecriteriaID = value; }
        }
        public int CommissionCriteriaID_FK
        {
            get { return _commCriteriaID; }
            set { _commCriteriaID = value; }
        }

        public int OperatorID_FK1
        {
            get { return _operatorID1; }
            set { _operatorID1 = value; }
        }
        public int Value1
        {
            get { return _value1; }
            set { _value1 = value; }
        }
        public int CommissionRateID_FK1
        {
            get { return _commissionRateID1; }
            set { _commissionRateID1 = value; }
        }
        public double CommisionRate1
        {
            get { return _commissionRate1; }
            set { _commissionRate1 = value; }
        }
        public int OperatorID_FK2
        {
            get { return _operatorID2; }
            set { _operatorID2 = value; }
        }
        public int Value2
        {
            get { return _value2; }
            set { _value2 = value; }
        }
        public int CommissionRateID_FK2
        {
            get { return _commissionRateID2; }
            set { _commissionRateID2 = value; }
        }
        public float CommisionRate2
        {
            get { return _commissionRate2; }
            set { _commissionRate2 = value; }
        }
        public int OperatorID_FK3
        {
            get { return _operatorID3; }
            set { _operatorID3 = value; }
        }
        public int Value3
        {
            get { return _value3; }
            set { _value3 = value; }
        }
        public int CommissionRateID_FK3
        {
            get { return _commissionRateID3; }
            set { _commissionRateID3 = value; }
        }
        public float CommisionRate3
        {
            get { return _commissionRate3; }
            set { _commissionRate3 = value; }
        }

        #endregion
    }
}
