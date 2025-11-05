package prana.ruleEngineMediator.ruleService;

import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.StringWriter;
import java.net.URI;
import java.net.URL;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.apache.commons.configuration.XMLConfiguration;
import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.auth.AuthScope;
import org.apache.http.auth.UsernamePasswordCredentials;
import org.apache.http.client.methods.HttpDelete;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;

import prana.businessObjects.rule.RuleType;
import prana.ruleEngineMediator.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.fileIO.FileHelper;
import prana.utility.logging.PranaLogManager;
import prana.utility.ruleFormatting.RuleFormatting;

import com.googlecode.sardine.Sardine;
import com.googlecode.sardine.SardineFactory;

//import com.googlecode.sardine.util.SardineException;

public class ShardineUtility {

	
	/*
	 *  Store the Information of rule name with Type and it's Compression Level
	 */
	public static LinkedHashMap<String,Object> _ruleNameWithCompression =new LinkedHashMap<String,Object>();
	
	/**
	 * Rename assets
	 * 
	 * @param oldRuleName
	 * @param newRuleName
	 * @param packageName
	 * @return
	 * @throws Exception
	 */
	public static HashMap<String, HashMap<String, String>> renameAsset(
			String oldRuleName, String newRuleName, String packageName)
			throws Exception {

		try {
			// String[] renameParam = renameStr.split(":");
			String vHost = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String ruleServer = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);
			String ruleServerUserId = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_USERID);

