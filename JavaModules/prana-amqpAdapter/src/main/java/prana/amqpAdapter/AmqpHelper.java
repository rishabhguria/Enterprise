package prana.amqpAdapter;

import java.util.ArrayList;

import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.utility.logging.PranaLogManager;

/**
 * Use this class as entry point to AmqpAdapter
 * 
 * @author dewashish
 * 
 *         TODO : Need to check in future if we can find a nice wrapper on
 *         rabbitmq for some trivial tasks like sending and receiving data using
 *         the connections and channels efficiently.
 */
public class AmqpHelper {

	/**
	 * This boolean flag is used to check whether initialization has been
	 * performed or not.
	 */
	private static boolean _isInitialized = false;

	/**
	 * This function should be called only once before using any functionality
	 * of amqp adapter. This function will log an exception if it is already
	 * initialized.
	 * 
	 * @param hostName
	 *            Amqp Host address
	 * @param virtualHost
	 *            Amqp Virtual host name
	 * @param userName
	 *            Name of the user which will be used to connect
	 * @param password
	 *            Password which will be used to connect
	 */
	public static void initialize(String hostName, String virtualHost,
			String userName, String password) {

		try {

			if (!_isInitialized) {
				PranaLogManager.logOnly("Initializing Amqp helper");
				_isInitialized = AmqpConnectionStore.getInstance()
						.initializeStore(hostName, virtualHost, userName,
								password);
				PranaLogManager.info(getAmqpSettings());

				PranaLogManager.logOnly("Amqp helper initailized");
			} else
				throw new Exception("AmqpHelper is already initailized.");

		} catch (Exception ex) {
			PranaLogManager.error(ex,"Could not initialize amqpHelper");
		}

	}

	/**
	 * Start amqp listener. which will be running in background and will be
	 * notified by callbackInstance
	 * 
	 * @param mediaName
	 *            Name of Media (Exchange or Queue)
	 * @param exchangeType
	 *            Type of exchange (Direct/Fanout/None if it is queue)
	 * @param mediaType
	 *            Type of Media (Exchange/Queue)
	 * @param routingKeyList
	 *            List of routing keys which will be used to listen
	 * @param callbackInstance
	 *            Instance of IAmqpListenerCallback which will be called of any
	 *            event
	 * @param isNewConnectionNeeded
	 *            Whether listener should be initialized on a new connection or
	 *            existing default connection should be used
	 */
	public static void startListener(String mediaName,
			ExchangeType exchangeType, MediaType mediaType,
			ArrayList<String> routingKeyList,
			IAmqpListenerCallback callbackInstance,
			boolean isNewConnectionNeeded) {

		try {
			// throwing exception if Amqp helper is not initialized already
			if (!_isInitialized)
				throw new Exception(
						"Amqp helper is not initialized. Please initialize it before using.");

			AmqpListener backgroundListener = new AmqpListener(mediaName,
					exchangeType, mediaType, routingKeyList, callbackInstance,
					isNewConnectionNeeded);
			Thread threadExchange = new Thread(backgroundListener);
			threadExchange.start();// Starting listener on another thread

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}

	/**
	 * This method returns sender (instance of IAmqpSender) which will used to
	 * send data to Amqp server
	 * 
	 * @param mediaName
	 *            Name of the media (either exchange/queue)
	 * @param exchangeType
	 *            Typeof exchange (Direct/Fanout/None in case of queue)
	 * @param mediaType
	 *            Type of media (Exchage/Queue)
	 * @param isNewConnectionNeeded
	 *            Whether listener should be initialized on a new connection or
	 *            existing default connection should be used
	 * @return Instance of IAmqpSender which can be further used to send data to
	 *         amqp server
	 */
	public static IAmqpSender getSender(String mediaName,
			ExchangeType exchangeType, MediaType mediaType,
			boolean isNewConnectionNeeded) {

		try {
			// throwing exception if Amqp helper is not initialized already
			if (!_isInitialized)
				throw new Exception(
						"Amqp helper is not initialized. Please initialize it before using.");
			IAmqpSender amqpSender = new AmqpSender(mediaName, exchangeType,
					mediaType, isNewConnectionNeeded);
			return amqpSender;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}

	}

	public static String getAmqpSettings() {
		try {
			return AmqpConnectionStore.getInstance().getAmqpConfiguaration();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

}
