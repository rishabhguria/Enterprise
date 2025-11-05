package prana.esperCalculator.shell;

import java.io.File;
import java.io.FileOutputStream;
import java.io.OutputStream;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.Set;

import net.lingala.zip4j.ZipFile;
import net.lingala.zip4j.model.ZipParameters;
import net.lingala.zip4j.model.enums.CompressionLevel;
import net.lingala.zip4j.model.enums.CompressionMethod;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;

import com.espertech.esper.common.client.EventBean;
import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;

/**
 * This class helps to output the current state of esper engine data into dump
 * files
 * 
 * @author dewashish
 * 
 */
class WindowDumpHandler {

	// Global hidden variables
	/**
	 * Tells the class if the Dump has to be cancelled
	 */
	private static boolean _cancelDump = false;

	/**
	 * Status whether the dump is in progress
	 */
	private static boolean _isDumpInProgress = false;

	/**
	 * This function dump all the named windows defined in configuration file
	 * 
	 * @param outputFileType
	 * @param name
	 * @param isCancellable
	 *            Determines whether the dump process can be cancelled by the user
	 */
	private static void dumpNamedWindow(String outputFileType, String name, boolean isCancellable) {
		try {

			_isDumpInProgress = true;

			if (outputFileType.equalsIgnoreCase("xml") || outputFileType.equalsIgnoreCase("json")) {
				String folderName = ConfigurationHelper.getInstance().getValueBySectionAndKey(
						ConfigurationConstants.SECTION_APP_SETTINGS,
						ConfigurationConstants.KEY_APP_SETTINGS_WINDOW_DUMP_DIRECTORY);

				HashMap<String, String> namedWindowList = ConfigurationHelper.getInstance()
						.getSection(ConfigurationConstants.SECTION_NAMED_WINDOW);

				File file = new File(folderName);
				if (file.exists())
					file.delete();
				file.mkdir();
				PranaLogManager.info("Starting dumping window output to directory: ./" + folderName + "/\nFileFormat: "
						+ outputFileType);
				boolean found = false;
				_cancelDump = false;

				if (outputFileType.equalsIgnoreCase("xml")) {
					if (name.equalsIgnoreCase("all")) {
						for (String windowName : namedWindowList.keySet()) {

							if (_cancelDump && isCancellable) {
								PranaLogManager.info("Dump was Cancelled by user");
								break;
							}

							writeResultToFile(
									getQueryResultXML(generateQueryForNamedWindow(namedWindowList.get(windowName)),
											windowName),
									windowName, folderName, outputFileType.toLowerCase());

						}
						found = true;

					} else {
						for (String key : namedWindowList.keySet()) {
							if (namedWindowList.get(key).equals(name)) {
								writeResultToFile(
										getQueryResultXML(generateQueryForNamedWindow(namedWindowList.get(key)), key),
										key, folderName, outputFileType.toLowerCase());
								found = true;

							}
						}
					}
				} else {
					if (name.equalsIgnoreCase("all")) {
						for (String windowName : namedWindowList.keySet()) {

							if (_cancelDump && isCancellable) {
								PranaLogManager.info("Dump was Cancelled by user");
								break;
							}

							writeResultToFile(
									getQueryResultJson(generateQueryForNamedWindow(namedWindowList.get(windowName)),
											windowName),
									windowName, folderName, outputFileType.toLowerCase());

						}
						found = true;
					} else {
						for (String key : namedWindowList.keySet()) {
							if (namedWindowList.get(key).equals(name)) {
								writeResultToFile(
										getQueryResultXML(generateQueryForNamedWindow(namedWindowList.get(key)), key),
										key, folderName, outputFileType.toLowerCase());
								found = true;

							}
						}
					}
				}
				if (!found)
					PranaLogManager.info("Invalid Window Name");

				PranaLogManager.info("Done dumping window output to file");
			} else
				PranaLogManager.info("Please enter valid file type xml/Json");
			_isDumpInProgress = false;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			_isDumpInProgress = false;
		}
	}

	/**
	 * This return the result string in JSON format for given query
	 * @param query
	 * @param name
	 * @return
	 */
	private static String getQueryResultJson(String query, String name) {

		PranaLogManager.info("Getting result for: " + name);
		StringBuilder res = new StringBuilder();// "Not Available";

		try {
			EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);
			res.append("{\n\"");
			res.append(name);
			res.append("\": [");

			for (EventBean event : result.getArray()) {
				StringBuilder buildTemp = new StringBuilder();
				String jsonRow = CEPManager.getEPRuntime().getRenderEventService().renderJSON(name, event);
				buildTemp.append(jsonRow.replaceFirst(name, ""));
				buildTemp.deleteCharAt(buildTemp.lastIndexOf("}"));
				buildTemp.deleteCharAt(0).delete(buildTemp.indexOf("\""), buildTemp.indexOf("\"") + 3);
				res.append(buildTemp.toString() + ",");
			}
			res.deleteCharAt(res.length() - 1);
			res.append("]\n}");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return res.toString();
	}

