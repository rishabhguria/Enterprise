using System;
using Prana.Interfaces;
using System.Collections.Generic;

namespace Prana.AllocationNew
{
	/// <summary>
	/// Summary description for AllocationPreferences.
	/// </summary>
	/// 
	using System.Xml.Serialization;
	using System.Drawing;
	using System.Collections;
	using System.IO;
	using System.Xml;
    using Prana.BusinessObjects;
using System.Data;

	[XmlRoot("AllocationPreferences")]
	public class AllocationPreferences:IPreferenceData
    {
        #region Private  Variables

        private  AutoGroupingRules _autoGroupingRules;		
		private  GeneralRules _generalRules;
		private  RowProperties _rowProperties;
        private ColumnList _ColumnList;
        private string _unallocatedColumns;
        private string _allocatedColumns;
        private string _unallocatedColumnWidth;
        private string _allocatedColumnWidth;
        private string _splitPanelSize;

        /// <summary>
        /// Allocation Form Height, PRANA-5836
        /// </summary>
        private int _allocationFormHeight;

        /// <summary>
        /// Allocation Form Width, PRANA-5836
        /// </summary>
        private int _allocationFormWidth;

        private AllocationByAccountOrSymbol _allocationByAccountOrSymbol;

        private List<AllocationDefault> _allocationDefaultList = new List<AllocationDefault>();
        //private AccountingMethods _accountingMethods = new AccountingMethods();

        #endregion

        public AllocationPreferences()
		{
			_autoGroupingRules= new AutoGroupingRules();
			_generalRules= new GeneralRules();
			_rowProperties= new RowProperties();
            _ColumnList = new ColumnList();
            _unallocatedColumns = String.Empty;
            _allocatedColumns = String.Empty;
            _allocationByAccountOrSymbol = new AllocationByAccountOrSymbol();
        }

        #region Properties
        /// <summary>
        /// Allocation Form Height Property, PRANA-5836
        /// </summary>
        public int AllocationFormHeight
        {
            get { return _allocationFormHeight; }
            set { _allocationFormHeight = value; }
        }

        /// <summary>
        /// Allocation Form Width property, PRANA-5836
        /// </summary>
        public int AllocationFormWidth
        {
            get { return _allocationFormWidth; }
            set { _allocationFormWidth = value; }
        }

        public AllocationByAccountOrSymbol AllocationByAccountOrSymbol
        {
            get { return _allocationByAccountOrSymbol; }
            set { _allocationByAccountOrSymbol = value; }

        }

        public AutoGroupingRules AutoGroupingRules
		{
			get{return _autoGroupingRules;}
			set{_autoGroupingRules=value;}

		}
		public GeneralRules GeneralRules
		{
			get{return _generalRules;}
			set{_generalRules=value;}

		}
		public RowProperties RowProperties
		{
			get{return _rowProperties;}
			set{_rowProperties=value;}

		}

        public ColumnList ColumnList
        {
            set { _ColumnList = value; }
            get { return _ColumnList; }
        }
        public string UnAllocatedColumns
        {
            get { return _unallocatedColumns; }
            set { _unallocatedColumns = value; }
        }
        public string AllocatedColumns
        {
            get { return _allocatedColumns; }
            set { _allocatedColumns = value; }
        }

        public string UnallocatedColumnWidth
        {
            get { return _unallocatedColumnWidth; }
            set { _unallocatedColumnWidth = value; }
        }
        public string AllocatedColumnWidth
        {
            get { return _allocatedColumnWidth; }
            set { _allocatedColumnWidth = value; }
        }
        public string SplitPanelSize
        {
            get { return _splitPanelSize; }
            set { _splitPanelSize = value; }
        }
        [XmlIgnore]
        public List<AllocationDefault> AllocationDefaultList
        {
            set { _allocationDefaultList = value; }
            get { return _allocationDefaultList; }
        }
        //[XmlIgnore]
        //public AccountingMethods AccountingMethods
        //{
        //    set { _accountingMethods = value; }
        //    get { return _accountingMethods; }
        //}

