package prana.esperCalculator.main;

import java.io.File;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Paths;

import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;

public class WindowValidator {

	private static WindowValidator _windowValidator = null;

	private String _symbolValidationQuery;

	public static WindowValidator getInstance() {
		if (_windowValidator == null)
			_windowValidator = new WindowValidator();
		return _windowValidator;
	}

	/**
	 * PRivate constructor
	 */
	private WindowValidator() {
	}

	/**
	 * Load the validation queries
	 * 
	 * @param validatorQueryPath
	 */
	public void initialize(String validatorQueryPath) {
		try {
			File folder = new File(validatorQueryPath);
			String absPath = folder.getAbsolutePath();
			PranaLogManager.info("Loading validation queries from " + absPath);
			_symbolValidationQuery = new String(Files.readAllBytes(Paths.get(absPath, "SymbolValidator.epl")),
					StandardCharsets.UTF_8);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Runs the symbol
	 * 
	 * @param symbol
	 * @return
	 */
	public String check(String symbol) {

		try {
			boolean missingInformationRequired = Boolean.parseBoolean(ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_MISSING_INFORMATION_REQUIRED));
			if (missingInformationRequired) {
				String response = "Following information might be missing from the system : ";
				String query = _symbolValidationQuery.replace("@XXXXX", symbol.replace("\\", "\\\\"));
				EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);
				String[] columns = result.getArray()[0].getEventType().getPropertyNames();
				boolean flag = false;
				for (int i = 0; i < columns.length; i++) {
					if ((boolean) result.getArray()[0].get(columns[i])) {
						response += columns[i] + ", ";
						flag = true;
					}
				}

				if (flag)
					return response;
			}

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return null;
	}
}