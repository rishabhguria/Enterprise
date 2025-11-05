using System;
using System.Drawing;
using System.Collections;
using Nirvana.Global;
using System.Xml.Serialization;
using Nirvana.Interfaces;

namespace Nirvana.PNL
{

	#region Prefrences Enum
	
	public enum PNLCalcuationColumn
	{
		Ask = 0,
		Bid = 1,
		Last = 2,
		Previous = 3
	}

	
	public enum PNLTab
	{
		Asset = 0,
		Symbol = 1,
		TradingAccount =2
	}
	
	
	public enum PNLColumns
	{
		Symbol=0,
		Last=1,
		Bid=2,
		Ask=3,
		LongExposure=4,
		ShortExposure=5,
		NetExposure=6,
		LongPNL=7,
		ShortPNL=8,
		NetPNL=9,
		Currency=10,
		Exchange=11,
		PercentChange=12,
		ExecutedQuantity=13,
		AveragePrice=14,
		Client=15,
		TradingAccount=16,
		Side=17,
		Asset=18,
		User=19,
		UnderLying=20
	}

	
	public enum PNLGroupColumns
	{
		Symbol=0,
		Exchange=11,
		//Client=15,
		TradingAccount=16,
		Side=17,
		Asset=18
		//User=19
	}
	#endregion

	/// <summary>
	/// Summary description for PNLPrefrencesData.
	/// </summary>
	[XmlRoot("PNLPrefrences")]
	public class PNLPrefrencesData: IPreferenceData
	{
		#region Preferences Column information

		[XmlIgnoreAttribute()]
		public static string[] PNLColumnTypes = new string[]
									{
										"System.String",
										"System.Double",
										"System.Double",
										"System.Double",
										"System.Int32",
										"System.Int32",
										"System.Int32",
										"System.Double",
										"System.Double",
										"System.Double",
										"System.String",
										"System.String",
										"System.Double",				
										"System.Int32",
										"System.Double",
										"System.String",
										"System.String",
										"System.String",
										"System.String",
										"System.String",
										"System.String"
									};
		[XmlIgnoreAttribute()]
		public static string[] PNLColumnNames = new string[]
									{
										"Symbol",
										"Last",
										"Bid",
										"Ask",
										"LongExposure",
										"ShortExposure",
										"NetExposure",
										"LongPNL",
										"ShortPNL",
										"NetPNL",
										"Currency",
										"Exchange",
										"PercentChange",
										"ExecutedQuantity",				
										"AveragePrice",
										"Client",
										"TradingAccount",
										"Side",
										"Asset",
										"User",
										"Underlying"
									};

		[XmlIgnoreAttribute()]
		public static string[] PNLCalcuationColumnNames = new string[]
								{
									"Ask",
									"Bid",
									"Last",
									"Previous Close"
								};
		#endregion

		#region PNL Calculation preferences properties

		private int _longExposoreCalculcationColumn;
		private int _shortExposureCalculationColumn;
		private int _longPNLCalculationColumn;
		private int _shortPNLCalculcationColumn;
		private int _defaultCalculcationColumn;

        
		public int LongExposoreCalculcationColumn
		{
			get
			{
				return this._longExposoreCalculcationColumn;
			}
			set
			{
				this._longExposoreCalculcationColumn = value;
			}
		}

		public int ShortExposureCalculationColumn
		{
			get
			{
				return this._shortExposureCalculationColumn;
			}
			set
			{
				this._shortExposureCalculationColumn = value;
			}
		}

		public int LongPNLCalculationColumn
		{
			get
			{
				return this._longPNLCalculationColumn;
			}
			set
			{
				this._longPNLCalculationColumn = value;
			}
		}

		public int ShortPNLCalculcationColumn
		{
			get
			{
				return this._shortPNLCalculcationColumn;
			}
			set
			{
				this._shortPNLCalculcationColumn = value;
			}
		}
		
		public int DefaultCalculcationColumn
		{
			get
			{
				return this._defaultCalculcationColumn;
			}
			set
			{
				this._defaultCalculcationColumn = value;
			}
		}

		#endregion

		#region PNL column preferences properties
			
