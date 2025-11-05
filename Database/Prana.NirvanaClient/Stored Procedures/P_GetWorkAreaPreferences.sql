CREATE PROC [dbo].[P_GetWorkAreaPreferences]    
AS    
BEGIN    
 SELECT TOP 1 IsUseWorkAreaPreferences    
  , CounterPartyID    
  , VenueID    
  , TIFID    
  , TradingAccountID    
  , CommaSeparatedFunds    
  , CalculationStrategy    
  , IsIncludeExcludeAllowed    
  FROM T_WorkAreaPreferences  
END
