DECLARE @errormsg VARCHAR(MAX)
DECLARE @FundIds VARCHAR(MAX)
DECLARE @FromDate DATETIME
DECLARE @ToDate DATETIME


SET @errormsg=''
SET @FromDate=''
SET @ToDate=''
SET @FundIds=''

SELECT VS.TickerSymbol, CUR1.CurrencySymbol AS 'Group Curreny', CUR.CurrencySymbol AS 'SM Currency', GroupID
INTO #TempDifferCurrency
FROM V_SecMasterData_WithUnderlying VS 
INNER JOIN T_Group G ON VS.TickerSymbol = G.Symbol
INNER JOIN T_Currency CUR ON CUR.CurrencyID = VS.CurrencyID
INNER JOIN T_Currency CUR1 ON CUR1.CurrencyID = G.CurrencyID
WHERE VS.CurrencyID <> G.CurrencyID
And G.CumQty > 0

IF EXISTS(SELECT * FROM #TempDifferCurrency)
BEGIN
	SET @errormsg='Traded Currency different from SM Currency.'
	SELECT * from #TempDifferCurrency
END

SELECT @errormsg AS ErrorMsg


DROP TABLE #TempDifferCurrency