package prana.esperCalculator.esperCEP;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.HashMap;

import prana.esperCalculator.commonCode.CEPManager;
import prana.esperCalculator.constants.ConfigurationConstants;
import prana.utility.logging.PranaLogManager;

public class HolidayHelper {
	/**
	 * Used to parse Date fields from map to java.util.Date
	 */
	private static SimpleDateFormat parserSDF = new SimpleDateFormat(ConfigurationConstants.SIMPLE_DATE_FORMAT);

	/**
	 * This method sends yearly holidays to esper engine
	 * 
	 * @param map
	 *            Key: AuecId, Value: it is arrayList of dates
	 */
	public static void sendYearlyHolidaysToEsper(HashMap<String, Object> map) {

		try {
			clearHolidays();
			for (String key : map.keySet()) {
				@SuppressWarnings("unchecked")
				ArrayList<String> holiday = (ArrayList<String>) (map.get(key));

				for (String date : holiday) {
					CEPManager.getEPRuntime().getEventService().sendEventObjectArray(
							new Object[] { Integer.parseInt(key), parserSDF.parse(date) }, "YearlyHolidaysEvent");
				}
			}
			triggerCalculations();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	/**
	 * This method sends weekly holidays to esper engine
	 * 
	 * @param map
	 *            Key: AuecId, Value: it is arrayList of days (0-6)
	 */
	public static void sendWeeklyHolidaysToEsper(HashMap<String, Object> map) {

		try {
			clearHolidays();
			for (String key : map.keySet()) {
				@SuppressWarnings("unchecked")
				ArrayList<Integer> holiday = (ArrayList<Integer>) (map.get(key));
				for (int day : holiday) {
					CEPManager.getEPRuntime().getEventService()
							.sendEventObjectArray(new Object[] { Integer.parseInt(key), day }, "WeeklyHolidaysEvent");
				}
			}
			triggerCalculations();

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
	}

	private static void triggerCalculations() {
		// TODO:Needs to implement
	}

	private static void clearHolidays() {
		// TODO:Needs to implement
	}
}
