package prana.loggingTool.communication;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

import org.json.JSONObject;
import org.json.XML;

import prana.businessObjects.complianceLevel.Alert;
import prana.businessObjects.interfaces.IAmqpListenerCallback;
import prana.businessObjects.rule.RuleType;
import prana.loggingTool.constants.ComplianceLevelConstants;
import prana.loggingTool.constants.ConfigurationConstants;
import prana.utility.logging.PranaLogManager;
import prana.utility.objectMapper.JSONMapper;
//import prana.ruleEngineMediator.Constants;

//import prana.ruleEngineMediator.ConfigurationManager;

public class AmqpListenerRuleValidator implements IAmqpListenerCallback {

	RuleType _ruleType;

	SimpleDateFormat sdf = new SimpleDateFormat(
			ConfigurationConstants.SIMPLE_DATE_FORMAT_2);

	public AmqpListenerRuleValidator(RuleType ruleType) {
		try {
			this._ruleType = ruleType;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
		}
	}

	HashMap<String,HashMap<String, ArrayList<String>>> data = new HashMap<String, HashMap<String, ArrayList<String>>>();

	@SuppressWarnings("unchecked")
	@Override
	public void amqpDataReceived(String jsonReceivedData, String routingKey) {
		Alert alert = null;
		try {
			if (data == null)
				data = new HashMap<String, HashMap<String, ArrayList<String>>>();
			if (routingKey.equals("EomPreTrade")) {

				alert = new Alert();

				HashMap<String, Object> map = JSONMapper.getHashMap(jsonReceivedData);
				String basketId = map.get("basketId").toString();

				alert.setOrderId(map.get("basketId").toString());
				alert.setIsEOM(true);

				if (!data.containsKey(basketId))
					data.put(basketId, new HashMap<String, ArrayList<String>>());
				if (!data.get(basketId).containsKey(routingKey))
					data.get(basketId).put(routingKey, new ArrayList<String>());

				data.get(basketId).get(routingKey).add(jsonReceivedData);

				String jsonString = JSONMapper.getStringForObject(alert);

				PranaLogManager.info("EoM message");

				PranaLogManager.info(jsonString);

				HashMap<String, ArrayList<String>> basketData = new HashMap<String, ArrayList<String>>();
				basketData = (HashMap<String, ArrayList<String>>) data.get(basketId).clone();
				for (String key : basketData.keySet()) {
					File dir = new File("Log\\" + map.get("basketId").toString());
					if (!dir.exists())
						dir.mkdir();

					FileWriter writer = new FileWriter(dir + "\\" + key + ".xml");
					StringBuilder builder = new StringBuilder();
					builder.append("<");
					builder.append(key);
					builder.append(">");
					for (String str : basketData.get(key)) {
						JSONObject json = new JSONObject(str);
						builder.append(XML.toString(json));
					}
					builder.append("</");
					builder.append(key);
					builder.append(">");
					writer.write(builder.toString());
					writer.close();
					data.get(basketId).remove(key);

					addFileToZip(dir + "\\" + key + ".xml", builder.toString(), key);
					File deleteFile = new File(dir + "\\" + key + ".xml");
					deleteFile.delete();
				}
				data.remove(basketId);
				map = null;

			} else if (routingKey.equals(ComplianceLevelConstants.OTHER_LOGGING_KEY)) {
				PranaLogManager.logOnly(jsonReceivedData);
			} else {
				HashMap<String, Object> map = JSONMapper.getHashMap(jsonReceivedData);

				String basketId = map.get("basketId").toString();
				if (!data.containsKey(basketId))
					data.put(basketId, new HashMap<String, ArrayList<String>>());
				if (!data.get(basketId).containsKey(routingKey))
					data.get(basketId).put(routingKey, new ArrayList<String>());

				data.get(basketId).get(routingKey).add(jsonReceivedData);
				map = null;

			}

		} catch (Exception ex) {
			PranaLogManager.error("Error in " + _ruleType, ex);
			PranaLogManager.error(ex.getMessage(), ex);
		} finally {
			if (data.size() == 0) {
				data = null;
			}
			alert = null;
		}

	}

