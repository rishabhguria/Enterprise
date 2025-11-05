package prana.businessObjects.complianceLevel;

public class MasterFund_Symbol

{
	private int userId;

	private double quantity;
	private double netMarketValueLocal;

	private String symbol;
	private String underlyingSymbol;
	private String masterFundName;
	private String exchange;
	private String currency;
	private double grossMarketValueLocal;
	private String sector;
	private String subSector;
	private String udaAsset;
	/* private String udaSymbol; */
	private String udaSecurityType;
	private String udaCountry;
	private String asset;
	private int assetId;

	private String riskCurrency;
	private String issuer;
	private String countryOfRisk;
	private String region;
	private String analyst;
	private String ucitsEligibleTag;
	private String liquidTag;
	private String marketCap;
	private String customUDA1;
	private String customUDA2;
	private String customUDA3;
	private String customUDA4;
	private String customUDA5;
	private String customUDA6;
	private String customUDA7;
	private String customUDA8;
	private String customUDA9;
	private String customUDA10;
	private String customUDA11;
	private String customUDA12;

	private double notionalLocal;
	private double equityHaircutLocal;
	private double indexHaircutLocal;
	private double cashImpactLocal;
	private double cashInflowLocal;
	private double cashOutflowLocal;
	private double costBasisPnLLocal;
	private double dayPnLLocal;
	private double netExposureLocal;
	private double grossExposureLocal;
	private double betaAdjustedExposureLocal;
	private double deltaAdjPosition;
	private double liquidationPnlLocal;
	private double underlyingValueForOptionsLocal;

	private double notionalBase;
	private double cashImpactBase;
	private double cashInflowBase;
	private double cashOutflowBase;
	private double netMarketValueBase;
	private double grossMarketValueBase;
	private double costBasisPnLBase;
	private double dayPnLBase;
	private double netExposureBase;
	private double grossExposureBase;
	private double betaAdjustedExposureBase;
	private double underlyingValueForOptionsBase;

	private double equityHircutBase;
	private double indexHaircutBase;
	private double liquidationPnlBase;

	private String defaultPositionSide;
	private String exposurePositionSide;
	private double longNotionalBase;
	private double shortNotionalBase;
	private double shortMarketValueBase;
	private double longMarketValueBase;
	private double shortExposureBase;
	private double longExposureBase;

	
	private double masterFundNav;
	private double grossMarketValueBaseMasterFund;
	private double grossExposureBaseMasterFund;
	private double netExposureBaseMasterFund;
	private double startOfDayNavMasterFund;
	
	private double globalNav;
	private double grossMarketValueBaseGlobal;
	private double grossExposureBaseGlobal;
	private double netExposureBaseGlobal;	
	private double startOfDayNavGlobal;

	private double percentLongExposureMasterFund;
	private double percentShortExposureMasterFund;
	private double percentNetExposureMasterFund;
	private double percentBetaAdjustedExposureMasterFund;
	private double percentGrossExposureMasterFund;
	private double percentLongMarketValueMasterFund;
	private double percentShortMarketValueMasterFund;
	private double percentNetMarketValueMasterFund;
	private double percentGrossMarketValueMasterFund;
	private double percentLongNotionalMasterFund;
	private double percentShortNotionalMasterFund;
	private double percentNetNotionalMasterFund;

	private double pnlContributionMasterFund;

	
	private double percentLongExposureGlobal;
	private double percentShortExposureGlobal;
	private double percentNetExposureGlobal;
	private double percentBetaAdjustedExposureGlobal;
	private double percentGrossExposureGlobal;
	private double percentLongMarketValueGlobal;
	private double percentShortMarketValueGlobal;
	private double percentGrossMarketValueGlobal;
	private double percentLongNotionalGlobal;
	private double percentShortNotionalGlobal;
	private double percentNetNotionalGlobal;
	private double pnlContributionGlobal;
	private double percentNetMarketValueGlobal;

	private double openInterest;
	private double avgVolume20Days;
	private double percentAvgVolume;
	private java.math.BigDecimal sharesOutstanding;
	private double percentCostBasisPnlGlobal;
	private double percentCostBasisPnlMasterFund;
	private String putOrCall;
	private double strikePrice;

	private double askPrice;
	private double bidPrice;
	private double lowPrice;
	private double highPrice;
	private double openPrice;
	private double lastPrice;
	private double markPrice;
	private double delta;
	private double beta5YearMonthly;
	private double selectedFeedPrice;
	private java.math.BigDecimal marketCapitalization;
	private double avgVolume;
	private double dayReturnGlobal;
	private double dayReturnMasterFund;
	
	private double todayAbsTradeQuantity;
	private double todayLongTradeQuantity;
	private double todayShortTradeQuantity;
	private double todayNetNotionalBase;
	private double todayLongNotionalBase;
	private double todayShortNotionalBase;	

	/**
	 * @return the riskCurrency
	 */
	public String getRiskCurrency() {
		return riskCurrency;
	}
	
	/**
	 * @param riskCurrency
	 *            the riskCurrency to set
	 */
	public void setRiskCurrency(String riskCurrency) {
		this.riskCurrency = riskCurrency;
	}
	
	/**
	 * @return the issuer
	 */
	public String getIssuer() {
		return issuer;
	}
	
	/**
	 * @param issuer
	 *            the issuer to set
	 */
	public void setIssuer(String issuer) {
		this.issuer = issuer;
	}

	/**
	 * @return the countryOfRisk
	 */
	public String getCountryOfRisk() {
		return countryOfRisk;
	}
	
	/**
	 * @param countryOfRisk
	 *            the countryOfRisk to set
	 */
	public void setCountryOfRisk(String countryOfRisk) {
		this.countryOfRisk = countryOfRisk;
	}

	/**
	 * @return the region
	 */
	public String getRegion() {
		return region;
	}
	
	/**
	 * @param region
	 *            the region to set
	 */
	public void setRegion(String region) {
		this.region = region;
	}

	/**
	 * @return the analyst
	 */
	public String getAnalyst() {
		return analyst;
	}
	
	/**
	 * @param analyst
	 *            the analyst to set
	 */
	public void setAnalyst(String analyst) {
		this.analyst = analyst;
	}

	/**
	 * @return the ucitsEligibleTag
	 */
	public String getUcitsEligibleTag() {
		return ucitsEligibleTag;
	}
	
	/**
	 * @param ucitsEligibleTag
	 *            the ucitsEligibleTag to set
	 */
	public void setUcitsEligibleTag(String ucitsEligibleTag) {
		this.ucitsEligibleTag = ucitsEligibleTag;
	}
	
	/**
	 * @return the LiquidTag
	 */
	public String getLiquidTag() {
		return liquidTag;
	}
	
