using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;
//using Prana.PostTrade;

namespace Prana.PM.Client.UI
{
    public partial class BaseEquityValue : Form
    {
        #region Constructor region
        public BaseEquityValue()
        {
            InitializeComponent();
            UltraDTPBaseEquityValue.Value = System.DateTime.Now;
            GetSavedValues();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnClose.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClose.ForeColor = System.Drawing.Color.White;
                btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClose.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClose.UseAppStyling = false;
                btnClose.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        #endregion Constructor region

        #region  private methods

        private void GetSavedValues()
        {
            clsBaseEquityValues baseEquityVals = EquityAndAccrualsDataManager.GetCompanyBaseEquityValues();

            if (baseEquityVals != null)
            {
                txtBaseEquityValue.Text = baseEquityVals.BaseEquityValue.ToString();
                UltraDTPBaseEquityValue.Value = baseEquityVals.BaseEquityDate;
            }
        }
        private clsBaseEquityValues GetBaseEquityValues()
        {
            clsBaseEquityValues baseEquityVals = new clsBaseEquityValues();

            if (txtBaseEquityValue.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the Base Equity Value.", "PM Alert");
                txtBaseEquityValue.Focus();
                baseEquityVals = null;
                return baseEquityVals;
            }
            Double res;
            bool blnresult = Double.TryParse(txtBaseEquityValue.Text.Trim(), out res);
            if (blnresult == false)
            {
                MessageBox.Show(" Please enter the numeric Base Equity Value.", "PM Alert");
                txtBaseEquityValue.Focus();
                baseEquityVals = null;
                return baseEquityVals;
            }

            if (txtBaseEquityValue.Text.Trim() != string.Empty && blnresult != false)
            {
                PM.Client.UI.Forms.PM ownerForm = (PM.Client.UI.Forms.PM)(this.Owner);
                baseEquityVals.CompanyId = ownerForm.LoginUser.CompanyID;
                baseEquityVals.BaseEquityDate = UltraDTPBaseEquityValue.Value;
                baseEquityVals.BaseEquityValue = Convert.ToDouble(txtBaseEquityValue.Text.Trim());
                return baseEquityVals;
            }

            return baseEquityVals;

        }

        #endregion  private methods

        #region Button Click Evens
        private void btnSave_Click(object sender, EventArgs e)
        {
            clsBaseEquityValues objBaseEquityVal = GetBaseEquityValues();
            if (objBaseEquityVal != null)
            {
                //TODO
                int result = EquityAndAccrualsDataManager.SaveCompanyBaseEquityValues(objBaseEquityVal.CompanyId, objBaseEquityVal.BaseEquityDate, objBaseEquityVal.BaseEquityValue);
                if (result == 1)
                {
                    MessageBox.Show("Base Equity Values Saved.", "Information");
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion Button Click Evens



    }
}