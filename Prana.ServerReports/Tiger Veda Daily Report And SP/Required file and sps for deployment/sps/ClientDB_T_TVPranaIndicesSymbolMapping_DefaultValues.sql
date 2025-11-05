PRINT 'Started executing ClientDB_T_TVPranaIndicesSymbolMapping_DefaultValues.sql'
GO
IF Exists(select name FROM sys.tables where Name = N'T_TVPranaIndicesSymbolMapping')
Begin
Delete T_TVPranaIndicesSymbolMapping
Insert InTo T_TVPranaIndicesSymbolMapping(eSignalSymbol,TVSymbol,OrderBy)

SELECT 	'$SPX',	'S&P',1	
UNION ALL
SELECT 	'$INDU','DJIA',	2

UNION ALL
SELECT 	'$NDX',	'NDX 100',3

UNION ALL
SELECT 	'$SXXP-STX','EUROSTOXX 600',4


UNION ALL
SELECT 	'$XAU','GOLD',	5

UNION ALL
SELECT 	'$HGA','COPPER',6

UNION ALL
SELECT 	'$NI225-NKI','NIKKEI',	7
UNION ALL
SELECT 	'$COMPOSITE-JKT','JCI',	8
UNION ALL
SELECT 	'$IME-MEX','MEXBOL',9
UNION ALL
SELECT 	'$IBOV-BSP','BOVESPA',	10

UNION ALL
SELECT 	'$SENSEX-BOM','SENSEX',	11

UNION ALL
SELECT 	'$DXY-FTSA','DXY',	12

UNION ALL
SELECT 	'BRL','BRL',13
UNION ALL
SELECT 	'EUR','EUR',14
UNION ALL
SELECT 	'IDR','IDR',15
UNION ALL
SELECT 	'INR','INR',16

End

GO
PRINT 'Done executing ClientDB_T_TVPranaIndicesSymbolMapping_DefaultValues.sql'
GO




