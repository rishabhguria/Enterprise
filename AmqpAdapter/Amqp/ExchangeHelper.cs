using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.AmqpAdapter.EventArguments;
using Prana.AmqpAdapter.Interfaces;
using Prana.AmqpAdapter.Json;
using Prana.LogManager;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.AmqpAdapter.Amqp
{
    class ExchangeHelper : IAmqpReceiver
    {
        #region IAmqpReceiver Members

        public event DataReceived AmqpDataReceived;

        public event ListenerStarted Started;

        public event ListenerStopped Stopped;

        public String HostName
        {
            get { return _hostName; }
        }

        public String MediaName
        {
            get { return _exchangeName; }
        }

        public MediaType Media
        {
            get { return _mediaType; }
        }

        public List<String> RoutingKey
        {
            get { return _routingKey; }
        }
        #endregion  





        /// <summary>
        /// Name of queue
        /// </summary>
        String _exchangeName;

        String _queueName;

        String _hostName;
        String _vhost;
        String _userId;
        String _password;

        MediaType _mediaType;

        List<String> _routingKey;

        bool _treatDataAsJson = false;

        IModel _channel;
        IConnection _connection;

        Boolean isOpen = false;

        internal ExchangeHelper(String hostName, String vhost, String userId, String password, String exchangeName, MediaType mediaType, List<String> routingKey, bool treatDataAsJson = false)
        {
            try
            {
                this._hostName = hostName;
                this._vhost = vhost;
                this._userId = userId;
                this._password = password;
                this._exchangeName = exchangeName;
                this._mediaType = mediaType;
                this._routingKey = routingKey;
                this._treatDataAsJson = treatDataAsJson;
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


        internal void StartReception()
        {
            try
            {
                ConnectionFactory connectionFactory = new ConnectionFactory();
                connectionFactory.HostName = this._hostName;
                connectionFactory.VirtualHost = _vhost;
                connectionFactory.UserName = this._userId;
                connectionFactory.Password = this._password;
                connectionFactory.RequestedHeartbeat = 30;  //to recover from connection issue, its ensuer better resillaince in real tim app
                _connection = connectionFactory.CreateConnection();


                _channel = _connection.CreateModel();

                _channel.ExchangeDeclarePassive(this._exchangeName);


                Dictionary<String, object> args = new Dictionary<string, object>();
                args.Add("Exclusive", true);
                String keyOut = String.Empty;
                if (_routingKey == null || _routingKey.Count == 0)
                    keyOut = "Fanout";
                else
                {
                    _routingKey.ForEach(key => keyOut += "_" + key);
                }


                this._queueName = _channel.QueueDeclare("", false, true, true, args).QueueName;

                if (this._mediaType == MediaType.Exchange_Fanout)
                    _channel.QueueBind(_queueName, _exchangeName, "");
                else if (_routingKey == null)
                {
                    throw new ArgumentNullException(nameof(_routingKey));
                }
                else
                {
                    _routingKey.ForEach(key => _channel.QueueBind(_queueName, _exchangeName, key));
                }

                QueueingBasicConsumer consumer = new QueueingBasicConsumer(_channel);
                _channel.BasicConsume(this._queueName, true, consumer);
                isOpen = true;
                if (this.Started != null)
                    this.Started(this, new ListenerStartedEventArguments(this));

                while (consumer.Model.IsOpen)
                {
                    SendAmqpDataReceived(keyOut, consumer);
                }
            }
            catch (Exception ex)
            {
                isOpen = false;
                if (Stopped != null)
                    Stopped(this, new ListenerStoppedEventArguments(this, ListenerStopCause.ErrorOccured));

                bool rethrow = ex.Message == "SharedQueue closed" ? Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY): 
                            Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyOut"></param>
        /// <param name="consumer"></param>
        private void SendAmqpDataReceived(string keyOut, QueueingBasicConsumer consumer)
        {
            BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

            try
            {
                if (ea != null && ea.Body.Length > 0 && AmqpDataReceived != null)
                {
                    if (this._treatDataAsJson)
                    {
                        string jsonMessage = Encoding.UTF8.GetString(ea.Body);
                        if (!string.IsNullOrWhiteSpace(jsonMessage))
                            this.AmqpDataReceived(this, new DataReceivedEventArguments(null, _exchangeName, this._mediaType, keyOut, jsonMessage));
                    }
                    else
                    {
                        DataSet ds = JsonHelper.Deserialize(Encoding.UTF8.GetString(ea.Body));
                        if (ds != null)
                            this.AmqpDataReceived(this, new DataReceivedEventArguments(ds, _exchangeName, this._mediaType, keyOut));
                    }

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void CloseListener()
        {
            try
            {
                if (isOpen)
                {
                    isOpen = false;
                    this._channel.Close();
                    this._connection.Close();
                    if (this.Stopped != null)
                        this.Stopped(this, new ListenerStoppedEventArguments(this, ListenerStopCause.ErrorOccured));
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
