CREATE PROCEDURE [dbo].[P_FFGetThirdPartyFundsDetails_FIX_CancelNew] 
 (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                          
	,@fileFormatID INT
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

--SET @thirdPartyID=41
--SET @companyFundIDs=N'1,2,3,4,5,6,7'
--SET @inputDate='2024-04-03'
--SET @companyID=7
--SET @auecIDs=N'72,152,20,164,69,65,71,67,64,76,63,102,29,55,53,47,49,44,34,43,56,59,31,54,45,108,21,60,18,61,74,1,15,11,62,73,105,12,80,90,16,100,19,32,33,81,'
--SET @TypeID=1
--SET @dateType=1
--SET @fileFormatID=176
----SET @includeSent = 0

Declare @CounterPartyId Int

Set @CounterPartyId = 
(
Select CounterPartyID From T_ThirdParty
Where @TypeID <> 0 And  ThirdPartyTypeID = 3 And ThirdPartyID = @thirdPartyID 
And CounterPartyID Is Not Null and CounterPartyID <> -2147483648
)

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

CREATE TABLE #SecMasterData 
	(
	TickerSymbol VARCHAR(200)
	,PutOrCall VARCHAR(10)
	,Multiplier Float
	,ISINSymbol VARCHAR(50)
	,CUSIPSymbol VARCHAR(50)
	,SEDOLSymbol VARCHAR(50)
	,BloombergSymbol VARCHAR(200)
	,CompanyName VARCHAR(500)
	,UnderlyingSymbol VARCHAR(100)
	,OSISymbol VARCHAR(30)
	,AssetName VARCHAR(200)
	,SecurityTypeName VARCHAR(200)
	,SectorName VARCHAR(200)
	,SubSectorName VARCHAR(200)
	,CountryName VARCHAR(100)
	)



CREATE TABLE #VT 
(
	GroupID VARCHAR(50)
	,TaxLotID VARCHAR(50)
	,Level2AllocationID VARCHAR(50)
	,FundID INT
	,OrderTypeTagValue VARCHAR(3)
	,SideID VARCHAR(5)
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
	,IsBasketGroup VARCHAR(20)
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
	,PBID INT
	,FromDeleted VARCHAR(5)
	,ProcessDate DATETIME
	,LotID VARCHAR(200)
	,ExternalTransID VARCHAR(200)
	,SecFee FLOAT
	,OccFee FLOAT
	,OrfFee FLOAT
	,ClearingBrokerFee FLOAT
	,SoftCommission FLOAT
	,ChangeType INT
	,TaxlotStateID INT
	,TaxLotID_1 VARCHAR(50)
	,Commission_BlockLevel FLOAT
	,OtherBrokerFees_BlockLevel FLOAT
	,StampDuty_BlockLevel FLOAT
	,TransactionLevy_BlockLevel FLOAT
	,ClearingFee_BlockLevel FLOAT
	,TaxOnCommissions_BlockLevel FLOAT
	,MiscFees_BlockLevel FLOAT
	,SecFee_BlockLevel FLOAT
	,OccFee_BlockLevel FLOAT
	,OrfFee_BlockLevel FLOAT
	,ClearingBrokerFee_BlockLevel FLOAT
	,SoftCommission_BlockLevel FLOAT
	,TradeDateTime DATETIME
	)

INSERT INTO #VT
SELECT 
	G.GroupID
	,L2.Level1AllocationID AS TaxlotID
	,L2.TaxLotID AS Level2AllocationID
	,L1.FundID AS FundID
	,G.OrderTypeTagValue
	,G.OrderSideTagValue AS SideID
	,G.Symbol
	,G.CounterPartyID
	,(L2.TaxLotQty) AS OrderQty
	,G.AvgPrice
	,G.CumQty
	,G.AUECID
	,G.AssetID
	,G.CurrencyID
	,L2.Level1AllocationID AS Level1AllocationID
	,(L2.TaxLotQty) As TaxLotQty
	,'' AS IsBasketGroup
	,G.SettlementDate
	,L2.Commission
	,L2.OtherBrokerFees
	,G.GroupRefID
	,0 AS TaxlotState
	,L2.StampDuty AS StampDuty
	,L2.TransactionLevy AS TransactionLevy
	,L2.ClearingFee AS ClearingFee
	,L2.TaxOnCommissions AS TaxOnCommissions
	,L2.MiscFees AS MiscFees
	,G.AUECLocalDate
	,0 AS Level2ID
	,@thirdPartyID AS PBID
	,'No' AS FromDeleted
	,G.ProcessDate
	,L2.LotID
	,L2.ExternalTransID
	,L2.SecFee AS SecFee
	,L2.OccFee AS OccFee
	,L2.OrfFee AS OrfFee
	,L2.ClearingBrokerFee
	,L2.SoftCommission
	,G.ChangeType AS ChangeType
	,0 AS TaxlotStateID
	,L2.TaxLotID As TaxLotID_1
	,G.Commission As Commission_BlockLevel
	,G.OtherBrokerFees As OtherBrokerFees_BlockLevel
	,G.StampDuty As StampDuty_BlockLevel
	,G.TransactionLevy As TransactionLevy_BlockLevel
	,G.ClearingFee As ClearingFee_BlockLevel
	,G.TaxOnCommissions As TaxOnCommissions_BlockLevel
	,G.MiscFees As MiscFees_BlockLevel
	,G.SecFee As SecFee_BlockLevel
	,G.OccFee As OccFee_BlockLevel
	,G.OrfFee As OrfFee_BlockLevel
	,G.ClearingBrokerFee As ClearingBrokerFee_BlockLevel
	,G.SoftCommission As SoftCommission_BlockLevel
	,G.AUECLocalDate As TradeDateTime
FROM T_Group G
Inner Join T_FundAllocation L1 On L1.GroupID = G.GroupID
Inner Join T_Level2Allocation L2 On L2.Level1AllocationID = L1.AllocationId
INNER JOIN @Fund Fund ON Fund.FundID = L1.FundID
INNER JOIN @AUECID auec ON auec.AUECID = G.AUECID
Inner Join #Temp_CounterPartyID CP On CP.CounterPartyID = G.CounterPartyID
WHERE L2.TaxlotQty <> 0 And
DateDiff(Day, (
			CASE 
				WHEN @dateType = 1
				THEN G.AUECLocalDate
				ELSE G.ProcessDate
				END
			), @inputdate) = 0
	AND (
		(
			G.TransactionType IN (
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
				G.TransactionSource IN (
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
			AND G.TransactionType IN (
				'Exercise'
				,'Expire'
				,'Assignment'
				)
			AND G.AssetID IN (
				2
				,4
				)
			)
		OR (
			@IncludeExpiredSettledTransaction = 1
			AND G.TransactionType IN (
				'CSCost'
				,'CSZero'
				,'DLCost'
				,'CSClosingPx'
				,'Expire'
				,'DLCostAndPNL'
				)
			AND G.AssetID IN (3)
			)
		OR (
			@IncludeExpiredSettledUnderlyingTransaction = 1
			AND G.TransactionType IN (
				'Exercise'
				,'Expire'
				,'Assignment'
				)
			AND G.TaxlotClosingID_FK IS NOT NULL
			AND G.AssetID IN (
				1
				,3
				)
			)
		OR (
			@IncludeCATransaction = 1
			AND (
				G.TransactionSource IN (
					6
					,7
					,8
					,9
					,10
					,11
					)
				)
			)
		OR G.TransactionSource = 13
		)


INSERT INTO #VT
SELECT 
	TDT.GroupID
	,TDT.Level1AllocationID AS TaxlotID
	,TDT.TaxLotID AS Level2AllocationID
	,ISNULL(TDT.FundID, 0) AS FundID
	,TDT.OrderTypeTagValue
	,TDT.OrderSideTagValue AS SideID
	,TDT.Symbol
	,TDT.CounterPartyID
	,TDT.TaxLotQty AS OrderQty
	,TDT.AvgPrice
	,TDT.CumQty
	,TDT.AUECID
	,TDT.AssetID
	,TDT.CurrencyID
	,TDT.Level1AllocationID AS Level1AllocationID
	,TDT.TaxLotQty As TaxLotQty
	,' ' AS IsBasketGroup
	,TDT.SettlementDate
	,TDT.Commission
	,(TDT.OtherBrokerFees)
	,TDT.GroupRefID
	,TDT.TaxLotState
	,(ISNULL(TDT.StampDuty, 0)) AS StampDuty
	,(ISNULL(TDT.TransactionLevy, 0)) AS TransactionLevy
	,(ISNULL(TDT.ClearingFee, 0)) AS ClearingFee
	,(ISNULL(TDT.TaxOnCommissions, 0)) AS TaxOnCommissions
	,(ISNULL(TDT.MiscFees, 0)) AS MiscFees
	,TDT.AUECLocalDate
	,0 AS Level2ID
	,TDT.PBID
	,'Yes' AS FromDeleted
	,TDT.ProcessDate
	,TDT.LotID
	,TDT.ExternalTransID
	,(ISNULL(TDT.SecFee, 0)) AS SecFee
	,(ISNULL(TDT.OccFee, 0)) AS OccFee
	,(ISNULL(TDT.OrfFee, 0)) AS OrfFee
	,(TDT.ClearingBrokerFee)
	,(TDT.SoftCommission)
	,'3' AS ChangeType
	,TDT.TaxlotState AS TaxlotStateID
	,TDT.TaxLotID As TaxLotID_1
	,Cast(0 as Float) As Commission_BlockLevel
	,Cast(0 as Float) As OtherBrokerFees_BlockLevel
	,Cast(0 as Float) As StampDuty_BlockLevel
	,Cast(0 as Float) As TransactionLevy_BlockLevel
	,Cast(0 as Float) As ClearingFee_BlockLevel
	,Cast(0 as Float) As TaxOnCommissions_BlockLevel
	,Cast(0 as Float) As MiscFees_BlockLevel
	,Cast(0 as Float) As SecFee_BlockLevel
	,Cast(0 as Float) As OccFee_BlockLevel
	,Cast(0 as Float) As OrfFee_BlockLevel
	,Cast(0 as Float) As ClearingBrokerFee_BlockLevel
	,Cast(0 as Float) As SoftCommission_BlockLevel
	,TDT.AUECLocalDate As TradeDateTime
FROM T_DeletedTaxLots TDT
INNER JOIN @Fund Fund ON Fund.FundID = TDT.FundID
INNER JOIN @AUECID auec ON auec.AUECID = TDT.AUECID
Inner Join #Temp_CounterPartyID CP On CP.CounterPartyID = TDT.CounterPartyID
WHERE (FileFormatID = @fileFormatID)
	AND TDT.TaxlotState = 3
	AND datediff(d, (
			CASE 
				WHEN @dateType = 1
					THEN TDT.AUECLocalDate
				ELSE TDT.ProcessDate
				END
			), @inputdate) >= 0

Select 
GroupRefID,
Sum(Commission) As Commission,
Sum(OtherBrokerFees) As OtherBrokerFees,
Sum(StampDuty) As StampDuty,
Sum(TransactionLevy) As TransactionLevy,
Sum(ClearingFee) As ClearingFee,
Sum(TaxOnCommissions) As TaxOnCommissions,
Sum(MiscFees) As MiscFees,
Sum(SecFee) As SecFee,
Sum(OccFee) As OccFee,
Sum(OrfFee) As OrfFee,
Sum(ClearingBrokerFee) As ClearingBrokerFee,
Sum(SoftCommission) As SoftCommission
InTo #Temp_DeletedGroupedData
From #VT
Where FromDeleted = 'Yes'
Group By GroupRefID

Update VT
Set 
VT.Commission_BlockLevel = GDeleted.Commission,
VT.OtherBrokerFees_BlockLevel = GDeleted.OtherBrokerFees,
VT.StampDuty_BlockLevel = GDeleted.StampDuty,
VT.TransactionLevy_BlockLevel = GDeleted.TransactionLevy,
VT.ClearingFee_BlockLevel = GDeleted.ClearingFee,
VT.TaxOnCommissions_BlockLevel = GDeleted.TaxOnCommissions,
VT.MiscFees_BlockLevel = GDeleted.MiscFees,
VT.SecFee_BlockLevel = GDeleted.SecFee,
VT.OccFee_BlockLevel = GDeleted.OccFee,
VT.OrfFee_BlockLevel = GDeleted.OrfFee,
VT.ClearingBrokerFee_BlockLevel = GDeleted.ClearingBrokerFee,
VT.SoftCommission_BlockLevel = GDeleted.SoftCommission
From #Temp_DeletedGroupedData GDeleted
Inner Join #VT VT On VT.GroupRefID = GDeleted.GroupRefID
Where FromDeleted = 'Yes'


UPDATE #VT
SET #VT.TaxlotState = PB.TaxlotState
FROM #VT
INNER JOIN T_PBWiseTaxlotState PB With (NoLock) ON (PB.TaxlotID = #VT.TaxLotID_1)
WHERE PB.FileFormatID = @fileFormatID AND PB.TaxlotState <> 0

CREATE TABLE #TaxlotsDates (TaxlotId Varchar(50))

CREATE TABLE #T_TradeAuditAction 
	(
	ActionType INT PRIMARY KEY
	,ActionName NVARCHAR(50)
	)

INSERT INTO #T_TradeAuditAction (
	ActionType
	,ActionName
	)
VALUES (1,'REALLOCATE')
	,(2,'UNALLOCATE')
	,(3,'GROUP')
	,(4,'UNGROUP')
	,(5,'DELETE')
	,(6,'TradeDate_Changed')
	,(7,'OrderSide_Changed')
	,(8,'Counterparty_Changed')
	,(9,'ExecutedQuantity_Changed')
	,(10,'AvgPrice_Changed')
	,(11,'SettlementDate_Changed')
	,(13,'Commission_Changed')
	,(14,'OtherBrokerFees_Changed')
	,(15,'StampDuty_Changed')
	,(16,'TransactionLevy_Changed')
	,(17,'ClearingFee_Changed')
	,(18,'TaxOnCommission_Changed')
	,(19,'MiscFees_Changed')
	,(20,'Venue_Changed')
	,(26,'UnderlyingDelta_Changed')
	,(27,'LotId_Changed')
	,(28,'CommissionAmount_Changed')
	,(29,'CommissionRate_Changed')
	,(36,'ExternalTransId_Changed')
	,(37,'TradeEdited')
	,(38,'SecFee_Changed')
	,(39,'OccFee_Changed')
	,(40,'OrfFee_Changed')
	,(41,'ClearingBrokerFee_Changed')
	,(42,'SoftCommission_Changed')
	,(43,'SoftCommissionAmount_Changed')
	,(44,'SoftCommissionRate_Changed')
	,(46,'InternalComments_Changed')
	,(47,'SettlCurrency_Changed')
	,(48,'OptionPremiumAdjustment_Changed')

INSERT INTO #TaxlotsDates
SELECT DISTINCT TA.TaxlotId
FROM T_TradeAudit TA
INNER JOIN #T_TradeAuditAction TAA ON TA.Action = TAA.ActionType
inner join T_Level2Allocation TL2 ON TL2.TaxLotID = TA.TaxlotId
INNER JOIN T_ThirdPartyFFGenerationDate TDG ON TDG.TaxlotID = TL2.Level1AllocationID
WHERE DATEDIFF(DAY,TDG.GenerationDate, TA.ActionDate) >= 0 AND TAA.ActionName NOT IN ('REALLOCATE','UNALLOCATE')

Select *
InTo #VT_New FROM #VT VT
WHERE (
		(
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

INSERT INTO #SecMasterData
SELECT TickerSymbol
	,PutOrCall
	,Multiplier
	,ISINSymbol
	,CUSIPSymbol
	,SEDOLSymbol
	,BloombergSymbol
	,CompanyName
	,UnderlyingSymbol
	,OSISymbol
	,AssetName
	,SecurityTypeName
	,SectorName
	,SubSectorName
	,CountryName	
FROM V_SecMasterData SM
Where SM.TickerSymbol In
	(
		Select Distinct Symbol From #VT_New
	)

SELECT 
	VT.GroupID
	,VT.TaxlotID AS TaxlotID
	,VT.Level1AllocationID AS EntityID
	,F.FundName AS AccountName
	,T_Side.Side AS Side
	,VT.Symbol
	,C.Shortname AS CounterParty
	,Sum(VT.TaxLotQty) AS OrderQty
	,VT.AvgPrice
	,VT.CumQty
	,T_Asset.AssetName AS Asset
	,Currency.CurrencySymbol
	,VT.Level1AllocationID AS Level1AllocationID
	,Sum(VT.TaxLotQty) As TaxLotQty
	,ISNULL(SM.PutOrCall, 0) As PutOrCall
	,VT.SettlementDate
	,Sum(VT.Commission) AS CommissionCharged
	,Sum(VT.OtherBrokerFees) AS OtherBrokerFees
	,VT.GroupRefID
	,VT.GroupRefID AS PBUniqueID
	,CASE 
		WHEN VT.TaxLotState = '0'
			THEN 'Allocated'
		WHEN VT.TaxLotState = '1'
			THEN 'Sent'
		WHEN VT.TaxLotState = '2'
			THEN 'Amended'
		WHEN VT.TaxLotState = '3'
			THEN 'Deleted'
		WHEN VT.TaxLotState = '4'
			THEN 'Ignored'
		END AS TaxLotState
	,Sum(ISNULL(VT.StampDuty, 0)) AS StampDuty
	,Sum(ISNULL(VT.TransactionLevy, 0)) AS TransactionLevy
	,Sum(ISNULL(VT.ClearingFee, 0)) AS ClearingFee
	,Sum(ISNULL(VT.TaxOnCommissions, 0)) AS TaxOnCommissions
	,Sum(ISNULL(VT.MiscFees, 0)) AS MiscFees
	,VT.AUECLocalDate AS TradeDate
	,ISNULL(SM.Multiplier, 1) AS AssetMultiplier
	,ISNULL(SM.ISINSymbol, ' ') AS ISIN
	,ISNULL(SM.CUSIPSymbol, ' ') AS CUSIP
	,ISNULL(SM.SEDOLSymbol, ' ') AS SEDOL
	,SM.BloombergSymbol AS BBCode
	,SM.CompanyName
	,SM.UnderlyingSymbol
	,SM.OSISymbol AS OSIOptionSymbol
	,Sum(ISNULL(VT.SecFee, 0)) AS SecFee
	,Sum(ISNULL(VT.OccFee, 0)) AS OccFee
	,Sum(ISNULL(VT.OrfFee, 0)) AS OrfFee
	,Sum(VT.ClearingBrokerFee) AS ClearingBrokerFee
	,Sum(VT.SoftCommission) AS SoftCommissionCharged
	,isnull(S.Side, '') AS OldSide
	,isnull(TGD.ExecutedQuantity, 0) AS OldExecutedQuantity
	,ISNULL(TGD.AvgPrice, 0) AS OldAvgPrice
	,ISNULL(TGD.Commission, 0) AS OldCommission
	,ISNULL(TGD.OtherBrokerFees, 0) AS OldOtherBrokerFees
	,ISNULL(TGD.StampDuty, 0) AS OldStampDuty
	,ISNULL(TGD.TransactionLevy, 0) AS OldTransactionLevy
	,ISNULL(TGD.ClearingFee, 0) AS OldClearingFee
	,ISNULL(TGD.MiscFees, 0) AS OldMiscFees
	,ISNULL(TGD.SecFee, 0) AS OldSecFee
	,ISNULL(TGD.OccFee, 0) AS OldOccFee
	,ISNULL(TGD.OrfFee, 0) AS OldOrfFee
	,ISNULL(TGD.ClearingBrokerFee, 0) AS OldClearingBrokerFee
	,ISNULL(TGD.SoftCommission, 0) AS OldSoftCommission
	,isnull(TGD.TaxOnCommissions, 0) AS OldTaxOnCommissions
	,Sum(VT.Commission_BlockLevel) As Commission_BlockLevel
	,Sum(VT.OtherBrokerFees_BlockLevel) As OtherBrokerFees_BlockLevel
	,Sum(VT.StampDuty_BlockLevel) As StampDuty_BlockLevel
	,Sum(VT.TransactionLevy_BlockLevel) As TransactionLevy_BlockLevel
	,Sum(VT.ClearingFee_BlockLevel) As ClearingFee_BlockLevel
	,Sum(VT.TaxOnCommissions_BlockLevel) As TaxOnCommissions_BlockLevel
	,Sum(VT.MiscFees_BlockLevel) As MiscFees_BlockLevel
	,Sum(VT.SecFee_BlockLevel) As SecFee_BlockLevel
	,Sum(VT.OccFee_BlockLevel) As OccFee_BlockLevel
	,Sum(VT.OrfFee_BlockLevel) As OrfFee_BlockLevel
	,Sum(VT.ClearingBrokerFee_BlockLevel) As ClearingBrokerFee_BlockLevel
	,Sum(VT.SoftCommission_BlockLevel) As SoftCommission_BlockLevel
	,Cast(0 as Float) As OldCommission_BlockLevel
	,Cast(0 as Float) As OldOtherBrokerFees_BlockLevel
	,Cast(0 as Float) As OldStampDuty_BlockLevel
	,Cast(0 as Float) As OldTransactionLevy_BlockLevel
	,Cast(0 as Float) As OldClearingFee_BlockLevel
	,Cast(0 as Float) As OldTaxOnCommissions_BlockLevel
	,Cast(0 as Float) As OldMiscFees_BlockLevel
	,Cast(0 as Float) As OldSecFee_BlockLevel
	,Cast(0 as Float) As OldOccFee_BlockLevel
	,Cast(0 as Float) As OldOrfFee_BlockLevel
	,Cast(0 as Float) As OldClearingBrokerFee_BlockLevel
	,Cast(0 as Float) As OldSoftCommission_BlockLevel
	,Cast(0 as Float) As OldExecutedQuantity_BlockLevel 
	,VT.TradeDateTime
	,'Taxlot' As GroupOrTaxlotType
	,2 As CustomizedGrouping 

InTo #VT_FinalData

FROM #VT_New VT
INNER JOIN T_CompanyFunds F ON F.CompanyFundID = VT.FundID
INNER JOIN #SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
INNER JOIN T_Asset ON VT.AssetID = T_Asset.AssetID
INNER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
INNER JOIN T_CounterParty C ON C.CounterPartyID = VT.CounterPartyID
INNER JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
INNER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
INNER JOIN dbo.T_CompanyThirdParty ON T_CompanyThirdParty.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK
LEFT JOIN #TaxlotsDates TD ON TD.TaxlotId = VT.Level2AllocationID
LEFT JOIN T_ThirdPartyFFGenerationDate TGD ON TGD.TaxLotID = VT.Level1AllocationID AND TGD.fileFormatID = @fileFormatID AND TGD.thirdPartyID = @thirdPartyID
LEFT JOIN T_CounterParty OldC ON OldC.CounterPartyID = TGD.CounterPartyID
LEFT JOIN T_Side S ON S.SideTagValue = TGD.SideID
LEFT JOIN dbo.T_OrderType ON VT.OrderTypeTagValue = dbo.T_OrderType.OrderTypeTagValue

GROUP BY 
	VT.GroupID
	,VT.TaxlotID
	,VT.Level1AllocationID
	,VT.FundID
	,F.FundName
	,T_Side.Side
	,VT.Symbol
	,C.ShortName
	,VT.AvgPrice
	,VT.CumQty
	,T_Asset.AssetName
	,VT.AssetID
	,Currency.CurrencySymbol
	,SM.PutOrCall
	,VT.SettlementDate
	,VT.GroupRefID
	,VT.TaxLotState
	,VT.AUECLocalDate
	,SM.Multiplier
	,SM.ISINSymbol
	,SM.CUSIPSymbol
	,SM.SEDOLSymbol
	,SM.BloombergSymbol
	,SM.CompanyName
	,SM.UnderlyingSymbol
	,SM.OSISymbol
	,TGD.TradeDate
	,S.Side
	,OldC.ShortName
	,TGD.ExecutedQuantity
	,TGD.AvgPrice
	,TGD.SettlmentDate
	,TGD.Commission
	,TGD.OtherBrokerFees
	,TGD.StampDuty
	,TGD.TransactionLevy
	,TGD.ClearingFee
	,TGD.MiscFees
	,TGD.UnderlyingDelta
	,TGD.LotID
	,TGD.CommissionAmount
	,TGD.CommissionRate
	,TGD.SecFee
	,TGD.OccFee
	,TGD.OrfFee
	,TGD.ClearingBrokerFee
	,TGD.SoftCommission
	,TGD.SoftCommissionAmount
	,TGD.TaxOnCommissions
	,VT.TradeDateTime

Select 
GroupRefID,
Sum(OldCommission) As OldCommission,
Sum(OldOtherBrokerFees) As OldOtherBrokerFees,
Sum(OldStampDuty) As OldStampDuty,
Sum(OldTransactionLevy) As OldTransactionLevy,
Sum(OldClearingFee) As OldClearingFee,
Sum(OldTaxOnCommissions) As OldTaxOnCommissions,
Sum(OldMiscFees) As OldMiscFees,
Sum(OldSecFee) As OldSecFee,
Sum(OldOccFee) As OldOccFee,
Sum(OldOrfFee) As OldOrfFee,
Sum(OldClearingBrokerFee) As OldClearingBrokerFee,
Sum(OldSoftCommission) As OldSoftCommission,
Sum(OldExecutedQuantity) As OldExecutedQuantity
InTo #Temp_GroupedData
From #VT_FinalData VT
Group By GroupRefID


Update VT
Set 
VT.OldCommission_BlockLevel = TG.OldCommission,
VT.OldOtherBrokerFees_BlockLevel = TG.OldOtherBrokerFees,
VT.OldStampDuty_BlockLevel = TG.OldStampDuty,
VT.OldTransactionLevy_BlockLevel = TG.OldTransactionLevy,
VT.OldClearingFee_BlockLevel = TG.OldClearingFee,
VT.OldTaxOnCommissions_BlockLevel = TG.OldTaxOnCommissions,
VT.OldMiscFees_BlockLevel = TG.OldMiscFees,
VT.OldSecFee_BlockLevel = TG.OldSecFee,
VT.OldOccFee_BlockLevel = TG.OldOccFee,
VT.OldOrfFee_BlockLevel = TG.OldOrfFee,
VT.OldClearingBrokerFee_BlockLevel = TG.OldClearingBrokerFee,
VT.OldSoftCommission_BlockLevel = TG.OldSoftCommission,
VT.OldExecutedQuantity_BlockLevel = TG.OldExecutedQuantity
From #VT_FinalData VT
Inner Join #Temp_GroupedData TG On TG.GroupRefID = VT.GroupRefID

SELECT 
	G.GroupID
	,'' AS TaxlotID
	,'' AS EntityID
	,'' AS AccountName
	,T_Side.Side AS Side
	,G.Symbol
	,C.Shortname AS CounterParty
	,G.CumQty AS OrderQty
	,G.AvgPrice
	,G.CumQty
	,T_Asset.AssetName AS Asset
	,Currency.CurrencySymbol
	,'' AS Level1AllocationID
	,0 As TaxLotQty
	,ISNULL(SM.PutOrCall, 0) As PutOrCall
	,G.SettlementDate
	,G.Commission AS CommissionCharged
	,G.OtherBrokerFees AS OtherBrokerFees
	,G.GroupRefID
	,G.GroupRefID AS PBUniqueID
	,Case 
	When G.StateID = 1
	Then 'Deleted'
	Else 'Allocated' 
	End AS TaxLotState
	,G.StampDuty AS StampDuty
	,G.TransactionLevy AS TransactionLevy
	,G.ClearingFee AS ClearingFee
	,G.TaxOnCommissions AS TaxOnCommissions
	,G.MiscFees AS MiscFees
	,G.AUECLocalDate AS TradeDate
	,ISNULL(SM.Multiplier, 1) AS AssetMultiplier
	,ISNULL(SM.ISINSymbol, '') AS ISIN
	,ISNULL(SM.CUSIPSymbol, '') AS CUSIP
	,ISNULL(SM.SEDOLSymbol, '') AS SEDOL
	,SM.BloombergSymbol AS BBCode
	,SM.CompanyName
	,SM.UnderlyingSymbol
	,SM.OSISymbol AS OSIOptionSymbol
	,G.SecFee AS SecFee
	,G.OccFee AS OccFee
	,G.OrfFee AS OrfFee
	,G.ClearingBrokerFee AS ClearingBrokerFee
	,G.SoftCommission AS SoftCommissionCharged
	,Cast('' As Varchar(50)) AS OldSide
	,Cast(0 as Float) AS OldExecutedQuantity
	,Cast(0 as Float) AS OldAvgPrice
	,Cast(0 as Float) AS OldCommission
	,Cast(0 as Float) AS OldOtherBrokerFees
	,Cast(0 as Float) AS OldStampDuty
	,Cast(0 as Float) AS OldTransactionLevy
	,Cast(0 as Float) AS OldClearingFee
	,Cast(0 as Float) AS OldMiscFees
	,Cast(0 as Float) AS OldSecFee
	,Cast(0 as Float) AS OldOccFee
	,Cast(0 as Float) AS OldOrfFee
	,Cast(0 as Float) AS OldClearingBrokerFee
	,Cast(0 as Float) AS OldSoftCommission
	,Cast(0 as Float) AS OldTaxOnCommission
	,Cast(0 as Float) As Commission_BlockLevel
	,Cast(0 as Float) As OtherBrokerFees_BlockLevel
	,Cast(0 as Float)As StampDuty_BlockLevel
	,Cast(0 as Float) As TransactionLevy_BlockLevel
	,Cast(0 as Float) As ClearingFee_BlockLevel
	,Cast(0 as Float) As TaxOnCommissions_BlockLevel
	,Cast(0 as Float) As MiscFees_BlockLevel
	,Cast(0 as Float) As SecFee_BlockLevel
	,Cast(0 as Float) As OccFee_BlockLevel
	,Cast(0 as Float) As OrfFee_BlockLevel
	,Cast(0 as Float) As ClearingBrokerFee_BlockLevel
	,Cast(0 as Float) As SoftCommission_BlockLevel
	,Cast(0 as Float) As OldCommission_BlockLevel
	,Cast(0 as Float) As OldOtherBrokerFees_BlockLevel
	,Cast(0 as Float) As OldStampDuty_BlockLevel
	,Cast(0 as Float) As OldTransactionLevy_BlockLevel
	,Cast(0 as Float) As OldClearingFee_BlockLevel
	,Cast(0 as Float) As OldTaxOnCommissions_BlockLevel
	,Cast(0 as Float) As OldMiscFees_BlockLevel
	,Cast(0 as Float) As OldSecFee_BlockLevel
	,Cast(0 as Float) As OldOccFee_BlockLevel
	,Cast(0 as Float) As OldOrfFee_BlockLevel
	,Cast(0 as Float) As OldClearingBrokerFee_BlockLevel
	,Cast(0 as Float) As OldSoftCommission_BlockLevel
	,Cast(0 as Float) As OldExecutedQuantity_BlockLevel
	,G.AUECLocalDate As TradeDateTime
	,'Group' As GroupOrTaxlotType
	,1 As CustomizedGrouping 
	 Into #Group_Data
	From T_Group G
	INNER JOIN #SecMasterData AS SM ON SM.TickerSymbol = G.Symbol
	INNER JOIN T_Asset ON G.AssetID = T_Asset.AssetID
	INNER JOIN T_Currency AS Currency ON Currency.CurrencyID = G.CurrencyID
	INNER JOIN T_CounterParty C ON C.CounterPartyID = G.CounterPartyID
	INNER JOIN T_Side ON dbo.T_Side.SideTagValue = G.OrderSideTagValue
	Where G.GroupID In
	(
		Select Distinct GroupID From #VT_FinalData
	)

Update G
Set
	G.Commission_BlockLevel = TG.Commission_BlockLevel
	,G.OtherBrokerFees_BlockLevel = TG.OtherBrokerFees_BlockLevel
	,G.StampDuty_BlockLevel = TG.StampDuty_BlockLevel
	,G.TransactionLevy_BlockLevel = TG.TransactionLevy_BlockLevel
	,G.ClearingFee_BlockLevel = TG.ClearingFee_BlockLevel
	,G.TaxOnCommissions_BlockLevel = TG.TaxOnCommissions_BlockLevel
	,G.MiscFees_BlockLevel = TG.MiscFees_BlockLevel
	,G.SecFee_BlockLevel = TG.SecFee_BlockLevel
	,G.OccFee_BlockLevel = TG.OccFee_BlockLevel
	,G.OrfFee_BlockLevel = TG.OrfFee_BlockLevel
	,G.ClearingBrokerFee_BlockLevel = TG.ClearingBrokerFee_BlockLevel
	,G.SoftCommission_BlockLevel = TG.SoftCommission_BlockLevel
	,G.OldAvgPrice = TG.OldAvgPrice
	,G.OldSide = TG.OldSide
	,G.OldExecutedQuantity = TG.OldExecutedQuantity
	,G.OldExecutedQuantity_BlockLevel = TG.OldExecutedQuantity_BlockLevel
	,G.OldCommission_BlockLevel = TG.OldCommission_BlockLevel
	,G.OldOtherBrokerFees_BlockLevel = TG.OldOtherBrokerFees_BlockLevel
	,G.OldTransactionLevy_BlockLevel = TG.OldTransactionLevy_BlockLevel 
	,G.OldClearingBrokerFee_BlockLevel = TG.OldClearingBrokerFee_BlockLevel 
	,G.OldMiscFees_BlockLevel = TG.OldMiscFees_BlockLevel 
	,G.OldSecFee_BlockLevel = TG.OldSecFee_BlockLevel 
	,G.OldOrfFee_BlockLevel = TG.OldOrfFee_BlockLevel 
	,G.OldSoftCommission_BlockLevel = TG.OldSoftCommission_BlockLevel 
	,G.OldTaxOnCommissions_BlockLevel = TG.OldTaxOnCommissions_BlockLevel
From #Group_Data G
Inner Join #VT_FinalData TG On TG.GroupRefID = G.GroupRefID
Where TG.TaxLotState In ('Deleted','Amended')

Update G
Set G.TaxLotState = VT.TaxLotState
From #Group_Data G
Inner Join #VT_FinalData VT On VT.GroupID = G.GroupID
Where VT.TaxLotState = 'Sent'

Insert InTo #VT_FinalData
Select *
From #Group_Data

---- Reallocation: if trade is re-allocated from 2 accounts to 3 or vice versa
Select 
GroupID,
Round(Sum(TaxlotQty),5) As TaxlotQtySum,
Round(Max(CumQty),5) As CumQty
InTo #Temp_DeletedGroupID
From #VT_FinalData
Group By GroupID

Select *
InTo #Temp_DeletedGroupID_1
from #Temp_DeletedGroupID
Where TaxlotQtySum <> CumQty

Select *
InTo #Temp_DeletedData
From #VT_FinalData
Where GroupID In
(
Select GroupId From #Temp_DeletedGroupID_1 Where (TaxLotState In ('Amended', 'Deleted') Or (GroupOrTaxlotType = 'Group'))
)

Update #Temp_DeletedData
Set TaxlotState = 'Deleted'

Update G  
Set  
 G.Commission_BlockLevel = TG.Commission_BlockLevel  
 ,G.OtherBrokerFees_BlockLevel = TG.OtherBrokerFees_BlockLevel  
 ,G.StampDuty_BlockLevel = TG.StampDuty_BlockLevel  
 ,G.TransactionLevy_BlockLevel = TG.TransactionLevy_BlockLevel  
 ,G.ClearingFee_BlockLevel = TG.ClearingFee_BlockLevel  
 ,G.TaxOnCommissions_BlockLevel = TG.TaxOnCommissions_BlockLevel  
 ,G.MiscFees_BlockLevel = TG.MiscFees_BlockLevel  
 ,G.SecFee_BlockLevel = TG.SecFee_BlockLevel  
 ,G.OccFee_BlockLevel = TG.OccFee_BlockLevel  
 ,G.OrfFee_BlockLevel = TG.OrfFee_BlockLevel  
 ,G.ClearingBrokerFee_BlockLevel = TG.ClearingBrokerFee_BlockLevel  
 ,G.SoftCommission_BlockLevel = TG.SoftCommission_BlockLevel  
 ,G.OldAvgPrice = TG.OldAvgPrice  
,G.OldSide = TG.OldSide  
,G.OldExecutedQuantity = TG.OldExecutedQuantity  
,G.OldExecutedQuantity_BlockLevel = TG.OldExecutedQuantity_BlockLevel  
,G.OldCommission_BlockLevel = TG.OldCommission_BlockLevel  
,G.OldOtherBrokerFees_BlockLevel = TG.OldOtherBrokerFees_BlockLevel  
,G.OldTransactionLevy_BlockLevel = TG.OldTransactionLevy_BlockLevel   
,G.OldClearingBrokerFee_BlockLevel = TG.OldClearingBrokerFee_BlockLevel   
,G.OldMiscFees_BlockLevel = TG.OldMiscFees_BlockLevel   
,G.OldSecFee_BlockLevel = TG.OldSecFee_BlockLevel   
,G.OldOrfFee_BlockLevel = TG.OldOrfFee_BlockLevel   
,G.OldSoftCommission_BlockLevel = TG.OldSoftCommission_BlockLevel   
,G.OldTaxOnCommissions_BlockLevel = TG.OldTaxOnCommissions_BlockLevel  
From #Temp_DeletedData G  
Inner Join #VT_FinalData TG On TG.GroupRefID = G.GroupRefID  
Where TG.TaxLotState In ('Deleted') And G.GroupOrTaxlotType = 'Group'

Delete From #VT_FinalData
Where GroupId In (Select GroupID From #Temp_DeletedData)
And TaxlotState = 'Deleted'

Update T
Set T.TaxLotState = 'Allocated'
From #VT_FinalData T
Inner Join #Temp_DeletedData D On D.GroupID = T.GroupID

---- Reallocation: if trade is re-allocated from 2 accounts to 3 or vice versa or editing from Allocation
Select GroupID
InTo #Temp_ReallocatedAmendedGroupId
From #VT_FinalData
Where TaxlotState = 'Amended'
Group By GroupID

Select VFT.GroupID
InTo #Temp_SentGroupID
From #VT_FinalData VFT
Inner Join #Temp_ReallocatedAmendedGroupId T On T.GroupID = VFT.GroupID
Where VFT.TaxLotState = 'Sent'

-- Delete which have sent taxlotstate. Sent and Amended states come when we modify Taxlot from Allocation
-- If we modify a trade, in this case, all the taxlots comes in Amended state
-- If we re-allocate any trade, in this case state comes either allocated or amended
Delete From #Temp_ReallocatedAmendedGroupId
Where GroupID In 
(
Select GroupID From #Temp_SentGroupID
)

Select *
InTo #Temp_Reallocated_AmendedData
From #VT_FinalData
Where GroupID In
(
Select T.GroupId From #Temp_ReallocatedAmendedGroupId T
)
And (TaxLotState In ('Amended') Or (GroupOrTaxlotType = 'Group'))

Update #Temp_Reallocated_AmendedData
Set TaxlotState = 'Deleted'

Update T
Set T.TaxLotState = 'Allocated'
From #VT_FinalData T
Inner Join #Temp_Reallocated_AmendedData D On D.GroupID = T.GroupID

Select Distinct GroupID
InTo #Temp_GroupID_Amended
From #VT_FinalData
Where TaxLotState = 'Amended' And GroupOrTaxlotType = 'Taxlot'

Update VT
Set VT.TaxLotState = 'Amended'
From #VT_FinalData VT
Inner Join #Temp_GroupID_Amended AG On AG.GroupID = VT.GroupID

Insert InTo #VT_FinalData
Select *
from #Temp_DeletedData

Insert InTo #VT_FinalData
Select *
From #Temp_Reallocated_AmendedData

-- Create #VT_FinalDataWithOrderDetail
SELECT T.*, 
       sub.ClOrderID, 
       ISNULL(Fills.OrderID, TTO.OrderID) AS OrderID,
       sub.ParentClOrderID, 
       sub.StagedOrderID, 
       TTO.TimeInForce
INTO #VT_FinalDataWithOrderDetail
FROM #VT_FinalData T
LEFT JOIN T_TradedOrders TTO ON TTO.GroupID = T.GroupID
LEFT JOIN T_Sub sub ON sub.ClOrderID = TTO.CLOrderID
LEFT JOIN T_Fills Fills ON (TTO.NirvanaSeqNumber = Fills.NirvanaSeqNumber AND TTO.CLOrderID = Fills.ClOrderID)
WHERE T.GroupOrTaxlotType = 'Group'

-- Insert remaining rows
INSERT INTO #VT_FinalDataWithOrderDetail
SELECT T.*, NULL, NULL, NULL, NULL, NULL
FROM #VT_FinalData T
WHERE T.GroupOrTaxlotType <> 'Group'


update T  
set ClOrderID = TTO.ClOrderID,
    ParentClOrderID = TTO.ParentClOrderID,
	OrderID = ISNULL(Fills.OrderID,TTO.OrderID)
From #VT_FinalDataWithOrderDetail T
LEFT JOIN T_TradedOrders TTO ON T.StagedOrderID = TTO.ParentClOrderID
LEFT JOIN T_Fills Fills ON (TTO.NirvanaSeqNumber = fills.NirvanaSeqNumber AND TTO.CLOrderID = Fills.ClOrderID)
Where (TTO.TimeInForce = 1 or TTO.TimeInForce = 6) AND (T.TimeInForce = 1 or T.TimeInForce = 6) AND GroupOrTaxlotType = 'Group'

Alter Table #VT_FinalDataWithOrderDetail
Add GroupID_Ref varchar(100)

Update #VT_FinalDataWithOrderDetail
Set GroupID_Ref = GroupID

Update T
Set GroupID_Ref = GroupID_Ref + 'N'
From #VT_FinalDataWithOrderDetail T
Inner Join #Temp_Reallocated_AmendedData RA On RA.GroupID = T.GroupID
Where T.TaxLotState = 'Allocated' 

Update T
Set GroupID_Ref = GroupID_Ref + 'N'
From #VT_FinalDataWithOrderDetail T
Inner Join #Temp_DeletedData RA On RA.GroupID = T.GroupID
Where T.TaxLotState = 'Allocated' 

Select *
From #VT_FinalDataWithOrderDetail
ORDER BY GroupID,TaxlotState DESC, CustomizedGrouping 

DROP TABLE #VT,#SecMasterData,#TaxlotsDates,#T_TradeAuditAction	
Drop Table #Temp_DeletedGroupedData,#VT_New,#VT_FinalData,#Temp_GroupedData,#Group_Data
Drop Table #Temp_GroupID_Amended, #Temp_DeletedGroupID,#Temp_DeletedGroupID_1,#Temp_DeletedData
Drop Table #Temp_Reallocated_AmendedData, #Temp_ReallocatedAmendedGroupId,#Temp_SentGroupID,#Temp_CounterPartyID, #VT_FinalDataWithOrderDetail