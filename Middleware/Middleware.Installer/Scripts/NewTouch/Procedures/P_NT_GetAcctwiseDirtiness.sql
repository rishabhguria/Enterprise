GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetAcctwiseDirtiness]    Script Date: 05/13/2015 16:36:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
--Exec P_NT_GetAcctwiseDirtiness '09/19/2014',1
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetAcctwiseDirtiness]
-- Add the parameters for the stored procedure here
@AsOfDate datetime,@SecondaryResultSet bit = 0 
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
--SET NOCOUNT ON;
SET NOCOUNT OFF;
SET FMTONLY OFF;
-- Insert statements for procedure here
Select AcctId,Max(AsOfDate) As LatestDate 
Into #AcctwiseLatest 
From T_NT_AcctwiseStatus 
Where AsOfDate <= @AsOfDate
Group By AcctId 

Create Table #AcctwiseStatus
(AcctId int Not Null,AcctName varchar(Max) Not Null,LatestDate datetime,IsDirty bit Not Null)
Insert Into #AcctwiseStatus
(AcctId,AcctName,LatestDate,IsDirty)
Select A.CompanyFundID,A.FundName,B.LatestDate,Case When LatestDate Is Not Null Then 0 Else 1 End 
From T_CompanyFunds A Left Outer Join #AcctwiseLatest B On A.CompanyFundId = B.AcctId 

Create Table #Acctwise 
(AcctId int Not Null, AcctName varchar(Max) Not Null,LatestDate datetime,AsOfDate datetime) 
Insert Into #Acctwise 
(AcctId,AcctName,LatestDate,AsOfDate)
Select AcctId,AcctName,LatestDate,@AsOfDate 
From #AcctwiseStatus 
Where IsDirty = 0

Select B.CheckSumId,B.AcctId,B.RunDate Into #FreshGenericPNL From #Acctwise A Join T_NT_GenericPNL B On A.AcctId = B.AcctId And DateDiff(d,RunDate,AsOfDate) >= 0
Select B.CheckSumId,B.AcctId,B.RunDate Into #ApprovedGenericPNL From #Acctwise A Join T_NT_ApprovedGenericPNL B On A.LatestDate = B.AsOfDate And A.AcctId = B.AcctId 
Select B.CheckSumId,B.AcctId,B.RunDate Into #FreshTransactions From #Acctwise A Join T_NT_Transactions B On A.AcctId = B.AcctId And DateDiff(d,RunDate,AsOfDate) >= 0
Select B.CheckSumId,B.AcctId,B.RunDate Into #ApprovedTransactions From #Acctwise A Join T_NT_ApprovedTransactions B On A.LatestDate = B.AsOfDate And A.AcctId = B.AcctId 
Select B.CheckSumId,B.AcctId,B.RunDate Into #FreshCashAccruals From #Acctwise A Join T_NT_CashAccruals B On A.AcctId = B.AcctId And DateDiff(d,RunDate,AsOfDate) >= 0
Select B.CheckSumId,B.AcctId,B.RunDate Into #ApprovedCashAccruals From #Acctwise A Join T_NT_ApprovedCashAccruals B On A.LatestDate = B.AsOfDate And A.AcctId = B.AcctId 

Create Table #GenericPNLDiscrepancies 
(CheckSumId varbinary(16),AcctId int,RunDate datetime,FromTo bit) 
Insert Into #GenericPNLDiscrepancies
(CheckSumId,AcctId,RunDate,FromTo) 
Select CheckSumId,AcctId,RunDate,1 From 
(Select CheckSumId,AcctId,RunDate From #FreshGenericPNL 
Except 
Select CheckSumId,AcctId,RunDate From #ApprovedGenericPNL) FVSA
Insert Into #GenericPNLDiscrepancies
(CheckSumId,AcctId,RunDate,FromTo) 
Select CheckSumId,AcctId,RunDate,0 From 
(Select CheckSumId,AcctId,RunDate From #ApprovedGenericPNL 
Except 
Select CheckSumId,AcctId,RunDate From #FreshGenericPNL) AVSF 
Create Table #TransactionsDiscrepancies 
(CheckSumId varbinary(16),AcctId int,RunDate datetime,FromTo bit) 
Insert Into #TransactionsDiscrepancies
(CheckSumId,AcctId,RunDate,FromTo) 
Select CheckSumId,AcctId,RunDate,1 From 
(Select CheckSumId,AcctId,RunDate From #FreshTransactions 
Except 
Select CheckSumId,AcctId,RunDate From #ApprovedTransactions) FVSA
Insert Into #TransactionsDiscrepancies
(CheckSumId,AcctId,RunDate,FromTo)
Select CheckSumId,AcctId,RunDate,0 From 
(Select CheckSumId,AcctId,RunDate From #ApprovedTransactions 
Except 
Select CheckSumId,AcctId,RunDate From #FreshTransactions) AVSF
Create Table #CashAccrualsDiscrepancies 
(CheckSumId varbinary(16),AcctId int,RunDate datetime,FromTo bit) 
Insert Into #CashAccrualsDiscrepancies
(CheckSumId,AcctId,RunDate,FromTo) 
Select CheckSumId,AcctId,RunDate,1 From 
(Select CheckSumId,AcctId,RunDate From #FreshCashAccruals 
Except 
Select CheckSumId,AcctId,RunDate From #ApprovedCashAccruals) FVSA
Insert Into #CashAccrualsDiscrepancies
(CheckSumId,AcctId,RunDate,FromTo)
Select CheckSumId,AcctId,RunDate,0 From 
(Select CheckSumId,AcctId,RunDate From #ApprovedCashAccruals 
Except 
Select CheckSumId,AcctId,RunDate From #FreshCashAccruals) AVSF

Create Table #DirtyAccts 
(AcctId int) 
Insert Into #DirtyAccts 
(AcctId)
Select Distinct AcctId From #GenericPNLDiscrepancies 
Insert Into #DirtyAccts 
(AcctId) 
Select Distinct AcctId From #TransactionsDiscrepancies
Insert Into #DirtyAccts 
(AcctId) 
Select Distinct AcctId From #CashAccrualsDiscrepancies

Update A Set IsDirty = 1 
From #AcctwiseStatus A Join #DirtyAccts B On A.AcctId = B.AcctId

Select AcctId,AcctName,LatestDate,IsDirty From #AcctwiseStatus 

If @SecondaryResultSet = 1  
Begin 
	Create Table #AcctwiseDirtinessSecondary(AcctId int Not Null,RunDate datetime Not Null)
	Insert Into #AcctwiseDirtinessSecondary(AcctId,RunDate)
	Select AcctId,RunDate From #GenericPNLDiscrepancies 
	Union 
	Select AcctId,RunDate From #TransactionsDiscrepancies 
	Union 
	Select AcctId,RunDate From #CashAccrualsDiscrepancies 
	Select AcctId,RunDate From #AcctwiseDirtinessSecondary
	Drop Table #AcctwiseDirtinessSecondary
End 

Drop Table #AcctwiseLatest,#Acctwise,#AcctwiseStatus
Drop Table #FreshGenericPNL,#ApprovedGenericPNL,#GenericPNLDiscrepancies 
Drop Table #FreshTransactions,#ApprovedTransactions,#TransactionsDiscrepancies
Drop Table #FreshCashAccruals,#ApprovedCashAccruals,#CashAccrualsDiscrepancies
Drop Table #DirtyAccts 

END


GO