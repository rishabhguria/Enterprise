package prana.businessObjects.complianceLevel;

public class Sector {
	private String sector;

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

	private double globalNav;
	private double grossMarketValueBaseGlobal;
	private double grossExposureBaseGlobal;
	private double netExposureBaseGlobal;
	private double startOfDayNavGlobal;

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
	private double dayReturnGlobal;
	
	private double todayAbsTradeQuantity;
	private double todayLongTradeQuantity;
	private double todayShortTradeQuantity;
	private double todayNetNotionalBase;
	private double todayLongNotionalBase;
	private double todayShortNotionalBase;	

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

}