        #endregion

    }
    public class ColumnList
    {
        private GridColumns _unAllocatedGridColumns;
        private GridColumns _allocatedGridColumns;
        public ColumnList()
         {
             _unAllocatedGridColumns = new GridColumns();
             _allocatedGridColumns = new GridColumns();

         }
        public GridColumns UnAllocatedGridColumns
         {
             get { return _unAllocatedGridColumns; }
             set { _unAllocatedGridColumns = value; }

         }
        public GridColumns AllocatedGridColumns
         {
             get { return _allocatedGridColumns; }
             set { _allocatedGridColumns = value; }

         }

    }
	public class AutoGroupingRules 
	{
    
		/// <remarks/>
		[XmlAttributeAttribute("CounterParty")]
		public bool CounterParty=true;

		[XmlAttributeAttribute("SideAndVenue")]
		public bool Venue=true;
    
		/// <remarks/>
		[XmlAttributeAttribute("BuyAndBCV")]
		public bool BuyAndBCV=true;
    
		/// <remarks/>
		[XmlAttributeAttribute("TradingAccount")]
		public bool TradingAccount=true;
    
                [XmlAttributeAttribute("TradeDate")]
                 public bool TradeDate=true;

                [XmlAttributeAttribute("ProcessDate")]
                 public bool ProcessDate=false;

        [XmlAttributeAttribute("TradeAttributes1")]
        public bool TradeAttributes1 = false;

        [XmlAttributeAttribute("TradeAttributes2")]
        public bool TradeAttributes2 = false;
    
        [XmlAttributeAttribute("TradeAttributes3")]
        public bool TradeAttributes3 = false;

        [XmlAttributeAttribute("TradeAttributes4")]
        public bool TradeAttributes4 = false;

        [XmlAttributeAttribute("TradeAttributes5")]
        public bool TradeAttributes5 = false;

        [XmlAttributeAttribute("TradeAttributes6")]
        public bool TradeAttributes6 = false;

    
		/// <remarks/>
		
		
	}
    public class GeneralRules
    {
        /// <remarks/>
        [XmlAttribute("ApplyRoundLotRules")]
        public bool ApplyRoundLotRules = true;

        /// <remarks/>
        [XmlAttribute("AveragePricingAllowed")]
        public bool AveragePricingAllowed = true;

        /// <remarks/>
		[XmlAttribute("IntegrateAccountAndStrategyBundling")]
		public bool IntegrateAccountAndStrategyBundling=false;

        [XmlAttribute("ClearAllocationAccountControlNumer")]
        public bool ClearAllocationAccountControlNumer = false;

        [XmlAttribute("AllocationMethodologyRevertToAccount")]
        public bool AllocationMethodologyRevertToAccount = false;

        [XmlAttribute("AllocateBasedonOpenPositions")]
        public bool AllocateBasedonOpenPositions = true;

        [XmlAttribute("IsPariPassuAllocation")]
        public bool IsPariPassuAllocation = false;

        [XmlAttribute("AllocateExtraShareToSelectedAccount")]
        public bool AllocateExtraShareToSelectedAccount = false;

        [XmlAttribute("IncludeSavewtState")]
        public bool IncludeSavewtState = false;

        [XmlAttribute("IncludeSavewtoutState")]
        public bool IncludeSavewtoutState = false;

        private int _selectedAccountID = int.MinValue;

        public int SelectedAccountID
        {
            get
            {

                if (AllocateExtraShareToSelectedAccount)
                {
                    return _selectedAccountID;
                }
                else
                {
                   return -1;
                }

            }
            set { _selectedAccountID = value; }
        }



