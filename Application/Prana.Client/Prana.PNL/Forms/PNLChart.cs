using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Nirvana.BusinessObjects;
using Nirvana.Interfaces;
using Nirvana.Global;
using Nirvana.LiveFeedProvider;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;

using Nirvana.CommonDataCache;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Nirvana.PNL
{
	/// <summary>
	/// Summary description for PNLChart.
	/// </summary>
	public class PNLChart : System.Windows.Forms.Form
	{
		private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraPNLChartTabControl;
		private int secondCounter = 0;
		private int defaultCurrencyID = Int32.MinValue;
		private PNLPrefrencesData _pnlPrefrences = null;
		public event EventHandler RefreshPNL;
		private ILiveFeedManager liveFeedManager;
		/// <summary>
		/// To hold the Currency Conversion Detail
		/// </summary>
		CurrencyConversionCollection currencyConversionCollection;
		private Nirvana.PNL.PNLChartControl pnlChartAsset;
		private Nirvana.PNL.PNLChartControl pnlChartSymbol;
		private Nirvana.PNL.PNLChartControl pnlChartTradingAccount;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage tabSharedControlsPage;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private const string FORM_NAME = "PNL: Chart";
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabAssetChart;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabSymbolChart;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabTradingAccountChart;
		private int _userID;

		public PNLPrefrencesData Preferences
		{
			set
			{
				this._pnlPrefrences = value;
			}
		}

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

		public PNLChart()
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();

				//Receive the timer event for every 100 ms
				CentralTimer.GetInstance().PerSecond +=new EventHandler(PNLMain_PerSecond);
			
				//Get the CurrencyConversion details
				currencyConversionCollection = CurrencyDataManager.GetInstance().GetCurrencyConversions();
			
				//Set the defaultCurrencyID to USD
				defaultCurrencyID = Global.Common.USDollar;
			
				//Get the prefereces
				_pnlPrefrences = new PNLPrefrencesData();
				_pnlPrefrences.SetDefaultPreferences();
				liveFeedManager = eSignalManager.GetInstance();
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
				this.RefreshPNL -=new EventHandler(this.pnlChartAsset.RefreshChart);
				this.RefreshPNL -=new EventHandler(this.pnlChartSymbol.RefreshChart);
				this.RefreshPNL -=new EventHandler(this.pnlChartTradingAccount.RefreshChart);

				if(_pnlPrefrences != null)
					_pnlPrefrences = null;

				if(liveFeedManager != null)
					liveFeedManager = null;

				if(this.pnlChartTradingAccount != null)
				{
					 
					this.pnlChartTradingAccount.Dispose();
				}

				if(this.pnlChartAsset != null)
					this.pnlChartAsset.Dispose();

				if(this.pnlChartSymbol != null)
					this.pnlChartSymbol.Dispose();

				if(currencyConversionCollection != null)
					currencyConversionCollection = null;

				if(_pnlPrefrences != null)
				{
					_pnlPrefrences = null;
				}

				if(components != null)
				{
					components.Dispose();
				}

				_instPNLChart = null;

			}
			base.Dispose( disposing );
		}

		private static PNLChart _instPNLChart ;

		/// <summary>
		/// Singleton instance for the newsStory form
		/// </summary>
		/// <returns></returns>
		public static PNLChart GetInstance()
		{
			if(_instPNLChart == null || _instPNLChart.IsDisposed)
				_instPNLChart = new PNLChart();

			return _instPNLChart ;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PNLChart));
			this.tabAssetChart = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.tabSymbolChart = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.tabTradingAccountChart = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraPNLChartTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.tabSharedControlsPage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			((System.ComponentModel.ISupportInitialize)(this.ultraPNLChartTabControl)).BeginInit();
			this.ultraPNLChartTabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabAssetChart
			// 
			this.tabAssetChart.Location = new System.Drawing.Point(1, 20);
			this.tabAssetChart.Name = "tabAssetChart";
			this.tabAssetChart.Size = new System.Drawing.Size(606, 352);
			// 
			// tabSymbolChart
			// 
			this.tabSymbolChart.Location = new System.Drawing.Point(-10000, -10000);
			this.tabSymbolChart.Name = "tabSymbolChart";
			this.tabSymbolChart.Size = new System.Drawing.Size(534, 297);
			// 
			// tabTradingAccountChart
			// 
			this.tabTradingAccountChart.Location = new System.Drawing.Point(-10000, -10000);
			this.tabTradingAccountChart.Name = "tabTradingAccountChart";
			this.tabTradingAccountChart.Size = new System.Drawing.Size(534, 297);
			// 
			// ultraPNLChartTabControl
			// 
			appearance1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance1.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance1.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.ultraPNLChartTabControl.ActiveTabAppearance = appearance1;
			appearance2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ultraPNLChartTabControl.Appearance = appearance2;
			this.ultraPNLChartTabControl.Controls.Add(this.tabSharedControlsPage);
			this.ultraPNLChartTabControl.Controls.Add(this.tabAssetChart);
			this.ultraPNLChartTabControl.Controls.Add(this.tabSymbolChart);
			this.ultraPNLChartTabControl.Controls.Add(this.tabTradingAccountChart);
			this.ultraPNLChartTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ultraPNLChartTabControl.Location = new System.Drawing.Point(0, 0);
			this.ultraPNLChartTabControl.Name = "ultraPNLChartTabControl";
			this.ultraPNLChartTabControl.SharedControlsPage = this.tabSharedControlsPage;
			this.ultraPNLChartTabControl.Size = new System.Drawing.Size(608, 373);
			this.ultraPNLChartTabControl.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.ultraPNLChartTabControl.TabButtonStyle = Infragistics.Win.UIElementButtonStyle.VisualStudio2005Button;
			this.ultraPNLChartTabControl.TabIndex = 0;
			ultraTab1.Key = "Asset Class";
			ultraTab1.TabPage = this.tabAssetChart;
			ultraTab1.Text = "Asset Class";
			ultraTab2.Key = "Symbol";
			ultraTab2.TabPage = this.tabSymbolChart;
			ultraTab2.Text = "Symbol";
			ultraTab3.Key = "Trading Account";
			ultraTab3.TabPage = this.tabTradingAccountChart;
			ultraTab3.Text = "Trading Account";
			this.ultraPNLChartTabControl.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																											  ultraTab1,
																											  ultraTab2,
																											  ultraTab3});
			this.ultraPNLChartTabControl.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
			this.ultraPNLChartTabControl.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.ultraPNLChartTabControl_SelectedTabChanged);
			// 
			// tabSharedControlsPage
			// 
			this.tabSharedControlsPage.Location = new System.Drawing.Point(-10000, -10000);
			this.tabSharedControlsPage.Name = "tabSharedControlsPage";
			this.tabSharedControlsPage.Size = new System.Drawing.Size(606, 352);
			// 
			// PNLChart
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(608, 373);
			this.Controls.Add(this.ultraPNLChartTabControl);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "PNLChart";
			this.Text = " PNL Chart";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.PNLChart_Closing);
			this.Load += new System.EventHandler(this.PNLChart_Load);
			((System.ComponentModel.ISupportInitialize)(this.ultraPNLChartTabControl)).EndInit();
			this.ultraPNLChartTabControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ultraPNLChartTabControl_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
