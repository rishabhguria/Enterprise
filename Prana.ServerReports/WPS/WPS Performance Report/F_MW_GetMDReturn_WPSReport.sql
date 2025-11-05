 
-- =============================================          
-- Author:  <Alan Hau>          
-- Create date: <12/29/2010>          
-- Description: <returns the Modified Dietz Return given date range for the entity, note this is for non-linked returns that are less than a month. For linked returns, use F_getLinkedMDReturn>          
--    2/3/2011-- expanded to include calculating returns for multi-entities          
-- Sample:  select dbo.F_MW_GetMDReturn('7/1/2010','7/2/2010', 'Symbol', 'SPY')            
--    select dbo.F_MW_GetMDReturn('7/1/2010','7/2/2010', 'Aggregate', '')           
--    select dbo.F_MW_GetMDReturn('1/1/2011','1/31/2011', 'Fund', 'UBS Series,May 09 Series')           
--    select dbo.F_MW_GetMDReturn('1/1/2011','1/31/2011', 'Fund', 'May 09 Series')           
--    select dbo.F_MW_GetMDReturn('1/1/2011','1/31/2011', 'Symbol', 'SPY,C')           
-- =============================================          
CREATE FUNCTION [dbo].[F_MW_GetMDReturn_WPSReport]           
(          
 @fromDate datetime,            
 @toDate datetime,          
 @groupfield varchar(50),          
 @entity varchar(MAX)          
)          
RETURNS Float          
AS          
BEGIN          
          
declare @result float          
          
declare @test float          
          
declare @T_entity table(          
entity varchar(max)          
)          
          
insert into @T_entity          
select * from dbo.split(@entity, ',')          
          
declare @D float  --total days in period          
set @D = DATEDIFF(d,@fromdate, @toDate) + 1          
          
          
declare @daysRemain float --days remaining in period          
set @daysRemain = @D          
          
declare @LastBusinessDateOfFromdate datetime          
set @LastBusinessDateOfFromdate = dbo.AdjustBusinessDays(@fromDate, -1, 1)          
          
declare @NextBusinessDateIfNotBusinessDay datetime          
if dbo.IsBusinessDay(@fromdate,1) = 0          
 begin           
 select @NextBusinessDateIfNotBusinessDay = dbo.AdjustBusinessDays(@fromDate, 1, 1)          
 end           
else          
 begin          
 select @NextBusinessDateIfNotBusinessDay = @fromDate          
 end          
          
declare @BMVsecurities float          
declare @BMVcash float          
declare @cashflow float          
          
Declare @PNL as float          
        
Declare @AdditionalPNL as float          
          
declare @BaseCurrencyID int          
select @BaseCurrencyID = BaseCurrencyID from T_Company          
          
declare @InvestorCash table          
(fundname varchar(max),payoutdate datetime,cashvalue float)          
          
insert into @InvestorCash          
SELECT Funds.FundName,Journal.TransactionDate AS PayOutDate,case when Journal.CurrencyID = @BaseCurrencyID then Journal.CR- Journal.DR else isnull(CurrencyConversionRate.ConversionRate,1)*(Journal.CR- Journal.DR) end AS CashValue          
FROM T_Journal AS Journal           
INNER JOIN T_SubAccounts AS SubAccounts ON Journal.SubAccountID = SubAccounts.SubAccountID           
INNER JOIN T_TransactionType AS TransactionType ON TransactionType.TransactionTypeID = SubAccounts.TransactionTypeID           
INNER JOIN T_CompanyFunds AS Funds ON Funds.CompanyFundID = Journal.FundID           
LEFT OUTER JOIN T_CurrencyStandardPairs AS CurrencyStandardPairs ON CurrencyStandardPairs.FromCurrencyID = Journal.CurrencyID AND CurrencyStandardPairs.ToCurrencyID = @BaseCurrencyID           
LEFT OUTER JOIN T_CurrencyConversionRate AS CurrencyConversionRate ON CurrencyConversionRate.CurrencyPairID_FK = CurrencyStandardPairs.CurrencyPairID AND CurrencyConversionRate.Date = Journal.TransactionDate           
WHERE TransactionType = 'Investor Cash Transactions'          
        
        
        
          
if @groupfield = 'Aggregate'          
 Begin          
 select @PNL = sum(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend) from dbo.T_MW_GenericPNL WHERE asset<>'cash' and Rundate between @fromdate and @todate          
 select @BMVsecurities = sum(beginningmarketvaluebase) from dbo.T_MW_GenericPNL where asset<>'cash' and DateDiff(d,TradeDate, @fromdate) > 0 AND rundate = @NextBusinessDateIfNotBusinessDay          
 select @BMVcash = sum(beginningmarketvaluebase) from dbo.T_MW_GenericPNL where asset='cash' and rundate = @NextBusinessDateIfNotBusinessDay          
 select @cashflow = sum(((DATEDIFF(d,PayOutDate, @todate) + 1)/@D)*Cashvalue) from @InvestorCash where payoutdate between @fromdate and @todate          
          
 set @BMVsecurities = ISNULL(@BMVsecurities,0)          
 set @BMVcash = ISNULL(@BMVcash,0)          
 set @cashflow = ISNULL(@cashflow,0)          
           
 set @result = @BMVsecurities + @BMVcash + @cashflow          
 End            
           
