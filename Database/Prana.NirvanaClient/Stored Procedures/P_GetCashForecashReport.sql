    
                  
                    
-- =============================================                    
-- Author:  Ashish Poddar and Bhupesh Bareja                    
-- Create date: 16 October, 2008                    
-- Description: To find the cash impact of transactions based on settlement date and currency. Note that FX                     
    --trades have double cash impace that is why we have double counted the FX trades.                     
/*    
exec P_GetCashForecashReport '29-MAY-2009', '29-MAY-2009'                
*/    
-- =============================================                    
CREATE PROCEDURE [dbo].[P_GetCashForecashReport]                    
 -- Add the parameters for the stored procedure here                    
 @StartDate datetime = Null,                    
 @EndDate datetime = Null                     
AS                    
BEGIN                    
                    
CREATE TABLE [dbo].#TempSettlementFundCurrencyDateTable                
(                          
 SettlementDate datetime,                
 FundID int,                
 CurrencySymbol varchar(10)                           
)                          
                          
CREATE TABLE [dbo].#TempSettlementDateTable                
(                          
 SettlementDate datetime                
)                
                 
INSERT INTO [dbo].#TempSettlementDateTable                          
      (                          
       SettlementDate                            
      )                
 select * from dbo.GetNextAskedDaysWithoutExchangeANDBusinessHolidays(@StartDate, 4)                 
                
CREATE TABLE [dbo].#TempFundTable                
(                          
 FundID int                
)                
                 
INSERT INTO [dbo].#TempFundTable                          
      (                          
       FundID                            
      )                
 select /*distinct*/ VT.FundID FROM V_Temp_TaxlotAmount VT LEFT OUTER JOIN T_Currency C ON                  
  VT.SettlementCurrency = C.CurrencySymbol                  
 where          
datediff(d,  VT.settlementdate,  @StartDate)<=0 And datediff(d,  VT.settlementdate,  @EndDate) >= 0       
                
CREATE TABLE [dbo].#TemCurrencySymbolTable                
(                          
 CurrencySymbol varchar(10)                
)                
                 
INSERT INTO [dbo].#TemCurrencySymbolTable                          
      (                          
       CurrencySymbol                            
      )                
 select distinct VT.SettlementCurrency FROM V_Temp_TaxlotAmount VT LEFT OUTER JOIN T_Currency C ON                  
  VT.SettlementCurrency = C.CurrencySymbol Where VT.CurrencyID <> 0 and     
 datediff(d,  VT.settlementdate,  @StartDate)<=0 And datediff(d,  VT.settlementdate,  @EndDate) >= 0       
union     
 select distinct VT.Vscurrency FROM V_Temp_TaxlotAmount VT LEFT OUTER JOIN T_Currency C ON                  
  VT.SettlementCurrency = C.CurrencySymbol Where VT.VsCurrencyID <> 0 and     
 datediff(d,  VT.settlementdate,  @StartDate)<=0 And datediff(d,  VT.settlementdate,  @EndDate) >= 0      
union    
select distinct VT.LeadCurrency FROM V_Temp_TaxlotAmount VT LEFT OUTER JOIN T_Currency C ON                  
  VT.SettlementCurrency = C.CurrencySymbol Where VT.LeadCurrencyID <> 0 and     
 datediff(d,  VT.settlementdate,  @StartDate)<=0 And datediff(d,  VT.settlementdate,  @EndDate) >= 0      
                
INSERT INTO [dbo].#TempSettlementFundCurrencyDateTable                
                
select * from #TempSettlementDateTable CROSS JOIN #TempFundTable CROSS JOIN #TemCurrencySymbolTable                
                
CREATE TABLE [dbo].#TempSettlementFundCurrencyDateTableFinal        
(                          
 SettlementDate datetime,                
 FundID int,                
 CurrencySymbol varchar(10)                           
)        
INSERT INTO [dbo].#TempSettlementFundCurrencyDateTableFinal        
select * from #TempSettlementFundCurrencyDateTable group by SettlementDate, CurrencySymbol, FundID order by CurrencySymbol                      
          
          
       
CREATE TABLE [dbo].#TempSettlementFundCurrencyDateTableC                
(                          
 SettlementDate datetime,                
 FundID int,                
 CurrencySymbol varchar(10),          
 Cash float                           
)                          
                          
CREATE TABLE [dbo].#TemCurrencySymbolTableC                
(                          
 FundID int,          
 CurrencySymbol varchar(10),          
 Cash Float                
)                
                 
