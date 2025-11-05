using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.TradingTicket.TTPresenter;
using Prana.TradingTicket.TTView;
using System;
using System.Collections.Generic;

namespace Prana.TradingTicket.OrderManager
{
    /// <summary>
    /// Get OrderSingle for FX
    /// </summary>
    /// <seealso cref="Prana.TradingTicket.OrderManager.GetOrderSingleBase" />
    public class GetOrderSingleFX : GetOrderSingleBase
    {
        /// <summary>
        /// Gets the order from ticket.
        /// </summary>
        /// <param name="iTicketView">The i trading ticket view.</param>
        /// <param name="ttPresenter">The tt presenter.</param>
        /// <param name="orderSingle">The order single.</param>
        public override void GetOrderFromTicket(ITicketView iTicketView, TicketPresenterBase ticketPresenter, OrderSingle orderSingle)
        {
            try
            {
                ITradingTicketView tradingTicketView = (ITradingTicketView)iTicketView;
                TradingTicketPresenter ttPresenter = (TradingTicketPresenter)ticketPresenter;
                base.GetOrderFromTicket(iTicketView, ttPresenter, orderSingle);
                orderSingle.SecurityType = FIXConstants.SECURITYTYPE_FX;
                orderSingle.LeadCurrencyID = ttPresenter.LeadCurrencyId;
                orderSingle.VsCurrencyID = ttPresenter.VsCurrencyId;
                double priceForCalculation = 0;
                double quantityForCalculation = 0;
                Double.TryParse(iTicketView.Quantity.ToString(), out quantityForCalculation);
                Double.TryParse(iTicketView.Price.ToString(), out priceForCalculation);
                if (tradingTicketView.DealIn == ttPresenter.VsCurrencyId.ToString() && priceForCalculation != 0)
                {
                    orderSingle.SettlementCurrencyID = ttPresenter.CurrencyId;
                }
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
        public override void SetSecMasterObjForTicket(SecMasterBaseObj secmasterObj, TicketPresenterBase ticketPresenter)
        {
            base.SetSecMasterObjForTicket(secmasterObj, ticketPresenter);
            try
            {
                TradingTicketPresenter ttPresenter = (TradingTicketPresenter)ticketPresenter;
                if (secmasterObj is SecMasterFXForwardObj)
                {
                    ttPresenter.LeadCurrencyId = ((SecMasterFXForwardObj)secmasterObj).LeadCurrencyID;
                    ttPresenter.VsCurrencyId = ((SecMasterFXForwardObj)secmasterObj).VsCurrencyID;
                }
                if (secmasterObj is SecMasterFxObj)
                {
                    ttPresenter.LeadCurrencyId = ((SecMasterFxObj)secmasterObj).LeadCurrencyID;
                    ttPresenter.VsCurrencyId = ((SecMasterFxObj)secmasterObj).VsCurrencyID;
                }

                Dictionary<int, string> currencyList = new Dictionary<int, string>(2);

                if (ttPresenter.LeadCurrencyId != int.MinValue)
                    currencyList.Add(ttPresenter.LeadCurrencyId, CachedDataManager.GetInstance.GetCurrencyText(ttPresenter.LeadCurrencyId));

                if (ttPresenter.VsCurrencyId != int.MinValue)
                    currencyList.Add(ttPresenter.VsCurrencyId, CachedDataManager.GetInstance.GetCurrencyText(ttPresenter.VsCurrencyId));
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
