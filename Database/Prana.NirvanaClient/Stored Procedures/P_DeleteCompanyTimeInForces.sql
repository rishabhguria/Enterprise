CREATE PROCEDURE dbo.P_DeleteCompanyTimeInForces (@companyID INT)
AS
DELETE
FROM T_CompanyTimeInForce
WHERE CompanyID = @companyID
