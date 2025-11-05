
Declare @errormsg varchar(max)

set @errormsg=''

Select 
TickerSymbol,
BloombergSymbol,
Multiplier


into #ZeroMultiplierData
from V_SecMasterData 
Where Multiplier = 0 or Multiplier is null



IF EXISTS (Select * from #ZeroMultiplierData)
BEGIN
set @errormsg ='Zero Or Null Multiplier detected'
Select * from #ZeroMultiplierData
END

select @errormsg as ErrorMsg


Drop Table #ZeroMultiplierData