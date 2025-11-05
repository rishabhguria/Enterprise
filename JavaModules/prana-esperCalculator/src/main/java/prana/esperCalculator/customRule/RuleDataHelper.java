package prana.esperCalculator.customRule;

import java.io.File;
import java.io.FileOutputStream;
import java.io.RandomAccessFile;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.concurrent.Semaphore;
import java.nio.channels.FileChannel;
import java.nio.channels.FileLock;

import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.apache.commons.configuration.ConfigurationException;
import org.apache.commons.configuration.XMLConfiguration;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.NamedNodeMap;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import prana.businessObjects.rule.customRules.CustomRuleConstantDefination;
import prana.businessObjects.rule.customRules.RuleDefinition;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.constants.CustomRuleConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.fileIO.FileHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class RuleDataHelper {
	/**
	 * Single locker object used when switching cache or filling the input cache. As
	 * output cache is used by only one thread(Event sender thread), while input
	 * cache is used by both thread
	 */
	private static Object _lockerObject = new Object();
	
	/**
	 * Added Semaphores with Lock to provide cross-process synchronization.The
	 * combination of the Semaphore and file lock mechanisms ensures that only one
	 * thread can access and modify the shared XML file at a time.
	 */  
	private static Semaphore _semaphore = new Semaphore(1);
	
	static HashMap<String, RuleDefinition> RuleMap = new HashMap<>();

	public static HashMap<String, RuleDefinition> loadRuleSettings() {
		try {
			for (String clientName : ConfigurationHelper.getInstance()
					.getSection(ConfigurationConstants.SECTION_CUSTOM_RULE_PATH).keySet()) {
				PranaLogManager.info("\nLoading rules for " + clientName);
				File f = new File(ConfigurationHelper.getInstance()
						.getSection(ConfigurationConstants.SECTION_CUSTOM_RULE_PATH).get(clientName));

				HashMap<String, RuleDefinition> rulemapTemp = loadKeyValueFromConfig(f.getAbsolutePath());

				if (rulemapTemp != null) {
					for (String key : rulemapTemp.keySet()) {

						if (RuleMap.containsKey(key)) {
							throw new Exception(

									"Same key has been set two different rules. KeyName: " + key);
						} else {
							rulemapTemp.get(key).setClientName(clientName);
							RuleMap.put(key, rulemapTemp.get(key));
						}
					}

					PranaLogManager.info("Total rules loaded: " + rulemapTemp.size());
				} else {
					PranaLogManager.info("No rule found for " + clientName);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return RuleMap;
	}

	private static HashMap<String, RuleDefinition> loadKeyValueFromConfig(String fileName)
			throws ConfigurationException {
		HashMap<String, RuleDefinition> tempMap = new HashMap<String, RuleDefinition>();

		try {
			XMLConfiguration configuration = new XMLConfiguration(CEPManager.getEsperDirectoryPath(fileName));
			int noOfQueues = 1;

			Object prop = configuration
					.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "." + CustomRuleConstants.RULE_CONF_NAME);

			if (prop == null)
				return null;

			if (prop instanceof Collection) {
				noOfQueues = ((Collection<?>) prop).size();
			}

			for (int i = 0; i < noOfQueues; i++) {
				String keyN = (String) configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i
						+ ")[@" + CustomRuleConstants.RULE_CONF_ID + "]");

				String valueN = (String) configuration.getProperty(
						CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i + ")." + CustomRuleConstants.RULE_CONF_NAME);
				boolean enabledN = Boolean.parseBoolean(configuration.getProperty(
						CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i + ")." + CustomRuleConstants.RULE_CONF_ENABLED)
						.toString());
				boolean isDeletedN = Boolean.parseBoolean(configuration.getProperty(
						CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i + ")." + CustomRuleConstants.RULE_CONF_DELETED)
						.toString());

				boolean blockedN = Boolean.parseBoolean(configuration.getProperty(
						CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i + ")." + CustomRuleConstants.RULE_CONF_BLOCKED)
						.toString());
				String ruleTypeN = (String) configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i
						+ ")." + CustomRuleConstants.RULE_CONF_RULE_TYPE);
				String eplPathN = (String) configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i
						+ ")." + CustomRuleConstants.RULE_CONF_EPL_PATH);
				String[] descriptionNArray = configuration.getStringArray(CustomRuleConstants.RULE_CONF_NODE_TAG + "("
						+ i + ")." + CustomRuleConstants.RULE_CONF_DESCRIPTION);

				String descriptionN = "";
				StringBuilder builder = new StringBuilder();
				for (int iter = 0; iter < descriptionNArray.length; iter++) {
					builder.append(descriptionNArray[iter].replaceAll("\n", "").replaceAll("\t", ""));
					if (iter != descriptionNArray.length - 1)
						builder.append(", ");
				}
				descriptionN = builder.toString();

				String compressionLevelN = (String) configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG
						+ "(" + i + ")." + CustomRuleConstants.RULE_CONF_COMPRESSION_LEVEL);
				String validationCompletedEventN = (String) configuration
						.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i + ")."
								+ CustomRuleConstants.RULE_CONF_VALIDATION_COMPLETED_EVENT);

				int noOfQueues1 = 1;

				Object prop1 = configuration
						.getProperty("rules.rule(" + i + ")." + CustomRuleConstants.RULE_CONF_OUTPUT_STATEMENT_LIST
								+ "." + CustomRuleConstants.RULE_CONF_OUTPUT_STATEMENT_NAME + "[@name]");
				if (prop1 instanceof Collection) {
					noOfQueues1 = ((Collection<?>) prop1).size();
				}

				ArrayList<String> outputStatementNameN = new ArrayList<>();

				for (int j = 0; j < noOfQueues1; j++) {
					outputStatementNameN.add((String) configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG
							+ "(" + i + ")." + CustomRuleConstants.RULE_CONF_OUTPUT_STATEMENT_LIST + "."
							+ CustomRuleConstants.RULE_CONF_OUTPUT_STATEMENT_NAME + "(" + j + ")[@name]"));
				}

				int noOfQueues2 = 1;
				Object prop2 = configuration.getProperty(
						"rules.rule(" + i + ")." + CustomRuleConstants.RULE_CONF_WINDOW_FILLER_STATEMENT_LIST + "."
								+ CustomRuleConstants.RULE_CONF_WINDOW_FILLER_STATEMENT_NAME + "[@name]");
				if (prop1 instanceof Collection) {
					noOfQueues2 = ((Collection<?>) prop2).size();
				}

				ArrayList<String> windowFillerStatementNameN = new ArrayList<>();

				for (int j = 0; j < noOfQueues2; j++) {
					windowFillerStatementNameN
							.add((String) configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i
									+ ")." + CustomRuleConstants.RULE_CONF_WINDOW_FILLER_STATEMENT_LIST + "."
									+ CustomRuleConstants.RULE_CONF_WINDOW_FILLER_STATEMENT_NAME + "(" + j
									+ ")[@name]"));
				}

				Object prop3 = configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i + ")."
						+ CustomRuleConstants.RULE_CONF_CONSTS + "." + CustomRuleConstants.RULE_CONF_CONST + "[@name]");

				List<CustomRuleConstantDefination> constants = new ArrayList<CustomRuleConstantDefination>();
				if (prop3 != null) {
					int constCount = 1;
					if (prop3 instanceof Collection)
						constCount = ((Collection<?>) prop3).size();
					for (int x = 0; x < constCount; x++) {
						String name = (String) configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "("
								+ i + ")." + CustomRuleConstants.RULE_CONF_CONSTS + "."
								+ CustomRuleConstants.RULE_CONF_CONST + "(" + x + ")." + "[@name]");
						String value = (configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i
								+ ")." + CustomRuleConstants.RULE_CONF_CONSTS + "."
								+ CustomRuleConstants.RULE_CONF_CONST + "(" + x + ")." + "[@value]")).toString()
										.replaceAll("\\[|\\]", "");
						String type = (String) configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "("
								+ i + ")." + CustomRuleConstants.RULE_CONF_CONSTS + "."
								+ CustomRuleConstants.RULE_CONF_CONST + "(" + x + ")." + "[@type]");
						String displayName = (String) configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG
								+ "(" + i + ")." + CustomRuleConstants.RULE_CONF_CONSTS + "."
								+ CustomRuleConstants.RULE_CONF_CONST + "(" + x + ")." + "[@displayName]");
						String comboList = "";
						if(type.equals("Combo"))
						{
							comboList = (configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i
									+ ")." + CustomRuleConstants.RULE_CONF_CONSTS + "."
									+ CustomRuleConstants.RULE_CONF_CONST + "(" + x + ")." + "[@comboList]")).toString()
											.replaceAll("\\[|\\]", "");
						}

						CustomRuleConstantDefination constantValue = new CustomRuleConstantDefination(name, value, type,
								displayName, comboList);
						constants.add(constantValue);
					}
				}

				HashMap<String, List<CustomRuleConstantDefination>> a = new HashMap<String, List<CustomRuleConstantDefination>>();
				a.put("consts", constants);
				String constantJson = JSONMapper.getStringForObject(a);

				if (!isDeletedN) {
					RuleDefinition rule = new RuleDefinition();
					rule.setRuleId(keyN);
					rule.setRuleName(valueN);
					rule.setEnabled(enabledN);
					rule.setIsDeleted(isDeletedN);
					rule.setRuleType(ruleTypeN);
					rule.setEplPath(eplPathN);
					rule.setDescription(descriptionN);
					rule.setOutputStatementList(outputStatementNameN);
					rule.setWindowFillerStatementList(windowFillerStatementNameN);
					rule.setCompressionLevel(compressionLevelN);
					rule.setValidationCompletedEventName(validationCompletedEventN);
					rule.setBlocked(blockedN);
					rule.setConstants(constantJson);
					tempMap.put(keyN, rule);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return tempMap;
	}

	public static void editConfig(String key, String prop, String value, String confPath) {
		try {
			File f = new File(confPath);
			String esperService = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_ESPER);
			String releaseModeEsper = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_RELEASE_MODE_ESPER);
			if (f.getAbsolutePath().contains(esperService) || f.getAbsolutePath().contains(releaseModeEsper)) {
				String absolutePath = CEPManager.getEsperDirectoryPath(f.getAbsolutePath());
				DocumentBuilderFactory domFactory = DocumentBuilderFactory.newInstance();
				Document doc = domFactory.newDocumentBuilder().parse(absolutePath);
				NodeList nodeList = doc.getElementsByTagName("rule");
				int i = 0;
				for (i = 0; i < nodeList.getLength(); i++) {
					NamedNodeMap nodeAttr = nodeList.item(i).getAttributes();
					Node node = nodeAttr.getNamedItem("id");
					if (node.getTextContent().equals(key))
						break;
				}
				Node nodes = doc.getElementsByTagName(prop).item(i);
				nodes.setTextContent(value);

				TransformerFactory transformerFactory = TransformerFactory.newInstance();
				Transformer transformer = transformerFactory.newTransformer();
				DOMSource source = new DOMSource(doc);
				FileOutputStream os = new FileOutputStream(absolutePath);
				StreamResult result = new StreamResult();
				result.setOutputStream(os);
				transformer.transform(source, result);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public static RuleDefinition importRuleConfig(String directoryPath, String clientName) {
		RuleDefinition ruleDef = new RuleDefinition();
		try {
			HashMap<String, RuleDefinition> ruleTemp = loadKeyValueFromConfig(directoryPath + "\\CustomRuleConfig.xml");

			// TODO: currently only one rule is present in the config file as
			// rules are imported individually.
			for (String key : ruleTemp.keySet()) {
				if (RuleMap.containsKey(key))
					ruleDef = null;
				else {
					ruleTemp.get(key).setClientName(clientName);
					ruleTemp.get(key).setEnabled(false);
					ruleDef = ruleTemp.get(key);
					updateRuleConfig(ruleDef);
				}
			}
			return ruleDef;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			return null;
		}
	}

	@SuppressWarnings("unchecked")
	private static void updateRuleConfig(RuleDefinition ruleDef) throws Exception {
		try {
			String path = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_CUSTOM_RULE_PATH, (ruleDef.getClientName()));
			File f = new File(path);
			DocumentBuilderFactory domFactory = DocumentBuilderFactory.newInstance();
			Document doc = domFactory.newDocumentBuilder().parse(f);

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

			NodeList nodeList = doc.getElementsByTagName("rules");
			nodeList.item(0).appendChild(root);
			FileHelper.writeXmlDocToFile(doc, path);
			RuleMap.put(ruleDef.getRuleId(), ruleDef);
			PranaLogManager.info("RuleImported");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * Update the custom rule config xml
	 * @param ruleid
	 * @param confPath
	 * @param constantKeyValue
	 */
	public static void updateConstants(String ruleid, String confPath, LinkedHashMap<String, String> constantKeyValue) {
		try {
			_semaphore.acquire();
			String updatedPath = CEPManager.getEsperDirectoryPath(new File(confPath).getAbsolutePath().toString());
			File f = new File(updatedPath);
		    File lockFile = new File(updatedPath + ".lock");
			try (FileChannel fileChannel = new RandomAccessFile(lockFile, "rw").getChannel()) {
	        FileLock lock = fileChannel.lock();
			DocumentBuilderFactory domFactory = DocumentBuilderFactory.newInstance();
			Document doc = domFactory.newDocumentBuilder().parse(f);
			NodeList nodeList = doc.getElementsByTagName(CustomRuleConstants.RULE_CONF_CONST);
			int i = 0;
			for (i = 0; i < nodeList.getLength(); i++) {
				NamedNodeMap nodeAttr = nodeList.item(i).getAttributes();
				Node node = nodeAttr.getNamedItem("name");
				if (constantKeyValue.containsKey(node.getTextContent())) {
					// We have a new value
					Node nodeValue = nodeAttr.getNamedItem("value");
					nodeValue.setTextContent(constantKeyValue.get(node.getTextContent()));
				}
			}

			TransformerFactory transformerFactory = TransformerFactory.newInstance();
			Transformer transformer = transformerFactory.newTransformer();
			DOMSource source = new DOMSource(doc);
			FileOutputStream os = new FileOutputStream(updatedPath);
			StreamResult result = new StreamResult();
			result.setOutputStream(os);
			transformer.transform(source, result);
			os.close();
			lock.release();
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		finally {
			_semaphore.release();
		}	
	}
}