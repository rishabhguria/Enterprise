
CREATE Proc [dbo].[P_BTSavePortfolioBasketOrders]
(
@savedBasketID varchar(200),
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
@ParentClientOrderID  varchar(50),
@fundID int,
@strategyID int
)
as
insert 
T_BTSavedBasketOrders(SavedBasketID,SavedOrderID,SideTagValue,Symbol,
ExchangeID,Quantity,AvgPrice,TradingAccountID,AUECID,AssetID,UnderlyingID,
CompanyUserID,Price,ExecutionInst,OrderTypeTagValue,TimeInForce,
CounterPartyID,VenueID,DiscOffset,PegDiff,StopPrice,FundID,StrategyID) 
values(
@savedBasketID,
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
@fundID,
@strategyID
)
