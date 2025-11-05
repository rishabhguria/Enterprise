package prana.businessObjects.complianceLevel;

public class Account_UnderlyingSymbol

{
	private int userId;

	private String underlyingSymbol;

	private String accountShortName;
	private String accountLongName;
	private String masterFundName;

	private double equityHaircutBase;
	private double indexHaircutBase;

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

	private double accountNav;
	private double grossMarketValueBaseAccount;
	private double grossExposureBaseAccount;
	private double netExposureBaseAccount;
	private double startOfDayNavAccount;
	
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

	private double percentNetExposureAccount;
	private double percentBetaAdjustedExposureAccount;
	private double percentGrossExposureAccount;
	private double percentNetMarketValueAccount;
	private double percentGrossMarketValueAccount;
	private double percentNetNotionalAccount;
	private double pnlContributionAccount;

	

	private double percentNetExposureMasterFund;
	private double percentBetaAdjustedExposureMasterFund;
	private double percentGrossExposureMasterFund;
	private double percentGrossMarketValueMasterFund;
	private double percentNetNotionalMasterFund;
	private double pnlContributionMasterFund;
	private double percentNetMarketValueMasterFund;

	

	private double percentNetExposureGlobal;
	private double percentBetaAdjustedExposureGlobal;
	private double percentGrossExposureGlobal;
	private double percentGrossMarketValueGlobal;
	private double percentNetNotionalGlobal;
	private double pnlContributionGlobal;
	private double percentNetMarketValueGlobal;

	private double percentCostBasisPnlGlobal;
	private double percentCostBasisPnlMasterFund;
	private double percentCostBasisPnlAccount;
	private double equivalent;
	private java.math.BigDecimal underlyingSharesOutstanding;

	private double percentGrossExposureUnderlyingGlobal;
	private double grossExposureUnderlying;
	private double percentGrossExposureUnderlyingMasterFund;
	private double percentGrossExposureUnderlyingAccount;
	private java.math.BigDecimal underlyingMarketCapitalization;
	private double accountNRA;
	private String underlyingAsset;
	private String underlyingCountry;
	private double dayReturnGlobal;
	private double dayReturnAccount;
	private double dayReturnMasterFund;
	
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
	
	private double percentBetaAdjustedGrossExposureUnderlyingBaseAccount;
	private double percentBetaAdjustedLongExposureUnderlyingBaseAccount;
	private double percentBetaAdjustedShortExposureUnderlyingBaseAccount;
	
