using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public sealed class OtherFeeRule
    {
        private Guid _ruleId = Guid.Empty;
        public Guid RuleID
        {
            get { return _ruleId; }
            set { _ruleId = value; }
        }

        private OtherFeeType _otherFeeType;

        public OtherFeeType OtherFeeType
        {
            get { return _otherFeeType; }
            set { _otherFeeType = value; }
        }

        private PositionType _positionSide;
        /// <summary>
        /// This can contain just 2 sides.
        /// PositionType.Long
        /// PositionType.Short
        /// </summary>
        public PositionType PositionSide
        {
            get { return _positionSide; }
            set { _positionSide = value; }
        }

        private CalculationFeeBasis _longCalculationBasis;

        public CalculationFeeBasis LongCalculationBasis
        {
            get { return _longCalculationBasis; }
            set { _longCalculationBasis = value; }
        }

        private CalculationFeeBasis _shortCalculationBasis;

        public CalculationFeeBasis ShortCalculationBasis
        {
            get { return _shortCalculationBasis; }
            set { _shortCalculationBasis = value; }
        }

        private double _longRate;

        public double LongRate
        {
            get { return _longRate; }
            set { _longRate = value; }
        }

        private double _shortRate;

        public double ShortRate
        {
            get { return _shortRate; }
            set { _shortRate = value; }
        }

        private short _roundOffPrecision = 2;
        /// <summary>
        /// digits stored here.
        /// </summary>
        public short RoundOffPrecision
        {
            get { return _roundOffPrecision; }
            set { _roundOffPrecision = value; }
        }

        private double _minValue;

        public double MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        private double _maxValue;

        public double MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        private int _auecID;

        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private int _roundUpPrecision;
        public int RoundUpPrecision
        {
            get { return _roundUpPrecision; }
            set { _roundUpPrecision = value; }
        }

        private int _roundDownPrecision;
        public int RoundDownPrecision
        {
            get { return _roundDownPrecision; }
            set { _roundDownPrecision = value; }
        }

        private FeePrecisionType _feePrecisionType;
        public FeePrecisionType FeePrecisionType
        {
            get { return _feePrecisionType; }
            set { _feePrecisionType = value; }
        }

        private bool _isCriteriaApplied = false;
        public bool IsCriteriaApplied
        {
            get { return _isCriteriaApplied; }
            set { _isCriteriaApplied = value; }
        }

        private List<OtherFeesCriteria> _longFeeRuleCriteriaList;
        public List<OtherFeesCriteria> LongFeeRuleCriteriaList
        {
            get { return _longFeeRuleCriteriaList; }
            set { _longFeeRuleCriteriaList = value; }

        }

        private List<OtherFeesCriteria> _shortFeeRuleCriteriaList;
        public List<OtherFeesCriteria> ShortFeeRuleCriteriaList
        {
            get { return _shortFeeRuleCriteriaList; }
            set { _shortFeeRuleCriteriaList = value; }

        }

    }
}
