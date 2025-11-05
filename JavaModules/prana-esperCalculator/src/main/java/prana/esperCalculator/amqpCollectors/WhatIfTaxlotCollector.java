package prana.esperCalculator.amqpCollectors;

import java.util.LinkedHashMap;

import prana.esperCalculator.main.WhatIfManager;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esperio.amqp.AMQPToObjectCollector;
import com.espertech.esperio.amqp.AMQPToObjectCollectorContext;

public class WhatIfTaxlotCollector implements AMQPToObjectCollector {

	@Override
	public void collect(AMQPToObjectCollectorContext context) {
		try {
			PranaLogManager.logOnly(new String(context.getBytes()));
			LinkedHashMap<String, Object> whatIfTaxlot = JSONMapper.getLinkedHashMap(new String(context.getBytes()));
			WhatIfManager.getInstance().handleWhatIfOrder(whatIfTaxlot);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}
