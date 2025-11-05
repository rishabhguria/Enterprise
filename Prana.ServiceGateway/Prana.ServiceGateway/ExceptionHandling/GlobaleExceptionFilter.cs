using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Prana.KafkaWrapper;
using Prana.ServiceGateway.Constants;
using Prana.ServiceGateway.Utility;
using Serilog;
using Serilog.Context;
using System.Net;
using System.Threading.Tasks;

namespace Prana.ServiceGateway.ExceptionHandling
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly Serilog.ILogger _logger = Log.ForContext<GlobalExceptionMiddleware>();

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var correlationId = GetCorrelationId(context);
            var ipAddress = context.Connection?.RemoteIpAddress?.ToString();

            using (LogContext.PushProperty(LogConstant.IP_ADDRESS, ipAddress))
            using (LogContext.PushProperty(LogConstant.CORRELATION_ID, correlationId))
            {
                try
                {
                    await next(context);
                }
                catch (Exception error)
                {

                    var respObj = new
                    {
                        ErrorMsg = "Internal Server error",
                        IsSuccess = false,
                        CorrelationId = correlationId,
                        IsDisplayError = error is ServiceException serviceException &&
                                        serviceException.IsDisplayError
                    };

                    _logger.Error(error, "Global error in Service Gateway: " + error.Message);
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var text = JsonConvert.SerializeObject(respObj);
                    await context.Response.WriteAsync(text);
                }
            }
        }

        private string GetCorrelationId(HttpContext context)
        {
            try
            {
                var httpHeader = context.Request.Headers;
                var correlationsId = Convert.ToString(httpHeader[LogConstant.CORRELATION_ID]);

                if (string.IsNullOrWhiteSpace(correlationsId))
                {
                    var corrId = RequestResponseModel.GetCorrelationId();
                    httpHeader[LogConstant.CORRELATION_ID] = corrId;
                    return corrId;
                }

                return correlationsId;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error while fetching correlation id");
                return "ERROR-NA";
            }
        }
    }
}
