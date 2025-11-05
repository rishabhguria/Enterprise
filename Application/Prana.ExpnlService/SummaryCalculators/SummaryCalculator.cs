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
    public class SummaryCalculator
    {
        private object _lockerObj = new object();
        private int _creditLimitCalculationBasis;
        private bool _creditLimitBoxPositionAllowed;
        private Dictionary<int, DistinctAccountSetWiseSummaryCollection> _distinctAccountSetWiseSummaryCollection;
        #region Singleton Instance
        //private static SummaryCalculator _summaryCalculator;

        //changing this to public to create multiple instances, single instance of this class is used in DataCalculator class, PRANA-26540
        public SummaryCalculator()
        {
            _distinctAccountSetWiseSummaryCollection = new Dictionary<int, DistinctAccountSetWiseSummaryCollection>();
            _creditLimitCalculationBasis = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("CreditLimitCalculationBasis"));
            _creditLimitBoxPositionAllowed = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("CreditLimitBoxPositionAllowed"));
        }

        //public static SummaryCalculator GetInstance()
        //{
        //    if (_summaryCalculator == null)
        //    {
        //        _summaryCalculator = new SummaryCalculator();
        //    }
        //    return _summaryCalculator;
        //}
        #endregion

        public void ClearCache()
        {
            try
            {
                lock (_lockerObj)
                {
                    _distinctAccountSetWiseSummaryCollection.Clear();
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

        public void AddOrder(EPnlOrder exposureAndPnlOrder, Dictionary<int, List<int>> distinctAccountPermissionSets)
        {
            try
            {
                lock (_lockerObj)
                {
                    UpdateDistictAccountSetWiseSummary(exposureAndPnlOrder, distinctAccountPermissionSets);
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

        private void UpdateDistictAccountSetWiseSummary(EPnlOrder exposureAndPnlOrder, Dictionary<int, List<int>> distinctAccountPermissionSets)
        {
            try
            {
                ExposureAndPnlOrderSummary symbolWiseSummary = null;
                ExposureAndPnlOrderSummary underlyingSymbolWiseSummary = null;
                foreach (KeyValuePair<int, List<int>> kvp in distinctAccountPermissionSets)
                {
                    if (kvp.Value != null && kvp.Value.Count == 1 && kvp.Value.Contains(exposureAndPnlOrder.Level1ID))
                    {
                        if (_distinctAccountSetWiseSummaryCollection.ContainsKey(kvp.Key))
                        {
                            DistinctAccountSetWiseSummaryCollection distinctAccountSetWiseSummaryCollection = _distinctAccountSetWiseSummaryCollection[kvp.Key];
                            if (distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary.ContainsKey(exposureAndPnlOrder.Symbol))
                            {
                                symbolWiseSummary = CalculateSummaryFromOrder(exposureAndPnlOrder, distinctAccountSetWiseSummaryCollection.SymbolWiseGroupSummary[exposureAndPnlOrder.Symbol]);
                            }
                            else
                            {
                                symbolWiseSummary = CalculateSummaryFromOrder(exposureAndPnlOrder, new ExposureAndPnlOrderSummary());
                            }

                            if (distinctAccountSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary.ContainsKey(exposureAndPnlOrder.UnderlyingSymbol))
                            {
                                underlyingSymbolWiseSummary = CalculateSummaryFromOrder(exposureAndPnlOrder, distinctAccountSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[exposureAndPnlOrder.UnderlyingSymbol]);
                            }
                            else
                            {
                                underlyingSymbolWiseSummary = CalculateSummaryFromOrder(exposureAndPnlOrder, new ExposureAndPnlOrderSummary());
                            }
                        }
                        else
                        {
                            _distinctAccountSetWiseSummaryCollection.Add(kvp.Key, new DistinctAccountSetWiseSummaryCollection());
                            symbolWiseSummary = CalculateSummaryFromOrder(exposureAndPnlOrder, new ExposureAndPnlOrderSummary());
                            underlyingSymbolWiseSummary = CalculateSummaryFromOrder(exposureAndPnlOrder, new ExposureAndPnlOrderSummary());
                        }
                        if (symbolWiseSummary != null)
                        {
                            AssignLongShort(symbolWiseSummary);
                        }

                        if (underlyingSymbolWiseSummary != null)
                        {
                            AssignLongShort(underlyingSymbolWiseSummary);
                        }
                        _distinctAccountSetWiseSummaryCollection[kvp.Key].SymbolWiseGroupSummary[exposureAndPnlOrder.Symbol] = symbolWiseSummary;
                        _distinctAccountSetWiseSummaryCollection[kvp.Key].UnderlyingSymbolWiseGroupSummary[exposureAndPnlOrder.UnderlyingSymbol] = underlyingSymbolWiseSummary;
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

        private ExposureAndPnlOrderSummary CalculateSummaryFromOrder(EPnlOrder exposureAndPnlOrder, ExposureAndPnlOrderSummary summaryExPnlOrderSummary)
        {
            try
            {
                if (String.IsNullOrEmpty(summaryExPnlOrderSummary.CurrencyID))
                {
                    summaryExPnlOrderSummary.CurrencyID = exposureAndPnlOrder.CurrencyID.ToString();
                }
                if ((!summaryExPnlOrderSummary.CurrencyID.Equals(exposureAndPnlOrder.CurrencyID.ToString())) && summaryExPnlOrderSummary.CurrencyID != ApplicationConstants.C_Multiple)
                {
                    summaryExPnlOrderSummary.CurrencyID = ApplicationConstants.C_Multiple;
                }

                //Kashish G., PRANA-32392
                decimal sum_dayPnL = (decimal)summaryExPnlOrderSummary.DayPnL;
                decimal dayPnL = (decimal)exposureAndPnlOrder.DayPnLInBaseCurrency;
                decimal totaldayPnL = sum_dayPnL + dayPnL;
                summaryExPnlOrderSummary.DayPnL = (double)totaldayPnL;

                summaryExPnlOrderSummary.CashProjected += exposureAndPnlOrder.CashImpactInBaseCurrency;
                if (exposureAndPnlOrder.CashImpactInBaseCurrency > 0)
                {
                    summaryExPnlOrderSummary.CashInflow += exposureAndPnlOrder.CashImpactInBaseCurrency;
                }
                else
                {
                    summaryExPnlOrderSummary.CashOutflow += Math.Abs(exposureAndPnlOrder.CashImpactInBaseCurrency);
                }

                #region Daily Credit Limit
                if (_creditLimitCalculationBasis == 0)
                {
                    double tempCashImpact = exposureAndPnlOrder.CashImpactInBaseCurrency * -1;
                    if (_creditLimitBoxPositionAllowed)
                    {
                        switch (exposureAndPnlOrder.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Sell:
                            case FIXConstants.SIDE_Buy_Open:
                            case FIXConstants.SIDE_Sell_Closed:
                                summaryExPnlOrderSummary.LongDebitBalance += tempCashImpact;
                                break;

                            case FIXConstants.SIDE_SellShort:
                            case FIXConstants.SIDE_Sell_Open:
                            case FIXConstants.SIDE_Buy_Closed:
                                summaryExPnlOrderSummary.ShortCreditBalance += tempCashImpact * -1;
                                break;
                        }
                    }
                    else
                    {
                        double newPositions = exposureAndPnlOrder.Quantity * exposureAndPnlOrder.SideMultiplier;
                        if (summaryExPnlOrderSummary.NetPosition >= 0 && (summaryExPnlOrderSummary.NetPosition + newPositions) >= 0)
                        {
                            summaryExPnlOrderSummary.LongDebitBalance += tempCashImpact;
                        }
                        else if (summaryExPnlOrderSummary.NetPosition <= 0 && (summaryExPnlOrderSummary.NetPosition + newPositions) <= 0)
                        {
                            summaryExPnlOrderSummary.ShortCreditBalance += tempCashImpact * -1;
                        }
                        else if (summaryExPnlOrderSummary.NetPosition >= 0 && (summaryExPnlOrderSummary.NetPosition + newPositions) < 0)
                        {
                            summaryExPnlOrderSummary.LongDebitBalance += tempCashImpact * Math.Abs(summaryExPnlOrderSummary.NetPosition / newPositions);
                            summaryExPnlOrderSummary.ShortCreditBalance += tempCashImpact * -1 * Math.Abs(summaryExPnlOrderSummary.NetPosition + newPositions) / Math.Abs(newPositions);
                        }
                        else if (summaryExPnlOrderSummary.NetPosition <= 0 && (summaryExPnlOrderSummary.NetPosition + newPositions) > 0)
                        {
                            summaryExPnlOrderSummary.ShortCreditBalance += tempCashImpact * -1 * Math.Abs(summaryExPnlOrderSummary.NetPosition / newPositions);
                            summaryExPnlOrderSummary.LongDebitBalance += tempCashImpact * Math.Abs(summaryExPnlOrderSummary.NetPosition + newPositions) / Math.Abs(newPositions);
                        }
                    }
                }
                #endregion Daily Credit Limit

                double calculatedPositions = (exposureAndPnlOrder.Quantity * exposureAndPnlOrder.SideMultiplier);
                summaryExPnlOrderSummary.NetPosition += calculatedPositions;
                if (TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates.ContainsKey(exposureAndPnlOrder.AUECID))
                {
                    DateTime currentDate = TimeZoneHelper.GetInstance().CurrentOffsetAdjustedAUECDates[exposureAndPnlOrder.AUECID];
                    //If the epOrder is of yesterday only then it would contribute to yesterday Net Positions
                    if (exposureAndPnlOrder.TransactionDate.Date < currentDate.Date)
                    {
                        summaryExPnlOrderSummary.YesterdayNetPosition += calculatedPositions;
                    }

                    if (exposureAndPnlOrder.TransactionDate.Date == currentDate.Date)
                    {
                        summaryExPnlOrderSummary.TodayNetPosition += calculatedPositions;
                    }
                }
                summaryExPnlOrderSummary.BetaAdjustedExposure += exposureAndPnlOrder.BetaAdjExposureInBaseCurrency;
                summaryExPnlOrderSummary.YesterdayNAV += exposureAndPnlOrder.YesterdayMarketValueInBaseCurrency;
                // Kuldeep A.: Below code is just for short term to handle Position Side Exposure correctly.
                // For permanent solution, the column data type should be changed to decimal..which will be scheduled later.
                decimal sum_netExp = (decimal)summaryExPnlOrderSummary.NetExposure;
                decimal netExp = (decimal)exposureAndPnlOrder.NetExposureInBaseCurrency;
                decimal total = sum_netExp + netExp;
                summaryExPnlOrderSummary.NetExposure = (double)total;

                if (summaryExPnlOrderSummary.NetExposure != 0)
                {
                    summaryExPnlOrderSummary.NetExposureBeforeZero = summaryExPnlOrderSummary.NetExposure;
                }
                if (summaryExPnlOrderSummary.NetPosition != 0)
                {
                    summaryExPnlOrderSummary.PositionBeforeZero = summaryExPnlOrderSummary.NetPosition;
                }

                summaryExPnlOrderSummary.NetExposureLocal += exposureAndPnlOrder.NetExposure;

                //Kashish G., PRANA-32392
                decimal sum_netMV = (decimal)summaryExPnlOrderSummary.NetMarketValue;
                decimal netMV = (decimal)exposureAndPnlOrder.MarketValueInBaseCurrency;
                decimal totalnetMV = sum_netMV + netMV;
                summaryExPnlOrderSummary.NetMarketValue = (double)totalnetMV;

                summaryExPnlOrderSummary.CostBasisPNL += exposureAndPnlOrder.CostBasisUnrealizedPnLInBaseCurrency;
                summaryExPnlOrderSummary.UnderlyingValueForOptions += exposureAndPnlOrder.UnderlyingValueForOptions;
                summaryExPnlOrderSummary.EarnedDividendBase += exposureAndPnlOrder.EarnedDividendBase;
                summaryExPnlOrderSummary.YesterdayMarketValue += exposureAndPnlOrder.YesterdayMarketValueInBaseCurrency;
                summaryExPnlOrderSummary.MTDPnL += exposureAndPnlOrder.DayPnLInBaseCurrency;
                summaryExPnlOrderSummary.QTDPnL += exposureAndPnlOrder.DayPnLInBaseCurrency;
                summaryExPnlOrderSummary.YTDPnL += exposureAndPnlOrder.DayPnLInBaseCurrency;

                if (exposureAndPnlOrder.Asset == AssetCategory.EquityOption || exposureAndPnlOrder.Asset == AssetCategory.FutureOption)
                {
                    summaryExPnlOrderSummary.AverageVolume20Day = exposureAndPnlOrder.SharesOutstanding;
                }
                else
                {
                    summaryExPnlOrderSummary.AverageVolume20Day = exposureAndPnlOrder.AverageVolume20Day;
                }

                summaryExPnlOrderSummary.LeverageFactor = exposureAndPnlOrder.LeveragedFactor;
                summaryExPnlOrderSummary.Beta = exposureAndPnlOrder.Beta;
                summaryExPnlOrderSummary.Level1ID = exposureAndPnlOrder.Level1ID;
                summaryExPnlOrderSummary.UnderlyingSymbol = exposureAndPnlOrder.UnderlyingSymbol;
                summaryExPnlOrderSummary.Symbol = exposureAndPnlOrder.Symbol;
                summaryExPnlOrderSummary.MasterfundID = exposureAndPnlOrder.MasterFundID;
                summaryExPnlOrderSummary.GrossMarketValue = Math.Abs(summaryExPnlOrderSummary.NetMarketValue);
                summaryExPnlOrderSummary.GrossExposure = Math.Abs(summaryExPnlOrderSummary.NetExposure);
                summaryExPnlOrderSummary.GrossExposureLocal = Math.Abs(summaryExPnlOrderSummary.NetExposureLocal);
                summaryExPnlOrderSummary.AssetCategory = exposureAndPnlOrder.Asset;

                if (summaryExPnlOrderSummary.AssetCategory.Equals(AssetCategory.FX) || summaryExPnlOrderSummary.AssetCategory.Equals(AssetCategory.FXForward))
                {
                    summaryExPnlOrderSummary.BetaAdjustedGrossExposure = summaryExPnlOrderSummary.GrossExposure * Math.Abs(summaryExPnlOrderSummary.Beta);
                    summaryExPnlOrderSummary.BetaAdjustedGrossExposureLocal = summaryExPnlOrderSummary.GrossExposureLocal * Math.Abs(summaryExPnlOrderSummary.Beta);
                }
                else
                {
                    if (summaryExPnlOrderSummary.LeverageFactor != 0)
                    {
                        summaryExPnlOrderSummary.BetaAdjustedGrossExposure = (Math.Abs((summaryExPnlOrderSummary.GrossExposure / summaryExPnlOrderSummary.LeverageFactor)) * Math.Abs(summaryExPnlOrderSummary.Beta));
                        summaryExPnlOrderSummary.BetaAdjustedGrossExposureLocal = (Math.Abs((summaryExPnlOrderSummary.GrossExposureLocal / summaryExPnlOrderSummary.LeverageFactor)) * Math.Abs(summaryExPnlOrderSummary.Beta));
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
            return summaryExPnlOrderSummary;
        }

        private void AssignLongShort(ExposureAndPnlOrderSummary symbolWiseSummary)
        {
            try
            {
                if (symbolWiseSummary.NetPosition > 0)
                {
                    symbolWiseSummary.PositionSideMV = PositionType.Long;
                }
                else if (symbolWiseSummary.NetPosition < 0)
                {
                    symbolWiseSummary.PositionSideMV = PositionType.Short;
                }
                else
                {
                    symbolWiseSummary.PositionSideMV = symbolWiseSummary.PositionBeforeZero > 0 ? PositionType.Long : PositionType.Short;
                }

                if (symbolWiseSummary.NetExposure > 0)
                {
                    symbolWiseSummary.PositionSideExposure = PositionType.Long;
                }
                else if (symbolWiseSummary.NetExposure < 0)
                {
                    symbolWiseSummary.PositionSideExposure = PositionType.Short;
                }
                else
                {
                    if (symbolWiseSummary.NetExposureBeforeZero == 0)
                    {
                        symbolWiseSummary.PositionSideExposure = PositionType.Long;
                    }
                    else
                    {
                        symbolWiseSummary.PositionSideExposure = symbolWiseSummary.NetExposureBeforeZero > 0 ? PositionType.Long : PositionType.Short;
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

        public void CalculatePreExistingSummary(Dictionary<int, List<int>> distinctAccountPermissionSets, Dictionary<int, ExposureAndPnlOrderSummary> accountwiseSummary)
        {
            try
            {
                lock (_lockerObj)
                {
                    foreach (KeyValuePair<int, List<int>> kvp in distinctAccountPermissionSets)
                    {
                        if (!_distinctAccountSetWiseSummaryCollection.ContainsKey(kvp.Key))
                        {
                            DistinctAccountSetWiseSummaryCollection tempDistinctAccountSetWiseSummaryCollection = new DistinctAccountSetWiseSummaryCollection();
                            _distinctAccountSetWiseSummaryCollection.Add(kvp.Key, tempDistinctAccountSetWiseSummaryCollection);
                        }

                        if (accountwiseSummary != null)
                        {
                            foreach (ExposureAndPnlOrderSummary uncalculatedSummary in accountwiseSummary.Values)
                            {
                                //If the NAV is picked from DB, we will override it here only.
                                if (kvp.Value.Contains(uncalculatedSummary.Level1ID) && uncalculatedSummary.Level1ID != int.MinValue) //As we are already adding the account wise summary to get consolidation summary. Thus excluding int.min value here
                                {
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.CashProjected += uncalculatedSummary.CashProjected;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.CashInflow += uncalculatedSummary.CashInflow;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.CashOutflow += uncalculatedSummary.CashOutflow;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.StartOfDayCash += uncalculatedSummary.StartOfDayCash;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.YesterdayNAV += uncalculatedSummary.YesterdayNAV;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.NetAssetValue += uncalculatedSummary.NetAssetValue;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.StartOfDayAccruals += uncalculatedSummary.StartOfDayAccruals;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.DayAccruals += uncalculatedSummary.DayAccruals;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.LongDebitLimit += uncalculatedSummary.LongDebitLimit;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.ShortCreditLimit += uncalculatedSummary.ShortCreditLimit;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.LongDebitBalance += uncalculatedSummary.LongDebitBalance;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.ShortCreditBalance += uncalculatedSummary.ShortCreditBalance;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.MTDReturn += uncalculatedSummary.MTDReturn;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.YTDReturn += uncalculatedSummary.YTDReturn;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.QTDReturn += uncalculatedSummary.QTDReturn;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.MTDPnL += uncalculatedSummary.MTDPnL;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.QTDPnL += uncalculatedSummary.QTDPnL;
                                    _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary.YTDPnL += uncalculatedSummary.YTDPnL;
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

        public void CalculateConsolidationSummary(Dictionary<int, List<int>> distinctAccountPermissionSets, bool isNAVSaved)
        {
            try
            {
                lock (_lockerObj)
                {
                    foreach (KeyValuePair<int, List<int>> kvp in distinctAccountPermissionSets)
                    {
                        if (_distinctAccountSetWiseSummaryCollection.ContainsKey(kvp.Key))
                        {
                            Dictionary<string, ExposureAndPnlOrderSummary> symbolWiseGroupSummary = _distinctAccountSetWiseSummaryCollection[kvp.Key].SymbolWiseGroupSummary;
                            Dictionary<string, ExposureAndPnlOrderSummary> underlyingSymbolWiseGroupSummary = _distinctAccountSetWiseSummaryCollection[kvp.Key].UnderlyingSymbolWiseGroupSummary;
                            ExposureAndPnlOrderSummary consolidatedSummary = _distinctAccountSetWiseSummaryCollection[kvp.Key].ConsolidationDashBoardSummary;
                            GetTotalSummary(symbolWiseGroupSummary, underlyingSymbolWiseGroupSummary, ref consolidatedSummary, isNAVSaved);
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

        private void GetTotalSummary(Dictionary<string, ExposureAndPnlOrderSummary> symbolWiseGroupSummary, Dictionary<string, ExposureAndPnlOrderSummary> underlyingSymbolWiseGroupSummary, ref ExposureAndPnlOrderSummary consolidatedSummary, bool isNAVSaved)
        {
            try
            {
                foreach (KeyValuePair<string, ExposureAndPnlOrderSummary> symbolWiseSummary in symbolWiseGroupSummary)
                {
                    FillLongShortBasedOnPositionSideInSummary(symbolWiseSummary.Value);
                    CalculateSummaryFromSummary(symbolWiseSummary.Value, consolidatedSummary, isNAVSaved, true);

                    #region Daily Credit Limit
                    if (_creditLimitCalculationBasis == 1)
                    {
                        if (symbolWiseSummary.Value.NetPosition > 0)
                        {
                            consolidatedSummary.LongDebitBalance += symbolWiseSummary.Value.CashOutflow - symbolWiseSummary.Value.CashInflow;
                        }
                        else
                        {
                            consolidatedSummary.ShortCreditBalance += symbolWiseSummary.Value.CashInflow - symbolWiseSummary.Value.CashOutflow;
                        }
                    }
                    #endregion Daily Credit Limit

                    if (consolidatedSummary.GrossExposure != 0)
                    {
                        consolidatedSummary.NetPercentExposureGross = consolidatedSummary.NetExposure / consolidatedSummary.GrossExposure;
                    }
                }

                MergeUnderlyingSummaryAndConsolidatedSummary(consolidatedSummary, underlyingSymbolWiseGroupSummary);
                ClearUnnecessaryConsolidatedSummaryField(consolidatedSummary);
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

        private void CalculateSummaryFromSummary(ExposureAndPnlOrderSummary inputSummary, ExposureAndPnlOrderSummary finalSummary, bool isNAVSaved, bool isDashboardSummary)
        {
            try
            {
                finalSummary.NetExposure += inputSummary.NetExposure;
                finalSummary.LongExposure += inputSummary.LongExposure;
                finalSummary.ShortExposure += inputSummary.ShortExposure;
                finalSummary.BetaAdjustedExposure += inputSummary.BetaAdjustedExposure;
                finalSummary.BetaAdjustedLongExposure += inputSummary.BetaAdjustedLongExposure;
                finalSummary.BetaAdjustedShortExposure += inputSummary.BetaAdjustedShortExposure;

                finalSummary.LongMarketValue += inputSummary.LongMarketValue;
                finalSummary.ShortMarketValue += inputSummary.ShortMarketValue;
                finalSummary.NetMarketValue += inputSummary.NetMarketValue;

                if (inputSummary.CurrencyID == ApplicationConstants.C_Multiple)
                {
                    finalSummary.NetExposureLocal = 0;
                }
                else
                {
                    finalSummary.NetExposureLocal += inputSummary.NetExposureLocal;
                }
                if (finalSummary.NetExposure != 0)
                {
                    finalSummary.NetExposureBeforeZero = finalSummary.NetExposure;
                }
                else
                {
                    finalSummary.NetExposureBeforeZero = inputSummary.NetExposureBeforeZero;
                }

                if (isDashboardSummary)
                {
                    finalSummary.LeverageFactor = 0;
                    finalSummary.Beta = 0;
                    finalSummary.AssetCategory = AssetCategory.None;
                    finalSummary.GrossExposure += inputSummary.GrossExposure;
                    finalSummary.GrossMarketValue += inputSummary.GrossMarketValue;
                    if (inputSummary.AssetCategory.Equals(AssetCategory.FX) || inputSummary.AssetCategory.Equals(AssetCategory.FXForward))
                    {
                        finalSummary.BetaAdjustedGrossExposure += inputSummary.GrossExposure * Math.Abs(inputSummary.Beta);
                    }
                    else
                    {
                        if (inputSummary.LeverageFactor != 0)
                        {
                            finalSummary.BetaAdjustedGrossExposure += inputSummary.GrossExposure * Math.Abs(inputSummary.Beta) / inputSummary.LeverageFactor;
                        }
                    }

                    if (inputSummary.CurrencyID == ApplicationConstants.C_Multiple)
                    {
                        finalSummary.GrossExposureLocal = 0;
                        finalSummary.BetaAdjustedGrossExposureLocal = 0;
                    }
                    else
                    {
                        finalSummary.GrossExposureLocal += inputSummary.GrossExposureLocal;
                        if (inputSummary.AssetCategory.Equals(AssetCategory.FX) || inputSummary.AssetCategory.Equals(AssetCategory.FXForward))
                        {
                            finalSummary.BetaAdjustedGrossExposureLocal += inputSummary.GrossExposureLocal * Math.Abs(inputSummary.Beta);
                        }
                        else
                        {
                            if (inputSummary.LeverageFactor != 0)
                            {
                                finalSummary.BetaAdjustedGrossExposureLocal += inputSummary.GrossExposureLocal * Math.Abs(inputSummary.Beta) / inputSummary.LeverageFactor;
                            }
                        }
                    }
                }
                else
                {
                    finalSummary.LeverageFactor = inputSummary.LeverageFactor;
                    finalSummary.Beta = inputSummary.Beta;
                    finalSummary.AssetCategory = inputSummary.AssetCategory;
                    finalSummary.GrossExposure = Math.Abs(finalSummary.NetExposure);
                    finalSummary.GrossMarketValue = Math.Abs(finalSummary.NetMarketValue);
                    if (finalSummary.AssetCategory.Equals(AssetCategory.FX) || finalSummary.AssetCategory.Equals(AssetCategory.FXForward))
                    {
                        finalSummary.BetaAdjustedGrossExposure = finalSummary.GrossExposure * Math.Abs(finalSummary.Beta);
                    }
                    else
                    {
                        if (finalSummary.LeverageFactor != 0)
                        {
                            finalSummary.BetaAdjustedGrossExposure = finalSummary.GrossExposure * Math.Abs(finalSummary.Beta) / finalSummary.LeverageFactor;
                        }
                    }

                    if (inputSummary.CurrencyID == ApplicationConstants.C_Multiple)
                    {
                        finalSummary.GrossExposureLocal = 0;
                        finalSummary.BetaAdjustedGrossExposureLocal = 0;
                    }
                    else
                    {
                        finalSummary.GrossExposureLocal = Math.Abs(finalSummary.NetExposureLocal);

                        if (finalSummary.AssetCategory.Equals(AssetCategory.FX) || finalSummary.AssetCategory.Equals(AssetCategory.FXForward))
                        {
                            finalSummary.BetaAdjustedGrossExposureLocal = finalSummary.GrossExposureLocal * Math.Abs(finalSummary.Beta);
                        }
                        else
                        {
                            if (finalSummary.LeverageFactor != 0)
                            {
                                finalSummary.BetaAdjustedGrossExposureLocal = finalSummary.GrossExposureLocal * Math.Abs(finalSummary.Beta) / finalSummary.LeverageFactor;
                            }
                        }
                    }
                }

                finalSummary.DayPnLShort += inputSummary.DayPnLShort;
                finalSummary.DayPnLLong += inputSummary.DayPnLLong;
                finalSummary.DayPnL += inputSummary.DayPnL;

                finalSummary.CashProjected += inputSummary.CashProjected;
                finalSummary.CostBasisPNL += inputSummary.CostBasisPNL;
                finalSummary.YesterdayMarketValue += inputSummary.YesterdayMarketValue;
                if (!isNAVSaved)
                {
                    finalSummary.YesterdayNAV += inputSummary.YesterdayNAV;
                }
                finalSummary.CashInflow += inputSummary.CashInflow;
                finalSummary.CashOutflow += inputSummary.CashOutflow;
                finalSummary.StartOfDayCash += inputSummary.StartOfDayCash;
                finalSummary.StartOfDayAccruals += inputSummary.StartOfDayAccruals;
                finalSummary.DayAccruals += inputSummary.DayAccruals;
                finalSummary.EarnedDividendBase += inputSummary.EarnedDividendBase;
                finalSummary.UnderlyingValueForOptions += inputSummary.UnderlyingValueForOptions;
                if (_creditLimitCalculationBasis == 0)
                {
                    finalSummary.LongDebitBalance += inputSummary.LongDebitBalance;
                    finalSummary.ShortCreditBalance += inputSummary.ShortCreditBalance;
                }
                finalSummary.LongDebitLimit += inputSummary.LongDebitLimit;
                finalSummary.ShortCreditLimit += inputSummary.ShortCreditLimit;
                finalSummary.PNLContributionPercentageSummary += inputSummary.PNLContributionPercentageSummary;
                finalSummary.MTDReturn += inputSummary.MTDReturn;
                finalSummary.QTDReturn += inputSummary.QTDReturn;
                finalSummary.YTDReturn += inputSummary.YTDReturn;
                finalSummary.MTDPnL += inputSummary.MTDPnL;
                finalSummary.QTDPnL += inputSummary.QTDPnL;
                finalSummary.YTDPnL += inputSummary.YTDPnL;
                finalSummary.Beta = inputSummary.Beta;
                finalSummary.DayPnLFX += inputSummary.DayPnLFX;

                if (String.IsNullOrEmpty(finalSummary.Symbol))
                {
                    finalSummary.Symbol = inputSummary.Symbol;
                }

                if (finalSummary.Symbol != ApplicationConstants.C_Multiple)
                {
                    if (finalSummary.Symbol != inputSummary.Symbol)
                    {
                        finalSummary.Symbol = ApplicationConstants.C_Multiple;
                        finalSummary.NetPosition = 0;
                        finalSummary.PositionBeforeZero = 0;
                    }
                    else
                    {
                        finalSummary.NetPosition += inputSummary.NetPosition;
                        finalSummary.TodayNetPosition += inputSummary.TodayNetPosition;
                        if (finalSummary.NetPosition != 0)
                        {
                            finalSummary.PositionBeforeZero = finalSummary.NetPosition;
                        }
                        else
                        {
                            finalSummary.PositionBeforeZero = inputSummary.PositionBeforeZero;
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

        private void ClearUnnecessaryConsolidatedSummaryField(ExposureAndPnlOrderSummary finalConsolSummary)
        {
            try
            {
                finalConsolSummary.PositionSideMV = PositionType.Long;
                finalConsolSummary.PositionSideExposure = PositionType.Long;
                finalConsolSummary.NetPosition = 0;
                finalConsolSummary.UnderlyingSymbol = string.Empty;
                finalConsolSummary.Symbol = String.Empty;
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

        private void MergeUnderlyingSummaryAndConsolidatedSummary(ExposureAndPnlOrderSummary finalConsolSummary, Dictionary<string, ExposureAndPnlOrderSummary> underlyingSymbolWiseGroupSummary)
        {
            try
            {
                foreach (KeyValuePair<string, ExposureAndPnlOrderSummary> kvp in underlyingSymbolWiseGroupSummary)
                {
                    FillLongShortBasedOnPositionSideInSummary(underlyingSymbolWiseGroupSummary[kvp.Key]);

                    finalConsolSummary.UnderlyingGrossExposure += kvp.Value.GrossExposure;
                    finalConsolSummary.UnderlyingLongExposure += kvp.Value.LongExposure;
                    finalConsolSummary.UnderlyingShortExposure += kvp.Value.ShortExposure;

                    finalConsolSummary.BetaAdjustedGrossExposureUnderlying += kvp.Value.BetaAdjustedGrossExposure;
                    finalConsolSummary.BetaAdjustedLongExposureUnderlying += kvp.Value.BetaAdjustedLongExposure;
                    finalConsolSummary.BetaAdjustedShortExposureUnderlying += kvp.Value.BetaAdjustedShortExposure;
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

        private ExposureAndPnlOrderSummary FillLongShortBasedOnPositionSideInSummary(ExposureAndPnlOrderSummary symbolWiseSummary)
        {
            try
            {
                if (symbolWiseSummary.PositionSideMV == PositionType.Long && symbolWiseSummary.AssetCategory != AssetCategory.FX && symbolWiseSummary.AssetCategory != AssetCategory.FXForward)
                {
                    symbolWiseSummary.LongMarketValue = symbolWiseSummary.NetMarketValue;
                    symbolWiseSummary.ShortMarketValue = 0;
                }
                else if (symbolWiseSummary.PositionSideMV == PositionType.Short && symbolWiseSummary.AssetCategory != AssetCategory.FX && symbolWiseSummary.AssetCategory != AssetCategory.FXForward)
                {
                    symbolWiseSummary.LongMarketValue = 0;
                    symbolWiseSummary.ShortMarketValue = symbolWiseSummary.NetMarketValue;
                }
                else
                {
                    symbolWiseSummary.LongMarketValue = 0;
                    symbolWiseSummary.ShortMarketValue = 0;
                }

                if (symbolWiseSummary.PositionSideExposure == PositionType.Long && symbolWiseSummary.AssetCategory != AssetCategory.FX && symbolWiseSummary.AssetCategory != AssetCategory.FXForward)
                {
                    symbolWiseSummary.LongExposure = symbolWiseSummary.NetExposure;
                    symbolWiseSummary.ShortExposure = 0;
                    symbolWiseSummary.BetaAdjustedShortExposure = 0;
                    if (symbolWiseSummary.AssetCategory.Equals(AssetCategory.FX) || symbolWiseSummary.AssetCategory.Equals(AssetCategory.FXForward))
                    {
                        symbolWiseSummary.BetaAdjustedLongExposure = symbolWiseSummary.NetExposure * symbolWiseSummary.Beta;
                    }
                    else
                    {
                        if (symbolWiseSummary.LeverageFactor != 0)
                        {
                            symbolWiseSummary.BetaAdjustedLongExposure = ((symbolWiseSummary.NetExposure / symbolWiseSummary.LeverageFactor) * symbolWiseSummary.Beta);
                        }
                    }
                    symbolWiseSummary.DayPnLLong = symbolWiseSummary.DayPnL;
                    symbolWiseSummary.DayPnLShort = 0;
                }
                else if (symbolWiseSummary.PositionSideExposure == PositionType.Short && symbolWiseSummary.AssetCategory != AssetCategory.FX && symbolWiseSummary.AssetCategory != AssetCategory.FXForward)
                {
                    symbolWiseSummary.LongExposure = 0;
                    symbolWiseSummary.ShortExposure = symbolWiseSummary.NetExposure;
                    symbolWiseSummary.BetaAdjustedLongExposure = 0;
                    if (symbolWiseSummary.AssetCategory.Equals(AssetCategory.FX) || symbolWiseSummary.AssetCategory.Equals(AssetCategory.FXForward))
                    {
                        symbolWiseSummary.BetaAdjustedShortExposure = symbolWiseSummary.NetExposure * symbolWiseSummary.Beta;
                    }
                    else
                    {
                        if (symbolWiseSummary.LeverageFactor != 0)
                        {
                            symbolWiseSummary.BetaAdjustedShortExposure = ((symbolWiseSummary.NetExposure / symbolWiseSummary.LeverageFactor) * symbolWiseSummary.Beta);
                        }
                    }
                    symbolWiseSummary.DayPnLLong = 0;
                    symbolWiseSummary.DayPnLShort = symbolWiseSummary.DayPnL;
                }
                else
                {
                    symbolWiseSummary.LongExposure = 0;
                    symbolWiseSummary.ShortExposure = 0;
                    symbolWiseSummary.BetaAdjustedLongExposure = 0;
                    symbolWiseSummary.BetaAdjustedShortExposure = 0;
                    symbolWiseSummary.DayPnLLong = 0;
                    symbolWiseSummary.DayPnLShort = 0;
                }

                if (symbolWiseSummary.AssetCategory == AssetCategory.FX || symbolWiseSummary.AssetCategory == AssetCategory.FXForward)
                {
                    symbolWiseSummary.DayPnLFX = symbolWiseSummary.DayPnL;
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
            return symbolWiseSummary;
        }

        public void GetConsolidationSummary(ref Dictionary<int, BusinessObjects.DistinctAccountSetWiseSummaryCollection> distinctAccountSetWiseSummaryCollection)
        {
            try
            {
                lock (_lockerObj)
                {
                    distinctAccountSetWiseSummaryCollection = new Dictionary<int, DistinctAccountSetWiseSummaryCollection>(_distinctAccountSetWiseSummaryCollection);
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

        public void GetFinalAccountWiseSummary(ref Dictionary<int, ExposureAndPnlOrderSummary> accountWiseSummary)
        {
            try
            {
                lock (_lockerObj)
                {
                    if (accountWiseSummary != null)
                    {
                        foreach (int accountId in SessionManager.AccountAndDistinctAccountPermissionSetsMapping.Keys)
                        {
                            if (_distinctAccountSetWiseSummaryCollection.ContainsKey(SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]))
                            {
                                if (accountWiseSummary.ContainsKey(accountId))
                                {
                                    accountWiseSummary[accountId] = _distinctAccountSetWiseSummaryCollection[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]].ConsolidationDashBoardSummary;
                                }
                                else
                                {
                                    accountWiseSummary.Add(accountId, _distinctAccountSetWiseSummaryCollection[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountId]].ConsolidationDashBoardSummary);
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

        public void FillSummaryValuesInOrder(ref EPnlOrder epnlOrder)
        {
            try
            {
                lock (_lockerObj)
                {
                    if (SessionManager.AccountAndDistinctAccountPermissionSetsMapping.ContainsKey(epnlOrder.Level1ID))
                    {
                        if (_distinctAccountSetWiseSummaryCollection.ContainsKey(SessionManager.AccountAndDistinctAccountPermissionSetsMapping[epnlOrder.Level1ID]))
                        {
                            ExposureAndPnlOrderSummary consolidationDashBoardSummary = _distinctAccountSetWiseSummaryCollection[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[epnlOrder.Level1ID]].ConsolidationDashBoardSummary;
                            epnlOrder.NavTouch = consolidationDashBoardSummary.NetAssetValue;

                            if (_distinctAccountSetWiseSummaryCollection[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[epnlOrder.Level1ID]].SymbolWiseGroupSummary.ContainsKey(epnlOrder.Symbol))
                                epnlOrder.DayTradedPosition = _distinctAccountSetWiseSummaryCollection[SessionManager.AccountAndDistinctAccountPermissionSetsMapping[epnlOrder.Level1ID]].SymbolWiseGroupSummary[epnlOrder.Symbol].TodayNetPosition;
                        }
                        else
                        {
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Please report Technical team. distinctAccountSetWiseSummaryCollection & AccountAndDistinctAccountPermissionSetsMapping out of sync.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                        }
                    }
                    else
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Order have invalid account id. Symbol = " + epnlOrder.Symbol + ", AccountID = " + epnlOrder.Level1ID + ", TaxlotID = " + epnlOrder.ID, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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

        public void CalculateDistinctPermissionSummaryUsingAccountSummary(Dictionary<int, List<int>> distinctAccountPermissionSets)
        {
            try
            {
                lock (_lockerObj)
                {
                    foreach (KeyValuePair<int, List<int>> kvp in distinctAccountPermissionSets)
                    {
                        if (kvp.Value != null && kvp.Value.Count > 1)
                        {
                            if (_distinctAccountSetWiseSummaryCollection.ContainsKey(kvp.Key))
                            {
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("DistinctAccountSetWiseSummaryCollection already has a key associated with the Accounts: " + string.Join(",", kvp.Value.ToArray()) + ". In Ideal case this should not happen.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                continue;
                            }

                            DistinctAccountSetWiseSummaryCollection distinctAccountSetWiseSummaryCollection = new DistinctAccountSetWiseSummaryCollection();
                            _distinctAccountSetWiseSummaryCollection.Add(kvp.Key, distinctAccountSetWiseSummaryCollection);

                            foreach (int accountID in kvp.Value)
                            {
                                int distinctAccountPermissionKey = SessionManager.AccountAndDistinctAccountPermissionSetsMapping[accountID];

                                if (!_distinctAccountSetWiseSummaryCollection.ContainsKey(distinctAccountPermissionKey))
                                    continue;

                                foreach (KeyValuePair<string, ExposureAndPnlOrderSummary> symbolWiseGroupSummaryAccount in _distinctAccountSetWiseSummaryCollection[distinctAccountPermissionKey].SymbolWiseGroupSummary)
                                {
                                    if (_distinctAccountSetWiseSummaryCollection[kvp.Key].SymbolWiseGroupSummary.ContainsKey(symbolWiseGroupSummaryAccount.Key))
                                    {
                                        CalculateSummaryFromSummary(symbolWiseGroupSummaryAccount.Value, _distinctAccountSetWiseSummaryCollection[kvp.Key].SymbolWiseGroupSummary[symbolWiseGroupSummaryAccount.Key], false, false);
                                    }
                                    else
                                    {
                                        ExposureAndPnlOrderSummary finalPermissionSetSymbolSummary = new ExposureAndPnlOrderSummary();
                                        CalculateSummaryFromSummary(symbolWiseGroupSummaryAccount.Value, finalPermissionSetSymbolSummary, false, false);
                                        _distinctAccountSetWiseSummaryCollection[kvp.Key].SymbolWiseGroupSummary.Add(symbolWiseGroupSummaryAccount.Key, finalPermissionSetSymbolSummary);
                                    }
                                }

                                foreach (KeyValuePair<string, ExposureAndPnlOrderSummary> underlyingSymbolWiseGroupSummaryAccount in _distinctAccountSetWiseSummaryCollection[distinctAccountPermissionKey].UnderlyingSymbolWiseGroupSummary)
                                {
                                    if (_distinctAccountSetWiseSummaryCollection[kvp.Key].UnderlyingSymbolWiseGroupSummary.ContainsKey(underlyingSymbolWiseGroupSummaryAccount.Key))
                                    {
                                        CalculateSummaryFromSummary(underlyingSymbolWiseGroupSummaryAccount.Value, _distinctAccountSetWiseSummaryCollection[kvp.Key].UnderlyingSymbolWiseGroupSummary[underlyingSymbolWiseGroupSummaryAccount.Key], false, false);
                                    }
                                    else
                                    {
                                        ExposureAndPnlOrderSummary finalPermissionSetUnderlyingSymbolSummary = new ExposureAndPnlOrderSummary();
                                        CalculateSummaryFromSummary(underlyingSymbolWiseGroupSummaryAccount.Value, finalPermissionSetUnderlyingSymbolSummary, false, false);
                                        _distinctAccountSetWiseSummaryCollection[kvp.Key].UnderlyingSymbolWiseGroupSummary.Add(underlyingSymbolWiseGroupSummaryAccount.Key, finalPermissionSetUnderlyingSymbolSummary);
                                    }
                                }

                                foreach (KeyValuePair<string, ExposureAndPnlOrderSummary> symbolWiseSummary in _distinctAccountSetWiseSummaryCollection[kvp.Key].SymbolWiseGroupSummary)
                                {
                                    AssignLongShort(_distinctAccountSetWiseSummaryCollection[kvp.Key].SymbolWiseGroupSummary[symbolWiseSummary.Key]);
                                }

                                foreach (KeyValuePair<string, ExposureAndPnlOrderSummary> underlyingSymbolWiseSummary in _distinctAccountSetWiseSummaryCollection[kvp.Key].UnderlyingSymbolWiseGroupSummary)
                                {
                                    AssignLongShort(_distinctAccountSetWiseSummaryCollection[kvp.Key].UnderlyingSymbolWiseGroupSummary[underlyingSymbolWiseSummary.Key]);
                                }
                            }
                        }
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
    }
}