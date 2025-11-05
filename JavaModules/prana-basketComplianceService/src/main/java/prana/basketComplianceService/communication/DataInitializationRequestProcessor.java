package prana.basketComplianceService.communication;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;
import java.util.concurrent.CountDownLatch;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.basketComplianceService.customRule.RuleManager;
import prana.basketComplianceService.main.BasketManager;
import prana.basketComplianceService.main.TaxlotCache;
import prana.basketComplianceService.models.AccountNavRequestDto;
import prana.esperCalculator.constants.CollectorConstants;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.main.WhatIfManager;
import prana.esperCalculator.commonCode.*;
import prana.esperCalculator.shell.ShellManager;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;

/**
 * This class handles data initialization process from other servers. It also
 * handles refresh action
 */
public class DataInitializationRequestProcessor {

	/**
	 * Singleton instance
	 */
	private static DataInitializationRequestProcessor _dataProcessor;

	private int _thresholdTimeForMarketSnapshotInSecs = 0;

	/**
	 * Returns the singleton instance of DataInitializationRequestProcessor
	 * 
	 * @return
	 */
	public static DataInitializationRequestProcessor getInstance() {
		if (_dataProcessor == null)
			_dataProcessor = new DataInitializationRequestProcessor();
		return _dataProcessor;
	}

