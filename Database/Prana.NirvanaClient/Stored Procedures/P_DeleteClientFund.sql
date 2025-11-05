


/****** Object:  Stored Procedure dbo.P_DeleteClientFund    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_DeleteClientFund
(
	@clientFundID int	
)
AS

Delete T_CompanyClientFund
Where CompanyClientFundID = @clientFundID


