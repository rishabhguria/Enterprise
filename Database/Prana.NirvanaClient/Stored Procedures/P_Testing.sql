

CREATE PROCEDURE dbo.P_Testing
	(
		@RLID int
	)
AS
	
create table #tempTable (TradingAccountID int, CounterPartyVenueID int, CounterPartyID int,  VenueID int, Rank int)

DECLARE @TradingID int, @CPVID int , @Rank int, @CP int, @Venue int


DECLARE c CURSOR
FOR SELECT     T_RoutingLogicVenues.CompanyTradingAccountID_FK AS TradingID, 
                      T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK AS CPVID,  T_RoutingLogicVenues.Rank
FROM         T_RoutingLogicVenues 
WHERE     (T_RoutingLogicVenues.RLID_FK = @RLID) 

OPEN c
FETCH c INTO @TradingID , @CPVID  , @Rank 


WHILE (@@FETCH_STATUS=0) 
	BEGIN	
		IF(@CPVID = NULL)
		BEGIN

			INSERT INTO #tempTable
                      (TradingAccountID , CounterPartyVenueID , CounterPartyID ,  VenueID , Rank )
VALUES     (@TradingID, NULL,NULL,NULL, @Rank)
		END
		ELSE
		BEGIN
			SELECT     @CP=CounterPartyID, @Venue=VenueID
		FROM         T_CounterPartyVenue
		WHERE     (CounterPartyVenueID = @CPVID)
			INSERT INTO #tempTable
                      (TradingAccountID , CounterPartyVenueID , CounterPartyID ,  VenueID , Rank )
VALUES     (NULL, @CPVID,@CP,@Venue, @Rank)
		END

FETCH c INTO @TradingID , @CPVID  , @Rank 

	END
	
CLOSE c
DEALLOCATE c

SELECT * From #tempTable

