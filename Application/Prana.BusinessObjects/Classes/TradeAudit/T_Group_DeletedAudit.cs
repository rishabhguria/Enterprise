using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class T_Group_DeletedAudit
    {
        private string _groupID;
        public string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        private string _orderSideTagValue = string.Empty;
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _orderTypeTagValue = string.Empty;
        public string OrderTypeTagValue
        {
            get { return _orderTypeTagValue; }
            set { _orderTypeTagValue = value; }
        }

        private int _counterPartyID = int.MinValue;
        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        private int _venueID = int.MinValue;
        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        private int _tradingAccountID = int.MinValue;
        public int TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }

        private int _auecID = int.MinValue;
        public int AuecID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private double _cumQty = 0;
        public double CumQty
        {
            get { return _cumQty; }
            set { _cumQty = value; }
        }

        private double _allocatedQty = 0.0;
        public double AllocatedQty
        {
            get { return _allocatedQty; }
            set { _allocatedQty = value; }
        }

        private double _quantity = 0;
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private double _avgPrice = 0;
        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }

        private int _settlCurrency;
        public int SettlCurrency
        {
            get { return _settlCurrency; }
            set { _settlCurrency = value; }
        }

        bool _isPreAllocated = false;
        public bool IsPreAllocated
        {
            get { return _isPreAllocated; }
            set { _isPreAllocated = value; }
        }

        private string _listID = string.Empty;
        public string ListID
        {
            get { return _listID; }
            set { _listID = value; }
        }

        private int _UserID;
        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        private bool _isProrataActive = false;
        public bool IsProrataActive
        {
            get { return _isProrataActive; }
            set { _isProrataActive = value; }
        }

        private bool _autoGrouped = false;
        public bool AutoGrouped
        {
            get { return _autoGrouped; }
            set { _autoGrouped = value; }
        }

        private PostTradeConstants.ORDERSTATE_ALLOCATION _state = PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED;
        public PostTradeConstants.ORDERSTATE_ALLOCATION State
        {
            get { return _state; }
            set { _state = value; }
        }

        private bool _isManualGroup = false;
        public bool IsManualGroup
        {
            get { return _isManualGroup; }
            set { _isManualGroup = value; }
        }

        private DateTime _allocationDate = DateTimeConstants.MinValue;
        public DateTime AllocationDate
        {
            get { return _allocationDate; }
            set { _allocationDate = value; }
        }

        private DateTime _settlementDate = DateTimeConstants.MinValue;
        public DateTime SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }

        private int _assetID = int.MinValue;
        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }

        private int _underlyingID = int.MinValue;
        public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }

        private int _exchangeID = int.MinValue;
        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        private int _currencyID = int.MinValue;
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _internalComments = string.Empty;
        public string InternalComments
        {
            get { return _internalComments; }
            set { _internalComments = value; }
        }

        private DateTime _aUECLocalDate = DateTimeConstants.MinValue;
        public DateTime AUECLocalDate
        {
            get { return _aUECLocalDate; }
            set { _aUECLocalDate = value; }
        }

        private bool _isSwapped = false;
        public bool IsSwapped
        {
            get { return _isSwapped; }
            set { _isSwapped = value; }
        }

        private double _avgFXRateForTrade = 0.0;
        public double AvgFXRateForTrade
        {
            get { return _avgFXRateForTrade; }
            set { _avgFXRateForTrade = value; }
        }

        private string _FXConversionMethodOperator = string.Empty;
        public string FXConversionMethodOperator
        {
            get { return _FXConversionMethodOperator; }
            set { _FXConversionMethodOperator = value; }
        }

        private string _taxLotClosingId;
        public string TaxLotClosingId
        {
            get { return _taxLotClosingId; }
            set { _taxLotClosingId = value; }
        }
        private double _commission = 0;
        public double Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }
        private double _softCommission = 0;
        public double SoftCommission
        {
            get { return _softCommission; }
            set { _softCommission = value; }
        }
        private double _otherBrokerfees = 0;
        public double OtherBrokerfees
        {
            get { return _otherBrokerfees; }
            set { _otherBrokerfees = value; }
        }

        private double _clearingBrokerFee = 0;
        public double ClearingBrokerFee
        {
            get { return _clearingBrokerFee; }
            set { _clearingBrokerFee = value; }
        }

        private double _stampDuty;
        public double StampDuty
        {
            get { return _stampDuty; }
            set { _stampDuty = value; }
        }

        private double _transactionLevy;
        public double TransactionLevy
        {
            get { return _transactionLevy; }
            set { _transactionLevy = value; }
        }

        private double _clearingFee;
        public double ClearingFee
        {
            get { return _clearingFee; }
            set { _clearingFee = value; }
        }

        private double _taxOnCommissions;
        public double TaxOnCommissions
        {
            get { return _taxOnCommissions; }
            set { _taxOnCommissions = value; }
        }

        private double _miscFees;
        public double MiscFees
        {
            get { return _miscFees; }
            set { _miscFees = value; }
        }

        private double _secFee;
        public double SecFee
        {
            get { return _secFee; }
            set { _secFee = value; }
        }

        private double _occFee;
        public double OccFee
        {
            get { return _occFee; }
            set { _occFee = value; }
        }

        private double _orfFee;
        public double OrfFee
        {
            get { return _orfFee; }
            set { _orfFee = value; }
        }

        private double _accruedInterest;
        public double AccruedInterest
        {
            get { return _accruedInterest; }
            set { _accruedInterest = value; }
        }

        private DateTime _processDate = DateTimeConstants.MinValue;
        public DateTime ProcessDate
        {
            get { return _processDate; }
            set { _processDate = value; }
        }

        private DateTime _originalPurchaseDate = DateTimeConstants.MinValue;
        public DateTime OriginalPurchaseDate
        {
            get { return _originalPurchaseDate; }
            set { _originalPurchaseDate = value; }
        }

        private bool _isModified = false;
        public bool IsModified
        {
            get { return _isModified; }
            set { _isModified = value; }
        }

        private int _allocationSchemeID = 0;
        public int AllocationSchemeID
        {
            get { return _allocationSchemeID; }
            set { _allocationSchemeID = value; }
        }

        private int _commissionSource = 1;
        public int CommissionSource
        {
            get { return _commissionSource; }
            set { _commissionSource = value; }
        }

        private int _softCommissionSource = 1;
        public int SoftCommissionSource
        {
            get { return _softCommissionSource; }
            set { _softCommissionSource = value; }
        }

        private string _tradeAttribute1 = string.Empty;
        public string TradeAttribute1
        {
            get { return _tradeAttribute1; }
            set { _tradeAttribute1 = value; }
        }

        private string _tradeAttribute2 = string.Empty;
        public string TradeAttribute2
        {
            get { return _tradeAttribute2; }
            set { _tradeAttribute2 = value; }
        }

        private string _tradeAttribute3 = string.Empty;
        public string TradeAttribute3
        {
            get { return _tradeAttribute3; }
            set { _tradeAttribute3 = value; }
        }

        private string _tradeAttribute4 = string.Empty;
        public string TradeAttribute4
        {
            get { return _tradeAttribute4; }
            set { _tradeAttribute4 = value; }
        }

        private string _tradeAttribute5 = string.Empty;
        public string TradeAttribute5
        {
            get { return _tradeAttribute5; }
            set { _tradeAttribute5 = value; }
        }

        private string _tradeAttribute6 = string.Empty;
        public string TradeAttribute6
        {
            get { return _tradeAttribute6; }
            set { _tradeAttribute6 = value; }
        }

        private string _taxLotIdsWithAttributes = string.Empty;
        public string TaxLotIdsWithAttributes
        {
            get { return _taxLotIdsWithAttributes; }
            set { _taxLotIdsWithAttributes = value; }
        }

        private string _transactionType = string.Empty;
        public string TransactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; }
        }

        private double _optionPremiumAdjustment;
        public double OptionPremiumAdjustment
        {
            get { return _optionPremiumAdjustment; }
            set { _optionPremiumAdjustment = value; }
        }

        public T_Group_DeletedAudit() { }

        public T_Group_DeletedAudit(AllocationGroup group)
        {
            this.AccruedInterest = group.AccruedInterest;
            this.AllocatedQty = group.AllocatedQty;
            this.AllocationDate = group.AllocationDate;
            this.AllocationSchemeID = group.AllocationSchemeID;
            this.AssetID = group.AssetID;
            this.AuecID = group.AUECID;
            this.AUECLocalDate = group.AUECLocalDate;
            this.AutoGrouped = group.AutoGrouped;
            this.AvgFXRateForTrade = group.FXRate;
            this.AvgPrice = group.AvgPrice;
            this.ClearingFee = group.ClearingFee;
            this.Commission = group.Commission;
            this.SoftCommission = group.SoftCommission;
            this.CommissionSource = group.CommissionSource;
            this.SoftCommissionSource = group.SoftCommissionSource;
            this.CounterPartyID = group.CounterPartyID;
            this.CumQty = group.CumQty;
            this.CurrencyID = group.CurrencyID;
            this.Description = group.Description;
            this.InternalComments = group.InternalComments;
            this.ExchangeID = group.ExchangeID;
            this.FXConversionMethodOperator = group.FXConversionMethodOperator;
            this.GroupID = group.GroupID;
            this.IsManualGroup = group.IsManualGroup;
            this.IsModified = group.IsModified;
            this.IsPreAllocated = group.IsPreAllocated;
            this.IsProrataActive = group.ISProrataActive;
            this.IsSwapped = group.IsSwapped;
            this.ListID = group.ListID;
            this.MiscFees = group.MiscFees;
            this.OrderSideTagValue = group.OrderSideTagValue;
            this.OrderTypeTagValue = group.OrderTypeTagValue;
            this.OriginalPurchaseDate = group.OriginalPurchaseDate;
            this.OtherBrokerfees = group.OtherBrokerFees;
            this.ClearingBrokerFee = group.ClearingBrokerFee;
            this.ProcessDate = group.ProcessDate;
            this.Quantity = group.Quantity;
            this.SettlementDate = group.SettlementDate;
            this.StampDuty = group.StampDuty;
            this.SecFee = group.SecFee;
            this.OccFee = group.OccFee;
            this.OrfFee = group.OrfFee;
            this.State = group.State;
            this.Symbol = group.Symbol;
            if (string.IsNullOrEmpty(group.TaxLotClosingId))
                this.TaxLotClosingId = null;
            else
                this.TaxLotClosingId = group.TaxLotClosingId;
            this.TaxLotIdsWithAttributes = group.TaxLotIdsWithAttributes;
            this.TaxOnCommissions = group.TaxOnCommissions;
            this.TradeAttribute1 = group.TradeAttribute1;
            this.TradeAttribute2 = group.TradeAttribute2;
            this.TradeAttribute3 = group.TradeAttribute3;
            this.TradeAttribute4 = group.TradeAttribute4;
            this.TradeAttribute5 = group.TradeAttribute5;
            this.TradeAttribute6 = group.TradeAttribute6;
            this.TradingAccountID = group.TradingAccountID;
            this.TransactionLevy = group.TransactionLevy;
            this.UnderlyingID = group.UnderlyingID;
            this.UserID = group.UserID;
            this.VenueID = group.VenueID;
            this.TransactionType = group.TransactionType;
            this.SettlCurrency = group.SettlementCurrencyID;
            //Added new field Option Premium Adjustment
            //http://jira.nirvanasolutions.com:8080/browse/CHMW-3345
            this.OptionPremiumAdjustment = group.OptionPremiumAdjustment;
        }
    }
}
