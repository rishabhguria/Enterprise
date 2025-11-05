using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.LiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
//using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Linq;

namespace Prana.SecurityMasterNew
{

    class SecMasterDataCache
    {
        static SecMasterDataCache _secMasterDataCache = null;
        private static readonly object _lockerObject = new object();
        private static readonly object dataLocker = new object();

        #region Compliance section
        //public event SecurityObjectReceivedHandler SecurityObjectReceived;
        public event EventHandler<EventArgs<SecMasterBaseObj>> SendSecurityToCompliance;
        //public event AuecDetailsUpdatedHandler AuecDetailsUpdated;
        //public event UDADataReceivedHandler UDADataReceived;
        #endregion


        private SecMasterGlobalPreferences _secMasterPreferences = new SecMasterGlobalPreferences();

        public SecMasterGlobalPreferences SecMasterPreferences
        {
            get { return _secMasterPreferences; }
            set { _secMasterPreferences = value; }
        }

        //private bool _isFutureCutOffTimeUsed = false;

        //public bool IsFutureCutOffTimeUsed
        //{
        //    get { return _isFutureCutOffTimeUsed; }
        //    set { _isFutureCutOffTimeUsed = value; }
        //}

        //renamed _contractMultipliers cache name to _FutRootSymbolCachedData - omshiv, nov 2013
        static Dictionary<string, List<FutureRootData>> _FutRootSymbolCachedData = new Dictionary<string, List<FutureRootData>>();

        //private Dictionary<string, SecMasterCoreObject> _secMasterCoreData = new Dictionary<string, SecMasterCoreObject>();
        private Dictionary<int, SecMasterSymbologyData> _secMasterAllSymbology = new Dictionary<int, SecMasterSymbologyData>();
        private Dictionary<long, string[]> _pkWiseSymbologyKeys = new Dictionary<long, string[]>();

        delegate void AsyncInvokeDelegate(Delegate del, params object[] args);
        bool _loggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LOGGINGENABLED"]);
        private SecMasterDataCache()
        {
            //_subscriberSnapShotHash = new Dictionary<int, List<string>>();
            _subscriberSnapShotHash = new Dictionary<int, Dictionary<string, SecMasterReqDataCached>>();
            FillSymbologyData();

        }
        public void Clean()
        {
            FillSymbologyData();
        }
        public void FillSymbologyData()
        {
            _secMasterAllSymbology = new Dictionary<int, SecMasterSymbologyData>();
            Array symbologyArray = Enum.GetValues(typeof(ApplicationConstants.SymbologyCodes));

            foreach (object symbologyCode in symbologyArray)
            {
                int value = (int)symbologyCode;
                SecMasterSymbologyData secMasterSymbologyData = new SecMasterSymbologyData();
                if (!_secMasterAllSymbology.ContainsKey(value))
                {
                    _secMasterAllSymbology.Add(value, secMasterSymbologyData);
                }
            }
        }

        public void SetPreferences(SecMasterGlobalPreferences preferences)
        {
            _secMasterPreferences.UseCutOffTime = preferences.UseCutOffTime;
        }

        public static SecMasterDataCache GetInstance
        {
            get
            {

                lock (_lockerObject)
                {
                    if (_secMasterDataCache == null)
                    {
                        _secMasterDataCache = new SecMasterDataCache();
                    }
                    return _secMasterDataCache;
                }
            }
        }
        public string GetKey(string symbol, DateTime date)
        {
            return symbol + date.Year.ToString() + date.Month.ToString() + date.Day.ToString();
        }
        //public void AddCoreData(string symbol, DateTime date, SecMasterCoreObject secMasterCoreData)
        //{ 
        //    string key=GetKey(symbol,date);
        //    if (!_secMasterCoreData.ContainsKey(key))
        //    {
        //        _secMasterCoreData.Add(key, secMasterCoreData);
        //    }
        //    else
        //    {
        //        _secMasterCoreData[key].UpDateData(secMasterCoreData);
        //    }
        //}

