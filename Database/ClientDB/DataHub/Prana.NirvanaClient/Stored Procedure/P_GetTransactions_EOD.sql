      
/*******************************************************************                                                                                        
                        
*/      
CREATE PROCEDURE [dbo].[P_GetTransactions_EOD] 
(  
  @ThirdPartyID INT  
 ,@CompanyFundIDs VARCHAR(max)  
 ,@InputDate DATETIME  
 ,@CompanyID INT  
 ,@AUECIDs VARCHAR(max)  
 ,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                      
 ,@DateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                      
 ,@FileFormatID INT  
 ,@IncludeSent Int = 0       
 )      
AS      
                        
--Declare @ThirdPartyID INT  
--Declare @CompanyFundIDs VARCHAR(max)  
--Declare @InputDate DATETIME  
--Declare @CompanyID INT  
--Declare @AUECIDs VARCHAR(max)  
--Declare @TypeID INT                    
--Declare @DateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                      
--Declare @FileFormatID INT  
--Declare @IncludeSent Int               
--                         
--set @ThirdPartyID= 50                          
--set @CompanyFundIDs= '13,1,2,11,3,4,12,8,9,10,5,6,7'                         
--set @InputDate= '2017-05-25 08:22:38:000'                          
--set @CompanyID= 7           
--set @AUECIDs= '63,34,43,56,59,47,21,18,1,15,11,62,73,80,81'  
--Set @TypeID = 0 
--Set @DateType = 0 
--Set @FileFormatID = 0  
--Set @IncludeSent = 0                   
     
BEGIN  
 
DECLARE @Fund TABLE 
(
  FundID INT
)
DECLARE @AUECID TABLE 
(
  AUECID INT
)

INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',')

INSERT INTO @AUECID
SELECT Cast(Items AS INT)
FROM dbo.Split(@auecIDs, ',')
    
      
CREATE TABLE #SecMasterDataTempTable 
(      
   AUECID INT      
  ,TickerSymbol VARCHAR(100)      
  ,CompanyName VARCHAR(500)      
  ,AssetName VARCHAR(100)      
  ,SecurityTypeName VARCHAR(200)      
  ,SectorName VARCHAR(100)      
  ,SubSectorName VARCHAR(100)      
  ,CountryName VARCHAR(100)      
  ,PutOrCall VARCHAR(5)      
  ,Multiplier FLOAT      
  ,LeadCurrencyID INT      
  ,VsCurrencyID INT      
  ,CurrencyID INT      
  ,UnderlyingSymbol VARCHAR(100)      
  ,ExpirationDate DATETIME      
  ,Coupon FLOAT      
  ,IssueDate DATETIME      
  ,MaturityDate DATETIME      
  ,FirstCouponDate DATETIME      
  ,CouponFrequencyID INT      
  ,AccrualBasisID INT     
  ,BondTypeID INT      
  ,IsZero INT      
  ,IsNDF BIT      
  ,FixingDate DATETIME      
  ,IDCOSymbol VARCHAR(50)      
  ,OSISymbol VARCHAR(50)      
  ,SEDOLSymbol VARCHAR(50)      
  ,CUSIPSymbol VARCHAR(50)      
  ,BloombergSymbol VARCHAR(200)      
  ,Delta FLOAT      
  ,StrikePrice FLOAT      
  ,UnderlyingDelta FLOAT      
  ,ISINSymbol VARCHAR(50)      
  ,ProxySymbol VARCHAR(100)      
  ,ReutersSymbol VARCHAR(100)      
  ,IsCurrencyFuture BIT      
  ,Symbol_PK BIGINT      
  )      
      
 INSERT INTO #SecMasterDataTempTable      
 SELECT AUECID      
  ,TickerSymbol      
  ,CompanyName      
  ,AssetName      
  ,SecurityTypeName      
  ,SectorName      
  ,SubSectorName      
  ,CountryName      
  ,PutOrCall      
  ,Multiplier      
  ,LeadCurrencyID      
  ,VsCurrencyID      
  ,CurrencyID      
  ,UnderlyingSymbol      
  ,ExpirationDate      
  ,Coupon      
  ,IssueDate      
  ,MaturityDate      
  ,FirstCouponDate      
  ,CouponFrequencyID      
  ,AccrualBasisID      
  ,BondTypeID      
  ,IsZero      
  ,IsNDF      
  ,FixingDate      
  ,IDCOSymbol      
  ,OSISymbol      
  ,SEDOLSymbol      
  ,CUSIPSymbol      
  ,BloombergSymbol      
  ,Delta      
  ,StrikePrice      
  ,UnderlyingDelta      
  ,ISINSymbol      
  ,ProxySymbol      
  ,ReutersSymbol      
  ,IsCurrencyFuture      
  ,Symbol_PK      
 FROM V_SecMasterData      
      
 CREATE TABLE #V_Taxlots 