if @groupfield = 'Fund'          
 Begin          
        
 declare @tempPNL table       
(      
subcategoryID int,         
SubCategoryName varchar(200),        
Name varchar(200),        
TransactionTypeID varchar(200),        
FundID int ,        
SubAccountID int,        
CurrencyID int,        
TransactionDate datetime,        
DR float,        
CR float       
)      
      
insert INTO @tempPNL      
SELECT         
a.subcategoryID,         
a.SubCategoryName ,        
b.Name,        
b.TransactionTypeID,        
c.FundID,        
c.SubAccountID,        
c.CurrencyID,        
c.TransactionDate,        
c.DR,        
c.CR        
from         
T_SubCategory a        
inner join T_SubAccounts b on b.acronym<>'cash'          
  and a.SubCategoryID=b.SubCategoryID        
          
inner join T_journal c on b.SubaccountID=c.SubaccountID         
  and FundID in         
   (SELECT CompanyFundID from t_companyfunds         
   WHERE FundName in(select * from @T_Entity))        
  and ((DATEDIFF(d,@fromDate,c.TransactionDate)>=0) AND DATEDIFF(d,c.TransactionDate,@todate)>=0)        
  and b.Acronym <> 'TAX'        
        
Where ((MasterCategoryID in ('3','4','5') and TransactionTypeID<>3)        
or (TransactionTypeID=3 and (TransactionSource in (2,6))))      
and a.SubCategoryName in ('Interest Income','Interest Expense','Misc Income','Misc Expense')      
and Transactiondate between @fromDate and @todate      
      
      
       
DECLARE  @FXConversionRates Table                               
(                                                                                                                                                          
 FromCurrencyID int,                 
 ToCurrencyID int,                                                       
 RateValue float,                         
 ConversionMethod int,                                                                                 
 Date DateTime,                                
 eSignalSymbol varchar(max)        
                                                         
)          
        
Insert into @FXConversionRates                
      
  Select StanPair.FromCurrencyID AS FromCurrencyID,                                    
      StanPair.ToCurrencyID AS ToCurrencyID,                                    
      Isnull(CCR.ConversionRate,0) AS RateValue,                                     
      0 AS ConversionMethod,                                     
      CCR.Date AS Date,                                    
      StanPair.eSignalSymbol AS eSignalSymbol                                    
   From T_CurrencyConversionRate AS CCR                                    
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK                               
   WHERE DateDiff(d,@fromDate,CCR.Date) >=0 -- dbo.GetFormattedDatePart(CCR.Date) >= dbo.GetFormattedDatePart(@startingDate)                        
  AND DateDiff(d,CCR.Date,@todate) >=0 -- dbo.GetFormattedDatePart(CCR.Date) <= dbo.GetFormattedDatePart(@endingDate)                          
                                  
  UNION                                  
                                  
  Select StanPair.ToCurrencyID AS FromCurrencyID,             
      StanPair.FromCurrencyID AS ToCurrencyID,                                    
      Isnull(CCR.ConversionRate,0) AS RateValue,                                     
      1 AS ConversionMethod,                                     
      CCR.Date AS Date,                                    
      StanPair.eSignalSymbol AS eSignalSymbol                             
   from T_CurrencyConversionRate AS CCR                                    
   INNER JOIN T_CurrencyStandardPairs StanPair on StanPair.CurrencyPairId = CCR.CurrencyPairID_FK                                 
   WHERE DateDiff(d,@fromDate,CCR.Date) >=0 --dbo.GetFormattedDatePart(CCR.Date) >= dbo.GetFormattedDatePart(@startingDate)             
  AND DateDiff(d,CCR.Date,@todate) >=0 -- dbo.GetFormattedDatePart(CCR.Date) <= dbo.GetFormattedDatePart(@endingDate)         
                                                                                                                                      
