

CREATE PROCEDURE dbo.P_DeleteRLConditions
	(
		@RLID	int,
		@Sequence int
	)
AS
	
	Declare @total int
	set @total = 0
	
	SELECT     @total = COUNT(*)
	FROM         T_RoutingLogicCondition
	WHERE     (RLID_FK = @RLID) AND (Sequence = @Sequence)
	if(@total > 0)
	Begin
		DELETE FROM T_RoutingLogicCondition
		WHERE     (RLID_FK = @RLID) AND (Sequence = @Sequence)
		
	End

	

