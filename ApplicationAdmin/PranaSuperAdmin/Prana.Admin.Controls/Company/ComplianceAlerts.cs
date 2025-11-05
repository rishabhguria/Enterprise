using Prana.BusinessObjects.Compliance.CompliancePref;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.IO;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    public partial class ComplianceAlerts : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComplianceAlerts"/> class.
        /// </summary>
        public ComplianceAlerts()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the compliance preference.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Import/Export Path is not correct, so it will not be saved</exception>
        public CompliancePref GetCompliancePref()
        {
            CompliancePref pref = new CompliancePref();
            try
            {
                pref.PrePostCrossImportAllowed = ultraChckCrossImport.Checked;
                pref.InMarket = checkBoxInMarket.Checked;
                pref.InStage = checkBoxInStage.Checked;
                pref.PostInMarket = checkBoxPostInMarket.Checked;
                pref.PostInStage = checkBoxPostInStage.Checked;
                pref.BlockTradeOnComplianceFaliure = chkBlockOnComplianceFailure.Checked;
                pref.StageValueFromField = checkBoxStageValueFromField.Checked;
                pref.StageValueFromFieldString = txtBoxStageFieldValue.Text;
                pref.IsBasketComplianceEnabledCompany = chbEnableBasketCompliance.Checked;
                ComplianceCacheManager.UpdateCompliancePref(pref);
                if (String.IsNullOrEmpty(ultraTxtImportExportPath.Text) || Directory.Exists(ultraTxtImportExportPath.Text))
                {
                    pref.ImportExportPath = ultraTxtImportExportPath.Text;
                }
                else
                    throw new Exception("Import/Export Path is not correct, so it will not be saved");

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return pref;
        }

        /// <summary>
        /// Sets the compliance preference.
        /// </summary>
        /// <param name="pref">The preference.</param>
        public void SetCompliancePref(CompliancePref pref)
        {
            try
            {
                ultraTxtImportExportPath.Text = pref.ImportExportPath;
                ultraChckCrossImport.Checked = pref.PrePostCrossImportAllowed;
                checkBoxInMarket.Checked = pref.InMarket;
                checkBoxInStage.Checked = pref.InStage;
                checkBoxPostInMarket.Checked = pref.PostInMarket;
                checkBoxPostInStage.Checked = pref.PostInStage;
                chkBlockOnComplianceFailure.Checked = pref.BlockTradeOnComplianceFaliure;
                checkBoxStageValueFromField.Checked = pref.StageValueFromField;
                txtBoxStageFieldValue.Text = pref.StageValueFromFieldString;
                chbEnableBasketCompliance.Checked = pref.IsBasketComplianceEnabledCompany;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Checkbox InMarket check state change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxInMarket_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxInMarket.CheckState.Equals(CheckState.Unchecked))
                {
                    checkBoxInStage.Checked = false;
                    checkBoxPostInMarket.Checked = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Checkbox InStage check state change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxInStage_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxInStage.CheckState.Equals(CheckState.Checked))
                    checkBoxInMarket.Checked = true;
                else if (checkBoxInStage.CheckState.Equals(CheckState.Unchecked))
                    checkBoxPostInStage.Checked = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBoxPostInStage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void checkBoxPostInStage_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxPostInStage.CheckState.Equals(CheckState.Checked))
                {
                    checkBoxInStage.Checked = true;
                    checkBoxPostInMarket.Checked = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBoxStageValueFromFields control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void checkBoxStageValueFromField_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtBoxStageFieldValue.Enabled = checkBoxStageValueFromField.Checked;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBoxPostInMarket control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void checkBoxPostInMarket_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxPostInMarket.CheckState.Equals(CheckState.Checked))
                    checkBoxInMarket.Checked = true;
                else if (checkBoxPostInMarket.CheckState.Equals(CheckState.Unchecked))
                    checkBoxPostInStage.Checked = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