		int			_sortKeyAssetL1;
		int			_sortKeyAssetL2;
		int			_sortKeySymbolL1;
		int			_sortKeySymbolL2;
		int			_sortKeyTradingAccountL1;
		int			_sortKeyTradingAccountL2;
		bool		_ascendingAssetL1;	
		bool		_ascendingAssetL2;
		bool		_ascendingSymbolL1;	
		bool		_ascendingSymbolL2;		
		bool		_ascendingTradingAccountL1;	
		bool		_ascendingTradingAccountL2;

		ArrayList	_displayListAssetL1	= new ArrayList();
		ArrayList	_displayListAssetL2	= new ArrayList();
		ArrayList	_displayListSymbolL1	= new ArrayList();
		ArrayList	_displayListSymbolL2	= new ArrayList();
		ArrayList	_displayListTradingAccountL1	= new ArrayList();
		ArrayList	_displayListTradingAccountL2	= new ArrayList();
		
		ArrayList	_availableColumnForSymbol	= new ArrayList();
		ArrayList	_availableColumnForExchange	= new ArrayList();
		ArrayList	_availableColumnForClient	= new ArrayList();
		ArrayList	_availableColumnForTradingAccount	= new ArrayList();
		ArrayList	_availableColumnForAsset	= new ArrayList();
		ArrayList	_availableColumnForUser	= new ArrayList();
		ArrayList	_availableColumnForSide	= new ArrayList();

		
		int _level1AssetColumn;
		int _level1SymbolColumn;
		int _level1TradingAccountColumn;

		int _level2AssetColumn;
		int _level2SymbolColumn;
		int _level2TradingAccountColumn;
		int _refreshRate;
		
		#region Set/Get Properties

		[XmlArray ("DisplayListAssetL1"), XmlArrayItem("Column", typeof(PNLColumns))]
		public ArrayList DisplayListAssetL1
		{
			get
			{
				return _displayListAssetL1;
			}

			set
			{
				_displayListAssetL1 = value;
			}
		}

		[XmlArray ("DisplayListAssetL2"), XmlArrayItem("Column", typeof(PNLColumns))]
		public ArrayList DisplayListAssetL2
		{
			get
			{
				return _displayListAssetL2;
			}

			set
			{
				_displayListAssetL2 = value;
			}
		}

		[XmlArray ("DisplayListSymbolL1"), XmlArrayItem("Column", typeof(PNLColumns))]
		public ArrayList DisplayListSymbolL1
		{
			get
			{
				return _displayListSymbolL1;
			}

			set
			{
				_displayListSymbolL1 = value;
			}
		}

		[XmlArray ("DisplayListSymbolL2"), XmlArrayItem("Column", typeof(PNLColumns))]
		public ArrayList DisplayListSymbolL2
		{
			get
			{
				return _displayListSymbolL2;
			}
			set
			{
				_displayListSymbolL2 = value;
			}
		}

		[XmlArray ("DisplayListTradingAccountL1"), XmlArrayItem("Column", typeof(PNLColumns))]
		public ArrayList DisplayListTradingAccountL1
		{
			get
			{
				return _displayListTradingAccountL1;
			}
			set
			{
				_displayListTradingAccountL1 = value;
			}
		}

		[XmlArray ("DisplayListTradingAccountL2"), XmlArrayItem("Column", typeof(PNLColumns))]
		public ArrayList DisplayListTradingAccountL2
		{
			get
			{
				return _displayListTradingAccountL2;
			}
			set
			{
				_displayListTradingAccountL2 = value;
			}
		}
		
		public int SortKeyAssetL1
		{
			get
			{
				return _sortKeyAssetL1;
			}

			set
			{
				_sortKeyAssetL1 = value;
			}
		}

		public int SortKeyAssetL2
		{
			get
			{
				return _sortKeyAssetL2;
			}

			set
			{
				_sortKeyAssetL2 = value;
			}
		}

		public int SortKeySymbolL1
		{
			get
			{
				return _sortKeySymbolL1;
			}

			set
			{
				_sortKeySymbolL1 = value;
			}
		}

		public int SortKeySymbolL2
		{
			get
			{
				return _sortKeySymbolL2;
			}

			set
			{
				_sortKeySymbolL2 = value;
			}
		}

		public int SortKeyTradingAccountL1
		{
			get
			{
				return _sortKeyTradingAccountL1;
			}

			set
			{
				_sortKeyTradingAccountL1 = value;
			}
		}

