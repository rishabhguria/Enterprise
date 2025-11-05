---Description :  To check Missing Missing Beta
Declare @Date datetime
Declare @FromDate datetime
Declare @ToDate datetime
Declare @errormsg varchar(max)



set @FromDate=''
set @ToDate=''
set @errormsg=''


CREATE TABLE #OptionsOpenPositions (
	TickerSymbol VARCHAR(100)
	,UnderlyingSymbol VARCHAR(100)
	,RunDate DATETIME
	)

SET @Date = @FromDate

WHILE (DateDiff(D, @Date, @ToDate) >= 0)
BEGIN
	INSERT INTO #OptionsOpenPositions
	SELECT DISTINCT PT.Symbol
		,SM.UnderlyingSymbol
		,@Date
	FROM PM_Taxlots PT
	INNER JOIN V_SecMasterdata SM ON SM.TickerSymbol = PT.Symbol
	WHERE Taxlot_PK IN (
			SELECT Max(Taxlot_PK)
			FROM PM_Taxlots
			WHERE DateDiff(Day, AUECModifiedDate, @Date) >= 0
			GROUP BY TaxlotID
			)
		AND TaxLotOpenQty <> 0

	SET @Date = DateAdd(D, 1, @Date)
END
--SELECT * FROM #OptionsOpenPositions

SELECT DISTINCT TickerSymbol,UnderlyingSymbol, RunDate into #OptionsOpenPositions_Final FROM #OptionsOpenPositions
--
--Select * from PM_DailyBeta

Select 
RunDate as [Run Date],
TickerSymbol as Symbol,
Beta
Into #MissingBeta
from #OptionsOpenPositions_Final
Left Outer Join PM_DailyBeta ON TickerSymbol=Symbol and RunDate=Date


IF Exists (Select * from #MissingBeta Where Beta is NULL)
Begin

Select * from #MissingBeta 
Where Beta is NULL 
Order By [Run Date],Symbol 

Set @errormsg='There are missing Beta'
End



select @errormsg as ErrorMsg


Drop TABLE #OptionsOpenPositions
Drop TABLE #OptionsOpenPositions_Final
Drop Table #MissingBeta