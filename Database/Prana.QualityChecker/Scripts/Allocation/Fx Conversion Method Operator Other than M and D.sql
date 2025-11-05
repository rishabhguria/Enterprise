Declare @ErrorMsg Varchar(Max)
DECLARE @FromDate datetime 
Declare	@ToDate datetime
Declare	@FundIds varchar(max)
set @FromDate='' 
set @ToDate='' 
set @FundIds=''
Set @ErrorMsg=''

Create Table #ErrorTable
(
Symbol Varchar(500),
FXConversionMethodOperator Varchar(20),
[Table Name] Varchar(200)
,Date datetime,
DateType Varchar(200),
Account varchar(100)
)

If Exists(Select FXConversionMethodOperator from T_DeletedTaxLots WHERE FXConversionMethodOperator NOT IN ('M','D')
AND CurrencyID NOT IN (1))
BEGIN
Insert Into #ErrorTable
Select
distinct
Symbol,
FXConversionMethodOperator,
'T_DeletedTaxLots'
,AUECLocalDate,
'AUECLocalDate',
FundID
From T_DeletedTaxLots 
WHERE FXConversionMethodOperator NOT IN ('M','D')
Set @ErrorMsg= @ErrorMsg + '1) T_DeletedTaxLots '
END

If Exists(Select PM.FXConversionMethodOperator from PM_Taxlots PM
INNER JOIN T_Group TG on PM.GroupID=TG.GroupID
WHERE PM.FXConversionMethodOperator NOT IN ('M','D') AND TG.CurrencyID NOT IN (1))
BEGIN
Insert Into #ErrorTable
Select
distinct
Symbol,
FXConversionMethodOperator,
'PM_Taxlots'
,AUECModifiedDate
,'AUECModifiedDate',
FundID
From PM_Taxlots 
WHERE FXConversionMethodOperator NOT IN ('M','D')
Set @ErrorMsg= @ErrorMsg + '2) PM_Taxlots '
END

If Exists(Select FXConversionMethodOperator from T_AllActivity WHERE FXConversionMethodOperator NOT IN ('M','D')
AND CurrencyID NOT IN (1))
BEGIN
Insert Into #ErrorTable
Select
distinct
Symbol,
FXConversionMethodOperator,
'T_AllActivity'
,TradeDate,
'TradeDate',
FundID
From T_AllActivity 
WHERE FXConversionMethodOperator NOT IN ('M','D')
Set @ErrorMsg= @ErrorMsg + '3) T_AllActivity '
END

If Exists(Select FXConversionMethodOperator from T_CashTransactions WHERE FXConversionMethodOperator NOT IN ('M','D')
AND CurrencyID NOT IN (1))
BEGIN
Insert Into #ErrorTable
Select
distinct
Symbol,
FXConversionMethodOperator,
'T_CashTransactions'
,ExDate,
'ExDate',
FundID
From T_CashTransactions 
WHERE FXConversionMethodOperator NOT IN ('M','D')
Set @ErrorMsg= @ErrorMsg + '4) T_CashTransactions '
END

If Exists(Select FXConversionMethodOperator from T_ExternalOrder WHERE FXConversionMethodOperator NOT IN ('M','D')
AND CurrencyID NOT IN (1))
BEGIN
Insert Into #ErrorTable
Select
distinct
Symbol,
FXConversionMethodOperator,
'T_ExternalOrder'
,AUECLocalDate,
'AUECLocalDate',
''
From T_ExternalOrder 
WHERE FXConversionMethodOperator NOT IN ('M','D')
Set @ErrorMsg= @ErrorMsg + '5) T_ExternalOrder '
END

If Exists(Select FXConversionMethodOperator from T_Group WHERE FXConversionMethodOperator NOT IN ('M','D')
AND CurrencyID NOT IN (1))
BEGIN
Insert Into #ErrorTable
Select
distinct
Symbol,
FXConversionMethodOperator,
'T_Group'
,AUECLocalDate,
'AUECLocalDate',
''
From T_Group 
WHERE FXConversionMethodOperator NOT IN ('M','D')
Set @ErrorMsg= @ErrorMsg + '6) T_Group '
END

If Exists(Select FXConversionMethodOperator from T_Journal WHERE FXConversionMethodOperator NOT IN ('M','D')
AND CurrencyID NOT IN (1))
BEGIN
Insert Into #ErrorTable
Select
distinct
Symbol,
FXConversionMethodOperator,
'T_Journal'
,TransactionDate,
'TransactionDate',
FundID
From T_Journal 
WHERE FXConversionMethodOperator NOT IN ('M','D')
Set @ErrorMsg= @ErrorMsg + '7) T_Journal '
END

If Exists(Select FXConversionMethodOperator from T_JournalManualTransaction WHERE FXConversionMethodOperator NOT IN ('M','D')
AND CurrencyID NOT IN (1))
BEGIN
Insert Into #ErrorTable
Select
distinct
Symbol,
FXConversionMethodOperator,
'T_JournalManualTransaction'
,TransactionDate,
'TransactionDate',
FundID
From T_JournalManualTransaction 
WHERE FXConversionMethodOperator NOT IN ('M','D')
Set @ErrorMsg= @ErrorMsg + '8) T_JournalManualTransaction '
END

If Exists(Select TL2.FXConversionMethodOperator from T_Level2Allocation TL2
INNER JOIN T_Group TG on TL2.GroupID=TG.GroupID
WHERE TL2.FXConversionMethodOperator NOT IN ('M','D') AND TG.CurrencyID not in(1))
BEGIN
Insert Into #ErrorTable
Select
distinct
Symbol,
FXConversionMethodOperator,
'T_Level2Allocation'
,TransactionDate,
'TransactionDate',
FundID
From T_JournalManualTransaction 
WHERE FXConversionMethodOperator NOT IN ('M','D')
Set @ErrorMsg= @ErrorMsg + '9) T_Level2Allocation'
END

--If Exists(Select FXConversionMethodOperator from T_TempAllActivity WHERE FXConversionMethodOperator NOT IN ('M','D'))
--BEGIN
--Insert Into #ErrorTable
--Select
--Symbol,
--FXConversionMethodOperator,
--'T_TempAllActivity'
--From T_TempAllActivity 
--WHERE FXConversionMethodOperator NOT IN ('M','D')
--Set @ErrorMsg= @ErrorMsg + 'T_TempAllActivity '
--END

If Exists(Select TTO.FXConversionMethodOperator from T_TradedOrders TTO
INNER JOIN T_Group TG on TTO.GroupID=TG.GroupID
WHERE TTO.FXConversionMethodOperator NOT IN ('M','D') AND TG.CurrencyID NOT IN(1))
BEGIN
Insert Into #ErrorTable
Select
distinct
Symbol,
FXConversionMethodOperator,
'T_TradedOrders'
,AUECLocalDate,
'AUECLocalDate',
FundID
From T_TradedOrders 
WHERE FXConversionMethodOperator NOT IN ('M','D')
Set @ErrorMsg= @ErrorMsg + '10) T_TradedOrders '
END

Select 
Symbol ,
FXConversionMethodOperator,
[Table Name], 
Date ,
DateType,
FundName as Account
 from #ErrorTable INNER JOIN T_CompanyFunds on Account=CompanyFundID

Select @ErrorMsg AS ErrorMsg 
Drop Table #ErrorTable