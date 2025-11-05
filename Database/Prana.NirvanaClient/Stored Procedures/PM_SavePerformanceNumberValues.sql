CREATE Proc [dbo].[PM_SavePerformanceNumberValues]          
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
                    
  CREATE TABLE #TempPerformanceNumberValues          
  (                                                                           
    FundID int                       
   ,Date datetime                  
   ,MTDValue float          
   ,QTDValue float          
   ,YTDValue float
   ,MTDReturn float
   ,QTDReturn float
   ,YTDReturn float          
   )                                                                          
                                                                          
INSERT INTO #TempPerformanceNumberValues          
 (                                                                          
    FundID            
   ,Date           
   ,MTDValue           
   ,QTDValue           
   ,YTDValue
   ,MTDReturn
   ,QTDReturn
   ,YTDReturn        
 )                                                                          
SELECT                                                                           
    FundID            
   ,Date           
   ,MTDValue           
   ,QTDValue           
   ,YTDValue
   ,MTDReturn
   ,QTDReturn
   ,YTDReturn           
                  
FROM OPENXML(@handle, '//DailyPerformanceNumbers', 2)                                                                             
 WITH                                                                           
 (                                                     
    FundID int                       
   ,Date datetime                  
   ,MTDValue float         
   ,QTDValue float         
   ,YTDValue float
   ,MTDReturn float
   ,QTDReturn float
   ,YTDReturn float        
 )                    
                  
DELETE  PM_DailyPerformanceNumbers  
WHERE DATEADD(day, DATEDIFF(day, 0, PM_DailyPerformanceNumbers.Date), 0) in (Select DATEADD(day, DATEDIFF(day, 0, Convert(datetime, #TempPerformanceNumberValues.Date, 113)), 0) from   #TempPerformanceNumberValues)              
      AND PM_DailyPerformanceNumbers.FundID in (Select #TempPerformanceNumberValues.FundID From #TempPerformanceNumberValues)    
  
DELETE  PM_DailyPerformanceNumbers          
 WHERE DATEADD(day, DATEDIFF(day, 0, PM_DailyPerformanceNumbers.Date), 0) in (Select DATEADD(day, DATEDIFF(day, 0, Convert(datetime, #TempPerformanceNumberValues.Date, 113)), 0) from   #TempPerformanceNumberValues)              
  AND PM_DailyPerformanceNumbers.FundID not in (Select #TempPerformanceNumberValues.FundID From #TempPerformanceNumberValues)     
                  
                   
                    
 INSERT INTO                   
 PM_DailyPerformanceNumbers          
   (                    
    FundID            
   ,Date           
   ,MTDValue           
   ,QTDValue           
   ,YTDValue
   ,MTDReturn
   ,QTDReturn
   ,YTDReturn     
  )                    
                    
  SELECT                     
    FundID            
   ,Date           
   ,Sum(MTDValue)           
   ,Sum(QTDValue)           
   ,Sum(YTDValue)            
   ,Sum(MTDReturn)
   ,Sum(QTDReturn)
   ,Sum(YTDReturn)
  FROM                     
   #TempPerformanceNumberValues      
Group By Date,FundID      
                    
   
                    
                    
                    
DROP TABLE #TempPerformanceNumberValues          
                    
                    
EXEC sp_xml_removedocument @handle                    
                    
COMMIT TRANSACTION TRAN1                    
                    
END TRY                    
BEGIN CATCH                 
SET @ErrorMessage = ERROR_MESSAGE();                    
print @errormessage                    
 SET @ErrorNumber = Error_number();                     
 ROLLBACK TRANSACTION TRAN1                     
END CATCH;


