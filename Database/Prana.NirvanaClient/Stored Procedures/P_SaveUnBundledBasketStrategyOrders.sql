

CREATE procedure [dbo].[P_SaveUnBundledBasketStrategyOrders]
(
@listID varchar(50),
@clOrderID varchar(50),
@allocationStateID int
)
as
insert into T_BTUnBundledBasketStrategyOreders(BasketID,ClOrderID,AllocationStateID) values (@listID,@clOrderID,@allocationStateID)



