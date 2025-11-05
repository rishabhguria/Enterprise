using Prana.BusinessObjects.Compliance.Alerting;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Prana.PreTrade.CacheStore
{
    class SimulationCache
    {
        private Dictionary<String, ManualResetEvent> _simulationRequests = new Dictionary<string, ManualResetEvent>();

        private Dictionary<String, List<Alert>> _simulatedAlerts = new Dictionary<String, List<Alert>>();

        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static SimulationCache _simulationCache = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private SimulationCache()
        { }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static SimulationCache GetInstance()
        {
            lock (_lock)
            {
                if (_simulationCache == null)
                    _simulationCache = new SimulationCache();
                return _simulationCache;
            }
        }
        #endregion

        /// <summary>
        /// Add a new request to the cache
        /// </summary>
        /// <param name="reqId"></param>
        /// <param name="manualResetEvent"></param>
        public void AddNewSimulation(String simId, ManualResetEvent manualResetEvent)
        {
            try
            {
                lock (_lock)
                {
                    _simulationRequests.Add(simId, manualResetEvent);
                    _simulatedAlerts.Add(simId, new List<Alert>());
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
        /// Add the Alerts to the cache
        /// </summary>
        /// <param name="simId"></param>
        /// <param name="calculations"></param>
        public void AddAlerts(String simId, Alert alert)
        {
            try
            {
                lock (_lock)
                {
                    if (_simulatedAlerts.ContainsKey(simId))
                        _simulatedAlerts[simId].Add(alert);
                    else
                        throw new Exception(String.Format("Alert received for an simulation that was not in the cache. Alert : {0}, Sim Id : {1} ", alert.RuleName, simId));
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
        /// Return the simulation alerts and clears the cache
        /// </summary>
        /// <param name="simID"></param>
        /// <returns></returns>
        public List<Alert> GetAlerts(String simID)
        {
            try
            {
                lock (_lock)
                {
                    List<Alert> alerts = _simulatedAlerts[simID];
                    _simulatedAlerts.Remove(simID);
                    _simulationRequests.Remove(simID);
                    return alerts;
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
            return null;
        }

        /// <summary>
        /// Returns if the basket was a simulation
        /// </summary>
        /// <param name="simId"></param>
        /// <returns></returns>
        public bool IsSimulation(String simId)
        {
            try
            {
                lock (_lock)
                {
                    if (_simulationRequests.ContainsKey(simId))
                        return true;
                    else
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
        /// Simulation complete, continue with the execution
        /// </summary>
        /// <param name="simId"></param>
        internal void ReceivedSimulationEOM(string simId)
        {
            try
            {
                lock (_lock)
                {
                    _simulationRequests[simId].Set();
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
    }
}
