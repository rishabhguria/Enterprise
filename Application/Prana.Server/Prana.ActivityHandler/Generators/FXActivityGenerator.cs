using Prana.ActivityHandler.Helpers;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ActivityHandler.Generators
{
    internal class FXActivityGenerator : TradingActivityGenerator
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
                            if (t.LeadCurrencyID.Equals(t.CurrencyID))
                                cashActivity.Amount = Convert.ToDecimal(t.TaxLotQty * t.AvgPrice * t.SideMultiplier);
                            else
                                cashActivity.Amount = Convert.ToDecimal(t.TaxLotQty / t.AvgPrice * t.SideMultiplier);

                            cashActivity.ClosedQty = Convert.ToDecimal(t.TaxLotQty);
                            cashActivity.CurrencyID = t.CurrencyID;
                            cashActivity.LeadCurrencyID = t.LeadCurrencyID;
                            cashActivity.VsCurrencyID = t.VsCurrencyID;
                            if (t.LongOrShort.Equals(PositionTag.Long))
                            {
                                if (t.TransactionType == Convert.ToString(TradingTransactionType.LongAddition))
                                {
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXLongAddition.ToString());
                                }
                                else if (t.TransactionType == Convert.ToString(TradingTransactionType.LongWithdrawal))
                                {
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXLongWithdrawal.ToString());
                                }
                                else
                                {
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXL.ToString());
                                }
                            }
                            else
                            {
                                if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortAddition))
                                {
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXShortAddition.ToString());
                                }
                                else if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortWithdrawal))
                                {
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXShortWithdrawal.ToString());
                                }
                                else
                                {
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXS.ToString());
                                }
                            }
                            cashActivity.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity.ActivityType);

                            if (Convert.ToString(t.FXConversionMethodOperator).Equals(Operator.D.ToString()))
                                cashActivity.FXConversionMethodOperator = Operator.M.ToString();

                            ActivityGeneratorHelper.SetActivityFxRate(cashActivity, t.AvgPrice);

                            cashActivity.avgPrice = t.AvgPrice;

                            if (cashActivity.ActivityTypeId != int.MinValue)
                            {
                                cashActivity.ActivitySource = CachedDataManager.GetActivitySource(cashActivity.ActivityTypeId);
                                lsCashActivity.Add(cashActivity);
                            }
                        }
                        break;

                    case CashTransactionType.Closing:
                        Position position = data as Position;
                        CashActivity cashActivity1 = new CashActivity();

                        cashActivity1.FKID = position.ClosingID;
                        //from closing state always remains new as deletion case is being handled from allocation(as TransactionSource.Trading)     
                        cashActivity1.ActivityState = (position.PositionState.Equals(Prana.Global.ApplicationConstants.TaxLotState.Updated) ? position.PositionState : Prana.Global.ApplicationConstants.TaxLotState.New);
                        cashActivity1.ActivityId = uIDGenerator.GenerateID();
                        cashActivity1.AccountID = position.AccountValue.ID;
                        cashActivity1.CurrencyID = CachedDataManager.GetInstance.GetCurrencyID(position.Currency);
                        cashActivity1.Symbol = position.Symbol;
                        cashActivity1.TransactionSource = CashTransactionSource;
                        cashActivity1.Date = position.ClosingTradeDate;
                        cashActivity1.ClosedQty = Convert.ToDecimal(position.ClosedQty);
                        cashActivity1.LeadCurrencyID = position.LeadCurrencyID;
                        cashActivity1.VsCurrencyID = position.VsCurrencyID;
                        cashActivity1.ActivityNumber = 1;
                        //Narendra Kumar jangir
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-2211
                        //before fkid was after the UniqueKey so unique key was not generating correctly because UniqueKey uses fkid
                        //cashActivity.FKID = cashActivity.TaxlotID;

                        int sideMultipier = -1;
                        string orderSide = CommonDataCache.TagDatabaseManager.GetInstance.GetOrderSideValue(position.Side);
                        if (orderSide == FIXConstants.SIDE_Buy || orderSide == FIXConstants.SIDE_Buy_Open || orderSide == FIXConstants.SIDE_Buy_Closed
                            || orderSide == FIXConstants.SIDE_Buy_Cover)
                            sideMultipier = 1;

                        cashActivity1.SideMultiplier = sideMultipier;

                        PositionTag LongOrShort = PositionTag.Short;
                        if (orderSide == FIXConstants.SIDE_Buy || orderSide == FIXConstants.SIDE_Sell || orderSide == FIXConstants.SIDE_Buy_Open
                            || orderSide == FIXConstants.SIDE_Sell_Closed)
                            LongOrShort = PositionTag.Long;

                        #region Activity Rule

                        //For Fx and FX Forward manual closing, realized PnL entry is now generated from revaluation, PRANA-31873
                        if (position.ClosingMode != ClosingMode.Offset)
                        {
                            if (cashActivity1.LeadCurrencyID.Equals(cashActivity1.CurrencyID))
                                cashActivity1.Amount = Convert.ToDecimal(position.OpenAveragePrice * position.ClosedQty);

                            else
                                cashActivity1.Amount = Convert.ToDecimal(position.ClosedQty / position.OpenAveragePrice);

                            cashActivity1.PnL = 0;
                            cashActivity1.FXPnL = 0;
                            cashActivity1.UniqueKey = cashActivity1.GetKey();
                            if (LongOrShort.Equals(PositionTag.Long))
                            {
                                cashActivity1.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXL_CurrencySettled.ToString());
                                cashActivity1.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity1.ActivityType);
                            }
                            else
                            {
                                cashActivity1.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXS_CurrencySettled.ToString());
                                cashActivity1.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity1.ActivityType);
                            }
                            ActivityGeneratorHelper.SetActivityFxRate(cashActivity1, position.OpenAveragePrice);
                            cashActivity1.FXConversionMethodOperator = Operator.M.ToString();
                            cashActivity1.avgPrice = position.OpenAveragePrice;

                            CashActivity cashActivity2 = DeepCopyHelper.Clone(cashActivity1);
                            //TODO: make following code clean
                            cashActivity2.ActivityId = uIDGenerator.GenerateID();
                            cashActivity2.ActivityNumber = 2;
                            cashActivity2.UniqueKey = cashActivity2.GetKey();
                            cashActivity2.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FX_Settled.ToString());
                            cashActivity2.ActivityTypeId = CachedDataManager.GetActivityTypeID(cashActivity2.ActivityType);

                            ActivityGeneratorHelper.SetActivityFxRate(cashActivity2, (position.ClosedAveragePrice == 0.0) ? position.OpenAveragePrice : position.ClosedAveragePrice);
                            cashActivity2.FXConversionMethodOperator = Operator.M.ToString();
                            cashActivity2.avgPrice = position.ClosedAveragePrice;
                            cashActivity2.PnL = Convert.ToDecimal(position.CostBasisGrossPNL);

                            if (cashActivity1.ActivityTypeId != int.MinValue)
                            {
                                cashActivity1.ActivitySource = CachedDataManager.GetActivitySource(cashActivity1.ActivityTypeId);
                                lsCashActivity.Add(cashActivity1);
                            }
                            if (cashActivity2.ActivityTypeId != int.MinValue)
                            {
                                cashActivity2.ActivitySource = CachedDataManager.GetActivitySource(cashActivity2.ActivityTypeId);
                                lsCashActivity.Add(cashActivity2);
                            }
                        }
                        #endregion
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
