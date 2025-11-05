using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization ;
using System.IO;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Interfaces;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Utilities.StringUtilities;
using Prana.Allocation.BLL;
namespace Prana.Allocation
{
	/// <summary>
	/// Summary description for AllocationPreferences.
	/// </summary>
	public class AllocationPreferencesUserControl : System.Windows.Forms.UserControl,IPreferences
	{
		
		#region Forms Elements
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tlbPreferences;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraCheckEditor8;
		private System.Windows.Forms.Label Auto;
        private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdDefaultFunds;
		private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxVenue;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblIntegrateFundStrategy;
		private System.Windows.Forms.Label lblAvgPricing;
		private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxIntegrateFundStrategy;
		private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxAvgPricing;
		private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxRoundLot;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorExeLessTotalText;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorUnAllocateText;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorExeLessTotalBack;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorUnAllocateBack;
		private System.Windows.Forms.Label lblTradingAcc;
		private System.Windows.Forms.Label lblBuyBCV;
		private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxTradingAcc;
		private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxBuyBCV;
		private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxCounterParty;
		private System.Windows.Forms.Button btnAddNewFund;
		private System.Windows.Forms.Button btnAddNewStrategy;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdDefaultStrategies;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tlbAllocationDefaults;
		private System.Windows.Forms.Label label3;
		private System.ComponentModel.Container components = null;

		#endregion
		
		#region Private Members

		private AllocationPreferences    _allocationPreferences;		
		private Defaults _strategydefaults;
		private Prana.Allocation.Controls.FundStrategyControl uctFundStrategy;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageFundAllocation;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageStrategyAllocation;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageFundStrategy;
		private Defaults _fundsdefaults;
		//private Defaults _addedfundsdefaults= new Defaults();
		//private Defaults _addedStrategydefaults= new Defaults();
		//private string _deletedFundDefaultIDS=string.Empty;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorAllocateEqualTotalBack;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorAllocateEqualTotalText;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorAllocateLessTotalText;
		private System.Windows.Forms.Label label2;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorAllocateLessTotalBack;
		//private string _deletedStrategyDefaultIDS=string.Empty;
        private bool isInitialized;
		private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
		private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
		private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox4;
        //private string FORM_NAME="AllocationPreferenceUserControl";
        private Prana.BusinessObjects.CompanyUser _loginUser;
		private System.Windows.Forms.Label label8;
		private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorSelectedRowTextColor;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker ColorSelectedRowBackColor;
        private System.Windows.Forms.GroupBox groupBox2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Prana.Allocation.Controls.ClearanceControl clearanceControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage4;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl10;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl11;
        private AllocationColumns fundColumns;
        private AllocationColumns strategyColumns;
		
		#endregion
		
		
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		

		#region Constructors
		
