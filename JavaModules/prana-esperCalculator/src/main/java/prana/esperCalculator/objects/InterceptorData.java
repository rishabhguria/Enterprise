package prana.esperCalculator.objects;

import java.math.BigDecimal;

import com.opencsv.bean.CsvBindByName;

public class InterceptorData {
	@CsvBindByName(column = "symbol", required = true)
	private String symbol;
	public String getSymbol() {
		return symbol;
	}

	@CsvBindByName(column = "underlyingSymbol")
	private String underlyingSymbol;
	public String getUnderlyingSymbol() {
		return underlyingSymbol;
	}

	@CsvBindByName(column = "askPrice")
	private double askPrice;
	public double getAskPrice() {
		return askPrice;
	}

	@CsvBindByName(column = "bidPrice")
	private double bidPrice;
	public double getBidPrice() {
		return bidPrice;
	}

	@CsvBindByName(column = "lowPrice")
	private double lowPrice;
	public double getLowPrice() {
		return lowPrice;
	}

	@CsvBindByName(column = "highPrice")
	private double highPrice;
	public double getHighPrice() {
		return highPrice;
	}

	@CsvBindByName(column = "openPrice")
	private double openPrice;
	public double getOpenPrice() {
		return openPrice;
	}

	@CsvBindByName(column = "closePrice")
	private double closePrice;
	public double getClosePrice() {
		return closePrice;
	}

	@CsvBindByName(column = "lastPrice")
	private double lastPrice;
	public double getLastPrice() {
		return lastPrice;
	}

	@CsvBindByName(column = "selectedFeedPrice")
	private double selectedFeedPrice;
	public double getSelectedFeedPrice() {
		return selectedFeedPrice;
	}

	@CsvBindByName(column = "conversionMethod")
	private String conversionMethod;
	public String getConversionMethod() {
		return conversionMethod;
	}

	@CsvBindByName(column = "markPrice")
	private double markPrice;
	public double getMarkPrice() {
		return markPrice;
	}

	@CsvBindByName(column = "delta")
	private double delta;
	public double getDelta() {
		return delta;
	}

	@CsvBindByName(column = "beta5YearMonthly")
	private double beta5YearMonthly;
	public double getBeta5YearMonthly() {
		return beta5YearMonthly;
	}

	@CsvBindByName(column = "assetId")
	private int assetId;
	public int getAssetId() {
		return assetId;
	}

	@CsvBindByName(column = "openInterest")
	private double openInterest;
	public double getOpenInterest() {
		return openInterest;
	}

	@CsvBindByName(column = "avgVolume20Days")
	private double avgVolume20Days;
	public double getAvgVolume20Days() {
		return avgVolume20Days;
	}

	@CsvBindByName(column = "sharesOutstanding")
	private BigDecimal sharesOutstanding;
	public BigDecimal getSharesOutstanding() {
		return sharesOutstanding;
	}
}