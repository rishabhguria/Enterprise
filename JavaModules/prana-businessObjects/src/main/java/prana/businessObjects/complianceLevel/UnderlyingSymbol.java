package prana.businessObjects.complianceLevel;

public class UnderlyingSymbol {

	private String underlyingSymbol;
	private double equityHaircutBase;
	private double indexHaircutBase;
	private double deltaAdjPosition;
	private double liquidationPnlBase;

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
	private int userId;

	private double percentNetExposureGlobal;
	private double percentBetaAdjustedExposureGlobal;
	private double percentGrossExposureGlobal;
	private double percentNetMarketValueGlobal;
	private double percentGrossMarketValueGlobal;
	private double percentNetNotionalGlobal;
	private double pnlContributionGlobal;

	private double percentCostBasisPnlGlobal;
	private double equivalent;
	private java.math.BigDecimal underlyingSharesOutstanding;

	private double globalNav;
	private double grossMarketValueBaseGlobal;
	private double grossExposureBaseGlobal;
	private double netExposureBaseGlobal;
	private double startOfDayNavGlobal;
	
	private double percentGrossExposureUnderlyingGlobal;
	private double grossExposureUnderlying;
	private java.math.BigDecimal underlyingMarketCapitalization;
	private String underlyingAsset;
	private String underlyingCountry;
	private double dayReturnGlobal;
	
	private double todayAbsTradeQuantity;
	private double todayLongTradeQuantity;
	private double todayShortTradeQuantity;
	private double todayNetNotionalBase;
	private double todayLongNotionalBase;
	private double todayShortNotionalBase;	
	
	private double longExposureUnderlying;
	private double shortExposureUnderlying;
	private String underlyingExposurePositionSide;
	
	private double betaAdjustedGrossExposureUnderlyingBase;
	private double betaAdjustedLongExposureUnderlyingBase;
	private double betaAdjustedShortExposureUnderlyingBase;
	
	private double percentBetaAdjustedGrossExposureUnderlyingBaseGlobal;
	private double percentBetaAdjustedLongExposureUnderlyingBaseGlobal;
	private double percentBetaAdjustedShortExposureUnderlyingBaseGlobal;
	
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
	
	private String exchange;
	private String udaCountry;
	
	/**
	 * @return the exchange
	 */
	public String getExchange() {
		return exchange;
	}

	/**
	 * @param exchange the exchange to set
	 */
	public void setExchange(String exchange) {
		this.exchange = exchange;
	}

	/**
	 * @return the udaCountry
	 */
	public String getUdaCountry() {
		return udaCountry;
	}

	/**
	 * @param udaCountry the udaCountry to set
	 */
	public void setUdaCountry(String udaCountry) {
		this.udaCountry = udaCountry;
	}
	
	/**
	 * @return the riskCurrency
	 */
	public String getRiskCurrency() {
		return riskCurrency;
	}

	/**
	 * @param riskCurrency the riskCurrency to set
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
	 * @param issuer the issuer to set
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
	 * @param countryOfRisk the countryOfRisk to set
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
	 * @param region the region to set
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
	 * @param analyst the analyst to set
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
	 * @param ucitsEligibleTag the ucitsEligibleTag to set
	 */
	public void setUcitsEligibleTag(String ucitsEligibleTag) {
		this.ucitsEligibleTag = ucitsEligibleTag;
	}

	/**
	 * @return the liquidTag
	 */
	public String getLiquidTag() {
		return liquidTag;
	}

	/**
	 * @param liquidTag the liquidTag to set
	 */
	public void setLiquidTag(String liquidTag) {
		this.liquidTag = liquidTag;
	}

	/**
	 * @return the marketCap
	 */
	public String getMarketCap() {
		return marketCap;
	}

	/**
	 * @param marketCap the marketCap to set
	 */
	public void setMarketCap(String marketCap) {
		this.marketCap = marketCap;
	}

