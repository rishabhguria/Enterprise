using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.AmqpAdapter.Json;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading;

namespace Prana.AmqpAdapter.Amqp
{
    public static class AmqpHelper
    {
        /// <summary>
        /// Currently this is static event needs to convert to instance so that only required invocation reaches listeners
        /// </summary>
        public static event DataReceived AmqpDataReceived;
        public static event ListenerStarted Started;
        public static event ListenerStopped Stopped;
        public static event EventHandler<EventArgs<String>> SenderInitialised;


        static readonly Dictionary<String, AmqpSender> _senderCache = new Dictionary<string, AmqpSender>();
        static readonly List<ExchangeHelper> _exchangeListenersCollection = new List<ExchangeHelper>();
        static readonly List<QueueHelper> _queueListenersCollection = new List<QueueHelper>();


        static String _vhost = "/";
        static String _hostName = "localhost";
        static String _userId;
        static String _password;

        static AmqpHelper()
        {
            _vhost = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_Vhost);
            _hostName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_AmqpServer);
            _userId = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_VhostUserId);
            _password = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_VhostPassword);
        }

        /// <summary>
        /// Initialize exchange listener. Data will be passed to AmqpDataReceived Event
        /// </summary>
        /// <param name="hostName">AmqpServer host address</param>
        /// <param name="exchangeName">name of exchange</param>
        /// <param name="mediaType">Type of media must not be queue</param>
        /// <param name="routingKey"></param>
        public static void InitializeListenerForExchange(String exchangeName, MediaType mediaType, List<String> routingKey, bool treatDataAsJson = false)
        {
            try
            {
                if (mediaType == MediaType.Exchange_Direct || mediaType == MediaType.Exchange_Fanout)
                {
                    BackgroundWorker bgInitializerExchange = new BackgroundWorker();
                    bgInitializerExchange.DoWork += new DoWorkEventHandler(bgInitializerExchange_DoWork);
                    bgInitializerExchange.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgInitializerExchange_RunWorkerCompleted);
                    bgInitializerExchange.RunWorkerAsync(new object[] { exchangeName, mediaType, routingKey, treatDataAsJson });
                }
                else
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Incorrect Media type", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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

        static void bgInitializerExchange_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        static void bgInitializerExchange_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] data = (object[])e.Argument;
                //string hostName = (String)data[0];
                string exchangeName = (String)data[0];
                MediaType mediaType = (MediaType)data[1];
                List<String> routingKey = (List<String>)data[2];
                bool treatDataAsJson = false;
                if(data.Length > 3)
                    treatDataAsJson = (bool)data[3];

                Thread.CurrentThread.Name = "AmqpExchangeListener- Exchange: " + exchangeName + " - ExchangeType: " + mediaType.ToString() + " - HostName: " + _hostName;

                ExchangeHelper exchangeHelper = new ExchangeHelper(_hostName, _vhost, _userId, _password, exchangeName, mediaType, routingKey, treatDataAsJson);

                lock (_exchangeListenersCollection)
                {
                    _exchangeListenersCollection.Add(exchangeHelper);
                }
                
                exchangeHelper.Started += new ListenerStarted(ListenerStartComplete);
                exchangeHelper.Stopped += new ListenerStopped(ListenerStopped);
                exchangeHelper.AmqpDataReceived += new DataReceived(ListenerDataReceived);
                exchangeHelper.StartReception();
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
        /// Initialize listener for queue
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="queueName"></param>
        public static void InitializeListenerForQueue(String queueName)
        {
            try
            {
                BackgroundWorker bgInitializerQueue = new BackgroundWorker();
                bgInitializerQueue.DoWork += new DoWorkEventHandler(bgInitializerQueue_DoWork);
                bgInitializerQueue.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgInitializerQueue_RunWorkerCompleted);
                bgInitializerQueue.RunWorkerAsync(new object[] { queueName });
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

        static void bgInitializerQueue_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        static void bgInitializerQueue_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                object[] data = (object[])e.Argument;
                string queueName = (String)data[0];
                Thread.CurrentThread.Name = "AmqpQueueListener- Q: " + queueName + " - HostName: " + _hostName;

                QueueHelper queueHelper = new QueueHelper(_hostName, _vhost, _userId, _password, queueName);

                lock (_queueListenersCollection)
                {
                    _queueListenersCollection.Add(queueHelper);
                }

                queueHelper.Started += new ListenerStarted(ListenerStartComplete);
                queueHelper.Stopped += new ListenerStopped(ListenerStopped);
                queueHelper.AmqpDataReceived += new DataReceived(ListenerDataReceived);
                queueHelper.StartReception();
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



        static void ListenerStartComplete(Object sender, ListenerStartedEventArguments e)
        {
            try
            {
                if (Started != null)
                    Started(sender, e);
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


        static void ListenerStopped(Object sender, ListenerStoppedEventArguments e)
        {
            try
            {
                if (Stopped != null)
                    Stopped(sender, e);
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


        static void ListenerDataReceived(Object sender, DataReceivedEventArguments e)
        {
            try
            {
                if (AmqpDataReceived != null)
                    AmqpDataReceived(sender, e);
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


        
        public static void InitializeSender(String cacheKey, String mediaName, MediaType mediaType)
        {
            try
            {
                lock (_senderCache)
                {
                    if (!_senderCache.ContainsKey(cacheKey))
                    {
                        BackgroundWorker bgSenderInitializer = new BackgroundWorker();
                        bgSenderInitializer.DoWork += new DoWorkEventHandler(bgSenderInitializer_DoWork);
                        bgSenderInitializer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgSenderInitializer_RunWorkerCompleted);
                        bgSenderInitializer.RunWorkerAsync(new Object[] { cacheKey, mediaName, mediaType });
                    }

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

        static void bgSenderInitializer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                object[] data = (object[])e.Result;
                string cacheKey = (String)data[0];
                AmqpSender amqpSender = (AmqpSender)data[1];
                lock (_senderCache)
                {
                    if(!_senderCache.ContainsKey(cacheKey))
                        _senderCache.Add(cacheKey, amqpSender);
                    if (SenderInitialised != null)
                        SenderInitialised(null, new EventArgs<String>(cacheKey));
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

        static void bgSenderInitializer_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Thread.CurrentThread.Name = "SenderInitializerThread";
                object[] data = (object[])e.Argument;
                string cacheKey = (String)data[0];
                string mediaName = (String)data[1];
                MediaType mediaType = (MediaType)data[2];

                AmqpSender amqpSender = new AmqpSender(_hostName, _vhost, _userId, _password);
                amqpSender.Initialize(mediaType, mediaName);
                e.Result = new Object[] { cacheKey, amqpSender };
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

        public static bool SendObject(object obj, String cacheKey, String routingKey)
        {

            try
            {
                if (_senderCache.ContainsKey(cacheKey))
                {
                    _senderCache[cacheKey].SendData(JsonHelper.Serialize(obj), routingKey);
                    return true;
                }
                else
                {
                    return false;
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
                return false;
            }

        }

        public static bool SendDataSet(DataSet dataSet, String cacheKey, String routingKey)
        {

            try
            {
                if (_senderCache.ContainsKey(cacheKey))
                {
                    _senderCache[cacheKey].SendData(JsonHelper.SerializeDataSet(dataSet), routingKey);
                    return true;
                }
                else
                {
                    return false;
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
                return false;
            }

        }

        public static bool SendDataSetRowByRow(DataSet dataSet, String cacheKey, String routingKey)
        {

            try
            {
                if (_senderCache.ContainsKey(cacheKey))
                {
                    foreach (byte[] jsonData in JsonHelper.SerializeDataSetAsRow(dataSet))
                    {
                        _senderCache[cacheKey].SendData(jsonData, routingKey);
                    }
                    return true;
                }
                else
                {
                    return false;
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
                return false;
            }

        }

        /// <summary>
        /// Closes all live connections to AmqpServer
        /// </summary>
        public static void CloseAllConnection()
        {
            try
            {
                lock (_exchangeListenersCollection)
                {
                    foreach (ExchangeHelper exch in _exchangeListenersCollection)
                    {
                        exch.CloseListener();
                    }
                }

                lock (_queueListenersCollection)
                {
                    foreach (QueueHelper que in _queueListenersCollection)
                    {
                        que.CloseListener();
                    }
                }

                lock (_senderCache)
                {
                    foreach (AmqpSender sender in _senderCache.Values)
                    {
                        sender.CloseSender();
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }

    }
}
