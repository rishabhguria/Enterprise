package prana.businessObjects.complianceLevel;

public class Alert {
	private String summary = "";
	private int severity = 0;
	private String description = "";
	private String orderId = "";
	private boolean isViolated = false;
	private boolean isEOM = false;
	private String compressionLevel = "";

	private int userId;
	private String userName = "";

	private String name = "";
	private String validationTime;
	private String ruleType;
	private String parameters = "";
	private String dimension ="";
	private boolean blocked=true;
	private String constraintFields = "";
	private String threshold = "";
	private String actualResult = "";

	// private String UUID ="";

	public String getOrderId() {
		return orderId;
	}

	public void setOrderId(String orderId) {
		this.orderId = orderId;
	}

	public boolean getViolated() {
		return isViolated;
	}

	public void setViolated(boolean isViolated) {
		this.isViolated = isViolated;
	}

	public String getCompressionLevel() {
		return compressionLevel;
	}

	public void setCompressionLevel(String compressionLevel) {
		this.compressionLevel = compressionLevel;
	}

	public boolean getIsEOM() {
		return isEOM;
	}

	public void setIsEOM(boolean isEOM) {
		this.isEOM = isEOM;
	}

	public String getValidationTime() {
		return validationTime;
	}

	public void setValidationTime(String string) {
		this.validationTime = string;
	}

	/**
	 * @return the ruleType
	 */

	public String getRuleType() {
		return ruleType;
	}

	public void setRuleType(String ruleType) {
		this.ruleType = ruleType;
	}

	public String getSummary() {
		return summary;
	}

	public void setSummary(String summary) {
		this.summary = summary;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getParameters() {
		return parameters;
	}

	public void setParameters(String parameters) {
		this.parameters = parameters;
	}

	public int getUserId() {
		return userId;
	}

	public void setUserId(int userId) {
		this.userId = userId;
	}

	public String getUserName() {
		return userName;
	}

	public void setUserName(String userName) {
		this.userName = userName;
	}

	@Override
	public Alert clone() {
		Alert a = new Alert();
		a.isViolated = this.isViolated;
		a.isEOM = this.isEOM;
		a.compressionLevel = this.compressionLevel;
		a.summary = this.summary;
		a.orderId = this.orderId;
		return a;
	}
	
	public String getDescription() {
		return description;
	}

	public void setDescription(String description) {
		this.description = description;
	}

	public int getSeverity() {
		return severity;
	}

	public void setSeverity(int severity) {
		this.severity = severity;
	}

	/**
	 * @return the dimension
	 */
	public String getDimension() {
		return dimension;
	}

	/**
	 * @param dimension the dimension to set
	 */
	public void setDimension(String dimension) {
		this.dimension = dimension;
	}

	public boolean isBlocked() {
		return blocked;
	}

	public void setBlocked(boolean blocked) {
		this.blocked = blocked;
	}

	public String getConstraintFields() {
		return constraintFields;
	}

	public void setConstraintFields(String constraintFields) {
		this.constraintFields = constraintFields;
	}
	
	public String getThreshold() {
		return threshold;
	}

	public void setThreshold(String threshold) {
		this.threshold = threshold;
	}
	
	public String getActualResult() {
		return actualResult;
	}

	public void setActualResult(String actualResult) {
		this.actualResult = actualResult;
	}
}