CREATE PROCEDURE [dbo].[P_GetFundsByCompanyID]
	(
		@CompanyID int
	)
AS
	SELECT     CompanyFundID, FundName, FundShortName
	FROM         T_CompanyFunds
	Where CompanyID = @CompanyID