CREATE PROCEDURE dbo.P_GetRMCommonFunds 
	(
		@companyID int
	)

AS
	SELECT        T_RMCompanyFundAccntOverall.CompanyFundAccntID,  T_CompanyFunds.FundName,T_CompanyFunds.FundShortName, T_RMCompanyFundAccntOverall.CompanyID 
	                        
	FROM            T_RMCompanyFundAccntOverall INNER JOIN
	                         T_CompanyFunds ON T_RMCompanyFundAccntOverall.CompanyFundAccntID = T_CompanyFunds.CompanyFundID
	WHERE        (T_RMCompanyFundAccntOverall.CompanyID = @companyID)
