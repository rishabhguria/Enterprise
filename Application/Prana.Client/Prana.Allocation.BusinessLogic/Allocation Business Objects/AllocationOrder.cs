using System;
using System.Collections.Generic;
using System.Text;
using Prana.Global;
using Prana.BusinessObjects;
using System.Xml.Serialization;
using System.ComponentModel;
namespace Prana.Allocation.BLL
{
    public class AllocationOrder
    {
        #region private Variable

        private string _clOrderID               = string.Empty;
		private string _orderSide				= string.Empty;
        private string _orderSideTagValue       = string.Empty;
		private string _orderType				= string.Empty;
        private string _orderTypeTagValue       = string.Empty;        
		private string _symbol					= string.Empty;
		private double _quantity				= double.Epsilon;
		private double _avgPrice				= double.Epsilon;
        private double _commission              = double.Epsilon;
        private double _fees                    = double.Epsilon;
        private string  _venue					= string.Empty;
		private int		_venueID				= int.MinValue;
		private int		 _counterPartyID		= int.MinValue;
		private string _counterPartyName		= string.Empty;
        private string _tradingAccountName      = string.Empty;
        private int _tradingAccountID           = int.MinValue; 		
		private int		_userID			        = int.MinValue;
        private int _assetID                    = int.MinValue;
		private string _assetName				= string.Empty;
        private int _underlyingID               = int.MinValue;
		private string _underlyingName			= string.Empty;
        private int _exchangeID                 = int.MinValue;
		private string _exchangeName			= string.Empty;
        private int _currencyID                 = int.MinValue;
		private string _currencyName			= string.Empty;
        private int _auecID                     = int.MinValue;
        private string _listID                  = string.Empty;
		private double   _cumQty;
		private bool _updated                   =false;
		private bool _notAllExecuted            =false;
        private double _allocatedQty = 0;
        private string _groupID                 = string.Empty;
        private int _fundID                     = int.MinValue;
        private int _strategyID                 = int.MinValue;
        private string _origClOrderID           = string.Empty;
        private int _allocationTypeID           = (int)PranaInternalConstants.TYPE_OF_ALLOCATION.NOTSET;
        private string _openClose = string.Empty;
        //private System.Nullable<DateTime> _auecLocalDate = null;
        private DateTime _auecLocalDate = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;
        private AllocationFunds _funds = null;
        private DateTime _groupAuecLocalDate = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;
        private PranaInternalConstants.ORDERSTATE_ALLOCATION _stateID = PranaInternalConstants.ORDERSTATE_ALLOCATION.UNALLOCATED;
        private DateTime _settlementDate = DateTimeConstants.MinValue;
        //private bool _commissionCalculationTime = false;
        //private float _commission ;
        //private float _fees ;

		#endregion

        public AllocationOrder()
		{
			
		}
        private DateTime _expirationDate = DateTimeConstants.MinValue;

        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }





        #region Properties

        
        public AllocationFunds AllocationFunds
        {
            set { _funds = value; }
            get { return _funds; }
        }
        public string OrderSideTagValue
		{
			get{return _orderSideTagValue;}
			set{_orderSideTagValue = value;}
		}
        public string OrderSide
        {
            get { return _orderSide; }
            set { _orderSide = value; }
        }		 
		public string OrderType
		{
			get{return _orderType;}
			set{_orderType = value;}
		}
        public string OrderTypeTagValue
        {
            get { return _orderTypeTagValue; }
            set { _orderTypeTagValue = value; }
        }
		public string Symbol
		{
			get{return _symbol;}
			set{_symbol = value;}
		}
		public string Venue
		{
			get{return _venue;}
			set{_venue = value;}
		}
		public double Quantity
		{
			get{return _quantity;}
			set{_quantity = value;}
		}
		public string ClOrderID
		{
			get{return _clOrderID;}
			set{_clOrderID = value;}
		}
		public double AvgPrice
		{
			get{return _avgPrice;}
			set{_avgPrice = value;}
		}
		public int  AssetID
		{
			get{return _assetID;}
			set{_assetID = value;}
		}
		public string  AssetName
		{
			get{return _assetName;}
			set{_assetName = value;}
		}
		public int  UnderlyingID
		{
			get{return _underlyingID;}
			set{_underlyingID = value;}
		}
		public string  UnderlyingName
		{
			get{return _underlyingName;}
			set{_underlyingName = value;}
		}
		public int  ExchangeID
		{
			get{return _exchangeID;}
			set{_exchangeID = value;}
		}
		public string  ExchangeName
		{
			get{return _exchangeName;}
			set{_exchangeName = value;}
		}
		public int  CurrencyID
		{
			get{return _currencyID;}
			set{_currencyID = value;}
		}
        public double  Commission
        {
            get { return _commission; }
            set
            {
                _commission = value;
                if (_commissionCalculationTime.Equals(false))
                {
                    CalculateTotalCommission();
                }
            }
        }
        public void CalculateTotalCommission()
        {
            foreach (AllocationFund allocationFund in _funds)
            {
                //allocationFund.Commission = (allocationFund.AllocatedQty / _allocatedQty) * _commission;
                allocationFund.Commission = (allocationFund.AllocatedQty / CumQty) * _commission;
            }
            _isCommissionCalculated = false;
        }

