package prana.ruleEngineMediator.communication;

import java.util.HashMap;

import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.interfaces.IAmqpSender;
import prana.ruleEngineMediator.ruleService.ShardineUtility;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;

public class AmqpListenerImportExportRequest implements IAmqpListenerCallback {

	private IAmqpSender _amqpSender;

	public AmqpListenerImportExportRequest(IAmqpSender amqsender) {
		try {

			_amqpSender = amqsender;

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void amqpRecieverStarted() {

		try {

			PranaLogManager.info("Import Export Listener started");

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	@Override
	public void amqpRecieverStopped(String message, Exception ex) {
		PranaLogManager.error(ex,message);
	}

	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		try {

			// XXX:Adding root node as it is not received from C# code. Need to
			// change it.
			HashMap<String, Object> map = JSONMapper.getHashMap("{\"HashMap\":"
					+ jsonReceivedData + "}");

			switch (routingKey) {
			case "ExportUserDefinedRule":
				String newPath = exportUserDefinedRules(map);
				map.put("filePath", newPath);
				map.put("responseType", "UserDefinedExportComplete");
				map.put("operationStatus", "true");
				//HashMap<String, String> path = new HashMap<>();
				//path.put("FilePath", newPath);
				String res = JSONMapper.getStringForObject(map);
				_amqpSender.sendData(res, "UserDefinedExportComplete");
				break;
			case "ImportUserDefinedRule":
				HashMap<String, HashMap<String, String>> response = importUserDefinedRule(map);
				for (String key : response.keySet()) {
					response.get(key).put("operationStatus", "true");
					response.get(key).put("ActionUser", String.valueOf(map.get("ActionUser")));//appending userid for every rule
				}
				/*
				 * map.put("ruleId", response); map.put("responseType",
				 * "UserDefinedImportComplete");
				 */
				String importRes = JSONMapper.getStringForObject(response);

				_amqpSender.sendData(importRes, "UserDefinedImportComplete");
				break;
			}

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private HashMap<String, HashMap<String, String>> importUserDefinedRule(
			HashMap<String, Object> map) {
		HashMap<String, HashMap<String, String>> response = new HashMap<String, HashMap<String, String>>();
		try {
			synchronized (map) {
				String packageName = map.get("packageName").toString();
				String oldRuleName = map.get("oldRuleName").toString();
				String directoryPath = map.get("directoryPath").toString();
				String newRuleName = map.get("newRuleName").toString();
				response = ShardineUtility.importRule(packageName, oldRuleName,
						directoryPath, newRuleName);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return response;

	}

	private String exportUserDefinedRules(HashMap<String, Object> map) {
		String newPath = null;
		try {
		synchronized (map) {
				String packageName = map.get("packageName").toString();
				String ruleName = map.get("ruleName").toString();
				String directoryPath = map.get("directoryPath").toString();
				String ruleCategory = map.get("ruleCategory").toString();
				newPath = ShardineUtility.exportRule(packageName, ruleName,
					directoryPath, ruleCategory);

			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
			return newPath;
		}

	}
