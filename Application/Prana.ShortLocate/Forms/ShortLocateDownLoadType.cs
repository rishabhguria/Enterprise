using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using Prana.Utilities.UI.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.ShortLocate.Forms
{
    public partial class ShortLocateDownLoadType : Form
    {
        private int format = int.MinValue;
        public ShortLocateDownLoadType()
        {
            InitializeComponent();
            if (CustomThemeHelper.ApplyTheme)
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_DAILY_PM_CLIENTUI);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" + "Choose Format!" + "</p>";
                // this.ultraFormManager1.DrawFilter = new FormTitleHelper("Choose Format!", "", CustomThemeHelper.UsedFont);
            }
            SetButtonsColor();
        }
        private void SetButtonsColor()
        {
            try
            {
                btn_OK.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btn_OK.ForeColor = System.Drawing.Color.White;
                btn_OK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btn_OK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btn_OK.UseAppStyling = false;
                btn_OK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider1.SetError(chkExcel, "");
                errorProvider1.SetError(chkCSV, "");

                if ((chkExcel.CheckState == CheckState.Unchecked) && (chkCSV.CheckState == CheckState.Unchecked))
                {
                    errorProvider1.SetError(chkExcel, "Please select either of the options before proceeding further!");
                    chkExcel.Focus();
                    return;
                }
                else if ((chkExcel.CheckState == CheckState.Checked) && (chkCSV.CheckState == CheckState.Checked))
                {
                    errorProvider1.SetError(chkExcel, "Please select only one option before proceeding further!");
                    chkExcel.Focus();
                    return;
                }
                else
                {
                    if (chkExcel.CheckState == CheckState.Checked)
                    {
                        format = 1;
                    }
                    else if (chkCSV.CheckState == CheckState.Checked)
                    {
                        format = 2;
                    }

                    this.Hide();

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

        public int FormatReturn()
        {
            try
            {
                if (format == int.MinValue)
                    return int.MinValue;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return format;
        }

        public void ExcelExport(UltraGrid Grid)
        {
            try
            {
                List<UltraGrid> GridList = new List<UltraGrid>();
                GridList.Add(Grid);

                ExcelAndPrintUtilities ExportObj = new ExcelAndPrintUtilities();
                ExportObj.ExportToExcel(GridList, "Test", true);
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

        public void CSVExport(UltraGrid Grid)
        {
            try
            {
                List<UltraGrid> GridList = new List<UltraGrid>();
                GridList.Add(Grid);

                ExcelAndPrintUtilities ExportObj = new ExcelAndPrintUtilities();
                ExportObj.SetinCSVFormat(GridList, false, null);
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
    }
}
