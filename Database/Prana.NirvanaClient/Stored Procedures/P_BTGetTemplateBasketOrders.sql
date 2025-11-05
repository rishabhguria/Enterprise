
CREATE proc [dbo].[P_BTGetTemplateBasketOrders]
(

@savedBasketID varchar(200)
)
as
select SavedOrderID,SavedOrderID,SideTagValue,Symbol,
Quantity,AvgPrice,TradingAccountID,
AUECID,AssetID,UnderlyingID,CompanyUserID,
Price,ExecutionInst,OrderTypeTagValue,TimeInForce,CounterPartyID,VenueID,'',FundID,StrategyID
from 
T_BTSavedBasketOrders
where SavedBasketID = @savedBasketID
