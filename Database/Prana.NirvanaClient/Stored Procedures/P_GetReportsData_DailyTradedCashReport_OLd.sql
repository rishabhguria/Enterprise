--select * from V_Taxlots           
--update T_Group           
--set T_Group.FxConversionMethodOperator='D'          
--select * from T_Group 

--P_GetReportsData_DailyTradedCashReport '04-07-2009'          
CREATE PROCEDURE [dbo].[P_GetReportsData_DailyTradedCashReport_OLd]                    
(                    
 @tradeDate datetime                    
)                    
AS                    
                    
Create TABLE #FXConversionRatesForDate                          
(                        
  FromCurrencyID int,                        
  ToCurrencyID int,                        
  RateValue float,                        
  ConversionMethod int,                        
  Date DateTime,                        
  eSignalSymbol varchar(max)                        
)                         
                    
INsert into #FXConversionRatesForDate                     
select * from GetAllFXConversionRates()                    
                
select                        
 Symbol,                 
 MAX(OrderSideTagValue) AS OrderSideTagValue,                 
 Side AS Side,                 
 MAX(SideMultiplier) AS SideMultiplier,                 
 MAX(AllocatedQty) AS AllocatedQty,                 
 MAX(AvgPrice) AS AvgPrice,                 
 MAX(Commission) AS Commission,                   
    MAX(Fees) AS Fees,                   
    MAX(OtherFees) AS OtherFees,                   
    MAX(AUECLocalDate) AS AUECLocalDate,                 
 MAX(settlementdate) AS settlementdate,                   
    MAX(Asset) AS Asset,                   
 MAX(AssetMultiplier) AS AssetMultiplier,                 
 MAX(Underlying) AS Underlying,                 
 MAX(AUEC_Currency) AS AUEC_Currency,                   
    MAX(VsCurrency) AS VsCurrency,                 
 MAX(LeadCurrency) AS TradedCurrency,                   
    MAX(FXFromCurrencyID) AS FXFromCurrencyID,                 
 MAX(AssetID) AS AssetID,                 
 MAX(UnderLyingID) AS UnderLyingID,                   
    MAX(ExchangeID) AS ExchangeID,                 
 MAX(LeadCurrencyID) AS TradedCurrencyID,                 
 MAX(VsCurrencyID) AS VsCurrencyID,                 
 MAX(AUECID) AS AUECID,                 
 MAX(AUEC_CurrencyID) AS AUEC_CurrencyID,                 
 MAX(FundID) AS FundID,                 
 --SUM(IsSwapped) AS IsSwapped,                 
 MAX(FundName) AS FundName,                   
 MAX(CurrencyID) AS CurrencyID,                   
    MAX(SettlementCurrency) AS SettlementCurrency,                   
    MAX(SwapMultiplier) AS SwapMultiplier,                 
 ISNULL(SUM(GrossAmt), 0) AS GrossAmt,                
 ISNULL(SUM(NetAmt), 0) AS NetAmt,                
 ISNULL(SUM(GrossPayment), 0) As GrossPayment,                      
 ISNULL(SUM(NetPayment), 0)As NetPayment,                      
 ISNULL(SUM(CashValue), 0) As CashValue,                      
 MAX(FXConversionRate) AS FXConversionRate                
