
/****** Object:  StoredProcedure [dbo].[P_GetCashTransactionsExceptions]          
Script Date: 04/19/2013 19:23:48 ******/      
--TODO: Need to update this sp, this sp is old      
CREATE PROC [dbo].[P_GetJournalAllExceptions]          
(                
 @startDate datetime,          
 @endDate datetime,  
 @fundIDs varchar(max)          
)          
As      
create table #funds  
(  
fundID int  
)  
insert INTO #funds  
SELECT Items from dbo.Split(@fundIDs,',')  
select  
act.UniqueKey,  
ActivityID,  
ActivityTypeId_FK,  
FKID,  
BalanceType,  
Symbol,  
act.FundID,   
TradeDate,   
SettlementDate,   
CurrencyID,  
LeadCurrencyID,  
VsCurrencyID,  
ClosedQty,   
Amount,   
Commission,   
SoftCommission,  
 OtherBrokerFees,  
 ClearingBrokerFee,   
StampDuty,   
TransactionLevy,  
ClearingFee,   
TaxOnCommissions,   
MiscFees,   
SecFee,   
OccFee,   
OrfFee,   
FXRate,   
FXConversionMethodOperator,   
ActivitySource,  
TransactionSource,   
ActivityNumber,   
Description,   
Subactivity,   
SideMultiplier,  
SettlCurrency,   
act.OptionPremiumAdjustment,
act.ModifyDate,
act.EntryDate,
act.UserId
from T_AllActivity act  
inner join T_CashPreferences tcpref  
on act.FundID=tcpref.FundID  
where   
DATEDIFF(d,@startDate,TradeDate)>=0  
AND   
datediff(d,@endDate,TradeDate)<=0  
AND   
ActivityId NOT IN   
(SELECT ActivityId_FK FROM T_Journal where datediff(d,@startDate,TransactionDate)>=0   
And datediff(d,@endDate,TransactionDate)<=0)   
and   
act.FundID in (SELECT #funds.fundID from #funds)  
and   
DATEDIFF(d,act.TradeDate,tcpref.CashMgmtStartDate)<=0  

