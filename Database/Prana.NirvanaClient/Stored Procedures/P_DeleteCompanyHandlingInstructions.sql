CREATE PROCEDURE dbo.P_DeleteCompanyHandlingInstructions (@companyID INT)
AS
DELETE
FROM T_CompanyHandlingInstructions
WHERE CompanyID = @companyID
