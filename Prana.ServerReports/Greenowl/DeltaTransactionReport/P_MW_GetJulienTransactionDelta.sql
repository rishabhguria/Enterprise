  
  
/****************************************************                
Author : Ankit Misra                                      
Creation Date : 2015/02/17                 
Purpose: Fetch Data for Julien Transaction Delta Report for a day.                                  
Execution Method :                                 
exec P_MW_GetJulienTransactionDelta @Date='2015-02-13',@Funds=N'1245,1213,1214,1238,1239,1240,1241,1242,1244,1243,1246',@Method=N'TradeDate'                
*****************************************************/                
CREATE Procedure [dbo].[P_MW_GetJulienTransactionDelta]                        
(                        
@Date datetime,                
@Funds Varchar(max),                        
@Method Varchar(100)                        
)                        
As                  
--Declare @Method Varchar(100)                
--Set  @Method =  'TradeDate'                        
--                               
--Declare @Date datetime                 
--Set @Date = '02-13-2015'                
--                
--  Declare @Funds Varchar(max)                
--  Set @Funds = '1245,1213,1214,1238,1239,1240,1241,1242,1244,1243,1246'                           
                
Select * Into #Funds                                                    
from dbo.Split(@Funds, ',')                             
                          
Select                  
 TradeDate,                           
 Symbol ,                                                        
 UnderlyingSymbol,                 
 MasterFund,                                                                                                
 Fund,                     
 Asset,                                                   
 ProcessDate,                
 UDASector,                
 UDASubSector,                
 PutOrCall,            
 Quantity,        
 SideMultiplier,        
 Multiplier,      
 Open_closeTag,  
IsNull(MarkFXRate_TradeDate,0) As MarkFXRate_TradeDate                 
        
Into #TempTradedSymbolData                            
From                             
T_MW_Transactions Trans                 
Inner Join T_CompanyFunds F on F.FundName = Trans.Fund                                        
Where @Date =                       
Case                       
 When (@Method = 'Tradedate')                      
 Then Tradedate                       
 Else ProcessDate                      