		public int SortKeyTradingAccountL2
		{
			get
			{
				return _sortKeyTradingAccountL2;
			}

			set
			{
				_sortKeyTradingAccountL2 = value;
			}
		}

		public bool AscendingAssetL1
		{
			get
			{
				return _ascendingAssetL1;
			}

			set
			{
				_ascendingAssetL1 = value;
			}
		}	

		public bool AscendingAssetL2
		{
			get
			{
				return _ascendingAssetL2;
			}

			set
			{
				_ascendingAssetL2 = value;
			}
		}

		public bool AscendingSymbolL1
		{
			get
			{
				return _ascendingSymbolL1;
			}

			set
			{
				_ascendingSymbolL1 = value;
			}
		}	

		public bool AscendingSymbolL2
		{
			get
			{
				return _ascendingSymbolL2;
			}
			set
			{
				_ascendingSymbolL2 = value;
			}
		}

		public bool AscendingTradingAccountL1
		{
			get
			{
				return _ascendingTradingAccountL1;
			}
			set
			{
				_ascendingTradingAccountL1 = value;
			}
		}	

		public bool AscendingTradingAccountL2
		{
			get
			{
				return _ascendingTradingAccountL2;
			}
			set
			{
				_ascendingTradingAccountL2 = value;
			}
		}

		public int Level2AssetColumn
		{
			get
			{
				return this._level2AssetColumn;
			}

			set
			{
				this._level2AssetColumn = value;
			}
		}

		public int Level2SymbolColumn
		{
			get
			{
				return this._level2SymbolColumn;
			}

			set
			{
				this._level2SymbolColumn = value;
			}
		}

		public int Level2TradingAccountColumn
		{
			get
			{
				return this._level2TradingAccountColumn;
			}

			set
			{
				this._level2TradingAccountColumn = value;
			}
		}
		
		public int Level1AssetColumn
		{
			get
			{
				return this._level1AssetColumn;
			}

			set
			{
				this._level1AssetColumn = value;
			}
		}

		public int Level1SymbolColumn
		{
			get
			{
				return this._level1SymbolColumn;
			}

			set
			{
				this._level1SymbolColumn = value;
			}
		}

		public int Level1TradingAccountColumn
		{
			get
			{
				return this._level1TradingAccountColumn;
			}

			set
			{
				this._level1TradingAccountColumn = value;
			}
		}

		public int RefreshRate
		{
			get
			{
				return this._refreshRate;
			}

			set
			{
				this._refreshRate = value;
			}
		}

		[XmlIgnoreAttribute()]
		public ArrayList AvailableColumnForSymbol
		{
			get
			{
				return this._availableColumnForSymbol;
			}

			set
			{
				this._availableColumnForSymbol = value;
			}
		}
	 
		[XmlIgnoreAttribute()]
		public ArrayList AvailableColumnForExchange
		{
			get
			{
				return this._availableColumnForExchange;
			}

			set
			{
				this._availableColumnForExchange = value;
			}
		}

		[XmlIgnoreAttribute()]
		public ArrayList AvailableColumnForClient
		{
			get
			{
				return this._availableColumnForClient;
			}

			set
			{
				this._availableColumnForClient = value;
			}
		}

		[XmlIgnoreAttribute()]
		public ArrayList AvailableColumnForTradingAccount
		{
			get
			{
				return this._availableColumnForTradingAccount;
			}

			set
			{
				this._availableColumnForTradingAccount = value;
			}
		}

		[XmlIgnoreAttribute()]
		public ArrayList AvailableColumnForAsset
		{
			get
			{
				return this._availableColumnForAsset;
			}

			set
			{
				this._availableColumnForAsset = value;
			}
		}

		[XmlIgnoreAttribute()]
		public ArrayList AvailableColumnForUser
		{
			get
			{
				return this._availableColumnForUser;
			}

			set
			{
				this._availableColumnForUser = value;
			}
		}

		[XmlIgnoreAttribute()]
		public ArrayList AvailableColumnForSide
		{
			get
			{
				return this._availableColumnForSide;
			}

			set
			{
				this._availableColumnForSide = value;
			}
		}

		#endregion

		#endregion

