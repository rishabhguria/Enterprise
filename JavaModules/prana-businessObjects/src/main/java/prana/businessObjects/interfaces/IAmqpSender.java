package prana.businessObjects.interfaces;

/**
 * Interface to send data on amqp server
 * 
 * @author dewashish
 * 
 */
public interface IAmqpSender {

	/**
	 * Send the given data to amqp server
	 * 
	 * @param message
	 *            Message to be sent
	 * @param routingKey
	 *            routing key on which data will be sent
	 * @return returns true if sent successfully, otherwise false
	 */
	boolean sendData(String message, String routingKey);

	/**
	 * Closes channel and connection if new connection was created
	 * 
	 * @return
	 */
	boolean closeChannelAndConnection();

}
