package prana.esperCalculator.esperUDF;

import java.util.HashMap;

import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.logging.PranaLogManager;

public class PreferenceManager {

	public static void preferencesReceived(HashMap<String, Object> map) {
		try {
			boolean isM2MIncludedInCash = Boolean.parseBoolean(map.get("IsM2MIncludedInCash").toString());
			String companyBaseCurrency = map.get("CompanyBaseCurrency").toString();
			CEPManager.setVariableValue("IsM2MIncludedInCash", isM2MIncludedInCash);
			CEPManager.setVariableValue("CompanyBaseCurrency", companyBaseCurrency);

			boolean isCreditLimitBoxPositionAllowed = Boolean
					.parseBoolean(map.get("IsCreditLimitBoxPositionAllowed").toString());
			CEPManager.setVariableValue("IsCreditLimitBoxPositionAllowed", isCreditLimitBoxPositionAllowed);

			boolean EquitySwapsMarketValueAsEquity = Boolean
					.parseBoolean(map.get("EquitySwapsMarketValueAsEquity").toString());
			CEPManager.setVariableValue("EquitySwapsMarketValueAsEquity", EquitySwapsMarketValueAsEquity);

			boolean calculateFxGainLossOnForexForwards = Boolean
					.parseBoolean(map.get("CalculateFxGainLossOnForexForwards").toString());
			CEPManager.setVariableValue("CalculateFxGainLossOnForexForwards", calculateFxGainLossOnForexForwards);
			
			boolean calculateFxGainLossOnSwaps = Boolean.parseBoolean(map.get("CalculateFxGainLossOnSwaps").toString());
			CEPManager.setVariableValue("CalculateFxGainLossOnSwaps", calculateFxGainLossOnSwaps);

			boolean setFxToZero = Boolean.parseBoolean(map.get("IsNetExposureZeroForForexForwards").toString());
			//CEPManager.setVariableValue("SetFxToZero", setFxToZero);
			
			boolean isBasketComplianceEnabled = Boolean.parseBoolean(map.get(ConfigurationConstants.IS_BASKETCOMPLIANCE_ENABLED).toString());
			CEPManager.setVariableValue(ConfigurationConstants.IS_BASKETCOMPLIANCE_ENABLED, isBasketComplianceEnabled);
			
			CEPManager.setVariableValue("IsEsperStarted", false);
			PranaLogManager.info("-------------------------------------");
			PranaLogManager.info("Preferences received as:");
			PranaLogManager.info("IsM2MIncludedInCash: " + CEPManager.getVariableValue("IsM2MIncludedInCash"));
			PranaLogManager.info("CompanyBaseCurrency: " + CEPManager.getVariableValue("CompanyBaseCurrency"));
			PranaLogManager.info("IsCreditLimitBoxPositionAllowed: "
					+ CEPManager.getVariableValue("IsCreditLimitBoxPositionAllowed"));
			PranaLogManager.info(
					"EquitySwapsMarketValueAsEquity: " + CEPManager.getVariableValue("EquitySwapsMarketValueAsEquity"));
			PranaLogManager.info(
					"CalculateFxGainLossOnForexForwards: " + CEPManager.getVariableValue("CalculateFxGainLossOnForexForwards"));
			PranaLogManager
					.info("CalculateFxGainLossOnSwaps: " + CEPManager.getVariableValue("CalculateFxGainLossOnSwaps"));
			PranaLogManager.info("SetFxToZero: " + CEPManager.getVariableValue("SetFxToZero"));

			// Update PostWithInMarketInStage Variables value
			UpdatePostVariableValues(
					Integer.parseInt(map.get(ConfigurationConstants.KEY_POST_WITH_INMARKET_INSTAGE).toString()));
			PranaLogManager.info(ConfigurationConstants.IS_BASKETCOMPLIANCE_ENABLED +": " + CEPManager.getVariableValue(ConfigurationConstants.IS_BASKETCOMPLIANCE_ENABLED));

			PranaLogManager.info("-------------------------------------");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Update GeUpdatePostVariableValues Variable Values
	 */
	public static void UpdatePostVariableValues(int postWithInMarketInStage) {
		try {
			String postWithInMarketInStageValue = ConfigurationConstants.CAPTION_KEY_POST;
			switch (postWithInMarketInStage) {
			case 1:
				postWithInMarketInStageValue = ConfigurationConstants.CAPTION_KEY_POST;
				break;
			case 2:
				postWithInMarketInStageValue = ConfigurationConstants.CAPTION_KEY_POST_AND_INMARKET;
				break;
			case 3:
				postWithInMarketInStageValue = ConfigurationConstants.CAPTION_KEY_POSTMARKETANDSTAGE;
				break;
			}

			CEPManager.setVariableValue(ConfigurationConstants.KEY_POST_WITH_INMARKET_INSTAGE,
					postWithInMarketInStage);
			PranaLogManager
					.info(ConfigurationConstants.KEY_POST_WITH_INMARKET_INSTAGE + ": " + postWithInMarketInStageValue);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}