	/**
	 * @param liquidTag
	 *            the liquidTag to set
	 */
	public void setLiquidTag(String liquidTag) {
		this.liquidTag = liquidTag;
	}
	
	/**
	 * @return the MarketCap
	 */
	public String getMarketCap() {
		return marketCap;
	}
	
	/**
	 * @param marketCap
	 *            the marketCap to set
	 */
	public void setMarketCap(String marketCap) {
		this.marketCap = marketCap;
	}
	
	/**
	 * @return the CustomUDA1
	 */
	public String getCustomUDA1() {
		return customUDA1;
	}
	
	/**
	 * @param customUDA1
	 *            the customUDA1 to set
	 */
	public void setCustomUDA1(String customUDA1) {
		this.customUDA1 = customUDA1;
	}
	
	/**
	 * @return the CustomUDA2
	 */
	public String getCustomUDA2() {
		return customUDA2;
	}
	
	/**
	 * @param customUDA2
	 *            the customUDA2 to set
	 */
	public void setCustomUDA2(String customUDA2) {
		this.customUDA2 = customUDA2;
	}
	
	/**
	 * @return the CustomUDA3
	 */
	public String getCustomUDA3() {
		return customUDA3;
	}
	
	/**
	 * @param customUDA3
	 *            the customUDA3 to set
	 */
	public void setCustomUDA3(String customUDA3) {
		this.customUDA3 = customUDA3;
	}
	
	/**
	 * @return the CustomUDA4
	 */
	public String getCustomUDA4() {
		return customUDA4;
	}
	
	/**
	 * @param customUDA4
	 *            the customUDA4 to set
	 */
	public void setCustomUDA4(String customUDA4) {
		this.customUDA4 = customUDA4;
	}
	
	/**
	 * @return the CustomUDA5
	 */
	public String getCustomUDA5() {
		return customUDA5;
	}
	
	/**
	 * @param customUDA5
	 *            the customUDA5 to set
	 */
	public void setCustomUDA5(String customUDA5) {
		this.customUDA5 = customUDA5;
	}
	
	/**
	 * @return the CustomUDA6
	 */
	public String getCustomUDA6() {
		return customUDA6;
	}

	/**
	 * @param customUDA6
	 *            the customUDA6 to set
	 */
	public void setCustomUDA6(String customUDA6) {
		this.customUDA6 = customUDA6;
	}
	
	/**
	 * @return the CustomUDA7
	 */
	public String getCustomUDA7() {
		return customUDA7;
	}

	/**
	 * @param customUDA7
	 *            the customUDA7 to set
	 */
	public void setCustomUDA7(String customUDA7) {
		this.customUDA7 = customUDA7;
	}

	/**
	 * @return the CustomUDA8
	 */
	public String getCustomUDA8() {
		return customUDA8;
	}
	/**
	 * @param customUDA8
	 *            the customUDA8 to set
	 */
	public void setCustomUDA8(String customUDA8) {
		this.customUDA8 = customUDA8;
	}
	
	/**
	 * @return the CustomUDA9
	 */
	public String getCustomUDA9() {
		return customUDA9;
	}

	/**
	 * @param customUDA9
	 *            the customUDA9 to set
	 */
	public void setCustomUDA9(String customUDA9) {
		this.customUDA9 = customUDA9;
	}

	/**
	 * @return the CustomUDA10
	 */
	public String getCustomUDA10() {
		return customUDA10;
	}

	/**
	 * @param customUDA10
	 *            the customUDA10 to set
	 */
	public void setCustomUDA10(String customUDA10) {
		this.customUDA10 = customUDA10;
	}
	
	/**
	 * @return the CustomUDA11
	 */
	public String getCustomUDA11() {
		return customUDA11;
	}

	/**
	 * @param customUDA11
	 *            the customUDA11 to set
	 */
	public void setCustomUDA11(String customUDA11) {
		this.customUDA11 = customUDA11;
	}
	
	/**
	 * @return the CustomUDA12
	 */
	public String getCustomUDA12() {
		return customUDA12;
	}

	/**
	 * @param customUDA12
	 *            the customUDA12 to set
	 */
	public void setCustomUDA12(String customUDA12) {
		this.customUDA12 = customUDA12;
	}
	
	/**
	 * @return the userId
	 */
	public int getUserId() {
		return userId;
	}

	/**
	 * @param userId
	 *            the userId to set
	 */
	public void setUserId(int userId) {
		this.userId = userId;
	}

	/**
	 * @return the quantity
	 */
	public double getQuantity() {
		return quantity;
	}

	/**
	 * @param quantity
	 *            the quantity to set
	 */
	public void setQuantity(double quantity) {
		this.quantity = quantity;
	}

	/**
	 * @return the netMarketValueLocal
	 */
	public double getNetMarketValueLocal() {
		return netMarketValueLocal;
	}

	/**
	 * @param netMarketValueLocal
	 *            the netMarketValueLocal to set
	 */
	public void setNetMarketValueLocal(double netMarketValueLocal) {
		this.netMarketValueLocal = netMarketValueLocal;
	}

	/**
	 * @return the symbol
	 */
	public String getSymbol() {
		return symbol;
	}

	/**
	 * @param symbol
	 *            the symbol to set
	 */
	public void setSymbol(String symbol) {
		this.symbol = symbol;
	}

	/**
	 * @return the underlyingSymbol
	 */
	public String getUnderlyingSymbol() {
		return underlyingSymbol;
	}

	/**
	 * @param underlyingSymbol
	 *            the underlyingSymbol to set
	 */
	public void setUnderlyingSymbol(String underlyingSymbol) {
		this.underlyingSymbol = underlyingSymbol;
	}

	/**
	 * @return the masterFundName
	 */
	public String getMasterFundName() {
		return masterFundName;
	}

	/**
	 * @param masterFundName
	 *            the masterFundName to set
	 */
	public void setMasterFundName(String masterFundName) {
		this.masterFundName = masterFundName;
	}

	/**
	 * @return the exchange
	 */
	public String getExchange() {
		return exchange;
	}

	/**
	 * @param exchange
	 *            the exchange to set
	 */
	public void setExchange(String exchange) {
		this.exchange = exchange;
	}

	/**
	 * @return the currency
	 */
	public String getCurrency() {
		return currency;
	}

	/**
	 * @param currency
	 *            the currency to set
	 */
	public void setCurrency(String currency) {
		this.currency = currency;
	}

	/**
	 * @return the grossMarketValueLocal
	 */
	public double getGrossMarketValueLocal() {
		return grossMarketValueLocal;
	}

	/**
	 * @param grossMarketValueLocal
	 *            the grossMarketValueLocal to set
	 */
	public void setGrossMarketValueLocal(double grossMarketValueLocal) {
		this.grossMarketValueLocal = grossMarketValueLocal;
	}

