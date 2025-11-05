                                                                                                                                         
/***********************************************        
Created by: Suraj nataraj        
Script Date: 09/08/2015        
Desc: For retrieving old and new data of amended trades        
http://jira.nirvanasolutions.com:8080/browse/PRANA-10893        
*************************************************/        
                                                                                                                                                               
        
ALTER PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_Amend]               
(                  
 @thirdPartyID INT                  
 ,@companyFundIDs VARCHAR(max)                  
 ,@inputDate DATETIME                  
 ,@companyID INT                  
 ,@auecIDs VARCHAR(max)                  
 ,@TypeID INT                  
 ,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                      
 @dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                      
 ,@fileFormatID INT                  
 )                  
AS                  
          
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
                  
Create Table #SecMasterData                  
(                  
TickerSymbol Varchar(200)                  
,PutOrCall Varchar(10)                  
,StrikePrice INT                  
,ExpirationDate DateTime                  
,Multiplier INT                  
,ISINSymbol Varchar(20)                  
,CUSIPSymbol Varchar(50)                  
,SEDOLSymbol Varchar(50)                  
,ReutersSymbol Varchar(200)                  
,BloombergSymbol Varchar(200)                  
,CompanyName Varchar(500)                  
,UnderlyingSymbol Varchar(100)                  
,LeadCurrencyID INT                  
,LeadCurrency Varchar(3)                  
,VsCurrencyID INT                  
,VsCurrency Varchar(3)                  
,OSISymbol Varchar(21)                  
,IDCOSymbol Varchar(22)                  
,OpraSymbol Varchar(21)                  
,Coupon INT                  
,IssueDate DateTime                  
,FirstCouponDate DateTime                  
,CouponFrequencyID INT                  
,AccrualBasisID INT                  
,BondTypeID INT                  
,AssetName Varchar(100)                  
,SecurityTypeName Varchar(100)                  
,SectorName Varchar(100)                
,SubSectorName Varchar(100)                  
,CountryName Varchar(100)                  
,Analyst VARCHAR(500)                  
,CountryOfRisk VARCHAR(500)                  
,CustomUDA1 VARCHAR(500)                  
,CustomUDA2 VARCHAR(500)                  
,CustomUDA3 VARCHAR(500)                  
,CustomUDA4 VARCHAR(500)                  
,CustomUDA5 VARCHAR(500)                  
,CustomUDA6 VARCHAR(500)                  
,CustomUDA7 VARCHAR(500)                  
,Issuer VARCHAR(500)                  
,LiquidTag VARCHAR(500)                  
,MarketCap VARCHAR(500)                  
,Region VARCHAR(500)                  
,RiskCurrency VARCHAR(500)                  
,UCITSEligibleTag VARCHAR(500)                  
)                  
                  
Insert INTO #SecMasterData                  
SELECT TickerSymbol                  
,PutOrCall                  
,StrikePrice                  
,ExpirationDate                  
,Multiplier                  
,ISINSymbol                  
,CUSIPSymbol                  
,SEDOLSymbol                  
,ReutersSymbol                  
,BloombergSymbol                  
,CompanyName                  
,UnderlyingSymbol                  
,LeadCurrencyID                  
,LeadCurrency                  
,VsCurrencyID                  
,VsCurrency                  
,OSISymbol                  
,IDCOSymbol                  
,OpraSymbol                  
,Coupon                  
,IssueDate                  
,FirstCouponDate                  
,CouponFrequencyID                  
,AccrualBasisID                  
,BondTypeID                  
,AssetName                 
,SecurityTypeName                  
,SectorName                  
,SubSectorName                  
,CountryName                  
,ISNULL(Analyst,'')                  
,ISNULL(CountryOfRisk,'')                  
,ISNULL(CustomUDA1,'')                  
,ISNULL(CustomUDA2,'')                  
,ISNULL(CustomUDA3,'')                  
,ISNULL(CustomUDA4,'')                  
,ISNULL(CustomUDA5,'')                  
,ISNULL(CustomUDA6,'')                  
,ISNULL(CustomUDA7,'')                  
,ISNULL(Issuer,'')                  
,ISNULL(LiquidTag,'')                  
,ISNULL(MarketCap,'')                  
,ISNULL(Region,'')                  
,ISNULL(RiskCurrency,'')                  
,ISNULL(UCITSEligibleTag,'')                  
 from V_SecMasterData SM                  
left OUTER join V_UDA_DynamicUDA UDA on UDA.Symbol_PK = SM.Symbol_PK                  
                  
