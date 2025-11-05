using Microsoft.AspNetCore.Authorization;
using Prana.ServiceGateway.Contracts;
using System.Net;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Constants;

namespace Prana.ServiceGateway.Utility
{
    public class TokenManagerMiddleware : IMiddleware
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<TokenManagerMiddleware> _logger;

        public TokenManagerMiddleware(
            ITokenService tokenService,
            ILogger<TokenManagerMiddleware> logger)
        {
            _tokenService = tokenService;
            this._logger = logger;
        }

        /// <summary>
        /// InvokeAsync
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            bool IsRequestAllowAnonymous = false;
            bool isActiveToken = false;
            string requestPath = context?.Request?.Path;
            try
            {
                IsRequestAllowAnonymous = context.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() is object;
                isActiveToken = await _tokenService.IsCurrentActiveToken();
                if (isActiveToken || IsRequestAllowAnonymous)
                {
                    await next(context);
                    return;
                }
                _logger.LogInformation("Returning Unauthorized with url: {0}, Parameter value- AllowAnonymous:{1}, TokenActive:{2}",
                    requestPath, IsRequestAllowAnonymous, isActiveToken);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in InvokeAsync Method. Request Path:{requestPath}, IsAllowAnonymous:{IsRequestAllowAnonymous}, IsTokenActive:{isActiveToken}");
                throw;
            }
        }
    }
}
