using Prana.ActivityHandler.DAL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ActivityHandler.Generators
{
    internal class FixedIncomeActivityGenerator : TradingActivityGenerator
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
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.BondLongAddition.ToString());
                            else if (t.TransactionType == Convert.ToString(TradingTransactionType.LongWithdrawal))
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.BondLongWithdrawal.ToString());
                            else
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.BondL.ToString());
                        }
                        else
                        {
                            if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortAddition))
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.BondShortAddition.ToString());
                            else if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortWithdrawal))
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.BondShortWithdrawal.ToString());
                            else
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.BondS.ToString());
                        }
                        cashActivity.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity.ActivityType);
                        if (cashActivity.ActivityTypeId != int.MinValue)
                        {
                            cashActivity.ActivitySource = CachedDataManager.GetActivitySource(cashActivity.ActivityTypeId);
                            lsCashActivity.Add(cashActivity);
                        }

                        CashActivity cashActivity2 = DeepCopyHelper.Clone(cashActivity);
                        cashActivity2.Amount = Convert.ToDecimal(t.AccruedInterest);
                        cashActivity2.ActivityNumber = 2;
                        cashActivity2.UniqueKey = cashActivity2.GetKey();
                        cashActivity2.ActivityId = uIDGenerator.GenerateID();
                        cashActivity2.ClearingFee = 0;
                        cashActivity2.Commission = 0;
                        cashActivity2.SoftCommission = 0;
                        cashActivity2.MiscFees = 0;
                        cashActivity2.OtherBrokerFees = 0;
                        cashActivity2.ClearingBrokerFee = 0;
                        cashActivity2.StampDuty = 0;
                        cashActivity2.TaxOnCommissions = 0;
                        cashActivity2.TransactionLevy = 0;
                        cashActivity2.SecFee = 0;
                        cashActivity2.OccFee = 0;
                        cashActivity2.OrfFee = 0;
                        cashActivity2.OptionPremiumAdjustment = 0;

                        if (t.SideMultiplier > 0)
                        {
                            if (string.Equals(t.OrderSideTagValue, FIXConstants.SIDE_Buy))
                            {
                                cashActivity2.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Trading_Interest.ToString());
                                //http://jira.nirvanasolutions.com:8080/browse/PRANA-8593
                                cashActivity2.Amount *= -1;
                            }
                            else if (string.Equals(t.OrderSideTagValue, FIXConstants.SIDE_Buy_Closed))
                                cashActivity2.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Paid.ToString());
                            else
                                cashActivity2.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Receivable.ToString());
                        }
                        else
                        {
                            if (string.Equals(t.OrderSideTagValue, FIXConstants.SIDE_SellShort))
                                cashActivity2.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Trading_Payable.ToString());
                            else
                                cashActivity2.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Received.ToString());
                        }
                        cashActivity2.BalanceType = (BalanceType)CachedDataManager.GetBalanceTypeID(cashActivity2.ActivityType);
                        cashActivity2.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity2.ActivityType);
                        if (cashActivity2.ActivityTypeId != int.MinValue)
                        {
                            cashActivity2.ActivitySource = CachedDataManager.GetActivitySource(cashActivity2.ActivityTypeId);
                            lsCashActivity.Add(cashActivity2);
                        }
                        break;

                    case CashTransactionType.DailyCalculation:
                        TaxLot taxlot = data as TaxLot;
                        if (taxlot.SideMultiplier > 0)
                        {
                            cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Receivable.ToString());
                        }
                        else
                        {
                            if (taxlot.LongOrShort.Equals(PositionTag.Long))
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Receivable.ToString());
                            else
                                cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Payable.ToString());
                        }
                        cashActivity.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity.ActivityType);
                        cashActivity.Amount = Convert.ToDecimal(taxlot.AccruedInterest);
                        cashActivity.BalanceType = (BalanceType)CachedDataManager.GetBalanceTypeID(cashActivity.ActivityType);
                        if (cashActivity.ActivityTypeId != int.MinValue)
                        {
                            cashActivity.ActivitySource = CachedDataManager.GetActivitySource(cashActivity.ActivityTypeId);
                            lsCashActivity.Add(cashActivity);
                        }

                        if (taxlot.AUECLocalDate > taxlot.SettlementDate)
                        {
                            taxlot.SettlementDate = taxlot.AUECLocalDate;
                            DateTime NextCouponPayDate = ServiceProxyConnector.FixedIncomeAdapter.GetNextCouponPayDate(taxlot, taxlot.AUECLocalDate.Date);
                            if (taxlot.AUECLocalDate.Date.Equals(NextCouponPayDate))
                            {
                                CreateCashActivityForCouponDate(cashActivity, lsCashActivity, taxlot);
                            }
                            //handling the case where next coupon date comes before the closing settlement date
                            //https://jira.nirvanasolutions.com:8443/browse/PRANA-35245
                            else if (taxlot.ClosingSettlementDate != DateTimeConstants.MinValue && taxlot.ClosingTradeDate != DateTimeConstants.MinValue && NextCouponPayDate > taxlot.ClosingTradeDate && NextCouponPayDate < taxlot.ClosingSettlementDate)
                            {
                                taxlot.SettlementDate = NextCouponPayDate;
                                CreateCashActivityForCouponDate(cashActivity, lsCashActivity, taxlot);
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

        /// <summary>
        /// Create CashActivity For CouponDate
        /// </summary>
        /// <param name="cashActivity"></param>
        /// <param name="lsCashActivity"></param>
        /// <param name="taxlot"></param>
        private static void CreateCashActivityForCouponDate(CashActivity cashActivity, List<CashActivity> lsCashActivity, TaxLot taxlot)
        {
            try
            {
                CashActivity cashActivit2 = DeepCopyHelper.Clone(cashActivity);
                if (taxlot.SideMultiplier > 0)
                {
                    cashActivit2.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Received.ToString());
                    cashActivit2.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivit2.ActivityType);
                    cashActivit2.BalanceType = (BalanceType)CachedDataManager.GetBalanceTypeID(cashActivit2.ActivityType);
                }
                else
                {
                    if (taxlot.LongOrShort.Equals(PositionTag.Long))
                    {
                        cashActivit2.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Received.ToString());
                        cashActivit2.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivit2.ActivityType);
                        cashActivit2.BalanceType = (BalanceType)CachedDataManager.GetBalanceTypeID(cashActivit2.ActivityType);
                    }
                    else
                    {
                        cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Paid.ToString());
                        cashActivit2.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity.ActivityType);
                        cashActivit2.BalanceType = (BalanceType)CachedDataManager.GetBalanceTypeID(cashActivity.ActivityType);
                    }
                }

                cashActivit2.Amount = Convert.ToDecimal(ServiceProxyConnector.FixedIncomeAdapter.CalculateAccruedInterest(taxlot));
                cashActivit2.ActivityNumber = 2;
                cashActivit2.UniqueKey = cashActivit2.GetKey();
                //  if (t.TaxLotState == Prana.Global.ApplicationConstants.TaxLotState.New || t.TaxLotState == Prana.Global.ApplicationConstants.TaxLotState.NotChanged)
                cashActivit2.ActivityId = uIDGenerator.GenerateID();
                if (cashActivit2.ActivityTypeId != int.MinValue)
                {
                    cashActivit2.ActivitySource = CachedDataManager.GetActivitySource(cashActivit2.ActivityTypeId);
                    lsCashActivity.Add(cashActivit2);
                }
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
