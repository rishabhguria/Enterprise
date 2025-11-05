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
    class EquityCalculator : Calculator
    {
        bool _calculateFxGainLossOnSwaps = false;
        public EquityCalculator(Dictionary<int, DateTime> _auecWiseAdjustedCurrentDates, Dictionary<int, DateTime> clearanceTimes)
        {
            base.AUECWiseAdjustedDateTime = _auecWiseAdjustedCurrentDates;
            base.ClearanceDateTime = clearanceTimes;
            _calculateFxGainLossOnSwaps = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("CalculateFxGainLossOnSwaps"));
        }

        internal override double FillPriceInfo(EPnlOrder order)
        {
            double yesterdayMarkPrice = 0.0;
            try
            {
                if (base.ClearanceDateTime.ContainsKey(order.AUECID))
                {
                    //Reset global values before start calculating new order
                    SymbolData symbolLevel1Data = LiveFeedManager.Instance.GetDynamicSymbolData(order.Symbol);
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
                    if (order.TransactionDate.Date >= base.AUECWiseAdjustedDateTime[order.AUECID].Date)
                    {
                        // => current Date Trade
                        yesterdayMarkPrice = order.AvgPrice;
                    }

                    if (order.ClassID == EPnLClassID.EPnLOrderEquitySwap)
                    {
                        CalculateInterestComponents(order);
                    }

                    ///Specially done for ETF
                    if (order.LeveragedFactor != 1)
                    {
                        order.TransactionSide = order.SideMultiplier * (order.LeveragedFactor) > 0 ? PositionType.Long : PositionType.Short;
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

        private void FillUpdatedLiveFeedData(EPnlOrder order, SymbolData symbolLevel1Data)
        {
            if (symbolLevel1Data.AverageVolume20Day != double.MinValue)
            {
                order.AverageVolume20Day = symbolLevel1Data.AverageVolume20Day;
                order.AverageVolume20DayUnderlyingSymbol = symbolLevel1Data.AverageVolume20Day;
            }
            if (symbolLevel1Data.SelectedFeedPrice != double.MinValue)
            {
                order.SelectedFeedPrice = symbolLevel1Data.SelectedFeedPrice;
                order.UnderlyingStockPrice = symbolLevel1Data.SelectedFeedPrice;
                order.SelectedFeedPriceInBaseCurrency = symbolLevel1Data.SelectedFeedPrice * order.FxRate;
            }

            ///Fill the Ask, Bid & Last price of order on the basis of which calculation is done
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
                if (((EquitySymbolData)symbolLevel1Data).SharesOutstanding > 0)
                {
                    order.SharesOutstanding = ((EquitySymbolData)symbolLevel1Data).SharesOutstanding;
                    order.IsLiveFeedSharesOutstanding = true;
                }
            }
            if (symbolLevel1Data.ForwardPoints != double.MinValue)
            {
                order.ForwardPoints = symbolLevel1Data.ForwardPoints;
            }
            order.LastUpdatedUTC = symbolLevel1Data.UpdateTime.ToLocalTime();

            order.TradeVolume = symbolLevel1Data.TotalVolume;
        }

        internal override void CalculatePNL(EPnlOrder order, double yesterdayMarkPrice)
        {
            try
            {
                double swapDayInterest = 0;
                double costBasisPNL = 0;
                costBasisPNL = BusinessLogic.Calculations.GetPnL(order.Quantity, order.SelectedFeedPrice, order.AvgPrice, order.Multiplier, order.SideMultiplier);
                if (order.ClassID == EPnLClassID.EPnLOrderEquity)
                {
                    order.CostBasisUnrealizedPnL = costBasisPNL;
                }

                order.CostBasisUnrealizedPnLInBaseCurrency = BusinessLogic.Calculations.GetPnLInBaseCurrency(order.Quantity, order.SelectedFeedPrice, order.AvgPrice, order.Multiplier, order.SideMultiplier, order.FxRate, order.FXRateOnTradeDate);

                if (order.ClassID == EPnLClassID.EPnLOrderEquitySwap)
                {
                    double swapTotalInterest = ((EPnLOrderEquitySwap)order).TotalInterest;
                    switch (order.OrderSideTagValue)
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Buy_Closed:
                        case FIXConstants.SIDE_Buy_Cover:
                        case FIXConstants.SIDE_Buy_Open:
                        case FIXConstants.SIDE_BuyMinus:
                            order.CostBasisUnrealizedPnL = costBasisPNL - swapTotalInterest;
                            order.CostBasisUnrealizedPnLInBaseCurrency -= (swapTotalInterest * order.FxRate);
                            break;

                        case FIXConstants.SIDE_Sell:
                        case FIXConstants.SIDE_Sell_Closed:
                        case FIXConstants.SIDE_Sell_Open:
                        case FIXConstants.SIDE_SellPlus:
                        case FIXConstants.SIDE_SellShort:
                            order.CostBasisUnrealizedPnL = costBasisPNL + swapTotalInterest;
                            order.CostBasisUnrealizedPnLInBaseCurrency += (swapTotalInterest * order.FxRate);
                            break;

                        default:
                            order.CostBasisUnrealizedPnL = costBasisPNL;
                            break;
                    }
                }

                double dayPNL = BusinessLogic.Calculations.GetPnL(order.Quantity, order.SelectedFeedPrice, yesterdayMarkPrice, order.Multiplier, order.SideMultiplier) + order.EarnedDividendLocal;

                if (order.ClassID == EPnLClassID.EPnLOrderEquity)
                {
                    order.DayPnL = dayPNL;
                }
                else if (order.ClassID == EPnLClassID.EPnLOrderEquitySwap)
                {
                    swapDayInterest = ((EPnLOrderEquitySwap)order).DayInterest;
                    switch (order.OrderSideTagValue)
                    {
                        case FIXConstants.SIDE_Buy:
                        case FIXConstants.SIDE_Buy_Closed:
                        case FIXConstants.SIDE_Buy_Cover:
                        case FIXConstants.SIDE_Buy_Open:
                        case FIXConstants.SIDE_BuyMinus:
                            order.DayPnL = dayPNL - swapDayInterest;

                            break;

                        case FIXConstants.SIDE_Sell:
                        case FIXConstants.SIDE_Sell_Closed:
                        case FIXConstants.SIDE_Sell_Open:
                        case FIXConstants.SIDE_SellPlus:
                        case FIXConstants.SIDE_SellShort:
                            order.DayPnL = dayPNL + swapDayInterest;
                            break;

                        default:
                            order.DayPnL = dayPNL;
                            break;
                    }
                }

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
                    order.EarnedDividendBase = order.EarnedDividendLocal * order.FxRate;
                    double dayPNLInBaseCurrency = BusinessLogic.Calculations.GetPnLInBaseCurrency(order.Quantity, order.SelectedFeedPrice, yesterdayMarkPrice, order.Multiplier, order.SideMultiplier, order.FxRate, order.YesterdayFXRate);

                    if (order.ClassID == EPnLClassID.EPnLOrderEquitySwap)
                    {
                        switch (order.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Closed:
                            case FIXConstants.SIDE_Buy_Cover:
                            case FIXConstants.SIDE_Buy_Open:
                            case FIXConstants.SIDE_BuyMinus:
                                dayPNLInBaseCurrency -= (swapDayInterest * order.FxRate);
                                break;

                            case FIXConstants.SIDE_Sell:
                            case FIXConstants.SIDE_Sell_Closed:
                            case FIXConstants.SIDE_Sell_Open:
                            case FIXConstants.SIDE_SellPlus:
                            case FIXConstants.SIDE_SellShort:
                                dayPNLInBaseCurrency += (swapDayInterest * order.FxRate);
                                break;
                        }
                    }
                    order.DayPnLInBaseCurrency = dayPNLInBaseCurrency + order.EarnedDividendBase;
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

        internal override void CalculateExposureAndMarketValue(EPnlOrder order)
        {
            try
            {
                order.DeltaAdjPosition = BusinessLogic.Calculations.GetDeltaAdjustedPosition(order.Quantity, order.Multiplier, order.SideMultiplier, 1);
                order.NetExposure = BusinessLogic.Calculations.GetNetExposure(order.Quantity, order.SelectedFeedPrice, order.Multiplier, order.SideMultiplier, 1, order.LeveragedFactor);
                order.SetNetExposureInBaseCurrency();

                order.Exposure = order.NetExposure;
                order.ExposureInBaseCurrency = order.NetExposureInBaseCurrency;

                order.BetaAdjExposure = BusinessLogic.Calculations.GetBetaAdjExposure(order.NetExposure, order.Beta, order.LeveragedFactor);
                order.BetaAdjExposureInBaseCurrency = BusinessLogic.Calculations.GetBetaAdjExposure(order.NetExposureInBaseCurrency, order.Beta, order.LeveragedFactor);

                if (order.ClassID == EPnLClassID.EPnLOrderEquitySwap && !Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EquitySwapsMarketValueAsEquity")))
                {
                    order.MarketValue = order.CostBasisUnrealizedPnL;
                    order.MarketValueInBaseCurrency = order.CostBasisUnrealizedPnLInBaseCurrency;
                }
                else
                {
                    order.MarketValue = BusinessLogic.Calculations.GetMarketValue(order.Quantity, order.SelectedFeedPrice, order.Multiplier, order.SideMultiplier);
                    order.SetMarketValueInBaseCurrency();
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

        private void CalculateInterestComponents(EPnlOrder order)
        {
            double interestRate = ((EPnLOrderEquitySwap)order).SwapParameters.BenchMarkRate + ((EPnLOrderEquitySwap)order).SwapParameters.Differential;
            TimeSpan tDiff = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID)).Date - order.TransactionDate.Date;

            int dayDiff = (int)Math.Ceiling(tDiff.TotalDays);
            int daycount = ((EPnLOrderEquitySwap)order).SwapParameters.DayCount;

            if (dayDiff > 0 && daycount > 0)
            {
                ((EPnLOrderEquitySwap)order).DayInterest = BusinessLogic.Calculations.GetInterestValue(((EPnLOrderEquitySwap)order).SwapParameters.NotionalValue, interestRate, (double)((EPnLOrderEquitySwap)order).SwapParameters.DayCount, 1.0); ;
            }
            else
            {
                ((EPnLOrderEquitySwap)order).DayInterest = 0.0;
            }

            ((EPnLOrderEquitySwap)order).TotalInterest = ((EPnLOrderEquitySwap)order).DayInterest * dayDiff;
        }

        public override void CalculateFxCostBasisPnL(EPnlOrder order)
        {
            if (order.IsSwapped)
            {
                if (_calculateFxGainLossOnSwaps)
                {
                    base.CalculateFxCostBasisPnL(order);
                }
            }
            else
            {
                base.CalculateFxCostBasisPnL(order);
            }
        }

        public override void CalculateFxDayPnL(EPnlOrder order)
        {
            if (order.IsSwapped)
            {
                if (_calculateFxGainLossOnSwaps)
                {
                    base.CalculateFxDayPnL(order);
                }
            }
            else
            {
                base.CalculateFxDayPnL(order);
            }
        }

        public override void OverridePnlInBaseCurrency(EPnlOrder order)
        {
            order.DayPnLInBaseCurrency = order.TradeDayPnl + order.FxDayPnl;
            order.CostBasisUnrealizedPnLInBaseCurrency = order.TradeCostBasisPnl + order.FxCostBasisPnl;

            if (order.IsSwapped)
            {
                if (!Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EquitySwapsMarketValueAsEquity")))
                    order.MarketValueInBaseCurrency = order.CostBasisUnrealizedPnLInBaseCurrency;

                CalculatePercentageGainLoss(order);
            }

            if (order.MarketValueInBaseCurrency != 0)
            {
                order.PercentDayPnLGrossMV = order.DayPnLInBaseCurrency / Math.Abs(order.MarketValueInBaseCurrency) * 100;
                order.PercentDayPnLNetMV = order.DayPnLInBaseCurrency / order.MarketValueInBaseCurrency * 100;
            }
        }

        internal override void CalculateCashImpact(EPnlOrder order)
        {
            try
            {
                // if non cash transaction
                if (order.ClassID == EPnLClassID.EPnLOrderEquitySwap)
                {
                    order.CashImpact = 0.0;
                }
                else // cash transaction
                {
                    // Cash mgmt cash impact is not subscribed
                    //if todays date, equal to negative of notional
                    if (base.AUECWiseAdjustedDateTime.ContainsKey(order.AUECID))
                    {
                        if (order.TransactionDate.Date >= base.AUECWiseAdjustedDateTime[order.AUECID].Date)
                        {
                            order.CashImpact = order.CashImpact = -1 * order.NetNotionalValue;
                            order.SetCashImpactInBaseCurrency();
                        }
                    }
                    else // back date, cash file adjusted for this transaction
                    {
                        order.CashImpact = 0.0;
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

        internal override void CalculateMarketCap(EPnlOrder order)
        {
            try
            {
                order.MarketCapitalization = order.SharesOutstanding * order.SelectedFeedPrice;
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