		public AllocationPreferencesUserControl()
		{
			//
			// Required for Windows Form Designer support
			//
		
			InitializeComponent();
			
			SetUp();

			

		}
		public void SetUp()
		{
			try
			{
               
			}
			catch(Exception ex)
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
        private void RowAppreanceSettings(UltraGrid grid)
        {
            grid.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.FromArgb(255, 250, 205);
            grid.DisplayLayout.Override.RowAppearance.BackColor = Color.White;
            grid.DisplayLayout.Override.RowAppearance.BackColor2 = Color.White;
            grid.DisplayLayout.Override.RowAppearance.ForeColor = Color.Black;
        }
        private void LinkDisplaySetting(UltraGrid grid)
        {
            //grid.DisplayLayout.Bands[0].Columns.Add("Modify", "Modify");
            //grid.DisplayLayout.Bands[0].Columns.Add("Delete","Delete");
            //grid.DisplayLayout.Bands[0].Columns["Modify"].CellActivation = Activation.NoEdit;
            //grid.DisplayLayout.Bands[0].Columns["Delete"].CellActivation = Activation.NoEdit;
            grid.DisplayLayout.Bands[0].Columns["Modify"].CellAppearance.Cursor = Cursors.Hand;
            grid.DisplayLayout.Bands[0].Columns["Modify"].CellAppearance.ForeColor = Color.FromArgb(0, 0, 255);
            grid.DisplayLayout.Bands[0].Columns["Modify"].CellAppearance.FontData.Underline = DefaultableBoolean.True;
            grid.DisplayLayout.Bands[0].Columns["Delete"].CellAppearance.Cursor = Cursors.Hand;
            grid.DisplayLayout.Bands[0].Columns["Delete"].CellAppearance.ForeColor = Color.Red;
            grid.DisplayLayout.Bands[0].Columns["Delete"].CellAppearance.FontData.Underline = DefaultableBoolean.True;
        }

		#endregion
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

		#region DataBinding
		private void BindFundDefaults()
		{
		
			_fundsdefaults=FundManager.GetFundDefaults(_loginUser.CompanyUserID );
            //grdDefaultFunds.DataSource = null;
			grdDefaultFunds.DataSource=_fundsdefaults;
			grdDefaultFunds.DataBind();
			

		}
		private void BindStrategyDefaults()
		{
		
			_strategydefaults=CompanyStrategyManager.GetStrategyDefaults(_loginUser.CompanyUserID);
            //grdDefaultStrategies.DataSource = null;
			grdDefaultStrategies.DataSource=_strategydefaults;
			grdDefaultStrategies.DataBind();
			

		}

		private void ReBindFundDefaults()
		{
		  	//grdDefaultFunds.DataSource=null;
			grdDefaultFunds.DataSource=_fundsdefaults;
			//grdDefaultFunds.Refresh();

			grdDefaultFunds.DataBind();			
			GridSettings(grdDefaultFunds);
			
		
		}
		private void ReBindStrategyDefaults()
		{
            //grdDefaultStrategies.DataSource = null;
			grdDefaultStrategies.DataSource=_strategydefaults;
			grdDefaultStrategies.DataBind();
			GridSettings(grdDefaultStrategies);
			
		
		}
		
		
		private void GridSettings(object objGrid)
		{
			
			UltraGrid objWinGrid=(UltraGrid)objGrid;
			ColumnsCollection columns = objWinGrid.DisplayLayout.Bands[0].Columns;
			
			
				if(!objWinGrid.DisplayLayout.Bands[0].Columns.Exists("DefaultName"))
				objWinGrid.DisplayLayout.Bands[0].Columns.Add("DefaultName");
			
			
			foreach (UltraGridColumn column in columns)
			{
				if(column.Key=="DefaultName")
				column.Header.VisiblePosition=0;
				if(column.Key=="Modify")
					column.Header.VisiblePosition=1;
				if(column.Key=="Delete")
					column.Header.VisiblePosition=2;
				if( !( column.Key == "DefaultName" || column.Key == "Modify" || column.Key =="Delete" ) )
				{
					column.Hidden = true;
				}
				if( column.Key == "DefaultName")
					column.CellActivation=Activation.ActivateOnly;
			}

			

		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllocationPreferencesUserControl));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Delete", 0, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Modify", 1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Modify", 0, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Delete", 1);
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab7 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab8 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab9 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab10 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab11 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl10 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.fundColumns = new Prana.Allocation.AllocationColumns();
            this.ultraTabPageControl11 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.strategyColumns = new Prana.Allocation.AllocationColumns();
            this.tabPageFundAllocation = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnAddNewFund = new System.Windows.Forms.Button();
            this.grdDefaultFunds = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabPageStrategyAllocation = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.btnAddNewStrategy = new System.Windows.Forms.Button();
            this.grdDefaultStrategies = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabPageFundStrategy = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.uctFundStrategy = new Prana.Allocation.Controls.FundStrategyControl();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkbxRoundLot = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblAvgPricing = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkbxIntegrateFundStrategy = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkbxAvgPricing = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblIntegrateFundStrategy = new System.Windows.Forms.Label();
            this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage4 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.label8 = new System.Windows.Forms.Label();
            this.ColorSelectedRowTextColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ColorSelectedRowBackColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.ColorAllocateLessTotalText = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.label2 = new System.Windows.Forms.Label();
            this.ColorAllocateLessTotalBack = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.label12 = new System.Windows.Forms.Label();
            this.ColorAllocateEqualTotalText = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ColorAllocateEqualTotalBack = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.ColorUnAllocateBack = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ColorUnAllocateText = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ColorExeLessTotalBack = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.ColorExeLessTotalText = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraGroupBox4 = new Infragistics.Win.Misc.UltraGroupBox();
            this.chkbxCounterParty = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.label3 = new System.Windows.Forms.Label();
            this.Auto = new System.Windows.Forms.Label();
            this.ultraCheckEditor8 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblTradingAcc = new System.Windows.Forms.Label();
            this.lblBuyBCV = new System.Windows.Forms.Label();
            this.chkbxTradingAcc = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.label4 = new System.Windows.Forms.Label();
            this.chkbxBuyBCV = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.chkbxVenue = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tlbAllocationDefaults = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.clearanceControl1 = new Prana.Allocation.Controls.ClearanceControl();
            this.tlbPreferences = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraTabPageControl10.SuspendLayout();
            this.ultraTabPageControl11.SuspendLayout();
            this.tabPageFundAllocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDefaultFunds)).BeginInit();
            this.tabPageStrategyAllocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDefaultStrategies)).BeginInit();
            this.tabPageFundStrategy.SuspendLayout();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.ultraTabPageControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl2)).BeginInit();
            this.ultraTabControl2.SuspendLayout();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorSelectedRowTextColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorSelectedRowBackColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateLessTotalText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateLessTotalBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateEqualTotalText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateEqualTotalBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorUnAllocateBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorUnAllocateText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorExeLessTotalBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorExeLessTotalText)).BeginInit();
            this.ultraTabPageControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).BeginInit();
            this.ultraGroupBox4.SuspendLayout();
            this.ultraTabPageControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlbAllocationDefaults)).BeginInit();
            this.tlbAllocationDefaults.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlbPreferences)).BeginInit();
            this.tlbPreferences.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl10
            // 
            this.ultraTabPageControl10.Controls.Add(this.fundColumns);
            this.ultraTabPageControl10.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl10.Name = "ultraTabPageControl10";
            this.ultraTabPageControl10.Size = new System.Drawing.Size(520, 321);
            // 
            // fundColumns
            // 
            this.fundColumns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.fundColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fundColumns.Location = new System.Drawing.Point(0, 0);
            this.fundColumns.Name = "fundColumns";
            this.fundColumns.Size = new System.Drawing.Size(520, 321);
            this.fundColumns.TabIndex = 1;
            // 
            // ultraTabPageControl11
            // 
            this.ultraTabPageControl11.Controls.Add(this.strategyColumns);
            this.ultraTabPageControl11.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl11.Name = "ultraTabPageControl11";
            this.ultraTabPageControl11.Size = new System.Drawing.Size(520, 321);
            // 
            // strategyColumns
            // 
            this.strategyColumns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.strategyColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.strategyColumns.Location = new System.Drawing.Point(0, 0);
            this.strategyColumns.Name = "strategyColumns";
            this.strategyColumns.Size = new System.Drawing.Size(520, 321);
            this.strategyColumns.TabIndex = 0;
            // 
            // tabPageFundAllocation
            // 
            this.tabPageFundAllocation.Controls.Add(this.btnAddNewFund);
            this.tabPageFundAllocation.Controls.Add(this.grdDefaultFunds);
            this.tabPageFundAllocation.Location = new System.Drawing.Point(1, 20);
            this.tabPageFundAllocation.Name = "tabPageFundAllocation";
            this.tabPageFundAllocation.Size = new System.Drawing.Size(520, 321);
            // 
            // btnAddNewFund
            // 
            this.btnAddNewFund.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddNewFund.BackColor = System.Drawing.Color.LightCyan;
            this.btnAddNewFund.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNewFund.Image")));
            this.btnAddNewFund.Location = new System.Drawing.Point(218, 289);
            this.btnAddNewFund.Name = "btnAddNewFund";
            this.btnAddNewFund.Size = new System.Drawing.Size(78, 26);
            this.btnAddNewFund.TabIndex = 6;
            this.btnAddNewFund.UseVisualStyleBackColor = false;
            this.btnAddNewFund.Click += new System.EventHandler(this.btnAddNewFund_Click);
            // 
            // grdDefaultFunds
            // 
            this.grdDefaultFunds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "Tahoma";
            appearance1.FontData.SizeInPoints = 8.25F;
            this.grdDefaultFunds.DisplayLayout.Appearance = appearance1;
            this.grdDefaultFunds.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.NullText = "Delete";
            ultraGridColumn1.Width = 256;
            ultraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.NullText = "Modify";
            ultraGridColumn2.Width = 256;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.grdDefaultFunds.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdDefaultFunds.DisplayLayout.GroupByBox.Hidden = true;
            this.grdDefaultFunds.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDefaultFunds.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdDefaultFunds.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.Yes;
            this.grdDefaultFunds.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdDefaultFunds.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdDefaultFunds.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdDefaultFunds.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdDefaultFunds.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "Tahoma";
            appearance2.FontData.SizeInPoints = 8.25F;
            this.grdDefaultFunds.DisplayLayout.Override.HeaderAppearance = appearance2;
            this.grdDefaultFunds.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdDefaultFunds.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdDefaultFunds.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdDefaultFunds.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdDefaultFunds.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdDefaultFunds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdDefaultFunds.Location = new System.Drawing.Point(2, 0);
            this.grdDefaultFunds.Name = "grdDefaultFunds";
            this.grdDefaultFunds.Size = new System.Drawing.Size(514, 285);
            this.grdDefaultFunds.TabIndex = 5;
            this.grdDefaultFunds.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdDefaultFunds.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdDefaultFunds_MouseUp);
            // 
            // tabPageStrategyAllocation
            // 
            this.tabPageStrategyAllocation.Controls.Add(this.btnAddNewStrategy);
            this.tabPageStrategyAllocation.Controls.Add(this.grdDefaultStrategies);
            this.tabPageStrategyAllocation.Location = new System.Drawing.Point(-10000, -10000);
            this.tabPageStrategyAllocation.Name = "tabPageStrategyAllocation";
            this.tabPageStrategyAllocation.Size = new System.Drawing.Size(520, 321);
            // 
            // btnAddNewStrategy
            // 
            this.btnAddNewStrategy.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddNewStrategy.BackColor = System.Drawing.Color.LightCyan;
            this.btnAddNewStrategy.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNewStrategy.Image")));
            this.btnAddNewStrategy.Location = new System.Drawing.Point(218, 291);
            this.btnAddNewStrategy.Name = "btnAddNewStrategy";
            this.btnAddNewStrategy.Size = new System.Drawing.Size(78, 26);
            this.btnAddNewStrategy.TabIndex = 8;
            this.btnAddNewStrategy.UseVisualStyleBackColor = false;
            this.btnAddNewStrategy.Click += new System.EventHandler(this.btnAddNewStrategy_Click);
            // 
            // grdDefaultStrategies
            // 
            this.grdDefaultStrategies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.FontData.BoldAsString = "False";
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8.25F;
            this.grdDefaultStrategies.DisplayLayout.Appearance = appearance3;
            this.grdDefaultStrategies.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn3.NullText = "Modify";
            ultraGridColumn3.Width = 252;
            ultraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridColumn4.NullText = "Delete";
            ultraGridColumn4.Width = 266;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4});
            this.grdDefaultStrategies.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdDefaultStrategies.DisplayLayout.CaptionAppearance = appearance4;
            this.grdDefaultStrategies.DisplayLayout.GroupByBox.Hidden = true;
            this.grdDefaultStrategies.DisplayLayout.MaxColScrollRegions = 1;
            this.grdDefaultStrategies.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdDefaultStrategies.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.grdDefaultStrategies.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdDefaultStrategies.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.Name = "Tahoma";
            appearance5.FontData.SizeInPoints = 8.25F;
            this.grdDefaultStrategies.DisplayLayout.Override.HeaderAppearance = appearance5;
            this.grdDefaultStrategies.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdDefaultStrategies.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdDefaultStrategies.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdDefaultStrategies.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdDefaultStrategies.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdDefaultStrategies.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdDefaultStrategies.Location = new System.Drawing.Point(0, 0);
            this.grdDefaultStrategies.Name = "grdDefaultStrategies";
            this.grdDefaultStrategies.Size = new System.Drawing.Size(520, 285);
            this.grdDefaultStrategies.TabIndex = 7;
            this.grdDefaultStrategies.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdDefaultStrategies.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdDefaultStrategies_MouseUp);
            // 
            // tabPageFundStrategy
            // 
            this.tabPageFundStrategy.Controls.Add(this.uctFundStrategy);
            this.tabPageFundStrategy.Location = new System.Drawing.Point(-10000, -10000);
            this.tabPageFundStrategy.Name = "tabPageFundStrategy";
            this.tabPageFundStrategy.Size = new System.Drawing.Size(520, 321);
            // 
            // uctFundStrategy
            // 
            this.uctFundStrategy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.uctFundStrategy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctFundStrategy.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.uctFundStrategy.Location = new System.Drawing.Point(0, 0);
            this.uctFundStrategy.Name = "uctFundStrategy";
            this.uctFundStrategy.Size = new System.Drawing.Size(520, 321);
            this.uctFundStrategy.TabIndex = 0;
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.ultraGroupBox3);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(522, 342);
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox3.Controls.Add(this.groupBox2);
            this.ultraGroupBox3.Location = new System.Drawing.Point(8, 12);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(504, 323);
            this.ultraGroupBox3.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkbxRoundLot);
            this.groupBox2.Controls.Add(this.lblAvgPricing);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.chkbxIntegrateFundStrategy);
            this.groupBox2.Controls.Add(this.chkbxAvgPricing);
            this.groupBox2.Controls.Add(this.lblIntegrateFundStrategy);
            this.groupBox2.Location = new System.Drawing.Point(66, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(322, 100);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rules";
            // 
            // chkbxRoundLot
            // 
            this.chkbxRoundLot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxRoundLot.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxRoundLot.Enabled = false;
            this.chkbxRoundLot.Location = new System.Drawing.Point(47, 14);
            this.chkbxRoundLot.Name = "chkbxRoundLot";
            this.chkbxRoundLot.Size = new System.Drawing.Size(14, 13);
            this.chkbxRoundLot.TabIndex = 0;
            this.chkbxRoundLot.Text = "ultraCheckEditor1";
            // 
            // lblAvgPricing
            // 
            this.lblAvgPricing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblAvgPricing.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAvgPricing.Location = new System.Drawing.Point(81, 42);
            this.lblAvgPricing.Name = "lblAvgPricing";
            this.lblAvgPricing.Size = new System.Drawing.Size(194, 23);
            this.lblAvgPricing.TabIndex = 4;
            this.lblAvgPricing.Text = "Average Pricing Allowed";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(81, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Apply Round Lot Rules";
            // 
            // chkbxIntegrateFundStrategy
            // 
            this.chkbxIntegrateFundStrategy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxIntegrateFundStrategy.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxIntegrateFundStrategy.Location = new System.Drawing.Point(47, 78);
            this.chkbxIntegrateFundStrategy.Name = "chkbxIntegrateFundStrategy";
            this.chkbxIntegrateFundStrategy.Size = new System.Drawing.Size(14, 13);
            this.chkbxIntegrateFundStrategy.TabIndex = 2;
            this.chkbxIntegrateFundStrategy.Text = "ultraCheckEditor3";
            this.chkbxIntegrateFundStrategy.Visible = false;
            // 
            // chkbxAvgPricing
            // 
            this.chkbxAvgPricing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxAvgPricing.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxAvgPricing.Enabled = false;
            this.chkbxAvgPricing.Location = new System.Drawing.Point(47, 48);
            this.chkbxAvgPricing.Name = "chkbxAvgPricing";
            this.chkbxAvgPricing.Size = new System.Drawing.Size(14, 13);
            this.chkbxAvgPricing.TabIndex = 1;
            this.chkbxAvgPricing.Text = "ultraCheckEditor2";
            // 
            // lblIntegrateFundStrategy
            // 
            this.lblIntegrateFundStrategy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblIntegrateFundStrategy.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblIntegrateFundStrategy.Location = new System.Drawing.Point(81, 74);
            this.lblIntegrateFundStrategy.Name = "lblIntegrateFundStrategy";
            this.lblIntegrateFundStrategy.Size = new System.Drawing.Size(194, 23);
            this.lblIntegrateFundStrategy.TabIndex = 5;
            this.lblIntegrateFundStrategy.Text = "Integrate AllocationFund & Strategy Bundling";
            this.lblIntegrateFundStrategy.Visible = false;
            // 
            // ultraTabPageControl6
            // 
            this.ultraTabPageControl6.Controls.Add(this.ultraTabControl2);
            this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl6.Name = "ultraTabPageControl6";
            this.ultraTabPageControl6.Size = new System.Drawing.Size(522, 342);
            // 
            // ultraTabControl2
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.ultraTabControl2.ActiveTabAppearance = appearance6;
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance7.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.ultraTabControl2.Appearance = appearance7;
            appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance8.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.ultraTabControl2.ClientAreaAppearance = appearance8;
            this.ultraTabControl2.Controls.Add(this.ultraTabSharedControlsPage4);
            this.ultraTabControl2.Controls.Add(this.ultraTabPageControl10);
            this.ultraTabControl2.Controls.Add(this.ultraTabPageControl11);
            this.ultraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl2.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl2.Name = "ultraTabControl2";
            this.ultraTabControl2.SharedControlsPage = this.ultraTabSharedControlsPage4;
            this.ultraTabControl2.Size = new System.Drawing.Size(522, 342);
            this.ultraTabControl2.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.ultraTabControl2.TabIndex = 8;
            ultraTab1.TabPage = this.ultraTabPageControl10;
            ultraTab1.Text = "AllocationFund Columns";
            ultraTab2.TabPage = this.ultraTabPageControl11;
            ultraTab2.Text = "Strategy Columns";
            this.ultraTabControl2.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this.ultraTabControl2.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // ultraTabSharedControlsPage4
            // 
            this.ultraTabSharedControlsPage4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage4.Name = "ultraTabSharedControlsPage4";
            this.ultraTabSharedControlsPage4.Size = new System.Drawing.Size(520, 321);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this.label8);
            this.ultraTabPageControl3.Controls.Add(this.ColorSelectedRowTextColor);
            this.ultraTabPageControl3.Controls.Add(this.ColorSelectedRowBackColor);
            this.ultraTabPageControl3.Controls.Add(this.ultraGroupBox2);
            this.ultraTabPageControl3.Controls.Add(this.ultraGroupBox1);
            this.ultraTabPageControl3.Controls.Add(this.label6);
            this.ultraTabPageControl3.Controls.Add(this.label5);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(522, 342);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(36, 227);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(144, 19);
            this.label8.TabIndex = 24;
            this.label8.Text = "Selected Row Color";
            // 
            // ColorSelectedRowTextColor
            // 
            this.ColorSelectedRowTextColor.AllowEmpty = false;
            this.ColorSelectedRowTextColor.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance9.BorderColor = System.Drawing.Color.Black;
            appearance9.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorSelectedRowTextColor.Appearance = appearance9;
            this.ColorSelectedRowTextColor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorSelectedRowTextColor.ButtonAppearance = appearance10;
            this.ColorSelectedRowTextColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorSelectedRowTextColor.Color = System.Drawing.Color.Black;
            this.ColorSelectedRowTextColor.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorSelectedRowTextColor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorSelectedRowTextColor.Location = new System.Drawing.Point(358, 227);
            this.ColorSelectedRowTextColor.Name = "ColorSelectedRowTextColor";
            this.ColorSelectedRowTextColor.Size = new System.Drawing.Size(115, 20);
            this.ColorSelectedRowTextColor.TabIndex = 25;
            this.ColorSelectedRowTextColor.Text = "Black";
            this.ColorSelectedRowTextColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ColorSelectedRowBackColor
            // 
            this.ColorSelectedRowBackColor.AllowEmpty = false;
            this.ColorSelectedRowBackColor.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance11.BorderColor = System.Drawing.Color.Black;
            appearance11.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorSelectedRowBackColor.Appearance = appearance11;
            this.ColorSelectedRowBackColor.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorSelectedRowBackColor.ButtonAppearance = appearance12;
            this.ColorSelectedRowBackColor.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorSelectedRowBackColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ColorSelectedRowBackColor.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorSelectedRowBackColor.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorSelectedRowBackColor.Location = new System.Drawing.Point(214, 227);
            this.ColorSelectedRowBackColor.Name = "ColorSelectedRowBackColor";
            this.ColorSelectedRowBackColor.Size = new System.Drawing.Size(115, 20);
            this.ColorSelectedRowBackColor.TabIndex = 23;
            this.ColorSelectedRowBackColor.Text = "192, 255, 192";
            this.ColorSelectedRowBackColor.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox2.Controls.Add(this.ColorAllocateLessTotalText);
            this.ultraGroupBox2.Controls.Add(this.label2);
            this.ultraGroupBox2.Controls.Add(this.ColorAllocateLessTotalBack);
            this.ultraGroupBox2.Controls.Add(this.label12);
            this.ultraGroupBox2.Controls.Add(this.ColorAllocateEqualTotalText);
            this.ultraGroupBox2.Controls.Add(this.ColorAllocateEqualTotalBack);
            this.ultraGroupBox2.Location = new System.Drawing.Point(3, 115);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(520, 81);
            this.ultraGroupBox2.TabIndex = 22;
            this.ultraGroupBox2.Text = "Allocated Row Colors";
            // 
            // ColorAllocateLessTotalText
            // 
            this.ColorAllocateLessTotalText.AllowEmpty = false;
            this.ColorAllocateLessTotalText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorAllocateLessTotalText.Appearance = appearance13;
            this.ColorAllocateLessTotalText.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorAllocateLessTotalText.ButtonAppearance = appearance14;
            this.ColorAllocateLessTotalText.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorAllocateLessTotalText.Color = System.Drawing.Color.Black;
            this.ColorAllocateLessTotalText.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorAllocateLessTotalText.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorAllocateLessTotalText.Location = new System.Drawing.Point(356, 39);
            this.ColorAllocateLessTotalText.Name = "ColorAllocateLessTotalText";
            this.ColorAllocateLessTotalText.Size = new System.Drawing.Size(115, 20);
            this.ColorAllocateLessTotalText.TabIndex = 20;
            this.ColorAllocateLessTotalText.Text = "Black";
            this.ColorAllocateLessTotalText.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(36, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 19);
            this.label2.TabIndex = 19;
            this.label2.Text = "Allocated < Quantity";
            // 
            // ColorAllocateLessTotalBack
            // 
            this.ColorAllocateLessTotalBack.AllowEmpty = false;
            this.ColorAllocateLessTotalBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance15.BorderColor = System.Drawing.Color.Black;
            appearance15.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorAllocateLessTotalBack.Appearance = appearance15;
            this.ColorAllocateLessTotalBack.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorAllocateLessTotalBack.ButtonAppearance = appearance16;
            this.ColorAllocateLessTotalBack.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorAllocateLessTotalBack.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ColorAllocateLessTotalBack.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorAllocateLessTotalBack.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorAllocateLessTotalBack.Location = new System.Drawing.Point(212, 41);
            this.ColorAllocateLessTotalBack.Name = "ColorAllocateLessTotalBack";
            this.ColorAllocateLessTotalBack.Size = new System.Drawing.Size(115, 20);
            this.ColorAllocateLessTotalBack.TabIndex = 18;
            this.ColorAllocateLessTotalBack.Text = "255, 192, 192";
            this.ColorAllocateLessTotalBack.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label12.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label12.Location = new System.Drawing.Point(36, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(144, 19);
            this.label12.TabIndex = 9;
            this.label12.Text = "AllocatedQty=Quantity";
            // 
            // ColorAllocateEqualTotalText
            // 
            this.ColorAllocateEqualTotalText.AllowEmpty = false;
            this.ColorAllocateEqualTotalText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance21.BorderColor = System.Drawing.Color.Black;
            appearance21.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorAllocateEqualTotalText.Appearance = appearance21;
            this.ColorAllocateEqualTotalText.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorAllocateEqualTotalText.ButtonAppearance = appearance22;
            this.ColorAllocateEqualTotalText.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorAllocateEqualTotalText.Color = System.Drawing.Color.Black;
            this.ColorAllocateEqualTotalText.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorAllocateEqualTotalText.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorAllocateEqualTotalText.Location = new System.Drawing.Point(356, 17);
            this.ColorAllocateEqualTotalText.Name = "ColorAllocateEqualTotalText";
            this.ColorAllocateEqualTotalText.Size = new System.Drawing.Size(115, 20);
            this.ColorAllocateEqualTotalText.TabIndex = 16;
            this.ColorAllocateEqualTotalText.Text = "Black";
            this.ColorAllocateEqualTotalText.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ColorAllocateEqualTotalBack
            // 
            this.ColorAllocateEqualTotalBack.AllowEmpty = false;
            this.ColorAllocateEqualTotalBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance23.BorderColor = System.Drawing.Color.Black;
            appearance23.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorAllocateEqualTotalBack.Appearance = appearance23;
            this.ColorAllocateEqualTotalBack.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorAllocateEqualTotalBack.ButtonAppearance = appearance24;
            this.ColorAllocateEqualTotalBack.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorAllocateEqualTotalBack.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ColorAllocateEqualTotalBack.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorAllocateEqualTotalBack.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorAllocateEqualTotalBack.Location = new System.Drawing.Point(212, 17);
            this.ColorAllocateEqualTotalBack.Name = "ColorAllocateEqualTotalBack";
            this.ColorAllocateEqualTotalBack.Size = new System.Drawing.Size(115, 20);
            this.ColorAllocateEqualTotalBack.TabIndex = 3;
            this.ColorAllocateEqualTotalBack.Text = "192, 255, 192";
            this.ColorAllocateEqualTotalBack.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox1.Controls.Add(this.label9);
            this.ultraGroupBox1.Controls.Add(this.label13);
            this.ultraGroupBox1.Controls.Add(this.ColorUnAllocateBack);
            this.ultraGroupBox1.Controls.Add(this.ColorUnAllocateText);
            this.ultraGroupBox1.Controls.Add(this.ColorExeLessTotalBack);
            this.ultraGroupBox1.Controls.Add(this.ColorExeLessTotalText);
            this.ultraGroupBox1.Location = new System.Drawing.Point(2, 18);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(520, 80);
            this.ultraGroupBox1.TabIndex = 21;
            this.ultraGroupBox1.Text = "UnAllocated Row Colors";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(34, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(144, 19);
            this.label9.TabIndex = 6;
            this.label9.Text = "CumQty =Quantity";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label13.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label13.Location = new System.Drawing.Point(34, 40);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 19);
            this.label13.TabIndex = 10;
            this.label13.Text = "CumQty < Quantity";
            // 
            // ColorUnAllocateBack
            // 
            this.ColorUnAllocateBack.AllowEmpty = false;
            this.ColorUnAllocateBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance25.BorderColor = System.Drawing.Color.Black;
            appearance25.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorUnAllocateBack.Appearance = appearance25;
            this.ColorUnAllocateBack.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            appearance26.BorderColor = System.Drawing.Color.Black;
            this.ColorUnAllocateBack.ButtonAppearance = appearance26;
            this.ColorUnAllocateBack.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorUnAllocateBack.Color = System.Drawing.SystemColors.ActiveCaption;
            this.ColorUnAllocateBack.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorUnAllocateBack.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorUnAllocateBack.Location = new System.Drawing.Point(212, 18);
            this.ColorUnAllocateBack.Name = "ColorUnAllocateBack";
            this.ColorUnAllocateBack.Size = new System.Drawing.Size(115, 20);
            this.ColorUnAllocateBack.TabIndex = 0;
            this.ColorUnAllocateBack.Text = "ActiveCaption";
            this.ColorUnAllocateBack.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ColorUnAllocateText
            // 
            this.ColorUnAllocateText.AllowEmpty = false;
            this.ColorUnAllocateText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance27.BorderColor = System.Drawing.Color.Black;
            appearance27.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorUnAllocateText.Appearance = appearance27;
            this.ColorUnAllocateText.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorUnAllocateText.ButtonAppearance = appearance28;
            this.ColorUnAllocateText.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorUnAllocateText.Color = System.Drawing.Color.Black;
            this.ColorUnAllocateText.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorUnAllocateText.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorUnAllocateText.Location = new System.Drawing.Point(356, 20);
            this.ColorUnAllocateText.Name = "ColorUnAllocateText";
            this.ColorUnAllocateText.Size = new System.Drawing.Size(115, 20);
            this.ColorUnAllocateText.TabIndex = 13;
            this.ColorUnAllocateText.Text = "Black";
            this.ColorUnAllocateText.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ColorExeLessTotalBack
            // 
            this.ColorExeLessTotalBack.AllowEmpty = false;
            this.ColorExeLessTotalBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance29.BorderColor = System.Drawing.Color.Black;
            appearance29.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorExeLessTotalBack.Appearance = appearance29;
            this.ColorExeLessTotalBack.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance30.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorExeLessTotalBack.ButtonAppearance = appearance30;
            this.ColorExeLessTotalBack.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorExeLessTotalBack.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ColorExeLessTotalBack.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorExeLessTotalBack.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorExeLessTotalBack.Location = new System.Drawing.Point(212, 42);
            this.ColorExeLessTotalBack.Name = "ColorExeLessTotalBack";
            this.ColorExeLessTotalBack.Size = new System.Drawing.Size(115, 20);
            this.ColorExeLessTotalBack.TabIndex = 4;
            this.ColorExeLessTotalBack.Text = "255, 192, 192";
            this.ColorExeLessTotalBack.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // ColorExeLessTotalText
            // 
            this.ColorExeLessTotalText.AllowEmpty = false;
            this.ColorExeLessTotalText.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance31.BorderColor = System.Drawing.Color.Black;
            appearance31.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ColorExeLessTotalText.Appearance = appearance31;
            this.ColorExeLessTotalText.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance32.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ColorExeLessTotalText.ButtonAppearance = appearance32;
            this.ColorExeLessTotalText.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.ColorExeLessTotalText.Color = System.Drawing.Color.Black;
            this.ColorExeLessTotalText.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.OnMouseEnter;
            this.ColorExeLessTotalText.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ColorExeLessTotalText.Location = new System.Drawing.Point(356, 44);
            this.ColorExeLessTotalText.Name = "ColorExeLessTotalText";
            this.ColorExeLessTotalText.Size = new System.Drawing.Size(115, 20);
            this.ColorExeLessTotalText.TabIndex = 17;
            this.ColorExeLessTotalText.Text = "Black";
            this.ColorExeLessTotalText.ColorChanged += new System.EventHandler(this.ColorChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(358, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "TextColor";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(214, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "RowColor";
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this.ultraGroupBox4);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(522, 342);
            // 
            // ultraGroupBox4
            // 
            this.ultraGroupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox4.Controls.Add(this.chkbxCounterParty);
            this.ultraGroupBox4.Controls.Add(this.label3);
            this.ultraGroupBox4.Controls.Add(this.Auto);
            this.ultraGroupBox4.Controls.Add(this.ultraCheckEditor8);
            this.ultraGroupBox4.Controls.Add(this.lblTradingAcc);
            this.ultraGroupBox4.Controls.Add(this.lblBuyBCV);
            this.ultraGroupBox4.Controls.Add(this.chkbxTradingAcc);
            this.ultraGroupBox4.Controls.Add(this.label4);
            this.ultraGroupBox4.Controls.Add(this.chkbxBuyBCV);
            this.ultraGroupBox4.Controls.Add(this.chkbxVenue);
            this.ultraGroupBox4.Location = new System.Drawing.Point(6, 10);
            this.ultraGroupBox4.Name = "ultraGroupBox4";
            this.ultraGroupBox4.Size = new System.Drawing.Size(502, 323);
            this.ultraGroupBox4.TabIndex = 15;
            // 
            // chkbxCounterParty
            // 
            this.chkbxCounterParty.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxCounterParty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxCounterParty.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxCounterParty.Location = new System.Drawing.Point(140, 119);
            this.chkbxCounterParty.Name = "chkbxCounterParty";
            this.chkbxCounterParty.Size = new System.Drawing.Size(14, 14);
            this.chkbxCounterParty.TabIndex = 14;
            this.chkbxCounterParty.Text = "ultraCheckEditor6";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label3.Location = new System.Drawing.Point(190, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 23);
            this.label3.TabIndex = 13;
            this.label3.Text = "CounterParty";
            // 
            // Auto
            // 
            this.Auto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Auto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Auto.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Auto.Location = new System.Drawing.Point(90, 75);
            this.Auto.Name = "Auto";
            this.Auto.Size = new System.Drawing.Size(298, 23);
            this.Auto.TabIndex = 12;
            this.Auto.Text = "Auto AllocationGroup Orders with Same Side and Same Symbol ";
            // 
            // ultraCheckEditor8
            // 
            this.ultraCheckEditor8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ultraCheckEditor8.Checked = true;
            this.ultraCheckEditor8.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ultraCheckEditor8.Enabled = false;
            this.ultraCheckEditor8.Location = new System.Drawing.Point(38, 76);
            this.ultraCheckEditor8.Name = "ultraCheckEditor8";
            this.ultraCheckEditor8.Size = new System.Drawing.Size(16, 20);
            this.ultraCheckEditor8.TabIndex = 11;
            this.ultraCheckEditor8.Text = "ultraCheckEditor8";
            // 
            // lblTradingAcc
            // 
            this.lblTradingAcc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTradingAcc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblTradingAcc.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblTradingAcc.Location = new System.Drawing.Point(190, 209);
            this.lblTradingAcc.Name = "lblTradingAcc";
            this.lblTradingAcc.Size = new System.Drawing.Size(146, 23);
            this.lblTradingAcc.TabIndex = 9;
            this.lblTradingAcc.Text = "And Same Trading A/c";
            // 
            // lblBuyBCV
            // 
            this.lblBuyBCV.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBuyBCV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblBuyBCV.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.lblBuyBCV.Location = new System.Drawing.Point(190, 179);
            this.lblBuyBCV.Name = "lblBuyBCV";
            this.lblBuyBCV.Size = new System.Drawing.Size(146, 23);
            this.lblBuyBCV.TabIndex = 8;
            this.lblBuyBCV.Text = "And Buy + BCV";
            // 
            // chkbxTradingAcc
            // 
            this.chkbxTradingAcc.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxTradingAcc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradingAcc.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxTradingAcc.Location = new System.Drawing.Point(140, 213);
            this.chkbxTradingAcc.Name = "chkbxTradingAcc";
            this.chkbxTradingAcc.Size = new System.Drawing.Size(14, 14);
            this.chkbxTradingAcc.TabIndex = 5;
            this.chkbxTradingAcc.Text = "ultraCheckEditor4";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label4.Location = new System.Drawing.Point(190, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "And Same Venue";
            // 
            // chkbxBuyBCV
            // 
            this.chkbxBuyBCV.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxBuyBCV.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxBuyBCV.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxBuyBCV.Location = new System.Drawing.Point(140, 183);
            this.chkbxBuyBCV.Name = "chkbxBuyBCV";
            this.chkbxBuyBCV.Size = new System.Drawing.Size(14, 14);
            this.chkbxBuyBCV.TabIndex = 4;
            this.chkbxBuyBCV.Text = "ultraCheckEditor5";
            // 
            // chkbxVenue
            // 
            this.chkbxVenue.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkbxVenue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxVenue.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.chkbxVenue.Location = new System.Drawing.Point(140, 149);
            this.chkbxVenue.Name = "chkbxVenue";
            this.chkbxVenue.Size = new System.Drawing.Size(14, 14);
            this.chkbxVenue.TabIndex = 3;
            this.chkbxVenue.Text = "ultraCheckEditor6";
            // 
            // ultraTabPageControl5
            // 
            this.ultraTabPageControl5.Controls.Add(this.tlbAllocationDefaults);
            this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl5.Name = "ultraTabPageControl5";
            this.ultraTabPageControl5.Size = new System.Drawing.Size(522, 342);
            // 
            // tlbAllocationDefaults
            // 
            appearance37.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance37.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance37.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance37.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.tlbAllocationDefaults.ActiveTabAppearance = appearance37;
            appearance38.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance38.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.tlbAllocationDefaults.Appearance = appearance38;
            appearance39.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            appearance39.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.tlbAllocationDefaults.ClientAreaAppearance = appearance39;
            this.tlbAllocationDefaults.Controls.Add(this.ultraTabSharedControlsPage2);
            this.tlbAllocationDefaults.Controls.Add(this.tabPageFundAllocation);
            this.tlbAllocationDefaults.Controls.Add(this.tabPageStrategyAllocation);
            this.tlbAllocationDefaults.Controls.Add(this.tabPageFundStrategy);
            this.tlbAllocationDefaults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlbAllocationDefaults.Location = new System.Drawing.Point(0, 0);
            this.tlbAllocationDefaults.Name = "tlbAllocationDefaults";
            this.tlbAllocationDefaults.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.tlbAllocationDefaults.Size = new System.Drawing.Size(522, 342);
            this.tlbAllocationDefaults.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tlbAllocationDefaults.TabIndex = 0;
            ultraTab3.TabPage = this.tabPageFundAllocation;
            ultraTab3.Text = "FundAllocation";
            ultraTab4.TabPage = this.tabPageStrategyAllocation;
            ultraTab4.Text = "Strategy Allocation";
            ultraTab5.TabPage = this.tabPageFundStrategy;
            ultraTab5.Text = "FundStrategyMapping";
            ultraTab5.Visible = false;
            this.tlbAllocationDefaults.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab3,
            ultraTab4,
            ultraTab5});
            this.tlbAllocationDefaults.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(520, 321);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.clearanceControl1);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(522, 342);
            // 
            // clearanceControl1
            // 
            this.clearanceControl1.AutoSize = true;
            this.clearanceControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clearanceControl1.Location = new System.Drawing.Point(0, 0);
            this.clearanceControl1.Name = "clearanceControl1";
            this.clearanceControl1.Size = new System.Drawing.Size(522, 342);
            this.clearanceControl1.TabIndex = 0;
            // 
            // tlbPreferences
            // 
            appearance40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance40.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance40.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tlbPreferences.ActiveTabAppearance = appearance40;
            appearance41.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.tlbPreferences.Appearance = appearance41;
            this.tlbPreferences.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance42.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.tlbPreferences.ClientAreaAppearance = appearance42;
            this.tlbPreferences.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl1);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl3);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl4);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl5);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl2);
            this.tlbPreferences.Controls.Add(this.ultraTabPageControl6);
            this.tlbPreferences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlbPreferences.Location = new System.Drawing.Point(0, 0);
            this.tlbPreferences.Name = "tlbPreferences";
            this.tlbPreferences.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tlbPreferences.Size = new System.Drawing.Size(524, 363);
            this.tlbPreferences.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tlbPreferences.TabIndex = 0;
            appearance43.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            ultraTab6.Appearance = appearance43;
            ultraTab6.TabPage = this.ultraTabPageControl1;
            ultraTab6.Text = "General";
            ultraTab7.TabPage = this.ultraTabPageControl6;
            ultraTab7.Text = "Columns";
            appearance44.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            ultraTab8.Appearance = appearance44;
            ultraTab8.TabPage = this.ultraTabPageControl3;
            ultraTab8.Text = "Colors";
            appearance45.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            ultraTab9.Appearance = appearance45;
            ultraTab9.TabPage = this.ultraTabPageControl4;
            ultraTab9.Text = "Auto Grouping Rules";
            appearance46.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            ultraTab10.Appearance = appearance46;
            ultraTab10.TabPage = this.ultraTabPageControl5;
            ultraTab10.Text = "Allocation Defaults";
            ultraTab11.TabPage = this.ultraTabPageControl2;
            ultraTab11.Text = "Clearance";
            this.tlbPreferences.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab6,
            ultraTab7,
            ultraTab8,
            ultraTab9,
            ultraTab10,
            ultraTab11});
            this.tlbPreferences.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.AutoScroll = true;
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(522, 342);
            // 
            // AllocationPreferencesUserControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.tlbPreferences);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AllocationPreferencesUserControl";
            this.Size = new System.Drawing.Size(524, 363);
            this.ultraTabPageControl10.ResumeLayout(false);
            this.ultraTabPageControl11.ResumeLayout(false);
            this.tabPageFundAllocation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDefaultFunds)).EndInit();
            this.tabPageStrategyAllocation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDefaultStrategies)).EndInit();
            this.tabPageFundStrategy.ResumeLayout(false);
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ultraTabPageControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl2)).EndInit();
            this.ultraTabControl2.ResumeLayout(false);
            this.ultraTabPageControl3.ResumeLayout(false);
            this.ultraTabPageControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorSelectedRowTextColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorSelectedRowBackColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateLessTotalText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateLessTotalBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateEqualTotalText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorAllocateEqualTotalBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ColorUnAllocateBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorUnAllocateText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorExeLessTotalBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorExeLessTotalText)).EndInit();
            this.ultraTabPageControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).EndInit();
            this.ultraGroupBox4.ResumeLayout(false);
            this.ultraTabPageControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tlbAllocationDefaults)).EndInit();
            this.tlbAllocationDefaults.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlbPreferences)).EndInit();
            this.tlbPreferences.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion
		
		#region Preferences
		private void SetPreferences()
		{
			try
			{
				if(_allocationPreferences==null) return;
		
				#region General Rules
			
				if(_allocationPreferences.GeneralRules.ApplyRoundLotRules )
				{
					chkbxRoundLot.Checked=true;			
				}
				else
				{
					chkbxRoundLot.Checked=false;		

				}
				if(_allocationPreferences.GeneralRules.AveragePricingAllowed)
				{
					chkbxAvgPricing.Checked=true;
				}
				else
				{
					chkbxAvgPricing.Checked=false;		
				}
				if(_allocationPreferences.GeneralRules.IntegrateFundAndStrategyBundling)
				{
					chkbxIntegrateFundStrategy.Checked=true;
				}
				else
				{
					chkbxIntegrateFundStrategy.Checked=false;

				}

				

			
				#endregion
				#region Colors
				ColorUnAllocateBack.Color=Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedBackColor);
				ColorUnAllocateBack.Appearance.BackColor=ColorUnAllocateBack.Color;
				ColorUnAllocateBack.Appearance.ForeColor =ColorUnAllocateBack.Color;
				ColorUnAllocateBack.Appearance.BorderColor=ColorUnAllocateBack.Color;
				//ColorUnAllocateBack.Text=" ";

				ColorUnAllocateText.Color=Color.FromArgb(_allocationPreferences.RowProperties.UnAllocatedTextColor) ;
				ColorUnAllocateText.Appearance.BackColor= ColorUnAllocateText.Color;
				ColorUnAllocateText.Appearance.ForeColor= ColorUnAllocateText.Color;
				ColorUnAllocateText.Appearance.BorderColor = ColorUnAllocateText.Color;
				//ColorUnAllocateText.Text=" ";

			

				ColorAllocateEqualTotalBack.Color=Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyBackColor);
				ColorAllocateEqualTotalBack.Appearance.BackColor =ColorAllocateEqualTotalBack.Color;
				ColorAllocateEqualTotalBack.Appearance.ForeColor =ColorAllocateEqualTotalBack.Color;
				ColorAllocateEqualTotalBack.Appearance.BorderColor =ColorAllocateEqualTotalBack.Color;
				//ColorAllocateEqualTotalBack.Text =" ";

				ColorAllocateEqualTotalText.Color=Color.FromArgb(_allocationPreferences.RowProperties.AllocatedEqualTotalQtyTextColor) ;
				ColorAllocateEqualTotalText.Appearance.BackColor =ColorAllocateEqualTotalText.Color;
				ColorAllocateEqualTotalText.Appearance.ForeColor  =ColorAllocateEqualTotalText.Color;
				ColorAllocateEqualTotalText.Appearance.BorderColor =ColorAllocateEqualTotalText.Color;
				//ColorAllocateEqualTotalText.Text=" ";

				ColorExeLessTotalBack.Color=Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyBackColor);
				ColorExeLessTotalBack.Appearance.BackColor =ColorExeLessTotalBack.Color;
				ColorExeLessTotalBack.Appearance.ForeColor =ColorExeLessTotalBack.Color;
				ColorExeLessTotalBack.Appearance.BorderColor =ColorExeLessTotalBack.Color;
				//ColorExeLessTotalBack.Text=" ";

				ColorExeLessTotalText.Color=Color.FromArgb(_allocationPreferences.RowProperties.ExecutedLessTotalQtyTextColor);
				ColorExeLessTotalText.Appearance.BackColor =ColorExeLessTotalText.Color;
				ColorExeLessTotalText.Appearance.ForeColor =ColorExeLessTotalText.Color;
				ColorExeLessTotalText.Appearance.BorderColor =ColorExeLessTotalText.Color;
				//ColorExeLessTotalText.Text=" ";
				ColorAllocateLessTotalText.Color=Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyTextColor); 
				ColorAllocateLessTotalText.Appearance.BackColor=ColorAllocateLessTotalText.Color;
				ColorAllocateLessTotalText.Appearance.ForeColor=ColorAllocateLessTotalText.Color;
				ColorAllocateLessTotalText.Appearance.BorderColor=ColorAllocateLessTotalText.Color;

				ColorAllocateLessTotalBack.Color=Color.FromArgb(_allocationPreferences.RowProperties.AllocatedLessTotalQtyBackColor); 
				ColorAllocateLessTotalBack.Appearance.BackColor=ColorAllocateLessTotalBack.Color;
				ColorAllocateLessTotalBack.Appearance.ForeColor=ColorAllocateLessTotalBack.Color;
				ColorAllocateLessTotalBack.Appearance.BorderColor=ColorAllocateLessTotalBack.Color;

				ColorSelectedRowBackColor.Color=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
				ColorSelectedRowBackColor.Appearance.BackColor=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
				ColorSelectedRowBackColor.Appearance.ForeColor=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);
				ColorSelectedRowBackColor.Appearance.BorderColor=Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowBackColor);

				ColorSelectedRowTextColor.Color =Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
				ColorSelectedRowTextColor.Appearance.BackColor =Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
				ColorSelectedRowTextColor.Appearance.ForeColor =Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);
				ColorSelectedRowTextColor.Appearance.BorderColor =Color.FromArgb(_allocationPreferences.RowProperties.SelectedRowTextColor);

				#endregion
				isInitialized=true;
				#region AutoGrouping Rules
				if(_allocationPreferences.AutoGroupingRules.CounterParty)
					chkbxCounterParty.Checked=true;

				if(_allocationPreferences.AutoGroupingRules.Venue)
					chkbxVenue.Checked=true;
				if(_allocationPreferences.AutoGroupingRules.BuyAndBCV  )
					chkbxBuyBCV.Checked=true;
				
				if(_allocationPreferences.AutoGroupingRules.TradingAccount )
					chkbxTradingAcc.Checked=true;



				#endregion
		
				#region Columns
                fundColumns.SetUp(_allocationPreferences, PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
                strategyColumns.SetUp(_allocationPreferences, PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);
				#endregion
			}
			catch(Exception ex)
			{
				throw ex;
			}
			

		
		
		}
		private void  LoadPreferences()
		{
            _allocationPreferences = AllocationPreferencesManager.GetPreferences();			
		
		}
		
		#endregion

		#region Events
		private void grdDefaultFunds_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			UIElement objUIElement;
			UltraGridCell objUltraGridCell;		
			objUIElement = grdDefaultFunds.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
			if(objUIElement == null)
				return;
			objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
			if(objUltraGridCell == null)
				return;
		
			if(objUltraGridCell.Text  == "Modify" )
			{
				string defaultID=objUltraGridCell.Row.Cells["DefaultID"].Text;
				Default defaultEdit= _fundsdefaults.GetDefault(defaultID);
							
				int oldIndex=_fundsdefaults.IndexOf(defaultEdit);
				FundStrategiesDefaults fundStrategiesDefaults= new FundStrategiesDefaults(_loginUser.CompanyUserID,defaultEdit,true);
					fundStrategiesDefaults.ShowDialog(this);
					_fundsdefaults.Remove(defaultEdit);						
					_fundsdefaults.Insert(oldIndex,fundStrategiesDefaults.Default);
				
				
				
				
				ReBindFundDefaults();
				
			}

			if(objUltraGridCell.Text  == "Delete" )
			{
				if(MessageBox.Show(this, "Do you want to delete this Default?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.No)
					
				{
					ReBindFundDefaults();
					return;
				}
				string defaultID=objUltraGridCell.Row.Cells["DefaultID"].Text;
				Default defaultEdit= _fundsdefaults.GetDefault(defaultID);				
				grdDefaultFunds.ActiveRow.Delete(false);
				_fundsdefaults.Remove(defaultEdit);
				ReBindFundDefaults();
				
				
				
				
			}
		
		}

		private void grdDefaultStrategies_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			UIElement objUIElement;
			UltraGridCell objUltraGridCell;
			objUIElement = grdDefaultStrategies.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
			if(objUIElement == null)
				return;
			objUltraGridCell = (UltraGridCell)objUIElement.GetContext(typeof(UltraGridCell));
			if(objUltraGridCell == null)
				return;

            if (objUltraGridCell != null && objUltraGridCell.Text == "Modify")
			{
				string defaultID=objUltraGridCell.Row.Cells["DefaultID"].Text;
				Default defaultEdit= _strategydefaults.GetDefault(defaultID);
				int oldIndex=_strategydefaults.IndexOf(defaultEdit);
				

				FundStrategiesDefaults fundStrategiesDefaults= new FundStrategiesDefaults(_loginUser.CompanyUserID,defaultEdit,false);
				
				fundStrategiesDefaults.ShowDialog(this);
				_strategydefaults.Remove(defaultEdit);
				_strategydefaults.Insert(oldIndex,fundStrategiesDefaults.Default);
			
				ReBindStrategyDefaults();
			}

            if (objUltraGridCell!=null && objUltraGridCell.Text == "Delete")
			{
				if(MessageBox.Show(this, "Do you want to delete this Default?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.No)
					
				{
					ReBindStrategyDefaults();
					return;
				}
				string defaultID=objUltraGridCell.Row.Cells["DefaultID"].Text;
				Default defaultEdit= _strategydefaults.GetDefault(defaultID);				
//				if(_deletedStrategyDefaultIDS==string.Empty)
//					_deletedStrategyDefaultIDS=defaultID;
//				else
//					_deletedStrategyDefaultIDS=_deletedStrategyDefaultIDS+","+defaultID;
				grdDefaultStrategies.ActiveRow.Delete(false);
				_strategydefaults.Remove(defaultEdit);

				ReBindStrategyDefaults();
				
			}
		
		}
		private void btnAddNewFund_Click(object sender, System.EventArgs e)
		{

			FundStrategiesDefaults fundStrategiesDefaults= new FundStrategiesDefaults(_loginUser.CompanyUserID,true );
			fundStrategiesDefaults.ShowDialog(this);
			if(fundStrategiesDefaults.Default !=null)
			{
				_fundsdefaults.Add(fundStrategiesDefaults.Default);
				
			}
			ReBindFundDefaults();
			fundStrategiesDefaults=null;

		}

		private void btnAddNewStrategy_Click(object sender, System.EventArgs e)
		{
			FundStrategiesDefaults fundStrategiesDefaults= new FundStrategiesDefaults(_loginUser.CompanyUserID,false );
			fundStrategiesDefaults.ShowDialog(this);
			if(fundStrategiesDefaults.Default !=null)
			{
				_strategydefaults.Add(fundStrategiesDefaults.Default);
				
			}
			ReBindStrategyDefaults();
			fundStrategiesDefaults=null;
		}

		#endregion

		#region Color Changed Events
		private void ColorChanged(object sender , System.EventArgs e)
		{
			
            Infragistics.Win.UltraWinEditors.UltraColorPicker ultraColorPicker=(Infragistics.Win.UltraWinEditors.UltraColorPicker) sender;
			System.Drawing.Color selectedColor=ultraColorPicker.Color;
			ultraColorPicker.Appearance.BackColor= selectedColor;
			ultraColorPicker.Appearance.BorderColor= selectedColor;
			ultraColorPicker.Appearance.ForeColor= selectedColor;

		
		}


		#endregion

		#region IPreferences Members
		
		public bool Save()
		{
			try
			{

				_allocationPreferences= GetLatestPrefData(false);
				this.SaveAllocationPreferences();
				
				
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }

			}
			return true;
		}
        public void SetUp(Prana.BusinessObjects.CompanyUser user)
        {
            try
            {
                _loginUser = user;
                AllocationPreferencesManager.SetUp(_loginUser.CompanyUserID);
                LoadPreferences();
                SetPreferences();
                BindFundDefaults();
                BindStrategyDefaults();
                RowAppreanceSettings(grdDefaultFunds);
                RowAppreanceSettings(grdDefaultStrategies);
                LinkDisplaySetting(grdDefaultFunds);
                LinkDisplaySetting(grdDefaultStrategies);
                GridSettings(grdDefaultFunds);
                GridSettings(grdDefaultStrategies);
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
		public void RestoreDefault()
		{
            _allocationPreferences = AllocationPreferencesManager.GetDefualtPreferences();			
			SetPreferences();
		}
		
		public UserControl Reference()
		{
			return this;
		}
		public IPreferenceData GetPrefs()
		{
			AllocationPreferences	allocationPreferences=GetLatestPrefData(true);
			return allocationPreferences;
		}


        // we have more than one tab in the Livefeed preferences, so need to select a particular Tab from a particular module
        // so declare a property in the IPreference interface
        private string _modulename = string.Empty;
        public string SetModuleActive
        {
            set
            {
                _modulename = value;
                
            }
        }



		#endregion
	
		#region Properties

		public AllocationPreferences AllocationPreferences
		{
		
			get{return _allocationPreferences;}
			set
			{
				_allocationPreferences=value;
				
				
			}
			
				
				
		}
		
		
		#endregion

		#region Save Preferences
		
		public void SaveAllocationPreferences()
		{
			try
			{
				AllocationPreferencesManager.SavePreferences(_allocationPreferences);
				#region Saving FundStrategy Mapping Details 
                //FundStrategies  fundStrategies=uctFundStrategy.GetFundStrategies();
                //if(fundStrategies!=null)
                //    FundStraegyManager.SaveFundStrategy(fundStrategies);
                //else
                //{
                //    tlbPreferences.SelectedTab = tlbPreferences.Tabs[4];
                //    tlbAllocationDefaults.SelectedTab=tlbAllocationDefaults.Tabs[2];
                //    return;
                //}
				#endregion

				#region Saving FundDefaults
				FundManager.DeleteDefaults(_loginUser.CompanyUserID);
				FundManager.SaveDefaults(_loginUser.CompanyUserID,_fundsdefaults);
			
				#endregion

				#region StrategyDefaults
				CompanyStrategyManager.DeleteDefaults(_loginUser.CompanyUserID);
				CompanyStrategyManager.SaveDefaults(_loginUser.CompanyUserID,_strategydefaults);			
				
				#endregion

                #region SaveClearanceTimeTable

                clearanceControl1.SaveData();

                #endregion
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
		#endregion

       

		private  AllocationPreferences  GetLatestPrefData(bool Prompt)
		{
			AllocationPreferences allocationPref= new AllocationPreferences();
			try
			{
				

				#region General Rules
				if(chkbxRoundLot.Checked )
					allocationPref.GeneralRules.ApplyRoundLotRules=true;
				else
					allocationPref.GeneralRules.ApplyRoundLotRules=false;

				if(chkbxAvgPricing.Checked)
					allocationPref.GeneralRules.AveragePricingAllowed =true;
				else
					allocationPref.GeneralRules.AveragePricingAllowed =false;
			
				if(chkbxIntegrateFundStrategy.Checked)			
					allocationPref.GeneralRules.IntegrateFundAndStrategyBundling=true;
				else
					allocationPref.GeneralRules.IntegrateFundAndStrategyBundling=false;
			
				#endregion

				#region Colors
				allocationPref.RowProperties.UnAllocatedBackColor =ColorUnAllocateBack.Color.ToArgb();
				allocationPref.RowProperties.UnAllocatedTextColor =ColorUnAllocateText.Color.ToArgb();
				
				allocationPref.RowProperties.AllocatedEqualTotalQtyBackColor=ColorAllocateEqualTotalBack.Color.ToArgb();
				allocationPref.RowProperties.AllocatedEqualTotalQtyTextColor =ColorAllocateEqualTotalText.Color.ToArgb();

				allocationPref.RowProperties.AllocatedLessTotalQtyBackColor=ColorAllocateLessTotalBack.Color.ToArgb();
				allocationPref.RowProperties.AllocatedLessTotalQtyTextColor =ColorAllocateLessTotalText.Color.ToArgb();


				allocationPref.RowProperties.ExecutedLessTotalQtyBackColor=ColorExeLessTotalBack.Color.ToArgb();
				allocationPref.RowProperties.ExecutedLessTotalQtyTextColor=ColorExeLessTotalText.Color.ToArgb();
				allocationPref.RowProperties.SelectedRowBackColor =ColorSelectedRowBackColor.Color.ToArgb();
				allocationPref.RowProperties.SelectedRowTextColor =ColorSelectedRowTextColor.Color.ToArgb();
				#endregion

				#region AutoGrouping Rules
				if(chkbxCounterParty.Checked)
					allocationPref.AutoGroupingRules.CounterParty=true;
				else
					allocationPref.AutoGroupingRules.CounterParty=false;

				if(chkbxVenue.Checked)
					allocationPref.AutoGroupingRules.Venue=true;
				else
					allocationPref.AutoGroupingRules.Venue=false;
				if(chkbxBuyBCV.Checked )
					allocationPref.AutoGroupingRules.BuyAndBCV =true;
				
				if(chkbxTradingAcc.Checked )
					allocationPref.AutoGroupingRules.TradingAccount  =true;
				else
					allocationPref.AutoGroupingRules.TradingAccount  =false;



				#endregion
		
				#region Columns
                allocationPref.FundType.UnAllocatedGridColumns = fundColumns.UnAllocatedColumns.AllocationColumnPreferenceData;
                allocationPref.FundType.GroupedGridColumns = fundColumns.GroupedColumns.AllocationColumnPreferenceData;
                allocationPref.FundType.AllocatedGridColumns = fundColumns.AllocatedColumns.AllocationColumnPreferenceData;

                allocationPref.StrategyType.UnAllocatedGridColumns = strategyColumns.UnAllocatedColumns.AllocationColumnPreferenceData;
                allocationPref.StrategyType.GroupedGridColumns = strategyColumns.GroupedColumns.AllocationColumnPreferenceData;
                allocationPref.StrategyType.AllocatedGridColumns = strategyColumns.AllocatedColumns.AllocationColumnPreferenceData;
			
				#endregion	
				return allocationPref;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally 
			{
				
			}
		}











        #region IPreferences Members


        

        #endregion
    }
}
