package prana.esperCalculator.esperUDF;

import java.text.DecimalFormat;
import java.text.NumberFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.math.BigDecimal;
import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.communication.DataInitializationRequestProcessor;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.utility.configuration.ConfigurationHelper;
import prana.utility.logging.PranaLogManager;
import prana.businessObjects.uda.DynamicUDA;

public class Misc {
	private static NumberFormat fm1 = new DecimalFormat(",##0.00##");
	private static HashMap<String, String> previousValue = new HashMap<String, String>();
	/**
	 * Locker object to prevent cross thread operations on connection objects
	 */
	private static Object _lockerObject = new Object();
	/**
	 * Locker object for UniqueKey to prevent cross thread operations on connection objects
	 */
	private static Object _lockerObjectUniqueKey = new Object();

	public static double calculatePercentage(double amount, double total) {
		try {

			if (total != 0 && total > 0)
				return (amount / total) * 100;
			else
				return 0.0;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return 0.0;
		}
	}

	public static String format(double number) {
		try {
			// Format numbers
			String formattedNumber = fm1.format(number);
			return formattedNumber;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return "";
		}
	}

	public static String formatBigDecimal(BigDecimal number) {
		try {
			String formattedNumber = fm1.format(number);
			return formattedNumber;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return "";
		}
	}
	
