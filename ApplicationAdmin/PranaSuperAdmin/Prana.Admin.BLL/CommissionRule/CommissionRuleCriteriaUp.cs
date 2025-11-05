using System;

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CommissionRuleCriteriaUp.
    /// </summary>
    public class CommissionRuleCriteriaUp
    {
        #region Private Members      
        private int _commRulecriteriaID = int.MinValue;
        private int _commCriteriaID = int.MinValue;
        private Int64 _valuefrom = 0;
        private Int64 _valueto = 0;
        private int _commissionRateID = int.MinValue;
        private double _commissionRate = 0;
        #endregion
        public CommissionRuleCriteriaUp()
        {

        }
        #region Public Members       
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

        public Int64 ValueFrom
        {
            get { return _valuefrom; }
            set { _valuefrom = value; }
        }

        public Int64 ValueTo
        {
            get { return _valueto; }
            set { _valueto = value; }
        }

        public int CommissionRateID_FK
        {
            get { return _commissionRateID; }
            set { _commissionRateID = value; }
        }
        public double CommisionRate
        {
            get { return _commissionRate; }
            set { _commissionRate = value; }
        }

        #endregion
    }
}
