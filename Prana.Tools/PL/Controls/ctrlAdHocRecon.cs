using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Enums;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.TaskManagement.Definition.Definition;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace Prana.Tools
{
    public partial class ctrlAdHocRecon : UserControl
    {
        #region Commented Code
        /// <summary>
        ///  return the bool value weather recon report is save dor not
        /// </summary>
        //public bool IsExceptionReportGenerated
        //{
        //get { return ctrlReconOutput1.IsExceptionReportGenerated; }
        //set { ctrlReconOutput1.IsExceptionReportGenerated = value; }
        //}

        //bool isSavedReportToBeLoaded = true;
        /// <summary>
        /// BindTemplateCombo
        /// </summary>
        /// <param name="reconTypeKey"></param>
        // private void BindTemplateCombo(string reconTypeKey)
        // {
        //    try
        //    {
        //        //modified by amit on 21/04/2015
        //        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3454
        //        ReconUtilities.GetDataInTemplateCombo(reconTypeKey, cmbTemplate);
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
        // }

        #endregion

        #region Private Methods

        private void SetButtonsColor()
        {
            try
            {
                btnBrowse.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnBrowse.ForeColor = System.Drawing.Color.White;
                btnBrowse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnBrowse.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnBrowse.UseAppStyling = false;
                btnBrowse.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnPostTransaction.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnPostTransaction.ForeColor = System.Drawing.Color.White;
                btnPostTransaction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnPostTransaction.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnPostTransaction.UseAppStyling = false;
                btnPostTransaction.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRunRecon.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRunRecon.ForeColor = System.Drawing.Color.White;
                btnRunRecon.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRunRecon.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRunRecon.UseAppStyling = false;
                btnRunRecon.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// Set user based permission
        /// </summary>
        private void SetUserPermissions()
        {
            try
            {
                //_releaseType = CachedDataManager.GetInstance.GetPranaReleaseViewType();
                //if (_releaseType == PranaReleaseViewType.CHMiddleWare)
                //{
                //Prana.Admin.BLL.ModuleResources module = Prana.Admin.BLL.ModuleResources.ReconCancelAmend;
                //var hasWritePermForRecon = Prana.Admin.BLL.AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, AuthAction.Write);
                //if (!hasWritePermForRecon)
                //{
                //CHMW-1795	[Recon] - Centralize Permission code in FrmReconCancelAmend and all of its sub controls.
                AuthAction permissionLevel = CachedDataManagerRecon.GetInstance.GetPermissionLevel();
                if (permissionLevel == AuthAction.Write || permissionLevel == AuthAction.Approve)
                {
                    btnPostTransaction.Enabled = true;
                }
                else
                {
                    btnPostTransaction.Enabled = false;
                }
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

        private void BindReconTypeCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();// EnumHelper.ConvertEnumForBindingWithSelectValue(typeof(ReconType));

                int i = 0;
                foreach (string reconTemplate in ReconPrefManager.ReconPreferences.getRootTemplates())
                {
                    EnumerationValue value = new EnumerationValue(reconTemplate, reconTemplate);
                    listValues.Add(value);
                    i++;
                }
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));

                cmbReconType.DataSource = null;
                cmbReconType.DataSource = listValues;
                cmbReconType.DisplayMember = "DisplayText";
                cmbReconType.ValueMember = "Value";
                cmbReconType.DataBind();
                cmbReconType.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbReconType.DisplayLayout.Bands[0].Header.Enabled = false;
                cmbReconType.Value = -1;
                cmbReconType.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2288
                listValues = listValues.OrderBy(e => e.DisplayText).ToList();
                listValues.Insert(0, new EnumerationValue(ApplicationConstants.C_COMBO_SELECT, -1));

                cmbClient.DataSource = null;
                cmbClient.DataSource = listValues;
                cmbClient.DisplayMember = "DisplayText";
                cmbClient.ValueMember = "Value";
                cmbClient.DataBind();
                cmbClient.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbClient.Value = -1;
                cmbClient.DisplayLayout.Bands[0].ColHeadersVisible = false;
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1229					
                //set tool tip for every row               
                foreach (UltraGridRow row in cmbClient.Rows)
                {
                    if (row.Cells["DisplayText"].Text.Equals(ApplicationConstants.C_COMBO_SELECT, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }
                    row.ToolTipText = row.Cells["DisplayText"].Text;
                }
                if (string.IsNullOrEmpty(cmbClient.Text) || cmbClient.Text.Equals(ApplicationConstants.C_COMBO_SELECT, StringComparison.InvariantCultureIgnoreCase))
                {
                    ultraToolTipInfo1.ToolTipText = "No client selected";
                }
                //ultraToolTipManager1.DisplayStyle = ToolTipDisplayStyle.Standard;
                ultraToolTipManager1.SetUltraToolTip(cmbClient, ultraToolTipInfo1);
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

        private void btnRunRecon_Click(object sender, EventArgs e)
        {
            try
            {
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1436

                if (((DateTime)dtFromDate.Value).Date > ((DateTime)dtToDate.Value).Date)
                {
                    //modified by amit on 23.03.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-2937
                    MessageBox.Show("From Date can not be greater than To Date", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string rootDirectoryPath = ReconConstants.ReconDataDirectoryPath;

                //1. Checks if recon type is not changed from default
                //2. Checks if template type is not changed from default
                //3. check if template name is empty
                if ((!cmbReconType.Text.Equals(ApplicationConstants.C_COMBO_SELECT)) && (!cmbTemplate.Text.Equals(ApplicationConstants.C_COMBO_SELECT)) && !string.IsNullOrWhiteSpace(cmbTemplate.Text))
                {
                    //1. Checks if provided file is empty or not 
                    if (!string.IsNullOrEmpty(txtThirdPartyFile.Text))
                    {
                        if (File.Exists(txtThirdPartyFile.Text))
                        {
                            // Replaced the label with Status Bar, now status bar displays the status of recon.
                            ctrlReconOutput1.DisableAmendments(true, true);
                            ctrlReconOutput1.UpdateStatusBarText("Running...");
                            //ctrlReconOutput1.grdReconOutput.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                            if (!bgWorkerRecon.IsBusy)
                            {
                                _templateKey = cmbTemplate.Value.ToString();
                                if (_dictReconTemplates.ContainsKey(_templateKey))
                                {
                                    ReconParameters reconParameters = new ReconParameters();
                                    reconParameters.TemplateKey = _templateKey;
                                    reconParameters.DTFromDate = dtFromDate.DateTime;
                                    reconParameters.DTToDate = dtToDate.DateTime;
                                    reconParameters.FormatName = _dictReconTemplates[_templateKey].FormatName;
                                    reconParameters.RunDate = DateTime.Now.ToString(ApplicationConstants.DateFormat);
                                    reconParameters.ReconDateType = _dictReconTemplates[_templateKey].ReconDateType;
                                    reconParameters.IsReconReportToBeGenerated = _isActiveFromDashboard;
                                    reconParameters.PBFilePath = txtThirdPartyFile.Text;
                                    //CHMW-2225 Rows on recon dashboard should be unique by the following characteristics: Format Name, Recon Type, Run Date, From Date, End Date, Date Type (trade date, process date, etc)
                                    reconParameters.ReconFilePath = ReconUtilities.GetReconFilePath(rootDirectoryPath, reconParameters) + ".xml";
                                    ctrlReconOutput1.SetValues(reconParameters);
                                    ctrlReconOutput1.SetUserComment(string.Empty);
                                    bgWorkerRecon.RunWorkerAsync(reconParameters);
                                }
                                else
                                {
                                    MessageBox.Show(this.FindForm(), "Template either deleted or modified.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                }
                            }
                            else
                            {
                                MessageBox.Show(this.FindForm(), "Please Wait while previous batch is run", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                        }
                        else
                        {
                            MessageBox.Show("File.does not exist.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select Third party File", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Please select a template.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        void SetGridDataSourceEvent(object sender, EventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() => SetGridDataSourceEvent(sender, e)));
                }
                else if (!this.IsDisposed)
                {
                    if (sender != null && cmbTemplate != null && cmbTemplate.Value != null)
                    {
                        ListEventAargs args = e as ListEventAargs;
                        if (args == null || args.listOfValues.Count == 0 || string.IsNullOrEmpty(args.listOfValues[0]))
                        {
                            ctrlReconOutput1.SetGridDataSource(args, _templateKey, false);
                        }
                        else
                        {
                            MessageBox.Show(this, args.listOfValues[0], "Alert", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            ctrlReconOutput1.SetGridDataSource(args, _templateKey, false);
                        }
                        #region Update status bar text
                        if (_isActiveFromDashboard)
                        {
                            ctrlReconOutput1.UpdateStatusBarText("Data Saved");
                        }
                        else
                        {
                            ctrlReconOutput1.UpdateStatusBarText("Recon process completed, data will be updated on dashboard on clicking 'Save' button.");
                        }
                        #endregion
                        ReconManager.SetDataTableEvent -= SetGridDataSourceEvent;
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
        /// Manual Trading Ticket will be opened from this UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPostTransaction_Click(object sender, EventArgs e)
        {
            try
            {
                launchForm = ReconPrefManager.GetLaunchForm();
                if (launchForm != null)
                {
                    ListEventAargs args = new ListEventAargs();
                    args.listOfValues.Add(ApplicationConstants.CONST_MANUAL_TRADING_TICKET_UI.ToString());
                    launchForm(this, args);
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
        private void cmbClient_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbClient.Text.Equals(ApplicationConstants.C_COMBO_SELECT) || string.IsNullOrWhiteSpace(cmbClient.Text))
                {
                    cmbReconType.DataSource = null;
                    cmbTemplate.DataSource = null;
                }
                else
                {
                    //set recontype and template to -select- if client is -select-

                    cmbReconType.Value = -1;
                    cmbTemplate.DataSource = null;
                    BindReconTypeCombo();
                    ultraToolTipInfo1.ToolTipText = cmbClient.Text;
                    ultraToolTipManager1.SetUltraToolTip(cmbClient, ultraToolTipInfo1);
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
        /// if recontype is position than from date should be disabled otherwise it should be enabled.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbReconType_ValueChanged(object sender, EventArgs e)
        {
            try
            {

                if (cmbReconType.Text.Equals(ApplicationConstants.C_COMBO_SELECT) || string.IsNullOrWhiteSpace(cmbReconType.Text))
                {
                    cmbTemplate.DataSource = null;
                }
                if ((!cmbClient.Text.Equals(ApplicationConstants.C_COMBO_SELECT)) && (!cmbReconType.Text.Equals(ApplicationConstants.C_COMBO_SELECT)) && !string.IsNullOrWhiteSpace(cmbReconType.Text))
                {
                    string reconTypeKey = cmbClient.Value.ToString() + Seperators.SEPERATOR_6 + cmbReconType.Text;
                    //modified by amit on 21/04/2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3454
                    ReconUIUtilities.BindTemplateCombo(reconTypeKey, cmbTemplate);
                }
                if (cmbReconType.Text == ReconType.Position.ToString() || cmbReconType.Text == ReconType.TaxLot.ToString())
                {
                    dtFromDate.Enabled = false;
                }
                else
                {
                    dtFromDate.Enabled = true;
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
        /// TODO: Remove this method, as this method do not have any definition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTemplate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //if (!cmbTemplate.Text.Equals(ApplicationConstants.C_COMBO_SELECT) && !string.IsNullOrEmpty(cmbTemplate.Text))
                //{
                //    bgWorkerLoadPreSavedReport.RunWorkerAsync();
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



        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                string fileNameTobeImported = string.Empty;

                //browse desktop
                openFileDialog1.InitialDirectory = "\\\\tsclient\\C";
                openFileDialog1.Title = "Select File to Import";
                openFileDialog1.Filter = openFileDialog1.Filter = "Data Files (*.xls,*.csv)|*.xls;*.csv| All Files|*.*";
                openFileDialog1.FileName = String.Empty;
                DialogResult importResult = openFileDialog1.ShowDialog();
                if (importResult == DialogResult.OK)
                {
                    fileNameTobeImported = openFileDialog1.FileName;
                    txtThirdPartyFile.Text = fileNameTobeImported;
                }
                else if (importResult == DialogResult.Cancel)
                {
                    fileNameTobeImported = string.Empty;
                    MessageBox.Show("Operation cancelled by User.", "Exception Report");
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

        private void bgWorkerRecon_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //template key is passed to check for the editable columns for the datatable
                ReconManager.SetDataTableEvent += SetGridDataSourceEvent;
                ReconParameters reconParameters = (ReconParameters)e.Argument;

                ReconManager.ExecuteTask(reconParameters);
                //ImportManager.Instance.RunImportProcess(formatname, null);

                ctrlReconOutput1._closingStatus = ReconManager.Instance.GetDictionaryForClosing(reconParameters);

                //DataTable dt = ReconManager.RunReconciliationForSelectedTemplate(cmbTemplate.Value.ToString(), cmbReconType.Text, dtFromDate.DateTime, dtToDate.DateTime, txtThirdPartyFile.Text);
                // ctrlReconOutput1.SetGridDataSource(dt, templateKey);
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

        // Modified by Ankit Gupta on 03 Nov, 2014.
        // Moving the code to ctrlReconOutput, in method 'SetGridDataSource',therefore, this method is not required.

        //private void bgWorkerRecon_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    try
        //    {
        //        ctrlReconOutput1.lblReconStatus.Text = "Completed";                         
        //         ctrlReconOutput1.grdReconOutput.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
        //        Added a method, DisableControls(bool isDisabled), which controls the enabling and disabling of controls.

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


        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //modified by amit on 23.03.2015
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-2937
                if (cmbReconType.Text.Equals(ReconType.Position.ToString()) || cmbReconType.Text.Equals(ReconType.TaxLot.ToString()))
                {
                    dtFromDate.Value = dtToDate.Value;
                }
                //if (!cmbTemplate.Text.Equals(ApplicationConstants.C_COMBO_SELECT) && !string.IsNullOrEmpty(cmbTemplate.Text))
                //{
                //    bgWorkerLoadPreSavedReport.RunWorkerAsync();
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
        private void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //if (!cmbTemplate.Text.Equals(ApplicationConstants.C_COMBO_SELECT) && !string.IsNullOrEmpty(cmbTemplate.Text))
                //{
                //    bgWorkerLoadPreSavedReport.RunWorkerAsync();
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
        ///// <summary>
        ///// load pre saved report for the data selected on ad-hoc recon
        ///// </summary>     
        //private void bgWorkerLoadPreSavedReport_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        string rootDirectoryPath = ReconConstants.ReconDataDirectoryPath;
        //        string path = ReconUtilities.GetReconFilePath(rootDirectoryPath, cmbClient.Value.ToString(), cmbReconType.Text, cmbTemplate.Text, dtFromDate.DateTime.ToString("MM-dd-yyyy"), dtToDate.DateTime.ToString("MM-dd-yyyy"));
        //        path = path + ".xml";
        //        if (isSavedReportToBeLoaded && File.Exists(path))
        //        {
        //            DataSet ds = new DataSet();
        //            ds.ReadXml(path, XmlReadMode.InferTypedSchema);
        //            if (ds != null && ds.Tables.Count > 0)
        //            {
        //                ctrlReconOutput1.setValues(path, DateTime.Parse(dtFromDate.DateTime.ToString("MM-dd-yyyy")), DateTime.Parse(dtFromDate.DateTime.ToString("MM-dd-yyyy")));
        //                ctrlReconOutput1.SetGridDataSource(ds.Tables[0], cmbTemplate.Value.ToString(), false);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rthrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void txtThirdPartyFile_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ultraToolTipInfo1.ToolTipText = txtThirdPartyFile.Text;
                ultraToolTipManager1.SetUltraToolTip(txtThirdPartyFile, ultraToolTipInfo1);
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

        #endregion

        #region Internal Methods

        /// <summary>
        /// Update event of disabling other controls if amendments are made
        /// </summary>
        /// <param name="DisableApproveChanges"></param>
        internal void UpdateEvent(EventHandler DisableApproveChanges)
        {
            try
            {
                ctrlReconOutput1.UpdateEvent(DisableApproveChanges);
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
        /// sets data on the grid of ctrlreconoutput
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="reconType"></param>
        /// <param name="templateName"></param>
        /// <param name="toDate"></param>
        internal bool SetDataonControl(ReconParameters reconParameters, TaskResult taskResult)
        {
            try
            {
                //string templateKey = ReconUtilities.GetTemplateKeyFromParameters(clientID, reconType, templateName);
                if (_dictReconTemplates != null && _dictReconTemplates.ContainsKey(reconParameters.TemplateKey))
                {
                    _isActiveFromDashboard = true;
                    reconParameters.ReconFilePath = ReconUtilities.GetReconFilePath(ReconConstants.ReconDataDirectoryPath, reconParameters) + ".xml";

                    cmbClient.Value = reconParameters.ClientID;
                    cmbReconType.Value = reconParameters.ReconType;
                    cmbTemplate.Value = reconParameters.TemplateKey;
                    if (string.IsNullOrEmpty(reconParameters.PBFilePath))
                    {
                        txtThirdPartyFile.Text = "File path not available.";
                    }
                    else
                    {
                        txtThirdPartyFile.Text = reconParameters.PBFilePath;
                    }
                    dtFromDate.Value = reconParameters.DTFromDate;
                    dtToDate.Value = reconParameters.DTToDate;
                    if (cmbClient.Value != null && cmbReconType.Value != null && cmbTemplate.Value != null && File.Exists(reconParameters.ReconFilePath))
                    {
                        //cmbClient.Enabled = false;
                        //cmbReconType.Enabled = false;
                        //cmbTemplate.Enabled = false;
                        //btnBrowse.Enabled = false;
                        //txtThirdPartyFile.Enabled = false;
                        DataSet ds = new DataSet();
                        ds = XMLUtilities.ReadXmlUsingBufferedStream(reconParameters.ReconFilePath);

                        //string path = ReconUtilities.GetReconFilePath(ReconConstants.ReconDataDirectoryPath, clientID, reconType, templateName, fromDate, toDate);
                        //set the file path  for amendments
                        ctrlReconOutput1.SetValues(reconParameters);

                        //checks if table exist in dataset
                        if (ds.Tables.Count != 0)
                        {
                            ListEventAargs args = new ListEventAargs();
                            args.argsObject = ds.Tables[0];
                            args.argsObject2 = taskResult;
                            ctrlReconOutput1.SetGridDataSource(args, reconParameters.TemplateKey, _isActiveFromDashboard);
                        }
                        else
                        {
                            //ctrlReconOutput1.SetGridDataSource(null, null);
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            #region Catch
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
            #endregion
        }



        internal void DisableControls()
        {
            try
            {
                ctrlReconOutput1.DisableAmendments(true, true);
                ctrlReconOutput1.ultraStatusBarReconStatus.Text = "Data modified, please re-run recon to get updated data.";
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


        #endregion

        #region Constructors

        public ctrlAdHocRecon()
        {
            try
            {
                InitializeComponent();
                if (!CustomThemeHelper.IsDesignMode())
                {
                    SetUserPermissions();
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

        #endregion

        #region Public Variable

        Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("-Select-", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);
        SerializableDictionary<string, ReconTemplate> _dictReconTemplates;
        event EventHandler launchForm;
        string _templateKey;
        bool _isActiveFromDashboard = false;
        //public PranaReleaseViewType _releaseType { get; set; }

        #endregion

        #region Public Methods




        /// <summary>
        /// Initialize data sources
        /// </summary>
        public void InitializeDataSourcesOfCombo()
        {
            try
            {
                //SetUserPermissions();
                //ctrlReconOutput1.SetUserPermissions();
                //report not to be loaded on tab change
                //isSavedReportToBeLoaded = false;
                //if (isGridDisabled)
                //{
                //    ctrlReconOutput1.Disable();
                //}
                _dictReconTemplates = ReconPrefManager.ReconPreferences.DictReconTemplates;

                //Dictionary<int, string> dictClients = new Dictionary<int, string>();
                //Dictionary<int, string> dictReconTypes = new Dictionary<int, string>();
                //Dictionary<int, string> dictTemplates = new Dictionary<int, string>();
                //foreach (KeyValuePair<string, ReconTemplate> kvp in _dictReconTemplates)
                //{
                //    if (!dictClients.ContainsKey(kvp.Value.ClientID))
                //    {
                //        dictClients.Add(kvp.Value.ClientID, kvp.Value.ClientID.ToString());
                //    }
                //}

                //to restore the default value back to comboclien if not changed
                object clientID = cmbClient.Value;
                object reconTypeID = cmbReconType.Value;
                object templateID = cmbTemplate.Value;
                BindClientCombo();
                if (clientID != null)
                {
                    cmbClient.Value = clientID;
                    cmbReconType.Value = reconTypeID;
                    cmbTemplate.Value = templateID;
                }

                //isSavedReportToBeLoaded = true;

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


        #endregion


    }
}
