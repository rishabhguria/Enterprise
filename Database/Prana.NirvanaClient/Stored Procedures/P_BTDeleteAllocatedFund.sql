create Procedure P_BTDeleteAllocatedFund
( 
@groupID varchar(50)
)
as
delete from T_BTFundAllocation where GroupID=@groupID
