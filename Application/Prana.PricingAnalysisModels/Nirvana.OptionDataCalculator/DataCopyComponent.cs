using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.ESignalAdapter;
using Prana.Global;
using Prana.LogManager;
using Prana.OptionCalculator.Common;
using Prana.QueueManager;
using Prana.SocketCommunication;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Prana.OptionCalculator.CalculationComponent
{
    class DataCopyComponent
    {
        BroadcastMemoryQueueManager _greekQueue = null;
        int _companyID = CommonDataCache.CachedDataManager.GetInstance.GetCompanyID();

        private int _greeksCalculationInterval = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("GreeksCalculationInterval"));
        private int _initialFilePricingDelay = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("InitialFilePricingDelay"));
        private bool _isLivePricesFromFile = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsLivePricesFromFile"));
        private bool _isExchangePricesFromFile = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsExchangePricesFromFile"));
        private List<string> _filePriceExchangeList = new List<string>();

        public DataCopyComponent(BroadcastMemoryQueueManager greekQueue)
        {
            try
            {
                _greekQueue = greekQueue;
                if (_isExchangePricesFromFile)
                {
                    string configExchangeString = ConfigurationHelper.Instance.GetAppSettingValueByKey("ExchangesForFilePricing");
                    _filePriceExchangeList = new List<string>(configExchangeString.Split(','));
                }
                ThreadPool.QueueUserWorkItem(new WaitCallback(CopyData));
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
        }

        private Dictionary<string, SymbolData> dictLiveFeedData = new Dictionary<string, SymbolData>();

        /// <summary>
        /// copies the data after a specified time 
        /// which is based on feed back details
        /// </summary>
        void CopyData(object state)
        {
            try
            {

                if (_isLivePricesFromFile)
                    Thread.Sleep(_initialFilePricingDelay);
                while (true)
                {
                    Thread.Sleep(_greeksCalculationInterval);

                    dictLiveFeedData = null;
                    dictLiveFeedData = LiveFeedManager.LiveFeedManager.GetInstance().GetLiveFeedDataDictCopy();

                    if (dictLiveFeedData == null)
                    {
                        dictLiveFeedData = new Dictionary<string, SymbolData>();
                    }
                    if (_isLivePricesFromFile)
                    {
                        if (CachedDataManager.GetInstance.IsFilePricingForTouch())
                        {

                            Dictionary<string, SymbolData> fileLiveFeedData = UpdatePricesFromFilePricingCache(new Dictionary<string, SymbolData>(), false, null);

                            fileLiveFeedData = SetSelectedFeedPriceAndPerformOMIChanges(fileLiveFeedData);
                            OptionInputValuesCache.GetInstance.GetAvailableDividends(fileLiveFeedData);
                            if (fileLiveFeedData != null && fileLiveFeedData.Count > 0)
                            {
                                QueueMessage queueMsg = new QueueMessage(OptionDataFormatter.MSGTYPE_PricingDataFile, fileLiveFeedData);
                                _greekQueue.SendMessage(queueMsg);
                            }
                        }
                        if (_isExchangePricesFromFile || !CachedDataManager.GetInstance.IsFilePricingForTouch())
                            dictLiveFeedData = UpdatePricesFromFilePricingCache(dictLiveFeedData, _isExchangePricesFromFile, _filePriceExchangeList);
                    }

                    dictLiveFeedData = SetSelectedFeedPriceAndPerformOMIChanges(dictLiveFeedData);
                    OptionInputValuesCache.GetInstance.GetAvailableDividends(dictLiveFeedData);
                    if (dictLiveFeedData != null && dictLiveFeedData.Count > 0)
                    {
                        QueueMessage queueMsg = new QueueMessage(OptionDataFormatter.MSGTYPE_PricingData, dictLiveFeedData);
                        _greekQueue.SendMessage(queueMsg);
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
        }

        private Dictionary<string, SymbolData> UpdatePricesFromFilePricingCache(Dictionary<string, SymbolData> dictLiveFeedData, bool isOnlyExchangePrices, List<string> exchanges)
        {
            try
            {
                if (isOnlyExchangePrices)
                {
                    Dictionary<string, SymbolData> temp = new Dictionary<string, SymbolData>();
                    FilePricingCache.UpdatePricesFromCache(temp);
                    foreach (var kvp in temp)
                    {
                        if (!string.IsNullOrWhiteSpace(kvp.Value.ListedExchange) && exchanges.Contains(kvp.Value.ListedExchange))
                            dictLiveFeedData[kvp.Key] = kvp.Value;
                    }
                }
                else
                    FilePricingCache.UpdatePricesFromCache(dictLiveFeedData);
                LiveFeedManager.LiveFeedManager.SymbolPreferenceHandling(new List<SymbolData>(dictLiveFeedData.Values), dictLiveFeedData);
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
            return dictLiveFeedData;
        }

        public Dictionary<string, SymbolData> SetSelectedFeedPriceAndPerformOMIChanges(Dictionary<string, SymbolData> liveFeedDataDict)
        {
            try
            {
                Dictionary<string, UserOptModelInput> OMIData = (Dictionary<string, UserOptModelInput>)OptionModelUserInputCache.Clone();
                if (liveFeedDataDict != null && liveFeedDataDict.Count > 0)
                {
                    foreach (KeyValuePair<string, SymbolData> individualElement in liveFeedDataDict)
                    {
                        UserOptModelInput userdata = null;

                        //CHMW-2560	CLONE -Apply Microsoft Managed Recommended Rules in Prana.OptionCalculator.CalculationComponent project
                        //SymbolData underlyingSymbolData = null;
                        SymbolData symboldata = individualElement.Value;
                        if (symboldata != null)
                        {
                            if (MarkCacheManager.LatesMarkPrices != null && MarkCacheManager.LatesMarkPrices.ContainsKey(symboldata.Symbol))
                            {
                                symboldata.MarkPrice = MarkCacheManager.LatesMarkPrices[symboldata.Symbol].MarkPrice;
                                symboldata.MarkPriceStr = MarkCacheManager.LatesMarkPrices[symboldata.Symbol].MarkPriceStr;
                            }
                        }
                        if (symboldata is EquitySymbolData)
                        {
                            OptionInputValuesCache.GetInstance.UpdateDividend(symboldata.Symbol, symboldata.DividendYield);
                        }
                        else if (symboldata is OptionSymbolData || symboldata is FutureOptionSymbolData)
                        {
                            OptionInputValuesCache.GetInstance.UpdateStockBorrowCost(symboldata.Symbol, symboldata.StockBorrowCost);
                        }

                        string symbol = individualElement.Key;

                        if (OMIData.ContainsKey(symbol))
                        {
                            userdata = OMIData[symbol];
                        }

                        if (FeedPriceChooser.UseClosingMark == true)
                        {
                            symboldata.SelectedFeedPrice = symboldata.MarkPrice;
                            symboldata.PricingSource = PricingSource.EODT_1Snapshot;
                        }
                        else
                        {
                            FeedPriceChooser.SetSelectedFeedPrice(ref symboldata);

                            /* OMI changes are applied here*/
                            # region OMI Changes For Selected Feed Price
                            if (OMIData.ContainsKey(symbol))
                            {
                                // Kuldeep A.: If closing mark is used for the symbol then we're giving it highest priority.
                                if (userdata != null)
                                {
                                    if (userdata.ClosingMarkUsed)
                                    {
                                        symboldata.SelectedFeedPrice = symboldata.MarkPrice;
                                        if (MarkCacheManager.LatesMarkPrices != null && MarkCacheManager.LatesMarkPrices.ContainsKey(symboldata.Symbol))
                                        {
                                            if (MarkCacheManager.LatesMarkPrices[symboldata.Symbol].MarkPriceIndicator == 1)
                                                symboldata.PricingSource = PricingSource.StaleClosingMarkPI;
                                            else
                                                symboldata.PricingSource = PricingSource.ClosingMarkPI;
                                        }
                                    }
                                    else
                                    {
                                        if (userdata.LastPriceUsed)
                                        {
                                            symboldata.SelectedFeedPrice = userdata.LastPrice;
                                            symboldata.PricingSource = PricingSource.UserDefined;
                                        }
                                        else if (userdata.ProxySymbolUsed)
                                        {
                                            symboldata.PricingSource = PricingSource.LiveFeedProxySymbol;
                                        }
                                        else if (userdata.TheoreticalPriceUsed)
                                        {
                                            symboldata.PricingSource = PricingSource.TheoreticalPrice;
                                        }
                                    }
                                }
                            }
                            # endregion OMI Changes For Selected Feed Price

                            //special case fx/fxforward to adjust selected feed px based on forward pts, if price not OMI overridden.
                            //Divya:Forward points are now not fx fxforward speicific. All assets can be forward point adjusted.                            
                        }
                        #region Other OMI Fields Overriding
                        if (symboldata.CategoryCode == AssetCategory.FutureOption)
                        {
                            OptionSymbolData opData = symboldata as OptionSymbolData;
                            if (OMIData.ContainsKey(symbol))
                            {
                                userdata = OMIData[symbol];
                            }
                            if (userdata != null)
                            {
                                opData.BloombergSymbol = userdata.Bloomberg;
                            }
                            string exchangeIdentifier = string.Empty;
                            if (opData != null)
                            {
                                if (string.IsNullOrEmpty(opData.ListedExchange) && opData.Symbol.Contains("LME"))
                                {
                                    exchangeIdentifier = "LME" + "-" + opData.CategoryCode.ToString();
                                }
                                else
                                {
                                    exchangeIdentifier = opData.ListedExchange + "-" + opData.CategoryCode.ToString();
                                }
                            }
                            if (string.Compare(exchangeIdentifier, "LME-FutureOption", true) == 0)
                            {
                                if (OMIData.ContainsKey(symbol))
                                {
                                    userdata = OMIData[symbol];
                                }
                                if (userdata != null)
                                {
                                    if (opData.UnderlyingSymbol != null)
                                    {
                                        opData.UnderlyingSymbol = userdata.UnderlyingSymbol;
                                    }
                                    opData.ExpirationDate = userdata.ExpirationDate;
                                    TimeSpan ts1 = opData.ExpirationDate.Date.Subtract(DateTime.Now.Date);
                                    int noOfDaysToExpiration = ts1.Days;
                                    opData.DaysToExpiration = noOfDaysToExpiration;
                                }
                            }
                        }
                        if (userdata != null)
                        {
                            if (userdata.DeltaUsed)
                            {
                                if (symboldata.CategoryCode == AssetCategory.EquityOption || symboldata.CategoryCode == AssetCategory.FutureOption)
                                {
                                    ((OptionSymbolData)symboldata).Delta = userdata.Delta;
                                    ((OptionSymbolData)symboldata).DeltaSource = DeltaSource.UserDefined;
                                }
                                if (symboldata.CategoryCode == AssetCategory.ConvertibleBond)
                                {
                                    ((FixedIncomeSymbolData)symboldata).Delta = userdata.Delta;
                                    ((FixedIncomeSymbolData)symboldata).DeltaSource = DeltaSource.UserDefined;
                                }
                            }

                            if (userdata.DividendUsed)
                            {
                                if (symboldata is EquitySymbolData)
                                {
                                    OptionInputValuesCache.GetInstance.UpdateDividend(symboldata.Symbol, userdata.Dividend);
                                }
                            }
                            if (userdata.StockBorrowCostUsed && (symboldata is OptionSymbolData || symboldata is FutureOptionSymbolData))
                            {
                                OptionInputValuesCache.GetInstance.UpdateStockBorrowCost(symboldata.Symbol, userdata.StockBorrowCost);
                            }

                            if (userdata.IntRateUsed)
                            {
                                if (symboldata.CategoryCode == AssetCategory.EquityOption || symboldata.CategoryCode == AssetCategory.FutureOption)
                                {
                                    ((OptionSymbolData)symboldata).FinalInterestRate = userdata.IntRate;
                                }
                            }
                            if (userdata.ForwardPointsUsed)
                            {
                                symboldata.ForwardPoints = userdata.ForwardPoints;
                            }
                            if (userdata.VolatilityUsed)
                            {
                                if (symboldata.CategoryCode == AssetCategory.EquityOption || symboldata.CategoryCode == AssetCategory.FutureOption)
                                {
                                    ((OptionSymbolData)symboldata).FinalImpliedVol = userdata.Volatility;
                                }
                            }
                            if (userdata.HistoricalVolUsed)
                            {
                                if (symboldata.CategoryCode == AssetCategory.EquityOption || symboldata.CategoryCode == AssetCategory.FutureOption)
                                {
                                    ((OptionSymbolData)symboldata).FinalImpliedVol = userdata.HistoricalVol;
                                }
                            }
                            if (userdata.SharesOutstandingUsed)
                            {
                                symboldata.SharesOutstanding = (long)userdata.SharesOutstanding;
                            }
                            else if (userdata.SMSharesOutstandingUsed)
                            {
                                symboldata.SharesOutstanding = (long)userdata.SMSharesOutstanding;
                            }
                            else
                            {
                                if (symboldata.SharesOutstanding == 0)
                                {
                                    if (userdata.SMSharesOutstanding != 0)
                                        symboldata.SharesOutstanding = (long)userdata.SMSharesOutstanding;
                                }
                                if (symboldata.CategoryCode == AssetCategory.EquityOption || symboldata.CategoryCode == AssetCategory.FutureOption || symboldata.CategoryCode == AssetCategory.Future)
                                {
                                    symboldata.SharesOutstanding = (long)symboldata.OpenInterest;
                                }
                            }
                            if (userdata == null || !userdata.LastPriceUsed)
                            {
                                symboldata.AdjustSelectedFeedPrice();
                            }
                        }
                        #endregion Other OMI Fields Overriding
                    }
                }

                List<UserOptModelInput> listDatatoAppend = new List<UserOptModelInput>(OMIData.Values);
                listDatatoAppend.RemoveAll(delegate (UserOptModelInput userOMI)
                {
                    if (liveFeedDataDict != null && liveFeedDataDict.ContainsKey(userOMI.Symbol))
                    {
                        return true;
                    }
                    return false;
                });

                SymbolDataHandling(liveFeedDataDict, listDatatoAppend);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return liveFeedDataDict;
        }

        public static void SymbolDataHandling(Dictionary<string, SymbolData> liveFeedDataDict, List<UserOptModelInput> listDatatoAppend)
        {
            try
            {
                //Bharat Kumar Jangir (06 June 2014)
                //also commented on JIRA http://jira.nirvanasolutions.com:8080/browse/PRANA-3666
                //There is no need of isOMIOverridden variable
                //because Currently we are flowing those symbol's prices which have either selected feed price or OMI overridden PX,
                //but now Convertible bond like asset classes not have own prices but still want to flow it's underlying prices and exposure calculations
                //So we are applying same process for all the asset categories - after discussion with Gaurav
                //bool isOMIOverridden = false;
                foreach (UserOptModelInput temp in listDatatoAppend)
                {
                    AssetCategory assetCat = (AssetCategory)temp.AssetID;
                    SymbolData data = null;
                    OptionSymbolData dataOpt = null;
                    if (!liveFeedDataDict.ContainsKey(temp.Symbol))
                    {
                        switch (assetCat)
                        {
                            case AssetCategory.Equity:// equity
                            case AssetCategory.PrivateEquity:// private equity
                            case AssetCategory.CreditDefaultSwap:// credit default swap
                                data = new EquitySymbolData();
                                data.CategoryCode = AssetCategory.Equity;
                                break;

                            case AssetCategory.EquityOption:// equity option
                                dataOpt = new OptionSymbolData();
                                dataOpt.CategoryCode = AssetCategory.EquityOption;
                                if (temp.StrikePrice != double.MinValue)
                                {
                                    dataOpt.StrikePrice = temp.StrikePrice;
                                }
                                else
                                {
                                    //else fetch the strike from symbol..
                                    dataOpt.StrikePrice = eSignalHelper.GetStrikePrice(dataOpt.Symbol);
                                }
                                if (temp.ExpirationDate != DateTime.MinValue)
                                {
                                    TimeSpan ts1 = temp.ExpirationDate.Date.Subtract(DateTime.Now.Date);
                                    int noOfDaysToExpiration = ts1.Days;
                                    dataOpt.DaysToExpiration = noOfDaysToExpiration;
                                    dataOpt.ExpirationDate = temp.ExpirationDate;
                                }
                                if (temp.PutorCall != OptionType.NONE)
                                {
                                    dataOpt.PutOrCall = temp.PutorCall;
                                }
                                break;

                            case AssetCategory.FutureOption:// furure option
                                dataOpt = new OptionSymbolData();
                                dataOpt.CategoryCode = AssetCategory.FutureOption;

                                if (temp.StrikePrice != double.MinValue)
                                {
                                    dataOpt.StrikePrice = temp.StrikePrice;
                                }
                                else
                                {
                                    //else fetch the strike from symbol..
                                    dataOpt.StrikePrice = eSignalHelper.GetStrikePrice(dataOpt.Symbol);
                                }
                                if (temp.ExpirationDate != DateTime.MinValue)
                                {
                                    TimeSpan ts1 = temp.ExpirationDate.Date.Subtract(DateTime.Now.Date);
                                    int noOfDaysToExpiration = ts1.Days;
                                    dataOpt.DaysToExpiration = noOfDaysToExpiration;
                                    dataOpt.ExpirationDate = temp.ExpirationDate;
                                }
                                if (temp.PutorCall != OptionType.NONE)
                                {
                                    dataOpt.PutOrCall = temp.PutorCall;
                                }
                                //dataOpt.StrikePrice = eSignalHelper.GetStrikePrice(dataOpt.Symbol);
                                break;

                            case AssetCategory.FXOption: // fx option
                                dataOpt = new OptionSymbolData();
                                dataOpt.CategoryCode = AssetCategory.FXOption;
                                dataOpt.StrikePrice = eSignalHelper.GetStrikePrice(dataOpt.Symbol);
                                break;

                            case AssetCategory.Future: // future
                                data = new FutureSymbolData();
                                data.CategoryCode = AssetCategory.Future;
                                break;

                            case AssetCategory.FX: //fx
                                data = new FxContractSymbolData();
                                data.CategoryCode = AssetCategory.FX;
                                break;

                            case AssetCategory.FXForward:// fxforward
                                data = new FxForwardContractSymbolData();
                                data.CategoryCode = AssetCategory.FXForward;
                                break;

                            case AssetCategory.Indices: // indices
                                data = new IndexSymbolData();
                                data.CategoryCode = AssetCategory.Indices;
                                break;

                            case AssetCategory.FixedIncome:// fixedincome
                                data = new FixedIncomeSymbolData();
                                data.CategoryCode = AssetCategory.FixedIncome;
                                break;

                            case AssetCategory.ConvertibleBond:// convertible bond
                                data = new FixedIncomeSymbolData();
                                data.CategoryCode = AssetCategory.ConvertibleBond;
                                break;

                            default:
                                data = new EquitySymbolData();
                                data.CategoryCode = AssetCategory.Equity;
                                break;
                        }
                        if (data != null)
                        {
                            data.Symbol = temp.Symbol;

                            if (MarkCacheManager.LatesMarkPrices != null && MarkCacheManager.LatesMarkPrices.ContainsKey(data.Symbol))
                            {
                                data.MarkPrice = MarkCacheManager.LatesMarkPrices[data.Symbol].MarkPrice;
                                data.MarkPriceStr = MarkCacheManager.LatesMarkPrices[data.Symbol].MarkPriceStr;

                                if (FeedPriceChooser.UseClosingMark)
                                {
                                    data.SelectedFeedPrice = data.MarkPrice;
                                    data.PricingSource = PricingSource.EODT_1Snapshot;
                                }
                                else if (temp.ClosingMarkUsed)
                                {
                                    data.SelectedFeedPrice = data.MarkPrice;
                                    if (MarkCacheManager.LatesMarkPrices[data.Symbol].MarkPriceIndicator == 1)
                                    {
                                        data.PricingSource = PricingSource.StaleClosingMarkPI;
                                    }
                                    else
                                    {
                                        data.PricingSource = PricingSource.ClosingMarkPI;
                                    }
                                }
                            }
                        }
                        if (dataOpt != null)
                        {
                            dataOpt.Symbol = temp.Symbol;
                            dataOpt.UnderlyingSymbol = temp.UnderlyingSymbol;

                            if (MarkCacheManager.LatesMarkPrices != null && MarkCacheManager.LatesMarkPrices.ContainsKey(dataOpt.Symbol))
                            {
                                dataOpt.MarkPrice = MarkCacheManager.LatesMarkPrices[dataOpt.Symbol].MarkPrice;
                                dataOpt.MarkPriceStr = MarkCacheManager.LatesMarkPrices[dataOpt.Symbol].MarkPriceStr;

                                if (FeedPriceChooser.UseClosingMark)
                                {
                                    dataOpt.SelectedFeedPrice = dataOpt.MarkPrice;
                                    dataOpt.PricingSource = PricingSource.EODT_1Snapshot;
                                }
                                else if (temp.ClosingMarkUsed)
                                {
                                    dataOpt.SelectedFeedPrice = dataOpt.MarkPrice;
                                    if (MarkCacheManager.LatesMarkPrices[dataOpt.Symbol].MarkPriceIndicator == 1)
                                    {
                                        dataOpt.PricingSource = PricingSource.StaleClosingMarkPI;
                                    }
                                    else
                                    {
                                        dataOpt.PricingSource = PricingSource.ClosingMarkPI;
                                    }
                                }
                            }
                        }
                    }
                    if (data != null)
                    {
                        // Kuldeep A.: Give highest priority to Closing Mark if it is selected from PI for the symbol.
                        if (!FeedPriceChooser.UseClosingMark && !temp.ClosingMarkUsed)
                        {
                            if (temp.LastPriceUsed)
                            {
                                data.SelectedFeedPrice = temp.LastPrice;
                                data.PricingSource = PricingSource.UserDefined;
                            }
                        }
                        if (temp.SharesOutstandingUsed)
                        {
                            data.SharesOutstanding = (long)temp.SharesOutstanding;
                        }
                        else if (temp.SMSharesOutstandingUsed)
                        {
                            data.SharesOutstanding = (long)temp.SMSharesOutstanding;
                        }
                        else
                        {
                            if (data.SharesOutstanding == 0)
                            {
                                if (temp.SMSharesOutstanding != 0)
                                    data.SharesOutstanding = (long)temp.SMSharesOutstanding;
                            }
                            if (temp.AssetID == 3)
                                data.SharesOutstanding = (long)data.OpenInterest;
                        }
                        if (temp.ForwardPointsUsed)
                        {
                            data.ForwardPoints = temp.ForwardPoints;
                        }
                    }

                    switch (assetCat)
                    {
                        case AssetCategory.Equity:
                        case AssetCategory.PrivateEquity:
                        case AssetCategory.CreditDefaultSwap:
                            if (data != null)
                            {
                                if (temp.DividendUsed)
                                {
                                    data.FinalDividendYield = temp.Dividend;
                                    OptionInputValuesCache.GetInstance.UpdateDividend(data.Symbol, temp.Dividend);
                                }
                                if (temp.StockBorrowCostUsed)
                                {
                                    data.StockBorrowCost = temp.StockBorrowCost;
                                    OptionInputValuesCache.GetInstance.UpdateStockBorrowCost(data.Symbol, temp.StockBorrowCost);
                                }
                                liveFeedDataDict.Add(data.Symbol, data);
                            }
                            break;

                        case AssetCategory.EquityOption:
                        case AssetCategory.FutureOption:
                        case AssetCategory.FXOption:
                            string exchangeIdentifier = string.Empty;
                            if (string.IsNullOrEmpty(dataOpt.ListedExchange) && dataOpt.Symbol.Contains("LME"))
                            {
                                exchangeIdentifier = "LME" + "-" + dataOpt.CategoryCode.ToString();
                            }
                            else
                            {
                                exchangeIdentifier = dataOpt.ListedExchange + "-" + dataOpt.CategoryCode.ToString();
                            }
                            if (dataOpt != null)
                            {
                                dataOpt.BloombergSymbol = temp.Bloomberg;
                                if (string.Compare(exchangeIdentifier, "LME-FutureOption", true) == 0)
                                {
                                    dataOpt.UnderlyingSymbol = temp.UnderlyingSymbol;
                                }
                                // Kuldeep A.: Give highest priority to Closing Mark if it is selected from PI for the symbol.
                                if (!FeedPriceChooser.UseClosingMark && !temp.ClosingMarkUsed)
                                {
                                    if (temp.LastPriceUsed)
                                    {
                                        dataOpt.SelectedFeedPrice = temp.LastPrice;
                                        dataOpt.PricingSource = PricingSource.UserDefined;
                                    }
                                    else if (temp.TheoreticalPriceUsed)
                                    {
                                        dataOpt.PricingSource = PricingSource.TheoreticalPrice;
                                    }
                                }
                                if (temp.DividendUsed)
                                {
                                    dataOpt.FinalDividendYield = temp.Dividend;
                                }
                                if (temp.StockBorrowCostUsed)
                                {
                                    dataOpt.StockBorrowCost = temp.StockBorrowCost;
                                    OptionInputValuesCache.GetInstance.UpdateStockBorrowCost(dataOpt.Symbol, temp.StockBorrowCost);
                                }
                                if (temp.HistoricalVolUsed)
                                {
                                    dataOpt.FinalImpliedVol = temp.HistoricalVol;
                                }
                                if (temp.IntRateUsed)
                                {
                                    dataOpt.FinalInterestRate = temp.IntRate;
                                }
                                if (temp.DeltaUsed)
                                {
                                    dataOpt.Delta = temp.Delta;
                                    dataOpt.DeltaSource = DeltaSource.UserDefined;
                                }
                                if (temp.VolatilityUsed)
                                {
                                    dataOpt.FinalImpliedVol = temp.Volatility;
                                }
                                if (temp.SharesOutstandingUsed)
                                {
                                    dataOpt.SharesOutstanding = (long)temp.SharesOutstanding;
                                }
                                else
                                {
                                    dataOpt.SharesOutstanding = (long)dataOpt.OpenInterest;
                                }
                                if (temp.ForwardPointsUsed)
                                {
                                    dataOpt.ForwardPoints = temp.ForwardPoints;
                                }
                                liveFeedDataDict.Add(dataOpt.Symbol, dataOpt);
                            }
                            break;

                        case AssetCategory.ConvertibleBond:
                            if (temp.DeltaUsed)
                            {
                                data.Delta = temp.Delta;
                                data.DeltaSource = DeltaSource.UserDefined;
                            }
                            liveFeedDataDict.Add(data.Symbol, data);
                            break;

                        default:
                            if (data != null)
                            {
                                liveFeedDataDict.Add(data.Symbol, data);
                            }
                            break;
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
