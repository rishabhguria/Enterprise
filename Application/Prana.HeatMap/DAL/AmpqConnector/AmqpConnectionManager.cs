using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.HeatMap.Delegates;
using Prana.HeatMap.EventArguments;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.HeatMap.DAL.AmpqConnector
{
    internal static class AmqpConnectionManager
    {
        private static String notificationExchangeName = "Nirvana.Portfolio";
        private static String otherDataExchangeName = "Nirvana.OtherEventStreams";

        static ConnectionStatusManager _esperConnection;

        public static event Prana.Interfaces.ConnectionMessageReceivedDelegate EsperConnected;
        public static event Prana.Interfaces.ConnectionMessageReceivedDelegate EsperDisconnected;

        public static event EsperDataReceived dsreceived;

        /// <summary>
        /// Initialise the Amqp helper for receiving the data from esper
        /// </summary>
        internal static void Initialise()
        {
            try
            {
                AmqpHelper.Started += AmqpStarted;
                AmqpHelper.Stopped += AmqpHelper_Stopped;

                _esperConnection = new ConnectionStatusManager(Module.EsperCalculator);
                _esperConnection.StatusChanged += _esperConnection_StatusChanged;

                List<String> accountSymbolSubscriber = new List<string>();
                accountSymbolSubscriber.Add("Account-Symbol");
                AmqpHelper.InitializeListenerForExchange(notificationExchangeName, MediaType.Exchange_Direct, accountSymbolSubscriber);

                List<String> esperHeatBeatSubscriber = new List<string>();
                esperHeatBeatSubscriber.Add("EsperHEARTBEAT");
                AmqpHelper.InitializeListenerForExchange(otherDataExchangeName, MediaType.Exchange_Direct, esperHeatBeatSubscriber);
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
        /// Handel esper connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void _esperConnection_StatusChanged(object sender, ConnectionEventArguments e)
        {
            try
            {
                if (e.Module == Module.EsperCalculator)
                    if (e.ConnectionStatus)
                    {
                        if (EsperConnected != null)
                            EsperConnected(null, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Esper Calculation Engine", IdentifierID = "" }));
                    }
                    else
                    {
                        if (EsperDisconnected != null)
                            EsperDisconnected(null, new EventArgs<ConnectionProperties>(new ConnectionProperties { IdentifierName = "Esper Calculation Engine", IdentifierID = "" }));
                    }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Unwire the wired events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AmqpHelper_Stopped(Object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                e.AmqpReceiver.AmqpDataReceived -= amqpReceiver_AmqpDataReceived;
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
        /// Wire the function foe receiving of account-symbol data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AmqpStarted(Object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == notificationExchangeName)
                {
                    if (e.AmqpReceiver.RoutingKey.Contains("Account-Symbol"))
                    {
                        e.AmqpReceiver.AmqpDataReceived += amqpReceiver_AmqpDataReceived;
                    }
                }
                else if (e.AmqpReceiver.MediaName == otherDataExchangeName)
                {
                    if (e.AmqpReceiver.RoutingKey.Contains("EsperHEARTBEAT"))
                    {
                        e.AmqpReceiver.AmqpDataReceived += amqpReceiver_HeartBeatReceived;
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

        /// <summary>
        /// received a heartbeat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void amqpReceiver_HeartBeatReceived(object sender, DataReceivedEventArguments e)
        {
            try
            {
                int interval = Convert.ToInt32(e.DsReceived.Tables[0].Rows[0]["Interval"].ToString());
                if (e.RoutingKey.Equals("_EsperHEARTBEAT"))
                    _esperConnection.GotAPing(interval);
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
        /// Raise the data received event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void amqpReceiver_AmqpDataReceived(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (dsreceived != null)
                    dsreceived(null, new EsperDataReceivedEventArgs() { Data = e.DsReceived });
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
