using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Prana.Analytics
{
    public partial class NonParallelShiftsUI : Form
    {
        public NonParallelShiftsUI()
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

        private string _stepAnalViewID = string.Empty;
        private string _stepAnalViewName = string.Empty;

        public event EventHandler PrefChanged;

        public void SetUp(string viewID, string viewName)
        {
            try
            {
                _stepAnalViewID = viewID;
                _stepAnalViewName = viewName;
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

        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                statusLabel.Text = String.Empty;
                if (ultraTabControl1.SelectedTab.Key == "Settings")
                {
                    ctrlSettings1.SetUp(_stepAnalViewName);
                }
                else if (ultraTabControl1.SelectedTab.Key == "VolShockAdjustment")
                {
                    ctrlVolShockAdjustment1.SetUp(_stepAnalViewName);
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

        private void btnSaveNonParallelShifts_Click(object sender, EventArgs e)
        {
            try
            {
                statusLabel.Text = String.Empty;
                if (ultraTabControl1.SelectedTab.Key == "Settings")
                {
                    ctrlSettings1.SavePreferences();
                    if (PrefChanged != null)
                    {
                        PrefChanged(this, null);
                    }
                }
                else if (ultraTabControl1.SelectedTab.Key == "VolShockAdjustment")
                {
                    ctrlVolShockAdjustment1.SavePreferences();
                }
                statusLabel.ForeColor = System.Drawing.Color.Green;
                statusLabel.Text = "Preferences Saved";
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

        public void SetUpGrid(Dictionary<string, Dictionary<int, string>> UDAData)
        {
            try
            {
                ctrlSettings1.SetUpGrid(UDAData);
                statusLabel.Text = String.Empty;
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

        private void NonParallelShiftsUI_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
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
        /// <summary>
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnSaveNonParallelShifts.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSaveNonParallelShifts.ForeColor = System.Drawing.Color.White;
                btnSaveNonParallelShifts.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSaveNonParallelShifts.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSaveNonParallelShifts.UseAppStyling = false;
                btnSaveNonParallelShifts.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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