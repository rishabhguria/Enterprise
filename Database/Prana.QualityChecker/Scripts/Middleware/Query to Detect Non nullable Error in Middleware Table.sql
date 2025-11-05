/*
First Run Middleware through query for the date in which issue is coming then run this query for same date.
*/

declare @FromDate datetime
declare @ToDate datetime
Declare @errormsg varchar(max)

set @FromDate=''
set @ToDate=''
set @errormsg=''

Select 
DividendLocal ,
TradePNLMTMBase,
FxPNLMTMBase,
TotalProceeds_Local,
TotalProceeds_Base,
LeveragedFactor,
Open_CloseTag,
Symbol,
Rundate

into #CorruptMiddlewareData
from T_MW_GenericPNL 
Where RunDate=@ToDate
AND
(
DividendLocal IS NULL 
OR
TradePNLMTMBase IS NULL 
OR
FxPNLMTMBase IS NULL 
OR
TotalProceeds_Local IS NULL 
OR
TotalProceeds_Base IS NULL 
OR
LeveragedFactor IS NULL
)



IF EXISTS (Select * from #CorruptMiddlewareData)
BEGIN
set @errormsg ='Non nullable error in middleware detected'
Select * from #CorruptMiddlewareData
END

select @errormsg as ErrorMsg


Drop Table #CorruptMiddlewareData