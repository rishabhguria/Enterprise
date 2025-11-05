package prana.esperCalculator.esperCEP;

import java.util.ArrayList;
import java.util.HashMap;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class AdviseSymbolHelper {

	private static AdviseSymbolHelper _dataProcessor;
	private static Object _lockerObject = new Object();

	public static AdviseSymbolHelper getInstance() {
		synchronized (_lockerObject) {
			if (_dataProcessor == null)
				_dataProcessor = new AdviseSymbolHelper();
			return _dataProcessor;
		}
	}

	/**
	 * Instance of IAmqpSender, used to send advice request
	 */
	private IAmqpSender _amqpAdviceRequestSender;

	ArrayList<String> _adviseSymbolList = new ArrayList<>();

	private AdviseSymbolHelper() {
		String otherdataExchangeName;
		try {
			otherdataExchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_EXCHANGE_LIST, ConfigurationConstants.KEY_EXCHANGE_OTHERDATA);

			_amqpAdviceRequestSender = AmqpHelper.getSender(otherdataExchangeName, ExchangeType.Direct,
					MediaType.Exchange, false);
		} catch (Exception e) {
			PranaLogManager.error(e);
		}
	}

	/**
	 * Sends a livefeed subscription request to esper
	 * 
	 * @param symbol
	 */
	public void addAdviceForSymbol(String symbol) {
		try {
			if (!_adviseSymbolList.contains(symbol)) {
				PranaLogManager.logOnly(symbol + ", symbol added to advice for InMarket/InStage order.");
				HashMap<String, Object> adviceRequest = WhatIfHelper.getSecurityDetailsForSymbol(symbol);
				String requestString = JSONMapper.getStringForObject(adviceRequest);
				_amqpAdviceRequestSender.sendData(requestString, "SymbolAdviceRequest");
				_adviseSymbolList.add(symbol);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Reset Advise List
	 */
	public void resetAdviseList() {
		try {
			_adviseSymbolList.clear();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Update Advise List
	 * 
	 * @param symbol
	 */
	public void updateAdviceList(String symbol) {
		try {
			if (!_adviseSymbolList.contains(symbol))
				_adviseSymbolList.add(symbol);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
	}
}