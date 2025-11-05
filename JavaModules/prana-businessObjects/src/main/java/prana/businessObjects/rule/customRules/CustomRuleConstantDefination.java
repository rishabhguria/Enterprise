package prana.businessObjects.rule.customRules;

public class CustomRuleConstantDefination {

	private String name;
	private String value;
	private String type;
	private String displayName;
	private String comboList;
	
	public CustomRuleConstantDefination()
	{		
	}
	
	public CustomRuleConstantDefination(String name, String value, String type, String displayName, String comboList)
	{
		this.setName(name);
		this.setType(type);
		this.setValue(value);
		this.setDisplayName(displayName);
		this.setComboList(comboList);
	}

	public String getType() {
		return type;
	}

	public void setType(String type) {
		this.type = type;
	}

	public String getValue() {
		return value;
	}

	public void setValue(String value) {
		this.value = value;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getDisplayName() {
		return displayName;
	}

	public void setDisplayName(String displayName) {
		this.displayName = displayName;
	}
	
	public String getComboList() {
		return comboList;
	}

	public void setComboList(String comboList) {
		this.comboList = comboList;
	}
}
