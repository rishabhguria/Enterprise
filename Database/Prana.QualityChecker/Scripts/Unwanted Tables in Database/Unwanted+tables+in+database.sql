Declare @errormsg varchar(max)
declare @FundIds varchar(max)
Declare @FromDate datetime
Declare @ToDate datetime
Declare @Smdb Varchar(max)
Declare @dbName VARCHAR(MAX)
DECLARE @varSQL VARCHAR(512)

Select @dbName = DB_NAME()

set @errormsg=''
set @FromDate=''
set @ToDate=''
set @FundIds=''
set @Smdb=''

CREATE TABLE #TmpTable 
(
	DBName VARCHAR(256),
	TableName VARCHAR(256)
)

-- check temp and backup tables in client db
SET @varSQL = 'USE [' + @DBName + '];
	INSERT INTO #TmpTable
	SELECT '''+ @DBName + ''' AS DBName,
	name AS TableName
	FROM sys.tables
	WHERE (name LIKE ''%' + 'temp' + '%'' Or name LIKE ''%' + 'back' + '%'' Or name LIKE ''%' + '[0-9]' + '%'')
	And name <> (''T_Level2Allocation'') And name <> (''T_ReportTemplate'') And name <> (''T_BTTemplateList'')'

EXEC (@varSQL)

-- check temp and backup tables in SM db

SET @varSQL = 'USE [' + @Smdb + '];
	INSERT INTO #TmpTable
	SELECT '''+ @Smdb + ''' AS DBName,
	name AS TableName
	FROM sys.tables
	WHERE (name LIKE ''%' + 'temp' + '%'' or name LIKE ''%' + 'back' + '%'' or name LIKE ''%' + '[0-9]' + '%'')'

EXEC (@varSQL)

IF  Exists(select * from #TmpTable)
BEGIN
	Set @errormsg='Tables not in used'
	Select * From #TmpTable
END

Select @errormsg as ErrorMsg

DROP TABLE #TmpTable