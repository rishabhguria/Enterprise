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
public class CepEventListenerQueue implements UpdateListener, IDisposable {
	IAmqpSender _amqpSender;

	/**
	 * Constructor to create an instance
	 * 
	 * @param queueName
	 *            Name of the queue if JSON output need to be sent out else empty
	 *            string ("")
	 */
	public CepEventListenerQueue(String queueName) {
		try {
			if (queueName.length() > 0) {
				_amqpSender = AmqpHelper.getSender(queueName, ExchangeType.None, MediaType.Queue, false);
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
		PranaLogManager.info("----------------------------------------------------");
		if (newEvents != null) {
			for (EventBean eventBean : newEvents) {
				try {
					String json = JSONMapper.getStringForObject(eventBean.getUnderlying());
					if (_amqpSender != null) {
						_amqpSender.sendData(json, "");
					}
				} catch (Exception ex) {
					PranaLogManager.error(ex);
				}
			}
		}
	}

	public void disposeListener() {
		try {
			this._amqpSender.closeChannelAndConnection();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}
