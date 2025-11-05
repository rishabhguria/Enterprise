
Create PROCEDURE [dbo].[P_GetCustomPIData]     
AS      
BEGIN                                   
 SELECT DISTINCT OMI.Symbol AS Symbol   
,'' AS SecurityDescription      
  ,IsNull(OMI.HistoricalVolatility, 0) AS HistoricalVolatility      
  ,IsNull(OMI.HistoricalVolatilityUsed, 0) AS HistoricalVolatilityUsed      
  ,IsNull(OMI.UserVolatility, 0) AS UserVolatility      
  ,IsNull(OMI.UserVolatilityUsed, 0) AS UserVolatilityUsed      
  ,IsNull(OMI.UserInterestRate, 0) AS UserInterestRate      
  ,IsNull(OMI.UserInterestRateUsed, 0) AS UserInterestRateUsed      
  ,IsNull(OMI.UserDividend, 0) AS UserDividend      
  ,IsNull(OMI.UserDividendUsed, 0) AS UserDividendUsed         
  ,IsNull(OMI.UserStockBorrowCost, 0) AS UserStockBorrowCost      
  ,IsNull(OMI.UserStockBorrowCostUsed, 0) AS UserStockBorrowCostUsed      
  ,IsNull(OMI.UserDelta, 0) AS UserDelta      
  ,IsNull(OMI.UserDeltaUsed, 0) AS UserDeltaUsed      
  ,IsNull(OMI.UserLastPrice, 0) AS UserLastPrice      
  ,IsNull(OMI.UserLastPriceUsed, 0) AS UserLastPriceUsed      
  ,IsNull(OMI.UserForwardPoints, 0) AS UserForwardPoints      
  ,IsNull(OMI.UserForwardPointsUsed, 0) AS UserForwardPointsUsed      
  ,IsNull(OMI.UserTheoreticalPriceUsed, 0) AS UserTheoreticalPriceUsed      
  ,IsNull(OMI.UserProxySymbolUsed, 0) AS UserProxySymbolUsed      
  ,ISNULL(OMI.UserSharesOutstandingUsed, 0) AS UserSharesOutstandingUsed      
  ,ISNULL(OMI.UserSharesOutstanding, 0) AS UserSharesOutstanding      
  ,ISNULL(OMI.UserClosingMarkUsed, 0) AS UserClosingMarkUsed       
  ,ISNULL(OMI.SMUserSharesOutstanding, 0) AS SMUserSharesOutstanding      
  ,ISNULL(OMI.SMUserSharesOutstandingUsed, 0) AS SMUserSharesOutstandingUsed            
  ,'' AS OSISymbol
  ,'' AS IDCOSymbol
  ,'PS' AS PSSymbol
  ,'' AS AssetID      
  ,'' AS UnderLyingSymbol      
  ,'' AS ExpirationDate      
  ,'' AS StrikePrice      
  ,'' AS PutorCall      
  ,'' AS VsCurrencyID      
  ,'' AS LeadCurrencyID      
  ,'' AS ProxySymbol      
  ,'' AS BloombergSymbol      
  ,'' AS IsHistorical      
  ,'' AS AuecID    
  ,IsNull(OMI.IsManuallyAdded,0) as ManualInput 
  ,'' AS BloombergSymbolWithExchangeCode
 FROM  T_UserOptionModelInput AS OMI  
Where OMI.IsManuallyAdded=1   
END 

