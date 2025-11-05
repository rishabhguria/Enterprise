CREATE PROCEDURE P_GetOptionModelTable                                                  
                                                                                                        
AS       
      
BEGIN      
      
SELECT      
Symbol,
HistoricalVolatility,      
IsNull(HistoricalVolatilityUsed, 0) as HistoricalVolatilityUsed,      
UserVolatility,      
IsNull(UserVolatilityUsed, 0) as UserVolatilityUsed,      
UserInterestRate,      
IsNull(UserInterestRateUsed, 0) as UserInterestRateUsed,      
UserDividend,      
IsNull(UserDividendUsed, 0) as UserDividendUsed,         
UserStockBorrowCost,      
IsNull(UserStockBorrowCostUsed, 0) as UserStockBorrowCostUsed,      
UserDelta,      
IsNull(UserDeltaUsed, 0) as UserDeltaUsed,      
UserLastPrice,      
IsNull(UserLastPriceUsed, 0) as UserLastPriceUsed    
FROM T_UserOptionModelInput  
      
END 