create FUNCTION [dbo].[F_MW_GetMDReturn_WPSPerformance]         
(        
 @fromDate datetime,          
 @toDate datetime,        
 @groupfield varchar(50),        
 @entity varchar(MAX)        
)        
RETURNS Float        
AS        
BEGIN        
      
        
declare @T_entity table(        
entity varchar(max),      
ITDDate datetime,      
RemainingDay int,      
CashFlow float      
        
)        
        
insert into @T_entity  (Entity)      
select * from dbo.split(@entity, ',')        
      
update @T_Entity      
SET ITDDate=  CASE      
     WHEN  dbo.IsBusinessDay(cashMgmtStartDate,1) = 0        
     THEN dbo.AdjustBusinessDays(cashMgmtStartDate, 1, 1)      
    ELSE cashMgmtStartDate      
     END         
 from t_CashPreferences      
inner join t_CompanyFunds on CompanyfundID=FundID       
inner join @T_Entity on Entity=Fundname      
      
UPDATE @T_Entity      
SET RemainingDay= datediff(d,ITDDate,@toDate) + 1       
      
      
declare @D float       
set @D = DATEDIFF(d,@fromdate, @toDate) + 1        
      
      
declare @NextBusinessDateIfNotBusinessDay datetime        
if dbo.IsBusinessDay(@fromdate,1) = 0        
 begin         
 select @NextBusinessDateIfNotBusinessDay = dbo.AdjustBusinessDays(@fromDate, 1, 1)        
 end         
else        
 begin        
 select @NextBusinessDateIfNotBusinessDay = @fromDate        
 end       
      
declare @BaseCurrencyID int        
select @BaseCurrencyID = BaseCurrencyID from T_Company        
      
      
declare @InvestorCash table        
(fundname varchar(max),payoutdate datetime,cashvalue float)        
      
insert into @InvestorCash        
SELECT Funds.FundName,Journal.TransactionDate AS PayOutDate,case when Journal.CurrencyID = 1 then Journal.CR- Journal.DR else isnull(CurrencyConversionRate.ConversionRate,1)*(Journal.CR- Journal.DR) end AS CashValue        
FROM T_Journal AS Journal         
INNER JOIN T_SubAccounts AS SubAccounts ON Journal.SubAccountID = SubAccounts.SubAccountID         
INNER JOIN T_TransactionType AS TransactionType ON TransactionType.TransactionTypeID = SubAccounts.TransactionTypeID         
INNER JOIN T_CompanyFunds AS Funds ON Funds.CompanyFundID = Journal.FundID         
LEFT OUTER JOIN T_CurrencyStandardPairs AS CurrencyStandardPairs ON CurrencyStandardPairs.FromCurrencyID = Journal.CurrencyID AND CurrencyStandardPairs.ToCurrencyID = @BaseCurrencyID         
LEFT OUTER JOIN T_CurrencyConversionRate AS CurrencyConversionRate ON CurrencyConversionRate.CurrencyPairID_FK = CurrencyStandardPairs.CurrencyPairID AND CurrencyConversionRate.Date = Journal.TransactionDate         
WHERE TransactionType = 'Investor Cash Transactions'        
      
--DECLARE @dd float      
--set @dd =2      
--SELECT ISNULL(@dd,0)      
      
 update @T_entity      
 set CashFlow=  ISNULL((((DATEDIFF(d,IC.PayOutDate, @todate) + 1)*IC.Cashvalue)/E.RemainingDay),0)       
from @InvestorCash IC inner join @T_entity E on IC.fundname=E.entity      
where payoutdate between @fromdate and @todate       
      
--SELECT * FROM @InvestorCash      
--      
--select * FROM @T_entity      
      
declare @BMVsecurities float        
declare @BMVcash float        
declare @cashflow float        
Declare @PNL as float       
DECLARE @result float      
 select @cashflow=sum(ISnull(CashFlow,0)) from @T_entity      
      
--print @cashflow      
      
if @groupfield = 'Fund'        
 Begin      
        
 select @PNL = IsNull(sum(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend),0)       
 from dbo.T_MW_GenericPNL       
 WHERE asset<>'cash' and Rundate between @fromdate and @todate and Fund in (select Entity from @T_Entity)       
      
 select @BMVsecurities = ISnull(sum(beginningmarketvaluebase),0)       
 from dbo.T_MW_GenericPNL       
 where asset<>'cash' and DateDiff(d,TradeDate, @fromdate) > 0 AND rundate = @NextBusinessDateIfNotBusinessDay AND fund =@Entity       
       
 select @BMVcash = IsNull(sum(beginningmarketvaluebase),0)       
 from dbo.T_MW_GenericPNL       
 where asset='cash' AND rundate = @NextBusinessDateIfNotBusinessDay AND fund =@Entity        
       
 select @cashflow=ISnull(SUM(CashFlow),0) from @T_entity      
      
Set @result=@BMVsecurities+@BMVcash+@cashflow      
End      
      
      
Declare @MDreturn float        
select @MDreturn = isnull(@pnl,0) / NULLIF( abs(@result),0)        
        
if @MDreturn <= -1        
begin        
select @MDreturn = -.999        
end        
        
      
--PRINT @MDreturn      
return @MDreturn        
        
END       
      