
CREATE procedure [dbo].[P_BTUpdateUploadedBasketOrder] (
@savedOrderID varchar(50),
@sideTagValue char(10),
@symbol varchar(50),
@quantity float,
@price float,
@orderTypeTagValue char(10),
@counterpartyID int,
@venueID int,
@fundID int,
@strategyID int
)
as

update T_BTUpLoadedBasketOrders 
set Quantity=@quantity,
sideTagValue=@sideTagValue,
Symbol=@symbol,
Price=@price,
OrderTypeTagValue=@orderTypeTagValue,
CounterpartyID =@counterpartyID ,
VenueID=@venueID,
FundID=@fundID,
StrategyID=@strategyID

where ClientOrderID=@savedOrderID
