using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    public partial class EnableDisabledItems : UserControl
    {
        public EnableDisabledItems()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Button Click Event To Enable the selected Disabled Items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnableItems_Click(object sender, EventArgs e)
        {

            int selectedValue = -1;
            try
            {
                List<int> entriesToBeEnabled = new List<int>();
                entriesToBeEnabled = GetListOfItemsToBeEnabled();
                if (Convert.ToInt32(cmbSelection.Value) != -1)
                    if (entriesToBeEnabled.Count > 0)
                        selectedValue = Convert.ToInt32(cmbSelection.Value);
                    else
                    {
                        MessageBox.Show("No Items Selected , Please Select at least one Item to be Enabled", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        return;
                    }
                else
                {
                    MessageBox.Show("Please Select a Valid Value", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    return;
                }
                EnableDisabledItemsManager.SaveItemsToBeEnabled(selectedValue, entriesToBeEnabled);
                DataTable dtDisabledItems = new DataTable();
                if (Convert.ToInt32(cmbSelection.Value) != -1)
                {
                    dtDisabledItems = EnableDisabledItemsManager.GetSelectedDisabledItems(Convert.ToInt32(cmbSelection.Value));
                }
                BindGrid(dtDisabledItems);
                MessageBox.Show("The Selected Items Have been Enabled", " ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

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
        /// Created By Faisal Shah 17/07/14
        /// Gets the List of Selected Items which are to be enabled
        /// </summary>
        /// <returns></returns>
        private List<int> GetListOfItemsToBeEnabled()
        {
            List<int> ListOfItemsToBeEnabled = new List<int>();
            try
            {
                foreach (UltraGridRow dr in grdDisabledItems.Rows)
                {
                    if (dr.Cells["Select"].Value != null && dr.Cells["Select"].Value.ToString() == "True")
                        ListOfItemsToBeEnabled.Add(int.Parse(dr.Cells["ID"].Value.ToString()));
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
            return ListOfItemsToBeEnabled;
        }

        /// <summary>
        /// Created By Faisal Shah 17/07/14
        /// Bind Data to Combo Box from Enum created for Items.
        /// </summary>
        public void BindCombo()
        {
            try
            {
                List<EnumerationValue> listValues = new List<EnumerationValue>();
                cmbSelection.DataSource = null;
                listValues = Prana.Utilities.MiscUtilities.EnumHelper.ConvertEnumForBindingWithSelectValueAndCaption(typeof(DisabledItemsList));
                cmbSelection.DataSource = listValues;
                cmbSelection.DisplayMember = "DisplayText";
                cmbSelection.ValueMember = "Value";
                cmbSelection.DataBind();
                cmbSelection.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbSelection.DisplayLayout.Bands[0].ColHeadersVisible = false;
                cmbSelection.Value = -1;
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

        private void grdDisabledItems_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand band = e.Layout.Bands[0];

            band.Override.RowAlternateAppearance.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;

            band.Override.ButtonStyle = UIElementButtonStyle.Button3D;
            grdDisabledItems.DisplayLayout.Override.AllowColSizing =
                   Infragistics.Win.UltraWinGrid.AllowColSizing.Free;

            grdDisabledItems.DisplayLayout.Override.RowSizing = RowSizing.AutoFree;
            foreach (UltraGridColumn col in this.grdDisabledItems.DisplayLayout.Bands[0].Columns)
            {
                // Here we "turn off" theme for the column header.  
                col.Header.Appearance.ThemedElementAlpha = Alpha.Transparent;
                col.Header.Appearance.BackColor = Color.White;
                col.Header.Appearance.ForeColor = Color.White;
                col.Header.Appearance.BackColorDisabled = Color.Orange;

                col.CellAppearance.ThemedElementAlpha = Alpha.Transparent;
                col.CellAppearance.BackColor = Color.Black;
                col.CellAppearance.ForeColor = Color.White;

                if (col.Key != null && col.Key != "Select")
                {
                    col.CellActivation = Activation.NoEdit;
                }
            }

            //   string[] array = { "Select","ID","Name","ShortName" };
            //  List<string> lstColumns = new List<string>(array);
            int visiblePosition = 0;
            //foreach (string column in lstColumns)
            //{
            //    if (!band.Columns.Exists(column))
            //    {
            //        band.Columns.Add(column);
            //    }
            //}

            if (!band.Columns.Exists("Select"))
            {
                band.Columns.Add("Select");
            }
            UltraGridColumn colSelect = band.Columns["Select"];
            colSelect.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
            colSelect.Width = 50;
            colSelect.Header.Caption = "";
            colSelect.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
            colSelect.Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
            colSelect.Header.CheckBoxAlignment = HeaderCheckBoxAlignment.Center;
            colSelect.Header.VisiblePosition = visiblePosition++;
            colSelect.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;

            if (band.Columns.Exists("ID"))
            {
                band.Columns["ID"].Hidden = true;
            }
            if (band.Columns.Exists("ExecutionTime"))
            {
                UltraGridColumn colDeletedDate = band.Columns["ExecutionTime"];
                colDeletedDate.Header.Caption = "Deleted Date";
                colDeletedDate.CellActivation = Activation.NoEdit;
            }
            if (band.Columns.Exists("DeletedBy"))
            {
                UltraGridColumn colShortName = band.Columns["DeletedBy"];
                colShortName.Header.Caption = "Deleted By";
                colShortName.CellActivation = Activation.NoEdit;
            }
        }

        /// <summary>
        /// Created By Faisal Shah 17/07/14
        /// On changing selected value in Combo Box Changing the respective entries on the grid as per the value selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSelection_ValueChanged(object sender, EventArgs e)
        {
            DataTable dtDisabledItems = new DataTable();
            if (Convert.ToInt32(cmbSelection.Value) != -1)
            {
                dtDisabledItems = EnableDisabledItemsManager.GetSelectedDisabledItems(Convert.ToInt32(cmbSelection.Value));
                if (dtDisabledItems.Rows.Count == 0 && Convert.ToInt32(cmbSelection.Value) != -1)
                {
                    MessageBox.Show("There is no disabled item for " + Enum.GetName(typeof(DisabledItemsList), cmbSelection.Value).ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            BindGrid(dtDisabledItems);
        }

        public void BindGrid(DataTable dtDisabledItems)
        {
            if (Convert.ToInt32(cmbSelection.Value) != -1)
            {
                if (dtDisabledItems.Rows.Count == 0 && Convert.ToInt32(cmbSelection.Value) != -1)
                {
                    grdDisabledItems.DataSource = null;
                    return;
                }
                grdDisabledItems.DataSource = dtDisabledItems;
            }
            else
                grdDisabledItems.DataSource = null;

        }
        /// <summary>
        /// Apply Theme to the Controls.
        /// </summary>
        public void ApplyTheme()
        {
            this.ForeColor = System.Drawing.Color.White;
            grdDisabledItems.DisplayLayout.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
            grdDisabledItems.DisplayLayout.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            grdDisabledItems.DisplayLayout.Appearance.ForeColor = System.Drawing.Color.White;
            this.grpDisabledItems.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
            this.grpDisabledItems.Appearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
            this.grpDisabledItems.ForeColor = System.Drawing.Color.White;
            this.grpDisabledItems.HeaderAppearance.ForeColor = System.Drawing.Color.White;
            this.btnEnableItems.Appearance.BackColor = System.Drawing.Color.Gray;
            this.btnEnableItems.Appearance.ForeColor = System.Drawing.Color.Black;

        }

        /// <summary>
        /// Set Controls as read only for Read permissions.
        /// </summary>  
        public void SetControlsAsReadOnly()
        {
            grdDisabledItems.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
            btnEnableItems.Enabled = false;
        }
    }
}
