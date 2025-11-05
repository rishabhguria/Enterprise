using Newtonsoft.Json;
using Prana.BusinessObjects.AppConstants;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class TradingTicketUIPrefs
    {
        private List<DefAssetSide> _defAssetSides = new List<DefAssetSide>();
        private string _defTTControlsMapping = string.Empty;
        private int? _broker = null;
        private int? _venue;
        private int? _orderType;
        private int? _timeInForce;
        private int? _handlingInstruction;
        private int? _executionInstruction;
        private int? _tradingAccount;
        private int? _strategy;
        private int? _account;
        private bool? _isSettlementCurrencyBase;
        private bool? _isShowTargetQTY;
        private double? _quantity = 0;
        private double? _incrementOnQty = 0;
        private double? _incrementOnStop = 0;
        private double? _incrementOnLimit = 0;
        private bool _isUseRoundLots;
        public List<DefAssetSide> DefAssetSides
        {
            get { return _defAssetSides; }
            set { _defAssetSides = value; }
        }

        public string DefTTControlsMapping
        {
            get { return _defTTControlsMapping; }
            set { _defTTControlsMapping = value; }
        }

        public List<DefTTControlsMapping> listTTControlsMapping
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_defTTControlsMapping) ? JsonConvert.DeserializeObject<List<DefTTControlsMapping>>(_defTTControlsMapping) : new List<DefTTControlsMapping>();
            }
        }

        public int? Broker
        {
            get { return _broker; }
            set { _broker = value; }
        }

        public int? Venue
        {
            get { return _venue; }
            set { _venue = value; }
        }

        public int? OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }

        public int? TimeInForce
        {
            get { return _timeInForce; }
            set { _timeInForce = value; }
        }

        public int? HandlingInstruction
        {
            get { return _handlingInstruction; }
            set { _handlingInstruction = value; }
        }

        public int? ExecutionInstruction
        {
            get { return _executionInstruction; }
            set { _executionInstruction = value; }
        }

        public int? TradingAccount
        {
            get { return _tradingAccount; }
            set { _tradingAccount = value; }
        }

        public int? Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        public int? Account
        {
            get { return _account; }
            set { _account = value; }
        }

        public bool? IsSettlementCurrencyBase
        {
            get { return _isSettlementCurrencyBase; }
            set { _isSettlementCurrencyBase = value; }
        }


        public bool? IsShowTargetQTY
        {
            get { return _isShowTargetQTY; }
            set { _isShowTargetQTY = value; }
        }


        public double? Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public double? IncrementOnQty
        {
            get { return _incrementOnQty; }
            set { _incrementOnQty = value; }
        }

        public double? IncrementOnStop
        {
            get { return _incrementOnStop; }
            set { _incrementOnStop = value; }
        }

        public double? IncrementOnLimit
        {
            get { return _incrementOnLimit; }
            set { _incrementOnLimit = value; }
        }

        public QuantityTypeOnTT QuantityType { get; set; }

        public bool IsUseRoundLots
        {
            get { return _isUseRoundLots; }
            set { _isUseRoundLots = value; }
        }
    }
}
