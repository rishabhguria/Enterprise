-- =============================================
-- Author:		Harsh Kumar	
-- Create date: 20 Mar 2009
-- Description:	Save and/or updates AUEC wise clearance times set from Exposure Clearance Screen
-- =============================================

CREATE procedure P_SaveandUpdatePMClearance  
(  
@xml  nText  
)                                                                  
as        
                                                                
DECLARE @handle int                                                                  
                                                                
                                                                 
exec sp_xml_preparedocument @handle OUTPUT,@xml                                                                
create table #Temp_ClearanceTimes  
(                                                                
AUECID int,                                                                
ClearanceTime DateTime,                                                                
)                                                                
                                                                
insert into #Temp_ClearanceTimes                                                                
(                                                                
 AUECID,                                                                
ClearanceTime                                                                
)                                                                 
select                                                                 
AUECID,                                                                
ClearanceTime                                                                
                                                                 
FROM  OPENXML(@handle, '/DocumentElement/ClearanceTable',2)    
WITH                                                                    
(                                                                        
AUECID  int ,                                                                
ClearanceTime DateTime                                                             
)         
  
Update T_PMClearanceTimes  
  
set   
ClearanceTime = #Temp_ClearanceTimes.ClearanceTime  
  
from #Temp_ClearanceTimes  
where #Temp_ClearanceTimes.AUECID = T_PMClearanceTimes.AUECID  
  
insert into T_PMClearanceTimes  
(  
AUECID,  
ClearanceTime  
)  
select   
  
AUECID,  
ClearanceTime  
  
from #Temp_ClearanceTimes  
where #Temp_ClearanceTimes.AUECID not in (select AUECID from T_PMClearanceTimes)  
        
drop table #Temp_ClearanceTimes                                                              
exec sp_xml_removedocument @handle                                                           