	private double percentBetaAdjustedGrossExposureUnderlyingBaseMasterFund;
	private double percentBetaAdjustedLongExposureUnderlyingBaseMasterFund;
	private double percentBetaAdjustedShortExposureUnderlyingBaseMasterFund;
	
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
	private double deltaAdjPosition;
	/**
	 * @return the userId
	 */
	public int getUserId() {
		return userId;
	}
	/**
	 * @param userId the userId to set
	 */
	public void setUserId(int userId) {
		this.userId = userId;
	}
	/**
	 * @return the underlyingSymbol
	 */
	public String getUnderlyingSymbol() {
		return underlyingSymbol;
	}
	/**
	 * @param underlyingSymbol the underlyingSymbol to set
	 */
	public void setUnderlyingSymbol(String underlyingSymbol) {
		this.underlyingSymbol = underlyingSymbol;
	}
	/**
	 * @return the accountShortName
	 */
	public String getAccountShortName() {
		return accountShortName;
	}
	/**
	 * @param accountShortName the accountShortName to set
	 */
	public void setAccountShortName(String accountShortName) {
		this.accountShortName = accountShortName;
	}
	/**
	 * @return the accountLongName
	 */
	public String getAccountLongName() {
		return accountLongName;
	}
	/**
	 * @param accountLongName the accountLongName to set
	 */
	public void setAccountLongName(String accountLongName) {
		this.accountLongName = accountLongName;
	}
	/**
	 * @return the masterFundName
	 */
	public String getMasterFundName() {
		return masterFundName;
	}
	/**
	 * @param masterFundName the masterFundName to set
	 */
	public void setMasterFundName(String masterFundName) {
		this.masterFundName = masterFundName;
	}
	/**
	 * @return the equityHaircutBase
	 */
	public double getEquityHaircutBase() {
		return equityHaircutBase;
	}
	/**
	 * @param equityHaircutBase the equityHaircutBase to set
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
	 * @param indexHaircutBase the indexHaircutBase to set
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
	 * @param liquidationPnlBase the liquidationPnlBase to set
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
	 * @param notionalBase the notionalBase to set
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
	 * @param cashImpactBase the cashImpactBase to set
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
	 * @param cashInflowBase the cashInflowBase to set
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
	 * @param cashOutflowBase the cashOutflowBase to set
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
	 * @param netMarketValueBase the netMarketValueBase to set
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
	 * @param grossMarketValueBase the grossMarketValueBase to set
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
	 * @param costBasisPnLBase the costBasisPnLBase to set
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
	 * @param dayPnLBase the dayPnLBase to set
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
	 * @param netExposureBase the netExposureBase to set
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
	 * @param grossExposureBase the grossExposureBase to set
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
	 * @param betaAdjustedExposureBase the betaAdjustedExposureBase to set
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
	 * @param underlyingValueForOptionsBase the underlyingValueForOptionsBase to set
	 */
	public void setUnderlyingValueForOptionsBase(
			double underlyingValueForOptionsBase) {
		this.underlyingValueForOptionsBase = underlyingValueForOptionsBase;
	}
	/**
	 * @return the accountNav
	 */
	public double getAccountNav() {
		return accountNav;
	}
	/**
	 * @param accountNav the accountNav to set
	 */
	public void setAccountNav(double accountNav) {
		this.accountNav = accountNav;
	}
	/**
	 * @return the grossMarketValueBaseAccount
	 */
	public double getGrossMarketValueBaseAccount() {
		return grossMarketValueBaseAccount;
	}
	/**
	 * @param grossMarketValueBaseAccount the grossMarketValueBaseAccount to set
	 */
	public void setGrossMarketValueBaseAccount(double grossMarketValueBaseAccount) {
		this.grossMarketValueBaseAccount = grossMarketValueBaseAccount;
	}
	/**
	 * @return the grossExposureBaseAccount
	 */
	public double getGrossExposureBaseAccount() {
		return grossExposureBaseAccount;
	}
	/**
	 * @param grossExposureBaseAccount the grossExposureBaseAccount to set
	 */
	public void setGrossExposureBaseAccount(double grossExposureBaseAccount) {
		this.grossExposureBaseAccount = grossExposureBaseAccount;
	}
	/**
	 * @return the netExposureBaseAccount
	 */
	public double getNetExposureBaseAccount() {
		return netExposureBaseAccount;
	}
	/**
	 * @param netExposureBaseAccount the netExposureBaseAccount to set
	 */
	public void setNetExposureBaseAccount(double netExposureBaseAccount) {
		this.netExposureBaseAccount = netExposureBaseAccount;
	}
	/**
	 * @return the startOfDayNavAccount
	 */
	public double getStartOfDayNavAccount() {
		return startOfDayNavAccount;
	}
	/**
	 * @param startOfDayNavAccount the startOfDayNavAccount to set
	 */
	public void setStartOfDayNavAccount(double startOfDayNavAccount) {
		this.startOfDayNavAccount = startOfDayNavAccount;
	}
	/**
	 * @return the masterFundNav
	 */
	public double getMasterFundNav() {
		return masterFundNav;
	}
	/**
	 * @param masterFundNav the masterFundNav to set
	 */
	public void setMasterFundNav(double masterFundNav) {
		this.masterFundNav = masterFundNav;
	}
	/**
	 * @return the grossMarketValueBaseMasterFund
	 */
	public double getGrossMarketValueBaseMasterFund() {
		return grossMarketValueBaseMasterFund;
	}
	/**
	 * @param grossMarketValueBaseMasterFund the grossMarketValueBaseMasterFund to set
	 */
	public void setGrossMarketValueBaseMasterFund(
			double grossMarketValueBaseMasterFund) {
		this.grossMarketValueBaseMasterFund = grossMarketValueBaseMasterFund;
	}
	/**
	 * @return the grossExposureBaseMasterFund
	 */
	public double getGrossExposureBaseMasterFund() {
		return grossExposureBaseMasterFund;
	}
	/**
	 * @param grossExposureBaseMasterFund the grossExposureBaseMasterFund to set
	 */
	public void setGrossExposureBaseMasterFund(double grossExposureBaseMasterFund) {
		this.grossExposureBaseMasterFund = grossExposureBaseMasterFund;
	}
	/**
	 * @return the netExposureBaseMasterFund
	 */
	public double getNetExposureBaseMasterFund() {
		return netExposureBaseMasterFund;
	}
	/**
	 * @param netExposureBaseMasterFund the netExposureBaseMasterFund to set
	 */
	public void setNetExposureBaseMasterFund(double netExposureBaseMasterFund) {
		this.netExposureBaseMasterFund = netExposureBaseMasterFund;
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
	 * @return the globalNav
	 */
	public double getGlobalNav() {
		return globalNav;
	}
	/**
	 * @param globalNav the globalNav to set
	 */
	public void setGlobalNav(double globalNav) {
		this.globalNav = globalNav;
	}
	/**
	 * @return the grossMarketValueBaseGlobal
	 */
	public double getGrossMarketValueBaseGlobal() {
		return grossMarketValueBaseGlobal;
	}
	/**
	 * @param grossMarketValueBaseGlobal the grossMarketValueBaseGlobal to set
	 */
	public void setGrossMarketValueBaseGlobal(double grossMarketValueBaseGlobal) {
		this.grossMarketValueBaseGlobal = grossMarketValueBaseGlobal;
	}
	/**
	 * @return the grossExposureBaseGlobal
	 */
	public double getGrossExposureBaseGlobal() {
		return grossExposureBaseGlobal;
	}
	/**
	 * @param grossExposureBaseGlobal the grossExposureBaseGlobal to set
	 */
	public void setGrossExposureBaseGlobal(double grossExposureBaseGlobal) {
		this.grossExposureBaseGlobal = grossExposureBaseGlobal;
	}
	/**
	 * @return the netExposureBaseGlobal
	 */
	public double getNetExposureBaseGlobal() {
		return netExposureBaseGlobal;
	}
	/**
	 * @param netExposureBaseGlobal the netExposureBaseGlobal to set
	 */
	public void setNetExposureBaseGlobal(double netExposureBaseGlobal) {
		this.netExposureBaseGlobal = netExposureBaseGlobal;
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
	 * @return the percentNetExposureAccount
	 */
	public double getPercentNetExposureAccount() {
		return percentNetExposureAccount;
	}
	/**
	 * @param percentNetExposureAccount the percentNetExposureAccount to set
	 */
	public void setPercentNetExposureAccount(double percentNetExposureAccount) {
		this.percentNetExposureAccount = percentNetExposureAccount;
	}
	/**
	 * @return the percentBetaAdjustedExposureAccount
	 */
	public double getPercentBetaAdjustedExposureAccount() {
		return percentBetaAdjustedExposureAccount;
	}
	/**
	 * @param percentBetaAdjustedExposureAccount the percentBetaAdjustedExposureAccount to set
	 */
	public void setPercentBetaAdjustedExposureAccount(
			double percentBetaAdjustedExposureAccount) {
		this.percentBetaAdjustedExposureAccount = percentBetaAdjustedExposureAccount;
	}
	/**
	 * @return the percentGrossExposureAccount
	 */
	public double getPercentGrossExposureAccount() {
		return percentGrossExposureAccount;
	}
	/**
	 * @param percentGrossExposureAccount the percentGrossExposureAccount to set
	 */
	public void setPercentGrossExposureAccount(double percentGrossExposureAccount) {
		this.percentGrossExposureAccount = percentGrossExposureAccount;
	}
	/**
	 * @return the percentNetMarketValueAccount
	 */
	public double getPercentNetMarketValueAccount() {
		return percentNetMarketValueAccount;
	}
	/**
	 * @param percentNetMarketValueAccount the percentNetMarketValueAccount to set
	 */
	public void setPercentNetMarketValueAccount(double percentNetMarketValueAccount) {
		this.percentNetMarketValueAccount = percentNetMarketValueAccount;
	}
	/**
	 * @return the percentGrossMarketValueAccount
	 */
	public double getPercentGrossMarketValueAccount() {
		return percentGrossMarketValueAccount;
	}
	/**
	 * @param percentGrossMarketValueAccount the percentGrossMarketValueAccount to set
	 */
	public void setPercentGrossMarketValueAccount(
			double percentGrossMarketValueAccount) {
		this.percentGrossMarketValueAccount = percentGrossMarketValueAccount;
	}
	/**
	 * @return the percentNetNotionalAccount
	 */
	public double getPercentNetNotionalAccount() {
		return percentNetNotionalAccount;
	}
	/**
	 * @param percentNetNotionalAccount the percentNetNotionalAccount to set
	 */
	public void setPercentNetNotionalAccount(double percentNetNotionalAccount) {
		this.percentNetNotionalAccount = percentNetNotionalAccount;
	}
	/**
	 * @return the pnlContributionAccount
	 */
	public double getPnlContributionAccount() {
		return pnlContributionAccount;
	}
	/**
	 * @param pnlContributionAccount the pnlContributionAccount to set
	 */
	public void setPnlContributionAccount(double pnlContributionAccount) {
		this.pnlContributionAccount = pnlContributionAccount;
	}
	/**
	 * @return the percentNetExposureMasterFund
	 */
	public double getPercentNetExposureMasterFund() {
		return percentNetExposureMasterFund;
	}
	/**
	 * @param percentNetExposureMasterFund the percentNetExposureMasterFund to set
	 */
	public void setPercentNetExposureMasterFund(double percentNetExposureMasterFund) {
		this.percentNetExposureMasterFund = percentNetExposureMasterFund;
	}
	/**
	 * @return the percentBetaAdjustedExposureMasterFund
	 */
	public double getPercentBetaAdjustedExposureMasterFund() {
		return percentBetaAdjustedExposureMasterFund;
	}
	/**
	 * @param percentBetaAdjustedExposureMasterFund the percentBetaAdjustedExposureMasterFund to set
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
	 * @param percentGrossExposureMasterFund the percentGrossExposureMasterFund to set
	 */
	public void setPercentGrossExposureMasterFund(
			double percentGrossExposureMasterFund) {
		this.percentGrossExposureMasterFund = percentGrossExposureMasterFund;
	}
	/**
	 * @return the percentGrossMarketValueMasterFund
	 */
	public double getPercentGrossMarketValueMasterFund() {
		return percentGrossMarketValueMasterFund;
	}
	/**
	 * @param percentGrossMarketValueMasterFund the percentGrossMarketValueMasterFund to set
	 */
	public void setPercentGrossMarketValueMasterFund(
			double percentGrossMarketValueMasterFund) {
		this.percentGrossMarketValueMasterFund = percentGrossMarketValueMasterFund;
	}
	/**
	 * @return the percentNetNotionalMasterFund
	 */
	public double getPercentNetNotionalMasterFund() {
		return percentNetNotionalMasterFund;
	}
	/**
	 * @param percentNetNotionalMasterFund the percentNetNotionalMasterFund to set
	 */
	public void setPercentNetNotionalMasterFund(double percentNetNotionalMasterFund) {
		this.percentNetNotionalMasterFund = percentNetNotionalMasterFund;
	}
	/**
	 * @return the pnlContributionMasterFund
	 */
	public double getPnlContributionMasterFund() {
		return pnlContributionMasterFund;
	}
	/**
	 * @param pnlContributionMasterFund the pnlContributionMasterFund to set
	 */
	public void setPnlContributionMasterFund(double pnlContributionMasterFund) {
		this.pnlContributionMasterFund = pnlContributionMasterFund;
	}
	/**
	 * @return the percentNetMarketValueMasterFund
	 */
	public double getPercentNetMarketValueMasterFund() {
		return percentNetMarketValueMasterFund;
	}
	/**
	 * @param percentNetMarketValueMasterFund the percentNetMarketValueMasterFund to set
	 */
	public void setPercentNetMarketValueMasterFund(
			double percentNetMarketValueMasterFund) {
		this.percentNetMarketValueMasterFund = percentNetMarketValueMasterFund;
	}
	/**
	 * @return the percentNetExposureGlobal
	 */
	public double getPercentNetExposureGlobal() {
		return percentNetExposureGlobal;
	}
	/**
	 * @param percentNetExposureGlobal the percentNetExposureGlobal to set
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
	 * @param percentBetaAdjustedExposureGlobal the percentBetaAdjustedExposureGlobal to set
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
	 * @param percentGrossExposureGlobal the percentGrossExposureGlobal to set
	 */
	public void setPercentGrossExposureGlobal(double percentGrossExposureGlobal) {
		this.percentGrossExposureGlobal = percentGrossExposureGlobal;
	}
	/**
	 * @return the percentGrossMarketValueGlobal
	 */
	public double getPercentGrossMarketValueGlobal() {
		return percentGrossMarketValueGlobal;
	}
	/**
	 * @param percentGrossMarketValueGlobal the percentGrossMarketValueGlobal to set
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
	 * @param percentNetNotionalGlobal the percentNetNotionalGlobal to set
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
	 * @param pnlContributionGlobal the pnlContributionGlobal to set
	 */
	public void setPnlContributionGlobal(double pnlContributionGlobal) {
		this.pnlContributionGlobal = pnlContributionGlobal;
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
	 * @return the percentCostBasisPnlGlobal
	 */
	public double getPercentCostBasisPnlGlobal() {
		return percentCostBasisPnlGlobal;
	}
	/**
	 * @param percentCostBasisPnlGlobal the percentCostBasisPnlGlobal to set
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
	 * @param percentCostBasisPnlMasterFund the percentCostBasisPnlMasterFund to set
	 */
	public void setPercentCostBasisPnlMasterFund(
			double percentCostBasisPnlMasterFund) {
		this.percentCostBasisPnlMasterFund = percentCostBasisPnlMasterFund;
	}
	/**
	 * @return the percentCostBasisPnlAccount
	 */
	public double getPercentCostBasisPnlAccount() {
		return percentCostBasisPnlAccount;
	}
	/**
	 * @param percentCostBasisPnlAccount the percentCostBasisPnlAccount to set
	 */
	public void setPercentCostBasisPnlAccount(double percentCostBasisPnlAccount) {
		this.percentCostBasisPnlAccount = percentCostBasisPnlAccount;
	}
	/**
	 * @return the equivalent
	 */
	public double getEquivalent() {
		return equivalent;
	}
	/**
	 * @param equivalent the equivalent to set
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
	 * @param underlyingSharesOutstanding the underlyingSharesOutstanding to set
	 */
	public void setUnderlyingSharesOutstanding(java.math.BigDecimal underlyingSharesOutstanding) {
		this.underlyingSharesOutstanding = underlyingSharesOutstanding;
	}
	/**
	 * @return the percentGrossExposureUnderlyingGlobal
	 */
	public double getPercentGrossExposureUnderlyingGlobal() {
		return percentGrossExposureUnderlyingGlobal;
	}
	/**
	 * @param percentGrossExposureUnderlyingGlobal the percentGrossExposureUnderlyingGlobal to set
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
	 * @param grossExposureUnderlying the grossExposureUnderlying to set
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
	 * @param percentGrossExposureUnderlyingMasterFund the percentGrossExposureUnderlyingMasterFund to set
	 */
	public void setPercentGrossExposureUnderlyingMasterFund(
			double percentGrossExposureUnderlyingMasterFund) {
		this.percentGrossExposureUnderlyingMasterFund = percentGrossExposureUnderlyingMasterFund;
	}
	/**
	 * @return the percentGrossExposureUnderlyingAccount
	 */
	public double getPercentGrossExposureUnderlyingAccount() {
		return percentGrossExposureUnderlyingAccount;
	}
	/**
	 * @param percentGrossExposureUnderlyingAccount the percentGrossExposureUnderlyingAccount to set
	 */
	public void setPercentGrossExposureUnderlyingAccount(
			double percentGrossExposureUnderlyingAccount) {
		this.percentGrossExposureUnderlyingAccount = percentGrossExposureUnderlyingAccount;
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
	 * @return the accountNRA
	 */
	public double getAccountNRA() {
		return accountNRA;
	}
	/**
	 * @param accountNRA the accountNRA to set
	 */
	public void setAccountNRA(double accountNRA) {
		this.accountNRA = accountNRA;
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
	 * @return the dayReturnAccount
	 */
	public double getDayReturnAccount() {
		return dayReturnAccount;
	}
	/**
	 * @param dayReturnAccount the dayReturnAccount to set
	 */
	public void setDayReturnAccount(double dayReturnAccount) {
		this.dayReturnAccount = dayReturnAccount;
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
	 * @return the percentBetaAdjustedGrossExposureUnderlyingBaseAccount
	 */
	public double getPercentBetaAdjustedGrossExposureUnderlyingBaseAccount() {
		return percentBetaAdjustedGrossExposureUnderlyingBaseAccount;
	}
	/**
	 * @param percentBetaAdjustedGrossExposureUnderlyingBaseAccount the percentBetaAdjustedGrossExposureUnderlyingBaseAccount to set
	 */
	public void setPercentBetaAdjustedGrossExposureUnderlyingBaseAccount(
			double percentBetaAdjustedGrossExposureUnderlyingBaseAccount) {
		this.percentBetaAdjustedGrossExposureUnderlyingBaseAccount = percentBetaAdjustedGrossExposureUnderlyingBaseAccount;
	}
	/**
	 * @return the percentBetaAdjustedLongExposureUnderlyingBaseAccount
	 */
	public double getPercentBetaAdjustedLongExposureUnderlyingBaseAccount() {
		return percentBetaAdjustedLongExposureUnderlyingBaseAccount;
	}
	/**
	 * @param percentBetaAdjustedLongExposureUnderlyingBaseAccount the percentBetaAdjustedLongExposureUnderlyingBaseAccount to set
	 */
	public void setPercentBetaAdjustedLongExposureUnderlyingBaseAccount(
			double percentBetaAdjustedLongExposureUnderlyingBaseAccount) {
		this.percentBetaAdjustedLongExposureUnderlyingBaseAccount = percentBetaAdjustedLongExposureUnderlyingBaseAccount;
	}
	/**
	 * @return the percentBetaAdjustedShortExposureUnderlyingBaseAccount
	 */
	public double getPercentBetaAdjustedShortExposureUnderlyingBaseAccount() {
		return percentBetaAdjustedShortExposureUnderlyingBaseAccount;
	}
	/**
	 * @param percentBetaAdjustedShortExposureUnderlyingBaseAccount the percentBetaAdjustedShortExposureUnderlyingBaseAccount to set
	 */
	public void setPercentBetaAdjustedShortExposureUnderlyingBaseAccount(
			double percentBetaAdjustedShortExposureUnderlyingBaseAccount) {
		this.percentBetaAdjustedShortExposureUnderlyingBaseAccount = percentBetaAdjustedShortExposureUnderlyingBaseAccount;
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
	 * @return the deltaAdjPosition
	 */
	public double getDeltaAdjPosition() {
		return deltaAdjPosition;
	}
	/**
	 * @param deltaAdjPosition the deltaAdjPosition to set
	 */
	public void setDeltaAdjPosition(double deltaAdjPosition) {
		this.deltaAdjPosition = deltaAdjPosition;
	}
}