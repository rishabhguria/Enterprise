
CREATE Function DBO.GetLastFillAvgPriceAndExecTime(@ClOrder Varchar(50))
Returns Table
As

Return(

Select ClOrderID,AveragePrice, TransactTime
,Quantity 
from T_Fills With (NoLock)
Where ClOrderID = @ClOrder  
And MsgSeqNumber = (Select Max(MsgSeqNumber) from T_Fills With (NoLock)Where ClOrderID = @ClOrder)
);