        public double Fees
        {
            get { return _fees; }
            set
            {
                _fees = value;
                if (_commissionCalculationTime.Equals(false))
                {
                    CalculateTotalFees();
                }
            }
        }
        public void CalculateTotalFees()
        {
            foreach (AllocationFund allocationFund in _funds)
            {
                //allocationFund.Fees = (allocationFund.AllocatedQty / _allocatedQty) * _fees;
                allocationFund.Fees = (allocationFund.AllocatedQty / CumQty) * _fees;
            }
            _isCommissionCalculated = false;
        }
        public void RecalculateGroupCommAndFee_FromTaxlotCommforBasket()
        {
            if (_commissionCalculationTime.Equals(true))
            {
                double new_commission = 0;
                double new_Fees = 0;

                foreach (AllocationFund fund in _funds)
                {
                    new_commission += fund.Commission;
                    new_Fees += fund.Fees;
                }
                _commission = new_commission;
                _fees = new_Fees;
                _isCommissionCalculated = false;
            }
        }
        public void AllocateGroupToFund(AllocationFunds funds, double allocatedQty)
        {

            _funds = funds;
            foreach (AllocationFund allocationFund in _funds)
            {
                allocationFund.ParentBasketGroup = this;
            }
            _allocatedQty = allocatedQty;
            // SetEntityValues();



        }
		public string  CurrencyName
		{
			get{return _currencyName;}
			set{_currencyName = value;}
		}
		public int  AUECID
		{
			get{return _auecID;}
			set{_auecID = value;}
		}
		public int TradingAccountID
		{
			get{return _tradingAccountID;}
			set{_tradingAccountID = value;}
		}
		public string  TradingAccountName
		{
			get{return _tradingAccountName;}
			set{_tradingAccountName = value;}
		}
		public int UserID
		{
			get{return _userID;}
            set { _userID = value; }
		}
		public int CounterPartyID
		{
			get{return _counterPartyID;}
			set{_counterPartyID = value;}
		}
		public string  CounterPartyName
		{
			get{return _counterPartyName;}
			set{_counterPartyName = value;}
		}
		public int VenueID
		{
			get{return _venueID;}
			set{_venueID = value;}
		}
		public double  CumQty
		{
			set{_cumQty=value;}
			get{return _cumQty;}
		}
        public double AllocatedQty
		{
			set{_allocatedQty=value;}
			get{return _allocatedQty;}
		}
		public bool Updated
		{
			set{_updated=value;}
			get{return _updated;}
		}
		public bool NotAllExecuted
		{
			set{_notAllExecuted=value;}
			get{return _notAllExecuted;}
        }
        public string ListID
        {
            get { return _listID; }
            set { _listID = value; }
        }
        public string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }
        public PranaInternalConstants.ORDERSTATE_ALLOCATION StateID
        {
            get { return _stateID; }
            set { _stateID = value; }
        }
        public int FundID
        {
            get { return _fundID; }
            set { _fundID = value; }
        }
        public int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }
        public int AllocationTypeID
        { 
            get {return  _allocationTypeID;}
            set {_allocationTypeID=value ;}
        }
        public string OpenClose
        {
            get { return _openClose; }
            set { _openClose = value; }
        }
        //public DateTime AUECLocalDate
        //{
        //    get { return _auecLocalDate.Value; }
        //    set { _auecLocalDate = value; }
        //}
        public DateTime AUECLocalDate
        {
            get { return _auecLocalDate; }
            set
            {
                if ((DateTime.Parse(value.ToString())).Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
                {
                    _auecLocalDate = Prana.CommonDataCache.TimeZoneHelper.GetAUECLocalDateFromUTC(this.AUECID, DateTime.UtcNow);
                }
                else
                {
                    _auecLocalDate = value;
                }

            }
        }
        public DateTime GroupAuecLocalDate
        {
            get { return _groupAuecLocalDate; }
            set
            {
                if ((DateTime.Parse(value.ToString())).Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
                {
                    _groupAuecLocalDate = Prana.CommonDataCache.TimeZoneHelper.GetAUECLocalDateFromUTC(this.AUECID, DateTime.UtcNow);
                }
                else
                {
                    _groupAuecLocalDate = value;
                }

            }
        }
        /// <summary>
        /// Tag 41
        /// </summary>
        public string OrigClOrderID
        {
            get { return _origClOrderID; }
            set { _origClOrderID = value; }
        }
        private bool _isCommissionCalculated = false;

        public bool IsCommissionCalculated
        {
            get { return _isCommissionCalculated; }
            set { _isCommissionCalculated = value; }
        }
        [XmlIgnore]
        private bool _commissionCalculationTime = false;

        [Browsable(false)]
        public bool CommissionCalculationTime
        {
            get { return _commissionCalculationTime; }
            set { _commissionCalculationTime = value; }
        }
        private string _transactionTime= string.Empty;
        public string TransactionTime
        {
            get { return _transactionTime; }
            set { _transactionTime = value; }
        }

        public DateTime  SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }
        //public float  Commission
        //{
        //    get { return _commission; }
        //    set 
        //    { 
        //        _commission = value;
        //        //Set
        //    }
        //}
        //public float Fees
        //{
        //    get { return _fees; }
        //    set { _fees = value; }
        //}	
        #endregion

        public AllocationOrder Clone()
        {
            AllocationOrder order = new AllocationOrder();

            order.ClOrderID = this.ClOrderID;
            order.OrderSideTagValue = _orderSideTagValue;
            order.OrderSide = this.OrderSide;
            order.OrderTypeTagValue = this.OrderTypeTagValue;
            order.OrderType = _orderType ;
            order.Symbol = this.Symbol;
            order.AllocationTypeID = _allocationTypeID;
            order.GroupID = _groupID ;
            order.ListID = _listID;
            order.NotAllExecuted = _notAllExecuted;
            order.Updated = _updated;


            order.AssetID = this.AssetID;
            order.AssetName = this.AssetName;
            order.UnderlyingID = this.UnderlyingID;
            order.UnderlyingName = this.UnderlyingName;
            order.ExchangeID = this.ExchangeID;
            order.ExchangeName = this.ExchangeName;
            order.CurrencyID = this.CurrencyID;
            order.AUECID = _auecID;

            order.CounterPartyID = this.CounterPartyID;
            order.CounterPartyName = this.CounterPartyName;
            order.Venue = this.Venue;
            order.VenueID = this.VenueID;
            order.TradingAccountID = this.TradingAccountID;
            order.TradingAccountName = this.TradingAccountName;
            order.FundID = _fundID;
            order.StrategyID = _strategyID;
            //order.Commission = this.Commission;
            //order.Fees = this.Fees;

            order.AvgPrice = this.AvgPrice;
            order.Quantity = this.Quantity;
            order.CumQty = this.CumQty;
            order.AllocatedQty = this.AllocatedQty;
            order.Quantity = this.Quantity;
            order.AUECLocalDate = this.AUECLocalDate;
            order.GroupAuecLocalDate = this.GroupAuecLocalDate;
            order.SettlementDate = this.SettlementDate;
            //order.Commission = this.Commission;
            //order.Fees = this.Fees;
            

            order.UserID = _userID;
            

            return order;
        }
        public void Update(AllocationOrder order)
        {
            // Because of Replace  Order Qty May Change
            //_quantity = order.AllocatedQty;

            if (_cumQty != order.CumQty)
            {
                _cumQty = order.CumQty;
                _avgPrice = order.AvgPrice;
                _updated = true;
                if (_quantity == _cumQty)
                    _notAllExecuted = false;
            }


        }
       

    }
}
