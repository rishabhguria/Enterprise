using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Blotter.Forms
{
    public partial class MultiBrokerSubOrders : Form
    {
        static private MultiBrokerSubOrders _multiBrokerSubOrders = null;
        private string _preferencePath = BlotterPreferenceManager.GetInstance().GetOrdersPreferencePath(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser, "StagedOrders");

        public MultiBrokerSubOrders()
        {
            InitializeComponent();
        }

        public void SetUp(string parentClOrderId)
        {
            try
            {
                ultraGrid1.DataSource = BlotterOrderCollections.GetInstance().PrepareMultiBrokerOrdersList(parentClOrderId);
                LoadGridPreferences();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void MultiBrokerSubOrders_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void saveLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (ultraGrid1.DataSource != null)
                    ultraGrid1.DisplayLayout.SaveAsXml(_preferencePath);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void restoreDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetLayoutForGird();
                if (ultraGrid1.DataSource != null)
                    ultraGrid1.DisplayLayout.SaveAsXml(_preferencePath);
                LoadGridPreferences();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraGrid1_BeforeColumnChooserDisplayed(object sender, Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.ultraGrid1);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Load Grid Preferences
        /// </summary>
        public void LoadGridPreferences()
        {
            try
            {
                if (System.IO.File.Exists(_preferencePath) && ultraGrid1.DisplayLayout != null)
                    ultraGrid1.DisplayLayout.LoadFromXml(_preferencePath);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                SetLayoutForGird();
                LoadGridPreferences();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetLayoutForGird()
        {
            try
            {
                UltraGridBand band = ultraGrid1.DisplayLayout.Bands[0];
                List<string> lsColumnsToDisplay = new List<string>(new string[] { OrderFields.PROPERTY_TRANSACTION_TIME, OrderFields.PROPERTY_SYMBOL, OrderFields.PROPERTY_ORDER_SIDE, OrderFields.PROPERTY_QUANTITY
                , OrderFields.PROPERTY_ORDER_TYPE, OrderFields.PROPERTY_PRICE, OrderFields.PROPERTY_EXECUTED_QTY, OrderFields.PROPERTY_LEAVES_QUANTITY, OrderFields.PROPERTY_AVGPRICE, OrderFields.PROPERTY_LASTPRICE,
                OrderFields.PROPERTY_ORDER_STATUS, OrderFields.PROPERTY_COUNTERPARTY_NAME, OrderFields.PROPERTY_USER, OrderFields.PROPERTY_VENUE, OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL, OrderFields.PROPERTY_AVGPRICEBASE , OrderFields.PROPERTY_TIF_TAGVALUE });
                foreach (UltraGridColumn existingCol in ultraGrid1.DisplayLayout.Bands[0].Columns)
                {
                    existingCol.Hidden = true;
                    existingCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                }
                UltraWinGridUtils.SetBand(lsColumnsToDisplay, band);

                band.Columns[Global.OrderFields.PROPERTY_ASSET_NAME].Header.Caption = OrderFields.CAPTION_ASSET_NAME;
                band.Columns[Global.OrderFields.PROPERTY_ASSET_NAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].Header.Caption = OrderFields.CAPTION_FX_CONVERSION_METHOD_OPERATOR;
                band.Columns[Global.OrderFields.PROPERTY_FXCONVERSIONMETHODOPERATOR].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_FXRATE].Header.Caption = OrderFields.CAPTION_FX_RATE;
                band.Columns[Global.OrderFields.PROPERTY_FXRATE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_LAST_SHARES].Header.Caption = OrderFields.CAPTION_GRID_LAST_SHARES;
                band.Columns[Global.OrderFields.PROPERTY_LAST_SHARES].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_ORDER_ID].Header.Caption = OrderFields.CAPTION_ORDER_ID;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_ID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_PARENT_CL_ORDERID].Header.Caption = OrderFields.PROPERTY_PARENT_CL_ORDERID;
                band.Columns[Global.OrderFields.PROPERTY_PARENT_CL_ORDERID].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_PUT_CALL].Header.Caption = OrderFields.CAPTION_PUT_CALL;
                band.Columns[Global.OrderFields.PROPERTY_PUT_CALL].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_SYMBOL].Header.Caption = OrderFields.CAPTION_SYMBOL;
                band.Columns[Global.OrderFields.PROPERTY_SYMBOL].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_SYMBOL].Width = 80;
                band.Columns[Global.OrderFields.PROPERTY_SYMBOL].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_TRANSACTION_TIME].Header.Caption = OrderFields.CAPTION_TRANSACTION_TIME;
                band.Columns[Global.OrderFields.PROPERTY_TRANSACTION_TIME].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_TRANSACTION_TIME].Width = 80;
                band.Columns[Global.OrderFields.PROPERTY_TRANSACTION_TIME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[OrderFields.PROPERTY_TRANSACTION_TIME].Format = DateTimeConstants.NirvanaDateTimeFormat;

                band.Columns[Global.OrderFields.PROPERTY_VENUE].Header.Caption = OrderFields.CAPTION_VENUE;
                band.Columns[Global.OrderFields.PROPERTY_VENUE].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_VENUE].Width = 80;
                band.Columns[Global.OrderFields.PROPERTY_VENUE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL].Width = 180;
                band.Columns[Global.OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_TIF_TAGVALUE].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_TIF_TAGVALUE].Width = 80;
                band.Columns[Global.OrderFields.PROPERTY_TIF_TAGVALUE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                DataTable dataTableTIF = CommonDataCache.TagDatabase.GetInstance().TIF;
                ValueList timeInForce = new ValueList();
                foreach (DataRow item in dataTableTIF.Rows)
                {
                    timeInForce.ValueListItems.Add(Convert.ToString(item[1]), Convert.ToString(item[2]));
                }
                timeInForce.ValueListItems.Add(0, ApplicationConstants.C_COMBO_NONE);

                if (band.Columns.Exists(OrderFields.PROPERTY_TIF_TAGVALUE))
                {
                    band.Columns[OrderFields.PROPERTY_TIF_TAGVALUE].ValueList = timeInForce;
                    band.Columns[OrderFields.PROPERTY_TIF_TAGVALUE].CellActivation = Activation.NoEdit;
                    band.Columns[OrderFields.PROPERTY_TIF_TAGVALUE].Width = 100;
                    band.Columns[OrderFields.PROPERTY_TIF_TAGVALUE].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                }


                band.Columns[Global.OrderFields.PROPERTY_UNDERLYINGSYMBOL].Header.Caption = OrderFields.CAPTION_UNDERLYINGSYMBOL;
                band.Columns[Global.OrderFields.PROPERTY_UNDERLYINGSYMBOL].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE1].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_1);
                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE1].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE2].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_2);
                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE2].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE3].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_3);
                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE3].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE4].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_4);
                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE4].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE5].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_5);
                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE5].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE6].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE_6);
                band.Columns[Global.OrderFields.PROPERTY_TRADEATTRIBUTE6].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                for (int i = 7; i <= 45; i++)
                {
                    band.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue(OrderFields.CAPTION_TRADE_ATTRIBUTE + i);
                    band.Columns[OrderFields.PROPERTY_TRADEATTRIBUTE + i].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                }

                band.Columns[Global.OrderFields.PROPERTY_INTERNALCOMMENTS].Header.Caption = OrderFields.CAPTION_INTERNAL_COMMENTS;
                band.Columns[Global.OrderFields.PROPERTY_INTERNALCOMMENTS].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_BLOOMBERGSYMBOL].Header.Caption = OrderFields.CAPTION_BLOOMBERG_SYMBOL;
                band.Columns[Global.OrderFields.PROPERTY_BLOOMBERGSYMBOL].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_SEDOLSYMBOL].Header.Caption = OrderFields.CAPTION_SEDOLSYMBOL;
                band.Columns[Global.OrderFields.PROPERTY_SEDOLSYMBOL].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_COMPANYNAME].Header.Caption = OrderFields.CAPTION_COMPANYNAME;
                band.Columns[Global.OrderFields.PROPERTY_COMPANYNAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_QUANTITY].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_QUANTITY].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_QUANTITY].Width = 80;
                band.Columns[Global.OrderFields.PROPERTY_QUANTITY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_PRICE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_LEAVES_QUANTITY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_SIDE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_TYPE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_EXECUTED_QTY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICEBASE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_STATUS].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_ALGOSTRATEGYNAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_COUNTERPARTY_NAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_USER].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_STRIKE_PRICE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_STOP_PRICE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_LASTPRICE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_PEG_DIFF].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_UNDERLYING_NAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_TRADING_ACCOUNT].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[Global.OrderFields.PROPERTY_PROCESSDATE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[OrderFields.PROPERTY_NOTIONALVALUE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                band.Columns[OrderFields.PROPERTY_NOTIONALVALUEBASE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[OrderFields.PROPERTY_DAY_EXECUTED_QUANTITY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[OrderFields.PROPERTY_DAY_EXECUTED_QUANTITY].Hidden = true;
                band.Columns[OrderFields.PROPERTY_START_OF_DAY_QUANTITY].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[OrderFields.PROPERTY_START_OF_DAY_QUANTITY].Hidden = true;
                band.Columns[OrderFields.PROPERTY_DAY_AVERAGE_PRICE].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[OrderFields.PROPERTY_DAY_AVERAGE_PRICE].Hidden = true;

                band.Columns[Global.OrderFields.PROPERTY_LASTPRICE].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_LASTPRICE].Width = 80;
                band.Columns[Global.OrderFields.PROPERTY_PRICE].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_PRICE].Width = 80;
                band.Columns[Global.OrderFields.PROPERTY_LEAVES_QUANTITY].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_LEAVES_QUANTITY].Width = 80;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_SIDE].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_SIDE].Width = 80;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_TYPE].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_TYPE].Width = 80;
                band.Columns[Global.OrderFields.PROPERTY_EXECUTED_QTY].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_EXECUTED_QTY].Width = 90;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICE].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICE].Width = 140;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICEBASE].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICEBASE].Width = 140;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_STATUS].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_STATUS].Width = 80;
                band.Columns[Global.OrderFields.PROPERTY_COUNTERPARTY_NAME].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_COUNTERPARTY_NAME].Width = 90;
                band.Columns[Global.OrderFields.PROPERTY_USER].Hidden = false;
                band.Columns[Global.OrderFields.PROPERTY_USER].Width = 80;

                foreach (UltraGridColumn col in band.Columns.All)
                    col.CellAppearance.TextHAlign = HAlign.Center;
                band.Columns[Global.OrderFields.PROPERTY_PRICE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICEBASE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_LAST_SHARES].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_LASTPRICE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_STOP_PRICE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_LEAVES_QUANTITY].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_STRIKE_PRICE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_UNSENT_QTY].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_EXECUTED_QTY].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_LEAVES_QUANTITY].Header.Caption = OrderFields.CAPTION_WORKING_QTY;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_SIDE].Header.Caption = OrderFields.CAPTION_ORDER_SIDE;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_TYPE].Header.Caption = OrderFields.CAPTION_ORDER_TYPE;
                band.Columns[Global.OrderFields.PROPERTY_EXECUTED_QTY].Header.Caption = OrderFields.CAPTION_EXECUTED_QTY;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICE].Header.Caption = OrderFields.CAPTION_AVG_FILL_PRICE_LOCAL;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICEBASE].Header.Caption = OrderFields.CAPTION_AVG_FILL_PRICE_BASE;
                band.Columns[Global.OrderFields.PROPERTY_EXECUTION_TIME_LAST_FILL].Header.Caption = OrderFields.CAPTION_EXECUTION_TIME_LAST_FILL;
                band.Columns[Global.OrderFields.PROPERTY_PRICE].Header.Caption = OrderFields.CAPTION_LIMITPX;
                band.Columns[Global.OrderFields.PROPERTY_ASSET_NAME].Header.Caption = OrderFields.CAPTION_ASSET_NAME;
                band.Columns[Global.OrderFields.PROPERTY_IMPORTFILENAME].Header.Caption = OrderFields.CAPTION_IMPORTED_FILE_NAME;
                band.Columns[Global.OrderFields.PROPERTY_LAST_SHARES].Header.Caption = OrderFields.CAPTION_GRID_LAST_SHARES;
                band.Columns[Global.OrderFields.PROPERTY_PUT_CALL].Header.Caption = OrderFields.CAPTION_PUT_CALL;
                band.Columns[Global.OrderFields.PROPERTY_TRANSACTION_TIME].Header.Caption = OrderFields.CAPTION_TRANSACTION_TIME;
                band.Columns[Global.OrderFields.PROPERTY_ORDER_STATUS].Header.Caption = OrderFields.CAPTION_ORDER_STATUS;
                band.Columns[Global.OrderFields.PROPERTY_ALGOSTRATEGYNAME].Header.Caption = OrderFields.CAPTION_ALGOSTRATEGYNAME;
                band.Columns[Global.OrderFields.PROPERTY_COUNTERPARTY_NAME].Header.Caption = OrderFields.CAPTION_COUNTER_PARTY;
                band.Columns[Global.OrderFields.PROPERTY_USER].Header.Caption = OrderFields.CAPTION_USER;
                band.Columns[Global.OrderFields.PROPERTY_STRIKE_PRICE].Header.Caption = OrderFields.CAPTION_STRIKE_PRICE;
                band.Columns[Global.OrderFields.PROPERTY_STOP_PRICE].Header.Caption = OrderFields.CAPTION_STOP_PRICE;
                band.Columns[Global.OrderFields.PROPERTY_LASTPRICE].Header.Caption = OrderFields.CAPTION_LASTPRICE_BLOTTER;
                band.Columns[Global.OrderFields.PROPERTY_PEG_DIFF].Header.Caption = OrderFields.CAPTION_PEG;
                band.Columns[Global.OrderFields.PROPERTY_UNDERLYING_NAME].Header.Caption = OrderFields.CAPTION_UNDERLYING_COUNTRY;
                band.Columns[Global.OrderFields.PROPERTY_TRADING_ACCOUNT].Header.Caption = OrderFields.CAPTION_TRADER;
                band.Columns[Global.OrderFields.PROPERTY_PROCESSDATE].Header.Caption = Global.OrderFields.CAPTION_PROCESS_DATE;
                band.Columns[OrderFields.PROPERTY_NOTIONALVALUE].Header.Caption = OrderFields.CAPTION_NOTIONAL_VALUE_LOCAL;
                band.Columns[OrderFields.PROPERTY_NOTIONALVALUEBASE].Header.Caption = OrderFields.CAPTION_NOTIONAL_VALUE_BASE_BLOTTER;
                band.Columns[Global.OrderFields.PROPERTY_COMMISSIONAMT].Header.Caption = OrderFields.CAPTION_COMMISSION;
                band.Columns[Global.OrderFields.PROPERTY_COMMISSIONAMT].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[Global.OrderFields.PROPERTY_COMMISSIONRATE].Header.Caption = OrderFields.CAPTION_COMMISSION_RATE;
                band.Columns[Global.OrderFields.PROPERTY_COMMISSIONRATE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[Global.OrderFields.PROPERTY_COMMISSIONRATE].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_COMMISSIONAMT].CellAppearance.TextHAlign = HAlign.Right;
                band.Columns[Global.OrderFields.PROPERTY_CALCBASIS].Header.Caption = OrderFields.CAPTION_CALCULATION_BASIS;

                band.Columns[Global.OrderFields.PROPERTY_IMPORTFILENAME].Header.Caption = OrderFields.CAPTION_IMPORTED_FILE_NAME;
                band.Columns[Global.OrderFields.PROPERTY_IMPORTFILENAME].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                band.Columns[Global.OrderFields.PROPERTY_QUANTITY].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[Global.OrderFields.PROPERTY_PRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[Global.OrderFields.PROPERTY_AVGPRICEBASE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[Global.OrderFields.PROPERTY_EXECUTED_QTY].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[Global.OrderFields.PROPERTY_LAST_SHARES].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[Global.OrderFields.PROPERTY_LASTPRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[Global.OrderFields.PROPERTY_LEAVES_QUANTITY].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[Global.OrderFields.PROPERTY_UNSENT_QTY].Format = ApplicationConstants.FORMAT_QTY;
                band.Columns[Global.OrderFields.PROPERTY_STOP_PRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[Global.OrderFields.PROPERTY_STRIKE_PRICE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_NOTIONALVALUE].Format = ApplicationConstants.FORMAT_COSTBASIS;
                band.Columns[OrderFields.PROPERTY_NOTIONALVALUEBASE].Format = ApplicationConstants.FORMAT_COSTBASIS;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void UpdateGrid()
        {
            try
            {
                ultraGrid1.DataBind();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static MultiBrokerSubOrders GetInstance()
        {
            try
            {
                if (_multiBrokerSubOrders == null)
                {
                    _multiBrokerSubOrders = new MultiBrokerSubOrders();
                    _multiBrokerSubOrders.FormClosing += new FormClosingEventHandler(MultiBrokerSubOrders_FormClosing);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _multiBrokerSubOrders;
        }

        private static void MultiBrokerSubOrders_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_multiBrokerSubOrders != null)
                {
                    _multiBrokerSubOrders.FormClosing -= new FormClosingEventHandler(MultiBrokerSubOrders_FormClosing);
                    _multiBrokerSubOrders.Dispose();
                    _multiBrokerSubOrders = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraGrid1_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            try
            {
                (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}