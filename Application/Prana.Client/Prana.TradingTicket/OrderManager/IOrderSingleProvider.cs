using Prana.BusinessObjects.SecurityMasterBusinessObjects;

namespace Prana.TradingTicket.OrderManager
{
    /// <summary>
    /// Based on secmasterObj assetId get the orderSingle calculator
    /// </summary>
    public interface IOrderSingleProvider
    {
        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <param name="secMasterBaseObj">The sec master base object.</param>
        /// <returns></returns>
        GetOrderSingleBase GetProvider(SecMasterBaseObj secMasterBaseObj);
    }
}