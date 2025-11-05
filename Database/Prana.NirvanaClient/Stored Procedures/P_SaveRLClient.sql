CREATE PROCEDURE dbo.P_SaveRLClient

	(
		@ClientID           int,  

		@ApplyRL          int,  
      
		@RLIDHSV         varchar(8000),
		
		@AUECID int,
		
		@CompanyID int
		
	)

AS
	 SET NOCOUNT ON 
	 
	 
	 DECLARE  @Rank int,  @result int,  @RLID varchar(10), @PosRL int, @Seperator char,  @GroupID int
	SET ANSI_NULLS OFF
	
IF ( @ClientID<0)
BEGIN

SET @result = -1

END

ELSE

BEGIN

DELETE FROM T_RoutingLogicCompanyClient
WHERE     ((CompanyClientID_FK = @ClientID) AND RLID_FK IN (SELECT     T_RoutingLogic.RLID
                                                            FROM         T_RoutingLogic INNER JOIN
                                                                                  T_CompanyAUEC ON T_RoutingLogic.AUECID_FK = T_CompanyAUEC.AUECID
                                                            WHERE     (T_RoutingLogic.AUECID_FK = @AUECID) AND (T_CompanyAUEC.CompanyID = @CompanyID))) 


	
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
			
				
				/**
				SELECT     @AUECID=AUECID_FK 
				    FROM         T_RoutingLogic
				    WHERE     (RLID = CAST(@RLID AS int))
				    **/
				    
				    
				    
				    
				    INSERT INTO T_RoutingLogicCompanyClient
				                      (CompanyClientID_FK, RLID_FK, Rank, ApplyRL)
				VALUES     (@ClientID, CAST(@RLID AS int), @Rank, @ApplyRL)
				
				
	DELETE FROM T_RoutingLogicGroupClient
WHERE     ((CompanyClientID_FK = @ClientID) AND ClientGroupID_FK IN (SELECT     T_RoutingLogicClientGroupRL.ClientGroupID_FK
                                                                     FROM         T_RoutingLogic INNER JOIN
                                                                                           T_RoutingLogicClientGroupRL ON T_RoutingLogic.RLID = T_RoutingLogicClientGroupRL.RLID_FK INNER JOIN
                                                                                           T_CompanyAUEC ON T_RoutingLogic.AUECID_FK = T_CompanyAUEC.AUECID
                                                                     WHERE     (T_RoutingLogic.AUECID_FK = @AUECID) AND (T_CompanyAUEC.CompanyID = @CompanyID)))
				
				/***
				DECLARE c CURSOR
				FOR	 SELECT    T_RoutingLogicGroupRL.ClientGroupID_FK AS GroupID
							FROM         T_RoutingLogic INNER JOIN
				                      T_RoutingLogicGroupRL ON T_RoutingLogic.RLID = T_RoutingLogicGroupRL.RLID_FK
							WHERE     (T_RoutingLogic.AUECID_FK = @AUECID)
				
				
				OPEN c
				FETCH c INTO @GroupID

					WHILE (@@FETCH_STATUS=0) 
					BEGIN	
					
					DELETE FROM T_RoutingLogicGroupClient
							WHERE     (CompanyClientID_FK = @ClientID) AND (ClientGroupID_FK=@GroupID)

					FETCH c INTO @AUECID
					END
	
				CLOSE c
				DEALLOCATE c
			***/
		/**		DELETE FROM T_RoutingLogicGroupClient
WHERE     (CompanyClientID_FK IN
                          (SELECT     CompanyClientID_FK 
                            FROM          T_RoutingLogicCompanyClient)) 
				**/
			/**
				DECLARE c CURSOR
				FOR SELECT     AUECID_FK AS AUECID
				    FROM         T_RoutingLogic
				    WHERE     (RLID = CAST(@RLID AS int))

				OPEN c
				FETCH c INTO @AUECID

					WHILE (@@FETCH_STATUS=0) 
					BEGIN	
					
					
					
	
					END					
		
				FETCH c INTO @AUECID
				END
	
				CLOSE c
				DEALLOCATE c
				
				**/
				
			END
			
			SET	@RLIDHSV = RIGHT(@RLIDHSV, LEN(@RLIDHSV) - @PosRL)

			SET @PosRL = CHARINDEX(@Seperator, @RLIDHSV, 1)

		END
	END		 
	
	

END

SET @result = @ClientID

SELECT @result

Return @result
