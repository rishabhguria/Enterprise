
create Procedure GetCurrencyStandardPairs
As
Select 
FromCurrencyID,
ToCurrencyId

from T_CurrencyStandardPairs order by fromcurrencyID