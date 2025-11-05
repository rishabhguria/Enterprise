using Prana.Allocation.Core.DataAccess;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;

namespace Prana.Allocation.Core.CacheStore
{
    internal sealed class FixedPreferenceCache
    {
        #region Members

        /// <summary>
        /// The singelton instance
        /// </summary>
        private static FixedPreferenceCache _singeltonInstance = new FixedPreferenceCache();

        /// <summary>
        /// The locker
        /// </summary>
        private static readonly object _locker = new object();

        /// <summary>
        /// The allocation scheme cache locker
        /// </summary>
        private readonly object _schemeCacheLocker = new object();

        /// <summary>
        /// The _allocation schemes
        /// </summary>
        private Dictionary<string, AllocationFixedPreference> _allocationSchemes;


        /// <summary>
        /// The account symbol position dictionary
        /// </summary>
        Dictionary<string, List<DataRow>> _accountSymbolPositionDict = new Dictionary<string, List<DataRow>>();

        /// <summary>
        /// The symbol and namewise account allocation scheme
        /// </summary>
        Dictionary<string, Dictionary<string, List<DataRow>>> _symbolAndNamewiseAccountAllocationScheme = new Dictionary<string, Dictionary<string, List<DataRow>>>();

        /// <summary>
        /// The is first time symbol wise allocation scheme cache
        /// </summary>
        bool _isFirstTimeSymbolWiseAllocationSchemeCache = true;

        /// <summary>
        /// The alloc scheme key
        /// </summary>
        AllocationSchemeKey _allocSchemeKey = AllocationSchemeKey.Symbol;

        /// <summary>
        /// The currency list
        /// </summary>
        List<string> _currencyList = new List<string>();

