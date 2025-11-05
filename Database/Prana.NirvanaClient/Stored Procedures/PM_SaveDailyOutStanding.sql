CREATE proc PM_SaveDailyOutStanding    
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
                         
 CREATE TABLE #TempOutStandings                                                                       
 (                                                                               
  Symbol varchar(100)                           
  ,Date datetime                      
  ,OutStandings   float      
 )                                                                              
                                                                              
 INSERT INTO #TempOutStandings                        
 (                                                                   
    Symbol                      
   ,Date                      
   ,OutStandings        
  )                                                                              
 SELECT                                                                              
    Symbol                      
   ,Date                      
   ,OutStandings         
                       
 FROM OPENXML(@handle, '//DailyOutStandings', 2)                                                                                 
  WITH                                                                               
  (                                                         
   Symbol varchar(100)                           
  ,Date datetime                      
  ,OutStandings   float       
  )As OS where OS.OutStandings is not null                      
              
DELETE PM_DailyOutStandings from PM_DailyOutStandings              
inner join  #TempOutStandings on DateDiff(d,Convert(VARCHAR(10), #TempOutStandings.Date, 110),PM_DailyOutStandings.Date)=0             
and #TempOutStandings.Symbol = PM_DailyOutStandings.Symbol             
            
                        
 INSERT INTO     
                      
    PM_DailyOutStandings                        
 (                        
    Symbol                      
   ,Date                      
   ,outStandings                         
 )                   
  SELECT                         
    Symbol                      
   ,Date                      
   ,outStandings       
   FROM                         
    #TempOutStandings               
                        
 DROP TABLE #TempOutStandings             
                    
 EXEC sp_xml_removedocument @handle                        
                         
COMMIT TRANSACTION TRAN1                        
                         
END TRY                        
 BEGIN CATCH                         
  SET @ErrorMessage = ERROR_MESSAGE();                        
  print @errormessage                        
  SET @ErrorNumber = Error_number();                         
  ROLLBACK TRANSACTION TRAN1                         
END CATCH; 