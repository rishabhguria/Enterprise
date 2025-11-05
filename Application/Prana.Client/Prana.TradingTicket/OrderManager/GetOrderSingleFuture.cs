using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using Prana.TradingTicket.TTPresenter;
using Prana.TradingTicket.TTView;
using System;

namespace Prana.TradingTicket.OrderManager
{
    /// <summary>
    /// Get OrderSingle for Future
    /// </summary>
    /// <seealso cref="Prana.TradingTicket.OrderManager.GetOrderSingleBase" />
    public class GetOrderSingleFuture : GetOrderSingleBase
    {

        #region Future Properties
        private DateTime _expirationDate = DateTimeConstants.MinValue;

        private int _maturityMonth = 0;


        #endregion



        /// <summary>
        /// Gets the order from ticket.
        /// </summary>
        /// <param name="iTradingTicketView">The i trading ticket view.</param>
        /// <param name="ttPresenter">The tt presenter.</param>
        /// <param name="orderSingle">The order single.</param>
        public override void GetOrderFromTicket(ITicketView iTradingTicketView, TicketPresenterBase ttPresenter, OrderSingle orderSingle)
        {
            try
            {
                base.GetOrderFromTicket(iTradingTicketView, ttPresenter, orderSingle);
                if (orderSingle.DiscretionInst == string.Empty)
                {
                    if (orderSingle.DiscretionOffset == 0.0)
                    {
                        orderSingle.DiscretionOffset = double.Epsilon;
                    }
                }
                orderSingle.ExpirationDate = _expirationDate;
                orderSingle.MaturityMonthYear = _maturityMonth.ToString();
                orderSingle.SecurityType = FIXConstants.SECURITYTYPE_Futures;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the sec master object for ticket.
        /// </summary>
        /// <param name="secmasterObj">The secmaster object.</param>
        /// <param name="ttPresenter">The tt presenter.</param>
        public override void SetSecMasterObjForTicket(SecMasterBaseObj secmasterObj, TicketPresenterBase ttPresenter)
        {
            try
            {
                base.SetSecMasterObjForTicket(secmasterObj, ttPresenter);
                _expirationDate = ((SecMasterFutObj)secmasterObj).ExpirationDate;
                _maturityMonth = ((SecMasterFutObj)secmasterObj).MaturityMonth;


            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
