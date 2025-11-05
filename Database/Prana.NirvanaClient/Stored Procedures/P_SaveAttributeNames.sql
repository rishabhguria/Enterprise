-- =============================================        
-- Author:  Divya Bansal       
-- Create date: 2 april 2013       
-- Description: Attribute names can be changed by user.     
-- =============================================        
CREATE PROCEDURE [dbo].[P_SaveAttributeNames]        
(    
 @Xml nText                      
 , @ErrorMessage varchar(500) output                      
 , @ErrorNumber int output                      
 )             
AS        
    
SET @ErrorMessage = 'Success'                      
SET @ErrorNumber = 0     
                     
                                 
BEGIN TRY     
BEGIN TRAN TRAN1       
    
DECLARE @handle int                         
exec sp_xml_preparedocument @handle OUTPUT,@Xml         
    
 CREATE TABLE #TempAttributeNames                                                                             
  (                                                                             
	AttributeValue varchar(max),    
	AttributeName varchar(max),
	KeepRecord BIT,
    DefaultValues varchar(max) 
   )      
    
INSERT INTO #TempAttributeNames                   
 (                                                                            
    AttributeValue                      
   ,AttributeName
   ,KeepRecord
   ,DefaultValues                                                    
 )                                                                            
SELECT                                                                             
  AttributeValue                      
   ,AttributeName
   ,KeepRecord
   ,DefaultValues                                    
    FROM OPENXML(@handle, '//Attributes', 2)                                                                               
 WITH                                                                             
 (                                                       
   AttributeValue varchar(max),    
AttributeName varchar(max),
	KeepRecord BIT,
    DefaultValues varchar(max)            
 )                        
    
Delete from T_AttributeNames    
    
Insert into T_AttributeNames(AttributeValue,AttributeName,KeepRecord,DefaultValues) 
select AttributeValue,AttributeName,KeepRecord,DefaultValues from #TempAttributeNames     
    
drop table #TempAttributeNames    
    
EXEC sp_xml_removedocument @handle                      
                      
COMMIT TRANSACTION TRAN1                      
                     
END TRY     
                   
BEGIN CATCH                       
SET @ErrorMessage = ERROR_MESSAGE();                      
--  print @errormessage                      
SET @ErrorNumber = Error_number();              
ROLLBACK TRANSACTION TRAN1                       
END CATCH;