//			if(e.PreviousSelectedTab != null)
//			{
//				switch(e.PreviousSelectedTab.Key)
//				{
//					case "Asset Class":
//						this.RefreshPNL -=new EventHandler(this.pnlChart.RefreshChart);
//						this.pnlChart.UnloadChart();
//						break;
//
//					case "Symbol":
//						this.RefreshPNL -=new EventHandler(this.pnlChart.RefreshChart);
//						this.pnlChart.UnloadChart();
//						break;
//
//					case "Trading Account":
//						this.RefreshPNL -=new EventHandler(this.pnlChart.RefreshChart);
//						this.pnlChart.UnloadChart();
//						break;
//				}
//			}	
			
//			switch(e.Tab.Key)
//			{
//				case "Asset Class":
//
//					LoadPNLChartControl((int)PNLTab.Asset,this._pnlPrefrences.Level1AssetColumn,this._pnlPrefrences.Level2AssetColumn);
//
//					break;
//
//				case "Symbol":
//
//					LoadPNLChartControl((int)PNLTab.Symbol,this._pnlPrefrences.Level1SymbolColumn,this._pnlPrefrences.Level2SymbolColumn);
//
//					break;
//
//				case "Trading Account":
//
//					LoadPNLChartControl((int)PNLTab.TradingAccount,this._pnlPrefrences.Level1TradingAccountColumn,this._pnlPrefrences.Level2TradingAccountColumn);
//
//					break;
//			}
		}

		private void PNLMain_PerSecond(object sender, EventArgs e)
		{
			int refreshRate = _pnlPrefrences.RefreshRate;

			if(secondCounter++ == refreshRate)
			{
				if(RefreshPNL != null)
					RefreshPNL(sender, e);

				secondCounter = 0;
			}

		}

		public void UpdateChartPreferences()
		{
			this.pnlChartAsset.Preferences = this._pnlPrefrences;
			this.pnlChartAsset.UpdateChartPreferences();

			this.pnlChartSymbol.Preferences = this._pnlPrefrences;
			this.pnlChartSymbol.UpdateChartPreferences();

			this.pnlChartTradingAccount.Preferences = this._pnlPrefrences;
			this.pnlChartTradingAccount.UpdateChartPreferences();
		}

		private void LoadPNLChartControl(int tabType, int level1ColumnIndex, int level2ColumnIndex)
		{

			switch(tabType)
			{
				case (int)PNLTab.Asset:

					if(pnlChartAsset == null)
					{
                        ShowWaitingMessage();
						this.pnlChartAsset = new PNLChartControl(this._pnlPrefrences, this.currencyConversionCollection, this.liveFeedManager, this._userID);
                        pnlChartAsset.DataReceived += new MethodInvoker(pnlChartAsset_DataReceived);
						this.pnlChartAsset.SelectedCurrencyID = this.defaultCurrencyID;
						this.pnlChartAsset.Dock = DockStyle.Fill;
						this.tabAssetChart.Controls.Add(this.pnlChartAsset);

						this.pnlChartAsset.TabType = tabType;
						this.pnlChartAsset.Level1ColumnIndex = level1ColumnIndex;
						this.pnlChartAsset.Level2ColumnIndex = level2ColumnIndex;
						this.pnlChartAsset.InitPNLChartControl();
                        
						this.RefreshPNL +=new EventHandler(this.pnlChartAsset.RefreshChart);

					}

				break;

				case (int)PNLTab.Symbol:

					if(pnlChartSymbol == null)
					{
						this.pnlChartSymbol = new PNLChartControl(this._pnlPrefrences, this.currencyConversionCollection, this.liveFeedManager, this._userID);
				
						this.pnlChartSymbol.SelectedCurrencyID = this.defaultCurrencyID;
						this.pnlChartSymbol.Dock = DockStyle.Fill;
						this.tabSymbolChart.Controls.Add(this.pnlChartSymbol);

						this.pnlChartSymbol.TabType = tabType;
						this.pnlChartSymbol.Level1ColumnIndex = level1ColumnIndex;
						this.pnlChartSymbol.Level2ColumnIndex = level2ColumnIndex;
						this.pnlChartSymbol.InitPNLChartControl();

						this.RefreshPNL +=new EventHandler(this.pnlChartSymbol.RefreshChart);
					}

				break;

				case (int)PNLTab.TradingAccount:

					if(pnlChartTradingAccount == null)
					{
						this.pnlChartTradingAccount = new PNLChartControl(this._pnlPrefrences, this.currencyConversionCollection, this.liveFeedManager, this._userID);
				
						this.pnlChartTradingAccount.SelectedCurrencyID = this.defaultCurrencyID;
						this.pnlChartTradingAccount.Dock = DockStyle.Fill;
						this.tabTradingAccountChart.Controls.Add(this.pnlChartTradingAccount);

						this.pnlChartTradingAccount.TabType = tabType;
						this.pnlChartTradingAccount.Level1ColumnIndex = level1ColumnIndex;
						this.pnlChartTradingAccount.Level2ColumnIndex = level2ColumnIndex;
						this.pnlChartTradingAccount.InitPNLChartControl();

						this.RefreshPNL +=new EventHandler(this.pnlChartTradingAccount.RefreshChart);
					}

				break;

			}

		}

        /// <summary>
        /// updated Rajat : 21 July 2006
        ///private bool blnIsChartProcessingComplete = false;
        /// As soon as the data is fetched for the livefeed, we will allow the user to work with the chart window
        /// </summary>
        void pnlChartAsset_DataReceived()
        {
            lblErrorMsg.Visible = false;
            this.Enabled = true;
            this.Cursor = System.Windows.Forms.Cursors.Hand;
        }

		private void PNLChart_Load(object sender, System.EventArgs e)
		{
			LoadPNLChartControl((int)PNLTab.Asset,this._pnlPrefrences.Level1AssetColumn,this._pnlPrefrences.Level2AssetColumn);
			LoadPNLChartControl((int)PNLTab.Symbol,this._pnlPrefrences.Level1SymbolColumn,this._pnlPrefrences.Level2SymbolColumn);
			LoadPNLChartControl((int)PNLTab.TradingAccount,this._pnlPrefrences.Level1TradingAccountColumn,this._pnlPrefrences.Level2TradingAccountColumn);
		}

		private void PNLChart_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
		}

        private System.Windows.Forms.Label lblErrorMsg;
        /// <summary>
        /// To display this error message untill the data is retrieved.
        /// </summary>
        private void ShowWaitingMessage()
        {
            if (this.lblErrorMsg == null)
            {
                this.lblErrorMsg = new Label();
                this.lblErrorMsg.Text = "Please wait while we retrieve the data.";
                this.lblErrorMsg.Size = new System.Drawing.Size(200, 23);
                this.lblErrorMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
                this.lblErrorMsg.BackColor = System.Drawing.Color.Red;
                this.lblErrorMsg.Location = new System.Drawing.Point(175, 72);
                this.lblErrorMsg.Name = "lblErrorMsg";
                this.lblErrorMsg.TabIndex = 0;
                this.tabAssetChart.Controls.Add(this.lblErrorMsg);
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.Enabled = false;
            }
            else
            {
                this.lblErrorMsg.Visible = true;
            }
        }

	}
}
