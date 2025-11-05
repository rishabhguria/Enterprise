using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ActivityHandler.Generators
{
    internal class FXOptionActivityGenerator : TradingActivityGenerator
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
                    case CashTransactionType.DailyCalculation:
                    case CashTransactionType.TradeImport:
                        TaxLot t = data as TaxLot;
                        //this check is required,becouse Fx/FxForword settlement journal entries are being created through closing Module
                        if (t.ClosingMode == ClosingMode.None)
                        {
                            if (t.LongOrShort.Equals(PositionTag.Long))
                            {
                                if (t.TransactionType == Convert.ToString(TradingTransactionType.LongAddition))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXOptionLongAddition.ToString());
                                else if (t.TransactionType == Convert.ToString(TradingTransactionType.LongWithdrawal))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXOptionLongWithdrawal.ToString());
                                else
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXOptionL.ToString());
                            }
                            else
                            {
                                if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortAddition))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXOptionShortAddition.ToString());
                                else if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortWithdrawal))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXOptionShortWithdrawal.ToString());
                                else
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXOptionS.ToString());
                            }
                            cashActivity.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity.ActivityType);

                            if (cashActivity.ActivityTypeId != int.MinValue)
                            {
                                cashActivity.ActivitySource = CachedDataManager.GetActivitySource(cashActivity.ActivityTypeId);
                                lsCashActivity.Add(cashActivity);
                            }
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
