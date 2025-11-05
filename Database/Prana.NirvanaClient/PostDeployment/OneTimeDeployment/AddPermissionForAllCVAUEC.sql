SELECT T_CounterPartyVenue.CounterPartyVenueID
	,T_AUEC.AUECID
INTO #Temp_AllCVAUEC
FROM T_CounterPartyVenue
	,T_AUEC

INSERT INTO T_CVAUEC (
	CounterPartyVenueID
	,AUECID
	) (
	SELECT #Temp_AllCVAUEC.CounterPartyVenueID
	,#Temp_AllCVAUEC.AUECID FROM #Temp_AllCVAUEC

EXCEPT
	
	SELECT T_CVAUEC.CounterPartyVenueID
	,T_CVAUEC.AUECID FROM T_CVAUEC
	)

DROP TABLE #Temp_AllCVAUEC
