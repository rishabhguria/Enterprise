package prana.amqpAdapter;

import java.io.IOException;
import java.util.ArrayList;

import com.rabbitmq.client.Channel;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.ConsumerCancelledException;
import com.rabbitmq.client.QueueingConsumer;
import com.rabbitmq.client.ShutdownSignalException;

import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.utility.logging.PranaLogManager;

/**
 * This class provides the definition of Amqp background listener. Instance of
 * this class runs on different thread
 * 
 * @author dewashish
 * 
 */
class AmqpListener implements Runnable {

	/**
	 * Constructor of this instance
	 * 
	 * @param mediaName
	 *            Name of the Media (Exchange/Queue)
	 * @param exchangeType
	 *            Type of exchange(Direct/fanout/None if queue is specified)
	 * @param mediaType
	 *            Type of media (Exchange/Queue)
	 * @param routingKeyList
	 *            List of routing key on which listener will start listening
	 * @param callbackInstance
	 *            Instance of callback, which will be used to notify changes
	 * @param isNewConnectionNeeded
	 *            Whether this should use new connection or existing default
	 *            connection should be used
	 */
	AmqpListener(String mediaName, ExchangeType exchangeType,
			MediaType mediaType, ArrayList<String> routingKeyList,
			IAmqpListenerCallback callbackInstance,
			boolean isNewConnectionNeeded) {

		_callbackInstance = callbackInstance;
		_mediaName = mediaName;
		_routingKeyList = routingKeyList;
		_exchangeType = exchangeType;
		_mediaType = mediaType;

	}

	/**
	 * Instance of callback, which will be used to notify changes
	 */
	private IAmqpListenerCallback _callbackInstance;

	/**
	 * Name of the Media (Exchange/Queue)
	 */
	private String _mediaName;

	/**
	 * List of routing key on which listener will start listening
	 */
	private ArrayList<String> _routingKeyList;

	/**
	 * Type of exchange(Direct/fanout/None if queue is specified)
	 */
	private ExchangeType _exchangeType;

	/**
	 * Type of media (Exchange/Queue)
	 */
	private MediaType _mediaType;

	/**
	 * Whether this should use new connection or existing default connection
	 * should be used
	 */
	private boolean _isNewConnectionNeeded = false;

	/**
	 * Created channel which will consume data from Amqp server
	 */
	private Channel _channel;

	private Connection _connection;

	@Override
	public void run() {

		try {
			String queueToBeConsumed = createConnection();
			startListening(queueToBeConsumed);
		} catch (Exception ex) {
			PranaLogManager.error(ex,"Listener thread is stopped");
		}

	}

	/**
	 * Create channel and assign it to global variable
	 * 
	 * @return Name of the queue on which consumption should take place
	 */
	private String createConnection() {

		try {
			_channel = AmqpConnectionStore.getInstance().getChannel(
					_isNewConnectionNeeded, _connection);

			if (_channel != null) {
				if (_mediaType.equals(MediaType.Exchange))
					return bindMediaForExchange();
				if (_mediaType.equals(MediaType.Queue))
					return bindMediaForQueue();
			} else {
				throw new Exception("Could not create channel");
			}
			return "";
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return "";
		}

	}

	/**
	 * This method bind the channel to a generated queue from defined exchange
	 * 
	 * @return Name of the queue on which consumption will take place
	 */
	private String bindMediaForExchange() {

		try {
			if (_mediaName.compareTo("") != 0) {

				try {
					_channel.exchangeDeclarePassive(_mediaName);
				} catch (IOException ex) {

					_channel.exchangeDeclare(_mediaName,
							_exchangeType.toString(), true, false, null);
				}

				String queueName = _channel.queueDeclare().getQueue();

				if (_routingKeyList != null && _routingKeyList.size() > 0) {
					for (String routingKey : _routingKeyList) {
						_channel.queueBind(queueName, _mediaName, routingKey);
					}
				} else {
					_channel.queueBind(queueName, _mediaName, "");
				}

				return queueName;

			}

			else {
				throw new Exception("Empty exchange name provided");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex,"Could not bind exchange");
			return "";
		}

	}

	/**
	 * This method bind the channel to a defined queue from defualt exchange
	 * 
	 * @return Name of the queue on which consumption will take place
	 */
	private String bindMediaForQueue() {

		try {
			if (_mediaName.compareTo("") != 0) {

				try {
					_channel.queueDeclarePassive(_mediaName);
				} catch (IOException ex) {

					_channel.queueDeclare(_mediaName, true, true, false, null);
				}

				_channel.queueBind(_mediaName, "", "");

				return _mediaName;
			}

			else
				throw new Exception("Empty queue name provided");
		} catch (Exception ex) {
			PranaLogManager.error(ex,"Could not bind queue");
			return "";
		}

	}
	QueueingConsumer consumer;
	String message;
	String routingKey;
	/**
	 * Start the listening process
	 * 
	 * @param queueToBeConsumed
	 *            queue on which consumption will take place
	 */
	private void startListening(String queueToBeConsumed) {

		try {
			consumer = new QueueingConsumer(_channel);
			_channel.basicConsume(queueToBeConsumed, false, consumer);
			_callbackInstance.amqpRecieverStarted();
			while (true) {
				QueueingConsumer.Delivery delivery = consumer.nextDelivery();
				message = new String(delivery.getBody());
				routingKey = delivery.getEnvelope().getRoutingKey();
				_callbackInstance.amqpDataReceived(message, routingKey);
				consumer.getChannel().basicAck(delivery.getEnvelope().getDeliveryTag(), false);
			}
		} catch (ShutdownSignalException ex) {
			PranaLogManager.error(ex,"Connection shutdown");
			_callbackInstance.amqpRecieverStopped("Connection shutdown", ex);
		} catch (ConsumerCancelledException ex) {
			PranaLogManager.error(ex,"Consumer cancel operation");
			_callbackInstance.amqpRecieverStopped("Consumer cancel operation",
					ex);
		} catch (IOException ex) {
			PranaLogManager.error(ex,"Error in IO");
			_callbackInstance.amqpRecieverStopped("Error in IO", ex);
		} catch (InterruptedException ex) {
			PranaLogManager.error(ex);
			_callbackInstance.amqpRecieverStopped("Listener interrupted", ex);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			_callbackInstance.amqpRecieverStopped(ex.getMessage(), ex);
		}
	}
}
