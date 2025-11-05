package prana.esperCalculator.customRule;

import java.io.File;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.apache.commons.io.FileUtils;
import org.w3c.dom.DOMException;
import org.w3c.dom.Document;
import org.w3c.dom.Element;

import prana.businessObjects.rule.customRules.RuleDefinition;
import prana.esperCalculator.commonCode.RuleDeploymentManagerCommon;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.constants.CustomRuleConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.fileIO.FileHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class ImportExportHelper {

	public static String exportRule(String packageName, String ruleName, String directoryPath, String ruleCategory,
			RuleDefinition ruleDef) throws Exception {
		try {
			StringBuilder newDirectoryPath = new StringBuilder();
			newDirectoryPath.append(FileHelper.createDirectory(directoryPath, packageName, ruleName, ruleCategory));

			exportRuleConfig(newDirectoryPath.toString(), ruleDef, ruleName);
			exportRuleEPL(newDirectoryPath.toString(), ruleDef, ruleName);
			PranaLogManager
					.info("RuleId: " + ruleDef.getRuleId() + ", RuleName: " + ruleDef.getRuleName() + " is Exported");
			return newDirectoryPath.toString();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return "";
		}
	}

	private static void exportRuleEPL(String path, RuleDefinition ruleDef, String ruleName) {
		try {
			String eplPath = CustomRuleConstants.RULE_CONF_EPL_ROOT_DIRECTORY + ruleDef.getClientName() + "/"
					+ ruleDef.getRuleType() + ruleDef.getEplPath();
			FileUtils.copyDirectoryToDirectory(new File(eplPath), new File(path));
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@SuppressWarnings("unchecked")
	private static void exportRuleConfig(String path, RuleDefinition ruleDef, String ruleName) {
		try {
			DocumentBuilderFactory domFactory = DocumentBuilderFactory.newInstance();
			DocumentBuilder documentBuilder = domFactory.newDocumentBuilder();

			Document doc = documentBuilder.newDocument();
			Element ruleDefRoot = doc.createElement("ruleConfig");
			Element rootTag = doc.createElement("rules");
			rootTag.setAttribute("name", ruleDef.getClientName());

			Element root = doc.createElement("rule");
			root.setAttribute("id", ruleDef.getRuleId());

			Element name = doc.createElement("name");
			name.setTextContent(ruleDef.getRuleName());
			root.appendChild(name);

			Element enabled = doc.createElement("enabled");
			enabled.setTextContent(String.valueOf(ruleDef.getEnabled()));
			root.appendChild(enabled);

			Element isDeleted = doc.createElement("isDeleted");
			isDeleted.setTextContent(String.valueOf(ruleDef.getIsDeleted()));
			root.appendChild(isDeleted);

			Element ruleType = doc.createElement("ruleType");
			ruleType.setTextContent(ruleDef.getRuleType());
			root.appendChild(ruleType);

			Element eplPath = doc.createElement("eplPath");
			eplPath.setTextContent(ruleDef.getEplPath());
			root.appendChild(eplPath);

			Element description = doc.createElement("description");
			description.setTextContent(ruleDef.getDescription());
			root.appendChild(description);

			Element compressionLevel = doc.createElement("compressionLevel");
			compressionLevel.setTextContent(ruleDef.getCompressionLevel());
			root.appendChild(compressionLevel);

			Element validationCompletedEventName = doc.createElement("validationCompletedEventName");
			validationCompletedEventName.setTextContent(ruleDef.getValidationCompletedEventName());
			root.appendChild(validationCompletedEventName);

			Element outputStatementList = doc.createElement("outputStatementList");
			for (int i = 0; i < ruleDef.getOutputStatementList().size(); i++) {
				Element outputStatementName = doc.createElement("outputStatementName");
				outputStatementName.setAttribute("name", ruleDef.getOutputStatementList().get(i));
				outputStatementList.appendChild(outputStatementName);
			}
			root.appendChild(outputStatementList);

			Element windowFillerStatementList = doc.createElement("windowFillerStatementList");
			for (int i = 0; i < ruleDef.getOutputStatementList().size(); i++) {
				Element windowFillerStatementName = doc.createElement("windowFillerStatementName");
				windowFillerStatementName.setAttribute("name", ruleDef.getWindowFillerStatementList().get(i));
				windowFillerStatementList.appendChild(windowFillerStatementName);
			}
			root.appendChild(windowFillerStatementList);

			Element constantList = doc.createElement("constantList");
			HashMap<String, Object> constants = JSONMapper.getHashMap(ruleDef.getConstants());
			constants = (HashMap<String, Object>) constants.get("HashMap");

			ArrayList<LinkedHashMap<String, Object>> list = (ArrayList<LinkedHashMap<String, Object>>) constants
					.get("consts");

			for (LinkedHashMap<String, Object> customRuleConstantDefination : list) {
				Element constantValue = doc.createElement("constantValue");
				constantValue.setAttribute("name", customRuleConstantDefination.get("name").toString());
				constantValue.setAttribute("type", customRuleConstantDefination.get("type").toString());
				constantValue.setAttribute("value", customRuleConstantDefination.get("value").toString());
				constantValue.setAttribute("displayName", customRuleConstantDefination.get("displayName").toString());
				constantList.appendChild(constantValue);
			}
			root.appendChild(constantList);

			Element blocked = doc.createElement("blocked");
			blocked.setTextContent(String.valueOf(ruleDef.getBlocked()));
			root.appendChild(blocked);

			rootTag.appendChild(root);
			ruleDefRoot.appendChild(rootTag);
			doc.appendChild(ruleDefRoot);
			FileHelper.writeXmlDocToFile(doc, path + "//CustomRuleConfig.xml");
		} catch (DOMException ex) {
			PranaLogManager.error(ex);
		} catch (ParserConfigurationException ex) {
			PranaLogManager.error(ex);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public static RuleDefinition importRule(String packageName, String oldRuleName, String directoryPath,
			String newRuleName) {
		RuleDefinition rule = new RuleDefinition();
		try {
			String clientName = "";
			for (String key : ConfigurationHelper.getInstance()
					.getSection(ConfigurationConstants.SECTION_CUSTOM_RULE_PATH).keySet()) {
				clientName = (key);
				break;
			}
			rule = RuleDataHelper.importRuleConfig(directoryPath, clientName);
			if (rule != null) {

				importRuleEPL(directoryPath + rule.getEplPath(), rule, oldRuleName);
				PranaLogManager
						.info("RuleId: " + rule.getRuleId() + ", RuleName: " + rule.getRuleName() + " is Imported");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return rule;
	}

	private static void importRuleEPL(String path, RuleDefinition ruleDef, String ruleName) {
		try {
			String eplPath = CustomRuleConstants.RULE_CONF_EPL_ROOT_DIRECTORY + ruleDef.getClientName() + "/"
					+ ruleDef.getRuleType();
			FileUtils.copyDirectoryToDirectory(new File(path), new File(eplPath));
			RuleDeploymentManagerCommon.loadEPLsFromDirectory(ruleDef);
			PranaLogManager.info("Epl copied.");

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
}