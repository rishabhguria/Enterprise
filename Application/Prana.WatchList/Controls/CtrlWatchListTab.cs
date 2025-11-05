using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using Prana.WatchList.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Prana.WatchList.Controls
{
    public partial class CtrlWatchListTab : UserControl
    {

        /// <summary>
        /// The is BBG symbolgy
        /// </summary>
        private ApplicationConstants.SymbologyCodes Symbology = SymbologyHelper.DefaultSymbology;

        /// <summary>
        /// The tab name
        /// </summary>
        private string _tabName = string.Empty;

        public string TabName
        {
            get { return _tabName; }
            set { _tabName = value; }
        }

        /// <summary>
        /// The company user identifier
        /// </summary>
        private int _companyUserID = -1;

        private DataTable _gridData = new DataTable();

        /// <summary>
        /// The adding symbol value
        /// </summary>
        private string _addingSymbolValue = string.Empty;

        /// <summary>
        /// The total row in grid
        /// </summary>
        int _maxRowInGrid = Convert.ToInt32(ConfigurationManager.AppSettings[WatchListConstants.TOTALROWINGRID_WATCHLIST]);

        /// <summary>
        /// The security master
        /// </summary>
        private ISecurityMasterServices _securityMaster = null;

        /// <summary>
        /// The symbol live feed manager
        /// </summary>
        WatchListManager _symbolLiveFeedManager = null;

        /// <summary>
        /// The locker
        /// </summary>
        object _locker = new object();

        //This event handler is responsible for opening Ptt for the particular symbol
        public event EventHandler<EventArgs<string>> SendSymbolToPTT;

        //This event handler is responsible for open tt for the particular symbol
        public event EventHandler<EventArgs<string, string>> SendSymbolToTT;

        /// <summary>
        /// Occurs when [send symbol to MTT].
        /// </summary>
        public event EventHandler<EventArgs<OrderBindingList>> SendSymbolToMTT;

        /// <summary>
        /// Occures when Optio Chain Window is opened
        /// </summary>
        public event EventHandler<EventArgs<int, string>> OptionChainModuleOpened;

        private delegate void UIThreadMarshellerForUpdateRow(SymbolData symbolData);
        private delegate void UIThreadMarshellerForAddRow(string symbol, bool isInitial);
        private delegate void UIThreadMarshellerForUnlinkTab();

        private Color UPCOLOR = Color.FromArgb(39, 174, 96);
        private Color DOWNCOLOR = Color.FromArgb(192, 57, 43);
        private Color NOCHANGECOLORFORACTIVE = Color.White;
        private Color NOCHANGECOLOR = Color.Black;

        /// <summary>
        /// The property infos
        /// </summary>
        private PropertyInfo[] _propertyInfos = typeof(SymbolData).GetProperties();

        /// <summary>
        /// The pending import symbols
        /// </summary>
        internal Dictionary<string, ApplicationConstants.SymbologyCodes> _pendingImportSymbols = new Dictionary<string, ApplicationConstants.SymbologyCodes>();

        /// <summary>
        /// Gets or sets the security master.
        /// </summary>
        /// <value>
        /// The security master.
        /// </value>
        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _securityMaster = value;
                _securityMaster.SecMstrDataResponse += _securityMaster_SecMstrDataResponse;
                _securityMaster.SecMstrDataSymbolSearcResponse += _securityMaster_SecMstrDataSymbolSearcResponse;
            }
        }

        public CtrlWatchListTab(WatchListManager watchListManager, string tabName, int companyUserID)
        {
            _companyUserID = companyUserID;
            _tabName = tabName;
            InitializeComponent();
            _symbolLiveFeedManager = watchListManager;
            dataGrid.DataSource = GetDataTableFromSymbolDataProp();
            dataGrid.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(Grid_BeforeColumnChooserDisplayed);
            dataGrid.DoubleClickCell += dataGrid_DoubleClickCell;
            dataGrid.ClickCell += dataGrid_ClickCell;
            symbolValue.SymbolSearch += SymbolSearch;
            if (_symbolLiveFeedManager.IsCurrentTabLinked(_tabName))
                linkTabBtn.Text = "Unlink";
            SetThemeForUserControl();

            Load += CtrlWatchListTab_LoadedOrEntered;
            Enter += CtrlWatchListTab_LoadedOrEntered;
        }

        void CtrlWatchListTab_LoadedOrEntered(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                symbolValue.Focus();
            }));
        }

        /// <summary>
        /// Handles the ClickCell event of the dataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ClickCellEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void dataGrid_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {

                if (_symbolLiveFeedManager.IsCurrentTabLinked(_tabName))
                {
                    string symbol = e.Cell.Row.Cells[WatchListConstants.COL_Symbol].Value.ToString();
                    if (_symbolLiveFeedManager.LinkedSymbolSelected != null)
                        _symbolLiveFeedManager.LinkedSymbolSelected(null, new EventArgs<string>(symbol));
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
        /// Symbols the search.
        /// </summary>
        /// <param name="e">The <see cref="string"/> instance containing the event data.</param>
        internal void SymbolSearch(object sender, EventArgs<string> e)
        {
            if (_securityMaster != null)
            {
                try
                {
                    SecMasterSymbolSearchReq searchReq = new SecMasterSymbolSearchReq(e.Value, Symbology)
                    {
                        HashCode = GetHashCode()
                    };

                    if (_securityMaster != null && _securityMaster.IsConnected)
                    {
                        _securityMaster.searchSymbols(searchReq);
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

        /// <summary>
        /// Sends the validated symbol to sm.
        /// </summary>
        /// <param name="e">The <see cref="string"/> instance containing the event data.</param>
        internal void SendValidatedSymbolToSM(object sender, EventArgs<string> e)
        {
            try
            {
                if (!IsHandleCreated)
                {
                    CreateHandle();
                }
                if (UIValidation.GetInstance().validate(this))
                {
                    if (_securityMaster != null && _securityMaster.IsConnected)
                    {
                        if (!string.IsNullOrEmpty(e.Value))
                        {
                            SecMasterRequestObj reqObj = new SecMasterRequestObj();
                            String symbol = e.Value.Trim();
                            reqObj.AddData(symbol, Symbology);
                            reqObj.HashCode = GetHashCode();
                            _securityMaster.SendRequest(reqObj);
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

        /// <summary>
        /// Adds the primary key.
        /// </summary>
        /// <param name="dt">The dt.</param>
        private void AddPrimaryKey(DataTable dt)
        {
            try
            {
                if (!dt.Columns.Contains("RowID"))
                {
                    dt.Columns.Add("RowID");
                    int rowID = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        row["RowID"] = rowID;
                        rowID++;
                    }
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["RowID"] };
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
        /// XSLTs the transform.
        /// </summary>
        /// <param name="dTable">The d table.</param>
        /// <returns></returns>
        private DataTable XSLTTransform(DataTable dTable)
        {
            try
            {
                string tempPath = Application.StartupPath;

                string inputXML = tempPath + "\\InputXML.xml";
                string outputXML = tempPath + "\\OutPutXML.xml";

                string path = Application.StartupPath;
                string xsltName = "WatchlistImport.xslt";
                string xsltPath = path + "\\" + xsltName;

                dTable.WriteXml(inputXML);

                System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
                xslt.Load(xsltPath);
                xslt.Transform(inputXML, outputXML);

                DataSet ds = new DataSet();
                ds.ReadXml(outputXML);
                if (ds.Tables.Count > 0 && ds.Tables[0] != null)
                {
                    dTable = ds.Tables[0];
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
            return dTable;
        }

        /// <summary>
        /// Handles the Click event of the importSymbolBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void importSymbolBtn_Click(object sender, System.EventArgs e)
        {
            DataTable inputDataTable = null;
            DataTable outputDataTable = null;
            _pendingImportSymbols.Clear();
            try
            {
                lock (_locker)
                {
                    string strFileName = OpenFileDialogHelper.GetFileNameUsingOpenFileDialog(true);
                    if (File.Exists(strFileName))
                    {
                        // Reading dropped file data
                        inputDataTable = GetDataTableFromDifferentFileFormats(strFileName);

                        if (inputDataTable != null)
                        {
                            AddPrimaryKey(inputDataTable);
                            inputDataTable.TableName = "PositionMaster";

                            // XSLT transformation
                            outputDataTable = XSLTTransform(inputDataTable);

                            if (outputDataTable != null)
                            {

                                for (int counter = 0; counter < outputDataTable.Rows.Count; counter++)
                                {
                                    DataRow dTableRow = outputDataTable.Rows[counter];
                                    string symbol = Convert.ToString(dTableRow[WatchListConstants.COL_Symbol]);
                                    string symbology = Convert.ToString(dTableRow["Symbology"]);
                                    if (!string.IsNullOrEmpty(symbol))
                                    {
                                        symbol = symbol.ToUpper();
                                        ApplicationConstants.SymbologyCodes code = ApplicationConstants.SymbologyCodes.TickerSymbol;
                                        Enum.TryParse<ApplicationConstants.SymbologyCodes>(symbology, out code);
                                        _pendingImportSymbols[symbol] = code;
                                    }
                                }
                                if (_pendingImportSymbols.Count + _symbolLiveFeedManager.GetTabSymbolsCount(_tabName) > _maxRowInGrid)
                                {
                                    MessageBox.Show("Some symbols might not be imported as the number of symbols imported plus current symbols in grid exceeds the row limit."
                                        , "Alert", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                                }
                                if (_securityMaster != null && _securityMaster.IsConnected)
                                {
                                    SecMasterRequestObj reqObj = new SecMasterRequestObj();
                                    foreach (var kvp in _pendingImportSymbols)
                                    {
                                        reqObj.AddData(kvp.Key, kvp.Value);
                                    }
                                    reqObj.HashCode = GetHashCode();
                                    _securityMaster.SendRequest(reqObj);
                                }
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

        /// <summary>
        /// Gets the data table from different file formats.
        /// </summary>
        /// <param name="dropFilePath">The drop file path.</param>
        /// <returns></returns>
        private DataTable GetDataTableFromDifferentFileFormats(string dropFilePath)
        {
            DataTable dTable = null;
            try
            {
                string fileFormat = dropFilePath.Substring(dropFilePath.LastIndexOf(".") + 1);

                switch (fileFormat.ToUpperInvariant())
                {
                    case "CSV":
                        dTable = GeneralUtilities.GetDataTableFromUploadedDataFileBulkRead(dropFilePath);
                        break;
                    case "XLS":
                        dTable = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(dropFilePath);
                        break;
                }
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("File in use! Please close the file and retry.");
            }
            return dTable;
        }

        /// <summary>
        /// This method take a response from symbol lookup if the symbol is present in system and add the symbol in watch list.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs<SecMasterBaseObj></param>
        private void _securityMaster_SecMstrDataSymbolSearcResponse(object sender, EventArgs<SecMasterSymbolSearchRes> e)
        {
            try
            {
                symbolValue.updateDropDown(e.Value.StartWith, e.Value.Result);
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
        /// Handles the SecMstrDataResponse event of the _securityMaster control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{SecMasterBaseObj}"/> instance containing the event data.</param>
        private void _securityMaster_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    _symbolLiveFeedManager.secMasterDataDict.TryAdd(e.Value.TickerSymbol, e.Value);
                    if (_symbolLiveFeedManager.GetTabSymbolsCount(_tabName) < _maxRowInGrid)
                    {
                        ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.TickerSymbol;
                        if ((Symbology == ApplicationConstants.SymbologyCodes.BloombergSymbol && e.Value.BloombergSymbol.Equals(_addingSymbolValue, StringComparison.InvariantCultureIgnoreCase) ||
                            (Symbology == ApplicationConstants.SymbologyCodes.TickerSymbol && e.Value.TickerSymbol.Equals(_addingSymbolValue, StringComparison.InvariantCultureIgnoreCase)) ||
                            (Symbology == ApplicationConstants.SymbologyCodes.FactSetSymbol && e.Value.FactSetSymbol.Equals(_addingSymbolValue, StringComparison.InvariantCultureIgnoreCase)) ||
                        (Symbology == ApplicationConstants.SymbologyCodes.ActivSymbol && e.Value.ActivSymbol.Equals(_addingSymbolValue, StringComparison.InvariantCultureIgnoreCase))))
                        {
                            if (!_symbolLiveFeedManager.IsSymbolInCurrentTab(e.Value.TickerSymbol, _tabName))
                            {
                                _symbolLiveFeedManager.AddSymbolToTab(e.Value.TickerSymbol, _tabName, e.Value);
                                symbolValue.Text = string.Empty;
                                SetLabelMessage(WatchListConstants.MSG_SYMBOL_ADDED);
                            }
                            else
                            {
                                SetLabelMessage(WatchListConstants.MSG_SYMBOL_ALREADY_ADDED);
                            }
                        }
                        else if (_pendingImportSymbols.TryGetValue(e.Value.TickerSymbol, out symbology) && symbology.Equals(ApplicationConstants.SymbologyCodes.TickerSymbol))
                        {
                            if (!_symbolLiveFeedManager.IsSymbolInCurrentTab(e.Value.TickerSymbol, _tabName))
                            {
                                _symbolLiveFeedManager.AddSymbolToTab(e.Value.TickerSymbol, _tabName, e.Value);
                            }
                            _pendingImportSymbols.Remove(e.Value.TickerSymbol);
                        }
                        else if (_pendingImportSymbols.TryGetValue(e.Value.BloombergSymbol.ToUpper(), out symbology) && symbology.Equals(ApplicationConstants.SymbologyCodes.BloombergSymbol))
                        {
                            if (!_symbolLiveFeedManager.IsSymbolInCurrentTab(e.Value.TickerSymbol, _tabName))
                            {
                                _symbolLiveFeedManager.AddSymbolToTab(e.Value.TickerSymbol, _tabName, e.Value);
                            }
                            _pendingImportSymbols.Remove(e.Value.BloombergSymbol.ToUpper());
                        }
                        else if (_pendingImportSymbols.TryGetValue(e.Value.FactSetSymbol.ToUpper(), out symbology) && symbology.Equals(ApplicationConstants.SymbologyCodes.FactSetSymbol))
                        {
                            if (!_symbolLiveFeedManager.IsSymbolInCurrentTab(e.Value.TickerSymbol, _tabName))
                            {
                                _symbolLiveFeedManager.AddSymbolToTab(e.Value.TickerSymbol, _tabName, e.Value);
                            }
                            _pendingImportSymbols.Remove(e.Value.FactSetSymbol.ToUpper());
                        }
                        else if (_pendingImportSymbols.TryGetValue(e.Value.ActivSymbol.ToUpper(), out symbology) && symbology.Equals(ApplicationConstants.SymbologyCodes.ActivSymbol))
                        {
                            if (!_symbolLiveFeedManager.IsSymbolInCurrentTab(e.Value.TickerSymbol, _tabName))
                            {
                                _symbolLiveFeedManager.AddSymbolToTab(e.Value.TickerSymbol, _tabName, e.Value);
                            }
                            _pendingImportSymbols.Remove(e.Value.ActivSymbol.ToUpper());
                        }
                    }
                    _addingSymbolValue = string.Empty;
                    _symbolLiveFeedManager.secMasterDataDict.TryAdd(e.Value.TickerSymbol, e.Value);
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
        /// Handles the Click event of the addSymbolBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void addSymbolBtn_Click(object sender, System.EventArgs e)
        {
            try
            {
                string symbol = symbolValue.Text.Trim();
                _addingSymbolValue = symbol;
                if (!string.IsNullOrWhiteSpace(symbol))
                {
                    if (_symbolLiveFeedManager.IsSymbolInCurrentTab(symbol, _tabName))
                    {
                        SetLabelMessage(WatchListConstants.MSG_SYMBOL_ALREADY_ADDED);
                    }
                    else
                    {
                        if (_symbolLiveFeedManager.GetTabSymbolsCount(_tabName) >= _maxRowInGrid)
                        {
                            MessageBox.Show(WatchListConstants.MSG_ADD_SYMBOL_ALERT, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            SetLabelMessage("Cannot add symbol as row count has reached the limit of " + _maxRowInGrid + ".");
                            return;
                        }
                        SendValidatedSymbolToSM(null, new EventArgs<string>(symbol));
                        System.Threading.Tasks.Task.Delay(100);
                        SetLabelMessage("Waiting for symbol validation.");
                    }
                }
                else
                    SetLabelMessage(WatchListConstants.MSG_SYMBOL_EMPTY);
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
        /// Handles the Click event of the linkTabBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LinkTabBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (linkTabBtn.Text.Equals("Link tab"))
                {
                    _symbolLiveFeedManager.LinkCurrentTab(_tabName);
                    linkTabBtn.Text = "Unlink";
                }
                else
                {
                    _symbolLiveFeedManager.UnlinkCurrentTab();
                    linkTabBtn.Text = "Link tab";
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
        /// Sets the error message.
        /// </summary>
        /// <param name="errMessage">The error message.</param>
        public void SetLabelMessage(string errMessage)
        {
            try
            {
                lblErrorMessage.Text = "[" + DateTime.Now + "] " + errMessage;
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
        /// This Method is use to get the datatable with coloums as symboldata property 
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetDataTableFromSymbolDataProp()
        {
            try
            {
                foreach (PropertyInfo propertyInfo in _propertyInfos)
                {
                    _gridData.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);
                }
                _gridData.PrimaryKey = new DataColumn[] { _gridData.Columns[WatchListConstants.COL_Symbol] };
                _gridData.AcceptChanges();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _gridData;
        }

        /// <summary>
        /// Handles the DoubleClickCell event of the dataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoubleClickCellEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void dataGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            try
            {
                string symbol = GetSymbologyBasedSymbolFromRow(e.Cell.Row);
                string columnKey = e.Cell.Column.Key;
                if (SendSymbolToTT != null)
                    SendSymbolToTT(null, new EventArgs<string, string>(symbol, columnKey));
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
        /// Handles the InitializeLayout event of the dataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeLayoutEventArgs"/> instance containing the event data.</param>
        void dataGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                AddComboBoxColumn();
                SetupGridColumns();
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
        /// Adds the ComboBox column.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        public void AddComboBoxColumn()
        {
            try
            {
                UltraGridBand gridBand = dataGrid.DisplayLayout.Bands[0];
                gridBand.Columns.Add("Select", "Select");
                UltraGridColumn colSelect = gridBand.Columns["Select"];
                colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                colSelect.DataType = typeof(bool);
                colSelect.Width = 50;
                colSelect.MinWidth = 50;
                colSelect.Header.Caption = " ";
                colSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                colSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                colSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                colSelect.Header.VisiblePosition = 0;
                colSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colSelect.CellClickAction = CellClickAction.Edit;
                colSelect.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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
        /// This Method is used to set the search bar on field chooser.
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">BeforeColumnChooserDisplayedEventArgs</param>
        public void Grid_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.dataGrid);
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
        /// Sets the column name in camel case.
        /// </summary>
        /// <param name="gridBand">The grid band.</param>
        public void SetupGridColumns()
        {
            try
            {
                UltraGridBand gridBand = dataGrid.DisplayLayout.Bands[0];

                foreach (UltraGridColumn column in gridBand.Columns)
                {
                    switch (column.Header.Caption)
                    {
                        case " ":
                        case WatchListConstants.COL_Ask:
                        case WatchListConstants.COL_AskSize:
                        case WatchListConstants.COL_Bid:
                        case WatchListConstants.COL_BidSize:
                        case WatchListConstants.COL_Change:
                        case WatchListConstants.COL_LastPrice:
                        case WatchListConstants.COL_PctChange:
                        case WatchListConstants.COL_Previous:
                        case WatchListConstants.COL_Symbol:
                        case WatchListConstants.COL_TradeVolume:
                            column.Hidden = false;
                            break;
                        default:
                            column.Hidden = true;
                            break;
                    }
                }

                UltraGridColumn colAnnualDivident = gridBand.Columns[WatchListConstants.COL_AnnualDividend];
                colAnnualDivident.Header.Caption = WatchListConstants.CAP_AnnualDividend;

                UltraGridColumn colAskExchange = gridBand.Columns[WatchListConstants.COL_AskExchange];
                colAskExchange.Header.Caption = WatchListConstants.CAP_AskExchange;

                UltraGridColumn colAskSize = gridBand.Columns[WatchListConstants.COL_AskSize];
                colAskSize.Header.Caption = WatchListConstants.CAP_AskSize;
                colAskSize.CellAppearance.ForeColor = Color.Red;

                UltraGridColumn colAverageVolume20Day = gridBand.Columns[WatchListConstants.COL_AverageVolume20Day];
                colAverageVolume20Day.Header.Caption = WatchListConstants.CAP_AverageVolume20Day;

                UltraGridColumn colAvgVolume = gridBand.Columns[WatchListConstants.COL_AvgVolume];
                colAvgVolume.Header.Caption = WatchListConstants.CAP_AvgVolume;

                UltraGridColumn colBeta_5yrMonthly = gridBand.Columns[WatchListConstants.COL_Beta_5yrMonthly];
                colBeta_5yrMonthly.Header.Caption = WatchListConstants.CAP_Beta_5yrMonthly;

                UltraGridColumn colBidExchange = gridBand.Columns[WatchListConstants.COL_BidExchange];
                colBidExchange.Header.Caption = WatchListConstants.CAP_BidExchange;

                UltraGridColumn colBidSize = gridBand.Columns[WatchListConstants.COL_BidSize];
                colBidSize.Header.Caption = WatchListConstants.CAP_BidSize;
                colBidSize.CellAppearance.ForeColor = Color.Blue;


                UltraGridColumn colBloombergSymbol = gridBand.Columns[WatchListConstants.COL_BloombergSymbol];
                colBloombergSymbol.Header.Caption = WatchListConstants.CAP_BloombergSymbol;

                UltraGridColumn colFactSetSymbol = gridBand.Columns[WatchListConstants.COL_FactSetSymbol];
                colFactSetSymbol.Header.Caption = WatchListConstants.CAP_FactSetSymbol;

                UltraGridColumn colActivSymbol = gridBand.Columns[WatchListConstants.COL_ActivSymbol];
                colActivSymbol.Header.Caption = WatchListConstants.CAP_ActivSymbol;

                UltraGridColumn colCategoryCode = gridBand.Columns[WatchListConstants.COL_CategoryCode];
                colCategoryCode.Header.Caption = WatchListConstants.CAP_CategoryCode;

                UltraGridColumn colCFICode = gridBand.Columns[WatchListConstants.COL_CFICode];
                colCFICode.Header.Caption = WatchListConstants.CAP_CFICode;

                UltraGridColumn colChange = gridBand.Columns[WatchListConstants.COL_Change];
                colChange.Header.Caption = WatchListConstants.CAP_Change;

                UltraGridColumn colConversionMethod = gridBand.Columns[WatchListConstants.COL_ConversionMethod];
                colConversionMethod.Header.Caption = WatchListConstants.CAP_ConversionMethod;

                UltraGridColumn colCurencyCode = gridBand.Columns[WatchListConstants.COL_CurencyCode];
                colCurencyCode.Header.Caption = WatchListConstants.CAP_CurencyCode;

                UltraGridColumn colCusipNo = gridBand.Columns[WatchListConstants.COL_CusipNo];
                colCusipNo.Header.Caption = WatchListConstants.CAP_CusipNo;

                UltraGridColumn colDaysToExpiration = gridBand.Columns[WatchListConstants.COL_DaysToExpiration];
                colDaysToExpiration.Header.Caption = WatchListConstants.CAP_DaysToExpiration;

                UltraGridColumn colDeltaSource = gridBand.Columns[WatchListConstants.COL_DeltaSource];
                colDeltaSource.Header.Caption = WatchListConstants.CAP_DeltaSource;

                UltraGridColumn colDivDistributionDate = gridBand.Columns[WatchListConstants.COL_DivDistributionDate];
                colDivDistributionDate.Header.Caption = WatchListConstants.CAP_DivDistributionDate;
                colDivDistributionDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colDividendAmtRate = gridBand.Columns[WatchListConstants.COL_DividendAmtRate];
                colDividendAmtRate.Header.Caption = WatchListConstants.CAP_DividendAmtRate;

                UltraGridColumn colDividendInterval = gridBand.Columns[WatchListConstants.COL_DividendInterval];
                colDividendInterval.Header.Caption = WatchListConstants.CAP_DividendInterval;

                UltraGridColumn colDividendYield = gridBand.Columns[WatchListConstants.COL_DividendYield];
                colDividendYield.Header.Caption = WatchListConstants.CAP_DividendYield;

                UltraGridColumn colExchangeID = gridBand.Columns[WatchListConstants.COL_ExchangeID];
                colExchangeID.Header.Caption = WatchListConstants.CAP_ExchangeID;

                UltraGridColumn colExpirationDate = gridBand.Columns[WatchListConstants.COL_ExpirationDate];
                colExpirationDate.Header.Caption = WatchListConstants.CAP_ExpirationDate;
                colExpirationDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colFinalDividendYield = gridBand.Columns[WatchListConstants.COL_FinalDividendYield];
                colFinalDividendYield.Header.Caption = WatchListConstants.CAP_FinalDividendYield;

                UltraGridColumn colFinalImpliedVol = gridBand.Columns[WatchListConstants.COL_FinalImpliedVol];
                colFinalImpliedVol.Header.Caption = WatchListConstants.CAP_FinalImpliedVol;

                UltraGridColumn colFinalInterestRate = gridBand.Columns[WatchListConstants.COL_FinalInterestRate];
                colFinalInterestRate.Header.Caption = WatchListConstants.CAP_FinalInterestRate;

                UltraGridColumn colForwardPoints = gridBand.Columns[WatchListConstants.COL_ForwardPoints];
                colForwardPoints.Header.Caption = WatchListConstants.CAP_ForwardPoints;

                UltraGridColumn colFullCompanyName = gridBand.Columns[WatchListConstants.COL_FullCompanyName];
                colFullCompanyName.Header.Caption = WatchListConstants.CAP_FullCompanyName;

                UltraGridColumn colGapOpen = gridBand.Columns[WatchListConstants.COL_GapOpen];
                colGapOpen.Header.Caption = WatchListConstants.CAP_GapOpen;

                UltraGridColumn colIDCOOptionSymbol = gridBand.Columns[WatchListConstants.COL_IDCOOptionSymbol];
                colIDCOOptionSymbol.Header.Caption = WatchListConstants.CAP_IDCOOptionSymbol;

                UltraGridColumn colImpliedVol = gridBand.Columns[WatchListConstants.COL_ImpliedVol];
                colImpliedVol.Header.Caption = WatchListConstants.CAP_ImpliedVol;

                UltraGridColumn colInterestRate = gridBand.Columns[WatchListConstants.COL_InterestRate];
                colInterestRate.Header.Caption = WatchListConstants.CAP_InterestRate;

                UltraGridColumn colIsChangedToHigherCurrency = gridBand.Columns[WatchListConstants.COL_IsChangedToHigherCurrency];
                colIsChangedToHigherCurrency.Header.Caption = WatchListConstants.CAP_IsChangedToHigherCurrency;

                UltraGridColumn colLastPrice = gridBand.Columns[WatchListConstants.COL_LastPrice];
                colLastPrice.Header.Caption = WatchListConstants.CAP_LastPrice;

                UltraGridColumn colLastTick = gridBand.Columns[WatchListConstants.COL_LastTick];
                colLastTick.Header.Caption = WatchListConstants.CAP_LastTick;

                UltraGridColumn colListedExchange = gridBand.Columns[WatchListConstants.COL_ListedExchange];
                colListedExchange.Header.Caption = WatchListConstants.CAP_ListedExchange;

                UltraGridColumn colMarketCapitalization = gridBand.Columns[WatchListConstants.COL_MarketCapitalization];
                colMarketCapitalization.Header.Caption = WatchListConstants.CAP_MarketCapitalization;

                UltraGridColumn colMarkPrice = gridBand.Columns[WatchListConstants.COL_MarkPrice];
                colMarkPrice.Header.Caption = WatchListConstants.CAP_MarkPrice;

                UltraGridColumn colMarkPriceStr = gridBand.Columns[WatchListConstants.COL_MarkPriceStr];
                colMarkPriceStr.Header.Caption = WatchListConstants.CAP_MarkPriceStr;

                UltraGridColumn colOpenInterest = gridBand.Columns[WatchListConstants.COL_OpenInterest];
                colOpenInterest.Header.Caption = WatchListConstants.CAP_OpenInterest;

                UltraGridColumn colOpraSymbol = gridBand.Columns[WatchListConstants.COL_OpraSymbol];
                colOpraSymbol.Header.Caption = WatchListConstants.CAP_OpraSymbol;

                UltraGridColumn colOSIOptionSymbol = gridBand.Columns[WatchListConstants.COL_OSIOptionSymbol];
                colOSIOptionSymbol.Header.Caption = WatchListConstants.CAP_OSIOptionSymbol;

                UltraGridColumn colPctChange = gridBand.Columns[WatchListConstants.COL_PctChange];
                colPctChange.Header.Caption = WatchListConstants.CAP_PctChange;

                UltraGridColumn colPreferencedPrice = gridBand.Columns[WatchListConstants.COL_PreferencedPrice];
                colPreferencedPrice.Header.Caption = WatchListConstants.CAP_PreferencedPrice;

                UltraGridColumn colPricingSource = gridBand.Columns[WatchListConstants.COL_PricingSource];
                colPricingSource.Header.Caption = WatchListConstants.CAP_PricingSource;

                UltraGridColumn colPricingProvider = gridBand.Columns[WatchListConstants.COL_PricingProvider];
                colPricingProvider.Header.Caption = WatchListConstants.CAP_PricingProvider;

                UltraGridColumn colPutOrCall = gridBand.Columns[WatchListConstants.COL_PutOrCall];
                colPutOrCall.Header.Caption = WatchListConstants.CAP_PutOrCall;

                UltraGridColumn colRequestedSymbology = gridBand.Columns[WatchListConstants.COL_RequestedSymbology];
                colRequestedSymbology.Header.Caption = WatchListConstants.CAP_RequestedSymbology;

                UltraGridColumn colReuterSymbol = gridBand.Columns[WatchListConstants.COL_ReuterSymbol];
                colReuterSymbol.Header.Caption = WatchListConstants.CAP_ReuterSymbol;

                UltraGridColumn colSedolSymbol = gridBand.Columns[WatchListConstants.COL_SedolSymbol];
                colSedolSymbol.Header.Caption = WatchListConstants.CAP_SedolSymbol;

                UltraGridColumn colSelectedFeedPrice = gridBand.Columns[WatchListConstants.COL_SelectedFeedPrice];
                colSelectedFeedPrice.Header.Caption = WatchListConstants.CAP_SelectedFeedPrice;

                UltraGridColumn colSharesOutstanding = gridBand.Columns[WatchListConstants.COL_SharesOutstanding];
                colSharesOutstanding.Header.Caption = WatchListConstants.CAP_SharesOutstanding;

                UltraGridColumn colStrikePrice = gridBand.Columns[WatchListConstants.COL_StrikePrice];
                colStrikePrice.Header.Caption = WatchListConstants.CAP_StrikePrice;

                UltraGridColumn colTheoreticalPrice = gridBand.Columns[WatchListConstants.COL_TheoreticalPrice];
                colTheoreticalPrice.Header.Caption = WatchListConstants.CAP_TheoreticalPrice;

                UltraGridColumn colTotalVolume = gridBand.Columns[WatchListConstants.COL_TotalVolume];
                colTotalVolume.Header.Caption = WatchListConstants.CAP_TotalVolume;

                UltraGridColumn colTradeVolume = gridBand.Columns[WatchListConstants.COL_TradeVolume];
                colTradeVolume.Header.Caption = WatchListConstants.CAP_TradeVolume;

                UltraGridColumn colUnderlyingCategory = gridBand.Columns[WatchListConstants.COL_UnderlyingCategory];
                colUnderlyingCategory.Header.Caption = WatchListConstants.CAP_UnderlyingCategory;

                UltraGridColumn colUnderlyingData = gridBand.Columns[WatchListConstants.COL_UnderlyingData];
                colUnderlyingData.Header.Caption = WatchListConstants.CAP_UnderlyingData;

                UltraGridColumn colUnderlyingSymbol = gridBand.Columns[WatchListConstants.COL_UnderlyingSymbol];
                colUnderlyingSymbol.Header.Caption = WatchListConstants.CAP_UnderlyingSymbol;

                UltraGridColumn colUpdateTime = gridBand.Columns[WatchListConstants.COL_UpdateTime];
                colUpdateTime.Header.Caption = WatchListConstants.CAP_UpdateTime;
                colUpdateTime.CellActivation = Activation.NoEdit;

                UltraGridColumn colVolume10DAvg = gridBand.Columns[WatchListConstants.COL_Volume10DAvg];
                colVolume10DAvg.Header.Caption = WatchListConstants.CAP_Volume10DAvg;

                UltraGridColumn colXDividendDate = gridBand.Columns[WatchListConstants.COL_XDividendDate];
                colXDividendDate.Header.Caption = WatchListConstants.CAP_XDividendDate;
                colXDividendDate.CellActivation = Activation.NoEdit;

                UltraGridColumn colBid = gridBand.Columns[WatchListConstants.COL_Bid];
                colBid.Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Bid);
                colBid.CellAppearance.ForeColor = Color.Blue;

                UltraGridColumn colAsk = gridBand.Columns[WatchListConstants.COL_Ask];
                colAsk.Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Ask);
                colAsk.CellAppearance.ForeColor = Color.Red;

                UltraGridColumn colBloombergSymbolWithExchangeCode = gridBand.Columns[WatchListConstants.COL_BloombergSymbolWithExchangeCode];
                colBloombergSymbolWithExchangeCode.Header.Caption = WatchListConstants.CAP_BloombergSymbolWithExchangeCode;

                gridBand.Columns[WatchListConstants.COL_StrikePrice].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_StrikePrice);

                gridBand.Columns[WatchListConstants.COL_TheoreticalPrice].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_TheoreticalPrice);

                gridBand.Columns[WatchListConstants.COL_LastPrice].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_LastPrice);

                gridBand.Columns[WatchListConstants.COL_TradeVolume].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_TradeVolume);

                gridBand.Columns[WatchListConstants.COL_Change].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Change);

                gridBand.Columns[WatchListConstants.COL_PctChange].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_PctChange);

                gridBand.Columns[WatchListConstants.COL_MarkPrice].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_MarkPrice);

                gridBand.Columns[WatchListConstants.COL_Previous].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Previous);

                gridBand.Columns[WatchListConstants.COL_Open].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Open);

                gridBand.Columns[WatchListConstants.COL_High].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_High);

                gridBand.Columns[WatchListConstants.COL_Low].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Low);

                gridBand.Columns[WatchListConstants.COL_AvgVolume].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_AvgVolume);

                gridBand.Columns[WatchListConstants.COL_PreferencedPrice].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_PreferencedPrice);

                gridBand.Columns[WatchListConstants.COL_AverageVolume20Day].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_AverageVolume20Day);

                gridBand.Columns[WatchListConstants.COL_Volume10DAvg].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Volume10DAvg);

                gridBand.Columns[WatchListConstants.COL_Spread].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Spread);

                gridBand.Columns[WatchListConstants.COL_GapOpen].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_GapOpen);

                gridBand.Columns[WatchListConstants.COL_SelectedFeedPrice].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_SelectedFeedPrice);

                gridBand.Columns[WatchListConstants.COL_MarketCapitalization].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_MarketCapitalization);

                gridBand.Columns[WatchListConstants.COL_Dividend].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Dividend);

                gridBand.Columns[WatchListConstants.COL_DividendAmtRate].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_DividendAmtRate);

                gridBand.Columns[WatchListConstants.COL_AnnualDividend].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_AnnualDividend);

                gridBand.Columns[WatchListConstants.COL_DividendYield].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_DividendYield);

                gridBand.Columns[WatchListConstants.COL_FinalDividendYield].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_FinalDividendYield);

                gridBand.Columns[WatchListConstants.COL_Mid].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Mid);

                gridBand.Columns[WatchListConstants.COL_Imid].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Imid);

                gridBand.Columns[WatchListConstants.COL_ImpliedVol].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_ImpliedVol);

                gridBand.Columns[WatchListConstants.COL_FinalImpliedVol].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_InterestRate);

                gridBand.Columns[WatchListConstants.COL_FinalInterestRate].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_FinalInterestRate);

                gridBand.Columns[WatchListConstants.COL_OpenInterest].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_OpenInterest);

                gridBand.Columns[WatchListConstants.COL_ForwardPoints].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_ForwardPoints);

                gridBand.Columns[WatchListConstants.COL_Theta].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Theta);

                gridBand.Columns[WatchListConstants.COL_Vega].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Vega);

                gridBand.Columns[WatchListConstants.COL_Rho].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Rho);

                gridBand.Columns[WatchListConstants.COL_Gamma].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Gamma);

                gridBand.Columns[WatchListConstants.COL_VWAP].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_VWAP);

                gridBand.Columns[WatchListConstants.COL_Delta].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Delta);

                gridBand.Columns[WatchListConstants.COL_Beta_5yrMonthly].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Beta_5yrMonthly);

                gridBand.Columns[WatchListConstants.COL_DaysToExpiration].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_DaysToExpiration);

                gridBand.Columns[WatchListConstants.COL_AskSize].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_AskSize);

                gridBand.Columns[WatchListConstants.COL_BidSize].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_BidSize);

                gridBand.Columns[WatchListConstants.COL_SharesOutstanding].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_SharesOutstanding);

                gridBand.Columns[WatchListConstants.COL_DividendInterval].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_DividendInterval);

                gridBand.Columns[WatchListConstants.COL_TotalVolume].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_TotalVolume);

                gridBand.Columns[WatchListConstants.COL_ExchangeID].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_ExchangeID);

                gridBand.Columns[WatchListConstants.COL_StockBorrowCost].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_StockBorrowCost);

                gridBand.Columns[WatchListConstants.COL_High52W].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_High52W);

                gridBand.Columns[WatchListConstants.COL_Low52W].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_Low52W);

                gridBand.Columns[ApplicationConstants.CONST_AUECID].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(ApplicationConstants.CONST_AUECID);

                gridBand.Columns[ApplicationConstants.CONST_Multiplier].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(ApplicationConstants.CONST_Multiplier);

                gridBand.Columns[WatchListConstants.COL_DelayInterval].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_DelayInterval);

                gridBand.Columns[WatchListConstants.COL_InterestRate].Format = WatchListConstants.GetDefaultCloumnWiseDecimalDigits(WatchListConstants.COL_InterestRate);

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
        /// Handles the InitializeRow event of the dataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="InitializeRowEventArgs"/> instance containing the event data.</param>
        void dataGrid_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(dataGrid))
                {
                    string tick = e.Row.GetCellValue(WatchListConstants.COL_LastTick).ToString();
                    if (tick.Equals("UP_TICK") || tick.Equals("UP_UNCHANGED"))
                    {
                        e.Row.Cells[WatchListConstants.COL_LastPrice].Appearance.ForeColor = Color.Green;
                        e.Row.Cells[WatchListConstants.COL_Change].Appearance.ForeColor = Color.Green;
                        e.Row.Cells[WatchListConstants.COL_PctChange].Appearance.ForeColor = Color.Green;
                    }
                    else if (tick.Equals("DOWN_TICK") || tick.Equals("DOWN_UNCHANGED"))
                    {
                        e.Row.Cells[WatchListConstants.COL_LastPrice].Appearance.ForeColor = Color.Red;
                        e.Row.Cells[WatchListConstants.COL_Change].Appearance.ForeColor = Color.Red;
                        e.Row.Cells[WatchListConstants.COL_PctChange].Appearance.ForeColor = Color.Red;
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

        /// <summary>
        /// This Method is used to change the row values of ultraGrid with symboldata object.
        /// </summary>
        /// <param name="ultraGrid"></param>
        /// <param name="rowIndex"></param>
        /// <param name="symbolData"></param>
        public void UpdateRowInGridWithSymbolData(SymbolData symbolData)
        {
            try
            {
                if (UIValidation.GetInstance().validate(dataGrid))
                {
                    if (dataGrid.InvokeRequired)
                    {
                        UIThreadMarshellerForUpdateRow mi = new UIThreadMarshellerForUpdateRow(UpdateRowInGridWithSymbolData);
                        this.BeginInvoke(mi, new object[] { symbolData });
                    }
                    else
                    {
                        DataRow row = _gridData.Rows.Find(symbolData.Symbol);
                        if (row != null)
                        {
                            //ChangeSpecificCellColor(rowIndex, ultraGrid, symbolData);
                            foreach (PropertyInfo propertyInfo in _propertyInfos)
                            {
                                row[propertyInfo.Name] = propertyInfo.GetValue(symbolData);
                            }
                        }
                        _gridData.AcceptChanges();
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
        /// This method add new row with symboldata object values into ultraGrid.
        /// </summary>
        /// <param name="ultraGrid">UltraGrid</param>
        /// <param name="symbolData">string</param>
        public void AddNewBlankRowIntoGrid(string symbol, bool isInitial)
        {
            try
            {
                if (isInitial || UIValidation.GetInstance().validate(dataGrid))
                {
                    if (dataGrid.InvokeRequired)
                    {
                        UIThreadMarshellerForAddRow mi = new UIThreadMarshellerForAddRow(AddNewBlankRowIntoGrid);
                        this.BeginInvoke(mi, new object[] { symbol, isInitial });
                    }
                    else
                    {
                        DataRow dataRow = _gridData.NewRow();
                        dataRow[WatchListConstants.COL_Symbol] = symbol;
                        _gridData.Rows.Add(dataRow);
                        _gridData.AcceptChanges();
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
        /// This method is use to load the layout of grid and applay it.
        /// </summary>
        /// <param name="CompanyUserID">int</param>
        /// <param name="index">int</param>
        /// <param name="ultraGrid">UltraGrid</param>
        public void GridLayoutLoad(int CompanyUserID)
        {
            try
            {
                string path = Application.StartupPath + "\\Prana Preferences\\" + CompanyUserID + "\\WatchList_GridLayout_" + _tabName + ".xml";
                if (File.Exists(path))
                {
                    dataGrid.DisplayLayout.LoadFromXml(path);
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

        /// <summary>
        /// Sets the theme for user control.
        /// </summary>
        internal void SetThemeForUserControl()
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(dataGrid, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_WATCHLIST);
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
        /// This method is use to enable or disable option of right-click menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void EnableDsiableStripOptions(object sender, EventArgs e)
        {
            try
            {
                mnuSymbolLookup.Items[0].Enabled = false;
                mnuSymbolLookup.Items[1].Enabled = false;
                mnuSymbolLookup.Items[2].Enabled = false;
                mnuSymbolLookup.Items[3].Enabled = false;
                if (dataGrid.ActiveRow != null)
                {
                    mnuSymbolLookup.Items[4].Visible = (dataGrid.ActiveRow.Cells["categoryCode"].Text != string.Empty && ((AssetCategory)dataGrid.ActiveRow.Cells["categoryCode"].Value != AssetCategory.Option
                        && (AssetCategory)dataGrid.ActiveRow.Cells["categoryCode"].Value != AssetCategory.EquityOption && (AssetCategory)dataGrid.ActiveRow.Cells["categoryCode"].Value != AssetCategory.FutureOption
                        && (AssetCategory)dataGrid.ActiveRow.Cells["categoryCode"].Value != AssetCategory.FXOption))
                        && ModuleManager.CheckModulePermissioning(PranaModules.WATCHLIST_MODULE, PranaModules.OPTIONCHAIN_MODULE) && dataGrid.Rows.Select(x => x.Selected).Count() > 0;
                    AdjustPositionStripMenuItem.Enabled = ModuleManager.CheckModulePermissioning(PranaModules.PERCENT_TRADING_TOOL, PranaModules.PERCENT_TRADING_TOOL);
                }
                foreach (var row in dataGrid.Rows)
                {
                    bool isRowSelected = Convert.ToBoolean(row.Cells["Select"].Text);
                    if (isRowSelected)
                    {
                        mnuSymbolLookup.Items[0].Enabled = true;
                        mnuSymbolLookup.Items[1].Enabled = true;
                        mnuSymbolLookup.Items[3].Enabled = true;
                        return;
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

        private void dataGrid_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    System.Drawing.Point mousePoint = new System.Drawing.Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    if (element == null)
                    {
                        dataGrid.ActiveRow = null;
                    }

                    UltraGridCell cell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                    if (cell != null)
                    {
                        cell.Row.Activate();
                        cell.Row.Selected = true;
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
        /// This method is use to Open the PTT with select rows symbol.
        /// </summary>
        private void PTTTradeSymbolToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (sender != null && dataGrid.ActiveRow != null)
                    SendSymbolToPTT(null, new EventArgs<string>(dataGrid.ActiveRow.Cells[WatchListConstants.COL_Symbol].Value.ToString()));
                else
                    SetLabelMessage("No row selected on tab");
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
        /// This method is use to Open the TT with select rows symbol.
        /// </summary>
        private void TradeSymbolToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    ToolStripItem button = (ToolStripItem)sender;
                    bool isBuyTrade = button.Text.Equals("Buy");
                    if (dataGrid.Rows.Count > 0)
                    {
                        if (dataGrid.Rows.Where(cells => Convert.ToBoolean(cells.Cells["Select"].Text) == true).Count() > 1)
                        {
                            OrderBindingList orderList = new OrderBindingList();
                            for (int rowIndex = 0; rowIndex < dataGrid.Rows.Count; rowIndex++)
                            {
                                bool isRowSelected = Convert.ToBoolean(dataGrid.Rows[rowIndex].Cells["Select"].Text);
                                if (isRowSelected)
                                {
                                    string symbol = dataGrid.Rows[rowIndex].Cells[WatchListConstants.COL_Symbol].Value.ToString();
                                    if (!string.IsNullOrEmpty(symbol))
                                    {
                                        OrderSingle order = _symbolLiveFeedManager.CreateOrderSingleForMTT(symbol, isBuyTrade);
                                        orderList.Add(order);
                                    }
                                }
                            }

                            SendSymbolToMTT(null, new EventArgs<OrderBindingList>(orderList));
                        }
                        else if (dataGrid.Rows.Where(cells => Convert.ToBoolean(cells.Cells["Select"].Text) == true).Count() == 1)
                        {
                            UltraGridRow row = dataGrid.Rows.First(cells => Convert.ToBoolean(cells.Cells["Select"].Text));

                            string symbol = GetSymbologyBasedSymbolFromRow(row).ToUpper();
                            if (!string.IsNullOrEmpty(symbol))
                            {
                                if (isBuyTrade)
                                {
                                    SendSymbolToTT(null, new EventArgs<string, string>(symbol, "Ask"));
                                }
                                else
                                    SendSymbolToTT(null, new EventArgs<string, string>(symbol, "Bid"));
                            }
                        }
                        else
                            SetLabelMessage("No row selected on tab");
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
        /// Handles the Click event of the deleteSymbolBtn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DeleteSymbolBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGrid.Rows.Count <= 0)
                {
                    SetLabelMessage(WatchListConstants.MSG_NO_ROW);
                    return;
                }
                List<string> selectedSymbols = new List<string>();
                for (int rowIndex = 0; rowIndex < dataGrid.Rows.Count; rowIndex++)
                {
                    bool isRowSelected = Convert.ToBoolean(dataGrid.Rows[rowIndex].Cells["Select"].Text);
                    if (isRowSelected)
                    {
                        string symbol = dataGrid.Rows[rowIndex].Cells[WatchListConstants.COL_Symbol].Value.ToString();
                        selectedSymbols.Add(symbol);
                        _symbolLiveFeedManager.DeleteSymbolFromTab(symbol, _tabName);
                        //rowIndex--;
                    }
                }
                if (selectedSymbols.Count > 0)
                {
                    for (int i = _gridData.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = _gridData.Rows[i];
                        if (selectedSymbols.Contains(dr[WatchListConstants.COL_Symbol]))
                            dr.Delete();
                    }
                    _gridData.AcceptChanges();
                    SetLabelMessage("Symbol deleted.");
                }
                else
                    SetLabelMessage(WatchListConstants.MSG_SELECT_ROW_FOR_DEL);
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
        /// This method is use to remove all filters from Grid
        /// </summary>
        private void RemoveFilterToolStripMenuItemForFileGrid_Click(object sender, EventArgs e)
        {
            try
            {
                dataGrid.DisplayLayout.Bands[0].ColumnFilters.ClearAllFilters();
                dataGrid.ActiveRowScrollRegion.Scroll(RowScrollAction.Top);
                SetLabelMessage(WatchListConstants.MSG_FILTER_REMOVED);
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
        /// This method is use to save the layout of current grid into xml.
        /// </summary>
        private void SaveLayoutToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                string startPath = Application.StartupPath + "\\Prana Preferences\\" + _companyUserID;
                if (!Directory.Exists(startPath))
                {
                    Directory.CreateDirectory(startPath);
                }
                string gridLayoutPath = startPath + "\\WatchList_GridLayout_" + _tabName + ".xml";
                dataGrid.DisplayLayout.SaveAsXml(gridLayoutPath, Infragistics.Win.UltraWinGrid.PropertyCategories.All);
                SetLabelMessage(WatchListConstants.MSG_LAYOUT_SAVED);
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
        /// This method is used to open Option Chain Window
        /// </summary>
        private void optionChainToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (OptionChainModuleOpened != null)
            {
                string symbol = GetSymbologyBasedSymbolFromRow(dataGrid.ActiveRow);
                OptionChainModuleOpened(this, new EventArgs<int, string>(_symbolLiveFeedManager.GetTabNumberFromTabName(TabName), symbol));
            }
        }

        /// <summary>
        /// Gets the symbology based symbol from row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        private string GetSymbologyBasedSymbolFromRow(UltraGridRow row)
        {
            try
            {
                switch (SymbologyHelper.DefaultSymbology)
                {
                    case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                        return row.Cells[WatchListConstants.COL_BloombergSymbol].Text;
                    case ApplicationConstants.SymbologyCodes.ActivSymbol:
                        return row.Cells[WatchListConstants.COL_ActivSymbol].Text;
                    case ApplicationConstants.SymbologyCodes.FactSetSymbol:
                        return row.Cells[WatchListConstants.COL_FactSetSymbol].Text;
                    default:
                        return row.Cells[WatchListConstants.COL_Symbol].Text;
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
            return string.Empty;
        }

        /// <summary>
        /// Unlinks the tab.
        /// </summary>
        internal void UnlinkTab()
        {
            try
            {
                if (UIValidation.GetInstance().validate(linkTabBtn))
                {
                    if (dataGrid.InvokeRequired)
                    {
                        UIThreadMarshellerForUnlinkTab mi = new UIThreadMarshellerForUnlinkTab(UnlinkTab);
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        linkTabBtn.Text = "Link tab";
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
        /// used to Export Data for automation
        /// </summary>
        /// <param name="exportFilePath"></param>
        public void ExportGridData(string exportFilePath)
        {
            try
            {
                // Create a new instance of the exporter
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                string directoryPath = Path.GetDirectoryName(exportFilePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // Perform the export
                exporter.Export(dataGrid, exportFilePath);
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
