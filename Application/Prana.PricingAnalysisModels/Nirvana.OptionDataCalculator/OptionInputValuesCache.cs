using Prana.BusinessObjects;
using Prana.LogManager;
//using Prana.LiveFeedProvider;
//using Prana.InstanceCreator;
using Prana.OptionCalculator.Common;
using Prana.PricingAnalysisModels;
using System;
using System.Collections.Generic;
//using Prana.FeedSubscriber;

namespace Prana.OptionCalculator.CalculationComponent
{
    public class OptionInputValuesCache
    {
        //static double _interestRateFromIDC = Convert.ToDouble(ConfigurationHelper.Instance.GetAppSettingValueByKey("InterestRate"));
        static OptionInputValuesCache _DividendAndIVCache = null;
        //ILiveFeedPublisher _liveFeedCacheInstance = LiveFeedInstanceCreator.Instance;
        //LiveFeedSubscriber _liveFeedSubscriber = LiveFeedSubscriber.Instance;
        static object _lockerDivYield = new object();
        //int identifier = int.MinValue;
        double _DividendYield = 0;
        double _stockBorrowCost = 0;
        Dictionary<string, double> _dividentList;
        Dictionary<string, double> _stockBorrowCostList;

        private static int _binomialSteps = 0;
        public static int BinomialSteps
        {
            get { return _binomialSteps; }
            set { _binomialSteps = value; }
        }

        private static int _volatilityIterations = 0;
        public static int VolatilityIterations
        {
            get { return _volatilityIterations; }
            set { _volatilityIterations = value; }
        }

        private static PricingAnalysisModelsEnum _selectedPricingModel = PricingAnalysisModelsEnum.None;
        public static PricingAnalysisModelsEnum SelectedPricingModel
        {
            get { return _selectedPricingModel; }
            set { _selectedPricingModel = value; }
        }


        static OptionInputValuesCache()
        {
            _DividendAndIVCache = new OptionInputValuesCache();
        }

        /// <summary>
        /// Caches dividend yields received from LiveFeed and Implied Vols calculated from Greeks Calculator and OptionsServer Manager
        /// </summary>
        private OptionInputValuesCache()
        {
            //_DividendYield = Convert.ToDouble(System.Configuration.ConfigurationSettings.AppSettings["Dividend"]);
            _dividentList = new Dictionary<string, double>();
            _stockBorrowCostList = new Dictionary<string, double>();
            //identifier = this.GetHashCode();

            //if (_liveFeedSubscriber == null)
            //{
            //    return;
            //}
            ///Need to subscribe before requesting for the continuous data.
            //    _liveFeedCacheInstance.Subscribe(identifier, 500);
            // _liveFeedCacheInstance.PublishLevel1SnapshotResponse += new EventHandler(_liveFeedCacheInstance_PublishLevel1SnapshotResponse);
            //_liveFeedSubscriber.PublishLevel1SnapshotResponse += new EventHandler<Data>(PublishLevel1SnapshotResponse);

        }
        public static OptionInputValuesCache GetInstance
        {
            get
            {
                return _DividendAndIVCache;
            }
        }


