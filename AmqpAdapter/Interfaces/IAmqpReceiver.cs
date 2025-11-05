using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using System;
using System.Collections.Generic;

namespace Prana.AmqpAdapter.Interfaces
{
    public interface IAmqpReceiver
    {
        String MediaName { get; }
        MediaType Media { get; }
        List<String> RoutingKey { get; }
        String HostName { get; }

        void CloseListener();

        event DataReceived AmqpDataReceived;
        event ListenerStarted Started;
        event ListenerStopped Stopped;

    }
}
