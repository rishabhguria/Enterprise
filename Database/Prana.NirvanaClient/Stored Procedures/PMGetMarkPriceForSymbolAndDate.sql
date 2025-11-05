

/****************************************************************************      
Name :   [PMGetMarkPriceForSymbolAndDate]      
Date Created: 20-Jun-2007    
Purpose:  Gets the MarkPrice for the specified Symbol and Date.    
Author: Sugandh Jain      
Module: Close Trade/PM
Parameters:       
   @Symbol int     ,    
   @day datetime    
Execution StateMent:       
 declare @day datetime;     
 set @day = dateadd(day, -2, getutcdate());    
   EXEC [PMGetMarkPriceForSymbolAndDate]  'MSFT' , @day, '', 00;    
 select @day    
Date Modified: <DateModified>       
Description:     <DescriptionOfChange>       
Modified By:     <ModifiedBy>       
****************************************************************************/      
CREATE PROCEDURE [dbo].[PMGetMarkPriceForSymbolAndDate] (      
     @Symbol varchar(50)         
   , @day datetime     
   , @account  int
   , @ErrorMessage varchar(500) output      
   , @ErrorNumber int output       
 )      
AS       
      
      
SET @ErrorMessage = 'Success'      
SET @ErrorNumber = 0      
      
BEGIN TRY      
      
SELECT  TOP 1   
 FinalMarkPrice     
FROM     
 PM_DayMarkPrice    
WHERE    
 Symbol = @symbol  
	AND FundID in (@account, 0)
    AND  Date <= @Day     
 AND ISActive = 1 ORDer by date desc,FundID desc; 
  
END TRY      
BEGIN CATCH      
 SET @ERRORNumber = ERROR_NUMBER();      
 SET @ErrorMessage = ERROR_MESSAGE();      
END CATCH;
