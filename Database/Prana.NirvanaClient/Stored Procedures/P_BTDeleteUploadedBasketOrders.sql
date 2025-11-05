CREATE procedure P_BTDeleteUploadedBasketOrders
(
@clientOrderID varchar(50)
)
as

delete  from T_BTUploadedBasketOrders where ClientOrderID=@clientOrderID