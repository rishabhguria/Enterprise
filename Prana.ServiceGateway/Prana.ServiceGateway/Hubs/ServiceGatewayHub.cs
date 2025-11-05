using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using Prana.ServiceGateway.ExceptionHandling;
using Prana.ServiceGateway.Constants;
using Serilog.Context;
using Prana.ServiceGateway.Utility;
using Microsoft.IdentityModel.Tokens;

namespace Prana.ServiceGateway.Hubs
{
    [SignalRHub]
    public class ServiceGatewayHub : Hub
    {
        private readonly ILogger<ServiceGatewayHub> logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Method Override to handle connected signalR clients
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                var userId = httpContext.Request.Query[GlobalConstants.USERID];
                string encryptedToken = httpContext.Request.Query[GlobalConstants.CONST_TOKEN];
                string tokenSalt = httpContext.Request.Query[GlobalConstants.CONST_TOKEN_SALT];
                var token = System.Web.HttpUtility.UrlDecode(encryptedToken);

                bool isValid = TokenService.IsTokenValid(token, tokenSalt, _configuration);

                if (string.IsNullOrEmpty(userId))
                {
                    logger.LogError($"{MessageConstants.MSG_CONST_SIGNALR_CONNECTION_REJECTED} {MessageConstants.MSG_CONST_USERID_MISSING}");
                    throw new HubException(MessageConstants.MSG_CONST_USERID_MISSING);
                }

                if (!isValid)
                {
                    logger.LogError($"{MessageConstants.MSG_CONST_SIGNALR_CONNECTION_REJECTED} {MessageConstants.MSG_CONST_TOKEN_VALIDATION_FAILD}");
                    throw new HubException(MessageConstants.MSG_CONST_TOKEN_VALIDATION_FAILD);
                }

                // If both userId and token are valid, then proceed
                if (isValid && !string.IsNullOrEmpty(userId))
                {
                    HubClientConnectionManager.AddConnection(userId, Context.ConnectionId);
                }
            }
            catch (HubException ex)
            {
                logger.LogError($"{MessageConstants.MSG_CONST_SIGNALR_CONNECTION_REJECTED} + {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError("error while handling conenction dictionary " + ex.ToString());
            }

            await base.OnConnectedAsync();
        }


        /// <summary>
        /// Method Override to handle connected signalR clients
        /// </summary>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                var userId = httpContext.Request.Query[GlobalConstants.USERID];

                if (!string.IsNullOrEmpty(userId))
                {
                    HubClientConnectionManager.RemoveConnection(userId, Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error while handling conenction dictionary disconnect :  " + ex);
            }
            return base.OnDisconnectedAsync(exception);
        }


        public ServiceGatewayHub(
   ILogger<ServiceGatewayHub> logger,
   IConfiguration configuration)
        {
            this.logger = logger;
            _configuration = configuration;
        }


        public async Task SendMessage(string user, string message)
        {
            try
            {
                await Clients.All.SendAsync(MessageConstants.MSG_CONST_RECEIVE_MESSAGE, user, message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in SendMessage of ServiceGatewayHub");
                throw;
            }
        }
    }
}
