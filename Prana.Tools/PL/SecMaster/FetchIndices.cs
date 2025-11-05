using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prana.Tools.PL.SecMaster
{
    /// <summary>
    /// modified by: sachin mishra,30 jan 2015
    /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
    /// </summary>
    public partial class FetchIndices : Form
    {
        public FetchIndices()
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
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="preSelectedIndices"></param>
        public FetchIndices(string preSelectedIndices)
        {
            try
            {
                InitializeComponent();
                _preSelectedIndices = preSelectedIndices;
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

        string _preSelectedIndices = string.Empty;

        public event EventHandler<EventArgs<string>> SelectedIndices;

        private void btSelect_Click(object sender, EventArgs e)
        {
            try
            {
                UltraGridRow[] rows = ultraGrid1.Rows.GetFilteredInNonGroupByRows();
                if (rows.Count() == 0)
                {
                    MessageBox.Show("No rows to select.");
                    return;
                }
                StringBuilder strBld = new StringBuilder();
                foreach (UltraGridRow row in rows)
                {
                    if (row.Cells["Select"].Text == true.ToString())
                    {
                        strBld.Append(row.Cells["SymbolPK"].Text + ",");
                    }
                }
                string selectedIndices = strBld.ToString().TrimEnd(',');
                if (SelectedIndices != null)
                    SelectedIndices(this, new EventArgs<string>(selectedIndices));

                this.Close();
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
        /// Adds checkbox type column to the grid
        /// </summary>
        /// <param name="grid">The ultragrid in which the column is to be added</param>
        /// <param name="checkboxName">The name of the column for checkbox</param>
        /// <param name="checkboxCaption">The caption to be shown for the checkbox column</param>
        /// <param name="ultraGroupKey">The key of the group to which the column belongs to, if any</param>
        private void AddCheckBoxinGrid(UltraGrid grid, String checkboxName, string checkboxCaption, String ultraGroupKey)
        {
            try
            {
                if (!grid.DisplayLayout.Bands[0].Columns.Exists(checkboxName))
                    grid.DisplayLayout.Bands[0].Columns.Add(checkboxName, checkboxCaption);
                else
                    grid.DisplayLayout.Bands[0].Columns[checkboxName].Header.Caption = checkboxCaption;
                grid.DisplayLayout.Bands[0].Columns[checkboxName].DataType = typeof(bool);
                //grid.DisplayLayout.Bands[0].Columns[checkboxName].CellClickAction = CellClickAction.Edit;
                grid.DisplayLayout.Bands[0].Columns[checkboxName].Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                grid.DisplayLayout.Bands[0].Columns[checkboxName].Header.CheckBoxSynchronization = HeaderCheckBoxSynchronization.Band;
                grid.DisplayLayout.Bands[0].Columns[checkboxName].CellActivation = Activation.AllowEdit;
                if (ultraGroupKey != null)
                    grid.DisplayLayout.Bands[0].Columns[checkboxName].Group = grid.DisplayLayout.Bands[0].Groups[ultraGroupKey];
                SetCheckBoxAtFirstPosition(grid, checkboxName);
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
        /// sets the checkbox column to the first position in the grid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="checkboxName"></param>
        private void SetCheckBoxAtFirstPosition(UltraGrid grid, String checkboxName)
        {
            try
            {
                grid.DisplayLayout.Bands[0].Columns[checkboxName].Hidden = false;
                grid.DisplayLayout.Bands[0].Columns[checkboxName].Header.VisiblePosition = 0;
                grid.DisplayLayout.Bands[0].Columns[checkboxName].Width = 45;
                grid.DisplayLayout.Bands[0].Columns[checkboxName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                grid.DisplayLayout.Bands[0].Columns[checkboxName].Header.Fixed = true;
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

        private void FetchIndices_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = SMBatchDAL.GetIndexSymbols();
                ultraGrid1.DataSource = ds;
                AddCheckBoxinGrid(ultraGrid1, "Select", "Select", null);
                ultraGrid1.DisplayLayout.Bands[0].HeaderVisible = false;
                CheckBoxesAccordingToPreSelectedIndices();
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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
                btSelect.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btSelect.ForeColor = System.Drawing.Color.White;
                btSelect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btSelect.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btSelect.UseAppStyling = false;
                btSelect.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void CheckBoxesAccordingToPreSelectedIndices()
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(_preSelectedIndices))
                {
                    string[] indices = _preSelectedIndices.Split(',');
                    foreach (string index in indices)
                    {
                        IEnumerable<UltraGridRow> rows = ultraGrid1.Rows.Where(r => r.Cells["SymbolPK"].Value.ToString() == index);
                        if (rows.Count() > 0)
                            rows.First().Cells["Select"].Value = true;
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
    }
}
