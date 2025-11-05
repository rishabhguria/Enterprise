GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetGroups]    Script Date: 05/13/2015 16:36:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_GetGroups
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetGroups] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here 

	Create Table #Groups (GroupTypeId int,GroupTypeName varchar(Max),GroupId int,GroupName varchar(Max))
	Insert Into #Groups (GroupTypeId,GroupTypeName,GroupId,GroupName)
    Select B.Id As GroupTypeId,B.[Name] As GroupTypeName,A.Id As GroupId,A.[Name] As GroupName 
	From T_NT_InvestorIP A Join T_NT_GroupType B On A.GroupTypeId = B.Id
	Insert Into #Groups (GroupTypeId,GroupTypeName,GroupId,GroupName)
    Select B.Id As GroupTypeId,B.[Name] As GroupTypeName,A.CompanyMasterFundID As GroupId,A.MasterFundName As GroupName 
	From T_CompanyMasterFunds A Join T_NT_GroupType B On A.GroupTypeId = B.Id

	Select GroupTypeId,GroupTypeName,GroupId,GroupName From #Groups 
	Drop Table #Groups

END



GO