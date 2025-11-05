


/****** Object:  Stored Procedure dbo.P_SaveCompanyCVVenues    Script Date: 11/17/2005 9:50:23 AM ******/
--This will basically save CounterPartyVenue for a company
CREATE PROCEDURE dbo.P_SaveCompanyCounterPartyVenuesPermissions
	(
		@companyID int,
		@counterPartyID int,
		@venueID int
	)
AS
	Declare @CounterPartyVenueID int
	Set @CounterPartyVenueID = 0
	Declare @total int 
	Set @total = 0
	Declare @result int 
	Set @result = 0
	
	-- This is done to look for the registered or existing venue for the equivalent counterparty.
	 Select @CounterPartyVenueID = CounterPartyVenueID
	FROM T_CounterPartyVenue
	Where CounterPartyID = @counterPartyID
		AND VenueID = @venueID 
	
	if(@CounterPartyVenueID > 0)
	Begin
		Select @total = Count(*)
		From T_CompanyCounterPartyVenues
		Where CounterPartyVenueID = @CounterPartyVenueID AND CompanyID = @companyID 
		
		if(@total > 0)
		begin
		--Update CompanyCounterPartyVenues
			Update T_CompanyCounterPartyVenues
			Set CompanyID = @companyID,
				CounterPartyVenueID = @CounterPartyVenueID,
				CounterPartyID = @counterPartyID
				
				where CounterPartyVenueID = @CounterPartyVenueID AND CompanyID = @companyID
				Select @result = CompanyCounterPartyCVID From T_CompanyCounterPartyVenues where CounterPartyVenueID = @CounterPartyVenueID AND CompanyID = @companyID
		end
		else
		begin
			Insert T_CompanyCounterPartyVenues(companyID, CounterPartyVenueID, CounterPartyID)
			Values(@companyID, @CounterPartyVenueID, @counterPartyID)
			
			Set @result = scope_identity()
		end
	end
select @result	