CREATE TABLE #VT                   
(                  
 TaxLotID VARCHAR(50)                  
,Level2AllocationID VARCHAR(50)                  
 ,FundID INT                  
 ,OrderTypeTagValue VARCHAR(3)                  
 ,SideID VARCHAR(5)                  
 ,Symbol VARCHAR(100)                  
 ,CounterPartyID INT                  
 ,VenueID INT                  
 ,OrderQty FLOAT                  
 ,AvgPrice FLOAT                  
 ,CumQty FLOAT                  
 ,Quantity FLOAT                  
 ,AUECID INT                  
 ,AssetID INT                  
 ,UnderlyingID INT                  
 ,ExchangeID INT                  
 ,CurrencyID INT                  
 ,Level1AllocationID VARCHAR(50)                  
 ,Level2Percentage FLOAT                  
 ,--Percentage,                                                                             
 TaxLotQty FLOAT                  
 ,IsBasketGroup VARCHAR(20)                  
 ,SettlementDate DATETIME                  
 ,Commission FLOAT                  
 ,OtherBrokerFees FLOAT                  
 ,GroupRefID INT                  
 ,TaxlotState VARCHAR(50)                  
 ,TaxlotFIXState VARCHAR(50)                  
 ,TaxlotFIXAckState VARCHAR(50)                  
 ,StampDuty FLOAT                  
 ,TransactionLevy FLOAT                  
 ,ClearingFee FLOAT                  
 ,TaxOnCommissions FLOAT                  
 ,MiscFees FLOAT                  
 ,AUECLocalDate DATETIME                  
 ,Level2ID INT                  
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
 ,TradeAttribute1 VARCHAR(200)                  
 ,TradeAttribute2 VARCHAR(200)                  
 ,TradeAttribute3 VARCHAR(200)                  
 ,TradeAttribute4 VARCHAR(200)                  
 ,TradeAttribute5 VARCHAR(200)                  
 ,TradeAttribute6 VARCHAR(200)                  
 ,Description VARCHAR(200)                  
 ,IsOldData BIT                  
 ,SecFee FLOAT                  
 ,OccFee FLOAT                  
 ,OrfFee FLOAT                  
 ,ClearingBrokerFee FLOAT         
 ,SoftCommission FLOAT                  
 ,TransactionType VARCHAR(200)            
 -------------- ParentSiblingInfo ---------------------------------------                          
 ,parentSiblingInfo NVARCHAR(max)                  
 ,SettlCurrency INT                  
                 
 ,ChangeType INT 
, TaxlotStateID int                 
                  
-------------- ParentSiblingInfo ---------------------------------------                    
)                  
                        
INSERT INTO #VT                  
SELECT VT.Level1AllocationID AS TaxlotID                  
,VT.TaxLotID as Level2AllocationID                  
 ,ISNULL(VT.FundID, 0) AS FundID                  
 ,VT.OrderTypeTagValue                  
 ,VT.OrderSideTagValue AS SideID                  
 ,VT.Symbol                  
 ,VT.CounterPartyID                  
 ,VT.VenueID                  
 ,(VT.TaxLotQty) AS OrderQty                  
 ,--AllocatedQty                                                                                                                                                          
 VT.AvgPrice                  
 ,VT.CumQty                  
 ,--ExecutedQty                                                                                              
 VT.Quantity                  
 ,--TotalQty                                                                                                               
 VT.AUECID                  
 ,VT.AssetID                  
 ,VT.UnderlyingID                  
 ,VT.ExchangeID                  
 ,VT.CurrencyID                  
 ,VT.Level1AllocationID AS Level1AllocationID                  
 ,(VT.Level2Percentage)                  
 ,--Percentage,                                                                                               
 (VT.TaxLotQty)                  
 ,'' AS IsBasketGroup                  
 ,VT.SettlementDate                  
 ,VT.Commission                  
 ,VT.OtherBrokerFees                  
 ,VT.GroupRefID                  
 ,PB.TaxLotState AS TaxlotState                  
 ,PB.TaxLotFIXState AS TaxlotFIXState                  
 ,PB.TaxlotFIXAckState AS TaxlotFIXAckState                  
    
 ,ISNULL(VT.StampDuty, 0) AS StampDuty                  
 ,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy                  
 ,ISNULL(ClearingFee, 0) AS ClearingFee                  
 ,ISNULL(TaxOnCommissions, 0) AS TaxOnCommissions                  
 ,ISNULL(MiscFees, 0) AS MiscFees                  
 ,VT.AUECLocalDate                  
 ,0 AS Level2ID                  
 ,PB.PBID                  
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
 ,VT.TradeAttribute1                  
 ,VT.TradeAttribute2                  
 ,VT.TradeAttribute3                  
 ,VT.TradeAttribute4                  
 ,VT.TradeAttribute5                  
 ,VT.TradeAttribute6                  
 ,VT.Description                  
 ,CASE                   
  WHEN PB.FileFormatID = 0                  
   THEN 1                  
  ELSE 0                  
  END AS IsOldData                  
 ,ISNULL(VT.SecFee, 0) AS SecFee                  
 ,ISNULL(VT.OccFee, 0) AS OccFee                  
 ,ISNULL(VT.OrfFee, 0) AS OrfFee                  
 ,VT.ClearingBrokerFee                  
 ,VT.SoftCommission                  
 ,VT.TransactionType                  
 ,                  
 -------------- ParentSiblingInfo ---------------------------------------                          
CASE 
		 WHEN @IsFillsRequired = 0 THEN '<parentSiblingInfo></parentSiblingInfo>' 
	     WHEN @IsFillsRequired = 1 THEN  (                
  SELECT DISTINCT VT.GroupID AS groupId                  
   ,parent.IsManualGroup                  
   ,(                  
    SELECT DISTINCT tradedOrder.CLOrderID                  
     ,tradedOrder.ParentClOrderID                  
     ,tradedOrder.ClientOrderID                  
     ,tradedOrder.ParentClientOrderID                  
     ,tradedOrder.NirvanaMsgType                  
     ,(                  
      SELECT DISTINCT fill.ExecutionID AS ExecutionID                  
       ,CONVERT(VARCHAR(100), CAST(fill.LastShares AS DECIMAL(30, 0))) AS LastShares                  
       ,CONVERT(VARCHAR(100), CAST(fill.LastPx AS DECIMAL(30, 6))) AS LastPrice                  
       --Added by Suraj Nataraj for correct fill order http://jira.nirvanasolutions.com:8080/browse/PRANA-7335                  
       ,fill.Fills_PK as FillID                  
      FROM T_Fills AS fill                  
      WHERE tradedOrder.CLOrderID = fill.ClOrderID                  
       AND fill.OrderStatus IN (                  
        '1'                  
        ,'2'                  
        ) ORDER BY fill.Fills_PK                  
      FOR XML AUTO                  
       ,ELEMENTS                  
      ) AS fills                  
    FROM [T_TradedOrders] AS tradedOrder                  
    WHERE parent.GroupID = tradedOrder.GroupID                  
    FOR XML AUTO                  
     ,ELEMENTS                  
    ) AS siblings                  
  FROM [T_Group] AS parent                  
  WHERE parent.GroupID = VT.GroupID                  
  FOR XML AUTO                  
   ,ELEMENTS                  
   ,ROOT('parentSiblingInfo')                  
  )  END AS parentSiblingInfo                  
 -------------- ParentSiblingInfo ---------------------------------------                          
 ,VT.SettlCurrency_Taxlot AS SettlCurrency                  
                  
