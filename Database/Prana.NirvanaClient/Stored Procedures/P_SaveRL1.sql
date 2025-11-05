


CREATE PROCEDURE dbo.P_SaveRL1
	(
		@MemoryID	varchar(50),
		@RLID	int,
		@RLName	varchar(50),
		@AUECID	int,
		@AssetID	int,
		@AssetName	varchar(50),
		@UnderLyingID	int,
		@UnderLyingName	varchar(50),
		@AUECExchangeID	int,
		@ExchangeName	varchar(50),
		@TradingAccountID0	int,
		@TradingAccountName0	varchar(50),
		@CounterPartyID0	int,
		@CounterPartyName0	varchar(50),
		@VenueID0	int,
		@VenueName0	varchar(50),
		@TradingAccountID1	int,
		@TradingAccountName1	varchar(50),
		@CounterPartyID1	int,
		@CounterPartyName1	varchar(50),
		@VenueID1	int,
		@VenueName1	varchar(50),
		@TradingAccountID2	int,
		@TradingAccountName2	varchar(50),
		@CounterPartyID2	int,
		@CounterPartyName2	varchar(50),
		@VenueID2	int,
		@VenueName2	varchar(50),
		@TradingAccountDefaultID	int,
		@TradingAccountNameDefault	varchar(50),
		@ParameterID0	int,
		@ParameterValue0	varchar(50),
		@JoinCondition0	int,
		@OperatorID0	int,
		@ParameterID1	int,
		@ParameterValue1	varchar(50),
		@JoinCondition1	int,
		@OperatorID1	int,
		@ParameterID2	int,
		@ParameterValue2	varchar(50),
		@JoinCondition2	int,
		@OperatorID2	int
				
	)
AS
	DECLARE @result int
	
	DECLARE @totalRLpk int, @totalRLClientfk int,@totalRLGroupfk int, @CounterPartyVenueID int
	
		
	--primary key  chk
	SET @totalRLpk = 0
	SELECT     @totalRLpk = COUNT(*)
	FROM         T_RoutingLogic
	WHERE     (RLID = @RLID)
	
	--foriegn key chk
	SET @totalRLClientfk = 0
	SELECT     @totalRLClientfk = COUNT(*)
	FROM         T_RoutingLogicCompanyClient
	WHERE     (RLID_FK = @RLID) 
	
	SET @totalRLGroupfk = 0
	 SELECT     @totalRLGroupfk = COUNT(*)
	                           FROM         T_RoutingLogicClientGroupRL
	                           WHERE      (RLID_FK = @RLID)
	
	
	
	---selecting insert or update
	
	IF( (@totalRLpk = 0) OR  ((@totalRLClientfk + @totalRLGroupfk) > 0) )
	BEGIN
		--Insert Data
		--  inserting in main table
		INSERT INTO T_RoutingLogic
		                      (Name, AUECID_FK, DefaultCompanyTradingAccountID_FK)
		VALUES     (@RLName, @AUECID, @TradingAccountDefaultID)
			
			SET @result = scope_identity()
			
			
			----if  fk exist then do this
			
			UPDATE    T_RoutingLogic
	SET              Name = ''
	WHERE     (RLID = @RLID)			
			
			
			SET @RLID = @result
			
			
	END
	ELSE
	BEGIN
			-- updating main table
	
	UPDATE    T_RoutingLogic
	SET              Name = @RLName, AUECID_FK = @AUECID, DefaultCompanyTradingAccountID_FK = @TradingAccountDefaultID
	WHERE     (RLID = @RLID)
	        SET @result = @RLID	
	
	END
	
	-----   param,eters saving.---conditions
	
	IF(	( @ParameterID0 >=0) )
	BEGIN

	EXEC dbo.P_SaveRLConditions  @RLID, @ParameterID0, @ParameterValue0	, @JoinCondition0, @OperatorID0 , @Sequence = 1
	END
	ELSE
	BEGIN
	EXEC dbo.P_DeleteRLConditions @RLID, @Sequence = 1
	END
	
	IF(	( @ParameterID1 >=0))
	BEGIN
	
	EXEC dbo.P_SaveRLConditions  @RLID, @ParameterID1, @ParameterValue1	, @JoinCondition1, @OperatorID1 , @Sequence = 2
	END
	ELSE
	BEGIN
	EXEC dbo.P_DeleteRLConditions @RLID, @Sequence = 2
	END
	
	IF(	( @ParameterID2 >=0))
	BEGIN
	
	EXEC dbo.P_SaveRLConditions  @RLID, @ParameterID2, @ParameterValue2	, @JoinCondition2, @OperatorID2 , @Sequence = 3
	END
	ELSE
	BEGIN
	EXEC dbo.P_DeleteRLConditions @RLID, @Sequence = 3
	END
	
	---trading account
	
	
	
	IF(	( @TradingAccountID0 >=0) OR (( @CounterPartyID0 >=0) AND ( @VenueID0 >=0)))
		BEGIN
	SET @CounterPartyVenueID =-1
	SELECT     @CounterPartyVenueID = CounterPartyVenueID
	           FROM         T_CounterPartyVenue
	           WHERE     (CounterPartyID = @CounterPartyID0) AND (VenueID = @VenueID0) 
	EXEC dbo.P_SaveRLVenues  @RLID, @TradingAccountID0, @CounterPartyVenueID  , @Rank =1
		END
	ELSE
		BEGIN
	EXEC dbo.P_DeleteRLVenues @RLID, @Rank = 1
		END
	
	IF(	( @TradingAccountID1 >=0) OR (( @CounterPartyID1 >=0) AND ( @VenueID1 >=0)))
		BEGIN
	SET @CounterPartyVenueID =-1
	SELECT     @CounterPartyVenueID = CounterPartyVenueID
	       FROM         T_CounterPartyVenue
	           WHERE     (CounterPartyID = @CounterPartyID1) AND (VenueID = @VenueID1) 
	EXEC dbo.P_SaveRLVenues  @RLID, @TradingAccountID1, @CounterPartyVenueID  , @Rank =2
		END
	ELSE
		BEGIN
	EXEC dbo.P_DeleteRLVenues @RLID, @Rank = 2
		END
	
	IF(	(@TradingAccountID2 >=0) OR ((@CounterPartyID2 >=0) AND (@VenueID2 >=0)))
		BEGIN
	SET @CounterPartyVenueID =-1
	SELECT     @CounterPartyVenueID = CounterPartyVenueID
	           FROM         T_CounterPartyVenue
	           WHERE     (CounterPartyID = @CounterPartyID2) AND (VenueID = @VenueID2) 
	EXEC dbo.P_SaveRLVenues  @RLID, @TradingAccountID2, @CounterPartyVenueID  , @Rank =3
		END
	ELSE
		BEGIN
	EXEC dbo.P_DeleteRLVenues @RLID, @Rank = 3
		END
	
	
	
	
		
				
select @result	



