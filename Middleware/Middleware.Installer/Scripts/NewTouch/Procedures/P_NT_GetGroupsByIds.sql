GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetGroupsByIds]    Script Date: 05/13/2015 16:36:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_GetGroupsByIds ''

Exec P_NT_GetGroupsByIds
'<Id>
  <GroupTypeId>5</GroupTypeId>
  <GroupTypeName />
  <GroupId>8</GroupId>
  <GroupName />
</Id>'
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetGroupsByIds] 
	-- Add the parameters for the stored procedure here
    @Ids xml
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here 
	Create Table #Ids 
	(GroupTypeId int,GroupId int)
	Insert Into #Ids 
	(GroupTypeId,GroupId)
	Select 
	ref.value('GroupTypeId[1]','int') As GroupTypeId,
	ref.value('GroupId[1]','int') As GroupId
	From @Ids.nodes('/Id') xmlData(ref)
		
	Create Table #Groups (GroupTypeId int,GroupTypeName varchar(Max),GroupId int,GroupName varchar(Max))
	Insert Into #Groups (GroupTypeId,GroupTypeName,GroupId,GroupName)
	Select B.Id As GroupTypeId,B.[Name] As GroupTypeName,A.Id As GroupId,A.[Name] As GroupName 
	From T_NT_InvestorIP A Join T_NT_GroupType B On A.GroupTypeId = B.Id
	Insert Into #Groups (GroupTypeId,GroupTypeName,GroupId,GroupName)
	Select B.Id As GroupTypeId,B.[Name] As GroupTypeName,A.CompanyMasterFundID As GroupId,A.MasterFundName As GroupName 
	From T_CompanyMasterFunds A Join T_NT_GroupType B On A.GroupTypeId = B.Id

	Select B.GroupTypeId,B.GroupTypeName,B.GroupId,B.GroupName 
	From #Ids A Join #Groups B On A.GroupTypeId = B.GroupTypeId And A.GroupId = B.GroupId

	Drop Table #Ids,#Groups

END

GO