	/**
	 * @return the customUDA1
	 */
	public String getCustomUDA1() {
		return customUDA1;
	}

	/**
	 * @param customUDA1 the customUDA1 to set
	 */
	public void setCustomUDA1(String customUDA1) {
		this.customUDA1 = customUDA1;
	}

	/**
	 * @return the customUDA2
	 */
	public String getCustomUDA2() {
		return customUDA2;
	}

	/**
	 * @param customUDA2 the customUDA2 to set
	 */
	public void setCustomUDA2(String customUDA2) {
		this.customUDA2 = customUDA2;
	}

	/**
	 * @return the customUDA3
	 */
	public String getCustomUDA3() {
		return customUDA3;
	}

	/**
	 * @param customUDA3 the customUDA3 to set
	 */
	public void setCustomUDA3(String customUDA3) {
		this.customUDA3 = customUDA3;
	}

	/**
	 * @return the customUDA4
	 */
	public String getCustomUDA4() {
		return customUDA4;
	}

	/**
	 * @param customUDA4 the customUDA4 to set
	 */
	public void setCustomUDA4(String customUDA4) {
		this.customUDA4 = customUDA4;
	}

	/**
	 * @return the customUDA5
	 */
	public String getCustomUDA5() {
		return customUDA5;
	}

	/**
	 * @param customUDA5 the customUDA5 to set
	 */
	public void setCustomUDA5(String customUDA5) {
		this.customUDA5 = customUDA5;
	}

	/**
	 * @return the customUDA6
	 */
	public String getCustomUDA6() {
		return customUDA6;
	}

	/**
	 * @param customUDA6 the customUDA6 to set
	 */
	public void setCustomUDA6(String customUDA6) {
		this.customUDA6 = customUDA6;
	}

	/**
	 * @return the customUDA7
	 */
	public String getCustomUDA7() {
		return customUDA7;
	}

	/**
	 * @param customUDA7 the customUDA7 to set
	 */
	public void setCustomUDA7(String customUDA7) {
		this.customUDA7 = customUDA7;
	}

	/**
	 * @return the customUDA8
	 */
	public String getCustomUDA8() {
		return customUDA8;
	}

	/**
	 * @param customUDA8 the customUDA8 to set
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
	 * @return the betaAdjustedGrossExposureUnderlyingBase
	 */
	public double getBetaAdjustedGrossExposureUnderlyingBase() {
		return betaAdjustedGrossExposureUnderlyingBase;
	}

	/**
	 * @param betaAdjustedGrossExposureUnderlyingBase the betaAdjustedGrossExposureUnderlyingBase to set
	 */
	public void setBetaAdjustedGrossExposureUnderlyingBase(
			double betaAdjustedGrossExposureUnderlyingBase) {
		this.betaAdjustedGrossExposureUnderlyingBase = betaAdjustedGrossExposureUnderlyingBase;
	}

	/**
	 * @return the betaAdjustedLongExposureUnderlyingBase
	 */
	public double getBetaAdjustedLongExposureUnderlyingBase() {
		return betaAdjustedLongExposureUnderlyingBase;
	}

	/**
	 * @param betaAdjustedLongExposureUnderlyingBase the betaAdjustedLongExposureUnderlyingBase to set
	 */
	public void setBetaAdjustedLongExposureUnderlyingBase(
			double betaAdjustedLongExposureUnderlyingBase) {
		this.betaAdjustedLongExposureUnderlyingBase = betaAdjustedLongExposureUnderlyingBase;
	}

	/**
	 * @return the betaAdjustedShortExposureUnderlyingBase
	 */
	public double getBetaAdjustedShortExposureUnderlyingBase() {
		return betaAdjustedShortExposureUnderlyingBase;
	}

