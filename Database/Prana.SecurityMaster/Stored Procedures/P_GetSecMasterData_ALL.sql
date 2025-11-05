
--P_GetSecMasterData_ALL 1

  
CREATE proc [dbo].[P_GetSecMasterData_ALL]  
(
@countryID int 
)
as  
  
select T_SMSymbolLookUpTable.AssetID,T_SMSymbolLookUpTable.UnderLyingID,T_SMSymbolLookUpTable.ExchangeID,T_SMSymbolLookUpTable.CurrencyID,
T_SMSymbolLookUpTable.AUECID,TickerSymbol,BloombergSymbol,SEDOLSymbol,T_SMReuters.ReutersSymbol,CUSIPSymbol as CusipSymbol,
UnderlyingSymbol,ISINSymbol,T_SMSymbolLookUpTable.Symbol_PK from T_SMSymbolLookUpTable
join  T_AUEC on T_AUEC.AUECID = T_SMSymbolLookUpTable.AUECID
join T_SMReuters on T_SMReuters.Symbol_PK=T_SMSymbolLookUpTable.Symbol_PK

  where T_AUEC.Country=@countryID
  





