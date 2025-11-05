CREATE proc PM_SaveDailyTradingVol    
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
                         
 CREATE TABLE #TempTradingVol                                                                          
 (                                                                               
  Symbol varchar(100)                           
  ,Date datetime                      
  ,TradingVolume Float  --BIGIN
 )                                                                              
                                                                              
 INSERT INTO #TempTradingVol                        
 (                                                                   
    Symbol                      
   ,Date                      
   ,TradingVolume        
  )                                                                              
 SELECT                                                                              
    Symbol                      
   ,Date                      
   ,TradingVolume         
                       
 FROM OPENXML(@handle, '//DailyTradingVol', 2)                                                                                 
  WITH                                                                               
  (                                                         
   Symbol varchar(100)                           
  ,Date datetime                      
  ,TradingVolume float  
  ) As TradingVol where   TradingVol.TradingVolume is not null                            
              
DELETE PM_DailyTradingVol from PM_DailyTradingVol              
inner join  #TempTradingVol on DateDiff(d,Convert(VARCHAR(10), #TempTradingVol.Date, 110),PM_DailyTradingVol.Date)=0             
and #TempTradingVol.Symbol = PM_DailyTradingVol.Symbol                
            
                        
 INSERT INTO     
                      
    PM_DailyTradingVol                        
 (                        
    Symbol                      
   ,Date                      
   ,TradingVolume                         
 )                   
  SELECT                         
    Symbol                      
   ,Date                      
   ,TradingVolume       
   FROM                         
    #TempTradingVol               
                        
 DROP TABLE #TempTradingVol             
                    
 EXEC sp_xml_removedocument @handle                        
                         
COMMIT TRANSACTION TRAN1                        
                         
END TRY                        
 BEGIN CATCH                         
  SET @ErrorMessage = ERROR_MESSAGE();                        
  print @errormessage                        
  SET @ErrorNumber = Error_number();                         
  ROLLBACK TRANSACTION TRAN1                         
END CATCH; 