	/**
	 * @return the sector
	 */
	public String getSector() {
		return sector;
	}

	/**
	 * @param sector
	 *            the sector to set
	 */
	public void setSector(String sector) {
		this.sector = sector;
	}

	/**
	 * @return the subSector
	 */
	public String getSubSector() {
		return subSector;
	}

	/**
	 * @param subSector
	 *            the subSector to set
	 */
	public void setSubSector(String subSector) {
		this.subSector = subSector;
	}

	/**
	 * @return the udaAsset
	 */
	public String getUdaAsset() {
		return udaAsset;
	}

	/**
	 * @param udaAsset
	 *            the udaAsset to set
	 */
	public void setUdaAsset(String udaAsset) {
		this.udaAsset = udaAsset;
	}

	/**
	 * @return the udaSymbol
	 */
	/*
	 * public String getUdaSymbol() { return udaSymbol; }
	 *//**
	 * @param udaSymbol
	 *            the udaSymbol to set
	 */
	/*
	 * public void setUdaSymbol(String udaSymbol) { this.udaSymbol = udaSymbol;
	 * }
	 */
	/**
	 * @return the udaSecurityType
	 */
	public String getUdaSecurityType() {
		return udaSecurityType;
	}

	/**
	 * @param udaSecurityType
	 *            the udaSecurityType to set
	 */
	public void setUdaSecurityType(String udaSecurityType) {
		this.udaSecurityType = udaSecurityType;
	}

	/**
	 * @return the udaCountry
	 */
	public String getUdaCountry() {
		return udaCountry;
	}

	/**
	 * @param udaCountry
	 *            the udaCountry to set
	 */
	public void setUdaCountry(String udaCountry) {
		this.udaCountry = udaCountry;
	}

	/**
	 * @return the asset
	 */
	public String getAsset() {
		return asset;
	}

	/**
	 * @param asset
	 *            the asset to set
	 */
	public void setAsset(String asset) {
		this.asset = asset;
	}

	/**
	 * @return the assetId
	 */
	public int getAssetId() {
		return assetId;
	}

	/**
	 * @param assetId
	 *            the assetId to set
	 */
	public void setAssetId(int assetId) {
		this.assetId = assetId;
	}

	/**
	 * @return the notionalLocal
	 */
	public double getNotionalLocal() {
		return notionalLocal;
	}

	/**
	 * @param notionalLocal
	 *            the notionalLocal to set
	 */
	public void setNotionalLocal(double notionalLocal) {
		this.notionalLocal = notionalLocal;
	}

	/**
	 * @return the equityHaircutLocal
	 */
	public double getEquityHaircutLocal() {
		return equityHaircutLocal;
	}

	/**
	 * @param equityHaircutLocal
	 *            the equityHaircutLocal to set
	 */
	public void setEquityHaircutLocal(double equityHaircutLocal) {
		this.equityHaircutLocal = equityHaircutLocal;
	}

	/**
	 * @return the indexHaircutLocal
	 */
	public double getIndexHaircutLocal() {
		return indexHaircutLocal;
	}

	/**
	 * @param indexHaircutLocal
	 *            the indexHaircutLocal to set
	 */
	public void setIndexHaircutLocal(double indexHaircutLocal) {
		this.indexHaircutLocal = indexHaircutLocal;
	}

	/**
	 * @return the cashImpactLocal
	 */
	public double getCashImpactLocal() {
		return cashImpactLocal;
	}

	/**
	 * @param cashImpactLocal
	 *            the cashImpactLocal to set
	 */
	public void setCashImpactLocal(double cashImpactLocal) {
		this.cashImpactLocal = cashImpactLocal;
	}

	/**
	 * @return the cashInflowLocal
	 */
	public double getCashInflowLocal() {
		return cashInflowLocal;
	}

	/**
	 * @param cashInflowLocal
	 *            the cashInflowLocal to set
	 */
	public void setCashInflowLocal(double cashInflowLocal) {
		this.cashInflowLocal = cashInflowLocal;
	}

	/**
	 * @return the cashOutflowLocal
	 */
	public double getCashOutflowLocal() {
		return cashOutflowLocal;
	}

	/**
	 * @param cashOutflowLocal
	 *            the cashOutflowLocal to set
	 */
	public void setCashOutflowLocal(double cashOutflowLocal) {
		this.cashOutflowLocal = cashOutflowLocal;
	}

	/**
	 * @return the costBasisPnLLocal
	 */
	public double getCostBasisPnLLocal() {
		return costBasisPnLLocal;
	}

	/**
	 * @param costBasisPnLLocal
	 *            the costBasisPnLLocal to set
	 */
	public void setCostBasisPnLLocal(double costBasisPnLLocal) {
		this.costBasisPnLLocal = costBasisPnLLocal;
	}

	/**
	 * @return the dayPnLLocal
	 */
	public double getDayPnLLocal() {
		return dayPnLLocal;
	}

	/**
	 * @param dayPnLLocal
	 *            the dayPnLLocal to set
	 */
	public void setDayPnLLocal(double dayPnLLocal) {
		this.dayPnLLocal = dayPnLLocal;
	}

	/**
	 * @return the netExposureLocal
	 */
	public double getNetExposureLocal() {
		return netExposureLocal;
	}

	/**
	 * @param netExposureLocal
	 *            the netExposureLocal to set
	 */
	public void setNetExposureLocal(double netExposureLocal) {
		this.netExposureLocal = netExposureLocal;
	}

	/**
	 * @return the grossExposureLocal
	 */
	public double getGrossExposureLocal() {
		return grossExposureLocal;
	}

	/**
	 * @param grossExposureLocal
	 *            the grossExposureLocal to set
	 */
	public void setGrossExposureLocal(double grossExposureLocal) {
		this.grossExposureLocal = grossExposureLocal;
	}

	/**
	 * @return the betaAdjustedExposureLocal
	 */
	public double getBetaAdjustedExposureLocal() {
		return betaAdjustedExposureLocal;
	}

	/**
	 * @param betaAdjustedExposureLocal
	 *            the betaAdjustedExposureLocal to set
	 */
	public void setBetaAdjustedExposureLocal(double betaAdjustedExposureLocal) {
		this.betaAdjustedExposureLocal = betaAdjustedExposureLocal;
	}

	/**
	 * @return the deltaAdjPosition
	 */
	public double getDeltaAdjPosition() {
		return deltaAdjPosition;
	}

	/**
	 * @param deltaAdjPosition
	 *            the deltaAdjPosition to set
	 */
	public void setDeltaAdjPosition(double deltaAdjPosition) {
		this.deltaAdjPosition = deltaAdjPosition;
	}

	/**
	 * @return the liquidationPnlLocal
	 */
	public double getLiquidationPnlLocal() {
		return liquidationPnlLocal;
	}

