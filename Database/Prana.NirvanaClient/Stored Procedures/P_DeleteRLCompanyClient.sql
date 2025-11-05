CREATE PROCEDURE dbo.P_DeleteRLCompanyClient
	(
		@ClientID	int,
		@AUECID int ,
		@CompanyID int		
	)
AS
/***
DECLARE @RLID0 int, @RLID1 int, @RLID2 int, @DeleteForceFully int


SELECT     @RLID0 = RLID_FK
FROM         T_RoutingLogicCompanyClient
WHERE     (CompanyClientID_FK = @ClientID) AND (Rank = 1)

SELECT     @RLID1 = RLID_FK
FROM         T_RoutingLogicCompanyClient
WHERE     (CompanyClientID_FK = @ClientID) AND (Rank = 2)

SELECT     @RLID2 = RLID_FK
FROM         T_RoutingLogicCompanyClient
WHERE     (CompanyClientID_FK = @ClientID) AND (Rank = 3)

--
DELETE FROM T_RoutingLogicCompanyClient
WHERE     (CompanyClientID_FK = @ClientID)

**/

 DELETE FROM T_RoutingLogicCompanyClient
WHERE     ((CompanyClientID_FK IN ( SELECT     CompanyClientID
                                 FROM         T_CompanyClient
                                 WHERE     (CompanyClientID = @ClientID) AND (CompanyID = @CompanyID))) AND RLID_FK IN (SELECT     T_RoutingLogic.RLID
                                                            FROM         T_RoutingLogic INNER JOIN
                                                                                  T_CompanyAUEC ON T_RoutingLogic.AUECID_FK = T_CompanyAUEC.AUECID
                                                            WHERE     (T_RoutingLogic.AUECID_FK = @AUECID) AND (T_CompanyAUEC.CompanyID = @CompanyID))) 

/***
SET @DeleteForceFully=0

EXEC dbo.P_DeleteRL @RLID0, @DeleteForceFully
EXEC dbo.P_DeleteRL @RLID1, @DeleteForceFully
EXEC dbo.P_DeleteRL @RLID2, @DeleteForceFully

***/

SELECT @ClientID, @AUECID