CREATE PROC [dbo].[P_IfJournalMappingInUse]  
(  
@activityTypeID int  
)  
  
as   
  
IF  EXISTS(SELECT 1 FROM T_AllActivity WHERE ActivityTypeId_FK = @activityTypeID)    
BEGIN  
select 1  
END  

