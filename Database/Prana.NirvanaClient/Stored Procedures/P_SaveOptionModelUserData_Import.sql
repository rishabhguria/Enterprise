    
CREATE PROCEDURE [dbo].[P_SaveOptionModelUserData_Import]            
(                                                                                                
 @Xml varchar(max),                                                                                                                                
 @ErrorMessage varchar(500) output,                                                                                                                                         
 @ErrorNumber int output                                                                                         
)                                                                                                
As                                                                                            
SET @ErrorNumber = 0                                                                                                                        
SET @ErrorMessage = 'Success'                                                                                                      
                                                                                                                
BEGIN TRY                                                                                                         
                                                                                                
 BEGIN TRAN TRAN1                 
              
-- SAVES PSSYMBOL AND MULTIPLIERS                                                                            
DECLARE @handle int                                                                             
exec sp_xml_preparedocument @handle OUTPUT,@Xml                   
           
           
CREATE TABLE #Temp                                                                                           
(                                                                                           
Symbol varchar(100),
HistoricalVolUsed bit, 
Volatility float,            
VolatilityUsed bit,            
IntRate float,            
IntRateUsed bit,            
Dividend float,            
DividendUsed bit,             
StockBorrowCost float,            
StockBorrowCostUsed bit,            
Delta float,            
DeltaUsed bit,            
LastPrice float,            
LastPriceUsed bit,    
ForwardPoints float,      
ForwardPointsUsed bit,  
TheoreticalPriceUsed bit,
SharesOutstanding float,
SharesOutstandingUsed bit        
)                                                                                    
INSERT INTO #Temp                                    
 (                                                                                          
Symbol,  
HistoricalVolUsed,    
Volatility,            
VolatilityUsed,            
IntRate,            
IntRateUsed,            
Dividend,            
DividendUsed,
StockBorrowCost,
StockBorrowCostUsed,            
Delta,            
DeltaUsed,            
LastPrice,            
LastPriceUsed,    
ForwardPoints,      
ForwardPointsUsed,
TheoreticalPriceUsed,  
SharesOutstanding,
SharesOutstandingUsed                                       
 )                  
          
SELECT                 
Symbol,   
HistoricalVolUsed,
IsNull(Volatility, 0),            
VolatilityUsed,            
IsNull(IntRate, 0),            
IntRateUsed,            
IsNull(Dividend, 0),            
DividendUsed,            
IsNull(StockBorrowCost, 0),            
StockBorrowCostUsed,            
IsNull(Delta, 0),            
DeltaUsed,            
IsNull(LastPrice, 0),            
LastPriceUsed,    
IsNull(ForwardPoints, 0),      
ForwardPointsUsed, 
TheoreticalPriceUsed, 
IsNull(SharesOutstanding, 0),      
SharesOutstandingUsed   
FROM  OPENXML(@handle, '//UserOptModelInput',2)                                                                    
WITH                                                                                            
(                   
Symbol varchar(100), 
HistoricalVolUsed bit,
Volatility float,            
VolatilityUsed bit,            
IntRate float,     
IntRateUsed bit,            
Dividend float,            
DividendUsed bit,
StockBorrowCost float,
StockBorrowCostUsed bit,            
Delta float,            
DeltaUsed bit,            
LastPrice float,            
LastPriceUsed bit,    
ForwardPoints float,      
ForwardPointsUsed bit,
TheoreticalPriceUsed bit, 
SharesOutstanding float,
SharesOutstandingUsed bit             
)      
      
Update T_UserOptionModelInput      
      
set Symbol=#Temp.Symbol,  
HistoricalVolatilityUsed  =  #Temp.HistoricalVolUsed,
UserVolatility = #Temp.Volatility, 
UserVolatilityUsed =#Temp.VolatilityUsed,   
UserInterestRate=#Temp.IntRate ,
UserInterestRateUsed= #Temp.IntRateUsed,            
UserDividend =#Temp.Dividend,  
UserDividendUsed = #Temp.DividendUsed,            
UserStockBorrowCost =#Temp.StockBorrowCost,  
UserStockBorrowCostUsed = #Temp.StockBorrowCostUsed,   
UserDelta = #Temp.Delta,   
UserDeltaUsed =  #Temp.DeltaUsed,  
UserLastPrice = #Temp.LastPrice,    
UserLastPriceUsed = #Temp.LastPriceUsed, 
UserForwardPoints =  #Temp.ForwardPoints, 
UserForwardPointsUsed = #Temp.ForwardPointsUsed,
UserTheoreticalPriceUsed = #Temp.TheoreticalPriceUsed,     
UserSharesOutstanding =  #Temp.SharesOutstanding, 
UserSharesOutstandingUsed = #Temp.SharesOutstandingUsed
from  T_UserOptionModelInput OMI Inner Join #Temp        
on OMI.Symbol=#Temp.Symbol      
      
      
INSERT INTO T_UserOptionModelInput            
(            
Symbol,  
HistoricalVolatilityUsed,  
UserVolatility,            
UserVolatilityUsed,            
UserInterestRate,            
UserInterestRateUsed,            
UserDividend,            
UserDividendUsed,                
UserStockBorrowCost,            
UserStockBorrowCostUsed,            
UserDelta,            
UserDeltaUsed,            
UserLastPrice,            
UserLastPriceUsed,    
UserForwardPoints,      
UserForwardPointsUsed, 
UserTheoreticalPriceUsed,  
UserSharesOutstanding,
UserSharesOutstandingUsed          
)            
select       
Symbol,  
HistoricalVolUsed,    
Volatility,            
VolatilityUsed,            
IntRate,            
IntRateUsed,            
Dividend,            
DividendUsed, 
StockBorrowCost,
StockBorrowCostUsed,           
Delta,            
DeltaUsed,            
LastPrice,            
LastPriceUsed,    
ForwardPoints,      
ForwardPointsUsed,
TheoreticalPriceUsed,  
SharesOutstanding,
SharesOutstandingUsed        
from #Temp  
where  #Temp.Symbol not in (select Symbol from  T_UserOptionModelInput)       
          
DELETE FROM T_UserOptionModelInput          
WHERE          
(UserVolatility + HistoricalVolatility + UserInterestRate + UserDividend + UserStockBorrowCost + UserDelta + UserLastPrice + UserForwardPoints + UserSharesOutstanding) = 0          
AND           
(HistoricalVolatilityUsed | UserVolatilityUsed | UserInterestRateUsed | UserDividendUsed | UserStockBorrowCostUsed | UserDeltaUsed | UserLastPriceUsed | UserForwardPointsUsed |UserTheoreticalPriceUsed | UserProxySymbolUsed | UserSharesOutstandingUsed | UserClosingMarkUsed) = 0           
 
 exec sp_xml_removedocument @handle
        
COMMIT TRANSACTION TRAN1                                                                                
Drop table #Temp                                                                                                       
END TRY                                                                                                        
BEGIN CATCH                                                                                              
SET @ErrorMessage = ERROR_MESSAGE();                                                                                                        
SET @ErrorNumber = Error_number();                                                   
ROLLBACK TRANSACTION TRAN1                                            
END CATCH;   
