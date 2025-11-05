
---Account Update/Insert
--update Dim_Accounts 
--Set [Name] = B.Name, Acronym = B.Acronym 
--from Dim_Accounts A 
--Inner join NirvanaClient.dbo.T_Accounts B on A.AccountID = B.AccountID
delete from Dim_Accounts

insert into Dim_Accounts
Select * from NirvanaClient.dbo.T_Accounts where NirvanaClient.dbo.T_Accounts.AccountID 
--not in (select Dim_Accounts.Accountid from Dim_Accounts)
------------------------------------

---SubAccount Update/Insert
--update Dim_SubAccounts 
--Set [Name] = B.Name, Acronym = B.Acronym, Type = C.Type, AccountID = B.AccountID
--from Dim_SubAccounts A 
--Inner join NirvanaClient.dbo.T_SubAccounts B on A.SubAccountID = B.SubAccountID
--Inner join NirvanaClient.dbo.T_AccountType C on B.TypeID = C.AccountTypeID

delete from Dim_SubAccounts
--select * from NirvanaClient.dbo.T_AccountType
--select * from  NirvanaClient.dbo.T_SubAccounts
insert into Dim_SubAccounts
Select * from NirvanaClient.dbo.T_SubAccounts 
--where SubAccountID 
--not in (select SubAccountID from Dim_SubAccounts)
-------------------------------------