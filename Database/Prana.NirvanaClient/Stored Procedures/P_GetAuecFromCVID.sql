

CREATE PROCEDURE [dbo].[P_GetAuecFromCVID]

	(
		@assetID int ,
		@underlyingID int,
		@cpID int,
		@venueID int
	)

AS
declare @value int


 select @value = cvauec.AuecID
--select cvauec.AuecID 
from 

T_CVAUEC  as cvauec
join T_CounterPartyVenue cv on cv.CounterPartyID = @cpID and cv.VenueID = @venueID

join T_AUEC auec on auec.AssetID = @assetID and auec.UnderlyingID = @underlyingID

where cvauec.counterPartyVenueID = cv.CounterPartyVenueID 
and cvauec.AuecID = auec.AuecID	


select @value
/* SET NOCOUNT ON */
	RETURN 


