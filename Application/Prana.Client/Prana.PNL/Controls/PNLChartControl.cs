using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Nirvana.BusinessObjects;

using Nirvana.LiveFeedProvider;
using Nirvana.Interfaces;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;

using Nirvana.CommonDataCache;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.Global;

namespace Nirvana.PNL
{
	/// <summary>
	/// Summary description for PNLChartControl.
	/// </summary>
	public class PNLChartControl : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private DataTable dtExposure = null;
		private const string FORM_NAME = "PNL: PNLChartControl";
		private CurrencyConversionCollection _currencyConversionList = null;
		private Infragistics.Win.UltraWinTabControl.UltraTabControl ultraTabControl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
		private Infragistics.Win.UltraWinChart.UltraChart chartExposure;
        private System.Windows.Forms.Label lblErrorMsg;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl5;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl6;
		private int _userID;
		private bool blnCalculationStarted = false;

        /// <summary>
        /// Added Rajat - 22 Jan 2007 to facilitate the updation of orders and not
        /// fetching the whole bunch every time
        /// </summary>
        private Dictionary<string, int> _orderDict = new Dictionary<string, int>();

		#region PNL Chart variables
		// holds the currencyID
		private int _selectedCurrencyID = Int32.MinValue;
		// Holds the preferences data
		private PNLPrefrencesData _pnlPrefrences = null;
		//private TagDatabase _tagDatabase;
		private DataTable _orderSide;
		private int _level1ColumnIndex = int.MinValue;
		private int _level2ColumnIndex = int.MinValue;
		private OrderCollection OrderDetails =  null;
		private Hashtable htSymbolData;
		private Hashtable htLevel2Data;
		private int _tabType = int.MinValue;
		private ILiveFeedManager _liveFeedManager;
		private object[] NetExposureData = null;
		private object[] LongExposureData = null;
		private object[] ShortExposureData = null;
		private object[] NetPNLData = null;
		private object[] LongPNLData = null;
		private object[] ShortPNLData = null;
		private object[] ExecutedQuantityData = null;
		private int _lastRecordEventCount = 0;
		private ArrayList _lstReceivedSymbolList = new ArrayList();
		private int _symbolListCount = 0;
		private Hashtable htSymbol;
		private bool _applyingPreferences = false;
		private double conversionFactorFeedPrice = 1.0; //from USD to selected currency price
        
		#endregion

		#region properties

		public CurrencyConversionCollection CurrencyConversionList
		{
			set
			{
				this._currencyConversionList = value;
				//Check if selectedCurrencyID is not dollar
				if(this._selectedCurrencyID != Global.Common.USDollar) 
				{
					//find the conversion id from the list
					conversionFactorFeedPrice = PNLHelper.findConversionFactor(this._currencyConversionList, Global.Common.USDollar,this._selectedCurrencyID); 
				}
			}
		}

		public PNLPrefrencesData Prefrences
		{
			set
			{
				this._pnlPrefrences = value;
			}
		}

		public int SelectedCurrencyID
		{
			set
			{
				this._selectedCurrencyID = value;
				//Call method asynchronously to update the calculation
			}
		}

		public int Level1ColumnIndex
		{
			set
			{
				this._level1ColumnIndex = value;
			}
		}

