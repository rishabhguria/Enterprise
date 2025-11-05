
CREATE procedure [dbo].[BT_UpdateUploadedBaskets]
(
@basketID varchar(50),
@tradedBasketID varchar(50),
@haswaves varchar(5)

)
as

update  T_BTUploadedBaskets 
set 
BasketID=@basketID ,
TradedBasketID=@tradedBasketID,
HasWaves=@haswaves,
IsSaved='True'
where BasketID=@basketID
