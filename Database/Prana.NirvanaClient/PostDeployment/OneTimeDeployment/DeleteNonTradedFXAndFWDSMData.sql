  BEGIN TRAN TRAN1  
  BEGIN TRY     
  
CREATE Table #TempSymbolDetails
(
 Symbol varchar(100)
,SymbolPK bigint
,AssetID int 
)

INSERT INTO #TempSymbolDetails
SELECT TickerSymbol,Symbol_PK,AssetID FROM [$(SecurityMaster)].[dbo].[T_SMSymbolLookUpTable]
 WHERE TickerSymbol NOT IN
(SELECT  DISTINCT(Symbol) FROM T_Group 
 WHERE T_Group.AssetID IN (5,11)) AND AssetID IN(5,11)

DELETE FROM [$(SecurityMaster)].[dbo].[T_SMreuters] WHERE Symbol_PK IN 
(SELECT SymbolPK FROM  #TempSymbolDetails)

DELETE FROM [$(SecurityMaster)].[dbo].[T_SMFxData] WHERE Symbol_PK IN 
( SELECT SymbolPK FROM  #TempSymbolDetails)

DELETE FROM [$(SecurityMaster)].[dbo].[T_SMSymbolLookUpTable] 
WHERE AssetID IN (5,11) AND
[$(SecurityMaster)].[dbo].[T_SMSymbolLookUpTable].[TickerSymbol] IN 
(SELECT Symbol FROM  #TempSymbolDetails)
DROP TABLE #TempSymbolDetails
COMMIT TRANSACTION TRAN1 
 END TRY                                                                                                                                                          
  BEGIN CATCH                                                     
	ROLLBACK TRANSACTION TRAN1                             
  END CATCH;
  
  