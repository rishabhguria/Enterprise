package prana.ruleEngineMediator.ruleService;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

import org.drools.KnowledgeBase;
import org.drools.KnowledgeBaseFactory;
import org.drools.QueryResult;
import org.drools.QueryResults;
import org.drools.builder.KnowledgeBuilder;
import org.drools.builder.KnowledgeBuilderErrors;
import org.drools.builder.KnowledgeBuilderFactory;
import org.drools.builder.ResourceType;
import org.drools.command.Command;
import org.drools.command.CommandFactory;
import org.drools.io.ResourceFactory;
import org.drools.io.impl.UrlResource;
import org.drools.runtime.ExecutionResults;
import org.drools.runtime.StatefulKnowledgeSession;
import org.drools.runtime.rule.FactHandle;
import org.drools.runtime.rule.impl.NativeQueryResults;

import prana.businessObjects.complianceLevel.Alert;
import prana.businessObjects.rule.RuleType;
import prana.ruleEngineMediator.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

public class RuleValidator {

	private StatefulKnowledgeSession _validatorSession;
	private Object _sessionLockerObject = new Object();

	private RuleType _ruleType;

	private String _packageName;
	private String _url;
	private String _ruleServerUserId;
	private String _ruleServerpassword;

	RuleValidator(RuleType ruleType) throws Exception {
		try {
			this._ruleType = ruleType;
			setProperties();
			buildSession();
		} catch (Exception ex) {
			PranaLogManager.error(ex,"Some rules has build error. Please check exception thread below.");
		}
	}

	private void setProperties() throws Exception {

		try {
			if (this._ruleType == RuleType.PostTrade)
				_packageName = ConfigurationConstants.POST_TRADE_COMPLIANCE;
			else if (this._ruleType == RuleType.PreTrade)
				_packageName = ConfigurationConstants.PRE_TRADE_COMPLIANCE;

			String vhost = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);

			this._url = "http://"
					+ ConfigurationHelper
							.getInstance()
							.getValueBySectionAndKey(
									ConfigurationConstants.SECTION_APP_SETTINGS,
									ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER)
					+ "/rest/packages/" + _packageName + "_" + vhost
					+ "/source";

			this._ruleServerUserId = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_USERID);

			this._ruleServerpassword = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_PASSWORD);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	void buildSession() throws Exception {

		try {
			PranaLogManager.info("Loading session for " + _packageName
					+ " from " + _url);

			KnowledgeBase knowledgeBase = null;
			knowledgeBase = readKBaseUsingKbuilder(_packageName);

			if (knowledgeBase == null)
				throw new Exception("Could not fetch knowledge base");
			else {
				synchronized (_sessionLockerObject) {
					_validatorSession = knowledgeBase
							.newStatefulKnowledgeSession();

					_validatorSession
							.addEventListener(new WorkingMemoryEventListnerImp());

				}
				PranaLogManager.info("Session loaded");
			}

		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	private KnowledgeBase readKBaseUsingKbuilder(String PackageName)
			throws Exception {

		try {
			KnowledgeBase kbase = null;

			UrlResource sourceCodeResource = (UrlResource) ResourceFactory
					.newUrlResource(_url);
			sourceCodeResource.setBasicAuthentication("enabled");
			sourceCodeResource.setUsername(_ruleServerUserId);
			sourceCodeResource.setPassword(_ruleServerpassword);
			KnowledgeBuilder kbuilder = KnowledgeBuilderFactory
					.newKnowledgeBuilder();
			kbuilder.add(sourceCodeResource, ResourceType.DRL);

			if (kbuilder.hasErrors()) {

				PranaLogManager.error("Error in building knowledgebase",
						(Throwable) kbuilder.getErrors());

				KnowledgeBuilderErrors buildError = kbuilder.getErrors();

				throw new Exception(buildError.toString());
			} else {
				kbase = KnowledgeBaseFactory.newKnowledgeBase();
				kbase.addKnowledgePackages(kbuilder.getKnowledgePackages());
			}
			return kbase;
		}

		catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	/**
	 * Apply rule on received esper stream.
	 * 
	 * @param obj
	 */
	public ArrayList<Alert> applyRule(Object complianceObj) {

		try {
			ArrayList<Alert> alertList = new ArrayList<>();
			synchronized (_sessionLockerObject) {
				FactHandle factHandle = _validatorSession.insert(complianceObj);
				@SuppressWarnings("rawtypes")
				List<Command> commands = new ArrayList<Command>();
				commands.add(CommandFactory.newFireAllRules());
				commands.add(CommandFactory.newQuery("outputObj",
						"GetOutputObj"));
				ExecutionResults results = _validatorSession
						.execute(CommandFactory.newBatchExecution(commands));
				QueryResults queryResults = ((NativeQueryResults) results
						.getValue("outputObj")).getResults();
				Iterator<QueryResult> iter = queryResults.iterator();
				while (iter.hasNext()) {
					QueryResult result = iter.next();
					Object resultObj = result.get("compliance");
					if (resultObj instanceof Alert) {
						alertList.add((Alert) resultObj);
						// break;
					}
				}
				_validatorSession.retract(factHandle);
			}
			return alertList;

		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;

		}

	}
}
