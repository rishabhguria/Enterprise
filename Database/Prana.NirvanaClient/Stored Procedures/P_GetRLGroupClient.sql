CREATE PROCEDURE dbo.P_GetRLGroupClient
	(
		@ClientID int,
		@GroupID int,
		@AUECID int
	)
AS


IF(@ClientID<0)
BEGIN
----group
SELECT     DISTINCT ClientGroupID AS ID, Name, ApplyRL
FROM         T_RoutingLogicClientGroup
WHERE     (ClientGroupID = @GroupID)

SELECT     RLID_FK AS RLID, Rank
FROM         T_RoutingLogicClientGroupRL
WHERE     (ClientGroupID_FK = @GroupID)

END

ELSE

BEGIN
---client

SELECT DISTINCT T_CompanyClient.CompanyClientID AS ID, T_CompanyClient.ClientName AS Name, T_RoutingLogicCompanyClient.ApplyRL
FROM         T_CompanyClient INNER JOIN
                      T_RoutingLogicCompanyClient ON T_CompanyClient.CompanyClientID = T_RoutingLogicCompanyClient.CompanyClientID_FK INNER JOIN
                      T_RoutingLogic ON T_RoutingLogicCompanyClient.RLID_FK = T_RoutingLogic.RLID
WHERE     (T_CompanyClient.CompanyClientID = @ClientID) AND (T_RoutingLogic.AUECID_FK = @AUECID)

SELECT     T_RoutingLogicCompanyClient.RLID_FK AS RLID, T_RoutingLogicCompanyClient.Rank
FROM         T_RoutingLogicCompanyClient INNER JOIN
                      T_RoutingLogic ON T_RoutingLogicCompanyClient.RLID_FK = T_RoutingLogic.RLID
WHERE     (T_RoutingLogicCompanyClient.CompanyClientID_FK = @ClientID) AND (T_RoutingLogic.AUECID_FK = @AUECID)



END
