
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_Venue')
AND EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'T_CounterPartyVenue')
AND EXISTS (select * from T_Venue  where VenueName='Drops')
	BEGIN
	DECLARE @newVenueID int, @dropsVenueID int

	select @dropsVenueID = VenueID from T_Venue  where VenueName='Drops'

	INSERT INTO T_Venue(VenueName, VenueTypeID, Route, ExchangeID)
			Values('Ex&Assign', 4, 'Exercise & Assignment', '-2147483648')
		        
	SET @newVenueID = scope_identity()

	INSERT INTO T_CounterPartyVenue(CounterPartyID, VenueID, DisplayName, IsElectronic, OatsIdentifier, SymbolConventionID, CurrencyID)
	SELECT CPV.CounterPartyID, @newVenueID, ShortName + '-Ex&Assign', IsElectronic, OatsIdentifier, SymbolConventionID, CurrencyID  
	FROM T_CounterPartyVenue CPV 
	INNER JOIN T_CounterParty CP 
	ON CPV.CounterPartyID = CP.CounterPartyID 
	WHERE VenueID = @dropsVenueID
		
		
	INSERT INTO T_CompanyCounterPartyVenues(CompanyID, CounterPartyVenueID,CounterPartyID)
	SELECT CompanyID, CPV.CounterPartyVenueID, CCPV.CounterPartyID
	FROM T_CompanyCounterPartyVenues CCPV 
	INNER JOIN T_CounterPartyVenue CPV 
	ON CCPV.CounterPartyID = CPV.CounterPartyID
	WHERE CPV.VenueID = @newVenueID
	AND CCPV.CounterPartyVenueID IN
	(SELECT CounterPartyVenueID FROM T_CounterPartyVenue
	WHERE VenueID = @dropsVenueID)

	INSERT INTO T_CVAUEC(CounterPartyVenueID, AUECID)
	SELECT CPV2.CounterPartyVenueID, A.AUECID FROM 
	(SELECT AUECID, CPV.CounterPartyID FROM T_CVAUEC 
	INNER JOIN T_CounterPartyVenue CPV 
	ON T_CVAUEC.CounterPartyVenueID = CPV.CounterPartyVenueID
	Where CPV.VenueID = @dropsVenueID) A
	INNER JOIN T_CounterPartyVenue CPV2 
	ON A.CounterPartyID = CPV2.CounterPartyID
	WHERE CPV2.VenueID = @newVenueID
			

	END

