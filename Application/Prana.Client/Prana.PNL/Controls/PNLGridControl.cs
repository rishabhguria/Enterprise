using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Nirvana.BusinessObjects;
using Infragistics.Win.UltraWinDataSource;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.Layout;
using System.Diagnostics;
using Nirvana.LiveFeedProvider;
using Nirvana.Interfaces;
using Nirvana.Global;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Nirvana.CommonDataCache;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Nirvana.PNL
{
	/// <summary>
	/// Summary description for PNLGridControl.
	/// </summary>
	public class PNLGridControl : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.IContainer components;
		private DataSet ds = null;
		private CurrencyConversionCollection _currencyConversionList = null;
		private int _selectedCurrencyID = Int32.MinValue;
		private PNLPrefrencesData _pnlPrefrences = null;
		private System.Windows.Forms.Panel pnlGrid;
		private System.Windows.Forms.Label lblErrorMsg;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdPNL;
		private const string FORM_NAME = "PNL: PNLGridControl";
		private int _level1ColumnIndex = int.MinValue;
		private int _level2ColumnIndex = int.MinValue;
		private int _tabType = int.MinValue;
		private OrderCollection OrderSubDetail;
		private Hashtable htSymbolData = null;
		private ILiveFeedManager _liveFeedManager;
		
		private DataTable _orderSide;
		private bool _applyingPreferences = false;
		private int _lastRecordEventCount = 0;
		private ArrayList _lstReceivedSymbolList = new ArrayList();
		private int _symbolListCount = 0;
		private Hashtable htSymbol;
		private double conversionFactorFeedPrice = 1.0; //from USD to selected currency price
		private PNLLevel1DataCollection _pnlLevel1DataCollection;
		private int _userID;
		private bool blnCalculationStarted = false;

        /// <summary>
        /// Added Rajat - 22 Jan 2007 to facilitate the updation of orders and not
        /// fetching the whole bunch every time
        /// </summary>
        private Dictionary<string, int> _orderDict = new Dictionary<string, int>();

		#region properties

		public CurrencyConversionCollection CurrencyConversionList
		{
			set
			{
				this._currencyConversionList = value;
			}
		}

		public int SelectedCurrencyID
		{
			set
			{
                ///Feeds price currencies are not being changed on assigning a different currency
				this._selectedCurrencyID = value;
				//Check if selectedCurrencyID is not dollar
                ///TODO : Right now we have hard coded the conversion to and from USD, Remove hard coding
                if (this._selectedCurrencyID != Global.Common.USDollar)
                {
                    //find the conversion id from the list
                    conversionFactorFeedPrice = PNLHelper.findConversionFactor(this._currencyConversionList, Global.Common.USDollar, this._selectedCurrencyID);
                }
                else
                {
                    ///Added Rajat : 21-08-2006
                    conversionFactorFeedPrice = PNLHelper.findConversionFactor(this._currencyConversionList,Global.Common.USDollar, this._selectedCurrencyID);
                }
                //ChangeCurrency();
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

		public int TabType
		{
			set
			{
				this._tabType = value;
			}
		}

		public OrderCollection OrderDetails
		{
			set
			{
				this.OrderSubDetail = value;
			}
		}


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

		#endregion

		#region constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public PNLGridControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public PNLGridControl(PNLPrefrencesData pnlPrefrences, CurrencyConversionCollection currencyConversionCollection, ILiveFeedManager liveFeedManager, int userID)
		{
			this._pnlPrefrences = pnlPrefrences;
			this._currencyConversionList = currencyConversionCollection;
			this._liveFeedManager = liveFeedManager;
			//this._tagDatabase = TagDatabase.GetInstance();
            this._orderSide = Nirvana.CommonDataCache.TagDatabaseManager.GetInstance.GetAllOrderSides();
			this._liveFeedManager.Level1DataResponse+=new EventHandler(_liveFeedManager_Level1DataResponse);
			this._userID = userID;
			
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this._pnlLevel1DataCollection = new PNLLevel1DataCollection();
			
			this.grdPNL.DataSource = _pnlLevel1DataCollection;

			this.ClearOrderCollection();

			//Update UI
			this.UpdateGridUI();

		}

		#endregion

		#region Dispose method
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
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			this.pnlGrid = new System.Windows.Forms.Panel();
			this.grdPNL = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.pnlGrid.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdPNL)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlGrid
			// 
			this.pnlGrid.Controls.Add(this.grdPNL);
			this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlGrid.Location = new System.Drawing.Point(0, 0);
			this.pnlGrid.Name = "pnlGrid";
			this.pnlGrid.Size = new System.Drawing.Size(696, 264);
			this.pnlGrid.TabIndex = 4;
			// 
			// grdPNL
			// 
			appearance1.Cursor = System.Windows.Forms.Cursors.Arrow;
			appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
			appearance1.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.grdPNL.DisplayLayout.Appearance = appearance1;
			this.grdPNL.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
			appearance2.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(24)), ((System.Byte)(73)), ((System.Byte)(171)));
			appearance2.FontData.BoldAsString = "True";
			appearance2.FontData.Name = "Tahoma";
			appearance2.FontData.SizeInPoints = 8.5F;
			appearance2.ForeColor = System.Drawing.Color.White;
			ultraGridBand1.Override.SummaryValueAppearance = appearance2;
			this.grdPNL.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.grdPNL.DisplayLayout.GroupByBox.Hidden = true;
			this.grdPNL.DisplayLayout.MaxColScrollRegions = 1;
			this.grdPNL.DisplayLayout.MaxRowScrollRegions = 1;
			this.grdPNL.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
			this.grdPNL.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
			this.grdPNL.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
			this.grdPNL.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			this.grdPNL.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
			this.grdPNL.DisplayLayout.Override.AllowGroupMoving = Infragistics.Win.UltraWinGrid.AllowGroupMoving.NotAllowed;
			this.grdPNL.DisplayLayout.Override.AllowGroupSwapping = Infragistics.Win.UltraWinGrid.AllowGroupSwapping.NotAllowed;
			this.grdPNL.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
			this.grdPNL.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
			appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
			appearance3.TextVAlign = Infragistics.Win.VAlign.Middle;
			this.grdPNL.DisplayLayout.Override.CellAppearance = appearance3;
			this.grdPNL.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
			appearance4.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			this.grdPNL.DisplayLayout.Override.FixedHeaderAppearance = appearance4;
			appearance5.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(0)));
			appearance5.FontData.BoldAsString = "True";
			appearance5.FontData.Name = "Tahoma";
			appearance5.FontData.SizeInPoints = 8.25F;
			this.grdPNL.DisplayLayout.Override.HeaderAppearance = appearance5;
			this.grdPNL.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
			appearance6.FontData.BoldAsString = "True";
			appearance6.FontData.Name = "Tahoma";
			appearance6.FontData.SizeInPoints = 8.25F;
			this.grdPNL.DisplayLayout.Override.RowAppearance = appearance6;
			this.grdPNL.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
			this.grdPNL.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
			this.grdPNL.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
			this.grdPNL.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
			this.grdPNL.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
			this.grdPNL.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.BottomFixed;
			appearance7.FontData.BoldAsString = "True";
			appearance7.FontData.Name = "Tahoma";
			appearance7.FontData.SizeInPoints = 8.25F;
			appearance7.TextHAlign = Infragistics.Win.HAlign.Left;
			this.grdPNL.DisplayLayout.Override.SummaryFooterCaptionAppearance = appearance7;
			this.grdPNL.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.grdPNL.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.grdPNL.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.grdPNL.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdPNL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grdPNL.Location = new System.Drawing.Point(0, 0);
			this.grdPNL.Name = "grdPNL";
			this.grdPNL.Size = new System.Drawing.Size(696, 264);
			this.grdPNL.TabIndex = 3;
			this.grdPNL.Visible = false;
			this.grdPNL.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdPNL_InitializeRow);
			// 
			// PNLGridControl
			// 
			this.Controls.Add(this.pnlGrid);
			this.Name = "PNLGridControl";
			this.Size = new System.Drawing.Size(696, 264);
			this.Load += new System.EventHandler(this.PNLGridControl_Load);
			this.pnlGrid.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdPNL)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Initialize the Control with calculated data
		/// </summary>
		public void InitPNL()
		{
			try
			{
				
				this.grdPNL.Visible = false;

				//Clear the Order Collection
				this.ClearOrderCollection();
				
				//Update Grid Asynchronously
				this.UpdateGridAsync();

				//System.Threading.Thread.Sleep(400);
				//Update Grid Layout
				this.UpdateGridColumnLayout();
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

		private void UpdateGridAsync()
		{
			try
			{
				lock(this.grdPNL)
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

		/// <summary>
		/// To unload the grid
		/// </summary>
		public void UnloadPNL()
		{
          this.ClearOrderCollection();
		}
	
		/// <summary>
		/// Update the Parent and Child Band rows
		/// </summary>
		private void UpdateGridUI()
		{
			try
			{
				this.grdPNL.SuspendLayout();

				UltraGridLayout displayLayout = this.grdPNL.DisplayLayout;
			
				//changing the color for the parent band
				if(displayLayout != null)
				{
					Infragistics.Win.UltraWinGrid.BandsCollection bandCollection = displayLayout.Bands; // layoutCollection
				
					displayLayout.Appearance.BackColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.BackgroundColor);
					displayLayout.Appearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.DefaulTextColor);
					displayLayout.Override.RowAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.DefaulTextColor);
					displayLayout.Override.RowAlternateAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.DefaulTextColor);

					displayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.BottomFixed;
					displayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.True;
					//displayLayout.Override.SpecialRowSeparator = SpecialRowSeparator.SummaryRow;
					//displayLayout.Override.BorderStyleSummaryFooter = Infragistics.Win.UIElementBorderStyle.None;
					//displayLayout.Override.BorderStyleSummaryValue = Infragistics.Win.UIElementBorderStyle.None;
					//displayLayout.Override.BorderStyleSpecialRowSeparator = Infragistics.Win.UIElementBorderStyle.None;
					displayLayout.Override.SummaryFooterSpacingBefore = -1;
					displayLayout.Override.SummaryFooterSpacingAfter = -1;
					//displayLayout.Override.SpecialRowSeparator = SpecialRowSeparator.None;
					
					//Infragistics.Win.UltraWinCalcManager.UltraCalcManager calcManager; 
					//calcManager = new Infragistics.Win.UltraWinCalcManager.UltraCalcManager( this.Container ); 
					//displayLayout.Grid.CalcManager = calcManager;
					
					#region Set level1 properties
					if(bandCollection.Count > 0)
					{
						//Set the L1 row colors
						bandCollection[0].Override.RowAppearance.BackColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.RowColorL1);
						bandCollection[0].Override.RowAlternateAppearance.BackColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.RowalternateColorL1);
						bandCollection[0].Override.HeaderAppearance.BackColor= PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.HeaderColorL1);
						//set the default text color
						bandCollection[0].Override.RowAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.DefaulTextColor);

						bandCollection[0].Override.SummaryFooterAppearance.BackColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.SummaryBackgroundColor);
						bandCollection[0].Override.SummaryFooterAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.SummaryTextColor);
						bandCollection[0].Override.SummaryFooterCaptionAppearance.BackColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.SummaryBackgroundColor);
						bandCollection[0].Override.SummaryFooterCaptionAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.SummaryTextColor);
						bandCollection[0].Override.SummaryValueAppearance.BackColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.SummaryBackgroundColor);
						bandCollection[0].Override.SummaryValueAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.SummaryTextColor);

						 
						bandCollection[0].SummaryFooterCaption = "Totals";

						//set the row spacing
						bandCollection[0].Override.RowSpacingAfter = 0;

						//set the text color for the specific column
						SummarySettings summary;
						//Long Exposure
						string columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.LongExposure];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							bandCollection[0].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.LongExposureTextColor);
							bandCollection[0].Columns[columnName].Format = "###,###,###,###,###,###";
					
							if(!bandCollection[0].Summaries.Exists("LongExposure"))
							{
								summary = bandCollection[0].Summaries.Add( "LongExposure", SummaryType.Sum , bandCollection[0].Columns[columnName] );
								summary.DisplayFormat = "{0:###,###,###,###}";
								summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
							}
						}

						//Short Exposure
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.ShortExposure];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							bandCollection[0].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.ShortExposureTextColor);
							bandCollection[0].Columns[columnName].Format = "###,###,###,###,###,###";
						
							if(!bandCollection[0].Summaries.Exists("ShortExposure"))
							{
								summary = bandCollection[0].Summaries.Add( "ShortExposure", SummaryType.Sum , bandCollection[0].Columns[columnName] );
								summary.DisplayFormat = "{0:###,###,###,###,###,###}";
								summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
							}

						}

						//Net Exposure
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.NetExposure];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							//bandCollection[0].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.NetExposureTextColor);
							bandCollection[0].Columns[columnName].Format = "###,###,###,###,###,###";

							if(!bandCollection[0].Summaries.Exists("NetExposure"))
							{
								summary = bandCollection[0].Summaries.Add( "NetExposure", SummaryType.Sum , bandCollection[0].Columns[columnName] );
								summary.DisplayFormat = "{0:###,###,###,###,###,###}";
								summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
							}

						}

						//Long PNL
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.LongPNL];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							//bandCollection[0].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.LongPNLTextColor);
							bandCollection[0].Columns[columnName].Format = "###,###,###,###,###,###";

							if(!bandCollection[0].Summaries.Exists("LongPNL"))
							{
								summary = bandCollection[0].Summaries.Add( "LongPNL", SummaryType.Sum , bandCollection[0].Columns[columnName] );
								//summary = bandCollection[0].Summaries.Add( "LongPNL","sum( ["+columnName+"] )" );
								summary.DisplayFormat = "{0:###,###,###,###,###,###}";
								summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
								//displayLayout.Override.FormulaRowIndexSource = FormulaRowIndexSource.ListIndex;
							}

						}

						//Short PNL
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.ShortPNL];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							//bandCollection[0].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.ShortPNLTextColor);
							bandCollection[0].Columns[columnName].Format = "###,###,###,###,###,###";

							if(!bandCollection[0].Summaries.Exists("ShortPNL"))
							{
								summary = bandCollection[0].Summaries.Add( "ShortPNL", SummaryType.Sum , bandCollection[0].Columns[columnName] );
								summary.DisplayFormat = "{0:###,###,###,###,###,###}";
								summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
							}
						}
					
						//Net PNL
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.NetPNL];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							//bandCollection[0].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.NetPNLTextColor);
							bandCollection[0].Columns[columnName].Format = "###,###,###,###,###,###";

							if(!bandCollection[0].Summaries.Exists("NetPNL"))
							{
								summary = bandCollection[0].Summaries.Add( "NetPNL", SummaryType.Sum , bandCollection[0].Columns[columnName] );
								summary.DisplayFormat = "{0:###,###,###,###,###,###}";
								summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
							}
						}

						//Average price
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.AveragePrice];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							bandCollection[0].Columns[columnName].Format = "#0.00";
						}

						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.Ask];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							bandCollection[0].Columns[columnName].Format = "#0.00";
						}

						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.Bid];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							bandCollection[0].Columns[columnName].Format = "#0.00";
						}

						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.Last];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							bandCollection[0].Columns[columnName].Format = "#0.00";
						}

						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.PercentChange];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							bandCollection[0].Columns[columnName].Format = "#0.00";
						}

						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.ExecutedQuantity];
						if(bandCollection[0].Columns.Exists(columnName))
						{
							bandCollection[0].Columns[columnName].Format = "###,###,###,###,###,###";
						}

					}
					#endregion

					#region Set level2 properties
					if(bandCollection.Count > 1)
					{
						bandCollection[1].Override.RowSpacingBefore = 0;

						bandCollection[1].Override.RowAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.DefaulTextColor);
						bandCollection[1].Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.None;

						//changing the colors for the child band
						bandCollection[1].Override.HeaderAppearance.BackColor= PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.HeaderColorL2);
						bandCollection[1].Override.RowAppearance.BackColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.RowColorL2);
						bandCollection[1].Override.RowAlternateAppearance.BackColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.RowalternateColorL2);

						bandCollection[1].Override.CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.DefaulTextColor);
						bandCollection[1].Override.HeaderAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.DefaulTextColor);


						//set the text color for the specific column

						//Long Exposure
						string columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.LongExposure];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							bandCollection[1].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.LongExposureTextColor);
							bandCollection[1].Columns[columnName].Format = "###,###,###,###,###,###";
						}

						//Short Exposure
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.ShortExposure];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							bandCollection[1].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.ShortExposureTextColor);
							bandCollection[1].Columns[columnName].Format = "###,###,###,###,###,###";
						}

						//Net Exposure
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.NetExposure];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							//bandCollection[1].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.NetExposureTextColor);
							bandCollection[1].Columns[columnName].Format = "###,###,###,###,###,###";
						}

						//Long PNL
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.LongPNL];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							//bandCollection[1].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.LongPNLTextColor);
							bandCollection[1].Columns[columnName].Format = "###,###,###,###,###,###";
						}

						//Short PNL
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.ShortPNL];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							//bandCollection[1].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.ShortPNLTextColor);
							bandCollection[1].Columns[columnName].Format = "###,###,###,###,###,###";
						}

						//Net PNL
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.NetPNL];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							//bandCollection[1].Columns[columnName].CellAppearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(_pnlPrefrences.NetPNLTextColor);
							bandCollection[1].Columns[columnName].Format = "###,###,###,###,###,###";
						}

						//Average price
						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.AveragePrice];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							bandCollection[1].Columns[columnName].Format = "#0.00";
						}

						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.Ask];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							bandCollection[1].Columns[columnName].Format = "#0.00";
						}

						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.Bid];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							bandCollection[1].Columns[columnName].Format = "#0.00";
						}

						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.Last];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							bandCollection[1].Columns[columnName].Format = "#0.00";
						}

						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.PercentChange];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							bandCollection[1].Columns[columnName].Format = "#0.00";
						}

						columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.ExecutedQuantity];
						if(bandCollection[1].Columns.Exists(columnName))
						{
							bandCollection[1].Columns[columnName].Format = "###,###,###,###,###,###";
						}

					}
					#endregion

					//do not display row connectors
					displayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.None;
							
					//do not display row selectors
					displayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;		
						
					//take away the "grid" lines
					displayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
				
					//displayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.None;
					displayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
					
					//turn off themes
					//this property needs to be turned off in order to color the headers
					this.grdPNL.SupportThemes = false;	

				}

				this.grdPNL.ResumeLayout();

				this.grdPNL.Refresh();
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
		/// To change the grid column settings
		/// </summary>
		private void UpdateGridColumnLayout()
		{
			try
			{
				this.grdPNL.SuspendLayout();

				UltraGridLayout displayLayout = this.grdPNL.DisplayLayout;
			
				//changing the color for the parent band
				if(displayLayout != null)
				{
					Infragistics.Win.UltraWinGrid.BandsCollection bandCollection = displayLayout.Bands; // layoutCollection
				
					//To sort the grid based on the preferences
					string sortColumnNameL1 = string.Empty;
					string sortColumnNameL2 = string.Empty;
					bool sortOrderL1 = false;
					bool sortOrderL2 = false;
					ArrayList displayList1 = null;
					ArrayList displayList2 = null;
					string level1ColumnName = string.Empty;
					string level2ColumnName = string.Empty;

					switch(this._tabType)
					{
						case (int)PNLTab.Asset:
							sortColumnNameL1 = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.SortKeyAssetL1];
							sortColumnNameL2 = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.SortKeyAssetL2];
							level1ColumnName = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.Level1AssetColumn];
							level2ColumnName = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.Level2AssetColumn];
							sortOrderL1 = !_pnlPrefrences.AscendingAssetL1;
							sortOrderL2 = !_pnlPrefrences.AscendingAssetL2;
							displayList1 = _pnlPrefrences.DisplayListAssetL1;
							displayList2 = _pnlPrefrences.DisplayListAssetL2;
							break;

						case (int)PNLTab.Symbol:
							sortColumnNameL1 = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.SortKeySymbolL1];
							sortColumnNameL2 = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.SortKeySymbolL2];
							level1ColumnName = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.Level1SymbolColumn];
							level2ColumnName = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.Level2SymbolColumn];
							sortOrderL1 = !_pnlPrefrences.AscendingSymbolL1;
							sortOrderL2 = !_pnlPrefrences.AscendingSymbolL2;
							displayList1 = _pnlPrefrences.DisplayListSymbolL1;
							displayList2 = _pnlPrefrences.DisplayListSymbolL2;
							break;

						case (int)PNLTab.TradingAccount:
							sortColumnNameL1 = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.SortKeyTradingAccountL1];
							sortColumnNameL2 = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.SortKeyTradingAccountL2];
							level1ColumnName = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.Level1TradingAccountColumn];
							level2ColumnName = PNLPrefrencesData.PNLColumnNames[(int)_pnlPrefrences.Level2TradingAccountColumn];
							sortOrderL1 = !_pnlPrefrences.AscendingTradingAccountL1;
							sortOrderL2 = !_pnlPrefrences.AscendingTradingAccountL2;
							displayList1 = _pnlPrefrences.DisplayListTradingAccountL1;
							displayList2 = _pnlPrefrences.DisplayListTradingAccountL2;
							break;
					}
				
					#region Set level1 properties

					if(bandCollection.Count > 0)
					{
						int columnCount = bandCollection[0].Columns.Count;

						for(int i=0;i<columnCount;i++)
						{
							UltraGridColumn gridColumn = bandCollection[0].Columns[i];
							gridColumn.Hidden = true;
						}

						bandCollection[0].SortedColumns.Clear();
						bandCollection[0].SortedColumns.Add(sortColumnNameL1, sortOrderL1);

						UltraGridColumn level1Column = bandCollection[0].Columns[level1ColumnName];
						level1Column.Hidden = false;
						level1Column.Header.VisiblePosition =0;

						//Change the order of the column and show them
						for(int i=0;i<displayList1.Count;i++)
						{
							UltraGridColumn gridColumn = bandCollection[0].Columns[PNLPrefrencesData.PNLColumnNames[(int)((PNLColumns)displayList1[i])]];
							gridColumn.Header.VisiblePosition = i+1;
							gridColumn.Hidden = false;
						}
					}

					#endregion

					#region Set level2 properties

					if(bandCollection.Count > 1)
					{
						int columnCount = bandCollection[1].Columns.Count;

						for(int i=0;i<columnCount;i++)
						{
							UltraGridColumn gridColumn = bandCollection[1].Columns[i];
							gridColumn.Hidden = true;
						}

						bandCollection[1].SortedColumns.Clear();
						bandCollection[1].SortedColumns.Add(sortColumnNameL2, sortOrderL1);

						UltraGridColumn level2Column = bandCollection[1].Columns[level2ColumnName];
						level2Column.Hidden = false;
						level2Column.Header.VisiblePosition =0;

						//Change the order of the column and show them
						for(int i=0;i<displayList2.Count;i++)
						{
							UltraGridColumn gridColumn = bandCollection[1].Columns[PNLPrefrencesData.PNLColumnNames[(int)((PNLColumns)displayList2[i])]];
							gridColumn.Header.VisiblePosition = i+1;
							gridColumn.Hidden = false;
						}
					}
					#endregion
					
				}
				
				this.grdPNL.ResumeLayout();

				this.grdPNL.Refresh();
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
		/// To update the PNL calculation with latest data.
		/// </summary>
		public void RefreshPNL(object sender, EventArgs e)
		{
			if(_applyingPreferences == false)
			{
				UpdateGridAsync();
			}
		}

        

		/// <summary>
		/// This method is called Asynchroniously to calculate the PNL
		/// </summary>
		private void StartCalculation()
		{
			try
			{
				//this.grdPNL.Visible = false;
                UpdateOrderCollection();

                if (OrderSubDetail == null)
                {
                    return;
                }
				//call the db method to get the order details by asset class
                //OrderSubDetail = PNLDataManager.GetInstance().GetPNLOrderDetails(this._userID, orderBy); //pass current login used id
				
				//Request for symbol data
				//first check that is eSignal Connected
				if(_liveFeedManager.IsDataManagerConnected())
				{
					//create Hashtable to hold the distinct symbol list
					//if(htSymbolData == null)
				
					htSymbolData = new Hashtable();
                
					ArrayList symbolList = new ArrayList();
					htSymbol = new Hashtable();

					_symbolListCount = 0;
					//find the disctinct symbols
					for(int i=0;i<OrderSubDetail.Count;i++)
					{
						Order order = (Order)OrderSubDetail[i];
						string symbolName = order.Symbol.Trim().ToString();
						if(!htSymbol.ContainsKey(symbolName))
						{
							symbolList.Add(symbolName);
							htSymbol.Add(symbolName, "");
							_symbolListCount++;
						}
					}

					_liveFeedManager.RequestSymbolList(symbolList);

                    //if(this.lblErrorMsg != null)
                    //{
                    //    this.lblErrorMsg.Visible = false;
                    //}
				}
				else //Do not anything and show error message 
				{
					//if data manager not connected then show message
					if(this.InvokeRequired == false)
					{
						ShowErrorMessage(null, null);
					}
					else
					{
						Invoke(new EventHandler(ShowErrorMessage), new object[] { null, null});
					}

					this.blnCalculationStarted = false;

				}
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

        //_newOrderSeqNumber = Nirvana.ServerClientCommon.OrderDataManager.GetMaxSeqNumber();
        //       AllocationOrderCollection  updatedFundOrders = OrderAllocationDBManager.GetUpdatedOrders(_userID, currentTime, _lastOrderSeqNumber);
        //       _lastOrderSeqNumber = _newOrderSeqNumber;
        //       CheckUpdatedFundOrdersLocation(updatedFundOrders);

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
                _maxOrderSeqNumber =OrderDataManager.GetMaxSeqNumber();

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
                if (OrderSubDetail == null)
                {
                    OrderSubDetail = new OrderCollection();
                }
                

                ///Add or update the orders
                foreach (Order tempOrder in tempOrders)
                {
                    string tempClOrderId = tempOrder.ClOrderID;
                    if (_orderDict.ContainsKey(tempClOrderId))
                    {
                        int rowNumber = _orderDict[tempClOrderId];
                        OrderSubDetail[rowNumber] = tempOrder;
                    }
                    else
                    {
                        _orderDict.Add(tempClOrderId, OrderSubDetail.Count);
                        OrderSubDetail.Add(tempOrder);
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

		private void ShowErrorMessage(object sender, EventArgs e)
		{
			if(this.lblErrorMsg == null)
			{
				this.lblErrorMsg = new Label();
				this.lblErrorMsg.Text = ""; //PNL Data is not available because live feed is down";
				this.lblErrorMsg.Size = new System.Drawing.Size(330, 23);
				this.lblErrorMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
					| System.Windows.Forms.AnchorStyles.Right)));
				this.lblErrorMsg.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
				this.lblErrorMsg.Location = new System.Drawing.Point(224, 72);
				this.lblErrorMsg.Name = "lblErrorMsg";
				this.lblErrorMsg.TabIndex = 0;
				this.pnlGrid.Controls.Add(this.lblErrorMsg);						
			}
			else
			{
				this.lblErrorMsg.Visible = true;				
			}
		}

		/// <summary>
		/// Calculate the profit and loss for the PNL data
		/// </summary>
		/// <param name="htSymbolData"></param>
		/// <param name="OrderSubDetail"></param>
		private void CalculatePNL(Hashtable htSymbolData, OrderCollection OrderSubDetail, bool IsDataManagerConnected)
		{		
			try	
			{	
				this.grdPNL.SuspendLayout();
		
				#region declare the variable to calculate the qty, exposure and pnl
							
				//its value is 1 in case of stocks
				int multiplier = 1; //Multiplier to be pickup from the database
							
				//its value is 1 in case of stocks
				int delta = 1;
				//To show the sum at Level 1
				int totalLongExposure = 0;
				int totalShortExposure = 0;
				int totalShortPNL = 0;
				int totalLongPNL = 0;
				int totalQuantity = 0;

				//To show the sum at Level 2
				int longExposure = 0;
				int shortExposure = 0;
				int shortPNL = 0;
				int longPNL = 0;
				int executedQuantity = 0;
                double totalLevel2_NotionalValue = 0;

				//to hold the informaton of level2 column
				int level2ColumnIndex = this._level2ColumnIndex;
				string level2ColumnName = PNLPrefrencesData.PNLColumnNames[level2ColumnIndex];
				string level2ColumnValue = string.Empty;
				//string level2ColumnDisplayValue = string.Empty;

				//to hold the informaton of level1 column
				int level1ColumnIndex = this._level1ColumnIndex;
				string level1ColumnName = PNLPrefrencesData.PNLColumnNames[level1ColumnIndex];
				string level1ColumnValue = string.Empty;
				//string level1ColumnDisplayValue = string.Empty;

				//Clear the collection
				//this.ClearOrderCollection();

				PNLLevel2DataCollection pnlLevel2DataCollection = new PNLLevel2DataCollection();
				PNLLevel1Data pnlLevel1Data = new PNLLevel1Data();
				
				#endregion

				//Apply here the logic to calculate the PNL
				for(int i=0;i<OrderSubDetail.Count;i++)
				{
					Order order = (Order)OrderSubDetail[i];
					//int auecID = order.AUECID;
				
					if(level1ColumnValue == string.Empty)
					{
						level1ColumnValue = PNLHelper.GetColumnValue(level1ColumnIndex, order);

						//Add new row at level1
						pnlLevel1Data = SearchLevel1Item(this._pnlLevel1DataCollection, level1ColumnIndex, PNLHelper.GetDisplayColumnValue(level1ColumnIndex, order));
									
						if(pnlLevel1Data == null)
						{
							pnlLevel1Data = new PNLLevel1Data();
							this._pnlLevel1DataCollection.Add(pnlLevel1Data);
						}
					}
					if(!level1ColumnValue.Equals(PNLHelper.GetColumnValue(level1ColumnIndex, order)))
					{
						//Update the Level1 summary
						#region update calculation
						totalLongExposure +=longExposure;
						totalShortExposure += shortExposure;
						totalShortPNL += shortPNL;
						totalLongPNL += longPNL;
						totalQuantity += executedQuantity;
						#endregion
	
						try	
						{
							Order tempOrder = ((Order)OrderSubDetail[i-1]);

							//Add new row at level2
							PNLLevel2Data pnlLevel2Data = null;
							pnlLevel2Data = SearchLevel2Item(pnlLevel1Data, level2ColumnIndex, PNLHelper.GetDisplayColumnValue(level2ColumnIndex, tempOrder));
								
							if(pnlLevel2Data == null)
							{
								pnlLevel2Data = new PNLLevel2Data();
								pnlLevel1Data.Level2DataCollection.Add(pnlLevel2Data);
							}

							SymbolL1Data l1Data = (SymbolL1Data)htSymbolData[tempOrder.Symbol];

							#region set the column data for level2 row					
							pnlLevel2Data.Exchange = tempOrder.ExchangeName;
                            //pnlLevel2Data.AveragePrice =tempOrder.AvgPrice;
							pnlLevel2Data.TradingAccount = tempOrder.TradingAccountName;
							pnlLevel2Data.Side = tempOrder.OrderSide;
							pnlLevel2Data.Symbol = tempOrder.Symbol;
							pnlLevel2Data.Currency = tempOrder.CurrencyName;
							pnlLevel2Data.LongExposure = longExposure;
							pnlLevel2Data.ShortExposure = shortExposure;
							pnlLevel2Data.NetExposure = longExposure - shortExposure;
							pnlLevel2Data.LongPNL = longPNL;
							pnlLevel2Data.ShortPNL = shortPNL;
							pnlLevel2Data.NetPNL = (longPNL + shortPNL);
							
							if(l1Data != null)
							{
								pnlLevel2Data.Last = l1Data.Last; 
								pnlLevel2Data.Bid = l1Data.Bid; 
								pnlLevel2Data.Ask = l1Data.Ask;
								pnlLevel2Data.PercentChange = l1Data.Change; 
							}
							else
							{
								pnlLevel2Data.Last = 0; 
								pnlLevel2Data.Bid = 0; 
								pnlLevel2Data.Ask = 0;
								pnlLevel2Data.PercentChange = 0; 
							}

							pnlLevel2Data.ExecutedQuantity = executedQuantity;
                            pnlLevel2Data.AveragePrice = Convert.ToDouble(totalLevel2_NotionalValue / executedQuantity);
							pnlLevel2Data.UnderLying = tempOrder.UnderlyingName;
							pnlLevel2Data.Asset = tempOrder.AssetName;
							#endregion

							//Add new row at Level1
							#region set the column data for level1 row
							pnlLevel1Data.Exchange = tempOrder.ExchangeName;
							pnlLevel1Data.AveragePrice = tempOrder.AvgPrice;
							pnlLevel1Data.TradingAccount = tempOrder.TradingAccountName;
							pnlLevel1Data.Side = tempOrder.OrderSide;
							pnlLevel1Data.Symbol = tempOrder.Symbol;
							pnlLevel1Data.Currency = tempOrder.CurrencyName;
							pnlLevel1Data.LongExposure = totalLongExposure;
							pnlLevel1Data.ShortExposure = totalShortExposure;
							pnlLevel1Data.NetExposure = totalLongExposure - totalShortExposure;
							pnlLevel1Data.LongPNL = totalLongPNL;
							pnlLevel1Data.ShortPNL = totalShortPNL;
							pnlLevel1Data.NetPNL = (totalLongPNL + totalShortPNL);

							if(l1Data != null)
							{
								pnlLevel1Data.Last = l1Data.Last; 
								pnlLevel1Data.Bid = l1Data.Bid; 
								pnlLevel1Data.Ask = l1Data.Ask;
								pnlLevel1Data.PercentChange = l1Data.Change; 
							}
							else
							{
								pnlLevel1Data.Last = 0; 
								pnlLevel1Data.Bid = 0; 
								pnlLevel1Data.Ask = 0;
								pnlLevel1Data.PercentChange = 0; 
							}

							pnlLevel1Data.ExecutedQuantity = totalQuantity;
							pnlLevel1Data.UnderLying = tempOrder.UnderlyingName;
							pnlLevel1Data.Asset = tempOrder.AssetName;
							#endregion
							
							//Update the Exchange Information
							level1ColumnValue = PNLHelper.GetColumnValue(level1ColumnIndex, order);

							//Add new row at level1
							pnlLevel1Data = SearchLevel1Item(this._pnlLevel1DataCollection, level1ColumnIndex, PNLHelper.GetDisplayColumnValue(level1ColumnIndex, order));
									
							if(pnlLevel1Data == null)
							{
								pnlLevel1Data = new PNLLevel1Data();
								this._pnlLevel1DataCollection.Add(pnlLevel1Data);
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
							
						//level1ColumnDisplayValue = PNLHelper.GetColumnDisplayValue(level1ColumnIndex, order);
							
						//Reset the L1 summary variables
						#region reset variables
                        totalLongExposure = 0;
                        totalShortExposure = 0;
                        totalShortPNL = 0;
                        totalLongPNL = 0;
                        totalQuantity = 0;

                        //Reset the L2 summary variables
                        longExposure = 0;
                        shortExposure = 0;
                        shortPNL = 0;
                        longPNL = 0;
                        executedQuantity = 0;
                        totalLevel2_NotionalValue = 0;
						level2ColumnValue = string.Empty;
						#endregion
					}

					if(level1ColumnValue.Equals(PNLHelper.GetColumnValue(level1ColumnIndex, order)))
					{
						if(level2ColumnValue == string.Empty)
						{
							level2ColumnValue = PNLHelper.GetColumnValue(level2ColumnIndex, order);
						}

						if(!level2ColumnValue.Equals(PNLHelper.GetColumnValue(level2ColumnIndex, order)))
						{
							//Update the Level1 summary
							#region update calculation
							totalLongExposure +=longExposure;
							totalShortExposure += shortExposure;
							totalShortPNL += shortPNL;
							totalLongPNL += longPNL;
							totalQuantity += executedQuantity;
							#endregion
									
							try
							{
								Order tempOrder = ((Order)OrderSubDetail[i-1]);
								
								//Add new row at level2
								PNLLevel2Data pnlLevel2Data = null;
								pnlLevel2Data = SearchLevel2Item(pnlLevel1Data, level2ColumnIndex, PNLHelper.GetDisplayColumnValue(level2ColumnIndex, tempOrder));
								
								if(pnlLevel2Data == null)
								{
									pnlLevel2Data = new PNLLevel2Data();
									pnlLevel1Data.Level2DataCollection.Add(pnlLevel2Data);
								}

								SymbolL1Data l1Data = (SymbolL1Data)htSymbolData[tempOrder.Symbol];

								#region set the column data for level2 row					
								pnlLevel2Data.Exchange = tempOrder.ExchangeName;
                                //pnlLevel2Data.AveragePrice = tempOrder.AvgPrice;
                                
								pnlLevel2Data.TradingAccount = tempOrder.TradingAccountName;
								pnlLevel2Data.Side = tempOrder.OrderSide;
								pnlLevel2Data.Symbol = tempOrder.Symbol;
								pnlLevel2Data.Currency = tempOrder.CurrencyName;
								pnlLevel2Data.LongExposure = longExposure;
								pnlLevel2Data.ShortExposure = shortExposure;
								pnlLevel2Data.NetExposure = longExposure - shortExposure;
								pnlLevel2Data.LongPNL = longPNL;
								pnlLevel2Data.ShortPNL = shortPNL;
								pnlLevel2Data.NetPNL = (longPNL + shortPNL);
								
								if(l1Data != null)
								{
									pnlLevel2Data.Last = l1Data.Last; 
									pnlLevel2Data.Bid = l1Data.Bid; 
									pnlLevel2Data.Ask = l1Data.Ask;
									pnlLevel2Data.PercentChange = l1Data.Change; 
								}
								else
								{
									pnlLevel2Data.Last = 0; 
									pnlLevel2Data.Bid = 0; 
									pnlLevel2Data.Ask = 0;
									pnlLevel2Data.PercentChange = 0; 
								}

								pnlLevel2Data.ExecutedQuantity = executedQuantity;
                                pnlLevel2Data.AveragePrice = totalLevel2_NotionalValue / executedQuantity;
								pnlLevel2Data.UnderLying = tempOrder.UnderlyingName;
								pnlLevel2Data.Asset = tempOrder.AssetName;
								#endregion

								level2ColumnValue = PNLHelper.GetColumnValue(level2ColumnIndex, order);
								
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
							//Reset the L2 summary variables
							#region reset variable
							longExposure = 0;
							shortExposure = 0;
							shortPNL = 0;
							longPNL = 0;
							executedQuantity = 0;
                            totalLevel2_NotionalValue = 0;
							#endregion

						}

						if(level2ColumnValue.Equals(PNLHelper.GetColumnValue(level2ColumnIndex, order)))
						{
							string side = order.OrderSide;

							#region Calculation Logic
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

							try
							{
                                
								double conversionFactor = 1.0;
															
								//Check for the Currency Type
								if(order.CurrencyID != this._selectedCurrencyID)
								{
									//find the conversion id from the list
									conversionFactor = PNLHelper.findConversionFactor(this._currencyConversionList, order.CurrencyID,this._selectedCurrencyID);
								}

								/*
								 * Remove all harding from here used for testing.
								 *
								*/
									
								SymbolL1Data l1Data = (SymbolL1Data)htSymbolData[order.Symbol];
										
								double feedPriceLE = double.MinValue;
								double feedPriceSE = double.MinValue;
								double feedPriceLP = double.MinValue;
								double feedPriceSP = double.MinValue;
							
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
									
								//Check here for Buy, Buy to Cover, Sell, Short Sell
								if(side.Equals(this._orderSide.Rows[1][1]) || side.Equals(this._orderSide.Rows[2][1])) //use some constant to check for buy/sell
								{
                                    
                                    ///Changed Rajat 19-Aug-2006
                                    longExposure += ((int)(Convert.ToInt32(order.CumQty) * feedPriceLE * conversionFactorFeedPrice)) * multiplier * delta; 
                                    //(int)(Math.Round((Convert.ToInt32(order.CumQty) * feedPriceLE * conversionFactorFeedPrice), 0)) * multiplier * delta;

                                    longPNL += (int)(Convert.ToInt32(order.CumQty) * (feedPriceLP * conversionFactorFeedPrice - Convert.ToDouble(order.AvgPrice) * conversionFactor) * multiplier);
                                    //Debug.WriteLine(order.Symbol + " Qty : " + order.CumQty + " Feed Price : " + feedPriceLE + " longExposure : " + longExposure + " longPNL : " + longPNL + " Avg Price : " + order.AvgPrice + " conversionFactorFeedPrice : " + conversionFactorFeedPrice);                                    
								}
								else
								{
									shortExposure += ((int)(Convert.ToInt32(order.CumQty)*feedPriceSE*conversionFactorFeedPrice))*multiplier*delta;
							
									shortPNL += (int)(-Convert.ToInt32(order.CumQty)*(feedPriceSP*conversionFactorFeedPrice - Convert.ToDouble(order.AvgPrice)*conversionFactor)*multiplier);
								}	
					
								executedQuantity += Convert.ToInt32(order.CumQty);
                                totalLevel2_NotionalValue += Convert.ToDouble(order.CumQty * order.AvgPrice);
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
					
				try
				{
					if(OrderSubDetail.Count > 0)
					{
						Order tempOrder = ((Order)OrderSubDetail[OrderSubDetail.Count-1]);

						SymbolL1Data l1Data = (SymbolL1Data)htSymbolData[tempOrder.Symbol];

						//						//Add new row at level1 if not found
						//						pnlLevel1Data = SearchLevel1Item(this._pnlLevel1DataCollection, level1ColumnIndex, PNLHelper.GetDisplayColumnValue(level1ColumnIndex, tempOrder));
						//									
						//						if(pnlLevel1Data == null)
						//						{
						//							pnlLevel1Data = new PNLLevel1Data();
						//							this._pnlLevel1DataCollection.Add(pnlLevel1Data);
						//						}

						//Add new row at level2
						PNLLevel2Data pnlLevel2Data = null;
						pnlLevel2Data = SearchLevel2Item(pnlLevel1Data, level2ColumnIndex, PNLHelper.GetDisplayColumnValue(level2ColumnIndex, tempOrder));
								
						if(pnlLevel2Data == null)
						{
							pnlLevel2Data = new PNLLevel2Data();
							pnlLevel1Data.Level2DataCollection.Add(pnlLevel2Data);
						}
					
						#region set the column data for level2 row				
						pnlLevel2Data.Exchange = tempOrder.ExchangeName;
						//pnlLevel2Data.AveragePrice =tempOrder.AvgPrice;
						pnlLevel2Data.TradingAccount = tempOrder.TradingAccountName;
						pnlLevel2Data.Side = tempOrder.OrderSide;
						pnlLevel2Data.Symbol = tempOrder.Symbol;
						pnlLevel2Data.Currency = tempOrder.CurrencyName;
						pnlLevel2Data.LongExposure = longExposure;
						pnlLevel2Data.ShortExposure = shortExposure;
						pnlLevel2Data.NetExposure = longExposure - shortExposure;
						pnlLevel2Data.LongPNL = longPNL;
						pnlLevel2Data.ShortPNL = shortPNL;
						pnlLevel2Data.NetPNL = (longPNL + shortPNL);
						
						if(l1Data != null)
						{
							pnlLevel2Data.Last = l1Data.Last; 
							pnlLevel2Data.Bid = l1Data.Bid; 
							pnlLevel2Data.Ask = l1Data.Ask;
							pnlLevel2Data.PercentChange = l1Data.Change; 
						}
						else
						{
							pnlLevel2Data.Last = 0; 
							pnlLevel2Data.Bid = 0; 
							pnlLevel2Data.Ask = 0;
							pnlLevel2Data.PercentChange = 0; 
						}

						pnlLevel2Data.ExecutedQuantity = executedQuantity;
                        pnlLevel2Data.AveragePrice = Convert.ToDouble(totalLevel2_NotionalValue / executedQuantity);
						pnlLevel2Data.UnderLying = tempOrder.UnderlyingName;
						pnlLevel2Data.Asset = tempOrder.AssetName;
						#endregion
					
						#region Update calculation
						//Update the Level1 summary
						totalLongExposure +=longExposure;
						totalShortExposure += shortExposure;
						totalShortPNL += shortPNL;
						totalLongPNL += longPNL;
						totalQuantity += executedQuantity;
						#endregion
					
						#region set the column data for level1 row
						pnlLevel1Data.Exchange = tempOrder.ExchangeName;
						pnlLevel1Data.AveragePrice = tempOrder.AvgPrice;
						pnlLevel1Data.TradingAccount = tempOrder.TradingAccountName;
						pnlLevel1Data.Side = tempOrder.OrderSide;
						pnlLevel1Data.Symbol = tempOrder.Symbol;
						pnlLevel1Data.Currency = tempOrder.CurrencyName;
						pnlLevel1Data.LongExposure = totalLongExposure;
						pnlLevel1Data.ShortExposure = totalShortExposure;
						pnlLevel1Data.NetExposure = totalLongExposure - totalShortExposure;
						pnlLevel1Data.LongPNL = totalLongPNL;
						pnlLevel1Data.ShortPNL = totalShortPNL;
						pnlLevel1Data.NetPNL = (totalLongPNL + totalShortPNL);
						
						if(l1Data != null)
						{
							pnlLevel1Data.Last = l1Data.Last; 
							pnlLevel1Data.Bid = l1Data.Bid; 
							pnlLevel1Data.Ask = l1Data.Ask;
							pnlLevel1Data.PercentChange = l1Data.Change; 
						}
						else
						{
							pnlLevel1Data.Last = 0; 
							pnlLevel1Data.Bid = 0; 
							pnlLevel1Data.Ask = 0;
							pnlLevel1Data.PercentChange = 0; 
						}

						pnlLevel1Data.ExecutedQuantity = totalQuantity;
						pnlLevel1Data.UnderLying = tempOrder.UnderlyingName;
						pnlLevel1Data.Asset = tempOrder.AssetName;				
						#endregion

						//this._pnlLevel1DataCollection.Add(pnlLevel1Data);
					
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
				
				//Remove the entries which we were not updated
				int level1Count = this._pnlLevel1DataCollection.Count;
				 
				
				for(int i=0;i<level1Count;i++)
				{
					PNLLevel1Data _pnlLevel1Data = this._pnlLevel1DataCollection[i];
					
					if(_pnlLevel1Data.IsUpdated == true)
					{
						_pnlLevel1Data.IsUpdated = false;
					}
					else
					{
						this._pnlLevel1DataCollection.RemoveAt(i);
						level1Count = this._pnlLevel1DataCollection.Count;
					}
				}

				this.grdPNL.ResumeLayout();

				if(this.grdPNL.DisplayLayout != null)
				{
					foreach(SummarySettings summarySettings in this.grdPNL.DisplayLayout.Bands[0].Summaries)
					{
						summarySettings.Refresh();

					}
				}

				try
				{
					//this.grdPNL.CalcManager.PerformAction(Infragistics.Win.CalcEngine.UltraCalcAction.Recalc, null);
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


                MethodInvoker mi = new MethodInvoker(grdPNL.Refresh);
                if (this.InvokeRequired)
                    this.BeginInvoke(mi);
                else
                    grdPNL.Refresh();

				//this.grdPNL.ref
				this.grdPNL.Visible = true;



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
				//MethodInvoker mi = new MethodInvoker(UpdateColumnColor);
				//mi.BeginInvoke(null,null);

                if (grdPNL.InvokeRequired)
                {
                    MethodInvoker mi = new MethodInvoker(UpdateColumnColor);
                    this.BeginInvoke(mi, null);
                }
                else
                {
                    UpdateColumnColor();
                }

				lock(this.grdPNL)
				{
					blnCalculationStarted = false;
				}
			}
		}
		
		/// <summary>
		/// Search for the exsting level1 data item for the same  value
		/// </summary>
		/// <param name="pnlLevel1DataCollection"></param>
		/// <param name="level1ColumnIndex"></param>
		/// <param name="level1ColumnValue"></param>
		/// <returns></returns>
		private PNLLevel1Data SearchLevel1Item(PNLLevel1DataCollection pnlLevel1DataCollection, int level1ColumnIndex, string level1ColumnValue )
		{
			try
			{
				int intCount = pnlLevel1DataCollection.Count;
			
				for(int i=0;i<intCount;i++)
				{
					PNLLevel1Data pnlLevel1Data = (PNLLevel1Data)pnlLevel1DataCollection[i];
				
					if(level1ColumnValue.Equals(PNLHelper.GetLevel1ColumnValue(level1ColumnIndex,pnlLevel1Data)))					
					{
						pnlLevel1Data.IsUpdated = true;

						return pnlLevel1Data;
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
			return null;
		}

		/// <summary>
		/// Search for the exsting level2 data item for the same  value
		/// </summary>
		/// <param name="pnlLevel1Data"></param>
		/// <param name="level2ColumnIndex"></param>
		/// <param name="level2ColumnValue"></param>
		/// <returns></returns>
		private PNLLevel2Data SearchLevel2Item(PNLLevel1Data pnlLevel1Data, int level2ColumnIndex, string level2ColumnValue )
		{
			try
			{
				int intCount = pnlLevel1Data.Level2DataCollection.Count;
			
				for(int i=0;i<intCount;i++)
				{
					PNLLevel2Data pnlLevel2Data = (PNLLevel2Data)pnlLevel1Data.Level2DataCollection[i];
				
					if(level2ColumnValue.Equals(PNLHelper.GetLevel2ColumnValue(level2ColumnIndex,pnlLevel2Data)))					
					{
						return pnlLevel2Data;
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
			return null;
		}

		/// <summary>
		/// Handler is called when control is loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PNLGridControl_Load(object sender, System.EventArgs e)
		{
		
		}

		/// <summary>
		/// Updates the Grid Preferences Asynchronusly
		/// </summary>
		public void UpdateGridPreference()
		{
			try
			{
				System.Windows.Forms.MethodInvoker methodInvoker = new System.Windows.Forms.MethodInvoker(this.UpdateGridPreferencesAsync);
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

/// <summary>
/// Updates the Grid Preferences
/// </summary>
		public void UpdateGridPreferencesAsync()
		{
			try
			{
				_applyingPreferences = true;
				bool blnCreateTable = false;

				//First check which preference to be apply

				switch(this._tabType)
				{
					case (int)PNLTab.Asset:
						
						#region Check of the Level1 preference modificaiton
						//Check the level1 and level2 column have been changed
						if(this._level1ColumnIndex != this._pnlPrefrences.Level1AssetColumn || this._level2ColumnIndex != this._pnlPrefrences.Level2AssetColumn)   
						{
							this.Level1ColumnIndex = this._pnlPrefrences.Level1AssetColumn;
							this.Level2ColumnIndex = this._pnlPrefrences.Level2AssetColumn;
							blnCreateTable = true;
						}
						#endregion

					break;

					case (int)PNLTab.Symbol:

						#region Check of the Level1 preference modificaiton
						if(this._level1ColumnIndex != this._pnlPrefrences.Level1SymbolColumn || this._level2ColumnIndex != this._pnlPrefrences.Level2SymbolColumn)
						{
							this.Level1ColumnIndex = this._pnlPrefrences.Level1SymbolColumn;
							this.Level2ColumnIndex = this._pnlPrefrences.Level2SymbolColumn;
							blnCreateTable = true;
						}

						#endregion

						break;

					case (int)PNLTab.TradingAccount:

						#region Check of the Level1 preference modificaiton
						if(this._level1ColumnIndex != this._pnlPrefrences.Level1TradingAccountColumn || this._level2ColumnIndex != this._pnlPrefrences.Level2TradingAccountColumn)   
						{
							this.Level1ColumnIndex = this._pnlPrefrences.Level1TradingAccountColumn;
							this.Level2ColumnIndex = this._pnlPrefrences.Level2TradingAccountColumn;
							blnCreateTable = true;
    					}
						#endregion

						break;
				}

				//update the UI of Grid 
                ///Rajat updated 24 July (Need to send the updation on the main grid)
                if (grdPNL.InvokeRequired)
                {
                    MethodInvoker mi = new MethodInvoker(UpdateGridUI);
                    this.BeginInvoke(mi, null);
                }
                else
                    this.UpdateGridUI();

                ///Rajat updated 24 July (Need to send the updation on the main grid)
                if (grdPNL.InvokeRequired)
                {
                    MethodInvoker mi1 = new MethodInvoker(UpdateGridColumnLayout);
                    this.BeginInvoke(mi1, null);
                }
                else
                    this.UpdateGridColumnLayout();

				//calculate PNL
				if(blnCreateTable)
				{
					this.ClearOrderCollection();
                    ///Rajat updated 24 July (Need to send the updation on the main grid)
                    if (grdPNL.InvokeRequired)
                    {
                        MethodInvoker mi1 = new MethodInvoker(UpdateGridAsync);
                        this.BeginInvoke(mi1, null);
                    }
                    else
					    UpdateGridAsync();
				}
				else
					this.UpdateColumnColor();

				_applyingPreferences = false;
			}
			catch(Exception ex)
			{
				_applyingPreferences = false;
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());

				throw;
			}			
		}


		public void ChangeCurrency()
		{
			try
			{
				System.Windows.Forms.MethodInvoker methodInvoker = new System.Windows.Forms.MethodInvoker(this.UpdateCurrencyCalculation);
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


		public void UpdateCurrencyCalculation()
		{
            if (OrderSubDetail != null)
            {
                //calculate PNL
                this.CalculatePNL(htSymbolData, OrderSubDetail, this._liveFeedManager.IsDataManagerConnected());
            }
		}
		

		delegate void CalulatePNLDelegate(Hashtable htSymbolData, OrderCollection OrderSubDetail, bool IsDataManagerConnected);
		/// <summary>
		/// Receives the data from live feed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _liveFeedManager_Level1DataResponse(object sender, EventArgs e)
		{
			try
			{
				//receive the symbol data
				System.Collections.Specialized.NameValueCollection level1DataCollection = (System.Collections.Specialized.NameValueCollection)sender;

				SymbolL1Data symbolL1Data;
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

				if(htSymbol !=  null && htSymbol.Contains(symbol))
				{

					if(htSymbolData.Contains(symbol))
					{
						symbolL1Data = (SymbolL1Data)htSymbolData[symbol];

                        symbolL1Data.Last = last;
                        symbolL1Data.Bid = bid;
                        symbolL1Data.Ask = ask;
                        symbolL1Data.Change = change;
                        //symbolL1Data.Close = Convert.ToDouble(level1DataCollection.Get("Close"));
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
                        //symbolL1Data.Close = Convert.ToDouble(level1DataCollection.Get("Close"));
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
						htSymbol.Clear();

						//Clear all the records
						//ds.Clear();
						//calculate PNL

						//CalulatePNLDelegate  calcPNL = new CalulatePNLDelegate(this.CalculatePNL);
						//calcPNL.BeginInvoke(htSymbolData, OrderSubDetail, true, null, null);
                        if(OrderSubDetail != null)
						    this.CalculatePNL(htSymbolData, OrderSubDetail, true);
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

		/// <summary>
		/// To clear the Order Collection
		/// </summary>
		private void ClearOrderCollection()
		{

            //while (this._pnlLevel1DataCollection.Count > 0)
            //{
            //    this._pnlLevel1DataCollection.RemoveAt(0);
            //}
		}

		private void grdPNL_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
//			try
//			{
//				if ((this.grdPNL != null) && (this.grdPNL.Rows.VisibleRowCount == 0))
//				{
//					StringFormat sf = new StringFormat();
//					sf.Alignment = StringAlignment.Center;
//					sf.LineAlignment = StringAlignment.Center;
//					e.Graphics.DrawString("There are no data available.", this.Font, Brushes.White, new RectangleF(0.0f, 0.0f, (float)this.grdPNL.DisplayLayout.Bands[0].Header.SizeResolved.Width, 90.0f), sf);
//					sf.Dispose();
//				}
//			}
//			catch (Exception ex)
//			{
//				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
//				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
//					FORM_NAME);
//				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
//				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());

//			}		
		}

		private void grdPNL_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
            ///TODO : A bit of hard coding need to remove it later on .
            //if (e.Row.Cells["Asset"].Value.ToString()  == string.Empty && e.Row.Cells["Underlying"].Value.ToString() == string.Empty)
            //    e.Row.Hidden = true;
            //else
            //    e.Row.Hidden = false;
		}

		private void UpdateColumnColor()
		{
			
			try
			{
				_applyingPreferences = true;
			
				this.grdPNL.SuspendLayout();

//				bool blnIsNetPNLExits = false;
//				bool blnIsShortPNLExits = false;
//				bool blnIsLongPNLExits = false;
//				bool blnIsNetExposureExits = false;
			
				string NetPNLColName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.NetPNL];
				string ShortPNLColName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.ShortPNL];
				string LongPNLColName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.LongPNL];
				string NetExposureColName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.LongExposure];
				string PctChangeColName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.PercentChange];

				//			UltraGridLayout displayLayout = this.grdPNL.DisplayLayout;
				//		
				//			//changing the color for the parent band
				//			if(displayLayout != null)
				//			{
				//				Infragistics.Win.UltraWinGrid.BandsCollection bandCollection = displayLayout.Bands; // layoutCollection
				//				if(bandCollection.Count > 0)
				//				{
				//				}
				//
				//				string columnName = PNLPrefrencesData.PNLColumnNames[(int)PNLColumns.LongExposure];
				//				if(bandCollection[0].Columns.Exists(columnName))
				//				{
				//				}
				//			}

				//Iterate over all the rows and change the color of text for both the level
				RowsCollection rowsCollection = this.grdPNL.Rows;
                if (rowsCollection != null && rowsCollection.Count > 0)
                {

                    int count = rowsCollection.Count;


                    for (int i = 0; i < count; i++)
                    {
                        UltraGridRow row = rowsCollection[i];

                        UpdateRowColumnColor(row, NetPNLColName);
                        UpdateRowColumnColor(row, ShortPNLColName);
                        UpdateRowColumnColor(row, LongPNLColName);
                        UpdateRowColumnColor(row, NetExposureColName);
                        UpdateRowColumnColor(row, PctChangeColName);

                        if (row.HasChild())
                        {
                            UltraGridRow childRow = row.GetChild(Infragistics.Win.UltraWinGrid.ChildRow.First);

                            UpdateRowColumnColor(childRow, NetPNLColName);
                            UpdateRowColumnColor(childRow, ShortPNLColName);
                            UpdateRowColumnColor(childRow, LongPNLColName);
                            UpdateRowColumnColor(childRow, NetExposureColName);
                            UpdateRowColumnColor(childRow, PctChangeColName);

                            while (childRow.HasNextSibling())
                            {
                                childRow = childRow.GetSibling(Infragistics.Win.UltraWinGrid.SiblingRow.Next);

                                UpdateRowColumnColor(childRow, NetPNLColName);
                                UpdateRowColumnColor(childRow, ShortPNLColName);
                                UpdateRowColumnColor(childRow, LongPNLColName);
                                UpdateRowColumnColor(childRow, NetExposureColName);
                                UpdateRowColumnColor(childRow, PctChangeColName);
                            }
                        }
                    }
                }
				this.grdPNL.ResumeLayout();

			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				//AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				//appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
			finally
			{
				_applyingPreferences = false;
			}
		}

		private void UpdateRowColumnColor(UltraGridRow row, string ColName)
		{
			try
			{
				if(row.Cells.Exists(ColName))
				{
					if((Convert.ToInt32(row.Cells[ColName].Value))<0)
					{
						row.Cells[ColName].Appearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.NegativeValueColor);
					}
					else
					{
						row.Cells[ColName].Appearance.ForeColor = PNLPrefrencesData.GetColorFromARGB(this._pnlPrefrences.PositiveValueColor);
					}
				}
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				//AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				//appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), Common.POLICY_LOGANDSHOW, System.Guid.NewGuid());
			}
		}
	}

  
}



