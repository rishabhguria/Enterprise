CREATE PROCEDURE [dbo].[P_GetCompanyFunds] (@companyID INT)
AS
SELECT CompanyFundID
	,FundName
	,FundShortName
	,CompanyID
FROM T_CompanyFunds
WHERE CompanyID = @companyID
	AND IsActive = 1
