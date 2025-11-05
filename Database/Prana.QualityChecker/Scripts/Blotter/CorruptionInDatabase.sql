DECLARE @errormsg VARCHAR(MAX)
DECLARE @FundIds VARCHAR(MAX)
DECLARE @FromDate DATETIME
DECLARE @ToDate DATETIME


SET @errormsg=''
SET @FromDate=''
SET @ToDate=''
SET @FundIds=''

Select * into  #tempOrder from T_order where ParentClOrderID not in (select ClOrderID from T_Sub) 

IF EXISTS(Select * from #tempOrder ) 
Begin 
SELECT* from #tempOrder
Set @errormsg='Corruption in database, Some orders of T_sub table are not present in the T_order table' 
END 

SELECT @errormsg AS ErrorMsg 

DROP TABLE #tempOrder