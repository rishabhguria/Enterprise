using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.Windows.Forms;

namespace Prana.Admin.Controls.PmPrefs
{
    public partial class PMPrefsCtrl : UserControl
    {
        DataTable _dtGridDataTable = new DataTable();
        public PMPrefsCtrl()
        {
            InitializeComponent();
        }

        public void SetUIFromPrefs(PMUIPrefs inputPrefs)
        {
            try
            {
                if (inputPrefs.NumberOfCustomViewsAllowed <= numericUpDown1.Maximum && inputPrefs.NumberOfCustomViewsAllowed >= numericUpDown1.Minimum)
                {
                    numericUpDown1.Value = inputPrefs.NumberOfCustomViewsAllowed;
                }
                else
                {
                    numericUpDown1.Value = numericUpDown1.Maximum;
                }
                if (inputPrefs.NumberOfVisibleColumnsAllowed <= numericUpDown3.Maximum && inputPrefs.NumberOfVisibleColumnsAllowed >= numericUpDown3.Minimum)
                {
                    numericUpDown3.Value = inputPrefs.NumberOfVisibleColumnsAllowed;
                }
                else
                {
                    numericUpDown3.Value = numericUpDown3.Maximum;
                }
                cbFetchData.Checked = inputPrefs.FetchDataFromHistoricalDb;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                throw;
            }
        }

        public PMUIPrefs GetPrefsFromUI()
        {
            PMUIPrefs selectedPrefs = new PMUIPrefs();
            try
            {
                selectedPrefs.NumberOfCustomViewsAllowed = (int)numericUpDown1.Value;
                selectedPrefs.NumberOfVisibleColumnsAllowed = (int)numericUpDown3.Value;
                selectedPrefs.FetchDataFromHistoricalDb = (bool)cbFetchData.Checked;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                throw;
            }
            return selectedPrefs;

        }

        public void SetPMCalculationPreference(DataSet dsPMCalculationPreference, AccountCollection dsAccounts)
        {
            try
            {
                //Set Account values to ComboBox
                cmbAccount.DataSource = null;
                cmbAccount.DataSource = dsAccounts;
                cmbAccount.DisplayMember = "Name";
                cmbAccount.ValueMember = "AccountID";
                Utils.UltraDropDownFilter(cmbAccount, "Name");

                //Set values to UltraGrid
                _dtGridDataTable = dsPMCalculationPreference.Tables[0];
                ultraGridForAccounts.DataSource = _dtGridDataTable;
                ultraGridForAccounts.DataBind();
                ultraGridForAccounts.DisplayLayout.Bands[0].Columns["TraderPayoutPercent"].Header.Caption = "Trader Payout %";
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

        bool isDataValidToSave = true;
        public DataTable GetPMCalculationPreference(int companyID)
        {
            isDataValidToSave = true;
            DataTable dtDataToSave = null;
            DataTable dtDataToSaveTemp = null;
            try
            {
                dtDataToSave = (DataTable)ultraGridForAccounts.DataSource;

                if (dtDataToSave != null && isDataValidToSave)
                {
                    //Creating a table with the stipulated columns to convert it to XML before saving.
                    dtDataToSaveTemp = new DataTable();
                    dtDataToSaveTemp.Columns.Add(new DataColumn("CompanyID"));
                    dtDataToSaveTemp.Columns.Add(new DataColumn("FundID"));
                    dtDataToSaveTemp.Columns.Add(new DataColumn("HighWaterMark"));
                    dtDataToSaveTemp.Columns.Add(new DataColumn("Stopout"));
                    dtDataToSaveTemp.Columns.Add(new DataColumn("TraderPayoutPercent"));

                    foreach (DataRow dr in dtDataToSave.Rows)
                    {
                        DataRow drNew = dtDataToSaveTemp.NewRow();
                        foreach (DataColumn dc in dtDataToSave.Columns)
                        {
                            if (dr["FundID"] == System.DBNull.Value)
                            {
                                InformationMessageBox.Display("Please Select A Account !", "PM Preferences");
                                isDataValidToSave = false;
                                break;
                            }
                            else if (dr[dc.ColumnName] == System.DBNull.Value)
                            {
                                InformationMessageBox.Display("Value Can't be NULL !", "PM Preferences");
                                isDataValidToSave = false;
                                break;
                            }

                            if (dc.ColumnName == "FundID")
                            {
                                drNew["FundID"] = dr["FundID"];
                            }
                            else
                            {
                                drNew[dc.ColumnName] = Math.Round(Convert.ToDouble(dr[dc.ColumnName].ToString()), 4);
                            }
                        }
                        if (isDataValidToSave)
                        {
                            drNew["CompanyID"] = companyID;
                            dtDataToSaveTemp.Rows.Add(drNew);
                            dtDataToSaveTemp.AcceptChanges();
                        }
                        else
                        {
                            dtDataToSaveTemp = null;
                            break;
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
            return dtDataToSaveTemp;
        }

        private void ultraGridForAccounts_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridColumn colAccountID = ultraGridForAccounts.DisplayLayout.Bands[0].Columns["FundID"];
                colAccountID.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colAccountID.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colAccountID.ValueList = cmbAccount;
                colAccountID.Header.Caption = "Account";
                colAccountID.Width = 150;
                colAccountID.Header.VisiblePosition = 0;
                foreach (UltraGridColumn column in ultraGridForAccounts.DisplayLayout.Bands[0].Columns)
                {
                    column.InvalidValueBehavior = InvalidValueBehavior.RevertValue;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ultraGridForAccounts.DataSource != null)
                {
                    _dtGridDataTable = (DataTable)ultraGridForAccounts.DataSource;
                    double zeroValue = 0;
                    DataRow dtRow = null;
                    dtRow = _dtGridDataTable.NewRow();
                    for (int i = 1; i < dtRow.ItemArray.Length; i++)
                    {
                        dtRow[i] = zeroValue;
                    }

                    _dtGridDataTable.Rows.Add(dtRow);
                    ultraGridForAccounts.DataSource = null;
                    ultraGridForAccounts.DataSource = _dtGridDataTable;
                    ultraGridForAccounts.Update();
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

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (ultraGridForAccounts.DataSource != null)
                {
                    if (ultraGridForAccounts.ActiveRow != null)
                    {
                        ultraGridForAccounts.ActiveRow.Delete(true);
                        _dtGridDataTable.AcceptChanges();
                    }
                    else
                    {
                        InformationMessageBox.Display("Please select a row to delete", "Information");
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

        private void ultraGridForAccounts_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                DataRow row = ((System.Data.DataRowView)(e.Cell.Row.ListObject)).Row;
                string column = e.Cell.Column.Key;
                if (column.Equals("AccountID"))
                {
                    isDataValidToSave = ValidateRowForGrid();
                    if (!isDataValidToSave)
                    {
                        e.Cell.CancelUpdate();
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

        private bool ValidateRowForGrid()
        {
            bool isValid = true;
            string account = ultraGridForAccounts.ActiveRow.Cells["AccountID"].Text;
            int currentIndex = ultraGridForAccounts.ActiveRow.Index;
            int checkIndex = 0;

            if (String.IsNullOrEmpty(account))
            {
                isValid = false;
                return isValid;
            }

            //If the same Account already exists.
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow dr in ultraGridForAccounts.Rows)
            {
                string dAccount = dr.Cells["AccountID"].Text;
                checkIndex = dr.Index;
                if (account == dAccount && checkIndex != currentIndex)
                {
                    isValid = false;
                    InformationMessageBox.Display("Account already exists, please select different one.", "PM Preference");
                    break;
                }
            }
            return isValid;
        }
    }
}
