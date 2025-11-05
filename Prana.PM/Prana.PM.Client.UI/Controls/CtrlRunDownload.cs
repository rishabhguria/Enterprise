using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.AlgoStrategyControls;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CashManagement;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Import;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.PM.DAL;
using Prana.Utilities;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities.XMLUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Prana.PM.Client.UI
{
    public partial class CtrlRunDownload : UserControl
    {

        private RunUploadList _runUploadList = new RunUploadList();
        private ClientCommon.ImportHelper _importHelper = new ClientCommon.ImportHelper();

        UltraGridBand _gridBandRunUpload = null;

        List<string> _currencyListForAlloScheme = null;
        List<TradeAuditEntry> _auditCollection_MarkPrice = new List<TradeAuditEntry>();
        List<TradeAuditEntry> _auditCollection_FXRate = new List<TradeAuditEntry>();

        //private RunUploadList _successfullUploads = new RunUploadList();

        #region Cash Management Proxy Section

        static ProxyBase<ICashManagementService> _CashManagementServices = null;
        public static ProxyBase<ICashManagementService> CashManagementServices
        {
            set
            {
                _CashManagementServices = value;

            }
            get { return _CashManagementServices; }
        }

        public static void CreateCashManagementProxy()
        {
            CashManagementServices = new ProxyBase<ICashManagementService>("TradeCashServiceEndpointAddress");
        }

        #endregion

        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        public DuplexProxyBase<IPricingService> PricingServices
        {
            set
            {
                _pricingServicesProxy = value;

            }
        }

        /// <summary>
        /// This will be set when server will save the groups chunk through Import.
        /// </summary>
        private bool _isGroupSaved = false;
        public bool IsGroupSaved
        {
            get { return _isGroupSaved; }
            set { _isGroupSaved = value; }
        }

        private bool _isImportRunning = false;
        public bool IsImportRunning
        {
            get { return _isImportRunning; }
            set { _isImportRunning = value; }
        }

        #region Grid Column Names

        const string CAPTION_IsSelected = "Select";
        const string CAPTION_CompanyUploadSetupID = "CompanyUploadSetupID";
        const string CAPTION_CompanyNameID = "CompanyNameIDValue";
        const string CAPTION_CompanyID = "CompanyID";
        const string CAPTION_DataSourceNameID = "DataSourceNameIDValue";
        const string CAPTION_ThirdPartyID = "ThirdPartyID";
        const string CAPTION_EnableAutoTime = "EnableAutoTime";
        const string CAPTION_AutoTime = "AutoTime";
        const string CAPTION_Status = "Status";
        const string CAPTION_DirPath = "DirPath";
        const string CAPTION_FileName = "FileName";
        const string CAPTION_NumberOfRecords = "TotalRecords";
        const string CAPTION_ErrorsButton = "DetailsButton";
        const string CAPTION_ExceptionsButton = "ManualEntryButton";

        const string CAPTION_FTPServer = "FTPServer";
        const string CAPTION_Port = "Port";
        const string CAPTION_UserName = "UserName";
        const string CAPTION_Password = "Password";
        const string CAPTION_LastRunUploadDate = "LastRunUploadDate";
        const string CAPTION_StatusProgress = "Progress";
        const string CAPTION_TableTypeID = "TableTypeID";
        const string CAPTION_TableFormatName = "TableFormatName";
        const string CAPTION_FilePath = "FilePath";
        const string CAPTION_FileSelectButton = "SelectButton";
        const string CAPTION_XSLTFilePath = "XSLTFile";
        const string CAPTION_XSLTSelectButton = "XSLTSelectButton";
        const string CAPTION_Account = "Account";
        const string CAPTION_Date = "Date";
        const string CAPTION_IsDateSelected = "DateSelect";
        const string CAPTION_True = "TRUE";
        const string CAPTION_False = "FALSE";
        const string DATEFORMAT = "MM/dd/yyyy";
        const string CAPTION_DataSourceXSLT = "DataSourceXSLT";

        const string _importTypeCash = "Cash";
        const string _importTypeNetPosition = "Net Position";
        const string _importTypeStagedOrder = "Staged Order";
        const string _importTypeTransaction = "Transaction";
        const string _importTypeMarkPrice = "Mark Price";
        const string _importTypeForexPrice = "Forex Price";
        const string _importTypeActivity = "Activities";
        const string _importTypeDailyBeta = "DailyBeta";
        const string _importTypeDailyCreditLimit = "Credit Limit";
        const string _importTypeSecMasterInsert = "SMInsert";
        const string _importTypeSecMasterUpdate = "SMUpdate";
        const string _importTypeOMI = "Option Model Inputs";
        const string _tradeServerConnected = "Trade Engine Connected";
        const string _tradeServerDISConnected = "Trade Engine Disconnected";
        const string _importTypeAllocationScheme = "Allocation Scheme";
        const string _importTypeAllocationScheme_AppPositions = "Allocation Scheme AppPositions";
        const string _importTypeDoubleEntryCash = "Double Entry Cash";
        const string _importTypeSettlementDateCash = "SettlementDate Cash";
        private const string _importTypeDailyVolatility = "Daily Volatility";
        private const string _importTypeDailyVWAP = "Daily VWAP";
        private const string _importTypeCollateralPrice = "Collateral Price";
        private const string _importTypeDailyDividendYield = "Daily Dividend Yield";
        private const string _importTypeCollateralInterest = "Collateral Interest";
        private const string _importTypeMultilegJournalImport = "Multileg Journal Import";
        const string _dataSuccessfullySave = "Data Successfully Saved";
        const int MAXCHUNKSIZE = 1000;

        private CompanyUser _companyUser = null;
        private string _startupPath = Application.StartupPath;
        int _pMTradingAccountID = int.MinValue;
        string _SMRequest = string.Empty;
        Dictionary<int, Dictionary<string, List<DataRow>>> _allocationSchemeSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<DataRow>>>();

        #endregion Grid Column Names

        //this form object which shows the trades feched from file.
        ImportPositionsDisplayForm _displayForm = null;

        private DataTable InsertDataintoCollateralInterest(DataSet ds)
        {
            DateTime date;
            DataTable dt = ds.Tables[0];
            dt.Columns.Add("Date", typeof(string)).SetOrdinal(0);
            date = DateTime.UtcNow;

            foreach (DataRow row in dt.Rows)
            {
                row["Date"] = Convert.ToString(date);
            }
            return dt;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlRunUpload"/> class.
        /// </summary>
        public CtrlRunDownload()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Proxy for Dynamic UDA
        /// </summary>
        ProxyBase<ISecMasterSyncServices> _secMasterSyncService = null;
        public ProxyBase<ISecMasterSyncServices> SecMasterSyncService
        {
            set { _secMasterSyncService = value; }
        }
        /// <summary>
        /// Create Proxy for Dynamic UDA
        /// </summary>
        private void CreateSMSyncPoxy()
        {
            try
            {
                _secMasterSyncService = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
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
        /// <summary>
        /// Populates the run upload details.
        /// </summary>
        /// <param name="selectedTreeNodeID">The selected tree node ID.</param>
        public void PopulateRunUploadDetails(CompanyUser companyUser, ISecurityMasterServices secMaster)//(CompanyNameID uploadClientID)
        {
            try
            {
                _securityMaster = secMaster;
                _companyUser = companyUser;
                int pMCompanyID = RunUploadManager.GetPMCompanyID(_companyUser.CompanyID);

                _runUploadList = RunUploadManager.GetRunUploadDataByCompanyID(pMCompanyID);
                //FileAndDbSyncManager.SyncFileWithDataBase(_startupPath, ApplicationConstants.MappingFileType.PMImportXSLT);
                // for XSD
                FileAndDbSyncManager.SyncFileWithDataBase(_startupPath, ApplicationConstants.MappingFileType.PranaXSD);
                // for XSLT
                RunUploadManager.SavePMImportXSLTfromDB(_startupPath);

                gridRunUpload.DataSource = null;
                gridRunUpload.DataSource = _runUploadList;
                BindGridComboBoxes();
                foreach (Prana.BusinessObjects.TradingAccount tradingAccount in _companyUser.TradingAccounts)
                {
                    _pMTradingAccountID = tradingAccount.TradingAccountID;
                    break;
                }
                _securityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(_securityMaster_SecMstrDataResponse);
                _securityMaster.Disconnected += new EventHandler(_securityMaster_Disconnected);
                _securityMaster.Connected += new EventHandler(_securityMaster_Connected);
                foreach (UltraGridRow row in gridRunUpload.Rows)
                {
                    row.Cells[CAPTION_Date].Activation = Activation.Disabled;
                    row.Cells[CAPTION_Date].Value = DateTime.UtcNow;
                    row.Cells[CAPTION_Account].Value = int.MinValue;
                }
                if (_securityMaster.IsConnected)
                {
                    toolStripStatusLabel1.Text = _tradeServerConnected;
                    toolStripStatusLabel1.ForeColor = Color.Green;
                }
                else
                {
                    toolStripStatusLabel1.Text = _tradeServerDISConnected;
                    toolStripStatusLabel1.ForeColor = Color.Red;
                }
                _currencyListForAlloScheme = _importHelper.AllocationServices.InnerChannel.GetCurrencyListForAllocationScheme();
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

        internal void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                if (secMasterObj.AssetID == (int)AssetCategory.FX || secMasterObj.AssetID == (int)AssetCategory.FXForward)
                {
                    if (FXandFXFWDSymbolGenerator.IsValidFxAndFwdSymbol(secMasterObj))
                        ValidateAndMarshal(sender, e);
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
                SecMasterBaseObj secMasterObj = e.Value;
                if ((_SMRequest.Equals("AllocationScheme") || _SMRequest.Equals("AllocationScheme_AppPositions")) &&
                    _schemeForm != null)
                {
                    // AllocationSchemeForm displayForm = AllocationSchemeForm.GetInstance();
                    if (UIValidation.GetInstance().validate(_schemeForm))
                    {
                        if (_schemeForm.InvokeRequired)
                        {
                            SecMasterObjHandler secMasterObjHandler = new SecMasterObjHandler(FillSecurityMasterDataFromObj);
                            if (_schemeForm != null && !_schemeForm.Disposing && !_schemeForm.IsDisposed)
                                _schemeForm.Invoke(secMasterObjHandler, new object[] { sender, e });

                            EventHandler<EventArgs<string>> genericEventHandler = new EventHandler<EventArgs<string>>(_schemeForm.RefreshGridGroup);
                            //MethodInvoker mi = new MethodInvoker(displayForm.RefreshGridGroup);
                            if (_schemeForm != null && !_schemeForm.Disposing && !_schemeForm.IsDisposed)
                                _schemeForm.Invoke(genericEventHandler, new object[] { sender, new EventArgs<string>(secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString()) });
                        }
                        else
                        {
                            FillSecurityMasterDataFromObj(sender, new EventArgs<SecMasterBaseObj>(secMasterObj));
                            _schemeForm.RefreshGridGroup(this, new EventArgs<string>(secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString()));
                        }
                    }
                }
                else
                {
                    if (UIValidation.GetInstance().validate(_displayForm))
                    {
                        if (_displayForm.InvokeRequired)
                        {
                            SecMasterObjHandler secMasterObjHandler = new SecMasterObjHandler(FillSecurityMasterDataFromObj);
                            if (_displayForm != null && !_displayForm.Disposing && !_displayForm.IsDisposed)
                                _displayForm.Invoke(secMasterObjHandler, new object[] { sender, e });

                            EventHandler<EventArgs<string>> stringHandler = new EventHandler<EventArgs<string>>(_displayForm.RefreshGridGroup);
                            //MethodInvoker mi = new MethodInvoker(displayForm.RefreshGridGroup);
                            if (_displayForm != null && !_displayForm.Disposing && !_displayForm.IsDisposed)
                                _displayForm.Invoke(stringHandler, new object[] { sender, new EventArgs<string>(secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString()) });
                        }
                        else
                        {
                            FillSecurityMasterDataFromObj(sender, new EventArgs<SecMasterBaseObj>(secMasterObj));
                            _displayForm.RefreshGridGroup(this, new EventArgs<string>(secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString()));
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

        public delegate void ConnectionInvokeDelegate(object sender, EventArgs e);
        void _securityMaster_Connected(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        ConnectionInvokeDelegate connectionStatusDelegate = new ConnectionInvokeDelegate(_securityMaster_Connected);
                        this.BeginInvoke(connectionStatusDelegate, new object[] { sender, e });
                    }
                    else
                    {
                        SetGetButtonDetails();
                        toolStripStatusLabel1.Text = _tradeServerConnected;
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

        void _securityMaster_Disconnected(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        ConnectionInvokeDelegate connectionStatusDelegate = new ConnectionInvokeDelegate(_securityMaster_Disconnected);
                        this.BeginInvoke(connectionStatusDelegate, new object[] { sender, e });
                    }
                    else
                    {
                        SetGetButtonDetails();
                        toolStripStatusLabel1.Text = _tradeServerDISConnected;
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

        private void SetGetButtonDetails()
        {
            if (toolStripStatusLabel1.Text.Equals(_tradeServerConnected))
            {
                toolStripStatusLabel1.ForeColor = Color.Black;
            }
            else
            {
                toolStripStatusLabel1.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Binds the grid combo boxes.
        /// </summary>
        private void BindGridComboBoxes()
        {
            try
            {
                cmbDataSources.DataSource = null;
                cmbDataSources.DataSource = Prana.PM.DAL.DataSourceManager.RetrieveDataSourceNames(true, false);// DataSourceNameIDList.GetInstance().Retrieve;
                cmbDataSources.DisplayMember = "ShortName";
                cmbDataSources.ValueMember = "ID";
                Utils.UltraDropDownFilter(cmbDataSources, "ShortName");

                cmbCompanies.DataSource = null;
                cmbCompanies.DataSource = Prana.PM.DAL.CompanyManager.GetCompanyNameIDListWithSelect(); // CompanyNameIDList.Retrieve();
                cmbCompanies.DisplayMember = "ShortName";
                cmbCompanies.ValueMember = "ID";
                Utils.UltraDropDownFilter(cmbCompanies, "ShortName");

                TableTypeList tableTypeList = DataSourceManager.GetTableTypes(false);
                cmbTableTypes.DataSource = null;
                cmbTableTypes.DataSource = tableTypeList;
                cmbTableTypes.DisplayMember = "TableTypeName";
                cmbTableTypes.ValueMember = "TableTypeID";
                Utils.UltraDropDownFilter(cmbTableTypes, "TableTypeName");
                cmbTableTypes.Text = "Transaction";

                //cmbStatus.DisplayMember = "DisplayText";
                //cmbStatus.ValueMember = "Value";
                //cmbStatus.DataSource = EnumHelper.ConvertEnumForBinding(typeof(RunUploadStatus));
                //Utils.UltraDropDownFilter(cmbStatus, "DisplayText");
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

        bool _isGridInitialized = false;
        /// <summary>
        /// Handles the InitializeLayout event of the gridRunUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs"/> instance containing the event data.</param>
        private void gridRunUpload_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                if (bool.Equals(_isGridInitialized, false))
                {
                    gridRunUpload.DisplayLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
                    gridRunUpload.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
                    gridRunUpload.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                    gridRunUpload.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
                    gridRunUpload.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
                    gridRunUpload.DisplayLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
                    gridRunUpload.DisplayLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

                    _gridBandRunUpload = gridRunUpload.DisplayLayout.Bands[0];

                    UltraGridColumn colID = _gridBandRunUpload.Columns[CAPTION_CompanyUploadSetupID];
                    colID.Hidden = true;

                    UltraGridColumn colFtpServer = _gridBandRunUpload.Columns[CAPTION_FTPServer];
                    colFtpServer.Hidden = true;

                    UltraGridColumn colPort = _gridBandRunUpload.Columns[CAPTION_Port];
                    colPort.Hidden = true;

                    UltraGridColumn colUserName = _gridBandRunUpload.Columns[CAPTION_UserName];
                    colUserName.Hidden = true;

                    UltraGridColumn colPassword = _gridBandRunUpload.Columns[CAPTION_Password];
                    colPassword.Hidden = true;

                    if (!_gridBandRunUpload.Columns.Exists(CAPTION_IsSelected))
                    {
                        UltraGridColumn colSelect = _gridBandRunUpload.Columns.Add(CAPTION_IsSelected);
                        colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                        colSelect.DataType = typeof(bool);
                        colSelect.Header.Caption = "Select Record";
                        colSelect.Header.VisiblePosition = 1;
                    }

                    UltraGridColumn colCompanyNameID = _gridBandRunUpload.Columns[CAPTION_CompanyNameID];
                    colCompanyNameID.Hidden = true;

                    UltraGridColumn colDataSourceNameID = _gridBandRunUpload.Columns[CAPTION_DataSourceNameID];
                    colDataSourceNameID.Header.Caption = "Upload ThirdParty";
                    colDataSourceNameID.Width = 100;
                    colDataSourceNameID.Header.VisiblePosition = 2;
                    colDataSourceNameID.Hidden = false;
                    gridRunUpload.DisplayLayout.Bands[0].Columns[CAPTION_DataSourceNameID].CellActivation = Activation.NoEdit;

                    UltraGridColumn colTableTypeID = _gridBandRunUpload.Columns[CAPTION_TableTypeID];
                    colTableTypeID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                    colTableTypeID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.OnCellActivate;
                    colTableTypeID.ValueList = cmbTableTypes;
                    colTableTypeID.Header.Caption = "Import Type";
                    colTableTypeID.Header.VisiblePosition = 3;
                    colTableTypeID.Width = 90;
                    colTableTypeID.Hidden = false;
                    gridRunUpload.DisplayLayout.Bands[0].Columns[CAPTION_TableTypeID].CellActivation = Activation.NoEdit;

                    UltraGridColumn colTableFormatName = _gridBandRunUpload.Columns[CAPTION_TableFormatName];
                    colTableFormatName.Header.VisiblePosition = 4;
                    colTableFormatName.Width = 150;
                    colTableFormatName.Header.Caption = "Import Type Format Name";
                    colTableFormatName.Hidden = false;
                    gridRunUpload.DisplayLayout.Bands[0].Columns[CAPTION_TableFormatName].CellActivation = Activation.NoEdit;

                    if (!_gridBandRunUpload.Columns.Exists(CAPTION_Account))
                    {
                        UltraGridColumn colAccount = _gridBandRunUpload.Columns.Add(CAPTION_Account);
                        colAccount.Width = 120;
                        colAccount.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                        colAccount.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        colAccount.ValueList = GetAccountsValueList();
                        colAccount.Header.Caption = "Account";
                        colAccount.Header.VisiblePosition = 5;
                    }

                    if (!_gridBandRunUpload.Columns.Exists(CAPTION_IsDateSelected))
                    {
                        UltraGridColumn colDateSelect = _gridBandRunUpload.Columns.Add(CAPTION_IsDateSelected);
                        colDateSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                        colDateSelect.DataType = typeof(bool);
                        colDateSelect.Header.Caption = "Select Date";
                        colDateSelect.Header.VisiblePosition = 6;
                    }

                    if (!_gridBandRunUpload.Columns.Exists(CAPTION_Date))
                    {
                        UltraGridColumn colDate = _gridBandRunUpload.Columns.Add(CAPTION_Date);
                        colDate.Width = 80;
                        colDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                        colDate.MaskInput = "mm/dd/yyyy";
                        colDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                        colDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                        colDate.Header.Caption = "Date";
                        colDate.Header.VisiblePosition = 7;
                    }

                    if (_gridBandRunUpload.Columns.Exists(CAPTION_EnableAutoTime))
                    {
                        UltraGridColumn colEnableAutoTime = _gridBandRunUpload.Columns[CAPTION_EnableAutoTime];
                        colEnableAutoTime.Hidden = true;
                    }
                    UltraGridColumn colAutoTime = _gridBandRunUpload.Columns[CAPTION_AutoTime];
                    colAutoTime.Hidden = true;


                    UltraGridColumn colStatus = _gridBandRunUpload.Columns[CAPTION_Status];
                    colStatus.Hidden = true;

                    UltraGridColumn colProgress = _gridBandRunUpload.Columns[CAPTION_StatusProgress];
                    colProgress.Hidden = true;

                    UltraGridColumn colFilePath = _gridBandRunUpload.Columns[CAPTION_FilePath];
                    colFilePath.Header.VisiblePosition = 8;
                    colFilePath.Header.Caption = "File Path";
                    colFilePath.Width = 150;
                    colFilePath.Hidden = false;
                    gridRunUpload.DisplayLayout.Bands[0].Columns[CAPTION_FilePath].CellActivation = Activation.NoEdit;

                    if (!_gridBandRunUpload.Columns.Exists(CAPTION_FileSelectButton))
                    {
                        UltraGridColumn colSelectButton = _gridBandRunUpload.Columns.Add(CAPTION_FileSelectButton);
                        colSelectButton.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                        colSelectButton.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                        colSelectButton.Hidden = false;
                        colSelectButton.Header.VisiblePosition = 9;
                        colSelectButton.Width = 70;
                        colSelectButton.Header.Caption = "Select File";
                        colSelectButton.NullText = "Select File";
                    }

                    UltraGridColumn colDirPath = _gridBandRunUpload.Columns[CAPTION_DirPath];
                    colDirPath.Hidden = true;

                    UltraGridColumn colFileName = _gridBandRunUpload.Columns[CAPTION_FileName];
                    colFileName.Hidden = true;


                    UltraGridColumn colTotalRecords = _gridBandRunUpload.Columns[CAPTION_NumberOfRecords];
                    colTotalRecords.Header.VisiblePosition = 10;
                    colTotalRecords.Header.Caption = "Total Records";
                    gridRunUpload.DisplayLayout.Bands[0].Columns[CAPTION_NumberOfRecords].CellActivation = Activation.NoEdit;

                    UltraGridColumn colLastUploadDate = _gridBandRunUpload.Columns[CAPTION_LastRunUploadDate];
                    colLastUploadDate.Hidden = true;

                    UltraGridColumn colDataSourceXSLT = _gridBandRunUpload.Columns[CAPTION_DataSourceXSLT];
                    colDataSourceXSLT.Hidden = true;

                    _isGridInitialized = true;
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
        /// Handles the ClickCellButton event of the gridRunUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.CellEventArgs"/> instance containing the event data.</param>
        private void gridRunUpload_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (string.Equals(e.Cell.Column.Key, CAPTION_ErrorsButton))
                {

                    RunUpload selectedRow = (RunUpload)e.Cell.Row.ListObject;
                    string errorMessage = selectedRow.StatusDescription;
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        MessageBox.Show(errorMessage, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (selectedRow.Status == RunUploadStatus.Successful)
                    {
                        MessageBox.Show("File Uploaded Successfully", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (string.Equals(e.Cell.Column.Key, CAPTION_FileSelectButton))
                {
                    //string title = "Select File ";
                    //string buttonKey = CAPTION_FileSelectButton;
                    string fileWithPath = GetFileName();
                    if (!String.IsNullOrEmpty(fileWithPath))
                    {
                        gridRunUpload.ActiveRow.Cells[CAPTION_FilePath].Value = fileWithPath;
                    }
                }
                else if (string.Equals(e.Cell.Column.Key, CAPTION_XSLTSelectButton))
                {
                    //string title = "Select XSLT ";
                    //string buttonKey = CAPTION_XSLTSelectButton;
                    string fileWithPath = GetFileName();
                    if (!String.IsNullOrEmpty(fileWithPath))
                    {
                        gridRunUpload.ActiveRow.Cells[CAPTION_XSLTFilePath].Value = fileWithPath;
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

        private string GetFileName()
        {
            string strFileName = string.Empty;
            try
            {
                #region commentedcode
                //OpenFileDialog openFileDialog1 = new OpenFileDialog();
                //openFileDialog1.InitialDirectory = "DeskTop";
                //openFileDialog1.Title = title;
                //if (buttonKey.Equals(CAPTION_FileSelectButton))
                //{
                //    openFileDialog1.Filter = "Excel Files (*.xls)|*.xls|CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                //}
                //else if (buttonKey.Equals(CAPTION_XSLTSelectButton))
                //{
                //    openFileDialog1.Filter = "XSLT Files (*.xslt)|*.xslt";
                //}

                //if (openFileDialog1.ShowDialog() == DialogResult.OK)
                //{
                //    strFileName = openFileDialog1.FileName;
                //}
                #endregion
                //Added By : Manvendra Prajapati
                // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-8989
                Boolean CheckAccessPermission = true;
                strFileName = OpenFileDialogHelper.GetFileNameUsingOpenFileDialog(CheckAccessPermission);
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
            return strFileName;

        }

        /// <summary>
        /// Handles the CellChange event of the gridRunUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.CellEventArgs"/> instance containing the event data.</param>
        private void gridRunUpload_CellChange(object sender, CellEventArgs e)
        {
            if (string.Equals(e.Cell.Column.Key, CAPTION_IsDateSelected))
            {
                string statusText = e.Cell.Text;
                if (statusText.ToUpper().Equals(CAPTION_True))
                {
                    e.Cell.Activation = Activation.AllowEdit;
                    e.Cell.Row.Cells[CAPTION_Date].Activation = Activation.AllowEdit;
                    e.Cell.Row.Cells[CAPTION_Date].Value = DateTime.UtcNow;
                }
                else if (statusText.ToUpper().Equals(CAPTION_False))
                {
                    e.Cell.Row.Cells[CAPTION_Date].Activation = Activation.Disabled;
                }
            }
        }

        /// <summary>
        /// Handles the AfterCellUpdate event of the gridRunUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Win.UltraWinGrid.CellEventArgs"/> instance containing the event data.</param>
        private void gridRunUpload_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (string.Equals(e.Cell.Column.Key, CAPTION_Status))
                {
                    e.Cell.Row.Cells[CAPTION_ErrorsButton].Activation = Activation.Disabled;
                }
                else if (string.Equals(e.Cell.Column.Key, CAPTION_Account) && string.IsNullOrEmpty(e.Cell.Text))
                {
                    e.Cell.Row.Cells[CAPTION_Account].Value = int.MinValue;
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
        /// It will upload the data into the respective table of database
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                UploadDataThruLocalFile();
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


        #region Upload through Local File
        // list collection of Positions or Transaction
        //List<PositionMaster> _positionMasterCollection = new List<PositionMaster>();
        List<PositionMaster> _positionMasterCollection = null;
        // list collection of Security Master Symbols
        List<SecMasterUIObj> secMasterInsertNewDataobj = null;
        // list collection of cash currency values
        List<CashCurrencyValue> _cashCurrencyValueCollection = new List<CashCurrencyValue>();
        // list collection of import mark price values
        List<MarkPriceImport> _markPriceValueCollection = new List<MarkPriceImport>();
        // list collection of import beta values
        List<BetaImport> _betaValueCollection = new List<BetaImport>();
        //list collection of import dividend values
        List<DividendImport> _dividendValueCollection = new List<DividendImport>();
        // list collection of import forex price values
        List<ForexPriceImport> _forexPriceValueCollection = new List<ForexPriceImport>();
        // list collection of update Security Master data 
        SecMasterUpdateDataByImportList _secMasterUpdateDataobj = new SecMasterUpdateDataByImportList();
        // list collection of import Option Model Input Values 
        List<UserOptModelInput> _omiValueCollection = new List<UserOptModelInput>();
        // list collection of import Daily Credit Limit Values 
        List<DailyCreditLimit> _dailyCreditLimitCollection = new List<DailyCreditLimit>();
        // list collection of Settlement Date cash currency values
        List<SettlementDateCashCurrencyValue> _settlementDateCashCurrencyValueCollection = new List<SettlementDateCashCurrencyValue>();
        // list collection for import volatility values
        List<VolatilityImport> _volatilityValueCollection = new List<VolatilityImport>();
        // list collection for import volatility values
        List<VWAPImport> _vWAPValueCollection = new List<VWAPImport>();
        // list collection for import Collateral values
        List<CollateralImport> _collateralValueCollection = new List<CollateralImport>();
        // list collection for import dividend yield values
        List<DividendYieldImport> _dividendYieldValueCollection = new List<DividendYieldImport>();
        // list collection for import Stage order 
        List<OrderSingle> _stageOrderCollection = new List<OrderSingle>();

        string _isUserSelectedDate = CAPTION_False;
        string _userSelectedDate = string.Empty;
        int _userSelectedAccountValue = int.MinValue;
        DataSet _dsSecMasterInsert = new DataSet();
        DataSet _dsCollIntMasterInsert = new DataSet();
        DataSet _dsAllocationScheme = new DataSet();
        string _SMMappingXMLName = string.Empty;
        DataTable _dtSMMapping = null;
        Dictionary<string, XmlNode> _SMMappingCOLList = new Dictionary<string, XmlNode>();

        private bool IsTradeServerConnected(string strTableType)
        {
            try
            {
                if (strTableType.Equals(_importTypeNetPosition) ||
                    strTableType.Equals(_importTypeTransaction) ||
                    strTableType.Equals(_importTypeMarkPrice) ||
                    strTableType.Equals(_importTypeOMI) ||
                    //strTableType.Equals(_importTypeCashTransactions) || 
                    strTableType.Equals(_importTypeAllocationScheme) ||
                    strTableType.Equals(_importTypeAllocationScheme_AppPositions) ||
                    strTableType.Equals(_importTypeActivity) ||
                    strTableType.Equals(_importTypeSecMasterInsert) ||
                    strTableType.Equals(_importTypeSecMasterUpdate) ||
                    strTableType.Equals(_importTypeDailyBeta) ||
                    strTableType.Equals(_importTypeSettlementDateCash) ||
                    strTableType.Equals(_importTypeDailyVolatility) ||
                    strTableType.Equals(_importTypeDailyVWAP) ||
                    strTableType.Equals(_importTypeCollateralPrice) ||
                    strTableType.Equals(_importTypeDailyDividendYield) ||
                    strTableType.Equals(_importTypeCollateralInterest)
                    )
                {
                    if (toolStripStatusLabel1.Text.Equals(_tradeServerDISConnected))
                    {
                        return true;
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
            return false;
        }

        AllocationSchemeForm _schemeForm = null;
        //BackgroundWorker _bgwImportData = null;
        private void UploadDataThruLocalFile()
        {
            try
            {
                toolStripStatusLabel2.Text = string.Empty;
                // check file from Local Directory and XSLT is selected for the selectedrow
                int totalRecordsCount = 0;
                bool validationcheck = ValidationCheck();
                bool isOMIImport = false;
                if (validationcheck)
                {
                    //List<AllocationGroup> allocationGroupList = new List<AllocationGroup>();
                    int rowsUpdated = 0;


                    foreach (UltraGridRow gridrow in gridRunUpload.Rows)
                    {
                        bool isRowSelected = Convert.ToBoolean(gridrow.Cells[CAPTION_IsSelected].Value);
                        if (isRowSelected)
                        {
                            //clear all lists and dictionaries
                            ClearAll();

                            string fileWithPath = Convert.ToString(gridrow.Cells[CAPTION_FilePath].Value);
                            string strTableType = Convert.ToString(gridrow.Cells[CAPTION_TableTypeID].Text);


                            if (IsTradeServerConnected(strTableType))
                            {
                                return;
                            }

                            _isUserSelectedDate = gridrow.Cells[CAPTION_IsDateSelected].Value.ToString();
                            if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True))
                            {
                                DateTime userDateTime = Convert.ToDateTime(gridrow.Cells[CAPTION_Date].Value.ToString());
                                _userSelectedDate = userDateTime.ToString(DATEFORMAT);
                            }

                            if (!gridrow.Cells[CAPTION_Account].Text.Equals(ApplicationConstants.C_COMBO_SELECT))
                            {
                                _userSelectedAccountValue = Convert.ToInt32(gridrow.Cells[CAPTION_Account].Value);
                            }

                            DataTable dataSource = null;

                            if (strTableType.Equals(_importTypeAllocationScheme_AppPositions))
                            {
                                DateTime date = DateTime.Today;
                                if (!string.IsNullOrEmpty(_userSelectedDate))
                                {
                                    date = Convert.ToDateTime(_userSelectedDate);
                                }

                                dataSource = AllocationSchemeImportHelper.GetPositions(date);
                            }
                            else
                            {
                                dataSource = FileReaderFactory.GetDataTableFromDifferentFileFormats(fileWithPath);
                            }

                            if (dataSource != null)
                            {
                                dataSource = _importHelper.ArrangeTable(dataSource);
                                // get the PM xslt with path                               
                                string strXSLTPath = Convert.ToString(gridrow.Cells[CAPTION_DataSourceXSLT].Value);
                                // get the XSLT name only
                                string strXSLTName = strXSLTPath.Substring(strXSLTPath.LastIndexOf("\\") + 1);
                                // set the XSLT path as StartUp Path
                                string dirPath = ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.PMImportXSLT.ToString();
                                string mappedfilePath = _importHelper.GenerateXML(dataSource, strXSLTName, strTableType);
                                // string getMappedXML = string.Empty;
                                if (!mappedfilePath.Equals(""))
                                {
                                    DataSet ds = new DataSet();
                                    ds.ReadXml(mappedfilePath);

                                    GenerateSMMapping(dirPath, ds);

                                    // Now we have arranged and updated XML
                                    // as above we inserted "*" in the blank columns, but "*" needs extra treatment, so
                                    // again we replace the "*" with blank string, the following looping does the same
                                    _importHelper.ReArrangeDataSet(ds);

                                    //Created By: Pooja Porwal
                                    //Date:12 Feb 2015
                                    //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-5820

                                    if (!(strTableType.Equals(_importTypeAllocationScheme_AppPositions) || strTableType.Equals(_importTypeAllocationScheme)))
                                    {
                                        _displayForm = ImportPositionsDisplayForm.GetInstance();
                                        //_displayForm is forcefully visible true false for forcefully creating Handel
                                        //because sometimes when this form is validated it sends false because of Handel is not created
                                        _displayForm.Visible = true;
                                        _displayForm.Visible = false;
                                    }

                                    #region Import Settlement Date Cash
                                    if (strTableType.Equals(_importTypeSettlementDateCash))
                                    {
                                        UpdateSettlementDateCashCurrencyValueCollection(ds);
                                        if (_settlementDateCashCurrencyValueCollection.Count > 0)
                                        {
                                            if (DisplayToUser(strTableType))
                                            {
                                                // insert cash values into the DB
                                                if (_settlementDateCashCurrencyValueCollection.Count > 0)
                                                {
                                                    // total number of records inserted
                                                    totalRecordsCount = _settlementDateCashCurrencyValueCollection.Count;

                                                    rowsUpdated = RunUploadManager.SaveRunUploadFileDataForSettlementDateCash(_settlementDateCashCurrencyValueCollection);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }

                                            if (rowsUpdated > 0)
                                            {
                                                gridrow.Cells[CAPTION_NumberOfRecords].Value = totalRecordsCount;
                                                gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                            }
                                        }
                                    }
                                    #endregion Import Settlement Date Cash

                                    #region Import Netposition/ Transactions
                                    else if (strTableType.Equals(_importTypeNetPosition) || strTableType.Equals(_importTypeTransaction))
                                    {
                                        _positionMasterCollection = new List<PositionMaster>();
                                        UpdatePositionMasterCollection(ds);
                                        _SMRequest = EnmImportType.PositionImport.ToString();
                                        if (_positionMasterCollection.Count > 0)
                                        {
                                            if (_SMMappingCOLList.Count > 0)
                                            {
                                                Prana.Utilities.MiscUtilities.GeneralUtilities.InsertDataIntoOneTableFromAnotherTable(ds.Tables[0], _dtSMMapping, _SMMappingCOLList);
                                                RemoveSMCachedDataFromEnRichedTable();
                                                SendSMEnRichData();
                                            }
                                            GetSMDataForTaxlotImport();

                                            if (DisplayToUser(strTableType))
                                            {
                                                // insert positions into the DB
                                                if (_positionMasterCollection.Count > 0)
                                                {
                                                    this.IsImportRunning = true;
                                                    InitializeProgessBar(_positionMasterCollection);
                                                    //InitializeBackgroundWorker();
                                                    ImportDataAsync();
                                                    _rowIndex = gridrow.Index;
                                                    // total number of records inserted
                                                    //rowsUpdated = _allocationServices.InnerChannel.CreateAndSavePositionsFromImport(_positionMasterCollection);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                            //if (rowsUpdated > 0)
                                            //{
                                            //    gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                            //    gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                            //}
                                        }
                                    }
                                    #endregion

                                    #region commented Import Activity
                                    //else if (strTableType.Equals(_importTypeCashTransactions))
                                    //{
                                    //    DataSet dsDoubleEntryCash = UpdateActivityValueCollection(ds);
                                    //    if (dsDoubleEntryCash.Tables.Count > 0)
                                    //    {
                                    //        ImportPositionsDisplayForm displayForm = ImportPositionsDisplayForm.GetInstance();
                                    //        displayForm.refersh += new EventHandler(displayForm_refersh);
                                    //        displayForm.Visible = false;
                                    //        displayForm.BindImportedDoubleEntryCash(ds, _userSelectedDate, _userSelectedAccountValue, strTableType, LaunchForm);
                                    //        displayForm.ShowDialog();
                                    //    }
                                    //    else
                                    //    {
                                    //        MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //    }
                                    //}
                                    #endregion

                                    #region Import Cash Transactions
                                    else if (strTableType.Equals(_importTypeActivity))
                                    {
                                        UpdateDividendValueCollection(ds);
                                        _SMRequest = EnmImportType.DividendImport.ToString();
                                        if (_dividendValueCollection.Count > 0)
                                        {
                                            GetSMDataForDividendImport();
                                            if (DisplayToUser(strTableType))
                                            {
                                                // insert positions into the DB
                                                if (_dividendValueCollection.Count > 0)
                                                {
                                                    // total number of records inserted                                                   
                                                    DataSet dsInserted = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateDataSetFromCollection(_dividendValueCollection, null);

                                                    if (dsInserted != null && dsInserted.Tables[0].Rows.Count > 0)
                                                    {
                                                        dsInserted = CashDataManager.GetInstance().SaveImortedCashDividend(dsInserted);
                                                        rowsUpdated = dsInserted.Tables[0].Rows.Count;
                                                        if (dsInserted.Tables[0].Rows.Count > 0)
                                                            AddDailyDataAuditEntryForDividendImport(dsInserted.Tables[0], TradeAuditActionType.ActionType.Dividend_Applied_CashTransaction, "Dividend Import", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }

                                            if (rowsUpdated > 0)
                                            {
                                                gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                                gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                            }

                                            //ImportPositionsDisplayForm displayForm = ImportPositionsDisplayForm.GetInstance();
                                            //displayForm.BindCashDividendImport(ds, _userSelectedDate, _userSelectedAccountValue, strTableType);
                                            //displayForm.Visible = false;
                                            //displayForm.ShowDialog();
                                        }
                                    }
                                    #endregion

                                    #region ImportCash
                                    else if (strTableType.Equals(_importTypeCash))
                                    {
                                        UpdateCashCurrencyValueCollection(ds);
                                        if (_cashCurrencyValueCollection.Count > 0)
                                        {
                                            if (DisplayToUser(strTableType))
                                            {
                                                // insert cash values into the DB
                                                if (_cashCurrencyValueCollection.Count > 0)
                                                {
                                                    // total number of records inserted
                                                    //totalRecordsCount = _cashCurrencyValueCollection.Count;

                                                    rowsUpdated = RunUploadManager.SaveRunUploadFileDataForCash(_cashCurrencyValueCollection);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }

                                            if (rowsUpdated > 0)
                                            {
                                                gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                                gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region MarkPriceImport
                                    else if (strTableType.Equals(_importTypeMarkPrice))
                                    {
                                        UpdateMarkPriceValueCollection(ds);
                                        _SMRequest = EnmImportType.MarkPriceImport.ToString();
                                        if (_markPriceValueCollection.Count > 0)
                                        {
                                            if (_SMMappingCOLList.Count > 0)
                                            {
                                                Prana.Utilities.MiscUtilities.GeneralUtilities.InsertDataIntoOneTableFromAnotherTable(ds.Tables[0], _dtSMMapping, _SMMappingCOLList);
                                                RemoveSMCachedDataFromEnRichedTable();
                                                SendSMEnRichData();
                                            }
                                            GetSMDataForMarkPriceImport();
                                            if (DisplayToUser(strTableType))
                                            {
                                                // insert cash values into the DB
                                                if (_markPriceValueCollection.Count > 0)
                                                {
                                                    // total number of records inserted
                                                    //totalRecordsCount = _markPriceValueCollection.Count;

                                                    DataTable dtMarkPrices = CreateDataTableForMarkPriceImport();

                                                    DataTable dtMarkPriceTableFromCollection = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateTableFromCollection<MarkPriceImport>(dtMarkPrices, _markPriceValueCollection);

                                                    if ((dtMarkPriceTableFromCollection != null) && (dtMarkPriceTableFromCollection.Rows.Count > 0))
                                                    {
                                                        //Modifeid by omshiv, isAutoApprove mark prices, for prana mode it is false
                                                        bool isAutoApprove = false;
                                                        rowsUpdated = _pricingServicesProxy.InnerChannel.SaveMarkPrices(dtMarkPriceTableFromCollection, isAutoApprove);
                                                        if (rowsUpdated > 0)
                                                        {
                                                            AddDailyDataAuditEntryForMarkPrice(dtMarkPriceTableFromCollection, Prana.BusinessObjects.TradeAuditActionType.ActionType.MarkPrice_Changed, "Mark Price Changed (Import)", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                                            AuditManager.Instance.SaveAuditList(_auditCollection_MarkPrice);
                                                            _auditCollection_MarkPrice.Clear();
                                                        }
                                                    }
                                                    //rowsUpdated = RunUploadManager.SaveRunUploadFileDataForMarkPrice(dtMarkPricesNew,_markPriceValueCollection);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }

                                            if (rowsUpdated > 0)
                                            {
                                                gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                                gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region FXImport
                                    else if (strTableType.Equals(_importTypeForexPrice))
                                    {
                                        UpdateForexPriceValueCollection(ds);
                                        if (_forexPriceValueCollection.Count > 0)
                                        {
                                            if (DisplayToUser(strTableType))
                                            {
                                                // insert cash values into the DB
                                                if (_forexPriceValueCollection.Count > 0)
                                                {
                                                    // total number of records inserted
                                                    //totalRecordsCount = _forexPriceValueCollection.Count;
                                                    rowsUpdated = RunUploadManager.SaveRunUploadFileDataForForexPrice(_forexPriceValueCollection);
                                                    if (rowsUpdated > 0)
                                                    {
                                                        UpdateDayEndBaseCashByForexRate(_forexPriceValueCollection);
                                                        AddDailyDataAuditEntryForFXRate(_forexPriceValueCollection, Prana.BusinessObjects.TradeAuditActionType.ActionType.ForexRate_Changed, "Forex Rate Changed (Import)", CachedDataManager.GetInstance.LoggedInUser.CompanyUserID);
                                                        AuditManager.Instance.SaveAuditList(_auditCollection_FXRate);
                                                        _auditCollection_FXRate.Clear();
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }

                                            if (rowsUpdated > 0)
                                            {
                                                gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                                gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region SecMasterInsert
                                    else if (strTableType.Equals(_importTypeSecMasterInsert))
                                    {
                                        CreateSMSyncPoxy();
                                        _dynamicUDACache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                                        _securityMaster.GetAllUDAAtrributes();
                                        _securityMaster.UDAAttributesResponse += new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                                        _dsSecMasterInsert = UpdateSecMasterInsertValueCollection(ds);
                                        if (_dsSecMasterInsert.Tables.Count > 0)
                                        {
                                            _SMRequest = EnmImportType.SecMasterInsertData.ToString();

                                            // check if security existing or not.
                                            GetSMDataForSecMasterInsertData();

                                            _displayForm.LaunchForm += new EventHandler(displayForm_LaunchForm);

                                            _displayForm.BindSecMasterData(_dsSecMasterInsert, strTableType, _dynamicUDACache);
                                            if (_displayForm.ShowDialog() == DialogResult.OK)
                                            {
                                                _dsSecMasterInsert = _displayForm.ValidatedSMInsertValue;
                                            }
                                            else
                                            {
                                                _dsSecMasterInsert = null;
                                            }
                                            if (_dsSecMasterInsert != null && ds.Tables.Count > 0)
                                            {
                                                // Saving Newly Added UDAs for SM Import. 
                                                SaveNewlyAddedUDAsBySMImport(_dsSecMasterInsert);
                                                secMasterInsertNewDataobj = ConvertDSToSecMasterInsertValueCollection(_dsSecMasterInsert);

                                                // insert values into the DB
                                                if (secMasterInsertNewDataobj.Count > 0)
                                                {
                                                    SecMasterbaseList lst = ValidateSecMasterDataBeforeSave(secMasterInsertNewDataobj);
                                                    if (lst.Count > 0)
                                                    {
                                                        if (toolStripStatusLabel1.Text.Equals(_tradeServerConnected))
                                                        {
                                                            // total number of records inserted
                                                            //rowsUpdated = lst.Count;
                                                            //rowsUpdated = totalRecordsCount;
                                                            _securityMaster.SaveNewSymbols_Import(lst);
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("TradeService not connected, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                                if (rowsUpdated > 0)
                                                {
                                                    gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                                    gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nothing to import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        _securityMaster.UDAAttributesResponse -= new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                                    }
                                    #endregion

                                    #region CollateralInterestImport
                                    else if (strTableType.Equals(_importTypeCollateralInterest))
                                    {
                                        DataTable dtSM = ds.Tables[0];

                                        if (!dtSM.Columns.Contains("ValidationStatus"))
                                        {
                                            DataColumn dcValidated = new DataColumn("ValidationStatus");
                                            dcValidated.DataType = typeof(string);
                                            dcValidated.DefaultValue = ApplicationConstants.ValidationStatus.None.ToString();
                                            dtSM.Columns.Add(dcValidated);
                                        }
                                        _dsCollIntMasterInsert = ds;
                                        InsertDataintoCollateralInterest(_dsCollIntMasterInsert);
                                        AddLocalfileUploadData(_dsCollIntMasterInsert);
                                        CheckValididationOfCollateralInterest(_dsCollIntMasterInsert);
                                        ChangeAccountIdToAccountName(_dsCollIntMasterInsert);
                                        if (_dsCollIntMasterInsert.Tables.Count > 0)
                                        {
                                            _displayForm.LaunchForm += new EventHandler(displayForm_LaunchForm);
                                            _displayForm.BindColMasterData(_dsCollIntMasterInsert, strTableType);
                                            if (_displayForm.ShowDialog() == DialogResult.OK)
                                            {
                                                _dsCollIntMasterInsert = _displayForm.ValidatedCollateralInterestValue;
                                            }
                                            else
                                            {
                                                _dsCollIntMasterInsert = null;
                                            }
                                            if (_dsCollIntMasterInsert != null && ds.Tables.Count > 0)
                                            {
                                                // insert values into the DB
                                                ChangeAccountNameToAccountID(_dsCollIntMasterInsert);
                                                DataTable CollaterVariable = _dsCollIntMasterInsert.Tables[0];

                                                if (CollaterVariable.Rows.Count > 0)
                                                {
                                                    rowsUpdated = RunUploadManager.SaveRunUploadFileDataForCollateral(CollaterVariable);
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                                if (rowsUpdated > 0)
                                                {
                                                    gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                                    gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nothing to import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    #endregion

                                    #region SecMasterUpdate
                                    else if (strTableType.Equals(_importTypeSecMasterUpdate))
                                    {
                                        CreateSMSyncPoxy();
                                        _dynamicUDACache = _secMasterSyncService.InnerChannel.GetDynamicUDAList();
                                        _securityMaster.UDAAttributesResponse += new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                                        //It will update the columnName and Enum Values.
                                        _securityMaster.GetAllUDAAtrributes();
                                        UpdateDataTableColumnNameandValues(ds);
                                        UpdateSecMasterUpdateDataValueCollection(ds);
                                        _SMRequest = EnmImportType.SecMasterUpdateData.ToString();
                                        if (_secMasterUpdateDataobj.Count > 0)
                                        {
                                            GetSMDataForSecMasterUpdateData();
                                            if (DisplayToUser(strTableType))
                                            {
                                                // insert Security Master values into the DB
                                                if (_secMasterUpdateDataobj.Count > 0)
                                                {
                                                    SaveNewlyAddedUDAsBySMUpdate(_secMasterUpdateDataobj);
                                                    _secMasterUpdateDataobj.RequestID = System.Guid.NewGuid().ToString();
                                                    //SecMasterbaseList lst = ValidateSecMasterDataBeforeSave(_secMasterUpdateDataobj);
                                                    //if (lst.Count > 0)
                                                    //{
                                                    // total number of records inserted
                                                    if (toolStripStatusLabel1.Text.Equals(_tradeServerConnected))
                                                    {
                                                        rowsUpdated = _secMasterUpdateDataobj.Count;
                                                        //rowsUpdated = totalRecordsCount;
                                                        _securityMaster.UpdateSymbols_Import(_secMasterUpdateDataobj);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("TradeService not connected, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    }
                                                    //}
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }

                                            if (rowsUpdated > 0)
                                            {
                                                gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                                gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                            }
                                        }
                                        _securityMaster.UDAAttributesResponse -= new EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>>(_securityMaster_UDAAttributesResponse);
                                    }
                                    #endregion

                                    #region OMIImport
                                    else if (strTableType.Equals(_importTypeOMI))
                                    {
                                        UpdateOMIValueCollection(ds);
                                        _SMRequest = EnmImportType.OMIImport.ToString();
                                        if (_omiValueCollection.Count > 0)
                                        {
                                            GetSMDataForOMIImport();
                                            if (DisplayToUser(strTableType))
                                            {
                                                // insert OMI values into the DB
                                                if (_omiValueCollection.Count > 0)
                                                {
                                                    // total number of records inserted
                                                    //totalRecordsCount = _omiValueCollection.Count;
                                                    rowsUpdated = _pricingServicesProxy.InnerChannel.SaveRunUploadFileDataForOMI(_omiValueCollection);
                                                    if (rowsUpdated > 0)
                                                    {
                                                        RunUploadManager.UpdateOMI(rowsUpdated, ref isOMIImport);
                                                    }

                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }

                                            if (rowsUpdated > 0)
                                            {
                                                gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                                gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region DailyBeta
                                    else if (strTableType.Equals(_importTypeDailyBeta))
                                    {
                                        UpdateBetaValueCollection(ds);
                                        _SMRequest = EnmImportType.BetaImport.ToString();
                                        if (_betaValueCollection.Count > 0)
                                        {
                                            GetSMDataForBetaImport();

                                            if (DisplayToUser(strTableType))
                                            {
                                                if (_betaValueCollection.Count > 0)
                                                {
                                                    // total number of records inserted

                                                    DataTable dtBetaValues = CreateDataTableForBetaImport();

                                                    DataTable dtBetaTableFromCollection = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateTableFromCollection<BetaImport>(dtBetaValues, _betaValueCollection);

                                                    if ((dtBetaTableFromCollection != null) && (dtBetaTableFromCollection.Rows.Count > 0))
                                                    {
                                                        rowsUpdated = _pricingServicesProxy.InnerChannel.SaveBeta(dtBetaTableFromCollection);
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region AllocationSchemeImport
                                    else if (strTableType.Equals(_importTypeAllocationScheme))
                                    {
                                        _dsAllocationScheme = AllocationSchemeImportHelper.UpdateAllocationSchemeValueCollection(ds, _currencyListForAlloScheme, false);

                                        _allocationSchemeSymbologyWiseDict = AllocationSchemeImportHelper.AllocationSchemeSymbologyWiseDict;

                                        int allocationschemeID = int.MinValue;
                                        if (_dsAllocationScheme.Tables.Count > 0)
                                        {
                                            _SMRequest = EnmImportType.AllocationScheme.ToString();
                                            GetSMDataForAllocationScheme();

                                            _schemeForm = new AllocationSchemeForm();
                                            //_schemeForm is forcefully visible true false for forcefully creating Handel
                                            //because sometimes when this form is validated it sends false because of Handel is not created
                                            _schemeForm.Visible = true;
                                            _schemeForm.Visible = false;

                                            _schemeForm.BindImportAllocationScheme(_dsAllocationScheme, AllocationScheme.Import, _userSelectedDate);
                                            if (_schemeForm.ShowDialog() == DialogResult.OK)
                                            {
                                                _dsAllocationScheme = _schemeForm.ValidatedAllocationScheme(ref _allocationSchemeName, ref _allocationSchemeDate, ref allocationschemeID);
                                                if (_dsAllocationScheme != null && _dsAllocationScheme.Tables[0].Rows.Count > 0)
                                                {
                                                    //// remove extra columns  
                                                    _dsAllocationScheme = AllocationSchemeImportHelper.GetUpdatedDataSet(_dsAllocationScheme);

                                                    if (toolStripStatusLabel1.Text.Equals(_tradeServerConnected))
                                                    {
                                                        // total number of records inserted

                                                        if (_SMMappingCOLList.Count > 0)
                                                        {
                                                            _secMasterUpdateDataobj.RequestID = System.Guid.NewGuid().ToString();
                                                            Prana.Utilities.MiscUtilities.GeneralUtilities.InsertDataIntoOneTableFromAnotherTable(_dsAllocationScheme.Tables[0], _dtSMMapping, _SMMappingCOLList);
                                                            DataSet dsSM = new DataSet();
                                                            dsSM.Tables.Add(_dtSMMapping);
                                                            UpdateSecMasterUpdateDataValueCollection(dsSM);
                                                            SendSMEnRichData();
                                                            _securityMaster.UpdateSymbols_Import(_secMasterUpdateDataobj);
                                                        }

                                                        // insert data into the DB
                                                        // total number of records inserted
                                                        rowsUpdated = _dsAllocationScheme.Tables[0].Rows.Count;
                                                        //rowsUpdated = totalRecordsCount;

                                                        AllocationFixedPreference fixedPref = new AllocationFixedPreference(int.MinValue, _allocationSchemeName, _dsAllocationScheme.GetXml(), _allocationSchemeDate, true, FixedPreferenceCreationSource.SchemeImport);
                                                        allocationschemeID = _importHelper.AllocationServices.InnerChannel.SaveAllocationScheme(fixedPref);

                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("TradeService not connected, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    }
                                                }
                                            }
                                            if (allocationschemeID > 0)
                                            {
                                                gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                                gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nothing to import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    #endregion

                                    #region SchemePositions
                                    else if (strTableType.Equals(_importTypeAllocationScheme_AppPositions))
                                    {
                                        _dsAllocationScheme = AllocationSchemeImportHelper.UpdateAllocationSchemeValueCollection(ds, null, true);

                                        //_allocationSchemeSymbologyWiseDict = AllocationSchemeImportHelper.AllocationSchemeSymbologyWiseDict;

                                        int allocationschemeID = int.MinValue;
                                        if (_dsAllocationScheme.Tables.Count > 0)
                                        {
                                            _SMRequest = EnmImportType.AllocationScheme_AppPositions.ToString();
                                            //GetSMDataForAllocationScheme();
                                            _schemeForm = new AllocationSchemeForm();
                                            //_schemeForm is forcefully visible true false for forcefully creating Handel
                                            //because sometimes when this form is validated it sends false because of Handel is not created
                                            _schemeForm.Visible = true;
                                            _schemeForm.Visible = false;

                                            _schemeForm.BindImportAllocationScheme(_dsAllocationScheme, AllocationScheme.Import, _userSelectedDate);
                                            if (_schemeForm.ShowDialog() == DialogResult.OK)
                                            {
                                                _dsAllocationScheme = _schemeForm.ValidatedAllocationScheme(ref _allocationSchemeName, ref _allocationSchemeDate, ref allocationschemeID);
                                                if (_dsAllocationScheme != null && _dsAllocationScheme.Tables[0].Rows.Count > 0)
                                                {
                                                    // remove extra columns  
                                                    _dsAllocationScheme = AllocationSchemeImportHelper.GetUpdatedDataSet(_dsAllocationScheme);

                                                    if (toolStripStatusLabel1.Text.Equals(_tradeServerConnected))
                                                    {
                                                        // insert data into the DB
                                                        // total number of records inserted
                                                        rowsUpdated = _dsAllocationScheme.Tables[0].Rows.Count;
                                                        //rowsUpdated = totalRecordsCount;
                                                        AllocationFixedPreference fixedPref = new AllocationFixedPreference(int.MinValue, _allocationSchemeName, _dsAllocationScheme.GetXml(), _allocationSchemeDate, true, FixedPreferenceCreationSource.SchemeImport);
                                                        allocationschemeID = _importHelper.AllocationServices.InnerChannel.SaveAllocationScheme(fixedPref);
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("TradeService not connected, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    }
                                                }
                                            }
                                            if (allocationschemeID > 0)
                                            {
                                                gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                                gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Nothing to import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    #endregion

                                    #region DailyCreditLimit
                                    else if (strTableType.Equals(_importTypeDailyCreditLimit))
                                    {
                                        UpdateDailyCreditLimitCollection(ds);
                                        if (_dailyCreditLimitCollection.Count > 0)
                                        {
                                            if (DisplayToUser(strTableType))
                                            {
                                                // insert cash values into the DB
                                                if (_dailyCreditLimitCollection.Count > 0)
                                                {
                                                    DataTable dtDailyCreditLimit = CreateDataTableForDailyCreditLimitImport();
                                                    DataTable dtDailyCreditLimitCollection = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateTableFromCollection<DailyCreditLimit>(dtDailyCreditLimit, _dailyCreditLimitCollection);
                                                    if ((dtDailyCreditLimitCollection != null) && (dtDailyCreditLimitCollection.Rows.Count > 0))
                                                    {
                                                        if (CashManagementServices == null)
                                                            CreateCashManagementProxy();
                                                        rowsUpdated = CashManagementServices.InnerChannel.SaveDailyCreditLimitValues(dtDailyCreditLimitCollection, true);
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }

                                            if (rowsUpdated > 0)
                                            {
                                                gridrow.Cells[CAPTION_NumberOfRecords].Value = rowsUpdated;
                                                gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                            }
                                        }
                                    }
                                    #endregion

                                    #region ImportDoubleEntryCash
                                    else if (strTableType.Equals(_importTypeDoubleEntryCash))
                                    {
                                        DataSet dsDoubleEntryCash = UpdateDoubleEntryCashValueCollection(ds);
                                        CheckValididationForDoubleEntry(dsDoubleEntryCash);
                                        if (dsDoubleEntryCash.Tables.Count > 0)
                                        {
                                            _displayForm.refersh += new EventHandler(displayForm_refersh);
                                            _displayForm.BindImportedDoubleEntryCash(ds, _userSelectedDate, _userSelectedAccountValue, strTableType, LaunchForm);
                                            _displayForm.ShowDialog();

                                            gridrow.Cells[CAPTION_NumberOfRecords].Value = _displayForm.NoOfDoubleEntryCashCreated;
                                            gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                        }
                                        else
                                        {
                                            MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    #endregion

                                    #region MultilegJournalImport
                                    else if (strTableType.Equals(_importTypeMultilegJournalImport))
                                    {
                                        DataSet dsMultilegImport = UpdateDoubleEntryCashValueCollection(ds);
                                        PrepareDatasetAndValidation(dsMultilegImport);
                                        CheckValididationForMultipleEntries(dsMultilegImport);
                                        if (dsMultilegImport.Tables.Count > 0)
                                        {
                                            _displayForm.refersh += new EventHandler(displayForm_refersh);
                                            _displayForm.BindImportedMultilegJournalImport(dsMultilegImport, _userSelectedDate, _userSelectedAccountValue, strTableType, LaunchForm);
                                            _displayForm.ShowDialog();

                                            rowsUpdated = _displayForm.NoOfMultiLegJournalCreated;
                                            gridrow.Cells[CAPTION_NumberOfRecords].Value = _displayForm.NoOfMultiLegJournalCreated;
                                            gridrow.Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                                        }
                                        else
                                        {
                                            MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    #endregion

                                    #region DailyVolatility
                                    else if (strTableType.Equals(_importTypeDailyVolatility))
                                    {
                                        UpdateVolatilityValueCollection(ds);
                                        _SMRequest = EnmImportType.VolatilityImport.ToString();
                                        if (_volatilityValueCollection.Count > 0)
                                        {
                                            GetSMDataForVolatilityImport();

                                            if (DisplayToUser(strTableType))
                                            {
                                                if (_volatilityValueCollection.Count > 0)
                                                {
                                                    // total number of records inserted

                                                    DataTable dtVolatilityValues = CreateDataTableForVolatilityImport();

                                                    DataTable dtVolatilityTableFromCollection = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateTableFromCollection<VolatilityImport>(dtVolatilityValues, _volatilityValueCollection);

                                                    if ((dtVolatilityTableFromCollection != null) && (dtVolatilityTableFromCollection.Rows.Count > 0))
                                                    {
                                                        rowsUpdated = _pricingServicesProxy.InnerChannel.SaveVolatility(dtVolatilityTableFromCollection);
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region DailyVWAP
                                    else if (strTableType.Equals(_importTypeDailyVWAP))
                                    {
                                        UpdateVWAPValueCollection(ds);
                                        _SMRequest = EnmImportType.VWAPImport.ToString();
                                        if (_vWAPValueCollection.Count > 0)
                                        {
                                            GetSMDataForVWAPImport();

                                            if (DisplayToUser(strTableType))
                                            {
                                                if (_vWAPValueCollection.Count > 0)
                                                {
                                                    // total number of records inserted

                                                    DataTable dtVWAPValues = CreateDataTableForVWAPImport();

                                                    DataTable dtVWAPTableFromCollection = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateTableFromCollection<VWAPImport>(dtVWAPValues, _vWAPValueCollection);

                                                    if ((dtVWAPTableFromCollection != null) && (dtVWAPTableFromCollection.Rows.Count > 0))
                                                    {
                                                        rowsUpdated = _pricingServicesProxy.InnerChannel.SaveVWAP(dtVWAPTableFromCollection);
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region CollateralPrice
                                    else if (strTableType.Equals(_importTypeCollateralPrice))
                                    {
                                        UpdateCollateralValueCollection(ds);
                                        _SMRequest = EnmImportType.CollateralImport.ToString();
                                        if (_collateralValueCollection.Count > 0)
                                        {
                                            GetSMDataForCollateralImport();

                                            if (DisplayToUser(strTableType))
                                            {
                                                if (_collateralValueCollection.Count > 0)
                                                {
                                                    // total number of records inserted

                                                    DataTable dtCollateralValues = CreateDataTableForCollateralImport();

                                                    DataTable dtCollateralTableFromCollection = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateTableFromCollection<CollateralImport>(dtCollateralValues, _collateralValueCollection);

                                                    if ((dtCollateralTableFromCollection != null) && (dtCollateralTableFromCollection.Rows.Count > 0))
                                                    {
                                                        rowsUpdated = _pricingServicesProxy.InnerChannel.SaveCollateralValues(dtCollateralTableFromCollection);
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region DailyDividendYield
                                    else if (strTableType.Equals(_importTypeDailyDividendYield))
                                    {
                                        UpdateDividendYieldValueCollection(ds);
                                        _SMRequest = EnmImportType.DividendYieldImport.ToString();
                                        if (_dividendYieldValueCollection.Count > 0)
                                        {
                                            GetSMDataForDividendYieldImport();

                                            if (DisplayToUser(strTableType))
                                            {
                                                if (_dividendYieldValueCollection.Count > 0)
                                                {
                                                    // total number of records inserted

                                                    DataTable dtDividendYieldValues = CreateDataTableForDividendYieldImport();

                                                    DataTable dtDividendYieldTableFromCollection = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateTableFromCollection<DividendYieldImport>(dtDividendYieldValues, _dividendYieldValueCollection);

                                                    if ((dtDividendYieldTableFromCollection != null) && (dtDividendYieldTableFromCollection.Rows.Count > 0))
                                                    {
                                                        rowsUpdated = _pricingServicesProxy.InnerChannel.SaveDividendYield(dtDividendYieldTableFromCollection);
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Stage Order
                                    else if (strTableType.Equals(_importTypeStagedOrder))
                                    {
                                        _stageOrderCollection.Clear();
                                        _importHelper.StageOrderCollection = _stageOrderCollection;
                                        _importHelper.StageSymbolWiseDict = _stageSymbolWiseDict;
                                        _importHelper.CurrencyListForAlloScheme = _currencyListForAlloScheme;
                                        _rowIndex = gridrow.Index;
                                        string filePath = gridRunUpload.Rows[_rowIndex].Cells[CAPTION_FilePath].Value.ToString();
                                        string fileName = filePath.Substring(filePath.LastIndexOf(@"\") + 1);
                                        string exactFile = "*fp#_" + filePath.Substring(filePath.LastIndexOf(@"\") + 1);

                                        UpdateStageOrderCollection(ds);
                                        _SMRequest = EnmImportType.StageImport.ToString();
                                        if (_stageOrderCollection.Count > 0)
                                        {
                                            GetSMDataForStageImport();
                                            ValidateAndUpdate();
                                            if (DisplayToUser(strTableType))
                                            {
                                                if (_stageOrderCollection.Count > 0)
                                                {
                                                    _importHelper.StageOrderCollection = _stageOrderCollection;
                                                    int importFileId = ImportDataManager.SaveImportedFileDetails(fileName, filePath, ImportType.StagedOrder, File.GetLastWriteTime(filePath));
                                                    GroupStagedOrders(exactFile);
                                                    foreach (var stageOrder in _stageOrderCollection)
                                                    {
                                                        stageOrder.CumQty = 0.0;
                                                        stageOrder.AvgPrice = 0;
                                                        if (stageOrder.Price == double.Epsilon)
                                                            stageOrder.Price = 0;
                                                        stageOrder.MsgType = FIXConstants.MSGOrder;
                                                        stageOrder.ImportFileID = importFileId;
                                                        stageOrder.ImportFileName = fileName;
                                                        if (!TradeManager.TradeManager.GetInstance().SendBlotterTrades(stageOrder, 0))
                                                            Logger.LoggerWrite("StageOrder not saved. Symbol :" + stageOrder.Symbol + " AvgPrice " + stageOrder.AvgPrice + " Quantity " + stageOrder.Quantity);
                                                    }

                                                }
                                                else
                                                {
                                                    MessageBox.Show("No Record is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    if (_displayForm != null)
                                    {
                                        _displayForm.LaunchForm -= new EventHandler(displayForm_LaunchForm);
                                        _displayForm.Dispose();
                                        _displayForm = null;
                                    }

                                }
                            }
                        }
                    }
                    if (rowsUpdated > 0 && !isOMIImport)
                    {
                        MessageBox.Show(" Import Successfully Done.", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                else
                {
                    //MessageBox.Show(" Either import file or XSLT file not selected.", "PM Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Saving Newly Added UDAs by SMUpdate.
        /// </summary>
        /// <param name="secMasterUpdateDataobj"></param>
        private void SaveNewlyAddedUDAsBySMUpdate(SecMasterUpdateDataByImportList secMasterUpdateDataobj)
        {
            Dictionary<String, Dictionary<string, object>> udaData = new Dictionary<String, Dictionary<string, object>>();
            UDACollection assetID = new UDACollection();
            UDACollection sectorID = new UDACollection();
            UDACollection subSectorID = new UDACollection();
            UDACollection countryID = new UDACollection();
            UDACollection securityTypeID = new UDACollection();
            int assetIdMax = GetMaxId(SecMasterHelper.getInstance().DictUdaAsset);
            int sectorIdMax = GetMaxId(SecMasterHelper.getInstance().DictUdaSector);
            int subSectorIdMax = GetMaxId(SecMasterHelper.getInstance().DictUdaSubSector);
            int countryIdMax = GetMaxId(SecMasterHelper.getInstance().DictUdaCountries);
            int securityTypeIdMax = GetMaxId(SecMasterHelper.getInstance().DictUdaSecurityType);

            foreach (SecMasterUpdateDataByImportUI updateDataObj in secMasterUpdateDataobj)
            {
                if (updateDataObj.UDAAssetClassID == -1 && assetID.Contains(updateDataObj.UDAAssetClass.Trim()))
                {
                    int k = assetID.GetUDAId(updateDataObj.UDAAssetClass.Trim());
                    updateDataObj.UDAAssetClassID = k;
                }
                else if (updateDataObj.UDAAssetClassID == -1)
                {
                    assetID.Add(new UDA
                    {
                        ID = ++assetIdMax,
                        Name = updateDataObj.UDAAssetClass.Trim()
                    });
                    updateDataObj.UDAAssetClassID = assetIdMax;
                }

                if (updateDataObj.UDASectorID == -1 && sectorID.Contains(updateDataObj.UDASector.Trim()))
                {
                    int k = sectorID.GetUDAId(updateDataObj.UDASector.Trim());
                    updateDataObj.UDASectorID = k;
                }
                else if (updateDataObj.UDASectorID == -1)
                {
                    sectorID.Add(new UDA
                    {
                        ID = ++sectorIdMax,
                        Name = updateDataObj.UDASector.Trim()
                    });
                    updateDataObj.UDASectorID = sectorIdMax;
                }

                if (updateDataObj.UDASubSectorID == -1 && subSectorID.Contains(updateDataObj.UDASubSector.Trim()))
                {
                    int k = subSectorID.GetUDAId(updateDataObj.UDASubSector.Trim());
                    updateDataObj.UDASubSectorID = k;
                }
                else if (updateDataObj.UDASubSectorID == -1)
                {
                    subSectorID.Add(new UDA
                    {
                        ID = ++subSectorIdMax,
                        Name = updateDataObj.UDASubSector.Trim()
                    });
                    updateDataObj.UDASubSectorID = subSectorIdMax;
                }

                if (updateDataObj.UDASecurityTypeID == -1 && securityTypeID.Contains(updateDataObj.UDASecurityType.Trim()))
                {
                    int k = securityTypeID.GetUDAId(updateDataObj.UDASecurityType.Trim());
                    updateDataObj.UDASecurityTypeID = k;
                }
                else if (updateDataObj.UDASecurityTypeID == -1)
                {
                    securityTypeID.Add(new UDA
                    {
                        ID = ++securityTypeIdMax,
                        Name = updateDataObj.UDASecurityType.Trim()
                    });
                    updateDataObj.UDASecurityTypeID = securityTypeIdMax;
                }

                if (updateDataObj.UDACountryID == -1 && countryID.Contains(updateDataObj.UDACountry.Trim()))
                {
                    int k = countryID.GetUDAId(updateDataObj.UDACountry.Trim());
                    updateDataObj.UDACountryID = k;
                }
                else if (updateDataObj.UDACountryID == -1)
                {
                    countryID.Add(new UDA
                    {
                        ID = ++countryIdMax,
                        Name = updateDataObj.UDACountry.Trim()
                    });
                    updateDataObj.UDACountryID = countryIdMax;
                }
            }
            if (assetID.Count > 0)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(SecMasterConstants.CONST_INSERT_UDAASSET_SP, assetID);
                udaData.Add(SecMasterConstants.UDATypes.AssetClass.ToString(), dict);
            }
            if (securityTypeID.Count > 0)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(SecMasterConstants.CONST_INSERT_UDASecurityTypes_SP, securityTypeID);
                udaData.Add(SecMasterConstants.UDATypes.SecurityType.ToString(), dict);
            }
            if (sectorID.Count > 0)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(SecMasterConstants.CONST_INSERT_UDASectors_SP, sectorID);
                udaData.Add(SecMasterConstants.UDATypes.Sector.ToString(), dict);
            }
            if (subSectorID.Count > 0)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(SecMasterConstants.CONST_INSERT_UDASubSectors_SP, subSectorID);
                udaData.Add(SecMasterConstants.UDATypes.SubSector.ToString(), dict);
            }
            if (countryID.Count > 0)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(SecMasterConstants.CONST_INSERT_UDACountries_SP, countryID);
                udaData.Add(SecMasterConstants.UDATypes.Country.ToString(), dict);
            }
            if (udaData.Count > 0)
            {
                SaveUDAData(udaData);
            }
        }
        /// <summary>
        /// Saving Newly Added UDAs by SMImport.
        /// </summary>
        /// <param name="_dsSecMasterInsert"></param>
        private void SaveNewlyAddedUDAsBySMImport(DataSet _dsSecMasterInsert)
        {
            DataTable dtSM = _dsSecMasterInsert.Tables[0];
            Dictionary<String, Dictionary<string, object>> udaData = new Dictionary<String, Dictionary<string, object>>();
            UDACollection assetID = new UDACollection();
            UDACollection sectorID = new UDACollection();
            UDACollection subSectorID = new UDACollection();
            UDACollection countryID = new UDACollection();
            UDACollection securityTypeID = new UDACollection();
            int assetIdMax = GetMaxId(SecMasterHelper.getInstance().DictUdaAsset);
            int sectorIdMax = GetMaxId(SecMasterHelper.getInstance().DictUdaSector);
            int subSectorIdMax = GetMaxId(SecMasterHelper.getInstance().DictUdaSubSector);
            int countryIdMax = GetMaxId(SecMasterHelper.getInstance().DictUdaCountries);
            int securityTypeIdMax = GetMaxId(SecMasterHelper.getInstance().DictUdaSecurityType);

            foreach (DataRow row in dtSM.Rows)
            {
                if (dtSM.Columns.Contains("UDAAssetClassID"))
                {
                    ValidateUDAs("UDAAssetClassID", SecMasterHelper.getInstance().DictUdaAsset, row, assetID, assetIdMax);
                }
                if (dtSM.Columns.Contains("UDASectorID"))
                {
                    ValidateUDAs("UDASectorID", SecMasterHelper.getInstance().DictUdaSector, row, sectorID, sectorIdMax);
                }
                if (dtSM.Columns.Contains("UDASubSectorID"))
                {
                    ValidateUDAs("UDASubSectorID", SecMasterHelper.getInstance().DictUdaSubSector, row, subSectorID, subSectorIdMax);
                }
                if (dtSM.Columns.Contains("UDASecurityTypeID"))
                {
                    ValidateUDAs("UDASecurityTypeID", SecMasterHelper.getInstance().DictUdaSecurityType, row, securityTypeID, securityTypeIdMax);
                }
                if (dtSM.Columns.Contains("UDACountryID"))
                {
                    ValidateUDAs("UDACountryID", SecMasterHelper.getInstance().DictUdaCountries, row, countryID, countryIdMax);
                }
            }
            if (assetID.Count > 0)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(SecMasterConstants.CONST_INSERT_UDAASSET_SP, assetID);
                udaData.Add(SecMasterConstants.UDATypes.AssetClass.ToString(), dict);
            }
            if (securityTypeID.Count > 0)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(SecMasterConstants.CONST_INSERT_UDASecurityTypes_SP, securityTypeID);
                udaData.Add(SecMasterConstants.UDATypes.SecurityType.ToString(), dict);
            }
            if (sectorID.Count > 0)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(SecMasterConstants.CONST_INSERT_UDASectors_SP, sectorID);
                udaData.Add(SecMasterConstants.UDATypes.Sector.ToString(), dict);
            }
            if (subSectorID.Count > 0)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(SecMasterConstants.CONST_INSERT_UDASubSectors_SP, subSectorID);
                udaData.Add(SecMasterConstants.UDATypes.SubSector.ToString(), dict);
            }
            if (countryID.Count > 0)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add(SecMasterConstants.CONST_INUSED_UDACountries_SP, countryID);
                udaData.Add(SecMasterConstants.UDATypes.Country.ToString(), dict);
            }
            if (udaData.Count > 0)
            {
                SaveUDAData(udaData);
            }


        }

        /// <summary>
        /// Handle UDA attributes response
        /// </summary>
        /// <param name="UDADataCol"></param>
        void _securityMaster_UDAAttributesResponse(object sender, EventArgs<Dictionary<string, Dictionary<int, string>>> e)
        {
            try
            {
                SecMasterHelper.getInstance().SetUDAValueLists(e.Value);
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

        /// <summary>
        /// Save UDA Data.
        /// </summary>
        /// <param name="udaData"></param>
        void SaveUDAData(Dictionary<String, Dictionary<string, object>> udaData)
        {
            try
            {
                if (_securityMaster.IsConnected)
                {

                    _securityMaster.SaveUDAData(udaData);
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
        /// Update DataTable Column name and values.
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateDataTableColumnNameandValues(DataSet ds)
        {
            try
            {
                DataTable dtSM = ds.Tables[0];
                #region UDA
                if (dtSM.Columns.Contains("UDAAssetClass"))
                {
                    dtSM.Columns["UDAAssetClass"].ColumnName = "UDAAssetClassID";
                    DataColumn udaAssetClass = new DataColumn("UDAAssetClass");
                    udaAssetClass.DataType = typeof(string);
                    udaAssetClass.DefaultValue = String.Empty;
                    dtSM.Columns.Add(udaAssetClass);
                }
                if (dtSM.Columns.Contains("UDASector"))
                {
                    dtSM.Columns["UDASector"].ColumnName = "UDASectorID";
                    DataColumn udaSector = new DataColumn("UDASector");
                    udaSector.DataType = typeof(string);
                    udaSector.DefaultValue = String.Empty;
                    dtSM.Columns.Add(udaSector);
                }
                if (dtSM.Columns.Contains("UDASubSector"))
                {
                    dtSM.Columns["UDASubSector"].ColumnName = "UDASubSectorID";
                    DataColumn udaSubSector = new DataColumn("UDASubSector");
                    udaSubSector.DataType = typeof(string);
                    udaSubSector.DefaultValue = String.Empty;
                    dtSM.Columns.Add(udaSubSector);
                }
                if (dtSM.Columns.Contains("UDASecurityType"))
                {
                    dtSM.Columns["UDASecurityType"].ColumnName = "UDASecurityTypeID";
                    DataColumn udaSecurityType = new DataColumn("UDASecurityType");
                    udaSecurityType.DataType = typeof(string);
                    udaSecurityType.DefaultValue = String.Empty;
                    dtSM.Columns.Add(udaSecurityType);
                }
                if (dtSM.Columns.Contains("UDACountry"))
                {
                    dtSM.Columns["UDACountry"].ColumnName = "UDACountryID";
                    DataColumn udaCountry = new DataColumn("UDACountry");
                    udaCountry.DataType = typeof(string);
                    udaCountry.DefaultValue = String.Empty;
                    dtSM.Columns.Add(udaCountry);
                }
                if (!dtSM.Columns.Contains("ValidationError"))
                {
                    DataColumn udaCountryID = new DataColumn("ValidationError");
                    udaCountryID.DataType = typeof(string);
                    udaCountryID.DefaultValue = String.Empty;
                    dtSM.Columns.Add(udaCountryID);
                }
                SetDynamicUDA(_dynamicUDACache, dtSM, _importTypeSecMasterInsert);
                #endregion
                //
                foreach (DataRow row in dtSM.Rows)
                {
                    if (dtSM.Columns.Contains("IsNDF"))
                    {
                        if (!row["IsNDF"].ToString().ToUpper().Equals("TRUE") && !row["IsNDF"].ToString().ToUpper().Equals("FALSE"))
                        {
                            row["IsNDF"] = false;
                        }
                    }
                    if (dtSM.Columns.Contains("IsZERO"))
                    {
                        if (!row["IsZERO"].ToString().ToUpper().Equals("TRUE") && !row["IsZERO"].ToString().ToUpper().Equals("FALSE"))
                        {
                            row["IsZERO"] = false;
                        }
                    }
                    if (dtSM.Columns.Contains("UDAAssetClassID"))
                    {
                        if (!string.IsNullOrWhiteSpace(row["UDAAssetClassID"].ToString())
                                   && SecMasterHelper.getInstance().DictUdaAsset.Where(x => x.Value.ToUpper() == row["UDAAssetClassID"].ToString().Trim().ToUpper()).Count() > 0)
                        {
                            row["UDAAssetClass"] = row["UDAAssetClassID"];
                            int k = SecMasterHelper.getInstance().DictUdaAsset.FirstOrDefault(x => x.Value.ToUpper() == row["UDAAssetClassID"].ToString().Trim().ToUpper()).Key;
                            row["UDAAssetClassID"] = k;
                        }
                        else if (!string.IsNullOrWhiteSpace(row["UDAAssetClassID"].ToString()))
                        {
                            row["UDAAssetClass"] = row["UDAAssetClassID"];
                            //Inserting -1 in case if a new value is inserted which is not present in Master Table
                            row["UDAAssetClassID"] = -1;
                        }
                        else
                        {
                            //Inserting int.MinValue in case if a some one has inserted blank value.
                            //It will be reflected as Undefined but will not save in DB and not reflect on SymbolLook UI.
                            row["UDAAssetClass"] = "Undefined";
                            row["UDAAssetClassID"] = int.MinValue;
                        }
                    }
                    if (dtSM.Columns.Contains("UDASectorID"))
                    {
                        if (!string.IsNullOrWhiteSpace(row["UDASectorID"].ToString())
                                    && SecMasterHelper.getInstance().DictUdaSector.Where(x => x.Value.ToUpper() == row["UDASectorID"].ToString().Trim().ToUpper()).Count() > 0)
                        {
                            row["UDASector"] = row["UDASectorID"];
                            int k = SecMasterHelper.getInstance().DictUdaSector.FirstOrDefault(x => x.Value.ToUpper() == row["UDASectorID"].ToString().Trim().ToUpper()).Key;
                            row["UDASectorID"] = k;
                        }
                        else if (!string.IsNullOrWhiteSpace(row["UDASectorID"].ToString()))
                        {
                            row["UDASector"] = row["UDASectorID"];
                            row["UDASectorID"] = -1;
                        }
                        else
                        {
                            row["UDASector"] = "Undefined";
                            row["UDASectorID"] = int.MinValue;
                        }
                    }
                    if (dtSM.Columns.Contains("UDASubSectorID"))
                    {
                        if (!string.IsNullOrWhiteSpace(row["UDASubSectorID"].ToString())
                                   && SecMasterHelper.getInstance().DictUdaSubSector.Where(x => x.Value.ToUpper() == row["UDASubSectorID"].ToString().Trim().ToUpper()).Count() > 0)
                        {
                            row["UDASubSector"] = row["UDASubSectorID"];
                            int k = SecMasterHelper.getInstance().DictUdaSubSector.FirstOrDefault(x => x.Value.ToUpper() == row["UDASubSectorID"].ToString().Trim().ToUpper()).Key;
                            row["UDASubSectorID"] = k;
                        }
                        else if (!string.IsNullOrWhiteSpace(row["UDASubSectorID"].ToString()))
                        {
                            row["UDASubSector"] = row["UDASubSectorID"];
                            row["UDASubSectorID"] = -1;
                        }
                        else
                        {
                            row["UDASubSector"] = "Undefined";
                            row["UDASubSectorID"] = int.MinValue;
                        }
                    }
                    if (dtSM.Columns.Contains("UDASecurityTypeID"))
                    {
                        if (!string.IsNullOrWhiteSpace(row["UDASecurityTypeID"].ToString())
                                   && SecMasterHelper.getInstance().DictUdaSecurityType.Where(x => x.Value.ToUpper() == row["UDASecurityTypeID"].ToString().Trim().ToUpper()).Count() > 0)
                        {
                            row["UDASecurityType"] = row["UDASecurityTypeID"];
                            int k = SecMasterHelper.getInstance().DictUdaSecurityType.FirstOrDefault(x => x.Value.ToUpper() == row["UDASecurityTypeID"].ToString().Trim().ToUpper()).Key;
                            row["UDASecurityTypeID"] = k;
                        }
                        else if (!string.IsNullOrWhiteSpace(row["UDASecurityTypeID"].ToString()))
                        {
                            row["UDASecurityType"] = row["UDASecurityTypeID"];
                            row["UDASecurityTypeID"] = -1;
                        }
                        else
                        {
                            row["UDASecurityType"] = "Undefined";
                            row["UDASecurityTypeID"] = int.MinValue;
                        }
                    }
                    if (dtSM.Columns.Contains("UDACountryID"))
                    {
                        if (!string.IsNullOrWhiteSpace(row["UDACountryID"].ToString())
                                   && SecMasterHelper.getInstance().DictUdaCountries.Where(x => x.Value.ToUpper() == row["UDACountryID"].ToString().Trim().ToUpper()).Count() > 0)
                        {
                            row["UDACountry"] = row["UDACountryID"];
                            int k = SecMasterHelper.getInstance().DictUdaCountries.FirstOrDefault(x => x.Value.ToUpper() == row["UDACountryID"].ToString().Trim().ToUpper()).Key;
                            row["UDACountryID"] = k;
                        }
                        else if (!string.IsNullOrWhiteSpace(row["UDACountryID"].ToString()))
                        {
                            row["UDACountry"] = row["UDACountryID"];
                            row["UDACountryID"] = -1;
                        }
                        else
                        {
                            row["UDACountry"] = "Undefined";
                            row["UDACountryID"] = int.MinValue;
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

        public bool AddDailyDataAuditEntryForDividendImport(DataTable dt, TradeAuditActionType.ActionType action, string comment, int currentUserID)
        {
            try
            {
                if (dt != null && comment != null)
                {
                    List<TradeAuditEntry> _auditCollection_DividendImport = new List<TradeAuditEntry>();
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(row["Symbol"].ToString()))
                        {
                            TradeAuditEntry newEntry = new TradeAuditEntry();
                            newEntry.Action = action;
                            newEntry.AUECLocalDate = DateTime.Now;
                            newEntry.OriginalDate = DateTime.Parse(row["ExDate"].ToString());
                            newEntry.Comment = comment;
                            newEntry.CompanyUserId = currentUserID;
                            newEntry.Symbol = row["Symbol"].ToString();
                            newEntry.Level1ID = Convert.ToInt32(row["FundID"].ToString());
                            newEntry.GroupID = int.MinValue.ToString();
                            newEntry.TaxLotClosingId = "";
                            newEntry.TaxLotID = "";
                            newEntry.OrderSideTagValue = "";
                            newEntry.OriginalValue = "0";
                            newEntry.Source = TradeAuditActionType.ActionSource.Import;
                            _auditCollection_DividendImport.Add(newEntry);
                        }
                        else
                        {
                            TradeAuditEntry newEntry = new TradeAuditEntry();
                            newEntry.Action = action;
                            newEntry.AUECLocalDate = DateTime.Now;
                            newEntry.OriginalDate = DateTime.Parse(row["ExDate"].ToString());
                            newEntry.Comment = comment;
                            newEntry.CompanyUserId = currentUserID;
                            newEntry.Symbol = CachedDataManager.GetInstance.GetAllCurrencies()[Convert.ToInt32(row["CurrencyID"])];
                            newEntry.Level1ID = Convert.ToInt32(row["FundID"].ToString());
                            newEntry.GroupID = int.MinValue.ToString();
                            newEntry.TaxLotClosingId = "";
                            newEntry.TaxLotID = "";
                            newEntry.OrderSideTagValue = "";
                            newEntry.OriginalValue = "0";
                            newEntry.Source = TradeAuditActionType.ActionSource.Import;
                            _auditCollection_DividendImport.Add(newEntry);
                        }
                    }

                    if (_auditCollection_DividendImport.Count > 0)
                        AuditManager.Instance.SaveAuditList(_auditCollection_DividendImport);

                    _auditCollection_DividendImport.Clear();
                }
                else
                    throw new NullReferenceException("The Data Table to add in audit dictionary is null");
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

        /// <summary>
        /// Groups Staged Orders and generates allocation Scheme based on the orders
        /// </summary>
        /// <param name="FileName">Name of imported file</param>
        private void GroupStagedOrders(string FileName)
        {
           _importHelper.GroupStagedOrders(FileName, ref _dsAllocationScheme);
           _stageOrderCollection = _importHelper.StageOrderCollection;
        }

        private void AddDailyDataAuditEntryForFXRate(List<ForexPriceImport> _forexPriceValueCollection, TradeAuditActionType.ActionType actionType, string comment, int currentUserID)
        {
            try
            {
                string oldValues = RunUploadManager.GetOldValues();
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(oldValues))
                {
                    string oldValuesFormatted = "<DeletedFXRates>" + oldValues + "</DeletedFXRates>";
                    StringReader theReader = new StringReader(oldValuesFormatted);
                    ds.ReadXml(theReader);
                }

                if (_forexPriceValueCollection != null && comment != null)
                {
                    Dictionary<int, string> accounts = CachedDataManager.GetInstance.GetAccounts();
                    foreach (ForexPriceImport FPI in _forexPriceValueCollection)
                    {
                        if (FPI.AccountID != 0)
                        {
                            TradeAuditEntry newEntry = new TradeAuditEntry();
                            newEntry.Action = actionType;
                            newEntry.AUECLocalDate = DateTime.Now;
                            newEntry.OriginalDate = DateTime.Parse(FPI.Date);
                            newEntry.Comment = comment;
                            newEntry.CompanyUserId = currentUserID;
                            newEntry.Level1ID = FPI.AccountID;
                            newEntry.GroupID = int.MinValue.ToString();
                            newEntry.TaxLotClosingId = "";
                            newEntry.TaxLotID = "";
                            newEntry.OrderSideTagValue = "";
                            newEntry.OriginalValue = "0";

                            if (FPI.BaseCurrency.Equals("USD"))
                            {
                                newEntry.Symbol = FPI.SettlementCurrency + " A0-FX";
                            }
                            else if (FPI.SettlementCurrency.Equals("USD"))
                            {
                                newEntry.Symbol = FPI.BaseCurrency + " A0-FX";
                            }
                            else
                            {
                                newEntry.Symbol = FPI.BaseCurrency + FPI.SettlementCurrency + " A0-FX";
                            }

                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    double conversionRate = 0;
                                    DateTime date = DateTime.Now;
                                    double.TryParse(dr["ConversionRate"].ToString(), out conversionRate);
                                    DateTime.TryParse(dr["Date"].ToString(), out date);
                                    if ((dr["FromCurrencyID"].ToString().Equals(FPI.BaseCurrencyID.ToString())) && (dr["ToCurrencyID"].ToString().Equals(FPI.SettlementCurrencyID.ToString())) && DateTime.Compare(date, DateTime.Parse(FPI.Date)) == 0 && (dr["AccountID"].ToString().Equals(FPI.AccountID.ToString())))
                                    {
                                        newEntry.OriginalValue = conversionRate.ToString();
                                        break;
                                    }
                                }
                            }
                            newEntry.Source = TradeAuditActionType.ActionSource.Import;
                            _auditCollection_FXRate.Add(newEntry);
                        }
                        else
                        {
                            foreach (KeyValuePair<int, string> pair in accounts)
                            {
                                TradeAuditEntry newEntry = new TradeAuditEntry();
                                newEntry.Action = actionType;
                                newEntry.AUECLocalDate = DateTime.Now;
                                newEntry.OriginalDate = DateTime.Parse(FPI.Date);
                                newEntry.Comment = comment;
                                newEntry.CompanyUserId = currentUserID;
                                newEntry.Level1ID = pair.Key;
                                newEntry.GroupID = int.MinValue.ToString();
                                newEntry.TaxLotClosingId = "";
                                newEntry.TaxLotID = "";
                                newEntry.OrderSideTagValue = "";
                                newEntry.OriginalValue = "0";

                                if (FPI.BaseCurrency.Equals("USD"))
                                {
                                    newEntry.Symbol = FPI.SettlementCurrency + " A0-FX";
                                }
                                else if (FPI.SettlementCurrency.Equals("USD"))
                                {
                                    newEntry.Symbol = FPI.BaseCurrency + " A0-FX";
                                }
                                else
                                {
                                    newEntry.Symbol = FPI.BaseCurrency + FPI.SettlementCurrency + " A0-FX";
                                }

                                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                    {
                                        double conversionRate = 0;
                                        DateTime date = DateTime.Now;
                                        double.TryParse(dr["ConversionRate"].ToString(), out conversionRate);
                                        DateTime.TryParse(dr["Date"].ToString(), out date);
                                        if ((dr["FromCurrencyID"].ToString().Equals(FPI.BaseCurrencyID.ToString())) && (dr["ToCurrencyID"].ToString().Equals(FPI.SettlementCurrencyID.ToString())) && DateTime.Compare(date, DateTime.Parse(FPI.Date)) == 0 && (dr["AccountID"].ToString().Equals(FPI.AccountID.ToString())))
                                        {
                                            newEntry.OriginalValue = conversionRate.ToString();
                                            break;
                                        }
                                    }
                                }
                                newEntry.Source = TradeAuditActionType.ActionSource.Import;
                                _auditCollection_FXRate.Add(newEntry);
                            }
                        }
                    }
                }
                else
                    throw new NullReferenceException("The Collection to add in audit dictionary is null");
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
        public bool AddDailyDataAuditEntryForMarkPrice(DataTable dt, TradeAuditActionType.ActionType action, string comment, int currentUserID)
        {
            try
            {
                if (dt != null && comment != null)
                {
                    Dictionary<int, string> accounts = CachedDataManager.GetInstance.GetAccounts();
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["AccountID"].ToString() != "0")
                        {
                            TradeAuditEntry newEntry = new TradeAuditEntry();
                            newEntry.Action = action;
                            newEntry.AUECLocalDate = DateTime.Now;
                            newEntry.OriginalDate = DateTime.Parse(row["Date"].ToString());
                            newEntry.Comment = comment;
                            newEntry.CompanyUserId = currentUserID;
                            newEntry.Symbol = row["Symbol"].ToString();
                            newEntry.Level1ID = Convert.ToInt32(row["AccountID"].ToString());
                            newEntry.GroupID = int.MinValue.ToString();
                            newEntry.TaxLotClosingId = "";
                            newEntry.TaxLotID = "";
                            newEntry.OrderSideTagValue = "";
                            newEntry.OriginalValue = "0";
                            newEntry.Source = TradeAuditActionType.ActionSource.Import;
                            _auditCollection_MarkPrice.Add(newEntry);
                        }
                        else
                        {
                            foreach (KeyValuePair<int, string> pair in accounts)
                            {
                                TradeAuditEntry newEntry = new TradeAuditEntry();
                                newEntry.Action = action;
                                newEntry.AUECLocalDate = DateTime.Now;
                                newEntry.OriginalDate = DateTime.Parse(row["Date"].ToString());
                                newEntry.Comment = comment;
                                newEntry.CompanyUserId = currentUserID;
                                newEntry.Symbol = row["Symbol"].ToString();
                                newEntry.Level1ID = pair.Key;
                                newEntry.GroupID = int.MinValue.ToString();
                                newEntry.TaxLotClosingId = "";
                                newEntry.TaxLotID = "";
                                newEntry.OrderSideTagValue = "";
                                newEntry.OriginalValue = "0";
                                newEntry.Source = TradeAuditActionType.ActionSource.Import;
                                _auditCollection_MarkPrice.Add(newEntry);
                            }
                        }
                    }
                }
                else
                    throw new NullReferenceException("The Data Table to add in audit dictionary is null");
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
        private DataTable CreateDataTableForBetaImport()
        {
            DataTable dtBetaNew = new DataTable();
            dtBetaNew.TableName = "DailyBeta";

            DataColumn colSymbol = new DataColumn("Symbol", typeof(string));
            DataColumn colDate = new DataColumn("Date", typeof(string));
            DataColumn colBetaPrice = new DataColumn("Beta", typeof(double));
            DataColumn colAUECID = new DataColumn("AUECID", typeof(Int32));

            dtBetaNew.Columns.Add(colSymbol);
            dtBetaNew.Columns.Add(colDate);
            dtBetaNew.Columns.Add(colBetaPrice);
            dtBetaNew.Columns.Add(colAUECID);

            return dtBetaNew;
        }


        Dictionary<int, Dictionary<string, List<BetaImport>>> _betaSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<BetaImport>>>();

        void UpdateBetaValueCollection(DataSet ds)
        {
            try
            {
                _betaSymbologyWiseDict.Clear();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(BetaImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    BetaImport betaValue = new BetaImport();
                    betaValue.Symbol = string.Empty;
                    betaValue.Beta = 0;
                    betaValue.Date = string.Empty;
                    betaValue.AUECID = 0;

                    for (int icol = 0; icol < dTable.Columns.Count; icol++)
                    {
                        string colName = dTable.Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(dTable.Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(betaValue, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(betaValue, dTable.Rows[irow][icol].ToString().Trim(), null);
                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(betaValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result;
                                    blnIsTrue = double.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(betaValue, Convert.ToDouble(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(betaValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(betaValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result;
                                    blnIsTrue = int.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(betaValue, Convert.ToInt32(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(betaValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(betaValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result;
                                    blnIsTrue = Int64.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(betaValue, Convert.ToInt64(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(betaValue, 0, null);
                                    }
                                }
                            }
                        }
                    }


                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        betaValue.Date = _userSelectedDate;
                    }
                    else if (!betaValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(betaValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(betaValue.Date));
                            betaValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(betaValue.Date);
                            betaValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }

                    string uniqueID = string.Empty;

                    if (!String.IsNullOrEmpty(betaValue.Symbol))
                    {
                        uniqueID = betaValue.Date + betaValue.Symbol.Trim().ToUpper();
                        if (_betaSymbologyWiseDict.ContainsKey(0))
                        {
                            Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[0];
                            if (betaSameSymbologyDict.ContainsKey(betaValue.Symbol))
                            {
                                List<BetaImport> betaSymbolWiseList = betaSameSymbologyDict[betaValue.Symbol];
                                betaSymbolWiseList.Add(betaValue);
                                betaSameSymbologyDict[betaValue.Symbol] = betaSymbolWiseList;
                                _betaSymbologyWiseDict[0] = betaSameSymbologyDict;
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                _betaSymbologyWiseDict[0].Add(betaValue.Symbol, betalist);
                            }
                        }
                        else
                        {
                            List<BetaImport> betalist = new List<BetaImport>();
                            betalist.Add(betaValue);
                            Dictionary<string, List<BetaImport>> betaSameSymbolDict = new Dictionary<string, List<BetaImport>>();
                            betaSameSymbolDict.Add(betaValue.Symbol, betalist);
                            _betaSymbologyWiseDict.Add(0, betaSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(betaValue.RIC))
                    {
                        uniqueID = betaValue.Date + betaValue.RIC.Trim().ToUpper();
                        if (_betaSymbologyWiseDict.ContainsKey(1))
                        {
                            Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[1];
                            if (betaSameSymbologyDict.ContainsKey(betaValue.RIC))
                            {
                                List<BetaImport> betaRICWiseList = betaSameSymbologyDict[betaValue.RIC];
                                betaRICWiseList.Add(betaValue);
                                betaSameSymbologyDict[betaValue.RIC] = betaRICWiseList;
                                _betaSymbologyWiseDict[1] = betaSameSymbologyDict;
                            }
                            else
                            {
                                List<BetaImport> betaList = new List<BetaImport>();
                                betaList.Add(betaValue);
                                _betaSymbologyWiseDict[1].Add(betaValue.RIC, betaList);
                            }
                        }
                        else
                        {
                            List<BetaImport> betaList = new List<BetaImport>();
                            betaList.Add(betaValue);
                            Dictionary<string, List<BetaImport>> betaSameRICDict = new Dictionary<string, List<BetaImport>>();
                            betaSameRICDict.Add(betaValue.RIC, betaList);
                            _betaSymbologyWiseDict.Add(1, betaSameRICDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(betaValue.ISIN))
                    {
                        uniqueID = betaValue.Date + betaValue.ISIN.Trim().ToUpper();
                        if (_betaSymbologyWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[2];
                            if (betaSameSymbologyDict.ContainsKey(betaValue.ISIN))
                            {
                                List<BetaImport> betaISINWiseList = betaSameSymbologyDict[betaValue.ISIN];
                                betaISINWiseList.Add(betaValue);
                                betaSameSymbologyDict[betaValue.ISIN] = betaISINWiseList;
                                _betaSymbologyWiseDict[2] = betaSameSymbologyDict;
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                _betaSymbologyWiseDict[2].Add(betaValue.ISIN, betalist);
                            }
                        }
                        else
                        {
                            List<BetaImport> betalist = new List<BetaImport>();
                            betalist.Add(betaValue);
                            Dictionary<string, List<BetaImport>> betaSameISINDict = new Dictionary<string, List<BetaImport>>();
                            betaSameISINDict.Add(betaValue.ISIN, betalist);
                            _betaSymbologyWiseDict.Add(2, betaSameISINDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(betaValue.SEDOL))
                    {
                        uniqueID = betaValue.Date + betaValue.SEDOL.Trim().ToUpper();
                        if (_betaSymbologyWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[3];
                            if (betaSameSymbologyDict.ContainsKey(betaValue.SEDOL))
                            {
                                List<BetaImport> betaSEDOLWiseList = betaSameSymbologyDict[betaValue.SEDOL];
                                betaSEDOLWiseList.Add(betaValue);
                                betaSameSymbologyDict[betaValue.SEDOL] = betaSEDOLWiseList;
                                _betaSymbologyWiseDict[3] = betaSameSymbologyDict;
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                _betaSymbologyWiseDict[3].Add(betaValue.SEDOL, betalist);
                            }
                        }
                        else
                        {
                            List<BetaImport> betalist = new List<BetaImport>();
                            betalist.Add(betaValue);
                            Dictionary<string, List<BetaImport>> betaSEDOLDict = new Dictionary<string, List<BetaImport>>();
                            betaSEDOLDict.Add(betaValue.SEDOL, betalist);
                            _betaSymbologyWiseDict.Add(3, betaSEDOLDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(betaValue.CUSIP))
                    {
                        uniqueID = betaValue.Date + betaValue.CUSIP.Trim().ToUpper();
                        if (_betaSymbologyWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[4];
                            if (betaSameSymbologyDict.ContainsKey(betaValue.CUSIP))
                            {
                                List<BetaImport> betaCUSIPWiseList = betaSameSymbologyDict[betaValue.CUSIP];
                                betaCUSIPWiseList.Add(betaValue);
                                betaSameSymbologyDict[betaValue.CUSIP] = betaCUSIPWiseList;
                                _betaSymbologyWiseDict[4] = betaSameSymbologyDict;
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                _betaSymbologyWiseDict[4].Add(betaValue.CUSIP, betalist);
                            }
                        }
                        else
                        {
                            List<BetaImport> betalist = new List<BetaImport>();
                            betalist.Add(betaValue);
                            Dictionary<string, List<BetaImport>> betaSameCUSIPDict = new Dictionary<string, List<BetaImport>>();
                            betaSameCUSIPDict.Add(betaValue.CUSIP, betalist);
                            _betaSymbologyWiseDict.Add(4, betaSameCUSIPDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(betaValue.Bloomberg))
                    {
                        uniqueID = betaValue.Date + betaValue.Bloomberg.Trim().ToUpper();
                        if (_betaSymbologyWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[5];
                            if (betaSameSymbologyDict.ContainsKey(betaValue.Bloomberg))
                            {
                                List<BetaImport> betaBloombergWiseList = betaSameSymbologyDict[betaValue.Bloomberg];
                                betaBloombergWiseList.Add(betaValue);
                                betaSameSymbologyDict[betaValue.Bloomberg] = betaBloombergWiseList;
                                _betaSymbologyWiseDict[5] = betaSameSymbologyDict;
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                _betaSymbologyWiseDict[5].Add(betaValue.Bloomberg, betalist);
                            }
                        }
                        else
                        {
                            List<BetaImport> betalist = new List<BetaImport>();
                            betalist.Add(betaValue);
                            Dictionary<string, List<BetaImport>> betaSameBloombergDict = new Dictionary<string, List<BetaImport>>();
                            betaSameBloombergDict.Add(betaValue.Bloomberg, betalist);
                            _betaSymbologyWiseDict.Add(5, betaSameBloombergDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(betaValue.OSIOptionSymbol))
                    {
                        uniqueID = betaValue.Date + betaValue.OSIOptionSymbol.Trim().ToUpper();
                        if (_betaSymbologyWiseDict.ContainsKey(6))
                        {
                            Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[6];
                            if (betaSameSymbologyDict.ContainsKey(betaValue.OSIOptionSymbol))
                            {
                                List<BetaImport> betaOSIWiseList = betaSameSymbologyDict[betaValue.OSIOptionSymbol];
                                betaOSIWiseList.Add(betaValue);
                                betaSameSymbologyDict[betaValue.OSIOptionSymbol] = betaOSIWiseList;
                                _betaSymbologyWiseDict[6] = betaSameSymbologyDict;
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                _betaSymbologyWiseDict[6].Add(betaValue.OSIOptionSymbol, betalist);
                            }
                        }
                        else
                        {
                            List<BetaImport> betalist = new List<BetaImport>();
                            betalist.Add(betaValue);
                            Dictionary<string, List<BetaImport>> betaSameOSIDict = new Dictionary<string, List<BetaImport>>();
                            betaSameOSIDict.Add(betaValue.OSIOptionSymbol, betalist);
                            _betaSymbologyWiseDict.Add(6, betaSameOSIDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(betaValue.IDCOOptionSymbol))
                    {
                        uniqueID = betaValue.Date + betaValue.IDCOOptionSymbol.Trim().ToUpper();
                        if (_betaSymbologyWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[7];
                            if (betaSameSymbologyDict.ContainsKey(betaValue.IDCOOptionSymbol))
                            {
                                List<BetaImport> betaIDCOWiseList = betaSameSymbologyDict[betaValue.IDCOOptionSymbol];
                                betaIDCOWiseList.Add(betaValue);
                                betaSameSymbologyDict[betaValue.IDCOOptionSymbol] = betaIDCOWiseList;
                                _betaSymbologyWiseDict[7] = betaSameSymbologyDict;
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                _betaSymbologyWiseDict[7].Add(betaValue.IDCOOptionSymbol, betalist);
                            }
                        }
                        else
                        {
                            List<BetaImport> betalist = new List<BetaImport>();
                            betalist.Add(betaValue);
                            Dictionary<string, List<BetaImport>> betaSameIDCODict = new Dictionary<string, List<BetaImport>>();
                            betaSameIDCODict.Add(betaValue.IDCOOptionSymbol, betalist);
                            _betaSymbologyWiseDict.Add(7, betaSameIDCODict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(betaValue.OpraOptionSymbol))
                    {
                        uniqueID = betaValue.Date + betaValue.OpraOptionSymbol.Trim().ToUpper();
                        if (_betaSymbologyWiseDict.ContainsKey(8))
                        {
                            Dictionary<string, List<BetaImport>> betaSameSymbologyDict = _betaSymbologyWiseDict[8];
                            if (betaSameSymbologyDict.ContainsKey(betaValue.OpraOptionSymbol))
                            {
                                List<BetaImport> betaOpraWiseList = betaSameSymbologyDict[betaValue.OpraOptionSymbol];
                                betaOpraWiseList.Add(betaValue);
                                betaSameSymbologyDict[betaValue.OpraOptionSymbol] = betaOpraWiseList;
                                _betaSymbologyWiseDict[8] = betaSameSymbologyDict;
                            }
                            else
                            {
                                List<BetaImport> betalist = new List<BetaImport>();
                                betalist.Add(betaValue);
                                _betaSymbologyWiseDict[8].Add(betaValue.OpraOptionSymbol, betalist);
                            }
                        }
                        else
                        {
                            List<BetaImport> betalist = new List<BetaImport>();
                            betalist.Add(betaValue);
                            Dictionary<string, List<BetaImport>> betaSameOpraDict = new Dictionary<string, List<BetaImport>>();
                            betaSameOpraDict.Add(betaValue.OpraOptionSymbol, betalist);
                            _betaSymbologyWiseDict.Add(7, betaSameOpraDict);
                        }
                    }
                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _betaValueCollection.Add(betaValue);
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

        private void UpdateDayEndBaseCashByForexRate(List<ForexPriceImport> forexPriceValueCollection)
        {
            DataTable dtForexConversionTemp = new DataTable();
            dtForexConversionTemp.Columns.Add(new DataColumn("FromCurrencyID"));
            dtForexConversionTemp.Columns.Add(new DataColumn("ToCurrencyID"));
            dtForexConversionTemp.Columns.Add(new DataColumn("Date"));
            dtForexConversionTemp.Columns.Add(new DataColumn("ConversionFactor"));
            for (int counter = 0; counter < forexPriceValueCollection.Count; counter++)
            {
                DataRow drNew = dtForexConversionTemp.NewRow();
                drNew["FromCurrencyID"] = forexPriceValueCollection[counter].BaseCurrencyID;
                drNew["ToCurrencyID"] = forexPriceValueCollection[counter].SettlementCurrencyID;
                drNew["Date"] = DateTime.ParseExact(forexPriceValueCollection[counter].Date, "MM/dd/yyyy", null);
                drNew["ConversionFactor"] = forexPriceValueCollection[counter].ForexPrice;
                dtForexConversionTemp.Rows.Add(drNew);
                dtForexConversionTemp.AcceptChanges();
            }
            dtForexConversionTemp.TableName = "Table1";
            if (CashManagementServices == null)
                CreateCashManagementProxy();
            CashManagementServices.InnerChannel.UpdateDayEndBaseCashByForexRate(dtForexConversionTemp);
        }
        private DataSet UpdateDoubleEntryCashValueCollection(DataSet ds)
        {
            try
            {
                if (ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    if (!ds.Tables[0].Columns.Contains("Validated"))
                    {
                        DataColumn dcValidated = new DataColumn("Validated");
                        dcValidated.DataType = typeof(string);
                        dcValidated.DefaultValue = "Validated";
                        ds.Tables[0].Columns.Add(dcValidated);
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
            return ds;
        }
        //private DataSet UpdateActivityValueCollection(DataSet ds)
        //{
        //    DataTable dtActivity = new DataTable();
        //    try
        //    {
        //        if (ds.Tables.Count > 0 && ds.Tables[0] != null)
        //        {
        //            dtActivity = ds.Tables[0];
        //        }

        //        if (!dtActivity.Columns.Contains("Validated"))
        //        {
        //            DataColumn dcValidated = new DataColumn("Validated");
        //            dcValidated.DataType = typeof(string);
        //            dcValidated.DefaultValue = "Validated";
        //            dtActivity.Columns.Add(dcValidated);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return ds;
        //}

        private DataTable CreateDataTableForVolatilityImport()
        {
            DataTable dtVolatilityNew = new DataTable();
            dtVolatilityNew.TableName = "DailyVolatility";

            DataColumn colSymbol = new DataColumn("Symbol", typeof(string));
            DataColumn colDate = new DataColumn("Date", typeof(string));
            DataColumn colVolitilityPrice = new DataColumn("Volatility", typeof(double));
            DataColumn colAUECID = new DataColumn("AUECID", typeof(Int32));

            dtVolatilityNew.Columns.Add(colSymbol);
            dtVolatilityNew.Columns.Add(colDate);
            dtVolatilityNew.Columns.Add(colVolitilityPrice);
            dtVolatilityNew.Columns.Add(colAUECID);

            return dtVolatilityNew;
        }

        private DataTable CreateDataTableForVWAPImport()
        {
            DataTable dtVWAPNew = new DataTable();
            dtVWAPNew.TableName = "DailyVWAP";

            DataColumn colSymbol = new DataColumn("Symbol", typeof(string));
            DataColumn colDate = new DataColumn("Date", typeof(string));
            DataColumn colVWAPPrice = new DataColumn("VWAP", typeof(double));
            DataColumn colAUECID = new DataColumn("AUECID", typeof(Int32));

            dtVWAPNew.Columns.Add(colSymbol);
            dtVWAPNew.Columns.Add(colDate);
            dtVWAPNew.Columns.Add(colVWAPPrice);
            dtVWAPNew.Columns.Add(colAUECID);

            return dtVWAPNew;
        }

        private DataTable CreateDataTableForCollateralImport()
        {
            DataTable dtCollateralPriceTemp = new DataTable();
            dtCollateralPriceTemp.TableName = "DailyCollateralPrice";
            dtCollateralPriceTemp.Columns.Add(new DataColumn("Symbol", typeof(string)));
            dtCollateralPriceTemp.Columns.Add(new DataColumn("Date", typeof(string)));
            dtCollateralPriceTemp.Columns.Add(new DataColumn("FundId", typeof(Int32)));
            dtCollateralPriceTemp.Columns.Add(new DataColumn("CollateralPrice", typeof(double)));
            dtCollateralPriceTemp.Columns.Add(new DataColumn("Haircut", typeof(double)));
            dtCollateralPriceTemp.Columns.Add(new DataColumn("RebateOnMV", typeof(double)));
            dtCollateralPriceTemp.Columns.Add(new DataColumn("RebateOnCollateral", typeof(double)));

            return dtCollateralPriceTemp;
        }

        Dictionary<int, Dictionary<string, List<VolatilityImport>>> _volatilitySymbologyWiseDict = new Dictionary<int, Dictionary<string, List<VolatilityImport>>>();
        void UpdateVolatilityValueCollection(DataSet ds)
        {
            try
            {
                _volatilitySymbologyWiseDict.Clear();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(VolatilityImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    VolatilityImport volatilityValue = new VolatilityImport();
                    volatilityValue.Symbol = string.Empty;
                    volatilityValue.Volatility = 0;
                    volatilityValue.Date = string.Empty;
                    volatilityValue.AUECID = 0;

                    for (int icol = 0; icol < dTable.Columns.Count; icol++)
                    {
                        string colName = dTable.Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(dTable.Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(volatilityValue, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(volatilityValue, dTable.Rows[irow][icol].ToString().Trim(), null);
                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(volatilityValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result;
                                    blnIsTrue = double.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(volatilityValue, Convert.ToDouble(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(volatilityValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(volatilityValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result;
                                    blnIsTrue = int.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(volatilityValue, Convert.ToInt32(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(volatilityValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(volatilityValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result;
                                    blnIsTrue = Int64.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(volatilityValue, Convert.ToInt64(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(volatilityValue, 0, null);
                                    }
                                }
                            }
                        }
                    }

                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        volatilityValue.Date = _userSelectedDate;
                    }
                    else if (!volatilityValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(volatilityValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(volatilityValue.Date));
                            volatilityValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(volatilityValue.Date);
                            volatilityValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }

                    string uniqueID = string.Empty;

                    if (!String.IsNullOrEmpty(volatilityValue.Symbol))
                    {
                        uniqueID = volatilityValue.Date + volatilityValue.Symbol.Trim().ToUpper();
                        if (_volatilitySymbologyWiseDict.ContainsKey(0))
                        {
                            Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[0];
                            if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.Symbol))
                            {
                                List<VolatilityImport> volatilitySymbolWiseList = volatilitySameSymbologyDict[volatilityValue.Symbol];
                                volatilitySymbolWiseList.Add(volatilityValue);
                                volatilitySameSymbologyDict[volatilityValue.Symbol] = volatilitySymbolWiseList;
                                _volatilitySymbologyWiseDict[0] = volatilitySameSymbologyDict;
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                _volatilitySymbologyWiseDict[0].Add(volatilityValue.Symbol, volatilitylist);
                            }
                        }
                        else
                        {
                            List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                            volatilitylist.Add(volatilityValue);
                            Dictionary<string, List<VolatilityImport>> volatilitySameSymbolDict = new Dictionary<string, List<VolatilityImport>>();
                            volatilitySameSymbolDict.Add(volatilityValue.Symbol, volatilitylist);
                            _volatilitySymbologyWiseDict.Add(0, volatilitySameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(volatilityValue.RIC))
                    {
                        uniqueID = volatilityValue.Date + volatilityValue.RIC.Trim().ToUpper();
                        if (_volatilitySymbologyWiseDict.ContainsKey(1))
                        {
                            Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[1];
                            if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.RIC))
                            {
                                List<VolatilityImport> volatilityRICWiseList = volatilitySameSymbologyDict[volatilityValue.RIC];
                                volatilityRICWiseList.Add(volatilityValue);
                                volatilitySameSymbologyDict[volatilityValue.RIC] = volatilityRICWiseList;
                                _volatilitySymbologyWiseDict[1] = volatilitySameSymbologyDict;
                            }
                            else
                            {
                                List<VolatilityImport> volatilityList = new List<VolatilityImport>();
                                volatilityList.Add(volatilityValue);
                                _volatilitySymbologyWiseDict[1].Add(volatilityValue.RIC, volatilityList);
                            }
                        }
                        else
                        {
                            List<VolatilityImport> volatilityList = new List<VolatilityImport>();
                            volatilityList.Add(volatilityValue);
                            Dictionary<string, List<VolatilityImport>> volatilitySameRICDict = new Dictionary<string, List<VolatilityImport>>();
                            volatilitySameRICDict.Add(volatilityValue.RIC, volatilityList);
                            _volatilitySymbologyWiseDict.Add(1, volatilitySameRICDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(volatilityValue.ISIN))
                    {
                        uniqueID = volatilityValue.Date + volatilityValue.ISIN.Trim().ToUpper();
                        if (_volatilitySymbologyWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[2];
                            if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.ISIN))
                            {
                                List<VolatilityImport> volitilityISINWiseList = volatilitySameSymbologyDict[volatilityValue.ISIN];
                                volitilityISINWiseList.Add(volatilityValue);
                                volatilitySameSymbologyDict[volatilityValue.ISIN] = volitilityISINWiseList;
                                _volatilitySymbologyWiseDict[2] = volatilitySameSymbologyDict;
                            }
                            else
                            {
                                List<VolatilityImport> volitilitylist = new List<VolatilityImport>();
                                volitilitylist.Add(volatilityValue);
                                _volatilitySymbologyWiseDict[2].Add(volatilityValue.ISIN, volitilitylist);
                            }
                        }
                        else
                        {
                            List<VolatilityImport> volitilitylist = new List<VolatilityImport>();
                            volitilitylist.Add(volatilityValue);
                            Dictionary<string, List<VolatilityImport>> volitilitySameISINDict = new Dictionary<string, List<VolatilityImport>>();
                            volitilitySameISINDict.Add(volatilityValue.ISIN, volitilitylist);
                            _volatilitySymbologyWiseDict.Add(2, volitilitySameISINDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(volatilityValue.SEDOL))
                    {
                        uniqueID = volatilityValue.Date + volatilityValue.SEDOL.Trim().ToUpper();
                        if (_volatilitySymbologyWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[3];
                            if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.SEDOL))
                            {
                                List<VolatilityImport> volatilitySEDOLWiseList = volatilitySameSymbologyDict[volatilityValue.SEDOL];
                                volatilitySEDOLWiseList.Add(volatilityValue);
                                volatilitySameSymbologyDict[volatilityValue.SEDOL] = volatilitySEDOLWiseList;
                                _volatilitySymbologyWiseDict[3] = volatilitySameSymbologyDict;
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                _volatilitySymbologyWiseDict[3].Add(volatilityValue.SEDOL, volatilitylist);
                            }
                        }
                        else
                        {
                            List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                            volatilitylist.Add(volatilityValue);
                            Dictionary<string, List<VolatilityImport>> volatilitySEDOLDict = new Dictionary<string, List<VolatilityImport>>();
                            volatilitySEDOLDict.Add(volatilityValue.SEDOL, volatilitylist);
                            _volatilitySymbologyWiseDict.Add(3, volatilitySEDOLDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(volatilityValue.CUSIP))
                    {
                        uniqueID = volatilityValue.Date + volatilityValue.CUSIP.Trim().ToUpper();
                        if (_volatilitySymbologyWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[4];
                            if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.CUSIP))
                            {
                                List<VolatilityImport> volatilityCUSIPWiseList = volatilitySameSymbologyDict[volatilityValue.CUSIP];
                                volatilityCUSIPWiseList.Add(volatilityValue);
                                volatilitySameSymbologyDict[volatilityValue.CUSIP] = volatilityCUSIPWiseList;
                                _volatilitySymbologyWiseDict[4] = volatilitySameSymbologyDict;
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                _volatilitySymbologyWiseDict[4].Add(volatilityValue.CUSIP, volatilitylist);
                            }
                        }
                        else
                        {
                            List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                            volatilitylist.Add(volatilityValue);
                            Dictionary<string, List<VolatilityImport>> volatilitySameCUSIPDict = new Dictionary<string, List<VolatilityImport>>();
                            volatilitySameCUSIPDict.Add(volatilityValue.CUSIP, volatilitylist);
                            _volatilitySymbologyWiseDict.Add(4, volatilitySameCUSIPDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(volatilityValue.Bloomberg))
                    {
                        uniqueID = volatilityValue.Date + volatilityValue.Bloomberg.Trim().ToUpper();
                        if (_volatilitySymbologyWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[5];
                            if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.Bloomberg))
                            {
                                List<VolatilityImport> volatilityBloombergWiseList = volatilitySameSymbologyDict[volatilityValue.Bloomberg];
                                volatilityBloombergWiseList.Add(volatilityValue);
                                volatilitySameSymbologyDict[volatilityValue.Bloomberg] = volatilityBloombergWiseList;
                                _volatilitySymbologyWiseDict[5] = volatilitySameSymbologyDict;
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                _volatilitySymbologyWiseDict[5].Add(volatilityValue.Bloomberg, volatilitylist);
                            }
                        }
                        else
                        {
                            List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                            volatilitylist.Add(volatilityValue);
                            Dictionary<string, List<VolatilityImport>> volatilitySameBloombergDict = new Dictionary<string, List<VolatilityImport>>();
                            volatilitySameBloombergDict.Add(volatilityValue.Bloomberg, volatilitylist);
                            _volatilitySymbologyWiseDict.Add(5, volatilitySameBloombergDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(volatilityValue.OSIOptionSymbol))
                    {
                        uniqueID = volatilityValue.Date + volatilityValue.OSIOptionSymbol.Trim().ToUpper();
                        if (_volatilitySymbologyWiseDict.ContainsKey(6))
                        {
                            Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[6];
                            if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.OSIOptionSymbol))
                            {
                                List<VolatilityImport> volatilityOSIWiseList = volatilitySameSymbologyDict[volatilityValue.OSIOptionSymbol];
                                volatilityOSIWiseList.Add(volatilityValue);
                                volatilitySameSymbologyDict[volatilityValue.OSIOptionSymbol] = volatilityOSIWiseList;
                                _volatilitySymbologyWiseDict[6] = volatilitySameSymbologyDict;
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                _volatilitySymbologyWiseDict[6].Add(volatilityValue.OSIOptionSymbol, volatilitylist);
                            }
                        }
                        else
                        {
                            List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                            volatilitylist.Add(volatilityValue);
                            Dictionary<string, List<VolatilityImport>> volatilitySameOSIDict = new Dictionary<string, List<VolatilityImport>>();
                            volatilitySameOSIDict.Add(volatilityValue.OSIOptionSymbol, volatilitylist);
                            _volatilitySymbologyWiseDict.Add(6, volatilitySameOSIDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(volatilityValue.IDCOOptionSymbol))
                    {
                        uniqueID = volatilityValue.Date + volatilityValue.IDCOOptionSymbol.Trim().ToUpper();
                        if (_volatilitySymbologyWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[7];
                            if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.IDCOOptionSymbol))
                            {
                                List<VolatilityImport> volatilityIDCOWiseList = volatilitySameSymbologyDict[volatilityValue.IDCOOptionSymbol];
                                volatilityIDCOWiseList.Add(volatilityValue);
                                volatilitySameSymbologyDict[volatilityValue.IDCOOptionSymbol] = volatilityIDCOWiseList;
                                _volatilitySymbologyWiseDict[7] = volatilitySameSymbologyDict;
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                _volatilitySymbologyWiseDict[7].Add(volatilityValue.IDCOOptionSymbol, volatilitylist);
                            }
                        }
                        else
                        {
                            List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                            volatilitylist.Add(volatilityValue);
                            Dictionary<string, List<VolatilityImport>> volatilitySameIDCODict = new Dictionary<string, List<VolatilityImport>>();
                            volatilitySameIDCODict.Add(volatilityValue.IDCOOptionSymbol, volatilitylist);
                            _volatilitySymbologyWiseDict.Add(7, volatilitySameIDCODict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(volatilityValue.OpraOptionSymbol))
                    {
                        uniqueID = volatilityValue.Date + volatilityValue.OpraOptionSymbol.Trim().ToUpper();
                        if (_volatilitySymbologyWiseDict.ContainsKey(8))
                        {
                            Dictionary<string, List<VolatilityImport>> volatilitySameSymbologyDict = _volatilitySymbologyWiseDict[8];
                            if (volatilitySameSymbologyDict.ContainsKey(volatilityValue.OpraOptionSymbol))
                            {
                                List<VolatilityImport> volatilityOpraWiseList = volatilitySameSymbologyDict[volatilityValue.OpraOptionSymbol];
                                volatilityOpraWiseList.Add(volatilityValue);
                                volatilitySameSymbologyDict[volatilityValue.OpraOptionSymbol] = volatilityOpraWiseList;
                                _volatilitySymbologyWiseDict[8] = volatilitySameSymbologyDict;
                            }
                            else
                            {
                                List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                                volatilitylist.Add(volatilityValue);
                                _volatilitySymbologyWiseDict[8].Add(volatilityValue.OpraOptionSymbol, volatilitylist);
                            }
                        }
                        else
                        {
                            List<VolatilityImport> volatilitylist = new List<VolatilityImport>();
                            volatilitylist.Add(volatilityValue);
                            Dictionary<string, List<VolatilityImport>> volatilitySameOpraDict = new Dictionary<string, List<VolatilityImport>>();
                            volatilitySameOpraDict.Add(volatilityValue.OpraOptionSymbol, volatilitylist);
                            _volatilitySymbologyWiseDict.Add(7, volatilitySameOpraDict);
                        }
                    }
                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _volatilityValueCollection.Add(volatilityValue);
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

        Dictionary<int, Dictionary<string, List<VWAPImport>>> _vWAPSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<VWAPImport>>>();
        void UpdateVWAPValueCollection(DataSet ds)
        {
            try
            {
                _vWAPSymbologyWiseDict.Clear();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(VWAPImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    VWAPImport vWAPValue = new VWAPImport();
                    vWAPValue.Symbol = string.Empty;
                    vWAPValue.VWAP = 0;
                    vWAPValue.Date = string.Empty;
                    vWAPValue.AUECID = 0;

                    for (int icol = 0; icol < dTable.Columns.Count; icol++)
                    {
                        string colName = dTable.Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(dTable.Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(vWAPValue, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(vWAPValue, dTable.Rows[irow][icol].ToString().Trim(), null);
                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(vWAPValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result;
                                    blnIsTrue = double.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(vWAPValue, Convert.ToDouble(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(vWAPValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(vWAPValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result;
                                    blnIsTrue = int.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(vWAPValue, Convert.ToInt32(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(vWAPValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(vWAPValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result;
                                    blnIsTrue = Int64.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(vWAPValue, Convert.ToInt64(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(vWAPValue, 0, null);
                                    }
                                }
                            }
                        }
                    }

                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        vWAPValue.Date = _userSelectedDate;
                    }
                    else if (!vWAPValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(vWAPValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(vWAPValue.Date));
                            vWAPValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(vWAPValue.Date);
                            vWAPValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }

                    string uniqueID = string.Empty;

                    if (!String.IsNullOrEmpty(vWAPValue.Symbol))
                    {
                        uniqueID = vWAPValue.Date + vWAPValue.Symbol.Trim().ToUpper();
                        if (_vWAPSymbologyWiseDict.ContainsKey(0))
                        {
                            Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[0];
                            if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.Symbol))
                            {
                                List<VWAPImport> vWAPSymbolWiseList = vWAPSameSymbologyDict[vWAPValue.Symbol];
                                vWAPSymbolWiseList.Add(vWAPValue);
                                vWAPSameSymbologyDict[vWAPValue.Symbol] = vWAPSymbolWiseList;
                                _vWAPSymbologyWiseDict[0] = vWAPSameSymbologyDict;
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                _vWAPSymbologyWiseDict[0].Add(vWAPValue.Symbol, vWAPlist);
                            }
                        }
                        else
                        {
                            List<VWAPImport> vWAPlist = new List<VWAPImport>();
                            vWAPlist.Add(vWAPValue);
                            Dictionary<string, List<VWAPImport>> vWAPSameSymbolDict = new Dictionary<string, List<VWAPImport>>();
                            vWAPSameSymbolDict.Add(vWAPValue.Symbol, vWAPlist);
                            _vWAPSymbologyWiseDict.Add(0, vWAPSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(vWAPValue.RIC))
                    {
                        uniqueID = vWAPValue.Date + vWAPValue.RIC.Trim().ToUpper();
                        if (_vWAPSymbologyWiseDict.ContainsKey(1))
                        {
                            Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[1];
                            if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.RIC))
                            {
                                List<VWAPImport> vWAPRICWiseList = vWAPSameSymbologyDict[vWAPValue.RIC];
                                vWAPRICWiseList.Add(vWAPValue);
                                vWAPSameSymbologyDict[vWAPValue.RIC] = vWAPRICWiseList;
                                _vWAPSymbologyWiseDict[1] = vWAPSameSymbologyDict;
                            }
                            else
                            {
                                List<VWAPImport> vWAPList = new List<VWAPImport>();
                                vWAPList.Add(vWAPValue);
                                _vWAPSymbologyWiseDict[1].Add(vWAPValue.RIC, vWAPList);
                            }
                        }
                        else
                        {
                            List<VWAPImport> vWAPList = new List<VWAPImport>();
                            vWAPList.Add(vWAPValue);
                            Dictionary<string, List<VWAPImport>> vWAPSameRICDict = new Dictionary<string, List<VWAPImport>>();
                            vWAPSameRICDict.Add(vWAPValue.RIC, vWAPList);
                            _vWAPSymbologyWiseDict.Add(1, vWAPSameRICDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(vWAPValue.ISIN))
                    {
                        uniqueID = vWAPValue.Date + vWAPValue.ISIN.Trim().ToUpper();
                        if (_vWAPSymbologyWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[2];
                            if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.ISIN))
                            {
                                List<VWAPImport> vWAPISINWiseList = vWAPSameSymbologyDict[vWAPValue.ISIN];
                                vWAPISINWiseList.Add(vWAPValue);
                                vWAPSameSymbologyDict[vWAPValue.ISIN] = vWAPISINWiseList;
                                _vWAPSymbologyWiseDict[2] = vWAPSameSymbologyDict;
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                _vWAPSymbologyWiseDict[2].Add(vWAPValue.ISIN, vWAPlist);
                            }
                        }
                        else
                        {
                            List<VWAPImport> vWAPlist = new List<VWAPImport>();
                            vWAPlist.Add(vWAPValue);
                            Dictionary<string, List<VWAPImport>> vWAPSameISINDict = new Dictionary<string, List<VWAPImport>>();
                            vWAPSameISINDict.Add(vWAPValue.ISIN, vWAPlist);
                            _vWAPSymbologyWiseDict.Add(2, vWAPSameISINDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(vWAPValue.SEDOL))
                    {
                        uniqueID = vWAPValue.Date + vWAPValue.SEDOL.Trim().ToUpper();
                        if (_vWAPSymbologyWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[3];
                            if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.SEDOL))
                            {
                                List<VWAPImport> vWAPSEDOLWiseList = vWAPSameSymbologyDict[vWAPValue.SEDOL];
                                vWAPSEDOLWiseList.Add(vWAPValue);
                                vWAPSameSymbologyDict[vWAPValue.SEDOL] = vWAPSEDOLWiseList;
                                _vWAPSymbologyWiseDict[3] = vWAPSameSymbologyDict;
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                _vWAPSymbologyWiseDict[3].Add(vWAPValue.SEDOL, vWAPlist);
                            }
                        }
                        else
                        {
                            List<VWAPImport> vWAPlist = new List<VWAPImport>();
                            vWAPlist.Add(vWAPValue);
                            Dictionary<string, List<VWAPImport>> vWAPSEDOLDict = new Dictionary<string, List<VWAPImport>>();
                            vWAPSEDOLDict.Add(vWAPValue.SEDOL, vWAPlist);
                            _vWAPSymbologyWiseDict.Add(3, vWAPSEDOLDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(vWAPValue.CUSIP))
                    {
                        uniqueID = vWAPValue.Date + vWAPValue.CUSIP.Trim().ToUpper();
                        if (_vWAPSymbologyWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[4];
                            if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.CUSIP))
                            {
                                List<VWAPImport> vWAPCUSIPWiseList = vWAPSameSymbologyDict[vWAPValue.CUSIP];
                                vWAPCUSIPWiseList.Add(vWAPValue);
                                vWAPSameSymbologyDict[vWAPValue.CUSIP] = vWAPCUSIPWiseList;
                                _vWAPSymbologyWiseDict[4] = vWAPSameSymbologyDict;
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                _vWAPSymbologyWiseDict[4].Add(vWAPValue.CUSIP, vWAPlist);
                            }
                        }
                        else
                        {
                            List<VWAPImport> vWAPlist = new List<VWAPImport>();
                            vWAPlist.Add(vWAPValue);
                            Dictionary<string, List<VWAPImport>> vWAPSameCUSIPDict = new Dictionary<string, List<VWAPImport>>();
                            vWAPSameCUSIPDict.Add(vWAPValue.CUSIP, vWAPlist);
                            _vWAPSymbologyWiseDict.Add(4, vWAPSameCUSIPDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(vWAPValue.Bloomberg))
                    {
                        uniqueID = vWAPValue.Date + vWAPValue.Bloomberg.Trim().ToUpper();
                        if (_vWAPSymbologyWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[5];
                            if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.Bloomberg))
                            {
                                List<VWAPImport> vWAPBloombergWiseList = vWAPSameSymbologyDict[vWAPValue.Bloomberg];
                                vWAPBloombergWiseList.Add(vWAPValue);
                                vWAPSameSymbologyDict[vWAPValue.Bloomberg] = vWAPBloombergWiseList;
                                _vWAPSymbologyWiseDict[5] = vWAPSameSymbologyDict;
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                _vWAPSymbologyWiseDict[5].Add(vWAPValue.Bloomberg, vWAPlist);
                            }
                        }
                        else
                        {
                            List<VWAPImport> vWAPlist = new List<VWAPImport>();
                            vWAPlist.Add(vWAPValue);
                            Dictionary<string, List<VWAPImport>> vWAPSameBloombergDict = new Dictionary<string, List<VWAPImport>>();
                            vWAPSameBloombergDict.Add(vWAPValue.Bloomberg, vWAPlist);
                            _vWAPSymbologyWiseDict.Add(5, vWAPSameBloombergDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(vWAPValue.OSIOptionSymbol))
                    {
                        uniqueID = vWAPValue.Date + vWAPValue.OSIOptionSymbol.Trim().ToUpper();
                        if (_vWAPSymbologyWiseDict.ContainsKey(6))
                        {
                            Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[6];
                            if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.OSIOptionSymbol))
                            {
                                List<VWAPImport> vWAPOSIWiseList = vWAPSameSymbologyDict[vWAPValue.OSIOptionSymbol];
                                vWAPOSIWiseList.Add(vWAPValue);
                                vWAPSameSymbologyDict[vWAPValue.OSIOptionSymbol] = vWAPOSIWiseList;
                                _vWAPSymbologyWiseDict[6] = vWAPSameSymbologyDict;
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                _vWAPSymbologyWiseDict[6].Add(vWAPValue.OSIOptionSymbol, vWAPlist);
                            }
                        }
                        else
                        {
                            List<VWAPImport> vWAPlist = new List<VWAPImport>();
                            vWAPlist.Add(vWAPValue);
                            Dictionary<string, List<VWAPImport>> vWAPSameOSIDict = new Dictionary<string, List<VWAPImport>>();
                            vWAPSameOSIDict.Add(vWAPValue.OSIOptionSymbol, vWAPlist);
                            _vWAPSymbologyWiseDict.Add(6, vWAPSameOSIDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(vWAPValue.IDCOOptionSymbol))
                    {
                        uniqueID = vWAPValue.Date + vWAPValue.IDCOOptionSymbol.Trim().ToUpper();
                        if (_vWAPSymbologyWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[7];
                            if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.IDCOOptionSymbol))
                            {
                                List<VWAPImport> vWAPIDCOWiseList = vWAPSameSymbologyDict[vWAPValue.IDCOOptionSymbol];
                                vWAPIDCOWiseList.Add(vWAPValue);
                                vWAPSameSymbologyDict[vWAPValue.IDCOOptionSymbol] = vWAPIDCOWiseList;
                                _vWAPSymbologyWiseDict[7] = vWAPSameSymbologyDict;
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                _vWAPSymbologyWiseDict[7].Add(vWAPValue.IDCOOptionSymbol, vWAPlist);
                            }
                        }
                        else
                        {
                            List<VWAPImport> vWAPlist = new List<VWAPImport>();
                            vWAPlist.Add(vWAPValue);
                            Dictionary<string, List<VWAPImport>> vWAPSameIDCODict = new Dictionary<string, List<VWAPImport>>();
                            vWAPSameIDCODict.Add(vWAPValue.IDCOOptionSymbol, vWAPlist);
                            _vWAPSymbologyWiseDict.Add(7, vWAPSameIDCODict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(vWAPValue.OpraOptionSymbol))
                    {
                        uniqueID = vWAPValue.Date + vWAPValue.OpraOptionSymbol.Trim().ToUpper();
                        if (_vWAPSymbologyWiseDict.ContainsKey(8))
                        {
                            Dictionary<string, List<VWAPImport>> vWAPSameSymbologyDict = _vWAPSymbologyWiseDict[8];
                            if (vWAPSameSymbologyDict.ContainsKey(vWAPValue.OpraOptionSymbol))
                            {
                                List<VWAPImport> vWAPOpraWiseList = vWAPSameSymbologyDict[vWAPValue.OpraOptionSymbol];
                                vWAPOpraWiseList.Add(vWAPValue);
                                vWAPSameSymbologyDict[vWAPValue.OpraOptionSymbol] = vWAPOpraWiseList;
                                _vWAPSymbologyWiseDict[8] = vWAPSameSymbologyDict;
                            }
                            else
                            {
                                List<VWAPImport> vWAPlist = new List<VWAPImport>();
                                vWAPlist.Add(vWAPValue);
                                _vWAPSymbologyWiseDict[8].Add(vWAPValue.OpraOptionSymbol, vWAPlist);
                            }
                        }
                        else
                        {
                            List<VWAPImport> vWAPlist = new List<VWAPImport>();
                            vWAPlist.Add(vWAPValue);
                            Dictionary<string, List<VWAPImport>> vWAPSameOpraDict = new Dictionary<string, List<VWAPImport>>();
                            vWAPSameOpraDict.Add(vWAPValue.OpraOptionSymbol, vWAPlist);
                            _vWAPSymbologyWiseDict.Add(7, vWAPSameOpraDict);
                        }
                    }
                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _vWAPValueCollection.Add(vWAPValue);
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

        Dictionary<int, Dictionary<string, List<CollateralImport>>> _collateralSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<CollateralImport>>>();
        void UpdateCollateralValueCollection(DataSet ds)
        {
            try
            {
                _collateralSymbologyWiseDict.Clear();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(CollateralImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    CollateralImport collateralValue = new CollateralImport();
                    collateralValue.Symbol = string.Empty;
                    collateralValue.FundId = 0;
                    collateralValue.CollateralPrice = 0;
                    collateralValue.Haircut = 0;
                    collateralValue.RebateOnMV = 0;
                    collateralValue.RebateOnCollateral = 0;
                    collateralValue.Date = string.Empty;
                    collateralValue.AUECID = 0;

                    for (int icol = 0; icol < dTable.Columns.Count; icol++)
                    {
                        string colName = dTable.Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(dTable.Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(collateralValue, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(collateralValue, dTable.Rows[irow][icol].ToString().Trim(), null);
                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(collateralValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result;
                                    blnIsTrue = double.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(collateralValue, Convert.ToDouble(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(collateralValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(collateralValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result;
                                    blnIsTrue = int.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(collateralValue, Convert.ToInt32(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(collateralValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(collateralValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result;
                                    blnIsTrue = Int64.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(collateralValue, Convert.ToInt64(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(collateralValue, 0, null);
                                    }
                                }
                            }
                        }
                    }

                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        collateralValue.Date = _userSelectedDate;
                    }
                    else if (!collateralValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(collateralValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(collateralValue.Date));
                            collateralValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(collateralValue.Date);
                            collateralValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }

                    if (_userSelectedAccountValue != int.MinValue)
                    {
                        collateralValue.FundId = _userSelectedAccountValue;
                        collateralValue.AccountName = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                    }
                    else if (!string.IsNullOrEmpty(collateralValue.AccountName))
                    {
                        collateralValue.FundId = CachedDataManager.GetInstance.GetAccountID(collateralValue.AccountName.Trim());
                    }
                    string uniqueID = string.Empty;

                    if (!String.IsNullOrEmpty(collateralValue.Symbol))
                    {
                        uniqueID = collateralValue.Date + collateralValue.Symbol.Trim().ToUpper() + collateralValue.FundId;
                        if (_collateralSymbologyWiseDict.ContainsKey(0))
                        {
                            Dictionary<string, List<CollateralImport>> collateralSameSymbologyDict = _collateralSymbologyWiseDict[0];
                            if (collateralSameSymbologyDict.ContainsKey(collateralValue.Symbol))
                            {
                                List<CollateralImport> collateralSymbolWiseList = collateralSameSymbologyDict[collateralValue.Symbol];
                                collateralSymbolWiseList.Add(collateralValue);
                                collateralSameSymbologyDict[collateralValue.Symbol] = collateralSymbolWiseList;
                                _collateralSymbologyWiseDict[0] = collateralSameSymbologyDict;
                            }
                            else
                            {
                                List<CollateralImport> collaterallist = new List<CollateralImport>();
                                collaterallist.Add(collateralValue);
                                _collateralSymbologyWiseDict[0].Add(collateralValue.Symbol, collaterallist);
                            }
                        }
                        else
                        {
                            List<CollateralImport> collaterallist = new List<CollateralImport>();
                            collaterallist.Add(collateralValue);
                            Dictionary<string, List<CollateralImport>> collateralSameSymbolDict = new Dictionary<string, List<CollateralImport>>();
                            collateralSameSymbolDict.Add(collateralValue.Symbol, collaterallist);
                            _collateralSymbologyWiseDict.Add(0, collateralSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(collateralValue.RIC))
                    {
                        uniqueID = collateralValue.Date + collateralValue.RIC.Trim().ToUpper() + collateralValue.FundId;
                        if (_collateralSymbologyWiseDict.ContainsKey(1))
                        {
                            Dictionary<string, List<CollateralImport>> collateralSameSymbologyDict = _collateralSymbologyWiseDict[1];
                            if (collateralSameSymbologyDict.ContainsKey(collateralValue.RIC))
                            {
                                List<CollateralImport> collateralRICWiseList = collateralSameSymbologyDict[collateralValue.RIC];
                                collateralRICWiseList.Add(collateralValue);
                                collateralSameSymbologyDict[collateralValue.RIC] = collateralRICWiseList;
                                _collateralSymbologyWiseDict[1] = collateralSameSymbologyDict;
                            }
                            else
                            {
                                List<CollateralImport> collateralList = new List<CollateralImport>();
                                collateralList.Add(collateralValue);
                                _collateralSymbologyWiseDict[1].Add(collateralValue.RIC, collateralList);
                            }
                        }
                        else
                        {
                            List<CollateralImport> collateralList = new List<CollateralImport>();
                            collateralList.Add(collateralValue);
                            Dictionary<string, List<CollateralImport>> collateralSameRICDict = new Dictionary<string, List<CollateralImport>>();
                            collateralSameRICDict.Add(collateralValue.RIC, collateralList);
                            _collateralSymbologyWiseDict.Add(1, collateralSameRICDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(collateralValue.ISIN))
                    {
                        uniqueID = collateralValue.Date + collateralValue.ISIN.Trim().ToUpper() + collateralValue.FundId;
                        if (_collateralSymbologyWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<CollateralImport>> collateralSameSymbologyDict = _collateralSymbologyWiseDict[2];
                            if (collateralSameSymbologyDict.ContainsKey(collateralValue.ISIN))
                            {
                                List<CollateralImport> collateralISINWiseList = collateralSameSymbologyDict[collateralValue.ISIN];
                                collateralISINWiseList.Add(collateralValue);
                                collateralSameSymbologyDict[collateralValue.ISIN] = collateralISINWiseList;
                                _collateralSymbologyWiseDict[2] = collateralSameSymbologyDict;
                            }
                            else
                            {
                                List<CollateralImport> collaterallist = new List<CollateralImport>();
                                collaterallist.Add(collateralValue);
                                _collateralSymbologyWiseDict[2].Add(collateralValue.ISIN, collaterallist);
                            }
                        }
                        else
                        {
                            List<CollateralImport> collaterallist = new List<CollateralImport>();
                            collaterallist.Add(collateralValue);
                            Dictionary<string, List<CollateralImport>> collateralSameISINDict = new Dictionary<string, List<CollateralImport>>();
                            collateralSameISINDict.Add(collateralValue.ISIN, collaterallist);
                            _collateralSymbologyWiseDict.Add(2, collateralSameISINDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(collateralValue.SEDOL))
                    {
                        uniqueID = collateralValue.Date + collateralValue.SEDOL.Trim().ToUpper() + collateralValue.FundId;
                        if (_collateralSymbologyWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<CollateralImport>> collateralSameSymbologyDict = _collateralSymbologyWiseDict[3];
                            if (collateralSameSymbologyDict.ContainsKey(collateralValue.SEDOL))
                            {
                                List<CollateralImport> collateralSEDOLWiseList = collateralSameSymbologyDict[collateralValue.SEDOL];
                                collateralSEDOLWiseList.Add(collateralValue);
                                collateralSameSymbologyDict[collateralValue.SEDOL] = collateralSEDOLWiseList;
                                _collateralSymbologyWiseDict[3] = collateralSameSymbologyDict;
                            }
                            else
                            {
                                List<CollateralImport> collaterallist = new List<CollateralImport>();
                                collaterallist.Add(collateralValue);
                                _collateralSymbologyWiseDict[3].Add(collateralValue.SEDOL, collaterallist);
                            }
                        }
                        else
                        {
                            List<CollateralImport> collaterallist = new List<CollateralImport>();
                            collaterallist.Add(collateralValue);
                            Dictionary<string, List<CollateralImport>> collateralSEDOLDict = new Dictionary<string, List<CollateralImport>>();
                            collateralSEDOLDict.Add(collateralValue.SEDOL, collaterallist);
                            _collateralSymbologyWiseDict.Add(3, collateralSEDOLDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(collateralValue.CUSIP))
                    {
                        uniqueID = collateralValue.Date + collateralValue.CUSIP.Trim().ToUpper() + collateralValue.FundId;
                        if (_collateralSymbologyWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<CollateralImport>> collateralSameSymbologyDict = _collateralSymbologyWiseDict[4];
                            if (collateralSameSymbologyDict.ContainsKey(collateralValue.CUSIP))
                            {
                                List<CollateralImport> collateralCUSIPWiseList = collateralSameSymbologyDict[collateralValue.CUSIP];
                                collateralCUSIPWiseList.Add(collateralValue);
                                collateralSameSymbologyDict[collateralValue.CUSIP] = collateralCUSIPWiseList;
                                _collateralSymbologyWiseDict[4] = collateralSameSymbologyDict;
                            }
                            else
                            {
                                List<CollateralImport> collaterallist = new List<CollateralImport>();
                                collaterallist.Add(collateralValue);
                                _collateralSymbologyWiseDict[4].Add(collateralValue.CUSIP, collaterallist);
                            }
                        }
                        else
                        {
                            List<CollateralImport> collaterallist = new List<CollateralImport>();
                            collaterallist.Add(collateralValue);
                            Dictionary<string, List<CollateralImport>> collateralSameCUSIPDict = new Dictionary<string, List<CollateralImport>>();
                            collateralSameCUSIPDict.Add(collateralValue.CUSIP, collaterallist);
                            _collateralSymbologyWiseDict.Add(4, collateralSameCUSIPDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(collateralValue.Bloomberg))
                    {
                        uniqueID = collateralValue.Date + collateralValue.Bloomberg.Trim().ToUpper() + collateralValue.FundId;
                        if (_collateralSymbologyWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<CollateralImport>> collateralSameSymbologyDict = _collateralSymbologyWiseDict[5];
                            if (collateralSameSymbologyDict.ContainsKey(collateralValue.Bloomberg))
                            {
                                List<CollateralImport> collateralBloombergWiseList = collateralSameSymbologyDict[collateralValue.Bloomberg];
                                collateralBloombergWiseList.Add(collateralValue);
                                collateralSameSymbologyDict[collateralValue.Bloomberg] = collateralBloombergWiseList;
                                _collateralSymbologyWiseDict[5] = collateralSameSymbologyDict;
                            }
                            else
                            {
                                List<CollateralImport> collaterallist = new List<CollateralImport>();
                                collaterallist.Add(collateralValue);
                                _collateralSymbologyWiseDict[5].Add(collateralValue.Bloomberg, collaterallist);
                            }
                        }
                        else
                        {
                            List<CollateralImport> collaterallist = new List<CollateralImport>();
                            collaterallist.Add(collateralValue);
                            Dictionary<string, List<CollateralImport>> collateralSameBloombergDict = new Dictionary<string, List<CollateralImport>>();
                            collateralSameBloombergDict.Add(collateralValue.Bloomberg, collaterallist);
                            _collateralSymbologyWiseDict.Add(5, collateralSameBloombergDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(collateralValue.OSIOptionSymbol))
                    {
                        uniqueID = collateralValue.Date + collateralValue.OSIOptionSymbol.Trim().ToUpper() + collateralValue.FundId;
                        if (_collateralSymbologyWiseDict.ContainsKey(6))
                        {
                            Dictionary<string, List<CollateralImport>> collateralSameSymbologyDict = _collateralSymbologyWiseDict[6];
                            if (collateralSameSymbologyDict.ContainsKey(collateralValue.OSIOptionSymbol))
                            {
                                List<CollateralImport> collateralOSIWiseList = collateralSameSymbologyDict[collateralValue.OSIOptionSymbol];
                                collateralOSIWiseList.Add(collateralValue);
                                collateralSameSymbologyDict[collateralValue.OSIOptionSymbol] = collateralOSIWiseList;
                                _collateralSymbologyWiseDict[6] = collateralSameSymbologyDict;
                            }
                            else
                            {
                                List<CollateralImport> collaterallist = new List<CollateralImport>();
                                collaterallist.Add(collateralValue);
                                _collateralSymbologyWiseDict[6].Add(collateralValue.OSIOptionSymbol, collaterallist);
                            }
                        }
                        else
                        {
                            List<CollateralImport> collaterallist = new List<CollateralImport>();
                            collaterallist.Add(collateralValue);
                            Dictionary<string, List<CollateralImport>> collateralSameOSIDict = new Dictionary<string, List<CollateralImport>>();
                            collateralSameOSIDict.Add(collateralValue.OSIOptionSymbol, collaterallist);
                            _collateralSymbologyWiseDict.Add(6, collateralSameOSIDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(collateralValue.IDCOOptionSymbol))
                    {
                        uniqueID = collateralValue.Date + collateralValue.IDCOOptionSymbol.Trim().ToUpper() + collateralValue.FundId;
                        if (_collateralSymbologyWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<CollateralImport>> collateralSameSymbologyDict = _collateralSymbologyWiseDict[7];
                            if (collateralSameSymbologyDict.ContainsKey(collateralValue.IDCOOptionSymbol))
                            {
                                List<CollateralImport> collateralIDCOWiseList = collateralSameSymbologyDict[collateralValue.IDCOOptionSymbol];
                                collateralIDCOWiseList.Add(collateralValue);
                                collateralSameSymbologyDict[collateralValue.IDCOOptionSymbol] = collateralIDCOWiseList;
                                _collateralSymbologyWiseDict[7] = collateralSameSymbologyDict;
                            }
                            else
                            {
                                List<CollateralImport> collaterallist = new List<CollateralImport>();
                                collaterallist.Add(collateralValue);
                                _collateralSymbologyWiseDict[7].Add(collateralValue.IDCOOptionSymbol, collaterallist);
                            }
                        }
                        else
                        {
                            List<CollateralImport> collaterallist = new List<CollateralImport>();
                            collaterallist.Add(collateralValue);
                            Dictionary<string, List<CollateralImport>> collateralSameIDCODict = new Dictionary<string, List<CollateralImport>>();
                            collateralSameIDCODict.Add(collateralValue.IDCOOptionSymbol, collaterallist);
                            _collateralSymbologyWiseDict.Add(7, collateralSameIDCODict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(collateralValue.OpraOptionSymbol))
                    {
                        uniqueID = collateralValue.Date + collateralValue.OpraOptionSymbol.Trim().ToUpper() + collateralValue.FundId;
                        if (_collateralSymbologyWiseDict.ContainsKey(8))
                        {
                            Dictionary<string, List<CollateralImport>> collateralSameSymbologyDict = _collateralSymbologyWiseDict[8];
                            if (collateralSameSymbologyDict.ContainsKey(collateralValue.OpraOptionSymbol))
                            {
                                List<CollateralImport> collateralOpraWiseList = collateralSameSymbologyDict[collateralValue.OpraOptionSymbol];
                                collateralOpraWiseList.Add(collateralValue);
                                collateralSameSymbologyDict[collateralValue.OpraOptionSymbol] = collateralOpraWiseList;
                                _collateralSymbologyWiseDict[8] = collateralSameSymbologyDict;
                            }
                            else
                            {
                                List<CollateralImport> collaterallist = new List<CollateralImport>();
                                collaterallist.Add(collateralValue);
                                _collateralSymbologyWiseDict[8].Add(collateralValue.OpraOptionSymbol, collaterallist);
                            }
                        }
                        else
                        {
                            List<CollateralImport> collaterallist = new List<CollateralImport>();
                            collaterallist.Add(collateralValue);
                            Dictionary<string, List<CollateralImport>> collateralSameOpraDict = new Dictionary<string, List<CollateralImport>>();
                            collateralSameOpraDict.Add(collateralValue.OpraOptionSymbol, collaterallist);
                            _collateralSymbologyWiseDict.Add(7, collateralSameOpraDict);
                        }
                    }
                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _collateralValueCollection.Add(collateralValue);
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

        private DataTable CreateDataTableForDividendYieldImport()
        {
            DataTable dtVolatilityNew = new DataTable();
            dtVolatilityNew.TableName = "DailyDividendYield";

            DataColumn colSymbol = new DataColumn("Symbol", typeof(string));
            DataColumn colDate = new DataColumn("Date", typeof(string));
            DataColumn colDividendYieldPrice = new DataColumn("DividendYield", typeof(double));
            DataColumn colAUECID = new DataColumn("AUECID", typeof(Int32));

            dtVolatilityNew.Columns.Add(colSymbol);
            dtVolatilityNew.Columns.Add(colDate);
            dtVolatilityNew.Columns.Add(colDividendYieldPrice);
            dtVolatilityNew.Columns.Add(colAUECID);

            return dtVolatilityNew;
        }
        Dictionary<int, Dictionary<string, List<DividendYieldImport>>> _dividendYieldSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<DividendYieldImport>>>();

        void UpdateDividendYieldValueCollection(DataSet ds)
        {
            try
            {
                _dividendYieldSymbologyWiseDict.Clear();
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll"
                );
                Type typeToLoad = assembly.GetType(typeof(DividendYieldImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    DividendYieldImport dividendYieldValue = new DividendYieldImport();
                    dividendYieldValue.Symbol = string.Empty;
                    dividendYieldValue.DividendYield = 0;
                    dividendYieldValue.Date = string.Empty;
                    dividendYieldValue.AUECID = 0;

                    for (int icol = 0; icol < dTable.Columns.Count; icol++)
                    {
                        string colName = dTable.Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(dTable.Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(dividendYieldValue, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(dividendYieldValue, dTable.Rows[irow][icol].ToString().Trim(), null);
                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(dividendYieldValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result;
                                    blnIsTrue = double.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(dividendYieldValue, Convert.ToDouble(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(dividendYieldValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(dividendYieldValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result;
                                    blnIsTrue = int.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(dividendYieldValue, Convert.ToInt32(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(dividendYieldValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(dividendYieldValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result;
                                    blnIsTrue = Int64.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(dividendYieldValue, Convert.ToInt64(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(dividendYieldValue, 0, null);
                                    }
                                }
                            }
                        }
                    }


                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        dividendYieldValue.Date = _userSelectedDate;
                    }
                    else if (!dividendYieldValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(dividendYieldValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(dividendYieldValue.Date));
                            dividendYieldValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(dividendYieldValue.Date);
                            dividendYieldValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }

                    string uniqueID = string.Empty;

                    if (!String.IsNullOrEmpty(dividendYieldValue.Symbol))
                    {
                        uniqueID = dividendYieldValue.Date + dividendYieldValue.Symbol.Trim().ToUpper();
                        if (_dividendYieldSymbologyWiseDict.ContainsKey(0))
                        {
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict = _dividendYieldSymbologyWiseDict[0];
                            if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.Symbol))
                            {
                                List<DividendYieldImport> dividendYieldSymbolWiseList = dividendYieldSameSymbologyDict[dividendYieldValue.Symbol];
                                dividendYieldSymbolWiseList.Add(dividendYieldValue);
                                dividendYieldSameSymbologyDict[dividendYieldValue.Symbol] = dividendYieldSymbolWiseList;
                                _dividendYieldSymbologyWiseDict[0] = dividendYieldSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                _dividendYieldSymbologyWiseDict[0].Add(dividendYieldValue.Symbol, dividendyieldlist);
                            }
                        }
                        else
                        {
                            List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                            dividendyieldlist.Add(dividendYieldValue);
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbolDict = new Dictionary<string, List<DividendYieldImport>>
                            ();
                            dividendYieldSameSymbolDict.Add(dividendYieldValue.Symbol, dividendyieldlist);
                            _dividendYieldSymbologyWiseDict.Add(0, dividendYieldSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(dividendYieldValue.RIC))
                    {
                        uniqueID = dividendYieldValue.Date + dividendYieldValue.RIC.Trim().ToUpper();
                        if (_dividendYieldSymbologyWiseDict.ContainsKey(1))
                        {
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict = _dividendYieldSymbologyWiseDict[1];
                            if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.RIC))
                            {
                                List<DividendYieldImport> dividendYieldRICWiseList = dividendYieldSameSymbologyDict[dividendYieldValue.RIC];
                                dividendYieldRICWiseList.Add(dividendYieldValue);
                                dividendYieldSameSymbologyDict[dividendYieldValue.RIC] = dividendYieldRICWiseList;
                                _dividendYieldSymbologyWiseDict[1] = dividendYieldSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendYieldImport> dividendYieldList = new List<DividendYieldImport>();
                                dividendYieldList.Add(dividendYieldValue);
                                _dividendYieldSymbologyWiseDict[1].Add(dividendYieldValue.RIC, dividendYieldList);
                            }
                        }
                        else
                        {
                            List<DividendYieldImport> dividendYieldList = new List<DividendYieldImport>();
                            dividendYieldList.Add(dividendYieldValue);
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameRICDict = new Dictionary<string, List<DividendYieldImport>>();
                            dividendYieldSameRICDict.Add(dividendYieldValue.RIC, dividendYieldList);
                            _dividendYieldSymbologyWiseDict.Add(1, dividendYieldSameRICDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(dividendYieldValue.ISIN))
                    {
                        uniqueID = dividendYieldValue.Date + dividendYieldValue.ISIN.Trim().ToUpper();
                        if (_dividendYieldSymbologyWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict = _dividendYieldSymbologyWiseDict[2];
                            if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.ISIN))
                            {
                                List<DividendYieldImport> dividendYieldISINWiseList = dividendYieldSameSymbologyDict[dividendYieldValue.ISIN];
                                dividendYieldISINWiseList.Add(dividendYieldValue);
                                dividendYieldSameSymbologyDict[dividendYieldValue.ISIN] = dividendYieldISINWiseList;
                                _dividendYieldSymbologyWiseDict[2] = dividendYieldSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                _dividendYieldSymbologyWiseDict[2].Add(dividendYieldValue.ISIN, dividendyieldlist);
                            }
                        }
                        else
                        {
                            List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                            dividendyieldlist.Add(dividendYieldValue);
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameISINDict = new Dictionary<string, List<DividendYieldImport>>()
                            ;
                            dividendYieldSameISINDict.Add(dividendYieldValue.ISIN, dividendyieldlist);
                            _dividendYieldSymbologyWiseDict.Add(2, dividendYieldSameISINDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(dividendYieldValue.SEDOL))
                    {
                        uniqueID = dividendYieldValue.Date + dividendYieldValue.SEDOL.Trim().ToUpper();
                        if (_dividendYieldSymbologyWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict = _dividendYieldSymbologyWiseDict[3];
                            if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.SEDOL))
                            {
                                List<DividendYieldImport> dividendYieldSEDOLWiseList = dividendYieldSameSymbologyDict[dividendYieldValue.SEDOL];
                                dividendYieldSEDOLWiseList.Add(dividendYieldValue);
                                dividendYieldSameSymbologyDict[dividendYieldValue.SEDOL] = dividendYieldSEDOLWiseList;
                                _dividendYieldSymbologyWiseDict[3] = dividendYieldSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                _dividendYieldSymbologyWiseDict[3].Add(dividendYieldValue.SEDOL, dividendyieldlist);
                            }
                        }
                        else
                        {
                            List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                            dividendyieldlist.Add(dividendYieldValue);
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSEDOLDict = new Dictionary<string, List<DividendYieldImport>>();
                            dividendYieldSEDOLDict.Add(dividendYieldValue.SEDOL, dividendyieldlist);
                            _dividendYieldSymbologyWiseDict.Add(3, dividendYieldSEDOLDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(dividendYieldValue.CUSIP))
                    {
                        uniqueID = dividendYieldValue.Date + dividendYieldValue.CUSIP.Trim().ToUpper();
                        if (_dividendYieldSymbologyWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict = _dividendYieldSymbologyWiseDict[4];
                            if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.CUSIP))
                            {
                                List<DividendYieldImport> dividendYieldCUSIPWiseList = dividendYieldSameSymbologyDict[dividendYieldValue.CUSIP];
                                dividendYieldCUSIPWiseList.Add(dividendYieldValue);
                                dividendYieldSameSymbologyDict[dividendYieldValue.CUSIP] = dividendYieldCUSIPWiseList;
                                _dividendYieldSymbologyWiseDict[4] = dividendYieldSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                _dividendYieldSymbologyWiseDict[4].Add(dividendYieldValue.CUSIP, dividendyieldlist);
                            }
                        }
                        else
                        {
                            List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                            dividendyieldlist.Add(dividendYieldValue);
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameCUSIPDict = new Dictionary<string, List<DividendYieldImport>>(
                            );
                            dividendYieldSameCUSIPDict.Add(dividendYieldValue.CUSIP, dividendyieldlist);
                            _dividendYieldSymbologyWiseDict.Add(4, dividendYieldSameCUSIPDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(dividendYieldValue.Bloomberg))
                    {
                        uniqueID = dividendYieldValue.Date + dividendYieldValue.Bloomberg.Trim().ToUpper();
                        if (_dividendYieldSymbologyWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict = _dividendYieldSymbologyWiseDict[5];
                            if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.Bloomberg))
                            {
                                List<DividendYieldImport> dividendYieldBloombergWiseList = dividendYieldSameSymbologyDict[dividendYieldValue.Bloomberg];
                                dividendYieldBloombergWiseList.Add(dividendYieldValue);
                                dividendYieldSameSymbologyDict[dividendYieldValue.Bloomberg] = dividendYieldBloombergWiseList;
                                _dividendYieldSymbologyWiseDict[5] = dividendYieldSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                _dividendYieldSymbologyWiseDict[5].Add(dividendYieldValue.Bloomberg, dividendyieldlist);
                            }
                        }
                        else
                        {
                            List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                            dividendyieldlist.Add(dividendYieldValue);
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameBloombergDict = new Dictionary<string, List<
                            DividendYieldImport>>();
                            dividendYieldSameBloombergDict.Add(dividendYieldValue.Bloomberg, dividendyieldlist);
                            _dividendYieldSymbologyWiseDict.Add(5, dividendYieldSameBloombergDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(dividendYieldValue.OSIOptionSymbol))
                    {
                        uniqueID = dividendYieldValue.Date + dividendYieldValue.OSIOptionSymbol.Trim().ToUpper();
                        if (_dividendYieldSymbologyWiseDict.ContainsKey(6))
                        {
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict = _dividendYieldSymbologyWiseDict[6];
                            if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.OSIOptionSymbol))
                            {
                                List<DividendYieldImport> dividendYieldOSIWiseList = dividendYieldSameSymbologyDict[dividendYieldValue.OSIOptionSymbol];
                                dividendYieldOSIWiseList.Add(dividendYieldValue);
                                dividendYieldSameSymbologyDict[dividendYieldValue.OSIOptionSymbol] = dividendYieldOSIWiseList;
                                _dividendYieldSymbologyWiseDict[6] = dividendYieldSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                _dividendYieldSymbologyWiseDict[6].Add(dividendYieldValue.OSIOptionSymbol, dividendyieldlist);
                            }
                        }
                        else
                        {
                            List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                            dividendyieldlist.Add(dividendYieldValue);
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameOSIDict = new Dictionary<string, List<DividendYieldImport>>();
                            dividendYieldSameOSIDict.Add(dividendYieldValue.OSIOptionSymbol, dividendyieldlist);
                            _dividendYieldSymbologyWiseDict.Add(6, dividendYieldSameOSIDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(dividendYieldValue.IDCOOptionSymbol))
                    {
                        uniqueID = dividendYieldValue.Date + dividendYieldValue.IDCOOptionSymbol.Trim().ToUpper();
                        if (_dividendYieldSymbologyWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict = _dividendYieldSymbologyWiseDict[7];
                            if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.IDCOOptionSymbol))
                            {
                                List<DividendYieldImport> dividendYieldIDCOWiseList = dividendYieldSameSymbologyDict[dividendYieldValue.IDCOOptionSymbol];
                                dividendYieldIDCOWiseList.Add(dividendYieldValue);
                                dividendYieldSameSymbologyDict[dividendYieldValue.IDCOOptionSymbol] = dividendYieldIDCOWiseList;
                                _dividendYieldSymbologyWiseDict[7] = dividendYieldSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                _dividendYieldSymbologyWiseDict[7].Add(dividendYieldValue.IDCOOptionSymbol, dividendyieldlist);
                            }
                        }
                        else
                        {
                            List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                            dividendyieldlist.Add(dividendYieldValue);
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameIDCODict = new Dictionary<string, List<DividendYieldImport>>()
                            ;
                            dividendYieldSameIDCODict.Add(dividendYieldValue.IDCOOptionSymbol, dividendyieldlist);
                            _dividendYieldSymbologyWiseDict.Add(7, dividendYieldSameIDCODict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(dividendYieldValue.OpraOptionSymbol))
                    {
                        uniqueID = dividendYieldValue.Date + dividendYieldValue.OpraOptionSymbol.Trim().ToUpper();
                        if (_dividendYieldSymbologyWiseDict.ContainsKey(8))
                        {
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameSymbologyDict = _dividendYieldSymbologyWiseDict[8];
                            if (dividendYieldSameSymbologyDict.ContainsKey(dividendYieldValue.OpraOptionSymbol))
                            {
                                List<DividendYieldImport> dividendYieldOpraWiseList = dividendYieldSameSymbologyDict[dividendYieldValue.OpraOptionSymbol];
                                dividendYieldOpraWiseList.Add(dividendYieldValue);
                                dividendYieldSameSymbologyDict[dividendYieldValue.OpraOptionSymbol] = dividendYieldOpraWiseList;
                                _dividendYieldSymbologyWiseDict[8] = dividendYieldSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                                dividendyieldlist.Add(dividendYieldValue);
                                _dividendYieldSymbologyWiseDict[8].Add(dividendYieldValue.OpraOptionSymbol, dividendyieldlist);
                            }
                        }
                        else
                        {
                            List<DividendYieldImport> dividendyieldlist = new List<DividendYieldImport>();
                            dividendyieldlist.Add(dividendYieldValue);
                            Dictionary<string, List<DividendYieldImport>> dividendYieldSameOpraDict = new Dictionary<string, List<DividendYieldImport>>()
                            ;
                            dividendYieldSameOpraDict.Add(dividendYieldValue.OpraOptionSymbol, dividendyieldlist);
                            _dividendYieldSymbologyWiseDict.Add(7, dividendYieldSameOpraDict);
                        }
                    }
                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _dividendYieldValueCollection.Add(dividendYieldValue);
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

        int _rowIndex = int.MinValue;
        private void SetRecordsImported()
        {
            try
            {
                if (!_recordsProcessed.Equals(int.MinValue) && !_rowIndex.Equals(int.MinValue))
                {
                    if (_recordsProcessed > 0)
                    {
                        gridRunUpload.Rows[_rowIndex].Cells[CAPTION_NumberOfRecords].Value = _recordsProcessed;
                        gridRunUpload.Rows[_rowIndex].Cells[CAPTION_LastRunUploadDate].Value = DateTime.Now;
                    }
                }
                _recordsProcessed = int.MinValue;
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

        //private void InitializeBackgroundWorker()
        //{
        //    if (_bgwImportData == null)
        //    {
        //        _bgwImportData = new BackgroundWorker();
        //    }
        //    _bgwImportData.DoWork += new DoWorkEventHandler(bgwImportData_DoWork);
        //    _bgwImportData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwImportData_RunWorkerCompleted);
        //    _bgwImportData.RunWorkerAsync();
        //    _bgwImportData.WorkerSupportsCancellation = true;
        //}

        private void SetReImportVariables(string filePath, bool isReimporting)
        {
            _isReimporting = isReimporting;
            _filePath = filePath;
        }

        private void UnwireEvents()
        {
            try
            {
                SetReImportVariables(string.Empty, false);

                if (ProgressEvent != null)
                {
                    ProgressEvent(this, new EventArgs<string, bool, Progress>(string.Empty, false, Progress.End));
                }

                //_bgwImportData.DoWork -= new DoWorkEventHandler(bgwImportData_DoWork);
                //_bgwImportData.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(bgwImportData_RunWorkerCompleted);


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

        public delegate void ProgressEventHandler(string message, bool addMessage, Progress progress);
        public event EventHandler<EventArgs<string, bool, Progress>> ProgressEvent;

        private void ShowProgress(string message, bool addMessage, Progress progress)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        ProgressEventHandler progressHandler = new ProgressEventHandler(ShowProgress);
                        this.BeginInvoke(progressHandler, new object[] { message, addMessage, progress });
                    }
                    else
                    {
                        if (ProgressEvent != null)
                        {
                            ProgressEvent(this, new EventArgs<string, bool, Progress>(message, addMessage, progress));
                        }
                    }
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



        /// <summary>
        /// If user wants to cancel the current running operation.
        /// </summary>
        //public void CancelRunningBackGroundWorker()
        //{
        //    try
        //    {
        //        if (_bgwImportData != null && _bgwImportData.IsBusy && _bgwImportData.WorkerSupportsCancellation)
        //        {
        //            _bgwImportData.CancelAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private string GetCurrentTimeStamp()
        {
            return DateTime.Now.ToLocalTime().ToString("MMddyyyyhhmmss", DateTimeFormatInfo.InvariantInfo);
        }

        private CtrlImportPreferences _ctrlImportPreferences = new CtrlImportPreferences();
        private int _recordsProcessed = int.MinValue;
        public async void ImportDataAsync()
        {
            object eventResult = new object();
            bool _isCancelled = false;
            List<PositionMaster> positionsNotImported = new List<PositionMaster>();
            try
            {
                bool isValidToTrade = validateTradeForAccountNAVLock();

                if (isValidToTrade)
                {
                    //chunkSize to Import data..
                    int chunkSize = int.Parse(ConfigurationManager.AppSettings["ImportChunkSize"]);
                    chunkSize = (chunkSize >= MAXCHUNKSIZE) ? MAXCHUNKSIZE : chunkSize;

                    int recordsProcessed = 0;
                    int count = 0;
                    bool shouldReturn = false;
                    var postionMasterListSorted = _positionMasterCollection.OrderBy(x => x.RowIndex).ToList();

                    //StringBuilder errorMsg = new StringBuilder();
                    List<List<PositionMaster>> positionChunks = ChunkingManager.CreateChunks<PositionMaster>(postionMasterListSorted, chunkSize);

                    foreach (List<PositionMaster> positionChunk in positionChunks)
                    {
                        if (positionChunk.Count < chunkSize)
                        {
                            chunkSize = positionChunk.Count;
                        }
                        string key = recordsProcessed + Seperators.SEPERATOR_7 + (recordsProcessed + chunkSize);
                        this.IsGroupSaved = false;
                        try
                        {
                            //if (_bgwImportData.CancellationPending)
                            //{
                            //    e.Cancel = true;
                            //    _recordsProcessed = recordsProcessed;
                            //    AddPositions(ref positionsNotImported, positionChunks, ref count);
                            //    positionChunks.Clear();
                            //    WritePositionsToFile(positionsNotImported);
                            //    if (WorkCompleted != null)
                            //    {
                            //        WorkCompleted(this, null);
                            //    }
                            //    return;
                            //}

                            string message = "Processing records from " + key;
                            int rowsUpdated = int.MinValue;
                            ShowProgress(message, false, Progress.Running);

                            try
                            {
                                rowsUpdated = await _importHelper.AllocationServices.InnerChannel.CreateAndSavePositionsFromImport(positionChunk);
                                count++;
                            }
                            catch (Exception ex)
                            {
                                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                                shouldReturn = true;

                                AddPositions(ref positionsNotImported, positionChunks, ref count);
                                positionChunks.Clear();
                            }
                            if (shouldReturn)
                                break;

                            //check to verify whether all records have been successfully saved.
                            if (rowsUpdated.Equals(positionChunk.Count) || this.IsGroupSaved)
                            {
                                recordsProcessed += rowsUpdated;
                                ShowProgress(recordsProcessed.ToString(), true, Progress.Running);
                            }

                        }
                        catch (Exception ex)
                        {
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            shouldReturn = true;

                            AddPositions(ref positionsNotImported, positionChunks, ref count);
                            positionChunks.Clear();
                        }
                        if (shouldReturn)
                            break;
                    }

                    if (positionsNotImported != null && positionsNotImported.Count > 0)
                    {
                        eventResult = WritePositionsToFile(positionsNotImported);
                    }
                    else
                    {
                        eventResult = ImportCompletionStatus.Success;
                        _recordsProcessed = recordsProcessed;
                        positionChunks.Clear();
                        this.IsImportRunning = false;
                    }
                }
                ImportDataCompleted(_isCancelled, eventResult);
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

        private bool validateTradeForAccountNAVLock()
        {
            bool isProcessToSave = true;
            try
            {
                #region NAV lock validation - modified by Omshiv, MArch 2014
                //get IsNAVLockingEnabled or not from cache
                Boolean isAccountNAVLockingEnabled = CachedDataManager.GetInstance.IsNAVLockingEnabled();

                if (isAccountNAVLockingEnabled)
                {
                    foreach (PositionMaster position in _positionMasterCollection)
                    {


                        //if account selected then only check NAV locked or not for selected account - omshiv, March 2014
                        if (!string.IsNullOrEmpty(position.AccountName))
                        {

                            DateTime tradeDate = DateTime.Parse(position.PositionStartDate);
                            bool isTradeAllowed = NAVLockManager.GetInstance.ValidateTrade(position.AccountID, tradeDate);
                            if (!isTradeAllowed)
                            {
                                MessageBox.Show("NAV is locked for selected account. You can not allow to trade on this trade date.", "Prana Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                isProcessToSave = false;
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Account name is not available!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            isProcessToSave = false;
                            break;
                        }
                    }
                    #endregion
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

            return isProcessToSave;
        }

        private object WritePositionsToFile(List<PositionMaster> positionsNotImported)
        {
            object result = new object();
            string directoryPath = _ctrlImportPreferences.ImportPrefs.DirectoryPath;
            if (directoryPath.Equals(string.Empty))
            {
                directoryPath = System.Windows.Forms.Application.StartupPath;
            }
            string path = directoryPath + @"\Import Data";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                string date = DateTime.Now.ToString("MM-dd-yyyy", DateTimeFormatInfo.InvariantInfo);
                string dirPath = path + @"\" + date;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                StringBuilder filePath = new StringBuilder();
                filePath.Append(dirPath);
                filePath.Append(@"\");
                filePath.Append(GetCurrentTimeStamp());
                filePath.Append(Seperators.SEPERATOR_13);
                filePath.Append(positionsNotImported[0].UserID);
                filePath.Append(Seperators.SEPERATOR_13);

                if (_filePath.Equals(string.Empty))
                {
                    string fileName = gridRunUpload.Rows[_rowIndex].Cells[CAPTION_FilePath].Value.ToString();
                    string exactFile = fileName.Substring(fileName.LastIndexOf(@"\"));

                    if (exactFile.Contains("."))
                        filePath.Append(exactFile.Substring(1, exactFile.IndexOf('.') - 1));
                    else
                        filePath.Append(exactFile);
                }
                else
                {
                    string file = _filePath.Substring(_filePath.LastIndexOf(@"\") + 1);
                    string exactFile = file.Substring(0, file.IndexOf("."));
                    string[] seperators = new string[] { Seperators.SEPERATOR_13 };
                    string[] fileSubstring = exactFile.Split(seperators, StringSplitOptions.None);
                    StringBuilder fileName = new StringBuilder();
                    for (int i = 2; i <= fileSubstring.Length - 1; i++)
                    {
                        fileName.Append(fileSubstring[i]);
                        fileName.Append(Seperators.SEPERATOR_13);
                    }

                    filePath.Append(fileName.ToString().Substring(0, fileName.Length - 1));

                }

                CSVFileFormatter formatter = new CSVFileFormatter();
                formatter.CreateFile(positionsNotImported, null, filePath.ToString() + ".csv", null);
                result = ImportCompletionStatus.ImportError;
            }
            catch (Exception ex)
            {
                result = ImportCompletionStatus.FileWriteError;
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        private void AddPositions(ref List<PositionMaster> positionsNotImported, List<List<PositionMaster>> positionChunks, ref int count)
        {
            try
            {
                if (this.IsGroupSaved)
                    count++;

                List<List<PositionMaster>> positionsToBeDumped = positionChunks.GetRange(count, positionChunks.Count - count);
                foreach (List<PositionMaster> positionList in positionsToBeDumped)
                {
                    positionsNotImported.AddRange(positionList);
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

        public event EventHandler ReImportCompleted;
        void ImportDataCompleted(bool _isCancelled, object result)
        {
            try
            {
                if (_isCancelled)
                {
                    _positionMasterCollection = null;
                }
                else
                {
                    String message = result as string;

                    if (!String.IsNullOrEmpty(message) && (message != ImportCompletionStatus.Success.ToString()) && !_recordsProcessed.Equals(_positionMasterCollection.Count))
                    {
                        StringBuilder boxMessage = new StringBuilder();
                        boxMessage.AppendLine("Some positions/transactions could not be imported. Kindly reimport from Reimport tab!");
                        MessageBox.Show(boxMessage.ToString(), "Information");
                    }
                    else
                    {
                        if (_isReimporting && File.Exists(_filePath))
                        {
                            File.Delete(_filePath);
                            if (ReImportCompleted != null)
                            {
                                ReImportCompleted(this, null);
                            }
                        }

                        SetRecordsImported();
                        _positionMasterCollection = null;
                    }
                }

                UnwireEvents();
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

        private string _filePath = string.Empty;
        //public event EventHandler WorkCompleted;

        private void InitializeProgessBar<T>(List<T> recordsList)
        {
            try
            {
                if (ProgressEvent != null)
                {
                    ProgressEvent(this, new EventArgs<string, bool, Progress>(recordsList.Count.ToString(), false, Progress.Start));
                }
                //this.pBarProgressing.Minimum = 0;
                //this.pBarProgressing.Maximum = _positionMasterCollection.Count;
                //this.pBarProgressing.Show();
            }
            catch (Exception)
            {

            }
        }

        private enum ImportCompletionStatus
        {
            /// <summary>
            /// All of the trades imported successfully.
            /// </summary>
            Success,
            /// <summary>
            /// Some error occured while importing trades
            /// </summary>
            ImportError,
            /// <summary>
            /// Some/All of the trades were not imported.
            /// </summary>
            FileWriteError
        }

        private DataTable ArrangeTable(DataTable dataSource)
        {
            try
            {
                // what XML we will generate, all the tagname will be like COL1,COL2 .                
                dataSource.TableName = "PositionMaster";

                // update the Table columns value with "*" where columns value blank in the excel sheet
                // when we generate the XML for that table, the blank coluns do not comes in the generated XML
                // the indexing of the generated XML changed because of blank columns
                // so defalut value of the columns will be  "*"
                for (int irow = 0; irow < dataSource.Rows.Count; irow++)
                {
                    for (int icol = 0; icol < dataSource.Columns.Count; icol++)
                    {
                        string val = dataSource.Rows[irow].ItemArray[icol].ToString();
                        if (String.IsNullOrEmpty(val.Trim()))
                        {
                            dataSource.Rows[irow][icol] = "*";
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
            return dataSource;
        }

        /// <summary>
        /// Now we have arranged and updated XML
        /// as above we inserted "*" in the blank columns, but "*" needs extra treatment, so        
        ///again we replace the "*" with blank string, the following looping does the same
        /// </summary>
        /// <param name="ds"></param>
        private void ReArrangeDataSet(DataSet ds)
        {
            try
            {
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    {
                        string val = ds.Tables[0].Rows[irow].ItemArray[icol].ToString();
                        if (val.Equals("*"))
                        {
                            ds.Tables[0].Rows[irow][icol] = string.Empty;
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

        private static bool ValidateConvertedXML(string convertedXMLPath, string tableType, string xsdPath)
        {
            bool isValidated = false;
            string tmpError;
            try
            {
                switch (tableType)
                {
                    case _importTypeCash:

                        xsdPath = xsdPath + @"\ImportCash.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeTransaction:
                    case _importTypeNetPosition:

                        xsdPath = xsdPath + @"\ImportPositions.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeMarkPrice:

                        xsdPath = xsdPath + @"\ImportMarkPrice.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeForexPrice:

                        xsdPath = xsdPath + @"\ImportForexPrice.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;

                    case _importTypeDailyBeta:

                        xsdPath = xsdPath + @"\ImportBetaValues.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeActivity:

                        xsdPath = xsdPath + @"\ImportActivities.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeCollateralInterest:
                        xsdPath = xsdPath + @"\ImportCollateralInterestInsert.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeSecMasterInsert:
                    case _importTypeSecMasterUpdate:
                        xsdPath = xsdPath + @"\ImportSecMasterInsertAndUpdate.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeOMI:
                        xsdPath = xsdPath + @"\OptionModelInputs.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    //case _importTypeCashTransactions:
                    //    xsdPath = xsdPath + @"\ImportCashTransactions.xsd";
                    //    isValidated = XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "");
                    //    break;
                    case _importTypeAllocationScheme:
                    case _importTypeAllocationScheme_AppPositions:
                        xsdPath = xsdPath + @"\ImportAllocationScheme.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeDailyCreditLimit:
                        xsdPath = xsdPath + @"\ImportDailyCreditLimit.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeDoubleEntryCash:
                        xsdPath = xsdPath + @"\DoubleEntryCash.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeMultilegJournalImport:
                        xsdPath = xsdPath + @"\MultilegJournalImport.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeSettlementDateCash:
                        xsdPath = xsdPath + @"\ImportSettlementDateCash.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeDailyVolatility:
                        xsdPath = xsdPath + @"\ImportVolatilityValues.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeDailyVWAP:
                        xsdPath = xsdPath + @"\ImportVWAPValues.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeCollateralPrice:
                        xsdPath = xsdPath + @"\ImportCollateralPrice.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeDailyDividendYield:
                        xsdPath = xsdPath + @"\ImportDividendYieldValues.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    case _importTypeStagedOrder:
                        xsdPath = xsdPath + @"\ImportBlotterTrades.xsd";
                        isValidated = Prana.Utilities.UI.XMLUtilities.XMLUtilities.ValidateXML(convertedXMLPath, xsdPath, "", out tmpError);
                        break;
                    default:
                        isValidated = false;
                        break;
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
            return isValidated;
        }

        private void GenerateSMMapping(string dirPath, DataSet ds)
        {
            try
            {
                DataColumn[] keys = new DataColumn[1];

                bool blnSMMappingReq = ds.Tables[0].Columns.Contains("SMMappingReq");

                if (blnSMMappingReq)
                {
                    _SMMappingXMLName = ds.Tables[0].Rows[0]["SMMappingReq"].ToString();

                    _SMMappingXMLName = Application.StartupPath + @"\" + dirPath + @"\" + _SMMappingXMLName;

                    XmlDocument xmldocSMMapping = new XmlDocument();
                    xmldocSMMapping.Load(_SMMappingXMLName);

                    XmlNodeList xmlMappingColumns = xmldocSMMapping.SelectNodes("SecMasterMapping/SMData");

                    foreach (XmlNode node in xmlMappingColumns)
                    {
                        if (!_SMMappingCOLList.ContainsKey(node.Attributes["PMCOLName"].Value))
                        {
                            _SMMappingCOLList.Add(node.Attributes["PMCOLName"].Value, node);

                            DataColumn dc = new DataColumn(node.Attributes["SMCOLName"].Value);

                            string type = node.Attributes["type"].Value.ToString();

                            if (type.ToLower().Equals("string"))
                            {
                                dc.DataType = typeof(string);
                            }
                            else if (type.ToLower().Equals("int"))
                            {
                                dc.DataType = typeof(int);
                            }
                            else if (type.ToLower().Equals("double"))
                            {
                                dc.DataType = typeof(double);
                            }
                            else if (type.ToLower().Equals("datetime"))
                            {
                                dc.DataType = typeof(DateTime);
                            }

                            if (dc.ColumnName.Equals("TickerSymbol"))
                            {
                                keys[0] = dc;
                            }
                            _dtSMMapping.Columns.Add(dc);
                            _dtSMMapping.PrimaryKey = keys;
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

        private void ClearAll()
        {
            _positionMasterCollection = null;
            _cashCurrencyValueCollection.Clear();
            _settlementDateCashCurrencyValueCollection.Clear();
            _dividendValueCollection.Clear();
            _markPriceValueCollection.Clear();
            _forexPriceValueCollection.Clear();
            _omiValueCollection.Clear();
            _dailyCreditLimitCollection.Clear();
            _secMasterUpdateDataobj.Clear();
            _betaValueCollection.Clear();
            _volatilityValueCollection.Clear();
            _vWAPValueCollection.Clear();
            _collateralValueCollection.Clear();
            _dividendYieldValueCollection.Clear();
            _isUserSelectedDate = CAPTION_False;
            _userSelectedDate = string.Empty;
            _userSelectedAccountValue = int.MinValue;

            _SMMappingXMLName = string.Empty;
            _SMMappingCOLList.Clear();
            _dtSMMapping = new DataTable();

            _allocationSchemeSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<DataRow>>>();
            _allocationSchemeSymbologyWiseDict.Clear();

        }

        private DataTable CreateDataTableForMarkPriceImport()
        {
            DataTable dtMarkPricesNew = new DataTable();
            dtMarkPricesNew.TableName = "MarkPriceImport";

            DataColumn colSymbol = new DataColumn("Symbol", typeof(string));
            DataColumn colDate = new DataColumn("Date", typeof(string));
            DataColumn colMarkPrice = new DataColumn("MarkPrice", typeof(double));
            DataColumn colForwardPoints = new DataColumn("ForwardPoints", typeof(double));
            DataColumn colMarkPriceImportType = new DataColumn("MarkPriceImportType", typeof(string));
            DataColumn colAUECID = new DataColumn("AUECID", typeof(int));
            DataColumn colAccountID = new DataColumn("AccountID", typeof(int));

            dtMarkPricesNew.Columns.Add(colSymbol);
            dtMarkPricesNew.Columns.Add(colDate);
            dtMarkPricesNew.Columns.Add(colMarkPrice);
            dtMarkPricesNew.Columns.Add(colForwardPoints);
            dtMarkPricesNew.Columns.Add(colMarkPriceImportType);
            dtMarkPricesNew.Columns.Add(colAUECID);
            dtMarkPricesNew.Columns.Add(colAccountID);

            return dtMarkPricesNew;
        }

        private DataTable CreateDataTableForDailyCreditLimitImport()
        {
            DataTable dtDailyCreditLimit = new DataTable();
            dtDailyCreditLimit.TableName = "Table1";

            DataColumn colAccountID = new DataColumn("AccountID", typeof(int));
            DataColumn colLongDebitLimit = new DataColumn("LongDebitLimit", typeof(double));
            DataColumn colShortCreditLimit = new DataColumn("ShortCreditLimit", typeof(double));
            DataColumn colLongDebitBalance = new DataColumn("LongDebitBalance", typeof(double));
            DataColumn colShortCreditBalance = new DataColumn("ShortCreditBalance", typeof(double));

            dtDailyCreditLimit.Columns.Add(colAccountID);
            dtDailyCreditLimit.Columns.Add(colLongDebitLimit);
            dtDailyCreditLimit.Columns.Add(colShortCreditLimit);
            dtDailyCreditLimit.Columns.Add(colLongDebitBalance);
            dtDailyCreditLimit.Columns.Add(colShortCreditBalance);
            return dtDailyCreditLimit;
        }

        private SecMasterbaseList ValidateSecMasterDataBeforeSave(List<SecMasterUIObj> SecMasterListObj)
        {
            SecMasterbaseList lst = new SecMasterbaseList();
            try
            {
                lst.RequestID = System.Guid.NewGuid().ToString();
                foreach (SecMasterUIObj uiObj in SecMasterListObj)
                {
                    SecMasterBaseObj secMasterBaseObj = null;
                    AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)uiObj.AssetID);

                    switch (baseAssetCategory)
                    {
                        case AssetCategory.Equity:
                        case AssetCategory.PrivateEquity:
                        case AssetCategory.CreditDefaultSwap:
                            secMasterBaseObj = new SecMasterEquityObj();
                            secMasterBaseObj.FillUIData(uiObj);
                            break;
                        case AssetCategory.Option:
                            secMasterBaseObj = new SecMasterOptObj();
                            secMasterBaseObj.FillUIData(uiObj);
                            break;
                        case AssetCategory.Future:
                            if ((AssetCategory)uiObj.AssetID == AssetCategory.FXForward)
                            {
                                secMasterBaseObj = new SecMasterFXForwardObj();
                                secMasterBaseObj.FillUIData(uiObj);
                            }
                            else
                            {
                                secMasterBaseObj = new SecMasterFutObj();
                                secMasterBaseObj.FillUIData(uiObj);
                            }

                            break;
                        case AssetCategory.FX:
                            secMasterBaseObj = new SecMasterFxObj();
                            secMasterBaseObj.FillUIData(uiObj);
                            break;
                        case AssetCategory.Indices:
                            secMasterBaseObj = new SecMasterIndexObj();
                            secMasterBaseObj.FillUIData(uiObj);
                            break;
                        case AssetCategory.FixedIncome:
                        case AssetCategory.ConvertibleBond:
                            secMasterBaseObj = new SecMasterFixedIncome();
                            secMasterBaseObj.FillUIData(uiObj);
                            break;
                    }
                    if (secMasterBaseObj != null)
                    {
                        lst.Add(secMasterBaseObj);
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
            return lst;
        }

        private void GetSMDataForSecMasterUpdateData()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_secMasterUpdateDatauniqueSymbolDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<string, SecMasterUpdateDataByImportUI> kvp in _secMasterUpdateDatauniqueSymbolDict)
                    {
                        if (!string.IsNullOrEmpty(kvp.Key))
                        {
                            ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                            secMasterRequestObj.AddData(kvp.Key.Trim(), symbology);
                        }
                        //  secMasterRequestObj.AddNewRow();
                    }

                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            //string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                            int requestedSymbologyID = secMasterObj.RequestedSymbology;

                            if ((int)ApplicationConstants.SymbologyCodes.TickerSymbol == requestedSymbologyID)
                            {
                                if (_secMasterUpdateDatauniqueSymbolDict.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    SecMasterUpdateDataByImportUI secMasterUpdateDataObj = _secMasterUpdateDatauniqueSymbolDict[secMasterObj.RequestedSymbol];

                                    secMasterUpdateDataObj.TickerSymbol = secMasterObj.TickerSymbol;
                                    secMasterUpdateDataObj.AUECID = secMasterObj.AUECID;
                                    secMasterUpdateDataObj.ExistingLongName = secMasterObj.LongName;
                                    secMasterUpdateDataObj.ExistingMultiplier = secMasterObj.Multiplier;
                                    secMasterUpdateDataObj.ExistingUnderlyingSymbol = secMasterObj.UnderLyingSymbol;

                                    #region added
                                    secMasterUpdateDataObj.AssetID = secMasterObj.AssetID;
                                    secMasterUpdateDataObj.UnderLyingID = secMasterObj.UnderLyingID;
                                    secMasterUpdateDataObj.CurrencyID = secMasterObj.CurrencyID;
                                    secMasterUpdateDataObj.ExchangeID = secMasterObj.ExchangeID;

                                    secMasterUpdateDataObj.ISINSymbol = secMasterObj.ISINSymbol;
                                    secMasterUpdateDataObj.CusipSymbol = secMasterObj.CusipSymbol;
                                    secMasterUpdateDataObj.SedolSymbol = secMasterObj.SedolSymbol;
                                    secMasterUpdateDataObj.IDCOOptionSymbol = secMasterObj.IDCOOptionSymbol;
                                    secMasterUpdateDataObj.OPRAOptionSymbol = secMasterObj.OpraSymbol;
                                    secMasterUpdateDataObj.OSIOptionSymbol = secMasterObj.OSIOptionSymbol;
                                    secMasterUpdateDataObj.Symbol_PK = secMasterObj.Symbol_PK;
                                    if (secMasterUpdateDataObj.UDAAssetClassID == int.MinValue)
                                        secMasterUpdateDataObj.UDAAssetClassID = secMasterObj.SymbolUDAData.AssetID;
                                    if (secMasterUpdateDataObj.UDASectorID == int.MinValue)
                                        secMasterUpdateDataObj.UDASectorID = secMasterObj.SymbolUDAData.SectorID;
                                    if (secMasterUpdateDataObj.UDASubSectorID == int.MinValue)
                                        secMasterUpdateDataObj.UDASubSectorID = secMasterObj.SymbolUDAData.SubSectorID;
                                    if (secMasterUpdateDataObj.UDASecurityTypeID == int.MinValue)
                                        secMasterUpdateDataObj.UDASecurityTypeID = secMasterObj.SymbolUDAData.SecurityTypeID;
                                    if (secMasterUpdateDataObj.UDACountryID == int.MinValue)
                                        secMasterUpdateDataObj.UDACountryID = secMasterObj.SymbolUDAData.CountryID;
                                    foreach (string uda in secMasterObj.DynamicUDA.Keys)
                                    {
                                        if (string.IsNullOrWhiteSpace(secMasterUpdateDataObj.DynamicUDA[uda].ToString()))
                                        {
                                            secMasterUpdateDataObj.DynamicUDA[uda] = secMasterObj.DynamicUDA[uda];
                                        }
                                    }
                                    #endregion
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
        }

        private void GetSMDataForSecMasterInsertData()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_uniqueSymbolDictForSecMasterInsert.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<string, DataRow> kvp in _uniqueSymbolDictForSecMasterInsert)
                    {
                        if (!string.IsNullOrEmpty(kvp.Key))
                        {
                            ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                            secMasterRequestObj.AddData(kvp.Key.Trim(), symbology);
                        }
                        //  secMasterRequestObj.AddNewRow();
                    }

                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    // For Sec Master Import - Sending request for SM data and return  list of already existing SM data  
                    List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            //string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                            int requestedSymbologyID = secMasterObj.RequestedSymbology;

                            if ((int)ApplicationConstants.SymbologyCodes.TickerSymbol == requestedSymbologyID)
                            {
                                if (_uniqueSymbolDictForSecMasterInsert.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    DataRow[] rows = _dsSecMasterInsert.Tables[0].Select("TickerSymbol=" + "'" + secMasterObj.RequestedSymbol + "'");
                                    foreach (DataRow row in rows)
                                    {
                                        if (row["TickerSymbol"].ToString().Equals(secMasterObj.RequestedSymbol))
                                        {
                                            row["SymbolExistsInSM"] = "Exists";

                                            // set isSecApproved status of existing security 
                                            if (secMasterObj.IsSecApproved)
                                                row[ApplicationConstants.CONST_SEC_APPROVED_STATUS] = ApplicationConstants.CONST_APPROVED;
                                            else
                                                row[ApplicationConstants.CONST_SEC_APPROVED_STATUS] = ApplicationConstants.CONST_UN_APPROVED; ;
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
        /// Add dynamic UDAs to grdData
        /// </summary>        
        private void SetDynamicUDA(SerializableDictionary<string, DynamicUDA> dynamicUDAcache, DataTable dtSM, string importType)
        {
            try
            {
                if (dynamicUDAcache != null && dynamicUDAcache.Count > 0)
                {
                    foreach (string uda in dynamicUDAcache.Keys)
                    {
                        if (!dtSM.Columns.Contains(uda))
                        {
                            DataColumn dynamicUda = new DataColumn(uda);
                            dynamicUda.DataType = typeof(string);
                            if (importType.Equals(_importTypeSecMasterInsert))
                            {
                                dynamicUda.DefaultValue = dynamicUDAcache[uda].DefaultValue;
                            }
                            else
                            {
                                dynamicUda.DefaultValue = string.Empty;
                            }
                            dtSM.Columns.Add(dynamicUda);
                        }
                    }
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
        /// <summary>
        /// Cache to store dynamic UDAs data
        /// </summary>
        SerializableDictionary<string, DynamicUDA> _dynamicUDACache = new SerializableDictionary<string, DynamicUDA>();
        Dictionary<string, DataRow> _uniqueSymbolDictForSecMasterInsert = new Dictionary<string, DataRow>();
        private DataSet UpdateSecMasterInsertValueCollection(DataSet ds)
        {
            DataSet dsSecMaster = null;
            try
            {
                _uniqueSymbolDictForSecMasterInsert.Clear();
                DataTable dtSM = ds.Tables[0];

                #region added fields with default values
                //add column of Validation status  for each row 
                if (!dtSM.Columns.Contains("ValidationStatus"))
                {
                    DataColumn dcValidated = new DataColumn("ValidationStatus");
                    dcValidated.DataType = typeof(string);
                    dcValidated.DefaultValue = ApplicationConstants.ValidationStatus.None.ToString();
                    dtSM.Columns.Add(dcValidated);
                }
                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_AUEC_ID))
                {
                    DataColumn dcAUECID = new DataColumn(OrderFields.PROPERTY_AUEC_ID);
                    dcAUECID.DataType = typeof(Int32);
                    dcAUECID.DefaultValue = int.MinValue;
                    dtSM.Columns.Add(dcAUECID);
                }

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_ASSET_ID))
                {
                    DataColumn dcAssetID = new DataColumn(OrderFields.PROPERTY_ASSET_ID);
                    dcAssetID.DataType = typeof(Int32);
                    dcAssetID.DefaultValue = int.MinValue;
                    dtSM.Columns.Add(dcAssetID);
                }

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_UNDERLYING_ID))
                {
                    DataColumn dcUnderlyingID = new DataColumn(OrderFields.PROPERTY_UNDERLYING_ID);
                    dcUnderlyingID.DataType = typeof(Int32);
                    dcUnderlyingID.DefaultValue = int.MinValue;
                    dtSM.Columns.Add(dcUnderlyingID);
                }

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_EXCHANGEID))
                {
                    DataColumn dcExchangeID = new DataColumn(OrderFields.PROPERTY_EXCHANGEID);
                    dcExchangeID.DataType = typeof(Int32);
                    dcExchangeID.DefaultValue = int.MinValue;
                    dtSM.Columns.Add(dcExchangeID);
                }

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_CURRENCYID))
                {
                    DataColumn dcCurrencyID = new DataColumn(OrderFields.PROPERTY_CURRENCYID);
                    dcCurrencyID.DataType = typeof(Int32);
                    dcCurrencyID.DefaultValue = int.MinValue;
                    dtSM.Columns.Add(dcCurrencyID);
                }

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_TICKERSYMBOL))
                {
                    DataColumn dcTicker = new DataColumn(OrderFields.PROPERTY_TICKERSYMBOL);
                    dcTicker.DataType = typeof(string);
                    dcTicker.DefaultValue = string.Empty;
                    dtSM.Columns.Add(dcTicker);
                }
                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_LONGNAME))
                {
                    DataColumn dcLongName = new DataColumn(OrderFields.PROPERTY_LONGNAME);
                    dcLongName.DataType = typeof(string);
                    dcLongName.DefaultValue = string.Empty;
                    dtSM.Columns.Add(dcLongName);
                }
                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_REUTERSSYMBOL))
                {
                    DataColumn reutersSymbol = new DataColumn(OrderFields.PROPERTY_REUTERSSYMBOL);
                    reutersSymbol.DataType = typeof(string);
                    reutersSymbol.DefaultValue = string.Empty;
                    dtSM.Columns.Add(reutersSymbol);
                }
                if (!dtSM.Columns.Contains("Multiplier"))
                {
                    DataColumn dcMultiplier = new DataColumn("Multiplier");
                    dcMultiplier.DataType = typeof(double);
                    dcMultiplier.DefaultValue = 0;
                    dtSM.Columns.Add(dcMultiplier);
                }

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_UNDERLYINGSYMBOL))
                {
                    DataColumn dcUnderlyingSymbol = new DataColumn(OrderFields.PROPERTY_UNDERLYINGSYMBOL);
                    dcUnderlyingSymbol.DataType = typeof(string);
                    dcUnderlyingSymbol.DefaultValue = string.Empty;
                    dtSM.Columns.Add(dcUnderlyingSymbol);
                }

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_STRIKE_PRICE))
                {
                    DataColumn dcStrikePrice = new DataColumn(OrderFields.PROPERTY_STRIKE_PRICE);
                    dcStrikePrice.DataType = typeof(double);
                    dcStrikePrice.DefaultValue = 0;
                    dtSM.Columns.Add(dcStrikePrice);
                }
                if (!dtSM.Columns.Contains("ExpirationDate"))
                {
                    DataColumn dcExpirationDate = new DataColumn("ExpirationDate");
                    dcExpirationDate.DataType = typeof(DateTime);
                    dcExpirationDate.DefaultValue = DateTimeConstants.MinValue;
                    dtSM.Columns.Add(dcExpirationDate);
                }

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_LEADCURRENCYID))
                {
                    DataColumn dcLeadCurrencyID = new DataColumn(OrderFields.PROPERTY_LEADCURRENCYID);
                    dcLeadCurrencyID.DataType = typeof(Int32);
                    dcLeadCurrencyID.DefaultValue = int.MinValue;
                    dtSM.Columns.Add(dcLeadCurrencyID);
                }

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_VSCURRENCYID))
                {
                    DataColumn dcVSCurrencyID = new DataColumn(OrderFields.PROPERTY_VSCURRENCYID);
                    dcVSCurrencyID.DataType = typeof(Int32);
                    dcVSCurrencyID.DefaultValue = int.MinValue;
                    dtSM.Columns.Add(dcVSCurrencyID);
                }

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_PUT_CALL))
                {
                    DataColumn dcPutOrCall = new DataColumn(OrderFields.PROPERTY_PUT_CALL);
                    dcPutOrCall.DataType = typeof(int);
                    dcPutOrCall.DefaultValue = 2;
                    dtSM.Columns.Add(dcPutOrCall);
                }
                if (!dtSM.Columns.Contains("Delta"))
                {
                    DataColumn dcDelta = new DataColumn("Delta");
                    dcDelta.DataType = typeof(float);
                    dcDelta.DefaultValue = 1;
                    dtSM.Columns.Add(dcDelta);
                }
                if (!dtSM.Columns.Contains("Symbol_PK"))
                {
                    DataColumn dcSymbol_PK = new DataColumn("Symbol_PK");
                    dcSymbol_PK.DataType = typeof(Int64);
                    dcSymbol_PK.DefaultValue = 0;
                    dtSM.Columns.Add(dcSymbol_PK);
                }
                if (!dtSM.Columns.Contains("SymbolExistsInSM"))
                {
                    DataColumn dcSymbolExistsInSM = new DataColumn("SymbolExistsInSM");
                    dcSymbolExistsInSM.DataType = typeof(string);
                    dcSymbolExistsInSM.DefaultValue = "NotExists";
                    dtSM.Columns.Add(dcSymbolExistsInSM);
                }
                if (!dtSM.Columns.Contains("IssueDate"))
                {
                    DataColumn dcIssueDate = new DataColumn("IssueDate");
                    dcIssueDate.DataType = typeof(DateTime);
                    dcIssueDate.DefaultValue = DateTimeConstants.MinValue;
                    dtSM.Columns.Add(dcIssueDate);
                }
                if (!dtSM.Columns.Contains("Coupon"))
                {
                    DataColumn dcCoupon = new DataColumn("Coupon");
                    dcCoupon.DataType = typeof(double);
                    dcCoupon.DefaultValue = 0;
                    dtSM.Columns.Add(dcCoupon);
                }
                if (!dtSM.Columns.Contains("MaturityDate"))
                {
                    DataColumn dcMaturityDate = new DataColumn("MaturityDate");
                    dcMaturityDate.DataType = typeof(DateTime);
                    dcMaturityDate.DefaultValue = DateTimeConstants.MinValue;
                    dtSM.Columns.Add(dcMaturityDate);
                }
                if (!dtSM.Columns.Contains("AccrualBasisID"))
                {
                    DataColumn dcAccrualBasisID = new DataColumn("AccrualBasisID");
                    dcAccrualBasisID.DataType = typeof(int);
                    dcAccrualBasisID.DefaultValue = 0;
                    dtSM.Columns.Add(dcAccrualBasisID);
                }
                if (!dtSM.Columns.Contains("CouponFrequencyID"))
                {
                    DataColumn dcCouponFrequencyID = new DataColumn("CouponFrequencyID");
                    dcCouponFrequencyID.DataType = typeof(int);
                    dcCouponFrequencyID.DefaultValue = 0;
                    dtSM.Columns.Add(dcCouponFrequencyID);
                }
                if (!dtSM.Columns.Contains("IsZero"))
                {
                    DataColumn dcIsZero = new DataColumn("IsZero");
                    dcIsZero.DataType = typeof(bool);
                    dcIsZero.DefaultValue = false;
                    dtSM.Columns.Add(dcIsZero);
                }
                if (!dtSM.Columns.Contains("FirstCouponDate"))
                {
                    DataColumn dcFirstCouponDate = new DataColumn("FirstCouponDate");
                    dcFirstCouponDate.DataType = typeof(DateTime);
                    dcFirstCouponDate.DefaultValue = DateTimeConstants.MinValue;
                    dtSM.Columns.Add(dcFirstCouponDate);
                }

                if (!dtSM.Columns.Contains("DaysToSettlement"))
                {
                    DataColumn dcDaysToSettlement = new DataColumn("DaysToSettlement");
                    dcDaysToSettlement.DataType = typeof(int);
                    dcDaysToSettlement.DefaultValue = 1;
                    dtSM.Columns.Add(dcDaysToSettlement);
                }
                //Add Approved Status column in table
                if (!dtSM.Columns.Contains(ApplicationConstants.CONST_SEC_APPROVED_STATUS))
                {
                    DataColumn dcApprovalStatus = new DataColumn(ApplicationConstants.CONST_SEC_APPROVED_STATUS);
                    dcApprovalStatus.DataType = typeof(String);
                    dcApprovalStatus.DefaultValue = ApplicationConstants.CONST_UN_APPROVED;
                    dtSM.Columns.Add(dcApprovalStatus);
                }

                if (!dtSM.Columns.Contains("FixingDate"))
                {
                    DataColumn dFixingDate = new DataColumn("FixingDate");
                    dFixingDate.DataType = typeof(DateTime);
                    dFixingDate.DefaultValue = DateTimeConstants.MinValue;
                    dtSM.Columns.Add(dFixingDate);
                }
                if (!dtSM.Columns.Contains("IsNDF"))
                {
                    DataColumn dcIsNDF = new DataColumn("IsNDF");
                    dcIsNDF.DataType = typeof(bool);
                    dcIsNDF.DefaultValue = false;
                    dtSM.Columns.Add(dcIsNDF);
                }


                if (!dtSM.Columns.Contains("SecurityTypeID"))
                {
                    DataColumn bondTypeId = new DataColumn("BondTypeID");
                    bondTypeId.DataType = typeof(int);
                    bondTypeId.DefaultValue = 0;
                    dtSM.Columns.Add(bondTypeId);
                }
                else
                {
                    dtSM.Columns["SecurityTypeID"].ColumnName = "BondTypeID";
                }

                #region Newly added fields

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_CUSIPSYMBOL))
                {
                    DataColumn cusipSymbol = new DataColumn(OrderFields.PROPERTY_CUSIPSYMBOL);
                    cusipSymbol.DataType = typeof(string);
                    cusipSymbol.DefaultValue = string.Empty;
                    dtSM.Columns.Add(cusipSymbol);
                }
                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_SEDOLSYMBOL))
                {
                    DataColumn sedolSymbol = new DataColumn(OrderFields.PROPERTY_SEDOLSYMBOL);
                    sedolSymbol.DataType = typeof(string);
                    sedolSymbol.DefaultValue = string.Empty;
                    dtSM.Columns.Add(sedolSymbol);
                }
                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_ISINSYMBOL))
                {
                    DataColumn isinSymbol = new DataColumn(OrderFields.PROPERTY_ISINSYMBOL);
                    isinSymbol.DataType = typeof(string);
                    isinSymbol.DefaultValue = string.Empty;
                    dtSM.Columns.Add(isinSymbol);
                }

                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_BLOOMBERGSYMBOL))
                {
                    DataColumn bloombergSymbol = new DataColumn(OrderFields.PROPERTY_BLOOMBERGSYMBOL);
                    bloombergSymbol.DataType = typeof(string);
                    bloombergSymbol.DefaultValue = string.Empty;
                    dtSM.Columns.Add(bloombergSymbol);
                }

                if (!dtSM.Columns.Contains(ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString()))
                {
                    DataColumn osiOptionSymbol = new DataColumn(ApplicationConstants.SymbologyCodes.OSIOptionSymbol.ToString());
                    osiOptionSymbol.DataType = typeof(string);
                    osiOptionSymbol.DefaultValue = string.Empty;
                    dtSM.Columns.Add(osiOptionSymbol);
                }
                if (!dtSM.Columns.Contains(ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()))
                {
                    DataColumn idcooOptionSymbol = new DataColumn(ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString());
                    idcooOptionSymbol.DataType = typeof(string);
                    idcooOptionSymbol.DefaultValue = string.Empty;
                    dtSM.Columns.Add(idcooOptionSymbol);
                }
                if (!dtSM.Columns.Contains(OrderFields.PROPERTY_OPRAOPTIONSYMBOL))
                {
                    DataColumn opraOptionymbol = new DataColumn(OrderFields.PROPERTY_OPRAOPTIONSYMBOL);
                    opraOptionymbol.DataType = typeof(string);
                    opraOptionymbol.DefaultValue = string.Empty;
                    dtSM.Columns.Add(opraOptionymbol);
                }

                if (!dtSM.Columns.Contains("StrikePriceMultiplier"))
                {
                    DataColumn dataColumn = new DataColumn("StrikePriceMultiplier");
                    dataColumn.DataType = typeof(double);
                    dataColumn.DefaultValue = 0;
                    dtSM.Columns.Add(dataColumn);
                }
                if (!dtSM.Columns.Contains("EsignalOptionRoot"))
                {
                    DataColumn opraOptionymbol = new DataColumn("EsignalOptionRoot");
                    opraOptionymbol.DataType = typeof(string);
                    opraOptionymbol.DefaultValue = string.Empty;
                    dtSM.Columns.Add(opraOptionymbol);
                }
                if (!dtSM.Columns.Contains("BloombergOptionRoot"))
                {
                    DataColumn opraOptionymbol = new DataColumn("BloombergOptionRoot");
                    opraOptionymbol.DataType = typeof(string);
                    opraOptionymbol.DefaultValue = string.Empty;
                    dtSM.Columns.Add(opraOptionymbol);
                }
                if (!dtSM.Columns.Contains("Sector"))
                {
                    DataColumn sector = new DataColumn("Sector");
                    sector.DataType = typeof(string);
                    sector.DefaultValue = string.Empty;
                    dtSM.Columns.Add(sector);
                }

                #region UDA
                if (!dtSM.Columns.Contains("UDAAssetClass"))
                {
                    DataColumn udaAssetClassID = new DataColumn("UDAAssetClassID");
                    udaAssetClassID.DataType = typeof(string);
                    udaAssetClassID.DefaultValue = "Undefined";
                    dtSM.Columns.Add(udaAssetClassID);
                }
                else
                {
                    dtSM.Columns["UDAAssetClass"].ColumnName = "UDAAssetClassID";

                }
                if (!dtSM.Columns.Contains("UDASector"))
                {
                    DataColumn udaSectorID = new DataColumn("UDASectorID");
                    udaSectorID.DataType = typeof(string);
                    udaSectorID.DefaultValue = "Undefined";
                    dtSM.Columns.Add(udaSectorID);
                }
                else
                {
                    dtSM.Columns["UDASector"].ColumnName = "UDASectorID";
                }
                if (!dtSM.Columns.Contains("UDASubSector"))
                {
                    DataColumn udaSubSectorID = new DataColumn("UDASubSectorID");
                    udaSubSectorID.DataType = typeof(string);
                    udaSubSectorID.DefaultValue = "Undefined";
                    dtSM.Columns.Add(udaSubSectorID);
                }
                else
                {
                    dtSM.Columns["UDASubSector"].ColumnName = "UDASubSectorID";
                }
                if (!dtSM.Columns.Contains("UDASecurityType"))
                {
                    DataColumn udaSecurityTypeID = new DataColumn("UDASecurityTypeID");
                    udaSecurityTypeID.DataType = typeof(string);
                    udaSecurityTypeID.DefaultValue = "Undefined";
                    dtSM.Columns.Add(udaSecurityTypeID);
                }
                else
                {
                    dtSM.Columns["UDASecurityType"].ColumnName = "UDASecurityTypeID";
                }
                if (!dtSM.Columns.Contains("UDACountry"))
                {
                    DataColumn udaCountryID = new DataColumn("UDACountryID");
                    udaCountryID.DataType = typeof(string);
                    udaCountryID.DefaultValue = "Undefined";
                    dtSM.Columns.Add(udaCountryID);
                }
                else
                {
                    dtSM.Columns["UDACountry"].ColumnName = "UDACountryID";
                }
                #endregion

                #endregion
                #endregion
                SetDynamicUDA(_dynamicUDACache, dtSM, _importTypeSecMasterUpdate);
                dsSecMaster = ds.Clone();

                foreach (DataRow row in dtSM.Rows)
                {
                    if (Convert.ToInt32(row["AUECID"].ToString()) != int.MinValue && Convert.ToInt32(row["AUECID"].ToString()) != 0)
                    {
                        row["AssetID"] = CachedDataManager.GetInstance.GetAssetIdByAUECId(Convert.ToInt32(row["AUECID"].ToString()));
                        row["UnderLyingID"] = CachedDataManager.GetInstance.GetUnderlyingID(Convert.ToInt32(row["AUECID"].ToString()));
                        row["ExchangeID"] = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(Convert.ToInt32(row["AUECID"].ToString()));
                        row["CurrencyID"] = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(Convert.ToInt32(row["AUECID"].ToString()));
                    }
                    else if ((Convert.ToInt32(row["AUECID"].ToString()) == int.MinValue || Convert.ToInt32(row["AUECID"].ToString()) == 0) &&
                        (Convert.ToInt32(row["AssetID"].ToString()) != int.MinValue && Convert.ToInt32(row["AssetID"].ToString()) != 0) &&
                        (Convert.ToInt32(row["UnderLyingID"].ToString()) != int.MinValue && Convert.ToInt32(row["UnderLyingID"].ToString()) != 0) &&
                        (Convert.ToInt32(row["ExchangeID"].ToString()) != int.MinValue && Convert.ToInt32(row["ExchangeID"].ToString()) != 0) &&
                        (Convert.ToInt32(row["CurrencyID"].ToString()) != int.MinValue && Convert.ToInt32(row["CurrencyID"].ToString()) != 0))
                    {
                        //row["AUECID"] = CachedDataManager.GetInstance.GetAUECID(Convert.ToInt32(row["AssetID"].ToString()), Convert.ToInt32(row["UnderLyingID"].ToString()), Convert.ToInt32(row["ExchangeID"].ToString()), Convert.ToInt32(row["CurrencyID"].ToString()));
                        row["AUECID"] = CachedDataManager.GetInstance.GetAUECID(Convert.ToInt32(row["AssetID"].ToString()), Convert.ToInt32(row["UnderLyingID"].ToString()), Convert.ToInt32(row["ExchangeID"].ToString()));
                    }

                    if (dtSM.Columns.Contains("IsNDF"))
                    {
                        if (!row["IsNDF"].ToString().ToUpper().Equals("TRUE") && !row["IsNDF"].ToString().ToUpper().Equals("FALSE"))
                        {
                            row["IsNDF"] = false;
                        }
                    }
                    if (dtSM.Columns.Contains("IsZERO"))
                    {
                        if (!row["IsZERO"].ToString().ToUpper().Equals("FALSE") && !row["IsZERO"].ToString().ToUpper().Equals("TRUE"))
                        {
                            row["IsZERO"] = false;
                        }
                    }

                    if (!_uniqueSymbolDictForSecMasterInsert.ContainsKey(row["TickerSymbol"].ToString()))
                    {
                        _uniqueSymbolDictForSecMasterInsert.Add(row["TickerSymbol"].ToString(), row);
                        DataRow dr = dsSecMaster.Tables["PositionMaster"].NewRow();
                        dr.ItemArray = row.ItemArray;
                        dr.RowError = row.RowError;
                        //dr.RowError = "This is an error";
                        dsSecMaster.Tables["PositionMaster"].Rows.Add(dr);
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
            return dsSecMaster;
        }

        private void ValidateUDAs(string uda, Dictionary<int, string> udaDict, DataRow row, UDACollection udaIDs, int udaIdMax)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(row[uda].ToString())
                                   && udaDict.Where(x => x.Value.ToUpper() == row[uda].ToString().Trim().ToUpper()).Count() > 0)
                {
                    int k = udaDict.FirstOrDefault(x => x.Value.ToUpper() == row[uda].ToString().Trim().ToUpper()).Key;
                    row[uda] = k;
                }
                else if (!string.IsNullOrWhiteSpace(row[uda].ToString()) && udaIDs.Contains(row[uda].ToString().Trim()))
                {
                    int k = udaIDs.GetUDAId(row[uda].ToString().Trim());
                    row[uda] = k;
                }
                else if (!string.IsNullOrWhiteSpace(row[uda].ToString()))
                {
                    udaIDs.Add(new UDA
                    {
                        ID = ++udaIdMax,
                        Name = row[uda].ToString().Trim()
                    });
                    row[uda] = udaIdMax;
                }
                else
                {
                    row[uda] = int.MinValue;
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

        private int GetMaxId(Dictionary<int, string> dictionary)
        {
            if (dictionary.Count > 0)
                return dictionary.Keys.Max();

            return -1;
        }

        private void GetSMDataForAllocationScheme()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_allocationSchemeSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<DataRow>>> kvp in _allocationSchemeSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<DataRow>> symbolDict = _allocationSchemeSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<DataRow>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                                //    secMasterRequestObj.AddNewRow();
                            }
                        }
                    }

                    if (secMasterRequestObj != null && secMasterRequestObj.Count > 0)
                    {
                        secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                        secMasterRequestObj.HashCode = this.GetHashCode();
                        List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                        if (secMasterCollection != null && secMasterCollection.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                            {
                                string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                                //string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                                //string cUSIPSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                                //string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                                //string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                                //string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                                //string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                                //string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                                //string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();


                                int requestedSymbologyID = secMasterObj.RequestedSymbology;

                                if (_allocationSchemeSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                                {
                                    Dictionary<string, List<DataRow>> dictSymbols = _allocationSchemeSymbologyWiseDict[requestedSymbologyID];
                                    if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                    {
                                        List<DataRow> listDataRows = dictSymbols[secMasterObj.RequestedSymbol];
                                        foreach (DataRow row in listDataRows)
                                        {
                                            row["Symbol"] = pranaSymbol;
                                            row["IsSymbolValidatedFromSM"] = "Validated";
                                            //row["SymbolExistsInSM"] = "Exists";
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
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<SecMasterUIObj> ConvertDSToSecMasterInsertValueCollection(DataSet ds)
        {
            List<SecMasterUIObj> secMasterInsertNewDataobj = null;
            try
            {
                IList toReturn = new List<object>();

                secMasterInsertNewDataobj = new List<SecMasterUIObj>();

                List<SerializableDictionary<String, Object>> lstdynamicUDA = new List<SerializableDictionary<string, object>>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SerializableDictionary<String, Object> dynamicUDA = new SerializableDictionary<string, object>();
                    foreach (string uda in _dynamicUDACache.Keys)
                    {
                        if (ds.Tables[0].Columns.Contains(uda) && !string.IsNullOrWhiteSpace(row[uda].ToString()))
                        {
                            dynamicUDA.Add(uda, row[uda]);
                        }
                    }
                    lstdynamicUDA.Add(dynamicUDA);
                }
                Type type = typeof(SecMasterUIObj);
                if (ds.Tables.Count > 0)
                    toReturn = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateCollectionFromDataTableForSecMaster(ds.Tables[0], type);
                int count = 0;
                foreach (Object secmasterObj in toReturn)
                {
                    SecMasterUIObj secMasterUpdatedObj = (SecMasterUIObj)secmasterObj;
                    secMasterUpdatedObj.DynamicUDA = lstdynamicUDA[count];
                    count++;
                    secMasterInsertNewDataobj.Add(secMasterUpdatedObj);
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
            return secMasterInsertNewDataobj;
        }

        Dictionary<string, SecMasterUpdateDataByImportUI> _secMasterUpdateDatauniqueSymbolDict = new Dictionary<string, SecMasterUpdateDataByImportUI>();

        private void UpdateSecMasterUpdateDataValueCollection(DataSet ds)
        {
            try
            {
                _secMasterUpdateDatauniqueSymbolDict.Clear();
                List<SerializableDictionary<String, Object>> lstdynamicUDA = new List<SerializableDictionary<string, object>>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SerializableDictionary<String, Object> dynamicUDA = new SerializableDictionary<string, object>();
                    foreach (string uda in _dynamicUDACache.Keys)
                    {
                        if (ds.Tables[0].Columns.Contains(uda))
                        {
                            dynamicUDA.Add(uda, row[uda]);
                        }
                    }
                    lstdynamicUDA.Add(dynamicUDA);
                }
                IList toReturn = new List<object>();

                Type type = typeof(SecMasterUpdateDataByImportUI);
                if (ds.Tables.Count > 0)
                    toReturn = Prana.Utilities.MiscUtilities.GeneralUtilities.CreateCollectionFromDataTableForSecMaster(ds.Tables[0], type);
                int count = 0;
                foreach (Object secmasterObj in toReturn)
                {
                    SecMasterUpdateDataByImportUI secMasterUpdatedObj = (SecMasterUpdateDataByImportUI)secmasterObj;
                    secMasterUpdatedObj.AUECID = int.MinValue;
                    secMasterUpdatedObj.DynamicUDA = lstdynamicUDA[count];
                    count++;
                    if (!_secMasterUpdateDatauniqueSymbolDict.ContainsKey(secMasterUpdatedObj.TickerSymbol))
                    {
                        _secMasterUpdateDatauniqueSymbolDict.Add(secMasterUpdatedObj.TickerSymbol, secMasterUpdatedObj);
                        _secMasterUpdateDataobj.Add(secMasterUpdatedObj);
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

        void displayForm_refersh(object sender, EventArgs e)
        {
            UploadDataThruLocalFile();
        }

        private bool ValidationCheck()
        {
            bool iScheck = false;
            bool blnIsRowSel = false;

            //TODO find better way to validation. Unnecessary for loop checks are here -OM.
            foreach (UltraGridRow gridrow in gridRunUpload.Rows)
            {
                blnIsRowSel = Convert.ToBoolean(gridrow.Cells[CAPTION_IsSelected].Value);
                if (blnIsRowSel)
                {
                    break;
                }
            }
            if (blnIsRowSel == false)
            {
                MessageBox.Show("No records selected", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            foreach (UltraGridRow gridrow in gridRunUpload.Rows)
            {
                bool isRowSelected = Convert.ToBoolean(gridrow.Cells[CAPTION_IsSelected].Value);
                if (isRowSelected)
                {
                    string strTableType = Convert.ToString(gridrow.Cells[CAPTION_TableTypeID].Text);

                    string fileWithPath = Convert.ToString(gridrow.Cells[CAPTION_FilePath].Value);
                    string strXSLTPath = Convert.ToString(gridrow.Cells[CAPTION_DataSourceXSLT].Value);

                    if (!strTableType.Equals(_importTypeAllocationScheme_AppPositions))
                    {
                        if (String.IsNullOrEmpty(fileWithPath) || String.IsNullOrEmpty(strXSLTPath))
                        {
                            iScheck = false;
                            break;
                        }
                        else
                        {
                            iScheck = true;
                        }
                    }
                    else
                    {
                        iScheck = true;
                    }
                }
            }
            if (iScheck == false)
            {
                MessageBox.Show(" Either import file or XSLT file not selected.", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return iScheck;
            }

            return iScheck;
        }

        private bool _isReimporting = false;
        public void UploadReImportFile(string fileWithPath, string strTableType)
        {
            try
            {
                if (IsTradeServerConnected(strTableType))
                {
                    MessageBox.Show("Trade Server is down!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataTable dataSource = FileReaderFactory.GetDataTableFromDifferentFileFormats(fileWithPath);

                if (dataSource != null && dataSource.Rows.Count > 0)
                {
                    //Moving row 0 values to column names.
                    int i = 0;
                    foreach (DataColumn col in dataSource.Columns)
                    {
                        col.ColumnName = dataSource.Rows[0].ItemArray[i].ToString();
                        i++;
                    }
                    dataSource.TableName = "PositionMaster";

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dataSource);

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (_positionMasterCollection == null)
                            _positionMasterCollection = new List<PositionMaster>();
                        else
                            _positionMasterCollection.Clear();

                        UpdatePositionMasterCollection(ds);

                        if (DisplayToUser(strTableType))
                        {
                            if (_positionMasterCollection.Count > 0)
                            {
                                InitializeProgessBar(_positionMasterCollection);
                                //InitializeBackgroundWorker();
                                ImportDataAsync();
                                _isReimporting = true;
                                _filePath = fileWithPath;
                            }
                            else
                            {
                                MessageBox.Show("No Position is valid, Cannot import", "Import Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
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
        //This method moved to FileReader
        #region commented
        //private DataTable GetDataTableFromDifferentFileFormats(string fileName)
        //{
        //    DataTable datasourceData = null;
        //    try
        //    {
        //        string fileFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);

        //        switch (fileFormat.ToUpperInvariant())
        //        {
        //            case "CSV":
        //                datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Csv).GetDataTableFromUploadedDataFile(fileName);
        //                break;
        //            case "XLS":
        //                datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(fileName);
        //                break;
        //            default:
        //                datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Default).GetDataTableFromUploadedDataFile(fileName);
        //                break;
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
        //    return datasourceData;
        //}
        #endregion


        #region unused code, can be removed
        //private DataTable ParseCSVFile(string path)
        //{
        //    string inputString = "";
        //    try
        //    {
        //        // check that the file exists before opening it
        //        if (File.Exists(path))
        //        {
        //            StreamReader sr = new StreamReader(path);
        //            inputString = sr.ReadToEnd();
        //            sr.Close();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return ParseCSV(inputString);
        //}

        public DataTable ParseCSV(string inputString)
        {
            DataTable dt = new DataTable();
            try
            {
                string[] lines = inputString.Split('\n');
                string[] elements = lines[1].Split(',');
                string columnName1 = "COL";
                for (int j = 0; j < elements.Length; j++)
                {
                    dt.Columns.Add(new DataColumn(columnName1 + (j + 1)));

                }
                DataRow dr = dt.NewRow();
                for (int i = 0; i < lines.Length; i++)
                {
                    elements = lines[i].Split(',');
                    dr = dt.NewRow();
                    dr.ItemArray = elements;
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }
        #endregion


        Dictionary<int, Dictionary<string, List<PositionMaster>>> _positionMasterSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<PositionMaster>>>();
        bool isDisplaySwapColumns;
        private void UpdatePositionMasterCollection(DataSet ds)
        {
            try
            {
                _positionMasterSymbologyWiseDict.Clear();
                isDisplaySwapColumns = false;
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(PositionMaster).ToString());
                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    PositionMaster positionMaster = new PositionMaster();
                    positionMaster.AUECID = 0;
                    positionMaster.UnderlyingID = 0;
                    positionMaster.ExchangeID = 0;
                    positionMaster.PositionStartDate = string.Empty;
                    positionMaster.AccountName = string.Empty;
                    positionMaster.IsSecApproved = false;

                    for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    {
                        try
                        {
                            string colName = ds.Tables[0].Columns[icol].ColumnName.ToString();

                            // assign into property
                            PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                            if (propInfo != null)
                            {
                                Type dataType = propInfo.PropertyType;

                                if (dataType.FullName.Equals("System.String"))
                                {
                                    if (String.IsNullOrEmpty(ds.Tables[0].Rows[irow][icol].ToString()))
                                    {
                                        propInfo.SetValue(positionMaster, string.Empty, null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(positionMaster, ds.Tables[0].Rows[irow][icol].ToString().Trim(), null);

                                    }
                                }
                                else if (dataType.FullName.Equals("System.Double"))
                                {
                                    if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                    {
                                        propInfo.SetValue(positionMaster, 0, null);
                                    }
                                    else
                                    {
                                        bool blnIsTrue;
                                        double result;
                                        blnIsTrue = double.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                        if (blnIsTrue)
                                        {
                                            propInfo.SetValue(positionMaster, Convert.ToDouble(ds.Tables[0].Rows[irow][icol]), null);
                                        }
                                        else
                                        {
                                            propInfo.SetValue(positionMaster, double.MinValue, null);
                                        }
                                    }
                                }
                                else if (dataType.FullName.Equals("System.Int32"))
                                {
                                    if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                    {
                                        propInfo.SetValue(positionMaster, 0, null);
                                    }
                                    else
                                    {
                                        bool blnIsTrue;
                                        int result;
                                        blnIsTrue = int.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                        if (blnIsTrue)
                                        {
                                            propInfo.SetValue(positionMaster, Convert.ToInt32(ds.Tables[0].Rows[irow][icol]), null);
                                        }
                                        else
                                        {
                                            propInfo.SetValue(positionMaster, 0, null);
                                        }
                                    }
                                }
                                else if (dataType.FullName.Equals("System.Int64"))
                                {
                                    if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                    {
                                        propInfo.SetValue(positionMaster, 0, null);
                                    }
                                    else
                                    {
                                        bool blnIsTrue;
                                        Int64 result;
                                        blnIsTrue = Int64.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                        if (blnIsTrue)
                                        {
                                            propInfo.SetValue(positionMaster, Convert.ToInt64(ds.Tables[0].Rows[irow][icol]), null);
                                        }
                                        else
                                        {
                                            propInfo.SetValue(positionMaster, 0, null);
                                        }
                                    }
                                }
                                else if (dataType.BaseType.Equals(typeof(System.Enum)))
                                {
                                    //Enum handling on generic basis since we are also dealing with another column 
                                    //CommissionSource now.
                                    string colValue = ds.Tables[0].Rows[irow][icol].ToString();
                                    object value = null;
                                    if (!string.IsNullOrEmpty(colValue))
                                    {
                                        value = Enum.Parse(dataType, colValue, true);
                                        propInfo.SetValue(positionMaster, value, null);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                            if (rethrow)
                            {
                                throw;
                            }
                        }
                    }
                    //CHMW-3076 [Foreign Positions Settling in Base Currency][Cash Management] Add settlement currency fields in cash management
                    if (dictCurrencies.ContainsValue(positionMaster.SettlCurrencyName))
                    {
                        positionMaster.SettlementCurrencyID = dictCurrencies.Where(p => p.Value == positionMaster.SettlCurrencyName).Select(p => p.Key).ToList()[0];
                    }
                    else
                    {
                        positionMaster.SettlementCurrencyID = positionMaster.CurrencyID;
                        if (dictCurrencies.ContainsKey(positionMaster.CurrencyID))
                        {
                            positionMaster.SettlCurrencyName = dictCurrencies[positionMaster.CurrencyID];
                        }
                    }
                    positionMaster.TransactionSource = TransactionSource.TradeImport;
                    positionMaster.UserID = _companyUser.CompanyUserID;
                    positionMaster.CompanyID = _companyUser.CompanyID;
                    positionMaster.TradingAccountID = _pMTradingAccountID;
                    positionMaster.PranaMsgType = OrderFields.PranaMsgTypes.ImportPosition;
                    positionMaster.ExternalOrderID = OrderIDGenerator.GenerateExternalOrderID();

                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        positionMaster.PositionStartDate = _userSelectedDate;
                    }

                    // validate transaction type. If Transaction Type not correctly add in the XSLT file then default value i.e. side will be imported
                    if (!string.IsNullOrEmpty(positionMaster.TransactionType))
                    {
                        string transactionTypeName = CachedDataManager.GetInstance.GetTransactionTypeNameByAcronym(positionMaster.TransactionType);
                        if (string.IsNullOrEmpty(transactionTypeName))
                        {
                            positionMaster.TransactionType = string.Empty;
                        }
                    }

                    // if there is no transaction type in XSLT file to import, side will be imported in the transaction type
                    // transaction type is the super set of side
                    if (string.IsNullOrEmpty(positionMaster.TransactionType))
                    {
                        positionMaster.TransactionType = CachedDataManager.GetInstance.GetTransactionTypeAcronymByOrderSideTagValue(positionMaster.SideTagValue);
                    }



                    #region Swap Related Properties
                    if (positionMaster.IsSwapped == 1)
                    {
                        if (!isDisplaySwapColumns)
                            isDisplaySwapColumns = true;

                        positionMaster.NotionalValue = positionMaster.CostBasis * positionMaster.NetPosition;
                        if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                            positionMaster.OrigTransDate = _userSelectedDate;
                    }
                    #endregion

                    if (!String.IsNullOrEmpty(positionMaster.Symbol))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(0))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[0];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.Symbol))
                            {
                                List<PositionMaster> positionMasterSymbolWiseList = positionMasterSameSymbologyDict[positionMaster.Symbol];
                                positionMasterSymbolWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.Symbol] = positionMasterSymbolWiseList;
                                _positionMasterSymbologyWiseDict[0] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[0].Add(positionMaster.Symbol, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbolDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameSymbolDict.Add(positionMaster.Symbol, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(0, positionMasterSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.RIC))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(1))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[1];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.RIC))
                            {
                                List<PositionMaster> positionMasterRICWiseList = positionMasterSameSymbologyDict[positionMaster.RIC];
                                positionMasterRICWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.RIC] = positionMasterRICWiseList;
                                _positionMasterSymbologyWiseDict[1] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[1].Add(positionMaster.RIC, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameRICDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameRICDict.Add(positionMaster.RIC, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(1, positionMasterSameRICDict);
                        }

                    }
                    else if (!String.IsNullOrEmpty(positionMaster.ISIN))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[2];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.ISIN))
                            {
                                List<PositionMaster> positionMasterISINWiseList = positionMasterSameSymbologyDict[positionMaster.ISIN];
                                positionMasterISINWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.ISIN] = positionMasterISINWiseList;
                                _positionMasterSymbologyWiseDict[2] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[2].Add(positionMaster.ISIN, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameISINDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameISINDict.Add(positionMaster.ISIN, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(2, positionMasterSameISINDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.SEDOL))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[3];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.SEDOL))
                            {
                                List<PositionMaster> positionMasterSEDOLWiseList = positionMasterSameSymbologyDict[positionMaster.SEDOL];
                                positionMasterSEDOLWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.SEDOL] = positionMasterSEDOLWiseList;
                                _positionMasterSymbologyWiseDict[3] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[3].Add(positionMaster.SEDOL, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameSEDOLDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameSEDOLDict.Add(positionMaster.SEDOL, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(3, positionMasterSameSEDOLDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.CUSIP))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[4];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.CUSIP))
                            {
                                List<PositionMaster> positionMasterCUSIPWiseList = positionMasterSameSymbologyDict[positionMaster.CUSIP];
                                positionMasterCUSIPWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.CUSIP] = positionMasterCUSIPWiseList;
                                _positionMasterSymbologyWiseDict[4] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[4].Add(positionMaster.CUSIP, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameCUSIPDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameCUSIPDict.Add(positionMaster.CUSIP, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(4, positionMasterSameCUSIPDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.Bloomberg))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[5];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.Bloomberg))
                            {
                                List<PositionMaster> positionMasterBloombergWiseList = positionMasterSameSymbologyDict[positionMaster.Bloomberg];
                                positionMasterBloombergWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.Bloomberg] = positionMasterBloombergWiseList;
                                _positionMasterSymbologyWiseDict[5] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[5].Add(positionMaster.Bloomberg, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameBloombergDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameBloombergDict.Add(positionMaster.Bloomberg, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(5, positionMasterSameBloombergDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.OSIOptionSymbol))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(6))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[6];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.OSIOptionSymbol))
                            {
                                List<PositionMaster> positionMasterOSIWiseList = positionMasterSameSymbologyDict[positionMaster.OSIOptionSymbol];
                                positionMasterOSIWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.OSIOptionSymbol] = positionMasterOSIWiseList;
                                _positionMasterSymbologyWiseDict[6] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[6].Add(positionMaster.OSIOptionSymbol, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameOSIDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameOSIDict.Add(positionMaster.OSIOptionSymbol, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(6, positionMasterSameOSIDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.IDCOOptionSymbol))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[7];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.IDCOOptionSymbol))
                            {
                                List<PositionMaster> positionMasterIDCOWiseList = positionMasterSameSymbologyDict[positionMaster.IDCOOptionSymbol];
                                positionMasterIDCOWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.IDCOOptionSymbol] = positionMasterIDCOWiseList;
                                _positionMasterSymbologyWiseDict[7] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[7].Add(positionMaster.IDCOOptionSymbol, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameIDCODict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameIDCODict.Add(positionMaster.IDCOOptionSymbol, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(7, positionMasterSameIDCODict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(positionMaster.OpraOptionSymbol))
                    {
                        if (_positionMasterSymbologyWiseDict.ContainsKey(8))
                        {
                            Dictionary<string, List<PositionMaster>> positionMasterSameSymbologyDict = _positionMasterSymbologyWiseDict[8];
                            if (positionMasterSameSymbologyDict.ContainsKey(positionMaster.OpraOptionSymbol))
                            {
                                List<PositionMaster> positionMasterOpraWiseList = positionMasterSameSymbologyDict[positionMaster.OpraOptionSymbol];
                                positionMasterOpraWiseList.Add(positionMaster);
                                positionMasterSameSymbologyDict[positionMaster.OpraOptionSymbol] = positionMasterOpraWiseList;
                                _positionMasterSymbologyWiseDict[8] = positionMasterSameSymbologyDict;
                            }
                            else
                            {
                                List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                                positionMasterlist.Add(positionMaster);
                                _positionMasterSymbologyWiseDict[8].Add(positionMaster.OpraOptionSymbol, positionMasterlist);
                            }
                        }
                        else
                        {
                            List<PositionMaster> positionMasterlist = new List<PositionMaster>();
                            positionMasterlist.Add(positionMaster);
                            Dictionary<string, List<PositionMaster>> positionMasterSameOpraDict = new Dictionary<string, List<PositionMaster>>();
                            positionMasterSameOpraDict.Add(positionMaster.OpraOptionSymbol, positionMasterlist);
                            _positionMasterSymbologyWiseDict.Add(7, positionMasterSameOpraDict);
                        }
                    }

                    _positionMasterCollection.Add(positionMaster);
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

        // int _hashCode = 0;

        private void GetSMDataForMarkPriceImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_markPriceSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<MarkPriceImport>>> kvp in _markPriceSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<MarkPriceImport>> symbolDict = _markPriceSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<MarkPriceImport>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                                //  secMasterRequestObj.AddNewRow();
                            }
                        }
                    }

                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                            string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                            string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                            string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                            string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                            string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                            string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                            string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                            string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

                            int requestedSymbologyID = secMasterObj.RequestedSymbology;

                            if (_markPriceSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<MarkPriceImport>> dictSymbols = _markPriceSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<MarkPriceImport> listMarkPrice = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (MarkPriceImport markPriceImport in listMarkPrice)
                                    {
                                        markPriceImport.Symbol = pranaSymbol;
                                        markPriceImport.CUSIP = cuspiSymbol;
                                        markPriceImport.ISIN = isinSymbol;
                                        markPriceImport.SEDOL = sedolSymbol;
                                        markPriceImport.Bloomberg = bloombergSymbol;
                                        markPriceImport.RIC = reutersSymbol;
                                        markPriceImport.OSIOptionSymbol = osiOptionSymbol;
                                        markPriceImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        markPriceImport.AUECID = secMasterObj.AUECID;
                                        markPriceImport.IsSecApproved = secMasterObj.IsSecApproved;
                                        markPriceImport.OSIOptionSymbol = osiOptionSymbol;
                                        markPriceImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        markPriceImport.OpraOptionSymbol = opraOptionSymbol;

                                        if (markPriceImport.IsForexRequired.Trim().ToUpper().Equals("TRUE"))
                                            UpdateMarkPriceObj(markPriceImport, secMasterObj);

                                    }
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
        }

        private void GetSMDataForBetaImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_betaSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<BetaImport>>> kvp in _betaSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<BetaImport>> symbolDict = _betaSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<BetaImport>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                            }
                        }
                    }

                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                            string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                            string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                            string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                            string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                            string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                            string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                            string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                            string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

                            int requestedSymbologyID = secMasterObj.RequestedSymbology;

                            if (_betaSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<BetaImport>> dictSymbols = _betaSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<BetaImport> listBetaValues = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (BetaImport betaImport in listBetaValues)
                                    {
                                        betaImport.Symbol = pranaSymbol;
                                        betaImport.CUSIP = cuspiSymbol;
                                        betaImport.ISIN = isinSymbol;
                                        betaImport.SEDOL = sedolSymbol;
                                        betaImport.Bloomberg = bloombergSymbol;
                                        betaImport.RIC = reutersSymbol;
                                        betaImport.OSIOptionSymbol = osiOptionSymbol;
                                        betaImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        betaImport.AUECID = secMasterObj.AUECID;
                                        betaImport.OSIOptionSymbol = osiOptionSymbol;
                                        betaImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        betaImport.OpraOptionSymbol = opraOptionSymbol;
                                    }
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
        }

        //private void UpdateBetaValueObj(BetaImport betaImport, SecMasterBaseObj secMasterObj)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        private void GetSMDataForDividendImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_dividendSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<DividendImport>>> kvp in _dividendSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<DividendImport>> symbolDict = _dividendSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<DividendImport>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                                //    secMasterRequestObj.AddNewRow();
                            }
                        }
                    }

                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                            string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                            string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                            string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                            string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                            string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                            string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                            string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                            string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();
                            int requestedSymbologyID = secMasterObj.RequestedSymbology;

                            if (_dividendSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<DividendImport>> dictSymbols = _dividendSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<DividendImport> listDividend = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (DividendImport dividendImport in listDividend)
                                    {
                                        dividendImport.Symbol = pranaSymbol;
                                        dividendImport.CUSIP = cuspiSymbol;
                                        dividendImport.ISIN = isinSymbol;
                                        dividendImport.SEDOL = sedolSymbol;
                                        dividendImport.Bloomberg = bloombergSymbol;
                                        dividendImport.RIC = reutersSymbol;
                                        dividendImport.OSIOptionSymbol = osiOptionSymbol;
                                        dividendImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        dividendImport.AUECID = secMasterObj.AUECID;
                                        dividendImport.OSIOptionSymbol = osiOptionSymbol;
                                        dividendImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        dividendImport.OpraOptionSymbol = opraOptionSymbol;
                                        UpdateDividendImportObj(secMasterObj, dividendImport);
                                    }
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
        }


        private void GetSMDataForOMIImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();

                if (_omiSymbolWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, UserOptModelInput>> kvp in _omiSymbolWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, UserOptModelInput> symbolDict = _omiSymbolWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, UserOptModelInput> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                                //  secMasterRequestObj.AddNewRow();
                            }
                        }
                    }

                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                            string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                            string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                            string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                            string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                            string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                            string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                            string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                            string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

                            int requestedSymbologyID = secMasterObj.RequestedSymbology;
                            int AUECId = secMasterObj.AUECID;
                            if (_omiSymbolWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, UserOptModelInput> dictSymbols = _omiSymbolWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    UserOptModelInput omiInput = dictSymbols[secMasterObj.RequestedSymbol];
                                    omiInput.Symbol = pranaSymbol;
                                    omiInput.CUSIP = cuspiSymbol;
                                    omiInput.ISIN = isinSymbol;
                                    omiInput.SEDOL = sedolSymbol;
                                    omiInput.Bloomberg = bloombergSymbol;
                                    omiInput.RIC = reutersSymbol;
                                    omiInput.OSIOptionSymbol = osiOptionSymbol;
                                    omiInput.IDCOOptionSymbol = idcoOptionSymbol;
                                    omiInput.OSIOptionSymbol = osiOptionSymbol;
                                    omiInput.IDCOOptionSymbol = idcoOptionSymbol;
                                    omiInput.OpraOptionSymbol = opraOptionSymbol;
                                    omiInput.AuecID = AUECId;
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
        }


        private void GetSMDataForTaxlotImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_positionMasterSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<PositionMaster>>> kvp in _positionMasterSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<PositionMaster>> symbolDict = _positionMasterSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<PositionMaster>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                                // secMasterRequestObj.AddNewRow();
                            }
                        }
                    }

                    if (secMasterRequestObj != null && secMasterRequestObj.Count > 0)
                    {
                        secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                        secMasterRequestObj.HashCode = this.GetHashCode();
                        List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                        if (secMasterCollection != null && secMasterCollection.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                            {
                                string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                                string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                                string cUSIPSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                                string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                                string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                                string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                                string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                                string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                                string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

                                string requestedSymbol = secMasterObj.RequestedSymbol.ToUpper();
                                int requestedSymbologyID = secMasterObj.RequestedSymbology;

                                if (_positionMasterSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                                {
                                    Dictionary<string, List<PositionMaster>> dictSymbols = _positionMasterSymbologyWiseDict[requestedSymbologyID];

                                    if (dictSymbols.ContainsKey(requestedSymbol))
                                    {
                                        List<PositionMaster> listPosMaster = dictSymbols[requestedSymbol];

                                        foreach (PositionMaster positionMaster in listPosMaster)
                                        {
                                            positionMaster.Symbol = pranaSymbol;
                                            positionMaster.CUSIP = cUSIPSymbol;
                                            positionMaster.ISIN = isinSymbol;
                                            positionMaster.SEDOL = sedolSymbol;
                                            positionMaster.Bloomberg = bloombergSymbol;
                                            positionMaster.RIC = reutersSymbol;
                                            positionMaster.OSIOptionSymbol = osiOptionSymbol;
                                            positionMaster.IDCOOptionSymbol = idcoOptionSymbol;
                                            positionMaster.OpraOptionSymbol = opraOptionSymbol;
                                            positionMaster.UnderlyingSymbol = secMasterObj.UnderLyingSymbol;
                                            UpdatePositionMasterObj(positionMaster, secMasterObj);

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
        /// Validate And Update Stage order
        /// </summary>
        private void ValidateAndUpdate()
        {
            _importHelper.ValidateAndUpdate();
        }
        Dictionary<int, bool> IsCRDREqualMultipleEntries = null;
        public void PrepareDatasetAndValidation(DataSet ds)
        {
            try
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    int CRColNumberMultiple = 0;
                    int DRColNumberMultiple = 0;
                    int CurrencyColNumber = 0;
                    int CurrencyIDColNumber = 0;
                    int EntryIDcolNumber = 0;
                    int AllocationSideColNumber = 0;
                    int FXRateColNumber = 0;
                    int FXMethodOperatorColNumber = 0;

                    DRColNumberMultiple = dt.Columns["DR"].Ordinal;
                    CRColNumberMultiple = dt.Columns["CR"].Ordinal;
                    CurrencyColNumber = dt.Columns["CurrencyName"].Ordinal;
                    CurrencyIDColNumber = dt.Columns["CurrencyID"].Ordinal;
                    EntryIDcolNumber = dt.Columns["EntryID"].Ordinal;
                    AllocationSideColNumber = dt.Columns["AccountSide"].Ordinal;
                    FXRateColNumber = dt.Columns["FXRate"].Ordinal;
                    FXMethodOperatorColNumber = dt.Columns["FXConversionMethodOperator"].Ordinal;

                    if (CRColNumberMultiple != -1 && DRColNumberMultiple != -1)
                    {
                        IsCRDREqualMultipleEntries = new Dictionary<int, bool>();
                        decimal Debitsum = 0;
                        decimal Creditsum = 0;
                        int CurrencyID = 0;
                        int EntryId = 0;
                        decimal FXRate = 0;

                        EntryId = Convert.ToInt32(dt.Rows[0][EntryIDcolNumber]);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (CurrencyColNumber != -1)
                            {
                                if (Convert.ToString(dt.Rows[i][CurrencyColNumber]) != "")
                                {
                                    CurrencyID = CommonDataCache.CachedDataManager.GetInstance.GetCurrencyID(dt.Rows[i][CurrencyColNumber].ToString());
                                    if (CurrencyID != Convert.ToInt32(dt.Rows[i][CurrencyIDColNumber]))
                                        dt.Rows[i][CurrencyIDColNumber] = CurrencyID.ToString();
                                }
                                FXRate = Convert.ToDecimal(dt.Rows[i][FXRateColNumber]);
                            }
                            if (EntryId == Convert.ToInt32(dt.Rows[i][EntryIDcolNumber]))
                            {
                                if (Convert.ToString(dt.Rows[i][DRColNumberMultiple]) != "")
                                    Debitsum = Debitsum + (Convert.ToDecimal(dt.Rows[i][DRColNumberMultiple]) * FXRate);
                                if (Convert.ToString(dt.Rows[i][CRColNumberMultiple]) != "")
                                    Creditsum = Creditsum + (Convert.ToDecimal(dt.Rows[i][CRColNumberMultiple]) * FXRate);
                            }
                            else
                            {
                                if (Debitsum == Creditsum)
                                    IsCRDREqualMultipleEntries[i] = true;
                                else
                                    IsCRDREqualMultipleEntries[i] = false;

                                Debitsum = 0;
                                Creditsum = 0;
                                if (Convert.ToString(dt.Rows[i][DRColNumberMultiple]) != "")
                                    Debitsum = Debitsum + (Convert.ToDecimal(dt.Rows[i][DRColNumberMultiple]) * FXRate);
                                if (Convert.ToString(dt.Rows[i][CRColNumberMultiple]) != "")
                                    Creditsum = Creditsum + (Convert.ToDecimal(dt.Rows[i][CRColNumberMultiple]) * FXRate);
                                EntryId = Convert.ToInt32(dt.Rows[i][EntryIDcolNumber]);
                            }
                            if (Convert.ToString(dt.Rows[i][FXMethodOperatorColNumber]) == "")
                                dt.Rows[i][FXMethodOperatorColNumber] = "M";
                            if (Convert.ToString(dt.Rows[i][AllocationSideColNumber]) == "")
                            {
                                dt.Rows[i][AllocationSideColNumber] = Convert.ToDecimal(dt.Rows[i][DRColNumberMultiple]) != 0 ? "DR" : "CR";
                            }
                            CurrencyID = 0;
                        }
                        if (Debitsum == Creditsum)
                            IsCRDREqualMultipleEntries[dt.Rows.Count] = true;
                        else
                            IsCRDREqualMultipleEntries[dt.Rows.Count] = false;
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

        public void CheckValididationForMultipleEntries(DataSet ds)
        {
            try
            {
                if (IsCRDREqualMultipleEntries != null)
                {
                    int count = 0;
                    int Keyused = -1;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Keyused = IsCRDREqualMultipleEntries.Keys.First(kvp => kvp > count);
                        if (IsCRDREqualMultipleEntries[Keyused])
                        {
                            row["Validated"] = ApplicationConstants.ValidationStatus.Validated.ToString();
                        }
                        else
                        {
                            row["Validated"] = ApplicationConstants.ValidationStatus.InvalidData.ToString();
                        }
                        count = count + 1;
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

        public void CheckValididationForDoubleEntry(DataSet ds)
        {
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(row["JournalEntries"]) != "")
                    {
                        string[] arrStrDRCR = row["JournalEntries"].ToString().Split('|');
                        string[] DREntryWithAmount = arrStrDRCR[0].Split(':');
                        string[] CREntryWithAmount = arrStrDRCR[1].Split(':');

                        if (Convert.ToDecimal(DREntryWithAmount[1]) == Convert.ToDecimal(CREntryWithAmount[1]))
                        {
                            row["Validated"] = ApplicationConstants.ValidationStatus.Validated.ToString();
                        }
                        else
                        {
                            row["Validated"] = ApplicationConstants.ValidationStatus.InvalidData.ToString();
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
        public void CheckValididationOfCollateralInterest(DataSet ds)
        {
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (CachedDataManager.GetInstance.GetAccountText(Convert.ToInt32(row["Account"])) == string.Empty)
                    {
                        row["ValidationStatus"] = ApplicationConstants.ValidationStatus.InvalidData.ToString();
                    }
                    else
                    {
                        row["ValidationStatus"] = ApplicationConstants.ValidationStatus.Validated.ToString();
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

        public void AddLocalfileUploadData(DataSet ds)
        {
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (_userSelectedDate != string.Empty)
                    {
                        row["Date"] = _userSelectedDate;
                    }
                    if (_userSelectedAccountValue != int.MinValue)
                        row["Account"] = Convert.ToString(_userSelectedAccountValue);
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

        public void ChangeAccountIdToAccountName(DataSet ds)
        {
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (CachedDataManager.GetInstance.GetAccountText(Convert.ToInt32(row["Account"])) == string.Empty)
                        row["Account"] = "NULL";
                    else
                        row["Account"] = CachedDataManager.GetInstance.GetAccountText(Convert.ToInt32(row["Account"]));
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

        public void ChangeAccountNameToAccountID(DataSet ds)
        {
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    row["Account"] = CachedDataManager.GetInstance.GetAccountID(Convert.ToString(row["Account"]));
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
        /// Get SM data for stage  
        /// </summary>
        private void GetSMDataForStageImport()
        {
            _importHelper.GetSMDataForStageImport(_securityMaster, GetHashCode());
        }

        private void GetSMDataForVolatilityImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_volatilitySymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<VolatilityImport>>> kvp in _volatilitySymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<VolatilityImport>> symbolDict = _volatilitySymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<VolatilityImport>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                            }
                        }
                    }

                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                            string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                            string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                            string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                            string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                            string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                            string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                            string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                            string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

                            int requestedSymbologyID = secMasterObj.RequestedSymbology;

                            if (_volatilitySymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<VolatilityImport>> dictSymbols = _volatilitySymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<VolatilityImport> listVolatilityValues = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (VolatilityImport volatilityImport in listVolatilityValues)
                                    {
                                        volatilityImport.Symbol = pranaSymbol;
                                        volatilityImport.CUSIP = cuspiSymbol;
                                        volatilityImport.ISIN = isinSymbol;
                                        volatilityImport.SEDOL = sedolSymbol;
                                        volatilityImport.Bloomberg = bloombergSymbol;
                                        volatilityImport.RIC = reutersSymbol;
                                        volatilityImport.OSIOptionSymbol = osiOptionSymbol;
                                        volatilityImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        volatilityImport.AUECID = secMasterObj.AUECID;
                                        volatilityImport.OSIOptionSymbol = osiOptionSymbol;
                                        volatilityImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        volatilityImport.OpraOptionSymbol = opraOptionSymbol;
                                    }
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
        }

        private void GetSMDataForVWAPImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_vWAPSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<VWAPImport>>> kvp in _vWAPSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<VWAPImport>> symbolDict = _vWAPSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<VWAPImport>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                            }
                        }
                    }

                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                            string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                            string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                            string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                            string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                            string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                            string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                            string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                            string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

                            int requestedSymbologyID = secMasterObj.RequestedSymbology;

                            if (_vWAPSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<VWAPImport>> dictSymbols = _vWAPSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<VWAPImport> listVWAPValues = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (VWAPImport vWAPImport in listVWAPValues)
                                    {
                                        vWAPImport.Symbol = pranaSymbol;
                                        vWAPImport.CUSIP = cuspiSymbol;
                                        vWAPImport.ISIN = isinSymbol;
                                        vWAPImport.SEDOL = sedolSymbol;
                                        vWAPImport.Bloomberg = bloombergSymbol;
                                        vWAPImport.RIC = reutersSymbol;
                                        vWAPImport.OSIOptionSymbol = osiOptionSymbol;
                                        vWAPImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        vWAPImport.AUECID = secMasterObj.AUECID;
                                        vWAPImport.OSIOptionSymbol = osiOptionSymbol;
                                        vWAPImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        vWAPImport.OpraOptionSymbol = opraOptionSymbol;
                                    }
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
        }

        private void GetSMDataForCollateralImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_collateralSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<CollateralImport>>> kvp in _collateralSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<CollateralImport>> symbolDict = _collateralSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<CollateralImport>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                            }
                        }
                    }

                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                            string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                            string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                            string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                            string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                            string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                            string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                            string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                            string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

                            int requestedSymbologyID = secMasterObj.RequestedSymbology;

                            if (_collateralSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<CollateralImport>> dictSymbols = _collateralSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<CollateralImport> listCollateralValues = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (CollateralImport collateralImport in listCollateralValues)
                                    {
                                        collateralImport.Symbol = pranaSymbol;
                                        collateralImport.CUSIP = cuspiSymbol;
                                        collateralImport.ISIN = isinSymbol;
                                        collateralImport.SEDOL = sedolSymbol;
                                        collateralImport.Bloomberg = bloombergSymbol;
                                        collateralImport.RIC = reutersSymbol;
                                        collateralImport.OSIOptionSymbol = osiOptionSymbol;
                                        collateralImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        collateralImport.AUECID = secMasterObj.AUECID;
                                        collateralImport.OSIOptionSymbol = osiOptionSymbol;
                                        collateralImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        collateralImport.OpraOptionSymbol = opraOptionSymbol;
                                    }
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
        }

        private void GetSMDataForDividendYieldImport()
        {
            try
            {
                //_hashCode = this.GetHashCode();
                if (_dividendYieldSymbologyWiseDict.Count > 0)
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (KeyValuePair<int, Dictionary<string, List<DividendYieldImport>>> kvp in _dividendYieldSymbologyWiseDict)
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)kvp.Key;

                        Dictionary<string, List<DividendYieldImport>> symbolDict = _dividendYieldSymbologyWiseDict[kvp.Key];

                        if (symbolDict.Count > 0)
                        {
                            foreach (KeyValuePair<string, List<DividendYieldImport>> keyvaluepair in symbolDict)
                            {
                                if (!string.IsNullOrEmpty(keyvaluepair.Key))
                                {
                                    secMasterRequestObj.AddData(keyvaluepair.Key.Trim(), symbology);
                                }
                            }
                        }
                    }

                    secMasterRequestObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                    secMasterRequestObj.HashCode = this.GetHashCode();
                    List<SecMasterBaseObj> secMasterCollection = _securityMaster.SendRequestList(secMasterRequestObj);

                    if (secMasterCollection != null && secMasterCollection.Count > 0)
                    {
                        foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                        {
                            string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                            string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                            string cuspiSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                            string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                            string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                            string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                            string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                            string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                            string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

                            int requestedSymbologyID = secMasterObj.RequestedSymbology;

                            if (_dividendYieldSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<DividendYieldImport>> dictSymbols = _dividendYieldSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<DividendYieldImport> listDividendYieldValues = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (DividendYieldImport dividendYieldImport in listDividendYieldValues)
                                    {
                                        dividendYieldImport.Symbol = pranaSymbol;
                                        dividendYieldImport.CUSIP = cuspiSymbol;
                                        dividendYieldImport.ISIN = isinSymbol;
                                        dividendYieldImport.SEDOL = sedolSymbol;
                                        dividendYieldImport.Bloomberg = bloombergSymbol;
                                        dividendYieldImport.RIC = reutersSymbol;
                                        dividendYieldImport.OSIOptionSymbol = osiOptionSymbol;
                                        dividendYieldImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        dividendYieldImport.AUECID = secMasterObj.AUECID;
                                        dividendYieldImport.OSIOptionSymbol = osiOptionSymbol;
                                        dividendYieldImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        dividendYieldImport.OpraOptionSymbol = opraOptionSymbol;
                                    }
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
        }

        Dictionary<int, Dictionary<string, UserOptModelInput>> _omiSymbolWiseDict = new Dictionary<int, Dictionary<string, UserOptModelInput>>();

        private void UpdateOMIValueCollection(DataSet ds)
        {
            try
            {

                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(UserOptModelInput).ToString());
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    UserOptModelInput OMIvalue = new UserOptModelInput();
                    for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    {
                        string colName = ds.Tables[0].Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(OMIvalue, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(OMIvalue, ds.Tables[0].Rows[irow][icol].ToString().Trim(), null);



                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(OMIvalue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result;
                                    blnIsTrue = double.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(OMIvalue, Convert.ToDouble(ds.Tables[0].Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(OMIvalue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(OMIvalue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result;
                                    blnIsTrue = int.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(OMIvalue, Convert.ToInt32(ds.Tables[0].Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(OMIvalue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(OMIvalue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result;
                                    blnIsTrue = Int64.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(OMIvalue, Convert.ToInt64(ds.Tables[0].Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(OMIvalue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Boolean"))

                                if (String.IsNullOrEmpty(ds.Tables[0].Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(OMIvalue, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(OMIvalue, XmlConvert.ToBoolean((ds.Tables[0].Rows[irow][icol]).ToString()), null);

                                }
                        }
                    }
                    _omiValueCollection.Add(OMIvalue);
                    Dictionary<string, UserOptModelInput> omiValueSameSymbolDict = null;


                    int reqSymbology = 0;
                    string reqSymbol = string.Empty;

                    if (!String.IsNullOrEmpty(OMIvalue.Symbol))
                    {
                        reqSymbology = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
                        reqSymbol = OMIvalue.Symbol;
                    }
                    else if (!String.IsNullOrEmpty(OMIvalue.RIC))
                    {
                        reqSymbology = (int)ApplicationConstants.SymbologyCodes.ReutersSymbol;
                        reqSymbol = OMIvalue.RIC;
                    }

                    else if (!String.IsNullOrEmpty(OMIvalue.OpraOptionSymbol))
                    {
                        reqSymbology = (int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol;
                        reqSymbol = OMIvalue.OpraOptionSymbol;
                    }

                    else if (!String.IsNullOrEmpty(OMIvalue.OSIOptionSymbol))
                    {
                        reqSymbology = (int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol;
                        reqSymbol = OMIvalue.OSIOptionSymbol;
                    }

                    else if (!String.IsNullOrEmpty(OMIvalue.SEDOL))
                    {
                        reqSymbology = (int)ApplicationConstants.SymbologyCodes.SEDOLSymbol;
                        reqSymbol = OMIvalue.SEDOL;
                    }

                    else if (!String.IsNullOrEmpty(OMIvalue.Bloomberg))
                    {
                        reqSymbology = (int)ApplicationConstants.SymbologyCodes.BloombergSymbol;
                        reqSymbol = OMIvalue.Bloomberg;
                    }

                    else if (!String.IsNullOrEmpty(OMIvalue.CUSIP))
                    {
                        reqSymbology = (int)ApplicationConstants.SymbologyCodes.CUSIPSymbol;
                        reqSymbol = OMIvalue.CUSIP;
                    }

                    else if (!String.IsNullOrEmpty(OMIvalue.IDCOOptionSymbol))
                    {
                        reqSymbology = (int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol;
                        reqSymbol = OMIvalue.Symbol;
                    }
                    if (!String.IsNullOrEmpty(reqSymbol))
                    {
                        if (_omiSymbolWiseDict.ContainsKey(reqSymbology))
                        {
                            if (_omiSymbolWiseDict[reqSymbology].ContainsKey(reqSymbol))
                            {
                                _omiSymbolWiseDict[reqSymbology][reqSymbol] = OMIvalue;
                            }
                            else
                            {
                                _omiSymbolWiseDict[reqSymbology].Add(reqSymbol, OMIvalue);
                            }
                        }
                        else
                        {
                            omiValueSameSymbolDict = new Dictionary<string, UserOptModelInput>();
                            omiValueSameSymbolDict.Add(reqSymbol, OMIvalue);
                            _omiSymbolWiseDict.Add(reqSymbology, omiValueSameSymbolDict);
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

        private void RemoveSMCachedDataFromEnRichedTable()
        {
            try
            {
                if (_dtSMMapping != null && _dtSMMapping.Rows.Count > 0)
                {
                    //_hashCode = this.GetHashCode();

                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();

                    foreach (DataRow drow in _dtSMMapping.Rows)
                    {
                        if (!string.IsNullOrEmpty(drow["TickerSymbol"].ToString()))
                        {
                            secMasterRequestObj.AddData(drow["TickerSymbol"].ToString(), ApplicationConstants.SymbologyCodes.TickerSymbol);
                        }
                        // secMasterRequestObj.AddNewRow();
                    }

                    if (secMasterRequestObj != null && secMasterRequestObj.Count > 0)
                    {
                        secMasterRequestObj.HashCode = this.GetHashCode();
                        List<SecMasterBaseObj> secMasterCollection = _securityMaster.GetSMCachedData(secMasterRequestObj);

                        if (secMasterCollection != null && secMasterCollection.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in secMasterCollection)
                            {
                                DataRow[] rows = _dtSMMapping.Select("TickerSymbol=" + "'" + secMasterObj.TickerSymbol + "'");
                                foreach (DataRow row in rows)
                                {
                                    _dtSMMapping.BeginInit();
                                    _dtSMMapping.Rows.Remove(row);
                                    _dtSMMapping.EndInit();
                                    _dtSMMapping.AcceptChanges();
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
        }

        private void SendSMEnRichData()
        {
            try
            {
                if (_dtSMMapping != null && _dtSMMapping.Rows.Count > 0)
                {
                    _securityMaster.EnRichSecMasterObj(_dtSMMapping);
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


        private void UpdateCashCurrencyValueCollection(DataSet ds)
        {
            try
            {
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(CashCurrencyValue).ToString());
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    CashCurrencyValue cashCurrencyValue = new CashCurrencyValue();
                    cashCurrencyValue.BaseCurrencyID = 0;
                    cashCurrencyValue.LocalCurrencyID = 0;
                    cashCurrencyValue.Date = string.Empty;

                    for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    {
                        string colName = ds.Tables[0].Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        Type dataType = propInfo.PropertyType;

                        if (dataType.FullName.Equals("System.String"))
                        {
                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[irow][icol].ToString()))
                            {
                                propInfo.SetValue(cashCurrencyValue, string.Empty, null);
                            }
                            else
                            {
                                propInfo.SetValue(cashCurrencyValue, ds.Tables[0].Rows[irow][icol].ToString().Trim(), null);

                            }
                        }
                        else if (dataType.FullName.Equals("System.Double"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(cashCurrencyValue, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                double result;
                                blnIsTrue = double.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(cashCurrencyValue, Convert.ToDouble(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(cashCurrencyValue, 0, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("System.Int32"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(cashCurrencyValue, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                int result;
                                blnIsTrue = int.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(cashCurrencyValue, Convert.ToInt32(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(cashCurrencyValue, 0, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("System.Int64"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(cashCurrencyValue, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                Int64 result;
                                blnIsTrue = Int64.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(cashCurrencyValue, Convert.ToInt64(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(cashCurrencyValue, 0, null);
                                }
                            }
                        }
                    }


                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        cashCurrencyValue.Date = _userSelectedDate;
                    }
                    else if (!cashCurrencyValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(cashCurrencyValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(cashCurrencyValue.Date));
                            cashCurrencyValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(cashCurrencyValue.Date);
                            cashCurrencyValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }
                    if (_userSelectedAccountValue != int.MinValue)
                    {
                        cashCurrencyValue.AccountName = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                        cashCurrencyValue.AccountID = _userSelectedAccountValue;
                    }
                    else if (!string.IsNullOrEmpty(cashCurrencyValue.AccountName))
                    {
                        cashCurrencyValue.AccountID = CachedDataManager.GetInstance.GetAccountID(cashCurrencyValue.AccountName.Trim());
                    }

                    if (!string.IsNullOrEmpty(cashCurrencyValue.BaseCurrency))
                    {
                        cashCurrencyValue.BaseCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(cashCurrencyValue.BaseCurrency.Trim());
                    }
                    if (!string.IsNullOrEmpty(cashCurrencyValue.LocalCurrency))
                    {
                        cashCurrencyValue.LocalCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(cashCurrencyValue.LocalCurrency.Trim());
                    }

                    if (cashCurrencyValue.CashValueBase == 0 && cashCurrencyValue.CashValueLocal != 0)
                    {
                        if (cashCurrencyValue.LocalCurrencyID > 0 && cashCurrencyValue.BaseCurrencyID > 0 && cashCurrencyValue.LocalCurrencyID.Equals(cashCurrencyValue.BaseCurrencyID))
                        {
                            cashCurrencyValue.CashValueBase = cashCurrencyValue.CashValueLocal;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(cashCurrencyValue.Date))
                            {
                                ForexConverter.GetInstance(_companyUser.CompanyID, Convert.ToDateTime(cashCurrencyValue.Date)).GetForexData(Convert.ToDateTime(cashCurrencyValue.Date));
                                //CHMW-3132	Account wise fx rate handling for expiration settlement
                                ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(_companyUser.CompanyID).GetConversionRateFromCurrenciesForGivenDate(cashCurrencyValue.LocalCurrencyID, cashCurrencyValue.BaseCurrencyID, Convert.ToDateTime(cashCurrencyValue.Date), cashCurrencyValue.AccountID);
                                if (conversionRate != null)
                                {
                                    cashCurrencyValue.ForexConversionRate = conversionRate.RateValue;
                                    if (conversionRate.ConversionMethod == Operator.D)
                                    {
                                        if (conversionRate.RateValue != 0)
                                            cashCurrencyValue.CashValueBase = cashCurrencyValue.CashValueLocal / conversionRate.RateValue;
                                    }
                                    else
                                    {
                                        cashCurrencyValue.CashValueBase = cashCurrencyValue.CashValueLocal * conversionRate.RateValue;
                                    }
                                }
                            }
                        }
                    }
                    if (cashCurrencyValue.CashValueLocal == 0 && cashCurrencyValue.CashValueBase != 0)
                    {
                        if (cashCurrencyValue.LocalCurrencyID > 0 && cashCurrencyValue.BaseCurrencyID > 0 && cashCurrencyValue.LocalCurrencyID.Equals(cashCurrencyValue.BaseCurrencyID))
                        {
                            cashCurrencyValue.CashValueLocal = cashCurrencyValue.CashValueBase;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(cashCurrencyValue.Date))
                            {
                                ForexConverter.GetInstance(_companyUser.CompanyID, Convert.ToDateTime(cashCurrencyValue.Date)).GetForexData(Convert.ToDateTime(cashCurrencyValue.Date));
                                //CHMW-3132	Account wise fx rate handling for expiration settlement
                                ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(_companyUser.CompanyID).GetConversionRateFromCurrenciesForGivenDate(cashCurrencyValue.LocalCurrencyID, cashCurrencyValue.BaseCurrencyID, Convert.ToDateTime(cashCurrencyValue.Date), cashCurrencyValue.AccountID);
                                if (conversionRate != null)
                                {
                                    cashCurrencyValue.ForexConversionRate = conversionRate.RateValue;
                                    if (conversionRate.ConversionMethod == Operator.D)
                                    {
                                        if (conversionRate.RateValue != 0)
                                            cashCurrencyValue.CashValueLocal = cashCurrencyValue.CashValueBase / conversionRate.RateValue;
                                    }
                                    else
                                    {
                                        cashCurrencyValue.CashValueLocal = cashCurrencyValue.CashValueBase * conversionRate.RateValue;
                                    }
                                }
                            }
                        }
                    }

                    _cashCurrencyValueCollection.Add(cashCurrencyValue);
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


        //Created By: Pooja Porwal
        //Date:12 Feb 2015
        //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-5820

        /// <summary>
        /// Update SettlementDate Cash Currency Value Collection
        /// </summary>
        /// <param name="ds">Data Set of converted from xslt Data</param>
        private void UpdateSettlementDateCashCurrencyValueCollection(DataSet ds)
        {
            try
            {
                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(SettlementDateCashCurrencyValue).ToString());
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    SettlementDateCashCurrencyValue settlementDateCashCurrencyValue = new SettlementDateCashCurrencyValue();
                    settlementDateCashCurrencyValue.SettlementDateBaseCurrencyID = 0;
                    settlementDateCashCurrencyValue.SettlementDateLocalCurrencyID = 0;
                    settlementDateCashCurrencyValue.SettlementDate = string.Empty;

                    for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    {
                        string colName = ds.Tables[0].Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        Type dataType = propInfo.PropertyType;

                        if (dataType.FullName.Equals("System.String"))
                        {
                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[irow][icol].ToString()))
                            {
                                propInfo.SetValue(settlementDateCashCurrencyValue, string.Empty, null);
                            }
                            else
                            {
                                propInfo.SetValue(settlementDateCashCurrencyValue, ds.Tables[0].Rows[irow][icol].ToString().Trim(), null);

                            }
                        }
                        else if (dataType.FullName.Equals("System.Double"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(settlementDateCashCurrencyValue, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                double result;
                                blnIsTrue = double.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(settlementDateCashCurrencyValue, Convert.ToDouble(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(settlementDateCashCurrencyValue, 0, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("System.Int32"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(settlementDateCashCurrencyValue, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                int result;
                                blnIsTrue = int.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(settlementDateCashCurrencyValue, Convert.ToInt32(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(settlementDateCashCurrencyValue, 0, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("System.Int64"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(settlementDateCashCurrencyValue, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                Int64 result;
                                blnIsTrue = Int64.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(settlementDateCashCurrencyValue, Convert.ToInt64(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(settlementDateCashCurrencyValue, 0, null);
                                }
                            }
                        }
                    }


                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        settlementDateCashCurrencyValue.SettlementDate = _userSelectedDate;
                    }
                    else if (!settlementDateCashCurrencyValue.SettlementDate.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(settlementDateCashCurrencyValue.SettlementDate, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(settlementDateCashCurrencyValue.SettlementDate));
                            settlementDateCashCurrencyValue.SettlementDate = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(settlementDateCashCurrencyValue.SettlementDate);
                            settlementDateCashCurrencyValue.SettlementDate = dtn.ToString(DATEFORMAT);
                        }
                    }
                    if (_userSelectedAccountValue != int.MinValue)
                    {
                        settlementDateCashCurrencyValue.AccountName = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                        settlementDateCashCurrencyValue.AccountID = _userSelectedAccountValue;
                    }
                    else if (!string.IsNullOrEmpty(settlementDateCashCurrencyValue.AccountName))
                    {
                        settlementDateCashCurrencyValue.AccountID = CachedDataManager.GetInstance.GetAccountID(settlementDateCashCurrencyValue.AccountName.Trim());
                    }

                    if (!string.IsNullOrEmpty(settlementDateCashCurrencyValue.SettlementDateBaseCurrency))
                    {
                        settlementDateCashCurrencyValue.SettlementDateBaseCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(settlementDateCashCurrencyValue.SettlementDateBaseCurrency.Trim());
                    }
                    if (!string.IsNullOrEmpty(settlementDateCashCurrencyValue.SettlementDateLocalCurrency))
                    {
                        settlementDateCashCurrencyValue.SettlementDateLocalCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(settlementDateCashCurrencyValue.SettlementDateLocalCurrency.Trim());
                    }

                    #region FxCurrencyPairValidation

                    // If Fx currency pair validation is required at settlement date Cash import
                    //Jira: http://jira.nirvanasolutions.com:8080/browse/PRANA-7421

                    if (bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsFxRequiredForSettlementDateCashImport")))
                    {
                        if (settlementDateCashCurrencyValue.SettlementDateCashValueBase == 0 && settlementDateCashCurrencyValue.SettlementDateCashValueLocal != 0)
                        {
                            if (settlementDateCashCurrencyValue.SettlementDateLocalCurrencyID > 0 && settlementDateCashCurrencyValue.SettlementDateBaseCurrencyID > 0 && settlementDateCashCurrencyValue.SettlementDateLocalCurrencyID.Equals(settlementDateCashCurrencyValue.SettlementDateBaseCurrencyID))
                            {
                                settlementDateCashCurrencyValue.SettlementDateCashValueBase = settlementDateCashCurrencyValue.SettlementDateCashValueLocal;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(settlementDateCashCurrencyValue.SettlementDate))
                                {
                                    ForexConverter.GetInstance(_companyUser.CompanyID, Convert.ToDateTime(settlementDateCashCurrencyValue.SettlementDate)).GetForexData(Convert.ToDateTime(settlementDateCashCurrencyValue.SettlementDate));
                                    //CHMW-3132	Account wise fx rate handling for expiration settlement
                                    ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(_companyUser.CompanyID).GetConversionRateFromCurrenciesForGivenDate(settlementDateCashCurrencyValue.SettlementDateLocalCurrencyID, settlementDateCashCurrencyValue.SettlementDateBaseCurrencyID, Convert.ToDateTime(settlementDateCashCurrencyValue.SettlementDate), settlementDateCashCurrencyValue.AccountID);
                                    if (conversionRate != null)
                                    {
                                        settlementDateCashCurrencyValue.ForexConversionRate = conversionRate.RateValue;
                                        if (conversionRate.ConversionMethod == Operator.D)
                                        {
                                            if (conversionRate.RateValue != 0)
                                                settlementDateCashCurrencyValue.SettlementDateCashValueBase = settlementDateCashCurrencyValue.SettlementDateCashValueLocal / conversionRate.RateValue;
                                        }
                                        else
                                        {
                                            settlementDateCashCurrencyValue.SettlementDateCashValueBase = settlementDateCashCurrencyValue.SettlementDateCashValueLocal * conversionRate.RateValue;
                                        }
                                    }
                                }
                            }
                        }
                        if (settlementDateCashCurrencyValue.SettlementDateCashValueLocal == 0 && settlementDateCashCurrencyValue.SettlementDateCashValueBase != 0)
                        {
                            if (settlementDateCashCurrencyValue.SettlementDateLocalCurrencyID > 0 && settlementDateCashCurrencyValue.SettlementDateBaseCurrencyID > 0 && settlementDateCashCurrencyValue.SettlementDateLocalCurrencyID.Equals(settlementDateCashCurrencyValue.SettlementDateBaseCurrencyID))
                            {
                                settlementDateCashCurrencyValue.SettlementDateCashValueLocal = settlementDateCashCurrencyValue.SettlementDateCashValueBase;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(settlementDateCashCurrencyValue.SettlementDate))
                                {
                                    ForexConverter.GetInstance(_companyUser.CompanyID, Convert.ToDateTime(settlementDateCashCurrencyValue.SettlementDate)).GetForexData(Convert.ToDateTime(settlementDateCashCurrencyValue.SettlementDate));
                                    //CHMW-3132	Account wise fx rate handling for expiration settlement
                                    ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(_companyUser.CompanyID).GetConversionRateFromCurrenciesForGivenDate(settlementDateCashCurrencyValue.SettlementDateLocalCurrencyID, settlementDateCashCurrencyValue.SettlementDateBaseCurrencyID, Convert.ToDateTime(settlementDateCashCurrencyValue.SettlementDate), settlementDateCashCurrencyValue.AccountID);
                                    if (conversionRate != null)
                                    {
                                        settlementDateCashCurrencyValue.ForexConversionRate = conversionRate.RateValue;
                                        if (conversionRate.ConversionMethod == Operator.D)
                                        {
                                            if (conversionRate.RateValue != 0)
                                                settlementDateCashCurrencyValue.SettlementDateCashValueLocal = settlementDateCashCurrencyValue.SettlementDateCashValueBase / conversionRate.RateValue;
                                        }
                                        else
                                        {
                                            settlementDateCashCurrencyValue.SettlementDateCashValueLocal = settlementDateCashCurrencyValue.SettlementDateCashValueBase * conversionRate.RateValue;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    _settlementDateCashCurrencyValueCollection.Add(settlementDateCashCurrencyValue);
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

        Dictionary<int, Dictionary<string, List<MarkPriceImport>>> _markPriceSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<MarkPriceImport>>>();

        private void UpdateMarkPriceValueCollection(DataSet ds)
        {
            try
            {
                _markPriceSymbologyWiseDict.Clear();

                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(MarkPriceImport).ToString());
                DataTable dTable = ds.Tables[0];
                List<string> uniqueIdsList = new List<string>();

                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    MarkPriceImport markPriceValue = new MarkPriceImport();
                    markPriceValue.Symbol = string.Empty;
                    markPriceValue.MarkPrice = 0;
                    markPriceValue.ForwardPoints = 0;
                    markPriceValue.Date = string.Empty;
                    markPriceValue.AUECID = 0;
                    markPriceValue.AccountID = 0;
                    for (int icol = 0; icol < dTable.Columns.Count; icol++)
                    {
                        string colName = dTable.Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(dTable.Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(markPriceValue, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(markPriceValue, dTable.Rows[irow][icol].ToString().Trim(), null);
                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(markPriceValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result;
                                    blnIsTrue = double.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(markPriceValue, Convert.ToDouble(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(markPriceValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(markPriceValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result;
                                    blnIsTrue = int.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(markPriceValue, Convert.ToInt32(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(markPriceValue, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(markPriceValue, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result;
                                    blnIsTrue = Int64.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(markPriceValue, Convert.ToInt64(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(markPriceValue, 0, null);
                                    }
                                }
                            }
                        }
                    }

                    if (_userSelectedAccountValue != int.MinValue)
                    {
                        markPriceValue.AccountName = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                        markPriceValue.AccountID = _userSelectedAccountValue;
                    }

                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        markPriceValue.Date = _userSelectedDate;
                    }
                    else if (!markPriceValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(markPriceValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(markPriceValue.Date));
                            markPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(markPriceValue.Date);
                            markPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }

                    string uniqueID = string.Empty;

                    if (!String.IsNullOrEmpty(markPriceValue.Symbol))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.Symbol.Trim().ToUpper() + Seperators.SEPERATOR_13 + markPriceValue.AccountID.ToString().Trim().ToUpper();

                        if (_markPriceSymbologyWiseDict.ContainsKey(0))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[0];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.Symbol))
                            {
                                List<MarkPriceImport> markPriceSymbolWiseList = markPriceSameSymbologyDict[markPriceValue.Symbol];
                                markPriceSymbolWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.Symbol] = markPriceSymbolWiseList;
                                _markPriceSymbologyWiseDict[0] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[0].Add(markPriceValue.Symbol, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbolDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameSymbolDict.Add(markPriceValue.Symbol, markPricelist);
                            _markPriceSymbologyWiseDict.Add(0, markPriceSameSymbolDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.RIC))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.RIC.Trim().ToUpper() + Seperators.SEPERATOR_13 + markPriceValue.AccountID.ToString().Trim().ToUpper();

                        if (_markPriceSymbologyWiseDict.ContainsKey(1))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[1];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.RIC))
                            {
                                List<MarkPriceImport> markPriceRICWiseList = markPriceSameSymbologyDict[markPriceValue.RIC];
                                markPriceRICWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.RIC] = markPriceRICWiseList;
                                _markPriceSymbologyWiseDict[1] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPirceList = new List<MarkPriceImport>();
                                markPirceList.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[1].Add(markPriceValue.RIC, markPirceList);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPirceList = new List<MarkPriceImport>();
                            markPirceList.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameRICDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameRICDict.Add(markPriceValue.RIC, markPirceList);
                            _markPriceSymbologyWiseDict.Add(1, markPriceSameRICDict);
                        }

                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.ISIN))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.ISIN.Trim().ToUpper() + Seperators.SEPERATOR_13 + markPriceValue.AccountID.ToString().Trim().ToUpper();

                        if (_markPriceSymbologyWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[2];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.ISIN))
                            {
                                List<MarkPriceImport> markPriceISINWiseList = markPriceSameSymbologyDict[markPriceValue.ISIN];
                                markPriceISINWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.ISIN] = markPriceISINWiseList;
                                _markPriceSymbologyWiseDict[2] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[2].Add(markPriceValue.ISIN, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameISINDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameISINDict.Add(markPriceValue.ISIN, markPricelist);
                            _markPriceSymbologyWiseDict.Add(2, markPriceSameISINDict);
                        }

                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.SEDOL))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.SEDOL.Trim().ToUpper() + Seperators.SEPERATOR_13 + markPriceValue.AccountID.ToString().Trim().ToUpper();
                        if (_markPriceSymbologyWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[3];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.SEDOL))
                            {
                                List<MarkPriceImport> markPriceSEDOLWiseList = markPriceSameSymbologyDict[markPriceValue.SEDOL];
                                markPriceSEDOLWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.SEDOL] = markPriceSEDOLWiseList;
                                _markPriceSymbologyWiseDict[3] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[3].Add(markPriceValue.SEDOL, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSEDOLDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSEDOLDict.Add(markPriceValue.SEDOL, markPricelist);
                            _markPriceSymbologyWiseDict.Add(3, markPriceSEDOLDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.CUSIP))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.CUSIP.Trim().ToUpper() + Seperators.SEPERATOR_13 + markPriceValue.AccountID.ToString().Trim().ToUpper();
                        if (_markPriceSymbologyWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[4];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.CUSIP))
                            {
                                List<MarkPriceImport> markPriceCUSIPWiseList = markPriceSameSymbologyDict[markPriceValue.CUSIP];
                                markPriceCUSIPWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.CUSIP] = markPriceCUSIPWiseList;
                                _markPriceSymbologyWiseDict[4] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[4].Add(markPriceValue.CUSIP, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameCUSIPDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameCUSIPDict.Add(markPriceValue.CUSIP, markPricelist);
                            _markPriceSymbologyWiseDict.Add(4, markPriceSameCUSIPDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.Bloomberg))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.Bloomberg.Trim().ToUpper() + Seperators.SEPERATOR_13 + markPriceValue.AccountID.ToString().Trim().ToUpper();

                        if (_markPriceSymbologyWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[5];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.Bloomberg))
                            {
                                List<MarkPriceImport> markPriceBloombergWiseList = markPriceSameSymbologyDict[markPriceValue.Bloomberg];
                                markPriceBloombergWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.Bloomberg] = markPriceBloombergWiseList;
                                _markPriceSymbologyWiseDict[5] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[5].Add(markPriceValue.Bloomberg, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameBloombergDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameBloombergDict.Add(markPriceValue.Bloomberg, markPricelist);
                            _markPriceSymbologyWiseDict.Add(5, markPriceSameBloombergDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.OSIOptionSymbol))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.OSIOptionSymbol.Trim().ToUpper() + Seperators.SEPERATOR_13 + markPriceValue.AccountID.ToString().Trim().ToUpper();

                        if (_markPriceSymbologyWiseDict.ContainsKey(6))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[6];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.OSIOptionSymbol))
                            {
                                List<MarkPriceImport> markPriceOSIWiseList = markPriceSameSymbologyDict[markPriceValue.OSIOptionSymbol];
                                markPriceOSIWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.OSIOptionSymbol] = markPriceOSIWiseList;
                                _markPriceSymbologyWiseDict[6] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[6].Add(markPriceValue.OSIOptionSymbol, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameOSIDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameOSIDict.Add(markPriceValue.OSIOptionSymbol, markPricelist);
                            _markPriceSymbologyWiseDict.Add(6, markPriceSameOSIDict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.IDCOOptionSymbol))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.IDCOOptionSymbol.Trim().ToUpper() + Seperators.SEPERATOR_13 + markPriceValue.AccountID.ToString().Trim().ToUpper();

                        if (_markPriceSymbologyWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[7];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.IDCOOptionSymbol))
                            {
                                List<MarkPriceImport> markPriceIDCOWiseList = markPriceSameSymbologyDict[markPriceValue.IDCOOptionSymbol];
                                markPriceIDCOWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.IDCOOptionSymbol] = markPriceIDCOWiseList;
                                _markPriceSymbologyWiseDict[7] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[7].Add(markPriceValue.IDCOOptionSymbol, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameIDCODict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameIDCODict.Add(markPriceValue.IDCOOptionSymbol, markPricelist);
                            _markPriceSymbologyWiseDict.Add(7, markPriceSameIDCODict);
                        }
                    }
                    else if (!String.IsNullOrEmpty(markPriceValue.OpraOptionSymbol))
                    {
                        uniqueID = markPriceValue.Date + markPriceValue.OpraOptionSymbol.Trim().ToUpper() + Seperators.SEPERATOR_13 + markPriceValue.AccountID.ToString().Trim().ToUpper();

                        if (_markPriceSymbologyWiseDict.ContainsKey(8))
                        {
                            Dictionary<string, List<MarkPriceImport>> markPriceSameSymbologyDict = _markPriceSymbologyWiseDict[8];
                            if (markPriceSameSymbologyDict.ContainsKey(markPriceValue.OpraOptionSymbol))
                            {
                                List<MarkPriceImport> markPriceOpraWiseList = markPriceSameSymbologyDict[markPriceValue.OpraOptionSymbol];
                                markPriceOpraWiseList.Add(markPriceValue);
                                markPriceSameSymbologyDict[markPriceValue.OpraOptionSymbol] = markPriceOpraWiseList;
                                _markPriceSymbologyWiseDict[8] = markPriceSameSymbologyDict;
                            }
                            else
                            {
                                List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                                markPricelist.Add(markPriceValue);
                                _markPriceSymbologyWiseDict[8].Add(markPriceValue.OpraOptionSymbol, markPricelist);
                            }
                        }
                        else
                        {
                            List<MarkPriceImport> markPricelist = new List<MarkPriceImport>();
                            markPricelist.Add(markPriceValue);
                            Dictionary<string, List<MarkPriceImport>> markPriceSameOpraDict = new Dictionary<string, List<MarkPriceImport>>();
                            markPriceSameOpraDict.Add(markPriceValue.OpraOptionSymbol, markPricelist);
                            _markPriceSymbologyWiseDict.Add(7, markPriceSameOpraDict);
                        }
                    }


                    if (!uniqueIdsList.Contains(uniqueID) && !string.IsNullOrEmpty(uniqueID))
                    {
                        uniqueIdsList.Add(uniqueID);
                        _markPriceValueCollection.Add(markPriceValue);
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

        Dictionary<int, Dictionary<string, List<DividendImport>>> _dividendSymbologyWiseDict = new Dictionary<int, Dictionary<string, List<DividendImport>>>();

        private void UpdateDividendValueCollection(DataSet ds)
        {
            try
            {
                _dividendSymbologyWiseDict.Clear();

                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(DividendImport).ToString());
                DataTable dTable = ds.Tables[0];
                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    DividendImport dividendObj = new DividendImport();
                    dividendObj.Symbol = string.Empty;
                    dividendObj.Amount = 0;
                    dividendObj.CurrencyID = 0;
                    dividendObj.CurrencyName = string.Empty;
                    dividendObj.ExDate = string.Empty;
                    dividendObj.PayoutDate = string.Empty;
                    dividendObj.AUECID = 0;
                    dividendObj.AccountName = string.Empty;
                    dividendObj.FundID = 0;
                    dividendObj.ActivityTypeId = 0;
                    dividendObj.ActivityType = string.Empty;

                    for (int icol = 0; icol < dTable.Columns.Count; icol++)
                    {
                        string colName = dTable.Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        if (propInfo != null)
                        {
                            Type dataType = propInfo.PropertyType;

                            if (dataType.FullName.Equals("System.String"))
                            {
                                if (String.IsNullOrEmpty(dTable.Rows[irow][icol].ToString()))
                                {
                                    propInfo.SetValue(dividendObj, string.Empty, null);
                                }
                                else
                                {
                                    propInfo.SetValue(dividendObj, dTable.Rows[irow][icol].ToString().Trim(), null);
                                }
                            }
                            else if (dataType.FullName.Equals("System.Double"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(dividendObj, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    double result;
                                    blnIsTrue = double.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(dividendObj, Convert.ToDouble(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(dividendObj, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int32"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(dividendObj, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    int result;
                                    blnIsTrue = int.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(dividendObj, Convert.ToInt32(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(dividendObj, 0, null);
                                    }
                                }
                            }
                            else if (dataType.FullName.Equals("System.Int64"))
                            {
                                if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                                {
                                    propInfo.SetValue(dividendObj, 0, null);
                                }
                                else
                                {
                                    bool blnIsTrue;
                                    Int64 result;
                                    blnIsTrue = Int64.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                    if (blnIsTrue)
                                    {
                                        propInfo.SetValue(dividendObj, Convert.ToInt64(dTable.Rows[irow][icol]), null);
                                    }
                                    else
                                    {
                                        propInfo.SetValue(dividendObj, 0, null);
                                    }
                                }
                            }
                        }
                    }

                    if (dividendObj.UserId == 0)
                        dividendObj.UserId = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;

                    // Only blank Dates are updated from UI

                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        dividendObj.ExDate = _userSelectedDate;
                    }
                    else if (!string.IsNullOrEmpty(dividendObj.ExDate))
                    {  // ExDate Parsing
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(dividendObj.ExDate, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(dividendObj.ExDate));
                            dividendObj.ExDate = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(dividendObj.ExDate);
                            dividendObj.ExDate = dtn.ToString(DATEFORMAT);
                        }
                    }

                    // Only blank Dates are updated from UI
                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        dividendObj.PayoutDate = _userSelectedDate;
                    }
                    // PayoutDate Parsing 
                    else if (!string.IsNullOrEmpty(dividendObj.PayoutDate))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(dividendObj.PayoutDate, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(dividendObj.PayoutDate));
                            dividendObj.PayoutDate = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(dividendObj.PayoutDate);
                            dividendObj.PayoutDate = dtn.ToString(DATEFORMAT);
                        }
                    }

                    // Only blank Dates are updated from UI
                    //exdate and payout will be available only for dividends i.e. there should be symbol
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-2629
                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty) && !string.IsNullOrEmpty(dividendObj.Symbol))
                    {
                        dividendObj.RecordDate = _userSelectedDate;
                    }
                    else if (!string.IsNullOrEmpty(dividendObj.RecordDate) && !string.IsNullOrEmpty(dividendObj.Symbol))
                    {  // RecordDate Parsing
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(dividendObj.RecordDate, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(dividendObj.RecordDate));
                            dividendObj.RecordDate = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(dividendObj.RecordDate);
                            dividendObj.RecordDate = dtn.ToString(DATEFORMAT);
                        }
                    }
                    // Only blank Dates are updated from UI
                    //exdate and payout will be available only for dividends i.e. there should be symbol
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-2629
                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty) && !string.IsNullOrEmpty(dividendObj.Symbol))
                    {
                        dividendObj.DeclarationDate = _userSelectedDate;
                    }
                    // DeclarationDate Parsing 
                    else if (!string.IsNullOrEmpty(dividendObj.DeclarationDate) && !string.IsNullOrEmpty(dividendObj.Symbol))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(dividendObj.DeclarationDate, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(dividendObj.DeclarationDate));
                            dividendObj.DeclarationDate = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(dividendObj.DeclarationDate);
                            dividendObj.DeclarationDate = dtn.ToString(DATEFORMAT);
                        }
                    }

                    if (_userSelectedAccountValue != int.MinValue)
                    {
                        dividendObj.FundID = _userSelectedAccountValue;
                        dividendObj.AccountName = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                    }
                    else if (!String.IsNullOrEmpty(dividendObj.AccountName))
                    {
                        dividendObj.FundID = CachedDataManager.GetInstance.GetAccountID(dividendObj.AccountName.Trim());
                    }
                    //Fill ActivityTypeId from cache based on ActivityType
                    if (dividendObj.ActivityTypeId <= 0)
                    {
                        dividendObj.ActivityTypeId = CachedDataManager.GetActivityTypeID(dividendObj.ActivityType.Trim());
                    }

                    if (dividendObj.CurrencyID > 0)
                    {
                        dividendObj.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(dividendObj.CurrencyID);
                    }
                    else if (!string.IsNullOrWhiteSpace(dividendObj.CurrencyName))
                    {
                        dividendObj.CurrencyID = CachedDataManager.GetInstance.GetCurrencyID(dividendObj.CurrencyName);
                    }

                    // Key: 0 for Ticker
                    if (!String.IsNullOrEmpty(dividendObj.Symbol))
                    {
                        if (_dividendSymbologyWiseDict.ContainsKey(0))
                        {
                            Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[0];
                            if (dividendSameSymbologyDict.ContainsKey(dividendObj.Symbol))
                            {
                                List<DividendImport> dividendSymbolWiseList = dividendSameSymbologyDict[dividendObj.Symbol];
                                dividendSymbolWiseList.Add(dividendObj);
                                dividendSameSymbologyDict[dividendObj.Symbol] = dividendSymbolWiseList;
                                _dividendSymbologyWiseDict[0] = dividendSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                _dividendSymbologyWiseDict[0].Add(dividendObj.Symbol, dividendlist);
                            }
                        }
                        else
                        {
                            List<DividendImport> dividendlist = new List<DividendImport>();
                            dividendlist.Add(dividendObj);
                            Dictionary<string, List<DividendImport>> dividendSameSymbolDict = new Dictionary<string, List<DividendImport>>();
                            dividendSameSymbolDict.Add(dividendObj.Symbol, dividendlist);
                            _dividendSymbologyWiseDict.Add(0, dividendSameSymbolDict);
                        }
                    }
                    // Key: 1 for RIC
                    else if (!String.IsNullOrEmpty(dividendObj.RIC))
                    {
                        if (_dividendSymbologyWiseDict.ContainsKey(1))
                        {
                            Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[1];
                            if (dividendSameSymbologyDict.ContainsKey(dividendObj.RIC))
                            {
                                List<DividendImport> dividendRICWiseList = dividendSameSymbologyDict[dividendObj.RIC];
                                dividendRICWiseList.Add(dividendObj);
                                dividendSameSymbologyDict[dividendObj.RIC] = dividendRICWiseList;
                                _dividendSymbologyWiseDict[1] = dividendSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendImport> dividendList = new List<DividendImport>();
                                dividendList.Add(dividendObj);
                                _dividendSymbologyWiseDict[1].Add(dividendObj.RIC, dividendList);
                            }
                        }
                        else
                        {
                            List<DividendImport> dividendList = new List<DividendImport>();
                            dividendList.Add(dividendObj);
                            Dictionary<string, List<DividendImport>> dividendSameRICDict = new Dictionary<string, List<DividendImport>>();
                            dividendSameRICDict.Add(dividendObj.RIC, dividendList);
                            _dividendSymbologyWiseDict.Add(1, dividendSameRICDict);
                        }

                    }
                    // Key: 2 for ISIN
                    else if (!String.IsNullOrEmpty(dividendObj.ISIN))
                    {
                        if (_dividendSymbologyWiseDict.ContainsKey(2))
                        {
                            Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[2];
                            if (dividendSameSymbologyDict.ContainsKey(dividendObj.ISIN))
                            {
                                List<DividendImport> dividendISINWiseList = dividendSameSymbologyDict[dividendObj.ISIN];
                                dividendISINWiseList.Add(dividendObj);
                                dividendSameSymbologyDict[dividendObj.ISIN] = dividendISINWiseList;
                                _dividendSymbologyWiseDict[2] = dividendSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                _dividendSymbologyWiseDict[2].Add(dividendObj.ISIN, dividendlist);
                            }
                        }
                        else
                        {
                            List<DividendImport> dividendlist = new List<DividendImport>();
                            dividendlist.Add(dividendObj);
                            Dictionary<string, List<DividendImport>> dividendSameISINDict = new Dictionary<string, List<DividendImport>>();
                            dividendSameISINDict.Add(dividendObj.ISIN, dividendlist);
                            _dividendSymbologyWiseDict.Add(2, dividendSameISINDict);
                        }

                    }
                    // Key: 3 for SEDOL
                    else if (!String.IsNullOrEmpty(dividendObj.SEDOL))
                    {
                        if (_dividendSymbologyWiseDict.ContainsKey(3))
                        {
                            Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[3];
                            if (dividendSameSymbologyDict.ContainsKey(dividendObj.SEDOL))
                            {
                                List<DividendImport> dividendSEDOLWiseList = dividendSameSymbologyDict[dividendObj.SEDOL];
                                dividendSEDOLWiseList.Add(dividendObj);
                                dividendSameSymbologyDict[dividendObj.SEDOL] = dividendSEDOLWiseList;
                                _dividendSymbologyWiseDict[3] = dividendSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                _dividendSymbologyWiseDict[3].Add(dividendObj.SEDOL, dividendlist);
                            }
                        }
                        else
                        {
                            List<DividendImport> dividendlist = new List<DividendImport>();
                            dividendlist.Add(dividendObj);
                            Dictionary<string, List<DividendImport>> dividendSEDOLDict = new Dictionary<string, List<DividendImport>>();
                            dividendSEDOLDict.Add(dividendObj.SEDOL, dividendlist);
                            _dividendSymbologyWiseDict.Add(3, dividendSEDOLDict);
                        }
                    }
                    // Key: 4 for CUSIP
                    else if (!String.IsNullOrEmpty(dividendObj.CUSIP))
                    {
                        if (_dividendSymbologyWiseDict.ContainsKey(4))
                        {
                            Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[4];
                            if (dividendSameSymbologyDict.ContainsKey(dividendObj.CUSIP))
                            {
                                List<DividendImport> dividendCUSIPWiseList = dividendSameSymbologyDict[dividendObj.CUSIP];
                                dividendCUSIPWiseList.Add(dividendObj);
                                dividendSameSymbologyDict[dividendObj.CUSIP] = dividendCUSIPWiseList;
                                _dividendSymbologyWiseDict[4] = dividendSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                _dividendSymbologyWiseDict[4].Add(dividendObj.CUSIP, dividendlist);
                            }
                        }
                        else
                        {
                            List<DividendImport> dividendlist = new List<DividendImport>();
                            dividendlist.Add(dividendObj);
                            Dictionary<string, List<DividendImport>> dividendSameCUSIPDict = new Dictionary<string, List<DividendImport>>();
                            dividendSameCUSIPDict.Add(dividendObj.CUSIP, dividendlist);
                            _dividendSymbologyWiseDict.Add(4, dividendSameCUSIPDict);
                        }
                    }
                    // Key: 5 for Bloomberg
                    else if (!String.IsNullOrEmpty(dividendObj.Bloomberg))
                    {
                        if (_dividendSymbologyWiseDict.ContainsKey(5))
                        {
                            Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[5];
                            if (dividendSameSymbologyDict.ContainsKey(dividendObj.Bloomberg))
                            {
                                List<DividendImport> dividendBloombergWiseList = dividendSameSymbologyDict[dividendObj.Bloomberg];
                                dividendBloombergWiseList.Add(dividendObj);
                                dividendSameSymbologyDict[dividendObj.Bloomberg] = dividendBloombergWiseList;
                                _dividendSymbologyWiseDict[5] = dividendSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                _dividendSymbologyWiseDict[5].Add(dividendObj.Bloomberg, dividendlist);
                            }
                        }
                        else
                        {
                            List<DividendImport> dividendlist = new List<DividendImport>();
                            dividendlist.Add(dividendObj);
                            Dictionary<string, List<DividendImport>> dividendSameBloombergDict = new Dictionary<string, List<DividendImport>>();
                            dividendSameBloombergDict.Add(dividendObj.Bloomberg, dividendlist);
                            _dividendSymbologyWiseDict.Add(5, dividendSameBloombergDict);
                        }
                    }
                    // Key: 6 for OSIOptionSymbol
                    else if (!String.IsNullOrEmpty(dividendObj.OSIOptionSymbol))
                    {
                        if (_dividendSymbologyWiseDict.ContainsKey(6))
                        {
                            Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[6];
                            if (dividendSameSymbologyDict.ContainsKey(dividendObj.OSIOptionSymbol))
                            {
                                List<DividendImport> dividendOSIWiseList = dividendSameSymbologyDict[dividendObj.OSIOptionSymbol];
                                dividendOSIWiseList.Add(dividendObj);
                                dividendSameSymbologyDict[dividendObj.OSIOptionSymbol] = dividendOSIWiseList;
                                _dividendSymbologyWiseDict[6] = dividendSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                _dividendSymbologyWiseDict[6].Add(dividendObj.OSIOptionSymbol, dividendlist);
                            }
                        }
                        else
                        {
                            List<DividendImport> dividendlist = new List<DividendImport>();
                            dividendlist.Add(dividendObj);
                            Dictionary<string, List<DividendImport>> dividendSameOSIDict = new Dictionary<string, List<DividendImport>>();
                            dividendSameOSIDict.Add(dividendObj.OSIOptionSymbol, dividendlist);
                            _dividendSymbologyWiseDict.Add(6, dividendSameOSIDict);
                        }
                    }
                    // Key: 7 for IDCOOptionSymbol
                    else if (!String.IsNullOrEmpty(dividendObj.IDCOOptionSymbol))
                    {
                        if (_dividendSymbologyWiseDict.ContainsKey(7))
                        {
                            Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[7];
                            if (dividendSameSymbologyDict.ContainsKey(dividendObj.IDCOOptionSymbol))
                            {
                                List<DividendImport> dividendIDCOWiseList = dividendSameSymbologyDict[dividendObj.IDCOOptionSymbol];
                                dividendIDCOWiseList.Add(dividendObj);
                                dividendSameSymbologyDict[dividendObj.IDCOOptionSymbol] = dividendIDCOWiseList;
                                _dividendSymbologyWiseDict[7] = dividendSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                _dividendSymbologyWiseDict[7].Add(dividendObj.IDCOOptionSymbol, dividendlist);
                            }
                        }
                        else
                        {
                            List<DividendImport> dividendlist = new List<DividendImport>();
                            dividendlist.Add(dividendObj);
                            Dictionary<string, List<DividendImport>> dividendSameIDCODict = new Dictionary<string, List<DividendImport>>();
                            dividendSameIDCODict.Add(dividendObj.IDCOOptionSymbol, dividendlist);
                            _dividendSymbologyWiseDict.Add(7, dividendSameIDCODict);
                        }
                    }
                    // Key: 8 for OpraOptionSymbol
                    else if (!String.IsNullOrEmpty(dividendObj.OpraOptionSymbol))
                    {
                        if (_dividendSymbologyWiseDict.ContainsKey(8))
                        {
                            Dictionary<string, List<DividendImport>> dividendSameSymbologyDict = _dividendSymbologyWiseDict[8];
                            if (dividendSameSymbologyDict.ContainsKey(dividendObj.OpraOptionSymbol))
                            {
                                List<DividendImport> dividendOpraWiseList = dividendSameSymbologyDict[dividendObj.OpraOptionSymbol];
                                dividendOpraWiseList.Add(dividendObj);
                                dividendSameSymbologyDict[dividendObj.OpraOptionSymbol] = dividendOpraWiseList;
                                _dividendSymbologyWiseDict[8] = dividendSameSymbologyDict;
                            }
                            else
                            {
                                List<DividendImport> dividendlist = new List<DividendImport>();
                                dividendlist.Add(dividendObj);
                                _dividendSymbologyWiseDict[8].Add(dividendObj.OpraOptionSymbol, dividendlist);
                            }
                        }
                        else
                        {
                            List<DividendImport> dividendlist = new List<DividendImport>();
                            dividendlist.Add(dividendObj);
                            Dictionary<string, List<DividendImport>> dividendSameOpraDict = new Dictionary<string, List<DividendImport>>();
                            dividendSameOpraDict.Add(dividendObj.OpraOptionSymbol, dividendlist);
                            _dividendSymbologyWiseDict.Add(8, dividendSameOpraDict);
                        }
                    }

                    _dividendValueCollection.Add(dividendObj);
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

        private void UpdateForexPriceValueCollection(DataSet ds)
        {
            try
            {
                List<string> currencyStandardPairs = RunUploadManager.GetCurrencyStandardPairs();

                List<string> forexUniqueIdsList = new List<string>();

                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(ForexPriceImport).ToString());
                DataTable dTable = ds.Tables[0];

                for (int irow = 0; irow < dTable.Rows.Count; irow++)
                {
                    ForexPriceImport forexPriceValue = new ForexPriceImport();
                    forexPriceValue.BaseCurrencyID = 0;
                    forexPriceValue.SettlementCurrencyID = 0;
                    forexPriceValue.ForexPrice = 0;
                    forexPriceValue.Date = string.Empty;
                    forexPriceValue.AccountID = 0;
                    for (int icol = 0; icol < dTable.Columns.Count; icol++)
                    {
                        string colName = dTable.Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        Type dataType = propInfo.PropertyType;

                        if (dataType.FullName.Equals("System.String"))
                        {
                            if (String.IsNullOrEmpty(dTable.Rows[irow][icol].ToString()))
                            {
                                propInfo.SetValue(forexPriceValue, string.Empty, null);
                            }
                            else
                            {
                                propInfo.SetValue(forexPriceValue, dTable.Rows[irow][icol].ToString().Trim(), null);
                            }
                        }
                        else if (dataType.FullName.Equals("System.Double"))
                        {
                            if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(forexPriceValue, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                double result;
                                blnIsTrue = double.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(forexPriceValue, Convert.ToDouble(dTable.Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(forexPriceValue, 0, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("System.Int32"))
                        {
                            if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(forexPriceValue, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                int result;
                                blnIsTrue = int.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(forexPriceValue, Convert.ToInt32(dTable.Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(forexPriceValue, 0, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("System.Int64"))
                        {
                            if (dTable.Rows[irow][icol].Equals(string.Empty) || dTable.Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(forexPriceValue, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                Int64 result;
                                blnIsTrue = Int64.TryParse(dTable.Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(forexPriceValue, Convert.ToInt64(dTable.Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(forexPriceValue, 0, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("Prana.BusinessObjects.AppConstants.Operator"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(forexPriceValue, Operator.M, null);
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[irow][icol].ToString().Trim().ToUpper().Equals(Operator.M.ToString()))
                                {
                                    propInfo.SetValue(forexPriceValue, Operator.M, null);
                                }
                                else if (ds.Tables[0].Rows[irow][icol].ToString().Trim().ToUpper().Equals(Operator.D.ToString()))
                                {
                                    propInfo.SetValue(forexPriceValue, Operator.D, null);
                                }

                            }
                        }
                    }

                    if (_userSelectedAccountValue != int.MinValue)
                    {
                        forexPriceValue.AccountName = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                        forexPriceValue.AccountID = _userSelectedAccountValue;
                    }
                    if (_isUserSelectedDate.ToUpper().Equals(CAPTION_True) && !_userSelectedDate.Equals(String.Empty))
                    {
                        forexPriceValue.Date = _userSelectedDate;
                    }
                    else if (!forexPriceValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(forexPriceValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(forexPriceValue.Date));
                            forexPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(forexPriceValue.Date);
                            forexPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }
                    if (!string.IsNullOrEmpty(forexPriceValue.BaseCurrency))
                    {
                        forexPriceValue.BaseCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(forexPriceValue.BaseCurrency.Trim());
                    }
                    if (!string.IsNullOrEmpty(forexPriceValue.SettlementCurrency))
                    {
                        forexPriceValue.SettlementCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(forexPriceValue.SettlementCurrency.Trim());
                    }

                    if (forexPriceValue.FXConversionMethodOperator.Equals(Prana.BusinessObjects.AppConstants.Operator.D) && forexPriceValue.ForexPrice != 0)
                    {
                        forexPriceValue.ForexPrice = 1 / forexPriceValue.ForexPrice;
                    }

                    if (forexPriceValue.ForexPrice > 0)
                    {
                        string forexUniqueID = forexPriceValue.Date + forexPriceValue.BaseCurrencyID + Seperators.SEPERATOR_7 + forexPriceValue.SettlementCurrencyID + Seperators.SEPERATOR_13 + forexPriceValue.AccountID.ToString().Trim().ToUpper();
                        if (!forexUniqueIdsList.Contains(forexUniqueID))
                        {
                            forexUniqueIdsList.Add(forexUniqueID);

                            if (forexPriceValue.BaseCurrencyID > 0 && forexPriceValue.SettlementCurrencyID > 0 && forexPriceValue.BaseCurrencyID != forexPriceValue.SettlementCurrencyID)
                            {
                                string uniqueID = forexPriceValue.BaseCurrencyID + Seperators.SEPERATOR_7 + forexPriceValue.SettlementCurrencyID;
                                if (currencyStandardPairs.Contains(uniqueID))
                                {
                                    _forexPriceValueCollection.Add(forexPriceValue);
                                }
                                else
                                {
                                    ForexPriceCurrencyStandardPairCheck(currencyStandardPairs, forexPriceValue);
                                }
                            }
                            else
                            {
                                _forexPriceValueCollection.Add(forexPriceValue);
                                forexPriceValue.Validated = "Standard Currency Pairs does not exists in the application";
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

        private void ForexPriceCurrencyStandardPairCheck(List<string> currencyStandardPairs, ForexPriceImport forexPriceValue)
        {
            //  to restore the old value,if currency pair does not exits in the application
            double storedForexValue = double.MinValue;

            ///Condition Commented by Ashish. 16th Feb 09
            //if (forexPriceValue.FXConversionMethodOperator.Equals(Prana.BusinessObjects.AppConstants.Operator.M) && forexPriceValue.ForexPrice != 0)
            //{
            //    storedForexValue = forexPriceValue.ForexPrice;
            //    forexPriceValue.ForexPrice = 1 / forexPriceValue.ForexPrice;
            //}
            int baseCurrencyID = forexPriceValue.BaseCurrencyID;
            forexPriceValue.BaseCurrencyID = forexPriceValue.SettlementCurrencyID;
            forexPriceValue.SettlementCurrencyID = baseCurrencyID;

            string baseCurrency = forexPriceValue.BaseCurrency;
            forexPriceValue.BaseCurrency = forexPriceValue.SettlementCurrency;
            forexPriceValue.SettlementCurrency = baseCurrency;

            //Added by Ashish. 
            storedForexValue = forexPriceValue.ForexPrice;
            forexPriceValue.ForexPrice = 1 / storedForexValue;

            // new ID 
            string uniqueID = forexPriceValue.BaseCurrencyID + Seperators.SEPERATOR_7 + forexPriceValue.SettlementCurrencyID;
            if (currencyStandardPairs.Contains(uniqueID))
            {
                _forexPriceValueCollection.Add(forexPriceValue);
            }
            else // if currency standard pair does not exits in the cache, then restore old values and display the same
            {
                int currencyID = forexPriceValue.BaseCurrencyID;
                forexPriceValue.BaseCurrencyID = forexPriceValue.SettlementCurrencyID;
                forexPriceValue.SettlementCurrencyID = currencyID;

                string currency = forexPriceValue.BaseCurrency;
                forexPriceValue.BaseCurrency = forexPriceValue.SettlementCurrency;
                forexPriceValue.SettlementCurrency = currency;

                if (storedForexValue != double.MinValue)
                {
                    forexPriceValue.ForexPrice = storedForexValue;
                }
                _forexPriceValueCollection.Add(forexPriceValue);
                forexPriceValue.Validated = "Standard Currency Pairs does not exists in the application";
            }
        }

        private void UpdateDailyCreditLimitCollection(DataSet ds)
        {
            try
            {
                Dictionary<string, DailyCreditLimit> dailyCreditLimitCollection = new Dictionary<string, DailyCreditLimit>();

                Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "Prana.BusinessObjects.dll");
                Type typeToLoad = assembly.GetType(typeof(DailyCreditLimit).ToString());
                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                {
                    DailyCreditLimit dailyCreditLimit = new DailyCreditLimit();

                    for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                    {
                        string colName = ds.Tables[0].Columns[icol].ColumnName.ToString();

                        // assign into property
                        PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                        Type dataType = propInfo.PropertyType;

                        if (dataType.FullName.Equals("System.String"))
                        {
                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[irow][icol].ToString()))
                            {
                                propInfo.SetValue(dailyCreditLimit, string.Empty, null);
                            }
                            else
                            {
                                propInfo.SetValue(dailyCreditLimit, ds.Tables[0].Rows[irow][icol].ToString().Trim(), null);
                            }
                        }
                        else if (dataType.FullName.Equals("System.Double"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(dailyCreditLimit, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                double result;
                                blnIsTrue = double.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(dailyCreditLimit, Convert.ToDouble(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(dailyCreditLimit, 0, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("System.Int32"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(dailyCreditLimit, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                int result;
                                blnIsTrue = int.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(dailyCreditLimit, Convert.ToInt32(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(dailyCreditLimit, 0, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("System.Int64"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(dailyCreditLimit, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                Int64 result;
                                blnIsTrue = Int64.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(dailyCreditLimit, Convert.ToInt64(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(dailyCreditLimit, 0, null);
                                }
                            }
                        }
                    }

                    if (_userSelectedAccountValue != int.MinValue)
                    {
                        dailyCreditLimit.AccountID = _userSelectedAccountValue;
                        dailyCreditLimit.AccountName = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                    }
                    else if (!String.IsNullOrEmpty(dailyCreditLimit.AccountName))
                    {
                        dailyCreditLimit.AccountID = CachedDataManager.GetInstance.GetAccountID(dailyCreditLimit.AccountName.Trim());
                    }

                    if (dailyCreditLimitCollection.ContainsKey(dailyCreditLimit.AccountName))
                    {
                        if (dailyCreditLimit.LongDebitBalance != 0)
                            dailyCreditLimitCollection[dailyCreditLimit.AccountName].LongDebitBalance = dailyCreditLimit.LongDebitBalance;
                        else if (dailyCreditLimit.ShortCreditBalance != 0)
                            dailyCreditLimitCollection[dailyCreditLimit.AccountName].ShortCreditBalance = dailyCreditLimit.ShortCreditBalance;
                    }
                    else
                    {
                        dailyCreditLimitCollection.Add(dailyCreditLimit.AccountName, dailyCreditLimit);
                    }
                }

                foreach (KeyValuePair<string, DailyCreditLimit> kvp in dailyCreditLimitCollection)
                {
                    _dailyCreditLimitCollection.Add(kvp.Value);
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

        enum EnmImportType
        {
            PositionImport,
            TransactionImport,
            CashImport,
            DividendImport,
            MarkPriceImport,
            BetaImport,
            OMIImport,
            SecMasterUpdateData,
            SecMasterInsertData,
            AllocationScheme,
            AllocationScheme_AppPositions,
            VolatilityImport,
            DividendYieldImport,
            StageImport,
            VWAPImport,
            CollateralImport
        }



        /// <summary>
        /// Same method called when async data received from Security Master
        /// </summary>
        /// <param name="secMasterObj"></param>
        internal void FillSecurityMasterDataFromObj(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();
                string cUSIPSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.CUSIPSymbol].ToString();
                string isinSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ISINSymbol].ToString();
                string sedolSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.SEDOLSymbol].ToString();
                string bloombergSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol].ToString();
                string reutersSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.ReutersSymbol].ToString();
                string osiOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OSIOptionSymbol].ToString();
                string idcoOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].ToString();
                string opraOptionSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.SymbologyCodes.OPRAOptionSymbol].ToString();

                string requestedSymbol = secMasterObj.RequestedSymbol.ToUpper();
                int requestedSymbologyID = secMasterObj.RequestedSymbology;

                if (!String.IsNullOrEmpty(pranaSymbol))
                {
                    switch (_SMRequest)
                    {
                        case "PositionImport":
                            if (_positionMasterSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<PositionMaster>> dictSymbols = _positionMasterSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(requestedSymbol))
                                {
                                    List<PositionMaster> listPosMaster = dictSymbols[requestedSymbol];
                                    foreach (PositionMaster positionMaster in listPosMaster)
                                    {
                                        positionMaster.Symbol = pranaSymbol;
                                        positionMaster.CUSIP = cUSIPSymbol;
                                        positionMaster.ISIN = isinSymbol;
                                        positionMaster.SEDOL = sedolSymbol;
                                        positionMaster.Bloomberg = bloombergSymbol;
                                        positionMaster.RIC = reutersSymbol;
                                        positionMaster.OSIOptionSymbol = osiOptionSymbol;
                                        positionMaster.IDCOOptionSymbol = idcoOptionSymbol;
                                        positionMaster.OpraOptionSymbol = opraOptionSymbol;
                                        positionMaster.UnderlyingSymbol = secMasterObj.UnderLyingSymbol;
                                        UpdatePositionMasterObj(positionMaster, secMasterObj);
                                    }
                                }
                            }
                            break;

                        case "DividendImport":
                            if (_dividendSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<DividendImport>> dictSymbols = _dividendSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<DividendImport> listDividend = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (DividendImport dividendImport in listDividend)
                                    {
                                        dividendImport.Symbol = pranaSymbol;
                                        dividendImport.CUSIP = cUSIPSymbol;
                                        dividendImport.ISIN = isinSymbol;
                                        dividendImport.SEDOL = sedolSymbol;
                                        dividendImport.Bloomberg = bloombergSymbol;
                                        dividendImport.RIC = reutersSymbol;
                                        dividendImport.AUECID = secMasterObj.AUECID;
                                        dividendImport.OSIOptionSymbol = osiOptionSymbol;
                                        dividendImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        dividendImport.OpraOptionSymbol = opraOptionSymbol;
                                        UpdateDividendImportObj(secMasterObj, dividendImport);

                                    }
                                }
                            }
                            break;

                        case "BetaImport":
                            if (_betaSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<BetaImport>> dictSymbols = _betaSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<BetaImport> listBeta = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (BetaImport betaImport in listBeta)
                                    {
                                        betaImport.Symbol = pranaSymbol;
                                        betaImport.CUSIP = cUSIPSymbol;
                                        betaImport.ISIN = isinSymbol;
                                        betaImport.SEDOL = sedolSymbol;
                                        betaImport.Bloomberg = bloombergSymbol;
                                        betaImport.RIC = reutersSymbol;
                                        betaImport.AUECID = secMasterObj.AUECID;
                                        betaImport.OSIOptionSymbol = osiOptionSymbol;
                                        betaImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        betaImport.OpraOptionSymbol = opraOptionSymbol;
                                    }
                                }
                            }
                            break;

                        case "MarkPriceImport":
                            if (_markPriceSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<MarkPriceImport>> dictSymbols = _markPriceSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<MarkPriceImport> listMarkPrice = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (MarkPriceImport markPriceImport in listMarkPrice)
                                    {
                                        markPriceImport.Symbol = pranaSymbol;
                                        markPriceImport.CUSIP = cUSIPSymbol;
                                        markPriceImport.ISIN = isinSymbol;
                                        markPriceImport.SEDOL = sedolSymbol;
                                        markPriceImport.Bloomberg = bloombergSymbol;
                                        markPriceImport.RIC = reutersSymbol;
                                        markPriceImport.AUECID = secMasterObj.AUECID;
                                        markPriceImport.OSIOptionSymbol = osiOptionSymbol;
                                        markPriceImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        markPriceImport.OpraOptionSymbol = opraOptionSymbol;
                                        markPriceImport.IsSecApproved = secMasterObj.IsSecApproved;
                                        if (markPriceImport.IsForexRequired.Trim().ToUpper().Equals("TRUE"))
                                            UpdateMarkPriceObj(markPriceImport, secMasterObj);

                                    }
                                }
                            }
                            break;
                        case "OMIImport":

                            if (_omiSymbolWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, UserOptModelInput> dictOMIInputs = _omiSymbolWiseDict[requestedSymbologyID];
                                if (dictOMIInputs.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    UserOptModelInput userOptModelInput = dictOMIInputs[secMasterObj.RequestedSymbol];
                                    userOptModelInput.AuecID = secMasterObj.AUECID;
                                    userOptModelInput.Symbol = pranaSymbol;
                                    userOptModelInput.CUSIP = cUSIPSymbol;
                                    userOptModelInput.ISIN = isinSymbol;
                                    userOptModelInput.SEDOL = sedolSymbol;
                                    userOptModelInput.Bloomberg = bloombergSymbol;
                                    userOptModelInput.RIC = reutersSymbol;
                                    userOptModelInput.OSIOptionSymbol = osiOptionSymbol;
                                    userOptModelInput.IDCOOptionSymbol = idcoOptionSymbol;
                                    userOptModelInput.OpraOptionSymbol = opraOptionSymbol;
                                }
                            }

                            break;

                        case "SecMasterUpdateData":

                            if (_secMasterUpdateDatauniqueSymbolDict.ContainsKey(secMasterObj.RequestedSymbol))
                            {
                                SecMasterUpdateDataByImportUI secMasterUpdateDataObj = _secMasterUpdateDatauniqueSymbolDict[secMasterObj.RequestedSymbol];
                                secMasterUpdateDataObj.TickerSymbol = secMasterObj.TickerSymbol;
                                secMasterUpdateDataObj.AUECID = secMasterObj.AUECID;
                                secMasterUpdateDataObj.ExistingLongName = secMasterObj.LongName;
                                secMasterUpdateDataObj.ExistingMultiplier = secMasterObj.Multiplier;
                                secMasterUpdateDataObj.ExistingUnderlyingSymbol = secMasterObj.UnderLyingSymbol;
                            }

                            break;
                        case "SecMasterInsertData":

                            if (_uniqueSymbolDictForSecMasterInsert.ContainsKey(secMasterObj.RequestedSymbol))
                            {
                                DataRow[] rows = _dsSecMasterInsert.Tables[0].Select("TickerSymbol=" + "'" + secMasterObj.RequestedSymbol + "'");
                                foreach (DataRow row in rows)
                                {
                                    if (row["TickerSymbol"].ToString().Equals(secMasterObj.RequestedSymbol))
                                    {
                                        row["SymbolExistsInSM"] = "Exists";
                                    }
                                }
                            }
                            break;
                        case "AllocationScheme":
                        case "AllocationScheme_AppPositions":
                            if (_allocationSchemeSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<DataRow>> dictSymbols = _allocationSchemeSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<DataRow> listDataRow = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (DataRow row in listDataRow)
                                    {
                                        row["Symbol"] = pranaSymbol;
                                        row["IsSymbolValidatedFromSM"] = "Validated";
                                    }
                                }
                            }
                            break;

                        case "VolatilityImport":
                            if (_volatilitySymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<VolatilityImport>> dictSymbols = _volatilitySymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<VolatilityImport> listVolatility = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (VolatilityImport volatilityImport in listVolatility)
                                    {
                                        volatilityImport.Symbol = pranaSymbol;
                                        volatilityImport.CUSIP = cUSIPSymbol;
                                        volatilityImport.ISIN = isinSymbol;
                                        volatilityImport.SEDOL = sedolSymbol;
                                        volatilityImport.Bloomberg = bloombergSymbol;
                                        volatilityImport.RIC = reutersSymbol;
                                        volatilityImport.AUECID = secMasterObj.AUECID;
                                        volatilityImport.OSIOptionSymbol = osiOptionSymbol;
                                        volatilityImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        volatilityImport.OpraOptionSymbol = opraOptionSymbol;
                                    }
                                }
                            }
                            break;

                        case "VWAPImport":
                            if (_vWAPSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<VWAPImport>> dictSymbols = _vWAPSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<VWAPImport> listVWAP = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (VWAPImport vWAPImport in listVWAP)
                                    {
                                        vWAPImport.Symbol = pranaSymbol;
                                        vWAPImport.CUSIP = cUSIPSymbol;
                                        vWAPImport.ISIN = isinSymbol;
                                        vWAPImport.SEDOL = sedolSymbol;
                                        vWAPImport.Bloomberg = bloombergSymbol;
                                        vWAPImport.RIC = reutersSymbol;
                                        vWAPImport.AUECID = secMasterObj.AUECID;
                                        vWAPImport.OSIOptionSymbol = osiOptionSymbol;
                                        vWAPImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        vWAPImport.OpraOptionSymbol = opraOptionSymbol;
                                    }
                                }
                            }
                            break;

                        case "CollateralImport":
                            if (_collateralSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<CollateralImport>> dictSymbols = _collateralSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<CollateralImport> listCollateral = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (CollateralImport collateralImport in listCollateral)
                                    {
                                        collateralImport.Symbol = pranaSymbol;
                                        collateralImport.CUSIP = cUSIPSymbol;
                                        collateralImport.ISIN = isinSymbol;
                                        collateralImport.SEDOL = sedolSymbol;
                                        collateralImport.Bloomberg = bloombergSymbol;
                                        collateralImport.RIC = reutersSymbol;
                                        collateralImport.AUECID = secMasterObj.AUECID;
                                        collateralImport.OSIOptionSymbol = osiOptionSymbol;
                                        collateralImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        collateralImport.OpraOptionSymbol = opraOptionSymbol;
                                    }
                                }
                            }
                            break;

                        case "DividendYieldImport":
                            if (_dividendYieldSymbologyWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<DividendYieldImport>> dictSymbols = _dividendYieldSymbologyWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<DividendYieldImport> listDividendYield = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (DividendYieldImport dividendYieldImport in listDividendYield)
                                    {
                                        dividendYieldImport.Symbol = pranaSymbol;
                                        dividendYieldImport.CUSIP = cUSIPSymbol;
                                        dividendYieldImport.ISIN = isinSymbol;
                                        dividendYieldImport.SEDOL = sedolSymbol;
                                        dividendYieldImport.Bloomberg = bloombergSymbol;
                                        dividendYieldImport.RIC = reutersSymbol;
                                        dividendYieldImport.AUECID = secMasterObj.AUECID;
                                        dividendYieldImport.OSIOptionSymbol = osiOptionSymbol;
                                        dividendYieldImport.IDCOOptionSymbol = idcoOptionSymbol;
                                        dividendYieldImport.OpraOptionSymbol = opraOptionSymbol;
                                    }
                                }
                            }
                            break;
                        case "StageImport":
                            if (_stageSymbolWiseDict.ContainsKey(requestedSymbologyID))
                            {
                                Dictionary<string, List<OrderSingle>> dictSymbols = _stageSymbolWiseDict[requestedSymbologyID];
                                if (dictSymbols.ContainsKey(secMasterObj.RequestedSymbol))
                                {
                                    List<OrderSingle> listOrderSingle = dictSymbols[secMasterObj.RequestedSymbol];
                                    foreach (OrderSingle stageOrder in listOrderSingle)
                                    {
                                        stageOrder.AssetID = secMasterObj.AssetID;
                                        stageOrder.UnderlyingID = secMasterObj.UnderLyingID;
                                        stageOrder.ExchangeID = secMasterObj.ExchangeID;
                                        stageOrder.CurrencyID = secMasterObj.CurrencyID;
                                        stageOrder.AUECID = secMasterObj.AUECID;
                                        stageOrder.Symbol = secMasterObj.TickerSymbol;
                                        stageOrder.BloombergSymbol = secMasterObj.BloombergSymbol;
                                        stageOrder.ISINSymbol = secMasterObj.ISINSymbol;
                                        stageOrder.CusipSymbol = secMasterObj.CusipSymbol;
                                        stageOrder.SEDOLSymbol = secMasterObj.SedolSymbol;
                                        stageOrder.IDCOSymbol = secMasterObj.IDCOOptionSymbol;
                                        stageOrder.SettlementCurrencyID = secMasterObj.CurrencyID;

                                        switch (secMasterObj.AssetCategory)
                                        {
                                            case AssetCategory.EquityOption:
                                            case AssetCategory.Option:
                                            case AssetCategory.FutureOption:
                                            case AssetCategory.Future:
                                                if (stageOrder.OrderSideTagValue == "1")
                                                {
                                                    stageOrder.OrderSideTagValue = "A";
                                                    stageOrder.OrderSide = "Buy to Open";
                                                }
                                                if (stageOrder.OrderSideTagValue == "2")
                                                {
                                                    stageOrder.OrderSideTagValue = "D";
                                                    stageOrder.OrderSide = "Sell to Close";
                                                }
                                                if (stageOrder.OrderSideTagValue == "5")
                                                {
                                                    stageOrder.OrderSideTagValue = "C";
                                                    stageOrder.OrderSide = "Sell to Open";
                                                }
                                                /// Buy to close remains same.
                                                break;
                                        }
                                        stageOrder.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(stageOrder.OrderSideTagValue);
                                        stageOrder.AssetName = CachedDataManager.GetInstance.GetAssetText(stageOrder.AssetID);
                                        stageOrder.ValidationStatus = ApplicationConstants.ValidationStatus.Validated.ToString();
                                        AlgoPropertiesHelper.SetAlgoParameters(stageOrder);
                                        _importHelper.SetExpireTime(stageOrder);

                                    }
                                }
                            }

                            break;

                        default:
                            break;
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
                throw;
            }
        }

        private static void UpdateDividendImportObj(SecMasterBaseObj secMasterObj, DividendImport dividendImport)
        {
            try
            {
                if (dividendImport.CurrencyID == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                {
                    dividendImport.FXRate = 1;
                    dividendImport.FXConversionMethodOperator = Operator.M.ToString();
                }
                else if (dividendImport.FXRate == 0)
                {
                    switch (CachedDataManager.GetInstance.GetCurrencyText(dividendImport.CurrencyID))
                    {
                        case "GBP":
                        case "NZD":
                        case "AUD":
                        case "EUR":
                            dividendImport.FXConversionMethodOperator = Operator.M.ToString();
                            break;
                        default:
                            dividendImport.FXConversionMethodOperator = Operator.D.ToString();
                            break;

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
                throw;
            }
        }

        private void UpdateMarkPriceObj(MarkPriceImport markPriceImport, SecMasterBaseObj secMasterObj)
        {
            if (!string.IsNullOrEmpty(markPriceImport.Date))
            {
                //CHMW-3132	Account wise fx rate handling for expiration settlement           
                int accountBaseCurrencyID;
                if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(markPriceImport.AccountID))
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[markPriceImport.AccountID];
                }
                else
                {
                    accountBaseCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                }
                if (accountBaseCurrencyID != secMasterObj.CurrencyID)
                {
                    ForexConverter.GetInstance(_companyUser.CompanyID, Convert.ToDateTime(markPriceImport.Date)).GetForexData(Convert.ToDateTime(markPriceImport.Date));
                    ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(_companyUser.CompanyID).GetConversionRateFromCurrenciesForGivenDate(accountBaseCurrencyID, secMasterObj.CurrencyID, Convert.ToDateTime(markPriceImport.Date), markPriceImport.AccountID);
                    if (conversionRate != null)
                    {
                        if (conversionRate.ConversionMethod == Operator.D)
                        {
                            if (conversionRate.RateValue != 0)
                                markPriceImport.MarkPrice = markPriceImport.MarkPrice / conversionRate.RateValue;
                        }
                        else
                        {
                            markPriceImport.MarkPrice = markPriceImport.MarkPrice * conversionRate.RateValue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Merge position master with security details
        /// </summary>
        /// <param name="positionMaster"></param>
        /// <param name="secMasterObj"></param>
        /// <returns></returns>
        private PositionMaster UpdatePositionMasterObj(PositionMaster positionMaster, SecMasterBaseObj secMasterObj)
        {
            try
            {
                if (positionMaster.AUECID == int.MinValue || positionMaster.AUECID == 0)
                {
                    positionMaster.AUECID = secMasterObj.AUECID;
                }

                if (secMasterObj.AssetID != (int)AssetCategory.FX && secMasterObj.AssetID != (int)AssetCategory.FXForward)
                {
                    positionMaster.IsSecApproved = secMasterObj.IsSecApproved;
                }
                else
                {
                    positionMaster.IsSecApproved = FXandFXFWDSymbolGenerator.IsValidFxAndFwdSymbol(secMasterObj);
                }

                positionMaster.AssetID = secMasterObj.AssetID;
                positionMaster.AssetType = (AssetCategory)secMasterObj.AssetID;

                //updated RoundLot field value from secMasterObject, PRANA-12674
                positionMaster.RoundLot = secMasterObj.RoundLot;

                if (positionMaster.UnderlyingID == int.MinValue || positionMaster.UnderlyingID == 0)
                {
                    positionMaster.UnderlyingID = secMasterObj.UnderLyingID;
                }
                if (positionMaster.ExchangeID == int.MinValue || positionMaster.ExchangeID == 0)
                {
                    positionMaster.ExchangeID = secMasterObj.ExchangeID;
                }

                // Updateing fxrate, FXConversionMethodOperator and currencyID
                if (positionMaster.AssetID == (int)AssetCategory.FXForward && secMasterObj is SecMasterFXForwardObj)
                {
                    SecMasterFXForwardObj tempSecObj = secMasterObj as SecMasterFXForwardObj;
                    if (positionMaster.CurrencyID == tempSecObj.LeadCurrencyID || positionMaster.CurrencyID != tempSecObj.VsCurrencyID)
                        positionMaster.CurrencyID = tempSecObj.LeadCurrencyID;
                    else
                        positionMaster.CurrencyID = tempSecObj.VsCurrencyID;

                    positionMaster.FXConversionMethodOperator = Operator.M;
                    if (tempSecObj.LeadCurrencyID != CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                        positionMaster.FXRate = positionMaster.CostBasis;
                    else
                        positionMaster.FXRate = positionMaster.CostBasis != 0 ? Math.Round(1 / positionMaster.CostBasis, 4) : 0;
                }
                else if (positionMaster.AssetID == (int)AssetCategory.FX && secMasterObj is SecMasterFxObj)
                {
                    SecMasterFxObj tempSecObj = secMasterObj as SecMasterFxObj;
                    if (positionMaster.CurrencyID == tempSecObj.LeadCurrencyID || positionMaster.CurrencyID != tempSecObj.VsCurrencyID)
                        positionMaster.CurrencyID = tempSecObj.LeadCurrencyID;
                    else
                        positionMaster.CurrencyID = tempSecObj.VsCurrencyID;

                    positionMaster.FXConversionMethodOperator = Operator.M;
                    if (tempSecObj.LeadCurrencyID != CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                        positionMaster.FXRate = positionMaster.CostBasis;
                    else
                        positionMaster.FXRate = positionMaster.CostBasis != 0 ? Math.Round(1 / positionMaster.CostBasis, 4) : 0;
                }
                else if (positionMaster.CurrencyID == int.MinValue || positionMaster.CurrencyID == 0)
                {
                    positionMaster.CurrencyID = secMasterObj.CurrencyID;
                    //PRANA-9628	[Import] - Settlement currency field comes out none instead of Trade Currency while importing.
                    if (positionMaster.SettlementCurrencyID > 0 && CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(positionMaster.SettlementCurrencyID)
                        && positionMaster.SettlCurrencyName != CachedDataManager.GetInstance.GetAllCurrencies()[positionMaster.SettlementCurrencyID])
                    {
                        positionMaster.SettlCurrencyName = CachedDataManager.GetInstance.GetAllCurrencies()[positionMaster.SettlementCurrencyID];
                    }
                }

                if (positionMaster.AssetID != (int)AssetCategory.FX && positionMaster.AssetID != (int)AssetCategory.FXForward)
                {
                    if (positionMaster.CurrencyID == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                    {
                        positionMaster.FXRate = 1;
                        positionMaster.FXConversionMethodOperator = Operator.M;
                    }
                    else if (positionMaster.FXRate == 0)
                    {
                        switch (CachedDataManager.GetInstance.GetCurrencyText(positionMaster.CurrencyID))
                        {
                            case "GBP":
                            case "NZD":
                            case "AUD":
                            case "EUR":
                                positionMaster.FXConversionMethodOperator = Operator.M;
                                break;
                            default:
                                positionMaster.FXConversionMethodOperator = Operator.D;
                                break;

                        }
                    }
                }

                // Trade Date
                positionMaster.PositionStartDate = positionMaster.PositionStartDate;
                if (!string.IsNullOrEmpty(positionMaster.PositionStartDate))
                {
                    string[] splitDateFieldSlash = positionMaster.PositionStartDate.Split('/');
                    if (splitDateFieldSlash.Length == 1)
                    {
                        string[] splitDateFieldWithDash = positionMaster.PositionStartDate.Split('-');
                        if (splitDateFieldWithDash.Length == 1)
                        {
                            bool blnIsTrue;
                            double result;
                            blnIsTrue = double.TryParse(positionMaster.PositionStartDate, out result);
                            if (blnIsTrue)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.PositionStartDate));//.ParseExact(positionMaster.PositionStartDate, "yyyyMMdd", null);
                                positionMaster.PositionStartDate = dtn.ToShortDateString();

                                UpdatePositionMasterAUECandSettlementDate(positionMaster, secMasterObj);
                            }
                        }
                        else
                        {
                            UpdatePositionMasterAUECandSettlementDate(positionMaster, secMasterObj);
                        }
                    }
                    else
                    {
                        UpdatePositionMasterAUECandSettlementDate(positionMaster, secMasterObj);
                    }
                }

                // Process Date
                if (!string.IsNullOrEmpty(positionMaster.ProcessDate))
                {
                    string[] splitDateFieldSlash = positionMaster.ProcessDate.Split('/');
                    if (splitDateFieldSlash.Length == 1)
                    {
                        string[] splitDateFieldWithDash = positionMaster.ProcessDate.Split('-');
                        if (splitDateFieldWithDash.Length == 1)
                        {
                            bool blnIsTrue;
                            double result;
                            blnIsTrue = double.TryParse(positionMaster.ProcessDate, out result);
                            if (blnIsTrue)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.ProcessDate));
                                positionMaster.ProcessDate = dtn.ToShortDateString();
                            }
                        }
                    }
                }
                else
                {
                    positionMaster.ProcessDate = positionMaster.AUECLocalDate;
                }

                // Original Purchase Date
                if (!string.IsNullOrEmpty(positionMaster.OriginalPurchaseDate))
                {
                    string[] splitDateFieldSlash = positionMaster.OriginalPurchaseDate.Split('/');
                    if (splitDateFieldSlash.Length == 1)
                    {
                        string[] splitDateFieldWithDash = positionMaster.OriginalPurchaseDate.Split('-');
                        if (splitDateFieldWithDash.Length == 1)
                        {
                            bool blnIsTrue;
                            double result;
                            blnIsTrue = double.TryParse(positionMaster.OriginalPurchaseDate, out result);
                            if (blnIsTrue)
                            {
                                DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.OriginalPurchaseDate));
                                positionMaster.OriginalPurchaseDate = dtn.ToShortDateString();
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(positionMaster.ProcessDate))
                {
                    positionMaster.OriginalPurchaseDate = positionMaster.ProcessDate;
                }
                else
                {
                    positionMaster.OriginalPurchaseDate = positionMaster.AUECLocalDate;
                }

                //modified by : omshiv, jan 2014
                //this on after security approval status. becuase if security is approved then show data missing staus other wise it show Unapproved
                if (!String.IsNullOrEmpty(positionMaster.SideTagValue))
                {
                    positionMaster.Side = TagDatabaseManager.GetInstance.GetOrderSideText(positionMaster.SideTagValue);
                }
                else
                {
                    positionMaster.Side = string.Empty;
                }
                if (_userSelectedAccountValue != int.MinValue)
                {
                    positionMaster.AccountID = _userSelectedAccountValue;
                    positionMaster.AccountName = CachedDataManager.GetInstance.GetAccountText(_userSelectedAccountValue);
                }
                else if (!String.IsNullOrEmpty(positionMaster.AccountName))
                {
                    positionMaster.AccountID = CachedDataManager.GetInstance.GetAccountID(positionMaster.AccountName.Trim());
                }
                if (!String.IsNullOrEmpty(positionMaster.Strategy))
                {
                    positionMaster.StrategyID = CachedDataManager.GetInstance.GetStrategyID(positionMaster.Strategy.Trim());
                }
                if (positionMaster.CounterPartyID > 0)
                {
                    positionMaster.ExecutingBroker = CachedDataManager.GetInstance.GetCounterPartyText(positionMaster.CounterPartyID);
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
            return positionMaster;
        }

        private void UpdatePositionMasterAUECandSettlementDate(PositionMaster positionMaster, SecMasterBaseObj secMasterBaseObj)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(positionMaster.PositionStartDate);
                positionMaster.AUECLocalDate = dt.ToString(DATEFORMAT);
                if (positionMaster.AssetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.FXForward)
                {
                    positionMaster.PositionSettlementDate = ((SecMasterFXForwardObj)secMasterBaseObj).ExpirationDate.ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(positionMaster.PositionSettlementDate))
                    {
                        //this is special handling of date when we get date from excel file in number format
                        string[] splitDateFieldSlash = positionMaster.PositionSettlementDate.Split('/');
                        if (splitDateFieldSlash.Length == 1)
                        {
                            string[] splitDateFieldWithDash = positionMaster.PositionSettlementDate.Split('-');
                            if (splitDateFieldWithDash.Length == 1)
                            {
                                bool blnIsTrue;
                                double result;
                                blnIsTrue = double.TryParse(positionMaster.PositionSettlementDate, out result);
                                if (blnIsTrue)
                                {
                                    DateTime dtn = DateTime.FromOADate(Convert.ToDouble(positionMaster.PositionSettlementDate));
                                    positionMaster.PositionSettlementDate = dtn.ToString(DATEFORMAT);
                                }
                            }
                        }
                    }
                    else
                    {
                        int auecSettlementPeriod = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAUECSettlementPeriod(positionMaster.AUECID, positionMaster.SideTagValue);
                        DateTime positionSettlementDate = DateTimeConstants.MinValue;
                        if (auecSettlementPeriod == 0)
                        {
                            positionSettlementDate = Convert.ToDateTime(positionMaster.PositionStartDate);
                        }
                        else
                        {
                            positionSettlementDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(Convert.ToDateTime(positionMaster.PositionStartDate), auecSettlementPeriod, positionMaster.AUECID); ;
                        }
                        positionMaster.PositionSettlementDate = positionSettlementDate.ToShortDateString();
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

        string _allocationSchemeName = string.Empty;
        DateTime _allocationSchemeDate = DateTime.MinValue;
        private bool DisplayToUser(string tableType)
        {
            try
            {
                _displayForm.LaunchForm += new EventHandler(displayForm_LaunchForm);
                _displayForm.StartPosition = FormStartPosition.CenterParent;
                switch (tableType)
                {
                    case _importTypeNetPosition:
                    case _importTypeTransaction:
                        _displayForm.BindImportPositions(_positionMasterCollection, tableType, isDisplaySwapColumns);

                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _positionMasterCollection = _displayForm.ValidatedPositions;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case _importTypeCash:

                        _displayForm.BindImportCash(_cashCurrencyValueCollection, tableType);
                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _cashCurrencyValueCollection = _displayForm.ValidatedCashCurrencyValue;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case _importTypeSettlementDateCash:

                        _displayForm.BindImportSettlementDateCash(_settlementDateCashCurrencyValueCollection, tableType);
                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _settlementDateCashCurrencyValueCollection = _displayForm.ValidatedSettlementDateCashCurrencyValue;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case _importTypeMarkPrice:

                        _displayForm.BindImportMarkPrice(_markPriceValueCollection, tableType);
                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _markPriceValueCollection = _displayForm.ValidatedMarkPriceValue;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case _importTypeDailyBeta:
                        _displayForm.BindImportBeta(_betaValueCollection, tableType);
                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _betaValueCollection = _displayForm.ValidatedBetaValues;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case _importTypeActivity:

                        _displayForm.BindImportCashTransaction(_dividendValueCollection, tableType);
                        foreach (var dividend in _dividendValueCollection)
                        {
                            if (dividend.CurrencyID <= 0)
                            {
                                MessageBox.Show("Some Dividend Currency is not mapped with CurrencyMapping.xml", "Import Data", MessageBoxButtons.OK);
                                break;
                            }
                        }
                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _dividendValueCollection = _displayForm.ValidatedCashTransactionValue;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case _importTypeForexPrice:

                        _displayForm.BindImportForexPrice(_forexPriceValueCollection, tableType);
                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _forexPriceValueCollection = _displayForm.ValidatedForexPriceValue;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case _importTypeSecMasterUpdate:

                        _displayForm.BindSecMasterUpdateData(_secMasterUpdateDataobj, tableType, _dynamicUDACache);

                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _secMasterUpdateDataobj = _displayForm.ValidatedSMUpdateDataValues;

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case _importTypeOMI:
                        _displayForm.BindImportOMIData(_omiValueCollection, tableType);

                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _omiValueCollection = _displayForm.ValidatedOmiValue;

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case _importTypeDailyCreditLimit:
                        _displayForm.BindImportDailyCreditLimit(_dailyCreditLimitCollection, tableType);

                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _dailyCreditLimitCollection = _displayForm.ValidatedDailyCreditLimitValue;
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case _importTypeDailyVolatility:
                        _displayForm.BindImportVolatility(_volatilityValueCollection, tableType);
                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _volatilityValueCollection = _displayForm.ValidatedDailyVolatilityValue;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case _importTypeDailyVWAP:
                        _displayForm.BindImportVWAP(_vWAPValueCollection, tableType);
                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _vWAPValueCollection = _displayForm.ValidatedDailyVWAPValue;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case _importTypeCollateralPrice:
                        _displayForm.BindImportCollateral(_collateralValueCollection, tableType);
                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _collateralValueCollection = _displayForm.ValidatedDailyCollateralValue;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case _importTypeDailyDividendYield:
                        _displayForm.BindImportDividendYield(_dividendYieldValueCollection, tableType);
                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _dividendYieldValueCollection = _displayForm.ValudatedDailyDividendYieldValue;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    case _importTypeStagedOrder:

                        _displayForm.BindImportStageorder(_stageOrderCollection, tableType);
                        if (_displayForm.ShowDialog() == DialogResult.OK)
                        {
                            _stageOrderCollection = _displayForm.ValidatedStageOrder;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    default:
                        return false;
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
            return false;
        }

        void displayForm_LaunchForm(object sender, EventArgs e)
        {
            if (LaunchForm != null)
            {
                LaunchForm(this, e);
            }
        }
        #endregion Upload through Local File

        /// <summary>
        /// Gets the data table from uploaded data file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        //private DataTable GetDataTableFromUploadedDataFile(string fileName)
        //{
        //    DataTable datasourceData = null;
        //    try
        //    {
        //        string fileFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);

        //        string intermediateFileName = Path.Combine(@"\DataSource Files\", fileName);
        //        string startupPath = Application.StartupPath;
        //        string completeFileName = startupPath + intermediateFileName;

        //        switch (fileFormat.ToUpperInvariant())
        //        {
        //            case "CSV":
        //                datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Csv).GetDataTableFromUploadedDataFile(completeFileName);
        //                break;
        //            case "XLS":
        //                datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(completeFileName);
        //                break;
        //            case "TXT":
        //                datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Text).GetDataTableFromUploadedDataFile(completeFileName);
        //                break;
        //            default:
        //                break;
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
        //    return datasourceData;
        //}

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string path = string.Empty;
                StringBuilder message = new StringBuilder(2048);
                //get the latest values of the grid
                gridRunUpload.UpdateData();
                foreach (UltraGridRow gridrow in gridRunUpload.Rows)
                {
                    bool isRowSelected = Convert.ToBoolean(gridrow.Cells[CAPTION_IsSelected].Value);
                    //Manually getting the files for all uploadclients where auto time is not enabled
                    //!runUploadItem.EnableAutoTime
                    if (isRowSelected)
                    {
                        RunUpload runUploadItem = (RunUpload)gridrow.ListObject;
                        path = runUploadItem.FilePath;
                        if (System.IO.File.Exists(path))
                        {
                            System.Diagnostics.Process.Start(path);
                        }
                        else
                        {
                            message.Append("File for " + runUploadItem.DataSourceNameIDValue.ShortName.ToUpperInvariant() + " does not exist." + Environment.NewLine);
                        }
                    }
                }
                if (message.Length > 0)
                {
                    MessageBox.Show(message.ToString(), "Import Data", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                MessageBox.Show(ex.Message);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        ISecurityMasterServices _securityMaster = null;
        public event EventHandler LaunchForm;
        internal void SetLaunchForm(EventHandler LaunchFormMapping)
        {
            if (LaunchFormMapping != null)
            {
                LaunchForm = LaunchFormMapping;
            }
        }

        private void CtrlRunDownload_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(147, 145, 152);
                    this.statusStrip1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.BackColor = System.Drawing.Color.FromArgb(147, 145, 152);
                    this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel1.Font = new Font("Century Gothic", 9F);
                    this.toolStripStatusLabel2.BackColor = System.Drawing.Color.FromArgb(147, 145, 152);
                    this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.WhiteSmoke;
                    this.toolStripStatusLabel2.Font = new Font("Century Gothic", 9F);
                }
                PositionMaster.AccountsList = CachedDataManager.GetInstance.GetUserAccounts();
                PositionMaster.TotalAccounts = CachedDataManager.GetInstance.GetAllAccountsCount();
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
                btnUpload.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnUpload.ForeColor = System.Drawing.Color.White;
                btnUpload.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnUpload.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnUpload.UseAppStyling = false;
                btnUpload.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnView.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnView.ForeColor = System.Drawing.Color.White;
                btnView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnView.UseAppStyling = false;
                btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private ValueList GetAccountsValueList()
        {
            try
            {
                Dictionary<int, string> dictAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                ValueList accountsValList = new ValueList();
                accountsValList.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int accountID in dictAccounts.Keys)
                {
                    accountsValList.ValueListItems.Add(accountID, dictAccounts[accountID]);
                }
                return accountsValList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        private void gridRunUpload_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void gridRunUpload_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.gridRunUpload);
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


        Dictionary<int, Dictionary<string, List<OrderSingle>>> _stageSymbolWiseDict = new Dictionary<int, Dictionary<string, List<OrderSingle>>>();
        /// <summary>
        /// Update Stage Order Collection
        /// </summary>
        /// <param name="ds"></param>
        void UpdateStageOrderCollection(DataSet ds)
        {
            _importHelper.UpdateStageOrderCollection(ds, _userSelectedAccountValue, _isUserSelectedDate, _userSelectedDate);
        }

        /// <summary>
        /// used to Export Data for automation
        /// </summary>
        /// <param name="exportFilePath"></param>
        /// <param name="gridName"></param>
        public void ExportGridData(string exportFilePath, string gridName)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(exportFilePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // Perform the export
                if (gridName == "gridRunUpload")
                {
                    // Create a new instance of the exporter
                    UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                    exporter.Export(gridRunUpload, exportFilePath);
                }
                else if (gridName == "grdImportData" && _displayForm != null)
                {
                    this._displayForm.ExportGridData(exportFilePath);
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
    }
    public delegate void SecMasterObjHandler(object sender, EventArgs<SecMasterBaseObj> e);
}
