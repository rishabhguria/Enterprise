GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetComponentVaRs]    Script Date: 05/13/2015 16:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
--Select * From T_MW_RiskBySymbol Where Fund = 'Funds/77,78,79,80,81,82,83,84,85,86' And FundType = 2
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Funds','77,78,79,80,81,82,83,84,85,86'
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Funds',@GroupNames = 'OASIS CC LLC '
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Funds',@GroupNames = 'OASIS PGR LLC'
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Funds',@AcctNames = '599-59901'
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Accounts','87,88,89,90,91,92,93,94,95,96,97,98,99'
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Accounts',@GroupNames = '599-59901'
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Investors',1
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Investors',2 
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Fund Independent Parties',3
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Fund Independent Parties',4
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Account Independent Parties',5
Exec P_NT_GetComponentVaRs '09/18/2014',13,0.95,0.94,400,'Account Independent Parties',6 
--Select ','+Cast(CompanyMasterFundID As varchar(Max)) From T_CompanyMasterFunds Where GroupTypeId = 2 For Xml Path('')
--Select * From T_NT_InvestorIP
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetComponentVaRs] 
-- Add the parameters for the stored procedure here
@ToDate datetime,
@Method bigint,
@Confidence float,
@Decay float,
@DaysBack int,
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
Create Table #SymbolwiseRisk (Symbol varchar(Max) Not Null,Risk float Not Null) 
If @GroupIds <> '' 
Begin 
Insert Into #SymbolwiseRisk
Select Symbol,Risk From T_MW_RiskBySymbol 
Where SubString(Fund,CharIndex('/',Fund)+1,Len(Fund) - CharIndex('/',Fund)) = @GroupIds And SubString(Fund,0,CharIndex('/',Fund)) = @SubCategory And FundType = 2 And Method = @Method And Confidence = @Confidence And Decay = @Decay 
End 
Else If @GroupNames <> '' 
Begin  
Declare @GroupIdConcat varchar(Max) 
Select @GroupIdConcat = (Select ',' + Cast(GroupId As varchar(Max)) From 
(Select B.Id As GroupTypeId,B.[Name] As GroupTypeName,A.CompanyMasterFundID As GroupId,A.MasterFundName As GroupName 
From T_CompanyMasterFunds A Join T_NT_GroupType B On A.GroupTypeId = B.Id) Src Where GroupName In (Select Items From dbo.Split(@GroupNames,',')) And GroupTypeId = (Case @SubCategory When 'Funds' Then 1 When 'Accounts' Then 2 End) For Xml Path('')) 
Insert Into #SymbolwiseRisk
Select Symbol,Risk From T_MW_RiskBySymbol 
Where Fund = @SubCategory + '/' + SubString(@GroupIdConcat,2,Len(@GroupIdConcat)-1) And FundType = 2 And Method = @Method And Confidence = @Confidence And Decay = @Decay 
End 
Else If @AcctNames <> '' 
Begin 
Declare @AcctIdConcat varchar(Max) 
Select @AcctIdConcat = (Select ',' + Cast(FundName As varchar(Max)) From T_CompanyFunds Where FundName In (Select Items From dbo.Split(@AcctNames,',')) For Xml Path('')) 
Print Cast(@AcctIdConcat As varchar(Max)) 
Insert Into #SymbolwiseRisk
Select Symbol,Risk From T_MW_RiskBySymbol 
Where Fund = SubString(@AcctIdConcat,2,Len(@AcctIdConcat)-1) And FundType = 0 And Method = @Method And Confidence = @Confidence And Decay = @Decay 
End 

Select Symbol,Risk From #SymbolwiseRisk 
Drop Table #SymbolwiseRisk

END

GO