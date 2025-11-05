using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public partial class AllocationSchemes : Form
    {
        public AllocationSchemes()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
            BindListSchemes();
        }

        private void SetButtonsColor()
        {
            try
            {
                btnContinue.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnContinue.ForeColor = System.Drawing.Color.White;
                btnContinue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnContinue.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnContinue.UseAppStyling = false;
                btnContinue.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

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

        private bool _isInitialized = false;

        private Dictionary<int, string> _allocationSchemes = null;

        private void BindListSchemes()
        {
            try
            {
                _allocationSchemes = AllocationSchemeDataManager.GetInstance.GetAllASchemeNames();
                lstSchemes.DataSource = null;
                if (_allocationSchemes != null && _allocationSchemes.Count > 0)
                {
                    lstSchemes.DataSource = new BindingSource(_allocationSchemes, null);
                    lstSchemes.DisplayMember = "Value";
                    lstSchemes.ValueMember = "Key";
                    lstSchemes.Refresh();
                }
                _isInitialized = true;
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
        public void SuggestNewAllocationName(DateTime date)
        {
            try
            {
                if (_allocationSchemes == null || _allocationSchemes.Count == 0)
                {
                    lblError.Text = "Enter Allocation Scheme Name";
                    txtScheme.Text = "Scheme " + date.ToString("MMddyyyy");
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

        public string _schemeName = string.Empty;

        private void lstSchemes_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_isInitialized)
            {
                this.txtScheme.Text = lstSchemes.Text;
            }

        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                _schemeName = this.txtScheme.Text;
                if (_allocationSchemes != null)
                {
                    if (_allocationSchemes.ContainsValue(_schemeName.ToUpperInvariant()) || _allocationSchemes.ContainsValue(_schemeName) || _allocationSchemes.ContainsValue(_schemeName.ToLowerInvariant()))
                    {
                        DialogResult result = MessageBox.Show("Do you want to override data to the same scheme?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            this.DialogResult = DialogResult.None;
                        }
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Hide();
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
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

        private void txtScheme_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar.Equals('\r'))
                {
                    this.btnContinue_Click(this.btnContinue, e);
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
    }
}