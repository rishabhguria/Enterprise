using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.ThirdPartyReport
{
    public partial class ThirdPartyFileType : System.Windows.Forms.Form
    {
        public ThirdPartyFileType()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnOK.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnOK.ForeColor = System.Drawing.Color.White;
                btnOK.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnOK.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnOK.UseAppStyling = false;
                btnOK.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        public ThirdPartyFileType(string delimiterDisplayName)
        {
            InitializeComponent();
            chkUserDefind.Text = "Delimited - " + delimiterDisplayName;
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_THIRD_PARTY_FILE_TYPE);
                ultraFormManager1.FormStyleSettings.Caption = "<p style=\"Text-align:Left\">" + this.Text + "</p>";
                ultraFormManager1.DrawFilter = new FormTitleHelper(this.Text, CustomThemeHelper.PRODUCT_COMPANY_NAME, CustomThemeHelper.UsedFont);

            }
        }

        #region Properties 

        private int _format = int.MinValue;

        #endregion Properties

        #region Event

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider1.SetError(chkUserDefind, "");
                errorProvider1.SetError(chkEXL, "");

                if ((chkUserDefind.CheckState == CheckState.Unchecked) && (chkEXL.CheckState == CheckState.Unchecked))
                {
                    errorProvider1.SetError(chkUserDefind, "Please select either of the options before proceeding further!");
                    chkUserDefind.Focus();
                    return;
                }
                else
                {
                    if (chkUserDefind.CheckState == CheckState.Checked)
                    {
                        _format = 1;
                    }
                    else if (chkEXL.CheckState == CheckState.Checked)
                    {
                        _format = 2;
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

        private void chkUserDefind_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUserDefind.CheckState == CheckState.Checked)
            {
                chkEXL.CheckState = CheckState.Unchecked;

            }
        }

        private void chkEXL_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEXL.CheckState == CheckState.Checked)
            {
                chkUserDefind.CheckState = CheckState.Unchecked;
            }
        }

        #endregion Event

        public int FileFormat
        {
            get
            {
                return _format;
            }
        }

        private void ThirdPartyFileType_FormClosed(object sender, FormClosedEventArgs e)
        {
            _format = int.MinValue;
        }





    }
}