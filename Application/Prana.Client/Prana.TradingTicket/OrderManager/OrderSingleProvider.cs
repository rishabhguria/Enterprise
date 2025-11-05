using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;

namespace Prana.TradingTicket.OrderManager
{

    /// <summary>
    /// Get Order Single Based on Asset
    /// </summary>
    /// <seealso cref="Prana.TradingTicket.OrderManager.IOrderSingleProvider" />
    public class OrderSingleProvider : IOrderSingleProvider
    {
        /// <summary>
        /// The _get order single base
        /// </summary>
        private GetOrderSingleBase _getOrderSingleBase = null;
        /// <summary>
        /// The _get order single equity
        /// </summary>
        private GetOrderSingleEquity _getOrderSingleEquity = null;
        /// <summary>
        /// The _get order single fx
        /// </summary>
        private GetOrderSingleFX _getOrderSingleFX = null;
        /// <summary>
        /// The _get order single fixed income
        /// </summary>
        private GetOrderSingleFixedIncome _getOrderSingleFixedIncome = null;
        /// <summary>
        /// The _get order single future
        /// </summary>
        private GetOrderSingleFuture _getOrderSingleFuture = null;
        /// <summary>
        /// The _get order single order single option
        /// </summary>
        private GetOrderSingleOption _getOrderSingleOrderSingleOption = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderSingleProvider"/> class.
        /// </summary>
        public OrderSingleProvider()
        {
            try
            {
                _getOrderSingleOrderSingleOption = new GetOrderSingleOption();
                _getOrderSingleFuture = new GetOrderSingleFuture();
                _getOrderSingleFixedIncome = new GetOrderSingleFixedIncome();
                _getOrderSingleFX = new GetOrderSingleFX();
                _getOrderSingleEquity = new GetOrderSingleEquity();
                _getOrderSingleBase = new GetOrderSingleBase();
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
        /// Gets the provider.
        /// </summary>
        /// <param name="secMasterBaseObj">The sec master base object.</param>
        /// <returns></returns>
        public GetOrderSingleBase GetProvider(SecMasterBaseObj secMasterBaseObj)
        {
            try
            {
                if (secMasterBaseObj == null)
                    return _getOrderSingleEquity;

                AssetCategory category = (AssetCategory)secMasterBaseObj.AssetID;
                int baseAssetID = 0;
                baseAssetID = category == AssetCategory.FX || category == AssetCategory.FXForward ? (int)AssetCategory.FX
                              : Mapper.GetBaseAsset(secMasterBaseObj.AssetID);

                switch (baseAssetID)
                {

                    case (int)AssetCategory.Equity:
                        return _getOrderSingleEquity;

                    case (int)AssetCategory.Option:
                        return _getOrderSingleOrderSingleOption;

                    case (int)AssetCategory.Future:
                        return _getOrderSingleFuture;

                    case (int)AssetCategory.FX:
                        return _getOrderSingleFX;

                    case (int)AssetCategory.FixedIncome:
                        return _getOrderSingleFixedIncome;

                    default:
                        return _getOrderSingleEquity;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return _getOrderSingleBase;
            }
        }
    }
}
