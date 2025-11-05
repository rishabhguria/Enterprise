package prana.esperCalculator.esperUDF;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;

import org.joda.time.DateTime;

import prana.esperCalculator.commonCode.CEPManager;
import prana.utility.logging.PranaLogManager;
import com.espertech.esper.common.client.fireandforget.EPFireAndForgetQueryResult;

public class DateTimeHelper {

	private static String _yearlyHolidayCountPullQuery = "Select count(*) as count " + "from YearlyHolidaysWindow as H "
			+ "where H.auecId=@AUECID and " + "H.holiday.between(@INITIALDATE,@FINALDATE,false,true) ";

	private static String _weeklyHolidayCountPullQuery = "Select distinct holiday as dayNumber "
			+ "from WeeklyHolidaysWindow as H " + "where H.auecId=@AUECID";

	public static HashMap<Integer,ArrayList<Integer>>auecIDwithquery = new HashMap<Integer,ArrayList<Integer>>();
	
	public static int getNoOfDays(Date startDate, Date endDate) {
		try {
			// If expiration date is before current date then days=0;
			if (startDate != null && endDate != null && endDate.compareTo(startDate) > 0)
				return org.joda.time.Days.daysBetween(new DateTime(startDate), new DateTime(endDate)).getDays() + 1;
			else
				return 0;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return 0;
		}
	}

	/*
	 * Returns No of days If start date is less than the end date and start date and
	 * end date are equal Returns zero otherwise
	 */

	public static int getNoOfDaysNew(Date startDate, Date endDate) {
		try {
			// If expiration date is before current date then days=0;
			SimpleDateFormat dateFormat = new SimpleDateFormat("MM/dd/yyyy");
			Date startDateOnly = dateFormat.parse(dateFormat.format(startDate));
			Date endDateOnly = dateFormat.parse(dateFormat.format(endDate));

			if (startDateOnly != null && endDateOnly != null && endDateOnly.compareTo(startDateOnly) > 0
					|| endDateOnly.compareTo(startDateOnly) == 0)
				return org.joda.time.Days.daysBetween(new DateTime(startDateOnly), new DateTime(endDateOnly)).getDays()
						+ 1;
			else
				return 0;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return 0;
		}
	}

	/**
	 * Return Date only from DateTime
	 * 
	 * @param date
	 * @return
	 */
	public static String getOnlyDate(Date date) {
		try {
			SimpleDateFormat dateFormat = new SimpleDateFormat("dd MMM yyyy");
			Date dateOnly = dateFormat.parse(dateFormat.format(date));
			String formatedDate = dateFormat.format(dateOnly);
			return formatedDate;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return "";
		}
	}

	public static int getNoOfBusinessDays(int auecId, Date startDate, Date endDate) {
		try {
			if (endDate == null || startDate == null)
				return 0;
			int actualDays = getNoOfDays(startDate, endDate);
			int businessDays;
			if (actualDays > 0) {
				businessDays = actualDays - getNoOfYearlyHolidaysBetweenForAuec(auecId, startDate, endDate)
						- getNoOfWeeklyHolidaysBetweenForAuec(auecId, startDate, endDate);
			} else
				return 0;

			if (businessDays > 0)
				return businessDays;
			else
				return 0;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return 0;
		}
	}

	public static int getNoOfWeeklyHolidaysBetweenForAuec(int auecId, Date startDate, Date endDate) {
		try {
			String query = _weeklyHolidayCountPullQuery.replace("@AUECID", String.valueOf(auecId));
			EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);
			ArrayList<Integer> dayNumberList = new ArrayList<>();
			if (result.getArray().length > 0) {
				for (int arrayIndex = 0; arrayIndex < result.getArray().length; arrayIndex++) {
					dayNumberList.add(Integer.parseInt(result.getArray()[arrayIndex].get("dayNumber").toString()));
				}
			}

			int count = 0;

			if (!dayNumberList.isEmpty()) {
				DateTime dtStart = new DateTime(startDate);
				DateTime dtEnd = new DateTime(endDate);

				while (dtEnd.isAfter(dtStart)) {

					dtStart = dtStart.plusDays(1);
					if (dayNumberList.contains(dtStart.getDayOfWeek()))
						count++;
				}
			}
			return count;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return 0;
		}
	}

	public static int getNoOfYearlyHolidaysBetweenForAuec(int auecId, Date initialDate, Date finalDate) {
		try {
			String query = _yearlyHolidayCountPullQuery.replace("@AUECID", String.valueOf(auecId))
					.replace("@FINALDATE", String.valueOf(new DateTime(finalDate).getMillis()))
					.replace("@INITIALDATE", String.valueOf(new DateTime(initialDate).getMillis()));

			EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);
			int count = 0;
			if (result.getArray().length > 0) {
				count = Integer.parseInt(result.getArray()[0].get("count").toString());
			}
			return count;
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return 0;
		}
	}

	public static boolean dateExistsInWeeklyHolidayForAuecId(int auecId, Date yearlyHoliday) {
		try {
			ArrayList<Integer> dayNumberList = new ArrayList<>();
			if(!auecIDwithquery.containsKey(auecId))
			{
			String query = _weeklyHolidayCountPullQuery.replace("@AUECID", String.valueOf(auecId));
			EPFireAndForgetQueryResult result = CEPManager.executeQuery(query);
			
			if (result.getArray().length > 0) {
				for (int arrayIndex = 0; arrayIndex < result.getArray().length; arrayIndex++) {
					dayNumberList.add(Integer.parseInt(result.getArray()[arrayIndex].get("dayNumber").toString()));
				}
			}
			auecIDwithquery.put(auecId, dayNumberList);
			}
			else 
				 dayNumberList =	auecIDwithquery.get(auecId);
			if(dayNumberList== null || dayNumberList.size()==0)
				return false;
			DateTime dtYearlyHoliday = new DateTime(yearlyHoliday);
			int dateno = dtYearlyHoliday.getDayOfWeek();
			if(dayNumberList!=null &&  dayNumberList.contains(dateno))
				return true;
			else return false;
			
		} catch (Exception ex) {
			PranaLogManager.error(ex);
			return false;
		}
	}
}