,VT.ChangeType As ChangeType
,PB.TaxlotState  as  TaxlotStateID                  
                  
FROM V_TaxLots VT                  
INNER JOIN T_PBWiseTaxlotState PB ON PB.TaxlotID = VT.TaxlotID                  
inner join #SecMasterData SM on VT.Symbol = SM.TickerSymbol                  
                  
WHERE (                  
  PB.fileFormatID = 0                  
  OR @fileFormatID = FileFormatID                  
  )                  
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
 or TransactionSource=13             
  )                  
                 
UNION ALL                  
                  
SELECT TDT.Level1AllocationID AS TaxlotID                  
,TDT.TaxLotID as Level2AllocationID                  
 ,ISNULL(TDT.FundID, 0) AS FundID                  
 ,TDT.OrderTypeTagValue                  
 ,TDT.OrderSideTagValue AS SideID                  
 ,TDT.Symbol                  
 ,TDT.CounterPartyID                  
 ,TDT.VenueID                  
 ,(TDT.TaxLotQty) AS OrderQty                  
 ,--AllocatedQty                                                                                                                                                    
 TDT.AvgPrice                  
 ,TDT.CumQty                  
 ,--ExecutedQty                                                                                            
 TDT.Quantity                  
 ,--TotalQty                                                                                                                                                                    
 TDT.AUECID                  
 ,TDT.AssetID                  
 ,TDT.UnderlyingID                  
 ,TDT.ExchangeID                  
 ,TDT.CurrencyID                  
 ,TDT.Level1AllocationID AS Level1AllocationID                  
 ,(TDT.Level2Percentage)                  
 ,--Percentage,                                                                                 
 (TDT.TaxLotQty)                  
 ,' ' AS IsBasketGroup                  
 ,--IsBasketGroup,                                                                                                                                                                     
 TDT.SettlementDate                  
 ,(TDT.Commission)                  
 ,(TDT.OtherBrokerFees)                  
 ,TDT.GroupRefID                  
 ,TDT.TaxLotState                  
 ,TDT.TaxLotFIXState                  
 ,TDT.TaxLotFIXAckState                  
 ,(ISNULL(TDT.StampDuty, 0)) AS StampDuty                  
 ,(ISNULL(TDT.TransactionLevy, 0)) AS TransactionLevy                  
 ,(ISNULL(TDT.ClearingFee, 0)) AS ClearingFee                  
 ,(ISNULL(TDT.TaxOnCommissions, 0)) AS TaxOnCommissions                  
 ,(ISNULL(TDT.MiscFees, 0)) AS MiscFees                  
 ,TDT.AUECLocalDate                  
 ,0 AS Level2ID            
 ,TDT.PBID                  
 ,TDT.FXRate AS FXRate                  
 ,TDT.FXConversionMethodOperator AS FXConversionMethodOperator                  
 ,'Yes' AS FromDeleted                  
 ,TDT.ProcessDate                  
 ,TDT.OriginalPurchaseDate                  
 ,TDT.AccruedInterest                  
 ,TDT.BenchMarkRate                  
 ,TDT.Differential                  
 ,TDT.SwapDescription                  
 ,TDT.DayCount                  
 ,TDT.FirstResetDate                  
 ,TDT.IsSwapped               
 ,TDT.FXRate_Taxlot                  
 ,TDT.FXConversionMethodOperator_Taxlot                  
 ,TDT.LotID                  
 ,TDT.ExternalTransID                  
 ,TDT.TradeAttribute1                  
 ,TDT.TradeAttribute2                  
 ,TDT.TradeAttribute3                  
 ,TDT.TradeAttribute4                  
 ,TDT.TradeAttribute5                  
 ,TDT.TradeAttribute6                  
 ,TDT.Description                  
 ,CASE                   
  WHEN TDT.FileFormatID = 0                  
   THEN 1                  
  ELSE 0                  
  END AS IsOldData            
 ,(ISNULL(TDT.SecFee, 0)) AS SecFee                  
 ,(ISNULL(TDT.OccFee, 0)) AS OccFee                  
 ,(ISNULL(TDT.OrfFee, 0)) AS OrfFee                  
 ,(TDT.ClearingBrokerFee)                  
 ,(TDT.SoftCommission)                  
 ,TDT.TransactionType                  
 ,                  
 -------------- ParentSiblingInfo ---------------------------------------                          
 '<parentSiblingInfo></parentSiblingInfo>' AS parentSiblingInfo                  
 -------------- ParentSiblingInfo ---------------------------------------                         
 ,TDT.SettlCurrency                  
                 
