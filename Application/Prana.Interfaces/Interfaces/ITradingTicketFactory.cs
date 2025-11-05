using Prana.BusinessObjects;

namespace Prana.Interfaces
{
    public interface ITradingTicketFactory
    {

        /// <summary>
        /// On passing all of the required inputs, it creates a new instance of TradingTicket control and returns back.
        /// </summary>
        /// <param name="assetID">int asset id</param>
        /// <param name="underlyingID">int underlying id</param>
        /// <param name="user">Company user object</param>
        /// <param name="communicationManager">instance of communication manager</param>
        /// <param name="shouldBind">true to bind default values to combos</param>
        /// <param name="isL2Ticket">is l2 ticket</param>
        /// <returns>Returns the control whose inherent type is TradingTicketBase</returns>
        System.Windows.Forms.Control GetTradingTicket(int assetID, int underlyingID, CompanyUser user, bool shouldBind, bool isL2Ticket, bool isManualTicket);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradingTktControl"></param>
        /// <param name="IsEnabled"></param>
        /// <param name="symbol"></param>
        void SetSymbolText(System.Windows.Forms.Control tradingTktControl, bool IsEnabled, string symbol);

        void SetLimitPrice(System.Windows.Forms.Control tradingTktControl, double price);

        void SetOrderSide(System.Windows.Forms.Control tradingTktControl, string side, string openOrClose);

        void SetOrder(System.Windows.Forms.Control tradingTktControl, OrderSingle orderRequest);

    }
}
