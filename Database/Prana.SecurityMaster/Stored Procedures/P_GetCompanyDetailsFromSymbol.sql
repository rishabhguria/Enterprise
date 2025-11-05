CREATE proc [dbo].[P_GetCompanyDetailsFromSymbol]      
(      
@symbol varchar(100)      
)      
as      
if(@symbol is not null)      
select ENHD.CompanyName,SM.BloombergSymbol,      
SM.ISINSymbol,SM.SEDOLSymbol,SM.CUSIPSymbol,SM.TickerSymbol      
from T_SMSymbolLookUpTable as SM      
join T_SMEquityNonHistoryData as ENHD on SM.Symbol_PK=ENHD.Symbol_PK      
where  TickerSymbol like  COALESCE(@symbol,TickerSymbol) 