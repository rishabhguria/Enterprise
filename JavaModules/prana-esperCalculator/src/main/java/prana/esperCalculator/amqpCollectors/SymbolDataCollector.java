package prana.esperCalculator.amqpCollectors;

import java.util.HashMap;
import java.util.List;

import prana.esperCalculator.cacheClasses.SymbolDataDualCache;
import prana.esperCalculator.constants.CollectorConstants;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esperio.amqp.AMQPToObjectCollector;
import com.espertech.esperio.amqp.AMQPToObjectCollectorContext;

public class SymbolDataCollector implements AMQPToObjectCollector {
	@Override
	public void collect(AMQPToObjectCollectorContext context) {
		try {
			String symbol;
			List<HashMap<String, Object>> lstSymbolData = JSONMapper.getListOfLinkedHashMap(new String(context.getBytes()));
			for (HashMap<String, Object> symbolData : lstSymbolData) {
				if (symbolData != null) {
					symbol = symbolData.get(CollectorConstants.SYMBOL).toString();
					SymbolDataDualCache.getInstance().addToCache(symbol, symbolData);
				}
			}
		} catch (Exception ex) {
			StringBuilder sb = new StringBuilder();
			sb.append(ex.getMessage());
			sb.append("\n----------------------");
			PranaLogManager.info("Error occured in SymbolDataReceived");
			PranaLogManager.info("Json : " + new String(context.getBytes()));
			sb.append("\n----------------------");
			PranaLogManager.error(ex, sb.toString());
		}
	}
}
