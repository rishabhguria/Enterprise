-- Query for identify same Ticker and Underlying symbol for Options

DECLARE @errormsg varchar(max)

SET @errormsg=''

CREATE TABLE #tempTickerInfo
(
TickerSymbol Varchar(200) ,
UnderLyingSymbol Varchar(200) 
)
INSERT INTO #tempTickerInfo
SELECT DISTINCT TickerSymbol, UnderLyingSymbol
FROM V_SecMasterData
WHERE (AssetID=2 or AssetID=4) and (TickerSymbol=UnderLyingSymbol)

IF  exists( SELECT * FROM #tempTickerInfo)
BEGIN
SET @errormsg='Some symbols have same Ticker Symbol and Underlying for Options '
SELECT DISTINCT TickerSymbol AS TickerSymbol, UnderLyingSymbol AS UnderLyingSymbol
FROM #tempTickerInfo
END

SELECT @errormsg AS ErrorMsg


DROP TABLE #tempTickerInfo