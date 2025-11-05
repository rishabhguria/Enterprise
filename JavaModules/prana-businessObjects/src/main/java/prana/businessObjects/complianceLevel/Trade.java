package prana.businessObjects.complianceLevel;

public class Trade {

	private int userId;

	private double quantity;
	private double netMarketValueLocal;

	private String symbol;
	private String underlyingSymbol;
	private String accountShortName;
	private String accountLongName;
	private int masterStrategyId;
	private String masterStrategyName;
	private int strategyId;
	private String strategyName;
	private String strategyFullName;
	private String exchange;
	private String currency;
	private double grossMarketValue;

	private double notionalLocal;
	private double equityHaircutLocal;
	private double indexHaircutLocal;
	private double equityHaircutBase;
	private double indexHaircutBase;
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
	private double liquidationPnlBase;
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
	private String masterFundName;

	private String orderSide;
	private String orderType;
	private String asset;

	private String fxSymbol;
	private String sector;
	private String subsector;
	private String counterParty;
	private String venue;

	private double avgPrice;
	private double limitPrice;
	private double stopPrice;

	private double askPrice;
	private double bidPrice;
	private double lowPrice;
	private double highPrice;
	private double openPrice;
	private double closePrice;
	private double lastPrice;
	private double markPrice;
	private double delta;
	private double beta5YearMonthly;
	private double selectedFeedPrice;
	private double openInterest;
	private double avgVolume20Days;
	private double percentAvgVolume;
	private java.math.BigDecimal sharesOutstanding;
	private String putOrCall;
	private double strikePrice;
	
	private boolean isTodayTrade;
	private double todayAbsTradeQuantity;
	private double todayLongTradeQuantity;
	private double todayShortTradeQuantity;
	private double todayNetNotionalBase;
	private double todayLongNotionalBase;
	private double todayShortNotionalBase;	

	private boolean isSwapped;
	
	private String udaAsset;
	/* private String udaSymbol; */
	private String udaSecurityType;
	private String udaCountry;
	
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
	private double roundLot;
	private String tif;
	private String tradeAttribute1;
	private String tradeAttribute2;
	private String tradeAttribute3;
	private String tradeAttribute4;
	private String tradeAttribute5;
	private String tradeAttribute6;
	
	/**
	 * @return the roundLot
	 */
	public double getRoundLot() {
		return roundLot;
	}

