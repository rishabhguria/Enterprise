using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Analytics.Classes;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.StringUtilities;
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
using System.Windows.Forms;

namespace Prana.Analytics
{
    public partial class CtrlRiskSimulation : UserControl
    {
        private List<string> _listRequest = new List<string>();
        private bool isAlreadyStarted = false;
        SecMasterBaseObj _secMasterObjEnteredSymbol = null;
        private DataTable _dtCalculatedRisks = new DataTable();
        private DataTable _dataPortfolio = new DataTable();
        private Dictionary<string, Dictionary<string, ComponentRiskData>> _componentRiskByGrouping = new Dictionary<string, Dictionary<string, ComponentRiskData>>();
        private delegate void UIThreadMarshellerForComplete(QueueMessage qMsg);
        private delegate void UIThreadMarshellersSecMaster(object sender, EventArgs<SecMasterBaseObj> e);
        private CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();
        public event EventHandler RefreshCompleted;
        private delegate void UIThreadMarshellerUpdateToolStrip(string statusMessage);
        ISecurityMasterServices _secMasterClient = null;
        GroupSortComparer _groupSortComparer = new GroupSortComparer();
        UltraGridColumn _columnSorted = null;
        List<string> GroupByColumnsCollection = new List<string>();
        private bool _isReloadingLayout = false;
        private RiskParamameter _riskParamsOld = new RiskParamameter();
        private RiskParamameter _riskParamsNew = new RiskParamameter();

        public CtrlRiskSimulation()
        {
            InitializeComponent();
        }

        bool _isRiskCalculatedGroup = true;
        bool _isRiskCalculatedIndividual = true;

        string _firstLevelGroup = string.Empty;
        string _secondLevelGroup = string.Empty;
        private string tabRiskSimulation = "Risk Simulation";

        private int _responseReceivedCount = 0;
        private int _requestedCallsCount = 0;
        private static readonly object _locker = new object();
        #region Column Names
        private const string COL_IsChecked = "IsChecked";
        private const string COL_Symbol = "Symbol";
        private const string COL_FactSet = "FactSetSymbol";
        private const string COL_Activ = "ActivSymbol";
        private const string COL_SecurityName = "SecurityName";
        private const string COL_UnderlyingSymbol = "UnderlyingSymbol";
        private const string COL_Account = "Account";
        private const string COL_MasterFund = "MasterFund";
        private const string COL_Strategy = "Strategy";
        private const string COL_OldQuantity = "OldQuantity";
        private const string COL_NewQuantity = "NewQuantity";
        private const string COL_PercentageChange = "PercentageChange";
        private const string COL_OldRisk = "OldRisk";
        private const string COL_NewRisk = "NewRisk";
        private const string COL_OldStandardDeviation = "OldStandardDeviation";
        private const string COL_NewStandardDeviation = "NewStandardDeviation";
        private const string COL_OldComponentRisk = "OldComponentRisk";
        private const string COL_NewComponentRisk = "NewComponentRisk";
        private const string COL_SectorName = "SectorName";
        private const string COL_CountryName = "CountryName";
        private const string COL_UDAAsset = "UDAAsset";
        private const string COL_PSSymbol = "PSSymbol";
        private const string COL_SecurityTypeName = "SecurityTypeName";
        private const string COL_ContractMultiplier = "ContractMultiplier";
        private const string COL_AssetName = "AssetName";
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
        private const string COL_Delta = "Delta";
        private const string COL_OldDeltaAdjPosition = "OldDeltaAdjPosition";
        private const string COL_NewDeltaAdjPosition = "NewDeltaAdjPosition";
        #endregion

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