,'3' AS ChangeType
,TDT.TaxlotState as TaxlotStateID                  
                  
FROM T_DeletedTaxLots TDT                  
inner join #SecMasterData SM on SM.TickerSymbol = TDT.Symbol                  
                  
WHERE (FileFormatID = @fileFormatID OR FileFormatID = 0) And TDT.TaxlotState = 3              
                  
CREATE table #TaxlotsDates                  
(                  
TaxlotId nvarchar(20)                  
)                  
                  
create table #T_TradeAuditAction                  
(                  
ActionType int Primary Key,                  
ActionName nvarchar(50)                  
)                  
                  
insert INTO #T_TradeAuditAction VALUES (1,'REALLOCATE')                  
insert INTO #T_TradeAuditAction VALUES (2,'UNALLOCATE')                  
insert INTO #T_TradeAuditAction VALUES (3,'GROUP')                  
insert INTO #T_TradeAuditAction VALUES (4,'UNGROUP')                  
insert INTO #T_TradeAuditAction VALUES (5,'DELETE')                  
insert INTO #T_TradeAuditAction VALUES (6,'TradeDate_Changed')                  
insert INTO #T_TradeAuditAction VALUES (7,'OrderSide_Changed')                  
insert INTO #T_TradeAuditAction VALUES (8,'Counterparty_Changed')                  
insert INTO #T_TradeAuditAction VALUES (9,'ExecutedQuantity_Changed')                  
insert INTO #T_TradeAuditAction VALUES (10,'AvgPrice_Changed')                  
insert INTO #T_TradeAuditAction VALUES (11,'SettlementDate_Changed')                  
insert INTO #T_TradeAuditAction VALUES (12,'FxRate_Changed')                  
insert INTO #T_TradeAuditAction VALUES (13,'Commission_Changed')                  
insert INTO #T_TradeAuditAction VALUES (14,'OtherBrokerFees_Changed')                  
insert INTO #T_TradeAuditAction VALUES (15,'StampDuty_Changed')                  
insert INTO #T_TradeAuditAction VALUES (16,'TransactionLevy_Changed')                  
insert INTO #T_TradeAuditAction VALUES (17,'ClearingFee_Changed')                  
insert INTO #T_TradeAuditAction VALUES (18,'TaxOnCommission_Changed')                  
insert INTO #T_TradeAuditAction VALUES (19,'MiscFees_Changed')                  
insert INTO #T_TradeAuditAction VALUES (20,'Venue_Changed')                  
insert INTO #T_TradeAuditAction VALUES (21,'FxConversionMethodOperator_Changed')         
insert INTO #T_TradeAuditAction VALUES (22,'ProcessDate_Changed')                  
insert INTO #T_TradeAuditAction VALUES (23,'OriginalPurchaseDate_Changed')                  
insert INTO #T_TradeAuditAction VALUES (24,'Description_Changed')                  
insert INTO #T_TradeAuditAction VALUES (25,'AccruedInterest_Changed')                  
insert INTO #T_TradeAuditAction VALUES (26,'UnderlyingDelta_Changed')                  
insert INTO #T_TradeAuditAction VALUES (27,'LotId_Changed')                  
insert INTO #T_TradeAuditAction VALUES (28,'CommissionAmount_Changed')                  
insert INTO #T_TradeAuditAction VALUES (29,'CommissionRate_Changed')                  
insert INTO #T_TradeAuditAction VALUES (30,'TradeAttribute1_Changed')                  
insert INTO #T_TradeAuditAction VALUES (31,'TradeAttribute2_Changed')                  
insert INTO #T_TradeAuditAction VALUES (32,'TradeAttribute3_Changed')                  
insert INTO #T_TradeAuditAction VALUES (33,'TradeAttribute4_Changed')                  
insert INTO #T_TradeAuditAction VALUES (34,'TradeAttribute5_Changed')                  
insert INTO #T_TradeAuditAction VALUES (35,'TradeAttribute6_Changed')        
insert INTO #T_TradeAuditAction VALUES (36,'ExternalTransId_Changed')                  
insert INTO #T_TradeAuditAction VALUES (37,'TradeEdited')                  
insert INTO #T_TradeAuditAction VALUES (38,'SecFee_Changed')                  
insert INTO #T_TradeAuditAction VALUES (39,'OccFee_Changed')                  
insert INTO #T_TradeAuditAction VALUES (40,'OrfFee_Changed')                  
insert INTO #T_TradeAuditAction VALUES (41,'ClearingBrokerFee_Changed')                  
insert INTO #T_TradeAuditAction VALUES (42,'SoftCommission_Changed')                  
insert INTO #T_TradeAuditAction VALUES (43,'SoftCommissionAmount_Changed')                  
insert INTO #T_TradeAuditAction VALUES (44,'SoftCommissionRate_Changed')                  
insert INTO #T_TradeAuditAction VALUES (45,'TransactionType_Changed')                  
insert INTO #T_TradeAuditAction VALUES (46,'InternalComments_Changed')                  
insert INTO #T_TradeAuditAction VALUES (47,'SettlCurrency_Changed')                  
               
insert INTO #T_TradeAuditAction VALUES (48,'OptionPremiumAdjustment_Changed')                  
                  
                  
insert into #TaxlotsDates                  
SELECT distinct TA.TaxlotId from T_TradeAudit TA                   
inner JOIN #T_TradeAuditAction TAA ON TA.Action=TAA.ActionType                  
inner join T_ThirdPartyFFGenerationDate TDG ON TDG.TaxlotID = TA.TaxlotId                  
where DATEDIFF(d,TDG.GenerationDate,ta.ActionDate)>=0                  
 and TAA.ActionName not in ('REALLOCATE','UNALLOCATE')                  
                  
                  
