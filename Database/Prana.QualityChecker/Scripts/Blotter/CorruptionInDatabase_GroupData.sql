DECLARE @errormsg VARCHAR(MAX)
DECLARE @FundIds VARCHAR(MAX)
DECLARE @FromDate DATETIME
DECLARE @ToDate DATETIME


SET @errormsg=''
SET @FromDate=''
SET @ToDate=''
SET @FundIds=''

-- Date check is specially applied in this script to avoid showing old data which is already corrupted.
Select * into #tempgroup from T_group G where G.Groupid not in (Select O.Groupid from T_Tradedorders O where O.groupid is not null)
and G.assetid not in (5,11) and datediff(dd, aueclocaldate, '2018-06-08') <=0
and G.TransactionSource in (1,2,3,4,14)

IF EXISTS(Select * from #tempgroup) 
Begin 
SELECT Symbol, OrderSideTagValue, GroupID, Quantity, CumQty, AvgPrice, AuecLocalDate  from #tempgroup
Set @errormsg='Corruption in database, Some orders of T_group table are not present in the T_Tradedorders table' 
END 

SELECT @errormsg AS ErrorMsg 

DROP TABLE #tempgroup