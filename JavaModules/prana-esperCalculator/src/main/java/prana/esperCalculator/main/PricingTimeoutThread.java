package prana.esperCalculator.main;

import prana.esperCalculator.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class PricingTimeoutThread extends Thread {

	/**
	 * The basket that needs to be validated
	 */
	private String _basketId;

	public void run() {
		try {
			int pricingTimeout = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_PRICING_TIMEOUT));
			// PendingWhatIfCache.getInstance().startTimer();
			Thread.sleep(pricingTimeout);
			PendingWhatIfCache.getInstance().cancelBasket(_basketId);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Public constructor
	 * 
	 * @param basketId
	 */
	public PricingTimeoutThread(String basketId) {
		try {
			_basketId = basketId;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}
