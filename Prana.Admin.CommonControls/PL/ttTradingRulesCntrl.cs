using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.TTPrefs;
using Prana.ClientPreferences;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Admin.CommonControls.PL
{
    public partial class ttTradingRulesCntrl : UserControl
    {
        public ttTradingRulesCntrl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Setups the control.
        /// </summary>
        public void SetupControl()
        {
            BindAbsoluteAmountOrDefinePercent();
            BindAccountOrMfCombo();
            SetTradingRulesPreferencesOnUI();
        }

        /// <summary>
        /// Binds the account or mf combo.
        /// </summary>
        private void BindAccountOrMfCombo()
        {
            try
            {
                List<EnumerationValue> ds = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PTTMasterFundOrAccount));
                cmbFatFingerAccountOrMasterFund.DataSource = ds;
                cmbFatFingerAccountOrMasterFund.DataBind();
                cmbFatFingerAccountOrMasterFund.DisplayMember = "DisplayText";
                cmbFatFingerAccountOrMasterFund.ValueMember = "Value";
                cmbFatFingerAccountOrMasterFund.DropDownStyle = DropDownStyle.DropDownList;

                List<EnumerationValue> dsForShareOutstanding = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(FundSelectionType));
                cmbSharesOutstandingAccountOrMF.DataSource = dsForShareOutstanding;
                cmbSharesOutstandingAccountOrMF.DataBind();
                cmbSharesOutstandingAccountOrMF.DisplayMember = "DisplayText";
                cmbSharesOutstandingAccountOrMF.ValueMember = "Value";
                cmbSharesOutstandingAccountOrMF.DropDownStyle = DropDownStyle.DropDownList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Binds the absolute amount or Define Percent
        /// </summary>
        private void BindAbsoluteAmountOrDefinePercent()
        {
            try
            {
                List<EnumerationValue> ds = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(AbsoluteAmountOrDefinePercent));
                cmbFatFingerAbsoluteAmountOrDefinePercent.DataSource = ds;
                cmbFatFingerAbsoluteAmountOrDefinePercent.DataBind();
                cmbFatFingerAbsoluteAmountOrDefinePercent.DisplayMember = "DisplayText";
                cmbFatFingerAbsoluteAmountOrDefinePercent.ValueMember = "Value";
                cmbFatFingerAbsoluteAmountOrDefinePercent.DropDownStyle = DropDownStyle.DropDownList;
            }

            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// sets the values of trading rules preferences in UI
        /// </summary>
        /// <returns></returns>
        private void SetTradingRulesPreferencesOnUI()
        {
            try
            {
                bool? isInMarketIncluded = TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.IsInMarketIncluded;
                if (isInMarketIncluded != null && (bool)isInMarketIncluded)
                    rdbtnInMarketYes.Checked = true;
                else
                    rdbtnInMarketNo.Checked = true;
                bool? isSharesOutstandingRule = TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.IsSharesOutstandingRule;
                if (isSharesOutstandingRule != null && (bool)isSharesOutstandingRule)
                    rdbtnSharesOutstandingYes.Checked = true;
                else
                    rdbtnSharesOutstandingNo.Checked = true;
                bool? isOversellTradingRule = TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.IsOversellTradingRule;
                if (isOversellTradingRule != null && (bool)isOversellTradingRule)
                    rdbtnOversellYes.Checked = true;
                else
                    rdbtnOversellNo.Checked = true;
                bool? isOverbuyTradingRule = TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.IsOverbuyTradingRule;
                if (isOverbuyTradingRule != null && (bool)isOverbuyTradingRule)
                    rdbtnOverbuyYes.Checked = true;
                else
                    rdbtnOverbuyNo.Checked = true;
                bool? isUnallocatedTradeAlert = TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.IsUnallocatedTradeAlert;
                if (isUnallocatedTradeAlert != null && (bool)isUnallocatedTradeAlert)
                    rdbtnUnallocatedYes.Checked = true;
                else
                    rdbtnUnallocatedNo.Checked = true;
                bool? isFatFingerTradingRule = TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.IsFatFingerTradingRule;
                if (isFatFingerTradingRule != null && (bool)isFatFingerTradingRule)
                    rdbtnFatFingerYes.Checked = true;
                else
                    rdbtnFatFingerNo.Checked = true;
                bool? isDuplicateTradeAlert = TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.IsDuplicateTradeAlert;
                if (isDuplicateTradeAlert != null && (bool)isDuplicateTradeAlert)
                    rdbtnDuplicateTradeYes.Checked = true;
                else
                    rdbtnDuplicateTradeNo.Checked = true;
                bool? isPendingNewTradeAlert = TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.IsPendingNewTradeAlert;
                if (isPendingNewTradeAlert != null && (bool)isPendingNewTradeAlert)
                    rdbtnPendingNewYes.Checked = true;
                else
                    rdbtnPendingNewNo.Checked = true;
                decimal fatFingerPercentValue = 0;
                decimal duplicateTradeTimeValue = 0;
                decimal pendingNewTimeValue = 0;
                decimal sharesOutstandingValue = 0;
                decimal.TryParse(TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.DefineFatFingerValue.ToString(), out fatFingerPercentValue);
                decimal.TryParse(TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.DuplicateTradeAlertTime.ToString(), out duplicateTradeTimeValue);
                decimal.TryParse(TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.PendingNewOrderAlertTime.ToString(), out pendingNewTimeValue);
                decimal.TryParse(TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.SharesOutstandingValue.ToString(), out sharesOutstandingValue);
                fatFingerPercent.Maximum = decimal.MaxValue;
                fatFingerPercent.Value = fatFingerPercentValue;
                duplicateTradeTime.Value = duplicateTradeTimeValue;
                pendingNewTime.Value = pendingNewTimeValue;
                sharesOutstandingPercent.Maximum = decimal.MaxValue;
                sharesOutstandingPercent.Value = sharesOutstandingValue;
                if (TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.IsAbsoluteAmountOrDefinePercent != null)
                    cmbFatFingerAbsoluteAmountOrDefinePercent.SelectedIndex = (int)TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.IsAbsoluteAmountOrDefinePercent;
                if (TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.FatFingerAccountOrMasterFund != null)
                    cmbFatFingerAccountOrMasterFund.SelectedIndex = (int)TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.FatFingerAccountOrMasterFund;
                if (TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.SharesOutstandingAccOrMF != null)
                    cmbSharesOutstandingAccountOrMF.SelectedIndex = (int)TradingTicketPrefManager.GetInstance.TradingTicketRulesPrefs.SharesOutstandingAccOrMF;
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
        /// saves the values for trading rules preferences from UI
        /// </summary>
        /// <returns></returns>
        public TradingTicketRulesPrefs SaveTradingRulesPreferences()
        {
            TradingTicketRulesPrefs ttRulesPreferences = new TradingTicketRulesPrefs();
            try
            {
                ttRulesPreferences.IsInMarketIncluded = rdbtnInMarketYes.Checked ? true : false;
                ttRulesPreferences.IsSharesOutstandingRule = rdbtnSharesOutstandingYes.Checked ? true : false;
                ttRulesPreferences.IsOversellTradingRule = rdbtnOversellYes.Checked ? true : false;
                ttRulesPreferences.IsOverbuyTradingRule = rdbtnOverbuyYes.Checked ? true : false;
                ttRulesPreferences.IsUnallocatedTradeAlert = rdbtnUnallocatedYes.Checked ? true : false;
                ttRulesPreferences.IsFatFingerTradingRule = rdbtnFatFingerYes.Checked ? true : false;
                ttRulesPreferences.IsDuplicateTradeAlert = rdbtnDuplicateTradeYes.Checked ? true : false;
                ttRulesPreferences.IsPendingNewTradeAlert = rdbtnPendingNewYes.Checked ? true : false;
                ttRulesPreferences.DefineFatFingerValue = (double?)ReturnValueAfterConversion(MaskFatFingerValue(fatFingerPercent.Value), typeof(double));
                ttRulesPreferences.SharesOutstandingValue = (double?)ReturnValueAfterConversion(sharesOutstandingPercent.Value, typeof(double));
                ttRulesPreferences.DuplicateTradeAlertTime = (int?)ReturnValueAfterConversion(duplicateTradeTime.Value, typeof(Int32));
                ttRulesPreferences.PendingNewOrderAlertTime = (int?)ReturnValueAfterConversion(pendingNewTime.Value, typeof(Int32));
                ttRulesPreferences.FatFingerAccountOrMasterFund = (int?)ReturnValueAfterConversion(cmbFatFingerAccountOrMasterFund.Value, typeof(Int32));
                ttRulesPreferences.SharesOutstandingAccOrMF = (int?)ReturnValueAfterConversion(cmbSharesOutstandingAccountOrMF.Value, typeof(Int32));
                ttRulesPreferences.IsAbsoluteAmountOrDefinePercent = (int?)ReturnValueAfterConversion(cmbFatFingerAbsoluteAmountOrDefinePercent.Value, typeof(Int32));
                TradingTicketPrefManager.GetInstance.SaveTradingRulesPreference(ttRulesPreferences);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return ttRulesPreferences;
        }

        private decimal MaskFatFingerValue(decimal maskFatFingerValue)
        {
            if (cmbFatFingerAbsoluteAmountOrDefinePercent.Text.ToString().Equals(AbsoluteAmountOrDefinePercent.AbsoluteAmount.ToString()))
                maskFatFingerValue = Math.Round(maskFatFingerValue, 4);
            else
                maskFatFingerValue = Math.Round(maskFatFingerValue, 2);

            return maskFatFingerValue;
        }

        /// <summary>
        /// Returns the value after conversion.
        /// </summary>
        /// <param name="controlValue">The control value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static object ReturnValueAfterConversion(object controlValue, Type type)
        {
            object value = null;
            try
            {
                if (controlValue != null)
                {
                    if (type == typeof(Int32))
                    {
                        value = Convert.ToInt32(controlValue);
                    }
                    else if (type == typeof(Double))
                    {
                        value = Convert.ToDouble(controlValue);
                    }
                    else if ((type == typeof(Double)))
                    {
                        value = Convert.ToBoolean(controlValue);
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
            return value;
        }
        /// <summary>
        /// Update the maximum value of NumericUpDown box while selecting Absolute Amount from dropdown
        /// </summary>
        private void cmbFatFingerAbsoluteAmountOrDefinePercent_ValueChanged_2(object sender, EventArgs e)
        {
            if (cmbFatFingerAbsoluteAmountOrDefinePercent.Text.ToString().Equals(AbsoluteAmountOrDefinePercent.AbsoluteAmount.ToString()))
            {
                fatFingerPercent.Maximum = decimal.MaxValue;
                this.fatFingerPercent.DecimalPlaces = 4;

            }
            else
            {
                fatFingerPercent.Maximum = 100;
                this.fatFingerPercent.DecimalPlaces = 2;
            }
        }
    }
}
