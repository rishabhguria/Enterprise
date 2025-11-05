CREATE VIEW [dbo].[V_GetAllCVAUEC] AS
SELECT DISTINCT CVAUEC.CVAUECID, CVAUEC.AUECID, CV.CounterPartyID, CV.VenueID, CUAUEC.CompanyUserID
FROM         dbo.T_CompanyUserAUEC AS CUAUEC INNER JOIN
                      dbo.T_CompanyAUEC AS COMAUEC ON CUAUEC.CompanyAUECID = COMAUEC.CompanyAUECID INNER JOIN
                      dbo.T_CVAUEC AS CVAUEC ON CVAUEC.AUECID = COMAUEC.AUECID INNER JOIN
                      dbo.T_CompanyUserCounterPartyVenues AS CUCPV ON CUCPV.CounterPartyVenueID = CVAUEC.CounterPartyVenueID INNER JOIN
                      dbo.T_CounterPartyVenue AS CV ON CV.CounterPartyVenueID = CUCPV.CounterPartyVenueID
