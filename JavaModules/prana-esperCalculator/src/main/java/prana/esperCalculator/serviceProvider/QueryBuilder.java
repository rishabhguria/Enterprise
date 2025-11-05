package prana.esperCalculator.serviceProvider;

import java.util.List;

import prana.utility.logging.PranaLogManager;

public class QueryBuilder {

	/**
	 * Uses the compression and field to create a EPL query
	 * 
	 * @param compression
	 * @param fields
	 * @return
	 */
	public static String getQuery(String compression, List<String> fields) {
		try {
			StringBuilder sb = new StringBuilder();
			sb.append("Select ");
			for (String s : fields) {
				sb.append(s);
				sb.append(",");
			}
			sb.setLength(sb.length() - 1); // Delete the extra comma
			sb.append(" from ");
			sb.append(compression);
			return sb.toString();
		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}
		return "";
	}
}
