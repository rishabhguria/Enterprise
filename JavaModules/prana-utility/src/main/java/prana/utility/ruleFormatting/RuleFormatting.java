package prana.utility.ruleFormatting;

public class RuleFormatting {
	
	public static String getFormattedKey(String url) {
		String id = "ID" + url;
		while (id.contains("-")) {
			if (id.contains("-"))
				id = id.replace("-", "");

			else
				id = "ID" + url;
		}
		return id;
	}

	public static String getUIFormattedRule(String nameN) {

		String rule1 = nameN;
		while (rule1.contains("(20)") || rule1.contains("(25)")) {
			if (rule1.contains("(20)"))
				rule1 = rule1.replace("(20)", " ");
			else if (rule1.contains("(25)"))
				rule1 = rule1.replace("(25)", "%");
			else
				rule1 = nameN;
		}
		return rule1;
		// return null;
	}

	public static String getGuvnorFormattedRule(String nameN) {

		String rule1 = nameN;
		while (rule1.contains(" ") || rule1.contains("%")) {
			if (rule1.contains(" "))
				rule1 = rule1.replace(" ", "(20)");
			else if (rule1.contains("%"))
				rule1 = rule1.replace("%", "(25)");
			else
				rule1 = nameN;
		}
		return rule1;
		// return null;
	}

}
