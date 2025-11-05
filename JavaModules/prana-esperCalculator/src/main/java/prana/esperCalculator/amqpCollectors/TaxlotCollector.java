package prana.esperCalculator.amqpCollectors;

import java.util.LinkedHashMap;

import prana.esperCalculator.cacheClasses.TaxlotDualCache;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esperio.amqp.AMQPToObjectCollector;
import com.espertech.esperio.amqp.AMQPToObjectCollectorContext;

public class TaxlotCollector implements AMQPToObjectCollector {

	/*
	 * TypeReference<LinkedHashMap<String, Object>> _typeReference = new
	 * TypeReference<LinkedHashMap<String, Object>>() { };
	 */
	@Override
	public void collect(AMQPToObjectCollectorContext context) {
		try {
			LinkedHashMap<String, Object> taxlot = JSONMapper.getLinkedHashMap(new String(context.getBytes()));
			if (taxlot != null) {
				String taxlotId = taxlot.get("ID").toString();
				TaxlotDualCache.getInstance().addToCache(taxlotId, taxlot);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}
