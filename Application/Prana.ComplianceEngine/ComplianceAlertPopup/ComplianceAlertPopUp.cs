using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.ComplianceAlertPopup
{
    public partial class ComplianceAlertPopUp : Form
    {
        /// <summary>
        /// Is Trade Allowed
        /// </summary>
        public bool IsTradeAllowed
        {
            get { return complianceAlertPopupUC.IsTradeAllowed; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rulePackage"></param>
        public ComplianceAlertPopUp(RulePackage rulePackage = RulePackage.PreTrade)
        {
            try
            {
                InitializeComponent();
                complianceAlertPopupUC.CloseCompliancePopUp += new EventHandler(CloseCompliancePopUpForm);
                complianceAlertPopupUC.HideCompliancePopUp += HideCompliancePopUpForm;

                if (CustomThemeHelper.ApplyTheme)
                {
                    this.Text = (rulePackage == RulePackage.PostTrade ? ComplainceConstants.CONST_POST : ComplainceConstants.CONST_PRE) + ComplainceConstants.CONST_TRADE_COMPLIANCE_RESULTS;
                    complianceAlertPopupUC.SetThemeForUserControl();
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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
        /// Binding Compliance Alert Data
        /// </summary>
        /// <param name="alertPopUpType"></param>
        /// <param name="dataAlerts"></param>
        public void BindingComplianceAlertData(AlertPopUpType alertPopUpType, List<Alert> alertsList, bool isOnlyHardAlerts = false)
        {
            try
            {
                List<string> ListOfFieldData = new List<string>(ComplainceConstants.CONST_FieldDataStr.Split(','));
                foreach (Alert alert in alertsList)
                {
                    alert.Description = alert.Summary;
                    if (CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                        && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                    {
                        List<string> fieldsList = new List<string>();
                        foreach (var field in ListOfFieldData)
                        {
                            fieldsList.Add(field.ToLower());
                        }
                        if (!alert.ConstraintFields.Contains(ComplainceConstants.CONST_SEPARATOR_CHAR) && !fieldsList.Contains(alert.ConstraintFields.ToLower()) && !alert.ConstraintFields.Equals(ComplainceConstants.CONST_NA))
                        {
                            alert.ActualResult = ComplainceConstants.CONST_CensorValue;
                            alert.Threshold = ComplainceConstants.CONST_CensorValue;
                        }
                        alert.Parameters = ComplainceConstants.CONST_CensorValue;
                        alert.Description = ComplainceConstants.CONST_CensorValue;
                        alert.Summary = ComplainceConstants.CONST_CensorValue;
                    }
                }  
                complianceAlertPopupUC.BindingComplianceAlertData(alertPopUpType, alertsList, isOnlyHardAlerts);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// ComplianceAlertPopUp_FormClosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComplianceAlertPopUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //CloseReason is UserClosing: The user is closing the form through the user interface (UI), for example
                //by clicking the Close button on the form window, selecting Close from the
                if (e.CloseReason != CloseReason.UserClosing) return;
                e.Cancel = true;
                this.Visible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// GetUpdatedAlerts
        /// </summary>
        /// <returns></returns>
        public List<Alert> GetUpdatedAlerts()
        {
            return complianceAlertPopupUC.GetUpdatedAlerts(); ;
        }

        /// <summary>
        /// Intialises Properties of Columns and Alerts.
        /// </summary>
        /// <param name="userId"></param>
        public void InitialiseControlsForPostPopUp(int userId)
        {
            try
            {
                complianceAlertPopupUC.InitialiseControlsForPostPopUp(userId);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        ///  Ocurrs when new alert is receive and pop up need to be shown
        /// </summary>
        /// <param name="ruleType"></param>
        /// <param name="ruleName"></param>
        /// <param name="postdataAlerts"></param>
        public void NewAlertReceived(string ruleType, string ruleName, DataSet postdataAlerts)
        {
            try
            {
                complianceAlertPopupUC.NewAlertReceived(ruleType, ruleName, postdataAlerts);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// CloseCompliancePopUpForm
        /// </summary>
        private void CloseCompliancePopUpForm(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// CloseCompliancePopUpForm
        /// </summary>
        private void HideCompliancePopUpForm(object sender, EventArgs<bool> e)
        {
            this.Visible = e.Value;
        }
    }
}