        /// <summary>
        /// The pbeb mappping dictionary
        /// </summary>
        Dictionary<string, List<int>> _PBEBMapppingDict = new Dictionary<string, List<int>>();

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        internal static FixedPreferenceCache Instance
        {
            get
            {
                if (_singeltonInstance == null)
                {
                    lock (_locker)
                    {
                        if (_singeltonInstance == null)
                            _singeltonInstance = new FixedPreferenceCache();
                    }
                }
                return _singeltonInstance;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="FixedPreferenceCache" /> class from being created.
        /// </summary>
        private FixedPreferenceCache()
        {
            try
            {
                // Do cache object initialization which should be done before Initialize() method
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal void Initialize()
        {
            try
            {
                lock (_schemeCacheLocker)
                {
                    _allocationSchemes = AllocationPrefDataManager.GetAllocationSchemes();
                }
                GetNonUsableCurrenciesForSwap();
                GetPBEBMapping();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Clears the cache master fund based positions.
        /// </summary>
        internal void ClearCacheMasterFundBasedPositions()
        {
            try
            {
                lock (_schemeCacheLocker)
                {
                    //Clear cache masterfund based postions chache - omshiv, Jan 2014
                    _accountSymbolPositionDict.Clear();
                }
                _isFirstTimeSymbolWiseAllocationSchemeCache = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Get data table from symbol wise scheme dict
        /// </summary>
        /// <param name="dataRowDict">The data row dictionary.</param>
        /// <returns></returns>
        private DataTable CreatDataTableFromDataRows(Dictionary<string, List<DataRow>> dataRowDict)
        {
            DataTable dt = new DataTable();
            try
            {
                bool isFirstTime = true;
                dt.TableName = "PositionMaster";

                foreach (KeyValuePair<string, List<DataRow>> dictItem in dataRowDict)
                {
                    foreach (DataRow row in dictItem.Value)
                    {
                        if (isFirstTime)
                        {
                            isFirstTime = false;
                            foreach (DataColumn col in row.Table.Columns)
                            {
                                dt.Columns.Add(col.ColumnName, col.DataType, col.Expression);
                            }
                        }
                        dt.Rows.Add(row.ItemArray);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dt;
        }

        /// <summary>
        /// Deletes the allocation scheme.
        /// </summary>
        /// <param name="allocationSchemeID">The allocation scheme identifier.</param>
        /// <param name="schemeName">Name of the scheme.</param>
        /// <returns></returns>
        internal int DeleteAllocationScheme(int allocationSchemeID, string schemeName)
        {
            int affectedRow = 0;
            try
            {
                lock (_schemeCacheLocker)
                {
                    affectedRow = AllocationPrefDataManager.DeleteAllocationScheme(allocationSchemeID, schemeName);
                    if (affectedRow > 0)
                    {
                        if (_allocationSchemes.ContainsKey(schemeName))
                        {
                            _allocationSchemes.Remove(schemeName);

                            _symbolAndNamewiseAccountAllocationScheme = new Dictionary<string, Dictionary<string, List<DataRow>>>();

                            _isFirstTimeSymbolWiseAllocationSchemeCache = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return affectedRow;
        }

        /// <summary>
        /// Gets all allocation scheme names.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<int, string> GetAllAllocationSchemes()
        {
            Dictionary<int, string> allocSchemes = null;
            try
            {
                lock (_schemeCacheLocker)
                {
                    allocSchemes = _allocationSchemes.Where(p => p.Value.IsPrefVisible).ToDictionary(p => p.Value.SchemeID, p => p.Value.SchemeName);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocSchemes;
        }

        /// <summary>
        /// Gets the allocation schemes by source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        internal Dictionary<int, string> GetAllocationSchemesBySource(FixedPreferenceCreationSource source)
        {
            Dictionary<int, string> allocSchemes = null;
            try
            {
                lock (_schemeCacheLocker)
                {
                    allocSchemes = _allocationSchemes.Where(p => p.Value.CreationSource == source).ToDictionary(p => p.Value.SchemeID, p => p.Value.SchemeName);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocSchemes;
        }

        /// <summary>
        /// Gets the name of the allocation scheme by.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <returns></returns>
        internal AllocationFixedPreference GetAllocationSchemeByName(string allocationSchemeName)
        {
            AllocationFixedPreference allocationScheme = null;
            try
            {
                lock (_schemeCacheLocker)
                {
                    if (_allocationSchemes[allocationSchemeName].Scheme.Equals(string.Empty))
                        allocationScheme = AllocationPrefDataManager.GetAllocationSchemeByName(allocationSchemeName);
                    else
                        allocationScheme = _allocationSchemes[allocationSchemeName];
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationScheme;
        }

        /// <summary>
        /// Gets the allocation scheme key.
        /// </summary>
        /// <returns></returns>
        internal AllocationSchemeKey GetAllocationSchemeKey()
        {
            AllocationSchemeKey allocKey = AllocationSchemeKey.Symbol;
            try
            {
                allocKey = _allocSchemeKey;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocKey;
        }

        /// <summary>
        /// Gets the allocation scheme name by identifier.
        /// </summary>
        /// <param name="allocationSchemeID">The allocation scheme identifier.</param>
        /// <returns></returns>
        internal string GetAllocationSchemeNameByID(int allocationSchemeID)
        {
            string prefName = string.Empty;
            try
            {
                lock (_schemeCacheLocker)
                {
                    string myKey = _allocationSchemes.FirstOrDefault(x => x.Value.SchemeID == allocationSchemeID).Key;
                    if (myKey != null && _allocationSchemes.ContainsKey(myKey))
                        prefName = myKey;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return prefName;
        }

        /// <summary>
        /// Gets the allocation scheme id by name.
        /// </summary>
        /// <param name="allocationSchemeName">The allocation scheme Name.</param>
        /// <returns></returns>
        internal int GetAllocationSchemeIdByName(string allocationSchemeName)
        {
            int prefId = 0;
            try
            {
                lock (_schemeCacheLocker)
                {
                    return _allocationSchemes.FirstOrDefault(x => x.Key == allocationSchemeName).Value == null ? 0 : _allocationSchemes.FirstOrDefault(x => x.Key == allocationSchemeName).Value.SchemeID;

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return prefId;
        }

        /// <summary>
        /// this method is used to get the allocation scheme from database and fill in the cache
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        internal void GetAllocationSchemeSymbolWise(string allocationSchemeName)
        {
            try
            {
                lock (_schemeCacheLocker)
                {
                    // if allocation scheme does not exist in the cache
                    if (!_allocationSchemes.ContainsKey(allocationSchemeName) || _allocationSchemes[allocationSchemeName].Scheme.Equals(string.Empty))
                    {
                        // get Allocation Scheme from db by Name
                        AllocationFixedPreference allocationScheme = GetAllocationSchemeByName(allocationSchemeName);
                        if (!_allocationSchemes.ContainsKey(allocationSchemeName))
                            _allocationSchemes.Add(allocationSchemeName, allocationScheme);
                        else
                            _allocationSchemes[allocationSchemeName].Scheme = allocationScheme.Scheme;
                        _symbolAndNamewiseAccountAllocationScheme = new Dictionary<string, Dictionary<string, List<DataRow>>>();
                        _isFirstTimeSymbolWiseAllocationSchemeCache = true;

                    }

                    if (_allocationSchemes.ContainsKey(allocationSchemeName))
                    {
                        string allocationSchemeXML = _allocationSchemes[allocationSchemeName].Scheme;
                        if ((!string.IsNullOrEmpty(allocationSchemeXML) && !_symbolAndNamewiseAccountAllocationScheme.ContainsKey(allocationSchemeName)) || _isFirstTimeSymbolWiseAllocationSchemeCache)
                        {
                            _symbolAndNamewiseAccountAllocationScheme = new Dictionary<string, Dictionary<string, List<DataRow>>>();
                            DataSet ds = new DataSet();
                            StringReader sr = new StringReader(allocationSchemeXML);
                            ds.ReadXml(sr);


                            //here we assume that Allocation Scheme Key will be same for one scheme, can not ne more than one
                            string AllocSchemeKeyName = ds.Tables[0].Rows[0]["AllocationSchemeKey"].ToString();

                            _allocSchemeKey = (AllocationSchemeKey)Enum.Parse(typeof(AllocationSchemeKey), AllocSchemeKeyName);

                            Dictionary<string, List<DataRow>> symbolDict = GetSymbolWiseSchemeDict(ds.Tables[0]);

                            _symbolAndNamewiseAccountAllocationScheme.Add(allocationSchemeName, symbolDict);

                            //modified by omshiv, get copy of symbol wise scheme and craete a cache for Master account allocation.
                            // as data row can not serialized so, here creating the copy of dataset and then getting symbol wise dict
                            _accountSymbolPositionDict = GetSymbolWiseSchemeDict(ds.Copy().Tables[0]);
                            _isFirstTimeSymbolWiseAllocationSchemeCache = false;
                        }
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
        /// Gets the currency list for allocation scheme.
        /// </summary>
        /// <returns></returns>
        internal List<string> GetCurrencyListForAllocationScheme()
        {
            List<string> currencyList = new List<string>();
            try
            {
                currencyList = _currencyList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return currencyList;
        }

        /// <summary>
        /// Gets the non usable currencies for swap.
        /// </summary>
        private void GetNonUsableCurrenciesForSwap()
        {
            try
            {
                NameValueCollection currency = ConfigurationHelper.Instance.LoadSectionBySectionName(ConfigurationHelper.SECTION_NonUsableCurrencyForSwap);
                string commaSeparatedCurrency = string.Empty;
                if (currency != null && currency.Count > 0)
                {
                    commaSeparatedCurrency = currency["Currency"];
                    if (!string.IsNullOrEmpty(commaSeparatedCurrency))
                    {
                        string[] currencies = commaSeparatedCurrency.Split(',');
                        if (currencies.Length > 0)
                        {
                            for (int i = 0; i < currencies.Length; i++)
                            {
                                _currencyList.Add(currencies[i].ToUpperInvariant());
                            }
                        }
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
        /// Returns Allocation Scheme Prime Broker and Execution Broker mapping and currencies from config file
        /// </summary>
        private void GetPBEBMapping()
        {
            try
            {
                NameValueCollection pbEbMaping = ConfigurationHelper.Instance.LoadSectionBySectionName(ConfigurationHelper.SECTION_PBEBMappingForAllocation);
                if (pbEbMaping != null && pbEbMaping.Count > 0)
                {
                    foreach (string key in pbEbMaping.Keys)
                    {
                        string EBIDList = pbEbMaping[key];
                        if (!_PBEBMapppingDict.ContainsKey(key) && !string.IsNullOrEmpty(EBIDList))
                        {
                            List<int> lstEBID = new List<int>();
                            string[] EBID = EBIDList.Split(',');
                            if (EBID.Length > 0)
                            {
                                for (int i = 0; i < EBID.Length; i++)
                                {
                                    lstEBID.Add(Convert.ToInt32(EBID[i]));
                                }
                            }
                            _PBEBMapppingDict.Add(key.ToUpperInvariant(), lstEBID);
                        }
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
        /// Pbebs the mapping.
        /// </summary>
        /// <param name="pbname">The pbname.</param>
        /// <param name="ebID">The eb identifier.</param>
        /// <returns></returns>
        internal bool PBEBMapping(string pbname, int ebID)
        {
            try
            {
                if (_PBEBMapppingDict.ContainsKey(pbname))
                {
                    List<int> EBIDList = _PBEBMapppingDict[pbname];
                    if (EBIDList != null && EBIDList.Count > 0 && EBIDList.Contains(ebID))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Gets the positions from symbol and namewise account allocation scheme.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="allocSchemeKey">The alloc scheme key.</param>
        /// <param name="lstRows">The LST rows.</param>
        /// <returns></returns>
        internal string GetPositionsFromSymbolAndNamewiseAccountAllocationScheme(string allocationSchemeName, string allocSchemeKey, ref List<DataRow> lstRows)
        {
            string errMessage = string.Empty;
            try
            {
                lock (_schemeCacheLocker)
                {
                    //if postions for master account allocation is null or empty then get it from _symbolAndNamewiseAccountAllocationScheme
                    if (_accountSymbolPositionDict == null || _accountSymbolPositionDict.Count == 0)
                    {
                        if (_symbolAndNamewiseAccountAllocationScheme.ContainsKey(allocationSchemeName))
                        {
                            Dictionary<string, List<DataRow>> scheme = _symbolAndNamewiseAccountAllocationScheme[allocationSchemeName];
                            DataTable ds = CreatDataTableFromDataRows(scheme);
                            _accountSymbolPositionDict = GetSymbolWiseSchemeDict(ds);
                        }
                    }
                    if(_accountSymbolPositionDict == null)
                    {
                        throw new ArgumentNullException(nameof(_accountSymbolPositionDict));
                    }
                    if (_accountSymbolPositionDict.ContainsKey(allocSchemeKey))
                        lstRows = _accountSymbolPositionDict[allocSchemeKey];
                    else
                    {
                        errMessage = "Master account allocation failed.";
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return errMessage;
        }

        /// <summary>
        /// Updates the account wise postion in cache.
        /// </summary>
        /// <param name="group">The group.</param>
        internal void UpdateAccountWisePostionInCache(AllocationGroup group)
        {
            try
            {
                lock (_schemeCacheLocker)
                {
                    List<TaxLot> taxlotList = group.TaxLots;
                    if (_accountSymbolPositionDict.ContainsKey(group.Symbol))
                    {
                        List<DataRow> accountSymbolPostions = _accountSymbolPositionDict[group.Symbol];
                        foreach (TaxLot taxlot in taxlotList)
                        {
                            foreach (DataRow positionRow in accountSymbolPostions)
                            {
                                if (taxlot.Level1ID == Convert.ToInt32(positionRow["FundID"].ToString()))
                                {
                                    double quantityAfteUnAlloc = (Convert.ToDouble(positionRow["Quantity"].ToString()) - taxlot.TaxLotQty);
                                    //subtract cumulative qty instead of total quantity, PRANA-10298
                                    double totalQty = (Convert.ToDouble(positionRow["TotalQty"].ToString()) - group.CumQty);
                                    double percentage = quantityAfteUnAlloc * 100 / totalQty;

                                    positionRow["Percentage"] = percentage;
                                    positionRow["Quantity"] = (int)quantityAfteUnAlloc;
                                    positionRow["TotalQty"] = (int)totalQty;
                                    break;
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
                    throw;
            }
        }

        /// <summary>
        /// Gets the symbol wise dictionary.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <returns></returns>
        internal Dictionary<string, List<DataRow>> GetSymbolWiseDictionary(string allocationSchemeName)
        {
            Dictionary<string, List<DataRow>> symbolwiseDict = new Dictionary<string, List<DataRow>>();
            try
            {
                lock (_schemeCacheLocker)
                {
                    symbolwiseDict = _symbolAndNamewiseAccountAllocationScheme[allocationSchemeName];
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return symbolwiseDict;
        }

        /// <summary>
        /// get symbol wise dict from scheme data table
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        internal Dictionary<string, List<DataRow>> GetSymbolWiseSchemeDict(DataTable dt)
        {
            Dictionary<string, List<DataRow>> symbolDict = new Dictionary<string, List<DataRow>>();
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    string symbol = row["Symbol"].ToString();
                    string ordersidetagvalue = row["orderSideTagValue"].ToString();
                    string key = string.Empty;

                    switch (_allocSchemeKey)
                    {
                        case AllocationSchemeKey.Symbol:
                            key = symbol;
                            break;
                        case AllocationSchemeKey.SymbolSide:
                        case AllocationSchemeKey.PBSymbolSide:
                            key = symbol + Seperators.SEPERATOR_5 + ordersidetagvalue;
                            break;
                        default:
                            break;
                    }
                    if (symbolDict.ContainsKey(key))
                    {
                        List<DataRow> lstRows = symbolDict[key];
                        lstRows.Add(row);
                        symbolDict[key] = lstRows;
                    }
                    else
                    {
                        List<DataRow> lstRows = new List<DataRow>();
                        lstRows.Add(row);
                        symbolDict.Add(key, lstRows);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return symbolDict;
        }

        /// <summary>
        /// Saves the allocation scheme.
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="allocationSchemeDate">The allocation scheme date.</param>
        /// <param name="allocationSchemeXML">The allocation scheme XML.</param>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <returns></returns>
        public int SaveAllocationScheme(AllocationFixedPreference fixedPref)
        {
            int allocationSchemeID = 0;
            try
            {
                lock (_schemeCacheLocker)
                {
                    allocationSchemeID = AllocationPrefDataManager.SaveAllocationScheme(fixedPref);
                    if (allocationSchemeID > 0)
                    {
                        fixedPref.SchemeID = allocationSchemeID;
                        UpdateAllocationSchemeXML(fixedPref);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return allocationSchemeID;
        }

        /// <summary>
        /// Updates Names and XMLs Cache
        /// </summary>
        /// <param name="allocationSchemeName">Name of the allocation scheme.</param>
        /// <param name="allocationSchemeXML">The allocation scheme XML.</param>
        internal void UpdateAllocationSchemeXML(AllocationFixedPreference fixedPref)
        {
            try
            {
                lock (_schemeCacheLocker)
                {
                    if (_allocationSchemes.ContainsKey(fixedPref.SchemeName))
                    {
                        _allocationSchemes[fixedPref.SchemeName] = fixedPref;

                        _isFirstTimeSymbolWiseAllocationSchemeCache = true;

                        _symbolAndNamewiseAccountAllocationScheme = new Dictionary<string, Dictionary<string, List<DataRow>>>();
                    }
                    else
                    {
                        _allocationSchemes.Add(fixedPref.SchemeName, fixedPref);
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

        #endregion Methods
    }
}
