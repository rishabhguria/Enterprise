                                     
/****************************                                          
Author : Ankit                                          
Creation date : Mar 7 , 2013                                
Description : Get Cash Activity Summary Report Data from Middleware Table.                                
Execution Method :                                          
P_MW_GetCashActivitySummary_Test '2015-03-23 20:00:00.000','2015-03-23 20:00:00.000'                                          
                                          
****************************/                                        
                                          
ALTER Procedure [dbo].[P_MW_GetCashActivitySummary_Test]                                           
(                                          
@StartDate datetime,                                          
@EndDate datetime                                          
)                                          
As                                          
Begin     
    
--Declare @StartDate datetime                                          
--Declare @EndDate datetime     
--Set @StartDate = '2015-03-25'     
--Set @EndDate = '2015-03-25'    
                                         
                
select                                            
 Symbol ,                                                                      
 UnderlyingSymbol,                                                                      
 Strategy,                                                                      
 Fund,                                                                      
 Asset,                                                                      
 Underlyer,                                                                      
 Exchange,                                                                        
 UDASector ,                                                                      
 UDACountry ,                                                                      
 UDASecurityType ,                                                                      
 UDAAssetClass ,                                                                      
 UDASubSector ,                                                                      
 TradeCurrency ,                           
                        
 --Side,                       
CASE                                           
When (open_closeTag = 'DP' and (Descriptions = '' or Descriptions is Null) and DividendLocal>=0 and Asset <> 'FixedIncome')                                                        
Then 'Dividend Received'                      
When (open_closeTag = 'DP' and (Descriptions = '' or Descriptions is Null) and DividendLocal<0 and Asset <> 'FixedIncome')                                                        
Then 'Dividend Charged'                      
When (open_closeTag = 'DP' and (Asset = 'FixedIncome'))                                                        
Then 'Bond Interest'                      
When (open_closeTag = 'DP' and Descriptions <> '')                      
Then Descriptions                           
WHEN Side = 'Cash Activity'                       
THEN SA.Name                          
ELSE Side                                 
END  as                                       
Side,                                                                   
                         
 CounterParty ,                                                   
 PrimeBroker ,                                                  
 Trader ,                                                              
 SecurityName ,                                                   
 TradeDate ,                                                      
 SettleDate as SettlementDate ,                                                  
 Quantity ,                                              
 Multiplier,                                               
 SideMultiplier,                                
 AvgPrice ,                                                
 PutOrCall,                                                       
 FXRate_TradeDate as OpeningFXRate ,                                                  
 MarkFXRate_TradeDate as TradeDateFXRate ,                                                   
 MarkFXRate_SettleDate as SettlementDateFXRate,                                         
                                      
CASE                                       
WHEN Open_CloseTag ='DP' THEN Dividend                                    
WHEN (Asset = 'FX' and TradeOrigin ='NotManual') THEN NetAmountBase                  
ELSE (-1*NetAmountBase)                                      
END  as                  
NetAmountBase,                                      
                                              
CASE                                       
WHEN Open_CloseTag ='DP' THEN DividendLocal                                      
WHEN (Asset = 'FX' and TradeOrigin ='NotManual') THEN NetAmountLocal                                      
ELSE (-1*NetAmountLocal)                                      
END  as                                       
NetAmountLocal,                                      
                                         
 PrincipalAmountBase ,                                                  
 PrincipalAmountLocal ,                                                  
 TradeOrigin ,                                   
 Open_CloseTag ,                                                  
 DividendLocal,                                              
 Dividend as DividendBase,                                              
 BloomBergSymbol,                                              
 SedolSymbol,                                              
 OSISymbol,                                              
 IDCOSymbol,                                              
 ISINSymbol  
                         
                                             
from                                         
T_MW_Transactions   TR                          
Left OUTER JOIN T_SubAccounts  SA                          
on TR.SubAccountID = SA.SubAccountID                                         
                                         
