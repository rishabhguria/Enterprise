CREATE PROCEDURE [dbo].[P_FFGetThirdPartyLevel2_KLDSExportOnly]     
(      
 @thirdPartyID INT      
 ,@companyFundIDs VARCHAR(max)      
 ,@inputDate DATETIME      
 ,@companyID INT      
 ,@auecIDs VARCHAR(max)      
 ,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties      
 ,@dateType INT -- 0 for Process Date and 1 for Trade Date                                                                                                                       
 ,@fileFormatID INT      
 ,@includeSent BIT = 1      
 )      
AS    
    
--Declare @ThirdPartyID INT    
-- ,@companyFundIDs VARCHAR(max)      
-- ,@inputDate DATETIME      
-- ,@companyID INT      
-- ,@auecIDs VARCHAR(max)      
-- ,@TypeID INT    
-- ,@dateType INT    
-- ,@fileFormatID INT      
-- ,@includeSent BIT     
    
--Set @thirdPartyID=34    
--Set @companyFundIDs=N'1,14,10,12,13,15,8,9,11,2,7,5,3,6,4'    
--Set @inputDate='2019-02-05 08:01:15:000'    
--Set @companyID=7    
--Set @auecIDs=N'11,153,105,150,106,12,80,22,140,141,90,142,139,91,133,97,94,129,131,92,101,138,136,26,24,23,144,98,86,122,114,16,17,85,87,147,120,145,116,89,84,100,117,107,113,19,109,110,32'    
--Set @TypeID=1    
--Set @dateType=0    
--Set @fileFormatID=96    
--Set @includeSent=0      
    
    
DECLARE @Fund TABLE (FundID INT)      
DECLARE @AUECID TABLE (AUECID INT)      
DECLARE @IncludeExpiredSettledTransaction INT      
DECLARE @IncludeExpiredSettledUnderlyingTransaction INT      
DECLARE @IncludeCATransaction INT      
DECLARE @IsFillsRequired BIT      
      
SET @IncludeExpiredSettledTransaction = (      
  SELECT IncludeExercisedAssignedTransaction      
  FROM T_ThirdPartyFileFormat      
  WHERE FileFormatId = @fileFormatID      
  )      
SET @IncludeExpiredSettledUnderlyingTransaction = (      
  SELECT IncludeExercisedAssignedUnderlyingTransaction      
  FROM T_ThirdPartyFileFormat      
  WHERE FileFormatId = @fileFormatID      
  )      
SET @IncludeCATransaction = (      
  SELECT IncludeCATransaction      
  FROM T_ThirdPartyFileFormat      
  WHERE FileFormatId = @fileFormatID      
  )      
      
SET @IsFillsRequired = (      
  SELECT IsFillsRequired      
  FROM T_ThirdPartyFileFormat      
  WHERE FileFormatId = @fileFormatID      
  )      
      
DECLARE @FXForwardAuecID INT      
SET @FXForwardAuecID = (      
  SELECT TOP 1 Auecid      
  FROM T_AUEC      
  WHERE assetid = 11      
  )      
      
INSERT INTO @Fund      
SELECT Cast(Items AS INT)      
FROM dbo.Split(@companyFundIDs, ',')      
      
INSERT INTO @AUECID      
SELECT Cast(Items AS INT)      
FROM dbo.Split(@auecIDs, ',')    
  
CREATE TABLE #FXConversionRatesForTradeDate   
(  
 FromCurrencyID INT  
 ,ToCurrencyID INT  
 ,RateValue FLOAT  
 ,ConversionMethod INT  
 ,DATE DATETIME  
 ,eSignalSymbol VARCHAR(max)  
 ,FundID INT  
)  
  
INSERT INTO #FXConversionRatesForTradeDate  
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @inputDate ,@inputDate  
  
--UPDATE #FXConversionRatesForTradeDate  
--SET RateValue = 1.0 / RateValue  
--WHERE RateValue <> 0  
--AND ConversionMethod = 1  
  
UPDATE #FXConversionRatesForTradeDate  
SET RateValue = 0  
WHERE RateValue IS NULL  
    
      
SELECT DISTINCT     
VT.TaxLotID      
 ,VT.FundID AS FundID     
 ,CF.FundName As AccountName     
 ,VT.OrderSideTagValue AS SideID      
 ,T_Side.Side AS Side      
 ,VT.Symbol      
 ,VT.TaxLotQty AS OrderQty      
