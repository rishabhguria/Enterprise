
------------------------------------------------------------

CREATE proc PM_SaveDailyBeta    
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
                         
 CREATE TABLE #TempBeta                                                                             
 (                                                                               
  Symbol varchar(100)                           
  ,Date datetime                      
  ,Beta float      
 )                                                                              
                                                                              
 INSERT INTO #TempBeta                        
 (                                                                   
    Symbol                      
   ,Date                      
   ,Beta      
  )                                                                              
 SELECT                                                                              
    Symbol                      
   ,Date                      
   ,Beta       
                       
 FROM OPENXML(@handle, '//DailyBeta', 2)                                                                                 
  WITH                                                                               
  (                                                         
   Symbol varchar(100)                           
  ,Date datetime                      
  ,Beta float       
  )  As B where   B.Beta is not null                         
              
DELETE PM_DailyBeta from PM_DailyBeta              
inner join  #TempBeta on DateDiff(d,Convert(VARCHAR(10), #TempBeta.Date, 110),PM_DailyBeta.Date)=0             
and #TempBeta.Symbol = PM_DailyBeta.Symbol               
            
                        
 INSERT INTO     
                      
    PM_DailyBeta                        
 (                        
    Symbol                      
   ,Date                      
   ,Beta                         
 )                   
  SELECT                         
    Symbol                      
   ,Date                      
   ,Beta       
   FROM                         
    #TempBeta               
                        
 DROP TABLE #TempBeta             
                    
 EXEC sp_xml_removedocument @handle                        
                         
COMMIT TRANSACTION TRAN1                        
                         
END TRY                        
 BEGIN CATCH                         
  SET @ErrorMessage = ERROR_MESSAGE();                        
  print @errormessage                        
  SET @ErrorNumber = Error_number();                         
  ROLLBACK TRANSACTION TRAN1                         
END CATCH;   
  
  
--select * from PM_DailyBeta  

