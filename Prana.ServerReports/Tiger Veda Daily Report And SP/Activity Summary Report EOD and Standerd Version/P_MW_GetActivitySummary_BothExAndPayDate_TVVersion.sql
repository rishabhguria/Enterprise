          
/*          
P_MW_GetActivitySummary_BothExAndPayDate '06-15-2015','07-15-2015',1,'','Symbol'          
*/                
ALTER Procedure [dbo].[P_MW_GetActivitySummary_BothExAndPayDate_TVVersion]                     
(                    
@StartDate datetime,                                    
@EndDate datetime,                              
@IsPayDate bit,                            
@SearchString Varchar(5000) ,                                    
@SearchBy Varchar(100)                                    
)                    
As  
SELECT  
 * INTO #TempSymbol  
FROM dbo.split(@SearchString, ',')  
  
SELECT DISTINCT  
 * INTO #Symbol  
FROM #TempSymbol  
BEGIN  
SELECT  
 Symbol,  
 UnderlyingSymbol,  
 Strategy,  
 Fund,  
 Asset,  
 Underlyer,  
 Exchange,  
 UDASector,  
 UDACountry,  
 UDASecurityType,  
 UDAAssetClass,  
 UDASubSector,  
 TradeCurrency,  
 CASE  
  WHEN ((open_closeTag = 'd' OR  
  open_closeTag = 'dp') AND  
  (Descriptions = '' OR  
  Descriptions IS NULL) AND  
  DividendLocal >= 0 AND  
  Asset <> 'FixedIncome') THEN 'Dividend Received'  
  WHEN ((open_closeTag = 'd' OR  
  open_closeTag = 'dp') AND  
  (Descriptions = '' OR  
  Descriptions IS NULL) AND  
  DividendLocal < 0 AND  
  Asset <> 'FixedIncome') THEN 'Dividend Charged'  
  WHEN ((open_closeTag = 'd' OR  
  open_closeTag = 'dp') AND  
  (Asset = 'FixedIncome')) THEN 'Bond Interest'  
  WHEN ((open_closeTag = 'd' OR  
  open_closeTag = 'dp') AND  
  Descriptions <> '') THEN Descriptions  
  ELSE Side  
 END AS Side,  
 --Side ,                             
 CounterParty,  
 PrimeBroker,  
 Trader,  
 SecurityName,  
 TradeDate,  
 SettleDate AS SettlementDate,  
 Quantity,  
 Multiplier,  
 SideMultiplier,  
 AvgPrice,  
 PutOrCall,  
 CommissionLocal,  
 CommissionBase,  
 FeesLocal,  
 FeesBase,  
 OtherFeesLocal,  
 OtherFeesBase,  
 FXRate_TradeDate AS OpeningFXRate,  
--MarkFXRate_TradeDate as TradeDateFXRate,
 Case When IsNumeric(MarkFXRate_TradeDate)<>0 THEN 
      CONVERT(decimal (10,5),MarkFXRate_TradeDate)
   ELSE 
      isnull(MarkFXRate_TradeDate,0)
   End as TradeDateFXRate,
  MarkFXRate_SettleDate AS SettlementDateFXRate,  
  
 CASE  
  WHEN BaseCurrency <> TradeCurrency THEN CASE  
   WHEN FXRate_TradeDate = 0 OR  
   FXRate_TradeDate = 1 THEN AvgPrice * MarkFXRate_TradeDate  
   ELSE AvgPrice * FXRate_TradeDate  
  END  
  ELSE AvgPrice  
 END AS AvgPrice_Base,  
 CASE Open_CloseTag  
  WHEN 'D' THEN Dividend  
  WHEN 'DP' THEN Dividend  
  WHEN 'Cash' THEN NetAmountBase  -- positive for Sell and negative for buy cash entries.              
  ELSE (-1 * NetAmountBase)  
 END AS  
 NetAmountBase,  
  
 CASE Open_CloseTag  
  WHEN 'D' THEN DividendLocal  
  WHEN 'DP' THEN DividendLocal  
  WHEN 'Cash' THEN NetAmountLocal  
  ELSE (-1 * NetAmountLocal)  
 END AS  
 NetAmountLocal,  
  
 PrincipalAmountBase,  
 PrincipalAmountLocal,  
 TradeOrigin,  
 Open_CloseTag,  
 DividendLocal,  
 Dividend AS DividendBase,  
 BloomBergSymbol,  
 SedolSymbol,  
 OSISymbol,  
 IDCOSymbol,  
 ISINSymbol,  
 ISNULL(RiskCurrency, 'Undefined') AS RiskCurrency,  
 ISNULL(Issuer, 'Undefined') AS Issuer,  
 ISNULL(CountryOfRisk, 'Undefined') AS CountryOfRisk,  
 ISNULL(Region, 'Undefined') AS Region,  
 ISNULL(Analyst, 'Undefined') AS Analyst,  
 ISNULL(CustomUDA1, 'Undefined') AS CustomUDA1,----UCITSEligibleTag,              
 ISNULL(CustomUDA2, 'Undefined') AS CustomUDA2,----LiquidTag,              
 ISNULL(MarketCap, 'Undefined') AS MarketCap,  
 ISNULL(TransactionType, Side) AS TransactionType,  
 CASE  
  WHEN TransactionType = 'CASH' -----Or TransactionType = 'Cash Dividend'                        
  THEN 2----Non Trading Transaction Type                        
  ELSE 1----Trading Transaction Type                        
 END TradingNonTradingType,  
 BaseCurrency,  
 SettlCurrency AS SettlementCurrency,  
 SettlCurrFxRate,  
 SettlCurrAmt INTO #ActivitySummaryReport  
