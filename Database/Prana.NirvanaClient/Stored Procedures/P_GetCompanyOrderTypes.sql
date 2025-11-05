CREATE PROCEDURE dbo.P_GetCompanyOrderTypes (@companyID INT)
AS
SELECT CompanyID
	,OrderTypeID
FROM T_CompanyOrderTypes
WHERE CompanyID = @companyID