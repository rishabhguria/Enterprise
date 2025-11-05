using Microsoft.AspNetCore.Mvc;
using Prana.ServiceGateway.CacheStore;
using Prana.ServiceGateway.Constants;

namespace Prana.ServiceGateway.Controllers
{
    /// <summary>  
    /// Controller to handle health-related endpoints.  
    /// </summary>
    [ApiController]
    [Route(ControllerMethodConstants.CONST_CONTROLLER)]
    public class HealthController : ControllerBase
    {
        private readonly ServiceHealthStatusStore _serviceHealthStatusStore;

        /// <summary>  
        /// Constructor to initialize the HealthController with a ServiceHealthStatusStore.  
        /// </summary>  
        public HealthController(ServiceHealthStatusStore serviceHealthStatusStore)
        {
            _serviceHealthStatusStore = serviceHealthStatusStore;
        }

        /// <summary>  
        /// Endpoint to get the health status of all services.  
        /// </summary>  
        [HttpGet]
        [Route(ControllerMethodConstants.CONST_SERVICE_HEALTH_STATUS)]
        public IActionResult GetServiceHealthStatus()
        {
            try
            {
                var allStatuses = _serviceHealthStatusStore.GetAllStatuses();
                var nowUtc = DateTime.UtcNow;

                // Get the server's local timezone
                var localZone = TimeZoneInfo.Local;

                var result = allStatuses
                    .Select(kvp => new
                    {
                        ServiceName = kvp.Value.ServiceDisplayName,
                        Status = _serviceHealthStatusStore.IsServiceHealthy(kvp.Value.ServiceName) ? "Healthy" : "Unhealthy",
                        LastUpdated_Utc = kvp.Value.TimeStamp.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss")
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(MessageConstants.MSG_CONST_AN_ERROR_OCCURRED + ex.Message);
            }

        }
    }
}
