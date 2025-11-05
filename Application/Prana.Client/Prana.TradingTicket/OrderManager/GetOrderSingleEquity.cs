using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.TradingTicket.TTPresenter;
using Prana.TradingTicket.TTView;
using System;

namespace Prana.TradingTicket.OrderManager
{
    /// <summary>
    /// Get OrderSingle for Equity
    /// </summary>
    /// <seealso cref="Prana.TradingTicket.OrderManager.GetOrderSingleBase" />
    class GetOrderSingleEquity : GetOrderSingleBase
    {
        /// <summary>
        /// Gets the order from ticket.
        /// </summary>
        /// <param name="iTradingTicketView">The i trading ticket view.</param>
        /// <param name="ttPresenter">The tt presenter.</param>
        /// <param name="orderSingle">The order single.</param>
        public override void GetOrderFromTicket(ITicketView iTradingTicketView, TicketPresenterBase ttPresenter, OrderSingle orderSingle)
        {
            base.GetOrderFromTicket(iTradingTicketView, ttPresenter, orderSingle);
            orderSingle.SecurityType = FIXConstants.SECURITYTYPE_Equity;
            orderSingle.DiscretionInst = "0";
            orderSingle.PegDifference = double.Epsilon;
            orderSingle.PNP = "0";
            orderSingle.DiscretionInst = string.Empty;
            orderSingle.DiscretionOffset = double.Epsilon;
            orderSingle.DisplayQuantity = 0;
            //Check for orders with associated AllocationScheme
            //https://jira.nirvanasolutions.com:8443/browse/PRANA-25709 
            int preferenceID = int.MinValue;
            Int32.TryParse(iTradingTicketView.Account, out preferenceID);
            if (preferenceID > 0 && iTradingTicketView.AccountText.StartsWith("Custom"))
            {
                orderSingle.OriginalAllocationPreferenceID = preferenceID;
            }
            orderSingle.LocateReqd = false;
            if (iTradingTicketView.IsSwap)
            {
                orderSingle.SwapParameters = iTradingTicketView.CtrlSwapParameter.GetSelectedParams(SwapValidate.Trade); ;
            }
            else
            {
                orderSingle.SwapParameters = null;
            }
        }
    }
}
