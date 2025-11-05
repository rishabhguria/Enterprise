using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Analytics.Classes;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.Analytics
{
    public partial class CtrlStepAnalysis : UserControl
    {
        #region Private Variables
        bool isAlreadyStarted = false;
        bool _isDataAlreadyStressed = false;
        bool _isNeedToPerformStepAnalysisAfterStressTest = false;
        bool _lowerGridHandleCreated = false;
        SecMasterBaseObj _secMasterObjEnteredSymbol;
        SecMasterBaseObj _secMasterObjUnderlyingSymbol;
        private delegate void UIThreadMarsheller(List<StepAnalysisResponse> stepRes);
        private delegate void UIThreadMarshellersSecMaster(object sender, EventArgs<SecMasterBaseObj> e);
        private delegate void UIThreadMarshellerGreekscalc(ResponseObj optionResponseObj);
        private delegate void UIThreadMarshellerForComplete(QueueMessage qMsg);
        private delegate void Level1DataUpdateHandler(SymbolData level1Data);
        private delegate void UIThreadMarshellerVolSkew(VolSkewObject SkewRes);
        private delegate void UIThreadMarshellerConnectionStatus();

        public event EventHandler<EventArgs<bool, List<PranaPositionWithGreeks>>> AddSymbolAcrossAllViews;
        DataTable dtGraphSelected = null;
        int _responseReceivedCount = 0;
        List<string> _listRequestedSymbols = new List<string>();

        Dictionary<string, List<PranaPositionWithGreeks>> _dictBindedData = null;
        Dictionary<string, List<PranaPositionWithGreeks>> _dictUnderlyingFututes = null;
        Dictionary<string, DataRow> _existingRows = new Dictionary<string, DataRow>();
        PranaPositionWithGreekColl _pranaPositionWithGreekColl = new PranaPositionWithGreekColl();
        CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();

        ISecurityMasterServices _secMasterClient = null;
        StepAnalysisCache _stepAnalysisCache = null;
        IPricingAnalysis _pricingAnalysis = null;
        int _userID = int.MinValue;
        Dictionary<string, string> _dictListBoxText = null;
        string cmbPorfolioLastValue = "Portfolio";
        GroupSortComparer _groupSortComparer = new GroupSortComparer();
        //Color _colorLevel1 = Color.DimGray;
        //Color _colorLevel2 = Color.DarkGray;
        //Color _colorLevel3 = Color.LightGray;
        //int hashcode = int.MinValue;
        private bool _isStressTestRequest = false;
        private bool _isStepAnalysisRequest = false;
        private string _viewID = string.Empty;
        private string _viewName = string.Empty;
        StepParameter _stepParameter = null;
        StepAnalysisPref _preferences = null;
        private readonly int _greekAnalysisTabTimeOut;

        //Bharat (31 December 2013)
        //http://jira.nirvanasolutions.com:8080/browse/PRANA-3083
        private Dictionary<int, string> _allowedAssets;
        private bool _isReloadingLayout = false;
        bool _isLiveFeedConnected = false;
        #endregion

        public string ViewID
        {
            get { return _viewID; }
            set { _viewID = value; }
        }
        public string ViewName
        {
            get { return _viewName; }
            set { _viewName = value; }
        }

        public event EventHandler RefreshCompleted;
        public event EventHandler DeleteViewClick;
        public event EventHandler AddViewClick;
        public event EventHandler RenameViewClick;
        public event EventHandler SaveLayoutAllClick;
        public event EventHandler<EventArgs<string>> GreeksRequested;
        public event EventHandler<EventArgs<string>> GreeksCalculated;
        public event EventHandler<EventArgs<bool>> UseVolSkew;
        public delegate void MsgreceivedInvokeDelegate(PranaPositionWithGreekColl posList);
        public delegate void InUsedUDADataResInvokeDelegate(object sender, EventArgs<Dictionary<string, Dictionary<int, string>>> e);

        #region Column Names
        private const string COL_IsChecked = "IsChecked";
        private const string COL_Symbol = "Symbol";
        private const string COL_Factset = "FactSetSymbol";
        private const string COL_Activ = "ActivSymbol";
        private const string COL_CompanyName = "CompanyName";
        private const string COL_UnderlyingSymbol = "UnderlyingSymbol";
        private const string COL_Level1Name = "Level1Name";
        private const string COL_MasterFund = "MasterFund";
        private const string COL_AssetName = "AssetName";
        private const string COL_Quantity = "Quantity";
        private const string COL_SelectedFeedPrice = "SelectedFeedPrice";
        private const string COL_SimulatedUnderlyingStockPrice = "SimulatedUnderlyingStockPrice";
        private const string COL_SimulatedPrice = "SimulatedPrice";
        private const string COL_SimulatedPnl = "SimulatedPnl";
        private const string COL_Volatility = "Volatility";
        private const string COL_DeltaAdjExposure = "DeltaAdjExposure";
        private const string COL_Delta = "Delta";
        private const string COL_DollarDelta = "DollarDelta";
        private const string COL_Gamma = "Gamma";
        private const string COL_DollarGamma = "DollarGamma";
        private const string COL_Theta = "Theta";
        private const string COL_DollarTheta = "DollarTheta";
        private const string COL_Vega = "Vega";
        private const string COL_DollarVega = "DollarVega";
        private const string COL_Rho = "Rho";
        private const string COL_DollarRho = "DollarRho";
        private const string COL_DaysToExpiration = "DaysToExpiration";
        private const string COL_MarketValue = "MarketValue";
        private const string COL_AvgPrice = "AvgPrice";
        private const string COL_Level2Name = "Level2Name";
        private const string COL_StrikePrice = "StrikePrice";
        private const string COL_AUECLocalDate = "AUECLocalDate";
        private const string COL_CompanyUserName = "CompanyUserName";
        private const string COL_CounterPartyName = "CounterPartyName";
        private const string COL_CountryName = "CountryName";
        private const string COL_CurrencyName = "CurrencyName";
        private const string COL_ExchangeName = "ExchangeName";
        private const string COL_ExpirationDate = "ExpirationDate";
        private const string COL_ContractMultiplier = "ContractMultiplier";
        private const string COL_PositionType = "PositionType";
        private const string COL_PutOrCall = "PutOrCall";
        private const string COL_SectorName = "SectorName";
        private const string COL_SubSectorName = "SubSectorName";
        private const string COL_SecurityTypeName = "SecurityTypeName";
        private const string COL_UnderlyingName = "UnderlyingName";
        private const string COL_InterestRate = "InterestRate";
        private const string COL_DeltaAdjPosition = "DeltaAdjPosition";
        private const string COL_CostBasisUnrealizedPnL = "CostBasisUnrealizedPnL";
        private const string COL_ProxySymbol = "ProxySymbol";
        private const string COL_PositionSideMVInPortfolio = "PositionSideMVInPortfolio";
        private const string COL_PositionSideExposureInPortfolio = "PositionSideExposureInPortfolio";
        private const string COL_DeltaAdjPositionLME = "DeltaAdjPositionLME";
        private const string COL_ExpirationMonth = "ExpirationMonth";
        private const string COL_GammaAdjPosition = "GammaAdjPosition";
        private const string COL_UDAAsset = "UDAAsset";
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
        private const string COL_SedolSymbol = "SedolSymbol";
        private const string COL_OSISymbol = "OSISymbol";
        private const string COL_CusipSymbol = "CusipSymbol";
        private const string COL_PricingSource = "PricingSource";
        private const string COL_UnderlyingPrice = "UnderlyingPrice";
        private const string COL_Beta = "Beta";
        private const string COL_BetaAdjExposure = "BetaAdjExposure";
        public const string COL_Analyst = "Analyst";
        public const string COL_CountryOfRisk = "CountryOfRisk";
        public const string COL_CustomUDA1 = "CustomUDA1";
        public const string COL_CustomUDA2 = "CustomUDA2";
        public const string COL_CustomUDA3 = "CustomUDA3";
        public const string COL_CustomUDA4 = "CustomUDA4";
        public const string COL_CustomUDA5 = "CustomUDA5";
        public const string COL_CustomUDA6 = "CustomUDA6";
        public const string COL_CustomUDA7 = "CustomUDA7";
        public const string COL_Issuer = "Issuer";
        public const string COL_LiquidTag = "LiquidTag";
        public const string COL_MarketCap = "MarketCap";
        public const string COL_Region = "Region";
        public const string COL_RiskCurrency = "RiskCurrency";
        public const string COL_UcitsEligibleTag = "UcitsEligibleTag";
        public const string COL_SelectedFeedPriceInBaseCurrency = "SelectedFeedPriceInBaseCurrency";
        public const string COL_BetaAdjExposureInBaseCurrency = "BetaAdjExposureInBaseCurrency";
        public const string COL_CostBasisUnrealizedPnLInBaseCurrency = "CostBasisUnrealizedPnLInBaseCurrency";
        public const string COL_DeltaAdjExposureInBaseCurrency = "DeltaAdjExposureInBaseCurrency";
        public const string COL_DollarDeltaInBaseCurrency = "DollarDeltaInBaseCurrency";
        public const string COL_DollarGammaInBaseCurrency = "DollarGammaInBaseCurrency";
        public const string COL_DollarRhoInBaseCurrency = "DollarRhoInBaseCurrency";
        public const string COL_DollarThetaInBaseCurrency = "DollarThetaInBaseCurrency";
        public const string COL_DollarVegaInBaseCurrency = "DollarVegaInBaseCurrency";
        public const string COL_MarketValueInBaseCurrency = "MarketValueInBaseCurrency";
        public const string COL_SimulatedPnlInBaseCurrency = "SimulatedPnlInBaseCurrency";
        public const string COL_SimulatedPriceInBaseCurrency = "SimulatedPriceInBaseCurrency";
        public const string COL_SimulatedUnderlyingStockPriceInBaseCurrency = "SimulatedUnderlyingStockPriceInBaseCurrency";
        public const string COL_AvgPriceInBaseCurrency = "AvgPriceInBaseCurrency";
        public const string COL_FXRate = "FXRate";
        #endregion

        public CtrlStepAnalysis()
        {
            try
            {
                InitializeComponent();
                _greekAnalysisTabTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["GreekAnalysisTabTimeOut"]);
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

        DuplexProxyBase<IGreekAnalysisServices> _greekAnalysisServiceProxy = null;
        public void DisposeProxy()
        {
            try
            {
                if (_greekAnalysisServiceProxy != null)
                {
                    _greekAnalysisServiceProxy.Dispose();
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

        private bool _useVolSkewLimitReached = false;
        public bool UseVolSkewLimitReached
        {
            get { return _useVolSkewLimitReached; }
            set { _useVolSkewLimitReached = value; }
        }

        /// <summary>
        /// Sets the status of Use Vol Skew Check box.
        /// </summary>
        /// <param name="useVolSkew">
        /// Determine the status for Use Vol Skew Check Box.</param>
        public void SetUseVolSkew(bool useVolSkew)
        {
            try
            {
                if (useVolSkew)
                {
                    this.chkBoxUseVolSkew.CheckState = CheckState.Checked;
                }
                else
                {
                    this.chkBoxUseVolSkew.CheckState = CheckState.Unchecked;
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

        private async void SendRequestForStepAnalysis(InputParametersCollection inputParametersCollection)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() => _greekAnalysisServiceProxy.InnerChannel.RequestStepAnalysisData(inputParametersCollection));
                _isStepAnalysisRequest = true;
                DisableForm();
                btnStepAnalysis.BackColor = Color.Red;
                btnStepAnalysis.Text = "Analysing...";
                toolStripStatusLabel.ForeColor = Color.Red;
                toolStripStatusLabel.Text = DateTime.Now + ": Performing Step Analysis...";
                grdData.DataSource = null;
                grdData.DataBind();
                if (dtGraphSelected != null)
                {
                    dtGraphSelected.Rows.Clear();
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

        public void SetUp(ISecurityMasterServices secMasterClient, IPricingAnalysis pricingAnalysis, string viewName, string viewID, DuplexProxyBase<IGreekAnalysisServices> greekAnalysisServiceProxy)
        {
            try
            {
                if (!isAlreadyStarted)
                {
                    _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                    _viewID = viewID;
                    _viewName = viewName;
                    _dictBindedData = new Dictionary<string, List<PranaPositionWithGreeks>>();
                    _dictUnderlyingFututes = new Dictionary<string, List<PranaPositionWithGreeks>>();
                    _dictListBoxText = GetListBoxTextDictionary();
                    _secMasterClient = secMasterClient;
                    _pricingAnalysis = pricingAnalysis;
                    _stepAnalysisCache = new StepAnalysisCache(this);
                    BindComboBoxes();
                    SetGridFontSize();
                    BindAccountListCombo();
                    BindAssetCategoryCombo();
                    BindGridData();
                    SetPreferences();
                    UpdateHeaderWrapHeader(false);
                    WireEvents();
                    exGrpBoxAddSymbol.Expanded = false;
                    _greekAnalysisServiceProxy = greekAnalysisServiceProxy;
                    if (RiskLayoutManager.RiskLayout.GetStepAnalLayout(_viewName).SplitterPosition <= 15) //case it is minimized or too narrow
                    {
                        _lowerGridHandleCreated = false;
                        exGrpBoxStepAnalysis.Expanded = false;
                    }
                    isAlreadyStarted = true;
                    DisableForm();
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {
                if (this.IsHandleCreated && !this.IsDisposed)
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
                btnSimulate.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSimulate.ForeColor = System.Drawing.Color.White;
                btnSimulate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSimulate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSimulate.UseAppStyling = false;
                btnSimulate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnNonParallelShifts.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnNonParallelShifts.ForeColor = System.Drawing.Color.White;
                btnNonParallelShifts.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnNonParallelShifts.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnNonParallelShifts.UseAppStyling = false;
                btnNonParallelShifts.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRevert.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRevert.ForeColor = System.Drawing.Color.White;
                btnRevert.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRevert.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRevert.UseAppStyling = false;
                btnRevert.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnStepAnalysis.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnStepAnalysis.ForeColor = System.Drawing.Color.White;
                btnStepAnalysis.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnStepAnalysis.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnStepAnalysis.UseAppStyling = false;
                btnStepAnalysis.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                button4.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                button4.ForeColor = System.Drawing.Color.White;
                button4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                button4.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                button4.UseAppStyling = false;
                button4.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                button5.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                button5.ForeColor = System.Drawing.Color.White;
                button5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                button5.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                button5.UseAppStyling = false;
                button5.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClearGraph.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClearGraph.ForeColor = System.Drawing.Color.White;
                btnClearGraph.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClearGraph.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClearGraph.UseAppStyling = false;
                btnClearGraph.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAddSymbol.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnAddSymbol.ForeColor = System.Drawing.Color.White;
                btnAddSymbol.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAddSymbol.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAddSymbol.UseAppStyling = false;
                btnAddSymbol.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// To bind the list of accounts combo provided to add the symbol to selected account.
        /// </summary>
        private void BindAccountListCombo()
        {
            try
            {
                KeyValuePair<int, string> itemSelectAll = new KeyValuePair<int, string>(int.MinValue, "All");
                checkedMultipleItems.Items.Add(itemSelectAll);
                foreach (KeyValuePair<int, string> kvp in CachedDataManager.GetInstance.GetAccounts())
                {
                    checkedMultipleItems.Items.Add(kvp);
                }
                checkedMultipleItems.DisplayMember = "Value";
                checkedMultipleItems.ValueMember = "Key";
                AdjustCheckListBoxWidth();
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

        //Bharat (31 December 2013)
        //http://jira.nirvanasolutions.com:8080/browse/PRANA-3083
        private void BindAssetCategoryCombo()
        {
            try
            {
                //TODO : Bharat - LINQ Code
                //List<int> assetIdsForStressTestInRisk = (ConfigurationHelper.Instance.GetAppSettingValueByKey("AssetIdsForStressTestInRisk")).Split(',').Select(int.Parse).ToList();

                //TODO : Comment below code after VS 2012 on Prod Branch
                /////////////////////////////////////////////////////////////////////////////////
                string[] allowedAssetId = (ConfigurationHelper.Instance.GetAppSettingValueByKey("AssetIdsForStressTestInRisk")).Split(',');
                List<int> assetIdsForStressTestInRisk = new List<int>();
                for (int counter = 0; counter < allowedAssetId.Length; counter++)
                {
                    assetIdsForStressTestInRisk.Add(int.Parse(allowedAssetId[counter]));
                }
                /////////////////////////////////////////////////////////////////////////////////

                Dictionary<int, string> dictAssets = new Dictionary<int, string>();
                dictAssets = CommonDataCache.CachedDataManager.GetInstance.GetAllAssets();

                Dictionary<int, string> bindableAssets = new Dictionary<int, string>();
                foreach (KeyValuePair<int, string> kvp in dictAssets)
                {
                    if (assetIdsForStressTestInRisk.Contains(kvp.Key))
                    {
                        bindableAssets.Add(kvp.Key, kvp.Value);
                    }
                }
                //add Assets to the check list default value will be unchecked
                multiSelectDropDown1.AddItemsToTheCheckList(bindableAssets, CheckState.Checked);
                //adjust checklistbox width according to the longest Asset Name
                multiSelectDropDown1.AdjustCheckListBoxWidth();
                multiSelectDropDown1.TitleText = "Asset";
                multiSelectDropDown1.SetTitleText(multiSelectDropDown1.GetNoOfTotalItems());
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

        private void WireEvents()
        {
            try
            {
                _secMasterClient.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_secMasterClient_SecMstrDataResponse);
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

        public void UpdateConnectionStatus(bool isLiveFeedConnected)
        {
            try
            {
                _isLiveFeedConnected = isLiveFeedConnected;
                if (_pricingAnalysis == null)
                {
                    return;
                }
                if (_pricingAnalysis.ConnectionStatus.Equals(Prana.BusinessObjects.PranaInternalConstants.ConnectionStatus.CONNECTED) && isLiveFeedConnected)
                {
                    toolStripStatusLabel.ForeColor = Color.Green;
                    toolStripStatusLabel.Text = DateTime.Now + ": Ready";
                }
                else
                {
                    if (_isStepAnalysisRequest || _isStressTestRequest)
                    {
                        EnableForm();
                    }
                    if (_pricingAnalysis.ConnectionStatus.Equals(Prana.BusinessObjects.PranaInternalConstants.ConnectionStatus.DISCONNECTED) || _pricingAnalysis.ConnectionStatus.Equals(Prana.BusinessObjects.PranaInternalConstants.ConnectionStatus.NOSERVER))
                    {
                        toolStripStatusLabel.ForeColor = Color.Red;
                        toolStripStatusLabel.Text = DateTime.Now + ": Pricing Server Not Available";
                    }
                    else
                    {
                        if (!isLiveFeedConnected)
                        {
                            toolStripStatusLabel.ForeColor = Color.Red;
                            toolStripStatusLabel.Text = DateTime.Now + ": Live Feed is Disconnected";
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

        private void SetPreferences()
        {
            try
            {
                SetColumns();
                if (RiskPreferenceManager.RiskPrefernece.StepAnalPreferencesDict.ContainsKey(_viewName))
                {
                    _preferences = RiskPreferenceManager.RiskPrefernece.GetStepAnalViewPreferences(_viewName);
                }
                else
                {
                    //Provides the ability to get the default preferences, saved by user, for new tab.
                    _preferences = RiskPreferenceManager.GetDefaultPreferences();
                    _preferences.StepAnalViewName = _viewName;
                    _preferences.StepAnalViewID = _viewID;
                    //Updates the preferences dictionary in case of new step analysis view added.
                    RiskPreferenceManager.RiskPrefernece.UpdateStepAnalPrefDict(_viewName, _preferences);
                }
                _preferences.StepAnalViewID = _viewID;
                numericUpDownVol.Value = Double.Parse(_preferences.ChangeVolatility.ToString());
                numericUpDownUnderLyingPrice.Value = Double.Parse(_preferences.ChangeUnderlyingPrice.ToString());
                numericUpDownIntRate.Value = Double.Parse(_preferences.ChangeInterestRate.ToString());
                numericUpDownDaysToExp.Value = Double.Parse(_preferences.ChangeDaysToExpiration.ToString());
                chkbxVol.Checked = _preferences.IsVolShock;
                chkbxUnderLyingPrice.Checked = _preferences.IsUnderlyingShock;
                chkbxInterestRate.Checked = _preferences.IsIntRateShock;
                ckhbxExpiration.Checked = _preferences.IsDaysToExpShock;
                chkBoxUseVolSkew.Checked = _preferences.UseVolSkew;
                checkBoxUseAbsoluteValuesForUnderlyingPrice.Checked = _preferences.UseAbsoluteValuesForUnderlyingPrice;
                checkBoxUseStressTestDataInStepAnalysis.Checked = _preferences.UseStressTestDataInStepAnalysis;
                chkBoxUseBetaAdj.Checked = _preferences.UseBetaAdjPrice;
                if (_preferences.UseAbsoluteValuesForUnderlyingPrice)
                {
                    chkbxUnderLyingPrice.Text = "Underlying Price";
                }

                if (_preferences.UseNonParallelShifts)
                {
                    UpdateStressTestInputsControl(false);
                    toolTip1.Active = true;
                }
                else
                {
                    toolTip1.Active = false;
                }
                _allowedAssets = _preferences.AllowedAssetCategory as Dictionary<int, string>;
                if (_allowedAssets != null && _allowedAssets.Count > 0)
                {
                    multiSelectDropDown1.SelectUnselectAll(CheckState.Unchecked);
                    multiSelectDropDown1.SelectUnselectItems(_allowedAssets, CheckState.Checked);
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

        private async void SendRequestForGreekCalculation(InputParametersCollection inputParametersCollection)
        {
            try
            {
                _isStressTestRequest = true;
                inputParametersCollection.UserID = _userID.ToString();
                await System.Threading.Tasks.Task.Run(() => { _greekAnalysisServiceProxy.InnerChannel.RequestSnapshotData(inputParametersCollection); });
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

        private void AddPositionToDict(PranaPositionWithGreeks obj)
        {
            try
            {
                if (_dictBindedData.ContainsKey(obj.Symbol))
                {
                    _dictBindedData[obj.Symbol].Add(obj);
                }
                else
                {
                    List<PranaPositionWithGreeks> list = new List<PranaPositionWithGreeks>();
                    list.Add(obj);
                    _dictBindedData.Add(obj.Symbol, list);
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

        private void AddPositionToUnderlyingFutureDict(PranaPositionWithGreeks posGreeks)
        {
            try
            {
                if (_dictUnderlyingFututes.ContainsKey(posGreeks.UnderlyingSymbol))
                {
                    _dictUnderlyingFututes[posGreeks.UnderlyingSymbol].Add(posGreeks);
                }
                else
                {
                    List<PranaPositionWithGreeks> list = new List<PranaPositionWithGreeks>();
                    list.Add(posGreeks);
                    _dictUnderlyingFututes.Add(posGreeks.UnderlyingSymbol, list);
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

        public bool ValidateRequest()
        {
            try
            {
                if (ValidateConnection())
                {
                    if (GetSelectedRows().Count < 1)
                    {
                        toolStripStatusLabel.ForeColor = Color.Red;
                        toolStripStatusLabel.Text = "No Rows Selected";
                        return false;
                    }

                    if (multiSelectDropDown1.GetNoOfCheckedItems() < 1)
                    {
                        toolStripStatusLabel.ForeColor = Color.Red;
                        toolStripStatusLabel.Text = "No Asset Categories Selected";
                        return false;
                    }
                    return true;
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
            return false;
        }

        private void DisableForm()
        {
            try
            {
                btnSimulate.Enabled = false;
                btnStepAnalysis.Enabled = false;
                btnRevert.Enabled = false;
                cmbPortfolio.Enabled = false;
                cmbbxXAxis.Enabled = false;
                timerRefresh.Interval = _greekAnalysisTabTimeOut * 1000;
                if (_isStepAnalysisRequest)
                {
                    _existingRows.Clear();
                }
                if (_isStressTestRequest)
                {
                    _pranaPositionWithGreekColl.RaiseListChangedEvents = false;

                    grdPositions.SuspendRowSynchronization();
                    grdPositions.SuspendSummaryUpdates();
                }
                grdPositions.DisplayLayout.Bands[0].Columns[COL_IsChecked].CellActivation = Activation.Disabled;
                grdPositions.CreationFilter = null;
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

        public void EnableForm()
        {
            try
            {
                _pranaPositionWithGreekColl.RaiseListChangedEvents = true;
                grdPositions.ResumeRowSynchronization();
                grdPositions.ResumeSummaryUpdates(true);
                grdPositions.Rows.Refresh(RefreshRow.ReloadData);

                btnSimulate.Enabled = true;
                btnStepAnalysis.Enabled = true;
                btnRevert.Enabled = true;
                cmbbxXAxis.Enabled = true;

                btnSimulate.BackColor = Color.FromArgb(104, 156, 46);
                btnSimulate.Text = "Run";
                btnStepAnalysis.BackColor = Color.FromArgb(104, 156, 46);
                btnStepAnalysis.Text = "Run";
                btnRevert.BackColor = Color.FromArgb(55, 67, 85);

                _isStepAnalysisRequest = false;
                _isStressTestRequest = false;
                _responseReceivedCount = 0;
                _listRequestedSymbols.Clear();
                timerRefresh.Stop();
                grdPositions.DisplayLayout.Bands[0].Columns[COL_IsChecked].CellActivation = Activation.AllowEdit;
                grdPositions.CreationFilter = headerCheckBox;
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

        private void BindGridData()
        {
            try
            {
                if (!this.IsDisposed)
                {
                    _isReloadingLayout = true;
                    UltraGridLayout layout = grdPositions.DisplayLayout.Clone();
                    grdPositions.DataSource = null;
                    grdPositions.DataSource = _pranaPositionWithGreekColl;
                    grdPositions.DisplayLayout.Load(layout, PropertyCategories.All);
                    _isReloadingLayout = false;
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

        private void BindComboBoxes()
        {
            try
            {
                cmbbxXAxis.DataSource = EnumHelper.ConvertEnumForBindingWithActualAssignedValuesWithCaption(typeof(StepAnalParameterCode));
                cmbbxXAxis.DisplayMember = "DisplayText";
                cmbbxXAxis.ValueMember = "Value";
                cmbbxXAxis.DataBind();
                cmbbxXAxis.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbbxXAxis.Value = StepAnalParameterCode.UnderlyingPrice;

                foreach (string key in _dictListBoxText.Keys)
                {
                    lstbxYAxis.Items.Add(key);
                }
                lstbxYAxis.SetItemChecked(0, true);
                lstbxYAxis.SetItemChecked(1, true);
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

        private Dictionary<string, string> GetListBoxTextDictionary()
        {
            Dictionary<string, string> dictListBoxText = new Dictionary<string, string>();
            try
            {
                dictListBoxText.Add(COL_Delta, COL_Delta);
                dictListBoxText.Add(COL_Gamma, COL_Gamma);
                dictListBoxText.Add(COL_Theta, COL_Theta);
                dictListBoxText.Add(COL_Vega, COL_Vega);
                dictListBoxText.Add(COL_Rho, COL_Rho);
                dictListBoxText.Add("Dollar Delta (Base)", COL_DollarDeltaInBaseCurrency);
                dictListBoxText.Add("Dollar Gamma (Base)", COL_DollarGammaInBaseCurrency);
                dictListBoxText.Add("Dollar Theta (Base)", COL_DollarThetaInBaseCurrency);
                dictListBoxText.Add("Dollar Vega (Base)", COL_DollarVegaInBaseCurrency);
                dictListBoxText.Add("Dollar Rho (Base)", COL_DollarRhoInBaseCurrency);
                dictListBoxText.Add("Simulated Price (Base)", COL_SimulatedPriceInBaseCurrency);
                dictListBoxText.Add("Delta Exposure (Base)", COL_DeltaAdjExposureInBaseCurrency);
                dictListBoxText.Add("Cost Basis P&L (Base)", COL_CostBasisUnrealizedPnLInBaseCurrency);
                dictListBoxText.Add("Simulated P&L (Base)", COL_SimulatedPnlInBaseCurrency);
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
            return dictListBoxText;
        }

        private void BindPortfoliosCombo()
        {
            try
            {
                List<string> symbols = _stepAnalysisCache.GetSymbols();
                if (symbols.Count > 0)
                {
                    EnumerationValueList symbolList = new EnumerationValueList();
                    foreach (string name in symbols)
                    {
                        symbolList.Add(new EnumerationValue(name, name));
                    }
                    cmbPortfolio.DataSource = null;
                    cmbPortfolio.DataSource = symbolList;
                    cmbPortfolio.DataBind();
                    if (symbols.Contains(cmbPorfolioLastValue))
                    {
                        cmbPortfolio.Value = cmbPorfolioLastValue;
                    }
                    else
                    {
                        cmbPortfolio.Value = "Portfolio";
                    }
                    cmbPortfolio.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                    cmbPortfolio.Enabled = true;
                    cmbPortfolio.DisplayLayout.Bands[0].ColHeadersVisible = false;
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

        private void SetColumns()
        {
            try
            {
                bool isApplyDefaultFilters = false;
                if (RiskLayoutManager.RiskLayout.GetStepAnalLayout(_viewName).StepAnalysisColumns.Count > 0)
                {
                    LoadColumnsFromXML();
                }
                else
                {
                    LoadColumns();
                    isApplyDefaultFilters = true;
                }
                SetColumnFormatting();
                SetColumnCustomizations(isApplyDefaultFilters);
                SetColumnSummaries(grdPositions);
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
                if (RiskLayoutManager.RiskLayout.StepAnalysisColumnsList.ContainsKey(_viewName))
                {
                    List<ColumnData> listColData = RiskLayoutManager.RiskLayout.GetStepAnalLayout(_viewName).StepAnalysisColumns;
                    List<SortedColumnData> listGroupByColumnsCollection = RiskLayoutManager.RiskLayout.GetStepAnalLayout(_viewName).GroupByColumnsCollection;
                    RiskLayoutManager.SetGridColumnLayout(grdPositions, listColData, listGroupByColumnsCollection, GetAllDisplayableColumns());
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

        private void LoadColumns()
        {
            try
            {
                List<string> colAll = GetAllDisplayableColumns();
                List<string> colDefault = GetAllDefaultColumns();
                List<string> colVisible = GeneralUtilities.GetListFromString(RiskPreferenceManager.RiskPrefernece.GetStepAnalViewPreferences(_viewName).StepAnalysisColumns, ',');

                if (colVisible.Count < 1) // PrefFile Has No Columns
                {
                    colVisible.AddRange(colDefault);
                }

                ColumnsCollection gridColumns = grdPositions.DisplayLayout.Bands[0].Columns;
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

        private List<string> GetAllDefaultColumns()
        {
            List<string> colDefault = new List<string>();
            try
            {
                colDefault.Add(COL_IsChecked);
                colDefault.Add(COL_Symbol);
                colDefault.Add(COL_CompanyName);
                colDefault.Add(COL_UnderlyingSymbol);
                colDefault.Add(COL_Level1Name);
                colDefault.Add(COL_MasterFund);
                colDefault.Add(COL_AssetName);
                colDefault.Add(COL_Quantity);
                colDefault.Add(COL_SelectedFeedPrice);
                colDefault.Add(COL_SimulatedUnderlyingStockPrice);
                colDefault.Add(COL_SimulatedPrice);
                colDefault.Add(COL_SimulatedPnl);
                colDefault.Add(COL_Volatility);
                colDefault.Add(COL_DeltaAdjExposure);
                colDefault.Add(COL_Delta);
                colDefault.Add(COL_DollarDelta);
                colDefault.Add(COL_Gamma);
                colDefault.Add(COL_DollarGamma);
                colDefault.Add(COL_Theta);
                colDefault.Add(COL_DollarTheta);
                colDefault.Add(COL_Vega);
                colDefault.Add(COL_DollarVega);
                colDefault.Add(COL_Rho);
                colDefault.Add(COL_DollarRho);
                colDefault.Add(COL_DaysToExpiration);
                colDefault.Add(COL_MarketValue);
                colDefault.Add(COL_SelectedFeedPriceInBaseCurrency);
                colDefault.Add(COL_BetaAdjExposureInBaseCurrency);
                colDefault.Add(COL_CostBasisUnrealizedPnLInBaseCurrency);
                colDefault.Add(COL_DeltaAdjExposureInBaseCurrency);
                colDefault.Add(COL_DollarDeltaInBaseCurrency);
                colDefault.Add(COL_DollarGammaInBaseCurrency);
                colDefault.Add(COL_DollarRhoInBaseCurrency);
                colDefault.Add(COL_DollarThetaInBaseCurrency);
                colDefault.Add(COL_DollarVegaInBaseCurrency);
                colDefault.Add(COL_MarketValueInBaseCurrency);
                colDefault.Add(COL_SimulatedPnlInBaseCurrency);
                colDefault.Add(COL_SimulatedPriceInBaseCurrency);
                colDefault.Add(COL_SimulatedUnderlyingStockPriceInBaseCurrency);
                colDefault.Add(COL_AvgPriceInBaseCurrency);
                colDefault.Add(COL_FXRate);
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

        private List<string> GetAllDisplayableColumns()
        {
            List<string> colAll = new List<string>();
            try
            {
                List<string> colDefault = GetAllDefaultColumns();
                colAll.AddRange(colDefault);
                colAll.Add(COL_AvgPrice);
                colAll.Add(COL_Level2Name);
                colAll.Add(COL_StrikePrice);
                colAll.Add(COL_AUECLocalDate);
                colAll.Add(COL_CompanyUserName);
                colAll.Add(COL_CounterPartyName);
                colAll.Add(COL_CountryName);
                colAll.Add(COL_CurrencyName);
                colAll.Add(COL_ExchangeName);
                colAll.Add(COL_ExpirationDate);
                colAll.Add(COL_ContractMultiplier);
                colAll.Add(COL_PositionType);
                colAll.Add(COL_PutOrCall);
                colAll.Add(COL_SectorName);
                colAll.Add(COL_SubSectorName);
                colAll.Add(COL_SecurityTypeName);
                colAll.Add(COL_UnderlyingName);
                colAll.Add(COL_InterestRate);
                colAll.Add(COL_DeltaAdjPosition);
                colAll.Add(COL_CostBasisUnrealizedPnL);
                colAll.Add(COL_ProxySymbol);
                colAll.Add(COL_PositionSideMVInPortfolio);
                colAll.Add(COL_PositionSideExposureInPortfolio);
                colAll.Add(COL_DeltaAdjPositionLME);
                colAll.Add(COL_ExpirationMonth);
                colAll.Add(COL_GammaAdjPosition);
                colAll.Add(COL_UDAAsset);
                colAll.Add(COL_TradeAttribute1);
                colAll.Add(COL_TradeAttribute2);
                colAll.Add(COL_TradeAttribute3);
                colAll.Add(COL_TradeAttribute4);
                colAll.Add(COL_TradeAttribute5);
                colAll.Add(COL_TradeAttribute6);
                colAll.Add(COL_BloombergSymbol);
                colAll.Add(COL_Factset);
                colAll.Add(COL_Activ);
                colAll.Add(COL_IDCOSymbol);
                colAll.Add(COL_ISINSymbol);
                colAll.Add(COL_SedolSymbol);
                colAll.Add(COL_OSISymbol);
                colAll.Add(COL_CusipSymbol);
                colAll.Add(COL_PricingSource);
                colAll.Add(COL_Beta);
                colAll.Add(COL_BetaAdjExposure);
                colAll.Add(COL_Analyst);
                colAll.Add(COL_CountryOfRisk);
                colAll.Add(COL_CustomUDA1);
                colAll.Add(COL_CustomUDA2);
                colAll.Add(COL_CustomUDA3);
                colAll.Add(COL_CustomUDA4);
                colAll.Add(COL_CustomUDA5);
                colAll.Add(COL_CustomUDA6);
                colAll.Add(COL_CustomUDA7);
                colAll.Add(COL_Issuer);
                colAll.Add(COL_LiquidTag);
                colAll.Add(COL_MarketCap);
                colAll.Add(COL_Region);
                colAll.Add(COL_RiskCurrency);
                colAll.Add(COL_UcitsEligibleTag);
                colAll.Add(COL_BloombergSymbolWithExchangeCode);
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

        private void SetColumnFormatting()
        {
            try
            {
                ColumnsCollection columns = grdPositions.DisplayLayout.Bands[0].Columns;
                columns[COL_Quantity].Format = "#,0.0";
                columns[COL_AvgPrice].Format = "#,#.0000";
                columns[COL_Delta].Format = "#.0000";
                columns[COL_Gamma].Format = "#.0000";
                columns[COL_Theta].Format = "#.0000";
                columns[COL_Vega].Format = "#.0000";
                columns[COL_Rho].Format = "#.0000";
                columns[COL_Volatility].Format = "#,#.0000";
                columns[COL_SimulatedUnderlyingStockPrice].Format = "#,#.0000";
                columns[COL_StrikePrice].Format = "#,#.0000";
                columns[COL_SimulatedPrice].Format = "#,#.0000";
                columns[COL_SelectedFeedPrice].Format = "#,#.0000";
                columns[COL_InterestRate].Format = "#,#.0000";
                columns[COL_DeltaAdjExposure].Format = "#,0";
                columns[COL_DeltaAdjPosition].Format = "#,0";
                columns[COL_CostBasisUnrealizedPnL].Format = "#,0";
                columns[COL_SimulatedPnl].Format = "#,0";
                columns[COL_DollarDelta].Format = "#,0";
                columns[COL_DollarGamma].Format = "#,0";
                columns[COL_DollarTheta].Format = "#,0";
                columns[COL_DollarVega].Format = "#,0";
                columns[COL_DollarRho].Format = "#,0";
                columns[COL_DeltaAdjPositionLME].Format = "#,0";
                columns[COL_GammaAdjPosition].Format = "#,0.0000";
                columns[COL_MarketValue].Format = "#,0";
                columns[COL_Beta].Format = "#,#.0000";
                columns[COL_BetaAdjExposure].Format = "#,0";
                columns[COL_SelectedFeedPriceInBaseCurrency].Format = "#,#.0000";
                columns[COL_BetaAdjExposureInBaseCurrency].Format = "#,0";
                columns[COL_CostBasisUnrealizedPnLInBaseCurrency].Format = "#,0";
                columns[COL_DeltaAdjExposureInBaseCurrency].Format = "#,0";
                columns[COL_DollarDeltaInBaseCurrency].Format = "#,0";
                columns[COL_DollarGammaInBaseCurrency].Format = "#,0";
                columns[COL_DollarRhoInBaseCurrency].Format = "#,0";
                columns[COL_DollarThetaInBaseCurrency].Format = "#,0";
                columns[COL_DollarVegaInBaseCurrency].Format = "#,0";
                columns[COL_MarketValueInBaseCurrency].Format = "#,0";
                columns[COL_SimulatedPnlInBaseCurrency].Format = "#,0";
                columns[COL_SimulatedPriceInBaseCurrency].Format = "#,#.0000";
                columns[COL_SimulatedUnderlyingStockPriceInBaseCurrency].Format = "#,#.0000";
                columns[COL_AvgPriceInBaseCurrency].Format = "#,#.0000";
                columns[COL_FXRate].Format = "#,#.0000";
                columns[COL_GammaAdjPosition].Format = "#,0";
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

        private void SetColumnCustomizations(bool isApplyDefaultFilters)
        {
            try
            {
                grdPositions.CreationFilter = headerCheckBox;
                UltraGridBand band = grdPositions.DisplayLayout.Bands[0];
                band.Columns[COL_IsChecked].CellClickAction = CellClickAction.Edit;
                band.Columns[COL_IsChecked].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                band.Columns[COL_IsChecked].Header.Caption = "";
                band.Columns[COL_IsChecked].Header.FixedHeaderIndicator = FixedHeaderIndicator.None;
                band.Columns[COL_IsChecked].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                band.Columns[COL_IsChecked].AllowGroupBy = DefaultableBoolean.False;
                band.Columns[COL_Quantity].Header.Caption = "Position";
                band.Columns[COL_AssetName].Header.Caption = "Asset";
                band.Columns[COL_AUECLocalDate].Header.Caption = "Trade Date";
                band.Columns[COL_AvgPrice].Header.Caption = "Cost Basis (Local)";
                band.Columns[COL_CompanyUserName].Header.Caption = "User";
                band.Columns[COL_CostBasisUnrealizedPnL].Header.Caption = "Cost Basis P&L (Local)";
                band.Columns[COL_CounterPartyName].Header.Caption = ApplicationConstants.CONST_BROKER;
                band.Columns[COL_CountryName].Header.Caption = "Country";
                band.Columns[COL_CurrencyName].Header.Caption = "Currency";
                band.Columns[COL_SimulatedPnl].Header.Caption = "Simulated P&L (Local)";
                band.Columns[COL_DaysToExpiration].Header.Caption = "Days To Expiration";
                band.Columns[COL_ExchangeName].Header.Caption = "Exchange";
                band.Columns[COL_ExpirationDate].Header.Caption = "Expiration Date";
                band.Columns[COL_Volatility].Header.Caption = "Volatility (%)";
                band.Columns[COL_InterestRate].Header.Caption = "Interest Rate (%)";
                band.Columns[COL_SelectedFeedPrice].Header.Caption = "Px Selected Feed (Local)";
                band.Columns[COL_SimulatedPrice].Header.Caption = "Simulated Price (Local)";
                band.Columns[COL_PositionType].Header.Caption = "Position Type";
                band.Columns[COL_SectorName].Header.Caption = "Sector";
                band.Columns[COL_CompanyName].Header.Caption = "Security Name";
                band.Columns[COL_SecurityTypeName].Header.Caption = "Security Type";
                band.Columns[COL_SimulatedUnderlyingStockPrice].Header.Caption = "Simulated Underlying Price (Local)";
                band.Columns[COL_StrikePrice].Header.Caption = "Strike Price";
                band.Columns[COL_SubSectorName].Header.Caption = "Sub Sector";
                band.Columns[COL_UnderlyingName].Header.Caption = "Underlying";
                band.Columns[COL_UnderlyingSymbol].Header.Caption = "Underlying Symbol";
                band.Columns[COL_DeltaAdjExposure].Header.Caption = "Delta Exposure (Local)";
                band.Columns[COL_DeltaAdjPosition].Header.Caption = "Delta Position";
                band.Columns[COL_PutOrCall].Header.Caption = "Put/Call";
                band.Columns[COL_DollarDelta].Header.Caption = "Dollar Delta (Local)";
                band.Columns[COL_DollarGamma].Header.Caption = "Dollar Gamma (Local)";
                band.Columns[COL_DollarTheta].Header.Caption = "Dollar Theta (Local)";
                band.Columns[COL_DollarVega].Header.Caption = "Dollar Vega (Local)";
                band.Columns[COL_DollarRho].Header.Caption = "Dollar Rho (Local)";
                band.Columns[COL_Level1Name].Header.Caption = "Account";
                band.Columns[COL_Level2Name].Header.Caption = "Strategy";
                band.Columns[COL_ProxySymbol].Header.Caption = "Proxy Symbol";
                band.Columns[COL_MasterFund].Header.Caption = "Master Fund";
                band.Columns[COL_PositionSideMVInPortfolio].Header.Caption = "Position Side (Portfolio)";
                band.Columns[COL_PositionSideExposureInPortfolio].Header.Caption = "Position Side Exposure (Portfolio)";
                band.Columns[COL_PositionType].Header.Caption = "Position Side";
                band.Columns[COL_UDAAsset].Header.Caption = "User Asset";
                band.Columns[COL_AUECLocalDate].CellActivation = Activation.NoEdit;
                band.Columns[COL_ExpirationDate].CellActivation = Activation.NoEdit;
                band.Columns[COL_PutOrCall].CellActivation = Activation.NoEdit;
                band.Columns[COL_DeltaAdjPositionLME].Header.Caption = "Delta Position (LME)";
                band.Columns[COL_ExpirationMonth].Header.Caption = "Expiration Month";
                band.Columns[COL_ExpirationMonth].CellActivation = Activation.NoEdit;
                band.Columns[COL_GammaAdjPosition].Header.Caption = "Gamma Position";
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
                band.Columns[COL_SedolSymbol].Header.Caption = "SEDOL";
                band.Columns[COL_OSISymbol].Header.Caption = "OSI";
                band.Columns[COL_CusipSymbol].Header.Caption = "CUSIP";
                band.Columns[COL_MarketValue].Header.Caption = "Market Value (Local)";
                band.Columns[COL_PricingSource].Header.Caption = "Pricing Source";
                band.Columns[COL_Beta].Header.Caption = "Beta";
                band.Columns[COL_BetaAdjExposure].Header.Caption = "Beta Adj Exposure (Local)";
                band.Columns[COL_Analyst].Header.Caption = "Analyst";
                band.Columns[COL_CountryOfRisk].Header.Caption = "Country Of Risk";
                band.Columns[COL_CustomUDA1].Header.Caption = "Custom UDA1";
                band.Columns[COL_CustomUDA2].Header.Caption = "Custom UDA2";
                band.Columns[COL_CustomUDA3].Header.Caption = "Custom UDA3";
                band.Columns[COL_CustomUDA4].Header.Caption = "Custom UDA4";
                band.Columns[COL_CustomUDA5].Header.Caption = "Custom UDA5";
                band.Columns[COL_CustomUDA6].Header.Caption = "Custom UDA6";
                band.Columns[COL_CustomUDA7].Header.Caption = "Custom UDA7";
                band.Columns[COL_Issuer].Header.Caption = "Issuer";
                band.Columns[COL_LiquidTag].Header.Caption = "Liquid Tag";
                band.Columns[COL_MarketCap].Header.Caption = "Market Cap UDA";
                band.Columns[COL_Region].Header.Caption = "Region";
                band.Columns[COL_RiskCurrency].Header.Caption = "Risk Currency";
                band.Columns[COL_UcitsEligibleTag].Header.Caption = "UCITS Eligible Tag";
                band.Columns[COL_SelectedFeedPriceInBaseCurrency].Header.Caption = "Px Selected Feed (Base)";
                band.Columns[COL_BetaAdjExposureInBaseCurrency].Header.Caption = "Beta Adj Exposure (Base)";
                band.Columns[COL_CostBasisUnrealizedPnLInBaseCurrency].Header.Caption = "Cost Basis P&L (Base)";
                band.Columns[COL_DeltaAdjExposureInBaseCurrency].Header.Caption = "Delta Exposure (Base)";
                band.Columns[COL_DollarDeltaInBaseCurrency].Header.Caption = "Dollar Delta (Base)";
                band.Columns[COL_DollarGammaInBaseCurrency].Header.Caption = "Dollar Gamma (Base)";
                band.Columns[COL_DollarRhoInBaseCurrency].Header.Caption = "Dollar Rho (Base)";
                band.Columns[COL_DollarThetaInBaseCurrency].Header.Caption = "Dollar Theta (Base)";
                band.Columns[COL_DollarVegaInBaseCurrency].Header.Caption = "Dollar Vega (Base)";
                band.Columns[COL_MarketValueInBaseCurrency].Header.Caption = "Market Value (Base)";
                band.Columns[COL_SimulatedPnlInBaseCurrency].Header.Caption = "Simulated P&L (Base)";
                band.Columns[COL_SimulatedPriceInBaseCurrency].Header.Caption = "Simulated Price (Base)";
                band.Columns[COL_SimulatedUnderlyingStockPriceInBaseCurrency].Header.Caption = "Simulated Underlying Price (Base)";
                band.Columns[COL_AvgPriceInBaseCurrency].Header.Caption = "Cost Basis (Base)";
                band.Columns[COL_FXRate].Header.Caption = "FX Rate";

                if (isApplyDefaultFilters)
                {
                    band.ColumnFilters[COL_AssetName].FilterConditions.Add(FilterComparisionOperator.Equals, "EquityOption");
                    grdPositions.DisplayLayout.Override.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                }
                foreach (UltraGridColumn col in grdPositions.DisplayLayout.Bands[0].Columns)
                {
                    col.GroupByComparer = _groupSortComparer;
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

        private void SetColumnSummaries(UltraGrid grdPosition)
        {
            try
            {
                UltraGridBand band = grdPosition.DisplayLayout.Bands[0];
                RiskSummaryFactory summFactory = new RiskSummaryFactory();

                UltraGridColumn colDeltaAdjExposure = band.Columns[COL_DeltaAdjExposure];
                UltraGridColumn colDeltaAdjPosition = band.Columns[COL_DeltaAdjPosition];
                UltraGridColumn colCostBasisUnrealizedPnL = band.Columns[COL_CostBasisUnrealizedPnL];
                UltraGridColumn colSimulatedPnl = band.Columns[COL_SimulatedPnl];
                UltraGridColumn colDelta = band.Columns[COL_Delta];
                UltraGridColumn colGamma = band.Columns[COL_Gamma];
                UltraGridColumn colTheta = band.Columns[COL_Theta];
                UltraGridColumn colVega = band.Columns[COL_Vega];
                UltraGridColumn colRho = band.Columns[COL_Rho];
                UltraGridColumn colDollarDelta = band.Columns[COL_DollarDelta];
                UltraGridColumn colDollarGamma = band.Columns[COL_DollarGamma];
                UltraGridColumn colDollarTheta = band.Columns[COL_DollarTheta];
                UltraGridColumn colDollarVega = band.Columns[COL_DollarVega];
                UltraGridColumn colDollarRho = band.Columns[COL_DollarRho];
                UltraGridColumn colSymbol = band.Columns[COL_Symbol];
                UltraGridColumn colProxySymbol = band.Columns[COL_ProxySymbol];
                UltraGridColumn colSecurityName = band.Columns[COL_CompanyName];
                UltraGridColumn colUnderlyingSymbol = band.Columns[COL_UnderlyingSymbol];
                UltraGridColumn colStrikePrice = band.Columns[COL_StrikePrice];
                UltraGridColumn colAccount = band.Columns[COL_Level1Name];
                UltraGridColumn colStrategy = band.Columns[COL_Level2Name];
                UltraGridColumn colAssetName = band.Columns[COL_AssetName];
                UltraGridColumn colQuantity = band.Columns[COL_Quantity];
                UltraGridColumn colAvgPrice = band.Columns[COL_AvgPrice];
                UltraGridColumn colVolatility = band.Columns[COL_Volatility];
                UltraGridColumn colSelectedFeedPrice = band.Columns[COL_SelectedFeedPrice];
                UltraGridColumn colSimulatedUnderlyingStockPrice = band.Columns[COL_SimulatedUnderlyingStockPrice];
                UltraGridColumn colSimulatedPrice = band.Columns[COL_SimulatedPrice];
                UltraGridColumn colDaysToExpiration = band.Columns[COL_DaysToExpiration];
                UltraGridColumn colInterestRate = band.Columns[COL_InterestRate];
                UltraGridColumn colAUECLocalDate = band.Columns[COL_AUECLocalDate];
                UltraGridColumn colCompanyUserName = band.Columns[COL_CompanyUserName];
                UltraGridColumn colCounterPartyName = band.Columns[COL_CounterPartyName];
                UltraGridColumn colCountryName = band.Columns[COL_CountryName];
                UltraGridColumn colCurrencyName = band.Columns[COL_CurrencyName];
                UltraGridColumn colExchangeName = band.Columns[COL_ExchangeName];
                UltraGridColumn colExpirationDate = band.Columns[COL_ExpirationDate];
                UltraGridColumn colMultiplier = band.Columns[COL_ContractMultiplier];
                UltraGridColumn colPositionType = band.Columns[COL_PositionType];
                UltraGridColumn colPutOrCall = band.Columns[COL_PutOrCall];
                UltraGridColumn colSectorName = band.Columns[COL_SectorName];
                UltraGridColumn colSubSectorName = band.Columns[COL_SubSectorName];
                UltraGridColumn colSecurityTypeName = band.Columns[COL_SecurityTypeName];
                UltraGridColumn colUnderlyingName = band.Columns[COL_UnderlyingName];
                UltraGridColumn colMasterFund = band.Columns[COL_MasterFund];
                UltraGridColumn colPositionSideMVInPortfolio = band.Columns[COL_PositionSideMVInPortfolio];
                UltraGridColumn colPositionSideExposureInPortfolio = band.Columns[COL_PositionSideExposureInPortfolio];
                UltraGridColumn colBloombergSymbol = band.Columns[COL_BloombergSymbol];
                UltraGridColumn colBloombergSymbolWithExchangeCode = band.Columns[COL_BloombergSymbolWithExchangeCode];
                UltraGridColumn colFactsetSymbol = band.Columns[COL_Factset];
                UltraGridColumn colActivSymbol = band.Columns[COL_Activ];
                UltraGridColumn colDeltaAdjPositionLME = band.Columns[COL_DeltaAdjPositionLME];
                UltraGridColumn colExpirationMonth = band.Columns[COL_ExpirationMonth];
                UltraGridColumn colGammaAdjPosition = band.Columns[COL_GammaAdjPosition];
                UltraGridColumn colUDAAsset = band.Columns[COL_UDAAsset];
                UltraGridColumn colMarketValue = band.Columns[COL_MarketValue];
                UltraGridColumn colPricingSource = band.Columns[COL_PricingSource];
                UltraGridColumn colTradeAttribute1 = band.Columns[COL_TradeAttribute1];
                UltraGridColumn colTradeAttribute2 = band.Columns[COL_TradeAttribute2];
                UltraGridColumn colTradeAttribute3 = band.Columns[COL_TradeAttribute3];
                UltraGridColumn colTradeAttribute4 = band.Columns[COL_TradeAttribute4];
                UltraGridColumn colTradeAttribute5 = band.Columns[COL_TradeAttribute5];
                UltraGridColumn colTradeAttribute6 = band.Columns[COL_TradeAttribute6];
                UltraGridColumn colIDCOSymbol = band.Columns[COL_IDCOSymbol];
                UltraGridColumn colISINSymbol = band.Columns[COL_ISINSymbol];
                UltraGridColumn colSedolSymbol = band.Columns[COL_SedolSymbol];
                UltraGridColumn colOSISymbol = band.Columns[COL_OSISymbol];
                UltraGridColumn colCusipSymbol = band.Columns[COL_CusipSymbol];
                UltraGridColumn colBeta = band.Columns[COL_Beta];
                UltraGridColumn colBetaAdjExposure = band.Columns[COL_BetaAdjExposure];
                UltraGridColumn colAnalyst = band.Columns[COL_Analyst];
                UltraGridColumn colCountryOfRisk = band.Columns[COL_CountryOfRisk];
                UltraGridColumn colCustomUDA1 = band.Columns[COL_CustomUDA1];
                UltraGridColumn colCustomUDA2 = band.Columns[COL_CustomUDA2];
                UltraGridColumn colCustomUDA3 = band.Columns[COL_CustomUDA3];
                UltraGridColumn colCustomUDA4 = band.Columns[COL_CustomUDA4];
                UltraGridColumn colCustomUDA5 = band.Columns[COL_CustomUDA5];
                UltraGridColumn colCustomUDA6 = band.Columns[COL_CustomUDA6];
                UltraGridColumn colCustomUDA7 = band.Columns[COL_CustomUDA7];
                UltraGridColumn colIssuer = band.Columns[COL_Issuer];
                UltraGridColumn colLiquidTag = band.Columns[COL_LiquidTag];
                UltraGridColumn colMarketCap = band.Columns[COL_MarketCap];
                UltraGridColumn colRegion = band.Columns[COL_Region];
                UltraGridColumn colRiskCurrency = band.Columns[COL_RiskCurrency];
                UltraGridColumn colUcitsEligibleTag = band.Columns[COL_UcitsEligibleTag];
                UltraGridColumn colSelectedFeedPriceInBaseCurrency = band.Columns[COL_SelectedFeedPriceInBaseCurrency];
                UltraGridColumn colBetaAdjExposureInBaseCurrency = band.Columns[COL_BetaAdjExposureInBaseCurrency];
                UltraGridColumn colCostBasisUnrealizedPnLInBaseCurrency = band.Columns[COL_CostBasisUnrealizedPnLInBaseCurrency];
                UltraGridColumn colDeltaAdjExposureInBaseCurrency = band.Columns[COL_DeltaAdjExposureInBaseCurrency];
                UltraGridColumn colDollarDeltaInBaseCurrency = band.Columns[COL_DollarDeltaInBaseCurrency];
                UltraGridColumn colDollarGammaInBaseCurrency = band.Columns[COL_DollarGammaInBaseCurrency];
                UltraGridColumn colDollarRhoInBaseCurrency = band.Columns[COL_DollarRhoInBaseCurrency];
                UltraGridColumn colDollarThetaInBaseCurrency = band.Columns[COL_DollarThetaInBaseCurrency];
                UltraGridColumn colDollarVegaInBaseCurrency = band.Columns[COL_DollarVegaInBaseCurrency];
                UltraGridColumn colMarketValueInBaseCurrency = band.Columns[COL_MarketValueInBaseCurrency];
                UltraGridColumn colSimulatedPnlInBaseCurrency = band.Columns[COL_SimulatedPnlInBaseCurrency];
                UltraGridColumn colSimulatedPriceInBaseCurrency = band.Columns[COL_SimulatedPriceInBaseCurrency];
                UltraGridColumn colSimulatedUnderlyingStockPriceInBaseCurrency = band.Columns[COL_SimulatedUnderlyingStockPriceInBaseCurrency];
                UltraGridColumn colAvgPriceInBaseCurrency = band.Columns[COL_AvgPriceInBaseCurrency];
                UltraGridColumn colFxRate = band.Columns[COL_FXRate];

                band.Summaries.Add(colDeltaAdjPosition.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDeltaAdjPosition, SummaryPosition.UseSummaryPositionColumn, colDeltaAdjPosition);
                band.Summaries.Add(colDelta.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colDelta, SummaryPosition.UseSummaryPositionColumn, colDelta);
                band.Summaries.Add(colGamma.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colGamma, SummaryPosition.UseSummaryPositionColumn, colGamma);
                band.Summaries.Add(colTheta.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colTheta, SummaryPosition.UseSummaryPositionColumn, colTheta);
                band.Summaries.Add(colVega.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colVega, SummaryPosition.UseSummaryPositionColumn, colVega);
                band.Summaries.Add(colRho.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colRho, SummaryPosition.UseSummaryPositionColumn, colRho);
                band.Summaries.Add(colDeltaAdjExposure.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDeltaAdjExposure, SummaryPosition.UseSummaryPositionColumn, colDeltaAdjExposure);
                band.Summaries.Add(colCostBasisUnrealizedPnL.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colCostBasisUnrealizedPnL, SummaryPosition.UseSummaryPositionColumn, colCostBasisUnrealizedPnL);
                band.Summaries.Add(colSimulatedPnl.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colSimulatedPnl, SummaryPosition.UseSummaryPositionColumn, colSimulatedPnl);
                band.Summaries.Add(colDollarDelta.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDollarDelta, SummaryPosition.UseSummaryPositionColumn, colDollarDelta);
                band.Summaries.Add(colDollarGamma.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDollarGamma, SummaryPosition.UseSummaryPositionColumn, colDollarGamma);
                band.Summaries.Add(colDollarTheta.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDollarTheta, SummaryPosition.UseSummaryPositionColumn, colDollarTheta);
                band.Summaries.Add(colDollarVega.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDollarVega, SummaryPosition.UseSummaryPositionColumn, colDollarVega);
                band.Summaries.Add(colDollarRho.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDollarRho, SummaryPosition.UseSummaryPositionColumn, colDollarRho);
                band.Summaries.Add(colSymbol.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colSymbol, SummaryPosition.UseSummaryPositionColumn, colSymbol);
                band.Summaries.Add(colProxySymbol.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colProxySymbol, SummaryPosition.UseSummaryPositionColumn, colProxySymbol);
                band.Summaries.Add(colSecurityName.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colSecurityName, SummaryPosition.UseSummaryPositionColumn, colSecurityName);
                band.Summaries.Add(colUnderlyingSymbol.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colUnderlyingSymbol, SummaryPosition.UseSummaryPositionColumn, colUnderlyingSymbol);
                band.Summaries.Add(colStrikePrice.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colStrikePrice, SummaryPosition.UseSummaryPositionColumn, colStrikePrice);
                band.Summaries.Add(colAccount.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colAccount, SummaryPosition.UseSummaryPositionColumn, colAccount);
                band.Summaries.Add(colMasterFund.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colMasterFund, SummaryPosition.UseSummaryPositionColumn, colMasterFund);
                band.Summaries.Add(colBloombergSymbol.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colBloombergSymbol, SummaryPosition.UseSummaryPositionColumn, colBloombergSymbol);
                band.Summaries.Add(colStrategy.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colStrategy, SummaryPosition.UseSummaryPositionColumn, colStrategy);
                band.Summaries.Add(colAssetName.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colAssetName, SummaryPosition.UseSummaryPositionColumn, colAssetName);
                band.Summaries.Add(colQuantity.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSymbolSum"), colQuantity, SummaryPosition.UseSummaryPositionColumn, colQuantity);
                band.Summaries.Add(colAvgPrice.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcWeightedSum"), colAvgPrice, SummaryPosition.UseSummaryPositionColumn, colAvgPrice);
                band.Summaries.Add(colVolatility.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colVolatility, SummaryPosition.UseSummaryPositionColumn, colVolatility);
                band.Summaries.Add(colSelectedFeedPrice.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colSelectedFeedPrice, SummaryPosition.UseSummaryPositionColumn, colSelectedFeedPrice);
                band.Summaries.Add(colSimulatedUnderlyingStockPrice.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colSimulatedUnderlyingStockPrice, SummaryPosition.UseSummaryPositionColumn, colSimulatedUnderlyingStockPrice);
                band.Summaries.Add(colSimulatedPrice.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colSimulatedPrice, SummaryPosition.UseSummaryPositionColumn, colSimulatedPrice);
                band.Summaries.Add(colDaysToExpiration.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colDaysToExpiration, SummaryPosition.UseSummaryPositionColumn, colDaysToExpiration);
                band.Summaries.Add(colInterestRate.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colInterestRate, SummaryPosition.UseSummaryPositionColumn, colInterestRate);
                band.Summaries.Add(colSelectedFeedPriceInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colSelectedFeedPriceInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colSelectedFeedPriceInBaseCurrency);
                band.Summaries.Add(colAUECLocalDate.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), colAUECLocalDate, SummaryPosition.UseSummaryPositionColumn, colAUECLocalDate);
                band.Summaries.Add(colCompanyUserName.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCompanyUserName, SummaryPosition.UseSummaryPositionColumn, colCompanyUserName);
                band.Summaries.Add(colCounterPartyName.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCounterPartyName, SummaryPosition.UseSummaryPositionColumn, colCounterPartyName);
                band.Summaries.Add(colCountryName.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCountryName, SummaryPosition.UseSummaryPositionColumn, colCountryName);
                band.Summaries.Add(colCurrencyName.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCurrencyName, SummaryPosition.UseSummaryPositionColumn, colCurrencyName);
                band.Summaries.Add(colExchangeName.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colExchangeName, SummaryPosition.UseSummaryPositionColumn, colExchangeName);
                band.Summaries.Add(colExpirationDate.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), colExpirationDate, SummaryPosition.UseSummaryPositionColumn, colExpirationDate);
                band.Summaries.Add(colMultiplier.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colMultiplier, SummaryPosition.UseSummaryPositionColumn, colMultiplier);
                band.Summaries.Add(colPositionType.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colPositionType, SummaryPosition.UseSummaryPositionColumn, colPositionType);
                band.Summaries.Add(colPutOrCall.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colPutOrCall, SummaryPosition.UseSummaryPositionColumn, colPutOrCall);
                band.Summaries.Add(colSectorName.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colSectorName, SummaryPosition.UseSummaryPositionColumn, colSectorName);
                band.Summaries.Add(colSubSectorName.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colSubSectorName, SummaryPosition.UseSummaryPositionColumn, colSubSectorName);
                band.Summaries.Add(colSecurityTypeName.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colSecurityTypeName, SummaryPosition.UseSummaryPositionColumn, colSecurityTypeName);
                band.Summaries.Add(colUDAAsset.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colUDAAsset, SummaryPosition.UseSummaryPositionColumn, colUDAAsset);
                band.Summaries.Add(colUnderlyingName.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colUnderlyingName, SummaryPosition.UseSummaryPositionColumn, colUnderlyingName);
                band.Summaries.Add(colPositionSideMVInPortfolio.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colPositionSideMVInPortfolio, SummaryPosition.UseSummaryPositionColumn, colPositionSideMVInPortfolio);
                band.Summaries.Add(colPositionSideExposureInPortfolio.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colPositionSideExposureInPortfolio, SummaryPosition.UseSummaryPositionColumn, colPositionSideExposureInPortfolio);
                band.Summaries.Add(colDeltaAdjPositionLME.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colDeltaAdjPositionLME, SummaryPosition.UseSummaryPositionColumn, colDeltaAdjPositionLME);
                band.Summaries.Add(colExpirationMonth.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcDate"), colExpirationMonth, SummaryPosition.UseSummaryPositionColumn, colExpirationMonth);
                band.Summaries.Add(colGammaAdjPosition.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colGammaAdjPosition, SummaryPosition.UseSummaryPositionColumn, colGammaAdjPosition);
                band.Summaries.Add(colMarketValue.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colMarketValue, SummaryPosition.UseSummaryPositionColumn, colMarketValue);
                band.Summaries.Add(colPricingSource.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colPricingSource, SummaryPosition.UseSummaryPositionColumn, colPricingSource);
                band.Summaries.Add(colTradeAttribute1.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colTradeAttribute1, SummaryPosition.UseSummaryPositionColumn, colTradeAttribute1);
                band.Summaries.Add(colTradeAttribute2.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colTradeAttribute2, SummaryPosition.UseSummaryPositionColumn, colTradeAttribute2);
                band.Summaries.Add(colTradeAttribute3.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colTradeAttribute3, SummaryPosition.UseSummaryPositionColumn, colTradeAttribute3);
                band.Summaries.Add(colTradeAttribute4.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colTradeAttribute4, SummaryPosition.UseSummaryPositionColumn, colTradeAttribute4);
                band.Summaries.Add(colTradeAttribute5.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colTradeAttribute5, SummaryPosition.UseSummaryPositionColumn, colTradeAttribute5);
                band.Summaries.Add(colTradeAttribute6.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colTradeAttribute6, SummaryPosition.UseSummaryPositionColumn, colTradeAttribute6);
                band.Summaries.Add(colIDCOSymbol.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colIDCOSymbol, SummaryPosition.UseSummaryPositionColumn, colIDCOSymbol);
                band.Summaries.Add(colISINSymbol.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colISINSymbol, SummaryPosition.UseSummaryPositionColumn, colISINSymbol);
                band.Summaries.Add(colSedolSymbol.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colSedolSymbol, SummaryPosition.UseSummaryPositionColumn, colSedolSymbol);
                band.Summaries.Add(colOSISymbol.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colOSISymbol, SummaryPosition.UseSummaryPositionColumn, colOSISymbol);
                band.Summaries.Add(colCusipSymbol.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCusipSymbol, SummaryPosition.UseSummaryPositionColumn, colCusipSymbol);
                band.Summaries.Add(colBeta.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), colBeta, SummaryPosition.UseSummaryPositionColumn, colBeta);
                band.Summaries.Add(colBetaAdjExposure.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colBetaAdjExposure, SummaryPosition.UseSummaryPositionColumn, colBetaAdjExposure);
                band.Summaries.Add(colAnalyst.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colAnalyst, SummaryPosition.UseSummaryPositionColumn, colAnalyst);
                band.Summaries.Add(colCountryOfRisk.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCountryOfRisk, SummaryPosition.UseSummaryPositionColumn, colCountryOfRisk);
                band.Summaries.Add(colCustomUDA1.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCustomUDA1, SummaryPosition.UseSummaryPositionColumn, colCustomUDA1);
                band.Summaries.Add(colCustomUDA2.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCustomUDA2, SummaryPosition.UseSummaryPositionColumn, colCustomUDA2);
                band.Summaries.Add(colCustomUDA3.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCustomUDA3, SummaryPosition.UseSummaryPositionColumn, colCustomUDA3);
                band.Summaries.Add(colCustomUDA4.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCustomUDA4, SummaryPosition.UseSummaryPositionColumn, colCustomUDA4);
                band.Summaries.Add(colCustomUDA5.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCustomUDA5, SummaryPosition.UseSummaryPositionColumn, colCustomUDA5);
                band.Summaries.Add(colCustomUDA6.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCustomUDA6, SummaryPosition.UseSummaryPositionColumn, colCustomUDA6);
                band.Summaries.Add(colCustomUDA7.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colCustomUDA7, SummaryPosition.UseSummaryPositionColumn, colCustomUDA7);
                band.Summaries.Add(colIssuer.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colIssuer, SummaryPosition.UseSummaryPositionColumn, colIssuer);
                band.Summaries.Add(colLiquidTag.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colLiquidTag, SummaryPosition.UseSummaryPositionColumn, colLiquidTag);
                band.Summaries.Add(colMarketCap.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colMarketCap, SummaryPosition.UseSummaryPositionColumn, colMarketCap);
                band.Summaries.Add(colRegion.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colRegion, SummaryPosition.UseSummaryPositionColumn, colRegion);
                band.Summaries.Add(colRiskCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colRiskCurrency, SummaryPosition.UseSummaryPositionColumn, colRiskCurrency);
                band.Summaries.Add(colUcitsEligibleTag.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colUcitsEligibleTag, SummaryPosition.UseSummaryPositionColumn, colUcitsEligibleTag);
                band.Summaries.Add(colBetaAdjExposureInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colBetaAdjExposureInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colBetaAdjExposureInBaseCurrency);
                band.Summaries.Add(colCostBasisUnrealizedPnLInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colCostBasisUnrealizedPnLInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colCostBasisUnrealizedPnLInBaseCurrency);
                band.Summaries.Add(colDeltaAdjExposureInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colDeltaAdjExposureInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colDeltaAdjExposureInBaseCurrency);
                band.Summaries.Add(colDollarDeltaInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDollarDeltaInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colDollarDeltaInBaseCurrency);
                band.Summaries.Add(colDollarGammaInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDollarGammaInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colDollarGammaInBaseCurrency);
                band.Summaries.Add(colDollarRhoInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDollarRhoInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colDollarRhoInBaseCurrency);
                band.Summaries.Add(colDollarThetaInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDollarThetaInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colDollarThetaInBaseCurrency);
                band.Summaries.Add(colDollarVegaInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcLocalColumns"), colDollarVegaInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colDollarVegaInBaseCurrency);
                band.Summaries.Add(colMarketValueInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colMarketValueInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colMarketValueInBaseCurrency);
                band.Summaries.Add(colSimulatedPnlInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), colSimulatedPnlInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colSimulatedPnlInBaseCurrency);
                band.Summaries.Add(colSimulatedPriceInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colSimulatedPriceInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colSimulatedPriceInBaseCurrency);
                band.Summaries.Add(colSimulatedUnderlyingStockPriceInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colSimulatedUnderlyingStockPriceInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colSimulatedUnderlyingStockPriceInBaseCurrency);
                band.Summaries.Add(colAvgPriceInBaseCurrency.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colAvgPriceInBaseCurrency, SummaryPosition.UseSummaryPositionColumn, colAvgPriceInBaseCurrency);
                band.Summaries.Add(colFxRate.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), colFxRate, SummaryPosition.UseSummaryPositionColumn, colFxRate);
                foreach (SummarySettings summary in band.Summaries)
                {
                    summary.DisplayFormat = "{0}";
                }
                band.Summaries[colDeltaAdjExposure.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDeltaAdjPosition.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colCostBasisUnrealizedPnL.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colSimulatedPnl.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDelta.Key].DisplayFormat = "{0:#.0000}";
                band.Summaries[colGamma.Key].DisplayFormat = "{0:#.0000}";
                band.Summaries[colTheta.Key].DisplayFormat = "{0:#.0000}";
                band.Summaries[colVega.Key].DisplayFormat = "{0:#.0000}";
                band.Summaries[colRho.Key].DisplayFormat = "{0:#.0000}";
                band.Summaries[colDollarDelta.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDollarGamma.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDollarTheta.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDollarVega.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDollarRho.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colQuantity.Key].DisplayFormat = "{0:#,0.0#}";
                band.Summaries[colAvgPrice.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colVolatility.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colSimulatedPrice.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colInterestRate.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colSelectedFeedPrice.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colSimulatedUnderlyingStockPrice.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colStrikePrice.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colAUECLocalDate.Key].DisplayFormat = "{0:MM/dd/yyyy}";
                band.Summaries[colExpirationDate.Key].DisplayFormat = "{0:MM/dd/yyyy}";
                band.Summaries[colExpirationMonth.Key].DisplayFormat = "{0:MMMM yyyy}";
                band.Summaries[colSelectedFeedPriceInBaseCurrency.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colSimulatedPnl.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDeltaAdjPositionLME.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colGammaAdjPosition.Key].DisplayFormat = "{0:#.0000}";
                band.Summaries[colMarketValue.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colBeta.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colBetaAdjExposure.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colBetaAdjExposureInBaseCurrency.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colCostBasisUnrealizedPnLInBaseCurrency.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDeltaAdjExposureInBaseCurrency.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDollarDeltaInBaseCurrency.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDollarGammaInBaseCurrency.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDollarRhoInBaseCurrency.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDollarThetaInBaseCurrency.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colDollarVegaInBaseCurrency.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colMarketValueInBaseCurrency.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colAvgPriceInBaseCurrency.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colFxRate.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colSimulatedPriceInBaseCurrency.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colSimulatedUnderlyingStockPriceInBaseCurrency.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colAvgPriceInBaseCurrency.Key].DisplayFormat = "{0:#,#.0000}";
                band.Summaries[colSimulatedPnlInBaseCurrency.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colGammaAdjPosition.Key].DisplayFormat = "{0:#,0}";
                band.Summaries[colMultiplier.Key].DisplayFormat = "{0:#,0}";

                grdPosition.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.Bottom;
                grdPosition.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.InGroupByRows;
                grdPosition.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.RootRowsFootersOnly;
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

        private void SetColumnFormattingGraph(ColumnsCollection columns)
        {
            try
            {
                columns[COL_Delta].Format = "#.0000";
                columns[COL_Gamma].Format = "#.0000";
                columns[COL_Theta].Format = "#.0000";
                columns[COL_Vega].Format = "#.0000";
                columns[COL_Rho].Format = "#.0000";
                columns[COL_DollarDeltaInBaseCurrency].Format = "#,0";
                columns[COL_DollarGammaInBaseCurrency].Format = "#,0";
                columns[COL_DollarThetaInBaseCurrency].Format = "#,0";
                columns[COL_DollarVegaInBaseCurrency].Format = "#,0";
                columns[COL_DollarRhoInBaseCurrency].Format = "#,0";
                columns[COL_SimulatedPriceInBaseCurrency].Format = "#,#.0000";
                columns[COL_DeltaAdjExposureInBaseCurrency].Format = "#,0";
                columns[COL_CostBasisUnrealizedPnLInBaseCurrency].Format = "#,#.0000";
                columns[COL_SimulatedPnlInBaseCurrency].Format = "#,#.0000";
                columns[COL_InterestRate].Format = "#,#.0000";
                columns[COL_DollarDeltaInBaseCurrency].Header.Caption = "Dollar Delta (Base)";
                columns[COL_DollarGammaInBaseCurrency].Header.Caption = "Dollar Gamma (Base)";
                columns[COL_DollarThetaInBaseCurrency].Header.Caption = "Dollar Theta (Base)";
                columns[COL_DollarVegaInBaseCurrency].Header.Caption = "Dollar Vega (Base)";
                columns[COL_DollarRhoInBaseCurrency].Header.Caption = "Dollar Rho (Base)";
                columns[COL_SimulatedPriceInBaseCurrency].Header.Caption = "Simulated Price (Base)";
                columns[COL_DeltaAdjExposureInBaseCurrency].Header.Caption = "Delta Exposure (Base)";
                columns[COL_CostBasisUnrealizedPnLInBaseCurrency].Header.Caption = "Cost Basis P&L (Base)";
                columns[COL_SimulatedPnlInBaseCurrency].Header.Caption = "Simulated P&L (Base)";
                columns[COL_InterestRate].Header.Caption = "Interest %";
                columns[COL_UnderlyingPrice].Header.Caption = "Underlying Price (Base)";
                columns[COL_UnderlyingSymbol].Header.Caption = "Underlying Symbol";
                columns[COL_DaysToExpiration].Header.Caption = "Days To Expiration";

                if (columns.Exists(StepAnalParameterCode.UnderlyingPrice + "MUL"))
                    columns[StepAnalParameterCode.UnderlyingPrice + "MUL"].Header.Caption = EnumHelper.GetDescription(StepAnalParameterCode.UnderlyingPrice) + " (Multiple)";

                if (columns.Exists(StepAnalParameterCode.Volatility + "MUL"))
                    columns[StepAnalParameterCode.Volatility + "MUL"].Header.Caption = EnumHelper.GetDescription(StepAnalParameterCode.Volatility) + " (Multiple)";

                if (columns.Exists(StepAnalParameterCode.DaysToExpiration + "MUL"))
                    columns[StepAnalParameterCode.DaysToExpiration + "MUL"].Header.Caption = EnumHelper.GetDescription(StepAnalParameterCode.DaysToExpiration) + " (Multiple)";

                if (columns.Exists(StepAnalParameterCode.InterestRate + "MUL"))
                    columns[StepAnalParameterCode.InterestRate + "MUL"].Header.Caption = EnumHelper.GetDescription(StepAnalParameterCode.InterestRate) + " (Multiple)";

                grdData.DisplayLayout.Override.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
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

        public List<PranaPositionWithGreeks> GetSelectedRows()
        {
            List<PranaPositionWithGreeks> list = new List<PranaPositionWithGreeks>();
            try
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow[] rows = grdPositions.Rows.GetFilteredInNonGroupByRows();
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in rows)
                {
                    if (row.Cells[COL_IsChecked].Text.ToUpper().Equals("TRUE"))
                    {
                        PranaPositionWithGreeks pranaPosWithGreeks = (PranaPositionWithGreeks)row.ListObject;
                        list.Add(pranaPosWithGreeks);
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
            return list;
        }

        public List<PranaPositionWithGreeks> GetUniqueSelectedRows()
        {
            List<PranaPositionWithGreeks> list = new List<PranaPositionWithGreeks>();
            Infragistics.Win.UltraWinGrid.UltraGridRow[] rows = grdPositions.Rows.GetFilteredInNonGroupByRows();
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in rows)
            {
                if (row.Cells[COL_IsChecked].Text.ToUpper().Equals("TRUE"))
                {
                    PranaPositionWithGreeks pranaPosWithGreeks = (PranaPositionWithGreeks)row.ListObject;
                    if (list.Count < 1)
                    {
                        list.Add(pranaPosWithGreeks);
                    }
                    else
                    {
                        bool found = false;
                        foreach (PranaPositionWithGreeks pos in list)
                        {
                            if (pos.Symbol == pranaPosWithGreeks.Symbol)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            list.Add(pranaPosWithGreeks);
                        }
                    }
                }
            }
            return list;
        }

        public Dictionary<string, List<PranaPositionWithGreeks>> GetUniqueSelectedRowsDict()
        {
            Dictionary<string, List<PranaPositionWithGreeks>> dictSymbolList = new Dictionary<string, List<PranaPositionWithGreeks>>();
            try
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow[] rows = grdPositions.Rows.GetFilteredInNonGroupByRows();

                if (rows != null)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in rows)
                    {
                        if (row != null)
                        {
                            if (row.Cells[COL_IsChecked].Text.ToUpper().Equals("TRUE"))
                            {
                                PranaPositionWithGreeks pranaPosWithGreeks = (PranaPositionWithGreeks)row.ListObject;
                                if (dictSymbolList.ContainsKey(pranaPosWithGreeks.Symbol))
                                {
                                    dictSymbolList[pranaPosWithGreeks.Symbol].Add(pranaPosWithGreeks);
                                }
                                else
                                {
                                    List<PranaPositionWithGreeks> listPranaPos = new List<PranaPositionWithGreeks>();
                                    listPranaPos.Add(pranaPosWithGreeks);
                                    dictSymbolList.Add(pranaPosWithGreeks.Symbol, listPranaPos);
                                }
                            }
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
            return dictSymbolList;
        }

        public List<PranaPositionWithGreeks> GetSelectedNonOptionRows()
        {
            List<PranaPositionWithGreeks> listNonOption = new List<PranaPositionWithGreeks>();
            try
            {
                List<PranaPositionWithGreeks> list = GetSelectedRows();
                foreach (PranaPositionWithGreeks position in list)
                {
                    int baseAssetID = Mapper.GetBaseAsset(position.AssetID);
                    if (position.IsChecked && baseAssetID != (int)AssetCategory.Option)
                    {
                        listNonOption.Add(position);
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
            return listNonOption;
        }

        public List<PranaPositionWithGreeks> GetSelectedRows(string symbol)
        {
            List<PranaPositionWithGreeks> list = new List<PranaPositionWithGreeks>();
            try
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow[] rows = grdPositions.Rows.GetFilteredInNonGroupByRows();
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in rows)
                {
                    if (row.Cells[COL_IsChecked].Text.ToUpper().Equals("TRUE") && row.Cells[COL_Symbol].Text.ToUpper().Equals(symbol))
                    {
                        PranaPositionWithGreeks pranaPosWithGreeks = (PranaPositionWithGreeks)row.ListObject;
                        list.Add(pranaPosWithGreeks);
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
            return list;
        }

        private List<PranaPositionWithGreeks> GetAllCheckedRows(string symbol)
        {
            List<PranaPositionWithGreeks> list = new List<PranaPositionWithGreeks>();
            try
            {
                if (!_dictBindedData.ContainsKey(symbol))
                {
                    return list;
                }
                foreach (PranaPositionWithGreeks position in _dictBindedData[symbol])
                {
                    if (position.IsChecked)
                    {
                        list.Add(position);
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
            return list;
        }

        private List<PranaPositionWithGreeks> GetAllRows(string symbol)
        {
            List<PranaPositionWithGreeks> list = new List<PranaPositionWithGreeks>();
            try
            {
                if (!_dictBindedData.ContainsKey(symbol))
                {
                    return list;
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
            return _dictBindedData[symbol];
        }

        private List<PranaPositionWithGreeks> GetSelectedOptionRows()
        {
            List<PranaPositionWithGreeks> listOptions = new List<PranaPositionWithGreeks>();
            try
            {
                List<PranaPositionWithGreeks> list = GetUniqueSelectedRows();
                foreach (PranaPositionWithGreeks position in list)
                {
                    int baseAssetID = Mapper.GetBaseAsset(position.AssetID);
                    if (position.IsChecked && baseAssetID == (int)AssetCategory.Option)
                    {
                        listOptions.Add(position);
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
            return listOptions;
        }

        public InputParametersCollection GetInputParametersForSimulation(InputParametersCollection inputParametersCollection)
        {
            try
            {
                _responseReceivedCount = 0;
                _listRequestedSymbols.Clear();
                bool useNonParallelShifts = false;
                bool useVolShockAdjFactor = false;
                _isStressTestRequest = true;

                SubscriberViewInputs inputs = new SubscriberViewInputs();
                inputs.HashCode = this.GetHashCode();
                inputs.ID = _viewID;
                inputs.IsStressTestRequest = _isStressTestRequest;
                if (_preferences != null)
                {
                    useNonParallelShifts = _preferences.UseNonParallelShifts;
                    useVolShockAdjFactor = _preferences.UseVolShockAdjustment;
                }

                OptionSimulationInputs OptionSimInputsPortfolio = new OptionSimulationInputs();
                if (!useNonParallelShifts)
                {
                    if (chkBoxUseVolSkew.Checked && (chkbxUnderLyingPrice.Checked || ckhbxExpiration.Checked))
                    {
                        inputs.IsVolSkewRequest = true;
                    }
                    if (chkbxVol.Checked)
                    {
                        OptionSimInputsPortfolio.ChangeVolatility = (1 + Convert.ToDouble((numericUpDownVol.Value / 100)));
                    }
                    if (chkbxUnderLyingPrice.Checked)
                    {
                        //Bharat Kumar Jangir (11 December 2013)
                        //Check for Underlying Price used as absolute value or percentage value
                        if (checkBoxUseAbsoluteValuesForUnderlyingPrice.Checked)
                        {
                            OptionSimInputsPortfolio.ChangeUnderlyingPrice = Convert.ToDouble(numericUpDownUnderLyingPrice.Value);
                            OptionSimInputsPortfolio.UnderlyingPriceAsAbosluteValue = true;
                        }
                        else
                        {
                            OptionSimInputsPortfolio.ChangeUnderlyingPrice = (1 + Convert.ToDouble((numericUpDownUnderLyingPrice.Value / 100)));
                        }
                    }
                    if (chkbxInterestRate.Checked)
                    {
                        OptionSimInputsPortfolio.ChangeInterestRate = (1 + Convert.ToDouble((numericUpDownIntRate.Value / 100)));
                    }
                    if (ckhbxExpiration.Checked)
                    {
                        OptionSimInputsPortfolio.ChangeDaysToExpiration = Convert.ToInt32((numericUpDownDaysToExp.Value));
                    }
                }
                else
                {
                    // volSkewReq only valid if either of underlying Price or daysToExp is shocked..
                    if (chkBoxUseVolSkew.Checked)
                    {
                        inputs.IsVolSkewRequest = _preferences.CheckIFVolSkewReqValid();
                    }
                }

                Dictionary<string, List<PranaPositionWithGreeks>> dictListRequests = GetUniqueSelectedRowsDict();
                List<PranaPositionWithGreeks> listRequests = GetUniqueSelectedRows();

                _allowedAssets = multiSelectDropDown1.GetSelectedItemsInDictionary();

                foreach (PranaPositionWithGreeks position in listRequests)
                {
                    switch (position.AssetID)
                    {
                        case (int)AssetCategory.EquityOption:
                        case (int)AssetCategory.FutureOption:
                            if (position.ExpirationDate.Date >= DateTime.Now.Date)
                            {
                                OptionSimulationInputs optionSimulationInputsForEachRow = new OptionSimulationInputs();
                                optionSimulationInputsForEachRow.ChangeUnderlyingPrice = OptionSimInputsPortfolio.ChangeUnderlyingPrice;
                                optionSimulationInputsForEachRow.ChangeInterestRate = OptionSimInputsPortfolio.ChangeInterestRate;
                                optionSimulationInputsForEachRow.ChangeDaysToExpiration = OptionSimInputsPortfolio.ChangeDaysToExpiration;
                                optionSimulationInputsForEachRow.ChangeVolatility = OptionSimInputsPortfolio.ChangeVolatility;
                                optionSimulationInputsForEachRow.UnderlyingPriceAsAbosluteValue = OptionSimInputsPortfolio.UnderlyingPriceAsAbosluteValue;

                                if (chkbxUnderLyingPrice.Checked)
                                {
                                    if (!checkBoxUseAbsoluteValuesForUnderlyingPrice.Checked && chkBoxUseBetaAdj.Checked)
                                    {
                                        optionSimulationInputsForEachRow.ChangeUnderlyingPrice = (1 + Convert.ToDouble(((numericUpDownUnderLyingPrice.Value * position.Beta) / 100)));
                                    }
                                }
                                OptionSimulationInputs groupSimInputs = null;
                                if (!inputs.DictOptSimInputs.ContainsKey(position.Symbol))
                                {
                                    if (useNonParallelShifts)
                                    {
                                        groupSimInputs = GetGroupSimulationInputsNew(dictListRequests[position.Symbol], position);
                                    }
                                    if (useVolShockAdjFactor)
                                    {
                                        if (!useNonParallelShifts)
                                        {
                                            groupSimInputs = new OptionSimulationInputs();
                                            groupSimInputs.ChangeInterestRate = optionSimulationInputsForEachRow.ChangeInterestRate;
                                            groupSimInputs.ChangeDaysToExpiration = optionSimulationInputsForEachRow.ChangeDaysToExpiration;
                                            groupSimInputs.ChangeVolatility = optionSimulationInputsForEachRow.ChangeVolatility;
                                            groupSimInputs.ChangeUnderlyingPrice = optionSimulationInputsForEachRow.ChangeUnderlyingPrice;
                                            groupSimInputs.UnderlyingPriceAsAbosluteValue = optionSimulationInputsForEachRow.UnderlyingPriceAsAbosluteValue;
                                        }
                                        int ActualDaysToExp = CentralRiskPositionsManager.GetInstance.GetDaysToExpirationForOption(position);
                                        int simulatedDaysToExp = (ActualDaysToExp - groupSimInputs.ChangeDaysToExpiration);
                                        // vol shock Adjusted with vol shock adj factor
                                        double VolShockAdjFactor = _preferences.GetVolShockAdjFactor(simulatedDaysToExp);
                                        double volShockAdjusted = (groupSimInputs.ChangeVolatility - 1) * VolShockAdjFactor;
                                        groupSimInputs.ChangeVolatility = (1 + volShockAdjusted);
                                    }
                                    if (groupSimInputs != null)
                                    {
                                        //Bharat (31 December 2013)
                                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-3083
                                        if (_allowedAssets.ContainsKey(position.AssetID))
                                        {
                                            inputs.DictOptSimInputs.Add(position.Symbol, groupSimInputs);
                                        }
                                        else
                                        {
                                            inputs.DictOptSimInputs.Add(position.Symbol, DefaultOptionSimulationInputs());
                                        }
                                    }
                                    else
                                    {
                                        //Bharat (31 December 2013)
                                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-3083
                                        if (_allowedAssets.ContainsKey(position.AssetID))
                                        {
                                            inputs.DictOptSimInputs.Add(position.Symbol, optionSimulationInputsForEachRow);
                                        }
                                        else
                                        {
                                            inputs.DictOptSimInputs.Add(position.Symbol, DefaultOptionSimulationInputs());
                                        }
                                    }
                                }
                                if (inputs.DictSymbolOptions.ContainsKey(position.UnderlyingSymbol))
                                {
                                    List<string> listOpts = inputs.DictSymbolOptions[position.UnderlyingSymbol];
                                    if (!listOpts.Contains(position.Symbol))
                                    {
                                        listOpts.Add(position.Symbol);
                                        if (!_listRequestedSymbols.Contains(position.Symbol))
                                        {
                                            _listRequestedSymbols.Add(position.Symbol);
                                        }
                                    }
                                    if (inputs.IsVolSkewRequest)
                                    {
                                        List<VolSkewObject> listVolSkewReq = inputs.DictVolSkewReq[position.UnderlyingSymbol];
                                        VolSkewObject reqObject = new VolSkewObject(position);
                                        listVolSkewReq.Add(reqObject);
                                    }
                                }
                                else
                                {
                                    if (!_listRequestedSymbols.Contains(position.Symbol))
                                    {
                                        _listRequestedSymbols.Add(position.Symbol);
                                    }
                                    List<string> listOptsNew = new List<string>();
                                    listOptsNew.Add(position.Symbol);
                                    inputs.DictSymbolOptions.Add(position.UnderlyingSymbol, listOptsNew);
                                    if (inputs.IsVolSkewRequest)
                                    {
                                        List<VolSkewObject> listVolSkewReqNew = new List<VolSkewObject>();
                                        VolSkewObject reqObject = new VolSkewObject(position);
                                        listVolSkewReqNew.Add(reqObject);
                                        inputs.DictVolSkewReq.Add(position.UnderlyingSymbol, listVolSkewReqNew);
                                    }
                                    if (!inputParametersCollection.ListUniqueSymbols.Contains(position.UnderlyingSymbol))
                                    {
                                        inputParametersCollection.ListUniqueSymbols.Add(position.UnderlyingSymbol);
                                    }
                                }
                                if (!inputParametersCollection.ListUniqueSymbols.Contains(position.Symbol))
                                {
                                    inputParametersCollection.ListUniqueSymbols.Add(position.Symbol);
                                }
                            }
                            break;

                        case (int)AssetCategory.FX:
                        case (int)AssetCategory.FXForward:
                            if (!inputs.ListNonOptions.Contains(position.Symbol))
                            {
                                inputs.ListNonOptions.Add(position.Symbol);
                                if (!_listRequestedSymbols.Contains(position.Symbol))
                                {
                                    _listRequestedSymbols.Add(position.Symbol);
                                }
                            }
                            if (!inputParametersCollection.DictFXSymbols.ContainsKey(position.Symbol))
                            {
                                fxInfo fxReqObj = new fxInfo();
                                fxReqObj.PranaSymbol = position.Symbol;
                                fxReqObj.FromCurrencyID = position.LeadCurrencyID;
                                fxReqObj.ToCurrencyID = position.VsCurrencyID;
                                fxReqObj.CategoryCode = (AssetCategory)position.AssetID;
                                inputParametersCollection.DictFXSymbols.Add(position.Symbol, fxReqObj);
                            }
                            break;

                        default: // Live Feed Request should be sent only for Non Option Symbol and Future Underling
                            if (!inputs.ListNonOptions.Contains(position.Symbol))
                            {
                                inputs.ListNonOptions.Add(position.Symbol);
                                if (!_listRequestedSymbols.Contains(position.Symbol))
                                {
                                    _listRequestedSymbols.Add(position.Symbol);
                                }
                            }
                            if (!String.IsNullOrEmpty(position.UnderlyingSymbol) && !inputs.ListNonOptions.Contains(position.UnderlyingSymbol))
                            {
                                inputs.ListNonOptions.Add(position.UnderlyingSymbol);
                            }

                            if (!inputParametersCollection.ListUniqueSymbols.Contains(position.Symbol))
                            {
                                inputParametersCollection.ListUniqueSymbols.Add(position.Symbol);
                            }
                            break;
                    }
                }
                inputParametersCollection.DictSubscriberInputs.Add(_viewName, inputs);
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
            return inputParametersCollection;
        }

        //Bharat (31 December 2013)
        //http://jira.nirvanasolutions.com:8080/browse/PRANA-3083
        private OptionSimulationInputs DefaultOptionSimulationInputs()
        {
            OptionSimulationInputs optionSimulationInputs = new OptionSimulationInputs();
            try
            {
                optionSimulationInputs.ChangeInterestRate = 1;
                optionSimulationInputs.ChangeVolatility = 1;
                optionSimulationInputs.ChangeUnderlyingPrice = 1;
                optionSimulationInputs.ChangeDaysToExpiration = 0;
                optionSimulationInputs.UnderlyingPriceAsAbosluteValue = false;
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
            return optionSimulationInputs;
        }

        private OptionSimulationInputs GetGroupSimulationInputsNew(List<PranaPositionWithGreeks> listPosition, PranaPositionWithGreeks position)
        {
            OptionSimulationInputs groupSimInputs = null;
            try
            {
                bool useDefaultGroupInputs = false;
                string shockBasis = _preferences.GroupShockFilter;

                string positionType = string.Empty;
                double totalQuantity = 0;

                switch ((GroupShockBasis)(Enum.Parse(typeof(GroupShockBasis), shockBasis)))
                {
                    case GroupShockBasis.PositionSide:

                        if (position.Quantity >= 0)
                        {
                            positionType = PositionTag.Long.ToString();
                        }
                        else if (position.Quantity < 0)
                        {
                            positionType = PositionTag.Short.ToString();
                        }
                        break;
                    case GroupShockBasis.PositionSidePortfolio:
                        foreach (PranaPositionWithGreeks pranaPos in listPosition)
                        {
                            totalQuantity += pranaPos.Quantity;
                        }

                        if (totalQuantity >= 0)
                            positionType = PositionTag.Long.ToString();
                        else if (totalQuantity < 0)
                            positionType = PositionTag.Short.ToString();
                        else
                            useDefaultGroupInputs = true;

                        break;
                    case GroupShockBasis.PositionSideExpPortfolio:
                        foreach (PranaPositionWithGreeks pranaPos in listPosition)
                        {
                            if (position.PutOrCall == 0)
                            {
                                totalQuantity += ((-1) * pranaPos.Quantity * Math.Sign(pranaPos.UnderlyingDelta));
                            }
                            else if (position.PutOrCall == 1)
                            {
                                totalQuantity += (pranaPos.Quantity * Math.Sign(pranaPos.UnderlyingDelta));
                            }
                            else
                            {
                                totalQuantity += (pranaPos.Quantity * Math.Sign(pranaPos.Delta));
                            }
                        }
                        if (totalQuantity >= 0)
                            positionType = PositionTag.Long.ToString();
                        else if (totalQuantity < 0)
                            positionType = PositionTag.Short.ToString();
                        else
                            useDefaultGroupInputs = true;
                        break;

                    case GroupShockBasis.UDAAssets:
                        if (!String.IsNullOrEmpty(position.UDAAsset))
                            positionType = position.UDAAsset;
                        else
                            useDefaultGroupInputs = true;
                        break;
                    case GroupShockBasis.UDACountries:
                        if (!String.IsNullOrEmpty(position.CountryName))
                            positionType = position.CountryName;
                        else
                            useDefaultGroupInputs = true;
                        break;
                    case GroupShockBasis.UDASectors:
                        if (!String.IsNullOrEmpty(position.SectorName))
                            positionType = position.SectorName;
                        else
                            useDefaultGroupInputs = true;
                        break;
                    case GroupShockBasis.UDASecurityTypes:
                        if (!String.IsNullOrEmpty(position.SecurityTypeName))
                            positionType = position.SecurityTypeName;
                        else
                            useDefaultGroupInputs = true;
                        break;
                    case GroupShockBasis.UDASubSectors:
                        if (!String.IsNullOrEmpty(position.SubSectorName))
                            positionType = position.SubSectorName;
                        else
                            useDefaultGroupInputs = true;
                        break;
                    default:
                        break;
                }
                groupSimInputs = _preferences.GetGroupSimulationInputs(shockBasis + "-" + positionType, useDefaultGroupInputs);
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
            return groupSimInputs;
        }

        private void UpdateData(ResponseObj optionResponseObj)
        {
            try
            {
                foreach (KeyValuePair<string, OptionGreeks> item in optionResponseObj.CalculatedGreeks)
                {
                    if (_listRequestedSymbols.Contains(item.Key))
                    {
                        _listRequestedSymbols.Remove(item.Key);
                    }
                    List<PranaPositionWithGreeks> list = GetAllCheckedRows(item.Key);
                    foreach (PranaPositionWithGreeks pranaPositionWithGreeks in list)
                    {
                        pranaPositionWithGreeks.UpdatePranaPositionWithGreeks(item.Value, pranaPositionWithGreeks.FXRate);
                        CalculatePnls(pranaPositionWithGreeks);
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

        private void CalculatePnls(PranaPositionWithGreeks pos)
        {
            try
            {
                List<WeightedGreekCalcInputs> list = new List<WeightedGreekCalcInputs>();
                list.Add(new WeightedGreekCalcInputs(pos));

                pos.CostBasisUnrealizedPnL = OptionGreekCalculater.CalculateNetCostBasisPnl(list);
                pos.SimulatedPnl = OptionGreekCalculater.CalculateNetDayPnl(list);
                pos.DeltaAdjExposure = OptionGreekCalculater.CalculateNetDeltaAdjExposure(list);
                pos.DeltaAdjPosition = OptionGreekCalculater.CalculateNetDeltaAdjPosition(list);
                pos.DollarDelta = OptionGreekCalculater.CalculateNetDollarDelta(list);
                pos.DollarGamma = OptionGreekCalculater.CalculateNetDollarGamma(list);
                pos.DollarRho = OptionGreekCalculater.CalculateNetDollarRho(list);
                pos.DollarTheta = OptionGreekCalculater.CalculateNetDollarTheta(list);
                pos.DollarVega = OptionGreekCalculater.CalculateNetDollarVega(list);
                pos.DeltaAdjPositionLME = OptionGreekCalculater.CalculateNetDeltaAdjPositionLME(list);
                pos.GammaAdjPosition = OptionGreekCalculater.CalculateNetGammaAdjPosition(list);
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-4318
                pos.MarketValue = OptionGreekCalculater.CalculateMarketValue(list);
                pos.BetaAdjExposure = pos.DeltaAdjExposure * pos.Beta;
                pos.CostBasisUnrealizedPnLInBaseCurrency = pos.CostBasisUnrealizedPnL * pos.FXRate;
                pos.BetaAdjExposureInBaseCurrency = pos.BetaAdjExposure * pos.FXRate;
                pos.CostBasisUnrealizedPnLInBaseCurrency = pos.CostBasisUnrealizedPnL * pos.FXRate;
                pos.DeltaAdjExposureInBaseCurrency = pos.DeltaAdjExposure * pos.FXRate;
                pos.DollarDeltaInBaseCurrency = pos.DollarDelta * pos.FXRate;
                pos.DollarGammaInBaseCurrency = pos.DollarGamma * pos.FXRate;
                pos.DollarRhoInBaseCurrency = pos.DollarRho * pos.FXRate;
                pos.DollarThetaInBaseCurrency = pos.DollarTheta * pos.FXRate;
                pos.DollarVegaInBaseCurrency = pos.DollarVega * pos.FXRate;
                pos.MarketValueInBaseCurrency = pos.MarketValue * pos.FXRate;
                pos.SimulatedPnlInBaseCurrency = pos.SimulatedPnl * pos.FXRate;
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
        /// </summary>
        /// <param name="gridDict"></param>
        /// <param name="stressTestDetails"></param>
        public void SetGridExportSettings(Dictionary<string, UltraGrid> gridDict, Dictionary<string, string> stressTestDetails)
        {
            try
            {
                UltraGrid grid = new UltraGrid();
                Form UI = new Form();
                UI.Controls.Add(grid);
                //Sets the grid data source to be exported.
                grid.DataSource = _pranaPositionWithGreekColl;
                //Sets allow group by property to true.
                grid.DisplayLayout.Bands[0].Override.AllowGroupBy = DefaultableBoolean.True;
                //Set the grid layout properties.
                grid.DisplayLayout.ViewStyleBand = ViewStyleBand.OutlookGroupBy;
                grid.DisplayLayout.Override.GroupByColumnsHidden = DefaultableBoolean.False;
                grid.DisplayLayout.Bands[0].ColumnFilters.CopyFrom(grdPositions.DisplayLayout.Bands[0].ColumnFilters);
                //Gets the default layout for export.S
                //List<ColumnData> listOfColData = new List<ColumnData>();
                //listOfColData = RiskLayoutManager.RiskLayout.GetStepAnalLayout(_viewName).StepAnalysisColumns;

                //gets the layout set as preferences for 
                //Set the caption for the grid columns.
                foreach (UltraGridColumn col in grdPositions.DisplayLayout.Bands[0].Columns)
                {
                    if (col.Header.Caption != string.Empty)
                    {
                        grid.DisplayLayout.Bands[0].Columns[col.Key].Header.Caption = col.Header.Caption;
                    }
                    grid.DisplayLayout.Bands[0].Columns[col.Key].Hidden = true;
                }

                //Kashish G. (27/10/2015)
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-7593
                // In the Risk Export File we need to show "N/A" under Expiration Date Column
                // and "Non-Expiring Position" under Expiration Month Column
                foreach (UltraGridRow row in grid.Rows)
                {
                    int AssetID = ((PranaPositionWithGreeks)(row.ListObject)).AssetID;
                    if (AssetID == (int)AssetCategory.Equity || AssetID == (int)(AssetCategory.PrivateEquity) || AssetID == (int)(AssetCategory.CreditDefaultSwap))
                    {
                        row.Cells[COL_ExpirationDate].Value = DateTimeConstants.MinValue;
                        ValueList valuelist = new ValueList();
                        valuelist.ValueListItems.Add(new ValueListItem(DateTimeConstants.MinValue, "N/A"));
                        row.Cells[COL_ExpirationDate].ValueList = valuelist;

                        row.Cells[COL_ExpirationMonth].Value = DateTimeConstants.MinValue;
                        valuelist = new ValueList();
                        valuelist.ValueListItems.Add(new ValueListItem(DateTimeConstants.MinValue, "Non-Expiring Position"));
                        row.Cells[COL_ExpirationMonth].ValueList = valuelist;
                        row.Update();
                    }
                    // Set the Put and call Enum value in the export file. 
                    List<EnumerationValue> PutOrCallType = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(OptionType));
                    ValueList PutOrCallValueList = new ValueList();
                    foreach (EnumerationValue value in PutOrCallType)
                    {
                        PutOrCallValueList.ValueListItems.Add(value.Value, value.DisplayText.Substring(0, 1));
                    }
                    PutOrCallValueList.ValueListItems.Add(int.MinValue, " ");
                    row.Cells[COL_PutOrCall].ValueList = PutOrCallValueList;
                    row.Update();
                }

                UltraGridBand band = this.grdPositions.DisplayLayout.Bands[0];
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

                //Provides the detail of stress test being run on the current view.
                string stressTestDetail = "Change in Volatility" + "=" + numericUpDownVol.Value + Seperators.SEPERATOR_6 + "Change in Underlying" + "=" + numericUpDownUnderLyingPrice.Value + Seperators.SEPERATOR_6 + "Days To Expiration" + "=" + numericUpDownDaysToExp.Value;

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

                stressTestDetails.Add(_viewName, stressTestDetail);
                SortedDictionary<int, string> positionMapping = new SortedDictionary<int, string>();
                foreach (UltraGridColumn col in grdPositions.DisplayLayout.Bands[0].Columns)
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
                gridDict.Add(_viewName, grid);
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

        public void RefreshPositions()
        {
            try
            {
                UpdateStatus(false, true);
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
                PranaPositionWithGreekColl positions = await CentralRiskPositionsManager.GetInstance.GetPositionsAsRiskPref();
                GetInstance_PositionReceived(positions);
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

        public void SetUseVolSkewState(bool enable)
        {
            try
            {
                if (!enable)
                {
                    if (chkBoxUseVolSkew.CheckState == CheckState.Unchecked)
                    {
                        chkBoxUseVolSkew.Enabled = enable;
                    }
                }
                else
                {
                    chkBoxUseVolSkew.Enabled = enable;
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

        public void UnwireEvents()
        {
            try
            {
                if (_secMasterClient != null)
                {
                    _secMasterClient.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(_secMasterClient_SecMstrDataResponse);
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

        private bool ValidateConnection()
        {
            try
            {
                if (_pricingAnalysis.ConnectionStatus.Equals(Prana.BusinessObjects.PranaInternalConstants.ConnectionStatus.DISCONNECTED) || _pricingAnalysis.ConnectionStatus.Equals(Prana.BusinessObjects.PranaInternalConstants.ConnectionStatus.NOSERVER))
                {
                    UpdateConnectionStatus(_isLiveFeedConnected);
                    return false;
                }
                else if (!_isLiveFeedConnected)
                {
                    UpdateConnectionStatus(_isLiveFeedConnected);
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

        private void ClearCheckBoxes()
        {
            try
            {
                numericUpDownVol.Value = 0;
                numericUpDownDaysToExp.Value = 0;
                numericUpDownIntRate.Value = 0;
                numericUpDownUnderLyingPrice.Value = 0;

                chkbxVol.Checked = false;
                ckhbxExpiration.Checked = false;
                chkbxInterestRate.Checked = false;
                chkbxUnderLyingPrice.Checked = false;
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

        private void ClearGraphData()
        {
            try
            {
                if (dtGraphSelected != null)
                {
                    dtGraphSelected.Rows.Clear();
                }
                cmbPortfolio.Enabled = false;
                analyticsChart1.Refresh();
                analyticsChart1.SetXYAxisNames(cmbbxXAxis.Text);
                _stepAnalysisCache.ClearCache();
                analyticsChart1.SetData(dtGraphSelected, new List<int>(), 6, 1);
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

        //Bharat Kumar Jangir (11 December 2013)
        //Common Method for step analysis after stress test
        private void ProcessStepAnalysisTestUsingStressedData()
        {
            try
            {
                _responseReceivedCount = 0;
                _listRequestedSymbols.Clear();
                toolStripStatusLabel.Text = string.Empty;

                Dictionary<string, Dictionary<string, StepAnalysisResponse>> stepAnalReqDict = new Dictionary<string, Dictionary<string, StepAnalysisResponse>>();
                List<StepAnalysisResponse> listStepAnalysisInputs = new List<StepAnalysisResponse>();
                List<PranaPositionWithGreeks> listUniqueRows = GetSelectedOptionRows();
                foreach (PranaPositionWithGreeks position in listUniqueRows)
                {
                    if (position.UnderlyingSymbol != string.Empty && position.ExpirationDate.Date >= DateTime.UtcNow.Date)
                    {
                        StepAnalysisResponse stepAnalReqObj = new StepAnalysisResponse(_userID.ToString(), position.Symbol, position.UnderlyingSymbol, _stepParameter);
                        stepAnalReqObj.InputParameters = new InputParametersForGreeks();
                        stepAnalReqObj.InputParameters.SetValues(position);
                        stepAnalReqObj.Greeks = new OptionGreeks();
                        stepAnalReqObj.Greeks.SetValues(position);

                        Dictionary<string, StepAnalysisResponse> tempDict = new Dictionary<string, StepAnalysisResponse>();
                        tempDict.Add(string.Empty, stepAnalReqObj);
                        stepAnalReqDict.Add(position.Symbol, tempDict);
                        listStepAnalysisInputs.Add(stepAnalReqObj);
                        if (!_listRequestedSymbols.Contains(position.Symbol))
                        {
                            _listRequestedSymbols.Add(position.Symbol);
                        }
                    }
                }
                InputParametersCollection inputParametersCollection = new InputParametersCollection();
                SubscriberViewInputs inputs = new SubscriberViewInputs();
                inputs.ListStepAnalysisInputs = listStepAnalysisInputs;
                inputs.StepAnalResDict = stepAnalReqDict;
                inputs.StepAnalysisUsingStressData = true;
                inputs.ID = _viewID;
                inputs.HashCode = this.GetHashCode();
                inputParametersCollection.DictSubscriberInputs.Add(_viewID, inputs);
                inputParametersCollection.UserID = _userID.ToString();

                if (listStepAnalysisInputs.Count > 0)
                {
                    SendRequestForStepAnalysis(inputParametersCollection);
                }
                else
                {
                    List<PranaPositionWithGreeks> listUniqueSelectedRows = GetUniqueSelectedRows();
                    foreach (PranaPositionWithGreeks gridUniqueRow in listUniqueSelectedRows)
                    {
                        int baseAssetID = Mapper.GetBaseAsset(gridUniqueRow.AssetID);
                        if (baseAssetID != (int)AssetCategory.Option && gridUniqueRow.SelectedFeedPrice != 0)
                        {
                            _stepAnalysisCache.AddNonOptionsDataTable(_stepParameter, null, gridUniqueRow.Symbol);
                        }
                    }
                    _stepAnalysisCache.AddPortfolioDataTable(_stepParameter);
                    BindPortfoliosCombo();
                    lstbxYAxis_SelectedIndexChanged(null, null);
                    toolStripStatusLabel.ForeColor = Color.Green;
                    toolStripStatusLabel.Text = DateTime.Now + ": Step Analysis Completed";
                    EnableForm();
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

        private void btnStepAnalysis_Click(object sender, EventArgs e)
        {
            try
            {
                if (MarketDataValidation.CheckMarketDataPermissioning())
                {
                    if (ValidateRequest())
                    {
                        ClearGraphData();
                        _responseReceivedCount = 0;
                        _listRequestedSymbols.Clear();
                        toolStripStatusLabel.Text = string.Empty;
                        _isNeedToPerformStepAnalysisAfterStressTest = false;
                        if (!_lowerGridHandleCreated)
                        {
                            _lowerGridHandleCreated = true;
                            exGrpBoxStepAnalysis.Expanded = true;
                        }
                        //Step Analysis inputs
                        StepAnalParameterCode xCode = (StepAnalParameterCode)Enum.Parse(typeof(StepAnalParameterCode), cmbbxXAxis.Value.ToString());
                        double lowValue = Convert.ToDouble(numericUpDownLowValue.Value);
                        double highValue = Convert.ToDouble(numericUpDownHighValue.Value);
                        int steps = Convert.ToInt16(numericUpDownSteps.Value);
                        _stepParameter = new StepParameter(xCode, steps, lowValue, highValue);

                        if (checkBoxUseStressTestDataInStepAnalysis.Checked && !_isDataAlreadyStressed)
                        {
                            _isNeedToPerformStepAnalysisAfterStressTest = true;
                            btnSimulate_Click(sender, e);
                        }
                        else if (checkBoxUseStressTestDataInStepAnalysis.Checked && _isDataAlreadyStressed)
                        {
                            _isNeedToPerformStepAnalysisAfterStressTest = true;
                            ProcessStepAnalysisTestUsingStressedData();
                        }
                        else
                        {
                            InputParametersCollection inputParametersCollection = new InputParametersCollection();
                            SubscriberViewInputs inputs = new SubscriberViewInputs();
                            inputs.ID = _viewID;
                            inputs.HashCode = this.GetHashCode();
                            inputs.IsStressTestRequest = false;

                            if ((xCode.Equals(StepAnalParameterCode.DaysToExpiration) || xCode.Equals(StepAnalParameterCode.UnderlyingPrice)) && chkBoxUseVolSkew.Checked)
                            {
                                inputs.IsVolSkewRequest = true;
                            }
                            // Send Request for OPTIONS
                            List<PranaPositionWithGreeks> listRequests = GetUniqueSelectedRows();

                            foreach (PranaPositionWithGreeks pranaPos in listRequests)
                            {
                                switch (pranaPos.AssetID)
                                {
                                    case (int)AssetCategory.EquityOption:
                                    case (int)AssetCategory.FutureOption:
                                        if (pranaPos.UnderlyingSymbol != string.Empty && pranaPos.ExpirationDate.Date >= DateTime.UtcNow.Date)
                                        {
                                            StepAnalysisResponse stepAnalReqObj = new StepAnalysisResponse();
                                            InputParametersForGreeks inputParams = pranaPos.GetBasicInputParams();
                                            if (inputs.DictSymbolOptions.ContainsKey(pranaPos.UnderlyingSymbol))
                                            {
                                                List<string> listOpts = inputs.DictSymbolOptions[pranaPos.UnderlyingSymbol];
                                                if (!listOpts.Contains(pranaPos.Symbol))
                                                {
                                                    inputs.DictSymbolOptions[pranaPos.UnderlyingSymbol].Add(pranaPos.Symbol);
                                                }
                                                if (inputs.IsVolSkewRequest)
                                                {
                                                    List<VolSkewObject> listVolSkewReq = inputs.DictVolSkewReq[pranaPos.UnderlyingSymbol];
                                                    VolSkewObject reqObject = new VolSkewObject(pranaPos);
                                                    reqObject.ParameterCode = xCode;
                                                    listVolSkewReq.Add(reqObject);
                                                }
                                            }
                                            else
                                            {
                                                List<string> listOptsNew = new List<string>();
                                                listOptsNew.Add(pranaPos.Symbol);
                                                inputs.DictSymbolOptions.Add(pranaPos.UnderlyingSymbol, listOptsNew);

                                                if (inputs.IsVolSkewRequest)
                                                {
                                                    List<VolSkewObject> listVolSkewReqNew = new List<VolSkewObject>();
                                                    VolSkewObject reqObject = new VolSkewObject(pranaPos);
                                                    reqObject.ParameterCode = xCode;
                                                    listVolSkewReqNew.Add(reqObject);
                                                    inputs.DictVolSkewReq.Add(pranaPos.UnderlyingSymbol, listVolSkewReqNew);
                                                }
                                                if (!inputParametersCollection.ListUniqueSymbols.Contains(pranaPos.UnderlyingSymbol))
                                                {
                                                    inputParametersCollection.ListUniqueSymbols.Add(pranaPos.UnderlyingSymbol);
                                                }
                                            }
                                            if (!inputs.StepAnalResDict.ContainsKey(pranaPos.Symbol))
                                            {
                                                Dictionary<string, StepAnalysisResponse> dictGeneralMatrix = new Dictionary<string, StepAnalysisResponse>();
                                                dictGeneralMatrix.Add(string.Empty, stepAnalReqObj);
                                                inputs.StepAnalResDict.Add(pranaPos.Symbol, dictGeneralMatrix);
                                                if (!_listRequestedSymbols.Contains(pranaPos.Symbol))
                                                {
                                                    _listRequestedSymbols.Add(pranaPos.Symbol);
                                                }
                                            }
                                            else
                                            {
                                                Dictionary<string, StepAnalysisResponse> dictGeneralMatrix = inputs.StepAnalResDict[pranaPos.Symbol];
                                                dictGeneralMatrix.Add(string.Empty, stepAnalReqObj);
                                            }
                                            if (!inputParametersCollection.ListUniqueSymbols.Contains(pranaPos.Symbol))
                                            {
                                                inputParametersCollection.ListUniqueSymbols.Add(pranaPos.Symbol);
                                            }
                                            stepAnalReqObj.InputParameters = inputParams;
                                            stepAnalReqObj.StepParameter_X = _stepParameter;
                                            stepAnalReqObj.Symbol = pranaPos.Symbol;
                                            stepAnalReqObj.UnderlyingSymbol = pranaPos.UnderlyingSymbol;
                                        }
                                        break;

                                    case (int)AssetCategory.FX:
                                    case (int)AssetCategory.FXForward:
                                        if (!inputs.ListNonOptions.Contains(pranaPos.Symbol))
                                        {
                                            inputs.ListNonOptions.Add(pranaPos.Symbol);
                                            if (!_listRequestedSymbols.Contains(pranaPos.Symbol))
                                            {
                                                _listRequestedSymbols.Add(pranaPos.Symbol);
                                            }
                                        }
                                        if (!inputParametersCollection.DictFXSymbols.ContainsKey(pranaPos.Symbol))
                                        {
                                            fxInfo fxReq = new fxInfo();
                                            fxReq.PranaSymbol = pranaPos.Symbol;
                                            fxReq.FromCurrencyID = pranaPos.LeadCurrencyID;
                                            fxReq.ToCurrencyID = pranaPos.VsCurrencyID;
                                            fxReq.CategoryCode = (AssetCategory)pranaPos.AssetID;
                                            inputParametersCollection.DictFXSymbols.Add(pranaPos.Symbol, fxReq);
                                        }
                                        break;

                                    default:
                                        if (!inputs.ListNonOptions.Contains(pranaPos.Symbol))
                                        {
                                            inputs.ListNonOptions.Add(pranaPos.Symbol);
                                            if (!_listRequestedSymbols.Contains(pranaPos.Symbol))
                                            {
                                                _listRequestedSymbols.Add(pranaPos.Symbol);
                                            }
                                        }
                                        if (!inputParametersCollection.ListUniqueSymbols.Contains(pranaPos.Symbol))
                                        {
                                            inputParametersCollection.ListUniqueSymbols.Add(pranaPos.Symbol);
                                        }
                                        break;
                                }
                            }
                            inputParametersCollection.DictSubscriberInputs.Add(_viewName, inputs);
                            inputParametersCollection.UserID = _userID.ToString();
                            if (inputParametersCollection.ListUniqueSymbols.Count > 0 || inputParametersCollection.DictFXSymbols.Count > 0)
                            {
                                SendRequestForStepAnalysis(inputParametersCollection);
                            }
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

        public void AdjustCheckListBoxWidth()
        {
            try
            {
                int LongestItemLength = 0;
                for (int i = 0; i < checkedMultipleItems.Items.Count; i++)
                {
                    Graphics g = checkedMultipleItems.CreateGraphics();
                    //get width of checklist value item
                    int tempLength = Convert.ToInt32((g.MeasureString(checkedMultipleItems.Items[i].ToString(), this.checkedMultipleItems.Font)).Width);
                    if (tempLength > LongestItemLength)
                    {
                        LongestItemLength = tempLength;
                    }
                }
                if (LongestItemLength > 0)
                {
                    checkedMultipleItems.Width = LongestItemLength;
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

        private void btnSimulate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MarketDataValidation.CheckMarketDataPermissioning())
                {
                    if (ValidateRequest())
                    {
                        if (_isNeedToPerformStepAnalysisAfterStressTest && !_lowerGridHandleCreated)
                        {
                            _lowerGridHandleCreated = true;
                            exGrpBoxStepAnalysis.Expanded = true;
                        }
                        InputParametersCollection inputParametersCollection = new InputParametersCollection();
                        inputParametersCollection = GetInputParametersForSimulation(inputParametersCollection);
                        if (inputParametersCollection.ListUniqueSymbols.Count > 0 || inputParametersCollection.DictFXSymbols.Count > 0)
                        {
                            UpdateStatus(true, false);
                            SendRequestForGreekCalculation(inputParametersCollection);
                            if (GreeksRequested != null)
                            {
                                GreeksRequested(this, new EventArgs<string>(_viewName));
                            }
                        }
                        _isDataAlreadyStressed = true;
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

        public void UpdateStatus(bool isSimulating, bool isRefreshing)
        {
            try
            {
                if (isSimulating)
                {
                    btnSimulate.BackColor = Color.Red;
                    btnSimulate.Text = "Simulating...";
                    toolStripStatusLabel.ForeColor = Color.Red;
                    toolStripStatusLabel.Text = DateTime.Now + ": Simulating...";
                }
                if (isRefreshing)
                {
                    toolStripStatusLabel.ForeColor = Color.Red;
                    toolStripStatusLabel.Text = DateTime.Now + ": Refreshing Positions...";
                }
                DisableForm();
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

        private void btnRevert_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateRequest())
                {
                    ClearCheckBoxes();
                    btnSimulate_Click(sender, e);
                    btnRevert.BackColor = Color.Red;
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

        private void btnClearGraph_Click(object sender, EventArgs e)
        {
            try
            {
                ClearGraphData();
                toolStripStatusLabel.ForeColor = Color.Green;
                toolStripStatusLabel.Text = DateTime.Now + ": Graph Data Cleared";
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

        private void mnuClearFilters_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ColumnFilter colFilters in grdPositions.DisplayLayout.Bands[0].ColumnFilters)
                {
                    colFilters.ClearFilterConditions();
                }
                toolStripStatusLabel.ForeColor = Color.Green;
                toolStripStatusLabel.Text = DateTime.Now + ": Filters Cleared";
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

        private void lstbxYAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            int offset = 6;
            try
            {
                if (dtGraphSelected == null)
                {
                    return;
                }
                if (dtGraphSelected.Rows.Count > 0)
                {
                    StepAnalParameterCode xParameterCode = (StepAnalParameterCode)Enum.Parse(typeof(StepAnalParameterCode), cmbbxXAxis.Value.ToString());
                    CheckedListBox.CheckedItemCollection checkedItems = lstbxYAxis.CheckedItems;

                    List<int> selectedIndexs = new List<int>();
                    foreach (object obj in checkedItems)
                    {
                        string checkedItem = _dictListBoxText[obj.ToString()];
                        int i = Convert.ToInt32(Enum.Parse(typeof(CalculatedParamters), checkedItem));
                        selectedIndexs.Add(i + offset);
                    }
                    analyticsChart1.SetData(dtGraphSelected, selectedIndexs, offset, (int)xParameterCode + 1);
                }
                grdData.Refresh();
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

        void pranaSymbolCtrl_SymbolEntered(object sender, EventArgs<string> e)
        {
            try
            {
                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                reqObj.AddData(e.Value, ApplicationConstants.SymbologyCodes.TickerSymbol);
                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                reqObj.HashCode = this.GetHashCode();
                _secMasterClient.SendRequest(reqObj);
                btnAddSymbol.Enabled = false;
                btnAddSymbol.BackColor = Color.Red;
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

        public void AddSymbol(PranaPositionWithGreeks pranaPos)
        {
            try
            {
                if (pranaPos != null)
                {
                    PranaPositionWithGreeks pranaPosition = (PranaPositionWithGreeks)pranaPos.Clone();
                    _pranaPositionWithGreekColl.Add(pranaPosition);
                    AddPositionToDict(pranaPosition);
                    int posBaseAssetID = Mapper.GetBaseAsset(pranaPosition.AssetID);
                    if (posBaseAssetID == (int)AssetCategory.Future)
                    {
                        AddPositionToUnderlyingFutureDict(pranaPosition);
                    }
                    grdPositions.DataBind();
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

        private void btnAddSymbol_Click(object sender, EventArgs e)
        {
            try
            {
                if (_secMasterObjEnteredSymbol != null)
                {
                    errorProvider.SetError(txtbxQuantity, "");
                    errorProvider.SetError(txtbxPrice, "");

                    Dictionary<int, List<int>> masterFundSubAccountAssociation = new Dictionary<int, List<int>>(CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation());
                    if (!Prana.Global.Utilities.RegularExpressionValidation.IsNumber(txtbxQuantity.Text))
                    {
                        errorProvider.SetError(txtbxQuantity, "Please input Correct value of Quantity");
                        return;
                    }
                    if (!Prana.Global.Utilities.RegularExpressionValidation.IsNumber(txtbxPrice.Text))
                    {
                        errorProvider.SetError(txtbxPrice, "Please input Correct value of Price");
                        return;
                    }
                    List<PranaPositionWithGreeks> listPranaPos = new List<PranaPositionWithGreeks>();
                    if (checkedMultipleItems.CheckedItems.Count > 0)
                    {
                        for (int i = 0; i < checkedMultipleItems.CheckedItems.Count; i++)
                        {
                            if (checkedMultipleItems.CheckedItems[i].ToString() != "All")
                            {
                                PranaPositionWithGreeks pranaPos = new PranaPositionWithGreeks();
                                foreach (KeyValuePair<int, List<int>> kvp in masterFundSubAccountAssociation)
                                {
                                    foreach (int accountid in kvp.Value)
                                    {
                                        if (accountid == ((KeyValuePair<int, string>)checkedMultipleItems.CheckedItems[i]).Key)
                                        {
                                            pranaPos.Level1ID = accountid;
                                            pranaPos.MasterFund = CachedDataManager.GetInstance.GetMasterFund(kvp.Key);
                                            break;
                                        }
                                    }
                                }
                                pranaPos.Level1Name = ((KeyValuePair<int, string>)checkedMultipleItems.CheckedItems[i]).Value;
                                FillOtherDetails(pranaPos);
                                FillSecmasterDetails(pranaPos);
                                listPranaPos.Add(pranaPos);
                            }
                        }
                        CalculatePositionSidePortfolio(listPranaPos);
                    }
                    else
                    {
                        PranaPositionWithGreeks pranaPos = new PranaPositionWithGreeks();
                        FillOtherDetails(pranaPos);
                        FillSecmasterDetails(pranaPos);
                        listPranaPos.Add(pranaPos);
                        CalculatePositionSidePortfolio(listPranaPos);
                    }

                    bool ForAllViews = false;
                    if (chkAddSymbolAcrossAllViews.CheckState == CheckState.Checked)
                    {
                        ForAllViews = true;
                    }
                    if (AddSymbolAcrossAllViews != null)
                    {
                        if (_secMasterObjEnteredSymbol != null)
                        {
                            AddSymbolAcrossAllViews(this, new EventArgs<bool, List<PranaPositionWithGreeks>>(ForAllViews, listPranaPos));
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

        private void FillSecmasterDetails(PranaPositionWithGreeks pranaPos)
        {
            try
            {
                pranaPos.Symbol = pranaSymbolCtrl.Text;
                pranaPos.UnderlyingSymbol = _secMasterObjEnteredSymbol.UnderLyingSymbol;
                pranaPos.CompanyName = _secMasterObjEnteredSymbol.LongName;
                pranaPos.ContractMultiplier = _secMasterObjEnteredSymbol.Multiplier;
                pranaPos.BloombergSymbol = _secMasterObjEnteredSymbol.BloombergSymbol;
                pranaPos.BloombergSymbolWithExchangeCode = _secMasterObjEnteredSymbol.BloombergSymbolWithExchangeCode;
                pranaPos.FactSetSymbol = _secMasterObjEnteredSymbol.FactSetSymbol;
                pranaPos.ActivSymbol = _secMasterObjEnteredSymbol.ActivSymbol;
                pranaPos.CurrencyID = _secMasterObjEnteredSymbol.CurrencyID;
                pranaPos.CurrencyName = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(_secMasterObjEnteredSymbol.CurrencyID);
                pranaPos.ProxySymbol = _secMasterObjEnteredSymbol.ProxySymbol;

                double qty = 0;
                double.TryParse(txtbxQuantity.Text, out qty);
                pranaPos.Quantity = qty;
                pranaPos.TaxLotQty = Math.Abs(pranaPos.Quantity);
                pranaPos.SideMultiplier = (pranaPos.Quantity > 0) ? 1 : -1;
                float avPrice = 0;
                float.TryParse(txtbxPrice.Text, out avPrice);
                pranaPos.AvgPrice = avPrice;
                pranaPos.AssetID = _secMasterObjEnteredSymbol.AssetID;
                pranaPos.AssetName = CommonDataCache.CachedDataManager.GetInstance.GetAssetText(pranaPos.AssetID);
                pranaPos.AssetCategoryValue = (AssetCategory)pranaPos.AssetID;
                pranaPos.ExchangeID = _secMasterObjEnteredSymbol.ExchangeID;
                pranaPos.ExchangeName = CommonDataCache.CachedDataManager.GetInstance.GetExchangeText(pranaPos.ExchangeID);
                pranaPos.PositionType = (pranaPos.SideMultiplier == 1) ? "Long" : "Short";
                // if not option add delta as 1
                int baseAssetID = Mapper.GetBaseAsset(_secMasterObjEnteredSymbol.AssetID);
                if (baseAssetID == (int)(AssetCategory.Option))
                {
                    SecMasterOptObj secMasterOptionObject = _secMasterObjEnteredSymbol as SecMasterOptObj;
                    pranaPos.ExpirationDate = secMasterOptionObject.ExpirationDate;
                    pranaPos.ExpirationMonth = new DateTime(secMasterOptionObject.ExpirationDate.Year, secMasterOptionObject.ExpirationDate.Month, 01);
                    pranaPos.StrikePrice = secMasterOptionObject.StrikePrice;
                    pranaPos.PutOrCall = secMasterOptionObject.PutOrCall;
                    pranaPos.DaysToExpiration = CentralRiskPositionsManager.GetInstance.GetDaysToExpirationForOption(pranaPos);
                    if (_secMasterObjUnderlyingSymbol != null && _secMasterObjUnderlyingSymbol.TickerSymbol == _secMasterObjEnteredSymbol.UnderLyingSymbol)
                    {
                        pranaPos.UnderlyingDelta = _secMasterObjUnderlyingSymbol.Delta;
                    }
                }
                else if (baseAssetID == (int)(AssetCategory.Future))
                {
                    SecMasterFutObj secMasterFutureObject = _secMasterObjEnteredSymbol as SecMasterFutObj;
                    pranaPos.ExpirationDate = secMasterFutureObject.ExpirationDate;
                    pranaPos.ExpirationMonth = new DateTime(secMasterFutureObject.ExpirationDate.Year, secMasterFutureObject.ExpirationDate.Month, 01);
                }

                if (baseAssetID != (int)AssetCategory.Option)
                {
                    pranaPos.Delta = _secMasterObjEnteredSymbol.Delta;
                }
                //UDA Details
                pranaPos.UDAAsset = _secMasterObjEnteredSymbol.SymbolUDAData.UDAAsset;
                pranaPos.SectorName = _secMasterObjEnteredSymbol.SymbolUDAData.UDASector;
                pranaPos.SubSectorName = _secMasterObjEnteredSymbol.SymbolUDAData.UDASubSector;
                pranaPos.CountryName = _secMasterObjEnteredSymbol.SymbolUDAData.UDACountry;
                pranaPos.SecurityTypeName = _secMasterObjEnteredSymbol.SymbolUDAData.UDASecurityType;
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

        private void FillOtherDetails(PranaPositionWithGreeks pranaPos)
        {
            try
            {
                pranaPos.CompanyUserID = _userID;
                pranaPos.CompanyUserName = CommonDataCache.CachedDataManager.GetInstance.GetUserText(pranaPos.CompanyUserID);
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

        private void CalculatePositionSidePortfolio(List<PranaPositionWithGreeks> listPranaPos)
        {
            try
            {
                double netQtyPortfolioMV = 0;
                double netQtyPortofolioExp = 0;

                //fetch existing positions if any to calculate the net qty at portfolio level...
                List<PranaPositionWithGreeks> listAllPositions = GetAllRows(listPranaPos[0].Symbol);
                List<PranaPositionWithGreeks> listPranaPositionNew = new List<PranaPositionWithGreeks>();
                listPranaPositionNew.AddRange(listPranaPos);
                listPranaPositionNew.AddRange(listAllPositions);

                foreach (PranaPositionWithGreeks position in listPranaPositionNew)
                {
                    //calculated based on portfolio net market value..
                    netQtyPortfolioMV += Convert.ToInt32(position.Quantity);
                    if (position.PutOrCall == 0)
                    {
                        netQtyPortofolioExp += Convert.ToInt32(position.Quantity) * Math.Sign(position.UnderlyingDelta) * (-1);
                    }
                    else if (position.PutOrCall == 1)
                    {
                        netQtyPortofolioExp += Convert.ToInt32(position.Quantity) * Math.Sign(position.UnderlyingDelta);
                    }
                    else
                    {
                        netQtyPortofolioExp += Convert.ToInt32(position.Quantity) * Math.Sign(position.Delta);
                    }
                }
                foreach (PranaPositionWithGreeks pranaPos in listPranaPositionNew)
                {
                    if (netQtyPortfolioMV >= 0)
                    {
                        pranaPos.PositionSideMVInPortfolio = PositionTag.Long.ToString();
                    }
                    else
                    {
                        pranaPos.PositionSideMVInPortfolio = PositionTag.Short.ToString();
                    }
                    if (netQtyPortofolioExp >= 0)
                    {
                        pranaPos.PositionSideExposureInPortfolio = PositionTag.Long.ToString();
                    }
                    else
                    {
                        pranaPos.PositionSideExposureInPortfolio = PositionTag.Short.ToString();
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

        private void StressTestCompleted()
        {
            try
            {
                if (_isStressTestRequest)
                {
                    toolStripStatusLabel.ForeColor = Color.Green;
                    toolStripStatusLabel.Text = DateTime.Now + ": Stress Test Completed";
                    if (GreeksCalculated != null)
                    {
                        GreeksCalculated(this, new EventArgs<string>(_viewName));
                    }
                    //Bharat Kumar Jangir (11 December 2013)
                    //Step Analysis using stress test data
                    if (_isNeedToPerformStepAnalysisAfterStressTest)
                    {
                        ProcessStepAnalysisTestUsingStressedData();
                    }
                    else
                    {
                        EnableForm();
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

        private void StepAnalysisCompleted()
        {
            try
            {
                if (_isStepAnalysisRequest)
                {
                    //Bharat Kumar Jangir (11 December 2013)
                    //Add non options if step test analysis performed using stress test data
                    if (_isNeedToPerformStepAnalysisAfterStressTest)
                    {
                        List<PranaPositionWithGreeks> listUniqueRows = GetUniqueSelectedRows();
                        foreach (PranaPositionWithGreeks gridUniqueRow in listUniqueRows)
                        {
                            int baseAssetID = Mapper.GetBaseAsset(gridUniqueRow.AssetID);
                            if (baseAssetID != (int)AssetCategory.Option)
                            {
                                _stepAnalysisCache.AddNonOptionsDataTable(_stepParameter, null, gridUniqueRow.Symbol);
                            }
                        }
                    }
                    _isNeedToPerformStepAnalysisAfterStressTest = false;
                    _stepAnalysisCache.AddPortfolioDataTable(_stepParameter);
                    BindPortfoliosCombo();
                    lstbxYAxis_SelectedIndexChanged(null, null);
                    toolStripStatusLabel.ForeColor = Color.Green;
                    toolStripStatusLabel.Text = DateTime.Now + ": Step Analysis Completed";
                    EnableForm();
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

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                grdPositions.DataBind();
                toolStripStatusLabel.ForeColor = Color.Green;
                StringBuilder message = new StringBuilder();
                if (_isStressTestRequest)
                {
                    if (_listRequestedSymbols.Count > 0)
                    {
                        message.Clear();
                        if (_listRequestedSymbols.Count > 5)
                        {
                            message.Append(DateTime.Now + ": Stress Test Completed but " + _listRequestedSymbols.Count + " number of symbol(s) were not stressed");
                        }
                        else
                        {
                            message.Append(DateTime.Now + ": Stress Test Completed but " + _listRequestedSymbols.Count + " number of symbol(s) were not stressed");
                            foreach (string sb in _listRequestedSymbols)
                            {
                                message.Append(" " + sb);
                            }
                        }

                        toolStripStatusLabel.Text = message.ToString();
                    }
                    else
                    {
                        toolStripStatusLabel.Text = DateTime.Now + ": Stress Test Completed";
                    }
                }
                if (_isStepAnalysisRequest)
                {
                    if (_listRequestedSymbols.Count > 0)
                    {
                        message.Clear();
                        if (_listRequestedSymbols.Count > 5)
                        {
                            message.Append(DateTime.Now + ":  Step Analysis Completed but " + _listRequestedSymbols.Count + " number of symbol(s) were not stressed");
                        }
                        else
                        {
                            message.Append(DateTime.Now + ":  Step Analysis Completed but " + _listRequestedSymbols.Count + " number of symbol(s) were not stressed");
                            foreach (string sb in _listRequestedSymbols)
                            {
                                message.Append(" " + sb);
                            }
                        }
                        toolStripStatusLabel.Text = message.ToString();
                    }
                    else
                    {
                        toolStripStatusLabel.Text = DateTime.Now + ":  Step Analysis Completed";
                    }
                }
                EnableForm();
                if (GreeksCalculated != null)
                {
                    GreeksCalculated(this, new EventArgs<string>(_viewName));
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

        private void numericUpDownVol_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                chkbxVol.Checked = true;
                _isDataAlreadyStressed = false;
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

        private void numericUpDownUnderLyingPrice_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                chkbxUnderLyingPrice.Checked = true;
                _isDataAlreadyStressed = false;
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

        private void numericUpDownIntRate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                chkbxInterestRate.Checked = true;
                _isDataAlreadyStressed = false;
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

        private void numericUpDownDaysToExp_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ckhbxExpiration.Checked = true;
                _isDataAlreadyStressed = false;
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

        private void cmbbxXAxis_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGraphData();
                if (cmbbxXAxis.Value.ToString() == COL_DaysToExpiration)
                {
                    lblRange.Text = "Range: T +";
                    lblTo.Text = "To T +";
                    numericUpDownLowValue.MinValue = -100;
                    numericUpDownLowValue.MaxValue = 500;
                    numericUpDownHighValue.MinValue = 0;
                    numericUpDownHighValue.MaxValue = 500;
                }
                else
                {
                    lblRange.Text = "% Range: From";
                    lblTo.Text = "To";
                    numericUpDownLowValue.MinValue = -100;
                    numericUpDownLowValue.MaxValue = 100;
                    numericUpDownHighValue.MinValue = 0;
                    numericUpDownHighValue.MaxValue = 100;
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

        private void cmbPortfolio_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbPortfolio.Value != null)
                {
                    string symbolName = cmbPortfolio.Value.ToString();
                    cmbPorfolioLastValue = symbolName;
                    dtGraphSelected = _stepAnalysisCache.GetDataTable(symbolName);
                    grdData.DataSource = null;
                    grdData.DataSource = dtGraphSelected;
                    grdData.DataBind();
                    grdData.DisplayLayout.Bands[0].Columns[cmbbxXAxis.Value.ToString()].Header.VisiblePosition = 0;
                    grdData.DisplayLayout.Bands[0].Columns[cmbbxXAxis.Value.ToString()].Header.Caption = "X";
                    grdData.DisplayLayout.Bands[0].Columns[cmbbxXAxis.Value + "MUL"].Header.VisiblePosition = 1;
                    grdData.DisplayLayout.Bands[0].Columns[cmbbxXAxis.Value + "MUL"].Header.Caption = cmbbxXAxis.Text;
                    grdData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                    SetColumnFormattingGraph(grdData.DisplayLayout.Bands[0].Columns);
                    // change the graph
                    lstbxYAxis_SelectedIndexChanged(null, null);
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

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            try
            {
                if (isAlreadyStarted && exGrpBoxStepAnalysis.Expanded && this.Height != 0)
                {
                    RiskLayoutManager.RiskLayout.GetStepAnalLayout(_viewName).SplitterPosition = (exGrpBoxStepAnalysis.Height * 100) / this.Height;
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

        private void CtrlStepAnalysis_Resize(object sender, EventArgs e)
        {
            try
            {
                if (isAlreadyStarted && exGrpBoxStepAnalysis.Expanded)
                {
                    if (RiskLayoutManager.RiskLayout.GetStepAnalLayout(_viewName).SplitterPosition > 30)
                    {
                        splitter1.SplitPosition = (this.Height * RiskLayoutManager.RiskLayout.GetStepAnalLayout(_viewName).SplitterPosition) / 100;
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

        int ColumnsGroupingLevelOnPranaAnalysisUI = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("ColumnsGroupingLevelOnPranaAnalysisUI"));
        int _count = 0;
        List<string> GroupByColumnsCollection = new List<string>();
        private void grdPositions_BeforeSortChange(object sender, BeforeSortChangeEventArgs e)
        {
            try
            {
                if (!isAlreadyStarted || _isReloadingLayout)
                {
                    return;
                }
                grdPositions.DisplayLayout.Bands[0].Columns[COL_IsChecked].Header.VisiblePosition = 0;
                _count = 0;
                foreach (UltraGridColumn var in e.SortedColumns)
                {
                    if (var.IsGroupByColumn)
                    {
                        _count++;
                    }
                }
                //Bharat Kumar Jangir (22 August 2013)
                //Increase grouping level on Risk Analysis UI
                if (_count > ColumnsGroupingLevelOnPranaAnalysisUI)
                {
                    MessageBox.Show("Positions can not be grouped by more than " + ColumnsGroupingLevelOnPranaAnalysisUI + " columns.", "Option Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Cancel = true;
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

        private void grdPositions_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                if (!isAlreadyStarted || _isReloadingLayout)
                {
                    return;
                }

                int sortCount = grdPositions.DisplayLayout.Bands[0].SortedColumns.Count;
                if (sortCount > 0)
                {
                    //Correction made as it was not returning the column that has been sorted.
                    //Now the sorted column will be detected by mouse click event on column header.
                    UltraGridColumn sortColumn;
                    if (grdPositions.DisplayLayout.Bands[0].SortedColumns.Contains(_columnSorted))
                    {
                        sortColumn = grdPositions.DisplayLayout.Bands[0].SortedColumns[_columnSorted.Key];
                    }
                    else
                    {
                        foreach (UltraGridColumn var in grdPositions.DisplayLayout.Bands[0].SortedColumns)
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
                        foreach (UltraGridColumn var in grdPositions.DisplayLayout.Bands[0].SortedColumns)
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
                        foreach (UltraGridColumn var in grdPositions.DisplayLayout.Bands[0].SortedColumns)
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
                        foreach (UltraGridColumn var in grdPositions.DisplayLayout.Bands[0].SortedColumns)
                        {
                            var.GroupByComparer = null;
                        }
                    }
                    this.grdPositions.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
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

        void grdPositions_BeforeRowFilterDropDown(object sender, Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs e)
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

        void grdPositions_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if ((e.Column.Key.Equals(COL_AUECLocalDate) || e.Column.Key.Equals(COL_ExpirationDate)) && e.NewColumnFilter.FilterConditions != null && e.NewColumnFilter.FilterConditions.Count == 1 && e.NewColumnFilter.FilterConditions[0].CompareValue.Equals("(Today)"))
                {
                    grdPositions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Clear();
                    grdPositions.DisplayLayout.Bands[0].ColumnFilters[e.Column.Key].FilterConditions.Add(FilterComparisionOperator.StartsWith, DateTime.Now.Date.ToString(DateTimeConstants.DateformatForClosing));
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

        UltraGridColumn _columnSorted = null;
        private void grdPositions_MouseClick(object sender, MouseEventArgs e)
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

        void grdPositions_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    // Get a reference to the UIElement at the current mouse position
                    UIElement thisElem = this.grdPositions.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));

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
                        for (int counter = 0; counter < grdPositions.DisplayLayout.Bands[0].SortedColumns.Count; counter++)
                        {
                            if (columnSortedName.Equals(grdPositions.DisplayLayout.Bands[0].SortedColumns[counter].Header.Caption))
                            {
                                _columnSorted = grdPositions.DisplayLayout.Bands[0].SortedColumns[grdPositions.DisplayLayout.Bands[0].SortedColumns[counter].Key];
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

        private void grdPositions_InitializeGroupByRow(object sender, InitializeGroupByRowEventArgs e)
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
                    // Bottom Level Group Row
                    else
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

        private void grdPositions_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdPositions);
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

        private void _secMasterClient_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                if (secMasterObj.AssetID == (int)AssetCategory.FX || secMasterObj.AssetID == (int)AssetCategory.FXForward)
                {
                    if (FXandFXFWDSymbolGenerator.IsValidFxAndFwdSymbol(secMasterObj))
                    {
                        if (ValidateAndMarshal(sender, e)) return;
                    }
                }
                else
                {
                    if (ValidateAndMarshal(sender, e)) return;
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

        private bool ValidateAndMarshal(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                UIThreadMarshellersSecMaster mi = new UIThreadMarshellersSecMaster(_secMasterClient_SecMstrDataResponse);
                if (UIValidation.GetInstance().validate(grdData))
                {
                    if (grdData.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { sender, e });
                    }
                    else
                    {
                        if (pranaSymbolCtrl.Text == secMasterObj.TickerSymbol)
                        {
                            _secMasterObjEnteredSymbol = secMasterObj;
                            if (secMasterObj.AssetCategory == AssetCategory.EquityOption)
                            {
                                //request for underlying symbol
                                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                                reqObj.AddData(secMasterObj.UnderLyingSymbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                                reqObj.HashCode = this.GetHashCode();
                                _secMasterClient.SendRequest(reqObj);
                                return true;
                            }
                            btnAddSymbol.Enabled = true;
                            btnAddSymbol.BackColor = Color.FromArgb(55, 67, 85);
                        }
                        else if (secMasterObj.AssetCategory == AssetCategory.Equity)
                        {
                            if (_secMasterObjEnteredSymbol != null && _secMasterObjEnteredSymbol.AssetCategory == AssetCategory.EquityOption && _secMasterObjEnteredSymbol.UnderLyingSymbol == secMasterObj.TickerSymbol)
                            {
                                _secMasterObjUnderlyingSymbol = secMasterObj;
                                btnAddSymbol.Enabled = true;
                                btnAddSymbol.BackColor = Color.FromArgb(55, 67, 85);
                            }
                        }
                    }
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
            return false;
        }

        private void RowSummarySettings()
        {
            try
            {
                bool groupedBySomeColumn = false;
                foreach (UltraGridColumn col in grdPositions.DisplayLayout.Bands[0].SortedColumns)
                {
                    if (col.IsGroupByColumn)
                    {
                        groupedBySomeColumn = true;
                        break;
                    }
                }
                if (!groupedBySomeColumn)
                {
                    this.grdPositions.DisplayLayout.Override.SummaryDisplayArea = SummaryDisplayAreas.Bottom;
                }
                else
                {
                    //Changes made to show Summary at Grouping level on PM for both Custom Level and Account Level.
                    //http://jira.nirvanasolutions.com:8080/browse/QUAD-43
                    this.grdPositions.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.Bottom;
                    this.grdPositions.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.InGroupByRows;
                    this.grdPositions.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.RootRowsFootersOnly;
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

        private object _lockerStressTest = new object();
        private object _lockerStepAnalysis = new object();

        private void ResetTimer()
        {
            try
            {
                timerRefresh.Stop();
                timerRefresh.Start();
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

        private void UpdateNonOptionsWithSnapShotResponse(SymbolData l1Data)
        {
            try
            {
                string symbol = l1Data.Symbol;
                if (_listRequestedSymbols.Contains(symbol))
                {
                    _listRequestedSymbols.Remove(symbol);
                }
                // Set Stock Price for Non Option and Market Price for All
                if (_isStressTestRequest)
                {
                    List<PranaPositionWithGreeks> list = GetAllCheckedRows(symbol);

                    Dictionary<string, List<PranaPositionWithGreeks>> dictPositionList = GetUniqueSelectedRowsDict();
                    OptionSimulationInputs inputs = null;
                    double changeUnderlyingPrice = 1;

                    foreach (PranaPositionWithGreeks pranaPositionWithGreeks in list)
                    {
                        if (_preferences.UseNonParallelShifts)
                        {
                            if (inputs == null)
                            {
                                inputs = GetGroupSimulationInputsNew(dictPositionList[pranaPositionWithGreeks.Symbol], pranaPositionWithGreeks);
                                changeUnderlyingPrice = inputs.ChangeUnderlyingPrice;
                            }
                        }
                        else
                        {
                            if (chkbxUnderLyingPrice.Checked)
                            {
                                //Bharat Kumar Jangir (11 December 2013)
                                //Check for Underlying Price used as absolute value or percentage value
                                if (checkBoxUseAbsoluteValuesForUnderlyingPrice.Checked)
                                {
                                    changeUnderlyingPrice = Convert.ToDouble(numericUpDownUnderLyingPrice.Value);
                                }
                                else
                                {
                                    if (chkBoxUseBetaAdj.Checked)
                                    {
                                        if (numericUpDownUnderLyingPrice.Value == -100)
                                        {
                                            changeUnderlyingPrice = .000099999999999988987d * pranaPositionWithGreeks.Beta;
                                        }
                                        else
                                        {
                                            changeUnderlyingPrice = (1 + Convert.ToDouble(((numericUpDownUnderLyingPrice.Value * pranaPositionWithGreeks.Beta) / 100)));
                                        }
                                    }
                                    else
                                    {
                                        if (numericUpDownUnderLyingPrice.Value == -100)
                                        {
                                            changeUnderlyingPrice = .000099999999999988987d;
                                        }
                                        else
                                        {
                                            changeUnderlyingPrice = (1 + Convert.ToDouble((numericUpDownUnderLyingPrice.Value / 100)));
                                        }
                                    }
                                }
                            }
                        }
                        double marketPrice = l1Data.SelectedFeedPrice;
                        if (marketPrice < 0.00001)
                        {
                            marketPrice = 0;
                        }

                        pranaPositionWithGreeks.SelectedFeedPrice = marketPrice;
                        pranaPositionWithGreeks.SelectedFeedPriceInBaseCurrency = pranaPositionWithGreeks.SelectedFeedPrice * pranaPositionWithGreeks.FXRate;
                        if (_preferences.UseNonParallelShifts && _preferences.UseAbsoluteValuesForUnderlyingPrice)
                        {
                            //Bharat (31 December 2013)
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-3083
                            if (!_allowedAssets.ContainsKey(pranaPositionWithGreeks.AssetID))
                            {
                                changeUnderlyingPrice = 0;
                            }
                            pranaPositionWithGreeks.SimulatedUnderlyingStockPrice = changeUnderlyingPrice + marketPrice;
                            pranaPositionWithGreeks.SimulatedPrice = changeUnderlyingPrice + marketPrice;
                            pranaPositionWithGreeks.SimulatedPriceInBaseCurrency = pranaPositionWithGreeks.SimulatedPrice * pranaPositionWithGreeks.FXRate;
                            pranaPositionWithGreeks.SimulatedUnderlyingStockPriceInBaseCurrency = pranaPositionWithGreeks.SimulatedUnderlyingStockPrice * pranaPositionWithGreeks.FXRate;

                            //Bharat Kumar Jangir (11 December 2013)
                            //if stock price or option price is less than 0 then we use it 0
                            if (pranaPositionWithGreeks.SimulatedUnderlyingStockPrice < 0)
                            {
                                pranaPositionWithGreeks.SimulatedUnderlyingStockPrice = 0;
                                pranaPositionWithGreeks.SimulatedUnderlyingStockPriceInBaseCurrency = 0;
                            }
                            if (pranaPositionWithGreeks.SimulatedPrice < 0)
                            {
                                pranaPositionWithGreeks.SimulatedPrice = 0;
                                pranaPositionWithGreeks.SimulatedPriceInBaseCurrency = 0;
                            }
                        }
                        else
                        {
                            if (chkbxUnderLyingPrice.Checked && checkBoxUseAbsoluteValuesForUnderlyingPrice.Checked)
                            {
                                //Bharat (31 December 2013)
                                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3083
                                if (!_allowedAssets.ContainsKey(pranaPositionWithGreeks.AssetID))
                                {
                                    changeUnderlyingPrice = 0;
                                }
                                pranaPositionWithGreeks.SimulatedUnderlyingStockPrice = changeUnderlyingPrice + marketPrice;
                                pranaPositionWithGreeks.SimulatedPrice = changeUnderlyingPrice + marketPrice;
                                pranaPositionWithGreeks.SimulatedPriceInBaseCurrency = pranaPositionWithGreeks.SimulatedPrice * pranaPositionWithGreeks.FXRate;
                                pranaPositionWithGreeks.SimulatedUnderlyingStockPriceInBaseCurrency = pranaPositionWithGreeks.SimulatedUnderlyingStockPrice * pranaPositionWithGreeks.FXRate;
                                //Bharat Kumar Jangir (11 December 2013)
                                //if stock price or option price is less than 0 then we use it 0
                                if (pranaPositionWithGreeks.SimulatedUnderlyingStockPrice < 0)
                                {
                                    pranaPositionWithGreeks.SimulatedUnderlyingStockPrice = 0;
                                    pranaPositionWithGreeks.SimulatedUnderlyingStockPriceInBaseCurrency = 0;
                                }
                                if (pranaPositionWithGreeks.SimulatedPrice < 0)
                                {
                                    pranaPositionWithGreeks.SimulatedPrice = 0;
                                    pranaPositionWithGreeks.SimulatedPriceInBaseCurrency = 0;
                                }
                            }
                            else
                            {
                                //Bharat (31 December 2013)
                                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3083
                                if (!_allowedAssets.ContainsKey(pranaPositionWithGreeks.AssetID))
                                {
                                    changeUnderlyingPrice = 1;
                                }
                                pranaPositionWithGreeks.SimulatedUnderlyingStockPrice = changeUnderlyingPrice * marketPrice;
                                pranaPositionWithGreeks.SimulatedPrice = changeUnderlyingPrice * marketPrice;
                                pranaPositionWithGreeks.SimulatedPriceInBaseCurrency = pranaPositionWithGreeks.SimulatedPrice * pranaPositionWithGreeks.FXRate;
                                pranaPositionWithGreeks.SimulatedUnderlyingStockPriceInBaseCurrency = pranaPositionWithGreeks.SimulatedUnderlyingStockPrice * pranaPositionWithGreeks.FXRate;
                            }
                        }
                        pranaPositionWithGreeks.DividendYield = l1Data.FinalDividendYield * 100;
                        pranaPositionWithGreeks.Gamma = 0;
                        CalculatePnls(pranaPositionWithGreeks);
                    }
                }
                if (_isStepAnalysisRequest)
                {
                    _stepAnalysisCache.AddNonOptionsDataTable(_stepParameter, l1Data, string.Empty);
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

        private delegate void UpdatePositionsHandler(PranaPositionWithGreekColl posList);
        public void GetInstance_PositionReceived(PranaPositionWithGreekColl posList)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    UpdatePositionsHandler updateResponseHandler = new UpdatePositionsHandler(UpdatePositionsOnNewThread);
                    updateResponseHandler.BeginInvoke(posList, null, null);
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

        private void UpdatePositionsOnNewThread(PranaPositionWithGreekColl posList)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    _dictBindedData.Clear();
                    _dictUnderlyingFututes.Clear();

                    if (posList != null)
                    {
                        PranaPositionWithGreekColl posListNew = new PranaPositionWithGreekColl();
                        foreach (PranaPositionWithGreeks posGreeks in posList)
                        {
                            PranaPositionWithGreeks posGreeksNew = (PranaPositionWithGreeks)posGreeks.Clone();
                            posListNew.Add(posGreeksNew);
                            AddPositionToDict(posGreeksNew);
                            int baseAssetID = Mapper.GetBaseAsset(posGreeks.AssetID);
                            if (baseAssetID == (int)AssetCategory.Future)
                            {
                                AddPositionToUnderlyingFutureDict(posGreeksNew);
                            }
                        }

                        if (UIValidation.GetInstance().validate(this))
                        {
                            if (this.InvokeRequired)
                            {
                                MsgreceivedInvokeDelegate msgreceivedInvokeDelegate = new MsgreceivedInvokeDelegate(PopulateAndBindData);
                                if (UIValidation.GetInstance().validate(this))
                                {
                                    this.BeginInvoke(msgreceivedInvokeDelegate, new object[] { posListNew });
                                }
                            }
                            else
                            {
                                PopulateAndBindData(posListNew);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (this.IsHandleCreated && !this.IsDisposed)
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
        }

        private void PopulateAndBindData(PranaPositionWithGreekColl posList)
        {
            try
            {
                if (!this.IsDisposed)
                {
                    grdPositions.BeginUpdate();
                    grdPositions.SuspendRowSynchronization();
                    grdPositions.SuspendSummaryUpdates();
                    _isDataAlreadyStressed = false;

                    _pranaPositionWithGreekColl.RaiseListChangedEvents = false;
                    _pranaPositionWithGreekColl.Clear();

                    foreach (PranaPositionWithGreeks posGreeks in posList)
                    {
                        _pranaPositionWithGreekColl.Add(posGreeks);
                    }

                    //Bharat Kumar Jangir (10 April 2014)
                    //For refreshing the SM data of newly added symbol when symbol already inserted in textbox
                    if (pranaSymbolCtrl != null && !pranaSymbolCtrl.Text.Equals(string.Empty))
                    {
                        pranaSymbolCtrl_SymbolEntered(this, new EventArgs<string>(pranaSymbolCtrl.Text));
                    }
                    if (RefreshCompleted != null)
                    {
                        RefreshCompleted(_viewName, null);
                    }
                    EnableForm();
                    toolStripStatusLabel.ForeColor = Color.Green;
                    toolStripStatusLabel.Text = DateTime.Now + ": Refresh Completed";
                }
            }
            catch (Exception ex)
            {
                if (this.IsHandleCreated && !this.IsDisposed)
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
            finally
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    _pranaPositionWithGreekColl.RaiseListChangedEvents = true;
                    grdPositions.ResumeRowSynchronization();
                    grdPositions.ResumeSummaryUpdates(true);
                    grdPositions.EndUpdate();
                }
            }
        }

        public void UpdateGridAsPref()
        {
            try
            {
                SetGridFontSize();
                UpdateHeaderWrapHeader(true);
                grdPositions.DisplayLayout.Bands[0].SortedColumns.RefreshSort(true);
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

        private void SetGridFontSize()
        {
            try
            {
                float fontSize = Convert.ToSingle(RiskPreferenceManager.RiskPrefernece.FontSize);
                Font oldFont = grdPositions.Font;
                Font newFont = new Font(oldFont.FontFamily, fontSize, oldFont.Style, oldFont.Unit, oldFont.GdiCharSet, oldFont.GdiVerticalFont);
                grdPositions.Font = newFont;
                grdPositions.DisplayLayout.Override.SummaryValueAppearance.ForeColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.ColorSummaryText);
                grdData.Font = newFont;
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

        public void UpdateHeaderWrapHeader(bool isUpdatePreference)
        {
            try
            {
                bool wrapHeader = Convert.ToBoolean(RiskPreferenceManager.RiskPrefernece.WrapHeader);
                if (wrapHeader.Equals(true))
                    this.grdPositions.DisplayLayout.Override.WrapHeaderText = DefaultableBoolean.True;
                else
                    this.grdPositions.DisplayLayout.Override.WrapHeaderText = DefaultableBoolean.False;

                if (wrapHeader)
                {
                    foreach (UltraGridColumn col in grdPositions.DisplayLayout.Bands[0].Columns)
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
                    if (RiskLayoutManager.RiskLayout.GetStepAnalLayout(_viewName).StepAnalysisColumns.Count > 0)
                    {
                        if (RiskLayoutManager.RiskLayout.StepAnalysisColumnsList.ContainsKey(_viewName))
                        {
                            List<ColumnData> listColData = RiskLayoutManager.RiskLayout.GetStepAnalLayout(_viewName).StepAnalysisColumns;
                            RiskLayoutManager.LoadColumnsWidthFromXML(grdPositions, listColData);
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

        private void itemAddView_Click(object sender, EventArgs e)
        {
            try
            {
                if (AddViewClick != null)
                {
                    AddViewClick(this, null);
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

        private void ItemDeleteView_Click(object sender, EventArgs e)
        {
            try
            {
                if (DeleteViewClick != null)
                {
                    DeleteViewClick(this, null);
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

        NonParallelShiftsUI _formNonParallelShifts = null;
        private void btnNonParallelShifts_Click(object sender, EventArgs e)
        {
            try
            {
                if (_formNonParallelShifts == null)
                {
                    _formNonParallelShifts = new NonParallelShiftsUI();
                    string formTextView = _formNonParallelShifts.Text + " : " + _viewName;
                    _formNonParallelShifts.Text = formTextView;
                    _formNonParallelShifts.SetUp(_viewID, _viewName);
                    _formNonParallelShifts.Show(this);
                    _formNonParallelShifts.FormClosed += new FormClosedEventHandler(_formNonParallelShifts_FormClosed);
                    _formNonParallelShifts.PrefChanged += new EventHandler(_formNonParallelShifts_PrefChanged);

                    //Bharat (31 December 2013)
                    //Server Request for Getting Used UDAs
                    if (_secMasterClient != null)
                    {
                        if (_secMasterClient.IsConnected)
                        {
                            _secMasterClient.EventInUsedUDARes += new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_secMasterClient_EventInUsedUDARes);
                            _secMasterClient.GetInUsedUDAIds();
                        }
                        else
                        {
                            MessageBox.Show("Trade Server disconnected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                _formNonParallelShifts.BringToFront();
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
        /// Bharat (31 December 2013)
        /// </summary>
        /// <param name="inUsedUDAsDict">Dictionary of Used UDA Symbols in Current Portfolio</param>
        void _secMasterClient_EventInUsedUDARes(object sender, EventArgs<Dictionary<string, Dictionary<int, string>>> e)
        {
            try
            {
                Dictionary<string, Dictionary<int, string>> inUsedUDAsDict = e.Value;
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        InUsedUDADataResInvokeDelegate mainThread = new InUsedUDADataResInvokeDelegate(_secMasterClient_EventInUsedUDARes);
                        this.BeginInvoke(mainThread, new object[] { sender, e });
                    }
                    else
                    {
                        if (_formNonParallelShifts != null && inUsedUDAsDict != null)
                        {
                            _formNonParallelShifts.SetUpGrid(inUsedUDAsDict);
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

        void _formNonParallelShifts_PrefChanged(object sender, EventArgs e)
        {
            try
            {
                if (_preferences != null)
                {
                    _isDataAlreadyStressed = false;
                    if (_preferences.UseNonParallelShifts)
                    {
                        UpdateStressTestInputsControl(false);
                        toolTip1.Active = true;
                    }
                    else
                    {
                        UpdateStressTestInputsControl(true);
                        toolTip1.Active = false;
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

        //event occur when selection window open
        void mnuStepAnal_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (this.ViewName == "Main")
                {
                    //select the delete option from context menu
                    if (mnuStepAnal.Items.Contains(ItemDeleteView))
                        ItemDeleteView.Enabled = false;

                    // select rename option from context menu
                    if (mnuStepAnal.Items.Contains(renameToolStripMenuItem))
                        renameToolStripMenuItem.Enabled = false;
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

        private void UpdateStressTestInputsControl(bool isEnabled)
        {
            try
            {
                if (isEnabled)
                {
                    grpBoxUnderlyingShock.Enabled = true;
                    grpBoxVolShock.Enabled = true;
                    grpBoxIntRateShock.Enabled = true;
                    grpBoxDaysToExpShock.Enabled = true;
                    checkBoxUseAbsoluteValuesForUnderlyingPrice.Enabled = true;
                }
                else
                {
                    grpBoxUnderlyingShock.Enabled = false;
                    grpBoxVolShock.Enabled = false;
                    grpBoxIntRateShock.Enabled = false;
                    grpBoxDaysToExpShock.Enabled = false;
                    checkBoxUseAbsoluteValuesForUnderlyingPrice.Enabled = false;
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

        void _formNonParallelShifts_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_formNonParallelShifts != null)
                {
                    _formNonParallelShifts.FormClosed -= new FormClosedEventHandler(_formNonParallelShifts_FormClosed);
                    _formNonParallelShifts.PrefChanged -= new EventHandler(_formNonParallelShifts_PrefChanged);
                    if (_secMasterClient != null)
                    {
                        _secMasterClient.EventInUsedUDARes -= new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_secMasterClient_EventInUsedUDARes);
                    }
                    _formNonParallelShifts = null;
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

        private void grpBoxStressTestInputs_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                toolTip1.Show("Currently using Non-Parallel Shifts", grpBoxStressTestInputs, 3000);
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

        private void chkBoxUseVolSkew_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool checkState = false;

                if (chkBoxUseVolSkew.CheckState == CheckState.Checked)
                {
                    checkState = true;
                }
                else
                {
                    checkState = false;
                }
                if (UseVolSkew != null)
                {
                    UseVolSkew(this, new EventArgs<bool>(checkState));
                }
                if (UseVolSkewLimitReached && chkBoxUseVolSkew.CheckState == CheckState.Checked)
                {
                    MessageBox.Show("Max number of allowed StressTest views with Vol Skew reached", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public void SaveDefaultLayout()
        {
            try
            {
                StepAnalysisPref stepAnalysisPref = new StepAnalysisPref();
                //Set the preferences value.
                stepAnalysisPref.SetValues(Decimal.Parse(numericUpDownVol.Value.ToString()), Decimal.Parse(numericUpDownUnderLyingPrice.Value.ToString()), Decimal.Parse(numericUpDownIntRate.Value.ToString()), Decimal.Parse(numericUpDownDaysToExp.Value.ToString()), chkbxVol.Checked, chkbxUnderLyingPrice.Checked, chkbxInterestRate.Checked, ckhbxExpiration.Checked, chkBoxUseVolSkew.Checked, checkBoxUseAbsoluteValuesForUnderlyingPrice.Checked, checkBoxUseStressTestDataInStepAnalysis.Checked, multiSelectDropDown1.GetSelectedItemsInDictionary(), chkBoxUseBetaAdj.Checked);
                stepAnalysisPref.StepAnalysisColumns = UltraWinGridUtils.GetColumnsString(grdPositions);
                //It will save the default preferences to xml.
                RiskPreferenceManager.SaveDefaultPreferences(stepAnalysisPref);
                StepAnalLayout stepAnallayout = new StepAnalLayout();
                stepAnallayout.StepAnalysisColumns = RiskLayoutManager.GetGridColumnLayout(grdPositions);
                stepAnallayout.GroupByColumnsCollection = RiskLayoutManager.GetGridGroupByColumnLayout(grdPositions);
                if (this.Height != 0)
                {
                    stepAnallayout.SplitterPosition = (exGrpBoxStepAnalysis.Height * 100) / this.Height;
                }
                //Saves the risk default layout to xml.
                RiskLayoutManager.SaveDefaultStepAnalysisLayout(stepAnallayout);
                //Update status.
                toolStripStatusLabel.ForeColor = Color.Green;
                toolStripStatusLabel.Text = DateTime.Now + ": Layout Saved";
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

        public void SaveLayout()
        {
            try
            {
                RiskPrefernece riskPrefs = RiskPreferenceManager.RiskPrefernece;
                _preferences.SetValues(Decimal.Parse(numericUpDownVol.Value.ToString()), Decimal.Parse(numericUpDownUnderLyingPrice.Value.ToString()), Decimal.Parse(numericUpDownIntRate.Value.ToString()), Decimal.Parse(numericUpDownDaysToExp.Value.ToString()), chkbxVol.Checked, chkbxUnderLyingPrice.Checked, chkbxInterestRate.Checked, ckhbxExpiration.Checked, chkBoxUseVolSkew.Checked, checkBoxUseAbsoluteValuesForUnderlyingPrice.Checked, checkBoxUseStressTestDataInStepAnalysis.Checked, multiSelectDropDown1.GetSelectedItemsInDictionary(), chkBoxUseBetaAdj.Checked);
                _preferences.StepAnalysisColumns = UltraWinGridUtils.GetColumnsString(grdPositions);
                _preferences.StepAnalViewName = _viewName;
                _preferences.StepAnalViewID = _viewID;
                riskPrefs.UpdateStepAnalPrefDict(_viewName, _preferences);
                RiskPreferenceManager.SavePreferences(riskPrefs);

                RiskLayout riskLayout = RiskLayoutManager.RiskLayout;
                StepAnalLayout StepAnallayout = riskLayout.GetStepAnalLayout(_viewName);
                StepAnallayout.StepAnalysisColumns = RiskLayoutManager.GetGridColumnLayout(grdPositions);
                StepAnallayout.GroupByColumnsCollection = RiskLayoutManager.GetGridGroupByColumnLayout(grdPositions);
                if (this.Height != 0)
                {
                    StepAnallayout.SplitterPosition = (exGrpBoxStepAnalysis.Height * 100) / this.Height;
                }
                RiskLayoutManager.SaveRiskLayout();
                toolStripStatusLabel.ForeColor = Color.Green;
                toolStripStatusLabel.Text = DateTime.Now + ": Layout Saved";
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

        private void saveCurrentViewLayout_Click(object sender, EventArgs e)
        {
            try
            {
                SaveLayout();
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

        private void saveAllViewsLayout_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveLayoutAllClick != null)
                {
                    SaveLayoutAllClick(this, null);
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

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (RenameViewClick != null)
                {
                    RenameViewClick(this, null);
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

        private void saveAsDefaultLayout_Click(object sender, EventArgs e)
        {
            try
            {
                SaveDefaultLayout();
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

        private void checkedMultipleItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                //select deselect all the views on the basis of select all checkbox
                if (e.Index == 0)
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);

                    for (int i = 1; i < checkedMultipleItems.Items.Count; i++)
                    {
                        checkedMultipleItems.SetItemCheckState(i, e.NewValue);
                    }
                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                }
                else if (e.NewValue == CheckState.Checked)
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);

                    if (checkedMultipleItems.CheckedItems.Count == checkedMultipleItems.Items.Count - 2)
                    {
                        checkedMultipleItems.SetItemCheckState(0, e.NewValue);
                    }
                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                }
                else if (e.NewValue == CheckState.Unchecked)
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                    if (checkedMultipleItems.CheckedItems.Count == checkedMultipleItems.Items.Count)
                    {
                        checkedMultipleItems.SetItemCheckState(0, e.NewValue);
                    }
                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
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

        private void grdPositions_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (!e.ReInitialize)
                {
                    int AssetID = ((PranaPositionWithGreeks)(e.Row.ListObject)).AssetID;

                    if (AssetID == (int)AssetCategory.Equity || AssetID == (int)(AssetCategory.PrivateEquity) || AssetID == (int)(AssetCategory.CreditDefaultSwap))
                    {
                        e.Row.Cells[COL_ExpirationDate].Value = DateTimeConstants.MinValue;
                        ValueList valuelist = new ValueList();
                        valuelist.ValueListItems.Add(new ValueListItem(DateTimeConstants.MinValue, "N/A"));
                        e.Row.Cells[COL_ExpirationDate].ValueList = valuelist;
                        e.Row.Cells[COL_ExpirationMonth].Value = DateTimeConstants.MinValue;
                        valuelist = new ValueList();
                        valuelist.ValueListItems.Add(new ValueListItem(DateTimeConstants.MinValue, "Non-Expiring Positions"));
                        e.Row.Cells[COL_ExpirationMonth].ValueList = valuelist;
                        e.Row.Update();
                    }
                    List<EnumerationValue> PutOrCallType = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(OptionType));
                    ValueList PutOrCallValueList = new ValueList();
                    foreach (EnumerationValue value in PutOrCallType)
                    {
                        PutOrCallValueList.ValueListItems.Add(value.Value, value.DisplayText.Substring(0, 1));
                    }
                    PutOrCallValueList.ValueListItems.Add(int.MinValue, " ");
                    e.Row.Cells[COL_PutOrCall].ValueList = PutOrCallValueList;
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

        private void checkBoxUseAbsoluteValuesForUnderlyingPrice_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxUseAbsoluteValuesForUnderlyingPrice.Checked)
                {
                    chkbxUnderLyingPrice.Text = "Underlying Price";
                }
                else
                {
                    chkbxUnderLyingPrice.Text = "Underlying Price %";
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

        private void chkbxVol_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _isDataAlreadyStressed = false;
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

        private void chkbxUnderLyingPrice_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _isDataAlreadyStressed = false;
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

        private void chkbxInterestRate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _isDataAlreadyStressed = false;
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

        private void ckhbxExpiration_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _isDataAlreadyStressed = false;
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

        private void CtrlStepAnalysis_Load(object sender, EventArgs e)
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

        internal void ProcessPublishedSnapShotData(ResponseObj responseObj)
        {
            try
            {
                lock (_lockerStressTest)
                {
                    ResetTimer();

                    if (responseObj.Data == null && responseObj.CalculatedGreeks.Count > 0)
                        UpdateData(responseObj);
                    else
                        UpdateNonOptionsWithSnapShotResponse(responseObj.Data);

                    _responseReceivedCount++;
                    int percentageCompletion = _responseReceivedCount * 100 / (_listRequestedSymbols.Count + _responseReceivedCount);
                    if (_isStressTestRequest)
                    {
                        if (_listRequestedSymbols.Count > 0)
                            toolStripStatusLabel.Text = DateTime.Now + ": " + percentageCompletion + "% Stress Test Completed";
                        else
                            toolStripStatusLabel.Text = DateTime.Now + ": Stress Test Completed";
                    }
                    if (_isStepAnalysisRequest)
                    {
                        if (_listRequestedSymbols.Count > 0)
                            toolStripStatusLabel.Text = DateTime.Now + ": " + percentageCompletion + "% Step Analysis Completed";
                        else
                            toolStripStatusLabel.Text = DateTime.Now + ": Step Analysis Completed";
                    }
                    if (_listRequestedSymbols.Count == 0)
                    {
                        if (_isStepAnalysisRequest)
                            StepAnalysisCompleted();
                        if (_isStressTestRequest)
                            StressTestCompleted();
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

        internal void ProcessPublishedStepAnalysisData(List<StepAnalysisResponse> stepRes)
        {
            try
            {
                lock (_lockerStepAnalysis)
                {
                    ResetTimer();
                    _stepAnalysisCache.AddOptionsDataTable(this._stepParameter, stepRes);
                    _responseReceivedCount++;
                    if (stepRes.Count > 0)
                    {
                        if (_listRequestedSymbols.Contains(stepRes[0].Symbol))
                        {
                            _listRequestedSymbols.Remove(stepRes[0].Symbol);
                        }
                    }
                    if (_listRequestedSymbols.Count > 0)
                    {
                        int percentageCompletion = _responseReceivedCount * 100 / (_listRequestedSymbols.Count + _responseReceivedCount);
                        toolStripStatusLabel.Text = DateTime.Now + ": " + percentageCompletion + "% Step Analysis Completed";
                    }
                    else
                    {
                        toolStripStatusLabel.Text = DateTime.Now + ": Step Analysis Completed";
                    }
                    if (_listRequestedSymbols.Count == 0)
                    {
                        StepAnalysisCompleted();
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

        private void grdPositions_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdData_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            UnwireEvents();

            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (headerCheckBox != null)
                {
                    headerCheckBox.Dispose();
                }
                if (_greekAnalysisServiceProxy != null)
                {
                    _greekAnalysisServiceProxy.Dispose();
                }
                if (grdPositions.DisplayLayout.Bands[0] != null && grdPositions.DisplayLayout.Bands[0].Summaries.Count > 0)
                {
                    grdPositions.DisplayLayout.Bands[0].Summaries.Clear();
                }
                if (grdPositions != null)
                    grdPositions.Dispose();

                if (dtGraphSelected != null)
                {
                    dtGraphSelected.Dispose();
                }
                if (_columnSorted != null)
                {
                    _columnSorted.Dispose();
                }
                if (_formNonParallelShifts != null)
                {
                    _formNonParallelShifts.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }

    public enum CalculatedParamters
    {
        Delta,
        Gamma,
        Theta,
        Vega,
        Rho,
        DollarDeltaInBaseCurrency,
        DollarGammaInBaseCurrency,
        DollarThetaInBaseCurrency,
        DollarVegaInBaseCurrency,
        DollarRhoInBaseCurrency,
        SimulatedPriceInBaseCurrency,
        DeltaAdjExposureInBaseCurrency,
        CostBasisUnrealizedPnLInBaseCurrency,
        SimulatedPnlInBaseCurrency
    }
}
