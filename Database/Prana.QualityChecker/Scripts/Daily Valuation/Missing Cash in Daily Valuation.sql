---Description : To check fund not in T_Journal which don't have Acronym='Cash' 
Declare @Date datetime 
Declare @FromDate datetime 
Declare @ToDate datetime 
Declare @errormsg varchar(max) 



set @FromDate='' 
set @ToDate='' 
set @errormsg='' 

CREATE TABLE #Fundtest( 
fund varchar(100) 
) 

INSERT INTO #Fundtest 
 
SELECT DISTINCT FundID from T_Journal WHERE SubAccountID  IN 
(select SubAccountID from T_SubAccounts WHERE Acronym='Cash' ) 
select CompanyFundID,FundName into #ShowFund from T_CompanyFunds WHERE CompanyFundID  not in (select fund from #Fundtest)
IF EXISTS(Select * from #ShowFund ) 
Begin 
SELECT* from #ShowFund
Set @errormsg='There are funds which have no cash!' 
END 

SELECT @errormsg AS ErrorMsg 

DROP TABLE #Fundtest, #ShowFund