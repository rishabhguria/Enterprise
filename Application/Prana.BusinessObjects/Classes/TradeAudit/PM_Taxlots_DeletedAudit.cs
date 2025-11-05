using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class PM_Taxlots_DeletedAudit
    {
        private string _taxlotID = string.Empty;
        public string TaxlotID
        {
            get { return _taxlotID; }
            set { _taxlotID = value; }
        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private double _taxLotQty;
        public double TaxLotQty
        {
            get { return _taxLotQty; }
            set { _taxLotQty = value; }
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

        private DateTime _timeOfSaveUTC = DateTimeConstants.MinValue;
        public DateTime TimeOfSaveUTC
        {
            get { return _timeOfSaveUTC; }
            set { _timeOfSaveUTC = value; }
        }

        private string _groupID;
        public string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        private DateTime _auecModifiedDate = DateTimeConstants.MinValue;
        public DateTime AuecModifiedDate
        {
            get { return _auecModifiedDate; }
            set { _auecModifiedDate = value; }
        }

        private int _level1ID;
        public int Level1ID
        {
            get { return _level1ID; }
            set { _level1ID = value; }
        }

        private int _level2ID = 0;
        public int Level2ID
        {
            get { return _level2ID; }
            set { _level2ID = value; }
        }

        private double _openTotalCommissionandFees = 0.0;
        public double OpenTotalCommissionandFees
        {
            get { return _openTotalCommissionandFees; }
            set { _openTotalCommissionandFees = value; }
        }

        private double _closedTotalCommissionandFees = 0.0;
        public double ClosedTotalCommissionandFees
        {
            get { return _closedTotalCommissionandFees; }
            set { _closedTotalCommissionandFees = value; }
        }

        private PositionTag _positionTag;
        public PositionTag PositionTag
        {
            get { return _positionTag; }
            set { _positionTag = value; }
        }

        private string _orderSideTagValue = string.Empty;
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }

        private string _taxLotClosingId;
        public string TaxLotClosingId
        {
            get { return _taxLotClosingId; }
            set { _taxLotClosingId = value; }
        }

        private double _accruedInterest;
        public double AccruedInterest
        {
            get { return _accruedInterest; }
            set { _accruedInterest = value; }
        }

        private double _avgFXRateForTrade = 0.0;
        public double FXRate
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

        private string _externalTransId;
        public string ExternalTransId
        {
            get { return _externalTransId; }
            set { _externalTransId = value; }
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

        private string _lotId = string.Empty;
        public string LotId
        {
            get { return _lotId; }
            set { _lotId = value; }
        }

        public PM_Taxlots_DeletedAudit() { }

        public PM_Taxlots_DeletedAudit(TaxLot taxlot)
        {
            this.AccruedInterest = taxlot.AccruedInterest;
            this.AuecModifiedDate = taxlot.AUECModifiedDate;
            this.AvgPrice = taxlot.AvgPrice;
            this.ClosedTotalCommissionandFees = taxlot.ClosedTotalCommissionandFees;
            this.ExternalTransId = taxlot.ExternalTransId;
            this.FXConversionMethodOperator = taxlot.FXConversionMethodOperator;
            this.FXRate = taxlot.FXRate;
            this.SettlCurrency = taxlot.SettlementCurrencyID;
            this.GroupID = taxlot.GroupID;
            this.Level1ID = taxlot.Level1ID;
            this.Level2ID = taxlot.Level2ID;
            this.LotId = taxlot.LotId;
            this.OpenTotalCommissionandFees = taxlot.OpenTotalCommissionandFees;
            this.OrderSideTagValue = taxlot.OrderSideTagValue;
            this.PositionTag = taxlot.PositionTag;
            this.Symbol = taxlot.Symbol;
            this.TaxLotClosingId = taxlot.TaxLotClosingId;
            this.TaxlotID = taxlot.TaxLotID;
            this.TaxLotQty = taxlot.TaxLotQty;
            this.TimeOfSaveUTC = taxlot.TimeOfSaveUTC;
            this.TradeAttribute1 = taxlot.TradeAttribute1;
            this.TradeAttribute2 = taxlot.TradeAttribute2;
            this.TradeAttribute3 = taxlot.TradeAttribute3;
            this.TradeAttribute4 = taxlot.TradeAttribute4;
            this.TradeAttribute5 = taxlot.TradeAttribute5;
            this.TradeAttribute6 = taxlot.TradeAttribute6;
        }
    }
}