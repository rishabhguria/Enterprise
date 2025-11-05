
CREATE proc [dbo].[P_BTGetUpLoadedBasketOrders]
(
	@basketID varchar(50)
)
as 
select ClientOrderID,ParentClientOrderID,SideTagValue,Symbol,
Quantity,AvgPrice,TradingAccountID,
AUECID,AssetID,UnderlyingID,CompanyUserID,
Price,ExecutionInst,OrderTypeTagValue,TimeInForce,CounterPartyID,VenueID,'',FundID,StrategyID -- for WaveID
from T_BTUpLoadedBasketOrders 
where BasketID = @basketID
order by ClientOrderID
