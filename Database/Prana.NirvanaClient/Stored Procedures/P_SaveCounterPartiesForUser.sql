


/****** Object:  Stored Procedure dbo.P_SaveCounterPartiesForUser    Script Date: 11/17/2005 9:50:24 AM ******/
CREATE PROCEDURE dbo.P_SaveCounterPartiesForUser
	(
		@counterPartyVenueID int,
		@userID int
	)
AS

	Insert T_CompanyUserCounterPartyVenues(CounterPartyVenueID, CompanyUserID)
	Values(@counterPartyVenueID, @userID)
	
	


