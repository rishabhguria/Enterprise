CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_FIX] (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                            
	@dateType INT -- 0 for Process Date and 1 for Trade Date.                                                                                                                                                            
	,@fileFormatID INT
	,@includeSent INT = 0
	)
AS
 
--declare	@thirdPartyID INT
--declare	@companyFundIDs VARCHAR(max)
--declare @inputDate DATETIME
--declare	@companyID INT
--declare	@auecIDs VARCHAR(max)
--declare	@TypeID INT                         
--declare	@dateType INT                                                                                                                                                           
--declare	@fileFormatID INT
--declare @includeSent INT  
 
--SET @thirdPartyID=41
--SET @companyFundIDs=N'1213,1214,1238,1239,1249,1250,1251,1254,1255,1256,1257,1258,1259,1260,1264,1265,1266,1267,1268,1269,1270,'
--SET @inputDate='2024-04-03'
--SET @companyID=7
--SET @auecIDs=N'72,152,20,164,69,65,71,67,64,76,63,102,29,55,53,47,49,44,34,43,56,59,31,54,45,108,21,60,18,61,74,1,15,11,62,73,105,12,80,90,16,100,19,32,33,81,'
--SET @TypeID=1
--SET @dateType=1
--SET @fileFormatID=129
--SET @includeSent = 0
  
Declare @CounterPartyId Int  
  
Set @CounterPartyId =   
(  
Select CounterPartyID From T_ThirdParty  
Where ThirdPartyTypeID = 3 And ThirdPartyID = @thirdPartyID   
And CounterPartyID Is Not Null and CounterPartyID <> -2147483648  
)  
  
--Select @CounterPartyId  
  
Create Table #Temp_CounterPartyID  
(  
CounterPartyID INT  
)  
  
If (@CounterPartyId Is Null Or @CounterPartyId = '')  
Begin  
 Insert InTo #Temp_CounterPartyID  
 Select CounterPartyID From T_CounterParty  
End  
Else  
 Begin  
  Insert InTo #Temp_CounterPartyID  
  Select @CounterPartyId  
 End  
  
  
  
DECLARE @Fund TABLE (FundID INT)  
DECLARE @AUECID TABLE (AUECID INT)  
DECLARE @IncludeExpiredSettledTransaction INT  
DECLARE @IncludeExpiredSettledUnderlyingTransaction INT  
DECLARE @IncludeCATransaction INT  
  
SELECT @IncludeExpiredSettledTransaction = IncludeExercisedAssignedTransaction  
 ,@IncludeExpiredSettledUnderlyingTransaction = IncludeExercisedAssignedUnderlyingTransaction  
 ,@IncludeCATransaction = IncludeCATransaction  
FROM T_ThirdPartyFileFormat  
WHERE FileFormatId = @fileFormatID  
  
INSERT INTO @Fund  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@companyFundIDs, ',')  
  
INSERT INTO @AUECID  
SELECT Cast(Items AS INT)  
FROM dbo.Split(@auecIDs, ',')  
  
CREATE TABLE #SecMasterData   
 (  
 TickerSymbol VARCHAR(200)  
 ,PutOrCall VARCHAR(10)  
 ,Multiplier FLOAT  
 ,ISINSymbol VARCHAR(20)  
 ,CUSIPSymbol VARCHAR(50)  
 ,SEDOLSymbol VARCHAR(50)  
 ,BloombergSymbol VARCHAR(200)  
 ,CompanyName VARCHAR(500)  
 ,UnderlyingSymbol VARCHAR(100)  
 ,OSISymbol VARCHAR(25)  
 )  
  
INSERT INTO #SecMasterData  
SELECT   
 TickerSymbol  
 ,PutOrCall  
 ,Multiplier  
 ,ISINSymbol  
 ,CUSIPSymbol  
 ,SEDOLSymbol  
 ,BloombergSymbol  
 ,CompanyName  
 ,UnderlyingSymbol  
 ,OSISymbol  
FROM V_SecMasterData SM  
  
