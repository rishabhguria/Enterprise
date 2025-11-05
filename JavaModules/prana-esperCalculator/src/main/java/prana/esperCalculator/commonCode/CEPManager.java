package prana.esperCalculator.commonCode;

import java.io.File;
import java.io.IOException;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.Collections;
import java.util.Comparator;
import java.util.Date;
import java.util.HashMap;
import java.util.Iterator;
import java.util.List;
import java.util.Map.Entry;
import java.util.Set;
import java.sql.Timestamp;

import org.apache.commons.configuration.XMLConfiguration;
import org.apache.commons.io.FileUtils;
import org.apache.commons.io.filefilter.TrueFileFilter;

import com.espertech.esper.common.client.EPCompiled;
import com.espertech.esper.common.client.configuration.Configuration;
import com.espertech.esper.common.client.configuration.ConfigurationException;
import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;
import com.espertech.esper.common.client.json.minimaljson.ParseException;
import com.espertech.esper.common.client.module.Module;
import com.espertech.esper.common.client.module.ModuleOrder;
import com.espertech.esper.common.client.module.ModuleOrderOptions;
import com.espertech.esper.common.client.module.ModuleOrderUtil;
import com.espertech.esper.common.internal.util.DeploymentIdNamePair;
import com.espertech.esper.common.internal.util.UuidGenerator;
import com.espertech.esper.compiler.client.CompilerArguments;
import com.espertech.esper.compiler.client.EPCompileException;
import com.espertech.esper.compiler.client.EPCompiler;
import com.espertech.esper.compiler.client.EPCompilerProvider;
import com.espertech.esper.compiler.client.util.EPCompiledIOUtil;
import com.espertech.esper.runtime.client.DeploymentOptions;
import com.espertech.esper.runtime.client.EPDeployment;
import com.espertech.esper.runtime.client.EPRuntime;
import com.espertech.esper.runtime.client.EPRuntimeDestroyedException;
import com.espertech.esper.runtime.client.EPStatement;
import com.espertech.esper.runtime.client.EPUndeployException;
import com.espertech.esper.runtime.client.UpdateListener;
import com.espertech.esper.runtime.client.option.StatementNameRuntimeContext;
import com.espertech.esper.runtime.client.option.StatementNameRuntimeOption;

import prana.businessObjects.interfaces.IDisposable;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.constants.CustomRuleConstants;
import prana.esperCalculator.esperCEP.CepEventListenerExchange;
import prana.esperCalculator.esperCEP.CepEventListenerQueue;
import prana.esperCalculator.esperCEP.CepEventLoggingListener;
import prana.esperCalculator.esperCEP.CepEventLoggingListenerExchange;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import java.util.concurrent.locks.ReentrantReadWriteLock;

public class CEPManager {

	public static EPRuntime _epRunTime;
	private static final ReentrantReadWriteLock _deploymentsLock = new ReentrantReadWriteLock(true);
/*
	 * Store the Enable rule with compression
	 */
	public static HashMap<String,String>ruletypeWithCompression = new HashMap<String,String>();
	public static HashMap<String,String>CustomruletypeWithCompression = new HashMap<String,String>();
	public static int timerThreshold = 30000; // default value
	