select                   
VT.TaxlotID AS TaxlotID                  
,VT.Level1AllocationID as EntityID                  
  ,ISNULL(VT.FundID, 0) AS FundID                  
,ISNULL(F.FundName,'') as FundName                  
  ,ISNULL(T_OrderType.OrderTypesID, 0) AS OrderTypesID                  
  ,ISNULL(T_OrderType.OrderTypes, 'Multiple') AS OrderTypes                  
  ,VT.SideID                  
  ,T_Side.Side AS Side                  
  ,VT.Symbol                  
  ,VT.CounterPartyID                  
,C.Shortname as CounterParty                  
  ,VT.VenueID                  
  ,Sum(VT.TaxLotQty) AS OrderQty                  
  ,--AllocatedQty                                                                               
  VT.AvgPrice                  
  ,VT.CumQty                  
  ,--ExecutedQty                                                                                                                                                                    
  VT.Quantity                  
  ,--TotalQty                                                                             
  VT.AUECID                  
  ,T_Asset.AssetName as Asset                  
  ,VT.UnderlyingID                  
  ,VT.ExchangeID                
,E.DisplayName as Exchange                
  ,Currency.CurrencyID                  
  ,Currency.CurrencyName                  
  ,Currency.CurrencySymbol                  
  ,CTPM.MappedName                  
  ,CTPM.FundAccntNo                  
  ,CTPM.FundTypeID_FK                  
  ,FT.FundTypeName                  
  ,VT.Level1AllocationID AS Level1AllocationID                  
  ,Sum(VT.Level2Percentage) AS Level2Percentage                  
  ,--Percentage,                                 
  Sum(VT.TaxLotQty)                  
  ,'' AS IsBasketGroup                  
  ,SM.PutOrCall                  
  ,SM.StrikePrice                  
  ,SM.ExpirationDate                  
  ,VT.SettlementDate                  
  ,Sum(VT.Commission) as CommissionCharged                  
  ,Sum(VT.OtherBrokerFees) as OtherBrokerFees                  
  ,T_ThirdPartyType.ThirdPartyTypeID                  
  ,T_ThirdPartyType.ThirdPartyTypeName                  
  --,CTPFD.CompanyIdentifier                  
  --,0 AS SecFee                  
  --,ISNULL(T_CounterPartyVenue.DisplayName, '') AS CVName                  
  --,ISNULL(T_CompanyThirdPartyCVIdentifier.CVIdentifier, '') CVIdentifier                  
  --,T_CompanyCounterPartyVenues.CompanyCounterPartyCVID AS CompanyCounterPartyCVID                  
  ,VT.GroupRefID                
,VT.GroupRefID as PBUniqueID                
  ,case                    
  when VT.TaxLotState = '0'                    
  then 'Allocated'                         
  when VT.TaxLotState = '1'                    
  then 'Sent'                               
  when VT.TaxLotState = '2'                  
  then 'Amemded'                             
  when VT.TaxLotState = '3'                  
  then 'Deleted'                             
  when VT.TaxLotState = '4'                  
 then 'Ignored'                             
  end                                        
  as TaxLotState                  
  ,VT.TaxLotFIXState As TaxLotFIXStateID                 
  ,VT.TaxLotFIXAckState As TaxLotFIXAckStateID                
  ,Sum(ISNULL(VT.StampDuty, 0)) AS StampDuty                  
  ,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy                  
  ,Sum(ISNULL(VT.ClearingFee, 0)) AS ClearingFee                  
  ,Sum(ISNULL(VT.TaxOnCommissions, 0)) AS TaxOnCommissions                  
  ,Sum(ISNULL(VT.MiscFees, 0)) AS MiscFees                  
  ,VT.AUECLocalDate as TradeDate                  
  ,SM.Multiplier as AssetMultiplier                  
  ,0 AS Level2ID                  
  ,SM.ISINSymbol as ISIN                
  ,SM.CUSIPSymbol as CUSIP              
 ,SM.SEDOLSymbol as SEDOL              
  ,SM.ReutersSymbol as RIC                 
  ,SM.BloombergSymbol as BBCode               
  ,SM.CompanyName                  
  ,SM.UnderlyingSymbol                  
  ,SM.LeadCurrencyID                  
  ,SM.LeadCurrency as LeadCurrencyName                  
  ,SM.VsCurrencyID                  
  ,SM.VsCurrency as VsCurrencyName                  
  ,SM.OSISymbol as OSIOptionSymbol               
  ,SM.IDCOSymbol                  
  ,SM.OpraSymbol                  
  ,VT.FXRate                  
  ,VT.FXConversionMethodOperator                  
  ,VT.FromDeleted                  
  ,VT.ProcessDate                  
  ,VT.OriginalPurchaseDate                  
  ,VT.AccruedInterest                  
  ,                  
  -- Reserved for future use                                                          
  '' AS Comment1                  
  ,'' AS Comment2                  
  ,                  
  --FixedIncome Members                                                        
  SM.Coupon                  
  ,SM.IssueDate                  
  ,SM.FirstCouponDate                  
  ,SM.CouponFrequencyID                  
  ,Sm.AccrualBasisID                  
  ,SM.BondTypeID                  
                    
  --Swap Parameters                                   
  ,isnull(VT.BenchMarkRate,0) as BenchMarkRate                  
  ,isnull(VT.Differential,0) as Differential                  
  ,isnull(VT.SwapDescription,'') as SwapDescription                  
  ,isnull(VT.DayCount,0) as DayCount                  
  ,isnull(VT.FirstResetDate,'1800-01-01') as FirstResetDate                  
  ,isnull(VT.IsSwapped,0) as IsSwapped                  
  ,T_Country.CountryName AS CountryName                  
  ,CASE                   
   WHEN VT.AssetID = 11                  
    AND SM.ExpirationDate <> '1800-01-01 00:00:00.000'                  
    THEN dbo.AdjustBusinessDays(SM.ExpirationDate, - 1, @FXForwardAuecID)                  
   ELSE ''                  
   END AS RerateDateBusDayAdjusted1                  
  ,CASE                   
   WHEN VT.AssetID = 11                  
    AND SM.ExpirationDate <> '1800-01-01 00:00:00.000'                  
    THEN dbo.AdjustBusinessDays(SM.ExpirationDate, - 2, @FXForwardAuecID)                  
   ELSE ''                  
   END AS RerateDateBusDayAdjusted2                  
  ,VT.FXRate_Taxlot                  
  ,VT.FXConversionMethodOperator_Taxlot                  
  ,VT.LotID                  
  ,VT.ExternalTransID                  
  ,VT.TradeAttribute1                  
  ,VT.TradeAttribute2                  
  ,VT.TradeAttribute3                  
  ,VT.TradeAttribute4                  
  ,VT.TradeAttribute5                  
  ,VT.TradeAttribute6                  
  ,SM.AssetName AS UDAAssetName                  
  ,SM.SecurityTypeName AS UDASecurityTypeName                  
  ,SM.SectorName AS UDASectorName                  
  ,SM.SubSectorName AS UDASubSectorName             
  ,SM.CountryName AS UDACountryName                  
  ,VT.Description                  
  ,dbo.AdjustBusinessDays(SM.ExpirationDate, 2, VT.AUECID) AS DeliveryDate                  
  ,Sum(ISNULL(VT.SecFee, 0)) AS SecFee                  
  ,Sum(ISNULL(VT.OccFee, 0)) AS OccFee                  
  ,Sum(ISNULL(VT.OrfFee, 0)) AS OrfFee                  
  ,Sum(VT.ClearingBrokerFee) as ClearingBrokerFee                  
  ,Sum(VT.SoftCommission) as SoftCommissionCharged                  
  ,VT.TransactionType                  
  ,                  
  -------------- ParentSiblingInfo ---------------------------------------                          
  VT.parentSiblingInfo                  
  -------------- ParentSiblingInfo ---------------------------------------                          
  ,COALESCE(TC.CurrencySymbol, 'None') AS SettlCurrency                  
    
