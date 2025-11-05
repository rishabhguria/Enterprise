using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.LiveFeed;
using Prana.ClientCommon;
using Prana.ClientCommon.BLL;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.ReconciliationNew;
using Prana.Utilities.ImportExportUtilities.Excel;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlSetForex : UserControl, ILiveFeedCallback, IPublishing
    {
        bool _isHeaderCheckBoxChecked = false;

        private bool _isDataCopiedFromBackDate = false;
        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        private MethdologySelected _methodologySelected = MethdologySelected.DateRange;
        public delegate void PriceRespUIInvokeDelegate(DataTable dt);
        delegate void SetTextCallback2(object sender, EventArgs<string> e);

        DataTable _dtGridDataSource = new DataTable();
        public static DateTime _dateSelected = DateTime.Now;
        public static DateTime _startDate = DateTime.Now;
        public static DateTime _endDate = DateTime.Now;
        public static int _filter = 0;
        bool _isPermissionToApprove = false;

        static int _userID = int.MinValue;
        static string _forexRateFilePath = string.Empty;
        static string _forexRateLayoutDirectoryPath = string.Empty;
        const string DATEFORMAT = "MM/dd/yyyy";

        List<int> _alreadyPromptedAccountsForLock = new List<int>();
        private List<UltraGridRow> _selectedColumnList = new List<UltraGridRow>();

        int _maxDigitsNumber;

        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServicesProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
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

        public ctrlSetForex()
        {
            try
            {
                InitializeComponent();
                if (!CustomThemeHelper.IsDesignMode())
                {
                    string maxDigits = ConfigurationHelper.Instance.GetAppSettingValueByKey("MaxDigitsOnForexConversion");
                    CreatePricingServiceProxy();
                    if (CashManagementServices == null)
                        CreateCashManagementProxy();
                    this.InitializeControl();
                    ClientPricingManager.GetInstance.PriceResponseEventHandler += SMPricing_PriceResponseEventHandler;

                    if (!string.IsNullOrEmpty(maxDigits) && int.TryParse(maxDigits, out _maxDigitsNumber))
                    {
                        if (_maxDigitsNumber < 2)
                            _maxDigitsNumber = 10;
                    }
                    else
                    {
                        _maxDigitsNumber = 10;
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

        void _securityMaster_CentralSMDisconnected(object sender, EventArgs<string> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        SetTextCallback2 mi = new SetTextCallback2(_securityMaster_CentralSMDisconnected);
                        this.BeginInvoke(mi, new object[] { sender, e });
                    }
                    else
                    {
                        if (btnFetchFromAPI.Text == "Fetching...")
                        {
                            SetControlsOnFetchAPIClick(false);
                            ultraStatusBarForexRateStatus.Text = " Central Com Server Disconnected.";
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

        void SMPricing_PriceResponseEventHandler(object sender, EventArgs<DataTable> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() => SMPricing_PriceResponseEventHandler(sender, e)));

                    }
                    else
                    {
                        DataTable dt = e.Value;
                        SetControlsOnFetchAPIClick(false);
                        DataColumn column = dt.Columns[0];
                        String colName = column.ColumnName;
                        foreach (DataRow priceData in dt.Rows)
                        {


                            String symbol = priceData["Symbol"].ToString();
                            DateTime date;
                            if (DateTime.TryParse(priceData["Date"].ToString(), out date))
                            {

                                Dictionary<int, Double> accountWisePrice = GetAccountAndFieldForPriceResponse(priceData, colName);
                                String dateInResp = date.ToString("MM/dd/yyyy");
                                // double price = 10; //TODO get price
                                // int AccountId = 1182; //TODO get account id
                                foreach (DataRow dRow in _dtGridDataSource.Rows)
                                {
                                    int accountIdInGridRow = int.Parse(dRow["FundID"].ToString());
                                    String symbolinGridRow = dRow["BloombergSymbol"].ToString();
                                    //String dateInResp = date.Date.ToString("MM/DD/YYYY");
                                    //modified by: Bharat Raturi, 05 may 2014
                                    //purpose: represent the date in proper format

                                    if (symbolinGridRow.Equals(symbol.ToUpper(), StringComparison.OrdinalIgnoreCase) && accountWisePrice.ContainsKey(accountIdInGridRow) && _dtGridDataSource.Columns.Contains(dateInResp))
                                    {
                                        _isDataCopiedFromBackDate = true;
                                        dRow[dateInResp] = accountWisePrice[accountIdInGridRow];
                                        dRow["PricingSource"] = PricingSource.BloombergDLWS;
                                        dRow["Comments"] = string.Empty;

                                        if (accountWisePrice[accountIdInGridRow] == 0)
                                            dRow["Comments"] = "Invalid Security";
                                        //break;
                                        if (dRow.RowState == DataRowState.Unchanged)
                                            dRow.SetModified();

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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private Dictionary<int, Double> GetAccountAndFieldForPriceResponse(DataRow priceData, string colName)
        {
            Dictionary<int, Double> accountWisePrice = new Dictionary<int, double>();
            try
            {
                string symbol = priceData["Symbol"].ToString().ToUpper();
                List<int> accountList = GetAccountIDFromdict(priceData, colName);
                foreach (int accountID in accountList)
                {
                    if (_requestedSymbols.ContainsKey(accountID))
                    {
                        Dictionary<string, SymbolPriceRequest> detailsDict = new Dictionary<string, SymbolPriceRequest>();
                        detailsDict = _requestedSymbols[accountID];
                        if (detailsDict.ContainsKey(symbol))
                        {
                            SymbolPriceRequest symbolReq = detailsDict[symbol];
                            double price = 0;
                            if (priceData.Table.Columns.Contains(symbolReq.PriceFieldType.ToString()) && priceData["SecondarySource"].ToString().Equals(symbolReq.secondaryPricingSource))
                            {

                                double.TryParse(priceData[symbolReq.PriceFieldType.ToString()].ToString(), out price);
                            }
                            else if (symbolReq.PriceFieldType == PricingDataType.Avg_AskBid && priceData["SecondarySource"].ToString().Equals(symbolReq.secondaryPricingSource))
                            {
                                double avgAsk_Bid = 0;
                                bool isDataInsufficient = false;
                                IEnumerable<DataColumn> requiredColumns = priceData.Table.Columns.Cast<DataColumn>().Where(x => x.ColumnName.Equals(PricingDataType.Ask.ToString()) || x.ColumnName.Equals(PricingDataType.Bid.ToString()));

                                foreach (DataColumn col in requiredColumns)
                                {
                                    if (priceData[col.ColumnName] != null && priceData[col.ColumnName] != DBNull.Value && !String.IsNullOrWhiteSpace(priceData[col.ColumnName].ToString()))
                                    {
                                        if (double.TryParse(priceData[col.ColumnName].ToString(), out price))
                                            avgAsk_Bid += price;
                                    }
                                    else
                                    {
                                        isDataInsufficient = true;
                                        break;
                                    }
                                }
                                if (!isDataInsufficient)
                                {
                                    price = avgAsk_Bid / 2;
                                }
                            }
                            if (!accountWisePrice.ContainsKey(symbolReq.accountId))
                            {
                                accountWisePrice.Add(symbolReq.accountId, price);
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
            return accountWisePrice;
        }



        private List<int> GetAccountIDFromdict(DataRow priceData, string colName)
        {
            List<int> accountlist = new List<int>();
            try
            {
                PricingDataType colNameEnum = new PricingDataType();
                Enum.TryParse(colName, out colNameEnum);
                foreach (int accountID in _requestedSymbols.Keys)
                {
                    Dictionary<string, SymbolPriceRequest> dictRequestedSymbol = _requestedSymbols[accountID];
                    String symbol = priceData["Symbol"].ToString().ToUpper();
                    if (dictRequestedSymbol.ContainsKey(symbol))
                    {
                        SymbolPriceRequest data = dictRequestedSymbol[symbol];
                        if (data.PriceFieldType.Equals(colNameEnum))
                        {
                            accountlist.Add(accountID);
                        }
                        else if (data.PriceFieldType.Equals(PricingDataType.Avg_AskBid) && priceData.Table.Columns.Contains(PricingDataType.Ask.ToString()) && priceData.Table.Columns.Contains(PricingDataType.Bid.ToString()))
                        {
                            accountlist.Add(accountID);
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
            return accountlist;
        }





        /// <summary>
        /// Import the pricing details from a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dtGridDataSource != null && _dtGridDataSource.Columns.Contains("Select"))
                {
                    var selectedRows = from row in _dtGridDataSource.AsEnumerable()
                                       where row["Select"].ToString().Equals("True")
                                       select row;

                    if (selectedRows != null && selectedRows.Count() > 0)
                    {
                        // File open dialog, ask user to import forex rates.
                        OpenFileDialog openFileDialog1 = new OpenFileDialog();
                        openFileDialog1.InitialDirectory = "\\\\tsclient\\C";
                        openFileDialog1.Title = "Select file for import";
                        openFileDialog1.Filter = "Text Files (*.xls)|*.xls";
                        string strFileName = string.Empty;
                        DialogResult importResult = openFileDialog1.ShowDialog();
                        if (importResult == DialogResult.OK)
                        {
                            strFileName = openFileDialog1.FileName;
                        }
                        else if (importResult == DialogResult.Cancel)
                        {
                            InformationMessageBox.Display("Operation cancelled by User", "Forex Rate Import");
                            return;
                        }
                        UpdationAfterImporting(strFileName);
                    }
                    else
                    {
                        MessageBox.Show("Please select symbols to import rates.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
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
        /// Get the price details from file and load in the grid
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        private void UpdationAfterImporting(string filePath)
        {
            StringBuilder accountsWithNavLock = new StringBuilder();
            string message = "Forex Rate cannot be imported for following accounts before NAV Lock Date\n Details are:\n";
            string strFileName = filePath;
            DataSet result = new DataSet();

            ExcelDataReader spreadSheet = null;
            //string fileName = string.Empty;
            FileStream fs = null;
            try
            {
                fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception)
            {
                WarningMessageBox.Display("File is in use. Close file and retry", "Forex rate Import");
                return;
            }
            try
            {
                spreadSheet = new ExcelDataReader(fs);

                if (spreadSheet != null && spreadSheet.WorkbookData != null && spreadSheet.WorkbookData.Tables != null && spreadSheet.WorkbookData.Tables.Count != 0)
                {
                    // create a new table whose columns will be in proper organized according to our requirement

                    DataTable tableForexRateImported = new DataTable();
                    // result is a Dataset , get the excel sheet values into the Dataset named result
                    result = spreadSheet.WorkbookData;
                    // copy all the record from result Dataset to the Table dt
                    tableForexRateImported = result.Tables[0].Copy();

                    // now rename the columns of the Table, 
                    // xml create a row of Headers of the excel sheet, so 1st row will give the headers
                    // of the table so rename the columns
                    for (int j = 0; j < result.Tables[0].Columns.Count; j++)
                    {
                        tableForexRateImported.Columns[j].ColumnName = result.Tables[0].Rows[0].ItemArray[j].ToString();
                    }
                    // delete the 1st row of the table because columns has given the same name as the headers
                    tableForexRateImported.Rows[0].Delete();
                    tableForexRateImported.TableName = "TableForexRateImported";


                    DataTable tableGridForexRate = _dtGridDataSource;
                    if (_dtGridDataSource.Rows.Count > 0)
                    {
                        //Looping through the grid to update values in the grid from the imported data.
                        foreach (DataRow rowTableGridForexRates in tableGridForexRate.Rows)
                        {
                            if (tableGridForexRate.Columns.Contains("Select") == true && rowTableGridForexRates["Select"].ToString() == "True")
                            {
                                foreach (DataColumn col in tableGridForexRate.Columns)
                                {
                                    DateTime dateParsedOut = DateTime.Now;
                                    if (DateTime.TryParse(col.ToString(), out dateParsedOut))
                                    {
                                        string tabColSymbol = rowTableGridForexRates["BloombergSymbol"].ToString();
                                        string tabColAccount = rowTableGridForexRates["FundName"].ToString();
                                        string tabColDate = col.ColumnName.ToString();

                                        foreach (DataRow rowTableForexRateImported in tableForexRateImported.Rows)
                                        {
                                            string fileTabColSymbol = rowTableForexRateImported["BloombergSymbol"].ToString();
                                            string fileColAccount = rowTableForexRateImported["FundName"].ToString();

                                            foreach (DataColumn importFileCol in tableForexRateImported.Columns)
                                            {
                                                bool isParsed = false;
                                                double outResult;
                                                isParsed = double.TryParse(importFileCol.ToString(), out outResult);
                                                if (isParsed)
                                                {
                                                    int accountID = CachedDataManager.GetInstance.GetAccountID(fileColAccount);
                                                    DateTime fileDateParsedOut = DateTime.FromOADate(Convert.ToDouble(importFileCol.ToString()));
                                                    string fileColDate = fileDateParsedOut.ToString(DATEFORMAT);
                                                    bool isImportAllowed = NAVLockManager.GetInstance.ValidateTrade(accountID, Convert.ToDateTime(fileColDate));
                                                    if (tabColDate.Equals(fileColDate))
                                                    {

                                                        //If the symbols match i.e. in the grid and in the imported data.
                                                        if (string.Compare(tabColSymbol, fileTabColSymbol, true) == 0 && string.Compare(tabColAccount, fileColAccount, true) == 0)
                                                        {
                                                            if (isImportAllowed)
                                                            {
                                                                if (rowTableGridForexRates.RowState == DataRowState.Unchanged)
                                                                {
                                                                    rowTableGridForexRates.SetModified();
                                                                    rowTableGridForexRates["Select"] = "True";
                                                                }
                                                                //Added By : Manvendra P.
                                                                //Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3426

                                                                int tabColAccountID = CachedDataManager.GetInstance.GetAccountID(tabColAccount);
                                                                if (!NAVLockManager.GetInstance.ValidateTrade(tabColAccountID, Convert.ToDateTime(tabColDate)))
                                                                {
                                                                    rowTableGridForexRates["Comments"] = "NAV is Locked, price can not be imported.";
                                                                }
                                                                else
                                                                {
                                                                    rowTableGridForexRates[col] = rowTableForexRateImported[importFileCol];
                                                                    rowTableGridForexRates["PricingSource"] = PricingSource.Gateway;
                                                                }

                                                                rowTableGridForexRates["Comments"] = string.Empty;
                                                            }
                                                            else
                                                            {
                                                                if (!accountsWithNavLock.ToString().Contains(message))
                                                                    accountsWithNavLock.Append(message);
                                                                if (!accountsWithNavLock.ToString().Contains("FundName: " + fileColAccount))
                                                                {
                                                                    DateTime navLockDate = NAVLockManager.GetInstance.GetNavLockDateForAccount(accountID);
                                                                    accountsWithNavLock.Append("FundName: " + fileColAccount + " LockDate: " + navLockDate + "\n");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (accountsWithNavLock.Length > 0)
                {
                    MessageBox.Show(accountsWithNavLock.ToString(), "NAV Lock Alert", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    WarningMessageBox.Display("Data file columns are not in predefined format, \n 1st column value should be the account name. Please include header 'AccountName'.\n2nd column value should be the BloombergSymbol. Please include header 'BloombergSymbol'.\nRemaining column value should be forex rate with header as date in format 'MM/DD/YYYY'.\nPlease select the correct file and try again.", "Forex Rate Import");
                }
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>The message (name of the control here)</returns>
        public string getReceiverUniqueName()
        {
            try
            {
                return "ctrlSetForex";
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
            return null;
        }

        /// <summary>
        /// Initialize the control
        /// </summary>
        public void InitializeControl()
        {
            try
            {
                if (ucbClient.Rows.Count > 0)
                {
                    ucbClient.DataSource = null;
                }
                BindCurrencyComboBoxes();
                BindClientCombo();
                BindFilterCombo();
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
        /// Setup the layout of the account control
        /// 
        /// added by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        private void SetAccountControl()
        {
            try
            {
                if (ucbAccount.DataSource != null)
                {
                    if (!ucbAccount.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn cbAccount = ucbAccount.DisplayLayout.Bands[0].Columns.Add();
                        cbAccount.Key = "Selected";
                        cbAccount.Header.Caption = string.Empty;
                        cbAccount.Width = 25;
                        cbAccount.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        cbAccount.DataType = typeof(bool);
                        cbAccount.Header.VisiblePosition = 1;
                    }
                    ucbAccount.CheckedListSettings.CheckStateMember = "Selected";
                    ucbAccount.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    ucbAccount.CheckedListSettings.ListSeparator = " , ";
                    ucbAccount.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    ucbAccount.DisplayMember = "Value";
                    ucbAccount.ValueMember = "Key";
                    ucbAccount.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
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
        /// bind data on client combobox on startup
        /// </summary>
        private void BindClientCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();// EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(ReconType));

                // To bind all permitted clients for selected user
                Dictionary<int, string> dictClients = new Dictionary<int, string>();
                foreach (KeyValuePair<int, List<int>> clients in CachedDataManagerRecon.GetInstance.GetAllCompanyAccounts())
                {
                    if (CachedDataManager.GetUserPermittedCompanyList().ContainsKey(clients.Key))
                    {
                        if (!dictClients.ContainsKey(clients.Key))
                        {
                            dictClients.Add(clients.Key, CachedDataManager.GetUserPermittedCompanyList()[clients.Key]);
                        }
                    }
                }

                foreach (KeyValuePair<int, string> kvp in dictClients)
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value, kvp.Key);
                    listValues.Add(value);
                }
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2288
                listValues = listValues.OrderBy(e => e.DisplayText).ToList();
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                // cmbClient = new UltraCombo();
                ucbClient.DataSource = null;
                ucbClient.DataSource = listValues;
                ucbClient.DisplayMember = "DisplayText";
                ucbClient.ValueMember = "Value";
                ucbClient.DataBind();
                ucbClient.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                ucbClient.Value = -1;
                ucbClient.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
        /// bind data on filter combobox on startup
        /// </summary>
        private void BindFilterCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                listValues.Insert(1, new EnumerationValue("All", 0));
                listValues.Insert(2, new EnumerationValue("Missing Forex", 1));
                listValues.Insert(3, new EnumerationValue("Manual Source", 2));

                cmbFilter.DataSource = null;
                cmbFilter.DataSource = listValues;
                cmbFilter.DisplayMember = "DisplayText";
                cmbFilter.ValueMember = "Value";
                cmbFilter.DataBind();
                cmbFilter.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbFilter.Value = -1;
                cmbFilter.DisplayLayout.Bands[0].ColHeadersVisible = false;

                // To select "All" by default
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in cmbFilter.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "All")
                    {
                        cmbFilter.SelectedRow = row;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int rowsAffected = 0;
                DataTable dtDataToSave = new DataTable();
                dtDataToSave = GetDataToSave();
                String message = "Nothing to Save.";
                if (dtDataToSave != null && dtDataToSave.Rows.Count > 0)
                {
                    bool isAccountlocked = CheckforAccountLocked(dtDataToSave);//Check for Account lock
                    if (isAccountlocked)
                    {
                        //purpose: check if the datatable is not null before save
                        if (_dtGridDataSource.Rows.Count > 0)
                        {
                            rowsAffected = SaveForexRate(dtDataToSave);
                            if (rowsAffected > 0)
                            {
                                message = "Forex rate details saved.";
                                if (_isPermissionToApprove)
                                    message = "Forex rate details saved and approved.";

                            }
                        }
                    }
                }
                MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        string _colName = string.Empty;
        static Dictionary<int, Dictionary<string, SymbolPriceRequest>> _requestedSymbols = new Dictionary<int, Dictionary<string, SymbolPriceRequest>>();
        private void btnFetchFromAPI_Click(object sender, EventArgs e)
        {
            try
            {
                bool isRowSelect = false;
                //if (_securityMaster.IsCSMConnected)
                //{
                ultraStatusBarForexRateStatus.Text = string.Empty;
                if (_methodologySelected == MethdologySelected.Daily)
                {
                    _endDate = _startDate;
                }
                else if (_methodologySelected == MethdologySelected.Monthly)
                {
                    DateTime selectedDate = DateTime.Parse(dtStartDate.Value.ToString());

                    _startDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                    if (selectedDate.Month.Equals(DateTime.Today.Month))
                        _endDate = DateTime.Today;
                    else
                        _endDate = new DateTime(selectedDate.Year, selectedDate.Month, DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month));
                }


                if (_dtGridDataSource.Rows.Count > 0)
                {
                    _requestedSymbols = new Dictionary<int, Dictionary<string, SymbolPriceRequest>>();
                    _dtGridDataSource.AcceptChanges();

                    DateTime dtLiveFeedTime = _startDate;

                    foreach (DataColumn col in _dtGridDataSource.Columns)
                    {
                        DateTime dateOut = DateTime.Now;
                        DateTime dateGrid = DateTime.Now;
                        _colName = col.ColumnName;
                        string symbol = string.Empty;
                        if (DateTime.TryParse(_colName, out dateOut))
                        {
                            dateGrid = dateOut;
                            if (dtLiveFeedTime.Equals(dateGrid))
                            {
                                foreach (UltraGridRow filteredRow in this.grdForexRate.DisplayLayout.Rows.GetFilteredInNonGroupByRows())
                                {
                                    if (filteredRow.Cells["Select"].Value.ToString() == "True")
                                    {
                                        isRowSelect = true;
                                        //symbol = filteredRow.Cells["BloombergSymbol"].Value.ToString();
                                        symbol = filteredRow.Cells["BloombergSymbol"].Value.ToString();
                                        SymbolPriceRequest request = new SymbolPriceRequest();
                                        request.accountId = int.Parse(filteredRow.Cells["FundID"].Value.ToString());
                                        request.Symbol = symbol;
                                        //int auecId = int.Parse(filteredRow.Cells["AUECID"].Value.ToString());
                                        int accountID = int.Parse(filteredRow.Cells["FundID"].Value.ToString());
                                        //request.ExchangeId = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(auecId);
                                        //request.AssetId = CachedDataManager.GetInstance.GetAssetIdByAUECId(auecId);
                                        request.AssetId = (int)(AssetCategory.FX);
                                        request.ExchangeId = 30;
                                        filteredRow.Cells["Comments"].Value = string.Empty;

                                        //Added By : Manvendra P.
                                        //Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3426

                                        if (!NAVLockManager.GetInstance.ValidateTrade(accountID, Convert.ToDateTime(dateOut)))
                                        {
                                            filteredRow.Cells["Comments"].Value = "NAV is Locked, price can not be fetched.";
                                        }
                                        else
                                        {

                                            if (!string.IsNullOrEmpty(symbol))
                                            {
                                                if (!_requestedSymbols.ContainsKey(accountID))
                                                {

                                                    Dictionary<string, SymbolPriceRequest> detailsDict = new Dictionary<string, SymbolPriceRequest>();
                                                    detailsDict.Add(symbol, request);
                                                    _requestedSymbols.Add(accountID, detailsDict);
                                                }
                                                else
                                                {
                                                    Dictionary<string, SymbolPriceRequest> details = _requestedSymbols[accountID];

                                                    if (details.ContainsKey(symbol))
                                                    {
                                                        details[symbol] = request;
                                                    }
                                                    else
                                                    {
                                                        details.Add(symbol, request);
                                                    }
                                                }
                                            }
                                            // For cases where bloomberg symbol is blank
                                            else
                                            {
                                                filteredRow.Cells["Comments"].Value = "Invalid Security";
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }

                    if (_requestedSymbols.Count > 0)
                    {
                        int statusCode = ClientPricingManager.GetInstance.GetPrices_New(_requestedSymbols, _startDate, _endDate, true);
                        bool isFetching = true;
                        if (statusCode == -1)
                        {
                            isFetching = false;
                            SetControlsOnFetchAPIClick(isFetching);
                            return;
                        }
                        else
                        {
                            SetControlsOnFetchAPIClick(isFetching);
                        }
                    }
                    else
                    {
                        if (!isRowSelect)
                            MessageBox.Show("Please select symbols to fetch rates from API.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Please see comments column for rate fetching status.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("There are no symbols to fetch rates for them.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //}
                //else
                //{
                //    ultraStatusBarForexRateStatus.Text = " Central Com Server Disconnected.";
                //}

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
        /// To set FetchFromAPI button status 
        /// </summary>
        /// <param name="isFetching"></param>
        private void SetControlsOnFetchAPIClick(bool isFetching)
        {
            try
            {
                if (isFetching)
                {
                    btnFetchFromAPI.Text = "Fetching...";
                    btnFetchFromAPI.Enabled = false;
                }
                else
                {
                    btnFetchFromAPI.Text = "Fetch From API";
                    btnFetchFromAPI.Enabled = true;
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
        //private void UpdateMarkPriceAndFXDataWithOMIData(List<UserOptModelInput> symbolWiseOMIdata)
        //{
        //    try
        //    {
        //        //Updating the symbol's price when tab mark price is selected.

        //        foreach (UserOptModelInput userOMI in symbolWiseOMIdata)
        //        {
        //            //DateTime dt = (DateTime)DateTime.Now.Date;
        //            string symbol = userOMI.Symbol;
        //            //double price = userOMI.LastPrice;
        //            //bool uselastPrice = userOMI.LastPriceUsed;


        //            if (string.IsNullOrWhiteSpace(_colName))
        //            {
        //                return;
        //            }
        //            foreach (DataRow dRow in _dtGridDataSource.Rows)
        //            {
        //                if (string.Compare(dRow["BloombergSymbol"].ToString(), symbol, true) == 0 && _dtGridDataSource.Columns.Contains(_colName))
        //                {
        //                    if (userOMI.LastPrice.Equals(double.MinValue) && userOMI.LastPriceUsed)
        //                    {
        //                        dRow[_colName] = 0;
        //                    }
        //                    else
        //                    {
        //                        //_btnGetLiveFeedClicked = true;
        //                        if (userOMI.LastPriceUsed)
        //                        {
        //                            dRow[_colName] = userOMI.LastPrice;
        //                        }
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

        /// <summary>
        /// Export the data of the grid into an excel file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdForexRate.DataSource != null)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                    string pathName = null;
                    saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = Application.StartupPath;
                    saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        pathName = saveFileDialog1.FileName;
                    }
                    else
                    {
                        return;
                    }
                    string workbookName = "Report" + DateTime.Now.Date.ToString("yyyyMMdd");
                    workBook.Worksheets.Add(workbookName);

                    workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];

                    workBook = this.ultraGridExcelExporter1.Export(this.grdForexRate, workBook.Worksheets[workbookName]);
                    workBook.Save(pathName);
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

        private Dictionary<int, string> _currencyDict = new Dictionary<int, string>();
        private CurrencyCollection _currencyCollection = new CurrencyCollection();
        Infragistics.Win.UltraWinGrid.UltraDropDown cmbCurrency = new Infragistics.Win.UltraWinGrid.UltraDropDown();

        private void BindCurrencyComboBoxes()
        {
            try
            {
                _currencyCollection = WindsorContainerManager.GetCurrenciesWithSymbol();
                cmbCurrency.DataSource = null;
                cmbCurrency.DataSource = _currencyCollection;
                cmbCurrency.DisplayMember = "Symbol";
                cmbCurrency.ValueMember = "CurrencyID";
                Utils.UltraDropDownFilter(cmbCurrency, "Symbol");

                foreach (Prana.BusinessObjects.Currency currency in _currencyCollection)
                {
                    if (!_currencyDict.ContainsKey(currency.CurrencyID))
                    {
                        _currencyDict.Add(currency.CurrencyID, currency.Symbol);
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
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable BindGridForForexConversion(string xmlAccount, DateTime startDate, DateTime endDate, int dateMethodology, int filter)
        {
            DataTable dtConvertionRate = new DataTable();
            //DataTable dtNew = new DataTable();
            try
            {
                //Get Conversion Rate data for symbols for the selected date and methodology.
                dtConvertionRate = WindsorContainerManager.GetAccountWiseConversionRate(xmlAccount, startDate, endDate, dateMethodology, filter);

                //Purpose : For updating date column name in case of copy from date option
                if (_methodologySelected.Equals(MethdologySelected.Daily))
                {
                    foreach (DataColumn col in dtConvertionRate.Columns)
                    {
                        DateTime dateParsedOut = DateTime.Now;
                        string date;
                        if (DateTime.TryParse(col.ToString(), out dateParsedOut))
                        {
                            date = ((DateTime)dtStartDate.Value).ToString("MM/dd/yyyy");
                            dtConvertionRate.Columns[col.ToString()].ColumnName = date;
                        }
                    }
                }

                if (!dtConvertionRate.Columns.Contains("PricingSource"))
                {
                    DataColumn colPricingSource = new DataColumn("PricingSource", typeof(PricingSource));
                    colPricingSource.DefaultValue = PricingSource.None;
                    dtConvertionRate.Columns.Add(colPricingSource);
                }

                //Assigning the forex rate 0 to the symbols whose forex rate is blank.
                int colLength = dtConvertionRate.Columns.Count;

                foreach (DataRow dRow in dtConvertionRate.Rows)
                {
                    for (int i = 0; i < colLength; i++)
                    {
                        if (string.IsNullOrEmpty(dRow[i].ToString()))
                        {
                            dRow[i] = 0;
                        }
                    }
                    if ((int)Enum.Parse(typeof(PricingSource), dRow["SourceID"].ToString()) == 0)
                        dRow["PricingSource"] = PricingSource.None;
                    else
                        dRow["PricingSource"] = (PricingSource)Enum.Parse(typeof(PricingSource), dRow["SourceID"].ToString());
                    dRow.AcceptChanges();
                }
                if (dtConvertionRate.Rows.Count == 0)
                {
                    DataRow dtRow = dtConvertionRate.NewRow();
                    dtConvertionRate.Rows.Add(dtRow);
                    dtRow.AcceptChanges();
                }

                _dtGridDataSource = dtConvertionRate;
                if (_dtGridDataSource != null)
                {
                    if (optDaily.Checked == true)
                    {
                        foreach (DataRow dr in _dtGridDataSource.Rows)
                        {
                            foreach (DataColumn dc in _dtGridDataSource.Columns)
                            {
                                DateTime dateOut = DateTime.Now;
                                if (DateTime.TryParse(dc.ColumnName, out dateOut))
                                {
                                    if ((!string.IsNullOrEmpty(dr["FromCurrencyID"].ToString()) && !dr["FromCurrencyID"].ToString().Equals(ApplicationConstants.C_COMBO_SELECT)) && (!string.IsNullOrEmpty(dr["ToCurrencyID"].ToString()) && !dr["ToCurrencyID"].ToString().Equals(ApplicationConstants.C_COMBO_SELECT)) && (Convert.ToDouble((dr[dc.ColumnName].ToString())) > 0 && dr[dc.ColumnName] != System.DBNull.Value))
                                    {
                                        dr["Summary"] = "1 " + _currencyDict[Convert.ToInt32(dr["FromCurrencyID"].ToString())] + " = " + Convert.ToDouble(dr[dc.ColumnName].ToString()) + " " + _currencyDict[Convert.ToInt32(dr["ToCurrencyID"].ToString())];
                                        dr.AcceptChanges();
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
            return dtConvertionRate;
        }

        /// <summary>
        /// Initialize the layout of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdForexRate_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraWinGridUtils.EnableFixedFilterRow(e);
                grdForexRate.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                grdForexRate.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;

                if (grdForexRate.DisplayLayout.Bands[0].Columns.Exists("IsApproved"))
                {
                    grdForexRate.DisplayLayout.Bands[0].Columns["IsApproved"].Hidden = true;
                }

                if (grdForexRate.DisplayLayout.Bands[0].Columns.Exists("Select"))
                {
                    UltraGridColumn colBtnSelect = grdForexRate.DisplayLayout.Bands[0].Columns["Select"];

                    colBtnSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

                    colBtnSelect.Header.Caption = "";
                    colBtnSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    colBtnSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                    colBtnSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                    colBtnSelect.Header.Fixed = true;
                    colBtnSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colBtnSelect.Width = 30;
                    colBtnSelect.CellActivation = Activation.AllowEdit;
                    colBtnSelect.Header.VisiblePosition = 0;
                }

                UltraGridColumn colFromCurrency = grdForexRate.DisplayLayout.Bands[0].Columns["FromCurrencyID"];
                //colFromCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                //colFromCurrency.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colFromCurrency.ValueList = cmbCurrency;
                colFromCurrency.CellActivation = Activation.NoEdit;
                colFromCurrency.Header.Caption = "From Currency";
                colFromCurrency.Header.VisiblePosition = 1;

                UltraGridColumn colToCurrency = grdForexRate.DisplayLayout.Bands[0].Columns["ToCurrencyID"];
                //colToCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                //colToCurrency.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colToCurrency.ValueList = cmbCurrency;
                colToCurrency.CellActivation = Activation.NoEdit;
                colToCurrency.Header.Caption = "To Currency";
                colToCurrency.Header.VisiblePosition = 2;

                UltraGridColumn colAccountName = grdForexRate.DisplayLayout.Bands[0].Columns["FundName"];
                colAccountName.CharacterCasing = CharacterCasing.Upper;
                colAccountName.CellActivation = Activation.NoEdit;
                colAccountName.Header.Caption = "Account";
                colAccountName.Header.VisiblePosition = 3;

                UltraGridColumn colSummary = grdForexRate.DisplayLayout.Bands[0].Columns["Summary"];
                colSummary.CellActivation = Activation.NoEdit;
                colSummary.Width = 200;
                colSummary.Header.VisiblePosition = 4;

                UltraGridColumn colBloomberg = grdForexRate.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                colBloomberg.CharacterCasing = CharacterCasing.Upper;
                colBloomberg.CellActivation = Activation.NoEdit;
                colBloomberg.Header.VisiblePosition = 5;

                grdForexRate.DisplayLayout.Bands[0].Columns["FundID"].Hidden = true;
                grdForexRate.DisplayLayout.Bands[0].Columns["SourceID"].Hidden = true;
                grdForexRate.DisplayLayout.Bands[0].Columns["IsApproved"].Hidden = true;
                grdForexRate.DisplayLayout.Bands[0].Columns["CurrencyPairID"].Hidden = true;
                grdForexRate.DisplayLayout.Bands[0].Columns["Symbol"].Hidden = true;
                grdForexRate.DisplayLayout.Bands[0].Columns["FundID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grdForexRate.DisplayLayout.Bands[0].Columns["SourceID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grdForexRate.DisplayLayout.Bands[0].Columns["IsApproved"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grdForexRate.DisplayLayout.Bands[0].Columns["CurrencyPairID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grdForexRate.DisplayLayout.Bands[0].Columns["Symbol"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;

                UltraGridColumn colPricingSourceID = grdForexRate.DisplayLayout.Bands[0].Columns["SourceID"];
                colPricingSourceID.Header.Caption = "SourceID";
                colPricingSourceID.Header.Fixed = true;
                colPricingSourceID.CellActivation = Activation.NoEdit;

                if (grdForexRate.DisplayLayout.Bands[0].Columns.Exists("Comments"))
                {
                    UltraGridColumn colComments = grdForexRate.DisplayLayout.Bands[0].Columns["Comments"];
                    colComments.Header.Caption = "Comments";
                    colComments.Hidden = true;
                    colComments.CellActivation = Activation.NoEdit;
                }

                if (grdForexRate.DisplayLayout.Bands[0].Columns.Exists("PricingSource"))
                {
                    UltraGridColumn colPricingSource = grdForexRate.DisplayLayout.Bands[0].Columns["PricingSource"];
                    colPricingSource.Header.Caption = "PricingSource";
                    //colPricingSource.Hidden = true;
                    colPricingSource.CellActivation = Activation.NoEdit;
                    colPricingSource.Header.VisiblePosition = 6;
                }

                // load the layout file if it exists
                LoadReportSaveLayoutXML();

                if (grdForexRate.DisplayLayout.Bands[0].Columns.Exists("FromCurrencyID"))
                {
                    grdForexRate.DisplayLayout.Bands[0].Columns["FromCurrencyID"].ValueList = cmbCurrency;
                }

                if (grdForexRate.DisplayLayout.Bands[0].Columns.Exists("ToCurrencyID"))
                {
                    grdForexRate.DisplayLayout.Bands[0].Columns["ToCurrencyID"].ValueList = cmbCurrency;
                }

                foreach (UltraGridColumn col in grdForexRate.DisplayLayout.Bands[0].Columns)
                {
                    DateTime columndate;
                    if (DateTime.TryParse(col.Header.Caption.Trim(), out columndate))
                    {
                        col.MaxLength = _maxDigitsNumber;
                        col.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                        col.CellActivation = Activation.AllowEdit;
                        col.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                        col.MaskInput = "nnnnnnnnn.nnnnn";
                    }
                }

                grdForexRate.DisplayLayout.Override.FilterOperatorDefaultValue = FilterOperatorDefaultValue.Equals;
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
        /// Get the datatable representing the changes
        /// </summary>
        /// <returns>The datatable of changes</returns>
        public DataTable GetDataToSave()
        {
            try
            {
                if (grdForexRate.Rows.Count > 0)
                {

                    int rowIndex = 1;
                    bool isValidatedData = true;
                    string msgNAVlock = string.Empty;
                    foreach (DataRow dRow in _dtGridDataSource.Rows)
                    {
                        if (dRow.RowState != DataRowState.Unchanged)
                        {

                            foreach (DataColumn dCol in _dtGridDataSource.Columns)
                            {
                                DateTime date = new DateTime();
                                bool isParsed = DateTime.TryParse(dCol.ColumnName, out date);
                                if (isParsed)
                                {
                                    //If forex rate field is blank.
                                    if (!string.IsNullOrWhiteSpace(dRow["BloombergSymbol"].ToString()) && string.IsNullOrWhiteSpace(dRow[dCol.ColumnName].ToString()))
                                    {
                                        isValidatedData = false;
                                        InformationMessageBox.Display("Please enter the forex rate for Symbol '" + dRow["BloombergSymbol"], "Forex Rate Save");
                                        return null;
                                    }
                                    else
                                    {
                                        int accountID = Convert.ToInt32(dRow["FundID"].ToString());
                                        //string accountName = dRow["FundName"].ToString();
                                        bool isNavLocked = NAVLockManager.GetInstance.ValidateTrade(accountID, date);//check NAV nock
                                        if (!isNavLocked)
                                        {
                                            msgNAVlock = "NAV is locked for some account(s).\nModification in forex rates allowed only for valid accounts.";
                                            // if locked after showing message.
                                            //MessageBox.Show("NAV is locked for the account: " + accountName + "\nPlease Select Valid accounts for modification in forex rates", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            //return null;
                                        }
                                    }
                                }
                            }

                        }
                        rowIndex++;
                        if ((dRow.RowState == DataRowState.Unchanged && _isDataCopiedFromBackDate) && (!dRow[5].ToString().Equals("0")))
                            dRow.SetModified();
                    }

                    if (!string.IsNullOrWhiteSpace(msgNAVlock))
                    {
                        MessageBox.Show(msgNAVlock, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (isValidatedData.Equals(true))
                    {
                        DataTable dtChanges = ((DataTable)grdForexRate.DataSource).GetChanges();
                        _dtGridDataSource.AcceptChanges();
                        return dtChanges;
                    }
                    else
                    {
                        return null;
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
            return null;
        }

        private bool CheckforAccountLocked(DataTable ChangedData)
        {
            try
            {
                StringBuilder errMsg = new StringBuilder();
                List<int> newAccountsToBelocked = new List<int>();
                foreach (DataRow dRow in ChangedData.Rows)
                {
                    string accountName = dRow["FundName"].ToString();
                    int accountID = Convert.ToInt32(dRow["FundID"].ToString());
                    //Account Lock error should not be given if account id is not retrieved
                    if (!CachedDataManager.GetInstance.isAccountLocked(accountID) && accountID != int.MinValue && !errMsg.ToString().Contains(accountName))
                    {
                        //add Account Name in Error message if Account is not locked
                        errMsg.Append(", ").Append(accountName);
                        newAccountsToBelocked.Add(accountID);
                    }
                }

                if (errMsg.Length != 0)
                {
                    if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking following accounts " + errMsg.ToString().Substring(1) + ".", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                        if (AccountLockManager.SetAccountsLockStatus(newAccountsToBelocked))
                        {
                            MessageBox.Show("The lock for accounts has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Accounts are currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //set active cell to null so that it cannot be modified
                            return false;
                        }
                    }
                    else
                    {
                        return false;
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
            return true;
        }

        /// <summary>
        /// validate the end date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            //DateTime endDate = DateTime.Parse(dtEndDate.Value.ToString());
            //DateTime startDate = DateTime.Parse(dtStartDate.Value.ToString());
            //if (endDate > startDate.AddMonths(1))
            //{
            //    MessageBox.Show("End Date cannot be more than one month ahead of start date", "Prana Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    dtEndDate.Value = DateTime.Today;
            //    //dtEndDate.Value = DateTime.Today;
            //}
        }

        /// <summary>
        /// Action to be performed on change of client from the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucbClient_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                clearGrdForexRateDataSource();
                if (!string.IsNullOrEmpty(ucbClient.SelectedRow.Cells["Value"].Text))
                {
                    int companyID = Convert.ToInt32(ucbClient.SelectedRow.Cells["Value"].Text);
                    if (companyID == -1)
                    {
                        ucbAccount.Enabled = false;
                        return;
                    }
                    ucbAccount.Enabled = true;
                    ucbAccount.Text = string.Empty;
                    Dictionary<int, string> userAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                    Dictionary<int, string> userAccountsForSelectedCompany = new Dictionary<int, string>();
                    foreach (Account account in CachedDataManager.GetInstance.CompanyAccountsMapping[companyID])
                    {
                        if (userAccounts.ContainsKey(account.AccountID) && !userAccountsForSelectedCompany.ContainsKey(account.AccountID))
                        {
                            userAccountsForSelectedCompany.Add(account.AccountID, account.FullName);
                        }
                    }
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-2288
                    Dictionary<int, string> sortedAccounts = GeneralUtilities.SortDictionaryByValues<int, string>(userAccountsForSelectedCompany);
                    ucbAccount.DataSource = sortedAccounts;
                    ucbAccount.DisplayLayout.Bands[0].Columns["Key"].Hidden = true;

                    SetAccountControl();

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
        /// Get the comma separated IDs of the accounts
        /// </summary>
        /// <returns>The string holding the Comma Separated IDs of the selected accounts</returns>
        private String GetAccountIDs()
        {
            String accountID = string.Empty;
            DataTable dtAccount = new DataTable("dtAccount");
            DataSet dsAccount = new DataSet("dsAccount");
            dtAccount.Columns.Add("FundID", typeof(int));

            try
            {
                foreach (UltraGridRow row in ucbAccount.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["Selected"].Value))
                    {
                        if (!string.IsNullOrEmpty(row.Cells["Key"].Value.ToString()))
                        {
                            int account = Convert.ToInt32(row.Cells["Key"].Value);
                            dtAccount.Rows.Add(account);
                        }
                    }
                }
                dsAccount.Tables.Add(dtAccount);
                accountID = dsAccount.GetXml();
                return accountID;
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
            return null;
        }

        /// <summary>
        /// Get the Pricing details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetPrice_Click(object sender, EventArgs e)
        {
            try
            {
                SetControlsOnFetchAPIClick(false);
                _requestedSymbols.Clear();
                if (!ucbAccount.Enabled || string.IsNullOrWhiteSpace(ucbAccount.Text))
                {
                    MessageBox.Show("Select Client and at least one account to show the data", "Forex Rate Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string xmlAccounts = GetAccountIDs();

                _isDataCopiedFromBackDate = false;
                _dateSelected = (DateTime)dtStartDate.Value;
                _startDate = (DateTime)dtStartDate.Value;
                int filterValue = Convert.ToInt32(cmbFilter.Value.ToString());
                _filter = (filterValue >= 0) ? filterValue : 0;
                _endDate = (DateTime)dtEndDate.Value;

                BindGridWithData(xmlAccounts, _filter);
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
        /// Fill the grid with the data
        /// </summary>
        /// <param name="xmlAccounts">XML document containing the IDs of the selected accounts</param>
        /// <param name="filter">0 for all, 1 for missing rates, 2 for manual source</param>
        private void BindGridWithData(string xmlAccounts, int filter)
        {
            try
            {
                clearGrdForexRateDataSource();
                grdForexRate.DataBind();
                DataTable dtOriginal = new DataTable();
                int methodology = (int)_methodologySelected;

                dtOriginal = BindGridForForexConversion(xmlAccounts, _startDate, _endDate, methodology, filter);

                // Purpose : To show comments for non valid symbols in case prices not fetched from API.
                if (!dtOriginal.Columns.Contains("Comments"))
                {
                    DataColumn colComments = new DataColumn("Comments", typeof(System.String));
                    colComments.DefaultValue = string.Empty;
                    dtOriginal.Columns.Add(colComments);
                }

                if (!dtOriginal.Columns.Contains("Select"))
                {
                    DataColumn colComments = new DataColumn("Select", typeof(System.Boolean));
                    colComments.DefaultValue = false;
                    dtOriginal.Columns.Add(colComments);
                }

                grdForexRate.DataSource = dtOriginal;
                UltraGridRow[] FilteredRows = grdForexRate.Rows.GetFilteredInNonGroupByRows();
                List<String> symbols = new List<string>();
                foreach (UltraGridRow row in FilteredRows)
                {
                    if (!symbols.Contains(row.Cells["BloombergSymbol"].Value.ToString()))
                    {
                        symbols.Add(row.Cells["BloombergSymbol"].Value.ToString());
                    }
                }
                //if (dtOriginal != null)
                //{
                //    DataTable dtClone = dtOriginal.Clone();

                //    foreach (DataRow dr in dtOriginal.Rows)
                //    {
                //        if (symbols.Contains(dr["Symbol"].ToString()))
                //        {
                //            dtClone.ImportRow(dr);
                //        }
                //    }
                //    ConvertBlankNumericValues(dtClone);
                //    DataTable dtFinalCopied = (DataTable)grdForexRate.DataSource;

                //    dtFinalCopied.AcceptChanges();
                //    grdForexRate.DataSource = dtFinalCopied;

                //    _dtGridDataSource = dtFinalCopied;

                //}
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
        /// Handle the null values received from the database
        /// </summary>
        /// <param name="dtNum">Datasource of the grid</param>
        //private void ConvertBlankNumericValues(DataTable dtNum)
        //{
        //    try
        //    {
        //        if (dtNum != null && dtNum.Columns.Count > 0)
        //        {
        //            foreach (DataRow dr in dtNum.Rows)
        //            {
        //                foreach (DataColumn dCol in dtNum.Columns)
        //                {
        //                    if ((dCol.DataType == typeof(System.Single)
        //                       || dCol.DataType == typeof(System.Double)
        //                       || dCol.DataType == typeof(System.Decimal)
        //                       || dCol.DataType == typeof(System.Byte)
        //                       || dCol.DataType == typeof(System.Int16)
        //                       || dCol.DataType == typeof(System.Int32)
        //                       || dCol.DataType == typeof(System.Int64))
        //                       && (dr[dCol].ToString().Equals("0") || string.IsNullOrWhiteSpace(dr[dCol].ToString())))
        //                    {
        //                        dr[dCol] = 0;
        //                    }
        //                }

        //                if (dtNum.Columns.Contains("BloombergSymbol"))
        //                {

        //                    if (!dr["BloombergSymbol"].ToString().ToUpper().Equals(dr["BloombergSymbol"].ToString()))
        //                    {
        //                        dr["BloombergSymbol"] = dr["BloombergSymbol"].ToString().ToUpper();
        //                    }
        //                }
        //                dr.AcceptChanges();
        //            }
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


        //private void ResetFilterAfterRebindingGrid()
        //{
        //    try
        //    {
        //        //txtSymbolFilteration.Text = string.Empty;
        //        //SetExchangeGroupFilteration();
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

        /// <summary>
        /// Method to save the forex rate details
        /// </summary>
        /// <param name="dtDataToSave">Datatable holding the data</param>
        /// <returns>Number of rows affected</returns>
        internal int SaveForexRate(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtForexRates = null;
                dtForexRates = dtDataToSave;
                if (dtForexRates != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtForexRatesNew = new DataTable();
                    dtForexRatesNew.TableName = "ForexRateImport";
                    //dtForexRatesNew.Columns.Add(new DataColumn("ForexRateImportType"));
                    dtForexRatesNew.Columns.Add(new DataColumn("CurrencyPairID"));
                    dtForexRatesNew.Columns.Add(new DataColumn("ConversionType"));
                    dtForexRatesNew.Columns.Add(new DataColumn("Date"));
                    dtForexRatesNew.Columns.Add(new DataColumn("ConversionFactor"));
                    dtForexRatesNew.Columns.Add(new DataColumn("BloombergSymbol"));

                    //Added AUECID as it will be used at pricing server end to update cache
                    //dtForexRatesNew.Columns.Add(new DataColumn("AUECID"));
                    ///dtForexRatesNew.Columns.Add(new DataColumn("AUECIdentifier"));
                    dtForexRatesNew.Columns.Add(new DataColumn("FundID"));
                    dtForexRatesNew.Columns.Add(new DataColumn("SourceID"));
                    dtForexRatesNew.Columns.Add(new DataColumn("IsApproved"));

                    foreach (DataRow dr in dtForexRates.Rows)
                    {
                        //Assigning the row having symbol being not blank.
                        if (!string.IsNullOrWhiteSpace(dr["BloombergSymbol"].ToString()))
                        {
                            foreach (DataColumn dc in dtForexRates.Columns)
                            {
                                DateTime date = new DateTime();
                                bool isDateParsed = DateTime.TryParse(dc.ColumnName, out date);
                                if (isDateParsed)
                                {
                                    double forexRate;
                                    double.TryParse(dr[dc.ColumnName].ToString(), out forexRate);
                                    //only non-zero forex rates will be saved.  
                                    if (forexRate != 0)
                                    {
                                        DataRow drNew = dtForexRatesNew.NewRow();

                                        drNew["IsApproved"] = 1;
                                        drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);
                                        drNew["CurrencyPairID"] = dr["CurrencyPairID"];
                                        drNew["ConversionFactor"] = forexRate;
                                        // this column value has been fixed to differentiate whether data save into the DB from Import module or Mark price UI
                                        // L stands for Live feed Data
                                        //drNew["ForexRateImportType"] = Prana.BusinessObjects.AppConstants.MarkPriceImportType.L.ToString();

                                        drNew["BloombergSymbol"] = dr["BloombergSymbol"].ToString().ToUpper();
                                        drNew["FundID"] = Convert.ToInt32(dr["AccountID"].ToString());
                                        if (dr["SourceID"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["PricingSource"].ToString()))
                                            drNew["SourceID"] = (int)Enum.Parse(typeof(PricingSource), dr["PricingSource"].ToString());
                                        else
                                            drNew["SourceID"] = 1; //TODO check why source is null or blank

                                        //Added AUECID as it will be used at pricing server end to update cache
                                        //if (dtForexRates.Columns.Contains("AUECID") && dr["AUECID"] != System.DBNull.Value)
                                        //    drNew["AUECID"] = dr["AUECID"].ToString().ToUpper();

                                        //if (dtForexRates.Columns.Contains("AUECIdentifier") && dr["AUECIdentifier"] != System.DBNull.Value)
                                        //    drNew["AUECIdentifier"] = dr["AUECIdentifier"].ToString();

                                        dtForexRatesNew.Rows.Add(drNew);
                                        dtForexRatesNew.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }
                    if (dtForexRatesNew != null && dtForexRatesNew.Rows.Count > 0)
                    {
                        rowsAffected = _pricingServicesProxy.InnerChannel.SaveForexRate(dtForexRatesNew);
                        //if (rowsAffected > 0)
                        //{
                        //    _isDataCopiedFromBackDate = false;
                        //    CashManagementServices.InnerChannel.UpdateDayEndBaseCashByForexRate(dtForexRatesNew);
                        //    //ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).UpdateForexConversionCache(dtForexConversionTemp);
                        //}
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
            _isDataCopiedFromBackDate = false;
            return rowsAffected;
        }

        //private DataTable GetTableFromSymbolMarkPriceDictDateRange(Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> accountSymbolMarkPriceInfo)
        //{
        //    //_isForwardPointsRecalculated = false;

        //    DataTable dt = null;
        //    try
        //    {

        //        if (accountSymbolMarkPriceInfo != null)
        //        {
        //            dt = new DataTable();
        //            dt.Columns.Add("Select", typeof(System.Boolean));
        //            dt.Columns["Select"].DefaultValue = false;
        //            dt.Columns.Add("Symbol", typeof(System.String));
        //            dt.Columns.Add("AUECID", typeof(System.Int32));
        //            dt.Columns.Add("AUECIdentifier", typeof(System.String));
        //            dt.Columns.Add("ForwardPoints", typeof(System.Double));
        //            dt.Columns.Add("FxRate", typeof(System.Double));
        //            dt.Columns.Add("BloombergSymbol", typeof(System.String));
        //            dt.Columns.Add("AccountID", typeof(System.String));
        //            dt.Columns.Add("SourceID", typeof(System.String));
        //            dt.Columns.Add("AccountName", typeof(System.String));
        //            dt.Columns.Add("ISINSymbol", typeof(System.String));
        //            dt.Columns.Add("CUSIPSymbol", typeof(System.String));
        //            dt.Columns.Add("Currency", typeof(System.String));
        //            dt.Columns.Add("ExpirationDate", typeof(System.DateTime));
        //            dt.PrimaryKey = new DataColumn[] { dt.Columns["BloombergSymbol"], dt.Columns["AccountID"] };
        //        }
        //        else
        //        {
        //            return dt;
        //        }

        //        foreach (DateTime markPriceDate in accountSymbolMarkPriceInfo.Keys)
        //        {
        //            Dictionary<int, Dictionary<string, MarkPriceInfo>> symbolMarkPriceInfo = accountSymbolMarkPriceInfo[markPriceDate];
        //            foreach (int AccountId in symbolMarkPriceInfo.Keys)
        //            {
        //                string date;
        //                if (_methodologySelected.Equals(MethdologySelected.Daily))
        //                    date = ((DateTime)dtStartDate.Value).ToString("MM/dd/yyyy");
        //                else
        //                    date = markPriceDate.Date.ToString("MM/dd/yyyy");
        //                // Here date column contain values of markprices, hence double.
        //                if (!dt.Columns.Contains(date))
        //                {
        //                    dt.Columns.Add(date, typeof(System.Double));
        //                    //string approvalStatus=
        //                    //dt.Columns.Add()
        //                }
        //                Dictionary<string, MarkPriceInfo> symbolMarkInfo = symbolMarkPriceInfo[AccountId];

        //                foreach (KeyValuePair<string, MarkPriceInfo> symbolMarkKeyValue in symbolMarkInfo)
        //                {
        //                    Object[] objArray = { symbolMarkKeyValue.Key, symbolMarkKeyValue.Value.AccountID };
        //                    DataRow symbolRow = dt.Rows.Find(objArray);
        //                    if (symbolRow == null)
        //                    {
        //                        symbolRow = dt.NewRow();
        //                        symbolRow["AccountName"] = symbolMarkKeyValue.Value.AccountName;
        //                        symbolRow["Symbol"] = symbolMarkKeyValue.Key;
        //                        symbolRow["AUECID"] = symbolMarkKeyValue.Value.AUECID;
        //                        symbolRow["AUECIdentifier"] = symbolMarkKeyValue.Value.AUECIdentifier;
        //                        symbolRow["FxRate"] = symbolMarkKeyValue.Value.FxRate;
        //                        symbolRow["BloombergSymbol"] = symbolMarkKeyValue.Value.BloombergSymbol;
        //                        symbolRow["AccountID"] = symbolMarkKeyValue.Value.AccountID;
        //                        symbolRow["SourceID"] = symbolMarkKeyValue.Value.PricingSource;
        //                        symbolRow["ISINSymbol"] = symbolMarkKeyValue.Value.ISINSymbol;
        //                        symbolRow["CUSIPSymbol"] = symbolMarkKeyValue.Value.CUSIPSymbol;
        //                        symbolRow["Currency"] = symbolMarkKeyValue.Value.Currency;
        //                        symbolRow["ExpirationDate"] = symbolMarkKeyValue.Value.ExpirationDate;
        //                        dt.Rows.Add(symbolRow);
        //                    }
        //                    else
        //                    {

        //                    }


        //                    if (markPriceDate.Date.Equals(symbolMarkKeyValue.Value.DateActual.Date))
        //                    {
        //                        symbolRow[date] = symbolMarkKeyValue.Value.MarkPrice;
        //                        double forwardPoints = symbolMarkKeyValue.Value.ForwardPoints;
        //                        symbolRow["ForwardPoints"] = forwardPoints;
        //                    }
        //                    else
        //                    {
        //                        symbolRow[date] = 0;
        //                        symbolRow["ForwardPoints"] = 0;
        //                    }
        //                }
        //            }
        //        }
        //        dt.AcceptChanges();
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
        //    return dt;
        //}

        private enum MethdologySelected
        {
            Daily = 0,
            Weekly = 1,
            Monthly = 2,
            DateRange = 3
        }

        public delegate void L1ObjHandler(SymbolData level1Data);
        public delegate void L1ListObjHandler(List<SymbolData> level1Data);

        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        #region IPublishing Members

        public void Publish(MessageData e, string topicName)
        {
            try
            {

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

        //public string getReceiverUniqueName()
        //{
        //    return "ctrlMarkPriceAppend";
        //}

        #endregion

        public void LiveFeedConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void LiveFeedDisConnected()
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Check if the Daily radio button is selected
        /// 
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optDaily_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                clearGrdForexRateDataSource();
                dtStartDate.MaskInput = "{LOC}mm/dd/yyyy";
                dtEndDate.Visible = true;
                lblEndDate.Visible = true;
                if (optDaily.Checked)
                {
                    _methodologySelected = MethdologySelected.Daily;
                    if (dtEndDate.Enabled)
                    {
                        dtEndDate.Enabled = false;
                    }
                    lblCopy.Visible = true;
                    dtCopyDate.Visible = true;
                    ubtnCopyForexRate.Visible = true;
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
        /// Check if the month radio button is selected
        /// 
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optMonthly_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                clearGrdForexRateDataSource();
                dtStartDate.MaskInput = "{LOC}mm/yyyy";
                dtEndDate.Visible = false;
                lblEndDate.Visible = false;
                if (optMonthly.Checked)
                {
                    _methodologySelected = MethdologySelected.Monthly;

                    // TODO : Need to remove after implementation of copy mark price for monthly & custom mode.
                    lblCopy.Visible = false;
                    dtCopyDate.Visible = false;
                    ubtnCopyForexRate.Visible = false;
                }
                if (dtEndDate.Enabled)
                {
                    dtEndDate.Enabled = false;
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
        /// Check if the Custom radio button is selected
        /// 
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void optCustom_CheckedChanged(object sender, EventArgs e)
        {

            try
            {
                clearGrdForexRateDataSource();
                dtStartDate.MaskInput = "{LOC}mm/dd/yyyy";
                dtEndDate.Visible = true;
                lblEndDate.Visible = true;
                if (optCustom.Checked)
                {
                    _methodologySelected = MethdologySelected.DateRange;
                    lblCopy.Visible = false;
                    dtCopyDate.Visible = false;
                    ubtnCopyForexRate.Visible = false;
                }
                if (!dtEndDate.Enabled)
                {
                    dtEndDate.Enabled = true;
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

        private void dtEndDate_Leave(object sender, EventArgs e)
        {

        }

        private void dtEndDate_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                DateTime endDate = DateTime.Parse(dtEndDate.Value.ToString());
                DateTime startDate = DateTime.Parse(dtStartDate.Value.ToString());
                clearGrdForexRateDataSource();
                if (optDaily.Checked)
                {
                    // To handle future date in start date
                    if (startDate > DateTime.Today)
                    {
                        MessageBox.Show("Start Date cannot be a future date", "Mark Price Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtStartDate.Value = DateTime.Today;
                    }
                }
                else if (optMonthly.Checked)
                {
                    if (startDate > DateTime.Today)
                    {
                        MessageBox.Show("Month cannot be a future month", "Mark Price Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtStartDate.Value = DateTime.Today;
                        dtEndDate.Value = DateTime.Today;
                    }
                }
                else if (optCustom.Checked)
                {
                    if (endDate > startDate.AddMonths(1))
                    {
                        MessageBox.Show("End Date cannot be more than one month ahead of start date", "Mark Price Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtEndDate.Value = dtStartDate.Value;
                    }

                    // // To handle future date in start date
                    else if (startDate > DateTime.Today)
                    {
                        MessageBox.Show("Start Date cannot be a future date", "Mark Price Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtStartDate.Value = DateTime.Today;
                    }

                    // To handle future date in end date
                    else if (endDate > DateTime.Today)
                    {
                        MessageBox.Show("End Date cannot be a future date", "Mark Price Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtEndDate.Value = DateTime.Today;
                    }

                    else if (endDate < startDate)
                    {
                        MessageBox.Show("End Date cannot be less than the start date", "Mark Price Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtEndDate.Value = dtStartDate.Value;
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
        private ISecurityMasterServices _securityMaster;
        public ISecurityMasterServices SecurityMaster
        {
            get
            {
                return _securityMaster;
            }

            set
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    _securityMaster = value;
                    ClientPricingManager.GetInstance.SecurityMasterServices = _securityMaster;
                    WireEvents();
                }
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlSetForex_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    if (optDaily.Checked)
                    {
                        _methodologySelected = MethdologySelected.Daily;
                        if (dtEndDate.Enabled)
                        {
                            dtEndDate.Enabled = false;
                        }
                    }
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                ubtnCopyForexRate.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ubtnCopyForexRate.ForeColor = System.Drawing.Color.White;
                ubtnCopyForexRate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ubtnCopyForexRate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ubtnCopyForexRate.UseAppStyling = false;
                ubtnCopyForexRate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetPrice.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetPrice.ForeColor = System.Drawing.Color.White;
                btnGetPrice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetPrice.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetPrice.UseAppStyling = false;
                btnGetPrice.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExportExcel.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExportExcel.ForeColor = System.Drawing.Color.White;
                btnExportExcel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExportExcel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExportExcel.UseAppStyling = false;
                btnExportExcel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnFetchFromAPI.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnFetchFromAPI.ForeColor = System.Drawing.Color.White;
                btnFetchFromAPI.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnFetchFromAPI.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnFetchFromAPI.UseAppStyling = false;
                btnFetchFromAPI.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnImport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnImport.ForeColor = System.Drawing.Color.White;
                btnImport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnImport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnImport.UseAppStyling = false;
                btnImport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSaveLayout.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSaveLayout.ForeColor = System.Drawing.Color.White;
                btnSaveLayout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSaveLayout.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSaveLayout.UseAppStyling = false;
                btnSaveLayout.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// Check if the trade being edited is having its accounts locked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdForexRate_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                if (e.Cell.GetType() == typeof(UltraGridFilterCell))
                {
                    return;
                }
                if (!e.Cell.Row.Cells.Exists(ReconConstants.COLUMN_AccountName))
                {
                    return;
                }
                string rowAccountname = e.Cell.Row.Cells[ReconConstants.COLUMN_AccountName].Text;
                int accountID = CachedDataManager.GetInstance.GetAccountID(rowAccountname);
                if (!CachedDataManager.GetInstance.isAccountLocked(accountID) && accountID != int.MinValue)
                {
                    //don't check for account lock when header is checked
                    if (_isHeaderCheckBoxChecked)
                    {
                        e.Cancel = true;
                        return;
                    }
                    if (!_alreadyPromptedAccountsForLock.Contains(accountID))
                    {
                        if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking " + rowAccountname + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            List<int> newAccountsToBelocked = new List<int>();
                            newAccountsToBelocked.Add(accountID);
                            newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                            if (ReconUtilities.SetAccountsLockStatus(newAccountsToBelocked))
                            {
                                MessageBox.Show("The lock for " + rowAccountname + " has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                _alreadyPromptedAccountsForLock.Add(accountID);
                                MessageBox.Show(rowAccountname + " is currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //cancel the update
                                e.Cancel = true;
                            }

                        }
                        else
                        {
                            _alreadyPromptedAccountsForLock.Add(accountID);
                            // user clicked no
                            //cancel the update
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        //cancel the update
                        e.Cancel = true;
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
        /// Account lock implementation on header checkbox  click of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdForexRate_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                _isHeaderCheckBoxChecked = false;
                if (grdForexRate.Rows.Count > 0 && grdForexRate.Rows.GetFilteredOutNonGroupByRows() != null)
                {
                    CheckState state = grdForexRate.DisplayLayout.Bands[0].Columns["Select"].GetHeaderCheckedState(grdForexRate.Rows);
                    UltraGridRow[] grdrows = grdForexRate.Rows.GetFilteredOutNonGroupByRows();
                    if (grdrows.Length > 0 && grdForexRate.Rows.Count > 0)
                    {
                        foreach (UltraGridRow row in grdrows)
                        {
                            if (state.Equals(CheckState.Checked))
                            {
                                if (row.Cells != null)
                                {
                                    row.Cells["Select"].Value = false;
                                }
                            }
                        }
                    }
                    foreach (UltraGridRow row in _selectedColumnList)
                    {
                        if (row.Cells != null && row.Cells.Exists("Select"))
                        {
                            row.Cells["Select"].Value = true;
                        }
                    }
                    if (state == CheckState.Unchecked)
                    {
                        foreach (UltraGridRow row in grdForexRate.Rows)
                        {
                            if (row.Cells != null)
                            {
                                row.Cells["Select"].Value = false;
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

        /// <summary>
        /// Account lock implementation on header checkbox  click of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdForexRate_BeforeHeaderCheckStateChanged(object sender, BeforeHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                _isHeaderCheckBoxChecked = true;
                if (e.NewCheckState == CheckState.Checked)
                {
                    if (grdForexRate.DataSource != null)
                    {
                        DataTable dt = (DataTable)grdForexRate.DataSource;
                        if (dt != null && dt.Columns.Count > 0 && dt.Columns.Contains(ReconConstants.COLUMN_AccountName))
                        {
                            List<string> lstAccountName = (from DataRow dr in dt.Rows select (string)dr[ReconConstants.COLUMN_AccountName]).Distinct().ToList();
                            List<string> accountUnlocked = new List<string>();
                            List<int> accountIDUnlocked = new List<int>();
                            lstAccountName.ForEach(x =>
                            {
                                int accountID = CachedDataManager.GetInstance.GetAccountID(x);
                                if (!CachedDataManager.GetInstance.isAccountLocked(accountID) && accountID != int.MinValue)
                                {
                                    accountIDUnlocked.Add(accountID);
                                    accountUnlocked.Add(x);
                                }
                            });

                            if (accountIDUnlocked.Count > 0)
                            {
                                if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking following accounts " + string.Join(Seperators.SEPERATOR_8, accountUnlocked.ToArray()) + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    List<int> newAccountsToBelocked = new List<int>();
                                    newAccountsToBelocked.AddRange(accountIDUnlocked);
                                    newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                                    if (ReconUtilities.SetAccountsLockStatus(newAccountsToBelocked))
                                    {
                                        MessageBox.Show("The lock for accounts has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        //update Locks in cache
                                        CachedDataManager.GetInstance.SetLockedAccounts(newAccountsToBelocked);
                                    }
                                    else
                                    {
                                        e.Cancel = true;
                                        _isHeaderCheckBoxChecked = false;
                                        MessageBox.Show("Accounts are currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    e.Cancel = true;
                                    _isHeaderCheckBoxChecked = false;

                                }
                            }
                        }
                    }
                    if (grdForexRate.Rows.Count > 0)
                    {
                        _selectedColumnList.Clear();
                        foreach (UltraGridRow row in grdForexRate.Rows)
                        {
                            if (row.Cells != null && Convert.ToBoolean(row.Cells["Select"].Value) == true)
                            {
                                _selectedColumnList.Add(row);
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

        private void grdForexRate_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (!e.Cell.Row.Cells.Exists(ReconConstants.COLUMN_AccountName) || e.Cell.GetType() == typeof(UltraGridFilterCell))
                {
                    return;
                }
                DateTime selectedColumnDate = new DateTime();
                bool isParsed = DateTime.TryParse(e.Cell.Column.Key, out selectedColumnDate);
                if (isParsed)
                {
                    string rowAccountname = e.Cell.Row.Cells[ReconConstants.COLUMN_AccountName].Text;
                    int accountID = CachedDataManager.GetInstance.GetAccountID(rowAccountname);
                    if (!NAVLockManager.GetInstance.ValidateTrade(accountID, Convert.ToDateTime(selectedColumnDate)))
                    {
                        MessageBox.Show("The NAV for " + rowAccountname + " for this Date is Locked.\nPlease contact System Administrator for making any changes!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cell.CancelUpdate();
                        return;
                    }
                    e.Cell.Row.Cells["PricingSource"].Value = PricingSource.UserDefined.ToString();
                    e.Cell.Row.Cells["Comments"].Value = string.Empty;
                    #region To update summary for daily mode

                    string fromCurrency = e.Cell.Row.Cells[ReconConstants.COLUMN_FromCurrencyID].Text;
                    string toCurrency = e.Cell.Row.Cells[ReconConstants.COLUMN_ToCurrencyID].Text;
                    string colDate = selectedColumnDate.ToString("MM/dd/yyyy");

                    if (_methodologySelected == MethdologySelected.Daily)
                    {
                        if ((!String.IsNullOrEmpty(fromCurrency) && !fromCurrency.Equals(ApplicationConstants.C_COMBO_SELECT)) && (!String.IsNullOrEmpty(toCurrency) && !toCurrency.Equals(ApplicationConstants.C_COMBO_SELECT)))
                        {
                            if (e.Cell.Row.Cells.Exists(colDate))
                            {
                                if (e.Cell.Row.Cells[colDate].Value != System.DBNull.Value && Convert.ToDouble(e.Cell.Row.Cells[colDate].Value) > 0)
                                {
                                    e.Cell.Row.Cells[ReconConstants.COLUMN_Summary].Value = "1 " + fromCurrency + "= " + Convert.ToDouble(e.Cell.Row.Cells[colDate].Value) + toCurrency;
                                }
                            }
                        }
                        else
                        {
                            e.Cell.Row.Cells[ReconConstants.COLUMN_Summary].Value = string.Empty;
                        }
                    }
                    #endregion
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
        /// Wires the Event
        /// </summary>
        public void WireEvents()
        {
            try
            {
                _securityMaster.CentralSMDisconnected += _securityMaster_CentralSMDisconnected;
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
        /// UnWires the event
        /// </summary>
        private void UnwireEvents()
        {
            try
            {
                if (_securityMaster != null)
                    _securityMaster.CentralSMDisconnected -= _securityMaster_CentralSMDisconnected;
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
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnwireEvents();
                if (components != null)
                {
                    components.Dispose();
                    if (!CustomThemeHelper.IsDesignMode())
                    {
                        _pricingServicesProxy.Dispose();
                    }
                }
                if (_dtGridDataSource != null)
                {
                    _dtGridDataSource.Dispose();
                }
                if (cmbCurrency != null)
                {
                    cmbCurrency.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Filter cell value changed for grdMarkPrice.
        /// Added by Ankit Gupta on 30 Oct, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1328
        /// Filtering grouped data should be smooth.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdForexRate_FilterCellValueChanged(object sender, FilterCellValueChangedEventArgs e)
        {
            try
            {
                if (UltraWinGridUtils.IsGrouppingAppliedOnGrid(grdForexRate))
                {
                    String filterCondition = e.FilterCell.Text;
                    grdForexRate.Rows.ColumnFilters[e.FilterCell.Column.Key].FilterConditions.Clear();
                    grdForexRate.Rows.ColumnFilters[e.FilterCell.Column.Key].FilterConditions.Add(new FilterCondition(FilterComparisionOperator.StartsWith, filterCondition));
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
        /// To save layout of MarkPrice grid as MarkPriceAppendLayout xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdForexRate != null)
                {
                    if (grdForexRate.DisplayLayout.Bands[0].Columns.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(_forexRateFilePath))
                        {
                            grdForexRate.DisplayLayout.SaveAsXml(_forexRateFilePath);
                        }
                    }
                }
                MessageBox.Show("Forex rate layout saved", "Forex Rate", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Load report layout xml if file exist
        /// </summary>
        private void LoadReportSaveLayoutXML()
        {
            try
            {
                _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _forexRateLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID;
                _forexRateFilePath = _forexRateLayoutDirectoryPath + @"\ManageForexRateLayout.xml";

                if (!Directory.Exists(_forexRateLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_forexRateLayoutDirectoryPath);
                }
                if (File.Exists(_forexRateFilePath))
                {
                    grdForexRate.DisplayLayout.LoadFromXml(_forexRateFilePath);
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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ubtnCopyMarkPrice_Click(object sender, EventArgs e)
        {
            try
            {
                _startDate = (DateTime)dtStartDate.Value;
                if (_startDate != (DateTime)dtCopyDate.Value)
                {
                    _requestedSymbols.Clear();
                    if (!ucbAccount.Enabled || string.IsNullOrWhiteSpace(ucbAccount.Text))
                    {
                        MessageBox.Show("Select Client and at least one account to Copy the data", "Forex Rate Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    DialogResult dlgResultCopy = MessageBox.Show("Do you want copy the data from " + dtCopyDate.Value.ToString() + " for " + _startDate.ToString("MM/dd/yyyy") + "?", "CONFIRMATION", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlgResultCopy.Equals(DialogResult.Yes))
                    {
                        string xmlAccounts = GetAccountIDs();

                        _isDataCopiedFromBackDate = true;
                        int filterValue = Convert.ToInt32(cmbFilter.Value.ToString());
                        _filter = (filterValue >= 0) ? filterValue : 0;
                        _startDate = _dateSelected = (DateTime)dtCopyDate.Value;
                        BindGridWithData(xmlAccounts, _filter);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("SELECT DATE and COPY FROM dates are equal, Please select different dates", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Instead of LOGANDTHROW I have replaced to LOGANDSHOW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtStartDate_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                clearGrdForexRateDataSource();
                _startDate = (DateTime)dtStartDate.Value;
                DateTime endDate = DateTime.Parse(dtEndDate.Value.ToString());
                DateTime startDate = DateTime.Parse(dtStartDate.Value.ToString());
                // Modified by Ankit Gupta on 7 Oct, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1581
                // End date error is displayed, but it is not editable.

                if (optDaily.Checked)
                {
                    // To handle future date in start date
                    if (startDate > DateTime.Today)
                    {
                        MessageBox.Show("Start Date cannot be a future date", "Forex Rate Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtStartDate.Value = DateTime.Today;
                    }
                }
                else if (optMonthly.Checked)
                {
                    if (startDate > DateTime.Today)
                    {
                        MessageBox.Show("Month cannot be a future month", "Forex Rate Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtStartDate.Value = DateTime.Today;
                        dtEndDate.Value = DateTime.Today;
                    }
                }
                else if (optCustom.Checked)
                {
                    if (endDate > startDate.AddMonths(1))
                    {
                        MessageBox.Show("End Date cannot be more than one month ahead of start date", "Forex Rate Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //dtStartDate.Value = DateTime.Today;
                        dtEndDate.Value = dtStartDate.Value;
                    }

                    // // To handle future date in start date
                    else if (startDate > DateTime.Today)
                    {
                        MessageBox.Show("Start Date cannot be a future date", "Forex Rate Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtStartDate.Value = DateTime.Today;
                    }

                    // To handle future date in end date
                    else if (endDate > DateTime.Today)
                    {
                        MessageBox.Show("End Date cannot be a future date", "Forex Rate Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtEndDate.Value = DateTime.Today;
                    }

                    else if (endDate < startDate)
                    {
                        MessageBox.Show("End Date cannot be less than the start date", "Forex Rate Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dtEndDate.Value = dtStartDate.Value;
                        //dtStartDate.Value = DateTime.Today;
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
        ///Added by sachin mishra 13/04/15 
        ///Purpose: For clearing the datasource of the grdForexRate JIRA-CHMW-3378
        /// </summary>
        public void clearGrdForexRateDataSource()
        {
            if (grdForexRate.DataSource != null)
            {
                DataTable dt = (DataTable)grdForexRate.DataSource;
                dt.Rows.Clear();
                grdForexRate.DataSource = dt;
            }
        }

        private void ucbAccount_ValueChanged(object sender, EventArgs e)
        {
            clearGrdForexRateDataSource();
        }

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