	/*
	 * Get Rule name with its compression type
	 */
	public static void GetRuleNameWithCompression(String ruleNameWithCompression) {
		try {
			if (ruleNameWithCompression != null) {
				ruletypeWithCompression = new HashMap<String, String>();
				if (!ruleNameWithCompression.equals("")) {
					List<String> ruleList = Arrays.asList(ruleNameWithCompression.split(","));
					for (String part : ruleList) {
						String ruleData[] = part.split("=");
						String name = ruleData[0].trim();
						String compression = ruleData[1].trim();
						ruletypeWithCompression.put(name, compression);
					}
				}
				if (CustomruletypeWithCompression != null) {
					for (HashMap.Entry<String, String> mapElement : CustomruletypeWithCompression.entrySet()) {
						String key = mapElement.getKey();
						String value = mapElement.getValue();
						ruletypeWithCompression.put(key, value);
					}
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/*
	 * Update Custom rule information in the cache
	 */
	public static void UpdateCustomRuleWithCompression() {
		try {
			if (CustomruletypeWithCompression != null) {
				for (HashMap.Entry<String, String> mapElement : CustomruletypeWithCompression.entrySet()) {
					String key = mapElement.getKey();
					String value = mapElement.getValue();
					ruletypeWithCompression.put(key, value);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
	/**
	 * @return the _epRunTime
	 */
	public static EPRuntime getEPRuntime() {
		return _epRunTime;
	}

	public static void loadEplFiles(List<File> defaultEpls) throws Exception {
		try {
			DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss.SSS");

			// Define the timer threshold for while loop
			timerThreshold = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_PRICING_TIMEOUT));
			
			boolean IsEPLCompilationRequired = Boolean.parseBoolean(ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_IS_EPL_COMPILATION_REQUIRED));
			
			int NoOfDaysToCompileEPLAfter = Integer.parseInt(ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_NO_OF_DAYS_TO_COMPILE_EPL_AFTER));
			
			boolean monitorEnabled = Boolean.parseBoolean(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_START_STATEMENT_MONITOR));
			
			List<File> monitoringEpls = null;
			int monitoringEplsCount = 0;
			if (monitorEnabled) {
				String monitoringEplPath = ConfigurationHelper.getInstance().getValueBySectionAndKey(
						ConfigurationConstants.SECTION_APP_SETTINGS,
						ConfigurationConstants.KEY_APP_SETTINGS_MONITORING_EPL);
				// Loading Monitoring EPLs
				monitoringEpls = loadEPLs(monitoringEplPath);
				monitoringEplsCount = monitoringEpls.size();
			}
			
			int EPLCount = defaultEpls.size() + monitoringEplsCount;
			
			File directory = new File(ConfigurationConstants.COMPILED_JAR_FILES);
			
			int hoursElapsed= 0;
			
			if(directory.exists())
			{
				Date date = new Date();  
				Timestamp lastModifiedTime  = new Timestamp(directory.lastModified());
				Timestamp currentTime  = new Timestamp(date.getTime());
				// get time difference in seconds
			    long milliseconds = currentTime.getTime() - lastModifiedTime.getTime();
			    int seconds = (int) milliseconds / 1000;
			 
			    // calculate hours
			    hoursElapsed = seconds / 3600;
			    PranaLogManager.info("Last Modified Time in Compiled Jar: "+ lastModifiedTime);
			    PranaLogManager.info("Current Time: "+ currentTime);
			}
			
			if(directory.exists() && (EPLCount == directory.list().length) && (hoursElapsed <= NoOfDaysToCompileEPLAfter * 24) && !IsEPLCompilationRequired)
			{   
				File f = new File(ConfigurationConstants.COMPILED_JAR_FILES);
				List<File> compliedJars = CEPManager.loadEPLs(f.getAbsolutePath());
				// Need to improve this code
				File[] files = new File[compliedJars.size()];
				int i = 0;
				for (File jarFile : compliedJars) {
					files[i] = jarFile;
					i++;
				}

				Arrays.sort(files, new Comparator<File>() {
					public int compare(File f1, File f2) {
						return Long.valueOf(f1.lastModified()).compareTo(f2.lastModified());
					}
				});
				
				PranaLogManager.info("\n--- Deploying statements through compiled Jars ---, Starts at: " + dateFormat.format(new Date()));
				for (File jarFile : files) {
					compileDeployJarsAndAssignListener(jarFile);
				}
			}
			else
			{
				ArrayList<Module> listOfModule = new ArrayList<>();
				EPCompiler epComplier = EPCompilerProvider.getCompiler();

				for (File file : defaultEpls) {
					Module module = epComplier.readModule(file);
					listOfModule.add(module);
				}
				if (monitoringEpls != null) {
					for (File file : monitoringEpls) {
						Module module = epComplier.readModule(file);
						listOfModule.add(module);
					}
				}

				if (!directory.exists())
					directory.mkdir();
				else
					FileUtils.cleanDirectory(directory);

				PranaLogManager.info("\n--- Deploying statements ---, Starts at: " + dateFormat.format(new Date()));

				Set<String> emptySet = Collections.emptySet();
				ModuleOrder moduleOrder = ModuleOrderUtil.getModuleOrder(listOfModule, emptySet,
						new ModuleOrderOptions());
				for (Module mymodule : moduleOrder.getOrdered()) {
					compileDeployAndAssignListener(mymodule);
				}
			} 
			PranaLogManager.info("--- Deployed statements ---, Completed at: " + dateFormat.format(new Date()));
		} catch (EPRuntimeDestroyedException ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		} catch (IOException ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		} catch (ParseException ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		} catch (RuntimeException ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	public static void compileDeployJarsAndAssignListener(File jarFile) {
		try {
			EPDeployment deployment = compileDeployJars(jarFile, "");
			for (EPStatement stmt : deployment.getStatements()) {
				// Events will be logged only when EventsLoging is set to true in the config
				// file
				Boolean loggingPermission = Boolean.parseBoolean(ConfigurationHelper.getInstance()
						.getValueBySectionAndKey(ConfigurationConstants.SECTION_APP_SETTINGS,
								ConfigurationConstants.KEY_APP_SETTINGS_EVENTS_LOGGING));

				if (loggingPermission) {
					// Adding Listener
					stmt.addListener(new CepEventLoggingListener("Nirvana.LoggingData"));
				}

				// PranaLogManager.info("---- Deployed statement: " + stmt.getName());
			}
		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}
	}

	/*
	 * Compiling, deploying and assign listener the Module by using epRunTime
	 */
	public static void compileDeployAndAssignListener(Module module) {
		try {
			EPDeployment deployment = compileDeploy(module, "");
			for (EPStatement stmt : deployment.getStatements()) {
				// Events will be logged only when EventsLoging is set to true in the config
				// file
				Boolean loggingPermission = Boolean.parseBoolean(ConfigurationHelper.getInstance()
						.getValueBySectionAndKey(ConfigurationConstants.SECTION_APP_SETTINGS,
								ConfigurationConstants.KEY_APP_SETTINGS_EVENTS_LOGGING));

				if (loggingPermission) {
					// Adding Listener
					stmt.addListener(new CepEventLoggingListener("Nirvana.LoggingData"));
				}

				// PranaLogManager.info("---- Deployed statement: " + stmt.getName());
			}
		} catch (Exception ex) {
			throw new RuntimeException(ex);
		}
	}

	public static List<File> loadEPLs(String eplDirectory) throws Exception {
		try {
			File folder = new File(eplDirectory);

			PranaLogManager.info("Files loaded from " + '"' + folder.getAbsolutePath() + '"');

			List<File> listOfFiles = (List<File>) FileUtils.listFiles(new File(folder.getAbsolutePath()),
					TrueFileFilter.INSTANCE, TrueFileFilter.INSTANCE);

			return listOfFiles;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/*
	 * Return the Esper directory path, we are using common files from Esper.
	 */
	public static String getEsperDirectoryPath(String path) throws Exception {
		try {
			String BasketService = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_BASKET);
			String EsperService = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_ESPER);
			String ReleaseModeBasket = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_RELEASE_MODE_BASKET);
			String ReleaseModeEsper = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_RELEASE_MODE_ESPER);
			String f = new File(path).getAbsolutePath();
			if (f.contains(BasketService))
				return f.replaceFirst(BasketService, EsperService);
			else
				return f.replaceFirst(ReleaseModeBasket, ReleaseModeEsper);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/*
	 * Adding constants for the custom rule. In version 8.x there is no
	 * "addVariable" runtime API.
	 */
	public static Configuration addConstantsForCustomRules(Configuration cepConfig) throws ConfigurationException {
		try {
			for (String clientName : ConfigurationHelper.getInstance()
					.getSection(ConfigurationConstants.SECTION_CUSTOM_RULE_PATH).keySet()) {
				PranaLogManager.info("Adding constants for " + clientName);
				File file = new File(ConfigurationHelper.getInstance()
						.getSection(ConfigurationConstants.SECTION_CUSTOM_RULE_PATH).get(clientName));

				XMLConfiguration configuration = new XMLConfiguration(getEsperDirectoryPath(file.getAbsolutePath()));
				int noOfQueues = 1;

				Object prop = configuration
						.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "." + CustomRuleConstants.RULE_CONF_NAME);

				if (prop instanceof Collection) {
					noOfQueues = ((Collection<?>) prop).size();
				}

				for (int i = 0; i < noOfQueues; i++) {
					Object prop3 = configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i + ")."
							+ CustomRuleConstants.RULE_CONF_CONSTS + "." + CustomRuleConstants.RULE_CONF_CONST
							+ "[@name]");

					if (prop3 != null) {
						int constCount = 1;
						if (prop3 instanceof Collection)
							constCount = ((Collection<?>) prop3).size();
						for (int x = 0; x < constCount; x++) {
							String name = (String) configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG
									+ "(" + i + ")." + CustomRuleConstants.RULE_CONF_CONSTS + "."
									+ CustomRuleConstants.RULE_CONF_CONST + "(" + x + ")." + "[@name]");
							String value = (configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG + "(" + i
									+ ")." + CustomRuleConstants.RULE_CONF_CONSTS + "."
									+ CustomRuleConstants.RULE_CONF_CONST + "(" + x + ")." + "[@value]")).toString()
											.replaceAll("\\[|\\]", "");
							String type = (String) configuration.getProperty(CustomRuleConstants.RULE_CONF_NODE_TAG
									+ "(" + i + ")." + CustomRuleConstants.RULE_CONF_CONSTS + "."
									+ CustomRuleConstants.RULE_CONF_CONST + "(" + x + ")." + "[@type]");
							if(type.equals("Combo"))
							{
								type = "String";
							}
							PranaLogManager.info("Name: " + name + ", type: " + type + ", value: " + value);
							cepConfig.getCommon().addVariable(name, type, value);
						}
					}
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return cepConfig;
	}

	public static void addAllListener() throws Exception {
		try {
			// Adding listeners to Exchange
			HashMap<String, String> exchangeListenerStatementList = ConfigurationHelper.getInstance()
					.getSection(ConfigurationConstants.SECTION_STATEMENT_EXCHANGE);

			for (String stmtString : exchangeListenerStatementList.keySet()) {
				if (stmtString != null) {
					String exchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
							ConfigurationConstants.SECTION_EXCHANGE_LIST,
							exchangeListenerStatementList.get(stmtString));
					if (exchangeName == null)
						exchangeName = "";

					getStatement(stmtString).addListener(new CepEventListenerExchange(exchangeName));
					PranaLogManager.info("Statement: " + stmtString + " is attached to exchange - " + exchangeName);
				}
			}

			// Adding listeners to queue
			HashMap<String, String> queueListenerStatementList = ConfigurationHelper.getInstance()
					.getSection(ConfigurationConstants.SECTION_STATEMENT_QUEUE);

			for (String stmtString : queueListenerStatementList.keySet()) {
				if (stmtString != null) {
					String queueName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
							ConfigurationConstants.SECTION_QUEUE_LIST, exchangeListenerStatementList.get(stmtString));
					if (queueName == null)
						queueName = "";
					getStatement(stmtString).addListener(new CepEventListenerQueue(queueName));
					PranaLogManager.info("Statement: " + stmtString + " is attached to Queue - " + queueName);
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	public static void addLoggingListener() throws Exception {
		try {
			boolean monitorEnabled = Boolean.parseBoolean(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_START_STATEMENT_MONITOR));
			setVariableValue("IsLoggingEnabled", monitorEnabled);
			PranaLogManager.info("Attaching Logging Listener.");

			if (monitorEnabled) {
				HashMap<String, String> exchangeListenerStatementList = ConfigurationHelper.getInstance()
						.getSection(ConfigurationConstants.SECTION_STATEMENT_LOGGING);

				for (String stmtString : exchangeListenerStatementList.keySet()) {
					if (stmtString != null) {

						String exchangeName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
								ConfigurationConstants.SECTION_EXCHANGE_LIST,
								exchangeListenerStatementList.get(stmtString));

						if (exchangeName == null)
							exchangeName = "";
						getStatement(stmtString).addListener(new CepEventLoggingListenerExchange(exchangeName));
						PranaLogManager.info("Statement: " + stmtString + " is attached to exchange - " + exchangeName);
					}
				}
			}
			PranaLogManager.info("Logging Listener Added.");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	public static void removeStatementListener() throws Exception {
		try {
			PranaLogManager.info("Removing Statement Listeners");
			HashMap<String, String> exchangeListenerStatementList = ConfigurationHelper.getInstance()
					.getSection(ConfigurationConstants.SECTION_STATEMENT_EXCHANGE);

			for (String stmtString : exchangeListenerStatementList.keySet()) {
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

			HashMap<String, String> queueListenerStatementList = ConfigurationHelper.getInstance()
					.getSection(ConfigurationConstants.SECTION_STATEMENT_EXCHANGE);
			for (String stmtString : queueListenerStatementList.keySet()) {
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
	}

	/*
	 * Execute Query
	 */
	public static EPFireAndForgetQueryResult executeQuery(String query) {
		CompilerArguments compilerArguments = new CompilerArguments();
		compilerArguments.getPath().add(_epRunTime.getRuntimePath());
		EPCompiled compiled = new EPCompiled(null, null);
		try {
			compiled = EPCompilerProvider.getCompiler().compileQuery(query, compilerArguments);
		} catch (EPCompileException ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
		return _epRunTime.getFireAndForgetService().executeQuery(compiled);
	}
	
	/**
	 * Sets the value of a variable and logs debug information such as the variable's name,
	 * value, and the calling class/method.
	 * <p>
	 * This method uses the current stack trace to capture the calling method and class
	 * and logs the details before attempting to set the variable value.
	 * </p>
	 * 
	 * @param variableName The name of the variable to set.
	 * @param variableValue The value to set the variable to.
	 */
	public static void setVariableWithDebugInfo(String variableName, Object variableValue) {
	    try {
	        // Get calling class and method details
	        StackTraceElement[] stackTrace = Thread.currentThread().getStackTrace();
	        StackTraceElement caller = stackTrace.length > 2 ? stackTrace[2] : stackTrace[1];

	        // Log calling details
	        PranaLogManager.logOnly("Setting variable: " + variableName + " with value: " + variableValue);
	        PranaLogManager.logOnly("Called from: " + caller.getClassName() + "." + caller.getMethodName() + " (Line "
	                + caller.getLineNumber() + ")");
	        
	        setVariableValue(variableName, variableValue);
	    } catch (Exception e) {
	        PranaLogManager.error(e);
	    }
	}

	/**
	 * Sets the value of a variable in the runtime environment based on the deployment ID.
	 * <p>
	 * This method iterates through all available variables in the runtime environment,
	 * finds the variable that matches the provided name, and updates its value.
	 * </p>
	 * 
	 * @param variableName The name of the variable to set.
	 * @param variableValue The value to set the variable to.
	 */
	public static void setVariableValue(String variableName, Object variableValue) {
	    try {
	        for (Entry<DeploymentIdNamePair, Object> s : _epRunTime.getVariableService().getVariableValueAll()
	                .entrySet()) {
	            if (s.getKey().getName().equals(variableName)) {
	                _epRunTime.getVariableService().setVariableValue(s.getKey().getDeploymentId(), variableName,
	                        variableValue);
	            }
	        }
	    } catch (Exception e) {
	        PranaLogManager.error(e);
	    }
	}

	/*
	 * Get Esper variable Name
	 */
	public static Object getVariableValue(String variableName) {

		try {
			for (Entry<DeploymentIdNamePair, Object> s : _epRunTime.getVariableService().getVariableValueAll()
					.entrySet()) {
				if (s.getKey().getName().equals(variableName)) {
					return _epRunTime.getVariableService().getVariableValue(s.getKey().getDeploymentId(), variableName);
				}
			}
		} catch (Exception e) {
			PranaLogManager.error(e);
		}
		return null;
	}
	
	/**
	 * Checks if the provided timer value (in milliseconds) exceeds a predefined threshold (33 seconds).
	 * If the threshold is exceeded, a log message is generated to notify about the excess time.
	 * This can help to identify potential delays or performance issues in processing.
	 *
	 * @param totalMilliSeconds The total elapsed time in milliseconds to be checked.
	 *                           If this value exceeds 33,000 milliseconds (33 seconds),
	 *                           the method will log a warning message.
	 *
	 * @throws Exception If any error occurs during the logging process, it will be caught and logged.
	 */
	public static void notifyIfTimerExceedsLimit(int totalMilliSeconds) {
		try {
			// Check if the totalMilliseconds exceeds the threshold
			if (totalMilliSeconds > timerThreshold) {
				// Get calling class and method details
				StackTraceElement[] stackTrace = Thread.currentThread().getStackTrace();
				StackTraceElement caller = stackTrace.length > 2 ? stackTrace[2] : stackTrace[1];

				PranaLogManager.info("Called from: " + caller.getClassName() + "." + caller.getMethodName() + " (Line "
						+ caller.getLineNumber() + "). Timer threshold exceeded: Total time of " + totalMilliSeconds
						+ " milliseconds has surpassed the " + timerThreshold + " -second limit.");
			}
		} catch (Exception e) {
			PranaLogManager.error(e);
		}
	}

	/*
	 * createEPL for expression, statementName and listener
	 */
	public static void createEPL(String expression, String statementName, UpdateListener listener) {
		EPStatement stmt = compileDeploy(expression, statementName).getStatements()[0];
		stmt.addListener(listener);
	}

	/*
	 * createEPL for expression
	 */
	public static void createEPL(String expression) {
		compileDeploy(expression, "");
	}

	/*
	 * createEPL for expression and statementName
	 */
	public static void createEPL(String expression, String statementName) {
		compileDeploy(expression, statementName);
	}

	public static EPDeployment compileDeployJars(File jarFile, String statementName) {
		try {
			EPCompiled compiled = EPCompiledIOUtil.read(jarFile);
			_deploymentsLock.writeLock().lock();
			try {
				EPDeployment deployment = _epRunTime.getDeploymentService().deploy(compiled);
				return deployment;
			} finally {
				_deploymentsLock.writeLock().unlock();
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			return null;
		}
	}

	/*
	 * Compiling, deploying and assign listener the Module by using epRunTime
	 */
	public static EPDeployment compileDeploy(Object expression, String statementName) {
		try {
			// Build compiler arguments
			CompilerArguments args = new CompilerArguments(_epRunTime.getConfigurationDeepCopy());

			// Make the existing EPL objects available to the compiler
			args.getPath().add(_epRunTime.getRuntimePath());

			// Compile
			EPCompiled compiled = expression instanceof Module
					? EPCompilerProvider.getCompiler().compile((Module) expression, args)
					: EPCompilerProvider.getCompiler().compile(expression.toString(), args);

			if (statementName == "") {
				File jarFile = getJarFile();
				EPCompiledIOUtil.write(compiled, jarFile);
			}

			EPDeployment deployment;
			
		// Serialize deployment mutation
		_deploymentsLock.writeLock().lock();
		try {
if (statementName != "") {
				DeploymentOptions options = new DeploymentOptions();
				options.setStatementNameRuntime(getStatementNameRuntime(statementName));
				deployment = _epRunTime.getDeploymentService().deploy(compiled, options);
			} else {
				deployment = _epRunTime.getDeploymentService().deploy(compiled);
			}
		} finally {
			_deploymentsLock.writeLock().unlock();
		}

			return deployment;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			return null;
		}
	}

	static int count = 1;

	private static File getJarFile() {
		File f = new File(ConfigurationConstants.COMPILED_JAR_FILES);
		String path = f.getAbsolutePath();

		String filename = count + ". compiled-" + UuidGenerator.generate() + ".jar";
		count++;
		File file = new File(path, filename);
		if (file.exists()) {
			PranaLogManager.logOnly("File already exists for file '" + file + "'");
		}

		return file;
	}

	/*
	 * Get the Statement Name Runtime
	 */
	public static StatementNameRuntimeOption getStatementNameRuntime(final String statementName) {
		StatementNameRuntimeOption statementNameRuntimeOption = new StatementNameRuntimeOption() {
			public String getStatementName(StatementNameRuntimeContext env) {
				return statementName;
			}
		};
		return statementNameRuntimeOption;
	}

	/*
	 * Undeploy an Statement
	 */
	public static void destroy(String statementName) {
		_deploymentsLock.writeLock().lock();
		try {

		String[] deployments = safeGetDeployments();
		for (String deploymentId : deployments) {
			EPDeployment deployment = _epRunTime.getDeploymentService().getDeployment(deploymentId);
			for (EPStatement stmt : deployment.getStatements()) {
				if (statementName.equals(stmt.getName())) {
					try {
						_epRunTime.getDeploymentService().undeploy(deploymentId);
					} catch (EPUndeployException ex) {
						PranaLogManager.error(ex.getMessage(), ex);
					}
				}
			}
		}
	
		} finally {
			_deploymentsLock.writeLock().unlock();
		}
}

	
	/**
	 * Snapshot deployments. Callers must hold appropriate read or write lock.
	 */
	private static String[] safeGetDeployments() {
		return _epRunTime.getDeploymentService().getDeployments();
	}

public static EPStatement getStatement(String statementName) {
		_deploymentsLock.readLock().lock();
		try {
			String[] deployments = safeGetDeployments();
			for (String deploymentId : deployments) {
				EPDeployment deployment = _epRunTime.getDeploymentService().getDeployment(deploymentId);
				for (EPStatement stmt : deployment.getStatements()) {
					if (statementName.equals(stmt.getName())) {
						return stmt;
					}
				}
			}
			return null;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	
		 finally {
			_deploymentsLock.readLock().unlock();
		}
}
	
	public static List<EPStatement> getStatementList(String statementName) {
		_deploymentsLock.readLock().lock();
			try {
			List<EPStatement> epStatements = new ArrayList<EPStatement>();
			String[] deployments = safeGetDeployments();
			for (String deploymentId : deployments) {
				EPDeployment deployment = _epRunTime.getDeploymentService().getDeployment(deploymentId);
				for (EPStatement stmt : deployment.getStatements()) {
					if (statementName.equals(stmt.getName())) {
						epStatements.add(stmt);
					}
				}
			}
			return epStatements;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	
		finally {
			_deploymentsLock.readLock().unlock();
		}
}
	
	

	public static EPStatement[] getStatements() {
		_deploymentsLock.readLock().lock();
			try {
			String[] deployments = safeGetDeployments();
			for (String deploymentId : deployments) {
				EPDeployment deployment = _epRunTime.getDeploymentService().getDeployment(deploymentId);
				return deployment.getStatements();
			}
			return null;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	
		 finally {
			_deploymentsLock.readLock().unlock();
		}
}
	
	/*
	 * Creates dynamic EOM for WhatIf flow.
	 */
	public static void createEOMBasedOnEnabledRules() throws Exception {
		try {
			ArrayList<String> preTradeWhatIfEomEvent = new ArrayList<String>();
			if (ruletypeWithCompression != null) {
				CEPManager.resetCompressionVariables();
				for (String compression : ruletypeWithCompression.values()) {
					String[] items = compression.split("~");
					for (String value : items) {
						value = value.contains("_") ? value.replace("_", "")
								: (value.contains("-") ? value.replace("-", "")
										: (value.contains(" ") ? value.replace(" ", "") : value));
						PranaLogManager.logOnly("Value : " + value);

						// Trade compressions
						if (value.equalsIgnoreCase("Trade") && !preTradeWhatIfEomEvent.contains("WhatIfAggregationTrade")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationTrade");
							CEPManager.setVariableValue("IsTradeCompressionEnabled", true);
						}
						// Account compressions
						else if (value.equalsIgnoreCase("AccountSymbol")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationAccountSymbol")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationAccountSymbol");
							CEPManager.setVariableValue("IsAccountSymbolCompressionEnabled", true);
						} else if (value.equalsIgnoreCase("AccountUnderlyingSymbol")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationAccountUnderlyingSymbol")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationAccountUnderlyingSymbol");
							CEPManager.setVariableValue("IsAccountUnderlyingSymbolCompressionEnabled", true);
						} else if (value.equalsIgnoreCase("Account")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationAccount")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationAccount");
							CEPManager.setVariableValue("IsAccountCompressionEnabled", true);
						}
						// MasterFund compressions
						else if (value.equalsIgnoreCase("MasterFundSymbol")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationMasterFundSymbol")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationMasterFundSymbol");
							CEPManager.setVariableValue("IsMasterFundSymbolCompressionEnabled", true);
						} else if (value.equalsIgnoreCase("MasterFundUnderlyingSymbol")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationMasterFundUnderlyingSymbol")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationMasterFundUnderlyingSymbol");
							CEPManager.setVariableValue("IsMasterFundUnderlyingSymbolCompressionEnabled", true);
						} else if (value.equalsIgnoreCase("MasterFund")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationMasterFund")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationMasterFund");
							CEPManager.setVariableValue("IsMasterFundCompressionEnabled", true);
						}
						// Symbol compression
						else if (value.equalsIgnoreCase("UnderlyingSymbol")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationUnderlyingSymbol")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationUnderlyingSymbol");
							CEPManager.setVariableValue("IsUnderlyingSymbolCompressionEnabled", true);
						} else if (value.equalsIgnoreCase("Symbol")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationSymbol")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationSymbol");
							CEPManager.setVariableValue("IsSymbolCompressionEnabled", true);
						} else if (value.equalsIgnoreCase("Sector")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationSector")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationSector");
							CEPManager.setVariableValue("IsSectorCompressionEnabled", true);
						} else if (value.equalsIgnoreCase("SubSector")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationSubSector")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationSubSector");
							CEPManager.setVariableValue("IsSubSectorCompressionEnabled", true);
						} else if (value.equalsIgnoreCase("Asset")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationAsset")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationAsset");
							CEPManager.setVariableValue("IsAssetCompressionEnabled", true);
						} else if (value.equalsIgnoreCase("Global")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationGlobal")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationGlobal");
							CEPManager.setVariableValue("IsGlobalCompressionEnabled", true);
						} else if (value.equalsIgnoreCase("CustomMasterFundSymbol")
								&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationCustomMasterFundSymbol")) {
							preTradeWhatIfEomEvent.add("WhatIfAggregationCustomMasterFundSymbol");
							CEPManager.setVariableValue("IsCustomMasterFundSymbolEnabled", true);
						}
					}
				}
			}
			setGroupCompressionVariables();
			PranaLogManager.logOnly("-----------------------------------------" 
			+ "\nIsTradeCompressionEnabled = " + CEPManager.getVariableValue("IsTradeCompressionEnabled") 
			+ "\nIsAccountSymbolCompressionEnabled = " + CEPManager.getVariableValue("IsAccountSymbolCompressionEnabled")
			+ "\nIsAccountUnderlyingSymbolCompressionEnabled = " + CEPManager.getVariableValue("IsAccountUnderlyingSymbolCompressionEnabled") 
			+ "\nIsAccountCompressionEnabled = " + CEPManager.getVariableValue("IsAccountCompressionEnabled")
			+ "\nIsMasterFundSymbolCompressionEnabled = " + CEPManager.getVariableValue("IsMasterFundSymbolCompressionEnabled")
			+ "\nIsMasterFundUnderlyingSymbolCompressionEnabled = " + CEPManager.getVariableValue("IsMasterFundUnderlyingSymbolCompressionEnabled")
			+ "\nIsMasterFundCompressionEnabled = " + CEPManager.getVariableValue("IsMasterFundCompressionEnabled")
			+ "\nIsSymbolCompressionEnabled = " + CEPManager.getVariableValue("IsSymbolCompressionEnabled")
			+ "\nIsUnderlyingSymbolCompressionEnabled = " + CEPManager.getVariableValue("IsUnderlyingSymbolCompressionEnabled")
			+ "\nIsSectorCompressionEnabled = " + CEPManager.getVariableValue("IsSectorCompressionEnabled")
			+ "\nIsSubSectorCompressionEnabled = " + CEPManager.getVariableValue("IsSubSectorCompressionEnabled")
			+ "\nIsAssetCompressionEnabled = " + CEPManager.getVariableValue("IsAssetCompressionEnabled")
			+ "\nIsGlobalCompressionEnabled = " + CEPManager.getVariableValue("IsGlobalCompressionEnabled")
			+ "\nAnyMasterFundFlowCompressionEnabled = " + CEPManager.getVariableValue("AnyMasterFundFlowCompressionEnabled")
			+ "\nAnySymbolFlowCompressionEnabled = " + CEPManager.getVariableValue("AnySymbolFlowCompressionEnabled")
			+ "\nIsCustomMasterFundSymbolEnabled = " + CEPManager.getVariableValue("IsCustomMasterFundSymbolEnabled")
			+ "\n-----------------------------------------");
			
			/*
			 * If no user defined rule is enabled then we're adding AccountSymbol compression
			 * For WhatIfEOM
			 */
			if (!(boolean) CEPManager.getVariableValue("AnyGroupFlowCompressionEnabled")
					&& !preTradeWhatIfEomEvent.contains("WhatIfAggregationAccountSymbol")) {
				preTradeWhatIfEomEvent.add("WhatIfAggregationAccountSymbol");
				CEPManager.setVariableValue("AnyGroupFlowCompressionEnabled", true);
			}
			
			RuleDeploymentManagerCommon.generateAndDeployWhatIfEom(preTradeWhatIfEomEvent);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
	
	/*
	 * Reset all compressions variables.
	 */
	public static void resetCompressionVariables() {
		try {
			CEPManager.setVariableValue("IsTradeCompressionEnabled", false);
			CEPManager.setVariableValue("IsAccountSymbolCompressionEnabled", false);
			CEPManager.setVariableValue("IsAccountUnderlyingSymbolCompressionEnabled", false);
			CEPManager.setVariableValue("IsAccountCompressionEnabled", false);
			CEPManager.setVariableValue("IsMasterFundSymbolCompressionEnabled", false);
			CEPManager.setVariableValue("IsMasterFundUnderlyingSymbolCompressionEnabled", false);
			CEPManager.setVariableValue("IsMasterFundCompressionEnabled", false);
			CEPManager.setVariableValue("IsSymbolCompressionEnabled", false);
			CEPManager.setVariableValue("IsUnderlyingSymbolCompressionEnabled", false);
			CEPManager.setVariableValue("IsSectorCompressionEnabled", false);
			CEPManager.setVariableValue("IsSubSectorCompressionEnabled", false);
			CEPManager.setVariableValue("IsAssetCompressionEnabled", false);
			CEPManager.setVariableValue("IsGlobalCompressionEnabled", false);
			CEPManager.setVariableValue("AnyMasterFundFlowCompressionEnabled", false);
			CEPManager.setVariableValue("AnySymbolFlowCompressionEnabled", false);
			CEPManager.setVariableValue("AnyGroupFlowCompressionEnabled", false);
			CEPManager.setVariableValue("IsCustomMasterFundSymbolEnabled", false);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
	
	/*
	 * Set bulk variables.
	 */
	public static void setGroupCompressionVariables() {
		try {
			if ((boolean) CEPManager.getVariableValue("IsMasterFundSymbolCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("IsMasterFundUnderlyingSymbolCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("IsMasterFundCompressionEnabled")) {
				CEPManager.setVariableValue("AnyMasterFundFlowCompressionEnabled", true);
			}
			if ((boolean) CEPManager.getVariableValue("IsSymbolCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("IsUnderlyingSymbolCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("IsSectorCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("IsSubSectorCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("IsAssetCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("IsGlobalCompressionEnabled")) {
				CEPManager.setVariableValue("AnySymbolFlowCompressionEnabled", true);
			}
			if ((boolean) CEPManager.getVariableValue("IsAccountSymbolCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("IsAccountUnderlyingSymbolCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("IsAccountCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("AnyMasterFundFlowCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("AnySymbolFlowCompressionEnabled")
					|| (boolean) CEPManager.getVariableValue("IsCustomMasterFundSymbolEnabled")) {
				CEPManager.setVariableValue("AnyGroupFlowCompressionEnabled", true);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}
