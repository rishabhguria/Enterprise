/*                
Modified By : Ankit Gupta on 30 Oct, 2014        
Description : Fetch allocated quantity per taxlot in P_GetPositions.        
JIRA  : http://jira.nirvanasolutions.com:8080/browse/CHMW-1670        
        
Modified By : Narendra Kumar Jangir        
Description : Fund wise fx rate implementation.        
JIRA  : http://jira.nirvanasolutions.com:8080/browse/CHMW-2994        
*/  
CREATE PROCEDURE [dbo].[PMGetFundPositionsWithMarketValue_SS_MS] (  
 @FromAllAUECDatesString VARCHAR(MAX)  
 , @ToAllAUECDatesString VARCHAR(MAX)  
 , @AssetIds VARCHAR(MAX)  
 , @FundIds VARCHAR(MAX)  
 --@symbol varchar(50)                                                                                                                
 )  
AS  
BEGIN  
 CREATE TABLE #PositionTable (  
  TaxLotID VARCHAR(50)  
  , AUECLocalDate DATETIME  
  , SideID CHAR(1)  
  , Symbol VARCHAR(200)  
  , Quantity FLOAT  
  , AvgPX FLOAT  
  , FundID INT  
  , AssetID INT  
  , UnderLyingID INT  
  , ExchangeID INT  
  , CurrencyID INT  
  , AUECID INT  
  , TotalCommissionandFees FLOAT  
  , Multiplier FLOAT  
  , SettlementDate DATETIME  
  , LeadCurrencyID INT  
  , VsCurrencyID INT  
  , ExpirationDate DATETIME  
  , Description VARCHAR(max)  
  , Level2ID INT  
  , NotionalValue FLOAT  
  , BenchMarkRate FLOAT  
  , Differential FLOAT  
  , OrigCostBasis FLOAT  
  , DayCount INT  
  , SwapDescription VARCHAR(max)  
  , FirstResetDate DATETIME  
  , OrigTransDate DATETIME  
  , IsSwapped BIT  
  , AllocationDate DATETIME  
  , GroupID VARCHAR(50)  
  , PositionTag INT  
  , FXRate FLOAT  
  , FXConversionMethodOperator VARCHAR(5)  
  , CompanyName VARCHAR(500)  
  , UnderlyingSymbol VARCHAR(50)  
  , Delta FLOAT  
  , PutOrCall VARCHAR(5)  
  , IsGrPreAllocated BIT  
  , GrCumQty FLOAT  
  , GrAllocatedQty FLOAT  
  , GrQuantity FLOAT  
  , Taxlot_Pk BIGINT  
  , ParentRow_Pk BIGINT  
  , StrikePrice FLOAT  
  , UserID INT  
  , CounterPartyID INT  
  , CorpActionID UNIQUEIDENTIFIER  
  , Coupon FLOAT  
  , IssueDate DATETIME  
  , MaturityDate DATETIME  
  , FirstCouponDate DATETIME  
  , CouponFrequencyID INT  
  , AccrualBasisID INT  
  , BondTypeID INT  
  , IsZero BIT  
  , ProcessDate DATETIME  
  , OriginalPurchaseDate DATETIME  
  , IsNDF BIT  
  , FixingDate DATETIME  
  , IDCO VARCHAR(50)  
  , OSI VARCHAR(50)  
  , SEDOL VARCHAR(50)  
  , CUSIP VARCHAR(50)  
  , Bloomberg VARCHAR(50)  
  , MasterFund VARCHAR(100)  
  , UnderlyingDelta FLOAT  
  , ISINSymbol VARCHAR(50)  
  , LotId VARCHAR(200)  
  , ExternalTransId VARCHAR(100)  
  , TradeAttribute1 VARCHAR(200)  
  , TradeAttribute2 VARCHAR(200)  
  , TradeAttribute3 VARCHAR(200)  
  , TradeAttribute4 VARCHAR(200)  
  , TradeAttribute5 VARCHAR(200)  
  , TradeAttribute6 VARCHAR(200)  
  , ProxySymbol VARCHAR(100)  
  ,  
  --Added UDA columns ,by Omshiv, Nov, 2013                            
  AssetName VARCHAR(100)  
  , SecurityTypeName VARCHAR(200)  
  , SectorName VARCHAR(100)  
  , SubSectorName VARCHAR(100)  
  , CountryName VARCHAR(100)  
  , BBGID VARCHAR(20)  
  , TransactionType VARCHAR(200)  
  , ExecutedQty FLOAT  
  , ClosingTaxlotId VARCHAR(50)  
  , ReutersSymbol VARCHAR(50)  
  , InternalComments VARCHAR(500)  
  , SettlCurrency VARCHAR(4)  
  , SettlCurrFxRate FLOAT  
  , SettlCurrAmt FLOAT  
  , SettlCurrFxRateCalc VARCHAR(10)  
  , IsCurrencyFuture BIT  
  , Symbol_PK BIGINT  
  , FactSetSymbol VARCHAR(100)
  , ActivSymbol VARCHAR(100)
  ,BloombergSymbolWithExchangeCode VARCHAR(200)
  )  
 INSERT INTO #PositionTable  
 EXEC P_GetPositions @ToAllAUECDatesString  
  , @AssetIds  
  , @FundIds  
  , NULL  
 --Declare @baseCurrencyID int                                                                   
 --Set @baseCurrencyID= (Select BaseCurrencyID from T_Company)                                                   
 DECLARE @AUECDatesTable TABLE (  
  AUECID INT  
  , CurrentAUECDate DATETIME  
  )  
 INSERT INTO @AUECDatesTable  
 SELECT *  
 FROM dbo.GetAllAUECDatesFromString(@ToAllAUECDatesString)  
 -- variable to hold CURRENT DATE                                                                            
 DECLARE @UTCDate DATETIME  
 SELECT @UTCDate = CurrentAUECDate  
 FROM @AUECDatesTable  
 WHERE AUECID = 0  
 CREATE TABLE #FXConversionRatesDayMark (  
  FromCurrencyID INT  
  , ToCurrencyID INT  
  , RateValue FLOAT  
  , ConversionMethod INT  
  , DATE DATETIME  
  , eSignalSymbol VARCHAR(max)  
  , FundID INT  
  )  
 INSERT INTO #FXConversionRatesDayMark  
 EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @UTCDate  
  , @UTCDate  
 UPDATE #FXConversionRatesDayMark  
 SET RateValue = 1.0 / RateValue  
 WHERE RateValue <> 0  
  AND ConversionMethod = 1  
 UPDATE #FXConversionRatesDayMark  
 SET RateValue = 0  
 WHERE RateValue IS NULL  
 UPDATE #PositionTable  
 SET Multiplier = Multiplier  
 WHERE AssetID = 8  
 SELECT #PositionTable.SideID  
  , T_Side.Side  
  , #PositionTable.Symbol  
  ,  
  --TODO:                                            
  --#PositionTable.OrigTransDate as TradeDate,                                                         
  (#PositionTable.Quantity * dbo.GetSideMultiplier(#PositionTable.SideID)) AS Quantity  
  , IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) AS MarkPrice  
  , IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) * FXRate.Val AS MarkPriceBase  
  , TC.FundShortName AS AccountName  
  ,  
  -- Changed By Nishant Jain [2015-02-20]                                                    
  #PositionTable.CompanyName AS CompanyName  
  , MarketValue.Val AS MarketValue  
  , MarketValue.Val * FXRate.Val AS MarketValueBase  
  , NetNotionalValue.Val AS NetNotionalValue  
  , NetNotionalValue.Val * FXRate.Val AS NetNotionalValueBase  
  , #PositionTable.Multiplier AS Multiplier  
  , #PositionTable.IDCO AS IDCO  
  , #PositionTable.SEDOL AS SEDOL  
  , #PositionTable.CUSIP AS CUSIP  
  , #PositionTable.OSI AS OSI  
  , #PositionTable.Bloomberg AS Bloomberg  
  , #PositionTable.StrikePrice  
  , FXRate.Val AS FXRate  
  , CONVERT(VARCHAR(10), #PositionTable.AUECLocalDate, 101) AS TradeDate  
  ,  
  --#PositionTable.AUECLocalDate as TradeDate,                                                                
  #PositionTable.ExpirationDate  
  , #PositionTable.PutOrCall  
  , #PositionTable.AvgPx AS AvgPX  
  , #PositionTable.UnderlyingSymbol  
  , #PositionTable.CounterPartyID  
  , #PositionTable.AssetID  
  , T_Asset.AssetName AS Asset  
  , CP.ShortName AS CounterParty  
  , #PositionTable.AUECID  
  , #PositionTable.LotID AS LotID  
  , IsNull(MF.MasterFundName, TC.FundShortName) AS MasterFund  
  , -- Changed By Nishant Jain [2015-02-20]                                   
  TTP.ShortName AS PrimeBroker  
  , CUR.CurrencySymbol  
  , #PositionTable.ExternalTransId AS ExternalTransId  
  , UnitCost.Val AS UnitCost  
  , #PositionTable.ISINSymbol AS ISINSymbol  
  , #PositionTable.BBGID AS BBGID  
  , #PositionTable.ReutersSymbol AS ReutersSymbol  
  , #PositionTable.IsSwapped  
  , #PositionTable.TaxLotID  
  , dbo.GetSideMultiplier(#PositionTable.SideID) AS SideMultiplier  
  , CUR1.CurrencySymbol AS BaseCurrency  
  , #PositionTable.SettlCurrency  
  , IsNull(#PositionTable.SettlCurrFxRate, 0) AS SettlCurrFxRate  
  , IsNull(#PositionTable.SettlCurrAmt, 0) AS SettlCurrAmt  
  , #PositionTable.SettlCurrFxRateCalc  
  , EODSettlFxRate.Val * IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) AS SettlementCurrencyMarkPrice  
  , SettlCurrFxRate1.Val * UnitCost.Val AS SettlementCurrencyCostBasis  
  , EODSettlFxRate.Val * MarketValue.Val AS SettlementCurrencyMarketValue  
  , SettlCurrFxRate1.Val * NetNotionalValue.Val AS SettlementCurrencyTotalCost  
  , #PositionTable.ProcessDate  
  , #PositionTable.OriginalPurchaseDate  
  , IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) AS MarkedFxrate  
  , (IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) - IsNull(coalesce(FX3.RateValue, FX4.RateValue), 0)) AS ForwardPoint  
  , CASE   
   WHEN (  
     #PositionTable.AssetID = 11  
     OR #PositionTable.AssetID = 5  
     )  
    THEN CASE   
      WHEN #PositionTable.LeadCurrencyID = TC.LocalCurrency  
       THEN IsNull((#PositionTable.Quantity * IsNull(#PositionTable.AvgPx, 0) * dbo.GetSideMultiplier(#PositionTable.SideID) * (- 1)), 0)  
      ELSE IsNull(#PositionTable.Quantity * dbo.GetSideMultiplier(#PositionTable.SideID), 0)  
      END  
   ELSE 0  
   END AS LocalAmount  
  , CASE   
   WHEN (  
     #PositionTable.AssetID = 11  
     OR #PositionTable.AssetID = 5  
     )  
    THEN CASE   
      WHEN #PositionTable.LeadCurrencyID = TC.LocalCurrency  
       THEN IsNull((#PositionTable.Quantity * IsNull(#PositionTable.AvgPx, 0) * dbo.GetSideMultiplier(#PositionTable.SideID) * (- 1)), 0) * IsNull(coalesce(FX3.RateValue, FX4.RateValue), 0)  
      ELSE IsNull(#PositionTable.Quantity * dbo.GetSideMultiplier(#PositionTable.SideID), 0) * IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0)  
      END  
   ELSE 0  
   END AS BaseAmount,
   #PositionTable.BloombergSymbolWithExchangeCode  
 into #tempData  
 FROM #PositionTable  
 INNER JOIN T_CompanyFunds AS TC ON TC.CompanyFundID = #PositionTable.FundID  
 LEFT OUTER JOIN T_Currency CUR2 ON CUR2.CurrencySymbol = #PositionTable.SettlCurrency  
 LEFT OUTER JOIN PM_DayMarkPrice AS MP ON (  
   MP.Symbol = #PositionTable.Symbol  
   AND DateDiff(d, MP.DATE, @UTCDate) = 0  
   AND MP.FundID = #PositionTable.FundID  
   )  
 LEFT OUTER JOIN PM_DayMarkPrice AS MP1 ON (  
   MP1.Symbol = #PositionTable.Symbol  
   AND DateDiff(d, MP1.DATE, @UTCDate) = 0  
   AND MP1.FundID = 0  
   )  
 LEFT OUTER JOIN #FXConversionRatesDayMark AS FX ON (  
   FX.FromCurrencyID = #PositionTable.CurrencyID  
   AND FX.ToCurrencyID = TC.LocalCurrency  
   AND FX.FundID = #PositionTable.FundID  
   )  
 LEFT OUTER JOIN #FXConversionRatesDayMark AS FX1 ON (  
   FX1.FromCurrencyID = #PositionTable.CurrencyID  
   AND FX1.ToCurrencyID = TC.LocalCurrency  
   AND FX1.FundID = 0  
   )  
 LEFT OUTER JOIN #FXConversionRatesDayMark AS SettlFX ON (  
   SettlFX.FromCurrencyID = #PositionTable.CurrencyID  
   AND SettlFX.ToCurrencyID = CUR2.CurrencyID  
   AND SettlFX.FundID = #PositionTable.FundID  
   )  
 LEFT OUTER JOIN #FXConversionRatesDayMark AS SettlFX1 ON (  
   SettlFX1.FromCurrencyID = #PositionTable.CurrencyID  
   AND SettlFX1.ToCurrencyID = CUR2.CurrencyID  
   AND SettlFX1.FundID = 0  
   )  
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = #PositionTable.FundID  
 LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID  
 LEFT OUTER JOIN T_ThirdParty TTP ON TTP.ThirdPartyID = TC.CompanyThirdPartyID  
 INNER JOIN T_Side ON T_Side.SideTagValue = #PositionTable.SideID  
 LEFT OUTER JOIN T_CounterParty CP ON CP.CounterPartyID = #PositionTable.CounterPartyID  
 INNER JOIN T_Asset ON T_Asset.AssetID = #PositionTable.AssetID  
 INNER JOIN T_Currency CUR ON CUR.CurrencyID = #PositionTable.CurrencyID  
 LEFT OUTER JOIN T_Currency CUR1 ON CUR1.CurrencyID = TC.LocalCurrency  
 LEFT OUTER JOIN #FXConversionRatesDayMark AS FX3 ON (  
   FX3.FromCurrencyID = #PositionTable.LeadCurrencyID  
   AND FX3.ToCurrencyID = #PositionTable.VsCurrencyID  
   AND FX3.FundID = #PositionTable.FundID  
   )  
 LEFT OUTER JOIN #FXConversionRatesDayMark AS FX4 ON (  
   FX4.FromCurrencyID = #PositionTable.LeadCurrencyID  
   AND FX4.ToCurrencyID = #PositionTable.VsCurrencyID  
   AND FX4.FundID = 0  
   )  
 CROSS APPLY (  
  SELECT CASE   
    WHEN (  
      (#PositionTable.IsSwapped = 1)  
      OR (#PositionTable.AssetID IN (3, 5, 11))  
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
    ELSE IsNull(coalesce(FX.RateValue, FX1.RateValue), 0)  
    END  
  ) AS FXRate(Val)  
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
 CROSS APPLY (  
  SELECT CASE   
    WHEN #PositionTable.SettlCurrency <> CUR.CurrencySymbol  
     THEN IsNull(coalesce(SettlFX.RateValue, SettlFX1.RateValue), 0)  
    ELSE 1  
    END  
  ) AS EODSettlFxRate(Val)  
 CROSS APPLY (  
  SELECT CASE   
    WHEN #PositionTable.SettlCurrFxRateCalc <> 'D'  
     THEN IsNull(#PositionTable.SettlCurrFxRate, 0)  
    ELSE COALESCE(1 / NULLIF(#PositionTable.SettlCurrFxRate, 0), 0)  
    END  
  ) AS SettlCurrFxRate1(Val)  
  
update #tempData  
set MasterFund = 'MainStay MAP Fund'  
where MasterFund = 'MainStay MAP Fund LCV'  
  
select * from #tempData  
 DROP TABLE #PositionTable  
  , #FXConversionRatesDayMark  
  --Select * from ##PositionTable                                                            
END  