,VT.ChangeType As ChangeType                  
----------Dynamic UDA---------------                  
,ISNULL(Analyst,'') as Analyst             
,ISNULL(CountryOfRisk,'') as CountryOfRisk                  
,ISNULL(CustomUDA1,'') as CustomUDA1                  
,ISNULL(CustomUDA2,'') as CustomUDA2                  
,ISNULL(CustomUDA3,'') as CustomUDA3                  
,ISNULL(CustomUDA4,'') as CustomUDA4                  
,ISNULL(CustomUDA5,'') as CustomUDA5                  
,ISNULL(CustomUDA6,'') as CustomUDA6                  
,ISNULL(CustomUDA7,'') as CustomUDA7                  
,ISNULL(Issuer,'') as Issuer                  
,ISNULL(LiquidTag,'') as LiquidTag                  
,ISNULL(MarketCap,'') as MarketCap                  
,ISNULL(Region,'') as Region                  
,ISNULL(RiskCurrency,'') as RiskCurrency                  
,ISNULL(UCITSEligibleTag,'') as UCITSEligibleTag                  
--,taa.ActionName                  
--,ta.OriginalValue                  
,isnull(TGD.TradeDate,'1800-01-01') as OldTradeDate,                  
isnull(S.Side,'') as OldSide,                
isnull(TGD.CounterpartyID,0) as OldCounterpartyID,                  
isnull(OldC.ShortName,'') as OldCounterparty,                
isnull(TGD.ExecutedQuantity,0) as OldExecutedQuantity,                  
ISNULL(TGD.AvgPrice,0) as OldAvgPrice,                  
isnull(TGD.SettlmentDate,'1800-01-01') as OldSettlementDate,                  
isnull(TGD.FXRate,0) as OldFXRate,                  
ISNULL(TGD.Commission,0) as OldCommission,                  
ISNULL(TGD.OtherBrokerFees,0) as OldOtherBrokerFees,                  
ISNULL(TGD.StampDuty,0) as OldStampDuty,                  
ISNULL(TGD.TransactionLevy,0) as OldTransactionLevy,                  
ISNULL(TGD.ClearingFee,0) as OldClearingFee,                  
ISNULL(TGD.MiscFees,0) as OldMiscFees,                  
ISNULL(TGD.VenueID,0) as OldVenueID,                  
TGD.FXConversionMethodOperator as OldFXConversionMethodOperator,                  
ISNULL(TGD.ProcessDate,'1800-01-01') as OldProcessDate,          ISNULL(TGD.OriginalPurchaseDate,'1800-01-01') as OldOriginalPurchaseDate,                  
TGD.Description as OldDescription,                  
ISNULL(TGD.AccruedInterest,0) as OldAccruedInterest,                  
ISNULL(TGD.UnderlyingDelta,0) as OldUnderlyingDelta,                  
TGD.LotID as OldLotID,                  
ISNULL(TGD.CommissionAmount,0) as OldCommissionAmount,                  
ISNULL(TGD.CommissionRate,0) as OldCommissionRate,                  
ISNULL(TGD.SecFee,0) as OldSecFee,                  
ISNULL(TGD.OccFee,0) as OldOccFee,                  
ISNULL(TGD.OrfFee,0) as OldOrfFee,                  
ISNULL(TGD.ClearingBrokerFee,0) as OldClearingBrokerFee,                  
ISNULL(TGD.SoftCommission,0) as OldSoftCommission,                  
ISNULL(TGD.SoftCommissionAmount,0) as OldSoftCommissionAmount,                  
TGD.TransactionType as OldTransactionType,                  
ISNULL(OTC.CurrencySymbol,'') as OldSettlCurrency,                  
                  