        /// <summary>
        /// This dividend cache contains the OMI overridden values for the dividends
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public double GetDividend(string symbol)
        {
            double divYield = _DividendYield;
            try
            {
                lock (_lockerDivYield)
                {
                    if (symbol != null)
                    {
                        if (!_dividentList.ContainsKey(symbol))
                        {
                            //only first request will be sent to live feed for dividend
                            // _liveFeedCacheInstance.RequestLevel1Snapshot(symbol, this.GetHashCode(), ApplicationConstants.SymbologyCodes.TickerSymbol);
                            //_liveFeedSubscriber.RequestLevel1Snapshot(symbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                            _dividentList.Add(symbol, _DividendYield);

                        }
                        else
                        {
                            divYield = _dividentList[symbol];
                        }
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
            return divYield;
        }

        /// <summary>
        /// This dividend cache contains the OMI overridden values for the dividends
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public double GetStockBorrowCost(string symbol)
        {
            double stockCost = _stockBorrowCost;
            try
            {
                lock (_lockerDivYield)
                {
                    if (symbol != null)
                    {
                        if (!_stockBorrowCostList.ContainsKey(symbol))
                        {
                            //only first request will be sent to live feed for dividend
                            // _liveFeedCacheInstance.RequestLevel1Snapshot(symbol, this.GetHashCode(), ApplicationConstants.SymbologyCodes.TickerSymbol);
                            //_liveFeedSubscriber.RequestLevel1Snapshot(symbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                            _stockBorrowCostList.Add(symbol, stockCost);

                        }
                        else
                        {
                            stockCost = _stockBorrowCostList[symbol];
                        }
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
            return stockCost;
        }

        /// <summary>
        /// Updates dividends in Cache, made private as should be updated only when data is received from LiveFeed for Underlying Symbol
        /// This should update the OMI overriden dividendyields
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="dividend"></param>
        public void UpdateDividend(string symbol, double dividend)
        {
            try
            {
                if (string.Empty != symbol)
                {
                    if (dividend > 0)
                    {
                        lock (_lockerDivYield)
                        {
                            if (_dividentList.ContainsKey(symbol))
                            {
                                _dividentList[symbol] = dividend;
                            }
                            else
                            {
                                _dividentList.Add(symbol, dividend);
                            }
                        }
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

        /// <summary>
        /// Updates the stock borrow cost.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="stockBorrowCost">The stock borrow cost.</param>
        public void UpdateStockBorrowCost(string symbol, double stockBorrowCost)
        {
            try
            {
                if (string.Empty != symbol && stockBorrowCost > 0)
                {
                    lock (_lockerDivYield)
                    {
                        _stockBorrowCostList[symbol] = stockBorrowCost;
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



        /// <summary>
        /// Allows caller to update it's own dividend information from cache
        /// </summary>
        /// <param name="pricingDataList"></param>
        public void GetAvailableDividends(Dictionary<string, SymbolData> pricingDataList)
        {
            try
            {
                Dictionary<string, UserOptModelInput> OMIData = (Dictionary<string, UserOptModelInput>)OptionModelUserInputCache.Clone();
                foreach (KeyValuePair<string, SymbolData> dataItem in pricingDataList)
                {
                    if (dataItem.Value is EquitySymbolData)
                    {
                        if (OMIData.ContainsKey(dataItem.Value.Symbol) && OMIData[dataItem.Value.Symbol].DividendUsed)
                            dataItem.Value.FinalDividendYield = GetDividend(dataItem.Value.Symbol);
                    }
                    else
                    {
                        dataItem.Value.FinalDividendYield = GetDividend(dataItem.Value.UnderlyingSymbol);
                    }

                    dataItem.Value.StockBorrowCost = GetStockBorrowCost(dataItem.Value.Symbol);
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

        void PublishLevel1SnapshotResponse(object sender, Data e)
        {
            try
            {
                //LiveFeedEventArgs liveFeedArg = e as LiveFeedEventArgs;
                // SymbolDataLiveFeedEventArgs liveFeedArg = e as SymbolDataLiveFeedEventArgs;
                //if ( e.Info!= null)
                //    {
                //    SymbolData level1Data = e.Info;
                //    float dividendYieldInDec = Convert.ToSingle((level1Data.DividendYield * 1.0) / 100);
                //    UpdateDividend(level1Data.Symbol, dividendYieldInDec);
                //    //lock (locker)
                //    //{
                //    //    if (_impliedVolList.ContainsKey(level1Data.Symbol))
                //    //    {
                //    //        _impliedVolList.Remove(level1Data.Symbol);
                //    //    }
                //    //}
                //    }
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


        //public static double InterestRateFromIDC
        //{
        //    get { return _interestRateFromIDC; }
        //}

    }
}