FROM T_MW_Transactions  
  
WHERE DATEDIFF(DAY, Rundate, @EndDate) >= 0  
AND DATEDIFF(DAY, @Startdate, Rundate) >= 0  
AND (  
Open_CloseTag = 'o'  
OR Open_CloseTag = 'c'  
OR Open_CloseTag = 'CASH'  
OR Open_CloseTag =  
 CASE @IsPayDate  
  WHEN 0 THEN 'd'  
  WHEN 1 THEN 'dp'  
 END  
)  
  
ALTER TABLE #ActivitySummaryReport  
ADD UnderlyingSymbolCompanyName nvarchar(200)  
  
UPDATE #ActivitySummaryReport  
SET UnderlyingSymbolCompanyName = SM.CompanyName  
FROM #ActivitySummaryReport ASR  
INNER JOIN V_SecMasterData_WithUnderlying SM  
 ON SM.TickerSymbol = ASR.UnderlyingSymbol  
  
  
  
IF (@SearchString <> '') BEGIN  
IF (@searchby = 'Symbol') BEGIN  
SELECT  
 *  
FROM #ActivitySummaryReport  
INNER JOIN #Symbol  
 ON #Symbol.items = #ActivitySummaryReport.Symbol  
ORDER BY symbol  
END ELSE IF (@searchby = 'underlyingSymbol') BEGIN  
SELECT  
 *  
FROM #ActivitySummaryReport  
INNER JOIN #Symbol  
 ON #Symbol.items = #ActivitySummaryReport.underlyingSymbol  
ORDER BY symbol  
END ELSE IF (@searchby = 'BloombergSymbol') BEGIN  
SELECT  
 *  
FROM #ActivitySummaryReport  
INNER JOIN #Symbol  
 ON #Symbol.items = #ActivitySummaryReport.BloombergSymbol  
ORDER BY symbol  
END ELSE IF (@searchby = 'SedolSymbol') BEGIN  
SELECT  
 *  
FROM #ActivitySummaryReport  
INNER JOIN #Symbol  
 ON #Symbol.items = #ActivitySummaryReport.SedolSymbol  
ORDER BY symbol  
END ELSE IF (@searchby = 'OSISymbol') BEGIN  
SELECT  
 *  
FROM #ActivitySummaryReport  
INNER JOIN #Symbol  
 ON #Symbol.items = #ActivitySummaryReport.OSISymbol  
ORDER BY symbol  
END ELSE IF (@searchby = 'IDCOSymbol') BEGIN  
SELECT  
 *  
FROM #ActivitySummaryReport  
INNER JOIN #Symbol  
 ON #Symbol.items = #ActivitySummaryReport.IDCOSymbol  
ORDER BY symbol  
END ELSE IF (@searchby = 'ISINSymbol') BEGIN  
SELECT  
 *  
FROM #ActivitySummaryReport  
INNER JOIN #Symbol  
 ON #Symbol.items = #ActivitySummaryReport.ISINSymbol  
ORDER BY symbol  
END ELSE IF (@searchby = 'CUSIPSymbol') BEGIN  
SELECT  
 *  
FROM #ActivitySummaryReport  
INNER JOIN #Symbol  
 ON #Symbol.items = #ActivitySummaryReport.CUSIPSymbol  
ORDER BY symbol  
END  
END ELSE BEGIN  
SELECT  
 *  
FROM #ActivitySummaryReport  
ORDER BY symbol  
END  
  
END  
DROP TABLE #ActivitySummaryReport, #Symbol, #TempSymbol