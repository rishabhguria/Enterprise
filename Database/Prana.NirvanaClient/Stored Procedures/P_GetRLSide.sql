

CREATE PROCEDURE dbo.P_GetRLSide
(
		@AUECID int
)
AS
SELECT     DISTINCT T_Side.SideID, T_Side.Side
FROM         T_Side INNER JOIN
                      T_AUECSide ON T_Side.SideID = T_AUECSide.SideID
WHERE     (T_AUECSide.AUECID = @AUECID)
ORDER BY T_Side.Side

