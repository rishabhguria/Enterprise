//using Prana.PM.ApplicationConstants;
using Csla;
using Csla.Validation;
using Prana.BusinessObjects.AppConstants;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Prana.BusinessObjects.PositionManagement
{

    [Serializable(), System.Runtime.InteropServices.ComVisible(false)]
    public class AllocatedTrade : BusinessBase<AllocatedTrade>, IKeyable
    {

        public AllocatedTrade()
        {
            MarkAsChild();
        }

        public const string SIDEBUY = "BUY";
        public const string SIDEBUYTOCLOSE = "BUY TO CLOSE";
        public const string SIDESELL = "SELL";
        public const string SIDESELLSHORT = "SELL SHORT";
        const string CONST_CASHSETTLEDPRICE = "CashSettledPrice";
        const string CONST_SETTLEQTY = "SettledQty";
        const string CONST_SETTLEMENTMODE = "SettlementMode";
        public const string SIDEBUYTOOPEN = "BUY TO OPEN";
        public const string SIDESELLTOOPEN = "SELL TO OPEN";

        private string _ID = string.Empty;

        /// <summary>
        /// Gets or sets the allocation ID.
        /// </summary>
        /// <value>The allocation ID.</value>        
        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private Account _account;

        /// <summary>
        /// Gets or sets the account value.
        /// </summary>
        /// <value>The account value.</value>   
      //  [XmlIgnore]
        public Account AccountValue
        {
            get
            {
                if (_account == null)
                {

                    _account = new Account();
                }
                return _account;
            }
            set
            {
                _account = value;
                PropertyHasChanged();
            }
        }
        private Strategy _strategy;

        public Strategy StrategyValue
        {
            get
            {
                if (_strategy == null)
                {

                    _strategy = new Strategy();
                }
                return _strategy;
            }
            set
            {
                _strategy = value;
                PropertyHasChanged();
            }
        }

        private DateTime _tradeDate;

        /// <summary>
        /// Gets or sets the trade date AUEC Local Date.
        /// </summary>
        /// <value>The trade date.</value>
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime TradeDate
        {
            get { return _tradeDate; }
            set { _tradeDate = value; }
        }

        /// <summary>
        /// Gets or sets the trade date AUEC Local Date.
        /// </summary>
        /// <value>The trade date string.</value>

        [System.ComponentModel.Browsable(false)]
        public virtual string TradeDateString
        {
            get { return _tradeDate.ToString("MMM dd yyyy HH:mm:ss:fff"); }
        }

        private DateTime _tradeDateUTC;
        [System.Xml.Serialization.XmlIgnore()]
        [Browsable(false)]
        public DateTime TradeDateUTC
        {
            get { return _tradeDateUTC; }
            set { _tradeDateUTC = value; }
        }


        private string _side;

        /// <summary>
        /// Gets or sets the side.
        /// </summary>
        /// <value>The side.</value>
        [XmlIgnore]
        public string Side
        {
            get { return _side; }
            set { _side = value; }
        }

        private string _symbol;

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>The symbol.</value>
        public string Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                PropertyHasChanged();
            }
        }

        #region Quantity fields

        private double _openQty = 0;
        /// <summary>
        /// Gets or sets the open qty.
        /// </summary>
        /// <value>The open qty.</value>
        public double OpenQty
        {
            get { return _openQty; }
            set { _openQty = value; }
        }




        private double _startQty = 0;

        /// <summary>
        /// Gets or sets the start quantity.
        /// </summary>
        /// <value>The start quantity.</value>
        [Browsable(false)]

        public double StartQty
        {
            get { return _startQty; }
            set { _startQty = value; }
        }

        #endregion


        private double _multiplier = 1;

        /// <summary>
        /// Gets or sets the multiplier.
        /// </summary>
        /// <value>The multiplier.</value>
        [Browsable(false)]
        public double Multiplier
        {
            get { return _multiplier; }
            set { _multiplier = value; }
        }


        private double _averagePrice = 0.0;

        /// <summary>
        /// Gets or sets the average price.
        /// </summary>
        /// <value>The average price.</value>
        public double AveragePrice
        {
            get { return _averagePrice; }
            set
            {
                _averagePrice = value;
                PropertyHasChanged();
            }
        }

        private string _sideID;
        /// <summary>
        /// Gets or sets the side ID.
        /// </summary>
        /// <value>The side ID.</value>        

        public string SideID
        {
            get { return _sideID; }
            set
            {
                _sideID = value;
            }
        }

        private int _currencyID;
        [Browsable(false)]
        public int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }



        private bool _isPosition;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is position.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is position; otherwise, <c>false</c>.
        /// </value>

        public bool IsPosition
        {
            get { return _isPosition; }
            set { _isPosition = value; }
        }

        private string _positionTaxlotID = string.Empty;

        /// <summary>
        /// Gets or sets the position ID which is a guid to make the postion id unique.
        /// Also position id is taken as guid as in the consolidation view, we maintain dictionary of ids of 
        /// trade,taxlot and positions. So to avoid the clash in between taxlotid and position id.
        /// </summary>
        /// <value>The position ID.</value> 
        [Browsable(false)]
        public string PositionTaxlotID
        {
            get { return _positionTaxlotID; }
            set { _positionTaxlotID = value; }
        }

        #region Commission & Fees fields
        private double _openTotalCommissionandFees = 0.0;

        /// <summary>
        /// Gets or sets the commission.
        /// </summary>
        /// <value>The commission.</value>
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

        /// <summary>
        /// Open + close commission
        /// </summary>
        [Browsable(false)]
        public double TotalCommission
        {
            get { return _openTotalCommissionandFees + _closedTotalCommissionandFees; }
        }

        private bool _isCurrentBuyClosedWithShort = false;
        /// <summary>
        /// isCurrentBuyClosedWithShort
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public bool IsCurrentBuyClosedWithShort
        {
            get { return _isCurrentBuyClosedWithShort; }
            set { _isCurrentBuyClosedWithShort = value; }
        }
        #endregion

        /// <summary>
        /// Gets the net notional value.Cannot set this property.
        /// </summary>
        /// <value>The net notional value.</value>
        public double NetNotionalValue
        {
            get
            {
                if (string.Equals(_sideID, FIXConstants.SIDE_Buy) || string.Equals(_sideID, FIXConstants.SIDE_Buy_Closed) || string.Equals(_sideID, FIXConstants.SIDE_Buy_Open) || string.Equals(_sideID, FIXConstants.SIDE_Buy_Cover))
                    return (_averagePrice * _openQty * _multiplier) + (_openTotalCommissionandFees);
                else
                    return (_averagePrice * _openQty * _multiplier) - (_openTotalCommissionandFees);
            }
        }

        private double _costBasisRealizedPNL = 0.0;
        /// <summary>
        /// Gets the PNL generated by this taxlot in the parent position.
        /// </summary>
        /// <value>The net notional value.</value> 
        public double CostBasisRealizedPNL
        {
            get { return _costBasisRealizedPNL; }
            set { _costBasisRealizedPNL = value; }
        }

        private double _monthsProfitForParentPosition = 0.0;

        /// <summary>
        /// Gets or sets the months profit for parent position.
        /// </summary>
        /// <value>The months profit for parent position.</value>
        public double MonthsProfitForParentPosition
        {
            get { return _monthsProfitForParentPosition; }
            set { _monthsProfitForParentPosition = value; }
        }

        private double _markPriceForMonth = 0.0;

        /// <summary>
        /// Gets or sets the mark price for month.
        /// </summary>
        /// <value>The mark price for month.</value>
        public double MarkPriceForMonth
        {
            get { return _markPriceForMonth; }
            set { _markPriceForMonth = value; }
        }
        private double _parentPositionBalanceQuantity = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <value>The net notional value.</value> 
        //[System.ComponentModel.Browsable(false)]
        public double ParentPositionBalanceQuantity
        {
            get { return _parentPositionBalanceQuantity; }
            set { _parentPositionBalanceQuantity = value; }
        }

        private double _symbolAveragePrice = 0.0;

        public double SymbolAveragePrice
        {
            get { return _symbolAveragePrice; }
            set { _symbolAveragePrice = value; }
        }

        private double _symbolAverageProfitForParentPosition = 0.0;
        /// <summary>
        /// 
        /// </summary>
        /// <value>The net notional value.</value> 
        //[System.ComponentModel.Browsable(false)]
        [Browsable(false)]
        public double SymbolAverageProfitForParentPosition
        {
            get { return _symbolAverageProfitForParentPosition; }
            set { _symbolAverageProfitForParentPosition = value; }
        }

        private int _auecID;
        /// <summary>
        /// Gets or sets the AUECID.
        /// </summary>
        /// <value>The AUECID.</value>
        public int AUECID
        {
            get { return _auecID; }
            set { _auecID = value; }
        }


        private AssetCategory _assetCategoryValue;
        /// <summary>
        /// determines the asset type on the setting of auecid
        /// </summary>

        public AssetCategory AssetCategoryValue
        {
            get { return _assetCategoryValue; }
            set { _assetCategoryValue = value; }
        }
        private double _settledQty = 0;
        /// <summary>
        /// Settled or Expired Qty for Physical or cash Settled
        /// </summary>

        public double SettledQty
        {
            get { return _settledQty; }
            set
            {
                _settledQty = value;
                PropertyHasChanged(CONST_SETTLEQTY);
            }
        }
        private double _cashSettledPrice;
        /// <summary>
        /// cash Settlement price for cash settlement
        /// </summary>

        public double CashSettledPrice
        {
            get { return _cashSettledPrice; }
            set
            {
                _cashSettledPrice = value;
                PropertyHasChanged(CONST_CASHSETTLEDPRICE);
            }
        }
        private int _intClosingMode;
        /// <summary>
        /// We have created this property as we needed to bind the enum type property to grid combo
        // TODO : Remove this if possible.
        /// </summary>
        [Browsable(false)]
        public int IntClosingMode
        {
            get { return _intClosingMode; }
            set
            {
                _intClosingMode = value;
                _closingMode = (ClosingMode)value;
            }
        }
        private ClosingMode _closingMode = ClosingMode.None;

        /// <summary>
        /// Gets or sets the taxlots which create the position
        /// Child taxlots which creates this position
        /// </summary>
        /// <value>The position allocated trades.</value>

        public ClosingMode ClosingMode
        {
            get { return _closingMode; }
            set
            {
                _closingMode = value;
                _intClosingMode = Convert.ToInt32(_closingMode);
            }
        }
        private string _generatedTaxlot;
        /// <summary>
        /// for physical settlement
        /// </summary>
        [Browsable(false)]
        public string GeneratedTaxlot
        {
            get { return _generatedTaxlot; }
            set { _generatedTaxlot = value; }
        }
        private double _parentClosedQty;
        /// <summary>
        /// for Expiration
        /// </summary>

        public double ParentClosedQty
        {
            get { return _parentClosedQty; }
            set { _parentClosedQty = value; }
        }




        private DateTime _expirationDate;

        /// <summary>
        /// Gets or sets the position start date.
        /// </summary>
        /// <value>The positon start date.</value>
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }
        private string _expirationID = string.Empty;
        /// <summary>
        /// for physical settlement
        /// </summary>

        public string ExpirationID
        {
            get { return _expirationID; }
            set { _expirationID = value; }
        }
        private DateTime _parentPositionEndDate;

        /// <summary>
        /// Gets or sets the position trade date.
        /// </summary>
        /// <value>The positon trade date.</value>
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime ParentPositionEndDate
        {
            get { return _parentPositionEndDate; }
            set { _parentPositionEndDate = value; }
        }

        private double _unRealizedPL = 0.0;
        /// <summary>
        /// Gets or sets the unRealized profit.
        /// </summary>
        /// <value>The unRealized profit.</value>
        [Browsable(false)]
        public double UnRealizedPNL
        {
            get
            {
                return _unRealizedPL;
            }
            set
            {
                _unRealizedPL = value;
            }
        }


        private double _ytdMarkPrice = 0.0;
        /// <summary>
        /// Gets or sets the mark price for today.
        /// </summary>
        /// <value>The mark price for today.</value>
        [Browsable(false)]
        public double YTDMarkPrice
        {
            get { return _ytdMarkPrice; }
            set { _ytdMarkPrice = value; }
        }


        private DateTime _auecLocalCloseDate = DateTimeConstants.MinValue;
        /// <summary>
        /// Kept datatype as string so that it could easily serialized in xml.
        /// Earlier it was AUECLocalDate. Changed on 13 March 08
        /// </summary>
        [Browsable(false)]
        public DateTime AUECLocalCloseDate
        {
            get { return _auecLocalCloseDate; }
            set { _auecLocalCloseDate = value; }
        }

        private DateTime _timeOfSaveUTC = DateTimeConstants.MinValue;
        ///Bring the data in the utc datetime and assign after converting to local date time.
        ///Always returns the local datetime while always sets datetime in UTC.
        ///changed this logic thus we are showing aueclocaldate Abhishek
        public DateTime TimeOfSaveUTC
        {
            get { return _timeOfSaveUTC; }
            set { _timeOfSaveUTC = value; }
        }
        private DateTime _positionStartDate;

        /// <summary>
        /// Gets or sets the position start date.
        /// </summary>
        /// <value>The positon start date.</value>
        [System.Xml.Serialization.XmlIgnore()]
        public DateTime PositionStartDate
        {
            get { return _positionStartDate; }
            set { _positionStartDate = value; }
        }
        private string _groupID;
        [Browsable(false)]
        public string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        private bool _isClosingSaved = true;
        [Browsable(false)]
        public bool IsClosingSaved
        {
            get { return _isClosingSaved; }
            set { _isClosingSaved = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
        [Browsable(false)]
        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }
        /// <summary>
        /// it is only to get the UnderLying Symbol for getting sec master response
        /// </summary>
        private string _underLyingSymbol = string.Empty;
        [Browsable(false)]
        public string UnderLyingSymbol
        {
            get { return _underLyingSymbol; }
            set { _underLyingSymbol = value; }
        }

        private int _underLyingAUECID;
        [Browsable(false)]
        public int UnderLyingAUECID
        {
            get { return _underLyingAUECID; }
            set { _underLyingAUECID = value; }
        }



        /// <summary>
        /// only to get strike price for option
        /// </summary>
        private double _strikePrice = 0.0;
        [Browsable(false)]
        public double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        private BusinessObjects.AppConstants.Underlying _underlying;

        public BusinessObjects.AppConstants.Underlying Underlying
        {
            get { return _underlying; }
            set { _underlying = value; }
        }
        private double _generatedTaxlotQty;

        public double GeneratedTaxlotQty
        {
            get { return _generatedTaxlotQty; }
            set { _generatedTaxlotQty = value; }
        }
        /// <summary>
        /// to get the full security name
        /// </summary>
        private string _securityFullName;

        public string SecurityFullName
        {
            get { return _securityFullName; }
            set
            {
                _securityFullName = value;
                PropertyHasChanged();
            }
        }

        /// <summary>
        /// this is for the options to decide whether call or put 
        /// </summary>
        private int _putOrCall = int.MinValue;
        /// <summary>
        /// Same int as used in Prana.BusinessObjects.FIXConstants class
        /// </summary>
        [Browsable(false)]
        public int PutOrCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
        }
        private string _taxLotClosingId;

        [Browsable(false)]

        public string TaxLotClosingId
        {
            get { return _taxLotClosingId; }
            set { _taxLotClosingId = value; }
        }


        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _ID;

        }



        private SwapParameters _swapParameters;
        [Browsable(false)]
        public SwapParameters SwapParameters
        {
            get { return _swapParameters; }
            set { _swapParameters = value; }
        }

        private SecMasterOTCData _otcParameters;
        [Browsable(false)]
        public SecMasterOTCData OTCParameters
        {
            get { return _otcParameters; }
            set { _otcParameters = value; }
        }

        private bool _isSwapped = false;

        public bool IsSwapped
        {
            get { return _isSwapped; }
            set { _isSwapped = value; }
        }

        private PositionTag _positionTag;

        public PositionTag PositionTag
        {
            get { return _positionTag; }
            set { _positionTag = value; }
        }

        private Int64 _parentRowPk;

        [Browsable(false)]
        public Int64 ParentRowPk
        {
            get { return _parentRowPk; }
            set { _parentRowPk = value; }
        }


        private Int64 _taxlotPk;

        [Browsable(false)]
        public Int64 TaxlotPk
        {
            get { return _taxlotPk; }
            set { _taxlotPk = value; }
        }
        private bool _isExerciseAtZero = false;
        [Browsable(false)]
        public bool IsExerciseAtZero
        {
            get { return _isExerciseAtZero; }
            set { _isExerciseAtZero = value; }
        }

        public double UnitCost
        {
            get
            {
                if (OpenQty == 0)
                {
                    return 0;
                }

                if (SideID.Equals(FIXConstants.SIDE_Buy) || SideID.Equals(FIXConstants.SIDE_Buy_Closed) || SideID.Equals(FIXConstants.SIDE_Buy_Open))
                {

                    return (_averagePrice * _openQty * _multiplier + _openTotalCommissionandFees) / (_openQty * _multiplier);
                }
                else
                {
                    return (_averagePrice * _openQty * _multiplier - OpenTotalCommissionandFees) / (_openQty * _multiplier);
                }

            }
        }



        #region ICloneable Members

        //internal static class ObjectCloner
        // {

        //public virtual AllocatedTrade Clone()
        //{
        //    AllocatedTrade obj = this;
        //    using (MemoryStream buffer = new MemoryStream())
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(buffer, obj);
        //        buffer.Position = 0;
        //        object temp = formatter.Deserialize(buffer);
        //        AllocatedTrade clone = (AllocatedTrade)temp;
        //        return clone;
        //    }
        //}

        public new AllocatedTrade Clone()
        {
            return (AllocatedTrade)GetClone();
        }



        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        //public virtual AllocatedTrade Clone()
        //{

        //    AllocatedTrade clone = new AllocatedTrade();

        //    clone.ID = this.ID;
        //    clone.AUECID = this._auecID;
        //    clone.AveragePrice = this.AveragePrice;
        //    clone.AccountValue = this.AccountValue;
        //    clone.IsPosition = this.IsPosition;
        //    clone.PositionID = this.PositionID;
        //    clone.Quantity = this.Quantity;
        //    clone.SideID = this.SideID;
        //    clone.Side = this.Side;
        //    clone.Symbol = this.Symbol;
        //    clone.TradeDate = this.TradeDate;

        //    return clone;

        //}
        #endregion

        #region Validation Rules
        protected override void AddBusinessRules()
        {
            ValidationRules.AddRule(CustomClass.CashSettledPriceCheck, CONST_CASHSETTLEDPRICE);
            ValidationRules.AddRule(CustomClass.SettleQtyCheck, CONST_SETTLEQTY);
            //ValidationRules.AddRule(CustomClass.CashSettledPriceCheck, CONST_SETTLEMENTMODE);
        }
        [System.Runtime.InteropServices.ComVisible(false)]
        public class CustomClass : RuleArgs
        {
            public CustomClass(string validation)
                : base(validation)
            {
            }

            public static bool CashSettledPriceCheck(object target, RuleArgs e)
            {
                AllocatedTrade finalTarget = target as AllocatedTrade;
                if (finalTarget != null)
                {
                    if (finalTarget._cashSettledPrice < 0.0)
                    {
                        e.Description = "Cash Settlement price should be greater than Zero";
                        return false;
                    }

                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            public static bool SettleQtyCheck(object target, RuleArgs e)
            {
                AllocatedTrade finalTarget = target as AllocatedTrade;
                if (finalTarget != null)
                {
                    if (finalTarget._settledQty > finalTarget._openQty)
                    {
                        // e.Description = "Cash Settled price required";
                        e.Description = "Settlement Qty cannot  greater than Open Qty";
                        return false;
                    }
                    else if (finalTarget._settledQty < 0)
                    {
                        e.Description = "Settlement Qty cannot  zero or negative";
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        #region IKeyable Members

        public string GetKey()
        {
            return this.ID;
        }

        public void Update(IKeyable item)
        {
            AllocatedTrade allocatedTrade = (AllocatedTrade)item;

            this.OpenQty = allocatedTrade.OpenQty;
            this.OpenTotalCommissionandFees = allocatedTrade.OpenTotalCommissionandFees;
            this.ClosedTotalCommissionandFees = allocatedTrade.ClosedTotalCommissionandFees;
            this.AUECLocalCloseDate = allocatedTrade.AUECLocalCloseDate;

        }

        #endregion
    }

}