--,CASE      
-- When sm.underlyingsymbol like '%6S%' or sm.underlyingsymbol like '%6L%'  or     
--  sm.underlyingsymbol like '%6C%' or sm.underlyingsymbol like '%6J%' or     
--  sm.underlyingsymbol like '%6B%' or sm.underlyingsymbol like '%6A%' or     
--  sm.underlyingsymbol like '%6M%' or sm.underlyingsymbol like '%6N%' or     
--  sm.underlyingsymbol like '%6R%'  or sm.underlyingsymbol like '%6Z%' or     
--  sm.underlyingsymbol like '%HG%' or sm.underlyingsymbol like '%HO%'  or     
--  sm.underlyingsymbol like '%XRB%' or sm.underlyingsymbol like '%SF%'         
-- Then Vt.AVGPRice * 100      
--Else VT.AvgPrice      
--END as AveragePrice,  
  
,CASE      
 When Substring(SM.UnderlyingSymbol,0,CharIndex(' ',SM.UnderlyingSymbol))   
 In ('6L','6C','6J','6S','6B','6A','6M','6N','6R','6Z','HG','HO','ZR','XRB','SF')  
 Then VT.AVGPRice * 100      
Else VT.AvgPrice      
END as AveragePrice  
   
,CASE     
 WHEN T_Asset.AssetId = 8    
 THEN     
  CASE     
   WHEN VT.CurrencyID <> CF.LocalCurrency    
   THEN     
    CASE     
     WHEN IsNull(VT.FXRate, 0) <> 0    
     THEN     
      CASE     
       WHEN VT.FXConversionMethodOperator = 'M'    
       THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + (VT.AccruedInterest + (VT.SideMultiplier * VT.TotalExpenses))) * IsNull(VT.FXRate, 0)    
       WHEN VT.FXConversionMethodOperator = 'D' AND VT.FXRate > 0    
       THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + (VT.AccruedInterest + (VT.SideMultiplier * VT.TotalExpenses))) * 1 / VT.FXRate    
      END    
     ELSE IsNull((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + (VT.AccruedInterest + (VT.SideMultiplier * VT.TotalExpenses))),0)-- * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)    
    END    
   ELSE (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + (VT.AccruedInterest + (VT.SideMultiplier * VT.TotalExpenses)))    
  END    
ELSE     
 CASE     
  WHEN VT.CurrencyID <> CF.LocalCurrency    
  THEN     
   CASE     
    WHEN IsNull(VT.FXRate, 0) <> 0    
    THEN     
     CASE     
      WHEN VT.FXConversionMethodOperator = 'M'    
      THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses)* IsNull(VT.FXRate, 0)    
      WHEN VT.FXConversionMethodOperator = 'D' AND VT.FXRate > 0    
      THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * 1 / VT.FXRate    
     END    
    ELSE IsNull((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses),0)-- * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)    
   END    
  ELSE (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses)    
 END    
END AS NetAmount,  
  
--CASE     
-- WHEN VT.CurrencyID <> CF.LocalCurrency    
-- THEN     
--  CASE     
--   WHEN IsNull(VT.FXRate, 0) <> 0    
--   THEN     
--    CASE     
--     WHEN VT.FXConversionMethodOperator = 'M'    
--     THEN IsNull(VT.FXRate, 0)    
--     WHEN VT.FXConversionMethodOperator = 'D' AND VT.FXRate > 0    
--     THEN 1 / VT.FXRate    
--    END    
--   ELSE IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0)    
--  END    
-- ELSE 0    
--END As ForexRate       
  
CASE     
 WHEN VT.CurrencyID <> CF.LocalCurrency    
 THEN     
  CASE     
   WHEN IsNull(VT.FXRate, 0) <> 0    
   THEN VT.FXRate  
   ELSE IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0)    
  END    
 ELSE 1   
END As ForexRate   
  
,T_Asset.AssetName as Asset    
    
 ,VT.CumQty      
 ,VT.Quantity      
 ,Currency.CurrencySymbol      
-- ,CTPM.MappedName      
-- ,CTPM.FundAccntNo       
  ,VT.TaxLotID      
 ,VT.TaxLotQty As AllocatedQty    
 ,SM.PutOrCall      
 ,SM.StrikePrice      
