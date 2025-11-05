CREATE PROCEDURE dbo.P_GetCompanyExecutionInstructions (@companyID INT)
AS
SELECT CompanyID
	,ExecutionInstructionsID
FROM T_CompanyExecutionsInstructions
WHERE CompanyID = @companyID