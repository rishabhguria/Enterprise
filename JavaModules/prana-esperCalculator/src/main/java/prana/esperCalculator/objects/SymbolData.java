package prana.esperCalculator.objects;


import java.math.BigDecimal;

import prana.esperCalculator.constants.CollectorConstants;

public class SymbolData {	
	public double delta = 1;
	public String conversionMethod = CollectorConstants.CONVERSION_METHOD_OPERATOR_MULTIPLY;
	public double beta5yr = 1;
	public double selectedfeedPX = 0.0;
	public double ask = 0.0;
	public double bid = 0.0;
	public double low = 0.0;
	public double high = 0.0;
	public double open = 0.0;
	public double closingPrice = 0.0;
	public Object lastPrice = 0.0;
	public Object markPrice = 0.0;
	public BigDecimal sharesOutStandings = BigDecimal.ZERO;
	public String symbol = "";
	public String underlyingSymbol = "";
	public int categoryCode = 0;
	public double openInterest = 0.0;
	public double avgVol20Days = 0.0;	
	// 0 = None, 1 = Real-Time, 2 = Delayed
	public int pricingStatus = 0;
}