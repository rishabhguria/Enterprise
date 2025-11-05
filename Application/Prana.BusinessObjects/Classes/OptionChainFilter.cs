using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class OptionChainFilter
    {
        private DateTime _expirationDate;
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }

        private int _maxNumberOfStrikes;
        public int MaxNumberOfStrikes
        {
            get { return _maxNumberOfStrikes; }
            set { _maxNumberOfStrikes = value; }
        }

        private double _lowerStrike;
        public double LowerStrike
        {
            get { return _lowerStrike; }
            set { _lowerStrike = value; }
        }

        private double _upperStrike;
        public double UpperStrike
        {
            get { return _upperStrike; }
            set { _upperStrike = value; }
        }

        private double _strikeTolerancePercentage;
        public double StrikeTolerancePercentage
        {
            get { return _strikeTolerancePercentage; }
            set { _strikeTolerancePercentage = value; }
        }

        private string _turnAroundID;
        public string TurnAroundID
        {
            get { return _turnAroundID; }
            set { _turnAroundID = value; }
        }

        private AssetCategory _categoryCode;
        public AssetCategory CategoryCode
        {
            get { return _categoryCode; }
            set { _categoryCode = value; }
        }

        private OptionTypeFilter _optionTypeFilter;
        public OptionTypeFilter OptionTypeFilter
        {
            get { return _optionTypeFilter; }
            set { _optionTypeFilter = value; }
        }

        private double _underlyingSymbolLastTradedPrice;
        public double UnderlyingSymbolLastTradedPrice
        {
            get { return _underlyingSymbolLastTradedPrice; }
            set { _underlyingSymbolLastTradedPrice = value; }
        }
    }
}
