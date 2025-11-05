using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.Admin.Controls.BlotterPrefs.Constants;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    public partial class CtrlBlotterClearance : UserControl
    {
        private DateTime[] _dbClearanceTimes;
        private DataTable clearanceDataSource = new DataTable();

        public CtrlBlotterClearance()
        {
            try
            {
                InitializeComponent();
                cmbTimeZone.Visible = true;
                lblTimeZone.Visible = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private DataTable GetData(int companyID)
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetBlotterClearanceForCompany";
            try
            {
                DataSet ds = new DataSet();

                queryData.DictionaryDatabaseParameter.Add("@companyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@companyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });

                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (ds != null && ds.Tables != null && ds.Tables.Count == 1)
                {
                    for (int counter = 0; counter < ds.Tables[0].Rows.Count; counter++)
                    {
                        int AuecID = int.Parse(ds.Tables[0].Rows[counter][1].ToString());
                        BusinessObjects.TimeZone auecTimeZone = CachedDataManager.GetInstance.GetAUECTimeZone(AuecID);

                        ds.Tables[0].Rows[counter][2] = BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(Convert.ToDateTime(ds.Tables[0].Rows[counter][2]), auecTimeZone).ToShortTimeString();
                        ds.Tables[0].Rows[counter][3] = BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(Convert.ToDateTime(ds.Tables[0].Rows[counter][3]), auecTimeZone).ToShortTimeString();
                        ds.Tables[0].Rows[counter][4] = Convert.ToDateTime(ds.Tables[0].Rows[counter][4]).ToShortTimeString();
                        if (!Convert.ToBoolean(ds.Tables[0].Rows[counter][9]) || ds.Tables[0].Rows[counter][10].ToString() == "1/1/1900 12:00:00 AM")
                            ds.Tables[0].Rows[counter][10] = DBNull.Value;
                        if (ds.Tables[0].Rows[counter][11].ToString() == "1/1/1900 12:00:00 AM") //Compared to min date shows no time was saved
                            ds.Tables[0].Rows[counter][11] = DBNull.Value;
                    }

                    clearanceDataSource.Clear();
                    clearanceDataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return clearanceDataSource;
        }

        public void LoadControl(int companyID)
        {
            try
            {
                if (clearanceDataSource.Columns.Count == 0)
                {
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_AUEC, typeof(string));
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_AUECID, typeof(int));
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_START_TIME, typeof(string));
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_END_TIME, typeof(string));
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_CLEARANCE_TIME, typeof(string));
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_CLEARANCE_TIME_ID, typeof(int));
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_COMPANYAUECID, typeof(int));
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_EXCHANGE_IDENTIFIER, typeof(string));
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_PERMIT_ROLLOVER, typeof(bool));
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_IS_SEND_MANUAL_ORDER, typeof(bool));
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_SEND_MANUAL_ORDER_TRIGGER_TIME, typeof(string));
                    this.clearanceDataSource.Columns.Add(BlotterPreferenceConstants.PROPERTY_LAST_MANUAL_ORDER_RUN_TRIGGER_TIME, typeof(string));
                }

                gridClearanceTable.DataSource = null;
                gridClearanceTable.DataSource = GetData(companyID);
                _dbClearanceTimes = new DateTime[gridClearanceTable.Rows.Count];

                foreach (UltraGridRow row in gridClearanceTable.Rows)
                {
                    _dbClearanceTimes[row.Index] = Convert.ToDateTime(row.Cells[BlotterPreferenceConstants.PROPERTY_CLEARANCE_TIME].Value);

                    //Set row color according to permit rollover column value
                    if (Convert.ToBoolean(row.Cells[BlotterPreferenceConstants.PROPERTY_PERMIT_ROLLOVER].Value))
                        gridClearanceTable.DisplayLayout.Rows[row.Index].Appearance.ForeColor = Color.Green;
                    else
                        gridClearanceTable.DisplayLayout.Rows[row.Index].Appearance.ForeColor = Color.Red;
                }

                SetGridStyles();

                Users users = UserManager.GetUsers(companyID);
                ultraComboRolloverPermittedUser.DataSource = users;
                ultraComboRolloverPermittedUser.ValueMember = BlotterPreferenceConstants.PROPERTY_USERID;
                ultraComboRolloverPermittedUser.DisplayMember = BlotterPreferenceConstants.PROPERTY_LOGINNAME;
                ultraComboRolloverPermittedUser.DataBind();

                GetCompanyClearanceCommonData(companyID);
                GetNotifyActiveGtcGtdOrderData(companyID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void GetCompanyClearanceCommonData(int companyID)
        {
            uoAutoClearing.CheckedIndex = 0;
            object[] parameter = new object[1];
            parameter[0] = companyID;

            try
            {
                this.cmbTimeZone.ValueChanged -= new System.EventHandler(this.cmbTimeZone_ValueChanged);
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClearanceCommonData", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (!string.IsNullOrWhiteSpace(row[0].ToString()))
                        {
                            string tempTimeZone = row[0].ToString();
                            tempTimeZone = tempTimeZone.Replace("GMT", "UTC");
                            cmbTimeZone.Text = tempTimeZone;
                        }
                        else
                        {
                            cmbTimeZone.Text = Infragistics.Win.TimeZoneInfo.CurrentTimeZone.ToString();
                        }

                        if (row[1] != DBNull.Value)
                        {
                            uoAutoClearing.CheckedIndex = Convert.ToBoolean(row[1]) ? 0 : 1;
                        }

                        dtBaseTime.ValueChanged -= dtBaseTime_ValueChanged;
                        if (!string.IsNullOrWhiteSpace(row[2].ToString()) && row[0] != DBNull.Value)
                        {
                            dtBaseTime.Value = Convert.ToDateTime(row[2]);
                        }
                        else
                        {
                            if (row[0] != DBNull.Value)
                            {
                                BusinessObjects.TimeZone currentTimeZone = BusinessObjects.TimeZoneInfo.FindTimeZoneByString(cmbTimeZone.Text);
                                DateTime currentUtcTime = DateTime.UtcNow;
                                dtBaseTime.Value = BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime((currentUtcTime), currentTimeZone);
                            }
                            else
                            {
                                dtBaseTime.Value = DateTime.Now;
                            }
                        }
                        if (row[3] != DBNull.Value)
                        {
                            ultraComboRolloverPermittedUser.Value = Convert.ToInt32(row[3]);
                        }
                        dtBaseTime.ValueChanged += dtBaseTime_ValueChanged;

                        if (row[4] != DBNull.Value)
                        {
                            checkBoxLiveManualOrderSend.Checked = Convert.ToBoolean(row[4]);
                        }
                    }
                }
                this.cmbTimeZone.ValueChanged += new System.EventHandler(this.cmbTimeZone_ValueChanged);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void SetGridStyles()
        {
            try
            {
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_CLEARANCE_TIME_ID].Hidden = true;
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_COMPANYAUECID].Hidden = true;
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_AUECID].Hidden = true;

                //Set the cell activation to false.
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_AUEC].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_START_TIME].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_END_TIME].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;

                //Set the column styles.
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_START_TIME].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_END_TIME].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Time;
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_CLEARANCE_TIME].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.TimeWithSpin;
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_SEND_MANUAL_ORDER_TRIGGER_TIME].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.TimeWithSpin;
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_LAST_MANUAL_ORDER_RUN_TRIGGER_TIME].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DateTime;
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_LAST_MANUAL_ORDER_RUN_TRIGGER_TIME].CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_EXCHANGE_IDENTIFIER].Hidden = true;

                gridClearanceTable.DisplayLayout.Bands[0].Columns[BlotterPreferenceConstants.PROPERTY_PERMIT_ROLLOVER].Header.Caption = BlotterPreferenceConstants.CAPTION_PERMIT_ROLLOVER;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void SaveData(int companyID)
        {
            try
            {
                SaveCommonData(companyID);
                SaveNotifyActiveGtcGtdOrder(companyID);
                for (int counter = 0; counter < clearanceDataSource.Rows.Count; counter++)
                {
                    object[] parameters = new object[7];
                    parameters[0] = Convert.ToInt32(clearanceDataSource.Rows[counter][BlotterPreferenceConstants.PROPERTY_CLEARANCE_TIME_ID]);

                    int auecID = int.Parse(clearanceDataSource.Rows[counter][BlotterPreferenceConstants.PROPERTY_AUECID].ToString());
                    BusinessObjects.TimeZone auecTimeZone = CachedDataManager.GetInstance.GetAUECTimeZone(auecID);
                    DateTime UTCTimeFromAUECTime = BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(Convert.ToDateTime(clearanceDataSource.Rows[counter][BlotterPreferenceConstants.PROPERTY_CLEARANCE_TIME]), auecTimeZone);
                    parameters[1] = clearanceDataSource.Rows[counter][BlotterPreferenceConstants.PROPERTY_CLEARANCE_TIME];
                    parameters[2] = Convert.ToInt32(clearanceDataSource.Rows[counter][BlotterPreferenceConstants.PROPERTY_COMPANYAUECID]);
                    parameters[3] = Convert.ToBoolean(clearanceDataSource.Rows[counter][BlotterPreferenceConstants.PROPERTY_PERMIT_ROLLOVER]);
                    parameters[4] = Convert.ToBoolean(clearanceDataSource.Rows[counter][BlotterPreferenceConstants.PROPERTY_IS_SEND_MANUAL_ORDER]);
                    parameters[5] = clearanceDataSource.Rows[counter][BlotterPreferenceConstants.PROPERTY_SEND_MANUAL_ORDER_TRIGGER_TIME];
                    parameters[6] = clearanceDataSource.Rows[counter][BlotterPreferenceConstants.PROPERTY_LAST_MANUAL_ORDER_RUN_TRIGGER_TIME];
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_UpdateBlotterClearanceTime", parameters);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// To fetch the permissions of NotifyActiveGtcGtdOrderData.
        /// </summary>
        /// <param name="companyID"></param>
        private void GetNotifyActiveGtcGtdOrderData(int companyID)
        {
            object[] parameters = new object[1];
            parameters[0] = companyID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("GetNotifyActiveGtcGtdOrdersByCompanyId", parameters))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row != null)
                        {
                            if (row[1] != DBNull.Value && row[1].ToString() == "1")
                            {
                                checkBoxNotifyGTCGTDOrders.Checked = true;
                            }

                            if (row[2] != DBNull.Value && !string.IsNullOrWhiteSpace(row[2].ToString()))
                            {
                                timeToNotify.Value = Convert.ToDateTime(row[2]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// To save the NotifyActiveGtcGtdOrders data in DB.
        /// </summary>
        /// <param name="companyID"></param>
        private void SaveNotifyActiveGtcGtdOrder(int companyID)
        {
            try
            {
                object[] parameters = new object[3];
                parameters[0] = companyID;
                parameters[1] = checkBoxNotifyGTCGTDOrders.Checked ? 1 : 0;
                parameters[2] = timeToNotify.Value.TimeOfDay.ToString();

                DatabaseManager.DatabaseManager.ExecuteNonQuery("UpdateInsertNotifyActiveGtcGtdOrders", parameters);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void SaveCommonData(int companyID)
        {
            try
            {
                object[] parameters = new object[6];
                parameters[0] = cmbTimeZone.Text;
                parameters[1] = uoAutoClearing.CheckedIndex == 0 ? true : false;
                parameters[2] = dtBaseTime.Value.TimeOfDay.ToString();
                parameters[3] = ultraComboRolloverPermittedUser.Value;
                parameters[4] = companyID;
                parameters[5] = checkBoxLiveManualOrderSend.Checked ? 1 : 0;

                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveCompanyClearanceCommonData", parameters);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private void dtBaseTime_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime hwTime = dtBaseTime.Value;
                System.TimeZoneInfo currentTimeZone = System.TimeZoneInfo.FindSystemTimeZoneById(BusinessObjects.TimeZoneInfo.FindTimeZoneByString(cmbTimeZone.Text).StandardName);

                foreach (UltraGridRow row in gridClearanceTable.Rows)
                {
                    int auecid = Convert.ToInt32(row.Cells[BlotterPreferenceConstants.PROPERTY_AUECID].Value);
                    BusinessObjects.TimeZone localtimezone = CachedDataManager.GetInstance.GetAUECTimeZone(auecid);
                    if (localtimezone != null)
                    {
                        System.TimeZoneInfo hwZone = System.TimeZoneInfo.FindSystemTimeZoneById(localtimezone.TimeZoneKey);
                        row.Cells[BlotterPreferenceConstants.PROPERTY_CLEARANCE_TIME].Value = System.TimeZoneInfo.ConvertTime(hwTime, currentTimeZone, hwZone);
                    }
                    else
                    {
                        MessageBox.Show("Time Zone Information not set for AUEC " + CachedDataManager.GetInstance.GetAUECText(auecid), CachedDataManager.GetInstance.GetAUECText(auecid));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private System.TimeZoneInfo previousTimeZone = null;

        private void cmbTimeZone_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                BusinessObjects.TimeZone timeZone = BusinessObjects.TimeZoneInfo.FindTimeZone(cmbTimeZone.Text);

                if (previousTimeZone == null)
                    previousTimeZone = System.TimeZoneInfo.FindSystemTimeZoneById(System.TimeZoneInfo.Local.StandardName);

                System.TimeZoneInfo currentTimeZone = System.TimeZoneInfo.FindSystemTimeZoneById(timeZone.TimeZoneKey);
                dtBaseTime.ValueChanged -= dtBaseTime_ValueChanged;
                dtBaseTime.Value = System.TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(dtBaseTime.Value, DateTimeKind.Unspecified), previousTimeZone, currentTimeZone);
                previousTimeZone = currentTimeZone;
                dtBaseTime.ValueChanged += dtBaseTime_ValueChanged;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        private void clearanceTable_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.Equals(BlotterPreferenceConstants.PROPERTY_PERMIT_ROLLOVER))
                {
                    if (e.Cell.Text.Equals("True"))
                        gridClearanceTable.DisplayLayout.Rows[e.Cell.Row.Index].Appearance.ForeColor = Color.Green;
                    else
                        gridClearanceTable.DisplayLayout.Rows[e.Cell.Row.Index].Appearance.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// To enable/disable timePicker as per NotifyGtcGtdOrdersCheckbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxNotifyGTCGTDOrders_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxNotifyGTCGTDOrders.Checked)
                    timeToNotify.Enabled = true;
                else
                    timeToNotify.Enabled = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}