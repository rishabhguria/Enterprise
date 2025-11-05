using System;

namespace Prana.BusinessObjects.EventArguments
{
    public class QueuedDelegateEventArgs : EventArgs
    {
        Order _order;
        public Order Orders { get { return _order; } }

        PranaInternalConstants.ConnectionStatus _connStatus;
        public PranaInternalConstants.ConnectionStatus ConnStatus { get { return _connStatus; } }

        public QueuedDelegateEventArgs(Order order, PranaInternalConstants.ConnectionStatus connStatus)
        {
            this._connStatus = connStatus;
            this._order = order;
        }
    }
}
