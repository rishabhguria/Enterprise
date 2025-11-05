package prana.amqpAdapter;

import java.io.IOException;

import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.utility.logging.PranaLogManager;

import com.rabbitmq.client.AMQP.BasicProperties;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.Channel;

/**
 * This is instance of amqp sender used to send data amqp server
 * 
 * @author dewashish
 * 
 */
class AmqpSender implements IAmqpSender {

	/**
	 * Channel which will be used to send data
	 */
	private Channel _channel;

	/**
	 * Will contain connection if separate connection has been created or just
	 * contain null
	 */
	private Connection _connection;

	/**
	 * Name of the media (Exchange/Queue)
	 */
	private String _mediaName;

	/**
	 * Type of media (Exchange/Queue)
	 */
	private MediaType _mediaType;

	/**
	 * Type of exchange (Direct/Fanout/None in case of queue)
	 */
	private ExchangeType _exchangeType;

	/**
	 * Whether the channel should be created on new connection or it should use
	 * default connection
	 */
	private boolean _isNewConnectionNeeded = false;

	/**
	 * Constructor, creates a new instance of amqp sender
	 * 
	 * @param mediaName
	 *            Name of the media (Exchange/Queue)
	 * @param exchangeType
	 *            Type of exchange (Direct/Fanout/None in case of queue)
	 * @param mediaType
	 *            Type of media (Exchange/Queue)
	 * @param isNewConnectionNeeded
	 *            Whether the channel should be created on new connection or it
	 *            should use default connection
	 */
	AmqpSender(String mediaName, ExchangeType exchangeType,
			MediaType mediaType, boolean isNewConnectionNeeded) {

		this._mediaName = mediaName;
		this._exchangeType = exchangeType;
		this._mediaType = mediaType;
		this._isNewConnectionNeeded = isNewConnectionNeeded;
		initializeConnection();
	}

	/**
	 * Initialize the connection to amqp server (Creates a new channel)
	 * 
	 * @return true if created successfully, otherwise false
	 */
	private boolean initializeConnection() {
		try {
			_channel = AmqpConnectionStore.getInstance().getChannel(
					_isNewConnectionNeeded, _connection);
			return true;

		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	@Override
	public boolean sendData(String message, String routingKey) {

		try {
			if (_mediaType.equals(MediaType.Exchange)) {

				if (_exchangeType.equals(ExchangeType.Direct)) {
					_channel.basicPublish(_mediaName, routingKey,
							new BasicProperties(), message.getBytes());
					return true;
				} else if (_exchangeType.equals(ExchangeType.Fanout)) {
					_channel.basicPublish(_mediaName, "",
							new BasicProperties(), message.getBytes());
					return true;
				} else {
					throw new Exception(
							"Exchange type is not recognized. It should be either direct or fanout");
				}
			} else if (_mediaType.equals(MediaType.Queue)) {
				_channel.basicPublish("", _mediaName, new BasicProperties(),
						message.getBytes());
				return true;
			} else {
				throw new Exception("Media type is not recognized");
			}

		} catch (Exception ex) {
			PranaLogManager.error(ex,"Could not send data to media: " + _mediaName);
			return false;
		}
	}

	@Override
	public boolean closeChannelAndConnection() {

		try {
			if (_channel != null && _channel.isOpen())
				_channel.close();
			if (_connection != null && _connection.isOpen())
				_connection.close();
			return true;
		} catch (IOException ex) {
			PranaLogManager.error(ex);	
			return false;
		} catch (Exception ex) {
			PranaLogManager.error(ex);		
			return false;
		}
	}

}
