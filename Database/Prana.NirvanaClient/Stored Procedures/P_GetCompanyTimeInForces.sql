CREATE PROCEDURE dbo.P_GetCompanyTimeInForces (@companyID INT)
AS
SELECT CompanyID
	,TimeInForceID
FROM T_CompanyTimeInForce
WHERE CompanyID = @companyID