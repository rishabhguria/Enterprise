


/****** Object:  Stored Procedure dbo.P_GetClients    Script Date: 11/17/2005 9:50:20 AM ******/
CREATE PROCEDURE dbo.P_GetClients
(
	@companyID int	
)
AS
	SELECT     CompanyClientID, ClientName
	FROM         T_CompanyClient
	Where CompanyID = @companyID