	/**
	 * sets value for IsSymbolDataChanged
	 */
	public static void setSymbolDataChangedValue(boolean value) {
		try {
			CEPManager.setVariableValue(ConfigurationConstants.KEY_IS_SYMBOL_DATA_CHANGED, value);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}
	
	/**
	 * converts any string to integer Datatype only when convertible.
	 * @param str
	 * @return integer
	 */
	public static int convertStrToInt(String str) {
		try {
			if (str != null) {
				int num = Integer.parseInt(str);
				return num;
			}
		} catch (NumberFormatException ex) {
			return 0;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return 0;
	}
	
	/** check if string is convertible to double or not .
	 * @param str
	 * @return boolean
	 */
	
	public static boolean isConvertibleToDouble(String str) {
        try {
            Double.parseDouble(str);
            return true;
        } catch (NumberFormatException e) {
            return false;
        } catch (Exception ex) {
			PranaLogManager.error(ex);
		}
        return false;
	}
	
	/**
	 * converts any string to double Datatype only when convertible.
	 * @param str
	 * @return integer
	 */
	public static double convertStrToDouble(String str) {
		try {
			if (str != null) {
				double num = Double.parseDouble(str);
				return num;
			}
		} catch (NumberFormatException ex) {
			return 0;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return 0;
	}
	
	/**
	 * checks whether given value is defined or not.
	 * @param str
	 * @return boolean
	 */
	public static boolean isValueDefined(String str)
    {
        boolean isDefined = true;
        try {
        if(str==null)
            return false;
        String s = str.toLowerCase();
        if(s.equals("") || s.equals("undefined"))
            isDefined =  false;
        } catch (Exception ex) {
            PranaLogManager.error(ex);    
        }
        return isDefined;
    }
    
    /** Checks if string in null or have some value
     * @param str
     * @return string
     */
    public static String getValue(String str)
    {
        try {
        if(str==null)
            return "Null";
        } catch (Exception ex) {
            PranaLogManager.error(ex);    
        }
        return str;
    }

	/**
	 * check string contains in a string with csv value
	 * 
	 * @param value
	 * @param csv
	 * @return
	 */
	public static boolean valueExistsInCSV(String value, String csv) {
		try {
			String[] elements = csv.split(",");
			for (int i = 0; i < elements.length; i++) {
				if (elements[i].trim().equals(value))
					return true;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return false;
	}

	public static String concatValues(Integer accountId, String rule, String value) {
		try {
			if(accountId != null)
			{
				String key = accountId.toString() + "_" + rule;
				String finalValue = "";
				if (value != null && !value.equals("")) {
					if (!previousValue.containsKey(key))
						finalValue = value;
					else if( previousValue.get(key).contains(value) )
						finalValue = previousValue.get(key);
					else finalValue = previousValue.get(key) + "~" + value;				
					previousValue.put(key, finalValue);
				}
				return finalValue;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return "";
	}

	public static String resetConcatValues(Integer accountId, String rule) {
		try {
			if (accountId != null && rule != null) {
				String key = accountId.toString() + "_" + rule;
				if (previousValue.containsKey(key))
					previousValue.remove(key);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return "";
	}

	public static String getConcatValues(Integer accountId, String rule) {
		try {
			String key = accountId.toString() + "_" + rule;
			if(previousValue.containsKey(key)) {
				List<String> newList = Arrays.asList(previousValue.get(key).split("~"));
				Collections.sort(newList);
				String finalString = String.join(", ", newList);
				return finalString;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return "";
	}

	/**
	 * Multiply double value to Big decimal value. We have created this method
	 * because directly multiplication of BigDecimal number to double was creating
	 * problem
	 * 
	 * @param objectValue1
	 * @param objectValue
	 * @return
	 */
	public static BigDecimal MultiplyDoubleToBigDecimal(BigDecimal bigDecimalVal, double doubleVal) {
		try {
			BigDecimal result = bigDecimalVal.multiply(BigDecimal.valueOf(doubleVal));
			return result;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return BigDecimal.ZERO;
	}
	
	public static String getAsset(int assetId) {
		String asset = "";
		try {
			switch (assetId) {
			case 1:
				return "Equity";
			case 2:
				return "EquityOption";
			case 3:
				return "Future";
			case 4:
				return "FutureOption";
			case 5:
				return "FX";
			case 6:
				return "Cash";
			case 7:
				return "Indices";
			case 8:
				return "FixedIncome";
			case 9:
				return "PrivateEquity";
			case 10:
				return "FXOption";
			case 11:
				return "FXForward";
			case 12:
				return "Forex";
			case 13:
				return "ConvertibleBond";
			case 14:
				return "CreditDefaultSwap";
			case 101:
				return "Option";
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return asset;
	}
	
	/**
	 * Round off the quantity for the Order side rule to the defined precision
	 * 
	 * @param quantity
	 * @return number
	 */
	public static double RoundOffDouble(double quantity) {
		double number = 0.0;

		try {
			int precisonAfterDecimal = Integer.parseInt(ConfigurationHelper.getInstance().getValueBySectionAndKey(
					ConfigurationConstants.SECTION_APP_SETTINGS,
					ConfigurationConstants.KEY_APP_SETTINGS_PRECISION_AFTER_DECIMAL));

			number = Math.round(quantity * Math.pow(10, precisonAfterDecimal)) / Math.pow(10, precisonAfterDecimal);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return number;
	}
	
	/**
	 * Checks if the given symbol is available in the FX Forward symbols list.
	 *
	 * @param symbol
	 *            The symbol to check.
	 * @return true if the symbol is present, false otherwise.
	 */
	public static boolean isFxForwardSymbolAvailable(String symbol) {
		try {
			// Retrieve the list of FX Forward symbols from the data processor.
			List<String> fxForwardSymbols = DataInitializationRequestProcessor.getInstance().getSymbolsFxFwdDetails();
			return fxForwardSymbols.contains(symbol);
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		// Return false if an exception occurs or the symbol is not found.
		return false;
	}
	
	/**
	 * Gets Custom UDA for Underlying Symbol from Dynamic UDA collection.
	 * @param underlyingSymbol
	 * @param dynamicUDAName
	 * @param dynamicUDAValue
	 * @return underlyingUDA
	 */
	public static String getUnderlyingDynamicUda(String underlyingSymbol,String dynamicUDAName, String dynamicUDAValue) {
		String underlyingUDA = dynamicUDAValue;
		try {
			synchronized(_lockerObject) {
				String underlyingDynamicUDA = DynamicUDA.getInstance().getSymbolIndividualDyamicUDAData(underlyingSymbol, dynamicUDAName);
				if(underlyingDynamicUDA!=null)
					underlyingUDA = underlyingDynamicUDA;
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return underlyingUDA;
	}
	
	/**
	 * Get Unique Key for ExtendedAccountSymbolWithNav Window.
	 * @param accountId
	 * @param symbol
	 * @param assetId
	 * @param strategyId
	 * @param orderSide
	 * @param tradeAttribute1
	 * @param tradeAttribute2
	 * @param tradeAttribute3
	 * @param tradeAttribute4
	 * @param tradeAttribute5
	 * @param tradeAttribute6
	 * @return uniqueKey
	 */
	public static String getUniqueKeyExtendedAccountSymbolWithNav(int accountId, String symbol, int assetId, int strategyId, String orderSide,
			String tradeAttribute1, String tradeAttribute2, String tradeAttribute3, String tradeAttribute4, String tradeAttribute5, String tradeAttribute6) {
		String uniqueKey = "";
		try {
			synchronized(_lockerObjectUniqueKey) {
				uniqueKey = accountId + "_" + symbol + "_" + assetId + "_" + strategyId + "_" + orderSide + 
						"_" + getTradeAttributeValue(tradeAttribute1) + "_" + getTradeAttributeValue(tradeAttribute2) + "_" + getTradeAttributeValue(tradeAttribute3) +
						"_" + getTradeAttributeValue(tradeAttribute4) + "_" + getTradeAttributeValue(tradeAttribute5) + "_" + getTradeAttributeValue(tradeAttribute6);
			}
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return uniqueKey;
	}
	
	/**
	 * Get Trade Attribute value. It returns default if Trade Attribute is null or empty.
	 * @param tradeAttribute
	 * @return tradeAttributeValue
	 */
	private static String getTradeAttributeValue(String tradeAttribute) {
		String tradeAttributeValue = "-";
		try {
			if(tradeAttribute != null && !tradeAttribute.isEmpty()) {
				tradeAttributeValue = tradeAttribute;
			}
			
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return tradeAttributeValue;
	}
}