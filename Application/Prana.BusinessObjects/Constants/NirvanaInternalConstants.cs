namespace Prana.BusinessObjects
{
    public class PranaInternalConstants
    {
        public enum ConnectionStatus
        {
            CONNECTED,
            DISCONNECTED,
            NOSERVER,
            Connecting
        }
        public enum TYPE_OF_ALLOCATION
        {
            FUND,
            STRATEGY,
            BOTH,
            NOTSET
        }
        public enum ORDERSTATE_ALLOCATION
        {
            UNALLOCATED,
            GROUPED,
            ALLOCATED
        }

        public delegate void OrderSingleHandler(Prana.BusinessObjects.OrderSingle order);
        public delegate void OrderHandler(Prana.BusinessObjects.Order order);
    }
}