	/**
	 * @param roundLot the roundLot to set
	 */
	public void setRoundLot(int roundLot) {
		this.roundLot = roundLot;
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
	 * @return the accountShortName
	 */
	public String getAccountShortName() {
		return accountShortName;
	}

	/**
	 * @param accountShortName
	 *            the accountShortName to set
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
	 * @param accountLongName
	 *            the accountLongName to set
	 */
	public void setAccountLongName(String accountLongName) {
		this.accountLongName = accountLongName;
	}

	/**
	 * @return the masterStrategyId
	 */
	public int getMasterStrategyId() {
		return masterStrategyId;
	}

	/**
	 * @param masterStrategyId
	 *            the masterStrategyId to set
	 */
	public void setMasterStrategyId(int masterStrategyId) {
		this.masterStrategyId = masterStrategyId;
	}

	/**
	 * @return the masterStrategyName
	 */
	public String getMasterStrategyName() {
		return masterStrategyName;
	}

	/**
	 * @param masterStrategyName
	 *            the masterStrategyName to set
	 */
	public void setMasterStrategyName(String masterStrategyName) {
		this.masterStrategyName = masterStrategyName;
	}

	/**
	 * @return the strategyId
	 */
	public int getStrategyId() {
		return strategyId;
	}

	/**
	 * @param strategyId
	 *            the strategyId to set
	 */
	public void setStrategyId(int strategyId) {
		this.strategyId = strategyId;
	}

	/**
	 * @return the strategyName
	 */
	public String getStrategyName() {
		return strategyName;
	}

	/**
	 * @param strategyName
	 *            the strategyName to set
	 */
	public void setStrategyName(String strategyName) {
		this.strategyName = strategyName;
	}

	/**
	 * @return the strategyFullName
	 */
	public String getStrategyFullName() {
		return strategyFullName;
	}

	/**
	 * @param strategyFullName
	 *            the strategyFullName to set
	 */
	public void setStrategyFullName(String strategyFullName) {
		this.strategyFullName = strategyFullName;
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
	 * @return the grossMarketValue
	 */
	public double getGrossMarketValue() {
		return grossMarketValue;
	}

	/**
	 * @param grossMarketValue
	 *            the grossMarketValue to set
	 */
	public void setGrossMarketValue(double grossMarketValue) {
		this.grossMarketValue = grossMarketValue;
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
	 * @return the orderSide
	 */
	public String getOrderSide() {
		return orderSide;
	}

	/**
	 * @param orderSide
	 *            the orderSide to set
	 */
	public void setOrderSide(String orderSide) {
		this.orderSide = orderSide;
	}

	/**
	 * @return the orderType
	 */
	public String getOrderType() {
		return orderType;
	}

	/**
	 * @param orderType
	 *            the orderType to set
	 */
	public void setOrderType(String orderType) {
		this.orderType = orderType;
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
	 * @return the fxSymbol
	 */
	public String getFxSymbol() {
		return fxSymbol;
	}

	/**
	 * @param fxSymbol
	 *            the fxSymbol to set
	 */
	public void setFxSymbol(String fxSymbol) {
		this.fxSymbol = fxSymbol;
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
	 * @return the subsector
	 */
	public String getSubsector() {
		return subsector;
	}

	/**
	 * @param subsector
	 *            the subsector to set
	 */
	public void setSubsector(String subsector) {
		this.subsector = subsector;
	}

	/**
	 * @return the counterParty
	 */
	public String getCounterParty() {
		return counterParty;
	}

	/**
	 * @param counterParty
	 *            the counterParty to set
	 */
	public void setCounterParty(String counterParty) {
		this.counterParty = counterParty;
	}

	/**
	 * @return the venue
	 */
	public String getVenue() {
		return venue;
	}

	/**
	 * @param venue
	 *            the venue to set
	 */
	public void setVenue(String venue) {
		this.venue = venue;
	}

	/**
	 * @return the avgPrice
	 */
	public double getAvgPrice() {
		return avgPrice;
	}
	
	/**
	 * @return the limitPrice
	 */
	public double getLimitPrice() {
		return limitPrice;
	}
	
	/**
	 * @return the stopPrice
	 */
	public double getStopPrice() {
		return stopPrice;
	}

	/**
	 * @param avgPrice
	 *            the avgPrice to set
	 */
	public void setAvgPrice(double avgPrice) {
		this.avgPrice = avgPrice;
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
	 * @return the closePrice
	 */
	public double getClosePrice() {
		return closePrice;
	}

	/**
	 * @param closePrice
	 *            the closePrice to set
	 */
	public void setClosePrice(double closePrice) {
		this.closePrice = closePrice;
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
	 * @return the isTodayTrade
	 */
	public boolean isTodayTrade() {
		return isTodayTrade;
	}

	/**
	 * @param isTodayTrade the isTodayTrade to set
	 */
	public void setTodayTrade(boolean isTodayTrade) {
		this.isTodayTrade = isTodayTrade;
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
	 * 
	 * @return the isSwapped status
	 */
	public boolean getIsSwapped() {
		return isSwapped;
	}

	/**
	 * 
	 * @param isSwapped the isSwapped to set
	 */
	public void setIsSwapped(boolean isSwapped) {
		this.isSwapped = isSwapped;
	}

	/**
	 * @return the udaAsset
	 */
	public String getUdaAsset() {
		return udaAsset;
	}

	/**
	 * @param udaAsset the udaAsset to set
	 */
	public void setUdaAsset(String udaAsset) {
		this.udaAsset = udaAsset;
	}

	/**
	 * @return the udaSecurityType
	 */
	public String getUdaSecurityType() {
		return udaSecurityType;
	}

	/**
	 * @param udaSecurityType the udaSecurityType to set
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
	 * @param customUDA7 the customUDA8 to set
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
	 * @param isSwapped the isSwapped to set
	 */
	public void setSwapped(boolean isSwapped) {
		this.isSwapped = isSwapped;
	}
	
	
	/**
	 * @return the tif
	 */
	public String getTif() {
		return tif;
	}

	/**
	 * @param tif
	 *            the tif to set
	 */
	public void setTif(String tif) {
		this.tif = tif;
	}
	
	/**
	 * @param tradeAttribute1
	 *            the tradeAttribute1 to set
	 */
	public void setTradeAttribute1(String tradeAttribute1) {
	    this.tradeAttribute1 = tradeAttribute1;
	}

	/**
	 * @return the tradeAttribute1
	 */
	public String getTradeAttribute1() {
	    return tradeAttribute1;
	}

	/**
	 * @param tradeAttribute2
	 *            the tradeAttribute2 to set
	 */
	public void setTradeAttribute2(String tradeAttribute2) {
	    this.tradeAttribute2 = tradeAttribute2;
	}

	/**
	 * @return the tradeAttribute2
	 */
	public String getTradeAttribute2() {
	    return tradeAttribute2;
	}

	/**
	 * @param tradeAttribute3
	 *            the tradeAttribute3 to set
	 */
	public void setTradeAttribute3(String tradeAttribute3) {
	    this.tradeAttribute3 = tradeAttribute3;
	}

	/**
	 * @return the tradeAttribute3
	 */
	public String getTradeAttribute3() {
	    return tradeAttribute3;
	}

	/**
	 * @param tradeAttribute4
	 *            the tradeAttribute4 to set
	 */
	public void setTradeAttribute4(String tradeAttribute4) {
	    this.tradeAttribute4 = tradeAttribute4;
	}

	/**
	 * @return the tradeAttribute4
	 */
	public String getTradeAttribute4() {
	    return tradeAttribute4;
	}

	/**
	 * @param tradeAttribute5
	 *            the tradeAttribute5 to set
	 */
	public void setTradeAttribute5(String tradeAttribute5) {
	    this.tradeAttribute5 = tradeAttribute5;
	}

	/**
	 * @return the tradeAttribute5
	 */
	public String getTradeAttribute5() {
	    return tradeAttribute5;
	}

	/**
	 * @param tradeAttribute6
	 *            the tradeAttribute6 to set
	 */
	public void setTradeAttribute6(String tradeAttribute6) {
	    this.tradeAttribute6 = tradeAttribute6;
	}

	/**
	 * @return the tradeAttribute6
	 */
	public String getTradeAttribute6() {
	    return tradeAttribute6;
	}
}