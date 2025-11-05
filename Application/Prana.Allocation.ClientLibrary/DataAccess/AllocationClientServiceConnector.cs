using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.WCFConnectionMgr;
using System;

namespace Prana.Allocation.ClientLibrary.DataAccess
{
    public class AllocationClientServiceConnector
    {
        #region Events

        /// <summary>
        /// Occurs when [server proxy connected disconnected event].
        /// </summary>
        public static event EventHandler<EventArgs<bool>> ServerProxyConnectedDisconnectedEvent;

        #endregion Events

        #region Members

        /// <summary>
        /// The _allocation
        /// </summary>
        private static ProxyBase<IAllocationManager> _allocation;

        /// <summary>
        /// The _closing services
        /// </summary>
        private static ProxyBase<IClosingServices> _closingServices;
        /// <summary>
        /// The synchronous service
        /// </summary>
        private static ProxyBase<ISecMasterSyncServices> _securityMasterServices;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets the allocation.
        /// </summary>
        /// <value>
        /// The allocation.
        /// </value>
        public static ProxyBase<IAllocationManager> Allocation
        {
            get { return _allocation; }
            set { _allocation = value; }
        }

        /// <summary>
        /// Gets the closing services.
        /// </summary>
        /// <value>
        /// The closing services.
        /// </value>
        public static ProxyBase<IClosingServices> ClosingServices
        {
            get { return _closingServices; }
            set { _closingServices = value; }
        }

        /// <summary>
        /// Gets the synchronous services.
        /// </summary>
        /// <value>
        /// The synchronous service.
        /// </value>
        public static ProxyBase<ISecMasterSyncServices> SecurityMasterServices
        {
            get { return _securityMasterServices; }
            set { _securityMasterServices = value; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="AllocationClientServiceConnector"/> class.
        /// </summary>
        static AllocationClientServiceConnector()
        {
            try
            {
                CreateAllocationServicesProxy();
                CreateClosingServicesProxy();
                CreateSecurityMasterServicesProxy();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Handles the ConnectedEvent event of the _allocationServices control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void _allocationServices_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (ServerProxyConnectedDisconnectedEvent != null)
                    ServerProxyConnectedDisconnectedEvent(null, new EventArgs<bool>(false));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the DisconnectedEvent event of the _allocationServices control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        static void _allocationServices_DisconnectedEvent(object sender, EventArgs e)
        {
            try
            {
                if (ServerProxyConnectedDisconnectedEvent != null)
                    ServerProxyConnectedDisconnectedEvent(null, new EventArgs<bool>(true));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Creates the allocation services proxy.
        /// </summary>
        private static void CreateAllocationServicesProxy()
        {
            try
            {
                _allocation = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
                _allocation.ConnectedEvent += _allocationServices_ConnectedEvent;
                _allocation.DisconnectedEvent += _allocationServices_DisconnectedEvent;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Creates the closing services proxy.
        /// </summary>
        private static void CreateClosingServicesProxy()
        {
            try
            {
                _closingServices = new ProxyBase<IClosingServices>("TradeClosingServiceEndpointAddress");
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Creates the security services proxy.
        /// </summary>
        private static void CreateSecurityMasterServicesProxy()
        {
            try
            {
                _securityMasterServices = new ProxyBase<ISecMasterSyncServices>("TradeSecMasterSyncServiceEndpointAddress");
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        #endregion Methods
    }
}
