using Prana.AmqpAdapter.Enums;
using System;

namespace Prana.AmqpAdapter.EventArguments
{
    /// <summary>
    /// These arguments are sent when the connection status of a module changes
    /// </summary>
    public class ConnectionEventArguments : EventArgs
    {
        public Module Module { get; set; }
        public bool ConnectionStatus { get; set; }

        public ConnectionEventArguments(Module module, bool status)
        {
            Module = module;
            ConnectionStatus = status;
        }
    }
}
