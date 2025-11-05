---Description :  To check Lower case BloombergSymbol

Declare @errormsg varchar(max)
set @errormsg=''







SELECT 
BloombergSymbol as [Bloomberg Symbol]
Into #LcaseBloombergSymbol
FROM V_SecMasterData 
where CONVERT(VARBINARY(250),UPPER(lTRIM(RTRIM(Replace(BloombergSymbol,'Equity','EQUITY'))))) != CONVERT(VARBINARY(250),lTRIM(RTRIM(Replace(BloombergSymbol,'Equity','EQUITY'))))



if  exists( select * from #LcaseBloombergSymbol)
begin

set @errormsg='Bloomberg Symbols should be in Upper case'
select * from #LcaseBloombergSymbol

END

select @errormsg as ErrorMsg
drop table #LcaseBloombergSymbol