CREATE PROCEDURE [dbo].[P_GetCompanyPermittedExecutionInstructions] (@companyID INT)
AS
SELECT T_CompanyExecutionsInstructions.ExecutionInstructionsID
	,T_ExecutionInstructions.ExecutionInstructions
	,T_ExecutionInstructions.ExecutionInstructionsTagValue
FROM T_CompanyExecutionsInstructions
INNER JOIN T_ExecutionInstructions
	ON T_CompanyExecutionsInstructions.ExecutionInstructionsID = T_ExecutionInstructions.ExecutionInstructionsID and CompanyID = @companyID