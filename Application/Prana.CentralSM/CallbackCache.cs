using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Prana.CentralSM
{
    internal sealed class CallbackCache
    {
        #region singleton
        private static volatile CallbackCache instance;
        private static object syncRoot = new Object();

        private CallbackCache() { }

        public static CallbackCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CallbackCache();
                    }
                }
                return instance;
            }
        }
        #endregion singleton

        /// <summary>
        /// Used to store the symboldatarows requested symbols. SymbologyCode,symbol,SymbolDataRow
        /// </summary>
        ConcurrentDictionary<int, ConcurrentDictionary<string, List<SymbolDataRow>>> _securitySymbolRowMapping = new ConcurrentDictionary<int, ConcurrentDictionary<string, List<SymbolDataRow>>>();

        /// <summary>
        /// Used to store the hashcode of SymbolDataRow and ICentralSMSecurityCallback for requested
        /// </summary>
        ConcurrentDictionary<int, Tuple<ICentralSMSecurityCallback, string>> _symbolRowSecurityCallbackCache = new ConcurrentDictionary<int, Tuple<ICentralSMSecurityCallback, string>>();

        /// <summary>
        /// Used to store ICentralSMSecurityCallback for pricing related requests. requestID, Tuple Callback,TradeServerName
        /// </summary>
        ConcurrentDictionary<string,Tuple<ICentralSMSecurityCallback,string>> _pricingCallbackCache = new ConcurrentDictionary<string,Tuple<ICentralSMSecurityCallback,string>>();

        public void AddInSecuritySubscriber(SecMasterRequestObj secMasterReqObj, ICentralSMSecurityCallback secCallback,string clientName)
        {
            try
            {
                foreach (SymbolDataRow symDataRow in secMasterReqObj.SymbolDataRowCollection)
                {
                    int symbCode = 0;
                    //first add all the symbols in the row in _securitySymbolRowMapping
                    foreach (string symbol in symDataRow.SymbolData)
                    {
                        if (!String.IsNullOrWhiteSpace(symbol))
                        {
                            AddRequestToSymbolMapping(symDataRow, symbCode, symbol);
                        }
                        symbCode++;
                    }

                    if (!String.IsNullOrWhiteSpace(symDataRow.BBGID))
                    {
                        AddRequestToSymbolMapping(symDataRow, symbCode, symDataRow.BBGID);
                    }
                    //Add the mapping for SymbolDataRow.GetHashCode() and ICentralSMSecurityCallback
                    if (!_symbolRowSecurityCallbackCache.TryAdd(symDataRow.GetHashCode(),new Tuple<ICentralSMSecurityCallback,string>(secCallback,clientName)))
                    {
                        try
                        {
                            throw new Exception("Error while adding symbolDataRow in callback cache in CentralSM Security validation for the symbol " + symDataRow.PrimarySymbol + ". The hashcode for symbolDataRow already exists");
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

        //Add new symbol Request Symbol Mapping 
        private void AddRequestToSymbolMapping(SymbolDataRow symDataRow, int symbCode, string symbol)
        {
            try
            {
                if (_securitySymbolRowMapping.ContainsKey(symbCode))
                {
                    if (_securitySymbolRowMapping[symbCode].ContainsKey(symbol))
                    {
                        _securitySymbolRowMapping[symbCode][symbol].Add(symDataRow);
                    }
                    else
                    {
                        _securitySymbolRowMapping[symbCode].TryAdd(symbol, new List<SymbolDataRow>() { symDataRow });
                    }
                }
                else
                {
                    ConcurrentDictionary<string, List<SymbolDataRow>> tempDict = new ConcurrentDictionary<string, List<SymbolDataRow>>();
                    tempDict.TryAdd(symbol, new List<SymbolDataRow>() { symDataRow });
                    _securitySymbolRowMapping.TryAdd(symbCode, tempDict);
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

        public HashSet<ICentralSMSecurityCallback> GetAndRemoveSecuritySubscribersList(SecMasterBaseObj secMasterBaseObj)
        {
            int code = 0;
            HashSet<ICentralSMSecurityCallback> hashOfCallbacks = new HashSet<ICentralSMSecurityCallback>();
            HashSet<SymbolDataRow> hashOfRows = new HashSet<SymbolDataRow>();
            try
            {
                foreach (string sym in secMasterBaseObj.SymbologyMapping)
                {
                    if (!String.IsNullOrWhiteSpace(sym))
                    {
                        if (_securitySymbolRowMapping.ContainsKey(code))
                        {
                            List<SymbolDataRow> tempListOfRow;
                        if (_securitySymbolRowMapping[code].TryRemove(sym.Trim(), out tempListOfRow))
                            {
                                tempListOfRow.ForEach(i => hashOfRows.Add(i));
                            }
                        }
                    }
                    code++;
                }

                //OMshiv - currently, I am adding BBGID request in as symbology request
                if (!string.IsNullOrWhiteSpace(secMasterBaseObj.BBGID) && _securitySymbolRowMapping.ContainsKey(code))
                {
                    List<SymbolDataRow> tempListOfRow;

                    if (_securitySymbolRowMapping[code].TryRemove(secMasterBaseObj.BBGID, out tempListOfRow))
                    {
                        tempListOfRow.ForEach(i => hashOfRows.Add(i));
                    }
                }

                foreach (SymbolDataRow symRow in hashOfRows)
                {
                    Tuple<ICentralSMSecurityCallback, string> tempCallback;
                    if (_symbolRowSecurityCallbackCache.TryRemove(symRow.GetHashCode(), out tempCallback))
                    {
                        hashOfCallbacks.Add(tempCallback.Item1);
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
            return hashOfCallbacks;
        }

        internal void AddInPricingSubscriber(string requestID,string clientName, ICentralSMSecurityCallback securityCallback)
        {
            try
            {
                _pricingCallbackCache.AddOrUpdate(requestID, new Tuple<ICentralSMSecurityCallback, string>(securityCallback, clientName), (key, oldValue) => new Tuple<ICentralSMSecurityCallback, string>(securityCallback, clientName));
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

        internal Tuple<ICentralSMSecurityCallback,string> GetAndRemoveHistPricingSubscribersList(string requestID)
        {
            try
            {
                if (_pricingCallbackCache.ContainsKey(requestID))
                {
                    Tuple<ICentralSMSecurityCallback,string> tempTuple;
                    if (_pricingCallbackCache.TryRemove(requestID, out tempTuple))
                        return tempTuple;
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
            return null;
        }

        internal void ClientDisconnected(string clientName)
        {
            try
            {
                IEnumerable<KeyValuePair<string, Tuple<ICentralSMSecurityCallback, string>>> toBeDeletedPricing = _pricingCallbackCache.Where(x => String.Compare(clientName, x.Value.Item2,true) == 0);
                Tuple<ICentralSMSecurityCallback, string> tempCallback;
                bool datadeleted = false;
                foreach (KeyValuePair<string, Tuple<ICentralSMSecurityCallback, string>> kvp in toBeDeletedPricing)
                {
                    _pricingCallbackCache.TryRemove(kvp.Key, out tempCallback);
                    datadeleted = true;
                }
                IEnumerable<KeyValuePair<int, Tuple<ICentralSMSecurityCallback, string>>> toBeDeletedSecmaster = _symbolRowSecurityCallbackCache.Where(x => String.Compare(clientName, x.Value.Item2, true) == 0);
                foreach (KeyValuePair<int, Tuple<ICentralSMSecurityCallback, string>> kvp in toBeDeletedSecmaster)
                {
                    _symbolRowSecurityCallbackCache.TryRemove(kvp.Key, out tempCallback);
                    datadeleted = true;
                }
                if (datadeleted)
                {
                    Logger.Write("Calback cache cleared for client "+clientName+". Please request the data again if needed", ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
                    InformationReporter.GetInstance.Write("Calback cache cleared for client " + clientName + ". Please request the data again if needed");
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

        ConcurrentDictionary<string, ICentralSMSecurityCallback> _saveSecurityCompletedCallbackCache = new ConcurrentDictionary<string, ICentralSMSecurityCallback>();

        public void AddSaveSecurityCompletedCallBack(string requestId,ICentralSMSecurityCallback securityCallback)
        {
            try
            {
                _saveSecurityCompletedCallbackCache.AddOrUpdate(requestId, securityCallback, (key, oldValue) => securityCallback);
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

        public ICentralSMSecurityCallback GetSaveSecurityCompletedCallBack(string requestId)
        {
            ICentralSMSecurityCallback serviceCallback = null;
            try
            {
                _saveSecurityCompletedCallbackCache.TryRemove(requestId, out serviceCallback);
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
            return serviceCallback;
        }

    }
}