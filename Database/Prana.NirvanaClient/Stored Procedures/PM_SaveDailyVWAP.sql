
CREATE proc PM_SaveDailyVWAP      
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
                           
 CREATE TABLE #TempVWAP                                                                               
 (                                                                                 
  Symbol varchar(100)                             
  ,Date datetime                        
  ,VWAP float        
 )                                                                                
                                                                                
 INSERT INTO #TempVWAP                          
 (                                                                     
    Symbol                        
   ,Date                        
   ,VWAP        
  )                                                                                
 SELECT                                                                                
    Symbol                        
   ,Date                        
   ,VWAP         
                         
 FROM OPENXML(@handle, '//DailyVWAP', 2)                                                                                   
  WITH                                                                                 
  (                                                           
   Symbol varchar(100)                             
  ,Date datetime                        
  ,VWAP float         
  )  As VT where   VT.VWAP is not null                           
                
DELETE PM_DailyVWAP from PM_DailyVWAP                
inner join  #TempVWAP  on DateDiff(d,Convert(VARCHAR(10), #TempVWAP .Date, 110),PM_DailyVWAP.Date)=0               
and #TempVWAP .Symbol = PM_DailyVWAP.Symbol                 
              
                          
 INSERT INTO       
                        
    PM_DailyVWAP                          
 (                          
    Symbol                        
   ,Date                        
   ,VWAP                           
 )                     
  SELECT                           
    Symbol                        
   ,Date                        
   ,VWAP         
   FROM                           
    #TempVWAP                  
                          
 DROP TABLE #TempVWAP               
                      
 EXEC sp_xml_removedocument @handle                          
                           
COMMIT TRANSACTION TRAN1                          
                           
END TRY                          
 BEGIN CATCH                           
  SET @ErrorMessage = ERROR_MESSAGE();                          
  print @errormessage                          
  SET @ErrorNumber = Error_number();                           
  ROLLBACK TRANSACTION TRAN1                           
END CATCH;     
    
    
   