	/**
	 * @param liquidationPnlLocal
	 *            the liquidationPnlLocal to set
	 */
	public void setLiquidationPnlLocal(double liquidationPnlLocal) {
		this.liquidationPnlLocal = liquidationPnlLocal;
	}

	/**
	 * @return the underlyingValueForOptionsLocal
	 */
	public double getUnderlyingValueForOptionsLocal() {
		return underlyingValueForOptionsLocal;
	}

	/**
	 * @param underlyingValueForOptionsLocal
	 *            the underlyingValueForOptionsLocal to set
	 */
	public void setUnderlyingValueForOptionsLocal(
			double underlyingValueForOptionsLocal) {
		this.underlyingValueForOptionsLocal = underlyingValueForOptionsLocal;
	}

	/**
	 * @return the notionalBase
	 */
	public double getNotionalBase() {
		return notionalBase;
	}

	/**
	 * @param notionalBase
	 *            the notionalBase to set
	 */
	public void setNotionalBase(double notionalBase) {
		this.notionalBase = notionalBase;
	}

	/**
	 * @return the cashImpactBase
	 */
	public double getCashImpactBase() {
		return cashImpactBase;
	}

	/**
	 * @param cashImpactBase
	 *            the cashImpactBase to set
	 */
	public void setCashImpactBase(double cashImpactBase) {
		this.cashImpactBase = cashImpactBase;
	}

	/**
	 * @return the cashInflowBase
	 */
	public double getCashInflowBase() {
		return cashInflowBase;
	}

	/**
	 * @param cashInflowBase
	 *            the cashInflowBase to set
	 */
	public void setCashInflowBase(double cashInflowBase) {
		this.cashInflowBase = cashInflowBase;
	}

	/**
	 * @return the cashOutflowBase
	 */
	public double getCashOutflowBase() {
		return cashOutflowBase;
	}

	/**
	 * @param cashOutflowBase
	 *            the cashOutflowBase to set
	 */
	public void setCashOutflowBase(double cashOutflowBase) {
		this.cashOutflowBase = cashOutflowBase;
	}

	/**
	 * @return the netMarketValueBase
	 */
	public double getNetMarketValueBase() {
		return netMarketValueBase;
	}

	/**
	 * @param netMarketValueBase
	 *            the netMarketValueBase to set
	 */
	public void setNetMarketValueBase(double netMarketValueBase) {
		this.netMarketValueBase = netMarketValueBase;
	}

	/**
	 * @return the grossMarketValueBase
	 */
	public double getGrossMarketValueBase() {
		return grossMarketValueBase;
	}

	/**
	 * @param grossMarketValueBase
	 *            the grossMarketValueBase to set
	 */
	public void setGrossMarketValueBase(double grossMarketValueBase) {
		this.grossMarketValueBase = grossMarketValueBase;
	}

	/**
	 * @return the costBasisPnLBase
	 */
	public double getCostBasisPnLBase() {
		return costBasisPnLBase;
	}

	/**
	 * @param costBasisPnLBase
	 *            the costBasisPnLBase to set
	 */
	public void setCostBasisPnLBase(double costBasisPnLBase) {
		this.costBasisPnLBase = costBasisPnLBase;
	}

	/**
	 * @return the dayPnLBase
	 */
	public double getDayPnLBase() {
		return dayPnLBase;
	}

	/**
	 * @param dayPnLBase
	 *            the dayPnLBase to set
	 */
	public void setDayPnLBase(double dayPnLBase) {
		this.dayPnLBase = dayPnLBase;
	}

	/**
	 * @return the netExposureBase
	 */
	public double getNetExposureBase() {
		return netExposureBase;
	}

	/**
	 * @param netExposureBase
	 *            the netExposureBase to set
	 */
	public void setNetExposureBase(double netExposureBase) {
		this.netExposureBase = netExposureBase;
	}

	/**
	 * @return the grossExposureBase
	 */
	public double getGrossExposureBase() {
		return grossExposureBase;
	}

	/**
	 * @param grossExposureBase
	 *            the grossExposureBase to set
	 */
	public void setGrossExposureBase(double grossExposureBase) {
		this.grossExposureBase = grossExposureBase;
	}

	/**
	 * @return the betaAdjustedExposureBase
	 */
	public double getBetaAdjustedExposureBase() {
		return betaAdjustedExposureBase;
	}

	/**
	 * @param betaAdjustedExposureBase
	 *            the betaAdjustedExposureBase to set
	 */
	public void setBetaAdjustedExposureBase(double betaAdjustedExposureBase) {
		this.betaAdjustedExposureBase = betaAdjustedExposureBase;
	}

	/**
	 * @return the underlyingValueForOptionsBase
	 */
	public double getUnderlyingValueForOptionsBase() {
		return underlyingValueForOptionsBase;
	}

	/**
	 * @param underlyingValueForOptionsBase
	 *            the underlyingValueForOptionsBase to set
	 */
	public void setUnderlyingValueForOptionsBase(
			double underlyingValueForOptionsBase) {
		this.underlyingValueForOptionsBase = underlyingValueForOptionsBase;
	}

	/**
	 * @return the equityHircutBase
	 */
	public double getEquityHircutBase() {
		return equityHircutBase;
	}

	/**
	 * @param equityHircutBase
	 *            the equityHircutBase to set
	 */
	public void setEquityHircutBase(double equityHircutBase) {
		this.equityHircutBase = equityHircutBase;
	}

	/**
	 * @return the indexHaircutBase
	 */
	public double getIndexHaircutBase() {
		return indexHaircutBase;
	}

	/**
	 * @param indexHaircutBase
	 *            the indexHaircutBase to set
	 */
	public void setIndexHaircutBase(double indexHaircutBase) {
		this.indexHaircutBase = indexHaircutBase;
	}

	/**
	 * @return the liquidationPnlBase
	 */
	public double getLiquidationPnlBase() {
		return liquidationPnlBase;
	}

	/**
	 * @param liquidationPnlBase
	 *            the liquidationPnlBase to set
	 */
	public void setLiquidationPnlBase(double liquidationPnlBase) {
		this.liquidationPnlBase = liquidationPnlBase;
	}

	/**
	 * @return the defaultPositionSide
	 */
	public String getDefaultPositionSide() {
		return defaultPositionSide;
	}

	/**
	 * @param defaultPositionSide
	 *            the defaultPositionSide to set
	 */
	public void setDefaultPositionSide(String defaultPositionSide) {
		this.defaultPositionSide = defaultPositionSide;
	}

	/**
	 * @return the exposurePositionSide
	 */
	public String getExposurePositionSide() {
		return exposurePositionSide;
	}

	/**
	 * @param exposurePositionSide
	 *            the exposurePositionSide to set
	 */
	public void setExposurePositionSide(String exposurePositionSide) {
		this.exposurePositionSide = exposurePositionSide;
	}

