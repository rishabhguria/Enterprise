using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Prana.SecurityMasterNew.BLL
{
    sealed class CentralSMDataCache : IDisposable
    {
        #region singleton
        private static volatile CentralSMDataCache instance;
        private static readonly object syncRoot = new Object();

        private CentralSMDataCache() { }

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
        #endregion singleton

        /// <summary>
        /// Keeps a check that duplicate symbols are not requested to BB while it is processing the symbols.
        /// </summary>
        ConcurrentDictionary<int, HashSet<Tuple<string, DateTime>>> _symbolsRequestedToCentralSM = new ConcurrentDictionary<int, HashSet<Tuple<string, DateTime>>>();

        /// <summary>
        /// Hashset which keeps all the BBGID response received from central SM so that we do not send the symbol for saving again to DB. Needed for requests when made through different symbology
        /// </summary>
        HashSet<string> _bbgidResponseFromCentralSM = new HashSet<string>();

        //Timer _heartbeatTimerToClearCache;

        /// <summary>
        /// Starts the timer which clears the cache in case of timeouts
        /// </summary>
        //void CreateAndStartTimer()
        //{
        //    try
        //    {
        //        //will elapse every 500 millisecond and clear the cache for timed out symbols
        //        _heartbeatTimerToClearCache = new Timer(500);
        //        _heartbeatTimerToClearCache.AutoReset = true;
        //        _heartbeatTimerToClearCache.Elapsed += _heartbeatTimerToClearCache_Elapsed;
        //        _heartbeatTimerToClearCache.Start();
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

        void _heartbeatTimerToClearCache_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                foreach (HashSet<Tuple<string, DateTime>> set in _symbolsRequestedToCentralSM.Values)
                {
                    set.RemoveWhere(x => CheckAndLogTimedOutSymbols(x.Item2, x.Item1));
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
        /// checks whether the requested symbol has timed out(occurs only in case of some error or issues with connectivity) and needs to be cleared from cache so that same symbol can be again requested from the CentralSM
        /// </summary>
        /// <param name="timeOfSymbol"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        bool CheckAndLogTimedOutSymbols(DateTime timeOfSymbol, string symbol)
        {
            try
            {
                if (DateTime.Now - timeOfSymbol > new TimeSpan(0, 15, 0))
                {
                    Logger.LoggerWrite("The response for symbol " + symbol + " was not received from centralSM. Time" + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_ClientMessages);
                    InformationReporter.GetInstance.Write("The response for symbol " + symbol + " was not reeived from centralSM.");
                    return true;
                }
                else
                {
                    return false;
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
            return false;
        }

        /// <summary>
        /// Adds the symbols not already requested to CentralSM to the requested cache and removes the symbols already requested earlier from the requestObj. 
        /// The secMasterRequestObj itself is modified in the request
        /// </summary>
        /// <param name="secMasterRequestObj"></param>
        internal void AddAndModifySymbolRequestsForCentralSM(ref SecMasterRequestObj secMasterRequestObj)
        {
            List<SymbolDataRow> symbolsNotRequested = new List<SymbolDataRow>();
            try
            {
                foreach (SymbolDataRow symbDr in secMasterRequestObj.SymbolDataRowCollection)
                {
                    if (!String.IsNullOrWhiteSpace(symbDr.BBGID))
                    {
                        //Handled BBGID here, used 9 as the symbology code for BBGID for storing in dictionary
                        if (_symbolsRequestedToCentralSM.ContainsKey(9))
                        {
                            if (!_symbolsRequestedToCentralSM[9].Any(x => String.Compare(x.Item1, symbDr.BBGID.Trim()) == 0))
                            {
                                _symbolsRequestedToCentralSM[9].Add(new Tuple<string, DateTime>(symbDr.BBGID.Trim(), DateTime.Now));
                                symbolsNotRequested.Add(symbDr);
                            }
                        }
                        else
                        {
                            //Modified by puneet, if symbology code 9 not exists
                            _symbolsRequestedToCentralSM.TryAdd(9, new HashSet<Tuple<string, DateTime>>(new Tuple<string, DateTime>[] { new Tuple<string, DateTime>(symbDr.BBGID.Trim(), DateTime.Now) }));
                            symbolsNotRequested.Add(symbDr);
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(symbDr.PrimarySymbol))
                        {
                            if (_symbolsRequestedToCentralSM.ContainsKey((int)symbDr.PrimarySymbology))
                            {
                                if (!_symbolsRequestedToCentralSM[(int)symbDr.PrimarySymbology].Any(x => String.Compare(x.Item1, symbDr.PrimarySymbol.Trim()) == 0))
                                {
                                    _symbolsRequestedToCentralSM[(int)symbDr.PrimarySymbology].Add(new Tuple<string, DateTime>(symbDr.PrimarySymbol.Trim(), DateTime.Now));
                                    symbolsNotRequested.Add(symbDr);
                                }
                            }
                            else
                            {
                                _symbolsRequestedToCentralSM.TryAdd((int)symbDr.PrimarySymbology, new HashSet<Tuple<string, DateTime>>(new Tuple<string, DateTime>[] { new Tuple<string, DateTime>(symbDr.PrimarySymbol.Trim(), DateTime.Now) }));
                                symbolsNotRequested.Add(symbDr);
                            }
                        }
                    }
                }
                secMasterRequestObj.SymbolDataRowCollection.RemoveAll(x => !symbolsNotRequested.Contains(x));
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
        /// Removes the symbol for which the response is received from the cache that maintains the list of symbols under process.
        /// </summary>
        /// <param name="secMasterObj"></param>
        /// <returns></returns>
        internal bool RemoveAndCheckRequestedSymbolForCentralSM(SecMasterBaseObj secMasterObj)
        {
            try
            {
                if (_symbolsRequestedToCentralSM.Count > 0)
                {
                    if (_symbolsRequestedToCentralSM.ContainsKey(0))
                    {
                        _symbolsRequestedToCentralSM[0].RemoveWhere(x => String.Compare(x.Item1, secMasterObj.TickerSymbol) == 0);
                    }
                    if (_symbolsRequestedToCentralSM.ContainsKey(1))
                    {
                        _symbolsRequestedToCentralSM[1].RemoveWhere(x => String.Compare(x.Item1, secMasterObj.ReutersSymbol) == 0);
                    }
                    if (_symbolsRequestedToCentralSM.ContainsKey(2))
                    {
                        _symbolsRequestedToCentralSM[2].RemoveWhere(x => String.Compare(x.Item1, secMasterObj.ISINSymbol) == 0);
                    }
                    if (_symbolsRequestedToCentralSM.ContainsKey(3))
                    {
                        _symbolsRequestedToCentralSM[3].RemoveWhere(x => String.Compare(x.Item1, secMasterObj.SedolSymbol) == 0);
                    }
                    if (_symbolsRequestedToCentralSM.ContainsKey(4))
                    {
                        _symbolsRequestedToCentralSM[4].RemoveWhere(x => String.Compare(x.Item1, secMasterObj.CusipSymbol) == 0);
                    }
                    if (_symbolsRequestedToCentralSM.ContainsKey(5))
                    {
                        _symbolsRequestedToCentralSM[5].RemoveWhere(x => String.Compare(x.Item1, secMasterObj.BloombergSymbol) == 0);
                    }
                    if (_symbolsRequestedToCentralSM.ContainsKey(6))
                    {
                        _symbolsRequestedToCentralSM[6].RemoveWhere(x => String.Compare(x.Item1, secMasterObj.OSIOptionSymbol) == 0);
                    }
                    if (_symbolsRequestedToCentralSM.ContainsKey(7))
                    {
                        _symbolsRequestedToCentralSM[7].RemoveWhere(x => String.Compare(x.Item1, secMasterObj.IDCOOptionSymbol) == 0);
                    }
                    if (_symbolsRequestedToCentralSM.ContainsKey(8))
                    {
                        _symbolsRequestedToCentralSM[8].RemoveWhere(x => String.Compare(x.Item1, secMasterObj.OpraSymbol) == 0);
                    }
                    if (_symbolsRequestedToCentralSM.ContainsKey(9))
                    {

                        _symbolsRequestedToCentralSM[9].RemoveWhere(x => String.Compare(x.Item1, secMasterObj.BBGID) == 0);
                    }
                }
                //modified by omshiv, id response is blank in case of invalid security then BBGID will be empty
                if (string.IsNullOrWhiteSpace(secMasterObj.BBGID))
                {
                    return true;
                }
                if (_bbgidResponseFromCentralSM.Contains(secMasterObj.BBGID))
                    return false;
                else
                {
                    _bbgidResponseFromCentralSM.Add(secMasterObj.BBGID);
                    return true;
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
            return true;
        }

        internal void CleanCacheForSymbolsRequestedToBB()
        {
            try
            {
                if (_symbolsRequestedToCentralSM.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (HashSet<Tuple<String, DateTime>> hshSet in _symbolsRequestedToCentralSM.Values)
                    {
                        hshSet.AsParallel().ForAll(x => sb.Append(x.Item1 + " "));
                    }
                    Logger.LoggerWrite("CentralSM disconnected. Cache Cleared please request the data again. Symbols: " + sb.ToString() + " Time " + DateTime.UtcNow, LoggingConstants.CATEGORY_FLAT_FILE_TRACING);
                    InformationReporter.GetInstance.Write("CentralSM disconnected. Cache cleared please request the data again for " + sb.ToString());
                    _symbolsRequestedToCentralSM.Clear();
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

        public void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    //  if (_heartbeatTimerToClearCache!=null)
                    //  {
                    //      _heartbeatTimerToClearCache.Dispose();
                    //  }
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
        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
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
    }
}