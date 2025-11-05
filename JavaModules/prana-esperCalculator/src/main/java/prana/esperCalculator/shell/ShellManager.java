package prana.esperCalculator.shell;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.ArrayList;
import java.util.HashMap;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;
import com.espertech.esper.runtime.client.EPStatement;

import prana.amqpAdapter.AmqpHelper;
import prana.esperCalculator.communication.DataInitializationRequestProcessor;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.configuration.ApplicationHelper;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class ShellManager implements Runnable {

	private HashMap<String, ArrayList<String>> _statementList = new HashMap<>();
	private HashMap<String, ArrayList<String>> _startedStatementList = new HashMap<>();
	private ShellConsoleListener _shellConsoleListener = new ShellConsoleListener();
	private static ShellManager _shellManager;

	public static ShellManager getInstance() {
		if (_shellManager == null)
			_shellManager = new ShellManager();
		return _shellManager;
	}

	@Override
	public void run() {
		try {
			PranaLogManager.info("\nInitializing CEP Engine shell.");
			initializeShell();
			startConsoleReader();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void startConsoleReader() throws IOException {
		try {
			InputStreamReader converter = new InputStreamReader(System.in);
			BufferedReader in = new BufferedReader(converter);
			PranaLogManager.info("CEP Engine shell started.");
			while (true) {
				String cmd = in.readLine().trim();
				if (cmd.equals(null) || cmd.equals(""))
					continue;

				// If it a command to cancel dump
				if (cmd.equals("cancel")) {
					if (WindowDumpHandler.isDumpinProgress())
						cancelDump();
					else
						PranaLogManager.info("No Dump in progress.");
					continue;
				}

				// Do not execute a command if a dump is in progress
				if (WindowDumpHandler.isDumpinProgress()) {
					PranaLogManager.info("\n-- Can not execute a command while dump in progress\n");
					continue;
				}

				String[] cmdList = cmd.split(" ");
				if (cmdList.length > 0 && cmdList[0].length() > 0) {
					PranaLogManager.info("------------------------------------------------------------------------\n");
					switch (cmdList[0].toLowerCase()) {
					case "cls":
						handleClsCommand(cmdList);
						break;
					case "print":
						handlePrintCommand(cmdList);
						break;
					case "list":
						listStatement(cmdList);
						break;
					case "help":
						printHelpText(cmdList);
						break;
					case "query":
						handleQueryText(cmdList);
						break;
					case "dump":
						// Starting dumping on a new thread, so that it can be cancelled if the need
						// arises
						final String[] tempCmdList = cmdList;
						new Thread(new Runnable() {
							public void run() {
								dumpAllNamedWindow(tempCmdList);
							}
						}).start();
						break;
					case "gc":
						runGarbageCollector(cmdList);
						break;
					case "info":
						PranaLogManager.info(ApplicationHelper.getApplicationinfo());
						PranaLogManager.info(AmqpHelper.getAmqpSettings());
						break;
					case "refresh":
						refresh();
						break;
					case "eom":
						handleEOMText();
						break;
					case "streamflowwhatif":
						handleWhatIfStreamFlowText();
						break;
					case "streamflow":
						handleStreamFlowText();
						break;
					case "streamflowbasket":
						handleStreamFlowBasketCompliance();
					case "datainfo":
						handleDataInfo();
						break;
					default:
						PranaLogManager.info("\nCommand not supported.\nPlease see help text.\n");
						printHelpText(cmdList);
						break;
					}
					PranaLogManager.info("Command executed");
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public void handleWhatIfStreamFlowText() {
		try {
			String eplName = "";
			String eplStatement = "";
			String streamFlow[] = { "Security", "SecurityWindowInsert", "SecurityWindow", "SecurityWindowUpdated",
					"RowCalculation", "RowCalculationBaseWindow", "RowCalculationBaseWindowModified",
					"RowCalculationUpdateEventForSecurity", "AuecDetails",
					"AuecWindow", "AuecWindowUpdated", "RowCalculationUpdateEventForAuec", "SymbolData",
					"SymbolDataWithBeta", "SymbolDataWindow", "SymbolDataWindowUpdated", "TaxlotWindowEvent",
					"TaxlotWindow", "TaxlotWindowUpdated", "BasketAggreation",
					"WhatIfAccountWhatIfPart", "WhatIfMasterFundWhatIfPart", "WhatIfAggregationCheck",
					"WhatIfAggregationStart", "WhatIfAggregationAccountSymbolIntermediate",
					"WhatIfAggregationAccountSymbolIntermediate1", "WhatIfAccountSymbolWithNav",
					"WhatIfAccountSymbolUpdated", "WhatIfAccountSymbolAggregationDone",
					"WhatIfAggregationAccountSymbol", "WhatIfAggregationAccountUnderlyingSymbolIntermediate",
					"WhatIfAccountUnderlyingSymbolWithNav", "WhatIfAccountUnderlyingSymbolUpdated",
					"WhatIfAggregationAccountUnderlyingSymbol", "WhatIfAggregationAccountIntermediate1",
					"WhatIfAggregationAccountIntermediate", "WhatIfAccountWithNav", "WhatIfAggregationAccount",
					"WhatIfAggregationMasterFundSymbolIntermediate", "WhatIfMasterFundSymbolIntermediate1",
					"WhatIfMasterFundSymbolWithNav", "WhatIfMasterFundSymbolUpdated",
					"WhatIfAggregationMasterFundSymbol", "WhatIfAggregationMasterFundUnderlyingSymbolIntermediate",
					"WhatIfMasterFundUnderlyingSymbolWithNav", "WhatIfMasterFundUnderlyingSymbolUpdated",
					"WhatIfAggregationMasterFundUnderlyingSymbol", "WhatIfAggregationMasterFundIntermediate",
					"WhatIfMasterFundWithNav", "WhatIfAggregationMasterFund", "WhatIfAggregationSymbolIntermediate",
					"WhatIfSymbolIntermediate1", "WhatIfSymbolWithNav", "WhatIfSymbolUpdated",
					"WhatIfAggregationSymbol", "WhatIfAggregationUnderlyingSymbolIntermediate",
					"WhatIfUnderlyingSymbolWithNav", "WhatIfUnderlyingSymbolUpdated",
					"WhatIfAggregationUnderlyingSymbol", "WhatIfAggregationGlobalIntermediate", "WhatIfGlobalWithNav",
					"WhatIfAggregationGlobal", "WhatIfAggregationAssetIntermidiate", "WhatIfAssetWithNav",
					"WhatIfAggregationAsset", "WhatIfAggregationSectorIntermidiate", "WhatIfSectorWithNav",
					"WhatIfAggregationSector", "WhatIfAggregationSubSectorIntermidiate", "WhatIfSubSectorWithNav",
					"WhatIfAggregationSubSector", "WhatIfEndMessage", "WhatIfFinalEndMessage",
					"RowCalculationUpdateEventForSymbolData",
					"AccountSymbolIntermediate", "AccountSymbolWithNavEvent", "AccountSymbolWithNav",
					"AccountSymbolWithNavUpdated", "AccountSymbolWindowInitial", "AccountUnderlyingSymbolWithNavIntermediate",
					"AccountUnderlyingSymbolWithNavInsert",
					"AccountUnderlyingSymbolWithNav","AccountUnderlyingSymbolUpdated", "AccountUnderlyingWindowInitial",
					"AccountUnderlyingSymbolWithNav", "AccountWithNavIntermediate", "AccountWithNavUpdate",
					"AccountWithNav", "AccountWindowInitial", "MasterFundSymbolIntermediate",
					"MasterFundSymbolWithNavEvent", "MasterFundSymbolWithNav", "MasterFundSymbolWithNavUpdated",
					"MasterFundSymbolWindowInitial", "MasterFundUnderlyingSymbolWithNavIntermediate",
					"MasterFundUnderlyingSymbolWithNavInsert", "MasterFundUnderlyingSymbolWithNav",
					"MasterFundUnderlyingSymbolUpdated", "MasterFundUnderlyingWindowInitial",
					"MasterFundWithNavIntermediate", "MasterFundWithNavUpdate", "MasterFundWithNav",
					"MasterFundWindowInitial", "UnderlyingSymbolWithNavIntermediate", "UnderlyingSymbolWithNavInsert",
					"UnderlyingSymbolWithNav", "UnderlyingSymbolUpdated", "UnderlyingSymbolWindowInitial",
					"SymbolIntermediate", "SymbolWithNavEvent", "SymbolWithNav", "SymbolWithNavUpdated",
					"SymbolWindowInitial", "GlobalWithNavIntermediate",
					"GlobalWithNav", "GlobalWindowInitial", "AssetWithNavIntermediate", "AssetWithNavUpdated",
					"AssetWithNav", "AssetWindowInitial", "SectorWithNavIntermediate", "SectorWithNavUpdated",
					"SectorWithNav", "SectorWindowInitial", "SubSectorWithNavIntermediate", "SubSectorWithNavUpdated",
					"SubSectorWithNav", "SubSectorWindowInitial", "AggregationTrade", "AccountSymbolDivisorInsert",
					"AccountSymbolDivisorWindow", "AccountSymbolDivisorUpdate", "MasterFundSymbolDivisorInsert",
					"MasterFundSymbolDivisorWindow", "MasterFundSymbolDivisorUpdate", "SymbolDivisorInsert",
					"SymbolDivisorWindow", "AccountNavInserted", "AccountDivisorWindow", "AccountDivisorUpdateEvent",
					"MasterFundFieldsInserted", "MasterFundDivisorWindow", "MasterFundDivisorUpdateEvent",
					"GlobalNavInserted", "GlobalDivisorWindow", "GlobalDivisorUpdateEvent", "DivisorEndMessage" };
			for (int i = 0; i < streamFlow.length; i++) {
				eplName = streamFlow[i];
// 				eplStatement = "Select count(*) as " + eplName + " from " + eplName;
				eplStatement = "Select *from " + eplName;
				CEPManager.compileDeploy(eplStatement, eplName);
				_statementList.get("runtime").add(eplName);
				printStream(eplName);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}

	public void handleStreamFlowText() {
		try {
			String eplName = "";
			String eplStatement = "";
			String streamFlow[] = { "SymbolData", "SymbolDataWithBeta", "SymbolDataWindow", "SymbolDataWindowUpdated",
					"TaxlotWindow", "RowCalculation", "RowCalculationBaseWindow",
					"RowCalculationBaseWindowModified",
					"RowCalculationUpdateEventForSymbolData", "AccountSymbolIntermediate", "AccountSymbolWithNavEvent",
					"AccountSymbolWithNav", "AccountSymbolWithNavUpdated", "AccountSymbolWindowInitial",
					"AccountUnderlyingSymbolWithNavIntermediate", "AccountUnderlyingSymbolWithNavInsert",
					"AccountUnderlyingSymbolWithNav", "AccountUnderlyingSymbolUpdated",
					"AccountUnderlyingWindowInitial", "AccountUnderlyingSymbolWithNav", "AccountWithNavIntermediate",
					"AccountWithNavUpdate", "AccountWithNav", "AccountWindowInitial", "MasterFundSymbolIntermediate",
					"MasterFundSymbolWithNavEvent", "MasterFundSymbolWithNav", "MasterFundSymbolWithNavUpdated",
					"MasterFundSymbolWindowInitial", "MasterFundUnderlyingSymbolWithNavIntermediate",
					"MasterFundUnderlyingSymbolWithNavInsert", "MasterFundUnderlyingSymbolWithNav",
					"MasterFundUnderlyingSymbolUpdated", "MasterFundUnderlyingWindowInitial",
					"MasterFundWithNavIntermediate", "MasterFundWithNavUpdate", "MasterFundWithNav",
					"MasterFundWindowInitial", "UnderlyingSymbolWithNavIntermediate", "UnderlyingSymbolWithNavInsert",
					"UnderlyingSymbolWithNav", "UnderlyingSymbolUpdated", "UnderlyingSymbolWindowInitial",
					"SymbolIntermediate", "SymbolWithNavEvent", "SymbolWithNav", "SymbolWithNavUpdated",
					"SymbolWindowInitial", "GlobalWithNavIntermediate", "GlobalWithNav", "GlobalWindowInitial",
					"AssetWithNavIntermediate", "AssetWithNavUpdated", "AssetWithNav", "AssetWindowInitial",
					"SectorWithNavIntermediate", "SectorWithNavUpdated", "SectorWithNav", "SectorWindowInitial",
					"SubSectorWithNavIntermediate", "SubSectorWithNavUpdated", "SubSectorWithNav",
					"SubSectorWindowInitial", "AggregationTrade", "AccountSymbolDivisorInsert",
					"AccountSymbolDivisorWindow", "AccountSymbolDivisorUpdate", "MasterFundSymbolDivisorInsert",
					"MasterFundSymbolDivisorWindow", "MasterFundSymbolDivisorUpdate", "SymbolDivisorInsert",
					"SymbolDivisorWindow", "AccountNavInserted", "AccountDivisorWindow", "AccountDivisorUpdateEvent",
					"MasterFundFieldsInserted", "MasterFundDivisorWindow", "MasterFundDivisorUpdateEvent",
					"GlobalNavInserted", "GlobalDivisorWindow", "GlobalDivisorUpdateEvent", "DivisorEndMessage",
					"RowCalculationUpdateEventForAuec", "RowCalculationUpdateEventForSecurity",
					"AccountUnderlyingSymbolUpdated" };
			for (int i = 0; i < streamFlow.length; i++) {
				eplName = streamFlow[i];
				eplStatement = "Select count(*) as " + eplName + " from " + eplName;
				CEPManager.compileDeploy(eplStatement, eplName);
				_statementList.get("runtime").add(eplName);
				printStream(eplName);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}

	private void handleEOMText() {
		try {
			String eplName = "";
			String eplStatement = "";
			String whatIfEndMessage[] = { "WhatIfAggregationTrade", "WhatIfAggregationSymbol",
					"WhatIfAggregationAccountSymbol", "WhatIfAggregationMasterFundSymbol",
					"WhatIfAggregationCustomMasterFundSymbol", "WhatIfAggregationUnderlyingSymbol",
					"WhatIfAggregationAccountUnderlyingSymbol", "WhatIfAggregationMasterFundUnderlyingSymbol",
					"WhatIfAggregationAsset", "WhatIfAggregationSector", "WhatIfAggregationSubSector",
					"WhatIfAggregationGlobal", "WhatIfAggregationAccount", "WhatIfAggregationMasterFund",
					"WhatIfEndMessage", "WhatIfCustomRuleEndMessage", "WhatIfFinalEndMessage" };

			for (int i = 0; i < whatIfEndMessage.length; i++) {
				eplName = whatIfEndMessage[i];
				eplStatement = "Select basketId as " + whatIfEndMessage[i] + " from " + whatIfEndMessage[i];
				CEPManager.compileDeploy(eplStatement, eplName);
				_statementList.get("runtime").add(eplName);
				printStream(eplName);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}
	
	//handleDataInfo
	private void handleDataInfo() {
		try {
			PranaLogManager.info("*** Esper Window(s) Data Information ***");

			//SymbolDataWindow
			logTaxlotCount("SELECT COUNT(*) AS TotalCount FROM SymbolDataWindow", "Total Symbols: ");

			//SecurityWindow
			logTaxlotCount("SELECT COUNT(*) AS TotalCount FROM SecurityWindow", "Total Securities: ");
			
			//AccountWindow
			logTaxlotCount("SELECT COUNT(*) AS TotalCount FROM AccountWindow", "Total Accounts: ");
			
			//TaxlotWindow
			logTaxlotCount("SELECT COUNT(*) AS TotalCount FROM TaxlotWindow", "Total Taxlots: ");

			//TaxlotWindow with Post
			logTaxlotCount("Select count(*) as TotalCount from TaxlotWindow where taxlotType = \'Post\'",
					"Post Taxlots: ");

			//TaxlotWindow with InTrade
			logTaxlotCount("Select count(*) as TotalCount from TaxlotWindow where taxlotType != \'Post\'",
					"In Trade Taxlots: ");

			//AggregationTaxlotWindow
			logTaxlotCount("SELECT COUNT(*) AS TotalCount FROM AggregationTaxlotWindow",
					"Grouped Taxlots: ");
			
			//ExtendedAccountSymbolWithNav
			logTaxlotCount("SELECT COUNT(*) AS TotalCount FROM ExtendedAccountSymbolWithNav",
					"RTPNL Taxlots: ");

			PranaLogManager.info("------------------------------------------------------------------------\n");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	//Method to execute query and log the result
	private void logTaxlotCount(String query, String logMessage) {
		try {
			EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);
			for (EventBean event : result.getArray()) {
				PranaLogManager.info(logMessage + event.get("TotalCount").toString());
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	public void handleStreamFlowBasketCompliance() {
		try {
			String eplName = "";
			String eplStatement = "";
			String streamFlow[] = { "AccountNavPreferenceWindow", "AccountNavPreference", "AccrualForAccountWindow",
					"AccrualForAccount", "AccountCollection", "AccrualForAccountWindowUpdated", "BasketStartEOM",
					"AggregationTaxlotEventPost", "AggregationTaxlotWindow", "AggregationTaxlotUpdated",
					"AggregationTaxlotEventInTrade", "AuecWindow", "AuecDetails", "CashFlowWindow", "CashFlow",
					"DayEndCash", "DayEndCashEvent", "DayEndCashAccountWindow", "DayEndCashWindowUpdated",
					"DayEndCashMasterFundWindow", "CashFlowWindowUpdated", "ResetCashFlowWindow",
					"CashFlowMasterFundWindow", "DbNavWindow", "DbNav",
					"PmCalculationPreferenceWindow", "PMCalculationPrefs", "SecurityWindow", "SecurityWindowData",
					"Security", "SymbolDataWindow", "SymbolDataWindowInsert", "TaxlotWindow", "TaxlotWindowEvent",
					"Taxlot", "TaxlotWindowUpdated", "AccountNavInsertedWithoutTaxlots", "AccountNavInserted",
					"AccountSymbolDivisorWindow", "AccountSymbolDivisorUpdate", "AccountDivisorWindow",
					"AccountWiseNRA", "AccountSymbolDivisorInsert", "MasterFundSymbolDivisorWindow",
					"MasterFundSymbolDivisorInsert", "SymbolDivisorWindow", "SymbolDivisorInsert", "InitComplete",
					"GlobalNavInserted", "GlobalFieldsInserted", "SymbolDivisorUpdate", "GlobalDivisorWindow",
					"MasterFundDivisorWindow", "MasterFundFieldsInserted", "RowCalculation", "RowCalculationBaseWindow",
					"StrategyWindow", "RowCalculationBaseWindowModified", "RowCalculationBaseWindowWhatIfModified" };
			for (int i = 0; i < streamFlow.length; i++) {
				eplName = streamFlow[i];
				eplStatement = "Select *from " + eplName;
				CEPManager.compileDeploy(eplStatement, eplName);
				_statementList.get("runtime").add(eplName);
				printStream(eplName);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * This tells the dump manager to cancel dump
	 */
	private void cancelDump() {
		try {
			WindowDumpHandler.cancelDump();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void refresh() {
		try {
			if (DataInitializationRequestProcessor.getInstance().refreshData())
				PranaLogManager.info("Refresh initialized");
			else
				PranaLogManager.info("Refresh could not be initialized");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}

	private void runGarbageCollector(String[] cmdList) {
		System.gc();

	}

	/**
	 * Starting a dump that can be cancelled so that user can cancel the dump of
	 * need arises
	 * 
	 * @param cmdList
	 */
	private void dumpAllNamedWindow(String[] cmdList) {
		try {
			WindowDumpHandler.dump(getTokenizedCommand(cmdList), true);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public HashMap<String, String> getTokenizedCommand(String[] cmdList) {
		try {
			HashMap<String, String> tokenizedCommand = new HashMap<>();
			if (cmdList == null || cmdList.length == 0)
				return null;
			tokenizedCommand.put("Command", cmdList[0]);

			if (cmdList.length > 1) {
				int counter = 1;
				while (counter < cmdList.length) {
					if (cmdList.length > counter + 1) {
						tokenizedCommand.put(cmdList[counter].toLowerCase(), cmdList[counter + 1]);
						counter += 2;
					} else {
						PranaLogManager.info("Command not in proper format");
						return null;
					}
				}
			}
			return tokenizedCommand;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	private void handleQueryText(String[] cmdList) throws Exception {
		try {
			InputStreamReader converter = new InputStreamReader(System.in);
			BufferedReader in = new BufferedReader(converter);
			PranaLogManager.info("Enter statement Name");
			String eplName = in.readLine().trim();
			String eplStatement = "";
			boolean doStatementNameAlreadyExist = false;
			// Checking if statement name already in the engine
			for (String key : _statementList.keySet()) {
				if (_statementList.get(key).contains(eplName)) {
					doStatementNameAlreadyExist = true;
					break;
				}
			}
			// Initialize statement if already not in engine
			if (!doStatementNameAlreadyExist) {
				PranaLogManager.info("Enter statement Text");
				eplStatement = in.readLine().trim();
				try {
					CEPManager.compileDeploy(eplStatement, eplName);
					_statementList.get("runtime").add(eplName);
					printStream(eplName);
				} catch (Exception ex) {
					PranaLogManager.error(ex);
				}
			} else {
				PranaLogManager.info("Stream Name already exist." + "\nPlease choose different name.");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void listStatement(String[] cmdList) {
		try {
			if (cmdList.length > 1) {
				switch (cmdList[1].toLowerCase()) {
				case "whatif":
					listGroup("whatif");
					break;
				case "post":
					listGroup("post");
					break;
				case "other":
					listGroup("other");
					break;
				case "current":
					listGroup("current");
					break;
				case "runtime":
					listGroup("runtime");
					break;
				case "window":
					listGroup("window");
					break;
				default:
					PranaLogManager.info(cmdList[1].toLowerCase() + " group is not found\nUse help.");
					break;
				}
			} else {
				PranaLogManager.info("All Streams");
				for (String key : _statementList.keySet()) {
					listGroup(key.toLowerCase());
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void listGroup(String key) {
		try {
			PranaLogManager.info("\nListing statements in group " + key);
			if (key.equalsIgnoreCase("current")) {
				if (_startedStatementList.get("whatif").isEmpty() && _startedStatementList.get("post").isEmpty()
						&& _startedStatementList.get("other").isEmpty()
						&& _startedStatementList.get("runtime").isEmpty()) {
					PranaLogManager.info("\nNo started statements\n");
				} else {
					for (String keySet : _startedStatementList.keySet()) {
						if (!_startedStatementList.get(keySet).isEmpty()) {

							for (String s : _startedStatementList.get(keySet)) {
								PranaLogManager.info(" - " + s);
							}
						}
					}
				}
			} else if (_statementList.containsKey(key)) {
				if (_statementList.get(key).isEmpty()) {
					PranaLogManager.info("No statements in this group\n");
				} else {
					for (String s : _statementList.get(key)) {
						PranaLogManager.info(" - " + s);
					}
				}
			} else if (key.equalsIgnoreCase("window")) {
				HashMap<String, String> namedWindowList = ConfigurationHelper.getInstance()
						.getSection(ConfigurationConstants.SECTION_NAMED_WINDOW);
				for (String s : namedWindowList.keySet()) {
					PranaLogManager.info(" - " + namedWindowList.get(s));
				}
			} else {
				PranaLogManager.info("\n" + key + " group not found.");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void printHelpText(String[] cmdList) {
		try {
			PranaLogManager.info("------------------------------------------" + "\nCommands and Description\n"
					+ "------------------------------------------"
					+ "\ncls [ post | whatIf | other | runtime | <stream name> ] " + " Removes custom streams"
					+ "\n\tcls- Remove all Streams " + " \n\tcls post- Remove all Post ComplianceLevel Streams "
					+ "\n\tcls whatIf- Remove all WhatIf ComplianceLevel Streams "
					+ "\n\tcls other-  Remove all other intermediate Streams "
					+ "\n\tcls runtime-  Remove all Runtime created Streams "
					+ "\n\tcls <stream name>- Remove Respective Stream " + "\n--------" +

					"\nprint [ post | whatIf | other | runtime | <stream name> ]:- " + " Print custom streams"
					+ "\n\tprint- Print all Streams " + "\n\tprint post- Print Post ComplianceLevel Streams "
					+ "\n\tprint whatIf- Print WhatIf Streams " + "\n\tprint other- Print other intermediate Streams "
					+ "\n\tprint runtime- Print Runtime created Streams "
					+ "\n\tprint <stream name>- Print Respective Streams " + "\n--------" +

					"\nlist [ post | whatIf | other | current | runtime | window]:- " + " List custom streams"
					+ "\n\tlist- List all Streams " + "\n\tlist post- List all Post ComplianceLevel Streams "
					+ "\n\tlist whatIf- List all WhatIf ComplianceLevel Streams "
					+ "\n\tlist other- List other intermediate Streams "
					+ "\n\tlist current- List currently active Streams "
					+ "\n\tlist runtime- List Runtime created Streams "
					+ "\n\tlist window- List all named windows which can be dumped." + "\n---------" +

					"\ndump [-f fileFormat] [-n windowName]:- " + "Allows to dump current data window to file"
					+ "\n\tUse either xml or json file format" + "\n\t\tas : \n\t\t\t dump -f xml\n\t\t\t dump -f json"
					+ "\n\tUse window name for dumping" + "\n\t\tas : \n\t\t\t dump -n windowName"
					+ "\n\tUse both file format and windowName"
					+ "\n\t\tas : \n\t\t\t dump -f xml -n windowName\n\t\t\t dump -f json -n windowName"
					+ "\n\tDefault value : File format is xml and all windows will be dumped." + "\n---------" +

					"\ncancel:- " + "Cancels a Dump in progress" + "\n---------" +

					"\ninfo:- " + "Returns current running information of application" + "\n---------" +

					"\nrefresh:- " + "Refresh data for caculation" + "\n---------" + "\nquery:- "
					+ "Allows to create dynamic query" + "\nWill ask Statement Name"
					+ "\nAnd Statement(Epl query) to execute" + "\n------------------------------------------");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void handlePrintCommand(String[] cmdList) {
		try {
			if (cmdList.length > 1) {
				switch (cmdList[1].toLowerCase()) {
				case "whatif":
					printGroup("whatif");
					break;
				case "post":
					printGroup("post");
					break;
				case "other":
					printGroup("other");
					break;
				case "runtime":
					printGroup("runtime");
					break;
				default:
					printStream(cmdList[1]);
					break;
				}
			} else {
				printGroup("whatif");
				printGroup("post");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void handleClsCommand(String[] cmdList) {
		try {
			if (cmdList.length > 1) {
				switch (cmdList[1].toLowerCase()) {
				case "whatif":
					clearGroup("whatif");
					break;
				case "post":
					clearGroup("post");
					break;
				case "other":
					clearGroup("other");
					break;
				case "runtime":
					clearGroup("runtime");
					break;
				default:
					clearStream(cmdList[1]);
					break;
				}
			} else {
				for (String key : _statementList.keySet()) {
					clearGroup(key.toLowerCase());
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}

	private void initializeShell() {
		try {
			loadStreams();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void printGroup(String key) {
		try {
			for (String s : _statementList.get(key)) {
				printStream(s);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void clearGroup(String key) {
		try {
			for (String s : _startedStatementList.get(key)) {
				CEPManager.getStatement(s).removeListener(_shellConsoleListener);
				PranaLogManager.info(s + " stream output is removed from console output.");
			}
			_startedStatementList.get(key).clear();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void printStream(String stream) {
		try {
			EPStatement stmt = CEPManager.getStatement(stream);
			if (stmt == null) {
				PranaLogManager.info(stream + " stream/group not found in engine\nName of stream is case sensitive.");
			} else {
				boolean isAlreadyStarted = false;
				String group = "other";
				for (String key : _startedStatementList.keySet()) {
					if (_startedStatementList.get(key).contains(stream)) {
						isAlreadyStarted = true;
						group = key;
						break;
					}
				}

				if (isAlreadyStarted) {
					PranaLogManager
							.info(" - " + stream + " stream output is already directed to console. Group: " + group);
				} else {
					String keyOfStream = "Other";
					for (String key : _statementList.keySet()) {
						if (_statementList.get(key).contains(stream)) {
							keyOfStream = key;
							break;
						}
					}

					stmt.addListener(_shellConsoleListener);
					_startedStatementList.get(keyOfStream).add(stream);
					PranaLogManager
							.info(" - " + stream + " stream output is directed to console. Group: " + keyOfStream);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void clearStream(String stream) {
		try {
			EPStatement stmt = CEPManager.getStatement(stream);
			if (stmt == null) {
				PranaLogManager.info(stream + " stream/group not found in engine");
			} else {
				boolean isAlreadyStopped = true;
				String group = "other";
				for (String key : _startedStatementList.keySet()) {
					if (_startedStatementList.get(key).contains(stream)) {
						isAlreadyStopped = false;
						group = key;
						break;
					}
				}

				if (isAlreadyStopped) {
					PranaLogManager.info(" - " + stream + " stream output is already stopped. Group: " + group);
				} else {
					String keyOfStream = "Other";
					for (String key : _statementList.keySet()) {
						if (_statementList.get(key).contains(stream)) {
							keyOfStream = key;
							break;
						}
					}

					stmt.removeListener(_shellConsoleListener);
					_startedStatementList.get(keyOfStream).remove(stream);
					PranaLogManager.info(" - " + stream + " stream output is stopped. Group: " + keyOfStream);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private void loadStreams() {
		try {
			_statementList.put("post", new ArrayList<String>());
			_statementList.put("whatif", new ArrayList<String>());
			_statementList.put("other", new ArrayList<String>());
			_statementList.put("runtime", new ArrayList<String>());
			_startedStatementList.put("post", new ArrayList<String>());
			_startedStatementList.put("whatif", new ArrayList<String>());
			_startedStatementList.put("other", new ArrayList<String>());
			_startedStatementList.put("runtime", new ArrayList<String>());

			HashMap<String, String> exchangeStatementList = ConfigurationHelper.getInstance()
					.getSection(ConfigurationConstants.SECTION_STATEMENT_EXCHANGE);

			for (EPStatement epStatement : CEPManager.getStatements()) {
				String stmt = epStatement.getName();
				if (exchangeStatementList.containsKey(stmt)) {
					if (stmt.toLowerCase().startsWith("whatif")) {
						_statementList.get("whatif").add(stmt);
					} else {
						_statementList.get("post").add(stmt);
					}
				} else {
					_statementList.get("other").add(stmt);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public void reloadCurrentStatements() {
		for (String key : _startedStatementList.keySet()) {
			if (!_startedStatementList.get(key).isEmpty() && !key.equals("runtime")) {
				for (String s : _startedStatementList.get(key)) {
					CEPManager.getStatement(s).addListener(_shellConsoleListener);
				}
			}
		}
		PranaLogManager.info("Console Statements Loaded.");
	}
}
