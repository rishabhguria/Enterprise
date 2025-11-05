

CREATE PROCEDURE dbo.P_SaveRLogic

	(
		@RLID           int,  
		@RLName             varchar(99),
		@AUECID          int,  
		@TradingAccountDefaultID       int,      
		@ParameterIDHSV         varchar(8000),
		@ParameterValueHSV       varchar(8000),     
		@JoinConditionHSV        varchar(8000) ,
		@OperatorIDHSV          varchar(8000)   , 
		@TradingAccIDHSV         varchar(8000)   ,    
		@CounterPartyVenueIDHSV    varchar(8000)        
	)

AS
	 SET NOCOUNT ON 
	 
	 
	 DECLARE @totalRLpk int, @totalRLClientfk int,@totalRLGroupfk int,  @result int
	SET ANSI_NULLS OFF
	
	IF(@RLID<0)
	BEGIN
	SET @totalRLpk = 0
	SET @totalRLClientfk = 0
	SET @totalRLGroupfk = 0
	
	END
	ELSE
	BEGIN		
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
	
	
	END
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
	 
	 
	 DELETE FROM T_RoutingLogicCondition
		WHERE     (RLID_FK = @RLID)
		
		
	DECLARE @ParameterID varchar(10),@ParameterValue varchar(50),@JoinID varchar(10),@OperatorID varchar(10), @PosParameterID int, @PosParameterValue int, @PosJoin int, @PosOperator int, @Seperator char, @Rank int
	
	SET @Seperator = '#'
	
	SET @ParameterIDHSV = LTRIM(RTRIM(@ParameterIDHSV))+ @Seperator
	SET	@ParameterValueHSV = LTRIM(RTRIM(@ParameterValueHSV))+ @Seperator
	SET	@JoinConditionHSV = LTRIM(RTRIM(@JoinConditionHSV))+ @Seperator
	SET	@OperatorIDHSV   = LTRIM(RTRIM(@OperatorIDHSV))+ @Seperator
	  
	SET @PosParameterID = CHARINDEX(@Seperator, @ParameterIDHSV, 1)
	SET @PosParameterValue = CHARINDEX(@Seperator, @ParameterValueHSV, 1)
	SET @PosJoin = CHARINDEX(@Seperator, @JoinConditionHSV, 1)
	SET @PosOperator = CHARINDEX(@Seperator, @OperatorIDHSV, 1)
	
	SET @Rank=0

	IF (REPLACE(@ParameterIDHSV, @Seperator, '') <> '')
	BEGIN
		WHILE @PosParameterID > 0
		BEGIN
		
		
			SET @ParameterID = LTRIM(RTRIM(LEFT(@ParameterIDHSV, @PosParameterID - 1)))
			SET	@ParameterValue = LTRIM(RTRIM(LEFT(@ParameterValueHSV, @PosParameterValue - 1)))
			SET	@JoinID = LTRIM(RTRIM(LEFT(@JoinConditionHSV, @PosJoin - 1)))
			SET	@OperatorID   = LTRIM(RTRIM(LEFT(@OperatorIDHSV, @PosOperator - 1)))
			
			SET @Rank = @Rank + 1
			
			
			IF ((@ParameterID <> '') AND (CAST(@ParameterID AS int)>=0))
			BEGIN
			
				INSERT INTO T_RoutingLogicCondition
				                      (RLID_FK, ParameterID_FK, ParameterValue, Sequence, JoinCondition, OperatorID_FK)
				VALUES     (@RLID, CAST(@ParameterID AS int), @ParameterValue, @Rank, CAST(@JoinID AS int),CAST(@OperatorID AS int) )
				
			END
			
			
			SET @ParameterIDHSV = RIGHT(@ParameterIDHSV, LEN(@ParameterIDHSV) - @PosParameterID)
			SET	@ParameterValueHSV = RIGHT(@ParameterValueHSV, LEN(@ParameterValueHSV) - @PosParameterValue)
			SET	@JoinConditionHSV = RIGHT(@JoinConditionHSV, LEN(@JoinConditionHSV) - @PosJoin)
			SET	@OperatorIDHSV   = RIGHT(@OperatorIDHSV, LEN(@OperatorIDHSV) - @PosOperator)
			
			
			SET @PosParameterID = CHARINDEX(@Seperator, @ParameterIDHSV, 1)
			SET @PosParameterValue = CHARINDEX(@Seperator, @ParameterValueHSV, 1)
			SET @PosJoin = CHARINDEX(@Seperator, @JoinConditionHSV, 1)
			SET @PosOperator = CHARINDEX(@Seperator, @OperatorIDHSV, 1)
			
			

		END
	END	
	 
	 
	 
	 
	 
	 -----   trading accoount counterparty venue
	 
	 
	 DELETE FROM T_RoutingLogicVenues
		WHERE     (RLID_FK = @RLID)
		
		
	DECLARE @TradingAccountID varchar(10), @CounterPartyVenueID varchar(10), @PosTA int, @PosCPV int
	
	SET @Seperator = '#'

	SET	@TradingAccIDHSV = LTRIM(RTRIM(@TradingAccIDHSV))+ @Seperator
	SET	@CounterPartyVenueIDHSV   = LTRIM(RTRIM(@CounterPartyVenueIDHSV))+ @Seperator
	  
	SET @PosTA = CHARINDEX(@Seperator, @TradingAccIDHSV, 1)
	SET @PosCPV = CHARINDEX(@Seperator, @CounterPartyVenueIDHSV, 1)
	
	SET @Rank=0

	IF (REPLACE(@TradingAccIDHSV, @Seperator, '') <> '')
	BEGIN
		WHILE @PosTA > 0
		BEGIN
		
		
			SET	@TradingAccountID = LTRIM(RTRIM(LEFT(@TradingAccIDHSV, @PosTA - 1)))
			SET	@CounterPartyVenueID   = LTRIM(RTRIM(LEFT(@CounterPartyVenueIDHSV, @PosCPV - 1)))
			
			SET @Rank = @Rank + 1
			
			
			IF ((@TradingAccountID <> ''  AND @CounterPartyVenueID<> '') AND (CAST(@TradingAccountID AS int) * CAST(@CounterPartyVenueID AS int)<=0))
			BEGIN
			
				IF(CAST(@TradingAccountID AS int)<0)
				BEGIN
				INSERT INTO T_RoutingLogicVenues
				                      (RLID_FK, CompanyTradingAccountID_FK, CompanyCounterPartyVenueID_FK, Rank)
				VALUES     (@RLID, NULL , CAST(@CounterPartyVenueID AS int), @Rank)
				END
				ELSE
				BEGIN
			
				INSERT INTO T_RoutingLogicVenues
				                      (RLID_FK, CompanyTradingAccountID_FK, CompanyCounterPartyVenueID_FK, Rank)
				VALUES     (@RLID, CAST(@TradingAccountID AS int), NULL, @Rank)
				END
				
			END
			
			

			SET	@TradingAccIDHSV = RIGHT(@TradingAccIDHSV, LEN(@TradingAccIDHSV) - @PosTA)
			SET	@CounterPartyVenueIDHSV   = RIGHT(@CounterPartyVenueIDHSV, LEN(@CounterPartyVenueIDHSV) - @PosCPV)
			
			

			SET @PosTA = CHARINDEX(@Seperator, @TradingAccIDHSV, 1)
			SET @PosCPV = CHARINDEX(@Seperator, @CounterPartyVenueIDHSV, 1)
			
			

		END
	END		 

	 
	 
	 select @result	
	 
	RETURN 