	/**
	 * This return the result string in XML format for given query
	 * 
	 * @param query
	 * @param name
	 * @return
	 */
	private static String getQueryResultXML(String query, String name) {
		PranaLogManager.info("Getting result for: " + name);
		StringBuilder res = new StringBuilder();// "Not Available";
		try {
			EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);
			res.append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			res.append("\n<");
			res.append(name);
			res.append("List>");

			for (EventBean event : result.getArray()) {
				StringBuilder buildTemp = new StringBuilder();
				String jsonRow = CEPManager.getEPRuntime().getRenderEventService().renderXML(name, event);
				buildTemp.append(jsonRow.replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""));
				res.append(buildTemp.toString());
			}			
			res.append("\n</");
			res.append(name);
			res.append("List>");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return res.toString();
	}

	/**
	 * This method generate query to output data for given named window
	 * 
	 * @param namedWindow
	 * @return
	 */
	private static String generateQueryForNamedWindow(String namedWindow) {
		StringBuilder builder = new StringBuilder();
		try {
			builder.append("Select * from ");
			builder.append(namedWindow);
		} catch (Exception ex) {

			PranaLogManager.error(ex);
		}
		return builder.toString();
	}

	/**
	 * This function output data into file
	 * 
	 * @param result
	 * @param fileName
	 * @param folderName
	 */
	private static void writeResultToFile(String result, String fileName, String folderName, String extension) {
		try {
			if (result != null && !result.isEmpty()) {
				byte bWrite[] = result.getBytes();
				OutputStream os = new FileOutputStream(folderName + "/" + fileName + "." + extension);
				// for (int x = 0; x < bWrite.length; x++) {
				os.write(bWrite, 0, bWrite.length); // Writes the bytes
				// }
				os.close();
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public static void dump(HashMap<String, String> tokenizedCommand, boolean isCancellable) {
		try {
			if (tokenizedCommand == null)
				return;

			String outPutType = "xml";
			String windowName = "all";

			if (tokenizedCommand.size() > 1 && !validatedCommands(tokenizedCommand.keySet())) {
				PranaLogManager.info("Not valid Comand");
				return;
			}
			if (tokenizedCommand.containsKey("-f"))
				outPutType = tokenizedCommand.get("-f").toLowerCase();

			if (tokenizedCommand.containsKey("-n"))
				windowName = tokenizedCommand.get("-n");

			dumpNamedWindow(outPutType, windowName, isCancellable);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private static boolean validatedCommands(Set<String> keySet) {
		try {
			ArrayList<String> switches = new ArrayList<String>();
			switches.add("Command");
			switches.add("-f");
			switches.add("-n");
			for (String key : keySet) {
				if (!switches.contains(key))
					return false;
			}
			return true;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}

	/**
	 * The dump has to be cancelled
	 */
	public static void cancelDump() {
		try {
			_cancelDump = true;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Returns wether dumping is currently in progress
	 * 
	 * @return
	 */
	public static boolean isDumpinProgress() {
		try {
			return _isDumpInProgress;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			return false;
		}
	}

	/**
	 * Zip the current Dump with Date and Time
	 */
	public static void zipLastDump() {
		try {
			String dumpFolder = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_WINDOW_DUMP_DIRECTORY);
			String detectionDumpFolder = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_WINDOW_DETECTION_DUMP_DIRECTORY);

			File file = new File(detectionDumpFolder);
			File file1 = new File(dumpFolder);
			if (!file.exists())
				file.mkdir();

			String path = detectionDumpFolder + "\\" + new SimpleDateFormat("yyyyMMdd_HH_mm'.zip'").format(new Date());
			ZipFile zipFile = new ZipFile(path);
			ZipParameters parameters = new ZipParameters();
			parameters.setCompressionMethod(CompressionMethod.DEFLATE);
			parameters.setCompressionLevel(CompressionLevel.NORMAL);
			zipFile.addFolder(file1, parameters);
			PranaLogManager.info("Dump was zipped sucessfully : " + path);

		} catch (Exception ex) {
			PranaLogManager.info("Could not zip dump data");
			PranaLogManager.error(ex);
		}
	}
}