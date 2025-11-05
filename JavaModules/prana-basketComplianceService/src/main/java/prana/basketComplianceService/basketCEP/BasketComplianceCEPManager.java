package prana.basketComplianceService.basketCEP;

import java.io.File;
import java.util.List;
import prana.esperCalculator.commonCode.*;
import prana.esperCalculator.constants.ConfigurationConstants;

import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

import com.espertech.esper.common.client.configuration.Configuration;
import com.espertech.esper.runtime.client.EPRuntimeProvider;

public class BasketComplianceCEPManager {
	
	public static void initializeCepEngine() throws Exception {
		try {
			String configFile = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_BASKET_COMPLIANCE_CONFIG_FILE);
			String eplDirectory = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_EPL_DIRECTORY);
			String commonEplDirectory = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_COMMON_EPL_DIRECTORY);
			String eplDirectoryExpressions = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_EPL_DIRECTORY_EXPRESSIONS);

			configureCEPEngine(configFile);
			List<File> defaultEpls = CEPManager.loadEPLs(eplDirectory);
			List<File> expressionEpls = CEPManager.loadEPLs(CEPManager.getEsperDirectoryPath(eplDirectoryExpressions));
			List<File> commonEpls = CEPManager.loadEPLs(CEPManager.getEsperDirectoryPath(commonEplDirectory));
			
			defaultEpls.addAll(expressionEpls);
			defaultEpls.addAll(commonEpls);
			CEPManager.loadEplFiles(defaultEpls);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	private static void configureCEPEngine(String configFile) throws Exception {
		try {
			File configFileAbs = new File(configFile);
			PranaLogManager.info("Configuring CEP engine....");
			Configuration cepConfig = new Configuration();
			cepConfig.configure(configFileAbs);
			cepConfig.getCompiler().getByteCode().setAccessModifiersPublic();
			cepConfig = CEPManager.addConstantsForCustomRules(cepConfig);
			CEPManager._epRunTime = EPRuntimeProvider.getRuntime(ConfigurationConstants.CEP_ENGINE_BASKET_COMPLIANCE, cepConfig);
			PranaLogManager.info("--- Basket Compliance CEP Engine Configured. ---\n");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}