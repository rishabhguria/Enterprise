package prana.businessObjects.rule.customRules;

import java.util.ArrayList;

public class RuleDefinition {

	private String ruleId;
	private String ruleName;
	private boolean isEnabled;
	private boolean isDeleted;
	private String ruleType;
	private String eplPath;
	private String description;
	private ArrayList<String> outputStatementList;
	private ArrayList<String> windowFillerStatementList;
	private String validationCompletedEventName;
	private String compressionLevel;
	private String clientName;
	private boolean blocked = true; 
	private String constants; // stores the constants as a json string

	/**
	 * @return the clientName
	 */
	public String getClientName() {
		return clientName;
	}

	/**
	 * @param clientName
	 *            the clientName to set
	 */
	public void setClientName(String clientName) {
		this.clientName = clientName;
	}

	/**
	 * @return the name
	 */
	public String getRuleName() {
		return ruleName;
	}

	/**
	 * @param name
	 *            the name to set
	 */
	public void setRuleName(String name) {
		this.ruleName = name;
	}

	/**
	 * @return the ruleType
	 */
	public String getRuleType() {
		return ruleType;
	}

	/**
	 * @param ruleType
	 *            the ruleType to set
	 */
	public void setRuleType(String ruleType) {
		this.ruleType = ruleType;
	}

	/**
	 * @return the eplPath
	 */
	public String getEplPath() {
		return eplPath;
	}

	/**
	 * @param eplPath
	 *            the eplPath to set
	 */
	public void setEplPath(String eplPath) {
		this.eplPath = eplPath;
	}

	/**
	 * @return the description
	 */
	public String getDescription() {
		return description;
	}

	/**
	 * @param description
	 *            the description to set
	 */
	public void setDescription(String description) {
		this.description = description;
	}

	/**
	 * @return the outputStatementList
	 */
	public ArrayList<String> getOutputStatementList() {
		return outputStatementList;
	}

	/**
	 * @param outputStatementList
	 *            the outputStatementList to set
	 */
	public void setOutputStatementList(ArrayList<String> outputStatementList) {
		this.outputStatementList = outputStatementList;
	}

	/**
	 * @return the windowFillerStatementList
	 */
	public ArrayList<String> getWindowFillerStatementList() {
		return windowFillerStatementList;
	}

	/**
	 * @param windowFillerStatementList
	 *            the windowFillerStatementList to set
	 */
	public void setWindowFillerStatementList(
			ArrayList<String> windowFillerStatementList) {
		this.windowFillerStatementList = windowFillerStatementList;
	}

	/**
	 * @return the validationCompletedEventName
	 */
	public String getValidationCompletedEventName() {
		return validationCompletedEventName;
	}

	/**
	 * @param validationCompletedEventName
	 *            the validationCompletedEventName to set
	 */
	public void setValidationCompletedEventName(
			String validationCompletedEventName) {
		this.validationCompletedEventName = validationCompletedEventName;
	}

	/**
	 * @return the compressionLevel
	 */
	public String getCompressionLevel() {
		return compressionLevel;
	}

	/**
	 * @param compressionLevel
	 *            the compressionLevel to set
	 */
	public void setCompressionLevel(String compressionLevel) {
		this.compressionLevel = compressionLevel;
	}

	/**
	 * @return the enabled
	 */
	public boolean getEnabled() {
		return isEnabled;
	}

	/**
	 * @param enabled
	 *            the enabled to set
	 */
	public void setEnabled(boolean enabled) {
		this.isEnabled = enabled;
	}

	/**
	 * @return the isDeleted
	 */
	public boolean getIsDeleted() {
		return isDeleted;
	}

	/**
	 * @param isDeleted
	 *            the isDeleted to set
	 */
	public void setIsDeleted(boolean isDeleted) {
		this.isDeleted = isDeleted;
	}

	/**
	 * @return the ruleId
	 */
	public String getRuleId() {
		return ruleId;
	}

	/**
	 * @param ruleId
	 *            the ruleId to set
	 */
	public void setRuleId(String ruleId) {
		this.ruleId = ruleId;
	}

	public boolean getBlocked() {
		return blocked;
	}

	public void setBlocked(boolean blocked) {
		this.blocked = blocked;
	}

	public String getConstants() {
		return constants;
	}

	public void setConstants(String constants) {
		this.constants = constants;
	}

}
