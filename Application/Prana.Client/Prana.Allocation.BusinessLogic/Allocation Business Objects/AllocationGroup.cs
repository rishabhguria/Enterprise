using System;
using System.Collections.Generic;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using System.Xml;
using System.ComponentModel;
using System.Xml.Serialization;
using Prana.Utilities.DateTimeUtilities;
using Prana.CommonDataCache;
namespace Prana.Allocation.BLL

{
	/// <summary>
	/// Summary description for AllocationGroup.
	/// </summary>
    [Serializable]
	public class AllocationGroup
	{

        private double _commission = 0;
        private double _fees = 0;
        [XmlIgnore]
        bool _isPreAllocated                    = false;
        int _userID                              = int.MinValue;
		string _groupID                         =string.Empty;
        [XmlIgnore]
        AllocationOrderCollection _orders       = new AllocationOrderCollection();
        [XmlIgnore]
        private string _symbol                  =string.Empty;
        [XmlIgnore]
        private string _orderSide               =string.Empty;
        [XmlIgnore]
        private string _orderSideTagValue       = string.Empty;
        [XmlIgnore]
        private string _listID                  = string.Empty;
        [XmlIgnore]
        private string _counterPartyName        = string.Empty;
        [XmlIgnore]
        private int _counterPartyID             = int.MinValue;
        [XmlIgnore]
        private string _venue                   =string.Empty;
        [XmlIgnore]
        private int  _venueID                   =int.MinValue;
        [XmlIgnore]
		private double  _quantity               =0.0;
        [XmlIgnore]
        private double _cumQty                  =0.0;
        [XmlIgnore]
        private string _tradingAccountName      =string.Empty ;
        [XmlIgnore]
        private int  _tradingAccountID          = int.MinValue;
        [XmlIgnore]
		private double _avgprice                =0.0;
        [XmlIgnore]
        private bool _notAllExecuted            =false;
        [XmlIgnore]
        private bool _autoGrouped               =false;
        [XmlIgnore]
        private bool _updated                   =false;
        [XmlIgnore]
		private string  _orderType;
        [XmlIgnore]
        private string _orderTypeTagValue;
        [XmlIgnore]
		private int _assetID					= Int32.MinValue;
        [XmlIgnore]
        private string _assetName				= string.Empty;
        [XmlIgnore]
        private int _underlyingID				= Int32.MinValue ;
        [XmlIgnore]
        private string _underlyingName			= string.Empty;
        [XmlIgnore]
        private int _exchangeID					= Int32.MinValue ;
        [XmlIgnore]
        private string _exchangeName			= string.Empty;
        [XmlIgnore]
        private int _currencyID					= Int32.MinValue ;
        [XmlIgnore]
        private string _currencyName			= string.Empty;
        [XmlIgnore]
        private int _auecID						= Int32.MinValue ;
        [XmlIgnore]
        private bool _isProrataActive           =false;

        private AllocationFunds _funds                    =null ;

        private AllocationStrategies _strategies   =null ;
        [XmlIgnore]
        private bool _singleOrderAllocation     = false;
        [XmlIgnore]
        private double _allocatedQty            = 0.0;
        [XmlIgnore]
        private bool _allocatedEqualTotalQty    = false;

        [XmlIgnore]
        private DateTime _auecLocalDate = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;
        [XmlIgnore]
        private DateTime _settlementDate = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;
        private DateTime _expirationDate = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;

        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { _expirationDate = value; }
        }
	

      
        [XmlIgnore]
        private PranaInternalConstants.TYPE_OF_ALLOCATION _typeOfAllocation = PranaInternalConstants.TYPE_OF_ALLOCATION.FUND;
        [XmlIgnore]
        private PranaInternalConstants.ORDERSTATE_ALLOCATION _state = PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED;
        [XmlIgnore]
        private Dictionary<string, AllocationOrder> _dictOrders = new Dictionary<string, AllocationOrder>();
             
        public AllocationGroup(PranaInternalConstants.TYPE_OF_ALLOCATION typeOfAllocation)
		{
            _groupID = AllocationIDGenerator.GenerateGroupID();
            _typeOfAllocation = typeOfAllocation;
		}
     
        public AllocationGroup()
        {
            
        }
		
