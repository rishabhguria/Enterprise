/**
 * 
 */
package prana.ruleEngineMediator.communication;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.ruleEngineMediator.constants.ConfigurationConstants;
import prana.ruleEngineMediator.ruleService.ShardineUtility;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

/**
 * @author Chaturvedi Dewashish <dewashish@nirvana-sol.com> Callback listener
 *         file which perform actions after data is received from amqp
 */
public class AmqpListenerEsperRequest implements IAmqpListenerCallback {

	private IAmqpSender _amqpSender;
	private String _outMessage = "{\"ResponseType\":\"InitCompleteRuleMediator\"}";

	/**
	 * @author Chaturvedi Dewashish <dewashish@nirvana-sol.com>
	 * @description Returns instance of listener
	 * @param hostName
	 *            Simply stores hostname for further uses
	 * @param exchangeName
	 *            Name of media
	 * @param routingKeyList
	 *            List of routing key
	 */
	public AmqpListenerEsperRequest(IAmqpSender amqsender) {
		try {

			_amqpSender = amqsender;// AmqpHelper.getSender(exchangeName,
			// ExchangeType.Direct, MediaType.Exchange, false);

		} catch (Exception ex) {
			PranaLogManager.error(ex);
			}
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see prana.amqpAdapter.IAmqpListenerCallback#AmqpRecieverStarted()
	 */
	@Override
	public void amqpRecieverStarted() {

		try {
			_amqpSender.sendData(_outMessage, "InitCompleteInfo");

			PranaLogManager.info("Other Event Listener started");

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see prana.amqpAdapter.IAmqpListenerCallback#AmqpRecieverStopped()
	 */
	@Override
	public void amqpRecieverStopped(String message, Exception ex) {
		PranaLogManager.error(ex,message);
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see
	 * prana.amqpAdapter.IAmqpListenerCallback#AmqpDataReceived(java.lang.String
	 * , java.lang.String)
	 */
	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		try {
			_amqpSender.sendData(_outMessage, "InitCompleteInfo");
			 String message = ShardineUtility._ruleNameWithCompression.toString(); 
			 if(message!=null) {
				message = message.substring(1, message.length()-1);
			 String requestExchangeEsper = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					 ConfigurationConstants.SECTION_EXCHANGE_LIST,ConfigurationConstants.KEY_EXCHANGE_ESPER_REQUEST);
		     IAmqpSender _amqpInitializationRequestSenderEsper = AmqpHelper.getSender(requestExchangeEsper, ExchangeType.Direct,
						MediaType.Exchange, false);
				_amqpInitializationRequestSenderEsper.sendData(message,
						ConfigurationConstants.ROUTING_KEY_RULE_COMPRESSION_INFO);
				_amqpSender.sendData(message, ConfigurationConstants.ROUTING_KEY_RULE_COMPRESSION_INFO);
			 }
			PranaLogManager.info("Initialization response sent");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

}
