package prana.amqpAdapter;

import java.util.HashMap;
import java.util.TimerTask;

import prana.businessObjects.interfaces.IAmqpSender;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class AmpqHeartbeat extends TimerTask {

	private IAmqpSender _ampqHeartbeatSeander;
	private String ROUTING_KEY;
	private int interval;

	public AmpqHeartbeat(String module, int milliSeconds, IAmqpSender sender) {
		ROUTING_KEY = module + "HEARTBEAT";
		_ampqHeartbeatSeander = sender;
		interval = milliSeconds;
	}

	/**
	 * This is called regularly
	 * It sends a 'heart-beat' to module specific routing key
	 */
	@Override
	public void run() {

		try {
			// send a heart-beat and the current heart-beat interval
			HashMap<String, Object> map = new HashMap<String, Object>();
			map.put("Response", "heartbeat");
			map.put("Interval", interval);
			
			String data = JSONMapper.getStringForObject(map);
			_ampqHeartbeatSeander.sendData(data, ROUTING_KEY);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

}