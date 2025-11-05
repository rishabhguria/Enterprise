package prana.esperCalculator.esperCEP;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.businessObjects.interfaces.IDisposable;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.runtime.client.UpdateListener;
import com.espertech.esper.runtime.client.EPRuntime;
import com.espertech.esper.runtime.client.EPStatement;

public class CepEventLoggingListener implements UpdateListener, IDisposable {
	private Object _lockerObject = new Object();
	private IAmqpSender _amqpSender;

	public CepEventLoggingListener(String exchangeName) {
		try {
			synchronized (_lockerObject) {
				if (exchangeName != null && exchangeName != "") {
					_amqpSender = AmqpHelper.getSender(exchangeName, ExchangeType.Direct, MediaType.Exchange, false);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void update(EventBean[] newEvents, EventBean[] oldEvents, EPStatement statement, EPRuntime runtime) {
		if (newEvents != null) {
			for (EventBean eventBean : newEvents) {
				try {

					String json = JSONMapper.getStringForObject(eventBean.getUnderlying());
					synchronized (_lockerObject) {

						if (_amqpSender != null) {
							String routingKey = "EventLogging";
							// To print Event Name along with the JSON data in logs.
							String s = ",\r\n    " + "\"EventName\"" + " : " + eventBean.getEventType().getName()
									+ "\t";
							json = json.substring(0, json.length() - 8) + s + json.substring(json.length() - 8);
							_amqpSender.sendData(json, routingKey);
						}
					}
				} catch (Exception ex) {
					PranaLogManager.error(ex);
				}
			}
		}
	}

	public void disposeListener() {
		try {
			synchronized (_lockerObject) {
				this._amqpSender.closeChannelAndConnection();
				this._amqpSender = null;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}