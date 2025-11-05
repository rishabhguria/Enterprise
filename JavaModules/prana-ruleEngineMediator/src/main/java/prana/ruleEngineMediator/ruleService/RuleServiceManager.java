package prana.ruleEngineMediator.ruleService;

import java.util.ArrayList;

import prana.businessObjects.complianceLevel.Alert;
import prana.businessObjects.rule.RuleType;
import prana.utility.logging.PranaLogManager;

public class RuleServiceManager {

	private static Object _preTradeLockerObject = new Object();
	private static RuleValidator _preTradeRuleValidator;

	private static Object _postTradeLockerObject = new Object();
	private static RuleValidator _postTradeRuleValidator;

	public static void initializeRuleService() throws Exception {

		try {
			loadPrePostRuleServices();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	private static void loadPrePostRuleServices() throws Exception {
		try {
			_preTradeRuleValidator = new RuleValidator(RuleType.PreTrade);
			_postTradeRuleValidator = new RuleValidator(RuleType.PostTrade);
			ShardineUtility.GetEnabledRules();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	public static ArrayList<Alert> applyRule(Object complianceObj,
			RuleType ruleType) {

		try {
			if (ruleType == RuleType.PreTrade || ruleType == RuleType.Basket) {
				synchronized (_preTradeLockerObject) {
					return _preTradeRuleValidator.applyRule(complianceObj);
				}
			} else if (ruleType == RuleType.PostTrade) {
				synchronized (_postTradeLockerObject) {
					return _postTradeRuleValidator.applyRule(complianceObj);
				}
			} else
				throw new Exception("Rule type is not identified");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}
	}

	public static boolean buildPreTradePackage() {
		try {
			synchronized (_preTradeLockerObject) {
				_preTradeRuleValidator.buildSession();
			}
			return true;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	public static boolean buildPostTradePackage() {
		try {
			synchronized (_postTradeLockerObject) {
				_postTradeRuleValidator.buildSession();
			}
			return true;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	public static boolean buildPackage() {

		try {
			return buildPreTradePackage() && buildPostTradePackage();

			/*synchronized (_postTradeLockerObject) {
				_postTradeRuleValidator.buildSession();
			}

			synchronized (_preTradeLockerObject) {
				_preTradeRuleValidator.buildSession();
			}
			return true;*/
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}
}
