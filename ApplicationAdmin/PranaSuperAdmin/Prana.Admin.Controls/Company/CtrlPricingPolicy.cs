using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    public partial class CtrlPricingPolicy : UserControl
    {
        public CtrlPricingPolicy()
        {
            InitializeComponent();
        }

        public static event EventHandler PricingPolicyEvent;

        private const string COLUMN_IsActive = "IsActive";
        private const string COLUMN_PolicyName = "PolicyName";
        private const string COLUMN_SPName = "SPName";
        private const string COLUMN_IsFileAvailable = "IsFileAvailable";
        private const string COLUMN_FilePath = "FilePath";
        private const string COLUMN_FolderPath = "FolderPath";
        List<int> _deletedPricingPolicies = new List<int>();
        public bool _isSaveRequired = false;



        private void CtrlPricingPolicy_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet dt = PricingRuleManager.GetPricingPolicies();
                grdPricingPolicy.DataSource = dt;
                grdPricingPolicy.DataBind();

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

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            CtrlPricingPolicy_Load(this, null);
            _deletedPricingPolicies.Clear();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                grdPricingPolicy.DisplayLayout.Bands[0].AddNew();
                grdPricingPolicy.Rows[grdPricingPolicy.Rows.Count - 1].Cells[COLUMN_IsActive].SetValue(0, true);
                grdPricingPolicy.Rows[grdPricingPolicy.Rows.Count - 1].Cells[COLUMN_IsFileAvailable].SetValue(0, true);
                grdPricingPolicy.Rows[grdPricingPolicy.Rows.Count - 1].Cells[COLUMN_FilePath].Activation = Activation.Disabled;
                grdPricingPolicy.Rows[grdPricingPolicy.Rows.Count - 1].Cells[COLUMN_FolderPath].Activation = Activation.Disabled;
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
                SavePolicyDetails();

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

        private bool SavePolicyDetails()
        {
            try
            {
                if (_isSaveRequired)
                {

                    if (HasEmpty())
                    {
                        MessageBox.Show("Blank pricing policy cannot be inserted. \nFill in all the details", "Pricing Policy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    DataSet ds = (DataSet)grdPricingPolicy.DataSource;
                    DataTable dt = ds.Tables[0];

                    int isSaved = PricingRuleManager.SavePricingPolicy(dt, _deletedPricingPolicies);
                    if (isSaved < 0)
                    {
                        MessageBox.Show("Duplicate policy cannot be saved! ", "policy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else if (isSaved > 0)
                    {
                        MessageBox.Show("Policy associated with company cannot be deleted !", "policy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        _deletedPricingPolicies.Clear();
                        MessageBox.Show("Policy saved", "policy", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        CtrlPricingPolicy_Load(this, null);
                        if (PricingPolicyEvent != null)
                            PricingPolicyEvent(this, null);
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
            return true;
        }



        private void grdPricingPolicy_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
                UltraGridBand band = e.Layout.Bands[0];
                band.Override.AllowRowFiltering = DefaultableBoolean.True;
                this.grdPricingPolicy.DisplayLayout.Override.GroupByColumnsHidden = DefaultableBoolean.True;

                int i = 1;
                if (band.Columns.Exists("IsActive"))
                {
                    UltraGridColumn colIsActive = band.Columns["IsActive"];
                    colIsActive.Header.Caption = "Is Active";
                    //colIsActive.Width = 50;
                    colIsActive.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    colIsActive.Header.VisiblePosition = i++;
                    colIsActive.AutoSizeMode = ColumnAutoSizeMode.Default;
                }

                if (band.Columns.Exists("PolicyName"))
                {
                    UltraGridColumn colPolicyName = band.Columns["PolicyName"];
                    colPolicyName.Header.Caption = "Policy Name";
                    //colPolicyName.Width = 100;
                    colPolicyName.Header.VisiblePosition = i++;
                    colPolicyName.AutoSizeMode = ColumnAutoSizeMode.Default;
                }

                if (band.Columns.Exists("SPName"))
                {
                    UltraGridColumn colSPName = band.Columns["SPName"];
                    colSPName.Header.Caption = "SP Name";
                    //colSPName.Width = 100;
                    colSPName.Header.VisiblePosition = i++;
                    colSPName.AutoSizeMode = ColumnAutoSizeMode.Default;
                }

                if (band.Columns.Exists("IsFileAvailable"))
                {
                    UltraGridColumn colIsFileAvailable = band.Columns["IsFileAvailable"];
                    colIsFileAvailable.Header.Caption = "Is File Available";
                    //colIsFileAvailable.Width = 150;
                    colIsFileAvailable.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    colIsFileAvailable.Header.VisiblePosition = i++;
                    colIsFileAvailable.AutoSizeMode = ColumnAutoSizeMode.Default;
                }

                if (band.Columns.Exists("FilePath"))
                {
                    UltraGridColumn colFilePath = band.Columns["FilePath"];
                    //edited by amit
                    //CHMW-3287
                    colFilePath.Header.Caption = "FileName";
                    //colFilePath.Width = 240;
                    colFilePath.Header.VisiblePosition = i++;
                    colFilePath.AutoSizeMode = ColumnAutoSizeMode.Default;
                }

                if (band.Columns.Exists("FolderPath"))
                {
                    UltraGridColumn colFolderPath = band.Columns["FolderPath"];
                    colFolderPath.Header.Caption = "Folder Path";
                    //colFolderPath.Width = 240;
                    colFolderPath.Header.VisiblePosition = i++;
                    colFolderPath.AutoSizeMode = ColumnAutoSizeMode.Default;
                }

                if (!band.Columns.Exists("DeleteButton"))
                {
                    UltraGridColumn colDelete = band.Columns.Add("DeleteButton");
                    colDelete.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    //colDelete.Width = 60;
                    colDelete.Header.Caption = "Delete";
                    colDelete.NullText = "Delete";
                    colDelete.Header.VisiblePosition = i++;
                    colDelete.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                    colDelete.AutoSizeMode = ColumnAutoSizeMode.Default;
                }

                //added by amit on 07.04.2015
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3282
                foreach (UltraGridRow row in e.Layout.Rows)
                {
                    UltraGridCell cell = row.Cells[COLUMN_IsFileAvailable];
                    if (bool.Parse(cell.Text) == false)
                    {
                        row.Cells[COLUMN_FilePath].Value = String.Empty;
                        row.Cells[COLUMN_FolderPath].Value = String.Empty;
                        row.Cells[COLUMN_FilePath].Activation = Activation.Disabled;
                        row.Cells[COLUMN_FolderPath].Activation = Activation.Disabled;
                    }
                    else
                    {
                        row.Cells[COLUMN_FilePath].Activation = Activation.AllowEdit;
                        row.Cells[COLUMN_FolderPath].Activation = Activation.AllowEdit;
                    }
                }

                grdPricingPolicy.DisplayLayout.Bands[0].Columns["IsActive"].Hidden = true;
                grdPricingPolicy.DisplayLayout.Bands[0].Columns["Id"].Hidden = true;

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

        private void grdPricingPolicy_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridRow row = e.Cell.Row;
                if (e.Cell.Column.Key == "DeleteButton")
                {


                    int policyID;

                    bool isParsed = int.TryParse(row.Cells["Id"].Value.ToString(), out policyID);
                    bool isActive = Convert.ToBoolean(row.Cells["IsActive"].Value.ToString());
                    if (policyID == 0)
                    {
                        e.Cell.Row.Delete(false);
                    }
                    else
                    {
                        if (isActive)
                        {
                            MessageBox.Show("Policy associated with company cannot be deleted ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            DialogResult dr = MessageBox.Show("Do you want to delete the selected pricing policy", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dr == DialogResult.No)
                            {
                                return;
                            }
                            if (isParsed && !_deletedPricingPolicies.Contains(policyID))
                            {
                                _deletedPricingPolicies.Add(policyID);
                                //  dictActionStatus.TryAdd(pricingRuleID, 2);
                            }
                            _isSaveRequired = true;
                            e.Cell.Row.Delete(false);
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

        private void grdPricingPolicy_AfterCellUpdate(object sender, CellEventArgs e)
        {
            if (!_isSaveRequired)
            {
                _isSaveRequired = true;
            }
        }

        private void grdPricingPolicy_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridRow row = e.Cell.Row;
                if (e.Cell.Column.Key == COLUMN_IsFileAvailable)
                {
                    if (bool.Parse(e.Cell.Text) == false)
                    {
                        //added by amit on 07.04.2015
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3282
                        row.Cells[COLUMN_FilePath].Value = String.Empty;
                        row.Cells[COLUMN_FolderPath].Value = String.Empty;
                        row.Cells[COLUMN_FilePath].Activation = Activation.Disabled;
                        row.Cells[COLUMN_FolderPath].Activation = Activation.Disabled;
                    }
                    else
                    {
                        row.Cells[COLUMN_FilePath].Activation = Activation.AllowEdit;
                        row.Cells[COLUMN_FolderPath].Activation = Activation.AllowEdit;
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

        public bool HasEmpty()
        {
            for (int i = grdPricingPolicy.Rows.Count - 1; i >= 0; i--)
            {
                UltraGridRow grdRow = grdPricingPolicy.Rows[i];
                if (string.IsNullOrEmpty(grdRow.Cells[COLUMN_PolicyName].Text) && string.IsNullOrEmpty(grdRow.Cells[COLUMN_SPName].Text) && string.IsNullOrEmpty(grdRow.Cells[COLUMN_FolderPath].Text) && string.IsNullOrEmpty(grdRow.Cells[COLUMN_FilePath].Text))
                {
                    grdRow.Delete(false);
                    continue;
                }

                else
                {
                    if (grdRow.Cells[COLUMN_IsFileAvailable].Value.ToString() == false.ToString())
                    {

                        if (string.IsNullOrEmpty(grdRow.Cells[COLUMN_PolicyName].Text) || string.IsNullOrEmpty(grdRow.Cells[COLUMN_SPName].Text))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(grdRow.Cells[COLUMN_PolicyName].Text) || string.IsNullOrEmpty(grdRow.Cells[COLUMN_SPName].Text) || string.IsNullOrEmpty(grdRow.Cells[COLUMN_FolderPath].Text) || string.IsNullOrEmpty(grdRow.Cells[COLUMN_FilePath].Text))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
