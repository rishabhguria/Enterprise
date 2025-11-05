/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package prana.esperCalculator.esperCEP;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.businessObjects.interfaces.IDisposable;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.runtime.client.EPRuntime;
import com.espertech.esper.runtime.client.EPStatement;
import com.espertech.esper.runtime.client.UpdateListener;

/**
 * 
 * @author dewashish
 */
public class CepEventListenerExchange implements UpdateListener, IDisposable {
	private Object _lockerObject = new Object();
	private IAmqpSender _amqpSender;

	/**
	 * Constructor to create an instance
	 * 
	 * @param queueName
	 *            Name of the queue if JSON output need to be sent out else empty
	 *            string ("")
	 */
	public CepEventListenerExchange(String exchangeName) {
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

	/**
	 * 
	 * @param newData
	 * @param oldData
	 */
	@Override
	public void update(EventBean[] newEvents, EventBean[] oldEvents, EPStatement statement, EPRuntime runtime) {
		if (newEvents != null) {
			for (EventBean eventBean : newEvents) {
				try {
					String json = JSONMapper.getStringForObject(eventBean.getUnderlying());
					synchronized (_lockerObject) {
						if (_amqpSender != null) {
							String compressionLevel = eventBean.get("compressionLevel").toString();
							_amqpSender.sendData(json, compressionLevel);
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
