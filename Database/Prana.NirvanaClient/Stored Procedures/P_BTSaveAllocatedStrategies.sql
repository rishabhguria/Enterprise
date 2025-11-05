

CREATE  procedure [dbo].[P_BTSaveAllocatedStrategies]
(
@groupID varchar(50),
@strategyID int,
@allocatedQty float,
@percentage float 
)
as

insert into  T_BTStrategyAllocation(GroupID,StrategyID,AllocatedQty,Percentage) 
values(@groupID,@strategyID,@allocatedQty,@percentage)

--select * from T_StrategyAllocation