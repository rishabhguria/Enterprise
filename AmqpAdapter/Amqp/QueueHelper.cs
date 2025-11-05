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
    class QueueHelper : IAmqpReceiver
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
            get { return _queueName; }
        }

        public MediaType Media
        {
            get { return MediaType.Queue; }
        }

        public List<String> RoutingKey
        {
            get { return null; }
        }



        #endregion

        //public DataReceived QueueDataReceived;
        //public ListenerStarted QueueListenerStarted;
        //public ListenerStopped QueueListenerStopped;

        /// <summary>
        /// Name of queue
        /// </summary>
        String _queueName;

        /// <summary>
        /// Amqpserver Host
        /// </summary>
        String _hostName;
        String _vhost;
        String _userId;
        String _password;
        IModel _channel;
        IConnection _connection;

        Boolean isOpen = false;


        internal QueueHelper(String hostName, String vhost, String userId, String password, String queueName)
        {
            this._hostName = hostName;
            this._queueName = queueName;
            this._vhost = vhost;
            this._userId = userId;
            this._password = password;
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
                _connection = connectionFactory.CreateConnection();
                _channel = _connection.CreateModel();

                //Now predefined queue must be already present so declaring it passively autodelete made false for queue
                //http://jira.nirvanasolutions.com:8080/browse/COMPALERT-157
                this._queueName = _channel.QueueDeclarePassive(this._queueName);//, false, false, false, null).QueueName;

                QueueingBasicConsumer consumer = new QueueingBasicConsumer(_channel);
                _channel.BasicConsume(_queueName, true, consumer);

                isOpen = true;
                if (Started != null)
                    Started(this, new ListenerStartedEventArguments(this));

                while (consumer.Model.IsOpen)
                {
                    SendAmqpDataReceived(consumer);
                }
            }
            catch (Exception ex)
            {
                isOpen = false;
                if (Stopped != null)
                    Stopped(this, new ListenerStoppedEventArguments(this, ListenerStopCause.ErrorOccured));
                bool rethrow = (ex.Message == "SharedQueue closed") ? Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY) :
                             Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SendAmqpDataReceived(QueueingBasicConsumer consumer)
        {
            BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

            try
            {
                if (ea != null && ea.Body.Length > 0 && this.AmqpDataReceived != null)
                {
                    DataSet ds = JsonHelper.Deserialize(Encoding.UTF8.GetString(ea.Body));
                    if (ds != null)
                        this.AmqpDataReceived(this, new DataReceivedEventArguments(ds, _queueName, MediaType.Queue, String.Empty));
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
                    this._channel.Close();
                    this._connection.Close();
                    isOpen = false;
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