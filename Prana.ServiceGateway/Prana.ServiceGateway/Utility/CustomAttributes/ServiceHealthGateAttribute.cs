using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.CacheStore;
using Prana.ServiceGateway.Models;

namespace Prana.ServiceGateway.Utility.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ServiceHealthGateAttribute : TypeFilterAttribute
    {
        public ServiceHealthGateAttribute(string serviceName, string displayName = null)
            : base(typeof(ServiceHealthGateFilter))
        {
            Arguments = new object[] { serviceName, displayName };
            Order = int.MinValue; // run as early as possible  
        }
    }

    public sealed class ServiceHealthGateFilter : IActionFilter
    {
        private readonly ServiceHealthStatusStore _serviceHealthStatusStore;
        private readonly ILogger<ServiceHealthGateFilter> _logger;
        private readonly string _serviceName;
        private readonly string _displayName;

        public ServiceHealthGateFilter(ServiceHealthStatusStore store,
                                        ILogger<ServiceHealthGateFilter> logger,
                                        string serviceName,
                                        string displayName = null)
        {
            _serviceHealthStatusStore = store;
            _logger = logger;
            _serviceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
            _displayName = string.IsNullOrWhiteSpace(displayName) ? serviceName : displayName!;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var statuses = _serviceHealthStatusStore.GetAllStatuses();

            statuses.TryGetValue(_serviceName, out ServiceStatusDto dto);

            var nowUtc = DateTime.UtcNow;

            var isLive = _serviceHealthStatusStore.IsServiceHealthy(_serviceName);

            if (!isLive)
            {
                var msg = $"{_displayName} is not running";
                _logger.LogError("Service health gate blocked request: {Service} not live or stale heartbeat.", _serviceName);

                var dataJson = JsonConvert.SerializeObject(new { status = "S_503", message = msg });

                var body = new RequestResponseModel(0, dataJson);

                context.Result = new ObjectResult(body)
                {
                    StatusCode = StatusCodes.Status503ServiceUnavailable
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
