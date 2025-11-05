using Microsoft.Extensions.Configuration;
using Prana.BusinessObjects.GreenFieldModels;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Prana.GreenfieldServices.Common
{
    /// <summary>
    /// Base service with thread-safe service-status tracking and periodic health polling.
    /// </summary>
    public abstract class BaseService : IDisposable
    {
        // Use a concurrent map for thread-safety under timers / multi-threaded access.
        // Keys are case-insensitive to avoid duplicate entries caused by casing differences.
        private readonly ConcurrentDictionary<string, ServiceStatusDto> _services
            = new ConcurrentDictionary<string, ServiceStatusDto>(StringComparer.OrdinalIgnoreCase);

        protected Timer ServiceHealthPollingTimer;

        //timer for polling interval
        private int TimeInterval = 0;

        /// <summary>
        /// Inserts or updates a service status in a thread-safe way.
        /// </summary>
        protected void UpdateServiceStatus(string serviceName, string displayName, bool isAlive)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(serviceName))
                    throw new ArgumentException("serviceName is required.", nameof(serviceName));

                var key = serviceName.Trim();

                _services.AddOrUpdate(
                    key,
                    addValueFactory: _ => new ServiceStatusDto(key, displayName, isAlive, TimeInterval),
                    updateValueFactory: (_, existing) =>
                    {
                        return new ServiceStatusDto(key, displayName, isAlive, TimeInterval);
                    });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow) throw;
            }
        }

        /// <summary>
        /// Start a timer to periodically check service status.
        /// </summary>
        /// <param name="callBackActions"></param>
        protected void StartServiceHealthPollingTimer(Action callBackActions, int _heartBeatInterval)
        {
            try
            {
                TimeInterval = _heartBeatInterval;
                // Dispose previous timer if any.
                ServiceHealthPollingTimer?.Dispose();
                ServiceHealthPollingTimer = null;

                TimerCallback callback = _ => callBackActions();
                ServiceHealthPollingTimer = new Timer(
                    callback,
                    null,
                    TimeSpan.FromSeconds(1),   // How long to wait before the first callback , start after 1 secs
                    TimeSpan.FromMilliseconds(_heartBeatInterval)
                );
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow) throw;
            }
        }

        /// <summary>
        /// Get status for a specific service.
        /// </summary>
        protected ServiceStatusDto GetServiceStatus(string serviceName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(serviceName))
                    return null;

                _services.TryGetValue(serviceName.Trim(), out var dto);
                return dto;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow) throw;
                return null;
            }
        }

        /// <summary>
        /// Get all services statuses.
        /// </summary>
        protected IReadOnlyCollection<ServiceStatusDto> GetAllServiceStatus()
        {
            try
            {
                return _services.Values.ToArray(); // snapshot
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow) throw;
                return Array.Empty<ServiceStatusDto>();
            }
        }

        // Stops the service health polling timer and disposes of its resources.  
        protected void StopServiceHealthPollingTimer()
        {
            try
            {
                ServiceHealthPollingTimer?.Change(Timeout.Infinite, Timeout.Infinite);
                ServiceHealthPollingTimer?.Dispose();
                ServiceHealthPollingTimer = null;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow) throw;
            }
        }

        #region IDisposable Methods
        /// <summary>
        /// Cleanup code 
        /// </summary>
        public void Dispose()
        {
            try
            {
                ServiceHealthPollingTimer?.Change(Timeout.Infinite, Timeout.Infinite);
                ServiceHealthPollingTimer?.Dispose();
                ServiceHealthPollingTimer = null;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow) throw;
            }
        }
        #endregion
    }
}