CREATE TABLE #VT   
 (  
 FundID INT  
 ,SideID VARCHAR(3)  
 ,Symbol VARCHAR(100)  
 ,CounterPartyID INT  
 ,OrderQty FLOAT  
 ,AvgPrice FLOAT  
 ,CumQty FLOAT  
 ,AUECID INT  
 ,AssetID INT  
 ,CurrencyID INT  
 ,Level1AllocationID VARCHAR(50)  
 ,TaxLotQty FLOAT  
 ,SettlementDate DATETIME  
 ,Commission FLOAT  
 ,OtherBrokerFees FLOAT  
 ,GroupRefID INT  
 ,TaxlotState VARCHAR(20)  
 ,TaxlotStateID VARCHAR(10)  
 ,StampDuty FLOAT  
 ,TransactionLevy FLOAT  
 ,ClearingFee FLOAT  
 ,TaxOnCommissions FLOAT  
 ,MiscFees FLOAT  
 ,AUECLocalDate DATETIME  
 ,ProcessDate DATETIME  
 ,PBID INT  
 ,SecFee FLOAT  
 ,OccFee FLOAT  
 ,OrfFee FLOAT  
 ,ClearingBrokerFee FLOAT  
 ,SoftCommission FLOAT  
 ,TaxLotID_1 VARCHAR(50)  
 ,GroupID Varchar(50)  
 )  
  
INSERT INTO #VT  
SELECT   
 VT.FundID AS FundID  
 ,VT.OrderSideTagValue AS SideID  
 ,VT.Symbol  
 ,VT.CounterPartyID  
 ,(VT.TaxLotQty) AS OrderQty  
 ,VT.AvgPrice  
 ,VT.CumQty   
 ,VT.AUECID  
 ,VT.AssetID  
 ,VT.CurrencyID  
 ,VT.Level1AllocationID AS Level1AllocationID  
 ,VT.TaxLotQty  
 ,VT.SettlementDate  
 ,VT.Commission  
 ,VT.OtherBrokerFees  
 ,VT.GroupRefID  
 ,'' AS TaxlotState  
 ,0 AS TaxlotStateID  
 ,ISNULL(VT.StampDuty, 0) AS StampDuty  
 ,ISNULL(VT.TransactionLevy, 0) AS TransactionLevy  
 ,ISNULL(VT.ClearingFee, 0) AS ClearingFee  
 ,ISNULL(VT.TaxOnCommissions, 0) AS TaxOnCommissions  
 ,ISNULL(VT.MiscFees, 0) AS MiscFees  
 ,VT.AUECLocalDate  
 ,VT.ProcessDate  
 ,@thirdPartyID AS PBID  
 ,ISNULL(VT.SecFee, 0) AS SecFee  
 ,ISNULL(VT.OccFee, 0) AS OccFee  
 ,ISNULL(VT.OrfFee, 0) AS OrfFee  
 ,VT.ClearingBrokerFee  
 ,VT.SoftCommission  
 ,VT.TaxLotID As TaxLotID_1  
 ,VT.GroupID  
FROM V_TaxLots VT  
INNER JOIN @Fund Fund ON Fund.FundID = VT.FundID  
Inner Join #Temp_CounterPartyID CP On CP.CounterPartyID = VT.CounterPartyID  
INNER JOIN @AUECID auec ON auec.AUECID = VT.AUECID  
WHERE datediff(d, (
   CASE   
    WHEN @dateType = 1  
     THEN VT.AUECLocalDate  
    ELSE VT.ProcessDate  
    END  
   ), @inputdate) = 0  
 AND (  
  (  
   VT.TransactionType IN ('Buy','BuytoClose','BuytoOpen','Sell','Sellshort','SelltoClose','SelltoOpen','LongAddition','LongWithdrawal','ShortAddition','ShortWithdrawal','')  
   AND (VT.TransactionSource IN (0,1,2,3,4,14))  
   )  
  OR (  
   @IncludeExpiredSettledTransaction = 1  
   AND VT.TransactionType IN ('Exercise','Expire','Assignment')  
   AND VT.AssetID IN (2,4)  
   )  
  OR (  
   @IncludeExpiredSettledTransaction = 1  
   AND VT.TransactionType IN ('CSCost','CSZero','DLCost','CSClosingPx','Expire','DLCostAndPNL')  
   AND VT.AssetID IN (3)  
   )  
  OR (  
   @IncludeExpiredSettledUnderlyingTransaction = 1  
   AND VT.TransactionType IN ('Exercise','Expire','Assignment')  
   AND VT.TaxlotClosingID_FK IS NOT NULL  
   AND VT.AssetID IN (1,3)  
   )  
  OR (  
   @IncludeCATransaction = 1  
   AND (VT.TransactionSource IN ( 6,7,8,9,10,11))  
   )  
  OR VT.TransactionSource = 13  
  )  
  
  
