using Prana.ActivityHandler.DAL;
using Prana.ActivityHandler.Helpers;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ActivityHandler.Generators
{
    internal class FXForwardActivityGenerator : TradingActivityGenerator
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
            Dictionary<int, CashPreferences> cashPreferences = ServiceProxyConnector.CashManagementServices.GetCashPreferences();
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
                            cashActivity.Amount = 0;
                            cashActivity.ClosedQty = Convert.ToDecimal(t.TaxLotQty);
                            cashActivity.CurrencyID = t.CurrencyID;
                            cashActivity.LeadCurrencyID = t.LeadCurrencyID;
                            cashActivity.VsCurrencyID = t.VsCurrencyID;

                            if (t.LongOrShort.Equals(PositionTag.Long))
                            {
                                if (t.TransactionType == Convert.ToString(TradingTransactionType.LongAddition))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXForwardLongAddition.ToString());
                                else if (t.TransactionType == Convert.ToString(TradingTransactionType.LongWithdrawal))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXForwardLongWithdrawal.ToString());
                                else
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXForwardL.ToString());
                            }
                            else
                            {
                                if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortAddition))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXForwardShortAddition.ToString());
                                else if (t.TransactionType == Convert.ToString(TradingTransactionType.ShortWithdrawal))
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXForwardShortWithdrawal.ToString());
                                else
                                    cashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FXForwardS.ToString());
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
                        CashActivity closingCashActivity = new CashActivity();

                        closingCashActivity.FKID = position.ClosingID;
                        //from closing state always remains new as deletion case is being handled from allocation(as TransactionSource.Trading)     
                        closingCashActivity.ActivityState = (position.PositionState.Equals(Prana.Global.ApplicationConstants.TaxLotState.Updated) ? position.PositionState : Prana.Global.ApplicationConstants.TaxLotState.New);
                        closingCashActivity.ActivityId = uIDGenerator.GenerateID();
                        closingCashActivity.AccountID = position.AccountValue.ID;
                        closingCashActivity.CurrencyID = CachedDataManager.GetInstance.GetCurrencyID(position.Currency);
                        closingCashActivity.Symbol = position.Symbol;
                        closingCashActivity.TransactionSource = CashTransactionSource;
                        closingCashActivity.Date = position.ClosingTradeDate;
                        closingCashActivity.ClosedQty = Convert.ToDecimal(position.ClosedQty);
                        closingCashActivity.LeadCurrencyID = position.LeadCurrencyID;
                        closingCashActivity.VsCurrencyID = position.VsCurrencyID;
                        closingCashActivity.ActivityNumber = 1;


                        int sideMultipier = -1;
                        string orderSide = CommonDataCache.TagDatabaseManager.GetInstance.GetOrderSideValue(position.Side);
                        if (orderSide == FIXConstants.SIDE_Buy || orderSide == FIXConstants.SIDE_Buy_Open || orderSide == FIXConstants.SIDE_Buy_Closed
                            || orderSide == FIXConstants.SIDE_Buy_Cover)
                            sideMultipier = 1;

                        closingCashActivity.SideMultiplier = sideMultipier;
                        if (position.ClosingMode != ClosingMode.Offset)
                        {
                            if (position.LeadCurrencyID.Equals(position.CurrencyID))
                                closingCashActivity.Amount = Convert.ToDecimal(position.OpenAveragePrice * position.ClosedQty);
                            else if (position.OpenAveragePrice != 0)  //Opposite deal in currency
                                closingCashActivity.Amount = Convert.ToDecimal(position.ClosedQty / position.OpenAveragePrice);

                            closingCashActivity.PnL = Convert.ToDecimal(position.CostBasisGrossPNL);
                            if (closingCashActivity.SideMultiplier < 0 && position.FxRate != 0 && position.ClosedAveragePrice != 0)
                            {
                                if (position.LeadCurrencyID != position.CurrencyID)
                                {
                                    closingCashActivity.FXPnL = (position.CurrencyID == 1) ? Convert.ToDecimal(((position.FxRate - position.ClosedAveragePrice) * (double)closingCashActivity.Amount) / position.FxRate) : Convert.ToDecimal((1 / position.ClosedAveragePrice - 1 / position.FxRate) * position.ClosedQty);
                                }
                                else
                                {
                                    closingCashActivity.FXPnL = (position.CurrencyID != 1) ? Convert.ToDecimal((position.ClosedAveragePrice - position.FxRate) * position.ClosedQty) : Convert.ToDecimal((1 / position.FxRate - 1 / position.ClosedAveragePrice) * (double)closingCashActivity.Amount * position.FxRate);
                                }
                            }
                            else if (position.FxRate != 0 && position.ClosedAveragePrice != 0)
                            {
                                if (position.LeadCurrencyID != position.CurrencyID)
                                {
                                    closingCashActivity.FXPnL = (position.CurrencyID == 1) ? Convert.ToDecimal(((position.ClosedAveragePrice - position.FxRate) * (double)closingCashActivity.Amount) / position.FxRate) : Convert.ToDecimal((1 / position.FxRate - 1 / position.ClosedAveragePrice) * position.ClosedQty);
                                }
                                else
                                {
                                    closingCashActivity.FXPnL = (position.CurrencyID != 1) ? Convert.ToDecimal((position.FxRate - position.ClosedAveragePrice) * position.ClosedQty) : Convert.ToDecimal((1 / position.ClosedAveragePrice - 1 / position.FxRate) * (double)closingCashActivity.Amount * position.FxRate);
                                }
                            }

                            closingCashActivity.UniqueKey = closingCashActivity.GetKey();
                            if (cashPreferences[closingCashActivity.AccountID].IsBreakRealizedPnlSubaccount)
                            {
                                if (position.PositionSide.Equals(PositionTag.Long.ToString()) && (position.ClosingTradeDate - position.TradeDate).TotalDays > 365)
                                    closingCashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FxForwardLongLT_Settled.ToString());
                                else if (position.PositionSide.Equals(PositionTag.Long.ToString()) && (position.ClosingTradeDate - position.TradeDate).TotalDays <= 365)
                                    closingCashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FxForwardLongST_Settled.ToString());
                                else
                                    closingCashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FxForwardShortST_Settled.ToString());
                            }
                            else
                                closingCashActivity.ActivityType = CachedDataManager.GetActivityTypeWithAcronym(Activities.FxForward_Settled.ToString());
                            closingCashActivity.ActivityTypeId = CachedDataManager.GetActivityTypeID(closingCashActivity.ActivityType);
                            closingCashActivity.FXConversionMethodOperator = Operator.M.ToString();
                            closingCashActivity.avgPrice = position.FxRate;
                            ActivityGeneratorHelper.SetActivityFxRate(closingCashActivity, position.FxRate);
                            if (closingCashActivity.ActivityTypeId != int.MinValue)
                            {
                                closingCashActivity.ActivitySource = CachedDataManager.GetActivitySource(closingCashActivity.ActivityTypeId);
                                lsCashActivity.Add(closingCashActivity);
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
