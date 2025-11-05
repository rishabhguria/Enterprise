package prana.ruleEngineMediator.ruleService;

import prana.utility.logging.PranaLogManager;

public class Utilities {

	public static String formatExpression(String rawString) {

		int start;
		StringBuilder sbFormatter = new StringBuilder();

		try {
			rawString = Character.toUpperCase(rawString.charAt(0))
					+ rawString.substring(1);
			sbFormatter.append(rawString.replaceAll("([A-Z][a-z]+)", " $1") // Words
																			// beginning
																			// with
																			// UC
					.replaceAll("([A-Z][A-Z]+)", "$1") // "Words" of only UC
					.replaceAll("([^A-Za-z ]+)", "$1") // "Words" of non-letters
					.trim());

			// rawString=sbFormatter.toString();

			while (sbFormatter.indexOf("==") != -1) {
				start = sbFormatter.indexOf("==");
				sbFormatter.replace(start, start + 2, "equals to");
				// value=rawString.substring(start+3).toString();
			}
			while (sbFormatter.indexOf("~=") != -1) {
				start = sbFormatter.indexOf("~=");
				sbFormatter.replace(start, start + 2, "matches");
				// value=rawString.substring(start+3).toString();
			}
			while (sbFormatter.indexOf(">=") != -1) {
				start = sbFormatter.indexOf(">=");
				sbFormatter.replace(start, start + 2,
						"greater than or equal to");
				// value=rawString.substring(start+3).toString();
			}
			while (sbFormatter.indexOf(">") != -1) {
				start = sbFormatter.indexOf(">");
				sbFormatter.replace(start, start + 1, "greater than");
				// value=rawString.substring(start+2).toString();
			}
			while (sbFormatter.indexOf("<=") != -1) {
				start = sbFormatter.indexOf("<=");
				sbFormatter.replace(start, start + 2, "less than equal to");
				// value=rawString.substring(start+3).toString();
			}
			while (sbFormatter.indexOf("<") != -1) {
				start = sbFormatter.indexOf("<");
				sbFormatter.replace(start, start + 1, "less than");
				// value=rawString.substring(start+3).toString();
			}
			while (sbFormatter.indexOf("||") != -1) {
				start = sbFormatter.indexOf("||");
				sbFormatter.replace(start, start + 2, "or");
				// value=rawString.substring(start+3).toString();
			}
			while (sbFormatter.indexOf("&&") != -1) {
				start = sbFormatter.indexOf("&&");
				sbFormatter.replace(start, start + 2, "and");
				// value=rawString.substring(start+3).toString();
			}
			while (sbFormatter.indexOf("!=") != -1) {
				start = sbFormatter.indexOf("!=");
				sbFormatter.replace(start, start + 2, "not equals to");
				// value=rawString.substring(start+3).toString();
			}

		} catch (Exception ex) {
			PranaLogManager.error(ex);
		}

		return sbFormatter.toString();
	}
}