	/**
	 * @param betaAdjustedShortExposureUnderlyingBase the betaAdjustedShortExposureUnderlyingBase to set
	 */
	public void setBetaAdjustedShortExposureUnderlyingBase(
			double betaAdjustedShortExposureUnderlyingBase) {
		this.betaAdjustedShortExposureUnderlyingBase = betaAdjustedShortExposureUnderlyingBase;
	}

	/**
	 * @return the percentBetaAdjustedGrossExposureUnderlyingBaseGlobal
	 */
	public double getPercentBetaAdjustedGrossExposureUnderlyingBaseGlobal() {
		return percentBetaAdjustedGrossExposureUnderlyingBaseGlobal;
	}

	/**
	 * @param percentBetaAdjustedGrossExposureUnderlyingBaseGlobal the percentBetaAdjustedGrossExposureUnderlyingBaseGlobal to set
	 */
	public void setPercentBetaAdjustedGrossExposureUnderlyingBaseGlobal(
			double percentBetaAdjustedGrossExposureUnderlyingBaseGlobal) {
		this.percentBetaAdjustedGrossExposureUnderlyingBaseGlobal = percentBetaAdjustedGrossExposureUnderlyingBaseGlobal;
	}

	/**
	 * @return the percentBetaAdjustedLongExposureUnderlyingBaseGlobal
	 */
	public double getPercentBetaAdjustedLongExposureUnderlyingBaseGlobal() {
		return percentBetaAdjustedLongExposureUnderlyingBaseGlobal;
	}

	/**
	 * @param percentBetaAdjustedLongExposureUnderlyingBaseGlobal the percentBetaAdjustedLongExposureUnderlyingBaseGlobal to set
	 */
	public void setPercentBetaAdjustedLongExposureUnderlyingBaseGlobal(
			double percentBetaAdjustedLongExposureUnderlyingBaseGlobal) {
		this.percentBetaAdjustedLongExposureUnderlyingBaseGlobal = percentBetaAdjustedLongExposureUnderlyingBaseGlobal;
	}

	/**
	 * @return the percentBetaAdjustedShortExposureUnderlyingBaseGlobal
	 */
	public double getPercentBetaAdjustedShortExposureUnderlyingBaseGlobal() {
		return percentBetaAdjustedShortExposureUnderlyingBaseGlobal;
	}

