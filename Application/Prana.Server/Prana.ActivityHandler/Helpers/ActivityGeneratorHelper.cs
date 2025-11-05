using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Configuration;

namespace Prana.ActivityHandler.Helpers
{
    internal static class ActivityGeneratorHelper
    {
        /// <summary>
        /// Sets the activity fx rate.
        /// </summary>
        /// <param name="cashActivity">The cash activity.</param>
        /// <param name="price">The price.</param>
        internal static void SetActivityFxRate(CashActivity cashActivity, double price)
        {
            try
            {
                if (cashActivity.LeadCurrencyID.Equals(CachedDataManager.GetInstance.GetCompanyBaseCurrencyID()))
                {
                    if (price > 0)
                        cashActivity.FXRate = 1 / price;
                    else
                        cashActivity.FXRate = price;
                }
                else
                    cashActivity.FXRate = 1; //considering vs currency to be base currency
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Update commission and fees in activity
        /// </summary>
        /// <param name="cashActivity">The cash activity</param>
        /// <param name="t">Taxlot</param>
        internal static void UpdateCommissionAndFees(CashActivity cashActivity, TaxLot t)
        {
            try
            {
                //raturi: Commission details should not be added if swap is being traded 
                //and gross amount has not be shown on balance sheet
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-8591
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-8863
                if (!t.ISSwap || (t.ISSwap && IsSwapJournalAllowed()))
                {
                    cashActivity.ClearingFee = Convert.ToDecimal(t.ClearingFee);
                    cashActivity.Commission = Convert.ToDecimal(t.Commission);
                    cashActivity.SoftCommission = Convert.ToDecimal(t.SoftCommission);
                    cashActivity.MiscFees = Convert.ToDecimal(t.MiscFees);
                    cashActivity.OtherBrokerFees = Convert.ToDecimal(t.OtherBrokerFees);
                    cashActivity.ClearingBrokerFee = Convert.ToDecimal(t.ClearingBrokerFee);
                    cashActivity.StampDuty = Convert.ToDecimal(t.StampDuty);
                    cashActivity.TaxOnCommissions = Convert.ToDecimal(t.TaxOnCommissions);
                    cashActivity.TransactionLevy = Convert.ToDecimal(t.TransactionLevy);
                    cashActivity.SecFee = Convert.ToDecimal(t.SecFee);
                    cashActivity.OccFee = Convert.ToDecimal(t.OccFee);
                    cashActivity.OrfFee = Convert.ToDecimal(t.OrfFee);
                    cashActivity.OptionPremiumAdjustment = Convert.ToDecimal(t.OptionPremiumAdjustment);
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
        /// Check if the Swap journal is not allowed
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-8863
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is swap journal allowed]; otherwise, <c>false</c>.
        /// </returns>
        internal static bool IsSwapJournalAllowed()
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["IsSwapJournalAllowed"]))
                return Convert.ToBoolean(ConfigurationManager.AppSettings["IsSwapJournalAllowed"]);
            return false;
        }
    }
}
