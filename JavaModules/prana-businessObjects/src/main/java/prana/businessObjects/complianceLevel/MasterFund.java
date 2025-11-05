package prana.businessObjects.complianceLevel;

public class MasterFund {

	private int userId;

	private String masterFundName;
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
	private double longNotionalBase;
	private double shortNotionalBase;
	private double shortMarketValueBase;
	private double longMarketValueBase;
	private double shortExposureBase;
	private double longExposureBase;

	private double equityHircutBase;
	private double indexHaircutBase;
	private double liquidationPnlBase;


	private double masterFundNav;
	private double startOfDayNavMasterFund;
	private double currentCash;
	private double accrual;
	
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
	private double percentNetMarketValueGlobal;
	private double percentGrossMarketValueGlobal;
	private double percentLongNotionalGlobal;
	private double percentShortNotionalGlobal;
	private double percentNetNotionalGlobal;

	private double pnlContributionGlobal;

	private double percentCostBasisPnlGlobal;
	private double percentCostBasisPnlMasterFund;
	private double percentGrossExposureUnderlyingGlobal;
	private double grossExposureUnderlying;
	private double percentGrossExposureUnderlyingMasterFund;
	private double dayReturnGlobal;
	private double dayReturnMasterFund;
	
	private double todayAbsTradeQuantity;
	private double todayLongTradeQuantity;
	private double todayShortTradeQuantity;
	private double todayNetNotionalBase;
	private double todayLongNotionalBase;
	private double todayShortNotionalBase;	
	
	private double longExposureUnderlying;
	private double shortExposureUnderlying;
	
	private double betaAdjustedGrossExposureUnderlyingBase;
	private double betaAdjustedLongExposureUnderlyingBase;
	private double betaAdjustedShortExposureUnderlyingBase;
	
	private double percentBetaAdjustedGrossExposureUnderlyingBaseGlobal;
	private double percentBetaAdjustedLongExposureUnderlyingBaseGlobal;
	private double percentBetaAdjustedShortExposureUnderlyingBaseGlobal;
	
	private double percentBetaAdjustedGrossExposureUnderlyingBaseMasterFund;
	private double percentBetaAdjustedLongExposureUnderlyingBaseMasterFund;
	private double percentBetaAdjustedShortExposureUnderlyingBaseMasterFund;
	
	
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
	 * @return the percentBetaAdjustedGrossExposureUnderlyingBaseMasterFund
	 */
	public double getPercentBetaAdjustedGrossExposureUnderlyingBaseMasterFund() {
		return percentBetaAdjustedGrossExposureUnderlyingBaseMasterFund;
	}

	/**
	 * @param percentBetaAdjustedGrossExposureUnderlyingBaseMasterFund the percentBetaAdjustedGrossExposureUnderlyingBaseMasterFund to set
	 */
	public void setPercentBetaAdjustedGrossExposureUnderlyingBaseMasterFund(
			double percentBetaAdjustedGrossExposureUnderlyingBaseMasterFund) {
		this.percentBetaAdjustedGrossExposureUnderlyingBaseMasterFund = percentBetaAdjustedGrossExposureUnderlyingBaseMasterFund;
	}

	/**
	 * @return the percentBetaAdjustedLongExposureUnderlyingBaseMasterFund
	 */
	public double getPercentBetaAdjustedLongExposureUnderlyingBaseMasterFund() {
		return percentBetaAdjustedLongExposureUnderlyingBaseMasterFund;
	}

	/**
	 * @param percentBetaAdjustedLongExposureUnderlyingBaseMasterFund the percentBetaAdjustedLongExposureUnderlyingBaseMasterFund to set
	 */
	public void setPercentBetaAdjustedLongExposureUnderlyingBaseMasterFund(
			double percentBetaAdjustedLongExposureUnderlyingBaseMasterFund) {
		this.percentBetaAdjustedLongExposureUnderlyingBaseMasterFund = percentBetaAdjustedLongExposureUnderlyingBaseMasterFund;
	}

	/**
	 * @return the percentBetaAdjustedShortExposureUnderlyingBaseMasterFund
	 */
	public double getPercentBetaAdjustedShortExposureUnderlyingBaseMasterFund() {
		return percentBetaAdjustedShortExposureUnderlyingBaseMasterFund;
	}

	/**
	 * @param percentBetaAdjustedShortExposureUnderlyingBaseMasterFund the percentBetaAdjustedShortExposureUnderlyingBaseMasterFund to set
	 */
	public void setPercentBetaAdjustedShortExposureUnderlyingBaseMasterFund(
			double percentBetaAdjustedShortExposureUnderlyingBaseMasterFund) {
		this.percentBetaAdjustedShortExposureUnderlyingBaseMasterFund = percentBetaAdjustedShortExposureUnderlyingBaseMasterFund;
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
	 * @return the percentGrossExposureUnderlyingMasterFund
	 */
	public double getPercentGrossExposureUnderlyingMasterFund() {
		return percentGrossExposureUnderlyingMasterFund;
	}

	/**
	 * @param percentGrossExposureUnderlyingMasterFund
	 *            the percentGrossExposureUnderlyingMasterFund to set
	 */
	public void setPercentGrossExposureUnderlyingMasterFund(
			double percentGrossExposureUnderlyingMasterFund) {
		this.percentGrossExposureUnderlyingMasterFund = percentGrossExposureUnderlyingMasterFund;
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
	 * @return the currentCash
	 */
	public double getCurrentCash() {
		return currentCash;
	}

	/**
	 * @param currentCash the currentCash to set
	 */
	public void setCurrentCash(double currentCash) {
		this.currentCash = currentCash;
	}

	/**
	 * @return the accrual
	 */
	public double getAccrual() {
		return accrual;
	}

	/**
	 * @param accrual the accrual to set
	 */
	public void setAccrual(double accrual) {
		this.accrual = accrual;
	}

}
