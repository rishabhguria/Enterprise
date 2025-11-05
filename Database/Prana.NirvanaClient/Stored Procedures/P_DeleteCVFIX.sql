


/****** Object:  Stored Procedure dbo.P_DeleteCVFIX    Script Date: 12/29/2005 12:35:21 PM ******/
CREATE PROCEDURE dbo.P_DeleteCVFIX
	(
		@counterPartyVenueID int	
	)
AS
Delete T_CVFIX
Where CounterPartyVenueID = @counterPartyVenueID


