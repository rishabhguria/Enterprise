using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ExpnlService.Grouping_Components.Views_and_SummaryCalculators.Compressors
{
    public abstract class GenericCompressor
    {
        protected Dictionary<string, ExposurePnlCacheItem> _outputDictionary = new Dictionary<string, ExposurePnlCacheItem>();
        private Dictionary<int, ExposureAndPnlOrderCollection> _inputOrderCollection;
        private Dictionary<string, List<string>> _dictOfTaxlots = new Dictionary<string, List<string>>();

        public Dictionary<int, ExposureAndPnlOrderCollection> InputOrderCollection
        {
            get
            {
                return _inputOrderCollection;
            }
        }

        public Dictionary<string, List<string>> DictOfTaxlots
        {
            get
            {
                return _dictOfTaxlots;
            }
        }

        public CompressedDataDictionaries GetCompressedData(Dictionary<int, ExposureAndPnlOrderCollection> taxLots)
        {
            _inputOrderCollection = taxLots;
            _dictOfTaxlots.Clear();
            _outputDictionary.Clear();

            CompressedDataDictionaries dictToReturn = new CompressedDataDictionaries();
            try
            {
                foreach (KeyValuePair<int, ExposureAndPnlOrderCollection> temp in taxLots)
                {
                    ExposurePnlCacheItemList list = new ExposurePnlCacheItemList();
                    dictToReturn.OutputCompressedData.Add(temp.Key, list);
                    int count = 0;
                    // This bool variable is to check if this is the last record for the respective key. If this is the last record then
                    // all of the related taxlots are already summed up. Just need to call abs on the value.
                    bool isReadyToCalculateAbsoluteValue = false;
                    foreach (EPnlOrder taxlot in temp.Value)
                    {
                        count++;
                        if (count.Equals(temp.Value.Count))
                        {
                            isReadyToCalculateAbsoluteValue = true;
                        }
                        string key = getKey(taxlot);

                        ExposurePnlCacheItem uiObject = null;
                        if (_outputDictionary.ContainsKey(key))
                        {
                            uiObject = _outputDictionary[key];
                            //Static data has to be send for a key, if here is any change in any taxlot
                            if (taxlot.HasBeenSentToUser == 0)
                                uiObject.HasBeenSentToUser = 0;

                            if (uiObject.StartTradeDate.CompareTo(taxlot.TransactionDate) > 0)
                            {
                                uiObject.StartTradeDate = taxlot.TransactionDate;
                            }
                            _dictOfTaxlots[key].Add(taxlot.ID);
                            CalculateSpecificDetails(uiObject, taxlot);
                            if (isReadyToCalculateAbsoluteValue)
                            {
                                CalculateAbsoluteValues(uiObject);
                            }
                        }
                        else
                        {
                            uiObject = new ExposurePnlCacheItem();
                            taxlot.IsServerUpdated = true;

                            switch (taxlot.ClassID)
                            {
                                case EPnLClassID.EPnlOrder:
                                    taxlot.GetBindableObject(uiObject);
                                    break;

                                case EPnLClassID.EPnLOrderEquity:
                                    ((EPnLOrderEquity)taxlot).GetBindableObject(uiObject);
                                    break;

                                case EPnLClassID.EPnLOrderOption:
                                    ((EPnLOrderOption)taxlot).GetBindableObject(uiObject);

                                    break;

                                case EPnLClassID.EPnLOrderFuture:
                                    ((EPnLOrderFuture)taxlot).GetBindableObject(uiObject);

                                    break;

                                case EPnLClassID.EPnLOrderFX:
                                    ((EPnLOrderFX)taxlot).GetBindableObject(uiObject);

                                    break;

                                case EPnLClassID.EPnLOrderEquitySwap:
                                    ((EPnLOrderEquitySwap)taxlot).GetBindableObject(uiObject);
                                    break;

                                case EPnLClassID.EPnLOrderFXForward:
                                    ((EPnLOrderFXForward)taxlot).GetBindableObject(uiObject);
                                    break;

                                case EPnLClassID.EPnLOrderFixedIncome:
                                    ((EPnLOrderFixedIncome)taxlot).GetBindableObject(uiObject);

                                    break;

                                default:
                                    break;
                            }

                            uiObject.ID = key;
                            _dictOfTaxlots.Add(key, new List<string>() { taxlot.ID });
                            _outputDictionary.Add(key, uiObject);
                            if (isReadyToCalculateAbsoluteValue)
                            {
                                CalculateAbsoluteValues(uiObject);
                            }
                            dictToReturn.OutputCompressedData[temp.Key].Add(uiObject);
                            //list.Add(uiObject);
                        }
                    }
                    //Kashish G., PRANA-32392
                    //TODO: Below code is to handle short term solution.
                    foreach (ExposurePnlCacheItem item in dictToReturn.OutputCompressedData[temp.Key])
                    {
                        CalculateAbsoluteValues(item);
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

            return dictToReturn;
        }

        abstract protected string getKey(EPnlOrder taxlot);

        protected void CalculateAbsoluteValues(ExposurePnlCacheItem uiObject)
        {
            if (uiObject.AvgPrice != null)
            {
                double? baseValue = uiObject.Quantity * uiObject.AvgPrice * uiObject.SideMultiplier * uiObject.Multiplier;
                if (baseValue != null && baseValue != 0)
                {
                    uiObject.DayPnLBP = Math.Round((uiObject.DayPnL / Math.Abs((double)baseValue)) * ApplicationConstants.BASISPOINTVALUE);
                }
            }
            if (uiObject.MarketValueInBaseCurrency != 0 && uiObject.Quantity != 0)
            {
                uiObject.PercentDayPnLGrossMV = uiObject.DayPnLInBaseCurrency / Math.Abs(uiObject.MarketValueInBaseCurrency) * 100;
                uiObject.PercentDayPnLNetMV = uiObject.DayPnLInBaseCurrency / uiObject.MarketValueInBaseCurrency * 100;
            }
            else if (uiObject.Quantity == 0)
            {
                uiObject.PercentDayPnLGrossMV = 0.0;
                uiObject.PercentDayPnLNetMV = 0.0;
            }
        }

        protected abstract void CalculateSpecificDetails(ExposurePnlCacheItem uiObject, EPnlOrder taxlot);

        public void FillOrderWithSummaryValues(DistinctAccountSetWiseSummaryCollection distinctPermissionSetWiseSummaryCollection, ExposurePnlCacheItem uiObject)
        {
            try
            {
                if (distinctPermissionSetWiseSummaryCollection != null && (distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary.ContainsKey(uiObject.UnderlyingSymbol) && distinctPermissionSetWiseSummaryCollection.SymbolWiseGroupSummary.ContainsKey(uiObject.Symbol)))
                {
                    uiObject.UnderlyingGrossExposureInBaseCurrency = distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[uiObject.UnderlyingSymbol].GrossExposure;
                    uiObject.UnderlyingGrossExposure = distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[uiObject.UnderlyingSymbol].GrossExposureLocal;
                    uiObject.StartOfDayNAV = distinctPermissionSetWiseSummaryCollection.ConsolidationDashBoardSummary.YesterdayNAV;
                    uiObject.BetaAdjGrossExposureUnderlying = distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[uiObject.UnderlyingSymbol].BetaAdjustedGrossExposureLocal;
                    uiObject.BetaAdjGrossExposureUnderlyingInBaseCurrency = distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[uiObject.UnderlyingSymbol].BetaAdjustedGrossExposure;

                    uiObject.NAV = distinctPermissionSetWiseSummaryCollection.ConsolidationDashBoardSummary.NetAssetValue;
                    if (uiObject.StartOfDayNAV > 0)
                    {
                        uiObject.DayReturn = (uiObject.DayPnLInBaseCurrency * 1.0 / uiObject.StartOfDayNAV) * 100;
                    }
                    else
                    {
                        uiObject.DayReturn = 0.0;
                    }
                    if (uiObject.NAV > 0)
                    {
                        uiObject.PercentNetExposureInBaseCurrency = (uiObject.NetExposureInBaseCurrency * 1.0 / uiObject.NAV) * 100;
                        uiObject.PercentGrossExposureInBaseCurrency = (Math.Abs(uiObject.NetExposureInBaseCurrency * 1.0) / uiObject.NAV) * 100;
                        uiObject.PercentExposureInBaseCurrency = (uiObject.ExposureInBaseCurrency * 1.0 / uiObject.NAV) * 100;
                        uiObject.PercentUnderlyingGrossExposureInBaseCurrency = (uiObject.UnderlyingGrossExposureInBaseCurrency / uiObject.NAV) * 100;
                        uiObject.PercentBetaAdjGrossExposureInBaseCurrency = (uiObject.BetaAdjGrossExposureUnderlyingInBaseCurrency / uiObject.NAV) * 100;
                        uiObject.PercentGrossMarketValueInBaseCurrency = (Math.Abs(uiObject.MarketValueInBaseCurrency * 1.0) / uiObject.NAV) * 100;
                        uiObject.PercentagePNLContribution = uiObject.DayPnLInBaseCurrency / uiObject.NAV * ApplicationConstants.PERCENTAGEVALUE;
                        uiObject.PercentNetMarketValueInBaseCurrency = (uiObject.MarketValueInBaseCurrency * 1.0 / uiObject.NAV) * 100;
                    }
                    else
                    {
                        uiObject.PercentNetExposureInBaseCurrency = 0.0;
                        uiObject.PercentGrossExposureInBaseCurrency = 0.0;
                        uiObject.PercentExposureInBaseCurrency = 0.0;
                        uiObject.PercentUnderlyingGrossExposureInBaseCurrency = 0.0;
                        uiObject.PercentBetaAdjGrossExposureInBaseCurrency = 0.0;
                        uiObject.PercentGrossMarketValueInBaseCurrency = 0.0;
                        uiObject.PercentagePNLContribution = 0.0;
                        uiObject.PercentNetMarketValueInBaseCurrency = 0.0;
                    }

                    if (uiObject.Asset == AssetCategory.FX.ToString() || uiObject.Asset == AssetCategory.FXForward.ToString())
                    {
                        uiObject.PositionSideExposureUnderlying = AssetCategory.FX.ToString();
                    }
                    else
                    {
                        uiObject.PositionSideExposureUnderlying = distinctPermissionSetWiseSummaryCollection.UnderlyingSymbolWiseGroupSummary[uiObject.UnderlyingSymbol].PositionSideExposure.ToString();
                    }

                    if (uiObject.NAV == 0)
                    {
                        uiObject.ExposureBPInBaseCurrency = 0;
                    }
                    else
                    {
                        uiObject.ExposureBPInBaseCurrency = (((uiObject.NetExposureInBaseCurrency * 1.0) / uiObject.NAV) * ApplicationConstants.BASISPOINTVALUE);
                    }
                }

                if (distinctPermissionSetWiseSummaryCollection.SymbolWiseGroupSummary.ContainsKey(uiObject.Symbol))
                {
                    if (uiObject.Asset == AssetCategory.FX.ToString() || uiObject.Asset == AssetCategory.FXForward.ToString())
                    {
                        uiObject.PositionSideMV = PositionType.FX.ToString();
                        uiObject.PositionSideExposure = PositionType.FX.ToString();
                    }
                    else
                    {
                        uiObject.PositionSideMV = distinctPermissionSetWiseSummaryCollection.SymbolWiseGroupSummary[uiObject.Symbol].PositionSideMV.ToString();
                        uiObject.PositionSideExposure = distinctPermissionSetWiseSummaryCollection.SymbolWiseGroupSummary[uiObject.Symbol].PositionSideExposure.ToString();
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
    }
}

