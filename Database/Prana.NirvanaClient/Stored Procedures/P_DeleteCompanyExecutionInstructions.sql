CREATE PROCEDURE dbo.P_DeleteCompanyExecutionInstructions (@companyID INT)
AS
DELETE
FROM T_CompanyExecutionsInstructions
WHERE CompanyID = @companyID
