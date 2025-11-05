

CREATE PROCEDURE [dbo].[P_GetAllocAndUnAllocTaxlots_VC_KISEOD] 
(
	 @thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                      
	,@fileFormatID INT
	,@includeSent BIT = 1
	)
AS

 --Declare 
	--@thirdPartyID INT
	--,@companyFundIDs VARCHAR(max)
	--,@inputDate DATETIME
	--,@companyID INT
	--,@auecIDs VARCHAR(max)
	--,@TypeID INT
	--,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                      
	--,@fileFormatID INT
	--,@includeSent BIT

	--Set @thirdPartyID = 45
	--Set @companyFundIDs = N'25,21,22,20,18,19,31,30,24,27,28,23,26,29'
	--Set @inputDate = '08-01-2024'
	--Set @companyID = 7
	--Set @auecIDs=N'66,175,20,163,171,65,71,67,76,63,44,56,21,180,1,15,62,12,22,243,26,16,17'
	--Set @TypeID = 1
	--Set @dateType = 0
	--Set  @fileFormatID = 118
	--Set @includeSent = 0

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

INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',')

INSERT INTO @AUECID
SELECT Cast(Items AS INT)
FROM dbo.Split(@auecIDs, ',')

Create Table #TempEOD
(
AccountName Varchar(200),
Side Varchar(20),
TaxLotQty Float,
AveragePrice Float,
Symbol Varchar(200),
FullSecurityName Varchar(500),
BloombergSymbol Varchar(200),
ISIN Varchar(50),
SEDOL Varchar(50),
Asset Varchar(50),
CurrencySymbol Varchar(10),
TradeDate Varchar(20),
SettlementDate Varchar(20),
AssetMultiplier Float,
ExecutedQty Float,
CommissionCharged Float,
OtherBrokerFee Float,
StampDuty Float,
TransactionLevy Float,
ClearingFee Float,
TaxOnCommissions Float,
MiscFees Float,
SecFee Float,
OccFee Float,
OrfFee Float,
ClearingBrokerFee Float,
SoftCommissionCharged Float,
SideMultiplier Int,
TotalExpenses Float,
GrossAmount Float,
NetAmount Float,
Country Varchar(200),
AllocationTag Varchar(20),
CustomOrderBy Int
)

Insert InTo #TempEOD
Select 
CF.FundName  As AccountName,
S.Side,
VT.TaxLotQty As TaxLotQty,
VT.AvgPrice As AveragePrice,
VT.Symbol,
SM.CompanyName As FullSecurityName,
SM.BloombergSymbol As BloombergSymbol,
SM.ISINSymbol As ISIN,
SM.SEDOLSymbol As SEDOL,
A.AssetName As Asset,
TradeCurr.CurrencySymbol As CurrencySymbol,
Convert(Varchar,VT.AUECLocalDate,101) As TradeDate,
Convert(Varchar,VT.SettlementDate,101) As SettlementDate,
SM.Multiplier As AssetMultiplier,
VT.CumQty As ExecutedQty,
VT.Commission As CommissionCharged,
VT.OtherBrokerFees As OtherBrokerFee,
VT.StampDuty As StampDuty,
VT.TransactionLevy As TransactionLevy,
VT.ClearingFee As ClearingFee,
VT.TaxOnCommissions As TaxOnCommissions,
VT.MiscFees As MiscFees,
VT.SecFee As SecFee,
VT.OccFee As OccFee,
VT.OrfFee As OrfFee,
VT.ClearingBrokerFee As ClearingBrokerFee,
VT.SoftCommission As SoftCommissionCharged,
VT.SideMultiplier As SideMultiplier,
VT.TotalExpenses As TotalExpenses,
(IsNull(VT.AvgPrice * VT.TaxLotQty * SM.Multiplier ,0)) As GrossAmount,
(IsNull((VT.AvgPrice * VT.TaxLotQty * SM.Multiplier) + (VT.SideMultiplier * VT.TotalExpenses) ,0)) As NetAmount,
 SM.CountryName As Country,
 'Allocated' As AllocationTag,
