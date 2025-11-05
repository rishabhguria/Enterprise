                          
CREATE Procedure [dbo].[P_MW_GetActivitySummary_BothExAndPayDate]                           
(                          
@StartDate datetime,                          
@EndDate datetime,                    
@IsPayDate bit,                  
@SearchString Varchar(5000) ,                          
@SearchBy Varchar(100)                          
)                          
As                        
Select * InTo #TempSymbol             
From dbo.split(@SearchString , ',')           
            
Select Distinct * InTo #Symbol             
From #TempSymbol                  
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
 ISNULL(TransactionType,Side) AS TransactionType,              
Case              
When TransactionType = 'CASH' -----Or TransactionType = 'Cash Dividend'              
Then 2----Non Trading Transaction Type              
Else 1----Trading Transaction Type              
End  TradingNonTradingType                   
 into #ActivitySummaryReport                            
from                         
T_MW_Transactions                         
                         
where                       
datediff(day , Rundate , @EndDate)>=0                         
and datediff (day , @Startdate , Rundate)>=0                      
and                    
(                    
 Open_CloseTag = 'o' or Open_CloseTag = 'c' Or Open_CloseTag = 'CASH'                   
 OR                    
 Open_CloseTag =                     
 CASE @IsPayDate                    
 WHEN 0 THEN  'd'                    
 WHEN 1 THEN 'dp'                    
 END                    
)       
      
Alter table #ActivitySummaryReport      
Add UnderlyingSymbolCompanyName nvarchar(200)      
      
Update  #ActivitySummaryReport      
Set UnderlyingSymbolCompanyName = SM.CompanyName      
From  #ActivitySummaryReport ASR      
Inner Join V_SecMasterData_WithUnderlying  SM on SM.TickerSymbol = ASR.UnderlyingSymbol         
      
      
                   
If(@SearchString <> '')                                   
 Begin                                 
  if (@searchby='Symbol')                        
  begin                        
  SELECT * FROM #ActivitySummaryReport                        
  Inner Join #Symbol on #Symbol.items = #ActivitySummaryReport.Symbol                        
  Order by symbol                        
  end                        
  else if (@searchby='underlyingSymbol')                        
  begin                        
  SELECT * FROM #ActivitySummaryReport                        
  Inner Join #Symbol on #Symbol.items = #ActivitySummaryReport.underlyingSymbol                        
  Order by symbol                        
  end                          
  else if (@searchby='BloombergSymbol')                        
  begin                        
  SELECT * FROM #ActivitySummaryReport                        
  Inner Join #Symbol on #Symbol.items = #ActivitySummaryReport.BloombergSymbol                        
  Order by symbol                        
  end                            
  else if (@searchby='SedolSymbol')                        
  begin                        
  SELECT * FROM #ActivitySummaryReport                        
  Inner Join #Symbol on #Symbol.items = #ActivitySummaryReport.SedolSymbol                        
  Order by symbol                        
  end                            
  else if (@searchby='OSISymbol')                        
  begin                        
  SELECT * FROM #ActivitySummaryReport                        
  Inner Join #Symbol on #Symbol.items = #ActivitySummaryReport.OSISymbol                        
  Order by symbol                        
  end                            
  else if (@searchby='IDCOSymbol')                        
  begin                        
  SELECT * FROM #ActivitySummaryReport                        
  Inner Join #Symbol on #Symbol.items = #ActivitySummaryReport.IDCOSymbol                        
  Order by symbol                        
  end                            
  else if (@searchby='ISINSymbol')                        
  begin                        
  SELECT * FROM #ActivitySummaryReport                        
  Inner Join #Symbol on #Symbol.items = #ActivitySummaryReport.ISINSymbol                        
  Order by symbol                        
  end                           
  else if (@searchby='CUSIPSymbol')                     
  begin                        
  SELECT * FROM #ActivitySummaryReport                        
  Inner Join #Symbol on #Symbol.items = #ActivitySummaryReport.CUSIPSymbol                        
  Order by symbol                        
  end                                     
 End                                    
Else               
 Begin                                    
  Select * from #ActivitySummaryReport Order By symbol                          
 End                   
                    
End                 
Drop Table #ActivitySummaryReport,#Symbol,#TempSymbol 
  