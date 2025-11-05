                                 
/*******************************************************************                                                                                                                        
 Exec P_GetTransactions_EOD_Batch '','09/05/2017'                                                      
*/                                      
CREATE PROCEDURE [dbo].[P_GetTransactions_EOD_Batch]                                 
(                                  
  @CompanyFundIDs VARCHAR(max),        
  @InputDate DATETIME                                  
 )                                      
AS                                      
                          
SET NOCOUNT On                           
               
--Declare @CompanyFundIDs VARCHAR(max)                                  
--Declare @InputDate DATETIME                                  
--         
--Set @CompanyFundIDs= '1213,1214,1238,1239,1240,1241'                                                         
--Set @InputDate= '10/10/2017'          
           
BEGIN          
        
If (@InputDate = '')        
Begin        
 Set @InputDate = GetDate()        
End        
            
-- Update input date to last business date               
--SELECT @InputDate = dbo.AdjustBusinessDays(@InputDate,-1,1)                           
              
--Select @InputDate                              
                                 
DECLARE @Fund TABLE                                 
(                                
  FundID INT                                
)                                
        
If (@CompanyFundIDs Is NULL Or @CompanyFundIDs = '')                                                                    
 Insert InTo @Fund                                                                    
  Select                            
  CompanyFundID as FundID                             
  From T_CompanyFunds   
  Where IsActive=1                                                                  
Else                                                                    
 INSERT INTO @Fund                                                                    
  SELECT Cast(Items AS INT)                                
  FROM dbo.Split(@companyFundIDs, ',')          
                    
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
  ,BloombergSymbol VARCHAR(50)                                      
  ,Delta FLOAT                                      
  ,StrikePrice FLOAT                                      
  ,UnderlyingDelta FLOAT                                      
  ,ISINSymbol VARCHAR(50)                                      
  ,ProxySymbol VARCHAR(100)                                      
  ,ReutersSymbol VARCHAR(100)                
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
  --,IsNull(VT.FXRate_Taxlot, VT.FXRate) AS FXRate                                      
  ,IsNull(CONVERT(DECIMAL(38,9),VT.FXRate_Taxlot),CONVERT(DECIMAL(38,9), VT.FXRate)) AS FXRate                                      
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
 INNER JOIN T_Side ON T_Side.SideTagValue = VT.OrderSideTagValue                                    
 INNER JOIN T_Currency CUR ON CUR.CurrencyID = VT.CurrencyID                                 
 WHERE                                      
   VT.TaxLotQty <> 0                                   
   And Datediff(Day,VT.ProcessDate, @InputDate) = 0                                                            
                                
                                                                                                         
 SELECT                                 
VT.Symbol AS Symbol,                          
--VT.AllocatedQty,    
(VT.TaxLotQty) AS Quantity,                        
--CONVERT(VARCHAR,VT.AvgPrice) AS AvgPX,                          
CONVERT(DECIMAL(38,9),VT.AvgPrice) AS AvgPX,     
VT.CurrencySymbol AS CurrencySymbol,                        
LEFT(CONVERT(VARCHAR, VT.OriginalPurchaseDate, 101), 10) AS OriginalPurchaseDate,                          
LEFT(CONVERT(VARCHAR, VT.AUECLocalDate, 101), 10) AS TradeDate,        
LEFT(CONVERT(VARCHAR, VT.ProcessDate, 101), 10) AS ProcessDate,                        
LEFT(CONVERT(VARCHAR, VT.SettlementDate, 101), 10) AS SettlementDate,                          
LEFT(CONVERT(VARCHAR, Isnull(SM.ExpirationDate, '1/1/1800'), 101), 10) AS ExpirationDate,                          
CASE                      
                    
WHEN VT.CurrencyID <> CF.LocalCurrency                     
                    
THEN                     
                    
CASE                     
                    
WHEN IsNull(VT.FXRate, 0) <> 0                     
                    
