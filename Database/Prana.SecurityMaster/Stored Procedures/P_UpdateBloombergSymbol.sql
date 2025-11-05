----------------------------------Procedure Start----------------------------------------------------------------

Create Procedure P_UpdateBloombergSymbol
@Execute varchar(10)=NULL
AS
select TickerSymbol,BloombergSymbol=
case when TickerSymbol like '%.%' Then replace(TickerSymbol,'.','/')+' US EQUITY'
     when TickerSymbol like '$%' Then replace(TickerSymbol,'$','')+' INDEX'
else
TickerSymbol +' US EQUITY'
end
into #TempTable 
from T_SMSymbolLookUpTable smlook inner join T_Auec auec on smlook.AUECID=auec.AUECID where datalength(BloombergSymbol)=0 and
smlook.TickerSymbol NOT like '%/%' and
 ExchangeIdentifier IN('NASD-Equity', 'ARCX-Equity', 'NYSE-Equity', 'AMEX-Equity', 'BB-Equity', 'PS-Equity', 'OT-Equity')

Insert into #TempTable
select TickerSymbol,BloombergSymbol=
case when UnderlyingSymbol like '$%' Then substring(UnderlyingSymbol,2,len(UnderlyingSymbol))
else UnderlyingSymbol
end
+ ' US ' + Convert(varchar, ExpirationDate, 1)+ ' ' +
Case
When Type = 1
Then
 'C'
Else
'P'
End
+ convert(varchar, Strike) + ' EQUITY'
 from T_SMOptionData optiond
inner join T_SMSymbolLookupTable smlook on optiond.Symbol_PK=smlook.Symbol_PK inner join T_Auec auec on smlook.auecid=auec.auecid
 where datalength(smlook.BloombergSymbol)=0 and auec.ExchangeIdentifier IN('OPTS-EquityOption')

if (lower(@Execute) like 'yes')
Begin
Update SM set SM.BloombergSymbol = T.BloombergSymbol From T_SMSymbolLookupTable SM inner join #TempTable T
on T.TickerSymbol=SM.TickerSymbol
End
else
Begin
SELECT * FROM #TempTable
End

Drop TABLE #TempTable
