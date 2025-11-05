package prana.esperCalculator.amqpCollectors;

import java.text.SimpleDateFormat;
import java.util.Date;

import com.espertech.esper.common.client.EventBean;

import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.communication.DataInitializationRequestProcessor;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.main.PendingWhatIfCache;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class AmqpListenerBasketCompliance implements IAmqpListenerCallback {

	/**
	 * AmqpDataReceived
	 */
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		try {
			switch (routingKey) {
			case "InitializationRequestForEsper":
				CEPManager.setVariableValue(ConfigurationConstants.IS_BASKETCOMPLIANCE_STARTED, true);
				DataInitializationRequestProcessor.getInstance().sendInitCompleteToBasketCompliance();
				break;
			case "EsperPostData":
				PendingWhatIfCache.setIsValidatedSymbolDataReceived(true);
				SendEsperPostDataToBasketCompliance(jsonReceivedData);
				if (jsonReceivedData.contains(ConfigurationConstants.PST))
					DataInitializationRequestProcessor.getInstance()
							.sendDataForBasketCompliance(ConfigurationConstants.PST, "DataSentFromEsper");
				else
					DataInitializationRequestProcessor.getInstance()
							.sendDataForBasketCompliance(ConfigurationConstants.STAGE, "DataSentFromEsper");
			    PendingWhatIfCache.setIsValidatedSymbolDataReceived(false);
				break;
			case "EsperRunningStatus":
				PranaLogManager.info("Received Esper running status request from Basket Compliance.");
				DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance("{\"ResponseType\":\"EsperRunningStatusResponse\"}", "InitCompleteInfo");
				break;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * SendEsperPostDataToBasketCompliance
	 */
	private void SendEsperPostDataToBasketCompliance(String jsonReceivedData) {
		try {
			// In case esper is re-started after basket then this variable remains false
			// and SendEsperPostDataToBasketCompliance implies basket compliance is started
			CEPManager.setVariableValue(ConfigurationConstants.IS_BASKETCOMPLIANCE_STARTED, true);
			SimpleDateFormat _parserSdf = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);
			PranaLogManager.info("Started sending data to Basket compliance at : " + _parserSdf.format(new Date())
					+ " for UserID: " + jsonReceivedData);
			
			//Auec Window
			String query = "Select * from AuecWindow where isEsperStarted = true";
			EventBean[] eventBeanArray9 = CEPManager.executeQuery(query).getArray();
			for (EventBean eventBean : eventBeanArray9) {
				DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
						JSONMapper.getStringForObject(eventBean.getUnderlying()), "AuecDetails");
			}

			// Security Window
			String query1 = "Select * from SecurityWindow where isEsperStarted = true";
			EventBean[] eventBeanArray10 = CEPManager.executeQuery(query1).getArray();
			for (EventBean eventBean : eventBeanArray10) {
				DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
						JSONMapper.getStringForObject(eventBean.getUnderlying()), "Security");
			}
			
			// SymbolData Window
			/*
			EventBean[] eventBeanArray11 = DataInitializationRequestProcessor.getInstance()
					.getWindowDataFrom("SymbolDataWindow");
			for (EventBean eventBean : eventBeanArray11) {
				DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
						JSONMapper.getStringForObject(eventBean.getUnderlying()), "SymbolDataWindowData");
			}

			DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance("{\"ResponseType\":\"SymbolDataEOMSent\"}", "SymbolDataEOMSent");
			 */

			// TaxlotWindow Base Window
			EventBean[] eventBeanArray3 = DataInitializationRequestProcessor.getInstance()
					.getWindowDataWithWhere("TaxlotWindow where taxlotType <> 'WhatIf'");
			for (EventBean eventBean : eventBeanArray3) {
				DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
						JSONMapper.getStringForObject(eventBean.getUnderlying()), "TaxlotWindowData");
			}

			// Account Divisor Window
			/*
			 * EventBean[] eventBeanArray =
			 * DataInitializationRequestProcessor.getInstance().getWindowDataFrom(
			 * "AccountDivisorWindow"); for (EventBean eventBean : eventBeanArray) {
			 * DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
			 * JSONMapper.getStringForObject(eventBean.getUnderlying()),
			 * "AccountDivisorWindowData"); }
			 * 
			 * // MasterFund Divisor Window EventBean[] eventBeanArray1 =
			 * DataInitializationRequestProcessor.getInstance().getWindowDataFrom(
			 * "MasterFundDivisorWindow"); for (EventBean eventBean : eventBeanArray1) {
			 * DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
			 * JSONMapper.getStringForObject(eventBean.getUnderlying()),
			 * "MasterFundDivisorWindowData"); }
			 * 
			 * // Global Divisor Window EventBean[] eventBeanArray2 =
			 * DataInitializationRequestProcessor.getInstance().getWindowDataFrom(
			 * "GlobalDivisorWindow"); for (EventBean eventBean : eventBeanArray2) {
			 * DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
			 * JSONMapper.getStringForObject(eventBean.getUnderlying()),
			 * "GlobalDivisorWindowData"); }
			 */
			
			//DayEndCashAccountWindow
			EventBean[] eventBeanArray4 = DataInitializationRequestProcessor.getInstance()
					.getWindowDataFrom("DayEndCashAccountWindow");
			for (EventBean eventBean : eventBeanArray4) {
				DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
						JSONMapper.getStringForObject(eventBean.getUnderlying()), "DayEndCashAccount");
			}

			//AccountNavPreferenceWindow
			EventBean[] eventBeanArray5 = DataInitializationRequestProcessor.getInstance()
					.getWindowDataFrom("AccountNavPreferenceWindow");
			for (EventBean eventBean : eventBeanArray5) {
				DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
						JSONMapper.getStringForObject(eventBean.getUnderlying()), "AccountNavPreference");
			}

			//AccrualForAccountWindow
			EventBean[] eventBeanArray6 = DataInitializationRequestProcessor.getInstance()
					.getWindowDataFrom("AccrualForAccountWindow");
			for (EventBean eventBean : eventBeanArray6) {
				DataInitializationRequestProcessor.getInstance().sendDataForBasketCompliance(
						JSONMapper.getStringForObject(eventBean.getUnderlying()), "AccrualForAccount");
			}

			//DbNavWindow
			EventBean[] eventBeanArray7 = DataInitializationRequestProcessor.getInstance()
					.getWindowDataFrom("DbNavWindow");
			for (EventBean eventBean : eventBeanArray7) {
				DataInitializationRequestProcessor.getInstance()
						.sendDataForBasketCompliance(JSONMapper.getStringForObject(eventBean.getUnderlying()), "DbNav");
			}
			PranaLogManager.info("Complete data sent to Basket compliance at : " + _parserSdf.format(new Date())
					+ " for UserID: " + jsonReceivedData);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * AmqpRecieverStarted
	 */
	public void amqpRecieverStarted() {
		PranaLogManager.logOnly("AmqpReceiver for BasketCompliance has STARTED");
	}

	/**
	 * AmqpRecieverStopped
	 */
	public void amqpRecieverStopped(String message, Exception ex) {
		PranaLogManager.info("AmqpReceiver for BasketCompliance has Stopeed");
		PranaLogManager.error(ex, message);
	}
}