package prana.esperCalculator.shell;

import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;
import prana.businessObjects.interfaces.*;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.runtime.client.EPRuntime;
import com.espertech.esper.runtime.client.EPStatement;
import com.espertech.esper.runtime.client.UpdateListener;
import com.espertech.esper.runtime.client.EPRuntimeDestroyedException;

public class ShellConsoleListener implements UpdateListener, IDisposable {

	@Override
	public void update(EventBean[] newEvents, EventBean[] oldEvents, EPStatement statement, EPRuntime runtime) {
		try {
			//PranaLogManager.logOnly("----------------------------------------------------");
			int i = 1;
			if (newEvents != null) {
				for (EventBean eventBean : newEvents) {
					try {
						String eventType = eventBean.getEventType().getName();
						String json = JSONMapper.getStringForObject(eventBean
								.getUnderlying());
						PranaLogManager.logOnly(eventType);
						PranaLogManager.logOnly("TriggerID: " + i++ + "/"
								+ newEvents.length);
						PranaLogManager.logOnly(json);

					} catch (EPRuntimeDestroyedException ex) {
						PranaLogManager.error(ex);
					} catch (Exception ex) {
						PranaLogManager.error(ex);
					}
				}
			}
			//PranaLogManager.logOnly("----------------------------------------------------");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void disposeListener() {
		// TODO Auto-generated method stub
	}
}
