

CREATE Procedure [dbo].[P_BTGetAllocatedStrategies] (  
 @AllAUECDatesString varchar(8000)   
  
)  
as  
  
select SA.GroupID,SA.StrategyID,SA.AllocatedQty,SA.Percentage,SA.AllocationID     
 from T_BTStrategyAllocation as SA  
join BT_BasketGroups as G on SA.GroupID=G.GroupID  
inner join T_AUEC AUEC on AUEC.AUECID = G.AUECID    
inner join GetAllAUECDatesFromString(@AllAUECDatesString) As AUECDates on AUECDates.AUECID = G.AUECID                                                      
    
where   
 DATEDIFF(d,G.AddedDate,AUECDates.CurrentAUECDate) = 0  
set ANSI_NULLS ON  
set QUOTED_IDENTIFIER ON
