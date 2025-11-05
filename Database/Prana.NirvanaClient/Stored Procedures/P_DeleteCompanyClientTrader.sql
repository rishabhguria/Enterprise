


/****** Object:  Stored Procedure dbo.P_DeleteCompanyClientTrader    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClientTrader
(
	@companyClientID int	
)
AS

Delete T_CompanyClientTrader
Where CompanyClientID = @companyClientID
	



