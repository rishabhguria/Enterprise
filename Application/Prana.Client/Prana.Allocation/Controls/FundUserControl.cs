using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Prana.Interfaces;
using Infragistics.Win.UltraWinGrid;
using System.Xml.Serialization ;
using System.IO;
using Infragistics.Win;
using Infragistics.Win.UltraWinEditors;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Infragistics.Win.UltraWinDock;
using Prana.BusinessObjects;
using Prana.Utilities.UIUtilities;
using Prana.Utilities.StringUtilities;
using Prana.CommonDataCache;
using Prana.Allocation.BLL;
namespace Prana.Allocation
{
	/// <summary>
	/// Summary description for FundControl.
	/// </summary>
	public class FundUserControl : System.Windows.Forms.UserControl
	{
	
		#region Windows Code

		private System.Windows.Forms.Label label11;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxExchange;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxUnderlying;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxAsset;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbbxDefaults;
		private System.Windows.Forms.Panel pnlUnAllocated;
		private System.Windows.Forms.Label lblShares;
		private System.Windows.Forms.RadioButton rbtnPercentage;
		private System.Windows.Forms.RadioButton rbtnNumber;
		private System.Windows.Forms.ContextMenu menuOrders;
		private System.Windows.Forms.MenuItem mnuOrdersGroup;
		private System.Windows.Forms.MenuItem mnuOrdersAllocate;
		private System.Windows.Forms.ContextMenu menuFunds;
		private System.Windows.Forms.MenuItem mnuFundsUnAllocate;
		private System.Windows.Forms.MenuItem mnuFundsProrata;
		private System.Windows.Forms.ContextMenu menuGroups;
		private System.Windows.Forms.MenuItem mnuGroupsAllocate;
		private System.Windows.Forms.MenuItem mnuGroupsUngroup;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdUnallocated;
		private System.Windows.Forms.Panel pnlTop;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdGrouped;
		private System.Windows.Forms.ErrorProvider errorProviderFunds;
		private System.Windows.Forms.GroupBox grpbxUnAllocatedFunds;
		private System.Windows.Forms.Label lblExchange;
		private System.Windows.Forms.Label lblUnderLying;
		private System.Windows.Forms.Label lblAsset;
        private System.Windows.Forms.Panel pnlUnallocatedNumberOrPercentage;
		private System.Windows.Forms.Label lblSelectFundDefault;
		private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _FundUserControlUnpinnedTabAreaLeft;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _FundUserControlUnpinnedTabAreaRight;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _FundUserControlUnpinnedTabAreaTop;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _FundUserControlUnpinnedTabAreaBottom;
		private Infragistics.Win.UltraWinDock.AutoHideControl _FundUserControlAutoHideControl;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdAllocated;
        private System.Windows.Forms.Label lblAllocation;
        private UltraGrid grdGroupedBaskets;
        private UltraGrid grdUnAllocatedBasket;
        private UltraGrid grdAllocatedBasket;
        private ContextMenu mnuBasketUnAllocate;
        private MenuItem menuItemBasketAllocate;
        private ContextMenu menuBasketAllocated;
        private MenuItem menuItemBasketUnAllocae;
        private MenuItem menuItemProrata;
        private ContextMenu menuBasketGroups;
        private MenuItem menuItemBasketGrpAllocate;
        private MenuItem menuItemBasketgrpUnGroup;
        private MenuItem menuItemBasketGroup;
        private MenuItem menuItemUnBundle;
        private Label label2;
        private Label label3;
        private Label label1;
        private UltraCombo cmbVenue;
        private UltraCombo cmbCounterParty;
        private TextBox txtSymbol;
        private Label label4;
        private UltraCombo cmbSide;
        private UltraCombo cmbOrderSide;
		private System.ComponentModel.IContainer components;
		
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
			
				
				if(components != null)
				{
					
					components.Dispose();

				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Private Members

      //  private bool isGridBinded = false;
        private Color _selectedRowBackColor = Color.Cornsilk;
        private Color _selectedRowForeColor = Color.Black;
        private Color _notTradedBackColor = Color.White;
        private Color _notTradedForeColor = Color.Black;
		private Label  [] lblUnAllocatedFunds;
		private TextBox[] txtUnAllocatedFunds;
		private AllocationConstants.SelectedTab _selectedTab=AllocationConstants.SelectedTab.UnAllocatedOrder;
		internal DateTime  currentDateTime;
        public DateTime AUECLocalDate = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;
        BasketAllocationManager _basketAllocationManager = null;
        OrderAllocationManager _orderAllocationManager = null;
        public bool InitializeUI = false;

        private AllocationPreferences _allocationPreferences;



        private Prana.BusinessObjects.FundCollection _funds;
        private Prana.BusinessObjects.StrategyCollection _strategies;

        private AllocationOrderCollection _orders;
        private AllocationGroups _groups;
        private AllocationGroups _allocatedGroups = new AllocationGroups();

        AllocationOrderCollection _selectedOrders = new AllocationOrderCollection();
        AllocationGroups _selectedGroups = new AllocationGroups();
        AllocationGroups _selectedAllocatedGroups = new AllocationGroups();

        BasketGroupCollection _groupedBaskets = new BasketGroupCollection();
        BasketGroupCollection _allocatedGroupedBaskets = new BasketGroupCollection();
        Prana.BusinessObjects.BasketCollection _unAllocatedBaskets = new Prana.BusinessObjects.BasketCollection();

        Assets assets = null;
        Exchanges exchanges = null;
        CounterPartyCollection counterParties = null;
        VenueCollection venues = null;
        Prana.BusinessObjects.BasketCollection _selectedBaskets = new Prana.BusinessObjects.BasketCollection();
        BasketGroupCollection _selectedGropuedBaskets = new BasketGroupCollection();
        BasketGroupCollection _selectedAllocatedBaskets = new BasketGroupCollection();
        
				

		private Prana.BusinessObjects.CompanyUser _loginUser;

		bool isSingleFundPermitted=false; 	

	    #region  Creation Filters
		CheckBoxOnHeader_CreationFilter headerCheckBoxUnallocated = new CheckBoxOnHeader_CreationFilter();
		CheckBoxOnHeader_CreationFilter headerCheckBoxAllocated = new CheckBoxOnHeader_CreationFilter();
		CheckBoxOnHeader_CreationFilter headerCheckBoxGrouped= new CheckBoxOnHeader_CreationFilter();
        CheckBoxOnHeader_CreationFilter headerCheckBoxUnAllocatedBasket = new CheckBoxOnHeader_CreationFilter();
        CheckBoxOnHeader_CreationFilter headerCheckBoxGroupedBasket = new CheckBoxOnHeader_CreationFilter();
        CheckBoxOnHeader_CreationFilter headerCheckBoxAllocatedBasket = new CheckBoxOnHeader_CreationFilter();
        #endregion

        //private string FORM_NAME="FundUserControl";


        private PranaInternalConstants.TYPE_OF_ALLOCATION _formType = PranaInternalConstants.TYPE_OF_ALLOCATION.FUND;
        string _deletedGroups = string.Empty;
        delegate void CrossThreadUICall();
        private bool initializeGrid = true;

       // private bool _isDataStateChanged = false;
		#endregion

		#region  Public Members
		public string addedEntityIDS=string.Empty ;
		public string deletedEntityIDS=string.Empty;
        public string addedBasketEntityIDS = string.Empty;
        public string deletedBasketEntityIDS = string.Empty;
        public string deletedGroupedBasketIDS = string.Empty;
        public string addedGroupedBasketIDS = string.Empty;
     
		//public  delegate void FundStrategyBundlingEventDelegate(AllocationGroup strategyEntity,bool Addition);
		//public event FundStrategyBundlingEventDelegate FundStrategyBundling;
		
		
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Percentages", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FundIDs", 3);
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Percentages", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FundIDs", 3);
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance107 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance108 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance109 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance110 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance111 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance112 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance113 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance114 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Percentages", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn12 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FundIDs", 3);
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Percentages", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FundIDs", 3);
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand6 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn23 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Percentages", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn24 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FundIDs", 3);
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance103 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance104 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance105 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand7 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn25 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn26 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn27 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Percentages", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn28 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FundIDs", 3);
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultName", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("DefaultID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Percentages", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FundIDs", 3);
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            this.grdUnallocated = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.menuOrders = new System.Windows.Forms.ContextMenu();
            this.mnuOrdersGroup = new System.Windows.Forms.MenuItem();
            this.mnuOrdersAllocate = new System.Windows.Forms.MenuItem();
            this.grdGrouped = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.menuGroups = new System.Windows.Forms.ContextMenu();
            this.mnuGroupsAllocate = new System.Windows.Forms.MenuItem();
            this.mnuGroupsUngroup = new System.Windows.Forms.MenuItem();
            this.menuFunds = new System.Windows.Forms.ContextMenu();
            this.mnuFundsUnAllocate = new System.Windows.Forms.MenuItem();
            this.mnuFundsProrata = new System.Windows.Forms.MenuItem();
            this.grpbxUnAllocatedFunds = new System.Windows.Forms.GroupBox();
            this.pnlUnAllocated = new System.Windows.Forms.Panel();
            this.lblAllocation = new System.Windows.Forms.Label();
            this.cmbbxDefaults = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblSelectFundDefault = new System.Windows.Forms.Label();
            this.pnlUnallocatedNumberOrPercentage = new System.Windows.Forms.Panel();
            this.lblShares = new System.Windows.Forms.Label();
            this.rbtnPercentage = new System.Windows.Forms.RadioButton();
            this.rbtnNumber = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbbxExchange = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblExchange = new System.Windows.Forms.Label();
            this.cmbbxUnderlying = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblUnderLying = new System.Windows.Forms.Label();
            this.cmbbxAsset = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblAsset = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbVenue = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCounterParty = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.errorProviderFunds = new System.Windows.Forms.ErrorProvider(this.components);
            this.ultraDockManager = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._FundUserControlUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._FundUserControlUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._FundUserControlUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._FundUserControlUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._FundUserControlAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.grdAllocated = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grdGroupedBaskets = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grdUnAllocatedBasket = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grdAllocatedBasket = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuBasketUnAllocate = new System.Windows.Forms.ContextMenu();
            this.menuItemBasketAllocate = new System.Windows.Forms.MenuItem();
            this.menuItemBasketGroup = new System.Windows.Forms.MenuItem();
            this.menuItemUnBundle = new System.Windows.Forms.MenuItem();
            this.menuBasketAllocated = new System.Windows.Forms.ContextMenu();
            this.menuItemBasketUnAllocae = new System.Windows.Forms.MenuItem();
            this.menuItemProrata = new System.Windows.Forms.MenuItem();
            this.menuBasketGroups = new System.Windows.Forms.ContextMenu();
            this.menuItemBasketGrpAllocate = new System.Windows.Forms.MenuItem();
            this.menuItemBasketgrpUnGroup = new System.Windows.Forms.MenuItem();
            this.cmbOrderSide = new Infragistics.Win.UltraWinGrid.UltraCombo();
            ((System.ComponentModel.ISupportInitialize)(this.grdUnallocated)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdGrouped)).BeginInit();
            this.grpbxUnAllocatedFunds.SuspendLayout();
            this.pnlUnAllocated.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxDefaults)).BeginInit();
            this.pnlUnallocatedNumberOrPercentage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxExchange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxUnderlying)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxAsset)).BeginInit();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderFunds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAllocated)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdGroupedBaskets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdUnAllocatedBasket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAllocatedBasket)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderSide)).BeginInit();
            this.SuspendLayout();
            // 
            // grdUnallocated
            // 
            this.grdUnallocated.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdUnallocated.ContextMenu = this.menuOrders;
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.Color.Black;
            this.grdUnallocated.DisplayLayout.Appearance = appearance1;
            this.grdUnallocated.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance2.BackColor = System.Drawing.Color.White;
            this.grdUnallocated.DisplayLayout.CaptionAppearance = appearance2;
            this.grdUnallocated.DisplayLayout.GroupByBox.Hidden = true;
            this.grdUnallocated.DisplayLayout.MaxColScrollRegions = 1;
            this.grdUnallocated.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdUnallocated.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdUnallocated.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdUnallocated.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdUnallocated.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;

            this.grdUnallocated.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnallocated.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnallocated.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdUnallocated.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.CellsOnly;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.FontData.Name = "Tahoma";
            this.grdUnallocated.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdUnallocated.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdUnallocated.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdUnallocated.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdUnallocated.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdUnallocated.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdUnallocated.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdUnallocated.Location = new System.Drawing.Point(6, 6);
            this.grdUnallocated.Name = "grdUnallocated";
            this.grdUnallocated.Size = new System.Drawing.Size(342, 58);
            this.grdUnallocated.TabIndex = 13;
            this.grdUnallocated.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate;
            this.grdUnallocated.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnallocated.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdUnallocated_InitializeRow);
            this.grdUnallocated.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdUnallocated_MouseUp);
            this.grdUnallocated.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdUnallocated_AfterSelectChange);
            this.grdUnallocated.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdUnallocated_InitializeLayout);
    
            // 
            // menuOrders
            // 
            this.menuOrders.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOrdersGroup,
            this.mnuOrdersAllocate});
            this.menuOrders.Popup += new System.EventHandler(this.menuOrders_Popup);
            // 
            // mnuOrdersGroup
            // 
            this.mnuOrdersGroup.Index = 0;
            this.mnuOrdersGroup.Text = "Group";
            this.mnuOrdersGroup.Click += new System.EventHandler(this.mnuOrdersGroup_Click);
            // 
            // mnuOrdersAllocate
            // 
            this.mnuOrdersAllocate.Index = 1;
            this.mnuOrdersAllocate.Text = "Allocate";
            this.mnuOrdersAllocate.Click += new System.EventHandler(this.mnuOrdersAllocate_Click);
            // 
            // grdGrouped
            // 
            this.grdGrouped.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdGrouped.ContextMenu = this.menuGroups;
            appearance4.BackColor = System.Drawing.Color.Black;
            appearance4.BackColor2 = System.Drawing.Color.Black;
            appearance4.BorderColor = System.Drawing.Color.Black;
            appearance4.FontData.Name = "Tahoma";
            appearance4.FontData.SizeInPoints = 8.25F;
            this.grdGrouped.DisplayLayout.Appearance = appearance4;
            this.grdGrouped.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance5.BackColor = System.Drawing.Color.White;
            this.grdGrouped.DisplayLayout.CaptionAppearance = appearance5;
            this.grdGrouped.DisplayLayout.GroupByBox.Hidden = true;
            this.grdGrouped.DisplayLayout.MaxColScrollRegions = 1;
            this.grdGrouped.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdGrouped.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdGrouped.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdGrouped.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdGrouped.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdGrouped.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdGrouped.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdGrouped.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdGrouped.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.CellsOnly;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdGrouped.DisplayLayout.Override.HeaderAppearance = appearance6;
            this.grdGrouped.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdGrouped.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdGrouped.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdGrouped.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdGrouped.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdGrouped.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdGrouped.Location = new System.Drawing.Point(6, 70);
            this.grdGrouped.Name = "grdGrouped";
            this.grdGrouped.Size = new System.Drawing.Size(321, 66);
            this.grdGrouped.TabIndex = 24;
            this.grdGrouped.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdGrouped.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdGrouped_InitializeRow);
            this.grdGrouped.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdGrouped_MouseUp);
            this.grdGrouped.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdGrouped_AfterSelectChange);
            this.grdGrouped.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdGrouped_InitializeLayout);
            // 
            // menuGroups
            // 
            this.menuGroups.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuGroupsAllocate,
            this.mnuGroupsUngroup});
            this.menuGroups.Popup += new System.EventHandler(this.menuGroups_Popup);
            // 
            // mnuGroupsAllocate
            // 
            this.mnuGroupsAllocate.Index = 0;
            this.mnuGroupsAllocate.Text = "Allocate";
            this.mnuGroupsAllocate.Click += new System.EventHandler(this.mnuGroupsAllocate_Click);
            // 
            // mnuGroupsUngroup
            // 
            this.mnuGroupsUngroup.Index = 1;
            this.mnuGroupsUngroup.Text = "UnGroup";
            this.mnuGroupsUngroup.Click += new System.EventHandler(this.mnuGroupsUngroup_Click);
            // 
            // menuFunds
            // 
            this.menuFunds.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFundsUnAllocate,
            this.mnuFundsProrata});
            this.menuFunds.Popup += new System.EventHandler(this.mnuFunds_Popup);
            // 
            // mnuFundsUnAllocate
            // 
            this.mnuFundsUnAllocate.Index = 0;
            this.mnuFundsUnAllocate.Text = "UnAllocate";
            this.mnuFundsUnAllocate.Click += new System.EventHandler(this.mnuFundsUnAllocate_Click);
            // 
            // mnuFundsProrata
            // 
            this.mnuFundsProrata.Index = 1;
            this.mnuFundsProrata.Text = "Prorata";
            this.mnuFundsProrata.Visible = false;
            this.mnuFundsProrata.Click += new System.EventHandler(this.mnuFundsProrata_Click);
            // 
            // grpbxUnAllocatedFunds
            // 
            this.grpbxUnAllocatedFunds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpbxUnAllocatedFunds.Controls.Add(this.pnlUnAllocated);
            this.grpbxUnAllocatedFunds.Location = new System.Drawing.Point(698, 6);
            this.grpbxUnAllocatedFunds.Name = "grpbxUnAllocatedFunds";
            this.grpbxUnAllocatedFunds.Size = new System.Drawing.Size(176, 406);
            this.grpbxUnAllocatedFunds.TabIndex = 25;
            this.grpbxUnAllocatedFunds.TabStop = false;
            // 
            // pnlUnAllocated
            // 
            this.pnlUnAllocated.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.pnlUnAllocated.Controls.Add(this.lblAllocation);
            this.pnlUnAllocated.Controls.Add(this.cmbbxDefaults);
            this.pnlUnAllocated.Controls.Add(this.lblSelectFundDefault);
            this.pnlUnAllocated.Controls.Add(this.pnlUnallocatedNumberOrPercentage);
            this.pnlUnAllocated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUnAllocated.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.pnlUnAllocated.Location = new System.Drawing.Point(3, 17);
            this.pnlUnAllocated.Name = "pnlUnAllocated";
            this.pnlUnAllocated.Size = new System.Drawing.Size(170, 386);
            this.pnlUnAllocated.TabIndex = 14;
            // 
            // lblAllocation
            // 
            this.lblAllocation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAllocation.Location = new System.Drawing.Point(4, 44);
            this.lblAllocation.Name = "lblAllocation";
            this.lblAllocation.Size = new System.Drawing.Size(162, 23);
            this.lblAllocation.TabIndex = 6;
            this.lblAllocation.Text = "AllocationFund";
            this.lblAllocation.Visible = false;
            // 
            // cmbbxDefaults
            // 
            appearance7.FontData.BoldAsString = "False";
            this.cmbbxDefaults.Appearance = appearance7;
            this.cmbbxDefaults.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            appearance8.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxDefaults.DisplayLayout.Appearance = appearance8;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.Caption = "";
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand1.GroupHeadersVisible = false;
            this.cmbbxDefaults.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbbxDefaults.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxDefaults.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance9.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxDefaults.DisplayLayout.GroupByBox.Appearance = appearance9;
            appearance10.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxDefaults.DisplayLayout.GroupByBox.BandLabelAppearance = appearance10;
            this.cmbbxDefaults.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance11.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance11.BackColor2 = System.Drawing.SystemColors.Control;
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance11.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxDefaults.DisplayLayout.GroupByBox.PromptAppearance = appearance11;
            this.cmbbxDefaults.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxDefaults.DisplayLayout.MaxRowScrollRegions = 1;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxDefaults.DisplayLayout.Override.ActiveCellAppearance = appearance12;
            appearance13.BackColor = System.Drawing.SystemColors.Highlight;
            appearance13.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxDefaults.DisplayLayout.Override.ActiveRowAppearance = appearance13;
            this.cmbbxDefaults.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxDefaults.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance14.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxDefaults.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BorderColor = System.Drawing.Color.Silver;
            appearance15.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxDefaults.DisplayLayout.Override.CellAppearance = appearance15;
            this.cmbbxDefaults.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxDefaults.DisplayLayout.Override.CellPadding = 0;
            appearance16.BackColor = System.Drawing.SystemColors.Control;
            appearance16.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance16.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxDefaults.DisplayLayout.Override.GroupByRowAppearance = appearance16;
            appearance17.TextHAlignAsString = "Left";
            this.cmbbxDefaults.DisplayLayout.Override.HeaderAppearance = appearance17;
            this.cmbbxDefaults.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxDefaults.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance18.BackColor = System.Drawing.SystemColors.Window;
            appearance18.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxDefaults.DisplayLayout.Override.RowAppearance = appearance18;
            this.cmbbxDefaults.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance19.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxDefaults.DisplayLayout.Override.TemplateAddRowAppearance = appearance19;
            this.cmbbxDefaults.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxDefaults.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxDefaults.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxDefaults.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbbxDefaults.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxDefaults.Location = new System.Drawing.Point(94, 38);
            this.cmbbxDefaults.Name = "cmbbxDefaults";
            this.cmbbxDefaults.Size = new System.Drawing.Size(76, 21);
            this.cmbbxDefaults.TabIndex = 5;
            this.cmbbxDefaults.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxDefaults.ValueChanged += new System.EventHandler(this.cmbbxDefaults_ValueChanged);
            // 
            // lblSelectFundDefault
            // 
            this.lblSelectFundDefault.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblSelectFundDefault.Location = new System.Drawing.Point(4, 40);
            this.lblSelectFundDefault.Name = "lblSelectFundDefault";
            this.lblSelectFundDefault.Size = new System.Drawing.Size(92, 16);
            this.lblSelectFundDefault.TabIndex = 4;
            this.lblSelectFundDefault.Text = "Allocation %";
            // 
            // pnlUnallocatedNumberOrPercentage
            // 
            this.pnlUnallocatedNumberOrPercentage.Controls.Add(this.lblShares);
            this.pnlUnallocatedNumberOrPercentage.Controls.Add(this.rbtnPercentage);
            this.pnlUnallocatedNumberOrPercentage.Controls.Add(this.rbtnNumber);
            this.pnlUnallocatedNumberOrPercentage.Controls.Add(this.label11);
            this.pnlUnallocatedNumberOrPercentage.Location = new System.Drawing.Point(48, 4);
            this.pnlUnallocatedNumberOrPercentage.Name = "pnlUnallocatedNumberOrPercentage";
            this.pnlUnallocatedNumberOrPercentage.Size = new System.Drawing.Size(106, 36);
            this.pnlUnallocatedNumberOrPercentage.TabIndex = 2;
            // 
            // lblShares
            // 
            this.lblShares.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblShares.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShares.Location = new System.Drawing.Point(64, 6);
            this.lblShares.Name = "lblShares";
            this.lblShares.Size = new System.Drawing.Size(46, 12);
            this.lblShares.TabIndex = 2;
            this.lblShares.Text = "Shares";
            // 
            // rbtnPercentage
            // 
            this.rbtnPercentage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.rbtnPercentage.Checked = true;
            this.rbtnPercentage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rbtnPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnPercentage.Location = new System.Drawing.Point(10, 1);
            this.rbtnPercentage.Name = "rbtnPercentage";
            this.rbtnPercentage.Size = new System.Drawing.Size(17, 24);
            this.rbtnPercentage.TabIndex = 0;
            this.rbtnPercentage.TabStop = true;
            // 
            // rbtnNumber
            // 
            this.rbtnNumber.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.rbtnNumber.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.rbtnNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnNumber.Location = new System.Drawing.Point(52, 0);
            this.rbtnNumber.Name = "rbtnNumber";
            this.rbtnNumber.Size = new System.Drawing.Size(12, 24);
            this.rbtnNumber.TabIndex = 1;
            this.rbtnNumber.CheckedChanged += new System.EventHandler(this.rbtnNumber_CheckedChanged);
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(28, 6);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 14);
            this.label11.TabIndex = 3;
            this.label11.Text = "%";
            // 
            // cmbbxExchange
            // 
            this.cmbbxExchange.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance68.BackColor = System.Drawing.SystemColors.Window;
            appearance68.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxExchange.DisplayLayout.Appearance = appearance68;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn5.Header.Caption = "";
            ultraGridColumn5.Header.VisiblePosition = 0;
            ultraGridColumn6.Header.VisiblePosition = 1;
            ultraGridColumn6.Hidden = true;
            ultraGridColumn7.Header.VisiblePosition = 2;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn8.Header.VisiblePosition = 3;
            ultraGridColumn8.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8});
            ultraGridBand2.GroupHeadersVisible = false;
            this.cmbbxExchange.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbbxExchange.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxExchange.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance69.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance69.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance69.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance69.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxExchange.DisplayLayout.GroupByBox.Appearance = appearance69;
            appearance70.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxExchange.DisplayLayout.GroupByBox.BandLabelAppearance = appearance70;
            this.cmbbxExchange.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance106.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance106.BackColor2 = System.Drawing.SystemColors.Control;
            appearance106.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance106.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxExchange.DisplayLayout.GroupByBox.PromptAppearance = appearance106;
            this.cmbbxExchange.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxExchange.DisplayLayout.MaxRowScrollRegions = 1;
            appearance107.BackColor = System.Drawing.SystemColors.Window;
            appearance107.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxExchange.DisplayLayout.Override.ActiveCellAppearance = appearance107;
            appearance108.BackColor = System.Drawing.SystemColors.Highlight;
            appearance108.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxExchange.DisplayLayout.Override.ActiveRowAppearance = appearance108;
            this.cmbbxExchange.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxExchange.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance109.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxExchange.DisplayLayout.Override.CardAreaAppearance = appearance109;
            appearance110.BorderColor = System.Drawing.Color.Silver;
            appearance110.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxExchange.DisplayLayout.Override.CellAppearance = appearance110;
            this.cmbbxExchange.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxExchange.DisplayLayout.Override.CellPadding = 0;
            appearance111.BackColor = System.Drawing.SystemColors.Control;
            appearance111.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance111.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance111.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance111.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxExchange.DisplayLayout.Override.GroupByRowAppearance = appearance111;
            appearance112.TextHAlignAsString = "Left";
            this.cmbbxExchange.DisplayLayout.Override.HeaderAppearance = appearance112;
            this.cmbbxExchange.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxExchange.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance113.BackColor = System.Drawing.SystemColors.Window;
            appearance113.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxExchange.DisplayLayout.Override.RowAppearance = appearance113;
            this.cmbbxExchange.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance114.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxExchange.DisplayLayout.Override.TemplateAddRowAppearance = appearance114;
            this.cmbbxExchange.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxExchange.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxExchange.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxExchange.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbbxExchange.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxExchange.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmbbxExchange.Location = new System.Drawing.Point(88, 130);
            this.cmbbxExchange.Name = "cmbbxExchange";
            this.cmbbxExchange.Size = new System.Drawing.Size(82, 21);
            this.cmbbxExchange.TabIndex = 29;
            this.cmbbxExchange.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxExchange.ValueChanged += new System.EventHandler(this.cmbbxExchange_ValueChanged);
            // 
            // lblExchange
            // 
            this.lblExchange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblExchange.Location = new System.Drawing.Point(3, 129);
            this.lblExchange.Name = "lblExchange";
            this.lblExchange.Size = new System.Drawing.Size(62, 22);
            this.lblExchange.TabIndex = 28;
            this.lblExchange.Text = "Exchange";
            // 
            // cmbbxUnderlying
            // 
            this.cmbbxUnderlying.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance32.BackColor = System.Drawing.SystemColors.Window;
            appearance32.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxUnderlying.DisplayLayout.Appearance = appearance32;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridColumn9.Header.Caption = "";
            ultraGridColumn9.Header.VisiblePosition = 0;
            ultraGridColumn10.Header.VisiblePosition = 1;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn11.Header.VisiblePosition = 2;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn12.Header.VisiblePosition = 3;
            ultraGridColumn12.Hidden = true;
            ultraGridBand3.Columns.AddRange(new object[] {
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11,
            ultraGridColumn12});
            ultraGridBand3.GroupHeadersVisible = false;
            this.cmbbxUnderlying.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbbxUnderlying.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxUnderlying.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance33.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxUnderlying.DisplayLayout.GroupByBox.Appearance = appearance33;
            appearance34.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxUnderlying.DisplayLayout.GroupByBox.BandLabelAppearance = appearance34;
            this.cmbbxUnderlying.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance35.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance35.BackColor2 = System.Drawing.SystemColors.Control;
            appearance35.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance35.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxUnderlying.DisplayLayout.GroupByBox.PromptAppearance = appearance35;
            this.cmbbxUnderlying.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxUnderlying.DisplayLayout.MaxRowScrollRegions = 1;
            appearance36.BackColor = System.Drawing.SystemColors.Window;
            appearance36.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxUnderlying.DisplayLayout.Override.ActiveCellAppearance = appearance36;
            appearance37.BackColor = System.Drawing.SystemColors.Highlight;
            appearance37.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxUnderlying.DisplayLayout.Override.ActiveRowAppearance = appearance37;
            this.cmbbxUnderlying.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxUnderlying.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance38.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxUnderlying.DisplayLayout.Override.CardAreaAppearance = appearance38;
            appearance39.BorderColor = System.Drawing.Color.Silver;
            appearance39.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxUnderlying.DisplayLayout.Override.CellAppearance = appearance39;
            this.cmbbxUnderlying.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxUnderlying.DisplayLayout.Override.CellPadding = 0;
            appearance40.BackColor = System.Drawing.SystemColors.Control;
            appearance40.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance40.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance40.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxUnderlying.DisplayLayout.Override.GroupByRowAppearance = appearance40;
            appearance41.TextHAlignAsString = "Left";
            this.cmbbxUnderlying.DisplayLayout.Override.HeaderAppearance = appearance41;
            this.cmbbxUnderlying.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxUnderlying.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance42.BackColor = System.Drawing.SystemColors.Window;
            appearance42.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxUnderlying.DisplayLayout.Override.RowAppearance = appearance42;
            this.cmbbxUnderlying.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance43.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxUnderlying.DisplayLayout.Override.TemplateAddRowAppearance = appearance43;
            this.cmbbxUnderlying.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxUnderlying.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxUnderlying.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxUnderlying.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbbxUnderlying.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxUnderlying.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmbbxUnderlying.Location = new System.Drawing.Point(88, 76);
            this.cmbbxUnderlying.Name = "cmbbxUnderlying";
            this.cmbbxUnderlying.Size = new System.Drawing.Size(82, 21);
            this.cmbbxUnderlying.TabIndex = 27;
            this.cmbbxUnderlying.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbbxUnderlying.ValueChanged += new System.EventHandler(this.cmbbxUnderlying_ValueChanged);
            // 
            // lblUnderLying
            // 
            this.lblUnderLying.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblUnderLying.Location = new System.Drawing.Point(3, 76);
            this.lblUnderLying.Name = "lblUnderLying";
            this.lblUnderLying.Size = new System.Drawing.Size(68, 22);
            this.lblUnderLying.TabIndex = 26;
            this.lblUnderLying.Text = "Underlying";
            // 
            // cmbbxAsset
            // 
            this.cmbbxAsset.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance44.BackColor = System.Drawing.SystemColors.Window;
            appearance44.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbbxAsset.DisplayLayout.Appearance = appearance44;
            ultraGridBand4.ColHeadersVisible = false;
            ultraGridColumn13.Header.Caption = "";
            ultraGridColumn13.Header.VisiblePosition = 0;
            ultraGridColumn14.Header.VisiblePosition = 1;
            ultraGridColumn14.Hidden = true;
            ultraGridColumn15.Header.VisiblePosition = 2;
            ultraGridColumn15.Hidden = true;
            ultraGridColumn16.Header.VisiblePosition = 3;
            ultraGridColumn16.Hidden = true;
            ultraGridBand4.Columns.AddRange(new object[] {
            ultraGridColumn13,
            ultraGridColumn14,
            ultraGridColumn15,
            ultraGridColumn16});
            ultraGridBand4.GroupHeadersVisible = false;
            this.cmbbxAsset.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.cmbbxAsset.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbbxAsset.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance45.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance45.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxAsset.DisplayLayout.GroupByBox.Appearance = appearance45;
            appearance46.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxAsset.DisplayLayout.GroupByBox.BandLabelAppearance = appearance46;
            this.cmbbxAsset.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance47.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance47.BackColor2 = System.Drawing.SystemColors.Control;
            appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance47.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbbxAsset.DisplayLayout.GroupByBox.PromptAppearance = appearance47;
            this.cmbbxAsset.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbbxAsset.DisplayLayout.MaxRowScrollRegions = 1;
            appearance48.BackColor = System.Drawing.SystemColors.Window;
            appearance48.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbbxAsset.DisplayLayout.Override.ActiveCellAppearance = appearance48;
            appearance49.BackColor = System.Drawing.SystemColors.Highlight;
            appearance49.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbbxAsset.DisplayLayout.Override.ActiveRowAppearance = appearance49;
            this.cmbbxAsset.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbbxAsset.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance50.BackColor = System.Drawing.SystemColors.Window;
            this.cmbbxAsset.DisplayLayout.Override.CardAreaAppearance = appearance50;
            appearance51.BorderColor = System.Drawing.Color.Silver;
            appearance51.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbbxAsset.DisplayLayout.Override.CellAppearance = appearance51;
            this.cmbbxAsset.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbbxAsset.DisplayLayout.Override.CellPadding = 0;
            appearance52.BackColor = System.Drawing.SystemColors.Control;
            appearance52.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance52.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance52.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbbxAsset.DisplayLayout.Override.GroupByRowAppearance = appearance52;
            appearance53.TextHAlignAsString = "Left";
            this.cmbbxAsset.DisplayLayout.Override.HeaderAppearance = appearance53;
            this.cmbbxAsset.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbbxAsset.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance54.BackColor = System.Drawing.SystemColors.Window;
            appearance54.BorderColor = System.Drawing.Color.Silver;
            this.cmbbxAsset.DisplayLayout.Override.RowAppearance = appearance54;
            this.cmbbxAsset.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance55.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbbxAsset.DisplayLayout.Override.TemplateAddRowAppearance = appearance55;
            this.cmbbxAsset.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbbxAsset.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbbxAsset.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbbxAsset.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbbxAsset.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbbxAsset.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmbbxAsset.Location = new System.Drawing.Point(88, 49);
            this.cmbbxAsset.Name = "cmbbxAsset";
            this.cmbbxAsset.Size = new System.Drawing.Size(82, 21);
            this.cmbbxAsset.TabIndex = 25;
            this.cmbbxAsset.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // lblAsset
            // 
            this.lblAsset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblAsset.Location = new System.Drawing.Point(3, 49);
            this.lblAsset.Name = "lblAsset";
            this.lblAsset.Size = new System.Drawing.Size(70, 18);
            this.lblAsset.TabIndex = 24;
            this.lblAsset.Text = "Asset ";
            // 
            // pnlTop
            // 
            this.pnlTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTop.Controls.Add(this.cmbOrderSide);
            this.pnlTop.Controls.Add(this.txtSymbol);
            this.pnlTop.Controls.Add(this.label2);
            this.pnlTop.Controls.Add(this.label4);
            this.pnlTop.Controls.Add(this.label3);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Controls.Add(this.cmbVenue);
            this.pnlTop.Controls.Add(this.cmbCounterParty);
            this.pnlTop.Controls.Add(this.cmbbxExchange);
            this.pnlTop.Controls.Add(this.lblExchange);
            this.pnlTop.Controls.Add(this.cmbbxUnderlying);
            this.pnlTop.Controls.Add(this.lblUnderLying);
            this.pnlTop.Controls.Add(this.cmbbxAsset);
            this.pnlTop.Controls.Add(this.lblAsset);
            this.pnlTop.Location = new System.Drawing.Point(452, 6);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(191, 252);
            this.pnlTop.TabIndex = 2;
            // 
            // txtSymbol
            // 
            this.txtSymbol.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtSymbol.Location = new System.Drawing.Point(88, 20);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(82, 21);
            this.txtSymbol.TabIndex = 37;
            this.txtSymbol.TextChanged += new System.EventHandler(this.txtSymbol_TextChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label2.Location = new System.Drawing.Point(3, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 22);
            this.label2.TabIndex = 36;
            this.label2.Text = "Counter Party";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label4.Location = new System.Drawing.Point(3, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 22);
            this.label4.TabIndex = 38;
            this.label4.Text = "Order Side";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label3.Location = new System.Drawing.Point(3, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 22);
            this.label3.TabIndex = 35;
            this.label3.Text = "Venue";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label1.Location = new System.Drawing.Point(3, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 22);
            this.label1.TabIndex = 33;
            this.label1.Text = "Symbol";
            // 
            // cmbVenue
            // 
            this.cmbVenue.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance67.BackColor = System.Drawing.SystemColors.Window;
            appearance67.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbVenue.DisplayLayout.Appearance = appearance67;
            ultraGridBand6.ColHeadersVisible = false;
            ultraGridColumn21.Header.Caption = "";
            ultraGridColumn21.Header.VisiblePosition = 0;
            ultraGridColumn22.Header.VisiblePosition = 1;
            ultraGridColumn22.Hidden = true;
            ultraGridColumn23.Header.VisiblePosition = 2;
            ultraGridColumn23.Hidden = true;
            ultraGridColumn24.Header.VisiblePosition = 3;
            ultraGridColumn24.Hidden = true;
            ultraGridBand6.Columns.AddRange(new object[] {
            ultraGridColumn21,
            ultraGridColumn22,
            ultraGridColumn23,
            ultraGridColumn24});
            ultraGridBand6.GroupHeadersVisible = false;
            this.cmbVenue.DisplayLayout.BandsSerializer.Add(ultraGridBand6);
            this.cmbVenue.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbVenue.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance71.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance71.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance71.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance71.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.GroupByBox.Appearance = appearance71;
            appearance72.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.BandLabelAppearance = appearance72;
            this.cmbVenue.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance73.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance73.BackColor2 = System.Drawing.SystemColors.Control;
            appearance73.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance73.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.PromptAppearance = appearance73;
            this.cmbVenue.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbVenue.DisplayLayout.MaxRowScrollRegions = 1;
            appearance74.BackColor = System.Drawing.SystemColors.Window;
            appearance74.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbVenue.DisplayLayout.Override.ActiveCellAppearance = appearance74;
            appearance75.BackColor = System.Drawing.SystemColors.Highlight;
            appearance75.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbVenue.DisplayLayout.Override.ActiveRowAppearance = appearance75;
            this.cmbVenue.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbVenue.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance76.BackColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.CardAreaAppearance = appearance76;
            appearance77.BorderColor = System.Drawing.Color.Silver;
            appearance77.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbVenue.DisplayLayout.Override.CellAppearance = appearance77;
            this.cmbVenue.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbVenue.DisplayLayout.Override.CellPadding = 0;
            appearance78.BackColor = System.Drawing.SystemColors.Control;
            appearance78.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance78.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance78.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance78.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.GroupByRowAppearance = appearance78;
            appearance103.TextHAlignAsString = "Left";
            this.cmbVenue.DisplayLayout.Override.HeaderAppearance = appearance103;
            this.cmbVenue.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbVenue.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance104.BackColor = System.Drawing.SystemColors.Window;
            appearance104.BorderColor = System.Drawing.Color.Silver;
            this.cmbVenue.DisplayLayout.Override.RowAppearance = appearance104;
            this.cmbVenue.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance105.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbVenue.DisplayLayout.Override.TemplateAddRowAppearance = appearance105;
            this.cmbVenue.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbVenue.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbVenue.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbVenue.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbVenue.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbVenue.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmbVenue.Location = new System.Drawing.Point(88, 184);
            this.cmbVenue.Name = "cmbVenue";
            this.cmbVenue.Size = new System.Drawing.Size(82, 21);
            this.cmbVenue.TabIndex = 32;
            this.cmbVenue.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbVenue.ValueChanged += new System.EventHandler(this.cmbVenue_ValueChanged);
            // 
            // cmbCounterParty
            // 
            this.cmbCounterParty.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance91.BackColor = System.Drawing.SystemColors.Window;
            appearance91.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCounterParty.DisplayLayout.Appearance = appearance91;
            ultraGridBand7.ColHeadersVisible = false;
            ultraGridColumn25.Header.Caption = "";
            ultraGridColumn25.Header.VisiblePosition = 0;
            ultraGridColumn26.Header.VisiblePosition = 1;
            ultraGridColumn26.Hidden = true;
            ultraGridColumn27.Header.VisiblePosition = 2;
            ultraGridColumn27.Hidden = true;
            ultraGridColumn28.Header.VisiblePosition = 3;
            ultraGridColumn28.Hidden = true;
            ultraGridBand7.Columns.AddRange(new object[] {
            ultraGridColumn25,
            ultraGridColumn26,
            ultraGridColumn27,
            ultraGridColumn28});
            ultraGridBand7.GroupHeadersVisible = false;
            this.cmbCounterParty.DisplayLayout.BandsSerializer.Add(ultraGridBand7);
            this.cmbCounterParty.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterParty.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance92.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance92.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance92.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance92.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.GroupByBox.Appearance = appearance92;
            appearance93.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BandLabelAppearance = appearance93;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance94.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance94.BackColor2 = System.Drawing.SystemColors.Control;
            appearance94.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance94.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.PromptAppearance = appearance94;
            this.cmbCounterParty.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCounterParty.DisplayLayout.MaxRowScrollRegions = 1;
            appearance95.BackColor = System.Drawing.SystemColors.Window;
            appearance95.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveCellAppearance = appearance95;
            appearance96.BackColor = System.Drawing.SystemColors.Highlight;
            appearance96.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveRowAppearance = appearance96;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance97.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.CardAreaAppearance = appearance97;
            appearance98.BorderColor = System.Drawing.Color.Silver;
            appearance98.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCounterParty.DisplayLayout.Override.CellAppearance = appearance98;
            this.cmbCounterParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCounterParty.DisplayLayout.Override.CellPadding = 0;
            appearance99.BackColor = System.Drawing.SystemColors.Control;
            appearance99.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance99.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance99.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance99.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.GroupByRowAppearance = appearance99;
            appearance100.TextHAlignAsString = "Left";
            this.cmbCounterParty.DisplayLayout.Override.HeaderAppearance = appearance100;
            this.cmbCounterParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCounterParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance101.BackColor = System.Drawing.SystemColors.Window;
            appearance101.BorderColor = System.Drawing.Color.Silver;
            this.cmbCounterParty.DisplayLayout.Override.RowAppearance = appearance101;
            this.cmbCounterParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance102.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCounterParty.DisplayLayout.Override.TemplateAddRowAppearance = appearance102;
            this.cmbCounterParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCounterParty.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbCounterParty.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCounterParty.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmbCounterParty.Location = new System.Drawing.Point(88, 157);
            this.cmbCounterParty.Name = "cmbCounterParty";
            this.cmbCounterParty.Size = new System.Drawing.Size(82, 21);
            this.cmbCounterParty.TabIndex = 31;
            this.cmbCounterParty.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCounterParty.ValueChanged += new System.EventHandler(this.cmbCounterParty_ValueChanged);
            // 
            // errorProviderFunds
            // 
            this.errorProviderFunds.ContainerControl = this;
            // 
            // ultraDockManager
            // 
            this.ultraDockManager.DefaultGroupSettings.TabStyle = Infragistics.Win.UltraWinTabs.TabStyle.Excel;
            appearance56.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance56.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ultraDockManager.DefaultPaneSettings.ActivePaneAppearance = appearance56;
            appearance57.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance57.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance57.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ultraDockManager.DefaultPaneSettings.SelectedTabAppearance = appearance57;
            this.ultraDockManager.HostControl = this;
            this.ultraDockManager.ShowCloseButton = false;
            this.ultraDockManager.UseDefaultContextMenus = false;
            this.ultraDockManager.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraDockManager.PaneActivate += new Infragistics.Win.UltraWinDock.ControlPaneEventHandler(this.ultraDockManager_PaneActivate);
            this.ultraDockManager.BeforePaneButtonClick += new Infragistics.Win.UltraWinDock.CancelablePaneButtonEventHandler(this.ultraDockManager_BeforePaneButtonClick);
            // 
            // _FundUserControlUnpinnedTabAreaLeft
            // 
            this._FundUserControlUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._FundUserControlUnpinnedTabAreaLeft.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._FundUserControlUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._FundUserControlUnpinnedTabAreaLeft.Name = "_FundUserControlUnpinnedTabAreaLeft";
            this._FundUserControlUnpinnedTabAreaLeft.Owner = this.ultraDockManager;
            this._FundUserControlUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 414);
            this._FundUserControlUnpinnedTabAreaLeft.TabIndex = 26;
            // 
            // _FundUserControlUnpinnedTabAreaRight
            // 
            this._FundUserControlUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._FundUserControlUnpinnedTabAreaRight.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._FundUserControlUnpinnedTabAreaRight.Location = new System.Drawing.Point(880, 0);
            this._FundUserControlUnpinnedTabAreaRight.Name = "_FundUserControlUnpinnedTabAreaRight";
            this._FundUserControlUnpinnedTabAreaRight.Owner = this.ultraDockManager;
            this._FundUserControlUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 414);
            this._FundUserControlUnpinnedTabAreaRight.TabIndex = 27;
            // 
            // _FundUserControlUnpinnedTabAreaTop
            // 
            this._FundUserControlUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._FundUserControlUnpinnedTabAreaTop.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._FundUserControlUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._FundUserControlUnpinnedTabAreaTop.Name = "_FundUserControlUnpinnedTabAreaTop";
            this._FundUserControlUnpinnedTabAreaTop.Owner = this.ultraDockManager;
            this._FundUserControlUnpinnedTabAreaTop.Size = new System.Drawing.Size(880, 0);
            this._FundUserControlUnpinnedTabAreaTop.TabIndex = 28;
            // 
            // _FundUserControlUnpinnedTabAreaBottom
            // 
            this._FundUserControlUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._FundUserControlUnpinnedTabAreaBottom.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._FundUserControlUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 414);
            this._FundUserControlUnpinnedTabAreaBottom.Name = "_FundUserControlUnpinnedTabAreaBottom";
            this._FundUserControlUnpinnedTabAreaBottom.Owner = this.ultraDockManager;
            this._FundUserControlUnpinnedTabAreaBottom.Size = new System.Drawing.Size(880, 0);
            this._FundUserControlUnpinnedTabAreaBottom.TabIndex = 29;
            // 
            // _FundUserControlAutoHideControl
            // 
            this._FundUserControlAutoHideControl.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this._FundUserControlAutoHideControl.Location = new System.Drawing.Point(0, 0);
            this._FundUserControlAutoHideControl.Name = "_FundUserControlAutoHideControl";
            this._FundUserControlAutoHideControl.Owner = this.ultraDockManager;
            this._FundUserControlAutoHideControl.Size = new System.Drawing.Size(0, 0);
            this._FundUserControlAutoHideControl.TabIndex = 30;
            // 
            // grdAllocated
            // 
            this.grdAllocated.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAllocated.ContextMenu = this.menuFunds;
            appearance65.BackColor = System.Drawing.Color.Black;
            appearance65.BackColor2 = System.Drawing.Color.Black;
            appearance65.BorderColor = System.Drawing.Color.Black;
            this.grdAllocated.DisplayLayout.Appearance = appearance65;
            this.grdAllocated.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grdAllocated.DisplayLayout.GroupByBox.Hidden = true;
            this.grdAllocated.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAllocated.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdAllocated.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdAllocated.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdAllocated.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocated.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocated.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocated.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance66.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance66.FontData.Name = "Tahoma";
            this.grdAllocated.DisplayLayout.Override.HeaderAppearance = appearance66;
            this.grdAllocated.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdAllocated.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdAllocated.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.grdAllocated.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAllocated.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAllocated.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdAllocated.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAllocated.Location = new System.Drawing.Point(16, 287);
            this.grdAllocated.Name = "grdAllocated";
            this.grdAllocated.Size = new System.Drawing.Size(213, 117);
            this.grdAllocated.TabIndex = 28;
            this.grdAllocated.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocated.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdAllocated_InitializeRow);
            this.grdAllocated.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdAllocated_MouseUp);
            this.grdAllocated.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdAllocated_AfterSelectChange);
            this.grdAllocated.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdAllocated_InitializeLayout);
            this.grdAllocated.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdAllocated_DragDrop);
            // 
            // grdGroupedBaskets
            // 
            this.grdGroupedBaskets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance60.BackColor = System.Drawing.Color.Black;
            appearance60.BackColor2 = System.Drawing.Color.Black;
            appearance60.BorderColor = System.Drawing.Color.Black;
            this.grdGroupedBaskets.DisplayLayout.Appearance = appearance60;
            this.grdGroupedBaskets.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance61.BackColor = System.Drawing.Color.White;
            this.grdGroupedBaskets.DisplayLayout.CaptionAppearance = appearance61;
            this.grdGroupedBaskets.DisplayLayout.GroupByBox.Hidden = true;
            this.grdGroupedBaskets.DisplayLayout.MaxColScrollRegions = 1;
            this.grdGroupedBaskets.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdGroupedBaskets.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdGroupedBaskets.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdGroupedBaskets.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdGroupedBaskets.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdGroupedBaskets.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdGroupedBaskets.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdGroupedBaskets.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdGroupedBaskets.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.CellsOnly;
            appearance62.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance62.FontData.Name = "Tahoma";
            this.grdGroupedBaskets.DisplayLayout.Override.HeaderAppearance = appearance62;
            this.grdGroupedBaskets.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdGroupedBaskets.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdGroupedBaskets.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdGroupedBaskets.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdGroupedBaskets.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdGroupedBaskets.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdGroupedBaskets.Location = new System.Drawing.Point(6, 219);
            this.grdGroupedBaskets.Name = "grdGroupedBaskets";
            this.grdGroupedBaskets.Size = new System.Drawing.Size(333, 62);
            this.grdGroupedBaskets.TabIndex = 31;
            this.grdGroupedBaskets.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate;
            this.grdGroupedBaskets.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdGroupedBaskets.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdGroupedBaskets_InitializeRow);
            this.grdGroupedBaskets.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdGroupedBaskets_MouseUp);
            this.grdGroupedBaskets.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdGroupedBaskets_AfterSelectChange);
            // 
            // grdUnAllocatedBasket
            // 
            this.grdUnAllocatedBasket.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance63.BackColor = System.Drawing.Color.Black;
            appearance63.BackColor2 = System.Drawing.Color.Black;
            appearance63.BorderColor = System.Drawing.Color.Black;
            this.grdUnAllocatedBasket.DisplayLayout.Appearance = appearance63;
            this.grdUnAllocatedBasket.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grdUnAllocatedBasket.DisplayLayout.GroupByBox.Hidden = true;
            this.grdUnAllocatedBasket.DisplayLayout.MaxColScrollRegions = 1;
            this.grdUnAllocatedBasket.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdUnAllocatedBasket.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdUnAllocatedBasket.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdUnAllocatedBasket.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnAllocatedBasket.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnAllocatedBasket.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnAllocatedBasket.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance64.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance64.FontData.Name = "Tahoma";
            this.grdUnAllocatedBasket.DisplayLayout.Override.HeaderAppearance = appearance64;
            this.grdUnAllocatedBasket.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdUnAllocatedBasket.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdUnAllocatedBasket.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.grdUnAllocatedBasket.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdUnAllocatedBasket.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdUnAllocatedBasket.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdUnAllocatedBasket.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdUnAllocatedBasket.Location = new System.Drawing.Point(6, 142);
            this.grdUnAllocatedBasket.Name = "grdUnAllocatedBasket";
            this.grdUnAllocatedBasket.Size = new System.Drawing.Size(333, 71);
            this.grdUnAllocatedBasket.TabIndex = 32;
            this.grdUnAllocatedBasket.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnAllocatedBasket.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdUnAllocatedBasket_InitializeRow);
            this.grdUnAllocatedBasket.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdUnAllocatedBasket_MouseUp);
            this.grdUnAllocatedBasket.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdUnAllocatedBasket_AfterSelectChange);
            // 
            // grdAllocatedBasket
            // 
            this.grdAllocatedBasket.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAllocatedBasket.ContextMenu = this.menuFunds;
            appearance58.BackColor = System.Drawing.Color.Black;
            appearance58.BackColor2 = System.Drawing.Color.Black;
            appearance58.BorderColor = System.Drawing.Color.Black;
            this.grdAllocatedBasket.DisplayLayout.Appearance = appearance58;
            this.grdAllocatedBasket.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grdAllocatedBasket.DisplayLayout.GroupByBox.Hidden = true;
            this.grdAllocatedBasket.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAllocatedBasket.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdAllocatedBasket.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdAllocatedBasket.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdAllocatedBasket.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocatedBasket.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            this.grdAllocatedBasket.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocatedBasket.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance59.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance59.FontData.Name = "Tahoma";
            this.grdAllocatedBasket.DisplayLayout.Override.HeaderAppearance = appearance59;
            this.grdAllocatedBasket.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdAllocatedBasket.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdAllocatedBasket.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.grdAllocatedBasket.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAllocatedBasket.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAllocatedBasket.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdAllocatedBasket.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAllocatedBasket.Location = new System.Drawing.Point(252, 294);
            this.grdAllocatedBasket.Name = "grdAllocatedBasket";
            this.grdAllocatedBasket.Size = new System.Drawing.Size(213, 117);
            this.grdAllocatedBasket.TabIndex = 33;
            this.grdAllocatedBasket.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdAllocatedBasket.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdAllocatedBasket_InitializeRow);
            this.grdAllocatedBasket.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdAllocatedBasket_MouseUp);
            this.grdAllocatedBasket.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdAllocatedBasket_AfterSelectChange);
            // 
            // mnuBasketUnAllocate
            // 
            this.mnuBasketUnAllocate.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemBasketAllocate,
            this.menuItemBasketGroup,
            this.menuItemUnBundle});
            this.mnuBasketUnAllocate.Popup += new System.EventHandler(this.mnuBasketUnAllocate_Popup);
            // 
            // menuItemBasketAllocate
            // 
            this.menuItemBasketAllocate.Index = 0;
            this.menuItemBasketAllocate.Text = "Allocate";
            this.menuItemBasketAllocate.Click += new System.EventHandler(this.menuItemBasketAllocate_Click);
            // 
            // menuItemBasketGroup
            // 
            this.menuItemBasketGroup.Index = 1;
            this.menuItemBasketGroup.Text = "Group";
            this.menuItemBasketGroup.Visible = false;
            this.menuItemBasketGroup.Click += new System.EventHandler(this.menuItemBasketGroup_Click);
            // 
            // menuItemUnBundle
            // 
            this.menuItemUnBundle.Index = 2;
            this.menuItemUnBundle.Text = "UnBundle";
            this.menuItemUnBundle.Click += new System.EventHandler(this.menuItemUnBundle_Click);
            // 
            // menuBasketAllocated
            // 
            this.menuBasketAllocated.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemBasketUnAllocae,
            this.menuItemProrata});
            this.menuBasketAllocated.Popup += new System.EventHandler(this.menuBasketAllocated_Popup);
            // 
            // menuItemBasketUnAllocae
            // 
            this.menuItemBasketUnAllocae.Index = 0;
            this.menuItemBasketUnAllocae.Text = "UnAllocate";
            this.menuItemBasketUnAllocae.Click += new System.EventHandler(this.menuItemBasketUnAllocae_Click);
            // 
            // menuItemProrata
            // 
            this.menuItemProrata.Index = 1;
            this.menuItemProrata.Text = "Prorata";
            this.menuItemProrata.Visible = false;
            this.menuItemProrata.Click += new System.EventHandler(this.menuItemProrata_Click);
            // 
            // menuBasketGroups
            // 
            this.menuBasketGroups.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemBasketGrpAllocate,
            this.menuItemBasketgrpUnGroup});
            this.menuBasketGroups.Popup += new System.EventHandler(this.menuBasketGroups_Popup);
            // 
            // menuItemBasketGrpAllocate
            // 
            this.menuItemBasketGrpAllocate.Index = 0;
            this.menuItemBasketGrpAllocate.Text = "Allocate";
            this.menuItemBasketGrpAllocate.Click += new System.EventHandler(this.menuItemBasketGrpAllocate_Click);
            // 
            // menuItemBasketgrpUnGroup
            // 
            this.menuItemBasketgrpUnGroup.Index = 1;
            this.menuItemBasketgrpUnGroup.Text = "UnGroup";
            this.menuItemBasketgrpUnGroup.Click += new System.EventHandler(this.menuItemBasketgrpUnGroup_Click);
            // 
            // cmbOrderSide
            // 
            this.cmbOrderSide.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance79.BackColor = System.Drawing.SystemColors.Window;
            appearance79.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbOrderSide.DisplayLayout.Appearance = appearance79;
            ultraGridBand5.ColHeadersVisible = false;
            ultraGridColumn17.Header.Caption = "";
            ultraGridColumn17.Header.VisiblePosition = 0;
            ultraGridColumn18.Header.VisiblePosition = 1;
            ultraGridColumn18.Hidden = true;
            ultraGridColumn19.Header.VisiblePosition = 2;
            ultraGridColumn19.Hidden = true;
            ultraGridColumn20.Header.VisiblePosition = 3;
            ultraGridColumn20.Hidden = true;
            ultraGridBand5.Columns.AddRange(new object[] {
            ultraGridColumn17,
            ultraGridColumn18,
            ultraGridColumn19,
            ultraGridColumn20});
            ultraGridBand5.GroupHeadersVisible = false;
            this.cmbOrderSide.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.cmbOrderSide.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOrderSide.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance80.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance80.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance80.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance80.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderSide.DisplayLayout.GroupByBox.Appearance = appearance80;
            appearance81.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderSide.DisplayLayout.GroupByBox.BandLabelAppearance = appearance81;
            this.cmbOrderSide.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance82.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance82.BackColor2 = System.Drawing.SystemColors.Control;
            appearance82.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance82.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderSide.DisplayLayout.GroupByBox.PromptAppearance = appearance82;
            this.cmbOrderSide.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbOrderSide.DisplayLayout.MaxRowScrollRegions = 1;
            appearance83.BackColor = System.Drawing.SystemColors.Window;
            appearance83.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbOrderSide.DisplayLayout.Override.ActiveCellAppearance = appearance83;
            appearance84.BackColor = System.Drawing.SystemColors.Highlight;
            appearance84.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbOrderSide.DisplayLayout.Override.ActiveRowAppearance = appearance84;
            this.cmbOrderSide.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbOrderSide.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance85.BackColor = System.Drawing.SystemColors.Window;
            this.cmbOrderSide.DisplayLayout.Override.CardAreaAppearance = appearance85;
            appearance86.BorderColor = System.Drawing.Color.Silver;
            appearance86.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbOrderSide.DisplayLayout.Override.CellAppearance = appearance86;
            this.cmbOrderSide.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbOrderSide.DisplayLayout.Override.CellPadding = 0;
            appearance87.BackColor = System.Drawing.SystemColors.Control;
            appearance87.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance87.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance87.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance87.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderSide.DisplayLayout.Override.GroupByRowAppearance = appearance87;
            appearance88.TextHAlignAsString = "Left";
            this.cmbOrderSide.DisplayLayout.Override.HeaderAppearance = appearance88;
            this.cmbOrderSide.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbOrderSide.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance89.BackColor = System.Drawing.SystemColors.Window;
            appearance89.BorderColor = System.Drawing.Color.Silver;
            this.cmbOrderSide.DisplayLayout.Override.RowAppearance = appearance89;
            this.cmbOrderSide.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance90.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbOrderSide.DisplayLayout.Override.TemplateAddRowAppearance = appearance90;
            this.cmbOrderSide.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbOrderSide.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbOrderSide.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbOrderSide.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbOrderSide.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbOrderSide.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmbOrderSide.Location = new System.Drawing.Point(88, 103);
            this.cmbOrderSide.Name = "cmbOrderSide";
            this.cmbOrderSide.Size = new System.Drawing.Size(82, 21);
            this.cmbOrderSide.TabIndex = 39;
            this.cmbOrderSide.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbOrderSide.ValueChanged += new System.EventHandler(this.cmbOrderSide_ValueChanged);
            // 
            // FundUserControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this._FundUserControlAutoHideControl);
            this.Controls.Add(this.grdAllocatedBasket);
            this.Controls.Add(this.grdGroupedBaskets);
            this.Controls.Add(this.grdUnAllocatedBasket);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.grpbxUnAllocatedFunds);
            this.Controls.Add(this.grdUnallocated);
            this.Controls.Add(this.grdGrouped);
            this.Controls.Add(this.grdAllocated);
            this.Controls.Add(this._FundUserControlUnpinnedTabAreaTop);
            this.Controls.Add(this._FundUserControlUnpinnedTabAreaBottom);
            this.Controls.Add(this._FundUserControlUnpinnedTabAreaLeft);
            this.Controls.Add(this._FundUserControlUnpinnedTabAreaRight);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "FundUserControl";
            this.Size = new System.Drawing.Size(880, 414);
            ((System.ComponentModel.ISupportInitialize)(this.grdUnallocated)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdGrouped)).EndInit();
            this.grpbxUnAllocatedFunds.ResumeLayout(false);
            this.pnlUnAllocated.ResumeLayout(false);
            this.pnlUnAllocated.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxDefaults)).EndInit();
            this.pnlUnallocatedNumberOrPercentage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxExchange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxUnderlying)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbbxAsset)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderFunds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAllocated)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdGroupedBaskets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdUnAllocatedBasket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAllocatedBasket)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderSide)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region Constructor and SetUp
		public FundUserControl()
		{
          
            try
            {
               
                InitializeComponent();
                //custom filters in pnlTop hidden as default infragistics filters working now
                this.pnlTop.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
			// TODO: Add any initialization after the InitializeComponent call
		}

        private int _read_write=0;

        public int Read_Write
        {
            get { return _read_write; }
            set { _read_write = value; }
        }
        	
        private bool _isCurrentDate = true;

        public bool IsCurrentDate
        {
            get { return _isCurrentDate; }
            set { _isCurrentDate = value; }
        }

        public void SetUp(Prana.BusinessObjects.CompanyUser loginUser, PranaInternalConstants.TYPE_OF_ALLOCATION formType)
		{
            try
            {
                _formType = formType;
                if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                {
                    _basketAllocationManager = BasketFundAllocationManager.GetInstance;
                    _orderAllocationManager = OrderFundAllocationManager.GetInstance ;
                }
                else
                {
                    _basketAllocationManager = BasketStrategyAllocationManager.GetInstance;
                    _orderAllocationManager = OrderStrategyAllocationManager.GetInstance;
                }
                _loginUser = loginUser;
                BackgroundWorker backGroundWorker = new BackgroundWorker();
                backGroundWorker.DoWork += new DoWorkEventHandler(backGroundWorker_DoWork);
                backGroundWorker.RunWorkerAsync();
                backGroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backGroundWorker_RunWorkerCompleted);
                //_basketAllocationManager.Initilize(_loginUser.CompanyUserID);
               
                _unAllocatedBaskets = _basketAllocationManager.UnallocatedBaskets;
                _groupedBaskets = _basketAllocationManager.Groupedbaskets;
                _allocatedGroupedBaskets = _basketAllocationManager.AllocatedBasketGroups;

                _orders = _orderAllocationManager.Orders;
                _groups = _orderAllocationManager.Groups;
                _allocatedGroups = _orderAllocationManager.AllocatedGroups;

                
                if (InitializeUI.Equals(false))
                {
                    _allocationPreferences = AllocationPreferencesManager.AllocationPreferences;
                    _funds = new Prana.BusinessObjects.FundCollection();
                    _strategies = new Prana.BusinessObjects.StrategyCollection();
                    CreateDockManagerPanes();
                    InitilizeUI();
                }

            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }


            }		
		}

        void backGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        void backGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string AllAUECDatesString = string.Empty;
            if (IsCurrentDate)
            {
                AllAUECDatesString = TimeZoneHelper.GetAllAUECLocalDatesFromUTCStr(DateTime.UtcNow);
            }
            else
            {
                AllAUECDatesString = TimeZoneHelper.GetSameDateForAllAUEC(currentDateTime);
            }

            _basketAllocationManager.Initilize(_loginUser.CompanyUserID, AllAUECDatesString);
        }

        private void FillFunds()
        {
            Prana.BusinessObjects.FundCollection funds = CachedDataManager.GetInstance.GetUserFunds();
            foreach (Prana.BusinessObjects.Fund fund in funds)
            {
                if (fund.FundID != int.MinValue)
                {
                    _funds.Add(fund);
                }
            }
        }
        private void FillStrategies()
        {
            Prana.BusinessObjects.StrategyCollection strategies = CachedDataManager.GetInstance.GetUserStrategies();
            foreach (Prana.BusinessObjects.Strategy strategy in strategies)
            {
                if (strategy.StrategyID != int.MinValue)
                {
                    _strategies.Add(strategy);
                }
            }
        }
        private void InitilizeUI()
        {
            InitializeUI = true;
            BindGrids(initializeGrid );
            ApplyGridSetting();
            //isGridBinded = true;
            //ApplyRowColorUnAllocated();
            //ApplyRowColorGrouped();
            //ApplyRowColorAllocated();
            SetPreferences();
            grdUnAllocatedBasket.ContextMenu = this.mnuBasketUnAllocate;
            grdGroupedBaskets.ContextMenu = this.menuBasketGroups;
            grdAllocatedBasket.ContextMenu = this.menuBasketAllocated;


            if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
            {

                FillFunds();
                AddFundsTextBoxes();
                if (_funds.Count == 1)
                    isSingleFundPermitted = true;
                lblAllocation.Text = "Allocation Fund";
                BindFundDefaults();
            }
            if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY)
            {
                FillStrategies();
                AddStrategiesTextBoxes();
                if (_strategies.Count == 1)
                    isSingleFundPermitted = true;
                lblAllocation.Text = "Allocation Strategy";
                BindStrategyDefaults();
            }
            
            BindFilterCombos();
            headerCheckBoxUnallocated._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxUnallocated__CLICKED);
            headerCheckBoxGrouped._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxGrouped__CLICKED);
            headerCheckBoxAllocated._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxAllocated__CLICKED);
            headerCheckBoxUnAllocatedBasket._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxUnAllocatedBasket__CLICKED);
            headerCheckBoxGroupedBasket._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxGroupedBasket__CLICKED);
            headerCheckBoxAllocatedBasket._CLICKED += new CheckBoxOnHeader_CreationFilter.HeaderCheckBoxClickedHandler(headerCheckBoxAllocatedBasket__CLICKED);
        }

        private void CreateDockManagerPanes()
		{
            string fundText = string.Empty;
            if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
            {
                fundText="Allocation Fund";
            }
            else
            { 
                 fundText="Allocation Strategy";
            }

            DockableControlPane paneRight = new DockableControlPane("Right", fundText, grpbxUnAllocatedFunds); 
			//DockableControlPane paneFilters = new DockableControlPane( "Filters", "Filters", pnlTop);
			DockAreaPane dockAreaRight = new DockAreaPane(DockedLocation.DockedRight); 			
			dockAreaRight.Panes.Add( paneRight); 
			//dockAreaRight.Panes.Add( paneFilters); 
			dockAreaRight.Size = new Size(190, 200); 


			dockAreaRight.ChildPaneStyle = ChildPaneStyle.TabGroup; 
			 
		
			dockAreaRight.GroupSettings.TabSizing = Infragistics.Win.UltraWinTabs.TabSizing.AutoSize; 

            // UnAllocated and Grouped Grid Addition
            DockableControlPane paneUnAllocated = new DockableControlPane(AllocationConstants.SelectedTab.UnAllocatedOrder.ToString(), "Un-Allocated Orders", grdUnallocated);
            DockableControlPane paneGrouped = new DockableControlPane(AllocationConstants.SelectedTab.GroupedOrder.ToString(), "Grouped Orders", grdGrouped);

            // Basket UnAllocated and Grouped Grid Addition
            DockableControlPane paneUnAllocatedBaskets = new DockableControlPane(AllocationConstants.SelectedTab.UnAllocatedBasket.ToString(), "Un-Allocated Baskets", grdUnAllocatedBasket);
            DockableControlPane paneGroupedBaskets = new DockableControlPane(AllocationConstants.SelectedTab.GroupedBasket.ToString(), "Grouped Baskets", grdGroupedBaskets); 
			
			DockAreaPane dockAreaUnAllocated = new DockAreaPane(DockedLocation.DockedTop); 
			dockAreaUnAllocated.Panes.Add( paneUnAllocated ); 
			dockAreaUnAllocated.Panes.Add( paneGrouped );
            dockAreaUnAllocated.Panes.Add(paneUnAllocatedBaskets);
            dockAreaUnAllocated.Panes.Add(paneGroupedBaskets); 
			dockAreaUnAllocated.Size = new Size(350, 250); 

			dockAreaUnAllocated.ChildPaneStyle = ChildPaneStyle.TabGroup; 
			 
		
			dockAreaUnAllocated.GroupSettings.TabSizing = Infragistics.Win.UltraWinTabs.TabSizing.AutoSize;



            DockableControlPane paneAllocated = new DockableControlPane(AllocationConstants.SelectedTab.AllocatedOrder.ToString(), "Allocated Orders", grdAllocated);
            DockableControlPane paneAllocatedBasket = new DockableControlPane(AllocationConstants.SelectedTab.AllocatedBasket.ToString(), "Allocated Baskets", grdAllocatedBasket); 
 
			//create a dock area on the right to contain the rich text 
			DockAreaPane dockAreaAllocated = new DockAreaPane(DockedLocation.DockedBottom);
            
 
			// initialize the size of the dock area 
			dockAreaAllocated.Size = new Size(120, 250); 
 
			// contain the rich text pane in the dock area 
			dockAreaAllocated.Panes.Add( paneAllocated );
            dockAreaAllocated.Panes.Add(paneAllocatedBasket);
            dockAreaAllocated.ChildPaneStyle = ChildPaneStyle.TabGroup;


            dockAreaAllocated.GroupSettings.TabSizing = Infragistics.Win.UltraWinTabs.TabSizing.AutoSize; 
			// finally, add the dock areas to the dock manager 
         if(ultraDockManager.DockAreas.Count.Equals(0))
            this.ultraDockManager.DockAreas.AddRange(new DockAreaPane[]
                                        { dockAreaRight, dockAreaUnAllocated, dockAreaAllocated }); 
          
		}
        public  void BindGrids(bool initilise)
        {
            try
            {
                if (initilise)
                {
                    OrderBind(new AllocationOrderCollection());
                    FundsBind(new AllocationGroups());
                    GroupBind(new AllocationGroups());
                    BindUnAllocatedBaskets(new Prana.BusinessObjects.BasketCollection());
                    BindGroupedBaskets(new BasketGroupCollection());
                    BindAllocatedBaskets(new BasketGroupCollection());
                }
                else
                {
                    initializeGrid = false;
                    SetDataSourceCollections();
                    OrderBind(_orders);
                    FundsBind(_allocatedGroups);
                    GroupBind(_groups);
                    BindUnAllocatedBaskets(_unAllocatedBaskets );
                    BindGroupedBaskets(_groupedBaskets);
                    BindAllocatedBaskets(_allocatedGroupedBaskets);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }


        }
        private void BindFilterCombos()
        {
            BackgroundWorker bgworkerBindCombos = new BackgroundWorker();
            bgworkerBindCombos.DoWork += new DoWorkEventHandler(bgworkerBindCombos_DoWork);
            bgworkerBindCombos.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgworkerBindCombos_RunWorkerCompleted);
            bgworkerBindCombos.RunWorkerAsync();
        }

        void bgworkerBindCombos_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BindAsset();
            BindExchange();
            BindUndeyLying(int.MinValue);
            BindCounterParty();
            BindVenue(int.MinValue);
            BindOrderSide();
        }

        void bgworkerBindCombos_DoWork(object sender, DoWorkEventArgs e)
        {
            assets = ClientsCommonDataManager.GetCompanyUserAssets(_loginUser.CompanyUserID);
            assets.Insert(0, new Asset(int.MinValue, ApplicationConstants.C_COMBO_ALL));
            exchanges = ClientsCommonDataManager.GetExchnagesByUserID(_loginUser.CompanyUserID);
            exchanges.Insert(0, new Exchange(int.MinValue, ApplicationConstants.C_COMBO_ALL));
            counterParties = ClientsCommonDataManager.GetCompanyCounterParties(_loginUser.CompanyID);
            counterParties.Insert(0, new CounterParty(int.MinValue, ApplicationConstants.C_COMBO_ALL));
            cmbbxAsset.ValueChanged+=new EventHandler(cmbbxAsset_ValueChanged);
            
        }
        
		#endregion

		#region ContextMenu

		private void mnuOrdersGroup_Click(object sender, System.EventArgs e)
		{
			GroupOrder(AUECLocalDate);
			
		}
		public void GroupOrder(DateTime AUECLocalDate)
		{
			if(_selectedOrders.Count <= 1)
			{
				MessageBox.Show("Select More than One AllocationOrder");
				return ;
			}
            bool bValidated = true;

            string AlreadyAllocatedOrdersString = string.Empty;
            if (bValidated)
            {
                foreach (AllocationOrder aOrder in _selectedOrders)
                {
                    if (aOrder.StateID.Equals(PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED))
                    {
                        AlreadyAllocatedOrdersString += "Order " + aOrder.Symbol
                                                      + " " + aOrder.OrderSide
                                                      + " " + aOrder.OrderType
                                                      + " " + aOrder.Quantity +
                                                      " " + "Already Grouped" + " on "
                                                      + aOrder.GroupAuecLocalDate.Month
                                                      + "/" + aOrder.GroupAuecLocalDate.Day
                                                      + "/" + aOrder.GroupAuecLocalDate.Year + "\n";

                        //+ " is gouped or allocated"
                        bValidated = false;
                    }
                    else if (aOrder.StateID.Equals(PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED))
                    {
                        AlreadyAllocatedOrdersString += "Order " + aOrder.Symbol
                                                      + " " + aOrder.OrderSide
                                                      + " " + aOrder.OrderType
                                                      + " " + aOrder.Quantity +
                                                      " " + "Already Allocated" + " on "
                                                      + aOrder.GroupAuecLocalDate.Month
                                                      + "/" + aOrder.GroupAuecLocalDate.Day
                                                      + "/" + aOrder.GroupAuecLocalDate.Year + "\n";

                        //+ " is gouped or allocated"
                        bValidated = false;
                    }
                }
            }

            if (!bValidated)
            {
                MessageBox.Show(AlreadyAllocatedOrdersString);
                return;
            }
			try
			{
  
                
				bool result=GroupManager.isGroupingRulePassed(_selectedOrders,_allocationPreferences);
                if (result)
                {
                    _orderAllocationManager.GroupOrders(_selectedOrders, false, AUECLocalDate);
                }
                else

                    MessageBox.Show("AllocationOrderCollection of Different types can't be Grouped");

                UnCheckAllRows(grdUnallocated, headerCheckBoxUnallocated);
                _selectedOrders = new AllocationOrderCollection();

			}
				#region Catch
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
			}		
			#endregion
		}
		private void mnuOrdersAllocate_Click(object sender, System.EventArgs e)
		{
			AllocateOrder();
			
		}
		public void AllocateOrder()
		{
			if(_selectedOrders.Count ==0)
				return ;
            
			OnAllocateClick();
			if(_formType==PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
			{
				OrderAllocationToFund(AUECLocalDate);
			}
			else
			{
                OrderAllocationToStrategy(AUECLocalDate);
			}
            UnCheckAllRows(grdUnallocated, headerCheckBoxUnallocated);
            _selectedOrders = new AllocationOrderCollection();
		}


		private void mnuGroupsAllocate_Click(object sender, System.EventArgs e)
		{
			AllocateGroup();

		}
		public void AllocateGroup()
		{
			if(_selectedGroups.Count ==0)
				return ;
			OnAllocateClick();
			
			if(_formType==PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
			{
				GroupAllocatedFund();
			
			}
			else
			{
				GroupAllocatedStrategy();
			}
            UnCheckAllRows(grdGrouped, headerCheckBoxGrouped);
            _selectedGroups.Clear();
		}
	
		private void mnuGroupsUngroup_Click(object sender, System.EventArgs e)
		{
			UnGroup();
		
		}

		public void UnGroup()
		{
            try
            {
                if (_selectedGroups.Count == 0)
                {
                    MessageBox.Show("Please Select a AllocationGroup");
                    return;
                }

                _orderAllocationManager.UnBundleGroup(_selectedGroups, AUECLocalDate);
                _selectedGroups = new AllocationGroups();
                headerCheckBoxGrouped.Checked = CheckState.Unchecked;

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }		
			#endregion

		}
		private void mnuFundsUnAllocate_Click(object sender, System.EventArgs e)
		{
			UnAllocate();	
		
		}        
		public void UnAllocate()
		{
            try
            {
                _orderAllocationManager.UnBundleGroup(_selectedAllocatedGroups, AUECLocalDate);
                UnCheckAllRows(grdAllocated, headerCheckBoxAllocated);
                _selectedAllocatedGroups.Clear();
            }
            #region Catch
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
			#endregion

		}

		private void mnuFundsProrata_Click(object sender, System.EventArgs e)
		{
          
			try
			{
                foreach (AllocationGroup group in _selectedAllocatedGroups)
                {
                    _orderAllocationManager.ProrataAllocatedGroup(group);
                }
				UnCheckAllRows(grdAllocated, headerCheckBoxUnallocated);
                _selectedAllocatedGroups.Clear();
			}
				#region Catch
			catch(Exception ex)
			{
				// Invoke our policy that is responsible for making sure no secure information
				// gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
			}		
			#endregion
		}

		#endregion

		#region	Tabs
		public void AddFundsTextBoxes()
		{
            
			lblUnAllocatedFunds= new Label[_funds.Count];
			txtUnAllocatedFunds = new TextBox[_funds.Count];
			int i=0;
			int startTxtBox_X=110;
			int startLabel_X=8;
			int start_Y=90;
			int yIncrement=40;
			int lblLength=90;
			int lblheight=20;
			int txtbxLength=50;
			int txtbxheight=10;
            try
            {
                for (i = 0; i < lblUnAllocatedFunds.Length; i++)
                {
                    Prana.BusinessObjects.Fund fund = (Prana.BusinessObjects.Fund)(_funds[i]);

                    lblUnAllocatedFunds[i] = new Label();
                    lblUnAllocatedFunds[i].Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    //lblUnAllocatedFunds[i].Anchor=AnchorStyles.Bottom;

                    txtUnAllocatedFunds[i] = new TextBox();
                    txtUnAllocatedFunds[i].Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    pnlUnAllocated.Controls.Add(lblUnAllocatedFunds[i]);
                    pnlUnAllocated.Controls.Add(txtUnAllocatedFunds[i]);
                    //				lblUnAllocatedFunds[i].Anchor =AnchorStyles.Top;
                    //				txtUnAllocatedFunds[i].Anchor = AnchorStyles.Top;

                    lblUnAllocatedFunds[i].Location = new System.Drawing.Point(startLabel_X, start_Y + i * yIncrement);
                    lblUnAllocatedFunds[i].Size = new System.Drawing.Size(lblLength, lblheight);
                    lblUnAllocatedFunds[i].Name = fund.FundID.ToString();
                    lblUnAllocatedFunds[i].Text = fund.Name;


                    txtUnAllocatedFunds[i].Location = new System.Drawing.Point(startTxtBox_X, start_Y + i * yIncrement);
                    txtUnAllocatedFunds[i].Size = new System.Drawing.Size(txtbxLength, txtbxheight);
                    txtUnAllocatedFunds[i].Text = "";
                    txtUnAllocatedFunds[i].Name = fund.FundID.ToString();

                }


                if (_funds.Count > 0)
                {

                }
                else
                    pnlUnallocatedNumberOrPercentage.Visible = false;

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }		
			#endregion
		}

		public void AddStrategiesTextBoxes()
		{
			lblUnAllocatedFunds= new Label[_strategies.Count];
			txtUnAllocatedFunds = new TextBox[_strategies.Count];
			int i=0;
			int startTxtBox_X=110;
			int startLabel_X=8;
			int start_Y=90;
			int yIncrement=40;
			int lblLength=90;
			int lblheight=20;
			int txtbxLength=50;
			int txtbxheight=10;
            try
            {
                for (i = 0; i < lblUnAllocatedFunds.Length; i++)
                {
                    Prana.BusinessObjects.Strategy strategy=(( Prana.BusinessObjects.Strategy)(_strategies[i]));
                    lblUnAllocatedFunds[i] = new Label();
                    lblUnAllocatedFunds[i].Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    txtUnAllocatedFunds[i] = new TextBox();
                    txtUnAllocatedFunds[i].Font = lblUnAllocatedFunds[i].Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    pnlUnAllocated.Controls.Add(lblUnAllocatedFunds[i]);
                    pnlUnAllocated.Controls.Add(txtUnAllocatedFunds[i]);


                    lblUnAllocatedFunds[i].Location = new System.Drawing.Point(startLabel_X, start_Y + i * yIncrement);
                    lblUnAllocatedFunds[i].Name = strategy.StrategyID.ToString();
                    lblUnAllocatedFunds[i].Size = new System.Drawing.Size(lblLength, lblheight);
                    lblUnAllocatedFunds[i].Text = strategy.Name;


                    txtUnAllocatedFunds[i].Location = new System.Drawing.Point(startTxtBox_X, start_Y + i * yIncrement);
                    txtUnAllocatedFunds[i].Size = new System.Drawing.Size(txtbxLength, txtbxheight);
                    txtUnAllocatedFunds[i].Text = "";
                    txtUnAllocatedFunds[i].Name = strategy.StrategyID.ToString();


                }
                if (_strategies.Count > 0)
                {

                }
                else
                    pnlUnallocatedNumberOrPercentage.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
		
		}

		public void ShowNumberInUnAllocatedTextBoxes(double  exeQty)
		{
			try
			{
				ClearUnAllocatedFundsText();
				if(cmbbxDefaults.Value==null)
					return;
                if (cmbbxDefaults.Value.ToString().Equals(ApplicationConstants.C_COMBO_SELECT))
					return;
				string FundIDS= cmbbxDefaults.SelectedRow.Cells["FundIDS"].Value.ToString();
				string Percentages= cmbbxDefaults.SelectedRow.Cells["Percentages"].Value.ToString();
				string [] FundIDCollection=FundIDS.Split(',');
				string[] PercentageCollection=Percentages.Split(',');
				
				#region TextBox Filling		
				foreach(TextBox txtbx in txtUnAllocatedFunds)
				{
					int i=0;
				
					foreach(string str in FundIDCollection)
					{
					
						if(str.Equals(txtbx.Name))
						{
							txtbx.Text=PercentageCollection[i].ToString();
							break;
										
						}
						i++;

					}
				

			
				}
				#endregion					

				foreach(TextBox txtbx in txtUnAllocatedFunds)
				{
					Int64  shares=Convert.ToInt64((exeQty*float.Parse(txtbx.Text.ToString()))/100);
					txtbx.Text=shares.ToString();
			
				}

				//Adjust the Shares in case of Discrepencies
                //double sumOfSharesInTextBoxes = SumOfSharesInTextBoxes();
                //if( sumOfSharesInTextBoxes< exeQty)
                //    AdjustShares(SumOfSharesInTextBoxes(),exeQty);
			
			}
				#region Catch
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
				
				
			}		
			#endregion
		}

		public void ShowPercentagesInUnAllocatedTextBoxes()
		{
			ClearUnAllocatedFundsText();
            //if (_selectedTab == AllocationConstants.SelectedTab.AllocatedOrder || _selectedTab == AllocationConstants.SelectedTab.AllocatedBasket)
            //{
            //    return;
            //}
			
			try
			{
				if(cmbbxDefaults.Value==null)
				{
					return;
				}
                if (cmbbxDefaults.Value.ToString().Equals(ApplicationConstants.C_COMBO_SELECT))
				{
					return;
				}

			
				string FundIDS= cmbbxDefaults.SelectedRow.Cells["FundIDS"].Value.ToString();
				string Percentages= cmbbxDefaults.SelectedRow.Cells["Percentages"].Value.ToString();
				string [] FundIDCollection=FundIDS.Split(',');
				string[] PercentageCollection=Percentages.Split(',');
				
				#region TextBox Filling		
				foreach(TextBox txtbx in txtUnAllocatedFunds)
				{
					int i=0;
				
					foreach(string str in FundIDCollection)
					{
					
						if(str.Equals(txtbx.Name))
						{
							txtbx.Text=PercentageCollection[i].ToString();
							break;
										
						}
						i++;

					}
				

			
				}
				#endregion
			
			}
				#region Catch
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
				
				
			}		
			#endregion
		}

		public void ShowPercentageOrNumberAllocatedFunds(AllocationFunds funds,bool percentage)
		{
			
			
			try
			{
				ClearAllocatedFundsText();
                foreach (AllocationFund fund in funds)
				{
					int i=0;
					foreach(TextBox txtbx in txtUnAllocatedFunds)
					{
						if(txtbx.Name==fund.FundID.ToString())
						{
							if(percentage)
							{
								txtUnAllocatedFunds[i].Text=fund.Percentage.ToString();
								rbtnPercentage.Checked=true;
							
							}
							else
							{
								txtUnAllocatedFunds[i].Text=fund.AllocatedQty.ToString();							
								rbtnNumber.Checked=true;
							}	
						
							break;
					    
						}
						i++;
					}
		
				
				}
			}
				#region Catch
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
				
				
			}		
			#endregion
			
		
		}

        public void ShowPercentageOrNumberAllocatedStrategy(AllocationStrategies strategies, bool percentage)
		{

            try
            {
                ClearAllocatedFundsText();
                foreach (AllocationStrategy companyStrategy in strategies)
                {
                    int i = 0;
                    foreach (TextBox txtbx in txtUnAllocatedFunds)
                    {
                        if (txtbx.Name == companyStrategy.StrategyID.ToString())
                        {
                            if (percentage)
                            {
                                txtUnAllocatedFunds[i].Text = companyStrategy.Percentage.ToString();
                                rbtnPercentage.Checked = true;
                            }
                            else
                            {
                                txtUnAllocatedFunds[i].Text = companyStrategy.AllocatedQty.ToString();
                                rbtnNumber.Checked = true;
                            }
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
		}
	
		
		private void ClearAllocatedFundsText()
		{
            
			foreach(TextBox txtbx in txtUnAllocatedFunds )
				txtbx.Text="0";
		
		}
		private void ClearUnAllocatedFundsText()
		{
			foreach(TextBox txtbx in txtUnAllocatedFunds )
				txtbx.Text="0";
		
		}

		
		#endregion

        #region Getting Filtered Data
        private AllocationOrderCollection GetFilteredOrders()
        {
            UltraGridRow[] filteredRows = grdUnallocated.Rows.GetFilteredInNonGroupByRows();
            AllocationOrderCollection filteredOrders = new AllocationOrderCollection();
            foreach (UltraGridRow row in filteredRows)
            {
                AllocationOrder order = _orders.GetOrder(row.Cells["ClOrderID"].Value.ToString());
                filteredOrders.Add(order);

            }
            return filteredOrders;

        }
        #endregion

        #region Events

        public void AutoGroup()
        {
            bool bValidated = true;
            string AlreadyAllocatedOrdersString = string.Empty;
            try
            {

                AllocationOrderCollection filteredOrders = GetFilteredOrders();

                //IF no filtered AllocationOrder Return 
                if (filteredOrders.Count == 0)
                    return;
                //Activete Grouped Tab
                foreach (AllocationOrder aOrder in filteredOrders)
                {
                    if (aOrder.StateID.Equals(PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED))
                    {
                        AlreadyAllocatedOrdersString += "Order " + aOrder.Symbol
                                                      + " " + aOrder.OrderSide
                                                      + " " + aOrder.OrderType
                                                      + " " + aOrder.Quantity +
                                                      " " + "Already Grouped" + " on "
                                                      + aOrder.GroupAuecLocalDate.Month
                                                      + "/" + aOrder.GroupAuecLocalDate.Day
                                                      + "/" + aOrder.GroupAuecLocalDate.Year + "\n";

                        //+ " is gouped or allocated"
                        bValidated = false;
                    }
                }
                if (bValidated)
                {
                    _orderAllocationManager.AutoGroupOrders(filteredOrders, _allocationPreferences, string.Empty, AUECLocalDate);
                    ultraDockManager.DockAreas[1].Panes[AllocationConstants.SelectedTab.GroupedOrder.ToString()].Activate();
                    _selectedOrders = new AllocationOrderCollection();
                }
                else
                {
                    MessageBox.Show(AlreadyAllocatedOrdersString);
                }
            }

            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
            #endregion
        }
		private void cmbbxDefaults_ValueChanged(object sender, System.EventArgs e)
		{
			try
			{
				DefaultChanged();			
			}
				#region Catch
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
				
			}		
			#endregion
			

		}
		
		private void rbtnNumber_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				DefaultChanged();
			}
				#region Catch
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
				
			}		
			#endregion

		}
		private void DefaultChanged()
		{
         
			try
			{
                
				switch(_selectedTab)
				{
					case AllocationConstants.SelectedTab.UnAllocatedOrder :



                        if (grdUnallocated.ActiveRow == null)
                            return;
						if(rbtnNumber.Checked)
						{

                            string ClOrderID = grdUnallocated.ActiveRow.Cells["ClOrderID"].Value.ToString();
							AllocationOrder order= _orders.GetOrder(ClOrderID);	
							ShowNumberInUnAllocatedTextBoxes(order.CumQty);
				
						}
						else
						{
							ShowPercentagesInUnAllocatedTextBoxes();

						}
			

					

						break;
                    case AllocationConstants.SelectedTab.UnAllocatedBasket:



                        if (grdUnAllocatedBasket.ActiveRow == null)
                            return;
                        if (rbtnNumber.Checked)
                        {

                            string tradedbasketID = grdUnAllocatedBasket.ActiveRow.Cells["TradedBasketID"].Value.ToString();
                            Prana.BusinessObjects.BasketDetail  basket = _unAllocatedBaskets.GetBasket(tradedbasketID);
                            ShowNumberInUnAllocatedTextBoxes(basket.CumQty);

                        }
                        else
                        {
                            ShowPercentagesInUnAllocatedTextBoxes();

                        }




                        break;
					case AllocationConstants.SelectedTab.GroupedOrder :



                        if (grdGrouped.ActiveRow == null)
                            return;
						if(rbtnNumber.Checked)
						{

                            string GroupID = grdGrouped.ActiveRow.Cells["GroupID"].Value.ToString();
							AllocationGroup group= _groups.GetGroup(GroupID);		
							ShowNumberInUnAllocatedTextBoxes(group.CumQty);
				
						}
						else
						{
							ShowPercentagesInUnAllocatedTextBoxes();

						}
			
						break;
                    case AllocationConstants.SelectedTab.GroupedBasket:



                        if (grdGroupedBaskets.ActiveRow == null)
                            return;
                        if (rbtnNumber.Checked)
                        {

                            string GroupID = grdGroupedBaskets.ActiveRow.Cells["BasketGroupID"].Value.ToString();
                            BasketGroup group = _groupedBaskets.GetBasketGroup(GroupID);
                            
                            ShowNumberInUnAllocatedTextBoxes(group.CumQty);

                        }
                        else
                        {
                            ShowPercentagesInUnAllocatedTextBoxes();

                        }

                        break;
					case AllocationConstants.SelectedTab.AllocatedOrder :
                        if (grdAllocated.ActiveRow == null)
                            return;

                        string groupID = grdAllocated.ActiveRow.Cells["GroupID"].Value.ToString();
						AllocationGroup allocatedGroup= _allocatedGroups.GetGroup(groupID);				
			
						if(_formType==PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
						{					
							ShowPercentageOrNumberAllocatedFunds(allocatedGroup.AllocationFunds,rbtnPercentage.Checked);
						}
						else
						{
                            ShowPercentageOrNumberAllocatedStrategy(allocatedGroup.Strategies, rbtnPercentage.Checked);
						}
						break;
                    case AllocationConstants.SelectedTab.AllocatedBasket:
                        if (grdAllocatedBasket.ActiveRow == null)
                            return;

                        string groupBasketID = grdAllocatedBasket.ActiveRow.Cells["BasketGroupID"].Value.ToString();
                        BasketGroup basketGroup = _allocatedGroupedBaskets.GetBasketGroup(groupBasketID);

                        if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                        {
                            ShowPercentageOrNumberAllocatedFunds(basketGroup.AllocationFunds, rbtnPercentage.Checked);
                        }
                        else
                        {
                            ShowPercentageOrNumberAllocatedStrategy(basketGroup.Strategies, rbtnPercentage.Checked);
                        }
                        break;
				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
			}
		
		}

		private void grdUnallocated_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
		{
            try
            {

                DefaultChanged();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }		
		}

		private void grdGrouped_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
		{
            try
            {
                DefaultChanged();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }		
		}

		private void grdAllocated_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
		{
            try
            {
                DefaultChanged();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }		
		}


		private void menuOrders_Popup(object sender, System.EventArgs e)
		{
			//if fund strategy Bundling is Actived Give a PopUp Message
			if(_allocationPreferences.GeneralRules.IntegrateFundAndStrategyBundling && _formType==PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY)
			{
				mnuOrdersGroup.Visible=false;
				mnuOrdersAllocate.Visible=false;
				MessageBox.Show("User has selected for FundStrategy Bundling ");
				return;
			}

			if(_selectedOrders.Count ==0)
			{
				mnuOrdersAllocate.Visible=false;
				
			}
			else
			{
                if (_read_write == 1)
                {
                    mnuOrdersAllocate.Visible = true;
                    mnuOrdersAllocate.Enabled = true;
                }
                else
                {
                    mnuOrdersAllocate.Visible = true;
                    mnuOrdersAllocate.Enabled = false;
                }
				
			}
			if(_selectedOrders.Count < 2)
			{
				
				mnuOrdersGroup.Visible=false;
			}
			else
			{
                if (_read_write == 1)
                {
                    mnuOrdersGroup.Visible = true;
                    mnuOrdersGroup.Enabled = true;
                }
                else
                {
                    mnuOrdersGroup.Visible = true;
                    mnuOrdersGroup.Enabled = false;
                }
			}
		}

        private void menuGroups_Popup(object sender, System.EventArgs e)
        {
            bool isAnyManualGroup = false;
            //if fund strategy Bundling is Actived Give a PopUp Message
            if (_allocationPreferences.GeneralRules.IntegrateFundAndStrategyBundling && _formType == PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY)
            {
                mnuGroupsAllocate.Visible = false;
                mnuGroupsUngroup.Visible = false;
                MessageBox.Show(AllocationMessages.MSG_FUNDSTRATEGYBUNDLING);
                return;
            }
            if (_selectedGroups.Count == 0)
            {
                mnuGroupsAllocate.Visible = false;
                mnuGroupsUngroup.Visible = false;
            }
            else
            {
                if (_read_write == 1)
                {
                    foreach (AllocationGroup allocationGroup in _selectedGroups)
                    {
                        if (allocationGroup.IsManualGroup)
                        {
                            mnuGroupsAllocate.Visible = true;
                            mnuGroupsAllocate.Enabled = true;
                            mnuGroupsUngroup.Visible = false;
                            isAnyManualGroup = true;
                        }
                        else
                        {
                            if (isAnyManualGroup)
                            {
                                mnuGroupsAllocate.Visible = true;
                                mnuGroupsAllocate.Enabled = true;
                                mnuGroupsUngroup.Visible = false;
                            }
                            else
                            {
                                mnuGroupsAllocate.Visible = true;
                                mnuGroupsAllocate.Enabled = true;
                                mnuGroupsUngroup.Visible = true;
                                mnuGroupsUngroup.Enabled = true;
                            }
                        }

                    }
                }
                else
                {
                    mnuGroupsAllocate.Visible = true;
                    mnuGroupsAllocate.Enabled = false;
                    mnuGroupsUngroup.Visible = true;
                    mnuGroupsUngroup.Enabled = false;
                }

            }
        }

        private void mnuFunds_Popup(object sender, System.EventArgs e)
        {                     
            if (_read_write == 1)
            {
                foreach (AllocationGroup group in _selectedAllocatedGroups)
                {
                    if ((group.IsProrataActive) && (group.AllocatedQty < group.CumQty))
                    {
                        mnuFundsProrata.Visible = true;
                        mnuFundsProrata.Enabled = true;
                        mnuFundsUnAllocate.Visible = true;
                        mnuFundsUnAllocate.Enabled = true;
                    }
                    else
                    {
                        mnuFundsProrata.Visible = false;
                        break;
                    }
                }
            }
            else
            {
                foreach (AllocationGroup group in _selectedAllocatedGroups)
                {
                    if ((group.IsProrataActive) && (group.AllocatedQty < group.CumQty))
                    {
                        mnuFundsProrata.Visible = false;
                        mnuFundsProrata.Enabled = false;
                        mnuFundsUnAllocate.Visible = true;
                        mnuFundsUnAllocate.Enabled = false;
                    }
                    else if (group.IsProrataActive) 
                    {
                        mnuFundsProrata.Visible = false;
                        mnuFundsProrata.Enabled = false;
                        mnuFundsUnAllocate.Visible = true;
                        mnuFundsUnAllocate.Enabled = false;
                    }
                    else
                    {
                        mnuFundsProrata.Visible = false;
                        break;
                    }
                }
            }
        }
        
    

		
        
		
        //public void UpdateDataInGrids(AllocationOrderCollection latestOrders)
        //{
        //    try
        //    {
        //        BackgroundWorker backGroundWorkerBasketUpdate = new BackgroundWorker();
        //        backGroundWorkerBasketUpdate.DoWork += new DoWorkEventHandler(backGroundWorkerBasketUpdate_DoWork);
        //        backGroundWorkerBasketUpdate.RunWorkerAsync();

        //        SetRowColorForUpdated(grdUnallocated);
        //        SetRowColorForUpdated(grdGrouped);
        //        SetRowColorForUpdated(grdAllocated);

        //        SetRowColorForUpdated(grdUnAllocatedBasket);
        //        SetRowColorForUpdated(grdGroupedBaskets);
        //        SetRowColorForUpdated(grdAllocatedBasket);
			
        //    }
        //    catch(Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }

        //    }
			
        //}

        //void backGroundWorkerBasketUpdate_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    _basketAllocationManager.UpdateBaskets(AUECLocalDate);
            
        //}
		private void tabCtrlFunds_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
			if(e.Tab.Key.ToString() =="Allocated")
			{
				lblSelectFundDefault.Visible=false;
				cmbbxDefaults.Visible=false;
			}
			else
			{
				lblSelectFundDefault.Visible=true;
				cmbbxDefaults.Visible=true;
			}
		}

		private void ultraDockManager_BeforePaneButtonClick(object sender, Infragistics.Win.UltraWinDock.CancelablePaneButtonEventArgs e)
		{
			if(e.Button.ToString()=="Close")
			{
				e.Pane.Dock(true);
				e.Cancel = true;
			}
		
		}
		private void  OnAllocateClick()
		{
			ultraDockManager.DockAreas[0].Pin();

		}
        private void UpdateDefaultCombos()
        {
            
            if (this.cmbbxDefaults.InvokeRequired)
            {
                CrossThreadUICall d = new CrossThreadUICall(UpdateDefaultCombos);
                this.Invoke(d);


            }
            else
            {
                if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                {
                    BindFundDefaults();
                }
                else
                {
                    BindStrategyDefaults();

                }

            }
        }
		private void ultraDockManager_PaneActivate(object sender, Infragistics.Win.UltraWinDock.ControlPaneEventArgs e)
		{
		
			
                if(e.Pane.Key== AllocationConstants.SelectedTab.UnAllocatedOrder.ToString())
                {
				
					_selectedTab=AllocationConstants.SelectedTab.UnAllocatedOrder ;
					cmbbxDefaults.Visible=true;
					lblSelectFundDefault.Visible=true;
					lblAllocation.Visible=false;
                }
				
               else if(e.Pane.Key== AllocationConstants.SelectedTab.UnAllocatedBasket.ToString())
               {_selectedTab = AllocationConstants.SelectedTab.UnAllocatedBasket;
                    cmbbxDefaults.Visible = true;
                    lblSelectFundDefault.Visible = true;
                    lblAllocation.Visible = false;
               }
                
               else if(e.Pane.Key== AllocationConstants.SelectedTab.GroupedOrder.ToString() )
                {
						_selectedTab=AllocationConstants.SelectedTab.GroupedOrder ;
					cmbbxDefaults.Visible=true;
					lblSelectFundDefault.Visible=true;
					lblAllocation.Visible=false;
               }
                else if(e.Pane.Key== AllocationConstants.SelectedTab.GroupedBasket.ToString())
                {
                    _selectedTab = AllocationConstants.SelectedTab.GroupedBasket;
                    cmbbxDefaults.Visible = true;
                    lblSelectFundDefault.Visible = true;
                    lblAllocation.Visible = false;
                }
					
				else if(e.Pane.Key== AllocationConstants.SelectedTab.AllocatedOrder.ToString())
                {
						_selectedTab=AllocationConstants.SelectedTab.AllocatedOrder ;
					cmbbxDefaults.Visible=false;
					lblSelectFundDefault.Visible=false;
					lblAllocation.Visible=true;
				}
                
               
               else if(e.Pane.Key== AllocationConstants.SelectedTab.AllocatedBasket.ToString())
                {
                    _selectedTab = AllocationConstants.SelectedTab.AllocatedBasket;
                    cmbbxDefaults.Visible = false;
                    lblSelectFundDefault.Visible = false;
                    lblAllocation.Visible = true;
               }
				
			
		}

		#region Header Clicked Events
		private void headerCheckBoxUnallocated__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
		{
			
			try
			{
				if(e.CurrentCheckState==CheckState.Checked)
				{
					UltraGridRow[]  filteredRows= grdUnallocated.Rows.GetFilteredInNonGroupByRows() ;
					
					_selectedOrders= new AllocationOrderCollection();
					foreach(UltraGridRow row in filteredRows)
					{
                        AllocationOrder order = (AllocationOrder)row.ListObject;
						_selectedOrders.Add(order);
                        order.Updated = false ;
					}
					
					
					
			     
					int RowCount=grdUnallocated.Rows.Count;
					for(int rowIndex=0;rowIndex<RowCount ;rowIndex++)
					{
						grdUnallocated.Rows[rowIndex].Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
						grdUnallocated.Rows[rowIndex].Appearance.BackColor2 =Color.FromArgb( _allocationPreferences.RowProperties.SelectedRowBackColor);
						grdUnallocated.Rows[rowIndex].Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
					}
					
					
					
				}
				else
				{
					_selectedOrders=new AllocationOrderCollection();
					ApplyRowColorUnAllocated();
					

				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
				

			}


		}

		private void headerCheckBoxGrouped__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
		{
			try
			{
				if(e.CurrentCheckState==CheckState.Checked)
				{
					_selectedGroups.Clear();
					foreach(AllocationGroup  group  in _groups)
					{
                        group.Updated = false;
						_selectedGroups.Add(group);
					}
					int RowCount=grdGrouped.Rows.Count;
					for(int rowIndex=0;rowIndex<RowCount ;rowIndex++)
					{
						grdGrouped.Rows[rowIndex].Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
						grdGrouped.Rows[rowIndex].Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
						grdGrouped.Rows[rowIndex].Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
					}
					
				}
				else
				{
					_selectedGroups.Clear();
					ApplyRowColorGrouped();
				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
				

			}

		}

		private void headerCheckBoxAllocated__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
		{
		
			try
			{
				if(e.CurrentCheckState==CheckState.Checked)
				{
					_selectedAllocatedGroups.Clear();
					foreach(AllocationGroup   groupEntity  in _allocatedGroups)
					{
                        groupEntity.Updated = false;
						_selectedAllocatedGroups.Add(groupEntity);
					}
					int RowCount=grdAllocated.Rows.Count;
					for(int rowIndex=0;rowIndex<RowCount ;rowIndex++)
					{
						grdAllocated.Rows[rowIndex].Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
						grdAllocated.Rows[rowIndex].Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
						grdAllocated.Rows[rowIndex].Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
					}
				}
				else
				{
					_selectedAllocatedGroups.Clear();
					ApplyRowColorAllocated();

				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
				

			}

		}


        void headerCheckBoxAllocatedBasket__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {
            try
            {

                if (e.CurrentCheckState == CheckState.Checked)
                {
                    UltraGridRow[] filteredRows = grdAllocatedBasket.Rows.GetFilteredInNonGroupByRows();

                    _selectedAllocatedBaskets = new BasketGroupCollection();
                    foreach (UltraGridRow row in filteredRows)
                    {
                        BasketGroup basketGroup = _allocatedGroupedBaskets.GetBasketGroup(row.Cells["BasketGroupID"].Value.ToString());
                        row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
                        row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
                        row.Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
                        _selectedAllocatedBaskets.Add(basketGroup);

                    }
                }
                else
                {
                    _selectedAllocatedBaskets = new BasketGroupCollection();
                    ApplyRowColorBaskets(grdAllocatedBasket);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }


            }
        }

        void headerCheckBoxGroupedBasket__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {
            try
            {

                if (e.CurrentCheckState == CheckState.Checked)
                {
                    UltraGridRow[] filteredRows = grdGroupedBaskets.Rows.GetFilteredInNonGroupByRows();

                    _selectedGropuedBaskets = new BasketGroupCollection();
                    foreach (UltraGridRow row in filteredRows)
                    {
                        
                        BasketGroup basketGroup = _groupedBaskets.GetBasketGroup(row.Cells["BasketGroupID"].Value.ToString());
                        row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
                        row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
                        row.Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
                        _selectedGropuedBaskets.Add(basketGroup);
                    }
                }
                else
                {
                    _selectedGropuedBaskets = new BasketGroupCollection();
                    ApplyRowColorBaskets(grdGroupedBaskets);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }


            }
        }

        void headerCheckBoxUnAllocatedBasket__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        {
            try
            {

                if (e.CurrentCheckState == CheckState.Checked)
                {
                    UltraGridRow[] filteredRows = grdUnAllocatedBasket.Rows.GetFilteredInNonGroupByRows();

                    _selectedBaskets = new Prana.BusinessObjects.BasketCollection();
                    foreach (UltraGridRow row in filteredRows)
                    {
                        Prana.BusinessObjects.BasketDetail basket = _unAllocatedBaskets.GetBasket(row.Cells["TradedBasketID"].Value.ToString());
                        row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
                        row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
                        row.Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
                        _selectedBaskets.Add(basket);

                    }
                }
                else
                {
                    _selectedBaskets = new Prana.BusinessObjects.BasketCollection();
                    ApplyRowColorBaskets(grdUnAllocatedBasket);


                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }


            }
        }
		#endregion

		#region Checking of CheckBox on Grid
		
		private void grdGrouped_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			bool isRowSelected=false;
			
			if (e.Button.ToString() =="Right")
				return;
			UIElement objUIElement;
			UltraGridCell objUltraGridCell;		
			objUIElement = grdGrouped.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
			if(objUIElement == null)
				return;
			objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
			if(objUltraGridCell == null)
				return;
			
			
			if((objUltraGridCell.Value.ToString()=="True" || objUltraGridCell.Value.ToString()=="False"))
			{
				if(objUltraGridCell.Row.Cells["checkBox"].Value.ToString().Equals("true",StringComparison.OrdinalIgnoreCase))
				{
					objUltraGridCell.Row.Cells["checkBox"].Value=false;
					isRowSelected=false;
				
				}
				else
				{
					objUltraGridCell.Row.Cells["checkBox"].Value=true;
					isRowSelected=true;
					
				}

			}
			else
				return;
			
			//Checking The Current Status And Changing it 
			
			//Make A group an add it to Collection
            AllocationGroup group = (AllocationGroup)objUltraGridCell.Row.ListObject;
            group.Updated = false;
			//if row is seleted
			#region Apply Colors and Add AllocationGroup
			if(isRowSelected)
			{
				//if(isRowChecked)
				//{
						_selectedGroups.Add(group);
				//}
				objUltraGridCell.Row.Appearance.BackColor=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
				objUltraGridCell.Row.Appearance.BackColor2=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
				objUltraGridCell.Row.Appearance.ForeColor =Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
				

			}
				//if Unselected
			else
			{
				//if(!isRowChecked)
				//{
					_selectedGroups.Remove(group);
				//}
				Int32 exeQty= Convert.ToInt32(objUltraGridCell.Row.Cells["CumQty"].Value);
				Int32 totalQty= Convert.ToInt32(objUltraGridCell.Row.Cells["Quantity"].Value);
				
				//Reverting to Preferences Colors 
				if(exeQty <totalQty)
				{
			
					objUltraGridCell.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor)  ;
					objUltraGridCell.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyTextColor)  ;
					objUltraGridCell.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor)  ;
					objUltraGridCell.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
			
				}
				else
				{
					objUltraGridCell.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
					objUltraGridCell.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedTextColor)  ;
					objUltraGridCell.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
					objUltraGridCell.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
				}
			}
			#endregion
			

		}

		private void grdAllocated_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			OnAllocateClick();
			bool isRowSelected=false;
			if (e.Button.ToString() =="Right")
				return;
			
			UIElement objUIElement;
			UltraGridCell objUltraGridCell;		
			objUIElement = grdAllocated.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
			if(objUIElement == null)
				return;
			objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
			if(objUltraGridCell == null)
				return;
			
			if((objUltraGridCell.Value.ToString()=="True" || objUltraGridCell.Value.ToString()=="False"))
			{
				if(objUltraGridCell.Row.Cells["checkBox"].Value.ToString().Equals("true",StringComparison.OrdinalIgnoreCase))
				{
					objUltraGridCell.Row.Cells["checkBox"].Value=false;
					isRowSelected=false;
				
				}
				else
				{
					objUltraGridCell.Row.Cells["checkBox"].Value=true;
					isRowSelected=true;
					
				}

			}
			else
				return;

            //AllocationOrder order = (AllocationOrder)objUltraGridCell.Row.ListObject;

            AllocationGroup groupEntity = (AllocationGroup)objUltraGridCell.Row.ListObject;
            groupEntity.Updated = false;
				if(isRowSelected)
					
				{
					_selectedAllocatedGroups.Add(groupEntity);
					objUltraGridCell.Row.Appearance.BackColor=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
					objUltraGridCell.Row.Appearance.BackColor2=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
					objUltraGridCell.Row.Appearance.ForeColor =Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);

				}
				else
				{
					_selectedAllocatedGroups.Remove(groupEntity);
					Int32 allocatedQty= Convert.ToInt32(objUltraGridCell.Row.Cells["AllocatedQty"].Value);
					Int32 totalQty= Convert.ToInt32(objUltraGridCell.Row.Cells["Quantity"].Value);
					if(allocatedQty < totalQty)
					{
					
						objUltraGridCell.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyBackColor )  ;
						objUltraGridCell.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyTextColor)  ;
						objUltraGridCell.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyBackColor)  ;
						objUltraGridCell.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
					
					}
					else
					{
						objUltraGridCell.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyBackColor )  ;
						objUltraGridCell.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyTextColor)  ;
						objUltraGridCell.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyBackColor)  ;
						objUltraGridCell.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
					}
				}
		

	}

		private void grdUnallocated_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			bool isRowSelected=false;
			if (e.Button.ToString() =="Right")
				return;
			UIElement objUIElement;
			UltraGridCell objUltraGridCell;		
			objUIElement = grdUnallocated.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
			if(objUIElement == null)
				return;
			objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
			if(objUltraGridCell == null)
				return;
			if((objUltraGridCell.Value.ToString()=="True" || objUltraGridCell.Value.ToString()=="False"))
			{
                if (objUltraGridCell.Row.Cells["checkBox"].Value.ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					objUltraGridCell.Row.Cells["checkBox"].Value=false;
					isRowSelected=false;
				
				}
				else
				{
					objUltraGridCell.Row.Cells["checkBox"].Value=true;
					isRowSelected=true;
					
				}

			}
			else
				return;

			AllocationOrder order=  (AllocationOrder)objUltraGridCell.Row.ListObject;
            order.Updated = false;
			if(isRowSelected)
			{
				_selectedOrders.Add(order);
				
				
 
				objUltraGridCell.Row.Appearance.BackColor=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
				objUltraGridCell.Row.Appearance.BackColor2=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
				objUltraGridCell.Row.Appearance.ForeColor=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
			}
			else
			{
				_selectedOrders.Remove(order);
				Int32 exeQty= Convert.ToInt32(objUltraGridCell.Row.Cells["CumQty"].Value);
				Int32 totalQty= Convert.ToInt32(objUltraGridCell.Row.Cells["Quantity"].Value);
				if(exeQty <totalQty)
				{
				
					objUltraGridCell.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor)  ;
					objUltraGridCell.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyTextColor)  ;
					objUltraGridCell.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor)  ;
					objUltraGridCell.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
				
				}
				else
				{
					objUltraGridCell.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
					objUltraGridCell.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedTextColor)  ;
					objUltraGridCell.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
					objUltraGridCell.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
				}
			}

			
		
		}
		#endregion
			#endregion	

		#region Grouping Rules
		
		
        //private void AddToGroup()
        //{
        //    int RowCount=grdUnallocated.Rows.Count;
        //    foreach(AllocationOrder    order in _selectedOrders)
        //    {
				
        //        _orders.Remove(order);
				
        //    }
			
        //    AllocationOrderCollection  copiedGroupedOrders=new AllocationOrderCollection(); 
        //    copiedGroupedOrders.AddOrders(_selectedOrders);
        //    AllocationGroup group= new AllocationGroup(copiedGroupedOrders);
        //    _groups.Add(group);			
        //    _selectedOrders.Clear();
			
	
        //}
		
		#endregion
		
		#region   Binding Grid
		private  void OrderBind(AllocationOrderCollection orders)
		{
            //grdUnallocated.DataSource = null;
            grdUnallocated.DataSource = orders;
            grdUnallocated.DataBind();
            
            //AddCheckBoxinGrid(grdUnallocated, headerCheckBoxUnallocated);
            //HideUnAllocatedGridColumns();

		}
		private  void GroupBind(AllocationGroups groups)
		{
            //grdGrouped.DataSource = null;
            grdGrouped.DataSource = groups;				
			grdGrouped.DataBind();
            //AddCheckBoxinGrid(grdGrouped, headerCheckBoxGrouped);
            //HideGroupedGridColumns();
			
		}
        private void FundsBind(AllocationGroups groups)
		{
            //grdAllocated.DataSource = null;
            grdAllocated.DataSource = groups;				
			grdAllocated.DataBind();
            grdAllocated.Refresh();
            //AddCheckBoxinGrid(grdAllocated, headerCheckBoxAllocated);
            //HideAllocatedGridColumns();
			
		}
        private void BindUnAllocatedBaskets(Prana.BusinessObjects.BasketCollection baskets)
        {
            //grdUnAllocatedBasket.DataSource = null;
            grdUnAllocatedBasket.DataSource = baskets;
            grdUnAllocatedBasket.DataBind();
            //AddCheckBoxinGrid(grdUnAllocatedBasket, headerCheckBoxUnAllocatedBasket);
            //HideBasketColumns();
        }
        private void BindGroupedBaskets(BasketGroupCollection basketGroups)
        {
            //grdGroupedBaskets.DataSource = null;
            grdGroupedBaskets.DataSource = basketGroups;
            grdGroupedBaskets.DataBind();
            //AddCheckBoxinGrid(grdGroupedBaskets, headerCheckBoxGroupedBasket);
            //HideBasketColumns();

        }
        private void BindAllocatedBaskets(BasketGroupCollection basketGroups)
        {
            //grdAllocatedBasket.DataSource = null;
            grdAllocatedBasket.DataSource = basketGroups;
            grdAllocatedBasket.DataBind();
            //AddCheckBoxinGrid(grdAllocatedBasket, headerCheckBoxAllocatedBasket);
            //HideBasketColumns();
        }
        #endregion

        #region Binding Default
        private void  BindFundDefaults()
		{
			//TBC
			try
			{
				Defaults defaults= FundManager.GetFundDefaults(_loginUser.CompanyUserID);
                defaults.Insert(0, new Default(ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
				cmbbxDefaults.DisplayMember ="DefaultName";
				cmbbxDefaults.ValueMember="DefaultID";
                cmbbxDefaults.DataSource = null;
				cmbbxDefaults.DataSource=defaults;		
				cmbbxDefaults.DataBind();
                cmbbxDefaults.Value = ApplicationConstants.C_COMBO_SELECT;
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
			}
		}
		private void  BindStrategyDefaults()
		{
			//TBC
			try
			{
				Defaults defaults= CompanyStrategyManager.GetStrategyDefaults(_loginUser.CompanyUserID);
                defaults.Insert(0, new Default(ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));	
				cmbbxDefaults.DisplayMember ="DefaultName";
				cmbbxDefaults.ValueMember="DefaultID";
                cmbbxDefaults.DataSource = null;
				cmbbxDefaults.DataSource=defaults;	
				cmbbxDefaults.DataBind();
                cmbbxDefaults.Value = ApplicationConstants.C_COMBO_SELECT;
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
			}
		}
		#endregion

        #region Grid Initilization 

        private void ApplyGridSetting()
        {
            try
            {

                SetFormatForFields();
                SetFieldsNotEditable();
                if (!grdUnallocated.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdUnallocated, headerCheckBoxUnallocated);
                }
                if (!grdGrouped.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdGrouped, headerCheckBoxGrouped);
                }
                if (!grdAllocated.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdAllocated, headerCheckBoxAllocated);
                }
                if (!grdUnAllocatedBasket.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdUnAllocatedBasket, headerCheckBoxUnAllocatedBasket);
                }
                if (!grdGroupedBaskets.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdGroupedBaskets, headerCheckBoxGroupedBasket);
                }
                if (!grdAllocatedBasket.DisplayLayout.Bands[0].Columns.Exists("checkBox"))
                {
                    AddCheckBoxinGrid(grdAllocatedBasket, headerCheckBoxAllocatedBasket);
                }

                ////UnCheckAllGroupBoxes(grdUnallocated);
                // UnCheckAllGroupBoxes(grdGrouped);
                // UnCheckAllGroupBoxes(grdAllocated);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void UnCheckAllGroupBoxes(UltraGrid grid)
        {
            RowsCollection rows = grid.Rows;
            foreach (UltraGridRow row in rows)
            {
                row.Cells["checkBox"].Value = false;
            }

        }

        private void AddCheckBoxinGrid(UltraGrid grid, CheckBoxOnHeader_CreationFilter headerCheckBox)
        {
            grid.CreationFilter = headerCheckBox;
            grid.DisplayLayout.Bands[0].Columns.Add("checkBox", "");
            grid.DisplayLayout.Bands[0].Columns["checkBox"].DataType = typeof(bool);


        }

        private void SetFormatForFields()
        {
            SetColumnDataFormat(grdUnallocated, "AvgPrice", "F2");
            SetColumnDataFormat(grdGrouped, "AvgPrice", "F2");
            SetColumnDataFormat(grdAllocated, "AvgPrice", "F2");

        }
        private void SetFieldsNotEditable()
        {
            SetColumnsNonEditable(grdUnallocated);
            SetColumnsNonEditable(grdGrouped);
            SetColumnsNonEditable(grdAllocated);

        }

        private void SetColumnsNonEditable(UltraGrid grid)
        {
            try
            {
                //UltraGrid Grid = ((UltraGrid) grid);
                ColumnsCollection columns = grid.DisplayLayout.Bands[0].Columns;
                columns["AvgPrice"].Header.Caption = "AvgPX";
                
                
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "checkBox")
                    {
                        column.CellActivation = Activation.NoEdit;

                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetColumnDataFormat(UltraGrid grid, string columnName, string format)
        {

            if (grid.DisplayLayout.Bands[0].Columns.Exists(columnName))
                grid.DisplayLayout.Bands[0].Columns["AvgPrice"].Format = format;


        }


        private void grdUnallocated_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{

            grdUnallocated.CreationFilter = headerCheckBoxUnallocated;
			
		
		}
		private void grdGrouped_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
            
          //  grdGrouped.CreationFilter = headerCheckBoxGrouped;
		}
		private void grdAllocated_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
            
           // grdAllocated.CreationFilter = headerCheckBoxAllocated;
        }
        #endregion

        #region DataGrid Bands Setting

      
		#endregion

		#region Utility
		
		private double  SumOfSharesInTextBoxes()
		{
            double Sum = 0;
			foreach(TextBox  txtbx in txtUnAllocatedFunds)
			{
				if(txtbx.Text.Trim().Equals(string.Empty))
					txtbx.Text="0";
                Sum = Sum + double.Parse(txtbx.Text);
			
			
			}
			return Sum;
		
		}
		
		private bool CheckSumOfPercentage()
		{
			float percentage=0;
			for(int i=0;i<txtUnAllocatedFunds.Length;i++)
			{
				
				if(txtUnAllocatedFunds[i].Text.Trim().Equals(string.Empty))
				{
					txtUnAllocatedFunds[i].Text="0";
				}
				else 
				{
					percentage=percentage+float.Parse(txtUnAllocatedFunds[i].Text);
				}
			}
            if (percentage != 100.0)
            {
                MessageBox.Show("Sum of percentages should be 100 % ");
                return false;
            }
            else
            {
                return true;
            }
			
		}

		private bool CheckEntiresInTextBoxes()
		{
			if(isSingleFundPermitted)
				return true;


			if(rbtnNumber.Checked)
			{
				foreach(TextBox txtbx in txtUnAllocatedFunds)
			
				{
                    if (txtbx.Text.Trim().Equals(string.Empty))
                        txtbx.Text = "0";

					errorProviderFunds.SetError(txtbx,"");
					if(!RegularExpressionValidation.IsPositiveInteger(txtbx.Text))
					{
						errorProviderFunds.SetError(txtbx,"Please Enter a Positive Number");
						return false;
					}

				}
			}
			else
			{
                

				foreach(TextBox txtbx in txtUnAllocatedFunds)
			
				{
                    if (txtbx.Text.Trim().Equals(string.Empty))
                        txtbx.Text = "0";
					errorProviderFunds.SetError(txtbx,"");
					if(!RegularExpressionValidation.IsPositiveNumber(txtbx.Text))
					{
						errorProviderFunds.SetError(txtbx,"Please Enter a Positive Number");
						return false;
					}

				}


			}
			return true;
		}
		private void AdjustShares(Int64  sum1,double   sum2)
		{
			if(txtUnAllocatedFunds[0] !=null)
			{
                double newValue = (sum2 - sum1) + Convert.ToInt64(txtUnAllocatedFunds[0].Text);			
				txtUnAllocatedFunds[0].Text=newValue.ToString();
			}
			
		}

		public  void  ClearOrderCollection() 
		{
            //while (_orders.Count > 0)
            //{
                _orders.Clear();
            //}
           
            //while(_orders.Count>0)
            //{
            //    _orders.RemoveAt(0);
            //}
			
				
		}


		#endregion

		#region Preferences

		private  void SetPreferences()
		{
            try
            {
                HideColumns();
                SetSortKey();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
		}

       
     
		private void SetSortKey()
		{
            string unAllocatedSortKey;
            bool unAllocateddescending;
            string allocatedSortKey;
            bool allocatedDescending;
            string groupedSortKey;
            bool groupedDescending;
            if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
            {
                unAllocatedSortKey = _allocationPreferences.FundType.UnAllocatedGridColumns.SortKey;
                unAllocateddescending = !_allocationPreferences.FundType.UnAllocatedGridColumns.Ascending;

                allocatedSortKey = _allocationPreferences.FundType.AllocatedGridColumns.SortKey;
                allocatedDescending = !_allocationPreferences.FundType.AllocatedGridColumns.Ascending;

                groupedSortKey = _allocationPreferences.FundType.GroupedGridColumns.SortKey;
                groupedDescending = !_allocationPreferences.FundType.GroupedGridColumns.Ascending;

            }
            else
            {
                unAllocatedSortKey = _allocationPreferences.StrategyType.UnAllocatedGridColumns.SortKey;
                unAllocateddescending = !_allocationPreferences.StrategyType.UnAllocatedGridColumns.Ascending;

                allocatedSortKey = _allocationPreferences.StrategyType.AllocatedGridColumns.SortKey;
                allocatedDescending = !_allocationPreferences.StrategyType.AllocatedGridColumns.Ascending;

                groupedSortKey = _allocationPreferences.StrategyType.GroupedGridColumns.SortKey;
                groupedDescending = !_allocationPreferences.StrategyType.GroupedGridColumns.Ascending;
            }
			
           

		
			
			try
			{
				if(unAllocatedSortKey !=string.Empty)
				{
                    if (grdUnallocated.DisplayLayout.Bands[0].Columns.Exists(unAllocatedSortKey))
                    {
                        grdUnallocated.DisplayLayout.Bands[0].SortedColumns.Clear();
                        grdUnallocated.DisplayLayout.Bands[0].SortedColumns.Add(unAllocatedSortKey, unAllocateddescending);
                    }
				}
				if(groupedSortKey !=string.Empty)
				{
                    if (grdGrouped.DisplayLayout.Bands[0].Columns.Exists(groupedSortKey))
                    {
                        grdGrouped.DisplayLayout.Bands[0].SortedColumns.Clear();
                        grdGrouped.DisplayLayout.Bands[0].SortedColumns.Add(groupedSortKey, groupedDescending);
                    }
				}
				if(allocatedSortKey !=string.Empty)
				{
                    if (grdAllocated.DisplayLayout.Bands[0].Columns.Exists(allocatedSortKey))
                    {
                        grdAllocated.DisplayLayout.Bands[0].SortedColumns.Clear();
                        grdAllocated.DisplayLayout.Bands[0].SortedColumns.Add(allocatedSortKey, allocatedDescending);
                    }
				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
			}
		}
		
		public void UpdatePreferences()
		{
			try
			{
                HideColumns();
				//ApplyRowColorUnAllocated();
				//ApplyRowColorGrouped();
				//ApplyRowColorAllocated();
                UpdateDefaultCombos();
				SetSortKey();
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                
			}
	
		}
        public void SetAllocationPreferences(AllocationPreferences newPreferences)
        {
            _allocationPreferences = newPreferences;
        }
		#endregion

		#region Checking Where the AllocationOrder is present , in case of Update 
        //private void  CheckOrdersInEntities(AllocationOrderCollection latestorders,AllocationGroups  groups)
        //{
        //    try
        //    {
        //        AllocationOrderCollection foundOrders= new AllocationOrderCollection();
			
        //        #region CHeck In AllocationGroup

        //        foreach (AllocationGroup groupedEntity in groupedEntities)
        //        {
        //            AllocationGroup group = groupedEntity.GetGroup();
        //            groupedEntity.Updated=false;
				
        //            #region if the saved group consist Allocated AllocationOrderCollection 
        //            if(group.GroupID==string.Empty)
        //            {
        //                foreach(AllocationOrder    latestOrder in latestorders)
        //                {
                            
        //                    if(groupedEntity.ClOrderID ==latestOrder.ClOrderID)   
        //                    {
                                
        //                            if (latestOrder.CumQty > groupedEntity.CumQty)
        //                            {

        //                                groupedEntity.Updated = true;

        //                            }
        //                            groupedEntity.SetEntityValues(latestOrder);
        //                            if (latestOrder.FundID != int.MinValue && _formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
        //                            {
        //                                groupedEntity.ReAllocatePreAllocatedOrderToFund(latestOrder);
        //                            }
        //                            if (latestOrder.StrategyID != int.MinValue && _formType == PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY)
        //                            {
        //                                groupedEntity.ReAllocatePreAllocatedOrderToStrategy(latestOrder);
        //                            }

        //                            foundOrders.Add(latestOrder);
        //                            break;
                                
								
        //                    }
        //                }
					
        //            }
        //                #endregion
				
        //            else
        //            {
					
        //                #region if the saved group consist Allocated AllocationGroups 
        //                foreach(AllocationOrder    order in group.orders)
        //                {
        //                    foreach(AllocationOrder    latestorder in latestorders)
        //                    {
        //                        if(order.ClOrderID.Equals(latestorder.ClOrderID))
        //                        {
        //                            foundOrders.Add(latestorder);
									
        //                            if(latestorder.CumQty>order.CumQty)
        //                            {
										
        //                                groupedEntity.Updated=true;
														
        //                            }
        //                            order.UpdateFills(latestorder);
								
        //                            break;
							
        //                        }
						
        //                    }
						
        //                }
        //                #endregion
					
        //                groupedEntity.SetEntityValues();
        //                group.SetGroupDetails();
        //            }	
					
			
        //        }
			
			
        //        #endregion
        //        foreach(AllocationOrder    order in foundOrders)
        //        {
        //            latestorders.Remove(order);
				
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
		
        //}
		
        //private void CheckOrdersInGroups(AllocationOrderCollection latestorders,AllocationGroups groups)
        //{
        //    AllocationOrderCollection foundOrders= new AllocationOrderCollection();
        //    #region Check In Grouped AllocationOrderCollection 
        //    foreach(AllocationOrder  latestorder in latestorders)
        //    {
        //        foreach (AllocationGroup group in groups)
        //        {		
        //            group.Updated=false;		
        //            foreach(AllocationOrder  order in group.orders)
        //            {
						

        //                if(order.ClOrderID.Equals(latestorder.ClOrderID))
        //                {
							
        //                    if(latestorder.CumQty>order.CumQty)
        //                    {
														
        //                        group.Updated=true;								
								
        //                    }	
        //                    else
        //                    {
								
        //                    }
        //                    order.UpdateFills(latestorder);
        //                    foundOrders.Add(latestorder);
							
        //                    break;

						
        //                }
						

        //            }
        //            group.SetGroupDetails();
        //        }
        //    }
				
			
        //    #endregion
        //    foreach(AllocationOrder    order in foundOrders)
        //    {
        //        latestorders.Remove(order);
				
        //    }
		
        //}
        //private void CheckOrdersInOldOrders(AllocationOrderCollection latestorders)
        //{
        //    AllocationOrderCollection foundOrders= new AllocationOrderCollection();
			
        //    #region Check in AllocationOrderCollection 
        //    foreach(AllocationOrder    latestorder in latestorders)
        //    {	
        //        foreach(AllocationOrder    order in _orders)
        //        {
        //            order.Updated=false;
        //            if(order.ClOrderID.Equals(latestorder.ClOrderID))
        //            {
						
        //                if(latestorder.CumQty>order.CumQty)
        //                {
        //                    order.Updated=true;
        //                    order.CumQty=latestorder.CumQty;
        //                    order.AvgPrice=latestorder.AvgPrice;
        //                    order.Quantity=latestorder.Quantity;
        //                }
						
        //                foundOrders.Add(latestorder);
						
        //                break;
						
						
							
        //            }
					
        //        }
        //    }
			
        //    #endregion
        //    foreach(AllocationOrder    order in foundOrders)
        //        latestorders.Remove(order);
		
		
        //}
       
	
		#endregion
		
		#region Binding Filters
		private void BindAsset()
		{


            cmbbxAsset.DataSource = null;
			cmbbxAsset.DataSource = assets;
			cmbbxAsset.DisplayMember = "Name";
			cmbbxAsset.ValueMember = "AssetID";
			cmbbxAsset.Value=int.MinValue;
			ColumnsCollection columns = cmbbxAsset.DisplayLayout.Bands[0].Columns;
			foreach (UltraGridColumn column in columns)
			{
				
				if(column.Key != "Name")
				{
					column.Hidden = true;
				}
			}
		}
		public void BindUndeyLying(int assetID)
		{
			
				
			UnderLyings  underLyings = new UnderLyings();
            if (assetID != int.MinValue)
            {
                underLyings = ClientsCommonDataManager.GetUnderLyingsByAssetAndUserID(_loginUser.CompanyUserID, assetID);
            }
            underLyings.Insert(0, new UnderLying(int.MinValue, ApplicationConstants.C_COMBO_ALL));
            cmbbxUnderlying.DataSource = null;
			cmbbxUnderlying.DataSource = underLyings;
			cmbbxUnderlying.DisplayMember = "Name";
			cmbbxUnderlying.ValueMember = "UnderLyingID";
			cmbbxUnderlying.Value=int.MinValue;
			ColumnsCollection columns = cmbbxUnderlying.DisplayLayout.Bands[0].Columns;
			foreach (UltraGridColumn column in columns)
			{
				 
				if(column.Key != "Name")
				{
					column.Hidden = true;
				}
			}
			

		}
		public void BindExchange()
		{
			//	cmbbxAsset.DataSource= AssetManager.
            try
            {
                cmbbxExchange.DataSource = null;
			cmbbxExchange.DataSource = exchanges;
                cmbbxExchange.DisplayMember = "Name";
			cmbbxExchange.ValueMember = "ExchangeID";
			cmbbxExchange.Value=int.MinValue;
			ColumnsCollection columns = cmbbxExchange.DisplayLayout.Bands[0].Columns;
			foreach (UltraGridColumn column in columns)
			{
				 
                    if (column.Key != "Name")
				{
					column.Hidden = true;
				}
			}
		}
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
		}
        public void BindCounterParty()
        {
            //	cmbbxAsset.DataSource= AssetManager.
            try
            {
                cmbCounterParty.DataSource = null;
                cmbCounterParty.DataSource = counterParties;
                cmbCounterParty.DisplayMember = "Name";
                cmbCounterParty.ValueMember = "CounterPartyID";
                cmbCounterParty.Value = int.MinValue;
                ColumnsCollection columns = cmbCounterParty.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {

                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
        }
        public void BindVenue(int counterPartyID)
        {
            //	cmbbxAsset.DataSource= AssetManager.
            try
            {
                VenueCollection venues = new VenueCollection();
                if(counterPartyID!=int.MinValue)
                {
                    venues = ClientsCommonDataManager.GetVenues(_loginUser.CompanyUserID, counterPartyID);
                }
                venues.Insert(0, new Venue(int.MinValue, ApplicationConstants.C_COMBO_ALL));
                cmbVenue.DataSource = null;
                cmbVenue.DataSource = venues;
                cmbVenue.DisplayMember = "Name";
                cmbVenue.ValueMember = "VenueID";
                cmbVenue.Value = int.MinValue;
                ColumnsCollection columns = cmbVenue.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {

                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
        }
        public  void BindOrderSide()
        {
            try
            {
                Sides orderSides = new Sides();
                //orderSides = ClientsCommonDataManager.GetOrderSidesByCVAUEC(int.Parse(cmbAsset.Value.ToString()), int.Parse(cmbUnderLying.Value.ToString()), int.Parse(cmbCounterParty.Value.ToString()), int.Parse(cmbVenue.Value.ToString()));
                orderSides = ClientsCommonDataManager.GetSides();// GetOrderTypesByAUCVID(assetID,underlyingID,counterPartyID, venueID);
                orderSides.Insert(0, new Side(int.MinValue, ApplicationConstants.C_COMBO_ALL, int.MinValue.ToString()));
                cmbOrderSide.DataSource = null;
                cmbOrderSide.DataSource = orderSides;
                cmbOrderSide.DisplayMember = "Name";
                // valuemember changed to tagvalue as we are identifying sides by that value across the application
                // wherever we are using the filter we can check for cmbside.value to fixconstants.sides...
                cmbOrderSide.ValueMember = "TagValue";
                cmbOrderSide.Value = int.MinValue.ToString();


                ColumnsCollection columns = cmbOrderSide.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {

                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        } 

		#endregion
		
		#region Allocation

		private void OrderAllocationToFund(DateTime AUECLocalDate)
		{
			try
			{
				if(! AreOrdersElligibleToAllocation())
					return;
                AllocationFunds funds = null;
				if(rbtnNumber.Checked)
				{
                    double allocatedQty = SumOfSharesInTextBoxes();
                    funds = GetEntityFundsForShares();
                    _orderAllocationManager.AllocateOrder(_selectedOrders[0], allocatedQty, funds, false, string.Empty, AUECLocalDate);
				}		
				else
                {
                    foreach (AllocationOrder order in _selectedOrders)
                    {
                        funds = GetEntityFundsForPercentage(order.CumQty);
                        _orderAllocationManager.AllocateOrder(order, order.CumQty, funds, true, string.Empty, AUECLocalDate);
                    }
				}
				_selectedOrders= new AllocationOrderCollection();
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
			}
		}
        private void OrderAllocationToStrategy(DateTime AUECLocalDate)
		{
			try
			{
				if(! AreOrdersElligibleToAllocation())
					return;

                AllocationStrategies strategies=null;
				
				if(rbtnNumber.Checked)
				{
					double allocatedQty=SumOfSharesInTextBoxes();
                    strategies = GetEntityStrategiesForShares();
                    _orderAllocationManager.AllocateOrder(_selectedOrders[0], allocatedQty, strategies, false, string.Empty, AUECLocalDate);
                 
			    }		
				else
				{
                    foreach (AllocationOrder order in _selectedOrders)
                    {
                        strategies = GetEntityStrategiesForPercentage(order.CumQty);
                        _orderAllocationManager.AllocateOrder(order, order.CumQty, strategies, true, string.Empty, AUECLocalDate);
                    }
				}

                
                //foreach(AllocationOrder    order in _selectedOrders)
                //{
                //    _orders.Remove(order);
                //}
				_selectedOrders= new AllocationOrderCollection();
               // _isDataStateChanged = true;
				
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
			}
		}
		private void GroupAllocatedFund()
		{
			try
			{
				if(! AreGroupsElligibleToAllocation())
					return;
				AllocationFunds funds=null;
				
				if(rbtnNumber.Checked)
				{
                    double  allocatedQty = SumOfSharesInTextBoxes();
					funds=GetEntityFundsForShares();

                    _orderAllocationManager.AllocateGroup(_selectedGroups[0], allocatedQty, funds, false,AUECLocalDate);
				}		
			
				else
				{
                    foreach (AllocationGroup group in _selectedGroups)
                    {
                        funds = GetEntityFundsForPercentage(group.CumQty);
                        _orderAllocationManager.AllocateGroup(group, group.CumQty, funds, true,AUECLocalDate);
                    }
                    
				}
                _selectedGroups = new AllocationGroups();
			}
				#region Catch
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
				
			}		
			#endregion
		}
		private void GroupAllocatedStrategy()
		{
			try
			{
				if(! AreGroupsElligibleToAllocation())
					return;
                AllocationStrategies strategies = null;
				if(rbtnNumber.Checked)
				{
                    double  allocatedQty = SumOfSharesInTextBoxes();
                    strategies = GetEntityStrategiesForShares();
                    _orderAllocationManager.AllocateGroup(_selectedGroups[0], allocatedQty, strategies, false,AUECLocalDate);
				}		
			
				else
				{

                    foreach (AllocationGroup group in _selectedGroups)
                    {
                        strategies = GetEntityStrategiesForPercentage(group.CumQty);
                        _orderAllocationManager.AllocateGroup(group, group.CumQty, strategies, true,AUECLocalDate);
                    }
				}
               
                _selectedGroups.Clear();
			}
				#region Catch
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
				
			}		
			#endregion
		}
		private bool AreGroupsElligibleToAllocation()
		{
			if(!CheckEntiresInTextBoxes())
			{
				return false;
				
			}
			
			if(rbtnNumber.Checked)
			{
				if(  _selectedGroups.Count>1)
				{
					MessageBox.Show("Only one group should be selected For Allocation with Shares");
					return false;
			        
				}
				double  allocatedQty=SumOfSharesInTextBoxes();
				if(!(allocatedQty >=_selectedGroups.SumOfExeQuantity()  && allocatedQty <=_selectedGroups.SumOfTotalQuantity()))
				{
					MessageBox.Show("Sum of AllocatedQty should be in Between CumQty And Quantity");
					return false;
				}

			}


			if(rbtnPercentage.Checked  && ! isSingleFundPermitted)
			{

                return CheckSumOfPercentage();
			}


			return true;


		}

		private bool AreOrdersElligibleToAllocation()
		{
            bool bValidated = true;
			
			if(!CheckEntiresInTextBoxes())
			{
				bValidated = false;
				
			}

            string AlreadyAllocatedOrdersString = string.Empty;
            if (bValidated)
            {
                foreach (AllocationOrder aOrder in _selectedOrders)
                {
                    if (aOrder.StateID.Equals(PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED))
                    {
                        AlreadyAllocatedOrdersString +=  aOrder.Symbol
                                                      + " " + aOrder.OrderSide
                                                      + " " + aOrder.OrderType
                                                      + " " + aOrder.Quantity + 
                                                      " "+"Already Grouped"+" on "
                                                      +aOrder.GroupAuecLocalDate.Month
                                                      +"/"+aOrder.GroupAuecLocalDate.Day
                                                      +"/"+aOrder.GroupAuecLocalDate.Year+"\n";

                        //+ " is gouped or allocated"
                        bValidated = false;
                    }
                    else if (aOrder.StateID.Equals(PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED))
                    {
                        AlreadyAllocatedOrdersString +=  aOrder.Symbol
                                                      + " " + aOrder.OrderSide
                                                      + " " + aOrder.OrderType
                                                      + " " + aOrder.Quantity +
                                                      " " + "Already Allocated" + " on "
                                                      + aOrder.GroupAuecLocalDate.Month
                                                      + "/" + aOrder.GroupAuecLocalDate.Day
                                                      + "/" + aOrder.GroupAuecLocalDate.Year + "\n";

                        //+ " is gouped or allocated"
                        bValidated = false;
                    }
                }
            }

            if (!bValidated)
            {
                MessageBox.Show(AlreadyAllocatedOrdersString);
            }

            if (bValidated && rbtnNumber.Checked)
			{

				if(  _selectedOrders.Count>1)
				{
					MessageBox.Show("Only one order should be selected For Allocation with Shares");
                    bValidated = false;
			        
				}
				double  allocatedQty=SumOfSharesInTextBoxes();
//				if(!(allocatedQty >=_selectedOrders.SumOfExeQuantity()  && allocatedQty <=_selectedOrders.SumOfTotalQuantity()))
                if (allocatedQty > _selectedOrders.SumOfExeQuantity())
				{
					MessageBox.Show("Sum of AllocatedQty can't be greater then ExeQty");
                    bValidated = false;
				}

			}


            if (bValidated && rbtnPercentage.Checked && !isSingleFundPermitted)
			{

                bValidated = CheckSumOfPercentage();
							
			}

            if (!bValidated)
            {
                _selectedOrders = new AllocationOrderCollection();
            }

            return bValidated;
		}

		private AllocationFunds GetEntityFundsForShares()
		{
			//UltraGrid Grid = (UltraGrid) grid;
			double  allocatedQty=SumOfSharesInTextBoxes();
			AllocationFunds entityFunds=new AllocationFunds();	

			for(int i=0;i<_funds.Count;i++)
			{
				if( (!(txtUnAllocatedFunds[i].Text.Equals(string.Empty) ))&& (!(txtUnAllocatedFunds[i].Text.Equals("0"))))
				{		
					Prana.BusinessObjects.Fund fund=((Prana.BusinessObjects.Fund) _funds[i]);								
					AllocationFund allocationFund=new AllocationFund();
					allocationFund.FundID=fund.FundID;						
					allocationFund.AllocatedQty=Int64.Parse(txtUnAllocatedFunds[i].Text);
                    allocationFund.Percentage = (allocationFund.AllocatedQty * 100) / allocatedQty;					
					entityFunds.Add(allocationFund);												
					
				}
			}
			return entityFunds;
		}
	
		private AllocationStrategies GetEntityStrategiesForShares()
		{
			//UltraGrid Grid = (UltraGrid) grid;
			double  allocatedQty=SumOfSharesInTextBoxes();
			AllocationStrategies   entityStrategies=new AllocationStrategies();	

			for(int i=0;i<_strategies.Count;i++)
			{
				if( (!(txtUnAllocatedFunds[i].Text.Equals(string.Empty) ))&& (!(txtUnAllocatedFunds[i].Text.Equals("0"))))
				{		
					Prana.BusinessObjects.Strategy strategy=((Prana.BusinessObjects.Strategy)_strategies[i]);						
					AllocationStrategy   allocationstrategy=new AllocationStrategy();
                    allocationstrategy.StrategyID = strategy.StrategyID;
                    allocationstrategy.AllocatedQty = Int64.Parse(txtUnAllocatedFunds[i].Text);
                    allocationstrategy.Percentage = (allocationstrategy.AllocatedQty * 100) / allocatedQty;
                    entityStrategies.Add(allocationstrategy);												
					
				}
			}
			return entityStrategies;
		}
		private AllocationFunds GetEntityFundsForPercentage(double  exeQty)
		{
			if(isSingleFundPermitted)
				txtUnAllocatedFunds[0].Text="100";
			
			AllocationFunds entityFunds=new AllocationFunds();	
			for(int j=0;j<_funds.Count;j++)
			{
				if( (!(txtUnAllocatedFunds[j].Text.Equals(string.Empty) ))&& (!(txtUnAllocatedFunds[j].Text.Equals("0"))))
				{
                    Prana.BusinessObjects.Fund fund = ((Prana.BusinessObjects.Fund)_funds[j]);	
                    AllocationFund allocationFund = new AllocationFund();
                    allocationFund.FundID = fund.FundID;
                    allocationFund.Percentage = float.Parse(txtUnAllocatedFunds[j].Text);
                    entityFunds.Add(allocationFund);												
				}
			}
            FundStraegyManager.SetFundsAllocationQty(entityFunds, exeQty);
			return entityFunds;
		}
        private AllocationStrategies GetEntityStrategiesForPercentage(double  exeQty)
		{
			if(isSingleFundPermitted)
				txtUnAllocatedFunds[0].Text="100";
			
			AllocationStrategies entityStrategies=new AllocationStrategies();	
			for(int j=0;j<_strategies.Count;j++)
			{
				if( (!(txtUnAllocatedFunds[j].Text.Equals(string.Empty) ))&& (!(txtUnAllocatedFunds[j].Text.Equals("0"))))
				{
                    Prana.BusinessObjects.Strategy strategy = ((Prana.BusinessObjects.Strategy)_strategies[j]);													
					AllocationStrategy allocationStrategy=new AllocationStrategy();
                    allocationStrategy.StrategyID = strategy.StrategyID;
                    allocationStrategy.Percentage = float.Parse(txtUnAllocatedFunds[j].Text);
                    entityStrategies.Add(allocationStrategy);												
					
				}
			}
            FundStraegyManager.SetStrategiesAllocationQty(entityStrategies, exeQty);
			return entityStrategies;
		}
		
		#endregion

        private void SetRowColorForUpdated(UltraGrid grid)
        {
            try
            {

                for (int rowIndex = 0; rowIndex < grid.Rows.Count; rowIndex++)
                {
                    if (grid.Rows[rowIndex].Cells["Updated"].Value.ToString() == "True")
                    {
                        grid.Rows[rowIndex].Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedWithUpdateBackColor);
                        grid.Rows[rowIndex].Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedWithUpdateTextColor);
                        grid.Rows[rowIndex].Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedWithUpdateBackColor);
                        grid.Rows[rowIndex].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;

                    }
                }
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }


        }
         

		#region Row Colors

		#region Intilize Row 
		private void grdUnallocated_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			try
			{
                Int64 totalQty = Convert.ToInt64(e.Row.Cells["Quantity"].Value);
                Int64 exeQty = Convert.ToInt64(e.Row.Cells["CumQty"].Value);
				
				if(exeQty <totalQty)
				{
					
					e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor)  ;
					e.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyTextColor)  ;
					e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor)  ;
					e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
					
				}
				else
				{
					e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
					e.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedTextColor)  ;
					e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
					e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
				}

			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
			}

		}

		private void grdGrouped_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			try
			{
                
				Int64 totalQty= Convert.ToInt64(e.Row.Cells["Quantity"].Value);
                Int64 exeQty = Convert.ToInt64(e.Row.Cells["CumQty"].Value);
               
				if(exeQty <totalQty)
				{
					
					e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor)  ;
					e.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyTextColor)  ;
					e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor)  ;
					e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
					
				}
				else
				{
					e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
					e.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedTextColor)  ;
					e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
					e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
				}
               
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
			}
		}

		private void grdAllocated_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			try
			{
                
                Int64 allocatedQty = Convert.ToInt64(e.Row.Cells["AllocatedQty"].Value);
                Int64 totalQty = Convert.ToInt64(e.Row.Cells["Quantity"].Value);
              
				if(allocatedQty <totalQty)
				{
					
					e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyBackColor )  ;
					e.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyTextColor )  ;
					e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyBackColor)  ;
					e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
					
				}
				else
				{
					e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyBackColor )  ;
					e.Row.Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyTextColor)  ;
					e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyBackColor)  ;
					e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
				}
                
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

			}
		
		}

        private void grdAllocatedBasket_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {

                Int64 allocatedQty = Convert.ToInt64(e.Row.Cells["AllocatedQty"].Value);
                Int64 totalQty = Convert.ToInt64(e.Row.Cells["Quantity"].Value);

                if (allocatedQty < totalQty)
                {

                    e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyBackColor);
                    e.Row.Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyTextColor);
                    e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyBackColor);
                    e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;

                }
                else
                {
                    e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyBackColor);
                    e.Row.Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyTextColor);
                    e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyBackColor);
                    e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void grdGroupedBaskets_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {

                Int64 totalQty = Convert.ToInt64(e.Row.Cells["Quantity"].Value);
                Int64 exeQty = Convert.ToInt64(e.Row.Cells["CumQty"].Value);

                if (exeQty < totalQty)
                {

                    e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor);
                    e.Row.Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyTextColor);
                    e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor);
                    e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;

                }
                else
                {
                    e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor);
                    e.Row.Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedTextColor);
                    e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor);
                    e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdUnAllocatedBasket_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {

                Int64 totalQty = Convert.ToInt64(e.Row.Cells["Quantity"].Value);
                Int64 exeQty = Convert.ToInt64(e.Row.Cells["CumQty"].Value);

                if (exeQty < totalQty)
                {

                    e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor);
                    e.Row.Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyTextColor);
                    e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor);
                    e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;

                }
                else
                {
                    e.Row.Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor);
                    e.Row.Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedTextColor);
                    e.Row.Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor);
                    e.Row.Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
		#endregion

       

        private void ApplyRowColorUnAllocated()
		{
			try
			{
				if(_orders.Count==0)
					return;
				
				for(int rowIndex=0;rowIndex<grdUnallocated.Rows.Count;rowIndex++)
				
				{
					if(Convert.ToBoolean(grdUnallocated.Rows[rowIndex].Cells["NotAllExecuted"].Value))
					{
					
						grdUnallocated.Rows[rowIndex].Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor)  ;
						grdUnallocated.Rows[rowIndex].Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyTextColor)  ;
						grdUnallocated.Rows[rowIndex].Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor)  ;
						grdUnallocated.Rows[rowIndex].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
					
					}
					else
					{
						grdUnallocated.Rows[rowIndex].Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
						grdUnallocated.Rows[rowIndex].Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedTextColor)  ;
						grdUnallocated.Rows[rowIndex].Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
						grdUnallocated.Rows[rowIndex].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  

				
					}
					//Selected Row 
                    if (grdUnallocated.Rows[rowIndex].Cells["checkBox"].Value.ToString().Equals("true", StringComparison.OrdinalIgnoreCase)) 
					{					
						grdUnallocated.Rows[rowIndex].Appearance.BackColor=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
						grdUnallocated.Rows[rowIndex].Appearance.BackColor2=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
						grdUnallocated.Rows[rowIndex].Appearance.ForeColor =Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
					}
			
				}
			
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
			}
		}
		
		private void ApplyRowColorGrouped()
		{
			try
			{
			
				for(int rowIndex=0;rowIndex<grdGrouped.Rows.Count;rowIndex++)
				{
					if(Convert.ToBoolean(grdGrouped.Rows[rowIndex].Cells["NotAllExecuted"].Value))
					{
					
						grdGrouped.Rows[rowIndex].Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor );
						grdGrouped.Rows[rowIndex].Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyTextColor);
						grdGrouped.Rows[rowIndex].Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor );
						grdGrouped.Rows[rowIndex].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
					
					}
					else
					{
						grdGrouped.Rows[rowIndex].Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
						grdGrouped.Rows[rowIndex].Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedTextColor)  ;
						grdGrouped.Rows[rowIndex].Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor)  ;
						grdGrouped.Rows[rowIndex].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  

				
					}
					//Selected Row 
                    if (grdGrouped.Rows[rowIndex].Cells["checkBox"].Value.ToString().Equals("true", StringComparison.OrdinalIgnoreCase)) 
					{					
						grdGrouped.Rows[rowIndex].Appearance.BackColor=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
						grdGrouped.Rows[rowIndex].Appearance.BackColor2=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
						grdGrouped.Rows[rowIndex].Appearance.ForeColor =Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
					}
				


				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
			}
	
		}

		private void ApplyRowColorAllocated()
		{
			try
			{
				for(int rowIndex=0;rowIndex<grdAllocated.Rows.Count;rowIndex++)
				{
					if(Convert.ToBoolean(grdAllocated.Rows[rowIndex].Cells["AllocatedEqualTotalQty"].Value))
					{
					
						grdAllocated.Rows[rowIndex].Appearance.BackColor =Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyBackColor);
						grdAllocated.Rows[rowIndex].Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyBackColor);
						grdAllocated.Rows[rowIndex].Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyTextColor);
						grdAllocated.Rows[rowIndex].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  
					}

					else
					{
						grdAllocated.Rows[rowIndex].Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyBackColor );
						grdAllocated.Rows[rowIndex].Appearance.ForeColor  = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyTextColor)  ;
						grdAllocated.Rows[rowIndex].Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyBackColor );
						grdAllocated.Rows[rowIndex].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;  

					}

					//Selected Row 
                    if (grdAllocated.Rows[rowIndex].Cells["checkBox"].Value.ToString().Equals("true", StringComparison.OrdinalIgnoreCase)) 
					{					
						grdAllocated.Rows[rowIndex].Appearance.BackColor=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
						grdAllocated.Rows[rowIndex].Appearance.BackColor2=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
						grdAllocated.Rows[rowIndex].Appearance.ForeColor =Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
					}
				

				


				}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
			}
		}
        private void ApplyRowColorBaskets(UltraGrid grid)
        {
            try
            {
                for (int rowIndex = 0; rowIndex < grid.Rows.Count; rowIndex++)
                {
                   
                        grid.Rows[rowIndex].Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor);
                        grid.Rows[rowIndex].Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedTextColor);
                        grid.Rows[rowIndex].Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor);
                        grid.Rows[rowIndex].Appearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;



                        if (grid.Rows[rowIndex].Cells["checkBox"].Value.ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        grid.Rows[rowIndex].Appearance.BackColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
                        grid.Rows[rowIndex].Appearance.BackColor2 = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
                        grid.Rows[rowIndex].Appearance.ForeColor = Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
                    }

                }

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
       

        #endregion

        #region Commented
        //public void ShowBundledStrategies(AllocationGroup groupedEntity,bool addition)
        //{
        //    AllocationGroup newgroupedEntity=null;
        //    //Adding into Entity AllocationGroups
        //    if(addition)
        //    {
        //            _allocatedGroups.Add(groupedEntity);
        //    //Allocated AllocationGroups
        //        if(groupedEntity.AllocationGroup.GroupID!=string.Empty )
        //        {
        //            foreach(AllocationOrder order in groupedEntity.AllocationGroup.orders)
        //            {
        //                AllocationOrder tempOrder = _orders.GetOrder(order.ClOrderID);
        //                if(tempOrder!=null)
        //                {
        //                    _orders.Remove(tempOrder);
        //                }
        //            }
        //        }
        //        //Allocated AllocationOrderCollection
        //        else
        //        {
					
        //            AllocationOrder tempOrder =_orders.GetOrder(groupedEntity.GetOrder().ClOrderID);
        //            if(tempOrder!=null)
        //            {
        //                _orders.Remove(tempOrder);
        //            }
				
        //        }
        //    }
        //    //Deleting from Entity AllocationGroups
        //    else
        //    {
        //        newgroupedEntity = _allocatedGroups.GetGroupEntity(groupedEntity.GroupedEntityID);
        //        if(newgroupedEntity ==null)
        //            return;
        //        _allocatedGroups.Remove(newgroupedEntity);
        //        //Allocated AllocationGroups
        //        if(groupedEntity.AllocationGroup.GroupID!=string.Empty )
        //        {
        //            foreach(AllocationOrder order in groupedEntity.AllocationGroup.orders)
        //            {
						
        //                _orders.Add(order);
        //            }
        //        }
        //            //Allocated AllocationOrderCollection
        //        else
        //        {
					
        //        AllocationOrder tempOrder = groupedEntity.GetOrder();
        //        _orders.Add(tempOrder);
					
        //        }
				
        //    }


        //}

        #endregion

        #region Grid Column Display Depending upon Prefs

        private void HideColumns()
        {
            try
            {
                HideUnAllocatedGridColumns();
                HideGroupedGridColumns();
                HideAllocatedGridColumns();
                HideBasketColumns();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// shows  columns specified in displayColumnList
        /// and hides other columns
        /// </summary>
        /// <param name="displayColumnList"></param>
        /// <param name="Columns"></param>
        private void ShowReleventColumns(DisplayColumn[] displayColumnList, ColumnsCollection Columns)
        {
            try
            {
                foreach (UltraGridColumn column in Columns)
                {
                    column.Hidden = true;
                }
                int i = 0;
                foreach (DisplayColumn displayColumn in displayColumnList)
                {
                    string captionName = displayColumn.DisplayName;
                    string columnName = OrderFields.ColumnNameHeaderCollection[captionName];
                    Columns[columnName].Hidden = false;
                    Columns[columnName].Header.VisiblePosition = i;
                    Columns[columnName].Width = 50;
                    Columns[columnName].Header.Caption = captionName;
                    i++;
                }
                if (Columns.Exists(OrderFields.HEADCOL_CHKBOX))
                {
                    Columns[OrderFields.HEADCOL_CHKBOX].Hidden = false;
                    Columns[OrderFields.HEADCOL_CHKBOX].Width = 8;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        private void HideUnAllocatedGridColumns()
        {
            try
            {
                ColumnsCollection columnsCollection = grdUnallocated.DisplayLayout.Bands[0].Columns;
                DisplayColumn[] displayColumnList = null;
                if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                {
                    displayColumnList = _allocationPreferences.FundType.UnAllocatedGridColumns.DisplayColumns;
                }
                else
                {
                    displayColumnList = _allocationPreferences.StrategyType.UnAllocatedGridColumns.DisplayColumns;
                }

                ShowReleventColumns(displayColumnList, columnsCollection);
                //int count ;
                //foreach (UltraGridColumn column in columnsCollection)
                //{
                //    column.Hidden = true;
                //}
                //if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                //{
                //    count = _allocationPreferences.FundType.UnAllocatedGridColumns.DisplayColumns.Length;
                //    for (int i = 0; i < count; i++)
                //    {
                //        string key = _allocationPreferences.FundType.UnAllocatedGridColumns.DisplayColumns[i].DisplayName;
                //        columnsCollection[key].Hidden = false;
                //        columnsCollection[key].Header.VisiblePosition = i;
                //        columnsCollection[key].Width = 100;
                       
                //    }
                //}
                //else
                //{
                //    count = _allocationPreferences.StrategyType.UnAllocatedGridColumns.DisplayColumns.Length;
                //    for (int i = 0; i < count; i++)
                //    {
                //        string key = _allocationPreferences.StrategyType.UnAllocatedGridColumns.DisplayColumns[i].DisplayName;
                //        columnsCollection[key].Hidden = false;
                //        columnsCollection[key].Header.VisiblePosition = i;
                //        columnsCollection[key].Width = 100;
                        
                //    }

                //}
                //columnsCollection["checkBox"].Hidden = false;
                //columnsCollection["checkBox"].Width = 20;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }
        private void HideGroupedGridColumns()
        {
            try
            {
                ColumnsCollection columnsCollection = grdGrouped.DisplayLayout.Bands[0].Columns;
                DisplayColumn[] displayColumnList = null;
                if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                {
                    displayColumnList = _allocationPreferences.FundType.GroupedGridColumns.DisplayColumns;
                }
                else
                {
                    displayColumnList = _allocationPreferences.StrategyType.GroupedGridColumns.DisplayColumns;
                }
                ShowReleventColumns(displayColumnList, columnsCollection);

                //ColumnsCollection columnsCollection = grdGrouped.DisplayLayout.Bands[0].Columns;
                //int count ;
                //foreach (UltraGridColumn column in columnsCollection)
                //{
                //    column.Hidden = true;
                //}
                //if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                //{
                //    count = _allocationPreferences.FundType.GroupedGridColumns.DisplayColumns.Length;
                //    for (int i = 0; i < count; i++)
                //    {
                //        string key = _allocationPreferences.FundType.GroupedGridColumns.DisplayColumns[i].DisplayName;
                //        columnsCollection[key].Hidden = false;
                //        columnsCollection[key].Header.VisiblePosition = i;
                //        columnsCollection[key].Width = 100;
                //    }
                //}
                //else
                //{
                //    count = _allocationPreferences.StrategyType.GroupedGridColumns.DisplayColumns.Length;
                //    for (int i = 0; i < count; i++)
                //    {
                //        string key = _allocationPreferences.StrategyType.GroupedGridColumns.DisplayColumns[i].DisplayName;
                //        columnsCollection[key].Hidden = false;
                //        columnsCollection[key].Header.VisiblePosition = i;
                //        columnsCollection[key].Width = 100;
                       
                //    }

                //}
                //columnsCollection["checkBox"].Hidden = false;
                //columnsCollection["checkBox"].Width = 20;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }
        private void HideAllocatedGridColumns()
        {
            try
            {
                ColumnsCollection columnsCollection = grdAllocated.DisplayLayout.Bands[0].Columns;
                DisplayColumn[] displayColumnList = null;
                if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                {
                    displayColumnList = _allocationPreferences.FundType.AllocatedGridColumns.DisplayColumns;
                }
                else
                {
                    displayColumnList = _allocationPreferences.StrategyType.AllocatedGridColumns.DisplayColumns;
                }
                ShowReleventColumns(displayColumnList, columnsCollection);


                //ColumnsCollection columnsCollection = grdAllocated.DisplayLayout.Bands[0].Columns;
                //int count ;
                //foreach (UltraGridColumn column in columnsCollection)
                //{
                //    column.Hidden = true;
                //}
                //if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                //{
                //    count = _allocationPreferences.FundType.AllocatedGridColumns.DisplayColumns.Length;
                //    for (int i = 0; i < count; i++)
                //    {
                //        string key = _allocationPreferences.FundType.AllocatedGridColumns.DisplayColumns[i].DisplayName;
                //        columnsCollection[key].Hidden = false;
                //        columnsCollection[key].Header.VisiblePosition = i;
                //        columnsCollection[key].Width = 100;
                //    }
                //}
                //else
                //{
                //    count = _allocationPreferences.StrategyType.AllocatedGridColumns.DisplayColumns.Length;
                //    for (int i = 0; i < count; i++)
                //    {
                //        string key = _allocationPreferences.StrategyType.AllocatedGridColumns.DisplayColumns[i].DisplayName;
                //        columnsCollection[key].Hidden = false;
                //        columnsCollection[key].Header.VisiblePosition = i;
                //        columnsCollection[key].Width = 100;
                       
                //    }

                //}
                //columnsCollection["checkBox"].Hidden = false;
                //columnsCollection["checkBox"].Width = 20;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void HideBasketColumns()
        {
            string[] UnallocatedNames = Enum.GetNames(typeof(UnallocatedBasketColumns));
            ShowColumns(UnallocatedNames, grdUnAllocatedBasket);
            grdUnAllocatedBasket.DisplayLayout.Bands[0].Columns[UnallocatedBasketColumns.Quantity.ToString()].Header.Caption = "UnAllocated Qty";
            string[] GroupColumns = Enum.GetNames(typeof(BasketGroupColumns));
            ShowColumns(GroupColumns, grdGroupedBaskets);
            string[] AllocatedNames = Enum.GetNames(typeof(AllocatedBasketColumns));
            ShowColumns(AllocatedNames, grdAllocatedBasket);


        }
        private void ShowColumns(string[] columnNames, UltraGrid grid)
        {

            ColumnsCollection columns = grid.DisplayLayout.Bands[0].Columns;
            if (columns.Count <= 0) return;
            foreach (UltraGridColumn column in columns)
            {
                column.Hidden = true;
            }
            int i = 0;
            foreach (string columnName in columnNames)
            {
                columns[columnName].Hidden = false;
                columns[columnName].Hidden = false;
                columns[columnName].Header.VisiblePosition = i;
                columns[columnName].Width = 100;
                i++;
            }
            columns["checkBox"].Hidden = false;
            columns["checkBox"].Width = 20;
        }

        #endregion

        #region Properties
        //public allocatedGroups allocatedGroups
        //{
        //    get { return _allocatedGroups; }
        //}
        //public AllocationGroups UnAllocatedGroups
        //{
        //    get { return _groups; }
        //}
        public AllocationPreferences AllocationPreferences
        {
            get { return _allocationPreferences; }
        }
        //public string DeletedUnAllocatedGroups
        //{
        //    get { return _deletedGroups; }

        //}
        //public bool ISDataStateChanged
        //{
        //    get { return _isDataStateChanged; }
        //    set {  _isDataStateChanged=value ; }
        //}
        public PranaInternalConstants.TYPE_OF_ALLOCATION FormType
        {
            set { _formType = value; }
            get { return _formType; }
        }
        #endregion

        #region Public Methods
        public void SetDefaultValuesToDefaultCombo()
        {
            cmbbxDefaults.Value = ApplicationConstants.C_COMBO_SELECT;
        }
        #endregion

        //public void ClearAllFlagIDS()
        //{
        //    addedEntityIDS = string.Empty;
        //    deletedEntityIDS = string.Empty;

        //    addedBasketEntityIDS = string.Empty;
        //    addedGroupedBasketIDS = string.Empty;

        //    deletedGroupedBasketIDS = string.Empty;
        //    deletedGroupedBasketIDS = string.Empty;
        //    _isDataStateChanged = false;
        //}

        private void grdUnAllocatedBasket_MouseUp(object sender, MouseEventArgs e)
        {
            #region Getting checkBox Cell


            if (e.Button.ToString() == "Right")
                return;
            UIElement objUIElement;
            UltraGridCell objUltraGridCell;
            objUIElement = grdUnAllocatedBasket.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
            if (objUIElement == null)
                return;
            objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
            if (objUltraGridCell == null)
                return;


            if ((objUltraGridCell.Column.Key != "checkBox"))
            {
                return;

            }
            #endregion

           Prana.BusinessObjects.BasketDetail basket= _unAllocatedBaskets.GetBasket(objUltraGridCell.Row.Cells["TradedBasketID"].Value.ToString());
           basket.Updated = false;
           if (_selectedBaskets.Contains(basket))
           {
               _selectedBaskets.Remove(basket);
               ApplyRowColor(objUltraGridCell.Row, _notTradedBackColor, _notTradedForeColor);
               objUltraGridCell.Value = false;
           }
           else
           {
               _selectedBaskets.Add(basket);
               ApplyRowColor(objUltraGridCell.Row, _selectedRowBackColor, _selectedRowForeColor);
               objUltraGridCell.Value = true ;
           }
           
            
        }
        private void ApplyRowColor(UltraGridRow row ,Color backColor,Color foreColor)
        {
            row.Appearance.BackColor = backColor;
            row.Appearance.BackColor2 = backColor;
            row.Appearance.ForeColor = foreColor;

        }
        private void grdGroupedBaskets_MouseUp(object sender, MouseEventArgs e)
        {
            #region Getting checkBox Cell


            if (e.Button.ToString() == "Right")
                return;
            UIElement objUIElement;
            UltraGridCell objUltraGridCell;
            objUIElement = grdGroupedBaskets.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
            if (objUIElement == null)
                return;
            objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
            if (objUltraGridCell == null)
                return;


            if ((objUltraGridCell.Column.Key != "checkBox"))
            {
                return;

            }
            #endregion

            BasketGroup basketGroup = _groupedBaskets.GetBasketGroup(objUltraGridCell.Row.Cells["BasketGroupID"].Value.ToString());
            basketGroup.Updated = false;
            if (_selectedGropuedBaskets.Contains(basketGroup))
            {
                _selectedGropuedBaskets.Remove(basketGroup);
                ApplyRowColor(objUltraGridCell.Row, _notTradedBackColor, _notTradedForeColor);
                objUltraGridCell.Value = false;
            }
            else
            {
                _selectedGropuedBaskets.Add(basketGroup);
                ApplyRowColor(objUltraGridCell.Row, _selectedRowBackColor, _selectedRowForeColor);
                objUltraGridCell.Value = true;
            }
        }
        private void grdAllocatedBasket_MouseUp(object sender, MouseEventArgs e)
        {
            #region Getting checkBox Cell


            if (e.Button.ToString() == "Right")
                return;
            UIElement objUIElement;
            UltraGridCell objUltraGridCell;
            objUIElement = grdAllocatedBasket.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
            if (objUIElement == null)
                return;
            objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
            if (objUltraGridCell == null)
                return;


            if ((objUltraGridCell.Column.Key != "checkBox"))
            {
                return;

            }
            #endregion

            BasketGroup basketGroup = _allocatedGroupedBaskets.GetBasketGroup(objUltraGridCell.Row.Cells["BasketGroupID"].Value.ToString());
            basketGroup.Updated = false;
            if (_selectedAllocatedBaskets.Contains(basketGroup))
            {
                _selectedAllocatedBaskets.Remove(basketGroup);
                ApplyRowColor(objUltraGridCell.Row, _notTradedBackColor, _notTradedForeColor);
                objUltraGridCell.Value = false;
            }
            else
            {
                _selectedAllocatedBaskets.Add(basketGroup);
                ApplyRowColor(objUltraGridCell.Row, _selectedRowBackColor, _selectedRowForeColor);
                objUltraGridCell.Value = true;
            }
        }

        #region Context Menus of Basket Trading

        private void menuItemBasketGroup_Click(object sender, EventArgs e)
        {
            bool bValidated = true;
            
            string AlreadyAllocatedOrdersString = string.Empty;
            if (bValidated)
            {


                foreach (Prana.BusinessObjects.BasketDetail basket in _selectedBaskets)
                {
                    BasketGroup basketGroup = new BasketGroup();
                    basketGroup = BasketAllocationManager.GetAllocationDate(basket.TradedBasketID, (int)_formType);
                    if (basketGroup.GroupState.Equals(PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED))
                    {
                        AlreadyAllocatedOrdersString += "Basket " + basket.BasketName
                                                       + " " + "(" + basket.TradedBasketID + ")"
                                                   + " " + "Already Grouped" + " on "
                                                   + basketGroup.AUECLocalDate.Month
                                                   + "/" + basketGroup.AUECLocalDate.Day
                                                   + "/" + basketGroup.AUECLocalDate.Year + "\n";
                        bValidated = false;
                    }
                    else if (basketGroup.GroupState.Equals(PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED))
                    {
                        AlreadyAllocatedOrdersString += "Basket " + basket.BasketName
                                                       + " " + "(" + basket.TradedBasketID + ")"
                                                                         + " " + "Already Allocated" + " on "
                                                                         + basketGroup.AUECLocalDate.Month
                                                                         + "/" + basketGroup.AUECLocalDate.Day
                                                                         + "/" + basketGroup.AUECLocalDate.Year + "\n";
                        bValidated = false;
                    }
                }
            }
            if (!bValidated)
            {
                MessageBox.Show(AlreadyAllocatedOrdersString);

            }
            else
            {
                _basketAllocationManager.GroupBaskets(_selectedBaskets, currentDateTime);
            }
            //foreach (Prana.BusinessObjects.BasketDetail basket in _selectedBaskets)
            //{
            //    _unAllocatedBaskets.Remove(basket);
           // }
            UnCheckAllRows(grdUnAllocatedBasket, headerCheckBoxUnAllocatedBasket);
            _selectedBaskets = new BasketCollection();
        }
        private void menuItemBasketgrpUnGroup_Click(object sender, EventArgs e)
        {
            foreach (BasketGroup basketGroup in _selectedGropuedBaskets)
            {
                _basketAllocationManager.UnGroupBasketGroup(basketGroup);
            }
            UnCheckAllRows(grdGroupedBaskets, headerCheckBoxGroupedBasket);
            _selectedGropuedBaskets = new BasketGroupCollection();
       }

        private void menuItemBasketAllocate_Click(object sender, EventArgs e)
        {
             bool bValidated = true;
              if (!CheckEntiresInTextBoxes())
                {
                    bValidated=false;
                }

            string AlreadyAllocatedOrdersString = string.Empty;
            if (bValidated)
            {
                    
              
                    foreach (Prana.BusinessObjects.BasketDetail basket in _selectedBaskets)
                    {
                        BasketGroup basketGroup = new BasketGroup();
                        basketGroup = BasketAllocationManager.GetAllocationDate(basket.TradedBasketID,(int)_formType);
                        if (basketGroup.GroupState.Equals(PranaInternalConstants.ORDERSTATE_ALLOCATION.GROUPED))
                        {
                            AlreadyAllocatedOrdersString += "Basket " + basket.BasketName 
                                                       +" " +"("+basket.TradedBasketID +")"
                                                      + " " + "Already Grouped" + " on "
                                                       + basketGroup.AUECLocalDate.Month
                                                       + "/" + basketGroup.AUECLocalDate.Day
                                                       + "/" + basketGroup.AUECLocalDate.Year + "\n";
                            bValidated = false;
                        }
                        else if (basketGroup.GroupState.Equals(PranaInternalConstants.ORDERSTATE_ALLOCATION.ALLOCATED))
                        {
                            AlreadyAllocatedOrdersString += "Basket " + basket.BasketName
                                                       + " " + "(" + basket.TradedBasketID + ")"
                                                                             + " " + "Already Allocated" + " on "
                                                                             + basketGroup.AUECLocalDate.Month
                                                                             + "/" + basketGroup.AUECLocalDate.Day
                                                                             + "/" + basketGroup.AUECLocalDate.Year + "\n";
                            bValidated = false;
                        }
                    }
            }
            if (!bValidated)
            {
                MessageBox.Show(AlreadyAllocatedOrdersString);
            } 
            else
            {
             bValidated = CheckSumOfPercentage();   
            }
            if (bValidated)
                {
                    foreach (Prana.BusinessObjects.BasketDetail basket in _selectedBaskets)
                    {
                        object fundsOrStrategy = null;
                        if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                        {
                            fundsOrStrategy = GetEntityFundsForPercentage(basket.CumQty);
                        }
                        else
                        {
                            fundsOrStrategy = GetEntityStrategiesForPercentage(basket.CumQty);
                        }
                        
                        
                      _basketAllocationManager.AllocateBasket(basket, fundsOrStrategy, AUECLocalDate);
                        
                    }
                    _selectedBaskets = new Prana.BusinessObjects.BasketCollection();
                }
            

            UnCheckAllRows(grdUnAllocatedBasket, headerCheckBoxUnAllocatedBasket);
           _selectedBaskets = new BasketCollection();
            
        }
        private void menuItemBasketGrpAllocate_Click(object sender, EventArgs e)
        {
            if (CheckEntiresInTextBoxes())
            {
                if (CheckSumOfPercentage())
                {
                    foreach (BasketGroup basketGroup in _selectedGropuedBaskets)
                    {
                        double exeQty = basketGroup.CumQty;

                        object fundsOrStrategy = null;
                        if (_formType == PranaInternalConstants.TYPE_OF_ALLOCATION.FUND)
                        {
                            fundsOrStrategy = GetEntityFundsForPercentage(basketGroup.CumQty);
                        }
                        else
                        {
                            fundsOrStrategy = GetEntityStrategiesForPercentage(basketGroup.CumQty);
                        }
                        
                        _basketAllocationManager.AllocateBasket(basketGroup, fundsOrStrategy,AUECLocalDate);

                    }
                    _selectedGropuedBaskets = new BasketGroupCollection();
                }
            }
            UnCheckAllRows(grdGroupedBaskets, headerCheckBoxGroupedBasket);
            _selectedGropuedBaskets = new BasketGroupCollection();
        }

        private void menuItemBasketUnAllocae_Click(object sender, EventArgs e)
        {
            foreach (BasketGroup basketGroup in _selectedAllocatedBaskets)
            {
                _basketAllocationManager.UnAllocateBasket(basketGroup);
                
            }
            headerCheckBoxAllocatedBasket.Checked = CheckState.Unchecked;
            UnCheckAllRows(grdAllocatedBasket, headerCheckBoxAllocatedBasket);
            _selectedAllocatedBaskets = new BasketGroupCollection();
        }

        #endregion

        private void grdUnAllocatedBasket_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            DefaultChanged();
        }

        private void grdGroupedBaskets_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            DefaultChanged();
        }

        private void grdAllocatedBasket_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            DefaultChanged();
        }

        private void menuItemUnBundle_Click(object sender, EventArgs e)
        {
            foreach (Prana.BusinessObjects.BasketDetail basket in _selectedBaskets)
            {
                _basketAllocationManager.UnBundleBasket(basket);
            }
            _selectedBaskets = new Prana.BusinessObjects.BasketCollection();
        }


        //private void AddEntityID(string entityID)
        //{
        //    if (addedEntityIDS.Equals(string.Empty))
        //        addedEntityIDS = entityID;
        //    else
        //        addedEntityIDS = addedEntityIDS + "," + entityID;

        //}
        //private void AddBasketEntityID(string entityID)
        //{
        //    if (addedBasketEntityIDS.Equals(string.Empty))
        //        addedBasketEntityIDS = entityID;
        //    else
        //        addedBasketEntityIDS = addedBasketEntityIDS + "," + entityID;

        //}
        //private void AddGroupedBasketID(string entityID)
        //{
        //    if (addedGroupedBasketIDS.Equals(string.Empty))
        //        addedGroupedBasketIDS = entityID;
        //    else
        //        addedGroupedBasketIDS = addedGroupedBasketIDS + "," + entityID;

        //}

        //private void DeleteEntityID(string entityID)
        //{
        //    if (deletedEntityIDS.Equals(string.Empty))
        //        deletedEntityIDS = entityID;
        //    else
        //        deletedEntityIDS = deletedEntityIDS + "," + entityID;

        //}
        //private void DeleteBasketEntityID(string entityID)
        //{
        //    if (deletedBasketEntityIDS.Equals(string.Empty))
        //        deletedBasketEntityIDS = entityID;
        //    else
        //        deletedBasketEntityIDS = deletedBasketEntityIDS + "," + entityID;

        //}
        //private void DeleteBasketGroupID(string entityID)
        //{
        //    if (deletedGroupedBasketIDS.Equals(string.Empty))
        //        deletedGroupedBasketIDS = entityID;
        //    else
        //        deletedGroupedBasketIDS = deletedGroupedBasketIDS + "," + entityID;

        //}

        public BasketGroupCollection AllocatedBaskets
        {
            get { return _allocatedGroupedBaskets; }
        }

        private void menuBasketGroups_Popup(object sender, EventArgs e)
        {
            if (_selectedGropuedBaskets.Count == 0)
            {
                menuItemBasketgrpUnGroup.Visible = false;
                menuItemBasketGrpAllocate.Visible  = false;
            }
            else
            {
                if (_read_write == 1)
                {
                    menuItemBasketgrpUnGroup.Visible = true;
                    menuItemBasketgrpUnGroup.Enabled = true;
                    menuItemBasketGrpAllocate.Visible = true;
                    menuItemBasketGrpAllocate.Enabled = true;
                }
                else
                {
                    menuItemBasketgrpUnGroup.Visible = true;
                    menuItemBasketgrpUnGroup.Enabled = false ;
                    menuItemBasketGrpAllocate.Visible = true;
                    menuItemBasketGrpAllocate.Enabled = false;

                }
            }
        }

        private void mnuBasketUnAllocate_Popup(object sender, EventArgs e)
        {
            menuItemUnBundle.Visible = false;
            if (_selectedBaskets.Count == 0)
            {
                menuItemBasketAllocate.Visible = false;
                menuItemBasketGroup.Visible = false;
                //menuItemUnBundle.Visible = false;
            }
            else if (_selectedBaskets.Count == 1)
            {
                if (_read_write == 1)
                {
                    menuItemBasketAllocate.Visible = true;
                    menuItemBasketAllocate.Enabled = true;
                    menuItemBasketGroup.Visible = false;
                    // menuItemUnBundle.Visible = true;
                }
                else
                {
                    menuItemBasketAllocate.Visible = true;
                    menuItemBasketAllocate.Enabled = false;
                    menuItemBasketGroup.Visible = false;
                }
               
            }
            else 
            {
                menuItemBasketAllocate.Visible = true;
                menuItemBasketGroup.Visible = true;
              //  menuItemUnBundle.Visible = true;
            }

        }

        private void menuBasketAllocated_Popup(object sender, EventArgs e)
        {
            if (_selectedAllocatedBaskets.Count == 0)
            {
                menuItemBasketUnAllocae.Visible = false;
                menuItemProrata.Visible = false;
            }
            else
            {
                if (_read_write == 1)
                {
                    menuItemBasketUnAllocae.Visible = true;
                    menuItemBasketUnAllocae.Enabled = true;
                }
                else
                {
                    menuItemBasketUnAllocae.Visible = true;
                    menuItemBasketUnAllocae.Enabled = false;
                }
            }

            foreach (BasketGroup  group in _selectedAllocatedBaskets)
            {
                if (group.AllocatedQty < group.CumQty)
                {
                    menuItemProrata.Visible = true;
                }
                else
                {
                    mnuFundsProrata.Visible = false;
                    break;
                }
            }
        }

        public BasketGroupCollection  GetAddedAllocatedBaskets()
        {
            BasketGroupCollection addedBasketGroups = new BasketGroupCollection();
            string[] addition= addedBasketEntityIDS.Split(',');
            foreach (string allocaedBasketID in addition)
            {
                foreach (BasketGroup basketGroup in _allocatedGroupedBaskets)
                {
                    if (basketGroup.BasketGroupID == allocaedBasketID)
                    {
                        addedBasketGroups.Add(basketGroup);
                        break;
                    }
                }
            }
            return addedBasketGroups;
        }
        public BasketGroupCollection GetAddedGroupedBaskets()
        {
            BasketGroupCollection addedBasketGroups = new BasketGroupCollection();
            string[] addition = addedGroupedBasketIDS.Split(',');
            foreach (string allocaedBasketID in addition)
            {
                foreach (BasketGroup basketGroup in _groupedBaskets)
                {
                    if (basketGroup.BasketGroupID == allocaedBasketID)
                    {
                        addedBasketGroups.Add(basketGroup);
                        break;
                    }
                }
            }
            return addedBasketGroups;
        }

        private void grdAllocated_DragDrop(object sender, DragEventArgs e)
        {
          
        }

        private void menuItemProrata_Click(object sender, EventArgs e)
        {
            foreach (BasketGroup group in _selectedAllocatedBaskets)
            {
                _basketAllocationManager.ProrataBasketAllocation(group,AUECLocalDate);
               
                

            }
        }

        private void cmbbxAsset_ValueChanged(object sender, EventArgs e)
        {
            if (cmbbxAsset.Value != null)
            {
                UltraGridBand band = grdUnallocated.DisplayLayout.Bands[0];
                UltraGridBand bandGrouped = grdGrouped.DisplayLayout.Bands[0];
                ClearAllFilters("AssetID");
                int assetID = int.Parse(cmbbxAsset.Value.ToString());
                BindUndeyLying(assetID);
                if (assetID != int.MinValue)
                {
                    band.ColumnFilters["AssetID"].FilterConditions.Add(FilterComparisionOperator.Equals, assetID.ToString());
                    bandGrouped.ColumnFilters["AssetID"].FilterConditions.Add(FilterComparisionOperator.Equals, assetID.ToString());
                }
                UnCheckAllRows(grdUnallocated,headerCheckBoxUnallocated);
                UnCheckAllRows(grdGrouped, headerCheckBoxGrouped);
            }
        }

        private void cmbbxUnderlying_ValueChanged(object sender, EventArgs e)
        {
            if (cmbbxUnderlying.Value != null)
            {
                UltraGridBand band = grdUnallocated.DisplayLayout.Bands[0];
                UltraGridBand bandGrouped = grdGrouped.DisplayLayout.Bands[0];
                ClearAllFilters("UnderLyingID");
                int underLyingID = int.Parse(cmbbxUnderlying.Value.ToString());
                if (underLyingID != int.MinValue)
                {
                    band.ColumnFilters["UnderLyingID"].FilterConditions.Add(FilterComparisionOperator.Equals, underLyingID.ToString());
                    bandGrouped.ColumnFilters["UnderLyingID"].FilterConditions.Add(FilterComparisionOperator.Equals, underLyingID.ToString());
                }
                UnCheckAllRows(grdUnallocated, headerCheckBoxUnallocated);
                UnCheckAllRows(grdGrouped, headerCheckBoxGrouped);
            }
        }

        private void cmbbxExchange_ValueChanged(object sender, EventArgs e)
        {
            if (cmbbxExchange.Value != null)
            {
                UltraGridBand band = grdUnallocated.DisplayLayout.Bands[0];
                UltraGridBand bandGrouped = grdGrouped.DisplayLayout.Bands[0];
                ClearAllFilters("ExchangeID");
                int exchangeID = int.Parse(cmbbxExchange.Value.ToString());

                if (exchangeID != int.MinValue)
                {
                    band.ColumnFilters["ExchangeID"].FilterConditions.Add(FilterComparisionOperator.Equals, exchangeID.ToString());
                    bandGrouped.ColumnFilters["ExchangeID"].FilterConditions.Add(FilterComparisionOperator.Equals, exchangeID.ToString());
                }
                UnCheckAllRows(grdUnallocated, headerCheckBoxUnallocated);
                UnCheckAllRows(grdGrouped, headerCheckBoxGrouped);
            }
        }

        private void ClearAllFilters(string columnsName)
        {
            //UnAllocated Grid
            UltraGridBand band = grdUnallocated.DisplayLayout.Bands[0];
            band.ColumnFilters[columnsName].ClearFilterConditions();
            //Grouped Grid
            UltraGridBand bandGrouped = grdGrouped.DisplayLayout.Bands[0];
            bandGrouped.ColumnFilters[columnsName].ClearFilterConditions();
        }
        private void UnCheckAllRows(UltraGrid grid ,CheckBoxOnHeader_CreationFilter headerChkbx)
        {
            try
            {
                foreach (UltraGridRow row in grid.Rows)
                {
                    row.Cells["checkBox"].Value = false;
                }
                headerChkbx.Checked = CheckState.Unchecked;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
        }

        private void cmbCounterParty_ValueChanged(object sender, EventArgs e)
        {
            if (cmbCounterParty.Value != null)
            {
                UltraGridBand band = grdUnallocated.DisplayLayout.Bands[0];
                UltraGridBand bandGrouped = grdGrouped.DisplayLayout.Bands[0];
                ClearAllFilters("CounterPartyID");
                int counterPartyID = int.Parse(cmbCounterParty.Value.ToString());
                BindVenue(counterPartyID);
                if (counterPartyID != int.MinValue)
                {
                    band.ColumnFilters["CounterPartyID"].FilterConditions.Add(FilterComparisionOperator.Equals, counterPartyID.ToString());
                    bandGrouped.ColumnFilters["CounterPartyID"].FilterConditions.Add(FilterComparisionOperator.Equals, counterPartyID.ToString());
                }
                UnCheckAllRows(grdUnallocated, headerCheckBoxUnallocated);
                UnCheckAllRows(grdGrouped, headerCheckBoxGrouped);
            }
        }

        private void cmbVenue_ValueChanged(object sender, EventArgs e)
        {
            if (cmbVenue.Value != null)
            {
                UltraGridBand band = grdUnallocated.DisplayLayout.Bands[0];
                UltraGridBand bandGrouped = grdGrouped.DisplayLayout.Bands[0];
                ClearAllFilters("VenueID");
                int venueID = int.Parse(cmbVenue.Value.ToString());

                if (venueID != int.MinValue)
                {
                    band.ColumnFilters["VenueID"].FilterConditions.Add(FilterComparisionOperator.Equals, venueID.ToString());
                    bandGrouped.ColumnFilters["VenueID"].FilterConditions.Add(FilterComparisionOperator.Equals, venueID.ToString());
                }
                UnCheckAllRows(grdUnallocated, headerCheckBoxUnallocated);
                UnCheckAllRows(grdGrouped, headerCheckBoxGrouped);
            }
        }

        private void txtSymbol_TextChanged(object sender, EventArgs e)
       {
           ClearAllFilters("Symbol");
            if (txtSymbol.Text != string.Empty)
            {
                string[] textSymbol = txtSymbol.Text.Split(',');

                grdUnallocated.DisplayLayout.Bands[0].ColumnFilters["Symbol"].LogicalOperator = FilterLogicalOperator.Or;
                foreach (string s in textSymbol)
                {
                    grdUnallocated.DisplayLayout.Bands[0].ColumnFilters["Symbol"].FilterConditions.Add(FilterComparisionOperator.StartsWith, s);
                }
            }
        }

        private void cmbOrderSide_ValueChanged(object sender, EventArgs e)
        {
            if (cmbOrderSide.Value != null)
            {
                UltraGridBand band = grdUnallocated.DisplayLayout.Bands[0];
                UltraGridBand bandGrouped = grdGrouped.DisplayLayout.Bands[0];
                ClearAllFilters("OrderSide");
                if (cmbOrderSide.Value != null)
                {
                    switch (cmbOrderSide.Value.ToString())
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Buy_Closed:
                        case FIXConstants.SIDE_Buy_Open:
                        case FIXConstants.SIDE_BuyMinus:
                        case FIXConstants.SIDE_Cross:
                        case FIXConstants.SIDE_CrossShort:
                        case FIXConstants.SIDE_Sell:
                        case FIXConstants.SIDE_Sell_Closed:
                        case FIXConstants.SIDE_Sell_Open:
                        case FIXConstants.SIDE_SellPlus:
                        case FIXConstants.SIDE_SellShort:
                        case FIXConstants.SIDE_SellShortExempt:
                            // TODO: find fix tagvalues for the following sides and add the cases
                            //case FIXConstants.SIDE_CrossShortExempt:
                            //case FIXConstants.SIDE_Opposite:
                            band.ColumnFilters["OrderSide"].FilterConditions.Add(FilterComparisionOperator.Equals, cmbOrderSide.Text);
                            bandGrouped.ColumnFilters["OrderSide"].FilterConditions.Add(FilterComparisionOperator.Equals, cmbOrderSide.Text);
                            break;
                        default:

                            break;
                    }
                }
                
                UnCheckAllRows(grdUnallocated, headerCheckBoxUnallocated);
                UnCheckAllRows(grdGrouped, headerCheckBoxGrouped);
            }

        }
        public void gridRefresh()
        {
            grdUnallocated.Refresh();
            grdAllocated.Refresh();
            grdAllocatedBasket.Refresh();
            grdUnAllocatedBasket.Refresh();
           
        }
        public void SetDataSourceCollections()
        {
            _unAllocatedBaskets = _basketAllocationManager.UnallocatedBaskets;
            _groupedBaskets = _basketAllocationManager.Groupedbaskets;
            _allocatedGroupedBaskets = _basketAllocationManager.AllocatedBasketGroups;

            _orders = _orderAllocationManager.Orders;
            _groups = _orderAllocationManager.Groups;
            _allocatedGroups = _orderAllocationManager.AllocatedGroups;
        }
       
        
    }
}