		public AllocationGroup(AllocationOrderCollection orders)
		{
            _groupID = AllocationIDGenerator.GenerateGroupID();
            foreach(AllocationOrder order in orders)
            AddOrder(order);




        }

        #region Commented
        public void SetGroupDetails(AllocationOrder order)
        {
            try
            {
                //double temptotal = 0;
                if (_orders.Count == 1)
                {
                    _symbol = order.Symbol;
                    _orderSide = order.OrderSide;
                    _orderSideTagValue = order.OrderSideTagValue;
                    _counterPartyID = order.CounterPartyID;
                    _counterPartyName = order.CounterPartyName;
                    _venue = order.Venue;
                    _venueID = order.VenueID;
                    _tradingAccountName = order.TradingAccountName;
                    _tradingAccountID = order.TradingAccountID;
                    _orderType = order.OrderType;
                    _orderTypeTagValue = order.OrderTypeTagValue;

                    _exchangeID = order.ExchangeID;
                    _exchangeName = order.ExchangeName;
                    _auecID = order.AUECID;
                    _assetID = order.AssetID;
                    _assetName = order.AssetName;
                    _underlyingID = order.UnderlyingID;
                    _underlyingName = order.UnderlyingName;
                    _listID = order.ListID;
                    _transactionTime = order.TransactionTime;
                    _settlementDate = order.SettlementDate;
                    _expirationDate = order.ExpirationDate;
                    _currencyID = order.CurrencyID;
                    _currencyName = order.CurrencyName;
                   // _auecLocalDate = order.AUECLocalDate;
                    
                    
                }
                else
                {
                    //order.Updated = false;
                   
                   // temptotal = temptotal + (Convert.ToDouble(order.AvgPrice)) * (order.CumQty);
                   
                    if (!_tradingAccountID.Equals(order.TradingAccountID))
                    {
                        _tradingAccountName = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_STR;
                        _tradingAccountID = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_INT;

                    }
                    //if (!_listID.Equals(order.ListID))
                    //{

                    //    _listID = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_STR;

                    //}

                    if (!_venueID.Equals(order.VenueID))
                    {
                        _venue = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_STR;
                        _venueID = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_INT;

                    }
                    if (!_orderTypeTagValue.Equals(order.OrderTypeTagValue))
                    {
                        _orderType = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_STR;
                        _orderTypeTagValue = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_EMPTY;

                    }
                    if (!_counterPartyID.Equals(order.CounterPartyID))
                    {
                        _counterPartyName = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_STR;
                        _counterPartyID = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_INT;

                    }


                    if (_exchangeID != order.ExchangeID)
                    {
                        _exchangeID = 0;

                    }

                    if (!_exchangeName.Equals(order.ExchangeName))
                    {
                        _exchangeName = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_STR;

                    }
                    if (_auecID != order.AUECID)
                    {
                        _auecID = 0;

                    }
                    if (_assetID != order.AssetID)
                    {
                        _assetID = 0;


                    }
                    if (!_assetName.Equals(order.AssetName))
                    {
                        _assetName = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_STR;

                    }


                    if (_underlyingID != order.UnderlyingID)
                    {
                        _underlyingID = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_INT;

                    }
                    if (!_underlyingName.Equals(order.UnderlyingName))
                    {
                        _underlyingName = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_STR;

                    }
                    if (_currencyID != order.CurrencyID)
                    {
                        _currencyID = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_INT;

                    }
                    if (!_currencyName.Equals(order.CurrencyName))
                    {
                        _currencyName = Prana.BusinessObjects.BusinessObjectConstants.MULTIPLE_ID_STR;

                    }
                }

                _quantity = order.Quantity + _quantity;
                double cumQty = this.CumQty;
                double avgPrice = this.AvgPrice;
                if (cumQty < _quantity)
                    _notAllExecuted = true;
                if (_isPreAllocated)
                {
                    _allocatedQty = cumQty;
                }
                if (_allocatedQty == _quantity)
                    _allocatedEqualTotalQty = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }



        }
        #endregion


        #region Private Methods

