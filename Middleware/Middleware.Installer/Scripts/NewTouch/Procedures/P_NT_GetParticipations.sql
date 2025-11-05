GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetParticipations]    Script Date: 05/13/2015 16:36:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
/* Usage:
Exec P_NT_GetParticipations '01/01/2014','09/18/2014','Funds','77,78,79,80,81,82,83,84,85,86'
Exec P_NT_GetParticipations '01/01/2014','09/18/2014','Accounts','87,88,89,90,91,92,93,94,95,96,97,98,99'
Exec P_NT_GetParticipations '01/01/2014','09/18/2014','Investors',1
Exec P_NT_GetParticipations '01/01/2014','09/18/2014','Investors',2 
Exec P_NT_GetParticipations '01/01/2014','09/18/2014','Fund Independent Parties',3
Exec P_NT_GetParticipations '01/01/2014','09/18/2014','Fund Independent Parties',4
Exec P_NT_GetParticipations '01/01/2014','09/18/2014','Account Independent Parties',5
Exec P_NT_GetParticipations '01/01/2014','09/18/2014','Account Independent Parties',6 
--Select ','+Cast(CompanyMasterFundID As varchar(Max)) From T_CompanyMasterFunds Where GroupTypeId = 2 For Xml Path('')
--Select * From T_NT_InvestorIP
*/
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetParticipations] 
-- Add the parameters for the stored procedure here
@StartDate datetime, 
@EndDate datetime,
@SubCategory varchar(Max),
@GroupIds varchar(Max),
@Approved bit = 1
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
--SET NOCOUNT ON;
SET NOCOUNT OFF;
SET FMTONLY OFF;

-- Insert statements for procedure here
Create Table #Dates (Date datetime) 
Declare @CurrentDate datetime 
Select @CurrentDate = @StartDate 
While @CurrentDate <= @EndDate
Begin 
Insert Into #Dates (Date) 
Select @CurrentDate 
Select @CurrentDate = DateAdd(d,1,@CurrentDate)
End 

Create Table #Participations_1 
(Date datetime,UpdateDate datetime,InvestorIPId int,InvestorIPName varchar(Max),FundId int,FundName varchar(Max),AcctId int,AcctName varchar(Max),AcctStartDate datetime,Part_TradingLevel decimal(9,8),Part_InvestedCash decimal(9,8),TheRank int)

Create Table #UpInvestorsPart 
(Approved bit,UpdateDate datetime,InvestorIPId int,InvestorIPName varchar(Max),FundId int,FundName varchar(Max),Part_TradingLevel decimal(9,8),Part_InvestedCash decimal(9,8))

Select B.UpdateDate,B.FundId,C.MasterFundName As FundName,Sum(Part_TradingLevel) As Filled_Part_TradingLevel,Sum(Part_InvestedCash) As Filled_Part_InvestedCash 
Into #UpInvestor From T_NT_InvestorIP A 
Join T_NT_InvestorIPFundPart B On A.Id = B.InvestorIPId And A.GroupTypeId = 4 
Join T_CompanyMasterFunds C On B.FundId = C.CompanyMasterFundID And C.GroupTypeId = 1 
Group By B.UpdateDate,B.FundId,C.MasterFundName 

Select A.UpdateDate,A.FundId,A.FundName,B.Id As InvestorIPId,B.Name As InvestorIPName
Into #UpInvestors From #UpInvestor A 
Join T_NT_InvestorIP B On B.GroupTypeId = 4 

