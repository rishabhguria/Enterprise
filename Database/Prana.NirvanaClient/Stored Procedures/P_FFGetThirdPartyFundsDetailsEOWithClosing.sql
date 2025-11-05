/*        
Modified By : Suraj Nataraj        
Date : 07-02-2015        
DESC : Add Dynamic UDA Fields        
JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-9129     
*/        
  
CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetailsEOWithClosing]   
(        
 @thirdPartyID INT        
 ,@companyFundIDs VARCHAR(max)        
 ,@inputDate DATETIME        
 ,@companyID INT        
 ,@auecIDs VARCHAR(max)        
 ,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties               
 ,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                              
 ,@fileFormatID INT        
 )        
AS 

--Declare 
-- @thirdPartyID INT        
-- ,@companyFundIDs VARCHAR(max)        
-- ,@inputDate DATETIME        
-- ,@companyID INT        
-- ,@auecIDs VARCHAR(max)        
-- ,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties               
-- ,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                              
-- ,@fileFormatID INT

--SET @thirdPartyID=12
--SET @companyFundIDs=N'2,6,3,1,4,5'
--SET @inputDate='2021-06-10 01:51:42'
--SET @companyID=2
--SET @auecIDs=N'20,77,63,53,44,34,43,59,31,18,61,74,1,15,11,62,73,80,81'
--SET @TypeID=0
--SET @dateType=0
--SET @fileFormatID=28

      
DECLARE @Fund TABLE (FundID INT)        
DECLARE @AUECID TABLE (AUECID INT)        
DECLARE @IncludeExpiredSettledTransaction INT        
DECLARE @IncludeExpiredSettledUnderlyingTransaction INT        
DECLARE @IncludeCATransaction INT        
        
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
  
CREATE Table #VT  
(  
 TaxLotID VARCHAR(50)  
 ,EntityID VARCHAR(50)  
 ,FundID INT   
 ,SideID VARCHAR(3)  
 ,Symbol VARCHAR(100)  
 ,CounterPartyID INT   
 ,OrderQty FLOAT  
 ,AvgPrice FLOAT  
 ,CumQty FLOAT  
 ,Quantity FLOAT   
 ,ExchangeID INT  
 ,CurrencyID INT  
 ,Level1AllocationID VARCHAR(50)  
 ,SettlementDate DATETIME  
 ,Commission FLOAT  
 ,OtherBrokerFees FLOAT  
 ,GroupRefID INT  
 ,TaxlotState VARCHAR(50)   
 ,StampDuty FLOAT  
 ,TransactionLevy FLOAT  
 ,ClearingFee FLOAT  
 ,TaxOnCommissions FLOAT  
 ,MiscFees FLOAT  
 ,AUECLocalDate DATETIME  
 ,Level2ID INT  
 ,AUECID INT  
 ,PBID INT  
 ,FXRate FLOAT  
 ,FXConversionMethodOperator VARCHAR(3)  
 ,FromDeleted VARCHAR(5)  
 ,ProcessDate DATETIME  
 ,OriginalPurchaseDate DATETIME  
 ,AccruedInterest FLOAT  
 ,BenchMarkRate FLOAT  
 ,Differential FLOAT  
 ,SwapDescription VARCHAR(500)  
 ,DayCount INT  
 ,FirstResetDate DATETIME  
 ,IsSwapped BIT  
 ,FXRate_Taxlot FLOAT  
 ,FXConversionMethodOperator_Taxlot VARCHAR(3)  
 ,LotID VARCHAR(200)  
 ,ExternalTransID VARCHAR(200)  
 ,SecFee FLOAT  
 ,OccFee FLOAT  
 ,OrfFee FLOAT  
 ,ClearingBrokerFee FLOAT  
 ,SoftCommission FLOAT  
 ,TransactionType VARCHAR(200)  
 ,AlgorithmAcronym VARCHAR(100)  
 ,AlgorithmDesc VARCHAR(100)  
)  
  
