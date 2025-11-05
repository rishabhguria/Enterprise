using Prana.AlgoStrategyControls;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PM.Client.UI;
using Prana.PM.DAL;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Prana.Blotter.Forms
{

    public partial class StageImport : Form
    {
        #region Private Fields
        private Dictionary<int, Dictionary<string, List<OrderSingle>>> _stageSymbolWiseDict = new Dictionary<int, Dictionary<string, List<OrderSingle>>>();
        private List<OrderSingle> _stageOrderCollection = new List<OrderSingle>();
        private ImportHelper _importHelper = new ImportHelper();
        private const string CAPTION_UPLOADING_DATA = "Uploading the data";
        private const string CAPTION_VALIDATING_SYMBOLS = "Validating symbols ({0} out of {1})";
        private const string CAPTION_GENERATING_OUTPUT = "Generating output";
        private EventHandler UpdateMessageEvent = null;
        private event EventHandler<EventArgs<int>> UpdateImportFeedbackMessageEvent = null;
        private EventHandler<EventArgs<int>> UpdateTotalCountOnImportFeedback = null;
        private int _symbolValidationCount;
        private int _totalSymbol;
        private DispatcherTimer _timerSnapShot;
        private bool _isValidationCompletedOrTimeExceeded = false;
        /// <summary>
        /// This is to save uploaded file details
        /// </summary>
        private string FileNameWithPath;
        /// <summary>
        /// This dictionary is to save the mapping of xslt files with their respective thrid parties.
        /// </summary>
        private Dictionary<string, string> XSLTMapping = new Dictionary<string, string>();
        #endregion


        public ISecurityMasterServices SecurityMaster { get; set; }
        public event EventHandler LaunchSecurityMasterForm;
        public event EventHandler SetBeginImportText;

        public StageImport()
        {
            InitializeComponent();
            UpdateMessageEvent += new EventHandler(UpdateMessage);
        }

        /// <summary>
        /// This method is to do required computations during form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StageImport_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER_NEW);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
                CompanyUser companyUser = Prana.CommonDataCache.CachedDataManager.GetInstance.LoggedInUser;
                int pMCompanyID = RunUploadManager.GetPMCompanyID(companyUser.CompanyID);
                var _runUploadList = RunUploadManager.GetRunUploadDataByCompanyID(pMCompanyID);
                List<string> thirdParties = new List<string>();

                foreach (var _runUpload in _runUploadList)
                {
                    if (_runUpload.ImportTypeAcronym.Equals(ImportType.StagedOrder))
                    {
                        string shortName = _runUpload.DataSourceNameIDValue.ShortName;

                        // Check if the key already exists in XSLTMapping
                        if (!XSLTMapping.ContainsKey(shortName))
                        {
                            thirdParties.Add(shortName);
                            XSLTMapping.Add(shortName, _runUpload.DataSourceXSLT);
                        }
                    }
                }
                thirdParties.Sort();
                cmbThirdParty.DataSource = thirdParties;
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
        /// This method is for performing browse button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browse_Click(object sender, EventArgs e)
        {
            try
            {
                FileNameWithPath = OpenFileDialogHelper.GetFileNameUsingOpenFileDialog(true);
                filePathTextBox.Text = FileNameWithPath;
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
        /// This method is for performing upload button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Upload_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbThirdParty.Items.IndexOf(cmbThirdParty.Text) != -1 && cmbThirdParty.SelectedItem != null && XSLTMapping.ContainsKey(cmbThirdParty.SelectedItem.ToString()))
                {
                    FileNameWithPath = filePathTextBox.Text;
                    if (File.Exists(FileNameWithPath))
                    {
                        string fileExtension = Path.GetExtension(FileNameWithPath).ToLower();
                        if (fileExtension == ".csv" || fileExtension == ".xls" || fileExtension == ".xlsx")
                        {
                            DataTable dataSource = null;
                            if (FileNameWithPath != null)
                                dataSource = FileReaderFactory.GetDataTableFromDifferentFileFormatsNew(FileNameWithPath);
                            if (dataSource == null)
                            {
                                MessageBox.Show("File already in use", "Stage Import", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                                return;
                            }
                            EnableDisableProgressCircle(true);
                            StartValidationAndDisplayResult(dataSource, XSLTMapping[cmbThirdParty.SelectedItem.ToString()]);
                        }
                        else
                        {
                            MessageBox.Show("Uploaded file schema is not as per requirement", "File Format Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            FileNameWithPath = null;
                            filePathTextBox.Text = string.Empty;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please browse a valid file path and try again", "File Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a valid third party", "Invalid Third Party", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        /// This method is for enabling or disabling UI elements
        /// </summary>
        /// <param name="enabled"></param>
        private void EnableDisableProgressCircle(bool enabled)
        {
            try
            {
                groupBox1.Enabled = !enabled;
                panel.Visible = enabled;
                panel.Enabled = enabled;
                progressCircle.Enabled = enabled;
                label.Enabled = enabled;
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
        /// This method is for updating the messages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        delegate void UpdateMessageCallback(string text);
        private void UpdateMessage(object sender, EventArgs e)
        {
            try
            {
                if (!_isValidationCompletedOrTimeExceeded)
                {
                    _symbolValidationCount++;
                    if (_symbolValidationCount >= _totalSymbol)
                    {
                        _isValidationCompletedOrTimeExceeded = true;
                        SetMessage(CAPTION_GENERATING_OUTPUT);

                        _importHelper.ValidateAndUpdate();
                        HideOrShowLoaderPanel(false);
                        DisplayImportForm();
                    }
                    else
                    {
                        SetMessage(string.Format(CAPTION_VALIDATING_SYMBOLS, _symbolValidationCount, _totalSymbol));
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
        /// This method is for setting the display messages
        /// </summary>
        /// <param name="text"></param>
        private void SetMessage(string text)
        {
            try
            {
                if (label.InvokeRequired)
                {
                    UpdateMessageCallback d = new UpdateMessageCallback(SetMessage);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    label.Text = text;
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
        /// This method is for hide and show the loader panel
        /// </summary>
        /// <param name="show"></param>
        delegate void HideOrShowLoaderPanelDelegate(bool show);
        private void HideOrShowLoaderPanel(bool show)
        {
            try
            {
                if (label.InvokeRequired)
                {
                    HideOrShowLoaderPanelDelegate d = new HideOrShowLoaderPanelDelegate(HideOrShowLoaderPanel);
                    this.Invoke(d, new object[] { show });
                }
                else
                {
                    panel.Visible = show;
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
        /// Sets the security master service and subscribes to the data response event
        /// </summary>
        /// <param name="securityMaster"></param>
        public void SetSecurityMasterService(ISecurityMasterServices securityMaster)
        {
            try
            {
                this.SecurityMaster = securityMaster;
                SecurityMaster.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(SecMasterClientSecMstrDataResponse);
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
        /// This method is to close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// This method is for starting validtion process
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="xsltName"></param>
        private void StartValidationAndDisplayResult(DataTable dataSource, string xsltName)
        {
            try
            {
                dataSource = _importHelper.ArrangeTable(dataSource);
                string mappedfilePath = _importHelper.GenerateXML(dataSource, xsltName, "Staged Order");
                if (!mappedfilePath.Equals(""))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(mappedfilePath);

                    _importHelper.ReArrangeDataSet(ds);

                    _stageOrderCollection.Clear();
                    _importHelper.StageOrderCollection = _stageOrderCollection;
                    _importHelper.StageSymbolWiseDict = _stageSymbolWiseDict;
                    _importHelper.SendUpdateAfterSymbolValidation += UpdateMessageEvent;

                    _importHelper.UpdateStageOrderCollection(ds, int.MinValue, string.Empty, string.Empty);

                    if (_stageOrderCollection.Count > 0)
                    {
                        foreach (KeyValuePair<int, Dictionary<string, List<OrderSingle>>> pair in _stageSymbolWiseDict)
                        {
                            _totalSymbol += pair.Value.Count;
                        }
                        _isValidationCompletedOrTimeExceeded = false;
                        bool _isTimerRequired = !(_importHelper.GetSMDataForStageImport(SecurityMaster, GetHashCode()));
                        if (_isTimerRequired)
                        {
                            _timerSnapShot = new DispatcherTimer();
                            _timerSnapShot.Tick += new EventHandler(_timerSnapShot_Tick);
                            _timerSnapShot.Interval = new TimeSpan(TimeSpan.TicksPerSecond * 5);
                            _timerSnapShot.Start();
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
                EnableDisableProgressCircle(false);
            }
        }


        /// <summary>
        /// This method is to perform computation on timmer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timerSnapShot_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!_isValidationCompletedOrTimeExceeded)
                {
                    _isValidationCompletedOrTimeExceeded = true;
                    _symbolValidationCount = _totalSymbol;
                    SetMessage(string.Format(CAPTION_VALIDATING_SYMBOLS, _symbolValidationCount, _totalSymbol));
                    SetMessage(CAPTION_GENERATING_OUTPUT);
                    _importHelper.ValidateAndUpdate();
                    HideOrShowLoaderPanel(false);
                    DisplayImportForm();
                }
                _timerSnapShot.Stop();
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
        /// This method is for opening import UI
        /// </summary>
        private void DisplayImportForm()
        {
            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(DisplayImportForm));
                    return;
                }

                ImportPositionsDisplayForm displayForm = null;

                try
                {
                    displayForm = ImportPositionsDisplayForm.GetInstance();
                    displayForm.IsComingFromBlotter = true;
                    displayForm.SendTradesToBlotterEvent += SendTradesToBlotter;
                    displayForm.UpdateAfterCloseEvent += EnableOrCloseUI;
                    displayForm.LaunchForm += displayForm_LaunchSecurityMasterForm;
                    UpdateImportFeedbackMessageEvent += displayForm.UpdateFeedbackMessageEvent;
                    UpdateTotalCountOnImportFeedback += displayForm.UpdateTotalCountAfterGroupingEvent;
                    displayForm.BindImportStageorder(_stageOrderCollection, "Staged Order");
                    displayForm.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    if (rethrow) throw;
                }
                finally
                {
                    if (displayForm != null)
                    {
                        displayForm.SendTradesToBlotterEvent -= SendTradesToBlotter;
                        displayForm.UpdateAfterCloseEvent -= EnableOrCloseUI;
                        displayForm.LaunchForm -= displayForm_LaunchSecurityMasterForm;

                        UpdateImportFeedbackMessageEvent -= displayForm.UpdateFeedbackMessageEvent;
                        UpdateTotalCountOnImportFeedback -= displayForm.UpdateTotalCountAfterGroupingEvent;
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
        /// This method handles the FormClosing of StageImport form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StageImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_timerSnapShot != null && _timerSnapShot.IsEnabled)
                {
                    _timerSnapShot.Tick -= _timerSnapShot_Tick;
                    _timerSnapShot.Stop();
                }
                SecurityMaster.SecMstrDataResponse -= SecMasterClientSecMstrDataResponse;
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
        /// This method is to send the validated trades to blotter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendTradesToBlotter(object sender, EventArgs<List<OrderSingle>> e)
        {
            try
            {
                var validatedTrades = e.Value;
                _importHelper.StageOrderCollection = validatedTrades;
                string fileName = FileNameWithPath.Substring(FileNameWithPath.LastIndexOf(@"\") + 1);
                string exactFile = "*fp#_" + FileNameWithPath.Substring(FileNameWithPath.LastIndexOf(@"\") + 1);
                int importFileId = Import.ImportDataManager.SaveImportedFileDetails(fileName, FileNameWithPath, ImportType.StagedOrder, File.GetLastWriteTime(FileNameWithPath));
                DataSet dataSet = null;
                _importHelper.GroupStagedOrders(exactFile, ref dataSet);
                validatedTrades = _importHelper.StageOrderCollection;
                if (UpdateTotalCountOnImportFeedback != null)
                    UpdateTotalCountOnImportFeedback(null, new EventArgs<int>(validatedTrades.Count));
                int tradeNumber = 0;
                if (SetBeginImportText != null)
                {
                    SetBeginImportText(this, EventArgs.Empty);
                }
                foreach (var stageOrder in validatedTrades)
                {
                    stageOrder.CumQty = 0.0;
                    stageOrder.AvgPrice = 0;
                    if (stageOrder.Price == double.Epsilon)
                        stageOrder.Price = 0;
                    stageOrder.MsgType = FIXConstants.MSGOrder;
                    stageOrder.ImportFileID = importFileId;
                    stageOrder.ImportFileName = fileName;
                    if (!TradeManager.TradeManager.GetInstance().SendBlotterTrades(stageOrder, 0))
                    {
                        Logger.LoggerWrite("StageOrder not saved. Symbol :" + stageOrder.Symbol + " AvgPrice " + stageOrder.AvgPrice + " Quantity " + stageOrder.Quantity);
                        SendUpdateToImport(-1);
                    } 
                    else
                    {
                        tradeNumber++;
                        SendUpdateToImport(tradeNumber);
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
        /// Sends the validated trades to the blotter.
        /// </summary>
        /// <param name="isTradeSuccessful"></param>
        private void SendUpdateToImport(int isTradeSuccessful)
        {
            try
            {
                if (UpdateImportFeedbackMessageEvent != null)
                {
                    UpdateImportFeedbackMessageEvent.BeginInvoke(this, new EventArgs<int>(isTradeSuccessful), null, null);                    
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
        /// This method is for enable or closing the UI
        /// </summary>
        /// <param name="isContinueClicked"></param>
        delegate void EnableOrCloseUIDelegate(bool isContinueClicked);
        private void EnableOrCloseUI(object sender, EventArgs<bool> e)
        {
            try
            {
                bool isContinueClicked = e.Value;
                if (this.InvokeRequired)
                {
                    EnableOrCloseUIDelegate d = new EnableOrCloseUIDelegate(EnableOrCloseUI);
                    this.Invoke(d, new object[] { isContinueClicked });
                }
                else
                {
                    EnableOrCloseUI(isContinueClicked);
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
        private void EnableOrCloseUI(bool isContinueClicked)
        {
            try
            {
                if (isContinueClicked)
                {
                    this.Close();
                }
                else
                {
                    groupBox1.BackColor = Color.FromArgb(217, 217, 217);
                    groupBox1.Enabled = true;
                    if (_timerSnapShot != null && _timerSnapShot.IsEnabled)
                    {
                        _timerSnapShot.Tick -= _timerSnapShot_Tick;
                        _timerSnapShot.Stop();
                    }
                    _symbolValidationCount = 0;
                    _totalSymbol = 0;
                    _stageOrderCollection.Clear();
                    _stageSymbolWiseDict.Clear();
                    _importHelper.SendUpdateAfterSymbolValidation -= UpdateMessageEvent;
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
        /// Data response of symbol validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecMasterClientSecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                if (secMasterObj.AssetID == (int)AssetCategory.FX || secMasterObj.AssetID == (int)AssetCategory.FXForward)
                {
                    if (FXandFXFWDSymbolGenerator.IsValidFxAndFwdSymbol(secMasterObj))
                        FillSecurityMasterDataFromObj(secMasterObj);
                }
                else
                {
                    FillSecurityMasterDataFromObj(secMasterObj);
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
        /// To fill Security master object after validation
        /// </summary>
        /// <param name="secMasterObj"></param>
        private void FillSecurityMasterDataFromObj(SecMasterBaseObj secMasterObj)
        {
            try
            {
                string pranaSymbol = secMasterObj.SymbologyMapping[(int)ApplicationConstants.PranaSymbology].ToString();

                int requestedSymbologyID = secMasterObj.RequestedSymbology;

                if (!string.IsNullOrEmpty(pranaSymbol))
                {
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

                            if (UpdateMessageEvent != null)
                            {
                                UpdateMessageEvent(this, null);
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

        delegate void LaunchSecurityMasterFormDelegate(object sender, EventArgs e);
        /// <summary>
        /// This method is for opening Security Master UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayForm_LaunchSecurityMasterForm(object sender, EventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    LaunchSecurityMasterFormDelegate d = new LaunchSecurityMasterFormDelegate(displayForm_LaunchSecurityMasterForm);
                    this.Invoke(d, new object[] { sender, e });
                }
                else
                {
                    if (LaunchSecurityMasterForm != null)
                    {
                        LaunchSecurityMasterForm(this, e);
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

        /// <summary>
        /// This method is for wiring Security Master UI Launch event
        /// </summary>
        /// <param name="launchSecurityMasterForm"></param>
        public void SetLaunchSecurityMasterForm(EventHandler launchSecurityMasterForm)
        {
            if (launchSecurityMasterForm != null)
            {
                LaunchSecurityMasterForm = launchSecurityMasterForm;
            }
        }
    }
}



