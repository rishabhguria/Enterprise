--Description: Find duplicated Journal entries 

Declare	@FromDate Datetime 
Declare @ToDate Datetime
--Declare @Smdb

set @FromDate='' 
set @ToDate='2015-05-01 00:00:00.000' 
--set @Smdb='' 


Declare  @errormsg varchar(max)
set @errormsg=''

/*
Date: 5/19/2016 
JIRA: http://jira.nirvanasolutions.com:8080/browse/PRANA-16158
Added two checks
1- Type should be available is this Trading, Non Trading, Revaluation, Dividends, etc 
2- Bond Interest amount is same for everyday. And this tool is catching those Interest Rates.(Not sure about this check)
-Ankit Misra
*/

Select Distinct UnderlyingSymbol,symbol,UnderlyingSymbolPrice,Rundate into #tempData from T_MW_GenericPNL 
where UnderlyingSymbolPrice is null or UnderlyingSymbolPrice = 0 and Rundate =@ToDate
order by UnderlyingSymbol

if exists (select * From #tempData)
begin
set @errormsg='Please Update Underlying Symbols Price for below symbols'
select * From #tempData
End

Select @errormsg as ErrorMsg

Drop Table #tempData