isnull(TGD.GenerationDate,'1800-01-01') as GenerationDate,                
isnull(TGD.AmendTaxLotId1,'') as AmendTaxLotId1,                
isnull(TGD.AmendTaxLotId2, '') as AmendTaxLotId2,        
isnull(TGD.TaxOnCommissions,0) as OldTaxOnCommissions,
VT.TaxlotStateID as TaxlotStateID                 
 from #VT VT                  
left JOIN #TaxlotsDates td on td.TaxlotId = VT.Level2AllocationID                  
left JOIN T_ThirdPartyFFGenerationDate TGD on TGD.TaxLotID = VT.Level2AllocationID                  
and TGD.fileFormatID = @fileFormatID                  
and TGD.thirdPartyID = @thirdPartyID                  
--left JOIN T_TradeAudit ta on ta.TaxlotId = td.TaxlotId and td.Date = ta.ActionDate                  
--left JOIN T_TradeAuditAction taa on ta.Action = taa.ActionType                  
inner JOIN T_Asset on VT.AssetID = T_Asset.AssetID                  
LEFT JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID                  
left JOIN T_CompanyFunds F ON F.CompanyFundID = VT.FundID                  
left join T_CounterParty C on C.CounterPartyID = VT.CounterPartyID                 
LEFT JOIN T_Exchange E on E.ExchangeID = VT.ExchangeID                 
left join T_CounterParty OldC on OldC.CounterPartyID = TGD.CounterPartyID                
 LEFT JOIN T_FundType AS FT ON FT.FundTypeID = CTPM.FundTypeID_FK                  
 LEFT JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID                  
 LEFT JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency                  
 LEFT JOIN T_Currency AS OTC ON OTC.CurrencyID = TGD.SettlCurrency                  
 LEFT JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID                  
left JOIN T_Side S on S.SideTagValue = TGD.SideID                
 LEFT JOIN T_Exchange ON dbo.T_Exchange.ExchangeID = VT.ExchangeID                  
 LEFT JOIN T_Country ON dbo.T_Country.CountryID = T_Exchange.Country                  
 LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue                  
 INNER JOIN dbo.T_CompanyThirdParty ON T_CompanyThirdParty.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK                  
 INNER JOIN dbo.T_ThirdParty ON T_ThirdParty.ThirdPartyId = T_CompanyThirdParty.ThirdPartyId                  
  AND T_CompanyThirdParty.CompanyID = @companyID                  
 INNER JOIN dbo.T_ThirdPartyType ON T_ThirdPartyType.ThirdPartyTypeId = T_ThirdParty.ThirdPartyTypeID      
 LEFT JOIN dbo.T_CompanyThirdPartyFlatFileSaveDetails CTPFD ON CTPFD.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK                  
 LEFT JOIN T_CounterPartyVenue ON T_CounterPartyVenue.CounterPartyID = VT.CounterPartyID                  
  AND T_CounterPartyVenue.VenueID = VT.VenueID                  
 LEFT OUTER JOIN T_CompanyCounterPartyVenues ON T_CompanyCounterPartyVenues.CounterPartyVenueID = T_CounterPartyVenue.CounterPartyVenueID                  
  AND T_CompanyCounterPartyVenues.CompanyID = @companyID                  
 LEFT OUTER JOIN T_CompanyThirdPartyCVIdentifier ON T_CompanyCounterPartyVenues.CompanyCounterPartyCVID = T_CompanyThirdPartyCVIdentifier.CompanyCounterPartyVenueID_FK                  
  AND T_CompanyThirdPartyCVIdentifier.CompanyThirdPartyID_FK = @thirdPartyID                  
 LEFT OUTER JOIN #SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol                  
                  
 WHERE datediff(d, (                  
    CASE                   
     WHEN @dateType = 1                  
      THEN VT.AUECLocalDate                  
     ELSE VT.ProcessDate                  
     END                  
    ), @inputdate) >= 0                  
  AND VT.FundID IN (                  
   SELECT FundID                  
   FROM @Fund                  
   )                  
  AND (                  
   (VT.TaxLotFIXAckState <> 0)                  
   and (                  
    (VT.TaxLotState <> 1)                  
    OR (                  
     VT.TaxLotState = 1                  
     AND datediff(d, (                  
       CASE                   
        WHEN @dateType = 1                  
         THEN VT.AUECLocalDate                  
        ELSE VT.ProcessDate                  
        END                  
       ), @inputdate) = 0                  
     )                  
    )                  
   AND (                  
    VT.TaxLotState <> 4                  
    OR (                  
     VT.TaxLotState = 4                  
     AND datediff(d, (                  
       CASE                   
        WHEN @dateType = 1                  
         THEN VT.AUECLocalDate                  
        ELSE VT.ProcessDate                  
        END                  
       ), @inputdate) = 0                  
     )                  
    )                  
   )                  
  AND VT.AUECID IN (            
   SELECT AUECID                  
   FROM @AUECID                  
   )                  
  AND VT.PBID = @thirdPartyID                  
