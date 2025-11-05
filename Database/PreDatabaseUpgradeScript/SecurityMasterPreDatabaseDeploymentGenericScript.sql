GO

DECLARE @smsql varchar(1000);
DECLARE @smtableName sysname;
DECLARE @smstatName sysname;
DECLARE smstatCursor CURSOR FOR   
Select OBJECT_NAME(s.[object_id]) AS TableName,             
s.[name] AS StatName        
FROM sys.stats s       
WHERE OBJECTPROPERTY(s.OBJECT_ID,'IsUserTable') = 1             
AND s.name LIKE '_dta_stat%';
 
OPEN smstatCursor;
FETCH smstatCursor INTO @smtableName, @smstatName;
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @smsql = 'DROP STATISTICS [' + @smtableName + '].[' + @smstatName + ']';
    PRINT @smsql;
    EXEC (@smsql);
      FETCH smstatCursor INTO @smtableName, @smstatName;
END
 
CLOSE smstatCursor;
DEALLOCATE smstatCursor;

------------------------------------------------
-- Replace Null With Default Values
------------------------------------------------
Update T_SMFixedIncomeData
Set BondTypeID=0 Where BondtypeID IS NULL
-------------------------------------------------
--Prod Security Master Database Cleaning Script
-------------------------------------------------
GO
Delete T_SMOptionData  from 
T_SMSymbolLookUpTable inner join T_SMOptionData on T_SMOptionData.Symbol_PK=T_SMSymbolLookUpTable.Symbol_PK
 where T_SMSymbolLookUpTable.AUECID not in (select AUECID from T_AUEC);

GO
Delete T_SMReuters  from T_SMSymbolLookUpTable inner join T_SMReuters on T_SMReuters.Symbol_PK=T_SMSymbolLookUpTable.Symbol_PK
 where T_SMSymbolLookUpTable.AUECID not in (select AUECID from T_AUEC);

GO
Delete T_SMFutureData  from T_SMSymbolLookUpTable inner join T_SMFutureData on T_SMFutureData.Symbol_PK=T_SMSymbolLookUpTable.Symbol_PK
 where T_SMSymbolLookUpTable.AUECID not in (select AUECID from T_AUEC);

GO
Delete from T_SMEquityNonHistoryData where Symbol_PK in (select Symbol_PK from T_SMSymbolLookUpTable where auecid is null or auecid ='')

GO
delete from T_SMSymbolLookUpTable  where T_SMSymbolLookUpTable.AUECID not in (select AUECID from T_AUEC);

GO
delete T_SMReuters  from T_SMSymbolLookUpTable inner join T_SMReuters on T_SMReuters.Symbol_PK=T_SMSymbolLookUpTable.Symbol_PK
 where T_SMSymbolLookUpTable.ExchangeID not in (select ExchangeID from T_Exchange);

GO
delete from T_SMFutureData where Symbol_PK in (select Symbol_pk from T_SMSymbolLookUpTable where T_SMSymbolLookUpTable.ExchangeID not in (select ExchangeID from T_Exchange));
 
GO
delete from T_SMSymbolLookUpTable where T_SMSymbolLookUpTable.ExchangeID not in (select ExchangeID from T_Exchange);

----------------------------------------------------------
-- 
----------------------------------------------------------