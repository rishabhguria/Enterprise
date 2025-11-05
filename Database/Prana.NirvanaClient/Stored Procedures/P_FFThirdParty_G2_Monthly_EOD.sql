/*
Author: Kuldeep Kumar
Created Date: 7 Jun 2024
Description: Requirement to fetch monthly data in Third Party Manager.

*/

Create PROCEDURE [dbo].[P_FFThirdParty_G2_Monthly_EOD] 
(
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties  
	,@dateType INT -- 0 for Process Date and 1 for Trade Date.                                                                                                                                                            
	,@fileFormatID INT
	,@includeSent INT = 0
	)
AS

DECLARE @Fund TABLE (FundID INT)
--DECLARE @AUECID TABLE (AUECID INT)
DECLARE @IncludeExpiredSettledTransaction INT
DECLARE @IncludeExpiredSettledUnderlyingTransaction INT
DECLARE @IncludeCATransaction INT
DECLARE @StartDate DATETIME

SET @StartDate = DATEADD(m, DATEDIFF(m, 0, @inputDate), 0)

SELECT @IncludeExpiredSettledTransaction = IncludeExercisedAssignedTransaction
	,@IncludeExpiredSettledUnderlyingTransaction = IncludeExercisedAssignedUnderlyingTransaction
	,@IncludeCATransaction = IncludeCATransaction
FROM T_ThirdPartyFileFormat
WHERE FileFormatId = @fileFormatID

INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',')

--INSERT INTO @AUECID
--SELECT Cast(Items AS INT)
--FROM dbo.Split(@auecIDs, ',')

CREATE TABLE #SecMasterData 
	(
	Symbol_PK BigInt
	,TickerSymbol VARCHAR(200)
	,PutOrCall VARCHAR(10)
	,StrikePrice Float
	,ExpirationDate DATETIME
	,Multiplier FLOAT
	,ISINSymbol VARCHAR(20)
	,CUSIPSymbol VARCHAR(50)
	,SEDOLSymbol VARCHAR(50)
	,BloombergSymbol VARCHAR(200)
	,CompanyName VARCHAR(500)
	,UnderlyingSymbol VARCHAR(100)
	,OSISymbol VARCHAR(25)
	,COUPON FLOAT
	)

INSERT INTO #SecMasterData
SELECT 
	Symbol_PK
	,TickerSymbol
	,PutOrCall
	,StrikePrice
	,ExpirationDate
	,Multiplier
	,ISINSymbol
	,CUSIPSymbol
	,SEDOLSymbol
	,BloombergSymbol
	,CompanyName
	,UnderlyingSymbol
	,OSISymbol
	,Coupon
FROM V_SecMasterData SM

CREATE TABLE #VT 
	(
	GroupID Varchar(50)
	,TaxlotID Varchar(50)
	,Level1AllocationID Varchar(50)
	,GroupRefID Int
	,Symbol VARCHAR(100)
	,TradeDate DATETIME
	,TradeDateTime DATETIME
	,ProcessDate DATETIME
	,SettlementDate DATETIME
	,AccountName Varchar(200)
	,TaxLotQty FLOAT
	,Side Varchar(50)
	,AvgPrice FLOAT
	,CumQty FLOAT
	,Quantity FLOAT
	,FXRate FLOAT
	,FXConversionMethodOperator VARCHAR(3)
	,FXRate_Taxlot FLOAT
	,FXConversionMethodOperator_Taxlot VARCHAR(3)
	,EntityID Varchar(50) 
	,Asset Varchar(100)
	,CounterParty Varchar(100)
	,TaxLotState Varchar(20)
	,TaxlotStateID Int 
	,CurrencySymbol Varchar(10)
	,SettlCurrency Varchar(10)
	,Commission FLOAT
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
	,SoftCommission FLOAT
	,TransactionType vARCHAR(200)
	,Strategy Varchar(200)
	,MappedName Varchar(200)
	,FundAccntNo Varchar(200)	 
	,PutOrCall VARCHAR(10)
	,StrikePrice Float
	,ExpirationDate DATETIME
	,Multiplier FLOAT
	,ISINSymbol VARCHAR(20)
	,CUSIPSymbol VARCHAR(50)
	,SEDOLSymbol VARCHAR(50)
	,BloombergSymbol VARCHAR(200)
	,CompanyName VARCHAR(500)
	,UnderlyingSymbol VARCHAR(100)
	,OSISymbol VARCHAR(25)
	,COUPON FLOAT
	,SideMultiplier Int
	,TotalCommissionAndFee Float
	,GrossAmount Float
	,NetAmount Float
	,FileFormatID Int
	,Symbol_PK BigInt
	,CounterPartyId Int
	,AccruedInterest FLOAT
	,TradeAttribute3 VARCHAR(200)
	)

