using Prana.AmqpAdapter.EventArguments;
using System;

namespace Prana.AmqpAdapter.Delegates
{
    /// <summary>
    /// Connection status changed event,
    /// Raised when the connection status of a module is changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ConnectionStatusChanged(Object sender, ConnectionEventArguments e);
}