        public  void AddOrder(AllocationOrder  order)
		{
            try
            {
                if (!_dictOrders.ContainsKey(order.ClOrderID))
                {
                    _dictOrders.Add(order.ClOrderID, order);
                }
                _orders.Add(order);
                SetGroupDetails(order);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        public bool ContainsOrder(string clOrderID)
        {
            return _dictOrders.ContainsKey(clOrderID);
        }
        #endregion


        #region Public Methods
        
        /// <summary>
        /// For Creating Single OrderGroup
        /// </summary>
        /// <param name="order"></param>
        public void CreateGroup(AllocationOrder order)
        {
            _singleOrderAllocation = true;
            _groupID = AllocationIDGenerator.GenerateGroupID();
            AddOrder(order);
           
        }
        /// <summary>
        /// For Creating Multiple Order AllocationGroup
        /// </summary>
        /// <param name="orders"></param>
        public void CreateGroup(AllocationOrderCollection orders)
        {
            _singleOrderAllocation = false;
            _groupID = AllocationIDGenerator.GenerateGroupID();
            foreach (AllocationOrder order in orders)
            {
                AddOrder(order);
            }
        }
       
       
        /// <summary>
        ///  For Unallocating AllocationGroup
        /// </summary>
        public void UnAllocateGroup()
        {
            _state = PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED;
            _updated = false;
        }
      
        public void AllocateGroup(double allocateQty, AllocationFunds funds, bool isProrata)
        {
            _isProrataActive = isProrata;
            _allocatedQty = allocateQty;
            _state = PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED;
            _updated = false;
            if (_isBasketGroup)
            {
                _funds = OrderAllocationManager.GetOrderFundsFrombasketFunds(funds, allocateQty);

                ///[Ashish] Set the Parent value in each fund. So that the parent can be called directly from the fund if required
                foreach (AllocationFund fund in _funds)
                {
                    fund.Parent = this;
                    fund.GroupID = this.GroupID;
                    fund.Commission = this.Commission;
                    fund.Fees = this.Fees;
                }
            }
            else
            {
                _funds = funds;
                ///[Ashish] Set the Parent value in each fund. So that the parent can be called directly from the fund if required
                foreach (AllocationFund fund in _funds)
                {
                    fund.Parent = this;
                    fund.GroupID = this.GroupID;
                }
            }

        }
        public void  AllocateGroup(double allocateQty, AllocationStrategies strategies, bool proRata)
        {
            _isProrataActive = proRata;
            _strategies = strategies;
            _allocatedQty = allocateQty;
            _state = PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED;
            _updated = false;
            if (_isBasketGroup)
            {
                _strategies = OrderAllocationManager.GetOrderStrategiesFrombasketFunds(strategies, allocateQty);
            }
            else
            {
                _strategies = strategies;
            }

        }
        public AllocationOrder GetOrder(string clOrderID)
        {
            return _dictOrders[clOrderID];
        }

        #endregion

	
		#region Properties


       
        public AllocationOrderCollection Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
               // SetGroupDetails();


            }

        }

        public AllocationFunds AllocationFunds
        {
            set {  _funds=value; }
            get { return _funds; }
        }

