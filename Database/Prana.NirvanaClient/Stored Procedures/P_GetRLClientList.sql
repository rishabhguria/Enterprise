CREATE PROCEDURE dbo.P_GetRLClientList
	(
		@CompanyID int,
		@GroupID int
	)
AS

DECLARE @AUECID int, @ClientID int
create table #tempTableRLClientList (ClientID int, ClientName varchar(50),  AUECID int, ApplyRL int, Checked int)


insert into #tempTableRLClientList SELECT     T_CompanyClient.CompanyClientID AS ClientID, T_CompanyClient.ClientName, T_CompanyAUEC.AUECID, 0 AS ApplyRL, 0 AS Checked
                                   FROM         T_CompanyClient INNER JOIN
                                                         T_CompanyClientAUEC ON T_CompanyClient.CompanyClientID = T_CompanyClientAUEC.CompanyClientID INNER JOIN
                                                         T_CompanyAUEC ON T_CompanyClientAUEC.CompanyAUECID = T_CompanyAUEC.CompanyAUECID
                                   WHERE     (T_CompanyAUEC.CompanyID = @CompanyID)
                                   ORDER BY T_CompanyClient.ClientName, T_CompanyAUEC.AUECID
              --              
UPDATE    #tempTableRLClientList
SET              ApplyRL = 1, Checked = 1
WHERE (ClientID in (SELECT     CompanyClientID_FK AS ClientID
                    FROM         T_RoutingLogicGroupClient
                    WHERE     (ApplyRL = 1) AND (ClientGroupID_FK = @GroupID)))    
                    
UPDATE    #tempTableRLClientList
SET              ApplyRL = 0, Checked = 1
WHERE (ClientID in (SELECT     CompanyClientID_FK AS ClientID
                    FROM         T_RoutingLogicGroupClient
                    WHERE     (ApplyRL = 0) AND (ClientGroupID_FK = @GroupID)))       
                    
              --                      
                            
                DECLARE c CURSOR
				FOR SELECT DISTINCT T_RoutingLogic.AUECID_FK AS AUECID, T_CompanyClient.CompanyClientID AS ClientID
				    FROM         T_RoutingLogicClientGroup INNER JOIN
				                          T_RoutingLogicClientGroupRL ON T_RoutingLogicClientGroup.ClientGroupID = T_RoutingLogicClientGroupRL.ClientGroupID_FK INNER JOIN
				                          T_RoutingLogic ON T_RoutingLogicClientGroupRL.RLID_FK = T_RoutingLogic.RLID INNER JOIN
				                          T_RoutingLogicGroupClient ON T_RoutingLogicClientGroup.ClientGroupID = T_RoutingLogicGroupClient.ClientGroupID_FK INNER JOIN
				                          T_CompanyClient ON T_RoutingLogicGroupClient.CompanyClientID_FK = T_CompanyClient.CompanyClientID
				    WHERE     (T_RoutingLogicClientGroup.ClientGroupID <> @GroupID) AND (T_CompanyClient.CompanyID = @CompanyID)
				OPEN c
				FETCH c INTO @AUECID, @ClientID
					WHILE (@@FETCH_STATUS=0) 
					BEGIN						
/**					SELECT     @AUECID = T_RoutingLogic.AUECID_FK
FROM         T_RoutingLogic INNER JOIN
                      T_RoutingLogicClientGroupRL ON T_RoutingLogic.RLID = T_RoutingLogicClientGroupRL.RLID_FK
WHERE     (T_RoutingLogicClientGroupRL.ClientGroupID_FK = @GroupID)
**/                    
					DELETE FROM #tempTableRLClientList
