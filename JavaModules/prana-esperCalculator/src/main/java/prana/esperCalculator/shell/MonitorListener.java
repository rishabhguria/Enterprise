package prana.esperCalculator.shell;

import prana.businessObjects.interfaces.IDisposable;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.runtime.client.EPRuntime;
import com.espertech.esper.runtime.client.EPRuntimeDestroyedException;
import com.espertech.esper.runtime.client.EPStatement;
import com.espertech.esper.runtime.client.UpdateListener;

public class MonitorListener implements UpdateListener, IDisposable {

	@Override
	public void update(EventBean[] newEvents, EventBean[] oldEvents, EPStatement statement, EPRuntime runtime) {
		try {
			int i = 1;
			if (newEvents != null) {
				for (EventBean eventBean : newEvents) {
					try {
						StringBuilder builder = new StringBuilder();
						builder.append(eventBean.getEventType().getName());
						builder.append("\n" + "TriggerID: " + i++ + "/"
								+ newEvents.length + "\n");
						builder.append(JSONMapper.getStringForObject(eventBean
								.getUnderlying()));
						PranaLogManager.logOnly(builder.toString());		
					} catch (EPRuntimeDestroyedException ex) {
						PranaLogManager.error(ex);
					} catch (Exception ex) {
						PranaLogManager.error(ex);
					}
				}
			}

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void disposeListener() {
		// TODO Auto-generated method stub

	}
}
