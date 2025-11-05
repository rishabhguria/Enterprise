package prana.esperCalculator.objects;
import java.util.Date;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotState;
import prana.esperCalculator.commonCode.TaxlotManager.TaxlotType;

public class Taxlot {
    public String basketId;
    public String taxlotId;
    public String clOrderId;
    public TaxlotType taxlotType;
    public TaxlotState taxlotState;
    public String symbol;
    public String underlyingSymbol;
    public double quantity;
    public double avgPrice;
    public String orderSideTagValue;
    public int accountId;
    public int counterPartyId;
    public int venueId;
    public Date auecLocalDate;
    public Date settlementDate;
    public String userId;
    public int strategyId;
    public String orderTypeTagValue;
    public double benchMarkRate;
    public double differential;
    public double swapNotional;
    public double dayCount;
    public boolean isSwapped;
    public double avgFxRateForTrade;
    public String conversionMethodOperator;
    public String tif;
    public String orderSide;
    public String counterParty;
    public String venue;
    public int sideMultiplier;
    public String orderType;
    public String underlyingAsset;
    public double limitPrice;
    public double stopPrice;
    public boolean isWhatIfTradeStreamRequired;
    public int assetId;
    public String asset;
    public int auecId;
    public double multiplier;
    public String tradeAttribute1;
    public String tradeAttribute2;
    public String tradeAttribute3;
    public String tradeAttribute4;
    public String tradeAttribute5;
    public String tradeAttribute6;
    
    @Override
	public String toString() {
		return "Taxlot{" + "basketId='" + basketId + '\'' + ", taxlotId='" + taxlotId + '\'' + ", clOrderId='"
				+ clOrderId + '\'' + ", taxlotType='" + taxlotType + '\'' + ", taxlotState='" + taxlotState + '\''
				+ ", symbol='" + symbol + '\'' + ", underlyingSymbol='" + underlyingSymbol + '\'' + ", quantity="
				+ quantity + ", avgPrice=" + avgPrice + ", orderSideTagValue='" + orderSideTagValue + '\''
				+ ", accountId=" + accountId + ", counterPartyId=" + counterPartyId + ", venueId=" + venueId
				+ ", auecLocalDate=" + auecLocalDate + ", settlementDate=" + settlementDate + ", userId='" + userId
				+ '\'' + ", strategyId=" + strategyId + ", orderTypeTagValue='" + orderTypeTagValue + '\''
				+ ", benchMarkRate=" + benchMarkRate + ", differential=" + differential + ", swapNotional="
				+ swapNotional + ", dayCount=" + dayCount + ", isSwapped=" + isSwapped + ", avgFxRateForTrade="
				+ avgFxRateForTrade + ", conversionMethodOperator='" + conversionMethodOperator + '\'' + ", tif='" + tif
				+ '\'' + ", orderSide='" + orderSide + '\'' + ", counterParty='" + counterParty + '\'' + ", venue='"
				+ venue + '\'' + ", sideMultiplier=" + sideMultiplier + ", orderType='" + orderType + '\''
				+ ", underlyingAsset='" + underlyingAsset + '\'' + ", limitPrice=" + limitPrice + ", stopPrice="
				+ stopPrice + ", isWhatIfTradeStreamRequired=" + isWhatIfTradeStreamRequired + ", assetId=" + assetId
				+ ", asset='" + asset + '\'' + ", auecId=" + auecId + ", multiplier=" + multiplier
				+ ", tradeAttribute1='" + tradeAttribute1 + '\'' + ", tradeAttribute2='" + tradeAttribute2 + '\''
				+ ", tradeAttribute3='" + tradeAttribute3 + '\'' + ", tradeAttribute4='" + tradeAttribute4 + '\''
				+ ", tradeAttribute5='" + tradeAttribute5 + '\'' + ", tradeAttribute6='" + tradeAttribute6 + '\'' + '}';
	}
}