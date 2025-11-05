
CREATE proc [dbo].[P_BTGetUpLoadedBasketWaveOrders]
(
	@basketID varchar(50)
)
as 
select ClientOrderID,ParentClientOrderID,SideTagValue,Symbol,
Quantity,AvgPrice,TradingAccountID,
AUECID,AssetID,UnderlyingID,CompanyUserID,
Price,ExecutionInst,OrderTypeTagValue,TimeInForce,CounterPartyID,VenueID,WO.WaveID,FundID,StrategyID
from T_BTWaveOrders as WO join T_BTWave as W
on WO.WaveID=W.WaveID
where W.BasketID = @basketID
order by ParentClientOrderID



--select *  from T_BTWave
