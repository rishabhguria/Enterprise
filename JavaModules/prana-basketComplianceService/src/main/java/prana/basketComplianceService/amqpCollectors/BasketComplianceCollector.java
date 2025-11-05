package prana.basketComplianceService.amqpCollectors;

import java.util.HashMap;
import java.util.List;

import prana.basketComplianceService.main.BasketManager;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esperio.amqp.AMQPToObjectCollector;
import com.espertech.esperio.amqp.AMQPToObjectCollectorContext;

public class BasketComplianceCollector implements AMQPToObjectCollector {

	@Override
	public void collect(AMQPToObjectCollectorContext context) {
		try {
			PranaLogManager.logOnly(new String(context.getBytes()));
			List<HashMap<String, Object>> basketTaxlots = JSONMapper.getListOfLinkedHashMap(new String(context.getBytes()));
			//LinkedHashMap<String, Object> basketTaxlot = JSONMapper.getLinkedHashMap(new String(context.getBytes()));
			BasketManager.getInstance().handleBasket(basketTaxlots);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}
