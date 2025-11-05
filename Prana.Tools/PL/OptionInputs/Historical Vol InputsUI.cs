using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Tools.PL.OptionInputs
{
    public partial class HistoricalVolInputsUI : Form
    {

        private static DateTime _startDate = DateTime.MinValue;

        public static DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        private static DateTime _endDate = DateTime.MinValue;

        public static DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        private static int _volatilityType = int.MinValue;


        public static int VolatilityType
        {
            get { return _volatilityType; }
            set { _volatilityType = value; }
        }

        /// <summary>
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public HistoricalVolInputsUI()
        {
            try
            {
                InitializeComponent();
                BindVolatilityTypeCombo();
                SetHistoricalVolInputs();
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
        private void BindVolatilityTypeCombo()
        {
            try
            {
                List<EnumerationValue> volatilityTypes = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(RiskConstants.VolType));
                cmbbxVolType.DataSource = null;
                cmbbxVolType.DataSource = volatilityTypes;
                cmbbxVolType.ValueMember = "Value";
                cmbbxVolType.DisplayMember = "DisplayText";
                cmbbxVolType.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbbxVolType.Value = 1;
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
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApplyVolInputs_Click(object sender, EventArgs e)
        {
            try
            {
                StartDate = Convert.ToDateTime(dtStartDate.Value.ToString());
                EndDate = Convert.ToDateTime(dtEndDate.Value.ToString());
                VolatilityType = Convert.ToInt32(cmbbxVolType.Value.ToString());
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
        public void SetHistoricalVolInputs()
        {
            try
            {
                dtStartDate.Value = (object)_startDate;
                dtEndDate.Value = (object)_endDate;
                cmbbxVolType.Value = (object)_volatilityType;
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
        /// modified by: sachin mishra,30 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public static void setDefaultValues_HistoricalVolInputs()
        {
            try
            {
                double days = -30;
                _endDate = DateTime.Now;
                _startDate = _endDate.AddDays(days);
                _volatilityType = 1;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
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

        private void HistoricalVolInputsUI_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRICING_INPUTS);
                    if (CustomThemeHelper.ApplyTheme)
                    {
                        this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                        this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                    }
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                btnApplyVolInputs.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnApplyVolInputs.ForeColor = System.Drawing.Color.White;
                btnApplyVolInputs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnApplyVolInputs.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnApplyVolInputs.UseAppStyling = false;
                btnApplyVolInputs.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCancel.BackColor = System.Drawing.Color.FromArgb(104, 5, 5);
                btnCancel.ForeColor = System.Drawing.Color.White;
                btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnCancel.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnCancel.UseAppStyling = false;
                btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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