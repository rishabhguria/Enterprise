using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
//using Prana.Reconciliation;
using Prana.ReconciliationNew;
using Prana.Utilities.UI;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class PranaPositionViewer : UserControl, IPluggableUserControl
    {
        BackgroundWorker _bgGetData = null;
        public event EventHandler DataReloaded;
        public event EventHandler FilterChanged;
        //int _hashCode = 0;
        DataTable dt = new DataTable();
        //Dictionary<string, List<DataRow>> _dictData = new Dictionary<string, List<DataRow>>();

        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public PranaPositionViewer()
        {
            try
            {
                InitializeComponent();
                //_hashCode = this.GetHashCode();
                grdData.DisplayLayout.Override.RowAppearance.BackColor = Color.Black;
                grdData.DisplayLayout.Override.RowAlternateAppearance.ForeColor = Color.Green;
                grdData.DisplayLayout.Override.RowAppearance.ForeColor = Color.LightGray;
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

        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="name"></param>
        public void SetUp(DataTable dt, string name)
        {

            try
            {
                this.Name = name;
                BindClientCombo();
                //BindReconCombo();
                //BindReconTemplatesCombo();

                //  BindAccountCombo();
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

        void _bgGetData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            try
            {

                DataSet ds = e.Result as DataSet;
                FilterAndBindData(ds);


                //if (ds != null && ds.Tables.Count > 0)
                //{
                //    dt = ds.Tables[0];
                //}
                //if (dt.Rows.Count > 0)
                //{
                //    if (cmbbxReconTemplates.Value != null && cmbbxReconType.Value != null)
                //    {
                //        string templateName = cmbbxReconTemplates.Value.ToString();
                //        string strReconType = cmbbxReconType.Value.ToString();

                //        Dictionary<ReconFilterType, Dictionary<int, string>> dictReconFilters = ReconPrefManager.ReconPreferences.GetReconFilters(templateName);

                //        if (dictReconFilters != null)
                //        {
                //            DataTable dtFiltered = FilteringLogic.GetFilteredData(dictReconFilters, dt);
                //            dt = dtFiltered;

                //        }
                //        GroupingCriteria criteria = ReconPrefManager.ReconPreferences.GetGroupingCriteria(templateName);
                //        GroupingLogic.Group(dt, criteria);
                //        dt.AcceptChanges();
                //        List<MasterColumn> listColumns = ReconPrefManager.ReconPreferences.GetNirvanaMasterColumns(templateName);
                //        ReconUtilities.AddCustomColumns(dt, listColumns);


                //    }
                //}
                //BindData();
                //dt.AcceptChanges();
                //btnView.Enabled = true;
                //btnView.Text = "View";
                //btnView.Appearance.BackColor = Color.FromArgb(192, 192, 255);
                //btnView.Appearance.BackColor2 = Color.FromArgb(192, 192, 255);
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
            finally
            {
                btnView.Enabled = true;
            }
        }

        void _bgGetData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //object[] arguments = e.Argument as  object[];
                //string strReconType = arguments[0].ToString();
                //string commaSeparatedAssetIDs = arguments[1].ToString();
                //string commaSeparatedAccountIDs = arguments[2].ToString();

                //DataSet ds = new DataSet();

                //if (strReconType.Equals(((int)ReconType.Position).ToString()))
                //{
                //    ds = _pranaPositionServices.InnerChannel.GetOpenPositionsFromDBWithMktValue(dtDatePicker.DateTime, false, commaSeparatedAssetIDs.ToString(), commaSeparatedAccountIDs.ToString());
                //}
                //else
                //{
                //    ds = _pranaPositionServices.InnerChannel.GetOpenPositionsFromDBWithMktValue(dtDatePicker.DateTime, true, commaSeparatedAssetIDs.ToString(), commaSeparatedAccountIDs.ToString());
                //}

                e.Result = FetchPositions(e.Argument);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private DataSet FetchPositions(object arg)
        {
            DataSet ds = new DataSet();
            try
            {
                ReconParameters reconParameters = new ReconParameters();
                object[] arguments = arg as object[];
                reconParameters.ClientID = arguments[0].ToString();
                reconParameters.ReconType = arguments[1].ToString();
                reconParameters.TemplateName = arguments[2].ToString();
                string commaSeparatedAssetIDs = arguments[3].ToString();
                string commaSeparatedAccountIDs = arguments[4].ToString();
                reconParameters.DTFromDate = dtFromDatePicker.DateTime;
                reconParameters.DTToDate = dtToDatePicker.DateTime;
                reconParameters.DTRunDate = DateTime.Now;
                //reconParameters.TemplateKey = ReconUtilities.GetTemplateKeyFromParameters(clientID, strReconType, templateName);
                reconParameters.SpName = ReconPrefManager.ReconPreferences.GetTemplates(reconParameters.TemplateKey).SpName;
                reconParameters.IsShowCAGeneratedTrades = ReconPrefManager.ReconPreferences.GetTemplates(reconParameters.TemplateKey).IsShowCAGeneratedTrades;
                //if sp name is not empty
                if (!string.IsNullOrWhiteSpace(reconParameters.SpName))
                {
                    ds = ReconUtilities.FetchDataForGivenSPName(reconParameters, commaSeparatedAssetIDs, commaSeparatedAccountIDs.ToString());
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
            return ds;
        }
        /// <summary>
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="ds"></param>
        private void FilterAndBindData(DataSet ds)
        {
            try
            {

                //  DataSet ds = e.Result as DataSet;

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                if (dt.Rows.Count > 0)
                {
                    if (cmbbxReconTemplates.Value != null && cmbbxReconType.Value != null)
                    {
                        //TODO:Make template key generation code generic, Here we are making template key from clientid, recontype and template name each time
                        //we can make this generic by addding key to template combobox                        
                        string templateName = cmbbxReconTemplates.Value.ToString();
                        string strReconType = cmbbxReconType.Text;
                        string templateKey = ReconUtilities.GetTemplateKeyFromParameters(cmbbxClient.Value.ToString(), strReconType, templateName);
                        dt = ReconManager.ProcessReconData(dt, templateKey, DataSourceType.Nirvana, null);
                    }
                }
                BindData();
                dt.AcceptChanges();
                btnView.Enabled = true;
                btnView.Text = "View";
                btnView.Appearance.BackColor = Color.FromArgb(192, 192, 255);
                btnView.Appearance.BackColor2 = Color.FromArgb(192, 192, 255);
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
        //Todo: This method is of no use code moved to ReconManager
        //private void UpdateTableSchemaForCustomCoulms()
        //{
        //    try
        //    {
        //        DataTable NewDT = dt.Copy();
        //        List<string> lstCustomColumns = new List<string>();
        //        //make expression column readonly property to false so that on that column grouping can be done
        //        //http://jira.nirvanasolutions.com:8080/browse/GUGGENHEIM-12
        //        foreach (DataColumn col in dt.Columns)
        //        {
        //            if (!string.IsNullOrEmpty(col.Expression))
        //            {
        //                col.Expression = string.Empty;
        //                col.ReadOnly = false;
        //                lstCustomColumns.Add(col.ColumnName);
        //            }
        //        }
        //        if (lstCustomColumns.Count > 0)
        //        {
        //            dt.Clear();
        //            foreach (DataRow row in NewDT.Rows)
        //            {
        //                DataRow newRow = dt.NewRow();
        //                newRow.ItemArray = row.ItemArray;
        //                dt.Rows.Add(newRow);
        //            }
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
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void BindReconCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();// EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(ReconType));

                int i = 0;
                foreach (string reconTemplate in ReconPrefManager.ReconPreferences.getRootTemplates())
                {
                    EnumerationValue value = new EnumerationValue(reconTemplate, i);
                    listValues.Add(value);
                    i++;
                }
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));

                cmbbxReconType.DataSource = null;
                cmbbxReconType.DataSource = listValues;
                cmbbxReconType.DisplayMember = "DisplayText";
                cmbbxReconType.ValueMember = "Value";
                cmbbxReconType.DataBind();
                cmbbxReconType.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbbxReconType.Value = -1;
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
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void BindReconTemplatesCombo()
        {

            try
            {
                int clientType = (int)(cmbbxClient.Value);
                string reconType = cmbbxReconType.Text.ToString();
                if (!reconType.Equals("-1"))
                {
                    List<string> listCorrespondingTemplates = ReconPrefManager.ReconPreferences.GetListOfTemplates(reconType, clientType);
                    cmbbxReconTemplates.DataSource = null;
                    cmbbxReconTemplates.DataSource = listCorrespondingTemplates;
                    cmbbxReconTemplates.DataBind();
                    cmbbxReconTemplates.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    if (listCorrespondingTemplates.Count > 0)
                    {
                        cmbbxReconTemplates.Value = listCorrespondingTemplates[0];
                    }
                    int i = 0;
                    //Narendra Kumar Jangir 2012/08/17 
                    //show tooltip to show template name onmousehover of recotemplate combobox
                    foreach (string templateName in listCorrespondingTemplates)
                    {
                        cmbbxReconTemplates.Rows[i].ToolTipText = templateName;
                        i++;
                    }
                    cmbbxReconTemplates.DisplayLayout.Bands[0].Columns["Value"].Width = cmbbxReconTemplates.Width;


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

                cmbbxClient.DataSource = null;
                cmbbxClient.DataSource = listValues;
                cmbbxClient.DisplayMember = "DisplayText";
                cmbbxClient.ValueMember = "Value";
                cmbbxClient.DataBind();
                cmbbxClient.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbbxClient.Value = -1;
                cmbbxClient.DisplayLayout.Bands[0].ColHeadersVisible = false;

                BindReconCombo();
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

        //private void BindAccountCombo()
        //{
        //    try
        //    {
        //        AccountCollection _accountsList = new AccountCollection();
        //        _accountsList = CachedDataManager.GetInstance.GetUserAccounts();

        //        cmbAccount.DataSource = _accountsList;
        //        cmbAccount.ValueMember = "AccountID";
        //        cmbAccount.DisplayMember = "Name";
        //        Utils.UltraComboFilter(cmbAccount, "Name");               
        //        cmbAccount.Value = int.MinValue;
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


        #region IPluggableUserControl Members

        public DataTable Data
        {
            get { return dt; }
            set { dt = value; }
        }
        /// <summary>
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void BindData()
        {
            try
            {
                grdData.DataSource = dt;
                string templateName = cmbbxReconTemplates.Value.ToString();
                if (!string.IsNullOrEmpty(templateName))
                {
                    string templateKey = ReconUtilities.GetTemplateKeyFromParameters(cmbbxClient.Value.ToString(), cmbbxReconType.Text, templateName);
                    List<string> listColumns = ReconPrefManager.ReconPreferences.GetNirvanaGridDisplayColumnNames(templateKey);
                    ReconUtilities.SetGridColumns(grdData, listColumns);
                }
                else
                {
                    ReconUtilities.SetGridColumns(grdData, _gridColumnList);
                }
                //UltraWinGridUtils.SetColumns(_gridColumnList, grdData);

                grdData.DataBind();
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
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void ExportToExcel()
        {
            try
            {
                ExcelAndPrintUtilities excelUtils = new ExcelAndPrintUtilities();
                excelUtils.ExportToExcel(grdData);
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
        public UserControl Control
        {
            get { return this; }
        }
        #endregion

        List<string> _gridColumnList = new List<string>();
        public List<string> DisplayedColumnList
        {
            set
            {
                _gridColumnList = value;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validate())
                {
                    ViewPositions(true);
                    dataReLoaded = true;
                    if (DataReloaded != null)
                    {
                        DataReloaded(null, null);
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
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="fetchDataAsync"></param>
        private void ViewPositions(bool fetchDataAsync)
        {
            try
            {

                btnView.Enabled = false;
                btnView.Text = "Fetching Data...";
                btnView.Appearance.BackColor = Color.Red;
                btnView.Appearance.BackColor2 = Color.Red;

                _bgGetData = new BackgroundWorker();
                _bgGetData.DoWork += new DoWorkEventHandler(_bgGetData_DoWork);
                _bgGetData.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgGetData_RunWorkerCompleted);
                //ReconType reconType;
                string clientID = cmbbxClient.Value.ToString();
                string strReconType = cmbbxReconType.Text.ToString();
                string templateName = cmbbxReconTemplates.Value.ToString();
                //all the fetching of data is based on templte key rather than template name
                string templateKey = ReconUtilities.GetTemplateKeyFromParameters(clientID, strReconType, templateName);

                string commaSeparatedAssetIDs = ReconUtilities.GetCommaSeparatedAssetIds(templateKey);
                StringBuilder commaSeparatedAccountIDs = ReconUtilities.GetCommaSeparatedAccountIds(templateKey);

                object[] arguments = new object[5];
                arguments[0] = clientID;
                arguments[1] = strReconType;
                arguments[2] = templateName;
                arguments[3] = commaSeparatedAssetIDs;
                arguments[4] = commaSeparatedAccountIDs;


                if (fetchDataAsync)
                {
                    _bgGetData.RunWorkerAsync(arguments);

                }
                else
                {

                    DataSet ds = FetchPositions(arguments);
                    FilterAndBindData(ds);
                }

                #region commented
                //string AUECWithDate = TimeZoneHelper.GetSameDateInUseAUECStr(dtToDatePickerAllocation.DateTime);
                //if (strReconType.Equals(((int)ReconType.Position).ToString()))
                //{
                //    DataSet ds = _pranaPositionServices.InnerChannel.GetOpenPositionsFromDBWithMktValue(dtDatePicker.DateTime, false, commaSeparatedAssetIDs.ToString(), commaSeparatedAccountIDs.ToString());
                //    if (ds.Tables.Count > 0)
                //    {
                //        dt = ds.Tables[0];
                //    }
                //}
                //else
                //{
                //    DataSet ds = _pranaPositionServices.InnerChannel.GetOpenPositionsFromDBWithMktValue(dtDatePicker.DateTime, true, commaSeparatedAssetIDs.ToString(), commaSeparatedAccountIDs.ToString());
                //    if (ds.Tables.Count > 0)
                //    {
                //        dt = ds.Tables[0];
                //    }
                //}
                //string spName = RuleCacheManager.GetSPForReconType(strReconType);
                //int accountID =Convert.ToInt32(cmbAccount.Value.ToString());
                //if (accountID.Equals(int.MinValue))
                //{
                //    accountID = 0;
                //}
                //if (String.IsNullOrEmpty(spName))
                //{
                //    MessageBox.Show("Set SP Name for the Recon Type in XmlMatchingRule.xml file.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                // dt = PranaDataManager.GetAllInternalPositions(AUECWithDate, int.Parse(cmbbxPrimeBroker.Value.ToString()), dtToDatePickerAllocation.DateTime, strReconType, spName, accountID);
                //if (dictReconFilters != null)
                //{
                //    //  string spName = ReconPrefManager.GetSPForReconType(strReconType);
                //    //int accountID = Convert.ToInt32(cmbAccount.Value.ToString());
                //    //if (accountID.Equals(int.MinValue))
                //    //{
                //    DataTable dtFiltered = FilteringLogic.GetFilteredData(dictReconFilters, dt);
                //    dt = dtFiltered;
                //    // }
                //}
                // List<MatchingRule> rules = ReconPrefManager.GetListOfRules("GroupingAccountSymbol");
                //GroupingCriteria criteria = ReconPrefManager.ReconPreferences.GetGroupingCriteria(templateName);
                //if (strReconType.Equals(((int)ReconType.Position).ToString()))
                //{

                //  GroupingLogic.Group(dt, criteria);

                // dt.AcceptChanges();
                // }
                //NewUtilities.AddPrimaryKey(dt);
                //  //ReconUtilities.AddPrimaryKey(dt);
                // List<MasterColumn> listColumns = ReconPrefManager.ReconPreferences.GetNirvanaMasterColumns(templateName);
                // ReconUtilities.AddCustomColumns(dt, listColumns);
                ///  BindData();

                //foreach (DataRow row in dt.Rows)
                //{
                //    string symbol = row["Symbol"].ToString();

                //    if (!_dictData.ContainsKey(symbol))
                //    {
                //        List<DataRow> templistobj = new List<DataRow>();
                //        templistobj.Add(row);
                //        _dictData.Add(symbol, templistobj);
                //    }
                //    else
                //    {
                //        _dictData[symbol].Add(row);
                //    }
                //}
                //dt.AcceptChanges();
                #endregion

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

        //void _secMaster_SecMstrDataResponse(SecMasterBaseObj secMasterObj)
        //{
        //    try
        //    {
        //        foreach (DataRow row in _dictData[secMasterObj.TickerSymbol])
        //        {
        //            row["CompanyName"] = secMasterObj.LongName;
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
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <returns></returns>
        public new bool Validate()
        {
            try
            {

                if (cmbbxReconType.Value.ToString().Equals("-1"))
                {
                    MessageBox.Show("Please select a Recon Type.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbbxReconType.Focus();
                    return false;
                }

                if (cmbbxReconTemplates.Value == null || cmbbxReconTemplates.Value.ToString().Equals(int.MinValue.ToString()))
                {
                    MessageBox.Show("Please select a Template.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbbxReconTemplates.Focus();
                    return false;
                }
                //Check if the template was modified so that combo can be reset
                string templateKey = ReconUtilities.GetTemplateKeyFromParameters(cmbbxClient.Value.ToString(), cmbbxReconType.Text, cmbbxReconTemplates.Text);
                if (!ReconPrefManager.ReconPreferences.DictReconTemplates.ContainsKey(templateKey))
                {
                    ResetReconTypeCombo();
                    return false;
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
        /// reset combo of template if any template has been modified can be 
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-4555
        /// </summary>
        private void ResetReconTypeCombo()
        {
            try
            {
                string reconType = cmbbxReconType.Value.ToString();
                cmbbxReconType.Value = null;
                cmbbxReconType.Value = reconType;
                MessageBox.Show("Template was modified. Please try again.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        bool dataReLoaded = false;
        public void Reload()
        {
            try
            {
                if (!dataReLoaded)
                {
                    ViewPositions(false);
                }
                dataReLoaded = false;
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

        //public Infragistics.Win.UltraWinGrid.UltraCombo  PrimeBrokerCombo
        //{
        //    get { return cmbbxPrimeBroker; }
        //}

        //public Infragistics.Win.UltraWinGrid.UltraCombo ReconTypeCombo
        //{
        //    get { return cmbbxReconType; }
        //}

        private void grdData_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

            try
            {
                if (grdData.DisplayLayout.Bands[0].Columns.Exists("Quantity"))
                {
                    Infragistics.Win.UltraWinGrid.UltraGridColumn colQuantity = grdData.DisplayLayout.Bands[0].Columns["Quantity"];
                    //CHMW-2955	CLONE -Quantity digit round off issue in Transaction Recon.
                    //colQuantity.Format = "#,#.####";

                    //Pranay Deep Oct 14 2015
                    //changing the Format, as "0" was not handled earlier.
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-11581
                    colQuantity.Format = "#,0.####";

                }
                //Narendra Kumar jangir 2012 Nov 01
                //disable calendar for date time fields and disable editing for all the columns
                foreach (UltraGridColumn column in grdData.DisplayLayout.Bands[0].Columns)
                {
                    column.CellActivation = Activation.ActivateOnly;
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

        //private void SetGridColumns(UltraGrid grid, List<string> listColumns)
        //{
        //    Infragistics.Win.UltraWinGrid.ColumnsCollection columns = grdData.DisplayLayout.Bands[0].Columns;
        //    if (listColumns != null)
        //    {
        //        //Hide all columns
        //        foreach (UltraGridColumn col in columns)
        //        {
        //            columns[col.Key].Hidden = true;
        //            col.ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
        //        }
        //        //Unhide and Set postions for required columns
        //        int visiblePosition = 1;
        //        foreach (string col in listColumns)
        //        {
        //            if (columns.Exists(col))
        //            {
        //                UltraGridColumn column = columns[col.ColumnName];
        //                column.Hidden = false;
        //                column.Header.VisiblePosition = visiblePosition;
        //                column.Width = 80;
        //                column.ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
        //                visiblePosition++;
        //            }
        //        }
        //    }


        //}

        private void grdData_AfterRowFilterChanged(object sender, Infragistics.Win.UltraWinGrid.AfterRowFilterChangedEventArgs e)
        {
            try
            {
                if (FilterChanged != null)
                {
                    FilterChanged(sender, e);
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
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        //private ValueList GetValueList(List<string> array)
        //{
        //    ValueList coll = new ValueList();
        //    try
        //    {

        //        coll.ValueListItems.Add(ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT);
        //        foreach (string name in array)
        //        {
        //            coll.ValueListItems.Add(name, name);
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
        //    return coll;
        //}


        #region IPluggableUserControl Members

        public void ApplyFilters(object sender, EventArgs e)
        {
            try
            {
                AfterRowFilterChangedEventArgs ef = (AfterRowFilterChangedEventArgs)e;
                if (grdData.DisplayLayout.Bands[0].ColumnFilters.Exists(ef.Column.Key))
                {
                    grdData.DisplayLayout.Bands[0].ColumnFilters[ef.Column.Key].ClearFilterConditions();

                    foreach (FilterCondition fc in ef.NewColumnFilter.FilterConditions)
                    {
                        grdData.DisplayLayout.Bands[0].ColumnFilters[ef.Column.Key].FilterConditions.Add((FilterCondition)fc.Clone());
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
        public void ValueSelected(object sender, EventArgs e)
        {

        }
        public string GetSelectedValue(int type)
        {
            string value = string.Empty;
            switch (type)
            {
                case 0:
                    value = cmbbxClient.Value.ToString();
                    break;
                case 1:
                    value = cmbbxReconType.Text.ToString();
                    break;
                case 2:
                    if (cmbbxReconTemplates.Value != null)
                    {
                        value = cmbbxReconTemplates.Value.ToString();
                    }
                    break;
                case 3:
                    value = dtFromDatePicker.Value.ToString();
                    break;
                case 4:
                    value = dtToDatePicker.Value.ToString();
                    break;
            }
            return value;
        }
        public event EventHandler SelectedValueChanged;
        #endregion
        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbbxReconType_ValueChanged(object sender, EventArgs e)
        {
            //if (SelectedValueChanged != null)
            //{
            //    List<string> values = new List<string>();
            //    values.Add(cmbbxReconType.Text.ToString());
            //    values.Add(cmbbxReconTemplates.Text.ToString());
            //    SelectedValueChanged(values, null);
            //}

            //Disable ToDate for position template, because position is not between date range.
            //TODO: Position is hard coded change it
            try
            {
                if (cmbbxReconType.Text.ToString().Equals("Position"))
                {
                    dtFromDatePicker.Enabled = false;
                }
                else
                {
                    dtFromDatePicker.Enabled = true;
                }
                BindReconTemplatesCombo();
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
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbbxReconTemplates_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (SelectedValueChanged != null && !string.IsNullOrEmpty(cmbbxReconTemplates.Text))
                {
                    List<string> values = new List<string>();
                    // values.Add(cmbbxReconType.Text.ToString());
                    string templateKey = ReconUtilities.GetTemplateKeyFromParameters(cmbbxClient.Value.ToString(), cmbbxReconType.Text, cmbbxReconTemplates.Text);
                    //Check if the template was modified so that combo can be reset
                    if (!ReconPrefManager.ReconPreferences.DictReconTemplates.ContainsKey(templateKey))
                    {
                        ResetReconTypeCombo();
                        return;
                    }
                    values.Add(templateKey);
                    values.Add(dtFromDatePicker.Value.ToString());
                    values.Add(dtToDatePicker.Value.ToString());
                    //cmbbxReconTemplates.SelectedRow.ToolTipText = cmbbxReconTemplates.Text.ToString();

                    SelectedValueChanged(values, null);
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
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void grdData_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdData);
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
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtDatePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (SelectedValueChanged != null)
                {
                    //check if client recontype and template is selected or not
                    if (cmbbxClient.Value.ToString() == "-1" || string.IsNullOrWhiteSpace(cmbbxReconTemplates.Text) || cmbbxReconType.Text == ReconConstants.SelectDefaultValue)
                    {
                        return;
                    }
                    List<string> values = new List<string>();
                    // values.Add(cmbbxReconType.Text.ToString());
                    string templateKey = ReconUtilities.GetTemplateKeyFromParameters(cmbbxClient.Value.ToString(), cmbbxReconType.Text, cmbbxReconTemplates.Text);
                    values.Add(templateKey);
                    values.Add(dtFromDatePicker.Value.ToString());
                    values.Add(dtToDatePicker.Value.ToString());
                    SelectedValueChanged(values, null);
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
        //private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        //{

        //}
        /// <summary>
        ///modified by: sachin mishra 30 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtFromDatePicker_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                dtToDatePicker.Value = dtFromDatePicker.Value;
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
        private void cmbbxClient_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbbxClient.Text.Equals(ApplicationConstants.C_COMBO_SELECT))
                {
                    //set recontype and template to -select- if client is -select-
                    //cmbbxReconType.Value = -1;
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
            BindReconTemplatesCombo();

        }

        private void grdData_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void cmbbxReconTemplates_BeforeDropDown(object sender, CancelEventArgs e)
        {
            cmbbxReconTemplates.DisplayLayout.Bands[0].Columns["Value"].Width = cmbbxReconTemplates.Width;
        }
        //private void cmbbxReconTemplates_MouseHover(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        cmbbxReconTemplates.ToolTipText = cmbbxReconTemplates.Text;
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
    }
}