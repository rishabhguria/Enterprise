/*            
Author: Sandeep Singh            
Date: 19 July, 2017            
Desc: This is a customized stored procedure developed for Agile to get open positions and             
Unrealized MTM P&L            
Note: Fund is hardcoded to optimize it else it will open positions for all funds            
and client wants to see data only for single fund.            
            
Only Date is used, other parameters are juts passed as third party module pass them in sp            
            
EXEC [P_GetOpenPositionsAndMTMPNL_Agile_NT] 1,1,'2017-07-14 05:41:37:000',1,1,1,1,1,1            
*/            
            
CREATE PROCEDURE [dbo].[P_GetOpenPositionsAndMTMPNL_Agile_CS]                   
(                      
  @ThirdPartyID INT                    
 ,@CompanyFundIDs VARCHAR(max)                    
 ,@InputDate DATETIME                    
 ,@CompanyID INT                    
 ,@AUECIDs VARCHAR(max)                    
 ,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                        
 ,@DateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                        
 ,@FileFormatID INT                    
 ,@IncludeSent Int = 0                                                                                                                                     
 )                      
AS              
            
--Declare @ThirdPartyID INT                    
--Declare @CompanyFundIDs VARCHAR(max)                    
--Declare @InputDate DATETIME                    
--Declare @CompanyID INT                    
--Declare @AUECIDs VARCHAR(max)                    
--Declare @TypeID INT                                      
--Declare @DateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                        
--Declare @FileFormatID INT                    
--Declare @IncludeSent Int                                 
--                                           
--set @ThirdPartyID= 50                                            
--set @CompanyFundIDs= '1287'                                          
--set @InputDate= '2017-07-11 10:25:12:000'                                            
--set @CompanyID= 7                             
--set @AUECIDs= '63,44,34,43,59,54,18,61,74,1,15,62,73,12,80,32,81'                  
--Set @TypeID = 0                   
--Set @DateType = 0                   
--Set @FileFormatID = 0                    
--Set @IncludeSent = 0                
            
Select             
Fund As AccountName,            
0 As TaxLotState,             
OpenTradeAttribute2 As TradeAttribute2,            
Case            
 When IsSwapped = 1      
Then 'EquitySwap'      
Else Asset       
end As Asset,            
TradeCurrency As CurrencySymbol,            
SEDOLSymbol As SEDOL,            
ISINSymbol,            
Symbol,            
SecurityName As CompanyName,            
(BeginningQuantity * SideMultiplier) As Quantity,            
EndingPriceLocal As MarkPrice,            
EndingPriceBase As MarkPriceBase,    
--Case            
-- When IsSwapped = 1     
-- Then UnrealizedTotalGainOnCostD2_Local    
-- Else EndingMarketValueLocal    
--End As  MarketValue,          
EndingMarketValueLocal As MarketValue,     
--Case            
-- When IsSwapped = 1     
-- Then UnrealizedTradingGainOnCostD2_Base    
-- Else EndingMarketValueBase    
--End As  MarketValueBase,           
    
EndingMarketValueBase As MarketValueBase,            
    
TradeDateFXRate As FXRate,         
EndingFXRate,          
TotalCost_Local As NetNotionalValue,            
TotalCost_Base As NetNotionalValueBase,            
ExpirationDate,            
CONVERT(VARCHAR(10), TradeDate, 101) As TradeDate,            
TaxlotID As EntityID,      
--Case            
-- When IsSwapped = 1            
-- Then UnrealizedTradingPNLMTM            
-- Else TotalUNRealizedPNLMTM            
--End As MTMUnrealizedPNL  
Case            
 When IsSwapped = 1            
 Then UnrealizedTotalGainOnCostD2_Local            
 Else TotalUNRealizedPNLMTM            
End As MTMUnrealizedPNL            
From T_MW_GenericPNL          Where DateDiff(Day,RunDate,@InputDate) = 0            
And Open_CloseTag = 'O'            
And Asset <> 'Cash'            
And Fund = 'UCIT'            
Order by Symbol