THEN VT.FXRate                     
--      ELSE ISNull((coalesce(FXDayRatesForTradeDate.RateValue, FXDayRatesForTradeDate1.RateValue)), 0)                
ELSE 1                                              
END                    
ELSE 1                     
END AS FXRate,                          
REPLACE(Isnull(SM.CompanyName, ''), ',', ' ') AS CompanyName,                             
Isnull(SM.UnderlyingSymbol, '') AS UnderlyingSymbol,                           
CF.FundShortName AS AccountName,                          
--CONVERT(VARCHAR,VT.Commission) AS Commission,     
CONVERT(DECIMAL(38,9),VT.Commission) AS Commission,                           
VT.OtherBrokerFees AS OtherBrokerFees,                            
VT.ClearingFee,                           
VT.MiscFees,                          
VT.ClearingFee  AS AUECFee1,                          
VT.MiscFees  AS AUECFee2,                          
--CONVERT(VARCHAR,VT.StampDuty) as StampDuty,       
CONVERT(DECIMAL(38,9),VT.StampDuty) AS StampDuty,                                              
SM.SEDOLSymbol AS SEDOL,                          
SM.CUSIPSymbol AS CUSIP,                           
SM.BloombergSymbol AS Bloomberg,                          
VT.Side,                           
Case                     
When VT.IsSwapped = 1                     
Then 'EquitySwap'                    
Else T_Asset.AssetName                     
End AS Asset,                     
CP.ShortName AS CounterParty,                          
CONVERT(DECIMAL(38,9) ,VT.TaxLotQty * VT.AvgPrice * SM.Multiplier) AS GrossNotionalValue ,                                     
                      
CONVERT(DECIMAL(38,9),                    
CASE                    
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
                    
END) AS GrossNotionalValueBase,                            
                            
--CONVERT(Varchar(100), NetNotionalValue.Val) AS NetNotionalValue ,                                      
--CONVERT(DECIMAL(16,12) ,NetNotionalValue.Val )AS NetNotionalValue ,                     
                      
CONVERT(DECIMAL(38,9) ,NetNotionalValue.Val )AS NetNotionalValue ,                                      
                         
                      
CONVERT(DECIMAL(38,9) , 
CASE 
                    
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
                    
END) AS NetNotionalValueBase,                        
                    
--CONVERT(VARCHAR,VT.TotalExpenses) AS TotalCommissionandFees,                          
 CONVERT(DECIMAL(38,9) ,VT.TotalExpenses)AS TotalCommissionandFees,                    
                      
CONVERT(DECIMAL(38,9) ,                      
CASE                     
                    
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
                    
END) AS TotalCommissionandFeesBase ,                          
                    
CONVERT(VARCHAR,VT.SecFee) as SecFee,                                      
        
VT.OccFee,                                      
                    
VT.OrfFee,                                      
                    
CONVERT(VARCHAR,CASE                                       
                    
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
                    
END)                 
AS SecFeeBase,                                      
                            
CASE                                       
                    
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
                    
END AS OccFeeBase,                                      
                    
CASE    
             
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
                    
END AS OrfFeeBase,            
                            
  --,VT.ClearingBrokerFee AS ClearingBrokerFee                                      
                    
CASE                                       
                    
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
                    
END AS ClearingBrokerFeeBase,                                      
                    
VT.SoftCommission AS SoftCommission,                                      
                    
CASE                              
                    
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
                    
END AS SoftCommissionBase,                                      
                    
VT.TaxOnCommissions,                          
                    
TransactionType,                          
                    
CONVERT(DECIMAL(38,9) ,UnitCost.Val) AS UnitCost,                          
                    
CUR1.CurrencySymbol AS BaseCurrency,                                      
                    
COALESCE(LTCUR.CurrencySymbol, GCUR.CurrencySymbol, 'None') AS SettlCurrency
,CASE
When IsNull(VT.FXRate, 0) <> 0
THEN CASE
 When VT.FXConversionMethodOperator ='M' 
     THEN CONVERT(DECIMAL(38,9),VT.AvgPrice * IsNull(VT.FXRate, 0))
		
 WHEN VT.FXConversionMethodOperator ='D' AND VT.FXRate >0
     THEN CONVERT(DECIMAL(38,9),VT.AvgPrice / VT.FXRate) 
	 end
	 else 0
End AS AvgPXBase ,
SM.OSISymbol AS OSI   
                            
 FROM #V_Taxlots VT         
 Inner JOIN #SecMasterDataTempTable SM ON VT.Symbol = SM.TickerSymbol                                                              
 INNER JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID                                      
 INNER JOIN T_Company AS TC ON CF.CompanyID = TC.CompanyID                                      
 INNER JOIN T_Asset ON T_Asset.AssetID = VT.AssetID                                      
 LEFT OUTER JOIN T_CompanyMasterFundSubAccountAssociation CMF ON CMF.CompanyFundID = VT.FundID                                      
 LEFT OUTER JOIN T_companyMasterFunds MF ON MF.CompanyMasterFundID = CMF.CompanyMasterFundID                                   
 LEFT OUTER JOIN T_ThirdParty TTP ON TTP.ThirdPartyID = CF.CompanyThirdPartyID                                     
 LEFT OUTER JOIN T_SwapParameters SW ON VT.GroupID = SW.GroupID                                      
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