where                   
datediff (day , @StartDate, rundate ) >= 0 and                                          
datediff(day , rundate , @EndDate ) >= 0   
And                                
 ( (Open_closeTag = 'o' or open_closeTag = 'C' or open_closeTag = 'DP' OR TR.Open_CloseTag = 'Cash')                               
  and Asset Not In ('FX','FXForward')
 )                
  
Union All  
  
select                                            
 TR.Symbol ,                                                                      
 UnderlyingSymbol,                                                                      
 Strategy,                                                                      
 Fund,                                                                      
 Asset,                                                                      
 Underlyer,                                                                      
 Exchange,                                                                        
 UDASector ,                                                                      
 UDACountry ,                                                                      
 UDASecurityType ,                                                                      
 UDAAssetClass ,                                                                      
 UDASubSector ,   
TradeCurrency ,                          
   
Case  
When SideMultiplier  = 1  
Then 'Buy'  
Else 'Sell'  
End As Side,  
                       
 CounterParty ,                                                   
 PrimeBroker ,                                                  
 Trader ,                                                              
 SecurityName ,                              
 TradeDate ,                                                      
 SettleDate as SettlementDate ,                                                  
 Quantity ,                                              
 Multiplier,                                               
 SideMultiplier,                                                  
 AvgPrice ,                                                
 PutOrCall,  
                                                      
 FXRate_TradeDate as OpeningFXRate ,     
Case  
 When Side   = 'Buy' And SideMultiplier = 1  
 Then 1/AvgPrice    
 When Side   = 'Buy' And SideMultiplier = -1  
 Then 1   
 When Side   = 'Sell' And SideMultiplier = 1  
 Then AvgPrice    
 When Side   = 'Sell' And SideMultiplier = -1  
 Then 1    
 Else 1   
End As TradeDateFXRate,  
                                               
-- MarkFXRate_TradeDate as TradeDateFXRate ,                                                   
 MarkFXRate_SettleDate as SettlementDateFXRate,                                         
   
  
Case  
When Side   = 'Buy'   
Then (Quantity * SideMultiplier) + (CommissionBase + FeesBase + OtherFeesBase)   
Else (Quantity * AvgPrice * SideMultiplier) + (CommissionBase + FeesBase + OtherFeesBase)  
End As NetAmountBase,                                     
                                              
Case  
 When Side   = 'Buy' And SideMultiplier = 1  
 Then Quantity * AvgPrice * SideMultiplier + (CommissionLocal + FeesLocal + OtherFeesLocal)   
 When Side   = 'Buy' And SideMultiplier = -1  
 Then Quantity * SideMultiplier + (CommissionLocal + FeesLocal + OtherFeesLocal)  
 When Side   = 'Sell' And SideMultiplier = -1  
 Then Quantity * AvgPrice * SideMultiplier + (CommissionLocal + FeesLocal + OtherFeesLocal)   
 When Side   = 'Sell' And SideMultiplier = 1  
 Then Quantity  * SideMultiplier + (CommissionLocal + FeesLocal + OtherFeesLocal)  
 Else Quantity * SideMultiplier + (CommissionLocal + FeesLocal + OtherFeesLocal)  
End As NetAmountLocal,                                       
                                         
 PrincipalAmountBase ,                                                  
 PrincipalAmountLocal ,                                                  
 TradeOrigin ,                                   
 Open_CloseTag ,                                                  
 DividendLocal,                                              
 Dividend as DividendBase,                                              
 BloomBergSymbol,                                              
 SedolSymbol,                                              
 OSISymbol,                                              
 IDCOSymbol,                                              
 ISINSymbol                      
                                             
from                                         
T_MW_Transactions   TR   
LEFT OUTER JOIN T_SubAccounts SA On TR.SubAccountID = SA.SubAccountID    
                                    
Where                   
datediff (day , @StartDate, rundate ) >= 0 and datediff(day , rundate , @EndDate ) >= 0   
----And TR.Symbol = 'USD-CAD SPOT'                               
and Asset In ('FX','FXForward') And TradeOrigin = 'NotManual'       
                  
End