End                 
And F.CompanyFundID in (Select * from #Funds)                     
And (Open_closeTag = 'O' Or Open_closeTag = 'C')          
        
Select Distinct         
Symbol,        
Delta,        
UnderlyingSymbolPrice,        
RunDate,      
Open_CloseTag       
InTo #TempGenericPnl        
From T_MW_GenericPNL        
Where DateDiff(Day,RunDate,@Date) = 0 And (Open_closeTag = 'O' Or Open_closeTag = 'C')       
          
        
Select Distinct         
Symbol,        
Delta,        
UnderlyingSymbolPrice,        
RunDate        
InTo #TempGenericPnl_OpenTrades        
From #TempGenericPnl        
Where Open_CloseTag = 'O'        
      
Select Distinct         
Symbol,        
Delta,        
UnderlyingSymbolPrice,        
RunDate        
InTo #TempGenericPnl_ClosingTrades        
From #TempGenericPnl        
Where Open_CloseTag = 'C'      
                    
                
 Select                
  TempTrade.TradeDate AS TradeDate,                
  TempTrade.Symbol AS Symbol,                          
  TempTrade.UnderlyingSymbol AS UnderlyingSymbol,                
  TempTrade.MasterFund AS MasterFund,                                                                                                
  TempTrade.Fund AS Fund,                     
  TempTrade.Asset AS Asset,                                                   
  TempTrade.ProcessDate AS ProcessDate,                
  TempTrade.UDASector AS UDASector,                
  TempTrade.UDASubSector AS UDASubSector,                
  TempTrade.Quantity AS Quantity,          
  TempTrade.PutOrCall AS PutOrCall ,      
  TempTrade.Open_CloseTag,      
  IsNull(PMDB.Beta,0) As Beta,       
  IsNull(TempPNL.Delta,0) As Delta,      
  SideMultiplier,      
  Multiplier,      
  IsNull(TempPNL.UnderlyingSymbolPrice,0) As UnderlyingSymbolPrice,      
  MarkFXRate_TradeDate,     
---- We are taking Delta as 1 when Open_CloseTag='C' to calculate NetExposure as per discussion with Lynn and PRANA-6003              
---- Handling only opening Transactions      
--Case      
--When TempTrade.Open_CloseTag = 'O'      
-- Then ISNULL((TempTrade.SideMultiplier * TempTrade.Quantity * IsNull(TempPNL.UnderlyingSymbolPrice,0) *         
-- TempTrade.Multiplier * IsNull(TempPNL.Delta,0) * MarkFXRate_TradeDate),0)        
--Else 0      
--End AS NetExposure ,      
--      
--Case      
--When TempTrade.Open_CloseTag = 'O'      
-- Then ISNULL((TempTrade.SideMultiplier * TempTrade.Quantity * IsNull(TempPNL.UnderlyingSymbolPrice,0) *         
-- TempTrade.Multiplier * IsNull(TempPNL.Delta,0) * IsNull(PMDB.Beta,0) * MarkFXRate_TradeDate),0)      
--Else 0      
--End  AS BetaExposure   
  
ISNULL((TempTrade.SideMultiplier * TempTrade.Quantity * IsNull(TempPNL.UnderlyingSymbolPrice,0) *    
TempTrade.Multiplier * IsNull(TempPNL.Delta,0) * MarkFXRate_TradeDate),0) AS NetExposure ,   
  
ISNULL((TempTrade.SideMultiplier * TempTrade.Quantity * IsNull(TempPNL.UnderlyingSymbolPrice,0) *         
TempTrade.Multiplier * IsNull(TempPNL.Delta,0) * IsNull(PMDB.Beta,0) * MarkFXRate_TradeDate),0) AS BetaExposure   
         
InTo #TempDeltaTransactionTable       
            
From #TempTradedSymbolData TempTrade        
Left Outer Join #TempGenericPnl_OpenTrades TempPNL On TempPNL.Symbol = TempTrade.Symbol                
Left Outer Join PM_DailyBeta PMDB ON (PMDB.Symbol = TempTrade.UnderlyingSymbol AND DateDiff(Day,TempTrade.TradeDate,PMDB.Date)= 0)      
      
---- Update BetaExposure for closing transactions      
--Update #TempDeltaTransactionTable      
--Set       
--NetExposure =       
--Case      
-- When #TempDeltaTransactionTable.Open_CloseTag = 'C' ---- Delta is one for closing transactions      
-- Then ISNULL((SideMultiplier * Quantity * IsNull(#TempGenericPnl_ClosingTrades.UnderlyingSymbolPrice,0) * Multiplier * MarkFXRate_TradeDate),0)       
--Else NetExposure      
--End,       
--BetaExposure =       
--Case      
-- When #TempDeltaTransactionTable.Open_CloseTag = 'C'      
-- Then ISNULL((SideMultiplier * Quantity * IsNull(#TempGenericPnl_ClosingTrades.UnderlyingSymbolPrice,0) * Multiplier * 1 * Beta * MarkFXRate_TradeDate),0)      
--Else BetaExposure      
--End,      
--#TempDeltaTransactionTable.UnderlyingSymbolPrice =       
--Case      
-- When #TempDeltaTransactionTable.Open_CloseTag = 'C'      
-- Then IsNull(#TempGenericPnl_ClosingTrades.UnderlyingSymbolPrice,0)      
--Else #TempDeltaTransactionTable.UnderlyingSymbolPrice      
--End,      
--#TempDeltaTransactionTable.Delta =       
--Case      
-- When #TempDeltaTransactionTable.Open_CloseTag = 'C'      
-- Then IsNull(#TempGenericPnl_ClosingTrades.Delta,0)      
--Else #TempDeltaTransactionTable.Delta      
--End      
--From #TempDeltaTransactionTable      
--Left Outer Join #TempGenericPnl_ClosingTrades On #TempGenericPnl_ClosingTrades.Symbol = #TempDeltaTransactionTable.Symbol              
      
Select * from #TempDeltaTransactionTable        
Order by Symbol      
      
Drop Table #TempTradedSymbolData,#Funds,#TempGenericPnl,#TempGenericPnl_OpenTrades      
Drop Table #TempGenericPnl_ClosingTrades,#TempDeltaTransactionTable  
  