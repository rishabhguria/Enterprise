using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.TradingTicket.TTPresenter;
using Prana.TradingTicket.TTView;

namespace Prana.TradingTicket.OrderManager
{
    /// <summary>
    /// Get Order Based on Asset.
    /// </summary>
    public interface IGetOrderSingle
    {
        /// <summary>
        /// Gets the order from ticket.
        /// </summary>
        /// <param name="iTradingTicketView">The i trading ticket view.</param>
        /// <param name="ttPresenter">The tt presenter.</param>
        /// <param name="orderSingle">The order single.</param>
        void GetOrderFromTicket(ITicketView iTradingTicketView, TicketPresenterBase ttPresenter, OrderSingle orderSingle);
        /// <summary>
        /// Sets the sec master object for ticket.
        /// </summary>
        /// <param name="secmasterObj">The secmaster object.</param>
        /// <param name="ttPresenter">The tt presenter.</param>
        void SetSecMasterObjForTicket(SecMasterBaseObj secmasterObj, TicketPresenterBase ttPresenter);
    }
}