	/**
	 * @param percentBetaAdjustedShortExposureUnderlyingBaseGlobal the percentBetaAdjustedShortExposureUnderlyingBaseGlobal to set
	 */
	public void setPercentBetaAdjustedShortExposureUnderlyingBaseGlobal(
			double percentBetaAdjustedShortExposureUnderlyingBaseGlobal) {
		this.percentBetaAdjustedShortExposureUnderlyingBaseGlobal = percentBetaAdjustedShortExposureUnderlyingBaseGlobal;
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
	 * @return the equityHaircutBase
	 */
	public double getEquityHaircutBase() {
		return equityHaircutBase;
	}

	/**
	 * @param equityHaircutBase
	 *            the equityHaircutBase to set
	 */
	public void setEquityHaircutBase(double equityHaircutBase) {
		this.equityHaircutBase = equityHaircutBase;
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
	 * @return the percentNetMarketValueGlobal
	 */
	public double getPercentNetMarketValueGlobal() {
		return percentNetMarketValueGlobal;
	}

	/**
	 * @param percentNetMarketValueGlobal
	 *            the percentNetMarketValueGlobal to set
	 */
	public void setPercentNetMarketValueGlobal(
			double percentNetMarketValueGlobal) {
		this.percentNetMarketValueGlobal = percentNetMarketValueGlobal;
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
	 * @return the equivalent
	 */
	public double getEquivalent() {
		return equivalent;
	}

	/**
	 * @param equivalent
	 *            the equivalent to set
	 */
	public void setEquivalent(double equivalent) {
		this.equivalent = equivalent;
	}

	/**
	 * @return the underlyingSharesOutstanding
	 */
	public java.math.BigDecimal getUnderlyingSharesOutstanding() {
		return underlyingSharesOutstanding;
	}

	/**
	 * @param underlyingSharesOutstanding
	 *            the underlyingSharesOutstanding to set
	 */
	public void setUnderlyingSharesOutstanding(
			java.math.BigDecimal underlyingSharesOutstanding) {
		this.underlyingSharesOutstanding = underlyingSharesOutstanding;
	}

	/**
	 * @return the percentGrossExposureUnderlyingGlobal
	 */
	public double getPercentGrossExposureUnderlyingGlobal() {
		return percentGrossExposureUnderlyingGlobal;
	}

	/**
	 * @param percentGrossExposureUnderlyingGlobal
	 *            the percentGrossExposureUnderlyingGlobal to set
	 */
	public void setPercentGrossExposureUnderlyingGlobal(
			double percentGrossExposureUnderlyingGlobal) {
		this.percentGrossExposureUnderlyingGlobal = percentGrossExposureUnderlyingGlobal;
	}

	/**
	 * @return the grossExposureUnderlying
	 */
	public double getGrossExposureUnderlying() {
		return grossExposureUnderlying;
	}

	/**
	 * @param grossExposureUnderlying
	 *            the grossExposureUnderlying to set
	 */
	public void setGrossExposureUnderlying(double grossExposureUnderlying) {
		this.grossExposureUnderlying = grossExposureUnderlying;
	}

	/**
	 * @return the underlyingMarketCapitalization
	 */
	public java.math.BigDecimal getUnderlyingMarketCapitalization() {
		return underlyingMarketCapitalization;
	}

	/**
	 * @param underlyingMarketCapitalization the underlyingMarketCapitalization to set
	 */
	public void setUnderlyingMarketCapitalization(
			java.math.BigDecimal underlyingMarketCapitalization) {
		this.underlyingMarketCapitalization = underlyingMarketCapitalization;
	}

	/**
	 * @return the underlyingAsset
	 */
	public String getUnderlyingAsset() {
		return underlyingAsset;
	}

	/**
	 * @param underlyingAsset the underlyingAsset to set
	 */
	public void setUnderlyingAsset(String underlyingAsset) {
		this.underlyingAsset = underlyingAsset;
	}

	/**
	 * @return the underlyingCountry
	 */
	public String getUnderlyingCountry() {
		return underlyingCountry;
	}

	/**
	 * @param underlyingCountry the underlyingCountry to set
	 */
	public void setUnderlyingCountry(String underlyingCountry) {
		this.underlyingCountry = underlyingCountry;
	}

	public double getGlobalNav() {
		return globalNav;
	}

	public void setGlobalNav(double globalNav) {
		this.globalNav = globalNav;
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
	 * @return the longExposureUnderlying
	 */
	public double getLongExposureUnderlying() {
		return longExposureUnderlying;
	}

	/**
	 * @param longExposureUnderlying the longExposureUnderlying to set
	 */
	public void setLongExposureUnderlying(double longExposureUnderlying) {
		this.longExposureUnderlying = longExposureUnderlying;
	}

	/**
	 * @return the shortExposureUnderlying
	 */
	public double getShortExposureUnderlying() {
		return shortExposureUnderlying;
	}

	/**
	 * @param shortExposureUnderlying the shortExposureUnderlying to set
	 */
	public void setShortExposureUnderlying(double shortExposureUnderlying) {
		this.shortExposureUnderlying = shortExposureUnderlying;
	}

	/**
	 * @return the underlyingExposurePositionSide
	 */
	public String getUnderlyingExposurePositionSide() {
		return underlyingExposurePositionSide;
	}

	/**
	 * @param underlyingExposurePositionSide the underlyingExposurePositionSide to set
	 */
	public void setUnderlyingExposurePositionSide(
			String underlyingExposurePositionSide) {
		this.underlyingExposurePositionSide = underlyingExposurePositionSide;
	}

	
}