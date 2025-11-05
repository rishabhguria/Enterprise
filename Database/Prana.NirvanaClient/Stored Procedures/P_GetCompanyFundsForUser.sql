
CREATE PROCEDURE [dbo].[P_GetCompanyFundsForUser]
(
		@paramUserID int		
)
AS
	
	Select CUF.CompanyUserFundID, CUF.CompanyFundID, CF.FundName
	From T_CompanyFunds CF, T_CompanyUserFunds CUF
	Where CF.CompanyFundID = CUF.CompanyFundID
	And CUF.CompanyUserID = @paramUserID
	And CF.IsActive = 1

