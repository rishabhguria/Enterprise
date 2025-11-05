declare @FromDate datetime
declare @ToDate datetime
Declare @errormsg varchar(max)
Declare @Smdb Varchar(max)

set @FromDate=''
set @ToDate=''
set @errormsg=''
set @Smdb=''

DECLARE @IndexInputQuery VARCHAR(MAX)

CREATE TABLE #SymbolPK 
(
	TickerSymbol VARCHAR(MAX),
	SymbolPK VARCHAR(MAX)	
)

INSERT INTO #SymbolPK
	SELECT
		TickerSymbol,
		Symbol_PK
	FROM V_SecMasterData_WithUnderlying

CREATE TABLE #SymbolPKMissing(TickerSymbol VARCHAR(MAX),SymbolPK VARCHAR(MAX))
BEGIN
SET @IndexInputQuery = 'INSERT INTO #SymbolPKMissing SELECT TickerSymbol, SymbolPK FROM #SymbolPK WHERE SymbolPK NOT IN 
						(SELECT Symbol_PK FROM 
						['+@Smdb+'].dbo.T_UDA_DynamicUDAData)'
EXEC (@IndexInputQuery)
END

ALTER TABLE #SymbolPKMissing
ADD Comments VARCHAR (max)

IF EXISTS (SELECT
	*
FROM #SymbolPKMissing)
BEGIN
	SET @errormsg = 'SM Data Corruption'
	UPDATE #SymbolPKMissing
	SET Comments = 'Dynamic UDA Data Missing'
END

SELECT @errormsg AS errormsg

DROP TABLE #SymbolPK, #SymbolPKMissing