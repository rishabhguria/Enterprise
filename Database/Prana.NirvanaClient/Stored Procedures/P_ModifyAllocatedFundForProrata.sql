

CREATE Procedure [dbo].[P_ModifyAllocatedFundForProrata] (    
@GroupID varchar(50),    
@FundID int,    
@AllocatedQty float ,
@Commission float ,
@Fees float    
)    
as    
    
Update T_FundAllocation     
Set AllocatedQty=@AllocatedQty    
Where GroupID=@GroupID    
And FundID=@FundID 

if( @Commission is not null)

Update 
T_FundAllocationCommission 

Set Commission = @Commission,
	Fees = @Fees
FROM T_FundAllocationCommission join T_FundAllocation 
on T_FundAllocation.AllocationID = T_FundAllocationCommission.AllocationID_Fk
where T_FundAllocation.GroupID = @GroupID and FundID = @FundID
