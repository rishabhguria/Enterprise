

/****************************************************************************            
Name :   [PMGetAllStoredMonthMarkPricesForCurrentMonth]            
Date Created: 22-Jun-2007          
Purpose:  Gets the MarkPrices applicable to this month present in our system.                
Author: Sugandh Jain            
Parameters:             
          
Execution StateMent:             
          
select * from PM_DayMarkPrice          
          
 declare @date datetime;          
 set @date = getutcdate();          
--select convert(varchar(6), @date, 112)          
   EXEC [PMGetAllStoredMonthMarkPricesForCurrentMonth]           '', 00;          
 select @day          
[PMGetMonthMarkPricesListForAllOpenPositions]             
Date Modified: <DateModified>             
Description:     <DescriptionOfChange>             
Modified By:     <ModifiedBy>             
****************************************************************************/            
CREATE PROCEDURE [dbo].[PMGetAllStoredMonthMarkPricesForCurrentMonth] (                  
   @ErrorMessage varchar(500) output            
   , @ErrorNumber int output             
 )            
AS              
            
SET @ErrorMessage = 'Success';          
SET @ErrorNumber = 0;          
            
BEGIN TRY            
            
select         
 A.Symbol,          
 A.FinalMarkPrice,       
 convert(varchar(6), getutcdate(), 112)  AS [Month]      
from pm_daymarkprice A        
     inner join (        
    SELECT         
     Symbol            
     , Max(Date) as Date        
    FROM           
     PM_DayMarkPrice          
    WHERE          
     convert(varchar(6), Date, 112) = convert(varchar(6), DateAdd(mm, -1,  getutcdate()), 112)        
     AND         
     ISActive = 1        
    GROUP BY         
     SYMBOL        
       ) AS B on A.symbol = b.symbol and a.date = b.date        
        
END TRY            
BEGIN CATCH            
 SET @ERRORNumber = ERROR_NUMBER();            
 SET @ErrorMessage = ERROR_MESSAGE();            
END CATCH;
