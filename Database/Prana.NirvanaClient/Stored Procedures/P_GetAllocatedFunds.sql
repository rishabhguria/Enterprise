

--------------------------------------------------------------------------------      
      
      
CREATE Procedure [dbo].[P_GetAllocatedFunds] (      
@AllAUECDatesString varchar(8000)      
      
)      
as      
      
select       
         
 FA.GroupID,      
 FA.FundID,      
 FA.AllocatedQty,      
 FA.Percentage,          
 FA.AllocationID,
FAC.Commission,
FAC.Fees      
from       
 T_FundAllocation  as FA  
left outer join T_FundAllocationCommission FAC on FAC.AllocationID_Fk= FA.AllocationID    
join T_Group as G on FA.GroupID=G.GroupID    
inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID      
inner join GetAllAUECDatesFromString(@AllAUECDatesString) As AUECDates on AUECDates.AUECID = G.AUECID     
where       
 DATEDIFF(d,G.AUECLocalDate,AUECDates.CurrentAUECDate) = 0      
      
      
      
      
set ANSI_NULLS ON      
set QUOTED_IDENTIFIER ON      
-------------------------------------------------------------------------------      
set ANSI_NULLS ON      
set QUOTED_IDENTIFIER ON
