using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Enums;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.TaskManagement.Definition.Definition;
using Prana.Tools.BLL;
using Prana.Utilities.UI;
using Prana.Utilities.UI.ImportExportUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.Utilities.XMLUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.Tools
{
    public partial class ctrlReconOutput : UserControl, ILiveFeedCallback
    {

        /// <summary>
        ///  return the bool value weather recon report is save dor not
        /// </summary>
        //private bool _isExceptionReportGenerated = false;
        //public bool IsExceptionReportGenerated
        //{
        //get { return _isExceptionReportGenerated; }
        //set { _isExceptionReportGenerated = value; }
        //}
        private DataTable dtMarkPrice = new DataTable();
        EventHandler DisableApproveChanges;
        //bool _isReconHasGroupping = false;
        //bool _isAutoApproved = false;
        bool _isFetchingData = false;
        bool _isAskForRejectedAccounts = true;
        bool _isHeaderCheckBoxChecked = false;
        bool _isAmendmentsAllowed = false;
        static ReconOutputLayout _reconOutputLayout = null;
        static string _reconOutputLayoutFilePath = string.Empty;
        static string _reconOutputLayoutDirectoryPath = string.Empty;
        internal Dictionary<string, string> _closingStatus = new Dictionary<string, string>();
        TaskResult _taskResult = null;
        //List<int> _accountIDUnlocked = new List<int>();
        //StringBuilder _accountUnlocked = new StringBuilder();
        //bool _isActiveFromDashboard = false;
        ReconParameters _reconParameters = null;
        //public string _filePath;
        //public DateTime _startDate;
        //public DateTime _endDate;
        ReconTemplate _reconTemplate;
        UltraGrid grdReport = new UltraGrid();
        List<int> _alreadyPromptedAccountsForLock = new List<int>();
        //Dictionary<string, UltraGridGroup> _dictUltraGridGroup = new Dictionary<string, UltraGridGroup>();
        //Dictionary<string, string> _dictClosingStatus = new Dictionary<string, string>();

        Dictionary<string, NAVLockItem> _accountNavLockDetails = new Dictionary<string, NAVLockItem>();
        public static event EventHandler _launchForm;
        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        public static event EventHandler UpdateCommentsFromPostReconAmendments = delegate { };
        //List<string> _selectedColumn = new List<string>();
        Form _frmViewFile = null;
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlReconOutput()
        {
            try
            {
                InitializeComponent();
                if (!CustomThemeHelper.IsDesignMode())
                {
                    SetUserPermissions();

                    _accountNavLockDetails = Prana.ClientCommon.NAVLockManager.GetInstance.GetNavLockItemDetails();
                    UpdateCommentsFromPostReconAmendments += ctrlReconOutput_UpdateCommentsFromPostReconAmendments;
                    CreatePricingProxy();
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

        public ReconOutputLayout ReconOutputLayout
        {
            get
            {
                _reconOutputLayout = GetReconOutputLayout();
                return _reconOutputLayout;
            }
        }


        /// <summary>
        /// CHMW-1620 [Closing] - Add Comments field in PostReconAmendenmtsUI
        /// ctrlReconOutput_Update Comments From Post ReconAmendments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlReconOutput_UpdateCommentsFromPostReconAmendments(object sender, EventArgs e)
        {
            try
            {
                ListEventAargs args = e as ListEventAargs;

                if (args != null && args.listOfValues != null && args.listOfValues.Count > 0 && grdReconOutput != null && grdReconOutput.ActiveRow != null && grdReconOutput.ActiveRow.Cells != null && grdReconOutput.ActiveRow.Cells.Exists(ReconConstants.COLUMN_Comments))
                {
                    grdReconOutput.ActiveRow.Cells[ReconConstants.COLUMN_Comments].Value = args.listOfValues[0];
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
        /// Set user based permission
        /// 
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW 
        /// </summary>
        public void SetUserPermissions()
        {
            try
            {
                //Prana.Admin.BLL.ModuleResources module = Prana.Admin.BLL.ModuleResources.ReconCancelAmend;
                //var hasWritePermForRecon = Prana.Admin.BLL.AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, AuthAction.Write);
                if (CachedDataManagerRecon.GetInstance.GetPermissionLevel() != AuthAction.Approve &&
                    CachedDataManagerRecon.GetInstance.GetPermissionLevel() != AuthAction.Write)
                {
                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;
                    btnCopyAll.Enabled = false;
                    btnCopyFailedValues.Enabled = false;
                    grdReconOutput.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                }
                else
                {
                    btnSave.Enabled = true;
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1728
                    //btnDelete.Enabled = true;
                    //btnCopyAll.Enabled = true;
                    //btnCopyFailedValues.Enabled = true;
                    grdReconOutput.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                }
                btnSave.Enabled = true;
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

        public void SetValues(ReconParameters reconParameters)
        {
            try
            {
                //_filePath = reconParameters + ".xml";
                //_startDate = reconParameters.FromDate;
                //_endDate = reconParameters.ToDate;
                _reconParameters = reconParameters;
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
        /// include try catch logandshow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyFailedValues_Click(object sender, EventArgs e)
        {
            try
            {
                //false-> to eliminate if the row is match with tolerance
                copyMismatchRows(false);
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
        /// ch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            try
            {
                //true-> to also check if the row is match with tolerance
                copyMismatchRows(true);
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //only peform action when groupping is not applied CHMW-2158
                if (!IsGrouppingAppliedOnGrid())
                {
                    foreach (UltraGridRow row in grdReconOutput.Rows)
                    {
                        //checks if checkbox is selected for the selected row
                        if (row.Cells.Exists(ReconConstants.COLUMN_Checkbox))
                        {
                            bool value = (bool)(row.Cells[ReconConstants.COLUMN_Checkbox].Value);
                            // http://jira.nirvanasolutions.com:8080/browse/CHMW-1428
                            //changes done by amit for http://jira.nirvanasolutions.com:8080/browse/CHMW-3644
                            if (row.Hidden == false &&
                                value && row.Cells.Exists(ReconConstants.COLUMN_TaxLotStatus)
                                && row.Cells.Exists(ReconConstants.MismatchReason) && row.Cells[ReconConstants.MismatchReason].Value != null
                                && (!row.Cells[ReconConstants.MismatchReason].Value.ToString().Equals(ReconConstants.MismatchReason_DataMissingNirvana))
                                && row.Cells.Exists(ReconConstants.COLUMN_ClosingStatus) && row.Cells[ReconConstants.COLUMN_ClosingStatus].Value != null
                                && row.Cells[ReconConstants.COLUMN_ClosingStatus].Value.ToString().Equals(ClosingStatus.Open.ToString())
                                && row.Cells.Exists(ReconConstants.COLUMN_LockStatus) && row.Cells[ReconConstants.COLUMN_LockStatus].Value != null
                                && row.Cells[ReconConstants.COLUMN_LockStatus].Value.ToString().Equals(ReconConstants.LockStatus_UnLocked)
                                && row.Cells.Exists(ReconConstants.COLUMN_NAVLockStatus) && row.Cells[ReconConstants.COLUMN_NAVLockStatus].Value != null
                                && row.Cells[ReconConstants.COLUMN_NAVLockStatus].Value.ToString().Equals(ReconConstants.LockStatus_UnLocked))
                            {
                                //edits the value of TaxLotStatus column for the row
                                row.Cells[ReconConstants.COLUMN_TaxLotStatus].Value = AmendedTaxLotStatus.Deleted.ToString();

                                #region Revert amendments when deleted
                                if (row.Cells.Exists(ReconConstants.COLUMN_ChangedColumns)
                                    && row.Cells[ReconConstants.COLUMN_ChangedColumns].Value != null
                                    && !string.IsNullOrEmpty(row.Cells[ReconConstants.COLUMN_ChangedColumns].Value.ToString()))
                                {
                                    List<string> listColumns = new List<string>();
                                    listColumns = row.Cells[ReconConstants.COLUMN_ChangedColumns].Value.ToString().Split(Seperators.SEPERATOR_8[0]).ToList();
                                    foreach (string column in listColumns)
                                    {
                                        if (row.Cells.Exists(ReconConstants.CONST_Nirvana + column) && row.Cells.Exists(ReconConstants.CONST_OriginalValue + column))
                                        {
                                            row.Cells[ReconConstants.CONST_Nirvana + column].Value = row.Cells[ReconConstants.CONST_OriginalValue + column].Value;
                                        }
                                    }
                                }
                                #endregion

                                //Disable Row editing which are Deleted
                                row.Activation = Activation.NoEdit;
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



        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //only perform action when grouping is not applied CHMW-2158
                if (!IsGrouppingAppliedOnGrid())
                {
                    if (_reconParameters != null)
                    {
                        if (!string.IsNullOrEmpty(_reconParameters.ReconFilePath))
                        {
                            if (grdReconOutput.DataSource != null)
                            {
                                DataTable dt = GetUpdatedDataSourceFromUltraGrid();
                                List<object> runWorkerAsyncArguments = new List<object>();
                                runWorkerAsyncArguments.Add(dt);
                                runWorkerAsyncArguments.Add(txtUserComments.Text);
                                if (dt.Columns.Count != 0)
                                {
                                    //modified by amit on 18.03.2015
                                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-2918
                                    if (bgSaveClickWorker.IsBusy)
                                    {
                                        MessageBox.Show(this, "Please wait while previous amendments are saved", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    else
                                    {
                                        bgSaveClickWorker.RunWorkerAsync(runWorkerAsyncArguments);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(this, "No data to save.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "No data to save.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "No data to save.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// copy values broker to nirvana data in grid when checked under Mismatch
        /// </summary>
        private void copyMismatchRows(bool checkfortolerance)
        {
            try
            {
                //only peform action when groupping is not applied CHMW-2158
                if (!IsGrouppingAppliedOnGrid())
                {
                    _isAskForRejectedAccounts = false;
                    _alreadyPromptedAccountsForLock.Clear();
                    // _isHeaderCheckBoxChecked = true;
                    StringBuilder errmsg = new StringBuilder();
                    StringBuilder copyFailedMessage = new StringBuilder();
                    copyFailedMessage.Append("Copy for following rows failed: Cannot copy negative value").AppendLine();

                    bool isCopyFailed = false;
                    List<MatchingRule> listOfRules = null;
                    if (_reconTemplate != null)
                    {
                        listOfRules = ReconPrefManager.ReconPreferences.GetListOfRules(_reconTemplate.TemplateKey);
                    }
                    foreach (UltraGridColumn column in grdReconOutput.DisplayLayout.Bands[0].Columns)
                    {

                        //loop to check each column with prefix as ReconStatus
                        if (column.Key.Contains(ReconConstants.CONST_ReconStatus))
                        {
                            string columnName = column.Key.Substring(ReconConstants.CONST_ReconStatus.Length);
                            #region if column does not exist
                            //CHMW-3659	[Recon] [Position Recon] Alert comes for Broker Quantity not exist and alert comes for market value -ve on copy failed values / copy all for position recon
                            if (_reconTemplate.EditableColumns.Contains(columnName))
                            {
                                if (!grdReconOutput.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.CONST_Nirvana + columnName) && !errmsg.ToString().Contains(ReconConstants.CONST_Nirvana + columnName))
                                {
                                    errmsg.Append(", " + ReconConstants.CONST_Nirvana + columnName);
                                    if (!grdReconOutput.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.CONST_Broker + columnName) && !errmsg.ToString().Contains(ReconConstants.CONST_Broker + columnName))
                                    {
                                        errmsg.Append(", " + ReconConstants.CONST_Broker + columnName);
                                    }
                                    continue;
                                }
                                //modified by amit on 23/04/2014
                                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3477
                                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3521
                                #region if column is not numeric
                                bool isNumeric = false;
                                if (listOfRules != null)
                                {
                                    if (listOfRules[0].NumericFields.Contains(column.Key))
                                        isNumeric = true;
                                }
                                #endregion
                                if (isNumeric && !grdReconOutput.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.CONST_Diff + columnName) && !errmsg.ToString().Contains(ReconConstants.CONST_Diff + columnName))
                                {
                                    errmsg.Append(", " + ReconConstants.CONST_Diff + columnName);
                                    continue;
                                }
                                if (!grdReconOutput.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.CONST_Broker + columnName) && !errmsg.ToString().Contains(ReconConstants.CONST_Broker + columnName))
                                {
                                    errmsg.Append(", " + ReconConstants.CONST_Broker + columnName);
                                    continue;
                                }
                            }
                            #endregion
                            foreach (UltraGridRow row in grdReconOutput.Rows)
                            {
                                if (row.Cells.Exists(ReconConstants.COLUMN_LockStatus)
                                    && row.Cells[ReconConstants.COLUMN_LockStatus].Value != null
                                    && row.Cells[ReconConstants.COLUMN_LockStatus].Value != DBNull.Value)
                                {
                                    //grdReconOutput.DisplayLayout.GroupByBox
                                    if (row.Cells.Exists(ReconConstants.COLUMN_NAVLockStatus)
                                     && row.Cells[ReconConstants.COLUMN_NAVLockStatus].Value != null
                                         && row.Cells[ReconConstants.COLUMN_LockStatus].Value != DBNull.Value)
                                    {
                                        string value = (string)(row.Cells[ReconConstants.COLUMN_LockStatus].Value);
                                        string valueNavLock = (string)(row.Cells[ReconConstants.COLUMN_NAVLockStatus].Value);
                                        string mismatchType = (string)(row.Cells[column].Value);
                                        //0. Check if row is hidden
                                        //1. Checks if the row is Unlocked
                                        //2. checks if the row on column ReconStatus contain Mismatch enum
                                        //3. checks if the method call is from btcopyall click.
                                        //4. check if the rows matched with tolerance are to be added.    
                                        //5. check if the tax lot status column exist
                                        //6. check if the column is deleted or not
                                        // Modified by Ankit Gupta on 16 Sep 2014.
                                        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1465
                                        // Disable copying of values if NAV is locked.
                                        if (row.Hidden == false
                                                    && valueNavLock == ReconConstants.LockStatus_UnLocked
                                                    && value == ReconConstants.LockStatus_UnLocked
                                                    && (mismatchType.Contains(ReconStatus.MisMatch.ToString())
                                                    || (checkfortolerance && mismatchType.Contains(ReconStatus.MatchedWithInTolerance.ToString())))
                                                    && (!row.Cells.Exists(ReconConstants.COLUMN_TaxLotStatus)
                                                    || (row.Cells[ReconConstants.COLUMN_TaxLotStatus].Value == null
                                                    || row.Cells[ReconConstants.COLUMN_TaxLotStatus].Value.ToString() != AmendedTaxLotStatus.Deleted.ToString())))
                                        {
                                            //retrieves the column name to be replaced
                                            //CHMW-3659	[Recon] [Position Recon] Alert comes for Broker Quantity not exist and alert comes for market value -ve on copy failed values / copy all for position recon
                                            if (row.Cells.Exists(ReconConstants.CONST_Broker + columnName) && row.Cells.Exists(ReconConstants.CONST_Nirvana + columnName))
                                            {
                                                double valueToCopy;
                                                bool isNumericColumn = false;
                                                DateTime dateToCopy = DateTime.MinValue;
                                                bool isDateColumn = false;
                                                if (double.TryParse(row.Cells[ReconConstants.CONST_Broker + columnName].Value.ToString(), out valueToCopy))
                                                {
                                                    if (valueToCopy < 0)
                                                    {
                                                        isCopyFailed = true;
                                                        copyFailedMessage.Append("\n TaxLotID: " + row.Cells[ReconConstants.COLUMN_TaxLotID].Text + " Column : " + ReconConstants.CONST_Broker + columnName + " value: " + row.Cells[ReconConstants.CONST_Broker + columnName].Text).AppendLine();
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        isNumericColumn = true;
                                                    }
                                                }
                                                else if (DateTime.TryParse(row.Cells[ReconConstants.CONST_Broker + columnName].Value.ToString(), out dateToCopy))
                                                {
                                                    isDateColumn = true;
                                                }
                                                //copies the value from Broker column to nirvana column
                                                if (_reconTemplate.EditableColumns.Contains(columnName))
                                                {
                                                    if (isDateColumn)
                                                    {
                                                        row.Cells[ReconConstants.CONST_Nirvana + columnName].Value = dateToCopy.ToString(ApplicationConstants.DateFormat);
                                                    }
                                                    else if (isNumericColumn)
                                                    {
                                                        row.Cells[ReconConstants.CONST_Nirvana + columnName].Value = valueToCopy.ToString();
                                                    }
                                                    else
                                                    {
                                                        row.Cells[ReconConstants.CONST_Nirvana + columnName].Value = row.Cells[ReconConstants.CONST_Broker + columnName].Value.ToString();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }

                        }
                    }
                    if (errmsg.Length > 0)
                    {
                        MessageBox.Show(errmsg.ToString().Substring(2) + " Columns do not exist", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    if (isCopyFailed)
                    {
                        MessageBox.Show(copyFailedMessage.ToString(), "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    //display account lock error message if any
                    //grdReconOutput_AfterHeaderCheckStateChanged(null, null);
                    _isAskForRejectedAccounts = true;
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
        /// returns bool on groupping applied or not
        /// </summary>
        /// <returns></returns>
        private bool IsGrouppingAppliedOnGrid()
        {
            try
            {
                if (UltraWinGridUtils.IsGrouppingAppliedOnGrid(grdReconOutput))
                {
                    MessageBox.Show("Cannot perform this action when grouping is applied on grid.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
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
            return false;
        }


        private DataTable GetUpdatedDataSourceFromUltraGrid()
        {
            DataTable dt = new DataTable("Comparision");
            try
            {
                DataRow dtRow;
                foreach (UltraGridRow row in grdReconOutput.Rows)
                {
                    //UpdateChangedColumns(row, grpName);
                    dtRow = dt.NewRow();
                    foreach (UltraGridColumn column in grdReconOutput.DisplayLayout.Bands[0].Columns)
                    {
                        //checkbox columns is not to be saved
                        if (!dt.Columns.Contains(column.Key) && column.Key != ReconConstants.COLUMN_Checkbox)
                        {
                            //modified by amit 02.04.2015
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-3249
                            //List<MatchingRule> listOfRules = ReconPrefManager.ReconPreferences.GetListOfRules(_reconTemplate.TemplateKey);
                            //List<String> listOfNumericFields = ReconUtilities.GetListOfNumericFields(listOfRules[0]);

                            //if (listOfNumericFields.Contains(column.Key))
                            //    dt.Columns.Add(column.Key, typeof(double));
                            //else
                            dt.Columns.Add(column.Key);

                        }
                        else if (column.Key == ReconConstants.COLUMN_Checkbox)
                        {
                            continue;
                        }
                        //modified by amit. changes done for http://jira.nirvanasolutions.com:8080/browse/CHMW-3576
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3638
                        //if (column.Key.Equals(ReconConstants.CONST_Nirvana + OrderFields.PROPERTY_SETTLEMENTCURRENCY) || column.Key.Equals(ReconConstants.CONST_OriginalValue + OrderFields.PROPERTY_SETTLEMENTCURRENCY))
                        //{
                        //    if (Int32.Parse(row.Cells[column.Key].Value.ToString()) == 0)
                        //        dtRow[column.Key] = ApplicationConstants.C_COMBO_NONE;
                        //    else
                        //        dtRow[column.Key] = CachedDataManager.GetInstance.GetAllCurrencies()[Int32.Parse(row.Cells[column.Key].Value.ToString())];
                        //}
                        //else
                        //{
                        dtRow[column.Key] = row.Cells[column.Key].Value;
                        //}
                    }
                    dt.Rows.Add(dtRow);
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
            return dt;
        }

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="row"></param>
        /// <param name="colName"></param>
        private void UpdateChangedColumns(UltraGridRow row, string colName)
        {
            try
            {
                if (grdReconOutput.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.CONST_Nirvana + colName) && grdReconOutput.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.CONST_OriginalValue + colName))
                {
                    string NirvanaValue = row.Cells[ReconConstants.CONST_Nirvana + colName].Value.ToString();
                    string OriginalValue = row.Cells[ReconConstants.CONST_OriginalValue + colName].Value.ToString();

                    if (grdReconOutput.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.COLUMN_ChangedColumns))
                    {

                        if (!NirvanaValue.Equals(OriginalValue))
                        {

                            if (row.Cells[ReconConstants.COLUMN_ChangedColumns].Value == null)
                                row.Cells[ReconConstants.COLUMN_ChangedColumns].Value = colName;
                            else
                            {
                                List<string> colChanged = new List<string>(row.Cells[ReconConstants.COLUMN_ChangedColumns].Value.ToString().Split(Convert.ToChar(Seperators.SEPERATOR_8)));

                                if (!colChanged.Contains(colName))
                                {
                                    row.Cells[ReconConstants.COLUMN_ChangedColumns].Value = row.Cells[ReconConstants.COLUMN_ChangedColumns].Value.ToString() + Seperators.SEPERATOR_8 + colName;
                                }

                            }
                        }
                        else
                        {
                            if (row.Cells[ReconConstants.COLUMN_ChangedColumns].Value != null)
                            {
                                List<string> colChanged = new List<string>(row.Cells[ReconConstants.COLUMN_ChangedColumns].Value.ToString().Split(Convert.ToChar(Seperators.SEPERATOR_8)));

                                if (colChanged.Contains(colName))
                                {
                                    colChanged.Remove(colName);
                                    row.Cells[ReconConstants.COLUMN_ChangedColumns].Value = string.Join(",", colChanged.ToArray());
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
        }

        // Modifeid by Ankit Gupta on 03 Sep 2014.
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1353
        internal void SetGridDataSource(ListEventAargs args, string templateKey, bool isActiveFromDashboard)
        {
            try
            {
                this.grdReconOutput.BeginUpdate();
                this.grdReconOutput.SuspendRowSynchronization();
                if (args != null)
                {
                    DataTable dt = (DataTable)args.argsObject;
                    if (args.argsObject2 != null)
                    {
                        _taskResult = (TaskResult)args.argsObject2;
                        if (_taskResult.TaskStatistics.TaskSpecificData.AsDictionary.ContainsKey("Comments"))
                        {
                            SetUserComment(_taskResult.TaskStatistics.TaskSpecificData.AsDictionary["Comments"].ToString());
                        }
                    }
                    _reconTemplate = ReconPrefManager.ReconPreferences.GetTemplates(templateKey);
                    if (_reconTemplate != null)
                    {
                        //set properties                    
                        _reconParameters.TemplateKey = _reconTemplate.TemplateKey;
                        #region Check if grouping applied and amendments allowed or not
                        if ((_reconTemplate.IsReconTemplateGroupping() && !isActiveFromDashboard) ||
                            ((_reconTemplate.ReconType == ReconType.Transaction ||
                                _reconTemplate.ReconType == ReconType.Position ||
                                 _reconTemplate.ReconType == ReconType.TaxLot) && !dt.Columns.Contains(ReconConstants.COLUMN_TaxLotID)))
                        {
                            //_isReconHasGroupping = true;
                            _isAmendmentsAllowed = false;
                        }
                        else
                        {
                            //_isReconHasGroupping = false;
                            if (_reconTemplate.ReconType == ReconType.Transaction || _reconTemplate.ReconType == ReconType.Position || _reconTemplate.ReconType == ReconType.TaxLot)
                            {
                                if (dt.Columns.Contains(ReconConstants.COLUMN_AccountName))
                                {
                                    _isAmendmentsAllowed = true;
                                }
                            }
                            else
                            {
                                _isAmendmentsAllowed = false;
                            }
                        }

                        //_selectedColumn = ReconUtilities.GetSelectedColumnsList(_reconTemplate.SelectedColumnList);
                        //if recon(Transaction) has grouping than taxlotID column will be removed 
                        //which can be used from dashboard as well to determine if recon is groupped
                        //as if groupping was dere at time of running is not saved.

                        #endregion

                        _isFetchingData = true;
                        //_isActiveFromDashboard = isActiveFromDashboard;
                        //Added a method, DisableControls(bool isDisabled), which controls the enabling and disabling of controls. 
                        //to clear the previous recon data
                        grdReconOutput.DisplayLayout.Bands[0].Reset();
                        //_dictUltraGridGroup.Clear();
                        //FilltaxLotClosingStatus(dt);
                        if (_reconTemplate.ReconType == ReconType.Transaction || _reconTemplate.ReconType == ReconType.Position || _reconTemplate.ReconType == ReconType.TaxLot)
                        {
                            UpdateDataTable(dt);
                        }
                        if (!dt.Columns.Contains(ReconConstants.COLUMN_UserLoggedIN))
                        {
                            dt.Columns.Add(ReconConstants.COLUMN_UserLoggedIN);
                            dt.Columns[ReconConstants.COLUMN_UserLoggedIN].DataType = typeof(System.String);
                        }
                        dt.Columns[ReconConstants.COLUMN_UserLoggedIN].Expression = string.Format("'{0}'", CachedDataManager.GetInstance.LoggedInUser.FirstName + " " + CachedDataManager.GetInstance.LoggedInUser.LastName);
                        grdReconOutput.DataSource = null;
                        BindingSource bS = new BindingSource();
                        bS.DataSource = dt;
                        dt.Dispose();
                        grdReconOutput.DataSource = bS;
                        grdReconOutput.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;
                        grdReconOutput.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                        if (_isAmendmentsAllowed)
                        {
                            DisableAmendments(false, false);
                        }
                        else
                        {
                            if (_reconTemplate.ReconType == ReconType.Position)
                            {
                                DisableAmendments(false, false);
                            }
                            else
                            {
                                DisableAmendments(true, false);
                            }
                            if (_reconTemplate.ReconType == ReconType.Transaction || _reconTemplate.ReconType == ReconType.Position || _reconTemplate.ReconType == ReconType.TaxLot)
                            {
                                ultraStatusBarReconStatus.Text = "Amendments cannot be made on groupped data.";
                            }
                        }
                        ////Binding ValueList of Closed or partially Closed Taxlots to 
                        //if (_reconTemplate.ReconType == ReconType.Position || _reconTemplate.ReconType == ReconType.Transaction)
                        //{
                        //    grdReconOutput.DisplayLayout.Bands[0].Columns[ReconConstants.COLUMN_Checkbox].SetHeaderCheckedState(grdReconOutput.Rows, CheckState.Unchecked);
                        //}
                        //if (!isActiveFromDashboard)
                        //{
                        //    ultraStatusBarReconStatus.Text = "Recon process completed, data will be updated on dashboard on clicking 'Save' button.";
                        //}
                        //grdReconOutput.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;
                        //grdReconOutput.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                        //if (_isAmendmentsAllowed)
                        //{
                        //    DisableAmendments(false, false);
                        //}
                        if (isActiveFromDashboard)
                        {
                            ultraStatusBarReconStatus.Text = "Recon process completed, data will be updated on dashboard on clicking 'Save' button.";
                        }
                        _isFetchingData = false;
                    }
                    else
                    {
                        MessageBox.Show("Recon template unavailable", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
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
                this.grdReconOutput.ResumeRowSynchronization();
                this.grdReconOutput.EndUpdate();
            }
        }
        /// <summary>
        /// Updates ClosingStatus in the DataSet on the basis of closing Status Dictionary filled from Server
        /// </summary>
        /// <param name="dt"></param>
        private void UpdateDataTable(DataTable dt)
        {
            try
            {
                if (!dt.Columns.Contains(ReconConstants.COLUMN_ClosingStatus))
                {
                    dt.Columns.Add(ReconConstants.COLUMN_ClosingStatus);
                }
                if (dt.Columns.Contains(ReconConstants.COLUMN_ClosingStatus) && dt.Columns.Contains(ReconConstants.COLUMN_TaxLotID))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(row[ReconConstants.COLUMN_TaxLotID].ToString()))
                        {
                            if (!_closingStatus.ContainsKey(row[ReconConstants.COLUMN_TaxLotID].ToString()))
                            {
                                double taxLotId = Double.MinValue;
                                Double.TryParse(row[ReconConstants.COLUMN_TaxLotID].ToString(), out taxLotId);
                                if (taxLotId != Double.MinValue)
                                {
                                    row[ReconConstants.COLUMN_ClosingStatus] = ClosingStatus.Open.ToString();
                                }
                            }
                            else
                            {
                                row[ReconConstants.COLUMN_ClosingStatus] = _closingStatus[row[ReconConstants.COLUMN_TaxLotID].ToString()].ToString();
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
        }
        /// <summary>
        /// Fill dictionary of taxlots in the datatable
        /// </summary>
        /// <param name="dt"></param>
        //private void FilltaxLotClosingStatus(DataTable dt)
        //{
        //    try
        //    {
        //        if (dt.Columns.Contains(ReconConstants.COLUMN_TaxLotID))
        //        {
        //            //  List<string> listTaxLotID = dt.AsEnumerable().Select(dr => dr.Field<string>(ReconConstants.COLUMN_TaxLotID)).Where(dr =>  != string.Empty).ToList();
        //            List<string> listTaxLotID = (from table in dt.AsEnumerable()
        //                                         where !string.IsNullOrEmpty(table.Field<string>(ReconConstants.COLUMN_TaxLotID))
        //                                         select table.Field<string>(ReconConstants.COLUMN_TaxLotID)).ToList();
        //            //CHMW-3173 [Recon] - Antlr.Runtime.NoViableAltException on server when transaction recon is run and there is not data for that recon
        //            if (listTaxLotID.Count > 0)
        //            {
        //                _dictClosingStatus = new Dictionary<string, string>();
        //                List<AllocationGroup> listAllocationGroup = AllocationManager.GetInstance().GetGroups(listTaxLotID, true);
        //                foreach (AllocationGroup grp in listAllocationGroup)
        //                {
        //                    foreach (TaxLot taxLot in grp.TaxLots)
        //                    {
        //                        if (listTaxLotID.Contains(taxLot.TaxLotID))
        //                        {
        //                            _dictClosingStatus.Add(taxLot.TaxLotID, taxLot.ClosingStatus.ToString());
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
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdReconOutput_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraWinGridUtils.EnableFixedFilterRow(e);
                //Set the HeaderCheckBoxVisibility so it will display the CheckBox whenever a CheckEditor is used within the UltraGridColumn 
                e.Layout.Override.HeaderCheckBoxVisibility = HeaderCheckBoxVisibility.WhenUsingCheckEditor;
                //Set the HeaderCheckBoxAlignment so the CheckBox will appear to the Right of the caption. 
                e.Layout.Override.HeaderCheckBoxAlignment = HeaderCheckBoxAlignment.Right;
                //Set the HeaderCheckBoxSynchronization so all rows within the GridBand will be synchronized with the CheckBox 
                e.Layout.Override.HeaderCheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                // Set the RowSelectorHeaderStyle to ColumnChooserButton.
                e.Layout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
                // Enable the RowSelectors. This is necessary because the column chooser
                // button is displayed over the row selectors in the column headers area.
                e.Layout.Override.RowSelectors = DefaultableBoolean.True;
                UltraGridBand band = e.Layout.Bands[0];
                AddColumnsInGrid(band);


                //end
                bool isColumnsNeedToBeAssignedInGroups = false;
                Dictionary<string, UltraGridGroup> dictUltraGridGroup = CreateUltraGridGroups(band);
                if (ReconOutputLayout.reconOutputGridColumns.Count > 0)
                {
                    List<ColumnData> listReconOutputColData = ReconOutputLayout.reconOutputGridColumns;
                    isColumnsNeedToBeAssignedInGroups = SetGridColumnLayout(grdReconOutput, listReconOutputColData);
                }
                else
                {
                    SetColumnsForUltraGrid(band);
                    isColumnsNeedToBeAssignedInGroups = true;

                }
                if (isColumnsNeedToBeAssignedInGroups)
                {
                    AssignColumnToRespectiveGroup(band, dictUltraGridGroup);
                }

                SetEditableColumns(band);
                //colors alternate rows
                //e.Layout.Override.RowAlternateAppearance.BackColor = Color.Gray;
                //e.Layout.Override.RowAlternateAppearance.BackColor2 = Color.DarkGray;
                //e.Layout.Override.RowAlternateAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
                SetColumnsCaptionForUltraGrid(grdReconOutput.DisplayLayout.Bands[0]);
                //Here btnSave_Click is called to save grid data in xml file
                //btnSave_Click(null, null);
                //set the header checkBox o unchecked state initially
                if (grdReconOutput.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.COLUMN_Checkbox))
                {
                    grdReconOutput.DisplayLayout.Bands[0].Columns[ReconConstants.COLUMN_Checkbox].SetHeaderCheckedState(grdReconOutput.Rows, CheckState.Unchecked);
                }
                //grdReconOutput.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;
                //grdReconOutput.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;

                foreach (UltraGridGroup group in band.Groups)
                {
                    if (group.Columns.Where(x => x.IsVisibleInLayout == true).Count() == 0)
                    {
                        group.Hidden = true;
                    }
                }
                //Added by sachin mishra Purpose-For setting comma separated values on grid and export files.
                ReconUIUtilities.CommaSeparatedMethod(e, _reconTemplate);

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

        private void SetEditableColumns(UltraGridBand band)
        {
            try
            {
                foreach (UltraGridColumn column in band.Columns)
                {
                    string columnName = column.Key;
                    //to allow edit nirvana columns (columns which contains caption nirvana)
                    if (columnName.Contains(ReconConstants.CONST_Nirvana))
                    {
                        columnName = columnName.Replace(ReconConstants.CONST_Nirvana, string.Empty).Trim();
                    }
                    column.CellActivation = Activation.NoEdit;
                    //make all columns not editable, columns which should be editable are configured below
                    if (_reconTemplate.EditableColumns.Contains(columnName))
                    {
                        if (_isAmendmentsAllowed)
                        {
                            column.CellActivation = Activation.AllowEdit;
                        }
                    }
                    //}
                    //else
                    //{
                    //    column.CellActivation = Activation.NoEdit;
                    //}
                }
                if (band.Columns.Exists(ReconConstants.COLUMN_Checkbox))
                {
                    band.Columns[ReconConstants.COLUMN_Checkbox].CellActivation = Activation.AllowEdit;
                }
                if (band.Columns.Exists(ReconConstants.COLUMN_Comments))
                {
                    band.Columns[ReconConstants.COLUMN_Comments].CellActivation = Activation.AllowEdit;
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

        private void AddColumnsInGrid(UltraGridBand band)
        {
            #region column checkbox
            if (_isAmendmentsAllowed)
            {
                //add checkbox column if not exists
                if (!band.Columns.Exists(ReconConstants.COLUMN_Checkbox))
                {
                    band.Columns.Add(ReconConstants.COLUMN_Checkbox, string.Empty);
                }
                band.Columns[ReconConstants.COLUMN_Checkbox].DataType = typeof(bool);
                band.Columns[ReconConstants.COLUMN_Checkbox].Header.VisiblePosition = 0;
                band.Columns[ReconConstants.COLUMN_Checkbox].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                band.Columns[ReconConstants.COLUMN_Checkbox].Header.Caption = string.Empty;
                band.Columns[ReconConstants.COLUMN_Checkbox].Width = 20;

                if (!band.Columns.Exists(ReconConstants.COLUMN_LockStatus))
                {
                    band.Columns.Add(ReconConstants.COLUMN_LockStatus, ReconConstants.CAPTION_LockStatus);
                }
                band.Columns[ReconConstants.COLUMN_LockStatus].Header.VisiblePosition = 1;
                band.Columns[ReconConstants.COLUMN_LockStatus].Width = 20;

                band.Columns[ReconConstants.COLUMN_LockStatus].CellAppearance.Image = Properties.Resources.unlock;
                band.Columns[ReconConstants.COLUMN_LockStatus].CellAppearance.Tag = ReconConstants.LockStatus_UnLocked;
            }
            #endregion
            if (!band.Columns.Exists(ReconConstants.COLUMN_NAVLockStatus) && band.Columns.Exists(ReconConstants.COLUMN_AccountName))
            {
                band.Columns.Add(ReconConstants.COLUMN_NAVLockStatus, ReconConstants.CAPTION_NAVLockStatus);
            }
            //if (!band.Columns.Exists("AccountID"))
            //{
            //    band.Columns.Add("AccountID");
            //}
            //band.Columns["AccountID"].Hidden = true;
            //band.Columns["AccountID"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
            //if (!band.Columns.Exists(ReconConstants.COLUMN_ClosingStatus) && band.Columns.Exists(ReconConstants.COLUMN_TaxLotID) && band.Columns.Exists(ReconConstants.COLUMN_AccountName))
            //{
            //    band.Columns.Add(ReconConstants.COLUMN_ClosingStatus, ReconConstants.CAPTION_ClosingStatus);
            //}
            //add Tax Lot Status Column if does not exist
            if (!band.Columns.Exists(ReconConstants.COLUMN_TaxLotStatus) && band.Columns.Exists(ReconConstants.COLUMN_TaxLotID) && !band.Columns[ReconConstants.COLUMN_TaxLotID].Hidden)
            {
                band.Columns.Add(ReconConstants.COLUMN_TaxLotStatus, ReconConstants.CAPTION_TaxLotStatus);
            }
            //column will contain the user who is currently logged in from cache
            if (!band.Columns.Exists(ReconConstants.COLUMN_UserLoggedIN))
            {
                band.Columns.Add(ReconConstants.COLUMN_UserLoggedIN, ReconConstants.COLUMN_UserLoggedIN);
            }
            if (!band.Columns.Exists(ReconConstants.COLUMN_ChangedColumns))
            {
                band.Columns.Add(ReconConstants.COLUMN_ChangedColumns, ReconConstants.COLUMN_ChangedColumns);
            }
            if (_reconTemplate.ReconType == ReconType.Position || _reconTemplate.ReconType == ReconType.TaxLot)
            {
                if (!band.Columns.Exists("Amend"))
                {
                    band.Columns.Add("Amend", "Amend");
                }
                SetColumnButtonType(band);//added by amit 02.03.2015 CHMW-2894
            }

            if (band.Columns.Exists(ReconConstants.CONST_Nirvana + OrderFields.PROPERTY_SETTLEMENTCURRENCY))
            {

                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                ValueList currencies = new ValueList();
                //PRANA-7935 Settlement Currency defaulting inappropriately on the Trading Ticket
                //currencies.ValueListItems.Add(0, ApplicationConstants.C_COMBO_NONE);
                foreach (KeyValuePair<int, string> item in dictCurrencies)
                {
                    currencies.ValueListItems.Add(item.Value, item.Value);
                }
                UltraGridColumn colSettlCurrency = band.Columns[ReconConstants.CONST_Nirvana + OrderFields.PROPERTY_SETTLEMENTCURRENCY];
                colSettlCurrency.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colSettlCurrency.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
                colSettlCurrency.ValueList = currencies;
            }
            //added by amit 02.03.2015 CHMW-2892
            if (band.Columns.Exists(ReconConstants.CONST_Broker + ReconConstants.COLUMN_RowIndex))
            {
                band.Columns[ReconConstants.CONST_Broker + ReconConstants.COLUMN_RowIndex].Hidden = true;
            }
            if (!ReconUtilities.GetSelectedColumnsList(_reconTemplate.SelectedColumnList).Contains(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_TradeDate)
                && band.Columns.Exists(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_TradeDate))
            {
                band.Columns[ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_TradeDate].Hidden = true;
            }
        }
        /// <summary>
        /// Sets Columns Caption For UltraGrid
        /// </summary>
        /// <param name="band"></param>
        private void SetColumnsCaptionForUltraGrid(UltraGridBand band)
        {
            try
            {
                foreach (UltraGridColumn col in band.Columns)
                {
                    //sets space in column caption whose coulmn name contains Nirvana,Broker,Diff,OriginalValue,ReconStatus,ToleranceType,ToleranceValue
                    string columnKey = col.Key;
                    String caption = columnKey;
                    if (columnKey.Contains(ReconConstants.CONST_Nirvana))
                    {
                        caption = ReconConstants.CONST_Nirvana + " " + columnKey.Substring(ReconConstants.CONST_Nirvana.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_OriginalValue))
                    {
                        caption = ReconConstants.CONST_OriginalValue + " " + columnKey.Substring(ReconConstants.CONST_OriginalValue.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_Broker))
                    {
                        caption = ReconConstants.CONST_Broker + " " + columnKey.Substring(ReconConstants.CONST_Broker.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_ReconStatus))
                    {
                        caption = ReconConstants.CONST_ReconStatus + " " + columnKey.Substring(ReconConstants.CONST_ReconStatus.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_ToleranceType))
                    {
                        caption = ReconConstants.CONST_ToleranceType + " " + columnKey.Substring(ReconConstants.CONST_ToleranceType.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_ToleranceValue))
                    {
                        caption = ReconConstants.CONST_ToleranceValue + " " + columnKey.Substring(ReconConstants.CONST_ToleranceValue.Length);
                    }
                    else if (columnKey.Contains(ReconConstants.CONST_Diff))
                    {
                        caption = ReconConstants.CONST_Diff + " " + columnKey.Substring(ReconConstants.CONST_Diff.Length);
                    }
                    col.Header.Caption = caption.ToString();
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
        /// Set columns for ultragrid
        /// </summary>
        /// <param name="band"></param>
        private void SetColumnsForUltraGrid(UltraGridBand band)
        {
            try
            {

                foreach (UltraGridColumn column in band.Columns)
                {
                    UltraGridColumn gridColumn = column;
                    gridColumn.Header.Caption = column.Key;

                    HideColumnsNotSelected(gridColumn);

                    //following line auto adjust width of columns of ultragrid according to text size of header.
                    //gridColumn.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                    //Define minimum width for the ultragridcell to 100
                    //if (gridColumn.Width < 70)
                    //   gridColumn.Width = 100;
                }
                if (!band.Columns.Exists(ReconConstants.COLUMN_AccountName))
                {
                    if (band.Columns.Exists(ReconConstants.COLUMN_TaxLotID))
                    {
                        band.Columns[ReconConstants.COLUMN_TaxLotID].Hidden = true;
                    }
                    if (band.Columns.Exists(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_TradeDate))
                    {
                        band.Columns[ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_TradeDate].Hidden = true;
                    }
                }
                //if (band.Columns.Exists(ReconConstants.CONST_Broker + ReconConstants.COLUMN_RowIndex))   //commented 02.03.2015 because rowindex should not be shown on recon output
                //{
                //    band.Columns[ReconConstants.CONST_Broker + ReconConstants.COLUMN_RowIndex].Hidden = true;
                //}
                if (!ReconUtilities.GetSelectedColumnsList(_reconTemplate.SelectedColumnList).Contains(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_TradeDate)
                    && band.Columns.Exists(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_TradeDate))
                {
                    band.Columns[ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_TradeDate].Hidden = true;
                }
                //foreach (UltraGridRow  row in grdReconOutput.Rows)
                //{
                //    if (row.Cells[ReconConstants.MismatchReason].Text.Contains("Missing"))
                //    {
                //        row.Activation = Activation.NoEdit;
                //    }
                //}
                #region column lock status
                //band.Columns["LockStatus"].Hidden=true;
                if (!band.Columns.Exists(ReconConstants.COLUMN_NAVLockStatus) && band.Columns.Exists(ReconConstants.COLUMN_AccountName))
                {
                    band.Columns[ReconConstants.COLUMN_NAVLockStatus].NullText = "N.A";
                    //band.Columns[ReconConstants.COLUMN_NAVLockStatus].CellActivation = Activation.NoEdit;
                    //band.Columns[ReconConstants.COLUMN_NAVLockStatus].Header.Caption = ReconConstants.CAPTION_NAVLockStatus;
                }

                //SetValueForUltraGridCell(band, ReconConstants.COLUMN_LockStatus, ReconConstants.LockStatus_UnLocked);
                #endregion



                #region column comments(Added in Recon XML's)
                //if (band.Columns.Exists(ReconConstants.COLUMN_Comments))
                //{
                //band.Columns.Add(ReconConstants.COLUMN_Comments, ReconConstants.CAPTION_Comments);
                //}
                //if (band.Columns.Exists(ReconConstants.COLUMN_Comments))
                //{
                //    band.Columns[ReconConstants.COLUMN_Comments].CellActivation = Activation.AllowEdit;
                //}
                #endregion

                #region column ClosingStatus

                //if (band.Columns.Exists(ReconConstants.COLUMN_ClosingStatus))
                //{
                //    band.Columns[ReconConstants.COLUMN_ClosingStatus].CellActivation = Activation.NoEdit;
                //}
                #endregion
                #region Changed columns section
                band.Columns[ReconConstants.COLUMN_ChangedColumns].Hidden = true;
                band.Columns[ReconConstants.COLUMN_ChangedColumns].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                #endregion


                #region TaxlotID column section
                //if (band.Columns.Exists(ReconConstants.COLUMN_TaxLotID))
                //{
                //    band.Columns[ReconConstants.COLUMN_ChangedColumns].Hidden = true;
                //}
                #endregion

                if (band.Columns.Exists(ReconConstants.COLUMN_LockStatus))
                {
                    band.Columns[ReconConstants.COLUMN_LockStatus].MaxWidth = 20;
                }
                if (band.Columns.Exists(ReconConstants.COLUMN_Checkbox))
                {
                    band.Columns[ReconConstants.COLUMN_Checkbox].MaxWidth = 20;
                }

                if (band.Columns.Exists(ReconConstants.COLUMN_UserLoggedIN))
                {
                    //add Tax Lot Status Column to GeneralInformation Group on grid
                    //band.Columns[ReconConstants.COLUMN_UserLoggedIN].Group = band.Groups[ReconConstants.GRIDGroup_Generalinformation];
                    band.Columns[ReconConstants.COLUMN_UserLoggedIN].Hidden = true;
                    //    band.Columns[ReconConstants.COLUMN_UserLoggedIN].CellActivation = Activation.NoEdit;
                }
                //column will contain the user who is currently logged in from cache

                //if (band.Columns.Exists(ReconConstants.COLUMN_Comments))
                //{
                //    band.Columns[ReconConstants.COLUMN_Comments].CellActivation = Activation.NoEdit;
                //}
                if (band.Columns.Exists(ReconConstants.CONST_Broker + ReconConstants.COLUMN_RowIndex))
                {
                    band.Columns[ReconConstants.CONST_Broker + ReconConstants.COLUMN_RowIndex].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                    band.Columns[ReconConstants.CONST_Broker + ReconConstants.COLUMN_RowIndex].Hidden = true;
                }
                //if (band.Columns.Exists(ReconConstants.COLUMN_Checkbox))
                //{
                //TEXT ON HEADER OF CHECKBOX SHOULD NOT BE VISIBLE SO ITS FOND COLOR IS SET TO USERCONTROL COLOUR TO HIDE IT
                //   band.Columns[ReconConstants.COLUMN_Checkbox].Header.Appearance.BackColor = SystemColors.ControlLightLight;
                //   band.Columns[ReconConstants.COLUMN_Checkbox].Header.Appearance.ForeColor = SystemColors.ControlLightLight;
                //   band.Columns[ReconConstants.COLUMN_Checkbox].Width = 20;
                //band.Columns[ReconConstants.COLUMN_Checkbox].CellActivation = Activation.AllowEdit;
                //}
                //if (band.Columns.Exists(ReconConstants.COLUMN_LockStatus))
                //{
                //    band.Columns[ReconConstants.COLUMN_LockStatus].CellActivation = Activation.NoEdit;
                //}

                if (band.Columns.Exists("NirvanaTradeDate"))
                {
                    UltraGridColumn colTradeDate = band.Columns["NirvanaTradeDate"];
                    colTradeDate.Width = 80;
                    colTradeDate.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTime;
                    colTradeDate.MaskDataMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                    colTradeDate.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.IncludeLiterals;
                    colTradeDate.MaskInput = "mm/dd/yyyy";
                }
                //if (_reconTemplate.ReconType == ReconType.Position || _reconTemplate.ReconType == ReconType.TaxLot)// modified by amit 02.03.2015 CHMW-2894
                //{
                //    if (band.Columns.Exists("NirvanaMarkPrice"))
                //    {
                //        _isAmendmentsAllowed = true;
                //        UltraGridColumn colMarkPrice = band.Columns["NirvanaMarkPrice"];
                //        colMarkPrice.CellActivation = Activation.AllowEdit;
                //    }
                //}
                // Added by Ankit Gupta on 6 Nov, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1708
                // Set width of each column according to the length of its caption, refer above mentioned JIRA for more details.
                //foreach (UltraGridColumn column in grdReconOutput.DisplayLayout.Bands[0].Columns)
                //{
                //    if (column != null && column.Key != null)
                //    {
                //        if (column.Hidden || column.Key.Equals("LockStatus", StringComparison.InvariantCultureIgnoreCase) || column.Key.Equals("checkBox", StringComparison.InvariantCultureIgnoreCase))
                //        {
                //            continue;
                //        }
                //        column.PerformAutoResize(PerformAutoSizeType.VisibleRows, AutoResizeColumnWidthOptions.IncludeHeader);
                //       //column.Width = column.Width + 8;
                //    }
                //}
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
        //added by amit 02.03.2015 CHMW-2894
        private void SetColumnButtonType(UltraGridBand band)
        {
            try
            {

                if (band.Columns.Exists("Amend"))
                {
                    UltraGridColumn colAmend = band.Columns["Amend"];
                    colAmend.Width = 80;
                    colAmend.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colAmend.Header.Caption = "View/Amend";
                    colAmend.NullText = "View/Amend";
                    colAmend.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    //colAmend.Group = band.Groups[ReconConstants.GRIDGroup_GeneralInformation];
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
            //throw new NotImplementedException();
        }



        private void HideColumnsNotSelected(UltraGridColumn gridColumn)
        {
            try
            {

                if (gridColumn.Key.Contains(ReconConstants.CONST_OriginalValue))
                {
                    //make all originalvalue columns hidden.
                    gridColumn.Hidden = true;
                }
                else if (gridColumn.Key.Contains(ReconConstants.CONST_ToleranceValue))
                {
                    string columnName = gridColumn.Key.Substring(ReconConstants.CONST_ToleranceValue.Length);
                    if (!(gridColumn.Band.Columns.Exists(ReconConstants.CONST_Nirvana + columnName) || gridColumn.Band.Columns.Exists(ReconConstants.CONST_Broker + columnName)))
                    {
                        gridColumn.Hidden = true;
                    }
                }
                else if (gridColumn.Key.StartsWith(ReconConstants.CONST_ToleranceType))
                {
                    string columnName = gridColumn.Key.Substring(ReconConstants.CONST_ToleranceType.Length);
                    if (!(gridColumn.Band.Columns.Exists(ReconConstants.CONST_Nirvana + columnName) || gridColumn.Band.Columns.Exists(ReconConstants.CONST_Broker + columnName)))
                    {
                        gridColumn.Hidden = true;
                    }
                }
                else if (gridColumn.Key.StartsWith(ReconConstants.CONST_ReconStatus))
                {
                    string columnName = gridColumn.Key.Substring(ReconConstants.CONST_ReconStatus.Length);
                    if (!(gridColumn.Band.Columns.Exists(ReconConstants.CONST_Nirvana + columnName) || gridColumn.Band.Columns.Exists(ReconConstants.CONST_Broker + columnName)))
                    {
                        gridColumn.Hidden = true;
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

        private void AssignColumnToRespectiveGroup(UltraGridBand band, Dictionary<string, UltraGridGroup> dictUltraGridGroup)
        {
            try
            {
                if (band.Groups.Exists(ReconConstants.GRIDGroup_GeneralInformation))
                {
                    if (band.Columns.Exists(ReconConstants.COLUMN_NAVLockStatus) && !band.Columns[ReconConstants.COLUMN_NAVLockStatus].Hidden && band.Columns[ReconConstants.COLUMN_NAVLockStatus].Group == null)
                    {
                        band.Columns[ReconConstants.COLUMN_NAVLockStatus].Group = band.Groups[ReconConstants.GRIDGroup_GeneralInformation];
                    }
                    if (band.Columns.Exists(ReconConstants.COLUMN_ChangedColumns) && !band.Columns[ReconConstants.COLUMN_ChangedColumns].Hidden && band.Columns[ReconConstants.COLUMN_ChangedColumns].Group == null)
                    {
                        band.Columns[ReconConstants.COLUMN_ChangedColumns].Group = band.Groups[ReconConstants.GRIDGroup_GeneralInformation];
                    }
                    if (band.Columns.Exists(ReconConstants.COLUMN_Comments) && !band.Columns[ReconConstants.COLUMN_Comments].Hidden && band.Columns[ReconConstants.COLUMN_Comments].Group == null)
                    {
                        band.Columns[ReconConstants.COLUMN_Comments].Group = band.Groups[ReconConstants.GRIDGroup_GeneralInformation];
                    }
                    if (band.Columns.Exists(ReconConstants.COLUMN_ClosingStatus) && !band.Columns[ReconConstants.COLUMN_ClosingStatus].Hidden && band.Columns[ReconConstants.COLUMN_ClosingStatus].Group == null)
                    {
                        band.Columns[ReconConstants.COLUMN_ClosingStatus].Group = band.Groups[ReconConstants.GRIDGroup_GeneralInformation];
                    }
                    if (band.Columns.Exists(ReconConstants.COLUMN_Checkbox) && !band.Columns[ReconConstants.COLUMN_Checkbox].Hidden && band.Columns[ReconConstants.COLUMN_Checkbox].Group == null)
                    {
                        band.Columns[ReconConstants.COLUMN_Checkbox].Group = band.Groups[ReconConstants.GRIDGroup_GeneralInformation];
                        band.Groups[ReconConstants.GRIDGroup_GeneralInformation].Columns[ReconConstants.COLUMN_Checkbox].Header.VisiblePosition = 0;
                        band.Groups[ReconConstants.GRIDGroup_GeneralInformation].Columns[ReconConstants.COLUMN_Checkbox].Width = 20;
                    }
                    if (band.Columns.Exists(ReconConstants.COLUMN_LockStatus) && !band.Columns[ReconConstants.COLUMN_LockStatus].Hidden && band.Columns[ReconConstants.COLUMN_LockStatus].Group == null)
                    {
                        band.Columns[ReconConstants.COLUMN_LockStatus].Group = band.Groups[ReconConstants.GRIDGroup_GeneralInformation];
                        band.Groups[ReconConstants.GRIDGroup_GeneralInformation].Columns[ReconConstants.COLUMN_LockStatus].Header.VisiblePosition = 1;
                        band.Groups[ReconConstants.GRIDGroup_GeneralInformation].Columns[ReconConstants.COLUMN_LockStatus].Width = 20;
                    }
                }
                //sets column to their respective group
                foreach (KeyValuePair<string, UltraGridGroup> kvp in dictUltraGridGroup)
                {
                    if (band.Columns.Exists(ReconConstants.CONST_Nirvana + kvp.Key) && band.Columns[ReconConstants.CONST_Nirvana + kvp.Key].Group == null)
                    {
                        band.Columns[ReconConstants.CONST_Nirvana + kvp.Key].Group = kvp.Value;
                    }
                    if (band.Columns.Exists(ReconConstants.CONST_Broker + kvp.Key) && band.Columns[ReconConstants.CONST_Broker + kvp.Key].Group == null)
                    {
                        band.Columns[ReconConstants.CONST_Broker + kvp.Key].Group = kvp.Value;
                    }
                    if (band.Columns.Exists(ReconConstants.CONST_ToleranceType + kvp.Key) && band.Columns[ReconConstants.CONST_ToleranceType + kvp.Key].Group == null)
                    {
                        band.Columns[ReconConstants.CONST_ToleranceType + kvp.Key].Group = kvp.Value;
                    }
                    if (band.Columns.Exists(ReconConstants.CONST_ToleranceValue + kvp.Key) && band.Columns[ReconConstants.CONST_ToleranceValue + kvp.Key].Group == null)
                    {
                        band.Columns[ReconConstants.CONST_ToleranceValue + kvp.Key].Group = kvp.Value;
                    }
                    if (band.Columns.Exists(ReconConstants.CONST_Diff + kvp.Key) && band.Columns[ReconConstants.CONST_Diff + kvp.Key].Group == null)
                    {
                        band.Columns[ReconConstants.CONST_Diff + kvp.Key].Group = kvp.Value;
                    }
                    if (band.Columns.Exists(ReconConstants.CONST_ReconStatus + kvp.Key) && band.Columns[ReconConstants.CONST_ReconStatus + kvp.Key].Group == null)
                    {
                        band.Columns[ReconConstants.CONST_ReconStatus + kvp.Key].Group = kvp.Value;
                    }
                    if (band.Columns.Exists(ReconConstants.CONST_OriginalValue + kvp.Key) && band.Columns[ReconConstants.CONST_OriginalValue + kvp.Key].Group == null)
                    {
                        band.Columns[ReconConstants.CONST_OriginalValue + kvp.Key].Group = kvp.Value;
                    }
                }
                int i = 2;
                foreach (UltraGridColumn column in band.Columns)
                {
                    //checks if it is not added to any other group then it is added to GeneralInformation Group Tab
                    if (column.Group == null && !column.Hidden)
                    {
                        column.Group = dictUltraGridGroup[ReconConstants.GRIDGroup_GeneralInformation];
                        band.Groups[ReconConstants.GRIDGroup_GeneralInformation].Columns[column.Key].Header.VisiblePosition = i++;
                    }
                }

                if (band.Columns.Exists(ReconConstants.COLUMN_TaxLotStatus) && band.Columns[ReconConstants.COLUMN_TaxLotStatus].Group == null)
                {
                    band.Columns[ReconConstants.COLUMN_TaxLotStatus].Group = band.Groups[ReconConstants.GRIDGroup_GeneralInformation];
                }
                //added by amit 02.03.2015 CHMW-2894
                if (band.Columns.Exists("Amend") && band.Columns["Amend"].Group == null)
                {
                    band.Columns["Amend"].Group = band.Groups[ReconConstants.GRIDGroup_GeneralInformation];
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

        private Dictionary<string, UltraGridGroup> CreateUltraGridGroups(UltraGridBand band)
        {
            Dictionary<string, UltraGridGroup> dictUltraGridGroup = new Dictionary<string, UltraGridGroup>();
            try
            {
                //creates a ultragrid for general column
                UltraGridGroup gridGrouppGeneralInfo = new UltraGridGroup(ReconConstants.GRIDGroup_GeneralInformation, 0);
                gridGrouppGeneralInfo.Header.VisiblePosition = 0;
                gridGrouppGeneralInfo.Header.Appearance.TextHAlign = HAlign.Center;
                int groupid = 0;
                if (!dictUltraGridGroup.ContainsKey(ReconConstants.GRIDGroup_GeneralInformation))
                {
                    dictUltraGridGroup.Add(ReconConstants.GRIDGroup_GeneralInformation, gridGrouppGeneralInfo);
                    band.Groups.Add(gridGrouppGeneralInfo);
                }
                //checks for all the groups to be formed from columns containing "Nirvana"  as header caption
                foreach (UltraGridColumn column in band.Columns)
                {
                    //handling is done for header caption
                    if (!column.Hidden)
                    {
                        string groupName = string.Empty;
                        if (column.Key.StartsWith(ReconConstants.CONST_Nirvana))
                        {
                            groupName = column.Key.Substring(ReconConstants.CONST_Nirvana.Length).Trim();
                        }
                        if (column.Key.StartsWith(ReconConstants.CONST_Broker))
                        {
                            groupName = column.Key.Substring(ReconConstants.CONST_Broker.Length).Trim();
                        }
                        if (column.Key.StartsWith(ReconConstants.CONST_Diff))
                        {
                            groupName = column.Key.Substring(ReconConstants.CONST_Diff.Length).Trim();
                        }
                        if (!string.IsNullOrEmpty(groupName))
                        {
                            //Initializelayout is called each time when 
                            //ultragrid group are saved in global dictionary
                            if (!dictUltraGridGroup.ContainsKey(groupName))
                            {
                                UltraGridGroup tempGrp = new UltraGridGroup(groupName, ++groupid);
                                dictUltraGridGroup.Add(groupName, tempGrp);
                                tempGrp.Header.Appearance.TextHAlign = HAlign.Center;
                                band.Groups.Add(tempGrp);
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
            return dictUltraGridGroup;
        }

        /// <summary>
        /// set intial value of colum to value passed
        /// </summary>
        /// <param name="band"></param>
        /// <param name="ColumnName"></param>
        /// <param name="Value"></param>
        //private void SetValueForUltraGridCell(UltraGridBand band, string ColumnName, string Value)
        //{
        //    try
        //    {
        //        band.Columns[ColumnName].Header.Appearance.Image = Properties.Resources.unlock;
        //        foreach (UltraGridRow row in band.Layout.Rows)
        //        {
        //            //object O = Properties.Resources.ResourceManager.GetObject("Unlocked"); //Return an object from the image chan1.png in the project
        //            //channelPic.Image = (Image)O;
        //            row.Cells[ReconConstants.COLUMN_LockStatus].Appearance.Image = Properties.Resources.unlock;
        //            row.Cells[ReconConstants.COLUMN_LockStatus].Value = ReconConstants.LockStatus_UnLocked;
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
        /// sets lock status on context menu click
        /// </summary>
        /// <param name="lockStatus"></param>
        private void SetLockStatusForUltraGridCell(string lockStatus)
        {
            try
            {
                foreach (UltraGridRow row in grdReconOutput.Rows)
                {
                    if (row.Cells.Exists(ReconConstants.COLUMN_Checkbox))
                    {
                        bool value = Convert.ToBoolean(row.Cells[ReconConstants.COLUMN_Checkbox].Text);
                        if (value == true)
                        {
                            //row.Cells[ReconConstants.COLUMN_LockStatus].Value = lockStatus;
                            if (lockStatus.Equals("Locked"))
                            {
                                row.Cells[ReconConstants.COLUMN_LockStatus].Appearance.Image = Properties.Resources._lock;
                                row.Cells[ReconConstants.COLUMN_LockStatus].Value = ReconConstants.LockStatus_Locked;
                                //row.Activation = Activation.NoEdit;
                            }
                            else
                            {
                                row.Cells[ReconConstants.COLUMN_LockStatus].Appearance.Image = Properties.Resources.unlock;
                                row.Cells[ReconConstants.COLUMN_LockStatus].Value = ReconConstants.LockStatus_UnLocked;
                                //if (row.Cells.Exists(ReconConstants.COLUMN_NAVLockStatus)
                                //&& row.Cells[ReconConstants.COLUMN_NAVLockStatus].Value == ReconConstants.LockStatus_UnLocked
                                //&& row.Cells.Exists(ReconConstants.MismatchReason)
                                //&& row.Cells[ReconConstants.MismatchReason].Value != ReconConstants.MismatchReason_DataMissingNirvana
                                //&& row.Cells.Exists(ReconConstants.COLUMN_TaxLotStatus)
                                //&& (row.Cells["TaxLotStatus"].Value != ReconConstants.MismatchReason_DataMissingNirvana
                                // || row.Cells["TaxLotStatus"].Value == null)
                                //&& row.Cells.Exists(ReconConstants.COLUMN_ClosingStatus)
                                //&& row.Cells[ReconConstants.COLUMN_ClosingStatus].Value.ToString().Equals(ClosingStatus.Open.ToString()))
                                //{
                                //row.Activation = Activation.AllowEdit;
                                //}
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

        //private void SetCheckBoxAtFirstPosition(UltraGrid grid)
        //{
        //    try
        //    {
        //        if (grid.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.COLUMN_Checkbox))
        //        {
        //            grid.DisplayLayout.Bands[0].Columns[ReconConstants.COLUMN_Checkbox].Hidden = false;
        //            grid.DisplayLayout.Bands[0].Columns[ReconConstants.COLUMN_Checkbox].Header.VisiblePosition = 0;
        //            grid.DisplayLayout.Bands[0].Columns[ReconConstants.COLUMN_Checkbox].Width = 10;
        //            grid.DisplayLayout.Bands[0].Columns[ReconConstants.COLUMN_Checkbox].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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

        // private void headerCheckBox__CLICKED(object sender, CheckBoxOnHeader_CreationFilter.HeaderCheckBoxEventArgs e)
        // {

        // }
        private void lockedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetLockStatusForUltraGridCell(ReconConstants.LockStatus_Locked);
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
        private void unlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SetLockStatusForUltraGridCell(ReconConstants.LockStatus_UnLocked);

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
        /// for exporting .cv,.pdf,.xls files 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void exportToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {

                if (grdReconOutput.DataSource != null && grdReconOutput.Rows.IsGroupByRows != true)
                {
                    //grdReconOutput.DisplayLayout.PerformAutoResizeColumns(true, PerformAutoSizeType.VisibleRows);
                    //MemoryStream s = new MemoryStream();
                    //grdReconOutput.DisplayLayout.Save(s);
                    //grdReconOutputExport.DataSource = ((BindingSource)(grdReconOutput.DataSource)).DataSource;



                    WireUnwiredEvents(true);

                    foreach (UltraGridRow row in grdReconOutput.Rows)
                    {
                        foreach (UltraGridCell cell in row.Cells)
                        {
                            cell.Value = cell.Text;
                        }
                    }
                    WireUnwiredEvents(false);
                    string pathName = null;
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.InitialDirectory = Application.StartupPath;
                    saveFileDialog1.Filter = "Excel WorkBook File (*.xls)|*.xls|CSV File (*.csv)|*.csv|PDF File (*.PDF)|*.pdf";
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {

                        grdReconOutput.DisplayLayout.Bands[0].GroupHeadersVisible = false;
                        HiddenColumns(true);
                        pathName = saveFileDialog1.FileName;
                        UltraGridColumn col = grdReconOutput.DisplayLayout.Bands[0].Columns[0];
                        col = col.GetRelatedVisibleColumn(VisibleRelation.First);

                        UltraGridRow row = grdReconOutput.DisplayLayout.Bands[0].AddNew();
                        UltraGridRow row1 = grdReconOutput.DisplayLayout.Bands[0].AddNew();
                        UltraGridRow row2 = grdReconOutput.DisplayLayout.Bands[0].AddNew();
                        row.ParentCollection.Move(row, grdReconOutput.Rows.Count - 1);
                        row1.ParentCollection.Move(row1, grdReconOutput.Rows.Count - 1);
                        row2.ParentCollection.Move(row2, grdReconOutput.Rows.Count - 1);
                        row1.Cells[col.Index].Value = "Comment:- " + txtUserComments.Text;
                        UltraGridFileExporter.ExportFileForFileFormat(grdReconOutput, pathName);
                        grdReconOutput.Rows[(grdReconOutput.Rows.Count - 1)].Delete(false);
                        grdReconOutput.Rows[(grdReconOutput.Rows.Count - 1)].Delete(false);
                        grdReconOutput.Rows[(grdReconOutput.Rows.Count - 1)].Delete(false);


                        grdReconOutput.DisplayLayout.Bands[0].GroupHeadersVisible = true;
                        HiddenColumns(false);
                        // grdReconOutput.DisplayLayout.Load(s);

                    }
                }
                else
                {
                    MessageBox.Show("Remove Grouping first !", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// clear Grid Filters Tool Strip Menu Item_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clearGridFiltersToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            try
            {
                UltraWinGridUtils.ClearAllGridFilters(grdReconOutput);
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
        /// makes hidden some column for exporting purpose.
        /// </summary>
        /// <param name="isHidden"></param>
        private void HiddenColumns(bool isHidden)
        {
            UltraGridBand band = grdReconOutput.DisplayLayout.Bands[0];
            if (band.Columns.Exists(ReconConstants.COLUMN_LockStatus))
            {
                grdReconOutput.DisplayLayout.Bands[0].Columns[ReconConstants.COLUMN_LockStatus].Hidden = isHidden;
            }
            if (band.Columns.Exists(ReconConstants.COLUMN_Checkbox))
            {
                band.Columns[ReconConstants.COLUMN_Checkbox].Hidden = isHidden;
            }
            if (band.Columns.Exists(ReconConstants.LockStatus_Locked))
            {
                band.Columns[ReconConstants.LockStatus_Locked].Hidden = isHidden;
            }
            if (band.Columns.Exists(ReconConstants.LockStatus_UnLocked))
            {
                band.Columns[ReconConstants.LockStatus_UnLocked].Hidden = isHidden;
            }
            if (band.Columns.Exists(ReconConstants.COLUMN_TaxLotID))
            {
                band.Columns[ReconConstants.COLUMN_TaxLotID].Hidden = isHidden;
            }

        }
        /// <summary>
        /// for file exporting propose.
        /// </summary>
        /// <param name="isWired"></param>
        private void WireUnwiredEvents(bool isWired)
        {
            try
            {
                if (isWired)
                {
                    this.grdReconOutput.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReconOutput_AfterCellUpdate);
                    this.grdReconOutput.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdReconOutput_InitializeLayout);
                    this.grdReconOutput.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdReconOutput_InitializeRow);
                    this.grdReconOutput.BeforeRowActivate -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdReconOutput_BeforeRowActivate);
                    this.grdReconOutput.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReconOutput_CellChange);
                    this.grdReconOutput.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReconOutput_ClickCellButton);
                    this.grdReconOutput.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdReconOutput_BeforeCellUpdate);

                }
                else
                {
                    this.grdReconOutput.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReconOutput_AfterCellUpdate);
                    this.grdReconOutput.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdReconOutput_InitializeLayout);
                    this.grdReconOutput.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdReconOutput_InitializeRow);
                    this.grdReconOutput.BeforeRowActivate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdReconOutput_BeforeRowActivate);
                    this.grdReconOutput.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReconOutput_CellChange);
                    this.grdReconOutput.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReconOutput_ClickCellButton);
                    this.grdReconOutput.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdReconOutput_BeforeCellUpdate);

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

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                if (_reconTemplate == null && !_isAmendmentsAllowed)
                {
                    e.Cancel = true;
                    return;
                }
                if (grdReconOutput.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.COLUMN_ClosingStatus)
                    && grdReconOutput.ActiveRow != null
                    && grdReconOutput.ActiveRow.Cells != null
                    && grdReconOutput.ActiveRow.Cells.Exists(ReconConstants.MismatchReason)
                    && grdReconOutput.ActiveRow.Cells[ReconConstants.MismatchReason].Value != null
                    && grdReconOutput.ActiveRow.Cells[ReconConstants.MismatchReason].Value != DBNull.Value
                    && !grdReconOutput.ActiveRow.Cells[ReconConstants.MismatchReason].Value.ToString().Equals(ReconConstants.MismatchReason_DataMissingPB))
                {
                    contextMenuStrip1.Items["viewInFileToolStripMenuItem"].Enabled = true;
                }
                else
                {
                    contextMenuStrip1.Items["viewInFileToolStripMenuItem"].Enabled = false;
                    if (grdReconOutput.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.COLUMN_ClosingStatus))
                    {
                        contextMenuStrip1.Items["lockedToolStripMenuItem"].Enabled = false;
                        contextMenuStrip1.Items["unlockToolStripMenuItem"].Enabled = false;
                    }
                }
                //contextMenuStrip1.Items[1].Enabled = false;
                //contextMenuStrip1.Items[0].Enabled = false;
                //foreach (UltraGridRow row in grdReconOutput.Rows)
                //{
                //bool value = false;
                //if (row.Cells != null)
                //{
                //if (row.Cells.Exists(ReconConstants.COLUMN_Checkbox))
                //{
                //value = Convert.ToBoolean(row.Cells[ReconConstants.COLUMN_Checkbox].Text);
                //}
                //if (value == true)
                //{
                //foreach (ToolStripItem item in contextMenuStrip1.Items)
                //{
                //item.Enabled = true;
                //item.Enabled = true;
                //}
                //break;
                //}
                //}
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


        private void grdReconOutput_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                //as there are multiple values updated in cell update, so we are wiring it again in finally block
                grdReconOutput.AfterCellUpdate -= new CellEventHandler(this.grdReconOutput_AfterCellUpdate);

                if (e.Cell.GetType() == typeof(UltraGridFilterCell))
                {
                    return;
                }
                //int rowIndex = e.Cell.Row.Index;
                //1. Copy failed values from broker to nirvana
                //2. Copy all values from broker to nirvana
                //3. User changes nirvana value manually
                string colKey = e.Cell.Column.Key;
                if (colKey.Contains(ReconConstants.CONST_Nirvana) && !string.IsNullOrEmpty(e.Cell.Value.ToString()))
                {
                    if (e.Cell.Column.Group != null && colKey != "NirvanaMarkPrice")
                    {
                        //CHMW-3160	[Recon] Amendments are not saving if a column is under different group
                        string colName = string.Empty;
                        if (colKey.Contains(ReconConstants.CONST_Nirvana))
                        {
                            colName = colKey.Replace(ReconConstants.CONST_Nirvana, string.Empty).Trim();
                        }
                        UpdateChangedColumns(e.Cell.Row, colName);
                    }
                    UpdateGridCells(e.Cell, colKey);
                    if (colKey == "NirvanaMarkPrice")
                    {
                        UpdateMarkPriceForSymbolandAccountCombination(e);
                        UpdateMarkPriceChangesToDataTable(e);
                    }
                    FillStatus(e.Cell);
                }
                if (e.Cell.Column.Key.Equals("NirvanaTradeDate"))
                {
                    bool isValidTradeDate = false;
                    DateTime dt = DateTime.MinValue;
                    isValidTradeDate = DateTime.TryParse(e.Cell.Value.ToString(), out dt);

                    if (!isValidTradeDate)
                    {
                        MessageBox.Show("Invalid trade date.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cell.CancelUpdate();
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
            finally
            {
                grdReconOutput.AfterCellUpdate += new CellEventHandler(this.grdReconOutput_AfterCellUpdate);
            }
        }

        private void FillStatus(UltraGridCell cell)
        {
            //update MismatchDetails columns on nirvanaValue Change
            List<string> mismatchDetails = new List<string>();
            try
            {
                foreach (UltraGridColumn col in cell.Band.Columns)
                {
                    if (col.Header.Caption.Contains(ReconConstants.CONST_ReconStatus))
                    {
                        if (cell.Row.Cells[col.Key].Text != Prana.BusinessObjects.AppConstants.ReconStatus.ExactlyMatched.ToString()
                            && !cell.Row.Cells[col.Key].Text.Contains("Missing"))
                        {
                            string columnName = col.Header.Caption.Substring(ReconConstants.CONST_ReconStatus.Length).Trim();
                            string mismatchdetails = columnName + " " + ReconConstants.MismatchType_MismatchWithPB;
                            mismatchDetails.Add(mismatchdetails);
                        }
                    }
                }
                //if MismatchDetails column contains missing data then no data is altered
                if (cell.Row.Cells.Exists(ReconConstants.MismatchReason))
                {
                    if (!cell.Row.Cells[ReconConstants.MismatchReason].Text.Contains("Missing"))
                    {
                        cell.Row.Cells[ReconConstants.MismatchReason].Value = string.Join(Seperators.SEPERATOR_8, mismatchDetails.ToArray());
                    }
                }
                //modified by amit 23.03.2015
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3081
                if (cell.Row.Cells.Exists(ReconConstants.MismatchReason) && cell.Row.Cells.Exists(ReconConstants.MismatchType))
                {
                    if (cell.Row.Cells[ReconConstants.MismatchReason].Text.Contains("Missing"))
                        cell.Row.Cells[ReconConstants.MismatchType].Value = "Missing Data";
                    else if (mismatchDetails.Count > 1)
                        cell.Row.Cells[ReconConstants.MismatchType].Value = "Multiple Mismatches";
                    else if (mismatchDetails.Count == 0)
                        cell.Row.Cells[ReconConstants.MismatchType].Value = "Exactly Matched";
                    else
                        cell.Row.Cells[ReconConstants.MismatchType].Value = string.Join(string.Empty, mismatchDetails.ToArray());
                }
                CalculateDependentColumns(cell);
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

        private static void UpdateGridCells(UltraGridCell cell, string colKey)
        {
            try
            {
                string colName = cell.Column.Header.Caption.Substring(ReconConstants.CONST_Nirvana.Length).Trim();
                if (cell.Row.Cells.Exists(ReconConstants.CONST_Diff + colName))
                {
                    decimal nirOriginalValue = 0.0M;
                    if (decimal.TryParse(cell.Row.Cells[ReconConstants.CONST_Nirvana + colName].OriginalValue.ToString(), out nirOriginalValue))
                    {
                        decimal nirValue;
                        if (decimal.TryParse(cell.Row.Cells[ReconConstants.CONST_Nirvana + colName].Value.ToString(), out nirValue))
                        {
                            decimal originalDiff;
                            if (decimal.TryParse(cell.Row.Cells[ReconConstants.CONST_Diff + colName].Value.ToString(), out originalDiff))
                            {
                                cell.Row.Cells[ReconConstants.CONST_Diff + colName].Value = (originalDiff - (nirOriginalValue - nirValue));
                            }
                        }
                    }
                }
                // if nirvana value is equal to broker value then change recon status to MatchedExactly 
                //Decimal valPB, valNirvana;
                Double valDiff;
                DateTime dateNirvana = DateTime.MinValue;
                DateTime datePB = DateTime.MinValue;
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1348
                if (cell.Row.Cells.Exists(colKey) && cell.Row.Cells.Exists(ReconConstants.CONST_Broker + colName)
                            && ((colKey.Contains(ReconConstants.COLUMN_TradeDate)
                            && DateTime.TryParse(cell.Row.Cells[colKey].Text.ToString(), out dateNirvana)
                            && DateTime.TryParse(cell.Row.Cells[ReconConstants.CONST_Broker + colName].Text.ToString(), out datePB)
                        && dateNirvana == datePB)
                        ||
                            (cell.Row.Cells.Exists(colKey)
                            && cell.Row.Cells.Exists(ReconConstants.CONST_Diff + colName)
                    && Double.TryParse(cell.Row.Cells[ReconConstants.CONST_Diff + colName].Text.ToString(), out valDiff)
                && valDiff == 0.0)
            ||
            cell.Row.Cells[colKey].Text.ToString().Equals(cell.Row.Cells["Broker" + colName].Text.ToString())))
                //(decimal.TryParse(e.Cell.Row.Cells[colKey].Text.ToString(), out valNirvana)
                //&& decimal.TryParse(e.Cell.Row.Cells[ReconConstants.CONST_Broker + colName].Text.ToString(), out valPB)
                //&& valNirvana == valPB))
                {
                    if (cell.Row.Cells.Exists(ReconConstants.CONST_ReconStatus + colName))
                    {
                        if (!cell.Row.Cells[ReconConstants.CONST_ReconStatus + colName].Value.ToString().ToLower().Contains("missing"))
                        {
                            //modified by amit 09.04.2015
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-3306
                            cell.Row.Cells[ReconConstants.CONST_ReconStatus + colName].Value = Prana.BusinessObjects.AppConstants.ReconStatus.ExactlyMatched.ToString();
                            if (cell.Row.Cells.Exists(ReconConstants.CONST_ToleranceType + colName))
                                cell.Row.Cells[ReconConstants.CONST_ToleranceType + colName].Value = Prana.BusinessObjects.AppConstants.ToleranceType.None.ToString();
                            if (cell.Row.Cells.Exists(ReconConstants.CONST_ToleranceValue + colName))
                                cell.Row.Cells[ReconConstants.CONST_ToleranceValue + colName].Value = 0;
                        }
                    }
                }
                // if nirvana value is not equal to broker value then change recon status to Mismatch
                else
                {
                    if (cell.Row.Cells.Exists(ReconConstants.CONST_ReconStatus + colName))
                    {
                        if (!cell.Row.Cells[ReconConstants.CONST_ReconStatus + colName].Value.ToString().ToLower().Contains("missing"))
                        {
                            //modified by amit 09.04.2015
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-3306
                            cell.Row.Cells[ReconConstants.CONST_ReconStatus + colName].Value = Prana.BusinessObjects.AppConstants.ReconStatus.MisMatch.ToString();
                            if (cell.Row.Cells.Exists(ReconConstants.CONST_ToleranceType + colName))
                                cell.Row.Cells[ReconConstants.CONST_ToleranceType + colName].Value = Prana.BusinessObjects.AppConstants.ToleranceType.None.ToString();
                            if (cell.Row.Cells.Exists(ReconConstants.CONST_ToleranceValue + colName))
                                cell.Row.Cells[ReconConstants.CONST_ToleranceValue + colName].Value = 0;
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

        private void CalculateDependentColumns(UltraGridCell cell)
        {
            try
            {
                if (cell.Column.Key == ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_Quantity || cell.Column.Key == ReconConstants.CONST_Nirvana + ReconConstants.CONST_AvgPX)
                {

                    if (cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.CONST_AvgPX) && cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_Quantity))
                    {
                        double avgpx = Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_AvgPX].Value);
                        double quantity = Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_Quantity].Value);
                        if (cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + OrderFields.CONST_NetNotionalValue)
                        && cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.CONST_Multiplier)
                        && cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.CONST_SideMultiplier)
                        && cell.Row.Cells.Exists(ReconConstants.CONST_NirvanaTotalCommissionAndFees)
                        && cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + OrderFields.CONST_NetNotionalValue))
                        {
                            double multiplier = Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_Multiplier].Value);
                            double sideMultiplier = Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_SideMultiplier].Value);
                            double totalCommissionFees = Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.COL_TOTALCOMMISSIONANDFEES].Value);
                            double netNotionalValue = Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Nirvana + OrderFields.CONST_NetNotionalValue].Value);


                            netNotionalValue = Math.Round((quantity * avgpx * multiplier) + (sideMultiplier * totalCommissionFees), 4);
                            if (netNotionalValue != Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Nirvana + OrderFields.CONST_NetNotionalValue].Value))
                            {
                                cell.Row.Cells[ReconConstants.CONST_Nirvana + OrderFields.CONST_NetNotionalValue].Value = netNotionalValue;

                                if (cell.Row.Cells.Exists(ReconConstants.CONST_Diff + OrderFields.CONST_NetNotionalValue) && cell.Row.Cells.Exists(ReconConstants.CONST_Broker + OrderFields.CONST_NetNotionalValue))
                                {
                                    cell.Row.Cells[ReconConstants.CONST_Diff + OrderFields.CONST_NetNotionalValue].Value = netNotionalValue -
                                        Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Broker + OrderFields.CONST_NetNotionalValue].Value);
                                }
                            }
                        }
                        if (cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.CONST_Multiplier)
                        && cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.CONST_GrossNotionalValue))
                        {
                            double multiplier = Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_Multiplier].Value);
                            double grossValue = Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_GrossNotionalValue].Value);
                            grossValue = Math.Round(quantity * avgpx * multiplier);
                            if (grossValue != Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_GrossNotionalValue].Value))
                            {
                                cell.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_GrossNotionalValue].Value = grossValue;

                                if (cell.Row.Cells.Exists(ReconConstants.CONST_Diff + ReconConstants.CONST_GrossNotionalValue) && cell.Row.Cells.Exists(ReconConstants.CONST_Broker + ReconConstants.CONST_GrossNotionalValue))
                                {
                                    cell.Row.Cells[ReconConstants.CONST_Diff + ReconConstants.CONST_GrossNotionalValue].Value = grossValue -
                                        Convert.ToDouble(cell.Row.Cells[ReconConstants.CONST_Broker + ReconConstants.CONST_GrossNotionalValue].Value);
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
        }

        private void grdReconOutput_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            this.grdReconOutput.BeginUpdate();
            this.grdReconOutput.SuspendRowSynchronization();
            try
            {
                #region Hide Row
                if (e.Row.Cells.Exists(ReconConstants.COLUMN_AccountName) && e.Row.Cells[ReconConstants.COLUMN_AccountName].Value != null
                    && !string.IsNullOrEmpty(e.Row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString()) && !CachedDataManager.GetInstance.GetUserAccounts().Contains(e.Row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString()))
                {

                    //int accountID = int.MinValue;
                    //if (grdReconOutput.DisplayLayout.Bands[0].Columns.Exists("AccountID") && e.Row.Cells.Exists("AccountID") && !string.IsNullOrEmpty(e.Row.Cells["AccountID"].Value.ToString()))
                    //    Int32.TryParse(e.Row.Cells["AccountID"].Value.ToString(), out accountID);
                    //#region When User Dont have permission for account
                    //if (accountID != int.MinValue && !CachedDataManager.GetInstance.GetUserAccounts().Contains(accountID))
                    //{
                    e.Row.Hidden = true;
                }
                #endregion
                //#region Fetch Account ID
                //int accountID = int.MinValue;
                //if (e.Row.Cells.Exists(ReconConstants.COLUMN_AccountName) && e.Row.Cells[ReconConstants.COLUMN_AccountName].Value != null
                //    && !string.IsNullOrEmpty(e.Row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString()))
                //{
                //    accountID = CachedDataManager.GetInstance.GetAccountID(e.Row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString());
                //}
                //#endregion

                //#region Hide Row
                //#region When User Dont have permission for account
                //if (accountID != int.MinValue && !CachedDataManager.GetInstance.GetUserAccounts().Contains(accountID))
                //{
                //    e.Row.Hidden = true;
                //}
                //#endregion
                //#endregion

                //#region Disable row editing
                //#region When Data is missing in nirvana
                //if (e.Row.Cells.Exists("MismatchDetails") && e.Row.Cells["MismatchDetails"].Text.Contains(ReconConstants.MismatchReason_DataMissingNirvana))
                //{
                //    e.Row.Activation = Activation.NoEdit;
                //}
                //#endregion
                //#region When trade is partially closed or closed
                //if (e.Row.Cells.Exists(ReconConstants.COLUMN_ClosingStatus))// && e.Row.Cells.Exists(ReconConstants.COLUMN_TaxLotID) && _dictClosingStatus.Keys.Contains(e.Row.Cells[ReconConstants.COLUMN_TaxLotID].Value.ToString()))
                //{
                //    //e.Row.Cells[ReconConstants.COLUMN_ClosingStatus].Value = _dictClosingStatus[e.Row.Cells[ReconConstants.COLUMN_TaxLotID].Value.ToString()];
                //    //disable editing if the closing status is not open
                //    if (e.Row.Cells[ReconConstants.COLUMN_ClosingStatus].Value != null && !e.Row.Cells[ReconConstants.COLUMN_ClosingStatus].Value.ToString().Equals(ClosingStatus.Open.ToString()) && _reconTemplate.ReconType != ReconType.Position && _reconTemplate.ReconType != ReconType.TaxLot)
                //    {
                //        e.Row.Activation = Activation.NoEdit;
                //    }
                //}
                //#endregion
                #region When nav for account is Locked(Also Fills Nav lock Status)
                if (e.Row.Cells.Exists(ReconConstants.COLUMN_NAVLockStatus) && e.Row.Cells.Exists(ReconConstants.COLUMN_AccountName) && !string.IsNullOrEmpty(e.Row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString()))
                {
                    string accountName = e.Row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString();
                    string tradeDate = e.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_TradeDate].Value.ToString();
                    DateTime dt = DateTime.MinValue;
                    if (DateTime.TryParse(tradeDate, out dt))
                    {
                        if (_accountNavLockDetails.ContainsKey(accountName))
                        {
                            if (_accountNavLockDetails[accountName].LockAppliedDate != DateTime.MinValue && (dt - _accountNavLockDetails[accountName].LockAppliedDate).Days < 0)
                            {
                                e.Row.Cells[ReconConstants.COLUMN_NAVLockStatus].Value = ReconConstants.LockStatus_Locked;
                                e.Row.Activation = Activation.NoEdit;
                            }
                            else
                            {
                                e.Row.Cells[ReconConstants.COLUMN_NAVLockStatus].Value = ReconConstants.LockStatus_UnLocked;
                                e.Row.Cells[ReconConstants.COLUMN_NAVLockStatus].Activation = Activation.NoEdit;

                            }
                        }
                    }
                }
                #endregion
                //#region When trade is deleted
                //if (e.Row.Cells.Exists(ReconConstants.COLUMN_TaxLotStatus) && e.Row.Cells[ReconConstants.COLUMN_TaxLotStatus].Value != null
                //    && !string.IsNullOrEmpty(e.Row.Cells[ReconConstants.COLUMN_TaxLotStatus].Value.ToString())
                //    && e.Row.Cells[ReconConstants.COLUMN_TaxLotStatus].Value.ToString() == AmendedTaxLotStatus.Deleted.ToString())
                //{
                //    e.Row.Activation = Activation.NoEdit;
                //}
                //#endregion
                //#endregion

                //#region Fill LoggedIn User
                //if (e.Row.Cells.Exists(ReconConstants.COLUMN_UserLoggedIN))
                //{
                //    e.Row.Cells[ReconConstants.COLUMN_UserLoggedIN].Value = CachedDataManager.GetInstance.LoggedInUser.FirstName + " " + CachedDataManager.GetInstance.LoggedInUser.LastName;
                //}
                //#endregion
                #region Fill LockStatus(For First time only while running recon)
                if (e.Row.Cells.Exists(ReconConstants.COLUMN_LockStatus) && !e.ReInitialize)// _isActiveFromDashboard && e.Row.Cells[ReconConstants.COLUMN_LockStatus].Appearance.Image == null)
                {
                    // e.Row.Cells[ReconConstants.COLUMN_LockStatus].Appearance.Image = Properties.Resources.unlock;
                    e.Row.Cells[ReconConstants.COLUMN_LockStatus].Value = ReconConstants.LockStatus_UnLocked;
                }
                #endregion

                //Modified By: Aman Chauhan 04 Nov 2015
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-11638
                #region Row Coloring-- making this as comment, as it is not required.
                // Added the ForeColor property to the text displayed in the grid:
                // Text will be GREEN in case of exactly matched.
                // Text will be RED in case of failed reconciliation.
                // Text will be YELLOW in every other case.
                //if (e.Row.Cells.Exists("MismatchType"))
                //{
                //   if (e.Row.Cells["MismatchType"].Text == ReconConstants.MismatchType_ExactlyMatched)
                //    {
                //        e.Row.CellAppearance.ForeColor = Color.Green;
                //    }
                //    else if (e.Row.Cells["MismatchType"].Text.ToUpper().Contains("MISMATCH") || e.Row.Cells["MismatchType"].Text.ToUpper().Contains("MISSING"))
                //    {
                //        e.Row.CellAppearance.ForeColor = Color.Red;
                //    }
                //    else
                //    {
                //        e.Row.CellAppearance.ForeColor = Color.Black;
                //    }
                //}
                #endregion
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
                this.grdReconOutput.EndUpdate();
                this.grdReconOutput.ResumeRowSynchronization();
            }

        }
        /// <summary>
        /// Check if the trade being edited is having its accounts locked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdReconOutput_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        {
            try
            {
                if (e.Cell.GetType() == typeof(UltraGridFilterCell))
                {
                    return;
                }
                if (e.Cell.Column.Key != ReconConstants.COLUMN_ClosingStatus && e.Cell.Column.Key != ReconConstants.COLUMN_UserLoggedIN && e.Cell.Column.Key != ReconConstants.COLUMN_LockStatus && e.Cell.Column.Key != ReconConstants.COLUMN_NAVLockStatus)
                {
                    if (!e.Cell.Row.Cells.Exists(ReconConstants.COLUMN_AccountName))
                    {
                        return;
                    }
                    string rowAccountname = e.Cell.Row.Cells[ReconConstants.COLUMN_AccountName].Text;
                    int accountID = CachedDataManager.GetInstance.GetAccountID(rowAccountname);
                    if (!CachedDataManager.GetInstance.isAccountLocked(accountID) && accountID != int.MinValue)
                    {
                        //dont check for account lock when header is checked
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
                        if ((_isAskForRejectedAccounts || !_alreadyPromptedAccountsForLock.Contains(accountID)))
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

                //dont allow user to check the rows whose data is missing in nirvana
                if (e.Cell.Row.Cells.Exists(ReconConstants.MismatchReason)
                   && e.Cell.Row.Cells[ReconConstants.MismatchReason].Value.ToString().Equals(ReconConstants.MismatchReason_DataMissingNirvana))
                {
                    e.Cancel = true;
                }

                if (!e.Cell.Column.Key.Equals(ReconConstants.COLUMN_Checkbox) && !e.Cell.Column.Key.Equals(ReconConstants.COLUMN_LockStatus)
                    && e.Cell.Row.Cells.Exists(ReconConstants.COLUMN_LockStatus)
                 && e.Cell.Row.Cells[ReconConstants.COLUMN_LockStatus].Value != null && e.Cell.Row.Cells[ReconConstants.COLUMN_LockStatus].Value.ToString().Equals(ReconConstants.LockStatus_Locked))
                {
                    //e.Cancel = true;
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
        private void grdReconOutput_AfterHeaderCheckStateChanged(object sender, AfterHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                _isHeaderCheckBoxChecked = false;
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2123
                if (!_isFetchingData)
                {
                    foreach (UltraGridRow row in grdReconOutput.Rows)
                    {
                        if (row.Cells != null && row.Cells.Exists(ReconConstants.COLUMN_NAVLockStatus) && row.Cells[ReconConstants.COLUMN_NAVLockStatus].Value != null &&
                            row.Cells[ReconConstants.COLUMN_NAVLockStatus].Value.ToString().Equals(ReconConstants.LockStatus_Locked))
                        {
                            row.Cells[ReconConstants.COLUMN_Checkbox].SetValue(false, false);
                        }
                    }
                }
                //if (_accountUnlocked.Length > 0)
                //{
                //    if (MessageBox.Show("The ability to make changes to a account can only be granted to one user at a time, would you like to proceed in locking following accounts " + _accountUnlocked.ToString().Substring(0, _accountUnlocked.Length - 1) + "?", "Account Lock", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //    {
                //        List<int> newAccountsToBelocked = new List<int>();
                //        newAccountsToBelocked.AddRange(_accountIDUnlocked);
                //        newAccountsToBelocked.AddRange(CachedDataManager.GetInstance.GetLockedAccounts());
                //        if (ReconUtilities.SetAccountsLockStatus(newAccountsToBelocked))
                //        {
                //            MessageBox.Show("The lock for accounts has been acquired, you may continue.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            //update Locks in cache
                //            CachedDataManager.GetInstance.SetLockedAccounts(newAccountsToBelocked);
                //        }
                //        else
                //        {
                //            MessageBox.Show("Accounts are currently locked by another user, please refer to the Account Lock screen for more information.", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        }
                //    }

                //}

                #region Comments
                //grdReconOutput.BeforeHeaderCheckStateChanged -= new Infragistics.Win.UltraWinGrid.BeforeHeaderCheckStateChangedEventHandler(grdReconOutput_BeforeHeaderCheckStateChanged);
                //grdReconOutput.AfterHeaderCheckStateChanged -= new Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventHandler(grdReconOutput_AfterHeaderCheckStateChanged);

                //CheckState checkState = grdReconOutput.DisplayLayout.Bands[0].Columns["CheckBox"].GetHeaderCheckedState(grdReconOutput.Rows);
                //if (checkState != CheckState.Indeterminate)
                //{
                //    if (checkState != CheckState.Checked)
                //    {
                //        grdReconOutput.DisplayLayout.Bands[0].Columns["CheckBox"].SetHeaderCheckedState(grdReconOutput.Rows, false );
                //       // grdReconOutput.DisplayLayout.Bands[0].Columns["CheckBox"].SetHeaderCheckedState(grdReconOutput.Rows, true);
                //    }
                //    else
                //    {
                //        grdReconOutput.DisplayLayout.Bands[0].Columns["CheckBox"].SetHeaderCheckedState(grdReconOutput.Rows, true);
                //        //grdReconOutput.DisplayLayout.Bands[0].Columns["CheckBox"].SetHeaderCheckedState(grdReconOutput.Rows, false);

                //    }
                //}
                //grdReconOutput.BeforeHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.BeforeHeaderCheckStateChangedEventHandler(grdReconOutput_BeforeHeaderCheckStateChanged);
                //grdReconOutput.AfterHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventHandler(grdReconOutput_AfterHeaderCheckStateChanged);
                #endregion


                // _accountUnlocked = new StringBuilder();
                //_accountIDUnlocked = new List<int>();
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
        private void grdReconOutput_BeforeHeaderCheckStateChanged(object sender, BeforeHeaderCheckStateChangedEventArgs e)
        {
            try
            {
                _isHeaderCheckBoxChecked = true;
                //e.IsCancelable is used to check if user have clicked on header checkbox or it is changed by checking/Unchecking rows.
                if (!_isFetchingData && e.NewCheckState != CheckState.Indeterminate && e.IsCancelable)
                {
                    if (grdReconOutput.DataSource != null)
                    {
                        // DataTable dt = (DataTable)grdReconOutput.DataSource;
                        if (grdReconOutput != null && grdReconOutput.DisplayLayout != null
                            && grdReconOutput.DisplayLayout.Bands != null && grdReconOutput.DisplayLayout.Bands.Count > 0
                            && grdReconOutput.DisplayLayout.Bands[0].Columns.Count > 0
                            && grdReconOutput.DisplayLayout.Bands[0].Columns.Exists(ReconConstants.COLUMN_AccountName))
                        {
                            List<object> lstAccountName = grdReconOutput.Rows.Select(r => r.GetCellValue(ReconConstants.COLUMN_AccountName)).Distinct().ToList();
                            // List<string> lstAccountName = (from DataRow dr in dt.Rows select (string)dr[ReconConstants.COLUMN_AccountName]).Distinct().ToList();
                            List<string> accountUnlocked = new List<string>();
                            List<int> accountIDUnlocked = new List<int>();
                            lstAccountName.ForEach(x =>
                            {
                                int accountID = CachedDataManager.GetInstance.GetAccountID(x.ToString());
                                if (!CachedDataManager.GetInstance.isAccountLocked(accountID) && accountID != int.MinValue)
                                {
                                    accountIDUnlocked.Add(accountID);
                                    accountUnlocked.Add(x.ToString());
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
        void frmViewFile_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (_frmViewFile != null)
                {
                    _frmViewFile = null;
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
        private void grdReconOutput_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                //check the button name
                if (e.Cell.Column.Key == "Amend")
                {

                    _launchForm = ReconPrefManager.GetLaunchForm();
                    if (_launchForm != null)
                    {
                        DataRowView rowView = e.Cell.Row.ListObject as DataRowView;

                        int accountID = int.MinValue;

                        if (rowView.Row.Table.Columns.Contains("AccountName") && !string.IsNullOrWhiteSpace(rowView.Row["AccountName"].ToString()))
                        {
                            accountID = CachedDataManager.GetInstance.GetAccountID(rowView.Row["AccountName"].ToString());
                        }

                        if (accountID != int.MinValue && rowView.Row.Table.Columns.Contains("Symbol") && !string.IsNullOrWhiteSpace(rowView.Row["Symbol"].ToString()))
                        {
                            //CHMW-1620 [Closing] - Add Comments field in PostReconAmendenmtsUI
                            grdReconOutput.ActiveRow = e.Cell.Row;
                            string comment = string.Empty;
                            if (grdReconOutput != null && grdReconOutput.ActiveRow != null && grdReconOutput.ActiveRow.Cells != null
                                && grdReconOutput.ActiveRow.Cells.Exists(ReconConstants.COLUMN_Comments)
                                && grdReconOutput.ActiveRow.Cells[ReconConstants.COLUMN_Comments].Value != DBNull.Value
                               && grdReconOutput.ActiveRow.Cells[ReconConstants.COLUMN_Comments].Value != null)
                            {
                                comment = grdReconOutput.ActiveRow.Cells[ReconConstants.COLUMN_Comments].Value.ToString();
                            }

                            ListEventAargs args = new ListEventAargs();
                            args.listOfValues.Add(ApplicationConstants.CONST_POST_RECON_AMENDMENTS.ToString());
                            args.listOfValues.Add(accountID.ToString());
                            args.listOfValues.Add(rowView.Row["Symbol"].ToString());
                            args.listOfValues.Add(_reconParameters.ToDate);
                            args.listOfValues.Add(comment);
                            args.argsObject2 = _taskResult;
                            args.argsObject = UpdateCommentsFromPostReconAmendments;
                            _launchForm(this, args);
                        }
                        else
                        {
                            MessageBox.Show("Account name or symbol not valid", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                dynamicForm_Fill_Panel.ClientArea.ResumeLayout(false);
                dynamicForm_Fill_Panel.ClientArea.PerformLayout();
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
        // Modified by Ankit Gupta on 15th Sep, 2014
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1448
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1465
        //private void grdReconOutput_ClickCell(object sender, ClickCellEventArgs e)
        //{
        //    try
        //    {
        //        if (grdReconOutput.ActiveRow.Cells[ReconConstants.COLUMN_LockStatus].Value.ToString().Equals(ReconConstants.LockStatus_Locked) ||
        //                                grdReconOutput.ActiveRow.Cells[ReconConstants.COLUMN_NAVLockStatus].Value.ToString().Equals(ReconConstants.LockStatus_Locked))
        //        {
        //            grdReconOutput.ActiveCell = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        // Invoke our policy that is responsible for making sure no secure information
        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private bool ApproveChangesfromDataTable(DataTable dt)
        {
            try
            {
                bool isAutoApproveAllowed = false;
                //read from config if auto approved is allowed or not.
                bool.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_AutoApproveAmendments), out isAutoApproveAllowed);

                // if (AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(ModuleResources.ReconCancelAmend, AuthAction.Approve)&& isAutoApproveAllowed)
                if (CachedDataManagerRecon.GetInstance.GetPermissionLevel() == AuthAction.Approve && isAutoApproveAllowed)
                {
                    Dictionary<string, List<ApprovedChanges>> dictApprovedChanges = getApprovedChanges(dt);
                    if (dictApprovedChanges != null && dictApprovedChanges.Count > 0 || dtMarkPrice.Rows.Count > 0)
                    {
                        #region Save Amendments
                        //As per discussion with Narender this is only required for CHMW so removing call from PRANA.
                        //AllocationManager.GetInstance().MakeNewCancelAmendChanges(dictApprovedChanges);
                        #endregion
                        #region Save taxlot workflow status  - modified by omshiv
                        ConcurrentDictionary<string, NirvanaWorkFlowsStats> dictTaxlotStates = new ConcurrentDictionary<string, NirvanaWorkFlowsStats>();
                        foreach (String taxlot in dictApprovedChanges.Keys)
                        {
                            dictTaxlotStates.TryAdd(taxlot, NirvanaWorkFlowsStats.ReconApproved);
                        }
                        ReconUtilities.SaveTaxLotWorkflowStates(dictTaxlotStates);
                        #endregion
                        #region SaveMarkPrice
                        if (dtMarkPrice.Rows.Count > 0)
                        {
                            _pricingServicesProxy.InnerChannel.SaveMarkPrices(dtMarkPrice, true);
                            dtMarkPrice.Rows.Clear();
                        }
                        #endregion
                        return true;
                    }
                }
                if (dtMarkPrice.Rows.Count > 0)
                {
                    _pricingServicesProxy.InnerChannel.SaveMarkPrices(dtMarkPrice, false);
                    dtMarkPrice.Rows.Clear();
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
            return false;
        }



        private Dictionary<string, List<ApprovedChanges>> getApprovedChanges(DataTable dt)
        {
            Dictionary<string, List<ApprovedChanges>> dictApprovedChanges = new Dictionary<string, List<ApprovedChanges>>();
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    ApprovedChanges approvedChanges = null;
                    List<ApprovedChanges> lstApprovedChanges = new List<ApprovedChanges>();

                    if (dt.Columns.Contains(ReconConstants.COLUMN_ChangedColumns)
                         && !string.IsNullOrEmpty(row[ReconConstants.COLUMN_ChangedColumns].ToString()))
                    {
                        List<string> listColumns = new List<string>();
                        listColumns = row[ReconConstants.COLUMN_ChangedColumns].ToString().Split(Seperators.SEPERATOR_8[0]).ToList();

                        foreach (string column in listColumns)
                        {
                            approvedChanges = new ApprovedChanges();
                            if (dt.Columns.Contains(ReconConstants.CONST_Nirvana + column) && dt.Columns.Contains(ReconConstants.CONST_OriginalValue + column))
                            {
                                approvedChanges.TaxlotID = row[ReconConstants.COLUMN_TaxLotID].ToString();
                                approvedChanges.TaxlotStatus = AmendedTaxLotStatus.ValueChanged;
                                approvedChanges.ColumnName = column;
                                approvedChanges.OldValue = row[ReconConstants.CONST_OriginalValue + column].ToString();
                                approvedChanges.NewValue = row[ReconConstants.CONST_Nirvana + column].ToString();
                                lstApprovedChanges.Add(approvedChanges);
                            }
                        }
                        row[ReconConstants.COLUMN_ChangedColumns] = string.Empty;
                    }
                    else if (dt.Columns.Contains(ReconConstants.COLUMN_TaxLotStatus)
                        && !string.IsNullOrEmpty(row[ReconConstants.COLUMN_TaxLotStatus].ToString())
                        && row[ReconConstants.COLUMN_TaxLotStatus].ToString().Equals(AmendedTaxLotStatus.Deleted.ToString()))
                    {
                        approvedChanges = new ApprovedChanges();
                        approvedChanges.TaxlotID = row[ReconConstants.COLUMN_TaxLotID].ToString();
                        approvedChanges.TaxlotStatus = AmendedTaxLotStatus.Deleted;
                        row[ReconConstants.COLUMN_TaxLotStatus] = string.Empty;
                        lstApprovedChanges.Add(approvedChanges);
                    }
                    else
                    {
                        break;
                    }

                    if (approvedChanges.TaxlotID != null && !dictApprovedChanges.ContainsKey(approvedChanges.TaxlotID))
                    {
                        dictApprovedChanges.Add(approvedChanges.TaxlotID, lstApprovedChanges);
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
            return dictApprovedChanges;
        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgSaveClickWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<object> arguments = e.Argument as List<object>;
                DataTable dt = arguments[0] as DataTable;
                //string reconComments = arguments[1].ToString();
                ReconUtilities.CreateDirectoryIfNotExists(_reconParameters.ReconFilePath);

                #region Sort datatable in ascending order as per amendments made
                //sort table as per modification for approve changes in descending order to get modified rows on top
                string sortingColumnOrder = ReconConstants.COLUMN_ChangedColumns + " Desc," + ReconConstants.COLUMN_TaxLotStatus + " Desc";
                dt = ReconManager.sortDataTable(dt, sortingColumnOrder, null);
                #endregion

                if (ApproveChangesfromDataTable(dt))
                {
                    e.Result = "Amendments approved";
                }
                else
                {
                    e.Result = "Amendments saved";
                }
                XMLUtilities.WriteXML(dt, _reconParameters.ReconFilePath);
                //dt.WriteXml(_filePath);
                _taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Comments", txtUserComments.Text, null);
                _taskResult.TaskStatistics.Status = NirvanaTaskStatus.Completed;
                ReconManager.SetTaskSpecificData(dt, _taskResult);

                ReconUtilities.SaveTaxLotWorkFlowStates(dt);

                #region Amendments File Update

                #region Count Amendments
                //TODO extract this in recon utilities for common use in here and approve changes                         
                int amendmentCount = 0;
                if (dt.Columns.Contains(ReconConstants.COLUMN_ChangedColumns))
                {
                    amendmentCount = (from DataRow row in dt.Rows where !(string.IsNullOrEmpty(row[ReconConstants.COLUMN_ChangedColumns].ToString())) select row).Count();
                }
                if (dt.Columns.Contains(ReconConstants.COLUMN_TaxLotStatus))
                {
                    amendmentCount += (from DataRow row in dt.Rows where !(string.IsNullOrEmpty(row[ReconConstants.COLUMN_TaxLotStatus].ToString())) select row).Count();
                }
                #endregion

                #region check if amendment Dictionary exist else create it
                SerializableDictionary<string, int> amendmends = new SerializableDictionary<string, int>();
                amendmends = ReconUtilities.LoadAmendmentDictionary();
                // if file is already write then update its value else add an entry

                string relativeExceptionFilePath = ReconUtilities.GetRelativeExceptionFilePath(_reconParameters.ReconFilePath);
                if (amendmends.ContainsKey(relativeExceptionFilePath))
                {
                    amendmends[relativeExceptionFilePath] = amendmentCount;
                }
                else
                {
                    amendmends.Add(relativeExceptionFilePath, amendmentCount);
                }
                #endregion

                #region write the amendment File;
                ReconUtilities.WriteAmendmentDictionary(amendmends);
                #endregion

                #region CommentedCode Creating data table instead of dictionary
                //    if (!ds.Tables.Contains("ReconAmendments"))
                //    {
                //        amentments.Columns.Add("FilePath", typeof(string));
                //        amentments.Columns.Add("AmendedRecords", typeof(Int32));
                //    }
                //    else
                //    {
                //        amentments = ds.Tables["ReconAmendments"];
                //    }
                //    int index = (from DataRow row in amentments.Rows where row["FilePath"].ToString() == relativeExceptionFilePath select amentments.Rows.IndexOf(row)).SingleOrDefault();

                //    //if index is not 0|| if index is zero check the value exist at zero or is returned by default
                //    if (index != 0 || amentments.Rows[0]["FilePath"].ToString() == relativeExceptionFilePath)
                //    {
                //        amentments.Rows[index]["AmendedRecords"] = amendmentCount;
                //    }
                //    else
                //    {
                //        //add the row if does not exist
                //        amentments.Rows.Add(relativeExceptionFilePath, amendmentCount);
                //        amentments.WriteXml(amendmentsFilePath);
                //    }
                //}
                //else
                //{
                //    amentments.Columns.Add("FilePath", typeof(string));
                //    amentments.Columns.Add("AmendedRecords", typeof(Int32));

                //    //primary key of the file should be exception file path
                //    DataColumn[] columns = new DataColumn[1];
                //    columns[0] = dt.Columns["FilePath"];
                //    amentments.PrimaryKey = columns;

                //    //add the current row in file
                //    amentments.Rows.Add(relativeExceptionFilePath, amendmentCount);
                //    amentments.TableName = "ReconAmendments";
                //}
                //DataView viewamentments = amentments.DefaultView;
                //if (viewamentments.Table.Columns.Contains("AmendedRecords"))
                //{
                //    viewamentments.Sort = "AmendedRecords";
                //}
                //viewamentments.ToTable().WriteXml(amendmentsFilePath);
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
                _taskResult.Error = ex;
                _taskResult.LogResult();
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
        private void bgSaveClickWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (DisableApproveChanges != null)
                {
                    DisableApproveChanges(this, null);
                }
                //_isExceptionReportGenerated = true;
                if (e.Result != null)
                {
                    if (e.Result.ToString().Equals("Amendments approved"))
                    {
                        DisableAmendments(true, true);
                        UpdateStatusBarText("Amendments approved, please re-run reconciliation to get updated data.");
                    }
                    else if (e.Result.ToString().Equals("Amendments saved"))
                    {
                        UpdateStatusBarText("Amendments Saved");
                    }
                    MessageBox.Show("Amendment Saved", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(e.Result.ToString(), "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Update the text of status bar
        /// </summary>
        /// <param name="status"></param>
        internal void UpdateStatusBarText(string status)
        {
            try
            {

                //grdReconOutput.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                ultraStatusBarReconStatus.Text = status;
                //_isFetchingData = true;
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
        /// disable controls when amendments made from other UI's
        /// </summary>
        //internal void Disable()
        //{
        //    try
        //    {
        //        DisableAmendments(true);
        //        ultraStatusBarReconStatus.Text = "Amendments approved/rescinded, please re-run recon to get updated data.";
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

        internal void DisableAmendments(bool isDisabled, bool isDisableGrid)
        {
            try
            {
                //HardCoded if the recon type is Transaction and grouping is done then don't allow making amendments
                if (isDisabled || !_isAmendmentsAllowed)
                {
                    btnCopyAll.Enabled = false;
                    btnCopyFailedValues.Enabled = false;
                    //btnSave.Enabled = false;
                    btnDelete.Enabled = false;
                    if (isDisableGrid)
                    {
                        grdReconOutput.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                        grdReconOutput.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                    }
                }
                else if (!isDisabled)
                {
                    // Disable updating on the entire grid
                    btnCopyAll.Enabled = true;
                    btnCopyFailedValues.Enabled = true;
                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                    grdReconOutput.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;
                    grdReconOutput.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                    SetUserPermissions();
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

        // <summary>
        /// Update event of disabling other controls if amendments are made
        /// </summary>
        /// <param name="DisableApproveChanges"></param>
        internal void UpdateEvent(EventHandler disableApproveChanges)
        {
            try
            {
                DisableApproveChanges = disableApproveChanges;
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

        private void viewInFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int rowindex = int.MinValue;
                string reconXmlDataDirectoryPath = ReconUtilities.GetReconDirectoryPath(ReconConstants.ReconDataDirectoryPath, _reconParameters);
                string tempPath = reconXmlDataDirectoryPath + "\\xmls\\Transformation\\Temp\\";
                string inputXMLFileName = tempPath + "InputXML" + Seperators.SEPERATOR_6 + _reconParameters.FromDate + Seperators.SEPERATOR_6 + _reconParameters.ToDate + ".xml";

                if (grdReconOutput.ActiveRow != null && grdReconOutput.ActiveRow.Cells != null &&
                       grdReconOutput.ActiveRow.Cells.Exists(ReconConstants.CONST_Broker + ReconConstants.COLUMN_RowIndex) &&
                       grdReconOutput.ActiveRow.Cells[ReconConstants.CONST_Broker + ReconConstants.COLUMN_RowIndex].Value != null &&
                       grdReconOutput.ActiveRow.Cells[ReconConstants.CONST_Broker + ReconConstants.COLUMN_RowIndex].Value != DBNull.Value &&
                       int.TryParse(grdReconOutput.ActiveRow.Cells[ReconConstants.CONST_Broker + ReconConstants.COLUMN_RowIndex].Value.ToString(), out rowindex) &&
                       File.Exists(inputXMLFileName))
                {
                    DataSet ds = new DataSet();
                    //ds.ReadXml(inputXMLFileName);
                    ds = XMLUtilities.ReadXmlUsingBufferedStream(inputXMLFileName);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count >= rowindex)
                    {
                        if (_frmViewFile == null || _frmViewFile.IsDisposed)
                        {
                            grdReport = new UltraGrid();
                            _frmViewFile = new Form();
                            _frmViewFile.ShowIcon = false;
                            _frmViewFile.FormClosed += frmViewFile_FormClosed;
                            // http://jira.nirvanasolutions.com:8080/browse/CHMW-2265
                            // Fix minimum size of the form.
                            _frmViewFile.MinimumSize = new System.Drawing.Size(910, 500);
                            //set the form and grid properties
                            grdReport.DisplayLayout.Bands[0].ColHeadersVisible = false;
                            SetThemeAtDynamicForm(_frmViewFile, grdReport);
                            grdReport.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                            grdReport.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                        }
                        else
                        {
                            //else previous form is bring to front
                            _frmViewFile.BringToFront();
                        }
                        _frmViewFile.Text = string.Empty;
                        if (CachedDataManager.GetUserPermittedCompanyList().ContainsKey(_reconTemplate.ClientID))
                        {
                            _frmViewFile.Text = CachedDataManager.GetUserPermittedCompanyList()[_reconTemplate.ClientID];
                        }
                        _frmViewFile.Text = _frmViewFile.Text + " " + _reconParameters.ReconType + " " + _reconParameters.TemplateName + " " + _reconParameters.FromDate + " " + _reconParameters.ToDate;

                        grdReport.DataSource = ds.Tables[0];
                        DataRow row = ds.Tables[0].Select(ReconConstants.COLUMN_RowIndex + " = '" + rowindex + "'").FirstOrDefault();
                        grdReport.Rows[ds.Tables[0].Rows.IndexOf(row)].CellAppearance.ForeColor = Color.Green;
                        // http://jira.nirvanasolutions.com:8080/browse/CHMW-2265
                        // Delete index columns from PB file when displayed from recon.
                        if (grdReport.DisplayLayout.Bands[0].Columns != null && grdReport.DisplayLayout.Bands[0].Columns.Exists("RowIndex"))
                        {
                            grdReport.DisplayLayout.Bands[0].Columns["RowIndex"].Hidden = true;
                        }
                        if (grdReport.DisplayLayout.Bands[0].Columns != null && grdReport.DisplayLayout.Bands[0].Columns.Exists("RowId"))
                        {
                            grdReport.DisplayLayout.Bands[0].Columns["RowId"].Hidden = true;
                        }
                        CustomThemeHelper.SetThemeProperties(_frmViewFile, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                        _frmViewFile.Show();
                    }
                }
                else
                {
                    MessageBox.Show("File Data Corrupted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void grdReconOutput_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    UltraWinGridUtils.RightClickRowSelect((UltraGrid)sender, e.Location);
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
        /// This method is wired to 'CellChange' event for grdRecon. 
        /// Added by Ankit Gupta on 24 Dec, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-2197
        /// [Recon] User can enter string in Nirvana columns and application error : 'Input string was not in correct format' comes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdReconOutput_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                CancelUpdateInSettlementColumn(e);

                //modified by amit. changes done for http://jira.nirvanasolutions.com:8080/browse/CHMW-3051
                if (!e.Cell.Column.Key.Equals(ReconConstants.COLUMN_Checkbox) && !e.Cell.Column.Key.Equals(ReconConstants.COLUMN_LockStatus)
                   && e.Cell.Row.Cells.Exists(ReconConstants.COLUMN_LockStatus)
                && e.Cell.Row.Cells[ReconConstants.COLUMN_LockStatus].Value != null && e.Cell.Row.Cells[ReconConstants.COLUMN_LockStatus].Value.ToString().Equals(ReconConstants.LockStatus_Locked))
                {
                    e.Cell.Value = e.Cell.OriginalValue;
                    MessageBox.Show("Row Locked", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (!_isFetchingData && _reconTemplate != null && _reconTemplate.RulesList != null && _reconTemplate.RulesList.Count > 0 &&
                        e.Cell.Column.Key.StartsWith("Nirvana", StringComparison.InvariantCultureIgnoreCase))
                {
                    List<string> numericColumns = _reconTemplate.RulesList[0].NumericFields;
                    // List of numeric columns fetched above contains column names without the Prefix 'Nirvana', Example: 'Quantity', 'AvgPX'
                    // But the name of the column returned by 'e' contains the the Prefix 'Nirvana', Example 'NirvanaQuantity', 'NirvanaAvgPX'.
                    // Therefore, for comparison purpose, Replace method is used.                    
                    if (numericColumns.Contains(e.Cell.Column.Key.Replace(ReconConstants.CONST_Nirvana, String.Empty)))
                    {
                        if (String.IsNullOrEmpty(e.Cell.Text))
                        {
                            e.Cell.Value = "0";
                            e.Cell.SelectAll();
                            return;
                        }
                        double i;
                        if (!Double.TryParse(e.Cell.Text, out i))
                        {
                            e.Cell.Value = e.Cell.OriginalValue;
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

        private void CancelUpdateInSettlementColumn(CellEventArgs e)
        {
            try
            {
                if (!e.Cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_CurrencySymbol))
                {
                    if (e.Cell.Column.Key.Equals(ReconConstants.CONST_Nirvana + ReconConstants.CONST_AvgPX))
                    {
                        MessageBox.Show(this, "Could not update 'CurrencySymbol' column missing", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        e.Cell.CancelUpdate();
                    }
                }
                else if (!e.Cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + OrderFields.PROPERTY_SETTLEMENTCURRENCY))
                {
                    if (e.Cell.Column.Key.Equals(ReconConstants.CONST_Nirvana + ReconConstants.CONST_AvgPX))
                    {
                        MessageBox.Show(this, "Could not update 'Settlement Currency' column missing", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        e.Cell.CancelUpdate();
                    }
                }
                else
                {
                    string SettlCurrency = e.Cell.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_CurrencySymbol].Text;
                    string tradeCurrency = e.Cell.Row.Cells[ReconConstants.CONST_Nirvana + OrderFields.PROPERTY_SETTLEMENTCURRENCY].Text;
                    if (tradeCurrency != SettlCurrency)
                    {
                        //Average Price is to be auto calculated
                        //PRANA-9121 Avg price should be calculated using the settlement fix rate and settlement amount
                        if (e.Cell.Column.Key.Equals(ReconConstants.CONST_Nirvana + ReconConstants.CONST_AvgPX))
                        {
                            if (SettlementCachePreferences.SettlementAutoCalculateField == SettlementAutoCalculateField.AveragePrice)
                            {
                                MessageBox.Show(this, "This is a auto calculate field and will update on change in dependent column", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                e.Cell.CancelUpdate();
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
        }
        private void UpdateMarkPriceForSymbolandAccountCombination(Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                //grdReconOutput.AfterCellUpdate += new CellEventHandler(this.grdReconOutput_AfterCellUpdate);
                foreach (UltraGridRow row in grdReconOutput.Rows)
                {
                    if (row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString() == e.Cell.Row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString() && row.Cells[ReconConstants.COLUMN_Symbol].Value.ToString() == e.Cell.Row.Cells[ReconConstants.COLUMN_Symbol].Value.ToString())
                    {
                        //if (!row.Cells["NirvanaMarkPrice"].Value.Equals(e.Cell.Row.Cells["NirvanaMarkPrice"].Text.ToString()))
                        {
                            row.Cells["NirvanaMarkPrice"].Value = Convert.ToDouble(e.Cell.Row.Cells["NirvanaMarkPrice"].Text.ToString());
                            UpdateGridCells(row.Cells["NirvanaMarkPrice"], "NirvanaMarkPrice");
                            if (e.Cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_Quantity)
                                && e.Cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.CONST_Multiplier)
                                && e.Cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.CONST_SideMultiplier)
                                && e.Cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue))
                            {
                                //[Position Recon] On updating mark price, dependent field Nirvana MarketValue doesn't take into account side multiplier
                                double quantity = Math.Abs(Convert.ToDouble(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_Quantity].Value));
                                double markPrice = Convert.ToDouble(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_MARKPRICE].Value.ToString());
                                double multiplier = Convert.ToDouble(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_Multiplier].Value);
                                double sideMultiplier = Convert.ToDouble(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_SideMultiplier].Value);
                                double marketValue = Convert.ToDouble(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue].Value);
                                marketValue = Math.Round((quantity * multiplier * sideMultiplier * markPrice), 4);
                                UpdateGridCells(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue], ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue);
                                if (marketValue != Convert.ToDouble(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue].Value) && !double.IsNaN(marketValue))
                                {
                                    row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue].Value = marketValue;

                                    if (row.Cells.Exists(ReconConstants.CONST_Diff + ReconConstants.CONST_MarketValue) && row.Cells.Exists(ReconConstants.CONST_Broker + ReconConstants.CONST_MarketValue))
                                    {
                                        row.Cells[ReconConstants.CONST_Diff + ReconConstants.CONST_MarketValue].Value = marketValue -
                                            Convert.ToDouble(row.Cells[ReconConstants.CONST_Broker + ReconConstants.CONST_MarketValue].Value);
                                    }
                                    if (row.Cells.Exists(ReconConstants.CONST_Diff + ReconConstants.CONST_MARKPRICE) && row.Cells.Exists(ReconConstants.CONST_Broker + ReconConstants.CONST_MARKPRICE))
                                    {
                                        row.Cells[ReconConstants.CONST_Diff + ReconConstants.CONST_MARKPRICE].Value = markPrice -
                                            Convert.ToDouble(row.Cells[ReconConstants.CONST_Broker + ReconConstants.CONST_MARKPRICE].Value);
                                    }
                                }
                                //grdReconOutput.AfterCellUpdate -= new CellEventHandler(this.grdReconOutput_AfterCellUpdate);

                            }
                            if (row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue))
                            {
                                FillStatus(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue]);
                            }

                            FillStatus(row.Cells["NirvanaMarkPrice"]);
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
        //private void UpdateMarkPriceForSymbolandAccountCombination(Infragistics.Win.UltraWinGrid.CellEventArgs e)
        //{
        //    try
        //    {
        //        //grdReconOutput.AfterCellUpdate += new CellEventHandler(this.grdReconOutput_AfterCellUpdate);
        //        foreach (UltraGridRow row in grdReconOutput.Rows)
        //        {
        //            if (row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString() == e.Cell.Row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString() && row.Cells[ReconConstants.COLUMN_Symbol].Value.ToString() == e.Cell.Row.Cells[ReconConstants.COLUMN_Symbol].Value.ToString())
        //            {
        //                if (!row.Cells["NirvanaMarkPrice"].Value.Equals(e.Cell.Row.Cells["NirvanaMarkPrice"].Text.ToString()))
        //                {
        //                    row.Cells["NirvanaMarkPrice"].Value = Convert.ToDouble(e.Cell.Row.Cells["NirvanaMarkPrice"].Text.ToString());
        //                    UpdateGridCells(row.Cells["NirvanaMarkPrice"], "NirvanaMarkPrice");
        //                    if (e.Cell.Row.Cells.Exists(ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue))
        //                    {
        //                        double originalMarkPrice = Convert.ToDouble(row.Cells[ReconConstants.CONST_OriginalValue + ReconConstants.CONST_MARKPRICE].Value.ToString());
        //                        double markPrice = Convert.ToDouble(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_MARKPRICE].Value.ToString());
        //                        double marketValue = Convert.ToDouble(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue].Value);
        //                        marketValue = Math.Round((marketValue * markPrice / originalMarkPrice), 4);
        //                        if (marketValue != Convert.ToDouble(row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue].Value) && !double.IsNaN(marketValue))
        //                        {
        //                            row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.CONST_MarketValue].Value = marketValue;

        //                            if (row.Cells.Exists(ReconConstants.CONST_Diff + ReconConstants.CONST_MarketValue) && row.Cells.Exists(ReconConstants.CONST_Broker + ReconConstants.CONST_MarketValue))
        //                            {
        //                                row.Cells[ReconConstants.CONST_Diff + ReconConstants.CONST_MarketValue].Value = marketValue -
        //                                    Convert.ToDouble(row.Cells[ReconConstants.CONST_Broker + ReconConstants.CONST_MarketValue].Value);
        //                            }
        //                            if (row.Cells.Exists(ReconConstants.CONST_Diff + ReconConstants.CONST_MARKPRICE) && row.Cells.Exists(ReconConstants.CONST_Broker + ReconConstants.CONST_MARKPRICE))
        //                            {
        //                                row.Cells[ReconConstants.CONST_Diff + ReconConstants.CONST_MARKPRICE].Value = markPrice -
        //                                    Convert.ToDouble(row.Cells[ReconConstants.CONST_Broker + ReconConstants.CONST_MARKPRICE].Value);
        //                            }
        //                        }
        //                        //grdReconOutput.AfterCellUpdate -= new CellEventHandler(this.grdReconOutput_AfterCellUpdate);

        //                    }
        //                    FillStautus(row.Cells["NirvanaMarkPrice"]);
        //                }
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

        internal void SetUserComment(string comment)
        {
            try
            {
                txtUserComments.Text = comment;
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

        private void saveLayoutToTemplate_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (grdReconOutput != null && grdReconOutput.DisplayLayout.Bands[0].Columns.Count > 0)
                {
                    _reconOutputLayout.reconOutputGridColumns = GetGridColumnLayout(grdReconOutput);
                }
                SaveReconLayout();
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
        /// Added By Faisal Shah
        /// Purpose to Save Layout of  grid in UserID Foler in Prana Preferences
        /// </summary>
        private void SaveReconLayout()
        {
            try
            {

                using (XmlTextWriter writer = new XmlTextWriter(_reconOutputLayoutFilePath, Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    XmlSerializer serializer;
                    serializer = new XmlSerializer(typeof(ReconOutputLayout));
                    serializer.Serialize(writer, _reconOutputLayout);

                    writer.Flush();
                }
            }
            #region catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
        }

        /// <summary>
        /// Added By Faisal Shah
        /// Purpose to Get the Layout details before Saving
        /// </summary>
        /// <param name="grdArchiveDashBoard"></param>
        /// <returns></returns>
        private List<ColumnData> GetGridColumnLayout(UltraGrid grdArchiveDashBoard)
        {
            List<ColumnData> listGridCols = new List<ColumnData>();
            UltraGridBand band = grdArchiveDashBoard.DisplayLayout.Bands[0];
            try
            {
                foreach (UltraGridColumn gridCol in band.Columns)
                {
                    ColumnData colData = new ColumnData();
                    colData.Key = gridCol.Key;
                    colData.Caption = gridCol.Header.Caption;
                    colData.Format = gridCol.Format;
                    colData.Hidden = gridCol.Hidden;
                    colData.VisiblePosition = gridCol.Header.VisiblePosition;
                    colData.Width = gridCol.Width;
                    colData.ExcludeFromColumnChooser = gridCol.ExcludeFromColumnChooser;
                    colData.IsGroupByColumn = gridCol.IsGroupByColumn;
                    colData.Fixed = gridCol.Header.Fixed;
                    //colData.CellActivation = gridCol.CellActivation;
                    colData.ColumnStyle = gridCol.Style;
                    //modified by amit. changes done for http://jira.nirvanasolutions.com:8080/browse/CHMW-3569
                    colData.MaskInput = gridCol.MaskInput;
                    colData.NullText = gridCol.NullText;
                    colData.ButtonDisplayStyle = gridCol.ButtonDisplayStyle;
                    if (gridCol.Group != null)
                    {
                        colData.GroupHeaderName = gridCol.Group.Key;
                    }

                    // Sorted Columns
                    colData.SortIndicator = gridCol.SortIndicator;

                    //// Summary Settings
                    //if (band.Summaries.Exists(gridCol.Key))
                    //{
                    //    string colSummKey = band.Summaries[gridCol.Key].CustomSummaryCalculator.ToString();
                    //    colData.ColSummaryKey = (colSummKey.Contains(".")) ? colSummKey.Split('.')[2] : String.Empty;
                    //    colData.ColSummaryFormat = band.Summaries[gridCol.Key].DisplayFormat;
                    //}

                    //Filter Settings
                    foreach (FilterCondition fCond in band.ColumnFilters[gridCol.Key].FilterConditions)
                    {
                        colData.FilterConditionList.Add(fCond);
                    }
                    colData.FilterLogicalOperator = band.ColumnFilters[gridCol.Key].LogicalOperator;

                    listGridCols.Add(colData);
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return listGridCols;
        }
        /// <summary>
        /// Added By Faisal Shah
        /// Purpose to Load Layout from the File
        /// </summary>
        /// <returns></returns>
        private ReconOutputLayout GetReconOutputLayout()
        {
            ReconOutputLayout reconLayout = new ReconOutputLayout();
            try
            {

                _reconOutputLayoutDirectoryPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + ReconConstants.RECONLAYOUT;

                _reconOutputLayoutFilePath = _reconOutputLayoutDirectoryPath + @"\" + _reconTemplate.TemplateKey;

                if (!Directory.Exists(_reconOutputLayoutDirectoryPath))
                {
                    Directory.CreateDirectory(_reconOutputLayoutDirectoryPath);
                }
                if (File.Exists(_reconOutputLayoutFilePath))
                {
                    using (FileStream fs = File.OpenRead(_reconOutputLayoutFilePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ReconOutputLayout));
                        reconLayout = (ReconOutputLayout)serializer.Deserialize(fs);
                    }
                }

                _reconOutputLayout = reconLayout;
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

            return reconLayout;
        }
        internal static bool SetGridColumnLayout(UltraGrid grid, List<ColumnData> listColData)
        {
            bool isColumnsNeedToBeAssignedInGroups = false;
            List<ColumnData> listSortedGridCols = new List<ColumnData>();
            UltraGridBand band = grid.DisplayLayout.Bands[0];
            ColumnsCollection gridColumns = band.Columns;// Just for readability ;)
            listColData.Sort();
            try
            {
                // Hide All
                List<string> allColumn = new List<string>();
                foreach (UltraGridColumn gridCol in gridColumns)
                {
                    allColumn.Add(gridCol.Key);
                    gridCol.Hidden = true;
                }

                //Set Columns Properties
                foreach (ColumnData colData in listColData)
                {
                    allColumn.Remove(colData.Key.ToString());
                    if (gridColumns.Exists(colData.Key))
                    {
                        UltraGridColumn gridCol = gridColumns[colData.Key];
                        gridCol.Width = colData.Width;
                        gridCol.Format = colData.Format;
                        gridCol.Header.Caption = colData.Caption;
                        gridCol.Header.VisiblePosition = colData.VisiblePosition;
                        gridCol.Hidden = colData.Hidden;
                        //modified by amit. changes done for http://jira.nirvanasolutions.com:8080/browse/CHMW-3569
                        gridCol.Style = colData.ColumnStyle;
                        gridCol.MaskInput = colData.MaskInput;
                        //gridCol.ExcludeFromColumnChooser = colData.ExcludeFromColumnChooser;
                        gridCol.Header.Fixed = colData.Fixed;
                        gridCol.SortIndicator = colData.SortIndicator;
                        //gridCol.CellActivation = Activation.NoEdit;
                        if (grid.DisplayLayout.Bands[0].Groups.Exists(colData.GroupHeaderName))
                        {
                            gridCol.Group = band.Groups[colData.GroupHeaderName];
                        }
                        // Sorted Columns
                        if (colData.SortIndicator == SortIndicator.Descending || colData.SortIndicator == SortIndicator.Ascending)
                        {
                            listSortedGridCols.Add(colData);
                        }

                        //Summary Settings
                        //if (colData.ColSummaryKey != String.Empty)
                        //{
                        //    SummarySettings summary = band.Summaries.Add(gridCol.Key, SummaryType.Custom, riskSummFactory.GetSummaryCalculator(colData.ColSummaryKey), gridCol, SummaryPosition.UseSummaryPositionColumn, gridCol);
                        //    summary.DisplayFormat = colData.ColSummaryFormat;
                        //}

                        // Filter Settings
                        if (colData.FilterConditionList.Count > 0)
                        {
                            band.ColumnFilters[colData.Key].LogicalOperator = colData.FilterLogicalOperator;
                            foreach (FilterCondition fCond in colData.FilterConditionList)
                            {
                                band.ColumnFilters[colData.Key].FilterConditions.Add(fCond);
                            }
                        }
                    }
                }
                foreach (string key in allColumn)
                {
                    gridColumns[key].Hidden = false;

                }
                if (allColumn.Count > 0)
                { isColumnsNeedToBeAssignedInGroups = true; }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return isColumnsNeedToBeAssignedInGroups;
        }
        /// <summary>
        /// Added BY Faisal Shah
        /// Updates MarkPrice DataTable to be saved in DataBase
        /// </summary>
        /// <param name="taxlot"></param>
        private void UpdateMarkPriceChangesToDataTable(Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                if (dtMarkPrice.TableName == string.Empty)
                {
                    SetDataTableSchema();
                }
                bool isAutoApproveAllowed = false;
                //read from config if auto approved is allowed or not.
                bool.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_AutoApproveAmendments), out isAutoApproveAllowed);
                DateTime date = new DateTime();
                bool isDateParsed = DateTime.TryParse(_reconParameters.DTToDate.ToString(), out date);
                if (isDateParsed)
                {
                    double markPrice;
                    double.TryParse(e.Cell.Row.Cells["NirvanaMarkPrice"].Value.ToString(), out markPrice);
                    //only non-zero mark prices will be saved.  
                    int accountID = CachedDataManager.GetInstance.GetAccountID(e.Cell.Row.Cells[ReconConstants.COLUMN_AccountName].Value.ToString());
                    if (dtMarkPrice.Rows.Count > 0)
                    {
                        foreach (DataRow dRow in dtMarkPrice.Rows)
                        {

                            if (Convert.ToDateTime(dRow["Date"]) == Convert.ToDateTime(_reconParameters.DTToDate.ToString()) && dRow["Symbol"].ToString().ToUpper() == e.Cell.Row.Cells[ReconConstants.COLUMN_Symbol].Value.ToString().ToUpper() && dRow["FundID"].ToString() == accountID.ToString())
                            {
                                return;
                            }
                        }
                    }
                    if (markPrice != 0)
                    {
                        DataRow drNew = dtMarkPrice.NewRow();
                        if (isAutoApproveAllowed)
                        {
                            drNew["IsApproved"] = 1;
                        }
                        else
                        {
                            drNew["IsApproved"] = 0;
                        }
                        drNew["Date"] = Convert.ToDateTime(_reconParameters.DTToDate.ToString());
                        drNew["MarkPrice"] = markPrice;//  dr[dc.ColumnName];
                        // this column value has been fixed to differentiate whether data save into the DB from Import module or Mark price UI
                        // L stands for Live feed Data
                        drNew["MarkPriceImportType"] = Prana.BusinessObjects.AppConstants.MarkPriceImportType.P.ToString();

                        drNew["Symbol"] = e.Cell.Row.Cells[ReconConstants.COLUMN_Symbol].Value.ToString().ToUpper();
                        drNew["FundID"] = accountID.ToString();
                        drNew["Source"] = (int)Enum.Parse(typeof(PricingSource), PricingSource.UserDefined.ToString());
                        drNew["ForwardPoints"] = 0;
                        //drNew["AUECID"] = taxlot.AUECID.ToString().ToUpper();
                        //drNew["AUECIdentifier"] = taxlot.Iden.ToString();
                        dtMarkPrice.Rows.Add(drNew);
                        dtMarkPrice.AcceptChanges();
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
        /// Set Schema For Datable to Save Mark Prices
        /// </summary>
        private void SetDataTableSchema()
        {
            try
            {
                dtMarkPrice.TableName = "MarkPriceImport";
                dtMarkPrice.Columns.Add(new DataColumn("Symbol"));
                dtMarkPrice.Columns.Add(new DataColumn("Date"));
                dtMarkPrice.Columns.Add(new DataColumn("MarkPrice"));
                dtMarkPrice.Columns.Add(new DataColumn("MarkPriceImportType"));
                dtMarkPrice.Columns.Add(new DataColumn("ForwardPoints"));
                dtMarkPrice.Columns.Add(new DataColumn("IsApproved"));

                //Added AUECID as it will be used at pricing server end to update cache
                dtMarkPrice.Columns.Add(new DataColumn("AUECID"));
                dtMarkPrice.Columns.Add(new DataColumn("AUECIdentifier"));
                dtMarkPrice.Columns.Add(new DataColumn("FundID"));
                dtMarkPrice.Columns.Add(new DataColumn("Source"));
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

        // Sorted Columns are returned as they need to be handled after data is binded.
        //  return listSortedGridCols;

        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            throw new NotImplementedException();
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            throw new NotImplementedException();
        }

        public void LiveFeedDisConnected()
        {
            throw new NotImplementedException();
        }

        private void CreatePricingProxy()
        {
            try
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

        private void ctrlReconOutput_Load(object sender, EventArgs e)
        {
            try
            {
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
                btnCopyFailedValues.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnCopyFailedValues.ForeColor = System.Drawing.Color.White;
                btnCopyFailedValues.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCopyFailedValues.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCopyFailedValues.UseAppStyling = false;
                btnCopyFailedValues.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCopyAll.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnCopyAll.ForeColor = System.Drawing.Color.White;
                btnCopyAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCopyAll.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCopyAll.UseAppStyling = false;
                btnCopyAll.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnDelete.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnDelete.ForeColor = System.Drawing.Color.White;
                btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDelete.UseAppStyling = false;
                btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// Moved Code from Initialize Row to improve the Recon Process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdReconOutput_BeforeRowActivate(object sender, RowEventArgs e)
        {

            try
            {

                //int fundID = int.MinValue;
                //if (grdReconOutput.DisplayLayout.Bands[0].Columns.Exists("FundID") && e.Row.Cells.Exists("FundID") && !string.IsNullOrEmpty(e.Row.Cells["FundID"].Value.ToString()))
                //    Int32.TryParse(e.Row.Cells["FundID"].Value.ToString(), out fundID);
                #region Disable row editing
                #region When Data is missing in nirvana
                if (e.Row.Cells != null)
                {
                    if (e.Row.Cells.Exists("MismatchDetails") && e.Row.Cells["MismatchDetails"].Text.Contains(ReconConstants.MismatchReason_DataMissingNirvana))
                    {
                        e.Row.Activation = Activation.NoEdit;
                    }

                    #endregion

                    #region When trade is partially closed or closed
                    if (e.Row.Cells.Exists(ReconConstants.COLUMN_TaxLotID) && e.Row.Cells[ReconConstants.COLUMN_TaxLotID].Value != null)
                    {
                        if (e.Row.Cells.Exists(ReconConstants.COLUMN_ClosingStatus) && _closingStatus.Keys.Contains(e.Row.Cells[ReconConstants.COLUMN_TaxLotID].Value.ToString()))
                        {
                            //e.Row.Cells[ReconConstants.COLUMN_ClosingStatus].Value = _dictClosingStatus[e.Row.Cells[ReconConstants.COLUMN_TaxLotID].Value.ToString()];
                            //disable editing if the closing status is not open
                            if (e.Row.Cells[ReconConstants.COLUMN_ClosingStatus].Value != null
                                && !e.Row.Cells[ReconConstants.COLUMN_ClosingStatus].Value.ToString().Equals(ClosingStatus.Open.ToString())
                                && _reconTemplate.ReconType != ReconType.Position && _reconTemplate.ReconType != ReconType.TaxLot)
                            {
                                e.Row.Activation = Activation.NoEdit;
                            }
                        }
                    }
                    #endregion
                    //#region When nav for fund is Locked(Also Fills Nav lock Status)
                    //if (e.Row.Cells.Exists(ReconConstants.COLUMN_NAVLockStatus) && fundID != int.MinValue)
                    //{
                    //    string tradeDate = e.Row.Cells[ReconConstants.CONST_Nirvana + ReconConstants.COLUMN_TradeDate].Value.ToString();
                    //    DateTime dt = DateTime.MinValue;
                    //    Boolean isValidTradeDate = DateTime.TryParse(tradeDate, out dt);
                    //    if (isValidTradeDate)
                    //    {
                    //        bool isTradeAllowed = Prana.ClientCommon.NAVLockManager.GetInstance.ValidateTrade(fundID, dt);
                    //        if (isTradeAllowed)
                    //        {
                    //            e.Row.Cells[ReconConstants.COLUMN_NAVLockStatus].Value = ReconConstants.LockStatus_UnLocked;
                    //            e.Row.Cells[ReconConstants.COLUMN_NAVLockStatus].Activation = Activation.NoEdit;
                    //        }
                    //        else
                    //        {
                    //            e.Row.Cells[ReconConstants.COLUMN_NAVLockStatus].Value = ReconConstants.LockStatus_Locked;
                    //            e.Row.Activation = Activation.NoEdit;
                    //        }
                    //    }
                    //}
                    //#endregion
                    #region When trade is deleted
                    if (e.Row.Cells.Exists(ReconConstants.COLUMN_TaxLotStatus) && e.Row.Cells[ReconConstants.COLUMN_TaxLotStatus].Value != null
                        && !string.IsNullOrEmpty(e.Row.Cells[ReconConstants.COLUMN_TaxLotStatus].Value.ToString())
                        && e.Row.Cells[ReconConstants.COLUMN_TaxLotStatus].Value.ToString() == AmendedTaxLotStatus.Deleted.ToString())
                    {
                        e.Row.Activation = Activation.NoEdit;
                    }
                }
                #endregion
                #endregion
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
        //modified by amit.changes done for http://jira.nirvanasolutions.com:8080/browse/CHMW-3569
        private void grdReconOutput_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            if (this.grdReconOutput.ActiveCell.Column.Key.Equals("NirvanaTradeDate"))
            {
                bool isValidTradeDate = false;
                DateTime dt = DateTime.MinValue;
                isValidTradeDate = DateTime.TryParse(this.grdReconOutput.ActiveCell.Text.ToString(), out dt);

                if (!isValidTradeDate)
                {
                    MessageBox.Show("Invalid trade date.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.grdReconOutput.ActiveCell.CancelUpdate();
                    return;
                }
            }
        }

        private void grdReconOutput_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            (e.CustomRowFiltersDialog as Form).PaintDynamicForm();
        }

        private void grdReconOutput_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                (this.FindForm()).AddCustomColumnChooser(this.grdReconOutput);
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
