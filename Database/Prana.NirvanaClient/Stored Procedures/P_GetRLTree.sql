CREATE PROCEDURE dbo.P_GetRLTree
(
	@CompanyID int
)
AS
	declare @PrefixRL char(2)
		set @PrefixRL = 'r:'
	declare @PrefixClient char(2)
		set @PrefixClient = 'c:'
	declare @PrefixGroup char(2)
		set @PrefixGroup = 'g:'
	declare @Seperator char(1)
		set @Seperator = ':'

create table #tempTableRLTree (KeyID varchar(50), NodeName varchar(500))

--create table #tempTableRLValues (KeyID varchar(50), NodeName varchar(500))

insert into #tempTableRLTree SELECT DISTINCT @PrefixRL + CAST(T_RoutingLogic.RLID AS char(5)) AS KeyID, T_RoutingLogic.Name AS NodeName
FROM         T_RoutingLogic INNER JOIN
                      T_AUEC ON T_RoutingLogic.AUECID_FK = T_AUEC.AUECID INNER JOIN
                      T_CompanyAUEC ON T_AUEC.AUECID = T_CompanyAUEC.AUECID
WHERE     (T_RoutingLogic.Name <> '' OR
                      T_RoutingLogic.Name <> NULL) AND (T_CompanyAUEC.CompanyID = @CompanyID)
 

insert into #tempTableRLTree 
SELECT DISTINCT 
                      @PrefixClient + CAST(T_CompanyClient.CompanyClientID AS char(5)) + @Seperator + CAST(T_RoutingLogic.AUECID_FK AS char(5)) AS KeyID, 
                      T_CompanyClient.ClientName AS NodeName
FROM         T_RoutingLogicCompanyClient INNER JOIN
                      T_CompanyClient ON T_RoutingLogicCompanyClient.CompanyClientID_FK = T_CompanyClient.CompanyClientID INNER JOIN
                      T_RoutingLogic ON T_RoutingLogicCompanyClient.RLID_FK = T_RoutingLogic.RLID INNER JOIN
                      T_AUEC ON T_RoutingLogic.AUECID_FK = T_AUEC.AUECID INNER JOIN
                      T_CompanyAUEC ON T_AUEC.AUECID = T_CompanyAUEC.AUECID
WHERE     (T_CompanyAUEC.CompanyID = @CompanyID) AND (T_CompanyClient.CompanyID = @CompanyID)

/**WHERE     (T_RoutingLogicCompanyClient.CompanyClientID_FK NOT IN
                          (SELECT     CompanyClientID_FK
                            FROM          T_RoutingLogicGroupClient)) **/
                        
insert into #tempTableRLTree 
SELECT DISTINCT @PrefixGroup + CAST(T_RoutingLogicClientGroup.ClientGroupID AS char(5)) AS KeyID, T_RoutingLogicClientGroup.Name AS NodeName
FROM         T_RoutingLogicClientGroup INNER JOIN
                      T_RoutingLogicClientGroupRL ON T_RoutingLogicClientGroup.ClientGroupID = T_RoutingLogicClientGroupRL.ClientGroupID_FK INNER JOIN
                      T_RoutingLogic ON T_RoutingLogicClientGroupRL.RLID_FK = T_RoutingLogic.RLID INNER JOIN
                      T_CompanyAUEC ON T_RoutingLogic.AUECID_FK = T_CompanyAUEC.AUECID INNER JOIN
                      T_RoutingLogicGroupClient ON T_RoutingLogicClientGroup.ClientGroupID = T_RoutingLogicGroupClient.ClientGroupID_FK INNER JOIN
                      T_CompanyClient ON T_RoutingLogicGroupClient.CompanyClientID_FK = T_CompanyClient.CompanyClientID
WHERE     (T_CompanyAUEC.CompanyID = @CompanyID) AND (T_CompanyClient.CompanyID = @CompanyID)




SELECT * FROM  #tempTableRLTree ORDER BY NodeName

drop table #tempTableRLTree
--drop table #tempTableRLValues

RETURN 1