		#region PNL color preferences properties
		string _rowColorL1;
		string _rowalternateColorL1;
		string _rowColorL2;
		string _rowalternateColorL2;
		
		string _headerColorL2;
		string _headerColorL1;

		string _longExposureTextColor;
		string _shortExposureTextColor;
		string _netExposureTextColor;
		string _longPNLTextColor;
		string _shortPNLTextColor;
		string _netPNLTextColor;

		string _positiveValueColor;
		string _negativeValueColor;
		
		string _backgroundColor;
		string _defaulTextColor;

		string _summaryBackgroundColor;
		string _summaryTextColor;

		public string RowColorL1
		{
			get
			{
				return _rowColorL1;
			}
			set
			{
				_rowColorL1 = value;
			}
		}

		public string RowalternateColorL1
		{
			get
			{
				return _rowalternateColorL1;
			}
			set
			{
				_rowalternateColorL1 = value;
			}
		}

		public string RowColorL2
		{
			get
			{
				return _rowColorL2;
			}
			set
			{
				_rowColorL2 = value;
			}
		}

		public string RowalternateColorL2
		{
			get
			{
				return _rowalternateColorL2;
			}
			set
			{
				_rowalternateColorL2 = value;
			}
		}

		public string LongExposureTextColor
		{
			get
			{
				return _longExposureTextColor;
			}
			set
			{
				_longExposureTextColor = value;
			}
		}

		public string ShortExposureTextColor
		{
			get
			{
				return _shortExposureTextColor;
			}
			set
			{
				_shortExposureTextColor = value;
			}
		}

		public string NetExposureTextColor
		{
			get
			{
				return _netExposureTextColor;
			}
			set
			{
				_netExposureTextColor = value;
			}
		}

		public string HeaderColorL2
		{
			get
			{
				return _headerColorL2;
			}
			set
			{
				_headerColorL2 = value;
			}
		}

		public string HeaderColorL1
		{
			get
			{
				return _headerColorL1;
			}
			set
			{
				_headerColorL1 = value;
			}
		}

		public string LongPNLTextColor
		{
			get
			{
				return _longPNLTextColor;
			}
			set
			{
				_longPNLTextColor = value;
			}
		}

		public string ShortPNLTextColor
		{
			get
			{
				return _shortPNLTextColor;
			}
			set
			{
				_shortPNLTextColor = value;
			}
		}

		public string NetPNLTextColor
		{
			get
			{
				return _netPNLTextColor;
			}
			set
			{
				_netPNLTextColor = value;
			}
		}

		public string PositiveValueColor
		{
			get
			{
				return this._positiveValueColor;
			}
			set
			{
				_positiveValueColor = value;
			}
		}

		public string NegativeValueColor
		{
			get
			{
				return this._negativeValueColor;
			}
			set
			{
				_negativeValueColor = value;
			}
		}

		public string BackgroundColor
		{
			get
			{
				return _backgroundColor;
			}
			set
			{
				_backgroundColor = value;
			}
		}

		public string DefaulTextColor
		{
			get
			{
				return _defaulTextColor;
			}
			set
			{
				_defaulTextColor = value;
			}
		}

		public string SummaryBackgroundColor
		{
			get
			{
				return _summaryBackgroundColor;
			}
			set
			{
				_summaryBackgroundColor = value;
			}
		}

		public string SummaryTextColor
		{
			get
			{
				return _summaryTextColor;
			}
			set
			{
				_summaryTextColor = value;
			}
		}


		#endregion

		#region PNL Chart color preferences properties

		string _chartBackgroundColor;
		string _chartBorderColor;
		string _chartTextColor;
		
		bool _showLegend;
		string _legendBackgroundColor;
		string _legendBorderColor;
		string _legendTextColor;

		public string ChartBackgroundColor
		{
			get
			{
				return _chartBackgroundColor;
			}
			set
			{
				_chartBackgroundColor = value;
			}
		}


		public string ChartBorderColor
		{
			get
			{
				return _chartBorderColor;
			}
			set
			{
				_chartBorderColor = value;
			}
		}


		public string ChartTextColor
		{
			get
			{
				return _chartTextColor;
			}
			set
			{
				_chartTextColor = value;
			}
		}

		public string LegendBackgroundColor
		{
			get
			{
				return _legendBackgroundColor;
			}
			set
			{
				_legendBackgroundColor = value;
			}
		}

