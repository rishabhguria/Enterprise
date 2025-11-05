 CREATE PROCEDURE [dbo].[P_FFThirdParty_ChimeraGroupBYSymbol] 
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
	,@includeSent INT = 0
	)
AS

--Declare 
--	@thirdPartyID INT
--	,@companyFundIDs VARCHAR(max)
--	,@inputDate DATETIME
--	,@companyID INT
--	,@auecIDs VARCHAR(max)
--	,@TypeID INT
--	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                            
--	@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                            
--	,@fileFormatID INT
--	,@includeSent INT = 0

--	SET @thirdPartyID = 88
--	SET @companyFundIDs = N'8,2,1,7,5,6,11,12,3,9,4,10'
--	SET @inputDate = '2021-05-19 04:16:33'
--	SET @companyID = 7
--	SET @auecIDs = N'1,15,11,62,73,12'
--	SET @TypeID = 0
--	SET @dateType =  0                                                                                                                                                           
--	SET @fileFormatID = 191
--	--SET @includeSent = 

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

CREATE TABLE #SecMasterData 
	(
	TickerSymbol VARCHAR(200)
	,ExpirationDate DATETIME
	,Multiplier FLOAT
	,CompanyName VARCHAR(500)
	)

INSERT INTO #SecMasterData
SELECT 
	SM.TickerSymbol
	,SM.ExpirationDate
	,SM.Multiplier
	,SM.CompanyName
FROM V_SecMasterData SM

CREATE TABLE #VT (
    AccountName VARCHAR(100)
	,FundID INT
	,SideID VARCHAR(20)
	,Symbol VARCHAR(100)
	,CounterPartyID INT
	,OrderQty FLOAT
	,AvgPrice FLOAT
	,CumQty FLOAT
	,Quantity FLOAT
	,TaxLotQty FLOAT
	,SettlementDate DATETIME
	,Commission FLOAT
	,OtherBrokerFees FLOAT
	,StampDuty FLOAT
	,TransactionLevy FLOAT
	,ClearingFee FLOAT
	,TaxOnCommissions FLOAT
	,MiscFees FLOAT
	,AUECLocalDate DATETIME
	,FXRate FLOAT
	,FXConversionMethodOperator VARCHAR(3)
	,ProcessDate DATETIME
	,OriginalPurchaseDate DATETIME
	,SecFee FLOAT
	,OccFee FLOAT
	,OrfFee FLOAT
	,ClearingBrokerFee FLOAT
	,SoftCommission FLOAT
	,SettlCurrency INT
	,AUECID INT
	,AssetID INT
	,CurrencyID INT
	)

INSERT INTO #VT
SELECT
    CF.FundName AS AccountName
	,VT.FundID AS FundID
	,VT.OrderSideTagValue AS SideID
	,VT.Symbol
	,VT.CounterPartyID
	,VT.TaxLotQty AS OrderQty
	,VT.AvgPrice
	,VT.CumQty
	,VT.Quantity
	,VT.TaxLotQty
	,VT.SettlementDate
	,VT.Commission
	,VT.OtherBrokerFees
	,VT.StampDuty AS StampDuty
	,VT.TransactionLevy AS TransactionLevy
	,ClearingFee AS ClearingFee
	,TaxOnCommissions AS TaxOnCommissions
	,MiscFees AS MiscFees
	,VT.AUECLocalDate
	,VT.FXRate
	,VT.FXConversionMethodOperator
	,VT.ProcessDate
	,VT.OriginalPurchaseDate
	,VT.SecFee AS SecFee
	,VT.OccFee AS OccFee
	,VT.OrfFee AS OrfFee
	,VT.ClearingBrokerFee
	,VT.SoftCommission
	,VT.SettlCurrency_Taxlot AS SettlCurrency
	,VT.AUECID
	,AssetID
	,CurrencyID
