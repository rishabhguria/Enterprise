

CREATE PROCEDURE dbo.P_GetUserClients
	(
		@userID int	
	)
AS
	SELECT     CompanyClientID, ClientName
	FROM         T_CompanyClient