		public string LegendBorderColor
		{
			get
			{
				return _legendBorderColor;
			}
			set
			{
				_legendBorderColor = value;
			}
		}

		public string LegendTextColor
		{
			get
			{
				return _legendTextColor;
			}
			set
			{
				_legendTextColor = value;
			}
		}

		public bool ShowLegend
		{
			get
			{
				return _showLegend;
			}
			set
			{
				_showLegend = value;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor to initialize different variables
		/// </summary>
		public PNLPrefrencesData()
		{
			_availableColumnForSymbol.Add(PNLColumns.Last);
			_availableColumnForSymbol.Add(PNLColumns.Bid);
			_availableColumnForSymbol.Add(PNLColumns.Ask);
			_availableColumnForSymbol.Add(PNLColumns.ExecutedQuantity);
			_availableColumnForSymbol.Add(PNLColumns.PercentChange);
			_availableColumnForSymbol.Add(PNLColumns.LongExposure);
			_availableColumnForSymbol.Add(PNLColumns.ShortExposure);
			_availableColumnForSymbol.Add(PNLColumns.NetExposure);
			_availableColumnForSymbol.Add(PNLColumns.LongPNL);
			_availableColumnForSymbol.Add(PNLColumns.ShortPNL);
			_availableColumnForSymbol.Add(PNLColumns.NetPNL);

			_availableColumnForExchange.Add(PNLColumns.Currency);
			_availableColumnForExchange.Add(PNLColumns.ExecutedQuantity);
			_availableColumnForExchange.Add(PNLColumns.LongExposure);
			_availableColumnForExchange.Add(PNLColumns.ShortExposure);
			_availableColumnForExchange.Add(PNLColumns.NetExposure);
			_availableColumnForExchange.Add(PNLColumns.LongPNL);
			_availableColumnForExchange.Add(PNLColumns.ShortPNL);
			_availableColumnForExchange.Add(PNLColumns.NetPNL);

			_availableColumnForClient.Add(PNLColumns.ExecutedQuantity);
			_availableColumnForClient.Add(PNLColumns.LongExposure);
			_availableColumnForClient.Add(PNLColumns.ShortExposure);
			_availableColumnForClient.Add(PNLColumns.NetExposure);
			_availableColumnForClient.Add(PNLColumns.LongPNL);
			_availableColumnForClient.Add(PNLColumns.ShortPNL);
			_availableColumnForClient.Add(PNLColumns.NetPNL);

			_availableColumnForTradingAccount.Add(PNLColumns.ExecutedQuantity);
			_availableColumnForTradingAccount.Add(PNLColumns.LongExposure);
			_availableColumnForTradingAccount.Add(PNLColumns.ShortExposure);
			_availableColumnForTradingAccount.Add(PNLColumns.NetExposure);
			_availableColumnForTradingAccount.Add(PNLColumns.LongPNL);
			_availableColumnForTradingAccount.Add(PNLColumns.ShortPNL);
			_availableColumnForTradingAccount.Add(PNLColumns.NetPNL);

			_availableColumnForAsset.Add(PNLColumns.ExecutedQuantity);
			_availableColumnForAsset.Add(PNLColumns.UnderLying);
			_availableColumnForAsset.Add(PNLColumns.LongExposure);
			_availableColumnForAsset.Add(PNLColumns.ShortExposure);
			_availableColumnForAsset.Add(PNLColumns.NetExposure);
			_availableColumnForAsset.Add(PNLColumns.LongPNL);
			_availableColumnForAsset.Add(PNLColumns.ShortPNL);
			_availableColumnForAsset.Add(PNLColumns.NetPNL);
			
			_availableColumnForUser.Add(PNLColumns.ExecutedQuantity);
			_availableColumnForUser.Add(PNLColumns.LongExposure);
			_availableColumnForUser.Add(PNLColumns.ShortExposure);
			_availableColumnForUser.Add(PNLColumns.NetExposure);
			_availableColumnForUser.Add(PNLColumns.LongPNL);
			_availableColumnForUser.Add(PNLColumns.ShortPNL);
			_availableColumnForUser.Add(PNLColumns.NetPNL);

			_availableColumnForSide.Add(PNLColumns.ExecutedQuantity);
			_availableColumnForSide.Add(PNLColumns.AveragePrice);
			_availableColumnForSide.Add(PNLColumns.LongExposure);
			_availableColumnForSide.Add(PNLColumns.ShortExposure);
			_availableColumnForSide.Add(PNLColumns.NetExposure);
			_availableColumnForSide.Add(PNLColumns.LongPNL);
			_availableColumnForSide.Add(PNLColumns.ShortPNL);
			_availableColumnForSide.Add(PNLColumns.NetPNL);

		}
		#endregion

		#region General methods

		/// <summary>
		/// This method sets the default preferences into preferences object
		/// </summary>
		public void SetDefaultPreferences()
		{
			//set default colors
			_rowColorL1 =  PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(255,153,0));
			_rowalternateColorL1 = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(249,179,75));
			_rowColorL2 =  PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(255,204,0));
			_rowalternateColorL2 = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(254,225,111));
			
