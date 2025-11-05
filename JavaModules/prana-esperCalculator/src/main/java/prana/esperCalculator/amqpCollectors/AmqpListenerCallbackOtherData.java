/**
 * Holds all amqp data collectors
 */
package prana.esperCalculator.amqpCollectors;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedHashMap;

import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.uda.DynamicUDA;
import prana.esperCalculator.cacheClasses.MarketDataDualCache;
import prana.esperCalculator.cacheClasses.StageDataDualCache;
import prana.esperCalculator.communication.DataInitializationRequestProcessor;
import prana.esperCalculator.constants.CollectorConstants;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.customRule.RuleManager;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.commonCode.TaxlotManager;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotType;
import prana.esperCalculator.esperCEP.HolidayHelper;
import prana.esperCalculator.esperUDF.PreferenceManager;
import prana.esperCalculator.main.WhatIfManager;
import prana.esperCalculator.serviceProvider.ServiceProvider;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;
import com.espertech.esper.common.internal.util.UuidGenerator;
import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.databind.JsonMappingException;

/**
 * Callback listener file which perform actions after data is received from amqp
 * 
 * @author Chaturvedi Dewashish <dewashish@nirvana-sol.com>
 */
public class AmqpListenerCallbackOtherData implements IAmqpListenerCallback {
	static SimpleDateFormat parserSDF;