        public void SetUp(ISecurityMasterServices secMasterClient, bool isOnlyLoadPositions, ref bool isTabBusy)
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
                    _secMasterClient = secMasterClient;
                    _secMasterClient.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_secMasterClient_SecMstrDataResponse);
                    SetCalculatedRiskTable();
                    SetGridFontSize();
                    if (RiskPreferenceManager.RiskPrefernece.IsAutoLoadDataOnStartup)
                    {
                        GetData();
                        isTabBusy = true;
                    }
                    BindGrid();
                    BindBenchMarkCombo();
                    SetColumns();
                    isAlreadyStarted = true;
                    datePickerEnddate.DateTime = DateTime.Now;
                    datePickerStartdate.DateTime = DateTime.Now.AddDays(-RiskPreferenceManager.RiskPrefernece._riskSimulationDateRange);
                    SetDataTable();
                    if (_riskServiceProxy != null)
                    {
                        _riskServiceProxy.Dispose();
                    }
                    CreateRiskServiceProxy();
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
                btnSimulation.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSimulation.ForeColor = System.Drawing.Color.White;
                btnSimulation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSimulation.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSimulation.UseAppStyling = false;
                btnSimulation.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAddPosition.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAddPosition.ForeColor = System.Drawing.Color.White;
                btnAddPosition.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAddPosition.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAddPosition.UseAppStyling = false;
                btnAddPosition.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void _secMasterClient_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                if (secMasterObj.AssetID == (int)AssetCategory.FX || secMasterObj.AssetID == (int)AssetCategory.FXForward)
                {
                    if (FXandFXFWDSymbolGenerator.IsValidFxAndFwdSymbol(secMasterObj))
                    {
                        ValidateAndMarshal(sender, e);
                    }
                }
                else
                {
                    ValidateAndMarshal(sender, e);
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

        private void ValidateAndMarshal(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                UIThreadMarshellersSecMaster mi = new UIThreadMarshellersSecMaster(_secMasterClient_SecMstrDataResponse);
                if (UIValidation.GetInstance().validate(grdData))
                {
                    if (grdData.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { sender, e });
                    }
                    else
                    {
                        if (pranaSymbolCtrl1.Text == e.Value.TickerSymbol)
                        {
                            _secMasterObjEnteredSymbol = e.Value;
                            btnAddPosition.Enabled = true;
                            btnAddPosition.BackColor = Color.FromArgb(55, 67, 85);
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
        }

        private void SetCalculatedRiskTable()
        {
            try
            {
                _dtCalculatedRisks.Columns.Add(new DataColumn("Grouping Criterion"));
                _dtCalculatedRisks.Columns.Add(new DataColumn("Risk", typeof(double)));
                _dtCalculatedRisks.Columns.Add(new DataColumn("Correlation", typeof(double)));
                _dtCalculatedRisks.Columns.Add(new DataColumn("StandardDeviation", typeof(double)));

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

        private void SetDataTable()
        {
            try
            {
                _dataPortfolio.Columns.Add(new DataColumn("Grouping Criterion"));
                _dataPortfolio.Columns.Add(new DataColumn("Risk", typeof(double)));
                _dataPortfolio.Columns.Add(new DataColumn("Correlation", typeof(double)));
                _dataPortfolio.Columns.Add(new DataColumn("StandardDeviation", typeof(double)));

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

        void GetInstance_RiskResponseCompleted(QueueMessage qMsg)
        {
            // send to main UI thread
            UIThreadMarshellerForComplete mi = new UIThreadMarshellerForComplete(GetInstance_RiskResponseCompleted);
            try
            {
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

        private delegate void UIThreadMarsheller(PranaRequestCarrier pranaRequestCarrier);
        Dictionary<string, List<PranaRiskObj>> _dictRiskObjs = new Dictionary<string, List<PranaRiskObj>>();
        Dictionary<string, List<PranaRiskSimulationObj>> _dictRiskSimulationObjs = new Dictionary<string, List<PranaRiskSimulationObj>>();
        PranaRiskObjColl _riskObjcollection = new PranaRiskObjColl();
        List<PranaRiskSimulationObj> _riskObjSimulationcollection = new List<PranaRiskSimulationObj>();
        private void BindGrid()
        {
            try
            {
                grdData.DataSource = _riskObjSimulationcollection;
                grdData.DataBind();
                grdData.DisplayLayout.Override.CellClickAction = CellClickAction.Edit;

                foreach (UltraGridColumn column in grdData.DisplayLayout.Bands[0].Columns)
                {
                    column.CellActivation = Activation.NoEdit;
                }
                UltraGridColumn columnPercentage = grdData.DisplayLayout.Bands[0].Columns[COL_PercentageChange];
                columnPercentage.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoubleWithSpin;
                columnPercentage.CellActivation = Activation.AllowEdit;
                columnPercentage.MaxValue = 100;
                columnPercentage.MinValue = -100;
                UltraGridColumn columnNewQuantity = grdData.DisplayLayout.Bands[0].Columns[COL_NewQuantity];
                columnNewQuantity.CellActivation = Activation.AllowEdit;
                UltraGridColumn columnIsChecked = grdData.DisplayLayout.Bands[0].Columns[COL_IsChecked];
                columnIsChecked.CellActivation = Activation.AllowEdit;
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
                if (RiskLayoutManager.RiskLayout.RiskSimulationColumns.Count > 0)
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
                List<ColumnData> listColData = RiskLayoutManager.RiskLayout.RiskSimulationColumns;
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
            try
            {
                List<string> colAll = GetAllDisplayableColumns();
                List<string> colDefault = GetAllDefaultColumns();
                List<string> colVisible = Prana.Utilities.MiscUtilities.GeneralUtilities.GetListFromString(RiskPreferenceManager.RiskPrefernece.RiskSimulationColumns, ',');

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

        private List<string> GetAllDefaultColumns()
        {
            List<string> colDefault = new List<string>();
            try
            {
                colDefault.Add(COL_IsChecked);
                colDefault.Add(COL_Symbol);
                colDefault.Add(COL_SecurityName);
                colDefault.Add(COL_UnderlyingSymbol);
                colDefault.Add(COL_Account);
                colDefault.Add(COL_MasterFund);
                colDefault.Add(COL_Strategy);
                colDefault.Add(COL_OldQuantity);
                colDefault.Add(COL_NewQuantity);
                colDefault.Add(COL_PercentageChange);
                colDefault.Add(COL_OldRisk);
                colDefault.Add(COL_NewRisk);
                colDefault.Add(COL_OldStandardDeviation);
                colDefault.Add(COL_NewStandardDeviation);
                colDefault.Add(COL_OldComponentRisk);
                colDefault.Add(COL_NewComponentRisk);
                colDefault.Add(COL_SectorName);
                colDefault.Add(COL_CountryName);
                colDefault.Add(COL_UDAAsset);
                colDefault.Add(COL_PSSymbol);
                colDefault.Add(COL_SecurityTypeName);
                colDefault.Add(COL_ContractMultiplier);
                colDefault.Add(COL_AssetName);
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
                colAll.Add(COL_TradeAttribute1);
                colAll.Add(COL_TradeAttribute2);
                colAll.Add(COL_TradeAttribute3);
                colAll.Add(COL_TradeAttribute4);
                colAll.Add(COL_TradeAttribute5);
                colAll.Add(COL_TradeAttribute6);
                colAll.Add(COL_BloombergSymbol);
                colAll.Add(COL_FactSet);
                colAll.Add(COL_Activ);
                colAll.Add(COL_IDCOSymbol);
                colAll.Add(COL_ISINSymbol);
                colAll.Add(COL_SedolSymbol);
                colAll.Add(COL_OSISymbol);
                colAll.Add(COL_CusipSymbol);
                colAll.Add(COL_Delta);
                colAll.Add(COL_OldDeltaAdjPosition);
                colAll.Add(COL_NewDeltaAdjPosition);
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
                ColumnsCollection columns = grdData.DisplayLayout.Bands[0].Columns;
                columns[COL_NewQuantity].Format = "#,#.#";
                columns[COL_OldQuantity].Format = "#,#0.#";
                columns[COL_PercentageChange].Format = "#.00";
                columns[COL_Delta].Format = "#.0000";
                columns[COL_OldDeltaAdjPosition].Format = "#,#0.#";
                columns[COL_NewDeltaAdjPosition].Format = "#,#.#";
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

                band.Columns[COL_CountryName].Header.Caption = "Country";
                band.Columns[COL_NewQuantity].Header.Caption = "Position (New)";
                band.Columns[COL_NewRisk].Header.Caption = "Risk (New)";
                band.Columns[COL_NewStandardDeviation].Header.Caption = "Standard Deviation (New)";
                band.Columns[COL_NewComponentRisk].Header.Caption = "Component Risk (New)";
                band.Columns[COL_OldQuantity].Header.Caption = "Position (Old)";
                band.Columns[COL_OldRisk].Header.Caption = "Risk (Old)";
                band.Columns[COL_OldStandardDeviation].Header.Caption = "Standard Deviation (Old)";
                band.Columns[COL_OldComponentRisk].Header.Caption = "Component Risk (Old)";
                band.Columns[COL_PercentageChange].Header.Caption = "% Change";
                band.Columns[COL_SectorName].Header.Caption = "Sector";
                band.Columns[COL_SecurityName].Header.Caption = "Security Name";
                band.Columns[COL_SecurityTypeName].Header.Caption = "Security Type";
                band.Columns[COL_UnderlyingSymbol].Header.Caption = "Underlying Symbol";
                band.Columns[COL_PSSymbol].Header.Caption = "PS Symbol";
                band.Columns[COL_MasterFund].Header.Caption = "Master Fund";
                band.Columns[COL_ContractMultiplier].Header.Caption = "Multiplier";

                band.Columns[COL_TradeAttribute1].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 1");
                band.Columns[COL_TradeAttribute2].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 2");
                band.Columns[COL_TradeAttribute3].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 3");
                band.Columns[COL_TradeAttribute4].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 4");
                band.Columns[COL_TradeAttribute5].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 5");
                band.Columns[COL_TradeAttribute6].Header.Caption = CommonDataCache.CachedDataManager.GetInstance.GetAttributeNameForValue("Trade Attribute 6");

                band.Columns[COL_BloombergSymbol].Header.Caption = "Bloomberg";
                band.Columns[COL_BloombergSymbolWithExchangeCode].Header.Caption = "Bloomberg Symbol(with Exchange Code)";
                band.Columns[COL_FactSet].Header.Caption = "FactSet Symbol";
                band.Columns[COL_Activ].Header.Caption = "ACTIV Symbol";
                band.Columns[COL_IDCOSymbol].Header.Caption = "IDCO";
                band.Columns[COL_ISINSymbol].Header.Caption = "ISIN";
                band.Columns[COL_SedolSymbol].Header.Caption = "SEDOL";
                band.Columns[COL_OSISymbol].Header.Caption = "OSI";
                band.Columns[COL_CusipSymbol].Header.Caption = "CUSIP";
                band.Columns[COL_UDAAsset].Header.Caption = "User Asset";
                band.Columns[COL_OldDeltaAdjPosition].Header.Caption = "Delta Position (Old)";
                band.Columns[COL_NewDeltaAdjPosition].Header.Caption = "Delta Position (New)";
                band.Columns[COL_AssetName].Header.Caption = "Asset";
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

        private void SetColumnSummaries(UltraGrid grid)
        {
            try
            {
                UltraGridBand band = grid.DisplayLayout.Bands[0];
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
                        case COL_SecurityName:
                        case COL_UnderlyingSymbol:
                        case COL_Account:
                        case COL_MasterFund:
                        case COL_Strategy:
                        case COL_SectorName:
                        case COL_CountryName:
                        case COL_UDAAsset:
                        case COL_PSSymbol:
                        case COL_SecurityTypeName:
                        case COL_AssetName:
                        case COL_TradeAttribute1:
                        case COL_TradeAttribute2:
                        case COL_TradeAttribute3:
                        case COL_TradeAttribute4:
                        case COL_TradeAttribute5:
                        case COL_TradeAttribute6:
                        case COL_BloombergSymbol:
                        case COL_BloombergSymbolWithExchangeCode:
                        case COL_FactSet:
                        case COL_Activ:
                        case COL_IDCOSymbol:
                        case COL_ISINSymbol:
                        case COL_SedolSymbol:
                        case COL_OSISymbol:
                        case COL_CusipSymbol:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcText"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_ContractMultiplier:
                        case COL_Delta:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcNum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#.0000}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_OldDeltaAdjPosition:
                        case COL_NewDeltaAdjPosition:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSymbolSum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,0}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_PercentageChange:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,0.#}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_OldQuantity:
                        case COL_NewQuantity:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.Custom, summFactory.GetSummaryCalculator("SummaryCalcSymbolSum"), ultraGridColumn, SummaryPosition.UseSummaryPositionColumn, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,0.#}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;

                        case COL_OldRisk:
                        case COL_NewRisk:
                        case COL_OldStandardDeviation:
                        case COL_NewStandardDeviation:
                        case COL_OldComponentRisk:
                        case COL_NewComponentRisk:
                            band.Summaries.Add(ultraGridColumn.Key, SummaryType.External, ultraGridColumn);
                            band.Summaries[ultraGridColumn.Key].DisplayFormat = "{0:#,0}";
                            band.Summaries[ultraGridColumn.Key].Appearance.TextHAlign = HAlign.Right;
                            break;
                    }
                }
                grid.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.Bottom;
                grid.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.InGroupByRows;
                grid.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.RootRowsFootersOnly;
                grid.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
                grid.DisplayLayout.Override.ExpansionIndicator = ShowExpansionIndicator.CheckOnDisplay;
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
                allowedGroupByColumns.Add(COL_Account);
                allowedGroupByColumns.Add(COL_Strategy);
                allowedGroupByColumns.Add(COL_Symbol);
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
            try
            {
                ColumnsCollection columns = grdData.DisplayLayout.Bands[0].Columns;
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

        private void GetData()
        {
            try
            {
                toolStripStatusLabel.ForeColor = Color.Red;
                toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Refreshing Positions...";
                _dictRiskSimulationObjs.Clear();
                _dictRiskObjs.Clear();
                _riskObjcollection.Clear();
                _riskObjSimulationcollection.Clear();
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void AddPositionToUIList(PranaRiskSimulationObj risksimulation)
        {
            try
            {
                _riskObjSimulationcollection.Add(risksimulation);
                if (!_dictRiskSimulationObjs.ContainsKey(risksimulation.Symbol))
                {
                    List<PranaRiskSimulationObj> list = new List<PranaRiskSimulationObj>();
                    list.Add(risksimulation);
                    _dictRiskSimulationObjs.Add(risksimulation.Symbol, list);
                }
                else
                {
                    _dictRiskSimulationObjs[risksimulation.Symbol].Add(risksimulation);
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
                if (RiskPreferenceManager.RiskPrefernece.CorrelationSymbolName != null)
                    cmbbxBenchMark.Value = RiskPreferenceManager.RiskPrefernece.CorrelationSymbolName;
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
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnSimulation_Click(object sender, EventArgs e)
        {
            try
            {
                // string requestID = string.Empty;
                if (ValidateRequest())
                {
                    lock (_locker)
                    {
                        _responseReceivedCount = 0;
                        _requestedCallsCount = 0;
                    }
                    ClearData();
                    DisableForm();
                    btnSimulation.BackColor = Color.Red;
                    btnSimulation.Text = "Simulating...";
                    toolStripStatusLabel.ForeColor = Color.Red;
                    toolStripStatusLabel.Text = DateTime.Now.ToString() + ": Calculating...";
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

        void CalculateRisk()
        {
            try
            {
                if (!CheckRiskServiceConnected())
                {
                    UpdateToolStripStatus("Risk Server not connected to Portfolio Science.");
                    return;
                }
                // Updated the risk preferences as the preferences are only kept at the client side in a xml file and
                // the preferences has to be communicated to the pricing server as it is using these in several calculations
                _riskServiceProxy.InnerChannel.UpdateRiskPreferences(RiskPreferenceManager.RiskPrefernece);

                _isRiskCalculatedIndividual = false;
                _componentRiskByGrouping.Clear();
                PranaRiskObjColl oldQtyColl = new PranaRiskObjColl();
                PranaRiskObjColl newQtyColl = new PranaRiskObjColl();

                List<PranaRiskSimulationObj> listSimuObj = GetSelectedRows();
                foreach (PranaRiskSimulationObj simuObj in listSimuObj)
                {
                    oldQtyColl.Add(CreateRiskObjWithOldQty(simuObj));
                    newQtyColl.Add(CreateRiskObjWithNewQty(simuObj));
                }

                #region Individual Risk
                object[] argumentsIndividual = new object[2];
                PranaRequestCarrier pranaRequestCarrierOld = new PranaRequestCarrier();
                PranaRequestCarrier pranaRequestCarrierNew = new PranaRequestCarrier();

                //individual level request with old Qty
                if (oldQtyColl.Count > 0) //request for old individual rows
                {
                    UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];

                    _riskParamsOld.IsRiskRequired = gridBand.Columns["OldRisk"].IsVisibleInLayout ? true : false;
                    _riskParamsOld.IsComponentRiskRequired = gridBand.Columns["OldComponentRisk"].IsVisibleInLayout ? true : false;
                    _riskParamsOld.IsstddevRequired = gridBand.Columns["OldStandardDeviation"].IsVisibleInLayout ? true : false;

                    pranaRequestCarrierOld = new PranaRequestCarrier(oldQtyColl, datePickerStartdate.DateTime, datePickerEnddate.DateTime, cmbbxBenchMark.Value.ToString(), "1", 0, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, false, _riskParamsOld);
                    pranaRequestCarrierOld.GroupingName = "Old Portfolio";
                    pranaRequestCarrierOld.ForNew = false;
                }

                //individual level request with new Qty
                if (newQtyColl.Count > 0) //request for new individual rows
                {
                    UltraGridBand gridBand = grdData.DisplayLayout.Bands[0];

                    _riskParamsNew.IsRiskRequired = gridBand.Columns["NewRisk"].IsVisibleInLayout ? true : false;
                    _riskParamsNew.IsComponentRiskRequired = gridBand.Columns["NewComponentRisk"].IsVisibleInLayout ? true : false;
                    _riskParamsNew.IsstddevRequired = gridBand.Columns["NewStandardDeviation"].IsVisibleInLayout ? true : false;

                    pranaRequestCarrierNew = new PranaRequestCarrier(newQtyColl, datePickerStartdate.DateTime, datePickerEnddate.DateTime, cmbbxBenchMark.Value.ToString(), "1", 0, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, false, _riskParamsNew);
                    pranaRequestCarrierNew.GroupingName = "New Portfolio";
                    pranaRequestCarrierNew.ForNew = true;
                }
                argumentsIndividual[0] = pranaRequestCarrierOld;
                argumentsIndividual[1] = pranaRequestCarrierNew;
                CalculateRiskIndividualData(argumentsIndividual);
                #endregion

                #region Group Risk
                if (!string.IsNullOrEmpty(_firstLevelGroup))
                {
                    object[] argumentsGroup = new object[4];
                    argumentsGroup[0] = cmbbxBenchMark.Value.ToString();

                    // portfolio Risk
                    PranaRequestCarrier pranaRequestCarrierPortfolioOld = new PranaRequestCarrier(oldQtyColl, datePickerStartdate.DateTime, datePickerEnddate.DateTime, cmbbxBenchMark.Value.ToString(), "1", 0, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, false, _riskParamsOld);
                    pranaRequestCarrierPortfolioOld.GroupingName = "Old Portfolio";

                    //Portfolio request for new portfolio
                    PranaRequestCarrier pranaRequestCarrierPortfolioNew = new PranaRequestCarrier(newQtyColl, datePickerStartdate.DateTime, datePickerEnddate.DateTime, cmbbxBenchMark.Value.ToString(), "1", 0, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, false, _riskParamsNew);
                    pranaRequestCarrierPortfolioNew.GroupingName = "New Portfolio";
                    argumentsGroup[1] = pranaRequestCarrierPortfolioOld;
                    argumentsGroup[2] = pranaRequestCarrierPortfolioNew;

                    Dictionary<string, PranaRiskObjColl[]> dictGroupedRiskColl = new Dictionary<string, PranaRiskObjColl[]>();
                    _isRiskCalculatedGroup = false;
                    _dtCalculatedRisks.Rows.Clear();
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
                                        oldQtyColl = new PranaRiskObjColl();
                                        newQtyColl = new PranaRiskObjColl();
                                        PranaRiskObjColl[] groupRiskCollectionSecondLevel = new PranaRiskObjColl[2];
                                        groupRiskCollectionSecondLevel[0] = oldQtyColl;
                                        groupRiskCollectionSecondLevel[1] = newQtyColl;

                                        UltraGridGroupByRow groupByRowSecondLevel = (UltraGridGroupByRow)groupRowSecondLevel;

                                        RequestCollection(groupRowSecondLevel, oldQtyColl, newQtyColl);
                                        if (oldQtyColl.Count > 0 && !dictGroupedRiskColl.ContainsKey(groupByRow.Value.ToString() + "|" + groupByRowSecondLevel.Value.ToString()))
                                            dictGroupedRiskColl.Add(groupByRow.Value.ToString() + "|" + groupByRowSecondLevel.Value.ToString(), groupRiskCollectionSecondLevel);
                                    }
                                }
                            }

                            oldQtyColl = new PranaRiskObjColl();
                            newQtyColl = new PranaRiskObjColl();
                            PranaRiskObjColl[] groupRiskCollection = new PranaRiskObjColl[2];
                            groupRiskCollection[0] = oldQtyColl;
                            groupRiskCollection[1] = newQtyColl;

                            RequestCollection(groupRow, oldQtyColl, newQtyColl);
                            if (oldQtyColl.Count > 0 && !dictGroupedRiskColl.ContainsKey(_firstLevelGroup + "|" + groupByRow.Value.ToString()))
                                dictGroupedRiskColl.Add(_firstLevelGroup + "|" + groupByRow.Value.ToString(), groupRiskCollection);
                        }
                    }
                    lock (_locker)
                    {
                        _requestedCallsCount = _requestedCallsCount + dictGroupedRiskColl.Count * 2;
                    }
                    argumentsGroup[3] = dictGroupedRiskColl;
                    CalculateRiskGroupData(argumentsGroup);
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

        async void CalculateRiskIndividualData(object[] arguments)
        {
            try
            {
                if (arguments != null)
                {
                    PranaRequestCarrier pranaRequestCarrierOld = arguments[0] as PranaRequestCarrier;
                    PranaRequestCarrier pranaRequestCarrierNew = arguments[1] as PranaRequestCarrier;

                    if (pranaRequestCarrierOld.IndividualSymbolList.Count > 0)
                    {
                        lock (_locker)
                        {
                            _requestedCallsCount = _requestedCallsCount + 1;
                        }
                        var pranaRequestCarrierOldTask = _riskServiceProxy.InnerChannel.CalculateRiskRelatedData(pranaRequestCarrierOld, true).ContinueWith(CompleteWorkAfterRiskResponse);
                    }
                    if (pranaRequestCarrierNew.IndividualSymbolList.Count > 0)
                    {
                        lock (_locker)
                        {
                            _requestedCallsCount = _requestedCallsCount + 1;
                        }
                        var pranaRequestCarrierNewTask = _riskServiceProxy.InnerChannel.CalculateRiskRelatedData(pranaRequestCarrierNew, true).ContinueWith(CompleteWorkAfterRiskResponse);
                        await pranaRequestCarrierNewTask;
                    }
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

        async void CalculateRiskGroupData(object[] arguments)
        {
            try
            {
                if (arguments != null)
                {
                    string benchMarkSymbol = arguments[0].ToString();
                    PranaRequestCarrier pranaRequestCarrierGroupOld = arguments[1] as PranaRequestCarrier;
                    PranaRequestCarrier pranaRequestCarrierGroupNew = arguments[2] as PranaRequestCarrier;
                    if (!_isRiskCalculatedGroup)
                    {
                        Dictionary<string, PranaRiskObjColl[]> dictGroupedRiskColl = (Dictionary<string, PranaRiskObjColl[]>)arguments[3];
                        foreach (KeyValuePair<string, PranaRiskObjColl[]> kp in dictGroupedRiskColl)
                        {
                            PranaRiskObjColl oldQtyColl = kp.Value[0];
                            PranaRiskObjColl newQtyColl = kp.Value[1];

                            PranaRequestCarrier pranaRequestCarrierBandOldGroup = new PranaRequestCarrier(oldQtyColl, datePickerStartdate.DateTime, datePickerEnddate.DateTime, benchMarkSymbol, "1", 0, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, false, _riskParamsOld);
                            pranaRequestCarrierBandOldGroup.GroupingName = kp.Key.ToString() + "$OLDGROUP";
                            pranaRequestCarrierBandOldGroup.ForNew = false;

                            PranaRequestCarrier pranaRequestCarrierBandNewGroup = new PranaRequestCarrier(newQtyColl, datePickerStartdate.DateTime, datePickerEnddate.DateTime, benchMarkSymbol, "1", 0, RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn, false, _riskParamsNew);
                            pranaRequestCarrierBandNewGroup.GroupingName = kp.Key.ToString() + "$NEWGROUP";
                            pranaRequestCarrierBandNewGroup.ForNew = true;

                            var pranaRequestCarrierBandOldGroupTask = _riskServiceProxy.InnerChannel.CalculateRiskRelatedData(pranaRequestCarrierBandOldGroup, true).ContinueWith(CompleteWorkAfterRiskResponse);

                            var pranaRequestCarrierBandNewGroupTask = _riskServiceProxy.InnerChannel.CalculateRiskRelatedData(pranaRequestCarrierBandNewGroup, true).ContinueWith(CompleteWorkAfterRiskResponse);
                            await pranaRequestCarrierBandNewGroupTask;
                        }
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
                    RiskSimulationCompleted(completedTask.Result);
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

        private void RequestCollection(UltraGridRow groupRow, PranaRiskObjColl oldQtyColl, PranaRiskObjColl newQtyColl)
        {
            try
            {
                UltraGridRow[] rows = groupRow.ChildBands[0].Rows.GetFilteredInNonGroupByRows();
                foreach (UltraGridRow row in rows)
                {
                    PranaRiskSimulationObj riskSimObjIndiv = (PranaRiskSimulationObj)row.ListObject;
                    if (riskSimObjIndiv.IsChecked)
                    {
                        oldQtyColl.Add(CreateRiskObjWithOldQty(riskSimObjIndiv));
                        newQtyColl.Add(CreateRiskObjWithNewQty(riskSimObjIndiv));
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

        private List<PranaRiskSimulationObj> GetSelectedRows()
        {
            UltraGridRow[] rows = grdData.Rows.GetFilteredInNonGroupByRows();
            List<PranaRiskSimulationObj> list = new List<PranaRiskSimulationObj>();
            try
            {
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells[COL_IsChecked].Text.ToUpper().Equals("TRUE"))
                    {
                        PranaRiskSimulationObj simuObj = (PranaRiskSimulationObj)row.ListObject;
                        list.Add(simuObj);
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
            return list;
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
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
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
                _dtCalculatedRisks.Rows.Clear();
                _componentRiskByGrouping.Clear();
                portfolioResultsCtrl1.Refresh();
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

        void RiskSimulationCompleted(PranaRequestCarrier pranaRequestCarrier)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    UIThreadMarsheller mi = new UIThreadMarsheller(RiskSimulationCompleted);
                    if (grdData.InvokeRequired)
                    {
                        this.BeginInvoke(mi, new object[] { pranaRequestCarrier });
                    }
                    else
                    {
                        if (pranaRequestCarrier != null)
                        {
                            if (pranaRequestCarrier.GroupingName == "Old Portfolio" || pranaRequestCarrier.GroupingName == "New Portfolio")
                            {
                                DataRow dr = _dataPortfolio.NewRow();
                                dr["Grouping Criterion"] = pranaRequestCarrier.GroupingName;
                                dr["Correlation"] = pranaRequestCarrier.Correlation;
                                dr["Risk"] = Math.Round(pranaRequestCarrier.PortfolioRisk);
                                dr["StandardDeviation"] = Math.Round(pranaRequestCarrier.StandardDeviation);
                                AddComponentRiskDict(pranaRequestCarrier);
                                _dataPortfolio.Rows.Add(dr);
                                portfolioResultsCtrl1.Refresh();

                                if (string.IsNullOrEmpty(_firstLevelGroup))
                                    UpdateGridRows(pranaRequestCarrier);
                            }
                            else
                            {
                                DataRow dr = _dtCalculatedRisks.NewRow();
                                dr["Grouping Criterion"] = pranaRequestCarrier.GroupingName;
                                dr["Risk"] = Math.Round(pranaRequestCarrier.PortfolioRisk);
                                dr["Correlation"] = pranaRequestCarrier.Correlation;
                                dr["StandardDeviation"] = pranaRequestCarrier.StandardDeviation;
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
                    string symbol = riskReply.Key.Substring(0, symbolLen);
                    string newPositionType = string.Empty;
                    string oldPositionType = string.Empty;
                    if (_dictRiskSimulationObjs.ContainsKey(symbol))
                    {
                        List<PranaRiskSimulationObj> list = _dictRiskSimulationObjs[riskReply.Value.Symbol];
                        foreach (PranaRiskSimulationObj riskObjToUpdate in list)
                        {
                            if (riskObjToUpdate.IsChecked)
                            {
                                if (!pranaRequestCarrier.ForNew)
                                {
                                    if (riskObjToUpdate.OldQuantity >= 0)
                                    {
                                        oldPositionType = PositionType.Long.ToString();
                                    }
                                    else
                                    {
                                        oldPositionType = PositionType.Short.ToString();
                                    }

                                    if (riskReply.Value.Quantity != 0 && riskReply.Value.Quantity == riskObjToUpdate.OldQuantity && oldPositionType.Equals(positionType))
                                    {
                                        if (RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn == RiskConstants.RiskCalculationBasedOn.Quantity)
                                        {
                                            riskObjToUpdate.OldRisk = Math.Round((riskReply.Value.Risk * riskObjToUpdate.OldQuantity) / (riskReply.Value.Quantity));
                                            riskObjToUpdate.OldStandardDeviation = Math.Round((riskReply.Value.StandardDeviation * riskObjToUpdate.OldQuantity) / (riskReply.Value.Quantity));
                                            riskObjToUpdate.OldComponentRisk = Math.Round((riskReply.Value.ComponentRisk * riskObjToUpdate.OldQuantity) / (riskReply.Value.Quantity));
                                        }
                                        else if (RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn == RiskConstants.RiskCalculationBasedOn.DeltaAdjPosition)
                                        {
                                            riskObjToUpdate.OldRisk = Math.Round((riskReply.Value.Risk * riskObjToUpdate.OldDeltaAdjPosition) / (riskReply.Value.Quantity));
                                            riskObjToUpdate.OldStandardDeviation = Math.Round((riskReply.Value.StandardDeviation * riskObjToUpdate.OldDeltaAdjPosition) / (riskReply.Value.Quantity));
                                            riskObjToUpdate.OldComponentRisk = Math.Round((riskReply.Value.ComponentRisk * riskObjToUpdate.OldDeltaAdjPosition) / (riskReply.Value.Quantity));
                                        }
                                    }
                                }
                                else
                                {
                                    if (riskObjToUpdate.NewQuantity > 0)
                                    {
                                        newPositionType = PositionType.Long.ToString();
                                    }
                                    else
                                    {
                                        newPositionType = PositionType.Short.ToString();
                                    }

                                    if (riskReply.Value.Quantity != 0 && riskReply.Value.Quantity == riskObjToUpdate.NewQuantity && newPositionType.Equals(positionType))
                                    {
                                        if (RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn == RiskConstants.RiskCalculationBasedOn.Quantity)
                                        {
                                            riskObjToUpdate.NewRisk = Math.Round((riskReply.Value.Risk * riskObjToUpdate.NewQuantity) / (riskReply.Value.Quantity));
                                            riskObjToUpdate.NewStandardDeviation = Math.Round((riskReply.Value.StandardDeviation * riskObjToUpdate.NewQuantity) / (riskReply.Value.Quantity));
                                            riskObjToUpdate.NewComponentRisk = Math.Round((riskReply.Value.ComponentRisk * riskObjToUpdate.NewQuantity) / (riskReply.Value.Quantity));
                                        }
                                        else if (RiskPreferenceManager.RiskPrefernece.RiskCalculationBasedOn == RiskConstants.RiskCalculationBasedOn.DeltaAdjPosition)
                                        {
                                            riskObjToUpdate.NewRisk = Math.Round((riskReply.Value.Risk * riskObjToUpdate.NewDeltaAdjPosition) / (riskReply.Value.Quantity));
                                            riskObjToUpdate.NewStandardDeviation = Math.Round((riskReply.Value.StandardDeviation * riskObjToUpdate.NewDeltaAdjPosition) / (riskReply.Value.Quantity));
                                            riskObjToUpdate.NewComponentRisk = Math.Round((riskReply.Value.ComponentRisk * riskObjToUpdate.NewDeltaAdjPosition) / (riskReply.Value.Quantity));
                                        }
                                    }
                                }
                            }
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
            if (e.SummaryValue.SummarySettings.SourceColumn.Key == COL_OldRisk || e.SummaryValue.SummarySettings.SourceColumn.Key == COL_NewRisk || e.SummaryValue.SummarySettings.SourceColumn.Key == COL_OldStandardDeviation || e.SummaryValue.SummarySettings.SourceColumn.Key == COL_NewStandardDeviation)
            {
                DataRow dr;
                Infragistics.Win.UltraWinGrid.RowsCollection rows = e.SummaryValue.ParentRows;
                if (rows != null && rows.ParentRow != null && _dtCalculatedRisks.Columns.Contains("Grouping Criterion"))
                {
                    string groupByValue = ((Infragistics.Win.UltraWinGrid.UltraGridGroupByRow)(rows.ParentRow)).ValueAsDisplayText;

                    if (!string.IsNullOrEmpty(_secondLevelGroup) && rows.ParentRow.HasParent())
                    {
                        string parentGroupByValue = ((Infragistics.Win.UltraWinGrid.UltraGridGroupByRow)(rows.ParentRow.ParentRow)).ValueAsDisplayText;

                        if (e.SummaryValue.SummarySettings.SourceColumn.Key.ToUpper().Contains("OLD"))
                            dr = _dtCalculatedRisks.Rows.Find(parentGroupByValue + "|" + groupByValue + "$OLDGROUP");
                        else
                            dr = _dtCalculatedRisks.Rows.Find(parentGroupByValue + "|" + groupByValue + "$NEWGROUP");
                    }
                    else
                    {
                        if (e.SummaryValue.SummarySettings.SourceColumn.Key.ToUpper().Contains("OLD"))
                            dr = _dtCalculatedRisks.Rows.Find(_firstLevelGroup + "|" + groupByValue + "$OLDGROUP");
                        else
                            dr = _dtCalculatedRisks.Rows.Find(_firstLevelGroup + "|" + groupByValue + "$NEWGROUP");
                    }

                    if (dr != null && _dtCalculatedRisks.Columns.Contains(e.SummaryValue.SummarySettings.SourceColumn.Key.Replace("Old", "").Replace("New", "")))
                        e.SummaryValue.SetExternalSummaryValue(dr[e.SummaryValue.SummarySettings.SourceColumn.Key.Replace("Old", "").Replace("New", "")]);
                    else
                        e.SummaryValue.SetExternalSummaryValue(0);
                }
                else
                {
                    if (rows != null && rows.ParentRow == null && _dataPortfolio.Columns.Contains("Grouping Criterion"))
                    {
                        if (e.SummaryValue.SummarySettings.SourceColumn.Key.ToUpper().Contains("OLD"))
                            dr = _dataPortfolio.Rows.Find("Old Portfolio");
                        else
                            dr = _dataPortfolio.Rows.Find("New Portfolio");

                        if (dr != null && _dataPortfolio.Columns.Contains(e.SummaryValue.SummarySettings.SourceColumn.Key.Replace("Old", "").Replace("New", "")))
                            e.SummaryValue.SetExternalSummaryValue(dr[e.SummaryValue.SummarySettings.SourceColumn.Key.Replace("Old", "").Replace("New", "")]);
                        else
                            e.SummaryValue.SetExternalSummaryValue(0);
                    }
                    else
                    {
                        e.SummaryValue.SetExternalSummaryValue(0);
                    }
                }
            }
            else if (e.SummaryValue.SummarySettings.SourceColumn.Key == COL_OldComponentRisk || e.SummaryValue.SummarySettings.SourceColumn.Key == COL_NewComponentRisk)
            {
                Infragistics.Win.UltraWinGrid.RowsCollection rows = e.SummaryValue.ParentRows;
                if (rows != null && rows.ParentRow != null)
                {
                    string groupByValue = ((Infragistics.Win.UltraWinGrid.UltraGridGroupByRow)(rows.ParentRow)).ValueAsDisplayText;

                    if (!string.IsNullOrEmpty(_secondLevelGroup) && rows.ParentRow.HasParent())
                    {
                        string parentGroupByValue = ((Infragistics.Win.UltraWinGrid.UltraGridGroupByRow)(rows.ParentRow.ParentRow)).ValueAsDisplayText;

                        string final1Key = string.Empty;
                        string final2Key = string.Empty;
                        if (e.SummaryValue.SummarySettings.SourceColumn.Key == COL_NewComponentRisk)
                        {
                            final1Key = _firstLevelGroup + "|" + parentGroupByValue + "$NEWGROUP";
                            final2Key = parentGroupByValue + "|" + groupByValue + "$NEWGROUP";
                        }
                        else
                        {
                            final1Key = _firstLevelGroup + "|" + parentGroupByValue + "$OLDGROUP";
                            final2Key = parentGroupByValue + "|" + groupByValue + "$OLDGROUP";
                        }
                        if (_componentRiskByGrouping.ContainsKey(final2Key))
                        {
                            Dictionary<string, ComponentRiskData> symbolWiseComponentRisk = _componentRiskByGrouping[final2Key];
                            double componentRisk = 0;
                            foreach (string key in symbolWiseComponentRisk.Keys)
                            {
                                if (_componentRiskByGrouping[final1Key].ContainsKey(key))
                                {
                                    ComponentRiskData componentRiskData = _componentRiskByGrouping[final1Key][key];
                                    if (componentRiskData.Quantity != 0)
                                    {
                                        componentRisk += componentRiskData.ComponentRisk * symbolWiseComponentRisk[key].Quantity / componentRiskData.Quantity;
                                    }
                                }
                            }
                            e.SummaryValue.SetExternalSummaryValue(componentRisk);
                        }
                        else
                            e.SummaryValue.SetExternalSummaryValue(0);
                    }
                    else if (!string.IsNullOrEmpty(_firstLevelGroup))
                    {
                        string final1Key = string.Empty;
                        string final2Key = string.Empty;
                        if (e.SummaryValue.SummarySettings.SourceColumn.Key == COL_NewComponentRisk)
                        {
                            final1Key = "New Portfolio";
                            final2Key = _firstLevelGroup + "|" + groupByValue + "$NEWGROUP";
                        }
                        else
                        {
                            final1Key = "Old Portfolio";
                            final2Key = _firstLevelGroup + "|" + groupByValue + "$OLDGROUP";
                        }
                        if (_componentRiskByGrouping.ContainsKey(final2Key) && _componentRiskByGrouping.ContainsKey(final1Key))
                        {
                            Dictionary<string, ComponentRiskData> symbolWiseComponentRisk = _componentRiskByGrouping[final2Key];
                            double componentRisk = 0;
                            foreach (string key in symbolWiseComponentRisk.Keys)
                            {
                                if (_componentRiskByGrouping[final1Key].ContainsKey(key))
                                {
                                    ComponentRiskData componentRiskData = _componentRiskByGrouping[final1Key][key];
                                    if (componentRiskData.Quantity != 0)
                                    {
                                        componentRisk += componentRiskData.ComponentRisk * symbolWiseComponentRisk[key].Quantity / componentRiskData.Quantity;
                                    }
                                }
                            }
                            e.SummaryValue.SetExternalSummaryValue(componentRisk);
                        }
                        else
                            e.SummaryValue.SetExternalSummaryValue(0);
                    }
                    else
                        e.SummaryValue.SetExternalSummaryValue(0);
                }
                else
                {
                    string finalKey = string.Empty;
                    if (e.SummaryValue.SummarySettings.SourceColumn.Key == COL_NewComponentRisk)
                    {
                        finalKey = "New Portfolio";
                    }
                    else
                    {
                        finalKey = "Old Portfolio";
                    }
                    if (rows != null && rows.ParentRow == null && _componentRiskByGrouping.ContainsKey(finalKey))
                    {
                        Dictionary<string, ComponentRiskData> symbolWiseComponentRisk = _componentRiskByGrouping[finalKey];
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
                    MessageBox.Show("Please select a Date Range");
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

        private void btnAddPosition_Click(object sender, EventArgs e)
        {
            try
            {
                if (_secMasterObjEnteredSymbol != null)
                {
                    if (String.IsNullOrEmpty(pranaSymbolCtrl1.Text.ToString()) || !RegularExpressionValidation.IsNumber(textBoxQuantity.Text.ToString()))
                    {
                        MessageBox.Show("Input Symbol and Quantity.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ClearData();

                        #region PS Symbol Request
                        #region PS Symbol
                        List<PSSymbolRequestObject> psReqobj = new List<PSSymbolRequestObject>();
                        PSSymbolRequestObject obj = new PSSymbolRequestObject();
                        obj.AssetID = _secMasterObjEnteredSymbol.AssetID;
                        obj.Symbol = _secMasterObjEnteredSymbol.TickerSymbol;
                        obj.UnderlyingSymbol = _secMasterObjEnteredSymbol.UnderLyingSymbol;
                        obj.AUECID = _secMasterObjEnteredSymbol.AUECID;
                        obj.ExchangeID = _secMasterObjEnteredSymbol.ExchangeID;

                        int baseAssetID = Mapper.GetBaseAsset(_secMasterObjEnteredSymbol.AssetID);
                        if (baseAssetID == (int)AssetCategory.Option)
                        {
                            SecMasterOptObj optionObj = _secMasterObjEnteredSymbol as SecMasterOptObj;
                            obj.ExpirationDate = optionObj.ExpirationDate;
                            obj.StrikePrice = optionObj.StrikePrice;
                            int putOrCall = optionObj.PutOrCall;
                            if (putOrCall == (int)OptionType.CALL)
                            {
                                obj.PutOrCall = Convert.ToString('C');
                            }
                            else if (putOrCall == (int)OptionType.PUT)
                            {
                                obj.PutOrCall = Convert.ToString('P');
                            }
                        }
                        if (baseAssetID == (int)AssetCategory.Future)
                        {
                            SecMasterFutObj futureObj = _secMasterObjEnteredSymbol as SecMasterFutObj;
                            obj.ExpirationDate = futureObj.ExpirationDate;
                        }
                        SymbolData symbolData = CentralRiskPositionsManager.GetInstance.GetDynamicSymbolData(_secMasterObjEnteredSymbol.TickerSymbol);
                        if (symbolData != null)
                        {
                            obj.Volatility = symbolData.FinalImpliedVol;
                        }
                        psReqobj.Add(obj);
                        #endregion

                        #region Underlying PS Symbol
                        SymbolData underlyingSymbolData = CentralRiskPositionsManager.GetInstance.GetDynamicSymbolData(_secMasterObjEnteredSymbol.UnderLyingSymbol);
                        PSSymbolRequestObject objUnderlying = new PSSymbolRequestObject();
                        if (underlyingSymbolData != null)
                        {
                            objUnderlying.Symbol = underlyingSymbolData.Symbol;
                            objUnderlying.UnderlyingSymbol = underlyingSymbolData.UnderlyingSymbol;
                            objUnderlying.AUECID = underlyingSymbolData.AUECID;
                            objUnderlying.ExchangeID = underlyingSymbolData.ExchangeID;
                            objUnderlying.Volatility = underlyingSymbolData.FinalImpliedVol;

                            if (_secMasterObjEnteredSymbol.AssetID.Equals((int)AssetCategory.EquityOption))
                            {
                                objUnderlying.AssetID = (int)AssetCategory.Equity;
                            }
                            else if (_secMasterObjEnteredSymbol.AssetID.Equals((int)AssetCategory.FutureOption))
                            {
                                objUnderlying.AssetID = (int)AssetCategory.Future;
                                objUnderlying.ExpirationDate = underlyingSymbolData.ExpirationDate;
                            }
                            else if (_secMasterObjEnteredSymbol.AssetID.Equals((int)AssetCategory.FXOption))
                            {
                                objUnderlying.AssetID = (int)AssetCategory.FX;
                            }
                        }
                        psReqobj.Add(objUnderlying);
                        #endregion

                        Dictionary<string, string> dictPSSymbols = CentralRiskPositionsManager.GetInstance.GetPSSymbols(psReqobj);
                        #endregion

                        PranaRiskObj riskObj = new PranaRiskObj();
                        riskObj.Symbol = pranaSymbolCtrl1.Text.ToString();
                        riskObj.Quantity = Convert.ToDouble(textBoxQuantity.Text.ToString());
                        if (dictPSSymbols.ContainsKey(riskObj.Symbol))
                        {
                            riskObj.PSSymbol = dictPSSymbols[riskObj.Symbol];
                        }
                        riskObj.UnderlyingSymbol = _secMasterObjEnteredSymbol.UnderLyingSymbol;
                        if (dictPSSymbols.ContainsKey(riskObj.UnderlyingSymbol))
                        {
                            riskObj.UnderlyingPSSymbol = dictPSSymbols[riskObj.UnderlyingSymbol];
                        }
                        riskObj.CompanyName = _secMasterObjEnteredSymbol.LongName;
                        riskObj.ContractMultiplier = _secMasterObjEnteredSymbol.Multiplier;
                        riskObj.SideMultiplier = (riskObj.Quantity >= 0) ? 1 : -1;
                        riskObj.AssetID = _secMasterObjEnteredSymbol.AssetID;
                        riskObj.BloombergSymbol = _secMasterObjEnteredSymbol.BloombergSymbol;
                        riskObj.BloombergSymbolWithExchangeCode = _secMasterObjEnteredSymbol.BloombergSymbolWithExchangeCode;
                        riskObj.FactSetSymbol = _secMasterObjEnteredSymbol.FactSetSymbol;
                        riskObj.ActivSymbol = _secMasterObjEnteredSymbol.ActivSymbol;
                        riskObj.SEDOLSymbol = _secMasterObjEnteredSymbol.SedolSymbol;
                        riskObj.ISINSymbol = _secMasterObjEnteredSymbol.ISINSymbol;
                        riskObj.IDCOSymbol = _secMasterObjEnteredSymbol.IDCOOptionSymbol;
                        riskObj.OSISymbol = _secMasterObjEnteredSymbol.OSIOptionSymbol;
                        riskObj.CusipSymbol = _secMasterObjEnteredSymbol.CusipSymbol;


                        riskObj.AssetName = CommonDataCache.CachedDataManager.GetInstance.GetAssetText(riskObj.AssetID);
                        #region Delta & DeltaAdjPosition
                        if (symbolData != null)
                        {
                            riskObj.Delta = symbolData.Delta;
                            riskObj.DeltaAdjPosition = OptionGreekCalculater.CalculateNetDeltaAdjPosition(riskObj.Quantity, riskObj.Delta, riskObj.AssetID, riskObj.SideMultiplier, riskObj.ContractMultiplier);
                        }
                        #endregion
                        _riskObjcollection.Add(riskObj);

                        PranaRiskSimulationObj risksimulation = CreateSimuObj(riskObj);
                        risksimulation.OldQuantity = 0;
                        risksimulation.OldDeltaAdjPosition = 0;
                        AddPositionToUIList(risksimulation);
                        AddPositionToDict(riskObj);
                        grdData.DataBind();
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

        private PranaRiskSimulationObj CreateSimuObj(PranaRiskObj riskObj)
        {
            PranaRiskSimulationObj risksimulation = new PranaRiskSimulationObj();
            try
            {
                risksimulation.IsChecked = riskObj.IsChecked;
                risksimulation.Symbol = riskObj.Symbol;
                risksimulation.UnderlyingSymbol = riskObj.UnderlyingSymbol;
                risksimulation.NewQuantity = riskObj.Quantity;
                risksimulation.OldQuantity = riskObj.Quantity;
                risksimulation.PSSymbol = riskObj.PSSymbol;
                risksimulation.UnderlyingPSSymbol = riskObj.UnderlyingPSSymbol;
                risksimulation.SecurityName = riskObj.CompanyName;
                if (riskObj.Level1Name == string.Empty)
                {
                    risksimulation.Account = "N/A";
                }
                else
                {
                    risksimulation.Account = riskObj.Level1Name;
                }

                if (riskObj.MasterFund == string.Empty)
                {
                    risksimulation.MasterFund = "N/A";
                }
                else
                {
                    risksimulation.MasterFund = riskObj.MasterFund;
                }
                risksimulation.Strategy = riskObj.Level2Name;
                risksimulation.CountryName = riskObj.CountryName;
                risksimulation.SectorName = riskObj.SectorName;
                risksimulation.UDAAsset = riskObj.UDAAsset;
                risksimulation.SecurityTypeName = riskObj.SecurityTypeName;
                risksimulation.SideMultiplier = riskObj.SideMultiplier;

                risksimulation.ContractMultiplier = riskObj.ContractMultiplier;
                risksimulation.PSSymbol = riskObj.PSSymbol;

                risksimulation.TradeAttribute1 = riskObj.TradeAttribute1;
                risksimulation.TradeAttribute2 = riskObj.TradeAttribute2;
                risksimulation.TradeAttribute3 = riskObj.TradeAttribute3;
                risksimulation.TradeAttribute4 = riskObj.TradeAttribute4;
                risksimulation.TradeAttribute5 = riskObj.TradeAttribute5;
                risksimulation.TradeAttribute6 = riskObj.TradeAttribute6;

                risksimulation.BloombergSymbol = riskObj.BloombergSymbol;
                risksimulation.BloombergSymbolWithExchangeCode = riskObj.BloombergSymbolWithExchangeCode;
                risksimulation.ActivSymbol = riskObj.ActivSymbol;
                risksimulation.FactSetSymbol = riskObj.FactSetSymbol;
                risksimulation.IDCOSymbol = riskObj.IDCOSymbol;
                risksimulation.ISINSymbol = riskObj.ISINSymbol;
                risksimulation.SedolSymbol = riskObj.SEDOLSymbol;
                risksimulation.OSISymbol = riskObj.OSISymbol;
                risksimulation.CusipSymbol = riskObj.CusipSymbol;

                risksimulation.Delta = riskObj.Delta;
                risksimulation.OldDeltaAdjPosition = riskObj.DeltaAdjPosition;
                risksimulation.NewDeltaAdjPosition = riskObj.DeltaAdjPosition;

                risksimulation.AssetID = riskObj.AssetID;
                risksimulation.AssetName = riskObj.AssetName;
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
            return risksimulation;
        }

        private PranaRiskObj CreateRiskObjWithOldQty(PranaRiskSimulationObj risksimulation)
        {
            PranaRiskObj riskObj = new PranaRiskObj();
            try
            {
                riskObj.Symbol = risksimulation.Symbol;
                riskObj.Quantity = risksimulation.OldQuantity;
                riskObj.PSSymbol = risksimulation.PSSymbol;
                riskObj.UnderlyingPSSymbol = risksimulation.UnderlyingPSSymbol;
                riskObj.SideMultiplier = risksimulation.SideMultiplier;
                riskObj.ContractMultiplier = risksimulation.ContractMultiplier;

                riskObj.TradeAttribute1 = risksimulation.TradeAttribute1;
                riskObj.TradeAttribute2 = risksimulation.TradeAttribute2;
                riskObj.TradeAttribute3 = risksimulation.TradeAttribute3;
                riskObj.TradeAttribute4 = risksimulation.TradeAttribute4;
                riskObj.TradeAttribute5 = risksimulation.TradeAttribute5;
                riskObj.TradeAttribute6 = risksimulation.TradeAttribute6;

                riskObj.BloombergSymbol = risksimulation.BloombergSymbol;
                riskObj.BloombergSymbolWithExchangeCode = risksimulation.BloombergSymbolWithExchangeCode;
                riskObj.FactSetSymbol = risksimulation.FactSetSymbol;
                riskObj.ActivSymbol = risksimulation.ActivSymbol;
                riskObj.IDCOSymbol = risksimulation.IDCOSymbol;
                riskObj.ISINSymbol = risksimulation.ISINSymbol;
                riskObj.SEDOLSymbol = risksimulation.SedolSymbol;
                riskObj.OSISymbol = risksimulation.OSISymbol;
                riskObj.CusipSymbol = risksimulation.CusipSymbol;

                riskObj.Delta = risksimulation.Delta;
                riskObj.DeltaAdjPosition = risksimulation.OldDeltaAdjPosition;

                riskObj.AssetID = risksimulation.AssetID;
                riskObj.AssetName = risksimulation.AssetName;
                riskObj.UDAAsset = risksimulation.UDAAsset;

                if (riskObj.Quantity > 0)
                    riskObj.PositionType = PositionType.Long.ToString();
                else
                    riskObj.PositionType = PositionType.Short.ToString();
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
            return riskObj;
        }

        private PranaRiskObj CreateRiskObjWithNewQty(PranaRiskSimulationObj risksimulation)
        {
            PranaRiskObj riskObj = new PranaRiskObj();
            try
            {
                riskObj.Symbol = risksimulation.Symbol;
                riskObj.Quantity = risksimulation.NewQuantity;
                riskObj.PSSymbol = risksimulation.PSSymbol;
                riskObj.UnderlyingPSSymbol = risksimulation.UnderlyingPSSymbol;
                riskObj.SideMultiplier = risksimulation.SideMultiplier;
                riskObj.ContractMultiplier = risksimulation.ContractMultiplier;

                riskObj.TradeAttribute1 = risksimulation.TradeAttribute1;
                riskObj.TradeAttribute2 = risksimulation.TradeAttribute2;
                riskObj.TradeAttribute3 = risksimulation.TradeAttribute3;
                riskObj.TradeAttribute4 = risksimulation.TradeAttribute4;
                riskObj.TradeAttribute5 = risksimulation.TradeAttribute5;
                riskObj.TradeAttribute6 = risksimulation.TradeAttribute6;

                riskObj.BloombergSymbol = risksimulation.BloombergSymbol;
                riskObj.BloombergSymbolWithExchangeCode = risksimulation.BloombergSymbolWithExchangeCode;
                riskObj.FactSetSymbol = risksimulation.FactSetSymbol;
                riskObj.ActivSymbol = riskObj.ActivSymbol;
                riskObj.IDCOSymbol = risksimulation.IDCOSymbol;
                riskObj.ISINSymbol = risksimulation.ISINSymbol;
                riskObj.SEDOLSymbol = risksimulation.SedolSymbol;
                riskObj.OSISymbol = risksimulation.OSISymbol;
                riskObj.CusipSymbol = risksimulation.CusipSymbol;

                riskObj.Delta = risksimulation.Delta;
                riskObj.DeltaAdjPosition = risksimulation.NewDeltaAdjPosition;

                riskObj.AssetID = risksimulation.AssetID;
                riskObj.AssetName = risksimulation.AssetName;
                riskObj.UDAAsset = risksimulation.UDAAsset;

                if (riskObj.Quantity > 0)
                    riskObj.PositionType = PositionType.Long.ToString();
                else
                    riskObj.PositionType = PositionType.Short.ToString();
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
            return riskObj;
        }

        private void grdData_AfterRowFilterChanged(object sender, AfterRowFilterChangedEventArgs e)
        {
            try
            {
                _dataPortfolio.Rows.Clear();
                portfolioResultsCtrl1.Refresh();
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

        private void DisableForm()
        {
            try
            {
                btnSimulation.Enabled = false;
                cmbbxBenchMark.Enabled = false;
                datePickerStartdate.Enabled = false;
                datePickerEnddate.Enabled = false;
                textBoxQuantity.Enabled = false;
                pranaSymbolCtrl1.Enabled = false;
                btnAddPosition.Enabled = false;

                //When we disable Grid then Summary of summary rows got cleared so we are not disabling grid (disabling only editable columns)
                //grdData.Enabled = false;
                grdData.DisplayLayout.Bands[0].Columns[COL_IsChecked].CellActivation = Activation.Disabled;
                grdData.DisplayLayout.Bands[0].Columns[COL_PercentageChange].CellActivation = Activation.ActivateOnly;
                grdData.DisplayLayout.Bands[0].Columns[COL_NewQuantity].CellActivation = Activation.ActivateOnly;
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

                btnSimulation.BackColor = Color.FromArgb(104, 156, 46);
                btnSimulation.Text = "Simulation";
                btnSimulation.Enabled = true;
                cmbbxBenchMark.Enabled = true;
                datePickerStartdate.Enabled = true;
                datePickerEnddate.Enabled = true;
                textBoxQuantity.Enabled = true;
                pranaSymbolCtrl1.Enabled = true;
                btnAddPosition.Enabled = true;
                grdData.DisplayLayout.Bands[0].Columns[COL_IsChecked].CellActivation = Activation.AllowEdit;
                grdData.DisplayLayout.Bands[0].Columns[COL_PercentageChange].CellActivation = Activation.AllowEdit;
                grdData.DisplayLayout.Bands[0].Columns[COL_NewQuantity].CellActivation = Activation.AllowEdit;
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

        private void saveColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RiskPrefernece riskPrefs = RiskPreferenceManager.RiskPrefernece;
                riskPrefs.RiskSimulationColumns = UltraWinGridUtils.GetColumnsString(grdData);
                riskPrefs.CorrelationSymbolName = cmbbxBenchMark.Value.ToString();
                RiskPreferenceManager.SavePreferences(riskPrefs);

                RiskLayoutManager.RiskLayout.RiskSimulationColumns = RiskLayoutManager.GetGridColumnLayout(grdData);
                RiskLayoutManager.RiskLayout.RiskSimulationGroupByColumnsCollection = RiskLayoutManager.GetGridGroupByColumnLayout(grdData);
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
                                PranaRiskSimulationObj risksimulation = CreateSimuObj(riskObj);
                                AddPositionToUIList(risksimulation);
                                AddPositionToDict(riskObj);
                            }
                            BindGrid();
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
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void pranaSymbolCtrl1_SymbolEntered(object sender, EventArgs<string> e)
        {
            try
            {
                SecMasterRequestObj reqObj = new SecMasterRequestObj();
                reqObj.AddData(e.Value, ApplicationConstants.SymbologyCodes.TickerSymbol);
                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                reqObj.HashCode = this.GetHashCode();
                _secMasterClient.SendRequest(reqObj);
                btnAddPosition.Enabled = false;
                btnAddPosition.BackColor = Color.Red;
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
                Font oldFont = grdData.Font;
                Font newFont = new Font(oldFont.FontFamily, fontSize, oldFont.Style, oldFont.Unit, oldFont.GdiCharSet, oldFont.GdiVerticalFont);
                grdData.Font = newFont;
                grdData.DisplayLayout.Override.SummaryValueAppearance.ForeColor = Color.FromArgb(RiskPreferenceManager.RiskPrefernece.ColorSummaryText);
                portfolioResultsCtrl1.SetGridFonts(newFont);
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
                    if (RiskLayoutManager.RiskLayout.RiskSimulationColumns.Count > 0)
                    {
                        List<ColumnData> listColData = RiskLayoutManager.RiskLayout.RiskSimulationColumns;
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

        private void grdData_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == COL_PercentageChange)
                {
                    PranaRiskSimulationObj pranaRiskSimuObj = (PranaRiskSimulationObj)e.Cell.Row.ListObject;
                    pranaRiskSimuObj.NewQuantity = pranaRiskSimuObj.OldQuantity * (1 + (double.Parse(e.Cell.Text.Replace("_", "").ToString())) / 100);
                    e.Cell.Row.Refresh();
                }
                else if (e.Cell.Column.Key == COL_NewQuantity)
                {
                    PranaRiskSimulationObj pranaRiskSimuObj = (PranaRiskSimulationObj)e.Cell.Row.ListObject;
                    double newQty = double.Parse(e.Cell.Text.ToString());
                    pranaRiskSimuObj.PercentageChange = Convert.ToSingle(((newQty - pranaRiskSimuObj.OldQuantity) * 100) / pranaRiskSimuObj.OldQuantity);

                    //Bharat Kumar Jangir (28 August 2014)
                    //Update New DeltaAdjPosition when new quantity changes
                    pranaRiskSimuObj.NewDeltaAdjPosition = OptionGreekCalculater.CalculateNetDeltaAdjPosition(newQty, pranaRiskSimuObj.Delta, pranaRiskSimuObj.AssetID, (newQty >= 0) ? 1 : -1, pranaRiskSimuObj.ContractMultiplier);

                    e.Cell.Row.Refresh();
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

        private void CtrlRiskSimulation_Load(object sender, EventArgs e)
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
                    this.grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.Bottom;
                    this.grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.InGroupByRows;
                    this.grdData.DisplayLayout.Override.SummaryDisplayArea |= SummaryDisplayAreas.RootRowsFootersOnly;
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
                grid.DataSource = _riskObjSimulationcollection;
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
                StringBuilder sbRiskLowerGridValuesFirstRow = new StringBuilder();
                UltraGridLayout gridDisplayLayout = portfolioResultsCtrl1.grdData.DisplayLayout;
                foreach (UltraGridColumn col in gridDisplayLayout.Bands[0].Columns)
                {
                    if (rowCount == 0)
                    {
                        sbRiskLowerGridValuesFirstRow.Append(col.Key + "=" + "0" + Seperators.SEPERATOR_5 + "0" + Seperators.SEPERATOR_6);
                    }
                    else
                    {
                        sbRiskLowerGridValuesFirstRow.Append(col.Key + "=" + gridDisplayLayout.Rows[0].Cells[col.Key].Value + Seperators.SEPERATOR_5 + gridDisplayLayout.Rows[1].Cells[col.Key].Value + Seperators.SEPERATOR_6);
                    }
                }
                sbRiskLowerGridValuesFirstRow.Remove(sbRiskLowerGridValuesFirstRow.Length - 1, 1);
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

                riskReportDetails.Add(tabRiskSimulation, sbRiskLowerGridValuesFirstRow.ToString());
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
                gridDict.Add(tabRiskSimulation, grid);
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
                    if (_secMasterClient != null)
                    {
                        _secMasterClient.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(_secMasterClient_SecMstrDataResponse);
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
                        grdData.Dispose();

                    if (portfolioResultsCtrl1 != null)
                        portfolioResultsCtrl1 = null;

                    if (_columnSorted != null)
                        _columnSorted.Dispose();

                    if (_dataPortfolio != null)
                        _dataPortfolio.Dispose();

                    if (_dtCalculatedRisks != null)
                        _dtCalculatedRisks.Dispose();
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
