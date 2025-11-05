//using System;

//namespace Prana.Allocation.BLL

//{
//    /// <summary>
//    /// Summary description for Allocation.
//    /// </summary>
//    public class GroupedEntity
//    {
//        //string _fundAccountGroupID;
//        #region Private Members
//        private AllocationGroup _group;
//        private AllocationFunds _funds;
//        private AllocationStrategies  _companyStrategies;

//        string _groupedEntityID;	
//        private string _clOrderID=string.Empty ;
//        private string _orderType= string.Empty;
//        private string _orderTypeTagValue = string.Empty;
//        private string _symbol;
//        private string _orderSide;
//        private string _orderSideTagValue;
//        private int  _counterPartyID=int.MinValue;
//        private string _counterPartyName;
//        private string _venue;
//        private int  _venueID=int.MinValue;
//        private double _cumQty;
//        private double _quantity;
//        private string _tradingAccountName=string.Empty;
//        private string _tradingAccountID = string.Empty;
//        private double _price;
//        private double _allocatedQty = 0;
//        private bool _updated;		
//        private bool _allocatedEqualTotalQty;
//        private bool _isProrataActive;
        
		
//        private int _assetID					= Int32.MinValue;
//        private string _assetName				= string.Empty;
//        private int _underlyingID				= Int32.MinValue ;
//        private string _underlyingName			= string.Empty;
//        private int _exchangeID					= Int32.MinValue ;
//        private string _exchangeName			= string.Empty;
//        private int _currencyID					= Int32.MinValue ;
//        private string _currencyName			= string.Empty;
//        private int _auecID						= Int32.MinValue;
//        private string _listID			= string.Empty;
//        #endregion


//        public GroupedEntity()
//        {
//            _groupedEntityID = IDGenerator.GenerateOrderEntityID();
//            _group = new AllocationGroup();
//            _funds= new AllocationFunds();
//            _companyStrategies= new AllocationStrategies();
			

//        }
		

//        public GroupedEntity( string entityid,AllocationGroup group,AllocationFunds funds)
//        {
//            _groupedEntityID=entityid;	
//            _group = group;
//            _funds= funds;
			

//        }
//        public GroupedEntity( string entityid,AllocationGroup group,AllocationFunds funds,AllocationStrategies companyStrategies)
//        {
//            _groupedEntityID=entityid;	
//            _group = group;
//            _funds= funds;
//            _companyStrategies=	companyStrategies;
			

//        }
//        public GroupedEntity( string entityid,AllocationGroup group,AllocationStrategies companyStrategies)
//        {
//            _groupedEntityID=entityid;	
//            _group = group;
			
//            _companyStrategies=	companyStrategies;
			

//        }

//        public AllocationGroup GetGroup()
//        {
//        return _group;
//        }
//        public Order  GetOrder()
//        {
//            Order  order= new Order ();
//            order.ClOrderID=_clOrderID;
//            order.CumQty=_cumQty;
//            order.Quantity=_quantity;
//            if(_cumQty < _quantity)
//                order.NotAllExecuted=true;
//            else
//                order.NotAllExecuted=false;
//            order.OrderType=_orderType;
//            order.OrderTypeTag = _orderTypeTagValue;

			
//            order.Symbol=_symbol;
//            order.OrderSide=_orderSide;
//            order.SideTagValue = _orderSideTagValue;
//            order.CounterPartyName=_counterPartyName;
//            order.CounterPartyID = _counterPartyID;
//            order.Venue=_venue;
//            order.VenueID = _venueID;
//            order.AvgPrice=_price;
			
			
//            order.TradingAccountName=_tradingAccountName;
//            order.TradingAccountID = _tradingAccountID ;	
//            order.AllocatedQty=_allocatedQty;
//            order.AssetID=_assetID;
//            order.AssetName=_assetName;
//            order.UnderlyingID=_underlyingID;
//            order.UnderlyingName=_underlyingName;
//            order.ExchangeID=_exchangeID;
//            order.ExchangeName =_exchangeName;
			
