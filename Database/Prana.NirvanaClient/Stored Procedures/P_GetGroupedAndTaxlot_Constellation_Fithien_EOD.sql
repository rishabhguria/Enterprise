
 CREATE PROCEDURE [dbo].[P_GetGroupedAndTaxlot_Constellation_Fithien_EOD]
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

--	SET @thirdPartyID = 78
--	SET @companyFundIDs = N'51,80,56,57,42,84,71,67,94,107,101,99,92,106,97,96,93,105,100,98,103,95,104,102,76,36,63,48,35,47,58,60,88,62,52,85,87,83,41,40,65,39,59,78,53,55,38,50,79,69,81,45,70,43,33,68,74,64,37,46,34,66,89,77,44,75,82,49,72,54,86,90,73,61'
--	SET @inputDate = '2023-04-28 04:11:08'
--	SET @companyID = 7
--	SET @auecIDs = N'44,34,54,180,1,15,230,62'
--	SET @TypeID = 0
--	SET @dateType =  0                                                                                                                                                           
--	SET @fileFormatID = 121
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
	,SEDOL VARCHAR(20)
	)

INSERT INTO #SecMasterData
SELECT 
	SM.TickerSymbol
	,SM.ExpirationDate
	,SM.Multiplier
	,SM.CompanyName
	,SM.SEDOLSymbol
FROM V_SecMasterData SM

CREATE TABLE #Temp_Final
(
    AccountName VARCHAR(500)
	,Symbol VARCHAR(100)
	,SEDOL VARCHAR(100)
	,Side VARCHAR(50)
	,AvgPrice FLOAT
	,NotionalValue FLOAT
	,TaxLotQty FLOAT
	,TradeDate Date
	,TotalQty Float
	,RecordCount Int
	,CustomOrderBy Int
)

CREATE TABLE #VT 
	(
    AccountName VARCHAR(100)
	,Symbol VARCHAR(100)
	,SEDOL VARCHAR(100)
	,Side VARCHAR(50)
	,AvgPrice FLOAT
	,NotionalValue FLOAT
	,TaxLotQty FLOAT
	,TradeDate Date
	,CustomOrderBy Int
	)

INSERT INTO #VT
SELECT
    CF.FundName AS AccountName
	,VT.Symbol
	,SM.SEDOL 
	,S.Side
	,VT.AvgPrice As AvgPrice
	,0.0 As NotionalValue
	,VT.TaxLotQty As TaxLotQty
	,Cast(VT.AUECLocalDate As Date) As TradeDate
	,2 As CustomOrderBy
	
FROM V_TaxLots VT
Inner Join @Fund F On F.FundID = VT.FundID
Inner Join @AUECID AUEC On AUEC.AUECID = VT.AUECID
INNER JOIN #SecMasterData SM ON VT.Symbol = SM.TickerSymbol
INNER JOIN T_CompanyFunds CF ON CF.CompanyFundID = VT.FundID
Inner JOIN T_Currency AS Currency ON Currency.CurrencyID = VT.CurrencyID
Inner JOIN T_Side S ON S.SideTagValue = VT.OrderSideTagValue

Where VT.AssetID = 1
And 
Datediff(DAY, VT.AUECLocalDate, @inputdate) = 0
	AND (
		(
			VT.TransactionType IN ('Buy','BuytoClose','BuytoOpen','Sell','Sellshort','SelltoClose','SelltoOpen','LongAddition','LongWithdrawal','ShortAddition','ShortWithdrawal','')
			AND (VT.TransactionSource IN (0,1,2,3,4,14))
			)
		OR (
			@IncludeExpiredSettledTransaction = 1
			AND VT.TransactionType IN ('Exercise','Expire','Assignment')
			AND VT.AssetID IN (	2,4)
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
			AND VT.AssetID IN (1,3)
			)
		OR (
			@IncludeCATransaction = 1
			AND VT.TransactionType IN ('LongAddition','LongWithdrawal','ShortAddition','ShortWithdrawal','LongCostAdj','ShortCostAdj','LongWithdrawalCashInLieu','ShortWithdrawalCashInLieu')
			AND (VT.TransactionSource IN (6,7,8,9,11))
			)
		OR TransactionSource = 13
		)

