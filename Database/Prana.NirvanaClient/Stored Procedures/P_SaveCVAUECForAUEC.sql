CREATE PROCEDURE dbo.P_SaveCVAUECForAUEC (@auecID INT)
AS
DECLARE @total INT

SET @total = 0

SELECT @total = Count(*)
FROM T_CVAUEC
WHERE AUECID = @auecID

IF (@total <= 0)
BEGIN
	INSERT INTO T_CVAUEC (
		CounterPartyVenueID
		,AUECID
		)
	SELECT CounterPartyVenueID
		,@auecID
	FROM T_CounterPartyVenue
END
