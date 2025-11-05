


/****** Object:  Stored Procedure dbo.P_GetCVAUEC    Script Date: 12/27/2005 7:50:22 PM ******/
CREATE PROCEDURE dbo.P_GetCVAUEC
	(
		@counterPartyVenueID int,
		@auecID	int	
	)
AS
	
	Select CVAUECID, CounterPartyVenueID, AUECID From T_CVAUEC
	Where CounterPartyVenueID = @counterPartyVenueID AND AUECID = @auecID


