package prana.esperCalculator.amqpCollectors;

import java.util.Arrays;
import java.util.LinkedHashMap;

import com.espertech.esperio.amqp.AMQPToObjectCollector;
import com.espertech.esperio.amqp.AMQPToObjectCollectorContext;

import prana.esperCalculator.constants.CollectorConstants;
import prana.esperCalculator.esperUDF.Misc;
import prana.esperCalculator.main.WhatIfManager;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class ValidatedSymbolDataCollector implements AMQPToObjectCollector {

	@Override
	public void collect(AMQPToObjectCollectorContext context) {
		try {
			LinkedHashMap<String, Object> symbolData = JSONMapper.getLinkedHashMap(new String(context.getBytes()));
			if (symbolData != null) {
				String asset = Misc.getAsset(Integer.parseInt(symbolData.get("CategoryCode").toString()));
				if (!asset.isEmpty()
						&& Arrays.asList(WhatIfManager.getInstance().assetsRequiringPricing).contains(asset)) {
					WhatIfManager.getInstance().ValidatedSymbolDataReceivedFromTT(symbolData);
					PranaLogManager.logOnly("Symbol data inserted for Symbol: " + symbolData.get(CollectorConstants.SYMBOL));
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

}
