using Infragistics.Win.UltraWinEditors;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.BusinessObjects.Enumerators;
using Prana.ClientCommon;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Prana.ThirdPartyUI.Forms
{
    public partial class ToleranceProfile : Form
    {
        private ThirdPartyToleranceProfile _thirdPartyToleranceProfile;
        private List<ThirdPartyToleranceProfileCommon> _thirdPartyToleranceProfileCommon;
        private List<string> listOfJobName;
        private List<string> listOfExecutingBroker;
        private bool _createNewToleranceProfile = false;
        private int _decimalPrecision;

        /// <summary>
        /// Event handler for sending back the created tolerance profile
        /// </summary>
        public event EventHandler<EventArgs<bool>> updateToleranceProfileGrid;

        public ToleranceProfile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Tolerance Profile Load Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToleranceProfile_Load(object sender, EventArgs e)
        {
            try
            {
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY);
                this.StartPosition = FormStartPosition.CenterScreen;
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
        /// Initializes DataSource of Tolerance Profile
        /// </summary>
        /// <param name="thirdPartyToleranceProfile"></param>
        /// <param name="createNewToleranceProfile"></param>
        public void InitializeDataSource(ThirdPartyToleranceProfile thirdPartyToleranceProfile, bool createNewToleranceProfile = false)
        {
            try
            {
                _thirdPartyToleranceProfile = thirdPartyToleranceProfile;
                _createNewToleranceProfile = createNewToleranceProfile;
                if (_thirdPartyToleranceProfile != null)
                {
                    if (_createNewToleranceProfile)
                    {
                        SetUILayoutForNewToleranceProfile();
                        LoadComboBoxesData();
                        IntializeComboBoxes();
                    }
                    else
                        SetUILayoutForEditToleranceProfile();
                    IntializeToleranceFields();
                }
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
        /// Initializes Text/Radio Fields of Tolerance Profile
        /// </summary>
        private void IntializeToleranceFields()
        {
            try
            {
                executingBrokerUltraCombo.Text = _thirdPartyToleranceProfile.ExecutingBroker;
                jobNameUltraCombo.Text = _thirdPartyToleranceProfile.JobName;
                lastModifiedTextBox.Text = _thirdPartyToleranceProfile.LastModified.ToString();
                if (_thirdPartyToleranceProfile.MatchingField == 1)
                    toleranceValueRadioButton.Checked = true;
                else
                    tolerancePercentageRadioButton.Checked = true;
                avgPriceTextEditor.Value = _thirdPartyToleranceProfile.AvgPrice;
                netMoneyTextEditor.Value = _thirdPartyToleranceProfile.NetMoney;
                commissionTextEditor.Value = _thirdPartyToleranceProfile.Commission;
                miscFeesTextEditor.Value = _thirdPartyToleranceProfile.MiscFees;
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
        /// Load data required for New Tolerance Profile
        /// </summary>
        private void LoadComboBoxesData()
        {
            try
            {
                //Get Tolerance profile common data
                _thirdPartyToleranceProfileCommon = ThirdPartyClientManager.ServiceInnerChannel.GetToleranceProfileCommonData();
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
        /// Initializes ComboBoxes
        /// </summary>
        private void IntializeComboBoxes()
        {
            try
            {
                listOfExecutingBroker = _thirdPartyToleranceProfileCommon.Select(x => x.ExecutingBroker).Distinct().ToList();
                executingBrokerUltraCombo.DataSource = listOfExecutingBroker;
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
        /// Set UI Layout For New Tolerance Profile
        /// </summary>
        private void SetUILayoutForNewToleranceProfile()
        {
            try
            {
                lastModifiedPanel.Visible = false;
                lastModifiedPanel.Height = 0;

                int y = toleranceProfileAttributePanel.Location.Y - 10;
                int x = toleranceProfileAttributePanel.Location.X;
                toleranceProfileAttributePanel.Location = new System.Drawing.Point(x, y);

                jobNameUltraCombo.ReadOnly = true;

                cancelButton.Text = "Reset";
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
        /// Set UI Layout For Edit Tolerance Profile
        /// </summary>
        private void SetUILayoutForEditToleranceProfile()
        {
            try
            {
                executingBrokerUltraCombo.ReadOnly = true;
                jobNameUltraCombo.ReadOnly = true;
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
        /// Cancel Button Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (_createNewToleranceProfile)
                {
                    executingBrokerUltraCombo.Value = null;
                    jobNameUltraCombo.Value = null;
                    jobNameUltraCombo.ReadOnly = true;
                    toleranceValueRadioButton.Checked = true;
                    // For ValueRadioButton, set decimal percision to 6
                    _decimalPrecision = 6;
                    avgPriceTextEditor.Value = 0;
                    netMoneyTextEditor.Value = 0;
                    commissionTextEditor.Value = 0;
                    miscFeesTextEditor.Value = 0;
                }
                else
                    this.Close();
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
        /// Save Button Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (validateToleranceProfile(_thirdPartyToleranceProfile))
                {
                    if (_createNewToleranceProfile)
                        _thirdPartyToleranceProfile.ThirdPartyBatchId = _thirdPartyToleranceProfileCommon
                                                                        .Where(x => x.ExecutingBroker == executingBrokerUltraCombo.Text && x.JobName == jobNameUltraCombo.Text)
                                                                        .Select(x => x.ThirdPartyBatchId).FirstOrDefault();

                    _thirdPartyToleranceProfile.MatchingField = toleranceValueRadioButton.Checked ? (int)MatchingField.ToleranceInValue : (int)MatchingField.ToleranceInPercentage;
                    _thirdPartyToleranceProfile.AvgPrice = Convert.ToDecimal(avgPriceTextEditor.Value);
                    _thirdPartyToleranceProfile.NetMoney = Convert.ToDecimal(netMoneyTextEditor.Value);
                    _thirdPartyToleranceProfile.Commission = Convert.ToDecimal(commissionTextEditor.Value);
                    _thirdPartyToleranceProfile.MiscFees = Convert.ToDecimal(miscFeesTextEditor.Value);
                    _thirdPartyToleranceProfile.LastModified = DateTime.Now;

                    //Save in DB
                    if (_createNewToleranceProfile)
                    {
                        if (ThirdPartyClientManager.ServiceInnerChannel.SaveThirdPartyToleranceProfile(_thirdPartyToleranceProfile) != 0)
                        {
                            MessageBox.Show(this, "Another Tolerance Profile with same Job Name and Executing Broker already exists.\nPlease delete it to proceed.", "Tolerance Profile", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                        ThirdPartyClientManager.ServiceInnerChannel.UpdateThirdPartyToleranceProfile(_thirdPartyToleranceProfile);

                    // before close refresh Tolerance data grid
                    refreshToleranceProfileGrid();
                    this.Close();
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
        /// Refresh to Tolerance Profile Grid
        /// </summary>
        protected virtual void refreshToleranceProfileGrid()
        {
            try
            {
                updateToleranceProfileGrid?.Invoke(this, new EventArgs<bool>(true));
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
        /// Value Radio Button Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toleranceValueRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (toleranceValueRadioButton.Checked)
                {
                    _thirdPartyToleranceProfile.MatchingField = (int)MatchingField.ToleranceInValue;
                    // For Value, set decimal percision to 6
                    _decimalPrecision = 6;
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
        /// Percentage radio Button Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tolerancePercentageRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (tolerancePercentageRadioButton.Checked)
                {
                    _thirdPartyToleranceProfile.MatchingField = (int)MatchingField.ToleranceInPercentage;
                    // For Value, set decimal percision to 4
                    _decimalPrecision = 4;

                    miscFeesTextEditor.Text = precisionChanged(miscFeesTextEditor.Text, miscFeesTextEditor.Text.IndexOf("."), _decimalPrecision);
                    avgPriceTextEditor.Text = precisionChanged(avgPriceTextEditor.Text, avgPriceTextEditor.Text.IndexOf("."), _decimalPrecision);
                    commissionTextEditor.Text = precisionChanged(commissionTextEditor.Text, commissionTextEditor.Text.IndexOf("."), _decimalPrecision);
                    netMoneyTextEditor.Text = precisionChanged(netMoneyTextEditor.Text, netMoneyTextEditor.Text.IndexOf("."), _decimalPrecision);
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
        /// Executing Broker Combo Box handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void executingBrokerUltraCombo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                clearStatus();
                jobNameUltraCombo.ReadOnly = true;
                if (_createNewToleranceProfile)
                {
                    jobNameUltraCombo.Value = null;
                    listOfJobName = _thirdPartyToleranceProfileCommon
                                                    .Where(x => x.ExecutingBroker == executingBrokerUltraCombo.Text)
                                                    .Select(x => x.JobName).Distinct().ToList();
                    jobNameUltraCombo.DataSource = listOfJobName;

                    if (listOfExecutingBroker.Contains(executingBrokerUltraCombo.Text))
                    {
                        jobNameUltraCombo.ReadOnly = false;
                    }
                }
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
        /// Job Nmae Combox Handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void jobNameUltraCombo_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                clearStatus();
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
        /// Set Status on StatusBar
        /// </summary>
        /// <param name="message"></param>
        private void setStatus(string message)
        {
            try
            {
                toolStripStatusLabel1.Text = message;
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
        /// clear Status on StatusBar
        /// </summary>
        private void clearStatus()
        {
            try
            {
                toolStripStatusLabel1.Text = string.Empty;
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
        /// validating Tolerance Profile
        /// </summary>
        /// <param name="thirdPartyToleranceProfile"></param>
        private bool validateToleranceProfile(ThirdPartyToleranceProfile thirdPartyToleranceProfile)
        {
            try
            {
                if (_createNewToleranceProfile)
                {
                    if (!listOfExecutingBroker.Contains(executingBrokerUltraCombo.Text))
                    {
                        setStatus("Please select Executing Broker");
                        return false;
                    }
                    if (!listOfJobName.Contains(jobNameUltraCombo.Text))
                    {
                        setStatus("Please select Job Name");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return true;
        }

        /// <summary>
        /// To Handle Textbox only takes decimal values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decimalTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                var decimalTextBox = sender as UltraTextEditor;
                // Allow control keys and decimal point and if not already present
                if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || (e.KeyChar == '.') && !decimalTextBox.Text.Contains("."))
                {
                    return;
                }
                e.Handled = true;
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
        /// To Handle Textbox decimal values upto given decimal points
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decimalTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var decimalTextBox = sender as UltraTextEditor;
                int decimalPointIndex = decimalTextBox.Text.IndexOf(".");
                if (decimalPointIndex > -1 && decimalTextBox.Text.Length - decimalPointIndex > _decimalPrecision + 1)
                {
                    // Remove the extra characters
                    decimalTextBox.Text = decimalTextBox.Text.Substring(0, decimalPointIndex + _decimalPrecision + 1);

                    // Set the cursor to the end of the text
                    decimalTextBox.SelectionStart = decimalTextBox.Text.Length;
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
        /// To Handle Textbox decimal values percision changes on radio button
        /// </summary>
        /// <param name="sender"></param>
        private string precisionChanged(string text, int index, int _decimalPrecision)
        {
            try
            {
                if (index > -1 && text.Length - index > _decimalPrecision + 1)
                {
                    // Remove the extra characters
                    text = text.Substring(0, index + _decimalPrecision + 1);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return text;
        }
    }
}