	/**
	 * @return the longNotionalBase
	 */
	public double getLongNotionalBase() {
		return longNotionalBase;
	}

	/**
	 * @param longNotionalBase
	 *            the longNotionalBase to set
	 */
	public void setLongNotionalBase(double longNotionalBase) {
		this.longNotionalBase = longNotionalBase;
	}

	/**
	 * @return the shortNotionalBase
	 */
	public double getShortNotionalBase() {
		return shortNotionalBase;
	}

	/**
	 * @param shortNotionalBase
	 *            the shortNotionalBase to set
	 */
	public void setShortNotionalBase(double shortNotionalBase) {
		this.shortNotionalBase = shortNotionalBase;
	}

	/**
	 * @return the shortMarketValueBase
	 */
	public double getShortMarketValueBase() {
		return shortMarketValueBase;
	}

	/**
	 * @param shortMarketValueBase
	 *            the shortMarketValueBase to set
	 */
	public void setShortMarketValueBase(double shortMarketValueBase) {
		this.shortMarketValueBase = shortMarketValueBase;
	}

	/**
	 * @return the longMarketValueBase
	 */
	public double getLongMarketValueBase() {
		return longMarketValueBase;
	}

	/**
	 * @param longMarketValueBase
	 *            the longMarketValueBase to set
	 */
	public void setLongMarketValueBase(double longMarketValueBase) {
		this.longMarketValueBase = longMarketValueBase;
	}

	/**
	 * @return the shortExposureBase
	 */
	public double getShortExposureBase() {
		return shortExposureBase;
	}

	/**
	 * @param shortExposureBase
	 *            the shortExposureBase to set
	 */
	public void setShortExposureBase(double shortExposureBase) {
		this.shortExposureBase = shortExposureBase;
	}

	/**
	 * @return the longExposureBase
	 */
	public double getLongExposureBase() {
		return longExposureBase;
	}

	/**
	 * @param longExposureBase
	 *            the longExposureBase to set
	 */
	public void setLongExposureBase(double longExposureBase) {
		this.longExposureBase = longExposureBase;
	}

	/**
	 * @return the masterFundNav
	 */
	public double getMasterFundNav() {
		return masterFundNav;
	}

	/**
	 * @param masterFundNav
	 *            the masterFundNav to set
	 */
	public void setMasterFundNav(double masterFundNav) {
		this.masterFundNav = masterFundNav;
	}

	/**
	 * @return the percentLongExposureMasterFund
	 */
	public double getPercentLongExposureMasterFund() {
		return percentLongExposureMasterFund;
	}

	/**
	 * @param percentLongExposureMasterFund
	 *            the percentLongExposureMasterFund to set
	 */
	public void setPercentLongExposureMasterFund(
			double percentLongExposureMasterFund) {
		this.percentLongExposureMasterFund = percentLongExposureMasterFund;
	}

	/**
	 * @return the percentShortExposureMasterFund
	 */
	public double getPercentShortExposureMasterFund() {
		return percentShortExposureMasterFund;
	}

	/**
	 * @param percentShortExposureMasterFund
	 *            the percentShortExposureMasterFund to set
	 */
	public void setPercentShortExposureMasterFund(
			double percentShortExposureMasterFund) {
		this.percentShortExposureMasterFund = percentShortExposureMasterFund;
	}

	/**
	 * @return the percentNetExposureMasterFund
	 */
	public double getPercentNetExposureMasterFund() {
		return percentNetExposureMasterFund;
	}

	/**
	 * @param percentNetExposureMasterFund
	 *            the percentNetExposureMasterFund to set
	 */
	public void setPercentNetExposureMasterFund(
			double percentNetExposureMasterFund) {
		this.percentNetExposureMasterFund = percentNetExposureMasterFund;
	}

	/**
	 * @return the percentBetaAdjustedExposureMasterFund
	 */
	public double getPercentBetaAdjustedExposureMasterFund() {
		return percentBetaAdjustedExposureMasterFund;
	}

	/**
	 * @param percentBetaAdjustedExposureMasterFund
	 *            the percentBetaAdjustedExposureMasterFund to set
	 */
	public void setPercentBetaAdjustedExposureMasterFund(
			double percentBetaAdjustedExposureMasterFund) {
		this.percentBetaAdjustedExposureMasterFund = percentBetaAdjustedExposureMasterFund;
	}

	/**
	 * @return the percentGrossExposureMasterFund
	 */
	public double getPercentGrossExposureMasterFund() {
		return percentGrossExposureMasterFund;
	}

	/**
	 * @param percentGrossExposureMasterFund
	 *            the percentGrossExposureMasterFund to set
	 */
	public void setPercentGrossExposureMasterFund(
			double percentGrossExposureMasterFund) {
		this.percentGrossExposureMasterFund = percentGrossExposureMasterFund;
	}

	/**
	 * @return the percentLongMarketValueMasterFund
	 */
	public double getPercentLongMarketValueMasterFund() {
		return percentLongMarketValueMasterFund;
	}

	/**
	 * @param percentLongMarketValueMasterFund
	 *            the percentLongMarketValueMasterFund to set
	 */
	public void setPercentLongMarketValueMasterFund(
			double percentLongMarketValueMasterFund) {
		this.percentLongMarketValueMasterFund = percentLongMarketValueMasterFund;
	}

	/**
	 * @return the percentShortMarketValueMasterFund
	 */
	public double getPercentShortMarketValueMasterFund() {
		return percentShortMarketValueMasterFund;
	}

	/**
	 * @param percentShortMarketValueMasterFund
	 *            the percentShortMarketValueMasterFund to set
	 */
	public void setPercentShortMarketValueMasterFund(
			double percentShortMarketValueMasterFund) {
		this.percentShortMarketValueMasterFund = percentShortMarketValueMasterFund;
	}

	/**
	 * @return the percentNetMarketValueMasterFund
	 */
	public double getPercentNetMarketValueMasterFund() {
		return percentNetMarketValueMasterFund;
	}

	/**
	 * @param percentNetMarketValueMasterFund
	 *            the percentNetMarketValueMasterFund to set
	 */
	public void setPercentNetMarketValueMasterFund(
			double percentNetMarketValueMasterFund) {
		this.percentNetMarketValueMasterFund = percentNetMarketValueMasterFund;
	}

	/**
	 * @return the percentGrossMarketValueMasterFund
	 */
	public double getPercentGrossMarketValueMasterFund() {
		return percentGrossMarketValueMasterFund;
	}

	/**
	 * @param percentGrossMarketValueMasterFund
	 *            the percentGrossMarketValueMasterFund to set
	 */
	public void setPercentGrossMarketValueMasterFund(
			double percentGrossMarketValueMasterFund) {
		this.percentGrossMarketValueMasterFund = percentGrossMarketValueMasterFund;
	}

