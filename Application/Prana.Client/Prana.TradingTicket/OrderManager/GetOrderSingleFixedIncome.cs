using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using Prana.TradingTicket.TTPresenter;
using Prana.TradingTicket.TTView;
using System;

namespace Prana.TradingTicket.OrderManager
{
    /// <summary>
    /// Get OrderSingle for FixedIncome
    /// </summary>
    /// <seealso cref="Prana.TradingTicket.OrderManager.GetOrderSingleBase" />
    public class GetOrderSingleFixedIncome : GetOrderSingleBase
    {
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
                //orderSingle.MaturityDay = ttPresenter.MaturityDate.ToString();
                //orderSingle.CouponRate = ttPresenter.CouponRate;
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
                //if (secmasterObj.AssetID == (int)AssetCategory.FixedIncome)
                //{
                //   ttPresenter.MaturityDate = ((SecMasterFixedIncome)secmasterObj).MaturityDate;
                //    ttPresenter.CouponRate = ((SecMasterFixedIncome)secmasterObj).Coupon;

                //}
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
