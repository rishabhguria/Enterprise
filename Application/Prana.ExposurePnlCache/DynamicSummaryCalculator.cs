using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ExposurePnlCache
{
    internal static class DynamicSummaryCalculator
    {
        internal static void FillOrderWithSummaryValues(DistinctAccountSetWiseSummaryCollection distinctPermissionSetWiseSummaryCollection, ExposurePnlCacheItem exOrder, List<string> PMCurrentViewGroupedColumns, ref bool isGroupingColumnValueChanged)
        {
            try
            {
                if (distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary.ContainsKey(exOrder.UnderlyingSymbol) && distinctPermissionSetWiseSummaryCollection.SymbolWiseGroupSummary.ContainsKey(exOrder.Symbol))
                {
                    string str = AssetCategory.FX.ToString();
                    if (exOrder.Asset.Equals(AssetCategory.FX.ToString()) || exOrder.Asset.Equals(AssetCategory.FXForward.ToString()))
                        exOrder.UnderlyingGrossExposureInBaseCurrency = exOrder.GrossExposure;
                    else
                        exOrder.UnderlyingGrossExposureInBaseCurrency = distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[exOrder.UnderlyingSymbol].GrossExposure;
                    exOrder.UnderlyingGrossExposure = distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[exOrder.UnderlyingSymbol].GrossExposureLocal;
                    exOrder.StartOfDayNAV = distinctPermissionSetWiseSummaryCollection.ConsolidationDashBoardSummary.YesterdayNAV;
                    exOrder.BetaAdjGrossExposureUnderlying = distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[exOrder.UnderlyingSymbol].BetaAdjustedGrossExposureLocal;
                    exOrder.BetaAdjGrossExposureUnderlyingInBaseCurrency = distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[exOrder.UnderlyingSymbol].BetaAdjustedGrossExposure;

                    exOrder.NAV = distinctPermissionSetWiseSummaryCollection.ConsolidationDashBoardSummary.NetAssetValue;
                    if (exOrder.StartOfDayNAV > 0)
                    {
                        exOrder.DayReturn = (exOrder.DayPnLInBaseCurrency * 1.0 / exOrder.StartOfDayNAV) * 100;
                    }
                    else
                    {
                        exOrder.DayReturn = 0.0;
                    }
                    if (exOrder.NAV > 0)
                    {
                        exOrder.PercentNetExposureInBaseCurrency = (exOrder.NetExposureInBaseCurrency * 1.0 / exOrder.NAV) * 100;
                        exOrder.PercentGrossExposureInBaseCurrency = (Math.Abs(exOrder.NetExposureInBaseCurrency * 1.0) / exOrder.NAV) * 100;
                        exOrder.PercentExposureInBaseCurrency = (exOrder.ExposureInBaseCurrency * 1.0 / exOrder.NAV) * 100;
                        exOrder.PercentUnderlyingGrossExposureInBaseCurrency = (exOrder.UnderlyingGrossExposureInBaseCurrency / exOrder.NAV) * 100;
                        exOrder.PercentBetaAdjGrossExposureInBaseCurrency = (exOrder.BetaAdjGrossExposureUnderlyingInBaseCurrency / exOrder.NAV) * 100;
                        exOrder.PercentGrossMarketValueInBaseCurrency = (Math.Abs(exOrder.MarketValueInBaseCurrency * 1.0) / exOrder.NAV) * 100;
                        exOrder.PercentagePNLContribution = exOrder.DayPnLInBaseCurrency / exOrder.NAV * ApplicationConstants.PERCENTAGEVALUE;
                        exOrder.PercentNetMarketValueInBaseCurrency = (exOrder.MarketValueInBaseCurrency * 1.0 / exOrder.NAV) * 100;
                    }
                    else
                    {
                        exOrder.PercentNetExposureInBaseCurrency = 0.0;
                        exOrder.PercentGrossExposureInBaseCurrency = 0.0;
                        exOrder.PercentExposureInBaseCurrency = 0.0;
                        exOrder.PercentUnderlyingGrossExposureInBaseCurrency = 0.0;
                        exOrder.PercentBetaAdjGrossExposureInBaseCurrency = 0.0;
                        exOrder.PercentGrossMarketValueInBaseCurrency = 0.0;
                        exOrder.PercentagePNLContribution = 0.0;
                        exOrder.PercentNetMarketValueInBaseCurrency = 0.0;
                    }

                    if (exOrder.NAV == 0)
                    {
                        exOrder.ExposureBPInBaseCurrency = 0.0;
                    }
                    else
                    {
                        exOrder.ExposureBPInBaseCurrency = (((exOrder.NetExposureInBaseCurrency * 1.0) / exOrder.NAV) * ApplicationConstants.BASISPOINTVALUE);
                    }

                    if (exOrder.Asset == AssetCategory.FX.ToString() || exOrder.Asset == AssetCategory.FXForward.ToString())
                    {
                        exOrder.PositionSideExposureUnderlying = PositionType.FX.ToString();
                    }
                    else
                    {
                        //HasBeenSentToUser = 1 means data is coming second time from Expnl
                        if (!isGroupingColumnValueChanged && exOrder.HasBeenSentToUser == 1 && PMCurrentViewGroupedColumns.Contains("PositionSideExposureUnderlying") && exOrder.PositionSideExposureUnderlying != distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[exOrder.UnderlyingSymbol].PositionSideExposure.ToString())
                        {
                            isGroupingColumnValueChanged = true;
                        }
                        exOrder.PositionSideExposureUnderlying = distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[exOrder.UnderlyingSymbol].PositionSideExposure.ToString();
                    }
                }

                if (distinctPermissionSetWiseSummaryCollection.SymbolWiseGroupSummary.ContainsKey(exOrder.Symbol))
                {
                    //HasBeenSentToUser = 1 means data is coming second time from Expnl
                    if (!isGroupingColumnValueChanged && exOrder.HasBeenSentToUser == 1 && PMCurrentViewGroupedColumns.Contains("PositionSideMV") && exOrder.PositionSideMV != distinctPermissionSetWiseSummaryCollection.SymbolWiseGroupSummary[exOrder.Symbol].PositionSideMV.ToString())
                    {
                        isGroupingColumnValueChanged = true;
                    }
                    exOrder.PositionSideMV = distinctPermissionSetWiseSummaryCollection.SymbolWiseGroupSummary[exOrder.Symbol].PositionSideMV.ToString();

                    //HasBeenSentToUser = 1 means data is coming second time from Expnl
                    if (!isGroupingColumnValueChanged && exOrder.HasBeenSentToUser == 1 && PMCurrentViewGroupedColumns.Contains("PositionSideExposure") && exOrder.PositionSideExposure != distinctPermissionSetWiseSummaryCollection.SymbolWiseGroupSummary[exOrder.Symbol].PositionSideExposure.ToString())
                    {
                        isGroupingColumnValueChanged = true;
                    }
                    exOrder.PositionSideExposure = distinctPermissionSetWiseSummaryCollection.SymbolWiseGroupSummary[exOrder.Symbol].PositionSideExposure.ToString();
                }

                if (!CommonDataCache.CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                {
                    exOrder.UnderlyingGrossExposureInBaseCurrency = 0.0;
                    exOrder.UnderlyingGrossExposure = 0.0;
                    exOrder.StartOfDayNAV = 0.0;
                    exOrder.BetaAdjGrossExposureUnderlying = 0.0;
                    exOrder.BetaAdjGrossExposureUnderlyingInBaseCurrency = 0.0;
                    exOrder.NAV = 0.0;
                    exOrder.DayReturn = 0.0;
                    exOrder.ExposureBPInBaseCurrency = 0.0;

                    if (exOrder.Asset == AssetCategory.FX.ToString() || exOrder.Asset == AssetCategory.FXForward.ToString())
                    {
                        exOrder.PositionSideExposureUnderlying = PositionType.FX.ToString();
                        exOrder.PositionSideMV = PositionType.FX.ToString();
                        exOrder.PositionSideExposure = PositionType.FX.ToString();
                    }
                    else
                    {
                        exOrder.PositionSideExposureUnderlying = PositionType.Long.ToString();
                        exOrder.PositionSideMV = PositionType.Long.ToString();
                        exOrder.PositionSideExposure = PositionType.Long.ToString();
                    }

                    exOrder.PercentNetExposureInBaseCurrency = 0.0;
                    exOrder.PercentGrossExposureInBaseCurrency = 0.0;
                    exOrder.PercentExposureInBaseCurrency = 0.0;
                    exOrder.PercentUnderlyingGrossExposureInBaseCurrency = 0.0;
                    exOrder.PercentBetaAdjGrossExposureInBaseCurrency = 0.0;
                    exOrder.PercentGrossMarketValueInBaseCurrency = 0.0;
                    exOrder.PercentagePNLContribution = 0.0;
                    exOrder.PercentNetMarketValueInBaseCurrency = 0.0;
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
    }
}