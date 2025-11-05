CREATE PROCEDURE [dbo].[PMGetAllMarkPricesForSymbolsAndExactDates] (      
	 @xml NTEXT
	,@ErrorMessage VARCHAR(500) OUTPUT
	,@ErrorNumber INT OUTPUT       
)                                      
AS  
SET @ErrorMessage = 'Success'
SET @ErrorNumber = 0

BEGIN TRY
                           
 DECLARE @handle INT
 exec sp_xml_preparedocument @handle OUTPUT,@Xml 
     
 CREATE TABLE #TempSymbolDate                                               
 (                                                                                             
  Symbol VARCHAR(100)                    
  ,Date DATETIME
  ,AccountId	INT           
 )                                                   
                                                                                            
 INSERT INTO #TempSymbolDate(Symbol,Date)                                                                                            
 SELECT Symbol,Date
 FROM OPENXML(@handle, '//SymbolDate', 2)
 WITH (Symbol VARCHAR(100),Date DATETIME)


SELECT PM_DayMarkPrice.Symbol
,PM_DayMarkPrice.Date
,PM_DayMarkPrice.FinalMarkPrice
FROM PM_DayMarkPrice
RIGHT OUTER JOIN #TempSymbolDate
ON PM_DayMarkPrice.Date = #TempSymbolDate.Date
AND PM_DayMarkPrice.Symbol=#TempSymbolDate.Symbol
WHERE PM_DayMarkPrice.ISActive = 1

DROP Table #TempSymbolDate
   
END TRY      
BEGIN CATCH      
 SET @ERRORNumber = ERROR_NUMBER();      
 SET @ErrorMessage = ERROR_MESSAGE();      
END CATCH;