//            return order;
//        }



//        public void AllocateGroupToFund(AllocationGroup group, AllocationFunds funds, double allocatedQty)
//        {
//            _group = group;
//            _funds= funds;
//            _allocatedQty=allocatedQty;			
//            SetEntityValues();
			
			
			
//        }
//        public void AllocateGroupToStrategy(AllocationGroup group, AllocationStrategies companyStrategies, double allocatedQty)
//        {
//            _group = group;
//            _companyStrategies= companyStrategies;
//            _allocatedQty=allocatedQty;			
//            SetEntityValues();
			
//        }
//        public void AllocateOrderToFund(Order order, AllocationFunds funds, double allocatedQty)
//        {
//            _allocatedQty=allocatedQty;	
//            order.AllocatedQty=allocatedQty;
//            SetEntityValues(order);
//            _funds= funds;
						
		
//        }
//        public void AllocateOrderToStrategy(Order order, AllocationStrategies companyStrategies, double allocatedQty)
//        {
//            _allocatedQty=allocatedQty;	
//            order.AllocatedQty=allocatedQty;
//            SetEntityValues(order);
//            _companyStrategies= companyStrategies;
			
						
		
//        }
//        public void SetEntityValues()
//        {		
//            try
//            {
//                _clOrderID=Constants.MULTIPLE;
//                _symbol=_group.Symbol;
//                _orderSide=_group.OrderSide;
//                _orderSideTagValue = _group.OrderSideTagValue;
//                _counterPartyName=_group.CounterPartyName;
//                _counterPartyID = _group.CounterPartyID;
//                _venue=_group.Venue;
//                _venueID = _group.VenueID;
//                _price=_group.AvgPrice;
//                _cumQty=_group.CumQty;
//                _quantity=_group.Quantity;
//                _tradingAccountName=_group.TradingAccountName;
//                _tradingAccountID = _group.TradingAccountID;
//                _orderType=_group.OrderType;
//                _orderTypeTagValue = _group.OrderTypeTagValue;
//                _auecID=_group.AUECID ;
//                _assetID=_group.AssetID;
//                _assetName=_group.AssetName;
//                _underlyingID=_group.UnderlyingID;
//                _underlyingName=_group.UnderlyingName;
//                _exchangeID=_group.ExchangeID;
//                _exchangeName=_group.ExchangeName ;
//                _listID = _group.ListID;
//                if(_allocatedQty== _quantity)
//                    _allocatedEqualTotalQty=true;
//                else
//                    _allocatedEqualTotalQty=false;
//            }
//            catch(Exception ex)
//            {
//                throw ex;
//            }
				
//        }

//        public void SetEntityValues(Order  order)
//        {		
//            try
//            {
//                if(_clOrderID==string.Empty)
//                    _clOrderID=order.ClOrderID;

//                if(order.ClOrderID!=_clOrderID)
//                    _clOrderID=Constants.MULTIPLE;
			
//                _symbol=order.Symbol;
//                _orderSide=order.OrderSide;
//                _orderSideTagValue = order.SideTagValue;
//                _counterPartyName=order.CounterPartyName;
//                _counterPartyID = order.CounterPartyID;
//                _venue=order.Venue;
//                _venueID = order.VenueID;
//                _price=order.AvgPrice;
//                _cumQty=order.CumQty;
//                _quantity=order.Quantity;
//                _tradingAccountName=order.TradingAccountName;
//                _tradingAccountID  = order.TradingAccountID;				
//                _orderType=order.OrderType;
//                _orderTypeTagValue = order.OrderTypeTag;
////				_allocatedQty=order.AllocatedQty;
//                _auecID=order.AUECID ;
//                _assetID=order.AssetID;
//                _assetName=order.AssetName;
//                _underlyingID=order.UnderlyingID;
//                _underlyingName=order.UnderlyingName;
//                _exchangeID=order.ExchangeID;
//                _exchangeName=order.ExchangeName ;
//                _listID = order.ListID;
//                if(_allocatedQty== _quantity)
//                    _allocatedEqualTotalQty=true;
//                else
//                    _allocatedEqualTotalQty=false;
//            }
//            catch(Exception ex)
//            {
//                throw ex;
//            }
//        }
//        public void ReAllocatePreAllocatedOrderToFund(Order order)
//        {
//            try
//            {
//                    _allocatedQty = order.CumQty;
//                    ((AllocationFund)_funds[0]).OrderQty = _allocatedQty;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
//        public void ReAllocatePreAllocatedOrderToStrategy(Order order)
//        {
//            try
//            {
//                _allocatedQty = order.CumQty;
//                ((AllocationStrategy)_companyStrategies[0]).OrderQty = _allocatedQty;

