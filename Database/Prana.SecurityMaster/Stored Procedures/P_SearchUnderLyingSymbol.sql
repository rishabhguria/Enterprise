Create Proc P_SearchUnderLyingSymbol
(
@UnderLyingSymbol varchar(20)
)

as 

Select symbol_pk  From T_SmSymbolLookuptable where tickerSymbol=@UnderLyingSymbol