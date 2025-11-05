namespace Prana.TradingTicket.TTView
{
    public interface IQuickTradingTicketView : ITicketView
    {
        /// <summary>
        /// Set the validationTokenSource
        /// </summary>
        ///
        void SetValidationTokenSource();
        void ToggleBlotterLinking();
    }
}


