using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Prana.ExposurePnlCache
{
    /// <summary>
    /// Make it generic for other types and thread safe.
    /// </summary>

    [Serializable()]
    public class ExposurePnlCacheBindableDictionary : IDisposable
    {
        private ConcurrentDictionary<string, ExposurePnlCacheItem> _dictionary = new ConcurrentDictionary<string, ExposurePnlCacheItem>();
        ExposurePnlCacheItemList _bindingList = new ExposurePnlCacheItemList();
        int _IsTracingEnabledForPublishedTaxlots = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsTracingEnabledForEXPNLData").ToString());
        string[] _expnlDataTracingEnabledUsers = (ConfigurationHelper.Instance.GetAppSettingValueByKey("EXPNLDataTracingEnabledForUsers").ToString()).Split(',');
        object _locker = new object();
        string _lastUpdatedID = string.Empty;
        private List<PropertyInfo> _dynamicColumnPropertyList = new List<PropertyInfo>();
        public event EventHandler<IndexEventArgs> RowsToUpdateColor;
        public event EventHandler PMDataBinded;
        //Applied the optimization based on the following link
        //http://www.infragistics.com/community/forums/t/15306.aspx

        //But BeginUpdate/EndUpdate was causing fliker, so commented the code.Following is the Relivent link
        //http://devcenter.infragistics.com/Support/KnowledgeBaseArticle.Aspx?ArticleID=2013

        public List<PropertyInfo> DynamicColumnPropertyList
        {
            get { return _dynamicColumnPropertyList; }
            set
            {
                _dynamicColumnPropertyList = value;
            }
        }

        List<string> _IDsToRemove = new List<string>();

        public ExposurePnlCacheItemList Values
        {
            get
            {
                return _bindingList;
            }
        }

        public void SuspendListChangeEvent()
        {
            try
            {
                if (_bindingList != null)
                    _bindingList.RaiseListChangedEvents = false;
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

        public void ResumeListChangeEvent()
        {
            try
            {
                if (_bindingList != null)
                    _bindingList.RaiseListChangedEvents = true;
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

        public void Clear()
        {
            lock (_locker)
            {
                ResumeListChangeEvent();
                _dictionary.Clear();
                _bindingList.Clear();
            }
        }
        private string _rowColorBasis = "0";  //orderSide
        public string RowColorBasis
        {
            get
            {
                return _rowColorBasis;
            }
            set
            {
                _rowColorBasis = value;
            }
        }

        public ConcurrentDictionary<string, ExposurePnlCacheItem> ExposurePnlCacheItemDictionary
        {
            get { return _dictionary; }
        }

        private string RemoveInBindingList(StringDictionary newItems)
        {
            try
            {
                lock (_locker)
                {
                    _IDsToRemove.Clear();
                    // If no trades are coming from expnl server (may be because of no trades in DB), then clean the existing binded items 
                    if (newItems.Count == 0)
                    {
                        ReloadPM();
                    }
                    foreach (KeyValuePair<string, ExposurePnlCacheItem> existingItem in _dictionary)
                    {
                        if (!(newItems.ContainsKey(existingItem.Key)))
                        {
                            _IDsToRemove.Add(existingItem.Key);
                        }
                    }

                    for (int i = 0; i < _IDsToRemove.Count; i++)
                    {
                        string idToRemove = _IDsToRemove[i];
                        if (_dictionary.ContainsKey(idToRemove))
                        {
                            ExposurePnlCacheItem epnlCacheItem = _dictionary[idToRemove];
                            if (_bindingList.Contains(epnlCacheItem))
                            {
                                _bindingList.Remove(epnlCacheItem);
                            }
                            _dictionary.TryRemove(idToRemove, out epnlCacheItem);
                        }
                    }
                    ResumeListChangeEvent();

                    // added in case there is only one trade on PM & that trade is busted than 
                    // nothing should bw visible on PM.
                    if (_IDsToRemove.Count != 0 && _dictionary.Count == 0 && _bindingList.Count > 0)
                    {
                        _dictionary.Clear();
                        _bindingList.Clear();
                    }

                    if (_dictionary.Count > 0 && _bindingList.Count > 0)
                    {
                        return _bindingList[0].ID;
                    }
                    else
                    {
                        return string.Empty;
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
            return string.Empty;
        }

        /// <summary>
        ///   /// If no trades are coming from expnl server (may be because of no trades in DB), then clean the existing binded items 
        /// </summary>
        private void ReloadPM()
        {
            _dictionary.Clear();
            _bindingList.Clear();
            //as dictionary is cleared, so property has changed is not being fired for any ExposurePnlCacheItem. 
            // Thus added a blank key and item to raise property chnaged event.
            string key = string.Empty;
            ExposurePnlCacheItem expnlItem = new ExposurePnlCacheItem();
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary[key] = expnlItem;
                _dictionary[key].PropertyHasChanged();
            }
            expnlItem = null;
        }

        public void AddOrUpdateList(string lastUpdatedID, List<string> idsToupdateColor)
        {
            try
            {
                FormMarshaller formMarshaller = UIThreadMarshaller.GetFormMarshallerByKey(UIThreadMarshaller.PM_FORM);
                if (formMarshaller != null && formMarshaller.Form != null && !formMarshaller.Form.Disposing && formMarshaller.Form.IsHandleCreated)
                {
                    if (formMarshaller.InvokeRequired)
                    {
                        DataUpdated exposurePNLCacheDictHandler = new DataUpdated(RaisepropertyChanged);
                        formMarshaller.Invoke(exposurePNLCacheDictHandler, new object[] { lastUpdatedID, idsToupdateColor });
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

        private void RaisepropertyChanged(string lastAddedID, List<string> IDs)
        {
            try
            {
                if (IDs != null && IDs.Count > 0)
                {
                    lastAddedID = IDs[0];
                }
                if (!String.IsNullOrEmpty(_lastUpdatedID))
                {
                    if (_dictionary.ContainsKey(_lastUpdatedID))
                    {
                        _dictionary[_lastUpdatedID].PropertyHasChanged();
                    }
                }
                else if (!String.IsNullOrEmpty(lastAddedID))
                {
                    _dictionary[lastAddedID].PropertyHasChanged();
                }

                if (_IDsToRemove != null && _IDsToRemove.Count > 0 && !String.IsNullOrEmpty(_IDsToRemove[0]))
                {
                    if (PMDataBinded != null)
                    {
                        PMDataBinded(null, null);
                    }
                }

                foreach (string id in IDs)
                {
                    ExposurePnlCacheItem item = null;
                    if (_dictionary.ContainsKey(id))
                    {
                        item = _dictionary[id];
                    }
                    if (item != null)
                    {
                        int index = _bindingList.IndexOf(item);
                        IndexEventArgs arg = new IndexEventArgs(index);
                        if (arg.Index != -1)
                        {
                            if (RowsToUpdateColor != null)
                            {
                                RowsToUpdateColor(this, arg);
                            }
                        }
                    }
                }
                if (_IsTracingEnabledForPublishedTaxlots == 2 && _expnlDataTracingEnabledUsers.Contains(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString()))
                {
                    _logMessage.Clear();
                    _logMessage.Append("PM Logging: ExposurePnlCacheBindableDictionary.RaisepropertyChanged");
                    _logMessage.Append("DateTime(Machine) = " + DateTime.Now);
                    _logMessage.Append(", IDs.Count = " + IDs.Count);
                    _logMessage.Append(", _lastUpdatedID = " + _lastUpdatedID.ToString());
                    _logMessage.Append(", lastAddedID = " + lastAddedID.ToString());
                    _logMessage.Append(", _IDsToRemove.Count = " + _IDsToRemove.Count.ToString());
                    _logMessage.Append(", UserID = " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString());
                    Logger.LoggerWrite(_logMessage, LoggingConstants.CATEGORY_GENERAL);
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

        #region New Code for Performance
        StringBuilder _logMessage = new StringBuilder();
        public void UpdateCurrentdataFromIncomingOrders(List<ExposurePnlCacheItem> incomingOrders, ref ExposurePnlCacheManager.StatusMonitor currentStatus, List<string> pmCurrentViewGroupedColumns, ref bool isGroupingColumnValueChanged)
        {
            try
            {
                lock (_locker)
                {
                    if (_dictionary != null)
                    {
                        this.SuspendListChangeEvent();
                        bool isOrderChanged = false;
                        bool isItemInBindingList = false;
                        bool isItemInDictionary = false;
                        foreach (ExposurePnlCacheItem newItem in incomingOrders)
                        {
                            isItemInBindingList = false;

                            if (_dictionary != null)
                                isItemInDictionary = _dictionary.ContainsKey(newItem.ID);

                            if (isItemInDictionary)
                            {
                                //Divya : 28062012: isOrderChanged is specific to _pmRowColorbasis. If rowColorbasis is Day pnl then isOrderChanged will be true only 
                                // if daypnL switches sign as only the we have to change the color or if rowColorbasis is order side, then if order side changes then 
                                //only isOrderChanged is true

                                if (newItem.HasBeenSentToUser == 0)
                                {
                                    if (_dictionary != null && _bindingList != null)
                                    {
                                        ExposurePnlCacheItem epnlCacheItem = _dictionary[newItem.ID];
                                        isItemInBindingList = DoExistInBindingList(_bindingList, epnlCacheItem.ID);
                                        if (isItemInBindingList)
                                        {
                                            _bindingList.Remove(epnlCacheItem);
                                        }
                                        _dictionary[newItem.ID] = newItem;
                                        _bindingList.Add(newItem);
                                    }
                                }
                                else
                                {
                                    if (!newItem.IsStaleData && _dictionary != null)
                                    {
                                        isOrderChanged = _dictionary[newItem.ID].UpdateDynamicDataFromOrder(ref _dynamicColumnPropertyList, newItem, Convert.ToInt16(_rowColorBasis), pmCurrentViewGroupedColumns, ref isGroupingColumnValueChanged);
                                        if (isOrderChanged && !currentStatus.latestUpdatedIDs.Contains(newItem.ID))
                                        {
                                            currentStatus.latestUpdatedIDs.Add(newItem.ID);
                                        }
                                    }
                                }
                                //Checking if static data is missed for existing order, so the it can be fetched in next cycle
                                if (_dictionary != null && string.IsNullOrEmpty(_dictionary[newItem.ID].Symbol) && !currentStatus.lsItemIDsToRequestCompleteData.Contains(newItem.ID))
                                    currentStatus.lsItemIDsToRequestCompleteData.Add(newItem.ID);
                            }
                            else
                            {
                                //Checking if static data is missed for new order, so the it can be fetched in next cycle
                                if (string.IsNullOrEmpty(newItem.Symbol) && !currentStatus.lsItemIDsToRequestCompleteData.Contains(newItem.ID))
                                    currentStatus.lsItemIDsToRequestCompleteData.Add(newItem.ID);

                                if (_dictionary != null && _bindingList != null)
                                {
                                    _dictionary[newItem.ID] = newItem;
                                    _bindingList.Add(newItem);
                                }
                            }

                            if (_IsTracingEnabledForPublishedTaxlots > 0 && _expnlDataTracingEnabledUsers.Contains(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString()))
                            {
                                _logMessage.Clear();
                                _logMessage.Append("PM Logging: ExposurePnlCacheBindableDictionary.UpdateCurrentdataFromIncomingOrders Data");
                                _logMessage.Append("DateTime(Machine) = " + DateTime.Now);
                                _logMessage.Append(" Symbol = " + newItem.Symbol);
                                _logMessage.Append(" _dictionary Symbol = " + _dictionary[newItem.ID].Symbol);
                                _logMessage.Append(", HasBeenSentToUser = " + newItem.HasBeenSentToUser);
                                _logMessage.Append(", ID = " + newItem.ID);
                                _logMessage.Append(", IsStaleData = " + newItem.IsStaleData);
                                _logMessage.Append(", isItemInDictionary = " + isItemInDictionary);
                                _logMessage.Append(", isItemInBindingList = " + isItemInBindingList.ToString());
                                _logMessage.Append(", isOrderChanged = " + isOrderChanged.ToString());
                                _logMessage.Append(", isGroupingColumnValueChanged = " + isGroupingColumnValueChanged.ToString());
                                _logMessage.Append(", AUEC ID = " + newItem.AUECID);
                                _logMessage.Append(", Asset = " + newItem.Asset);
                                _logMessage.Append(", Underlying ID = " + newItem.UnderlyingID);
                                _logMessage.Append(", Exchange = " + newItem.ExchangeID);
                                _logMessage.Append(", Currency ID = " + newItem.CurrencyID);
                                _logMessage.Append(", Order Side = " + newItem.OrderSideTagValue);
                                _logMessage.Append(", Account ID = " + newItem.Level1ID);
                                _logMessage.Append(", Strategy ID = " + newItem.Level2ID);
                                _logMessage.Append(", Quantity = " + newItem.Quantity);
                                _logMessage.Append(", Trade Date = " + newItem.TradeDate);
                                _logMessage.Append(", Settlement Date = " + newItem.SettlementDate);
                                _logMessage.Append(", Start Trade Date = " + newItem.StartTradeDate);
                                _logMessage.Append(", LastUpdatedUTC = " + newItem.LastUpdatedUTC);
                                _logMessage.Append(", ExDividend Date = " + newItem.ExDividendDate);
                                _logMessage.Append(", Expiration Date = " + newItem.ExpirationDate);
                                _logMessage.Append(", SelectedFeedPrice = " + newItem.SelectedFeedPrice);
                                _logMessage.Append(", Username = " + newItem.UserName);
                                _logMessage.Append(", DateTime (UTC) = " + DateTime.UtcNow);
                                _logMessage.Append(", UserID = " + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString());

                                if (_IsTracingEnabledForPublishedTaxlots == 1)
                                {
                                    //Logger.HandleException(new Exception(_logMessage.ToString()), LoggingConstants.POLICY_LOGONLY);
                                    Logger.LoggerWrite(_logMessage.ToString(), LoggingConstants.CATEGORY_GENERAL);
                                }
                                else if (_IsTracingEnabledForPublishedTaxlots == 2)
                                {

                                    Logger.LoggerWrite(_logMessage.ToString(), LoggingConstants.CATEGORY_GENERAL);
                                    Logger.LoggerWrite("Serialzed Text [ExposurePnlCacheBindableDictionary.UpdateCurrentdataFromIncomingOrders] : " + newItem.ToString(), LoggingConstants.CATEGORY_GENERAL);
                                    //Logger.HandleException(new Exception(_logMessage.ToString()), LoggingConstants.POLICY_LOGONLY);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                throw;
            }
        }

        private bool DoExistInBindingList(ExposurePnlCacheItemList bindingList, string ID)
        {
            try
            {
                foreach (ExposurePnlCacheItem exposurePnlCacheItem in bindingList)
                {
                    if (exposurePnlCacheItem.ID == ID)
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

        public void UpdateUIFromLatestOrderIDCollection(StringDictionary newIDDictionary, List<string> latestUpdatedIds)
        {
            try
            {
                lock (_locker)
                {
                    string lastUpdatedIDs = RemoveInBindingList(newIDDictionary);
                    ResumeListChangeEvent();
                    AddOrUpdateList(lastUpdatedIDs, latestUpdatedIds);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        public delegate void DataUpdated(string lastUpdatesId, List<string> idsToUpdateColor);

        public class IndexEventArgs : EventArgs
        {
            private int index = 0;
            public IndexEventArgs()
            { }
            public IndexEventArgs(int indexRow)
            {
                index = indexRow;
            }
            public int Index
            {
                get { return index; }
                set { index = value; }
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (RowsToUpdateColor != null)
                        foreach (var d in RowsToUpdateColor.GetInvocationList())
                            RowsToUpdateColor -= d as EventHandler<IndexEventArgs>;
                    if (_bindingList != null)
                    {
                        _bindingList.Clear();
                        _bindingList = null;
                    }

                    if (_dynamicColumnPropertyList != null)
                    {
                        _dynamicColumnPropertyList.Clear();
                        _dynamicColumnPropertyList = null;
                    }

                    if (_dictionary != null)
                    {
                        _dictionary.Clear();
                    }

                    _lastUpdatedID = null;

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion


        public IList<ExposurePnlCacheItem> getPositionForAccountsbySymbol(string symbol)
        {
            IList<ExposurePnlCacheItem> rtn = null;
            try
            {
                rtn = (from item in _dictionary.Values where item.Symbol == symbol select item).ToList();
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
            return rtn;
        }

        public IList<string> getSymbolsForAccounts()
        {
            IList<string> rtn = null;
            try
            {
                rtn = (from item in _dictionary.Values select item.Symbol).Distinct<string>().ToList();
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
            return rtn;
        }

        public Dictionary<string, string> getBloombergSymbolToTickerForAccounts()
        {
            Dictionary<string, string> rtn = null;
            try
            {
                rtn = (from item in _dictionary.Values where (item.BloombergSymbol != null && item.BloombergSymbol != string.Empty) select item).ToList().Select(x => new { x.BloombergSymbol, x.Symbol }).Distinct().ToDictionary(x => x.BloombergSymbol, x => x.Symbol);
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
            return rtn;
        }

        public Dictionary<int, decimal> GetPositionForSymbolAndAccounts(string symbol, List<int> accountIds)
        {
            Dictionary<int, decimal> accountWisePositions = new Dictionary<int, decimal>();
            try
            {
                foreach (int accountId in accountIds)
                {
                    IEnumerable<double> items = from item in _dictionary.Values where item.Symbol == symbol && item.Level1ID == accountId let position = item.Quantity select position;
                    accountWisePositions.Add(accountId, Convert.ToDecimal(items.Sum()));
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
            return accountWisePositions;
        }

        public Dictionary<int, decimal> GetGrossExposureForSymbolAndAccounts(string symbol, List<int> accountIds)
        {
            Dictionary<int, decimal> accountWiseGrossExposure = new Dictionary<int, decimal>();
            try
            {
                foreach (int accountId in accountIds)
                {
                    IEnumerable<double> items = from item in _dictionary.Values where item.Symbol == symbol && item.Level1ID == accountId let netExposure = item.NetExposureInBaseCurrency select netExposure;
                    accountWiseGrossExposure.Add(accountId, Convert.ToDecimal(Math.Abs(items.Sum())));
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
            return accountWiseGrossExposure;
        }

        public Dictionary<int, decimal> GetAccountNAV(List<int> accountIds)
        {
            Dictionary<int, decimal> dictAccountNAV = new Dictionary<int, decimal>();
            try
            {
                foreach (int accountId in accountIds)
                {
                    IEnumerable<double> items = from item in _dictionary.Values where item.Level1ID == accountId let accountNAV = item.NavTouch select accountNAV;
                    if (items.Count() > 0)
                        dictAccountNAV.Add(accountId, Convert.ToDecimal(items.First()));
                    else
                        dictAccountNAV.Add(accountId, 0);
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
            return dictAccountNAV;
        }

        public decimal GetPXSelectedFeedForSymbol(string symbol)
        {
            double pxSelectedFeed = 0;
            try
            {
                IEnumerable<double> items = from item in _dictionary.Values where item.Symbol == symbol let selectedFeedPrice = item.SelectedFeedPrice select selectedFeedPrice;
                if (items.Count() > 0)
                    pxSelectedFeed = Convert.ToDouble(items.First());
                else
                    pxSelectedFeed = 0;
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
            return Decimal.Parse(pxSelectedFeed.ToString());
        }

        public decimal GetPXSelectedFeedBaseForSymbol(string symbol)
        {
            double pxSelectedFeedBase = 0;
            try
            {
                IEnumerable<double> items = from item in _dictionary.Values where item.Symbol == symbol let selectedFeedPriceInBaseCurrency = item.SelectedFeedPriceInBaseCurrency select selectedFeedPriceInBaseCurrency;
                if (items.Count() > 0)
                    pxSelectedFeedBase = Convert.ToDouble(items.First());
                else
                    pxSelectedFeedBase = 0;
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
            return Decimal.Parse(pxSelectedFeedBase.ToString());
        }
    }
}
