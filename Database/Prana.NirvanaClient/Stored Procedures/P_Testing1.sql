

CREATE PROCEDURE dbo.P_Testing1
	(
		@RLID int
	)
AS
SET ANSI_NULLS OFF
	
create table #tempTable (TradingAccountID int, CounterPartyVenueID int, Rank int,CounterPartyID int,  VenueID int )

DECLARE @TradingID int, @CPVID int , @Rank int,  @Dummy int
SEt @Dummy =1

INSERt INTO #tempTable SELECT     T_RoutingLogicVenues.CompanyTradingAccountID_FK AS TradingAccountID, 
                      T_RoutingLogicVenues.CompanyCounterPartyVenueID_FK AS CounterPartyVenueID,  T_RoutingLogicVenues.Rank, NULL,NULL
FROM         T_RoutingLogicVenues 
WHERE     (T_RoutingLogicVenues.RLID_FK = @RLID) 


DECLARE c CURSOR
FOR SELECT    CounterPartyVenueID , Rank FROM         #tempTable 


OPEN c
FETCH c INTO  @CPVID  , @Rank 

DECLARE @CP int, @Venue int
SET @CP=-1
SET @Venue = -1

WHILE (@@FETCH_STATUS=0) 
	BEGIN	
		
/**
			INSERT INTO #tempTable
                      (TradingAccountID , CounterPartyVenueID , CounterPartyID ,  VenueID , Rank )
VALUES     (@TradingID, NULL,NULL,NULL, @Rank)
		END
		ELSE, @Venue=VenueID
		BEGIN   **/
			SELECT     @CP=CounterPartyID, @Venue=VenueID
		FROM         T_CounterPartyVenue
		WHERE     (CounterPartyVenueID = @CPVID)
			UPDATE    #tempTable
SET              CounterPartyID = @CP , VenueID =@Venue
WHERE     (Rank = @Rank)
		/**	INSERT INTO #tempTable
                      (TradingAccountID , CounterPartyVenueID , CounterPartyID ,  VenueID , Rank )
VALUES     (NULL, @CPVID,@CP,@Venue, @Rank)
		END
		**/
		SET @CP=-1
SET @Venue = -1
		

FETCH c INTO  @CPVID  , @Rank 

	END
	
CLOSE c
DEALLOCATE c

SELECT * From #tempTable

