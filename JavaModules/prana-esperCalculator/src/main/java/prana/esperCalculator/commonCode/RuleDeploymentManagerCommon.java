package prana.esperCalculator.commonCode;

import java.io.File;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Set;

import org.apache.commons.io.FileUtils;
import org.apache.commons.io.filefilter.TrueFileFilter;

import com.espertech.esper.common.client.EPCompiled;
import com.espertech.esper.common.client.module.Module;
import com.espertech.esper.common.client.module.ModuleOrder;
import com.espertech.esper.common.client.module.ModuleOrderOptions;
import com.espertech.esper.common.client.module.ModuleOrderUtil;
import com.espertech.esper.compiler.client.CompilerArguments;
import com.espertech.esper.compiler.client.EPCompiler;
import com.espertech.esper.compiler.client.EPCompilerProvider;
import com.espertech.esper.runtime.client.EPDeployment;
import com.espertech.esper.runtime.client.EPStatement;
import com.espertech.esper.runtime.client.UpdateListener;

import prana.amqpAdapter.AmqpHelper;
import prana.businessObjects.constants.ExchangeType;
import prana.businessObjects.constants.MediaType;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.businessObjects.interfaces.IDisposable;
import prana.businessObjects.rule.customRules.RuleDefinition;
import prana.esperCalculator.communication.DataInitializationRequestProcessor;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.constants.CustomRuleConstants;
import prana.esperCalculator.customRule.CustomRulesListener;
import prana.esperCalculator.customRule.RuleDeploymentManager;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class RuleDeploymentManagerCommon {

	static Map<String, EPStatement> listOfStatement = new HashMap<>();

	static Map<String, ArrayList<String>> ruleStatements = new HashMap<String, ArrayList<String>>();

	private static Map<EPStatement, UpdateListener> listeners = new HashMap<EPStatement, UpdateListener>();
	
	public static ArrayList<String> preTradeCustomEomEvents = new ArrayList<>();

	public static void ConfigureCEPEngine(RuleDefinition node) {
		loadEPLsFromDirectory(node);
	}
	/*
	 * Update the Custom rule Compression
	 */
	public static void UpdateCustomRuleCache(RuleDefinition ruleDef, boolean insert) {
		try {
			if (ruleDef != null) {
				String key = ruleDef.getClientName() + "_" + ruleDef.getRuleName() + "_" + ruleDef.getRuleType();
				if (insert) {
					String value = ruleDef.getCompressionLevel();
					CEPManager.CustomruletypeWithCompression.put(key, value);
					CEPManager.UpdateCustomRuleWithCompression();
				} else {
					CEPManager.ruletypeWithCompression.remove(key);
					CEPManager.CustomruletypeWithCompression.remove(key);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public static void loadEPLsFromDirectory(RuleDefinition node) {
		try {
			String eplDirectory = CustomRuleConstants.RULE_CONF_EPL_ROOT_DIRECTORY + node.getClientName() + "/"
					+ node.getRuleType() + node.getEplPath();
			String ruleId = node.getRuleId();

			if (node.getRuleType().equalsIgnoreCase("preTrade")) {
				// Destroy the validation statement, this may required if the rule is being
				// enabled
				List<EPStatement> epStatements = CEPManager.getStatementList(node.getValidationCompletedEventName());
				for (EPStatement epStatement : epStatements) {
					if (epStatement != null) {
						removeListenersFrom(node.getOutputStatementList());
						CEPManager.destroy("WhatIfCustomRuleEndMessageEOM");
						CEPManager.destroy(epStatement.getName());
						PranaLogManager.info("-- Destroyed previous statement " + epStatement.getName());
					}
				}

				if (!node.getEnabled()) {
					createPlaceHolderStatement(node);
					return;
				}
			}

			String esperService = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_ESPER);
			String releaseModeEsper = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_RELEASE_MODE_ESPER);
			File folder = new File(eplDirectory);
			if (folder.getAbsolutePath().contains(esperService)
					|| folder.getAbsolutePath().contains(releaseModeEsper) || !node.getRuleType().equalsIgnoreCase("PostTrade")) {
				String absolutePath = CEPManager.getEsperDirectoryPath(folder.getAbsolutePath());
				ruleStatements.put(ruleId, new ArrayList<String>());
				PranaLogManager.info("-- Loading rule module from " + '"' + absolutePath + '"');

				@SuppressWarnings("unchecked")
				List<File> listOfFiles = (List<File>) FileUtils.listFiles(new File(absolutePath),
						TrueFileFilter.INSTANCE, TrueFileFilter.INSTANCE);
				ArrayList<Module> listOfModule = new ArrayList<>();
				EPCompiler epComplier = EPCompilerProvider.getCompiler();

				for (File file : listOfFiles) {
					Module module = epComplier.readModule(file);
					listOfModule.add(module);
				}

				Set<String> emptySet = Collections.emptySet();
				ModuleOrder moduleOrder = ModuleOrderUtil.getModuleOrder(listOfModule, emptySet,
						new ModuleOrderOptions());

				// Build compiler arguments
				CompilerArguments args = new CompilerArguments(CEPManager.getEPRuntime().getConfigurationDeepCopy());

				// Make the existing EPL objects available to the compiler
				args.getPath().add(CEPManager.getEPRuntime().getRuntimePath());

				for (Module mymodule : moduleOrder.getOrdered()) {
					// Compile
					EPCompiled compiled = EPCompilerProvider.getCompiler().compile(mymodule, args);
					EPDeployment deployment = CEPManager.getEPRuntime().getDeploymentService().deploy(compiled);
					for (EPStatement ep : deployment.getStatements()) {
						listOfStatement.put(ep.getName(), ep);
						PranaLogManager.info("---- Deployed statement: " + ep.getName());
						ruleStatements.get(ruleId).add(ep.getName());
					}
				}
			}
			
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public static boolean removeListenersFrom(ArrayList<String> outputStatementList) {
		try {
			for (String statementName : outputStatementList) {
				List<EPStatement> epStatements = CEPManager.getStatementList(statementName);
				for (EPStatement epStatement : epStatements) {
					if (epStatement != null) {
						epStatement.removeAllListeners();
						Iterator<UpdateListener> listOfUpdateListener = epStatement.getUpdateListeners();
						while (listOfUpdateListener.hasNext()) {
							IDisposable listener = (IDisposable) listOfUpdateListener.next();
							listener.disposeListener();
						}

						listeners.remove(epStatement);
						PranaLogManager.info("-- Removed Listeners from " + epStatement.getName());
					}
				}
			}
			return true;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	public static boolean addAllListener(String exchangeName, RuleDefinition ruleDef, String routingKey) {
		IAmqpSender _amqpSender = null;

		try {
			_amqpSender = AmqpHelper.getSender(exchangeName, ExchangeType.Direct, MediaType.Exchange, false);
			if (ruleDef.getEnabled() && !ruleDef.getIsDeleted()) {
				for (String name : ruleDef.getOutputStatementList()) {
					EPStatement epStatement = CEPManager.getStatement(name);

					if (epStatement != null) {
						if (listeners.containsKey(epStatement)) {
							return false;
						}

						UpdateListener listener = new CustomRulesListener(_amqpSender, routingKey, ruleDef);
						epStatement.addListener(listener);
						// save listener instance
						listeners.put(epStatement, listener);
						UpdateCustomRuleCache(ruleDef,true);
						PranaLogManager.info(
								"-- Listener Attached to : " + name + " " + ruleDef.getRuleType() + " " + exchangeName);
					}
				}
				if (DataInitializationRequestProcessor.getInstance()._isEsperStarted) {
					CEPManager.destroy("WhatIfCustomRuleEndMessageEOM");
					RuleDeploymentManager.generateAndDeployCustomRuleEom(preTradeCustomEomEvents);
				}
			}
			return true;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	/**
	 * Unloads the statements to the disabled rule
	 * 
	 * @param ruleId
	 * @param outputSmtList
	 */
	public static void removeStatementsForRule(RuleDefinition rule) {
		try {
			boolean isRemoved = removeListenersFrom(ruleStatements.get(rule.getRuleId()));
			if (isRemoved) {
				CEPManager.destroy("WhatIfCustomRuleEndMessageEOM");
				for (String smt : ruleStatements.get(rule.getRuleId())) {
					CEPManager.destroy(smt);
					PranaLogManager.info("-- Destroyed statement " + smt);
				}

				ruleStatements.remove(rule.getRuleId());
				createPlaceHolderStatement(rule);
				RuleDeploymentManager.generateAndDeployCustomRuleEom(preTradeCustomEomEvents);

			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Creates a default statement with violated set to false
	 * 
	 * @param rule
	 */
	private static void createPlaceHolderStatement(RuleDefinition rule) {
		String ruleTemplate = "On BasketEOM as E Insert into @XXXXX select " + "E.basketId as basketId,"
				+ "E.basketId as taxlotId," + "false as isViolated," + "E.userId as userId," + "'' as summary,"
				+ "current_timestamp as validationTime," + "'' as dimension," + "'' as parameters," + "'' as threshold,"
				+ "'' as actualResult," + "'' as constraintField";
		ruleTemplate = ruleTemplate.replace("@XXXXX", rule.getValidationCompletedEventName());
		CEPManager.createEPL(ruleTemplate, rule.getOutputStatementList().get(0));
	}
	
	/**
	 * Creates EOM for WhatIf trade(s)
	 * 
	 * @param preTradeEomEvent
	 */
	public static void generateAndDeployWhatIfEom(ArrayList<String> preTradeEomEvent) {
		try {
			CEPManager.destroy("WhatIfEndMessageDynamic");
			PranaLogManager.logOnly("----------Generating WhatIf EOM----------");
			StringBuilder sbEom = new StringBuilder();
			sbEom.append("insert into WhatIfEndMessage\n");
			sbEom.append("\tselect distinct \n\tD.basketId as basketId,\n\t\t'EomPreTrade' as compressionLevel \n\tfrom\n");
			sbEom.append("pattern[every\n\t(D = RowCalculationBaseWindowWhatIfModified ");

			if (preTradeEomEvent.size() > 0)
				sbEom.append("->(\n\t");

			for (int enu = 0; enu < preTradeEomEvent.size(); enu++) {
				String eventName = preTradeEomEvent.get(enu);
				if (eventName != null && eventName.compareTo("") != 0) {
					sbEom.append(eventName);
					sbEom.append("(basketId = D.basketId)\n");
					if (enu < preTradeEomEvent.size() - 1)
						sbEom.append("\tand ");
					else
						sbEom.append(")");
				}
			}

			sbEom.append("\t) where timer:within(EventResetTimeout Seconds)]\ngroup by D.basketId");
			String eom = sbEom.toString();
			PranaLogManager.logOnly("WhatIf EOM genereted. Generated EOM is:\n" + eom);
			CEPManager.createEPL(eom, "WhatIfEndMessageDynamic");
			PranaLogManager.logOnly("----------WhatIf EOM deployed----------");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}

}
