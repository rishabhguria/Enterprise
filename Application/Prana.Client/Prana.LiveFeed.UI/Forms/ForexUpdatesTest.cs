////This is the service, This class can be used to test the service.

//using System;
//using System.Threading;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Diagnostics;
//using System.ServiceProcess;
//using System.Text;

//using Prana.Global;
//using Prana.BusinessObjects;
//using Prana.BusinessLogic;
//using Prana.Interfaces;
//using Prana.InstanceCreator; /// Remove reference from Prana.LiveFeed.UI
//using Prana.LiveFeed.UI.BusinessObjects;

//using Prana.Logging;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;


//namespace Prana.LiveFeed.UI
//{
//    public class ForexRatesUpdater : IDisposable    
//    {
//        ILiveFeedPublisher _liveFeedPublisher = null;
//        Timer _forexRatesUpdateTimer = null;

//        public ForexRatesUpdater()
//        {
//            try
//            {
//                OnStart();
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, Common.POLICY_EVENTLOGONLYPOLICY);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        /// <summary>
//        /// When implemented in a derived class, executes when a Start command is sent to the service 
//        /// by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically).
//        /// Specifies actions to take when the service starts.
//        /// </summary>
//        /// <param name="args">Data passed by the start command.</param>
//        private void OnStart()
//        {
//            try
//            {
//                bool isPublisherFound = GetLiveFeedForexPublisher();

//                if (isPublisherFound)
//                {
//                    const int SERVICETIMERINTERVAL = 15000;
//                    const int TIMERSTARTDUETIME = 10000;

//                    RequestForexRates();

//                    TimerCallback timerCallback = new TimerCallback(SaveForexCache);
//                    _forexRatesUpdateTimer = new Timer(timerCallback, null, TIMERSTARTDUETIME, SERVICETIMERINTERVAL);
//                }
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, Common.POLICY_EVENTLOGONLYPOLICY);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }

//        }

//        /// <summary>
//        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service 
//        /// Control Manager (SCM). Specifies actions to take when a service stops running.
//        /// </summary>
//        private void OnStop()
//        {
//            try
//            {
//                //_liveFeedPublisher.PublishForexSnapshotResponse += new EventHandler(_liveFeedPublisher_PublishForexSnapshotResponse);
//                _liveFeedPublisher.ForexContinuousDataResponse -= new EventHandler(_liveFeedPublisher_ForexContinuousDataResponse);
//                _liveFeedPublisher.RemoveForexRates(_currencyConversionList);
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, Common.POLICY_EVENTLOGONLYPOLICY);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        #region Properties

//        private List<CurrencyConversion> _currencyConversionList = null;
//        /// <summary>
//        /// Gets the currency conversion list.
//        /// </summary>
//        /// <value>The currency conversion list.</value>
//        internal List<CurrencyConversion> CurrencyConversionList
//        {
//            get
//            {
//                return _currencyConversionList;
//            }
//        }

//        Dictionary<string, CurrencyConversion> _localForexCache = new Dictionary<string, CurrencyConversion>();

//        /// <summary>
//        /// Gets the deep copy of local forex cache.
//        /// TODO : Need to make own collection of CurrencyConversion which implements the clone functionality
//        /// </summary>
//        /// <value>The local forex cache.</value>
//        internal ICollection<CurrencyConversion> LocalForexCache
//        {
//            get
//            {
//                lock (_localForexCache)
//                {
//                    if (_localForexCache.Count > 0)
//                    {
//                        return (ICollection<CurrencyConversion>)DeepCopyHelper.Clone(_localForexCache.Values);
//                    }
//                    else
//                    {
//                        return null;
//                    }
//                }
//            }
//        }

//        #endregion Properties

//        #region Forex request/response

//        /// <summary>
//        /// Gets the live feed forex publisher.
//        /// </summary>
//        /// <returns></returns>
//        private bool GetLiveFeedForexPublisher()
//        {
//            try
//            {
//                //Get the LiveFeed cache instance
//                _liveFeedPublisher = LiveFeedInstanceCreator.Instance;

//                if (_liveFeedPublisher == null)
//                    return false;

//                return true;
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, Common.POLICY_EVENTLOGONLYPOLICY);
//                if (rethrow)
//                {

//                    throw;
//                }
//                return false;
//            }
//        }

//        /// <summary>
//        /// Requests the forex rates for all available currencies in company compliance.
//        /// </summary>
//        private void RequestForexRates()
//        {
//            try
//            {
//                ///Need to subscribe before requesting for the continuous data.
//                ///TODO : If want to get forex data on a configurable interval, then modify the forex providing architecture.
//                //_liveFeedPublisher.Subscribe(this.GetHashCode(), 500);
//                List<CurrencyConversion> _currencyConversionList = CurrencyDataManager.GetInstance().GetCurrencyConversionList();

