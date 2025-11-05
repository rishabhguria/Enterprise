

CREATE PROCEDURE dbo.P_DeleteRL
	(
		@RLID	int,
		@DeleteForceFully int
	)
AS
	
	DECLARE @totalRLpk int, @totalRLClientfk int,@totalRLGroupfk int, @Sequence int, @Rank int, @RLName varchar(50)
	
		
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
	                           
	SET @RLName = ''
	SELECT     @RLName = Name
	FROM         T_RoutingLogic
	WHERE     (RLID = @RLID) 	                           
	
	IF((@totalRLpk > 0))
	BEGIN
	
	IF( ((@totalRLClientfk + @totalRLGroupfk) = 0 )  AND(  (@RLName = '')  OR (@DeleteForceFully =1)))
		BEGIN
		EXEC dbo.P_DeleteRLConditions @RLID, @Sequence = 1
		EXEC dbo.P_DeleteRLConditions @RLID, @Sequence = 2
		EXEC dbo.P_DeleteRLConditions @RLID, @Sequence = 3
		
		EXEC dbo.P_DeleteRLVenues @RLID, @Rank = 1
		EXEC dbo.P_DeleteRLVenues @RLID, @Rank = 2
		EXEC dbo.P_DeleteRLVenues @RLID, @Rank = 3
		
		DELETE FROM T_RoutingLogic
		WHERE     (RLID = @RLID) 
		
		END
		ELSE IF ( @DeleteForceFully =1)
		BEGIN
		
		UPDATE    T_RoutingLogic
		SET              Name = ''
		WHERE     (RLID = @RLID)
		
		
		END
		END
		
		
	select @RLID
		