UPDATE #VT  
SET #VT.TaxlotStateID = PB.TaxlotState  
FROM #VT  
INNER JOIN T_PBWiseTaxlotState PB with (nolock) ON (PB.TaxlotID = #VT.TaxLotID_1)  
WHERE PB.TaxlotState <> 0 AND PB.FileFormatID = @fileFormatID  
  
SELECT   
 VT.GroupID  
 ,VT.Symbol  
 ,CF.FundName As AccountName  
 ,CASE   
  WHEN VT.TaxlotStateID = '0'  
   THEN 'Allocated'  
  WHEN VT.TaxlotStateID = '1'  
   THEN 'Sent'  
  WHEN VT.TaxlotStateID = '2'  
   THEN 'Amended'  
  WHEN VT.TaxlotStateID = '3'  
   THEN 'Deleted'  
  WHEN VT.TaxlotStateID = '4'  
   THEN 'Ignored'  
 END AS TaxLotState  
 ,VT.TaxlotStateID  
 ,CAST('' AS VARCHAR(500)) AS ClOrderID  
  ,CAST('' AS VARCHAR(500)) AS ParentClOrderID  
 ,CAST('' AS VARCHAR(500)) AS OrderID  
 ,S.Side AS Side  
 ,VT.AvgPrice  
 ,VT.CumQty  
 ,Currency.CurrencySymbol
 ,VT.Level1AllocationID AS Level1AllocationID  
 ,Sum(VT.TaxLotQty) As TaxLotQty  
 ,SM.PutOrCall  
 ,VT.SettlementDate  
 ,VT.GroupRefID  
 ,Sum(VT.Commission) As CommissionCharged  
 ,Sum(VT.OtherBrokerFees) As OtherBrokerFees  
 ,Sum(ISNULL(VT.StampDuty, 0)) AS StampDuty  
 ,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy  
 ,Sum(ISNULL(ClearingFee, 0)) AS ClearingFee  
 ,Sum(ISNULL(TaxOnCommissions, 0)) AS TaxOnCommissions  
 ,Sum(ISNULL(MiscFees, 0)) AS MiscFees  
 ,VT.AUECLocalDate As TradeDate  
 ,SM.Multiplier As AssetMultiplier  
 ,SM.ISINSymbol As ISIN  
 ,SM.CUSIPSymbol As CUSIP  
 ,SM.SEDOLSymbol As SEDOL  
 ,SM.BloombergSymbol  
 ,SM.CompanyName  
 ,SM.UnderlyingSymbol  
 ,SM.OSISymbol  
 ,VT.Level1AllocationID As EntityID  
 ,Sum(ISNULL(VT.SecFee, 0)) AS SecFee  
 ,Sum(ISNULL(VT.OccFee, 0)) AS OccFee  
 ,Sum(ISNULL(VT.OrfFee, 0)) AS OrfFee  
 ,Sum(VT.ClearingBrokerFee) As ClearingBrokerFee  
 ,Sum(VT.SoftCommission) As SoftCommissionCharged  
 ,VT.AUECLocalDate As TradeDateTime  
 ,A.AssetName  As Asset  
 ,@fileFormatID As FileFormatID  
 ,'Taxlot' As GroupOrTaxlotType  
 ,2 As CustomizedGrouping 
 , Counterparty.fullname as Counterparty
InTo #FinalTaxlots        
FROM #VT VT  
Inner Join T_CompanyFunds CF On CF.CompanyFundID = VT.FundID  
Inner JOIN #SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol  
INNER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID  
INNER JOIN T_Side S ON S.SideTagValue = VT.SideID  
INNER JOIN T_Asset A ON VT.AssetID = A.AssetID 
INNER JOIN T_CounterParty as counterparty on counterparty.CounterPartyID = VT.CounterPartyID  
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID  
LEFT OUTER JOIN T_CompanyThirdPartyFlatFileSaveDetails CTPFD ON CTPFD.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK 

  
WHERE (  
  (  
   VT.TaxlotStateID <> 1  
   OR (  
    VT.TaxlotStateID = 1  
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
   VT.TaxlotStateID <> 4  
   OR (  
    VT.TaxlotStateID = 4  
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
   
GROUP BY  
 VT.Symbol  
 ,CF.FundName   
 ,VT.Level1AllocationID  
 ,S.Side   
 ,VT.AvgPrice  
 ,VT.CumQty  
 ,Currency.CurrencySymbol   
 ,SM.PutOrCall  
 ,VT.SettlementDate  
 ,VT.GroupRefID  
 ,VT.TaxLotState  
 ,VT.TaxlotStateID  
 ,VT.AUECLocalDate  
 ,SM.Multiplier  
 ,SM.ISINSymbol  
 ,SM.CUSIPSymbol  
 ,SM.SEDOLSymbol  
 ,SM.BloombergSymbol  
 ,SM.CompanyName  
 ,SM.UnderlyingSymbol  
 ,SM.OSISymbol  
 ,VT.GroupID   
 ,A.AssetName   
 ,Counterparty.FullName  

  
---- Get Amanded taxlots with GroupId to update the Group level data (if one or more Taxlots are Amended)  
Select Distinct GroupID, TaxlotState, TaxlotStateID   
InTo #Temp_AmendedGroupIDWithTaxlotState  
From #FinalTaxlots
Where TaxlotState = 'Amended'  
And GroupOrTaxlotType = 'Taxlot'  
  
-- Update axlotState  
Update TT  
Set TT.TaxLotState = T.TaxLotState,  
TT.TaxlotStateID = T.TaxlotStateID  
From #FinalTaxlots TT
Inner Join #Temp_AmendedGroupIDWithTaxlotState T On T.GroupID = TT.GroupID  
Where TT.TaxlotState <> 'Amended'  
  
Select Distinct GroupID, TaxlotState, TaxlotStateID   
InTo #Temp_UniqueGroupIDWithTaxlotState  
From #FinalTaxlots
  
SELECT   
 G.GroupID  
 ,G.Symbol  
 ,'' As AccountName  
 ,UG.TaxLotState As TaxLotState  
 ,UG.TaxlotStateID 
	,sub.ClOrderID
	,sub.ParentClOrderID
	,ISNULL(Fills.OrderID,TTO.OrderID) as OrderID
 ,S.Side AS Side  
 ,G.AvgPrice  
 ,G.CumQty  
 ,Currency.CurrencySymbol  
 ,'' AS Level1AllocationID  
 ,0 As TaxLotQty  
 ,SM.PutOrCall  
 ,G.SettlementDate  
 ,G.GroupRefID  
 ,G.Commission As Commission  
 ,G.OtherBrokerFees As OtherBrokerFees  
 ,G.StampDuty AS StampDuty  
 ,G.TransactionLevy AS TransactionLevy  
 ,G.ClearingFee AS ClearingFee  
 ,G.TaxOnCommissions AS TaxOnCommissions  
 ,G.MiscFees AS MiscFees  
 ,G.AUECLocalDate As TradeDate  
 ,SM.Multiplier  
 ,SM.ISINSymbol As ISIN  
 ,SM.CUSIPSymbol As CUSIP  
 ,SM.SEDOLSymbol As SEDOL  
 ,SM.BloombergSymbol  
 ,SM.CompanyName  
 ,SM.UnderlyingSymbol  
 ,SM.OSISymbol  
 ,'' As EntityID  
 ,G.SecFee AS SecFee  
 ,G.OccFee AS OccFee  
 ,G.OrfFee AS OrfFee  
 ,G.ClearingBrokerFee As ClearingBrokerFee  
 ,G.SoftCommission As SoftCommission  
 ,G.AUECLocalDate As TradeDateTime  
 ,A.AssetName  As Asset  
 ,@fileFormatID As FileFormatID  
 ,'Group' As GroupOrTaxlotType  
 ,1 As CustomizedGrouping  
, counterparty.fullname as Counterparty
	,sub.StagedOrderID
	,TTO.TimeInForce
	into #TempTaxlots
 --Into #Group_Data  
 From T_Group G  
	Inner Join #Temp_UniqueGroupIDWithTaxlotState UG On UG.GroupID = G.GroupID  
	INNER JOIN #SecMasterData AS SM ON SM.TickerSymbol = G.Symbol  
	INNER JOIN T_Asset A ON G.AssetID = A.AssetID  
	INNER JOIN T_Currency AS Currency ON Currency.CurrencyID = G.CurrencyID  
	INNER JOIN T_Side S On S.SideTagValue = G.OrderSideTagValue 
	Inner JOIN T_CounterParty as counterparty on counterparty.CounterPartyID = G.CounterPartyID 
	LEFT JOIN T_TradedOrders TTO ON TTO.GroupID = G.GroupID  
	LEFT JOIN T_Sub sub ON sub.ClOrderID = TTO.CLOrderID
	LEFT JOIN T_Fills Fills ON (TTO.NirvanaSeqNumber = fills.NirvanaSeqNumber AND TTO.CLOrderID = Fills.ClOrderID)


update T  
set OrderID = ISNULL(Fills.OrderID,TTO.OrderID),
    ClOrderID = TTO.CLOrderID, 
	ParentClOrderID = TTO.ParentClOrderID
From #TempTaxlots T
LEFT JOIN T_TradedOrders TTO ON T.StagedOrderID = TTO.ParentClOrderID
LEFT JOIN T_Fills Fills ON (TTO.NirvanaSeqNumber = fills.NirvanaSeqNumber AND TTO.CLOrderID = Fills.ClOrderID)
WHERE (TTO.TimeInForce = 1 or TTO.TimeInForce = 6) AND (T.TimeInForce = 1 or T.TimeInForce = 6)

Insert InTo #FinalTaxlots
SELECT 
	Temp.GroupID
	,Temp.Symbol
	,'' As AccountName
	,Temp.TaxLotState As TaxLotState
	,Temp.TaxlotStateID
	,Temp.ClOrderID
	,Temp.ParentClOrderID
	,Temp.OrderID
	,Temp.Side AS Side
	,Temp.AvgPrice
	,Temp.CumQty
	,Temp.CurrencySymbol
	,'' AS Level1AllocationID
	,0 As TaxLotQty
	,Temp.PutOrCall
	,Temp.SettlementDate
	,Temp.GroupRefID
	,Temp.Commission As Commission
	,Temp.OtherBrokerFees As OtherBrokerFees
	,Temp.StampDuty AS StampDuty
	,Temp.TransactionLevy AS TransactionLevy
	,Temp.ClearingFee AS ClearingFee
	,Temp.TaxOnCommissions AS TaxOnCommissions
	,Temp.MiscFees AS MiscFees
	,Temp.TradeDate
	,Temp.Multiplier
	,Temp.ISIN
	,Temp.CUSIP
	,Temp.SEDOL
	,Temp.BloombergSymbol
	,Temp.CompanyName
	,Temp.UnderlyingSymbol
	,Temp.OSISymbol
	,'' As EntityID
	,Temp.SecFee
	,Temp.OccFee
	,Temp.OrfFee
	,Temp.ClearingBrokerFee
	,Temp.SoftCommission
	,Temp.TradeDateTime
	,Temp.Asset
	,@fileFormatID As FileFormatID
	,'Group' As GroupOrTaxlotType
	,1 As CustomizedGrouping 
	,Temp.Counterparty
	From #TempTaxlots temp
	



Select Distinct   
TDT.GroupID,  
TDT.FileFormatID  
Into #Temp_DeletedGroupId  
FROM T_DeletedTaxLots TDT  
INNER JOIN @Fund Fund ON Fund.FundID = TDT.FundID  
INNER JOIN @AUECID auec ON auec.AUECID = TDT.AUECID  
Inner Join #Temp_CounterPartyID CP On CP.CounterPartyID = TDT.CounterPartyID  
WHERE DateDiff(DAY, (  
   CASE   
    WHEN @DateType = 1  
    THEN TDT.AUECLocalDate  
    ELSE TDT.ProcessDate  
    END  
   ), @Inputdate) = 0  
AND (FileFormatID = @FileFormatID)  
 AND TDT.TaxLotState = 3  
  
Update Taxlots  
Set Taxlots.TaxLotState = 'Amended', Taxlots.TaxlotStateID = 2  
From #FinalTaxlots Taxlots
Inner Join #Temp_DeletedGroupId D On D.GroupID = Taxlots.GroupID And D.FileFormatID = Taxlots.FileFormatID  
Where Taxlots.TaxLotState = 'Allocated'  
  
  
Select *  
From #FinalTaxlots
Where TaxlotStateID NOT IN (  
  CASE @includeSent  
   WHEN 0  
    THEN 1  
   ELSE - 1  
   END  
  )  
ORDER BY GroupID, CustomizedGrouping   
  
DROP TABLE #VT,#SecMasterData,#FinalTaxlots,#Temp_AmendedGroupIDWithTaxlotState,#Temp_UniqueGroupIDWithTaxlotState,#Temp_CounterPartyID,#TempTaxlots,#Temp_DeletedGroupId