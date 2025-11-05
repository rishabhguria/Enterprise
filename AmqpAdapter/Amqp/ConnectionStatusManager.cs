using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.LogManager;
using System;
using System.Timers;

namespace Prana.AmqpAdapter.Amqp
{
    /// <summary>
    /// This class provides for connection status of various modules
    /// As of now it only manages EsperCalulator and RuleMediator
    /// </summary>
    public class ConnectionStatusManager
    {
        public event ConnectionStatusChanged StatusChanged;

        private bool _isConnected;
        /// <summary>
        /// Store information about Esper state
        /// </summary>
        public static bool _isEsperConnected = false;
        /// <summary>
        /// Store information about Basket state
        /// </summary>
        public static bool _isBasketconnected = false;
        /// <summary>
        /// Store information about rule mediator state
        /// </summary>
        public static bool _isRuleMediatorConnected = false;

        private DateTime _lastPing;
        private int _lastPingInterval = 0;

        private Module _module;

        public ConnectionStatusManager(Module module)
        {
            try
            {
                _module = module;
                _isConnected = false;
                _lastPing = DateTime.MinValue;

                // start heartbeatTimer
                Timer updateTimer = new System.Timers.Timer(5000);
                updateTimer.Elapsed += UpdateTimer_Elapsed;
                updateTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update the last ping as the current time and stores the heartbeat interval
        /// and raise an event if the status changes to connected
        /// </summary>
        public void GotAPing(int interval)
        {
            try
            {
                _lastPing = DateTime.Now;
                _lastPingInterval = interval;

                if (!_isConnected)
                {
                    _isConnected = true;
                    if (StatusChanged != null)
                        StatusChanged(this, new ConnectionEventArguments(_module, true));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Checks for current connection status
        /// Raises an event if the connection status changes to false/dis-connected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if ((DateTime.Now - _lastPing).TotalMilliseconds > (_lastPingInterval * 2))   // comparing with twice the heartbeat interval
                {
                    if (_isConnected)
                    {
                        _isConnected = false;
                        if (StatusChanged != null)
                            StatusChanged(this, new ConnectionEventArguments(_module, false));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
