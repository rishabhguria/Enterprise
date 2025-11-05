package prana.utility.configuration;

import java.security.KeyException;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;

import org.apache.commons.configuration.ConfigurationException;
import org.apache.commons.configuration.XMLConfiguration;

import prana.utility.logging.PranaLogManager;

/**
 * Definition of a configuration node
 * 
 * @author dewashish
 * 
 */
class ConfigurationNode {

	private Object _lockerObject = new Object();
	private HashMap<String, String> configuration;
	private String filePath;
	private String node;

	/**
	 * Constructor for configuration node
	 * 
	 * @param filePath
	 * @param node
	 */
	ConfigurationNode(String filePath, String node) {

		try {
			this.filePath = filePath;
			this.node = node;
			reloadConfig();
		} catch (ConfigurationException ex) {
			PranaLogManager.error(ex);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * Load cofiguration from given file
	 * 
	 * @return
	 * @throws ConfigurationException
	 */
	private HashMap<String, String> loadKeyValueFromConfig()
			throws ConfigurationException {
		XMLConfiguration configuration = new XMLConfiguration(filePath);
		// configuration.validate();
		int noOfQueues = 1;
		HashMap<String, String> tempMap = new HashMap<>();
		Object prop = configuration.getProperty(node + "[@key]");
		if (prop instanceof Collection) {
			noOfQueues = ((Collection<?>) prop).size();
		}

		for (int i = 0; i < noOfQueues; i++) {
			String keyN = (String) configuration.getProperty(node + "(" + i
					+ ")[@key]");
			
			Object valueObj = configuration.getProperty(node + "(" + i + ")[@value]");
			
			String valueN;
	        if (valueObj instanceof Collection) {
	        	valueN = String.join(",", ((Collection<?>) valueObj).stream()
	                    .map(Object::toString)
	                    .toArray(String[]::new));
	        } else {
	            valueN = valueObj != null ? valueObj.toString() : "";
	        }

	        tempMap.put(keyN, valueN);
		}
		return tempMap;
	}

	/**
	 * Returns the value for given key
	 * 
	 * @param key
	 * @return
	 * @throws Exception
	 */
	String getValueForKey(String key) throws Exception {
		try {
			synchronized (_lockerObject) {
				if (configuration.containsKey(key))
					return configuration.get(key);
				else
					throw new KeyException("key Not found" + key);
			}
		} catch (KeyException ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	/**
	 * Reload configuration from given path
	 * 
	 * @throws Exception
	 */
	void reloadConfig() throws Exception {

		try {
			synchronized (_lockerObject) {
				this.configuration = loadKeyValueFromConfig();
			}
		} catch (ConfigurationException ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

	/**
	 * Returns the keys available for this configuration node
	 * 
	 * @return
	 */
	ArrayList<String> getCacheKeySet() throws Exception {
		try {
			ArrayList<String> listOfKeys = new ArrayList<>();
			synchronized (_lockerObject) {
				for (String key : this.configuration.keySet()) {
					listOfKeys.add(key);
				}
			}
			return listOfKeys;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}

	}

	/**
	 * Returns the complete node
	 * 
	 * @return
	 */
	HashMap<String, String> getMAP() throws Exception {
		try {
			HashMap<String, String> listOfKeys = new HashMap<>();
			synchronized (_lockerObject) {
				for (String key : this.configuration.keySet()) {
					listOfKeys.put(key, this.configuration.get(key));
				}
			}
			return listOfKeys;
		} catch (Exception ex) {
			PranaLogManager.error(ex.getMessage(), ex);
			throw ex;
		}
	}

}
