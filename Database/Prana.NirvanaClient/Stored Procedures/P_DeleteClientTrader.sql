


/****** Object:  Stored Procedure dbo.P_DeleteClientTrader    Script Date: 11/17/2005 9:50:23 AM ******/
CREATE PROCEDURE dbo.P_DeleteClientTrader
(
	@companyClientID int	
)
AS

Delete T_CompanyClientTrader
Where CompanyClientID = @companyClientID
	



