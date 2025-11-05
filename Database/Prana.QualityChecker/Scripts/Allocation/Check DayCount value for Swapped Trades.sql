Declare @Date datetime 
Declare @FromDate datetime 
Declare @ToDate datetime 
Declare @errormsg varchar(max) 

set @FromDate='' 
set @ToDate='' 
set @errormsg='' 

Select SP.GroupID, SP.DayCount into #temp 
from T_SwapParameters SP WHERE SP.DayCount=0

IF EXISTS (SELECT * FROM #temp)
BEGIN
SELECT * FROM #temp
SET @errormsg = 'Value of DayCount column is 0 for below GroupIds in T_SwapParameters table'
END

SELECT @errormsg AS errormsg
DROP TABLE #temp