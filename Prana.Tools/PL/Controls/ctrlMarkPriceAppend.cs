using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.LiveFeed;
using Prana.ClientCommon;
using Prana.ClientCommon.BLL;
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
    public partial class ctrlMarkPriceAppend : UserControl, ILiveFeedCallback, IPublishing
    {

        //EventHandler DisableReconOutputUI;
        //bool _btnGetLiveFeedClicked = false;
        bool _isHeaderCheckBoxChecked = false;
        //List<int> _accountIDUnlocked = new List<int>();
        //StringBuilder _accountUnlocked = new StringBuilder();
        //DialogResult copydateresult;
        //Global variable for getting only modified rows when modified from daily valuation UI.
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
        static string _markPriceFilePath = string.Empty;
        static string _markPriceLayoutDirectoryPath = string.Empty;
        const string DATEFORMAT = "MM/dd/yyyy";

        List<int> _alreadyPromptedAccountsForLock = new List<int>();
        private List<UltraGridRow> _selectedColumnList = new List<UltraGridRow>();
        /// <summary>
        /// added by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
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

        public ctrlMarkPriceAppend()
        {
            try
            {
                InitializeComponent();
                if (!CustomThemeHelper.IsDesignMode())
                {
                    CreatePricingServiceProxy();
                    this.InitializeControl();
                    ClientPricingManager.GetInstance.PriceResponseEventHandler += SMPricing_PriceResponseEventHandler;
                    ClientPricingManager.GetInstance.PriceResponseEventHandlerfromfile += CtrlPricingPolicy_PricingPolicyEvent;
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
                //string value = e.Value;
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
                            //MessageBox.Show("Central Com Server Disconnected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            ultraStatusBarMarkPriceStatus.Text = " Central Com Server Disconnected.";
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
                                    if (Convert.ToBoolean(dRow["Select"]))
                                    {
                                        if (symbolinGridRow.Equals(symbol.ToUpper(), StringComparison.OrdinalIgnoreCase) && accountWisePrice.ContainsKey(accountIdInGridRow) && _dtGridDataSource.Columns.Contains(dateInResp))
                                        {
                                            _isDataCopiedFromBackDate = true;
                                            dRow[dateInResp] = accountWisePrice[accountIdInGridRow];
                                            dRow["Source"] = PricingSource.BloombergDLWS;
                                            dRow["Comments"] = string.Empty;

                                            if (accountWisePrice[accountIdInGridRow] == 0)
                                                dRow["Comments"] = "Price not available on bloomberg";
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




        private void CtrlPricingPolicy_PricingPolicyEvent(object sender, EventArgs<DataTable> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() => CtrlPricingPolicy_PricingPolicyEvent(sender, e)));
                    }
                    else
                    {
                        SetControlsOnFetchAPIClick(false);
                        DataTable dt = e.Value;
                        foreach (DataRow priceData in dt.Rows)
                        {


                            String symbol = priceData["Symbol"].ToString();
                            DateTime date;
                            if (DateTime.TryParse(priceData["Date"].ToString(), out date))
                            {

                                Dictionary<int, Double> accountWisePrice = GetAccountAndFieldForPriceResponseForFile(priceData);
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
                                        dRow["Source"] = PricingSource.Gateway;
                                        dRow["Comments"] = string.Empty;

                                        if (accountWisePrice[accountIdInGridRow] == 0)
                                            dRow["Comments"] = "Price not available on File";
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


        private Dictionary<int, double> GetAccountAndFieldForPriceResponseForFile(DataRow priceData)
        {
            Dictionary<int, Double> accountWisePrice = new Dictionary<int, double>();
            try
            {
                String symbol = priceData["Symbol"].ToString().ToUpper();
                int accountID = int.Parse(priceData["AccountID"].ToString());

                if (_requestedSymbols.ContainsKey(accountID))
                {
                    Dictionary<string, SymbolPriceRequest> requestedSymbols = new Dictionary<string, SymbolPriceRequest>();
                    requestedSymbols = _requestedSymbols[accountID];
                    if (requestedSymbols.ContainsKey(symbol))
                    {
                        SymbolPriceRequest symbolsReqList = requestedSymbols[symbol];
                        double price = 0;

                        double.TryParse(priceData[5].ToString(), out price);
                        if (!accountWisePrice.ContainsKey(symbolsReqList.accountId))
                        {
                            accountWisePrice.Add(symbolsReqList.accountId, price);
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





        private Dictionary<int, Double> GetAccountAndFieldForPriceResponse(DataRow priceData, string colName)
        {
            Dictionary<int, Double> accountWisePrice = new Dictionary<int, double>();
            try
            {

                String symbol = priceData["Symbol"].ToString().ToUpper();
                //  String priceField = priceData["Symbol"].ToString().ToUpper();
                List<int> accountList = GetAccountIDFromdict(priceData, colName);
                foreach (int accountID in accountList)
                {
                    if (_requestedSymbols.ContainsKey(accountID))
                    {
                        Dictionary<string, SymbolPriceRequest> requestedSymbols = new Dictionary<string, SymbolPriceRequest>();
                        requestedSymbols = _requestedSymbols[accountID];
                        if (requestedSymbols.ContainsKey(symbol))
                        {
                            SymbolPriceRequest symbolReq = requestedSymbols[symbol];
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
                        // File open dialog, ask user to import mark prices.
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
                            InformationMessageBox.Display("Operation cancelled by User", "Mark Price Import");
                            return;
                        }
                        UpdationAfterImporting(strFileName);
                    }
                    else
                    {
                        MessageBox.Show("Please select symbols to import prices.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                WarningMessageBox.Display("File is in use. Close file and retry", "Mark Price Import");
                return;
            }
            try
            {
                spreadSheet = new ExcelDataReader(fs);

                if (spreadSheet != null && spreadSheet.WorkbookData != null && spreadSheet.WorkbookData.Tables != null && spreadSheet.WorkbookData.Tables.Count != 0)
                {
                    // create a new table whose columns will be in proper organized according to our requirement

                    DataTable tableMarkPriceImported = new DataTable();
                    // result is a Dataset , get the excel sheet values into the Dataset named result
                    result = spreadSheet.WorkbookData;
                    // copy all the record from result Dataset to the Table dt
                    tableMarkPriceImported = result.Tables[0].Copy();

                    // now rename the columns of the Table, 
                    // xml create a row of Headers of the excel sheet, so 1st row will give the headers
                    // of the table so rename the columns
                    for (int j = 0; j < result.Tables[0].Columns.Count; j++)
                    {
                        tableMarkPriceImported.Columns[j].ColumnName = result.Tables[0].Rows[0].ItemArray[j].ToString();
                    }
                    // delete the 1st row of the table because columns has given the same name as the headers
                    tableMarkPriceImported.Rows[0].Delete();
                    tableMarkPriceImported.TableName = "TableMarkPriceImported";


                    DataTable tableGridMarkPrices = _dtGridDataSource;
                    //string colName = string.Empty;
                    //DateTime colParesedDate = DateTime.Now;
                    //DateTime dateImported = (DateTime)dtLiveFeed.Value;
                    //DateTime dateImported = (DateTime)dtStartDate.Value;

                    //string importedDateColumn = dateImported.ToString("MM/dd/yyyy");
                    //If grid already have some rows.
                    if (_dtGridDataSource.Rows.Count > 0)
                    {
                        //Looping through the grid to update values in the grid from the imported data.
                        foreach (DataRow rowTableGridMarkPrices in tableGridMarkPrices.Rows)
                        {
                            if (tableGridMarkPrices.Columns.Contains("Select") == true && rowTableGridMarkPrices["Select"].ToString() == "True")
                            {
                                //int colLen = rowTableGridMarkPrices.Table.Columns.Count;
                                //colName = rowTableGridMarkPrices.Table.Columns[colLen - 1].ColumnName;

                                foreach (DataColumn col in tableGridMarkPrices.Columns)
                                {
                                    DateTime dateParsedOut = DateTime.Now;
                                    if (DateTime.TryParse(col.ToString(), out dateParsedOut))
                                    {
                                        string tabColSymbol = rowTableGridMarkPrices["Symbol"].ToString();
                                        string tabColAccount = rowTableGridMarkPrices["FundName"].ToString();
                                        string tabColDate = col.ColumnName.ToString();

                                        foreach (DataRow rowTableMarkPriceImported in tableMarkPriceImported.Rows)
                                        {
                                            string fileTabColSymbol = rowTableMarkPriceImported["Symbol"].ToString();
                                            string fileColAccount = rowTableMarkPriceImported["FundName"].ToString();

                                            foreach (DataColumn importFileCol in tableMarkPriceImported.Columns)
                                            {
                                                bool isParsed = false;
                                                double outResult;
                                                isParsed = double.TryParse(importFileCol.ToString(), out outResult);
                                                if (isParsed)
                                                {
                                                    DateTime fileDateParsedOut = DateTime.FromOADate(Convert.ToDouble(importFileCol.ToString()));
                                                    string fileColDate = fileDateParsedOut.ToString(DATEFORMAT);

                                                    if (tabColDate.Equals(fileColDate))
                                                    {
                                                        //If the symbols match i.e. in the grid and in the imported data.
                                                        if (string.Compare(tabColSymbol, fileTabColSymbol, true) == 0 && string.Compare(tabColAccount, fileColAccount, true) == 0)
                                                        {
                                                            if (rowTableGridMarkPrices.RowState == DataRowState.Unchanged)
                                                            {
                                                                rowTableGridMarkPrices.SetModified();
                                                                rowTableGridMarkPrices["Select"] = "True";
                                                            }

                                                            //Added By : Manvendra P.
                                                            //Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3426

                                                            int accountID = CachedDataManager.GetInstance.GetAccountID(tabColAccount);
                                                            if (!NAVLockManager.GetInstance.ValidateTrade(accountID, Convert.ToDateTime(tabColDate)))
                                                            {
                                                                rowTableGridMarkPrices["Comments"] = "NAV is Locked, price can not be imported.";
                                                            }
                                                            else
                                                            {
                                                                rowTableGridMarkPrices[col] = rowTableMarkPriceImported[importFileCol];
                                                                rowTableGridMarkPrices["Source"] = PricingSource.Gateway;
                                                                rowTableGridMarkPrices["Comments"] = string.Empty;
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
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    WarningMessageBox.Display("Data file columns are not in predefind format, \n 1st column value should be the account name. Please include header 'AccountName'.\n2nd column value should be the symbol. Please include header 'Symbol'.\nRemaining column value should be mark price with header as date in format 'MM/DD/YYYY'.\nPlease select the correct file and try again.", "Mark Price Import");
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
        /// Unique message method implemntation
        /// 
        /// added by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <returns>The message (name of the control here)</returns>
        public string getReceiverUniqueName()
        {
            try
            {
                return "ctrlMarkPriceAppend";
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
                //  CachedData objCachedData = new CachedData();
                if (ucbClient.Rows.Count > 0)
                {
                    ucbClient.DataSource = null;
                }

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
                listValues.Insert(2, new EnumerationValue("Missing Marks", 1));
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

                        //modified By: Bharat raturi, 10 jun 2014
                        //purpose: check if the datatable is not null before save
                        if (_dtGridDataSource.Rows.Count > 0)
                        {
                            //int countSelectedRows = (from row in _dtGridDataSource.AsEnumerable()
                            //                   where row["Select"].ToString().Equals("True")
                            //                   select row).Count();

                            //if (countSelectedRows == 0)
                            //    message = "Please select symbols to save mark price";
                            rowsAffected = SaveMarkPriceData(dtDataToSave);
                            if (rowsAffected > 0)
                            {
                                message = "Mark Price details saved. Please Approved the changes.";
                                if (_isPermissionToApprove)
                                    message = "Mark Price details saved and approved.";

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
                if (_securityMaster.IsCSMConnected)
                {
                    ultraStatusBarMarkPriceStatus.Text = string.Empty;
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
                        //                    if (_requestedSymbols == null)
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
                                    foreach (UltraGridRow filteredRow in this.grdMarkPrice.DisplayLayout.Rows.GetFilteredInNonGroupByRows())
                                    {
                                        if (filteredRow.Cells["Select"].Value.ToString() == "True")
                                        {
                                            isRowSelect = true;
                                            //symbol = filteredRow.Cells["Symbol"].Value.ToString();
                                            symbol = filteredRow.Cells["BloombergSymbol"].Value.ToString();
                                            SymbolPriceRequest request = new SymbolPriceRequest();
                                            int accountId = int.Parse(filteredRow.Cells["FundID"].Value.ToString());
                                            request.accountId = accountId;
                                            request.Symbol = symbol;
                                            int auecId = int.Parse(filteredRow.Cells["AUECID"].Value.ToString());

                                            request.ExchangeId = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(auecId);
                                            request.AssetId = CachedDataManager.GetInstance.GetAssetIdByAUECId(auecId);
                                            filteredRow.Cells["Comments"].Value = string.Empty;

                                            //Added By : Manvendra P.
                                            //Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3426

                                            if (!NAVLockManager.GetInstance.ValidateTrade(accountId, Convert.ToDateTime(dateOut)))
                                            {
                                                filteredRow.Cells["Comments"].Value = "NAV is Locked, price can not be fetched.";
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrEmpty(symbol))
                                                {
                                                    if (DateTime.Parse(filteredRow.Cells["ExpirationDate"].Value.ToString()).Date != DateTimeConstants.MinValue.Date && DateTime.Parse(filteredRow.Cells["ExpirationDate"].Value.ToString()) < _endDate)
                                                    {
                                                        filteredRow.Cells["Comments"].Value = "Symbol expired on " + DateTime.Parse(filteredRow.Cells["ExpirationDate"].Value.ToString()).Date.ToString("MM/dd/yyyy");
                                                    }
                                                    else
                                                    {
                                                        if (!_requestedSymbols.ContainsKey(accountId))
                                                        {
                                                            SymbolPriceRequest symbolPriceReqList = new SymbolPriceRequest();
                                                            symbolPriceReqList = request;
                                                            Dictionary<string, SymbolPriceRequest> detailsDict = new Dictionary<string, SymbolPriceRequest>();
                                                            detailsDict.Add(symbol, symbolPriceReqList);
                                                            _requestedSymbols.Add(accountId, detailsDict);
                                                        }
                                                        else
                                                        {
                                                            Dictionary<string, SymbolPriceRequest> details = _requestedSymbols[accountId];

                                                            if (details.ContainsKey(symbol))
                                                            {
                                                                details[symbol] = request;
                                                            }
                                                            else
                                                            {
                                                                SymbolPriceRequest symbolPriceReqList = new SymbolPriceRequest();
                                                                symbolPriceReqList = request;
                                                                details.Add(symbol, symbolPriceReqList);
                                                            }

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
                            int statusCode = ClientPricingManager.GetInstance.GetPrices_New(_requestedSymbols, _startDate, _endDate, false);
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
                                MessageBox.Show("Please select symbols to fetch prices from API.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                                MessageBox.Show("Please see comments column for price fetching status.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        //List<UserOptModelInpu> listSymbolOMIdata = _pricingServicesProxy.InnerChannel.GetDataFromOMI(requestedSymbols);
                        //UpdateMarkPriceAndFXDataWithOMIData(listSymbolOMIdata);

                    }
                    else
                    {
                        MessageBox.Show("There are no symbols to fetch prices for them.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    ultraStatusBarMarkPriceStatus.Text = " Central Com Server Disconnected.";
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
        //                if (string.Compare(dRow["Symbol"].ToString(), symbol, true) == 0 && _dtGridDataSource.Columns.Contains(_colName))
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
                if (grdMarkPrice.DataSource != null)
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

                    workBook = this.ultraGridExcelExporter1.Export(this.grdMarkPrice, workBook.Worksheets[workbookName]);
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

        /// <summary>
        /// Initialize the layout of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMarkPrice_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraWinGridUtils.EnableFixedFilterRow(e);
                // Set the RowSelectorHeaderStyle to ColumnChooserButton.
                grdMarkPrice.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                // Enable the RowSelectors. This is necessary because the column chooser
                // button is displayed over the row selectors in the column headers area.
                grdMarkPrice.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;

                //grdPivotDisplay.DisplayLayout
                //Infragistics.Win.UltraWinGrid.UltraGridLayout.us
                grdMarkPrice.DisplayLayout.UseFixedHeaders = true;

                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("IsApproved"))
                {
                    grdMarkPrice.DisplayLayout.Bands[0].Columns["IsApproved"].Hidden = true;
                }

                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("Select"))
                {
                    UltraGridColumn colBtnSelect = grdMarkPrice.DisplayLayout.Bands[0].Columns["Select"];

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

                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("Symbol"))
                {
                    UltraGridColumn colSymbol = grdMarkPrice.DisplayLayout.Bands[0].Columns["Symbol"];
                    colSymbol.Header.Fixed = true;
                    colSymbol.CharacterCasing = CharacterCasing.Upper;
                    colSymbol.CellActivation = Activation.NoEdit;
                }
                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("FundName"))
                {
                    UltraGridColumn colAccountName = grdMarkPrice.DisplayLayout.Bands[0].Columns["FundName"];
                    colAccountName.Header.Fixed = false;
                    colAccountName.CharacterCasing = CharacterCasing.Upper;
                    colAccountName.CellActivation = Activation.NoEdit;
                    colAccountName.Header.Caption = "Account";
                    //colAccountName.Header.VisiblePosition = 0;
                }
                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("BloombergSymbol"))
                {
                    UltraGridColumn colBloomberg = grdMarkPrice.DisplayLayout.Bands[0].Columns["BloombergSymbol"];
                    colBloomberg.Header.Fixed = true;
                    colBloomberg.CharacterCasing = CharacterCasing.Upper;
                    colBloomberg.CellActivation = Activation.NoEdit;
                }
                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("ForwardPoints"))
                {
                    UltraGridColumn colForwardPoints = grdMarkPrice.DisplayLayout.Bands[0].Columns["ForwardPoints"];
                    colForwardPoints.Header.Fixed = true;
                    colForwardPoints.Hidden = true;
                    colForwardPoints.CellActivation = Activation.NoEdit;
                }
                // Excluding FX Rate from column chooser
                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("FxRate"))
                {
                    UltraGridColumn colFxRate = grdMarkPrice.DisplayLayout.Bands[0].Columns["FxRate"];
                    colFxRate.Header.Fixed = true;
                    colFxRate.Hidden = true;
                    colFxRate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    colFxRate.CellActivation = Activation.NoEdit;
                }
                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("ISINSymbol"))
                {
                    UltraGridColumn colISIN = grdMarkPrice.DisplayLayout.Bands[0].Columns["ISINSymbol"];
                    colISIN.Header.Fixed = true;
                    colISIN.Hidden = true;
                    colISIN.Header.Caption = "ISIN";
                    colISIN.CellActivation = Activation.NoEdit;
                }
                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("CUSIPSymbol"))
                {
                    UltraGridColumn colCUSIP = grdMarkPrice.DisplayLayout.Bands[0].Columns["CUSIPSymbol"];
                    colCUSIP.Header.Fixed = true;
                    colCUSIP.Hidden = true;
                    colCUSIP.Header.Caption = "CUSIP";
                    colCUSIP.CellActivation = Activation.NoEdit;
                }
                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("Currency"))
                {
                    UltraGridColumn colCurrency = grdMarkPrice.DisplayLayout.Bands[0].Columns["Currency"];
                    colCurrency.Header.Fixed = true;
                    colCurrency.Hidden = true;
                    colCurrency.Header.Caption = "Currency";
                    colCurrency.CellActivation = Activation.NoEdit;
                }
                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("FundID"))
                {
                    grdMarkPrice.DisplayLayout.Bands[0].Columns["FundID"].Hidden = true;
                    grdMarkPrice.DisplayLayout.Bands[0].Columns["FundID"].CellActivation = Activation.NoEdit;
                }
                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("AUECID"))
                {
                    grdMarkPrice.DisplayLayout.Bands[0].Columns["AUECID"].Hidden = true;
                    grdMarkPrice.DisplayLayout.Bands[0].Columns["AUECID"].CellActivation = Activation.NoEdit;
                }
                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("AUECIdentifier"))
                {
                    grdMarkPrice.DisplayLayout.Bands[0].Columns["AUECIdentifier"].Hidden = true;
                    grdMarkPrice.DisplayLayout.Bands[0].Columns["AUECIdentifier"].CellActivation = Activation.NoEdit;
                }

                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("Source"))
                {
                    UltraGridColumn colPricingSource = grdMarkPrice.DisplayLayout.Bands[0].Columns["Source"];
                    colPricingSource.Header.Caption = "Pricing Source";
                    colPricingSource.Header.Fixed = true;
                    // colPricingSource.CharacterCasing = CharacterCasing.Upper;
                    colPricingSource.CellActivation = Activation.NoEdit;

                    if (_methodologySelected != MethdologySelected.Daily)
                    {
                        colPricingSource.Hidden = true;
                        colPricingSource.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    }
                    else
                    {
                        colPricingSource.Hidden = false;
                        colPricingSource.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                    }
                }
                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("Comments"))
                {
                    UltraGridColumn colComments = grdMarkPrice.DisplayLayout.Bands[0].Columns["Comments"];
                    colComments.Header.Caption = "Comments";
                    colComments.Hidden = true;
                    colComments.CellActivation = Activation.NoEdit;
                }

                if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("ExpirationDate"))
                {
                    grdMarkPrice.DisplayLayout.Bands[0].Columns["ExpirationDate"].Hidden = true;
                    grdMarkPrice.DisplayLayout.Bands[0].Columns["ExpirationDate"].CellActivation = Activation.NoEdit;
                }

                //if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Exists("IsApproved"))
                //{
                //    foreach (UltraGridRow row in grdMarkPrice.Rows)
                //    {
                //        if (!string.IsNullOrWhiteSpace(row.Cells["IsApproved"].Value.ToString()) && Convert.ToBoolean(row.Cells["IsApproved"].Value))
                //        {
                //            foreach (UltraGridCell cell in row.Cells)
                //            {
                //                DateTime dt = DateTime.MinValue;
                //                //if(DateTime.TryParse(cell.Column.Key))
                //            }
                //        }
                //    }
                //}

                foreach (UltraGridColumn col in grdMarkPrice.DisplayLayout.Bands[0].Columns)
                {
                    DateTime columndate;
                    if (DateTime.TryParse(col.Header.Caption.Trim(), out columndate))
                    {
                        col.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DoublePositive;
                        //col.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Double;
                        col.MaskInput = "nnnnnnnnn.nnnnn";
                    }
                }

                // load the saveout file if it exists
                LoadReportSaveLayoutXML();
                grdMarkPrice.DisplayLayout.Override.FilterOperatorDefaultValue = FilterOperatorDefaultValue.Equals;
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
                if (grdMarkPrice.Rows.Count > 0)
                {

                    int rowIndex = 1;
                    bool isValidatedData = true;
                    string msgNAVlock = string.Empty;
                    foreach (DataRow dRow in _dtGridDataSource.Rows)
                    {
                        if (dRow.RowState != DataRowState.Unchanged)
                        {

                            foreach (DataColumn dCol in _dtGridDataSource.Columns)
                            {  //Modified by omshiv, check for date column only
                                DateTime date = new DateTime();
                                bool isParsed = DateTime.TryParse(dCol.ColumnName, out date);
                                if (isParsed)
                                {
                                    //If mark price field is blank.
                                    if (!string.IsNullOrWhiteSpace(dRow["Symbol"].ToString()) && string.IsNullOrWhiteSpace(dRow[dCol.ColumnName].ToString()))
                                    {
                                        isValidatedData = false;
                                        InformationMessageBox.Display("Please enter the mark price for Symbol '" + dRow["Symbol"], "Mark Price Save");
                                        //modified by: Bharat Raturi, 10 jun 2014
                                        //purpose: return from the method if any of the symbol does not have mark price
                                        return null;
                                    }
                                    else
                                    {
                                        int accountID = Convert.ToInt32(dRow["FundID"].ToString());
                                        //string accountName = dRow["FundName"].ToString();
                                        bool isNavLocked = NAVLockManager.GetInstance.ValidateTrade(accountID, date);//check NAV nock
                                        if (!isNavLocked)
                                        {
                                            msgNAVlock = "NAV is locked for some account(s).\nModification in mark prices allowed only for valid accounts.";
                                            // if locked after showin message.
                                            //MessageBox.Show("NAV is locked for the account: " + accountName + "\nPlease Select Valid accounts for modification in markprices", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            //return null;
                                        }
                                    }
                                }
                            }

                        }
                        rowIndex++;
                        if ((dRow.RowState == DataRowState.Unchanged && _isDataCopiedFromBackDate) && (!dRow[5].ToString().Equals("0") || !dRow["ForwardPoints"].ToString().Equals("0")))
                            dRow.SetModified();
                    }
                    if (!string.IsNullOrWhiteSpace(msgNAVlock))
                    {
                        MessageBox.Show(msgNAVlock, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    if (isValidatedData.Equals(true))
                    {
                        //SetDefaultGridAppreance(_colName);

                        DataTable dtChanges = ((DataTable)grdMarkPrice.DataSource).GetChanges();
                        //if (dtChanges != null)
                        //{
                        //    DataTable dtSelectedChanges = new DataTable();
                        //    if (dtChanges != null)
                        //    {
                        //        var selectedRows = from row in dtChanges.AsEnumerable()
                        //                           where row["Select"].ToString().Equals("True")
                        //                           select row;

                        //        if (selectedRows != null && selectedRows.Count() > 0)
                        //            dtSelectedChanges = selectedRows.CopyToDataTable();

                        //        _dtGridDataSource.AcceptChanges();
                        //    }
                        _dtGridDataSource.AcceptChanges();
                        return dtChanges;
                        //}
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

        //Modified BY: Bharat raturi
        //Validate the end date for the mark pricing 
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
                clearGrdMarkPriceDataSource();
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
            dtAccount.Columns.Add("AccountID", typeof(int));

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
                    MessageBox.Show("Select Client and at least one account to show the data", "Mark Price Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                //SetFxFxForwardFilteration();
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
        /// Set the filteration for the grid
        /// there is no use of this method
        /// Commented by: sachin mishra 28 jan 2015
        /// </summary>
        //private void SetFxFxForwardFilteration()
        //{
        //if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Count > 0)
        //{
        //    //Divya: As we have to maintain the exchange group filteration applied from Combobox, to remove Fx Fxforwards from Mark price
        //    // tab , I am applying a filter on AUECId instead of AUECIdentifier
        //    grdMarkPrice.DisplayLayout.Bands[0].ColumnFilters["AUECId"].FilterConditions.Add(FilterComparisionOperator.NotEquals, "33");
        //    grdMarkPrice.DisplayLayout.Bands[0].ColumnFilters["AUECId"].FilterConditions.Add(FilterComparisionOperator.NotEquals, "32");
        //}
        //}

        //private void BindGridForMarkPrice(string selectedTab)
        //{
        //    //Get mark prices data for symbols for the selected date and methodology.
        //    DataTable dtMarkPrices = new DataTable();
        //    try
        //    {
        //        //bool isFxFXForwardData = true;
        //        if (dtMarkPrices != null)
        //        {
        //            grdMarkPrice.DataSource = null;
        //            grdMarkPrice.DataSource = dtMarkPrices;
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

        /// <summary>
        /// Fill the grid with the data
        /// 
        /// Modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced LOGANDTHROW
        /// </summary>
        /// <param name="xmlAccounts">XML document containing the IDs of the selected accounts</param>
        /// <param name="filter">0 for all, 1 for missing marks, 2 for manual source</param>
        private void BindGridWithData(string xmlAccounts, int filter)
        {
            try
            {
                clearGrdMarkPriceDataSource();
                grdMarkPrice.DataBind();
                DataTable dtOriginal = new DataTable();
                int methodology = (int)_methodologySelected;
                Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> symbolMarkPriceInfo = _pricingServicesProxy.InnerChannel.GetMarkPriceForDateRangeWithAccounts(xmlAccounts, _startDate, _endDate, methodology, false, filter);
                dtOriginal = GetTableFromSymbolMarkPriceDictDateRange(symbolMarkPriceInfo);

                // Purpose : To show comments for non valid symbols in case prices not fetched from API.
                if (!dtOriginal.Columns.Contains("Comments"))
                {
                    DataColumn colComments = new DataColumn("Comments", typeof(System.String));
                    colComments.DefaultValue = string.Empty;
                    dtOriginal.Columns.Add(colComments);
                }

                grdMarkPrice.DataSource = dtOriginal;
                UltraGridRow[] FilteredRows = grdMarkPrice.Rows.GetFilteredInNonGroupByRows();
                List<String> symbols = new List<string>();
                foreach (UltraGridRow row in FilteredRows)
                {
                    if (!symbols.Contains(row.Cells["Symbol"].Value.ToString()))
                    {
                        symbols.Add(row.Cells["Symbol"].Value.ToString());

                    }
                }
                if (dtOriginal != null)
                {
                    DataTable dtClone = dtOriginal.Clone();

                    foreach (DataRow dr in dtOriginal.Rows)
                    {
                        if (symbols.Contains(dr["Symbol"].ToString()))
                        {
                            dtClone.ImportRow(dr);
                        }
                    }
                    ConvertBlankNumericValues(dtClone);
                    DataTable dtFinalCopied = (DataTable)grdMarkPrice.DataSource;

                    //added by: Bharat Raturi, date: 20 may 2014
                    //purpose: add new column to preserve the existing mark price
                    //if (!dtFinalCopied.Columns.Contains("OldMarkPrice"))
                    //{
                    //    dtFinalCopied.Columns.Add("OldMarkPrice", typeof(float));
                    //}
                    foreach (DataRow dr in dtClone.Rows)
                    {
                        foreach (DataRow drow in dtFinalCopied.Rows)
                        {
                            if (dr["Symbol"].ToString().Equals(drow["Symbol"].ToString()))
                            {
                                if (dtFinalCopied.Columns.Contains("ForwardPoints"))
                                {
                                    if (dtClone.Columns.Contains("ForwardPoints"))
                                    {
                                        drow["ForwardPoints"] = dr["ForwardPoints"];
                                        drow[5] = dr[5];
                                        drow.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }
                    //foreach (DataRow dr in dtFinalCopied.Rows)
                    //{
                    //    dr["OldMarkPrice"]=dr[""]
                    //}
                    dtFinalCopied.AcceptChanges();
                    grdMarkPrice.DataSource = dtFinalCopied;

                    //  ResetFilterAfterRebindingGrid();
                    _dtGridDataSource = dtFinalCopied;

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
        /// Handle the null values received from the database
        /// </summary>
        /// <param name="dtNum">Datasource of the grid</param>
        private void ConvertBlankNumericValues(DataTable dtNum)
        {
            try
            {
                if (dtNum != null && dtNum.Columns.Count > 0)
                {
                    foreach (DataRow dr in dtNum.Rows)
                    {
                        foreach (DataColumn dCol in dtNum.Columns)
                        {
                            if ((dCol.DataType == typeof(System.Single)
                               || dCol.DataType == typeof(System.Double)
                               || dCol.DataType == typeof(System.Decimal)
                               || dCol.DataType == typeof(System.Byte)
                               || dCol.DataType == typeof(System.Int16)
                               || dCol.DataType == typeof(System.Int32)
                               || dCol.DataType == typeof(System.Int64))
                               && (dr[dCol].ToString().Equals("0") || string.IsNullOrWhiteSpace(dr[dCol].ToString())))
                            {
                                dr[dCol] = 0;
                            }
                        }

                        if (dtNum.Columns.Contains("Symbol"))
                        {

                            if (!dr["Symbol"].ToString().ToUpper().Equals(dr["Symbol"].ToString()))
                            {
                                dr["Symbol"] = dr["Symbol"].ToString().ToUpper();
                            }
                        }
                        dr.AcceptChanges();
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


        //private void ResetFilterAfterRebindingGrid()
        //{
        //    try
        //    {
        //txtSymbolFilteration.Text = string.Empty;
        //SetExchangeGroupFilteration();
        //}
        //catch (Exception ex)
        //{
        // Invoke our policy that is responsible for making sure no secure information
        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Method to save the mark price details        
        /// Modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced LOGANDTHROW.
        /// </summary>
        /// <param name="dtDataToSave">Datatable holding the data</param>
        /// <returns>Number of rows affected</returns>
        internal int SaveMarkPriceData(DataTable dtDataToSave)
        {
            int rowsAffected = 0;
            try
            {
                DataTable dtMarkPrices = null;
                //Getting the mark price data from the control to save it.
                dtMarkPrices = dtDataToSave;
                if (dtMarkPrices != null)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    DataTable dtMarkPricesNew = new DataTable();
                    dtMarkPricesNew.TableName = "MarkPriceImport";
                    dtMarkPricesNew.Columns.Add(new DataColumn("Symbol"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("Date"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("MarkPrice"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("MarkPriceImportType"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("ForwardPoints"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("IsApproved"));

                    //Added AUECID as it will be used at pricing server end to update cache
                    dtMarkPricesNew.Columns.Add(new DataColumn("AUECID"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("AUECIdentifier"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("AccountID"));
                    dtMarkPricesNew.Columns.Add(new DataColumn("Source"));

                    //dtMarkPricesNew = dtMarkPrices.Copy();
                    foreach (DataRow dr in dtMarkPrices.Rows)
                    {
                        // only modified rows are present in dtMarkPrices
                        //if (dr.RowState != DataRowState.Modified)
                        //{
                        //    continue;
                        //}
                        //Assigning the row having symbol being not blank.
                        if (!string.IsNullOrWhiteSpace(dr["Symbol"].ToString()))
                        {
                            foreach (DataColumn dc in dtMarkPrices.Columns)
                            {
                                // commented by omshiv, modifed the condition, it date column found then extract the mark price data from row. 
                                //if (dc.ColumnName != "Select" && dc.ColumnName != "Symbol" && dc.ColumnName != "AUECID" && dc.ColumnName != "AUECIdentifier" && dc.ColumnName != "ForwardPoints" && dc.ColumnName != "FxRate" && dc.ColumnName != "BloombergSymbol" && dc.ColumnName != "AccountID" && dc.ColumnName != "Source" && dc.ColumnName != "PriceType" && dc.ColumnName != "AccountName")

                                DateTime date = new DateTime();
                                bool isDateParsed = DateTime.TryParse(dc.ColumnName, out date);
                                if (isDateParsed)
                                {
                                    double markPrice;
                                    double.TryParse(dr[dc.ColumnName].ToString(), out markPrice);
                                    //only non-zero mark prices will be saved.  
                                    if (markPrice != 0)
                                    {
                                        DataRow drNew = dtMarkPricesNew.NewRow();

                                        drNew["IsApproved"] = 0;
                                        drNew["Date"] = DateTime.ParseExact(dc.ColumnName, "MM/dd/yyyy", null);

                                        drNew["MarkPrice"] = markPrice;//  dr[dc.ColumnName];
                                        // this column value has been fixed to differentiate whether data save into the DB from Import module or Mark price UI
                                        // L stands for Live feed Data
                                        drNew["MarkPriceImportType"] = Prana.BusinessObjects.AppConstants.MarkPriceImportType.L.ToString();

                                        drNew["Symbol"] = dr["Symbol"].ToString().ToUpper();
                                        drNew["FundID"] = Convert.ToInt32(dr["FundID"].ToString());
                                        if (dr["Source"] != DBNull.Value && !string.IsNullOrWhiteSpace(dr["Source"].ToString()))
                                            drNew["Source"] = (int)Enum.Parse(typeof(PricingSource), dr["Source"].ToString());
                                        else
                                            drNew["Source"] = 1; //TODO check why source is null or blank

                                        drNew["ForwardPoints"] = dr["ForwardPoints"].ToString();
                                        //Added AUECID as it will be used at pricing server end to update cache
                                        if (dtMarkPrices.Columns.Contains("AUECID") && dr["AUECID"] != System.DBNull.Value)
                                            drNew["AUECID"] = dr["AUECID"].ToString().ToUpper();

                                        if (dtMarkPrices.Columns.Contains("AUECIdentifier") && dr["AUECIdentifier"] != System.DBNull.Value)
                                            drNew["AUECIdentifier"] = dr["AUECIdentifier"].ToString();


                                        dtMarkPricesNew.Rows.Add(drNew);
                                        dtMarkPricesNew.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }
                    if (dtMarkPricesNew != null && dtMarkPricesNew.Rows.Count > 0)
                    {

                        rowsAffected = _pricingServicesProxy.InnerChannel.SaveMarkPrices(dtMarkPricesNew, _isPermissionToApprove);
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
        #region Commented
        /// <summary>
        /// there is no use of this mthod.
        /// </summary>
        //public void SetupControl(string tabPageName)
        //{
        //    try
        //    {
        //        //_userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
        //        //optDaily.Checked = true;
        //        //this.optMonthly.Location = new System.Drawing.Point(60, 15);
        //        //optLastPrice.Checked = true;
        //        //optOverwriteAllSymbols.Checked = true;
        //        //optUseLiveFeed.Checked = true;
        //        //dtDateMonth.MaxDate = DateTime.Now;
        //        //lblLastDate.Text = "";
        //        //lblPreviousDate.Text = "";
        //        //lblIMidDate.Text = "";
        //        //lblMidDate.Text = "";
        //        //lblSelectedFeedDate.Text = "";
        //        //lblLiveFeedDatePrice.Text = "";
        //        //btnGetFilteredData.Visible = true;
        //        //btnClearFilter.Visible = true;
        //        //txtSymbolFilteration.Visible = true;
        //        //lblFilteredSymbol.Visible = true;
        //        //grpBoxLiveFeedHandling.Visible = true;
        //        //ultraLabel1.Visible = true;
        //        //optOverwriteAllSymbols.Visible = true;
        //        //optOverwriteAllZeroSymbols.Visible = true;
        //        //optOMIPrice.Visible = false;
        //        //_tabPageSelected = tabPageName;
        //        //_exchangeGroupings = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("UnderlyingGroupings");
        //        //_exchangeGroupings = ConfigurationHelper.Instance.LoadSectionBySectionName("UnderlyingGroupings");
        //        //If Forex tab is selected.
        //        //if (_tabPageSelected.Equals(TabName_ForexConversion))
        //        //{
        //        //    SetupForexConversion();
        //        //}
        //        else if (_tabPageSelected.Equals(TabName_MarkPrice))
        //        {
        //            cmbExchangeGroup.Visible = true;
        //            lblExchangeGroup.Visible = true;
        //            optUseImportExport.Visible = true;
        //            grpBoxImportExport.Visible = true;
        //            grdPivotDisplay.AllowDrop = true;
        //            grpBoxFilter.Visible = true;
        //            btnImport.Visible = true;
        //            btnExport.Visible = true;
        //            optOMIPrice.Visible = true;
        //            optDaily.Visible = true;
        //            optMonthly.Visible = true;
        //            grpBoxLiveFeedHandling.Visible = true;
        //            grpSelectDateMethodology.Visible = true;
        //            if (cmbExchangeGroup.DataSource == null)
        //            {
        //                BindAUECFilter(TabName_MarkPrice);
        //            }
        //            else if (cmbExchangeGroup.Value != null && !cmbExchangeGroup.Value.Equals("Nothing"))
        //            {
        //                cmbExchangeGroup.ValueChanged -= new EventHandler(cmbAUEC_ValueChanged);
        //                cmbExchangeGroup.Value = "Nothing";
        //                cmbExchangeGroup.ValueChanged += new EventHandler(cmbAUEC_ValueChanged);
        //            }
        //            optPreviousPrice.Visible = true;
        //            optIMidPrice.Visible = true;
        //            optMidPrice.Visible = true;
        //            optSelectedFeedPrice.Visible = true;
        //            optLastPrice.Visible = true;
        //            optLastPrice.Text = "Last Price";
        //            lblUpdatePrice.Visible = true;
        //            lblUpdatePrice.Text = "Update Price:";
        //            lblLastDate.Visible = true;
        //            lblPreviousDate.Visible = true;
        //            lblIMidDate.Visible = true;
        //            lblMidDate.Visible = true;
        //            lblSelectedFeedDate.Visible = true;
        //            btnGetLiveFeedData.Text = "Get Prices";
        //        }
        //        else if (_tabPageSelected.Equals(TabName_FXMarkPrice))
        //        {
        //            cmbExchangeGroup.Visible = false;
        //            lblExchangeGroup.Visible = false;
        //            optUseImportExport.Visible = false;
        //            grpBoxImportExport.Visible = false;
        //            grdPivotDisplay.AllowDrop = true;
        //            grpBoxFilter.Visible = false;
        //            btnImport.Visible = false;
        //            btnExport.Visible = false;
        //            optDaily.Visible = true;
        //            optMonthly.Visible = false;
        //            grpBoxLiveFeedHandling.Visible = false;
        //            grpSelectDateMethodology.Visible = true;
        //            optPreviousPrice.Visible = false;
        //            optIMidPrice.Visible = false;
        //            optMidPrice.Visible = false;
        //            optSelectedFeedPrice.Visible = false;
        //            optLastPrice.Visible = false;
        //            optLastPrice.Text = "Last Price";
        //            lblUpdatePrice.Visible = true;
        //            lblUpdatePrice.Text = "Update Price:";
        //            lblLastDate.Visible = false;
        //            lblPreviousDate.Visible = false;
        //            lblIMidDate.Visible = false;
        //            lblMidDate.Visible = false;
        //            lblSelectedFeedDate.Visible = false;
        //            btnGetLiveFeedData.Text = "Get Prices";
        //        }
        //        else if (_tabPageSelected.Equals(TabName_NAV))
        //        {
        //            SetupNAV();
        //        }
        //        else if (_tabPageSelected.Equals(TabName_DailyCash))
        //        {
        //            SetupDailyCash();
        //        }
        //        else if (_tabPageSelected.Equals(TabName_Beta))
        //        {
        //            ShowHideButtons();
        //            if (cmbExchangeGroup.Value != null && !cmbExchangeGroup.Value.Equals("Nothing"))
        //            {
        //                cmbExchangeGroup.ValueChanged -= new EventHandler(cmbAUEC_ValueChanged);
        //                cmbExchangeGroup.Value = "Nothing";
        //                cmbExchangeGroup.ValueChanged += new EventHandler(cmbAUEC_ValueChanged);
        //            }
        //        }
        //        else if (_tabPageSelected.Equals(TabName_TradingVol))
        //        {
        //            ShowHideButtons();
        //            if (cmbExchangeGroup.Value != null && !cmbExchangeGroup.Value.Equals("Nothing"))
        //            {
        //                cmbExchangeGroup.ValueChanged -= new EventHandler(cmbAUEC_ValueChanged);
        //                cmbExchangeGroup.Value = "Nothing";
        //                cmbExchangeGroup.ValueChanged += new EventHandler(cmbAUEC_ValueChanged);
        //            }
        //        }
        //        else if (_tabPageSelected.Equals(TabName_Delta))
        //        {
        //            ShowHideButtons();
        //            //if (!Alreadywired)
        //            //{
        //            //    if (_pricingAnalysis != null)
        //            //    {
        //            //        _pricingAnalysis.GreeksCalculated += new GreeksCalculaterHandler(_pricingAnalysis_GreeksCalculated);
        //            //    }
        //            //    Alreadywired = true;
        //            //}
        //            if (cmbExchangeGroup.Value != null && !cmbExchangeGroup.Value.Equals("Nothing"))
        //            {
        //                cmbExchangeGroup.ValueChanged -= new EventHandler(cmbAUEC_ValueChanged);
        //                cmbExchangeGroup.Value = "Nothing";
        //                cmbExchangeGroup.ValueChanged += new EventHandler(cmbAUEC_ValueChanged);
        //            }
        //        }
        //        else if (_tabPageSelected.Equals(TabName_Outstanding))
        //        {
        //            ShowHideButtons();
        //            cmbExchangeGroup.Visible = false;
        //            lblExchangeGroup.Visible = false;
        //            optLastPrice.Visible = false;
        //            lblUpdatePrice.Visible = false;
        //        }
        //        else if (_tabPageSelected.Equals(TabName_PerformanceNumbers))
        //        {
        //            BindAccounts();
        //            Account consolidated = new Account(int.MinValue, "Consolidated");
        //            _accountCollection.Add(consolidated);
        //            cmbExchangeGroup.Visible = false;
        //            lblExchangeGroup.Visible = false;
        //            optUseImportExport.Visible = false;
        //            grpBoxImportExport.Visible = false;
        //            grdPivotDisplay.AllowDrop = false;
        //            grpBoxFilter.Visible = false;
        //            btnImport.Visible = false;
        //            btnExport.Visible = false;
        //            optDaily.Visible = false;
        //            optMonthly.Visible = false;
        //            grpBoxFilter.Visible = false;
        //            grpBoxLiveFeedHandling.Visible = false;
        //            lblFilteredSymbol.Visible = false;
        //            txtSymbolFilteration.Visible = false;
        //            btnClearFilter.Visible = false;
        //            btnGetFilteredData.Visible = false;
        //            btnGetLiveFeedData.Text = "Get Prices";
        //            lblSelectDateView.Visible = true;
        //        }
        //        else if (_tabPageSelected.Equals(TabName_StartOfMonthCapitalAccount))
        //        {
        //            SetupStartOfMonthCapitalAccount();
        //        }
        //        else if (_tabPageSelected.Equals(TabName_UserDefinedMTDPnL))
        //        {
        //            SetupUserDefinedMTDPnL();
        //        }
        //        // bind grid for selected tab
        //        BindGridForSelectedTab(_tabPageSelected);
        //        //Filters saved for various tabs. AUEC filter has to be cleared as it was being saved with all other filters.
        //        if (tabPageName.Equals(TabName_Beta) || tabPageName.Equals(TabName_TradingVol) || tabPageName.Equals(TabName_Delta) || tabPageName.Equals(TabName_MarkPrice))
        //        {
        //            if (grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters.Exists("AUECIdentifier"))
        //                grdPivotDisplay.DisplayLayout.Bands[0].ColumnFilters["AUECIdentifier"].ClearFilterConditions();
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
        #endregion

        //private DataTable GetTableFromSymbolMarkPriceDict(Dictionary<DateTime, Dictionary<string, MarkPriceInfo>> symbolMarkPriceInfo, MethdologySelected methodologySelected)
        //{
        //    //_isForwardPointsRecalculated = false;
        //    DataTable dt = null;
        //    try
        //    {
        //        if (symbolMarkPriceInfo != null)
        //        {
        //            dt = new DataTable();
        //            dt.Columns.Add("Symbol", typeof(System.String));
        //            dt.Columns.Add("AUECID", typeof(System.Int32));
        //            dt.Columns.Add("AUECIdentifier", typeof(System.String));
        //            dt.Columns.Add("ForwardPoints", typeof(System.Double));
        //            dt.Columns.Add("FxRate", typeof(System.Double));
        //            dt.Columns.Add("BloombergSymbol", typeof(System.String));
        //            dt.PrimaryKey = new DataColumn[] { dt.Columns["Symbol"] };
        //        }
        //        else
        //        {
        //            return dt;
        //        }
        //        foreach (KeyValuePair<DateTime, Dictionary<string, MarkPriceInfo>> dateKeyValueMarkPrice in symbolMarkPriceInfo)
        //        {
        //            string date;
        //            if (_methodologySelected.Equals(MethdologySelected.Daily))
        //                date = ((DateTime)dtStartDate.Value).ToString("MM/dd/yyyy");
        //            else
        //                date = dateKeyValueMarkPrice.Key.Date.ToString("MM/dd/yyyy");
        //            // Here date column contain values of markprices, hence double.
        //            dt.Columns.Add(date, typeof(System.Double));
        //            Dictionary<string, MarkPriceInfo> symbolMarkInfo = dateKeyValueMarkPrice.Value;
        //            foreach (KeyValuePair<string, MarkPriceInfo> symbolMarkKeyValue in symbolMarkInfo)
        //            {
        //                DataRow symbolRow = dt.Rows.Find(symbolMarkKeyValue.Key);
        //                if (symbolRow == null)
        //                {
        //                    symbolRow = dt.NewRow();
        //                    symbolRow["Symbol"] = symbolMarkKeyValue.Key;
        //                    symbolRow["AUECID"] = symbolMarkKeyValue.Value.AUECID;
        //                    symbolRow["AUECIdentifier"] = symbolMarkKeyValue.Value.AUECIdentifier;
        //                    symbolRow["FxRate"] = symbolMarkKeyValue.Value.FxRate;
        //                    symbolRow["BloombergSymbol"] = symbolMarkKeyValue.Value.BloombergSymbol;
        //                    dt.Rows.Add(symbolRow);
        //                }
        //                if (dateKeyValueMarkPrice.Key.Date.Equals(symbolMarkKeyValue.Value.DateActual.Date))
        //                {
        //                    symbolRow[date] = symbolMarkKeyValue.Value.MarkPrice;
        //                    double forwardPoints = symbolMarkKeyValue.Value.ForwardPoints;
        //                    symbolRow["ForwardPoints"] = forwardPoints;
        //                }
        //                else
        //                {
        //                    symbolRow[date] = 0;
        //                    symbolRow["ForwardPoints"] = 0;
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

        private DataTable GetTableFromSymbolMarkPriceDictDateRange(Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> accountSymbolMarkPriceInfo)
        {
            //_isForwardPointsRecalculated = false;

            DataTable dt = null;
            try
            {

                if (accountSymbolMarkPriceInfo != null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("Select", typeof(System.Boolean));
                    dt.Columns["Select"].DefaultValue = false;
                    dt.Columns.Add("Symbol", typeof(System.String));
                    dt.Columns.Add("AUECID", typeof(System.Int32));
                    dt.Columns.Add("AUECIdentifier", typeof(System.String));
                    dt.Columns.Add("ForwardPoints", typeof(System.Double));
                    dt.Columns.Add("FxRate", typeof(System.Double));
                    dt.Columns.Add("BloombergSymbol", typeof(System.String));
                    dt.Columns.Add("FundID", typeof(System.String));
                    dt.Columns.Add("Source", typeof(System.String));
                    dt.Columns.Add("FundName", typeof(System.String));
                    dt.Columns.Add("ISINSymbol", typeof(System.String));
                    dt.Columns.Add("CUSIPSymbol", typeof(System.String));
                    dt.Columns.Add("Currency", typeof(System.String));
                    dt.Columns.Add("ExpirationDate", typeof(System.DateTime));
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["Symbol"], dt.Columns["AccountID"] };
                }
                else
                {
                    return dt;
                }

                foreach (DateTime markPriceDate in accountSymbolMarkPriceInfo.Keys)
                {
                    Dictionary<int, Dictionary<string, MarkPriceInfo>> symbolMarkPriceInfo = accountSymbolMarkPriceInfo[markPriceDate];
                    foreach (int AccountId in symbolMarkPriceInfo.Keys)
                    {
                        string date;
                        if (_methodologySelected.Equals(MethdologySelected.Daily))
                            date = ((DateTime)dtStartDate.Value).ToString("MM/dd/yyyy");
                        else
                            date = markPriceDate.Date.ToString("MM/dd/yyyy");
                        // Here date column contain values of markprices, hence double.
                        if (!dt.Columns.Contains(date))
                        {
                            dt.Columns.Add(date, typeof(System.Double));
                            //string approvalStatus=
                            //dt.Columns.Add()
                        }
                        Dictionary<string, MarkPriceInfo> symbolMarkInfo = symbolMarkPriceInfo[AccountId];

                        foreach (KeyValuePair<string, MarkPriceInfo> symbolMarkKeyValue in symbolMarkInfo)
                        {
                            Object[] objArray = { symbolMarkKeyValue.Key, symbolMarkKeyValue.Value.AccountID };
                            DataRow symbolRow = dt.Rows.Find(objArray);
                            if (symbolRow == null)
                            {
                                symbolRow = dt.NewRow();
                                symbolRow["FundName"] = symbolMarkKeyValue.Value.AccountName;
                                symbolRow["Symbol"] = symbolMarkKeyValue.Key;
                                symbolRow["AUECID"] = symbolMarkKeyValue.Value.AUECID;
                                symbolRow["AUECIdentifier"] = symbolMarkKeyValue.Value.AUECIdentifier;
                                symbolRow["FxRate"] = symbolMarkKeyValue.Value.FxRate;
                                symbolRow["BloombergSymbol"] = symbolMarkKeyValue.Value.BloombergSymbol;
                                symbolRow["FundID"] = symbolMarkKeyValue.Value.AccountID;
                                symbolRow["Source"] = symbolMarkKeyValue.Value.PricingSource;
                                symbolRow["ISINSymbol"] = symbolMarkKeyValue.Value.ISINSymbol;
                                symbolRow["CUSIPSymbol"] = symbolMarkKeyValue.Value.CUSIPSymbol;
                                symbolRow["Currency"] = symbolMarkKeyValue.Value.Currency;
                                symbolRow["ExpirationDate"] = symbolMarkKeyValue.Value.ExpirationDate;
                                dt.Rows.Add(symbolRow);
                            }
                            else
                            {

                            }


                            if (markPriceDate.Date.Equals(symbolMarkKeyValue.Value.DateActual.Date))
                            {
                                symbolRow[date] = symbolMarkKeyValue.Value.MarkPrice;
                                double forwardPoints = symbolMarkKeyValue.Value.ForwardPoints;
                                symbolRow["ForwardPoints"] = forwardPoints;
                            }
                            else
                            {
                                symbolRow[date] = 0;
                                symbolRow["ForwardPoints"] = 0;
                            }
                        }
                    }
                }
                dt.AcceptChanges();
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
            return dt;
        }

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
                clearGrdMarkPriceDataSource();
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
                    ubtnCopyMarkPrice.Visible = true;
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
                clearGrdMarkPriceDataSource();
                dtStartDate.MaskInput = "{LOC}mm/yyyy";
                dtEndDate.Visible = false;
                lblEndDate.Visible = false;
                if (optMonthly.Checked)
                {
                    _methodologySelected = MethdologySelected.Monthly;

                    // TODO : Need to remove after implementation of copy mark price for monthly & custom mode.
                    lblCopy.Visible = false;
                    dtCopyDate.Visible = false;
                    ubtnCopyMarkPrice.Visible = false;
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
                clearGrdMarkPriceDataSource();
                dtStartDate.MaskInput = "{LOC}mm/dd/yyyy";
                dtEndDate.Visible = true;
                lblEndDate.Visible = true;
                if (optCustom.Checked)
                {
                    _methodologySelected = MethdologySelected.DateRange;
                    lblCopy.Visible = false;
                    dtCopyDate.Visible = false;
                    ubtnCopyMarkPrice.Visible = false;
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
                clearGrdMarkPriceDataSource();
                // Modified by Ankit Gupta on 7 Oct, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1581
                // End date error is displayed, but it is not editable.

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
                        //dtStartDate.Value = DateTime.Today;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlMarkPriceAppend_Load(object sender, EventArgs e)
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
                ubtnCopyMarkPrice.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ubtnCopyMarkPrice.ForeColor = System.Drawing.Color.White;
                ubtnCopyMarkPrice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ubtnCopyMarkPrice.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ubtnCopyMarkPrice.UseAppStyling = false;
                ubtnCopyMarkPrice.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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

                btnSaveLayout.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
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
        private void grdMarkPrice_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
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
                        //if (!_accountUnlocked.ToString().Contains(rowAccountname))
                        //{
                        //    _accountUnlocked.Append(rowAccountname).Append(',');
                        //    _accountIDUnlocked.Add(accountID);
                        //}
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
        private void grdMarkPrice_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                //    if (_accountUnlocked.Length > 0)
                //    {
                //        if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking following accounts " + _accountUnlocked.ToString().Substring(0, _accountUnlocked.Length - 1) + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //        {
                //            Dictionary<int, string> dictAccounts = CachedDataManagerRecon.GetInstance.GetAllPermittedAccounts().ToDictionary(
                //                                    entry => entry.Key,
                //                                    entry => entry.Value.ToUpper());

                //            List<int> lockedAccounts = CachedDataManager.GetInstance.GetLockedAccounts();
                //            List<int> newAccountsToBelocked = new List<int>();
                //            newAccountsToBelocked.AddRange(_accountIDUnlocked);
                //            newAccountsToBelocked.AddRange(lockedAccounts);
                //            if (ReconUtilities.SetAccountsLockStatus(newAccountsToBelocked))
                //            {

                //                MessageBox.Show("The lock for accounts has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //                //update Locks in cache
                //                CachedDataManager.GetInstance.SetLockedAccounts(newAccountsToBelocked);
                //            }
                //            else
                //            {
                //                MessageBox.Show("CashAccounts are currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //            }
                //        }
                //    }
                _isHeaderCheckBoxChecked = false;
                //  _accountUnlocked = new StringBuilder();
                //  _accountIDUnlocked = new List<int>();
                if (grdMarkPrice.Rows.Count > 0 && grdMarkPrice.Rows.GetFilteredOutNonGroupByRows() != null)
                {
                    CheckState state = grdMarkPrice.DisplayLayout.Bands[0].Columns["Select"].GetHeaderCheckedState(grdMarkPrice.Rows);
                    UltraGridRow[] grdrows = grdMarkPrice.Rows.GetFilteredOutNonGroupByRows();
                    if (grdrows.Length > 0 && grdMarkPrice.Rows.Count > 0)
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
                        //modified by amit 03.03.2015 CHMW-2787
                        if (row.Cells != null && row.Cells.Exists("Select"))
                        {
                            row.Cells["Select"].Value = true;
                        }
                    }
                    if (state == CheckState.Unchecked)
                    {
                        foreach (UltraGridRow row in grdMarkPrice.Rows)
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
        private void grdMarkPrice_BeforeHeaderCheckStateChanged(object sender, BeforeHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                _isHeaderCheckBoxChecked = true;
                if (e.NewCheckState == CheckState.Checked)
                {
                    if (grdMarkPrice.DataSource != null)
                    {
                        DataTable dt = (DataTable)grdMarkPrice.DataSource;
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
                    if (grdMarkPrice.Rows.Count > 0)
                    {
                        _selectedColumnList.Clear();
                        foreach (UltraGridRow row in grdMarkPrice.Rows)
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

        private void grdMarkPrice_CellChange(object sender, CellEventArgs e)
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
                    e.Cell.Row.Cells["Source"].Value = PricingSource.UserDefined.ToString();
                    e.Cell.Row.Cells["Comments"].Value = string.Empty;
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
                if (!CustomThemeHelper.IsDesignMode())
                {
                    UnwireEvents();
                }
                if (components != null)
                {
                    components.Dispose();
                }
                if (_pricingServicesProxy != null && !CustomThemeHelper.IsDesignMode())
                {
                    _pricingServicesProxy.Dispose();
                }
                if (_dtGridDataSource != null)
                {
                    _dtGridDataSource.Dispose();
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
        private void grdMarkPrice_FilterCellValueChanged(object sender, FilterCellValueChangedEventArgs e)
        {
            try
            {
                if (UltraWinGridUtils.IsGrouppingAppliedOnGrid(grdMarkPrice))
                {
                    String filterCondition = e.FilterCell.Text;
                    grdMarkPrice.Rows.ColumnFilters[e.FilterCell.Column.Key].FilterConditions.Clear();
                    grdMarkPrice.Rows.ColumnFilters[e.FilterCell.Column.Key].FilterConditions.Add(new FilterCondition(FilterComparisionOperator.StartsWith, filterCondition));
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
        // private void btnSaveLayout_Click(object sender, EventArgs e)
        //  {
        //try
        //{
        //    if (grdMarkPrice != null)
        //    {
        //        if (grdMarkPrice.DisplayLayout.Bands[0].Columns.Count > 0)
        //        {
        //            if (!string.IsNullOrEmpty(_markPriceFilePath))
        //            {
        //                //grdMarkPrice.DisplayLayout.Bands[0].Columns["Source"].Hidden = true;
        //                grdMarkPrice.DisplayLayout.SaveAsXml(_markPriceFilePath);
        //                //grdMarkPrice.DisplayLayout.Bands[0].Columns["Source"].Hidden = false;

        //            }
        //        }
        //    }
        //    MessageBox.Show("Mark price layout saved", "Mark Price", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}
        //catch (Exception ex)
        //{
        //    // Invoke our policy that is responsible for making sure no secure information
        //    // gets out of our layer.
        //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //    if (rethrow)
        //    {
        //        throw;
        //    }
        //}
        //}

        /// <summary>
        /// Load report layout xml if file exist
        /// </summary>
        private void LoadReportSaveLayoutXML()
        {
            try
            {
                _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _markPriceLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID;
                _markPriceFilePath = _markPriceLayoutDirectoryPath + @"\ManageMarksLayout.xml";

                if (!Directory.Exists(_markPriceLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_markPriceLayoutDirectoryPath);
                }
                if (File.Exists(_markPriceFilePath) && btnSaveLayout.Visible)
                {
                    grdMarkPrice.DisplayLayout.LoadFromXml(_markPriceFilePath);
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

        // <summary>
        /// Update event of disabling other controls if amendments are made
        /// </summary>
        /// <param name="DisableApproveChanges"></param>
        //internal void UpdateEvent(EventHandler evntDisableReconOutputUI)
        //{
        //    try
        //    {
        //        //DisableReconOutputUI = evntDisableReconOutputUI;
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

        //internal void DisableControls(bool p)
        //{
        //    try
        //    {
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
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
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
                        MessageBox.Show("Select Client and at least one account to Copy the data", "Mark Price Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        ///Added by sachin mishra 13/04/15 
        ///Purpose: For clearing the datasource of the grdMarkPrice JIRA-CHMW-3378
        /// </summary>
        public void clearGrdMarkPriceDataSource()
        {
            if (grdMarkPrice.DataSource != null)
            {
                DataTable dt = (DataTable)grdMarkPrice.DataSource;
                dt.Rows.Clear();
                grdMarkPrice.DataSource = dt;
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
                _startDate = (DateTime)dtStartDate.Value;
                clearGrdMarkPriceDataSource();
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
                        //dtStartDate.Value = DateTime.Today;
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

        private void ucbAccount_ValueChanged(object sender, EventArgs e)
        {
            clearGrdMarkPriceDataSource();
        }

        #region IServiceOnDemandStatus Members
        public System.Threading.Tasks.Task<bool> HealthCheck()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
