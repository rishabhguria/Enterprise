/*        
Exec P_GetFundwiseOpenPosition_EOD_Batch '','2019-08-21'        
'1254,1252,1253,1239,1260,1261,1213,1258,1251,1214,1259,1255,1238,1257,1250,1247,1248,1249,1262,1240,1241,1263,1256','2017-05-25'        
*/    
CREATE PROCEDURE [dbo].[P_GetFundwiseOpenPosition_EOD_Batch] (    
 @CompanyFundIDs VARCHAR(max)    
 ,@InputDate DATETIME    
 )    
AS    
    
--Declare @CompanyFundIDs VARCHAR(max) ,@InputDate DATETIME    
--SET @CompanyFundIDs = ''    
--SET @InputDate = '2019-09-17'    
    
SET NOCOUNT ON                                                           
BEGIN    
 IF (@InputDate = '')    
 BEGIN    
  SET @InputDate = GetDate()    
 END    
    
 -- Update input date to last business date               
 --SELECT @InputDate = dbo.AdjustBusinessDays(@InputDate,-1,1)          
 ----Select @InputDate                                     
 DECLARE @Fund TABLE (FundID INT)    
    
 IF (    
   @CompanyFundIDs IS NULL    
   OR @CompanyFundIDs = ''    
   )    
  INSERT INTO @Fund    
  SELECT CompanyFundID AS FundID    
  FROM T_CompanyFunds Where IsActive=1    
 ELSE    
  INSERT INTO @Fund    
  SELECT Cast(Items AS INT)    
  FROM dbo.Split(@companyFundIDs, ',')    
    
 CREATE TABLE #TempTaxlotPK (Taxlot_PK BIGINT)    
    
 INSERT INTO #TempTaxlotPK    
 SELECT MAX(Taxlot_PK)    
 FROM PM_Taxlots    
 INNER JOIN @Fund F ON PM_Taxlots.FundID = F.FundID    
 WHERE Datediff(d, AUECModifiedDate, @InputDate) >= 0    
 GROUP BY TaxlotId    
    
 CREATE TABLE #PositionTable (    
  TaxLotID VARCHAR(50)    
  ,AUECLocalDate DATETIME    
  ,SideID CHAR(1)    
  ,Symbol VARCHAR(200)    
  ,Quantity FLOAT    
  ,AvgPX FLOAT    
  ,FundID INT    
  ,AssetID INT    
  ,UnderLyingID INT    
  ,ExchangeID INT    
  ,CurrencyID INT    
  ,AUECID INT    
  ,TotalCommissionandFees FLOAT    
  ,Multiplier FLOAT    
  ,SettlementDate DATETIME    
  ,LeadCurrencyID INT    
  ,VsCurrencyID INT    
  ,ExpirationDate DATETIME    
  ,Description VARCHAR(max)    
  ,Level2ID INT    
  ,NotionalValue FLOAT    
  ,BenchMarkRate FLOAT    
  ,Differential FLOAT    
  ,OrigCostBasis FLOAT    
  ,DayCount INT    
  ,SwapDescription VARCHAR(max)    
  ,FirstResetDate DATETIME    
  ,OrigTransDate DATETIME    
  ,IsSwapped BIT    
  ,AllocationDate DATETIME    
  ,GroupID VARCHAR(50)    
  ,PositionTag INT    
  ,FXRate FLOAT    
  ,FXConversionMethodOperator VARCHAR(5)    
  ,CompanyName VARCHAR(500)    
  ,UnderlyingSymbol VARCHAR(50)    
  ,Delta FLOAT    
  ,PutOrCall VARCHAR(5)    
  ,IsGrPreAllocated BIT    
  ,GrCumQty FLOAT    
  ,GrAllocatedQty FLOAT    
  ,GrQuantity FLOAT    
  ,Taxlot_Pk BIGINT    
  ,ParentRow_Pk BIGINT    
  ,StrikePrice FLOAT    
  ,UserID INT    
  ,CounterPartyID INT    
  ,CorpActionID UNIQUEIDENTIFIER    
  ,Coupon FLOAT    
  ,IssueDate DATETIME    
  ,MaturityDate DATETIME    
  ,FirstCouponDate DATETIME    
  ,CouponFrequencyID INT    
  ,AccrualBasisID INT    
  ,BondTypeID INT    
  ,IsZero BIT    
  ,ProcessDate DATETIME    
  ,OriginalPurchaseDate DATETIME    
  ,IsNDF BIT    
  ,FixingDate DATETIME    
  ,IDCO VARCHAR(50)    
  ,OSI VARCHAR(50)    
  ,SEDOL VARCHAR(50)    
  ,CUSIP VARCHAR(50)    
  ,Bloomberg VARCHAR(50)    
  ,MasterFund VARCHAR(50)    
  ,UnderlyingDelta FLOAT    
  ,ISINSymbol VARCHAR(50)    
  ,LotId VARCHAR(200)    
  ,ExternalTransId VARCHAR(100)    
  ,TradeAttribute1 VARCHAR(200)    
  ,TradeAttribute2 VARCHAR(200)    
  ,TradeAttribute3 VARCHAR(200)    
  ,TradeAttribute4 VARCHAR(200)    
  ,TradeAttribute5 VARCHAR(200)    
  ,TradeAttribute6 VARCHAR(200)    
  ,ProxySymbol VARCHAR(100)    
  ,AssetName VARCHAR(100)    
  ,SecurityTypeName VARCHAR(200)    
  ,SectorName VARCHAR(100)    
  ,SubSectorName VARCHAR(100)    
  ,CountryName VARCHAR(100)    
  ,BBGID VARCHAR(20)    
  ,TransactionType VARCHAR(200)    
  ,ExecutedQty FLOAT    
  ,ClosingTaxlotId VARCHAR(50)    
  ,ReutersSymbol VARCHAR(50)    
  ,InternalComments VARCHAR(500)    
  ,SettlCurrency VARCHAR(4)    
  ,Symbol_PK BIGINT    
  ,StratergyName Varchar(200)  
  )    
    
 INSERT INTO #PositionTable    
 SELECT PT.TaxLotID AS TaxLotID    
  ,G.AUECLocalDate AS TradeDate    
  ,PT.OrderSideTagValue AS SideID    
  ,PT.Symbol AS Symbol    
  ,PT.TaxLotOpenQty AS TaxLotOpenQty    
  ,PT.AvgPrice AS AvgPrice    
  ,PT.FundID AS FundID    
  ,G.AssetID AS AssetID    
  ,G.UnderLyingID AS UnderLyingID    
  ,G.ExchangeID AS ExchangeID    
  ,G.CurrencyID AS CurrencyID    
  ,G.AUECID AS AUECID    
  ,PT.OpenTotalCommissionandFees    
  ,Isnull(V_SecMasterData.Multiplier, 1) AS Multiplier    
  ,G.SettlementDate AS SettlementDate    
  ,V_SecMasterData.LeadCurrencyID    
  ,V_SecMasterData.VsCurrencyID    
  ,Isnull(V_SecMasterData.ExpirationDate, 1 / 1 / 1800) AS ExpirationDate    
  ,G.Description AS Description    
  ,PT.Level2ID AS Level2ID    
  ,Isnull((    
    PT.TaxLotOpenQty * SW.NotionalValue / CASE     
     WHEN (    
       G.CumQty IS NULL    
       OR G.CumQty = 0    
       )    
      THEN 1    
     ELSE G.CumQty    
     END    
    ), 0) AS NotionalValue    
  ,Isnull(SW.BenchMarkRate, 0) AS BenchMarkRate    
  ,Isnull(SW.Differential, 0) AS Differential    
  ,Isnull(SW.OrigCostBasis, 0) AS OrigCostBasis    
  ,Isnull(SW.DayCount, 0) AS DayCount    
  ,Isnull(SW.SwapDescription, '') AS SwapDescription    
  ,SW.FirstResetDate AS FirstResetDate    
  ,SW.OrigTransDate AS OrigTransDate    
  ,G.IsSwapped AS IsSwapped    
  ,G.AllocationDate AS AUECLocalDate    
  ,G.GroupID    
  ,PT.PositionTag    
  ,IsNull(PT.FXRate, G.FXRate) AS FXRate    
  ,IsNull(PT.FXConversionMethodOperator, G.FXConversionMethodOperator) AS FXConversionMethodOperator    
  ,Replace(isnull(V_SecMasterData.CompanyName, ''), ',', ' ') AS CompanyName    
  ,isnull(V_SecMasterData.UnderlyingSymbol, '') AS UnderlyingSymbol    
  ,IsNull(V_SecMasterData.Delta, 1) AS Delta    
  ,IsNull(V_SecMasterData.PutOrCall, '') AS PutOrCall    
  ,G.IsPreAllocated    
  ,G.CumQty    
  ,G.AllocatedQty    
  ,G.Quantity    
  ,PT.taxlot_Pk    
  ,PT.ParentRow_Pk    
  ,IsNull(V_SecMasterData.StrikePrice, 0) AS StrikePrice    
  ,G.UserID    
  ,G.CounterPartyID    
  ,CATaxlots.CorpActionID    
  ,V_SecMasterData.Coupon    
  ,V_SecMasterData.IssueDate    
  ,V_SecMasterData.MaturityDate    
  ,V_SecMasterData.FirstCouponDate    
  ,V_SecMasterData.CouponFrequencyID    
  ,V_SecMasterData.AccrualBasisID    
  ,V_SecMasterData.BondTypeID    
  ,V_SecMasterData.IsZero    
  ,G.ProcessDate    
  ,G.OriginalPurchaseDate    
  ,V_SecMasterData.IsNDF    
  ,V_SecMasterData.FixingDate    
  ,V_SecMasterData.IDCOSymbol    
  ,V_SecMasterData.OSISymbol    
  ,V_SecMasterData.SEDOLSymbol    
  ,V_SecMasterData.CUSIPSymbol    
  ,V_SecMasterData.BloombergSymbol    
  ,MF.MasterFundName AS MasterFund    
  ,V_SecMasterData.UnderlyingDelta    
  ,V_SecMasterData.ISINSymbol    
  ,PT.LotId    
  ,PT.ExternalTransId    
  ,PT.TradeAttribute1    
  ,PT.TradeAttribute2    
  ,PT.TradeAttribute3    
  ,PT.TradeAttribute4    
  ,PT.TradeAttribute5    
  ,PT.TradeAttribute6    
  ,V_SecMasterData.ProxySymbol    
  ,V_SecMasterData.AssetName    
  ,V_SecMasterData.SecurityTypeName    
  ,V_SecMasterData.SectorName    
  ,V_SecMasterData.SubSectorName    
  ,V_SecMasterData.CountryName    
  ,V_SecMasterData.BBGID    
  ,G.TransactionType    
  ,L2.TaxLotQty AS ExecutedQty    
  ,CATaxlotsForClosing.ClosingTaxlotId    
  ,V_SecMasterData.ReutersSymbol    
  ,G.InternalComments    
  ,COALESCE(PTCUR.CurrencySymbol, GCUR.CurrencySymbol, 'None') AS SettlCurrency    
  ,V_SecMasterData.Symbol_PK ,  
  CS.StrategyName  
 FROM PM_Taxlots PT WITH (NOLOCK)    
 INNER JOIN @Fund F ON F.FundID = PT.FundID    
 INNER JOIN #TempTaxlotPK T ON T.Taxlot_PK = PT.Taxlot_PK    
 INNER JOIN T_Level2Allocation L2 ON L2.TaxLotID = PT.TaxLotID    
 INNER JOIN T_CompanyStrategy CS ON CS.CompanyStrategyID = L2.Level2ID    
 INNER JOIN T_Group G ON G.GroupID = PT.GroupID    
 INNER JOIN V_SecMasterData ON PT.Symbol = V_SecMasterData.TickerSymbol    
 LEFT OUTER JOIN T_SwapParameters SW ON G.GroupID = SW.GroupID    
 LEFT OUTER JOIN PM_CorpActionTaxlots CATaxlots ON PT.Taxlot_PK = CATaxlots.FKId    
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = F.FundID    
 LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID    
 LEFT OUTER JOIN T_Currency PTCUR ON PT.SettlCurrency = PTCUR.CurrencyID    
 LEFT OUTER JOIN T_Currency GCUR ON G.SettlCurrency = GCUR.CurrencyID    
 LEFT OUTER JOIN PM_CorpActionTaxlots CATaxlotsForClosing ON PT.TaxLotID = CATaxlotsForClosing.TaxlotId    
  AND CATaxlotsForClosing.ClosingTaxlotId IS NOT NULL    
 WHERE TaxLotOpenQty <> 0    
    
 CREATE TABLE #FXConversionRatesDayMark (    
  FromCurrencyID INT    
  ,ToCurrencyID INT    
  ,RateValue FLOAT    
  ,ConversionMethod INT    
  ,DATE DATETIME    
  ,eSignalSymbol VARCHAR(max)    
  ,FundID INT    
  )    
    
 DECLARE @FromDate VARCHAR(max)    
 SET @FromDate = (SELECT TOP 1 AUECLocalDate FROM #PositionTable ORDER BY AUECLocalDate ASC)    
      
 INSERT INTO #FXConversionRatesDayMark    
 EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @FromDate    
  ,@InputDate    
    
 UPDATE #FXConversionRatesDayMark    
 SET RateValue = 1.0 / RateValue    
 WHERE RateValue <> 0    
  AND ConversionMethod = 1    
    
 UPDATE #FXConversionRatesDayMark    
 SET RateValue = 0    
 WHERE RateValue IS NULL    
    
 --select *from #FXConversionRatesDayMark    
    
 UPDATE #PositionTable    
 SET Multiplier = Multiplier    
 WHERE AssetID = 8    
    
 UPDATE #PositionTable    
 SET FXRate = 1.0 / FXRate    
 WHERE FXRate <> 0    
  AND FXConversionMethodOperator = 'D'    
    
 Select date,symbol,FinalMarkPrice,fundid into #PM_DayMarkPrice_Temp     
 FROM PM_DayMarkPrice AS MarkP    
 Where MarkP.Date >=(SELECT TOP 1 AUECLocalDate FROM #PositionTable ORDER BY AUECLocalDate ASC)    
    
 --select *from #PM_DayMarkPrice_Temp    
     
    
 SELECT    
  -- #PositionTable.SideID      
                                                                
  #PositionTable.Symbol    
  ,CONVERT(DECIMAL(22, 8), (#PositionTable.Quantity * dbo.GetSideMultiplier(#PositionTable.SideID))) AS Quantity    
  ,CONVERT(DECIMAL(22, 8), #PositionTable.AvgPx) AS AvgPX    
  ,CONVERT(DECIMAL(16, 9), IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0)) AS MarkPrice    
  ,CONVERT(DECIMAL(22, 8), IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) * DayEndFxRate.Val) AS MarkPriceBase    
  ,CONVERT(DECIMAL(22, 8), MarketValue.Val) AS MarketValue    
  ,CONVERT(DECIMAL(22, 8), MarketValue.Val * DayEndFxRate.Val) AS MarketValueBase    
  ,#PositionTable.Multiplier AS Multiplier    
  ,CUR.CurrencySymbol    
  ,LEFT(CONVERT(VARCHAR, #PositionTable.OriginalPurchaseDate, 101), 10) AS OriginalPurchaseDate    
  ,LEFT(CONVERT(VARCHAR, #PositionTable.AUECLocalDate, 101), 10) AS TradeDate    
  ,LEFT(CONVERT(VARCHAR, #PositionTable.ProcessDate, 101), 10) AS ProcessDate    
  ,LEFT(CONVERT(VARCHAR, Isnull(#PositionTable.ExpirationDate, '1/1/1800'), 101), 10) ExpirationDate    
  ,#PositionTable.PutOrCall    
  ,CONVERT(DECIMAL(22, 8), FXRate.Val) AS TradeFXRate    
  ,#PositionTable.StrikePrice    
  ,#PositionTable.CompanyName AS CompanyName    
  ,#PositionTable.UnderlyingSymbol    
  ,TC.FundShortName AS FundName    
  ,#PositionTable.IDCO AS IDCO    
  ,#PositionTable.SEDOL AS SEDOL    
  ,#PositionTable.OSI AS OSI    
  ,#PositionTable.CUSIP AS CUSIP    
  ,#PositionTable.Bloomberg AS Bloomberg    
  ,#PositionTable.ISINSymbol AS ISINSymbol    
  ,#PositionTable.ReutersSymbol AS ReutersSymbol    
  ,T_Side.Side    
  ,CASE     
   WHEN #PositionTable.IsSwapped = 1    
    THEN 'EquitySwap'    
   ELSE T_Asset.AssetName    
   END AS Asset    
  ,CP.ShortName AS CounterParty    
  ,0 AS GrossNotionalValue    
  ,0 AS GrossNotionalValueBase    
  ,CONVERT(DECIMAL(22, 8), NetNotionalValue.Val) AS NetNotionalValue    
  ,CONVERT(DECIMAL(22,8),NetNotionalValue.Val * FXRateForNetNotional.Val) AS NetNotionalValueBase    
  ,#PositionTable.TransactionType    
  ,CONVERT(DECIMAL(22, 8), UnitCost.Val) AS UnitCost    
  ,CUR1.CurrencySymbol AS BaseCurrency    
  ,#PositionTable.SettlCurrency      
  ,CONVERT(DECIMAL(22,8),UnitCost.Val * FXRateForNetNotional.Val) AS AvgPXBase    
  ,CONVERT(DECIMAL(22, 8), DayEndFxRate.Val) AS FXRate ,  
  StratergyName  
 FROM #PositionTable    
 INNER JOIN T_CompanyFunds AS TC ON TC.CompanyFundID = #PositionTable.FundID    
 LEFT OUTER JOIN T_Currency CUR2 ON CUR2.CurrencySymbol = #PositionTable.SettlCurrency    
 LEFT OUTER JOIN #PM_DayMarkPrice_Temp AS MP ON (    
   MP.Symbol = #PositionTable.Symbol    
   AND MP.FundID = #PositionTable.FundID    
   AND DateDiff(d, MP.DATE, @InputDate) = 0    
   )    
 LEFT OUTER JOIN #PM_DayMarkPrice_Temp AS MP1 ON (    
   MP1.Symbol = #PositionTable.Symbol    
   AND MP1.FundID = 0    
   AND DateDiff(d, MP1.DATE, @InputDate) = 0    
   )    
      
 LEFT OUTER JOIN #FXConversionRatesDayMark AS NetNotionalFX ON (    
   NetNotionalFX.FromCurrencyID = #PositionTable.CurrencyID    
   AND NetNotionalFX.ToCurrencyID = TC.LocalCurrency    
   AND NetNotionalFX.FundID = #PositionTable.FundID    
   AND DATEDIFF(d, NetNotionalFX.[DATE], #PositionTable.AUECLocalDate) = 0    
   )    
 LEFT OUTER JOIN #FXConversionRatesDayMark AS FX ON (    
   FX.FromCurrencyID = #PositionTable.CurrencyID    
   AND FX.ToCurrencyID = TC.LocalCurrency    
   AND FX.FundID = #PositionTable.FundID    
   AND DATEDIFF(d, FX.[DATE], @InputDate) = 0    
   )    
 LEFT OUTER JOIN #FXConversionRatesDayMark AS FX1 ON (    
   FX1.FromCurrencyID = #PositionTable.CurrencyID    
   AND FX1.ToCurrencyID = TC.LocalCurrency    
   AND FX1.FundID = 0    
   AND DATEDIFF(day, FX1.[DATE], @InputDate) = 0    
   )    
 --LEFT OUTER JOIN #FXConversionRatesDayMark AS SettlFX ON (    
 --  SettlFX.FromCurrencyID = #PositionTable.CurrencyID    
 --  AND SettlFX.ToCurrencyID = CUR2.CurrencyID    
 --  AND SettlFX.FundID = #PositionTable.FundID    
 --  AND DATEDIFF(day, SettlFX.[DATE], @InputDate) = 0    
 --  )    
 --LEFT OUTER JOIN #FXConversionRatesDayMark AS SettlFX1 ON (    
 --  SettlFX1.FromCurrencyID = #PositionTable.CurrencyID    
 --  AND SettlFX1.ToCurrencyID = CUR2.CurrencyID    
 --  AND SettlFX1.FundID = 0    
 --  AND DATEDIFF(day, SettlFX1.[DATE], @InputDate) = 0    
 --  )    
    
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = #PositionTable.FundID    
 LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID    
 LEFT OUTER JOIN T_ThirdParty TTP ON TTP.ThirdPartyID = TC.CompanyThirdPartyID    
 INNER JOIN T_Side ON T_Side.SideTagValue = #PositionTable.SideID    
 LEFT OUTER JOIN T_CounterParty CP ON CP.CounterPartyID = #PositionTable.CounterPartyID    
 INNER JOIN T_Asset ON T_Asset.AssetID = #PositionTable.AssetID    
 INNER JOIN T_Currency CUR ON CUR.CurrencyID = #PositionTable.CurrencyID    
 LEFT OUTER JOIN T_Currency CUR1 ON CUR1.CurrencyID = TC.LocalCurrency    
 --LEFT OUTER JOIN #FXConversionRatesDayMark AS FX3 ON (    
 --  FX3.FromCurrencyID = #PositionTable.LeadCurrencyID    
 --  AND FX3.ToCurrencyID = #PositionTable.VsCurrencyID    
 --  AND FX3.FundID = #PositionTable.FundID    
 --  AND DATEDIFF(d, FX.[DATE], @InputDate) = 0    
 --  )    
 --LEFT OUTER JOIN #FXConversionRatesDayMark AS FX4 ON (    
 --  FX4.FromCurrencyID = #PositionTable.LeadCurrencyID    
 --  AND FX4.ToCurrencyID = #PositionTable.VsCurrencyID    
 --  AND FX4.FundID = 0    
 --  AND DATEDIFF(d, FX.[DATE], @InputDate) = 0    
 --  )    
 CROSS APPLY (    
  SELECT CASE     
    WHEN (    
      (#PositionTable.IsSwapped = 1)    
      OR (    
       #PositionTable.AssetID IN (    
        3    
        ,5    
        ,11    
        )    
       )    
      )    
     THEN IsNull((#PositionTable.Quantity * (IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) - IsNull(#PositionTable.AvgPX, 0)) * #PositionTable.Multiplier * dbo.GetSideMultiplier(#PositionTable.SideID)), 0)    
    ELSE IsNull((#PositionTable.Quantity * IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) * #PositionTable.Multiplier * dbo.GetSideMultiplier(#PositionTable.SideID)), 0)    
    END    
  ) AS MarketValue(Val)    
 CROSS APPLY (    
  SELECT IsNull((#PositionTable.Quantity * IsNull(#PositionTable.AvgPx, 0) * #PositionTable.Multiplier * dbo.GetSideMultiplier(#PositionTable.SideID) + #PositionTable.TotalCommissionandFees), 0)    
  ) AS NetNotionalValue(Val)    
 CROSS APPLY (    
  SELECT CASE     
    WHEN #PositionTable.CurrencyID = TC.LocalCurrency    
     THEN 1    
    ELSE CASE     
      WHEN #PositionTable.FXRate = 0    
       THEN IsNull(coalesce(NetNotionalFX.RateValue, FX.RateValue, FX1.RateValue), 0)    
      ELSE IsNull(coalesce(#PositionTable.FXRate, NetNotionalFX.RateValue, FX.RateValue, FX1.RateValue), 0)    
      END    
    END    
  ) AS FXRate(Val)    
 CROSS APPLY (    
  SELECT     
  --IsNull(coalesce(NetNotionalFX.RateValue, #PositionTable.FXRate, FX.RateValue, FX1.RateValue), 0)    
  CASE     
    WHEN #PositionTable.CurrencyID = TC.LocalCurrency    
     THEN 1    
    ELSE CASE     
      WHEN #PositionTable.FXRate = 0    
       THEN IsNull(coalesce( NetNotionalFX.RateValue, FX.RateValue, FX1.RateValue), 0)    
      ELSE IsNull(coalesce(#PositionTable.FXRate, NetNotionalFX.RateValue, FX.RateValue, FX1.RateValue), 0)    
      END    
    END    
  ) AS FXRateForNetNotional(Val)    
  --CROSS APPLY (                                        
  -- SELECT CASE                        
  --   WHEN #PositionTable.CurrencyID = TC.LocalCurrency                              
  --    THEN 1                              
  --      ELSE                                   
  --   CASE                                       
  --  WHEN #PositionTable.FXRate = 0                                  
  --  THEN IsNull(coalesce(FX.RateValue, FX1.RateValue), 0)                                  
  -- ELSE                                  
  --  IsNull(coalesce(#PositionTable.FXRate, FX.RateValue, FX1.RateValue), 0)                                          
  -- end                       
  --   END                              
                             
  -- ) AS FXRateDailyVal(Val)                     
 CROSS APPLY (    
  SELECT CASE     
    WHEN #PositionTable.CurrencyID = TC.LocalCurrency    
     THEN 1    
    ELSE IsNull(coalesce(FX.RateValue, FX1.RateValue), 0)    
    END    
  ) AS DayEndFxRate(Val)    
 CROSS APPLY (    
  SELECT CASE     
    WHEN (    
      #PositionTable.Quantity = 0    
      OR #PositionTable.Multiplier = 0    
      )    
     THEN 0    
    WHEN (    
      #PositionTable.SideID = '1'    
      OR #PositionTable.SideID = '3'    
      OR #PositionTable.SideID = 'A'    
      OR #PositionTable.SideID = 'B'    
      OR #PositionTable.SideID = 'E'    
      )    
     THEN (IsNull((#PositionTable.Quantity * IsNull(#PositionTable.AvgPx, 0) * #PositionTable.Multiplier + #PositionTable.TotalCommissionandFees), 0)) / (#PositionTable.Quantity * #PositionTable.Multiplier)    
    ELSE (IsNull((#PositionTable.Quantity * IsNull(#PositionTable.AvgPx, 0) * #PositionTable.Multiplier - #PositionTable.TotalCommissionandFees), 0)) / (#PositionTable.Quantity * #PositionTable.Multiplier)    
    END    
  ) AS UnitCost(Val)    
 --CROSS APPLY (    
 -- SELECT CASE     
 --   WHEN #PositionTable.SettlCurrency <> CUR.CurrencySymbol    
 --    THEN IsNull(coalesce(SettlFX.RateValue, SettlFX1.RateValue), 0)    
 --   ELSE 1    
 --   END    
 -- ) AS EODSettlFxRate(Val)    
    
 DROP TABLE #PositionTable    
  ,#FXConversionRatesDayMark    
  ,#TempTaxlotPK    
  ,#PM_DayMarkPrice_Temp    
  --Select * from ##PositionTable                                                                                                  
END