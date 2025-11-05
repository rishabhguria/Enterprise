using Prana.ActivityHandler.Helpers;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ActivityHandler.Generators
{
    internal class PrivateEquityActivityGenerator : TradingActivityGenerator
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
                switch (CashTransactionSource)
                {
                    case CashTransactionType.Trading:
                    case CashTransactionType.TradeImport:
                        TaxLot t = data as TaxLot;
                        if (t.LongOrShort.Equals(PositionTag.Long))
                        {
                            if (t.TransactionType == Convert.ToString(TradingTransactionType.LongAddition))
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.PrivateEquityLongAddition.ToString());

                            }
                            else if (t.TransactionType == Convert.ToString(TradingTransactionType.LongWithdrawal))
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.PrivateEquityLongWithdrawal.ToString());
                            }
                            else
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.PrivateEquityL.ToString());
                            }
                        }
                        else
                        {
                            if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortAddition))
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.PrivateEquityShortAddition.ToString());
                            }
                            else if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortWithdrawal))
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.PrivateEquityShortWithdrawal.ToString());
                            }
                            else
                            {
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.PrivateEquityS.ToString());
                            }
                        }
                        cashActivity.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity.ActivityType);
                        if (t.ISSwap && !ActivityGeneratorHelper.IsSwapJournalAllowed())
                        {
                            cashActivity.Amount = 0;
                        }
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
