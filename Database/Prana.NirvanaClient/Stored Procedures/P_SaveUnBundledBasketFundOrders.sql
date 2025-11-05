
CREATE procedure [dbo].[P_SaveUnBundledBasketFundOrders]
(
@listID varchar(50),
@clOrderID varchar(50),
@allocationStateID int
)
as

insert into T_BTUnBundledBasketFundOrders(BasketID,ClOrderID,AllocationStateID) values (@listID,@clOrderID,@allocationStateID)



