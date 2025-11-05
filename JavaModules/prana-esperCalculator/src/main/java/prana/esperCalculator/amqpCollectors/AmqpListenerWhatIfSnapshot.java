package prana.esperCalculator.amqpCollectors;

import java.util.LinkedHashMap;

import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.esperCalculator.constants.CollectorConstants;
import prana.esperCalculator.main.WhatIfManager;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class AmqpListenerWhatIfSnapshot implements IAmqpListenerCallback {

	/**
	 * AmqpDataReceived
	 */
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		try {
			LinkedHashMap<String, Object> symbolData = JSONMapper.getLinkedHashMap(jsonReceivedData);
			if (symbolData != null) {
				String selectedFeedPrice = symbolData.get(CollectorConstants.SELECTED_FEED_PRICE).toString();
				String askPrice = symbolData.get(CollectorConstants.ASK).toString();
				String bidPrice = symbolData.get(CollectorConstants.BID).toString();
				if (!selectedFeedPrice.equals("0.0") || !askPrice.equals("0.0") || !bidPrice.equals("0.0")) {
					WhatIfManager.getInstance().whatIfSymbolDataReceived(symbolData);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * AmqpRecieverStarted
	 */
	public void amqpRecieverStarted() {
		PranaLogManager.info("AmqpReceiver for WhatIfSnapshot has STARTED");
	}

	/**
	 * AmqpRecieverStopped
	 */
	public void amqpRecieverStopped(String message, Exception ex) {
		PranaLogManager.info("AmqpReceiver for WhatIfSnapshot has Stopeed");
		PranaLogManager.error(ex, message);
	}
}