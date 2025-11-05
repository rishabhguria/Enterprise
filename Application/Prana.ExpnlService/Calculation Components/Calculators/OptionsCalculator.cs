using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prana.ExpnlService
{
    class OptionsCalculator : Calculator
    {
        public OptionsCalculator(Dictionary<int, DateTime> auecWiseAdjustedCurrentDates, Dictionary<int, DateTime> clearanceTimes)
        {
            base.AUECWiseAdjustedDateTime = auecWiseAdjustedCurrentDates;
            base.ClearanceDateTime = clearanceTimes;
        }

        public override void CalculateUnderlyingValueforOptions(EPnlOrder order)
        {
            order.UnderlyingValueForOptions = BusinessLogic.Calculations.GetUnderlyingValueForOptions(order.Quantity, order.Multiplier, order.UnderlyingStockPrice);
        }

        internal override double FillPriceInfo(EPnlOrder order)
        {
            double yesterdayMarkPrice = 0.0;
            //Reset global values before start calculating new order            
            try
            {
                OptionSymbolData fullOptionData = (LiveFeedManager.Instance.GetDynamicSymbolData(order.Symbol)) as OptionSymbolData;
                if (fullOptionData != null)
                {
                    order.YesterdayMarkPrice = fullOptionData.MarkPrice;
                    order.YesterdayMarkPriceStr = fullOptionData.MarkPriceStr;
                    yesterdayMarkPrice = order.YesterdayMarkPrice;
                    order.PricingSource = fullOptionData.PricingSource;
                    order.DeltaSource = fullOptionData.DeltaSource;
                    order.PricingStatus = fullOptionData.PricingStatus;
                    FillLiveFeedData(order, fullOptionData);
                }
                if (base.AUECWiseAdjustedDateTime.ContainsKey(order.AUECID))
                {
                    if (order.TransactionDate.Date >= base.AUECWiseAdjustedDateTime[order.AUECID].Date)
                    {
                        yesterdayMarkPrice = order.AvgPrice;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return yesterdayMarkPrice;
        }

        /// <summary>
        /// Fill LiveFeed Data to Option Orders
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private void FillLiveFeedData(EPnlOrder order, OptionSymbolData fullOptionData)
        {
            if (fullOptionData != null)
            {
                //If options are expired, then no calculation on those options
                if (String.IsNullOrEmpty(order.UnderlyingSymbol))
                {
                    // Write into the text box that the symbol is null or empty
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Underlying Symbol for option symbol " + order.Symbol + " is empty ", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }
                SymbolData l1UnderlyingData = LiveFeedManager.Instance.GetDynamicSymbolData(order.UnderlyingSymbol);
                double dividendYield = 0;
                if (l1UnderlyingData != null)
                {
                    order.UnderlyingStockPrice = l1UnderlyingData.SelectedFeedPrice;
                    order.YesterdayUnderlyingMarkPrice = l1UnderlyingData.MarkPrice;
                    order.YesterdayUnderlyingMarkPriceStr = l1UnderlyingData.MarkPriceStr;
                    // Multiplied div yield by 100 as it is supplied in decimal terms everywhere
                    dividendYield = l1UnderlyingData.FinalDividendYield * 100;
                    order.AverageVolume20DayUnderlyingSymbol = l1UnderlyingData.AverageVolume20Day;
                }

                //Fill the Ask, Bid & Last price of order on the basis of which calculation is done
                order.AskPrice = fullOptionData.Ask;
                order.BidPrice = fullOptionData.Bid;
                order.LastPrice = fullOptionData.LastPrice;
                order.HighPrice = fullOptionData.High;
                order.LowPrice = fullOptionData.Low;
                order.MidPrice = fullOptionData.Mid;
                order.iMidPrice = fullOptionData.iMid;
                order.ClosingPrice = fullOptionData.Previous;
                order.DividendYield = dividendYield;
                ((EPnLOrderOption)order).Delta = fullOptionData.Delta;
                ((EPnLOrderOption)order).Volatility = fullOptionData.FinalImpliedVol;
                order.AverageVolume20Day = fullOptionData.AverageVolume20Day;
                order.SharesOutstanding = fullOptionData.SharesOutstanding;
                order.SelectedFeedPrice = fullOptionData.SelectedFeedPrice;
                order.SelectedFeedPriceInBaseCurrency = fullOptionData.SelectedFeedPrice * order.FxRate;
                order.LastUpdatedUTC = fullOptionData.UpdateTime.ToLocalTime();
                order.ForwardPoints = fullOptionData.ForwardPoints;
                order.TradeVolume = fullOptionData.TotalVolume;
                ((EPnLOrderOption)order).DaysToExpiry = fullOptionData.DaysToExpiration;
            }
        }

        /// <summary>
        /// This method is responsible for Exposure Calculation
        /// </summary>
        internal override void CalculateExposureAndMarketValue(EPnlOrder order)
        {
            try
            {
                if (order.Asset == AssetCategory.FutureOption)
                {
                    order.DeltaAdjPosition = BusinessLogic.Calculations.GetDeltaAdjustedPosition(order.Quantity, 1, order.SideMultiplier, ((EPnLOrderOption)order).Delta);
                }
                else
                {
                    order.DeltaAdjPosition = BusinessLogic.Calculations.GetDeltaAdjustedPosition(order.Quantity, order.Multiplier, order.SideMultiplier, ((EPnLOrderOption)order).Delta);
                }
                if (order.UnderlyingStockPrice == 0)
                {
                    order.NetExposure = 0;
                    order.SetNetExposureInBaseCurrency();

                    order.Exposure = order.NetExposure;
                    order.ExposureInBaseCurrency = order.NetExposureInBaseCurrency;
                }

                if (order.SelectedFeedPrice == 0)
                {
                    order.MarketValue = 0;
                    order.SetMarketValueInBaseCurrency();
                }
                else
                {
                    order.MarketValue = BusinessLogic.Calculations.GetMarketValue(order.Quantity, order.SelectedFeedPrice, order.Multiplier, order.SideMultiplier);
                    order.SetMarketValueInBaseCurrency();
                }

                order.NetExposure = BusinessLogic.Calculations.GetNetExposure(order.Quantity, order.UnderlyingStockPrice, order.Multiplier, order.SideMultiplier, ((EPnLOrderOption)order).Delta, order.LeveragedFactor);
                order.SetNetExposureInBaseCurrency();

                //Bharat Jangir (6 May 2015)
                //Handling of Currency Futures/Future Options
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-8159

                if (((EPnLOrderOption)order).IsCurrencyFuture)
                {
                    order.Exposure = BusinessLogic.Calculations.GetLocalExposureForFutureAndFutureOptions(order.Quantity, order.Multiplier, order.SideMultiplier, order.FxRate);
                    order.ExposureInBaseCurrency = BusinessLogic.Calculations.GetExposureInBaseCurrencyForFutureAndFutureOptions(order.Quantity, order.UnderlyingStockPrice, order.Multiplier, order.SideMultiplier, order.FxRate);
                }
                else
                {
                    order.Exposure = order.NetExposure;
                    order.ExposureInBaseCurrency = order.NetExposureInBaseCurrency;
                }

                order.BetaAdjExposure = BusinessLogic.Calculations.GetBetaAdjExposure(order.NetExposure, order.Beta, order.LeveragedFactor);
                order.BetaAdjExposureInBaseCurrency = BusinessLogic.Calculations.GetBetaAdjExposure(order.NetExposureInBaseCurrency, order.Beta, order.LeveragedFactor);

                if (order.MarketValueInBaseCurrency != 0)
                {
                    order.PercentDayPnLGrossMV = order.DayPnLInBaseCurrency / Math.Abs(order.MarketValueInBaseCurrency) * 100;
                    order.PercentDayPnLNetMV = order.DayPnLInBaseCurrency / order.MarketValueInBaseCurrency * 100;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public override void CalculateDeltaAdjustedLME(EPnlOrder order)
        {
            if (order.ExchangeName.ToUpper().Equals("COMX") && order.Symbol.ToUpper().StartsWith("HG") && (order.Asset == AssetCategory.Future || order.Asset == AssetCategory.FutureOption))
            {
                order.DeltaAdjPositionLME = order.DeltaAdjPosition * ApplicationConstants.DELTAADJPOSITION_LME_MULTIPLIER;
            }
            else
            {
                order.DeltaAdjPositionLME = order.DeltaAdjPosition;
            }
        }

        /// <summary>
        /// This method is responsible for PNL Calculation
        /// </summary>
        internal override void CalculatePNL(EPnlOrder order, double yesterdayMarkPrice)
        {
            try
            {
                order.CostBasisUnrealizedPnL = BusinessLogic.Calculations.GetPnL(order.Quantity, order.SelectedFeedPrice, order.AvgPrice, order.Multiplier, order.SideMultiplier);
                order.CostBasisUnrealizedPnLInBaseCurrency = BusinessLogic.Calculations.GetPnLInBaseCurrency(order.Quantity, order.SelectedFeedPrice, order.AvgPrice, order.Multiplier, order.SideMultiplier, order.FxRate, order.FXRateOnTradeDate);
                order.DayPnL = BusinessLogic.Calculations.GetPnL(order.Quantity, order.SelectedFeedPrice, yesterdayMarkPrice, order.Multiplier, order.SideMultiplier);

                int accountBaseCurrency;
                if (CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID().ContainsKey(order.Level1ID))
                {
                    accountBaseCurrency = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID()[order.Level1ID];
                }
                else
                {
                    accountBaseCurrency = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                }
                if (order.CurrencyID != accountBaseCurrency)
                {
                    order.DayPnLInBaseCurrency = BusinessLogic.Calculations.GetPnLInBaseCurrency(order.Quantity, order.SelectedFeedPrice, yesterdayMarkPrice, order.Multiplier, order.SideMultiplier, order.FxRate, order.YesterdayFXRate);
                }
                else
                {
                    order.SetDayPnLInBaseCurrency();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        // Bharat Kumar Jangir (04 October 2013)
        // http://jira.nirvanasolutions.com:8080/browse/PRANA-2425
        // http://jira.nirvanasolutions.com:8080/browse/PRANA-2426
        public override void CalculatePremiums(EPnlOrder order)
        {
            if (((EPnLOrderOption)order).ContractType.ToUpper() == OptionType.CALL.ToString().ToUpper())
            {
                if (((EPnLOrderOption)order).StrikePrice < order.UnderlyingStockPrice)
                {
                    order.Premium = ((EPnLOrderOption)order).SelectedFeedPrice - (order.UnderlyingStockPrice - ((EPnLOrderOption)order).StrikePrice);
                }
                else
                {
                    order.Premium = ((EPnLOrderOption)order).SelectedFeedPrice;
                }
            }
            else if (((EPnLOrderOption)order).ContractType.ToUpper() == OptionType.PUT.ToString().ToUpper())
            {
                if (((EPnLOrderOption)order).StrikePrice > order.UnderlyingStockPrice)
                {
                    order.Premium = ((EPnLOrderOption)order).SelectedFeedPrice - (((EPnLOrderOption)order).StrikePrice - order.UnderlyingStockPrice);
                }
                else
                {
                    order.Premium = ((EPnLOrderOption)order).SelectedFeedPrice;
                }
            }

            order.PremiumDollar = order.Premium * order.Quantity * order.Multiplier * order.FxRate * order.SideMultiplier;
        }

        internal override void CalculateCashImpact(EPnlOrder order)
        {
            try
            {
                //Cash mgmt cash impact is not subscribed
                //Only calculated for unallocated as for allocated orders, cash management is supplying the cash impact

                //if current date order, cash impact = negative of notional
                if (order.TransactionDate.Date >= base.AUECWiseAdjustedDateTime[order.AUECID].Date)
                {
                    order.CashImpact = -1 * order.NetNotionalValue;
                    order.SetCashImpactInBaseCurrency();
                }
                else // back date order, cash file adjusted for this transaction
                {
                    order.CashImpact = 0.0;
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
        }

        internal override void CalculateMarketCap(EPnlOrder order)
        {
            try
            {
                order.MarketCapitalization = 0.0;
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
        /// Calculates the option moneyness.
        /// </summary>
        /// <param name="order">The order.</param>
        public override void CalculateOptionMoneyness(EPnlOrder order)
        {
            try
            {
                EPnLOrderOption optOrder = (EPnLOrderOption)order;
                if (optOrder.StrikePrice > 0.0)
                {
                    if (optOrder.StrikePrice == optOrder.UnderlyingStockPrice)
                    {
                        optOrder.ItmOtm = OptionMoneyness.ATM;
                        optOrder.PercentOfITMOTM = 0.0;
                        optOrder.IntrinsicValue = 0.0;
                        optOrder.GainLossIfExerciseAssign = -optOrder.AvgPrice * order.Quantity * order.Multiplier * order.FxRate * order.SideMultiplier;
                    }
                    else if (optOrder.ContractType.ToUpper() == OptionType.CALL.ToString().ToUpper())
                    {
                        if (optOrder.StrikePrice < optOrder.UnderlyingStockPrice)
                        {
                            optOrder.ItmOtm = OptionMoneyness.ITM;
                            optOrder.IntrinsicValue = optOrder.UnderlyingStockPrice - optOrder.StrikePrice;
                        }
                        else
                        {
                            optOrder.ItmOtm = OptionMoneyness.OTM;
                            optOrder.IntrinsicValue = 0.0;
                        }
                        optOrder.PercentOfITMOTM = (optOrder.UnderlyingStockPrice - optOrder.StrikePrice) * 100 / optOrder.StrikePrice;
                        optOrder.GainLossIfExerciseAssign = (optOrder.UnderlyingStockPrice - optOrder.StrikePrice - optOrder.AvgPrice)
                            * order.Quantity * order.Multiplier * order.FxRate * order.SideMultiplier;
                    }
                    else if (optOrder.ContractType.ToUpper() == OptionType.PUT.ToString().ToUpper())
                    {
                        if (optOrder.StrikePrice > optOrder.UnderlyingStockPrice)
                        {
                            optOrder.ItmOtm = OptionMoneyness.ITM;
                            optOrder.IntrinsicValue = optOrder.StrikePrice - optOrder.UnderlyingStockPrice;
                        }
                        else
                        {
                            optOrder.ItmOtm = OptionMoneyness.OTM;
                            optOrder.IntrinsicValue = 0.0;
                        }
                        optOrder.PercentOfITMOTM = (optOrder.StrikePrice - optOrder.UnderlyingStockPrice) * 100 / optOrder.StrikePrice;
                        optOrder.GainLossIfExerciseAssign = (optOrder.StrikePrice - optOrder.UnderlyingStockPrice - optOrder.AvgPrice)
                            * order.Quantity * order.Multiplier * order.FxRate * order.SideMultiplier;
                    }
                    if (optOrder.ItmOtm == OptionMoneyness.OTM)
                    {
                        double maxGainLoss = optOrder.AvgPrice * optOrder.Quantity * optOrder.Multiplier * optOrder.FxRate;

                        if (optOrder.GainLossIfExerciseAssign < -maxGainLoss)
                            optOrder.GainLossIfExerciseAssign = -maxGainLoss;
                        else if (optOrder.GainLossIfExerciseAssign > maxGainLoss)
                            optOrder.GainLossIfExerciseAssign = maxGainLoss;
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
        }

        internal override void CalculateNotional(EPnlOrder order)
        {
            try
            {
                order.NetNotionalValue = BusinessLogic.Calculations.GetNotional(order.Quantity, order.AvgPrice, order.Multiplier, order.SideMultiplier);
                order.SetNetNotionalInCompanyBaseCurrency();
                order.NetNotionalForCostBasisBreakEven = order.NetNotionalValue;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}