        public AllocationStrategies Strategies
        {
            set { _strategies = value; }
            get { return _strategies; }
        }
        [XmlIgnore]
        public bool SingleOrderAllocation
        {
            get { return _singleOrderAllocation; }
             set { _singleOrderAllocation=value ; }
        }
        [XmlIgnore]
		public string OrderType
		{
			get{return _orderType;}
			set 
			{_orderType=value;}
		
		}
        [XmlIgnore]
        public string OrderTypeTagValue
        {
            get { return _orderTypeTagValue; }
            set
            { _orderTypeTagValue = value; }

        }
        [XmlIgnore]
		public string Symbol
		{
			set{_symbol=value;}
			get{return _symbol;}
		}
        [XmlIgnore]
		public string OrderSide
		{
			set{_orderSide=value;}
			get{return _orderSide;}
		}
        [XmlIgnore]
        public string OrderSideTagValue
        {
            set { _orderSideTagValue = value; }
            get { return _orderSideTagValue; }
        }
        [XmlIgnore]
		public string CounterPartyName
		{
			set{_counterPartyName=value;}
			get{return _counterPartyName;}
		}
        [XmlIgnore]
        public int CounterPartyID
        {
            set { _counterPartyID= value; }
            get { return _counterPartyID; }
        }
        [XmlIgnore]
		public string Venue
		{
			set{_venue=value;}
			get{return _venue;}
		}
        [XmlIgnore]
        public int VenueID
        {
            set { _venueID = value; }
            get { return _venueID; }
        }
        [XmlIgnore]
		public double Quantity
		{
			set{_quantity=value;}
			get{return _quantity;}
		}
        [XmlIgnore]
        public double CumQty
		{
			set{_cumQty=value;}
            get
            {
                if (_isManualGroup)
                {
                    return _cumQty;
                }
                double cumQty = 0.0;
                foreach (AllocationOrder tempOrder in _orders)
                {
                    cumQty = cumQty + tempOrder.CumQty;
                }
                return cumQty;
            }
		}
        [XmlIgnore]
		public double AvgPrice
		{
			set{_avgprice=value;}
			get{
                if (_isManualGroup)
                {
                    return _avgprice;
                }
                double avgprice = 0.0;
                double temptotal = 0.0;
                foreach (AllocationOrder tempOrder in _orders)
                {
                    temptotal = temptotal + tempOrder.AvgPrice * tempOrder.CumQty;
                }
                if (this.CumQty != 0)
                    avgprice = temptotal / this.CumQty ;
                else
                    avgprice = 0.0;

                return avgprice;
            }
		}
		public string  GroupID
		{
			set{_groupID=value;}
			get{return _groupID;}
		}		
		public bool AutoGrouped 
		{
			set{_autoGrouped=value;}
			get{return _autoGrouped;}
		}
		public bool Updated 
		{
			set{_updated=value;}
			get{return _updated;}
		}
        [XmlIgnore]
		public string  TradingAccountName
		{
			set{_tradingAccountName=value;}
			get{return _tradingAccountName;}
		}
        [XmlIgnore]
        public int TradingAccountID
        {
            set { _tradingAccountID = value; }
            get { return _tradingAccountID; }
        }
        [XmlIgnore]
		public int  AssetID
		{
			get{return _assetID;}
			set{_assetID = value;}
		}
        [XmlIgnore]
		public string  AssetName
		{
			get{return _assetName;}
			set{_assetName = value;}
		}
        [XmlIgnore]
		public int  UnderLyingID
		{
			get{return _underlyingID;}
			set{_underlyingID = value;}
		}
        [XmlIgnore]
		public string  UnderLyingName
		{
			get{return _underlyingName;}
			set{_underlyingName = value;}
		}
        [XmlIgnore]
		public int  ExchangeID
		{
			get{return _exchangeID;}
			set{_exchangeID = value;}
		}
        [XmlIgnore]
		public string  ExchangeName
		{
			get{return _exchangeName;}
			set{_exchangeName = value;}
		}
        [XmlIgnore]
		public int  CurrencyID
		{
			get{return _currencyID;}
			set{_currencyID = value;}
		}
        [XmlIgnore]
		public string  CurrencyName
		{
			get{return _currencyName;}
			set{_currencyName = value;}
		}
        [XmlIgnore]
		public int  AUECID
		{
			get{return _auecID;}
			set{_auecID = value;}
		}
        [XmlIgnore]
        public string ListID
        {
            get { return _listID; }
            set { _listID = value; }
        }
        [XmlIgnore]
        public bool IsProrataActive
        {
            set { _isProrataActive = value; }
            get { return _isProrataActive; }
        }
        [XmlIgnore]
        public double AllocatedQty
        {
            set { _allocatedQty = value; }
            get { return _allocatedQty; }
        }
        public int UserID
        {
            set { _userID  = value; }
            get { return _userID; }
        }
        public bool  IsPreAllocated
        {
            set { _isPreAllocated  = value; }
            get { return _isPreAllocated; }
        }
        [XmlIgnore]
        public bool NotAllExecuted
        {
            set { _notAllExecuted = value; }
            get { return _notAllExecuted; }
        }
        [XmlIgnore]
        public bool AllocatedEqualTotalQty
        {
            get { return _allocatedEqualTotalQty; }
            set { _allocatedEqualTotalQty = value; }
        }
        [XmlIgnore]
        public PranaInternalConstants.ORDERSTATE_ALLOCATION State
        {
            get { return _state; }
            set { _state = value; } 
        }
        [XmlIgnore]
        public PranaInternalConstants.TYPE_OF_ALLOCATION AllocationType
        {
            get { return _typeOfAllocation; }
            set { _typeOfAllocation = value; }
        }
        [XmlIgnore]
        public DateTime AUECLocalDate
        {
            get { return _auecLocalDate; }
            set
            {
                if ((DateTime.Parse(value.ToString())).Equals(Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue))
                {
                    _auecLocalDate = TimeZoneHelper.GetAUECLocalDateFromUTC(this.AUECID, DateTime.UtcNow);
                }
                else
                {
                    _auecLocalDate = value;
                }
            }
        }
        [Browsable(false)]
        public DateTime SettlementDate
        {
            get { return _settlementDate; }
            set { _settlementDate = value; }
        }

