using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.OptionCalculator.Common
{
    /// <summary>
    /// This call is to maintain the proxy symbol requests and publish the proxy Symbol data when new symbols are traded.
    /// If proxy Symbol info is updated from Symbol lookup, even the it is published here. 
    /// </summary>
    public class ProxySymbolHelper : IPublishing, IDisposable
    {
        private static ProxySymbolHelper _singleton = null;
        private static object _locker = new object();
        public static event EventHandler<ProxyDataEventArgs> PlugUnplugProxy;
        public static event EventHandler<ListEventAargs> SecurityUpdateRecieved;
        public static ProxySymbolHelper GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new ProxySymbolHelper();
                    }
                }
            }
            return _singleton;
        }

        DuplexProxyBase<ISubscription> _proxy;
        public void CreateSubscriptionServicesProxy()
        {
            try
            {
                if (_proxy == null)
                {
                    _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);
                    _proxy.Subscribe(Topics.Topic_PricingInputsData, null);
                    _proxy.Subscribe(Topics.Topic_SecurityMaster, null);
                    _proxy.Subscribe(Topics.Topic_ClosingCompleted, null);
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

        public void DisposeSubscriptionServicesProxy()
        {
            try
            {
                if (_proxy != null)
                {
                    try
                    {
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_PricingInputsData);
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_SecurityMaster);
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_ClosingCompleted);
                    }
                    catch
                    {
                    }

                    _proxy.Dispose();
                    _proxy = null;
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

        public List<SymbolInfo> GetSymbolsForProxy(string ProxySymbol)
        {
            List<SymbolInfo> listSymbols = new List<SymbolInfo>();
            try
            {
                listSymbols = OptionModelUserInputCache.GetSymbolsForProxy(ProxySymbol);
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
            return listSymbols;
        }

        public string GetProxySymbol(string symbol)
        {
            try
            {
                UserOptModelInput userOMI = OptionModelUserInputCache.GetUserOMIData(symbol);
                if (userOMI != null)
                {
                    if (userOMI.ProxySymbolUsed)
                        return userOMI.ProxySymbol;
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

        #region IPublishing Members
        public void Publish(Prana.BusinessObjects.MessageData e, string topicName)
        {
            try
            {
                System.Object[] dataList = null;
                switch (topicName)
                {
                    case Topics.Topic_PricingInputsData:
                        dataList = (System.Object[])e.EventData;
                        List<UserOptModelInput> listOMI = new List<UserOptModelInput>();
                        foreach (Object obj in dataList)
                        {
                            UserOptModelInput userOMI = (UserOptModelInput)obj;

                            //This is a special case where we have to plug proxy automatically for every new symbol traded..

                            //Below condition checks if the symbol is newly traded in which case we have to update the cache and also save the proxy 
                            //  info in DB so as to synchronize the info between cache and OMI table. 

                            bool isExistinCache = OptionModelUserInputCache.IsExistDatainCache(userOMI.Symbol);
                            // Kuldeep A.: This was put outside the IF clause because symbol should be added everytime to the list, else OMI cache will not be updated.
                            // Due to this the historical symbols on re-trading didn't get shown on PI as the "IsHistorical" flag value for those was not updated.
                            listOMI.Add(userOMI);
                            if (!isExistinCache)
                            {
                                userOMI.IsDirtyToSave = true;
                            }
                        }
                        if (listOMI.Count > 0)
                        {
                            OptionModelUserInputCache.UpdateCacheFromOMICollection(listOMI);
                        }
                        break;

                    case Topics.Topic_SecurityMaster:
                        dataList = (System.Object[])e.EventData;
                        SecMasterbaseList secMasterObjlist = new SecMasterbaseList();
                        foreach (Object secmasterObj in dataList)
                        {
                            secMasterObjlist.Add((SecMasterBaseObj)secmasterObj);
                        }
                        UpdateSecMasterInfo(secMasterObjlist);

                        //publish security update 
                        if (SecurityUpdateRecieved != null)
                        {
                            ListEventAargs args = new ListEventAargs() { argsObject = secMasterObjlist };
                            SecurityUpdateRecieved(this, args);
                        }
                        break;

                    case Topics.Topic_ClosingCompleted:
                        QueryData queryData = new QueryData();
                        queryData.StoredProcedureName = "P_UpdateOptionModelUserData";
                        queryData.CommandTimeout = 180;
                        queryData.DictionaryDatabaseParameter.Add("@cacheupdationrequired", new DatabaseParameter()
                        {
                            IsOutParameter = false,
                            ParameterName = "@cacheupdationrequired",
                            ParameterType = DbType.Int32,
                            ParameterValue = 1
                        });

                        DataSet ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            lock (_locker)
                            {
                                OptionModelUserInputCache.UpdateCacheFromDataSet(ds);
                            }
                        }
                        break;

                    default:
                        break;
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

        private void UpdateSecMasterInfo(SecMasterbaseList secMasterList)
        {
            List<UserOptModelInput> listUserOMI = new List<UserOptModelInput>();
            foreach (SecMasterBaseObj obj in secMasterList)
            {
                UserOptModelInput userOMI = OptionModelUserInputCache.GetUserOMIData(obj.TickerSymbol);
                if (userOMI != null)
                {
                    if (userOMI.ProxySymbol != obj.ProxySymbol)
                    {
                        if (!string.IsNullOrEmpty(obj.ProxySymbol))
                        {
                            userOMI.ProxySymbol = obj.ProxySymbol;
                            PlugUnplugProxyFromLiveFeed(userOMI);
                        }
                        //if the proxy symbol is blank, then we have to unplug the proxy symbol
                        else
                        {
                            PlugUnplugProxyFromLiveFeed(userOMI);
                        }
                        if (string.IsNullOrEmpty(obj.ProxySymbol))
                        {
                            userOMI.ProxySymbol = string.Empty;
                        }
                    }
                    if (userOMI.SMSharesOutstanding != obj.SharesOutstanding)
                    {
                        if (obj.SharesOutstanding != 0)
                        {
                            userOMI.SMSharesOutstanding = obj.SharesOutstanding;
                            PlugUnplugProxyFromLiveFeed(userOMI);
                        }
                        else
                        {
                            PlugUnplugProxyFromLiveFeed(userOMI);
                        }
                    }
                    userOMI.UnderlyingSymbol = obj.UnderLyingSymbol;
                    userOMI.SecurityDescription = obj.LongName;
                    userOMI.Bloomberg = obj.BloombergSymbol;
                    userOMI.AssetID = obj.AssetID;
                    userOMI.AuecID = obj.AUECID;
                    switch (obj.AssetCategory)
                    {
                        case AssetCategory.Future:
                            SecMasterFutObj futObj = (SecMasterFutObj)obj;
                            userOMI.ExpirationDate = futObj.ExpirationDate;
                            break;

                        case AssetCategory.EquityOption:
                        case AssetCategory.FutureOption:
                            SecMasterOptObj optObject = (SecMasterOptObj)obj;
                            userOMI.PutorCall = (OptionType)optObject.PutOrCall;
                            userOMI.StrikePrice = optObject.StrikePrice;
                            userOMI.IDCOOptionSymbol = optObject.IDCOOptionSymbol;
                            userOMI.ExpirationDate = optObject.ExpirationDate;
                            userOMI.OSIOptionSymbol = optObject.OSIOptionSymbol;
                            break;

                        case AssetCategory.FX:
                            SecMasterFxObj fxObj = (SecMasterFxObj)obj;
                            userOMI.LeadCurrencyID = fxObj.LeadCurrencyID;
                            userOMI.VsCurrencyID = fxObj.VsCurrencyID;
                            break;

                        case AssetCategory.FXForward:
                            SecMasterFXForwardObj forwardObj = (SecMasterFXForwardObj)obj;
                            userOMI.LeadCurrencyID = forwardObj.LeadCurrencyID;
                            userOMI.VsCurrencyID = forwardObj.VsCurrencyID;
                            userOMI.ExpirationDate = forwardObj.ExpirationDate;
                            break;

                        case AssetCategory.FixedIncome:
                        case AssetCategory.ConvertibleBond:
                            SecMasterFixedIncome fixedIncomeobj = (SecMasterFixedIncome)obj;
                            userOMI.ExpirationDate = fixedIncomeobj.MaturityDate;
                            break;

                        case AssetCategory.PrivateEquity:
                        case AssetCategory.CreditDefaultSwap:
                        case AssetCategory.Equity:
                        case AssetCategory.Cash:
                        case AssetCategory.Forex:
                        case AssetCategory.FXOption:
                        case AssetCategory.Indices:
                            break;

                        default:
                            break;
                    }
                    listUserOMI.Add(userOMI);
                }
                if (listUserOMI.Count > 0)
                {
                    OptionModelUserInputCache.UpdateCacheFromOMICollection(listUserOMI);
                }
            }
        }
        public string getReceiverUniqueName()
        {
            return "ProxyHelper";
        }

        #endregion

        /// <summary>
        /// When proxy is updated from OMI, we need to fire an event to updat the information from live feed.
        /// </summary>
        /// <param name="userOMI"></param>
        internal static void PlugUnplugProxyFromLiveFeed(UserOptModelInput userOMI)
        {
            try
            {
                ProxyDataEventArgs arg = new ProxyDataEventArgs();
                arg.Symbol = userOMI.Symbol;
                arg.ProxySymbol = userOMI.ProxySymbol;
                arg.UseProxySymbol = userOMI.ProxySymbolUsed;
                if (PlugUnplugProxy != null)
                {
                    PlugUnplugProxy(null, arg);
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

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Dispose Internal Channel and remove connections from ConnectionManager
        /// </summary>
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
                    _proxy.Dispose();
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
        #endregion

        public static void UpdatePICacheWithCustomSymbols()
        {
            try
            {
                OptionModelUserInputCache.GetCustomDataInformation();
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
