using Prana.ActivityHandler.DAL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ActivityHandler.Generators
{
    internal class FutureActivityGenerator : TradingActivityGenerator
    {
        /// <summary>
        /// Updates the activity asset wise.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="cashActivity">The cash activity.</param>
        /// <returns></returns>
        internal override List<CashActivity> UpdateActivityAssetWise<T>(T data, CashActivity cashActivity)
        {
            List<CashActivity> lsCashActivity = new List<CashActivity>();
            try
            {
                //Narendra Kumar Jangir, Aug 27 2013
                //For PNL handling of futures, generate trade activity for future if closing mode is none
                //when a futute is settled/expired then an trade with opposite side also generate
                //Do not make trade activity for that trade
                switch (CashTransactionSource)
                {
                    case CashTransactionType.Trading:
                    case CashTransactionType.TradeImport:
                        TaxLot t = data as TaxLot;
                        // Modified By : Manvendra P.
                        // Jira : http://jira.nirvanasolutions.com:8080/browse/CHMW-3668
                        decimal margin = 0;
                        if (ServiceProxyConnector.CashManagementServices.GetCashPreferences().ContainsKey(t.Level1ID) && ServiceProxyConnector.CashManagementServices.GetCashPreferences()[t.Level1ID] != null)
                        {
                            margin = Convert.ToDecimal(ServiceProxyConnector.CashManagementServices.GetCashPreferences()[t.Level1ID].MarginPercentage / 100);
                        }
                        if (t.LongOrShort.Equals(PositionTag.Long))
                        {
                            if (t.TransactionType == Convert.ToString(TradingTransactionType.LongAddition))
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FutureLongAddition.ToString());
                            }
                            else if (t.TransactionType == Convert.ToString(TradingTransactionType.LongWithdrawal))
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FutureLongWithdrawal.ToString());
                            }
                            else
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FutureL.ToString());
                            }
                        }
                        else
                        {
                            if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortAddition))
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FutureShortAddition.ToString());
                            }
                            else if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortWithdrawal))
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FutureShortWithdrawal.ToString());
                            }
                            else
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FutureS.ToString());
                            }
                        }
                        cashActivity.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity.ActivityType);
                        cashActivity.Amount = (Convert.ToDecimal(t.AvgPrice * t.TaxLotQty * t.ContractMultiplier)) * margin;

                        if (cashActivity.ActivityTypeId != int.MinValue)
                        {
                            cashActivity.ActivitySource = CachedDataManager.GetActivitySource(cashActivity.ActivityTypeId);
                            lsCashActivity.Add(cashActivity);
                        }
                        break;

                    case CashTransactionType.DailyCalculation:
                        TaxLot taxlot = data as TaxLot;
                        //Modified by: Bharat Raturi, 2015 04 Feb
                        //Correct sub accounts for daily calculation
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6090
                        if (taxlot.M2MProfitLoss >= 0)
                        {
                            cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.M2MProfit.ToString());
                            cashActivity.ActivityTypeId = CachedDataManager.GetActivityTypeID(Activities.M2MProfit.ToString());
                        }
                        else
                        {
                            cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.M2MLoss.ToString());
                            cashActivity.ActivityTypeId = CachedDataManager.GetActivityTypeID(Activities.M2MLoss.ToString());
                        }
                        cashActivity.Amount = Convert.ToDecimal(taxlot.M2MProfitLoss);
                        if (cashActivity.ActivityTypeId != int.MinValue)
                        {
                            cashActivity.ActivitySource = CachedDataManager.GetActivitySource(cashActivity.ActivityTypeId);
                            lsCashActivity.Add(cashActivity);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsCashActivity;
        }
    }
}
