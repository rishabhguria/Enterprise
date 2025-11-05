using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using Prana.ServiceGateway.Constants;

namespace Prana.ServiceGateway.Hubs
{
    [SignalRHub]
    public class ServiceGatewayHubRTPNL : Hub
    {
        private readonly ILogger<ServiceGatewayHub> logger;

        /// <summary>
        /// Method Override to handle connected signalR clients
        /// </summary>
        public override Task OnConnectedAsync()
        {
            try
            {
                var httpContext = Context.GetHttpContext();
                var userId = httpContext.Request.Query[GlobalConstants.USERID];

                if (!string.IsNullOrEmpty(userId))
                {
                    HubClientConnectionManagerRTPNL.AddConnection(userId, Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error while handling conenction dictionary " + ex);
            }

            return base.OnConnectedAsync();
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
                    HubClientConnectionManagerRTPNL.RemoveConnection(userId, Context.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error while handling conenction dictionary disconnect :  " + ex);
            }
            return base.OnDisconnectedAsync(exception);
        }


        public ServiceGatewayHubRTPNL(ILogger<ServiceGatewayHub> logger)
        {
            this.logger = logger;
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
