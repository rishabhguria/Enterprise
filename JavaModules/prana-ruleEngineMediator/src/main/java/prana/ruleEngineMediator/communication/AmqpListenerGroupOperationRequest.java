package prana.ruleEngineMediator.communication;

import java.util.HashMap;

import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class AmqpListenerGroupOperationRequest implements IAmqpListenerCallback {

	private IAmqpSender _amqpSender;

	public AmqpListenerGroupOperationRequest(IAmqpSender amqsender) {
		try {

			_amqpSender = amqsender;

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		try {
			HashMap<String, Object> map = JSONMapper.getHashMap("{\"HashMap\":"
					+ jsonReceivedData + "}");
			String res = JSONMapper.getStringForObject(map);
			_amqpSender.sendData(res, "GroupOperationResponse");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void amqpRecieverStarted() {
		PranaLogManager.info("Group Operation Listener started");

	}

	@Override
	public void amqpRecieverStopped(String message, Exception ex) {
		PranaLogManager.error(ex,message);
	}

}