(      
  TaxLotID VARCHAR(50)      
  ,OrderSideTagValue CHAR(1)      
  ,TotalExpenses FLOAT      
  ,Level2ID INT      
  ,TaxLotQty FLOAT      
  ,AvgPrice FLOAT      
  ,Commission FLOAT      
  ,OtherBrokerFees FLOAT      
  ,ClearingFee FLOAT      
  ,MiscFees FLOAT      
  ,StampDuty FLOAT      
  ,AUECLocalDate DATETIME      
  ,OriginalPurchaseDate DATETIME      
  ,ProcessDate DATETIME      
  ,AssetID INT      
  ,UnderLyingID INT      
  ,ExchangeID INT      
  ,CurrencyID INT      
  ,CurrencySymbol VARCHAR(20)      
  ,AUECID INT      
  ,SettlementDate DATETIME      
  ,Description VARCHAR(max)      
  ,AllocationDate DATETIME      
  ,IsSwapped BIT      
  ,GroupID VARCHAR(50)      
  ,FXRate FLOAT      
  ,FXConversionMethodOperator VARCHAR(3)      
  ,IsPreAllocated BIT      
  ,CumQty FLOAT      
  ,AllocatedQty FLOAT      
  ,Quantity FLOAT      
  ,UserID INT      
  ,CounterPartyID INT      
  ,FundID INT      
  ,symbol VARCHAR(50)      
  ,SideMultiplier INT      
  ,Side VARCHAR(50)      
  ,LotID VARCHAR(200)      
  ,ExternalTransID VARCHAR(100)      
  ,TradeAttribute1 VARCHAR(200)      
  ,TradeAttribute2 VARCHAR(200)      
  ,TradeAttribute3 VARCHAR(200)      
  ,TradeAttribute4 VARCHAR(200)      
  ,TradeAttribute5 VARCHAR(200)      
  ,TradeAttribute6 VARCHAR(200)      
  ,SecFee FLOAT      
  ,OccFee FLOAT      
  ,OrfFee FLOAT      
  ,ClearingBrokerFee FLOAT      
  ,SoftCommission FLOAT      
  ,TaxOnCommissions FLOAT      
  ,TransactionLevy FLOAT      
  ,TransactionType VARCHAR(200)      
  ,InternalComments VARCHAR(500)      
  ,SettlCurrency_Group INT        
  ,SettlCurrency_Taxlot INT          
  ,TransactionSource INT        
  ,OptionPremiumAdjustment FLOAT      
  )      
      
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
 EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @InputDate ,@InputDate      
      
 UPDATE #FXConversionRatesForTradeDate   
 SET RateValue = 1.0 / RateValue      
 WHERE RateValue <> 0 AND ConversionMethod = 1      
      
 UPDATE #FXConversionRatesForTradeDate      
 SET RateValue = 0      
 WHERE RateValue IS NULL      
      