	/**
	 * @return the percentLongNotionalMasterFund
	 */
	public double getPercentLongNotionalMasterFund() {
		return percentLongNotionalMasterFund;
	}

	/**
	 * @param percentLongNotionalMasterFund
	 *            the percentLongNotionalMasterFund to set
	 */
	public void setPercentLongNotionalMasterFund(
			double percentLongNotionalMasterFund) {
		this.percentLongNotionalMasterFund = percentLongNotionalMasterFund;
	}

	/**
	 * @return the percentShortNotionalMasterFund
	 */
	public double getPercentShortNotionalMasterFund() {
		return percentShortNotionalMasterFund;
	}

	/**
	 * @param percentShortNotionalMasterFund
	 *            the percentShortNotionalMasterFund to set
	 */
	public void setPercentShortNotionalMasterFund(
			double percentShortNotionalMasterFund) {
		this.percentShortNotionalMasterFund = percentShortNotionalMasterFund;
	}

	/**
	 * @return the percentNetNotionalMasterFund
	 */
	public double getPercentNetNotionalMasterFund() {
		return percentNetNotionalMasterFund;
	}

	/**
	 * @param percentNetNotionalMasterFund
	 *            the percentNetNotionalMasterFund to set
	 */
	public void setPercentNetNotionalMasterFund(
			double percentNetNotionalMasterFund) {
		this.percentNetNotionalMasterFund = percentNetNotionalMasterFund;
	}

	/**
	 * @return the pnlContributionMasterFund
	 */
	public double getPnlContributionMasterFund() {
		return pnlContributionMasterFund;
	}

	/**
	 * @param pnlContributionMasterFund
	 *            the pnlContributionMasterFund to set
	 */
	public void setPnlContributionMasterFund(double pnlContributionMasterFund) {
		this.pnlContributionMasterFund = pnlContributionMasterFund;
	}

	/**
	 * @return the globalNav
	 */
	public double getGlobalNav() {
		return globalNav;
	}

	/**
	 * @param globalNav
	 *            the globalNav to set
	 */
	public void setGlobalNav(double globalNav) {
		this.globalNav = globalNav;
	}

	/**
	 * @return the percentLongExposureGlobal
	 */
	public double getPercentLongExposureGlobal() {
		return percentLongExposureGlobal;
	}

	/**
	 * @param percentLongExposureGlobal
	 *            the percentLongExposureGlobal to set
	 */
	public void setPercentLongExposureGlobal(double percentLongExposureGlobal) {
		this.percentLongExposureGlobal = percentLongExposureGlobal;
	}

	/**
	 * @return the percentShortExposureGlobal
	 */
	public double getPercentShortExposureGlobal() {
		return percentShortExposureGlobal;
	}

	/**
	 * @param percentShortExposureGlobal
	 *            the percentShortExposureGlobal to set
	 */
	public void setPercentShortExposureGlobal(double percentShortExposureGlobal) {
		this.percentShortExposureGlobal = percentShortExposureGlobal;
	}

	/**
	 * @return the percentNetExposureGlobal
	 */
	public double getPercentNetExposureGlobal() {
		return percentNetExposureGlobal;
	}

	/**
	 * @param percentNetExposureGlobal
	 *            the percentNetExposureGlobal to set
	 */
	public void setPercentNetExposureGlobal(double percentNetExposureGlobal) {
		this.percentNetExposureGlobal = percentNetExposureGlobal;
	}

	/**
	 * @return the percentBetaAdjustedExposureGlobal
	 */
	public double getPercentBetaAdjustedExposureGlobal() {
		return percentBetaAdjustedExposureGlobal;
	}

	/**
	 * @param percentBetaAdjustedExposureGlobal
	 *            the percentBetaAdjustedExposureGlobal to set
	 */
	public void setPercentBetaAdjustedExposureGlobal(
			double percentBetaAdjustedExposureGlobal) {
		this.percentBetaAdjustedExposureGlobal = percentBetaAdjustedExposureGlobal;
	}

	/**
	 * @return the percentGrossExposureGlobal
	 */
	public double getPercentGrossExposureGlobal() {
		return percentGrossExposureGlobal;
	}

	/**
	 * @param percentGrossExposureGlobal
	 *            the percentGrossExposureGlobal to set
	 */
	public void setPercentGrossExposureGlobal(double percentGrossExposureGlobal) {
		this.percentGrossExposureGlobal = percentGrossExposureGlobal;
	}

	/**
	 * @return the percentLongMarketValueGlobal
	 */
	public double getPercentLongMarketValueGlobal() {
		return percentLongMarketValueGlobal;
	}

	/**
	 * @param percentLongMarketValueGlobal
	 *            the percentLongMarketValueGlobal to set
	 */
	public void setPercentLongMarketValueGlobal(
			double percentLongMarketValueGlobal) {
		this.percentLongMarketValueGlobal = percentLongMarketValueGlobal;
	}

	/**
	 * @return the percentShortMarketValueGlobal
	 */
	public double getPercentShortMarketValueGlobal() {
		return percentShortMarketValueGlobal;
	}

	/**
	 * @param percentShortMarketValueGlobal
	 *            the percentShortMarketValueGlobal to set
	 */
	public void setPercentShortMarketValueGlobal(
			double percentShortMarketValueGlobal) {
		this.percentShortMarketValueGlobal = percentShortMarketValueGlobal;
	}

	/**
	 * @return the percentGrossMarketValueGlobal
	 */
	public double getPercentGrossMarketValueGlobal() {
		return percentGrossMarketValueGlobal;
	}

	/**
	 * @param percentGrossMarketValueGlobal
	 *            the percentGrossMarketValueGlobal to set
	 */
	public void setPercentGrossMarketValueGlobal(
			double percentGrossMarketValueGlobal) {
		this.percentGrossMarketValueGlobal = percentGrossMarketValueGlobal;
	}

	/**
	 * @return the percentLongNotionalGlobal
	 */
	public double getPercentLongNotionalGlobal() {
		return percentLongNotionalGlobal;
	}

	/**
	 * @param percentLongNotionalGlobal
	 *            the percentLongNotionalGlobal to set
	 */
	public void setPercentLongNotionalGlobal(double percentLongNotionalGlobal) {
		this.percentLongNotionalGlobal = percentLongNotionalGlobal;
	}

	/**
	 * @return the percentShortNotionalGlobal
	 */
	public double getPercentShortNotionalGlobal() {
		return percentShortNotionalGlobal;
	}

	/**
	 * @param percentShortNotionalGlobal
	 *            the percentShortNotionalGlobal to set
	 */
	public void setPercentShortNotionalGlobal(double percentShortNotionalGlobal) {
		this.percentShortNotionalGlobal = percentShortNotionalGlobal;
	}

