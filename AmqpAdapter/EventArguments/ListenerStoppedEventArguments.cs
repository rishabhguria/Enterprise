using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.Interfaces;
using System;

namespace Prana.AmqpAdapter.EventArguments
{
    /// <summary>
    /// These arguments are sent when the connection status of a module changes
    /// </summary>
    public class ListenerStoppedEventArguments : EventArgs
    {
        public IAmqpReceiver AmqpReceiver { get; set; }
        public ListenerStopCause Cause { get; set; }

        public ListenerStoppedEventArguments(IAmqpReceiver amqpReceiver, ListenerStopCause cause)
        {
            AmqpReceiver = amqpReceiver;
            Cause = cause;
        }
    }
}
