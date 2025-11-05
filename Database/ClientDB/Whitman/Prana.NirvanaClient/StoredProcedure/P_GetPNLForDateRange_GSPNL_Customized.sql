            
/*****************************************************************                            
Author:Ashish Poddar                                                                                                                       
Creation Date: September 07, 2012                                 
Description : Get Realized and Unrealized PNL for date range, Data picked from T_MW_genericPNL table                            
                          
Usage :                           
EXEC P_GetPNLForDateRange_GSPNL_Customized @StartDate='06-29-2016',@EndDate='06-29-2016',@AssetIds=N'',@FundIds=N'1267,1276'            
******************************************************************/            
CREATE PROCEDURE [dbo].[P_GetPNLForDateRange_GSPNL_Customized]   
(            
 @StartDate DATETIME            
 ,@EndDate DATETIME            
 ,@AssetIds VARCHAR(MAX)            
 ,@FundIds VARCHAR(MAX)            
 )            
AS   
  
--Declare @StartDate DATETIME            
--Declare @EndDate DATETIME            
--Declare @AssetIds VARCHAR(MAX)            
--Declare @FundIds VARCHAR(MAX)  
--  
--Set @StartDate = '06-29-2016'  
--Set @EndDate = '06-29-2016'  
--Set @AssetIds = N''  
--Set @FundIds = N'1267,1276'  
   
           
BEGIN            
 DECLARE @IncludeFXPNLinFutures BIT            
 DECLARE @IncludeFXPNLinInternationalFutOptions BIT            
 DECLARE @IncludeFXPNLinEquity BIT            
 DECLARE @IncludeFXPNLinEquityOption BIT            
 DECLARE @IncludeFXPNLinFX BIT            
 DECLARE @IncludeFXPNLinSwaps BIT            
 DECLARE @IncludeFXPNLinOther BIT            
            
 SET @IncludeFXPNLinFutures = 1            
 SET @IncludeFXPNLinInternationalFutOptions = 1            
 SET @IncludeFXPNLinEquity = 1            
 SET @IncludeFXPNLinEquityOption = 1            
 SET @IncludeFXPNLinFX = 1            
 SET @IncludeFXPNLinSwaps = 1            
 SET @IncludeFXPNLinOther = 1            
            
 CREATE TABLE #AssetClass (AssetID INT)            
            
 IF (            
   @AssetIds IS NULL            
   OR @AssetIds = ''            
   )            
 BEGIN            
  INSERT INTO #AssetClass            
  SELECT AssetID            
  FROM T_Asset            
 END            
 ELSE            
  INSERT INTO #AssetClass            
  SELECT Items AS AssetID            
  FROM dbo.Split(@AssetIds, ',')            
            
 --This temporary table stores comma seprated asset id's and corresponding asset name                          
 CREATE TABLE #T_Asset (            
  AssetID INT            
  ,AssetName VARCHAR(100)            
  )            
            
 INSERT INTO #T_Asset (            
  AssetID            
  ,AssetName            
  )            
 SELECT AssetClass.AssetID            
  ,Asset.AssetName            
 FROM #AssetClass AssetClass            
 INNER JOIN T_Asset Asset ON AssetClass.AssetId = Asset.AssetID            
            
 --This temporary table stores comma seprated fund id's                                                   
 CREATE TABLE #FundClass (FundID INT)            
            
 IF (            
   @FundIds IS NULL            
   OR @FundIds = ''            
   )            
 BEGIN            
  INSERT INTO #FundClass            
  SELECT CompanyFundID AS FundID            
  FROM T_CompanyFunds            
 END            
 ELSE            
  INSERT INTO #FundClass            
  SELECT Items AS FundID            
  FROM dbo.Split(@FundIds, ',')            
            
 --This temporary table stores comma seprated fund id's and corresponding asset name                           
 CREATE TABLE #T_CompanyFunds (            
  FundID INT            
  ,FundName VARCHAR(100)            
  )            
            
 INSERT INTO #T_CompanyFunds (            
  FundID            
  ,FundName            
  )            
 SELECT FundClass.FundID            
  ,Funds.FundName            
 FROM #FundClass FundClass            
 INNER JOIN T_CompanyFunds Funds ON FundClass.FundID = Funds.CompanyFundID            
            
 SELECT Rundate            
  ,Symbol            
  ,Fund AS AccountName            
  ,MasterFund            
  ,Asset            
  ,Side            
  --Start Date FX Rate                   
  ,BeginningFXRate AS StartDateFXRate            
  --End Date FX Rate                   
  ,EndingFXRate AS EndDateFXRate            
  -- Trade FX Rate                   
  ,TradeDateFXRate            
  -- Start Date Mark Price                   
  ,BeginningPriceLocal AS StartDateMarkPrice            
  -- End Date Mark Price                   
  ,EndingPriceLocal AS EndDateMarkPrice            
  --Unrealized P&L inclusive of commissions (Local)                  
  ,ChangeInUnrealizedPNL_Local AS UnRealizedPNLWithCommission_Local            
  --Unrealized P&L exclusive of commissions (Local)                  
  ,CASE             
   WHEN (            
     datediff(d, @StartDate, tradedate) >= 0            
     AND datediff(d, tradedate, @EndDate) >= 0            
     AND Open_closeTag = 'O'            
     AND datediff(d, rundate, tradedate) = 0            
     )            
    THEN ChangeInUNRealizedPNL_Local + TotalOpenCommissionAndFees_Local            
   WHEN (            
     datediff(d, tradedate, closingDate) <> 0            
     AND Open_closeTag = 'C'            
     )            
    THEN ChangeInUNRealizedPNL_Local - TotalOpenCommissionAndFees_Local            
   ELSE ChangeInUNRealizedPNL_Local            
   END AS UnRealizedPNLWithoutCommission_Local            
  -- Unrealized P&L inclusive of commissions (Base)                  
  ,ChangeInUnrealizedPNL AS UnRealizedPNLWithCommission_Base            
  -- Unrealized P&L inclusive of commissions (Base)                  
  ,CASE             
   WHEN (            
     datediff(d, @StartDate, tradedate) >= 0            
     AND datediff(d, tradedate, @EndDate) >= 0            
     AND Open_closeTag = 'O'            
     AND datediff(d, rundate, tradedate) = 0            
     )            
    THEN ChangeInUNRealizedPNL + TotalOpenCommissionAndFees_Base            
   WHEN (            
     datediff(d, tradedate, closingDate) <> 0            
     AND Open_closeTag = 'C'            
     )            
    THEN ChangeInUNRealizedPNL - TotalOpenCommissionAndFees_Base            
   ELSE ChangeInUNRealizedPNL            
   END AS UnRealizedPNLWithoutCommission_Base            
  -------------------------------------------------Region short term PNL---------------------------------------------------                  
  ---------------------------------------------------Including Commission--------------------------------------------                  
  ,CASE             
   --Short term Realized P&L for Futures with Commission and Both FX Rate                       
   WHEN (            
     (            
      Asset = 'Future'            
      AND @IncludeFXPNLinFutures = 1            
      )            
     OR (            
      Asset = 'FutureOption'            
      AND @IncludeFXPNLinInternationalFutOptions = 1            
      )            
     )            
    THEN 0.4 * (RealizedTradingPNLOnCost + RealizedFXPNLOnCost)            
     --Short term Realized P&L for Futures with Commission and Single FX Rate                       
   WHEN (            
     (            
      Asset = 'Future'            
      AND @IncludeFXPNLinFutures = 0            
      )            
     OR (            
      Asset = 'FutureOption'            
      AND @IncludeFXPNLinInternationalFutOptions = 0            
      )            
     )            
    THEN 0.4 * (RealizedTradingPNLOnCost)            
   WHEN (            
     DateDiff(d, DateAdd(year, 1, OriginalPurchaseDate), ClosingDate) <= 0            
     OR Side = 'Short'            
     )            
    THEN CASE             
      --Short term Realized P&L with Commission and Both FX Rate                          
      WHEN (            
        (            
         Asset = 'Equity'            
         AND IsSwapped = 0            
         AND @IncludeFXPNLinEquity = 1            
)            
        OR (            
         Asset = 'EquityOption'            
         AND @IncludeFXPNLinEquityOption = 1            
         )            
        OR (            
         Asset = 'FX'            
         AND @IncludeFXPNLinFX = 1            
         )            
        OR (            
         Asset = 'Equity'            
         AND IsSwapped = 1            
         AND @IncludeFXPNLinSwaps = 1            
         )            
        OR (            
         Asset NOT IN (            
          'Equity'            
          ,'EquityOption'            
          ,'Future'            
          ,'FutureOption'            
          ,'FX'            
          )            
         AND @IncludeFXPNLinOther = 1            
         )            
        )            
       THEN RealizedTradingPNLOnCost + RealizedFXPNLOnCost            
        --Short term Realized P&L with Commission and Single FX Rate                          
      WHEN (            
        (            
         Asset = 'Equity'            
         AND IsSwapped = 0            
         AND @IncludeFXPNLinEquity = 0            
         )            
     OR (            
         Asset = 'EquityOption'            
         AND @IncludeFXPNLinEquityOption = 0            
         )            
        OR (            
         Asset = 'Future'            
         AND @IncludeFXPNLinFutures = 0            
         )            
        OR (            
         Asset = 'FX'            
         AND @IncludeFXPNLinFX = 0            
         )            
        OR (            
         Asset = 'Equity'            
         AND IsSwapped = 1            
         AND @IncludeFXPNLinSwaps = 0            
         )            
        OR (            
         Asset NOT IN (            
          'Equity'            
          ,'EquityOption'            
          ,'Future'            
          ,'FutureOption'            
          ,'FX'            
          )            
         AND @IncludeFXPNLinOther = 0            
         )            
        )            
       THEN RealizedTradingPNLOnCost            
      ELSE 0            
      END            
   ELSE 0            
   END AS ShortTermTotalRealizedPNLWithCommission            
  ,            
  --------------------------------------------------------------Excluding Commission-------------------------------------------------------                  
  CASE             
   --Short term Realized P&L for Futures without Commission and Both FX Rate                      
   WHEN (            
     (            
      Asset = 'Future'            
      AND @IncludeFXPNLinFutures = 1            
      )            
     OR (            
      Asset = 'FutureOption'            
      AND @IncludeFXPNLinInternationalFutOptions = 1            
      )            
     )            
    THEN 0.4 * (RealizedTradingPNLOnCost + RealizedFXPNLOnCost + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base)            
     --Short term Realized P&L for Futures without Commission and Single FX Rate                       
   WHEN (            
     (            
      Asset = 'Future'            
      AND @IncludeFXPNLinFutures = 0            
      )            
     OR (            
      Asset = 'FutureOption'            
      AND @IncludeFXPNLinInternationalFutOptions = 0            
      )            
     )            
    THEN 0.4 * (RealizedTradingPNLOnCost + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base)            
   WHEN (            
     DateDiff(d, DateAdd(year, 1, OriginalPurchaseDate), ClosingDate) <= 0            
     OR Side = 'Short'            
     )            
    THEN CASE             
      --Short term Realized P&L without Commission and Both FX Rate                          
      WHEN (            
        (            
         Asset = 'Equity'            
         AND IsSwapped = 0            
         AND @IncludeFXPNLinEquity = 1            
         )            
        OR (            
         Asset = 'EquityOption'            
         AND @IncludeFXPNLinEquityOption = 1            
         )            
        OR (            
         Asset = 'Future'            
         AND @IncludeFXPNLinFutures = 1            
         )            
        OR (            
         Asset = 'FX'            
         AND @IncludeFXPNLinFX = 1            
         )            
        OR (            
         Asset = 'Equity'            
         AND IsSwapped = 1            
         AND @IncludeFXPNLinSwaps = 1            
         )            
        OR (            
         Asset NOT IN (            
          'Equity'            
          ,'EquityOption'            
          ,'Future'            
          ,'FutureOption'            
          ,'FX'            
          )            
         AND @IncludeFXPNLinOther = 1            
         )            
        )            
       THEN (RealizedTradingPNLOnCost + RealizedFXPNLOnCost) + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base            
        --Short term Realized P&L without Commission and Single FX Rate                          
      WHEN (            
        (            
         Asset = 'Equity'            
         AND IsSwapped = 0            
         AND @IncludeFXPNLinEquity = 0            
         )            
        OR (            
         Asset = 'EquityOption'            
         AND @IncludeFXPNLinEquityOption = 0            
         )            
        OR (            
         Asset = 'Future'            
         AND @IncludeFXPNLinFutures = 0            
         )            
        OR (            
 Asset = 'FX'            
         AND @IncludeFXPNLinFX = 0            
         )            
        OR (            
         Asset = 'Equity'            
         AND IsSwapped = 1            
         AND @IncludeFXPNLinSwaps = 0            
         )            
        OR (            
         Asset NOT IN (            
          'Equity'            
          ,'EquityOption'            
          ,'Future'            
          ,'FutureOption'            
          ,'FX'            
          )            
         AND @IncludeFXPNLinOther = 0            
         )            
        )            
       THEN RealizedTradingPNLOnCost + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base            
      ELSE 0            
      END            
   ELSE 0            
   END AS ShortTermTotalRealizedPNLWithoutCommission            
  -----------------------------------------------------------------------------------------------------------------------------------------                  
  --------------------------------------------------------End of short term realized PNL---------------------------------------------------                                
  -------------------------------------------------------------Including Commission----------------------------------------------------------                  
  ,CASE             
   --Long term Realized P&L for Futures with Commission and Both FX Rate                       
   WHEN (            
     (            
      Asset = 'Future'            
      AND @IncludeFXPNLinFutures = 1            
      )            
     OR (            
      Asset = 'FutureOption'            
      AND @IncludeFXPNLinInternationalFutOptions = 1            
      )            
     )            
    THEN 0.6 * (RealizedTradingPNLOnCost + RealizedFXPNLOnCost)            
     --Long term Realized P&L for Futures with Commission and Single FX Rate                       
   WHEN (            
     (            
      Asset = 'Future'            
      AND @IncludeFXPNLinFutures = 0            
      )            
     OR (            
      Asset = 'FutureOption'            
      AND @IncludeFXPNLinInternationalFutOptions = 0            
      )            
     )            
    THEN 0.6 * (RealizedTradingPNLOnCost)            
   WHEN (            
     DateDiff(d, DateAdd(year, 1, OriginalPurchaseDate), ClosingDate) > 0            
     AND Side <> 'Short'            
     )            
    THEN CASE             
      --Long term Realized P&L with Commission and Both FX Rate                          
      WHEN (            
        (            
         Asset = 'Equity'            
         AND IsSwapped = 0            
         AND @IncludeFXPNLinEquity = 1            
         )            
        OR (            
         Asset = 'EquityOption'            
         AND @IncludeFXPNLinEquityOption = 1            
         )            
        OR (            
         Asset = 'FX'            
         AND @IncludeFXPNLinFX = 1            
         )            
        OR (            
         Asset = 'Equity'            
         AND IsSwapped = 1            
         AND @IncludeFXPNLinSwaps = 1            
         )            
        OR (            
         Asset NOT IN (            
          'Equity'            
          ,'EquityOption'            
          ,'Future'            
          ,'FutureOption'            
          ,'FX'            
          )            
         AND @IncludeFXPNLinOther = 1            
         )            
        )            
       THEN RealizedTradingPNLOnCost + RealizedFXPNLOnCost            
        --Long term Realized P&L with Commission and Single FX Rate                          
      WHEN (            
        (            
         Asset = 'Equity'            
         AND IsSwapped = 0            
         AND @IncludeFXPNLinEquity = 0            
         )            
        OR (            
         Asset = 'EquityOption'            
         AND @IncludeFXPNLinEquityOption = 0            
         )            
        OR (            
         Asset = 'Future'            
         AND @IncludeFXPNLinFutures = 0            
         )            
        OR (            
         Asset = 'FX'            
         AND @IncludeFXPNLinFX = 0            
         )            
        OR (            
         Asset = 'Equity'            
         AND IsSwapped = 1            
         AND @IncludeFXPNLinSwaps = 0            
         )            
        OR (            
         Asset NOT IN (            
          'Equity'            
          ,'EquityOption'            
          ,'Future'            
          ,'FutureOption'            
          ,'FX'            
          )            
         AND @IncludeFXPNLinOther = 0            
         )            
        )            
       THEN RealizedTradingPNLOnCost            
      ELSE 0            
      END            
   ELSE 0            
   END AS LongTermTotalRealizedPNLWithCommission            
  ,            
  ------------------------------------------------------------------------------------------------------------------                  
  --------------------------------------------------Excluding Commission--------------------------------------------                  
  CASE             
   --Long term Realized P&L for Futures without Commission and Both FX Rate                      
   WHEN (            
     (            
      Asset = 'Future'            
      AND @IncludeFXPNLinFutures = 1            
      )            
OR (            
      Asset = 'FutureOption'            
      AND @IncludeFXPNLinInternationalFutOptions = 1            
      )            
     )            
    THEN 0.6 * (RealizedTradingPNLOnCost + RealizedFXPNLOnCost + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base)            
     --Long term Realized P&L for Futures without Commission and Single FX Rate                       
   WHEN (            
     (            
      Asset = 'Future'            
      AND @IncludeFXPNLinFutures = 0            
      )            
     OR (            
      Asset = 'FutureOption'            
      AND @IncludeFXPNLinInternationalFutOptions = 0            
      )            
     )            
    THEN 0.6 * (RealizedTradingPNLOnCost + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base)            
   WHEN (            
     DateDiff(d, DateAdd(year, 1, OriginalPurchaseDate), ClosingDate) > 0            
     AND Side <> 'Short'            
     )            
    THEN CASE             
      --Long term Realized P&L without Commission and Both FX Rate                          
      WHEN (            
        (            
   Asset = 'Equity'            
         AND IsSwapped = 0            
         AND @IncludeFXPNLinEquity = 1            
         )            
        OR (            
         Asset = 'EquityOption'            
         AND @IncludeFXPNLinEquityOption = 1            
         )            
        OR (            
         Asset = 'Future'            
         AND @IncludeFXPNLinFutures = 1            
         )            
        OR (            
         Asset = 'FX'            
         AND @IncludeFXPNLinFX = 1            
         )            
        OR (            
         Asset = 'Equity'            
         AND IsSwapped = 1            
         AND @IncludeFXPNLinSwaps = 1            
         )            
        OR (            
         Asset NOT IN (            
          'Equity'            
          ,'EquityOption'            
          ,'Future'            
          ,'FutureOption'            
          ,'FX'            
          )            
         AND @IncludeFXPNLinOther = 1            
         )            
        )            
       THEN (RealizedTradingPNLOnCost + RealizedFXPNLOnCost) + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base            
        --Long term Realized P&L without Commission and Single FX Rate                          
      WHEN (            
        (            
         Asset = 'Equity'            
         AND IsSwapped = 0            
         AND @IncludeFXPNLinEquity = 0            
         )            
        OR (            
         Asset = 'EquityOption'            
         AND @IncludeFXPNLinEquityOption = 0            
         )            
        OR (            
         Asset = 'Future'            
         AND @IncludeFXPNLinFutures = 0            
         )            
        OR (            
         Asset = 'FX'            
         AND @IncludeFXPNLinFX = 0            
         )            
        OR (            
         Asset = 'Equity'            
         AND IsSwapped = 1            
         AND @IncludeFXPNLinSwaps = 0            
         )            
        OR (            
         Asset NOT IN (            
          'Equity'            
          ,'EquityOption'            
          ,'Future'            
          ,'FutureOption'            
,'FX'            
          )            
         AND @IncludeFXPNLinOther = 0            
         )            
        )            
       THEN RealizedTradingPNLOnCost + TotalOpenCommissionAndFees_Base + TotalClosedCommissionAndFees_Base            
      ELSE 0            
      END            
   ELSE 0            
   END AS LongTermTotalRealizedPNLWithoutCommission            
  ,MW.ISSwapped            
  ,Asset.AssetID            
  ,MW.BeginningQuantity AS Quantity            
  ,MW.UnitCostLocal AS AvgPX            
  ,MW.BaseCurrency  
