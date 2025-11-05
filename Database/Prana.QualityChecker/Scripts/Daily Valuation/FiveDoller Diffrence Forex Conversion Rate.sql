
Declare @FromDate Datetime
Declare @ToDate Datetime

Declare @errormsg varchar(max)

--set @ToDate='12/21/2018'

set @ToDate=''
set @errormsg=''

Set @FromDate=dbo.AdjustBusinessDays(@ToDate,-1,1)

Select *  
into #TempConversionRate
FROM 
T_CurrencyStandardPairs CSP
inner join T_CurrencyConversionRate CCR on CCR.CurrencyPairID_FK=CSP.CurrencyPairID


Select FX1.eSignalSymbol,
FX1.ConversionRate as FXRate1,
FX2.ConversionRate as FXRate2,
FX1.FundID as fundid,
FX1.Date as FXdate
into #tempFXRate
From #TempConversionRate FX1
join 
#TempConversionRate FX2
on FX1.CurrencyPairID_FK= FX2.CurrencyPairID_FK and abs(FX1.ConversionRate - FX2.ConversionRate) >= 5 
and FX1.Date=@FromDate and FX2.Date=@ToDate


IF EXISTS(Select * from #tempFXRate) 
Begin 
SELECT eSignalSymbol, FXRate1,FXRate2,fundid,FXdate from #tempFXRate
Set @errormsg='More then five doller Diffrence in Forex conversion Rate for The Symbol in previous Date' 
END 

SELECT @errormsg AS ErrorMsg 

DROP TABLE #tempFXRate

DROP TABLE #TempConversionRate