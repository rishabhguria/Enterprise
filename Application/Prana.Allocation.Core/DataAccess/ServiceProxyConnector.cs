// ***********************************************************************
// Assembly         : Prana.Allocation.Core
// Author           : dewashish
// Created          : 09-05-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-10-2014
// ***********************************************************************
// <copyright file="ServiceConnector.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.Interfaces;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;

/// <summary>
/// The DataAccess namespace.
/// </summary>
namespace Prana.Allocation.Core.DataAccess
{
    /// <summary>
    /// This class contains all required service connection proxies
    /// </summary>
    internal static class ServiceProxyConnector
    {
        /// <summary>
        /// Initializes static members of the <see cref="ServiceProxyConnector"/> class.
        /// </summary>
        static ServiceProxyConnector()
        {
            PublishingProxy = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
        }

        /// <summary>
        /// Gets or sets the closing proxy.
        /// </summary>
        /// <value>The closing proxy.</value>
        internal static IClosingServices ClosingProxy { get; set; }

        /// <summary>
        /// Gets or sets the secmaster proxy.
        /// </summary>
        /// <value>The secmaster proxy.</value>
        internal static ISecMasterServices SecmasterProxy { get; set; }

        /// <summary>
        /// Gets the activity service.
        /// </summary>
        /// <value>The activity service.</value>
        internal static IActivityServices ActivityService { get; set; }

        /// <summary>
        /// Gets the activity service.
        /// </summary>
        /// <value>The activity service.</value>
        internal static ISecMasterOTCService SecMasterOTCService { get; set; }

        /// <summary>
        /// Gets the publishing proxy.
        /// </summary>
        /// <value>The publishing proxy.</value>
        internal static ProxyBase<IPublishing> PublishingProxy { get; private set; }

        /// <summary>
        /// Gets or sets the CashManagement service.
        /// </summary>
        /// <value>The CashManagement service.</value>
        internal static ICashManagementService CashManagementService { get; set; }

        /// <summary>
        /// Gets or sets the position management services.
        /// </summary>
        /// <value>
        /// The position management services.
        /// </value>
        internal static IPranaPositionServices PositionManagementServices { get; set; }
    }
}