        /// <summary>
        /// Get All SM data from cache on Tickersymbol symbology
        /// </summary>
        /// <returns></returns>
        public List<SecMasterBaseObj> GetAllSecMasterData()
        {
            List<SecMasterBaseObj> secMasterBaseObjList = new List<SecMasterBaseObj>();

            try
            {
                int tickerCode = Convert.ToInt32(ApplicationConstants.SymbologyCodes.TickerSymbol);
                foreach (SecMasterBaseObj obj in _secMasterAllSymbology[tickerCode].GetAllData())
                {
                    secMasterBaseObjList.Add(obj);
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

            return secMasterBaseObjList;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="date"></param>
        /// <param name="symbologyCode"></param>
        /// <returns></returns>

        public List<SecMasterBaseObj> GetSecMasterData(SecMasterRequestObj secMasterRequestObj, DateTime date)
        {
            List<SecMasterBaseObj> newlist = new List<SecMasterBaseObj>();
            try
            {
                foreach (SymbolDataRow symbolDataRow in secMasterRequestObj.SymbolDataRowCollection)
                {

                    if (!String.IsNullOrWhiteSpace(symbolDataRow.PrimarySymbol))
                    {
                        string key = GetKey(symbolDataRow.PrimarySymbol, date);
                        int code = (int)symbolDataRow.PrimarySymbology;
                        List<SecMasterBaseObj> list = _secMasterAllSymbology[code].GetData(key);
                        if (list != null)
                        {
                            foreach (SecMasterBaseObj secMasterObj in list)
                            {
                                SecMasterBaseObj secMasterResponse = DeepCopyHelper.Clone<SecMasterBaseObj>(secMasterObj); //(SecMasterBaseObj)secMasterObj.Clone();
                                secMasterResponse.RequestedSymbology = (int)symbolDataRow.PrimarySymbology;
                                if (secMasterResponse.AUECID > 0)
                                {
                                    secMasterResponse.SourceOfData = SecMasterConstants.SecMasterSourceOfData.Database;
                                }
                                else
                                {
                                    secMasterResponse.SourceOfData = SecMasterConstants.SecMasterSourceOfData.None;
                                }
                                newlist.Add(secMasterResponse);
                            }
                        }
                    }

                    if (newlist.Count == 0 && !String.IsNullOrWhiteSpace(symbolDataRow.BBGID))
                    {
                        //Currently, I am assuming the cache is updated with ticker symbol in case of any symbology search.
                        int code = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;

                        List<SecMasterBaseObj> list = _secMasterAllSymbology[code].GetAllData();
                        if (list != null && list.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in list)
                            {
                                if (secMasterObj.BBGID.Trim().Equals(symbolDataRow.BBGID))
                                {
                                    SecMasterBaseObj secMasterResponse = (SecMasterBaseObj)secMasterObj.Clone();
                                    secMasterResponse.RequestedSymbology = (int)symbolDataRow.PrimarySymbology;
                                    if (secMasterResponse.AUECID > 0)
                                    {
                                        secMasterResponse.SourceOfData = SecMasterConstants.SecMasterSourceOfData.Database;
                                    }
                                    else
                                    {
                                        secMasterResponse.SourceOfData = SecMasterConstants.SecMasterSourceOfData.None;
                                    }
                                    newlist.Add(secMasterResponse);
                                    break;
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

            return newlist;

        }


        /// <summary>
        /// Add SM data to cache with single SM object
        /// </summary>
        /// <param name="secMasterObj"></param>
        /// <param name="date"></param>
        public void AddValues(SecMasterBaseObj secMasterObj, DateTime date)
        {
            try
            {
                int code = 0;
                foreach (string symbolInDiffSymbology in secMasterObj.SymbologyMapping)
                {
                    lock (dataLocker)
                    {
                        UpdatePKWiseCache(date, secMasterObj, code, symbolInDiffSymbology);

                        if (symbolInDiffSymbology != string.Empty)
                        {
                            _secMasterAllSymbology[code].Add(symbolInDiffSymbology.ToUpper(), date, secMasterObj);
                            UpdateLeveragedFactorForUnderlyings(_secMasterAllSymbology[code], secMasterObj);
                        }
                    }
                    code++;
                }

                #region Compliance Section
                //send security details to compliance, PRANA-21715
                if (SendSecurityToCompliance != null)
                    SendSecurityToCompliance(this, new EventArgs<SecMasterBaseObj>(secMasterObj));
                #endregion

                // SetExtraFromCache(secMasterObj);
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

        private void UpdateLeveragedFactorForUnderlyings(SecMasterSymbologyData secMasterSymbologyData, SecMasterBaseObj secMasterBaseObj)
        {
            try
            {
                foreach (SecMasterBaseObj secMasterObj in secMasterSymbologyData.GetAllData())
                {
                    if (secMasterObj.UnderLyingSymbol == secMasterBaseObj.TickerSymbol)
                    {
                        secMasterObj.Delta = secMasterBaseObj.Delta;
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

        /// <summary>
        /// Add SM data to cache with Secmaster obj list
        /// </summary>
        /// <param name="secMasterObjCollection"></param>
        /// <param name="date"></param>
        public void AddValues(List<SecMasterBaseObj> secMasterObjCollection, DateTime date)
        {
            try
            {
                foreach (SecMasterBaseObj secMasterObj in secMasterObjCollection)
                {
                    int code = 0;
                    foreach (string symbolInDiffSymbology in secMasterObj.SymbologyMapping)
                    {
                        lock (dataLocker)
                        {
                            if (_secMasterAllSymbology.ContainsKey(code))
                            {
                                UpdatePKWiseCache(date, secMasterObj, code, symbolInDiffSymbology);

                                if (symbolInDiffSymbology != string.Empty)
                                {
                                    _secMasterAllSymbology[code].Add(symbolInDiffSymbology.ToUpper(), date, secMasterObj);
                                }
                            }
                        }
                        code++;
                    }

                    #region Compliance Section

                    //send security details to compliance, PRANA-21715
                    if (SendSecurityToCompliance != null)
                        SendSecurityToCompliance(this, new EventArgs<SecMasterBaseObj>(secMasterObj));
                    #endregion

                    //it should be before adding to cache and only on creat/ update sec
                    // SetExtraFromCache(secMasterObj);

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
        /// Updates the pk wise cache.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="secMasterObj">The sec master object.</param>
        /// <param name="code">The code.</param>
        /// <param name="symbolInDiffSymbology">The symbol in difference symbology.</param>
        private void UpdatePKWiseCache(DateTime date, SecMasterBaseObj secMasterObj, int code, string symbolInDiffSymbology)
        {
            try
            {
                if (_pkWiseSymbologyKeys.ContainsKey(secMasterObj.Symbol_PK))
                {
                    string oldSymbol = _pkWiseSymbologyKeys[secMasterObj.Symbol_PK][code];
                    if (!string.IsNullOrEmpty(oldSymbol) && oldSymbol != symbolInDiffSymbology)
                    {
                        _secMasterAllSymbology[code].Remove(oldSymbol.ToUpper(), date, secMasterObj);
                    }
                }
                else
                {
                    _pkWiseSymbologyKeys.Add(secMasterObj.Symbol_PK, new string[12]);
                }
                _pkWiseSymbologyKeys[secMasterObj.Symbol_PK][code] = symbolInDiffSymbology;
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
        /// Get SecMaster object from SymbolData object (e-signal data)  
        /// </summary>
        /// <param name="level1Data"></param>
        /// <returns></returns>
        public SecMasterBaseObj GetSecMasterObj(SymbolData level1Data)
        {
            SecMasterBaseObj secMasterObj = null;
            try
            {
                secMasterObj = SecurityMasterFactory.GetSecmasterObject(level1Data.CategoryCode);
                if (secMasterObj != null)
                {
                    secMasterObj.FillData(level1Data);
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
            return secMasterObj;
        }

        /// <summary>
        /// Update whole cache based on underlying based on underlying Symbol 
        /// om, Oct 2013
        /// </summary>
        /// <param name="secMasterData"></param>
        internal SecMasterbaseList UpdateUDAInSMCacheOfUnderlying(SecMasterbaseList secMasterObjCollection, SecMasterbaseList updatedSecMasterCacheData)
        { 
            try
            {
                List<SecMasterBaseObj> secMasterCacheData = new List<SecMasterBaseObj>();

                int tickerCode = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
                if (_secMasterAllSymbology.ContainsKey(tickerCode))
                {
                    SecMasterSymbologyData secMasterSymbologyData = _secMasterAllSymbology[tickerCode];
                    secMasterCacheData = secMasterSymbologyData.GetAllData();
                }
                foreach (SecMasterBaseObj secMasterbaseObj in secMasterObjCollection)
                {
                    foreach (SecMasterBaseObj secMasterCacheObj in secMasterCacheData)
                    {
                        if (secMasterCacheObj.UnderLyingSymbol.Equals(secMasterbaseObj.TickerSymbol, StringComparison.OrdinalIgnoreCase)
                            && !secMasterCacheObj.TickerSymbol.Equals(secMasterbaseObj.TickerSymbol, StringComparison.OrdinalIgnoreCase)
                            && secMasterbaseObj.SymbolUDAData != null)
                        {
                            // set UDA of underlying to thier derivative
                            SetSymbolUDAData(secMasterbaseObj, secMasterCacheObj);

                            //Update Dynamic UDAs
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-9019
                            Dictionary<string, DynamicUDA> _dynamicUDAcache = UDADataCache.GetInstance.GetDynamicUDAList();
                            foreach (string uda in _dynamicUDAcache.Keys)
                            {
                                if (secMasterbaseObj.DynamicUDA.ContainsKey(uda) && secMasterbaseObj.DynamicUDA[uda].ToString() != _dynamicUDAcache[uda].DefaultValue && secMasterbaseObj.DynamicUDA[uda].ToString() != "Undefined")
                                {
                                    if (secMasterCacheObj.DynamicUDA.ContainsKey(uda))
                                        secMasterCacheObj.DynamicUDA[uda] = secMasterbaseObj.DynamicUDA[uda];
                                    else
                                        secMasterCacheObj.DynamicUDA.Add(uda, secMasterbaseObj.DynamicUDA[uda]);
                                }
                            }

                            updatedSecMasterCacheData.Add(secMasterCacheObj);
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
            return updatedSecMasterCacheData;
        }

        /// <summary>
        /// set symbol uda data
        /// </summary>
        /// <param name="secMasterbaseObj"></param>
        /// <param name="secMasterCacheObj"></param>
        public static void SetSymbolUDAData(SecMasterBaseObj secMasterbaseObj, SecMasterBaseObj secMasterCacheObj)
        {
            try
            {
                secMasterCacheObj.SymbolUDAData.Symbol = secMasterCacheObj.TickerSymbol;
                secMasterCacheObj.SymbolUDAData.CompanyName = secMasterCacheObj.LongName;
                secMasterCacheObj.SymbolUDAData.CountryID = secMasterbaseObj.SymbolUDAData.CountryID;
                secMasterCacheObj.SymbolUDAData.AccountID = secMasterbaseObj.SymbolUDAData.AccountID;
                secMasterCacheObj.SymbolUDAData.SectorID = secMasterbaseObj.SymbolUDAData.SectorID;
                secMasterCacheObj.SymbolUDAData.SecurityTypeID = secMasterbaseObj.SymbolUDAData.SecurityTypeID;
                secMasterCacheObj.SymbolUDAData.SubSectorID = secMasterbaseObj.SymbolUDAData.SubSectorID;
                secMasterCacheObj.SymbolUDAData.UDACountry = secMasterbaseObj.SymbolUDAData.UDACountry;
                secMasterCacheObj.SymbolUDAData.UDASector = secMasterbaseObj.SymbolUDAData.UDASector;
                secMasterCacheObj.SymbolUDAData.UDASecurityType = secMasterbaseObj.SymbolUDAData.UDASecurityType;
                secMasterCacheObj.SymbolUDAData.UDASubSector = secMasterbaseObj.SymbolUDAData.UDASubSector;
                secMasterCacheObj.SymbolUDAData.UnderlyingSymbol = secMasterbaseObj.SymbolUDAData.UnderlyingSymbol;
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
        /// Sets Dynamic UDA data of underlying.
        /// </summary>
        /// <param name="secMasterbaseObj"></param>
        /// <param name="secMasterCacheObj"></param>
        public static void SetDynamicUDAData(SecMasterBaseObj secMasterbaseObj, SecMasterBaseObj secMasterCacheObj)
        {
            try
            {
                Dictionary<string, DynamicUDA> _dynamicUDACache = UDADataCache.GetInstance.GetDynamicUDAList();
                foreach (string uda in _dynamicUDACache.Keys)
                {
                    if (secMasterCacheObj.DynamicUDA.ContainsKey(uda))
                    {
                        // If the key exists in secMasterCacheObj, update it with the value from secMasterbaseObj
                        if (secMasterbaseObj.DynamicUDA.TryGetValue(uda, out var baseUdaValue))
                            secMasterCacheObj.DynamicUDA[uda] = baseUdaValue;
                    }
                    else if (secMasterbaseObj.DynamicUDA.ContainsKey(uda))
                    {
                        // If the key doesn't exist in secMasterCacheObj but exists in secMasterbaseObj, add it
                        secMasterCacheObj.DynamicUDA.Add(uda, secMasterbaseObj.DynamicUDA[uda]);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Update issuer in cache based on underlying Symbol 
        /// </summary>
        /// <param name="secMasterbaseObj"></param>
        internal void UpdateIssuerInSMCacheOfUnderlying(SecMasterBaseObj secMasterbaseObj)
        {
            try
            {
                List<SecMasterBaseObj> secMasterCacheData = new List<SecMasterBaseObj>();

                int tickerCode = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
                if (_secMasterAllSymbology.ContainsKey(tickerCode))
                {
                    SecMasterSymbologyData secMasterSymbologyData = _secMasterAllSymbology[tickerCode];
                    secMasterCacheData = secMasterSymbologyData.GetAllData();
                }
                foreach (SecMasterBaseObj secMasterCacheObj in secMasterCacheData)
                {
                    if (secMasterCacheObj.UnderLyingSymbol.Equals(secMasterbaseObj.TickerSymbol, StringComparison.OrdinalIgnoreCase))
                    {
                        if (secMasterCacheObj.DynamicUDA.ContainsKey("Issuer"))
                            secMasterCacheObj.DynamicUDA["Issuer"] = secMasterbaseObj.DynamicUDA["Issuer"];
                        else
                            secMasterCacheObj.DynamicUDA.Add("Issuer", secMasterbaseObj.DynamicUDA["Issuer"]);

                        //add in cache
                        SecMasterDataCache.GetInstance.AddValues(secMasterCacheObj, DateTime.UtcNow);
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

        /// <summary>
        /// Returns AuecDetails instance for given auecId
        /// </summary>
        /// <param name="auecId"></param>
        /// <returns></returns>
        internal AuecDetails GetAuecDetails(int auecId)
        {
            AuecDetails auecDetails = new AuecDetails();

            try
            {
                auecDetails.AuecId = auecId;

                auecDetails.Today = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(auecId));
                auecDetails.YesterDay = auecDetails.Today.AddDays(-1);

                auecDetails.AssetId = CachedDataManager.GetInstance.GetAssetIdByAUECId(auecId);
                auecDetails.Asset = CachedDataManager.GetInstance.GetAssetText(auecDetails.AssetId);

                auecDetails.ExchangeId = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(auecId);
                auecDetails.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(auecDetails.ExchangeId);

                auecDetails.CurrencyId = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(auecId);
                auecDetails.Currency = CachedDataManager.GetInstance.GetCurrencyText(auecDetails.CurrencyId);

                auecDetails.UnderlyingId = CachedDataManager.GetInstance.GetUnderlyingID(auecId);
                auecDetails.Underlying = CachedDataManager.GetInstance.GetUnderLyingText(auecDetails.UnderlyingId);

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
            return auecDetails;
        }

        /// <summary>
        /// Set extra SM data to secmaster object, when SM comes from e-signal
        /// modified by Omshiv, Nov 2013
        /// </summary>
        /// <param name="dataObj"></param>
        public void SetExtraFromCache(SecMasterBaseObj dataObj)
        {
            try
            {
                AssetCategory assetCategory = (AssetCategory)dataObj.AssetID;
                string[] str = dataObj.TickerSymbol.Split(' ');
                string[] exchange = dataObj.TickerSymbol.Split('-');
                switch (assetCategory)
                {
                    case AssetCategory.Future:
                        //case AssetCategory.FXForward:
                        SecMasterFutObj futObj = (SecMasterFutObj)dataObj;

                        if (str.Length > 1)
                        {
                            string rootSymbol = str[0];
                            if (_FutRootSymbolCachedData.ContainsKey(rootSymbol))
                            {
                                // if (_FutRootSymbolCachedData[rootSymbol].Count > 1)
                                // {
                                Boolean isFound = false;
                                foreach (FutureRootData rootData in _FutRootSymbolCachedData[rootSymbol])
                                {
                                    if (exchange.Length > 1 && rootData.Exchange.Equals(exchange[1]))
                                    {
                                        isFound = true;
                                    }
                                    else if (exchange.Length == 1)
                                    {
                                        isFound = true;
                                    }
                                    if (isFound)
                                    {
                                        if (futObj.Multiplier == 0)
                                            futObj.Multiplier = rootData.Multiplier;

                                        futObj.CutOffTime = rootData.CutoffTime;
                                        futObj.IsCurrencyFuture = rootData.IsCurrencyFuture;
                                        MergeUDASymbolDataFrmRootSymbol(dataObj, rootData);
                                        break;

                                    }
                                }

                            }
                        }

                        break;



                    case AssetCategory.FutureOption:
                        SecMasterOptObj futOptionObj = (SecMasterOptObj)dataObj;

                        if (str.Length > 1)
                        {
                            string rootSymbol = str[0];
                            if (_FutRootSymbolCachedData.ContainsKey(rootSymbol))
                            {
                                Boolean isFound = false;
                                foreach (FutureRootData rootData in _FutRootSymbolCachedData[rootSymbol])
                                {
                                    if (exchange.Length > 1 && rootData.Exchange.Equals(exchange[1]))
                                    {
                                        isFound = true;
                                    }
                                    else if (exchange.Length == 1)
                                    {
                                        isFound = true;
                                    }
                                    if (isFound)
                                    {
                                        if (futOptionObj.Multiplier == 0)
                                            futOptionObj.Multiplier = rootData.Multiplier;

                                        futOptionObj.IsCurrencyFuture = rootData.IsCurrencyFuture;
                                        MergeUDASymbolDataFrmRootSymbol(dataObj, rootData);
                                        break;
                                    }
                                }
                            }
                        }

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

        /// <summary>
        /// Merging UDA data to future/future option from fut root data from cache
        /// </summary>
        /// <param name="dataObj"></param>
        /// <param name="rootData"></param>
        private void MergeUDASymbolDataFrmRootSymbol(SecMasterBaseObj dataObj, FutureRootData rootData)
        {
            try
            {
                dataObj.SymbolUDAData = new UDAData();
                //dataObj.SymbolUDAData.AssetID = rootData.UDAAssetClassID;
                dataObj.SymbolUDAData.SectorID = rootData.UDASectorID;
                dataObj.SymbolUDAData.SubSectorID = rootData.UDASubSectorID;
                dataObj.SymbolUDAData.SecurityTypeID = rootData.UDASecurityTypeID;
                dataObj.SymbolUDAData.CountryID = rootData.UDACountryID;
                dataObj.SymbolUDAData.Symbol = dataObj.TickerSymbol;

                // set UDA Asset Class same as Asset class
                if (dataObj.AssetCategory == AssetCategory.EquityOption || dataObj.AssetCategory == AssetCategory.FutureOption)
                {
                    int putOrCall = (dataObj as SecMasterOptObj).PutOrCall;
                    dataObj.SymbolUDAData.AssetID = UDADataCache.GetInstance.GetUDAAssetFromParameters(dataObj.AssetCategory.ToString(), putOrCall);
                }
                else
                {
                    dataObj.SymbolUDAData.AssetID = rootData.UDAAssetClassID;
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

        # region SecMasterCacheMgr Invoke part
        // private Dictionary<int, List<string>> _subscriberSnapShotHash = new Dictionary<int, List<string>>();
        // private Dictionary<int, List<string>> _subscriberSnapShotHash = null;

        private Dictionary<int, Dictionary<string, SecMasterReqDataCached>> _subscriberSnapShotHash = null;

        public Dictionary<int, Dictionary<string, SecMasterReqDataCached>> SubscriberSnapShotHash
        {
            get { return _subscriberSnapShotHash; }
            // set { _subscriberSnapShotHash = value; }
        }

        Dictionary<int, SubscriberDetails> _subscriberHash = new Dictionary<int, SubscriberDetails>();
        readonly object snapShotLocker = new object();


        //internal void RequestSymbolData(SecMasterRequestObj secMasterRequestObj)
        //{
        //    try
        //    {
        //        List<string> listSymbols = secMasterRequestObj.GetPrimarySymbols();
        //        foreach (string symbol in listSymbols)
        //        {
        //            AddIntoSnapShotSubscribers(symbol, secMasterRequestObj.HashCode);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// Register the symbols for asynchronous return
        /// </summary>
        /// <param name="secMasterRequestObj"></param>
        /// <param name="secMasterReqObjTransformed"></param>
        internal void RequestSymbolData(SecMasterRequestObj secMasterRequestObj, SecMasterRequestObj secMasterReqObjTransformed)
        {
            try
            {
                if (secMasterReqObjTransformed == null)
                {
                    secMasterReqObjTransformed = secMasterRequestObj;
                }


                List<string> listSymbols = secMasterReqObjTransformed.GetPrimarySymbols();
                int i = 0;
                StringBuilder sb = new StringBuilder();
                foreach (string symbol in listSymbols)
                {
                    SecMasterReqDataCached cachedData = new SecMasterReqDataCached();

                    String requestedSymbol = symbol;
                    //TODO: need to check for expiration date and underlying symbol, omshiv
                    if (!string.IsNullOrWhiteSpace(secMasterRequestObj.SymbolDataRowCollection[i].BBGID))
                    {
                        cachedData.RequestedSymbol = secMasterRequestObj.SymbolDataRowCollection[i].BBGID;
                        requestedSymbol = secMasterRequestObj.SymbolDataRowCollection[i].BBGID;
                        // cachedData.RequestedSymbology = secMasterRequestObj.SymbolDataRowCollection[i].PrimarySymbology;
                        // cachedData.ExpirationDate = secMasterReqObjTransformed.SymbolDataRowCollection[i].ExpirationDate;
                        //  cachedData.UnderlyingSymbol = secMasterReqObjTransformed.SymbolDataRowCollection[i].UnderlyingSymbol;
                    }
                    else
                    {
                        cachedData.RequestedSymbol = secMasterRequestObj.SymbolDataRowCollection[i].PrimarySymbol;
                        cachedData.RequestedSymbology = (int)secMasterRequestObj.SymbolDataRowCollection[i].PrimarySymbology;
                        cachedData.ExpirationDate = secMasterReqObjTransformed.SymbolDataRowCollection[i].ExpirationDate;
                        cachedData.UnderlyingSymbol = secMasterReqObjTransformed.SymbolDataRowCollection[i].UnderlyingSymbol;
                    }
                    i++;
                    AddIntoSnapShotSubscribers(requestedSymbol, cachedData, secMasterReqObjTransformed.HashCode);
                    sb.Append(symbol + ", ");
                }
                if (_loggingEnabled)
                    Logger.LoggerWrite("Symbol validation Request ID: " + secMasterReqObjTransformed.HashCode + " for symbols " + sb.ToString() + " Which was received at:" + DateTime.UtcNow.ToString(), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);

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

        internal void AddIntoSnapShotSubscribers(string symbol, SecMasterReqDataCached cachedData, int senderHashCode)
        {
            Dictionary<string, SecMasterReqDataCached> dictSnapShotReqData = null;
            //List<string> snapShotSymbolList = null;
            lock (snapShotLocker)
            {
                if (_subscriberSnapShotHash.ContainsKey(senderHashCode))
                {
                    dictSnapShotReqData = _subscriberSnapShotHash[senderHashCode];
                    if (!dictSnapShotReqData.ContainsKey(symbol))
                    {
                        //List<SecMasterReqDataCached> listCachedData = new List<SecMasterReqDataCached>();
                        //listCachedData.Add(cachedData);
                        dictSnapShotReqData.Add(symbol, cachedData);
                    }
                    else
                    {
                        dictSnapShotReqData[symbol] = cachedData;
                    }
                }
                else
                {
                    dictSnapShotReqData = new Dictionary<string, SecMasterReqDataCached>();
                    //List<SecMasterReqDataCached> listCachedDataNew = new List<SecMasterReqDataCached>();
                    //listCachedDataNew.Add(cachedData);

                    dictSnapShotReqData.Add(symbol, cachedData);
                    _subscriberSnapShotHash.Add(senderHashCode, dictSnapShotReqData);
                }
            }
        }
        internal void RemoveFromSnapShotSubscribers(SecMasterBaseObj secMasterObj, int senderHashCode)
        {

            Dictionary<string, SecMasterReqDataCached> dictSnapShotData = null;
            // List<string> snapShotSymbolList = null;
            lock (snapShotLocker)
            {
                if (_subscriberSnapShotHash.ContainsKey(senderHashCode))
                {
                    dictSnapShotData = _subscriberSnapShotHash[senderHashCode];
                    if (dictSnapShotData.ContainsKey(secMasterObj.TickerSymbol))
                    {
                        dictSnapShotData.Remove(secMasterObj.TickerSymbol);
                    }
                    if (dictSnapShotData.ContainsKey(secMasterObj.IDCOOptionSymbol))
                    {
                        dictSnapShotData.Remove(secMasterObj.IDCOOptionSymbol);
                    }
                    if (dictSnapShotData.ContainsKey(secMasterObj.SedolSymbol))
                    {
                        dictSnapShotData.Remove(secMasterObj.SedolSymbol);
                    }
                    if (dictSnapShotData.ContainsKey(secMasterObj.ISINSymbol))
                    {
                        dictSnapShotData.Remove(secMasterObj.ISINSymbol);
                    }
                    if (dictSnapShotData.ContainsKey(secMasterObj.CusipSymbol))
                    {
                        dictSnapShotData.Remove(secMasterObj.CusipSymbol);
                    }
                    if (dictSnapShotData.ContainsKey(secMasterObj.BloombergSymbol))
                    {
                        dictSnapShotData.Remove(secMasterObj.BloombergSymbol);
                    }
                    if (dictSnapShotData.ContainsKey(secMasterObj.FactSetSymbol))
                    {
                        dictSnapShotData.Remove(secMasterObj.FactSetSymbol);
                    }
                    if (dictSnapShotData.ContainsKey(secMasterObj.ActivSymbol))
                    {
                        dictSnapShotData.Remove(secMasterObj.ActivSymbol);
                    }
                    if (dictSnapShotData.ContainsKey(secMasterObj.BloombergSymbolWithExchangeCode))
                    {
                        dictSnapShotData.Remove(secMasterObj.BloombergSymbolWithExchangeCode);
                    }
                    //Modified by omshiv, remove SnapShotData request for BBGID
                    if (!String.IsNullOrWhiteSpace(secMasterObj.BBGID) && dictSnapShotData.ContainsKey(secMasterObj.BBGID.ToUpper()))
                    {
                        dictSnapShotData.Remove(secMasterObj.BBGID);
                    }
                }
            }
        }
        internal bool IsSnapShotRequested(SecMasterBaseObj secMasterObj, int senderHashCode)
        {
            lock (snapShotLocker)
            {
                if (_subscriberSnapShotHash.ContainsKey(senderHashCode))
                {
                    // List<string> snapShotSymbolList = _subscriberSnapShotHash[senderHashCode];

                    Dictionary<string, SecMasterReqDataCached> snapShotData = _subscriberSnapShotHash[senderHashCode];

                    if (snapShotData.ContainsKey(secMasterObj.TickerSymbol.ToUpper()))
                    {
                        return true;
                    }
                    else if (snapShotData.ContainsKey(secMasterObj.IDCOOptionSymbol.ToUpper()))
                    {
                        return true;
                    }
                    else if (snapShotData.ContainsKey(secMasterObj.SedolSymbol.ToUpper()))
                    {
                        return true;
                    }
                    else if (snapShotData.ContainsKey(secMasterObj.ISINSymbol.ToUpper()))
                    {
                        return true;
                    }
                    else if (snapShotData.ContainsKey(secMasterObj.BloombergSymbol.ToUpper()))
                    {
                        return true;
                    }
                    else if (snapShotData.ContainsKey(secMasterObj.BloombergSymbolWithExchangeCode.ToUpper()))
                    {
                        return true;
                    }
                    else if (snapShotData.ContainsKey(secMasterObj.CusipSymbol.ToUpper()))
                    {
                        return true;
                    }
                    else if (snapShotData.ContainsKey(secMasterObj.OpraSymbol.ToUpper()))
                    {
                        return true;
                    }
                    else if (snapShotData.ContainsKey(secMasterObj.ReutersSymbol.ToUpper()))
                    {
                        return true;
                    }
                    else if (snapShotData.ContainsKey(secMasterObj.FactSetSymbol.ToUpper()))
                    {
                        return true;
                    }
                    else if (snapShotData.ContainsKey(secMasterObj.ActivSymbol.ToUpper()))
                    {
                        return true;
                    }
                    //Modified by omshiv, check SnapShotData requested for BBGID or not
                    else if (!String.IsNullOrWhiteSpace(secMasterObj.BBGID) && snapShotData.ContainsKey(secMasterObj.BBGID.ToUpper()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal void UpdateSecMasterResponse(SecMasterBaseObj secMasterObj, int senderHashCode)
        {
            try
            {
                lock (snapShotLocker)
                {
                    if (_subscriberSnapShotHash.ContainsKey(senderHashCode))
                    {
                        // List<string> snapShotSymbolList = _subscriberSnapShotHash[senderHashCode];

                        Dictionary<string, SecMasterReqDataCached> snapShotData = _subscriberSnapShotHash[senderHashCode];


                        //bloomberg symbol will be present incase the reponse is from SMDB.
                        string symbol = secMasterObj.BloombergSymbol;

                        if (string.IsNullOrEmpty(symbol))
                        {
                            symbol = secMasterObj.TickerSymbol;
                        }

                        //Modified by omshiv, BBGID check 
                        SecMasterReqDataCached cachedData = null;
                        if (!String.IsNullOrWhiteSpace(symbol) && snapShotData.ContainsKey(symbol))
                        {
                            cachedData = snapShotData[symbol];

                        }
                        else if (!String.IsNullOrWhiteSpace(secMasterObj.BBGID) && snapShotData.ContainsKey(secMasterObj.BBGID))
                        {
                            cachedData = snapShotData[secMasterObj.BBGID];
                        }
                        else
                        {
                            if (_loggingEnabled)
                                Logger.LoggerWrite("Ticker Symbol is: " + symbol + " and BloombergSymbol is :" + secMasterObj.BloombergSymbol + " and BBGID is : " + secMasterObj.BBGID + " senderHashCode is: " + senderHashCode + "Which was received at:" + DateTime.UtcNow.ToString(), LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                        }

                        if (cachedData != null)
                        {

                            if (cachedData.ExpirationDate != DateTime.MinValue)
                            {
                                if (secMasterObj is SecMasterOptObj)
                                {
                                    ((Prana.BusinessObjects.SecurityMasterBusinessObjects.SecMasterOptObj)(secMasterObj)).ExpirationDate = cachedData.ExpirationDate;
                                }
                            }
                            if (!string.IsNullOrEmpty(cachedData.UnderlyingSymbol) && secMasterObj.SourceOfData != SecMasterConstants.SecMasterSourceOfData.Internal)
                            {

                                secMasterObj.UnderLyingSymbol = cachedData.UnderlyingSymbol;
                            }
                            switch ((ApplicationConstants.SymbologyCodes)cachedData.RequestedSymbology)
                            {
                                case ApplicationConstants.SymbologyCodes.TickerSymbol:
                                    break;
                                case ApplicationConstants.SymbologyCodes.ReutersSymbol:
                                    secMasterObj.ReutersSymbol = cachedData.RequestedSymbol;
                                    secMasterObj.RequestedSymbology = cachedData.RequestedSymbology;
                                    secMasterObj.AddData(cachedData.RequestedSymbol, (ApplicationConstants.SymbologyCodes)cachedData.RequestedSymbology);
                                    break;
                                case ApplicationConstants.SymbologyCodes.ISINSymbol:
                                    secMasterObj.ISINSymbol = cachedData.RequestedSymbol;
                                    secMasterObj.RequestedSymbology = cachedData.RequestedSymbology;
                                    secMasterObj.AddData(cachedData.RequestedSymbol, (ApplicationConstants.SymbologyCodes)cachedData.RequestedSymbology);
                                    break;
                                case ApplicationConstants.SymbologyCodes.SEDOLSymbol:
                                    secMasterObj.SedolSymbol = cachedData.RequestedSymbol;
                                    secMasterObj.RequestedSymbology = cachedData.RequestedSymbology;
                                    secMasterObj.AddData(cachedData.RequestedSymbol, (ApplicationConstants.SymbologyCodes)cachedData.RequestedSymbology);
                                    break;
                                case ApplicationConstants.SymbologyCodes.CUSIPSymbol:
                                    secMasterObj.CusipSymbol = cachedData.RequestedSymbol;
                                    secMasterObj.RequestedSymbology = cachedData.RequestedSymbology;
                                    secMasterObj.AddData(cachedData.RequestedSymbol, (ApplicationConstants.SymbologyCodes)cachedData.RequestedSymbology);
                                    break;
                                case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                                    secMasterObj.BloombergSymbol = cachedData.RequestedSymbol;
                                    secMasterObj.RequestedSymbology = cachedData.RequestedSymbology;
                                    secMasterObj.AddData(cachedData.RequestedSymbol, (ApplicationConstants.SymbologyCodes)cachedData.RequestedSymbology);
                                    break;
                                case ApplicationConstants.SymbologyCodes.OSIOptionSymbol:
                                case ApplicationConstants.SymbologyCodes.IDCOOptionSymbol:
                                case ApplicationConstants.SymbologyCodes.OPRAOptionSymbol:
                                    break;
                                default:
                                    break;
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

        public void Subscribe(int senderHashCode)
        {
            try
            {

                SubscriberDetails newSubscribeDetails = new SubscriberDetails();
                if (!_subscriberHash.ContainsKey(senderHashCode))
                    _subscriberHash.Add(senderHashCode, newSubscribeDetails);
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

        public void UnSubscribe(int senderHashCode)
        {
            try
            {
                if (_subscriberHash.ContainsKey(senderHashCode))
                    _subscriberHash.Remove(senderHashCode);
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
        /// 
        /// </summary>
        /// <param name="sink"></param>
        /// <param name="args"></param>
        //private static void InvokeDelegate(Delegate sink, params object[] args)
        //{
        //    try
        //    {

        //        sink.DynamicInvoke(args);

        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        # endregion

        /// <summary>
        /// Update future root data cache and 
        /// Update Sec mastrer cache of server on  root symbol add/edit
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public SecMasterbaseList UpdateFutureSymbolsFromRootSymbol(DataTable dt)
        {
            SecMasterbaseList updatedFutureSymbols = new SecMasterbaseList();
            SecMasterbaseList listSecMasterBaseObj = new SecMasterbaseList();
            try
            {
                #region  Update futureRoot data cahce
                // update rootsymbol in server cache
                foreach (DataRow row in dt.Rows)
                {
                    string rootSymbol = row["Symbol"].ToString();
                    List<FutureRootData> listfutureRootData = new List<FutureRootData>();

                    FutureRootData futureRootData = new FutureRootData();
                    GetRootDataObjFromDataRow(row, futureRootData);

                    if (_FutRootSymbolCachedData.ContainsKey(rootSymbol))
                    {
                        listfutureRootData = _FutRootSymbolCachedData[rootSymbol];
                        bool isUpdated = false;

                        foreach (FutureRootData rootdata in listfutureRootData)
                        {
                            if (rootdata.Exchange.Equals(futureRootData.Exchange))
                            {
                                GetRootDataObjFromDataRow(row, rootdata);
                                isUpdated = true;
                                break;
                            }
                        }
                        if (isUpdated.Equals(false))
                        {
                            listfutureRootData.Add(futureRootData);
                        }

                        _FutRootSymbolCachedData[rootSymbol] = listfutureRootData;
                        //}
                        //else
                        //{
                        //    //??? why not checking Exchange here - omshiv 
                        //    List<FutureRootData> futureRootDataList = _FutRootSymbolCachedData[rootSymbol];
                        //    GetRootDataObjFromDataRow(row, futureRootDataList[0]);
                        //}
                    }
                    else
                    {
                        listfutureRootData.Add(futureRootData);
                        _FutRootSymbolCachedData.Add(rootSymbol, listfutureRootData);
                    }

                    #region  Update SecMaster Data cahce for this root symbol
                    int tickerCode = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
                    if (_secMasterAllSymbology.ContainsKey(tickerCode))
                    {
                        SecMasterSymbologyData secMasterSymbologyData = _secMasterAllSymbology[tickerCode];
                        listSecMasterBaseObj = secMasterSymbologyData.UpdateSMInCacheOnRootDataUpdate(futureRootData);
                    }

                    updatedFutureSymbols.AddRange(listSecMasterBaseObj);
                    #endregion
                }
                #endregion

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
            return updatedFutureSymbols;
        }

        internal void SetFutureRootData(DataSet ds)
        {
            try
            {
                if (ds.Tables.Count > 0)
                {
                    DataTable dataTable = ds.Tables[0];
                    _FutRootSymbolCachedData.Clear();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Boolean isFoundRootData = false;
                        if (row["Symbol"] != DBNull.Value && row["Multiplier"] != DBNull.Value)
                        {
                            List<FutureRootData> futRootdata = new List<FutureRootData>();
                            string key = row["Symbol"].ToString().Trim();
                            if (_FutRootSymbolCachedData.ContainsKey(key))
                            {
                                futRootdata = _FutRootSymbolCachedData[key];
                                foreach (FutureRootData root in futRootdata)
                                {
                                    if (root.Exchange.Equals(row["Exchange"].ToString()))
                                    {
                                        GetRootDataObjFromDataRow(row, root);
                                        isFoundRootData = true;
                                        break;
                                    }
                                }
                                if (!isFoundRootData)
                                {
                                    FutureRootData rootData = new FutureRootData();
                                    GetRootDataObjFromDataRow(row, rootData);
                                    futRootdata.Add(rootData);
                                }
                                // _FutRootSymbolCachedData[key] = futRootdata;
                            }
                            else
                            {
                                FutureRootData rootData = new FutureRootData();
                                rootData = GetRootDataObjFromDataRow(row, rootData);
                                futRootdata.Add(rootData);
                                _FutRootSymbolCachedData.Add(key, futRootdata);
                            }
                            //rootData.UnderlyingSymbol = row["UnderlyingSymbol"].ToString();
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
        /// Get future root data object from Data row.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private FutureRootData GetRootDataObjFromDataRow(DataRow row, FutureRootData rootData)
        {
            try
            {
                rootData.RootSymbol = row["Symbol"].ToString().Trim();
                rootData.PSRootSymbol = row["PSSymbol"].ToString();
                rootData.Multiplier = double.Parse(row["Multiplier"].ToString());
                rootData.Exchange = row["Exchange"].ToString();
                if (row.Table.Columns.Contains("CutOffTime") && row["CutOffTime"] != DBNull.Value && row["CutOffTime"].ToString() != "")
                {
                    rootData.CutoffTime = Convert.ToDateTime(row["CutOffTime"].ToString()).ToString("HH:mm:ss");
                }
                else
                {
                    rootData.CutoffTime = DateTime.Now.ToString("HH:mm:ss");
                }
                //UDA fileds, added by Om, Nov 2013
                rootData.UDAAssetClassID = int.Parse(row["UDAAssetClassID"].ToString());
                rootData.UDASecurityTypeID = int.Parse(row["UDASecurityTypeID"].ToString());
                rootData.UDASectorID = int.Parse(row["UDASectorID"].ToString());
                rootData.UDASubSectorID = int.Parse(row["UDASubSectorID"].ToString());
                rootData.UDACountryID = int.Parse(row["UDACountryID"].ToString());
                rootData.IsCurrencyFuture = bool.Parse(row["IsCurrencyFuture"].ToString());

                if (row.Table.Columns.Contains(BloombergSapiConstants.CONST_BLOOMBERG_BBG_YELLOW_KEY) && row[BloombergSapiConstants.CONST_BLOOMBERG_BBG_YELLOW_KEY] != DBNull.Value && !string.IsNullOrWhiteSpace(row[BloombergSapiConstants.CONST_BLOOMBERG_BBG_YELLOW_KEY].ToString()))
                {
                    rootData.BBGYellowKey = row[BloombergSapiConstants.CONST_BLOOMBERG_BBG_YELLOW_KEY].ToString();
                }
                if (row.Table.Columns.Contains(BloombergSapiConstants.CONST_BLOOMBERG_BBG_ROOT) && row[BloombergSapiConstants.CONST_BLOOMBERG_BBG_ROOT] != DBNull.Value && !string.IsNullOrWhiteSpace(row[BloombergSapiConstants.CONST_BLOOMBERG_BBG_ROOT].ToString()))
                {
                    rootData.BBGRoot = row[BloombergSapiConstants.CONST_BLOOMBERG_BBG_ROOT].ToString();
                }

                // Update Dynamic UDAs
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-8908
                Dictionary<string, DynamicUDA> _dynamicUDAcache = UDADataCache.GetInstance.GetDynamicUDAList();
                foreach (string uda in _dynamicUDAcache.Keys)
                {
                    if (row.Table.Columns.Contains(uda) && row[uda].ToString() != null)
                    {
                        if (rootData.DynamicUDA.ContainsKey(uda))
                            rootData.DynamicUDA[uda] = row[uda].ToString();
                        else
                            rootData.DynamicUDA.Add(uda, row[uda].ToString());
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
            return rootData;
        }

        //public string GetCutOffTime(string rootSymbol, string exchange)
        //{
        //    if (_FutRootSymbolCachedData.ContainsKey(rootSymbol))
        //    {
        //        if (_FutRootSymbolCachedData[rootSymbol].Count > 1)
        //        {
        //            foreach (FutureRootData rootData in _FutRootSymbolCachedData[rootSymbol])
        //            {
        //                if (rootData.Exchange.Equals(exchange))
        //                {
        //                    return rootData.CutoffTime;
        //                }
        //            }
        //            return string.Empty;
        //        }
        //        else
        //            return _FutRootSymbolCachedData[rootSymbol][0].CutoffTime;
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        /// <summary>
        /// Updating Uda data with name object by UDA data IDs 
        /// </summary>
        /// <param name="secMasterobj"></param>
        internal void UpdateUDADataWithName(SecMasterBaseObj secMasterobj)
        {

            try
            {
                if (secMasterobj.SymbolUDAData != null)
                {
                    UDAData udaSymbolData = secMasterobj.SymbolUDAData;


                    udaSymbolData.UDAAsset = UDADataCache.GetInstance.GetUDATextFromID(udaSymbolData.AssetID, SecMasterConstants.CONST_UDAAsset);
                    udaSymbolData.UDACountry = UDADataCache.GetInstance.GetUDATextFromID(udaSymbolData.CountryID, SecMasterConstants.CONST_UDACountry);
                    udaSymbolData.UDASector = UDADataCache.GetInstance.GetUDATextFromID(udaSymbolData.SectorID, SecMasterConstants.CONST_UDASector);
                    udaSymbolData.UDASecurityType = UDADataCache.GetInstance.GetUDATextFromID(udaSymbolData.SecurityTypeID, SecMasterConstants.CONST_UDASecurityType);
                    udaSymbolData.UDASubSector = UDADataCache.GetInstance.GetUDATextFromID(udaSymbolData.SubSectorID, SecMasterConstants.CONST_UDASubSector);
                    secMasterobj.SymbolUDAData = udaSymbolData;
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

        //internal Dictionary<string, ISecMasterBase> GetSecMasterDataDict(ApplicationConstants.SymbologyCodes symbologyCode)
        //{
        //    Dictionary<string, ISecMasterBase> symbologyData = new Dictionary<string, ISecMasterBase>();
        //    try
        //    {
        //        if (_secMasterAllSymbology.ContainsKey((int)symbologyCode))
        //        {
        //            SecMasterSymbologyData smSymbologyData = _secMasterAllSymbology[(int)symbologyCode];
        //            Dictionary<string, List<ISecMasterBase>> smDataDict = smSymbologyData.GetAllDataDict();

        //            foreach (KeyValuePair<string, List<ISecMasterBase>> smDataObj in smDataDict)
        //            {
        //                if (smDataObj.Value.Count >= 1)
        //                {
        //                    if (!symbologyData.ContainsKey(smDataObj.Value[0].TickerSymbol))
        //                    {
        //                        symbologyData.Add(smDataObj.Value[0].TickerSymbol, smDataObj.Value[0]);
        //                    }
        //                }
        //                else
        //                {

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return symbologyData;
        //}

        /// <summary>
        /// get future root data by ticker symbol
        /// </summary>
        /// <param name="TickerSymbol"></param>
        /// <returns></returns>
        internal FutureRootData GetFutSymbolRootdata(String TickerSymbol)
        {
            FutureRootData rootDataRequested = null;
            try
            {
                string[] str = TickerSymbol.Split(' ');
                string[] exchange = TickerSymbol.Split('-');

                if (str.Length > 1)
                {
                    string rootSymbol = str[0];
                    if (_FutRootSymbolCachedData.ContainsKey(rootSymbol))
                    {
                        foreach (FutureRootData rootData in _FutRootSymbolCachedData[rootSymbol])
                        {
                            //when exchange defined in ticker
                            if (exchange.Length > 1 && rootData.Exchange.Equals(exchange[1]))
                            {
                                rootDataRequested = rootData;
                                break;
                            }
                            //when exchange not defined in ticker ...eg US underlying
                            else if (exchange.Length == 1 && string.IsNullOrEmpty(rootData.Exchange))
                            {
                                rootDataRequested = rootData;
                                break;
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
            return rootDataRequested;
        }

        /// <summary>
        /// Gets Future Root by BBG Root.
        /// </summary>
        /// <param name="bbgRoot"></param>
        /// <returns></returns>
        internal string GetFutureRootByBBGRoot(string bbgRoot)
        {
            string futureRoot = string.Empty;
            try
            {
                foreach (KeyValuePair<string, List<FutureRootData>> kvpRootData in _FutRootSymbolCachedData)
                {
                    futureRoot = kvpRootData.Value.Where(a => a.BBGRoot == bbgRoot).Select(b => b.RootSymbol).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(futureRoot))
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
            return futureRoot;
        }

        /// <summary>
        /// Updates cache after renaming master values for dynamic udas
        /// </summary>
        /// <param name="udaTag">The dynamic UDA Name</param>
        /// <param name="oldMasterValues">list of old master values</param>
        /// <param name="newMasterValues">list of new master values</param>
        /// <returns></returns>
        internal SecMasterbaseList UpdateDynamicUDAMasterValueInSMCache(string udaTag, SerializableDictionary<string, string> oldMasterValues, SerializableDictionary<string, string> newMasterValues)
        {
            SecMasterbaseList updatedSecMasterCacheData = new SecMasterbaseList();
            try
            {
                List<SecMasterBaseObj> secMasterCacheData = new List<SecMasterBaseObj>();

                int tickerCode = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
                if (_secMasterAllSymbology.ContainsKey(tickerCode))
                {
                    SecMasterSymbologyData secMasterSymbologyData = _secMasterAllSymbology[tickerCode];
                    secMasterCacheData = secMasterSymbologyData.GetAllData();
                }
                foreach (SecMasterBaseObj secMasterbaseObj in secMasterCacheData)
                {
                    if (secMasterbaseObj.DynamicUDA.ContainsKey(udaTag))
                    {
                        foreach (string key in newMasterValues.Keys)
                        {
                            if (oldMasterValues.ContainsKey(key) && newMasterValues[key] != oldMasterValues[key])
                            {
                                if (secMasterbaseObj.DynamicUDA[udaTag].ToString() == oldMasterValues[key].ToString())
                                    secMasterbaseObj.DynamicUDA[udaTag] = newMasterValues[key];
                            }
                        }
                    }
                    //add in cache
                    SecMasterDataCache.GetInstance.AddValues(secMasterbaseObj, DateTime.UtcNow);
                    updatedSecMasterCacheData.Add(secMasterbaseObj);
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
            return updatedSecMasterCacheData;
        }

        ///// <summary>
        ///// publish default value change on pm
        ///// </summary>
        ///// <param name="tag">dynamic uda tag name</param>
        ///// <param name="oldValue">old default value of tag</param>
        ///// <param name="newValue">new default value of tag</param>
        ///// <returns>updated list of sec master cache</returns>
        //internal SecMasterbaseList GetUpdatedSMCacheWithDynamicUDADefaultValue(string tag, string oldValue, string newValue)
        //{
        //    SecMasterbaseList updatedSecMasterCacheData = new SecMasterbaseList();
        //    try
        //    {
        //        List<SecMasterBaseObj> secMasterCacheData = new List<SecMasterBaseObj>();

        //        int tickerCode = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
        //        if (_secMasterAllSymbology.ContainsKey(tickerCode))
        //        {
        //            SecMasterSymbologyData secMasterSymbologyData = _secMasterAllSymbology[tickerCode];
        //            secMasterCacheData = secMasterSymbologyData.GetAllData();
        //        }
        //        foreach (SecMasterBaseObj obj in secMasterCacheData)
        //        {
        //            SecMasterBaseObj secMasterbaseObj = DeepCopyHelper.Clone(obj);
        //            if (!secMasterbaseObj.DynamicUDA.ContainsKey(tag))
        //            {
        //                secMasterbaseObj.DynamicUDA.Add(tag, newValue);
        //                updatedSecMasterCacheData.Add(secMasterbaseObj);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return updatedSecMasterCacheData;
        //}

        /// <summary>
        /// Add Tag with value in secMaster Object
        /// </summary>
        /// <param name="tag">UDA tag name</param>
        /// <param name="value">UDA value</param>
        /// <returns>SecMasterBaseObj list</returns>
        internal SecMasterbaseList UpdateDynamicUDADefaultValueInSMCache(string tag, string value)
        {
            SecMasterbaseList updatedSecMasterCacheData = new SecMasterbaseList();
            try
            {
                List<SecMasterBaseObj> secMasterCacheData = new List<SecMasterBaseObj>();

                int tickerCode = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
                if (_secMasterAllSymbology.ContainsKey(tickerCode))
                {
                    SecMasterSymbologyData secMasterSymbologyData = _secMasterAllSymbology[tickerCode];
                    secMasterCacheData = secMasterSymbologyData.GetAllData();
                }
                foreach (SecMasterBaseObj secMasterbaseObj in secMasterCacheData)
                {
                    if (!secMasterbaseObj.DynamicUDA.ContainsKey(tag))
                    {
                        secMasterbaseObj.DynamicUDA.Add(tag, value);
                        //add in cache
                        SecMasterDataCache.GetInstance.AddValues(secMasterbaseObj, DateTime.UtcNow);
                        updatedSecMasterCacheData.Add(secMasterbaseObj);
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
            return updatedSecMasterCacheData;
        }
    }

    // class SecMasterDataObject
    //{
    //    public List<string> SymbologyMapping = new List<string>();
    //    public SecMasterBaseObj SecMasterObj = null; 
    //}

    class SecMasterSymbologyData : IDisposable
    {
        //https://jira.nirvanasolutions.com:8443/browse/PRANA-24323
        private static readonly object _lockerObjectForNewList = new object();
        private Dictionary<string, List<SecMasterBaseObj>> _SecMasterData = new Dictionary<string, List<SecMasterBaseObj>>();
        public void Add(string symbolInDiffSymbology, DateTime date, SecMasterBaseObj secMasterBaseObj)
        {

            try
            {
                lock (_lockerObjectForNewList)
                {
                    string key = SecMasterDataCache.GetInstance.GetKey(symbolInDiffSymbology, date);
                    if (!_SecMasterData.ContainsKey(key))
                    {
                        List<SecMasterBaseObj> list = new List<SecMasterBaseObj>();
                        list.Add(secMasterBaseObj);
                        _SecMasterData.Add(key, list);
                    }
                    else
                    {
                        List<SecMasterBaseObj> list = _SecMasterData[key];
                        if (secMasterBaseObj != null && secMasterBaseObj.SourceOfData != SecMasterConstants.SecMasterSourceOfData.ESignal &&
                            secMasterBaseObj.SourceOfData != SecMasterConstants.SecMasterSourceOfData.FactSet &&
                            secMasterBaseObj.SourceOfData != SecMasterConstants.SecMasterSourceOfData.ACTIV &&
                            list[0].AUECID == secMasterBaseObj.AUECID &&
                            list[0].Symbol_PK == secMasterBaseObj.Symbol_PK &&
                            secMasterBaseObj.AUECID != 0)
                        {
                            List<SecMasterBaseObj> newlist = new List<SecMasterBaseObj>();
                            if (secMasterBaseObj != null)
                            {
                                newlist.Add(secMasterBaseObj);
                                _SecMasterData[key] = newlist;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error While adding in SecMasterdataCache , Problem=" + ex.Message);
            }
        }

        public void Remove(string symbolInDiffSymbology, DateTime date, SecMasterBaseObj secMasterBaseObj)
        {

            try
            {
                lock (_lockerObjectForNewList)
                {
                    string key = SecMasterDataCache.GetInstance.GetKey(symbolInDiffSymbology, date);
                    if (_SecMasterData.ContainsKey(key))
                    {
                        List<SecMasterBaseObj> list = _SecMasterData[key];
                        if (secMasterBaseObj != null && secMasterBaseObj.SourceOfData != SecMasterConstants.SecMasterSourceOfData.ESignal &&
                            secMasterBaseObj.SourceOfData != SecMasterConstants.SecMasterSourceOfData.FactSet &&
                            secMasterBaseObj.SourceOfData != SecMasterConstants.SecMasterSourceOfData.ACTIV &&
                            list[0].Symbol_PK == secMasterBaseObj.Symbol_PK &&
                            secMasterBaseObj.AUECID != 0)
                        {
                            _SecMasterData.Remove(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error While removing from SecMasterdataCache , Problem=" + ex.Message);
            }
        }

        public List<SecMasterBaseObj> GetAllData()
        {
            List<SecMasterBaseObj> newList = new List<SecMasterBaseObj>();
            try
            {
                lock (_lockerObjectForNewList)
                {
                    foreach (KeyValuePair<string, List<SecMasterBaseObj>> keyValue in _SecMasterData)
                    {
                        newList.AddRange(keyValue.Value);
                    }
                }
                //(IEnumerable<SecMasterBaseObj>)  _SecMasterData.Values);
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

            return newList;

            // (IEnumerable<SecMasterBaseObj>) _SecMasterData.Values);
        }

        public List<SecMasterBaseObj> GetData(string key)
        {
            if (_SecMasterData.ContainsKey(key))
            {

                return _SecMasterData[key];

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update Security master Cache On RootData Update
        /// </summary>
        /// <param name="futureRootData"></param>
        /// <returns></returns>
        public SecMasterbaseList UpdateSMInCacheOnRootDataUpdate(FutureRootData futureRootData)
        {
            SecMasterbaseList listSecMasterBaseObj = new SecMasterbaseList();

            foreach (KeyValuePair<string, List<SecMasterBaseObj>> secMastObjDict in _SecMasterData)
            {
                foreach (SecMasterBaseObj secMasterObj in secMastObjDict.Value)
                {
                    //Divya : 02042013 - There can be two same root symbols traded on different exchanges. To handle this, we have a we have an exchange sufix,
                    //we break up the ticker symbol to update the multipliers and proxy symbol according to the exchanges
                    //Eg : C H2 and C H2-ENC can have different multipliers. Thus we keep, exchange suffix in proxy root data and update for exchange speicific 
                    //symbol. This exchanges suffix is not exchange code but the suffix in the e-signal symbol.

                    //We have to update the future root data for Futures as well as future options. 
                    if (secMasterObj.AssetCategory == AssetCategory.Future || secMasterObj.AssetCategory == AssetCategory.FutureOption)
                    {
                        if (secMasterObj.TickerSymbol.Remove(secMasterObj.TickerSymbol.IndexOf(' ')).Equals(futureRootData.RootSymbol))
                        {
                            //string proxySymbol = string.Empty;
                            string[] ticker = secMasterObj.TickerSymbol.Split(' ');
                            //if (futureRootData.ProxyRootSymbol != string.Empty && ticker.Length > 0)
                            //{
                            //    proxySymbol = futureRootData.ProxyRootSymbol + " " + ticker[1];
                            //}

                            // Update UDAs, http://jira.nirvanasolutions.com:8080/browse/PRANA-8908
                            SecMasterBaseObj udaUpdateObj = secMasterObj;
                            if (secMasterObj.TickerSymbol.Contains("-"))
                            {
                                string[] exchange = secMasterObj.TickerSymbol.Split('-');
                                if (exchange.Length > 1)
                                {
                                    if (futureRootData.Exchange.Equals(exchange[1]))
                                        UpdateUDAsInCache(futureRootData, ref udaUpdateObj);
                                }
                            }
                            else
                            {
                                if (futureRootData.Exchange.Equals(string.Empty))
                                    UpdateUDAsInCache(futureRootData, ref udaUpdateObj);
                            }


                            if (secMasterObj.AssetCategory == AssetCategory.Future)
                            {
                                SecMasterFutObj SecMasterFutObject = secMasterObj as SecMasterFutObj;
                                if (secMasterObj.TickerSymbol.Contains("-"))
                                {
                                    string[] exchange = secMasterObj.TickerSymbol.Split('-');
                                    if (exchange.Length > 1)
                                    {
                                        if (futureRootData.Exchange.Equals(exchange[1]))
                                        {

                                            SecMasterFutObject.Multiplier = futureRootData.Multiplier;
                                            SecMasterFutObject.CutOffTime = futureRootData.CutoffTime;
                                            SecMasterFutObject.ProxySymbol = futureRootData.ProxyRootSymbol;
                                            SecMasterFutObject.IsCurrencyFuture = futureRootData.IsCurrencyFuture;
                                            // listSecMasterBaseObj.Add(secMasterObj);
                                        }
                                    }
                                }
                                else
                                {
                                    if (futureRootData.Exchange.Equals(string.Empty))
                                    {
                                        // SecMasterFutObject = secMasterObj as SecMasterFutObj;
                                        SecMasterFutObject.Multiplier = futureRootData.Multiplier;
                                        SecMasterFutObject.CutOffTime = futureRootData.CutoffTime;
                                        SecMasterFutObject.ProxySymbol = futureRootData.ProxyRootSymbol;
                                        SecMasterFutObject.IsCurrencyFuture = futureRootData.IsCurrencyFuture;


                                    }
                                }
                            }

                            if (secMasterObj.AssetCategory == AssetCategory.FutureOption)
                            {
                                SecMasterOptObj SecMasterOptObject = secMasterObj as SecMasterOptObj;
                                if (secMasterObj.TickerSymbol.Contains("-"))
                                {
                                    string[] exchange = secMasterObj.TickerSymbol.Split('-');
                                    if (exchange.Length > 1)
                                    {
                                        if (futureRootData.Exchange.Equals(exchange[1]))
                                        {
                                            SecMasterOptObject.Multiplier = futureRootData.Multiplier;
                                            SecMasterOptObject.ProxySymbol = futureRootData.ProxyRootSymbol;
                                            SecMasterOptObject.IsCurrencyFuture = futureRootData.IsCurrencyFuture;
                                        }
                                    }
                                }
                                else
                                {
                                    if (futureRootData.Exchange.Equals(string.Empty))
                                    {
                                        SecMasterOptObject.Multiplier = futureRootData.Multiplier;
                                        SecMasterOptObject.ProxySymbol = futureRootData.ProxyRootSymbol;
                                        SecMasterOptObject.IsCurrencyFuture = futureRootData.IsCurrencyFuture;
                                    }
                                }
                            }

                            listSecMasterBaseObj.Add(secMasterObj);
                        }
                    }
                }
            }


            return listSecMasterBaseObj;
        }

        /// <summary>
        /// Update UDAs in cache from future root data
        /// </summary>
        /// <param name="futureRootData">FutureRootData object</param>
        /// <param name="secMasterObj">SecMasterBaseObj object</param>
        private void UpdateUDAsInCache(FutureRootData futureRootData, ref SecMasterBaseObj secMasterObj)
        {
            try
            {
                //update UDA fields  of security (future and future options) -omshiv, Nov 2013

                //secMasterObj.SymbolUDAData.AssetID = futureRootData.UDAAssetClassID;
                secMasterObj.SymbolUDAData.CountryID = futureRootData.UDACountryID;
                secMasterObj.SymbolUDAData.SectorID = futureRootData.UDASectorID;
                secMasterObj.SymbolUDAData.SubSectorID = futureRootData.UDASubSectorID;
                secMasterObj.SymbolUDAData.SecurityTypeID = futureRootData.UDASecurityTypeID;
                SecMasterDataCache.GetInstance.UpdateUDADataWithName(secMasterObj);

                //Update Dynamic UDAs
                Dictionary<string, DynamicUDA> _dynamicUDAcache = UDADataCache.GetInstance.GetDynamicUDAList();
                foreach (string uda in _dynamicUDAcache.Keys)
                {
                    if (futureRootData.DynamicUDA.ContainsKey(uda) && futureRootData.DynamicUDA[uda].ToString() != _dynamicUDAcache[uda].DefaultValue && futureRootData.DynamicUDA[uda].ToString() != "Undefined")
                    {
                        if (secMasterObj.DynamicUDA.ContainsKey(uda))
                            secMasterObj.DynamicUDA[uda] = futureRootData.DynamicUDA[uda];
                        else
                            secMasterObj.DynamicUDA.Add(uda, futureRootData.DynamicUDA[uda]);
                    }
                }
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


        /// <summary>
        /// Get all sec data of specific symbology as dict
        /// </summary>
        internal Dictionary<string, List<SecMasterBaseObj>> GetAllDataDict()
        {

            return _SecMasterData;

        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_SecMasterData != null)
                {
                    _SecMasterData.Clear();
                    _SecMasterData = null;
                }
            }
        }
    }


    class SecMasterReqDataCached
    {
        private string _requestedSymbol;

        public string RequestedSymbol
        {
            get { return _requestedSymbol; }
            set { _requestedSymbol = value; }
        }

        private string _underlyingSymbol;

        public string UnderlyingSymbol
        {
            get { return _underlyingSymbol; }
            set { _underlyingSymbol = value; }
        }

        private DateTime _expirationDate;
        public DateTime ExpirationDate
        {
            get
            {
                return _expirationDate;
            }

            set
            {
                _expirationDate = value;
            }
        }
        private int _requestedSymbology = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
        public int RequestedSymbology
        {
            get
            {
                return _requestedSymbology;
            }

            set { _requestedSymbology = value; }
        }
    }
}
