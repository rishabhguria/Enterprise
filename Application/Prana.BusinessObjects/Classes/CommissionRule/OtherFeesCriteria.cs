namespace Prana.BusinessObjects
{
    public class OtherFeesCriteria
    {
        private int _otherFeesCriteriaId = int.MinValue;
        public int OtherFeesCriteriaId
        {
            get { return _otherFeesCriteriaId; }
            set { _otherFeesCriteriaId = value; }
        }

        private double _longValueGreaterThan = double.MinValue;
        public double LongValueGreaterThan
        {
            get { return _longValueGreaterThan; }
            set { _longValueGreaterThan = value; }
        }

        private double _longValueLessThanOrEqual = double.MaxValue;
        public double LongValueLessThanOrEqual
        {
            get { return _longValueLessThanOrEqual; }
            set { _longValueLessThanOrEqual = value; }
        }

        private double _longFeeRate = double.MinValue;
        public double LongFeeRate
        {
            get { return _longFeeRate; }
            set { _longFeeRate = value; }
        }

        private int _longCalculationBasis;
        public int LongCalculationBasis
        {
            get { return _longCalculationBasis; }
            set { _longCalculationBasis = value; }
        }

        private string _longfeeCriteriaUnit;
        /// <summary>
        /// Whether it is per share, per contract, basis point
        /// </summary>
        public string LongFeeCriteriaUnit
        {
            get { return _longfeeCriteriaUnit; }
            set { _longfeeCriteriaUnit = value; }
        }

        private double _shortValueGreaterThan = double.MinValue;
        public double ShortValueGreaterThan
        {
            get { return _shortValueGreaterThan; }
            set { _shortValueGreaterThan = value; }
        }

        private double _shortValueLessThanOrEqual = double.MaxValue;
        public double ShortValueLessThanOrEqual
        {
            get { return _shortValueLessThanOrEqual; }
            set { _shortValueLessThanOrEqual = value; }
        }

        private double _shortFeeRate = double.MinValue;
        public double ShortFeeRate
        {
            get { return _shortFeeRate; }
            set { _shortFeeRate = value; }
        }

        private int _shortCalculationBasis;
        public int ShortCalculationBasis
        {
            get { return _shortCalculationBasis; }
            set { _shortCalculationBasis = value; }
        }

        private string _shortfeeCriteriaUnit;
        /// <summary>
        /// Whether it is per share, per contract, basis point
        /// </summary>
        public string ShortFeeCriteriaUnit
        {
            get { return _shortfeeCriteriaUnit; }
            set { _shortfeeCriteriaUnit = value; }
        }
    }
}
