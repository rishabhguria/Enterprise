

/****************************************************************************    
Name :   [PMGetMarkPriceListForDate] 
Date Created: 27-Jun-2007  
Purpose:  Gets the MarkPriceList for the specified Date for all the symbols.
Author: Sugandh Jain    
Parameters:        
   @day datetime  
Execution StateMent:     
 declare @day datetime;   
 set @day = dateadd(day, -2, getutcdate());  
   EXEC [PMGetMarkPriceListForDate]  @day, '', 00;  
 select @day  
Date Modified: <DateModified>     
Description:     <DescriptionOfChange>     
Modified By:     <ModifiedBy>     
****************************************************************************/    
CREATE PROCEDURE [dbo].[PMGetMarkPriceListForDate] (    
           
   @day datetime       
   , @ErrorMessage varchar(500) output    
   , @ErrorNumber int output     
 )    
AS     
    
    
SET @ErrorMessage = 'Success'    
SET @ErrorNumber = 0    
    
BEGIN TRY    
    
SELECT   
 FinalMarkPrice   
FROM   
 PM_DayMarkPrice  
WHERE  
 DATEADD(day, DATEDIFF(day, 0, Date ), 0) = DATEADD(day, DATEDIFF(day, 0, @Day ), 0)    
 AND ISActive = 1;   
END TRY    
BEGIN CATCH    
 SET @ERRORNumber = ERROR_NUMBER();    
 SET @ErrorMessage = ERROR_MESSAGE();    
END CATCH;

