GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetGroupTypeWiseFundParticipations]    Script Date: 05/13/2015 16:36:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec [P_NT_GetGroupTypeWiseFundParticipations] 4
Exec [P_NT_GetGroupTypeWiseFundParticipations] 5
Exec [P_NT_GetGroupTypeWiseFundParticipations] 6
-- Select * From T_NT_GroupType
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetGroupTypeWiseFundParticipations] 
-- Add the parameters for the stored procedure here
@GroupTypeId int = Null,@GroupTypeName varchar(Max) = Null
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
--SET NOCOUNT OFF;
--SET FMTONLY OFF;
SET NOCOUNT ON;
-- Insert statements for procedure here 

Create Table #UpdateDatesEntities 
(Approved bit Not Null,UpdateDate datetime Not Null,InvestorIPGroupTypeId int Not Null,InvestorIPId int Not Null,InvestorIPName varchar(Max) Not Null,FundGroupTypeId int Not Null,FundId int Not Null,FundName varchar(Max) Not Null,Part_TradingLevel decimal(9,8) Not Null,Part_InvestedCash decimal(9,8) Not Null)

Declare @InvestorIPGroupTypeId int 
If @GroupTypeId Is Not Null 
Begin 
Select @InvestorIPGroupTypeId = Id From T_NT_GroupType Where Id = @GroupTypeId 
End 
Else If @GroupTypeName Is Not Null 
Begin 
Select @InvestorIPGroupTypeId = Id From T_NT_GroupType Where [Name] = @GroupTypeName 
End 

If @InvestorIPGroupTypeId Is Not Null 
Begin 

	If @InvestorIPGroupTypeId = 4 
	Begin 
	Select Distinct Approved,UpdateDate,FundId 
	Into #UpInvestor From T_NT_InvestorIP A 
	Join T_NT_InvestorIPFundPart B On A.Id = B.InvestorIPId 
	Where A.GroupTypeId = 4

	Select A.Approved,A.UpdateDate,C.GroupTypeId As FundGroupTypeId,A.FundId,C.MasterFundName As FundName,B.GroupTypeId As InvestorIPGroupTypeId,B.Id As InvestorIPId,B.Name As InvestorIPName
	Into #UpInvestors From #UpInvestor A 
	Join T_NT_InvestorIP B On B.GroupTypeId = 4 
	Join T_CompanyMasterFunds C On A.FundId = C.CompanyMasterFundID And C.GroupTypeId = 1

	Insert Into #UpdateDatesEntities 
	(Approved,UpdateDate,InvestorIPGroupTypeId,InvestorIPId,InvestorIPName,FundGroupTypeId,FundId,FundName,Part_TradingLevel,Part_InvestedCash)
	Select IsNull(A.Approved,0),A.UpdateDate,A.InvestorIPGroupTypeId,A.InvestorIPId,A.InvestorIPName,A.FundGroupTypeId,A.FundId,A.FundName,IsNull(B.Part_TradingLevel,0) As Part_TradingLevel,IsNull(B.Part_InvestedCash,0) As Part_InvestedCash 
	From #UpInvestors A 
	Left Outer Join T_NT_InvestorIPFundPart B On A.UpdateDate = B.UpdateDate And A.FundId = B.FundId And A.InvestorIPId = B.InvestorIPId

	Drop Table #UpInvestor,#UpInvestors
	End 
	If @InvestorIPGroupTypeId = 5 
	Begin 
	-- FundIP
	Select Distinct Approved,UpdateDate,InvestorIPId
	Into #UpFundIP From T_CompanyMasterFunds A 
	Join T_NT_InvestorIPFundPart B On A.CompanyMasterFundID = B.FundId 
	Where A.GroupTypeId = 1 

	Select A.Approved,A.UpdateDate,B.GroupTypeID As FundGroupTypeId,B.CompanyMasterFundID As FundId,B.MasterFundName As FundName,C.GroupTypeId As InvestorIPGroupTypeId,A.InvestorIPId,C.Name As InvestorIPName
	Into #UpFundIPs From #UpFundIP A 
	Join T_CompanyMasterFunds B On B.GroupTypeId = 1 
	Join T_NT_InvestorIP C On A.InvestorIPId = C.Id And C.GroupTypeId = 5

	Insert Into #UpdateDatesEntities 
	(Approved,UpdateDate,InvestorIPGroupTypeId,InvestorIPId,InvestorIPName,FundGroupTypeId,FundId,FundName,Part_TradingLevel,Part_InvestedCash)
	Select IsNull(A.Approved,0),A.UpdateDate,A.InvestorIPGroupTypeId,A.InvestorIPId,A.InvestorIPName,A.FundGroupTypeId,A.FundId,A.FundName,IsNull(B.Part_TradingLevel,0) As Part_TradingLevel,IsNull(B.Part_InvestedCash,0) As Part_InvestedCash 
	From #UpFundIPs A 
	Left Outer Join T_NT_InvestorIPFundPart B On A.UpdateDate = B.UpdateDate And A.FundId = B.FundId And A.InvestorIPId = B.InvestorIPId

	Drop Table #UpFundIP,#UpFundIPs
	End 
	If @InvestorIPGroupTypeId = 6 
	Begin 
	-- AcctProxyIP
	Select Distinct Approved,UpdateDate,InvestorIPId
	Into #UpAcctProxyIP From T_CompanyMasterFunds A 
	Join T_NT_InvestorIPFundPart B On A.CompanyMasterFundID = B.FundId 
	Where A.GroupTypeId = 2 

	Select A.Approved,A.UpdateDate,B.GroupTypeId As FundGroupTypeId,B.CompanyMasterFundID As FundId,B.MasterFundName As FundName,C.GroupTypeId As InvestorIPGroupTypeId,A.InvestorIPId,C.Name As InvestorIPName
	Into #UpAcctProxyIPs From #UpAcctProxyIP A 
	Join T_CompanyMasterFunds B On B.GroupTypeId = 2 
	Join T_NT_InvestorIP C On A.InvestorIPId = C.Id And C.GroupTypeId = 6

	Insert Into #UpdateDatesEntities 
	(Approved,UpdateDate,InvestorIPGroupTypeId,InvestorIPId,InvestorIPName,FundGroupTypeId,FundId,FundName,Part_TradingLevel,Part_InvestedCash)
	Select IsNull(A.Approved,0),A.UpdateDate,A.InvestorIPGroupTypeId,A.InvestorIPId,A.InvestorIPName,A.FundGroupTypeId,A.FundId,A.FundName,IsNull(B.Part_TradingLevel,0) As Part_TradingLevel,IsNull(B.Part_InvestedCash,0) As Part_InvestedCash 
	From #UpAcctProxyIPs A 
	Left Outer Join T_NT_InvestorIPFundPart B On A.UpdateDate = B.UpdateDate And A.FundId = B.FundId And A.InvestorIPId = B.InvestorIPId

	Drop Table #UpAcctProxyIP,#UpAcctProxyIPs 
	End 

End 

Select Approved,UpdateDate,InvestorIPGroupTypeId,B.Name As InvestorIPGroupTypeName,InvestorIPId,InvestorIPName,FundGroupTypeId,C.Name As FundGroupTypeName,FundId,FundName,Part_TradingLevel,Part_InvestedCash 
From #UpdateDatesEntities A Join T_NT_GroupType B On A.InvestorIPGroupTypeId = B.Id Join T_NT_GroupType C On A.FundGroupTypeId = C.Id 
Order By UpdateDate Asc

Drop Table #UpdateDatesEntities

END

GO