


/****** Object:  Stored Procedure dbo.P_SaveCounterPartyVenuesForUser    Script Date: 11/17/2005 9:50:24 AM ******/ -- Updated on 12-01-06
CREATE PROCEDURE [dbo].[P_SaveCounterPartyVenuesForUser]
	(
		@counterPartyID int,
		@venueID int,
		@userID int,
		@companyID int
	)
AS
	Declare @result int
	Declare @total int 
	Set @total = 0
	
	Declare @CounterPartyVenueID int
	Set @CounterPartyVenueID = 0
	
	Declare @CompanyCounterPartyCVID int
	Set @CompanyCounterPartyCVID = 0
		
	-- This is done to look for the registered or existing venue for the equivalent counterparty.
	 Select @CounterPartyVenueID = CounterPartyVenueID
	FROM T_CounterPartyVenue
	Where CounterPartyID = @counterPartyID
		AND VenueID = @venueID
		
		-- This is done to get the CompanyCounterPartyCVID for the corresponding company, counterparty & 
		-- CounterPartyVenue from the T_CompanyCounterPartyVenues table so as to store it in the 
		--T_CompanyUserCounterPartyVenues table.
	 Select @CompanyCounterPartyCVID = CompanyCounterPartyCVID
	FROM T_CompanyCounterPartyVenues
	Where CounterPartyID = @counterPartyID
		AND CounterPartyVenueID = @CounterPartyVenueID AND CompanyID = @companyID
	
	if(@CounterPartyVenueID > 0)
	Begin
		Select @total = Count(*)
		From T_CompanyUserCounterPartyVenues
		Where CompanyUserID = @userID AND CompanyCounterPartyCVID = @CompanyCounterPartyCVID
		if(@total > 0)
		begin	
			--Update T_CompanyUserCounterPartyVenues
			Update T_CompanyUserCounterPartyVenues 
			Set CompanyUserID = @userID, 
				CompanyCounterPartyCVID = @CompanyCounterPartyCVID,
				CounterPartyVenueID = @CounterPartyVenueID
				
			Where CompanyUserID = @userID AND CompanyCounterPartyCVID = @CompanyCounterPartyCVID
			
			Select @result = CompanyUserCounterPartyCVID From T_CompanyUserCounterPartyVenues Where CompanyUserID = @userID AND CompanyCounterPartyCVID = @CompanyCounterPartyCVID
		end
		else
		--Insert T_CompanyUserCounterPartyVenues
		begin
		
			Insert T_CompanyUserCounterPartyVenues(CounterPartyVenueID, CompanyUserID, CompanyCounterPartyCVID)
			Values(@CounterPartyVenueID, @userID, @CompanyCounterPartyCVID)
			
			Set @result = scope_identity()
		end
	end
	select @result
	
	