from                       
(                
                
 (                    
 select                     
 Symbol,                 
 OrderSideTagValue,                 
 Side,                 
 SideMultiplier,                 
 AllocatedQty,                 
 AvgPrice,                 
 Commission,                   
 Fees,                   
 OtherFees,                   
 AUECLocalDate,                 
 settlementdate,                   
 Asset,                   
 AssetMultiplier,                 
 Underlying,                 
 AUEC_Currency,                   
 VsCurrency,                 
 LeadCurrency,                   
 FXFromCurrencyID,                 
 AssetID,                 
 UnderLyingID,                   
 ExchangeID,                 
 LeadCurrencyID,                 
 VsCurrencyID,                 
 AUECID,                 
 AUEC_CurrencyID,                 
 VG.FundID,                 
 IsSwapped,                 
 FundName,                   
 CurrencyID,                   
 SettlementCurrency,                   
 SwapMultiplier,                
                
                
 VG.AvgPrice* VG.AllocatedQty * VG.SideMultiplier * VG.AssetMultiplier * VG.SwapMultiplier   AS GrossAmt,                      
 (VG.AvgPrice* VG.AllocatedQty * VG.SideMultiplier * VG.AssetMultiplier * VG.SwapMultiplier) + (VG.Commission + VG.Fees + VG.OtherFees) AS NetAmt,                      
 VG.AvgPrice* VG.AllocatedQty *VG.SideMultiplier* VG.AssetMultiplier * (-1)* VG.SwapMultiplier AS GrossPayment,                      
 ((VG.AvgPrice* VG.AllocatedQty*VG.SideMultiplier*VG.AssetMultiplier * VG.SwapMultiplier ) + VG.Commission + VG.Fees + VG.OtherFees)*(-1) AS NetPayment,                      
                      
 Isnull(FundCash.cashValueLocal,0) as CashValue,                    
 Case When FXFromCurrencyID = 1                 
  Then 1                    
  Else                     
  CASE ISNULL(TradeFXRate, 0)                    
        WHEN 0 THEN                    
          Case When #FXConversionRatesForDate.ConversionMethod = 0                    
     Then IsNull(#FXConversionRatesForDate.RateValue,0)                
     Else IsNull(1/#FXConversionRatesForDate.RateValue,0)                    
    End                
        ELSE                
  Case When TradeFXConversionMethodOperator = 'M'                    
     Then IsNull(TradeFXRate,0)                    
Else IsNull(1/TradeFXRate,0)                    
                             
        END                    
   END                    
 End                    
  As FXConversionRate                    
                     
 from V_TradedCashReport as VG                     
 left join       #FXConversionRatesForDate                     
  on DATEDIFF(d, #FXConversionRatesForDate.Date, AUECLocalDate) = 0        
    and #FXConversionRatesForDate.FromCurrencyID = FXFromCurrencyID                     
    and #FXConversionRatesForDate.ToCurrencyID = 1                    
 left join T_DayEndBalances  as FundCash                     
    on  DATEDIFF(d, SettlementDate, FundCash.Date) = 0                     
    and VG.FundID = FundCash.FundID and FundCash.LocalCurrencyID = VG.AUEC_CurrencyID  AND FundCash.BalanceType=1                  
                     
 --order by AUECLocalDate                    
 where                     
 DATEDIFF(d, VG.AUECLocalDate, ISNULL(@tradeDate, VG.AUECLocalDate)) = 0                    
 )                
                
 UNION                
 (                
 select                     
  Symbol,                 
  OrderSideTagValue,                 
  Side,                 
  SideMultiplier*-1,                 
  AllocatedQty,                 
  AvgPrice,                 
  Commission,                   
  Fees,                   
  OtherFees,                   
  AUECLocalDate,                 
  settlementdate,                   
  Asset,                   
  AssetMultiplier,                 
  Underlying,                 
  AUEC_Currency,                   
  VsCurrency,                 
  LeadCurrency,                   
  FXFromCurrencyID,                 
  AssetID,                 
  UnderLyingID,                   
  ExchangeID,                 
  LeadCurrencyID,                 
  VsCurrencyID,                 
  AUECID,                 
  AUEC_CurrencyID,                 
  VG.FundID,                 
  IsSwapped,                 
  FundName,                   
  CurrencyID,                   
  LeadCurrency AS SettlementCurrency, --AS in FX trades the reverse amount i.e. traded amount is taken care of in respect of traded currency taken as settlement currency.                   
  SwapMultiplier,                
                
 VG.AllocatedQty * VG.SideMultiplier * VG.AssetMultiplier AS GrossAmt,                      
 (VG.AllocatedQty * VG.SideMultiplier * VG.AssetMultiplier) + (VG.Commission + VG.Fees + VG.OtherFees) AS NetAmt,                      
 VG.AllocatedQty * VG.SideMultiplier* VG.AssetMultiplier AS GrossPayment,                      
 ((VG.AllocatedQty * VG.SideMultiplier * VG.AssetMultiplier) + VG.Commission + VG.Fees + VG.OtherFees) AS NetPayment,                      
                      
 Isnull(FundCash.cashValueLocal,0) as CashValue,                    
 Case When VsCurrencyID = 1                     
  Then 1                    
  Else                     
  CASE ISNULL(TradeFXRate, 0)                    
        WHEN 0 THEN                    
          Case When #FXConversionRatesForDate.ConversionMethod = 0                    
     Then IsNull(#FXConversionRatesForDate.RateValue,0)                    
     Else IsNull(1/#FXConversionRatesForDate.RateValue,0)      
    End                
        ELSE                
  Case When TradeFXConversionMethodOperator = 'M'                    
     Then IsNull(TradeFXRate,0)                    
     Else IsNull(1/TradeFXRate,0)                    
                             
        END                    
   END                      
 End                    
  As FXConversionRate                    
               
 from V_TradedCashReport as VG                     
 left join       #FXConversionRatesForDate                     
    on DATEDIFF(d, #FXConversionRatesForDate.Date, AUECLocalDate) = 0        
    and #FXConversionRatesForDate.FromCurrencyID = LeadCurrencyID                     
    and #FXConversionRatesForDate.ToCurrencyID = 1                    
 left join T_DayEndBalances as FundCash                     
    on DATEDIFF(d, SettlementDate, FundCash.Date) = 0        
    and VG.FundID = FundCash.FundID and FundCash.LocalCurrencyID = VG.AUEC_CurrencyID  AND FundCash.BalanceType=1                   
                     
 --order by AUECLocalDate                    
 where                     
 DATEDIFF(d, VG.AUECLocalDate, ISNULL(@tradeDate, VG.AUECLocalDate)) = 0                    
 AND VG.AssetID = 5                
 )                
) AS AllData                
                
Group by  AllData.FundID, AllData.Symbol, AllData.SettlementCurrency, AllData.SettlementDate, AllData.Side                
                    
drop table #FXConversionRatesForDate 

/*            
Author: Sandeep Singh            
Date: May 19, 2012            
            
P_NewMTMBasedReport '4/1/2012','5/28/2012','1182,1183,1184,1185,1186','SedolSymbol',''           
*/            
