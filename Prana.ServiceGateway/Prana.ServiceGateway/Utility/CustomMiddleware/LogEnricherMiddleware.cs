using Prana.ServiceGateway.Constants;
using Serilog.Context;

namespace Prana.ServiceGateway.Utility.CustomMiddleware
{
    public class LogEnricherMiddleware
    {
        private readonly RequestDelegate next;

        public LogEnricherMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string correlationId = context.Request.Headers["CorrelationId"];
            using (LogContext.PushProperty(LogConstant.CORRELATION_ID, correlationId))
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ApiContants.COMPANY_USER_ID);
                    if (userIdClaim != null)
                    {
                        var userId = userIdClaim.Value;
                        using (LogContext.PushProperty(LogConstant.USER_ID, userId))
                        {
                            await next(context);
                        }
                        return;
                    }
                }
                else
                    await next(context);
            }
        }
    }
}
 