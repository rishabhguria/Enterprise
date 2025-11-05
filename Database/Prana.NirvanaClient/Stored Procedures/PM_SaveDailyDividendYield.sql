  
CREATE proc PM_SaveDailyDividendYield      
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
                           
 CREATE TABLE #TempDividendYield                                                                               
 (                                                                                 
  Symbol varchar(100)                             
  ,Date datetime                        
  ,DividendYield float        
 )                                                                                
                                                                                
 INSERT INTO #TempDividendYield                          
 (                                                                     
    Symbol                        
   ,Date                        
   ,DividendYield        
  )                                                                                
 SELECT                                                                                
    Symbol                        
   ,Date                        
   ,DividendYield         
                         
 FROM OPENXML(@handle, '//DailyDividendYield', 2)                                                                                   
  WITH                                                                                 
  (                                                           
   Symbol varchar(100)                             
  ,Date datetime                        
  ,DividendYield float         
  )  As DY where   DY.DividendYield is not null                           
                
DELETE PM_DailyDividendYield from PM_DailyDividendYield                
inner join  #TempDividendYield  on DateDiff(d,Convert(VARCHAR(10), #TempDividendYield .Date, 110),PM_DailyDividendYield.Date)=0               
and #TempDividendYield .Symbol = PM_DailyDividendYield.Symbol                 
              
                          
 INSERT INTO       
                        
    PM_DailyDividendYield                          
 (                          
    Symbol                        
   ,Date                        
   ,DividendYield                           
 )                     
  SELECT                           
    Symbol                        
   ,Date                        
   ,DividendYield         
   FROM                           
    #TempDividendYield                  
                          
 DROP TABLE #TempDividendYield               
                      
 EXEC sp_xml_removedocument @handle                          
                           
COMMIT TRANSACTION TRAN1                          
                           
END TRY                          
 BEGIN CATCH                           
  SET @ErrorMessage = ERROR_MESSAGE();                          
  print @errormessage                          
  SET @ErrorNumber = Error_number();                           
  ROLLBACK TRANSACTION TRAN1                           
END CATCH;     
    