Insert INTO #VT  
SELECT  
 VT.TaxLotID as TaxLotID  
 ,VT.Level1AllocationID AS EntityID  
 ,ISNULL(VT.FundID, 0) AS FundID  
 ,VT.OrderSideTagValue AS SideID  
 ,VT.Symbol  
 ,VT.CounterPartyID  
 ,VT.TaxLotQty AS OrderQty  
 ,VT.AvgPrice  
 ,VT.CumQty  
 ,VT.Quantity  
 ,VT.ExchangeID  
 ,VT.CurrencyID  
 ,VT.Level1AllocationID AS Level1AllocationID  
 ,VT.SettlementDate  
 ,VT.Commission  
 ,VT.OtherBrokerFees  
 ,VT.GroupRefID  
 ,0 AS TaxlotState   
 ,ISNULL(VT.StampDuty, 0) AS StampDuty  
 ,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy  
 ,ISNULL(ClearingFee, 0) AS ClearingFee  
 ,ISNULL(TaxOnCommissions, 0) AS TaxOnCommissions  
 ,ISNULL(MiscFees, 0) AS MiscFees  
 ,VT.AUECLocalDate  
 ,0 AS Level2ID  
 ,VT.AUECID  
 ,@ThirdPartyID AS PBID
 ,VT.FXRate  
 ,VT.FXConversionMethodOperator  
 ,'No' AS FromDeleted  
 ,VT.ProcessDate  
 ,VT.OriginalPurchaseDate  
 ,VT.AccruedInterest  
 ,VT.BenchMarkRate  
 ,VT.Differential  
 ,VT.SwapDescription  
 ,VT.DayCount  
 ,VT.FirstResetDate  
 ,VT.IsSwapped  
 ,VT.FXRate_Taxlot  
 ,VT.FXConversionMethodOperator_Taxlot  
 ,VT.LotID  
 ,VT.ExternalTransID  
 ,ISNULL(VT.SecFee, 0) AS SecFee  
 ,ISNULL(VT.OccFee, 0) AS OccFee  
 ,ISNULL(VT.OrfFee, 0) AS OrfFee  
 ,VT.ClearingBrokerFee  
 ,VT.SoftCommission  
 ,VT.TransactionType  
 ,''  
 ,''  
  
FROM V_TaxLots VT  
--INNER JOIN T_PBWiseTaxlotState PB ON PB.TaxlotID = VT.TaxlotID  
WHERE 
--(  
--  PB.fileFormatID = 0  
--  OR @fileFormatID = FileFormatID  
--  )  
 --AND (  
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
  --)  
  
