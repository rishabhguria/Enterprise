

CREATE procedure [dbo].[P_ChangeListOrdersState] (
@clOrderID varchar(50),
@stateID int,
@typeOfAllocation int
)
as
if(@typeOfAllocation =0) -- Fund

update T_BTUnBundledBasketFundOrders
set AllocationStateID=@stateID
where ClOrderID=@clOrderID
else              --Strategy
update T_BTUnBundledBasketStrategyOreders
set AllocationStateID=@stateID
where ClOrderID=@clOrderID