--  Select * from  dbo.GetAllFXConversionRatesForGivenDateRange(@MinTradeDate,@EndDate) as A                                                                                                                                       
                                                                
 UPDATE @FXConversionRates                                                                                   
 Set RateValue = 1.0/RateValue                                                                                                          
 Where RateValue <> 0 and ConversionMethod = 1                                                                                                                                        
                                                                                             
UPDATE @FXConversionRates                                                                                                                                     
 Set RateValue = 0                                                                
 Where RateValue is Null        
      
        
UPDATE @tempPNL         
SET DR = DR * RateValue,         
CR = CR * RateValue         
from @FXConversionRates         
WHERE ToCurrencyID = '1' and Date=TransactionDate and CurrencyID = FromCurrencyID        
        
UPDATE @tempPNL SET DR=-DR          
       
 select @AdditionalPNL = SUM(DR + CR)  from @tempPNL       
set @AdditionalPNL = ISNULL(@AdditionalPNL,0)      
        
    
 select @PNL = sum(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend) from dbo.T_MW_GenericPNL WHERE Asset not in ('Cash','Accruals')  and Rundate between @fromdate and @todate and Fund in (select * from @T_Entity)            
    -----Adj PnL with non-trading Income/Exp but not Cash Contributions and Withdrawls            
--select @PNL as nornalpnlstart         
 --select @PnL=@PnL+ isnull(sum(CashValue),0) from @InvestorCash where payoutdate between @fromdate and @todate and Fundname in (select * from @T_Entity) and MasterCategoryName<>'Equity'            
      
 select @BMVsecurities = sum(isnull(beginningmarketvaluebase,0)) from dbo.T_MW_GenericPNL where asset<>'cash' and DateDiff(d,TradeDate, @fromdate) > 0 AND rundate = @NextBusinessDateIfNotBusinessDay AND fund in (select * from @T_Entity)            
 select @BMVcash = sum(isnull(beginningmarketvaluebase,0)) from dbo.T_MW_GenericPNL where asset='cash' AND rundate = @NextBusinessDateIfNotBusinessDay AND fund in (select * from @T_Entity)            
 set @BMVsecurities = ISNULL(@BMVsecurities,0)            
 set @BMVcash = ISNULL(@BMVcash,0)            
 set @cashflow = ISNULL(@cashflow,0)           
 select @PNL = @PNL + isnull(@AdditionalPNL,0)       
 set @result = @BMVsecurities + @BMVcash + @cashflow            
      
 End            
           
 if @groupfield = 'Asset'          
 Begin          
 select @PNL = sum(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend) from dbo.T_MW_GenericPNL WHERE Rundate between @fromdate and @todate and asset in (select * from @T_Entity)          
 select @BMVsecurities = sum(beginningmarketvaluebase) from dbo.T_MW_GenericPNL where DateDiff(d,TradeDate, @fromdate) > 0 AND rundate = @NextBusinessDateIfNotBusinessDay AND asset in (select * from @T_Entity)          
 --select @cashflow = sum(((DATEDIFF(d,rundate, @todate) + 1)/@D)*(netamountbase-dividend)) from T_MW_Transactions where rundate between @fromdate and @todate and Asset = @entity          
          
     select @cashflow = ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 1)/@D)*(netamountbase-dividend)),0) from T_MW_Transactions where rundate between @fromdate and @toDate and Asset in (select * from @T_Entity) and open_closetag <> 'C'          
  select @cashflow = @cashflow + ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 0)/@D)*(netamountbase-dividend)),0) from T_MW_Transactions where rundate between @fromdate and @toDate and Asset in (select * from @T_Entity) and open_closetag = 'C'          
          
          
 set @BMVsecurities = ISNULL(@BMVsecurities,0)          
 set @cashflow = ISNULL(@cashflow,0)          
          
 set @result =  @BMVsecurities + @cashflow          
 End            
       
  if @groupfield = 'UDASector'          
 Begin          
 select @PNL = sum(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend) from dbo.T_MW_GenericPNL WHERE asset<>'cash' and Rundate between @fromdate and @todate and UDASector in (select * from @T_Entity)          
 select @BMVsecurities = sum(beginningmarketvaluebase) from dbo.T_MW_GenericPNL where asset<>'cash' and DateDiff(d,TradeDate, @fromdate) > 0 AND rundate = @NextBusinessDateIfNotBusinessDay AND UDASector in (select * from @T_Entity)          
 --select @cashflow = sum(((DATEDIFF(d,rundate, @todate) + 1)/@D)*(netamountbase-dividend)) from T_MW_Transactions where rundate between @fromdate and @todate and UDASector = @entity          
          
     select @cashflow = ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 1)/@D)*(netamountbase-dividend)),0) from T_MW_Transactions where rundate between @fromdate and @toDate and UDASector in (select * from @T_Entity) and open_closetag <> 'C'          
  select @cashflow = @cashflow + ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 0)/@D)*(netamountbase-dividend)),0) from T_MW_Transactions where rundate between @fromdate and @toDate and UDASector in (select * from @T_Entity) and open_closetag = 'C'        
  
          
          
 set @BMVsecurities = ISNULL(@BMVsecurities,0)          
 set @cashflow = ISNULL(@cashflow,0)          
          
 set @result =  @BMVsecurities + @cashflow          
 End            
           
           
           
  if @groupfield = 'Symbol'          
 Begin          
 select @PNL = sum(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend) from dbo.T_MW_GenericPNL WHERE Rundate between @fromdate and @todate and Symbol in (select * from @T_Entity)          
 --print @D          
 select @BMVsecurities = sum(beginningmarketvaluebase) from dbo.T_MW_GenericPNL where DateDiff(d,TradeDate, @fromdate) > 0 AND rundate = @NextBusinessDateIfNotBusinessDay AND Symbol in (select * from @T_Entity)          
