

/****************************************************************************  
Name :   PMGetMarkPositions  
Date Created: 07-feb-2007  
Purpose:  Get all the valid symbol's mark positions.  
Author: Bhupesh Bareja  
Execution Statement : exec PMGetMarkPositions '', 0  
  
Date Modified: <DateModified>   
Description:     <DescriptionOfChange>   
Modified By:     <ModifiedBy>   
****************************************************************************/  
CREATE PROCEDURE [dbo].[PMGetMarkPositions] (  
     
     @ErrorMessage varchar(500) output  
   , @ErrorNumber int output   
)  
AS  
  
SET @ErrorMessage = 'Success'  
SET @ErrorNumber = 0  
  
BEGIN TRY  
  
 Select   
   distinct MarkPositionID, Symbol, MarkPrice, MarkDateTime  
 From   
  PM_MarkPositions  
  Where IsActive = 1  
 Order By   
  Symbol  
  
END TRY  
BEGIN CATCH  
 SET @ERRORNumber = ERROR_NUMBER();  
 SET @ErrorMessage = ERROR_MESSAGE();  
END CATCH;
