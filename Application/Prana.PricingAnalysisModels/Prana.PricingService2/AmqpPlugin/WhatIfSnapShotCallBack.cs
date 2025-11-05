using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Enums;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Prana.PricingService2.AmqpPlugin
{

    /// <summary>
    /// Definition which handles any pre-trade snapshot request
    /// </summary>
    class WhatIfSnapShotCallBack : ILiveFeedCallback
    {
        internal event FurtherSnapShotRequestHandler FurtherSnapshotRequest;
        internal event FurtherSnapShotFxRequestHandler FurtherFxSnapshotRequest;
        internal event FurtherFxForwardSpotRequestHandler FurtherFxForwardSpotRequest;

        int _companyId;
        const String ROUTING_KEY = "LiveFeedStatus";
        String _otherDataExchange = String.Empty;
        bool isConnected;

        Dictionary<String, SnapshotResponse> _responseList = new Dictionary<String, SnapshotResponse>();

        /// <summary>
        /// When any new snapshot symboldata request arrives
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="underlyingSymbol"></param>
        /// <param name="fxSymbol"></param>
        /// <param name="currencyCode"></param>
        internal void AddPendingSnapshot(String symbol, String underlyingSymbol, String fxSymbol, int currencyCode, int leadCurrecnyId, int vsCurrencyId, AssetCategory assetId)
        {
            try
            {
                lock (_responseList)
                {
                    if (!isConnected)
                    {
                        SendCurrentStatus();
                        return;
                    }

                    if (!_responseList.ContainsKey(symbol))
                    {

                        try
                        {
                            List<String> requestSymbol = new List<string>();
                            requestSymbol.Add(symbol);//Adding main requestSymbol

                            if (!String.IsNullOrEmpty(underlyingSymbol))
                                requestSymbol.Add(underlyingSymbol);

                            String pranaForexSymbol = String.Empty;
                            //CHMW-3132	Fund wise fx rate handling for expiration settlement
                            if (!String.IsNullOrEmpty(fxSymbol))
                                pranaForexSymbol = ForexConverter.GetInstance(_companyId).GetPranaForexSymbolFromCurrencyToBaseCurrency(currencyCode);

                            SnapshotResponse snapResponse = new SnapshotResponse(symbol);
                            snapResponse.AddWaitingSymbolList(underlyingSymbol, pranaForexSymbol);
                            _responseList.Add(symbol, snapResponse);


                            if (assetId == AssetCategory.FXForward || assetId == AssetCategory.FX)
                            {

                                if (FurtherFxForwardSpotRequest != null)
                                    FurtherFxForwardSpotRequest(symbol, leadCurrecnyId, vsCurrencyId, assetId);

                                /*
                                 * Assuming the leadCurrencyId and vsCurrencyId are same for symbol and underlyning symbol.
                                 * Because in the case of Fx and FxForward the symbol and underlying symbol was same. So we were requesting of snapshots for a symbol. 
                                 * Now requesting for underlying symbol.
                                 * Assuming the underlying  asset id for Fx and Fx forward is Fx.
                                 */
                                if (!String.IsNullOrEmpty(underlyingSymbol) && FurtherFxForwardSpotRequest != null)
                                    FurtherFxForwardSpotRequest(underlyingSymbol, leadCurrecnyId, vsCurrencyId, AssetCategory.FX);

                            }
                            else
                            {
                                if (FurtherSnapshotRequest != null)
                                {
                                    FurtherSnapshotRequest(requestSymbol);
                                }
                            }
                            if (!String.IsNullOrEmpty(pranaForexSymbol) && FurtherFxSnapshotRequest != null)
                                FurtherFxSnapshotRequest(pranaForexSymbol, currencyCode);
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

        public WhatIfSnapShotCallBack()
        {
            try
            {
                _companyId = CachedDataManager.GetInstance.GetCompanyID();

                _otherDataExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);
                AmqpHelper.InitializeSender("OtherData", _otherDataExchange, MediaType.Exchange_Direct);
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

        #region ILiveFeedCallback Members
        /// <summary>
        /// Called when snapshot response arrives from LiveFeed
        /// </summary>
        /// <param name="data"></param>
        public void SnapshotResponse(Prana.BusinessObjects.SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            try
            {
                if (data != null)
                {
                    lock (_responseList)
                    {
                        List<String> removalKeys = new List<string>();

                        foreach (String key in _responseList.Keys)
                        {
                            SnapshotResponse snpResonse = _responseList[key];

                            bool isSent = snpResonse.SymbolDataRecieved(data);
                            if (isSent)
                                removalKeys.Add(key);
                        }

                        foreach (String key in removalKeys)
                        {
                            _responseList.Remove(key);
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

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        /// <summary>
        /// Sends status of LiveFeed to esper
        /// </summary>
        public void LiveFeedConnected()
        {
            try
            {
                isConnected = true;
                LiveFeedStatus status = new LiveFeedStatus();
                status.Status = true;
                AmqpHelper.SendObject(status, "OtherData", ROUTING_KEY);
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
        /// Sends status of LiveFeed to esper
        /// </summary>
        public void LiveFeedDisConnected()
        {
            try
            {

                isConnected = false;
                LiveFeedStatus status = new LiveFeedStatus();
                status.Status = false;
                AmqpHelper.SendObject(status, "OtherData", ROUTING_KEY);
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
        /// Sends status of LiveFeed to esper
        /// </summary>
        public void SendCurrentStatus()
        {
            try
            {
                if (isConnected)
                {
                    LiveFeedStatus status = new LiveFeedStatus();
                    status.Status = true;
                    AmqpHelper.SendObject(status, "OtherData", ROUTING_KEY);
                }
                else
                {
                    LiveFeedStatus status = new LiveFeedStatus();
                    status.Status = false;
                    AmqpHelper.SendObject(status, "OtherData", ROUTING_KEY);
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
        #endregion
    }

    /// <summary>
    /// Definition handling snapshot response object and maintains the required sequence in which symbol data needs to be sent
    /// </summary>
    internal class SnapshotResponse
    {
        const String WhatIf_Live_Feed_RoutingKey = "WhatIfSymbolData";

        String _requestedSymbol;
        SymbolData _requestedSymbolData;
        Dictionary<String, SymbolData> _waitingSymbolData = new Dictionary<string, SymbolData>();

        internal SnapshotResponse(String requestedSymbol)
        {
            try
            {
                this._requestedSymbol = requestedSymbol;
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
        /// Process internal list for all required symboldata
        /// </summary>
        /// <param name="symbolData">Recieved symboldata</param>
        /// <param name="result">Output result</param>
        /// <returns>True if sent to amqpo </returns>
        internal bool SymbolDataRecieved(SymbolData symbolData)//, out Dictionary<AssetCategory, String> result)
        {
            try
            {
                if (symbolData.Symbol == _requestedSymbol)
                {
                    this._requestedSymbolData = symbolData;
                }
                else if (_waitingSymbolData.Count > 0)
                {
                    //if syboldata is not equals to original requested symbol then it might be for derived requests
                    if (_waitingSymbolData.ContainsKey(symbolData.Symbol))
                    {
                        _waitingSymbolData[symbolData.Symbol] = symbolData;
                    }
                }

                if (CheckIfAllSymbolDataRecieved())
                {
                    //Check for all symboldata needed and then send to esper
                    SendAllSymbolDataToAmqp();
                    return true;//All symboldata recieved
                }

                return false;//Still waiting for all symboldata
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
                return false;
            }
        }

        /// <summary>
        /// Send all received symboldata to esper
        /// </summary>
        private void SendAllSymbolDataToAmqp()
        {
            try
            {
                //first send all waiting symboldata, Requested symboldata is required at last.
                //beacause it will send pending whatif to esper engine. 
                //So that other dependent symboldata must already be present in engine
                foreach (SymbolData symbolData in _waitingSymbolData.Values)
                {
                    Logger.LoggerWrite("Symbol: " + symbolData.Symbol + ", snapshot response send to Esper", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                    AmqpHelper.SendObject(symbolData, "OtherData", WhatIf_Live_Feed_RoutingKey);
                }

                Logger.LoggerWrite("Symbol: " + _requestedSymbolData.Symbol + ", snapshot response send to Esper", LoggingConstants.CATEGORY_FLAT_FILE_TRACING, 1, 1, TraceEventType.Information);
                AmqpHelper.SendObject(_requestedSymbolData, "OtherData", WhatIf_Live_Feed_RoutingKey);
                //InformationReporter.GetInstance.Write("Response Sent for symbol :" + _requestedSymbol + ", at: " + DateTime.Now.ToLongTimeString());
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
        /// Check if still waiting for some symbol
        /// </summary>
        /// <returns>true if all symboldata has been received otherwise false</returns>
        private bool CheckIfAllSymbolDataRecieved()
        {
            try
            {
                if (_waitingSymbolData.ContainsValue(null) || _requestedSymbolData == null)
                    return false;
                else return true;
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
                return false;
            }
        }

        internal void AddWaitingSymbolList(params String[] symbolList)
        {
            try
            {
                foreach (String symbol in symbolList)
                    if (!String.IsNullOrEmpty(symbol) && !_waitingSymbolData.ContainsKey(symbol))
                        _waitingSymbolData.Add(symbol, null);
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

        #region not in use
        /*
        /// <summary>
        /// If more symbol is needed it adds more symbol according  to assetCategory
        /// Put a check on null (Returns null if no more request is required)
        /// </summary>
        /// <param name="data">SymbolData which is under processing</param>
        /// <returns>Derived request symbols if needed or null</returns>
        private Dictionary<AssetCategory, String> GetDerivedRequestSymbols(SymbolData data)
        {
            Dictionary<AssetCategory, String> furtherRequest = new Dictionary<AssetCategory, String>();

            try
            {
                if (data != null)
                {
                    switch (data.CategoryCode)//== Prana.BusinessObjects.AppConstants.AssetCategory.Option)
                    {

                        case Prana.BusinessObjects.AppConstants.AssetCategory.Option:
                        case Prana.BusinessObjects.AppConstants.AssetCategory.EquityOption:
                        case Prana.BusinessObjects.AppConstants.AssetCategory.FutureOption:
                            furtherRequest.Add(AssetCategory.Equity, data.UnderlyingSymbol);
                            //_waitingSymbolData.Add(data.UnderlyingSymbol, null);
                            break;
                    }

                    int symbolDataCurrencyId = CachedDataManager.GetInstance.GetCurrencyID(data.CurencyCode);

                    if (symbolDataCurrencyId != _comapnyBaseCurrencyId && symbolDataCurrencyId != 11)
                    {
                        String pranaForexSymbol = ForexConverter.GetInstance(_companyId).GetPranaForexSymbolFromCurrencyToBaseCurrency(symbolDataCurrencyId);
                        //String eSignalFxSymbl = ForexConverter.GetInstance(_companyId).GetFxesignalSymbol(pranaForexSymbol);
                        furtherRequest.Add(AssetCategory.FX, pranaForexSymbol);
                        //_waitingSymbolData.Add(pranaForexSymbol, null);
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

            //Checking and sending requests
            //This might be as
            // Equity in Base currency => null
            // Equity in Foreign currrency=> count=1
            // Option in Base currency => count=1
            // Option in Foreign currency => count=2
            if (furtherRequest.Count > 0)
                return furtherRequest;
            else return null;
        }
        */
    }



    #endregion


    /// <summary>
    /// LiveFeed status definition
    /// </summary>
    public class LiveFeedStatus
    {
        public String DataCategory = "LiveFeedStatus";
        public bool Status = false;
    }

    internal delegate void FurtherFxForwardSpotRequestHandler(String symbol, int leadCurrencyId, int vsCurrencyId, AssetCategory assetCategory);
    internal delegate void FurtherSnapShotRequestHandler(List<String> symbol);
    internal delegate void FurtherSnapShotFxRequestHandler(String symbol, int fromCurencyCode);

}