	/*
	 * private Object getComplianceObject(HashMap<String, Object> map, String
	 * routingKey) { Object complianceObj = new Object(); try {
	 * 
	 * switch (routingKey) {
	 * 
	 * case ComplianceLevelConstants.ACCOUNT_COMPLIANCE_LEVEL: Account
	 * complianceLevel = JSONMapper.getJavaTypeFromHashMap( map, Account.class);
	 * // .convertValue(map, Fund.class); complianceObj = complianceLevel;
	 * break;
	 * 
	 * case ComplianceLevelConstants.ACCOUNT_SYMBOL_COMPLIANCE_LEVEL:
	 * Account_Symbol account_SymbolCL = JSONMapper .getJavaTypeFromHashMap(map,
	 * Account_Symbol.class); complianceObj = account_SymbolCL; break;
	 * 
	 * case ComplianceLevelConstants.UNDERLYING_COMPLIANCE_LEVEL:
	 * UnderlyingSymbol underlyingSymbolCL = JSONMapper
	 * .getJavaTypeFromHashMap(map, UnderlyingSymbol.class); complianceObj =
	 * underlyingSymbolCL; break;
	 * 
	 * case ComplianceLevelConstants.GLOBAL_COMPLIANCE_LEVEL: Global globalCL =
	 * JSONMapper.getJavaTypeFromHashMap(map, Global.class); complianceObj =
	 * globalCL; break;
	 * 
	 * case ComplianceLevelConstants.MASTER_FUND_COMPLIANCE_LEVEL: MasterFund
	 * masterFundCL = JSONMapper.getJavaTypeFromHashMap( map, MasterFund.class);
	 * complianceObj = masterFundCL; break;
	 * 
	 * case ComplianceLevelConstants.TRADE_COMPLIANCE_LEVEL: Trade taxlotCL =
	 * JSONMapper.getJavaTypeFromHashMap(map, Trade.class); complianceObj =
	 * taxlotCL; break;
	 * 
	 * case ComplianceLevelConstants.MASTERFUND_SYMBOL_COMPLIANCE_LEVEL:
	 * MasterFund_Symbol masterFund_SymbolCL = JSONMapper
	 * .getJavaTypeFromHashMap(map, MasterFund_Symbol.class); complianceObj =
	 * masterFund_SymbolCL; break;
	 * 
	 * case ComplianceLevelConstants.SYMBOL_COMPLIANCE_LEVEL: Symbol symbolCL =
	 * JSONMapper.getJavaTypeFromHashMap(map, Symbol.class); complianceObj =
	 * symbolCL; break; case
	 * ComplianceLevelConstants.MASTERFUND_UNDERLYINGSYMBOL_COMPLIANCE_LEVEL:
	 * MasterFund_UnderlyingSymbol MasterFund_UnderlyingSymbolCL = JSONMapper
	 * .getJavaTypeFromHashMap(map, MasterFund_UnderlyingSymbol.class);
	 * complianceObj = MasterFund_UnderlyingSymbolCL; break;
	 * 
	 * case ComplianceLevelConstants.ACCOUNT_UNDERLYINGSYMBOL_COMPLIANCE_LEVEL:
	 * Account_UnderlyingSymbol account_UnderlyingSymbolCL = JSONMapper
	 * .getJavaTypeFromHashMap(map, Account_UnderlyingSymbol.class);
	 * complianceObj = account_UnderlyingSymbolCL; break;
	 * 
	 * case ComplianceLevelConstants.ASSET_COMPLIANCE_LEVEL: Asset AssetCL =
	 * JSONMapper.getJavaTypeFromHashMap(map, Asset.class); complianceObj =
	 * AssetCL; break;
	 * 
	 * case ComplianceLevelConstants.SUBSECTOR_COMPLIANCE_LEVEL: SubSector
	 * SubSectorCL = JSONMapper.getJavaTypeFromHashMap(map, SubSector.class);
	 * complianceObj = SubSectorCL; break;
	 * 
	 * case ComplianceLevelConstants.SECTOR_COMPLIANCE_LEVEL: Sector SectorCL =
	 * JSONMapper.getJavaTypeFromHashMap(map, Sector.class); complianceObj =
	 * SectorCL; break;
	 * 
	 * default: break;
	 * 
	 * }
	 * 
	 * } catch (Exception ex) { PranaLogManager.error(ex.getMessage(), ex); }
	 * return complianceObj;
	 * 
	 * }
	 */

	@Override
	public void amqpRecieverStarted() {

		PranaLogManager.info("Service started. RuleType: "
				+ String.valueOf(_ruleType));
	}

	@Override
	public void amqpRecieverStopped(String message, Exception ex) {
		PranaLogManager.info("Service stopped. RuleType: "
				+ String.valueOf(_ruleType));
		PranaLogManager.error(message, ex);
	}

	private void addFileToZip(String path, String data,String key) throws FileNotFoundException {

		FileOutputStream baos = new FileOutputStream(path.substring(0,path.length()-4) +".zip");
		try(ZipOutputStream zos = new ZipOutputStream(baos)) {

		 /* File is not on the disk, test.txt indicates
		    only the file name to be put into the zip */
		 ZipEntry entry = new ZipEntry(key + ".xml"); 

		 zos.putNextEntry(entry);
		 zos.write(data.getBytes());
		 zos.closeEntry();

		 /* use more Entries to add more files
		    and use closeEntry() to close each file entry */

		 } catch(IOException ioe) {
		   ioe.printStackTrace();
		 }
		}
	
}
