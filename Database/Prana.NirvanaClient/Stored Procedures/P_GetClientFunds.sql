


/****** Object:  Stored Procedure dbo.P_GetClientFunds    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_GetClientFunds
	(
		@companyClientID int
	)

AS
	SELECT     CompanyClientFundID, CompanyClientFundName, CompanyClientFundShortName, CompanyClientID
	FROM         T_CompanyClientFund
	WHERE CompanyClientID = @companyClientID


