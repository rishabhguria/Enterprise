/*    
P_GetRebateRate '04-02-2015','1239'    
*/    
    
CREATE Procedure P_GetRebateRate    
(    
@EndDate DateTime,    
@Funds Varchar(5000)    
)    
As     
    
--Declare @EndDate DateTime    
--Set @EndDate = '04-02-2015'    
--    
--Declare @Funds Varchar(5000)    
--Set @Funds = '1239'    
    
Select * Into #Funds                                    
from dbo.Split(@Funds, ',')    
    
Select Top 10  
RR.SecurityID As CUSIPSymbol,  
RR.ChargePaymentRate As RebateRate,  
RR.SecurityDescription,  
SM.TickerSymbol As Symbol,  
CF.FundName,  
RR.AccountNumber  
From T_RebateReport RR    
Inner Join T_RebateReportAccountMapping CFM On CFM.ClientAccountNumber =  RR.AccountNumber            
Inner Join T_CompanyFunds CF On CF.CompanyFundID=CFM.PranaFundID     
Inner Join #Funds On #Funds.Items = CF.CompanyFundID   
Left Outer Join [TS_MC_CAPITAL_SMV1.7.1].dbo.T_SMSymbolLookUpTable SM On SM.CUSIPSymbol = RR.SecurityID    
Where DateDiff(Day,RR.Date,@EndDate)=0   
Order by RR.ChargePaymentRate ASC   
    
Drop Table #Funds