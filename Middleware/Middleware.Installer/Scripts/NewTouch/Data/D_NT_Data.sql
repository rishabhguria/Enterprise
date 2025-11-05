-- Step 1
delete from T_NT_GroupType
Insert Into T_NT_GroupType 
(Id,[Name]) 
Select 2,'Account Proxy'
Union
Select 6,'Account Proxy Independent Party'
Union
Select 1,'Fund'
Union
Select 5,'Fund Independent Party'
Union 
Select 4,'Investor'
go
-- Step 2
delete from T_NT_AssetwisePreferences
Insert Into T_NT_AssetwisePreferences 
(Asset,CommissionAndFees,FXPNL,PriceMultiplier,DeltaAdjPosMultiplier,ZeroOrEndingMVOrUnrealized,CouponRate,BlackScholesOrBlack76) 
Select 'Default','True','True',1,'True',1,'False','False'
Union 
Select 'Equity','True','True',1,'True',1,'False','False'
Union 
Select 'FixedIncome','True','True',1,'True',1,'False','False'
Union 
Select 'Future','True','True',1,'True',2,'False','False'
Union 
Select 'FX','True','True',1,'True',2,'False','False'
Union 
Select 'InternationalFutureOption','True','True',1,'False',2,'False','True'
Union 
Select 'LocalFutureOption','True','True',1,'False',1,'False','True'
Union 
Select 'Swap','True','False',1,'True',2,'False','False'

go
-- Step 3 
Update D Set StartDate = Case When S.CashMgmtStartDate Is Null Then Null Else DateAdd(d,1,S.CashMgmtStartDate) End 
From T_CompanyFunds D Left Outer Join T_CashPreferences S On D.CompanyFundID = S.FundID 

update T_CompanyMasterFunds
set GroupTypeId =1

go
-- Step 4 
delete from T_CompanyMasterFunds where GroupTypeId =2
Insert Into T_CompanyMasterFunds 
(MasterFundName,GroupTypeId)
Select FundName,2 From T_CompanyFunds

Go
-- Step 5 
delete MFSAA from  T_CompanyMasterFundSubAccountAssociation MFSAA
inner join  T_CompanyMasterFunds as MF on MF.CompanyMasterFundID =MFSAA.CompanyMasterFundID where GroupTypeId =2

Insert Into T_CompanyMasterFundSubAccountAssociation 
(CompanyMasterFundID,CompanyFundID) 
Select CompanyMasterFundID,CompanyFundID From T_CompanyMasterFunds A Join T_CompanyFunds B On A.MasterFundName = B.FundName