,CONVERT(VARCHAR(10),SM.ExpirationDate , 101) As ExpirationDate     
,CONVERT(VARCHAR(10),VT.SettlementDate , 101) As SettlementDate      
 ,VT.Commission As CommissionCharged     
 ,VT.OtherBrokerFees As OtherBrokerFee    
 ,VT.GroupRefID      
 ,0 AS TaxLotState      
 ,IsNull(VT.StampDuty, 0) As StampDuty      
 ,IsNull(VT.TransactionLevy, 0) As TransactionLevy     
 ,IsNull(VT.ClearingFee, 0) As ClearingFee      
 ,IsNull(VT.TaxOnCommissions, 0) As TaxOnCommissions    
 ,IsNull(VT.MiscFees, 0) As MiscFees      
 ,CONVERT(VARCHAR(10), VT.AUECLocalDate, 101) As TradeDate    
 ,SM.Multiplier As AssetMultiplier    
 ,VT.Level2ID      
 ,SM.ISINSymbol      
 ,SM.CUSIPSymbol As CUSIP     
 ,SM.SEDOLSymbol      
 ,SM.BloombergSymbol As BBCode     
 ,SM.CompanyName As FullSecurityName     
 ,SM.UnderlyingSymbol      
 ,SM.OSISymbol      
 ,SM.IDCOSymbol      
 ,SM.OpraSymbol      
 ,CONVERT(VARCHAR(10), VT.ProcessDate, 101) As ProcessDate    
,CONVERT(VARCHAR(10), VT.OriginalPurchaseDate, 101) As OriginalPurchaseDate    
 ,VT.AccruedInterest      
, SM.Coupon      
 ,SM.IssueDate      
 ,SM.FirstCouponDate      
 ,SM.CouponFrequencyID      
 ,SM.AccrualBasisID      
 ,SM.BondTypeID      
 ,T_CompanyStrategy.StrategyName as Strategy     
 ,SM.AssetName AS UDAAssetName      
 ,SM.SecurityTypeName AS UDASecurityTypeName      
 ,SM.SectorName AS UDASectorName      
 ,SM.SubSectorName AS UDASubSectorName      
 ,SM.CountryName AS UDACountryName      
 ,IsNull(VT.SecFee, 0) As SecFee      
 ,IsNull(VT.OccFee, 0) As OccFee    
 ,IsNull(VT.OrfFee, 0) As OrfFee    
 ,VT.ClearingBrokerFee      
 ,VT.SoftCommission      
 ,VT.TransactionType      
 ,TC.CurrencySymbol AS SettlCurrency   
,T_CounterParty.ShortName As CounterParty       
 ----------Dynamic UDA---------------      
-- ,ISNULL(Analyst, '') AS Analyst      
-- ,ISNULL(CountryOfRisk, '') AS CountryOfRisk      
-- ,ISNULL(CustomUDA1, '') AS CustomUDA1      
-- ,ISNULL(CustomUDA2, '') AS CustomUDA2      
-- ,ISNULL(CustomUDA3, '') AS CustomUDA3      
-- ,ISNULL(CustomUDA4, '') AS CustomUDA4      
-- ,ISNULL(CustomUDA5, '') AS CustomUDA5      
-- ,ISNULL(CustomUDA6, '') AS CustomUDA6      
-- ,ISNULL(CustomUDA7, '') AS CustomUDA7      
-- ,ISNULL(Issuer, '') AS Issuer      
-- ,ISNULL(LiquidTag, '') AS LiquidTag      
-- ,ISNULL(MarketCap, '') AS MarketCap      
-- ,ISNULL(Region, '') AS Region      
-- ,ISNULL(RiskCurrency, '') AS RiskCurrency   
----------Dynamic UDA---------------   
     
FROM V_TaxLots VT     
Inner Join T_Asset ON T_Asset.AssetID = VT.AssetID     
Inner Join T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID    
Inner Join T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID      
Inner Join T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency_Taxlot      
Inner Join T_Side ON dbo.T_Side.SideTagValue = VT.OrderSideTagValue  
Inner Join @AUECID AUEC ON AUEC.AUECID = VT.AUECID  
Inner Join @Fund TempFund ON TempFund.FundID = VT.FundID  
  
--LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID      
      