		public int Level2ColumnIndex
		{
			set
			{
				this._level2ColumnIndex = value;
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

		public int TabType
		{
			set
			{
				this._tabType = value;
			}
		}

		public PNLPrefrencesData Preferences
		{
			set
			{
				this._pnlPrefrences = value;
			}
		}
		#endregion

		#region Constructure
		public PNLChartControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public PNLChartControl(PNLPrefrencesData pnlPrefrences, CurrencyConversionCollection currencyConversionCollection, ILiveFeedManager liveFeedManager, int userID)
		{
			this._pnlPrefrences = pnlPrefrences;
			this._currencyConversionList = currencyConversionCollection;
			this._liveFeedManager = liveFeedManager;
			//this._tagDatabase = TagDatabase.GetInstance();
			this._orderSide = TagDatabaseManager.GetInstance.GetAllOrderSides();
			this._liveFeedManager.Level1DataResponse+=new EventHandler(_liveFeedManager_Level1DataResponse);
			this._userID = userID;
	
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}
		#endregion

		#region Dispose
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				this._liveFeedManager.Level1DataResponse-=new EventHandler(_liveFeedManager_Level1DataResponse);

				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Component Designer generated code
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
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab5 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.Win.UltraWinTabControl.UltraTab ultraTab6 = new Infragistics.Win.UltraWinTabControl.UltraTab();
			Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement1 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
			this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraTabPageControl5 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraTabPageControl6 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
			this.ultraTabControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
			this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
			this.chartExposure = new Infragistics.Win.UltraWinChart.UltraChart();
			((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).BeginInit();
			this.ultraTabControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chartExposure)).BeginInit();
			this.SuspendLayout();
			// 
			// ultraTabPageControl1
			// 
			this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 1);
			this.ultraTabPageControl1.Name = "ultraTabPageControl1";
			this.ultraTabPageControl1.Size = new System.Drawing.Size(590, 0);
			// 
			// ultraTabPageControl2
			// 
			this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl2.Name = "ultraTabPageControl2";
			this.ultraTabPageControl2.Size = new System.Drawing.Size(590, 0);
			// 
			// ultraTabPageControl3
			// 
			this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl3.Name = "ultraTabPageControl3";
			this.ultraTabPageControl3.Size = new System.Drawing.Size(590, 0);
			// 
			// ultraTabPageControl4
			// 
			this.ultraTabPageControl4.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl4.Name = "ultraTabPageControl4";
			this.ultraTabPageControl4.Size = new System.Drawing.Size(590, 0);
			// 
			// ultraTabPageControl5
			// 
			this.ultraTabPageControl5.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl5.Name = "ultraTabPageControl5";
			this.ultraTabPageControl5.Size = new System.Drawing.Size(590, 0);
			// 
			// ultraTabPageControl6
			// 
			this.ultraTabPageControl6.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabPageControl6.Name = "ultraTabPageControl6";
			this.ultraTabPageControl6.Size = new System.Drawing.Size(590, 0);
			// 
			// ultraTabControl1
			// 
			appearance1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(152)), ((System.Byte)(10)));
			appearance1.BackColor2 = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			appearance1.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
			appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
			this.ultraTabControl1.ActiveTabAppearance = appearance1;
			appearance2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ultraTabControl1.Appearance = appearance2;
			this.ultraTabControl1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ultraTabControl1.Controls.Add(this.ultraTabSharedControlsPage1);
			this.ultraTabControl1.Controls.Add(this.ultraTabPageControl1);
			this.ultraTabControl1.Controls.Add(this.ultraTabPageControl4);
			this.ultraTabControl1.Controls.Add(this.ultraTabPageControl2);
			this.ultraTabControl1.Controls.Add(this.ultraTabPageControl3);
			this.ultraTabControl1.Controls.Add(this.ultraTabPageControl5);
			this.ultraTabControl1.Controls.Add(this.ultraTabPageControl6);
			this.ultraTabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ultraTabControl1.Location = new System.Drawing.Point(0, 308);
			this.ultraTabControl1.Name = "ultraTabControl1";
			this.ultraTabControl1.SharedControlsPage = this.ultraTabSharedControlsPage1;
			this.ultraTabControl1.Size = new System.Drawing.Size(592, 20);
			this.ultraTabControl1.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
			this.ultraTabControl1.TabIndex = 2;
			this.ultraTabControl1.TabOrientation = Infragistics.Win.UltraWinTabs.TabOrientation.BottomLeft;
			ultraTab1.Key = "Net Exposure";
			ultraTab1.TabPage = this.ultraTabPageControl1;
			ultraTab1.Text = "Net Exposure";
			ultraTab2.Key = "Long Exposure";
			ultraTab2.TabPage = this.ultraTabPageControl2;
			ultraTab2.Text = "Long Exposure";
			ultraTab3.Key = "Short Exposure";
			ultraTab3.TabPage = this.ultraTabPageControl3;
			ultraTab3.Text = "Short Exposure";
			ultraTab4.Key = "Net PNL";
			ultraTab4.TabPage = this.ultraTabPageControl4;
			ultraTab4.Text = "Net PNL";
			ultraTab5.Key = "Long PNL";
			ultraTab5.TabPage = this.ultraTabPageControl5;
			ultraTab5.Text = "Long PNL";
			ultraTab6.Key = "Short PNL";
			ultraTab6.TabPage = this.ultraTabPageControl6;
			ultraTab6.Text = "Short PNL";
			this.ultraTabControl1.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
																									   ultraTab1,
																									   ultraTab2,
																									   ultraTab3,
																									   ultraTab4,
																									   ultraTab5,
																									   ultraTab6});
			this.ultraTabControl1.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
			// 
			// ultraTabSharedControlsPage1
			// 
			this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
			this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
			this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(590, 0);
			// 
			//'UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
			//'ChartType' must be persisted ahead of any Axes change made in design time.
			//
			this.chartExposure.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ColumnChart3D;
			// 
			// chartExposure
			// 
			this.chartExposure.Axis.X.Labels.Flip = true;
			this.chartExposure.Axis.X.Labels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chartExposure.Axis.X.Labels.FontColor = System.Drawing.Color.White;
			this.chartExposure.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
			this.chartExposure.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;
			this.chartExposure.Axis.X.Labels.OrientationAngle = 247;
			this.chartExposure.Axis.X.Labels.SeriesLabels.Flip = true;
			this.chartExposure.Axis.X.Labels.SeriesLabels.FormatString = "";
			this.chartExposure.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
			this.chartExposure.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
			this.chartExposure.Axis.X.Labels.SeriesLabels.OrientationAngle = 213;
			this.chartExposure.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.X.ScrollScale.Height = 10;
			this.chartExposure.Axis.X.ScrollScale.Visible = false;
			this.chartExposure.Axis.X.ScrollScale.Width = 15;
			this.chartExposure.Axis.X.TickmarkInterval = 0;
			this.chartExposure.Axis.X2.Labels.Flip = false;
			this.chartExposure.Axis.X2.Labels.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.chartExposure.Axis.X2.Labels.FontColor = System.Drawing.Color.White;
			this.chartExposure.Axis.X2.Labels.ItemFormatString = "<ITEM_LABEL>";
			this.chartExposure.Axis.X2.Labels.OrientationAngle = 0;
			this.chartExposure.Axis.X2.Labels.SeriesLabels.Flip = false;
			this.chartExposure.Axis.X2.Labels.SeriesLabels.FormatString = "";
			this.chartExposure.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.VerticalLeftFacing;
			this.chartExposure.Axis.X2.Labels.SeriesLabels.OrientationAngle = 0;
			this.chartExposure.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.X2.Labels.Visible = false;
			this.chartExposure.Axis.X2.ScrollScale.Height = 10;
			this.chartExposure.Axis.X2.ScrollScale.Visible = false;
			this.chartExposure.Axis.X2.ScrollScale.Width = 15;
			this.chartExposure.Axis.X2.TickmarkInterval = 0;
			this.chartExposure.Axis.Y.Labels.Flip = false;
			this.chartExposure.Axis.Y.Labels.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.chartExposure.Axis.Y.Labels.FontColor = System.Drawing.Color.White;
			this.chartExposure.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Far;
			this.chartExposure.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;
			this.chartExposure.Axis.Y.Labels.OrientationAngle = -20;
			this.chartExposure.Axis.Y.Labels.SeriesLabels.Flip = false;
			this.chartExposure.Axis.Y.Labels.SeriesLabels.FormatString = "";
			this.chartExposure.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Far;
			this.chartExposure.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
			this.chartExposure.Axis.Y.Labels.SeriesLabels.OrientationAngle = 0;
			this.chartExposure.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.Y.ScrollScale.Height = 10;
			this.chartExposure.Axis.Y.ScrollScale.Visible = false;
			this.chartExposure.Axis.Y.ScrollScale.Width = 15;
			this.chartExposure.Axis.Y.TickmarkInterval = 0;
			this.chartExposure.Axis.Y2.Labels.Flip = false;
			this.chartExposure.Axis.Y2.Labels.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.chartExposure.Axis.Y2.Labels.FontColor = System.Drawing.Color.White;
			this.chartExposure.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
			this.chartExposure.Axis.Y2.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
			this.chartExposure.Axis.Y2.Labels.OrientationAngle = 0;
			this.chartExposure.Axis.Y2.Labels.SeriesLabels.Flip = false;
			this.chartExposure.Axis.Y2.Labels.SeriesLabels.FormatString = "";
			this.chartExposure.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
			this.chartExposure.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
			this.chartExposure.Axis.Y2.Labels.SeriesLabels.OrientationAngle = 0;
			this.chartExposure.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.Y2.Labels.Visible = false;
			this.chartExposure.Axis.Y2.ScrollScale.Height = 10;
			this.chartExposure.Axis.Y2.ScrollScale.Visible = false;
			this.chartExposure.Axis.Y2.ScrollScale.Width = 15;
			this.chartExposure.Axis.Y2.TickmarkInterval = 0;
			this.chartExposure.Axis.Z.Labels.Flip = false;
			this.chartExposure.Axis.Z.Labels.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.chartExposure.Axis.Z.Labels.FontColor = System.Drawing.Color.White;
			this.chartExposure.Axis.Z.Labels.ItemFormatString = "<DATA_VALUE:00>";
			this.chartExposure.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;
			this.chartExposure.Axis.Z.Labels.OrientationAngle = 8;
			this.chartExposure.Axis.Z.Labels.SeriesLabels.Flip = false;
			this.chartExposure.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
			this.chartExposure.Axis.Z.Labels.SeriesLabels.OrientationAngle = 0;
			this.chartExposure.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.Z.ScrollScale.Height = 10;
			this.chartExposure.Axis.Z.ScrollScale.Visible = false;
			this.chartExposure.Axis.Z.ScrollScale.Width = 15;
			this.chartExposure.Axis.Z.TickmarkInterval = 0;
			this.chartExposure.Axis.Z.TickmarkPercentage = 20;
			this.chartExposure.Axis.Z2.Labels.Flip = false;
			this.chartExposure.Axis.Z2.Labels.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.chartExposure.Axis.Z2.Labels.FontColor = System.Drawing.Color.White;
			this.chartExposure.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
			this.chartExposure.Axis.Z2.Labels.ItemFormatString = "";
			this.chartExposure.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
			this.chartExposure.Axis.Z2.Labels.OrientationAngle = 0;
			this.chartExposure.Axis.Z2.Labels.SeriesLabels.Flip = false;
			this.chartExposure.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
			this.chartExposure.Axis.Z2.Labels.SeriesLabels.OrientationAngle = 0;
			this.chartExposure.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
			this.chartExposure.Axis.Z2.Labels.Visible = false;
			this.chartExposure.Axis.Z2.ScrollScale.Height = 10;
			this.chartExposure.Axis.Z2.ScrollScale.Visible = false;
			this.chartExposure.Axis.Z2.ScrollScale.Width = 15;
			this.chartExposure.Axis.Z2.TickmarkInterval = 0;
			this.chartExposure.BackColor = System.Drawing.Color.Black;
			this.chartExposure.ColorModel.ColorBegin = System.Drawing.Color.Yellow;
			this.chartExposure.ColorModel.Scaling = Infragistics.UltraChart.Shared.Styles.ColorScaling.Random;
			this.chartExposure.Data.EmptyStyle.LegendDisplayType = Infragistics.UltraChart.Shared.Styles.LegendEmptyDisplayType.PE;
			this.chartExposure.Data.EmptyStyle.LineStyle.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dash;
			this.chartExposure.Data.EmptyStyle.LineStyle.EndStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.NoAnchor;
			this.chartExposure.Data.EmptyStyle.LineStyle.MidPointAnchors = false;
			this.chartExposure.Data.EmptyStyle.LineStyle.StartStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.NoAnchor;
			this.chartExposure.Data.RowLabelsColumn = 0;
			this.chartExposure.Data.SwapRowsAndColumns = true;
			this.chartExposure.Data.UseRowLabelsColumn = true;
			this.chartExposure.Data.ZeroAligned = true;
			this.chartExposure.Dock = System.Windows.Forms.DockStyle.Fill;
			this.chartExposure.EmptyChartText = "";
			this.chartExposure.EnableCrossHair = true;
			this.chartExposure.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chartExposure.Legend.BackgroundColor = System.Drawing.Color.Beige;
			this.chartExposure.Legend.BorderColor = System.Drawing.Color.Yellow;
			this.chartExposure.Legend.Font = new System.Drawing.Font("Tahoma", 8.25F);
			this.chartExposure.Legend.FontColor = System.Drawing.Color.White;
			this.chartExposure.Legend.FormatString = " <ITEM_LABEL>";
			this.chartExposure.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Top;
			this.chartExposure.Legend.Margins.Bottom = 1;
			this.chartExposure.Legend.Margins.Left = 1;
			this.chartExposure.Legend.Margins.Right = 1;
			this.chartExposure.Legend.Margins.Top = 1;
			this.chartExposure.Legend.SpanPercentage = 15;
			this.chartExposure.Legend.Visible = true;
			this.chartExposure.Location = new System.Drawing.Point(0, 0);
			this.chartExposure.Name = "chartExposure";
			this.chartExposure.Size = new System.Drawing.Size(592, 308);
			this.chartExposure.TabIndex = 4;
			this.chartExposure.TitleBottom.Extent = 0;
			this.chartExposure.TitleBottom.Margins.Bottom = 0;
			this.chartExposure.TitleBottom.Margins.Left = 0;
			this.chartExposure.TitleBottom.Margins.Right = 0;
			this.chartExposure.TitleBottom.Margins.Top = 0;
			this.chartExposure.TitleBottom.Text = "";
			this.chartExposure.TitleLeft.Extent = 0;
			this.chartExposure.TitleLeft.Text = "";
			this.chartExposure.TitleRight.Extent = 0;
			this.chartExposure.TitleRight.Text = "";
			this.chartExposure.TitleTop.Extent = 0;
			this.chartExposure.TitleTop.Margins.Bottom = 0;
			this.chartExposure.TitleTop.Margins.Left = 0;
			this.chartExposure.TitleTop.Margins.Right = 0;
			this.chartExposure.TitleTop.Margins.Top = 0;
			this.chartExposure.TitleTop.Text = "";
			this.chartExposure.Tooltips.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
			paintElement1.Fill = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(255)));
			this.chartExposure.Tooltips.PE = paintElement1;
			this.chartExposure.Tooltips.UseControl = false;
			this.chartExposure.Transform3D.Scale = 55F;
			this.chartExposure.Transform3D.YRotation = 15F;
			this.chartExposure.Visible = false;
			// 
			// PNLChartControl
			// 
			this.Controls.Add(this.chartExposure);
			this.Controls.Add(this.ultraTabControl1);
			this.Name = "PNLChartControl";
			this.Size = new System.Drawing.Size(592, 328);
			((System.ComponentModel.ISupportInitialize)(this.ultraTabControl1)).EndInit();
			this.ultraTabControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.chartExposure)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// To initialize the PNL Chart Control with UI and Data
		/// </summary>
		public void InitPNLChartControl()
		{
			try
			{
               
                this.chartExposure.Visible = false; 

				this.ultraTabControl1.SelectedTabChanged +=new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(ultraTabControl1_SelectedTabChanged);
				this.ultraTabControl1.Tabs[0].Selected = true;

				//initilize datatable
				this.MakeDataTable();

				this.UpdateChartUI();

				this.UpdateChartAsync();

			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());

				throw;
			}
		}

		/// <summary>
		/// Make the data table
		/// </summary>
		private void MakeDataTable()
		{
			//create a dataset with two table to show hirarichal data
			dtExposure = new DataTable("ChartData");
				
		}

		/// <summary>
		/// Calculate the profit and loss for the PNL data
		/// </summary>
		private void CalculatePNL(OrderCollection OrderDetails, Hashtable htSymbolData, Hashtable htLevel2Data, bool IsDataManagerConnected)
		{
			try
			{
				
				this.chartExposure.SuspendLayout(); 

				#region declare the variable to calculate the qty, exposure and pnl
				//its value is 1 in case of stocks
				int multiplier = 1; //Multiplier to be pickup from the database
				//its value is 1 in case of stocks
				int delta = 1;

				//string tradingAccountName = string.Empty;
				//string symbol= string.Empty;

				int taCount = 0;
                
				int totalLongExposure = 0;
				int totalShortExposure = 0;
				int totalShortPNL = 0;
				int totalLongPNL = 0;
				int totalQuantity = 0;
				
				int longExposure = 0;
				int shortExposure = 0;
				int shortPNL = 0;
				int longPNL = 0;
				int executedQuantity = 0;
				
				int colCount = htLevel2Data.Count + 1; //+1 for the last column "Total"

				NetExposureData = null;
				LongExposureData = null;
				ShortExposureData = null;
				NetPNLData = null;
				LongPNLData = null;
				ShortPNLData = null;
				ExecutedQuantityData = null;

				object[] ExposureDataNE = new object[colCount + 1];
				object[] ExposureDataLE = new object[colCount + 1];
				object[] ExposureDataSE = new object[colCount + 1];
				object[] ExposureDataNP = new object[colCount + 1];
				object[] ExposureDataLP = new object[colCount + 1];
				object[] ExposureDataSP = new object[colCount + 1];
				object[] ExposureDataEQ = new object[colCount + 1]; //executed quantity
				
				int level2ColumnIndex = this._level2ColumnIndex;
				string level2ColumnName = PNLPrefrencesData.PNLColumnNames[level2ColumnIndex];
				string level2ColumnValue = string.Empty;
				string level2ColumnDisplayValue = string.Empty;

				int level1ColumnIndex = this._level1ColumnIndex;
				string level1ColumnName = PNLPrefrencesData.PNLColumnNames[level1ColumnIndex];
				string level1ColumnValue = string.Empty;
				string level1ColumnDisplayValue = string.Empty;
				#endregion

				//apply here the logic to calculate the PNL
				for(int i=0;i<OrderDetails.Count;i++)
				{
					Order order = (Order)OrderDetails[i];

					if(level1ColumnValue == string.Empty)
					{
						level1ColumnValue = PNLHelper.GetColumnValue(level1ColumnIndex, order);
						level1ColumnDisplayValue = PNLHelper.GetChartDisplayValue(level1ColumnIndex, order);
					}
					if(!level1ColumnValue.Equals(PNLHelper.GetColumnValue(level1ColumnIndex, order)))
					{
						#region update calculation
						totalLongExposure +=longExposure;
						totalShortExposure += shortExposure;
						totalShortPNL += shortPNL;
						totalLongPNL += longPNL;
						totalQuantity += executedQuantity;
						#endregion

						#region Fill array with the data
						ExposureDataNE[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longExposure - shortExposure;
						ExposureDataNE[colCount] = totalLongExposure - totalShortExposure;
						ExposureDataLE[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longExposure;
						ExposureDataLE[colCount] = totalLongExposure;
						ExposureDataSE[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = shortExposure;
						ExposureDataSE[colCount] = totalShortExposure;
						ExposureDataNP[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longPNL + shortPNL;
						ExposureDataNP[colCount] = totalLongPNL + totalShortPNL;
						ExposureDataLP[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longPNL;
						ExposureDataLP[colCount] = totalLongPNL;
						ExposureDataSP[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = shortPNL;
						ExposureDataSP[colCount] = totalShortPNL;
						ExposureDataEQ[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = executedQuantity;
						ExposureDataEQ[colCount] = totalQuantity;
						#endregion
						
						#region resize the array
						if(NetExposureData == null)
							NetExposureData = new object[1];
						else
							NetExposureData = ReDim(NetExposureData);

						if(LongExposureData == null)
							LongExposureData = new object[1];
						else
							LongExposureData = ReDim(LongExposureData);

						if(ShortExposureData == null)
							ShortExposureData = new object[1];
						else
							ShortExposureData = ReDim(ShortExposureData);

						if(NetPNLData == null)
							NetPNLData = new object[1];
						else
							NetPNLData = ReDim(NetPNLData);

						if(LongPNLData == null)
							LongPNLData = new object[1];
						else
							LongPNLData = ReDim(LongPNLData);

						if(ShortPNLData == null)
							ShortPNLData = new object[1];
						else
							ShortPNLData = ReDim(ShortPNLData);

						if(ExecutedQuantityData == null)
							ExecutedQuantityData = new object[1];
						else
							ExecutedQuantityData = ReDim(ExecutedQuantityData);
						#endregion

						#region set the data to array
						ExposureDataNE[0] = level1ColumnDisplayValue;
						ExposureDataLE[0] = level1ColumnDisplayValue;
						ExposureDataSE[0] = level1ColumnDisplayValue;
						ExposureDataNP[0] = level1ColumnDisplayValue;
						ExposureDataLP[0] = level1ColumnDisplayValue;
						ExposureDataSP[0] = level1ColumnDisplayValue;
						ExposureDataEQ[0] = level1ColumnDisplayValue;
						#endregion
				
						#region assign data arraysto final array of objects
						NetExposureData[taCount] = ExposureDataNE;
						LongExposureData[taCount] = ExposureDataLE;
						ShortExposureData[taCount] = ExposureDataSE;
						NetPNLData[taCount] = ExposureDataNP;
						LongPNLData[taCount] = ExposureDataLP;
						ShortPNLData[taCount] = ExposureDataSP;
						ExecutedQuantityData[taCount] = ExposureDataEQ;
						#endregion
						
						taCount++;

						#region reset the array
						ExposureDataNE = new object[colCount + 1];
						ExposureDataLE = new object[colCount + 1];
						ExposureDataSE = new object[colCount + 1];
						ExposureDataNP = new object[colCount + 1];
						ExposureDataLP = new object[colCount + 1];
						ExposureDataSP = new object[colCount + 1];
						ExposureDataEQ = new object[colCount + 1];
						#endregion
						
						#region reset the variables
						level1ColumnValue = PNLHelper.GetColumnValue(level1ColumnIndex, order);
						level1ColumnDisplayValue = PNLHelper.GetChartDisplayValue(level1ColumnIndex, order);
						totalQuantity = 0;
						totalLongExposure = 0;
						totalShortExposure = 0;
						totalLongPNL = 0;
						totalShortPNL = 0;

						longExposure = 0;
						shortExposure = 0;
						longPNL = 0;
						shortPNL = 0;
						executedQuantity = 0;

						level2ColumnValue = string.Empty;
						#endregion

					}

					if(level1ColumnValue.Equals(PNLHelper.GetColumnValue(level1ColumnIndex, order)))
					{
						if(level2ColumnValue == string.Empty)
						{
							level2ColumnValue = PNLHelper.GetColumnValue(level2ColumnIndex, order);
							level2ColumnDisplayValue = PNLHelper.GetChartDisplayValue(level2ColumnIndex, order);
						}

						if(!level2ColumnValue.Equals(PNLHelper.GetColumnValue(level2ColumnIndex, order)))
						{

							#region update calculation
							totalLongExposure +=longExposure;
							totalShortExposure += shortExposure;
							totalShortPNL += shortPNL;
							totalLongPNL += longPNL;
							totalQuantity += executedQuantity;
							#endregion

							#region Fill array with the data
							ExposureDataNE[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longExposure - shortExposure;
							ExposureDataNE[colCount] = totalLongExposure - totalShortExposure;
							ExposureDataLE[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longExposure;
							ExposureDataLE[colCount] = totalLongExposure;
							ExposureDataSE[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = shortExposure;
							ExposureDataSE[colCount] = totalShortExposure;
							ExposureDataNP[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longPNL + shortPNL;
							ExposureDataNP[colCount] = totalLongPNL + totalShortPNL;
							ExposureDataLP[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longPNL;
							ExposureDataLP[colCount] = totalLongPNL;
							ExposureDataSP[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = shortPNL;
							ExposureDataSP[colCount] = totalShortPNL;
							ExposureDataEQ[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = executedQuantity;
							ExposureDataEQ[colCount] = totalQuantity;
							#endregion

							#region reset the variables
							level2ColumnValue = PNLHelper.GetColumnValue(level2ColumnIndex, order);
							level2ColumnDisplayValue = PNLHelper.GetChartDisplayValue(level2ColumnIndex, order);

							longExposure = 0;
							shortExposure = 0;
							longPNL = 0;
							shortPNL = 0;
							executedQuantity = 0;
							#endregion
						}

						if(level2ColumnValue.Equals(PNLHelper.GetColumnValue(level2ColumnIndex, order)))
						{
							string side = order.OrderSide;

							#region calculation logic
							/*
								* Exposure calculation:
								*
								*((# Units * Selected Feed)* Multiplier* Delta)/ * Currency
								*
								* NOTE: Long Exposure is for Buys + Buy to cover
								* Short Exposure is for sells and Shorts
								*
								* Where:
								*
								*•	Units= No of units ( +ve for Buys and BCV / -ve for sells and short sells) 
								*•	Selected fees (as above)
								*•	Multiplier = As above
								*•	Delta = this is a measure that will be calculated using a option formula
								*•	Currency as above
								*
								* PNL is the calculation of Profit / loss of a position. The calculation of PNL is:
								*
								* ((Units* (Selected Feed Price – Average Execution Price))* Multiplier ) / OR * Currency Conversion
								*
								* Where:
								*
								* •	Units = # of shares / # or Contracts – For Buys & Covers this is a +ve  / For sells and shorts this is -ve
								* •	Selected Feed Price = User preference whether the live feed selected is E.G user will have the ability to mark the PNL on Last, Bid, Offer
								* •	Multiplier = If the AU has a multiplier OR divisor, this needs to be considered 
								* •	If the PNL needs to be converted into a base currency, the value needs to be divided / multiplied (depending whether the conversion is direct or indirect) by the exchange rate.
								*
								*/
							#endregion

							double conversionFactor = 1.0;
							
								
							//Check for the Currency Type
							if(order.CurrencyID != this._selectedCurrencyID)
							{
								//find the conversion id from the list
								conversionFactor = PNLHelper.findConversionFactor(this._currencyConversionList, order.CurrencyID,this._selectedCurrencyID);
							}


									
							SymbolL1Data l1Data = (SymbolL1Data)htSymbolData[order.Symbol];
										
							double feedPriceLE = double.MinValue;
							double feedPriceSE = double.MinValue;
							double feedPriceLP = double.MinValue;
							double feedPriceSP = double.MinValue;
							
							if(l1Data != null)
							{
								if(IsDataManagerConnected)
								{
									feedPriceLE = PNLHelper.GetFeedValue(l1Data, _pnlPrefrences.LongExposoreCalculcationColumn);
									feedPriceSE = PNLHelper.GetFeedValue(l1Data, _pnlPrefrences.ShortExposureCalculationColumn);
									feedPriceLP = PNLHelper.GetFeedValue(l1Data, _pnlPrefrences.LongPNLCalculationColumn);
									feedPriceSP = PNLHelper.GetFeedValue(l1Data, _pnlPrefrences.ShortPNLCalculcationColumn);
								}
								else
								{
									feedPriceLE = feedPriceSE = feedPriceLP = feedPriceSP = PNLHelper.GetFeedValue(l1Data, _pnlPrefrences.DefaultCalculcationColumn);
								}
							}
							else
								feedPriceLE = feedPriceSE = feedPriceLP = feedPriceSP = 0; //Pick the last from the Security Master
									
							//Check here for Buy, Buy to Cover, Sell, Short Sell
							if(side.Equals(this._orderSide.Rows[1][1]) || side.Equals(this._orderSide.Rows[2][1])) //use some constant to check for buy/sell
							{
								longExposure += ((int)(Convert.ToInt32(order.CumQty)*feedPriceLE*conversionFactorFeedPrice))*multiplier*delta; //Get the current price value for symbol

								longPNL += (int)(Convert.ToInt32(order.CumQty)*(feedPriceLP*conversionFactorFeedPrice - Convert.ToDouble(order.AvgPrice)*conversionFactor)*multiplier);
							}
							else
							{
								shortExposure += ((int)(Convert.ToInt32(order.CumQty)*feedPriceSE*conversionFactorFeedPrice))*multiplier*delta;//Get the current price value for symbol
							
								shortPNL += (int)(-Convert.ToInt32(order.CumQty)*(feedPriceSP*conversionFactorFeedPrice - Convert.ToDouble(order.AvgPrice)*conversionFactor)*multiplier);
							}
						
							executedQuantity += Convert.ToInt32(order.CumQty); 
						}
					
					}
				}
			
				if(OrderDetails.Count > 0)
				{
					#region update calculation
					//to be test for the last record
					totalLongExposure +=longExposure;
					totalShortExposure += shortExposure;
					totalShortPNL += shortPNL;
					totalLongPNL += longPNL;
					totalQuantity += executedQuantity;
				    #endregion

					#region Fill array with the data
					ExposureDataNE[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longExposure - shortExposure;
					ExposureDataNE[colCount] = totalLongExposure - totalShortExposure;
					ExposureDataLE[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longExposure;
					ExposureDataLE[colCount] = totalLongExposure;
					ExposureDataSE[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = shortExposure;
					ExposureDataSE[colCount] = totalShortExposure;
					ExposureDataNP[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longPNL + shortPNL;
					ExposureDataNP[colCount] = totalLongPNL + totalShortPNL;
					ExposureDataLP[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = longPNL;
					ExposureDataLP[colCount] = totalLongPNL;
					ExposureDataSP[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = shortPNL;
					ExposureDataSP[colCount] = totalShortPNL;
					ExposureDataEQ[int.Parse((string)htLevel2Data[level2ColumnDisplayValue])+1] = executedQuantity;
					ExposureDataEQ[colCount] = totalQuantity;
					#endregion

					#region resize the array
					if(NetExposureData == null)
						NetExposureData = new object[1];
					else
						NetExposureData = ReDim(NetExposureData);

					if(LongExposureData == null)
						LongExposureData = new object[1];
					else
						LongExposureData = ReDim(LongExposureData);

					if(ShortExposureData == null)
						ShortExposureData = new object[1];
					else
						ShortExposureData = ReDim(ShortExposureData);

					if(NetPNLData == null)
						NetPNLData = new object[1];
					else
						NetPNLData = ReDim(NetPNLData);

					if(LongPNLData == null)
						LongPNLData = new object[1];
					else
						LongPNLData = ReDim(LongPNLData);

					if(ShortPNLData == null)
						ShortPNLData = new object[1];
					else
						ShortPNLData = ReDim(ShortPNLData);

					if(ExecutedQuantityData == null)
						ExecutedQuantityData = new object[1];
					else
						ExecutedQuantityData = ReDim(ExecutedQuantityData);
					#endregion

					#region set the data to array
					ExposureDataNE[0] = level1ColumnDisplayValue;
					ExposureDataLE[0] = level1ColumnDisplayValue;
					ExposureDataSE[0] = level1ColumnDisplayValue;
					ExposureDataNP[0] = level1ColumnDisplayValue;
					ExposureDataLP[0] = level1ColumnDisplayValue;
					ExposureDataSP[0] = level1ColumnDisplayValue;
					ExposureDataEQ[0] = level1ColumnDisplayValue;
					#endregion
				
					#region assign data arraysto final array of objects
					NetExposureData[taCount] = ExposureDataNE;
					LongExposureData[taCount] = ExposureDataLE;
					ShortExposureData[taCount] = ExposureDataSE;
					NetPNLData[taCount] = ExposureDataNP;
					LongPNLData[taCount] = ExposureDataLP;
					ShortPNLData[taCount] = ExposureDataSP;
					ExecutedQuantityData[taCount] = ExposureDataEQ;
					#endregion
				}

//				for(int i=0; i<NetExposureData.Length; i++) 
//				{
//					DataRow ro = this.dtExposure.NewRow();
//					ro.ItemArray = (object[])NetExposureData[i];
//					this.dtExposure.Rows.Add(ro);
//				}

				UpdateChartData(this.ultraTabControl1.SelectedTab.Key);

				this.chartExposure.ResumeLayout();

				if(this.chartExposure.InvokeRequired == false)
				{
					BindChartData(null, null);
				}
				else
				{
					Invoke(new EventHandler(BindChartData), new object[] { null, null});
				}

			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());

				throw;
			}
			finally
			{
				lock(this.chartExposure)
				{
					blnCalculationStarted = false;
				}
			}
		}


		private void BindChartData(object sender, EventArgs e)
		{
			this.chartExposure.Visible = true;
			this.chartExposure.DataSource = this.dtExposure;
			this.chartExposure.DataBind();

		}

		/// <summary>
		/// To update the PNL calculation with latest data.
		/// </summary>
		public void RefreshChart(object sender, EventArgs e)
		{
			if(_applyingPreferences == false)
			      UpdateChartAsync();
		}

		private void UpdateChartAsync()
		{
			try
			{
				lock(this.chartExposure)
				{
					if(this.blnCalculationStarted==true)
					{
						return;
					}

					this.blnCalculationStarted = true;
				}

				System.Windows.Forms.MethodInvoker methodInvoker = new System.Windows.Forms.MethodInvoker(this.StartCalculation);
				methodInvoker.BeginInvoke(null, null);
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());

				throw;
			}
		}

		public void UpdateChartUI()
		{
			this.chartExposure.BackColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.ChartBackgroundColor);
			this.chartExposure.Border.Color = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.ChartBorderColor);
			
			this.chartExposure.Axis.X.Labels.FontColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.ChartTextColor);
			this.chartExposure.Axis.X2.Labels.FontColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.ChartTextColor);
			this.chartExposure.Axis.Y.Labels.FontColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.ChartTextColor);
			this.chartExposure.Axis.Y2.Labels.FontColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.ChartTextColor);
			this.chartExposure.Axis.Z.Labels.FontColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.ChartTextColor);
			this.chartExposure.Axis.Z2.Labels.FontColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.ChartTextColor);
				
			this.chartExposure.Legend.Visible = this._pnlPrefrences.ShowLegend;
			this.chartExposure.Legend.BorderColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.LegendBorderColor);
			this.chartExposure.Legend.BackgroundColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.LegendBackgroundColor);
			this.chartExposure.Legend.FontColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.LegendTextColor);
			
			this.chartExposure.Invalidate(); //Refresh();
		}


		public void UpdateChartPreferences()
		{
			try
			{
				_applyingPreferences = true;

				UpdateChartUI();

				bool blnCreateTable = false;

				//First check which preference to be apply
				switch(this._tabType)
				{
					case (int)PNLTab.Asset:
						if(this._level1ColumnIndex != this._pnlPrefrences.Level1AssetColumn || this._level2ColumnIndex != this._pnlPrefrences.Level2AssetColumn)  
						{
							this._level1ColumnIndex = this._pnlPrefrences.Level1AssetColumn;
							this._level2ColumnIndex = this._pnlPrefrences.Level2AssetColumn;
							blnCreateTable = true;	
						}
							 
						break;

					case (int)PNLTab.Symbol:
						if(this._level1ColumnIndex != this._pnlPrefrences.Level1SymbolColumn || this._level2ColumnIndex != this._pnlPrefrences.Level2SymbolColumn)    
						{
							this._level1ColumnIndex = this._pnlPrefrences.Level1SymbolColumn;
							this._level2ColumnIndex = this._pnlPrefrences.Level2SymbolColumn;
							blnCreateTable = true;						 
						}
					
						break;

					case (int)PNLTab.TradingAccount:
						if(this._level1ColumnIndex != this._pnlPrefrences.Level1TradingAccountColumn || this._level2ColumnIndex != this._pnlPrefrences.Level2TradingAccountColumn)    
						{
							this._level1ColumnIndex = this._pnlPrefrences.Level1TradingAccountColumn;
							this._level2ColumnIndex = this._pnlPrefrences.Level2TradingAccountColumn;
							blnCreateTable = true;
						}
					
						break;
				} 
		
				if(blnCreateTable)
				{
					this.MakeDataTable();
					this.UpdateChartAsync();
				}

				_applyingPreferences = true;
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());

				throw;
			}

		}

		/// <summary>
		/// Starts the process of the calculation
		/// </summary>
		private void StartCalculation()
		{
			try
			{
                //this.grdPNL.Visible = false;
                UpdateOrderCollection();

                if (OrderDetails == null)
                {
                    return;
                }
                //string orderBy = string.Empty;

                //orderBy = PNLHelper.GetOrderByColumnName(this._level1ColumnIndex) +", "+ PNLHelper.GetOrderByColumnName(this._level2ColumnIndex);

                ////call the db method to get the order details by trading account
                //OrderDetails = PNLDataManager.GetInstance().GetPNLOrderDetails(this._userID, orderBy);

				//create Hashtable to hold the distinct symbol list
				if(htSymbolData == null)
					htSymbolData = new Hashtable();

				htSymbol = new Hashtable();
			
				htLevel2Data = new Hashtable();

				ArrayList symbolList = new ArrayList();

				int level2Column = this._level2ColumnIndex;
				string colName = PNLPrefrencesData.PNLColumnNames[level2Column];
				string colType = PNLPrefrencesData.PNLColumnTypes[level2Column];

				DataColumn col = null;
				col = new DataColumn(colName, typeof(string));
				if(!this.dtExposure.Columns.Contains(colName))
					this.dtExposure.Columns.Add(col);
				int colCount = 0;

				_symbolListCount = 0;

				//find the disctinct symbols as well as add column in chart
				for(int i=0;i<OrderDetails.Count;i++)
				{
					Order order = (Order)OrderDetails[i];

					if(!htSymbol.ContainsKey(order.Symbol))
					{
						symbolList.Add(order.Symbol);
						htSymbol.Add(order.Symbol, i.ToString());
						_symbolListCount++;
					}

					string colValue = PNLHelper.GetChartDisplayValue(level2Column, order);

					if(!htLevel2Data.ContainsKey(colValue))
					{
						htLevel2Data.Add(colValue, (colCount++).ToString());

						col = new DataColumn(colValue, typeof(double));
						if(!this.dtExposure.Columns.Contains(colValue))
							this.dtExposure.Columns.Add(col);
					}	
				}		

				col = new DataColumn("Total", typeof(double));
				if(!this.dtExposure.Columns.Contains("Total"))
					this.dtExposure.Columns.Add(col);
			
				//Request for symbol data
				//first check that is eSignal Connected
				if(_liveFeedManager.IsDataManagerConnected())
				{
					_liveFeedManager.RequestSymbolList(symbolList);
				}
				else
				{
					this.blnCalculationStarted = false;
				}
//				else //do not do anything here show the message
//				{
//					//if data manager not connected then use the default calculation column
//					//Clear all the records
//					//	this.dtExposure = new DataTable();
//					//	dtExposure.Clear();
//					//calculate PNL
//					//	this.CalculatePNL(OrderDetails, htSymbolData, htLevel2Data, false);		
//				}

               

			}
			catch(Exception ex)
			{
				this.blnCalculationStarted = false;

				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());

				throw;
			}
		}


        private long _lastOrderSeqNumber = 0;
        private long _maxOrderSeqNumber = 0;

        /// <summary>
        /// Updates the order collection
        /// </summary>
        private void UpdateOrderCollection()
        {
            try
            {
                string orderBy = string.Empty;

                orderBy = PNLHelper.GetOrderByColumnName(this._level1ColumnIndex) + ", " + PNLHelper.GetOrderByColumnName(this._level2ColumnIndex);
                _maxOrderSeqNumber = OrderDataManager.GetMaxSeqNumber();

                ///Modified Rajat, send lastOrderSeqnumber
                ///22 January 2007
                OrderCollection tempOrders = PNLDataManager.GetInstance().GetPNLOrderDetails(this._userID, orderBy, _lastOrderSeqNumber); //pass current login used id                                

                if (tempOrders == null)
                {
                    return;
                }
                if (tempOrders.Count == 0)
                {
                    return;
                }

                if (OrderDetails == null)
                {
                    OrderDetails = new OrderCollection();
                }

                ///Add or update the orders
                foreach (Order tempOrder in tempOrders)
                {
                    string tempClOrderId = tempOrder.ClOrderID;
                    if (_orderDict.ContainsKey(tempClOrderId))
                    {
                        int rowNumber = _orderDict[tempClOrderId];
                        OrderDetails[rowNumber] = tempOrder;
                    }
                    else
                    {
                        _orderDict.Add(tempClOrderId, OrderDetails.Count);
                        OrderDetails.Add(tempOrder);
                    }
                }

                _lastOrderSeqNumber = _maxOrderSeqNumber;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
		
		/// <summary>
		/// To unload the grid
		/// </summary>
		public void UnloadChart()
		{
			this.ultraTabControl1.SelectedTabChanged -=new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(ultraTabControl1_SelectedTabChanged);
			
			NetExposureData = null;
			LongExposureData = null;
			ShortExposureData = null;
			NetPNLData = null;
			LongPNLData = null;
			ShortPNLData = null;
			ExecutedQuantityData = null;

			this.chartExposure.Visible = false;
		}

		private object[] ReDim(object[] old) 
		{ 
			object[] temp =null;
			if (old != null) 
			{
				temp = new object[old.Length+1]; 
			
				System.Array.Copy(old, temp, System.Math.Min(old.Length, temp.Length)); 
			}

			return temp;
		}

		/// <summary>
		/// Updates the Chart Data based on the Tab Selected
		/// </summary>
		/// <param name="tabName"></param>
		private void UpdateChartData(string tabName)
		{
			switch(tabName)
			{
				
				case "Net Exposure":
					
					if(NetExposureData != null)
					{
						for(int i=0; i<NetExposureData.Length; i++) 
						{
							DataRow ro = this.dtExposure.NewRow();
							ro.ItemArray = (object[])NetExposureData[i];
							this.dtExposure.Rows.Add(ro);
						}
					}
					break;

				case "Long Exposure":
					
					if(LongExposureData != null)
					{
						for(int i=0; i<LongExposureData.Length; i++) 
						{
							DataRow ro = this.dtExposure.NewRow();
							ro.ItemArray = (object[])LongExposureData[i];
							this.dtExposure.Rows.Add(ro);
						}
					}

					break;

				case "Short Exposure":

					if(ShortExposureData != null)
					{
						for(int i=0; i<ShortExposureData.Length; i++) 
						{
							DataRow ro = this.dtExposure.NewRow();
							ro.ItemArray = (object[])ShortExposureData[i];
							this.dtExposure.Rows.Add(ro);
						}
					}
					break;

				case "Net PNL":

					if(NetPNLData != null)
					{
						for(int i=0; i<NetPNLData.Length; i++) 
						{
							DataRow ro = this.dtExposure.NewRow();
							ro.ItemArray = (object[])NetPNLData[i];
							this.dtExposure.Rows.Add(ro);
						}
					}

					break;

				case "Long PNL":

					if(LongPNLData != null)
					{
						for(int i=0; i<LongPNLData.Length; i++) 
						{
							DataRow ro = this.dtExposure.NewRow();
							ro.ItemArray = (object[])LongPNLData[i];
							this.dtExposure.Rows.Add(ro);
						}
					}
					break;
				
				case "Short PNL":

					if(ShortPNLData != null)
					{
						for(int i=0; i<ShortPNLData.Length; i++) 
						{
							DataRow ro = this.dtExposure.NewRow();
							ro.ItemArray = (object[])ShortPNLData[i];
							this.dtExposure.Rows.Add(ro);
						}
					}
					break;

				case "Executed Quantity":

					if(ExecutedQuantityData != null)
					{
						for(int i=0; i<ExecutedQuantityData.Length; i++) 
						{
							DataRow ro = this.dtExposure.NewRow();
							ro.ItemArray = (object[])ExecutedQuantityData[i];
							this.dtExposure.Rows.Add(ro);
						}
					}
					break;

			}

            ///Notify the form the data receive has been completed and reenable the form
            if (DataReceived != null)
                DataReceived();
		}

        public event MethodInvoker DataReceived;

		private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
		{
			//MakeDataTable();
			this.dtExposure.Clear();
            
			this.UpdateChartData(e.Tab.Key);

			this.BindChartData(null, null);
		}

		
		private void _liveFeedManager_Level1DataResponse(object sender, EventArgs e)
		{
			try
			{
				//receive the symbol data
				System.Collections.Specialized.NameValueCollection level1DataCollection = (System.Collections.Specialized.NameValueCollection)sender;

				SymbolL1Data symbolL1Data = null;
				string symbol = level1DataCollection.Get("symbol");

                double last = 0;
                double bid = 0;
                double ask = 0;
                double change = 0;
                double previous = 0;

                double.TryParse(level1DataCollection.Get("Last"), System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.CurrentInfo, out last);
                double.TryParse(level1DataCollection.Get("BidPrice"), System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.CurrentInfo, out bid);
                double.TryParse(level1DataCollection.Get("AskPrice"), System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.CurrentInfo, out ask);
                double.TryParse(level1DataCollection.Get("Change"), System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.CurrentInfo, out change);
                double.TryParse(level1DataCollection.Get("Previous"), System.Globalization.NumberStyles.Float, System.Globalization.NumberFormatInfo.CurrentInfo, out previous); 


				if(htSymbol != null && htSymbol.Contains(symbol))
				{

					if(htSymbolData.Contains(symbol))
					{
                        symbolL1Data = (SymbolL1Data)htSymbolData[symbol];

                        symbolL1Data.Last = last;
                        symbolL1Data.Bid = bid;
                        symbolL1Data.Ask = ask;
                        symbolL1Data.Change = change;
                        symbolL1Data.Previous = previous;
                 
						htSymbolData[symbol] = symbolL1Data;                
					}
					else
					{
						symbolL1Data = new SymbolL1Data();
						symbolL1Data.Symbol = symbol;

                        symbolL1Data.Last = last;
                        symbolL1Data.Bid = bid;
                        symbolL1Data.Ask = ask;
                        symbolL1Data.Change = change;
                        symbolL1Data.Previous = previous;
                
						htSymbolData.Add(symbol,symbolL1Data);         
					}

					if(!_lstReceivedSymbolList.Contains(symbol))
					{
						_lstReceivedSymbolList.Add(symbol);
						++_lastRecordEventCount;
					}

					if(_symbolListCount!= int.MinValue && _lastRecordEventCount == _symbolListCount)
					{
						_lstReceivedSymbolList.Clear();
						_lastRecordEventCount = 0;
						_symbolListCount = 0;

						//Clear all the records
						//this.dtExposure = new DataTable();
						dtExposure.Clear();
						//calculate PNL
						this.CalculatePNL(OrderDetails, htSymbolData, htLevel2Data, true);	
					}
				}
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());

				throw;
			}			
		}

	}
}
