package prana.amqpAdapter;

import prana.utility.logging.PranaLogManager;

import com.rabbitmq.client.Channel;
import com.rabbitmq.client.Connection;
import com.rabbitmq.client.ConnectionFactory;

/**
 * Store of connection releted objects
 * 
 * @author dewashish
 * 
 */
class AmqpConnectionStore {

	/**
	 * Singleton pattern is implemented
	 */
	private static AmqpConnectionStore _amqpConnectionStore;

	/**
	 * This method can be used to get the singleton instance of this class
	 * 
	 * @return singleton instance of this class
	 */
	static AmqpConnectionStore getInstance() {

		if (_amqpConnectionStore == null)
			_amqpConnectionStore = new AmqpConnectionStore();
		return _amqpConnectionStore;

	}

	/**
	 * Private constructor to implement singleton pattern
	 */
	private AmqpConnectionStore() {

	}

	/**
	 * Instance of connection factory which will be used to create connection
	 * instances
	 */
	private ConnectionFactory _connectionFactory = new ConnectionFactory();

	/**
	 * Default connection instance which will be used to create channels when
	 * new connection is not required
	 */
	private Connection _defaultConnection;

	/**
	 * Locker object to prevent cross thread operations on connection objects
	 */
	private Object _lockerObject = new Object();

	/**
	 * This method initializes the store
	 * 
	 * @param hostName
	 *            amqp server host address
	 * @param virtualHost
	 *            Name of Amqp sever virtual host
	 * @param userName
	 *            Name of the user which will be used to connect to amqp server
	 * @param password
	 *            password whichi will be used to connect to amqp server
	 * @return true if initialized correctly otherwise false
	 */
	boolean initializeStore(String hostName, String virtualHost,
			String userName, String password) {

		try {
			synchronized (_lockerObject) {
				_connectionFactory.setHost(hostName);
				_connectionFactory.setVirtualHost(virtualHost);
				_connectionFactory.setUsername(userName);
				_connectionFactory.setPassword(password);
				_defaultConnection = _connectionFactory.newConnection();
			}
			return true;
		} catch (Exception ex) {
			PranaLogManager.error(ex,"Could not initialize amqpHelper");
			return false;
		}
	}

	/**
	 * This method returns new channel instance created over connection instance
	 * 
	 * @param newConnection
	 *            Whether a new connection will be used to create channel or
	 *            existing default connection should be used for creating
	 *            channel
	 * @return The newly created channel
	 */
	Channel getChannel(boolean newConnection, Connection connection) {
		Channel channel = null;
		try {
			synchronized (_lockerObject) {
				Connection connectionTemp = null;
				if (newConnection) {
					// creating new connection in case of new connection is
					// needed
					connectionTemp = _connectionFactory.newConnection();
					connection = connectionTemp;
				} else {
					// Using default connection as new connection is not needed
					connectionTemp = _defaultConnection;
				}
				if (connectionTemp != null) {
					channel = connectionTemp.createChannel();
				} else
					throw new Exception("Connection is not initialized.");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex,"Could not create channel");
		}

		return channel;

	}

	String getAmqpConfiguaration() {

		try {
			synchronized (_lockerObject) {
				return "Amqp configuration:\n\thostName: "
						+ _connectionFactory.getHost() + "\n\tvirtual host: "
						+ _connectionFactory.getVirtualHost()
						+ "\n\tuser name: " + _connectionFactory.getUsername()
						+ "\n\tPassword: " + _connectionFactory.getPassword()
						+ "\n\tEsper version: v8.7.0"
						+ "\n";
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex,"Could not create channel");
			return "";
		}
	}

}
