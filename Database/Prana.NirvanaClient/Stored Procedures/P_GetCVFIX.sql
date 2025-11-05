


/****** Object:  Stored Procedure dbo.P_GetCVFIX    Script Date: 12/29/2005 12:25:22 PM ******/
CREATE PROCEDURE dbo.P_GetCVFIX
	(
		@counterPartyVenueID int	
	)
AS
	
	Select CVFIXID, CounterPartyVenueID, Acronymn, FixVersionID, TargetCompID,
						DeliverToCompID, DeliverToSubID From T_CVFIX
	Where CounterPartyVenueID = @counterPartyVenueID


