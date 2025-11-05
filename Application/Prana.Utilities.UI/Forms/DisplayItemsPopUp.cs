using Infragistics.Win;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Utilities.UI.Forms
{
    public partial class DisplayItemsPopUp : Form
    {
        private DisplayItemsPopUp(string header)
        {
            try
            {
                InitializeComponent();

                if (CustomThemeHelper.ApplyTheme)
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_SHORTCUTS);
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, header, CustomThemeHelper.UsedFont);
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

        /// <summary>
        /// Bind the data for Trading Rules
        /// </summary>
        public static DialogResult ShowDialog(string Message, DataTable data, string header, MessageBoxButtons buttons)
        {
            DialogResult result = DialogResult.Cancel;
            try
            {
                DisplayItemsPopUp popUp = new DisplayItemsPopUp(header);
                popUp.Text = header;
                popUp.ultraLabelInformation.Text = Message;
                popUp.ultraGridData.DataSource = data;
                switch (buttons)
                {
                    case MessageBoxButtons.YesNo:
                        popUp.SetYesNoButtons();
                        break;
                    case MessageBoxButtons.OK:
                        popUp.SetOKButton();
                        break;
                }
                result = popUp.ShowDialog();
                result = popUp._isOverrideAllowed ? DialogResult.Yes : DialogResult.No;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

        private bool _isOverrideAllowed = false;

        private void btnYes_Click(object sender, EventArgs e)
        {
            _isOverrideAllowed = true;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            _isOverrideAllowed = false;
            this.Close();
        }

        /// <summary>
        /// Sets the yes no buttons.
        /// </summary>
        private void SetYesNoButtons()
        {
            try
            {
                this.ultraPanelBottom.ClientArea.Controls.Add(this.ultraButtonNo);
                this.ultraPanelBottom.ClientArea.Controls.Add(this.ultraButtonYes);
                ultraButtonNo.ButtonStyle = UIElementButtonStyle.Button3D;
                ultraButtonNo.BackColor = Color.FromArgb(140, 5, 5);
                ultraButtonNo.ForeColor = Color.White;
                ultraButtonNo.UseAppStyling = false;
                ultraButtonNo.UseOsThemes = DefaultableBoolean.False;

                ultraButtonYes.ButtonStyle = UIElementButtonStyle.Button3D;
                ultraButtonYes.BackColor = Color.FromArgb(104, 156, 46);
                ultraButtonYes.ForeColor = Color.White;
                ultraButtonYes.UseAppStyling = false;
                ultraButtonYes.UseOsThemes = DefaultableBoolean.False;
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

        /// <summary>
        /// Sets the ok button.
        /// </summary>
        private void SetOKButton()
        {
            try
            {
                this.ultraPanelBottom.ClientArea.Controls.Add(this.ultraButtonOK);
                ultraButtonOK.ButtonStyle = UIElementButtonStyle.Button3D;
                ultraButtonOK.BackColor = Color.FromArgb(55, 67, 85);
                ultraButtonOK.ForeColor = Color.White;
                ultraButtonOK.UseAppStyling = false;
                ultraButtonOK.UseOsThemes = DefaultableBoolean.False;
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
