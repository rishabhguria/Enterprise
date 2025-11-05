

/****** Object:  Stored Procedure dbo.P_DeleteCounterPartyVenue    Script Date: 11/17/2005 9:50:21 AM ******/
CREATE PROCEDURE [dbo].[P_DeleteCounterPartyVenue]
	(
		@counterPartyVenueID int	
	)
AS
Declare @total int

		Select @total = Count(1) 
		From T_CompanyCounterPartyVenues AS CCPV
			Where CCPV.CounterPartyVenueID = @counterPartyVenueID
			
		if( @total = 0)
		begin
			Declare @cvAUECID int
			Select @cvAUECID = CVAUECID FROM T_CVAUEC WHERE CounterPartyVenueID = @counterPartyVenueID
			
			Delete T_CVAUECSide Where CVAUECID IN(SELECT CVAUECID FROM T_CVAUEC WHERE CounterPartyVenueID = @counterPartyVenueID) 
			
			Delete T_CVAUECCompliance Where CVAUECID IN(SELECT CVAUECID FROM T_CVAUEC WHERE CounterPartyVenueID = @counterPartyVenueID) 
			
			Delete T_CVAUECExecutionInstructions Where CVAUECID IN(SELECT CVAUECID FROM T_CVAUEC WHERE CounterPartyVenueID = @counterPartyVenueID) 
			
			Delete T_CVAUECHandlingInstructions Where CVAUECID IN(SELECT CVAUECID FROM T_CVAUEC WHERE CounterPartyVenueID = @counterPartyVenueID) 
			
			Delete T_CVAUECOrderTypes Where CVAUECID IN(SELECT CVAUECID FROM T_CVAUEC WHERE CounterPartyVenueID = @counterPartyVenueID) 
			
			Delete T_CVAUECTimeInForce Where CVAUECID IN(SELECT CVAUECID FROM T_CVAUEC WHERE CounterPartyVenueID = @counterPartyVenueID) 
			
			exec P_DeleteSymbolMapping @counterPartyVenueID
			
			exec P_DeleteCVAUECs @counterPartyVenueID, '' --'' parameter is passed so as to give null value as the respective procedure has such requirement.

			exec P_DeleteCVCurrencies @counterPartyVenueID
			exec P_DeleteCVFIX @counterPartyVenueID
			--exec P_DeleteSymbolMapping @counterPartyVenueID 

			--Delete T_CounterPartyVenueDetails
			--Where CounterPartyVenueID = @counterPartyVenueID

		
			Delete T_CounterPartyVenue
			Where CounterPartyVenueID = @counterPartyVenueID

end
