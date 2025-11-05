


/****** Object:  Stored Procedure dbo.P_GetClientFundByID    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_GetClientFundByID
	(
		@companyID int,
		@clientFundID int
	)
AS
	SELECT CompanyClientFundID, CompanyClientFundName, CompanyClientFundShortName, CompanyClientID
	FROM T_CompanyClientFund
	Where CompanyClientID = @companyID or CompanyClientFundID = @clientFundID