---- do grouping by AccountName,TradeDate,Side ,Symbol,SEDOL
SELECT
    AccountName As AccountName
	,Symbol
	,SEDOL
	,Side
	,0 As AvgPrice
	,Sum(AvgPrice * TaxLotQty) As NotionalValue
	,Sum(TaxLotQty) As TaxLotQty
	,TradeDate
	,2 As CustomOrderBy
InTo #VT_TaxlotGrouped
FROM #VT
GROUP BY AccountName,TradeDate,Side ,Symbol,SEDOL

--Select *
--From #VT_TaxlotGrouped

---- Calculate Header row Data
SELECT
    '' As AccountName
	,Symbol
	,SEDOL
	,Side
	,Cast(0 As Decimal(18,4)) As AvgPrice
	,Sum(NotionalValue) As NotionalValue
	,Sum(TaxLotQty) As TaxLotQty
	,TradeDate
	,1 As CustomOrderBy
InTo #Temp_HeaderGrouped
FROM #VT_TaxlotGrouped
GROUP BY TradeDate,Side ,Symbol,SEDOL

--Select * From #Temp_HeaderGrouped

---- Calculate Wted Average Price
Update #Temp_HeaderGrouped
Set AvgPrice = 
Case 
When TaxLotQty > 0
Then NotionalValue/TaxLotQty
Else 0
End

--Select * From #Temp_HeaderGrouped

---- Calculate Trailer/Footer Data
Select 
Symbol , 
Count(*) As RecordCount,
Sum(TaxLotQty) As TotalQty,
Side,
3 As CustomOrderBy
InTo #Temp_Trailer 
From #VT_TaxlotGrouped
Group By Symbol,Side

--Select * 
--From #Temp_Trailer


Insert InTo #Temp_Final
Select
'HeaderRecord' As AccountName,
Symbol,
SEDOL,
Side,
AvgPrice,
NotionalValue,
TaxlotQty,
TradeDate,
0.0 As TotalQty,
0 As RecordCount,
CustomOrderBy 
From #Temp_HeaderGrouped

Insert InTo #Temp_Final
Select
AccountName,
Symbol,
SEDOL,
Side,
AvgPrice,
NotionalValue,
TaxlotQty,
TradeDate,
0.0 As TotalQty,
0 As RecordCount,
CustomOrderBy 
From #VT_TaxlotGrouped

Insert InTo #Temp_Final
Select
'TrailerRecord' As AccountName,
Symbol,
'' As SEDOL,
Side,
0.0 As AvgPrice,
0.0 As NotionalValue,
0.0 As TaxlotQty,
'' As TradeDate,
TotalQty,
RecordCount,
CustomOrderBy 
From #Temp_Trailer

Alter Table #Temp_Final
Add FundAccntNo Varchar(200),
MappedName Varchar(200)

Update #Temp_Final
Set FundAccntNo = 'Undefined', MappedName = 'Undefined'

Update #Temp_Final
Set FundAccntNo = IsNull(CTPM.FundAccntNo, 'Undefined'), 
MappedName = IsNull(CTPM.MappedName, 'Undefined')
From #Temp_Final Temp
Inner Join T_CompanyFunds CF On CF.FundName = Temp.AccountName
LEFT OUTER JOIN T_CompanyThirdPartyMappingDetails CTPM ON CTPM.InternalFundNameID_FK = CF.CompanyFundID 
Where CustomOrderBy = 2

Select 
AccountName,
Symbol,
SEDOL,
Side,
AvgPrice,
NotionalValue,
TaxLotQty,
Convert(Varchar,TradeDate,112) As TradeDate,
TotalQty,
RecordCount,
CustomOrderBy,
Convert(Varchar,@inputDate,112) As TransmissionDate,
FundAccntNo,
MappedName

From #Temp_Final Temp
Order By Symbol,Side, CustomOrderBy, AccountName


DROP TABLE #VT,#SecMasterData,#Temp_HeaderGrouped, #Temp_Final,#Temp_Trailer,#VT_TaxlotGrouped