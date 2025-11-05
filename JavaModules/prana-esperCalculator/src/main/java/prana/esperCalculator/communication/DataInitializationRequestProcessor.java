package prana.esperCalculator.communication;

import java.io.File;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedHashMap;
import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.customRule.RuleManager;
import prana.esperCalculator.commonCode.*;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotState;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotType;
import prana.esperCalculator.esperCEP.AdviseSymbolHelper;
import prana.esperCalculator.main.DivisorTimerManager;
import prana.esperCalculator.main.PendingWhatIfCache;
import prana.esperCalculator.objects.Taxlot;
import prana.esperCalculator.shell.ShellManager;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;
import com.fasterxml.jackson.core.type.TypeReference;
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

	/**
	 * Added delay of 3 Seconds while sending taxlot Symbol data to Esper.
	 * SleepPostOnWhatIf is configurable, you can change delay time in config file.
	 */
	int _sleepPostOnWhatIf = 3000;
	
	/**
	 * Symbol data timer value set in Pricing server.
	 */
	public int symbolDataTimerIntervalFromPricingServer = 0;
	
	/**
	 * List of Fx and FxForward symbols using userPX from Pricing Inputs.
	 */
	public ArrayList<String> symbolsFXForwards = new ArrayList<>();

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
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_ESPER_REQUEST);
			_amqpInitializationRequestSender = AmqpHelper.getSender(requestExchangeName, ExchangeType.Direct,
					MediaType.Exchange, false);

			parserSDF = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);

			_sleepPostOnWhatIf = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_SLEEP_POST_ON_WHAT_IF)) * 1000;
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
			PranaLogManager.info("Sending InitializationInformation to server");
			AdviseSymbolHelper.getInstance().resetAdviseList();
			sendInitializationMessage("InitializationRequest", "TradeServer");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Start refresh data sequence
	 */
	public boolean refreshData() {
		try {

			if (!getRefreshFlag()) {
				setRefreshFlag(true);
				CEPManager.setVariableValue("DataLoaded", false);
				CEPManager.setVariableValue("IsEsperStarted", false);
				_isEsperStarted=false;
				RuleManagerCommon.removeAllListener();
				CEPManager.removeStatementListener();
				// CEPManager.removeDetectonListener();

				EPFireAndForgetQueryResult result = CEPManager
						.executeQuery("select taxlotId as taxlotId from TaxlotWindow");
				_taxlotIdCollection.clear();
				for (EventBean event : result.getArray())
					_taxlotIdCollection.add(event.get("taxlotId").toString());
				
				PranaLogManager.info("Clearing TaxlotWindow, AggregationTaxlotWindow , RowCalculationBaseWindow and ExtendedAccountSymbolWithNav.");
				CEPManager.executeQuery("Delete from TaxlotWindow");
				CEPManager.executeQuery("Delete from AggregationTaxlotWindow");
				CEPManager.executeQuery("Delete from RowCalculationBaseWindow");
				
				//To-Do Added these logs for Bug-62117, will remove it after case validation
				EPFireAndForgetQueryResult countResult = CEPManager.executeQuery("select count(*) as cnt from ExtendedAccountSymbolWithNav");
				long remainingCount=0;
				if (countResult != null && countResult.getArray().length > 0 && countResult.getArray()[0].get("cnt")!=null) {
				 remainingCount = (long) countResult.getArray()[0].get("cnt");
				}
				PranaLogManager.info("Rows in ExtendedAccountSymbolWithNav before deletion: "+ remainingCount);
				
				CEPManager.executeQuery("Delete from ExtendedAccountSymbolWithNav");
				
				long remainingCount2=0;
				EPFireAndForgetQueryResult countResult2 = CEPManager.executeQuery("select count(*) as cnt from ExtendedAccountSymbolWithNav");
				if (countResult2 != null && countResult2.getArray().length > 0 && countResult2.getArray()[0].get("cnt")!=null) {
				remainingCount2 = (long) countResult2.getArray()[0].get("cnt");
				}
				PranaLogManager.info("Remaining rows in ExtendedAccountSymbolWithNav after deletion: "+ remainingCount2);
				startCommunicationProcess();
				return true;
			} else {
				PranaLogManager.info("Refresh already in progress.");
				return false;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	/**
	 * Keeps the state of Rule mediator communication so that listeners are attached
	 * only once
	 */
	public boolean _isRuleMediatorInitialized = false;

	/*
	 * State for Esper, if started completely then we need to send reponse to
	 * BasketCompliance service.
	 */
	public boolean _isEsperStarted = false;

	/**
	 * Keeps the state of LiveFeed collector so that it got initialized only once
	 */
	private boolean _isLiveFeedCollectorInitialized = false;

	/**
	 * Stores the collection of taxlot id so that it can be used while performing
	 * refresh
	 */
	private ArrayList<String> _taxlotIdCollection = new ArrayList<String>();

	/**
	 * Stores the collection of intrade trades so that they can be used during
	 * refresh
	 */
	private LinkedHashMap<String, HashMap<String, Object>> _inTradeCollection = new LinkedHashMap<>();

	/**
	 * Stores the collection of instage trades so that they can be used during
	 * refresh
	 */
	private LinkedHashMap<String, HashMap<String, Object>> _inStageCollection = new LinkedHashMap<>();

	/**
	 * Counter for historical taxlot received
	 */
	private long _historicalCounter = 0;

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
	 * Instance of IAmqpSender, used to send initialization request for Basket
	 * Compliance
	 */
	private IAmqpSender _basketComplianceExchangeName = null;

	/*
	 * Instance of IAmqpSender, used to send initialization request for Basket
	 * Compliance
	 */
	private IAmqpSender _basketComplianceSymbolExchangeName = null;
	
	/*
	 * Instance of IAmqpSender, used to send data for RTPNL
	 */
	private IAmqpSender _rtpnlExchangeName = null;

	/**
	 * For parsing date
	 */
	private SimpleDateFormat parserSDF;

	/**
	 * Perform action when any initialization info is received
	 * 
	 * @param map
	 */
	public void initializationInfoReceived(HashMap<String, Object> map) {
		DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss.SSS");
		try {
			String infoReceived = map.get("ResponseType").toString();
			switch (infoReceived) {
				case "InitCompleteTradeServer":
					PranaLogManager.info("\nInitCompleted from server at: " + dateFormat.format(new Date()));
					PranaLogManager.info("\nSending InitializationRequest to Pricing");
					sendInitializationMessage("InitializationRequest", "Pricing");
					break;
				case "InitCompleteExPnL":
					PranaLogManager.info("\nInitCompleted from ExPnL @ " + new Date());

					PranaLogManager.info("\nStarting Data loading @ " + new Date());
					CEPManager.setVariableValue("DataLoaded", true);
					PranaLogManager.info("Data loaded @ " + new Date());

					PranaLogManager.info("\nStarting NAV calculation @ " + new Date());
				CEPManager.getEPRuntime().getEventService()
						.sendEventObjectArray(new Object[] { true, "StartNavCalculation" }, "InitComplete");
				PranaLogManager.info("Nav Calculation completed @ " + new Date());

				long currentTime = CEPManager.getEPRuntime().getEventService().getCurrentTime();
				CEPManager.getEPRuntime().getEventService().advanceTime(currentTime);

				PranaLogManager.info("\nStarting Aggregation @ " + new Date());
				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { true, "DataLoaded" },
						"InitComplete");
				PranaLogManager.info("Aggregation done @ " + new Date());

				PranaLogManager.info("\nStarting divisor manager thread @ " + new Date());
					if (!getRefreshFlag())
						startDivisorTimerManager();
					PranaLogManager.logOnly("\nTotal historical taxlots count: " + _historicalCounter);
					PranaLogManager.info("Sending InitializationRequest to RuleMediatorEngine");
					sendInitializationMessage("InitializationRequest", "RuleMediatorEngine");
					_historicalCounter = 0;
					break;
				case "InitCompletePricing":
					PranaLogManager.info("InitCompleted from Pricing");
					PranaLogManager.info("\nSending InitializationRequest to ExPnL");
					sendInitializationMessage("InitializationRequest", "ExPnL");
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
					CEPManager.addLoggingListener();
					PranaLogManager.info("Refresh Complete");
					setRefreshFlag(false);
				}
				setInitializationFlag(false);
				PranaLogManager.info("Sending current state for rule validation");
				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { true, "Validation" },
						"InitComplete");
				AmqpCollectorHelper.initializeSecurityDetailsCollector();
				AmqpCollectorHelper.initializeValidatedSymbolDataCollector();
				PranaLogManager.info("Rule validation event completed");
				PranaLogManager.info("Initializing LiveFeed Collector");
				if (!_isLiveFeedCollectorInitialized) {
					AmqpCollectorHelper.initializeLiveFeedCollector();
					_isLiveFeedCollectorInitialized = true;
				}
				PranaLogManager.info("LiveFeed Collector initialized");
				sendInitializationMessage("EsperStartedCompletely", "ExPnL");
				sendInitializationMessage("EsperStartedCompletely", "Pricing");
				CEPManager.setVariableValue("IsEsperStarted", true);
				PranaLogManager.info("Data processing started....");
				PranaLogManager.logOnly("IsEsperStarted : " + CEPManager.getVariableValue("IsEsperStarted"));
				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { "StartCalculation" },
						"EsperStartEOM");
				int pricingTimeout = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
						ConfigurationConstants.SECTION_APP_SETTINGS,
						ConfigurationConstants.KEY_APP_SETTINGS_PRICING_TIMEOUT));
				CEPManager.setVariableValue("EventResetTimeout", pricingTimeout);

				// Send initCompleteInfo to BasketCompliance service
				_isEsperStarted = true;
				if (_basketComplianceExchangeName == null
						&& (boolean) CEPManager.getVariableValue(ConfigurationConstants.IS_BASKETCOMPLIANCE_ENABLED)) {
					String basketComplianceExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_BASKET_COMPLIANCE_EXCHANGE);
					_basketComplianceExchangeName = AmqpHelper.getSender(basketComplianceExchangeName,
							ExchangeType.Direct, MediaType.Exchange, false);

					String basketComplianceSymbolExchangeName = ConfigurationHelper.getInstance()
							.getValueBySectionAndKey(ConfigurationConstants.SECTION_EXCHANGE_LIST,
									ConfigurationConstants.KEY_EXCHANGE_BASKET_COMPLIANCE_SYMBOL_EXCHANGE);
					_basketComplianceSymbolExchangeName = AmqpHelper.getSender(basketComplianceSymbolExchangeName,
							ExchangeType.Direct, MediaType.Exchange, false);

					CommunicationManager.initializeBasketComplianceRequestExchange();
					// startBasketComplianceService();
					sendInitCompleteToBasketCompliance();
				}
				PranaLogManager.info("Completed at: " + dateFormat.format(new Date()));
				PranaLogManager.info("Esper calculation engine started........\nEsper>");
				
				//Sending request to calculation service to check availability.
				if (_rtpnlExchangeName == null) {
					String rtpnlExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_RTPNL_COMPRESSIONS_EXCHANGE);
					_rtpnlExchangeName = AmqpHelper.getSender(rtpnlExchangeName, ExchangeType.Direct,
							MediaType.Exchange, false);
					sendCommunicationRequestToRTPNL();
				}
				break;
			case "InitCompleteInfoServerFailed":
				Exception ex = new Exception("Could not initialize from trade server. Kindly check server");
				PranaLogManager.error(ex.getMessage(), ex);
				break;
				case "InitCompleteInfoExPnLFailed":
					Exception exEx = new Exception("Could not initialize from ExPnL. Kindly check ExPnL");
					PranaLogManager.error(exEx.getMessage(), exEx);
					break;
				case "InitCompleteInfoPricingFailed":
					Exception exPr = new Exception("Could not initialize from Pricing server. Kindly check Pricing");
					PranaLogManager.error(exPr.getMessage(), exPr);
				break;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/*
	 * Initializes the exchange for RTPNL
	 */
	public void initializationRequestReceived(HashMap<String, Object> map) {
		DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss.SSS");
		try {
			String infoReceived = map.get("ResponseType").toString();
			if (infoReceived.equals("InitRequestCalculationService")) {
				PranaLogManager.info("Initialization request received from Calculation service at: "
						+ dateFormat.format(new Date()));
				if (_rtpnlExchangeName == null) {
					String rtpnlExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							ConfigurationConstants.KEY_EXCHANGE_RTPNL_COMPRESSIONS_EXCHANGE);
					_rtpnlExchangeName = AmqpHelper.getSender(rtpnlExchangeName, ExchangeType.Direct,
							MediaType.Exchange, false);
				}
				sendStartupDataForRTPNL("startup");
			} else if (infoReceived.equals("CommunicationResponseForEsper")) {
				sendStartupDataForRTPNL("requested");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public void startBasketComplianceService() {
		try {
			String path;
			String basketService = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_BASKET);
			String esperService = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_ESPER);
			String releaseModeBasket = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_RELEASE_MODE_BASKET);
			String releaseModeEsper = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_RELEASE_MODE_ESPER);

			String f = new File(ConfigurationConstants.APPLICTION_CONF_PATH_BASKET_BATCH).getAbsolutePath();
			if (f.contains(esperService + "\\Export"))
				path = f.replaceFirst(esperService, basketService);
			else if (f.contains(esperService))
				path = f.replaceFirst(esperService, basketService + "\\\\Export");
			else
				path = f.replaceFirst(releaseModeEsper, releaseModeBasket);

			Runtime.getRuntime().exec("cmd.exe /C Start \"\"  \"" + path + "\"");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/*
	 * Sends the startup data for RTPNL
	 */
	public void sendStartupDataForRTPNL(String message) {
		try {
			if (_rtpnlExchangeName != null) {
				PranaLogManager.info("Sending " + message + " data to Calculation service.");
				
				//RowCalculation
				EventBean[] eventBeanArray5 = getWindowDataFrom("ExtendedAccountSymbolWithNav");
				for (EventBean eventBean5 : eventBeanArray5) {
					_rtpnlExchangeName.sendData(JSONMapper.getStringForObject(eventBean5.getUnderlying()), "RowCalculationBaseWithNavStartupData");
				}
				
				//Final initialization response
				sendCompletionInfoToRtpnl("DataSentFromEsper", "InitResponseCalculationService");
				if (message.equalsIgnoreCase("startup"))
					PranaLogManager.info("Initialization complete for Calculation service.");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/*
	 * Sending request to calculation service to check availability.
	 */
	public void sendCommunicationRequestToRTPNL() {
		try {
			if (_rtpnlExchangeName != null)
				sendCompletionInfoToRtpnl("CommunicationRequestFromEsper", "CommunicationRequestFromEsper");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public void sendDataForBasketCompliance(String typeOfRequest, String routingKey) {
		try {
			if (_basketComplianceExchangeName != null)
				_basketComplianceExchangeName.sendData(typeOfRequest, routingKey);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public void sendSymbolDataForBasketCompliance(String typeOfRequest, String routingKey) {
		try {
			if (_basketComplianceSymbolExchangeName != null) {
				_basketComplianceSymbolExchangeName.sendData(typeOfRequest, routingKey);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public void sendDeletedDataForBasketCompliance(HashMap<String, Object> taxlot, String routingKey) {
		try {
			if (_basketComplianceExchangeName != null)
				_basketComplianceExchangeName.sendData(JSONMapper.getStringForObject(taxlot), routingKey);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public void sendInitCompleteToBasketCompliance() {
		try {
			if (_isEsperStarted) {
				SimpleDateFormat parserSDF = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss.SSS");

				sendDataForBasketCompliance("{\"PostWithInMarketInStageValue\":\""
						+ CEPManager.getVariableValue(ConfigurationConstants.KEY_POST_WITH_INMARKET_INSTAGE) + "\","
						+ "\"IsM2MIncludedInCash\":\"" + CEPManager.getVariableValue("IsM2MIncludedInCash") + "\","
						+ "\"CompanyBaseCurrency\":\"" + CEPManager.getVariableValue("CompanyBaseCurrency") + "\","
						+ "\"IsCreditLimitBoxPositionAllowed\":\""
						+ CEPManager.getVariableValue("IsCreditLimitBoxPositionAllowed") + "\","
						+ "\"EquitySwapsMarketValueAsEquity\":\""
						+ CEPManager.getVariableValue("EquitySwapsMarketValueAsEquity") + "\","
						+ "\"CalculateFxGainLossOnForexForwards\":\""
						+ CEPManager.getVariableValue("CalculateFxGainLossOnForexForwards") + "\","
						+ "\"CalculateFxGainLossOnSwaps\":\""
						+ CEPManager.getVariableValue("CalculateFxGainLossOnSwaps") + "\","
						+ "\"SetFxToZero\":\"" + CEPManager.getVariableValue("SetFxToZero") + "\"}", "Permissions");
				// Account Window
				EventBean[] eventBeanArray = getWindowDataFrom("AccountWindow");
				for (EventBean eventBean : eventBeanArray) {
					sendDataForBasketCompliance(JSONMapper.getStringForObject(eventBean.getUnderlying()),
							"AccountCollection");
				}

				// YearlyHolidaysWindow Window
				EventBean[] eventBeanArray1 = getWindowDataFrom("YearlyHolidaysWindow");
				for (EventBean eventBean : eventBeanArray1) {
					DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()), "YearlyHolidaysEvent");
				}

				// WeeklyHolidays Window
				EventBean[] eventBeanArray2 = getWindowDataFrom("WeeklyHolidaysWindow");
				for (EventBean eventBean : eventBeanArray2) {
					DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()), "WeeklyHolidaysEvent");
				}

				// PmCalculationPreference Window
				EventBean[] eventBeanArray3 = getWindowDataFrom("PmCalculationPreferenceWindow");
				for (EventBean eventBean : eventBeanArray3) {
					DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()),
							"PmCalculationPreferenceWindowData");
				}

				// AUEC Window
				EventBean[] eventBeanArray4 = getWindowDataFrom("AuecWindow");
				for (EventBean eventBean : eventBeanArray4) {
					DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()), "AuecDetails");
				}

				// Security Window
				EventBean[] eventBeanArray5 = getWindowDataFrom("SecurityWindow");
				for (EventBean eventBean : eventBeanArray5) {
					DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()), "Security");
				}

				// Strategy Window
				EventBean[] eventBeanArray6 = getWindowDataFrom("StrategyWindow");
				for (EventBean eventBean : eventBeanArray6) {
					DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()), "StrategyCollection");
				}
				
				//DayEndCashAccountWindow
				EventBean[] eventBeanArray7 = DataInitializationRequestProcessor.getInstance()
						.getWindowDataFrom("DayEndCashAccountWindow");
				for (EventBean eventBean : eventBeanArray7) {
					DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()), "DayEndCashAccount");
				}

				//AccountNavPreferenceWindow
				EventBean[] eventBeanArray8 = DataInitializationRequestProcessor.getInstance()
						.getWindowDataFrom("AccountNavPreferenceWindow");
				for (EventBean eventBean : eventBeanArray8) {
					DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()), "AccountNavPreference");
				}

				//AccrualForAccountWindow
				EventBean[] eventBeanArray9 = DataInitializationRequestProcessor.getInstance()
						.getWindowDataFrom("AccrualForAccountWindow");
				for (EventBean eventBean : eventBeanArray9) {
					DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()), "AccrualForAccount");
				}

				//DbNavWindow
				EventBean[] eventBeanArray10 = DataInitializationRequestProcessor.getInstance()
						.getWindowDataFrom("DbNavWindow");
				for (EventBean eventBean : eventBeanArray10) {
					DataInitializationRequestProcessor.getInstance()
							.sendDataForBasketCompliance(JSONMapper.getStringForObject(eventBean.getUnderlying()), "DbNav");
				}
				
				// SymbolData Window
				EventBean[] eventBeanArray12 = DataInitializationRequestProcessor.getInstance()
						.getWindowDataFrom("SymbolDataWindow");
				for (EventBean eventBean : eventBeanArray12) {
					DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()), "SymbolDataWindowDataInit");
				}

				// TaxlotWindow Base Window
				EventBean[] eventBeanArray13 = getWindowDataWithWhere("TaxlotWindow where taxlotType <> 'WhatIf'");
				for (EventBean eventBean : eventBeanArray13) {
					DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
							JSONMapper.getStringForObject(eventBean.getUnderlying()), "TaxlotWindowData");
				}

				PranaLogManager
						.logOnly("\nSending InitCompleteInfo to Basket Compliance at: " + parserSDF.format(new Date()));
				sendDataForBasketCompliance("{\"ResponseType\":\"EsperStartedCompletely\"}", "InitCompleteInfo");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public EventBean[] getWindowDataFrom(String windowName) throws Exception {
		EventBean[] eventBeanArray = null;
		try {
			String query = "Select * from " + windowName;
			EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);
			eventBeanArray = result.getArray();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
		return eventBeanArray;
	}
	
	public EventBean[] getWindowDataWithWhere(String windowNameWithCondition) throws Exception {
		EventBean[] eventBeanArray = null;
		try {
			String query = "Select * from " + windowNameWithCondition;
			EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);
			eventBeanArray = result.getArray();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
		return eventBeanArray;
	}

	private static void startDivisorTimerManager() {
		try {
			Thread T = new Thread(new DivisorTimerManager());
			T.start();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Handles action when refresh data is completed
	 */
	public void handleRefreshComplete() {
		try {
			removeTaxlot();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Removes all tax lots which were present in esper window but deleted from
	 * system
	 */
	private void removeTaxlot() {
		try {
			SimpleDateFormat parserSdf = new SimpleDateFormat("E MMM dd HH:mm:ss z yyyy");
			if (_taxlotIdCollection.size() > 0 || _taxlotIdCollection != null) {
				for (int i = 0; i < _taxlotIdCollection.size(); i++) {
					EPFireAndForgetQueryResult result = CEPManager
							.executeQuery("select * from TaxlotWindow where taxlotId = '"
									+ _taxlotIdCollection.get(i).toString() + "'");
					for (EventBean event : result.getArray()) {
						boolean isSwap = Boolean.parseBoolean(event.get("isSwapped").toString());
						double benchMarkRate = 0.0;
						double differential = 0.0;
						double swapNotional = 0.0;
						double dayCount = 0.0;

						if (isSwap) {
							benchMarkRate = Double.parseDouble(event.get("benchMarkRate").toString());
							differential = Double.parseDouble(event.get("differential").toString());
							swapNotional = Double.parseDouble(event.get("swapNotional").toString());
							dayCount = Double.parseDouble(event.get("dayCount").toString());
						}

						String conversionMethod = event.get("conversionMethodOperator").toString();
						Date aUECLocalDate = parserSdf.parse(event.get("auecLocalDate").toString());
						Date settlementDate = parserSdf.parse(event.get("settlementDate").toString());

						Taxlot removeTaxlotObj = new Taxlot();
						removeTaxlotObj.basketId = TaxlotManager.getStringSafe(event.get("basketId"), "basketId");
						removeTaxlotObj.taxlotId = TaxlotManager.getStringSafe(event.get("taxlotId"), "taxlotId");
						removeTaxlotObj.clOrderId = TaxlotManager.getStringSafe(event.get("clOrderId"), "clOrderId");
						removeTaxlotObj.taxlotType = TaxlotType.Post;
						removeTaxlotObj.taxlotState = TaxlotState.Deleted;
						removeTaxlotObj.symbol = TaxlotManager.getStringSafe(event.get("symbol"), "symbol");
						removeTaxlotObj.underlyingSymbol = TaxlotManager.getStringSafe(event.get("underlyingSymbol"),
								"underlyingSymbol");
						removeTaxlotObj.quantity = TaxlotManager.getDoubleSafe(event.get("quantity"), "quantity");
						removeTaxlotObj.avgPrice = TaxlotManager.getDoubleSafe(event.get("avgPrice"), "avgPrice");
						removeTaxlotObj.orderSideTagValue = TaxlotManager.getStringSafe(event.get("orderSideTagValue"),
								"orderSideTagValue");
						removeTaxlotObj.accountId = TaxlotManager.getIntSafe(event.get("accountId"), "accountId");
						removeTaxlotObj.counterPartyId = TaxlotManager.getIntSafe(event.get("counterPartyId"),
								"counterPartyId");
						removeTaxlotObj.venueId = TaxlotManager.getIntSafe(event.get("venueId"), "venueId");
						removeTaxlotObj.auecLocalDate = aUECLocalDate;
						removeTaxlotObj.settlementDate = settlementDate;
						removeTaxlotObj.userId = TaxlotManager.getStringSafe(event.get("userId"), "userId");
						removeTaxlotObj.strategyId = TaxlotManager.getIntSafe(event.get("strategyId"), "strategyId");
						removeTaxlotObj.orderTypeTagValue = TaxlotManager.getStringSafe(event.get("orderTypeTagValue"),
								"orderTypeTagValue");
						removeTaxlotObj.benchMarkRate = benchMarkRate;
						removeTaxlotObj.differential = differential;
						removeTaxlotObj.swapNotional = swapNotional;
						removeTaxlotObj.dayCount = dayCount;
						removeTaxlotObj.isSwapped = isSwap;
						removeTaxlotObj.avgFxRateForTrade = TaxlotManager.getDoubleSafe(event.get("avgFxRateForTrade"),
								"avgFxRateForTrade");
						removeTaxlotObj.conversionMethodOperator = conversionMethod;
						removeTaxlotObj.tif = "";
						removeTaxlotObj.orderSide = TaxlotManager.getStringSafe(event.get("orderSide"), "orderSide");
						removeTaxlotObj.counterParty = TaxlotManager.getStringSafe(event.get("counterParty"),
								"counterParty");
						removeTaxlotObj.venue = TaxlotManager.getStringSafe(event.get("venue"), "venue");
						removeTaxlotObj.sideMultiplier = TaxlotManager.getIntSafe(event.get("sideMultiplier"),
								"sideMultiplier");
						removeTaxlotObj.orderType = TaxlotManager.getStringSafe(event.get("orderType"), "orderType");
						removeTaxlotObj.underlyingAsset = TaxlotManager.getStringSafe(event.get("underlyingAsset"),
								"underlyingAsset");
						removeTaxlotObj.limitPrice = TaxlotManager.getDoubleSafe(event.get("limitPrice"), "limitPrice");
						removeTaxlotObj.stopPrice = TaxlotManager.getDoubleSafe(event.get("stopPrice"), "stopPrice");
						removeTaxlotObj.isWhatIfTradeStreamRequired = TaxlotManager.getBooleanSafe(
								event.get("isWhatIfTradeStreamRequired"), "isWhatIfTradeStreamRequired");
						removeTaxlotObj.assetId = TaxlotManager.getIntSafe(event.get("assetId"), "assetId");
						removeTaxlotObj.asset = TaxlotManager.getStringSafe(event.get("asset"), "asset");
						removeTaxlotObj.auecId = TaxlotManager.getIntSafe(event.get("auecId"), "auecId");
						removeTaxlotObj.multiplier = TaxlotManager.getDoubleSafe(event.get("multiplier"), "multiplier");
						removeTaxlotObj.tradeAttribute1 = TaxlotManager.getStringSafe(event.get("tradeAttribute1"),
								"tradeAttribute1");
						removeTaxlotObj.tradeAttribute2 = TaxlotManager.getStringSafe(event.get("tradeAttribute2"),
								"tradeAttribute2");
						removeTaxlotObj.tradeAttribute3 = TaxlotManager.getStringSafe(event.get("tradeAttribute3"),
								"tradeAttribute3");
						removeTaxlotObj.tradeAttribute4 = TaxlotManager.getStringSafe(event.get("tradeAttribute4"),
								"tradeAttribute4");
						removeTaxlotObj.tradeAttribute5 = TaxlotManager.getStringSafe(event.get("tradeAttribute5"),
								"tradeAttribute5");
						removeTaxlotObj.tradeAttribute6 = TaxlotManager.getStringSafe(event.get("tradeAttribute6"),
								"tradeAttribute6");

						// Send to CEP
						TaxlotManager.sendTaxlotToCEPEngine(removeTaxlotObj, false);
					}
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * This send the initialization message to given routing key
	 * 
	 * @param typeOfRequest
	 * @param routingKey
	 */
	private void sendInitializationMessage(String typeOfRequest, String routingKey) {
		try {
			HashMap<String, Object> initializationInformationMap = new HashMap<>();
			initializationInformationMap.put("TypeOfRequest", typeOfRequest);
			String initializationInformation = JSONMapper.getStringForObject(initializationInformationMap);
			_amqpInitializationRequestSender.sendData(initializationInformation, routingKey);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/**
	 * This send the message to Rtpnl Exchange on given routing key
	 * 
	 * @param typeOfRequest
	 * @param routingKey
	 */
	private void sendCompletionInfoToRtpnl(String typeOfRequest, String routingKey) {
		try {
			HashMap<String, Object> initializationInformationMap = new HashMap<>();
			initializationInformationMap.put("TypeOfRequest", typeOfRequest);
			String initializationInformation = JSONMapper.getStringForObject(initializationInformationMap);
			_rtpnlExchangeName.sendData(initializationInformation, routingKey);
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
	public boolean getRefreshFlag() {
		boolean temp;
		synchronized (_refreshLockerObject) {
			temp = _isRefreshInProcess;
		}
		return temp;
	}

	/**
	 * Sends the taxlot to esper engine
	 * 
	 * @param jsonReceivedData
	 */
	public void historicalDataReceived(String jsonReceivedData) {
		try {
			LinkedHashMap<String, Object> taxlot = JSONMapper.getLinkedHashMap(jsonReceivedData);

			boolean isSwap = Boolean.parseBoolean(taxlot.get("IsSwapped").toString());
			double benchMarkRate = 0.0;
			double differential = 0.0;
			double swapNotional = 0.0;
			double dayCount = 0.0;

			if (isSwap) {
				// /XXX: Remove SupressWarning
				@SuppressWarnings("unchecked")
				HashMap<String, Object> swapParameters = (HashMap<String, Object>) taxlot.get("SwapParameters");
				benchMarkRate = Double.parseDouble(swapParameters.get("BenchMarkRate").toString());
				differential = Double.parseDouble(swapParameters.get("Differential").toString());
				swapNotional = Double.parseDouble(swapParameters.get("NotionalValue").toString());
				dayCount = Double.parseDouble(swapParameters.get("DayCount").toString());
			}

			String conversionMethod = "M";
			if (taxlot.containsKey("FXConversionMethodOnTradeDate")) {
				int methodId = Integer.parseInt(taxlot.get("FXConversionMethodOnTradeDate").toString());
				conversionMethod = methodId == 1 ? "D" : "M";
			}

			Date aUECLocalDate = parserSDF.parse(taxlot.get("AUECLocalDate").toString());
			Date settlementDate = parserSDF.parse(taxlot.get("SettlementDate").toString());

			_historicalCounter += 1;

			PranaLogManager.info("Historical Taxlot Received: " + _historicalCounter);

			if (getRefreshFlag()) {
				for (int i = 0; i < _taxlotIdCollection.size(); i++) {
					String id = _taxlotIdCollection.get(i).toString();
					if (taxlot.get("ID").equals(id)) {
						_taxlotIdCollection.remove(i);
					}
				}
			}


			Taxlot historicalTaxlot = new Taxlot();
			String taxlotId = TaxlotManager.getStringSafe(taxlot.get("ID"), "ID");
			historicalTaxlot.basketId = taxlotId;
			historicalTaxlot.taxlotId = taxlotId;
			historicalTaxlot.clOrderId = taxlotId;
			historicalTaxlot.taxlotType = TaxlotType.Post;
			historicalTaxlot.taxlotState = TaxlotState.New;
			historicalTaxlot.symbol = TaxlotManager.getStringSafe(taxlot.get("Symbol"), "Symbol");
			historicalTaxlot.underlyingSymbol = TaxlotManager.getStringSafe(taxlot.get("UnderlyingSymbol"),
					"UnderlyingSymbol");
			historicalTaxlot.quantity = TaxlotManager.getDoubleSafe(taxlot.get("Quantity"), "Quantity");
			historicalTaxlot.avgPrice = TaxlotManager.getDoubleSafe(taxlot.get("AvgPrice"), "AvgPrice");
			historicalTaxlot.orderSideTagValue = TaxlotManager.getStringSafe(taxlot.get("OrderSideTagValue"),
					"OrderSideTagValue");
			historicalTaxlot.accountId = TaxlotManager.getIntSafe(taxlot.get("Level1ID"), "Level1ID");
			historicalTaxlot.counterPartyId = TaxlotManager.getIntSafe(taxlot.get("CounterPartyId"), "CounterPartyId");
			historicalTaxlot.venueId = TaxlotManager.getIntSafe(taxlot.get("VenueId"), "VenueId");
			historicalTaxlot.auecLocalDate = aUECLocalDate;
			historicalTaxlot.settlementDate = settlementDate;
			historicalTaxlot.userId = TaxlotManager.getStringSafe(taxlot.get("CompanyUserID"), "CompanyUserID");
			historicalTaxlot.strategyId = TaxlotManager.getIntSafe(taxlot.get("Level2ID"), "Level2ID");
			historicalTaxlot.orderTypeTagValue = TaxlotManager.getStringSafe(taxlot.get("OrderTypeTagValue"),
					"OrderTypeTagValue");
			historicalTaxlot.benchMarkRate = benchMarkRate;
			historicalTaxlot.differential = differential;
			historicalTaxlot.swapNotional = swapNotional;
			historicalTaxlot.dayCount = dayCount;
			historicalTaxlot.isSwapped = isSwap;
			historicalTaxlot.avgFxRateForTrade = TaxlotManager.getDoubleSafe(taxlot.get("FXRateOnTradeDate"),
					"FXRateOnTradeDate");
			historicalTaxlot.conversionMethodOperator = conversionMethod;
			historicalTaxlot.tif = "";
			historicalTaxlot.orderSide = TaxlotManager.getStringSafe(taxlot.get("OrderSide"), "OrderSide");
			historicalTaxlot.counterParty = TaxlotManager.getStringSafe(taxlot.get("CounterPartyName"),
					"CounterPartyName");
			historicalTaxlot.venue = TaxlotManager.getStringSafe(taxlot.get("Venue"), "Venue");
			historicalTaxlot.sideMultiplier = TaxlotManager.getIntSafe(taxlot.get("SideMultiplier"), "SideMultiplier");
			historicalTaxlot.orderType = TaxlotManager.getStringSafe(taxlot.get("OrderType"), "OrderType");
			historicalTaxlot.underlyingAsset = TaxlotManager.getStringSafe(taxlot.get("UnderlyingName"),
					"UnderlyingName");
			historicalTaxlot.limitPrice = TaxlotManager.getDoubleSafe(taxlot.get("LimitPrice"), "LimitPrice");
			historicalTaxlot.stopPrice = TaxlotManager.getDoubleSafe(taxlot.get("StopPrice"), "StopPrice");
			historicalTaxlot.isWhatIfTradeStreamRequired = true; // Always true
			historicalTaxlot.assetId = TaxlotManager.getIntSafe(taxlot.get("Asset"), "Asset");
			historicalTaxlot.asset = TaxlotManager.getStringSafe(taxlot.get("AssetName"), "AssetName");
			historicalTaxlot.auecId = TaxlotManager.getIntSafe(taxlot.get("AUECID"), "AUECID");
			historicalTaxlot.multiplier = TaxlotManager.getDoubleSafe(taxlot.get("Multiplier"), "Multiplier");
			historicalTaxlot.tradeAttribute1 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute1"),
					"TradeAttribute1");
			historicalTaxlot.tradeAttribute2 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute2"),
					"TradeAttribute2");
			historicalTaxlot.tradeAttribute3 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute3"),
					"TradeAttribute3");
			historicalTaxlot.tradeAttribute4 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute4"),
					"TradeAttribute4");
			historicalTaxlot.tradeAttribute5 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute5"),
					"TradeAttribute5");
			historicalTaxlot.tradeAttribute6 = TaxlotManager.getStringSafe(taxlot.get("TradeAttribute6"),
					"TradeAttribute6");

			// Send to CEP
			TaxlotManager.sendTaxlotToCEPEngine(historicalTaxlot, false);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Send the trade to esper or store it incase refresh is going on
	 * 
	 * @param taxlot
	 */
	public void inTradeReceived(HashMap<String, Object> taxlot) {
		try {
			if (getInitializationFlag()) {
				_inTradeCollection.put(taxlot.get("TaxLotID").toString(), taxlot);
			} else {
				int sendTime = 0;
				while (!PendingWhatIfCache.getInstance().isEmpty()) {
					sendTime = sendTime + _sleepPostOnWhatIf;
					Thread.sleep(_sleepPostOnWhatIf);
					CEPManager.notifyIfTimerExceedsLimit(sendTime);
				}
				if (sendTime > 0)
					PranaLogManager.info(taxlot.get("TaxLotID").toString()
							+ ", InTradeMarket taxlot sending was delayed for " + sendTime / 1000 + " seconds");

				sendInTradeTaxlot(taxlot, TaxlotType.InTradeMarket);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Send the trade to esper or store it in case refresh is going on
	 * 
	 * @param taxlot
	 */
	public void inStageReceived(HashMap<String, Object> taxlot) {
		try {
			if (getInitializationFlag())
				_inStageCollection.put(taxlot.get("TaxLotID").toString(), taxlot);
			else {
				int sendTime = 0;
				while (!PendingWhatIfCache.getInstance().isEmpty()) {
					sendTime = sendTime + _sleepPostOnWhatIf;
					Thread.sleep(_sleepPostOnWhatIf);
					CEPManager.notifyIfTimerExceedsLimit(sendTime);
				}
				if (sendTime > 0)
					PranaLogManager.info(taxlot.get("TaxLotID").toString()
							+ ", InTradeStage taxlot sending was delayed for " + sendTime / 1000 + " seconds");

				sendInTradeTaxlot(taxlot, TaxlotType.InTradeStage);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Send all stored trades to esper
	 */
	public void sendInTradeToEsper() {
		try {
			for (String key : _inTradeCollection.keySet())
				sendInTradeTaxlot(_inTradeCollection.get(key), TaxlotType.InTradeMarket);
			_inTradeCollection.clear();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * send all trades to esper
	 */
	public void sendInStageToEsper() {
		try {
			for (String key : _inStageCollection.keySet())
				sendInTradeTaxlot(_inStageCollection.get(key), TaxlotType.InTradeStage);
			_inStageCollection.clear();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Send In Trade taxlots to Esper
	 * 
	 * @param whatIfTaxlot
	 * @param type
	 */
	private void sendInTradeTaxlot(HashMap<String, Object> whatIfTaxlot, TaxlotType type) {
		try {
			SimpleDateFormat parserSdf = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);

			@SuppressWarnings("unchecked")
			HashMap<String, Object> swapParameters = (HashMap<String, Object>) whatIfTaxlot.get("SwapParameters");
			boolean isSwap = TaxlotManager.checkAndLogSwap(whatIfTaxlot.get("IsSwapped"), swapParameters);

			double benchMarkRate = 0.0;
			double differential = 0.0;
			double swapNotional = 0.0;
			double dayCount = 0.0;

			if (isSwap) {
				benchMarkRate = Double.parseDouble(swapParameters.get("BenchMarkRate").toString());
				differential = Double.parseDouble(swapParameters.get("Differential").toString());
				swapNotional = Double.parseDouble(swapParameters.get("NotionalValue").toString());
				dayCount = Double.parseDouble(swapParameters.get("DayCount").toString());
			}

			TaxlotState taxlotState = TaxlotState.None;
			switch (whatIfTaxlot.get("TaxLotState").toString()) {
				case "0":
				case "1":
					taxlotState = TaxlotState.New;
					break;
				case "2":
					taxlotState = TaxlotState.Updated;
					break;
				case "3":
					taxlotState = TaxlotState.Deleted;
					break;
			}

			Taxlot whatIfTaxlotObj = new Taxlot();
			whatIfTaxlotObj.basketId = TaxlotManager.getStringSafe(whatIfTaxlot.get("GroupID"), "GroupID");
			whatIfTaxlotObj.taxlotId = TaxlotManager.getStringSafe(whatIfTaxlot.get("TaxLotID"), "TaxLotID");
			whatIfTaxlotObj.clOrderId = TaxlotManager.getStringSafe(whatIfTaxlot.get("LotId"), "LotId");
			whatIfTaxlotObj.taxlotType = type;
			whatIfTaxlotObj.taxlotState = taxlotState;
			whatIfTaxlotObj.symbol = TaxlotManager.getStringSafe(whatIfTaxlot.get("Symbol"), "Symbol");
			whatIfTaxlotObj.underlyingSymbol = TaxlotManager.getStringSafe(whatIfTaxlot.get("UnderlyingSymbol"),
					"UnderlyingSymbol");
			whatIfTaxlotObj.quantity = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("TaxLotQty"), "TaxLotQty");
			whatIfTaxlotObj.avgPrice = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("AvgPrice"), "AvgPrice");
			whatIfTaxlotObj.orderSideTagValue = TaxlotManager.getStringSafe(whatIfTaxlot.get("OrderSideTagValue"),
					"OrderSideTagValue");
			whatIfTaxlotObj.accountId = TaxlotManager.getIntSafe(whatIfTaxlot.get("Level1ID"), "Level1ID");
			whatIfTaxlotObj.counterPartyId = TaxlotManager.getIntSafe(whatIfTaxlot.get("CounterPartyID"),
					"CounterPartyID");
			whatIfTaxlotObj.venueId = TaxlotManager.getIntSafe(whatIfTaxlot.get("VenueID"), "VenueID");
			whatIfTaxlotObj.auecLocalDate = TaxlotManager.getDateSafe(whatIfTaxlot.get("AUECLocalDate"),
					"AUECLocalDate", parserSdf);
			whatIfTaxlotObj.settlementDate = TaxlotManager.getDateSafe(whatIfTaxlot.get("SettlementDate"),
					"SettlementDate", parserSdf);
			whatIfTaxlotObj.userId = TaxlotManager.getStringSafe(whatIfTaxlot.get("CompanyUserID"), "CompanyUserID");
			whatIfTaxlotObj.strategyId = TaxlotManager.getIntSafe(whatIfTaxlot.get("Level2ID"), "Level2ID");
			whatIfTaxlotObj.orderTypeTagValue = TaxlotManager.getStringSafe(whatIfTaxlot.get("OrderTypeTagValue"),
					"OrderTypeTagValue");
			whatIfTaxlotObj.benchMarkRate = benchMarkRate;
			whatIfTaxlotObj.differential = differential;
			whatIfTaxlotObj.swapNotional = swapNotional;
			whatIfTaxlotObj.dayCount = dayCount;
			whatIfTaxlotObj.isSwapped = isSwap;
			whatIfTaxlotObj.avgFxRateForTrade = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("FXRate"), "FXRate");
			whatIfTaxlotObj.conversionMethodOperator = "M";
			whatIfTaxlotObj.tif = TaxlotManager.getStringSafe(whatIfTaxlot.get("TIF"), "TIF");
			whatIfTaxlotObj.orderSide = TaxlotManager.getStringSafe(whatIfTaxlot.get("OrderSide"), "OrderSide");
			whatIfTaxlotObj.counterParty = TaxlotManager.getStringSafe(whatIfTaxlot.get("CounterPartyName"),
					"CounterPartyName");
			whatIfTaxlotObj.venue = TaxlotManager.getStringSafe(whatIfTaxlot.get("Venue"), "Venue");
			whatIfTaxlotObj.sideMultiplier = TaxlotManager.getIntSafe(whatIfTaxlot.get("SideMultiplier"),
					"SideMultiplier");
			whatIfTaxlotObj.orderType = TaxlotManager.getStringSafe(whatIfTaxlot.get("OrderType"), "OrderType");
			whatIfTaxlotObj.underlyingAsset = TaxlotManager.getStringSafe(whatIfTaxlot.get("UnderlyingName"),
					"UnderlyingName");
			whatIfTaxlotObj.limitPrice = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("LimitPrice"), "LimitPrice");
			whatIfTaxlotObj.stopPrice = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("StopPrice"), "StopPrice");
			whatIfTaxlotObj.isWhatIfTradeStreamRequired = true;
			whatIfTaxlotObj.assetId = TaxlotManager.getIntSafe(whatIfTaxlot.get("AssetID"), "AssetID");
			whatIfTaxlotObj.asset = TaxlotManager.getStringSafe(whatIfTaxlot.get("AssetName"), "AssetName");
			whatIfTaxlotObj.auecId = TaxlotManager.getIntSafe(whatIfTaxlot.get("AUECID"), "AUECID");
			whatIfTaxlotObj.multiplier = TaxlotManager.getDoubleSafe(whatIfTaxlot.get("ContractMultiplier"),
					"ContractMultiplier");
			whatIfTaxlotObj.tradeAttribute1 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute1"),
					"TradeAttribute1");
			whatIfTaxlotObj.tradeAttribute2 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute2"),
					"TradeAttribute2");
			whatIfTaxlotObj.tradeAttribute3 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute3"),
					"TradeAttribute3");
			whatIfTaxlotObj.tradeAttribute4 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute4"),
					"TradeAttribute4");
			whatIfTaxlotObj.tradeAttribute5 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute5"),
					"TradeAttribute5");
			whatIfTaxlotObj.tradeAttribute6 = TaxlotManager.getStringSafe(whatIfTaxlot.get("TradeAttribute6"),
					"TradeAttribute6");

			// Send to CEP
			TaxlotManager.sendTaxlotToCEPEngine(whatIfTaxlotObj, false);

			AdviseSymbolHelper.getInstance().addAdviceForSymbol(whatIfTaxlot.get("Symbol").toString());
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
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
	 *                                   the _isInitializationInProcess to set
	 */
	public void setInitializationFlag(boolean value) {
		synchronized (_initializationLockerObject) {
			this._isInitializationInProcess = value;
		}
	}
	
	/**
	 * Adds values to the FX Forward symbols list by parsing JSON data.
	 *
	 * @param jsonReceivedData
	 *            JSON string containing the list of FX Forward symbols.
	 */
	public void addValuesInFxFwdSymbolsList(String jsonReceivedData) {
		try {
			ObjectMapper objectMapper = new ObjectMapper();
			ArrayList<String> parsedList = objectMapper.readValue(jsonReceivedData,
					new TypeReference<ArrayList<String>>() {
					});

			symbolsFXForwards.clear();
			symbolsFXForwards.addAll(parsedList);

			if (!symbolsFXForwards.isEmpty()) {
				PranaLogManager.logOnly("Fx and FxForwad Symbols using Pricing Input's Selected Feed Price: ");
				for (String item : symbolsFXForwards) {
					PranaLogManager.logOnly(item);
				}
			} else {
				PranaLogManager.logOnly("No Fx and FxForwad Symbols are using Pricing Input's Selected Feed Price");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Retrieves the list of FX Forward symbols.
	 *
	 * @return A list of FX Forward symbols, or null if an exception occurs.
	 */
	public ArrayList<String> getSymbolsFxFwdDetails() {
		try {
			return symbolsFXForwards;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}
}
