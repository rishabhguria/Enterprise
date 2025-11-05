Create procedure P_GetSymbolDataForSymbolPks
(
@SymbolPK varchar(max)
)
As

Select * into #tempSplit from dbo.Split(@SymbolPK,',')as Items

SELECT TickerSymbol , SecmasterData.Symbol_PK as SymbolPK, SecmasterData.BloombergSymbol,SecmasterData.CUSIPSymbol, SecmasterData.ISINSymbol,SecmasterData.SEDOLSymbol from V_SecMasterData SecmasterData 
where SecmasterData.Symbol_PK in (select * from #tempSplit)
