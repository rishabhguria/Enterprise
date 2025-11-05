using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientPreferences;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.ImportExportUtilities.Excel;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.TradingTicket.Controls
{
    public partial class RestrictedAllowedSecuritiesPreference : UserControl
    {
        /// <summary>
        /// Transfer trade rules
        /// </summary>
        TranferTradeRules transfertraderules = CachedDataManager.GetInstance.GetTransferTradeRules();

        /// <summary>
        /// securitiesListToSave into db
        /// </summary>
        List<string> securitiesListToSave = new List<string>();

        /// <summary>
        /// The security master
        /// </summary>
        public ISecurityMasterServices _securityMaster;

        /// <summary>
        /// Gets or sets the security master.
        /// </summary>
        /// <value>
        /// The security master.
        /// </value>
        public ISecurityMasterServices SecurityMaster
        {
            get
            {
                return _securityMaster;
            }
            set
            {
                _securityMaster = value;
                _securityMaster.SecMstrDataResponse += _securityMaster_SecMstrDataResponse;
                _securityMaster.SecMstrDataSymbolSearcResponse += _securityMaster_SecMstrDataSymbolSearcResponse;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RestrictedAllowedSecuritiesPreference()
        {
            InitializeComponent();
            symbolValue.SymbolSearch += SymbolSearch;
            SetLabelText();
        }

        /// <summary>
        /// Sets the LabelText on UI
        /// </summary>
        private void SetLabelText()
        {
            try
            {
                if (transfertraderules.IsAllowRestrictedSecuritiesList)
                {
                    ultraGroupBox1.Text = "Restricted Securities List";
                }
                else
                {
                    ultraGroupBox1.Text = "Allowed Securities List";
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
        /// This method take a response from symbol lookup if the symbol is present in system and add the symbol in list.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EventArgs<SecMasterBaseObj></param>
        private void _securityMaster_SecMstrDataSymbolSearcResponse(object sender, EventArgs<SecMasterSymbolSearchRes> e)
        {
            try
            {
                UpdateDropDown(e.Value.StartWith, e.Value.Result);
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

        private delegate void UIThreadMarshellerForUpdateRow(object sender, EventArgs<SecMasterBaseObj> e);

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
                    if (pranaUltraGrid1.InvokeRequired)
                    {
                        UIThreadMarshellerForUpdateRow mi = new UIThreadMarshellerForUpdateRow(_securityMaster_SecMstrDataResponse);
                        this.BeginInvoke(mi, new object[] { sender, e });
                    }
                    else
                    {
                        if (radioButtonTicker.Checked)
                        {
                            if (!securitiesListToSave.Contains(e.Value.TickerSymbol))
                                securitiesListToSave.Add(e.Value.TickerSymbol);
                        }
                        else
                        {
                            if (!securitiesListToSave.Contains(e.Value.BloombergSymbol, StringComparer.OrdinalIgnoreCase))
                                securitiesListToSave.Add(e.Value.BloombergSymbol);
                        }
                        pranaUltraGrid1.DataBind();
                        symbolValue.Text = "";
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
                            ApplicationConstants.SymbologyCodes symbology = radioButtonTicker.Checked == true ? ApplicationConstants.SymbologyCodes.TickerSymbol : ApplicationConstants.SymbologyCodes.BloombergSymbol;
                            String symbol = e.Value.Trim();
                            reqObj.AddData(symbol, symbology);
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
        /// Updates the drop down.
        /// </summary>
        /// <param name="startWith">The start with.</param>
        /// <param name="items">The items.</param>
        public void UpdateDropDown(string startWith, IList<string> items)
        {
            try
            {
                symbolValue.updateDropDown(startWith, items);
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
        /// Symbols the search.
        /// </summary>
        /// <param name="e">The <see cref="string"/> instance containing the event data.</param>
        internal void SymbolSearch(object sender, EventArgs<string> e)
        {
            if (_securityMaster != null)
            {
                try
                {
                    SecMasterSymbolSearchReq searchReq = new SecMasterSymbolSearchReq(e.Value, radioButtonTicker.Checked == true ? ApplicationConstants.SymbologyCodes.TickerSymbol : ApplicationConstants.SymbologyCodes.BloombergSymbol)
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
        /// Imports the securities list 
        /// </summary>
        private void ImportSecuritiesButton_Click(object sender, EventArgs e)
        {
            try
            {
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
                    InformationMessageBox.Display("Operation cancelled by User", "Securities List Import");
                    return;
                }
                UpdationAfterImporting(strFileName);
                InformationMessageBox.Display("File imported successfully!", "Securities List Import");
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
        /// Adds the imported securities
        /// </summary>
        private void UpdationAfterImporting(string filePath)
        {
            string strFileName = filePath;
            DataSet result = new DataSet();

            ExcelDataReader spreadSheet = null;
            FileStream fs = null;
            try
            {
                fs = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception)
            {
                WarningMessageBox.Display("File is in use. Close file and retry", "Securities List Import");
                return;
            }
            try
            {
                spreadSheet = new ExcelDataReader(fs);

                if (spreadSheet != null && spreadSheet.WorkbookData != null && spreadSheet.WorkbookData.Tables != null && spreadSheet.WorkbookData.Tables.Count != 0)
                {
                    result = spreadSheet.WorkbookData;
                    for (int i = 1; i < result.Tables[0].Rows.Count; i++)
                    {
                        string symbol = result.Tables[0].Rows[i][0].ToString().ToUpper();
                        if (!string.IsNullOrEmpty(symbol) && !securitiesListToSave.Contains(symbol, StringComparer.OrdinalIgnoreCase))
                        {
                            SendValidatedSymbolToSM(null, new EventArgs<string>(symbol));
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
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Deletes the securities list
        /// </summary>
        private void DeleteSecuritiesButton_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Do you want to delete all securities?", ultraGroupBox1.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    securitiesListToSave.Clear();
                    pranaUltraGrid1.DataBind();
                    if (transfertraderules.IsAllowRestrictedSecuritiesList)
                        TradingTicketPrefManager.GetInstance.DeleteSecuritiesList("Restricted");
                    else
                        TradingTicketPrefManager.GetInstance.DeleteSecuritiesList("Allowed");
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
        /// Exports the securities list
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (securitiesListToSave.Count > 0)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
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
                    string name = string.Empty;
                    if (transfertraderules.IsAllowRestrictedSecuritiesList)
                        name = "Restricted";
                    else
                        name = "Allowed";
                    string workbookName = name + "SecuritiesList";
                    Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                    workBook.Worksheets.Add(workbookName);
                    workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];
                    workBook = this.ultraGridExcelExporter1.Export(this.pranaUltraGrid1, workBook.Worksheets[workbookName]);
                    workBook.Save(pathName);
                    InformationMessageBox.Display("File exported successfully!", "Securities List Export");
                }
                else
                    InformationMessageBox.Display("Nothing to export!", "Securities List Export");
            }
            catch (Exception ex)
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

        int radioButtonflag = 0;
        /// <summary>
        /// Handles ticker radio button check change
        /// </summary>
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonflag == 0)
                {
                    if (radioButtonTicker.Checked == false && securitiesListToSave.Count > 0)
                    {
                        DialogResult result = MessageBox.Show("All available Tickers will be Deleted - Do you want to Proceed?", ultraGroupBox1.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            securitiesListToSave.Clear();
                            pranaUltraGrid1.DataBind();
                            if (transfertraderules.IsAllowRestrictedSecuritiesList)
                                TradingTicketPrefManager.GetInstance.DeleteSecuritiesList("Restricted");
                            else
                                TradingTicketPrefManager.GetInstance.DeleteSecuritiesList("Allowed");
                        }
                        else
                        {
                            radioButtonflag = 1;
                            radioButtonTicker.Checked = true;
                        }
                    }
                    else if (radioButtonBloomberg.Checked == false && securitiesListToSave.Count > 0)
                    {
                        DialogResult result = MessageBox.Show("All available Bloomberg will be Deleted - Do you want to Proceed?", ultraGroupBox1.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            securitiesListToSave.Clear();
                            pranaUltraGrid1.DataBind();
                            if (transfertraderules.IsAllowRestrictedSecuritiesList)
                                TradingTicketPrefManager.GetInstance.DeleteSecuritiesList("Restricted");
                            else
                                TradingTicketPrefManager.GetInstance.DeleteSecuritiesList("Allowed");
                        }
                        else
                        {
                            radioButtonflag = 1;
                            radioButtonBloomberg.Checked = true;
                        }
                    }
                }
                else
                    radioButtonflag = 0;
            }
            catch (Exception ex)
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
        /// Adds the item in securityList
        /// </summary>
        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(symbolValue.Text) && !securitiesListToSave.Contains(symbolValue.Text, StringComparer.OrdinalIgnoreCase))
                {
                    SendValidatedSymbolToSM(null, new EventArgs<string>(symbolValue.Text.ToUpper()));
                }
                else if (symbolValue.Text != "")
                {
                    MessageBox.Show("Already exists in List", ultraGroupBox1.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please enter a value", ultraGroupBox1.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Remove the selected item from securityList
        /// </summary>
        private void removeButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (pranaUltraGrid1.DisplayLayout.ActiveRow != null)
                {
                    securitiesListToSave.Remove(pranaUltraGrid1.DisplayLayout.ActiveRow.ListObject.ToString());
                    pranaUltraGrid1.DataBind();
                }
                else
                {
                    MessageBox.Show("Please select a value to remove", ultraGroupBox1.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Saves the Security List based on restricted or allowed type
        /// </summary>
        internal void SaveSecuritiesList(int companyID)
        {
            string symbols;
            try
            {
                GetSymbols(out symbols);
                if (symbols == string.Empty)
                    symbols = null;
                if (transfertraderules.IsAllowRestrictedSecuritiesList)
                    TradingTicketPrefManager.GetInstance.SaveSecuritiesList(companyID, "Restricted", symbols, radioButtonTicker.Checked);
                else
                    TradingTicketPrefManager.GetInstance.SaveSecuritiesList(companyID, "Allowed", symbols, radioButtonTicker.Checked);
            }
            catch (Exception ex)
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
        /// Gets the comma separated securityList
        /// </summary>
        private void GetSymbols(out string symbols)
        {
            symbols = string.Empty;
            try
            {
                securitiesListToSave.Remove("");
                symbols = string.Join(",", securitiesListToSave);
            }
            catch (Exception ex)
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
        /// Sets the UI
        /// </summary>
        internal void SetupControl(int companyUserID)
        {
            try
            {
                Tuple<string, bool> fetchedValues;
                if (transfertraderules.IsAllowRestrictedSecuritiesList)
                    fetchedValues = TradingTicketPrefManager.GetInstance.GetSecuritiesList("Restricted");
                else
                    fetchedValues = TradingTicketPrefManager.GetInstance.GetSecuritiesList("Allowed");

                string securitiesFetched = fetchedValues.Item1;
                //radioButtonflag = 1;
                if (fetchedValues.Item2)
                    radioButtonTicker.Checked = true;
                else
                    radioButtonBloomberg.Checked = true;
                securitiesListToSave = securitiesFetched.Split(',').ToList();
                securitiesListToSave.Remove(string.Empty);
                pranaUltraGrid1.DataSource = securitiesListToSave;
                pranaUltraGrid1.DisplayLayout.Bands[0].Columns[0].Header.Caption = ultraGroupBox1.Text;
                pranaUltraGrid1.DisplayLayout.Bands[0].Columns[0].CellActivation = Activation.NoEdit;
                int readWritePermissionID = CompanyManager.GetUserSecuritiesListPermission(companyUserID);
                if (readWritePermissionID == 0)  //readonly
                {
                    addButton.Enabled = false;
                    removeButton.Enabled = false;
                    DeleteSecuritiesButton.Enabled = false;
                    ImportSecuritiesButton.Enabled = false;
                    radioButtonBloomberg.Enabled = false;
                    radioButtonTicker.Enabled = false;
                    symbolValue.Enabled = false;
                    pranaUltraGrid1.Enabled = false;

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
    }
}