INSERT INTO [dbo].#TemCurrencySymbolTableC                          
      (                          
       FundID,          
    CurrencySymbol,          
    Cash                            
      )                
 select distinct FundID, C.CurrencySymbol, CashValueLocal FROM T_DayEndBalances CFCCV LEFT OUTER JOIN T_Currency C ON                  
  CFCCV.LocalCurrencyID = C.CurrencyID          
  WHERE (DATEDIFF(d, CFCCV.Date, @startDate) = 0)  AND   CFCCV.BalanceType=1
               
                     
                
INSERT INTO [dbo].#TempSettlementFundCurrencyDateTableC                
                
select * from #TempSettlementDateTable CROSS JOIN #TemCurrencySymbolTableC           
          
               
                      
          
select                      
TradeData.FundID AS FundID,                
TradeData.SettlementCurrency AS SettlementCurrency,                    
TradeData.SettlementDate AS SettlementDate,                     
Sum(ISNULL(TradeData.BuyPayment, 0)) As BuyPayment,                    
Sum(ISNULL(TradeData.SellPayment, 0))As SellPayment,                    
Sum(ISNULL(TradeData.SellShortPayment, 0))As SellShortPayment,                    
Sum(ISNULL(TradeData.CoverPayment, 0)) As CoverPayment,                    
ISNULL(TradeData.CashValueLocal, 0) AS CashValueLocal          
FROM          
(          
           
  select                      
  TSFCDT.FundID AS FundID,                
  TSFCDT.CurrencySymbol AS SettlementCurrency,                    
  TSFCDT.SettlementDate AS SettlementDate,                     
  Sum(ISNULL(BuyPayment, 0)) As BuyPayment,                    
  Sum(ISNULL(SellPayment, 0))As SellPayment,                    
  Sum(ISNULL(SellShortPayment, 0))As SellShortPayment,                    
  Sum(ISNULL(CoverPayment, 0)) As CoverPayment,                    
  0 AS CashValueLocal                    
  from                     
  (                    
   (                    
   select                     
    VT.FundID,                    
    SettlementCurrency ,                    
    SettlementDate,                    
    Case Side                     
     When 'Buy'                    
     Then Sum(VT.NetPayment )                    
     Else 0 end                    
    As BuyPayment,                    
                 
    Case Side                     
     When 'Sell'                    
     Then Sum(VT.NetPayment )                    
     Else 0 end                    
    As SellPayment,                    
                      
    Case Side                     
     When 'SellShort'                    
     Then Sum(VT.NetPayment )                    
     Else 0 end                    
    As SellShortPayment,                   
                      
    Case Side                     
     When 'BuyToCover'                    
     Then Sum(VT.NetPayment )                    
     Else 0 end                    
    As CoverPayment,                  
                    
    0 AS CashValueLocal                  
   from V_Temp_TaxlotAmount VT LEFT OUTER JOIN T_Currency C ON      VT.SettlementCurrency = C.CurrencySymbol                  
                       
   where /*VT.FundID = ISNULL (@FundID, VT.FundID) AND   */  VT.AssetID<>5              and      
datediff(d,  VT.settlementdate,  @StartDate)<=0 And datediff(d,  VT.settlementdate, @EndDate) >= 0       
              
   Group by  VT.FundID,SettlementCurrency, Settlementdate , Side      
   )                    
                      
                      
   Union                    
                      
   (                   
   select                     
     VT.FundID,                    
     LeadCurrency as TradedCurrency ,                    
   SettlementDate,                    
     Case Side                     
   When 'Buy'                     
   Then Sum(VT.FxPaymentReceivedForTradedCurrency)                    
   Else 0 end                    
     As BuyPayment,                    
                      
     Case Side                     
   When 'Sell'                    
   Then Sum(VT.FxPaymentReceivedForTradedCurrency)                    
   Else 0 end                    
     As SellPayment,                    
                      
     Case Side                     
   When 'SellShort'                    
   Then Sum(VT.FxPaymentReceivedForTradedCurrency)                    
   Else 0 end                    
     As SellShortPayment,                    
                      
 Case Side                     
   When 'BuyToCover'                    
   Then Sum(VT.FxPaymentReceivedForTradedCurrency )                    
   Else 0 end                    
     As CoverPayment,                      
                    
   0 AS CashValueLocal                  
   from V_Temp_TaxlotAmount VT                    
   LEFT OUTER JOIN T_Currency C ON                  
    VT.LeadCurrency = C.CurrencySymbol                  
                   
   where /*VT.FundID = ISNULL (@FundID, VT.FundID)    and     */                
   VT.AssetID = 5  AND                   
   (datediff(d,  VT.settlementdate,  @StartDate)<=0 And datediff(d,  VT.settlementdate,  @EndDate) >= 0   )                  
                       
   Group by  VT.FundID, LeadCurrency, Settlementdate , Side                  
   )      
union    
(    
      select                     
     VT.FundID,                    
     VsCurrency as TradedCurrency ,                    
     SettlementDate,                    
     Case Side                     
   When 'Buy'                     
   Then Sum(VT.NetPayment)                    
   Else 0 end                    
     As BuyPayment,                    
                      
     Case Side                     
   When 'Sell'                    
   Then Sum(VT.NetPayment)                    
   Else 0 end                    
     As SellPayment,                    
                      
     Case Side                     
   When 'SellShort'                    
   Then Sum(VT.NetPayment)                    
   Else 0 end                    
     As SellShortPayment,                    
                      
 Case Side                     
   When 'BuyToCover'                    
   Then Sum(VT.NetPayment )                    
   Else 0 end                    
     As CoverPayment,                      
                    
   0 AS CashValueLocal                  
   from V_Temp_TaxlotAmount VT                    
   LEFT OUTER JOIN T_Currency C ON                  
    VT.Vscurrency = C.CurrencySymbol                  
                   
   where /*VT.FundID = ISNULL (@FundID, VT.FundID)    and     */                
   VT.AssetID = 5  AND                   
   (datediff(d,  VT.settlementdate,  @StartDate)<=0 And datediff(d,  VT.settlementdate,  @EndDate) >= 0   )                  
                       
   Group by  VT.FundID,VsCurrency, Settlementdate , Side        
)                    
  ) As AllData                
  RIGHT OUTER JOIN [dbo].#TempSettlementFundCurrencyDateTableFinal TSFCDT ON                
   DATEDIFF(d, TSFCDT.SettlementDate, AllData.SettlementDate) = 0                
   AND TSFCDT.CurrencySymbol = AllData.SettlementCurrency                
   AND TSFCDT.FundID = AllData.FundID                
  LEFT OUTER JOIN T_Currency C ON TSFCDT.CurrencySymbol = C.CurrencySymbol                
Group by TSFCDT.CurrencySymbol, TSFCDT.SettlementDate, TSFCDT.FundID               
       
            
      
    
            
 UNION ALL          
 (          
  SELECT          
  TSFCDT.FundID AS FundID,                
  TSFCDT.CurrencySymbol AS SettlementCurrency,                    
  TSFCDT.SettlementDate AS SettlementDate,                     
  0 As BuyPayment,                    
  0 As SellPayment,                    
  0 As SellShortPayment,                    
  0 As CoverPayment,                    
  MAX(ISNULL(CFCCV.CashValueLocal, 0)) AS CashValueLocal          
          
  FROM           
  T_DayEndBalances 
 CFCCV LEFT OUTER JOIN T_Currency C ON                  
  CFCCV.LocalCurrencyID = C.CurrencyID           
  AND DATEDIFF(d, CFCCV.Date, @startDate) = 0 AND  CFCCV.BalanceType=1     
  LEFT OUTER JOIN T_CompanyFunds CF ON CFCCV.FundID = CF.CompanyFundID          
            
  RIGHT OUTER JOIN [dbo].#TempSettlementFundCurrencyDateTableC TSFCDT ON                
  TSFCDT.CurrencySymbol = C.CurrencySymbol           
     AND TSFCDT.FundID = CFCCV.FundID          
  AND DATEDIFF(d, TSFCDT.SettlementDate, CFCCV.Date) = 0      
          
          
  GROUP BY TSFCDT.FundID, TSFCDT.CurrencySymbol, TSFCDT.SettlementDate          
 )               
) AS TradeData          
          
Group by TradeData.SettlementCurrency, TradeData.SettlementDate, TradeData.FundID,TradeData.CashValueLocal--, TSFCDT.CurrencySymbol, TSFCDT.SettlementDate                
        
END                    
                    
 drop table [dbo].#TempSettlementDateTable                
 drop table #TempSettlementFundCurrencyDateTable                
 drop table #TempFundTable                
 drop table #TemCurrencySymbolTable          
 drop table #TempSettlementFundCurrencyDateTableC                
 drop table #TemCurrencySymbolTableC         
 drop table [dbo].#TempSettlementFundCurrencyDateTableFinal    
    
