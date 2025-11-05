Declare @Date datetime 
Declare @FromDate datetime 
Declare @ToDate datetime 
Declare @errormsg varchar(max) 

set @FromDate='' 
set @ToDate='' 
set @errormsg='' 

Select G.Symbol, G.Aueclocaldate , G.Quantity, G.GroupID, G.CurrencyID into #tempSwapData 
from T_group G where G.Isswapped =1 
and G.groupid not in (Select s.groupid from T_Swapparameters s)

IF EXISTS(Select * from #tempSwapData) 
Begin 
SELECT * from #tempSwapData
Set @errormsg='Swap Parameters are not present in T_SwapParameters Table for the below symbols booked as Swap.' 
END 

SELECT @errormsg AS ErrorMsg 

DROP TABLE #tempSwapData