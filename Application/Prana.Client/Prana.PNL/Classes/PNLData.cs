using System;
using System.Diagnostics;

namespace Nirvana.PNL
{
	/// <summary>
	/// Summary description for RowColumns.
	/// </summary>
	public class PNLData
	{
		public PNLData()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private bool _isUpdated = true;

		public bool IsUpdated
		{
			get
			{
				return this._isUpdated;
			}

			set
			{
				this._isUpdated = value;
			}
		}

		private string _symbol = string.Empty;

		public string Symbol
		{
			get
			{
				return this._symbol;
			}

			set
			{
				this._symbol = value;
			}
		}

        private double _last = 0; //float.MinValue;

		public double Last
		{
			get
			{
				return this._last;
			}

			set
			{
				this._last = value;
			}
		}

		private double _bid = 0; //float.MinValue;

		public double Bid
		{
			get
			{
				return this._bid;
			}

			set
			{
				this._bid = value;
			}
		}

		private double _ask = 0; //float.MinValue;

		public double Ask
		{
			get
			{
				return this._ask;
			}

			set
			{
				this._ask = value;
			}
		}

		private int _longExposure = 0; //int.MinValue;

		public int LongExposure
		{
			get
			{
				return this._longExposure;
			}

			set
			{
				this._longExposure = value;
                //Debug.WriteLine("LongExposure : " + value);                
			}
		}

		private int _shortExposure = 0; //int.MinValue;

		public int ShortExposure
		{
			get
			{
				return this._shortExposure;
			}

			set
			{
				this._shortExposure = value;
			}
		}

		private int _netExposure = 0; //int.MinValue;

		public int NetExposure
		{
			get
			{
				return this._netExposure;
			}

			set
			{
				this._netExposure = value;
			}
		}

		private int _longPNL = 0; //int.MinValue;

		public int LongPNL
		{
			get
			{
				return this._longPNL;
			}

			set
			{
				this._longPNL = value;
			}
		}

		private int _shortPNL = 0; //int.MinValue;

		public int ShortPNL
		{
			get
			{
				return this._shortPNL;
			}

			set
			{
				this._shortPNL = value;
			}
		}

		private int _netPNL = 0; //int.MinValue;

		public int NetPNL
		{
			get
			{
				return this._netPNL;
			}

			set
			{
				this._netPNL = value;
			}
		}

		private string _currency = string.Empty;

		public string Currency
		{
			get
			{
				return this._currency;
			}

			set
			{
				this._currency = value;
			}
		}

		private string _exchange = string.Empty;

		public string Exchange
		{
			get
			{
				return this._exchange;
			}

			set
			{
				this._exchange = value;
			}
		}

		private double _percentChange = 0; // float.MinValue;

		public double PercentChange
		{
			get
			{
				return this._percentChange;
			}

			set
			{
				this._percentChange = value;
			}
		}

		private int _executedQuantity = 0; //int.MinValue;

		public int ExecutedQuantity
		{
			get
			{
				return this._executedQuantity;
			}

			set
			{
				this._executedQuantity = value;
			}
		}

        private double _averagePrice = 0; //float.MinValue;

		public double AveragePrice
		{
			get
			{
				return this._averagePrice;
			}

			set
			{
				this._averagePrice = value;
			}
		}

		private string _client = string.Empty;

		public string Client
		{
			get
			{
				return this._client;
			}

			set
			{
				this._client = value;
			}
		}

		private string _tradingAccount = string.Empty;

		public string TradingAccount
		{
			get
			{
				return this._tradingAccount;
			}

			set
			{
				this._tradingAccount = value;
			}
		}

		private string _side = string.Empty;

		public string Side
		{
			get
			{
				return this._side;
			}

			set
			{
				this._side = value;
			}
		}

		private string _asset = string.Empty;

		public string Asset
		{
			get
			{
				return this._asset;
			}

			set
			{
				this._asset = value;
			}
		}

		private string _user = string.Empty;

		public string User
		{
			get
			{
				return this._user;
			}

			set
			{
				this._user = value;
			}
		}

		private string _underLying = string.Empty;

		public string UnderLying
		{
			get
			{
				return this._underLying;
			}

			set
			{
				this._underLying = value;
			}
		}
		
	}
}
