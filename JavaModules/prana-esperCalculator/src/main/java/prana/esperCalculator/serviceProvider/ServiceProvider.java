package prana.esperCalculator.serviceProvider;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;

public class ServiceProvider {

	private static ServiceProvider _serviceProvider = null;

	private static IAmqpSender _amqpSender;

	/**
	 * Return the single ton instance
	 * 
	 * @return
	 */
	public static ServiceProvider getInstance() {
		if (_serviceProvider == null)
			_serviceProvider = new ServiceProvider();
		return _serviceProvider;
	}

	private ServiceProvider() {
		try {
			String exchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_OTHERDATA);
			_amqpSender = AmqpHelper.getSender(exchangeName, ExchangeType.Direct, MediaType.Exchange, false);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
	}

	/**
	 * Process a new calculation request
	 * 
	 * @param request
	 */
	public void processNewRequest(HashMap<String, Object> request) {
		try {
			List<String> fields = new ArrayList<String>();
			fields.addAll((Arrays.asList(request.get("FieldsList").toString().split(","))));
			String compression = request.get("Compression").toString();
			String requestId = request.get("RequestId").toString();
			compression = addCompressionSpecificFieldsAndReturnWindow(compression, fields);
			String query = QueryBuilder.getQuery(compression, fields);

			PranaLogManager.info("Calculation request received | " + requestId);
			PranaLogManager.info("Request Query : " + query);

			EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);
			HashMap<String, HashMap<String, String>> calculations = new HashMap<String, HashMap<String, String>>();

			// Now depending on the compression we can have multiple rows in the result object received from Esper
			if (result != null && result.getArray().length > 0)
				calculations = processResult(result.getArray(), compression, fields, requestId);

			String messageToSend = JSONMapper.getStringForObject(calculations);
			_amqpSender.sendData(messageToSend, "CalculationResponse");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Process the query result and return the result as a hashmap
	 * 
	 * @param result
	 * @param compression
	 * @param fields
	 * @param requestId
	 * @return
	 */
	private HashMap<String, HashMap<String, String>> processResult(EventBean[] result, String compression,
			List<String> fields, String requestId) {
		HashMap<String, HashMap<String, String>> resultSet = new HashMap<String, HashMap<String, String>>();
		try {
			// For each row
			for (int i = 0; i < result.length; i++) {
				HashMap<String, String> row = new HashMap<String, String>();
				String[] columnList = result[i].getEventType().getPropertyNames();
				// For each column
				for (int j = 0; j < columnList.length; j++) {
					row.put(columnList[j], result[i].get(columnList[j]).toString());
				}
				row.put("RequestId", requestId);
				resultSet.put("row_" + i, row);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return resultSet;
	}

	/**
	 * Adds compression specific fields and returns the window name
	 * 
	 * @param compression
	 * @param fields
	 * @return
	 */
	private String addCompressionSpecificFieldsAndReturnWindow(String compression, List<String> fields) {
		// TO-DO : This mapping should be done in a better way. An XML perhaps
		try {

			switch (compression.toLowerCase()) {
			case "global":
				fields.add("compressionLevel");
				return "GlobalWithNav";
			case "account":
				fields.add("compressionLevel");
				fields.add("accountId");
				fields.add("accountShortName");
				return "AccountWithNav";
			case "accountsymbol":
				fields.add("compressionLevel");
				fields.add("accountId");
				fields.add("accountShortName");
				fields.add("symbol");
				return "AccountSymbolWithNav";
			case "accountunderlyingsymbol":
				fields.add("compressionLevel");
				fields.add("accountId");
				fields.add("accountShortName");
				fields.add("underlyingSymbol");
				return "AccountUnderlyingSymbolWithNav";
			case "masterfund":
				fields.add("compressionLevel");
				fields.add("masterFundId");
				fields.add("masterFundName");
				return "MasterFundWithNav";
			case "masterfundsymbol":
				fields.add("compressionLevel");
				fields.add("masterFundId");
				fields.add("masterFundName");
				fields.add("symbol");
				return "MasterFundSymbolWithNav";
			case "masterfundunderlyingsymbol":
				fields.add("compressionLevel");
				fields.add("masterFundId");
				fields.add("masterFundName");
				fields.add("underlyingSymbol");
				return "MasterFundUnderlyingSymbolWithNav";
			case "symbol":
				fields.add("compressionLevel");
				fields.add("symbol");
				return "SymbolWithNav";
			case "underlyingsymbol":
				fields.add("compressionLevel");
				fields.add("underlyingSymbol");
				return "UnderlyingSymbolWithNav";
			case "sector":
				fields.add("compressionLevel");
				fields.add("sector");
				return "SectorWithNav";
			case "subsector":
				fields.add("compressionLevel");
				fields.add("subSector");
				return "SubSectorWithNav";
			case "asset":
				fields.add("compressionLevel");
				fields.add("asset");
				return "AssetWithNav";
			case "symboldata":
				fields.add("symbol");
				return "SymbolDataWindow";
			case "securitydata":
				fields.add("tickerSymbol");
				return "SecurityWindow";
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return null;
	}
}
