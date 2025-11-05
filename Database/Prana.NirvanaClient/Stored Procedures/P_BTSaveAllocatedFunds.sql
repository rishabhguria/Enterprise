

CREATE  procedure [dbo].[P_BTSaveAllocatedFunds]
(
@groupID varchar(50),
@fundID int,
@allocatedQty float,
@percentage float 
)
as

insert into  T_BTFundAllocation(GroupID,FundID,AllocatedQty,Percentage) 
values(@groupID,@fundID,@allocatedQty,@percentage)
