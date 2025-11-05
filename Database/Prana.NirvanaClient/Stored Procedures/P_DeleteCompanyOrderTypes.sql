CREATE PROCEDURE dbo.P_DeleteCompanyOrderTypes (@companyID INT)
AS
DELETE
FROM T_CompanyOrderTypes
WHERE CompanyID = @companyID