--print @BMVsecurities          
     select @cashflow = ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 1)/@D)*(netamountbase-dividend)),0) from T_MW_Transactions where rundate between @fromdate and @toDate and Symbol in (select * from @T_Entity) and open_closetag <> 'C'          
--print @cashflow          
  select @cashflow = @cashflow + ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 0)/@D)*(netamountbase-dividend)),0) from T_MW_Transactions where rundate between @fromdate and @toDate and Symbol in (select * from @T_Entity) and open_closetag = 'C'          
--print @cashflow          
 set @BMVsecurities = ISNULL(@BMVsecurities,0)          
 set @cashflow = ISNULL(@cashflow,0)          
           
 set @result =  @BMVsecurities + @cashflow          
          
 End            
           
if @groupfield = 'UnderlyingSymbol'          
 Begin          
 select @PNL = sum(totalUNrealizedPNLMTM + totalrealizedPNLMTM + dividend) from dbo.T_MW_GenericPNL WHERE Rundate between @fromdate and @todate and UnderlyingSymbol in (select * from @T_Entity)          
--print @D          
 select @BMVsecurities = sum(beginningmarketvaluebase) from dbo.T_MW_GenericPNL where DateDiff(d,TradeDate, @fromdate) > 0 AND rundate = @NextBusinessDateIfNotBusinessDay AND UnderlyingSymbol in (select * from @T_Entity)          
--print @BMVsecurities          
     select @cashflow = ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 1)/@D)*(netamountbase-dividend)),0) from T_MW_Transactions where rundate between @fromdate and @toDate and UnderlyingSymbol in (select * from @T_Entity) and open_closetag <> 'C'          
--print @cashflow          
  select @cashflow = @cashflow + ISNULL(sum(((DATEDIFF(d,rundate, @toDate) + 0)/@D)*(netamountbase-dividend)),0) from T_MW_Transactions where rundate between @fromdate and @toDate and UnderlyingSymbol in (select * from @T_Entity) and open_closetag = 'C'  
 
    
       
        
--print @cashflow          
 set @BMVsecurities = ISNULL(@BMVsecurities,0)          
 set @cashflow = ISNULL(@cashflow,0)          
           
 set @result =  @BMVsecurities + @cashflow          
          
 End            
          
--error checking here if return calculation does not make sense          
--check AP LP from 4/1/2010 to 4/30/2010, -588% return?!          
Declare @MDreturn float          
select @MDreturn = isnull(@pnl,0) / (NULLIF( abs(@result),0)   )        
          
if @MDreturn <= -1          
begin          
select @MDreturn = -.999          
end          
          
return @MDreturn          
          
END 