using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Analytics.Classes;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Prana.Analytics
{
    public partial class CtrlRiskReports : UserControl
    {
        private List<string> _listRequest = new List<string>();
        private bool isAlreadyStarted = false;
        private Dictionary<string, List<PranaRiskObj>> _dictRiskObjs = new Dictionary<string, List<PranaRiskObj>>();
        private PranaRiskObjColl _riskObjcollection = new PranaRiskObjColl();
        private DataTable _dtCalculatedRisks = new DataTable();
        private DataTable _dataPortfolio = new DataTable();
        private Dictionary<string, Dictionary<string, ComponentRiskData>> _componentRiskByGrouping = new Dictionary<string, Dictionary<string, ComponentRiskData>>();
        private CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();
        public event EventHandler RefreshCompleted;
        public ManualResetEvent resetEvent = null;
        private bool _isReloadingLayout = false;
        UltraGridColumn _columnSorted = null;
        List<string> GroupByColumnsCollection = new List<string>();
        GroupSortComparer _groupSortComparer = new GroupSortComparer();
        private RiskParamameter _riskParams = new RiskParamameter();

        bool _isRiskCalculatedGroup = true;
        bool _isRiskCalculatedIndividual = true;
        private delegate void UIThreadMarshellers(PranaRequestCarrier pranaRequestCarrier);
        private delegate void UIThreadMarshellerUpdateToolStrip(string statusMessage);

        DateTime _startDate = DateTime.MinValue;
        DateTime _endDate = DateTime.MinValue;

        string _firstLevelGroup = string.Empty;
        string _secondLevelGroup = string.Empty;
        private string tabRiskReport = "Risk Report";

        private int _responseReceivedCount = 0;
        private int _requestedCallsCount = 0;
        private static readonly object _locker = new object();

        #region Column Names
        private const string COL_IsChecked = "IsChecked";
        private const string COL_Symbol = "Symbol";
        private const string COL_Factset = "FactSetSymbol";
        private const string COL_Activ = "ActivSymbol";
        private const string COL_CompanyName = "CompanyName";
        private const string COL_UnderlyingSymbol = "UnderlyingSymbol";
        private const string COL_Level1Name = "Level1Name";
        private const string COL_MasterFund = "MasterFund";
        private const string COL_Level2Name = "Level2Name";
        private const string COL_AssetName = "AssetName";
        private const string COL_Quantity = "Quantity";
        private const string COL_AvgPrice = "AvgPrice";
        private const string COL_StandardDeviation = "StandardDeviation";
        private const string COL_Risk = "Risk";
        private const string COL_MarginalRisk = "MarginalRisk";
        private const string COL_ComponentRisk = "ComponentRisk";
        private const string COL_Correlation = "Correlation";
        private const string COL_SectorName = "SectorName";
        private const string COL_CountryName = "CountryName";
        private const string COL_UDAAsset = "UDAAsset";
        private const string COL_SecurityTypeName = "SecurityTypeName";
        private const string COL_Volatility = "Volatility";
        private const string COL_AUECLocalDate = "AUECLocalDate";
        private const string COL_CompanyUserName = "CompanyUserName";
        private const string COL_CounterPartyName = "CounterPartyName";
        private const string COL_CurrencyName = "CurrencyName";
        private const string COL_ExchangeName = "ExchangeName";
        private const string COL_ExpirationDate = "ExpirationDate";
        private const string COL_ContractMultiplier = "ContractMultiplier";
        private const string COL_PositionType = "PositionType";
        private const string COL_PSSymbol = "PSSymbol";
        private const string COL_PutOrCall = "PutOrCall";
        private const string COL_SubSectorName = "SubSectorName";
        private const string COL_StrikePrice = "StrikePrice";
        private const string COL_UnderlyingName = "UnderlyingName";
        private const string COL_TradeAttribute1 = "TradeAttribute1";
        private const string COL_TradeAttribute2 = "TradeAttribute2";
        private const string COL_TradeAttribute3 = "TradeAttribute3";
        private const string COL_TradeAttribute4 = "TradeAttribute4";
        private const string COL_TradeAttribute5 = "TradeAttribute5";
        private const string COL_TradeAttribute6 = "TradeAttribute6";
        private const string COL_BloombergSymbol = "BloombergSymbol";
        private const string COL_BloombergSymbolWithExchangeCode = "BloombergSymbolWithExchangeCode";
        private const string COL_IDCOSymbol = "IDCOSymbol";
        private const string COL_ISINSymbol = "ISINSymbol";
        private const string COL_SEDOLSymbol = "SEDOLSymbol";
        private const string COL_OSISymbol = "OSISymbol";
        private const string COL_CusipSymbol = "CusipSymbol";
        private const string COL_Delta = "Delta";
        private const string COL_DeltaAdjPosition = "DeltaAdjPosition";
        #endregion

        public CtrlRiskReports()
        {
            try
            {
                InitializeComponent();
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
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void DisposeProxy()
        {
            try
            {
                if (_riskServiceProxy != null)
                {
                    _riskServiceProxy.Dispose();
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

        public void SetUp(bool isOnlyLoadPositions, ref bool isTabBusy)
        {
            try
            {
                if (isAlreadyStarted && isOnlyLoadPositions)
                {
                    GetData();
                    isTabBusy = true;
                }

                if (!isAlreadyStarted)
                {
                    SetCalculatedRiskTable();
                    SetGridFontSize();
                    if (RiskPreferenceManager.RiskPrefernece.IsAutoLoadDataOnStartup)
                    {
                        GetData();
                        isTabBusy = true;
                    }
                    grdData.DataSource = _riskObjcollection;
                    grdData.DataBind();
                    BindBenchMarkCombo();
                    SetColumns();
                    if (_riskServiceProxy != null)
                    {
                        _riskServiceProxy.Dispose();
                    }
                    CreateRiskServiceProxy();
                    isAlreadyStarted = true;
                    datePickerStartdate.DateTime = DateTime.Now.AddDays(-RiskPreferenceManager.RiskPrefernece._riskReportDateRange);
                    datePickerEnddate.DateTime = DateTime.Now;
                    SetDataTable();
                    LoadPreferences();
                    UpdateHeaderWrapHeader(false);
                    // Sets status on Risk UI status bar
                    if (bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("PortfolioScienceLogoOnRisk")))
                    {
                        lblStatusPortfolioScience.Text = "Intra day VaR/Stress Tests calculated by Portfolio Science";
                    }
                    DisableForm();
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

        /// <summary>
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnCalculateRisk.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnCalculateRisk.ForeColor = System.Drawing.Color.White;
                btnCalculateRisk.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCalculateRisk.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCalculateRisk.UseAppStyling = false;
                btnCalculateRisk.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGraph.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGraph.ForeColor = System.Drawing.Color.White;
                btnGraph.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGraph.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGraph.UseAppStyling = false;
                btnGraph.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        public delegate void MsgreceivedInvokeDelegate(PranaRiskObjColl posList);
        void GetInstance_PositionReceived(PranaRiskObjColl riskObjColl)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MsgreceivedInvokeDelegate msgreceivedInvokeDelegate = new MsgreceivedInvokeDelegate(GetInstance_PositionReceived);
                        this.BeginInvoke(msgreceivedInvokeDelegate, new object[] { riskObjColl });
                    }
                    else
                    {
                        if (riskObjColl != null)
                        {
                            foreach (PranaRiskObj riskObj in riskObjColl)
                            {
                                _riskObjcollection.Add(riskObj);
                                AddPositionToDict(riskObj);
                            }
                            grdData.DataSource = _riskObjcollection;
                            grdData.DataBind();
                            _dtCalculatedRisks.Rows.Clear();
                            toolStripStatusLabel.ForeColor = Color.Green;
                            toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Ready";
                            if (RefreshCompleted != null)
                            {
                                RefreshCompleted(null, null);
                            }
                        }
                        EnableForm();
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

        private void SetDataTable()
        {
            try
            {
                _dataPortfolio.Columns.Add(new DataColumn("Grouping Criterion"));
                _dataPortfolio.Columns.Add(new DataColumn(COL_Risk, typeof(double)));
                _dataPortfolio.Columns.Add(new DataColumn(COL_Correlation, typeof(double)));
                _dataPortfolio.Columns.Add(new DataColumn(COL_StandardDeviation, typeof(double)));

                DataColumn[] pkColArray = new DataColumn[1];
                pkColArray[0] = _dataPortfolio.Columns["Grouping Criterion"];
                _dataPortfolio.PrimaryKey = pkColArray;

                portfolioResultsCtrl1.SetData(_dataPortfolio);
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

        private void SetCalculatedRiskTable()
        {
            try
            {
                _dtCalculatedRisks.Columns.Add(new DataColumn("Grouping Criterion"));
                _dtCalculatedRisks.Columns.Add(new DataColumn(COL_Risk, typeof(double)));
                _dtCalculatedRisks.Columns.Add(new DataColumn(COL_Correlation, typeof(double)));
                _dtCalculatedRisks.Columns.Add(new DataColumn(COL_StandardDeviation, typeof(double)));

                DataColumn[] pkColArray = new DataColumn[1];
                pkColArray[0] = _dtCalculatedRisks.Columns["Grouping Criterion"];
                _dtCalculatedRisks.PrimaryKey = pkColArray;
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

        public void BindBenchMarkCombo()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("BenchMarkSymbol");
                dt.Columns.Add("SymbolDisplayName");
                DataSet ds = PositionDataManager.GetBenchMarks();
                dt = ds.Tables[0];
                cmbbxBenchMark.DataSource = dt;
                cmbbxBenchMark.DisplayLayout.Bands[0].Columns["SymbolDisplayName"].Width = 148;
                cmbbxBenchMark.DisplayLayout.Bands[0].ColHeadersVisible = false;
                cmbbxBenchMark.DisplayLayout.Bands[0].Columns["BenchMarkSymbol"].Hidden = true;
                cmbbxBenchMark.DisplayMember = "SymbolDisplayName";
                cmbbxBenchMark.ValueMember = "BenchMarkSymbol";
                SetDefaultBenchMark(dt);
                cmbbxBenchMark.DataBind();
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

        private void LoadPreferences()
        {
            try
            {
                if (RiskPreferenceManager.RiskPrefernece.RiskReportsTabBenchMarkSymbolName != null)
                    cmbbxBenchMark.Value = RiskPreferenceManager.RiskPrefernece.RiskReportsTabBenchMarkSymbolName;
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

        private void SetDefaultBenchMark(DataTable dt)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    cmbbxBenchMark.Value = dt.Rows[0].ItemArray[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["SymbolDisplayName"].ToString().Contains("S&P"))
                        {
                            cmbbxBenchMark.Value = row.ItemArray[0];
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

        private void GetData()
        {
            try
            {
                toolStripStatusLabel.ForeColor = Color.Red;
                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Refreshing Positions...";
                _dictRiskObjs.Clear();
                _riskObjcollection.Clear();
                _dtCalculatedRisks.Clear();
                GetPositionsforRisk();
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

        public async void GetPositionsforRisk()
        {
            try
            {
                Dictionary<int, double> _dictAccountWiseCash = new Dictionary<int, double>();
                List<Object> riskData = await CentralRiskPositionsManager.GetInstance.GetPSPositionsAsRiskPref(false, _dictAccountWiseCash);
                PranaRiskObjColl riskObjColl = (PranaRiskObjColl)riskData[0];
                GetInstance_PositionReceived(riskObjColl);
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

        private void AddPositionToDict(PranaRiskObj riskObj)
        {
            try
            {
                if (!_dictRiskObjs.ContainsKey(riskObj.Symbol))
                {
                    List<PranaRiskObj> list = new List<PranaRiskObj>();
                    list.Add(riskObj);
                    _dictRiskObjs.Add(riskObj.Symbol, list);
                }
                else
                {
                    _dictRiskObjs[riskObj.Symbol].Add(riskObj);
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

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRequest())
                {
                    lock (_locker)
                    {
                        _responseReceivedCount = 0;
                        _requestedCallsCount = 0;
                    }
                    ClearData();
                    btnCalculateRisk.BackColor = Color.Red;
                    btnCalculateRisk.Text = "Calculating...";
                    toolStripStatusLabel.ForeColor = Color.Red;
                    toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Calculating...";
                    DisableForm();
                    CalculateRisk();
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

        private void UpdateToolStripStatus()
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        MethodInvoker marsheller = new MethodInvoker(UpdateToolStripStatus);
                        this.BeginInvoke(marsheller);
                    }
                    else
                    {
                        toolStripStatusLabel.ForeColor = Color.Green;
                        toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Success";
                        EnableForm();
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

        private void UpdateToolStripStatus(string message)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        UIThreadMarshellerUpdateToolStrip marsheller = new UIThreadMarshellerUpdateToolStrip(UpdateToolStripStatus);
                        this.BeginInvoke(marsheller, new object[] { message });
                    }
                    else
                    {
                        toolStripStatusLabel.ForeColor = Color.Red;
                        toolStripStatusLabel.Text = DateTime.Now.ToString() + ": " + message;
                        EnableForm();
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

        private bool CheckRiskServiceConnected()
        {
            try
            {
                return _riskServiceProxy.InnerChannel.CheckRiskServiceConnected();

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
            return false;
        }

        private void CalculateRisk()
        {
            try
            {
                if (!CheckRiskServiceConnected())
                {
                    UpdateToolStripStatus("Risk Server not connected to Portfolio Science.");
                    return;
                }
                //Updated the risk preferences as the preferences are only kept at the client side in a xml file and
                //the preferences has to be communicated to the pricing server as it is using these in several calculations
                _riskServiceProxy.InnerChannel.UpdateRiskPreferences(RiskPreferenceManager.RiskPrefernece);

                _isRiskCalculatedIndividual = false;
                _componentRiskByGrouping.Clear();
                _startDate = datePickerStartdate.DateTime;
                _endDate = datePickerEnddate.DateTime;
                PranaRiskObjColl selectedRows = GetSelectedRows();

                UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];

                _riskParams.IsMarginalRiskRequired = gridBand.Columns["MarginalRisk"].IsVisibleInLayout ? true : false;
                _riskParams.IsComponentRiskRequired = gridBand.Columns["ComponentRisk"].IsVisibleInLayout ? true : false;
                _riskParams.IsstddevRequired = gridBand.Columns["StandardDeviation"].IsVisibleInLayout ? true : false;
                _riskParams.IsCorrelationRequired = gridBand.Columns["Correlation"].IsVisibleInLayout ? true : false;
                _riskParams.IsVolatilityRequired = gridBand.Columns["Volatility"].IsVisibleInLayout ? true : false;
                _riskParams.IsRiskRequired = gridBand.Columns["Risk"].IsVisibleInLayout ? true : false;

                #region Individual Risk
                PranaRequestCarrier pranaRequestCarrier = new PranaRequestCarrier(selectedRows, _startDate, _endDate, cmbbxBenchMark.Value.ToString(), "1", 0, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, false, _riskParams);
                pranaRequestCarrier.VolatilityType = (int)RiskPreferenceManager.RiskPrefernece.VolatilityType;
                pranaRequestCarrier.GroupingName = "Portfolio";
                CalculateRiskIndividualData(pranaRequestCarrier);
                #endregion

                #region Group Risk
                if (!string.IsNullOrEmpty(_firstLevelGroup))
                {
                    object[] arguments = new object[2];
                    arguments[0] = cmbbxBenchMark.Value.ToString();

                    _isRiskCalculatedGroup = false;
                    _dtCalculatedRisks.Rows.Clear();
                    Dictionary<string, PranaRiskObjColl> dictGroupedRiskColl = new Dictionary<string, PranaRiskObjColl>();
                    PranaRiskObjColl groupedRiskColl;
                    foreach (UltraGridRow groupRow in grdData.Rows)
                    {
                        if (!groupRow.IsFilteredOut)
                        {
                            UltraGridGroupByRow groupByRow = (UltraGridGroupByRow)groupRow;
                            if (!string.IsNullOrEmpty(_secondLevelGroup))
                            {
                                foreach (UltraGridRow groupRowSecondLevel in groupRow.ChildBands[0].Rows)
                                {
                                    if (!groupRowSecondLevel.IsFilteredOut)
                                    {
                                        UltraGridGroupByRow groupByRowSecondLevel = (UltraGridGroupByRow)groupRowSecondLevel;

                                        groupedRiskColl = RequestCollection(groupRowSecondLevel);
                                        if (groupedRiskColl.Count > 0 && !dictGroupedRiskColl.ContainsKey(groupByRow.Value.ToString() + "|" + groupByRowSecondLevel.Value.ToString()))
                                        {
                                            dictGroupedRiskColl.Add(groupByRow.Value.ToString() + "|" + groupByRowSecondLevel.Value.ToString(), groupedRiskColl);
                                        }
                                    }
                                }
                            }

                            groupedRiskColl = RequestCollection(groupRow);
                            if (groupedRiskColl.Count > 0 && !dictGroupedRiskColl.ContainsKey(_firstLevelGroup + "|" + groupByRow.Value.ToString()))
                            {
                                dictGroupedRiskColl.Add(_firstLevelGroup + "|" + groupByRow.Value.ToString(), groupedRiskColl);
                            }
                        }
                    }
                    lock (_locker)
                    {
                        _requestedCallsCount = _requestedCallsCount + dictGroupedRiskColl.Count;
                    }
                    arguments[1] = dictGroupedRiskColl;
                    CalculateRiskGroupData(arguments);
                }
                #endregion
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

        async void CalculateRiskIndividualData(PranaRequestCarrier pranaRequestCarrier)
        {
            try
            {
                if (pranaRequestCarrier.IndividualSymbolList.Count > 0)
                {
                    lock (_locker)
                    {
                        _requestedCallsCount = _requestedCallsCount + 1;
                    }
                    var pranaRequestCarrierTask = _riskServiceProxy.InnerChannel.CalculateRiskRelatedData(pranaRequestCarrier, false).ContinueWith(CompleteWorkAfterRiskResponse);
                    await pranaRequestCarrierTask;
                    _isRiskCalculatedIndividual = true;
                    if (_isRiskCalculatedGroup && _isRiskCalculatedIndividual)
                    {
                        UpdateToolStripStatus();
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

        async void CalculateRiskGroupData(object[] argments)
        {
            try
            {
                if (argments != null)
                {
                    string benchMarkSymbol = argments[0].ToString();
                    Dictionary<string, PranaRiskObjColl> dictGroupedRiskColl = (Dictionary<string, PranaRiskObjColl>)argments[1];
                    resetEvent = new ManualResetEvent(false);
                    List<System.Threading.Tasks.Task> tasks = new List<System.Threading.Tasks.Task>(dictGroupedRiskColl.Count);
                    if (!_isRiskCalculatedGroup)
                    {
                        foreach (KeyValuePair<string, PranaRiskObjColl> kp in dictGroupedRiskColl)
                        {
                            PranaRequestCarrier pranaRequestCarrierGroup = new PranaRequestCarrier(kp.Value, _startDate, _endDate, benchMarkSymbol, "1", 0, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, false, _riskParams);
                            pranaRequestCarrierGroup.VolatilityType = (int)RiskPreferenceManager.RiskPrefernece.VolatilityType;
                            pranaRequestCarrierGroup.GroupingName = kp.Key.ToString();
                            var pranaRequestCarrierGroupTask = _riskServiceProxy.InnerChannel.CalculateRiskRelatedData(pranaRequestCarrierGroup, false).ContinueWith(CompleteWorkAfterRiskResponse);
                            tasks.Add(pranaRequestCarrierGroupTask);
                        }
                        await System.Threading.Tasks.Task.WhenAll(tasks.ToArray());
                        _isRiskCalculatedGroup = true;
                    }
                    if (_isRiskCalculatedGroup && _isRiskCalculatedIndividual)
                    {
                        UpdateToolStripStatus();
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

        private void CompleteWorkAfterRiskResponse(System.Threading.Tasks.Task<PranaRequestCarrier> completedTask)
        {
            try
            {
                lock (_locker)
                {
                    _responseReceivedCount++;
                    _requestedCallsCount--;
                }
                if (!completedTask.IsFaulted)
                {
                    int percentageCompletion = (_requestedCallsCount + _responseReceivedCount) <= 0
                         ? 0 : (_responseReceivedCount * 100 / (_requestedCallsCount + _responseReceivedCount));
                    toolStripStatusLabel.Text = DateTime.Now + ": " + percentageCompletion + "% Completed";
                    RiskResponseCompleted(completedTask.Result);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in completedTask.Exception.InnerExceptions)
                    {
                        sb.Append(item.Message + " ");
                    }
                    _isRiskCalculatedIndividual = false;
                    _isRiskCalculatedGroup = false;
                    UpdateToolStripStatus(DateTime.Now + ": " + sb.ToString());
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
        }

        private PranaRiskObjColl RequestCollection(UltraGridRow groupRow)
        {
            PranaRiskObjColl groupedRiskColl = new PranaRiskObjColl();
            UltraGridRow[] rows = groupRow.ChildBands[0].Rows.GetFilteredInNonGroupByRows();
            try
            {
                foreach (UltraGridRow row in rows)
                {
                    PranaRiskObj riskObjIndiv = (PranaRiskObj)row.ListObject;
                    if (riskObjIndiv.IsChecked)
                    {
                        groupedRiskColl.Add((PranaRiskObj)riskObjIndiv.Clone());
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return groupedRiskColl;
        }

        void RiskResponseCompleted(PranaRequestCarrier pranaRequestCarrier)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        UIThreadMarshellers marsheller = new UIThreadMarshellers(RiskResponseCompleted);
                        this.BeginInvoke(marsheller, new object[] { pranaRequestCarrier });
                    }
                    else
                    {
                        if (pranaRequestCarrier != null)
                        {
                            if (pranaRequestCarrier.GroupingName == "Portfolio")
                            {
                                DataRow dr = _dataPortfolio.NewRow();
                                dr["Grouping Criterion"] = pranaRequestCarrier.GroupingName;
                                dr[COL_Correlation] = pranaRequestCarrier.Correlation;
                                dr[COL_Risk] = Math.Round(pranaRequestCarrier.PortfolioRisk);
                                dr[COL_StandardDeviation] = Math.Round(pranaRequestCarrier.StandardDeviation);
                                AddComponentRiskDict(pranaRequestCarrier);
                                _dataPortfolio.Rows.Add(dr);
                                dr.AcceptChanges();
                                portfolioResultsCtrl1.Refresh();

                                if (string.IsNullOrEmpty(_firstLevelGroup))
                                    UpdateGridRows(pranaRequestCarrier);
                            }
                            else
                            {
                                DataRow dr = _dtCalculatedRisks.NewRow();
                                dr["Grouping Criterion"] = pranaRequestCarrier.GroupingName;
                                dr[COL_Risk] = Math.Round(pranaRequestCarrier.PortfolioRisk);
                                dr[COL_Correlation] = pranaRequestCarrier.Correlation;
                                dr[COL_StandardDeviation] = pranaRequestCarrier.StandardDeviation;
                                AddComponentRiskDict(pranaRequestCarrier);
                                _dtCalculatedRisks.Rows.Add(dr);
                                dr.AcceptChanges();

                                if (string.IsNullOrEmpty(_secondLevelGroup) || (!string.IsNullOrEmpty(_secondLevelGroup) && !pranaRequestCarrier.GroupingName.Contains(_firstLevelGroup + "|")))
                                    UpdateGridRows(pranaRequestCarrier);
                            }
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

        private void AddComponentRiskDict(PranaRequestCarrier pranaRequestCarrier)
        {
            try
            {
                Dictionary<string, ComponentRiskData> symbolWiseComponentRisk = new Dictionary<string, ComponentRiskData>();
                foreach (KeyValuePair<string, PranaRiskResult> riskReply in pranaRequestCarrier.IndividualSymbolList)
                {
                    ComponentRiskData componentRiskData = new ComponentRiskData();
                    componentRiskData.ComponentRisk = riskReply.Value.ComponentRisk;
                    componentRiskData.Quantity = riskReply.Value.Quantity;
                    symbolWiseComponentRisk.Add(riskReply.Key, componentRiskData);
                }
                _componentRiskByGrouping.Add(pranaRequestCarrier.GroupingName, symbolWiseComponentRisk);
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

        private void UpdateGridRows(PranaRequestCarrier pranaRequestCarrier)
        {
            try
            {
                foreach (KeyValuePair<string, PranaRiskResult> riskReply in pranaRequestCarrier.IndividualSymbolList)
                {
                    int symbolLen = riskReply.Value.Symbol.Length;
                    string positionType = riskReply.Key.Substring(symbolLen);
                    List<PranaRiskObj> list = _dictRiskObjs[riskReply.Value.Symbol];
                    foreach (PranaRiskObj riskObjToUpdate in list)
                    {
                        if (riskObjToUpdate.IsChecked && riskObjToUpdate.PositionType.Equals(positionType))// && riskReply.Value.Quantity == riskObjToUpdate.Quantity)
                        {
                            riskObjToUpdate.SetRiskData(riskReply.Value, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdData_ExternalSummaryValueRequested(object sender, ExternalSummaryValueEventArgs e)
        {
            try
            {
                if (e.SummaryValue.SummarySettings.SourceColumn.Key == COL_Risk || e.SummaryValue.SummarySettings.SourceColumn.Key == COL_StandardDeviation || e.SummaryValue.SummarySettings.SourceColumn.Key == COL_Correlation)
                {
                    DataRow dr;
                    Infragistics.Win.UltraWinGrid.RowsCollection rows = e.SummaryValue.ParentRows;
                    if (rows != null && rows.ParentRow != null && _dtCalculatedRisks.Columns.Contains("Grouping Criterion"))
                    {
                        string groupByValue = ((Infragistics.Win.UltraWinGrid.UltraGridGroupByRow)(rows.ParentRow)).ValueAsDisplayText;

                        if (!string.IsNullOrEmpty(_secondLevelGroup) && rows.ParentRow.HasParent())
                        {
                            string parentGroupByValue = ((Infragistics.Win.UltraWinGrid.UltraGridGroupByRow)(rows.ParentRow.ParentRow)).ValueAsDisplayText;
                            dr = _dtCalculatedRisks.Rows.Find(parentGroupByValue + "|" + groupByValue);
                        }
                        else
                        {
                            dr = _dtCalculatedRisks.Rows.Find(_firstLevelGroup + "|" + groupByValue);
                        }

                        if (dr != null && _dtCalculatedRisks.Columns.Contains(e.SummaryValue.SummarySettings.SourceColumn.Key))
                            e.SummaryValue.SetExternalSummaryValue(dr[e.SummaryValue.SummarySettings.SourceColumn.Key]);
                        else
                            e.SummaryValue.SetExternalSummaryValue(0);
                    }
                    else
                    {
                        if (rows != null && rows.ParentRow == null && _dataPortfolio.Columns.Contains("Grouping Criterion"))
                        {
                            dr = _dataPortfolio.Rows.Find("Portfolio");

                            if (dr != null && _dataPortfolio.Columns.Contains(e.SummaryValue.SummarySettings.SourceColumn.Key))
                                e.SummaryValue.SetExternalSummaryValue(dr[e.SummaryValue.SummarySettings.SourceColumn.Key]);
                            else
                                e.SummaryValue.SetExternalSummaryValue(0);
                        }
                        else
                        {
                            e.SummaryValue.SetExternalSummaryValue(0);
                        }
                    }
                }
                else if (e.SummaryValue.SummarySettings.SourceColumn.Key == COL_ComponentRisk)
                {
                    Infragistics.Win.UltraWinGrid.RowsCollection rows = e.SummaryValue.ParentRows;
                    if (rows != null && rows.ParentRow != null)
                    {
                        string groupByValue = ((Infragistics.Win.UltraWinGrid.UltraGridGroupByRow)(rows.ParentRow)).ValueAsDisplayText;

                        if (!string.IsNullOrEmpty(_secondLevelGroup) && rows.ParentRow.HasParent())
                        {
                            string parentGroupByValue = ((Infragistics.Win.UltraWinGrid.UltraGridGroupByRow)(rows.ParentRow.ParentRow)).ValueAsDisplayText;

                            if (_componentRiskByGrouping.ContainsKey(parentGroupByValue + "|" + groupByValue))
                            {
                                Dictionary<string, ComponentRiskData> symbolWiseComponentRisk = _componentRiskByGrouping[parentGroupByValue + "|" + groupByValue];
                                double componentRisk = 0;
                                foreach (string key in symbolWiseComponentRisk.Keys)
                                {
                                    if (_componentRiskByGrouping.ContainsKey(_firstLevelGroup + "|" + parentGroupByValue))
                                    {
                                        if (_componentRiskByGrouping[_firstLevelGroup + "|" + parentGroupByValue].ContainsKey(key))
                                        {
                                            ComponentRiskData componentRiskData = _componentRiskByGrouping[_firstLevelGroup + "|" + parentGroupByValue][key];
                                            if (componentRiskData.Quantity != 0)
                                            {
                                                componentRisk += componentRiskData.ComponentRisk * symbolWiseComponentRisk[key].Quantity / componentRiskData.Quantity;
                                            }
                                        }
                                    }
                                }
                                e.SummaryValue.SetExternalSummaryValue(componentRisk);
                            }
                            else
                            {
                                e.SummaryValue.SetExternalSummaryValue(0);
                            }
                        }
                        else if (!string.IsNullOrEmpty(_firstLevelGroup) && _componentRiskByGrouping.ContainsKey(_firstLevelGroup + "|" + groupByValue) && _componentRiskByGrouping.ContainsKey("Portfolio"))
                        {
                            Dictionary<string, ComponentRiskData> symbolWiseComponentRisk = _componentRiskByGrouping[_firstLevelGroup + "|" + groupByValue];
                            double componentRisk = 0;
                            foreach (string key in symbolWiseComponentRisk.Keys)
                            {
                                if (_componentRiskByGrouping["Portfolio"].ContainsKey(key))
                                {
                                    ComponentRiskData componentRiskData = _componentRiskByGrouping["Portfolio"][key];
                                    if (componentRiskData.Quantity != 0)
                                    {
                                        componentRisk += componentRiskData.ComponentRisk * symbolWiseComponentRisk[key].Quantity / componentRiskData.Quantity;
                                    }
                                }
                            }
                            e.SummaryValue.SetExternalSummaryValue(componentRisk);
                        }
                        else
                        {
                            e.SummaryValue.SetExternalSummaryValue(0);
                        }
                    }
                    else
                    {
                        if (rows != null && rows.ParentRow == null && _componentRiskByGrouping.ContainsKey("Portfolio"))
                        {
                            Dictionary<string, ComponentRiskData> symbolWiseComponentRisk = _componentRiskByGrouping["Portfolio"];
                            double componentRisk = 0;
                            foreach (ComponentRiskData componentRiskData in symbolWiseComponentRisk.Values)
                            {
                                componentRisk += componentRiskData.ComponentRisk;
                            }

                            e.SummaryValue.SetExternalSummaryValue(componentRisk);
                        }
                        else
                        {
                            e.SummaryValue.SetExternalSummaryValue(0);
                        }
                    }
                }
                else
                {
                    e.SummaryValue.SetExternalSummaryValue(0);
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void DisableForm()
        {
            try
            {
                cmbbxBenchMark.Enabled = false;
                btnCalculateRisk.Enabled = false;
                datePickerStartdate.Enabled = false;
                datePickerEnddate.Enabled = false;
                btnGraph.Enabled = false;

                //When we disable Grid then Summary of summary rows got cleared so we are not disabling grid (disabling only editable columns)
                //grdData.Enabled = false;
                grdData.DisplayLayout.Bands[0].Columns[COL_IsChecked].CellActivation = Activation.Disabled;
                grdData.CreationFilter = null;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void EnableForm()
        {
            try
            {
                grdData.ResumeRowSynchronization();
                grdData.ResumeSummaryUpdates(true);
                grdData.Rows.Refresh(RefreshRow.ReloadData);

                btnCalculateRisk.BackColor = Color.FromArgb(104, 156, 46);
                btnCalculateRisk.Text = "Calculate Risk";
                cmbbxBenchMark.Enabled = true;
                btnCalculateRisk.Enabled = true;
                datePickerStartdate.Enabled = true;
                datePickerEnddate.Enabled = true;
                btnGraph.Enabled = true;

                grdData.DisplayLayout.Bands[0].Columns[COL_IsChecked].CellActivation = Activation.AllowEdit;
                grdData.CreationFilter = headerCheckBox;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void RefreshPositions()
        {
            try
            {
                DisableForm();
                ClearData();
                toolStripStatusLabel.ForeColor = Color.Red;
                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Refreshing Positions...";
                GetData();
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

        private delegate void UIThreadMarsheller(PranaRequestCarrier pranaRequestCarrier);
        private delegate void UIThreadMarshellerForComplete(QueueMessage qMsg);

        private bool ValidateRequest()
        {
            try
            {
                if (GetSelectedRows().Count < 1)
                {
                    MessageBox.Show("Select some rows !", "Risk Analytics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (DateTime.Compare(datePickerStartdate.DateTime, datePickerEnddate.DateTime) == 0)
                {
                    MessageBox.Show("Please select a Date Range", "Risk Analytics", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                if (cmbbxBenchMark.Value == null)
                {
                    return false;
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
            return true;
        }

        private PranaRiskObjColl GetSelectedRows()
        {
            PranaRiskObjColl riskObjcollection = new PranaRiskObjColl();
            try
            {
                UltraGridRow[] rows = grdData.Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells[COL_IsChecked].Text.ToUpper().Equals("TRUE"))
                    {
                        PranaRiskObj pranaPosWithGreeks = (PranaRiskObj)row.ListObject;
                        riskObjcollection.Add(pranaPosWithGreeks);
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
            return riskObjcollection;
        }

        private void SetColumns()
        {
            try
            {
                if (RiskLayoutManager.RiskLayout.RiskReportColumns.Count > 0)
                {
                    LoadColumnsFromXML();
                }
                else
                {
                    LoadColumns();
                }
                SetColumnFormatting();
                SetColumnCustomizations();
                SetColumnSummaries(grdData);
                SetGroupByColumns();
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

        private void LoadColumnsFromXML()
        {
            try
            {
                List<ColumnData> listColData = RiskLayoutManager.RiskLayout.RiskReportColumns;
                List<SortedColumnData> listGroupByColumnsCollection = RiskLayoutManager.RiskLayout.RiskReportGroupByColumnsCollection;
                RiskLayoutManager.SetGridColumnLayout(grdData, listColData, listGroupByColumnsCollection, GetAllDisplayableColumns());
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

        private void LoadColumns()
        {
            List<string> colAll = GetAllDisplayableColumns();
            List<string> colDefault = GetAllDefaultColumns();
            List<string> colVisible = Prana.Utilities.MiscUtilities.GeneralUtilities.GetListFromString(RiskPreferenceManager.RiskPrefernece.RiskReportColumns, ',');
            try
            {
                if (colVisible.Count < 1) // PrefFile Has No Columns
                {
                    colVisible.AddRange(colDefault);
                }
                ColumnsCollection gridColumns = grdData.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    gridCol.Hidden = true;
                    if (!colAll.Contains(gridCol.Key))
                    {
                        gridCol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                }
                int visiblePos = 1;
                foreach (string col in colVisible)
                {
                    if (!String.IsNullOrEmpty(col) && gridColumns.Exists(col))
                    {
                        gridColumns[col].Hidden = false;
                        gridColumns[col].Header.VisiblePosition = visiblePos;
                        gridColumns[col].Width = 100;
                        visiblePos++;
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

        private List<string> GetAllDisplayableColumns()
        {
            List<string> colAll = new List<string>();
            try
            {
                List<string> colDefault = GetAllDefaultColumns();
                colAll.AddRange(colDefault);
                colAll.Add(COL_AUECLocalDate);
                colAll.Add(COL_CompanyUserName);
                colAll.Add(COL_CounterPartyName);
                colAll.Add(COL_CurrencyName);
                colAll.Add(COL_ExchangeName);
                colAll.Add(COL_ExpirationDate);
                colAll.Add(COL_ContractMultiplier);
                colAll.Add(COL_PositionType);
                colAll.Add(COL_PSSymbol);
                colAll.Add(COL_PutOrCall);
                colAll.Add(COL_SubSectorName);
                colAll.Add(COL_StrikePrice);
                colAll.Add(COL_UnderlyingName);
                colAll.Add(COL_TradeAttribute1);
                colAll.Add(COL_TradeAttribute2);
                colAll.Add(COL_TradeAttribute3);
                colAll.Add(COL_TradeAttribute4);
                colAll.Add(COL_TradeAttribute5);
                colAll.Add(COL_TradeAttribute6);
                colAll.Add(COL_BloombergSymbol);
                colAll.Add(COL_BloombergSymbolWithExchangeCode);
                colAll.Add(COL_Factset);
                colAll.Add(COL_Activ);
                colAll.Add(COL_IDCOSymbol);
                colAll.Add(COL_ISINSymbol);
                colAll.Add(COL_SEDOLSymbol);
                colAll.Add(COL_OSISymbol);
                colAll.Add(COL_CusipSymbol);
                colAll.Add(COL_Delta);
                colAll.Add(COL_DeltaAdjPosition);
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
            return colAll;
        }

        private List<string> GetAllDefaultColumns()
        {
            List<string> colDefault = new List<string>();
            try
            {
                colDefault.Add(COL_IsChecked);
                colDefault.Add(COL_Symbol);
                colDefault.Add(COL_Factset);
                colDefault.Add(COL_Activ);
                colDefault.Add(COL_CompanyName);
                colDefault.Add(COL_UnderlyingSymbol);
                colDefault.Add(COL_Level1Name);
                colDefault.Add(COL_MasterFund);
                colDefault.Add(COL_Level2Name);
                colDefault.Add(COL_AssetName);
                colDefault.Add(COL_Quantity);
                colDefault.Add(COL_AvgPrice);
                colDefault.Add(COL_StandardDeviation);
                colDefault.Add(COL_Risk);
                colDefault.Add(COL_MarginalRisk);
                colDefault.Add(COL_ComponentRisk);
                colDefault.Add(COL_Correlation);
                colDefault.Add(COL_SectorName);
                colDefault.Add(COL_CountryName);
                colDefault.Add(COL_UDAAsset);
                colDefault.Add(COL_SecurityTypeName);
                colDefault.Add(COL_Volatility);
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
            return colDefault;
        }

        private void SetColumnFormatting()
        {
            try
            {
                ColumnsCollection columns = grdData.DisplayLayout.Bands[0].Columns;
                columns[COL_Quantity].Format = "#,#.#";
                columns[COL_AvgPrice].Format = "#.00";
                columns[COL_Correlation].Format = "#.0000";
                columns[COL_Volatility].Format = "#.0000";
                columns[COL_Delta].Format = "#.0000";
                columns[COL_DeltaAdjPosition].Format = "#,#.#";
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetColumnCustomizations()
        {
            try
            {
                grdData.CreationFilter = headerCheckBox;
                UltraGridBand band = grdData.DisplayLayout.Bands[0];
                band.Columns[COL_IsChecked].CellClickAction = CellClickAction.Edit;
                band.Columns[COL_IsChecked].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                band.Columns[COL_IsChecked].Header.Caption = "";
                band.Columns[COL_IsChecked].Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                band.Columns[COL_IsChecked].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[COL_IsChecked].AllowGroupBy = DefaultableBoolean.False;
                band.Columns[COL_PutOrCall].CellActivation = Activation.NoEdit;
                band.Columns[COL_Level1Name].Header.Caption = "Account";
                band.Columns[COL_MasterFund].Header.Caption = "Master Fund";
                band.Columns[COL_Level2Name].Header.Caption = "Strategy";
                band.Columns[COL_AssetName].Header.Caption = "Asset";
                band.Columns[COL_AUECLocalDate].Header.Caption = "Trade Date";
                band.Columns[COL_AvgPrice].Header.Caption = "Cost Basis (Local)";
                band.Columns[COL_CompanyUserName].Header.Caption = "User";
                band.Columns[COL_CounterPartyName].Header.Caption = ApplicationConstants.CONST_BROKER;
                band.Columns[COL_CountryName].Header.Caption = "Country";
                band.Columns[COL_CurrencyName].Header.Caption = "Currency";
                band.Columns[COL_ExchangeName].Header.Caption = "Exchange";
                band.Columns[COL_ExpirationDate].Header.Caption = "Expiration Date";
                band.Columns[COL_PositionType].Header.Caption = "Position Type";
                band.Columns[COL_SectorName].Header.Caption = "Sector";
                band.Columns[COL_CompanyName].Header.Caption = "Security Name";
                band.Columns[COL_SecurityTypeName].Header.Caption = "Security Type";
                band.Columns[COL_StandardDeviation].Header.Caption = "Standard Deviation";
                band.Columns[COL_StrikePrice].Header.Caption = "Strike Price";
                band.Columns[COL_SubSectorName].Header.Caption = "Sub Sector";
                band.Columns[COL_UnderlyingName].Header.Caption = "Underlying";
                band.Columns[COL_UnderlyingSymbol].Header.Caption = "Underlying Symbol";
                band.Columns[COL_PSSymbol].Header.Caption = "PS Symbol";
                band.Columns[COL_PutOrCall].Header.Caption = "Put/Call";
                band.Columns[COL_MarginalRisk].Header.Caption = "Marginal Risk";
                band.Columns[COL_ComponentRisk].Header.Caption = "Component Risk";
                band.Columns[COL_AUECLocalDate].CellActivation = Activation.NoEdit;
                band.Columns[COL_ExpirationDate].CellActivation = Activation.NoEdit;
                band.Columns[COL_ContractMultiplier].Header.Caption = "Multiplier";

                band.Columns[COL_TradeAttribute1].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 1");
                band.Columns[COL_TradeAttribute2].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 2");
                band.Columns[COL_TradeAttribute3].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 3");
                band.Columns[COL_TradeAttribute4].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 4");
                band.Columns[COL_TradeAttribute5].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 5");
                band.Columns[COL_TradeAttribute6].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 6");

                band.Columns[COL_BloombergSymbol].Header.Caption = "Bloomberg";
                band.Columns[COL_BloombergSymbolWithExchangeCode].Header.Caption = "Bloomberg Symbol(with Exchange Code)";
                band.Columns[COL_Factset].Header.Caption = "FactSet Symbol";
                band.Columns[COL_Activ].Header.Caption = "ACTIV Symbol";
                band.Columns[COL_IDCOSymbol].Header.Caption = "IDCO";
                band.Columns[COL_ISINSymbol].Header.Caption = "ISIN";
                band.Columns[COL_SEDOLSymbol].Header.Caption = "SEDOL";
                band.Columns[COL_OSISymbol].Header.Caption = "OSI";
                band.Columns[COL_CusipSymbol].Header.Caption = "CUSIP";
                band.Columns[COL_UDAAsset].Header.Caption = "User Asset";
                band.Columns[COL_DeltaAdjPosition].Header.Caption = "Delta Position";
                band.Columns[COL_Volatility].Header.Caption = "Volatility (%)";
                band.Columns[COL_Quantity].Header.Caption = "Position";
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetColumnSummaries(UltraGrid grdTemp)
        {
            try
            {
                UltraGridBand band = grdTemp.DisplayLayout.Bands[0];
                foreach (SummarySettings summary in band.Summaries)
                {
                    summary.DisplayFormat = "{0}";
                }

                RiskSummaryFactory summFactory = new RiskSummaryFactory();
                foreach (UltraGridColumn ultraGridColumn in band.Columns)
                {
                    switch (ultraGridColumn.Key)
                    {
                        case COL_Symbol:
                        case COL_Factset:
                        case COL_Activ:
                        case COL_CompanyName:
                        case COL_UnderlyingSymbol:
                        case COL_Level1Name:
                        case COL_MasterFund:
                        case COL_Level2Name:
                        case COL_AssetName:
                        case COL_SectorName:
                        case COL_CountryName:
                        case COL_UDAAsset:
                        case COL_SecurityTypeName:
                        case COL_CompanyUserName:
                        case COL_CounterPartyName:
                        case COL_CurrencyName:
                        case COL_ExchangeName:
                        case COL_PositionType:
                        case COL_PSSymbol:
                        case COL_PutOrCall:
                        case COL_SubSectorName:
                        case COL_UnderlyingName:
                        case COL_TradeAttribute1:
                        case COL_TradeAttribute2:
                        case COL_TradeAttribute3:
                        case COL_TradeAttribute4:
                        case COL_TradeAttribute5:
                        case COL_TradeAttribute6:
                        case COL_BloombergSymbol:
                        case COL_BloombergSymbolWithExchangeCode:
                        case COL_IDCOSymbol:
                        case COL_ISINSymbol:
                        case COL_SEDOLSymbol:
                        case COL_OSISymbol:
                        case COL_CusipSymbol:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_DeltaAdjPosition:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSymbolSum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,0}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_Delta:
                        case COL_ContractMultiplier:
                        case COL_Volatility:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#.0000}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_StrikePrice:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,#.##}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_MarginalRisk:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,0.#}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_Quantity:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSymbolSum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,0.#}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_AvgPrice:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcWeightedSum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,#.0000}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_AUECLocalDate:
                        case COL_ExpirationDate:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:MM/dd/yyyy}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_Risk:
                        case COL_StandardDeviation:
                        case COL_ComponentRisk:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.External, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,0}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_Correlation:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.External, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,#.0000}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;
                    }
                }
                grdTemp.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.Bottom;
                grdTemp.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.InGroupByRows;
                grdTemp.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.RootRowsFootersOnly;
                grdTemp.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
                grdTemp.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.CheckOnDisplay;
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

        private List<string> GetAllowedGroupByColumns()
        {
            List<string> allowedGroupByColumns = new List<string>();
            try
            {
                allowedGroupByColumns.Add(COL_Level1Name);
                allowedGroupByColumns.Add(COL_Level2Name);
                allowedGroupByColumns.Add(COL_Symbol);
                allowedGroupByColumns.Add(COL_Factset);
                allowedGroupByColumns.Add(COL_Activ);
                allowedGroupByColumns.Add(COL_SectorName);
                allowedGroupByColumns.Add(COL_CountryName);
                allowedGroupByColumns.Add(COL_SecurityTypeName);
                allowedGroupByColumns.Add(COL_MasterFund);
                allowedGroupByColumns.Add(COL_TradeAttribute1);
                allowedGroupByColumns.Add(COL_TradeAttribute2);
                allowedGroupByColumns.Add(COL_TradeAttribute3);
                allowedGroupByColumns.Add(COL_TradeAttribute4);
                allowedGroupByColumns.Add(COL_TradeAttribute5);
                allowedGroupByColumns.Add(COL_TradeAttribute6);
                allowedGroupByColumns.Add(COL_UnderlyingSymbol);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return allowedGroupByColumns;
        }

        private void SetGroupByColumns()
        {
            ColumnsCollection columns = grdData.DisplayLayout.Bands[0].Columns;
            try
            {
                foreach (UltraGridColumn column in columns)
                {
                    column.AllowGroupBy = DefaultableBoolean.False;
                    if (GetAllowedGroupByColumns().Contains(column.Key))
                    {
                        column.AllowGroupBy = DefaultableBoolean.True;
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdData_AfterRowFilterChanged(object sender, AfterRowFilterChangedEventArgs e)
        {
            try
            {
                ClearData();
                if ((e.Column.Key.Equals(COL_AUECLocalDate) || e.Column.Key.Equals(COL_ExpirationDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdData.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdData.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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

        private void ClearData()
        {
            try
            {
                _dataPortfolio.Rows.Clear();
                portfolioResultsCtrl1.Refresh();
                _dtCalculatedRisks.Rows.Clear();
                _componentRiskByGrouping.Clear();
                toolStripStatusLabel.ForeColor = Color.Black;
                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Data Cleared";
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

        void GetInstance_RiskSimulationCompleted(QueueMessage qMsg)
        {
            try
            {
                UIThreadMarshellerForComplete mi = new UIThreadMarshellerForComplete(GetInstance_RiskSimulationCompleted);
                if (UIValidation.GetInstance().validate(grdData))
                {
                    if (grdData.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { qMsg });
                    }
                    else
                    {
                        string reqID = qMsg.RequestID;
                        if (_listRequest.Contains(reqID))
                        {
                            if (qMsg.Message.ToString() != string.Empty)
                            {
                                toolStripStatusLabel.ForeColor = Color.Red;
                                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": " + qMsg.Message.ToString();
                            }
                            _listRequest.Remove(reqID);
                        }
                        if (_listRequest.Count == 0)
                        {
                            if (qMsg.Message.ToString() == string.Empty)
                            {
                                toolStripStatusLabel.ForeColor = Color.Green;
                                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Success";
                            }
                            EnableForm();
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

        private void btnGraph_Click(object sender, EventArgs e)
        {
            try
            {
                RiskReportGraphUI riskGraph = new RiskReportGraphUI();
                if (_dtCalculatedRisks.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("Grouping Criterion"));
                    dt.Columns.Add(new DataColumn("Risk", typeof(double)));


                    foreach (DataRow dr in _dtCalculatedRisks.Rows)
                    {
                        DataRow drTemp = dt.NewRow();

                        string levelOfGrouping = dr["Grouping Criterion"].ToString().Split('|')[0].Trim();
                        string groupingName = dr["Grouping Criterion"].ToString().Split('|')[1].Trim();

                        if (levelOfGrouping.Equals(_firstLevelGroup))
                        {
                            drTemp["Grouping Criterion"] = groupingName;
                            drTemp["Risk"] = dr[COL_Risk];
                            dt.Rows.Add(drTemp);
                            dt.AcceptChanges();
                        }
                    }
                    riskGraph.SetUp(dt);
                    riskGraph.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show("No Results to Display !", "Risk Analytics", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void saveColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RiskPrefernece riskPrefs = RiskPreferenceManager.RiskPrefernece;
                riskPrefs.RiskReportColumns = UltraWinGridUtils.GetColumnsString(grdData);
                riskPrefs.RiskReportsTabBenchMarkSymbolName = cmbbxBenchMark.Value.ToString();
                RiskPreferenceManager.SavePreferences(riskPrefs);

                RiskLayoutManager.RiskLayout.RiskReportColumns = RiskLayoutManager.GetGridColumnLayout(grdData);
                RiskLayoutManager.RiskLayout.RiskReportGroupByColumnsCollection = RiskLayoutManager.GetGridGroupByColumnLayout(grdData);
                RiskLayoutManager.SaveRiskLayout();
                toolStripStatusLabel.ForeColor = Color.Green;
                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Layout Saved";
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

        public void ExportToExcel(string filename)
        {
            try
            {
                List<UltraGrid> lstGrids = new List<UltraGrid>();
                lstGrids.Add(grdData);
                lstGrids.Add(portfolioResultsCtrl1.grdData);
                ExcelAndPrintUtilities excelUtils = new ExcelAndPrintUtilities();
                excelUtils.SetExcelLayoutAndWriteForRisk(lstGrids, true, filename);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void grdData_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
        {
            try
            {
                if (e.Column.Key.Equals(COL_AUECLocalDate) || e.Column.Key.Equals(COL_ExpirationDate))
                {
                    e.ValueList.ValueListItems.Insert(4, "(Today)", "(Today)");
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

        private void grdData_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdData);
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

        private void mnuClearFilters_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ColumnFilter colFilters in grdData.DisplayLayout.Bands[0].ColumnFilters)
                {
                    colFilters.ClearFilterConditions();
                }
                toolStripStatusLabel.ForeColor = Color.Green;
                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Filters Cleared";
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

        public void UpdateGridAsPref()
        {
            try
            {
                SetGridFontSize();
                UpdateHeaderWrapHeader(true);
                grdData.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetGridFontSize()
        {
            try
            {
                float fontSize = Convert.ToSingle(RiskPreferenceManager.RiskPrefernece.FontSize);
                Font oldFont = grdData.Font;
                Font newFont = new Font(oldFont.FontFamily, fontSize, oldFont.Style, oldFont.Unit, oldFont.GdiCharSet, oldFont.GdiVerticalFont);
                grdData.Font = newFont;
                grdData.DisplayLayout.Override.SummaryValueAppearance.ForeColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.ColorSummaryText);
                portfolioResultsCtrl1.SetGridFonts(newFont);
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void UpdateHeaderWrapHeader(bool isUpdatePreference)
        {
            try
            {
                bool wrapHeader = Convert.ToBoolean(RiskPreferenceManager.RiskPrefernece.WrapHeader);
                if (wrapHeader.Equals(true))
                    this.grdData.DisplayLayout.Override.WrapHeaderText = DefaultableBoolean.True;
                else
                    this.grdData.DisplayLayout.Override.WrapHeaderText = DefaultableBoolean.False;

                if (wrapHeader)
                {
                    foreach (UltraGridColumn col in grdData.DisplayLayout.Bands[0].Columns)
                    {
                        Graphics g = CreateGraphics();
                        string maxLengthWord = col.Header.Caption.Split(' ').ToArray().OrderBy(w => w.Length).LastOrDefault();
                        SizeF stringSize = new SizeF();
                        Font font = new Font("Tahoma", 13);
                        stringSize = g.MeasureString(maxLengthWord, font);
                        col.Width = Convert.ToInt32(stringSize.Width) + 30;
                    }
                }
                else if (isUpdatePreference)
                {
                    if (RiskLayoutManager.RiskLayout.RiskReportColumns.Count > 0)
                    {
                        List<ColumnData> listColData = RiskLayoutManager.RiskLayout.RiskReportColumns;
                        RiskLayoutManager.LoadColumnsWidthFromXML(grdData, listColData);
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

        private void grdData_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                List<EnumerationValue> PutOrCallType = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(OptionType));
                ValueList PutOrCallValueList = new ValueList();
                foreach (EnumerationValue value in PutOrCallType)
                {
                    PutOrCallValueList.ValueListItems.Add(value.Value, value.DisplayText.Substring(0, 1));
                }
                PutOrCallValueList.ValueListItems.Add(int.MinValue, " ");
                e.Row.Cells[COL_PutOrCall].ValueList = PutOrCallValueList;
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

        private void grdData_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
        {
            try
            {
                if (!e.Row.HasParent())
                {
                    //Top Level Group Row
                    e.Row.Appearance.BackColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.BackColorLevel1);
                    e.Row.Appearance.ForeColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.ForeColorLevel1);
                }
                else
                {
                    //Intermediate Group Row
                    if (!e.Row.ParentRow.HasParent())
                    {
                        e.Row.Appearance.BackColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.BackColorLevel2);
                        e.Row.Appearance.ForeColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.ForeColorLevel2);
                    }
                    else // Bottom Level Group Row
                    {
                        e.Row.Appearance.BackColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.BackColorLevel3);
                        e.Row.Appearance.ForeColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.ForeColorLevel3);
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

        private void grdData_BeforeSortChange(object sender, BeforeSortChangeEventArgs e)
        {
            try
            {
                int counter = 0;
                _firstLevelGroup = string.Empty;
                _secondLevelGroup = string.Empty;

                foreach (UltraGridColumn var in e.SortedColumns)
                {
                    if (var.IsGroupByColumn)
                    {
                        if (counter == 0)
                        {
                            _firstLevelGroup = var.Key;
                        }
                        else
                        {
                            _secondLevelGroup = var.Key;
                        }
                        counter++;
                    }
                }

                //Bharat Kumar Jangir (1 September, 2014)
                //Risk Max Grouping level = 2, Otherwise Risk calculations might be very slow due to more PS API calls
                if (counter > 2)
                {
                    MessageBox.Show("Positions can not be grouped by more than 2 columns.", "Option Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CtrlRiskReports_Load(object sender, EventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(147, 145, 152);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel.BackColor = System.Drawing.Color.FromArgb(147, 145, 152);
                    this.toolStripStatusLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel.Font = new Font("Century Gothic", 9F);
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

        private void RowSummarySettings()
        {
            try
            {
                bool groupedBySomeColumn = false;
                foreach (UltraGridColumn col in grdData.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (col.IsGroupByColumn)
                    {
                        groupedBySomeColumn = true;
                        break;
                    }
                }
                if (!groupedBySomeColumn)
                {
                    this.grdData.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                }
                else
                {
                    //Changes made to show Summary at Grouping level on PM for both Custom Level and Account Level.
                    //http://jira.nirvanasolutions.com:8080/browse/QUAD-43
                    this.grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.Bottom;
                    this.grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.InGroupByRows;
                    this.grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.RootRowsFootersOnly;
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

        private void grdData_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                if (!isAlreadyStarted || _isReloadingLayout)
                {
                    return;
                }
                int sortCount = grdData.DisplayLayout.Bands[0].SortedColumns.Count;
                if (sortCount > 0)
                {
                    //Correction made as it was not returning the column that has been sorted.
                    //Now the sorted column will be detected by mouse click event on column header.
                    UltraGridColumn sortColumn;
                    if (grdData.DisplayLayout.Bands[0].SortedColumns.Contains(_columnSorted))
                    {
                        sortColumn = grdData.DisplayLayout.Bands[0].SortedColumns[_columnSorted.Key];
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grdData.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                        //Set the Group by row summary display settings 
                        RowSummarySettings();
                        return;
                    }
                    if (sortColumn.Formula != null && !(sortColumn.DataType.Equals(typeof(System.Double))))
                    {
                        //Set the Group by row summary display settings 
                        RowSummarySettings();
                        return;
                    }

                    if (!sortColumn.IsGroupByColumn && !GroupByColumnsCollection.Contains(sortColumn.Key))
                    {
                        _groupSortComparer.Column = sortColumn.Key;
                        _groupSortComparer.SortIndicator = sortColumn.SortIndicator;
                        foreach (UltraGridColumn var in grdData.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                            if (var.IsGroupByColumn)
                            {
                                if (var.SortIndicator == SortIndicator.Descending)
                                {
                                    if (_groupSortComparer.SortIndicator == SortIndicator.Ascending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Descending;
                                    }
                                    else if (_groupSortComparer.SortIndicator == SortIndicator.Descending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Ascending;
                                    }
                                }
                                var.GroupByComparer = _groupSortComparer;
                            }
                        }
                        sortColumn.GroupByComparer = _groupSortComparer;
                    }
                    else if (sortColumn.IsGroupByColumn && GroupByColumnsCollection.Contains(sortColumn.Key))
                    {
                        _groupSortComparer.Column = sortColumn.Key;
                        _groupSortComparer.SortIndicator = sortColumn.SortIndicator;
                        foreach (UltraGridColumn var in grdData.DisplayLayout.Bands[0].SortedColumns)
                        {
                            if (var.IsGroupByColumn)
                            {
                                if (var.SortIndicator == SortIndicator.Descending)
                                {
                                    if (_groupSortComparer.SortIndicator == SortIndicator.Ascending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Descending;
                                    }
                                    else if (_groupSortComparer.SortIndicator == SortIndicator.Descending)
                                    {
                                        _groupSortComparer.SortIndicator = SortIndicator.Ascending;
                                    }
                                }
                                var.GroupByComparer = _groupSortComparer;
                            }
                        }
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grdData.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                    }
                    this.grdData.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
                }
                RowSummarySettings();
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

        private void grdData_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                UltraGrid grid = sender as UltraGrid;
                UIElement controlElement = grid.DisplayLayout.UIElement;
                UIElement elementAtPoint = controlElement != null ? controlElement.ElementFromPoint(e.Location) : null;
                while (elementAtPoint != null)
                {
                    Infragistics.Win.UltraWinGrid.UltraGridUIElement uiElement = elementAtPoint.ControlElement as Infragistics.Win.UltraWinGrid.UltraGridUIElement;
                    HeaderUIElement headerElement = uiElement.ElementWithMouseCapture as HeaderUIElement;
                    if (headerElement != null &&
                         headerElement.Header is Infragistics.Win.UltraWinGrid.ColumnHeader)
                    {
                        _columnSorted = headerElement.GetContext(typeof(UltraGridColumn)) as UltraGridColumn;
                        break;
                    }
                    elementAtPoint = elementAtPoint.Parent;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdData_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    // Get a reference to the UIElement at the current mouse position
                    UIElement thisElem = this.grdData.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));

                    // Exit the event handler if no UIElement is found
                    if (thisElem == null)
                        return;

                    //See if the UIElement at the current mouse position is a GroupByBoxUIElement,
                    // or if it is contained as a child of a GroupByBoxUIElement
                    if (thisElem is GroupByBoxUIElement ||
                        thisElem.GetAncestor(typeof(GroupByBoxUIElement)) is GroupByBoxUIElement)
                    {
                        string columnSortedName = string.Empty;
                        if (thisElem is Infragistics.Win.TextUIElement)
                        {
                            columnSortedName = ((Infragistics.Win.TextUIElement)(thisElem)).Text;
                        }
                        else if (thisElem is Infragistics.Win.UltraWinGrid.SortIndicatorUIElement)
                        {
                            columnSortedName = ((Infragistics.Win.UltraWinGrid.SortIndicatorUIElement)(thisElem)).ToString();
                        }
                        else if (thisElem is GroupByButtonUIElement)
                        {
                            foreach (object thisElemChild in thisElem.ChildElements)
                            {
                                if (thisElemChild is Infragistics.Win.TextUIElement)
                                {
                                    columnSortedName = ((Infragistics.Win.TextUIElement)(thisElemChild)).Text;
                                    break;
                                }
                            }
                        }
                        for (int counter = 0; counter < grdData.DisplayLayout.Bands[0].SortedColumns.Count; counter++)
                        {
                            if (columnSortedName.Equals(grdData.DisplayLayout.Bands[0].SortedColumns[counter].Header.Caption))
                            {
                                _columnSorted = grdData.DisplayLayout.Bands[0].SortedColumns[grdData.DisplayLayout.Bands[0].SortedColumns[counter].Key];
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_SHOWONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void grdData_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        public void SetGridExportSettings(Dictionary<string, UltraGrid> gridDict, Dictionary<string, string> riskReportDetails)
        {
            try
            {
                UltraGrid grid = new UltraGrid();
                Form UI = new Form();
                UI.Controls.Add(grid);
                grid.DataSource = _riskObjcollection;
                grid.DisplayLayout.Bands[0].Override.AllowGroupBy = DefaultableBoolean.True;
                grid.DisplayLayout.ViewStyleBand = ViewStyleBand.OutlookGroupBy;
                grid.DisplayLayout.Override.GroupByColumnsHidden = DefaultableBoolean.False;
                grid.DisplayLayout.Bands[0].ColumnFilters.CopyFrom(grdData.DisplayLayout.Bands[0].ColumnFilters);
                foreach (UltraGridColumn col in grdData.DisplayLayout.Bands[0].Columns)
                {
                    if (col.Header.Caption != string.Empty)
                    {
                        grid.DisplayLayout.Bands[0].Columns[col.Key].Header.Caption = col.Header.Caption;
                    }
                    grid.DisplayLayout.Bands[0].Columns[col.Key].Hidden = true;
                }

                UltraGridBand band = this.grdData.DisplayLayout.Bands[0];
                SortedColumnsCollection sortedcolColl = band.SortedColumns;
                grid.DisplayLayout.Bands[0].SortedColumns.Clear();
                if (sortedcolColl != null && sortedcolColl.Count > 0)
                {
                    foreach (UltraGridColumn col in sortedcolColl)
                    {
                        bool isDescending = col.SortIndicator == SortIndicator.Descending;
                        if (col.IsGroupByColumn)
                        {
                            grid.DisplayLayout.Bands[0].SortedColumns.Add(col.Key, isDescending, true);
                        }
                        else
                        {
                            grid.DisplayLayout.Bands[0].SortedColumns.Add(col.Key, isDescending, false);
                        }
                    }
                }
                int rowCount = portfolioResultsCtrl1.grdData.DisplayLayout.Rows.Count;
                StringBuilder sbRiskLowerGridValues = new StringBuilder();
                UltraGridLayout gridDisplayLayout = portfolioResultsCtrl1.grdData.DisplayLayout;
                foreach (UltraGridColumn col in gridDisplayLayout.Bands[0].Columns)
                {
                    if (rowCount == 0)
                    {
                        sbRiskLowerGridValues.Append(col.Key + "=" + "0" + Seperators.SEPERATOR_6);
                    }
                    else
                    {
                        sbRiskLowerGridValues.Append(col.Key + "=" + gridDisplayLayout.Rows[0].Cells[col.Key].Value + Seperators.SEPERATOR_6);
                    }
                }
                sbRiskLowerGridValues.Remove(sbRiskLowerGridValues.Length - 1, 1);

                int count = 0;
                foreach (UltraGridColumn col in grid.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (col.IsGroupByColumn)
                    {
                        count++;
                    }
                }
                if (count > 0)
                {
                    grid.ExternalSummaryValueRequested += grdData_ExternalSummaryValueRequested;
                    SetColumnSummaries(grid);
                }
                grid.DisplayLayout.Bands[0].ColHeadersVisible = false;
                grid.DisplayLayout.Override.GroupByColumnsHidden = DefaultableBoolean.False;
                grid.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
                grid.DisplayLayout.Override.GroupBySummaryDisplayStyle = GroupBySummaryDisplayStyle.SummaryCells;
                grid.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.InGroupByRows
                | SummaryDisplayAreas.RootRowsFootersOnly
                | SummaryDisplayAreas.Bottom;
                grid.DisplayLayout.Override.SummaryFooterCaptionVisible = DefaultableBoolean.False;
                grid.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.CheckOnDisplay;

                riskReportDetails.Add(tabRiskReport, sbRiskLowerGridValues.ToString());
                SortedDictionary<int, string> positionMapping = new SortedDictionary<int, string>();
                foreach (UltraGridColumn col in grdData.DisplayLayout.Bands[0].Columns)
                {
                    if (!col.Hidden)
                    {
                        positionMapping[col.Header.VisiblePosition] = col.Key;
                    }
                }
                foreach (KeyValuePair<int, string> pair in positionMapping)
                {
                    if (pair.Value == "IsChecked")
                    {
                        grid.DisplayLayout.Bands[0].Columns[pair.Value].Hidden = true;
                    }
                    else
                    {
                        UltraGridColumn columnExportGrid = grid.DisplayLayout.Bands[0].Columns[pair.Value];
                        grid.DisplayLayout.Bands[0].Columns[pair.Value].Hidden = false;
                        columnExportGrid.Header.VisiblePosition = pair.Key;
                    }
                }
                gridDict.Add(tabRiskReport, grid);
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
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                    if (_dtCalculatedRisks != null)
                    {
                        _dtCalculatedRisks.Dispose();
                    }
                    if (_dataPortfolio != null)
                    {
                        _dataPortfolio.Dispose();
                    }
                    if (_columnSorted != null)
                    {
                        _columnSorted.Dispose();
                    }
                    if (resetEvent != null)
                    {
                        resetEvent.Dispose();
                    }
                    if (_riskServiceProxy != null)
                    {
                        _riskServiceProxy.Dispose();
                    }
                    if (headerCheckBox != null)
                    {
                        headerCheckBox.Dispose();
                    }
                    if (grdData.DisplayLayout.Bands[0] != null && grdData.DisplayLayout.Bands[0].Summaries.Count > 0)
                    {
                        grdData.DisplayLayout.Bands[0].Summaries.Clear();
                    }
                    if (grdData != null)
                    {
                        grdData.Dispose();
                    }
                    if (portfolioResultsCtrl1 != null)
                    {
                        portfolioResultsCtrl1 = null;
                    }
                }

                base.Dispose(disposing);
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
    }
}