//                ///Not taking the continuous data at this time.
//                _liveFeedPublisher.ForexContinuousDataResponse += new EventHandler(_liveFeedPublisher_ForexContinuousDataResponse);
//                _liveFeedPublisher.GetForexConversionRates(_currencyConversionList);

//                //_liveFeedPublisher.PublishForexSnapshotResponse += new EventHandler(_liveFeedPublisher_PublishForexSnapshotResponse);

//                //foreach (CurrencyConversion currencyConversion in _currencyConversionList)
//                //{
//                //    _liveFeedPublisher.GetForexConversionRatesSnapshot(currencyConversion);
//                //}
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, Common.POLICY_EVENTLOGONLYPOLICY);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }



//        }

//        /// <summary>
//        /// Handles the PublishForexSnapshotResponse event of the _liveFeedPublisher control.
//        /// </summary>
//        /// <param name="sender">The source of the event.</param>
//        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
//        void _liveFeedPublisher_PublishForexSnapshotResponse(object sender, EventArgs e)
//        {
//            try
//            {
//                LiveFeedEventArgs eventArg = e as LiveFeedEventArgs;
//                if (eventArg != null)
//                {
//                    CurrencyConversion currencyConversion = eventArg.SnapshotForexData;
//                    UpdateLocalForexCache(currencyConversion);
//                }
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, Common.POLICY_EVENTLOGONLYPOLICY);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        private delegate void CurrencyConversionHandler(ICollection<CurrencyConversion> currencyConversions);

//        /// <summary>
//        /// Handles the ForexContinuousDataResponse event of the _liveFeedPublisher control.
//        /// The service keep updating a local cache of Forex rates. This cache is written to database
//        /// on the service refresh interval.
//        /// Send the local cache updation task on new thread.
//        /// TODO : Check if we require EndInvoke (To avoid memory leaks)
//        /// </summary>
//        /// <param name="sender">The source of the event.</param>
//        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
//        private void _liveFeedPublisher_ForexContinuousDataResponse(object sender, EventArgs e)
//        {
//            try
//            {
//                LiveFeedEventArgs eventArg = e as LiveFeedEventArgs;
//                if (eventArg != null)
//                {
//                    ICollection<CurrencyConversion> currencyConversions = eventArg.CurrencyConversions;
//                    CurrencyConversionHandler currencyConversionHandler = new CurrencyConversionHandler(UpdateLocalForexCache);
//                    currencyConversionHandler.BeginInvoke(currencyConversions, null, null);
//                }
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, Common.POLICY_EVENTLOGONLYPOLICY);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        #endregion Forex request/response

//        #region Private functions

//        /// <summary>
//        /// Updates the local forex cache.
//        /// </summary>
//        private void UpdateLocalForexCache(ICollection<CurrencyConversion> currencyConversions)
//        {

//            try
//            {
//                lock (_localForexCache)
//                {
//                    foreach (CurrencyConversion currencyConversion in currencyConversions)
//                    {
//                        string currencyPairSymbol = currencyConversion.CurrencyPairSymbol;
//                        if (_localForexCache.ContainsKey(currencyPairSymbol))
//                        {
//                            _localForexCache[currencyPairSymbol] = currencyConversion;
//                        }
//                        else
//                        {
//                            _localForexCache.Add(currencyPairSymbol, currencyConversion);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, Common.POLICY_EVENTLOGONLYPOLICY);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        /// <summary>
//        /// Updates the local forex cache.
//        /// </summary>
//        private void UpdateLocalForexCache(CurrencyConversion currencyConversion)
//        {
//            try
//            {
//                lock (_localForexCache)
//                {
//                    string currencyPairSymbol = currencyConversion.CurrencyPairSymbol;
//                    if (_localForexCache.ContainsKey(currencyPairSymbol))
//                    {
//                        _localForexCache[currencyPairSymbol] = currencyConversion;
//                    }
//                    else
//                    {
//                        _localForexCache.Add(currencyPairSymbol, currencyConversion);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, Common.POLICY_EVENTLOGONLYPOLICY);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        /// <summary>
//        /// Saves the forex cache.
//        /// </summary>
//        /// <param name="state">The state.</param>
//        private void SaveForexCache(object state)
//        {
//            try
//            {
//                ICollection<CurrencyConversion> forexCacheCopy = LocalForexCache;

//                using (forexCacheCopy as IDisposable)
//                {
//                    if (forexCacheCopy != null && forexCacheCopy.Count > 0)
//                    {
//                        CurrencyDataManager.GetInstance().SaveCurrencyConversionList(forexCacheCopy);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, Common.POLICY_EVENTLOGONLYPOLICY);
//                if (rethrow)
//                {
//                    throw;
//                }

//            }
//        }

//        #endregion Private functions


//        #region IDisposable Members

//        public void Dispose()
//        {
//            OnStop();
//        }

//        #endregion
//    }
//}

