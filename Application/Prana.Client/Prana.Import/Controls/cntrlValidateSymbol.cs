using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.Import.Controls
{
    public partial class cntrlValidateSymbol : UserControl
    {
        public cntrlValidateSymbol()
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
                btnSymbolLookUp.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSymbolLookUp.ForeColor = System.Drawing.Color.White;
                btnSymbolLookUp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSymbolLookUp.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSymbolLookUp.UseAppStyling = false;
                btnSymbolLookUp.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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

        public void setupMethod()
        {
            try
            {
                //if (methodName == string.Empty)
                //{
                //    lblHeader.Text = "BB Symbol Pop Up";
                //    btnSymbolLookUp.Visible = false;
                //}
                //else
                //{
                //    lblHeader.Text = "Attempt Validation";
                //    btnSymbolLookUp.Visible = true;
                //}
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


        private void btnAttemptValidation_Click(object sender, EventArgs e)
        {
            try
            {

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
    }
}
