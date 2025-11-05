

/****************************************************************************    
Name :   [PMGetMarkPriceListForMonth]    
Date Created: 20-Jun-2007  
Purpose:  Gets the MarkPrice for the specified Symbol and Date.  
  Month has to be sent in yyyymm format.   
  will use format 112 in cast and convert  
Author: Sugandh Jain    
Parameters:     
   @Symbol int,  
   @day datetime  
Execution StateMent:     
  
select * from PM_DayMarkPrice  
  
 declare @date datetime;  
 set @date = getutcdate();  
--select convert(varchar(6), @date, 112)  
   EXEC [PMGetMarkPriceListForMonth]   '200704', '', 00;  
 select @day  
Date Modified: <DateModified>     
Description:     <DescriptionOfChange>     
Modified By:     <ModifiedBy>     
****************************************************************************/    
CREATE PROCEDURE [dbo].[PMGetMarkPriceListForMonth] (           
     @Month varchar(6)       
   , @ErrorMessage varchar(500) output    
   , @ErrorNumber int output     
 )    
AS      
    
SET @ErrorMessage = 'Success'  ;  
SET @ErrorNumber = 0  ;  
    
BEGIN TRY    
    
SELECT   
   DATEADD(day, DATEDIFF(day, 0, Date ), 0) as Day  
    , Symbol  
 , FinalMarkPrice   
FROM   
 PM_DayMarkPrice  
WHERE   
 convert(varchar(6), Date, 112) = @Month   
 AND ISActive = 1;  
  
END TRY    
BEGIN CATCH    
 SET @ERRORNumber = ERROR_NUMBER();    
 SET @ErrorMessage = ERROR_MESSAGE();    
END CATCH;
