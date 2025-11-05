

CREATE PROCEDURE dbo.P_SaveRLGroup

	(
		@GroupID           int,  
		@Name	varchar(50),
		@ApplyRLGroup          int,  
      
		@ClientIDHSV         varchar(8000),
		@ApplyRLHSV         varchar(8000),
		@RLIDHSV         varchar(8000)
		
	)

AS
	 SET NOCOUNT ON 
	 
	 
	 DECLARE    @result int,  @ClientID varchar(10), @PosClient int, @ApplyRL varchar(10), @PosApplyRL int,  @RLID varchar(10), @PosRL int, @Seperator char, @Rank int
	SET ANSI_NULLS OFF
	
	
	IF(@GroupID<0)
	BEGIN
	
	INSERT INTO T_RoutingLogicClientGroup
	                      (Name, ApplyRL)
	VALUES     (@Name, @ApplyRLGroup)
	
		SET @result = scope_identity()
		SET @GroupID = @result
	END
	ELSE
	BEGIN
	UPDATE    T_RoutingLogicClientGroup
	SET              Name = @Name, ApplyRL = @ApplyRLGroup
	WHERE     (ClientGroupID = @GroupID)
	
	SET @result = @GroupID
	
	
	END
	-----  group client
	
DELETE FROM T_RoutingLogicGroupClient
WHERE     (ClientGroupID_FK = @GroupID)


	
	SET @Seperator = '#'

	SET	@ClientIDHSV = LTRIM(RTRIM(@ClientIDHSV))+ @Seperator
	SET	@ApplyRLHSV = LTRIM(RTRIM(@ApplyRLHSV))+ @Seperator
	
  
	SET @PosClient = CHARINDEX(@Seperator, @ClientIDHSV, 1)
	SET @PosApplyRL = CHARINDEX(@Seperator, @ApplyRLHSV, 1)


	IF REPLACE(@ClientIDHSV, @Seperator, '') <> ''
	BEGIN
		WHILE @PosClient > 0
		BEGIN		
		
			SET	@ClientID = LTRIM(RTRIM(LEFT(@ClientIDHSV, @PosClient - 1)))
			SET	@ApplyRL = LTRIM(RTRIM(LEFT(@ApplyRLHSV, @PosApplyRL - 1)))
			
			IF (@ClientID <> '' )
			BEGIN
			
				INSERT INTO T_RoutingLogicGroupClient
				                      (ClientGroupID_FK, CompanyClientID_FK, ApplyRL)
				VALUES     (@GroupID, CAST(@ClientID AS int), CAST(@ApplyRL AS int))
				
			END
			
			SET	@ClientIDHSV = RIGHT(@ClientIDHSV, LEN(@ClientIDHSV) - @PosClient)
			SET	@ApplyRLHSV = RIGHT(@ApplyRLHSV, LEN(@ApplyRLHSV) - @PosApplyRL)

			SET @PosClient = CHARINDEX(@Seperator, @ClientIDHSV, 1)			
			SET @PosApplyRL = CHARINDEX(@Seperator, @ApplyRLHSV, 1)

		END
	END		 
	
	
-----grp rl


DELETE FROM T_RoutingLogicClientGroupRL
WHERE     (ClientGroupID_FK = @GroupID)

	
	SET @Seperator = '#'

	SET	@RLIDHSV = LTRIM(RTRIM(@RLIDHSV))+ @Seperator

	  
	SET @PosRL = CHARINDEX(@Seperator, @RLIDHSV, 1)

	
	SET @Rank=0

	IF REPLACE(@RLIDHSV, @Seperator, '') <> ''
	BEGIN
		WHILE @PosRL > 0
		BEGIN		
		
			SET	@RLID = LTRIM(RTRIM(LEFT(@RLIDHSV, @PosRL - 1)))
			
			SET @Rank = @Rank + 1			
			
			IF (@RLID <> '' )
			BEGIN
			INSERT INTO T_RoutingLogicClientGroupRL
			                      (ClientGroupID_FK, RLID_FK, Rank)
			VALUES     (@GroupID, CAST(@RLID AS int), @Rank)
				
			END
			
			SET	@RLIDHSV = RIGHT(@RLIDHSV, LEN(@RLIDHSV) - @PosRL)

			SET @PosRL = CHARINDEX(@Seperator, @RLIDHSV, 1)

		END
	END		 
	


SELECT @result


