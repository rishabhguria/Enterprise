


/****** Object:  Stored Procedure dbo.P_DeleteCompanyClientTraders    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_DeleteCompanyClientTraders
(
	@companyClientID int	
)
AS

Delete T_CompanyClientTrader
Where CompanyClientID = @companyClientID
	