INSERT INTO #VT
SELECT 
	VT.GroupID
	,VT.TaxLotID
	,VT.Level1AllocationID AS Level1AllocationID
	,VT.GroupRefID
	,VT.Symbol
	,VT.AUECLocalDate As TradeDate
	,VT.AUECLocalDate As TradeDateTime
	,VT.ProcessDate
	,VT.SettlementDate
	,CF.FundName As AccountName	
	,VT.TaxLotQty
	,S.Side
	,VT.AvgPrice
	,VT.CumQty
	,VT.Quantity
	,VT.FXRate
	,VT.FXConversionMethodOperator
	,VT.FXRate_Taxlot
	,VT.FXConversionMethodOperator_Taxlot
	,VT.Level1AllocationID As EntityID
	,A.AssetName As Asset	
	,CP.ShortName As CounterParty
	,'Allocated' AS TaxLotState
	,0 As TaxlotStateID
	,Currency.CurrencySymbol
	,COALESCE(SettleCurr.CurrencySymbol, 'None') AS SettlCurrency
	,VT.Commission
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
	,VT.SoftCommission
	,VT.TransactionType
	,CS.StrategyName As Strategy
	,IsNull(CTPM.MappedName,'') As MappedName
	,IsNull(CTPM.FundAccntNo,'') As FundAccntNo	
	,SM.PutOrCall
	,SM.StrikePrice
	,SM.ExpirationDate	
	,SM.Multiplier As AssetMultiplier
	,SM.ISINSymbol As ISIN
	,SM.CUSIPSymbol As CUSIP
	,SM.SEDOLSymbol As SEDOL
	,SM.BloombergSymbol
	,SM.CompanyName 
	,SM.UnderlyingSymbol
	,SM.OSISymbol	
	,SM.COUPON
	,VT.SideMultiplier
	,(VT.Commission + VT.OtherBrokerFees  + VT.StampDuty + VT.TransactionLevy + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees
	+ VT.SecFee + VT.OccFee + VT.OrfFee + VT.ClearingBrokerFee + VT.SoftCommission) As TotalCommissionAndFee
	,(VT.AvgPrice * VT.TaxlotQty * SM.Multiplier) As GrossAmount
	,(VT.AvgPrice * VT.TaxlotQty * SM.Multiplier) + 
	((VT.Commission + VT.OtherBrokerFees  + VT.StampDuty + VT.TransactionLevy + VT.ClearingFee + VT.TaxOnCommissions + VT.MiscFees
	+ VT.SecFee + VT.OccFee + VT.OrfFee + VT.ClearingBrokerFee + VT.SoftCommission) * VT.SideMultiplier) As NetAmount
	,@fileFormatID AS FileFormatID
	,SM.Symbol_PK
	,VT.CounterPartyId
	,VT.AccruedInterest
	,VT.TradeAttribute3

FROM V_TaxLots VT with (nolock)
INNER JOIN @Fund Fund ON Fund.FundID = VT.FundID
Inner Join T_CompanyFunds CF On CF.CompanyFundID = VT.FundID
Inner JOIN #SecMasterData AS SM ON SM.TickerSymbol = VT.Symbol
INNER JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
INNER JOIN T_Currency AS SettleCurr ON SettleCurr.CurrencyID = VT.SettlCurrency_Taxlot
INNER JOIN T_Side S ON S.SideTagValue = VT.OrderSideTagValue
INNER JOIN T_Asset A ON VT.AssetID = A.AssetID
INNER JOIN T_CounterParty CP ON CP.CounterPartyID = VT.CounterPartyID
Inner Join T_CompanyStrategy CS On CS.CompanyStrategyID = VT.Level2ID  
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails AS CTPM ON CTPM.InternalFundNameID_FK = VT.FundID
LEFT JOIN dbo.T_CompanyThirdPartyFlatFileSaveDetails CTPFD ON CTPFD.CompanyThirdPartyID = CTPM.CompanyThirdPartyID_FK
WHERE DATEDIFF(Day, @StartDate, VT.ProcessDate) >= 0
		     AND DATEDIFF(DAY, VT.ProcessDate, @Inputdate) >= 0
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

Select 
CONVERT(VARCHAR(10), TradeDate, 101) AS TradeDate,
CONVERT(VARCHAR(10), TradeDateTime, 101) + ' ' + CONVERT(VARCHAR(8), TradeDateTime, 108) AS TradeDateTime,
CONVERT(VARCHAR(10), SettlementDate, 101) AS SettlementDate,
AccountName,
TaxLotQty AS AllocatedQty,
Side,
AvgPrice AS AveragePrice,
FXRate,
FXConversionMethodOperator,
FXRate_Taxlot,
FXConversionMethodOperator_Taxlot,
EntityID,
Asset,
CounterParty,
TaxLotState,
CurrencySymbol,
SettlCurrency,
Commission AS CommissionCharged,
OtherBrokerFees,
StampDuty,
TransactionLevy,
ClearingFee,
TaxOnCommissions,
MiscFees,
SecFee,
OccFee,
OrfFee,
ClearingBrokerFee,
SoftCommission AS SoftCommissionCharged,
TransactionType,
ISINSymbol ,
CUSIPSymbol AS CUSIP,
SEDOLSymbol ,
BloombergSymbol,
CompanyName AS FullSecurityName,
TotalCommissionAndFee,
GrossAmount,
NetAmount,
TradeAttribute3,
Symbol
from #VT
Order By GroupRefID

DROP TABLE #VT,#SecMasterData