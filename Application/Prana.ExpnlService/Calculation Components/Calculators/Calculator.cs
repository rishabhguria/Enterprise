using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Prana.ExpnlService
{
    public abstract class Calculator : ICalculator
    {
        private bool _calculateFxGainLossOnForexForwards = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("CalculateFxGainLossOnForexForwards"));
        private bool _calculateFxGainLossOnFutures = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("CalculateFxGainLossOnFutures"));
        private bool _calculateFxGainLossOnSwaps = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("CalculateFxGainLossOnSwaps"));
        private bool _isM2MIncludedInCash = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsM2MIncludedInCash"));

        private Dictionary<int, DateTime> _auecWiseAdjustedDateTime;

        public Dictionary<int, DateTime> AUECWiseAdjustedDateTime
        {
            get { return _auecWiseAdjustedDateTime; }
            set { _auecWiseAdjustedDateTime = value; }
        }

        private Dictionary<int, DateTime> _clearanceDateTime;

        public Dictionary<int, DateTime> ClearanceDateTime
        {
            get { return _clearanceDateTime; }
            set { _clearanceDateTime = value; }
        }

        //Template Method Pattern implemented,the actual implemenation of some method is done only in the derived calculators. So execution
        //goes to them for actual methods. But the sequence of method calling is as defined here as Calculate(EPnlOrder order) method is implemented in
        //only this base class
        public void Calculate(EPnlOrder order)
        {
            try
            {
                double yesterdayMarkPrice = FillPriceInfo(order);
                calculateYesterdayMarketValueInBaseCurrency(order);
                CalculatePNL(order, yesterdayMarkPrice);
                CalculateExposureAndMarketValue(order);
                CalculateDeltaAdjustedLME(order);
                CalculateNotional(order);
                CalculatePercentageGainLoss(order);
                CalculateCashImpact(order);
                CalculateMarketCap(order);
                CalculatePercentageAverageVolume(order);
                CalculatePercentageAverageVolumeDeltaAdjusted(order);
                CalculateFxCostBasisPnL(order);
                CalculateCounterCurrencyAmountAndPnl(order, yesterdayMarkPrice);
                CalculateFxDayPnL(order);
                CalculateTradeCostBasisPnL(order);
                CalculateTradeDayPnl(order);
                OverridePnlInBaseCurrency(order);
                CalculatePercentageGainLossCostBasis(order);
                CalculateUnderlyingValueforOptions(order);
                CalculatePremiums(order);
                CalculateOptionMoneyness(order);
                CalculateClosingMarketValue(order);
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

        public virtual void CalculateClosingMarketValue(EPnlOrder order)
        {
            return;
        }

        public virtual void CalculateOptionMoneyness(EPnlOrder order)
        {
            return;
        }

        /// <summary>
        /// This method is use to calculated two feilds for fxforword and fx which names are CounterCurrencyAmount and CounterCurrencyCostBasisPnL
        /// </summary>
        /// <param name="order">EPnlOrder</param>
        public virtual void CalculateCounterCurrencyAmountAndPnl(EPnlOrder order, double yesterdayMarkPrice)
        {
            try
            {
                return;

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


        public virtual void CalculateDeltaAdjustedLME(EPnlOrder order)
        {
            order.DeltaAdjPositionLME = order.DeltaAdjPosition;
        }

        internal abstract double FillPriceInfo(EPnlOrder order);

        internal abstract void CalculatePNL(EPnlOrder order, double yesterdayMarkPrice);

        internal abstract void CalculateExposureAndMarketValue(EPnlOrder order);

        internal abstract void CalculateCashImpact(EPnlOrder order);

        internal abstract void CalculateMarketCap(EPnlOrder order);

        internal abstract void CalculateNotional(EPnlOrder order);
        /// <summary>
        /// Code change:Ishant Kathuria:20110124 :BLOCKER:PRANA 1752:Calculating yesterdayNav here, after markprice info is filled in order
        /// </summary>
        /// <param name="epOrder"></param>

        private void calculateYesterdayMarketValueInBaseCurrency(EPnlOrder epOrder)
        {
            try
            {
                //If M2M is included in cash then futures yesterday market value should not contribute in start of day nav
                //other wise here will be double counting for future M2M.
                if (epOrder.Asset == AssetCategory.Future && _isM2MIncludedInCash)
                    return;

                if (epOrder.Asset != AssetCategory.Indices)
                {
                    if (TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates.ContainsKey(epOrder.AUECID))
                    {
                        DateTime currentDate = TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates[epOrder.AUECID];
                        //If the epOrder is of yesterday then and only then it would contribute to yesterday NAV
                        if (epOrder.TransactionDate.Date < currentDate.Date)
                        {
                            //Divya: Special case for Fx/Forwards as for them MarketValue is equal to unrealized Gain/Loss on cost basis..
                            //Gaurav(5-Oct-2012): Futures will also be handled in the same way as Fx/Forwards
                            if (epOrder.Asset == AssetCategory.FXForward || epOrder.Asset == AssetCategory.FX || epOrder.Asset == AssetCategory.Future)
                            {
                                double fxCostBasisPnL = 0.0;
                                double tradeCostBasisPnl = 0.0;
                                double tradeCostBasisLocalPnl = 0.0;

                                if ((epOrder.Asset == AssetCategory.FXForward || epOrder.Asset == AssetCategory.FX) && _calculateFxGainLossOnForexForwards)
                                {
                                    fxCostBasisPnL = epOrder.Quantity * epOrder.Multiplier * epOrder.SideMultiplier * epOrder.AvgPrice * (epOrder.YesterdayFXRate - epOrder.FXRateOnTradeDate);
                                }
                                if (epOrder.Asset == AssetCategory.Future && _calculateFxGainLossOnFutures)
                                {
                                    fxCostBasisPnL = epOrder.Quantity * epOrder.Multiplier * epOrder.SideMultiplier * epOrder.AvgPrice * (epOrder.YesterdayFXRate - epOrder.FXRateOnTradeDate);
                                }

                                tradeCostBasisLocalPnl = (epOrder.YesterdayMarkPrice - epOrder.AvgPrice) * epOrder.Quantity * epOrder.Multiplier * epOrder.SideMultiplier;
                                tradeCostBasisPnl = tradeCostBasisLocalPnl * epOrder.YesterdayFXRate;

                                double YesterdayMarketValueInBaseCurrency = tradeCostBasisPnl + fxCostBasisPnL;
                                epOrder.YesterdayFXRate = epOrder.YesterdayFXRate;
                                epOrder.YesterdayMarketValue = tradeCostBasisLocalPnl;
                                epOrder.YesterdayMarketValueInBaseCurrency = YesterdayMarketValueInBaseCurrency;
                            }
                            else
                            {
                                if (epOrder.ClassID == EPnLClassID.EPnLOrderEquitySwap && !Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EquitySwapsMarketValueAsEquity")))
                                {
                                    double yesterdayfxCostBasisPnL = 0.0;
                                    if (_calculateFxGainLossOnSwaps)
                                    {
                                        yesterdayfxCostBasisPnL = CalculateYesterdayFxCostBasisPnLForSwaps(epOrder);
                                    }

                                    CalculateYesterdayMarketValueForSwaps(epOrder, yesterdayfxCostBasisPnL);
                                }
                                else
                                {
                                    double yesterdayMarketValueLocal = BusinessLogic.Calculations.GetMarketValue(epOrder.Quantity, epOrder.YesterdayMarkPrice, epOrder.Multiplier, epOrder.SideMultiplier);
                                    epOrder.YesterdayMarketValue = yesterdayMarketValueLocal;
                                    epOrder.SetYesterdayMarketValueInBaseCurrency();
                                }
                            }
                        }
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

        private double CalculateYesterdayFxCostBasisPnLForSwaps(EPnlOrder order)
        {
            try
            {
                return (order.Quantity * order.Multiplier * order.SideMultiplier * order.AvgPrice * (order.YesterdayFXRate - order.FXRateOnTradeDate));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }

        private void CalculateYesterdayMarketValueForSwaps(EPnlOrder order, double yesterdayfxCostBasisPnL)
        {
            try
            {
                double marketValueInBaseCurrency = 0;
                double yesterdaymarketValue = 0.0;
                yesterdaymarketValue = BusinessLogic.Calculations.GetPnL(order.Quantity, order.YesterdayMarkPrice, order.AvgPrice, order.Multiplier, order.SideMultiplier);
                double swapTotalInterest = ((EPnLOrderEquitySwap)order).TotalInterest;
                marketValueInBaseCurrency = yesterdaymarketValue * order.YesterdayFXRate;
                switch (order.OrderSideTagValue)
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Closed:
                    case FIXConstants.SIDE_Buy_Cover:
                    case FIXConstants.SIDE_Buy_Open:
                    case FIXConstants.SIDE_BuyMinus:
                        yesterdaymarketValue -= swapTotalInterest;
                        marketValueInBaseCurrency -= (swapTotalInterest * order.YesterdayFXRate);
                        break;

                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_Sell_Closed:
                    case FIXConstants.SIDE_Sell_Open:
                    case FIXConstants.SIDE_SellPlus:
                    case FIXConstants.SIDE_SellShort:
                        yesterdaymarketValue += swapTotalInterest;
                        marketValueInBaseCurrency += (swapTotalInterest * order.YesterdayFXRate);
                        break;

                    default:
                        break;
                }
                order.YesterdayMarketValue = yesterdaymarketValue;
                order.YesterdayMarketValueInBaseCurrency = marketValueInBaseCurrency + yesterdayfxCostBasisPnL;
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

        public virtual void CalculateUnderlyingValueforOptions(EPnlOrder order)
        {
            order.UnderlyingValueForOptions = 0;
        }

        public virtual void CalculatePercentageGainLoss(EPnlOrder order)
        {
            try
            {
                if (order.NetNotionalValueInBaseCurrency != 0.0)
                {
                    // Divya Bansal : 15062012 : http://jira.nirvanasolutions.com:8080/browse/KIN-5
                    order.PercentageGainLoss = (order.DayPnLInBaseCurrency / Math.Abs(order.NetNotionalValueInBaseCurrency)) * ApplicationConstants.PERCENTAGEVALUE;
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

        /// <summary>
        /// Calculates the trade day PNL.
        /// </summary>
        /// <param name="order">The order.</param>
        private static void CalculateTradeDayPnl(EPnlOrder order)
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
                                order.TradeDayPnl = ((EPnLOrderFX)order).CounterCurrencyDayPnL;
                            else
                                order.TradeDayPnl = (order.DayPnL * order.SelectedFeedPrice);

                            break;
                        default:
                            if ((order.CurrencyID != baseCurrency))
                                order.TradeDayPnl = ((EPnLOrderFX)order).CounterCurrencyDayPnL;
                            else
                                order.TradeDayPnl = order.SelectedFeedPrice != 0 ? (order.DayPnL / order.SelectedFeedPrice) : 0;

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
                                order.TradeDayPnl = ((EPnLOrderFXForward)order).CounterCurrencyDayPnL;
                            else
                                order.TradeDayPnl = (order.DayPnL * order.SelectedFeedPrice);

                            break;
                        default:
                            if ((order.CurrencyID != baseCurrency))
                                order.TradeDayPnl = ((EPnLOrderFXForward)order).CounterCurrencyDayPnL;
                            else
                                order.TradeDayPnl = order.SelectedFeedPrice != 0 ? (order.DayPnL / order.SelectedFeedPrice) : 0;

                            break;
                    }
                }
                else
                    order.TradeDayPnl = (order.DayPnL * order.FxRate);
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
        /// Calculates the FX P&L (on cost) since taxlot inception. Based on Current fx rate and the fx rate on trade date.
        /// </summary>
        /// <param name="order"></params>
        public virtual void CalculateFxCostBasisPnL(EPnlOrder order)
        {
            //( Current FX - FX on Trade ) * QTY * Multiplier * Cost Basis
            try
            {
                double currentFXRate = order.FxRate;
                double previousFXRate = order.FXRateOnTradeDate;
                if (previousFXRate == double.MinValue)
                {
                    previousFXRate = 0.0;
                }
                if (currentFXRate == double.MinValue)
                {
                    currentFXRate = 0.0;
                }
                if (order.Asset == AssetCategory.FX || order.Asset == AssetCategory.FXForward)
                    order.FxCostBasisPnl = 0;
                else
                    order.FxCostBasisPnl = order.Quantity * order.Multiplier * order.SideMultiplier * order.AvgPrice * (currentFXRate - previousFXRate);
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
        public virtual void CalculatePremiums(EPnlOrder order)
        {
            order.Premium = 0.0;
            order.PremiumDollar = 0.0;
        }

        /// <summary>
        /// Currently used only for FX and Futures, to override Day P&L Base calculation
        /// Day P&L Base = Day P&L Base(Trading) + Day P&L Base(FX)
        /// </summary>
        /// <param name="order"></param>
        public virtual void OverridePnlInBaseCurrency(EPnlOrder order)
        {
            return;
        }

        /// <summary>
        /// Calculates the Trading P&L (on cost) since taxlot inception. Based on Selected Feed price of security vs the cost basis on trade date. Uses current fx rate to convert the P&L to base currency
        /// </summary>
        /// <param name="order"></param>
        public virtual void CalculateTradeCostBasisPnL(EPnlOrder order)
        {
            //Last PX - Cost Basis ) * QTY * Multiplier * Current FX
            try
            {
                if (!order.FxRate.Equals(0))
                {
                    order.TradeCostBasisPnl = order.CostBasisUnrealizedPnL * order.FxRate;
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

        public virtual void CalculateFxDayPnL(EPnlOrder order)
        {
            try
            {
                int companyId = CachedDataManager.GetInstance.GetCompanyID();
                int companyBaseCurrencyId = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();

                if (companyBaseCurrencyId == order.CurrencyID)
                {
                    //Ashish Poddar 20120223: Since Trade Currency equals Base currency, the FX gain loss on security will be zero.
                    return;
                }

                DateTime currentDate = TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates[order.AUECID];
                ConversionRate latestAvailablefxRate = new ConversionRate();
                double availablePreviousFxRate = 0.0;

                if (order.TransactionDate.Date >= currentDate.Date)
                {
                    if (!order.FXRateOnTradeDate.Equals(0))
                    {
                        availablePreviousFxRate = order.FXRateOnTradeDate;
                    }
                    else if (order.YesterdayFXRate != 0)
                    {
                        availablePreviousFxRate = order.YesterdayFXRate;
                    }
                    else
                    {
                        latestAvailablefxRate = ForexConverter.GetInstance(companyId).GetLatestAvailableFxRatesLessThanToday(order.CurrencyID);
                        availablePreviousFxRate = latestAvailablefxRate.ConversionMethod == Operator.M ? latestAvailablefxRate.RateValue : 1 / latestAvailablefxRate.RateValue;
                        if (availablePreviousFxRate == double.MinValue)
                        {
                            availablePreviousFxRate = 0.0;
                        }
                    }
                    Calculate_GenericFXDayPNL(order, order.AvgPrice, order.FxRate, availablePreviousFxRate);
                }
                else
                {
                    if (order.YesterdayFXRate != 0)
                    {
                        availablePreviousFxRate = order.YesterdayFXRate;
                    }
                    else
                    {
                        latestAvailablefxRate = ForexConverter.GetInstance(companyId).GetLatestAvailableFxRatesLessThanToday(order.CurrencyID);
                        availablePreviousFxRate = latestAvailablefxRate.ConversionMethod == Operator.M ? latestAvailablefxRate.RateValue : 1 / latestAvailablefxRate.RateValue;
                    }
                    Calculate_GenericFXDayPNL(order, order.YesterdayMarkPrice, order.FxRate, availablePreviousFxRate);
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

        /// <summary>
        /// Calculates the generic fx day PNL.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="selectedSecurityPriceLocal">The selected security price local.</param>
        /// <param name="currentFXRate">The current fx rate.</param>
        /// <param name="availablePreviousFxRate">The available previous fx rate.</param>
        public virtual void Calculate_GenericFXDayPNL(EPnlOrder order, double selectedSecurityPriceLocal, double currentFXRate, double availablePreviousFxRate)
        {
            try
            {
                if (order.Asset == AssetCategory.FX || order.Asset == AssetCategory.FXForward)
                    order.FxDayPnl = 0.0;
                else
                {
                    order.FxDayPnl = (order.Quantity) * (order.Multiplier) * order.SideMultiplier * (selectedSecurityPriceLocal) * (currentFXRate - availablePreviousFxRate);
                    //Gaurav: If either currentFXRate or availablePreviousFxRate is equal to double.min/max value then FxDayPnl can be infinity
                    if (double.IsInfinity(order.FxDayPnl))
                        order.FxDayPnl = 0;
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

        /// <summary>
        /// calculates both - exposure in bp and ifexecuted exposure in bp
        /// </summary>
        /// <param name="order"></param>
        internal static void CalculatePercentageAverageVolumeDeltaAdjusted(EPnlOrder order)
        {
            try
            {
                if (!order.AverageVolume20DayUnderlyingSymbol.Equals(0))
                {
                    order.PercentageAverageVolumeDeltaAdjusted = ((order.DeltaAdjPosition) / (order.AverageVolume20DayUnderlyingSymbol)) * 100;
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

        // the following calculation is on the basis of PRANA-917 : sudhanshu
        internal static void CalculatePercentageAverageVolume(EPnlOrder order)
        {
            try
            {
                // Since for options also, we are putting OpenInterest in the SharesOutstanding, hence used it.
                if (order.AverageVolume20Day != 0)
                {
                    order.PercentageAverageVolume = ((order.Quantity * 100 * order.SideMultiplier) / (order.AverageVolume20Day));
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

        private static void CalculatePercentageGainLossCostBasis(EPnlOrder order)
        {
            try
            {
                if (order.NetNotionalValueInBaseCurrency != 0.0)
                {
                    order.PercentageGainLossCostBasis = (order.CostBasisUnrealizedPnLInBaseCurrency / Math.Abs(order.NetNotionalValueInBaseCurrency)) * ApplicationConstants.PERCENTAGEVALUE;
                }
                else
                {
                    order.PercentageGainLossCostBasis = 0.0;
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
    }
}
