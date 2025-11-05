using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SMObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.Tools.PL.SecMaster;
using Prana.Utilities.UI.CronUtility;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlSMBatchCreator : UserControl
    {
        TaskSchedulerForm ctrlTaskScheduler;

        static string _cronExpression = string.Empty;

        static int _smBatchID = 0;

        SMBatch.BatchType _batType = SMBatch.BatchType.HistoricalBatch;

        /// <summary>
        /// SMBatch object to hold all SMBatch relevant details
        /// </summary>
        SMBatch smBatch;
        //bool _isAdvncdSearching = false;
        AdvSearchFilterUI _advSearchFilertUI = null;
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="batType"></param>
        public ctrlSMBatchCreator(SMBatch.BatchType batType)
        {
            try
            {
                InitializeComponent();
                _batType = batType;
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
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void InitializeControl()
        {
            try
            {
                BindClientCombo();
                //BindRunTimeTypes();
                BindFields();
                cmbAccount.NullText = "-Select-";
                _smBatchID = 0;
                BindBatchType();
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
        /// Bind batch type (Current or historical)
        /// </summary>
        private void BindBatchType()
        {
            if (_batType == SMBatch.BatchType.CurrentBatch)
            {
                chkHistoricalData.Enabled = false;
                tbHistReq.Enabled = false;
            }
            else
            {
                chkHistoricalData.Enabled = true;
                tbHistReq.Enabled = true;
            }
        }

        /// <summary>
        /// To initialize SMBatch for edit
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="SMBatchID"></param>
        public void InitializeSMBatchForEdit(int SMBatchID)
        {
            try
            {
                _smBatchID = SMBatchID;
                BindClientCombo();
                BindBatchType();
                //BindFields();
                cmbAccount.NullText = "-Select-";
                BindingList<SMBatch> smBatchObjList = new BindingList<SMBatch>();

                smBatchObjList = SMBatchManager.GetSMBatchData();

                // check if smbatch null or not
                if (smBatchObjList != null)
                {
                    foreach (SMBatch smBatch in smBatchObjList)
                    {
                        if (smBatch.SMBatchID == SMBatchID)
                        {
                            FillSMBatchDetailsForEdit(smBatch);
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
        /// To fill UI with SMBatch details
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="smBatch"></param>
        private void FillSMBatchDetailsForEdit(SMBatch smBatch)
        {
            try
            {
                if (smBatch != null)
                {
                    _batType = smBatch.BatType;
                    BindFields();
                    int ClientID = 0;
                    string[] accounts = smBatch.AccountIDs.Split(',');
                    List<object> listaccounts = new List<object>();
                    foreach (string accountID in accounts)
                    {
                        Dictionary<int, List<int>> accountClientCollection = SMBatchManager.GetClientAccountMapping();
                        if (!string.IsNullOrEmpty(accountID))
                        {
                            foreach (KeyValuePair<int, List<int>> item in accountClientCollection)
                            {
                                if (item.Value.Contains(Convert.ToInt32(accountID)) && ClientID == 0)
                                {
                                    ClientID = item.Key;
                                }
                            }
                            listaccounts.Add(Convert.ToInt32(accountID));
                        }
                    }
                    cmbClient.Value = ClientID;
                    cmbAccount.Value = listaccounts;
                    txtSystemLevelName.Text = smBatch.SystemLevelName;
                    txtUserDefinedlName.Text = smBatch.UserDefinedName;
                    chkHistoricalData.Checked = smBatch.IsHistoric;
                    tbHistReq.Text = smBatch.HistoricDaysRequired.ToString();

                    string[] fields = smBatch.Fields.Split(',');
                    List<object> listfields = new List<object>();
                    foreach (string field in fields)
                    {
                        if (!string.IsNullOrEmpty(field))
                        {
                            listfields.Add(field.Trim());
                        }
                    }
                    cmbField.Value = listfields;

                    tbIndices.Text = smBatch.Indices;
                    //cmbIndices.Value = listIndices;

                    if (!string.IsNullOrEmpty(smBatch.CronExpression))
                    {
                        txtFrequency.Text = CronUtility.GetCronDescription(smBatch.CronExpression);
                    }
                    txtFilter.Text = smBatch.FilterClause;
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
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void BindFields()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();
                cmbField.NullText = "-Select-";
                ConcurrentDictionary<string, StructPricingField> dictFields = SMBatchManager.GetSecurityFields();
                foreach (KeyValuePair<string, StructPricingField> kvp in dictFields)
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value.FieldName, kvp.Key);
                    if ((_batType == SMBatch.BatchType.HistoricalBatch && kvp.Value.IsHistorical) || (_batType == SMBatch.BatchType.CurrentBatch && kvp.Value.IsRealTime))
                    {
                        listValues.Add(value);
                    }
                }
                cmbField.DataSource = null;
                cmbField.DataSource = listValues;

                if (cmbField.DataSource != null)
                {
                    if (!cmbField.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn cbBatch = cmbField.DisplayLayout.Bands[0].Columns.Add();
                        cbBatch.Key = "Selected";
                        cbBatch.Header.Caption = string.Empty;
                        cbBatch.Width = 25;
                        cbBatch.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        cbBatch.DataType = typeof(bool);
                        cbBatch.Header.VisiblePosition = 0;
                    }
                    cmbField.CheckedListSettings.CheckStateMember = "Selected";
                    cmbField.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    cmbField.CheckedListSettings.ListSeparator = " , ";
                    cmbField.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    cmbField.DisplayMember = "DisplayText";
                    cmbField.ValueMember = "Value";
                    cmbField.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                    cmbField.Value = -1;
                    cmbField.DisplayLayout.Bands[0].Columns["DisplayText"].Header.Caption = "Select All";
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

        #region Commented Code
        //Code commented By Faisal Shah 09/07/14
        //Needed to hide Run Time from UI
        /// <summary>
        /// Bind the formats to the combobox
        /// </summary>
        //private void BindRunTimeTypes()
        //{
        //    try
        //    {
        //        List<EnumerationValue> listValues = new List<EnumerationValue>();

        //        Dictionary<int, string> dictRunTime = SMBatchManager.GetRunTimeTypes();
        //        foreach (KeyValuePair<int, string> kvp in dictRunTime)
        //        {
        //            EnumerationValue value = new EnumerationValue(kvp.Value, kvp.Key);
        //            listValues.Add(value);
        //        }
        //        listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
        //        cmbRunTime.DataSource = null;
        //        cmbRunTime.DataSource = listValues;
        //        cmbRunTime.DisplayMember = "DisplayText";
        //        cmbRunTime.ValueMember = "Value";
        //        cmbRunTime.DataBind();
        //        cmbRunTime.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
        //        cmbRunTime.Value = 2;
        //        cmbRunTime.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                // cmbClient = new UltraCombo();
                cmbClient.DataSource = null;
                cmbClient.DataSource = listValues;
                cmbClient.DisplayMember = "DisplayText";
                cmbClient.ValueMember = "Value";
                cmbClient.DataBind();
                cmbClient.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbClient.Value = -1;
                cmbClient.DisplayLayout.Bands[0].ColHeadersVisible = false;
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

        private void btnGetSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                //TaskSchedulerForm taskForm = new TaskSchedulerForm();
                ctrlTaskScheduler = new TaskSchedulerForm();
                ctrlTaskScheduler.ShowDialog(this.Parent);
                DialogResult dr = ctrlTaskScheduler.DialogResult;
                if (dr == DialogResult.OK)
                {
                    string cronExp = ctrlTaskScheduler.GetCronExpression();
                    _cronExpression = cronExp;
                    if (!string.IsNullOrEmpty(cronExp))
                    {
                        txtFrequency.Text = CronUtility.GetCronDescription(cronExp); //SMBatchManager.GetRunSchedule(cronExp) + SMBatchManager.GetRunTime(cronExp);
                        //grdSMBatch.ActiveRow.Cells["CronExpression"].Value = cronExp;
                        //grdSMBatch.ActiveRow.Cells["RunSchedule"].Value = SMBatchManager.GetRunSchedule(cronExp);
                        //grdSMBatch.ActiveRow.Cells["RunTime"].Value = SMBatchManager.GetRunTime(cronExp);
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

        private void cmbClient_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                BindClientWiseAccount();
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
        /// Bind client wise account list
        /// </summary>
        private void BindClientWiseAccount()
        {
            try
            {
                if (cmbClient.Value != null && cmbClient.SelectedRow != null && !string.IsNullOrEmpty(cmbClient.SelectedRow.Cells["Value"].Text))
                {
                    //int companyID = Convert.ToInt32(cmbClient.SelectedRow.Cells["Value"].Text);
                    //if (companyID == -1)
                    //{
                    //    cmbAccount.Enabled = false;
                    //    return;
                    //}
                    cmbAccount.Enabled = true;
                    cmbAccount.Text = string.Empty;
                    cmbAccount.DataSource = CachedDataManager.GetInstance.GetAccountsForCompany();
                    cmbAccount.DisplayLayout.Bands[0].Columns["AccountID"].Hidden = true;
                    SetAccountControl();
                    if (cmbAccount.Rows.Count > 0)
                    {
                        cmbAccount.SelectedRow = cmbAccount.Rows[0];
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
        /// Setup the layout of the account control
        /// </summary>
        private void SetAccountControl()
        {
            try
            {
                if (cmbAccount.DataSource != null)
                {
                    if (!cmbAccount.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn cbAccount = cmbAccount.DisplayLayout.Bands[0].Columns.Add();
                        cbAccount.Key = "Selected";
                        cbAccount.Header.Caption = string.Empty;
                        cbAccount.Width = 25;
                        cbAccount.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        cbAccount.DataType = typeof(bool);
                        cbAccount.Header.VisiblePosition = 1;
                    }
                    cmbAccount.CheckedListSettings.CheckStateMember = "Selected";
                    cmbAccount.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    cmbAccount.CheckedListSettings.ListSeparator = " , ";
                    cmbAccount.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    cmbAccount.DisplayMember = "AccountName";
                    cmbAccount.ValueMember = "AccountID";
                    cmbAccount.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
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

        private void btnSaveBatch_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValidData())
                {
                    return;
                }
                smBatch = GetSMBatchDetails();
                //object[] values = CreateValuesSet();
                _smBatchID = SMBatchManager.SaveBatchDetailsFromCreator(smBatch);
                if (_smBatchID > 0)
                {
                    MessageBox.Show("Batch Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// To get smBatch details
        /// </summary>
        /// <param name="user"></param>
        private SMBatch GetSMBatchDetails()
        {
            SMBatch smBatchObj = new SMBatch();
            try
            {
                smBatchObj.SMBatchID = _smBatchID;
                smBatchObj.SystemLevelName = txtSystemLevelName.Text.Trim();
                smBatchObj.UserDefinedName = txtUserDefinedlName.Text.Trim();
                smBatchObj.AccountIDs = String.Join(", ", cmbAccount.Value as List<object>);
                if (chkHistoricalData.Checked == true)
                {
                    smBatchObj.IsHistoric = true;
                    smBatchObj.HistoricDaysRequired = Convert.ToInt32(tbHistReq.Text.Trim());
                }
                else
                {
                    smBatchObj.IsHistoric = false;
                    smBatchObj.HistoricDaysRequired = 0;
                }
                smBatchObj.Fields = String.Join(", ", cmbField.Value as List<object>);
                smBatchObj.Indices = tbIndices.Text;
                smBatchObj.RunTime = Convert.ToInt32(Prana.BusinessObjects.AppConstants.SMBatchRunTime.UserDefined);
                if (chkHistoricalData.Checked == true)
                {
                    smBatchObj.CronExpression = _cronExpression;
                }
                else
                {
                    smBatchObj.CronExpression = string.Empty;
                }
                smBatchObj.FilterClause = txtFilter.Text.Trim();
                smBatchObj.BatType = _batType;
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
            return smBatchObj;
        }

        //private object[] CreateValuesSet()
        //{
        //    string systemLevelName = txtSystemLevelName.Text.Trim();
        //    string userDefinedName = txtUserDefinedlName.Text.Trim();
        //    string accountIDs = String.Join(", ", cmbAccount.Value as List<object>);
        //    int isHistoric = (int)(chkHistoricalData.CheckState);
        //    int historicDaysRequired; 
        //    int.TryParse(tbHistReq.Text,out historicDaysRequired);
        //    int batchType = Convert.ToInt32(cmbBatchType.Value);
        //    int runTime = Convert.ToInt32(Prana.BusinessObjects.AppConstants.SMBatchRunTime.UserDefined);
        //    object[] values = { _smBatchID, systemLevelName, userDefinedName, accountIDs, batchType, isHistoric,historicDaysRequired, runTime, _cronExpression };
        //    return values;
        //}
        /// <summary>
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <returns></returns>
        private bool IsValidData()
        {
            try
            {
                errorProvider1.SetError(txtSystemLevelName, "");
                errorProvider1.SetError(txtUserDefinedlName, "");
                errorProvider1.SetError(cmbClient, "");
                errorProvider1.SetError(cmbAccount, "");
                //errorProvider1.SetError(cmbIndices, "");
                //errorProvider1.SetError(cmbRunTime, "");
                errorProvider1.SetError(cmbField, "");
                if (string.IsNullOrWhiteSpace(txtSystemLevelName.Text))
                {
                    errorProvider1.SetError(txtSystemLevelName, "Provide a system level name for the batch");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(txtUserDefinedlName.Text))
                {
                    errorProvider1.SetError(txtUserDefinedlName, "Provide a user defined name for the batch");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(cmbClient.Value.ToString()) || Convert.ToInt32(cmbClient.Value) == -1)
                {
                    errorProvider1.SetError(cmbClient, "Select at least one client for batch");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(cmbAccount.Value.ToString()) || cmbAccount.CheckedRows.Count == 0)// && Convert.ToInt32(cmbAccount.Value)==-1)
                {
                    errorProvider1.SetError(cmbAccount, "Select at least one account for the batch");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(cmbField.Value.ToString()) || cmbField.CheckedRows.Count == 0)//Convert.ToInt32(cmbField.Value) == -1)
                {
                    errorProvider1.SetError(cmbField, "Select at least one field for the batch");
                    return false;
                }
                //Commented By Faisal Shah 09/07/14 
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-954
                //if (string.IsNullOrWhiteSpace(cmbRunTime.Value.ToString()) || Convert.ToInt32(cmbRunTime.Value) == -1)
                //{
                //    errorProvider1.SetError(cmbRunTime, "Select a run time for the batch");
                //    return false;
                //}
                if (SMBatchManager.IsSMBatchExist(txtSystemLevelName.Text.Trim(), _smBatchID))
                {
                    MessageBox.Show("Duplicate System Level Name cannot be inserted. Details could not be saved.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                //if (!string.IsNullOrWhiteSpace(txtSystemLevelName.Text))
                //{
                //    errorProvider1.SetError(txtSystemLevelName, "Provide a system level name for the batch");
                //}
                return true;
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

        #region CommentedCode
        //private void cmbRunTime_ValueChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(cmbRunTime.Value.ToString()) && Convert.ToInt32(cmbRunTime.Value) != -1)
        //        {
        //            if (Convert.ToInt32(cmbRunTime.Value) == 2)
        //            {
        //                ultraPanel2.Visible = true;
        //            }
        //            else
        //            {
        //                ultraPanel2.Visible = false;
        //                _cronExpression = string.Empty;
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
        #endregion
        private void ctrlSMBatchCreator_Load(object sender, EventArgs e)
        {
            try
            {

                cmbField.DropDownStyle = UltraComboStyle.DropDownList;
                cmbClient.DropDownStyle = UltraComboStyle.DropDownList;
                //cmbRunTime.DropDownStyle = UltraComboStyle.DropDownList;
                //_smBatchID = 0;
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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
                btChooseIndex.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btChooseIndex.ForeColor = System.Drawing.Color.White;
                btChooseIndex.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btChooseIndex.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btChooseIndex.UseAppStyling = false;
                btChooseIndex.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnFilter.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnFilter.ForeColor = System.Drawing.Color.White;
                btnFilter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnFilter.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnFilter.UseAppStyling = false;
                btnFilter.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetSchedule.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetSchedule.ForeColor = System.Drawing.Color.White;
                btnGetSchedule.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetSchedule.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetSchedule.UseAppStyling = false;
                btnGetSchedule.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRefresh.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRefresh.ForeColor = System.Drawing.Color.White;
                btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRefresh.UseAppStyling = false;
                btnRefresh.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSaveBatch.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSaveBatch.ForeColor = System.Drawing.Color.White;
                btnSaveBatch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSaveBatch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSaveBatch.UseAppStyling = false;
                btnSaveBatch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void chkHistoricalData_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkHistoricalData.Checked)
                {
                    tbHistReq.Enabled = true;
                }
                else
                {
                    tbHistReq.Enabled = false;
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

        private void tbHistReq_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
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

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (_advSearchFilertUI == null)
                {
                    _advSearchFilertUI = new AdvSearchFilterUI();
                    Dictionary<String, ValueList> dataValuesList = SecMasterHelper.getInstance().GetRequiredValueListDict();
                    List<String> columnsList = new List<string>();
                    columnsList.Add("AssetID");
                    columnsList.Add("CreationDate");
                    columnsList.Add("DataSource");
                    columnsList.Add("IsSecApproved");
                    //columnsList.Add("UDASecurityTypeID");

                    _advSearchFilertUI.SetUp(typeof(SecMasterUIObj), dataValuesList, columnsList, false);
                    _advSearchFilertUI.FormClosed += new FormClosedEventHandler(advSearchFilertUI_FormClosed);
                    _advSearchFilertUI.SearchDataEvent += advSearchFilertUI_SearchDataEvent;
                }
                _advSearchFilertUI.StartPosition = FormStartPosition.Manual;
                _advSearchFilertUI.Show();

                // Theme for CH Release 
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
        /// Search form close event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void advSearchFilertUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_advSearchFilertUI != null)
            {
                _advSearchFilertUI = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Queury"></param>
        void advSearchFilertUI_SearchDataEvent(object sender, EventArgs<string> e)
        {
            try
            {
                //_isAdvncdSearching = true;
                _advSearchFilertUI.Close();
                txtFilter.Text = e.Value;
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

        private void btChooseIndex_Click(object sender, EventArgs e)
        {
            try
            {
                FetchIndices fetchIndicesForm;
                if (String.IsNullOrWhiteSpace(tbIndices.Text))
                    fetchIndicesForm = new FetchIndices();
                else
                    fetchIndicesForm = new FetchIndices(tbIndices.Text);
                fetchIndicesForm.SelectedIndices += fetchIndicesForm_SelectedIndices;
                fetchIndicesForm.ShowDialog(this);
                fetchIndicesForm.ShowInTaskbar = false;
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

        void fetchIndicesForm_SelectedIndices(object sender, EventArgs<string> e)
        {
            try
            {
                if (this.tbIndices.InvokeRequired)
                {
                    EventHandler<EventArgs<string>> d = new EventHandler<EventArgs<string>>(fetchIndicesForm_SelectedIndices);
                    this.BeginInvoke(d, new object[] { this, e });
                    return;
                }
                tbIndices.Text = e.Value;
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
        /// To refresh batch creator UI. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshSMBatchUI();
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
        /// function to refresh batch creator UI.
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void RefreshSMBatchUI()
        {
            try
            {
                txtSystemLevelName.Text = string.Empty;
                txtUserDefinedlName.Text = string.Empty;
                BindClientCombo();
                BindClientWiseAccount();
                BindFields();
                tbIndices.Text = string.Empty;
                txtFilter.Text = string.Empty;
                txtFrequency.Text = string.Empty;
                chkHistoricalData.Checked = false;
                tbHistReq.Text = string.Empty;
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

        private void chkFrequency_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkFrequency.Checked == true)
                {
                    btnGetSchedule.Enabled = true;
                }
                else
                {
                    btnGetSchedule.Enabled = false;
                    txtFrequency.Text = string.Empty;
                    _cronExpression = string.Empty;
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
