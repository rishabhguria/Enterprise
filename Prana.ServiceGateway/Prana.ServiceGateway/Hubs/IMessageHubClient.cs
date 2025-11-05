namespace Prana.ServiceGateway.Hubs
{
    public interface IMessageHubClient
    {
        Task SendMsgToUser(string message);
    }
}
