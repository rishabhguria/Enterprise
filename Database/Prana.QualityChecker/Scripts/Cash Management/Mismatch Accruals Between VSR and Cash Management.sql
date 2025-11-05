--Description: VSR Accrual and Cash Mgtmnt Accrual Mismatch
Declare	@FromDate Datetime 
Declare @ToDate Datetime
Declare @FundIds Varchar(Max)
Declare @Symbols Varchar(Max)
--Declare @Smdb

set @FromDate='' 
set @ToDate='' 
set @FundIds='' 
set @Symbols=''
--set @Smdb='' 

Declare  @errormsg varchar(max)
set @errormsg=''

Select
Curr.CurrencySymbol,
Round(SUM((SubBal.CloseDrBal - SubBal.CloseCrBal)),2) as BalanceLocal,
Round(SUM((SubBal.CloseDrBalBase - SubBal.CloseCrBalBase)),2) as BalanceBase
Into #CashMgt  
from T_SubAccountBalances SubBal
INNER JOIN T_SubAccounts SubAcc on SubBal.SubAccountID = SubAcc.SubAccountID
INNER join T_TransactionType TransType on SubAcc.TransactionTypeID = TransType.TransactionTypeID 
INNER JOIN T_CompanyFunds Funds on SubBal.FundID = Funds.CompanyFundID
INNER JOIN T_Currency Curr on SubBal.CurrencyID = Curr.CurrencyID
INNER JOIN T_SubCategory SubCat on SubAcc.SubCategoryID = SubCat.SubCategoryID
INNER JOIN T_MasterCategory MastCat on SubCat.MasterCategoryID = MastCat.MasterCategoryID        
WHERE           
DateDiff(Day,SubBal.TransactionDate,@ToDate)=0  
and TransType.TransactionType = 'Accrued Balance'
Group By Curr.CurrencySymbol
Order By Curr.CurrencySymbol


Select 
Symbol,
Sum(EndingMarketValueLocal) as BalanceLocal,
Sum(EndingMarketValueBase) as  BalanceBase
Into #VSRAccrual
From T_MW_genericPNL 
where Open_CloseTag='Accruals'
AND Rundate=@ToDate
Group By Symbol
Order By Symbol

--Select * from #CashMgt
--Select * from #VSRAccrual

Select
CM.CurrencySymbol as Symbol,
CM.BalanceLocal as [CM Local Ammount],
VSR.BalanceLocal as [VSR Local Ammount],
Round((CM.BalanceLocal-VSR.BalanceLocal),2) as [Local Mismatch],
CM.BalanceBase as [CM Base Amount],
VSR.BalanceBase as [VSR Base Amount], 
round((CM.BalanceBase-VSR.BalanceBase),2) as [Base Mismatch]
into #Mismatch
from #CashMgt CM
Left Outer Join #VSRAccrual VSR ON CM.CurrencySymbol = VSR.Symbol
WHERE (CM.BalanceLocal<>VSR.BalanceLocal OR CM.BalanceBase<>VSR.BalanceBase)
OR VSR.BalanceLocal IS NULL
OR VSR.BalanceBase IS NULL

IF Exists(Select * from #Mismatch where [Base Mismatch] <>0 )
BEGIN
Set @errormsg = @errormsg+'There is a Mismatch. Try running middleware once and recheck'
Select * from #Mismatch
END

Select @errormsg as ErrorMsg

Drop Table #CashMgt,#VSRAccrual,#Mismatch