INSERT INTO #V_Taxlots 
(      
  TaxLotID      
  ,OrderSideTagValue      
  ,TotalExpenses      
  ,Level2ID      
  ,TaxLotQty      
  ,AvgPrice      
  ,Commission      
  ,OtherBrokerFees      
  ,ClearingFee      
  ,MiscFees      
  ,StampDuty      
  ,AUECLocalDate      
  ,OriginalPurchaseDate      
  ,ProcessDate      
  ,AssetID      
  ,UnderLyingID      
  ,ExchangeID      
  ,CurrencyID      
  ,CurrencySymbol      
  ,AUECID      
  ,SettlementDate      
  ,[Description]      
  ,AllocationDate      
  ,IsSwapped      
  ,GroupID      
  ,FXRate      
  ,FXConversionMethodOperator      
  ,IsPreAllocated      
  ,CumQty      
  ,AllocatedQty      
  ,Quantity      
  ,UserID      
  ,CounterPartyID      
  ,FundID      
  ,symbol      
  ,SideMultiplier      
  ,Side      
  ,LotID      
  ,ExternalTransID      
  ,TradeAttribute1      
  ,TradeAttribute2      
  ,TradeAttribute3      
  ,TradeAttribute4      
  ,TradeAttribute5      
  ,TradeAttribute6      
  ,SecFee      
  ,OccFee      
  ,OrfFee      
  ,ClearingBrokerFee      
  ,SoftCommission      
  ,TaxOnCommissions      
  ,TransactionLevy      
  ,TransactionType      
  ,InternalComments      
  ,SettlCurrency_Group      
  ,SettlCurrency_Taxlot          
  ,TransactionSource        
  ,OptionPremiumAdjustment      
  )      
 SELECT VT.TaxLotID      
  ,OrderSideTagValue      
  ,TotalExpenses      
  ,Level2ID      
  ,TaxLotQty      
  ,AvgPrice      
  ,Commission      
  ,OtherBrokerFees      
  ,ClearingFee      
  ,MiscFees      
  ,StampDuty      
  ,AUECLocalDate      
  ,OriginalPurchaseDate      
  ,ProcessDate      
  ,VT.AssetID      
  ,VT.UnderLyingID      
  ,VT.ExchangeID      
  ,VT.CurrencyID      
  ,CUR.CurrencySymbol      
  ,VT.AUECID      
  ,SettlementDate      
  ,Description      
  ,AllocationDate      
  ,IsSwapped      
  ,VT.GroupID      
  ,IsNull(VT.FXRate_Taxlot, VT.FXRate) AS FXRate      
  ,IsNull(FXConversionMethodOperator_Taxlot, FXConversionMethodOperator) AS FXConversionMethodOperator      
  ,IsPreAllocated      
  ,CumQty      
  ,AllocatedQty      
  ,Quantity      
  ,UserID      
  ,CounterPartyID      
  ,VT.FundID      
  ,Symbol      
  ,SideMultiplier      
  ,T_Side.Side      
  ,VT.LotID      
  ,VT.ExternalTransID      
  ,VT.TradeAttribute1      
  ,VT.TradeAttribute2      
  ,VT.TradeAttribute3      
  ,VT.TradeAttribute4      
  ,VT.TradeAttribute5      
  ,VT.TradeAttribute6      
  ,SecFee      
  ,OccFee      
  ,OrfFee      
  ,ClearingBrokerFee      
  ,SoftCommission      
  ,VT.TaxOnCommissions      
  ,VT.TransactionLevy      
  ,TransactionType      
  ,VT.InternalComments      
  ,VT.SettlCurrency_Group         
  ,VT.SettlCurrency_Taxlot       
  ,TransactionSource        
  ,VT.OptionPremiumAdjustment      
 FROM V_Taxlots VT  
 INNER JOIN @Fund TempFund ON VT.FundID = TempFund.FundID      
 INNER JOIN @AUECID TempAUECID ON VT.AUECID = TempAUECID.AUECID      
 INNER JOIN T_Side ON T_Side.SideTagValue = VT.OrderSideTagValue      
 INNER JOIN T_Currency CUR ON CUR.CurrencyID = VT.CurrencyID 
 WHERE      
   VT.TaxLotQty <> 0   
   And Datediff(Day, 
				(
					CASE 
						WHEN @DateType = 1
						THEN VT.AUECLocalDate
						ELSE VT.ProcessDate
					END
				), @InputDate) = 0  

                                                                         
 SELECT 
  VT.TaxLotID AS TaxLotID      
  ,VT.AUECLocalDate AS TradeDate      
  ,VT.OriginalPurchaseDate      
  ,VT.ProcessDate      
  ,VT.OrderSideTagValue AS SideID      
  ,VT.Symbol AS Symbol      
  ,VT.TaxLotQty AS Quantity      
  ,VT.AvgPrice AS AvgPX      
  ,VT.FundID AS FundID      
  ,VT.AssetID AS AssetID      
  ,VT.UnderLyingID AS UnderLyingID      
  ,VT.ExchangeID AS ExchangeID      
  ,VT.CurrencyID AS CurrencyID      
  ,VT.CurrencySymbol AS CurrencySymbol      
  ,VT.AUECID AS AUECID     
  ,VT.TotalExpenses AS TotalCommissionandFees      
  ,Isnull(SM.Multiplier,0) AS Multiplier      
  ,VT.SettlementDate AS SettlementDate      
  ,SM.LeadCurrencyID      
  ,SM.VsCurrencyID      
  ,Isnull(SM.ExpirationDate, '1/1/1800') AS ExpirationDate      
  ,VT.Description AS Description      
  ,VT.Level2ID AS Level2ID      
  ,Isnull((VT.TaxLotQty * SW.NotionalValue / VT.CumQty), 0) AS NotionalValue      
  ,Isnull(SW.BenchMarkRate, 0) AS BenchMarkRate      
  ,Isnull(SW.Differential, 0) AS Differential      
  ,Isnull(SW.OrigCostBasis, 0) AS OrigCostBasis      
  ,Isnull(SW.DayCount, 0) AS DayCount      
  ,Isnull(SW.SwapDescription, '') AS SwapDescription      
  ,SW.FirstResetDate AS FirstResetDate      
  ,SW.OrigTransDate AS OrigTransDate      
  ,VT.IsSwapped AS IsSwapped      
  ,VT.AllocationDate AS AUECLocalDate      
  ,VT.GroupID      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN VT.FXRate      
      ELSE ISNull((coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue)), 0)      
      END      
   ELSE 1      
   END AS FXRate      
  ,VT.FXConversionMethodOperator      
  ,Isnull(SM.CompanyName, '') AS CompanyName      
  ,Isnull(SM.UnderlyingSymbol, '') AS UnderlyingSymbol      
  ,Isnull(SM.Delta, 1) AS Delta      
  ,Isnull(SM.PutOrCall, '') AS PutOrCall      
  ,VT.IsPreAllocated      
  ,VT.CumQty      
  ,VT.AllocatedQty      
  ,VT.Quantity      
  ,Isnull(SM.StrikePrice, 0) AS StrikePrice      
  ,VT.UserID      
  ,VT.CounterPartyID      
  ,SM.Coupon      
  ,SM.IssueDate      
  ,SM.MaturityDate      
  ,SM.FirstCouponDate      
  ,SM.CouponFrequencyID      
  ,SM.AccrualBasisID      
  ,SM.BondTypeID      
  ,SM.IsZero      
  ,SM.IsNDF      
  ,SM.FixingDate       
  ,CF.FundShortName AS AccountName      
  ,VT.Commission AS Commission      
  ,VT.OtherBrokerFees AS OtherBrokerFees      
  ,VT.ClearingFee      
  ,VT.MiscFees      
  ,VT.ClearingFee  AS AUECFee1        
  ,VT.MiscFees  AS AUECFee2        
  ,VT.StampDuty      
  ,SM.IDCOSymbol AS IDCO      
  ,SM.OSISymbol AS OSI      
  ,SM.SEDOLSymbol AS SEDOL      
  ,SM.CUSIPSymbol AS CUSIP      
  ,SM.BloombergSymbol AS Bloomberg      
  ,VT.Side      
  ,Case    
	When VT.IsSwapped = 1    
	Then 'EquitySwap'    
	Else T_Asset.AssetName     
  End AS Asset             
  ,CP.ShortName AS CounterParty      
  ,IsNull(MF.MasterFundName, CF.FundShortName) AS MasterFund      
  ,TTP.ThirdPartyName AS PrimeBroker      
  ,SM.UnderlyingDelta 
  ,VT.TaxLotQty * VT.AvgPrice * SM.Multiplier AS GrossNotionalValue      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.TaxLotQty * VT.AvgPrice * SM.Multiplier      
   END AS GrossNotionalValueBase 
 ,NetNotionalValue.Val AS NetNotionalValue       
  ,CASE       
   WHEN T_Asset.Assetid = 8      
    THEN CASE       
      WHEN VT.CurrencyID <> CF.LocalCurrency      
       THEN CASE       
         WHEN IsNull(VT.FXRate, 0) <> 0      
          THEN CASE       
            WHEN VT.FXConversionMethodOperator = 'M'      
             THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(VT.FXRate, 0)      
            WHEN VT.FXConversionMethodOperator = 'D'      
             AND VT.FXRate > 0      
             THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * 1 / VT.FXRate      
            END      
         ELSE IsNull((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
         END      
      ELSE (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses)      
      END      
   ELSE CASE       
     WHEN VT.CurrencyID <> CF.LocalCurrency      
      THEN CASE       
        WHEN IsNull(VT.FXRate, 0) <> 0      
         THEN CASE       
           WHEN VT.FXConversionMethodOperator = 'M'      
            THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(VT.FXRate, 0)      
           WHEN VT.FXConversionMethodOperator = 'D'      
            AND VT.FXRate > 0      
            THEN (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * 1 / VT.FXRate      
           END      
        ELSE IsNull((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
        END      
     ELSE (VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses)      
     END      
   END AS NetNotionalValueBase      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.TotalExpenses) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.TotalExpenses) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.TotalExpenses) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.TotalExpenses      
   END AS TotalCommissionandFeesBase      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.Commission) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.Commission) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.Commission) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.Commission      
   END AS CommissionBase      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.OtherBrokerFees) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.OtherBrokerFees) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.OtherBrokerFees) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.OtherBrokerFees      
   END AS OtherBrokerFeesBase      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.ClearingFee) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.ClearingFee) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.ClearingFee) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.ClearingFee      
   END AS ClearingFeeBase      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.ClearingFee) * IsNull(VT.FXRate, 0)          
         WHEN VT.FXConversionMethodOperator = 'D'          
          AND VT.FXRate > 0          
          THEN (VT.ClearingFee) * 1 / VT.FXRate          
         END          
      ELSE IsNull((VT.ClearingFee) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)          
      END          
   ELSE VT.ClearingFee          
   END AS AUECFee1Base          
  ,CASE           
   WHEN VT.CurrencyID <> CF.LocalCurrency          
    THEN CASE           
      WHEN IsNull(VT.FXRate, 0) <> 0          
       THEN CASE           
         WHEN VT.FXConversionMethodOperator = 'M'          
          THEN (VT.MiscFees) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.MiscFees) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.MiscFees) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.MiscFees      
   END AS MiscFeesBase      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.MiscFees) * IsNull(VT.FXRate, 0)          
         WHEN VT.FXConversionMethodOperator = 'D'          
          AND VT.FXRate > 0          
          THEN (VT.MiscFees) * 1 / VT.FXRate          
         END          
      ELSE IsNull((VT.MiscFees) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)          
      END          
   ELSE VT.MiscFees          
   END AS AUECFee2Base          
  ,CASE           
   WHEN VT.CurrencyID <> CF.LocalCurrency          
    THEN CASE           
      WHEN IsNull(VT.FXRate, 0) <> 0          
       THEN CASE           
         WHEN VT.FXConversionMethodOperator = 'M'          
          THEN (VT.StampDuty) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.StampDuty) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.StampDuty) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.StampDuty      
   END AS StampDutyBase      
  ,VT.LotID      
  ,VT.ExternalTransID      
  ,VT.TradeAttribute1      
  ,VT.TradeAttribute2      
  ,VT.TradeAttribute3      
  ,VT.TradeAttribute4      
  ,VT.TradeAttribute5      
  ,VT.TradeAttribute6      
  ,SM.ProxySymbol      
  ,SM.AssetName      
  ,SM.SecurityTypeName      
  ,SM.SectorName      
  ,SM.SubSectorName      
  ,SM.CountryName      
  ,VT.SecFee      
  ,VT.OccFee      
  ,VT.OrfFee      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.SecFee) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.SecFee) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.SecFee) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.SecFee      
   END AS SecFeeBase      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.OccFee) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.OccFee) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.OccFee) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.OccFee      
   END AS OccFeeBase      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.OrfFee) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.OrfFee) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.OrfFee) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.OrfFee      
   END AS OrfFeeBase      
  ,VT.ClearingBrokerFee AS ClearingBrokerFee      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.ClearingBrokerFee) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.ClearingBrokerFee) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.ClearingBrokerFee) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.ClearingBrokerFee      
   END AS ClearingBrokerFeeBase      
  ,VT.SoftCommission AS SoftCommission      
  ,CASE       
   WHEN VT.CurrencyID <> CF.LocalCurrency      
    THEN CASE       
      WHEN IsNull(VT.FXRate, 0) <> 0      
       THEN CASE       
         WHEN VT.FXConversionMethodOperator = 'M'      
          THEN (VT.SoftCommission) * IsNull(VT.FXRate, 0)      
         WHEN VT.FXConversionMethodOperator = 'D'      
          AND VT.FXRate > 0      
          THEN (VT.SoftCommission) * 1 / VT.FXRate      
         END      
      ELSE IsNull((VT.SoftCommission) * IsNull(coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue), 0), 0)      
      END      
   ELSE VT.SoftCommission      
   END AS SoftCommissionBase      
  ,VT.TaxOnCommissions      
  ,VT.TransactionLevy      
  ,VT.SideMultiplier      
  ,TransactionType      
  ,SM.ReutersSymbol      
  ,UnitCost.Val AS UnitCost      
  ,VT.SecFee AS SecFees      
  ,VT.InternalComments      
  ,CUR1.CurrencySymbol AS BaseCurrency      
  ,COALESCE(LTCUR.CurrencySymbol, GCUR.CurrencySymbol, 'None') AS SettlCurrency          
  ,SM.IsCurrencyFuture      
  ,UDA.*      
  ,SettlCurrFxRate1.Val * VT.Commission AS SettlementCurrencyCommission      
  ,SettlCurrFxRate1.Val * UnitCost.Val AS SettlementCurrencyCostBasis      
  ,SettlCurrFxRate1.Val * VT.AvgPrice AS SettlementCurrencyAveragePrice      
  ,SettlCurrFxRate1.Val * NetNotionalValue.Val AS SettlementCurrencyTotalCost      
  ,VT.TransactionSource        
  ,VT.OptionPremiumAdjustment      
 --                                                                      
 FROM #V_Taxlots VT 
 INNER JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID      
 INNER JOIN T_Company AS TC ON CF.CompanyID = TC.CompanyID      
 INNER JOIN T_Asset ON T_Asset.AssetID = VT.AssetID      
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = VT.FundID      
 LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID      
 LEFT OUTER JOIN T_ThirdParty TTP ON TTP.ThirdPartyID = CF.CompanyThirdPartyID      
 LEFT OUTER JOIN T_SwapParameters SW ON VT.GroupID = SW.GroupID      
 LEFT OUTER JOIN #SecMasterDataTempTable SM ON VT.Symbol = SM.TickerSymbol      
 LEFT OUTER JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID      
 LEFT OUTER JOIN T_currency CUR1 ON CUR1.CurrencyID = CF.LocalCurrency      
 LEFT OUTER JOIN T_currency GCUR ON GCUR.CurrencyID = VT.SettlCurrency_Group      
 LEFT OUTER JOIN T_currency LTCUR ON LTCUR.CurrencyID = VT.SettlCurrency_Taxlot      
 LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate      
  ON (      
    FXDayRatesForTradeDate.FromCurrencyID = VT.CurrencyID      
    AND FXDayRatesForTradeDate.ToCurrencyID = CF.LocalCurrency      
    AND DateDiff(d, VT.AUECLocalDate, FXDayRatesForTradeDate.DATE) = 0      
    AND FXDayRatesForTradeDate.FundID = VT.FundID      
    )      
 LEFT OUTER JOIN #FXConversionRatesForTradeDate FXDayRatesForTradeDate1      
  ON (      
    FXDayRatesForTradeDate1.FromCurrencyID = VT.CurrencyID      
    AND FXDayRatesForTradeDate1.ToCurrencyID = CF.LocalCurrency      
    AND DateDiff(d, VT.AUECLocalDate, FXDayRatesForTradeDate1.DATE) = 0      
    AND FXDayRatesForTradeDate1.FundID = 0      
    )      
 LEFT OUTER JOIN V_UDA_DynamicUDA UDA      
  ON SM.Symbol_PK = UDA.Symbol_PK           
 CROSS APPLY (      
  SELECT CASE  
		 WHEN VT.SettlCurrency_Group <> VT.CurrencyID
				THEN CASE 
					WHEN Isnull(VT.FXRate, 0) > 0
						THEN CASE Isnull(VT.FXConversionMethodOperator, 'M')
								WHEN 'M'
									THEN VT.FXRate
								WHEN 'D'
									THEN 1 / VT.FXRate
								END
					ELSE 0
					END
			ELSE 1
		END           
  ) AS SettlCurrFxRate1(Val) 

 CROSS APPLY (  
   SELECT CASE         
    WHEN (      
      Quantity = 0      
      OR Multiplier = 0      
      )      
     THEN 0      
    WHEN (      
      OrderSideTagValue = '1'      
      OR OrderSideTagValue = '3'      
      OR OrderSideTagValue = 'A'      
      OR OrderSideTagValue = 'B'      
      OR OrderSideTagValue = 'E'      
      )      
     THEN (IsNull((Quantity * IsNull(AvgPrice, 0) * Multiplier + TotalExpenses), 0)) / (Quantity * Multiplier)      
    ELSE (IsNull((Quantity * IsNull(AvgPrice, 0) * Multiplier - TotalExpenses), 0)) / (Quantity * Multiplier)      
    END      
  ) AS UnitCost(Val)      
 CROSS APPLY (      
  SELECT CASE       
    WHEN T_Asset.Assetid = 8      
     THEN VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses      
    ELSE VT.TaxLotQty * VT.AvgPrice * SM.Multiplier + VT.SideMultiplier * VT.TotalExpenses      
    END      
  ) AS NetNotionalValue(Val)      
      
 
DROP TABLE #V_Taxlots, #SecMasterDataTempTable, #FXConversionRatesForTradeDate     
        
END