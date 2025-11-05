Create Proc [dbo].[P_SaveAuecRoundRules]    
(    
 @Xml varchar (max)    
)    
    
as     
    
DECLARE @handle int                                                                                 
exec sp_xml_preparedocument @handle OUTPUT,@Xml                
          
CREATE TABLE #Temp                                                                                           
(              
AUECID int,            
RoundOff int,           
IsApplied bit               
)           
          
INSERT INTO #Temp                
(                
AUECID,            
RoundOff,            
IsApplied         
)             
          
SELECT                     
AuecID,            
RoundOff,          
IsApplied      
                  
FROM  OPENXML(@handle, '//ArrayOfValidAUEC/ValidAUEC',3)       
      
WITH                                                                                                
(                       
AuecID int,            
RoundOff int,          
IsApplied bit       
)    
    
delete from T_AuecRoundOffRules    
    
Insert into  T_AuecRoundOffRules    
(    
AUECID,            
RoundOff,            
IsApplied         
)    
    
select     
AUECID,            
RoundOff,            
IsApplied         
From #Temp   
Where (#Temp.IsApplied =1 or #Temp.RoundOff !=0) 
    
Drop Table #Temp
exec sp_xml_removedocument @handle