		#endregion

        public void Update(AllocationOrder order)
        {
            
            AllocationOrder oldOrder= _dictOrders[order.ClOrderID];
            _quantity = _quantity - oldOrder.Quantity + order.Quantity;
            // Because of Replace
            oldOrder.Quantity = order.Quantity;
            if (oldOrder.CumQty != order.CumQty)
            {
                // Update Old Order
                oldOrder.CumQty = order.CumQty;
                oldOrder.AvgPrice = order.AvgPrice;
                double cumQty = this.CumQty;
                double avgPrice = this.AvgPrice;
                // If Pre Alocated Set Allocated Qty= CumQty
                if (_isPreAllocated)
                {
                    _allocatedQty = cumQty;
                    TotalAllocatedQty(order);
                }
                // Set Not All Excecuted Flag
                if (cumQty < _quantity)
                    _notAllExecuted = true;
               
              
                // Set Updated Flag
                _updated = true;
            }
            
               
        }

        private bool _isBasketGroup=false;

        public bool IsBasketGroup
        {
            get { return _isBasketGroup; }
            set { _isBasketGroup = value; }
        }
        private string _basketGroupID;

        public string BasketGroupID
        {
            get { return _basketGroupID; }
            set { _basketGroupID = value; }
        }

        public double Commission 
        {
            get 
            { 
                return _commission; 
            }
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
            if (_funds != null)
            {
                double commission = 0.0;
               
                foreach (AllocationFund allocationFund in _funds)
                {
                    commission += allocationFund.Commission;
                    
                }
                if (_commission!=commission)
                {
                    foreach (AllocationFund allocationFund in _funds)
                    {
                        allocationFund.Commission = (allocationFund.AllocatedQty / _allocatedQty) * _commission;
                    }
                }
                _isCommissionCalculated = false;
            }
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
            if (_funds != null)
            {
                double fees = 0.0;
                foreach (AllocationFund allocationFund in _funds)
                {
                    fees += allocationFund.Fees;

                }
                if (_fees != fees)
                {
                    foreach (AllocationFund allocationFund in _funds)
                    {
                        //allocationFund.Fees = (allocationFund.AllocatedQty / _allocatedQty) * _fees;
                        allocationFund.Fees = (allocationFund.AllocatedQty / _allocatedQty) * _fees;
                    }
                }
                _isCommissionCalculated = false;
            }
        }

        public void RecalculateGroupCommissionFromTaxlotComm()
        {
            if (_commissionCalculationTime.Equals(true)) 
            {
                double new_commission = 0;

                foreach (AllocationFund fund in _funds)
                {
                    new_commission += fund.Commission;
                }
                _commission = new_commission;
                _isCommissionCalculated = false;
            }
        }

        public void RecalculateGroupFeesFromTaxlotComm()
        {
            if (_commissionCalculationTime.Equals(true))
            {
                double new_Fees = 0;

                foreach (AllocationFund fund in _funds)
                {
                    new_Fees += fund.Fees;
                }
                _fees = new_Fees;
                _isCommissionCalculated = false;
            }
        }

        [XmlIgnore]
        private bool  _commissionCalculationTime =false ;

        [Browsable(false)]
        public bool  CommissionCalculationTime
        {
            get { return _commissionCalculationTime; }
            set { _commissionCalculationTime = value; }
        }

        private bool  _isCommissionCalculated=false;