WHERE     (ClientID  =@ClientID AND AUECID=@AUECID)   								
		
					FETCH c INTO @AUECID, @ClientID
					END	
				CLOSE c
				DEALLOCATE c 
				
				
		--		                     
				                     

			    DECLARE c CURSOR
				FOR SELECT DISTINCT T_RoutingLogic.AUECID_FK AS AUECID, T_CompanyClient.CompanyClientID AS ClientID
				    FROM         T_RoutingLogic INNER JOIN
				                          T_RoutingLogicCompanyClient ON T_RoutingLogic.RLID = T_RoutingLogicCompanyClient.RLID_FK INNER JOIN
				                          T_CompanyClient ON T_RoutingLogicCompanyClient.CompanyClientID_FK = T_CompanyClient.CompanyClientID
				    WHERE     (T_CompanyClient.CompanyID = @CompanyID)
				OPEN c
				FETCH c INTO @AUECID, @ClientID
					WHILE (@@FETCH_STATUS=0) 
					BEGIN						
          
					DELETE FROM #tempTableRLClientList
WHERE     (ClientID  =@ClientID AND AUECID=@AUECID)   		

					FETCH c INTO @AUECID, @ClientID
					END	
				CLOSE c
				DEALLOCATE c 								                      

/**
DELETE FROM #tempTableRLClientList
WHERE     (ClientID IN
                          (SELECT     T_RoutingLogicCompanyClient.CompanyClientID_FK AS ClientID
                           FROM         T_RoutingLogicCompanyClient INNER JOIN
                                                 T_RoutingLogic ON T_RoutingLogicCompanyClient.RLID_FK = T_RoutingLogic.RLID
                           WHERE     (T_RoutingLogic.AUECID_FK = @AUECID)) AND AUECID=@AUECID)   
                           **/
                           
SELECT * From #tempTableRLClientList


	/*********
create table #tempTableRLClientList (ClientID int, ClientName varchar(50),  AUECID int, ApplyRL int)

IF( @GroupID <0 )
BEGIN
-----  new grp,  all client needed
insert into #tempTableRLClientList SELECT     T_CompanyClient.CompanyClientID, T_CompanyClient.ClientName, T_CompanyAUEC.AUECID, 0 AS ApplyRL
FROM         T_CompanyClient INNER JOIN
                      T_CompanyClientAUEC ON T_CompanyClient.CompanyClientID = T_CompanyClientAUEC.CompanyClientID INNER JOIN
                      T_CompanyAUEC ON T_CompanyClientAUEC.CompanyAUECID = T_CompanyAUEC.CompanyAUECID
WHERE     (T_CompanyAUEC.CompanyID = @CompanyID)


END
ELSE
BEGIN

----   only acticve grp,  need only grouped client

insert into #tempTableRLClientList SELECT     T_RoutingLogicGroupClient.CompanyClientID_FK, T_CompanyClient.ClientName, T_CompanyAUEC.AUECID, T_RoutingLogicGroupClient.ApplyRL
                       FROM         T_CompanyClientAUEC INNER JOIN
                                             T_CompanyAUEC ON T_CompanyClientAUEC.CompanyAUECID = T_CompanyAUEC.CompanyAUECID INNER JOIN
                                             T_CompanyClient INNER JOIN
                                             T_RoutingLogicGroupClient ON T_CompanyClient.CompanyClientID = T_RoutingLogicGroupClient.CompanyClientID_FK ON 
                                             T_CompanyClientAUEC.CompanyClientID = T_CompanyClient.CompanyClientID
                       WHERE     (T_RoutingLogicGroupClient.ClientGroupID_FK = @GroupID)

SELECT     T_CompanyClient.CompanyClientID AS ClientID, T_CompanyClient.ClientName, T_CompanyAUEC.AUECID, 0 AS ApplyRL
                      FROM         T_CompanyClient INNER JOIN
T_CompanyClientAUEC ON T_CompanyClient.CompanyClientID = T_CompanyClientAUEC.CompanyClientID INNER JOIN
                                            T_CompanyAUEC ON T_CompanyClientAUEC.CompanyAUECID = T_CompanyAUEC.CompanyAUECID INNER JOIN
T_RoutingLogicCompanyClient ON T_CompanyClient.CompanyClientID = T_RoutingLogicCompanyClient.CompanyClientID_FK
                      WHERE     (T_CompanyAUEC.CompanyID = @CompanyID) AND (T_CompanyClient.CompanyClientID NOT IN
                                                (SELECT T_RoutingLogicCompanyClient.CompanyClientID_FK AS CompanyClientID
                                                  FROM          T_RoutingLogicCompanyClient))

END
**********/


