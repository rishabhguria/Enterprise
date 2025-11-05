using Prana.BusinessObjects.AppConstants;
using System.Collections.Generic;

namespace Prana.TradingTicket.Classes
{
    internal class BulkOrderSingle
    {
        protected string _orderSide = string.Empty;
        public string OrderSide
        {
            get { return _orderSide; }
            set { _orderSide = value; }
        }

        private string _account = string.Empty;
        public string Account
        {
            get { return _account; }
            set { _account = value; }
        }

        protected string _counterPartyName = string.Empty;
        public string CounterPartyName
        {
            get { return _counterPartyName; }
            set { _counterPartyName = value; }
        }

        protected string _venue = string.Empty;
        public string Venue
        {
            get { return _venue; }
            set { _venue = value; }
        }

        protected string _orderType = string.Empty;
        public string OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }

        protected string _tif = string.Empty;
        public string TIF
        {
            get { return _tif; }
            set { _tif = value; }
        }

        protected string _expireTime = string.Empty;

        public string ExpireTime
        {
            get { return _expireTime; }
            set { _expireTime = value; }
        }

        protected string _strategy = string.Empty;
        public string Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        private double _stopPrice = 0.0;
        public double StopPrice
        {
            get { return _stopPrice; }
            set { _stopPrice = value; }
        }

        private double _limitPrice = 0.0;
        public double LimitPrice
        {
            get { return _limitPrice; }
            set { _limitPrice = value; }
        }

        protected CalculationBasis? _calcBasis = null;
        public CalculationBasis? CalcBasis
        {
            get { return _calcBasis; }
            set { _calcBasis = value; }
        }

        protected double _commissionRate = 0.0;
        public double CommissionRate
        {
            get { return _commissionRate; }
            set { _commissionRate = value; }
        }

        protected CalculationBasis? _softCommissionCalcBasis = null;
        public CalculationBasis? SoftCommissionCalcBasis
        {
            get { return _softCommissionCalcBasis; }
            set { _softCommissionCalcBasis = value; }
        }

        protected double _softCommissionRate = 0.0;
        public double SoftCommissionRate
        {
            get { return _softCommissionRate; }
            set { _softCommissionRate = value; }
        }

        protected string _tradeAttribute1 = string.Empty;
        public string TradeAttribute1
        {
            get { return _tradeAttribute1; }
            set { _tradeAttribute1 = (value == null) ? string.Empty : value; }
        }

        protected string _tradeAttribute2 = string.Empty;
        public string TradeAttribute2
        {
            get { return _tradeAttribute2; }
            set { _tradeAttribute2 = (value == null) ? string.Empty : value; }
        }

        protected string _tradeAttribute3 = string.Empty;
        public string TradeAttribute3
        {
            get { return _tradeAttribute3; }
            set { _tradeAttribute3 = (value == null) ? string.Empty : value; }
        }


        protected string _tradeAttribute4 = string.Empty;
        public string TradeAttribute4
        {
            get { return _tradeAttribute4; }
            set { _tradeAttribute4 = (value == null) ? string.Empty : value; }
        }

        protected string _tradeAttribute5 = string.Empty;
        public string TradeAttribute5
        {
            get { return _tradeAttribute5; }
            set { _tradeAttribute5 = (value == null) ? string.Empty : value; }
        }

        protected string _tradeAttribute6 = string.Empty;
        public string TradeAttribute6
        {
            get { return _tradeAttribute6; }
            set { _tradeAttribute6 = (value == null) ? string.Empty : value; }
        }

        protected string _tradingAccountName = string.Empty;
        public string TradingAccountName
        {
            get { return _tradingAccountName; }
            set { _tradingAccountName = value; }
        }

        protected string _handlingInstruction = string.Empty;
        public string HandlingInstruction
        {
            get { return _handlingInstruction; }
            set { _handlingInstruction = value; }
        }

        protected string _executionInstruction = string.Empty;
        public string ExecutionInstruction
        {
            get { return _executionInstruction; }
            set { _executionInstruction = value; }
        }

        protected int _level1ID = int.MinValue;
        public int Level1ID
        {
            get { return _level1ID; }
            set { _level1ID = value; }
        }

        protected int _level2ID = int.MinValue;
        public int Level2ID
        {
            get { return _level2ID; }
            set { _level2ID = value; }
        }

        protected int _counterPartyID = int.MinValue;
        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        protected int _venueID = int.MinValue;
        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        protected string _orderSideTagValue = string.Empty;
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }

        protected string _orderTypeTagValue = string.Empty;
        public string OrderTypeTagValue
        {
            get { return _orderTypeTagValue; }
            set { _orderTypeTagValue = value; }
        }

        protected int _tradingAccountID = int.MinValue;
        public int TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }

        protected string _algoStrategyID = string.Empty;
        public string AlgoStrategyID
        {
            get { return _algoStrategyID; }
            set { _algoStrategyID = value; }
        }

        protected string _algoStrategyName = string.Empty;
        public string AlgoStrategyName
        {
            get { return _algoStrategyName; }
            set { _algoStrategyName = value; }
        }

        protected Dictionary<string, string> _tagValueDictionary = new Dictionary<string, string>();
        public Dictionary<string, string> TagValueDictionary
        {
            get { return _tagValueDictionary; }
            set { _tagValueDictionary = value; }
        }

        protected bool _isCustom = false;
        public bool IsCustom
        {
            get { return _isCustom; }
            set { _isCustom = value; }
        }
    }
}
