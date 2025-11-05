using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.TradeManager.Forms
{
    public partial class MessageWithGridPopup : Form
    {
        public MessageWithGridPopup(string header)
        {
            try
            {
                InitializeComponent();
                if (CustomThemeHelper.ApplyTheme)
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_MESSAGE_WITH_GRID);
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, header, CustomThemeHelper.UsedFont);
                }
                SetButtonsColor();
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
        /// Sets the color of the button.
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                this.ultraButtonOk.BackColor = Color.FromArgb(55, 67, 85);
                this.ultraButtonOk.ForeColor = Color.White;
                this.ultraButtonOk.Font = new Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.ultraButtonOk.ButtonStyle = UIElementButtonStyle.Button3D;
                this.ultraButtonOk.UseAppStyling = false;
                this.ultraButtonOk.UseOsThemes = DefaultableBoolean.False;

                this.ultraButtonYes.BackColor = Color.FromArgb(55, 67, 85);
                this.ultraButtonYes.ForeColor = Color.White;
                this.ultraButtonYes.Font = new Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.ultraButtonYes.ButtonStyle = UIElementButtonStyle.Button3D;
                this.ultraButtonYes.UseAppStyling = false;
                this.ultraButtonYes.UseOsThemes = DefaultableBoolean.False;

                this.ultraButtonNo.BackColor = Color.FromArgb(55, 67, 85);
                this.ultraButtonNo.ForeColor = Color.White;
                this.ultraButtonNo.Font = new Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.ultraButtonNo.ButtonStyle = UIElementButtonStyle.Button3D;
                this.ultraButtonNo.UseAppStyling = false;
                this.ultraButtonNo.UseOsThemes = DefaultableBoolean.False;
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
        /// Bind the data and show dialog
        /// </summary>
        public DialogResult ShowDialog(string Message, DataTable data, MessageBoxButtons popupButtons)
        {
            DialogResult dialogResult = DialogResult.OK;
            try
            {
                switch (popupButtons)
                {
                    case MessageBoxButtons.YesNo:
                        this.ultraButtonYes.Visible = true;
                        this.ultraButtonYes.DialogResult = DialogResult.Yes;
                        this.ultraButtonNo.Visible = true;
                        this.ultraButtonNo.DialogResult = DialogResult.No;
                        break;

                    case MessageBoxButtons.OK:
                        this.ultraButtonOk.Visible = true;
                        this.ultraButtonOk.DialogResult = DialogResult.OK;
                        break;
                }
                ultraLabelInformation.Text = Message;
                ultraGridData.DataSource = data;
                ultraGridData.DrawFilter = new NoFocusIndicatorDrawFilter();
                dialogResult = this.ShowDialog();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dialogResult;
        }

        /// <summary>
        /// Initilaize layout of ultraGridData grid
        /// </summary>
        private void ultraGridData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                ultraGridData.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.False;
                ultraGridData.DisplayLayout.Override.SelectedAppearancesEnabled = Infragistics.Win.DefaultableBoolean.False;
                foreach (var column in ultraGridData.DisplayLayout.Bands[0].Columns)
                {
                    column.CellActivation = Activation.Disabled;
                    column.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand, true);
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
        /// Close the form
        /// </summary>
        private void CloseMessageBox(object sender, EventArgs e)
        {
            try
            {
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
    }
}
