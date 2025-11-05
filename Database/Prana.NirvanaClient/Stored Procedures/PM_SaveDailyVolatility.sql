
CREATE proc PM_SaveDailyVolatility      
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
                           
 CREATE TABLE #TempVolatility                                                                               
 (                                                                                 
  Symbol varchar(100)                             
  ,Date datetime                        
  ,Volatility float        
 )                                                                                
                                                                                
 INSERT INTO #TempVolatility                          
 (                                                                     
    Symbol                        
   ,Date                        
   ,Volatility        
  )                                                                                
 SELECT                                                                                
    Symbol                        
   ,Date                        
   ,Volatility         
                         
 FROM OPENXML(@handle, '//DailyVolatility', 2)                                                                                   
  WITH                                                                                 
  (                                                           
   Symbol varchar(100)                             
  ,Date datetime                        
  ,Volatility float         
  )  As VT where   VT.Volatility is not null                           
                
DELETE PM_DailyVolatility from PM_DailyVolatility                
inner join  #TempVolatility  on DateDiff(d,Convert(VARCHAR(10), #TempVolatility .Date, 110),PM_DailyVolatility.Date)=0               
and #TempVolatility .Symbol = PM_DailyVolatility.Symbol                 
              
                          
 INSERT INTO       
                        
    PM_DailyVolatility                          
 (                          
    Symbol                        
   ,Date                        
   ,Volatility                           
 )                     
  SELECT                           
    Symbol                        
   ,Date                        
   ,Volatility         
   FROM                           
    #TempVolatility                  
                          
 DROP TABLE #TempVolatility               
                      
 EXEC sp_xml_removedocument @handle                          
                           
COMMIT TRANSACTION TRAN1                          
                           
END TRY                          
 BEGIN CATCH                           
  SET @ErrorMessage = ERROR_MESSAGE();                          
  print @errormessage                          
  SET @ErrorNumber = Error_number();                           
  ROLLBACK TRANSACTION TRAN1                           
END CATCH;     
    
    
   