1 As CustomOrderBy
From V_Taxlots VT With (NoLock)
Inner Join T_Side S On S.SideTagValue = VT.OrderSideTagValue 
Inner Join @AUECID AUEC On AUEC.AUECID = VT.AUECID
Inner Join @Fund F On F.FundID = VT.FundID
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = VT.FundID 
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = VT.Symbol 
Inner Join T_Currency TradeCurr On TradeCurr.CurrencyID = VT.CurrencyID
Inner Join T_Asset A On A.AssetID = VT.AssetID 
WHERE Datediff(DAY, (
			CASE 
				WHEN @dateType = 1
					THEN VT.AUECLocalDate
				ELSE VT.ProcessDate
				END
			), @inputdate) = 0
	AND (
			(
				VT.TransactionType IN ('Buy','BuytoClose','BuytoOpen','Sell','Sellshort','SelltoClose','SelltoOpen','LongAddition','LongWithdrawal','ShortAddition','ShortWithdrawal','')
				AND (VT.TransactionSource IN (0,1,2,3,4,14)
			)
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
			AND TaxlotClosingID_FK IS NOT NULL
			AND VT.AssetID IN (1,3))
		OR (
			@IncludeCATransaction = 1
			AND (VT.TransactionSource IN (6,7,8,9,10,11))
			)
		OR TransactionSource = 13
		)


---- UnAllocated trades
Insert InTo #TempEOD

Select 
IsNull(TradeAttribute3,'') As AccountName,
S.Side,
G.CumQty As TaxLotQty,
G.AvgPrice As AveragePrice,
G.Symbol,
SM.CompanyName As FullSecurityName,
SM.BloombergSymbol As BloombergSymbol,
SM.ISINSymbol As ISIN,
SM.SEDOLSymbol As SEDOL,
A.AssetName As Asset,
TradeCurr.CurrencySymbol As CurrencySymbol,
Convert(Varchar,G.AUECLocalDate,101) As TradeDate,
Convert(Varchar,G.SettlementDate,101) As SettlementDate,
SM.Multiplier As AssetMultiplier,
G.CumQty As ExecutedQty,
G.Commission As CommissionCharged,
G.OtherBrokerFees As OtherBrokerFee,
G.StampDuty As StampDuty,
G.TransactionLevy As TransactionLevy,
G.ClearingFee As ClearingFee,
G.TaxOnCommissions As TaxOnCommissions,
G.MiscFees As MiscFees,
G.SecFee As SecFee,
G.OccFee As OccFee,
G.OrfFee As OrfFee,
G.ClearingBrokerFee As ClearingBrokerFee,
G.SoftCommission As SoftCommissionCharged,
[dbo].GetSideMultiplier(G.OrderSideTagValue) As SideMultiplier,
(G.Commission + G.OtherBrokerFees + G.StampDuty + G.TransactionLevy + G.ClearingFee + G.TaxOnCommissions + G.MiscFees + G.SecFee + G.OccFee + G.OrfFee + G.ClearingBrokerFee + 
G.SoftCommission + G.OptionPremiumAdjustment) AS TotalExpenses,
(IsNull(G.AvgPrice * G.CumQty * SM.Multiplier ,0)) As GrossAmount,
(IsNull((G.AvgPrice * G.CumQty * SM.Multiplier) +
 ([dbo].GetSideMultiplier(G.OrderSideTagValue) * G.Commission + G.OtherBrokerFees + G.StampDuty + G.TransactionLevy + G.ClearingFee +
 G.TaxOnCommissions + G.MiscFees + G.SecFee + G.OccFee + G.OrfFee + G.ClearingBrokerFee + G.SoftCommission + G.OptionPremiumAdjustment) ,0)) As NetAmount,
 SM.CountryName As Country,
 'UnAllocated' As AllocationTag,
2 As OrderByCol

From T_Group G With (NoLock)
Inner Join T_Side S On S.SideTagValue = G.OrderSideTagValue 
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = G.Symbol 
Inner Join T_Currency TradeCurr On TradeCurr.CurrencyID = G.CurrencyID
Inner Join T_Currency SettleCurr On SettleCurr.CurrencyID = G.SettlCurrency
Inner Join T_Asset A On A.AssetID = G.AssetID 
Where 
G.TradeAttribute3 <> '' And G.TradeAttribute3 Is Not Null
And G.StateID = 1
And G.CumQty > 0
And Datediff(DAY, (
			CASE 
				WHEN @dateType = 1
				THEN G.AUECLocalDate
				ELSE G.ProcessDate
			END
			), @inputdate) = 0

Alter Table #TempEOD
Add NetPrice Float,
CommissionRate Float

Update #TempEOD
Set NetPrice = 0, CommissionRate = 0

Update #TempEOD
Set NetPrice = 
Case 
When TaxLotQty <> 0
Then NetAmount/TaxLotQty
Else 0
End,
CommissionRate = 
Case 
When GrossAmount <> 0
Then (CommissionCharged + SoftCommissionCharged) /GrossAmount
Else 0
End

Select *
From #TempEOD
Order by CustomOrderBy

Drop Table #TempEOD