CREATE PROCEDURE dbo.P_GetRLTree1
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

SELECT DISTINCT @PrefixRL + CAST(RLID AS char(5)) AS KeyID, Name AS NodeName
FROM         T_RoutingLogic
WHERE     (Name <> '') OR
                      (Name <> NULL)

UNION ALL
SELECT DISTINCT 
                      @PrefixClient + CAST(T_CompanyClient.CompanyClientID AS char(5)) + @Seperator + CAST(T_RoutingLogic.AUECID_FK AS char(5)) AS KeyID, 
                      T_CompanyClient.ClientName AS NodeName
FROM         T_RoutingLogicCompanyClient INNER JOIN
                      T_CompanyClient ON T_RoutingLogicCompanyClient.CompanyClientID_FK = T_CompanyClient.CompanyClientID INNER JOIN
                      T_RoutingLogic ON T_RoutingLogicCompanyClient.RLID_FK = T_RoutingLogic.RLID

/**WHERE     (T_RoutingLogicCompanyClient.CompanyClientID_FK NOT IN
                          (SELECT     CompanyClientID_FK
                            FROM          T_RoutingLogicGroupClient)) **/
UNION ALL
SELECT DISTINCT @PrefixGroup + CAST(ClientGroupID AS char(5)) AS KeyID, Name AS NodeName
FROM         T_RoutingLogicClientGroup
ORDER BY NodeName, KeyID