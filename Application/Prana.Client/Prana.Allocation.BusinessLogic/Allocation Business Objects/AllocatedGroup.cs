using System;

namespace Prana.Allocation.BLL
{
	/// <summary>
	/// Summary description for AllocatedGroup.
	/// </summary>
	public class AllocatedGroup
	{
		#region Private Members
		string _allocatedEntityID;	
		private int  _fundID;
		private string _fundName;
		private int   _strategyID;
		private string  _strategyName;
		private string _clOrderID=string.Empty ;
		private string _orderType= string.Empty;
		private string _orderSide;
		private string _symbol;
		private string _counterPartyName;
		private string _venue;		
		private string _tradingAccountName=string.Empty;
		private Int64 _allocatedQty;
		private double _avgPrice;		
		private Int64  _cumQty;
		private Int64 _quantity;		
		private int _assetID					= Int32.MinValue;
		private string _assetName				= string.Empty;
		private int _underlyingID				= Int32.MinValue ;
		private string _underlyingName			= string.Empty;
		private int _exchangeID					= Int32.MinValue ;
		private string _exchangeName			= string.Empty;
		private int _currencyID					= Int32.MinValue ;
		private string _currencyName			= string.Empty;
		private int _auecID						= Int32.MinValue ;
        private float _commission;
        private float _fees;

		#endregion
        
        private string _orderSideTagValue;
        private string _orderTypeTagValue = string.Empty;
        private int _counterPartyID = int.MinValue;
        
        
        private int _venueID = int.MinValue;

        private int _tradingAccountID = int.MinValue;

		public AllocatedGroup()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region Properties
		public string ClOrderID
		{
			set{_clOrderID =value;}
			get{return _clOrderID;}
		}
		
		public string Symbol
		{
			set{_symbol=value;}
			get{return _symbol;}
		}
		
		
		
		public int  FundID
		{
			set{_fundID =value;}
			get{return _fundID;}
			
		}
		public int StrategyID
		{
			set{_strategyID =value;}
			get{return _strategyID;}
			
		}
		public string AllocationFund
		{
			set{_fundName =value;}
			get{return _fundName;}
			
		}
		public string Strategy
		{
			set{_strategyName =value;}
			get{return _strategyName;}
			
		}
        public string OrderSide
		{
			set{_orderSide=value;}
			get{return _orderSide;}
		}
		public string CounterPartyName
		{
			set{_counterPartyName=value;}
			get{return _counterPartyName;}
		}
		public string Venue
		{
			set{_venue=value;}
			get{return _venue;}
		}
	
		public Int64 CumQty
		{
			set{_cumQty=value;}
			get{return _cumQty;}
		}
		public Int64 Quantity
		{
			set{_quantity=value;}
			get{return _quantity;}
		}
		public Int64 AllocatedQty
		{
			set{_allocatedQty=value;}
			get{return _allocatedQty;}
		}
		public string TradingAccountName
		{
			set{_tradingAccountName=value;}
			get{return _tradingAccountName;}
		}
		public double AvgPrice
		{
			set{_avgPrice=value;}
			get{return _avgPrice;}
		}
		
		
		
		public string  OrderType
		{
			set{_orderType=value;}
			get{return _orderType;}
		}

        public string OrderTypeTagValue
        {
            set { _orderTypeTagValue = value; }
            get { return _orderTypeTagValue; }
        }
		public string  AllocationID
		{
			set{_allocatedEntityID=value;}
			get{return _allocatedEntityID;}
		}
		
		

		public int  AUECID
		{
			get{return _auecID;}
			set{_auecID = value;}
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

		public string  Currency
		{
			get{return _currencyName;}
			set{_currencyName = value;}
		}

		#endregion
        public int  TradingAccountID
        {
            set { _tradingAccountID = value; }
            get { return _tradingAccountID; }
        }

        public int CounterPartyID
        {
            set { _counterPartyID = value; }
            get { return _counterPartyID; }
        }
        public string SideTagValue
        {
            set { _orderSideTagValue = value; }
            get { return _orderSideTagValue; }
        }
        public int VenueID
        {
            set { _venueID = value; }
            get { return _venueID; }
        }
        public float Commission
        {
            set { _commission = value; }
            get { return _commission; }
        }
        public float Fees
        {
            set { _fees = value; }
            get { return _fees; }
        }
	}
}
