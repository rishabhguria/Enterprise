GO
/****** Object:  StoredProcedure [dbo].[P_NT_DisapproveApproveAcctwise]    Script Date: 05/13/2015 16:36:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
--Declare @AsOfDate datetime
--Select @AsOfDate = '09/19/2014'
--Declare @AcctId xml 
--Select @AcctId = '<Acct><AcctId>1309</AcctId></Acct><Acct><AcctId>1310</AcctId></Acct><Acct><AcctId>1311</AcctId></Acct><Acct><AcctId>1312</AcctId></Acct><Acct><AcctId>1313</AcctId></Acct><Acct><AcctId>1314</AcctId></Acct><Acct><AcctId>1315</AcctId></Acct><Acct><AcctId>1316</AcctId></Acct><Acct><AcctId>1317</AcctId></Acct><Acct><AcctId>1318</AcctId></Acct><Acct><AcctId>1319</AcctId></Acct><Acct><AcctId>1320</AcctId></Acct><Acct><AcctId>1321</AcctId></Acct>'
--Declare @DisapproveApprove bit 
--Select @DisapproveApprove = 1
--Exec P_NT_ApproveAcctwise @AsOfDate,@AcctId,@DisapproveApprove
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_DisapproveApproveAcctwise]
-- Add the parameters for the stored procedure here
@AsOfDate datetime,
@AcctId xml,
@DisapproveApprove bit 
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here 
Create Table #Acct
(AcctId int) 
Insert Into #Acct 
(AcctId) 
Select 
ref.value('AcctId[1]','int') As FundID
From @AcctId.nodes('/Acct') xmlData(ref)

Create Table #Acctwise 
(AcctId int,AcctName varchar(Max),AsOfDate datetime) 
Insert Into #Acctwise 
(AcctId,AcctName,AsOfDate) 
Select 
A.AcctId,
B.FundName,
@AsOfDate  
From #Acct A Join T_CompanyFunds B On A.AcctId = B.CompanyFundID 

Delete A From T_NT_ApprovedGenericPNL A Join #Acctwise B On A.AsOfDate = B.AsOfDate And A.AcctId = B.AcctId 
Delete A From T_NT_ApprovedTransactions A Join #Acctwise B On A.AsOfDate = B.AsOfDate And A.AcctId = B.AcctId 
Delete A From T_NT_ApprovedCashAccruals A Join #Acctwise B On A.AsOfDate = B.AsOfDate And A.AcctId = B.AcctId 
Delete A From T_NT_AcctwiseStatus A Join #Acctwise B On A.AsOfDate = B.AsOfDate And A.AcctId = B.AcctId 

If @DisapproveApprove = 1 
Begin 

Insert Into T_NT_ApprovedGenericPNL 
Select B.AsOfDate,A.* 
From T_NT_GenericPNL A Join #Acctwise B On A.AcctId = B.AcctId And DateDiff(d,Rundate,AsOfDate) >= 0 

Insert Into T_NT_ApprovedTransactions 
Select B.AsOfDate,A.* 
From T_NT_Transactions A Join #Acctwise B On A.AcctId = B.AcctId And DateDiff(d,Rundate,AsOfDate) >= 0 

Insert Into T_NT_ApprovedCashAccruals 
Select B.AsOfDate,A.* 
From T_NT_CashAccruals A Join #Acctwise B On A.AcctId = B.AcctId And DateDiff(d,Rundate,AsOfDate) >= 0 

Insert Into T_NT_AcctwiseStatus 
(AsOfDate,AcctId)
Select AsOfDate,AcctId From #Acctwise

End 

Drop Table #Acct,#Acctwise 

END

GO