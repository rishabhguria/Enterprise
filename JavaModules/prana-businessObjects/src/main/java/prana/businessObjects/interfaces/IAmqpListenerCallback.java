package prana.businessObjects.interfaces;

/**
 * All classes which needs to receive events from Amqp Background listeners need
 * to implement this interface <br/>
 * <br/>
 * It contains these methods <br/>
 * amqpDataReceived - Called when any data is received at Amqp server <br/>
 * amqpRecieverStarted - Called when receiver has connected<br/>
 * amqpRecieverStopped - Called when receiver has disconnected either because of
 * error or in normal flow<br/>
 * 
 * 
 * 
 * @author Chaturvedi Dewashish <dewashish@nirvana-sol.com>
 */
public interface IAmqpListenerCallback {
	/**
	 * This method is called when data is received at Amqp end
	 * 
	 * @author Chaturvedi Dewashish <dewashish@nirvana-sol.com>
	 * @param jsonReceivedData
	 *            Data received at Amqp end in json string format
	 * @param routingKey
	 *            key using which current data is fetched
	 */
	public void amqpDataReceived(String jsonReceivedData, String routingKey);

	/**
	 * This method is called when data is receiver has been started
	 * 
	 * @author Chaturvedi Dewashish <dewashish@nirvana-sol.com>
	 */
	public void amqpRecieverStarted();

	/**
	 * This method is called when data is receiver has been stopped
	 * 
	 * @author Chaturvedi Dewashish <dewashish@nirvana-sol.com>
	 */
	public void amqpRecieverStopped(String message, Exception ex);
}
