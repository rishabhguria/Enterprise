          
/*                    
Modified By : Ankit Gupta on 30 Oct, 2014            
Description : Fetch allocated quantity per taxlot in P_GetPositions.            
JIRA  : http://jira.nirvanasolutions.com:8080/browse/CHMW-1670            
            
Modified By : Narendra Kumar Jangir            
Description : Fund wise fx rate implementation.            
JIRA  : http://jira.nirvanasolutions.com:8080/browse/CHMW-2994            
*/          
CREATE PROCEDURE [dbo].[PMGetFundPositionsWithMarketValue_newforSP1] (          
 @StartDate datetime      
 ,@AssetIds VARCHAR(MAX)        
 ,@FundIds VARCHAR(MAX)                                                                                                         
 )          
AS          
BEGIN          
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
  ,          
  --Added UDA columns ,by Omshiv, Nov, 2013                        
  AssetName VARCHAR(100)          
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
  --,SettlCurrFxRate FLOAT          
  --,SettlCurrAmt FLOAT          
  --,SettlCurrFxRateCalc VARCHAR(10)          
  )          
      
          
 INSERT INTO #PositionTable          
 EXEC [P_GetPositions_forSP1] @StartDate,@AssetIds,@FundIds          
       
          
      
                                                                      
          
 CREATE TABLE #FXConversionRatesDayMark (          
  FromCurrencyID INT          
  ,ToCurrencyID INT          ,RateValue FLOAT          
  ,ConversionMethod INT          
  ,DATE DATETIME          
  ,eSignalSymbol VARCHAR(max)          
  ,FundID INT          
  )          
          
 INSERT INTO #FXConversionRatesDayMark          
 EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @StartDate          
  ,@StartDate          
          
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
    
    
    
    
    
          
insert into temptable1    
SELECT       
--#PositionTable.SideID          
 -- ,T_Side.Side          
--  ,      
#PositionTable.fundid,      
#PositionTable.currencyid,      
#PositionTable.Symbol          
  ,(#PositionTable.Quantity * dbo.GetSideMultiplier(#PositionTable.SideID)) AS Quantity          
  ,IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) AS MarkPrice          
  ,CASE           
   WHEN #PositionTable.CurrencyID = TC.LocalCurrency          
    THEN IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0)          
   ELSE IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) * IsNull(coalesce(FX.RateValue, FX1.RateValue), 0)          
   END AS MarkPriceBase          
  ,TC.FundShortName AS FundName           
  ,CASE           
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
     --IsNull((#PositionTable.Quantity * IsNull(MP.FinalMarkPrice,0) * #PositionTable.Multiplier * dbo.GetSideMultiplier(#PositionTable.SideID) ),0) as MarketValue,                         
   ELSE IsNull((#PositionTable.Quantity * IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) * #PositionTable.Multiplier * dbo.GetSideMultiplier(#PositionTable.SideID)), 0)          
   END AS MarketValue          
  ,CASE           
   WHEN (#PositionTable.CurrencyID = TC.LocalCurrency)          
    THEN CASE           
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
   ELSE CASE           
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
      THEN IsNull((#PositionTable.Quantity * (IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) - IsNull(#PositionTable.AvgPX, 0)) * #PositionTable.Multiplier * dbo.GetSideMultiplier(#PositionTable.SideID)), 0) * IsNull(coalesce(FX.RateValue, FX1
  
.RateValue), 0)          
     ELSE IsNull((#PositionTable.Quantity * IsNull(coalesce(MP.FinalMarkPrice, MP1.FinalMarkPrice), 0) * #PositionTable.Multiplier * dbo.GetSideMultiplier(#PositionTable.SideID)), 0) * IsNull(coalesce(FX.RateValue, FX1.RateValue), 0)          
     END          
   END AS MarketValueBase          
        
  ,CASE           
   WHEN #PositionTable.FXRate = 0          
    THEN IsNull(coalesce(FX.RateValue, FX1.RateValue), 0)          
   ELSE IsNull(coalesce(#PositionTable.FXRate, FX.RateValue, FX1.RateValue), 0)          
   END AS FXRate          
       
    
 FROM #PositionTable          
 INNER JOIN T_CompanyFunds AS TC ON TC.CompanyFundID = #PositionTable.FundID          
 LEFT OUTER JOIN PM_DayMarkPrice AS MP ON (          
   MP.Symbol = #PositionTable.Symbol          
   AND DateDiff(d, MP.DATE, @StartDate) = 0          
   AND MP.FundID = #PositionTable.FundID          
   )          
 LEFT OUTER JOIN PM_DayMarkPrice AS MP1 ON (        
   MP1.Symbol = #PositionTable.Symbol          
   AND DateDiff(d, MP1.DATE, @StartDate) = 0          
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
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = #PositionTable.FundID          
 LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID          
 LEFT OUTER JOIN T_ThirdParty TTP ON TTP.ThirdPartyID = TC.CompanyThirdPartyID          
 INNER JOIN T_Side ON T_Side.SideTagValue = #PositionTable.SideID          
 LEFT OUTER JOIN T_CounterParty CP ON CP.CounterPartyID = #PositionTable.CounterPartyID          
 INNER JOIN T_Asset ON T_Asset.AssetID = #PositionTable.AssetID          
 INNER JOIN T_Currency CUR ON CUR.CurrencyID = #PositionTable.CurrencyID          
 LEFT OUTER JOIN T_Currency CUR1 ON CUR1.CurrencyID = TC.LocalCurrency          
          
 DROP TABLE #PositionTable          
  ,#FXConversionRatesDayMark          
  --Select * from ##PositionTable                                                        
END 