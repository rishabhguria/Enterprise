package prana.utility.configuration;

import java.util.HashMap;

import prana.utility.logging.PranaLogManager;

/**
 * Handles operations on config file
 * 
 * @author dewashish
 * 
 */
public class ConfigurationHelper {

	public static Boolean EMAIL_ERROR_NOTIFICATION_PERMISSION = false;
	public static int EMAIL_ERROR_NOTIFICATION_TIMEOUT_SECONDS = 300;
	/**
	 * Singleton pattern
	 */
	private static ConfigurationHelper _configurationHelper;

	/**
	 * Singleton locker object
	 */
	private static Object _singletonLockerObject = new Object();

	/**
	 * Getinstance for singleton instance
	 * 
	 * @return Singleton instance in memory
	 */
	public static ConfigurationHelper getInstance() throws Exception {
		try {
			synchronized (_singletonLockerObject) {
				if (_configurationHelper == null)
					_configurationHelper = new ConfigurationHelper();
				return _configurationHelper;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * Private constructor for singleton pattern
	 */
	private ConfigurationHelper() {
	}

	/**
	 * Configuration cache locker object
	 */
	private Object _cacheLocker = new Object();

	/**
	 * List of Configuration sections of config file
	 */
	private ConfigurationNode _configurationSettingsNode;

	/**
	 * Configuration cache
	 */
	private HashMap<String, ConfigurationNode> _configurationCache = new HashMap<>();

	/**
	 * Initializes configuration from given file
	 * 
	 * @param configurationpath
	 *            path of configuration file
	 */
	public void initializeConfiguration(String configurationpath) {

		try {
			reloadConfiguration(configurationpath);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}

	/**
	 * Reload configuration if required
	 * 
	 * @param configurationpath
	 */
	public void reloadConfiguration(String configurationpath) {
		try {
			loadSections(configurationpath);
			loadConfigurations(configurationpath);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Load all mentioned sections from configuration file
	 * 
	 * @param configurationpath
	 *            path of configuration file
	 */
	private void loadSections(String configurationpath) {
		try {
			synchronized (_cacheLocker) {
				_configurationSettingsNode = new ConfigurationNode(
						configurationpath, "configSections.section");
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Load Configuaration as specified in configuration settings node
	 * 
	 * @param configurationpath
	 */
	private void loadConfigurations(String configurationpath) {

		try {
			_configurationCache.clear();
			synchronized (_cacheLocker) {
				for (String sectionKey : _configurationSettingsNode
						.getCacheKeySet()) {

					ConfigurationNode tempnode = new ConfigurationNode(
							configurationpath,
							_configurationSettingsNode
									.getValueForKey(sectionKey));

					_configurationCache.put(sectionKey, tempnode);

				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

	}

	/**
	 * Load Custom rules configuaration as specified in esper configuration settings node
	 * 
	 * @param configurationpath
	 */
	public void loadCustomRuleConfigFromEsper(String configurationpath) {
		try {
			synchronized (_cacheLocker) {
				_configurationSettingsNode = new ConfigurationNode(
						configurationpath, "configSections.section");
				for (String sectionKey : _configurationSettingsNode
						.getCacheKeySet()) {					
					if(sectionKey.equals("CustomRuleConfigPaths")){
					ConfigurationNode tempnode = new ConfigurationNode(
							configurationpath,
							_configurationSettingsNode
									.getValueForKey(sectionKey));

					_configurationCache.put(sectionKey, tempnode);
					}
				}
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/**
	 * Returns value by using section and key
	 * 
	 * @param section
	 * @param key
	 * @return
	 * @throws Exception
	 *             Throws exception if section is not
	 */
	public String getValueBySectionAndKey(String section, String key)
			throws Exception {
		try {
			if (_configurationCache.containsKey(section)) {
				return _configurationCache.get(section).getValueForKey(key);
			} else
				throw new Exception("Section not found");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * Returns the section as hashmap of strings
	 * 
	 * @param section
	 * @return
	 * @throws Exception
	 */
	public HashMap<String, String> getSection(String section) throws Exception {

		try {
			if (_configurationCache.containsKey(section)) {
				return _configurationCache.get(section).getMAP();
			} else
				throw new Exception("Section not found");
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

}
