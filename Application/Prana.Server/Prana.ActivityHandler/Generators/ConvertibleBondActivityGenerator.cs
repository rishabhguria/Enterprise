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
    internal class ConvertibleBondActivityGenerator : TradingActivityGenerator
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
                        //in case of Buy, here will be 3 transactions
                        if (t.SideMultiplier > 0)
                        {
                            if (t.LongOrShort.Equals(PositionTag.Long))
                            {
                                if (t.TransactionType == Convert.ToString(TradingTransactionType.LongAddition))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.ConvertibleBondLongAddition.ToString());
                                else if (t.TransactionType == Convert.ToString(TradingTransactionType.LongWithdrawal))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.ConvertibleBondLongWithdrawal.ToString());
                                else
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.ConvertibleBondL.ToString());

                            }
                            else
                            {
                                if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortAddition))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.ConvertibleBondShortAddition.ToString());
                                else if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortWithdrawal))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.ConvertibleBondShortWithdrawal.ToString());
                                else
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.ConvertibleBondS.ToString());
                            }
                            cashActivity.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity.ActivityType);
                            if (cashActivity.ActivityTypeId != int.MinValue)
                            {
                                cashActivity.ActivitySource = CachedDataManager.GetActivitySource(cashActivity.ActivityTypeId);
                                lsCashActivity.Add(cashActivity);
                            }

                            CashActivity cashActivity2 = DeepCopyHelper.Clone(cashActivity);
                            cashActivity2.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Expense.ToString());
                            cashActivity2.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity2.ActivityType);
                            cashActivity2.Amount = Convert.ToDecimal(t.AccruedInterest);
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-8593
                            if (string.Equals(t.OrderSideTagValue, FIXConstants.SIDE_Buy) || string.Equals(t.OrderSideTagValue, FIXConstants.SIDE_Buy_Closed) || string.Equals(t.OrderSideTagValue, FIXConstants.SIDE_Buy_Open) || string.Equals(t.OrderSideTagValue, FIXConstants.SIDE_Buy_Cover))
                                cashActivity2.Amount *= -1;
                            cashActivity2.ActivityNumber = 2;
                            cashActivity2.UniqueKey = cashActivity2.GetKey();
                            cashActivity2.BalanceType = (BalanceType)CachedDataManager.GetBalanceTypeID(cashActivity2.ActivityType);
                            cashActivity2.ActivityId = uIDGenerator.GenerateID();
                            if (cashActivity2.ActivityTypeId != int.MinValue)
                            {
                                cashActivity2.ActivitySource = CachedDataManager.GetActivitySource(cashActivity2.ActivityTypeId);
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
                                lsCashActivity.Add(cashActivity2);
                            }

                            CashActivity cashActivity3 = DeepCopyHelper.Clone(cashActivity);
                            cashActivity3.Amount = Convert.ToDecimal(t.AccruedInterest);
                            cashActivity3.ActivityNumber = 3;
                            cashActivity3.UniqueKey = cashActivity3.GetKey();
                            cashActivity3.ActivityId = uIDGenerator.GenerateID();
                            cashActivity3.ClearingFee = 0;
                            cashActivity3.Commission = 0;
                            cashActivity3.SoftCommission = 0;
                            cashActivity3.MiscFees = 0;
                            cashActivity3.OtherBrokerFees = 0;
                            cashActivity3.ClearingBrokerFee = 0;
                            cashActivity3.StampDuty = 0;
                            cashActivity3.TaxOnCommissions = 0;
                            cashActivity3.TransactionLevy = 0;
                            cashActivity3.SecFee = 0;
                            cashActivity3.OccFee = 0;
                            cashActivity3.OrfFee = 0;
                            cashActivity3.OptionPremiumAdjustment = 0;

                            cashActivity3.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Receivable.ToString());
                            cashActivity3.BalanceType = (BalanceType)CachedDataManager.GetBalanceTypeID(cashActivity3.ActivityType);
                            cashActivity3.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity3.ActivityType);
                            if (cashActivity3.ActivityTypeId != int.MinValue)
                            {
                                cashActivity3.ActivitySource = CachedDataManager.GetActivitySource(cashActivity3.ActivityTypeId);
                                lsCashActivity.Add(cashActivity3);
                            }
                        }
                        else
                        {
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

                            cashActivity2.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Received.ToString());
                            cashActivity2.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity2.ActivityType);
                            cashActivity2.BalanceType = (BalanceType)CachedDataManager.GetBalanceTypeID(cashActivity2.ActivityType);
                            if (cashActivity2.ActivityTypeId != int.MinValue)
                            {
                                cashActivity2.ActivitySource = CachedDataManager.GetActivitySource(cashActivity2.ActivityTypeId);
                                lsCashActivity.Add(cashActivity2);
                            }
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
                        //If coupon Paydate //TODO: If payout date is before settlement date (it has to be resolved)
                        if (taxlot.AUECLocalDate > taxlot.SettlementDate)
                        {
                            taxlot.SettlementDate = taxlot.AUECLocalDate;
                            if (taxlot.AUECLocalDate.Date.Equals(ServiceProxyConnector.FixedIncomeAdapter.GetNextCouponPayDate(taxlot, taxlot.AUECLocalDate.Date)))
                            {
                                CashActivity cashActivity2 = DeepCopyHelper.Clone(cashActivity);
                                if (taxlot.SideMultiplier > 0)
                                {
                                    cashActivity2.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Received.ToString());
                                    cashActivity2.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity2.ActivityType);
                                    cashActivity2.BalanceType = (BalanceType)CachedDataManager.GetBalanceTypeID(cashActivity2.ActivityType);
                                }
                                else
                                {
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Paid.ToString());
                                    cashActivity2.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity.ActivityType);
                                    cashActivity2.BalanceType = (BalanceType)CachedDataManager.GetBalanceTypeID(cashActivity.ActivityType);

                                }
                                cashActivity2.Amount = Convert.ToDecimal(ServiceProxyConnector.FixedIncomeAdapter.CalculateAccruedInterest(taxlot));

                                cashActivity2.ActivityNumber = 2;
                                cashActivity2.UniqueKey = cashActivity2.GetKey();
                                //  if (t.TaxLotState == Prana.Global.ApplicationConstants.TaxLotState.New || t.TaxLotState == Prana.Global.ApplicationConstants.TaxLotState.NotChanged)
                                cashActivity2.ActivityId = uIDGenerator.GenerateID();
                                if (cashActivity2.ActivityTypeId != int.MinValue)
                                {
                                    cashActivity2.ActivitySource = CachedDataManager.GetActivitySource(cashActivity2.ActivityTypeId);
                                    lsCashActivity.Add(cashActivity2);
                                }
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
