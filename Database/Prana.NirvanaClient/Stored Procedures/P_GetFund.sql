


/****** Object:  Stored Procedure dbo.P_GetFund    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_GetFund
	(
		@fundID int
	)
AS
	SELECT     CompanyFundID, FundName, FundShortName, CompanyID
	FROM         T_CompanyFunds
	Where CompanyFundID = @fundID



