create Procedure P_BTDeleteAllocatedStrategy
( 
@groupID varchar(50)
)
as
delete from T_BTStrategyAllocation where GroupID=@groupID