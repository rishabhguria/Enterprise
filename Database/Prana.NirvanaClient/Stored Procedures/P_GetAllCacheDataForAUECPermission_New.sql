CREATE procedure [dbo].[P_GetAllCacheDataForAUECPermission_New]          
(          
@companyUserID int,  
@companyID int             
)          
as        
Begin 

Select 'AUEC'+cast(V.AUECID as varchar(3))+':C'+cast(V.CounterPartyID as varchar(3))+':V'+cast(V.VenueID as varchar(5)) as KeyValue        
into #TempDataSet
from V_GetAllCVAUEC AS V where V.CompanyUserID=@companyUserID       
order by V.AUECID,V.CounterPartyID,V.VenueID      
  
select * from  #TempDataSet
exec P_GetAllAssets            
        
exec P_GetCompanyOrderTypes @companyID  
    
exec P_GetCompanyHandlingInstructions  @companyID    
    
exec P_GetCompanyExecutionInstructions  @companyID  
   
exec P_GetCompanyTimeInForces @companyID

End
drop table #TempDataSet