  
        
CREATE Procedure [dbo].[P_MW_GetActivitySummary_BothExAndPayDate]         
(        
@StartDate datetime,        
@EndDate datetime,  
@IsPayDate bit        
)        
As        
Begin        
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
CASE  
When ((open_closeTag = 'd' OR open_closeTag = 'dp') and (Descriptions = '' or Descriptions is Null) and DividendLocal>=0 and Asset <> 'FixedIncome')                                    
Then 'Dividend Received'  
When ((open_closeTag = 'd' OR open_closeTag = 'dp') and (Descriptions = '' or Descriptions is Null) and DividendLocal<0 and Asset <> 'FixedIncome')                                    
Then 'Dividend Charged'  
When ((open_closeTag = 'd' OR open_closeTag = 'dp') and (Asset = 'FixedIncome'))                                    
Then 'Bond Interest'  
When ((open_closeTag = 'd' OR open_closeTag = 'dp') and Descriptions <> '')  
Then Descriptions  
Else Side   
End As Side,  
 --Side ,                 
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
 CommissionLocal ,                
 CommissionBase ,                
 FeesLocal ,                
 FeesBase ,                
 OtherFeesLocal ,                
 OtherFeesBase ,                
 FXRate_TradeDate as OpeningFXRate ,                
 MarkFXRate_TradeDate as TradeDateFXRate ,                 
 MarkFXRate_SettleDate as SettlementDateFXRate,       
    
CASE  Open_CloseTag  
WHEN 'D'  THEN Dividend  
WHEN 'DP' THEN Dividend  
WHEN 'Cash' THEN NetAmountBase  -- positive for Sell and negative for buy cash entries.  
ELSE (-1*NetAmountBase)    
END  as     
NetAmountBase,    
            
CASE Open_CloseTag    
WHEN 'D' THEN DividendLocal    
WHEN 'DP'THEN DividendLocal    
WHEN 'Cash' THEN NetAmountLocal    
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
 ISINSymbol,    
 TransactionType           
           
from       
T_MW_Transactions       
       
where     
datediff(day , Rundate , @EndDate)>=0       
and datediff (day , @Startdate , Rundate)>=0    
and  
(  
 open_closeTag = 'o' or open_closeTag = 'c'  
 OR  
 open_closeTag =   
 CASE @IsPayDate  
 WHEN 0 THEN  'd'  
 WHEN 1 THEN 'dp'  
 END  
)  
  
End  