	/**
	 * @return the percentNetNotionalGlobal
	 */
	public double getPercentNetNotionalGlobal() {
		return percentNetNotionalGlobal;
	}

	/**
	 * @param percentNetNotionalGlobal
	 *            the percentNetNotionalGlobal to set
	 */
	public void setPercentNetNotionalGlobal(double percentNetNotionalGlobal) {
		this.percentNetNotionalGlobal = percentNetNotionalGlobal;
	}

	/**
	 * @return the pnlContributionGlobal
	 */
	public double getPnlContributionGlobal() {
		return pnlContributionGlobal;
	}

	/**
	 * @param pnlContributionGlobal
	 *            the pnlContributionGlobal to set
	 */
	public void setPnlContributionGlobal(double pnlContributionGlobal) {
		this.pnlContributionGlobal = pnlContributionGlobal;
	}

	/**
	 * @return the openInterest
	 */
	public double getOpenInterest() {
		return openInterest;
	}

	/**
	 * @param openInterest
	 *            the openInterest to set
	 */
	public void setOpenInterest(double openInterest) {
		this.openInterest = openInterest;
	}

	/**
	 * @return the avgVolume20Days
	 */
	public double getAvgVolume20Days() {
		return avgVolume20Days;
	}

	/**
	 * @param avgVolume20Days
	 *            the avgVolume20Days to set
	 */
	public void setAvgVolume20Days(double avgVolume20Days) {
		this.avgVolume20Days = avgVolume20Days;
	}

	/**
	 * @return the percentAvgVolume
	 */
	public double getPercentAvgVolume() {
		return percentAvgVolume;
	}

	/**
	 * @param percentAvgVolume
	 *            the percentAvgVolume to set
	 */
	public void setPercentAvgVolume(double percentAvgVolume) {
		this.percentAvgVolume = percentAvgVolume;
	}

	/**
	 * @return the percentCostBasisPnlGlobal
	 */
	public double getPercentCostBasisPnlGlobal() {
		return percentCostBasisPnlGlobal;
	}

	/**
	 * @param percentCostBasisPnlGlobal
	 *            the percentCostBasisPnlGlobal to set
	 */
	public void setPercentCostBasisPnlGlobal(double percentCostBasisPnlGlobal) {
		this.percentCostBasisPnlGlobal = percentCostBasisPnlGlobal;
	}

	/**
	 * @return the percentCostBasisPnlMasterFund
	 */
	public double getPercentCostBasisPnlMasterFund() {
		return percentCostBasisPnlMasterFund;
	}

	/**
	 * @param percentCostBasisPnlMasterFund
	 *            the percentCostBasisPnlMasterFund to set
	 */
	public void setPercentCostBasisPnlMasterFund(
			double percentCostBasisPnlMasterFund) {
		this.percentCostBasisPnlMasterFund = percentCostBasisPnlMasterFund;
	}

	/**
	 * @return the sharesOutstanding
	 */
	public java.math.BigDecimal getSharesOutstanding() {
		return sharesOutstanding;
	}

	/**
	 * @param sharesOutstanding
	 *            the sharesOutstanding to set
	 */
	public void setSharesOutstanding(java.math.BigDecimal sharesOutstanding) {
		this.sharesOutstanding = sharesOutstanding;
	}

	/**
	 * @return the putorCall
	 */
	public String getPutOrCall() {
		return putOrCall;
	}

	/**
	 * @param putorCall
	 *            the putorCall to set
	 */
	public void setPutOrCall(String putOrCall) {
		this.putOrCall = putOrCall;
	}

	/**
	 * @return the strikePrice
	 */
	public double getStrikePrice() {
		return strikePrice;
	}

	/**
	 * @param strikePrice
	 *            the strikePrice to set
	 */
	public void setStrikePrice(double strikePrice) {
		this.strikePrice = strikePrice;
	}

	/**
	 * @return the askPrice
	 */
	public double getAskPrice() {
		return askPrice;
	}

	/**
	 * @param askPrice
	 *            the askPrice to set
	 */
	public void setAskPrice(double askPrice) {
		this.askPrice = askPrice;
	}

	/**
	 * @return the bidPrice
	 */
	public double getBidPrice() {
		return bidPrice;
	}

	/**
	 * @param bidPrice
	 *            the bidPrice to set
	 */
	public void setBidPrice(double bidPrice) {
		this.bidPrice = bidPrice;
	}

	/**
	 * @return the lowPrice
	 */
	public double getLowPrice() {
		return lowPrice;
	}

	/**
	 * @param lowPrice
	 *            the lowPrice to set
	 */
	public void setLowPrice(double lowPrice) {
		this.lowPrice = lowPrice;
	}

	/**
	 * @return the highPrice
	 */
	public double getHighPrice() {
		return highPrice;
	}

	/**
	 * @param highPrice
	 *            the highPrice to set
	 */
	public void setHighPrice(double highPrice) {
		this.highPrice = highPrice;
	}

	/**
	 * @return the openPrice
	 */
	public double getOpenPrice() {
		return openPrice;
	}

	/**
	 * @param openPrice
	 *            the openPrice to set
	 */
	public void setOpenPrice(double openPrice) {
		this.openPrice = openPrice;
	}

	/**
	 * @return the lastPrice
	 */
	public double getLastPrice() {
		return lastPrice;
	}

	/**
	 * @param lastPrice
	 *            the lastPrice to set
	 */
	public void setLastPrice(double lastPrice) {
		this.lastPrice = lastPrice;
	}

	/**
	 * @return the markPrice
	 */
	public double getMarkPrice() {
		return markPrice;
	}

	/**
	 * @param markPrice
	 *            the markPrice to set
	 */
	public void setMarkPrice(double markPrice) {
		this.markPrice = markPrice;
	}

	/**
	 * @return the delta
	 */
	public double getDelta() {
		return delta;
	}

	/**
	 * @param delta
	 *            the delta to set
	 */
	public void setDelta(double delta) {
		this.delta = delta;
	}

	/**
	 * @return the beta5YearMonthly
	 */
	public double getBeta5YearMonthly() {
		return beta5YearMonthly;
	}

	/**
	 * @param beta5YearMonthly
	 *            the beta5YearMonthly to set
	 */
	public void setBeta5YearMonthly(double beta5YearMonthly) {
		this.beta5YearMonthly = beta5YearMonthly;
	}

	/**
	 * @return the selectedFeedPrice
	 */
	public double getSelectedFeedPrice() {
		return selectedFeedPrice;
	}

	/**
	 * @param selectedFeedPrice
	 *            the selectedFeedPrice to set
	 */
	public void setSelectedFeedPrice(double selectedFeedPrice) {
		this.selectedFeedPrice = selectedFeedPrice;
	}

