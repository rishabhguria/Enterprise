






/****************************************************************************      
Name :   [PMGetGivenMonthMarkPricesListForAllOpenPositions]      
Date Created: 27-Jun-2007    
Purpose:  Gets the MarkPrice for the specified Symbol and Date.    
  Month has to be sent in yyyymm format.     
  will use format 112 in cast and convert    
Author: Bhupesh Bareja      
Parameters:       
   @Symbol int,    
   @day datetime    
Execution StateMent:       
    
select * from PM_DayMarkPrice    
    
 declare @date datetime;    
 set @date = getutcdate();    
--select convert(varchar(6), @date, 112)    
   EXEC PMGetGivenMonthMarkPricesListForAllOpenPositions   '', 00, '5/27/2007 6:46:45 PM';    
 select @day    
Date Modified: <DateModified>       
Description:     <DescriptionOfChange>       
Modified By:     <ModifiedBy>       
****************************************************************************/      
CREATE PROCEDURE [dbo].[PMGetGivenMonthMarkPricesListForAllOpenPositions] (            
   @ErrorMessage varchar(500) output      
   , @ErrorNumber int output
   , @date datetime       
 )      
AS        
      
SET @ErrorMessage = 'Success';    
SET @ErrorNumber = 0;    
      
BEGIN TRY      
      
SELECT     distinct
 PMNET.Symbol,   
 MARK.FinalMarkPrice ,
 convert(varchar(6), DateAdd(mm, -1,  getutcdate()), 112)  AS [Month],
 MARK.ApplicationMarkPrice,
 MARK.DayMarkPriceID
FROM      
 PM_Netpositions AS PMNET   
 INNER JOIN   
 (  
select   
 A.symbol,    
 A.finalmarkprice,
 A.ApplicationMarkPrice,
 A.DayMarkPriceID	  
from pm_daymarkprice A  
     inner join (  
    SELECT   
     Symbol      
     , Max(Date) as Date  
    FROM     
     PM_DayMarkPrice    
    WHERE    
     --convert(varchar(6), Date, 112) = convert(varchar(6), DATEADD(day, DATEDIFF(day, 0, @date), 0), 112)/*convert(varchar(6), DateAdd(mm, -1,  getutcdate()), 112)*/  
	 convert(varchar(6), Date, 112) = convert(varchar(6), DateAdd(mm, -1,  @date), 112)
     AND   
     ISActive = 1  
    GROUP BY   
     SYMBOL  
       ) AS B on A.symbol = b.symbol and a.date = b.date  
) AS MARK ON Mark.Symbol = PMNET.Symbol     
  
END TRY      
BEGIN CATCH      
 SET @ERRORNumber = ERROR_NUMBER();      
 SET @ErrorMessage = ERROR_MESSAGE();      
END CATCH;






