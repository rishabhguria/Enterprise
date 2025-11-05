Create PROCEDURE P_GetIndexSymbols            
As        
select TickerSymbol as IndexSymbol, CompanyName as ShortName, SecmasterData.Symbol_PK as SymbolPK, SecmasterData.BloombergSymbol,SecmasterData.CUSIPSymbol, SecmasterData.ISINSymbol,SecmasterData.SEDOLSymbol from V_SecMasterData SecmasterData     
inner join T_AUEC auec on SecmasterData.AUECID = auec.AUECID    
inner join T_Asset asset on auec.AssetID = Asset.AssetID    
where auec.AssetID = 7