			_headerColorL2 = PNLPrefrencesData.GetARGBFromColor(Color.Gold);
			_headerColorL1 = PNLPrefrencesData.GetARGBFromColor(Color.Gold);
			
			_longExposureTextColor = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(252,0,30));
			_shortExposureTextColor = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(24,70,241));
			//_netExposureTextColor = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb (0,128,0));
			//_longPNLTextColor = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(159,112,11));
			//_shortPNLTextColor = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(0,128,0));
			//_netPNLTextColor = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(159,112,11));

			_negativeValueColor = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(255,0,0));
			_positiveValueColor = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(0,255,0));
		
			_backgroundColor = PNLPrefrencesData.GetARGBFromColor(Color.Black); //Color.FromArgb(236,233,216);
			_defaulTextColor = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(0,0,0));

			//set the default chart preferences
			_chartBackgroundColor = PNLPrefrencesData.GetARGBFromColor(Color.Black);
			_chartBorderColor = PNLPrefrencesData.GetARGBFromColor(Color.Gold);
			_chartTextColor = PNLPrefrencesData.GetARGBFromColor(Color.White);
		
			_showLegend = true;
			_legendBackgroundColor = PNLPrefrencesData.GetARGBFromColor(Color.Gold);
			_legendBorderColor = PNLPrefrencesData.GetARGBFromColor(Color.Yellow);
			_legendTextColor = PNLPrefrencesData.GetARGBFromColor(Color.White);

			_summaryBackgroundColor = PNLPrefrencesData.GetARGBFromColor(Color.FromArgb(24, 73, 171));
			_summaryTextColor = PNLPrefrencesData.GetARGBFromColor(Color.White);

			_refreshRate = 5; //in sec

			//group by column
			_level1AssetColumn = 18; 
			_level1SymbolColumn = 0;  
			_level1TradingAccountColumn = 16; 

			_level2AssetColumn = 11;
			_level2SymbolColumn = 17;  
			_level2TradingAccountColumn = 0; 

			_sortKeyAssetL1 = 18;
			_sortKeyAssetL2 = 11;
			_sortKeySymbolL1 = 0;
			_sortKeySymbolL2 = 17;
			_sortKeyTradingAccountL1 = 16;
			_sortKeyTradingAccountL2 = 0;
			_ascendingAssetL1 = true;	
			_ascendingAssetL2 = false;
			_ascendingSymbolL1 = false;	
			_ascendingSymbolL2 = true;		
			_ascendingTradingAccountL1 = false;	
			_ascendingTradingAccountL2 = false;

			PNLColumns pnlColumns = new PNLColumns();
						
			//set default columns for Asset tab L1
			_displayListAssetL1.Clear();
			_displayListAssetL1.Add(PNLColumns.UnderLying);
			_displayListAssetL1.Add(PNLColumns.LongExposure);
			_displayListAssetL1.Add(PNLColumns.ShortExposure);
			_displayListAssetL1.Add(PNLColumns.NetExposure);
			_displayListAssetL1.Add(PNLColumns.LongPNL);
			_displayListAssetL1.Add(PNLColumns.ShortPNL);
			_displayListAssetL1.Add(PNLColumns.NetPNL);
			
			//set default columns for Asset tab L2
			_displayListAssetL2.Clear();
			_displayListAssetL2.Add(PNLColumns.Currency);
			_displayListAssetL2.Add(PNLColumns.LongExposure);
			_displayListAssetL2.Add(PNLColumns.ShortExposure);
			_displayListAssetL2.Add(PNLColumns.LongPNL);
			_displayListAssetL2.Add(PNLColumns.ShortPNL);
			_displayListAssetL2.Add(PNLColumns.NetPNL);
			
			//set default columns for Symbol tab grid L1
			_displayListSymbolL1.Clear();
			_displayListSymbolL1.Add(PNLColumns.ExecutedQuantity);
			_displayListSymbolL1.Add(PNLColumns.LongExposure);
			_displayListSymbolL1.Add(PNLColumns.ShortExposure);
			_displayListSymbolL1.Add(PNLColumns.NetExposure);
			_displayListSymbolL1.Add(PNLColumns.LongPNL);
			_displayListSymbolL1.Add(PNLColumns.ShortPNL);
			_displayListSymbolL1.Add(PNLColumns.NetPNL);
			_displayListSymbolL1.Add(PNLColumns.Last);
			_displayListSymbolL1.Add(PNLColumns.Bid);
			_displayListSymbolL1.Add(PNLColumns.Ask);
			_displayListSymbolL1.Add(PNLColumns.PercentChange);
			
			//set default columns for Symbol tab grid L2
			_displayListSymbolL2.Clear();
			_displayListSymbolL2.Add(PNLColumns.ExecutedQuantity);
			_displayListSymbolL2.Add(PNLColumns.AveragePrice);
			_displayListSymbolL2.Add(PNLColumns.NetExposure);
			_displayListSymbolL2.Add(PNLColumns.NetPNL);
			
			//set default columns for trading account tab grid L1
			_displayListTradingAccountL1.Clear();
			_displayListTradingAccountL1.Add(PNLColumns.LongExposure);
			_displayListTradingAccountL1.Add(PNLColumns.ShortExposure);
			_displayListTradingAccountL1.Add(PNLColumns.NetExposure);
			_displayListTradingAccountL1.Add(PNLColumns.NetPNL);
			
			//set default columns for trading account tab grid L2
			_displayListTradingAccountL2.Clear();
			_displayListTradingAccountL2.Add(PNLColumns.ExecutedQuantity);
			_displayListTradingAccountL2.Add(PNLColumns.NetExposure);
			_displayListTradingAccountL2.Add(PNLColumns.NetPNL);
			_displayListTradingAccountL2.Add(PNLColumns.PercentChange);
			_displayListTradingAccountL2.Add(PNLColumns.Last);
			_displayListTradingAccountL2.Add(PNLColumns.Bid);
			_displayListTradingAccountL2.Add(PNLColumns.Ask);
			
			_longExposoreCalculcationColumn = 0;
			_shortExposureCalculationColumn = 1;
			_longPNLCalculationColumn = 2;
			_shortPNLCalculcationColumn = 3;
			_defaultCalculcationColumn = 2;

		}


		/// <summary>
		/// To get the column Index
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns></returns>
		public static int GetColumnIndex(string columnName)
		{
			int index = 0;
			for(int i=0;i<PNLPrefrencesData.PNLColumnNames.Length;i++)
			{
				if(PNLPrefrencesData.PNLColumnNames[i].Equals(columnName))
				{
					index = i;
					break;
				}
			}

			return index;
		}

		#endregion


		#region Convert Color object from 32 bit ARGB values and vice versa

		/// <summary>
		/// It receives the comma separated string as "R,G,B" and returns the Color object corresponding to that
		/// </summary>
		/// <param name="RGBValue"></param>
		/// <returns></returns>
		public static Color GetColorFromARGB(string ARGBValue)
		{
			Color computedColor = Color.Transparent;
			computedColor = Color.FromArgb(Convert.ToInt32(ARGBValue));
			return computedColor;

		}

		/// <summary>
		/// It receives the Color object and returns the comma separated string as "R,G,B" corresponding to that
		/// </summary>
		/// <param name="RGBValue"></param>
		/// <returns></returns>
		public static string GetARGBFromColor(Color color)
		{
			return color.ToArgb().ToString();

		}

		#endregion  Convert Color object from 32 bit ARGB values and vice versa
	}
}
