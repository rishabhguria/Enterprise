using ExportGridsData;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.Admin.BLL;
using Prana.Analytics;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PM.DAL;
using Prana.PubSubService.Interfaces;
using Prana.Tools.PL.OptionInputs;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class OptionModelInputs : Form, IPluggableTools, ILiveFeedCallback, IPublishing, IpluggableToolPI, IExportGridData
    {
        private int _scrollPos = 0;
        OMIPrefManager _prefManager = new OMIPrefManager();
        //static object _locker = new object();
        bool isDataSaved = true;
        bool _isSaveclick = true;
        //int hashcode = int.MinValue;
        DataTable dtOMI = null;
        //DataSet dsIR = null;
        //int _userID = int.MinValue;
        //private RiskPrefernece _riskPrefs;
        IPricingAnalysis _pricingAnalysis = null;
        //private delegate void Level1DataUpdateHandler(SymbolData level1Data);
        private delegate void UIThreadMarshallerGreekscalc(SymbolData symbolData);
        //private delegate void UIThreadMarshallers(object sender, RunWorkerCompletedEventArgs e);
        private delegate void UIThreadMarshaller(PranaRequestCarrier pranaRequestCarrier);
        private delegate void UIThreadMarshallerUpdateToolStrip(string statusMessage);
        public delegate void UIThreadMarshallerPublish(MessageData data, string topic);
        BackgroundWorker bgGetHistoricalvol = null;
        int _responseReceivedCount = 0;
        //int _companyID = 0;
        int _checkedIndex = 0;
        private const string const_BloombergSymbolExCode = "BloombergSymbolWithExchangeCode";
        private const string const_Header_BloombergSymbolExCode = "Bloomberg Symbol(With Exchange Code)";
        CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();
        ProxyBase<IPranaPositionServices> _positionManagementServices = null;
        public void CreatePositionManagementProxy()
        {
            try
            {
                _positionManagementServices = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
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

        DuplexProxyBase<IPricingService> _pricingServiceProxy = null;
        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServiceProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
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

        ProxyBase<IRiskServices> _riskServiceProxy = null;
        private void CreateRiskServiceProxy()
        {
            try
            {
                _riskServiceProxy = new ProxyBase<IRiskServices>("PricingRiskServiceEndpointAddress");
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

        DuplexProxyBase<ISubscription> _proxy;
        private void CreateSubscriptionServicesProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                _proxy.Subscribe(Topics.Topic_SecurityMaster, null);
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

        DuplexProxyBase<ISubscription> _proxyPricing;
        public void CreateSubscriptionServicesProxyPricing()
        {
            try
            {
                _proxyPricing = new DuplexProxyBase<ISubscription>("PricingSubscriptionEndpointAddress", this);
                _proxyPricing.Subscribe(Topics.Topic_OMIData, null);
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public OptionModelInputs()
        {
            try
            {
                InitializeComponent();
                HistoricalVolInputsUI.setDefaultValues_HistoricalVolInputs();
                if (!ModuleManager.CheckModulePermissioning(btnSymbolLookup.Text, btnSymbolLookup.Text))
                    btnSymbolLookup.Enabled = false;
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
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <returns></returns>
        private List<PSSymbolRequestObject> CreatePSSymbolRequestObjectList()
        {
            List<PSSymbolRequestObject> psObjList = new List<PSSymbolRequestObject>();
            try
            {
                foreach (DataRow dr in dtOMI.Rows)
                {
                    PSSymbolRequestObject PSrequestObject = new PSSymbolRequestObject();

                    PSrequestObject.Symbol = Convert.ToString(dr["Symbol"]);
                    PSrequestObject.UnderlyingSymbol = Convert.ToString(dr["UnderlyingSymbol"]);
                    PSrequestObject.AssetID = Convert.ToInt32(dr["AssetID"]);

                    int baseAssetID = Mapper.GetBaseAsset(PSrequestObject.AssetID);
                    if (baseAssetID == (int)AssetCategory.Option)
                    {
                        PSrequestObject.PutOrCall = (Convert.ToString(dr["PutorCall"]).Substring(0, 1));
                        PSrequestObject.StrikePrice = Convert.ToDouble(dr["StrikePrice"]);
                        if (!dr["ExpirationDate"].Equals(System.DBNull.Value))
                        {
                            PSrequestObject.ExpirationDate = Convert.ToDateTime(dr["ExpirationDate"]);
                        }
                    }
                    if (baseAssetID == (int)AssetCategory.Future)
                    {
                        if (!dr["ExpirationDate"].Equals(System.DBNull.Value))
                        {
                            PSrequestObject.ExpirationDate = Convert.ToDateTime(dr["ExpirationDate"]);
                        }
                    }
                    PSrequestObject.AUECID = Convert.ToInt32(dr["AuecID"]);
                    PSrequestObject.Volatility = Convert.ToDouble(dr["Volatility"]);
                    PSrequestObject.ExchangeID = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(PSrequestObject.AUECID);
                    PSrequestObject.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(PSrequestObject.ExchangeID);
                    psObjList.Add(PSrequestObject);
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
            return psObjList;
        }

        Dictionary<string, string> _dictPSsymbols = null;
        List<string> _lsRequiredColumns = null;
        /// <summary>
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="isOptions"></param>
        private void SetupOptionModelInputs()
        {
            try
            {
                _lsRequiredColumns = GetListOfColumnsToDisplayOnGrid();
                FetchOMIDataFromDB(true, _symbolListForFetch);
                BindCombos();
                LoadPreferences();
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
            finally
            {
                _symbolListForFetch = string.Empty;
            }
        }

        private List<string> GetListOfColumnsToDisplayOnGrid()
        {
            List<string> lsColumnsToDisplay = null;
            try
            {
                lsColumnsToDisplay = new List<string>();
                lsColumnsToDisplay.Add("Volatility");
                lsColumnsToDisplay.Add("VolatilityUsed");
                lsColumnsToDisplay.Add("IntRate");
                lsColumnsToDisplay.Add("IntRateUsed");
                lsColumnsToDisplay.Add("IsHistorical");
                lsColumnsToDisplay.Add("Dividend");
                lsColumnsToDisplay.Add("DividendUsed");
                lsColumnsToDisplay.Add("StockBorrowCost");
                lsColumnsToDisplay.Add("StockBorrowCostUsed");
                lsColumnsToDisplay.Add("Delta");
                lsColumnsToDisplay.Add("DeltaUsed");
                lsColumnsToDisplay.Add("LastPrice");
                lsColumnsToDisplay.Add("LastPriceUsed");
                lsColumnsToDisplay.Add("ForwardPoints");
                lsColumnsToDisplay.Add("TheoreticalPriceUsed");
                lsColumnsToDisplay.Add("ProxySymbolUsed");
                lsColumnsToDisplay.Add("ProxySymbol");
                lsColumnsToDisplay.Add("ForwardPointsUsed");
                lsColumnsToDisplay.Add("AssetID");
                lsColumnsToDisplay.Add("Symbol");
                lsColumnsToDisplay.Add("PSSymbol");
                lsColumnsToDisplay.Add("PBSymbol");
                lsColumnsToDisplay.Add("SecurityDescription");
                lsColumnsToDisplay.Add("SMSharesOutstanding");
                lsColumnsToDisplay.Add("SMSharesOutstandingUsed");
                lsColumnsToDisplay.Add("SharesOutstanding");
                lsColumnsToDisplay.Add("SharesOutstandingUsed");
                lsColumnsToDisplay.Add("ClosingMarkUsed");
                lsColumnsToDisplay.Add("HistoricalVolUsed");
                lsColumnsToDisplay.Add("HistoricalVol");
                lsColumnsToDisplay.Add("UnderlyingSymbol");
                lsColumnsToDisplay.Add("Bloomberg");
                lsColumnsToDisplay.Add("OSIOptionSymbol");
                lsColumnsToDisplay.Add("IDCOOptionSymbol");
                lsColumnsToDisplay.Add("LeadCurrencyID");
                lsColumnsToDisplay.Add("VsCurrencyID");
                lsColumnsToDisplay.Add("StrikePrice");
                lsColumnsToDisplay.Add("PutorCall");
                lsColumnsToDisplay.Add("ExpirationDate");
                lsColumnsToDisplay.Add("AuecID");
                lsColumnsToDisplay.Add("ManualInput");
                lsColumnsToDisplay.Add(const_BloombergSymbolExCode);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return lsColumnsToDisplay;
        }

        private void FetchOMIDataFromDB(bool fetchZeroPositionData, string symbols = "")
        {
            try
            {
                //Special handling for PSSymbol
                //Jira Link: http://jira.nirvanasolutions.com:8080/browse/PRANA-5974
                bool psSymbolVisible = false;
                if (grdOptionModel.DisplayLayout.Bands[0].Columns.Exists("PSSymbol"))
                {
                    psSymbolVisible = grdOptionModel.DisplayLayout.Bands[0].Columns["PSSymbol"].Hidden;
                }
                DataSet ds = new DataSet();
                List<UserOptModelInput> listOMIdata = _pricingServiceProxy.InnerChannel.GetOMIDataFromCache(fetchZeroPositionData, symbols);
                listOMIdata = UpdateOMICollectionWithPercentageValues(listOMIdata);

                if (listOMIdata != null)
                {
                    ds = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateTableStructureFromObject(listOMIdata, _lsRequiredColumns);
                }
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Columns["HistoricalVolUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["VolatilityUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["IntRateUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["DividendUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["StockBorrowCostUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["DeltaUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["LastPriceUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["TheoreticalPriceUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["ProxySymbolUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["ForwardPointsUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["SharesOutstandingUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["ClosingMarkUsed"].DataType = typeof(bool);
                    ds.Tables[0].Columns["ManualInput"].DataType = typeof(bool);
                    ds.Tables[0].Columns["SMSharesOutstandingUsed"].DataType = typeof(bool);
                    Prana.Utilities.MiscUtilities.GeneralUtilities.FillDataSetFromCollection(listOMIdata, ref ds, false, true, _lsRequiredColumns);
                    dtOMI = ds.Tables[0];
                }
                CentralRiskPositionsManager.GetInstance.PricingServiceProxy = _pricingServiceProxy;
                _dictPSsymbols = CentralRiskPositionsManager.GetInstance.GetPSSymbols(CreatePSSymbolRequestObjectList());
                dtOMI.Columns.Add("ImpliedVol", typeof(double));
                dtOMI.Columns.Add("InterestRate", typeof(double));
                dtOMI.Columns.Add("ActualDividend", typeof(double));
                dtOMI.Columns.Add("ActualDelta", typeof(double));
                dtOMI.Columns.Add("LastPx", typeof(double));
                dtOMI.Columns.Add("ActualSharesOutStanding", typeof(double));

                DataColumn[] pkColArray = new DataColumn[1];
                pkColArray[0] = dtOMI.Columns["Symbol"];
                dtOMI.PrimaryKey = pkColArray;
                SetDefaultColumnValues();
                dtOMI.AcceptChanges();
                grdOptionModel.DataSource = dtOMI;
                if (uOSetSymbols.CheckedIndex == 1)
                    FilterGridToDisplayOptions();
                else if (uOSetSymbols.CheckedIndex == 2)
                    FilterGridToDisplayOptionsAndUnderliers();
                GetAssetClass();
                grdOptionModel.DisplayLayout.Bands[0].Columns["ManualInput"].SortIndicator = SortIndicator.Descending;
                grdOptionModel.DisplayLayout.Bands[0].Columns["ManualInput"].CellActivation = Activation.NoEdit;
                grdOptionModel.UpdateData();

                //Special handling for PSSymbol
                //Jira Link: http://jira.nirvanasolutions.com:8080/browse/PRANA-5974
                if (grdOptionModel.DisplayLayout.Bands[0].Columns.Exists("PSSymbol"))
                {
                    if (psSymbolVisible)
                    {
                        grdOptionModel.DisplayLayout.Bands[0].Columns["PSSymbol"].Hidden = true;
                    }
                    else
                    {
                        grdOptionModel.DisplayLayout.Bands[0].Columns["PSSymbol"].Hidden = false;
                    }
                }
                grdOptionModel.DisplayLayout.RefreshFilters();
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

        private List<UserOptModelInput> UpdateOMICollectionWithPercentageValues(List<UserOptModelInput> listOMIdata)
        {
            List<UserOptModelInput> listToBind = new List<UserOptModelInput>();
            try
            {
                if (listOMIdata != null)
                {
                    foreach (UserOptModelInput userOMI in listOMIdata)
                    {
                        if (!userOMI.IsHistorical)
                        {
                            userOMI.HistoricalVol = userOMI.HistoricalVol * 100;
                            userOMI.Volatility = userOMI.Volatility * 100;
                            userOMI.IntRate = userOMI.IntRate * 100;
                            userOMI.Dividend = userOMI.Dividend * 100;
                            userOMI.StockBorrowCost = userOMI.StockBorrowCost * 100;
                            listToBind.Add(userOMI);
                        }
                    }
                }
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
            return listToBind;
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                timerRefresh.Stop();
                if (_responseReceivedCount == 0)
                {
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripLabelOMI.ForeColor = Color.Red;
                    }

                    toolStripLabelOMI.Text = DateTime.Now.ToString() + ": Operation Timed Out!";
                }
                else
                {
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripLabelOMI.ForeColor = Color.Green;
                    }

                    toolStripLabelOMI.Text = DateTime.Now.ToString() + ": Data Refreshed";
                }
                EnableForm();
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

        private void GetAssetClass()
        {
            try
            {
                foreach (UltraGridRow row in grdOptionModel.Rows)
                {
                    int assetID = Convert.ToInt32(row.Cells["AssetID"].Value.ToString());
                    row.Cells["AssetClass"].Value = CommonDataCache.CachedDataManager.GetInstance.GetAssetText(assetID).ToString();
                }
                grdOptionModel.DisplayLayout.Bands[0].Columns["AssetClass"].PerformAutoResize();
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

        private void SetDefaultColumnValues()
        {
            try
            {
                foreach (DataRow dr in dtOMI.Rows)
                {
                    dr["PSSymbol"] = GetPSSymbol(dr);
                    dr["HistoricalVol"] = 0.00;

                    // Set Default Delta
                    int assetID = Convert.ToInt32(dr["AssetID"].ToString());
                    if (assetID != 2)
                    {
                        dr["ActualDelta"] = 1;
                    }
                }
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

        private string GetPSSymbol(DataRow dr)
        {
            try
            {
                string symbol = dr["Symbol"].ToString();

                if (!string.IsNullOrEmpty(symbol) && _dictPSsymbols.ContainsKey(symbol))
                {
                    return _dictPSsymbols[symbol];
                }
                return string.Empty;
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
            return string.Empty;
        }

        private void BindCombos()
        {
            try
            {
                List<EnumerationValue> listFeedPxChecks = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(SelectedFeedPrice));
                SetComboDataSource(cmbBxOverrideWithOptions, listFeedPxChecks);
                SetComboDataSource(cmbBxOverrideWithOthers, listFeedPxChecks);

                //remove 'Never' option from list
                listFeedPxChecks = new List<EnumerationValue>(listFeedPxChecks.Where(x => x.DisplayText != SelectedFeedPrice.None.ToString() && x.DisplayText != SelectedFeedPrice.AskOrBid.ToString()));
                SetComboDataSource(cmbOptPrice, listFeedPxChecks);
                SetComboDataSource(cmbStockPrice, listFeedPxChecks);

                List<EnumerationValue> listNumericConditions = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(NumericConditionOperator));
                SetComboDataSource(cmbBxOverrideConditionOptions, listNumericConditions);
                SetComboDataSource(cmbBxOverrideConditionOthers, listNumericConditions);

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

        /// <summary>
        /// Sets the combo data source.
        /// </summary>
        /// <param name="ultraCombo">The ultra combo.</param>
        /// <param name="dataSourceList">The data source list.</param>
        private void SetComboDataSource(UltraCombo ultraCombo, List<EnumerationValue> dataSourceList)
        {
            try
            {
                string storedVal = ultraCombo.Text;
                ultraCombo.DisplayMember = "DisplayText";
                ultraCombo.ValueMember = "Value";
                ultraCombo.DataSource = dataSourceList;
                ultraCombo.DisplayLayout.Bands[0].ColHeadersVisible = false;
                ultraCombo.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                EnumerationValue selectedVal = dataSourceList.FirstOrDefault(x => x.DisplayText == storedVal);
                ultraCombo.Value = selectedVal != null ? selectedVal.Value : dataSourceList.FirstOrDefault().Value;

                //set tooltip
                int i = 0;
                foreach (EnumerationValue value in dataSourceList)
                {
                    ultraCombo.Rows[i].ToolTipText = value.DisplayText;
                    i++;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        //private void SetupInterestRates()
        //{
        //    try
        //    {
        //        if (_riskPrefs == null)
        //        {
        //            _riskPrefs = RiskPreferenceManager.RiskPrefernece;
        //        }
        //        dsIR = _riskPrefs.InterestRateTable;
        //        grdInterestRate.DataSource = null;
        //        grdInterestRate.DataSource = dsIR;

        //        UltraGridBand band = grdInterestRate.DisplayLayout.Bands[0];
        //        band.Columns["Period"].Header.Caption = "Period(in Months)";
        //        band.Columns["Rate"].Header.Caption = "Rate(%)";
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private void btnIntRateSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataSet dsInterestRate = (DataSet)grdInterestRate.DataSource;
        //        if (dsInterestRate.Tables.Count > 0)
        //        {
        //            DataTable dtInterestRate = dsInterestRate.Tables[0];
        //            foreach (DataRow row in dtInterestRate.Rows)
        //            {
        //                if (row["Period"].ToString().Equals(""))
        //                {
        //                    MessageBox.Show("Period cannot be blank, Please enter a valid value", "Interest Rates", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    return;
        //                }
        //                if (row["Rate"].ToString().Equals(""))
        //                {
        //                    MessageBox.Show("Rate cannot be blank, Please enter a valid value", "Interest Rates", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    return;
        //                }
        //            }
        //            RiskPreferenceManager.SaveInterestRatesToDB(dtInterestRate);
        //        }
        //        RiskPreferenceManager.RefreshInterestRate();
        //        _pricingAnalysis.SendRequest(OptionDataFormatter.MSGTYPE_PREFS_REFRESH);
        //        toolStripLabelIR.Text = DateTime.Now.ToString() + ": Interest Rates Updated.";
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void grdOptionModel_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                SetupRowLayoutForOMIGrid();
                grdOptionModel.CreationFilter = headerCheckBox;
                headerCheckBox.checkDisabledBoxes = false;
                // Kuldeep A.: Moved this method here as it was causing CheckBoxAll_Checked method to invoke and in that method newly added columns which were not there in grid due to old preferences
                // were being accessed and it was causing error.
                SetUPPreferences();
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

        private void SetupRowLayoutForOMIGrid()
        {
            try
            {
                UltraGridGroup ugGroupHidden;
                UltraGridGroup ugGroupSymbolDescription;
                UltraGridGroup ugGroupVolatility;
                UltraGridGroup ugGroupInterestRate;
                UltraGridGroup ugGroupDividend;
                UltraGridGroup ugGroupDelta;
                UltraGridGroup ugGroupPrice;
                UltraGridGroup ugGroupSharesOutstanding;
                UltraGridGroup ugGroupUseTheoreticalPrice;
                UltraGridGroup ugGroupUseClosingMark;
                UltraGridGroup ugGroupUseManualInput;
                foreach (UltraGridBand band in grdOptionModel.DisplayLayout.Bands)
                {
                    // Kuldeep A.: Here disabling the cells Delta and DeltaUsed for asset classes other than options as delta for them will always be one,
                    // so we are not permitting any change from PI.               
                    // EquityOption(2), FutureOption(4), FXOption(10) and ConvertibleBond(13)
                    foreach (UltraGridRow row in band.GetRowEnumerator(GridRowType.DataRow))
                    {
                        if ((AssetCategory)Int32.Parse(row.Cells["AssetID"].Value.ToString()) != AssetCategory.EquityOption && (AssetCategory)Int32.Parse(row.Cells["AssetID"].Value.ToString()) != AssetCategory.FutureOption && (AssetCategory)Int32.Parse(row.Cells["AssetID"].Value.ToString()) != AssetCategory.FXOption)
                        {
                            // Delta fields should be editable for Convertible Bonds but not Theoretical Prices.
                            if ((AssetCategory)Int32.Parse(row.Cells["AssetID"].Value.ToString()) != AssetCategory.ConvertibleBond)
                            {
                                row.Cells["Delta"].Activation = Activation.NoEdit;
                                row.Cells["DeltaUsed"].Activation = Activation.NoEdit;
                            }
                            row.Cells["TheoreticalPriceUsed"].Activation = Activation.NoEdit;
                            row.Cells["ManualInput"].Activation = Activation.NoEdit;
                        }
                    }

                    #region Groups
                    if (!band.Groups.Exists("Hidden"))
                    {
                        ugGroupHidden = band.Groups.Add("Hidden");
                        ugGroupHidden.Header.Caption = "";
                    }
                    else
                    {
                        ugGroupHidden = band.Groups["Hidden"];
                        ugGroupHidden.Header.Caption = "";
                    }

                    if (!band.Groups.Exists("SymbolInfo"))
                    {
                        ugGroupSymbolDescription = band.Groups.Add("SymbolInfo");
                        ugGroupSymbolDescription.Header.Caption = "Symbol Description";
                    }
                    else
                    {
                        ugGroupSymbolDescription = band.Groups["SymbolInfo"];
                        ugGroupSymbolDescription.Header.Caption = "Symbol Description";
                    }

                    if (!band.Groups.Exists("VolatilityHeader"))
                    {
                        ugGroupVolatility = band.Groups.Add("VolatilityHeader");
                        ugGroupVolatility.Header.Caption = "Volatility(%)";
                    }
                    else
                    {
                        ugGroupVolatility = band.Groups["VolatilityHeader"];
                        ugGroupVolatility.Header.Caption = "Volatility(%)";
                    }

                    if (!band.Groups.Exists("IRHeader"))
                    {
                        ugGroupInterestRate = band.Groups.Add("IRHeader");
                        ugGroupInterestRate.Header.Caption = "Interest Rate(%)";
                    }
                    else
                    {
                        ugGroupInterestRate = band.Groups["IRHeader"];
                        ugGroupInterestRate.Header.Caption = "Interest Rate(%)";
                    }

                    if (!band.Groups.Exists("DivHeader"))
                    {
                        ugGroupDividend = band.Groups.Add("DivHeader");
                        ugGroupDividend.Header.Caption = "Dividend Yield(%)";
                    }
                    else
                    {
                        ugGroupDividend = band.Groups["DivHeader"];
                        ugGroupDividend.Header.Caption = "Dividend Yield(%)";
                    }

                    if (!band.Groups.Exists("DeltaHeader"))
                    {
                        ugGroupDelta = band.Groups.Add("DeltaHeader");
                        ugGroupDelta.Header.Caption = "Delta";
                    }
                    else
                    {
                        ugGroupDelta = band.Groups["DeltaHeader"];
                        ugGroupDelta.Header.Caption = "Delta";
                    }

                    if (!band.Groups.Exists("LastPxHeader"))
                    {
                        ugGroupPrice = band.Groups.Add("LastPxHeader");
                        ugGroupPrice.Header.Caption = "Selected Px";
                    }
                    else
                    {
                        ugGroupPrice = band.Groups["LastPxHeader"];
                        ugGroupPrice.Header.Caption = "Selected Px";
                    }

                    if (!band.Groups.Exists("SharesoutStandingHeader"))
                    {
                        ugGroupSharesOutstanding = band.Groups.Add("SharesoutStandingHeader");
                        ugGroupSharesOutstanding.Header.Caption = "Shares Outstanding";
                    }
                    else
                    {
                        ugGroupSharesOutstanding = band.Groups["SharesoutStandingHeader"];
                        ugGroupSharesOutstanding.Header.Caption = "Shares Outstanding";
                    }

                    if (!band.Groups.Exists("TheoreticalPxHeader"))
                    {
                        ugGroupUseTheoreticalPrice = band.Groups.Add("TheoreticalPxHeader");
                        ugGroupUseTheoreticalPrice.Header.Caption = "Use Theoretical Price";
                    }
                    else
                    {
                        ugGroupUseTheoreticalPrice = band.Groups["TheoreticalPxHeader"];
                        ugGroupUseTheoreticalPrice.Header.Caption = "Use Theoretical Price";
                    }

                    if (!band.Groups.Exists("ClosingMarkHeader"))
                    {
                        ugGroupUseClosingMark = band.Groups.Add("ClosingMarkHeader");
                        ugGroupUseClosingMark.Header.Caption = "Use Closing Mark";
                    }
                    else
                    {
                        ugGroupUseClosingMark = band.Groups["ClosingMarkHeader"];
                        ugGroupUseClosingMark.Header.Caption = "Use Closing Mark";
                    }

                    if (!band.Groups.Exists("ManualInputHeader"))
                    {
                        ugGroupUseManualInput = band.Groups.Add("ManualInputHeader");
                        ugGroupUseManualInput.Header.Caption = "Manual Input";
                    }
                    else
                    {
                        ugGroupUseManualInput = band.Groups["ManualInputHeader"];
                        ugGroupUseManualInput.Header.Caption = "Manual Input";
                    }
                    #endregion

                    #region HiddenColumns
                    band.Columns["AssetID"].Hidden = true;
                    band.Columns["AssetID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["AssetID"].Group = ugGroupHidden;

                    band.Columns["AuecID"].Hidden = true;
                    band.Columns["AuecID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["AuecID"].Group = ugGroupHidden;


                    band.Columns["ExpirationDate"].Hidden = true;
                    band.Columns["ExpirationDate"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ExpirationDate"].Group = ugGroupHidden;


                    band.Columns["StrikePrice"].Hidden = true;
                    band.Columns["StrikePrice"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["StrikePrice"].Group = ugGroupHidden;


                    band.Columns["PutorCall"].Hidden = true;
                    band.Columns["PutorCall"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["PutorCall"].Group = ugGroupHidden;


                    band.Columns["LeadCurrencyID"].Hidden = true;
                    band.Columns["LeadCurrencyID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["LeadCurrencyID"].Group = ugGroupHidden;


                    band.Columns["VsCurrencyID"].Hidden = true;
                    band.Columns["VsCurrencyID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["VsCurrencyID"].Group = ugGroupHidden;


                    band.Columns["PBSymbol"].Hidden = true;
                    band.Columns["PBSymbol"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["PBSymbol"].Group = ugGroupHidden;



                    band.Columns["IsHistorical"].Hidden = true;
                    band.Columns["IsHistorical"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["IsHistorical"].Group = ugGroupHidden;
                    #endregion

                    #region SymbolDescriptionColumns
                    band.Columns["Symbol"].Header.Caption = "Symbol";
                    band.Columns["Symbol"].CellActivation = Activation.NoEdit;
                    band.Columns["Symbol"].CellAppearance.TextHAlign = HAlign.Left;
                    band.Columns["Symbol"].Header.VisiblePosition = 1;
                    band.Columns["Symbol"].Header.Fixed = true;
                    band.Columns["Symbol"].Group = ugGroupSymbolDescription;
                    band.Columns["Symbol"].Width = 130;

                    if (!band.Columns.Exists("AssetClass"))
                        band.Columns.Add("AssetClass");

                    band.Columns["AssetClass"].Header.Caption = "Asset Class";
                    band.Columns["AssetClass"].SortIndicator = SortIndicator.Disabled;
                    band.Columns["AssetClass"].CellAppearance.TextHAlign = HAlign.Left;
                    band.Columns["AssetClass"].CellActivation = Activation.NoEdit;  // Prevent the user from being able to tab into cells
                    band.Columns["AssetClass"].Header.Appearance.TextHAlign = HAlign.Center; // Center the text in the column header.
                    band.Columns["AssetClass"].AllowRowFiltering = DefaultableBoolean.True;
                    band.Columns["AssetClass"].Header.VisiblePosition = 2;
                    band.Columns["AssetClass"].Header.Fixed = true;
                    band.Columns["AssetClass"].Group = ugGroupSymbolDescription;
                    band.Columns["AssetClass"].Width = 130;

                    band.Columns["UnderlyingSymbol"].Header.Caption = "Underlying Symbol";
                    band.Columns["UnderlyingSymbol"].CellActivation = Activation.NoEdit;
                    band.Columns["UnderlyingSymbol"].CellAppearance.TextHAlign = HAlign.Left;
                    band.Columns["UnderlyingSymbol"].Header.VisiblePosition = 3;
                    band.Columns["UnderlyingSymbol"].Group = ugGroupSymbolDescription;
                    band.Columns["UnderlyingSymbol"].Width = 130;

                    band.Columns["SecurityDescription"].Header.Caption = "Security Description";
                    band.Columns["SecurityDescription"].CellActivation = Activation.NoEdit;
                    band.Columns["SecurityDescription"].CellAppearance.TextHAlign = HAlign.Left;
                    band.Columns["SecurityDescription"].Header.VisiblePosition = 4;
                    band.Columns["SecurityDescription"].Header.Fixed = true;
                    band.Columns["SecurityDescription"].Group = ugGroupSymbolDescription;
                    band.Columns["SecurityDescription"].Width = 130;

                    band.Columns["Bloomberg"].Header.Caption = "Bloomberg Symbol";
                    band.Columns["Bloomberg"].CellActivation = Activation.NoEdit;
                    band.Columns["Bloomberg"].CellAppearance.TextHAlign = HAlign.Left;
                    band.Columns["Bloomberg"].Header.VisiblePosition = 5;
                    band.Columns["Bloomberg"].Header.Fixed = true;
                    band.Columns["Bloomberg"].Group = ugGroupSymbolDescription;
                    band.Columns["Bloomberg"].Width = 130;

                    band.Columns["OSIOptionSymbol"].Header.Caption = "OSI Symbol";
                    band.Columns["OSIOptionSymbol"].CellActivation = Activation.NoEdit;
                    band.Columns["OSIOptionSymbol"].CellAppearance.TextHAlign = HAlign.Left;
                    band.Columns["OSIOptionSymbol"].Header.VisiblePosition = 6;
                    band.Columns["OSIOptionSymbol"].Header.Fixed = true;
                    band.Columns["OSIOptionSymbol"].Group = ugGroupSymbolDescription;
                    band.Columns["OSIOptionSymbol"].Width = 130;

                    band.Columns["IDCOOptionSymbol"].Header.Caption = "IDCO Symbol";
                    band.Columns["IDCOOptionSymbol"].CellActivation = Activation.NoEdit;
                    band.Columns["IDCOOptionSymbol"].CellAppearance.TextHAlign = HAlign.Left;
                    band.Columns["IDCOOptionSymbol"].Header.VisiblePosition = 7;
                    band.Columns["IDCOOptionSymbol"].Header.Fixed = true;
                    band.Columns["IDCOOptionSymbol"].Group = ugGroupSymbolDescription;
                    band.Columns["IDCOOptionSymbol"].Width = 130;


                    band.Columns["PSSymbol"].Header.Caption = "PS Symbol";
                    band.Columns["PSSymbol"].CellActivation = Activation.NoEdit;
                    band.Columns["PSSymbol"].CellAppearance.TextHAlign = HAlign.Left;
                    band.Columns["PSSymbol"].Hidden = false;
                    band.Columns["PSSymbol"].Header.VisiblePosition = 8;
                    band.Columns["PSSymbol"].Header.Fixed = true;
                    band.Columns["PSSymbol"].Group = ugGroupSymbolDescription;
                    band.Columns["PSSymbol"].Width = 130;

                    band.Columns["ProxySymbolUsed"].Header.Caption = "";
                    band.Columns["ProxySymbolUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["ProxySymbolUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["ProxySymbolUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ProxySymbolUsed"].Header.VisiblePosition = 9;
                    band.Columns["ProxySymbolUsed"].Header.Fixed = true;
                    band.Columns["ProxySymbolUsed"].Group = ugGroupSymbolDescription;
                    band.Columns["ProxySymbolUsed"].Width = 20;

                    band.Columns["ProxySymbol"].Header.Caption = "Proxy Symbol";
                    band.Columns["ProxySymbol"].CellActivation = Activation.NoEdit;
                    band.Columns["ProxySymbol"].CellAppearance.TextHAlign = HAlign.Left;
                    band.Columns["ProxySymbol"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    band.Columns["ProxySymbol"].Header.VisiblePosition = 10;
                    band.Columns["ProxySymbol"].Header.Fixed = true;
                    band.Columns["ProxySymbol"].Group = ugGroupSymbolDescription;
                    band.Columns["ProxySymbol"].Width = 130;

                    band.Columns[const_BloombergSymbolExCode].Header.Caption = const_Header_BloombergSymbolExCode;
                    band.Columns[const_BloombergSymbolExCode].CellActivation = Activation.NoEdit;
                    band.Columns[const_BloombergSymbolExCode].CellAppearance.TextHAlign = HAlign.Left;
                    band.Columns[const_BloombergSymbolExCode].Header.VisiblePosition = 5;
                    band.Columns[const_BloombergSymbolExCode].Header.Fixed = true;
                    band.Columns[const_BloombergSymbolExCode].Group = ugGroupSymbolDescription;
                    band.Columns[const_BloombergSymbolExCode].Width = 130;
                    band.Columns[const_BloombergSymbolExCode].Hidden = true;
                    #endregion

                    #region VolatilityColumns
                    band.Columns["ImpliedVol"].Header.Caption = "Volatility (%)";
                    band.Columns["ImpliedVol"].Format = "#,#.00";
                    band.Columns["ImpliedVol"].CellActivation = Activation.NoEdit;
                    band.Columns["ImpliedVol"].Header.Appearance.TextHAlign = HAlign.Center;
                    band.Columns["ImpliedVol"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ImpliedVol"].Header.VisiblePosition = 11;
                    band.Columns["ImpliedVol"].Header.Fixed = true;
                    band.Columns["ImpliedVol"].Group = ugGroupVolatility;
                    band.Columns["ImpliedVol"].Width = 130;

                    band.Columns["HistoricalVolUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["HistoricalVolUsed"].Header.Caption = "";
                    band.Columns["HistoricalVolUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["HistoricalVolUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["HistoricalVolUsed"].Header.VisiblePosition = 12;
                    band.Columns["HistoricalVolUsed"].Header.Fixed = true;
                    band.Columns["HistoricalVolUsed"].Group = ugGroupVolatility;
                    band.Columns["HistoricalVolUsed"].Width = 20;

                    band.Columns["HistoricalVol"].Header.Caption = "Historical Volatility";
                    band.Columns["HistoricalVol"].Format = "#,#.00";
                    band.Columns["HistoricalVol"].CellActivation = Activation.NoEdit;
                    band.Columns["HistoricalVol"].Header.Appearance.TextHAlign = HAlign.Center;
                    band.Columns["HistoricalVol"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["HistoricalVol"].Header.VisiblePosition = 13;
                    band.Columns["HistoricalVol"].Header.Fixed = true;
                    band.Columns["HistoricalVol"].Group = ugGroupVolatility;
                    band.Columns["HistoricalVol"].Width = 130;

                    band.Columns["VolatilityUsed"].Header.Caption = "";
                    band.Columns["VolatilityUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["VolatilityUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["VolatilityUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["VolatilityUsed"].Header.VisiblePosition = 14;
                    band.Columns["VolatilityUsed"].Header.Fixed = true;
                    band.Columns["VolatilityUsed"].Group = ugGroupVolatility;
                    band.Columns["VolatilityUsed"].Width = 20;

                    band.Columns["Volatility"].Header.Caption = "User Volatility";
                    band.Columns["Volatility"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["Volatility"].Header.VisiblePosition = 15;
                    band.Columns["Volatility"].Header.Fixed = true;
                    band.Columns["Volatility"].Group = ugGroupVolatility;
                    band.Columns["Volatility"].Width = 130;
                    #endregion

                    #region InterestRateColumns
                    band.Columns["InterestRate"].Header.Caption = "Interest Rate";
                    band.Columns["InterestRate"].Format = "#,#.00";
                    band.Columns["InterestRate"].CellActivation = Activation.NoEdit;
                    band.Columns["InterestRate"].Header.Appearance.TextHAlign = HAlign.Center;
                    band.Columns["InterestRate"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["InterestRate"].Header.VisiblePosition = 16;
                    band.Columns["InterestRate"].Header.Fixed = true;
                    band.Columns["InterestRate"].Group = ugGroupInterestRate;
                    band.Columns["InterestRate"].Width = 130;

                    band.Columns["IntRateUsed"].Header.Caption = "";
                    band.Columns["IntRateUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["IntRateUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["IntRateUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["IntRateUsed"].Header.VisiblePosition = 17;
                    band.Columns["IntRateUsed"].Header.Fixed = true;
                    band.Columns["IntRateUsed"].Group = ugGroupInterestRate;
                    band.Columns["IntRateUsed"].Width = 20;

                    band.Columns["IntRate"].Header.Caption = "User Interest Rate";
                    band.Columns["IntRate"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["IntRate"].Header.VisiblePosition = 18;
                    band.Columns["IntRate"].Header.Fixed = true;
                    band.Columns["IntRate"].Group = ugGroupInterestRate;
                    band.Columns["IntRate"].Width = 130;
                    #endregion

                    #region DividendColumns
                    band.Columns["ActualDividend"].Header.Caption = "Dividend Yield";
                    band.Columns["ActualDividend"].Format = "#,#.0000";
                    band.Columns["ActualDividend"].CellActivation = Activation.NoEdit;
                    band.Columns["ActualDividend"].Header.Appearance.TextHAlign = HAlign.Center;
                    band.Columns["ActualDividend"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ActualDividend"].Header.VisiblePosition = 19;
                    band.Columns["ActualDividend"].Header.Fixed = true;
                    band.Columns["ActualDividend"].Group = ugGroupDividend;
                    band.Columns["ActualDividend"].Width = 130;

                    band.Columns["DividendUsed"].Header.Caption = "";
                    band.Columns["DividendUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["DividendUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["DividendUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["DividendUsed"].Header.VisiblePosition = 20;
                    band.Columns["DividendUsed"].Header.Fixed = true;
                    band.Columns["DividendUsed"].Group = ugGroupDividend;
                    band.Columns["DividendUsed"].Width = 20;

                    band.Columns["Dividend"].Header.Caption = "User Dividend Yield";
                    band.Columns["Dividend"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["Dividend"].Header.VisiblePosition = 21;
                    band.Columns["Dividend"].Header.Fixed = true;
                    band.Columns["Dividend"].Group = ugGroupDividend;
                    band.Columns["Dividend"].Width = 130;

                    band.Columns["StockBorrowCostUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["StockBorrowCostUsed"].Header.Caption = "";
                    band.Columns["StockBorrowCostUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["StockBorrowCostUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["StockBorrowCostUsed"].Header.VisiblePosition = 22;
                    band.Columns["StockBorrowCostUsed"].Header.Fixed = true;
                    band.Columns["StockBorrowCostUsed"].Group = ugGroupDividend;
                    band.Columns["StockBorrowCostUsed"].Width = 20;

                    band.Columns["StockBorrowCost"].Header.Caption = "Stock Borrow Cost";
                    band.Columns["StockBorrowCost"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["StockBorrowCost"].Header.VisiblePosition = 23;
                    band.Columns["StockBorrowCost"].Header.Fixed = true;
                    band.Columns["StockBorrowCost"].Group = ugGroupDividend;
                    band.Columns["StockBorrowCost"].Width = 130;
                    #endregion

                    #region DeltaColumns
                    band.Columns["ActualDelta"].Format = "#,#.0000";
                    band.Columns["ActualDelta"].Header.Caption = "Actual Delta";
                    band.Columns["ActualDelta"].CellActivation = Activation.NoEdit;
                    band.Columns["ActualDelta"].Header.Appearance.TextHAlign = HAlign.Center;
                    band.Columns["ActualDelta"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ActualDelta"].Header.VisiblePosition = 24;
                    band.Columns["ActualDelta"].Header.Fixed = true;
                    band.Columns["ActualDelta"].Group = ugGroupDelta;
                    band.Columns["ActualDelta"].Width = 130;

                    band.Columns["DeltaUsed"].Header.Caption = "";
                    band.Columns["DeltaUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["DeltaUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["DeltaUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["DeltaUsed"].Header.VisiblePosition = 25;
                    band.Columns["DeltaUsed"].Header.Fixed = true;
                    band.Columns["DeltaUsed"].Group = ugGroupDelta;
                    band.Columns["DeltaUsed"].Width = 20;

                    band.Columns["Delta"].Header.Caption = "User Delta";
                    band.Columns["Delta"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["Delta"].Header.VisiblePosition = 26;
                    band.Columns["Delta"].Header.Fixed = true;
                    band.Columns["Delta"].Group = ugGroupDelta;
                    band.Columns["Delta"].Width = 130;
                    #endregion

                    #region PriceColumns
                    band.Columns["LastPx"].Header.Caption = "Selected Px";
                    band.Columns["LastPx"].Format = "#,#.00";
                    band.Columns["LastPx"].CellActivation = Activation.NoEdit;
                    band.Columns["LastPx"].Header.Appearance.TextHAlign = HAlign.Center;
                    band.Columns["LastPx"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["LastPx"].Header.VisiblePosition = 27;
                    band.Columns["LastPx"].Header.Fixed = true;
                    band.Columns["LastPx"].Group = ugGroupPrice;
                    band.Columns["LastPx"].Width = 130;

                    band.Columns["LastPriceUsed"].Header.Caption = "";
                    band.Columns["LastPriceUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["LastPriceUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["LastPriceUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["LastPriceUsed"].Header.VisiblePosition = 28;
                    band.Columns["LastPriceUsed"].Header.Fixed = true;
                    band.Columns["LastPriceUsed"].Group = ugGroupPrice;
                    band.Columns["LastPriceUsed"].Width = 20;

                    band.Columns["LastPrice"].Header.Caption = "User Px";
                    band.Columns["LastPrice"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["LastPrice"].Header.VisiblePosition = 29;
                    band.Columns["LastPrice"].Header.Fixed = true;
                    band.Columns["LastPrice"].Group = ugGroupPrice;
                    band.Columns["LastPrice"].Width = 130;

                    band.Columns["ForwardPointsUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["ForwardPointsUsed"].Header.Caption = "";
                    band.Columns["ForwardPointsUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["ForwardPointsUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ForwardPointsUsed"].Header.VisiblePosition = 30;
                    band.Columns["ForwardPointsUsed"].Header.Fixed = true;
                    band.Columns["ForwardPointsUsed"].Group = ugGroupPrice;
                    band.Columns["ForwardPointsUsed"].Width = 20;

                    band.Columns["ForwardPoints"].Header.Caption = "Forward Points";
                    band.Columns["ForwardPoints"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ForwardPoints"].Header.VisiblePosition = 31;
                    band.Columns["ForwardPoints"].Header.Fixed = true;
                    band.Columns["ForwardPoints"].Group = ugGroupPrice;
                    band.Columns["ForwardPoints"].Width = 130;
                    #endregion

                    #region SharesOutstandingColumns
                    band.Columns["ActualSharesOutStanding"].Header.Caption = "Shares Outstanding";
                    band.Columns["ActualSharesOutStanding"].Format = "#,#.00";
                    band.Columns["ActualSharesOutStanding"].CellActivation = Activation.NoEdit;
                    band.Columns["ActualSharesOutStanding"].Header.Appearance.TextHAlign = HAlign.Center;
                    band.Columns["ActualSharesOutStanding"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ActualSharesOutStanding"].Header.VisiblePosition = 32;
                    band.Columns["ActualSharesOutStanding"].Header.Fixed = true;
                    band.Columns["ActualSharesOutStanding"].Group = ugGroupSharesOutstanding;
                    band.Columns["ActualSharesOutStanding"].Width = 130;

                    band.Columns["SMSharesOutstandingUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["SMSharesOutstandingUsed"].Header.Caption = "";
                    band.Columns["SMSharesOutstandingUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["SMSharesOutstandingUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["SMSharesOutstandingUsed"].Header.VisiblePosition = 33;
                    band.Columns["SMSharesOutstandingUsed"].Header.Fixed = true;
                    band.Columns["SMSharesOutstandingUsed"].Group = ugGroupSharesOutstanding;
                    band.Columns["SMSharesOutstandingUsed"].Width = 20;

                    band.Columns["SMSharesOutstanding"].Header.Caption = "SM Shares Outstanding";
                    band.Columns["SMSharesOutstanding"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["SMSharesOutstanding"].CellActivation = Activation.NoEdit;
                    band.Columns["SMSharesOutstanding"].Header.VisiblePosition = 34;
                    band.Columns["SMSharesOutstanding"].Header.Fixed = true;
                    band.Columns["SMSharesOutstanding"].Group = ugGroupSharesOutstanding;
                    band.Columns["SMSharesOutstanding"].Width = 130;

                    band.Columns["SharesOutstandingUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["SharesOutstandingUsed"].Header.Caption = "";
                    band.Columns["SharesOutstandingUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["SharesOutstandingUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["SharesOutstandingUsed"].Header.VisiblePosition = 35;
                    band.Columns["SharesOutstandingUsed"].Header.Fixed = true;
                    band.Columns["SharesOutstandingUsed"].Group = ugGroupSharesOutstanding;
                    band.Columns["SharesOutstandingUsed"].Width = 20;

                    band.Columns["SharesOutstanding"].Header.Caption = "User Shares Outstanding";
                    band.Columns["SharesOutstanding"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["SharesOutstanding"].Header.VisiblePosition = 36;
                    band.Columns["SharesOutstanding"].Header.Fixed = true;
                    band.Columns["SharesOutstanding"].Group = ugGroupSharesOutstanding;
                    band.Columns["SharesOutstanding"].Width = 130;
                    #endregion

                    #region TheoreticalPriceColumn
                    band.Columns["TheoreticalPriceUsed"].Header.Caption = "";
                    band.Columns["TheoreticalPriceUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["TheoreticalPriceUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["TheoreticalPriceUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["TheoreticalPriceUsed"].Header.VisiblePosition = 37;
                    band.Columns["TheoreticalPriceUsed"].Header.Fixed = true;
                    band.Columns["TheoreticalPriceUsed"].Group = ugGroupUseTheoreticalPrice;
                    band.Columns["TheoreticalPriceUsed"].Width = 130;
                    #endregion

                    #region ClosingMarkColumn
                    band.Columns["ClosingMarkUsed"].Header.Caption = "";
                    band.Columns["ClosingMarkUsed"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["ClosingMarkUsed"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["ClosingMarkUsed"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ClosingMarkUsed"].Header.VisiblePosition = 38;
                    band.Columns["ClosingMarkUsed"].Header.Fixed = true;
                    band.Columns["ClosingMarkUsed"].Group = ugGroupUseClosingMark;
                    band.Columns["ClosingMarkUsed"].Width = 130;
                    #endregion

                    #region ManualInputColumn
                    band.Columns["ManualInput"].Header.Caption = "";
                    band.Columns["ManualInput"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    band.Columns["ManualInput"].AllowRowFiltering = DefaultableBoolean.False;
                    band.Columns["ManualInput"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns["ManualInput"].Header.VisiblePosition = 39;
                    band.Columns["ManualInput"].Header.Fixed = true;
                    band.Columns["ManualInput"].Group = ugGroupUseManualInput;
                    band.Columns["ManualInput"].Width = 130;
                    band.Columns["ManualInput"].Header.Enabled = false;
                    #endregion

                    ugGroupHidden.Hidden = true;

                    ugGroupSymbolDescription.Header.Fixed = true;
                    ugGroupSymbolDescription.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ugGroupVolatility.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ugGroupInterestRate.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ugGroupDividend.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ugGroupDelta.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ugGroupPrice.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ugGroupSharesOutstanding.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ugGroupUseTheoreticalPrice.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ugGroupUseClosingMark.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                    ugGroupUseManualInput.Header.FixedHeaderIndicator = FixedHeaderIndicator.None;

                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        band.Columns["VolatilityUsed"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["Volatility"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["DividendUsed"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["IntRateUsed"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["IntRate"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["Dividend"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["DeltaUsed"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["Delta"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["LastPriceUsed"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["LastPrice"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["ForwardPointsUsed"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["ForwardPoints"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["StockBorrowCostUsed"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["StockBorrowCost"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["SharesOutstandingUsed"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["SharesOutstanding"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["SMSharesOutstandingUsed"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["SMSharesOutstanding"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["TheoreticalPriceUsed"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["ClosingMarkUsed"].CellAppearance.BackColor = Color.DimGray;
                        band.Columns["ManualInput"].CellAppearance.BackColor = Color.DimGray;
                    }
                }
                //For auto sizing column width with respect to column data
                //foreach (UltraGridColumn column in grdOptionModel.DisplayLayout.Bands[0].Columns)
                //{
                //    column.PerformAutoResize();
                //}

                grdOptionModel.DisplayLayout.Bands[0].Override.AllowColMoving = AllowColMoving.NotAllowed;
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

        private void UnwireEvents()
        {
            try
            {
                RunUploadManager.DataImport_OMI -= new EventHandler(RefreshData_externalSource);
                if (bgGetHistoricalvol != null)
                {
                    bgGetHistoricalvol.DoWork -= new DoWorkEventHandler(bgGetHistoricalvol_DoWork);
                    bgGetHistoricalvol.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(bgGetHistoricalvol_RunWorkerCompleted);
                }
                if (_bgSaveData != null)
                {
                    _bgSaveData.DoWork -= new DoWorkEventHandler(_bgSaveData_DoWork);
                    _bgSaveData.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_bgSaveData_RunWorkerCompleted);
                }
                if (this.LaunchSymbolLookup != null)
                {
                    this.LaunchSymbolLookup -= PI_LaunchSymbolLookup;
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

        private void PI_LaunchSymbolLookup(object sender, EventArgs e)
        {
            try
            {
            }
            catch
            {
            }
        }

        private void CreateProxies()
        {
            try
            {
                CreatePricingServiceProxy();
                CreatePositionManagementProxy();
                CreateRiskServiceProxy();
                CreateSubscriptionServicesProxy();
                CreateSubscriptionServicesProxyPricing();
                _prefManager.PricingServiceProxy = _pricingServiceProxy;
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

        #region IPluggableTools Members
        public void SetUP()
        {
            try
            {
                //_companyID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyID;
                RunUploadManager.DataImport_OMI += new EventHandler(RefreshData_externalSource);
                //_userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                RiskPreferenceManager.SetUp(Application.StartupPath);
                PricingPreferenceManager.SetUp(Application.StartupPath);
                PricingLayoutManager.SetUp(Application.StartupPath);
                CreateProxies();
                SetupOptionModelInputs();
                btnRefreshLiveData_Click(null, null);
                //SetupInterestRates();
                if (!CustomThemeHelper.ApplyTheme)
                {
                    toolStripLabelOMI.ForeColor = Color.Red;

                }
                DisableForm(true);
                toolStripLabelOMI.Text = DateTime.Now.ToString() + ": Refreshing Live Data...";
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

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        public ISecurityMasterServices SecurityMaster
        {
            set {; }
        }

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set { _pricingAnalysis = value; }
        }

        private void OptionModelInputs_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, null);
                }
                //DisposeProxies();
                _prefManager = null;
                InstanceManager.ReleaseInstance(typeof(OptionModelInputs));
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


        //Modified By Kashish Goyal               
        //Date: 12 June 2015
        //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-8520
        // Commented as Proxies are already being diposed in Designer file.

        //private void DisposeProxies()
        //{
        //    try
        //    {
        //        if (_pricingServiceProxy != null)
        //        {
        //            _pricingServiceProxy.Dispose();
        //        }
        //        if (_riskServiceProxy != null)
        //        {
        //            _riskServiceProxy.Dispose();
        //        }
        //        if (_positionManagementServices != null)
        //        {
        //            _positionManagementServices.Dispose();
        //        }
        //        if (_proxy != null)
        //        {
        //            _proxy.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
        //            _proxy.Dispose();
        //        }
        //        if (_proxyPricing != null)
        //        {
        //            _proxyPricing.InnerChannel.UnSubscribe(Topics.Topic_OMIData);
        //            _proxyPricing.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        #endregion

        BackgroundWorker _bgSaveData = null;
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _scrollPos = this.grdOptionModel.ActiveRowScrollRegion.ScrollPosition;
                btnSave.Enabled = false;
                _bgSaveData = new BackgroundWorker();
                grdOptionModel.UpdateData();
                dtOMI = (DataTable)grdOptionModel.DataSource;
                AddOMIAuditEntry(dtOMI);
                dtOMI.AcceptChanges();

                _bgSaveData.DoWork += new DoWorkEventHandler(_bgSaveData_DoWork);
                _bgSaveData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgSaveData_RunWorkerCompleted);
                _bgSaveData.RunWorkerAsync(dtOMI);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                btnSave.Enabled = false;
                toolStripLabelOMI.Text = string.Empty;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// List of symbols for which OMI data changed.
        /// </summary>
        private List<string> modifiedSymbolsList = new List<string>();
        private void AddOMIAuditEntry(DataTable dataTable)
        {
            try
            {
                List<TradeAuditEntry> OMIauditlist = new List<TradeAuditEntry>();
                TradeAuditEntry OMIobj = new TradeAuditEntry();
                if (dataTable != null)
                {
                    DataTable ModifiedDataTable = dataTable.GetChanges(DataRowState.Modified);
                    if (ModifiedDataTable != null && ModifiedDataTable.Rows.Count > 0)
                    {
                        int noOfChangedRows = ModifiedDataTable.Rows.Count;
                        Dictionary<string, string> listofcolumn = new Dictionary<string, string>();
                        listofcolumn.Add("Volatility", "User Volatily Value Changed");
                        listofcolumn.Add("VolatilityUsed", "Volatilty Checkbox Changed");
                        listofcolumn.Add("IntRate", "User Interest Rate Value Changed");
                        listofcolumn.Add("IntRateUsed", "User Interest Rate Checkbox Changed");
                        listofcolumn.Add("Dividend", "User Dividend Value Changed");
                        listofcolumn.Add("DividendUsed", "User Dividend Checkbox Changed");
                        listofcolumn.Add("LastPrice", "User Last Price Value Changed");
                        listofcolumn.Add("LastPriceUsed", "User Last Price Checkbox Changed");
                        listofcolumn.Add("ForwardPoints", "Forward Points Value Changed");
                        listofcolumn.Add("ForwardPointsUsed", "Forward Points Checkbox Changed");
                        listofcolumn.Add("StockBorrowCost", "Stock Borrow Cost Value Changed");
                        listofcolumn.Add("StockBorrowCostUsed", "Stock Borrow Cost Checkbox Changed");
                        listofcolumn.Add("SharesOutstanding", "User Share Outstanding Value Changed");
                        listofcolumn.Add("SMSharesOutstanding", "SM User Share Outstanding Value Changed");
                        listofcolumn.Add("HistoricalVolUsed", "Historical Volatility Checkbox Changed");
                        listofcolumn.Add("DeltaUsed", "User Delta Checkbox Changed");
                        listofcolumn.Add("TheoreticalPriceUsed", "Theoretical Price Checkbox Changed");
                        listofcolumn.Add("ProxySymbolUsed", "User Proxy Symbol Checkbox Changed");
                        listofcolumn.Add("ClosingMarkUsed", "Closing Mark Checkbox Changed");
                        listofcolumn.Add("ManualInput", "Manual Input Checkbox Changed");

                        modifiedSymbolsList.Clear();
                        for (int i = 0; i < noOfChangedRows; i++)
                        {
                            if(ModifiedDataTable.Columns.Contains("Symbol") && ModifiedDataTable.Rows[i]["Symbol"] != null)
                                modifiedSymbolsList.Add(ModifiedDataTable.Rows[i]["Symbol"].ToString());

                            foreach (KeyValuePair<string, string> colName in listofcolumn)
                            {
                                if (!ModifiedDataTable.Rows[i][colName.Key, DataRowVersion.Original].ToString().Equals(ModifiedDataTable.Rows[i][colName.Key].ToString()))
                                {
                                    OMIobj = AuditEntry(int.MinValue.ToString(), ModifiedDataTable.Rows[i][colName.Key, DataRowVersion.Original].ToString(), TradeAuditActionType.ActionType.PricingInput, ModifiedDataTable.Rows[i]["Symbol"].ToString(), colName.Value.ToString(), CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                    OMIauditlist.Add(OMIobj);
                                }
                            }
                        }
                    }
                }
                if (OMIauditlist.Count > 0)
                {
                    AuditManager.Instance.SaveAuditList(OMIauditlist);
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

        private TradeAuditEntry AuditEntry(string groupid, string originalvalue, TradeAuditActionType.ActionType action, string symbol, string comment, int companyuserid)
        {
            TradeAuditEntry TradeAuditObj = null;
            try
            {
                TradeAuditObj = new TradeAuditEntry();
                TradeAuditObj.GroupID = groupid;
                TradeAuditObj.OriginalValue = originalvalue;
                TradeAuditObj.Action = action;
                TradeAuditObj.Symbol = symbol;
                TradeAuditObj.Comment = comment;
                TradeAuditObj.AUECLocalDate = DateTime.Now;
                TradeAuditObj.OriginalDate = DateTime.Now;
                TradeAuditObj.CompanyUserId = companyuserid;
                TradeAuditObj.Source = TradeAuditActionType.ActionSource.PricingInput;
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
            return TradeAuditObj;
        }

        void _bgSaveData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DataTable dtOMI = (DataTable)e.Argument;
                DataSet ds = new DataSet();
                ds.Tables.Add(dtOMI.Copy());
                isDataSaved = _pricingServiceProxy.InnerChannel.SaveOMIData(ds, modifiedSymbolsList);
                e.Result = isDataSaved;
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

        void _bgSaveData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    isDataSaved = (bool)e.Result;
                }
                if (isDataSaved.Equals(true))
                {
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripLabelOMI.ForeColor = Color.Green;

                    }
                    toolStripLabelOMI.Text = DateTime.Now.ToString() + ": Data Saved";
                    btnSave.Enabled = true;
                }
                if (uOSetSymbols.CheckedIndex == 3)
                {
                    FetchOMIDataFromDB(false);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                btnSave.Enabled = false;
                toolStripLabelOMI.Text = string.Empty;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        int _totalRequestedSymbols = 0;
        private void btnRefreshLiveData_Click(object sender, EventArgs e)
        {
            try
            {
                _responseReceivedCount = 0;
                //hashcode = this.GetHashCode();
                List<string> symbols = new List<string>();
                List<fxInfo> listFxSymbols = new List<fxInfo>();
                GetSymbolsForSnapshot(out symbols, out listFxSymbols);
                _totalRequestedSymbols = symbols.Count + listFxSymbols.Count;
                if (symbols.Count == 0 && listFxSymbols.Count == 0)
                {
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripLabelOMI.ForeColor = Color.Red;

                    }
                    toolStripLabelOMI.Text = DateTime.Now.ToString() + ": No Data Available...";
                }
                else
                {
                    if (symbols.Count > 0)
                    {
                        _pricingServiceProxy.InnerChannel.RequestSnapshot(symbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, null, true);
                    }
                    if (listFxSymbols.Count > 0)
                    {
                        _pricingServiceProxy.InnerChannel.RequestSnapshot(listFxSymbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, null, true);
                    }

                    timerRefresh.Interval = 3000;
                    timerRefresh.Start();
                    if (sender != null)
                    {
                        DisableForm(true);
                        toolStripLabelOMI.Text = DateTime.Now.ToString() + ": Refreshing Live Data...";
                    }
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripLabelOMI.ForeColor = Color.Red;

                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
                UpdateToolStripStatus(ex.Message);
            }
        }

        private void EnableForm()
        {
            try
            {
                btnRefreshLiveData.Enabled = true;
                btnGetHistoricalVol.Enabled = true;
                if (CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && CommonDataCache.CachedDataManager.IsMarketDataBlocked)
                    btnExport.Enabled = false;
                else
                    btnExport.Enabled = true;
                btnHistoricalVolInputs.Enabled = true;
                btnRefresh.Enabled = true;
                btnSave.Enabled = true;
                if (!ModuleManager.CheckModulePermissioning(btnSymbolLookup.Text, btnSymbolLookup.Text))
                    btnSymbolLookup.Enabled = false;
                else
                    btnSymbolLookup.Enabled = true;
                uOSetSymbols.Enabled = true;
                grdOptionModel.Enabled = true;
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

        private void DisableForm(bool isRefreshingLivedata)
        {
            try
            {
                btnRefreshLiveData.Enabled = false;
                btnGetHistoricalVol.Enabled = false;
                btnExport.Enabled = false;
                btnHistoricalVolInputs.Enabled = false;
                btnRefresh.Enabled = false;
                btnSave.Enabled = false;
                btnSymbolLookup.Enabled = false;
                uOSetSymbols.Enabled = false;
                if (!isRefreshingLivedata)
                    grdOptionModel.Enabled = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        void bgGetHistoricalvol_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                PranaRequestCarrier pranaRequestCarrier = e.Argument as PranaRequestCarrier;
                pranaRequestCarrier = _riskServiceProxy.InnerChannel.CalculateHistoricalVol(pranaRequestCarrier);
                e.Result = pranaRequestCarrier;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                if (ex.Message.Equals("Risk Server not connected to Portfolio Science."))
                {
                    EnableUIOnHistoricalVolFailure();
                }
                else
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                    if (rethrow)
                    {
                        throw;
                    }
                    e.Cancel = true;
                }

            }
        }

        private void EnableUIOnHistoricalVolFailure()
        {
            MethodInvoker mi = new MethodInvoker(EnableUIOnHistoricalVolFailure);
            if (this.InvokeRequired)
            {
                Invoke(mi);
            }
            else
            {
                EnableForm();
                if (!CustomThemeHelper.ApplyTheme)
                {
                    toolStripLabelOMI.ForeColor = Color.Red;
                }
                toolStripLabelOMI.Text = DateTime.Now.ToString() + ": Risk server not connected to Portfolio Science.";
            }
        }

        private void UpdateToolStripStatus(string message)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        UIThreadMarshallerUpdateToolStrip marsheller = new UIThreadMarshallerUpdateToolStrip(UpdateToolStripStatus);
                        this.BeginInvoke(marsheller, new object[] { message });
                    }
                    else
                    {
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            toolStripLabelOMI.ForeColor = Color.Red;

                        }
                        toolStripLabelOMI.Text = DateTime.Now.ToString() + " : " + message;
                        btnRefreshLiveData.Enabled = true;
                    }
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

        public void RefreshData_externalSource(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Some external sources have changed the data. The data will be refreshed automatically", "Option Model Inputs", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result.ToString().Equals(DialogResult.OK.ToString()))
                {
                    this.btnRefresh_Click(null, null);
                }
                else
                {
                    return;
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

        /// <summary>
        /// Handles the Click event of the btnRefresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        MethodInvoker del =
                            delegate
                            {
                                btnRefresh_Click(sender, e);
                            };
                        this.BeginInvoke(del);
                        return;
                    }
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripLabelOMI.ForeColor = Color.Red;

                    }

                    if (uOSetSymbols.CheckedIndex != 3)
                        FetchOMIDataFromDB(true);
                    else
                        FetchOMIDataFromDB(false);
                    _responseReceivedCount = 0;
                    List<string> symbols = new List<string>();
                    List<fxInfo> listFxSymbols = new List<fxInfo>();
                    GetSymbolsForSnapshot(out symbols, out listFxSymbols);
                    _totalRequestedSymbols = symbols.Count + listFxSymbols.Count;
                    if (symbols.Count > 0)
                    {
                        _pricingServiceProxy.InnerChannel.RequestSnapshot(symbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, null, true);
                    }
                    if (listFxSymbols.Count > 0)
                    {
                        _pricingServiceProxy.InnerChannel.RequestSnapshot(listFxSymbols, ApplicationConstants.SymbologyCodes.TickerSymbol, true, null, true);
                    }
                    SetUPPreferences();

                    timerRefresh.Interval = 3000;
                    timerRefresh.Start();
                    if (sender != null)
                    {
                        DisableForm(true);
                        toolStripLabelOMI.Text = DateTime.Now.ToString() + ": Refreshing Data";
                    }
                    if (!CustomThemeHelper.ApplyTheme)
                    {
                        toolStripLabelOMI.ForeColor = Color.Green;
                    }
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

        private void btnSymbolLookup_Click(object sender, System.EventArgs e)
        {

            try
            {

                ListEventAargs args = new ListEventAargs();

                //creating a args dict for Symbol lookup UI 
                Dictionary<String, String> argDict = new Dictionary<string, string>();
                //SecMasterUIObj secMasterUI = new SecMasterUIObj();

                argDict.Add("SearchCriteria", SecMasterConstants.SearchCriteria.Ticker.ToString());
                argDict.Add("Symbol", string.Empty);

                argDict.Add("Action", "SEARCH");
                args.argsObject = argDict;

                if (LaunchSymbolLookup != null)
                {
                    LaunchSymbolLookup(this, args);
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

        public event EventHandler LaunchSymbolLookup;

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelAndPrintUtilities excelUtils = new ExcelAndPrintUtilities();
                excelUtils.ExportToExcel(grdOptionModel);
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

        private void GetSymbolsForSnapshot(out List<string> symbols, out List<fxInfo> listFxSymbols)
        {
            symbols = new List<string>();
            listFxSymbols = new List<fxInfo>();
            try
            {
                Dictionary<string, fxInfo> dictFxSymbols = new Dictionary<string, fxInfo>();
                foreach (DataRow dr in dtOMI.Rows)
                {
                    int AssetID = Convert.ToInt32(dr["AssetID"].ToString());
                    string symbol = dr["Symbol"].ToString();
                    string underlyingSymbol = dr["UnderlyingSymbol"].ToString();

                    if (AssetID == (int)AssetCategory.Future)
                    {
                        if (!String.IsNullOrEmpty(underlyingSymbol) && !symbols.Contains(underlyingSymbol)) // Future Underlying symbols
                        {
                            symbols.Add(underlyingSymbol);
                        }
                    }
                    if (AssetID == (int)AssetCategory.FX || AssetID == (int)AssetCategory.FXForward)
                    {
                        int leadCurrencyID = int.Parse(dr["LeadCurrencyID"].ToString());
                        int VsCurrencyID = int.Parse(dr["VsCurrencyID"].ToString());

                        if (!dictFxSymbols.ContainsKey(symbol))
                        {
                            fxInfo fxReqObj = new fxInfo();
                            fxReqObj.PranaSymbol = symbol;
                            fxReqObj.FromCurrencyID = leadCurrencyID;
                            fxReqObj.ToCurrencyID = VsCurrencyID;
                            fxReqObj.CategoryCode = (AssetCategory)AssetID;
                            dictFxSymbols.Add(symbol, fxReqObj);
                        }
                    }
                    else
                    {
                        if (!symbols.Contains(symbol))
                        {
                            symbols.Add(symbol);

                        }
                    }
                }

                listFxSymbols = new List<fxInfo>(dictFxSymbols.Values);
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

        private void GetInstance_HistoricalVolCalculated(PranaRequestCarrier pranaRequestCarrier)
        {
            try
            {
                // send to main UI thread
                UIThreadMarshaller mi = new UIThreadMarshaller(GetInstance_HistoricalVolCalculated);
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { pranaRequestCarrier });
                    }
                    else
                    {
                        Dictionary<string, PranaRiskResult> dictRiskresult = pranaRequestCarrier.IndividualSymbolList;
                        foreach (KeyValuePair<string, PranaRiskResult> keyVal in dictRiskresult)
                        {
                            PranaRiskResult riskResult = keyVal.Value;
                            DataRow foundRow = dtOMI.Rows.Find(riskResult.Symbol);
                            if (foundRow != null)
                            {
                                foundRow["HistoricalVol"] = riskResult.PSVolatility * 100;
                            }
                        }
                        EnableForm();
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            toolStripLabelOMI.ForeColor = Color.Green;

                        }
                        toolStripLabelOMI.Text = DateTime.Now.ToString() + ": HistoricalVol Calculated";
                    }
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

        //private void mnuAddRow_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        grdInterestRate.DisplayLayout.Bands[0].AddNew();
        //        grdInterestRate.Rows.AddRowModifiedByUser = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private void mnuDeleteRow_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (grdInterestRate.ActiveRow != null)
        //        {
        //            string period = grdInterestRate.ActiveRow.Cells["Period"].Value.ToString();
        //            if (period != String.Empty)
        //            {
        //                int ID = Convert.ToInt32(period);
        //                RiskPreferenceManager.DeleteInterestRateFromDB(ID);
        //            }
        //            grdInterestRate.ActiveRow.Delete(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                if (checkBoxAll.Checked)
                {
                    checkBoxDelta.Checked = true;
                    checkBoxDividend.Checked = true;
                    checkBoxIR.Checked = true;
                    checkBoxLastPrice.Checked = true;
                    checkBoxVolatility.Checked = true;
                    checkBoxTheoreticalPrice.Checked = true;
                    checkBoxSharesOutstanding.Checked = true;
                    checkBoxClosingMark.Checked = true;
                    checkBoxManualInput.Checked = true;

                    band.Groups["VolatilityHeader"].Hidden = false;
                    band.Groups["TheoreticalPxHeader"].Hidden = false;
                    band.Groups["ClosingMarkHeader"].Hidden = false;
                    band.Groups["ManualInputHeader"].Hidden = false;
                    band.Groups["SharesoutStandingHeader"].Hidden = false;
                    band.Groups["LastPxHeader"].Hidden = false;
                    band.Groups["IRHeader"].Hidden = false;
                    band.Groups["DivHeader"].Hidden = false;
                    band.Groups["DeltaHeader"].Hidden = false;
                }
                if (!checkBoxAll.Checked)
                {
                    checkBoxDelta.Checked = false;
                    checkBoxDividend.Checked = false;
                    checkBoxIR.Checked = false;
                    checkBoxLastPrice.Checked = false;
                    checkBoxVolatility.Checked = false;
                    checkBoxTheoreticalPrice.Checked = false;
                    checkBoxSharesOutstanding.Checked = false;
                    checkBoxClosingMark.Checked = false;
                    checkBoxManualInput.Checked = false;

                    band.Groups["VolatilityHeader"].Hidden = true;
                    band.Groups["TheoreticalPxHeader"].Hidden = true;
                    band.Groups["ClosingMarkHeader"].Hidden = true;
                    band.Groups["ManualInputHeader"].Hidden = true;
                    band.Groups["SharesoutStandingHeader"].Hidden = true;
                    band.Groups["LastPxHeader"].Hidden = true;
                    band.Groups["IRHeader"].Hidden = true;
                    band.Groups["DivHeader"].Hidden = true;
                    band.Groups["DeltaHeader"].Hidden = true;
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

        void checkBoxTheoreticalPrice_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                if (checkBoxTheoreticalPrice.Checked)
                {
                    band.Groups["TheoreticalPxHeader"].Hidden = false;
                }
                else
                {
                    band.Groups["TheoreticalPxHeader"].Hidden = true;
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

        private void checkBoxVolatility_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                if (checkBoxVolatility.Checked)
                {
                    band.Groups["VolatilityHeader"].Hidden = false;
                    if (checkBoxLastPrice.Checked && checkBoxDividend.Checked && checkBoxDelta.Checked && checkBoxIR.Checked && checkBoxTheoreticalPrice.Checked && checkBoxSharesOutstanding.Checked && checkBoxClosingMark.Checked && checkBoxManualInput.Checked)
                    {
                        checkBoxAll.Checked = true;
                    }
                }
                else
                {
                    band.Groups["VolatilityHeader"].Hidden = true;
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

        private void checkBoxIR_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                if (checkBoxIR.Checked)
                {
                    band.Groups["IRHeader"].Hidden = false;
                    if (checkBoxLastPrice.Checked && checkBoxDividend.Checked && checkBoxDelta.Checked && checkBoxVolatility.Checked && checkBoxTheoreticalPrice.Checked && checkBoxSharesOutstanding.Checked && checkBoxClosingMark.Checked && checkBoxManualInput.Checked)
                    {
                        checkBoxAll.Checked = true;
                    }
                }
                else
                {
                    band.Groups["IRHeader"].Hidden = true;
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

        private void checkBoxDividend_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                if (checkBoxDividend.Checked)
                {
                    band.Groups["DivHeader"].Hidden = false;
                    if (checkBoxLastPrice.Checked && checkBoxIR.Checked && checkBoxDelta.Checked && checkBoxVolatility.Checked && checkBoxTheoreticalPrice.Checked && checkBoxSharesOutstanding.Checked && checkBoxClosingMark.Checked && checkBoxManualInput.Checked)
                    {
                        checkBoxAll.Checked = true;
                    }
                }
                else
                {
                    band.Groups["DivHeader"].Hidden = true;
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

        private void checkBoxDelta_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                if (checkBoxDelta.Checked)
                {
                    band.Groups["DeltaHeader"].Hidden = false;
                    if (checkBoxDividend.Checked && checkBoxIR.Checked && checkBoxLastPrice.Checked && checkBoxVolatility.Checked && checkBoxSharesOutstanding.Checked && checkBoxClosingMark.Checked && checkBoxManualInput.Checked)
                    {
                        checkBoxAll.Checked = true;
                    }
                }
                else
                {
                    band.Groups["DeltaHeader"].Hidden = true;
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

        private void checkBoxLastPrice_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                if (checkBoxLastPrice.Checked)
                {
                    band.Groups["LastPxHeader"].Hidden = false;
                    if (checkBoxDividend.Checked && checkBoxIR.Checked && checkBoxDelta.Checked && checkBoxVolatility.Checked && checkBoxTheoreticalPrice.Checked && checkBoxSharesOutstanding.Checked && checkBoxClosingMark.Checked && checkBoxManualInput.Checked)
                    {
                        checkBoxAll.Checked = true;
                    }
                }
                else
                {
                    band.Groups["LastPxHeader"].Hidden = true;
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

        private void radioAllSymbols_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioAllSymbols.Checked)
                {
                    UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                    band.ColumnFilters["AssetID"].ClearFilterConditions();
                    foreach (UltraGridRow row in grdOptionModel.Rows)
                    {
                        if (row.Hidden)
                        {
                            row.Hidden = false;
                        }
                    }
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

        private void radioOptions_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioOptions.Checked)
                {
                    UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                    band.ColumnFilters.ClearAllFilters();
                    band.ColumnFilters["AssetID"].LogicalOperator = FilterLogicalOperator.Or;
                    band.ColumnFilters["AssetID"].FilterConditions.Add(FilterComparisionOperator.Equals, "2");
                    band.ColumnFilters["AssetID"].FilterConditions.Add(FilterComparisionOperator.Equals, "4");
                    band.ColumnFilters["AssetID"].FilterConditions.Add(FilterComparisionOperator.Equals, "10");
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

        private void radioOptionUnder_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioOptionUnder.Checked)
                {
                    List<string> OptionUnderlyings = new List<string>();
                    UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                    band.ColumnFilters.ClearAllFilters();
                    UltraGridColumn column = grdOptionModel.DisplayLayout.Bands[0].Columns["AssetID"];
                    foreach (UltraGridRow row in grdOptionModel.Rows)
                    {
                        if (row.Cells[column].Value.ToString().Equals("2") || row.Cells[column].Value.ToString().Equals("4") || row.Cells[column].Value.ToString().Equals("10"))
                        {
                            if (!OptionUnderlyings.Contains(row.Cells["UnderlyingSymbol"].Value.ToString()))
                                OptionUnderlyings.Add(row.Cells["UnderlyingSymbol"].Value.ToString());
                        }
                    }
                    foreach (UltraGridRow row in grdOptionModel.Rows)
                    {
                        if (!row.Cells[column].Value.ToString().Equals("2") && !row.Cells[column].Value.ToString().Equals("4") && !row.Cells[column].Value.ToString().Equals("10"))
                        {
                            string symbol = row.Cells["Symbol"].Value.ToString();
                            if (!OptionUnderlyings.Contains(symbol))
                                row.Hidden = true;

                        }
                    }
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

        private void grdOptionModel_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "HistoricalVolUsed")
                {
                    if (e.Cell.Text == "True")
                    {
                        e.Cell.Row.Cells["VolatilityUsed"].Value = false;
                    }
                }
                if (e.Cell.Column.Key == "VolatilityUsed")
                {
                    if (e.Cell.Text == "True")
                    {
                        e.Cell.Row.Cells["HistoricalVolUsed"].Value = false;
                    }
                }

                if (e.Cell.Column.Key == "SharesOutstandingUsed")
                {
                    if (e.Cell.Text == "True")
                    {
                        e.Cell.Row.Cells["SMSharesOutstandingUsed"].Value = false;
                    }
                }
                if (e.Cell.Column.Key == "SMSharesOutstandingUsed")
                {
                    if (e.Cell.Text == "True")
                    {
                        e.Cell.Row.Cells["SharesOutstandingUsed"].Value = false;
                    }
                }

                if (e.Cell.Column.Key == "LastPriceUsed")
                {
                    if (e.Cell.Text == "True")
                    {
                        e.Cell.Row.Cells["ProxySymbolUsed"].Value = false;
                        e.Cell.Row.Cells["TheoreticalPriceUsed"].Value = false;
                        e.Cell.Row.Cells["ForwardPointsUsed"].Value = false;
                    }
                }
                if (e.Cell.Column.Key == "ProxySymbolUsed")
                {
                    if (e.Cell.Text == "True")
                    {
                        e.Cell.Row.Cells["TheoreticalPriceUsed"].Value = false;
                        e.Cell.Row.Cells["LastPriceUsed"].Value = false;
                    }
                }
                if (e.Cell.Column.Key == "TheoreticalPriceUsed")
                {
                    if (e.Cell.Text == "True")
                    {
                        e.Cell.Row.Cells["ProxySymbolUsed"].Value = false;
                        e.Cell.Row.Cells["LastPriceUsed"].Value = false;
                        e.Cell.Row.Cells["ForwardPointsUsed"].Value = false;
                    }
                }
                if (e.Cell.Column.Key == "ForwardPointsUsed")
                {
                    if (e.Cell.Text == "True")
                    {
                        e.Cell.Row.Cells["LastPriceUsed"].Value = false;
                        e.Cell.Row.Cells["TheoreticalPriceUsed"].Value = false;
                    }
                }
                isDataSaved = false;
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

        private void CreateRequest(PranaRequestCarrier pranaRequestCarrier)
        {
            try
            {
                foreach (DataRow dr in dtOMI.Rows)
                {
                    if (!pranaRequestCarrier.IndividualSymbolList.ContainsKey(dr["PSSymbol"].ToString()))
                    {
                        PranaRiskResult riskResult = new PranaRiskResult();

                        pranaRequestCarrier.IndividualSymbolList.Add(dr["PSSymbol"].ToString(), riskResult);
                        riskResult.Symbol = dr["Symbol"].ToString();
                        riskResult.PSSymbol = dr["PSSymbol"].ToString();
                    }
                }
                pranaRequestCarrier.StartDate = HistoricalVolInputsUI.StartDate;
                pranaRequestCarrier.EndDate = HistoricalVolInputsUI.EndDate;
                pranaRequestCarrier.VolatilityType = HistoricalVolInputsUI.VolatilityType;
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

        HistoricalVolInputsUI historicalVolInputsUI;
        private void btnHistoricalVolInputs_Click(object sender, EventArgs e)
        {
            try
            {
                historicalVolInputsUI = new HistoricalVolInputsUI();
                historicalVolInputsUI.ShowDialog();
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

        //string OMIPrefFilePath = string.Empty;
        //string OMIPrefDirectoryPath = string.Empty;
        private void menuSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                SavePricingInputsPreferences();

                PricingLayoutManager.PricingLayout.PricingInputColumns = PricingLayoutManager.GetGridColumnLayout(grdOptionModel);
                PricingLayoutManager.SavePricingInputsLayout();
                toolStripLabelOMI.ForeColor = Color.Green;
                toolStripLabelOMI.Text = DateTime.Now.ToString() + ": Layout Saved";
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //static string _startPath = string.Empty;
        private void LoadPreferences()
        {
            try
            {
                _prefManager.GetPreferences();
                SetPIPreferences();
                SetUpLiveFeedPreferences();
                if (PricingLayoutManager.PricingLayout.PricingInputColumns.Count > 0)
                {
                    GetPricingInputsGridFromXML();
                }
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


        private void SetUPPreferences()
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                band.Groups["VolatilityHeader"].Hidden = !checkBoxVolatility.Checked;
                band.Groups["IRHeader"].Hidden = !checkBoxIR.Checked;
                band.Groups["DivHeader"].Hidden = !checkBoxDividend.Checked;
                band.Groups["DeltaHeader"].Hidden = !checkBoxDelta.Checked;
                band.Groups["LastPxHeader"].Hidden = !checkBoxLastPrice.Checked;
                band.Groups["SharesoutStandingHeader"].Hidden = !checkBoxSharesOutstanding.Checked;
                band.Groups["TheoreticalPxHeader"].Hidden = !checkBoxTheoreticalPrice.Checked;
                band.Groups["ClosingMarkHeader"].Hidden = !checkBoxClosingMark.Checked;
                band.Groups["ManualInputHeader"].Hidden = !checkBoxManualInput.Checked;

                //SetUpLiveFeedPreferences();

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

        private void SetUpLiveFeedPreferences()
        {
            try
            {
                cmbStockPrice.Value = Convert.ToInt32(_prefManager.OMIPreferences.SelectedFeedPrice);
                cmbOptPrice.Value = Convert.ToInt32(_prefManager.OMIPreferences.OptionSelectedFeedPrice);

                cmbBxOverrideWithOptions.Value = Convert.ToInt32(_prefManager.OMIPreferences.OverrideWithOptions);
                cmbBxOverrideWithOthers.Value = Convert.ToInt32(_prefManager.OMIPreferences.OverrideWithOthers);
                checkBoxUseDefaultDelta.Checked = _prefManager.OMIPreferences.UseDefaultDelta;

                uOsetPricingSource.CheckedIndex = _prefManager.OMIPreferences.UseClosingMark ? 1 : 0;
                //rBtnLiveData.Checked = (!_prefManager.OMIPreferences.UseClosingMark);

                priceBarOptions.Value = _prefManager.OMIPreferences.PriceBarOptions;
                priceBarOthers.Value = _prefManager.OMIPreferences.PriceBarOthers;

                cmbBxOverrideConditionOptions.Value = Convert.ToInt32(_prefManager.OMIPreferences.OverrideConditionOptions);
                cmbBxOverrideConditionOthers.Value = Convert.ToInt32(_prefManager.OMIPreferences.OverrideConditionOthers);

                cmbBxOverrideCheckOptions.Value = Convert.ToInt32(_prefManager.OMIPreferences.OverrideCheckOptions);
                cmbBxOverrideCheckOthers.Value = Convert.ToInt32(_prefManager.OMIPreferences.OverrideCheckOthers);

                SetUIElementsVisibility();

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

        /// <summary>
        /// Sets the UI elements visibility.
        /// </summary>
        private void SetUIElementsVisibility()
        {
            try
            {
                bool optionsElementsVisibility = GetUIElementsVisibility((SelectedFeedPrice)(Convert.ToInt32(cmbBxOverrideWithOptions.Value)));
                ifLabel1.Visible = optionsElementsVisibility;
                isLabel1.Visible = optionsElementsVisibility;
                cmbBxOverrideCheckOptions.Visible = optionsElementsVisibility;
                cmbBxOverrideConditionOptions.Visible = optionsElementsVisibility;
                priceBarOptions.Visible = optionsElementsVisibility;

                bool othersElementsVisibility = GetUIElementsVisibility((SelectedFeedPrice)(Convert.ToInt32(cmbBxOverrideWithOthers.Value)));
                ifLabel2.Visible = othersElementsVisibility;
                isLabel2.Visible = othersElementsVisibility;
                cmbBxOverrideCheckOthers.Visible = othersElementsVisibility;
                cmbBxOverrideConditionOthers.Visible = othersElementsVisibility;
                priceBarOthers.Visible = othersElementsVisibility;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the price bar visibility.
        /// </summary>
        /// <param name="feedPriceChecks">The feed price checks.</param>
        /// <returns></returns>
        private bool GetUIElementsVisibility(SelectedFeedPrice feedPriceChecks)
        {
            bool isVisible = false;
            try
            {
                switch (feedPriceChecks)
                {
                    case SelectedFeedPrice.Ask:
                    case SelectedFeedPrice.Bid:
                    case SelectedFeedPrice.Last:
                    case SelectedFeedPrice.Previous:
                    case SelectedFeedPrice.Mid:
                    case SelectedFeedPrice.iMid:
                        isVisible = true;
                        break;

                    case SelectedFeedPrice.None:
                        isVisible = false;
                        break;
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isVisible;
        }

        /// <summary>
        /// Handles the ValueChanged event of the cmbBxOverrideSelectedPxOptions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void cmbBxOverrideSelectedPxOptions_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                SetUIElementsVisibility();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the cmbBxOverrideSelectedPxOthers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void cmbBxOverrideSelectedPxOthers_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                SetUIElementsVisibility();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the cmbOptPrice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void cmbOptPrice_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                List<EnumerationValue> listFeedPxChecks = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(SelectedFeedPrice));
                //remove option selected in option price combo
                listFeedPxChecks = new List<EnumerationValue>(listFeedPxChecks.Where(x => x.DisplayText != cmbOptPrice.Text && x.DisplayText != SelectedFeedPrice.AskOrBid.ToString()));
                SetComboDataSource(cmbBxOverrideWithOptions, listFeedPxChecks);

                SetOverrideCheckPriceCombo(cmbBxOverrideCheckOptions, Convert.ToInt32(cmbOptPrice.Value));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the cmbStockPrice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void cmbStockPrice_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                List<EnumerationValue> listFeedPxChecks = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(SelectedFeedPrice));
                //remove option selected in option price combo
                listFeedPxChecks = new List<EnumerationValue>(listFeedPxChecks.Where(x => x.DisplayText != cmbStockPrice.Text && x.DisplayText != SelectedFeedPrice.AskOrBid.ToString()));
                SetComboDataSource(cmbBxOverrideWithOthers, listFeedPxChecks);

                SetOverrideCheckPriceCombo(cmbBxOverrideCheckOthers, Convert.ToInt32(cmbStockPrice.Value));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the override check price combo.
        /// </summary>
        /// <param name="comboBox">The combo box.</param>
        /// <param name="selectedValue">The selected value.</param>
        private void SetOverrideCheckPriceCombo(UltraCombo comboBox, int selectedValue)
        {
            try
            {
                List<EnumerationValue> listFeedPx = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(SelectedFeedPrice));
                switch ((SelectedFeedPrice)selectedValue)
                {
                    case SelectedFeedPrice.Ask:
                    case SelectedFeedPrice.Bid:
                    case SelectedFeedPrice.Previous:
                        listFeedPx = new List<EnumerationValue>(listFeedPx.Where(x => Convert.ToInt32(x.Value) == selectedValue || x.DisplayText == SelectedFeedPrice.Last.ToString()));
                        break;

                    case SelectedFeedPrice.Last:
                        listFeedPx = new List<EnumerationValue>(listFeedPx.Where(x => Convert.ToInt32(x.Value) == selectedValue));
                        break;

                    case SelectedFeedPrice.Mid:
                        listFeedPx = new List<EnumerationValue>(listFeedPx.Where(x => x.DisplayText == SelectedFeedPrice.Mid.ToString() || x.DisplayText == SelectedFeedPrice.AskOrBid.ToString() || x.DisplayText == SelectedFeedPrice.Last.ToString()));
                        break;

                    case SelectedFeedPrice.iMid:
                        listFeedPx = new List<EnumerationValue>(listFeedPx.Where(x => x.DisplayText == SelectedFeedPrice.Mid.ToString() || x.DisplayText == SelectedFeedPrice.AskOrBid.ToString() || x.DisplayText == SelectedFeedPrice.Last.ToString()));
                        break;

                    default:
                        listFeedPx = new List<EnumerationValue>();
                        break;
                }
                SetComboDataSource(comboBox, listFeedPx);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        //private void grdInterestRate_AfterCellUpdate(object sender, CellEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Cell.Column.Key.Equals("Period"))
        //        {
        //            UltraGridRow Activerow = e.Cell.Row;
        //            foreach (UltraGridRow row in grdInterestRate.Rows)
        //            {
        //                if (!row.Equals(Activerow))
        //                {
        //                    if (row.Cells["Period"].Value.ToString().Equals(Activerow.Cells["Period"].Value.ToString()))
        //                    {
        //                        MessageBox.Show("The month period you have entered already exists,please enter a different value", "Pricing Inputs", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                        e.Cell.Value = e.Cell.OriginalValue;
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void grdOptionModel_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "ForwardPointsUsed")
                {
                    if (bool.Parse(e.Cell.Value.ToString()))
                    {
                        e.Cell.Row.Cells["LastPriceUsed"].Value = false;
                        e.Cell.Row.Cells["TheoreticalPriceUsed"].Value = false;
                    }
                }
                if (e.Cell.Column.Key == "LastPriceUsed")
                {
                    if (bool.Parse(e.Cell.Value.ToString()))
                    {
                        e.Cell.Row.Cells["ForwardPointsUsed"].Value = false;
                        e.Cell.Row.Cells["TheoreticalPriceUsed"].Value = false;
                        e.Cell.Row.Cells["ProxySymbolUsed"].Value = false;
                    }
                }
                if (e.Cell.Column.Key == "ProxySymbolUsed")
                {
                    if (bool.Parse(e.Cell.Value.ToString()))
                    {
                        e.Cell.Row.Cells["TheoreticalPriceUsed"].Value = false;
                        e.Cell.Row.Cells["LastPriceUsed"].Value = false;
                    }
                }
                if (e.Cell.Column.Key == "TheoreticalPriceUsed")
                {
                    if (bool.Parse(e.Cell.Value.ToString()))
                    {
                        e.Cell.Row.Cells["ForwardPointsUsed"].Value = false;
                        e.Cell.Row.Cells["LastPriceUsed"].Value = false;
                        e.Cell.Row.Cells["ProxySymbolUsed"].Value = false;
                    }
                }
                if (e.Cell.Column.Key == "HistoricalVolUsed")
                {
                    if (bool.Parse(e.Cell.Value.ToString()))
                    {
                        e.Cell.Row.Cells["VolatilityUsed"].Value = false;
                    }
                }
                if (e.Cell.Column.Key == "VolatilityUsed")
                {
                    if (bool.Parse(e.Cell.Value.ToString()))
                    {
                        e.Cell.Row.Cells["HistoricalVolUsed"].Value = false;
                    }
                }

                Type datatype = e.Cell.Value.GetType();
                if (datatype.FullName.Equals("System.Double"))
                    if (Convert.ToDouble(e.Cell.Value.ToString()) < 0 && e.Cell.Column.Header.Caption != "User Delta" && e.Cell.Column.Key != "ForwardPoints")
                    {
                        MessageBox.Show("Please enter a valid value, value cannot be negative", "Pricing Inputs", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cell.Value = e.Cell.OriginalValue;
                        return;
                    }
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Logger.HandleException(new Exception("Value entered is invalid according to its datatype"), LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #region ILiveFeedCallback Members
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            try
            {
                if (data != null)
                {
                    DataRow foundRow = null;
                    string symbol = data.Symbol;
                    //UIThreadMarshallerGreekscalc mi = new UIThreadMarshallerGreekscalc(SnapshotResponse);
                    if (UIValidation.GetInstance().validate(this))
                    {
                        if (InvokeRequired)
                        {
                            MethodInvoker del =
                                delegate
                                {
                                    SnapshotResponse(data);
                                };
                            this.BeginInvoke(del);
                            return;
                        }
                        timerRefresh.Stop();
                        timerRefresh.Start();
                        _responseReceivedCount++;
                        foundRow = dtOMI.Rows.Find(symbol);
                        if (foundRow != null)
                        {
                            if (data.CategoryCode == AssetCategory.EquityOption || data.CategoryCode == AssetCategory.FutureOption)
                            {

                                OptionSymbolData optData = data as OptionSymbolData;
                                foundRow["ImpliedVol"] = optData.ImpliedVol * 100;
                                foundRow["InterestRate"] = optData.InterestRate * 100;
                                foundRow["ActualDelta"] = optData.Delta;
                                foundRow["LastPx"] = optData.PreferencedPrice;
                                foundRow["ActualSharesOutstanding"] = optData.SharesOutstanding;
                            }
                            else
                            {
                                foundRow["LastPx"] = data.PreferencedPrice;
                                foundRow["ActualDividend"] = data.DividendYield * 100;
                                if (string.IsNullOrEmpty(foundRow["SecurityDescription"].ToString()))
                                {
                                    foundRow["SecurityDescription"] = data.FullCompanyName;
                                }
                                foundRow["ActualSharesOutstanding"] = data.SharesOutstanding;
                            }
                        }
                        dtOMI.AcceptChanges();
                        grdOptionModel.UpdateData();
                        toolStripLabelOMI.Text = "Refreshing Live Data..." + Math.Floor((Convert.ToDouble(_responseReceivedCount) / Convert.ToDouble(_totalRequestedSymbols)) * 100).ToString() + "% Completed";
                        statusStripOMI.Refresh();
                    }
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

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void LiveFeedDisConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        private void rBtnLiveData_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SetGroupBoxVisibility(rBtnLiveData.Checked);
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
        /// Sets the group box visibility.
        /// </summary>
        /// <param name="isVisible">if set to <c>true</c> [is visible].</param>
        private void SetGroupBoxVisibility(bool isVisible)
        {
            try
            {
                grpBoxOptionSelectedFeedPrice.Visible = isVisible;
                grpBoxOtherAssetsFeedPrice.Visible = isVisible;
                grpBoxUseDefaultDelta.Location = isVisible ? new System.Drawing.Point(123, 313) : grpBoxOptionSelectedFeedPrice.Location;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void btnSavePreferences_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyPreferences();
                _prefManager.SaveOMIPreferences();
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

        private void ApplyPreferences()
        {
            try
            {
                _prefManager.OMIPreferences.OptionSelectedFeedPrice = (SelectedFeedPrice)Convert.ToInt32(cmbOptPrice.Value);
                _prefManager.OMIPreferences.SelectedFeedPrice = (SelectedFeedPrice)Convert.ToInt32(cmbStockPrice.Value);
                _prefManager.OMIPreferences.UseClosingMark = uOsetPricingSource.CheckedIndex == 1 ? true : false;

                _prefManager.OMIPreferences.OverrideWithOptions = (SelectedFeedPrice)(Convert.ToInt32(cmbBxOverrideWithOptions.Value));
                _prefManager.OMIPreferences.OverrideWithOthers = (SelectedFeedPrice)(Convert.ToInt32(cmbBxOverrideWithOthers.Value));

                _prefManager.OMIPreferences.UseDefaultDelta = checkBoxUseDefaultDelta.Checked;

                _prefManager.OMIPreferences.OverrideConditionOptions = (NumericConditionOperator)(Convert.ToInt32(cmbBxOverrideConditionOptions.Value));
                _prefManager.OMIPreferences.OverrideConditionOthers = (NumericConditionOperator)(Convert.ToInt32(cmbBxOverrideConditionOthers.Value));
                _prefManager.OMIPreferences.PriceBarOptions = priceBarOptions.Value;
                _prefManager.OMIPreferences.PriceBarOthers = priceBarOthers.Value;
                _prefManager.OMIPreferences.OverrideCheckOptions = (SelectedFeedPrice)(Convert.ToInt32(cmbBxOverrideCheckOptions.Value));
                _prefManager.OMIPreferences.OverrideCheckOthers = (SelectedFeedPrice)(Convert.ToInt32(cmbBxOverrideCheckOthers.Value));
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

        private void rBtnClosingMark_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rBtnClosingMark.Checked)
                {
                    rBtnLiveData_CheckedChanged(null, null);
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

        //private void btnScreenshot_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SnapShotManager.GetInstance().TakeSnapshot(this);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void btnGetHistoricalVol_Click(object sender, EventArgs e)
        {
            try
            {
                DisableForm(false);
                if (!CustomThemeHelper.ApplyTheme)
                {
                    toolStripLabelOMI.ForeColor = Color.Red;

                }
                toolStripLabelOMI.Text = "Calculating Historical Volatility...";
                PranaRequestCarrier pranaRequestCarrier = new PranaRequestCarrier();
                CreateRequest(pranaRequestCarrier);
                bgGetHistoricalvol = new BackgroundWorker();
                bgGetHistoricalvol.DoWork += new DoWorkEventHandler(bgGetHistoricalvol_DoWork);
                bgGetHistoricalvol.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgGetHistoricalvol_RunWorkerCompleted);
                bgGetHistoricalvol.RunWorkerAsync(pranaRequestCarrier);
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

        void bgGetHistoricalvol_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                PranaRequestCarrier pranaRequestCarrier = e.Result as PranaRequestCarrier;
                if (pranaRequestCarrier != null)
                {
                    GetInstance_HistoricalVolCalculated(pranaRequestCarrier);
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

        private void OptionModelInputs_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (bgGetHistoricalvol != null)
                {
                    if (bgGetHistoricalvol.IsBusy)
                    {
                        if (!CustomThemeHelper.ApplyTheme)
                        {
                            toolStripLabelOMI.ForeColor = Color.Red;

                        }
                        toolStripLabelOMI.Text = "Please wait while Historical Volatility is being calculated, it may take few minutes...";
                        e.Cancel = true;
                    }
                }
                if (isDataSaved.Equals(false))
                {
                    DialogResult result = MessageBox.Show("There are some unsaved changes, Do you want to save?", "Pricing Inputs", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                    if (DialogResult.Yes.Equals(result))
                    {
                        this.btnSave_Click(this.btnSave, e);
                    }
                    if (_bgSaveData != null)
                    {
                        if (_bgSaveData.IsBusy)
                        {
                            toolStripLabelOMI.Text = "Please wait while data is being saved, it may take few minutes...";
                            e.Cancel = true;
                            _isSaveclick = false;
                        }
                    }
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

        #region IPublishing Members
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                if (e != null && topicName != null)
                {
                    UIThreadMarshallerPublish mi = new UIThreadMarshallerPublish(Publish);
                    if (UIValidation.GetInstance().validate(this))
                    {
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(mi, new object[] { e, topicName });
                        }
                        else
                        {
                            System.Object[] dataList = null;
                            switch (topicName)
                            {
                                case Topics.Topic_SecurityMaster:
                                    dataList = (System.Object[])e.EventData;
                                    SecMasterbaseList secMasterObjlist = new SecMasterbaseList();
                                    foreach (Object secmasterObj in dataList)
                                    {
                                        secMasterObjlist.Add((SecMasterBaseObj)secmasterObj);
                                    }
                                    UpdateOMIDataFromSecMaster(secMasterObjlist);
                                    break;

                                case Topics.Topic_OMIData:
                                    dataList = (System.Object[])e.EventData;

                                    if (dataList != null && dataList.Length == 2)
                                    {
                                        switch ((OMIPublishType)dataList[0])
                                        {
                                            case OMIPublishType.OMIData:
                                                DisableForm(false);
                                                if (((DataTable)grdOptionModel.DataSource).DataSet.HasChanges() || !isDataSaved)
                                                {
                                                    isDataSaved = true;
                                                    dtOMI.RejectChanges();
                                                    if (!CustomThemeHelper.ApplyTheme)
                                                    {
                                                        toolStripLabelOMI.ForeColor = Color.Green;
                                                    }
                                                    toolStripLabelOMI.Text = DateTime.Now.ToString() + ": Refresh Completed (different user has updated data)";
                                                }
                                                else
                                                {
                                                    if (!CustomThemeHelper.ApplyTheme)
                                                    {
                                                        toolStripLabelOMI.ForeColor = Color.Green;
                                                    }
                                                    toolStripLabelOMI.Text = DateTime.Now.ToString() + ": Refresh Completed";
                                                }

                                                List<UserOptModelInput> omiDataList = (List<UserOptModelInput>)dataList[1];
                                                foreach (UserOptModelInput userOMI in omiDataList)
                                                {
                                                    UpdateOMIData(userOMI);
                                                }

                                                dtOMI.AcceptChanges();
                                                grdOptionModel.DataSource = dtOMI;
                                                GetAssetClass();
                                                grdOptionModel.UpdateData();
                                                if (_scrollPos != 0)
                                                {
                                                    grdOptionModel.ActiveRowScrollRegion.ScrollPosition = _scrollPos;
                                                    _scrollPos = 0;
                                                }

                                                if (grdOptionModel.DisplayLayout.Bands[0].SortedColumns.Count > 0)
                                                {
                                                    grdOptionModel.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                                                }
                                                else
                                                {
                                                    grdOptionModel.DisplayLayout.Bands[0].Columns["Symbol"].SortIndicator = SortIndicator.Ascending;
                                                    grdOptionModel.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                                                }
                                                EnableForm();
                                                break;
                                            case OMIPublishType.OMIPreferences:
                                                _prefManager.OMIPreferences = (LiveFeedPreferences)dataList[1];
                                                SetUpLiveFeedPreferences();
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }

                if (_isSaveclick == false)
                {
                    if (this is Form)
                    {
                        this.Close();
                    }
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

        private void UpdateOMIDataFromSecMaster(SecMasterbaseList secMasterBaseList)
        {
            try
            {
                foreach (SecMasterBaseObj secMasterObject in secMasterBaseList)
                {
                    DataRow dr = dtOMI.Rows.Find(secMasterObject.TickerSymbol);
                    if (dr != null)
                    {
                        if (dr.Table.Columns.Contains("Bloomberg"))
                        {
                            dr["Bloomberg"] = secMasterObject.BloombergSymbol;
                        }
                        if (dr.Table.Columns.Contains("ProxySymbol"))
                        {
                            dr["ProxySymbol"] = secMasterObject.ProxySymbol;
                        }
                        if (dr.Table.Columns.Contains("SecurityDescription"))
                        {
                            dr["SecurityDescription"] = secMasterObject.LongName;
                        }
                        if (dr.Table.Columns.Contains("UnderlyingSymbol"))
                        {
                            dr["UnderlyingSymbol"] = secMasterObject.UnderLyingSymbol;
                        }

                        if (dr.Table.Columns.Contains("SMSharesOutstanding"))
                        {
                            dr["SMSharesOutstanding"] = secMasterObject.SharesOutstanding;
                        }

                        switch (secMasterObject.AssetCategory)
                        {
                            case AssetCategory.Future:
                                SecMasterFutObj futureObj = (SecMasterFutObj)secMasterObject;

                                if (dr.Table.Columns.Contains("ExpirationDate"))
                                {
                                    dr["ExpirationDate"] = futureObj.ExpirationDate;
                                }
                                if (dr.Table.Columns.Contains("SecurityDescription"))
                                {
                                    dr["SecurityDescription"] = futureObj.LongName;
                                }
                                break;

                            case AssetCategory.EquityOption:
                            case AssetCategory.FutureOption:

                                SecMasterOptObj optObject = (SecMasterOptObj)secMasterObject;
                                if (dr.Table.Columns.Contains("ExpirationDate"))
                                {
                                    dr["ExpirationDate"] = optObject.ExpirationDate;
                                }
                                if (dr.Table.Columns.Contains("PutorCall"))
                                {
                                    dr["PutorCall"] = optObject.PutOrCall;
                                }
                                if (dr.Table.Columns.Contains("StrikePrice"))
                                {
                                    dr["StrikePrice"] = optObject.StrikePrice;
                                }
                                if (dr.Table.Columns.Contains("IDCOOptionSymbol"))
                                {
                                    dr["IDCOOptionSymbol"] = optObject.IDCOOptionSymbol;
                                }

                                if (dr.Table.Columns.Contains("OSIOptionSymbol"))
                                {
                                    dr["OSIOptionSymbol"] = optObject.OSIOptionSymbol;
                                }
                                break;
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-3883
                            // Here at the time of changing proxy symbol from SM, PI tries to update its cache but if somehow it contains Expiration Date
                            // then it tries to cast Equity object into Fixed Income object which throws error, so commenting this.
                            //case AssetCategory.PrivateEquity:
                            case AssetCategory.FixedIncome:
                            case AssetCategory.ConvertibleBond:
                                if (dr.Table.Columns.Contains("ExpirationDate"))
                                {
                                    SecMasterFixedIncome fixedIncomeObj = (SecMasterFixedIncome)secMasterObject;
                                    dr["ExpirationDate"] = fixedIncomeObj.MaturityDate;
                                }
                                break;

                            case AssetCategory.FX:
                                SecMasterFxObj fxObj = (SecMasterFxObj)secMasterObject;
                                if (dr.Table.Columns.Contains("LeadCurrencyID"))
                                {
                                    dr["LeadCurrencyID"] = fxObj.LeadCurrencyID;
                                }
                                if (dr.Table.Columns.Contains("VsCurrencyID"))
                                {
                                    dr["VsCurrencyID"] = fxObj.VsCurrencyID;
                                }
                                break;

                            case AssetCategory.FXForward:
                                SecMasterFXForwardObj forwardObj = (SecMasterFXForwardObj)secMasterObject;
                                if (dr.Table.Columns.Contains("LeadCurrencyID"))
                                {
                                    dr["LeadCurrencyID"] = forwardObj.LeadCurrencyID;
                                }
                                if (dr.Table.Columns.Contains("VsCurrencyID"))
                                {
                                    dr["VsCurrencyID"] = forwardObj.VsCurrencyID;
                                }
                                if (dr.Table.Columns.Contains("ExpirationDate"))
                                {
                                    dr["ExpirationDate"] = forwardObj.ExpirationDate; ;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
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

        private void UpdateOMIData(UserOptModelInput userOMI)
        {
            try
            {
                if (userOMI.PersistenceStatus == ApplicationConstants.PersistenceStatus.Deleted)
                {
                    DataRow dr = dtOMI.Rows.Find(userOMI.Symbol);
                    if (dr != null)
                    {
                        dtOMI.Rows.Remove(dr);
                    }
                }
                else
                {
                    bool isNewRow = false;
                    DataRow dr = dtOMI.Rows.Find(userOMI.Symbol);
                    if (dr == null || dr.Table == null || !dr.Table.Columns.Contains("Symbol") || dr["Symbol"].ToString() != userOMI.Symbol)
                    {
                        dr = dtOMI.NewRow();
                        isNewRow = true;
                    }
                    dr["Volatility"] = userOMI.Volatility * 100;
                    dr["VolatilityUsed"] = userOMI.VolatilityUsed;
                    dr["IntRate"] = userOMI.IntRate * 100;
                    dr["IntRateUsed"] = userOMI.IntRateUsed;
                    dr["Dividend"] = userOMI.Dividend * 100;
                    dr["DividendUsed"] = userOMI.DividendUsed;
                    dr["StockBorrowCost"] = userOMI.StockBorrowCost * 100;
                    dr["StockBorrowCostUsed"] = userOMI.StockBorrowCostUsed;
                    dr["Delta"] = userOMI.Delta;
                    dr["DeltaUsed"] = userOMI.DeltaUsed;
                    dr["LastPrice"] = userOMI.LastPrice;
                    dr["LastPriceUsed"] = userOMI.LastPriceUsed;
                    dr["ForwardPoints"] = userOMI.ForwardPoints;
                    dr["ForwardPointsUsed"] = userOMI.ForwardPointsUsed;
                    dr["TheoreticalPriceUsed"] = userOMI.TheoreticalPriceUsed;
                    dr["ProxySymbol"] = userOMI.ProxySymbol;
                    dr["ProxySymbolUsed"] = userOMI.ProxySymbolUsed;
                    dr["AssetID"] = userOMI.AssetID;
                    dr["Symbol"] = userOMI.Symbol;
                    dr["PSSymbol"] = GetPSSymbol(dr);
                    dr["PBSymbol"] = userOMI.PBSymbol;
                    dr["SecurityDescription"] = userOMI.SecurityDescription;
                    dr["SharesOutstanding"] = userOMI.SharesOutstanding;
                    dr["SharesOutstandingUsed"] = userOMI.SharesOutstandingUsed;
                    dr["SMSharesOutstanding"] = userOMI.SMSharesOutstanding;
                    dr["SMSharesOutstandingUsed"] = userOMI.SMSharesOutstandingUsed;
                    dr["ClosingMarkUsed"] = userOMI.ClosingMarkUsed;
                    dr["ManualInput"] = userOMI.ManualInput;
                    dr["IsHistorical"] = userOMI.IsHistorical;
                    dr["HistoricalVol"] = userOMI.HistoricalVol * 100;
                    dr["HistoricalVolUsed"] = userOMI.HistoricalVolUsed;
                    dr["UnderlyingSymbol"] = userOMI.UnderlyingSymbol;
                    dr["Bloomberg"] = userOMI.Bloomberg;
                    dr["OSIOptionSymbol"] = userOMI.OSIOptionSymbol;
                    dr["IDCOOptionSymbol"] = userOMI.IDCOOptionSymbol;
                    dr["LeadCurrencyID"] = userOMI.LeadCurrencyID;
                    dr["VsCurrencyID"] = userOMI.VsCurrencyID;
                    dr["StrikePrice"] = userOMI.StrikePrice;
                    dr["PutorCall"] = userOMI.PutorCall;
                    dr["ExpirationDate"] = userOMI.ExpirationDate;
                    dr["AuecID"] = userOMI.AuecID;
                    dr[const_BloombergSymbolExCode] = userOMI.BloombergSymbolWithExchangeCode;

                    if (isNewRow)
                        dtOMI.Rows.Add(dr);
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

        public string getReceiverUniqueName()
        {
            return "Option Model Inputs";
        }
        #endregion

        // Kuldeep A.: This checkbox is used to filter out the zero position symbols.
        // If user check it then only symbols with positions greater than zero appears on PI.
        private void radioNonZeroPositions_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioNonZeroPositions.Checked == true)
                {
                    FetchOMIDataFromDB(false);
                }
                else
                {
                    FetchOMIDataFromDB(true);
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

        private void checkBoxSharesOutstanding_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                if (checkBoxSharesOutstanding.Checked)
                {
                    band.Groups["SharesoutStandingHeader"].Hidden = false;
                    if (checkBoxDividend.Checked && checkBoxIR.Checked && checkBoxLastPrice.Checked && checkBoxVolatility.Checked && checkBoxDelta.Checked && checkBoxSharesOutstanding.Checked)
                    {
                        checkBoxAll.Checked = true;
                    }
                }
                else
                {
                    band.Groups["SharesoutStandingHeader"].Hidden = true;
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

        private void checkBoxClosingMark_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                if (checkBoxClosingMark.Checked)
                {

                    band.Groups["ClosingMarkHeader"].Hidden = false;
                    if (checkBoxDividend.Checked && checkBoxIR.Checked && checkBoxLastPrice.Checked && checkBoxVolatility.Checked && checkBoxDelta.Checked && checkBoxSharesOutstanding.Checked && checkBoxClosingMark.Checked && checkBoxManualInput.Checked)
                    {
                        checkBoxAll.Checked = true;
                    }
                }
                else
                {
                    band.Groups["ClosingMarkHeader"].Hidden = true;
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

        private void checkBoxManualInput_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                if (checkBoxManualInput.Checked)
                {

                    band.Groups["ManualInputHeader"].Hidden = false;
                    if (checkBoxDividend.Checked && checkBoxIR.Checked && checkBoxLastPrice.Checked && checkBoxVolatility.Checked && checkBoxDelta.Checked && checkBoxSharesOutstanding.Checked && checkBoxClosingMark.Checked && checkBoxManualInput.Checked)
                    {
                        checkBoxAll.Checked = true;
                    }
                }
                else
                {
                    band.Groups["ManualInputHeader"].Hidden = true;
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

        private void OptionModelInputs_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRICING_INPUTS);
                    if (CustomThemeHelper.ApplyTheme)
                    {
                        this.statusStripOMI.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                        this.statusStripOMI.ForeColor = System.Drawing.Color.WhiteSmoke;
                        this.toolStripLabelOMI.BackColor = System.Drawing.Color.FromArgb(88, 88, 90);
                        this.toolStripLabelOMI.ForeColor = System.Drawing.Color.WhiteSmoke;
                        this.priceBarOptions.BackColor = System.Drawing.Color.WhiteSmoke;
                        this.priceBarOptions.ForeColor = System.Drawing.Color.Black;
                        this.priceBarOthers.BackColor = System.Drawing.Color.WhiteSmoke;
                        this.priceBarOthers.ForeColor = System.Drawing.Color.Black;
                        this.toolStripLabelOMI.Font = new Font("Century Gothic", 9F);
                        this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                        this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                        this.LaunchSymbolLookup += PI_LaunchSymbolLookup;
                    }
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
                    }
                }
                InstanceManager.RegisterInstance(this);
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

        private void SetButtonsColor()
        {
            try
            {
                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSavePreferences.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSavePreferences.ForeColor = System.Drawing.Color.White;
                btnSavePreferences.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSavePreferences.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSavePreferences.UseAppStyling = false;
                btnSavePreferences.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnHistoricalVolInputs.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnHistoricalVolInputs.ForeColor = System.Drawing.Color.White;
                btnHistoricalVolInputs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnHistoricalVolInputs.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnHistoricalVolInputs.UseAppStyling = false;
                btnHistoricalVolInputs.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetHistoricalVol.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetHistoricalVol.ForeColor = System.Drawing.Color.White;
                btnGetHistoricalVol.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetHistoricalVol.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetHistoricalVol.UseAppStyling = false;
                btnGetHistoricalVol.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExport.ForeColor = System.Drawing.Color.White;
                btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExport.UseAppStyling = false;
                btnExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSymbolLookup.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSymbolLookup.ForeColor = System.Drawing.Color.White;
                btnSymbolLookup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSymbolLookup.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSymbolLookup.UseAppStyling = false;
                btnSymbolLookup.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRefreshLiveData.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRefreshLiveData.ForeColor = System.Drawing.Color.White;
                btnRefreshLiveData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRefreshLiveData.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRefreshLiveData.UseAppStyling = false;
                btnRefreshLiveData.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRefresh.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRefresh.ForeColor = System.Drawing.Color.White;
                btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRefresh.UseAppStyling = false;
                btnRefresh.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void uOSetSymbols_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_checkedIndex == 3)
                {
                    FetchOMIDataFromDB(true);
                }

                switch (uOSetSymbols.CheckedIndex)
                {
                    case 0:
                        grdOptionModel.DisplayLayout.Bands[0].ColumnFilters["AssetID"].ClearFilterConditions();
                        foreach (UltraGridRow row in grdOptionModel.Rows)
                        {
                            if (row.Hidden)
                            {
                                row.Hidden = false;
                            }
                        }
                        _checkedIndex = 0;
                        break;
                    case 1:
                        FilterGridToDisplayOptions();
                        break;
                    case 2:
                        FilterGridToDisplayOptionsAndUnderliers();
                        break;
                    case 3:
                        grdOptionModel.DisplayLayout.Bands[0].ColumnFilters["AssetID"].ClearFilterConditions();
                        FetchOMIDataFromDB(false);
                        _checkedIndex = 3;
                        break;
                    default:
                        _checkedIndex = 0;
                        break;
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

        private void ultraOptionSet1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                switch (uOsetPricingSource.CheckedIndex)
                {
                    case 0:
                        SetGroupBoxVisibility(true);
                        break;
                    case 1:
                        SetGroupBoxVisibility(false);
                        break;
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
        void menuClearFilters_Click(object sender, System.EventArgs e)
        {
            try
            {
                foreach (UltraGridBand band in grdOptionModel.DisplayLayout.Bands)
                {
                    band.ColumnFilters.ClearAllFilters();
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
        void menuDeleteSymbol_Click(object sender, System.EventArgs e)
        {
            try
            {
                UltraGridRow activerow = grdOptionModel.ActiveRow;
                String symbol = activerow.Cells["Symbol"].Value.ToString();
                _pricingServiceProxy.InnerChannel.DeleteSymbolFromPI(symbol);
                btnRefresh_Click(null, null);
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

        private void contextmenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UltraGridRow activerow = grdOptionModel.ActiveRow;
            if (activerow != null)
            {
                bool manualInputvalue = Convert.ToBoolean(activerow.Cells["ManualInput"].Value);
                menuDeleteSymbol.Enabled = manualInputvalue;
            }
        }

        private void grdOptionModel_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdOptionModel);
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

        private void SavePricingInputsPreferences()
        {
            try
            {
                PricingPreference pricingPrefs = PricingPreferenceManager.PricingPreference;
                pricingPrefs.SymbolsIndexVal = uOSetSymbols.CheckedIndex;
                pricingPrefs.ClosingMark = checkBoxClosingMark.Checked;
                pricingPrefs.ManualInput = checkBoxManualInput.Checked;
                pricingPrefs.SharesOutstanding = checkBoxSharesOutstanding.Checked;
                pricingPrefs.All = checkBoxAll.Checked;
                pricingPrefs.LastPrice = checkBoxLastPrice.Checked;
                pricingPrefs.Delta = checkBoxDelta.Checked;
                pricingPrefs.Dividend = checkBoxDividend.Checked;
                pricingPrefs.InterestRate = checkBoxIR.Checked;
                pricingPrefs.Volatility = checkBoxVolatility.Checked;
                pricingPrefs.TheoreticalPrice = checkBoxTheoreticalPrice.Checked;
                PricingPreferenceManager.SavePreferences(pricingPrefs);
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

        private void GetPricingInputsGridFromXML()
        {
            try
            {
                List<ColumnData> listColData = PricingLayoutManager.PricingLayout.PricingInputColumns;
                PricingLayoutManager.SetGridColumnLayout(grdOptionModel, listColData, GetListOfColumnsToDisplayOnGrid());
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

        private void SetPIPreferences()
        {
            try
            {
                PricingPreference pricingPreference = PricingPreferenceManager.PricingPreference;
                uOSetSymbols.CheckedIndex = pricingPreference.SymbolsIndexVal;
                checkBoxClosingMark.Checked = pricingPreference.ClosingMark;
                checkBoxManualInput.Checked = pricingPreference.ManualInput;
                checkBoxSharesOutstanding.Checked = pricingPreference.SharesOutstanding;
                checkBoxAll.Checked = pricingPreference.All;
                checkBoxLastPrice.Checked = pricingPreference.LastPrice;
                checkBoxDelta.Checked = pricingPreference.Delta;
                checkBoxDividend.Checked = pricingPreference.Dividend;
                checkBoxIR.Checked = pricingPreference.InterestRate;
                checkBoxVolatility.Checked = pricingPreference.Volatility;
                checkBoxTheoreticalPrice.Checked = pricingPreference.TheoreticalPrice;
                _checkedIndex = uOSetSymbols.CheckedIndex;
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

        private void FilterGridToDisplayOptionsAndUnderliers()
        {
            try
            {
                List<string> OptionUnderlyings = new List<string>();
                grdOptionModel.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                UltraGridColumn column = grdOptionModel.DisplayLayout.Bands[0].Columns["AssetID"];
                foreach (UltraGridRow row in grdOptionModel.Rows)
                {
                    if (row.Cells[column].Value.ToString().Equals("2") || row.Cells[column].Value.ToString().Equals("4") || row.Cells[column].Value.ToString().Equals("10"))
                    {
                        if (!OptionUnderlyings.Contains(row.Cells["UnderlyingSymbol"].Value.ToString()))
                            OptionUnderlyings.Add(row.Cells["UnderlyingSymbol"].Value.ToString());
                    }
                    row.Hidden = false;
                }
                foreach (UltraGridRow row in grdOptionModel.Rows)
                {
                    if (!row.Cells[column].Value.ToString().Equals("2") && !row.Cells[column].Value.ToString().Equals("4") && !row.Cells[column].Value.ToString().Equals("10"))
                    {
                        string symbol = row.Cells["Symbol"].Value.ToString();
                        if (!OptionUnderlyings.Contains(symbol))
                            row.Hidden = true;

                    }
                }
                _checkedIndex = 2;
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

        string _symbolListForFetch = string.Empty;
        bool _isFetchFilterData = false;

        /// <summary>
        /// Sets the symbols for fetch.
        /// </summary>
        /// <param name="symbols">The symbols.</param>
        public void SetSymbolsForFetch(string symbols)
        {
            try
            {
                _isFetchFilterData = true;
                _symbolListForFetch = symbols;
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


        public void FilterBySelectedSymbol(string symbols)
        {
            try
            {
                UltraGridBand band = grdOptionModel.DisplayLayout.Bands[0];
                band.ColumnFilters.ClearAllFilters();
                string symbolList = symbols.TrimEnd(',');
                band.ColumnFilters["Symbol"].LogicalOperator = FilterLogicalOperator.Or;
                foreach (string symbol in symbolList.Split(','))
                {
                    band.ColumnFilters["Symbol"].FilterConditions.Add(FilterComparisionOperator.Equals, symbol);
                }
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

        public void AddSymboltoPI(List<string> list)
        {
            try
            {

                if (grdOptionModel.DataSource == null)
                {
                    FetchOMIDataFromDB(true);
                }

                DataTable dt = (DataTable)grdOptionModel.DataSource;
                DataRow row = dt.NewRow();
                if (list != null && list.Count > 0 && !dt.Rows.Contains(list[0]))
                {
                    row["Symbol"] = list[0];
                    row["UnderlyingSymbol"] = list[1];
                    row["AssetID"] = list[2];
                    row["Bloomberg"] = list[3];
                    row["OSIOptionSymbol"] = list[4];
                    row["IDCOOptionSymbol"] = list[5];
                    row["SecurityDescription"] = list[6];
                    row["ProxySymbol"] = list[7];
                    row["ManualInput"] = true;
                    row["HistoricalVol"] = 0.0;
                    row["HistoricalVolUsed"] = false;
                    row["Volatility"] = 0.0;
                    row["VolatilityUsed"] = false;
                    row["IntRate"] = 0.0;
                    row["IntRateUsed"] = false;
                    row["Dividend"] = 0.0;
                    row["DividendUsed"] = false;
                    row["Delta"] = 0.0;
                    row["DeltaUsed"] = false;
                    row["LastPrice"] = 0.0;
                    row["ForwardPoints"] = 0.0;
                    row["ForwardPointsUsed"] = false;
                    row["StockBorrowCost"] = 0.0;
                    row["StockBorrowCostUsed"] = false;
                    row["LastPriceUsed"] = false;
                    row["TheoreticalPriceUsed"] = false;
                    row["SharesOutstanding"] = 0.0;
                    row["SharesOutstandingUsed"] = false;
                    row["SMSharesOutstanding"] = 0.0;
                    row["SMSharesOutstandingUsed"] = false;
                    row["ClosingMarkUsed"] = false;
                    row["ProxySymbolUsed"] = false;

                    dt.Rows.InsertAt(row, 0);
                    GetAssetClass();
                    grdOptionModel.DataSource = dt;
                    dt.AcceptChanges();
                    btnSave_Click(this, new EventArgs());
                }

                else
                {
                    toolStripLabelOMI.Text = DateTime.Now.ToString() + " The Symbol is already present in PI";
                }
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

        private void FilterGridToDisplayOptions()
        {
            try
            {
                grdOptionModel.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                UltraGridColumn ccolumn = grdOptionModel.DisplayLayout.Bands[0].Columns["AssetID"];
                foreach (UltraGridRow row in grdOptionModel.Rows)
                {
                    if (!(row.Cells[ccolumn].Value.ToString().Equals("2") || row.Cells[ccolumn].Value.ToString().Equals("4") || row.Cells[ccolumn].Value.ToString().Equals("10")))
                    {
                        row.Hidden = true;
                    }
                }
                _checkedIndex = 1;
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

        private void grdOptionModel_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdOptionModel_AfterColPosChanged(object sender, AfterColPosChangedEventArgs e)
        {
            try
            {
                grdOptionModel.DisplayLayout.Bands[0].Columns["ProxySymbolUsed"].Hidden = grdOptionModel.DisplayLayout.Bands[0].Columns["ProxySymbol"].Hidden;
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
        /// Handles the AfterRowFilterChanged event of the grdOptionModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="AfterRowFilterChangedEventArgs"/> instance containing the event data.</param>
        private void grdOptionModel_AfterRowFilterChanged(object sender, AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if (_isFetchFilterData)
                {
                    btnRefresh_Click(null, null);
                    _isFetchFilterData = false;
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

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// used to Export Data for automation
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            try
            {
                // Create a new instance of the exporter
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // Perform the export
                if (gridName == "grdOptionModel")
                    exporter.Export(grdOptionModel, filePath);
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
    }
}