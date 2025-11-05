CREATE PROCEDURE [dbo].[P_DeleteSelectedCorporateAction]           
(                                          
  @caIDs  varchar(max)          
 ,@ErrorMessage varchar(500) output                                                                                       
 ,@ErrorNumber int output                                                          
)                                            
as                                          
     
    
Delete From T_SMCorporateActions      
 where CorpActionID in (Select Items from dbo.Split(@caIDs,',') )