			String ruleServerPassword = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_PASSWORD);

			Sardine sardine = SardineFactory.begin(ruleServerUserId,
					ruleServerPassword);
			String webDavUrl = "http://" + ruleServer
					+ "/org.drools.guvnor.Guvnor/webdav/packages/"
					+ packageName + "Compliance_" + vHost + "/";
			String oldAssetNameURL = (new URL(
					(webDavUrl
							+ RuleFormatting
									.getGuvnorFormattedRule(oldRuleName) + ".brl")
							.trim())).toString();
			String newAssetNameURL = (new URL(
					(webDavUrl
							+ RuleFormatting
									.getGuvnorFormattedRule(newRuleName) + ".brl")
							.trim())).toString();

			sardine.move(oldAssetNameURL, newAssetNameURL);
			deleteRules(packageName, oldRuleName);
			String baseUrl = "http://" + ruleServer + "/rest/packages/"
					+ packageName + "Compliance" + "_" + vHost + "/assets/"
					+ RuleFormatting.getGuvnorFormattedRule(newRuleName);

			ArrayList<String> listOfAssets = new ArrayList<String>();
			listOfAssets.add(baseUrl);

			HashMap<String, HashMap<String, String>> ruleList = loadRuleFromAsset(
					listOfAssets, packageName, ruleServerUserId,
					ruleServerPassword, "RenameRule");
			for (String key : ruleList.keySet()) {
				boolean enable=Boolean.parseBoolean(ruleList.get(key).get("enabled"));
				EnableDisableRules(ruleList.get(key).get("ruleType"), ruleList.get(key).get("ruleName"), !enable);
			}
			PranaLogManager.logOnly("Rule " + oldRuleName + " Renamed to "
					+ newRuleName);
			GetEnabledRules();
			return ruleList;
			// sardine.delete(oldAssetNameURL);

		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;

		}

	}

	public static String exportRule(String packageName, String name,
			String directoryPath, String ruleCategory) throws Exception {

		try {
			String ruleName = RuleFormatting.getGuvnorFormattedRule(name);
			StringBuilder newDirectoryPath = new StringBuilder();
			newDirectoryPath.append(FileHelper.createDirectory(directoryPath,
					packageName, name, ruleCategory));

			String vHost = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String ruleServer = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);
			String ruleServerUserId = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_USERID);

			String ruleServerPassword = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_PASSWORD);

			Sardine sardine = SardineFactory.begin(ruleServerUserId,
					ruleServerPassword);
			String webDavUrl = "http://" + ruleServer
					+ "/org.drools.guvnor.Guvnor/webdav/packages/"
					+ packageName + "Compliance_" + vHost + "/" + ruleName
					+ ".brl";

			// sardine.
			InputStream inputStream = sardine.getInputStream(webDavUrl);

			FileHelper.writeStreamToFile(inputStream,
					newDirectoryPath.toString(), name, ruleCategory);
			PranaLogManager.logOnly("Rule " + ruleName + " Exported ");
			PranaLogManager.info("Rule Exported " + ruleName);
			GetEnabledRules();
			return newDirectoryPath.toString();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			throw ex;
		}

	}

	public static HashMap<String, HashMap<String, String>> importRule(
			String packageName, String oldRuleName, String directoryPath,
			String newRuleName) {
		try {
			String vHost = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String ruleServer = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);
			String ruleServerUserId = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_USERID);

			String ruleServerPassword = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_PASSWORD);

			Sardine sardine = SardineFactory.begin(ruleServerUserId,
					ruleServerPassword);
			String webDavUrl = "http://" + ruleServer
					+ "/org.drools.guvnor.Guvnor/webdav/packages/"
					+ packageName + "Compliance_" + vHost + "/"
					+ RuleFormatting.getGuvnorFormattedRule(newRuleName)
					+ ".brl";

			// FileReader file=new FileReader(directoryPath);
			InputStream ips = new FileInputStream(directoryPath + "\\"
					+ oldRuleName + ".xml");

			String baseUrl = "http://" + ruleServer + "/rest/packages/"
					+ packageName + "Compliance" + "_" + vHost + "/assets/"
					+ RuleFormatting.getGuvnorFormattedRule(newRuleName);

			// java.io.InputStream inputStream
			sardine.put(webDavUrl, ips);
			// URI url = new URI(baseUrl + "/isdisabled/" + true);

			EnableDisableRules(packageName, newRuleName, true);
			// disableRuleAt(url);

			ArrayList<String> listOfAssets = new ArrayList<String>();
			listOfAssets.add(baseUrl);

			HashMap<String, HashMap<String, String>> ruleList = loadRuleFromAsset(
					listOfAssets, packageName, ruleServerUserId,
					ruleServerPassword, "ImportRule");

			PranaLogManager.logOnly("Rule " + newRuleName + " Imported");
			PranaLogManager.info("Rule Imported");
			return ruleList;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}

	}

	private static void disableRuleAt(URI url) {
		// URL url = new URL ("http://ip:port/login");
		try {
			/*
			 * String authStr = "admin:admin"; String authEncoded =
			 * Base64.encodeBase64String(authStr.getBytes());
			 */
			
			
			String ruleServerUserId = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_USERID);

			String ruleServerPassword = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_PASSWORD);

			DefaultHttpClient httpclient = new DefaultHttpClient();

			httpclient.getCredentialsProvider().setCredentials(
					new AuthScope(getHostName(url.toString()), getHostPort(url.toString())),
					new UsernamePasswordCredentials(ruleServerUserId, ruleServerPassword));

			HttpGet httpget = new HttpGet(url);

			PranaLogManager.info("executing request" + httpget.getRequestLine());
			HttpResponse response = httpclient.execute(httpget);
			HttpEntity entity = response.getEntity();

			PranaLogManager.info("----------------------------------------");
			PranaLogManager.info(response.getStatusLine().toString());
			if (entity != null) {
				PranaLogManager.info("Response content length: "
						+ entity.getContentLength());
			}
			EntityUtils.consume(entity);

			/*
			 * HttpURLConnection connection = (sHttpURLConnection) url
			 * .openConnection(); connection.setRequestMethod("POST");
			 * connection.setDoOutput(true); ; connection.set
			 * connection.setRequestProperty("accept", "text/plain");
			 * InputStream content = (InputStream) connection.getInputStream();
			 * BufferedReader in = new BufferedReader(new InputStreamReader(
			 * content)); String line; while ((line = in.readLine()) != null) {
			 * PranaLogManager.info(line); }
			 */
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	public static HashMap<String, HashMap<String, String>> loadAllRules(
			String packageName) throws Exception {

		try {
			String vHost = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String ruleServer = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);
			String ruleServerUserId = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_USERID);

			String ruleServerPassword = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_PASSWORD);

			/*
			 * Sardine sardine = SardineFactory.begin(ruleServerUserId,
			 * ruleServerPassword);
			 */
			String webDavUrl = "http://" + ruleServer + "/rest/packages/"
					+ packageName + "Compliance" + "_" + vHost + "/assets";

			String baseXML = loadXMLFromUrl(webDavUrl, ruleServerUserId,
					ruleServerPassword);
			ArrayList<String> listOfAssets = getListOfAssetFromXML(baseXML);

			HashMap<String, HashMap<String, String>> ruleList = loadRuleFromAsset(
					listOfAssets, packageName, ruleServerUserId,
					ruleServerPassword, "LoadRules");

			PranaLogManager.info("Rule Loaded" + packageName);
			return ruleList;

		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	private static HashMap<String, HashMap<String, String>> loadRuleFromAsset(
			ArrayList<String> listOfAssets, String packageName,
			String ruleServerUserId, String ruleServerPassword, String operation)
			throws Exception {

		String vHost = ConfigurationHelper.getInstance()
				.getValueBySectionAndKey(
						ConfigurationConstants.SECTION_APP_SETTINGS,
						ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
		String ruleServer = ConfigurationHelper.getInstance()
				.getValueBySectionAndKey(
						ConfigurationConstants.SECTION_APP_SETTINGS,
						ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);
		HashMap<String, HashMap<String, String>> list = new HashMap<String, HashMap<String, String>>();
		for (String asset : listOfAssets) {

			String baseXML = loadXMLFromUrl(asset, ruleServerUserId,
					ruleServerPassword);

			XMLConfiguration configuration = new XMLConfiguration();
			configuration.load(new ByteArrayInputStream(baseXML
					.replace("atom:", "").replace("xml:", "").getBytes()));

			HashMap<String, String> tempMap = new HashMap<>();
			String nameN = (String) configuration.getProperty("title");
			tempMap.put("ruleName", RuleFormatting.getUIFormattedRule(nameN));
			String uuidN = (String) configuration
					.getProperty("metadata.uuid.value");
			tempMap.put("uuid", uuidN);
			// String ruleidN = UUID.randomUUID().toString();
			tempMap.put("ruleId", uuidN);
			tempMap.put("ruleType", packageName);
			String url = "http://" + ruleServer
					+ "/org.drools.guvnor.Guvnor/webdav/packages/"
					+ packageName + "Compliance" + "_" + vHost + "/" + nameN
					+ ".brl";

			tempMap.put("ruleUrl", url);

			String enabled = (String) getAttribueFor(ruleServer, vHost,
					packageName, nameN, "enabled");
			tempMap.put("enabled", enabled);
			tempMap.put("operationType", operation);

			list.put(RuleFormatting.getFormattedKey(uuidN), tempMap);

		}
		return list;
		}

	private static ArrayList<String> getListOfAssetFromXML(String baseXML)
			throws Exception {

		ArrayList<String> listOfAssets = new ArrayList<String>();
		XMLConfiguration configuration = new XMLConfiguration();

		configuration.load(new ByteArrayInputStream(baseXML
				.replace("atom:", "").replace("xml:", "").getBytes()));
		String node = "entry";
		int noOfQueues = 1;

		Object prop = configuration.getProperty(node + "[@base]");
		if (prop instanceof Collection) {
			noOfQueues = ((Collection<?>) prop).size();
		}
		for (int i = 0; i < noOfQueues; i++) {
			String format = (String) configuration.getProperty(node + "(" + i
					+ ").metadata.format.value");
			if (format.equals("brl")) {

				listOfAssets.add((String) configuration.getString(node + "("
						+ i + ")[@base]"));
			}

		}
		return listOfAssets;
		}

	private static String headerType = "application/atom+xml";

	private static String loadXMLFromUrl(String url, String userName,
			String password) throws Exception {
		try {
			DefaultHttpClient httpclient = new DefaultHttpClient();

			httpclient.getCredentialsProvider().setCredentials(
					new AuthScope(getHostName(url), getHostPort(url)),
					new UsernamePasswordCredentials(userName, password));

			HttpGet httpget = new HttpGet(url);
			httpget.addHeader("accept", headerType);
			HttpResponse response = httpclient.execute(httpget);
			HttpEntity entity = response.getEntity();

			InputStream inputStream = entity.getContent();

			InputStreamReader is = new InputStreamReader(inputStream);
			StringBuilder sb = new StringBuilder();
			BufferedReader br = new BufferedReader(is);
			String read = br.readLine();

			while (read != null) {
				sb.append(read);
				read = br.readLine();

			}
			return sb.toString();
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	/*
	 * private static HashMap<String, HashMap<String, String>> loadRuleList(
	 * String inputStream, String packageName) {
	 * 
	 * try { String vHost = ConfigurationHelper.getInstance()
	 * .getValueBySectionAndKey( ConfigurationConstants.SECTION_APP_SETTINGS,
	 * ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST); String ruleServer =
	 * ConfigurationHelper.getInstance() .getValueBySectionAndKey(
	 * ConfigurationConstants.SECTION_APP_SETTINGS,
	 * ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);
	 * 
	 * 
	 * InputStreamReader is = new InputStreamReader(inputStream); StringBuilder
	 * sb = new StringBuilder(); BufferedReader br = new BufferedReader(is);
	 * String read = br.readLine();
	 * 
	 * while (read != null) { sb.append(read); read
	 * = br.readLine();
	 * 
	 * }
	 * 
	 * 
	 * XMLConfiguration configuration = new XMLConfiguration();
	 * configuration.load(new ByteArrayInputStream(inputStream .replace("atom:",
	 * "").replace("xml:", "").getBytes())); String node = "entry"; int
	 * noOfQueues = 1; HashMap<String, HashMap<String, String>> list = new
	 * HashMap<String, HashMap<String, String>>();
	 * 
	 * Object prop = configuration.getProperty(node + ".title"); if (prop
	 * instanceof Collection) { noOfQueues = ((Collection<?>) prop).size(); }
	 * for (int i = 0; i < noOfQueues; i++) { String format = (String)
	 * configuration.getProperty(node + "(" + i + ").metadata.format.value"); if
	 * (format.equals("brl")) { HashMap<String, String> tempMap = new
	 * HashMap<>(); String nameN = (String) configuration.getProperty(node + "("
	 * + i + ").title"); tempMap.put("name", getUIFormattedRule(nameN)); String
	 * uuidN = (String) configuration.getProperty(node + "(" + i +
	 * ").metadata.uuid.value"); tempMap.put("uuid", uuidN); // String ruleidN =
	 * UUID.randomUUID().toString(); tempMap.put("ruleId", uuidN);
	 * tempMap.put("ruleType", packageName); String url = "http://" + ruleServer
	 * + "/org.drools.guvnor.Guvnor/webdav/packages/" + packageName +
	 * "Compliance" + "_" + vHost + "/" + nameN + ".brl";
	 * 
	 * tempMap.put("ruleUrl", url);
	 * 
	 * String enabled = (String) getAttribueFor(ruleServer, vHost, packageName,
	 * nameN, "enabled"); tempMap.put("enabled", enabled);
	 * list.put(getFormattedKey(uuidN), tempMap); }
	 * 
	 * } return list; } catch (Exception ex) {
	 * PranaLogManager.error(ex.getMessage(), ex); return null; }
	 * 
	 * }
	 */

	private static Object getAttribueFor(String ruleServer, String vHost,
			String packageName, String ruleName, String attributeName) {

		try {
			String ruleServerUserId = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_USERID);

			String ruleServerPassword = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_PASSWORD);
			Sardine sardine = SardineFactory.begin(ruleServerUserId,
					ruleServerPassword);
			String webDavUrl = "http://" + ruleServer
					+ "/org.drools.guvnor.Guvnor/webdav/packages/"
					+ packageName + "Compliance" + "_" + vHost + "/" + ruleName
					+ ".brl";

			InputStream inputStream = sardine.getInputStream(webDavUrl);
			InputStreamReader is = new InputStreamReader(inputStream);
			StringBuilder sb = new StringBuilder();
			BufferedReader br = new BufferedReader(is);
			String read = br.readLine();

			while (read != null) {
				sb.append(read);
				read = br.readLine();

			}

			XMLConfiguration configuration = new XMLConfiguration();
			configuration.load(new ByteArrayInputStream(sb.toString()
					.getBytes()));
			String node = "attributes.attribute";
			int noOfQueues = 1;
			// HashMap<String, HashMap<String, String>> list = new
			// HashMap<String, HashMap<String, String>>();

			Object prop = configuration.getProperty(node + ".attributeName");
			if (prop instanceof Collection) {
				noOfQueues = ((Collection<?>) prop).size();
			}

			for (int i = 0; i < noOfQueues; i++) {
				String attributeNameN = (String) configuration.getProperty(node
						+ "(" + i + ").attributeName");
				if (attributeNameN.equals(attributeName)) {
					return configuration
							.getProperty(node + "(" + i + ").value");
				}
			}

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return false;
	}

	public static HashMap<String, HashMap<String, String>> CreateRule(
			String packageName, String ruleName) {
		try {
			/*
			 * String[] packageName = packageValue.split(":");
			 * 
			 * String[] rule = ruleName.split(":");
			 */

			String vHost = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String ruleServer = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);
			String ruleServerUserId = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_USERID);

			String ruleServerPassword = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_PASSWORD);

			String directoryPath = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULE_TEMPLATE);

			Sardine sardine = SardineFactory.begin(ruleServerUserId,
					ruleServerPassword);
			String webDavUrl = "http://" + ruleServer
					+ "/org.drools.guvnor.Guvnor/webdav/packages/"
					+ packageName + "Compliance_" + vHost + "/"
					+ RuleFormatting.getGuvnorFormattedRule(ruleName) + ".brl";
			File ruleTemplate = new File(directoryPath);
			InputStream ips = new FileInputStream(
					ruleTemplate.getAbsolutePath());

			sardine.put(webDavUrl, ips);

			String baseUrl = "http://" + ruleServer + "/rest/packages/"
					+ packageName + "Compliance" + "_" + vHost + "/assets/"
					+ RuleFormatting.getGuvnorFormattedRule(ruleName);

			URI url = new URI(baseUrl + "/isdisabled/" + true);
			disableRuleAt(url);

			/*
			 * String baseXML = loadXMLFromUrl(baseUrl, ruleServerUserId,
			 * ruleServerPassword);
			 */
			ArrayList<String> listOfAssets = new ArrayList<String>();
			listOfAssets.add(baseUrl);

			HashMap<String, HashMap<String, String>> ruleList = loadRuleFromAsset(
					listOfAssets, packageName, ruleServerUserId,
					ruleServerPassword, "AddRule");
			PranaLogManager.logOnly("Rule " + ruleName + " Created");
			PranaLogManager.info("RuleCreated");
			GetEnabledRules();
			return ruleList;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return null;
		}

	}

	public static boolean deleteRules(String packageName, String ruleName) {

		try {
			String ruleServer = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);
			String ruleServerUserId = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_USERID);

			String ruleServerPassword = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_PASSWORD);

			String vHost = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			/*
			 * Sardine sardine = SardineFactory.begin(ruleServerUserId,
			 * ruleServerPassword);
			 */
			String url = "http://" + ruleServer + "/rest/packages/"
					+ packageName + "Compliance_" + vHost + "/" + "assets/"
					+ RuleFormatting.getGuvnorFormattedRule(ruleName);

			DefaultHttpClient httpclient = new DefaultHttpClient();

			httpclient.getCredentialsProvider().setCredentials(
					new AuthScope(getHostName(url), getHostPort(url)),
					new UsernamePasswordCredentials(ruleServerUserId,
							ruleServerPassword));

			// HttpPost httpPost = new HttpPost(url);
			HttpDelete delete = new HttpDelete(url);
			// httpPost.addHeader("accept", headerType);
			// delete.addHeader("DELETE", "");
			HttpResponse response = httpclient.execute(delete);
			org.apache.http.StatusLine res = response.getStatusLine();
			if (res.getStatusCode() == 204) {
				PranaLogManager.logOnly("Rule " + ruleName + " Deleted");
				GetEnabledRules();
				return true;
			} else
				return false;

		} catch (Exception e) {
			PranaLogManager.error(e);
			e.printStackTrace();
			return false;
		}

	}

	public static boolean EnableDisableRules(String packageName,
			String ruleName, boolean isDisable) {

		try {

			String vHost = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String ruleServer = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);

			String baseUrl = "http://" + ruleServer + "/rest/packages/"
					+ packageName + "Compliance" + "_" + vHost + "/assets/"
					+ RuleFormatting.getGuvnorFormattedRule(ruleName);

			setAttribueFor(ruleServer, vHost, packageName,
					RuleFormatting.getGuvnorFormattedRule(ruleName), "enabled",
					!isDisable);
            	 GetEnabledRules();			
			URI url = new URI(baseUrl + "/isdisabled/" + isDisable);
			disableRuleAt(url);
			if (isDisable) {
				PranaLogManager.logOnly("Rule " + ruleName + " Disabled ");
			} else {
				PranaLogManager.logOnly("Rule " + ruleName + " Enabled");
			}
			return true;
		} catch (Exception e) {
			// TODO Auto-generated catch block
			PranaLogManager.error(e);
			e.printStackTrace();
			return false;
		}

	}

	private static Object setAttribueFor(String ruleServer, String vHost,
			String packageName, String ruleName, String attributeName,
			Object attributeValue) {

		try {
			String ruleServerUserId = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_USERID);

			String ruleServerPassword = ConfigurationHelper
					.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_PASSWORD);
			Sardine sardine = SardineFactory.begin(ruleServerUserId,
					ruleServerPassword);
			String webDavUrl = "http://" + ruleServer
					+ "/org.drools.guvnor.Guvnor/webdav/packages/"
					+ packageName + "Compliance" + "_" + vHost + "/" + ruleName
					+ ".brl";

			InputStream inputStream = sardine.getInputStream(webDavUrl);
			InputStreamReader is = new InputStreamReader(inputStream);
			StringBuilder sb = new StringBuilder();
			BufferedReader br = new BufferedReader(is);
			String read = br.readLine();

			while (read != null) {
				sb.append(read);
				read = br.readLine();

			}

			XMLConfiguration configuration = new XMLConfiguration();
			configuration.setAttributeSplittingDisabled(true);
			configuration.setDelimiterParsingDisabled(true);
			configuration.load(new ByteArrayInputStream(sb.toString()
					.getBytes()));
			String node = "attributes.attribute";
			int noOfQueues = 1;
			// HashMap<String, HashMap<String, String>> list = new
			// HashMap<String, HashMap<String, String>>();

			Object prop = configuration.getProperty(node + ".attributeName");
			if (prop instanceof Collection) {
				noOfQueues = ((Collection<?>) prop).size();
			}

			for (int i = 0; i < noOfQueues; i++) {
				String attributeNameN = (String) configuration.getProperty(node
						+ "(" + i + ").attributeName");
				if (attributeNameN.equals(attributeName)) {
					configuration.setProperty(node + "(" + i + ").value",
							attributeValue);
				}
			}
			StringWriter stringWriter = new StringWriter();
			configuration.save(stringWriter);
			InputStream ips = new ByteArrayInputStream(stringWriter.toString()
					.getBytes());
			// java.io.InputStream inputStream
			sardine.put(webDavUrl, ips);

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return true;
	}

	/**
	 * Returns enable state for rule in package.
	 * @param packageName
	 * @param ruleName
	 * @return
	 */
	public static Object getEnableStateAfterBuild(String packageName,
			String ruleName) {

		String vHost = "";
		String ruleServer = "";
		try {
			vHost = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			ruleServer = ConfigurationHelper.getInstance()
					.getValueBySectionAndKey(
							ConfigurationConstants.SECTION_APP_SETTINGS,
							ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);
			return getAttribueFor(ruleServer, vHost, packageName,
					RuleFormatting.getGuvnorFormattedRule(ruleName), "enabled");
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}

	}
	
	/**
	 * extracts the host name from the url
	 * @param url
	 * @return host name
	 */
	private static String getHostName(String url)
	{
		 try {
			return new URL(url).getHost();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return "localhost";
	}
	
	/**
	 * Extracts the port number from the url 
	 * @param url
	 * @return
	 */
	private static int getHostPort(String url)
	{
		 try {
				return new URL(url).getPort();
			} catch (Exception ex) {
				PranaLogManager.error(ex);
			}
			return 8080;
	}
	
	/**
	 * Extracts all pre/post user defined enable rules
	 */
	public static void GetEnabledRules() throws Exception {
		try {
			HashMap<String, HashMap<String, String>> rules = new HashMap<String, HashMap<String, String>>();
			for (RuleType packageName : RuleType.values()) {
				if (!packageName.equals(RuleType.Basket) && !packageName.equals(RuleType.None))
					rules.putAll(ShardineUtility.loadAllRules(packageName.toString()));
			}
			_ruleNameWithCompression.clear();
			String vHost = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_AMQP_VHOST);
			String ruleServer = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS, ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER);
			String ruleServerUserId = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_USERID);

			String ruleServerPassword = ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_RULESERVER_PASSWORD);
			Sardine sardine = SardineFactory.begin(ruleServerUserId, ruleServerPassword);
			if (rules.size() != 0) {
				for (String key : rules.keySet()) {
					if (rules.get(key).get("ruleType").equals(RuleType.PreTrade.toString())
							|| rules.get(key).get("ruleType").equals(RuleType.PostTrade.toString())) {
						if (rules.get(key).get("enabled").equals("true")) {
							String webDavUrl = "http://" + ruleServer + "/org.drools.guvnor.Guvnor/webdav/packages/"
									+ rules.get(key).get("ruleType") + "Compliance_" + vHost + "/"
									+ RuleFormatting.getGuvnorFormattedRule(rules.get(key).get("ruleName")) + ".brl";
							BufferedReader input = new BufferedReader(
									new InputStreamReader(sardine.getInputStream(webDavUrl)));
							String line = null;
							Pattern pattern = Pattern.compile("<factType>(.*?)</factType>", Pattern.DOTALL);
							Matcher matcher;
							while ((line = input.readLine()) != null) {
								matcher = pattern.matcher(line);
								while (matcher.find() && !matcher.group(1).equals("Alert")) {
									_ruleNameWithCompression.put(
											rules.get(key).get("ruleName") + "_" + rules.get(key).get("ruleType"),
											matcher.group(1));
									break;
								}
							}
						}
					}
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}
}
