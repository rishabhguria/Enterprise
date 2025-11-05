package prana.esperCalculator.communication;

import java.util.HashMap;

import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.businessObjects.rule.customRules.RuleDefinition;
import prana.esperCalculator.commonCode.ImportExportListenerCommon;

import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

/**
 * Listens the refresh action
 * 
 * @author dewashish
 * 
 */
public class ImportExportListener implements IAmqpListenerCallback {

	private IAmqpSender _amqpSender;

	public ImportExportListener(IAmqpSender amqpSender) {
		_amqpSender = amqpSender;
	}

	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		try {

			// XXX:Adding root node as it is not received from C# code. Need to
			// change it.
			HashMap<String, Object> map = JSONMapper.getHashMap(jsonReceivedData);

			switch (routingKey) {
			case "ExportCustomRule":
				String newPath = ImportExportListenerCommon.exportCustomRules(map);
				map.put("filePath", newPath);
				map.put("responseType", "CustomRuleExportComplete");
				map.put("operationStatus", "true");
				String res = JSONMapper.getStringForObject(map);
				_amqpSender.sendData(res, "CustomRuleImportExportComplete");
				break;
			case "ImportCustomRule":
				RuleDefinition rule = ImportExportListenerCommon.importCustomRule(map);

				if (rule != null) {
					map.put("ruleId", rule.getRuleId());
					map.put("description", rule.getDescription());
					map.put("enabled", rule.getEnabled());
					map.put("operationStatus", "true");
				} else {
					map.put("operationStatus", "false");
				}
				map.put("responseType", "CustomRuleImportComplete");

				String importRes = JSONMapper.getStringForObject(map);
				_amqpSender.sendData(importRes, "CustomRuleImportExportComplete");
				break;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);

		}
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
}