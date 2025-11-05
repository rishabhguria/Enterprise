using Prana.ServiceGateway.Contracts;
using Microsoft.Net.Http.Headers;
using Prana.ServiceGateway.Constants;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Prana.ServiceGateway.ExceptionHandling;
using Serilog;
using Serilog.Context;
using System.Security.Claims;
using Prana.ServiceGateway.Models;

namespace Prana.ServiceGateway.Utility.CustomMiddleware
{
    public class TokenDecrypterMiddleware : IMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenDecrypterMiddleware> _logger;

        public TokenDecrypterMiddleware(IConfiguration configuration,
            ILogger<TokenDecrypterMiddleware> logger)
        {
            _configuration = configuration;
            this._logger = logger;
        }

        /// <summary>
        /// InvokeAsync - Handles the Decryption of JWT Token on every API request . Runs before the authentication process.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string path = context.Request?.Path;
            try
            { 
                string encryptedToken = String.Empty;
                try
                {
                    if (AuthenticationHeaderValue.TryParse(context.Request.Headers[HeaderNames.Authorization], out var headerValue))
                    {
                        var scheme = headerValue.Scheme;
                        var parameter = headerValue.Parameter;
                        encryptedToken = parameter;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while parsing token from request header in TokenDecrypterMiddleware");
                }

                string decyptionSalt = context.Request.Headers[GlobalConstants.CONST_TOKEN_SALT].ToString();

                if (!string.IsNullOrEmpty(encryptedToken) &&
                    !encryptedToken.Equals(UtilityConstants.CONST_UNDEFINED, StringComparison.InvariantCultureIgnoreCase) && encryptedToken?.ToLower() != "null")
                {
                    byte[] decryptionKey = TokenService.GetKeyBytes(_configuration[GlobalConstants.CONST_AUTHSETTINGS_SECRET_KEY].ToString(), decyptionSalt);

                    string decryptedToken = TokenService.DecryptToken(encryptedToken, decryptionKey);

                    context.Request.Headers[GlobalConstants.CONST_HEADER_AUTHORIZATION] = MessageConstants.MSG_CONST_BEARER + decryptedToken;
                }
                await next(context);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Token Decrypter Service. Request Path:"+ path);
                throw;
            }
        }
    }
}
