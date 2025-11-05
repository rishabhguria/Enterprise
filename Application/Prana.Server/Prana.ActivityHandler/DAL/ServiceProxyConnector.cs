using Prana.Interfaces;
using Prana.WCFConnectionMgr;

namespace Prana.ActivityHandler.DAL
{
    internal static class ServiceProxyConnector
    {
        static ServiceProxyConnector()
        {
        }

        /// <summary>
        /// Gets or sets the secmaster proxy.
        /// </summary>
        /// <value>The secmaster proxy.</value>
        internal static IFixedIncomeAdapter FixedIncomeAdapter { get; set; }

        /// <summary>
        /// Gets the activity service.
        /// </summary>
        /// <value>The activity service.</value>
        internal static ProxyBase<IAllocationServices> AllocationServices { get; set; }

        /// <summary>
        /// Gets or sets the CashManagement service.
        /// </summary>
        /// <value>The CashManagement service.</value>
        internal static ICashManagementService CashManagementServices { get; set; }

        /// <summary>
        /// Gets or sets the closing services.
        /// </summary>
        /// <value>
        /// The closing services.
        /// </value>
        internal static IClosingServices ClosingServices { get; set; }
    }
}
