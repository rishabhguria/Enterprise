using Prana.AmqpAdapter.Enums;
using Prana.LogManager;
using RabbitMQ.Client;
using System;
using System.Diagnostics;

namespace Prana.AmqpAdapter.Amqp
{
    internal class AmqpSender
    {
        String _hostName;
        String _vhost;
        String _mediaName;
        MediaType _mediaType;
        String _userId;
        String _password;

        ConnectionFactory _connectionFactory = new ConnectionFactory();
        IConnection _conncetion;
        IModel _channel;

        internal AmqpSender(String hostName, String vhost, String userId, String password)
        {
            this._hostName = hostName;
            this._vhost = vhost;
            this._userId = userId;
            this._password = password;
            _connectionFactory.HostName = this._hostName;
            _connectionFactory.VirtualHost = this._vhost;
            _connectionFactory.UserName = this._userId;
            _connectionFactory.Password = this._password;
        }

        internal void Initialize(MediaType mediaType, String mediaName)
        {
            try
            {

                this._mediaType = mediaType;
                this._mediaName = mediaName;

                _conncetion = _connectionFactory.CreateConnection();

                _channel = _conncetion.CreateModel();

                switch (_mediaType)
                {
                    case MediaType.Exchange_Fanout:
                    case MediaType.Exchange_Direct:
                        _channel.ExchangeDeclarePassive(_mediaName);
                        break;
                    case MediaType.Queue:
                        //Now predefined queue must be already present so declaring it passively autodelete made false for queue
                        //http://jira.nirvanasolutions.com:8080/browse/COMPALERT-157
                        _channel.QueueDeclarePassive(_mediaName);//, false, false, true, null);
                        break;
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

        internal void SendData(byte[] message, String key)
        {
            try
            {
                if (this._channel != null && this._conncetion != null)
                {
                    switch (_mediaType)
                    {
                        case MediaType.Exchange_Fanout:
                            _channel.BasicPublish(_mediaName, "", null, message);
                            break;
                        case MediaType.Exchange_Direct:
                            _channel.BasicPublish(_mediaName, key, null, message);
                            break;
                        case MediaType.Queue:
                            _channel.BasicPublish("", _mediaName, null, message);
                            break;
                    }
                }
                else
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Amqp Sender not initialized or has been closed for Media type: " + this._mediaType + ", Media Name: " + this._mediaName
                        + ", Routing key: " + key, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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

        internal void CloseSender()
        {
            try
            {
                if (this._channel != null)
                    this._channel.Close();

                if (this._conncetion != null)
                    this._conncetion.Close();
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

    }
}
