

-- =============================================  
-- Author:  Abhishek  
-- Create date: 29 oct 2007  
-- Description: Calculation Methodology  
-- =============================================  
CREATE PROCEDURE [dbo].[AL_CalculationMethodlogy] (    
 @preAllocate bit   
 ,@ErrorMessage varchar(500) output                                     
 ,@ErrorNumber int output   
  
)      
AS                     
                    
SET @ErrorNumber = 0                    
SET @ErrorMessage = 'Success'                    
                    
BEGIN TRAN TRAN1                  
BEGIN TRY                    
      
UPDATE T_CommissionCalculationTime  
SET IsPostAllocatedCalculation = @preAllocate  
 
select @ErrorNumber  
COMMIT TRANSACTION TRAN1                      
END TRY                      
BEGIN CATCH                       
 SET @ErrorMessage = ERROR_MESSAGE();                      
 SET @ErrorNumber = Error_number();                       
 ROLLBACK TRANSACTION TRAN1                         
 select @ErrorNumber
END CATCH;