Insert Into #UpInvestorsPart 
(Approved,UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,Part_TradingLevel,Part_InvestedCash)
Select B.Approved,A.UpdateDate,A.InvestorIPId,A.InvestorIPName,A.FundId,A.FundName,IsNull(B.Part_TradingLevel,0) As Part_TradingLevel,IsNull(B.Part_InvestedCash,0) As Part_InvestedCash 
From #UpInvestors A 
Left Outer Join T_NT_InvestorIPFundPart B On A.UpdateDate = B.UpdateDate And A.FundId = B.FundId And A.InvestorIPId = B.InvestorIPId 
Insert Into #UpInvestorsPart 
(Approved,UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,Part_TradingLevel,Part_InvestedCash)
Select 0,UpdateDate,0,'Undefined',FundId,FundName,1 - Filled_Part_TradingLevel As Part_TradingLevel,1 - Filled_Part_InvestedCash As Part_InvestedCash 
From #UpInvestor 
Insert Into #UpInvestorsPart 
(Approved,UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,Part_TradingLevel,Part_InvestedCash)
Select 0,'01/01/1753',0,'Undefined',FundId,FundName,1 As Part_TradingLevel,1 As Part_InvestedCash From (Select Distinct FundId,FundName From #UpInvestor) A

If @SubCategory In ('Investors','Fund Independent Parties','Account Independent Parties')
Begin 
	Create Table #UpdateDatesEntities 
	(UpdateDate datetime,InvestorIPId int,InvestorIPName varchar(Max),FundId int,FundName varchar(Max),Part_TradingLevel decimal(9,8),Part_InvestedCash decimal(9,8))
	If @SubCategory = 'Investors' 
	Begin 
	Insert Into #UpdateDatesEntities 
	(UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,Part_TradingLevel,Part_InvestedCash)
	Select UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,Part_TradingLevel,Part_InvestedCash 
	From #UpInvestorsPart  
	Where InvestorIPId In (Select Items From dbo.Split(@GroupIds,',')) And IsNull(Approved,0) = (Case @Approved When 1 Then 1 Else IsNull(Approved,0) End) 
	End 
	Else If @SubCategory = 'Fund Independent Parties' 
	Begin 
	-- FundIP
	Create Table #UpFundIPsPart 
	(UpdateDate datetime,InvestorIPId int,InvestorIPName varchar(Max),FundId int,FundName varchar(Max),Part_TradingLevel decimal(9,8),Part_InvestedCash decimal(9,8))

	Select Distinct UpdateDate,InvestorIPId
	Into #UpFundIP From T_CompanyMasterFunds A 
	Join T_NT_InvestorIPFundPart B On A.CompanyMasterFundID = B.FundId 
	Where A.GroupTypeId = 1 

	Select A.UpdateDate,B.CompanyMasterFundID As FundId,B.MasterFundName As FundName,A.InvestorIPId,C.Name As InvestorIPName
	Into #UpFundIPs From #UpFundIP A 
	Join T_CompanyMasterFunds B On B.GroupTypeId = 1 
	Join T_NT_InvestorIP C On A.InvestorIPId = C.Id And C.GroupTypeId = 5

	Insert Into #UpFundIPsPart 
	(UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,Part_TradingLevel,Part_InvestedCash)
	Select A.UpdateDate,A.InvestorIPId,A.InvestorIPName,A.FundId,A.FundName,IsNull(B.Part_TradingLevel,0) As Part_TradingLevel,IsNull(B.Part_InvestedCash,0) As Part_InvestedCash 
	From #UpFundIPs A 
	Left Outer Join T_NT_InvestorIPFundPart B On A.UpdateDate = B.UpdateDate And A.FundId = B.FundId And A.InvestorIPId = B.InvestorIPId And IsNull(B.Approved,0) = (Case @Approved When 1 Then 1 Else IsNull(B.Approved,0) End) 

	Insert Into #UpdateDatesEntities 
	(UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,Part_TradingLevel,Part_InvestedCash)
	Select UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,Part_TradingLevel,Part_InvestedCash 
	From #UpFundIPsPart 
	Where InvestorIPId In (Select Items From dbo.Split(@GroupIds,','))  

	Drop Table #UpFundIP,#UpFundIPs,#UpFundIPsPart
	End 
	Else If @SubCategory = 'Account Independent Parties' 
	Begin 
	-- AcctProxyIP
	Create Table #UpAcctProxyIPsPart 
	(UpdateDate datetime,InvestorIPId int,InvestorIPName varchar(Max),FundId int,FundName varchar(Max),Part_TradingLevel decimal(9,8),Part_InvestedCash decimal(9,8))

	Select Distinct UpdateDate,InvestorIPId
	Into #UpAcctProxyIP From T_CompanyMasterFunds A 
	Join T_NT_InvestorIPFundPart B On A.CompanyMasterFundID = B.FundId 
	Where A.GroupTypeId = 2 

	Select A.UpdateDate,B.CompanyMasterFundID As FundId,B.MasterFundName As FundName,A.InvestorIPId,C.Name As InvestorIPName
	Into #UpAcctProxyIPs From #UpAcctProxyIP A 
	Join T_CompanyMasterFunds B On B.GroupTypeId = 2 
	Join T_NT_InvestorIP C On A.InvestorIPId = C.Id And C.GroupTypeId = 6

	Insert Into #UpAcctProxyIPsPart 
	(UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,Part_TradingLevel,Part_InvestedCash)
	Select A.UpdateDate,A.InvestorIPId,A.InvestorIPName,A.FundId,A.FundName,IsNull(B.Part_TradingLevel,0) As Part_TradingLevel,IsNull(B.Part_InvestedCash,0) As Part_InvestedCash 
	From #UpAcctProxyIPs A 
	Left Outer Join T_NT_InvestorIPFundPart B On A.UpdateDate = B.UpdateDate And A.FundId = B.FundId And A.InvestorIPId = B.InvestorIPId And IsNull(B.Approved,0) = (Case @Approved When 1 Then 1 Else IsNull(B.Approved,0) End) 

	Insert Into #UpdateDatesEntities 
	(UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,Part_TradingLevel,Part_InvestedCash)
	Select UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,Part_TradingLevel,Part_InvestedCash 
	From #UpAcctProxyIPsPart 
	Where InvestorIPId In (Select Items From dbo.Split(@GroupIds,','))  

	Drop Table #UpAcctProxyIP,#UpAcctProxyIPs,#UpAcctProxyIPsPart 
	End 
	Insert Into #Participations_1
	(Date,UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,AcctId,AcctName,AcctStartDate,Part_TradingLevel,Part_InvestedCash,TheRank)
	Select A.Date,B.UpdateDate,B.InvestorIPId,B.InvestorIPName,B.FundId,B.FundName,IsNull(E.CompanyFundID,0) As AcctId,IsNull(E.FundName,'Undefined') As AcctName,IsNull(E.StartDate,'01/01/1753') As AcctStartDate,B.Part_TradingLevel,B.Part_InvestedCash,Rank() Over(Partition By A.Date,B.InvestorIPId,B.FundId,IsNull(E.CompanyFundID,0) Order By B.UpdateDate Desc) As TheRank 
	From #Dates A 
	Left Outer Join #UpdateDatesEntities B On A.Date > = B.UpdateDate 
	Join T_CompanyMasterFunds C On B.FundId = C.CompanyMasterFundID 
	Left Outer Join T_CompanyMasterFundSubAccountAssociation D On C.CompanyMasterFundID = D.CompanyMasterFundID 
	Left Outer Join T_CompanyFunds E On D.CompanyFundID = E.CompanyFundID And DateDiff(d,IsNull(E.StartDate,'01/01/1753'),A.Date) >= 0
End 
Else If @SubCategory In ('Funds','Accounts') 
Begin 
	Create Table #Funds 
	(FundId int,FundName varchar(Max))
	If @SubCategory = 'Funds' 
	Begin
	Insert Into #Funds
	(FundId,FundName)
	Select A.CompanyMasterFundID As FundId,A.MasterFundName As FundName 
	From T_CompanyMasterFunds A 
	Where A.GroupTypeId = 1 And A.CompanyMasterFundID In (Select Items From dbo.Split(@GroupIds,',')) 
	End 
	Else If @SubCategory = 'Accounts' 
	Begin 
	Insert Into #Funds
	(FundId,FundName)
	Select A.CompanyMasterFundID As FundId,A.MasterFundName As FundName 
	From T_CompanyMasterFunds A 
	Where A.GroupTypeId = 2 And A.CompanyMasterFundID In (Select Items From dbo.Split(@GroupIds,',')) 
	End 
	Create Table #FundsAccts 
	(FundId int,FundName varchar(Max),AcctId int,AcctName varchar(Max),StartDate datetime)
	Insert Into #FundsAccts
	(FundId,FundName,AcctId,AcctName,StartDate)
	Select A.FundId,A.FundName,IsNull(C.CompanyFundID,0) As AcctId,IsNull(C.FundName,'Undefined') As AcctName,IsNull(C.StartDate,'01/01/1753') As StartDate
	From #Funds A 
	Left Outer Join T_CompanyMasterFundSubAccountAssociation B On A.FundId = B.CompanyMasterFundID 
	Left Outer Join T_CompanyFunds C On B.CompanyFundID = C.CompanyFundID
	Create Table #DatesFundsAccts 
	(Date datetime,FundId int,FundName varchar(Max),AcctId int,AcctName varchar(Max),AcctStartDate datetime)
	Insert Into #DatesFundsAccts
	(Date,FundId,FundName,AcctId,AcctName,AcctStartDate)
	Select Date,FundId,FundName,AcctId,AcctName,B.StartDate As AcctStartDate 
	From #Dates A Join #FundsAccts B On DateDiff(d,B.StartDate,A.Date) >= 0 
	
	If @SubCategory = 'Funds' 
	Begin 
	Insert Into #Participations_1
	(Date,UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,AcctId,AcctName,AcctStartDate,Part_TradingLevel,Part_InvestedCash,TheRank)
	Select A.Date,IsNull(B.UpdateDate,'01/01/1753'),IsNull(B.InvestorIPId,0),IsNull(B.InvestorIPName,'Undefined') As InvestorIPName,A.FundId,A.FundName,A.AcctId,A.AcctName,AcctStartDate,IsNull(B.Part_TradingLevel,1) As Part_TradingLevel,IsNull(B.Part_InvestedCash,1) As Part_InvestedCash,Rank() Over(Partition By A.Date,B.InvestorIPId,A.FundId,A.AcctName Order By B.UpdateDate Desc) As TheRank From #DatesFundsAccts A
	Left Outer Join #UpInvestorsPart B On A.Date > = B.UpdateDate And A.FundId = B.FundId 
	End 
	Else If @SubCategory = 'Accounts' 
	Begin 
	Insert Into #Participations_1
	(Date,UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,AcctId,AcctName,AcctStartDate,Part_TradingLevel,Part_InvestedCash,TheRank)
	Select Date,'01/01/1753' As UpdateDate,0 As InvestorIPId,'Undefined' As InvestorIPName,FundId,FundName,AcctId,AcctName,AcctStartDate,1 As Part_TradingLevel,1 As Part_InvestedCash,1 As TheRank From #DatesFundsAccts 
	End 
	Drop Table #Funds,#FundsAccts,#DatesFundsAccts
End

Create Table #Participations 
(Date datetime Not Null,UpdateDate datetime Not Null,InvestorIPId int Not Null,InvestorIPName varchar(Max) Not Null,FundId int Not Null,FundName varchar(Max) Not Null,AcctId int Not Null,AcctName varchar(Max) Not Null,AcctStartDate datetime Not Null,Part_TradingLevel decimal(9,8) Not Null,Part_InvestedCash decimal(9,8) Not Null)
Insert Into #Participations
(Date,UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,AcctId,AcctName,AcctStartDate,Part_TradingLevel,Part_InvestedCash)
Select Date,UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,AcctId,AcctName,AcctStartDate,Part_TradingLevel,Part_InvestedCash 
From #Participations_1 
Where TheRank = 1 And (Part_TradingLevel <> 0 Or Part_InvestedCash <> 0)

Select Date,UpdateDate,InvestorIPId,InvestorIPName,FundId,FundName,AcctId,AcctName,AcctStartDate,Part_TradingLevel,Part_InvestedCash 
From #Participations Order By Date,InvestorIPId,FundId,AcctId

Drop Table #UpInvestor,#UpInvestors,#UpInvestorsPart
Drop Table #Participations_1,#Participations 
Drop Table #Dates
END



GO