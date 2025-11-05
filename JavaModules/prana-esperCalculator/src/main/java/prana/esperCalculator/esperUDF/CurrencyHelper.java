package prana.esperCalculator.esperUDF;

import prana.utility.logging.PranaLogManager;

public class CurrencyHelper {
	// TODO: remove this function and do this processing in epl file
	public static String reverseCurrencyPair(String currency) {
		try {
			if (currency.indexOf('-') > 0) {
				String firstCurrency = currency.substring(0, currency.indexOf('-'));
				String lastCurrency = currency.substring(currency.indexOf('-') + 1);

				return lastCurrency + "-" + firstCurrency;
			} else
				return "";
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return "";
		}
	}

	/**
	 * To check Base and Lead Currency If equal then return true otherwise return
	 * false
	 * 
	 * @param currencyPair
	 * @return
	 */
	public static boolean isBaseAndLeadCurrencyEqual(String currencyPair) {
		try {
			if (currencyPair.indexOf('-') > 0) {
				String firstCurrency = currencyPair.substring(0, currencyPair.indexOf('-'));
				String lastCurrency = currencyPair.substring(currencyPair.indexOf('-') + 1);
				return lastCurrency.equals(firstCurrency);
			} else
				return false;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}
}