//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }
		
//        public AllocationFunds GetFunds()
//        {
//            return _funds;
		
		
//        }
//        public AllocationStrategies  GetStrategies()
//        {
//            return _companyStrategies;
//        }

//        public string ClOrderID
//        {
//            set{_clOrderID =value;}
//            get{return _clOrderID;}
//        }
		
//        public string Symbol
//        {
//            set{_symbol=value;}
//            get{return _symbol;}
//        }
		
		
//        public AllocationGroup AllocationGroup
//        {
//            set{_group =value;}
//            get{return _group;}

			
//        }
//        public AllocationFunds AllocationFunds
//        {
//            set{_funds =value;}
//            get{return _funds;}
			
//        }
//        public AllocationStrategies AllocationStrategies
//        {
//            set{_companyStrategies =value;}
//            get{return _companyStrategies;}
			
//        }

//        public string OrderSide
//        {
//            set{_orderSide=value;}
//            get{return _orderSide;}
//        }
//        public string OrderSideTagValue
//        {
//            set { _orderSideTagValue = value; }
//            get { return _orderSideTagValue; }
//        }
//        public string CounterPartyName
//        {
//            set{_counterPartyName=value;}
//            get{return _counterPartyName;}
//        }
//        public int CounterPartyID
//        {
//            set { _counterPartyID = value; }
//            get { return _counterPartyID; }
//        }
//        public string Venue
//        {
//            set{_venue=value;}
//            get{return _venue;}
//        }
//        public int VenueID
//        {
//            set { _venueID= value; }
//            get { return _venueID; }
//        }

//        public double CumQty
//        {
//            set{_cumQty=value;}
//            get{return _cumQty;}
//        }
//        public double  Quantity
//        {
//            set{_quantity=value;}
//            get{return _quantity;}
//        }
//        public double AllocatedQty
//        {
//            set{_allocatedQty=value;}
//            get{return _allocatedQty;}
//        }
//        public string TradingAccountName
//        {
//            set{_tradingAccountName=value;}
//            get{return _tradingAccountName;}
//        }
//        public string TradingAccountID
//        {
//            set { _tradingAccountID = value; }
//            get { return _tradingAccountID; }
//        }
//        public double AvgPrice
//        {
//            set{_price=value;}
//            get{return _price;}
//        }
//        public bool Updated
//        {
//            set{_updated=value;}
//            get{return _updated;}
//        }
		
//        public bool AllocatedEqualTotalQty
//        {
//            set{_allocatedEqualTotalQty=value;}
//            get{return _allocatedEqualTotalQty;}
//        }
//        public string  OrderType
//        {
//            set{_orderType=value;}
//            get{return _orderType;}
//        }
//        public string OrderTypeTagValue
//        {
//            set { _orderTypeTagValue = value; }
//            get { return _orderTypeTagValue; }
//        }
		
//        public string  GroupedEntityID
//        {
//            set{_groupedEntityID=value;}
//            get{return _groupedEntityID;}
//        }
		
//        public bool IsProrataActive
//        {
//            set{_isProrataActive=value;}
//            get{return _isProrataActive;}
//        }

//        public int  AUECID
//        {
//            get{return _auecID;}
//            set{_auecID = value;}
//        }
//        public string  ListID
//        {
//            get { return _listID; }
//            set { _listID = value; }
//        }

//    }
//}
