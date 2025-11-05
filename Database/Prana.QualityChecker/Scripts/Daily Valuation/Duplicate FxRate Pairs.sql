---Description :  To check duplicate Currency Pairs

Declare @Date datetime
Declare @FromDate datetime
Declare @ToDate datetime
Declare @errormsg varchar(max)

Set @errormsg=''
Set @FromDate=''
Set @ToDate=''



Select 
C.CurrencyName AS [From Currency],
D.CurrencyName AS [To Currency],
A.*  
INTO #duplicateCurrency 
FROM 
T_CurrencyStandardPairs a 
inner join T_CurrencyStandardPairs b on a.fromcurrencyid=b.tocurrencyid and a.tocurrencyid=b.fromcurrencyid
inner join t_currency c on a.fromcurrencyid=c.currencyid
inner join t_currency d on a.tocurrencyid=d.currencyid
order by fromcurrencyid,tocurrencyid

IF  EXISTS( select * from #DuplicateCurrency)
BEGIN

SET @errormsg=' Duplicate FX Rate Pairs. Total : '+(CAST((SELECT COUNT(*) FROM #duplicateCurrency )/2 AS VARCHAR(50)))
SELECT * FROM #duplicateCurrency

END
--select * from T_CurrencyConversionRate where currencypairid_fk=
--select * from T_CurrencyStandardPairs where currencypairid=

SELECT @errormsg AS ErrorMsg

DROP TABLE #duplicateCurrency