        public bool IsActiveTimer = false;
        public int UpdateInterval = 10;
        //added master fund ratio allocation is enabled or not - omshiv, Jan 2014 
        private bool _isMasterFundRatioAllocation;
        public bool isMasterFundRatioAllocation {
            get {
                return _isMasterFundRatioAllocation;
            }
            set {
                _isMasterFundRatioAllocation = value;
            }
        }
    }

    public class RowProperties 
	{
		
		
		private static int defaultBackColor=Color.FromArgb(192, 192, 255).ToArgb();
		private static int defaultTextColor=Color.Black.ToArgb();
		[XmlAttribute("SelectedRowBackColor")]
		public int SelectedRowBackColor=Color.Gold.ToArgb();
		[XmlAttribute("SelectedRowTextColor")]
		public int SelectedRowTextColor=Color.Black.ToArgb();
		/// <remarks/>
		[XmlAttribute("UnAllocatedExeEqualTotalQtyBackColor")]
		public int  UnAllocatedBackColor= Color.DimGray.ToArgb();
    
		/// <remarks/>
		[XmlAttribute("UnAllocatedExeEqualTotalQtyTextColor")] 
		public int UnAllocatedTextColor=Color.FromArgb(255, 128, 0).ToArgb();
		/// <remarks/>
		[XmlAttribute("ExecutedLessTotalQtyBackColor")]	
		public int ExecutedLessTotalQtyBackColor=Color.Black.ToArgb();
    
		/// <remarks/>
		[XmlAttribute("ExecutedLessTotalQtyTextColor")]	
		public int ExecutedLessTotalQtyTextColor=Color.Gold.ToArgb();
    
		/// <remarks/>
		[XmlAttribute("UnAllocatedWithUpdateBackColor")]
		public int UnAllocatedWithUpdateBackColor=Color.White.ToArgb();
    
		/// <remarks/>
		[XmlAttribute("UnAllocatedWithUpdateTextColor")]	
		public int UnAllocatedWithUpdateTextColor=Color.Red.ToArgb();
    
		/// <remarks/>
		[XmlAttribute("AllocatedEqualTotalQtyBackColor")]	
		public int AllocatedEqualTotalQtyBackColor=Color.Black.ToArgb();
    
		/// <remarks/>
		[XmlAttribute("AllocatedEqualTotalQtyTextColor")]	
		public int AllocatedEqualTotalQtyTextColor=Color.Gold.ToArgb();
    
		/// <remarks/>
		[XmlAttribute("AllocatedWithUpdateBackColor")]	
		public int AllocatedWithUpdateBackColor=Color.White.ToArgb();
    
		/// <remarks/>
		[XmlAttribute()]	
		public int AllocatedWithUpdateTextColor=Color.Red.ToArgb();
    
		/// <remarks/>
		[XmlAttribute("AllocatedLessTotalQtyBackColor")]	
		public int AllocatedLessTotalQtyBackColor=Color.DimGray.ToArgb();
    
		/// <remarks/>
		[XmlAttribute("AllocatedLessTotalQtyTextColor")]	
		public int AllocatedLessTotalQtyTextColor=Color.FromArgb(255, 128, 0).ToArgb();
    
	
	}

    public class AllocationByAccountOrSymbol
    {
        [XmlAttributeAttribute("Account")]
        public bool Account = true;

        [XmlAttributeAttribute("Symbol")]
        public bool Symbol = false;
    }

    public class GridColumns
	{
    
		
        private List<string> columns = new List<string>();
		
		public string SortKey=string.Empty;
    
		public bool Ascending;
		public GridColumns()
		{
			
			SortKey="OrderID";
			Ascending=true;
		}

		public void AddColumn(string columnname )
		{
            columns.Add(columnname);		
		}
		public void ClearColumns()
		{
			columns.Clear();
		
		
		
		}
		
		
        public List<string> DisplayColumns
		{
			get 
			{
                return columns;
			}
            set 
			{
                columns = new List<string>();
                foreach (string column in value)
                {
                    AddColumn(column);
                }
                
			}
		}



	}
   
}
