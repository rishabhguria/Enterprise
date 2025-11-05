
CREATE PROCEDURE [dbo].[P_SaveFundGroupMapping]          
(      
  @XMLDoc nText
 , @groupID int
 , @groupName varchar(50) 
 , @SavedFundId int output                      
 )               
AS          
      
SET @SavedFundId = 0      
                                                                      
DECLARE @handle int                          
exec sp_xml_preparedocument @handle OUTPUT,@XMLDoc            
      
 CREATE TABLE #TempTableNames                                                                               
  (                                                                               
    GroupId int,      
	FundId int                   
   )        
      
INSERT INTO #TempTableNames                     
 (                                                                              
    GroupId                        
   ,FundId                                                       
 )                                                                              
SELECT                                                                               
	GroupId                        
   ,FundId                                      
    FROM OPENXML(@handle, '/DSFundGroupMapping/TABFundGroupMapping', 2)                                                                                 
 WITH                                                                               
 (                                                         
   GroupId int,      
   FundId int              
 )                                

IF @groupID >= 0
BEGIN
UPDATE T_FundGroups SET GroupName=@groupName where FundGroupID=@groupID

delete from T_GroupFundMapping 
where FundGroupID in (SELECT GroupId from #TempTableNames)

Insert into T_GroupFundMapping (FundGroupID,FundId) select GroupId,FundId from #TempTableNames       
END

ELSE
BEGIN
INSERT INTO T_FundGroups(GroupName) VALUES(@groupName)
Insert into T_GroupFundMapping (FundGroupID,FundId) select @@identity,FundId from #TempTableNames
Select @SavedFundId = max(FundGroupID) from T_FundGroups
END

EXEC sp_xml_removedocument @handle                           
                                             