GROUP BY VT.TaxlotID                  
  ,VT.Level1AllocationID                  
  ,VT.FundID                  
,F.FundName                  
  ,T_OrderType.OrderTypesID                  
  ,T_OrderType.OrderTypes                  
  ,VT.SideID                  
  ,T_Side.Side                  
  ,VT.Symbol                  
  ,VT.CounterPartyID                  
,C.ShortName                  
  ,VT.VenueID                  
  ,VT.AvgPrice                  
  ,VT.CumQty                  
  ,VT.Quantity                  
  ,VT.AUECID                  
  ,T_Asset.AssetName                  
  ,VT.AssetID                  
  ,VT.UnderlyingID                  
  ,VT.ExchangeID                
,E.DisplayName                  
  ,Currency.CurrencyID                  
  ,Currency.CurrencyName                  
  ,Currency.CurrencySymbol                  
  ,CTPM.MappedName                  
  ,CTPM.FundAccntNo                  
  ,CTPM.FundTypeID_FK                  
  ,FT.FundTypeName                  
  ,SM.PutOrCall                  
  ,SM.StrikePrice                  
  ,SM.ExpirationDate                  
  ,VT.SettlementDate                  
  ,T_ThirdPartyType.ThirdPartyTypeID                  
  ,T_ThirdPartyType.ThirdPartyTypeName                  
  ,CTPFD.CompanyIdentifier                  
  ,T_CounterPartyVenue.DisplayName                  
  ,T_CompanyThirdPartyCVIdentifier.CVIdentifier                  
  ,T_CompanyCounterPartyVenues.CompanyCounterPartyCVID             
  ,VT.GroupRefID                  
  ,VT.TaxLotState                  
  ,VT.TaxLotFIXState                  
  ,VT.TaxLotFIXAckState                 
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
  ,VT.FromDeleted                  
  ,VT.ProcessDate                  
  ,VT.OriginalPurchaseDate                  
  ,VT.AccruedInterest                  
  ,SM.Coupon                  
  ,SM.IssueDate                  
  ,SM.FirstCouponDate                  
  ,SM.CouponFrequencyID                  
  ,Sm.AccrualBasisID                  
  ,SM.BondTypeID                  
  ,                  
  --Swap Parameters                                                        
  VT.BenchMarkRate                  
  ,VT.Differential                  
  ,VT.SwapDescription                  
  ,VT.DayCount                  
  ,VT.FirstResetDate                  
  ,VT.IsSwapped                  
  ,T_Country.CountryName                  
  ,VT.FXRate_Taxlot                  
  ,VT.FXConversionMethodOperator_Taxlot                  
  ,VT.LotID                  
  ,VT.ExternalTransID                  
  ,VT.TradeAttribute1                  
  ,VT.TradeAttribute2                  
  ,VT.TradeAttribute3                  
  ,VT.TradeAttribute4                  
  ,VT.TradeAttribute5                  
  ,VT.TradeAttribute6                  
  ,SM.AssetName                  
  ,SM.SecurityTypeName                  
  ,SM.SectorName                  
  ,SM.SubSectorName                  
  ,SM.CountryName                  
  ,VT.Description                  
  ,VT.TransactionType                  
  ,VT.parentSiblingInfo                  
  ,VT.SettlCurrency                  
       
  ,TC.CurrencySymbol                  
,VT.ChangeType                  
--------------Dynamic UDA-----------------                  
,Analyst                  
,CountryOfRisk                  
,CustomUDA1                  
,CustomUDA2                  
,CustomUDA3                  
,CustomUDA4                  
,CustomUDA5                  
,CustomUDA6                  
,CustomUDA7                  
,Issuer                  
,LiquidTag                  
,MarketCap                  
,Region                  
,RiskCurrency                  
,UCITSEligibleTag                  
--------------Dynamic UDA-----------------                  
,TGD.TradeDate,                  
S.Side,                
TGD.CounterpartyID,                  
OldC.ShortName,                
TGD.ExecutedQuantity,                  
TGD.AvgPrice,                  
TGD.SettlmentDate,                  
TGD.FXRate,                  
TGD.Commission,                  
TGD.OtherBrokerFees,                  
TGD.StampDuty,                  
TGD.TransactionLevy,                  
TGD.ClearingFee,                  
TGD.MiscFees,                  
TGD.VenueID,                  
TGD.FXConversionMethodOperator,                  
TGD.ProcessDate,                  
TGD.OriginalPurchaseDate,                  
TGD.Description,                  
TGD.AccruedInterest,                  
TGD.UnderlyingDelta,                  
TGD.LotID,                  
TGD.CommissionAmount,                  
TGD.CommissionRate,                  
TGD.SecFee,                  
TGD.OccFee,                  
TGD.OrfFee,                  
TGD.ClearingBrokerFee,                  
TGD.SoftCommission,                  
TGD.SoftCommissionAmount,                  
TGD.TransactionType,                  
OTC.CurrencySymbol,                  
                 
TGD.GenerationDate,                
TGD.AmendTaxLotId1,                
TGD.AmendTaxLotId2 ,        
TGD.TaxOnCommissions ,
VT.TaxlotStateID              
 ORDER BY GroupRefID                  
                  
drop TABLE #VT,                  
#SecMasterData,                  
#TaxlotsDates,                  
#T_TradeAuditAction