


CREATE PROCEDURE dbo.P_DeleteRLVenues
	(
		@RLID	int,	
		@Rank int
	)
AS
	
	Declare @total int
	set @total = 0
	
		SELECT     @total = COUNT(*)
	FROM         T_RoutingLogicVenues
	WHERE     (RLID_FK = @RLID) AND (Rank = @Rank)
	if(@total > 0)
	Begin
		DELETE FROM T_RoutingLogicVenues
		WHERE     (RLID_FK = @RLID) AND (Rank = @Rank)
		
	End


