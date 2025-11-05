


	Create PROCEDURE [dbo].[P_GetCompanyPermittedHandlingInstructions] (@companyID INT)  
AS  
SELECT T_CompanyHandlingInstructions.HandlingInstructionsID  
 ,T_HandlingInstructions.HandlingInstructions  
 ,T_HandlingInstructions.HandlingInstructionsTagValue  
FROM T_CompanyHandlingInstructions  
INNER JOIN T_HandlingInstructions  
 ON T_CompanyHandlingInstructions.HandlingInstructionsID = T_HandlingInstructions.HandlingInstructionsID and CompanyID = @companyID