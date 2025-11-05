using Prana.BusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.CommonDataCache.Cache_Classes;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Forms
{
    public partial class AddAccountWiseCurrenyPair : Form
    {
        private bool _isAdditionPossible = false;
        private StandardCurrencyPair _standardCurrency;

        public AddAccountWiseCurrenyPair()
        {
            InitializeComponent();
        }

        public StandardCurrencyPair StandardCurrency
        {
            get { return _standardCurrency; }
        }

        private void AddAccountWiseCurrenyPair_Load(object sender, EventArgs e)
        {
            CurrencyCollection currencyCollection = WindsorContainerManager.GetCurrenciesWithSymbol();
            cmbFromCurrency.DataSource = null;
            cmbFromCurrency.DataSource = currencyCollection;
            cmbFromCurrency.DisplayMember = "Symbol";
            cmbFromCurrency.ValueMember = "CurrencyID";

            cmbToCurrency.DataSource = null;
            cmbToCurrency.DataSource = currencyCollection;
            cmbToCurrency.DisplayMember = "Symbol";
            cmbToCurrency.ValueMember = "CurrencyID";

            cmbToCurrency.Value = 0;
            cmbFromCurrency.Value = 0;

            txtFxSymbol.Text = String.Empty;

            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
            ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
            ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
            SetButtonsColor();
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
                btnAdd.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnAdd.ForeColor = System.Drawing.Color.White;
                btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAdd.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAdd.UseAppStyling = false;
                btnAdd.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnCancel.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbFromCurrency.SelectedItem != null && cmbToCurrency.SelectedItem != null && !String.IsNullOrEmpty(txtFxSymbol.Text))
                {
                    int fromCurrency, toCurrency;
                    if (Int32.TryParse(cmbFromCurrency.SelectedItem.DataValue.ToString(), out fromCurrency) && Int32.TryParse(cmbToCurrency.SelectedItem.DataValue.ToString(), out toCurrency))
                    {

                        if (!CurrencyPairCache.GetInstance().IsPairAvailable(fromCurrency, toCurrency))
                        {
                            _standardCurrency = new StandardCurrencyPair();
                            _standardCurrency.FromCurrency = fromCurrency;
                            _standardCurrency.ToCurrency = toCurrency;
                            _standardCurrency.Symbol = txtFxSymbol.Text;
                            CurrencyPairCache.GetInstance().AddCurrencyPair(fromCurrency, toCurrency);
                            _isAdditionPossible = true;
                            this.Close();
                        }
                        else
                        {
                            _isAdditionPossible = false;
                            lblStatusBar.Text = "Same currency pair or opposite currency pair already exists, please select different pair.";
                        }
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                _isAdditionPossible = false;
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

        public bool ShowAddCurrencyForm(Form pForm)
        {
            try
            {
                this.ShowDialog(pForm);
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
            return this._isAdditionPossible;
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbFromCurrency.SelectedItem != null && cmbToCurrency.SelectedItem != null && !String.IsNullOrEmpty(txtFxSymbol.Text))
                {
                    int fromCurrency, toCurrency;
                    if (Int32.TryParse(cmbFromCurrency.SelectedItem.DataValue.ToString(), out fromCurrency) && Int32.TryParse(cmbToCurrency.SelectedItem.DataValue.ToString(), out toCurrency))
                    {
                        if (fromCurrency == 0 || toCurrency == 0 || fromCurrency == toCurrency)
                        {
                            btnAdd.Enabled = false;
                        }
                        else
                        {
                            btnAdd.Enabled = true;
                        }
                    }
                }
                else
                {
                    btnAdd.Enabled = false;
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
    }
}
