
CREATE PROCEDURE dbo.P_GetCVAUECForCVAUECID
	(
		@cvAUECID int
	)
AS
	
	Select CVAUECID, CounterPartyVenueID, AUECID From T_CVAUEC
	Where CVAUECID = @cvAUECID
