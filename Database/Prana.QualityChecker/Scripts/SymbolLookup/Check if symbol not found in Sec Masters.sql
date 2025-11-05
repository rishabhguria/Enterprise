
Declare @errormsg varchar(max)

set @errormsg=''

Select 
Symbol


into #SymbolNotInSecMaster
from T_Group TG
Where TG.Symbol not in (Select Tickersymbol from V_SecMasterData)
And TG.CUMQty > 0



IF EXISTS (Select * from #SymbolNotInSecMaster)
BEGIN
set @errormsg ='Trade available in Group table but symbol does not exist in Sec Master'
Select * from #SymbolNotInSecMaster
END

select @errormsg as ErrorMsg


Drop Table #SymbolNotInSecMaster