package prana.basketComplianceService.models;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonProperty;

@JsonIgnoreProperties(ignoreUnknown = true)
public class AccountNavRequestDto {
	@JsonProperty("AccountId") // Maps JSON key "AccountId" to this field
	private int accountId;

	@JsonProperty("UnderlyingSymbol")
	private String underlyingSymbol;

	@JsonProperty("Symbol")
	private String symbol;

	@JsonProperty("SymbolAUECID")
	private String symbolAUECID;

	@JsonProperty("Price")
	private double price;

	@JsonProperty("IsLimitPrice")
	private boolean isLimitPrice;

	@JsonProperty("AccountNav")
	private double accountNav;
	
	@JsonProperty("StartValue")
	private double startValue; 
	
	@JsonProperty("StartingPosition")
	private double startingPosition; 
	
	@JsonProperty("CorrelationId")
	private String correlationId;
	
	@JsonProperty("UserId")
	private int userId;
	
	@JsonProperty("FxRate")
	private double  fxRate;
	
	@JsonProperty("FxOperator")
	private String fxOperator;

	@JsonProperty("SelectedFeedPriceBase")
	private double selectedFeedPriceBase;

	@JsonProperty("IsLoadMarketSnapshot")
	private boolean isLoadMarketSnapshot;

	public boolean isLoadMarketSnapshot() {
		return isLoadMarketSnapshot;
	}

	public void setLoadMarketSnapshot(boolean isLoadMarketSnapshot) {
		this.isLoadMarketSnapshot = isLoadMarketSnapshot;
	}

	public void setSelectedFeedPriceBase(double value){
		this.selectedFeedPriceBase = value;
	}
	
	public double getSelectedFeedPriceBase(){
		return this.selectedFeedPriceBase;
	}

	// Getters and Setters
	public int getAccountId() {
		return accountId;
	}

	public void setAccountId(int accountId) {
		this.accountId = accountId;
	}

	public String getUnderlyingSymbol() {
		return underlyingSymbol;
	}

	public void setUnderlyingSymbol(String underlyingSymbol) {
		this.underlyingSymbol = underlyingSymbol;
	}

	public String getSymbol() {
		return symbol;
	}

	public void setSymbol(String symbol) {
		this.symbol = symbol;
	}

	public String getSymbolAUECID() {
		return symbolAUECID;
	}

	public void setSymbolAUECID(String symbolAUECID) {
		this.symbolAUECID = symbolAUECID;
	}

	public double getPrice() {
		return price;
	}

	public void setPrice(double price) {
		this.price = price;
	}

	public boolean isIsLimitPrice() {
		return isLimitPrice;
	}

	public void setIsLimitPrice(boolean isLimitPrice) {
		this.isLimitPrice = isLimitPrice;
	}

	public double getAccountNav() {
		return accountNav;
	}

	public void setAccountNav(double accountNav) {
		this.accountNav = accountNav;
	}

	@Override
	public String toString() {
		return "AccountId:" + accountId + ", Symbol:" + symbol + ",NAV:" + accountNav;
	}

	public double getStartValue() {
		return startValue;
	}

	public void setStartValue(double startValue) {
		this.startValue = startValue;
	}

	public double getStartingPosition() {
		return startingPosition;
	}

	public void setStartingPosition(double startingPosition) {
		this.startingPosition = startingPosition;
	}

	public String getCorrelationId() {
		return correlationId;
	}

	public void setCorrelationId(String correlationId) {
		this.correlationId = correlationId;
	}

	public int getUserId() {
		return userId;
	}

	public void setUserId(int userId) {
		this.userId = userId;
	}

	
	public String getFxOperator() {
		return fxOperator;
	}

	public void setFxOperator(String fxOperator) {
		this.fxOperator = fxOperator;
	}
	
	public double getFxRate() {
		return fxRate;
	}

	public void setFxRate(double fxRate) {
		this.fxRate = fxRate;
	}   
}
