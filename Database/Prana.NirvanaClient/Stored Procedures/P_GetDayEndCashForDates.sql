-----------------------------------------------------------------
--created by: SURENDRA BISHT
--date: 23-july-14
--purpose: Get the day end cash for different funds
--usage: P_GetDayEndCashForDates '1223,1238','2014/7/1','2014/7/23'
-----------------------------------------------------------------
CREATE PROCEDURE P_GetDayEndCashForDates 
	
	@fundIDs varchar(max), 
	@startDate datetime,
    @endDate datetime 
AS
BEGIN
Create Table #Funds (FundID int)                                                                                                                   
Insert into #Funds                                                          
Select Items as FundID from dbo.Split(@fundIDs,',')
select cash.* from 
T_CashPreferences pf inner join PM_CompanyFundCashCurrencyValue cash 
on pf.FundID =cash.FundID   
and  datediff(d,pf.CashMgmtStartDate,@endDate)>=0  
and cash.fundid in(SELECT fundid from #Funds)
where
(cash.date=
case 
WHEN datediff(d, DATEADD(day,-1,pf.CashMgmtStartDate), @startDate)>0
then @startDate
ELSE 
DATEADD(day,-1,pf.CashMgmtStartDate)
end
)
END

