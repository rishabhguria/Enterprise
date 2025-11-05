declare	@FromDate Datetime 
declare @ToDate Datetime
declare @ErrorMsg Varchar(Max)
declare @Smdb Varchar(max)
 
set @ToDate=''
set @FromDate=''
Set @ErrorMsg=''
set @Smdb=''

declare @InputQuery VARCHAR(MAX)

CREATE TABLE #Symbol 
(
	TickerSymbol VARCHAR(MAX)	
)

BEGIN
SET @InputQuery = 'INSERT INTO #Symbol SELECT TickerSymbol FROM ['+@Smdb+'].dbo.T_SMSymbolLookUpTable where CHARINDEX('','', TickerSymbol) > 0'
EXEC (@InputQuery)
END


IF EXISTS (SELECT * FROM #Symbol)
BEGIN
SET @errormsg='Comma present in Ticker Symbol in T_SMSymbolLookUpTable'
	SELECT TickerSymbol FROM #Symbol
END

SELECT @errormsg AS ErrorMsg

DROP TABLE #Symbol