--LEFT OUTER JOIN dbo.T_CompanyThirdParty ON T_CompanyThirdParty.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK      
--LEFT OUTER JOIN dbo.T_ThirdParty ON T_ThirdParty.ThirdPartyId = T_CompanyThirdParty.ThirdPartyId AND T_CompanyThirdParty.CompanyID = @companyID      
--LEFT OUTER JOIN dbo.T_ThirdPartyType ON T_ThirdPartyType.ThirdPartyTypeId = T_ThirdParty.ThirdPartyTypeID      
--LEFT OUTER JOIN dbo.T_CompanyThirdPartyFlatFileSaveDetails CTPFD ON CTPFD.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK      
LEFT OUTER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol      
--LEFT OUTER JOIN V_UDA_DynamicUDA UDA ON UDA.Symbol_PK = SM.Symbol_PK     
Left Outer Join T_CompanyStrategy On T_CompanyStrategy.CompanyStrategyID = VT.Level2ID    
Left Outer Join T_CounterParty On T_CounterParty.CounterPartyID = VT.CounterPartyID  
LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate ON (  
   FXDayRatesForTradeDate.FromCurrencyID = VT.CurrencyID  
   AND FXDayRatesForTradeDate.ToCurrencyID = CF.LocalCurrency  
   AND DateDiff(d, VT.AUECLocalDate, FXDayRatesForTradeDate.DATE) = 0  
   AND FXDayRatesForTradeDate.FundID = VT.FundID  
   )  
LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate1 ON (  
   FXDayRatesForTradeDate1.FromCurrencyID = VT.CurrencyID  
   AND FXDayRatesForTradeDate1.ToCurrencyID = CF.LocalCurrency  
   AND DateDiff(d, VT.AUECLocalDate, FXDayRatesForTradeDate1.DATE) = 0  
   AND FXDayRatesForTradeDate1.FundID = 0  
   )  
    
WHERE datediff(d, (      
   CASE       
    WHEN @dateType = 1      
     THEN VT.AUECLocalDate      
    ELSE VT.ProcessDate      
    END      
   ), @inputdate) = 0          
    
 AND (      
  (      
   VT.TransactionType IN (      
    'Buy'      
    ,'BuytoClose'      
    ,'BuytoOpen'      
    ,'Sell'      
    ,'Sellshort'      
    ,'SelltoClose'      
    ,'SelltoOpen'      
    ,'LongAddition'      
    ,'LongWithdrawal'      
    ,'ShortAddition'      
    ,'ShortWithdrawal'      
    ,''      
    )      
   AND (      
    VT.TransactionSource IN (      
     0      
     ,1      
     ,2      
     ,3      
     ,4      
     ,14      
     )      
    )      
   )      
  OR (      
   @IncludeExpiredSettledTransaction = 1      
   AND VT.TransactionType IN (      
    'Exercise'      
    ,'Expire'      
    ,'Assignment'      
    )      
   AND VT.AssetID IN (      
    2      
    ,4      
    )      
   )      
  OR (      
   @IncludeExpiredSettledTransaction = 1      
   AND VT.TransactionType IN (      
    'CSCost'      
    ,'CSZero'      
    ,'DLCost'      
    ,'CSClosingPx'      
    ,'Expire'      
    ,'DLCostAndPNL'      
    )      
   AND VT.AssetID IN (3)      
   )      
  OR (      
   @IncludeExpiredSettledUnderlyingTransaction = 1      
   AND VT.TransactionType IN (      
    'Exercise'      
    ,'Expire'      
    ,'Assignment'      
    )      
   AND TaxlotClosingID_FK IS NOT NULL      
   AND VT.AssetID IN (      
    1      
    ,3      
    )      
   )      
  OR (      
   @IncludeCATransaction = 1      
   AND VT.TransactionType IN (      
    'LongAddition'      
    ,'LongWithdrawal'      
    ,'ShortAddition'      
    ,'ShortWithdrawal'      
    ,'LongCostAdj'      
    ,'ShortCostAdj'      
    ,'LongWithdrawalCashInLieu'      
    ,'ShortWithdrawalCashInLieu'      
    )      
   AND (      
    VT.TransactionSource IN (      
     6      
     ,7      
     ,8      
     ,9      
     ,11      
     )      
    )      
   )      
  OR TransactionSource = 13      
  )      
Order by AveragePrice Asc   
  
Drop Table #FXConversionRatesForTradeDate