
 CREATE PROCEDURE P_GetAllocation_SummitStreet_AllData_EOD
 (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties 
	,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                            
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
--	SET @companyFundIDs = N'1,2,3,4,5'
--	SET @inputDate = '2024-01-04'
--	SET @companyID = 7
--	SET @auecIDs = N'44,34,54,180,1,15,230,62'
--	SET @TypeID = 0
--	SET @dateType =  0                                                                                                                                                           
--	SET @fileFormatID = 121
--	--SET @includeSent = 

DECLARE @Fund TABLE (FundID INT)
DECLARE @AUECID TABLE (AUECID INT)


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
	TradeDate Varchar(50)    
	,Symbol VARCHAR(100)
	,Side VARCHAR(50)
	,AccountName VARCHAR(100)	
	,OrderQty Float
	,Allocated_OrderQty Float
	,Level1Percentage Float
	,FilledQty Float
	,AvgPrice FLOAT
	,TaxLotQty FLOAT
	,Residual Float
	,GroupId Varchar(100)
	,OrderOrTaxlot Varchar(50)
	,CustomOrderBy Int
)



CREATE TABLE #VT 
	(
	TradeDate Varchar(50)    
	,Symbol VARCHAR(100)
	,Side VARCHAR(50)
	,AccountName VARCHAR(100)	
	,OrderQty Float
	,Allocated_OrderQty Float
	,Level1Percentage Float
	,FilledQty Float
	,AvgPrice FLOAT
	,TaxLotQty FLOAT
	,Residual Float
	,GroupId Varchar(100)
	,OrderOrTaxlot Varchar(50)
	,CustomOrderBy Int
	)

INSERT INTO #VT
SELECT
	Convert(Varchar,G.AUECLocalDate,101) As TradeDate
	,G.Symbol
	,S.Side
    ,CF.FundName AS AccountName
	,G.Quantity As OrderQty
	,(G.Quantity * 	L1.Percentage) / 100 As Allocated_OrderQty
	,L1.Percentage As Level1Percentage
	,G.CumQty As FilledQty
	,G.AvgPrice
	,L2.TaxLotQty
	,0 As Residual
	,G.GroupId
	,'Taxlot' As OrderOrTaxlot
	,1 As CustomOrderBy
	
FROM T_Level2Allocation L2 
INNER JOIN T_FundAllocation L1 ON L1.AllocationId = L2.Level1AllocationID
INNER JOIN T_Group AS G ON G.GroupID = L1.GroupID
Inner Join @Fund F On F.FundID = L1.FundID
Inner Join @AUECID AUEC On AUEC.AUECID = G.AUECID
INNER JOIN #SecMasterData SM ON G.Symbol = SM.TickerSymbol
INNER JOIN T_CompanyFunds CF ON CF.CompanyFundID = L1.FundID
Inner JOIN T_Currency AS Currency ON Currency.CurrencyID = G.CurrencyID
Inner JOIN T_Side S ON S.SideTagValue = G.OrderSideTagValue

Where L2.TaxlotQty > 0
And Datediff(DAY, G.AUECLocalDate, @inputdate) = 0
--And G.Symbol = 'WBD'


--Select *
--From #VT
--Order By AccountName, Symbol
------ do grouping by GroupId,TradeDate,Side ,Symbol,SEDOL
SELECT
	Max(TradeDate) As TradeDate
	,Max(Symbol) As Symbol
	,Max(Side) As Side
    ,'Total' As AccountName	
	,Sum(Allocated_OrderQty) As OrderQty
	,Sum(Allocated_OrderQty) As Allocated_OrderQty
	,Cast(0 As Float) As Level1Percentage
	,Max(FilledQty) As FilledQty
	,Max(AvgPrice) As AvgPrice
	,Sum(TaxLotQty) As TaxLotQty
	,0 As Residual
	,GroupId,
	'Order' As OrderOrTaxlot
	,2 As CustomOrderBy
InTo #VT_TaxlotGrouped
FROM #VT
GROUP BY GroupId

--Select *
--From #VT_TaxlotGrouped

Insert InTo #Temp_Final
Select 
TradeDate    
,Symbol
,Side
,AccountName	
,OrderQty
,Allocated_OrderQty
,Level1Percentage
,FilledQty
,AvgPrice
,TaxLotQty
,Residual
,GroupId 
,OrderOrTaxlot
,CustomOrderBy
From #VT

Insert InTo #Temp_Final
Select 
TradeDate    
,Symbol
,Side
,AccountName	
,OrderQty
,Allocated_OrderQty
,Level1Percentage
,FilledQty
,AvgPrice
,TaxLotQty
,Residual
,GroupId 
,OrderOrTaxlot
,CustomOrderBy
From #VT_TaxlotGrouped


Select 
TradeDate    
,Symbol
,Side
,AccountName	
,OrderQty
,Allocated_OrderQty
,Level1Percentage
,FilledQty
,AvgPrice
,TaxLotQty
,(Allocated_OrderQty - TaxLotQty) As Residual
,GroupId 
,OrderOrTaxlot
,CustomOrderBy
From #Temp_Final
Order By GroupId, CustomOrderBy, AccountName,Symbol 

DROP TABLE 
	#VT,#SecMasterData
	,#Temp_Final
	,#VT_TaxlotGrouped