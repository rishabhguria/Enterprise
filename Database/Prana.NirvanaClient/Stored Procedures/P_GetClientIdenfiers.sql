

CREATE PROCEDURE dbo.P_GetClientIdenfiers

	(
		@ClientID int
		
	)

AS
	select IdentifierID,Identifier from 
	T_CompanyClientIdentifier  where CompanyClientID=@ClientID
	
	
	
	RETURN 