        public bool  IsCommissionCalculated
        {
            get { return _isCommissionCalculated; }
            set { _isCommissionCalculated = value; }
        }

        [XmlIgnore]
        private bool _isManualGroup = false;

        public bool IsManualGroup
        {
            get { return _isManualGroup; }
            set { _isManualGroup = value; }
        }
        [XmlIgnore]
        private string _commissionText = "Calculated";

        public string CommissionText
        {
            get { return _commissionText; }
            set { _commissionText = value; }
    
        }

        public void TotalAllocatedQty(AllocationOrder order)
        {
            if (_funds != null)
            {
                foreach (AllocationFund fund in _funds)
                {
                    fund.AllocatedQty = order.CumQty;
                }
            }
        }
        private string _transactionTime = string.Empty;
        [Browsable(false)]
        public string TransactionTime
        {
            get { return _transactionTime; }
            set { _transactionTime = value; }
        }
        public AllocationGroup Clone()
        {
            AllocationGroup allocationGroup = new AllocationGroup();
            allocationGroup.AllocatedEqualTotalQty = this.AllocatedEqualTotalQty;
            allocationGroup.AllocatedQty = this.AllocatedQty;
            allocationGroup.AllocationFunds = this.AllocationFunds;
            allocationGroup.AllocationType = this.AllocationType;
            allocationGroup.AssetID = this.AssetID;
            allocationGroup.AssetName = this.AssetName;
            allocationGroup.AUECID = this.AUECID;
            allocationGroup.AUECLocalDate = this.AUECLocalDate;
            allocationGroup.AutoGrouped = this.AutoGrouped;
            allocationGroup.AvgPrice = this.AvgPrice;
            allocationGroup.BasketGroupID = this.BasketGroupID;
            allocationGroup.Commission = this.Commission;
            allocationGroup.CommissionCalculationTime = this.CommissionCalculationTime;
            allocationGroup.CommissionText = this.CommissionText;
            allocationGroup.CounterPartyID = this.CounterPartyID;
            allocationGroup.CounterPartyName = this.CounterPartyName;
            allocationGroup.CumQty = this.CumQty;
            allocationGroup.CurrencyID = this.CurrencyID;
            allocationGroup.CurrencyName = this.CurrencyName;
            allocationGroup.ExchangeID = this.ExchangeID;
            allocationGroup.ExchangeName = this.ExchangeName;
            allocationGroup.Fees = this.Fees;
            allocationGroup.GroupID = this.GroupID;
            allocationGroup.IsBasketGroup = this.IsBasketGroup;
            allocationGroup.IsCommissionCalculated = this.IsCommissionCalculated;
            allocationGroup.IsManualGroup = this.IsManualGroup;
            allocationGroup.IsPreAllocated = this.IsPreAllocated;
            allocationGroup.IsProrataActive = this.IsProrataActive;
            allocationGroup.ListID = this.ListID;
            allocationGroup.NotAllExecuted = this.NotAllExecuted;
            allocationGroup.Orders = this.Orders;
            allocationGroup.OrderSide = this.OrderSide;
            allocationGroup.OrderSideTagValue = this.OrderSideTagValue;
            allocationGroup.OrderType = this.OrderType;
            allocationGroup.OrderTypeTagValue = this.OrderTypeTagValue;
            allocationGroup.Quantity = this.Quantity;
            allocationGroup.SingleOrderAllocation = this.SingleOrderAllocation;
            allocationGroup.State = this.State;
            allocationGroup.Strategies = this.Strategies;
            allocationGroup.Symbol = this.Symbol;
            allocationGroup.TradingAccountID = this.TradingAccountID;
            allocationGroup.TradingAccountName = this.TradingAccountName;
            allocationGroup.TransactionTime = this.TransactionTime;
            allocationGroup.UnderLyingID = this.UnderLyingID;
            allocationGroup.UnderLyingName = this.UnderLyingName;
            allocationGroup.Updated = this.Updated;
            allocationGroup.UserID = this.UserID;
            allocationGroup.Venue = this.Venue;
            allocationGroup.VenueID = this.VenueID;
            allocationGroup.SettlementDate = this.SettlementDate;
            allocationGroup.CurrencyID = this.CurrencyID;
            return allocationGroup;

        }
		
	}
}
