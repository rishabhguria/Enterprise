using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class RiskParamameter
    {
        private bool _isstddevRequired = false;
        public bool IsstddevRequired
        {
            get { return _isstddevRequired; }
            set { _isstddevRequired = value; }
        }

        private bool _isMarginalRiskRequired = false;
        public bool IsMarginalRiskRequired
        {
            get { return _isMarginalRiskRequired; }
            set { _isMarginalRiskRequired = value; }
        }

        private bool _isRiskRequired = false;
        public bool IsRiskRequired
        {
            get { return _isRiskRequired; }
            set { _isRiskRequired = value; }
        }

        private bool _isComponentRiskRequired = false;
        public bool IsComponentRiskRequired
        {
            get { return _isComponentRiskRequired; }
            set { _isComponentRiskRequired = value; }
        }

        private bool _isCorrelationRequired = false;
        public bool IsCorrelationRequired
        {
            get { return _isCorrelationRequired; }
            set { _isCorrelationRequired = value; }
        }

        private bool _isVolatilityRequired = false;
        public bool IsVolatilityRequired
        {
            get { return _isVolatilityRequired; }
            set { _isVolatilityRequired = value; }
        }

        private bool _isImpactRequired = false;
        public bool IsImpactRequired
        {
            get { return _isImpactRequired; }
            set { _isImpactRequired = value; }
        }

        private bool _isBetaRequired = false;
        public bool IsBetaRequired
        {
            get { return _isBetaRequired; }
            set { _isBetaRequired = value; }
        }
    }
}