,(TotalRealizedPNLOnCost + ChangeInUNRealizedPNL + Dividend) As TotalRealizedPnl            
  into #temp          
 ------------------------------------------------------------------------------------------------------------------                  
 ------------------------------------------------------End of long term PNL------------------------------------------                  
 FROM T_MW_GenericPNL MW            
 --join for the asset filters                          
 INNER JOIN #T_Asset Asset ON MW.Asset = Asset.AssetName            
 --join for the fund filters                          
 INNER JOIN #T_CompanyFunds Funds ON MW.Fund = Funds.FundName            
 WHERE datediff(day, @Startdate, Rundate) >= 0            
  AND datediff(day, Rundate, @EndDate) >= 0    
And Open_CloseTag <> 'Accruals'  And Asset <> 'Cash'         
--and Symbol = 'GDX'          
--and fundname = 'U1420062'          
 ORDER BY RunDate            
  ,Fund            
  ,Symbol            
END            
          
select *  
--,(LongTermTotalRealizedPNLWithCommission + ShortTermTotalRealizedPNLWithCommission) as TotalRealizedPnl   
from #temp          
--where (LongTermTotalRealizedPNLWithCommission + ShortTermTotalRealizedPNLWithCommission) <> 0        
          
drop table #temp,#AssetClass,#T_Asset,#FundClass,#T_CompanyFunds