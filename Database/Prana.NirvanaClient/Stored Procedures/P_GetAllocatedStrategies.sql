

CREATE Procedure [dbo].[P_GetAllocatedStrategies] (    
@AllAUECDatesString varchar(8000)     
    
)    
as    
    
select SA.GroupID,SA.StrategyID,SA.AllocatedQty,SA.Percentage,SA.AllocationID       
 from T_StrategyAllocation as SA    
join T_Group as G on SA.GroupID=G.GroupID  
inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID    
inner join GetAllAUECDatesFromString(@AllAUECDatesString) As AUECDates on AUECDates.AUECID = G.AUECID     
where     
 DATEDIFF(d,G.AUECLocalDate,AUECDates.CurrentAUECDate) = 0    
set ANSI_NULLS ON    
set QUOTED_IDENTIFIER ON    
--------------------------------------------------------------
