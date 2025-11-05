package prana.esperCalculator.amqpCollectors;

import java.util.LinkedHashMap;

import com.espertech.esperio.amqp.AMQPToObjectCollector;
import com.espertech.esperio.amqp.AMQPToObjectCollectorContext;

import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class SecurityDetailsCollector implements AMQPToObjectCollector{
	
	@Override
	public void collect(AMQPToObjectCollectorContext context) {
		try {
			LinkedHashMap<String, Object> securityDetails = JSONMapper.getLinkedHashMap(new String(context.getBytes()));
			AmqpListenerCallbackOtherData.securityReceived(securityDetails);
			PranaLogManager.logOnly("Security details inserted for Symbol: " + securityDetails.get("TickerSymbol"));
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

}
