CREATE PROCEDURE dbo.P_GetCompanyHandlingInstructions (@companyID INT)
AS
SELECT CompanyID
	,HandlingInstructionsID
FROM T_CompanyHandlingInstructions
WHERE CompanyID = @companyID