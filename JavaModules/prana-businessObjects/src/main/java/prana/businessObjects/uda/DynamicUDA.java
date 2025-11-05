package prana.businessObjects.uda;

import java.util.HashMap;
import prana.utility.logging.PranaLogManager;

/**
 * Singleton class that manages DynamicUDA objects and caches them.
 */
public class DynamicUDA {
	
    /** Singleton instance of DynamicUDA */
    private static DynamicUDA INSTANCE = new DynamicUDA();
    /**
     * Private constructor to prevent instantiation.
     */
    private DynamicUDA() { }

    /**
     * Returns the singleton instance of DynamicUDA.
     *
     * @return The singleton instance.
     */
    public static DynamicUDA getInstance() {
        if (INSTANCE == null) {
            INSTANCE = new DynamicUDA();
        }
        return INSTANCE;
    }
    
    /**
     * Locker object to prevent cross thread operations on connection objects
     */
    private Object _lockerObject = new Object();
    
    /**
     * This collection maintains symbol wise Dynamic UDA Data.
     */
    private HashMap<String,HashMap<String,Object>> _symbolDynamicUDAData = new HashMap<String,HashMap<String,Object>>();
    
    /**
     * This method sets symbol wise dynamic UDA data in the collection.
     * @param symbol
     * @param dynamicUDAData
     */
    public void setSymbolDynamicUDAData(String symbol, HashMap<String,Object> dynamicUDAData)
    {
    	try {
    		synchronized (_lockerObject) {
    			if(_symbolDynamicUDAData!=null) {
    				HashMap<String,Object> modifiedDynamicUDAData = getModifiedDynamicUDAData(dynamicUDAData);
    				_symbolDynamicUDAData.put(symbol,modifiedDynamicUDAData);
    			}
    		}
    	} catch (Exception ex) {
    		PranaLogManager.error(ex);
    	}
    }
    
    /**
     * This method modifies the symbol of dynamic UDA into the required format.
     * @param dynamicUDAData
     * @return modifiedDynamicUDAData
     */
    private HashMap<String,Object> getModifiedDynamicUDAData(HashMap<String,Object> dynamicUDAData) {
    	HashMap<String,Object> modifiedDynamicUDAData = new HashMap<String,Object>();
    	try {
    		for(String symbol: dynamicUDAData.keySet()) {
    			String modifiedSymbol = Character.toLowerCase(symbol.charAt(0)) + symbol.substring(1);
    			modifiedDynamicUDAData.put(modifiedSymbol, dynamicUDAData.get(symbol));
    		}
    	} catch (Exception ex) {
    		PranaLogManager.error(ex);
    	}
    	return modifiedDynamicUDAData;
    }
    
    /**
     * This method gets the value of the dynamic UDA of the given symbol and dynamic UDA key.
     * @param symbol
     * @param dynamicUDAName
     * @return dynamicUDAValue
     */
    public String getSymbolIndividualDyamicUDAData(String symbol, String dynamicUDAName)
    {
    	String dynamicUDAValue = null;
    	try {
    		synchronized (_lockerObject) {
    			if(_symbolDynamicUDAData.containsKey(symbol) && _symbolDynamicUDAData.get(symbol).containsKey(dynamicUDAName)) {
    				Object dynamicUDA = _symbolDynamicUDAData.get(symbol).get(dynamicUDAName);
    				if(dynamicUDA!=null)
    					dynamicUDAValue = dynamicUDA.toString();
    			}
    		}
    	} catch (Exception ex) {
    		PranaLogManager.error(ex);
    	}
    	return dynamicUDAValue;
    }
}