Declare @FromDate datetime
Declare @ToDate datetime
Declare @errormsg varchar(max)

set @errormsg=''
set @FromDate=''

set @ToDate=''

if exists( select * from T_CompanyFunds where localcurrency is null or  companyid is null  or fundtypeid is null)
BEGIN
set @errormsg=@errormsg+' T_Companyfunds is inconsistent(Null Values) |' 
select * from T_CompanyFunds where localcurrency is null or  companyid is null  or fundtypeid is null 
END


if exists( select * from T_CompanyUser where companyid is null )
BEGIN
set @errormsg=@errormsg+' T_CompanyUser is inconsistent( UserID is Null )|' 
select * from T_CompanyUser where companyid is null 
END


--if exists(select count(*) from T_CompanyFunds having count(*)<>(select count(*) From T_CashPreferences))
--begin
SELECT CompanyFundID into #T_Missingfunds FROM T_CompanyFunds WHERE CompanyFundID NOT IN 
(SELECT FundID FROM T_CashPreferences)


if exists( select * from #T_Missingfunds  )
BEGIN
set @errormsg=@errormsg+' Some Funds  do not have Cash Management Start Date(T_CashPreferences).' 
select * from #T_Missingfunds
END

--select * from T_CompanyFunds where CompanyFundid not in (select Fundid from T_CashPreferences)



select @errormsg as ErrorMsg 
Drop table #T_Missingfunds


