
CREATE Proc [dbo].[P_BTSaveUploadedBasketOrders]
(

@BasketID varchar(200),
@savedOrderID varchar(200),
@sideTagValue char(10),
@symbol varchar(50),
@exchangeID varchar(200),
@quantity float,
@averagePrice float,
@tradingAccountID int,
@aUECID int,
@assetID int,
@underlyingID int,
@companyUserID int,
@price float,
@executionInstruction varchar(50),
@orderTypeTagValue char(10),
@timeInForce char(10),
@counterpartyID int,
@venueID int,
@discretionOffset float,
@pegDifference varchar(50),
@stopPrice float,
@parentClientOrderID varchar(50),
@fundID int,
@strategyID int
)
as
--if((select count(*) from T_BTSavedBasketOrders where  SavedOrderID= @savedOrderID) =0)
insert 
T_BTUpLoadedBasketOrders(BasketID,ClientOrderID,SideTagValue,Symbol,
ExchangeID,Quantity,AvgPrice,TradingAccountID,AUECID,AssetID,UnderlyingID,
CompanyUserID,Price,ExecutionInst,OrderTypeTagValue,TimeInForce,
CounterPartyID,VenueID,DiscOffset,PegDiff,StopPrice,ParentClientOrderID,FundID,StrategyID) 
values(
@BasketID,
@savedOrderID,
@sideTagValue,
@symbol,
@exchangeID,
@quantity,
@averagePrice,
@tradingAccountID,
@aUECID,
@assetID,
@underlyingID,
@companyUserID,
@price,
@executionInstruction,
@orderTypeTagValue,
@timeInForce,
@counterpartyID,
@venueID,
@discretionOffset,
@pegDifference,
@stopPrice,
@parentClientOrderID,
@fundID,
@strategyID
)
