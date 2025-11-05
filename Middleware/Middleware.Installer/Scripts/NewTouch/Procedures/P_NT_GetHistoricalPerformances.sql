GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetHistoricalPerformances]    Script Date: 05/13/2015 16:36:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_GetHistoricalPerformances 'Funds','77,78,79,80,81,82,83,84,85,86'
Exec P_NT_GetHistoricalPerformances 'Funds',@GroupNames = 'OASIS CC LLC '
Exec P_NT_GetHistoricalPerformances 'Funds',@GroupNames = 'OASIS PGR LLC'
Exec P_NT_GetHistoricalPerformances 'Funds',@AcctNames = '599-59901'
Exec P_NT_GetHistoricalPerformances 'Accounts','87,88,89,90,91,92,93,94,95,96,97,98,99'
Exec P_NT_GetHistoricalPerformances 'Accounts',@GroupNames = '599-59901'
Exec P_NT_GetHistoricalPerformances 'Investors',1
Exec P_NT_GetHistoricalPerformances 'Investors',2 
Exec P_NT_GetHistoricalPerformances 'Fund Independent Parties',3
Exec P_NT_GetHistoricalPerformances 'Fund Independent Parties',4
Exec P_NT_GetHistoricalPerformances 'Account Independent Parties',5
Exec P_NT_GetHistoricalPerformances 'Account Independent Parties',6 
--Select ','+Cast(CompanyMasterFundID As varchar(Max)) From T_CompanyMasterFunds Where GroupTypeId = 2 For Xml Path('')
--Select * From T_NT_InvestorIP
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetHistoricalPerformances] 
-- Add the parameters for the stored procedure here
@SubCategory varchar(Max),
@GroupIds varchar(Max) = Null,
@GroupNames varchar(Max) = Null,
@AcctNames varchar(Max) = Null
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
--SET NOCOUNT ON;
SET NOCOUNT OFF;
SET FMTONLY OFF;

-- Insert statements for procedure here 
If @GroupIds <> '' 
Begin 
Select [Year],[Month],NetRet,GrossRet From T_NT_HistoricalPerformances 
Where SubString(Entity,CharIndex('/',Entity)+1,Len(Entity) - CharIndex('/',Entity)) = @GroupIds And SubString(Entity,0,CharIndex('/',Entity)) = @SubCategory And FundType = 2 
End 
Else If @GroupNames <> '' 
Begin  
Declare @GroupIdConcat varchar(Max) 
Select @GroupIdConcat = (Select ',' + Cast(GroupId As varchar(Max)) From 
(Select B.Id As GroupTypeId,B.[Name] As GroupTypeName,A.CompanyMasterFundID As GroupId,A.MasterFundName As GroupName 
From T_CompanyMasterFunds A Join T_NT_GroupType B On A.GroupTypeId = B.Id) Src Where GroupName In (Select Items From dbo.Split(@GroupNames,',')) And GroupTypeId = (Case @SubCategory When 'Funds' Then 1 When 'Accounts' Then 2 End) For Xml Path('')) 
Select [Year],[Month],NetRet,GrossRet From T_NT_HistoricalPerformances 
Where Entity = @SubCategory + '/' + SubString(@GroupIdConcat,2,Len(@GroupIdConcat)-1) And FundType = 2
End 
Else If @AcctNames <> '' 
Begin 
Declare @AcctIdConcat varchar(Max) 
Select @AcctIdConcat = (Select ',' + Cast(FundName As varchar(Max)) From T_CompanyFunds Where FundName In (Select Items From dbo.Split(@AcctNames,',')) For Xml Path('')) 
Print Cast(@AcctIdConcat As varchar(Max))
Select [Year],[Month],NetRet,GrossRet From T_NT_HistoricalPerformances 
Where Entity = SubString(@AcctIdConcat,2,Len(@AcctIdConcat)-1) And FundType = 0 
End 
END

GO