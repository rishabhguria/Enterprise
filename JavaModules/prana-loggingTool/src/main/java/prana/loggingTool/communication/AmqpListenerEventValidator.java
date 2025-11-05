package prana.loggingTool.communication;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.HashMap;

import prana.businessObjects.interfaces.IAmqpListenerCallback;

import prana.loggingTool.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class AmqpListenerEventValidator implements IAmqpListenerCallback {
	String _ruleType;

	SimpleDateFormat sdf = new SimpleDateFormat(
			ConfigurationConstants.SIMPLE_DATE_FORMAT_2);

	public AmqpListenerEventValidator(String ruleType) {
		try {
			this._ruleType = ruleType;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
	}

	HashMap<String,HashMap<String, ArrayList<String>>> data = new HashMap<String, HashMap<String, ArrayList<String>>>();

	
	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		try {
			//events will be logged only when EventsLoging is set to true in config file
			Boolean loggingPermission = Boolean.parseBoolean(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,ConfigurationConstants.KEY_APP_SETTINGS_EVENTS_LOGGING));
			if(loggingPermission)
			{
				PranaLogManager.report(jsonReceivedData);
			}
			
		} catch (Exception ex) {
			PranaLogManager.error("Error in " + _ruleType, ex);
			PranaLogManager.error(ex.getMessage(), ex);
		} finally {
			
		}

	}


	@Override
	public void amqpRecieverStarted() {

		PranaLogManager.info("Service started. RuleType: "
				+ _ruleType);
	}


	@Override
	public void amqpRecieverStopped(String message, Exception ex) {
		PranaLogManager.info("Service stopped. RuleType: "
				+ _ruleType);
		PranaLogManager.error(message, ex);
	}
}
