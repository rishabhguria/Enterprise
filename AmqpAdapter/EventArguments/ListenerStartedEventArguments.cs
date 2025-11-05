using Prana.AmqpAdapter.Interfaces;
using System;

namespace Prana.AmqpAdapter.EventArguments
{
    /// <summary>
    /// These arguments are sent when the connection status of a module changes
    /// </summary>
    public class ListenerStartedEventArguments : EventArgs
    {
        public IAmqpReceiver AmqpReceiver { get; set; }

        public ListenerStartedEventArguments(IAmqpReceiver amqpReceiver)
        {
            AmqpReceiver = amqpReceiver;
        }
    }
}
