using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using Prana.TradingTicket.TTPresenter;
using Prana.TradingTicket.TTView;
using System;

namespace Prana.TradingTicket.OrderManager
{
    /// <summary>
    /// Get OrderSingle for Option
    /// </summary>
    /// <seealso cref="Prana.TradingTicket.OrderManager.GetOrderSingleBase" />
    public class GetOrderSingleOption : GetOrderSingleBase
    {
        #region Option Properties
        /// <summary>
        /// The _maturity month
        /// </summary>
        private int _maturityMonth = int.MinValue;
        /// <summary>
        /// The _maturity day
        /// </summary>
        private int _maturityDay = int.MinValue;
        /// <summary>
        /// The _put or call
        /// </summary>
        private string _putOrCall = string.Empty;
        /// <summary>
        /// The _strike price
        /// </summary>
        private double _strikePrice = double.MinValue;
        /// <summary>
        /// The _under lying symbol
        /// </summary>
        private string _underLyingSymbol = string.Empty;

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
                //orderSingle.CMTAID = int.Parse(cmbCMTA.Value.ToString());
                //orderSingle.CMTA = cmbCMTA.Text.ToString();
                orderSingle.MaturityMonthYear = _maturityMonth.ToString();
                orderSingle.MaturityDay = _maturityDay.ToString();
                orderSingle.StrikePrice = _strikePrice;
                orderSingle.PutOrCalls = _putOrCall;
                orderSingle.UnderlyingSymbol = _underLyingSymbol;
                orderSingle.SecurityType = FIXConstants.SECURITYTYPE_Options;
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
                _maturityMonth = ((SecMasterOptObj)secmasterObj).MaturityMonth;
                _maturityDay = ((SecMasterOptObj)secmasterObj).MaturityDay;
                _putOrCall = ((OptionType)((SecMasterOptObj)secmasterObj).PutOrCall).ToString();
                _strikePrice = ((SecMasterOptObj)secmasterObj).StrikePrice;
                _underLyingSymbol = ((SecMasterOptObj)secmasterObj).UnderLyingSymbol;
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