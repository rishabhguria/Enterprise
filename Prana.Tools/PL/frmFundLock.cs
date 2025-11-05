using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Enums;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class frmAccountLock : Form, IPluggableTools//, ILaunchForm
    {

        Dictionary<int, int> _dictAccountUser = new Dictionary<int, int>();
        string _currentUser = string.Empty;
        ProxyBase<IPranaPositionServices> _pranaPositionServices = null;

        /// <summary>
        /// modified by: sachin mishra,02 Feb 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public frmAccountLock()
        {

            try
            {
                InitializeComponent();
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
        /// Load Theme On Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAccountLock_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    _currentUser = CachedDataManager.GetInstance.LoggedInUser.ShortName;
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);


                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1267
                    // Modified by Ankit Gupta on 22 aug, 2014
                    // According to the permission level of the user, appropriate message is displayed on the status bar.
                    // In case of Read only permission, ultra grid is disabled for the user.

                    btnRefresh_Click(null, null);
                    if (AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(ModuleResources.AccountLock, AuthAction.Write))
                    {
                        grdAccountLock.Enabled = true;
                        ultraStatusBar1.Text = "Authorization Mode : Write";
                    }
                    else if (AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(ModuleResources.AccountLock, AuthAction.Read))
                    {
                        grdAccountLock.Enabled = false;
                        ultraStatusBar1.Text = "Authorization Mode : Read only";
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
                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRefresh.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnRefresh.ForeColor = System.Drawing.Color.White;
                btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRefresh.UseAppStyling = false;
                btnRefresh.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// Create Position Services Proxyto connect with server
        /// </summary>
        public void CreatePositionServicesProxy()
        {
            try
            {
                if (_pranaPositionServices == null)
                {
                    _pranaPositionServices = new ProxyBase<IPranaPositionServices>("TradePositionServiceEndpointAddress");
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
        /// gets the User-Account lock data from server
        /// </summary>
        public void SetDataSource()
        {

            try
            {
                DataTable dtAccountUser = new DataTable();
                if (!dtAccountUser.Columns.Contains("FundID"))
                {
                    dtAccountUser.Columns.Add("FundID", typeof(string));
                }
                if (!dtAccountUser.Columns.Contains("FundName"))
                {
                    dtAccountUser.Columns.Add("FundName", typeof(string));
                }
                if (!dtAccountUser.Columns.Contains("UserID"))
                {
                    dtAccountUser.Columns.Add("UserID", typeof(string));
                }
                if (!dtAccountUser.Columns.Contains("UserName"))
                {
                    dtAccountUser.Columns.Add("UserName", typeof(string));
                }
                if (!dtAccountUser.Columns.Contains("LockStatus"))
                {
                    dtAccountUser.Columns.Add("LockStatus", typeof(string));
                }

                dtAccountUser.Rows.Clear();
                _dictAccountUser.Clear();

                AccountCollection accountcollection = CachedDataManager.GetInstance.GetUserAccounts();

                if (accountcollection == null)
                    return;
                Dictionary<int, string> dictUsers = CachedDataManager.GetInstance.GetAllUsersName();//.GetUserPermittedCompanyList();

                if (_pranaPositionServices == null)
                {
                    CreatePositionServicesProxy();
                }
                //gets data from server
                _dictAccountUser = _pranaPositionServices.InnerChannel.GetAccountsLockStatus();


                //data to be stored for permitted Accounts for the user only
                foreach (Prana.BusinessObjects.Account account in accountcollection)
                {
                    if (account.AccountID != int.MinValue)
                    {
                        //add if the account does not exist in dictionary returned by server
                        if (!_dictAccountUser.ContainsKey(account.AccountID))
                        {
                            dtAccountUser.Rows.Add(account.AccountID, account.FullName, "N/A", string.Empty, "Unlocked");
                        }
                        else
                        {
                            //modofied by amit 31.03.2015
                            //http://jira.nirvanasolutions.com:8080/browse/CHMW-3155
                            if (dictUsers.ContainsKey(_dictAccountUser[account.AccountID]))
                            {
                                dtAccountUser.Rows.Add(account.AccountID, account.FullName, _dictAccountUser[account.AccountID], dictUsers[_dictAccountUser[account.AccountID]], "Locked");
                            }
                            else
                            {
                                dtAccountUser.Rows.Add(account.AccountID, account.FullName, _dictAccountUser[account.AccountID], "Unknown", "Locked");
                            }
                        }
                    }
                }

                //grdAccountLock = new UltraGrid();
                grdAccountLock.DataSource = dtAccountUser;
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
        /// sets the accounts to be locked on server and returns the respose
        /// </summary>
        /// <param name="accountsToBeLocked"></param>
        /// <returns></returns>
        public bool SetAccountsLockStatus(List<int> accountsToBeLocked)
        {
            bool isSucessuful = false;
            try
            {
                //GetUserPermittedCompanyList
                int userID = CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                if (_pranaPositionServices == null)
                {
                    CreatePositionServicesProxy();
                }
                isSucessuful = _pranaPositionServices.InnerChannel.SetAccountsLockStatus(userID, accountsToBeLocked);
                //update dictionary if successfull
                if (isSucessuful)
                {
                    CachedDataManager.GetInstance.UpdateAccountLockData(accountsToBeLocked);
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
            return isSucessuful;
        }

        /// <summary>
        /// Refreshes the Contol on the UI as per the updated cache from server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                //GroupByBox g = grdAccountLock.DisplayLayout.GroupByBox;
                // grdAccountLock = new UltraGrid();
                //set the selected row back to previous selected row.
                int selectedRowIndex = int.MinValue;
                // List<string> grouppedColumns = new List<string>();
                // foreach (object b in grdAccountLock.DisplayLayout.Bands[0].SortedColumns)
                // {
                //grouppedColumns.Add(b
                // }
                // Purpose : To retain sorting after lock/Unlock actions.
                //grdAccountLock.DisplayLayout.Bands[0].SortedColumns.Clear();
                if (grdAccountLock.ActiveRow != null)
                {
                    selectedRowIndex = grdAccountLock.ActiveRow.Index;
                }
                SetDataSource();
                setToggelButtonInGrid();
                //Added by Faisal Shah 24/07/14
                //Purpose To fill Blank Usernames with N/A
                //SetBlankUserNames();
                //frmAccountLock_Load(null, null);

                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1305
                // Modified by Ankit Gupta 22 aug, 2014

                if (selectedRowIndex != int.MinValue && selectedRowIndex != -1)
                {
                    grdAccountLock.Rows[selectedRowIndex].Activated = true;
                }
                // grdAccountLock.DisplayLayout.GroupByBox = g;
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
        ///modified by: sachin mishra 02 Feb 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void setToggelButtonInGrid()
        {
            try
            {
                foreach (UltraGridRow row in grdAccountLock.Rows)
                {
                    UltraCheckEditor uceLockStatus = new UltraCheckEditor();
                    uceLockStatus.Style = Infragistics.Win.EditCheckStyle.Button;
                    uceLockStatus.ThreeState = false;
                    if (row.Cells["LockStatus"].Text == "Unlocked")
                    {
                        uceLockStatus.CheckState = CheckState.Unchecked;
                        uceLockStatus.Text = "Unlocked";
                    }
                    else
                    {
                        uceLockStatus.CheckState = CheckState.Checked;
                        uceLockStatus.Text = "Locked";
                        if (row.Cells["UserName"].Value.ToString() != _currentUser)
                        {

                            row.Cells["LockStatus"].Activation = Activation.Disabled;
                        }
                    }
                    row.Cells["LockStatus"].EditorComponent = uceLockStatus;

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
        /// save the accounts checked in the UI as locked in server cache and Client's Global Cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> lockedAccounts = new List<int>();

                foreach (UltraGridRow row in grdAccountLock.Rows)
                {
                    UltraCheckEditor uceLockStatus = row.Cells["LockStatus"].EditorComponent as UltraCheckEditor;
                    if (uceLockStatus.Text == "Locked" && row.Cells["LockStatus"].Activation != Activation.Disabled)
                    {
                        lockedAccounts.Add(Convert.ToInt32((row.Cells["AccountID"].Value)));
                    }
                }
                bool isSucessuful = SetAccountsLockStatus(lockedAccounts);
                if (!isSucessuful)
                {
                    MessageBox.Show("Lock on account already acquired by another user", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //else
                //{
                //    if (lockedAccounts.Count > 0)
                //    {
                //        MessageBox.Show(lockedAccounts.Count + " Locks Acquired", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //    else
                //    {
                //        MessageBox.Show("All Locks Released", "Account Lock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }

                //}
                btnRefresh_Click(null, null);
                //Added by Faisal Shah 24/07/14
                //Purpose To fill Blank Usernames with N/A
                //SetBlankUserNames();

                //lockedAccounts = CachedDataManager.GetInstance.GetLockedAccounts();
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
        /// //Added by Faisal Shah 24/07/14
        //Purpose To fill Blank Usernames with N/A
        /// </summary>
        //private void SetBlankUserNames()
        //{
        //    try
        //    {
        //        if (grdAccountLock.DisplayLayout.Bands[0].Columns.Exists("UserName"))
        //        {
        //            foreach (UltraGridRow dRow in grdAccountLock.Rows)
        //            {
        //                if (string.IsNullOrEmpty(dRow.Cells["UserName"].Value.ToString()))
        //                {
        //                    dRow.Cells["UserName"].Value = "N/A";
        //                    //dRow.Cells["UserName"].Text = "N/A";
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

        #region ILaunchForm Members

        //public event EventHandler LaunchForm;

        #endregion

        #region IPluggableTools Members

        public Form Reference()
        {
            return this;
        }

        public event EventHandler PluggableToolsClosed;

        ISecurityMasterServices _secMaster = null;

        public ISecurityMasterServices SecurityMaster
        {
            set
            {
                _secMaster = value;
                NewUtilities.SecurityMaster = _secMaster;
                Prana.ReconciliationNew.SecMasterHelper.SecurityMaster = _secMaster;
            }
        }

        public void SetUP()
        { }

        public IPostTradeServices PostTradeServices
        {
            set {; }
        }

        public IPricingAnalysis PricingAnalysis
        {
            set {; }
        }

        #endregion

        private void frmAccountLock_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (PluggableToolsClosed != null)
                {
                    PluggableToolsClosed(this, EventArgs.Empty);
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

        private void grdAccountLock_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.GroupByBox.Hidden = true;
                UltraWinGridUtils.EnableFixedFilterRow(e);

                e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;

                if (grdAccountLock.DisplayLayout.Bands[0].Columns.Exists("FundName"))
                {
                    grdAccountLock.DisplayLayout.Bands[0].Columns["FundName"].CellActivation = Activation.NoEdit;
                    grdAccountLock.DisplayLayout.Bands[0].Columns["FundName"].Header.Caption = "Account Name";
                }
                if (grdAccountLock.DisplayLayout.Bands[0].Columns.Exists("UserName"))
                {
                    grdAccountLock.DisplayLayout.Bands[0].Columns["UserName"].CellActivation = Activation.NoEdit;
                    grdAccountLock.DisplayLayout.Bands[0].Columns["UserName"].Header.Caption = "User Name";
                    //Added by Faisal Shah 24/07/14
                    //Purpose To fill Blank Usernames with N/A
                    //SetBlankUserNames();
                }
                if (grdAccountLock.DisplayLayout.Bands[0].Columns.Exists("UserID"))
                {
                    grdAccountLock.DisplayLayout.Bands[0].Columns["UserID"].Hidden = true;
                }
                if (grdAccountLock.DisplayLayout.Bands[0].Columns.Exists("FundID"))
                {
                    grdAccountLock.DisplayLayout.Bands[0].Columns["FundID"].Hidden = true;
                }
                if (grdAccountLock.DisplayLayout.Bands[0].Columns.Exists("LockStatus"))
                {
                    grdAccountLock.DisplayLayout.Bands[0].Columns["LockStatus"].Header.Caption = "Lock Status";
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
        private void grdAccountLock_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.GetType() == typeof(UltraGridFilterCell))
                {
                    return;
                }
                if (e.Cell.Column.Key == "LockStatus")
                {
                    UltraCheckEditor uceLockStatus = e.Cell.EditorComponent as UltraCheckEditor;
                    if (uceLockStatus.Text == "Unlocked")
                    {
                        uceLockStatus.Text = "Locked";
                    }
                    else
                    {
                        uceLockStatus.Text = "Unlocked";
                    }
                }
                btnSave_Click(null, null);
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