UPDATE #VT
SET #VT.TaxlotState = PB.TaxlotState
FROM #VT
INNER JOIN T_PBWiseTaxlotState PB  with (nolock) ON (PB.TaxlotID = #VT.TaxLotID)
WHERE PB.TaxlotState <> 0
AND PB.FileFormatID = @fileFormatID

Update #VT  
Set AlgorithmAcronym = T_ClosingAlgos.AlgorithmAcronym  
 ,AlgorithmDesc = T_ClosingAlgos.AlgorithmDesc  
From #VT VT  
Inner Join PM_TaxLotClosing Closing On Closing.ClosingTaxLotID = VT.TaxLotID        
Inner Join T_ClosingAlgos On T_ClosingAlgos.AlgorithmId = Closing.ClosingAlgo        
  
  
IF (@TypeID = 0)        
BEGIN        
 SELECT VT.Level1AllocationID as EntityID        
 ,ISNULL(VT.FundID, 0) AS FundID          
 ,VT.SideID AS SideID        
 ,T_Side.Side AS Side        
 ,VT.Symbol as Symbol        
 ,VT.CounterPartyID as CounterPartyID        
 ,CP.ShortName as CounterParty      
 ,Sum(VT.OrderQty) AS AllocatedQty          
 ,VT.AvgPrice as AveragePrice        
 ,VT.CumQty as ExecutedQty        
 ,VT.Quantity as TotalQty        
 ,VT.ExchangeID as ExchangeID   
 ,T_Exchange.DisplayName as Exchange       
 ,Currency.CurrencyID as CurrencyID      
 ,Currency.CurrencyName as CurrencyName        
 ,Currency.CurrencySymbol as CurrencySymbol        
 ,CF.FundName as AccountName        
 ,CTPM.MappedName as AccountMappedName        
 ,CTPM.FundAccntNo as AccountNo         
 ,CTPM.FundTypeID_FK as AccountTypeID_FK        
 ,FT.FundTypeName as FundTypeName        
 ,SM.PutOrCall as PutOrCall        
 ,SM.StrikePrice as StrikePrice        
 ,SM.ExpirationDate as ExpirationDate        
 ,VT.SettlementDate as SettlementDate        
 ,sum(VT.Commission) as CommissionCharged        
 ,sum(VT.OtherBrokerFees) as OtherBrokerFees          
 ,VT.GroupRefID as GroupRefID        
 ,'Allocated' AS TaxLotState        
 ,sum(ISNULL(VT.StampDuty, 0)) AS StampDuty        
 ,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy        
 ,Sum(ISNULL(ClearingFee, 0)) AS ClearingFee        
 ,Sum(ISNULL(TaxOnCommissions, 0)) AS TaxOnCommissions        
 ,Sum(ISNULL(MiscFees, 0)) AS MiscFees        
 ,VT.AUECLocalDate as TradeDate        
 ,SM.Multiplier as AssetMultiplier          
 ,SM.ISINSymbol as ISINSymbol        
 ,SM.CUSIPSymbol as CUSIPSymbol        
 ,SM.SEDOLSymbol as SEDOLSymbol        
 ,SM.ReutersSymbol as ReutersSymbol        
 ,SM.BloombergSymbol as BloombergSymbol        
 ,SM.CompanyName as FullSecurityName        
 ,SM.UnderlyingSymbol as UnderlyingSymbol        
 ,SM.LeadCurrencyID as LeadCurrencyID        
 ,SM.LeadCurrency as LeadCurrency        
 ,SM.VsCurrencyID as VsCurrencyID        
 ,SM.VsCurrency as VsCurrency        
 ,SM.OSISymbol as OSISymbol        
 ,SM.IDCOSymbol as IDCOSymbol        
 ,SM.OpraSymbol as OpraSymbol        
 ,VT.FXRate as ForexRate        
 ,VT.FXConversionMethodOperator as FXConversionMethodOperator        
 ,VT.ProcessDate as ProcessDate        
 ,VT.OriginalPurchaseDate as OriginalPurchaseDate        
 ,VT.AccruedInterest as AccruedInterest        
 ,'' AS Comment1        
    ,'' AS Comment2        
  ,VT.FXRate_Taxlot as FXRate_Taxlot        
  ,VT.FXConversionMethodOperator_Taxlot as FXConversionMethodOperator_Taxlot        
  ,sum(ISNULL(VT.SecFee, 0)) AS SecFee        
  ,sum(ISNULL(VT.OccFee, 0)) AS OccFee        
  ,sum(ISNULL(VT.OrfFee, 0)) AS OrfFee        
  ,sum(VT.ClearingBrokerFee) as ClearingBrokerFee        
  ,sum(VT.SoftCommission) as SoftCommissionCharged        
  ,VT.TransactionType as TransactionType        
  ,ISNULL(VT.AlgorithmAcronym,'') as AlgorithmAcronym        
  ,ISNULL(VT.AlgorithmDesc,'') as AlgorithmDesc        
 FROM #VT VT
 INNER JOIN T_CompanyFunds AS CF ON CF.CompanyFundID=VT.FundID        
 INNER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID        
 INNER JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK    
 INNER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol     
 INNER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID         
 INNER JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID        
 INNER JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID   
 Left OUTER JOIN T_CounterParty as CP ON CP.CounterPartyID=VT.CounterPartyID  
 WHERE datediff(d, (        
    CASE         
     WHEN @dateType = 1        
      THEN VT.AUECLocalDate        
     ELSE VT.ProcessDate        
     END        
    ), @inputdate) = 0        
  AND CTPM.InternalFundNameID_FK IN (        
   SELECT FundID        
   FROM @Fund        
   )        
  AND VT.AUECID IN (        
   SELECT AUECID        
   FROM @AUECID        
   )          
 GROUP BY  
 VT.FundID        
 ,VT.Level1AllocationID     
 ,VT.SideID       
 ,T_Side.Side        
 ,VT.Symbol        
 ,VT.CounterPartyID        
 ,CP.ShortName          
 ,VT.AvgPrice        
 ,VT.CumQty        
 ,VT.Quantity              
 ,VT.ExchangeID      
 ,T_Exchange.DisplayName    
 ,Currency.CurrencyID        
 ,Currency.CurrencyName        
 ,Currency.CurrencySymbol        
 ,CTPM.MappedName        
 ,CF.FundName        
 ,CTPM.FundAccntNo        
 ,CTPM.FundTypeID_FK        
 ,FT.FundTypeName        
 ,SM.PutOrCall        
 ,SM.StrikePrice        
 ,SM.ExpirationDate        
 ,VT.SettlementDate             
 ,VT.GroupRefID        
 ,VT.AUECLocalDate        
 ,SM.Multiplier        
 ,SM.ISINSymbol        
 ,SM.CUSIPSymbol        
 ,SM.SEDOLSymbol        
 ,SM.ReutersSymbol        
 ,SM.BloombergSymbol        
 ,SM.CompanyName        
 ,SM.UnderlyingSymbol        
 ,SM.LeadCurrencyID        
 ,SM.LeadCurrency        
 ,SM.VsCurrencyID        
 ,SM.VsCurrency        
 ,SM.OSISymbol        
 ,SM.IDCOSymbol        
 ,SM.OpraSymbol        
 ,VT.FXRate        
 ,VT.FXConversionMethodOperator        
 ,VT.ProcessDate        
 ,VT.OriginalPurchaseDate        
 ,VT.AccruedInterest         
 ,VT.FXRate_Taxlot        
 ,VT.FXConversionMethodOperator_Taxlot          
 ,VT.TransactionType         
 ,VT.AlgorithmAcronym        
 ,VT.AlgorithmDesc        
 ORDER BY GroupRefID        
END        
ELSE        
BEGIN        
 SELECT VT.Level1AllocationID as EntityID        
 ,ISNULL(VT.FundID, 0) AS FundID            
 ,VT.SideID AS SideID        
 ,T_Side.Side AS Side        
 ,VT.Symbol as Symbol        
 ,VT.CounterPartyID as CounterPartyID       
 ,Cp.ShortName as CounterParty      
 ,Sum(VT.OrderQty)As AllocatedQty  
 ,VT.AvgPrice as AveragePrice        
 ,VT.CumQty as ExecutedQty        
 ,VT.Quantity as TotalQty        
 ,VT.ExchangeID as ExchangeID    
 ,T_Exchange.DisplayName      
 ,Currency.CurrencyID as CurrencyID        
 ,Currency.CurrencyName as CurrencyName        
 ,Currency.CurrencySymbol as CurrencySymbol        
 ,CF.FundName as FundName        
 ,CTPM.MappedName as FundMappedName        
 ,CTPM.FundAccntNo as FundAccntNo        
 ,CTPM.FundTypeID_FK as FundTypeID_FK        
 ,FT.FundTypeName as FundTypeName                                         
 ,SM.PutOrCall as PutOrCall        
 ,SM.StrikePrice as StrikePrice        
 ,SM.ExpirationDate as ExpirationDate        
 ,VT.SettlementDate as SettlementDate        
 ,sum(VT.Commission) as CommissionCharged        
 ,sum(VT.OtherBrokerFees) as OtherBrokerFees                                                                                               
 ,VT.GroupRefID as GroupRefID        
 ,'Allocated' AS TaxLotState        
 ,sum(ISNULL(VT.StampDuty, 0)) AS StampDuty        
 ,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy        
 ,Sum(ISNULL(ClearingFee, 0)) AS ClearingFee        
 ,Sum(ISNULL(TaxOnCommissions, 0)) AS TaxOnCommissions        
 ,Sum(ISNULL(MiscFees, 0)) AS MiscFees        
 ,VT.AUECLocalDate as TradeDate        
 ,SM.Multiplier as AssetMultiplier        
 ,SM.ISINSymbol as ISINSymbol        
 ,SM.CUSIPSymbol as CUSIPSymbol        
 ,SM.SEDOLSymbol as SEDOLSymbol        
 ,SM.ReutersSymbol as ReutersSymbol        
 ,SM.BloombergSymbol as BloombergSymbol        
 ,SM.CompanyName as FullSecurityName        
 ,SM.UnderlyingSymbol as UnderlyingSymbol        
 ,SM.LeadCurrencyID as LeadCurrencyID        
 ,SM.LeadCurrency as LeadCurrency        
 ,SM.VsCurrencyID as VsCurrencyID        
 ,SM.VsCurrency as VsCurrency        
 ,SM.OSISymbol as OSISymbol        
 ,SM.IDCOSymbol as IDCOSymbol        
 ,SM.OpraSymbol as OpraSymbol        
 ,VT.FXRate as ForexRate        
 ,VT.FXConversionMethodOperator as FXConversionMethodOperator        
 ,VT.ProcessDate as ProcessDate        
 ,VT.OriginalPurchaseDate as OriginalPurchaseDate        
 ,VT.AccruedInterest as AccruedInterest        
 ,'' AS Comment1        
 ,'' AS Comment2        
 ,VT.FXRate_Taxlot as FXRate_Taxlot        
 ,VT.FXConversionMethodOperator_Taxlot as FXConversionMethodOperator_Taxlot        
 ,sum(ISNULL(VT.SecFee, 0)) AS SecFee        
 ,sum(ISNULL(VT.OccFee, 0)) AS OccFee        
 ,sum(ISNULL(VT.OrfFee, 0)) AS OrfFee        
 ,sum(VT.ClearingBrokerFee) as ClearingBrokerFee        
 ,sum(VT.SoftCommission) as SoftCommissionCharged        
 ,VT.TransactionType as TransactionType        
 ,ISNULL(VT.AlgorithmAcronym,'') as AlgorithmAcronym        
 ,ISNULL(VT.AlgorithmDesc,'') as AlgorithmDesc        
 FROM #VT VT       
 INNER JOIN T_CompanyFunds as CF on CF.CompanyFundID=VT.FundID 
 INNER JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID              
 INNER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID        
 INNER JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID            
 INNER JOIN V_SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol 
 LEFT JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID        
 LEFT JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK  
 LEFT OUTER JOIN T_CounterParty as CP on CP.CounterPartyID=vt.CounterPartyID         
 WHERE datediff(d, (        
    CASE         
     WHEN @dateType = 1        
      THEN VT.AUECLocalDate        
     ELSE VT.ProcessDate        
     END        
    ), @inputdate) = 0        
  AND VT.FundID IN (        
   SELECT FundID        
   FROM @Fund        
   )        
  AND VT.AUECID IN (        
   SELECT AUECID        
   FROM @AUECID        
   )       
       
 GROUP BY  
VT.FundID        
,VT.Level1AllocationID     
,VT.SideID       
,T_Side.Side        
,VT.Symbol        
,VT.CounterPartyID        
,CP.ShortName          
,VT.AvgPrice        
,VT.CumQty        
,VT.Quantity              
,VT.ExchangeID      
,T_Exchange.DisplayName    
,Currency.CurrencyID        
,Currency.CurrencyName        
,Currency.CurrencySymbol        
,CTPM.MappedName        
,CF.FundName        
,CTPM.FundAccntNo        
,CTPM.FundTypeID_FK        
,FT.FundTypeName        
,SM.PutOrCall        
,SM.StrikePrice        
,SM.ExpirationDate        
,VT.SettlementDate             
,VT.GroupRefID        
,VT.AUECLocalDate        
,SM.Multiplier        
,SM.ISINSymbol        
,SM.CUSIPSymbol        
,SM.SEDOLSymbol        
,SM.ReutersSymbol        
,SM.BloombergSymbol        
,SM.CompanyName        
,SM.UnderlyingSymbol        
,SM.LeadCurrencyID        
,SM.LeadCurrency        
,SM.VsCurrencyID        
,SM.VsCurrency        
,SM.OSISymbol        
,SM.IDCOSymbol        
,SM.OpraSymbol        
,VT.FXRate        
,VT.FXConversionMethodOperator        
,VT.ProcessDate        
,VT.OriginalPurchaseDate        
,VT.AccruedInterest         
,VT.FXRate_Taxlot        
,VT.FXConversionMethodOperator_Taxlot          
,VT.TransactionType         
,VT.AlgorithmAcronym        
,VT.AlgorithmDesc        
 ORDER BY GroupRefID       
END 

Drop Table #VT