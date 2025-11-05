CREATE PROCEDURE dbo.P_GetRLList
(
	@Dummy int
)
AS


SELECT     RLID, Name AS RLName, AUECID_FK AS AUECID
FROM         T_RoutingLogic
WHERE     (Name <> '') OR
                      (Name <> NULL)
ORDER BY Name, RLID, AUECID_FK
