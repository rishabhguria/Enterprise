Declare @errormsg varchar(max)
declare @FundIds varchar(max)
Declare @FromDate datetime
Declare @ToDate datetime

set @errormsg=''
set @FromDate=''
set @ToDate=''
set @FundIds=''


SELECT MAX(PM.AUECModifiedDate) AS TradeDate, MAX(PM.Symbol) AS Symbol, MAX(TF.FileFormatName) AS [File Format]
INTO #UNSENTDATA
FROM PM_Taxlots PM INNER JOIN T_PBWiseTaxlotState PB ON PM.TaxLotID = PB.TaxLotID
INNER JOIN T_ThirdPartyFileFormat TF ON TF.FileFormatId = PB.FileFormatID
WHERE PB.TaxLotState <> 1 AND PM.TaxLotClosingId_Fk IS NULL 
and DATEDIFF(DAY,@FromDate,PM.AUECModifiedDate)>=0 AND DATEDIFF(DAY,@ToDate,PM.AUECModifiedDate)<=0
GROUP BY PM.TaxLotID, PB.FileFormatID, PM.AUECModifiedDate
ORDER BY PM.AUECModifiedDate

IF  exists( select * from #UNSENTDATA)
BEGIN
	SET @errormsg='Un-Sent Symbols in Third Party'
	SELECT * FROM #UNSENTDATA
END

SELECT @errormsg AS ErrorMsg

DROP TABLE #UNSENTDATA


