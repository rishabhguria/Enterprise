using Prana.BusinessObjects;
using Prana.Global;
using System;

namespace Prana.Interfaces
{
    public interface IPostTradeServices
    {
        void SendMessage(QueueMessage message);
        event EventHandler Disconnected;
        event EventHandler Connected;
        bool IsConnected { get; set; }
        void ConnectToServer();
        System.Threading.Tasks.Task ConnectToServerAsync();
        void DisConnect();
        event EventHandler<EventArgs<QueueMessage>> MessageReceived;
        event EventHandler<EventArgs<QueueMessage>> MissingTradesRecieved;
        void Retry(bool shouldRetry);
    }
}
