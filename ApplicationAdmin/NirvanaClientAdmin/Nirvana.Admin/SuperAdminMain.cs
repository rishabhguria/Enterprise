#region Using

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Nirvana.Admin.BLL;
using Nirvana.Admin.RiskManagement;	

//using Nirvana.Admin.

#endregion

namespace Nirvana.Admin
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class SuperAdminMain : System.Windows.Forms.Form
	{
		#region Constant Definitions
		const int MNU_FILE = 0;
		
		const int MNU_FILE_NEW = 0;
		const int MNU_FILE_NEW_SLSU = 2;

		const int MNU_FILE_OPEN = 1;
		const int MNU_FILE_OPEN_AUEC = 0;
		const int MNU_FILE_OPEN_COUNTERPARTYVENUE = 5;
		const int MNU_FILE_OPEN_COMPANY = 7;

		const int MNU_TOOLS = 1;
		const int MNU_TOOLS_AUECMASTER = 0;
		const int MNU_TOOLS_AUECMASTER_AUEC = 1;

		const int MNU_TOOLS_COMPANY = 2;
		
		const int MNU_TOOLS_COUNTERPARTYVENUEMASTER = 3;
		const int MNU_TOOLS_COUNTERPARTYVENUE = 1;

		const int MNU_TOOLS_SLSU = 5;
		#endregion
		
		#region Private and Protected Members
		private System.Windows.Forms.MainMenu mnuAdmin;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuFileOpen;
		private System.Windows.Forms.MenuItem mnuFileExit;
		private System.Windows.Forms.MenuItem mnuTools;
		private System.Windows.Forms.MenuItem mnuWindow;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuHelpAboutNirvana;
		private System.Windows.Forms.MenuItem mnuFileOpenAUEC;

		private AUEC frmAUEC = null;
		private ExchangeForm frmExchange = null;
		private AssetDetails frmAssetDetails = null;
		private UnderlyingDetails frmUnderlyingDetails = null;

		private Side frmSide = null;
		private Nirvana.Admin.OrderType frmOrderType = null;
		private Nirvana.Admin.ExecutionInstruction frmExecutionInstruction = null;
		private Nirvana.Admin.HandlingInstruction frmHandlingInstruction = null;
		private Nirvana.Admin.TimeInForce frmTimeInForce = null;
		private Nirvana.Admin.Fix frmFix = null;
		private Nirvana.Admin.FixCapability frmFixCapability = null;

		private Nirvana.Admin.Unit frmUnit = null;
		private Nirvana.Admin.Identifier frmIdentifier = null;
		private Nirvana.Admin.Module frmModule = null;
		private Nirvana.Admin.VenueType frmVenueType = null;
		private Nirvana.Admin.CounterPartyType frmCounterPartyType = null;
		private Nirvana.Admin.SymbolConvention frmSymbolConvention = null;
		private Nirvana.Admin.ClearingFirmsPrimeBrokers frmClearingFirmsPrimeBrokers= null;

		private Nirvana.Admin.CurrencyType frmCurrencyType = null;
		private Nirvana.Admin.ContractListingType frmContractListingType = null;
		private Nirvana.Admin.FutureMonthCode frmFutureMonthCode = null;
		private Nirvana.Admin.Holiday frmHoliday = null;
		private Nirvana.Admin.NirvanaHelp frmNirvanaHelp = null;

		private Nirvana.Admin.RiskManagement.RMAdmin frmRiskManagement = null;
		private Nirvana.Admin.CommissionRules.CommissionRules frmCommissionRules = null;
		private System.Windows.Forms.MenuItem mnuFileNewAdminUserDetail;
		private System.Windows.Forms.MenuItem mnuFileOpenAsset;
		private System.Windows.Forms.MenuItem mnuFileOpenUnderlying;
		private System.Windows.Forms.MenuItem mnuFileOpenExchange;
		private System.Windows.Forms.MenuItem mnuFileOpenCurrency;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager toolbarsManager;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _SuperAdminMain_Toolbars_Dock_Area_Left;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _SuperAdminMain_Toolbars_Dock_Area_Right;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _SuperAdminMain_Toolbars_Dock_Area_Top;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _SuperAdminMain_Toolbars_Dock_Area_Bottom;
		private System.Windows.Forms.MenuItem mnuFileOpenCounterParty;
		private System.Windows.Forms.MenuItem mnuFileOpenVenue;
		private System.Windows.Forms.MenuItem mnuFileOpenCompany;
		private System.Windows.Forms.MenuItem mnuWindowCascade;
		private System.Windows.Forms.MenuItem mnuThirdParty;
		private System.Windows.Forms.MenuItem mnuFileNewFIXTAGS;
		private System.Windows.Forms.MenuItem mnuNewFIXTAGSSIDE;
		private System.Windows.Forms.MenuItem mnuNewFIXTAGSOrderType;
		private System.Windows.Forms.MenuItem mnuNewFIXTAGSExecutionInstruction;
		private System.Windows.Forms.MenuItem mnuNewFIXTAGSHandlingInstructions;
		private System.Windows.Forms.MenuItem mnuNewFIXTAGSTimeInForce;
		private System.Windows.Forms.MenuItem mnuFileNewUnit;
		private System.Windows.Forms.MenuItem mnuFileNewIdentifier;
		private System.Windows.Forms.MenuItem mnuNewFIXTAGSFix;
		private System.Windows.Forms.MenuItem mnuNewFIXTAGSFixCapability;
		private System.Windows.Forms.MenuItem mnuFileNewModule;
		private System.Windows.Forms.MenuItem mnuFileNewVenueType;
		private System.Windows.Forms.MenuItem mnuFileNewCounterPartyType;
		private System.Windows.Forms.MenuItem mnuFileNewClearingFirmPrimeBroker;
		private System.Windows.Forms.MenuItem mnuFileNewSymbolConvention;
		private System.Windows.Forms.MenuItem mnuFileNewContractListingType;
		private System.Windows.Forms.MenuItem mnuFileNewFutureMonthCodes;
		private System.Windows.Forms.MenuItem mnuFileOpenRiskManagement;
		private System.Windows.Forms.MenuItem mnuFileOpenCommissionRules;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem mnuFileNewHoliday;
		private System.Windows.Forms.MenuItem mnuFileNewCurrencyType;
		private System.ComponentModel.IContainer components;

		#endregion

		public SuperAdminMain()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
			Application.Exit();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("tlbNirvanaMain");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AUEC");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CounterPartyVenue");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Company");
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SuperAdminMain));
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Asset");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Underlying");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exchange");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Currency");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("User");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AUEC");
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Underlying");
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Currency");
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exchange");
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("User");
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Company");
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Asset");
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CounterPartyVenue");
			Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
			this.mnuAdmin = new System.Windows.Forms.MainMenu();
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuFileOpen = new System.Windows.Forms.MenuItem();
			this.mnuFileOpenCommissionRules = new System.Windows.Forms.MenuItem();
			this.mnuFileNewContractListingType = new System.Windows.Forms.MenuItem();
			this.mnuFileNewFutureMonthCodes = new System.Windows.Forms.MenuItem();
			this.mnuFileNewIdentifier = new System.Windows.Forms.MenuItem();
			this.mnuFileNewModule = new System.Windows.Forms.MenuItem();
			this.mnuFileOpenRiskManagement = new System.Windows.Forms.MenuItem();
			this.mnuFileNewSymbolConvention = new System.Windows.Forms.MenuItem();
			this.mnuFileNewUnit = new System.Windows.Forms.MenuItem();
			this.mnuFileOpenVenue = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.mnuFileExit = new System.Windows.Forms.MenuItem();
			this.mnuTools = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuFileOpenAsset = new System.Windows.Forms.MenuItem();
			this.mnuFileOpenAUEC = new System.Windows.Forms.MenuItem();
			this.mnuFileOpenCurrency = new System.Windows.Forms.MenuItem();
			this.mnuFileNewCurrencyType = new System.Windows.Forms.MenuItem();
			this.mnuFileOpenExchange = new System.Windows.Forms.MenuItem();
			this.mnuFileNewHoliday = new System.Windows.Forms.MenuItem();
			this.mnuFileOpenUnderlying = new System.Windows.Forms.MenuItem();
			this.mnuFileNewClearingFirmPrimeBroker = new System.Windows.Forms.MenuItem();
			this.mnuFileOpenCompany = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mnuFileNewCounterPartyType = new System.Windows.Forms.MenuItem();
			this.mnuFileOpenCounterParty = new System.Windows.Forms.MenuItem();
			this.mnuFileNewVenueType = new System.Windows.Forms.MenuItem();
			this.mnuFileNewFIXTAGS = new System.Windows.Forms.MenuItem();
			this.mnuNewFIXTAGSExecutionInstruction = new System.Windows.Forms.MenuItem();
			this.mnuNewFIXTAGSFix = new System.Windows.Forms.MenuItem();
			this.mnuNewFIXTAGSFixCapability = new System.Windows.Forms.MenuItem();
			this.mnuNewFIXTAGSHandlingInstructions = new System.Windows.Forms.MenuItem();
			this.mnuNewFIXTAGSOrderType = new System.Windows.Forms.MenuItem();
			this.mnuNewFIXTAGSSIDE = new System.Windows.Forms.MenuItem();
			this.mnuNewFIXTAGSTimeInForce = new System.Windows.Forms.MenuItem();
			this.mnuFileNewAdminUserDetail = new System.Windows.Forms.MenuItem();
			this.mnuThirdParty = new System.Windows.Forms.MenuItem();
			this.mnuWindow = new System.Windows.Forms.MenuItem();
			this.mnuWindowCascade = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuHelpAboutNirvana = new System.Windows.Forms.MenuItem();
			this.toolbarsManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this._SuperAdminMain_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._SuperAdminMain_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._SuperAdminMain_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._SuperAdminMain_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			((System.ComponentModel.ISupportInitialize)(this.toolbarsManager)).BeginInit();
			this.SuspendLayout();
			// 
			// mnuAdmin
			// 
			this.mnuAdmin.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.mnuFile,
																					 this.mnuTools,
																					 this.mnuWindow,
																					 this.mnuHelp});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MdiList = true;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFileOpen,
																					this.menuItem13,
																					this.mnuFileExit});
			this.mnuFile.Text = "&File";
			// 
			// mnuFileOpen
			// 
			this.mnuFileOpen.Index = 0;
			this.mnuFileOpen.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.mnuFileOpenCommissionRules,
																						this.mnuFileNewContractListingType,
																						this.mnuFileNewFutureMonthCodes,
																						this.mnuFileNewIdentifier,
																						this.mnuFileNewModule,
																						this.mnuFileOpenRiskManagement,
																						this.mnuFileNewSymbolConvention,
																						this.mnuFileNewUnit,
																						this.mnuFileOpenVenue});
			this.mnuFileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mnuFileOpen.Text = "&Open";
			// 
			// mnuFileOpenCommissionRules
			// 
			this.mnuFileOpenCommissionRules.Index = 0;
			this.mnuFileOpenCommissionRules.Text = "Commission Rules";
			this.mnuFileOpenCommissionRules.Click += new System.EventHandler(this.mnuFileOpenCommissionRules_Click);
			// 
			// mnuFileNewContractListingType
			// 
			this.mnuFileNewContractListingType.Index = 1;
			this.mnuFileNewContractListingType.Text = "Contract Listing Type";
			this.mnuFileNewContractListingType.Click += new System.EventHandler(this.mnuFileNewContractListingType_Click);
			// 
			// mnuFileNewFutureMonthCodes
			// 
			this.mnuFileNewFutureMonthCodes.Index = 2;
			this.mnuFileNewFutureMonthCodes.Text = "Future Month Codes";
			this.mnuFileNewFutureMonthCodes.Click += new System.EventHandler(this.mnuFileNewFutureMonthCodes_Click);
			// 
			// mnuFileNewIdentifier
			// 
			this.mnuFileNewIdentifier.Index = 3;
			this.mnuFileNewIdentifier.Text = "Identifier";
			this.mnuFileNewIdentifier.Click += new System.EventHandler(this.mnuFileNewIdentifier_Click);
			// 
			// mnuFileNewModule
			// 
			this.mnuFileNewModule.Index = 4;
			this.mnuFileNewModule.Text = "Module";
			this.mnuFileNewModule.Click += new System.EventHandler(this.mnuFileNewModule_Click);
			// 
			// mnuFileOpenRiskManagement
			// 
			this.mnuFileOpenRiskManagement.Index = 5;
			this.mnuFileOpenRiskManagement.Text = "Risk Management";
			this.mnuFileOpenRiskManagement.Click += new System.EventHandler(this.mnuFileOpenRiskManagement_Click);
			// 
			// mnuFileNewSymbolConvention
			// 
			this.mnuFileNewSymbolConvention.Index = 6;
			this.mnuFileNewSymbolConvention.Text = "Symbol Convention";
			this.mnuFileNewSymbolConvention.Click += new System.EventHandler(this.mnuFileNewSymbolConvention_Click);
			// 
			// mnuFileNewUnit
			// 
			this.mnuFileNewUnit.Index = 7;
			this.mnuFileNewUnit.Text = "Unit";
			this.mnuFileNewUnit.Click += new System.EventHandler(this.mnuFileNewUnit_Click);
			// 
			// mnuFileOpenVenue
			// 
			this.mnuFileOpenVenue.Enabled = false;
			this.mnuFileOpenVenue.Index = 8;
			this.mnuFileOpenVenue.Text = "&Venue";
			this.mnuFileOpenVenue.Visible = false;
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 1;
			this.menuItem13.Text = "-";
			// 
			// mnuFileExit
			// 
			this.mnuFileExit.Index = 2;
			this.mnuFileExit.Text = "E&xit";
			this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
			// 
			// mnuTools
			// 
			this.mnuTools.Index = 1;
			this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItem1,
																					 this.mnuFileNewClearingFirmPrimeBroker,
																					 this.mnuFileOpenCompany,
																					 this.menuItem2,
																					 this.mnuFileNewFIXTAGS,
																					 this.mnuFileNewAdminUserDetail,
																					 this.mnuThirdParty});
			this.mnuTools.Text = "&Tools";
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuFileOpenAsset,
																					  this.mnuFileOpenAUEC,
																					  this.mnuFileOpenCurrency,
																					  this.mnuFileNewCurrencyType,
																					  this.mnuFileOpenExchange,
																					  this.mnuFileNewHoliday,
																					  this.mnuFileOpenUnderlying});
			this.menuItem1.Text = "AUEC Master";
			// 
			// mnuFileOpenAsset
			// 
			this.mnuFileOpenAsset.Index = 0;
			this.mnuFileOpenAsset.Text = "&Asset";
			this.mnuFileOpenAsset.Click += new System.EventHandler(this.mnuFileOpenAsset_Click);
			// 
			// mnuFileOpenAUEC
			// 
			this.mnuFileOpenAUEC.Index = 1;
			this.mnuFileOpenAUEC.Text = "AUEC";
			this.mnuFileOpenAUEC.Click += new System.EventHandler(this.mnuFileOpenAUEC_Click);
			// 
			// mnuFileOpenCurrency
			// 
			this.mnuFileOpenCurrency.Index = 2;
			this.mnuFileOpenCurrency.Text = "&Currency";
			this.mnuFileOpenCurrency.Click += new System.EventHandler(this.mnuFileOpenCurrency_Click);
			// 
			// mnuFileNewCurrencyType
			// 
			this.mnuFileNewCurrencyType.Index = 3;
			this.mnuFileNewCurrencyType.Text = "Currency&Type";
			this.mnuFileNewCurrencyType.Click += new System.EventHandler(this.mnuFileNewCurrencyType_Click);
			// 
			// mnuFileOpenExchange
			// 
			this.mnuFileOpenExchange.Index = 4;
			this.mnuFileOpenExchange.Text = "&Exchange";
			this.mnuFileOpenExchange.Click += new System.EventHandler(this.mnuFileOpenExchange_Click);
			// 
			// mnuFileNewHoliday
			// 
			this.mnuFileNewHoliday.Index = 5;
			this.mnuFileNewHoliday.Text = "Holiday";
			this.mnuFileNewHoliday.Click += new System.EventHandler(this.mnuFileNewHoliday_Click);
			// 
			// mnuFileOpenUnderlying
			// 
			this.mnuFileOpenUnderlying.Index = 6;
			this.mnuFileOpenUnderlying.Text = "&Underlying";
			this.mnuFileOpenUnderlying.Click += new System.EventHandler(this.mnuFileOpenUnderlying_Click);
			// 
			// mnuFileNewClearingFirmPrimeBroker
			// 
			this.mnuFileNewClearingFirmPrimeBroker.Index = 1;
			this.mnuFileNewClearingFirmPrimeBroker.Text = "Clearing Firm Prime Broker";
			this.mnuFileNewClearingFirmPrimeBroker.Click += new System.EventHandler(this.mnuFileNewClearingFirmPrimeBroker_Click);
			// 
			// mnuFileOpenCompany
			// 
			this.mnuFileOpenCompany.Index = 2;
			this.mnuFileOpenCompany.Text = "Company";
			this.mnuFileOpenCompany.Click += new System.EventHandler(this.mnuFileOpenCompany_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 3;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuFileNewCounterPartyType,
																					  this.mnuFileOpenCounterParty,
																					  this.mnuFileNewVenueType});
			this.menuItem2.Text = "CounterPatry Venue Master";
			// 
			// mnuFileNewCounterPartyType
			// 
			this.mnuFileNewCounterPartyType.Index = 0;
			this.mnuFileNewCounterPartyType.Text = "Counter Party Type";
			this.mnuFileNewCounterPartyType.Click += new System.EventHandler(this.mnuFileNewCounterPartyType_Click);
			// 
			// mnuFileOpenCounterParty
			// 
			this.mnuFileOpenCounterParty.Index = 1;
			this.mnuFileOpenCounterParty.Text = "Cou&nter Party Venue";
			this.mnuFileOpenCounterParty.Click += new System.EventHandler(this.mnuFileOpenCounterParty_Click);
			// 
			// mnuFileNewVenueType
			// 
			this.mnuFileNewVenueType.Index = 2;
			this.mnuFileNewVenueType.Text = "Venue Type";
			this.mnuFileNewVenueType.Click += new System.EventHandler(this.mnuFileNewVenueType_Click);
			// 
			// mnuFileNewFIXTAGS
			// 
			this.mnuFileNewFIXTAGS.Index = 4;
			this.mnuFileNewFIXTAGS.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							  this.mnuNewFIXTAGSExecutionInstruction,
																							  this.mnuNewFIXTAGSFix,
																							  this.mnuNewFIXTAGSFixCapability,
																							  this.mnuNewFIXTAGSHandlingInstructions,
																							  this.mnuNewFIXTAGSOrderType,
																							  this.mnuNewFIXTAGSSIDE,
																							  this.mnuNewFIXTAGSTimeInForce});
			this.mnuFileNewFIXTAGS.Text = "FIX TAGS";
			// 
			// mnuNewFIXTAGSExecutionInstruction
			// 
			this.mnuNewFIXTAGSExecutionInstruction.Index = 0;
			this.mnuNewFIXTAGSExecutionInstruction.Text = "Execution Instruction";
			this.mnuNewFIXTAGSExecutionInstruction.Click += new System.EventHandler(this.mnuNewFIXTAGSExecutionInstruction_Click);
			// 
			// mnuNewFIXTAGSFix
			// 
			this.mnuNewFIXTAGSFix.Index = 1;
			this.mnuNewFIXTAGSFix.Text = "Fix";
			this.mnuNewFIXTAGSFix.Click += new System.EventHandler(this.mnuNewFIXTAGSFix_Click);
			// 
			// mnuNewFIXTAGSFixCapability
			// 
			this.mnuNewFIXTAGSFixCapability.Index = 2;
			this.mnuNewFIXTAGSFixCapability.Text = "Fix Capability";
			this.mnuNewFIXTAGSFixCapability.Click += new System.EventHandler(this.mnuNewFIXTAGSFixCapability_Click);
			// 
			// mnuNewFIXTAGSHandlingInstructions
			// 
			this.mnuNewFIXTAGSHandlingInstructions.Index = 3;
			this.mnuNewFIXTAGSHandlingInstructions.Text = "Handling Instructions";
			this.mnuNewFIXTAGSHandlingInstructions.Click += new System.EventHandler(this.mnuNewFIXTAGSHandlingInstructions_Click);
			// 
			// mnuNewFIXTAGSOrderType
			// 
			this.mnuNewFIXTAGSOrderType.Index = 4;
			this.mnuNewFIXTAGSOrderType.Text = "Order Type";
			this.mnuNewFIXTAGSOrderType.Click += new System.EventHandler(this.mnuNewFIXTAGSOrderType_Click);
			// 
			// mnuNewFIXTAGSSIDE
			// 
			this.mnuNewFIXTAGSSIDE.Index = 5;
			this.mnuNewFIXTAGSSIDE.Text = "SIDE";
			this.mnuNewFIXTAGSSIDE.Click += new System.EventHandler(this.mnuNewFIXTAGSSIDE_Click);
			// 
			// mnuNewFIXTAGSTimeInForce
			// 
			this.mnuNewFIXTAGSTimeInForce.Index = 6;
			this.mnuNewFIXTAGSTimeInForce.Text = "Time In Force";
			this.mnuNewFIXTAGSTimeInForce.Click += new System.EventHandler(this.mnuNewFIXTAGSTimeInForce_Click);
			// 
			// mnuFileNewAdminUserDetail
			// 
			this.mnuFileNewAdminUserDetail.Index = 5;
			this.mnuFileNewAdminUserDetail.Text = "SLSU";
			this.mnuFileNewAdminUserDetail.Click += new System.EventHandler(this.mnuFileNewAdminUserDetail_Click);
			// 
			// mnuThirdParty
			// 
			this.mnuThirdParty.Index = 6;
			this.mnuThirdParty.Text = "&Third Party";
			this.mnuThirdParty.Click += new System.EventHandler(this.mnuThirdParty_Click);
			// 
			// mnuWindow
			// 
			this.mnuWindow.Index = 2;
			this.mnuWindow.MdiList = true;
			this.mnuWindow.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mnuWindowCascade});
			this.mnuWindow.Text = "&Window";
			// 
			// mnuWindowCascade
			// 
			this.mnuWindowCascade.Index = 0;
			this.mnuWindowCascade.Text = "Cascade";
			this.mnuWindowCascade.Click += new System.EventHandler(this.mnuWindowCascade_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 3;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuHelpAboutNirvana});
			this.mnuHelp.Text = "&Help";
			// 
			// mnuHelpAboutNirvana
			// 
			this.mnuHelpAboutNirvana.Index = 0;
			this.mnuHelpAboutNirvana.Text = "&About Nirvana";
			this.mnuHelpAboutNirvana.Click += new System.EventHandler(this.mnuHelpAboutNirvana_Click);
			// 
			// toolbarsManager
			// 
			this.toolbarsManager.DesignerFlags = 1;
			this.toolbarsManager.DockWithinContainer = this;
			this.toolbarsManager.ShowFullMenusDelay = 500;
			ultraToolbar1.DockedColumn = 0;
			ultraToolbar1.DockedRow = 0;
			ultraToolbar1.Text = "Main Toolbaar";
			appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
			buttonTool3.InstanceProps.AppearancesSmall.Appearance = appearance1;
			ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							  buttonTool1,
																							  buttonTool2,
																							  buttonTool3,
																							  buttonTool4,
																							  buttonTool5,
																							  buttonTool6,
																							  buttonTool7,
																							  buttonTool8,
																							  buttonTool9});
			this.toolbarsManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
																											ultraToolbar1});
			appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
			buttonTool10.SharedProps.AppearancesSmall.Appearance = appearance2;
			buttonTool10.SharedProps.Caption = "AUEC";
			buttonTool10.SharedProps.CustomizerCaption = "AUEC";
			buttonTool10.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool10.SharedProps.ToolTipText = "AUEC";
			appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
			buttonTool11.SharedProps.AppearancesSmall.Appearance = appearance3;
			buttonTool11.SharedProps.Caption = "Underlying";
			buttonTool11.SharedProps.CustomizerCaption = "Underlying";
			buttonTool11.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool11.SharedProps.ToolTipText = "Underlying";
			buttonTool11.SharedProps.Visible = false;
			appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
			buttonTool12.SharedProps.AppearancesSmall.Appearance = appearance4;
			buttonTool12.SharedProps.Caption = "Currency";
			buttonTool12.SharedProps.CustomizerCaption = "Currency";
			buttonTool12.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool12.SharedProps.ToolTipText = "Currency";
			buttonTool12.SharedProps.Visible = false;
			appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
			buttonTool13.SharedProps.AppearancesSmall.Appearance = appearance5;
			buttonTool13.SharedProps.Caption = "Exchange";
			buttonTool13.SharedProps.CustomizerCaption = "Exchange";
			buttonTool13.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool13.SharedProps.ToolTipText = "Exchange";
			buttonTool13.SharedProps.Visible = false;
			appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
			buttonTool14.SharedProps.AppearancesSmall.Appearance = appearance6;
			buttonTool14.SharedProps.Caption = "SLSU";
			buttonTool14.SharedProps.CustomizerCaption = "SLSU";
			buttonTool14.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool14.SharedProps.ToolTipText = "Users";
			appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
			buttonTool15.SharedProps.AppearancesSmall.Appearance = appearance7;
			buttonTool15.SharedProps.Caption = "Exit";
			buttonTool15.SharedProps.CustomizerCaption = "Exit";
			buttonTool15.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool15.SharedProps.ToolTipText = "Exit";
			buttonTool16.SharedProps.Caption = "Company";
			buttonTool16.SharedProps.CustomizerCaption = "Company";
			buttonTool16.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool16.SharedProps.ToolTipText = "Company";
			appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
			buttonTool17.SharedProps.AppearancesSmall.Appearance = appearance8;
			buttonTool17.SharedProps.Caption = "Asset";
			buttonTool17.SharedProps.CustomizerCaption = "Asset";
			buttonTool17.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			buttonTool17.SharedProps.ToolTipText = "Asset";
			buttonTool17.SharedProps.Visible = false;
			appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
			buttonTool18.SharedProps.AppearancesSmall.Appearance = appearance9;
			buttonTool18.SharedProps.Caption = "CounterParty Venue";
			buttonTool18.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
			this.toolbarsManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																									 buttonTool10,
																									 buttonTool11,
																									 buttonTool12,
																									 buttonTool13,
																									 buttonTool14,
																									 buttonTool15,
																									 buttonTool16,
																									 buttonTool17,
																									 buttonTool18});
			this.toolbarsManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.toolbarsManager_ToolClick);
			// 
			// _SuperAdminMain_Toolbars_Dock_Area_Left
			// 
			this._SuperAdminMain_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._SuperAdminMain_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.Azure;
			this._SuperAdminMain_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._SuperAdminMain_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._SuperAdminMain_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 24);
			this._SuperAdminMain_Toolbars_Dock_Area_Left.Name = "_SuperAdminMain_Toolbars_Dock_Area_Left";
			this._SuperAdminMain_Toolbars_Dock_Area_Left.ToolbarsManager = this.toolbarsManager;
			// 
			// _SuperAdminMain_Toolbars_Dock_Area_Right
			// 
			this._SuperAdminMain_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._SuperAdminMain_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.Azure;
			this._SuperAdminMain_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._SuperAdminMain_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._SuperAdminMain_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1020, 24);
			this._SuperAdminMain_Toolbars_Dock_Area_Right.Name = "_SuperAdminMain_Toolbars_Dock_Area_Right";
			this._SuperAdminMain_Toolbars_Dock_Area_Right.ToolbarsManager = this.toolbarsManager;
			// 
			// _SuperAdminMain_Toolbars_Dock_Area_Top
			// 
			this._SuperAdminMain_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._SuperAdminMain_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.Azure;
			this._SuperAdminMain_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._SuperAdminMain_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._SuperAdminMain_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
			this._SuperAdminMain_Toolbars_Dock_Area_Top.Name = "_SuperAdminMain_Toolbars_Dock_Area_Top";
			this._SuperAdminMain_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1020, 24);
			this._SuperAdminMain_Toolbars_Dock_Area_Top.ToolbarsManager = this.toolbarsManager;
			// 
			// _SuperAdminMain_Toolbars_Dock_Area_Bottom
			// 
			this._SuperAdminMain_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this._SuperAdminMain_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.Azure;
			this._SuperAdminMain_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._SuperAdminMain_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._SuperAdminMain_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 24);
			this._SuperAdminMain_Toolbars_Dock_Area_Bottom.Name = "_SuperAdminMain_Toolbars_Dock_Area_Bottom";
			this._SuperAdminMain_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1020, 0);
			this._SuperAdminMain_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.toolbarsManager;
			// 
			// SuperAdminMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.Azure;
			this.ClientSize = new System.Drawing.Size(1020, 24);
			this.Controls.Add(this._SuperAdminMain_Toolbars_Dock_Area_Left);
			this.Controls.Add(this._SuperAdminMain_Toolbars_Dock_Area_Right);
			this.Controls.Add(this._SuperAdminMain_Toolbars_Dock_Area_Top);
			this.Controls.Add(this._SuperAdminMain_Toolbars_Dock_Area_Bottom);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.MaximizeBox = false;
			this.Menu = this.mnuAdmin;
			this.Name = "SuperAdminMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Nirvana: Admin";
			((System.ComponentModel.ISupportInitialize)(this.toolbarsManager)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void mnuFileExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void mnuFileOpenAUEC_Click(object sender, System.EventArgs e)
		{
			if(frmAUEC == null)
			{
				frmAUEC = new AUEC();
				frmAUEC.Owner = this;
				frmAUEC.ShowInTaskbar = false;
			}
			frmAUEC.Show();
			frmAUEC.Disposed +=new EventHandler(frmAUEC_Disposed);
		}

		AddUser frmUser = null;
		private void mnuFileNewUser_Click(object sender, System.EventArgs e)
		{
			if(frmUser == null)
			{
				frmUser = new AddUser();
				frmUser.Owner = this;			
				frmUser.ShowInTaskbar = false;
			}
			frmUser.Show();
			frmUser.Disposed +=new EventHandler(frmUser_Disposed);

		}
		AddVendor frmVendor = null;
		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			if(frmVendor == null)
			{
				frmVendor = new AddVendor();
				frmVendor.Owner = this;
				frmVendor.ShowInTaskbar = false;
			}
			frmVendor.Show();
			frmVendor.Disposed +=new EventHandler(frmVendor_Disposed);
		}

		private void mnuFileClose_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		ThirdPartyVendor frmThirdPartyVendor = null;
		private void mnuFileNewAdminUserDetail_Click(object sender, System.EventArgs e)
		{
			if(frmThirdPartyVendor == null)
			{
				frmThirdPartyVendor = new ThirdPartyVendor();
				frmThirdPartyVendor.Owner = this;
				frmThirdPartyVendor.ShowInTaskbar = false;
			}
			frmThirdPartyVendor.Show();
			frmThirdPartyVendor.Disposed +=new EventHandler(frmThirdPartyVendor_Disposed);
		}
		
		private void mnuFileOpenAsset_Click(object sender, System.EventArgs e)
		{
			if(frmAssetDetails == null)
			{
				frmAssetDetails = new AssetDetails();
				frmAssetDetails.Owner = this;			
				frmAssetDetails.ShowInTaskbar = false;
			}
			frmAssetDetails.Show();
			frmAssetDetails.Disposed +=new EventHandler(frmAssetDetails_Disposed);
		}

		private void toolbarsManager_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			//			MessageBox.Show(e.Tool.Key); 
			switch(e.Tool.Key.ToUpper())
			{
				case "COMPANY":
					
					if(frmCompanyMaster == null)
					{
						frmCompanyMaster = new CompanyMaster();
						frmCompanyMaster.Owner = this;								
						frmCompanyMaster.ShowInTaskbar = false;
					}
					frmCompanyMaster.Show();
					frmCompanyMaster.Disposed +=new EventHandler(frmCompanyMaster_Disposed);
					break;

				case "AUEC":
					if(frmAUEC == null)
					{
						frmAUEC = new AUEC();
						frmAUEC.Owner = this;								
						frmAUEC.ShowInTaskbar = false;
					}
					frmAUEC.Show();
					frmAUEC.Disposed +=new EventHandler(frmAUEC_Disposed);
					break;

				case "ASSET":
					if(frmAssetDetails == null)
					{
						frmAssetDetails  = new AssetDetails();
						frmAssetDetails.Owner = this;					
						frmAssetDetails.ShowInTaskbar = false;
					}
					frmAssetDetails.Show();
					frmAssetDetails.Disposed +=new EventHandler(frmAssetDetails_Disposed);
					break;				

				case "UNDERLYING":
					if(frmUnderlyingDetails == null)
					{
						frmUnderlyingDetails = new UnderlyingDetails();
						frmUnderlyingDetails.Owner = this;
						frmUnderlyingDetails.ShowInTaskbar = false;
					}
					frmUnderlyingDetails.Show();
					frmUnderlyingDetails.Disposed +=new EventHandler(frmUnderlyingDetails_Disposed);
					break;

				case "EXCHANGE":
					if(frmExchange == null)
					{
						frmExchange = new ExchangeForm();
						frmExchange.Owner = this;			
						frmExchange.ShowInTaskbar = false;
					}
					frmExchange.Show();
					frmExchange.Disposed +=new EventHandler(frmExchange_Disposed);
					break;				

				case "CURRENCY":
					if(frmCurrencyForm == null)
					{
						frmCurrencyForm = new CurrencyForm();
						frmCurrencyForm.Owner = this;			
						frmCurrencyForm.ShowInTaskbar = false;
					}
					frmCurrencyForm.Show();
					frmCurrencyForm.Disposed +=new EventHandler(frmCurrencyForm_Disposed);
					break;

				case "USER":
					if(frmThirdPartyVendor == null)
					{
						frmThirdPartyVendor = new ThirdPartyVendor();
						frmThirdPartyVendor.Owner = this;
						frmThirdPartyVendor.ShowInTaskbar = false;
					}
					frmThirdPartyVendor.Show();
					frmThirdPartyVendor.Disposed += new EventHandler(frmThirdPartyVendor_Disposed);
					break;	
			
				case "COUNTERPARTYVENUE":

					if(frmCounterPartyVenueMaster == null)
					{
						frmCounterPartyVenueMaster = new CounterPartyVenueMaster();
						frmCounterPartyVenueMaster.Owner = this;
						frmCounterPartyVenueMaster.ShowInTaskbar = false;
					}
					frmCounterPartyVenueMaster.Show();
					frmCounterPartyVenueMaster.Disposed +=new EventHandler(frmCounterPartyVenueMaster_Disposed);
					 
					break;

				case "EXIT":
					Application.Exit();
					break;
			}
		}

		
		private void mnuFileOpenUnderlying_Click(object sender, System.EventArgs e)
		{
			if(frmUnderlyingDetails == null)
			{
				frmUnderlyingDetails = new UnderlyingDetails();
				frmUnderlyingDetails.Owner = this;
				frmUnderlyingDetails.ShowInTaskbar = false;
			}
			frmUnderlyingDetails.Show();			
			frmUnderlyingDetails.Disposed +=new EventHandler(frmUnderlyingDetails_Disposed);
		}

		private CounterPartyVenueMaster frmCounterPartyVenueMaster = null;
		private void mnuFileOpenCounterParty_Click(object sender, System.EventArgs e)
		{
			if(frmCounterPartyVenueMaster == null)
			{
				frmCounterPartyVenueMaster = new CounterPartyVenueMaster();
				frmCounterPartyVenueMaster.Owner = this;
				frmCounterPartyVenueMaster.ShowInTaskbar = false;
			}
			frmCounterPartyVenueMaster.Show();
			frmCounterPartyVenueMaster.Disposed +=new EventHandler(frmCounterPartyVenueMaster_Disposed);
		}

		CompanyMaster frmCompanyMaster = null;
		private void mnuFileOpenCompany_Click(object sender, System.EventArgs e)
		{
			if(frmCompanyMaster == null)
			{
				frmCompanyMaster = new CompanyMaster();
				frmCompanyMaster.Owner = this;
				frmCompanyMaster.ShowInTaskbar = false;
			}
			frmCompanyMaster.Show();
			frmCompanyMaster.Disposed +=new EventHandler(frmCompanyMaster_Disposed);
		}	
		
		Permissions _userPermissions = null;
		public Permissions UserPermissions
		{
			get{return _userPermissions;}
			set
			{
				_userPermissions = value;
				SavePreferences();
				SetUpMenuPermissions();
				
			}
		}

		private void SetUpMenuPermissions()
		{	
			Preferences preferences = Preferences.Instance;	
			bool chkMaintainCompanies = preferences.Maintain_Companies;
			bool chkSetUpCompanies = preferences.Set_Up_Company;
			bool chkMaintainAUEC = preferences.Maintain_AUEC;
			bool chkMaintainCounterParties = preferences.Maintain_Counter_Parties;
			//If the user doesnt have the permissions to maintain companies then the Add & Delete buttons are
			//disabled so that he/she can't add or delete the companies.
			
			//			if(chkSetUpCompanies == false )
			//			{
			//				mnuFile.MenuItems[MNU_FILE_OPEN].MenuItems[MNU_FILE_OPEN_COMPANY].Enabled = false;
			//			}
			if(chkMaintainAUEC == false)
			{
				toolbarsManager.Toolbars["tlbNirvanaMain"].Tools["AUEC"].SharedProps.Enabled = false;		
				mnuTools.MenuItems[MNU_TOOLS_AUECMASTER].MenuItems[MNU_TOOLS_AUECMASTER_AUEC].Enabled = false;
			}
			if(!preferences.Maintain_Counter_Parties)
			{
				mnuTools.MenuItems[MNU_TOOLS_COUNTERPARTYVENUEMASTER].MenuItems[MNU_TOOLS_COUNTERPARTYVENUE].Enabled = false;
				toolbarsManager.Toolbars["tlbNirvanaMain"].Tools["CounterPartyVenue"].SharedProps.Enabled = false;		
			}
			if(chkSetUpCompanies == false && chkMaintainCompanies == false)
			{
				mnuTools.MenuItems[MNU_TOOLS_COMPANY].Enabled = false;
				toolbarsManager.Toolbars["tlbNirvanaMain"].Tools["Company"].SharedProps.Enabled = false;		
			}
			if(!preferences.Maintain_AUEC || !preferences.Maintain_Counter_Parties || !preferences.Set_Up_Company || !preferences.Maintain_Companies)
			{
				mnuTools.MenuItems[MNU_TOOLS_SLSU].Enabled = false;
				toolbarsManager.Toolbars["tlbNirvanaMain"].Tools["User"].SharedProps.Enabled = false;
			}
		}

		public bool SavePreferences()
		{
			Preferences preferences = Preferences.Instance;			
			preferences.UserPermissions = _userPermissions;
			preferences.UserID = _userPermissions.UserID; //Newlly added code line on 14th Aprip 2006.
			return preferences.Reset();
		}

		private void mnuWindowCascade_Click(object sender, System.EventArgs e)
		{
			this.LayoutMdi(MdiLayout.Cascade);
		}

		private void mnuFileOpenExchange_Click(object sender, System.EventArgs e)
		{
			if(frmExchange == null)
			{
				frmExchange = new ExchangeForm();
				frmExchange.Owner = this;			
				frmExchange.ShowInTaskbar = false;
			}
			frmExchange.Show();
			frmExchange.Disposed +=new EventHandler(frmExchange_Disposed);
		}

		ThirdPartyForm frmThirdPartyForm = null;
		private void mnuThirdParty_Click(object sender, System.EventArgs e)
		{
			if(frmThirdPartyForm == null)
			{
				frmThirdPartyForm = new ThirdPartyForm();
				frmThirdPartyForm.Owner = this;	
				frmThirdPartyForm.ShowInTaskbar = false;
			}
			frmThirdPartyForm.Show();
			frmThirdPartyForm.Disposed +=new EventHandler(frmThirdPartyForm_Disposed);
		}

		CurrencyForm frmCurrencyForm = null;
		private void mnuFileOpenCurrency_Click(object sender, System.EventArgs e)
		{
			if(frmCurrencyForm == null)
			{
				frmCurrencyForm = new CurrencyForm();
				frmCurrencyForm.Owner = this;		
				frmCurrencyForm.ShowInTaskbar = false;
			}
			frmCurrencyForm.Show();
			frmCurrencyForm.Disposed +=new EventHandler(frmCurrencyForm_Disposed);
		}

		TestUserForm frmTestUserForm = null;
		private void mnuTestUser_Click(object sender, System.EventArgs e)
		{
			if(frmTestUserForm == null)
			{
				frmTestUserForm = new TestUserForm();
				frmTestUserForm.Owner = this;			
				frmTestUserForm.ShowInTaskbar = false;
			}
			frmTestUserForm.Show();
			frmTestUserForm.Disposed +=new EventHandler(frmTestUserForm_Disposed);
		}
		private void frmTestUserForm_Disposed(object sender, EventArgs e)
		{			
			frmTestUserForm = null;
		}
		private void frmThirdPartyForm_Disposed(object sender, EventArgs e)
		{
			frmThirdPartyForm = null;
		}
		private void frmCurrencyForm_Disposed(object sender, EventArgs e)
		{
			frmCurrencyForm = null;
		}
		private void frmExchange_Disposed(object sender, EventArgs e)
		{
			frmExchange = null;
		}

		private void frmAUEC_Disposed(object sender, EventArgs e)
		{
			frmAUEC = null;
		}

		private void frmUser_Disposed(object sender, EventArgs e)
		{
			frmUser = null;
		}
		private void frmVendor_Disposed(object sender, EventArgs e)
		{
			frmVendor = null;
		}

		private void frmThirdPartyVendor_Disposed(object sender, EventArgs e)
		{
			frmThirdPartyVendor = null;
		}

		private void frmAssetDetails_Disposed(object sender, EventArgs e)
		{
			frmAssetDetails = null;
		}

		private void frmUnderlyingDetails_Disposed(object sender, EventArgs e)
		{
			frmUnderlyingDetails = null;
		}

		private void frmCounterPartyVenueMaster_Disposed(object sender, EventArgs e)
		{
			frmCounterPartyVenueMaster = null;
		}

		private void frmCompanyMaster_Disposed(object sender, EventArgs e)
		{
			frmCompanyMaster = null;
		}
		
		private void frmSide_Disposed(object sender, EventArgs e)
		{
			frmSide = null;
		}

		private void frmOrderType_Disposed(object sender, EventArgs e)
		{
			frmOrderType = null;
		}

		private void frmExecutionInstruction_Disposed(object sender, EventArgs e)
		{
			frmExecutionInstruction = null;
		}

		private void frmTimeInForce_Disposed(object sender, EventArgs e)
		{
			frmTimeInForce = null;
		}

		private void frmHandlingInstruction_Disposed(object sender, EventArgs e)
		{
			frmHandlingInstruction = null;
		}

		private void frmUnit_Disposed(object sender, EventArgs e)
		{
			frmUnit = null;
		}

		private void frmIdentifier_Disposed(object sender, EventArgs e)
		{
			frmIdentifier = null;
		}

		private void frmFix_Disposed(object sender, EventArgs e)
		{
			frmFix = null;
		}

		private void frmFixCapability_Disposed(object sender, EventArgs e)
		{
			frmFixCapability = null;
		}

		private void frmModule_Disposed(object sender, EventArgs e)
		{
			frmModule = null;
		}

		private void frmVenueType_Disposed(object sender, EventArgs e)
		{
			frmVenueType = null;
		}

		private void frmCounterPartyType_Disposed(object sender, EventArgs e)
		{
			frmCounterPartyType = null;
		}

		private void frmSymbolConvention_Disposed(object sender, EventArgs e)
		{
			frmSymbolConvention = null;
		}

		private void frmClearingFirmsPrimeBrokers_Disposed(object sender, EventArgs e)
		{
			frmClearingFirmsPrimeBrokers = null;
		}

		private void frmCurrencyType_Disposed(object sender, EventArgs e)
		{
			frmCurrencyType = null;
		}

		private void frmContractListingType_Disposed(object sender, EventArgs e)
		{
			frmContractListingType = null;
		}

		private void frmFutureMonthCode_Disposed(object sender, EventArgs e)
		{
			frmFutureMonthCode = null;
		}
		
		private void frmHoliday_Disposed(object sender, EventArgs e)
		{
			frmHoliday = null;
		}

		private void frmRiskManagement_Disposed(object sender, EventArgs e)
		{
			frmRiskManagement = null;
		}

		private void frmCommissionRules_Disposed(object sender, EventArgs e)
		{
			frmCommissionRules = null;
		}

		private void frmNirvanaHelp_Disposed(object sender, EventArgs e)
		{
			frmNirvanaHelp = null;
		}

		private void mnuNewFIXTAGSSIDE_Click(object sender, System.EventArgs e)
		{
			if(frmSide == null)
			{
				frmSide = new Side();
				frmSide.Owner = this;			
				frmSide.ShowInTaskbar = false;
			}
			frmSide.Show();
			frmSide.Disposed +=new EventHandler(frmSide_Disposed);			
		}

		private void mnuNewFIXTAGSOrderType_Click(object sender, System.EventArgs e)
		{
			if(frmOrderType == null)
			{
				frmOrderType = new OrderType();
				frmOrderType.Owner = this;			
				frmOrderType.ShowInTaskbar = false;
			}
			frmOrderType.Show();
			frmOrderType.Disposed +=new EventHandler(frmOrderType_Disposed);			
		}

		private void mnuNewFIXTAGSExecutionInstruction_Click(object sender, System.EventArgs e)
		{
			if(frmExecutionInstruction == null)
			{
				frmExecutionInstruction = new ExecutionInstruction();
				frmExecutionInstruction.Owner = this;			
				frmExecutionInstruction.ShowInTaskbar = false;
			}
			frmExecutionInstruction.Show();
			frmExecutionInstruction.Disposed +=new EventHandler(frmExecutionInstruction_Disposed);
		}

		private void mnuNewFIXTAGSHandlingInstructions_Click(object sender, System.EventArgs e)
		{
			if(frmHandlingInstruction == null)
			{
				frmHandlingInstruction = new HandlingInstruction();
				frmHandlingInstruction.Owner = this;			
				frmHandlingInstruction.ShowInTaskbar = false;
			}
			frmHandlingInstruction.Show();
			frmHandlingInstruction.Disposed +=new EventHandler(frmHandlingInstruction_Disposed);
		}

		private void mnuNewFIXTAGSTimeInForce_Click(object sender, System.EventArgs e)
		{
			if(frmTimeInForce == null)
			{
				frmTimeInForce = new TimeInForce();
				frmTimeInForce.Owner = this;			
				frmTimeInForce.ShowInTaskbar = false;
			}
			frmTimeInForce.Show();
			frmTimeInForce.Disposed +=new EventHandler(frmTimeInForce_Disposed);
		}

		private void mnuFileNewUnit_Click(object sender, System.EventArgs e)
		{
			if(frmUnit == null)
			{
				frmUnit = new Unit();
				frmUnit.Owner = this;			
				frmUnit.ShowInTaskbar = false;
			}
			frmUnit.Show();
			frmUnit.Disposed +=new EventHandler(frmUnit_Disposed);
		}

		private void mnuFileNewIdentifier_Click(object sender, System.EventArgs e)
		{
			if(frmIdentifier == null)
			{
				frmIdentifier = new Identifier();
				frmIdentifier.Owner = this;			
				frmIdentifier.ShowInTaskbar = false;
			}
			frmIdentifier.Show();
			frmIdentifier.Disposed +=new EventHandler(frmIdentifier_Disposed);
		}

		private void mnuNewFIXTAGSFix_Click(object sender, System.EventArgs e)
		{
			if(frmFix == null)
			{
				frmFix = new Fix();
				frmFix.Owner = this;			
				frmFix.ShowInTaskbar = false;
			}
			frmFix.Show();
			frmFix.Disposed +=new EventHandler(frmFix_Disposed);
		}

		private void mnuNewFIXTAGSFixCapability_Click(object sender, System.EventArgs e)
		{
			if(frmFixCapability == null)
			{
				frmFixCapability = new FixCapability();
				frmFixCapability.Owner= this;			
				frmFixCapability.ShowInTaskbar = false;
			}
			frmFixCapability.Show();
			frmFixCapability.Disposed +=new EventHandler(frmFixCapability_Disposed);
		}

		private void mnuFileNewModule_Click(object sender, System.EventArgs e)
		{
			if(frmModule == null)
			{
				frmModule = new Module();
				frmModule.Owner = this;			
				frmModule.ShowInTaskbar = false;
			}
			frmModule.Show();
			frmModule.Disposed +=new EventHandler(frmModule_Disposed);
		}

		private void mnuFileNewVenueType_Click(object sender, System.EventArgs e)
		{
			if(frmVenueType == null)
			{
				frmVenueType = new VenueType();
				frmVenueType.Owner = this;			
				frmVenueType.ShowInTaskbar = false;
			}
			frmVenueType.Show();
			frmVenueType.Disposed +=new EventHandler(frmVenueType_Disposed);
		}

		private void mnuFileNewCounterPartyType_Click(object sender, System.EventArgs e)
		{
			if(frmCounterPartyType == null)
			{
				frmCounterPartyType = new CounterPartyType();
				frmCounterPartyType.Owner = this;			
				frmCounterPartyType.ShowInTaskbar = false;
			}
			frmCounterPartyType.Show();
			frmCounterPartyType.Disposed +=new EventHandler(frmCounterPartyType_Disposed);
		}

		//		private void mnuFileNewSymbolIdentifier_Click(object sender, System.EventArgs e)
		//		{
		//			if(frmSymbolConvention == null)
		//			{
		//				frmSymbolConvention = new SymbolConvention();
		//				frmSymbolConvention.Owner = this;			
		//				frmSymbolConvention.ShowInTaskbar = false;
		//			}
		//			frmSymbolConvention.Show();
		//			frmSymbolConvention.Disposed +=new EventHandler(frmSymbolConvention_Disposed);
		//		}

		private void mnuFileNewClearingFirmPrimeBroker_Click(object sender, System.EventArgs e)
		{
			if(frmClearingFirmsPrimeBrokers == null)
			{
				frmClearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();
				frmClearingFirmsPrimeBrokers.Owner = this;			
				frmClearingFirmsPrimeBrokers.ShowInTaskbar = false;
			}
			frmClearingFirmsPrimeBrokers.Show();
			frmClearingFirmsPrimeBrokers.Disposed +=new EventHandler(frmClearingFirmsPrimeBrokers_Disposed);
		}

		private void mnuFileNewSymbolConvention_Click(object sender, System.EventArgs e)
		{	
			if(frmSymbolConvention == null)
			{
				frmSymbolConvention = new SymbolConvention();
				frmSymbolConvention.Owner = this;			
				frmSymbolConvention.ShowInTaskbar = false;
			}
			frmSymbolConvention.Show();
			frmSymbolConvention.Disposed +=new EventHandler(frmSymbolConvention_Disposed);
		}

		private void mnuFileNewCurrencyType_Click(object sender, System.EventArgs e)
		{
			if(frmCurrencyType == null)
			{
				frmCurrencyType = new CurrencyType();
				frmCurrencyType.Owner = this;			
				frmCurrencyType.ShowInTaskbar = false;
			}
			frmCurrencyType.Show();
			frmCurrencyType.Disposed +=new EventHandler(frmCurrencyType_Disposed);
		}

		private void mnuFileNewContractListingType_Click(object sender, System.EventArgs e)
		{
			if(frmContractListingType == null)
			{
				frmContractListingType = new ContractListingType();
				frmContractListingType.Owner = this;			
				frmContractListingType.ShowInTaskbar = false;
			}
			frmContractListingType.Show();
			frmContractListingType.Disposed +=new EventHandler(frmContractListingType_Disposed);
		}

		private void mnuFileNewFutureMonthCodes_Click(object sender, System.EventArgs e)
		{
			if(frmFutureMonthCode == null)
			{
				frmFutureMonthCode = new FutureMonthCode();
				frmFutureMonthCode.Owner = this;			
				frmFutureMonthCode.ShowInTaskbar = false;
			}
			frmFutureMonthCode.Show();
			frmFutureMonthCode.Disposed +=new EventHandler(frmFutureMonthCode_Disposed);
		}

		private void mnuFileNewHoliday_Click(object sender, System.EventArgs e)
		{
			if(frmHoliday == null)
			{
				frmHoliday = new Holiday();
				frmHoliday.Owner = this;			
				frmHoliday.ShowInTaskbar = false;
			}
			frmHoliday.Show();
			frmHoliday.Disposed +=new EventHandler(frmHoliday_Disposed);
		}

		private void mnuFileOpenRiskManagement_Click(object sender, System.EventArgs e)
		{
			if(frmRiskManagement == null)
			{
				frmRiskManagement = new Nirvana.Admin.RiskManagement.RMAdmin();
				frmRiskManagement.Owner = this;			
				frmRiskManagement.ShowInTaskbar = false;
			}
			frmRiskManagement.Show();
			frmRiskManagement.Disposed +=new EventHandler(frmRiskManagement_Disposed);
		}

		private void mnuFileOpenCommissionRules_Click(object sender, System.EventArgs e)
		{
			if(frmCommissionRules == null)
			{
				frmCommissionRules = new Nirvana.Admin.CommissionRules.CommissionRules();
				frmCommissionRules.Owner = this;			
				frmCommissionRules.ShowInTaskbar = false;
			}
			frmCommissionRules.Show();
			frmCommissionRules.Disposed +=new EventHandler(frmCommissionRules_Disposed);
		}


		private void mnuHelpAboutNirvana_Click(object sender, System.EventArgs e)
		{
			if(frmNirvanaHelp == null)
			{
				frmNirvanaHelp = new Nirvana.Admin.NirvanaHelp();
				frmNirvanaHelp.Owner = this;			
				frmNirvanaHelp.ShowInTaskbar = false;
			}
			frmNirvanaHelp.Show();
			frmNirvanaHelp.Disposed +=new EventHandler(frmNirvanaHelp_Disposed);
		}
	
	}
}
