package prana.esperCalculator.esperCEP;

import java.io.File;
import java.util.List;

import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

import com.espertech.esper.common.client.configuration.Configuration;
import com.espertech.esper.runtime.client.EPRuntimeProvider;

public class EsperCEPManager {

	public static void initializeCepEngine() throws Exception {
		try {
			String configFile = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_ESPER_CONFIG_FILE);
			String eplDirectory = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_EPL_DIRECTORY);
			
			configureCEPEngine(configFile);
			List<File> defaultEpls = CEPManager.loadEPLs(eplDirectory);
			CEPManager.loadEplFiles(defaultEpls);
			CEPManager.setVariableValue("IsEsperStarted", false);
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
			// cepConfig.getCompiler().getExecution().setEnabledDeclaredExprValueCache(false);
			// cepConfig.getCompiler().getStreamSelection().setDefaultStreamSelector(StreamSelector.RSTREAM_ISTREAM_BOTH);
			CEPManager._epRunTime = EPRuntimeProvider.getRuntime(ConfigurationConstants.CEP_ENGINE_COMPLIANCE, cepConfig);
			PranaLogManager.info("--- CEP Engine Configured. ---\n");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/*// Remove listener from detection statements during refresh.
	public static void removeDetectonListener() throws Exception {
		try {
			PranaLogManager.info("Removing Detection Statement Listeners");
			HashMap<String, String> detectionListenerStatementList = ConfigurationHelper.getInstance()
					.getSection(ConfigurationConstants.SECTION_STATEMENT_DETECTION);

			for (String stmtString : detectionListenerStatementList.keySet()) {
				if (stmtString != null) {
					EPStatement epStatement = getStatement(stmtString);
					Iterator<UpdateListener> listOfUpdateListener = epStatement.getUpdateListeners();

					while (listOfUpdateListener.hasNext()) {
						IDisposable listener = (IDisposable) listOfUpdateListener.next();
						listener.disposeListener();
					}
					epStatement.removeAllListeners();
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}*/
}