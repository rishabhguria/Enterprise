using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prana.ExpnlService
{
    class FXCalculator : Calculator
    {
        bool _calculateFxGainLossOnForexForwards = true;

        public FXCalculator(Dictionary<int, DateTime> auecWiseAdjustedCurrentDates, Dictionary<int, DateTime> clearanceTimes)
        {
            base.AUECWiseAdjustedDateTime = auecWiseAdjustedCurrentDates;
            base.ClearanceDateTime = clearanceTimes;

            _calculateFxGainLossOnForexForwards = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("CalculateFxGainLossOnForexForwards"));
        }

        private void FillUpdatedLiveFeedData(EPnlOrder order, SymbolData symbolLevel1Data)
        {
            ConcurrentDictionary<int, int> accountWiseBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID();
            int baseCurrency = accountWiseBaseCurrencyID.ContainsKey(order.Level1ID) ? accountWiseBaseCurrencyID[order.Level1ID] : CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();

            if (symbolLevel1Data.AverageVolume20Day != double.MinValue)
            {
                order.AverageVolume20Day = symbolLevel1Data.AverageVolume20Day;
                order.AverageVolume20DayUnderlyingSymbol = symbolLevel1Data.AverageVolume20Day;
            }

            if (symbolLevel1Data.SelectedFeedPrice != double.MinValue)
            {
                order.SelectedFeedPrice = symbolLevel1Data.SelectedFeedPrice;
                order.UnderlyingStockPrice = symbolLevel1Data.SelectedFeedPrice;
                if ((order is EPnLOrderFX && ((EPnLOrderFX)order).LeadCurrencyID != baseCurrency) || (order is EPnLOrderFXForward && ((EPnLOrderFXForward)order).LeadCurrencyID != baseCurrency))
                    order.SelectedFeedPriceInBaseCurrency = symbolLevel1Data.SelectedFeedPrice;
                else
                    order.SelectedFeedPriceInBaseCurrency = symbolLevel1Data.SelectedFeedPrice != 0 ? 1 / symbolLevel1Data.SelectedFeedPrice : 0;
            }

            order.FxRateDisplay = order.SelectedFeedPrice;

            if (symbolLevel1Data.Ask != double.MinValue)
            {
                order.AskPrice = symbolLevel1Data.Ask;
            }
            if (symbolLevel1Data.Bid != double.MinValue)
            {
                order.BidPrice = symbolLevel1Data.Bid;
            }
            if (symbolLevel1Data.LastPrice != double.MinValue)
            {
                order.LastPrice = symbolLevel1Data.LastPrice;
            }
            if (symbolLevel1Data.High != double.MinValue)
            {
                order.HighPrice = symbolLevel1Data.High;
            }
            if (symbolLevel1Data.Low != double.MinValue)
            {
                order.LowPrice = symbolLevel1Data.Low;
            }
            if (symbolLevel1Data.Mid != double.MinValue)
            {
                order.MidPrice = symbolLevel1Data.Mid;
            }
            if (symbolLevel1Data.iMid != double.MinValue)
            {
                order.iMidPrice = symbolLevel1Data.iMid;
            }
            if (symbolLevel1Data.Previous != double.MinValue)
            {
                order.ClosingPrice = symbolLevel1Data.Previous;
            }
            // Multiplied div yield by 100 as it is supplied in decimal terms everywhere
            if (symbolLevel1Data.DividendYield != double.MinValue)
            {
                order.DividendYield = symbolLevel1Data.FinalDividendYield * 100;
            }

            if (symbolLevel1Data.CategoryCode == AssetCategory.Equity)
            {
                if (((EquitySymbolData)symbolLevel1Data).SharesOutstanding != double.MinValue)
                {
                    order.SharesOutstanding = ((EquitySymbolData)symbolLevel1Data).SharesOutstanding;
                }
            }
            if (symbolLevel1Data.ForwardPoints != double.MinValue)
            {
                order.ForwardPoints = symbolLevel1Data.ForwardPoints;
            }
            order.LastUpdatedUTC = symbolLevel1Data.UpdateTime.ToLocalTime();
            order.TradeVolume = symbolLevel1Data.TotalVolume;
        }

        public override void CalculateFxCostBasisPnL(EPnlOrder order)
        {
            if (_calculateFxGainLossOnForexForwards)
            {
                base.CalculateFxCostBasisPnL(order);
            }
            else
            {
                return;
            }
        }

        public override void CalculateFxDayPnL(EPnlOrder order)
        {
            if (_calculateFxGainLossOnForexForwards)
            {
                base.CalculateFxDayPnL(order);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Overrides the base PNL currency column.
        /// </summary>
        /// <param name="order">The order.</param>
        public override void OverridePnlInBaseCurrency(EPnlOrder order)
        {
            base.OverridePnlInBaseCurrency(order);
            order.DayPnLInBaseCurrency = order.TradeDayPnl + order.FxDayPnl;
            order.CostBasisUnrealizedPnLInBaseCurrency = order.TradeCostBasisPnl + order.FxCostBasisPnl;
            order.DayPnLInBaseCurrency = order.TradeDayPnl + order.FxDayPnl;

            /// Since market value is equal to CostBasisUnrealizedPnLInCompanyBaseCurrency. Reassigned CostBasisUnrealizedPnLInCompanyBaseCurrency 
            /// as it is being overridden in the function. Also calculating other dependent values.
            order.MarketValueInBaseCurrency = order.CostBasisUnrealizedPnLInBaseCurrency;
            order.NetExposureInBaseCurrency = order.CostBasisUnrealizedPnLInBaseCurrency;
            order.ExposureInBaseCurrency = order.CostBasisUnrealizedPnLInBaseCurrency;

            CalculatePercentageGainLoss(order);

            if (order.MarketValueInBaseCurrency != 0)
            {
                order.PercentDayPnLGrossMV = order.DayPnLInBaseCurrency / Math.Abs(order.MarketValueInBaseCurrency) * 100;
                order.PercentDayPnLNetMV = order.DayPnLInBaseCurrency / order.MarketValueInBaseCurrency * 100;
            }

            order.BetaAdjExposure = BusinessLogic.Calculations.GetBetaAdjExposure(order.NetExposure, order.Beta);
            order.BetaAdjExposureInBaseCurrency = BusinessLogic.Calculations.GetBetaAdjExposure(order.NetExposureInBaseCurrency, order.Beta);
        }

        /// <summary>
        /// Calculates the trade cost basis pnl.
        /// </summary>
        /// <param name="order">The order.</param>
        public override void CalculateTradeCostBasisPnL(EPnlOrder order)
        {
            try
            {
                ConcurrentDictionary<int, int> accountWiseBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID();
                int baseCurrency = accountWiseBaseCurrencyID.ContainsKey(order.Level1ID) ? accountWiseBaseCurrencyID[order.Level1ID] : CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                int otherCurrency = baseCurrency;

                if (order is EPnLOrderFX)
                {
                    otherCurrency = ((EPnLOrderFX)order).LeadCurrencyID;
                    switch (CachedDataManager.GetInstance.GetCurrencyText(otherCurrency))
                    {
                        case "EUR":
                        case "GBP":
                        case "NZD":
                        case "AUD":
                            if ((order.CurrencyID != baseCurrency))
                                order.TradeCostBasisPnl = ((EPnLOrderFX)order).CounterCurrencyCostBasisPnL;
                            else
                                order.TradeCostBasisPnl = (order.CostBasisUnrealizedPnL * order.SelectedFeedPrice);

                            break;
                        default:
                            if ((order.CurrencyID != baseCurrency))
                                order.TradeCostBasisPnl = ((EPnLOrderFX)order).CounterCurrencyCostBasisPnL;
                            else
                                order.TradeCostBasisPnl = order.SelectedFeedPrice != 0 ? (order.CostBasisUnrealizedPnL / order.SelectedFeedPrice) : 0;
                            break;
                    }
                }
                else if (order is EPnLOrderFXForward)
                {
                    otherCurrency = ((EPnLOrderFXForward)order).LeadCurrencyID;
                    switch (CachedDataManager.GetInstance.GetCurrencyText(otherCurrency))
                    {
                        case "EUR":
                        case "GBP":
                        case "NZD":
                        case "AUD":
                            if ((order.CurrencyID != baseCurrency))
                                order.TradeCostBasisPnl = ((EPnLOrderFXForward)order).CounterCurrencyCostBasisPnL;
                            else
                                order.TradeCostBasisPnl = (order.CostBasisUnrealizedPnL * order.SelectedFeedPrice);

                            break;
                        default:
                            if ((order.CurrencyID != baseCurrency))
                                order.TradeCostBasisPnl = ((EPnLOrderFXForward)order).CounterCurrencyCostBasisPnL;
                            else
                                order.TradeCostBasisPnl = order.SelectedFeedPrice != 0 ? (order.CostBasisUnrealizedPnL / order.SelectedFeedPrice) : 0;
                            break;
                    }
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

        public override void CalculateClosingMarketValue(EPnlOrder order)
        {
            try
            {
                if (order is EPnLOrderFXForward)
                {
                    double tempCounterCurrencyYesterdayCostBasisPnL = 0;

                    ConcurrentDictionary<int, int> accountWiseBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID();
                    int baseCurrency = accountWiseBaseCurrencyID.ContainsKey(order.Level1ID) ? accountWiseBaseCurrencyID[order.Level1ID] : CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();

                    EPnLOrderFXForward temp = order as EPnLOrderFXForward;
                    if (temp.CurrencyID != baseCurrency)
                    {
                        if (temp.VsCurrencyID == baseCurrency)
                        {
                            tempCounterCurrencyYesterdayCostBasisPnL = temp.SideMultiplier * temp.Quantity * (temp.YesterdayMarkPrice - temp.AvgPrice);
                        }
                        else
                        {
                            tempCounterCurrencyYesterdayCostBasisPnL = temp.YesterdayMarkPrice != 0 && temp.AvgPrice != 0 ? (temp.SideMultiplier * temp.Quantity / temp.YesterdayMarkPrice) - (temp.SideMultiplier * temp.Quantity / temp.AvgPrice) : 0;
                        }
                    }

                    int otherCurrency = ((EPnLOrderFXForward)order).LeadCurrencyID;
                    switch (CachedDataManager.GetInstance.GetCurrencyText(otherCurrency))
                    {
                        case "EUR":
                        case "GBP":
                        case "NZD":
                        case "AUD":
                            if ((order.CurrencyID != baseCurrency))
                                order.YesterdayMarketValueInBaseCurrency = tempCounterCurrencyYesterdayCostBasisPnL;
                            else
                            {
                                double tempCostBasisPNL = order.YesterdayMarkPrice != 0 && order.AvgPrice != 0 ? (((order.SideMultiplier * order.Quantity) / order.AvgPrice * order.YesterdayMarkPrice) - ((order.SideMultiplier * order.Quantity) / order.AvgPrice * order.AvgPrice)) / order.YesterdayMarkPrice * -1 : 0;
                                order.YesterdayMarketValueInBaseCurrency = (tempCostBasisPNL * order.YesterdayMarkPrice);
                            }

                            break;
                        default:
                            if ((order.CurrencyID != baseCurrency))
                                order.YesterdayMarketValueInBaseCurrency = tempCounterCurrencyYesterdayCostBasisPnL;
                            else
                            {
                                double tempCostBasisPNL = order.YesterdayMarkPrice != 0 && order.AvgPrice != 0 ? ((order.SideMultiplier * order.Quantity * order.AvgPrice / order.YesterdayMarkPrice) - (order.SideMultiplier * order.Quantity)) * order.YesterdayMarkPrice * -1 : 0;
                                order.YesterdayMarketValueInBaseCurrency = order.YesterdayMarkPrice != 0 ? (tempCostBasisPNL / order.YesterdayMarkPrice) : 0;
                            }
                            break;
                    }
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

        internal override double FillPriceInfo(EPnlOrder order)
        {
            double yesterdayMarkPrice = 0.0;
            try
            {
                if (base.ClearanceDateTime.ContainsKey(order.AUECID))
                {
                    //Reset global values before start calculating new order
                    int leadCurrency = 0;
                    int vsCurrency = 0;

                    if (order.Asset == AssetCategory.FX)
                    {
                        leadCurrency = ((EPnLOrderFX)order).LeadCurrencyID;
                        vsCurrency = ((EPnLOrderFX)order).VsCurrencyID;
                    }
                    else if (order.Asset == AssetCategory.FXForward)
                    {
                        leadCurrency = ((EPnLOrderFXForward)order).LeadCurrencyID;
                        vsCurrency = ((EPnLOrderFXForward)order).VsCurrencyID;
                    }
                    if (order.Asset == AssetCategory.FX || order.Asset == AssetCategory.FXForward)
                    {
                        SymbolData symbolLevel1Data = LiveFeedManager.Instance.GetDynamicSymbolData(order.Symbol, leadCurrency, vsCurrency, order.Asset);
                        if (symbolLevel1Data != null)
                        {
                            order.YesterdayMarkPrice = symbolLevel1Data.MarkPrice;
                            order.YesterdayMarkPriceStr = symbolLevel1Data.MarkPriceStr;
                            order.YesterdayUnderlyingMarkPrice = symbolLevel1Data.MarkPrice;
                            order.YesterdayUnderlyingMarkPriceStr = symbolLevel1Data.MarkPriceStr;
                            yesterdayMarkPrice = order.YesterdayMarkPrice;
                            order.PricingSource = symbolLevel1Data.PricingSource;
                            order.PricingStatus = symbolLevel1Data.PricingStatus;
                            FillUpdatedLiveFeedData(order, symbolLevel1Data);
                        }
                    }
                    if (base.AUECWiseAdjustedDateTime.ContainsKey(order.AUECID))
                    {
                        if (order.TransactionDate.Date >= base.AUECWiseAdjustedDateTime[order.AUECID].Date)
                        {
                            yesterdayMarkPrice = order.AvgPrice;
                        }
                    }
                }
                else
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Clearance Time not available for AUEC ID : " + order.AUECID + DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt"), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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
        /// Calculates the PNL related columns.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="yesterdayMarkPrice">The yesterday mark price.</param>
        internal override void CalculatePNL(EPnlOrder order, double yesterdayMarkPrice)
        {
            try
            {
                double costBasisPNL = 0;
                double dayPNL = 0.0;
                ConcurrentDictionary<int, int> accountWiseBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID();
                int baseCurrency = accountWiseBaseCurrencyID.ContainsKey(order.Level1ID) ? accountWiseBaseCurrencyID[order.Level1ID] : CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                int otherCurrency = baseCurrency;

                if (order is EPnLOrderFX)
                    otherCurrency = ((EPnLOrderFX)order).LeadCurrencyID;
                else if (order is EPnLOrderFXForward)
                    otherCurrency = ((EPnLOrderFXForward)order).LeadCurrencyID;

                switch (CachedDataManager.GetInstance.GetCurrencyText(otherCurrency))
                {
                    case "EUR":
                    case "GBP":
                    case "NZD":
                    case "AUD":
                        if ((order.CurrencyID == baseCurrency))
                        {
                            costBasisPNL = order.SelectedFeedPrice != 0 && order.AvgPrice != 0 ? (((order.SideMultiplier * order.Quantity) / order.AvgPrice * order.SelectedFeedPrice) - ((order.SideMultiplier * order.Quantity) / order.AvgPrice * order.AvgPrice)) / order.SelectedFeedPrice * -1 : 0;
                            dayPNL = order.SelectedFeedPrice != 0 && yesterdayMarkPrice != 0 ? ((((order.SideMultiplier * order.Quantity) / order.AvgPrice) * order.SelectedFeedPrice) - (((order.SideMultiplier * order.Quantity) / (order.AvgPrice) * yesterdayMarkPrice))) / order.SelectedFeedPrice * -1 : 0;
                        }
                        break;
                    default:
                        if ((order.CurrencyID == baseCurrency))
                        {
                            costBasisPNL = order.SelectedFeedPrice != 0 && order.AvgPrice != 0 ? ((order.SideMultiplier * order.Quantity * order.AvgPrice / order.SelectedFeedPrice) - (order.SideMultiplier * order.Quantity)) * order.SelectedFeedPrice * -1 : 0;
                            dayPNL = order.SelectedFeedPrice != 0 && yesterdayMarkPrice != 0 ? (((order.SideMultiplier * order.Quantity * order.AvgPrice) / order.SelectedFeedPrice) - ((order.SideMultiplier * order.Quantity * order.AvgPrice) / yesterdayMarkPrice)) * order.SelectedFeedPrice * -1 : 0;
                        }
                        break;
                }
                order.CostBasisUnrealizedPnL = costBasisPNL;
                order.DayPnL = dayPNL;
                order.CostBasisUnrealizedPnLInBaseCurrency = BusinessLogic.Calculations.GetPnLInBaseCurrency(order.Quantity, order.SelectedFeedPrice, order.AvgPrice, order.Multiplier, order.SideMultiplier, order.FxRate, order.FXRateOnTradeDate);

                order.DayPnLInBaseCurrency = order.TradeDayPnl + order.FxDayPnl;
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

        internal override void CalculateExposureAndMarketValue(EPnlOrder order)
        {
            try
            {
                order.MarketValue = order.CostBasisUnrealizedPnL;
                order.NetExposure = order.CostBasisUnrealizedPnL;
                order.DeltaAdjPosition = order.SideMultiplier * order.Quantity;
                order.Exposure = order.NetExposure;
                order.BetaAdjExposure = order.NetExposure;
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

        internal override void CalculateCashImpact(EPnlOrder order)
        {
            try
            {
                order.CashImpact = 0;
                order.CashImpactInBaseCurrency = 0;
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

        internal override void CalculateNotional(EPnlOrder order)
        {
            try
            {
                order.NetNotionalValue = 0;
                order.NetNotionalValueInBaseCurrency = 0;
                order.NetNotionalForCostBasisBreakEven = BusinessLogic.Calculations.GetNotional(order.Quantity, order.AvgPrice, order.Multiplier, order.SideMultiplier);
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

        /// <summary>
        /// This method is use to calculated two feilds for fxforword and fx which names are CounterCurrencyAmount and CounterCurrencyCostBasisPnL
        /// </summary>
        /// <param name="order">EPnlOrder</param>
        public override void CalculateCounterCurrencyAmountAndPnl(EPnlOrder order, double yesterdayMarkPrice)
        {
            try
            {
                ConcurrentDictionary<int, int> accountWiseBaseCurrencyID = CachedDataManager.GetInstance.GetAccountWiseBaseCurrencyID();
                int baseCurrency = accountWiseBaseCurrencyID.ContainsKey(order.Level1ID) ? accountWiseBaseCurrencyID[order.Level1ID] : CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                int otherCurrency = baseCurrency;

                if (order is EPnLOrderFX)
                    otherCurrency = ((EPnLOrderFX)order).LeadCurrencyID;
                else if (order is EPnLOrderFXForward)
                    otherCurrency = ((EPnLOrderFXForward)order).LeadCurrencyID;

                if (order is EPnLOrderFX)
                {
                    EPnLOrderFX temp = order as EPnLOrderFX;
                    if (temp.Quantity != 0 && temp.AvgPrice != 0)
                    {
                        if (temp.LeadCurrencyID == temp.CurrencyID)
                            temp.CounterCurrencyAmount = Math.Round(-temp.SideMultiplier * temp.Quantity * temp.AvgPrice, 4);
                        else
                            temp.CounterCurrencyAmount = Math.Round((-temp.SideMultiplier * temp.Quantity) / temp.AvgPrice, 4);
                    }
                    switch (CachedDataManager.GetInstance.GetCurrencyText(otherCurrency))
                    {
                        case "EUR":
                        case "GBP":
                        case "NZD":
                        case "AUD":
                            if ((order.CurrencyID != baseCurrency))
                            {
                                temp.CounterCurrencyCostBasisPnL = order.SelectedFeedPrice != 0 && order.AvgPrice != 0 ? ((order.SideMultiplier * order.Quantity * order.SelectedFeedPrice) - (order.SideMultiplier * order.Quantity * order.AvgPrice)) : 0;
                                temp.CounterCurrencyDayPnL = (temp.SideMultiplier * temp.Quantity * temp.SelectedFeedPrice) - (temp.SideMultiplier * temp.Quantity * yesterdayMarkPrice);
                            }
                            break;
                        default:
                            if ((order.CurrencyID != baseCurrency))
                            {
                                temp.CounterCurrencyCostBasisPnL = order.SelectedFeedPrice != 0 && order.AvgPrice != 0 ? (((order.SideMultiplier * order.Quantity) / order.SelectedFeedPrice) - ((order.SideMultiplier * order.Quantity) / order.AvgPrice)) : 0;
                                temp.CounterCurrencyDayPnL = temp.SelectedFeedPrice != 0 && yesterdayMarkPrice != 0 ? ((temp.SideMultiplier * temp.Quantity) / temp.SelectedFeedPrice) - ((temp.SideMultiplier * temp.Quantity) / yesterdayMarkPrice) : 0;
                            }
                            break;
                    }
                }

                if (order is EPnLOrderFXForward)
                {
                    EPnLOrderFXForward temp = order as EPnLOrderFXForward;
                    if (temp.Quantity != 0 && temp.AvgPrice != 0)
                    {
                        if (temp.LeadCurrencyID == temp.CurrencyID)
                            temp.CounterCurrencyAmount = Math.Round(-temp.SideMultiplier * temp.Quantity * temp.AvgPrice, 4);
                        else
                            temp.CounterCurrencyAmount = Math.Round((-temp.SideMultiplier * temp.Quantity) / temp.AvgPrice, 4);
                    }
                    if (temp.CurrencyID != baseCurrency)
                    {
                        if (temp.VsCurrencyID == baseCurrency)
                        {
                            temp.CounterCurrencyCostBasisPnL = (temp.SideMultiplier * temp.Quantity * temp.SelectedFeedPrice) - (temp.SideMultiplier * temp.Quantity * temp.AvgPrice);
                            temp.CounterCurrencyDayPnL = (temp.SideMultiplier * temp.Quantity * temp.SelectedFeedPrice) - (temp.SideMultiplier * temp.Quantity * yesterdayMarkPrice);
                        }
                        else
                        {
                            temp.CounterCurrencyCostBasisPnL = temp.SelectedFeedPrice != 0 && temp.AvgPrice != 0 ? (temp.SideMultiplier * temp.Quantity / temp.SelectedFeedPrice) - (temp.SideMultiplier * temp.Quantity / temp.AvgPrice) : 0;
                            temp.CounterCurrencyDayPnL = temp.SelectedFeedPrice != 0 && yesterdayMarkPrice != 0 ? (temp.SideMultiplier * temp.Quantity / temp.SelectedFeedPrice) - (temp.SideMultiplier * temp.Quantity / yesterdayMarkPrice) : 0;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}