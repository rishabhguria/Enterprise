

CREATE PROCEDURE dbo.P_GetClientFix

	(
		@ClientID int
		
	)

AS
	select SenderCompID,TargetCompID,OnBehalfOfCompID,IP,Port from 
	T_CompanyClientFix  where CompanyClientID=@ClientID
	
	
	
	RETURN 


