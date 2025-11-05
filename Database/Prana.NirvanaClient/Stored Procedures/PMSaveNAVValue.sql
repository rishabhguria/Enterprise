    
    
/****************************************************************************          
Name :   [PMSaveNAVValue]                       
Date Created: 14-May-2008                         
Purpose:  Save NAV values DateWise.           
Module: MarkPriceAndForexConversion/PM                   
Author: Sandeep Singh         
          
delete from PM_NAVValue   
Select * from PM_NAVValue   
Execution Statement           
[PMSaveNAVValue]            
'<NewDataSet>  
  <Table1>  
    <Date>6/12/2008 12:00:00 AM</Date> 
<FundID>2</FundID>  
    <NAVValue>0</NAVValue>
  </Table1>  
</NewDataSet>','',0   
 ****************************************************************************/          
 CREATE Proc [dbo].[PMSaveNAVValue]                        
 (                        
   @Xml nText                        
 , @ErrorMessage varchar(500) output                        
 , @ErrorNumber int output                        
 )                        
AS                         
                        
SET @ErrorMessage = 'Success'                        
SET @ErrorNumber = 0                        
BEGIN TRAN TRAN1                         
                        
BEGIN TRY                        
                        
DECLARE @handle int                           
exec sp_xml_preparedocument @handle OUTPUT,@Xml                           
                        
  CREATE TABLE #TempNAVValue                                                                               
  (                          
   Date datetime                      
  ,NAVValue float,
FundID int    
   )                                                                        
INSERT INTO #TempNAVValue                        
 (                                                                              
    Date                            
   ,NAVValue,
FundID              
 )                                                                              
SELECT                                                                               
  Date                      
 ,NAVValue,
FundID                  
    FROM OPENXML(@handle, '//Table1', 2)                                                                                 
 WITH                                                                               
 (                                                         
 Date datetime                        
,NAVValue float,
FundID int              
 )         
        
DELETE  PM_NAVValue          
  WHERE           
DATEADD(day, DATEDIFF(day, 0, PM_NAVValue.Date), 0) in   
(select DATEADD(day, DATEDIFF(day, 0, Convert(datetime, #TempNAVValue.Date, 113)), 0) from  #TempNAVValue)  
         
 INSERT INTO         
 PM_NAVValue          
   (     
  NAVValue,          
  Date,
FundID          
    )      
  SELECT   
  NAVValue,  
  Date,
FundID               
  FROM           
   #TempNAVValue     
          
          
DROP TABLE #TempNAVValue           
          
          
EXEC sp_xml_removedocument @handle          
          
COMMIT TRANSACTION TRAN1          
          
END TRY          
BEGIN CATCH           
 SET @ErrorMessage = ERROR_MESSAGE();          
print @errormessage          
 SET @ErrorNumber = Error_number();           
 ROLLBACK TRANSACTION TRAN1           
END CATCH; 