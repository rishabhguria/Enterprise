using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prana.ExpnlService
{
    public class SplitManager
    {
        private static SplitManager _instance;
        public static SplitManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SplitManager();
            }
            return _instance;
        }

        #region Box Position Splitting
        int _splittedTaxlotsCacheBasis = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("SplittedTaxlotsCacheBasis").ToString());
        ConcurrentDictionary<int, ConcurrentDictionary<string, SplitSymbolDataCollection>> _accountSymbolSideWiseOrderCollection;
        public ExposureAndPnlOrderCollection GetSplittedUncalculatedData(ExposureAndPnlOrderCollection tempCollectionFromDB, ApplicationConstants.TaxLotState taxlotState, bool isPublishRequest)
        {
            ExposureAndPnlOrderCollection listOfBrokenOrders = new ExposureAndPnlOrderCollection();
            try
            {
                int accountID = 0;

                if (!isPublishRequest)
                {
                    _accountSymbolSideWiseOrderCollection = new ConcurrentDictionary<int, ConcurrentDictionary<string, SplitSymbolDataCollection>>();
                }

                foreach (EPnlOrder epnlOrder in tempCollectionFromDB)
                {
                    if (_splittedTaxlotsCacheBasis == 1)
                        accountID = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(epnlOrder.Level1ID);
                    else
                        accountID = epnlOrder.Level1ID > 0 ? epnlOrder.Level1ID : -1;

                    if (taxlotState == ApplicationConstants.TaxLotState.Deleted)
                    {
                        RemoveFromCache(epnlOrder, accountID);
                    }
                    else
                    {
                        AddOrUpdateOrderToCache(epnlOrder, accountID);
                    }
                }

                if (isPublishRequest)
                {
                    foreach (EPnlOrder epnlOrder in tempCollectionFromDB)
                    {
                        if (_accountSymbolSideWiseOrderCollection.ContainsKey(accountID) && _accountSymbolSideWiseOrderCollection[accountID].ContainsKey(epnlOrder.Symbol))
                            CreateBrokenOrdersForSymbol(_accountSymbolSideWiseOrderCollection[accountID][epnlOrder.Symbol], ref listOfBrokenOrders);
                    }
                }
                else
                {
                    CreateBrokenOrders(ref listOfBrokenOrders);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return listOfBrokenOrders;
        }

        private void AddOrUpdateOrderToCache(EPnlOrder epnlOrder, int accountID)
        {
            try
            {
                if (!String.IsNullOrEmpty(epnlOrder.OrderSideTagValue))
                {
                    ConcurrentDictionary<string, SplitSymbolDataCollection> symbolSideWiseOrderCollection = new ConcurrentDictionary<string, SplitSymbolDataCollection>();
                    SplitSymbolDataCollection splitSymbolDataCollection = new SplitSymbolDataCollection();
                    ConcurrentDictionary<string, ExposureAndPnlOrderCollection> sideWiseOrderCollection = new ConcurrentDictionary<string, ExposureAndPnlOrderCollection>();
                    ExposureAndPnlOrderCollection exposureAndPnlOrderCollection = new ExposureAndPnlOrderCollection();

                    symbolSideWiseOrderCollection = _accountSymbolSideWiseOrderCollection.GetOrAdd(accountID, symbolSideWiseOrderCollection);
                    splitSymbolDataCollection = symbolSideWiseOrderCollection.GetOrAdd(epnlOrder.Symbol, splitSymbolDataCollection);
                    sideWiseOrderCollection = splitSymbolDataCollection.SideWiseOrderCollection;

                    splitSymbolDataCollection.Symbol = epnlOrder.Symbol;
                    splitSymbolDataCollection.AssetCategory = epnlOrder.Asset;
                    if (epnlOrder.Asset == AssetCategory.EquityOption || epnlOrder.Asset == AssetCategory.FutureOption || epnlOrder.Asset == AssetCategory.FXOption)
                    {
                        splitSymbolDataCollection.ContractType = ((EPnLOrderOption)epnlOrder).ContractType.ToUpper();
                    }

                    foreach (KeyValuePair<string, ExposureAndPnlOrderCollection> kvp in sideWiseOrderCollection)
                    {
                        if (kvp.Value.Contains(epnlOrder.ID))
                        {
                            kvp.Value.Remove(epnlOrder.ID);
                            kvp.Value.Sort();
                            break;
                        }
                    }

                    exposureAndPnlOrderCollection = sideWiseOrderCollection.GetOrAdd(epnlOrder.OrderSideTagValue, exposureAndPnlOrderCollection);
                    exposureAndPnlOrderCollection.Add(epnlOrder);

                    exposureAndPnlOrderCollection.Sort();
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("OrderSide blank for following order");
                    sb.Append(Environment.NewLine);
                    sb.Append("------------------------------------");
                    sb.Append(Environment.NewLine);
                    sb.Append(epnlOrder.Symbol);
                    sb.Append(Environment.NewLine);
                    sb.Append(epnlOrder.ID);
                    sb.Append(Environment.NewLine);
                    sb.Append(epnlOrder.Quantity);
                    sb.Append(Environment.NewLine);
                    sb.Append("------------------------------------");
                    sb.Append(Environment.NewLine);
                    Logger.LoggerWrite(sb, LoggingConstants.CATEGORY_GENERAL);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void RemoveFromCache(EPnlOrder epnlOrder, int accountID)
        {
            try
            {
                if (_accountSymbolSideWiseOrderCollection.ContainsKey(accountID) && _accountSymbolSideWiseOrderCollection[accountID].ContainsKey(epnlOrder.Symbol)
                    && _accountSymbolSideWiseOrderCollection[accountID][epnlOrder.Symbol].SideWiseOrderCollection.ContainsKey(epnlOrder.OrderSideTagValue) && _accountSymbolSideWiseOrderCollection[accountID][epnlOrder.Symbol].SideWiseOrderCollection[epnlOrder.OrderSideTagValue].Contains(epnlOrder.ID))
                {
                    _accountSymbolSideWiseOrderCollection[accountID][epnlOrder.Symbol].SideWiseOrderCollection[epnlOrder.OrderSideTagValue].Remove(epnlOrder.ID);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CreateBrokenOrders(ref ExposureAndPnlOrderCollection listOfBrokenOrders)
        {
            try
            {
                if (_accountSymbolSideWiseOrderCollection != null)
                {
                    foreach (ConcurrentDictionary<string, SplitSymbolDataCollection> symbolSideWiseOrderCollection in _accountSymbolSideWiseOrderCollection.Values)
                    {
                        foreach (KeyValuePair<string, SplitSymbolDataCollection> kvp in symbolSideWiseOrderCollection)
                        {
                            CreateBrokenOrdersForSymbol(kvp.Value, ref listOfBrokenOrders);
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

        private void CreateBrokenOrdersForSymbol(SplitSymbolDataCollection splitSymbolDataCollection, ref ExposureAndPnlOrderCollection listOfBrokenOrders)
        {
            try
            {
                switch (splitSymbolDataCollection.AssetCategory)
                {
                    case AssetCategory.FutureOption:
                    case AssetCategory.EquityOption:
                        switch (splitSymbolDataCollection.ContractType)
                        {
                            case "CALL":
                                ProcessListForBreakingOrders(splitSymbolDataCollection, FIXConstants.SIDE_Buy_Open, FIXConstants.SIDE_Sell_Closed, FIXConstants.SIDE_Sell_Open, FIXConstants.SIDE_Buy_Closed, ref listOfBrokenOrders);
                                break;

                            case "PUT":
                                ProcessListForBreakingOrders(splitSymbolDataCollection, FIXConstants.SIDE_Sell_Open, FIXConstants.SIDE_Buy_Closed, FIXConstants.SIDE_Buy_Open, FIXConstants.SIDE_Sell_Closed, ref listOfBrokenOrders);
                                break;
                        }
                        break;
                    case AssetCategory.FX:
                    case AssetCategory.FXForward:
                    case AssetCategory.FXOption:
                        foreach (KeyValuePair<string, ExposureAndPnlOrderCollection> kvp in splitSymbolDataCollection.SideWiseOrderCollection)
                        {
                            listOfBrokenOrders.AddRangeThreadSafely(kvp.Value.Select(c => { c.PositionSideExposureBoxed = PositionType.FX; return c; }).ToList());
                        }
                        break;
                    default:
                        ProcessListForBreakingOrders(splitSymbolDataCollection, FIXConstants.SIDE_Buy, FIXConstants.SIDE_Sell, FIXConstants.SIDE_SellShort, FIXConstants.SIDE_Buy_Closed, ref listOfBrokenOrders);
                        break;
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

        private void ProcessListForBreakingOrders(SplitSymbolDataCollection splitSymbolDataCollection, string longSideOpen, string longSideClose, string shortSideOpen, string shortSideClose, ref ExposureAndPnlOrderCollection listOfBrokenOrders)
        {
            try
            {
                double quantityToBeBoxed = 0.0;
                if (IsSplittingRequired(splitSymbolDataCollection.SideWiseOrderCollection, out quantityToBeBoxed, longSideOpen, longSideClose, shortSideOpen, shortSideClose))
                {
                    //Only checking for sell and buytoclose side in dictionary
                    //because this won't be executed if both buy and shortSellSide are absent
                    if (splitSymbolDataCollection.SideWiseOrderCollection.ContainsKey(longSideClose))
                    {
                        CloseBuyAndSellOrders(splitSymbolDataCollection.SideWiseOrderCollection[longSideOpen], splitSymbolDataCollection.SideWiseOrderCollection[longSideClose], quantityToBeBoxed, ref listOfBrokenOrders);
                    }
                    else
                    {
                        CloseBuyAndSellOrders(splitSymbolDataCollection.SideWiseOrderCollection[longSideOpen], null, quantityToBeBoxed, ref listOfBrokenOrders);
                    }

                    if (splitSymbolDataCollection.SideWiseOrderCollection.ContainsKey(shortSideClose))
                    {
                        CloseBuyAndSellOrders(splitSymbolDataCollection.SideWiseOrderCollection[shortSideOpen], splitSymbolDataCollection.SideWiseOrderCollection[shortSideClose], quantityToBeBoxed, ref listOfBrokenOrders);
                    }
                    else
                    {
                        CloseBuyAndSellOrders(splitSymbolDataCollection.SideWiseOrderCollection[shortSideOpen], null, quantityToBeBoxed, ref listOfBrokenOrders);
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, ExposureAndPnlOrderCollection> kvp in splitSymbolDataCollection.SideWiseOrderCollection)
                    {
                        listOfBrokenOrders.AddRangeThreadSafely(kvp.Value);
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

        private bool IsSplittingRequired(ConcurrentDictionary<string, ExposureAndPnlOrderCollection> concurrentDictionary, out double quantityToBeBoxed, string longSideOpen, string longSideClose, string shortSideOpen, string shortSideClose)
        {
            try
            {
                if (!(concurrentDictionary.ContainsKey(longSideOpen) && concurrentDictionary.ContainsKey(shortSideOpen)))
                {
                    quantityToBeBoxed = 0.0;
                    return false;
                }

                double longPositionUnresolved = PositionUnresolved(concurrentDictionary, longSideOpen, longSideClose);
                double shortPositionUnresolved = PositionUnresolved(concurrentDictionary, shortSideOpen, shortSideClose);
                if (longPositionUnresolved == 0.0 || shortPositionUnresolved == 0.0)
                {
                    quantityToBeBoxed = 0.0;
                    return false;
                }
                quantityToBeBoxed = Math.Min(Math.Abs(longPositionUnresolved), Math.Abs(shortPositionUnresolved));
                return true;
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
            quantityToBeBoxed = 0.0;
            return false;
        }

        private double PositionUnresolved(ConcurrentDictionary<string, ExposureAndPnlOrderCollection> concurrentDictionary, string openSide, string closeSide)
        {
            double openQuantity = 0.0;
            double closeQuantity = 0.0;
            try
            {
                if (concurrentDictionary.ContainsKey(openSide))
                {
                    foreach (EPnlOrder item in concurrentDictionary[openSide])
                    {
                        openQuantity += Math.Abs(item.Quantity);
                    }
                }
                if (concurrentDictionary.ContainsKey(closeSide))
                {
                    foreach (EPnlOrder item in concurrentDictionary[closeSide])
                    {
                        closeQuantity += Math.Abs(item.Quantity);
                    }
                }
                //Checking because closing side cannot be greater than opening side,
                //we are returning 0 so that splitting is not performed
                if (closeQuantity > openQuantity)
                {
                    return 0;
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

            return Math.Abs(openQuantity - closeQuantity);
        }

        private void CloseBuyAndSellOrders(ExposureAndPnlOrderCollection buyList, ExposureAndPnlOrderCollection sellList, double quantityToBeBoxed, ref ExposureAndPnlOrderCollection listOfBrokenOrders)
        {
            try
            {
                bool keepOnProgressing = true;
                EPnlOrder currentBuyOrder = null;
                EPnlOrder currentSellOrder = null;
                if (sellList != null && sellList.Count > 0)
                {
                    double boughtQuantity = 0.0;
                    double soldQuantity = 0.0;
                    var buyOrder = buyList.GetEnumerator();
                    var sellOrder = sellList.GetEnumerator();
                    FetchNextBuy(ref keepOnProgressing, ref currentBuyOrder, ref buyOrder);
                    FetchNextSell(ref keepOnProgressing, ref currentSellOrder, ref sellOrder);
                    boughtQuantity = currentBuyOrder.Quantity;
                    soldQuantity = currentSellOrder.Quantity;
                    while (keepOnProgressing)
                    {
                        if (boughtQuantity > soldQuantity)
                        {
                            boughtQuantity = boughtQuantity - soldQuantity;
                            listOfBrokenOrders.AddThreadSafely(currentSellOrder);
                            FetchNextSell(ref keepOnProgressing, ref currentSellOrder, ref sellOrder);
                            if (currentSellOrder != null)
                            {
                                soldQuantity = currentSellOrder.Quantity;
                            }
                            else
                            {
                                soldQuantity = 0.0;
                                break;
                            }
                        }
                        else if (boughtQuantity < soldQuantity)
                        {
                            soldQuantity = soldQuantity - boughtQuantity;
                            listOfBrokenOrders.AddThreadSafely(currentBuyOrder);
                            FetchNextBuy(ref keepOnProgressing, ref currentBuyOrder, ref buyOrder);
                            if (currentBuyOrder != null)
                            {
                                boughtQuantity = currentBuyOrder.Quantity;
                            }
                            else
                            {
                                boughtQuantity = 0.0;
                                break;
                            }
                        }
                        else
                        {
                            soldQuantity = 0.0;
                            boughtQuantity = 0.0;
                            listOfBrokenOrders.AddThreadSafely(currentSellOrder);
                            listOfBrokenOrders.AddThreadSafely(currentBuyOrder);
                            FetchNextBuy(ref keepOnProgressing, ref currentBuyOrder, ref buyOrder);
                            FetchNextSell(ref keepOnProgressing, ref currentSellOrder, ref sellOrder);

                            if (currentBuyOrder != null)
                                boughtQuantity = currentBuyOrder.Quantity;
                            else
                                break;
                            if (currentSellOrder != null)
                                soldQuantity = currentSellOrder.Quantity;
                            else
                                break;
                        }
                    }

                    while (quantityToBeBoxed != 0)
                    {
                        if (currentBuyOrder != null)
                        {
                            quantityToBeBoxed = SplitAndGetLeftQuantity(CloneHelper.CreateEPnlOrderClone(currentBuyOrder), boughtQuantity, Convert.ToDecimal(quantityToBeBoxed), ref listOfBrokenOrders);
                            FetchNextBuy(ref keepOnProgressing, ref currentBuyOrder, ref buyOrder);

                            //Do not remove this check - FetchNextBuy method null value if new Buy order not present
                            if (currentBuyOrder != null)
                                boughtQuantity = currentBuyOrder.Quantity;
                        }
                    }
                    while (currentBuyOrder != null)
                    {
                        listOfBrokenOrders.AddThreadSafely(currentBuyOrder);
                        FetchNextBuy(ref keepOnProgressing, ref currentBuyOrder, ref buyOrder);
                    }
                }
                else
                {
                    var buyOrder = buyList.GetEnumerator();
                    FetchNextBuy(ref keepOnProgressing, ref currentBuyOrder, ref buyOrder);
                    while (quantityToBeBoxed != 0)
                    {
                        if (currentBuyOrder != null)
                        {
                            quantityToBeBoxed = SplitAndGetLeftQuantity(CloneHelper.CreateEPnlOrderClone(currentBuyOrder), currentBuyOrder.Quantity, Convert.ToDecimal(quantityToBeBoxed), ref listOfBrokenOrders);
                            FetchNextBuy(ref keepOnProgressing, ref currentBuyOrder, ref buyOrder);
                        }
                    }
                    while (currentBuyOrder != null)
                    {
                        listOfBrokenOrders.AddThreadSafely(currentBuyOrder);
                        FetchNextBuy(ref keepOnProgressing, ref currentBuyOrder, ref buyOrder);
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

        private void FetchNextBuy(ref bool keepOnProgressing, ref EPnlOrder currentBuyOrder, ref IEnumerator<EPnlOrder> buyOrder)
        {
            try
            {
                if (buyOrder.MoveNext())
                {
                    currentBuyOrder = buyOrder.Current;
                }
                else
                {
                    currentBuyOrder = null;
                    keepOnProgressing = false;
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

        private void FetchNextSell(ref bool keepOnProgressing, ref EPnlOrder currentSellOrder, ref IEnumerator<EPnlOrder> sellOrder)
        {
            try
            {
                if (sellOrder.MoveNext())
                {
                    currentSellOrder = sellOrder.Current;
                }
                else
                {
                    currentSellOrder = null;
                    keepOnProgressing = false;
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

        private double SplitAndGetLeftQuantity(EPnlOrder currentBuyOrder, double boughtQuantity, decimal quantityToBeBoxed, ref ExposureAndPnlOrderCollection listOfBrokenOrders)
        {
            try
            {
                if (currentBuyOrder.Quantity == boughtQuantity && Convert.ToDecimal(boughtQuantity) == quantityToBeBoxed)
                {
                    currentBuyOrder.PositionSideExposureBoxed = PositionType.Boxed;
                    quantityToBeBoxed = 0;
                    listOfBrokenOrders.AddThreadSafely(currentBuyOrder);
                }
                else
                {
                    decimal quantityToBeBroken = Convert.ToDecimal(boughtQuantity) > quantityToBeBoxed ? quantityToBeBoxed : Convert.ToDecimal(boughtQuantity);
                    if (Convert.ToDecimal(currentBuyOrder.Quantity) == quantityToBeBroken)
                    {
                        currentBuyOrder.PositionSideExposureBoxed = PositionType.Boxed;
                        quantityToBeBoxed = quantityToBeBoxed - quantityToBeBroken;
                        listOfBrokenOrders.AddThreadSafely(currentBuyOrder);
                    }
                    else
                    {
                        EPnlOrder cloneOrder = CloneHelper.CreateEPnlOrderClone(currentBuyOrder);
                        cloneOrder.ID = currentBuyOrder.ID + "_2";
                        currentBuyOrder.ID = currentBuyOrder.ID + "_1";
                        cloneOrder.Quantity = Convert.ToDouble(quantityToBeBroken);
                        cloneOrder.PositionSideExposureBoxed = PositionType.Boxed;
                        currentBuyOrder.Quantity = currentBuyOrder.Quantity - Convert.ToDouble(quantityToBeBroken);
                        quantityToBeBoxed = quantityToBeBoxed - quantityToBeBroken;
                        listOfBrokenOrders.AddThreadSafely(cloneOrder);
                        listOfBrokenOrders.AddThreadSafely(currentBuyOrder);
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
            return Convert.ToDouble(quantityToBeBoxed);
        }
        #endregion
    }
}