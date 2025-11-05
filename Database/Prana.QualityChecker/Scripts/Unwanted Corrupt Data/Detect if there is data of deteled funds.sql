Declare @Date datetime 
Declare @FromDate datetime 
Declare @ToDate datetime 
Declare @errormsg varchar(max) 



set @FromDate='' 
set @ToDate='' 
set @errormsg='' 


Select  
*
Into #tempPM_NAVValue 
From PM_NAVValue 
Where fundid Not In (Select CompanyFundID from T_CompanyFunds)

IF EXISTS(Select * from #tempPM_NAVValue ) 
Begin 
SELECT* from #tempPM_NAVValue
Set @errormsg='Stale data in PM_NavValue' 
END 

SELECT @errormsg AS ErrorMsg 

DROP TABLE #tempPM_NAVValue
