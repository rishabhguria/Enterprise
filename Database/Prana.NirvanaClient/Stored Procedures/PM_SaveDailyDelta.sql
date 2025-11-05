CREATE proc PM_SaveDailyDelta    
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
                         
 CREATE TABLE #TempDelta                                                                         
 (                                                                               
  Symbol varchar(100)                           
  ,Date datetime                      
  ,Delta   float      
 )                                                                              
                                                                              
 INSERT INTO #TempDelta                        
 (                                                                   
    Symbol                      
   ,Date                      
   ,Delta        
  )                                                                              
 SELECT                                                                              
    Symbol                      
   ,Date                      
   ,Delta         
                       
 FROM OPENXML(@handle, '//DailyDelta', 2)                                                                                 
  WITH                                                                               
  (                                                         
   Symbol varchar(100)                           
  ,Date datetime                      
  ,Delta   float       
  ) As D where D.Delta is not null             


DELETE PM_DailyDelta from PM_DailyDelta              
inner join  #TempDelta on DateDiff(d,Convert(VARCHAR(10), #TempDelta.Date, 110),PM_DailyDelta.Date)=0             
and #TempDelta.Symbol = PM_DailyDelta.Symbol                
            
                        
 INSERT INTO     
                      
    PM_DailyDelta                        
 (                        
    Symbol                      
   ,Date                      
   ,Delta                         
 )                   
  SELECT                         
    Symbol                      
   ,Date                      
   ,Delta       
   FROM                         
    #TempDelta               
                        
 DROP TABLE #TempDelta             
                    
 EXEC sp_xml_removedocument @handle                        
                         
COMMIT TRANSACTION TRAN1                        
                         
END TRY                        
 BEGIN CATCH                         
  SET @ErrorMessage = ERROR_MESSAGE();                        
  print @errormessage                        
  SET @ErrorNumber = Error_number();                         
  ROLLBACK TRANSACTION TRAN1                         
END CATCH; 