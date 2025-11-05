using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Interfaces;
using Nirvana.LiveFeedProvider;
using Nirvana.BusinessObjects;
using Nirvana.Global;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Nirvana.CommonDataCache;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Nirvana.PNL
{
	/// <summary>
	/// Summary description for PNLMain.
	/// </summary>
	public class PNLMain : System.Windows.Forms.Form, Nirvana.Interfaces.IPNL
	{
		private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraPNLTabControl;
		private System.ComponentModel.IContainer components;
		private int secondCounter = 0;
		private int defaultCurrencyID = Int32.MinValue;
		public event EventHandler RefreshPNL;
		private PNLPrefrencesData _pnlPrefrences = null;
		/// <summary>
		/// To hold the Currency Conversion Detail
		/// </summary>
		private CurrencyConversionCollection currencyConversionCollection;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageControlAsset;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageControlSymbol;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageControlTradingAccount;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage tabSharedControlsPage;
		private System.Windows.Forms.Panel pnlTabTop;
		private Infragistics.Win.UltraWinEditors.UltraComboEditor currencyCombo;
		private System.Windows.Forms.Label lblCurrency;
		//private Nirvana.PNL.PNLGridControl pnlGridControl = null;
		private Nirvana.PNL.PNLGridControl pnlAssetControl = null;
		private Nirvana.PNL.PNLGridControl pnlSymbolControl = null;
		private Nirvana.PNL.PNLGridControl pnlTradingAccountControl = null;

		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _PNLMain_Toolbars_Dock_Area_Left;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _PNLMain_Toolbars_Dock_Area_Right;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _PNLMain_Toolbars_Dock_Area_Top;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _PNLMain_Toolbars_Dock_Area_Bottom;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager pnlToolBar;
		private System.Windows.Forms.ImageList imageList;
		private const string FORM_NAME = "PNL: Main Form";
		private int _userID;

		/// <summary>
		/// PNLClosed will be fired if PNL is closed
		/// </summary>
		public event EventHandler PNLClosed;
		/// <summary>
		/// LaunchPreferences to launch the preferences
		/// </summary>
		public event EventHandler LaunchPreferences;
		/// <summary>
		/// PNL Preferences Manager to save and load the preferences in XML
		/// </summary>
		private PNLPreferencesManager _PNLPreferencesManager = null;

		/// <summary>
		/// To store the PNL preferences
		/// </summary>
		private PNLPreferences pnlPreferences;
		/// <summary>
		/// Teference to PNL chart window
		/// </summary>
		private PNLChart pnlChart = null;
		/// <summary>
		/// interface to livefeed manager
		/// </summary>
		private ILiveFeedManager liveFeedManager;
		private System.Windows.Forms.Panel pnlAssetPanel;
		private System.Windows.Forms.Panel pnlSymbolPanel;
		private System.Windows.Forms.Panel pnlTradingAccountPanel;
		/// <summary>
		/// To hold the currency list
		/// </summary>
		CurrencyCollection currencyList;
 

		public int UserID
		{
			get
			{
				return this._userID;
			}

			set
			{
				this._userID = value;
			}

		}

		/// <summary>
		/// returns the reference of PNLMain window instance
		/// </summary>
		/// <returns></returns>
		public Form Reference()
		{
			return this;
		}

		/// <summary>
		/// default constructor
		/// </summary>
		public PNLMain()
		{

			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();
			
				//Receive the timer event for every 10 s
				CentralTimer.GetInstance().PerSecond +=new EventHandler(PNLMain_PerSecond);
			
				//Get the CurrencyConversion details
				currencyConversionCollection = CurrencyDataManager.GetInstance().GetCurrencyConversions();
			
				//Get the list of avaiable currency for which converions details are available.
				currencyList = CurrencyDataManager.GetInstance().GetCurrencies();
			
				//Set the defaultCurrencyID to USD
				defaultCurrencyID = Global.Common.USDollar;

				populateCurrencyCombo();
			
				_PNLPreferencesManager = PNLPreferencesManager.GetInstance() ;
				_pnlPrefrences = (PNLPrefrencesData)_PNLPreferencesManager.GetPreferences();

				//get the instance of the livefeed manager
				liveFeedManager = eSignalManager.GetInstance();

				if(!liveFeedManager.IsDataManagerConnected())
				{
					liveFeedManager.ConnectDataManager(); //What if it is still not connected
					System.Threading.Thread.Sleep(500);
				}

				this.tabPageControlAsset.Controls.Add(pnlTabTop);


			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				//deregister from event handler
				CentralTimer.GetInstance().PerSecond -=new EventHandler(PNLMain_PerSecond);

				if(_pnlPrefrences != null)
					_pnlPrefrences = null;

				if(liveFeedManager != null)
					liveFeedManager = null;

				if(currencyList != null)
					currencyList = null;

				if(currencyConversionCollection != null)
					currencyConversionCollection = null;

				if(this.pnlChart != null)
					this.pnlChart.Dispose();

				if(this.pnlAssetControl != null)
				{
					this.RefreshPNL -=new EventHandler(this.pnlAssetControl.RefreshPNL);
					this.pnlAssetControl.Dispose();
				}

				if(this.pnlSymbolControl != null)
				{
					this.RefreshPNL -=new EventHandler(this.pnlSymbolControl.RefreshPNL);
					this.pnlSymbolControl.Dispose();
				}

				if(this.pnlTradingAccountControl != null)
				{
					this.RefreshPNL -=new EventHandler(this.pnlTradingAccountControl.RefreshPNL);
					this.pnlTradingAccountControl.Dispose();
				}

				if(pnlPreferences != null)
				{
					pnlPreferences.Dispose();
				}
				if(components != null)
				{
					components.Dispose();
				}

				_instPNLMain = null;
			}
			base.Dispose( disposing );
		}


		private static PNLMain _instPNLMain ;

		/// <summary>
		/// Singleton instance for the newsStory form
		/// </summary>
		/// <returns></returns>
		public static PNLMain GetInstance()
		{
			if(_instPNLMain == null || _instPNLMain.IsDisposed)
				_instPNLMain = new PNLMain();

			return _instPNLMain ;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("PNLToolBar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Chart");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Chart");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PNLMain));
            this.tabPageControlAsset = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlAssetPanel = new System.Windows.Forms.Panel();
            this.pnlTabTop = new System.Windows.Forms.Panel();
            this.currencyCombo = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.tabPageControlSymbol = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlSymbolPanel = new System.Windows.Forms.Panel();
            this.tabPageControlTradingAccount = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlTradingAccountPanel = new System.Windows.Forms.Panel();
            this.ultraPNLTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.tabSharedControlsPage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.pnlToolBar = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this._PNLMain_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._PNLMain_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._PNLMain_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._PNLMain_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.tabPageControlAsset.SuspendLayout();
            this.pnlTabTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currencyCombo)).BeginInit();
            this.tabPageControlSymbol.SuspendLayout();
            this.tabPageControlTradingAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraPNLTabControl)).BeginInit();
            this.ultraPNLTabControl.SuspendLayout();
            this.tabSharedControlsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlToolBar)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPageControlAsset
            // 
            this.tabPageControlAsset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPageControlAsset.Controls.Add(this.pnlAssetPanel);
            this.tabPageControlAsset.Controls.Add(this.pnlTabTop);
            this.tabPageControlAsset.Location = new System.Drawing.Point(1, 20);
            this.tabPageControlAsset.Name = "tabPageControlAsset";
            this.tabPageControlAsset.Size = new System.Drawing.Size(692, 215);
            // 
            // pnlAssetPanel
            // 
            this.pnlAssetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAssetPanel.Location = new System.Drawing.Point(0, 20);
            this.pnlAssetPanel.Name = "pnlAssetPanel";
            this.pnlAssetPanel.Size = new System.Drawing.Size(690, 193);
            this.pnlAssetPanel.TabIndex = 1;
            // 
            // pnlTabTop
            // 
            this.pnlTabTop.Controls.Add(this.currencyCombo);
            this.pnlTabTop.Controls.Add(this.lblCurrency);
            this.pnlTabTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTabTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTabTop.Name = "pnlTabTop";
            this.pnlTabTop.Size = new System.Drawing.Size(690, 20);
            this.pnlTabTop.TabIndex = 0;
            // 
            // currencyCombo
            // 
            appearance1.BorderColor = System.Drawing.Color.Black;
            this.currencyCombo.Appearance = appearance1;
            this.currencyCombo.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.currencyCombo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.currencyCombo.Dock = System.Windows.Forms.DockStyle.Left;
            this.currencyCombo.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.currencyCombo.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.currencyCombo.Location = new System.Drawing.Point(96, 0);
            this.currencyCombo.Name = "currencyCombo";
            this.currencyCombo.Size = new System.Drawing.Size(144, 20);
            this.currencyCombo.TabIndex = 4;
            this.currencyCombo.SelectionChanged += new System.EventHandler(this.currencyCombo_SelectionChanged);
            // 
            // lblCurrency
            // 
            this.lblCurrency.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblCurrency.Location = new System.Drawing.Point(0, 0);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(96, 20);
            this.lblCurrency.TabIndex = 3;
            this.lblCurrency.Text = "Currency";
            this.lblCurrency.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPageControlSymbol
            // 
            this.tabPageControlSymbol.Controls.Add(this.pnlSymbolPanel);
            this.tabPageControlSymbol.Location = new System.Drawing.Point(-10000, -10000);
            this.tabPageControlSymbol.Name = "tabPageControlSymbol";
            this.tabPageControlSymbol.Size = new System.Drawing.Size(692, 215);
            // 
            // pnlSymbolPanel
            // 
            this.pnlSymbolPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSymbolPanel.Location = new System.Drawing.Point(0, 0);
            this.pnlSymbolPanel.Name = "pnlSymbolPanel";
            this.pnlSymbolPanel.Size = new System.Drawing.Size(692, 215);
            this.pnlSymbolPanel.TabIndex = 1;
            // 
            // tabPageControlTradingAccount
            // 
            this.tabPageControlTradingAccount.Controls.Add(this.pnlTradingAccountPanel);
            this.tabPageControlTradingAccount.Location = new System.Drawing.Point(-10000, -10000);
            this.tabPageControlTradingAccount.Name = "tabPageControlTradingAccount";
            this.tabPageControlTradingAccount.Size = new System.Drawing.Size(692, 215);
            // 
            // pnlTradingAccountPanel
            // 
            this.pnlTradingAccountPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTradingAccountPanel.Location = new System.Drawing.Point(0, 0);
            this.pnlTradingAccountPanel.Name = "pnlTradingAccountPanel";
            this.pnlTradingAccountPanel.Size = new System.Drawing.Size(692, 215);
            this.pnlTradingAccountPanel.TabIndex = 1;
            // 
            // ultraPNLTabControl
            // 
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance2.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ultraPNLTabControl.ActiveTabAppearance = appearance2;
            this.ultraPNLTabControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ultraPNLTabControl.Controls.Add(this.tabSharedControlsPage);
            this.ultraPNLTabControl.Controls.Add(this.tabPageControlAsset);
            this.ultraPNLTabControl.Controls.Add(this.tabPageControlSymbol);
            this.ultraPNLTabControl.Controls.Add(this.tabPageControlTradingAccount);
            this.ultraPNLTabControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ultraPNLTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPNLTabControl.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraPNLTabControl.Location = new System.Drawing.Point(0, 24);
            this.ultraPNLTabControl.Name = "ultraPNLTabControl";
            this.ultraPNLTabControl.SharedControls.AddRange(new System.Windows.Forms.Control[] {
            this.pnlTabTop});
            this.ultraPNLTabControl.SharedControlsPage = this.tabSharedControlsPage;
            this.ultraPNLTabControl.Size = new System.Drawing.Size(694, 236);
            this.ultraPNLTabControl.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.ultraPNLTabControl.TabButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
            this.ultraPNLTabControl.TabIndex = 1;
            ultraTab1.Key = "Asset Class";
            ultraTab1.TabPage = this.tabPageControlAsset;
            ultraTab1.Text = "Asset Class";
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            ultraTab2.ActiveAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            ultraTab2.Appearance = appearance4;
            ultraTab2.Key = "Symbol";
            ultraTab2.TabPage = this.tabPageControlSymbol;
            ultraTab2.Text = "Symbol";
            ultraTab2.ToolTipText = "Symbol";
            ultraTab3.Key = "Trading Account";
            ultraTab3.TabPage = this.tabPageControlTradingAccount;
            ultraTab3.Text = "Trading Account";
            this.ultraPNLTabControl.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3});
            this.ultraPNLTabControl.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            this.ultraPNLTabControl.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraPNLTabControl_SelectedTabChanged);
            // 
            // tabSharedControlsPage
            // 
            this.tabSharedControlsPage.Controls.Add(this.pnlTabTop);
            this.tabSharedControlsPage.Location = new System.Drawing.Point(-10000, -10000);
            this.tabSharedControlsPage.Name = "tabSharedControlsPage";
            this.tabSharedControlsPage.Size = new System.Drawing.Size(692, 215);
            // 
            // pnlToolBar
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.pnlToolBar.Appearance = appearance5;
            this.pnlToolBar.DesignerFlags = 1;
            this.pnlToolBar.DockWithinContainer = this;
            this.pnlToolBar.ImageListSmall = this.imageList;
            this.pnlToolBar.LockToolbars = true;
            this.pnlToolBar.RuntimeCustomizationOptions = Infragistics.Win.UltraWinToolbars.RuntimeCustomizationOptions.None;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.Text = "PNLToolBar";
            ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2});
            this.pnlToolBar.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance6.Image = 2;
            buttonTool3.SharedProps.AppearancesSmall.Appearance = appearance6;
            buttonTool3.SharedProps.Caption = "Preferences";
            buttonTool3.SharedProps.Category = "PNLToolBarGroup";
            buttonTool3.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance7.Image = 0;
            buttonTool4.SharedProps.AppearancesSmall.Appearance = appearance7;
            buttonTool4.SharedProps.Caption = "Chart";
            buttonTool4.SharedProps.Category = "PNLToolBarGroup";
            buttonTool4.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            this.pnlToolBar.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool3,
            buttonTool4});
            this.pnlToolBar.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.pnlToolBar_ToolClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            // 
            // _PNLMain_Toolbars_Dock_Area_Left
            // 
            this._PNLMain_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PNLMain_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._PNLMain_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._PNLMain_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PNLMain_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 24);
            this._PNLMain_Toolbars_Dock_Area_Left.Name = "_PNLMain_Toolbars_Dock_Area_Left";
            this._PNLMain_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 236);
            this._PNLMain_Toolbars_Dock_Area_Left.ToolbarsManager = this.pnlToolBar;
            // 
            // _PNLMain_Toolbars_Dock_Area_Right
            // 
            this._PNLMain_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PNLMain_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._PNLMain_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._PNLMain_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PNLMain_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(694, 24);
            this._PNLMain_Toolbars_Dock_Area_Right.Name = "_PNLMain_Toolbars_Dock_Area_Right";
            this._PNLMain_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 236);
            this._PNLMain_Toolbars_Dock_Area_Right.ToolbarsManager = this.pnlToolBar;
            // 
            // _PNLMain_Toolbars_Dock_Area_Top
            // 
            this._PNLMain_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PNLMain_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._PNLMain_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._PNLMain_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PNLMain_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._PNLMain_Toolbars_Dock_Area_Top.Name = "_PNLMain_Toolbars_Dock_Area_Top";
            this._PNLMain_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(694, 24);
            this._PNLMain_Toolbars_Dock_Area_Top.ToolbarsManager = this.pnlToolBar;
            // 
            // _PNLMain_Toolbars_Dock_Area_Bottom
            // 
            this._PNLMain_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PNLMain_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._PNLMain_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._PNLMain_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PNLMain_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 260);
            this._PNLMain_Toolbars_Dock_Area_Bottom.Name = "_PNLMain_Toolbars_Dock_Area_Bottom";
            this._PNLMain_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(694, 0);
            this._PNLMain_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.pnlToolBar;
            // 
            // PNLMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(694, 260);
            this.Controls.Add(this.ultraPNLTabControl);
            this.Controls.Add(this._PNLMain_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._PNLMain_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._PNLMain_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._PNLMain_Toolbars_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "PNLMain";
            this.Text = " PNL";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.PNLMain_Closing);
            this.Load += new System.EventHandler(this.PNLMain_Load);
            this.tabPageControlAsset.ResumeLayout(false);
            this.pnlTabTop.ResumeLayout(false);
            this.pnlTabTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currencyCombo)).EndInit();
            this.tabPageControlSymbol.ResumeLayout(false);
            this.tabPageControlTradingAccount.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraPNLTabControl)).EndInit();
            this.ultraPNLTabControl.ResumeLayout(false);
            this.tabSharedControlsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlToolBar)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void ultraPNLTabControl_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
			if(e.PreviousSelectedTab != null)
			{
				switch(e.PreviousSelectedTab.Key)
				{
					case "Asset Class":
						this.tabPageControlAsset.Controls.Remove(pnlTabTop);
						//this.RefreshPNL -=new EventHandler(this.pnlGridControl.RefreshPNL);
						//this.pnlGridControl.UnloadPNL();
						break;
					case "Symbol":
						this.tabPageControlSymbol.Controls.Remove(pnlTabTop);
						//this.RefreshPNL -=new EventHandler(this.pnlGridControl.RefreshPNL);
						//this.pnlGridControl.UnloadPNL();
						break;
					case "Trading Account":
						this.tabPageControlTradingAccount.Controls.Remove(pnlTabTop);
						//this.RefreshPNL -=new EventHandler(this.pnlGridControl.RefreshPNL);
						//this.pnlGridControl.UnloadPNL();
						break;
				}
			}
										
			switch(e.Tab.Key)
			{
				case "Asset Class":


					this.tabPageControlAsset.Controls.Add(pnlTabTop);
					//LoadPNLGridControl((int)PNLTab.Asset, this._pnlPrefrences.Level1AssetColumn,this._pnlPrefrences.Level2AssetColumn);

					break;

				case "Symbol":

					this.tabPageControlSymbol.Controls.Add(pnlTabTop);
					
					//LoadPNLGridControl((int)PNLTab.Symbol, this._pnlPrefrences.Level1SymbolColumn,this._pnlPrefrences.Level2SymbolColumn);

					break;

				case "Trading Account":

					this.tabPageControlTradingAccount.Controls.Add(pnlTabTop);
					
					//LoadPNLGridControl((int)PNLTab.TradingAccount, this._pnlPrefrences.Level1TradingAccountColumn,this._pnlPrefrences.Level2TradingAccountColumn);

					break;
			}		
		}

		/// <summary>
		/// Create an instance of pnlGridControl if does not exists and add it to shared page
		/// </summary>
		/// <param name="tabType"></param>
		/// <param name="level1ColumnIndex"></param>
		/// <param name="level2ColumnIndex"></param>
		private void LoadPNLGridControl(int tabType, int level1ColumnIndex, int level2ColumnIndex)
		{
			try
			{
				switch(tabType)
				{
					case (int)PNLTab.Asset:
					
						if(pnlAssetControl == null)
						{
							this.pnlAssetControl = new PNLGridControl(this._pnlPrefrences, this.currencyConversionCollection, this.liveFeedManager, this._userID);
					
							this.pnlAssetControl.Dock = DockStyle.Fill;

							this.pnlAssetPanel.Controls.Add(this.pnlAssetControl);
				
	

							this.pnlAssetControl.TabType = tabType;

							this.pnlAssetControl.Level1ColumnIndex = level1ColumnIndex;

							this.pnlAssetControl.Level2ColumnIndex = level2ColumnIndex;

							if(this.currencyCombo.SelectedIndex == -1)
							{
								this.pnlAssetControl.SelectedCurrencyID = defaultCurrencyID;
							}
							else
							{
								this.pnlAssetControl.SelectedCurrencyID = (int)this.currencyCombo.SelectedItem.DataValue;
							}
			
							//initialize the calculation
							this.pnlAssetControl.InitPNL();

							this.RefreshPNL +=new EventHandler(this.pnlAssetControl.RefreshPNL);

								}

					break;

					case (int)PNLTab.Symbol:
					
						if(pnlSymbolControl == null)
						{
							this.pnlSymbolControl = new PNLGridControl(this._pnlPrefrences, this.currencyConversionCollection, this.liveFeedManager, this._userID);
					
							this.pnlSymbolControl.Dock = DockStyle.Fill;

							this.pnlSymbolPanel.Controls.Add(this.pnlSymbolControl);
				
	

							this.pnlSymbolControl.TabType = tabType;

							this.pnlSymbolControl.Level1ColumnIndex = level1ColumnIndex;

							this.pnlSymbolControl.Level2ColumnIndex = level2ColumnIndex;

							if(this.currencyCombo.SelectedIndex == -1)
							{
								this.pnlSymbolControl.SelectedCurrencyID = defaultCurrencyID;
							}
							else
							{
								this.pnlSymbolControl.SelectedCurrencyID = (int)this.currencyCombo.SelectedItem.DataValue;
							}
			
							//initialize the calculation
							this.pnlSymbolControl.InitPNL();

								this.RefreshPNL +=new EventHandler(this.pnlSymbolControl.RefreshPNL);

						}

					break;

					case (int)PNLTab.TradingAccount:
					
						if(pnlTradingAccountControl == null)
						{
							this.pnlTradingAccountControl = new PNLGridControl(this._pnlPrefrences, this.currencyConversionCollection, this.liveFeedManager, this._userID);
					
							this.pnlTradingAccountControl.Dock = DockStyle.Fill;

							this.pnlTradingAccountPanel.Controls.Add(this.pnlTradingAccountControl);
	

							this.pnlTradingAccountControl.TabType = tabType;

							this.pnlTradingAccountControl.Level1ColumnIndex = level1ColumnIndex;

							this.pnlTradingAccountControl.Level2ColumnIndex = level2ColumnIndex;

							if(this.currencyCombo.SelectedIndex == -1)
							{
								this.pnlTradingAccountControl.SelectedCurrencyID = defaultCurrencyID;
							}
							else
							{
								this.pnlTradingAccountControl.SelectedCurrencyID = (int)this.currencyCombo.SelectedItem.DataValue;
							}
			
							//initialize the calculation
							this.pnlTradingAccountControl.InitPNL();

							this.RefreshPNL +=new EventHandler(this.pnlTradingAccountControl.RefreshPNL);

						}

					break;

				}

			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
		}

		private void PNLMain_PerSecond(object sender, EventArgs e)
		{
            ///Changed 10 Apr 2007
            if (_pnlPrefrences == null)
            {
                return;
            }
			int refreshRate = _pnlPrefrences.RefreshRate;

			if(secondCounter++ == refreshRate)
			{
				if(RefreshPNL != null && liveFeedManager.IsDataManagerConnected())
					RefreshPNL(sender, e);

				secondCounter = 0;
			}
		}

		private void populateCurrencyCombo()
		{
			currencyCombo.Items.Clear();			

			for(int i=0;i<this.currencyList.Count;i++)			
			{
				Infragistics.Win.ValueListItem item = new Infragistics.Win.ValueListItem();
				item.DisplayText  = ((Currency)this.currencyList[i]).CurrencyName;
				item.DataValue = ((Currency)this.currencyList[i]).CurrencyID;
				this.currencyCombo.Items.Add(item);
			}

			//Set all three dropdown to the first value
			this.currencyCombo.SelectionChanged-=new EventHandler(currencyCombo_SelectionChanged);
			this.currencyCombo.SelectedIndex = 0;
			this.currencyCombo.SelectionChanged+=new EventHandler(currencyCombo_SelectionChanged);
		}

		private void currencyCombo_SelectionChanged(object sender, System.EventArgs e)
		{
			if(this.currencyCombo.SelectedIndex != -1)
			{
                this.pnlAssetControl.SelectedCurrencyID = (int)this.currencyCombo.SelectedItem.DataValue;
                this.pnlAssetControl.InitPNL();

				this.pnlSymbolControl.SelectedCurrencyID = (int)this.currencyCombo.SelectedItem.DataValue;
				this.pnlSymbolControl.InitPNL();

                this.pnlTradingAccountControl.SelectedCurrencyID = (int)this.currencyCombo.SelectedItem.DataValue;
                this.pnlTradingAccountControl.InitPNL();

			}			
		}
	
		private void pnlToolBar_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			switch(e.Tool.Key.ToLower())
			{
				//launch preferences
				case "preferences":

					if(LaunchPreferences != null)
						LaunchPreferences(sender, e);

//					pnlPreferences = PNLPreferences.GetInstance();
//					pnlPreferences .SaveClick +=new EventHandler(PNLMain_SaveClick);
//					pnlPreferences .ApplyPreferences +=new EventHandler(PNLMain_ApplyPreferences);
//					pnlPreferences.Owner = this.Owner;
//					pnlPreferences.ShowInTaskbar = false;
//					pnlPreferences.Width = 552;
//					pnlPreferences.Height = 408;
//					pnlPreferences.Show();
//					pnlPreferences.Focus();

					break;

				//launch chart control
				case "chart":
					pnlChart = PNLChart.GetInstance();
					pnlChart.UserID = this._userID;
					pnlChart.Preferences = _pnlPrefrences;
					pnlChart.ShowInTaskbar = false;
					pnlChart.Owner = this.Owner;
					pnlChart.Show();	
					pnlChart.Focus();
					break;
			}
		}

		#region IPNL interface implementation
		/// <summary>
		/// 
		/// </summary>
		public  void ApplyPreferences(string moduleName, IPreferenceData prefs)
		{
			try
			{
				secondCounter = 0;

				//Return if preferences not updated for this module
				if(!moduleName.Equals(Global.Common.PNL_MODULE)) return;

				//Load the preferences
				_pnlPrefrences = (PNLPrefrencesData) prefs;

				UpdatePreferences();
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
			}
		}

		public void UpdatePreferences()
		{
			//Apply the prefereces on Grid
			#region Asset Tab
			this.RefreshPNL -=new EventHandler(this.pnlAssetControl.RefreshPNL);
			this.pnlAssetControl.Preferences = _pnlPrefrences;
			this.pnlAssetControl.UpdateGridPreference();
			this.RefreshPNL +=new EventHandler(this.pnlAssetControl.RefreshPNL);
			#endregion

			#region Symbol Tab
			this.RefreshPNL -=new EventHandler(this.pnlSymbolControl.RefreshPNL);
			this.pnlSymbolControl.Preferences = _pnlPrefrences;
			this.pnlSymbolControl.UpdateGridPreference();
			this.RefreshPNL +=new EventHandler(this.pnlSymbolControl.RefreshPNL);
			#endregion
			
			#region Trading Account Tab
			this.RefreshPNL -=new EventHandler(this.pnlTradingAccountControl.RefreshPNL);
			this.pnlTradingAccountControl.Preferences = _pnlPrefrences;
			this.pnlTradingAccountControl.UpdateGridPreference();
			this.RefreshPNL +=new EventHandler(this.pnlTradingAccountControl.RefreshPNL);
			#endregion

			//Apply the prefereces on Chart
			if(pnlChart != null)
			{
				pnlChart.Preferences = _pnlPrefrences;

				pnlChart.UpdateChartPreferences();
			}
		}
		#endregion

		private void PNLMain_ApplyPreferences(object sender, EventArgs e)
		{
			
		
		}

		/// <summary>
		/// Fires an event to notify NirvanMain when PNL Module if closed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PNLMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(this.PNLClosed != null)
				this.PNLClosed(sender, e);
		}

		private void PNLMain_Load(object sender, System.EventArgs e)
		{
            LoadPNLGridControl((int)PNLTab.Asset, this._pnlPrefrences.Level1AssetColumn, this._pnlPrefrences.Level2AssetColumn);
            LoadPNLGridControl((int)PNLTab.Symbol, this._pnlPrefrences.Level1SymbolColumn, this._pnlPrefrences.Level2SymbolColumn);
            LoadPNLGridControl((int)PNLTab.TradingAccount, this._pnlPrefrences.Level1TradingAccountColumn, this._pnlPrefrences.Level2TradingAccountColumn);
		}
	}
}
