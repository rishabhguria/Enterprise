

CREATE PROCEDURE dbo.P_DeleteRLGroup
	(
		@GroupID	int		
	)
AS
/*****
DECLARE @RLID0 int, @RLID1 int, @RLID2 int, @DeleteForceFully int

SELECT     RLID_FK
FROM         T_RoutingLogicClientGroupRL
WHERE     (ClientGroupID_FK = @GroupID) AND (Rank = 1)  

SELECT     @RLID1 = RLID_FK
FROM         T_RoutingLogicClientGroupRL
WHERE     (ClientGroupID_FK = @GroupID) AND (Rank = 2)

SELECT     @RLID2 = RLID_FK
FROM         T_RoutingLogicClientGroupRL
WHERE     (ClientGroupID_FK = @GroupID) AND (Rank = 3)

******/

DELETE FROM T_RoutingLogicGroupClient
WHERE     (ClientGroupID_FK = @GroupID)

DELETE FROM T_RoutingLogicClientGroupRL
WHERE     (ClientGroupID_FK = @GroupID)

DELETE FROM T_RoutingLogicClientGroup
WHERE     (ClientGroupID = @GroupID)


SELECT @GroupID
/****

SET @DeleteForceFully=0

EXEC dbo.P_DeleteRL @RLID0, @DeleteForceFully
EXEC dbo.P_DeleteRL @RLID1, @DeleteForceFully
EXEC dbo.P_DeleteRL @RLID2, @DeleteForceFully

****/