	/**
	 * Private constructor to implement singleton pattern
	 */
	private DataInitializationRequestProcessor() {

		try {
			String requestExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST,
					ConfigurationConstants.KEY_EXCHANGE_BASKET_COMPLIANCE_EXCHANGE);
			_amqpInitializationRequestSender = AmqpHelper.getSender(requestExchangeName, ExchangeType.Direct,
					MediaType.Exchange, false);

			setThresholdValueFromConfig();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Start communication with other server by sending initialization message to
	 * trade server
	 */
	void startCommunicationProcess() {
		try {
			setInitializationFlag(true);
			PranaLogManager.info("Sending Initialization Information to Trade Server");
			sendInitializationMessage("InitializationRequestFromBasketCompliance", "TradeServer");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Keeps the state of Rule mediator communication so that listeners are attached
	 * only once
	 */
	public boolean _isRuleMediatorInitialized = false;

	/**
	 * Keeps the state of LiveFeed collector so that it got initialized only once
	 */
	private boolean _isLiveFeedCollectorInitialized = false;

	/**
	 * Stores the state whether the refresh is in process
	 */
	private boolean _isRefreshInProcess = false;

	/**
	 * Locker for refresh state boolean object
	 */
	private Object _refreshLockerObject = new Object();

	/**
	 * Stores the state whether the refresh is in process
	 */
	private boolean _isInitializationInProcess = false;

	/**
	 * Locker for refresh state boolean object
	 */
	private Object _initializationLockerObject = new Object();

	/**
	 * Instance of IAmqpSender, used to send initialization request
	 */
	private IAmqpSender _amqpInitializationRequestSender;

	/*
	 * _isBasketServiceStarted
	 */
	public static boolean _isBasketServiceStarted = false;

	/**
	 * Perform action when any initialization info is received
	 * 
	 * @param map
	 */

	public static long lastMarketSnapshotUpdateTime = 0; 
	
	private final ExecutorService executorService = Executors.newFixedThreadPool(10);
	private static final Lock lock = new ReentrantLock();

	private static boolean _isEsperStarted = false;

	SimpleDateFormat _parserSdf = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);

	public void initializationInfoReceived(LinkedHashMap<String, Object> map) {
		DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss.SSS");
		try {
			String infoReceived = map.get("ResponseType").toString();

			switch (infoReceived) {
				case "InitCompleteTradeServerForBasketCompliance":
					PranaLogManager.info("\nInitCompleted from server at: " + dateFormat.format(new Date()));
					sendInitializationRequestToEsper();
					break;
				case "EsperStartedCompletely":
				PranaLogManager.info("\nInitCompleted from Esper at: " + dateFormat.format(new Date()));
				PranaLogManager.info("Data processing started....");
				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { "StartCalculation" },
						"BasketStartEOM");
				//StartNavCalculation when all data is received
				PranaLogManager.info("\nStarting NAV calculation @ " + new Date());
				CEPManager.getEPRuntime().getEventService()
						.sendEventObjectArray(new Object[] { true, "StartNavCalculation" }, "InitComplete");
				PranaLogManager.info("Nav Calculation completed @ " + new Date());
				// if (!getRefreshFlag())
				// startDivisorTimerManager();
				PranaLogManager.info("Sending InitializationRequest to RuleMediatorEngine");
				sendInitializationMessage("InitializationRequestFromBasketCompliance",
						"InitializationRequestForRuleMediator");
				_isEsperStarted = true;
				break;
			case "InitCompleteRuleMediator":
				PranaLogManager.info("InitCompleted from Rule Mediator");
				if (!_isRuleMediatorInitialized) {
					_isRuleMediatorInitialized = true;
					CEPManager.addAllListener();
					RuleManager.configure();
					CEPManager.addLoggingListener();
				}
				if (getRefreshFlag()) {
					PranaLogManager.info("Adding Listener");
					CEPManager.addAllListener();
					RuleManagerCommon.addRuleListener();
					ShellManager.getInstance().reloadCurrentStatements();
					PranaLogManager.info("Refresh Complete");
					CEPManager.addLoggingListener();
					setRefreshFlag(false);
				}
				setInitializationFlag(false);
				int pricingTimeout = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
						ConfigurationConstants.SECTION_APP_SETTINGS,
						ConfigurationConstants.KEY_APP_SETTINGS_PRICING_TIMEOUT));
				CEPManager.setVariableValue("EventResetTimeout", pricingTimeout);
				PranaLogManager.info("Sending current state for rule validation");
				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { true, "Validation" },
						"InitComplete");
				PranaLogManager.info("Rule validation event completed");
				PranaLogManager.info("Initializing LiveFeed Collector");
				if (!_isLiveFeedCollectorInitialized) {
					// AmqpCollectorHelper.initializeLiveFeedCollector();
					_isLiveFeedCollectorInitialized = true;
				}
				_isBasketServiceStarted = true;
				PranaLogManager.info("LiveFeed Collector initialized");
				PranaLogManager.info("Completed at: " + dateFormat.format(new Date()));
				CEPManager.setVariableValue("IsBasketStarted", true);
				PranaLogManager.info("Basket Compliance engine started........\nBasket Compliance>");
				PranaLogManager.logOnly("_isBasketServiceStarted: " + _isBasketServiceStarted);
				break;
			case "InitCompleteInfoServerFailedForBasketCompliance":
				Exception ex = new Exception("Could not initialize from trade server. Kindly check server");
				PranaLogManager.error(ex.getMessage(), ex);
				break;
			case "InitCompleteInfoEsperFailedForBasketCompliance":
				Exception exEx = new Exception("Could not initialize from Esper. Kindly check Esper");
				PranaLogManager.error(exEx.getMessage(), exEx);
				break;
			case "GetAccountNavNStartingValueFromBasket":
				executorService.submit(() -> {
					long threadId = Thread.currentThread().getId();
					lock.lock(); // Acquire the lock
					PranaLogManager
							.info("Processing GetAccountNavNStartingValueFromBasket in THREAD POOL: " + threadId);
					try {
						accountNavNStartingValueReqForPst(map);
					} catch (Exception e) {
						PranaLogManager.error("Error in thread " + threadId, e);
					} finally {
						lock.unlock();
					}
				});
				break;
			case "EsperRunningStatusResponse":
				_isEsperStarted = true;
				PranaLogManager.info("Received Esper running status, setting _isEsperStarted to true.");
				break;
			}

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public void accountNavNStartingValueReqForPst(LinkedHashMap<String, Object> map) throws Exception {
		try {
			long startTime = System.currentTimeMillis();
			PranaLogManager.info("Received AccountNav and starting Value req for PST " + new Date().toString());

			ObjectMapper objectMapper = new ObjectMapper();

			String payload = map.get("payLoad").toString();

			List<AccountNavRequestDto> accountNavRequests = objectMapper.readValue(payload,
					new TypeReference<List<AccountNavRequestDto>>() {
					});

			if (accountNavRequests.isEmpty()) {
				PranaLogManager.warn("Payload is empty for accountNavNStartingValueReqForPst, returning...");
				return;
			}

			AccountNavRequestDto dto = accountNavRequests.get(0);
			String correlationId = dto.getCorrelationId();

			boolean isDataLoaded = (Boolean) CEPManager.getVariableValue(ConfigurationConstants.DATA_LOADED_FOR_PST);

			PranaLogManager.logOnly(
					"Received Accout nav & Starting Value request (PST), Fetching data from Esper Engine for PST request with correlationId:"
							+ correlationId + ", and isDataLoaded flag:" + isDataLoaded);

			boolean isFreshMarketSnapshotReq = isMarketSnapshotFromEsperRequired();

			if (isFreshMarketSnapshotReq) {
				// set DataLoadedForPST to false, as we want sync fresh snapshot
				CEPManager.setVariableWithDebugInfo(ConfigurationConstants.DATA_LOADED_FOR_PST, false);
				getMarketSnapshotFromEsper();

				int sendTime = 0, sleep = 100;
				// Simulated wait for "DataLoadedForPST" flag
				while (!(Boolean) CEPManager.getVariableValue(ConfigurationConstants.DATA_LOADED_FOR_PST)) {
					sendTime += sleep;
					Thread.sleep(sleep);
					CEPManager.notifyIfTimerExceedsLimit(sendTime);
				}

				if (sendTime > 0)
					PranaLogManager.info("Received data from Esper in " + ((double) (sendTime / 1000))
							+ " seconds for correlationId:" + correlationId);
			}

			updateIntermediateCacheToTaxLotWindow(accountNavRequests);

			// Need to set limit price as user can set price in market order too.
			if (dto.getPrice() > 0)
				updateLimitPriceOfSymbol(dto.getSymbol(), dto.getUnderlyingSymbol(), dto.getPrice());
			
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { 0 },
					"StartNavCalculationsForBasket");
			
			processAccountNSymbolQueries(accountNavRequests, correlationId);

			LinkedHashMap<String, Object> list = new LinkedHashMap<String, Object>();
			list.put("Data", accountNavRequests);
			list.put("CorrelationId", correlationId);

			HashMap<String, Object> objList = new HashMap<>();
			objList.put("Data", accountNavRequests);

			String logRespMsg = "";
			for (AccountNavRequestDto obj : accountNavRequests) {
				logRespMsg = logRespMsg + '\n' + "AccountId:" + obj.getAccountId() + ", Symbol:" + obj.getSymbol()
						+ ", startValue:" + obj.getStartValue() + ", NAV:" + obj.getAccountNav() + ", position:"
						+ obj.getStartingPosition();
			}

			long endTime = System.currentTimeMillis();
			String msg = "Complete Account NAV,Symbol Exposure and position has been executed in "+(endTime - startTime) +"ms for correlationId: "
					+ correlationId;
			PranaLogManager.info((msg));
			PranaLogManager.logOnly(msg + "\n" + "Response to be send:" + logRespMsg);

			// To set DataLoadedForPST to false, for next request.
			CEPManager.setVariableWithDebugInfo(ConfigurationConstants.DATA_LOADED_FOR_PST, false);

			_amqpInitializationRequestSender.sendData(JSONMapper.getStringForObject(objList),
					"NavAndStartingPositionOfAccountsResponse");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		} finally {
			CEPManager.setVariableWithDebugInfo(ConfigurationConstants.DATA_LOADED_FOR_PST, false);
		}
	}

	private void updateLimitPriceOfSymbol(String symbol, String underLyingSymbol, double price) {
		EPFireAndForgetQueryResult result = getSymbolData(symbol, underLyingSymbol);
		long startTime = System.currentTimeMillis();

		if (result != null && result.getArray() != null && result.getArray().length > 0) {
			EventBean eventBean = result.getArray()[0];
			double askPrice = (double) eventBean.get("askPrice");
			double bidPrice = (double) eventBean.get("bidPrice");
			double lowPrice = (double) eventBean.get("lowPrice");
			double highPrice = (double) eventBean.get("highPrice");
			double openPrice = (double) eventBean.get("openPrice");
			double closePrice = (double) eventBean.get("closePrice");
			String conversionMethod = (String) eventBean.get("conversionMethod");
			double markPrice = (double) eventBean.get("markPrice");
			double delta = (double) eventBean.get("delta");
			double beta5YearMonthly = (double) eventBean.get("beta5YearMonthly");
			int assetId = (int) eventBean.get("assetId");
			double openInterest = (double) eventBean.get("openInterest");
			double avgVolume20Days = (double) eventBean.get("avgVolume20Days");
			Object sharesOutstanding = (Object) eventBean.get("sharesOutstanding");

			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { symbol, underLyingSymbol, askPrice, bidPrice, lowPrice, highPrice, openPrice,
							closePrice, price, price, conversionMethod, markPrice, delta, beta5YearMonthly, assetId,
							openInterest, avgVolume20Days, sharesOutstanding },
					CollectorConstants.SYMBOL_DATA_EVENT_NAME);
		} else {
			PranaLogManager.logOnly("No data found in SymbolDataWindow for the given filters.");
		}

		long endTime = System.currentTimeMillis();
		PranaLogManager.logOnly("Time taken to execute updateLimitPriceOfSymbol for PST: " + (endTime - startTime)
				+ " ms for symbol:" + symbol + ",price:" + price);
	}

	/**
	 * Processes two queries to retrieve and update account navigation data: 1.
	 * Updates NAV (`accountNav`) values for accounts from the first query. 2.
	 * Updates the start value (`netExposureBase`) for accounts using the second
	 * query.
	 * 
	 * @param navQuery
	 *            The EPL query to retrieve NAV data.
	 * @param exposureQuery
	 *            The EPL query to retrieve net exposure base data.
	 * @param correlationId
	 *            The unique identifier for the request, used for logging and
	 *            tracing.
	 * @param list
	 *            List of account navigation requests to be updated.
	 */
	private void processAccountNSymbolQueries(List<AccountNavRequestDto> list, String correlationId) {
		AccountNavRequestDto dto = list.get(0);

		String accountIds = list.stream().map(account -> String.valueOf(account.getAccountId()))
				.collect(Collectors.joining(", "));

		String whereQuery = "WHERE accountId in (" + accountIds + ") AND symbol='" + dto.getSymbol() + "'";

		String navQuery = "SELECT accountId, shadowNav FROM AccountDivisorWindow WHERE accountId in (" + accountIds
				+ ") ";

		PranaLogManager.logOnly("Executing account NAV query: " + navQuery + "; for correlationId: " + correlationId);

		EPFireAndForgetQueryResult navResult = CEPManager.executeQuery(navQuery);

		if (navResult != null && navResult.getArray() != null && navResult.getArray().length > 0) {
			for (EventBean eventBean : navResult.getArray()) {
				int accountId = (int) eventBean.get("accountId");
				double shadowNav = (double) eventBean.get("shadowNav");

				list.stream().filter(request -> request.getAccountId() == accountId)
						.forEach(request -> request.setAccountNav(shadowNav));

				PranaLogManager.debug("Updated NAV for accountId: " + accountId + " to: " + shadowNav
						+ ", correlationId: " + correlationId);
			}
		} else {
			PranaLogManager.logOnly("No results returned for NAV query for correlationId: " + correlationId);
		}

		// Process starting position query for Equity
		String positionQuery = "SELECT symbol, accountId, Sum(coalesce(quantity, 0)) as quantity, MAX(selectedFeedPriceBase) as selectedFeedPriceBase, MAX(fxConversionMethod) as fxConversionMethod,"
				+ " MAX(currentFxRate) as fxRate, SUM(coalesce(netMarketValueBase, 0)) as netExposureBase FROM RowCalculationBaseWindow " + whereQuery
				+ " AND (CASE WHEN PostWithInMarketInStage = 2 THEN (taxlotType in ('Post', 'InTradeMarket')) "
				+ " WHEN PostWithInMarketInStage = 3 THEN taxlotType <> 'WhatIf' ELSE taxlotType = 'Post' END) AND assetId = 1 "
				+ " GROUP BY accountId, symbol, underlyingSymbol";

		PranaLogManager
				.logOnly("Executing symbol position query: " + positionQuery + "; correlationId: " + correlationId);
		EPFireAndForgetQueryResult positionResult = CEPManager.executeQuery(positionQuery);

		if (positionResult != null && positionResult.getArray() != null && positionResult.getArray().length > 0) {
			for (EventBean eventBean : positionResult.getArray()) {
				int accountId = (int) eventBean.get("accountId");
				double quantity = (double) eventBean.get("quantity");
				double selectedFeedPriceBase = (double) eventBean.get("selectedFeedPriceBase");

				double fxRate = (double) eventBean.get("fxRate");
				String fxOperator = (String) eventBean.get("fxConversionMethod");
				double netExposureBase = (double) eventBean.get("netExposureBase");

				list.stream().filter(request -> request.getAccountId() == accountId).forEach(request -> {
					request.setStartingPosition(quantity);
					request.setSelectedFeedPriceBase(selectedFeedPriceBase);

					request.setFxOperator(fxOperator);
					request.setFxRate(fxRate);
					request.setStartValue(netExposureBase);
				});

				PranaLogManager.debug("Updated startingPosition for accountId: " + accountId + " to: " + quantity
						+ ", correlationId: " + correlationId);
			}
		} else {
			PranaLogManager.logOnly("No results returned for position query; correlationId: " + correlationId);
		}
	}

	/**
	 * Constructs an EPL (Event Processing Language) query to retrieve
	 * symbol-related data from the SymbolDataWindow. The query filters data based
	 * on the provided `symbol` and `underlyingSymbol`.
	 * 
	 * @param symbol
	 *            The symbol whose data needs to be retrieved.
	 * @param underlyingSymbol
	 *            The underlying symbol associated with the primary symbol.
	 * @return A formatted EPL query string.
	 */
	private EPFireAndForgetQueryResult getSymbolData(String symbol, String underlyingSymbol) {
		long startTime = System.currentTimeMillis();
		String query = String.format(
				"SELECT symbol, underlyingSymbol, askPrice, bidPrice, lowPrice, highPrice, openPrice, closePrice, "
						+ "lastPrice, selectedFeedPrice, conversionMethod, markPrice, delta, beta5YearMonthly, assetId, "
						+ "openInterest, avgVolume20Days, avgVolume, sharesOutstanding, parentWindowStream "
						+ "FROM SymbolDataWindow " + "WHERE symbol = '%s' LIMIT 1",
				symbol, underlyingSymbol);

		EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);

		long endTime = System.currentTimeMillis();
		long diff = endTime - startTime;
		PranaLogManager.logOnly("Execution of GetSymbolQuery to update limit price is completed in " + diff
				+ " ms and the query is: " + query);
		return result;
	}

	private void updateIntermediateCacheToTaxLotWindow(List<AccountNavRequestDto> accountNavRequests) {
		Set<Integer> accountIdSet = new HashSet<>();
		Set<String> symbolSet = new HashSet<>();

		for (AccountNavRequestDto accountNavReq : accountNavRequests) {
			accountIdSet.add(accountNavReq.getAccountId());
			symbolSet.add(accountNavReq.getSymbol());
		}

		//Fetch taxlot data from the cache
		HashMap<String, HashMap<String, Object>> taxlotList = TaxlotCache.getInstance()
				.getTaxlots(new ArrayList<>(accountIdSet), new ArrayList<>(symbolSet));

		//Send from intermediate taxlot of that account to BasketManager
		BasketManager.getInstance().sendSnapshotTaxlotsData(taxlotList, "PST");
	}

	/**
	 * This send the initialization message to given routing key
	 * 
	 * @param typeOfRequest
	 * @param routingKey
	 */
	public void sendInitializationMessage(String typeOfRequest, String routingKey) {
		try {
			HashMap<String, Object> initializationInformationMap = new HashMap<>();
			initializationInformationMap.put("TypeOfRequest", typeOfRequest);
			String initializationInformation = JSONMapper.getStringForObject(initializationInformationMap);

			_amqpInitializationRequestSender.sendData(initializationInformation, routingKey);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public void sendRequestWithMessage(String message, String routingKey) {
		try {
			_amqpInitializationRequestSender.sendData(message, routingKey);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * this set the refresh flag. true when refresh starts and false when ends
	 * 
	 * @param value
	 */
	private void setRefreshFlag(boolean value) {
		synchronized (_refreshLockerObject) {
			_isRefreshInProcess = value;
		}
	}

	/**
	 * Return the current state of refresh
	 * 
	 * @return
	 */
	private boolean getRefreshFlag() {
		boolean temp;
		synchronized (_refreshLockerObject) {
			temp = _isRefreshInProcess;
		}
		return temp;
	}

	/**
	 * @return the _isInitializationInProcess
	 */
	public boolean getInitializationFlag() {
		synchronized (_initializationLockerObject) {
			return _isInitializationInProcess;
		}
	}

	/**
	 * @param _isInitializationInProcess
	 *            the _isInitializationInProcess to set
	 */
	public void setInitializationFlag(boolean value) {
		synchronized (_initializationLockerObject) {
			this._isInitializationInProcess = value;
		}
	}

	private boolean isMarketSnapshotFromEsperRequired() {
		try {
			long currentTime = System.currentTimeMillis();
			long lastUpdateTime = DataInitializationRequestProcessor.lastMarketSnapshotUpdateTime;

			long thresholdInMillis = _thresholdTimeForMarketSnapshotInSecs * 1000;

			if (currentTime - lastUpdateTime > thresholdInMillis) {
				PranaLogManager.logOnly("Fetching fresh market snapshot from esper as the threshold time of "
						+ _thresholdTimeForMarketSnapshotInSecs + " seconds has been exceeded.");
				return true; // Fresh market snap from esper required
			}

			PranaLogManager.logOnly("Skipping fresh market snapshot update from esper as the threshold time of "
					+ _thresholdTimeForMarketSnapshotInSecs + " seconds has not been exceeded.");
			return false; // Fresh data not required

		} catch (Exception ex) {
			PranaLogManager.error("Error determining if market snapshot is required", ex);
			return true; // error, go for fresh market data
		}
	}

	private void getMarketSnapshotFromEsper() {
		PranaLogManager.info("Sending Post data request to Esper Engine for PST at: " + _parserSdf.format(new Date()));
		DataInitializationRequestProcessor.getInstance().sendRequestWithMessage("Data_PST", "EsperPostData");
	}

	/*
	 * A threshold value is time interval at which if pst request come, then fresh
	 * market snshot fr esper is not required.
	 */
	private void setThresholdValueFromConfig() throws Exception {
		_thresholdTimeForMarketSnapshotInSecs = 0;
		try {
			String value = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_THRESHOLD_TIME_INTERVAL_FOR_MARKET_SNAPSHOT);

			if (value != null && !value.trim().isEmpty()) {
				_thresholdTimeForMarketSnapshotInSecs = Integer.parseInt(value.trim());
			} else {
				PranaLogManager.warn(
						"Configuration value for threshold PST market snapshot request is missing or empty. Defaulting to 0.");
			}

		} catch (NumberFormatException ex) {
			PranaLogManager.error("Error parsing AMQP Server configuration value to long. Defaulting to 0.", ex);
		}
	}

	private void sendInitializationRequestToEsper() {
		//wait for Esper to start in separate thread,so as not to block main thread
		executorService.submit(() -> {
			try {
				while (!_isEsperStarted) {
					PranaLogManager.info("Waiting for Esper to start... ");
					DataInitializationRequestProcessor.getInstance().sendRequestWithMessage("IsEsperRunning", "EsperRunningStatus");
					Thread.sleep(3000);
				}
				PranaLogManager.info("Sending InitializationRequest to Esper");
				//this will be sent InitializationRequestFromBasketCompliance only once Esper is started
				//after which esper will send back EsperStartedCompletely message
				sendInitializationMessage("InitializationRequestFromBasketCompliance",
						"InitializationRequestForEsper");
			} catch (Exception e) {
				PranaLogManager.error("Error in while loop when sending InitializationRequest to Esper", e);
			}
		});
	}
}
