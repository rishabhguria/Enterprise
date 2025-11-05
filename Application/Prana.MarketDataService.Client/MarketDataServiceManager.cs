using Prana.BusinessLogic.Symbol;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using Prana.MarketDataAdapter.Common;
using Prana.MarketDataService.Common;
using Prana.OptionCalculator.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prana.MarketDataService.Client
{
    public class MarketDataServiceManager : ILiveFeedAdapter
    {
        private ConcurrentDictionary<string, SymbolData> _dictCachedData = new ConcurrentDictionary<string, SymbolData>();
        private HashSet<string> _requestedSymbols = new HashSet<string>();

        private static MarketDataServiceManager _instance;

        /// <summary>
        /// The locker object
        /// </summary>
        private static object _lockerObject = new object();
        /// <summary>
        /// Singleton Instance for the DataManager which fetch data for level 1 quotes.
        /// </summary>
        /// <returns>
        /// Instance of eSignalManager
        /// </returns>
        public static MarketDataServiceManager GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock (_lockerObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new MarketDataServiceManager();
                        }
                    }
                }

                return _instance;
            }
            catch (Exception)
            {

                return null;
            }
        }

        private MarketDataServiceManager()
        {
            _proxyInstance = new MarketDataProxy();
        }

        private MarketDataProxy _proxyInstance = null;

        public void Connect()
        {
            FilePricingCache.Connected += FilePricingCache_Connected;
        }

        private void FilePricingCache_Connected(object sender, EventArgs<bool> e)
        {
            if (Connected != null)
                Connected(this, e);
        }

        public void Disconnect()
        {
            FilePricingCache.Connected -= FilePricingCache_Connected;

        }

        public void GetContinuousData(string symbol)
        {

        }

        /// <summary>
        /// Gets the snap shot data.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="symbologyCode">The symbology code.</param>
        /// <param name="completeInfo">if set to <c>true</c> [complete information].</param>
        public void GetSnapShotData(string symbol, ApplicationConstants.SymbologyCodes symbologyCode, bool isSMRequest)
        {
            try
            {
                if (_proxyInstance != null)
                {
                    if (isSMRequest && _requestedSymbols.Contains(symbol))
                        return;
                    MarketDataSymbolResponse resp = MarketDataAdapterExtension.GetMarketDataSymbolInformationFromTickerSymbol(symbol);
                    if (resp != null)
                    {
                        MDServiceReqObject obj = new MDServiceReqObject { Ticker = symbol, Asset = resp.AssetCategory.ToString(), Exchange = MarketDataAdapterExtension.GetExchangeName(resp.AUECID) };
                        Dictionary<string, string> dict = null;
                        if (isSMRequest)
                        {
                            _requestedSymbols.Add(symbol);
                            dict = _proxyInstance.GetSMData(obj);
                        }
                        else
                            dict = _proxyInstance.SnapshotSymbol(obj);
                        if (dict != null)
                        {
                            SymbolData liveData = GetSymbolData(symbol, resp);
                            PricingSymbolDataMapper.ParseDataRowToSymbolData(liveData, null, dict.Keys.ToList(), ApplicationConstants.SymbologyCodes.TickerSymbol, dict);

                            if (liveData.CategoryCode == AssetCategory.EquityOption)
                            {
                                OptionDetail opt = OptionSymbolGenerator.GetOptionDetailObj(symbol, symbologyCode, string.Empty);
                                liveData.ExpirationDate = opt.ExpirationDate;
                                liveData.StrikePrice = opt.StrikePrice;
                                liveData.PutOrCall = opt.OptionType;
                                liveData.UnderlyingSymbol = opt.UnderlyingSymbol;
                            }
                            else
                                liveData.UnderlyingSymbol = liveData.Symbol;
                            Data data = new Data();
                            data.Info = liveData;
                            if (SnapShotDataResponse != null)
                                SnapShotDataResponse(this, data);
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

        private SymbolData GetSymbolData(string symbol, MarketDataSymbolResponse response)
        {
            SymbolData data = new SymbolData();
            try
            {
                switch (response.AssetCategory)
                {
                    case AssetCategory.Equity:// equity
                    case AssetCategory.PrivateEquity:// private equity
                    case AssetCategory.CreditDefaultSwap:// credit default swap
                        data = new EquitySymbolData();
                        data.CategoryCode = AssetCategory.Equity;
                        break;

                    case AssetCategory.EquityOption:// equity option
                        data = new OptionSymbolData();
                        data.CategoryCode = AssetCategory.EquityOption;
                        break;

                    case AssetCategory.FutureOption:// furure option
                        data = new OptionSymbolData();
                        data.CategoryCode = AssetCategory.FutureOption;
                        break;

                    case AssetCategory.FXOption: // fx option
                        data = new OptionSymbolData();
                        data.CategoryCode = AssetCategory.FXOption;
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

                data.AUECID = response.AUECID;
                data.ListedExchange = MarketDataAdapterExtension.GetExchangeName(response.AUECID);
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return data;
        }

        public void GetOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter)
        {

        }

        public void DeleteSymbol(string symbol)
        {

        }

        public List<SymbolData> GetAvailableLiveFeed()
        {
            return _dictCachedData.Values.ToList();
        }

        public Task<object> GetLiveDataDirectlyFromFeed()
        {
            return null;
        }

        public Dictionary<string, bool> CheckIfInternationalSymbols(List<string> symbols)
        {
            return null;
        }

        public event EventHandler<Prana.Global.EventArgs<bool>> Connected;

        #pragma warning disable CS0067
        //Suppressing these warnings as these events are not used in the current implementation but can't remove them as they are part of the interface contract.
        public event EventHandler<Prana.Global.EventArgs<bool>> Disconnected;

        public event EventHandler<Data> ContinuousDataResponse;

        public event EventHandler<Data> SnapShotDataResponse;

        public event EventHandler<EventArgs<string, List<OptionStaticData>>> OptionChainResponse;
        #pragma warning restore CS0067


        public void SetDebugEnableDisable(bool isDebugEnable, double pctTolerance)
        {

        }

        public void UpdateSecurityDetails(BusinessObjects.SecurityMasterBusinessObjects.SecMasterbaseList secMasterList)
        {

        }

        public void SendResult(Dictionary<string, Dictionary<string, string>> result)
        {
            throw new NotImplementedException();
        }


        public Dictionary<string, string> GetUserInformation()
        {
            throw new NotImplementedException();
        }

        public List<Dictionary<string, string>> GetUserPermissionsInformation()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, int> GetSubscriptionInformation()
        {
            throw new NotImplementedException();
        }
        public Dictionary<string, string> GetTickersLastStatusCode()
        {
            throw new NotImplementedException();
        }
    }
}
