package prana.esperCalculator.commonCode;

import java.util.HashMap;

import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.rule.customRules.RuleDefinition;

import prana.utility.logging.PranaLogManager;

public class ImportExportListenerCommon implements IAmqpListenerCallback  {

	public static RuleDefinition importCustomRule(HashMap<String, Object> map) {
		RuleDefinition rule = new RuleDefinition();
		try {
			synchronized (map) {
				String packageName = map.get("packageName").toString();
				String oldRuleName = map.get("oldRuleName").toString();
				String directoryPath = map.get("directoryPath").toString();
				String newRuleName = map.get("newRuleName").toString();

				rule = RuleManagerCommon.importRule(packageName, oldRuleName, directoryPath, newRuleName);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);

		}
		return rule;
	}

	public static String exportCustomRules(HashMap<String, Object> map) throws Exception {
		String newPath = null;
		try {
			String packageName = map.get("packageName").toString();
			String ruleName = map.get("ruleName").toString();
			String directoryPath = map.get("directoryPath").toString();
			String ruleCategory = map.get("ruleCategory").toString();
			String ruleId = map.get("ruleId").toString();
			newPath = RuleManagerCommon.exportRule(packageName, ruleName, directoryPath, ruleCategory, ruleId);
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
		return newPath;
	}

	@Override
	public void amqpRecieverStarted() {
		try {
			PranaLogManager.info("Import/Export listener has been started");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
	}

	@Override
	public void amqpRecieverStopped(String message, Exception ex) {
		PranaLogManager.error(message, ex);
	}

	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		// TODO Auto-generated method stub
		
	}
}
