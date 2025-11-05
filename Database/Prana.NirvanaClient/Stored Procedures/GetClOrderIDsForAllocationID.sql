/****************************************************************************
Name :   GetClOrderIDsForAllocationID
Date Created: 28-Dec-2006 
Purpose:  Get all the ClOrderIDs for specified AllocationID
Author: Ram Shankar Yadav
Execution Statement : exec GetClOrderIDsForAllocationID 2216

Date Modified: <DateModified> 
Description:     <DescriptionOfChange> 
Modified By:     <ModifiedBy> 
****************************************************************************/

create proc GetClOrderIDsForAllocationID
(
	@AllocationID int
)

as
	
select GO.ClOrderID 
from 
	T_FundAllocation  as FA,
	T_GroupOrder as GO 
where
	FA.GroupID=GO.GroupID and
	FA.AllocationID = @AllocationID