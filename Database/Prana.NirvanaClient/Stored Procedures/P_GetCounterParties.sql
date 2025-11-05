


/****** Object:  Stored Procedure dbo.P_GetCounterParties    Script Date: 11/17/2005 9:50:20 AM ******/
CREATE PROCEDURE dbo.P_GetCounterParties
	(
		@counterPartyID int
	)
AS
	SELECT CounterPartyID, FullName, ShortName, Address, Phone, Fax, ContactName1, Title1, EMail1, 
			ContactName2, Title2, EMail2
			CounterPartyTypeID
	FROM T_CounterParty
	Where CounterPartyID = @counterPartyID


