using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.ImportExportUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Prana.Import.Controls
{
    public partial class ctrlSymbolManagement : UserControl
    {
        #region local variables
        //ctrlSymbolMismatch _ctrlSymbolMismatch;
        Form _frmSymbolMismatch;
        private List<UltraGridRow> _selectedColumnList = new List<UltraGridRow>();
        static int _userID = int.MinValue;
        static string _symbolManagemantFilePath = string.Empty;
        static string _symbolManagemantLayoutDirectoryPath = string.Empty;
        public static event EventHandler launchForm;
        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        internal event EventHandler autoReRunSymbolValidation;
        internal ISecurityMasterServices secMasterService = null;
        public delegate void SMQMsgInvokeDelegate(object sender, EventArgs<QueueMessage> e);
        internal bool _isApprovingInProgess = false;
        #endregion

        public ctrlSymbolManagement()
        {
            InitializeComponent();

            btnValidate.Visible = false;
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnValidate.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnValidate.ForeColor = System.Drawing.Color.White;
                btnValidate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnValidate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnValidate.UseAppStyling = false;
                btnValidate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSymbolLookUp.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSymbolLookUp.ForeColor = System.Drawing.Color.White;
                btnSymbolLookUp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSymbolLookUp.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSymbolLookUp.UseAppStyling = false;
                btnSymbolLookUp.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExport.ForeColor = System.Drawing.Color.White;
                btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExport.UseAppStyling = false;
                btnExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSaveLayout.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSaveLayout.ForeColor = System.Drawing.Color.White;
                btnSaveLayout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSaveLayout.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSaveLayout.UseAppStyling = false;
                btnSaveLayout.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnApprove.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnApprove.ForeColor = System.Drawing.Color.White;
                btnApprove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnApprove.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnApprove.UseAppStyling = false;
                btnApprove.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void grdReport_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraWinGridUtils.EnableFixedFilterRow(e);
                grdReport.DisplayLayout.Override.HeaderCheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                //Set the HeaderCheckBoxAlignment so the CheckBox will appear to the Right of the caption. 
                grdReport.DisplayLayout.Override.HeaderCheckBoxAlignment = HeaderCheckBoxAlignment.Right;
                //Set the HeaderCheckBoxSynchronization so all rows within the GridBand will be synchronized with the CheckBox 
                grdReport.DisplayLayout.Override.HeaderCheckBoxSynchronization = HeaderCheckBoxSynchronization.None;

                // Set the RowSelectorHeaderStyle to ColumnChooserButton.
                grdReport.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                // Enable the RowSelectors. This is necessary because the column chooser
                // button is displayed over the row selectors in the column headers area.
                grdReport.DisplayLayout.Override.RowSelectors = DefaultableBoolean.True;

                UltraGridBand grdDataBand = null;
                grdDataBand = grdReport.DisplayLayout.Bands[0];
                string[] array = { "Select", "RequestedSymbol", "DataSource", ApplicationConstants.SymbologyCodes.TickerSymbol.ToString(), "Comments", OrderFields.PROPERTY_LONGNAME, OrderFields.PROPERTY_CURRENCYID, ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString(), ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString(), ApplicationConstants.SymbologyCodes.ISINSymbol.ToString(), ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString(), ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString(), ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString(), OrderFields.PROPERTY_ASSET_ID, ApplicationConstants.CONST_SEC_APPROVED_STATUS, "ApprovedBy", "Accounts", "ThirdParty", "btnSymbolLookUp", "btnCreate", "CreatedBy", "CreationDate" };
                List<string> lstColumns = new List<string>(array);

                //add all the columns to the grid given in lstColumns
                SetupDefaultGridColumns(grdDataBand, lstColumns);
                SetGridColumns(grdDataBand);
                SetValueInTextBoxes();

                // load the layout file if it exists
                LoadReportSaveLayoutXML();
                foreach (string column in lstColumns)
                {
                    if (!grdDataBand.Columns.Exists(column))
                    {
                        grdDataBand.Columns.Add(column);
                    }
                    grdDataBand.Columns[column].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                }

                //if (SymbolManagementLayout.SymbolManagementColumns.Count > 0)
                //{
                //    List<ColumnData> listColData = SymbolManagementLayout.SymbolManagementColumns;
                //    SetGridColumnLayout(grdReport, listColData);
                //}
                //make check box editable
                UltraGridColumn ColSelect = grdDataBand.Columns["Select"];
                ColSelect.CellActivation = Activation.AllowEdit;
                // grdDataBand.Columns["Comments"].Hidden = true;
                // grdDataBand.Columns["Comments"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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
        /// Sets count of validation from api from secmaster
        /// </summary>
        private void SetValueInTextBoxes()
        {
            try
            {
                int countValidationsAPI = 0;
                int countValidationsSecMaster = 0;
                foreach (UltraGridRow row in grdReport.Rows)
                {
                    if (row.Band.Columns.Exists("DataSource") && row.Cells["DataSource"].Value != null && !string.IsNullOrEmpty(row.Cells["DataSource"].Value.ToString()))
                    {
                        if (row.Cells["DataSource"].Text.ToUpper().Equals("SEC MASTER"))
                            countValidationsSecMaster++;
                        if (row.Cells["DataSource"].Text.ToUpper().Equals("API"))
                            countValidationsAPI++;
                    }
                }
                txtValidationAPI.Value = countValidationsAPI;
                txtValidationSecMaster.Value = countValidationsSecMaster;
                //[Import] Proceed to validation is not showing correct number of total symbols
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-2055
                txtTotalSymbol.Value = grdReport.Rows.Count;// countValidationsAPI + countValidationsSecMaster + Convert.ToInt32(txtFailedValidation.Value);

                txtFailedValidation.Value = grdReport.Rows.Count - (countValidationsSecMaster + countValidationsAPI);
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

        private void btnSymbolLookUp_Click(object sender, EventArgs e)
        {
            try
            {
                launchForm = ImportManager.Instance.GetLaunchForm();
                if (launchForm != null)
                {
                    ListEventAargs args = new ListEventAargs();
                    args.listOfValues.Add(ApplicationConstants.CONST_SYMBOL_LOOKUP.ToString());
                    launchForm(this.FindForm(), args);
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
        /// Approve the selected securities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSymbolNotApproved = false;
                //bool isSecAlreadyApproved = false;
                SecMasterbaseList lst = new SecMasterbaseList();
                lst.RequestID = System.Guid.NewGuid().ToString();
                StringBuilder approvedSecurities = new StringBuilder();
                //Modified By Faisal Shah 08/08/14
                //Need to Select only Filtered Rows
                foreach (UltraGridRow row in grdReport.Rows.GetFilteredInNonGroupByRows())
                {
                    bool value = Convert.ToBoolean(row.Cells["Select"].Text);
                    if (value == true)
                    {
                        DataRowView dr = row.ListObject as DataRowView;

                        if (dr != null)
                        {
                            SecMasterUIObj uiObj = new SecMasterUIObj();
                            Transformer.CreateObjThroughReflection(dr.Row, uiObj);
                            if (!uiObj.IsSecApproved)
                            {
                                #region Validate security before save
                                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1333
                                if (!ValidateSymbolForSave(uiObj))
                                {
                                    isSymbolNotApproved = true;
                                    //do not return validate other securities if they are valid.
                                    continue;
                                }
                                #endregion

                                //symbolCountForApprove++;
                                //Setting approval related properties
                                uiObj.ApprovalDate = DateTime.Now;
                                //modified by omshiv, 8 May 2014
                                uiObj.ApprovedBy = CachedDataManager.GetInstance.LoggedInUser.ShortName + "_" + CachedDataManager.GetInstance.LoggedInUser.CompanyName; //_loggedInUserId;
                                uiObj.IsSecApproved = true;

                                SecMasterBaseObj secMasterBaseObj = GetSecMasterObjFromUIObject(uiObj);


                                if (secMasterBaseObj != null)
                                {
                                    //setting source of sec Master data.
                                    secMasterBaseObj.SourceOfData = SecMasterConstants.SecMasterSourceOfData.SymbolLookup;
                                    secMasterBaseObj.SymbolType = (int)BusinessObjects.AppConstants.SymbolType.Updated;


                                    lst.Add(secMasterBaseObj);
                                    if (secMasterBaseObj.IsSecApproved && secMasterBaseObj.AUECID > 0)
                                    {
                                        row.Cells["IsSecApproved"].Value = "True";
                                        row.Cells[ApplicationConstants.CONST_SEC_APPROVED_STATUS].Value = "Approved";
                                        //modified by Bhavana at Jira : CHMW-910
                                        if (!string.IsNullOrEmpty(uiObj.ApprovedBy))
                                            row.Cells["ApprovedBy"].Value = uiObj.ApprovedBy;
                                    }
                                }

                            }
                            else
                            {
                                approvedSecurities.Append(uiObj.TickerSymbol.ToString() + " ," + " ");
                            }
                        }
                    }
                }
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1333
                if (isSymbolNotApproved)
                {
                    MessageBox.Show("Some securities are not approved successfully because of incomplete data. \nPlease check details on Security Master for security.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (lst.Count > 0)
                {
                    _isApprovingInProgess = true;
                    ultraStatusBarSymbolMgt.Text = lst.Count + " security/securities approving..";
                    //MessageBox.Show(lst.Count + " security/securities aprroving..", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SecurityMasterManager.Instance.SecurityMaster.SaveNewSymbols(lst);
                }
                else if (approvedSecurities.Length > 0)
                {
                    approvedSecurities.Remove(approvedSecurities.Length - 2, 1);
                    MessageBox.Show(approvedSecurities + "is/are already approved", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No security selected for approval", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// To save layout of symbol management report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                //SaveGridsLayout();
                if (grdReport != null)
                {
                    if (grdReport.DisplayLayout.Bands[0].Columns.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(_symbolManagemantFilePath))
                        {
                            grdReport.DisplayLayout.SaveAsXml(_symbolManagemantFilePath);
                        }
                        //SymbolManagementLayout.SymbolManagementColumns = GetGridColumnLayout(grdReport);
                    }
                }
                MessageBox.Show("Symbol management layout saved", "Symbol Management", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //string filePath = @"D:\Nirvana\NirvanaCode\SourceCode\Dev\Prana\Application\Prana.Client\Prana\bin\Debug\ReconData";

                UltraGridFileExporter.LoadFilePathAndExport(grdReport, this);
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
        /// set data in symbol management UI
        /// </summary>
        /// <param name="dictReportData"></param>
        internal void FillData(Dictionary<string, object> dictReportData)
        {
            //Dictionary<string, List<string>> dictAccounts = new Dictionary<string, List<string>>();
            //Dictionary<string, List<string>> dictThirdParties = new Dictionary<string, List<string>>();

            try
            {
                if (secMasterService != null && secMasterService.IsConnected)
                {

                    secMasterService.ResponseCompleted += secMasterService_ResponseCompleted;
                }
                DataTable dtMain = new DataTable();
                //call GetAllDefaults only when instance is created
                //Otherwise value lists will be filled each time
                SecMasterHelper.getInstance().GetAllDefaults();

                if (dictReportData.ContainsKey("StartDate"))
                {
                    DateTime dStartDate = DateTime.MinValue;
                    bool isParsed = DateTime.TryParse(dictReportData["StartDate"].ToString(), out dStartDate);
                    if (isParsed && dStartDate > DateTimeConstants.MinValue)
                    {
                        dtStartDate.Value = dStartDate;
                    }
                }

                if (dictReportData.ContainsKey("EndDate"))
                {
                    DateTime dEndDate = DateTime.MinValue;
                    bool isParsed = DateTime.TryParse(dictReportData["EndDate"].ToString(), out dEndDate);
                    if (isParsed && dEndDate > DateTimeConstants.MinValue)
                    {
                        dtEndDate.Value = dEndDate;
                    }
                }

                //TotalSymbols will set from all secmaster+api symbols
                //if (dictReportData.ContainsKey("TotalSymbols"))
                //{
                //    txtTotalSymbol.Value = Convert.ToInt32(dictReportData["TotalSymbols"].ToString());
                //}

                //http://jira.nirvanasolutions.com:8080/browse/CHMW-2055
                // [Import] Proceed to validation is not showing correct number of total symbols
                //if (dictReportData.ContainsKey("NonValidatedSymbols"))
                //{
                //    txtFailedValidation.Value = Convert.ToInt32(dictReportData["NonValidatedSymbols"].ToString());
                //}

                if (dictReportData.ContainsKey("FileName"))
                {
                    string[] xmlFiles = dictReportData["FileName"].ToString().Split(',');

                    foreach (string xmlFilePath in xmlFiles)
                    {

                        string[] xmlFileWithThirdParty = xmlFilePath.Split(Seperators.SEPERATOR_6);
                        //since we have used separator 6 in execution name so file name will be retrieved after combining xmlFileWithThirdParty[0] + xmlFileWithThirdParty[1]
                        if (File.Exists(xmlFileWithThirdParty[0] + Seperators.SEPERATOR_6 + xmlFileWithThirdParty[1]))
                        {
                            DataSet ds = new DataSet();
                            using (FileStream filestream = File.OpenRead(xmlFileWithThirdParty[0] + Seperators.SEPERATOR_6 + xmlFileWithThirdParty[1]))
                            {
                                BufferedStream buffered = new BufferedStream(filestream);
                                ds.ReadXml(buffered);
                            }
                            //ds.ReadXml(xmlFileWithThirdParty[0] + Seperators.SEPERATOR_6 + xmlFileWithThirdParty[1]);
                            if (ds.Tables.Count > 0)
                            {
                                //We have to copy data first in another datatable as ds have relationships
                                //RequestedSymbol and symbology are used here to make primary key 
                                DataTable dt = ds.Tables[0].Copy();
                                if (!dt.Columns.Contains("ThirdParty"))
                                {
                                    dt.Columns.Add("ThirdParty");
                                }
                                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                                {
                                    DataRow row = dt.Rows[i];
                                    if (!string.IsNullOrEmpty(xmlFileWithThirdParty[2].ToString()))
                                    {
                                        row["ThirdParty"] = xmlFileWithThirdParty[2].ToString();

                                    }
                                    if (dt.Columns.Contains("Accounts") && !string.IsNullOrEmpty(row["Accounts"].ToString()) && row["Accounts"].ToString().Length > 0)
                                    {
                                        row["Accounts"] = row["Accounts"].ToString().Substring(0, (row["Accounts"].ToString().Length - 1));
                                    }
                                    if (dt.Columns.Contains("RequestedSymbol") && (row["RequestedSymbol"] == System.DBNull.Value || string.IsNullOrEmpty(row["RequestedSymbol"].ToString())) || (row["RequestedSymbology"] == System.DBNull.Value || string.IsNullOrEmpty(row["RequestedSymbology"].ToString())))
                                    {
                                        dt.Rows[i].Delete();
                                    }
                                }
                                //added and commented by: Bharat Raturi
                                #region commented
                                //foreach (DataRow row in dt.Rows)
                                //{
                                //    string key=string.Empty; 
                                //    string valueThirdParty=string.Empty; 
                                //    string valueAccount=string.Empty;
                                //    if ((dt.Columns.Contains("RequestedSymbol") && !string.IsNullOrEmpty(row["RequestedSymbol"].ToString()) && row["RequestedSymbol"].ToString().Length > 0) && (dt.Columns.Contains("RequestedSymbology") && !string.IsNullOrEmpty(row["RequestedSymbology"].ToString()) && row["RequestedSymbology"].ToString().Length > 0))
                                //    {
                                //        key = row["RequestedSymbol"].ToString() + Seperators.SEPERATOR_6.ToString() + row["RequestedSymbology"].ToString();
                                //    }
                                //    if (dt.Columns.Contains("ThirdParty") && !string.IsNullOrEmpty(row["ThirdParty"].ToString()) && row["ThirdParty"].ToString().Length > 0)
                                //    {
                                //        valueThirdParty = row["ThirdParty"].ToString();
                                //    }

                                //    valueAccount = row["Accounts"].ToString();

                                //    //if (dictThirdParties.ContainsKey(key))
                                //    //{
                                //    //    dictThirdParties[key].Add(valueThirdParty);
                                //    //}
                                //    //else
                                //    //{
                                //    //    List<string> listThirdParties = new List<string>();
                                //    //    listThirdParties.Add(valueThirdParty);
                                //    //    dictThirdParties.Add(key, listThirdParties);
                                //    //}
                                //    //if (dictAccounts.ContainsKey(key))
                                //    //{
                                //    //    dictAccounts[key].Add(valueAccount);
                                //    //}
                                //    //else
                                //    //{
                                //    //    List<string> listAccounts = new List<string>();
                                //    //    listAccounts.Add(valueThirdParty);
                                //    //    dictAccounts.Add(key, listAccounts);
                                //    //}
                                //}

                                #endregion

                                if (dt.Columns.Contains("RequestedSymbol") && dt.Columns.Contains("RequestedSymbology"))
                                {
                                    DataColumn[] columns = new DataColumn[3];
                                    columns[0] = dt.Columns["RequestedSymbol"];
                                    columns[1] = dt.Columns["RequestedSymbology"];
                                    columns[2] = dt.Columns["ThirdParty"];
                                    dt.PrimaryKey = columns;
                                }
                                //schema for each datatable may be different beacuse of symbol validation
                                dtMain.Merge(dt, true, MissingSchemaAction.Add);

                            }
                        }
                    }
                }
                if (dtMain.Columns.Contains("RequestedSymbol") && dtMain.Columns.Contains("RequestedSymbology") && dtMain.Columns.Contains("ThirdParty"))
                {
                    string[] param = { "RequestedSymbol", "RequestedSymbology", "ThirdParty" };
                    dtMain.DefaultView.ToTable(true, param);
                }
                //added by: Bharat Raturi, 27 may 2014
                //purpose: to show the third party and the account clubbed for any symbol, symbology group
                #region show multiple thirdparty, account
                DataTable dtNewMain = dtMain.Clone();
                bool isAdded = false;
                foreach (DataRow dr in dtMain.Rows)
                {
                    isAdded = false;
                    string symbol1 = string.Empty;
                    string symbol2 = string.Empty;
                    string symbology1 = string.Empty;
                    string symbology2 = string.Empty;

                    if (!string.IsNullOrWhiteSpace(dr["RequestedSymbol"].ToString()))
                    {
                        symbol1 = dr["RequestedSymbol"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(dr["RequestedSymbology"].ToString()))
                    {
                        symbology1 = dr["RequestedSymbology"].ToString();
                    }

                    foreach (DataRow dr1 in dtNewMain.Rows)
                    {
                        if (!string.IsNullOrWhiteSpace(dr1["RequestedSymbol"].ToString()))
                        {
                            symbol2 = dr1["RequestedSymbol"].ToString();
                        }
                        if (!string.IsNullOrWhiteSpace(dr1["RequestedSymbology"].ToString()))
                        {
                            symbology2 = dr1["RequestedSymbology"].ToString();
                        }
                        if (symbol1.Equals(symbol2) && symbology1.Equals(symbology2))
                        {
                            string thirdParty1 = string.Empty;
                            string thirdParty2 = string.Empty;
                            string account1 = string.Empty;
                            string account2 = string.Empty;

                            thirdParty1 = dr["ThirdParty"].ToString();
                            thirdParty2 = dr1["ThirdParty"].ToString();
                            account1 = dr["Accounts"].ToString();
                            account2 = dr1["Accounts"].ToString();
                            if (!account1.Equals(account2))
                            {
                                dr1["Accounts"] = account1 + "," + account2;
                            }
                            if (!thirdParty1.Equals(thirdParty2))
                            {
                                dr1["ThirdParty"] = thirdParty1 + "," + thirdParty2;
                            }
                            isAdded = true;
                        }
                    }
                    if (!isAdded)
                    {
                        dtNewMain.ImportRow(dr);
                    }
                }
                grdReport.DataSource = dtNewMain;
                #endregion
                //commented by: Bharat Raturi, 27 may 2014
                //purpose: to change the data source of the grid
                //grdReport.DataSource = dtMain;
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

        void secMasterService_ResponseCompleted(object sender, EventArgs<QueueMessage> e)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (this.InvokeRequired)
                    {
                        SMQMsgInvokeDelegate invokeDelegate = new SMQMsgInvokeDelegate(secMasterService_ResponseCompleted);
                        this.BeginInvoke(invokeDelegate, new object[] { sender, e });
                    }
                    else
                    {
                        string message = e.Value.Message.ToString();
                        ultraStatusBarSymbolMgt.Text = string.Empty;
                        if (message.Contains("Success"))
                        {
                            if (_isApprovingInProgess)
                            {
                                MessageBox.Show("Selected security(s) has been approved successfully. Details are updating, please wait to complete.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                if (autoReRunSymbolValidation != null)
                                {
                                    autoReRunSymbolValidation(this, null);
                                }
                                _isApprovingInProgess = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Security approving failed. Please try again later.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Set layout for grid columns
        /// </summary>
        /// <param name="gridBand"></param>
        private void SetGridColumns(UltraGridBand gridBand)
        {

            try
            {
                int visiblePosition = 0;



                UltraGridColumn ColSelect = gridBand.Columns["Select"];
                ColSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                // ColSelect.
                ColSelect.Header.Caption = "";
                ColSelect.DataType = typeof(Boolean);
                ColSelect.Header.Column.Width = 10;
                ColSelect.Header.VisiblePosition = visiblePosition++;
                ColSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                ColSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                ColSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
                ColSelect.AllowRowFiltering = DefaultableBoolean.False;
                ColSelect.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                ColSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;

                UltraGridColumn ColRequestedSymbol = gridBand.Columns["RequestedSymbol"];
                ColRequestedSymbol.Header.VisiblePosition = visiblePosition++;
                ColRequestedSymbol.Header.Column.Width = 100;
                ColRequestedSymbol.Header.Caption = "Upload Symbol";
                ColRequestedSymbol.CharacterCasing = CharacterCasing.Upper;
                ColRequestedSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColRequestedSymbol.NullText = String.Empty;
                ColRequestedSymbol.SortIndicator = SortIndicator.Ascending;
                ColRequestedSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColSourceOfData = gridBand.Columns["DataSource"];
                ColSourceOfData.Header.VisiblePosition = visiblePosition++;
                ColSourceOfData.Header.Column.Width = 100;
                ColSourceOfData.Header.Caption = "Source of validation";
                ColSourceOfData.CharacterCasing = CharacterCasing.Upper;
                ColSourceOfData.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColSourceOfData.NullText = String.Empty;
                ColSourceOfData.SortIndicator = SortIndicator.Ascending;
                //we are making clone because there was error we cannot add same valuelist again
                ColSourceOfData.ValueList = SecMasterHelper.getInstance().SourceOfData.Clone();
                //ColSourceOfData.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.FormattedText;
                ColSourceOfData.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColTickerSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.TickerSymbol.ToString()];
                ColTickerSymbol.Header.VisiblePosition = visiblePosition++;
                ColTickerSymbol.Header.Column.Width = 100;
                ColTickerSymbol.Header.Caption = OrderFields.CAPTION_TICKERSYMBOL;
                ColTickerSymbol.CharacterCasing = CharacterCasing.Upper;
                ColTickerSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColTickerSymbol.NullText = String.Empty;
                ColTickerSymbol.SortIndicator = SortIndicator.Ascending;
                ColTickerSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColComments = gridBand.Columns["Comments"];
                // ColComments.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                ColComments.Width = 50;
                ColComments.Header.VisiblePosition = visiblePosition++;
                ColComments.Header.Caption = "Comments";
                ColComments.NullText = String.Empty;
                ColComments.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                ColComments.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColLongName = gridBand.Columns[OrderFields.PROPERTY_LONGNAME];
                ColLongName.Width = 100;
                ColLongName.Header.VisiblePosition = visiblePosition++;
                ColLongName.Header.Column.Width = 150;
                ColLongName.Header.Caption = "Description";
                ColLongName.CharacterCasing = CharacterCasing.Upper;
                ColLongName.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColLongName.NullText = String.Empty;
                ColLongName.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColCurrency = gridBand.Columns[OrderFields.PROPERTY_CURRENCYID];
                ColCurrency.Header.VisiblePosition = visiblePosition++;
                ColCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColCurrency.Header.Caption = "Currency";
                //we are making clone because there was error we cannot add same valuelist again
                ColCurrency.ValueList = SecMasterHelper.getInstance().Currencies.Clone();
                ColCurrency.Header.Column.Width = 70;
                ColCurrency.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColReutresSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.ReutersSymbol.ToString()];
                ColReutresSymbol.Header.VisiblePosition = visiblePosition++;
                ColReutresSymbol.Header.Column.Width = 70;
                ColReutresSymbol.CharacterCasing = CharacterCasing.Upper;
                ColReutresSymbol.Header.Caption = OrderFields.CAPTION_RICSYMBOL;
                ColReutresSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColReutresSymbol.NullText = String.Empty;
                ColReutresSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColBloombergSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.BloombergSymbol.ToString()];
                ColBloombergSymbol.Width = 70;
                ColBloombergSymbol.Header.VisiblePosition = visiblePosition++;
                ColBloombergSymbol.Header.Column.Width = 70;
                ColBloombergSymbol.CharacterCasing = CharacterCasing.Upper;
                ColBloombergSymbol.Header.Caption = OrderFields.CAPTION_BLOOMBERGSYMBOL;
                ColBloombergSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColBloombergSymbol.NullText = String.Empty;
                ColBloombergSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColThirdParty = gridBand.Columns["ThirdParty"];
                ColThirdParty.Width = 70;
                ColThirdParty.Header.VisiblePosition = visiblePosition++;
                ColThirdParty.Header.Column.Width = 70;
                ColThirdParty.CharacterCasing = CharacterCasing.Upper;
                ColThirdParty.Header.Caption = "Third Party";
                ColThirdParty.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColThirdParty.NullText = String.Empty;
                ColThirdParty.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColAccounts = gridBand.Columns["Accounts"];
                ColAccounts.Width = 70;
                ColAccounts.Header.VisiblePosition = visiblePosition++;
                ColAccounts.Header.Column.Width = 70;
                ColAccounts.CharacterCasing = CharacterCasing.Upper;
                ColAccounts.Header.Caption = "Fund / Account";
                ColAccounts.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColAccounts.NullText = String.Empty;
                ColAccounts.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColCusipSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.CUSIPSymbol.ToString()];
                ColCusipSymbol.Width = 70;
                ColCusipSymbol.Header.VisiblePosition = visiblePosition++;
                ColCusipSymbol.Header.Column.Width = 70;
                ColCusipSymbol.CharacterCasing = CharacterCasing.Upper;
                ColCusipSymbol.Header.Caption = OrderFields.CAPTION_CUSIPSYMBOL;
                ColCusipSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColCusipSymbol.NullText = String.Empty;
                ColCusipSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColISINSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.ISINSymbol.ToString()];
                ColISINSymbol.Header.VisiblePosition = visiblePosition++;
                ColISINSymbol.Header.Column.Width = 70;
                ColISINSymbol.CharacterCasing = CharacterCasing.Upper;
                ColISINSymbol.Header.Caption = OrderFields.CAPTION_ISINSYMBOL;
                ColISINSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColISINSymbol.NullText = String.Empty;
                ColISINSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColSEDOLSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.SEDOLSymbol.ToString()];
                ColSEDOLSymbol.Header.VisiblePosition = visiblePosition++;
                ColSEDOLSymbol.Header.Column.Width = 70;
                ColSEDOLSymbol.CharacterCasing = CharacterCasing.Upper;
                ColSEDOLSymbol.Header.Caption = OrderFields.CAPTION_SEDOLSYMBOL;
                ColSEDOLSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColSEDOLSymbol.NullText = String.Empty;
                ColSEDOLSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColIDCOOptionSymbol = gridBand.Columns[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol.ToString()];
                ColIDCOOptionSymbol.Header.VisiblePosition = visiblePosition++;
                ColIDCOOptionSymbol.Header.Column.Width = 150;
                ColIDCOOptionSymbol.CharacterCasing = CharacterCasing.Upper;
                ColIDCOOptionSymbol.Header.Caption = OrderFields.CAPTION_IDCOOPTIONSYMBOL;
                ColIDCOOptionSymbol.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColIDCOOptionSymbol.NullText = String.Empty;
                ColIDCOOptionSymbol.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn colAsset = gridBand.Columns[OrderFields.PROPERTY_ASSET_ID];
                colAsset.Header.VisiblePosition = visiblePosition++;
                colAsset.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colAsset.Header.Caption = OrderFields.CAPTION_ASSET_CLASS;
                //we are making clone because there was error we cannot add same valuelist again
                colAsset.ValueList = SecMasterHelper.getInstance().Assets.Clone();
                colAsset.Header.Column.Width = 70;
                colAsset.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;


                //we are showing Approval Status based on isSecApproved property.
                UltraGridColumn colApprovalStatus = gridBand.Columns[ApplicationConstants.CONST_SEC_APPROVED_STATUS];
                colApprovalStatus.Header.Caption = "Approval Status";
                //colApprovalStatus.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                //  colApprovalStatus.ValueList = _approvalSatus;
                colApprovalStatus.Header.Column.Width = 70;
                colApprovalStatus.Header.VisiblePosition = visiblePosition++;
                colApprovalStatus.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColApprovedBy = gridBand.Columns["ApprovedBy"];
                ColApprovedBy.Header.Caption = "Last Approved By ";
                ColApprovedBy.Header.Column.Width = 100;
                ColApprovedBy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColApprovedBy.ValueList = GetValueList(CachedDataManager.GetInstance.GetAllUsersName());
                ColApprovedBy.Header.VisiblePosition = visiblePosition++;
                ColApprovedBy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn colBtnAttemptValidation = gridBand.Columns["btnSymbolLookUp"];
                colBtnAttemptValidation.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colBtnAttemptValidation.Width = 50;
                colBtnAttemptValidation.Header.VisiblePosition = visiblePosition++;
                colBtnAttemptValidation.Header.Caption = "Security Master";
                colBtnAttemptValidation.NullText = "Symbol Lookup";
                colBtnAttemptValidation.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colBtnAttemptValidation.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn colBtnCreate = gridBand.Columns["btnCreate"];
                colBtnCreate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colBtnCreate.Width = 50;
                colBtnCreate.Header.VisiblePosition = visiblePosition++;
                colBtnCreate.Header.Caption = "Create Security";
                colBtnCreate.NullText = "Create Security";
                colBtnCreate.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colBtnCreate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;


                // Modified by Bhavana
                // Purpose : To show created by and creation date fields in report
                UltraGridColumn ColCreatedBy = gridBand.Columns["CreatedBy"];
                ColCreatedBy.Header.Caption = "Created By";
                ColCreatedBy.Header.Column.Width = 100;
                ColCreatedBy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                ColCreatedBy.ValueList = GetValueList(CachedDataManager.GetInstance.GetAllUsersName());
                ColCreatedBy.Header.VisiblePosition = visiblePosition++;
                ColCreatedBy.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;

                UltraGridColumn ColCreatedDate = gridBand.Columns["CreationDate"];
                ColCreatedDate.Header.Caption = "Creation Date";
                ColCreatedDate.Header.Column.Width = 70;
                ColCreatedDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
                ColCreatedDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.EmptyString;
                ColCreatedDate.NullText = String.Empty;
                ColCreatedDate.Header.VisiblePosition = visiblePosition++;
                ColCreatedDate.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;


                foreach (UltraGridColumn column in gridBand.Columns)
                {
                    column.CellActivation = Activation.NoEdit;
                    column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                }
                ColSelect.CellActivation = Activation.AllowEdit;
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
        /// Get value list for column
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private ValueList GetValueList(Dictionary<int, string> values)
        {
            ValueList list = new ValueList();
            list.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
            foreach (KeyValuePair<int, string> value in values)
            {
                list.ValueListItems.Add(value.Key, value.Value);
            }
            return list;
        }

        /// <summary>
        /// add default columns and hide rest of the columns
        /// </summary>
        /// <param name="band"></param>
        /// <param name="lstColumns"></param>
        private void SetupDefaultGridColumns(UltraGridBand band, List<string> lstColumns)
        {
            try
            {
                foreach (string column in lstColumns)
                {
                    if (!band.Columns.Exists(column))
                    {
                        band.Columns.Add(column);
                    }
                }
                foreach (UltraGridColumn column in band.Columns)
                {
                    if (!lstColumns.Contains(column.Key))
                    {
                        column.Hidden = true;
                        column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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

        private void grdReport_BeforeHeaderCheckStateChanged(object sender, Infragistics.Win.UltraWinGrid.BeforeHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                if (grdReport.Rows.Count > 0)
                {
                    // Modified by Ankit Gupta on 16 Oct, 2014.
                    // To clear the list, before adding new selected items.
                    _selectedColumnList.Clear();
                    foreach (UltraGridRow row in grdReport.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["Select"].Value) == true)
                        {
                            _selectedColumnList.Add(row);
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
        private void grdReport_AfterHeaderCheckStateChanged(object sender, Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                if (grdReport.Rows.Count > 0 && grdReport.Rows.GetFilteredOutNonGroupByRows() != null)
                {
                    CheckState state = grdReport.DisplayLayout.Bands[0].Columns["Select"].GetHeaderCheckedState(grdReport.Rows);
                    UltraGridRow[] grdrows = grdReport.Rows.GetFilteredOutNonGroupByRows();
                    if (grdrows.Length > 0 && grdReport.Rows.Count > 0)
                    {
                        foreach (UltraGridRow row in grdrows)
                        {
                            if (state.Equals(CheckState.Checked))
                            {
                                row.Cells["Select"].Value = false;
                            }
                        }
                    }
                    foreach (UltraGridRow row in _selectedColumnList)
                    {
                        row.Cells["Select"].Value = true;
                    }

                    // Modified by Ankit Gupta on 16 Oct, 2014.
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1583

                    if (state == CheckState.Unchecked)
                    {
                        foreach (UltraGridRow row in grdReport.Rows)
                        {
                            row.Cells["Select"].Value = false;
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

        private void grdReport_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                DataRowView dr = e.Cell.Row.ListObject as DataRowView;
                Dictionary<String, String> argDict = new Dictionary<string, string>();

                if (dr != null && e.Cell.Column.Key == "btnCreate")
                {
                    if (dr["AUECID"] != null && !string.IsNullOrEmpty(dr["AUECID"].ToString()) && int.Parse(dr["AUECID"].ToString()) > 0)
                    {
                        MessageBox.Show("Security already exists", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        SecMasterUIObj secMasterUI = new SecMasterUIObj();
                        secMasterUI.BloombergSymbol = dr["BloombergSymbol"].ToString();
                        secMasterUI.TickerSymbol = dr["TickerSymbol"].ToString();
                        if (dr["RequestedSymbology"].ToString() == "5")
                        {
                            argDict.Add("SearchCriteria", SecMasterConstants.SearchCriteria.Bloomberg.ToString());
                        }
                        else
                        {
                            argDict.Add("SearchCriteria", SecMasterConstants.SearchCriteria.Ticker.ToString());

                        }
                        argDict.Add("Action", SecMasterConstants.SecurityActions.ADD.ToString());
                        //creating a args dict for Symbol lookup UI 

                        argDict.Add("SecMaster", binaryFormatter.Serialize(secMasterUI));
                        launchForm = ImportManager.Instance.GetLaunchForm();
                        if (launchForm != null)
                        {
                            ListEventAargs args = new ListEventAargs();

                            args.argsObject = argDict;
                            args.listOfValues.Add(ApplicationConstants.CONST_SYMBOL_LOOKUP.ToString());
                            launchForm(this, args);
                        }
                    }
                }

                if (dr != null && e.Cell.Column.Key == "btnSymbolLookUp")
                {
                    if (dr["AUECID"] != null && !string.IsNullOrEmpty(dr["AUECID"].ToString()) && int.Parse(dr["AUECID"].ToString()) > 0)
                    {
                        if (dr["RequestedSymbology"].ToString() == "5")
                        {
                            argDict.Add("SearchCriteria", SecMasterConstants.SearchCriteria.Bloomberg.ToString());
                            argDict.Add("Symbol", dr["BloombergSymbol"].ToString());
                        }
                        else
                        {
                            argDict.Add("SearchCriteria", SecMasterConstants.SearchCriteria.Ticker.ToString());
                            argDict.Add("Symbol", dr["TickerSymbol"].ToString());
                        }
                        argDict.Add("Action", SecMasterConstants.SecurityActions.SEARCH.ToString());

                        launchForm = ImportManager.Instance.GetLaunchForm();
                        if (launchForm != null)
                        {
                            ListEventAargs args = new ListEventAargs();
                            args.argsObject = argDict;
                            args.listOfValues.Add(ApplicationConstants.CONST_SYMBOL_LOOKUP.ToString());
                            launchForm(this, args);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Security does not exist", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                if (e.Cell.Column.Key == "Comments")
                {
                    DataRowView rowView = e.Cell.Row.ListObject as DataRowView;

                    if (rowView != null)
                    {
                        DataTable dt = rowView.Row.Table.Clone();
                        dt.Rows.Add(rowView.Row.ItemArray);
                        //if form is not created


                        //if form is not created
                        if (_frmSymbolMismatch == null || _frmSymbolMismatch.IsDisposed)
                        {
                            _frmSymbolMismatch = new Form();
                            _frmSymbolMismatch.FormClosed += _frmSymbolMismatch_FormClosed;
                        }
                        else
                        {
                            //else previos form is bring to front
                            _frmSymbolMismatch.BringToFront();
                        }
                        //set the form and grid properties
                        _frmSymbolMismatch.Text = e.Cell.Text;
                        ctrlSymbolMismatch crlSymbolMismatch = new ctrlSymbolMismatch();
                        crlSymbolMismatch.InitializeData(e.Cell.Text, dt);

                        SetThemeAtDynamicForm(_frmSymbolMismatch, crlSymbolMismatch);


                        crlSymbolMismatch.Dock = DockStyle.Fill;
                        _frmSymbolMismatch.Width = 730;
                        _frmSymbolMismatch.Height = 132;
                        _frmSymbolMismatch.Text = "Duplicate Symbol";
                        _frmSymbolMismatch.MaximumSize = new System.Drawing.Size(730, 132);
                        _frmSymbolMismatch.MinimumSize = new System.Drawing.Size(730, 132);

                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1102


                        _frmSymbolMismatch.ShowIcon = false;

                        CustomThemeHelper.SetThemeProperties(_frmSymbolMismatch, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                        _frmSymbolMismatch.Show();

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _frmSymbolMismatch_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_frmSymbolMismatch != null)
                {
                    _frmSymbolMismatch = null;
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

        private void grdReport_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Band.Columns.Exists("IsSecApproved") && e.Row.Band.Columns.Exists(ApplicationConstants.CONST_SEC_APPROVED_STATUS) && !string.IsNullOrEmpty(e.Row.Cells["IsSecApproved"].Text))
                {
                    if (Convert.ToBoolean(e.Row.Cells["IsSecApproved"].Value.ToString()))
                        e.Row.Cells[ApplicationConstants.CONST_SEC_APPROVED_STATUS].Value = "Approved";
                    //Changed By Faisal Gani Shah. As there was no check for a Symbol which does not exist.
                    //So put a check for that and Updated the Approval Status to None.
                    else if (int.Parse(e.Row.Cells[ApplicationConstants.CONST_AUECID].Value.ToString()) <= 0)
                        e.Row.Cells[ApplicationConstants.CONST_SEC_APPROVED_STATUS].Value = "None";
                    else
                        e.Row.Cells[ApplicationConstants.CONST_SEC_APPROVED_STATUS].Value = "Unapproved";
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
        /// sets theme at the form if the `
        /// </summary>
        /// <param name="dynamicForm"></param>
        /// <param name="control"></param>
        private void SetThemeAtDynamicForm(Form dynamicForm, Object control)
        {
            try
            {
                System.ComponentModel.IContainer dynamicComponents = new System.ComponentModel.Container();
                Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
                Infragistics.Win.Misc.UltraPanel dynamicForm_Fill_Panel;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Left;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Right;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Bottom;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Top;
                // 
                // ultraToolbarsManager1
                // 
                ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(dynamicComponents);
                dynamicForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
                dynamicForm_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).BeginInit();
                dynamicForm_Fill_Panel.SuspendLayout();
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1691
                //SuspendLayout();
                // 
                // ultraToolbarsManager1
                // 
                ultraToolbarsManager1.DesignerFlags = 1;
                ultraToolbarsManager1.DockWithinContainer = dynamicForm;
                ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
                ultraToolbarsManager1.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
                ultraToolbarsManager1.IsGlassSupported = false;
                // 
                // frmReconCancelAmend_Fill_Panel
                // 
                dynamicForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
                dynamicForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
                dynamicForm_Fill_Panel.Location = new System.Drawing.Point(4, 52);
                dynamicForm_Fill_Panel.Name = "dynamicForm_Fill_Panel";
                dynamicForm_Fill_Panel.Size = new System.Drawing.Size(576, 261);
                dynamicForm_Fill_Panel.TabIndex = 0;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Left
                // 
                dynamicForm_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
                dynamicForm_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 52);
                dynamicForm_Toolbars_Dock_Area_Left.Name = "dynamicForm_Toolbars_Dock_Area_Left";
                dynamicForm_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 261);
                dynamicForm_Toolbars_Dock_Area_Left.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Right
                // 
                dynamicForm_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
                dynamicForm_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(580, 52);
                dynamicForm_Toolbars_Dock_Area_Right.Name = "dynamicForm_Toolbars_Dock_Area_Right";
                dynamicForm_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 261);
                dynamicForm_Toolbars_Dock_Area_Right.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Top
                // 
                dynamicForm_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
                dynamicForm_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
                dynamicForm_Toolbars_Dock_Area_Top.Name = "dynamicForm_Toolbars_Dock_Area_Top";
                dynamicForm_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(584, 52);
                dynamicForm_Toolbars_Dock_Area_Top.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Bottom
                // 
                dynamicForm_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
                dynamicForm_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 313);
                dynamicForm_Toolbars_Dock_Area_Bottom.Name = "dynamicForm_Toolbars_Dock_Area_Bottom";
                dynamicForm_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(584, 4);
                dynamicForm_Toolbars_Dock_Area_Bottom.ToolbarsManager = ultraToolbarsManager1;
                // 
                // frm
                //    
                if (control as UserControl == null)
                {
                    UltraGrid grid = control as UltraGrid;
                    grid.Dock = DockStyle.Fill;
                    dynamicForm.Controls.Add(grid);
                }
                else
                {
                    UserControl userControl = control as UserControl;
                    userControl.Dock = DockStyle.Fill;
                    dynamicForm.Controls.Add(userControl);
                }
                dynamicForm.Owner = this.FindForm();
                dynamicForm.ShowInTaskbar = false;
                dynamicForm.Size = new System.Drawing.Size(1107, 630);
                dynamicForm.Controls.Add(dynamicForm_Fill_Panel);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Left);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Right);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Bottom);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Top);
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).EndInit();
                dynamicForm_Fill_Panel.ResumeLayout(false);
                dynamicForm.ResumeLayout(false);
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

        #region Layout Functions
        /// <summary>
        /// Function Saves the Layout
        /// </summary>
        //public void SaveGridsLayout()
        //{
        //    try
        //    {
        //        if (grdReport != null)
        //        {
        //            if (grdReport.DisplayLayout.Bands[0].Columns.Count > 0)
        //            {

        //                SymbolManagementLayout.SymbolManagementColumns = GetGridColumnLayout(grdReport);
        //            }
        //        }

        //        SaveSymbolManagementLayout();
        //    }
        //    catch (Exception ex)
        //    {
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Returns the Layout as read from the Xml
        /// </summary>
        /// <returns></returns>
        //private static SymbolManagementLayout GetSymbolManagementLayout()
        //{
        //    _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
        //    _symbolManagemantLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID;
        //    _symbolManagemantFilePath = _symbolManagemantLayoutDirectoryPath + @"\SymbolManagementLayout.xml";

        //    SymbolManagementLayout symbolLayout = new SymbolManagementLayout();
        //    try
        //    {
        //        if (!Directory.Exists(_symbolManagemantLayoutDirectoryPath))
        //        {
        //            Directory.CreateDirectory(_symbolManagemantLayoutDirectoryPath);
        //        }
        //        if (File.Exists(_symbolManagemantFilePath))
        //        {
        //            using (FileStream fs = File.OpenRead(_symbolManagemantFilePath))
        //            {
        //                XmlSerializer serializer = new XmlSerializer(typeof(SymbolManagementLayout));
        //                symbolLayout = (SymbolManagementLayout)serializer.Deserialize(fs);
        //            }
        //        }

        //        _SymbolManagementLayout = symbolLayout;
        //    }
        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion

        //    return symbolLayout;
        //}
        /// <summary>
        /// Function Returns a list of Columns of Grid grdReport with Properties as set.
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        //public static List<ColumnData> GetGridColumnLayout(UltraGrid grid)
        //{
        //    List<ColumnData> listGridCols = new List<ColumnData>();
        //    UltraGridBand band = grid.DisplayLayout.Bands[0];
        //    try
        //    {
        //        foreach (UltraGridColumn gridCol in band.Columns)
        //        {
        //            ColumnData colData = new ColumnData();
        //            colData.Key = gridCol.Key;
        //            colData.Caption = gridCol.Header.Caption;
        //            colData.Format = gridCol.Format;
        //            colData.Hidden = gridCol.Hidden;
        //            colData.VisiblePosition = gridCol.Header.VisiblePosition;
        //            colData.Width = gridCol.Width;
        //            colData.ExcludeFromColumnChooser = gridCol.ExcludeFromColumnChooser;
        //            colData.IsGroupByColumn = gridCol.IsGroupByColumn;
        //            colData.Fixed = gridCol.Header.Fixed;
        //            colData.CellActivation = gridCol.CellActivation;

        //            // Sorted Columns
        //            colData.SortIndicator = gridCol.SortIndicator;

        //            //// Summary Settings
        //            //if (band.Summaries.Exists(gridCol.Key))
        //            //{
        //            //    string colSummKey = band.Summaries[gridCol.Key].CustomSummaryCalculator.ToString();
        //            //    colData.ColSummaryKey = (colSummKey.Contains(".")) ? colSummKey.Split('.')[2] : String.Empty;
        //            //    colData.ColSummaryFormat = band.Summaries[gridCol.Key].DisplayFormat;
        //            //}

        //            //Filter Settings
        //            foreach (FilterCondition fCond in band.ColumnFilters[gridCol.Key].FilterConditions)
        //            {
        //                colData.FilterConditionList.Add(fCond);
        //            }
        //            colData.FilterLogicalOperator = band.ColumnFilters[gridCol.Key].LogicalOperator;

        //            listGridCols.Add(colData);
        //        }
        //    }
        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion
        //    return listGridCols;
        //}

        /// <summary>
        /// Function Writes to the XMl the Layout(Columns and associated Properties) as User is using
        /// </summary>
        //public static void SaveSymbolManagementLayout()
        //{
        //    try
        //    {

        //        using (XmlTextWriter writer = new XmlTextWriter(_symbolManagemantFilePath, Encoding.UTF8))
        //        {
        //            writer.Formatting = Formatting.Indented;
        //            XmlSerializer serializer;
        //            serializer = new XmlSerializer(typeof(SymbolManagementLayout));
        //            serializer.Serialize(writer, _SymbolManagementLayout);

        //            writer.Flush();
        //            writer.Close();
        //        }
        //    }
        //    #region catch
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion
        //}

        /// <summary>
        /// Function Sets the Grid Layout as it reads from the List of Columns Layout which are Columns read from XML
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="listColData"></param>
        //public static void SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData)
        //{
        //    List<ColumnData> listSortedGridCols = new List<ColumnData>();
        //    UltraGridBand band = grid.DisplayLayout.Bands[0];
        //    ColumnsCollection gridColumns = band.Columns;// Just for readability ;)
        //    listColData.Sort();

        //    try
        //    {
        //        // Hide All
        //        foreach (UltraGridColumn gridCol in gridColumns)
        //        {
        //            gridCol.Hidden = true;
        //        }

        //        //Set Columns Properties
        //        foreach (ColumnData colData in listColData)
        //        {
        //            if (gridColumns.Exists(colData.Key))
        //            {
        //                UltraGridColumn gridCol = gridColumns[colData.Key];
        //                gridCol.Width = colData.Width;
        //                gridCol.Format = colData.Format;
        //                gridCol.Header.Caption = colData.Caption;
        //                gridCol.Header.VisiblePosition = colData.VisiblePosition;
        //                gridCol.Hidden = colData.Hidden;
        //                gridCol.ExcludeFromColumnChooser = colData.ExcludeFromColumnChooser;
        //                gridCol.Header.Fixed = colData.Fixed;
        //                gridCol.SortIndicator = colData.SortIndicator;
        //                gridCol.CellActivation = Activation.NoEdit;

        //                // Sorted Columns
        //                if (colData.SortIndicator == SortIndicator.Descending || colData.SortIndicator == SortIndicator.Ascending)
        //                {
        //                    listSortedGridCols.Add(colData);
        //                }

        //                //Summary Settings
        //                //if (colData.ColSummaryKey != String.Empty)
        //                //{
        //                //    SummarySettings summary = band.Summaries.Add(gridCol.Key, SummaryType.Custom, riskSummFactory.GetSummaryCalculator(colData.ColSummaryKey), gridCol, SummaryPosition.UseSummaryPositionColumn, gridCol);
        //                //    summary.DisplayFormat = colData.ColSummaryFormat;
        //                //}

        //                // Filter Settings
        //                if (colData.FilterConditionList.Count > 0)
        //                {
        //                    band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
        //                    foreach (FilterCondition fCond in colData.FilterConditionList)
        //                    {
        //                        band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion

        //    // Sorted Columns are returned as they need to be handled after data is binded.
        //    //  return listSortedGridCols;
        //}
        #endregion

        private bool ValidateSymbolForSave(SecMasterUIObj uiObj)
        {
            if (string.IsNullOrEmpty(uiObj.TickerSymbol))
            {
                // MessageBox.Show("Please enter Ticker Symbol", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //lblStatus.Text = toolStripStatusLabel1.Text = " Not Saved, Please enter Ticker Symbol";
                //toolStripStatusLabel1.ForeColor = Color.Red;
                return false;
            }
            if (uiObj.AUECID == int.MinValue)
            {
                //MessageBox.Show(uiObj.TickerSymbol+" is not a valid AUEC, Please re enter the AUEC details", "Error", MessageBoxButtons.OK);
                return false;
            }
            if (string.IsNullOrEmpty(uiObj.LongName))
            {
                //MessageBox.Show("Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Please enter Long Name", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Please enter Long Name";
                //toolStripStatusLabel1.ForeColor = Color.Red;
                return false;
            }
            if (uiObj.CurrencyID == int.MinValue)
            {
                //MessageBox.Show("Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Please enter Currency ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Please enter Currency ";
                //toolStripStatusLabel1.ForeColor = Color.Red;
                return false;
            }
            if (uiObj.Multiplier == 0.0)
            {
                //MessageBox.Show("Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Mutiplier can not be Zero ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, Mutiplier can not be Zero ";
                //toolStripStatusLabel1.ForeColor = Color.Red;
                return false;
            }
            if (uiObj.RoundLot < 1)
            {
                //MessageBox.Show("Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, RoundLots can not be less than 1 ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved, RoundLots can not be less than 1 ";
                //toolStripStatusLabel1.ForeColor = Color.Red;
                return false;
            }

            AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory((AssetCategory)uiObj.AssetID);
            switch (baseAssetCategory)
            {
                case AssetCategory.Equity:
                    break;

                case AssetCategory.Option:
                    if (uiObj.ExpirationDate == DateTimeConstants.MinValue)
                    {
                        //MessageBox.Show("Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Expiration date", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Expiration date";
                        //toolStripStatusLabel1.ForeColor = Color.Red;
                        return false;
                    }
                    if (uiObj.PutOrCall == int.MinValue)
                    {
                        //MessageBox.Show("TickerSymbol: " + uiObj.TickerSymbol + ": Not Saved,Please select OptionType ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //lblStatus.Text = toolStripStatusLabel1.Text = "TickerSymbol: " + uiObj.TickerSymbol + ": Not Saved,Please select OptionType ";
                        //toolStripStatusLabel1.ForeColor = Color.Red;
                        return false;
                    }
                    if (uiObj.StrikePrice == 0)
                    {
                        //MessageBox.Show("Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter strike price ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter strike price ";
                        //toolStripStatusLabel1.ForeColor = Color.Red;
                        return false;
                    }
                    if (string.IsNullOrEmpty(uiObj.UnderLyingSymbol))
                    {
                        //MessageBox.Show("Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter UnderLyingSymbol ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter UnderLyingSymbol ";
                        //toolStripStatusLabel1.ForeColor = Color.Red;
                        return false;
                    }

                    break;

                case AssetCategory.Future:
                    if (uiObj.ExpirationDate == DateTimeConstants.MinValue)
                    {
                        //MessageBox.Show("Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Expiration date", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Expiration date";
                        //toolStripStatusLabel1.ForeColor = Color.Red;
                        return false;
                    }
                    break;
                case AssetCategory.FixedIncome:
                case AssetCategory.ConvertibleBond:
                    if (uiObj.ExpirationDate == DateTimeConstants.MinValue)
                    {
                        //MessageBox.Show("Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Expiration date", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Expiration date";
                        //toolStripStatusLabel1.ForeColor = Color.Red;
                        return false;
                    }
                    if (uiObj.IsZero.Equals(false))
                    {
                        if (uiObj.Coupon == 0)
                        {
                            //MessageBox.Show("Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Coupon", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //lblStatus.Text = toolStripStatusLabel1.Text = "Ticker Symbol: " + uiObj.TickerSymbol + ": Not Saved,Please enter Coupon";
                            //toolStripStatusLabel1.ForeColor = Color.Red;
                            return false;
                        }
                    }
                    break;
                case AssetCategory.PrivateEquity:
                case AssetCategory.CreditDefaultSwap:
                    break;
            }

            //toolStripStatusLabel1.ForeColor = Color.Black;
            return true;
        }

        private SecMasterBaseObj GetSecMasterObjFromUIObject(SecMasterUIObj uiObj)
        {
            SecMasterBaseObj secMasterBaseObj = null;
            try
            {
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
            return secMasterBaseObj;
        }

        /// <summary>
        /// Load report layout xml if file exist
        /// </summary>
        private void LoadReportSaveLayoutXML()
        {
            try
            {
                _userID = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                _symbolManagemantLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + _userID;
                _symbolManagemantFilePath = _symbolManagemantLayoutDirectoryPath + @"\SymbolManagementLayout.xml";

                if (!Directory.Exists(_symbolManagemantLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_symbolManagemantLayoutDirectoryPath);
                }
                if (File.Exists(_symbolManagemantFilePath))
                {
                    grdReport.DisplayLayout.LoadFromXml(_symbolManagemantFilePath);
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

        internal void UnwireEvents()
        {
            try
            {
                secMasterService.ResponseCompleted -= secMasterService_ResponseCompleted;
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

        private void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                launchForm = ImportManager.Instance.GetLaunchForm();
                if (launchForm != null)
                {
                    ListEventAargs args = new ListEventAargs();
                    args.listOfValues.Add(ApplicationConstants.CONST_LIVEFEEDVALIDATION_UI.ToString());
                    launchForm(this.FindForm(), args);
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
    }

}
