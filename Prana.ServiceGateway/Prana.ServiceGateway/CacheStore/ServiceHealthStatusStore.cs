using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Models;
using Prana.ServiceGateway.Utility;
using System.Collections.Concurrent;

namespace Prana.ServiceGateway.CacheStore
{
    /// <summary>  
    /// Provides a store for managing the health status of services.  
    /// This class uses an in-memory cache to store and retrieve service health statuses.  
    /// </summary> 
    public class ServiceHealthStatusStore
    {
        private readonly AppMemoryCache _serviceStatusCache;
        public ServiceHealthStatusStore(AppMemoryCache cache)
        {
            _serviceStatusCache = cache;
        }

        // Add or update a service status
        public void UpsertStatus(string serviceName, ServiceStatusDto statusDto)
        {
            var dict = _serviceStatusCache.GetData<ConcurrentDictionary<string, ServiceStatusDto>>(GlobalConstants.SERVICE_STATUS_CACHE_KEY) ?? new ConcurrentDictionary<string, ServiceStatusDto>();
            dict[serviceName] = statusDto;
            _serviceStatusCache.SetData(GlobalConstants.SERVICE_STATUS_CACHE_KEY, dict);
        }

        // Get all service status
        public IReadOnlyDictionary<string, ServiceStatusDto> GetAllStatuses()
        {
            var dict = _serviceStatusCache.GetData<ConcurrentDictionary<string, ServiceStatusDto>>(GlobalConstants.SERVICE_STATUS_CACHE_KEY);
            return dict != null
            ? new Dictionary<string, ServiceStatusDto>(dict)
            : new Dictionary<string, ServiceStatusDto>();
        }

        /// <summary>
        /// Check if a given service is healthy.
        /// Healthy means: status exists, IsLive is true, and heartbeat timestamp is fresh.
        /// </summary>
        public bool IsServiceHealthy(string serviceName)
        {
            var allStatuses = GetAllStatuses();
            if (!allStatuses.TryGetValue(serviceName, out var dto))
                return false;

            var nowUtc = DateTime.UtcNow;

            // consider healthy only if IsLive = true and timestamp is within tolerance
            var hasFreshHeartbeat = dto.TimeStamp.ToUniversalTime() > nowUtc.AddMilliseconds(-(dto.TimeInterval * 2));

            return dto.IsLive && hasFreshHeartbeat;
        }
    }
}
