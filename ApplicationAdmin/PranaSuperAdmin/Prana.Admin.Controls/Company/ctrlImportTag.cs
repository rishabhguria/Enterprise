using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    public partial class ctrlImportTag : UserControl
    {

        public bool _isSaveRequired = false;
        public bool _isValidData = false;

        public ctrlImportTag()
        {
            InitializeComponent();
        }

        private void ctrlImportTag_Load(object sender, EventArgs e)
        {

        }

        public void ApplyTheme()
        {
            try
            {
                this.grdImportTag.DisplayLayout.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
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
        /// make the controls read only if the user does not have write permission
        /// </summary>
        /// <param name="isActive"></param>
        public void SetGridAccess(bool isActive)
        {
            try
            {
                if (!isActive)
                {
                    grdImportTag.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                    //grdPermissions.DisplayLayout.Bands[0].Override.AllowAddNew = DefaultableBoolean.False;
                    //grdPermissions.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                    btnAddRow.Enabled = false;
                }
                else
                {
                    grdImportTag.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                    btnAddRow.Enabled = true;
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

        public int SaveImportTagDetails()
        {
            try
            {
                if (grdImportTag.DisplayLayout.Bands[0].Override.AllowUpdate == DefaultableBoolean.False)
                {
                    return 1;
                }
                if (!_isSaveRequired)
                {
                    return 1;
                }
                if (HasEmpty())
                {
                    _isValidData = false;
                    return 0;
                }
                if (grdImportTag.DataSource != null)
                {
                    bool isSaved = ImportTagManager.SaveImportTagData((DataTable)grdImportTag.DataSource);
                    if (!isSaved)
                    {
                        _isValidData = false;
                        MessageBox.Show("Details could not be saved.", "Import Tag", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        grdImportTag.UpdateData();
                        return 1;
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
            return 0;
        }

        public void InitializeData()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ImportTagManager.GetImportTagDataTableFromDB();
                grdImportTag.DataSource = dt;
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
        /// Check if grid has empty rows or empty cells
        /// </summary>
        /// <returns>True if there are empty cells</returns>
        public bool HasEmpty()
        {
            List<string> lstrows = new List<string>();
            for (int i = grdImportTag.Rows.Count - 1; i >= 0; i--)
            {
                UltraGridRow ugRow = grdImportTag.Rows[i];

                if (string.IsNullOrEmpty(ugRow.Cells[ImportTagManager.colImportTagName].Text) && string.IsNullOrEmpty(ugRow.Cells[ImportTagManager.colAcronym].Text))
                {
                    ugRow.Delete(false);
                    continue;
                }
                if (string.IsNullOrEmpty(ugRow.Cells[ImportTagManager.colImportTagName].Text) || string.IsNullOrEmpty(ugRow.Cells[ImportTagManager.colAcronym].Text))
                {
                    MessageBox.Show("Blank Import Tag cannot be inserted. \nFill in all the details", "Import Tag", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }



                // Validation for unique Acronym name
                if (!lstrows.Contains(ugRow.Cells[ImportTagManager.colAcronym].Text.Trim(), StringComparer.OrdinalIgnoreCase))
                {
                    lstrows.Add(ugRow.Cells[ImportTagManager.colAcronym].Text.Trim());
                }
                else
                {
                    MessageBox.Show("Duplicate Tag details cannot be inserted. Details could not be saved.", "Import Tag", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isValidData = false;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Called when cell value changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdImportTag_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (!_isSaveRequired)
                {
                    _isSaveRequired = true;
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
        /// Add new rows to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                grdImportTag.DisplayLayout.Bands[0].AddNew();
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

        private void grdImportTag_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = e.Layout.Bands[0];
                band.Override.AllowRowFiltering = DefaultableBoolean.True;
                if (!band.Columns.Exists("DeleteButton"))
                {
                    UltraGridColumn colDelete = band.Columns.Add("DeleteButton");
                    colDelete.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colDelete.Width = 20;
                    colDelete.Header.Caption = "";
                    colDelete.Header.VisiblePosition = 0;
                    colDelete.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
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
        /// Handle the click of the delete button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdImportTag_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridRow row = e.Cell.Row;
                if (e.Cell.Column.Key == "DeleteButton")
                {
                    if (!string.IsNullOrEmpty(row.Cells[ImportTagManager.colAcronym].Value.ToString()) && !string.IsNullOrEmpty(row.Cells[ImportTagManager.colImportTagName].Value.ToString()))
                    {
                        DialogResult dr = MessageBox.Show("Do you want to delete the selected row?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                        {
                            return;
                        }
                        if (!_isSaveRequired)
                        {
                            _isSaveRequired = true;
                        }
                    }
                    e.Cell.Row.Delete(false);
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