	/**
	 * @author Chaturvedi Dewashish <dewashish@nirvana-sol.com>
	 * @description Returns instance of listener
	 */
	public AmqpListenerCallbackOtherData() {
		parserSDF = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);
	}

	/*
	 * @see prana.amqpAdapter.IAmqpListenerCallback#AmqpRecieverStarted()
	 */
	@Override
	public void amqpRecieverStarted() {
		PranaLogManager.info("AmqpReceiver for Other data has STARTED\n");
	}

	/*
	 * @see prana.amqpAdapter.IAmqpListenerCallback#AmqpRecieverStopped()
	 */
	@Override
	public void amqpRecieverStopped(String message, Exception cause) {
		PranaLogManager.info("AmqpReceiver for Other data has STOPPED");
		PranaLogManager.error(cause, message);
	}

	/*
	 * @see
	 * prana.amqpAdapter.IAmqpListenerCallback#AmqpDataReceived(java.lang.String ,
	 * java.lang.String)
	 */
	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		try {
			HashMap<String, Object> map = (routingKey.equals(ConfigurationConstants.ROUTING_KEY_RULE_COMPRESSION_INFO)
					|| routingKey.equals(ConfigurationConstants.ROUTING_KEY_FX_FWD_PRICE_AVAILABILITY)
					|| routingKey.equals(ConfigurationConstants.ROUTING_KEY_DYNAMIC_UDA_CACHE)) ? null
							: JSONMapper.getHashMap(jsonReceivedData);
			switch (routingKey) {
			case "SendRuleNameWithCompression":
				CEPManager.GetRuleNameWithCompression(jsonReceivedData);
				CEPManager.createEOMBasedOnEnabledRules();
				break;
			case "MarkPrice":
				sendMarkPriceToEsper(map);
				break;
			case "DbNav":
				sendDBNavToEsper(map);
				break;
			case "Currency":
				sendCurrencyToEsper(map);
				break;
			case "YesterdayFxRates":
				yesterdayFxRatesReceived(map);
				break;
			// TODO: Need to remove this as this seems to be not used.
			case "CashEvents":
				sendCashEventsToEsper(map);
				break;
			case "LiveFeedStatus":
				WhatIfManager.getInstance().liveFeedStatusChanged(Boolean.parseBoolean(map.get("Status").toString()));
				break;
			case "AccountCollection":
				accountCollectionReceived(map);
				break;
			case "DayEndCash":
				dayEndCashReceived(map);
				break;
			case "Security":
				securityReceived(map);
				break;
			case "AuecDetails":
				auecDetailsReceived(map);
				break;
			case "OrderSides":
				orderSidesReceived(map);
				break;
			case "VenueCollection":
				venueReceived(map);
				break;
			case "CounterPartyCollection":
				counterPartyReceived(map);
				break;
			case "InitCompleteInfo":
				DataInitializationRequestProcessor.getInstance().initializationInfoReceived(map);
				break;
			case "HistoricalTaxlots":
				DataInitializationRequestProcessor.getInstance().historicalDataReceived(jsonReceivedData);
				break;
			case "Preferences":
				PreferenceManager.preferencesReceived(map);
				break;
			case "AccountNavPreferences":
				accountNavPreferenceReceived(map);
				break;
			case "StrategyCollection":
				strategyCollectionReceived(map);
				break;
			case "OrderTypeTags":
				orderTypeTagsCollectionReceived(map);
				break;
			case "YearlyHolidaysEvent":
				HolidayHelper.sendYearlyHolidaysToEsper(map);
				break;
			case "WeeklyHolidaysEvent":
				HolidayHelper.sendWeeklyHolidaysToEsper(map);
				break;
			case "CustomRuleRequest":
				RuleManager.operations(map);
				break;
			case "HistoricalTaxlotCompleted":
				DataInitializationRequestProcessor.getInstance().handleRefreshComplete();
				DataInitializationRequestProcessor.getInstance().sendInStageToEsper();
				DataInitializationRequestProcessor.getInstance().sendInTradeToEsper();
				break;
			case "InitialLiveFeed":
				initialiLiveFeedReceived(map);
				break;
			case "BetaForSymbol":
				BetaForSymbolReceived(map);
				break;
			case "PMCalculationPrefs":
				pmCalculationPreference(map);
				break;
			case "UserDefinedMTDPnl":
				userDefinedMTDPnl(map);
				break;
			case "StartOfMonthCapitalAccount":
				startOfMonthCapitalAccount(map);
				break;
			case "AccountWiseNRA":
				accountWiseNra(map);
				break;
			case "AvgVolCustomDays":
				avgVolCustomDays(map);
				break;
			case "AccrualForAccount":
				accrualForAccount(map);
			case "StartOfDayAccrualForAccount":
				startOfDayAccrualForAccount(map);
				break;
			case "DayAccrualForAccount":
				dayAccrualForAccount(map);
				break;
			case "DailyCreditLimit":
				dailyCreditLimitReceived(map);
				break;
			case "InTradeMarket":
				if (DataInitializationRequestProcessor.getInstance().getInitializationFlag()) {
					DataInitializationRequestProcessor.getInstance().inTradeReceived(map);
				} else {
					if (map != null) {
						String taxlotId = map.get("TaxLotID").toString();
						MarketDataDualCache.getInstance().addToCache(taxlotId, map);
					}
				}
				break;
			case "CalculationRequest":
				processCalculationRequest(map);
				break;
			case "InTradeStage":
				if (DataInitializationRequestProcessor.getInstance().getInitializationFlag()) {
					DataInitializationRequestProcessor.getInstance().inStageReceived(map);
				} else {
					if (map != null) {
						String taxlotId = map.get("TaxLotID").toString();
						StageDataDualCache.getInstance().addToCache(taxlotId, map);
					}
				}
				break;
			case "InitRequestCalculationService":
				DataInitializationRequestProcessor.getInstance().initializationRequestReceived(map);
				break;
			case "CommunicationResponseForEsper":
				DataInitializationRequestProcessor.getInstance().initializationRequestReceived(map);
				break;
			case "SymbolDataTimerInterval":
				DataInitializationRequestProcessor.getInstance().symbolDataTimerIntervalFromPricingServer = Integer.parseInt(map.get("SymbolDataTimerIntervalValue").toString());
				PranaLogManager.info("Symbol data timer value received from Pricing server = " + DataInitializationRequestProcessor.getInstance().symbolDataTimerIntervalFromPricingServer);
				break;
			case "FxFwdPriceAvailableInPricingInput":
				DataInitializationRequestProcessor.getInstance().addValuesInFxFwdSymbolsList(jsonReceivedData);
				break;
			}
		} catch (Exception ex) {
			StringBuilder sb = new StringBuilder();
			sb.append(ex.getMessage());
			sb.append("\n----------------------");
			sb.append("\nError occured in CallBackOtherData");
			sb.append("\nRouting key : " + routingKey);
			sb.append("\nJson : " + jsonReceivedData);
			sb.append("\n----------------------");
			PranaLogManager.error(ex, sb.toString());
		}
	}

	private void processCalculationRequest(HashMap<String, Object> map) {
		try {
			ServiceProvider.getInstance().processNewRequest(map);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void dailyCreditLimitReceived(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("DailyCreditLimit Map Info: " + map.toString());
			int accountId = Integer.valueOf(map.get("AccountId").toString());
			double longDebitBalance = 0;
			if (map.containsKey("LongDebitBalance"))
				longDebitBalance = Double.valueOf(map.get("LongDebitBalance").toString());
			double longDebitLimit = 0;
			if (map.containsKey("LongDebitLimit"))
				longDebitLimit = Double.valueOf(map.get("LongDebitLimit").toString());

			double shortCreditBalance = 0;
			if (map.containsKey("ShortCreditBalance"))
				shortCreditBalance = Double.valueOf(map.get("ShortCreditBalance").toString());

			double shortCreditLimit = 0;
			if (map.containsKey("ShortCreditLimit"))
				shortCreditLimit = Double.valueOf(map.get("ShortCreditLimit").toString());

			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { accountId, longDebitBalance, longDebitLimit, shortCreditBalance, shortCreditLimit },
					"DailyCreditLimits");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}

	/**
	 * Receives accrual from Expnl It is added in Nav if config entry in expnl is
	 * true for accruals.
	 */
	private void accrualForAccount(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("AccrualForAccount Map Info: " + map.toString());
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { map.get("AccountId"), map.get("StartOfDayAccruals"), map.get("DayAccruals") },
					"AccrualForAccount");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/**
	 * Receives start of day accrual from Expnl It is added in Nav if config entry in expnl is
	 * true for accruals.
	 */
	private void startOfDayAccrualForAccount(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("StartOfDayAccrualForAccount Map Info: " + map.toString());
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { map.get("AccountId"), map.get("StartOfDayAccruals"), map.get("DayAccruals") },
					"StartOfDayAccrualForAccount");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/**
	 * Receives day accrual from Expnl It is added in Nav if config entry in expnl is
	 * true for accruals.
	 */
	private void dayAccrualForAccount(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("DayAccrualForAccount Map Info: " + map.toString());
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { map.get("AccountId"), map.get("StartOfDayAccruals"), map.get("DayAccruals") },
					"DayAccrualForAccount");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void avgVolCustomDays(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("AvgVolCustomDays Map Info: " + map.toString());
			for (String Symbol : map.keySet()) {
				CEPManager.getEPRuntime().getEventService()
						.sendEventObjectArray(new Object[] { Symbol, map.get(Symbol) }, "AvgVolCustomDays");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void accountWiseNra(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("AccountWiseNRA Map Info: " + map.toString());
			for (String account : map.keySet()) {
				int accountId = Integer.valueOf(account);
				CEPManager.getEPRuntime().getEventService()
						.sendEventObjectArray(new Object[] { accountId, map.get(account) }, "AccountWiseNRA");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void startOfMonthCapitalAccount(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("StartOfMonthCapitalAccount Map Info: " + map.toString());
			for (String account : map.keySet()) {
				int accountId = Integer.valueOf(account);
				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
						new Object[] { accountId, map.get(account) }, "StartOfMonthCapitalAccount");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void userDefinedMTDPnl(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("UserDefinedMTDPnl Map Info: " + map.toString());
			for (String account : map.keySet()) {
				int accountId = Integer.valueOf(account);
				CEPManager.getEPRuntime().getEventService()
						.sendEventObjectArray(new Object[] { accountId, map.get(account) }, "UserDefinedMTDPnl");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void pmCalculationPreference(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("PMCalculationPrefs Map Info: " + map.toString());
			int accountId = Integer.valueOf(map.get("AccountId").toString());
			double highWaterMark = 0;
			if (map.containsKey("HighWaterMark"))
				highWaterMark = Double.valueOf(map.get("HighWaterMark").toString());
			double stopout = 0;
			if (map.containsKey("StopOut"))
				stopout = Double.valueOf(map.get("StopOut").toString());
			double traderPayoutPercent = 0;
			if (map.containsKey("TradersPayoutPercent"))
				traderPayoutPercent = Double.valueOf(map.get("TradersPayoutPercent").toString());

			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { accountId, highWaterMark, stopout, traderPayoutPercent }, "PMCalculationPrefs");

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void BetaForSymbolReceived(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("BetaForSymbol Map Info: " + map.toString());
			for (String symbol : map.keySet()) {

				CEPManager.getEPRuntime().getEventService()
						.sendEventObjectArray(new Object[] { symbol, map.get(symbol) }, "BetaForSymbol");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void initialiLiveFeedReceived(HashMap<String, Object> symbolData) {
		try {
			if (symbolData != null) {
				String selectedFeedPrice = symbolData.get(CollectorConstants.SELECTED_FEED_PRICE).toString();
				String askPrice = symbolData.get(CollectorConstants.ASK).toString();
				String bidPrice = symbolData.get(CollectorConstants.BID).toString();
				if (!selectedFeedPrice.equals("0.0") || !askPrice.equals("0.0") || !bidPrice.equals("0.0")) {
					PranaLogManager.logOnly(
							" Initial SymbolData received for symbol: " + symbolData.get(CollectorConstants.SYMBOL));
					/* FxSymbolData fields */
					String conversionMethod = CollectorConstants.CONVERSION_METHOD_OPERATOR_MULTIPLY;

					double delta = 1;
					if (symbolData.containsKey(CollectorConstants.DELTA)) {
						delta = Double.valueOf(symbolData.get(CollectorConstants.DELTA).toString());
						if (delta == 0)
							delta = 1;
					}

					if (symbolData.containsKey(CollectorConstants.CONVERSION_METHOD)) {
						int methodId = Integer
								.parseInt(symbolData.get(CollectorConstants.CONVERSION_METHOD).toString());
						if (methodId == 1)
							conversionMethod = CollectorConstants.CONVERSION_METHOD_OPERATOR_DIVIDE;
						else
							conversionMethod = CollectorConstants.CONVERSION_METHOD_OPERATOR_MULTIPLY;
					}
					double beta5yr = 1;
					if (symbolData.containsKey(CollectorConstants.BETA_5_YR)) {
						beta5yr = Double.valueOf(symbolData.get(CollectorConstants.BETA_5_YR).toString());
					}

					// First converting SharesOutStanding object value to double and then getting
					// the BigDecimal value.
					// https://jira.nirvanasolutions.com:8443/browse/PRANA-37833
					BigDecimal sharesOutStandings = BigDecimal
							.valueOf(((Number) symbolData.get(CollectorConstants.SHARES_OUTSTANDING)).doubleValue());
					int pricingStatus = Integer.parseInt(symbolData.get(CollectorConstants.PRICING_STATUS).toString());
					CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] {

							symbolData.get(CollectorConstants.SYMBOL),
							symbolData.get(CollectorConstants.UNDERLYING_SYMBOL), Double.valueOf(askPrice),
							Double.valueOf(bidPrice), symbolData.get(CollectorConstants.LOW),
							symbolData.get(CollectorConstants.HIGH), symbolData.get(CollectorConstants.OPEN), 0.0, // XXX:
																													// Closing
																													// price
																													// might
																													// need
																													// to
																													// remove
							symbolData.get(CollectorConstants.LAST_PRICE), Double.valueOf(selectedFeedPrice),
							conversionMethod, // Might not be in
												// symbolData of
												// different asset
												// type
							symbolData.get(CollectorConstants.MARK_PRICE), delta, beta5yr,
							symbolData.get(CollectorConstants.CATEGORY_CODE),
							symbolData.get(CollectorConstants.OPEN_INTEREST),
							symbolData.get(CollectorConstants.AVERAGE_VOLUME_20_DAY), sharesOutStandings, pricingStatus },
							CollectorConstants.SYMBOL_DATA_EVENT_NAME);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void sendCurrencyToEsper(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("Currency Map Info: " + map.toString());
			for (String key : map.keySet()) {
				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
						new Object[] { Integer.parseInt(key), map.get(key).toString() }, "Currency");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void strategyCollectionReceived(HashMap<String, Object> map) throws Exception {
		try {
			PranaLogManager.logOnly("StrategyCollection Map Info: " + map.toString());
			CEPManager.getEPRuntime().getEventService()
					.sendEventObjectArray(new Object[] { Integer.parseInt(map.get("StrategyId").toString()),
							map.get("StrategyFullName").toString(), map.get("StrategyName").toString(),
							Integer.parseInt(map.get("CompanyId").toString()),
							Integer.parseInt(map.get("MasterStrategyId").toString()),
							map.get("MasterStrategyName").toString() }, "StrategyCollection");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void accountNavPreferenceReceived(HashMap<String, Object> map) throws Exception {
		try {
			PranaLogManager.logOnly("AccountNavPreference Map Info: " + map.toString());
			for (String key : map.keySet()) {
				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
						new Object[] { Integer.parseInt(key), Boolean.parseBoolean(map.get(key).toString()) },
						"AccountNavPreference");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void counterPartyReceived(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("CounterParty Map Info: " + map.toString());
			for (String key : map.keySet()) {
				CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
						new Object[] { Integer.parseInt(key), map.get(key).toString() }, "CounterParty");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void venueReceived(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("Venue Map Info: " + map.toString());
			for (String key : map.keySet()) {
				CEPManager.getEPRuntime().getEventService()
						.sendEventObjectArray(new Object[] { Integer.parseInt(key), map.get(key).toString() }, "Venue");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void orderTypeTagsCollectionReceived(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("OrderType Map Info: " + map.toString());
			for (String key : map.keySet()) {
				CEPManager.getEPRuntime().getEventService()
						.sendEventObjectArray(new Object[] { key, map.get(key).toString() }, "OrderType");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void orderSidesReceived(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("OrderSide Map Info: " + map.toString());
			for (String key : map.keySet()) {
				switch (key) {
				case "1":
				case "3":
				case "B":
				case "E":
				case "A":
				case "8":
					CEPManager.getEPRuntime().getEventService()
							.sendEventObjectArray(new Object[] { key, map.get(key), 1 }, "OrderSide");
					break;
				case "9":
				case "2":
				case "4":
				case "5":
				case "6":
				case "D":
				case "C":
					CEPManager.getEPRuntime().getEventService()
							.sendEventObjectArray(new Object[] { key, map.get(key), -1 }, "OrderSide");
					break;
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void auecDetailsReceived(HashMap<String, Object> map) {
		try {
			Date today=null, yesterDay=null;	
			try {
				today = parserSDF.parse(map.get("Today").toString());
				yesterDay = parserSDF.parse(map.get("YesterDay").toString());
				PranaLogManager.logOnly("AuecDetails Map Info: " + map.toString());
			}
			catch (Exception ex)
			{
				PranaLogManager.info("There is some error while parsing the date. Map Info: " + map.toString());
				return;
			}
				CEPManager.getEPRuntime().getEventService()
					.sendEventObjectArray(new Object[] { UuidGenerator.generate(), map.get("AuecId"), yesterDay, today,
							map.get("AssetId"), map.get("Asset"), map.get("ExchangeId"), map.get("ExchangeName"),
							map.get("UnderlyingId"), map.get("Underlying"), map.get("CurrencyId"),
							map.get("Currency"), DataInitializationRequestProcessor.getInstance()._isEsperStarted }, "AuecDetails");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			PranaLogManager.info(map.toString());
		}
	}

	/**
	 * @param map
	 */
	public static void securityReceived(HashMap<String, Object> map) {
		try {
			LinkedHashMap<String, Object> udaData = JSONMapper.convertObject(map.get("SymbolUDAData"));
			LinkedHashMap<String, Object> dynamicUdaData = JSONMapper.convertObject(map.get("DynamicUDA"));
			// Added Logging to confirm we get Security for a Symbol on Esper
			if (map != null)
				PranaLogManager.logOnly("Security details received for Symbol: " + map.get("TickerSymbol"));
			
			String udaAsset = "";
			String udaSecurityType = "";
			String udaSector = "";
			String udaSubSector = "";
			String udaCountry = "";
			String riskCurrency = "Undefined";
			String issuer = "Undefined";
			String countryOfRisk = "Undefined";
			String region = "Undefined";
			String analyst = "Undefined";
			String ucitsEligibleTag = "Undefined";
			String liquidTag = "Undefined";
			String marketCap = "Undefined";
			String customUDA1 = "Undefined";
			String customUDA2 = "Undefined";
			String customUDA3 = "Undefined";
			String customUDA4 = "Undefined";
			String customUDA5 = "Undefined";
			String customUDA6 = "Undefined";
			String customUDA7 = "Undefined";
			String underlyingSymbol = "";
			String customUDA8 = "Undefined";
			String customUDA9 = "Undefined";
			String customUDA10 = "Undefined";
			String customUDA11 = "Undefined";
			String customUDA12 = "Undefined";

			boolean isCurrencyFuture = false;

			if (udaData != null) {
				if (udaData.containsKey("UDAAsset"))
					udaAsset = udaData.get("UDAAsset").toString();

				if (udaData.containsKey("UDASecurityType"))
					udaSecurityType = udaData.get("UDASecurityType").toString();

				if (udaData.containsKey("UDASector"))
					udaSector = udaData.get("UDASector").toString();

				if (udaData.containsKey("UDASubSector"))
					udaSubSector = udaData.get("UDASubSector").toString();

				if (udaData.containsKey("UDACountry"))
					udaCountry = udaData.get("UDACountry").toString();
			}

			if (dynamicUdaData != null) {
				if (dynamicUdaData.containsKey("RiskCurrency"))
					riskCurrency = dynamicUdaData.get("RiskCurrency").toString();

				if (dynamicUdaData.containsKey("Issuer"))
					issuer = dynamicUdaData.get("Issuer").toString();

				if (dynamicUdaData.containsKey("CountryOfRisk"))
					countryOfRisk = dynamicUdaData.get("CountryOfRisk").toString();

				if (dynamicUdaData.containsKey("Region"))
					region = dynamicUdaData.get("Region").toString();

				if (dynamicUdaData.containsKey("Analyst"))
					analyst = dynamicUdaData.get("Analyst").toString();

				if (dynamicUdaData.containsKey("UCITSEligibleTag"))
					ucitsEligibleTag = dynamicUdaData.get("UCITSEligibleTag").toString();

				if (dynamicUdaData.containsKey("LiquidTag"))
					liquidTag = dynamicUdaData.get("LiquidTag").toString();

				if (dynamicUdaData.containsKey("MarketCap"))
					marketCap = dynamicUdaData.get("MarketCap").toString();

				if (dynamicUdaData.containsKey("CustomUDA1"))
					customUDA1 = dynamicUdaData.get("CustomUDA1").toString();

				if (dynamicUdaData.containsKey("CustomUDA2"))
					customUDA2 = dynamicUdaData.get("CustomUDA2").toString();

				if (dynamicUdaData.containsKey("CustomUDA3"))
					customUDA3 = dynamicUdaData.get("CustomUDA3").toString();

				if (dynamicUdaData.containsKey("CustomUDA4"))
					customUDA4 = dynamicUdaData.get("CustomUDA4").toString();

				if (dynamicUdaData.containsKey("CustomUDA5"))
					customUDA5 = dynamicUdaData.get("CustomUDA5").toString();

				if (dynamicUdaData.containsKey("CustomUDA6"))
					customUDA6 = dynamicUdaData.get("CustomUDA6").toString();

				if (dynamicUdaData.containsKey("CustomUDA7"))
					customUDA7 = dynamicUdaData.get("CustomUDA7").toString();
				
				if (dynamicUdaData.containsKey("CustomUDA8"))
					customUDA8 = dynamicUdaData.get("CustomUDA8").toString();
				
				if (dynamicUdaData.containsKey("CustomUDA9"))
					customUDA9 = dynamicUdaData.get("CustomUDA9").toString();
				
				if (dynamicUdaData.containsKey("CustomUDA10"))
					customUDA10 = dynamicUdaData.get("CustomUDA10").toString();
				
				if (dynamicUdaData.containsKey("CustomUDA11"))
					customUDA11 = dynamicUdaData.get("CustomUDA11").toString();
				
				if (dynamicUdaData.containsKey("CustomUDA12"))
					customUDA12 = dynamicUdaData.get("CustomUDA12").toString();
			}

			if (map.containsKey("IsCurrencyFuture")) {
				isCurrencyFuture = Boolean.parseBoolean(map.get("IsCurrencyFuture").toString());
			}

			BigDecimal sharesOutStandings = BigDecimal.ZERO;
			if (map.containsKey(CollectorConstants.SHARES_OUTSTANDING)) {
				if(map.get("TickerSymbol").equals("MSI"))					
				sharesOutStandings = BigDecimal
						.valueOf(((Number) map.get(CollectorConstants.SHARES_OUTSTANDING)).doubleValue());
			}

			int leadCurrencyId = -1;
			int vsCurrencyId = -1;
			Date expirationDate = null;
			String putOrCall = "N/A";
			double strikePrice = 0;
			double roundLot = 1.0;
			if (map.containsKey("PutOrCall")) {
				int key = Integer.parseInt(map.get("PutOrCall").toString());
				switch (key) {
				case 0:
					putOrCall = "Put";
					break;
				case 1:
					putOrCall = "Call";
					break;
				}
			}

			if (map.containsKey("LeadCurrencyID")) {
				leadCurrencyId = Integer.parseInt(map.get("LeadCurrencyID").toString());
			}

			if (map.containsKey("VsCurrencyID")) {
				vsCurrencyId = Integer.parseInt(map.get("VsCurrencyID").toString());
			}

			if (map.containsKey("ExpirationDate")) {
				expirationDate = parserSDF.parse(map.get("ExpirationDate").toString());
			}

			if (map.containsKey("MaturityDate")) {
				expirationDate = parserSDF.parse(map.get("MaturityDate").toString());
			}

			if (map.containsKey("StrikePrice")) {
				strikePrice = Double.parseDouble(map.get("StrikePrice").toString());
			}

			if (map.containsKey("RoundLot")) {
				roundLot = Double.parseDouble(map.get("RoundLot").toString());
			}

			if(map.containsKey("UnderLyingSymbol")) {
				underlyingSymbol = map.get("UnderLyingSymbol").toString();
				if(underlyingSymbol.isEmpty())
					underlyingSymbol = map.get("TickerSymbol").toString();
			}
			
			DynamicUDA.getInstance().setSymbolDynamicUDAData(map.get("TickerSymbol").toString(), dynamicUdaData);
			
			CEPManager.getEPRuntime().getEventService()
					.sendEventObjectArray(new Object[] { Integer.parseInt(map.get("AssetID").toString()),
							UuidGenerator.generate(), map.get("TickerSymbol"), map.get("AUECID"),
							underlyingSymbol, map.get("Delta"), map.get("LongName"), map.get("Multiplier"),
							Integer.parseInt(map.get("CurrencyID").toString()), expirationDate, putOrCall, strikePrice,
							leadCurrencyId, vsCurrencyId, isCurrencyFuture, udaAsset, udaSecurityType, udaSector,
							udaSubSector, udaCountry, riskCurrency, issuer, countryOfRisk, region, analyst,
							ucitsEligibleTag, liquidTag, marketCap, customUDA1, customUDA2, customUDA3, customUDA4,
							customUDA5, customUDA6, customUDA7, roundLot, sharesOutStandings, DataInitializationRequestProcessor.getInstance()._isEsperStarted, customUDA8,customUDA9,customUDA10,customUDA11,customUDA12,
							map.get("BloombergSymbol"), map.get("ActivSymbol"), map.get("FactSetSymbol"), map.get("BloombergSymbolWithExchangeCode")}, "Security");
		} catch (Exception ex) {
			if (map != null)
				PranaLogManager.error("Error thrown for Symbol: " + map.get("TickerSymbol"), ex);
			else
				PranaLogManager.error(ex);
		}
	}

	private void yesterdayFxRatesReceived(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("YesterdayFxRates Map Info: " + map.toString());
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] { map.get("CurrencySymbol"),
					map.get("ConversionRate"), map.get("ConversionMethodOperator"), map.get("RateTime") },
					"YesterdayFxRates");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * @param map
	 */
	private void dayEndCashReceived(HashMap<String, Object> map) {
		try {
			PranaLogManager.logOnly("DayEndCash Map Info: " + map.toString());
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { map.get("Cash"), new Date(), map.get("AccountId"), map.get("DayCash") },
					"DayEndCash");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * @param map
	 * @throws Exception
	 * @throws JsonMappingException
	 * @throws JsonParseException
	 */
	private void accountCollectionReceived(HashMap<String, Object> map) throws Exception {
		try {
			PranaLogManager.logOnly("AccountCollection Map Info: " + map.toString());
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] {
					Integer.parseInt(map.get("AccountId").toString()), map.get("AccountLongName").toString(),
					map.get("AccountShortName").toString(), Integer.parseInt(map.get("CompanyId").toString()),
					Integer.parseInt(map.get("MasterFundId").toString()), map.get("MasterFundName").toString(),
					Integer.parseInt(map.get("BaseCurrencyId").toString())},
					"AccountCollection");

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * 
	 * @param cashEvents
	 */
	void sendCashEventsToEsper(HashMap<String, Object> cashEvents) {
		try {
			PranaLogManager.logOnly("CashEventBaseCurrency Map Info: " + cashEvents.toString());
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { cashEvents.get("cashAmount"), cashEvents.get("date"), cashEvents.get("AccountId") },
					"CashEventBaseCurrency");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	void sendDBNavToEsper(HashMap<String, Object> dbNav) {
		try {
			PranaLogManager.logOnly("DbNav Map Info: " + dbNav.toString());
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { dbNav.get("AccountId"), dbNav.get("StartOfDayNav"), dbNav.get("CurrentNav") },
					"DbNav");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * 
	 * @param markPrice
	 */
	void sendMarkPriceToEsper(HashMap<String, Object> markPrice) {
		try {
			if (markPrice != null) {
				double markPrices = Double.parseDouble(markPrice.get("MarkPrice").toString());
				if (markPrices != 0) {
					CEPManager.getEPRuntime().getEventService()
							.sendEventObjectArray(new Object[] { markPrice.get("Symbol"), markPrices,
									Integer.parseInt(markPrice.get("AssetID").toString()) }, "MarkPrice");
					PranaLogManager.logOnly("MarkPrice received for symbol: " + markPrice.get("Symbol")
							+ ", MarkPrice: " + markPrice.get("MarkPrice"));
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	void sendYesterdayFxRatesToEsper(HashMap<String, Object> yesterdayFxRates) {
		try {
			PranaLogManager.logOnly("YesterdayFxRates Map Info: " + yesterdayFxRates.toString());
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(new Object[] {
					yesterdayFxRates.get("symbol"), yesterdayFxRates.get("date"), yesterdayFxRates.get("fxRate") },
					"YesterdayFxRates");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	void sendYesterdayNavToEsper(HashMap<String, Object> yesterdayNav) {
		try {
			PranaLogManager.logOnly("YesterDayNav Map Info: " + yesterdayNav.toString());
			CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
					new Object[] { yesterdayNav.get("AccountId"), yesterdayNav.get("date"), yesterdayNav.get("nav") },
					"YesterDayNav");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}
