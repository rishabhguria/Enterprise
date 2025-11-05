 
/*************************************************  
Author : Bharat Raturi
Creation date : aug 08, 2014  
Descritpion : Get the overriding activities for exception
Execution Method :   
exec [P_RunDailyAssetRevaluation] '2013-12-20' , '2013-12-24'
*************************************************/  
  
CREATE PROC [dbo].[P_GetOverridingActivitiesForJournal]          
(                
 @fromdate datetime,
@todate datetime,            
@fundIDs varchar(max)          
)          
As      
create table #funds  
(  
fundID int  
)  
insert INTO #funds  
SELECT Items from dbo.Split(@fundIDs,',')
--------------------------------
SELECT 
'' 'UniqueKey',  
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
SideMultiplier
FROM T_AllActivity act
INNER JOIN T_CashPreferences tcpref ON act.FundID=tcpref.FundID
INNER JOIN #funds on act.FundID=#funds.fundID
where DATEDIFF(d,act.TradeDate,tcpref.CashMgmtStartDate)<=0
and 
((DATEDIFF(d,act.TradeDate,@fromdate)<=0 and DATEDIFF(d,act.TradeDate,@todate)>=0)
OR
((DATEDIFF(d,act.SettlementDate,@fromdate)<=0 and DATEDIFF(d,act.SettlementDate,@todate)>=0)))
order BY act.TradeDate, act.SettlementDate
