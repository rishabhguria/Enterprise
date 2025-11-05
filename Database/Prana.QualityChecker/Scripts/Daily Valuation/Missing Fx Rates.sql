---Description :  To check Missing FxRates
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
	,AssetID INT
	,AUECID INT
	,RunDate DATETIME
	,CurrencyID INT
	)

SET @Date = @FromDate

WHILE (DateDiff(D, @Date, @ToDate) >= 0)
BEGIN
	INSERT INTO #OptionsOpenPositions
	SELECT DISTINCT PT.Symbol
		,SM.UnderlyingSymbol
		,SM.AssetID
		,SM.AUECID
		,@Date
		,SM.CurrencyID
	FROM PM_Taxlots PT
	INNER JOIN V_SecMasterdata SM ON SM.TickerSymbol = PT.Symbol
	WHERE Taxlot_PK IN (
			SELECT Max(Taxlot_PK)
			FROM PM_Taxlots
			WHERE DateDiff(Day, AUECModifiedDate, @Date) >= 0
			GROUP BY TaxlotID
			)
		AND TaxLotOpenQty <> 0

	INSERT INTO #OptionsOpenPositions (RunDate, CurrencyID) SELECT Date, LocalCurrencyID from PM_CompanyFundCashCurrencyValue 
where Date = @Date and CashValueLocal <> 0
	

	SET @Date = DateAdd(D, 1, @Date)
END

SELECT DISTINCT CurrencyID, RunDate, AUECID into #OptionsOpenPositions_Final FROM #OptionsOpenPositions where CurrencyID <> 1

Create table #OpenPositionsWithCurrencyPairs
(
CurrencyID int,
RunDate datetime,
AUECID int,
ToCurrencyID int,
CurrencyPairID int
)

INSERT INTO #OpenPositionsWithCurrencyPairs
SELECT CurrencyID,RunDate,AUECID,FromCurrencyID,T_CurrencyStandardPairs.CurrencyPairID FROM #OptionsOpenPositions_Final inner JOIN T_CurrencyStandardPairs
on #OptionsOpenPositions_Final.CurrencyID =T_CurrencyStandardPairs.ToCurrencyID
where FromCurrencyID=1
Union
SELECT CurrencyID,RunDate,AUECID,ToCurrencyID,T_CurrencyStandardPairs.CurrencyPairID FROM #OptionsOpenPositions_Final inner JOIN T_CurrencyStandardPairs
on #OptionsOpenPositions_Final.CurrencyID =T_CurrencyStandardPairs.FromCurrencyID
where toCurrencyID=1

ALTER table #OpenPositionsWithCurrencyPairs
add PairIDwithDate nvarchar(100)

UPDATE #OpenPositionsWithCurrencyPairs
SET PairIDwithDate= CAST (CurrencyPairID AS NVARCHAR(3))+'_'+CAST (rundate AS NVARCHAR(50))

Select * Into #MissingFXRates from #OpenPositionsWithCurrencyPairs where PairIDwithDate NOT in(SELECT  PairIDwithDate FROM #OpenPositionsWithCurrencyPairs OPWCP inner join T_CurrencyConversionRate CCR
on OPWCP.CurrencyPairID=CCR.CurrencyPairID_FK AND OPWCP.RunDate=CCR.Date) 

SELECT 
MFX.RunDate,
TC.CurrencyID,
TC.CurrencyName,
TC.CurrencySymbol,
TCSP.eSignalSymbol,
MFX.AUECID 
INTO #FinalMissingFXRates
FROM #MissingFXRates MFX
inner join T_currency TC on MFX.CurrencyID=TC.CurrencyID
inner join T_CurrencyStandardPairs TCSP ON TCSP.CurrencyPairID=MFX.CurrencyPairID
where dbo.IsBusinessDay(RunDate,auecid)=1


if exists(Select * from #FinalMissingFXRates)
BEGIN

set @errormsg='Fx rates Missing.'

Select * from #FinalMissingFXRates


END


select @errormsg as ErrorMsg


DROP TABLE #OptionsOpenPositions
DROP TABLE #OptionsOpenPositions_Final
DROP TABLE #MissingFXRates
Drop TABLE #FinalMissingFXRates
DROP table #OpenPositionsWithCurrencyPairs