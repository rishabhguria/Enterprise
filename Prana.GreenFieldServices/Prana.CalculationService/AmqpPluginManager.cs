using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.CalculationService.Constants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Prana.CalculationService
{
    public delegate void StartCommunicationHandler();
    public delegate void PostCompressionsReceivedHandler(string message, DataSet data);
    public delegate void RtpnlCompressionsDataReceivedHandler(string message, DataSet data, string jsonData);

    /// <summary>
    /// Singleton class for managing all Amqp server activity
    /// </summary>
    internal class AmqpPluginManager
    {

        #region Private properties
        internal event StartCommunicationHandler StartCommunication;
        internal event RtpnlCompressionsDataReceivedHandler RtpnlCompressionsDataReceived;

        string _otherDataExchangeName;
        string _rtpnlDataExchangeName;
        #endregion

        /// <summary>
        /// AmqpPluginManager instance
        /// </summary>
        static AmqpPluginManager _amqpPluginManager;

        /// <summary>
        /// Singleton pattern implemented
        /// </summary>
        /// <returns>Live instance of class</returns>
        internal static AmqpPluginManager GetInstance()
        {
            if (_amqpPluginManager == null)
                _amqpPluginManager = new AmqpPluginManager();

            return _amqpPluginManager;
        }

        /// <summary>
        /// Private constructor to implement singleton pattern
        /// </summary>
        private AmqpPluginManager()
        {

        }

        /// <summary>
        /// Initialize the manager for all compliance users
        /// </summary>
        internal void Initialise()
        {
            try
            {
                _otherDataExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);
                _rtpnlDataExchangeName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_RtpnlCompressionsExchange);
                InitializeSender();
                InitializeReceiver();
                AmqpHelper.Started += new ListenerStarted(RtpnlListenersStarted);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Initilises AmqpSenders
        /// </summary>
        private void InitializeSender()
        {
            try
            {
                AmqpHelper.InitializeSender("OtherDataSender", _otherDataExchangeName, MediaType.Exchange_Direct);
                AmqpHelper.SenderInitialised += AmqpHelper_SenderInitialised;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// AmqpHelper_SenderInitialised
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AmqpHelper_SenderInitialised(object sender, EventArgs<string> e)
        {
            try
            {
                if (StartCommunication != null)
                    StartCommunication();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// InitializeReceiver
        /// </summary>
        private void InitializeReceiver()
        {
            try
            {
                #region RtpnlDataExchange routing keys
                List<string> routingKeysStartup0 = new List<string>() { RtpnlConstants.CONST_CommunicationRequestFromEsper };
                AmqpHelper.InitializeListenerForExchange(_rtpnlDataExchangeName, MediaType.Exchange_Direct, routingKeysStartup0);

                List<string> routingKeysStartup1 = new List<string>() { RtpnlConstants.CONST_InitResponse };
                AmqpHelper.InitializeListenerForExchange(_rtpnlDataExchangeName, MediaType.Exchange_Direct, routingKeysStartup1);

                List<string> routingKeysStartup2 = new List<string>() { RtpnlConstants.CONST_RowCalculationBaseWithNavStartupData };
                AmqpHelper.InitializeListenerForExchange(_rtpnlDataExchangeName, MediaType.Exchange_Direct, routingKeysStartup2, true);

                List<string> routingKeys1 = new List<string>() { RtpnlConstants.CONST_ExtendedAccountSymbolWithNav };
                AmqpHelper.InitializeListenerForExchange(_rtpnlDataExchangeName, MediaType.Exchange_Direct, routingKeys1, true);
                #endregion
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Event listener when any of the listener has been configured and started
        /// </summary>
        /// <param name="sender">Instance of receiver which has been started</param>
        /// <param name="e"></param>
        void RtpnlListenersStarted(object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _rtpnlDataExchangeName)
                {
                    e.AmqpReceiver.AmqpDataReceived += new DataReceived(RtpnlCompressions_DataRecieved);
                    e.AmqpReceiver.Stopped += new ListenerStopped(RtpnlCompressionsReceiver_Stopped);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// RtpnlCompressionsReceiver_Stopped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RtpnlCompressionsReceiver_Stopped(object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                e.AmqpReceiver.AmqpDataReceived -= new DataReceived(RtpnlCompressions_DataRecieved);
                e.AmqpReceiver.Stopped -= new ListenerStopped(RtpnlCompressionsReceiver_Stopped);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// RtpnlCompressions_DataRecieved
        /// </summary>
        /// <param name="sender">Override data</param>
        /// <param name="e">Name of the media</param>      
        void RtpnlCompressions_DataRecieved(object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (e.MediaName == _rtpnlDataExchangeName)
                {
                    string routingKey = e.RoutingKey;

                    if (routingKey != null && RtpnlCompressionsDataReceived != null)
                    {
                        if ((routingKey.Equals(RtpnlConstants.CONST_Underscore + RtpnlConstants.CONST_RowCalculationBaseWithNavStartupData) ||
                            routingKey.Equals(RtpnlConstants.CONST_Underscore + RtpnlConstants.CONST_ExtendedAccountSymbolWithNav)) && (!string.IsNullOrWhiteSpace(e.JsonDataReceived)))
                        {
                            RtpnlCompressionsDataReceived(routingKey, null, e.JsonDataReceived);
                        }
                        else
                        {
                            if (e.DsReceived != null && e.DsReceived.Tables.Count > 0)
                                RtpnlCompressionsDataReceived(routingKey, e.DsReceived, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}