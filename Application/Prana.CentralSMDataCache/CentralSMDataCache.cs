using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
namespace Prana.CentralSMDataCache
{
    /// <summary>
    /// Cache class for central SM service. May contain unused copied code from SecMasterCache
    /// </summary>
    public sealed class CentralSMDataCache
    {

        #region singleton
        private static volatile CentralSMDataCache instance;
        private static object syncRoot = new Object();

        public static CentralSMDataCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CentralSMDataCache();
                    }
                }
                return instance;
            }
        }
        #endregion

        private CentralSMDataCache()
        {
            //_subscriberSnapShotHash = new Dictionary<int, Dictionary<string, SecMasterReqDataCached>>();
            CleanCentralCache();
        }

        /// <summary>
        /// Central cache for symbols 
        /// </summary>
        private ConcurrentDictionary<int, ConcurrentDictionary<string, SecMasterBaseObj>> _secMasterAllSymbologyCache = new ConcurrentDictionary<int, ConcurrentDictionary<string, SecMasterBaseObj>>();

        static Dictionary<string, List<FutureRootData>> _FutRootSymbolCachedData = new Dictionary<string, List<FutureRootData>>();

        public Dictionary<string, List<FutureRootData>> FutRootSymbolCachedData
        { 
            get { return _FutRootSymbolCachedData; } 
            set {  _FutRootSymbolCachedData= value; }
        }

        HashSet<string> _bbgidsValidated = new HashSet<string>();

        /// <summary>
        /// Cleans the central cache for all symbology data
        /// </summary>
        public void CleanCentralCache()
        {
            try
            {
                _secMasterAllSymbologyCache = new ConcurrentDictionary<int, ConcurrentDictionary<string, SecMasterBaseObj>>();
                Array symbologyArray = Enum.GetValues(typeof(ApplicationConstants.SymbologyCodes));

                foreach (object symbologyCode in symbologyArray)
                {
                    int value = (int)symbologyCode;
                    ConcurrentDictionary<string, SecMasterBaseObj> secMasterSymbologyData = new ConcurrentDictionary<string, SecMasterBaseObj>();
                    if (!_secMasterAllSymbologyCache.ContainsKey(value))
                    {
                        _secMasterAllSymbologyCache.TryAdd(value, secMasterSymbologyData);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public bool CheckBbgidAlreadyValidated(string bbGid)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(bbGid))
                    return false;
                if (_bbgidsValidated.Contains(bbGid))
                    return true;
                else
                {
                    _bbgidsValidated.Add(bbGid);
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Get SecMaster object from SymbolData object (e-signal data)  
        /// </summary>
        /// <param name="level1Data"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public SecMasterBaseObj GetSecMasterObjFromCache(SymbolData level1Data, DateTime date)
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return secMasterObj;
        }

        /// <summary>
        /// Add SM data to cache with Secmaster obj list
        /// </summary>
        /// <param name="secMasterObjCollection"></param>
        /// <param name="date"></param>
        public void AddValuesToSecurityCache(List<SecMasterBaseObj> secMasterObjCollection)
        {
            try
            {
                foreach (SecMasterBaseObj secMasterObj in secMasterObjCollection)
                {
                    int code = 0;
                    foreach (string symbolInDiffSymbology in secMasterObj.SymbologyMapping)
                    {
                        if (symbolInDiffSymbology != string.Empty)
                        {
                            if (_secMasterAllSymbologyCache.ContainsKey(code))
                            {
                                _secMasterAllSymbologyCache[code].TryAdd(symbolInDiffSymbology, secMasterObj);
                            }
                        }
                        code++;
                    }
                    //it should be before adding to cache and only on creat/ update sec
                    // SetExtraFromCache(secMasterObj);

                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Add SM data to cache with single SM object
        /// </summary>
        /// <param name="secMasterObj"></param>
        /// <param name="date"></param>
        public void AddValuesToSecurityCache(SecMasterBaseObj secMasterObj)
        {
            try
            {
                int code = 0;
                foreach (string symbolInDiffSymbology in secMasterObj.SymbologyMapping)
                {
                    if (symbolInDiffSymbology != string.Empty)
                    {
                        _secMasterAllSymbologyCache[code].TryAdd(symbolInDiffSymbology, secMasterObj);
                    }
                    code++;
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public List<SecMasterBaseObj> GetSecMasterDataFromCache(SecMasterRequestObj secMasterRequestObj)
        {
            List<SecMasterBaseObj> newlist = new List<SecMasterBaseObj>();
            try
            {
                foreach (SymbolDataRow symbolDataRow in secMasterRequestObj.SymbolDataRowCollection)
                {
                    if (!String.IsNullOrWhiteSpace(symbolDataRow.PrimarySymbol))
                    {
                    int code = symbolDataRow.PrimarySymbology;
                    //string key = GetKey(symbolDataRow.PrimarySymbol, date);

                    SecMasterBaseObj list;
                    _secMasterAllSymbologyCache[code].TryGetValue(symbolDataRow.PrimarySymbol, out list);
                    if (list != null)
                    {
                        SecMasterBaseObj secMasterResponse = (SecMasterBaseObj)list.Clone();
                        secMasterResponse.RequestedSymbology = symbolDataRow.PrimarySymbology;
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

                    if (newlist.Count == 0 && !String.IsNullOrWhiteSpace(symbolDataRow.BBGID))
                    {
                        //Currently, I am assuming the cache is updated with ticker symbol in case of any symbology search.
                        int code = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;

                        ConcurrentDictionary<string, SecMasterBaseObj> list = _secMasterAllSymbologyCache[code];

                        if (list != null && list.Count > 0)
                        {
                            foreach (SecMasterBaseObj secMasterObj in list.Values)
                            {
                                if (secMasterObj.BBGID.Trim().Equals(symbolDataRow.BBGID))
                                {
                                    SecMasterBaseObj secMasterResponse = (SecMasterBaseObj)secMasterObj.Clone();
                                    secMasterResponse.RequestedSymbology = symbolDataRow.PrimarySymbology;
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return newlist;
        }

        public void UpdateUDADataWithName(SecMasterBaseObj secMasterobj)
        {
            //TODO update UDA Names hare from Ids- osmhiv
            //throw new NotImplementedException();
        }

        public SecMasterbaseList UpdateFutureSymbolsFromRootSymbol(System.Data.DataTable dt)
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

                    // SecMasterBaseObj list;
                    //_secMasterAllSymbologyCache[tickerCode].TryGetValue((symbolDataRow.PrimarySymbol, out list);
                    //if (list != null)
                    //{
                    //}

                    if (_secMasterAllSymbologyCache.ContainsKey(tickerCode))
                    {
                        ConcurrentDictionary<string, SecMasterBaseObj> secMasterSymbologyData = _secMasterAllSymbologyCache[tickerCode];
                        listSecMasterBaseObj = UpdateSMInCacheOnRootDataUpdate(futureRootData, secMasterSymbologyData);
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            return updatedFutureSymbols;
        }

        /// <summary>
        /// Update Security master Cache On RootData Update
        /// </summary>
        /// <param name="futureRootData"></param>
        /// <returns></returns>
        public SecMasterbaseList UpdateSMInCacheOnRootDataUpdate(FutureRootData futureRootData, ConcurrentDictionary<string, SecMasterBaseObj> secMasterSymbologyData)
        {
            SecMasterbaseList listSecMasterBaseObj = new SecMasterbaseList();

            try
            {
                foreach (string symbol in secMasterSymbologyData.Keys)
                {
                    SecMasterBaseObj secMasterObj = secMasterSymbologyData[symbol];
                    if (secMasterObj != null)
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
                                string proxySymbol = string.Empty;
                                string[] ticker = secMasterObj.TickerSymbol.Split(' ');
                                if (futureRootData.ProxyRootSymbol != string.Empty && ticker.Length > 0)
                                {
                                    proxySymbol = futureRootData.ProxyRootSymbol + " " + ticker[1];
                                }

                                //update UDA fields  of security (future and future options) -omshiv, Nov 2013

                                secMasterObj.SymbolUDAData.AssetID = futureRootData.UDAAssetClassID;
                                secMasterObj.SymbolUDAData.CountryID = futureRootData.UDACountryID;
                                secMasterObj.SymbolUDAData.SectorID = futureRootData.UDASectorID;
                                secMasterObj.SymbolUDAData.SubSectorID = futureRootData.UDASubSectorID;
                                secMasterObj.SymbolUDAData.SecurityTypeID = futureRootData.UDASecurityTypeID;
                                UpdateUDADataWithName(secMasterObj);

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


                                        }
                                    }
                                }

                                listSecMasterBaseObj.Add(secMasterObj);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }


            return listSecMasterBaseObj;
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
                if (row.Table.Columns.Contains("CutOffTime") && row["CutOffTime"].ToString() != "")
                {
                    rootData.CutoffTime = Convert.ToDateTime(row["CutOffTime"].ToString()).ToString("HH:mm:ss");
                }
                else
                {
                    rootData.CutoffTime = DateTime.Now.ToString();
                }


                //UDA fileds, added by Om, Nov 2013
                rootData.UDAAssetClassID = int.Parse(row["UDAAssetClassID"].ToString());
                rootData.UDASecurityTypeID = int.Parse(row["UDASecurityTypeID"].ToString());
                rootData.UDASectorID = int.Parse(row["UDASectorID"].ToString());
                rootData.UDASubSectorID = int.Parse(row["UDASubSectorID"].ToString());
                rootData.UDACountryID = int.Parse(row["UDACountryID"].ToString());

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return rootData;
        }
    }

}