FROM V_TaxLots VT
INNER JOIN #SecMasterData SM ON VT.Symbol = SM.TickerSymbol
INNER JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID
Inner Join @AUECID AUEC On AUEC.AUECID = VT.AUECID
WHERE 
Datediff(DAY, (
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

UPDATE #VT
SET AccountName = 
CASE 
		WHEN CharIndex('Booth Bay SMA', AccountName) <> 0
			THEN '3E702CVU'
		WHEN CharIndex('Booth Bay DA', AccountName) <> 0
			THEN '3E702CVU'
		WHEN CharIndex('Walleye WOF', AccountName) <> 0
			THEN '3E702UX5'
		WHEN CharIndex('Walleye WIF', AccountName) <> 0
			THEN '3E702UX5'
		WHEN CharIndex('Walleye WIF Systematic', AccountName) <> 0
			THEN '3E702UX5'
		WHEN CharIndex('Walleye WOF Systematic', AccountName) <> 0
			THEN '3E702UX5'
			WHEN CharIndex('Walleye Concentrated', AccountName) <> 0
			THEN '3E702UX5'
		WHEN CharIndex('Diamond Growth Fund', AccountName) <> 0
			THEN '3E70205C'
		WHEN CharIndex('Diamond Neutral Fund', AccountName) <> 0
			THEN '3E70205C'
		WHEN CharIndex('MIO Offshore', AccountName) <> 0
			THEN '3E70205C'
		WHEN CharIndex('MIO Onshore', AccountName) <> 0
			THEN '3E70205C'
		WHEN CharIndex('Stevens Capital', AccountName) <> 0
			THEN '3E70205C'
		ELSE AccountName
		END

SELECT 
     VT.AccountName AS AccountName
	 ,T_CounterParty.ShortName AS CounterParty  
	,T_Side.Side AS Side
	,VT.Symbol AS Symbol
	,Sum(VT.TaxLotQty) AS OrderQty
	,Sum(VT.TaxLotQty * VT.AvgPrice) / NULLIF(Sum(VT.TaxLotQty),0) As AvgPrice
	,Sum(VT.CumQty) AS ExecutedQty
	,Max(Currency.CurrencySymbol) as CurrencySymbol
	,Max(Convert(VARCHAR, VT.SettlementDate, 101)) as SettlementDate
	,Sum(VT.Commission) as CommissionCharged
	,Sum(VT.OtherBrokerFees) as OtherBrokerFees
	,Max(T_Asset.AssetName) AS Asset
	,Sum(VT.StampDuty) AS StampDuty
	,Sum(VT.TransactionLevy) AS TransactionLevy
	,Sum(ClearingFee) AS ClearingFee
	,Sum(TaxOnCommissions) AS TaxOnCommissions
	,Sum(MiscFees) AS MiscFees
	,Max(Convert(VARCHAR, VT.AUECLocalDate, 101)) AS TradeDate
	,Max(SM.Multiplier) as Multiplier
	,Max(VT.FXRate) As FXRate
	,Max(VT.FXConversionMethodOperator) as FXConversionMethodOperator
	,Max(VT.ProcessDate) as ProcessDate
	,Max(VT.OriginalPurchaseDate) as OriginalPurchaseDate
	,Sum(VT.SecFee) AS SecFee
	,Sum(VT.OccFee) AS OccFee
	,Sum(VT.OrfFee) AS OrfFee
	,Sum(VT.ClearingBrokerFee) AS ClearingBrokerFee
	,Sum(VT.SoftCommission) As SoftCommissionCharged

FROM #VT VT
Inner JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
Inner JOIN T_Currency AS TC ON TC.CurrencyID = VT.SettlCurrency
Inner JOIN T_Side ON dbo.T_Side.SideTagValue = VT.SideID
Inner JOIN T_Asset ON T_Asset.AssetID = VT.AssetID 
Inner JOIN #SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
Left Outer JOIN T_CounterParty ON T_CounterParty.CounterPartyID = VT.CounterPartyID
Where T_Asset.AssetID = 1
GROUP BY 
	VT.AccountName ,T_Side.Side ,VT.Symbol ,T_CounterParty.ShortName

DROP TABLE #VT,#SecMasterData