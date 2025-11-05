


CREATE PROCEDURE dbo.P_SaveRLConditions
	(
		@RLID	int,
		@ParameterID	int,
		@ParameterValue	varchar(50),
		@JoinCondition	int,
		@OperatorID int,	
		@Sequence int
	)
AS
	
	Declare @total int
	set @total = 0
	
	SELECT     @total = COUNT(*)
	FROM         T_RoutingLogicCondition
	WHERE     (RLID_FK = @RLID) AND (Sequence = @Sequence)
	
	if(@total = 0)
	Begin
		INSERT INTO T_RoutingLogicCondition
		                      (RLID_FK, ParameterID_FK, ParameterValue, Sequence, JoinCondition, OperatorID_FK)
		VALUES     (@RLID, @ParameterID, @ParameterValue, @Sequence, @JoinCondition, @OperatorID )
		
	End
	Else
	Begin	
	
	UPDATE    T_RoutingLogicCondition
	SET              ParameterValue = @ParameterValue, ParameterID_FK = @ParameterID, JoinCondition = @JoinCondition, OperatorID_FK = @OperatorID
	WHERE     (RLID_FK = @RLID) AND (Sequence = @Sequence)
	end 


