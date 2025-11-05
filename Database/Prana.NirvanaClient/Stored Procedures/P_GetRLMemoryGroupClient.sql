

CREATE PROCEDURE dbo.P_GetRLMemoryGroupClient
	(
		@ClientID int,
		@GroupID int
	)
AS
	
create table #tempTable (ID int, Name varchar(50),  ApplyRL int, RLID0 int, RLID1 int, RLID2 int )
DECLARE @ID int, @Name varchar(50),  @ApplyRL int, @RLID0 int, @RLID1 int, @RLID2 int


IF( @ClientID <0 )
BEGIN
-----  new grp,  all client needed

SET @ID = @GroupID

SELECT     @Name = Name
FROM         T_RoutingLogicClientGroup
WHERE     (ClientGroupID = @GroupID)

SELECT     @ApplyRL = ApplyRL
FROM         T_RoutingLogicClientGroup
WHERE     (ClientGroupID = @GroupID)

SELECT     @RLID0 = RLID_FK
FROM         T_RoutingLogicClientGroupRL
WHERE     (Rank = 1) AND (ClientGroupID_FK = @GroupID)
                                                                                                                    
SELECT     @RLID1 = RLID_FK
FROM         T_RoutingLogicClientGroupRL
WHERE     (Rank = 2) AND (ClientGroupID_FK = @GroupID)
                                                                                                                    
SELECT     @RLID2 = RLID_FK
FROM         T_RoutingLogicClientGroupRL
WHERE     (Rank = 3) AND (ClientGroupID_FK = @GroupID)


/************

insert into #tempTable values(@GroupID, SELECT     Name
                                        FROM         T_RoutingLogicClientGroup
                                        WHERE     (ClientGroupID = @GroupID) , SELECT     ApplyRL
                                                                               FROM         T_RoutingLogicClientGroup
                                                                               WHERE     (ClientGroupID = @GroupID),SELECT     RLID_FK
                                                                                                                    FROM         T_RoutingLogicClientGroupRL
                                                                                                                    WHERE     (Rank = 1) AND (ClientGroupID_FK = @GroupID),
                                                                                                                    SELECT     RLID_FK
                                                                                                                    FROM         T_RoutingLogicClientGroupRL
                                                                                                                    WHERE     (Rank = 2) AND (ClientGroupID_FK = @GroupID),
                                                                                                                    SELECT     RLID_FK
                                                                                                                    FROM         T_RoutingLogicClientGroupRL
                                                                                                                    WHERE     (Rank = 3) AND (ClientGroupID_FK = @GroupID))
********/

END
ELSE
BEGIN

----   only acticve grp,  need only grouped client

SET @ID = @ClientID

SELECT     @Name = ClientName
FROM         T_CompanyClient
WHERE     (CompanyClientID = @ClientID)

SELECT     @ApplyRL = ApplyRL
FROM         T_RoutingLogicCompanyClient
WHERE     (CompanyClientID_FK = @ClientID)

SELECT     @RLID0 = RLID_FK
FROM         T_RoutingLogicCompanyClient
WHERE     (Rank = 1) AND (CompanyClientID_FK = @ClientID)
                                                                                                                    
SELECT     @RLID1 = RLID_FK
FROM         T_RoutingLogicCompanyClient
WHERE     (Rank = 2) AND (CompanyClientID_FK = @ClientID)
                                                                                                                    
SELECT     @RLID2 = RLID_FK
FROM         T_RoutingLogicCompanyClient
WHERE     (Rank = 3) AND (CompanyClientID_FK = @ClientID)


/*****
insert into #tempTable values(@ClientID, SELECT     ClientName
                                         FROM         T_CompanyClient
                                         WHERE     (CompanyClientID = @ClientID), 
                                         SELECT     TOP 1 ApplyRL
                                         FROM         T_RoutingLogicCompanyClient
                                         WHERE     (CompanyClientID_FK = @ClientID), SELECT     RLID_FK
                                                                                     FROM         T_RoutingLogicCompanyClient
                                                                                     WHERE     (Rank = 1) AND (CompanyClientID_FK = @ClientID),
                                                                                     SELECT     RLID_FK
                                                                                     FROM         T_RoutingLogicCompanyClient
                                                                                     WHERE     (Rank = 2) AND (CompanyClientID_FK = @ClientID),
                                                                                     SELECT     RLID_FK
                                                                                     FROM         T_RoutingLogicCompanyClient
                                                                                     WHERE     (Rank = 3) AND (CompanyClientID_FK = @ClientID))
      ****/                                                                               
 END

insert into #tempTable SELECT @ID , @Name ,  @ApplyRL , @RLID0 , @RLID1 , @RLID2 

select * from #tempTable


--SELECT * From #tempTable

