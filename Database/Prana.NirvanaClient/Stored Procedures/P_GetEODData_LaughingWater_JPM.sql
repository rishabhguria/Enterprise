CREATE PROCEDURE [dbo].[P_GetEODData_LaughingWater_JPM] 
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

--Declare @thirdPartyID INT
--Declare @companyFundIDs VARCHAR(max)
--Declare @inputDate DATETIME
--Declare @companyID INT
--Declare @auecIDs VARCHAR(max)
--Declare @TypeID INT
--Declare @dateType INT                                                                                                                                                         
--Declare @fileFormatID INT
--Declare @includeSent INT = 0

--Set @thirdPartyID=82
--Set @companyFundIDs=N'3,4'
--Set @inputDate='2023-1-31 02:19:08'
--Set @companyID=7
--Set @auecIDs=N'53,44,43,21,18,74,1,15,11,121,73,12'
--Set @TypeID=0
--Set @dateType=0
--Set @fileFormatID=11
--Set @includeSent=0


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
	,CompanyName VARCHAR(500)	
	,ISIN VARCHAR(50)
	, Multiplier Float
)

INSERT INTO #SecMasterData
SELECT 
	SM.TickerSymbol
	,SM.ExpirationDate	
	,SM.CompanyName
	,SM.ISINSymbol
	,SM.Multiplier
FROM V_SecMasterData SM With (NoLock)

CREATE TABLE #VT 
(
	AccountName VARCHAR(100)
	,Symbol VARCHAR(100)
	,Side VARCHAR(20)		
	,TradeDate DATE
	,SettlementDate DATE
	,AllocatedQty FLOAT
	,AveragePrice FLOAT
	,Commission FLOAT
	,AccruedInterest FLOAT		
	,SettlCurrency VARCHAR(20)
	,OtherBrokerFees FLOAT
	,StampDuty FLOAT
	,TransactionLevy FLOAT
	,ClearingFee FLOAT
	,TaxOnCommissions FLOAT
	,MiscFees FLOAT
	,SecFee FLOAT
	,OccFee FLOAT
	,OrfFee FLOAT
	,ClearingBrokerFee FLOAT
	,CurrencySymbol  VARCHAR(20)
	,CounterParty  VARCHAR(100)
	,FXRate_Taxlot FLOAT	
	,FXConversionMethodOperator VARCHAR(3)
	,SoftCommission FLOAT
	, PrincipalAmount FLOAT	
	, NetAmount FLOAT
	,ISIN  VARCHAR(50)
	,ExpirationDate  DateTime
	,CompanyName VARCHAR(200)
	
)

INSERT INTO #VT
SELECT

    CF.FundName AS AccountName
	,VT.Symbol
	,S.Side AS Side	
	,Cast(VT.AUECLocalDate As Date)
	,Cast(VT.SettlementDate As Date)
	,VT.TaxLotQty
	,VT.AvgPrice
	,VT.Commission
	,VT.AccruedInterest	
	,CUR_Settle.CurrencySymbol AS SettlCurrency
	,VT.OtherBrokerFees
	,VT.StampDuty AS StampDuty
	,VT.TransactionLevy AS TransactionLevy
	,VT.ClearingFee AS ClearingFee
	,VT.TaxOnCommissions AS TaxOnCommissions
	,VT.MiscFees AS MiscFees
	,VT.SecFee AS SecFee
	,VT.OccFee AS OccFee
	,VT.OrfFee AS OrfFee
	,VT.ClearingBrokerFee
	,CUR.CurrencySymbol
	,CP.ShortName As CounterParty
	,VT.FXRate_Taxlot
	,VT.FXConversionMethodOperator_Taxlot
	,VT.SoftCommission
	,(VT.AvgPrice * VT.TaxLotQty * SM.Multiplier) As PrincipalAmount
	,(VT.AvgPrice * VT.TaxLotQty * SM.Multiplier) + (VT.SideMultiplier * VT.TotalExpenses) As NetAmount
	,SM.ISIN 
	,Convert(varchar, SM.ExpirationDate ,101) As ExpirationDate
	,SM.CompanyName As CompanyName
	
FROM V_TaxLots VT
INNER JOIN #SecMasterData SM ON VT.Symbol = SM.TickerSymbol
Inner Join @Fund F On F.FundID = VT.FundID
INNER JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID
Inner Join @AUECID AUEC On AUEC.AUECID = VT.AUECID
Inner JOIN T_Side S ON S.SideTagValue = VT.OrderSideTagValue
Inner Join T_Currency CUR_Settle On CUR_Settle.CurrencyID = VT.SettlCurrency_Taxlot
Inner Join T_Currency CUR On CUR.CurrencyID = VT.CurrencyID
Left Outer JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID

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


----Select * FROM #VT

SELECT 
    AccountName,
	Symbol,
	Side,
	Convert(Varchar,TradeDate, 101) As TradeDate,
	Convert(Varchar,SettlementDate, 101) As SettlementDate,
	Sum(AllocatedQty) As AllocatedQty,
	Sum(AveragePrice * AllocatedQty) As AveragePrice,
	Sum(Commission) As CommissionCharged,
	Sum(AccruedInterest) As AccruedInterest,
	SettlCurrency,
	Sum(OtherBrokerFees) As OtherBrokerFee,
	Sum(StampDuty) As StampDuty,
	Sum(TransactionLevy) As TransactionLevy,
	Sum(ClearingFee) As ClearingFee ,
	Sum(TaxOnCommissions) As TaxOnCommissions ,
	Sum(MiscFees) As MiscFees,
	Sum(SecFee) As SecFee,
	Sum(OccFee) As OccFee ,
	Sum(OrfFee) as OrfFee ,
	Sum(ClearingBrokerFee) As ClearingBrokerFee,
	CurrencySymbol,
	CounterParty,
	Max(FXRate_Taxlot) As FXRate_Taxlot ,
	Max(FXConversionMethodOperator) As FXConversionMethodOperator,
	Sum(SoftCommission) As SoftCommissionCharged, 
	Sum(PrincipalAmount) As PrincipalAmount, 
	Sum(NetAmount) As NetAmount, 
	Max(ISIN) As ISIN,
	MAX(ExpirationDate) As ExpirationDate,
	Max(CompanyName) As CompanyName
InTo #Temp_ThirdPartyData

FROM #VT
GROUP BY AccountName, TradeDate,Symbol,Side ,CounterParty, CurrencySymbol, SettlCurrency,SettlementDate

Update #Temp_ThirdPartyData
Set AveragePrice = 
Case 
When AllocatedQty > 0
Then AveragePrice/AllocatedQty
Else 0.0
End

Select *
From #Temp_ThirdPartyData
Order By AccountName, Symbol, Side 

DROP TABLE #VT,#SecMasterData, #Temp_ThirdPartyData