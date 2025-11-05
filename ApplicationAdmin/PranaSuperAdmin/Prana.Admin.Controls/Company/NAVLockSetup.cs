using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace Prana.Admin.Controls.Company
{
    public partial class NAVLockSetup : UserControl
    {
        ValueList _vlUser = new ValueList();
        public NAVLockSetup()
        {
            InitializeComponent();
            //grdNavLock.DataSource = NAVLockManager.InitializeData();
        }

        /// <summary>
        /// Initialize the grid with the data
        /// </summary>
        public void InitializeControl()
        {
            try
            {
                grdNavLock.DataSource = NAVLockManager.InitializeData();
                //UserValueList();
                // ValidateLastLockDate();
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
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                this.grdNavLock.DisplayLayout.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
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
        /// Get the list of users
        /// </summary>
        private void UserValueList()
        {
            try
            {
                if (NAVLockManager._dictUsers.Count > 0)
                {
                    foreach (int userID in NAVLockManager._dictUsers.Keys)
                    {
                        string userName = NAVLockManager._dictUsers[userID];
                        _vlUser.ValueListItems.Add(userID, userName);
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
        /// Initialize the layout of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdNavLock_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.ActiveRowAppearance.BackColor = Color.Black;
                e.Layout.Override.ActiveRowAppearance.BackColor2 = Color.Black;
                e.Layout.Override.ActiveRowAppearance.ForeColor = Color.White;

                e.Layout.Override.RowAppearance.BackColor = Color.Black;
                e.Layout.Override.RowAppearance.BackColor2 = Color.Black;
                e.Layout.Override.RowAppearance.ForeColor = Color.White;

                e.Layout.Override.ActiveCellAppearance.BackColor = Color.Black;
                e.Layout.Override.ActiveCellAppearance.BackColor2 = Color.Black;
                e.Layout.Override.ActiveCellAppearance.ForeColor = Color.White;

                UltraGridBand grdBand = e.Layout.Bands[0];
                grdBand.Override.AllowRowFiltering = DefaultableBoolean.True;
                if (!grdBand.Columns.Exists("Select"))
                {
                    UltraGridColumn colSelect = grdBand.Columns.Add("Select");
                    colSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    colSelect.Header.Caption = string.Empty;
                    colSelect.Width = 25;
                    colSelect.DataType = typeof(bool);
                    colSelect.Header.VisiblePosition = 1;
                }

                if (grdBand.Columns.Exists("CompanyID"))
                {
                    UltraGridColumn colCompanyID = grdBand.Columns["CompanyID"];
                    colCompanyID.Hidden = true;
                }
                if (grdBand.Columns.Exists("ActionID"))
                {
                    UltraGridColumn colActionID = grdBand.Columns["ActionID"];
                    colActionID.Hidden = true;
                }
                if (grdBand.Columns.Exists("PostingLockScheduleID"))
                {
                    UltraGridColumn colPostLockScheduleID = grdBand.Columns["PostingLockScheduleID"];
                    colPostLockScheduleID.Hidden = true;
                }
                if (grdBand.Columns.Exists("CompanyFundID"))
                {
                    UltraGridColumn colAccountID = grdBand.Columns["CompanyFundID"];
                    colAccountID.Hidden = true;
                }
                if (grdBand.Columns.Exists("Name"))
                {
                    UltraGridColumn colClient = grdBand.Columns["Name"];
                    colClient.Header.Caption = "Client";
                    colClient.CellActivation = Activation.NoEdit;
                    colClient.Header.VisiblePosition = 2;
                }
                if (grdBand.Columns.Exists("FundName"))
                {
                    UltraGridColumn colAccount = grdBand.Columns["FundName"];
                    colAccount.Header.Caption = "Account";
                    colAccount.CellActivation = Activation.NoEdit;
                    colAccount.Header.VisiblePosition = 3;
                }

                if (grdBand.Columns.Exists("Status"))
                {
                    UltraGridColumn colStatus = grdBand.Columns["Status"];
                    colStatus.Header.Caption = "Current Status";
                    colStatus.CellActivation = Activation.NoEdit;
                    colStatus.Header.VisiblePosition = 4;
                }
                //UltraGridColumn colLastLockDate = grdBand.Columns["LastLockedDate"];
                //colLastLockDate.Header.Caption = "Last Locked";
                //colLastLockDate.CellActivation = Activation.NoEdit;

                if (grdBand.Columns.Exists("PreviousLockDate"))
                {
                    UltraGridColumn colLockAppliedDate = grdBand.Columns["PreviousLockDate"];
                    colLockAppliedDate.Header.Caption = "Previous Lock Date (GMT)";
                    colLockAppliedDate.Format = "yyyy-MM-dd HH:mm:ss";
                    colLockAppliedDate.CellActivation = Activation.NoEdit;
                    colLockAppliedDate.Header.VisiblePosition = 5;
                    //colLockAppliedDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Nothing;
                }
                if (grdBand.Columns.Exists("LockAppliedOn"))
                {
                    //UltraGridColumn colLockDate = grdBand.Columns["LockDate"];
                    UltraGridColumn colLockDate = grdBand.Columns["LockAppliedOn"];
                    colLockDate.Header.Caption = "Lock Date (GMT)";
                    colLockDate.MaskInput = "{LOC}mm/dd/yyyy hh:mm:ss";
                    colLockDate.Format = "yyyy-MM-dd HH:mm";
                    colLockDate.Header.VisiblePosition = 6;
                    colLockDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null;
                }
                if (grdBand.Columns.Exists("LokedOn"))
                {
                    //UltraGridColumn colLockApplyDate = grdBand.Columns["LockAppliedOn"];
                    UltraGridColumn colLockApplyDate = grdBand.Columns["LokedOn"];
                    colLockApplyDate.Header.Caption = "Locked On (GMT)";
                    colLockApplyDate.CellActivation = Activation.NoEdit;
                    colLockApplyDate.Format = "yyyy-MM-dd HH:mm:ss";
                    colLockApplyDate.Header.VisiblePosition = 7;
                }
                if (grdBand.Columns.Exists("scheduleName"))
                {
                    UltraGridColumn colLockSchedule = grdBand.Columns["scheduleName"];
                    colLockSchedule.Header.Caption = "Lock Schedule";
                    colLockSchedule.CellActivation = Activation.NoEdit;
                    colLockSchedule.Header.VisiblePosition = 8;
                }
                if (grdBand.Columns.Exists("SuggestedLockDate"))
                {
                    UltraGridColumn colSuggLockDate = grdBand.Columns["SuggestedLockDate"];
                    colSuggLockDate.Header.Caption = "Suggested Lock Date (GMT)";
                    colSuggLockDate.Format = "yyyy-MM-dd HH:mm";
                    colSuggLockDate.CellActivation = Activation.NoEdit;
                    colSuggLockDate.Header.VisiblePosition = 9;
                }

                if (grdBand.Columns.Exists("LokedBy"))
                {
                    grdBand.Columns["LokedBy"].Hidden = true;
                }

                if (grdBand.Columns.Exists("LockedAuditTrailId"))
                {
                    grdBand.Columns["LockedAuditTrailId"].Hidden = true;
                }

                if (grdBand.Columns.Exists("LockedByUserName"))
                {
                    UltraGridColumn colLockUser = grdBand.Columns["LockedByUserName"];
                    colLockUser.Header.Caption = "Last Locked By";
                    colLockUser.CellActivation = Activation.NoEdit;
                    colLockUser.Header.VisiblePosition = 10;
                    //colLockUser.ValueList = _vlUser;
                }

                if (grdBand.Columns.Exists("UnLokedBy"))
                {
                    grdBand.Columns["UnLokedBy"].Hidden = true;
                }

                if (grdBand.Columns.Exists("UnlockedByUserName"))
                {
                    UltraGridColumn colUnLockUser = grdBand.Columns["UnlockedByUserName"];
                    colUnLockUser.Header.Caption = "Last Unlocked By";
                    colUnLockUser.CellActivation = Activation.NoEdit;
                    colUnLockUser.Header.VisiblePosition = 11;
                    //colUnLockUser.ValueList = _vlUser;
                }
                if (grdBand.Columns.Exists("UnLokedOn"))
                {
                    UltraGridColumn colUnlockDate = grdBand.Columns["UnLokedOn"];
                    colUnlockDate.Header.Caption = "Last Unlocked On (GMT)";
                    colUnlockDate.CellActivation = Activation.NoEdit;
                    colUnlockDate.Format = "yyyy-MM-dd HH:mm";
                    colUnlockDate.Header.VisiblePosition = 12;
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
        /// Lock the selected NAV records and save details to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnLock_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValidInput())
                {
                    MessageBox.Show("Select at least one account to lock", "NAV Lock Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                foreach (UltraGridRow ugRow in grdNavLock.Rows)
                {

                    if (Convert.ToBoolean(ugRow.Cells["Select"].Value) == true)
                    {
                        DateTime lockDate;
                        DateTime PreviousLockDate;
                        DateTime.TryParse(ugRow.Cells["PreviousLockDate"].Value.ToString(), out PreviousLockDate);
                        bool islockDateParsed = DateTime.TryParse(ugRow.Cells["LockAppliedOn"].Value.ToString(), out lockDate);
                        if (islockDateParsed)
                        {
                            if (lockDate < PreviousLockDate)
                            {
                                MessageBox.Show("The Lock date cannot be less than the previous lock date", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            TimeSpan curTime = DateTime.Now.ToUniversalTime().TimeOfDay;
                            DateTime CurDateTime = lockDate.Date.Add(curTime);
                            ugRow.Cells["LockAppliedOn"].Value = CurDateTime;
                        }
                        else
                        {
                            ugRow.Cells["LockAppliedOn"].Value = DateTime.Now.ToUniversalTime();

                        }

                        //ugRow.Cells["PreviousLockDate"].Value = ugRow.Cells["LokedOn"].Value;
                        ugRow.Cells["ActionID"].Value = (int)NAVLockItem.LockAction.LOCK;
                        //ugRow.Cells["LokedOn"].Value = ugRow.Cells["LockAppliedOn"].Value;//DateTime.Now;
                        ugRow.Cells["LokedBy"].Value = AuthorizationManager.GetInstance()._authorizedPrincipal.UserId;
                        ugRow.Cells["LokedOn"].Value = DateTime.Now.ToUniversalTime();
                    }
                }
                if (grdNavLock.DataSource != null)
                {
                    // DataTable dt = (DataTable)grdNavLock.DataSource;
                    int i = NAVLockManager.SaveRecords(true);
                    if (i > 0)
                    {
                        MessageBox.Show("Selected records have been locked", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        foreach (UltraGridRow row in grdNavLock.Rows)
                        {
                            if (Convert.ToBoolean(row.Cells["Select"].Value) == true)
                            {
                                //row.Cells["LokedBy"].Value = AuthorizationManager.GetInstance().authorizedPrincipal.UserId;
                                // row.Cells["LokedOn"].Value = DateTime.Now.ToUniversalTime();
                                if (!String.IsNullOrEmpty(row.Cells["LockAppliedOn"].ToString()))
                                {
                                    //string value = row.Cells["LockAppliedOn"].Value.ToString();
                                    DateTime dtLock = DateTime.Parse(row.Cells["LockAppliedOn"].Value.ToString());//.ToUniversalTime();
                                    int accountID = int.Parse(row.Cells["CompanyFundID"].Value.ToString());

                                    Tuple<string, int> auditData = NAVLockManager.GetNewLastLockdate(dtLock, accountID);
                                    row.Cells["PreviousLockDate"].Value = auditData.Item1;
                                    row.Cells["LockedAuditTrailId"].Value = auditData.Item2;
                                    // row.Cells["PreviousLockDate"].Value = NAVLockManager.GetNewLastLockdate(dtLock, accountID);

                                }
                            }
                        }
                        DataTable dt1 = (DataTable)grdNavLock.DataSource;
                        foreach (DataRow dRow in dt1.Rows)
                        {
                            NAVLockManager.ExtendDataTable(dRow);
                        }
                        //InitializeControl();
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
        /// Check if at least one row is selected
        /// </summary>
        /// <returns>true if yes, false if no</returns>
        private bool IsValidInput()
        {
            foreach (UltraGridRow row in grdNavLock.Rows)
            {
                if (Convert.ToBoolean(row.Cells["Select"].Value) == true)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Unlock the selected NAV records and save details to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnUnlock_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValidInput())
                {
                    MessageBox.Show("Select at least one account to unlock", "NAV Lock Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                foreach (UltraGridRow ugRow in grdNavLock.Rows)
                {
                    if (Convert.ToBoolean(ugRow.Cells["Select"].Value) == true)
                    {
                        if (string.IsNullOrEmpty(ugRow.Cells["PreviousLockDate"].Value.ToString()) && string.IsNullOrEmpty(ugRow.Cells["LockAppliedOn"].Value.ToString()))
                        {
                            //MessageBox.Show("Some of the accounts have never been locked. Uncheck those accounts first ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            lblError.Visible = true;
                            return;
                        }
                        ugRow.Cells["ActionID"].Value = (int)NAVLockItem.LockAction.UNLOCK;
                        ugRow.Cells["UnLokedBy"].Value = AuthorizationManager.GetInstance()._authorizedPrincipal.UserId;
                        ugRow.Cells["UnLokedOn"].Value = DateTime.Now.ToUniversalTime();
                    }
                }
                DataTable dt = (DataTable)grdNavLock.DataSource;
                int i = NAVLockManager.SaveRecords(false);
                if (i > 0)
                {
                    MessageBox.Show("Most recently applied lock(s) have been reverted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    foreach (UltraGridRow ugRow in grdNavLock.Rows)
                    {
                        if (Convert.ToBoolean(ugRow.Cells["Select"].Value) == true)
                        {
                            int accountID = int.Parse(ugRow.Cells["CompanyFundID"].Value.ToString());
                            if (ugRow.Cells["PreviousLockDate"].Text != string.Empty && DateTime.Parse(ugRow.Cells["PreviousLockDate"].Value.ToString()) != DateTime.MinValue)
                            {
                                ugRow.Cells["LockAppliedOn"].Value = DateTime.Parse(ugRow.Cells["PreviousLockDate"].Value.ToString());
                            }
                            else
                            {
                                ugRow.Cells["LockAppliedOn"].Value = DBNull.Value;
                                ugRow.Cells["LokedOn"].Value = DBNull.Value;
                            }
                            DateTime lockDate = DateTimeConstants.MinValue;
                            if (!string.IsNullOrEmpty(ugRow.Cells["LockAppliedOn"].Value.ToString()))
                            {
                                lockDate = DateTime.Parse(ugRow.Cells["LockAppliedOn"].Value.ToString());
                            }
                            // ugRow.Cells["PreviousLockDate"].Value = NAVLockManager.GetNewLastLockdate(lockDate, accountID);
                            Tuple<string, int> auditData = NAVLockManager.GetNewLastLockdate(lockDate, accountID);
                            ugRow.Cells["PreviousLockDate"].Value = auditData.Item1;
                            ugRow.Cells["LockedAuditTrailId"].Value = auditData.Item2;

                            if (ugRow.Cells["PreviousLockDate"].Value == DBNull.Value)
                            {
                                ugRow.Cells["PreviousLockDate"].Value = DateTime.MinValue;// ugRow.Cells["LockAppliedOn"].Value;
                            }
                            DataTable dt1 = (DataTable)grdNavLock.DataSource;
                            foreach (DataRow dRow in dt1.Rows)
                            {
                                NAVLockManager.ExtendDataTable(dRow);
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
        /// Check to prevent the lock date to be less than the previous lock date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdNavLock_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                //CancelEventArgs cancelEvent = new CancelEventArgs();
                //grdNavLock_BeforeCellDeactivate(this, cancelEvent);

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
        /// validate the last lock date
        /// </summary>
        private void ValidateLastLockDate()
        {
            try
            {
                foreach (UltraGridRow row in grdNavLock.Rows)
                {
                    if (!string.IsNullOrEmpty(row.Cells["PreviousLockDate"].Value.ToString()) && !string.IsNullOrEmpty(row.Cells["LockAppliedOn"].Value.ToString()))
                    {
                        if (row.Cells["PreviousLockDate"].Value.ToString() == row.Cells["LockAppliedOn"].Value.ToString() || DateTime.Parse(row.Cells["PreviousLockDate"].Value.ToString()) == DateTime.MinValue)
                        {
                            row.Cells["PreviousLockDate"].Value = string.Empty;
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

        private void grdNavLock_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //modified by omshiv, Added checks for lock date 
            DateTime lockDate;
            DateTime PreviousLockDate;
            DateTime lockDateBeforeChange;
            try
            {
                if (grdNavLock.ActiveCell.Column.Key == "LockAppliedOn" && !string.IsNullOrEmpty(grdNavLock.ActiveRow.Cells["LockAppliedOn"].Text))
                {

                    bool islockDateParsed = DateTime.TryParse(grdNavLock.ActiveRow.Cells["LockAppliedOn"].Value.ToString(), out lockDate);
                    DateTime.TryParse(grdNavLock.ActiveRow.Cells["LockAppliedOn"].Value.ToString(), out lockDateBeforeChange);
                    DateTime.TryParse(grdNavLock.ActiveRow.Cells["PreviousLockDate"].Value.ToString(), out PreviousLockDate);

                    if (islockDateParsed)
                    {
                        if (lockDate < PreviousLockDate || lockDateBeforeChange < PreviousLockDate)
                        {
                            MessageBox.Show("The lock date cannot be less than the previous lock date", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Cancel = true;
                            grdNavLock.ActiveRow.Cells["LockAppliedOn"].CancelUpdate();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Format of the Lock date is not correct.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                        grdNavLock.ActiveRow.Cells["LockAppliedOn"].CancelUpdate();
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
        /// Added By faisal Shah
        /// Pupose to Set Grid to Read Only if User has  read Permissions only
        /// </summary>
        /// <param name="haspermission"></param>
        public void SetGridAccess(bool haspermission)
        {
            try
            {
                if (!haspermission)
                {
                    grdNavLock.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                    uBtnLock.Enabled = false;
                    uBtnUnlock.Enabled = false;
                }
                else
                {
                    grdNavLock.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                    uBtnLock.Enabled = true;
                    uBtnUnlock.Enabled = true;
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
