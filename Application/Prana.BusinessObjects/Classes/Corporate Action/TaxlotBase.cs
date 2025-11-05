using Csla;
using Prana.BusinessObjects.AppConstants;
using System;
using System.ComponentModel;
using System.Text;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Used in corporate action.
    /// </summary>
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class TaxlotBase : BusinessBase<TaxlotBase>
    {

        public TaxlotBase()
        {
            MarkAsChild();
        }

        private long _parenTaxlot_PK;
        [Browsable(false)]
        public long ParentTaxlot_PK
        {
            get { return _parenTaxlot_PK; }
            set { _parenTaxlot_PK = value; }
        }

        private string _fkID;
        [Browsable(false)]
        public string FKID
        {
            get { return _fkID; }
            set { _fkID = value; }
        }

        private string _transactionIDs;
        [Browsable(false)]
        public string TransactionIDs
        {
            get { return _transactionIDs; }
            set { _transactionIDs = value; }
        }

        private string _groupID = string.Empty;
        public string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        private string _strategy = string.Empty;
        public string Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        private string _l1TaxlotID = string.Empty;
        [Browsable(false)]
        public string L1TaxlotID
        {
            get { return _l1TaxlotID; }
            set { _l1TaxlotID = value; }
        }

        private string _l2TaxlotID = string.Empty;
        public string L2TaxlotID
        {
            get { return _l2TaxlotID; }
            set { _l2TaxlotID = value; }
        }

        private int _accountID;
        [Browsable(false)]
        public int Level1ID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private int _l2ID;
        [Browsable(false)]
        public int Level2ID
        {
            get { return _l2ID; }
            set { _l2ID = value; }
        }

        private string _symbol = string.Empty;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private AssetCategory _assetCategory = AssetCategory.None;
        [Browsable(false)]
        public AssetCategory AssetCategory
        {
            get { return _assetCategory; }
            set { _assetCategory = value; }
        }

        private Underlying _underlying = Underlying.None;
        [Browsable(false)]
        public Underlying Underlying
        {
            get { return _underlying; }
            set { _underlying = value; }
        }

        private int _exchangeID;
        [Browsable(false)]
        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        private int _currencyID;
        [Browsable(false)]
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        private string _orderSideTagValue = string.Empty;
        [Browsable(false)]
        public string OrderSideTagValue
        {
            get { return _orderSideTagValue; }
            set { _orderSideTagValue = value; }
        }

        private string _currency;
        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        private string _orderSide = string.Empty;
        public string Side
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

        private double _avgPrice = double.MinValue;
        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }

        private double _openQty;
        public double OpenQty
        {
            get { return _openQty; }
            set { _openQty = value; }
        }

        private double _closedQty = 0;
        [Browsable(false)]
        public double ClosedQty
        {
            get { return _closedQty; }
            set { _closedQty = value; }
        }

        private int _auecID = int.MinValue;
        [Browsable(false)]
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }

        private string _UTCDate = string.Empty;
        [Browsable(false)]
        public string UTCDate
        {
            get { return _UTCDate; }
            set { _UTCDate = value; }
        }

        private PositionTag _positionTag;
        public PositionTag PositionTag
        {
            get { return _positionTag; }
            set { _positionTag = value; }
        }

        private double _opentotalCommissionandFees = 0.0;
        public double OpenTotalCommissionandFees
        {
            get { return _opentotalCommissionandFees; }
            set { _opentotalCommissionandFees = value; }
        }

        private double _closedtotalCommissionandFees = 0.0;
        public double ClosedTotalCommissionandFees
        {
            get { return _closedtotalCommissionandFees; }
            set { _closedtotalCommissionandFees = value; }
        }

        private DateTime _AUECLocalDate = DateTimeConstants.MinValue;
        public DateTime AUECLocalDate
        {
            get { return _AUECLocalDate; }
            set { _AUECLocalDate = value; }
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

        private DateTime _settlementDate = DateTimeConstants.MinValue;
        public virtual DateTime SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }

        private string _auecDate = string.Empty;
        [Browsable(false)]
        public string AUECDate
        {
            get { return _auecDate; }
            set { _auecDate = value; }
        }

        private string _corpActionId = string.Empty;
        public string CorpActionID
        {
            get { return _corpActionId; }
            set { _corpActionId = value; }
        }

        private string _newCompanyName = string.Empty;
        public string NewCompanyName
        {
            get { return _newCompanyName; }
            set { _newCompanyName = value; }
        }

        private string _newTaxlotOpenQty = string.Empty;
        public string NewTaxlotOpenQty
        {
            get { return _newTaxlotOpenQty; }
            set { _newTaxlotOpenQty = value; }
        }

        private string _newAvgPrice = string.Empty;
        public string NewAvgPrice
        {
            get { return _newAvgPrice; }
            set { _newAvgPrice = value; }
        }

        private PositionType _positionType;
        [Browsable(false)]
        public PositionType PositionType
        {
            get { return _positionType; }
            set { _positionType = value; }
        }

        private string _taxLotPK = string.Empty;
        [Browsable(false)]
        public string TaxLotPK
        {
            get { return _taxLotPK; }
            set { _taxLotPK = value; }
        }

        private Guid _closingTaxLotID = Guid.Empty;
        [Browsable(false)]
        public Guid ClosingTaxlotID
        {
            get { return _closingTaxLotID; }
            set { _closingTaxLotID = value; }
        }

        private double _dividend;
        /// <summary>
        /// Dividend received on this taxlot
        /// </summary>
        public double Dividend
        {
            get { return _dividend; }
            set { _dividend = value; }
        }

        private string _divPayoutDate;
        /// <summary>
        /// date when dividend will be accrued.
        /// </summary>
        public string DivPayoutDate
        {
            get { return _divPayoutDate; }
            set { _divPayoutDate = value; }
        }

        private string _exDivDate;
        public string ExDivDate
        {
            get { return _exDivDate; }
            set { _exDivDate = value; }
        }

        private string _recordDate;
        public string RecordDate
        {
            get { return _recordDate; }
            set { _recordDate = value; }
        }

        private string _divDeclarationDate;
        public string DivDeclarationDate
        {
            get { return _divDeclarationDate; }
            set { _divDeclarationDate = value; }
        }

        private string _orderTypeTagValue;
        [Browsable(false)]
        public string OrderTypeTagValue
        {
            get { return _orderTypeTagValue; }
            set { _orderTypeTagValue = value; }
        }

        private int _counterPartyID;
        [Browsable(false)]
        public int CounterPartyID
        {
            get { return _counterPartyID; }
            set { _counterPartyID = value; }
        }

        private string _broker = string.Empty;
        public string Broker
        {
            get { return _broker; }
            set { _broker = value; }
        }

        private int _venueID;
        [Browsable(false)]
        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        private int _tradingAccountID;
        [Browsable(false)]
        public int TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }

        private double _cumQty;
        [Browsable(false)]
        public double CumQty
        {
            get { return _cumQty; }
            set { _cumQty = value; }
        }

        private double _allocatedQty;
        [Browsable(false)]
        public double AllocatedQty
        {
            get { return _allocatedQty; }
            set { _allocatedQty = value; }
        }

        private string _listID;
        [Browsable(false)]
        public string ListID
        {
            get { return _listID; }
            set { _listID = value; }
        }

        private int _userID;
        [Browsable(false)]
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private bool _isProrataActive;
        [Browsable(false)]
        public bool ISProrataActive
        {
            get { return _isProrataActive; }
            set { _isProrataActive = value; }
        }

        private bool _autoGrouped;
        [Browsable(false)]
        public bool AutoGrouped
        {
            get { return _autoGrouped; }
            set { _autoGrouped = value; }
        }

        private int stateID;
        [Browsable(false)]
        public int StateID
        {
            get { return stateID; }
            set { stateID = value; }
        }

        private int _isBasketGroup;
        [Browsable(false)]
        public int IsBasketGroup
        {
            get { return _isBasketGroup; }
            set { _isBasketGroup = value; }
        }

        private string basketGroupID;
        [Browsable(false)]
        public string BasketGroupID
        {
            get { return basketGroupID; }
            set { basketGroupID = value; }
        }

        private bool _isManualGroup;
        [Browsable(false)]
        public bool IsManualGroup
        {
            get { return _isManualGroup; }
            set { _isManualGroup = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string _internalComments;
        [Browsable(false)]
        public string InternalComments
        {
            get { return _internalComments; }
            set { _internalComments = value; }
        }

        private double _fxRate;
        //[Browsable(false)]
        public double FXRate
        {
            get { return _fxRate; }
            set { _fxRate = value; }
        }

        private string _fxConversionMethodOperator = string.Empty;
        //[Browsable(false)]
        public string FXConversionMethodOperator
        {
            get { return _fxConversionMethodOperator; }
            set { _fxConversionMethodOperator = value; }
        }

        private string _tradeAttribute1;
        [Browsable(false)]
        public string TradeAttribute1
        {
            get { return _tradeAttribute1; }
            set { _tradeAttribute1 = value; }
        }

        private string _tradeAttribute2;
        [Browsable(false)]
        public string TradeAttribute2
        {
            get { return _tradeAttribute2; }
            set { _tradeAttribute2 = value; }
        }

        private string _tradeAttribute3;
        [Browsable(false)]
        public string TradeAttribute3
        {
            get { return _tradeAttribute3; }
            set { _tradeAttribute3 = value; }
        }

        private string _tradeAttribute4;
        [Browsable(false)]
        public string TradeAttribute4
        {
            get { return _tradeAttribute4; }
            set { _tradeAttribute4 = value; }
        }

        private string _tradeAttribute5;
        [Browsable(false)]
        public string TradeAttribute5
        {
            get { return _tradeAttribute5; }
            set { _tradeAttribute5 = value; }
        }

        private string _tradeAttribute6;
        [Browsable(false)]
        public string TradeAttribute6
        {
            get { return _tradeAttribute6; }
            set { _tradeAttribute6 = value; }
        }

        private string _lotId;
        [Browsable(false)]
        public string LotId
        {
            get { return _lotId; }
            set { _lotId = value; }
        }

        private string _externalTransId;
        [Browsable(false)]
        public string ExternalTransId
        {
            get { return _externalTransId; }
            set { _externalTransId = value; }
        }

        private double _assetMultiplier = 1;
        [Browsable(false)]
        public double AssetMultiplier
        {
            get { return _assetMultiplier; }
            set { _assetMultiplier = value; }
        }

        private double _notionalChange = 0;
        public double NotionalChange
        {
            get { return _notionalChange; }
            set { _notionalChange = value; }
        }

        // Added By: Ankit Gupta on June 06, 2014
        //http://jira.nirvanasolutions.com:8080/browse/PRANA-4099

        private double _oldAveragePrice = 0;
        public double OldAveragePrice
        {
            get { return _oldAveragePrice; }
            set { _oldAveragePrice = value; }
        }

        // Added By: Ankit Gupta on June 06, 2014
        //http://jira.nirvanasolutions.com:8080/browse/PRANA-4100
        private double _notionalValue = 0;
        public double NotionalValue
        {
            get { return _notionalValue; }
            set { _notionalValue = value; }
        }

        private double _notionalValue_base = 0;
        public double NotionalValueBase
        {
            get { return _notionalValue_base; }
            set { _notionalValue_base = value; }
        }

        private string _transactionType = string.Empty;
        //[Browsable(false)]
        public virtual string TransactionType
        {
            get { return _transactionType; }
            set { _transactionType = value; }
        }

        private double _originalQty;
        [Browsable(false)]
        public double OriginalQty
        {
            get { return _originalQty; }
            set { _originalQty = value; }
        }


        private int _settlementCurrencyID;
        [Browsable(false)]
        public virtual int SettlementCurrencyID
        {
            get { return _settlementCurrencyID; }
            set { _settlementCurrencyID = value; }
        }


        private string _settlementCurrency;
        [Browsable(false)]
        public string SettlementCurrency
        {
            get { return _settlementCurrency; }
            set { _settlementCurrency = value; }
        }

        protected override object GetIdValue()
        {
            StringBuilder id = new StringBuilder();
            id.Append(_groupID);
            if (!String.IsNullOrEmpty(_l1TaxlotID))
            {
                id.Append(_l1TaxlotID);
            }
            if (!String.IsNullOrEmpty(_l2TaxlotID))
            {
                id.Append(_l2TaxlotID);
            }

            return id.ToString();
        }
    }
}
