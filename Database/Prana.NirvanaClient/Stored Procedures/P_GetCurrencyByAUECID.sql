Create proc P_GetCurrencyByAUECID(@AUECID int )
As
select ta.basecurrencyid, tc.currencysymbol, tc.currencyname
from t_auec as ta 
inner join t_currency as tc on ta.basecurrencyid = tc.currencyid
where 
auecid = @AUECID