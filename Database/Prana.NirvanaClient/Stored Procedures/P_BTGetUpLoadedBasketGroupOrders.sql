
CREATE proc [dbo].[P_BTGetUpLoadedBasketGroupOrders]
(
	@basketID varchar(50)
)
as 
select ClientOrderID,ParentClientOrderID,SideTagValue,Symbol,
Quantity,AvgPrice,TradingAccountID,
AUECID,AssetID,UnderlyingID,CompanyUserID,
Price,ExecutionInst,OrderTypeTagValue,TimeInForce,CounterPartyID,VenueID,'',FundID,StrategyID --For WaveID
from T_BTGroupOrders 
where BasketID = @basketID
order by ParentClientOrderID
