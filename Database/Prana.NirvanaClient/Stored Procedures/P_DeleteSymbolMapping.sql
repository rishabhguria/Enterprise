
/****** Object:  Stored Procedure dbo.P_DeleteSymbolMapping    Script Date: 11/17/2005 9:50:20 AM ******/ 
/* NOT IN USE NOW */
CREATE PROCEDURE dbo.P_DeleteSymbolMapping
	(
		@counterPartyVenueID int	
	)
AS
Delete T_CVSymbolMapping Where CVAUECID in (Select CVAUECID from T_CounterPartyVenue Where CounterPartyVenueID = @counterPartyVenueID)

