using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Prana.PricingService2UI.MarketDataMonitoring
{
    public partial class MarketDataMonitoringUI : Form
    {
        Form _marketDataProperties = null;
        private MarketDataProvider? _feedProvider = null;

        private Dictionary<string, string> dictRequestDataSnapshot = new Dictionary<string, string>();
        private Dictionary<string, string> dictRequestDataSubscription = new Dictionary<string, string>();
        private Dictionary<string, string> dictSnapshotMandateFields = new Dictionary<string, string>();

        private const string CONST_NIRVANAFIELDS = "NirvanaFields";
        private const string CONST_BBGMNEMONIC = "BBGMnemonic";
        private const string CONST_EQUITY = "Equity";
        private const string CONST_EQUITYOPTION = "EquityOption";
        private const string CONST_FUTURE = "Future";
        private const string CONST_FUTUREOPTION = "FutureOption";
        private const string CONST_FX = "FX";
        private const string CONST_FIXEDINCOME = "FixedIncome";
        private const string CONST_FXFORWARD = "FXForward";

        private enum ViewSelection
        {
            SymbolSubscription,
            UserQuotaAndPermission,
            SAPIRequestField
        }
        private ViewSelection _currentViewSelection;

        public MarketDataMonitoringUI(MarketDataProvider? feedProvider)
        {
            try
            {
                InitializeComponent();
                _feedProvider = feedProvider;

                if (_feedProvider == MarketDataProvider.ACTIV)
                {
                    ultraButtonUserQuotaPermissions.Visible = true;
                }
                if (_feedProvider == MarketDataProvider.SAPI)
                {
                    ultraButtonSAPIRequestFields.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Private Methods
        private async System.Threading.Tasks.Task RefreshSymbol()
        {
            try
            {
                if (ultraGridMarketDataSymbolSubscription.ActiveRow != null)
                {
                    string tickerSymbol = ultraGridMarketDataSymbolSubscription.ActiveRow.Cells[0].Value.ToString();

                    if (!string.IsNullOrWhiteSpace(tickerSymbol))
                    {
                        await PricingService2Manager.PricingService2Manager.GetInstance.DeleteAdvisedSymbol(tickerSymbol);
                        await PricingService2Manager.PricingService2Manager.GetInstance.RefreshMarketDataSymbolInformation(tickerSymbol);
                        await ShowSubscribedSymbols();

                        foreach (UltraGridRow row in ultraGridMarketDataSymbolSubscription.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value).Equals(tickerSymbol))
                            {
                                ultraGridMarketDataSymbolSubscription.ActiveRow = row;
                                break;
                            }
                        }
                    }

                    toolStripStatusLabel.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Data Refreshed.";
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private async System.Threading.Tasks.Task RefreshAllSymbols()
        {
            try
            {
                if (ultraGridMarketDataSymbolSubscription.Rows.Count > 0)
                {
                    foreach (UltraGridRow symbolRow in ultraGridMarketDataSymbolSubscription.Rows)
                    {
                        string tickerSymbol = symbolRow.Cells[0].Value.ToString();

                        if (!string.IsNullOrWhiteSpace(tickerSymbol))
                        {
                            await PricingService2Manager.PricingService2Manager.GetInstance.DeleteAdvisedSymbol(tickerSymbol);
                            await PricingService2Manager.PricingService2Manager.GetInstance.RefreshMarketDataSymbolInformation(tickerSymbol);
                        }
                    }

                    await ShowSubscribedSymbols();
                    toolStripStatusLabel.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Data Refreshed.";
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CopyToClipboard()
        {
            try
            {
                if (ultraGridMarketDataSymbolSubscription.ActiveRow != null)
                {
                    if (_feedProvider == MarketDataProvider.FactSet)
                        Clipboard.SetText(string.Format("Ticker Symbol: {0}, FactSet Symbol: {1}", ultraGridMarketDataSymbolSubscription.ActiveRow.Cells[0].Value.ToString(), ultraGridMarketDataSymbolSubscription.ActiveRow.Cells[1].Value.ToString()));
                    else if (_feedProvider == MarketDataProvider.ACTIV)
                        Clipboard.SetText(string.Format("Ticker Symbol: {0}, ACTIV Symbol: {1}", ultraGridMarketDataSymbolSubscription.ActiveRow.Cells[0].Value.ToString(), ultraGridMarketDataSymbolSubscription.ActiveRow.Cells[1].Value.ToString()));

                    toolStripStatusLabel.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Data Copied.";
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

        private void ExportData()
        {
            try
            {
                SaveFileDialog exportDataFileDialog = new SaveFileDialog();
                exportDataFileDialog.Title = "Subscribed Symbols Data Export";
                exportDataFileDialog.CheckPathExists = true;
                exportDataFileDialog.DefaultExt = ".xls";
                exportDataFileDialog.Filter = "Excel file (*.xls)|*.xls";

                if (exportDataFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ultraGridExcelSubscribedSymbolsExporter.Export(ultraGridMarketDataSymbolSubscription, exportDataFileDialog.FileName);
                    toolStripStatusLabel.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Data Exported.";
                }
                else
                {
                    toolStripStatusLabel.Text = "[" + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss") + "] Data Export Cancelled.";
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

        private async System.Threading.Tasks.Task ShowSubscribedSymbols()
        {
            try
            {


                DataTable dataTable = await PricingService2Manager.PricingService2Manager.GetInstance.GetSubscribedSymbolsMonitoringData();

                ultraGridMarketDataSymbolSubscription.DataSource = dataTable;
                ultraGridMarketDataSymbolSubscription.DataBind();
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
        /// This Method fetches the SAPI field data from DB and Bind it in the UltraGrid of Snapshot and Subscription
        /// </summary>
        private async System.Threading.Tasks.Task GetSAPIFieldData()
        {
            try
            {
                InitializeRequestDataDictSnapshot();
                InitializeRequestDataDictSubscription();
                InitializeDictSnapshotMandateFileds();

                DataSet dataSet = new DataSet();

                dataSet = await PricingService2Manager.PricingService2Manager.GetInstance.GetSAPIRequestFieldData("Subscription");
                if(dataSet != null && dataSet.Tables.Count > 0)
                {
                    ultraGridSAPISubscription.DataSource = dataSet.Tables[0];
                    ultraGridSAPISubscription.DataBind();
                }

                dataSet = await PricingService2Manager.PricingService2Manager.GetInstance.GetSAPIRequestFieldData("Snapshot");
                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    ultraGridSAPISnapshot.DataSource = dataSet.Tables[0];
                    ultraGridSAPISnapshot.DataBind();
                }
                SetSAPIGridView();
                SAPIGridCustomization();
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
        /// this method set the column header checkboxes in the grids
        /// </summary>
        private void SetSAPIGridView()
        {
            try
            {
                string columnHeaders = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                string[] values = columnHeaders.Split(',');

                //Set the header checkbox for Subscription Tab
                UltraGridBand band = ultraGridSAPISubscription.DisplayLayout.Bands[0];

                foreach (string str in values)
                {
                    if (!band.Columns.Exists(str))
                        band.Columns.Add(str);

                    UltraGridColumn col = band.Columns[str];
                    col.Header.Caption = str;
                    col.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    col.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Left;
                    col.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.RowsCollection;
                }

                //Set the header checkbox for Snapshot Tab
                UltraGridBand band2 = ultraGridSAPISnapshot.DisplayLayout.Bands[0];

                foreach (string str in values)
                {
                    if (!band2.Columns.Exists(str))
                        band2.Columns.Add(str);

                    UltraGridColumn col = band2.Columns[str];
                    col.Header.Caption = str;
                    col.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    col.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Left;
                    col.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.RowsCollection;
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
        /// This event reset the values of Disabled checkboxes for subscription Tab
        /// </summary>
        private void UltraGridSAPISubscription_AfterHeaderCheckStateChanged(object sender, Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in ultraGridSAPISubscription.Rows)
                {
                    DisableRequestFields(row, dictRequestDataSubscription);
                    SetMandatoryFields(row);
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
        /// This event reset the values of Disabled checkboxes for Snapshot Tab
        /// </summary>
        private void UltraGridSAPISnapshot_AfterHeaderCheckStateChanged(object sender, Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                foreach (UltraGridRow row in ultraGridSAPISnapshot.Rows)
                {
                    DisableRequestFields(row, dictRequestDataSnapshot);
                    SetMandatoryFields(row);
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
        /// This method set the customization of the grids set the columns in NoEdit Mode
        /// </summary>
        private void SAPIGridCustomization()
        {
            try
            {
                if(ultraGridSAPISubscription.DataSource != null)
                {
                    ultraGridSAPISubscription.DisplayLayout.Bands[0].Columns[CONST_NIRVANAFIELDS].CellActivation = Activation.NoEdit;
                    ultraGridSAPISubscription.DisplayLayout.Bands[0].Columns[CONST_BBGMNEMONIC].CellActivation = Activation.NoEdit;
                }
                if(ultraGridSAPISnapshot.DataSource != null)
                {
                    ultraGridSAPISnapshot.DisplayLayout.Bands[0].Columns[CONST_NIRVANAFIELDS].CellActivation = Activation.NoEdit;
                    ultraGridSAPISnapshot.DisplayLayout.Bands[0].Columns[CONST_BBGMNEMONIC].CellActivation = Activation.NoEdit;
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
        /// This method reset the location of update button according to the size of Main UI
        /// </summary>
        private void ultraPanel2_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    // Calculate the new position for the button (e.g., top-right corner)
                    int buttonX = this.ultraPanelSAPITabsContainer.Width - this.ultraButtonSAPIUpdate.Width - 10; // 10px padding from the right
                    int buttonY = 0; // 10px padding from the top

                    // Set the new location of the button
                    this.ultraButtonSAPIUpdate.Location = new Point(buttonX, buttonY);
                }
                else if (this.WindowState == FormWindowState.Normal)
                {
                    // Optionally, reposition the button when the form is restored to its normal size
                    this.ultraButtonSAPIUpdate.Location = new Point(780, 0); // Reset to original position or any other position
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
        /// This method reset the location of LabelMandatoryFields according to the size of Main UI
        /// </summary>
        private void ultraPanelHeader_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    // Calculate the new position for the button (e.g., top-right corner)
                    int buttonX = this.ultraPanelHeader.Width - this.labelMandatoryFields.Width - 10; // 10px padding from the right
                    int buttonY = 12; // 10px padding from the top

                    // Set the new location of the button
                    this.labelMandatoryFields.Location = new Point(buttonX, buttonY);
                }
                else if (this.WindowState == FormWindowState.Normal)
                {
                    // Optionally, reposition the button when the form is restored to its normal size
                    this.ultraButtonSAPIUpdate.Location = new Point(510, 12); // Reset to original position or any other position
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
        /// This method saves the Changes of tab selected and create the Dict for Request Field
        /// </summary>
        private void ultraButtonSAPIRequestFieldUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult response = MessageBox.Show("Updating the fields will impact on the costing. Do you want to continue?", "Bloomberg Fields Updation", MessageBoxButtons.YesNo);
                if (response.Equals(DialogResult.Yes))
                {
                    SaveSAPIRequestFieldData();
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
        /// This method saves the data of the grid selectd on the Tab in the DB
        /// </summary>
        private async void SaveSAPIRequestFieldData()
        {
            try
            {
                string tabSelected = ultraTabControl1.SelectedTab.Key.ToString();
                DataTable saveTable = new DataTable();
                if (tabSelected.Equals("Snapshot"))
                {
                    saveTable = (DataTable)ultraGridSAPISnapshot.DataSource;
                }
                else
                {
                    saveTable = (DataTable)ultraGridSAPISubscription.DataSource;
                }
                if (saveTable != null)
                {
                    DataTable dtSaveTableTemp = new DataTable();
                    dtSaveTableTemp.Columns.Add(new DataColumn(CONST_NIRVANAFIELDS));
                    dtSaveTableTemp.Columns.Add(new DataColumn(CONST_BBGMNEMONIC));
                    dtSaveTableTemp.Columns.Add(new DataColumn(CONST_EQUITY));
                    dtSaveTableTemp.Columns.Add(new DataColumn(CONST_EQUITYOPTION));
                    dtSaveTableTemp.Columns.Add(new DataColumn(CONST_FUTURE));
                    dtSaveTableTemp.Columns.Add(new DataColumn(CONST_FUTUREOPTION));
                    dtSaveTableTemp.Columns.Add(new DataColumn(CONST_FX));
                    dtSaveTableTemp.Columns.Add(new DataColumn(CONST_FIXEDINCOME));
                    dtSaveTableTemp.Columns.Add(new DataColumn(CONST_FXFORWARD));
                    foreach (DataRow dRow in saveTable.Rows)
                    {
                        DataRow dr = dtSaveTableTemp.NewRow();
                        dr[CONST_NIRVANAFIELDS] = dRow[CONST_NIRVANAFIELDS].ToString().Trim();
                        dr[CONST_BBGMNEMONIC] = dRow[CONST_BBGMNEMONIC].ToString().Trim();
                        dr[CONST_EQUITY] = dRow[CONST_EQUITY];
                        dr[CONST_EQUITYOPTION] = dRow[CONST_EQUITYOPTION];
                        dr[CONST_FUTURE] = dRow[CONST_FUTURE];
                        dr[CONST_FUTUREOPTION] = dRow[CONST_FUTUREOPTION];
                        dr[CONST_FX] = dRow[CONST_FX];
                        dr[CONST_FIXEDINCOME] = dRow[CONST_FIXEDINCOME];
                        dr[CONST_FXFORWARD] = dRow[CONST_FXFORWARD];
                        dtSaveTableTemp.Rows.Add(dr);
                        dtSaveTableTemp.AcceptChanges();
                    }
                    DataSet saveDataSetTemp = new DataSet();
                    saveDataSetTemp.Tables.Add(dtSaveTableTemp.Copy());
                    if (saveDataSetTemp != null && saveDataSetTemp.Tables[0].Rows.Count > 0)
                    {
                        await PricingService2Manager.PricingService2Manager.GetInstance.SaveSAPIRequestFieldData(saveDataSetTemp, tabSelected);
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
        /// This Method inilializes the Snapshot Dict for Disabled fields
        /// </summary>
        private void InitializeRequestDataDictSnapshot()
        {
            try
            {
                dictRequestDataSnapshot["VWAP"] = CONST_FUTURE + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["SedolSymbol"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["ISIN"] = CONST_EQUITYOPTION + "," + CONST_FUTURE;
                dictRequestDataSnapshot["AverageVolume20Day"] = CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["Volume10DAvg"] = CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["PutOrCall"] = CONST_EQUITY + "," + CONST_FUTURE + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["OpenInterest"] = CONST_EQUITY + "," + CONST_FX + "," + CONST_FIXEDINCOME;
                dictRequestDataSnapshot["ExpirationDate"] = CONST_EQUITY + "," + CONST_FX + "," + CONST_FIXEDINCOME;
                dictRequestDataSnapshot["StrikePrice"] = CONST_EQUITY + "," + CONST_FUTURE + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["TradeVolume"] = CONST_FX;
                dictRequestDataSnapshot["BidSize"] = CONST_FIXEDINCOME;
                dictRequestDataSnapshot["MarketCapitalization "] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["BidExchange"] = CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["AskSize"] = CONST_FIXEDINCOME;
                dictRequestDataSnapshot["AskExchange"] = CONST_FUTURE + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["High52W"] = CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION;
                dictRequestDataSnapshot["Low52W"] = CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION;
                dictRequestDataSnapshot["OpenInterestDelta"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["SharesOutstanding"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["OSIOptionSymbol"] = CONST_EQUITY + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FIXEDINCOME;
                dictRequestDataSnapshot["LastTick"] = CONST_EQUITYOPTION + "," + CONST_FUTURE;
                dictRequestDataSnapshot["DividendYield"] = CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["Dividend"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["DividendInterval"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["XDividendDate"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["AnnualDividend"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["RoundLot"] = CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION + "," + CONST_FIXEDINCOME;
                dictRequestDataSnapshot["UnderlyingSymbol"] = CONST_EQUITY + "," + CONST_FUTURE;
                dictRequestDataSnapshot["Future Contract Size(Multiplier)"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME;
                dictRequestDataSnapshot["Futures Contract Expiration Date"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME;
                dictRequestDataSnapshot["Bloomberg Composite Code"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FIXEDINCOME;
                dictRequestDataSnapshot["Issue Date"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["Coupon"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["MaturityDate"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["Accrual Basis"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["Coupon Frequency"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["Bond type"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["First Coupon Date"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSnapshot["OptionMultiplier"] = CONST_EQUITY + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
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
        /// This Method inilializes the Subscription Dict for Disabled fields
        /// </summary>
        private void InitializeRequestDataDictSubscription()
        {
            try
            {
                dictRequestDataSubscription["SedolSymbol"] = CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["AverageVolume20Day"] = CONST_EQUITYOPTION;
                dictRequestDataSubscription["Volume10DAvg"] = CONST_EQUITYOPTION;
                dictRequestDataSubscription["PutOrCall"] = CONST_EQUITY + "," + CONST_FUTURE + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["OpenInterest"] = CONST_EQUITY + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["ExpirationDate"] = CONST_EQUITY + "," + CONST_FX + "," + CONST_FIXEDINCOME;
                dictRequestDataSubscription["StrikePrice"] = CONST_EQUITY + "," + CONST_FUTURE + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["MarketCapitalization"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["High52W"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["Low52W"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["OpenInterestDelta"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["SharesOutstanding"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["OSIOptionSymbol"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["LastTick"] = CONST_EQUITYOPTION + "," + CONST_FUTURE;
                dictRequestDataSubscription["DividendYield"] = CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["Dividend"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["DividendInterval"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["XDividendDate"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["AnnualDividend"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["RoundLot"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["UnderlyingSymbol"] = CONST_EQUITY + "," + CONST_FX + "," + CONST_FIXEDINCOME;
                dictRequestDataSubscription["Future Contract Size(Multiplier)"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["Futures Contract Expiration Date"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["Bloomberg Composite Code"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["Issue Date"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["Coupon"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["MaturityDate"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FXFORWARD;
                dictRequestDataSubscription["OptionMultiplier"] = CONST_EQUITY + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
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
        /// This Method inilializes the Snapshot Dict for Mandatory fields
        /// </summary>
        private void InitializeDictSnapshotMandateFileds()
        {
            try
            {
                dictSnapshotMandateFields["CusipNo"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FIXEDINCOME;
                dictSnapshotMandateFields["PutOrCall"] = CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION;
                dictSnapshotMandateFields["ExpirationDate"] = CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION;
                dictSnapshotMandateFields["StrikePrice"] = CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION;
                dictSnapshotMandateFields["LastPrice"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictSnapshotMandateFields["Bid"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictSnapshotMandateFields["Ask"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictSnapshotMandateFields["CountryId"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictSnapshotMandateFields["CurrencyCode"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictSnapshotMandateFields["UnderlyingSymbol"] = CONST_EQUITYOPTION + "," + CONST_FUTUREOPTION;
                dictSnapshotMandateFields["Exchange"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION;
                dictSnapshotMandateFields["Bloomberg Composite Code"] = CONST_EQUITY;
                dictSnapshotMandateFields["Future Contract Size(Multiplier)"] = CONST_FUTURE;
                dictSnapshotMandateFields["FullCompanyName"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictSnapshotMandateFields["Issue Date"] = CONST_FIXEDINCOME;
                dictSnapshotMandateFields["Coupon"] = CONST_FIXEDINCOME;
                dictSnapshotMandateFields["MaturityDate"] = CONST_FIXEDINCOME;
                dictSnapshotMandateFields["Accrual Basis"] = CONST_FIXEDINCOME;
                dictSnapshotMandateFields["Coupon Frequency"] = CONST_FIXEDINCOME;
                dictSnapshotMandateFields["Bond type"] = CONST_FIXEDINCOME;
                dictSnapshotMandateFields["First Coupon Date"] = CONST_FIXEDINCOME;
                dictSnapshotMandateFields["Futures Contract Expiration Date"] = CONST_FUTURE;
                dictSnapshotMandateFields["OptionMultiplier"] = CONST_EQUITYOPTION;
                dictSnapshotMandateFields["EID"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
                dictSnapshotMandateFields["UpdateTime"] = CONST_EQUITY + "," + CONST_EQUITYOPTION + "," + CONST_FUTURE + "," + CONST_FUTUREOPTION + "," + CONST_FX + "," + CONST_FIXEDINCOME + "," + CONST_FXFORWARD;
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
        /// This Method initializes the Row for the Snapshot Grid and disable the selected Request Field
        /// </summary>
        public void ultraGridSAPISnapshot_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                DisableRequestFields(e.Row, dictRequestDataSnapshot);
                SetMandatoryFields(e.Row);
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
        /// This method Disable the request field by row wise
        /// </summary>
        private void DisableRequestFields(UltraGridRow row, Dictionary<string, string> dictRequestData)
        {
            try
            {
                if(row!=null && dictRequestData.Count > 0)
                {
                    string column = row.Cells[CONST_NIRVANAFIELDS].Value.ToString();
                    if (dictRequestData.ContainsKey(column))
                    {
                        string value = dictRequestData[column];
                        string[] values = value.Split(',');
                        foreach (string str in values)
                        {
                            row.Cells[str].Activation = Activation.Disabled;
                            row.Cells[str].Value = false;
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
        /// This method disable the mandatory columns and reset their values
        /// </summary>
        private void SetMandatoryFields(UltraGridRow row)
        {
            try
            {
                if (row != null && dictSnapshotMandateFields != null && dictSnapshotMandateFields.Count > 0)
                {
                    string column = row.Cells[CONST_NIRVANAFIELDS].Value.ToString();
                    if (dictSnapshotMandateFields.ContainsKey(column))
                    {
                        string value = dictSnapshotMandateFields[column];
                        string[] values = value.Split(',');
                        foreach (string str in values)
                        {
                            row.Cells[str].Activation = Activation.Disabled;
                            row.Cells[str].Value = true;
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
        /// This Method initializes the Row for the Subscription Grid and disable the selected Request Field
        /// </summary>
        public void ultraGridSAPISubscription_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                DisableRequestFields(e.Row, dictRequestDataSubscription);
                SetMandatoryFields(e.Row);
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

        private async System.Threading.Tasks.Task ShowUserQuotaAndPermissions()
        {
            try
            {
                StringBuilder information = new StringBuilder();

                Dictionary<string, string> userInformation = await PricingService2Manager.PricingService2Manager.GetInstance.GetUserInformation();

                if (userInformation.Count > 0)
                {
                    information.AppendLine("User Details: ");
                    information.AppendLine(string.Empty);

                    foreach (KeyValuePair<string, string> info in userInformation)
                    {
                        information.AppendLine(string.Format("{0}: {1}", info.Key, info.Value));
                    }
                }

                List<Dictionary<string, string>> userPermissions = await PricingService2Manager.PricingService2Manager.GetInstance.GetUserPermissionsInformation();

                if (userPermissions.Count == 2)
                {
                    information.AppendLine(string.Empty);
                    information.AppendLine("Realtime Permissions: ");
                    information.AppendLine(string.Empty);

                    // Realtime Permissions
                    foreach (KeyValuePair<string, string> permission in userPermissions[0])
                    {
                        information.AppendLine(string.Format("{0}: {1}", permission.Key, permission.Value));
                    }

                    information.AppendLine(string.Empty);
                    information.AppendLine("Delayed Permissions: ");
                    information.AppendLine(string.Empty);

                    // Delayed Permissions
                    foreach (KeyValuePair<string, string> permission in userPermissions[1])
                    {
                        information.AppendLine(string.Format("{0}: {1}", permission.Key, permission.Value));
                    }
                }

                textBoxUserQuotaAndPermissions.Text = information.ToString();
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

        private void ViewChanged()
        {
            try
            {
                switch (_currentViewSelection)
                {
                    case ViewSelection.SymbolSubscription:
                        ultraGridMarketDataSymbolSubscription.Visible = true;
                        textBoxUserQuotaAndPermissions.Visible = false;
                        ultraPanelSAPITabsContainer.Visible = false;
                        ultraPanelSubscribedSymbols.Visible = true;
                        labelMandatoryFields.Visible = false;
                        break;
                    case ViewSelection.UserQuotaAndPermission:
                        ultraGridMarketDataSymbolSubscription.Visible = false;
                        textBoxUserQuotaAndPermissions.Visible = true;
                        ultraPanelSAPITabsContainer.Visible = false;
                        ultraPanelSubscribedSymbols.Visible = true;
                        labelMandatoryFields.Visible = false;
                        break;
                    case ViewSelection.SAPIRequestField:
                        ultraGridMarketDataSymbolSubscription.Visible = false;
                        textBoxUserQuotaAndPermissions.Visible = false;
                        ultraPanelSAPITabsContainer.Visible = true;
                        ultraPanelSubscribedSymbols.Visible = false;
                        labelMandatoryFields.Visible = true;
                        break;
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
        #endregion

        #region UI Methods
        private void ultraButtonConnectionProperties_Click(object sender, EventArgs e)
        {
            try
            {
                if (_feedProvider == MarketDataProvider.FactSet)
                {
                    _marketDataProperties = new FactSetProperties();
                    ((FactSetProperties)_marketDataProperties).CredentialsUpdated += marketDataProperties_CredentialsUpdated;
                }
                else if (_feedProvider == MarketDataProvider.ACTIV)
                {
                    _marketDataProperties = new ActivProperties();
                    ((ActivProperties)_marketDataProperties).CredentialsUpdated += marketDataProperties_CredentialsUpdated;
                }
                else if (_feedProvider == MarketDataProvider.SAPI)
                {
                    _marketDataProperties = new SAPIProperties();
                    ((SAPIProperties)_marketDataProperties).CredentialsUpdated += marketDataProperties_CredentialsUpdated;
                }
                _marketDataProperties.FormClosed += marketDataProperties_FormClosed;
                _marketDataProperties.ShowDialog();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private async void ultraButtonViewSubscribedSymbols_Click(object sender, EventArgs e)
        {
            try
            {
                ultraButtonViewSubscribedSymbols.Enabled = false;
                _currentViewSelection = ViewSelection.SymbolSubscription;
                ViewChanged();

                await ShowSubscribedSymbols();
                ultraButtonViewSubscribedSymbols.Enabled = true;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private async void ultraButtonUserQuotaPermissions_Click(object sender, EventArgs e)
        {
            try
            {
                _currentViewSelection = ViewSelection.UserQuotaAndPermission;
                ViewChanged();

                await ShowUserQuotaAndPermissions();
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
        /// This method changed the View of Main UI and Visible the SAPI grids
        /// </summary>
        private async void ultraButtonSAPIRequestFields_Click(object sender, EventArgs e)
        {
            try
            {
                _currentViewSelection = ViewSelection.SAPIRequestField;
                ViewChanged();

                await GetSAPIFieldData();
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

        private async void refreshMarketDataSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await RefreshSymbol();
        }

        private async void refreshAllSymbolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await RefreshAllSymbols();
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }

        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        private void clearFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ColumnFilter colFilters in ultraGridMarketDataSymbolSubscription.DisplayLayout.Bands[0].ColumnFilters)
                {
                    colFilters.ClearFilterConditions();
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

        private void marketDataProperties_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_feedProvider == MarketDataProvider.FactSet)
                    ((FactSetProperties)_marketDataProperties).CredentialsUpdated -= marketDataProperties_CredentialsUpdated;
                else if (_feedProvider == MarketDataProvider.ACTIV)
                    ((ActivProperties)_marketDataProperties).CredentialsUpdated -= marketDataProperties_CredentialsUpdated;
                else if (_feedProvider == MarketDataProvider.SAPI)
                    ((SAPIProperties)_marketDataProperties).CredentialsUpdated -= marketDataProperties_CredentialsUpdated;

                _marketDataProperties.FormClosed -= marketDataProperties_FormClosed;
                _marketDataProperties = null;
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private async void marketDataProperties_CredentialsUpdated(object sender, EventArgs<List<string>> e)
        {
            try
            {
                await PricingService2Manager.PricingService2Manager.GetInstance.RestartLiveFeed();
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraGridMarketDataSymbolSubscription_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    System.Drawing.Point mousePoint = new System.Drawing.Point(e.X, e.Y);
                    UIElement element = ((UltraGrid)sender).DisplayLayout.UIElement.ElementFromPoint(mousePoint);
                    if (element == null)
                    {
                        ultraGridMarketDataSymbolSubscription.ActiveRow = null;
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
        #endregion
    }
}
