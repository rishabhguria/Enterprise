using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.Tools.PL.SecMaster;
using Prana.Utilities.UI.CronUtility;
using Prana.Utilities.UI.ImportExportUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlReportTemplate : UserControl
    {
        /// <summary>
        /// Variable to hold the cron expression
        /// </summary>
        static string _cronExpression = string.Empty;

        /// <summary>
        /// Hold the report ID
        /// </summary>
        static int _reportID = 0;

        /// <summary>
        /// Hold the grouping id
        /// </summary>
        static string _currentGrouping = "Select";

        /// <summary>
        /// frequency dictionary
        /// </summary>
        Dictionary<int, string> _dictFrequency;

        /// <summary>
        /// Variable to show the task schedular form
        /// </summary>
        TaskSchedulerForm ctrlTaskScheduler;

        //private int pageIndex = 0;
        //private int _pageSize = 20;
        //bool _isAdvncdSearching = false;

        AdvSearchFilterUI advSearchFilertUI = null;

        bool _isSaveRequired = false;

        /// <summary>
        /// Flag to indicate if the report is in edit mode
        /// </summary>
        bool isInEditMode = false;
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlReportTemplate()
        {
            try
            {
                InitializeComponent();
                //InitializeData();
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
        /// Initialize the data for the form
        /// </summary>
        public void InitializeData()
        {
            try
            {
                BindReportsCombo();
                BindReportsViewCombo();
                BindThirdPartyCombo();
                BindFrequency();
                BindFormat();
                BindGroupingColumns();
                BindSelectedColumns();
                if (cmbSelectedColumns.DataSource != null)
                {
                    if (!cmbSelectedColumns.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn cbAccount = cmbSelectedColumns.DisplayLayout.Bands[0].Columns.Add();
                        cbAccount.Key = "Selected";
                        cbAccount.Header.Caption = string.Empty;
                        cbAccount.Width = 25;
                        cbAccount.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        cbAccount.DataType = typeof(bool);
                        cbAccount.Header.VisiblePosition = 1;
                    }
                    cmbSelectedColumns.CheckedListSettings.CheckStateMember = "Selected";
                    cmbSelectedColumns.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    cmbSelectedColumns.CheckedListSettings.ListSeparator = " , ";
                    cmbSelectedColumns.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    cmbSelectedColumns.DisplayMember = "ColumnCaption";
                    cmbSelectedColumns.ValueMember = "ColumnName";
                    cmbSelectedColumns.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
                    grdSecReport.DisplayLayout.GroupByBox.Hidden = true;
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
        /// bind the selected columns
        /// </summary>
        private void BindSelectedColumns()
        {
            try
            {
                Dictionary<string, string> dictColumn = SecReportTemplateManager.GetColumns();
                cmbSelectedColumns.DataSource = SecReportTemplateManager.GetColumnsForReports(dictColumn);
                cmbSelectedColumns.DisplayLayout.Bands[0].Columns["ColumnName"].Hidden = true;
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
        /// Bind the reports combo to data
        /// </summary>
        private void BindReportsCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();

                Dictionary<int, string> dictReports = SecReportTemplateManager.GetReports();
                foreach (KeyValuePair<int, string> kvp in dictReports)
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value, kvp.Key);
                    listValues.Add(value);
                }
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                listValues.Insert(1, new EnumerationValue("-New Report-", 0));
                cmbReports.DataSource = null;
                cmbReports.DataSource = listValues;
                cmbReports.DisplayMember = "DisplayText";
                cmbReports.ValueMember = "Value";
                cmbReports.DataBind();
                cmbReports.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbReports.Value = -1;
                cmbReports.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
        /// bind the columns to the column chooser combo and grouping combos
        /// </summary>
        private void BindGroupingColumns()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();
                Dictionary<string, string> dictColumn = GroupingColumnList();
                foreach (KeyValuePair<string, string> kvp in dictColumn)
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value, kvp.Key);
                    listValues.Add(value);
                }
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, "Select"));

                //cmbSelectedColumns.DataSource = null;
                //cmbSelectedColumns.DataSource = listValues;
                //cmbSelectedColumns.DisplayMember = "ColumnName";
                //cmbSelectedColumns.ValueMember = "ColumnCaption";
                //cmbSelectedColumns.DataBind();
                //cmbSelectedColumns.Value = "Select";
                //cmbSelectedColumns.DisplayLayout.Bands[0].ColHeadersVisible = false;

                cmbGrouping1.DataSource = null;
                cmbGrouping1.DataSource = listValues;
                cmbGrouping1.DisplayMember = "DisplayText";
                cmbGrouping1.ValueMember = "Value";
                cmbGrouping1.DataBind();
                cmbGrouping1.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbGrouping1.Value = "Select";
                cmbGrouping1.DisplayLayout.Bands[0].ColHeadersVisible = false;

                cmbGrouping2.DataSource = null;
                cmbGrouping2.DataSource = listValues;
                cmbGrouping2.DisplayMember = "DisplayText";
                cmbGrouping2.ValueMember = "Value";
                cmbGrouping2.DataBind();
                cmbGrouping2.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbGrouping2.Value = "Select";
                cmbGrouping2.DisplayLayout.Bands[0].ColHeadersVisible = false;

                cmbGrouping3.DataSource = null;
                cmbGrouping3.DataSource = listValues;
                cmbGrouping3.DisplayMember = "DisplayText";
                cmbGrouping3.ValueMember = "Value";
                cmbGrouping3.DataBind();
                cmbGrouping3.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbGrouping3.Value = "Select";
                cmbGrouping3.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
        /// Set the list of grouping columns according to the selected columns for report
        /// </summary>
        /// <returns>dictionary of columns</returns>
        private Dictionary<string, string> GroupingColumnList()
        {
            Dictionary<string, string> dictGroup = new Dictionary<string, string>();
            try
            {
                List<object> columnList = cmbSelectedColumns.Value as List<object>;
                if (columnList != null)
                {
                    foreach (string s in columnList)
                    {
                        string colCaption = SecReportTemplateManager.GetCaption(s);
                        dictGroup.Add(s, colCaption);
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
            return dictGroup;
        }

        /// <summary>
        /// Bind the formats to the combobox
        /// </summary>
        private void BindFormat()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();

                Dictionary<int, string> dictReportFormat = SecReportTemplateManager.GetReportFormats();
                foreach (KeyValuePair<int, string> kvp in dictReportFormat)
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value, kvp.Key);
                    listValues.Add(value);
                }
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                cmbFormat.DataSource = null;
                cmbFormat.DataSource = listValues;
                cmbFormat.DisplayMember = "DisplayText";
                cmbFormat.ValueMember = "Value";
                cmbFormat.DataBind();
                cmbFormat.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbFormat.Value = -1;
                cmbFormat.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
        /// Bind the frequency to the combo boxes
        /// </summary>
        private void BindFrequency()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();
                _dictFrequency = SecReportTemplateManager.GetFrequency();
                //create the frequency list
                foreach (KeyValuePair<int, string> kvp in _dictFrequency)
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value, kvp.Key);
                    listValues.Add(value);
                }
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));

                //bind to the frequency combo
                //cmbFrequency.DataSource = null;
                //cmbFrequency.DataSource = listValues;
                //cmbFrequency.DisplayMember = "DisplayText";
                //cmbFrequency.ValueMember = "Value";
                //cmbFrequency.DataBind();
                //cmbFrequency.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                //cmbFrequency.Value = -1;
                //cmbFrequency.DisplayLayout.Bands[0].ColHeadersVisible = false;

                //bind to the send frequency combo
                //cmbSendFrequency.DataSource = null;
                //cmbSendFrequency.DataSource = listValues;
                //cmbSendFrequency.DisplayMember = "DisplayText";
                //cmbSendFrequency.ValueMember = "Value";
                //cmbSendFrequency.DataBind();
                //cmbSendFrequency.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                //cmbSendFrequency.Value = -1;
                //cmbSendFrequency.DisplayLayout.Bands[0].ColHeadersVisible = false; 

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
        /// Bind the third party combo to the data
        /// </summary>
        private void BindThirdPartyCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();

                Dictionary<int, string> dictThirdParties = CachedDataManager.GetInstance.GetAllThirdParties();
                foreach (KeyValuePair<int, string> kvp in dictThirdParties)
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value, kvp.Key);
                    listValues.Add(value);
                }
                //listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                cmbThirdParty.DataSource = null;
                cmbThirdParty.DataSource = listValues;
                cmbThirdParty.DisplayMember = "DisplayText";
                cmbThirdParty.ValueMember = "Value";
                cmbThirdParty.DataBind();
                cmbThirdParty.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                //cmbThirdParty.Value = -1;
                //cmbThirdParty.DisplayLayout.Bands[0].ColHeadersVisible = false;

                if (cmbThirdParty.DataSource != null)
                {
                    if (!cmbThirdParty.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn colSelectThirdParty = cmbThirdParty.DisplayLayout.Bands[0].Columns.Add("Selected");
                        colSelectThirdParty.Header.Caption = string.Empty;
                        colSelectThirdParty.Width = 25;
                        colSelectThirdParty.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        colSelectThirdParty.DataType = typeof(bool);
                        colSelectThirdParty.Header.VisiblePosition = 0;
                    }
                    cmbThirdParty.CheckedListSettings.CheckStateMember = "Selected";
                    cmbThirdParty.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    cmbThirdParty.CheckedListSettings.ListSeparator = " , ";
                    cmbThirdParty.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    cmbThirdParty.DisplayLayout.Bands[0].Columns[0].Header.Caption = "Select All";
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
        /// Populate the account combo on value change in third party combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbThirdParty_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbThirdParty.Value.ToString()))
                {
                    List<object> thirdPartyCol = cmbThirdParty.Value as List<object>;
                    List<int> thirdPartyIDList = new List<int>();
                    if (thirdPartyCol != null)
                    {
                        foreach (int thirdPartyID in thirdPartyCol)
                        {
                            thirdPartyIDList.Add(thirdPartyID);
                        }
                    }
                    BindAccountCombo(thirdPartyIDList);
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
        /// Fill the accounts when the third party is selected
        /// </summary>
        /// <param name="thirdPartyID">List of third party IDs</param>
        private void BindAccountCombo(List<int> thirdPartyIDList)
        {
            try
            {
                Dictionary<int, string> dictAccounts = new Dictionary<int, string>();
                List<int> accountList = CachedDataManager.GetInstance.GetThirdPartiesAccountsList(thirdPartyIDList);
                //if (accountList.Count <= 0)
                //{
                //    cmbAccount.DataSource = null;
                //    return;
                //}
                //else if (accountList.Count == 1 && accountList[0] == -1)
                //{
                //    cmbAccount.DataSource = null;
                //    return;
                //}
                //List<EnumerationValue> listValues = new List<EnumerationValue>();
                foreach (int accountId in accountList)
                {
                    string accountName = CachedDataManager.GetInstance.GetAccountText(accountId);
                    if (!dictAccounts.ContainsKey(accountId) && accountId != -1)
                    {
                        dictAccounts.Add(accountId, accountName);
                    }
                    //EnumerationValue value = new EnumerationValue(accountName, accountId);
                    //listValues.Add(value);
                }

                cmbAccount.DataSource = null;
                cmbAccount.DataSource = SecReportTemplateManager.GetAccounts(dictAccounts);
                //cmbAccount.DataSource = listValues;
                cmbAccount.DisplayMember = "AccountName";
                cmbAccount.ValueMember = "AccountID";
                cmbAccount.DataBind();
                cmbAccount.DisplayLayout.Bands[0].Columns["AccountID"].Hidden = true;
                //cmbAccount.Value = -1;
                //cmbAccount.DisplayLayout.Bands[0].ColHeadersVisible = false;

                if (cmbAccount.DataSource != null)
                {
                    if (!cmbAccount.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn colSelectAccount = cmbAccount.DisplayLayout.Bands[0].Columns.Add("Selected");
                        //colSelectAccount.Key = "Selected";
                        colSelectAccount.Header.Caption = string.Empty;
                        colSelectAccount.Width = 25;
                        colSelectAccount.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        colSelectAccount.DataType = typeof(bool);
                        colSelectAccount.Header.VisiblePosition = 1;
                    }
                    cmbAccount.CheckedListSettings.CheckStateMember = "Selected";
                    cmbAccount.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    cmbAccount.CheckedListSettings.ListSeparator = " , ";
                    cmbAccount.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    //cmbAccount.DisplayMember = "ColumnCaption";
                    //cmbAccount.ValueMember = "ColumnName";
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

        /// <summary>
        /// show the task schedular and fill the details 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                ctrlTaskScheduler = new TaskSchedulerForm();
                if (!string.IsNullOrWhiteSpace(_cronExpression))
                {
                    //string cronExp = Convert.ToString(grdBatchSetup.ActiveRow.Cells["CronExpression"].Value);
                    ctrlTaskScheduler.GetCronToFill(_cronExpression);
                }
                ctrlTaskScheduler.ShowDialog(this.Parent);
                DialogResult dr = ctrlTaskScheduler.DialogResult;
                if (dr == DialogResult.OK)
                {
                    string cronExp = ctrlTaskScheduler.GetCronExpression();
                    //grdBatchSetup.ActiveRow.Cells["CronExpression"].Value = cronExp;
                    _cronExpression = cronExp;
                    FillCronDetails(_cronExpression);

                    //int batchID = int.Parse(grdBatchSetup.ActiveRow.Cells["BatchID"].Value.ToString());
                    //foreach (DataRow row in ((DataTable)grdBatchSetup.DataSource).Rows)
                    //{
                    //    if (batchID == int.Parse(row["BatchID"].ToString()))
                    //    {
                    //        BatchSetupManager.FillCronDetails(cronExp, row);
                    //    }
                    //}
                    //CronDescription cronDetail=CronUtility.GetCronDescriptionObject(cronExp);
                    //int schedule = (int)cronDetail.Type;
                    //DateTime startTime = Convert.ToDateTime(cronDetail.StartDate.ToString() + cronDetail.StartTime.ToString());
                    //DateTime nxtExecTime=startTime
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
        /// Fill the schedule details with the cron expression 
        /// </summary>
        /// <param name="_cronExpression">cronexpression in the string form</param>
        private void FillCronDetails(string _cronExpression)
        {
            try
            {
                CronDescription cronDetail = CronUtility.GetCronDescriptionObject(_cronExpression);
                String runtime = CronUtility.GetCronDescription(_cronExpression);
                int schedule = (int)cronDetail.Type;
                txtFrequency.Text = _dictFrequency[schedule];

                DateTime actionTime = new DateTime();
                DateTime nxtExecTime = new DateTime();
                switch (schedule)
                {
                    case 0:
                        nxtExecTime = DateTime.MinValue;
                        string startTime = Convert.ToString(cronDetail.StartTime.ToShortTimeString());
                        string startDate = Convert.ToString(cronDetail.StartDate.ToShortDateString());
                        actionTime = Convert.ToDateTime(startDate + " " + startTime);

                        if (DateTime.Compare(DateTime.Now, actionTime) > 0)
                        {
                            runtime = "Expired";
                            //row["NxtExecTime"] = string.Empty;
                        }
                        else
                        {
                            //row["NxtExecTime"] = actionTime;
                            //row["LastExecTime"] = string.Empty;
                        }
                        break;
                    case 1:
                        string time = string.Empty;
                        if (cronDetail.StartTime.TimeOfDay > DateTime.Now.TimeOfDay)
                        {
                            time = DateTime.Now.ToShortDateString() + " " + cronDetail.StartTime.ToShortTimeString();
                            actionTime = Convert.ToDateTime(time);
                        }
                        else
                        {
                            time = DateTime.Now.AddDays(1).ToShortDateString() + " " + cronDetail.StartTime.ToShortTimeString();
                            actionTime = Convert.ToDateTime(time);
                        }
                        break;
                    case 2:
                        if (cronDetail.StartTime > DateTime.Now)
                        {
                            //row["NxtExecTime"] = cronDetail.StartTime.ToLongDateString();
                        }
                        else
                        {
                            //row["NxtExecTime"] = cronDetail.StartTime.AddDays(7);
                        }
                        break;
                    case 3:
                        if (cronDetail.StartTime > DateTime.Now)
                        {
                            //row["NxtExecTime"] = cronDetail.StartTime.ToLongDateString();
                        }
                        else
                        {
                            //row["NxtExecTime"] = cronDetail.StartTime.AddMonths(1);
                        }
                        break;
                    default:
                        break;
                }

                txtGenerationTime.Text = runtime;
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
        /// Save the report
        /// 
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveReport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveReport();
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
        /// method to save the report
        /// </summary>
        private void SaveReport()
        {
            int i = 0;
            try
            {
                if (IsValidData())
                {
                    string grouping1 = string.Empty;
                    string grouping2 = string.Empty;
                    string grouping3 = string.Empty;
                    string grouping = string.Empty;// grouping1 + ", " + grouping2 + ", " + grouping3;
                    string reportName = txtSaveAs.Text.Trim();
                    string whereClause = string.Empty;
                    whereClause = txtReportFilter.Text.Trim();
                    DateTime startDate = DateTime.Parse(dtStartDate.Value.ToString());
                    DateTime endDate = DateTime.Parse(dtEndDate.Value.ToString());
                    String thirdpartyIDs = GetThirdPartiesString();
                    String accounts = GetAccountsString();
                    String selectedColumns = String.Join(", ", cmbSelectedColumns.Value as List<object>);
                    if (Convert.ToString(cmbGrouping1.Value) != "Select")
                    {
                        grouping1 = ((cmbGrouping1.Value as object).ToString());
                    }
                    if (Convert.ToString(cmbGrouping2.Value) != "Select")
                    {
                        grouping2 = ((cmbGrouping2.Value as object).ToString());
                    }
                    if (Convert.ToString(cmbGrouping3.Value) != "Select")
                    {
                        grouping3 = ((cmbGrouping3.Value as object).ToString());
                    }

                    string reportFormat = cmbFormat.Text;
                    if (!string.IsNullOrEmpty(grouping1))
                    {
                        grouping = grouping1 + ", ";
                    }
                    if (!string.IsNullOrEmpty(grouping2))
                    {
                        grouping = grouping + grouping2 + ", ";
                    }
                    if (!string.IsNullOrEmpty(grouping3))
                    {
                        grouping = grouping + grouping3;
                    }
                    i = SecReportTemplateManager.SaveReport(_reportID, reportName, startDate, endDate, thirdpartyIDs, accounts, selectedColumns, grouping, _cronExpression, reportFormat, whereClause);
                    if (i > 0)
                    {
                        MessageBox.Show("Report template Saved", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _isSaveRequired = false;
                        isInEditMode = false;
                        BindReportsCombo();
                        BindReportsViewCombo();
                    }
                    else
                    {
                        MessageBox.Show("Report could not be saved", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        /// Get the list of accounts in comma separated string form
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <returns>string holding comma separated account IDs</returns>
        private string GetAccountsString()
        {
            string accounts = string.Empty;
            try
            {
                List<object> accountColl = cmbAccount.Value as List<object>;
                List<string> accountStrColl = new List<string>();
                foreach (int accountID in accountColl)
                {
                    accountStrColl.Add(accountID.ToString());
                }
                accounts = String.Join(", ", accountStrColl);
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
            return accounts;
        }

        /// <summary>
        /// Get the list of third party IDs in comma separated string form
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <returns>string holding comma separated third party IDs</returns>
        private string GetThirdPartiesString()
        {
            string thirdPartyIDs = string.Empty;
            try
            {
                List<object> thirdPartyCol = cmbThirdParty.Value as List<object>;
                List<string> thirdPartyStrCol = new List<string>();
                foreach (int thirdPartyID in thirdPartyCol)
                {
                    thirdPartyStrCol.Add(thirdPartyID.ToString());
                }
                thirdPartyIDs = String.Join(", ", thirdPartyStrCol);
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
            return thirdPartyIDs;
        }

        /// <summary>
        /// check if the data put in all the controls is valid or not
        /// </summary>
        /// <returns>true if data is valid, false otherwise</returns>
        private bool IsValidData()
        {
            try
            {
                ClearErrorProvider();

                //if (Convert.ToInt32(cmbFrequency.Value) == -1)
                //{
                //    errorProvider1.SetError(cmbFrequency, "Please select a frequency for report generation");
                //    return false;
                //}
                if (string.IsNullOrEmpty(dtStartDate.Value.ToString()))
                {
                    errorProvider1.SetError(dtStartDate, "Please set a start date");
                    return false;
                }
                if (string.IsNullOrEmpty(dtEndDate.Value.ToString()))
                {
                    errorProvider1.SetError(dtEndDate, "Please set an end date");
                    return false;
                }
                //if ((cmbThirdParty.Value as List<object>).Count <= 0)
                //{
                //    errorProvider1.SetError(cmbThirdParty, "Please select at least one third party");
                //    return false;
                //}
                //if ((cmbAccount.Value as List<object>).Count <= 0)
                //{
                //    errorProvider1.SetError(cmbAccount, "Please select at least one account");
                //    return false;
                //}
                if (!IsColumnSelected())
                {
                    errorProvider1.SetError(cmbSelectedColumns, "Please select the columns to show in the report");
                    return false;
                }
                //if (string.IsNullOrEmpty(txtFrequency.Text))
                //{
                //    errorProvider1.SetError(txtFrequency, "Please set a schedule for report generation");
                //    return false;
                //}
                if (Convert.ToInt32(cmbFormat.Value) == -1)
                {
                    errorProvider1.SetError(cmbFormat, "Please select a format for the report");
                    return false;
                }
                if (string.IsNullOrEmpty(txtSaveAs.Text.Trim()))
                {
                    errorProvider1.SetError(txtSaveAs, "Please put a name for the report template");
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
            _isSaveRequired = true;
            return true;
        }

        /// <summary>
        /// set the errors for all the controls to blank
        /// </summary>
        private void ClearErrorProvider()
        {
            try
            {
                //errorProvider1.SetError(cmbFrequency, "");
                errorProvider1.SetError(cmbFormat, "");
                errorProvider1.SetError(cmbAccount, "");
                errorProvider1.SetError(cmbSelectedColumns, "");
                errorProvider1.SetError(cmbThirdParty, "");
                errorProvider1.SetError(txtFrequency, "");
                errorProvider1.SetError(txtSaveAs, "");
                //errorProvider1.SetError(cmbGrouping1, "");
                //errorProvider1.SetError(cmbGrouping2, "");
                //errorProvider1.SetError(cmbGrouping3, "");
                errorProvider1.SetError(dtStartDate, "");
                errorProvider1.SetError(dtEndDate, "");
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
        /// Check whether at least one column for report generation is selected
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <returns>true if columns are selected, false otherwise</returns>
        private bool IsColumnSelected()
        {
            try
            {
                if ((cmbSelectedColumns.Value as List<object>).Count <= 0)
                    return false;
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
        /// validate the seleted column for grouping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbGrouping1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                UltraCombo cmb = sender as UltraCombo;
                //errorProvider1.SetError(cmbGrouping1, "");
                //errorProvider1.SetError(cmbGrouping2, "");
                //errorProvider1.SetError(cmbGrouping3, "");
                errorProvider1.SetError(cmb, "");

                if (cmb.Value != null)
                {
                    if (cmb.Value.ToString() == "Select")
                        return;
                    if (cmb.Name == "cmbGrouping1")
                    {
                        if (cmb.Value.ToString().Equals(cmbGrouping2.Value.ToString()) || cmb.Value.ToString().Equals(cmbGrouping3.Value.ToString()))
                        {
                            cmb.Value = _currentGrouping;
                            errorProvider1.SetError(cmb, "Same column cannot be used for grouping twice");
                        }
                    }
                    else if (cmb.Name == "cmbGrouping2")
                    {
                        if (cmb.Value.ToString().Equals(cmbGrouping1.Value.ToString()) || cmb.Value.ToString().Equals(cmbGrouping3.Value.ToString()))
                        {
                            cmb.Value = _currentGrouping;
                            errorProvider1.SetError(cmb, "Same column cannot be used for grouping twice");
                        }
                    }
                    else if (cmb.Name == "cmbGrouping3")
                    {
                        if (cmb.Value.ToString().Equals(cmbGrouping1.Value.ToString()) || cmb.Value.ToString().Equals(cmbGrouping2.Value.ToString()))
                        {
                            cmb.Value = _currentGrouping;
                            errorProvider1.SetError(cmb, "Same column cannot be used for grouping twice");
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
        /// Get the value of combo saved in the variable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void cmbGrouping1_Enter(object sender, EventArgs e)
        //{

        //}

        private void cmbGrouping1_Leave(object sender, EventArgs e)
        {
            try
            {
                //errorProvider1.SetError(cmbGrouping1, "");
                //errorProvider1.SetError(cmbGrouping2, "");
                //errorProvider1.SetError(cmbGrouping3, "");
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

            //UltraCombo cmb = sender as UltraCombo;
            //_currentGrouping = Convert.ToString(cmb.Value);
        }

        /// <summary>
        /// Load the report details when it is selected from the combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbReports_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int reportID = Convert.ToInt32(cmbReports.Value);
                if (reportID > 0)
                {
                    _isSaveRequired = false;
                    BindReportDetails(reportID);
                    _reportID = reportID;
                }
                else
                {
                    ResetFormControls();
                    _reportID = reportID;
                    //cmbReports.Value = 0;
                    grdSecReport.DataSource = null;
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
        /// get the report details from db and bind them to appropriate columns
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="reportID">ID of the report</param>
        private void BindReportDetails(int reportID)
        {
            try
            {
                string[] accountIDColl = null;
                DataRow dr = SecReportTemplateManager.GetReportData(reportID);
                if (dr[5] != DBNull.Value && !string.IsNullOrEmpty(dr[5].ToString()))
                {
                    accountIDColl = dr[5].ToString().Split(',');
                }
                grdSecReport.DataSource = SecReportTemplateManager.GetReportGridData(accountIDColl, reportID);
                if (grdSecReport.DataSource != null)// && (grdSecReport.DataSource as DataTable).Rows.Count > 0)
                {
                    BindFormControls(dr);
                }
                grdSecReport.Rows.ExpandAll(true);
                grdSecReport.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
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
        /// Bind the form controls to the data
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="dr">datarow holding the details</param>
        private void BindFormControls(DataRow dr)
        {
            try
            {
                ResetFormControls();
                //cmbAccount.DataSource = null;
                //cmbSelectedColumns.DataSource = null;
                //if (!string.IsNullOrEmpty(dr[0].ToString()))
                //{
                //    
                //}
                if (!string.IsNullOrEmpty(dr[1].ToString()))
                {
                    txtSaveAs.Text = dr[1].ToString();
                }
                if (!string.IsNullOrEmpty(dr[2].ToString()))
                {
                    dtStartDate.Value = DateTime.Parse(dr[2].ToString());
                }
                if (!string.IsNullOrEmpty(dr[3].ToString()))
                {
                    dtEndDate.Value = DateTime.Parse(dr[3].ToString());
                }
                if (!string.IsNullOrEmpty(dr[4].ToString()))
                {
                    cmbThirdParty.Value = GetCollectionString(dr[4].ToString());
                }
                if (!string.IsNullOrEmpty(dr[5].ToString()))
                {
                    cmbAccount.Value = GetCollectionString(dr[5].ToString());
                }
                if (!string.IsNullOrEmpty(dr[6].ToString()))
                {
                    cmbSelectedColumns.Value = GetColumnCollection(dr[6].ToString());
                }
                if (!string.IsNullOrEmpty(dr[7].ToString()))
                {
                    SetGroupingColumns(dr[7].ToString());
                }
                if (!string.IsNullOrEmpty(dr[8].ToString()))
                {
                    _cronExpression = dr[8].ToString();
                    FillCronDetails(_cronExpression);
                }
                if (!string.IsNullOrEmpty(dr[9].ToString()))
                {
                    //string formType = dr[9].ToString();
                    cmbFormat.Value = Enum.Parse(typeof(ImportFormat), dr[9].ToString());
                    //cmbFormat.Value=Enum.GetValues(ImportFormat);
                }
                if (!string.IsNullOrEmpty(dr[10].ToString()))
                {
                    txtReportFilter.Text = dr[10].ToString();
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
        /// Binds the grouping columns for the seelcted report
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="strColumns">string holding the comma separated columns</param>
        private void SetGroupingColumns(string strColumns)
        {
            try
            {
                UltraGridBand band = grdSecReport.DisplayLayout.Bands[0];
                string[] grouping = strColumns.Split(',');
                if (grouping.Length == 3)
                {
                    if (!string.IsNullOrWhiteSpace(grouping[0]))
                    {
                        cmbGrouping1.Value = grouping[0].Trim();
                        if (band.Columns.Exists(cmbGrouping1.Value.ToString()))
                        {
                            band.SortedColumns.Add(cmbGrouping1.Value.ToString(), false, true);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(grouping[1]))
                    {
                        cmbGrouping2.Value = grouping[1].Trim();
                        if (band.Columns.Exists(cmbGrouping2.Value.ToString()))
                        {
                            band.SortedColumns.Add(cmbGrouping2.Value.ToString(), false, true);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(grouping[2]))
                    {
                        cmbGrouping3.Value = grouping[2].Trim();
                        if (band.Columns.Exists(cmbGrouping3.Value.ToString()))
                        {
                            band.SortedColumns.Add(cmbGrouping3.Value.ToString(), false, true);
                        }
                    }
                }
                else if (grouping.Length == 2)
                {
                    if (!string.IsNullOrWhiteSpace(grouping[0]))
                    {
                        cmbGrouping1.Value = grouping[0].Trim();
                        if (band.Columns.Exists(cmbGrouping1.Value.ToString()))
                        {
                            band.SortedColumns.Add(cmbGrouping1.Value.ToString(), false, true);
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(grouping[1]))
                    {
                        cmbGrouping2.Value = grouping[1].Trim();
                        if (band.Columns.Exists(cmbGrouping2.Value.ToString()))
                        {
                            band.SortedColumns.Add(cmbGrouping2.Value.ToString(), false, true);
                        }
                    }
                    //cmbGrouping3.Value = "Select";
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(grouping[0]))
                    {
                        cmbGrouping1.Value = grouping[0].Trim();
                        if (band.Columns.Exists(cmbGrouping1.Value.ToString()))
                        {
                            band.SortedColumns.Add(cmbGrouping1.Value.ToString(), false, true);
                        }
                    }
                    //cmbGrouping2.Value = "Select";
                    //cmbGrouping3.Value = "Select";
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
        /// reset the form controls
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void ResetFormControls()
        {
            try
            {
                //cmbReports.Value = -1;
                cmbFormat.Value = -1;
                cmbThirdParty.Value = -1;
                cmbGrouping3.Value = cmbGrouping2.Value = cmbGrouping1.Value = "Select";
                dtEndDate.Value = dtStartDate.Value = DateTime.UtcNow;
                cmbAccount.Value = new List<int>();
                cmbSelectedColumns.Value = new List<String>();
                txtFrequency.Text = txtGenerationTime.Text = string.Empty;
                txtSaveAs.Text = string.Empty;
                _cronExpression = string.Empty;
                txtReportFilter.Text = string.Empty;
                txtLastRunDate.Text = string.Empty;
                //txtEndDate.Text = txtStartDate.Text=txtOutput.Text=txtLastRunDate.Text = string.Empty;
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
        /// Get the integer collection of IDs
        /// </summary>
        /// <param name="CollectionString">string holding the comma separated IDs</param>
        /// <returns>list of integers</returns>
        private List<int> GetCollectionString(string collectionString)
        {
            List<int> collectionList = new List<int>();
            try
            {
                if (!string.IsNullOrEmpty(collectionString))
                {
                    string[] strCollection = collectionString.Split(',');
                    foreach (string s in strCollection)
                    {
                        collectionList.Add(Convert.ToInt32(s.Trim()));
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
            return collectionList;
        }

        /// <summary>
        /// Get the integer collection of account ids
        /// </summary>
        /// <param name="columnString">string holding the comma separated account ids</param>
        /// <returns>list of integers</returns>
        private List<string> GetColumnCollection(string columnString)
        {
            List<string> colList = new List<string>();
            try
            {
                if (!string.IsNullOrEmpty(columnString))
                {
                    string[] strAccounts = columnString.Split(',');
                    foreach (string s in strAccounts)
                    {
                        colList.Add(s.Trim());
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
            return colList;
        }

        /// <summary>
        /// refresh the grouping columns list when the value in the selected columns combo is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSelectedColumns_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                BindGroupingColumns();
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
        /// Bind the controls to the data on the index changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbReportName_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int reportID = Convert.ToInt32(cmbReportName.Value);
                if (reportID > 0)
                {
                    BindReportViewDetails(reportID);
                    _reportID = reportID;
                }
                else
                {
                    ResetFormControls();
                    _reportID = reportID;
                    //cmbReports.Value = 0;
                    grdSecReport.DataSource = null;
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
        /// Bind the details of the report for viewing purpose
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="reportID">ID of the report</param>
        private void BindReportViewDetails(int reportID)
        {
            try
            {
                DataRow dr = SecReportTemplateManager.GetReportData(reportID);
                if (dr != null)
                {
                    BindReportViewControls(dr);
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
        /// Bind the controls of the view tab for viewing the report
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="dr">Datarow holding the data</param>
        private void BindReportViewControls(DataRow dr)
        {
            try
            {
                ResetReportViewControls();
                //The dates should be current date and not what was saved in the report. The date in the report should not be saved
                //if (!string.IsNullOrEmpty(dr[2].ToString()))
                //{
                //    //txtStartDate.Value = DateTime.Parse(dr[2].ToString());
                //}
                //if (!string.IsNullOrEmpty(dr[3].ToString()))
                //{
                //    //txtEndDate.Value = DateTime.Parse(dr[3].ToString());
                //}
                //if (!string.IsNullOrEmpty(dr[7].ToString()))
                //{
                //    SetGroupingColumns(dr[7].ToString());
                //}
                if (!string.IsNullOrEmpty(dr[9].ToString()))
                {
                    //string formType = dr[9].ToString();
                    txtOutput.Value = Enum.Parse(typeof(ImportFormat), dr[9].ToString());
                }
                if (!string.IsNullOrEmpty(dr["LastRunTime"].ToString()))
                {
                    txtLastRunDate.Text = Convert.ToDateTime(dr["LastRunTime"]).ToString("MM/dd/yyyy");
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
        private void ResetReportViewControls()
        {
            try
            {
                //txtEndDate.Value = txtStartDate.Value = DateTime.UtcNow;
                txtLastRunDate.Value = string.Empty;
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
        //grid.DisplayLayout.Bands[0].SortedColumns.Add(columnName, false, true);

        /// <summary>
        /// Bind the reports combo to data
        /// </summary>
        private void BindReportsViewCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();

                Dictionary<int, string> dictReports = SecReportTemplateManager.GetReports();
                foreach (KeyValuePair<int, string> kvp in dictReports)
                {
                    EnumerationValue value = new EnumerationValue(kvp.Value, kvp.Key);
                    listValues.Add(value);
                }
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));
                cmbReportName.DataSource = null;
                cmbReportName.DataSource = listValues;
                cmbReportName.DisplayMember = "DisplayText";
                cmbReportName.ValueMember = "Value";
                cmbReportName.DataBind();
                cmbReportName.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbReportName.Value = -1;
                cmbReportName.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
        /// show the consolidated report in the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cmbReportName.Value) < 0)
                {
                    MessageBox.Show("Select a report to show the data", "Reports Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int reportID = Convert.ToInt32(cmbReportName.Value);

                #region update last run date in DB
                SecReportTemplateManager.UpdateReport(reportID);
                #endregion

                DataRow dr = SecReportTemplateManager.GetReportData(reportID);

                //if (dr[5] != DBNull.Value && !string.IsNullOrEmpty(dr[5].ToString()))
                //{
                string[] accountIDColl = dr[5].ToString().Split(',');
                grdSecReport.DataSource = SecReportTemplateManager.GetReportGridData(accountIDColl, reportID);
                if (grdSecReport.DataSource != null)// && (grdSecReport.DataSource as DataTable).Rows.Count > 0)
                {
                    BindReportViewControls(dr);
                }
                grdSecReport.Rows.ExpandAll(true);
                grdSecReport.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
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
        /// Remove the report from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cmbReportName.Value) < 0)
                {
                    MessageBox.Show("Select a report to Remove", "Reports Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                DialogResult dr = MessageBox.Show("Are you sure to delete this report?", "Reports Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    return;
                }
                int reportID = Convert.ToInt32(cmbReportName.Value);
                int i = SecReportTemplateManager.DeleteReport(reportID);
                if (i > 0)
                {
                    MessageBox.Show("Report Deleted Successfully", "Delete Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //InitializeData();
                    ResetFormControls();
                    ClearErrorProvider();
                    BindReportsCombo();
                    BindReportsViewCombo();
                }
                else
                {
                    MessageBox.Show("Report could not be deleted", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// set the tab when it is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                this.splitContainer1.SplitterDistance = 160;

                if (ultraTabControl1.SelectedTab.Key == "tbpCreateReport")
                {
                    grdSecReport.DataSource = null;
                    if (isInEditMode)
                    {
                        cmbReports.Value = _reportID;
                        BindReportDetails(_reportID);
                    }
                    else
                    {
                        cmbReports.Value = -1;
                    }
                    btnSaveReport.Visible = true;
                    grdSecReport.Visible = true;
                }
                else if (ultraTabControl1.SelectedTab.Key == "tbpViewReport")
                {
                    grdSecReport.DataSource = null;
                    cmbReportName.Value = -1;
                    isInEditMode = false;
                    btnSaveReport.Visible = false;
                    grdSecReport.Visible = true;
                }
                else if (ultraTabControl1.SelectedTab.Key == "tbpSMBatchReport")
                {
                    grdSecReport.Visible = false;
                    grdSecReport.DataSource = null;
                    cmbReportName.Value = -1;
                    isInEditMode = false;
                    btnSaveReport.Visible = false;

                    this.splitContainer1.SplitterDistance = this.Height - 30;
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
        /// Modify the report details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                int reportID = Convert.ToInt32(cmbReportName.Value);
                if (reportID <= 0)
                {
                    MessageBox.Show("Select a report to modify first", "Modify Report Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                isInEditMode = true;
                _reportID = reportID;
                ClearErrorProvider();
                ResetFormControls();
                ultraTabControl1.SelectedTab = ultraTabControl1.Tabs["tbpCreateReport"];
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
        /// Check if the details are to be saved before tab change
        /// 
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraTabControl1_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            try
            {
                if (_isSaveRequired)
                {
                    DialogResult dr = MessageBox.Show("Report template is not saved. Save now?", "Save Report Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        SaveReport();
                    }
                    else
                    {
                        _isSaveRequired = false;
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
        /// Initialize the data on teh form load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlReportTemplate_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    InitializeData();
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
                btnAdvancedSearch.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAdvancedSearch.ForeColor = System.Drawing.Color.White;
                btnAdvancedSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAdvancedSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAdvancedSearch.UseAppStyling = false;
                btnAdvancedSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnGetSchedule.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGetSchedule.ForeColor = System.Drawing.Color.White;
                btnGetSchedule.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGetSchedule.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGetSchedule.UseAppStyling = false;
                btnGetSchedule.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnExportReport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnExportReport.ForeColor = System.Drawing.Color.White;
                btnExportReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnExportReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnExportReport.UseAppStyling = false;
                btnExportReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRun.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnRun.ForeColor = System.Drawing.Color.White;
                btnRun.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRun.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRun.UseAppStyling = false;
                btnRun.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRemove.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRemove.ForeColor = System.Drawing.Color.White;
                btnRemove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRemove.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRemove.UseAppStyling = false;
                btnRemove.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnModify.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnModify.ForeColor = System.Drawing.Color.White;
                btnModify.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnModify.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnModify.UseAppStyling = false;
                btnModify.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnViewReport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnViewReport.ForeColor = System.Drawing.Color.White;
                btnViewReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnViewReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnViewReport.UseAppStyling = false;
                btnViewReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSaveReport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSaveReport.ForeColor = System.Drawing.Color.White;
                btnSaveReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSaveReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSaveReport.UseAppStyling = false;
                btnSaveReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportReport_Click(object sender, EventArgs e)
        {
            try
            {
                //string filePath = @"D:\Nirvana\NirvanaCode\SourceCode\Dev\Prana\Application\Prana.Client\Prana\bin\Debug\ReconData";
                UltraGridFileExporter.LoadFilePathAndExport(grdSecReport, this);
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

        private void grdSecReport_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                grdSecReport.Rows.ExpandAll(true);
                grdSecReport.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
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
        /// To enable advance search option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (advSearchFilertUI == null)
                {
                    advSearchFilertUI = new AdvSearchFilterUI();
                    Dictionary<String, ValueList> dataValuesList = SecMasterHelper.getInstance().GetRequiredValueListDict();
                    List<String> columnsList = new List<string>();
                    columnsList.Add("AssetID");
                    columnsList.Add("CreatedBy");
                    columnsList.Add("ModifiedBy");
                    columnsList.Add("ApprovedBy");
                    columnsList.Add("CreationDate");
                    columnsList.Add("ModifiedDate");
                    columnsList.Add("DataSource");

                    advSearchFilertUI.SetUp(typeof(SecMasterUIObj), dataValuesList, columnsList, false);
                    advSearchFilertUI.FormClosed += new FormClosedEventHandler(advSearchFilertUI_FormClosed);
                    advSearchFilertUI.SearchDataEvent += advSearchFilertUI_SearchDataEvent;
                }
                advSearchFilertUI.StartPosition = FormStartPosition.Manual;
                advSearchFilertUI.Show();
                //BringFormToFront(advSearchFilertUI);
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
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void advSearchFilertUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (advSearchFilertUI != null)
                {
                    advSearchFilertUI = null;
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
        /// <param name="Queury"></param>
        void advSearchFilertUI_SearchDataEvent(object sender, EventArgs<string> e)
        {
            try
            {
                //_isAdvncdSearching = true;
                advSearchFilertUI.Close();
                txtReportFilter.Text = e.Value;
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

        private void dtRunReportEndDate_AfterCloseUp(object sender, EventArgs e)
        {
            try
            {
                DateTime endDate = DateTime.Parse(dtRunReportEndDate.Value.ToString());
                DateTime startDate = DateTime.Parse(dtRunReportStartDate.Value.ToString());

                if (endDate < startDate)
                {
                    MessageBox.Show("End Date cannot be less than the start date", "SM Report Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dtRunReportEndDate.Value = dtRunReportStartDate.Value;
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
