using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects
{
    public class CommissionRuleCriteria
    {
        public CommissionRuleCriteria()
        {

        }

        private int _commissionCriteriaId = int.MinValue;
        public int CommissionCriteriaId
        {
            get { return _commissionCriteriaId; }
            set { _commissionCriteriaId = value; }
        }

        private double _valueGreaterThan = double.MinValue;
        public double ValueGreaterThan
        {
            get { return _valueGreaterThan; }
            set { _valueGreaterThan = value; }
        }

        private double _valueLessThanOrEqual = double.MaxValue;
        public double ValueLessThanOrEqual
        {
            get { return _valueLessThanOrEqual; }
            set { _valueLessThanOrEqual = value; }
        }

        private string _commissionCriteriaUnit;
        /// <summary>
        /// Whether it is per share, per contract, basis point
        /// </summary>
        public string CommissionCriteriaUnit
        {
            get { return _commissionCriteriaUnit; }
            set { _commissionCriteriaUnit = value; }
        }

        private double _commissionRate = double.MinValue;
        public double CommissionRate
        {
            get { return _commissionRate; }
            set { _commissionRate = value; }
        }

        private CommissionType _commissionType;
        public CommissionType CommissionType
        {
            get { return _commissionType; }
            set { _commissionType = value; }
        }
    }
}