	/**
	 * @return the marketCapitalization
	 */
	public java.math.BigDecimal getMarketCapitalization() {
		return marketCapitalization;
	}

	/**
	 * @param marketCapitalization the marketCapitalization to set
	 */
	public void setMarketCapitalization(java.math.BigDecimal marketCapitalization) {
		this.marketCapitalization = marketCapitalization;
	}

	/**
	 * @return the avgVolume
	 */
	public double getAvgVolume() {
		return avgVolume;
	}

	/**
	 * @param avgVolume the avgVolume to set
	 */
	public void setAvgVolume(double avgVolume) {
		this.avgVolume = avgVolume;
	}

	public double getGrossMarketValueBaseMasterFund() {
		return grossMarketValueBaseMasterFund;
	}

	public void setGrossMarketValueBaseMasterFund(
			double grossMarketValueBaseMasterFund) {
		this.grossMarketValueBaseMasterFund = grossMarketValueBaseMasterFund;
	}

	public double getGrossExposureBaseMasterFund() {
		return grossExposureBaseMasterFund;
	}

	public void setGrossExposureBaseMasterFund(double grossExposureBaseMasterFund) {
		this.grossExposureBaseMasterFund = grossExposureBaseMasterFund;
	}

	public double getNetExposureBaseMasterFund() {
		return netExposureBaseMasterFund;
	}

	public void setNetExposureBaseMasterFund(double netExposureBaseMasterFund) {
		this.netExposureBaseMasterFund = netExposureBaseMasterFund;
	}

	public double getGrossMarketValueBaseGlobal() {
		return grossMarketValueBaseGlobal;
	}

	public void setGrossMarketValueBaseGlobal(double grossMarketValueBaseGlobal) {
		this.grossMarketValueBaseGlobal = grossMarketValueBaseGlobal;
	}

	public double getGrossExposureBaseGlobal() {
		return grossExposureBaseGlobal;
	}

	public void setGrossExposureBaseGlobal(double grossExposureBaseGlobal) {
		this.grossExposureBaseGlobal = grossExposureBaseGlobal;
	}

	public double getNetExposureBaseGlobal() {
		return netExposureBaseGlobal;
	}

	public void setNetExposureBaseGlobal(double netExposureBaseGlobal) {
		this.netExposureBaseGlobal = netExposureBaseGlobal;
	}

	/**
	 * @return the dayReturnMasterFund
	 */
	public double getDayReturnMasterFund() {
		return dayReturnMasterFund;
	}

	/**
	 * @param dayReturnMasterFund the dayReturnMasterFund to set
	 */
	public void setDayReturnMasterFund(double dayReturnMasterFund) {
		this.dayReturnMasterFund = dayReturnMasterFund;
	}

	/**
	 * @return the dayReturnGlobal
	 */
	public double getDayReturnGlobal() {
		return dayReturnGlobal;
	}

	/**
	 * @param dayReturnGlobal the dayReturnGlobal to set
	 */
	public void setDayReturnGlobal(double dayReturnGlobal) {
		this.dayReturnGlobal = dayReturnGlobal;
	}

	/**
	 * @return the startOfDayNavMasterFund
	 */
	public double getStartOfDayNavMasterFund() {
		return startOfDayNavMasterFund;
	}

	/**
	 * @param startOfDayNavMasterFund the startOfDayNavMasterFund to set
	 */
	public void setStartOfDayNavMasterFund(double startOfDayNavMasterFund) {
		this.startOfDayNavMasterFund = startOfDayNavMasterFund;
	}

	/**
	 * @return the startOfDayNavGlobal
	 */
	public double getStartOfDayNavGlobal() {
		return startOfDayNavGlobal;
	}

	/**
	 * @param startOfDayNavGlobal the startOfDayNavGlobal to set
	 */
	public void setStartOfDayNavGlobal(double startOfDayNavGlobal) {
		this.startOfDayNavGlobal = startOfDayNavGlobal;
	}

	/**
	 * @return the todayAbsTradeQuantity
	 */
	public double getTodayAbsTradeQuantity() {
		return todayAbsTradeQuantity;
	}

	/**
	 * @param todayAbsTradeQuantity the todayAbsTradeQuantity to set
	 */
	public void setTodayAbsTradeQuantity(double todayAbsTradeQuantity) {
		this.todayAbsTradeQuantity = todayAbsTradeQuantity;
	}

	/**
	 * @return the todayLongTradeQuantity
	 */
	public double getTodayLongTradeQuantity() {
		return todayLongTradeQuantity;
	}

	/**
	 * @param todayLongTradeQuantity the todayLongTradeQuantity to set
	 */
	public void setTodayLongTradeQuantity(double todayLongTradeQuantity) {
		this.todayLongTradeQuantity = todayLongTradeQuantity;
	}

	/**
	 * @return the todayShortTradeQuantity
	 */
	public double getTodayShortTradeQuantity() {
		return todayShortTradeQuantity;
	}

	/**
	 * @param todayShortTradeQuantity the todayShortTradeQuantity to set
	 */
	public void setTodayShortTradeQuantity(double todayShortTradeQuantity) {
		this.todayShortTradeQuantity = todayShortTradeQuantity;
	}

	/**
	 * @return the todayNetNotionalBase
	 */
	public double getTodayNetNotionalBase() {
		return todayNetNotionalBase;
	}

	/**
	 * @param todayNetNotionalBase the todayNetNotionalBase to set
	 */
	public void setTodayNetNotionalBase(double todayNetNotionalBase) {
		this.todayNetNotionalBase = todayNetNotionalBase;
	}

	/**
	 * @return the todayLongNotionalBase
	 */
	public double getTodayLongNotionalBase() {
		return todayLongNotionalBase;
	}

	/**
	 * @param todayLongNotionalBase the todayLongNotionalBase to set
	 */
	public void setTodayLongNotionalBase(double todayLongNotionalBase) {
		this.todayLongNotionalBase = todayLongNotionalBase;
	}

	/**
	 * @return the todayShortNotionalBase
	 */
	public double getTodayShortNotionalBase() {
		return todayShortNotionalBase;
	}

	/**
	 * @param todayShortNotionalBase the todayShortNotionalBase to set
	 */
	public void setTodayShortNotionalBase(double todayShortNotionalBase) {
		this.todayShortNotionalBase = todayShortNotionalBase;
	}

	/**
	 * @return the percentNetMarketValueGlobal
	 */
	public double getPercentNetMarketValueGlobal() {
		return percentNetMarketValueGlobal;
	}

	/**
	 * @param percentNetMarketValueGlobal the percentNetMarketValueGlobal to set
	 */
	public void setPercentNetMarketValueGlobal(double percentNetMarketValueGlobal) {
		this.percentNetMarketValueGlobal = percentNetMarketValueGlobal;
	}

}
