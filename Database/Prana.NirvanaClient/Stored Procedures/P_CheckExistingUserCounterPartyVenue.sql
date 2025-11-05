


/****** Object:  Stored Procedure dbo.P_CheckExistingUserCounterPartyVenue    Script Date: 11/17/2005 9:50:22 AM ******/
CREATE PROCEDURE dbo.P_CheckExistingUserCounterPartyVenue
	(
		@counterPartyID int,
		@venueID int,
		@companyID int
	)
AS
	Declare @CounterPartyVenueID int
	Set @CounterPartyVenueID = 0
	Declare @count int
	Declare @result int
	Set @count = 0
	Set @result = 0
	
	-- This is done to look for the registered or existing venue for the equivalent counterparty.
	 Select @CounterPartyVenueID = CounterPartyVenueID
	FROM T_CounterPartyVenue
	Where CounterPartyID = @counterPartyID
		AND VenueID = @venueID
	
Select @count = Count(*)
--From T_CompanyUserCounterPartyVenues
From T_CompanyCounterPartyVenues
Where CounterPartyVenueID = @CounterPartyVenueID AND CompanyID = @companyID

if(@count > 0)
begin
	Set @result = 1
end
else
begin
	Set @result = 0
end
select @result



