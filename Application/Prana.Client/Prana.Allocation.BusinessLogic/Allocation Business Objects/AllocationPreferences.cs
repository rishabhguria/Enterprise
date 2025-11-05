using System;
using Prana.Interfaces;

namespace Prana.Allocation.BLL
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

	[XmlRoot("AllocationPreferences")]
	public class AllocationPreferences:IPreferenceData
    {
        #region Private  Variables
        private  AutoGroupingRules _autoGroupingRules;		
		private  GeneralRules _generalRules;
		private  RowProperties _rowProperties;
        private AllocationType _strategyType;
        private AllocationType _fundType;
        
        #endregion
        public AllocationPreferences()
		{
			_autoGroupingRules= new AutoGroupingRules();
			_generalRules= new GeneralRules();
			_rowProperties= new RowProperties();
            _fundType = new AllocationType();
            _strategyType = new AllocationType();
			
        }
        #region Properties
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
        public AllocationType StrategyType
        {
            set { _strategyType = value; }
            get { return _strategyType; }
        }
        public AllocationType FundType
        {
            set { _fundType = value; }
            get { return _fundType; }
        }
        #endregion
    }
    public class AllocationType
    {
        private GridColumns _unAllocatedGridColumns;
        private GridColumns _allocatedGridColumns;
        private GridColumns _groupedGridColumns;
        public AllocationType()
         {
             _unAllocatedGridColumns = new GridColumns();
             _groupedGridColumns = new GridColumns();
             _allocatedGridColumns = new GridColumns();

         }
        public GridColumns UnAllocatedGridColumns
         {
             get { return _unAllocatedGridColumns; }
             set { _unAllocatedGridColumns = value; }

         }
        public GridColumns GroupedGridColumns
         {
             get { return _groupedGridColumns; }
             set { _groupedGridColumns = value; }

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
    
		/// <remarks/>
		
		
	}
    public class GeneralRules 
	{
    
		/// <remarks/>
		[XmlAttribute("ApplyRoundLotRules")]
		public bool ApplyRoundLotRules=true;
    
		/// <remarks/>
		[XmlAttribute("AveragePricingAllowed")]
		public bool AveragePricingAllowed=true;
    
		/// <remarks/>
		[XmlAttribute("IntegrateFundAndStrategyBundling")]
		public bool IntegrateFundAndStrategyBundling=false;
		public bool IsActiveTimer=false;
		public int UpdateInterval=10;
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
    public class GridColumns : ICloneable
	{
    
		/// <remarks/>
		
		//private string _typeOfGrid;
		private ArrayList columns = new ArrayList() ;
		//private string Gridtype=string.Empty;
    
		/// <remarks/>
		[XmlAttributeAttribute("SortKey")]
		public string SortKey=string.Empty;
    
		/// <remarks/>
		[XmlAttributeAttribute("Ascending")]
		public bool Ascending;
		public GridColumns()
		{
//			switch(Gridtype)
//			{
//				case AllocationConstants.UNALLOCATED_GRID:
////					string[] ColumnNames=Enum.GetNames(typeof(UnAllocatedOrder));
////					foreach(string str in ColumnNames)
////						columns.Add(str);
//					break;
//					case AllocationConstants.GROUPED_GRID:
//					 ColumnNames=Enum.GetNames(typeof(UnAllocatedOrder));
//						foreach(string str in ColumnNames)
//							columns.Add(str);
//					break;
//				case AllocationConstants.ALLOCATED_GRID:
//					ColumnNames=Enum.GetNames(typeof(UnAllocatedOrder));
//					foreach(string str in ColumnNames)
//						columns.Add(str);
//					break;
//
//			}
			

			
			
			SortKey="OrderID";
			Ascending=true;

		
		
		}

		public object Clone()
		{
			GridColumns gridColumns= new GridColumns();
			gridColumns.columns=new ArrayList();
			foreach(DisplayColumn obj in this.columns)
			gridColumns.AddColumn(obj.DisplayName);
			gridColumns.SortKey=this.SortKey;
			gridColumns.Ascending=this.Ascending;

			return gridColumns;
		}
		public void AddColumn(string columnname )
		{
			DisplayColumn displayColumn= new DisplayColumn();
			displayColumn.DisplayName=columnname;
			
			columns.Add(displayColumn);		
		}
		public void ClearColumns()
		{
			columns.Clear();
		
		
		
		}
		
		[System.Xml.Serialization.XmlElementAttribute("Column", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public DisplayColumn[] DisplayColumns
		{
			get 
			{
				DisplayColumn[] displayColumns = new DisplayColumn[ columns.Count ];
				columns.CopyTo( displayColumns );
				return displayColumns;
			}
			set 
			{
				if( value == null ) return;
				DisplayColumn[] displayColumns = (DisplayColumn[])value;
				columns.Clear();
				foreach( DisplayColumn item in displayColumns )
					columns.Add( item );
			}
		}



	}
    public class DisplayColumn 
	{
    
		/// <remarks/>
		[XmlAttributeAttribute("DisplayName")]
		public string DisplayName;
	}
}
