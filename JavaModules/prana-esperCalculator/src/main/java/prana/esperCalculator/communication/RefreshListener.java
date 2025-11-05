package prana.esperCalculator.communication;

import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.utility.logging.PranaLogManager;

/**
 * Listens the refresh action
 * 
 * @author dewashish
 * 
 */
public class RefreshListener implements IAmqpListenerCallback {
	/**
	 * Listener listening when Refresh data button is clicked from EXPNL.
	 * 
	 */
	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		try {
			switch (routingKey) {
			case "RefreshData":
				DataInitializationRequestProcessor.getInstance().refreshData();
				break;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void amqpRecieverStarted() {
		PranaLogManager.info("\nRefresh listener has been started");
	}

	@Override
	public void amqpRecieverStopped(String message, Exception ex) {
		PranaLogManager.error(ex);
	}
}