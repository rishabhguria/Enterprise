-- =============================================  
-- Author:  <Harsh Kumar>  
-- Create date: <24/07/08>  
-- Description: <Get Swap Params for Groups>  
-- =============================================  
CREATE PROCEDURE P_GetSwapParamsForGroups   
 @auecDateString varchar(max)  
AS  
BEGIN  
  
Declare @AUECDatesTable Table(AUECID int,CurrentAUECDate DateTime)                                                        
                                            
Insert Into @AUECDatesTable Select * From dbo.GetAllAUECDatesFromString(@auecDateString)  
  
select   
G.GroupID,  
Swap.NotionalValue,  
Swap.BenchMarkRate,  
Swap.Differential,  
Swap.OrigCostBasis,  
Swap.DayCount,  
Swap.SwapDescription,  
Swap.FirstResetDate,  
Swap.OrigTransDate,  
Swap.ClosingPrice,  
Swap.ClosingDate,  
Swap.TransDate,  
G.AUECID  
  
from T_Group G   
  inner join @AUECDatesTable As AUECDates on AUECDates.AUECID = G.AUECID   
right outer join T_SwapParameters Swap on G.GroupID = Swap.GroupID             
              
where (G.StateID=1 and DATEDIFF(d,G.AUECLocalDate,AUECDates.CurrentAUECDate)>=0 )  or (G.StateID=2 and DATEDIFF(d,G.AUECLocalDate,AUECDates.CurrentAUECDate)=0)                       
  
   
END  