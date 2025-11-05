CREATE Procedure [dbo].[P_GetWashSaleOpenTaxlots]                            
(                                                
@fundIDs VARCHAR(max),
@assetIDs VARCHAR(max),
@currencyIDs VARCHAR(max)                       
)
AS

--Declare @fundIDs VARCHAR(max),
--@assetIDs VARCHAR(1000),
--@currencyIDs VARCHAR(max) 

--Set @fundIDs='3'
--Set @assetIDs='1,2,15,8,9'
--Set @currencyIDs='5,44,14,55,17,47,6,7,23,24,15,25,26,27,19,48,8,4,20,49,28,2,29,30,31,21,32,3,16,33,34,11,35,51,9,36,37,56,38,40,41,13,10,52,42,18,43,46,1,45,53,54,12'   

-------------------------------------------------
--Gets all the fundID and store in temp table--
CREATE TABLE #Funds (FundID INT)

INSERT INTO #Funds
SELECT Items
FROM dbo.Split(@fundIDs, ',')
--------------------------------------------------
--Gets all the asset and store in temp table--
CREATE TABLE #Temp_Asset (assetID INT)

INSERT INTO #Temp_Asset
SELECT Items
FROM dbo.Split(@assetIDs, ',')
--------------------------------------------------
--Gets all the currency and store in temp table--
CREATE TABLE #Temp_Currency (currID INT)

INSERT INTO #Temp_Currency
SELECT Items
FROM dbo.Split(@currencyIDs, ',')

-----------------------------------------------------------------------------------------
--Table to fetch account and its Wash Sale Start Date---
CREATE TABLE #TEMPWashSalePreference
(
FundID               INT          DEFAULT ((0)) NOT NULL,
WashSaleStartDate    DATETIME     NOT NULL,
)
INSERT INTO #TEMPWashSalePreference
EXEC P_GetAllWashSalePreferences  


--Select * From #TEMPWashSalePreference

Alter Table #TEMPWashSalePreference
Add TransactionStartDate Date,
TransactionEndDate Date


Update #TEMPWashSalePreference
Set 
TransactionStartDate = DateAdd(Day, 1, Cast(WashSaleStartDate As Date)),
TransactionEndDate = DateAdd(Day, 30, Cast(WashSaleStartDate As Date))

--Select * From #TEMPWashSalePreference

					 
--------------------------------------------------
----Temp table to store all the open taxlots------
CREATE TABLE #TempWashSaleOpenTaxlots                                                                            
(
TaxlotID VARCHAR (50) NOT NULL,                                     
Symbol VARCHAR(200),
Quantity FLOAT,
FundID FLOAT,                              
FundName VARCHAR(50),                                               
CurrencyID INT,                  
AssetID INT,
OrderSideID VARCHAR(200),
OriginalPurchaseDate DATETIME,
CounterPartyID INT,
TradeDate DATETIME ,
BloombergSymbol VARCHAR(100),
UnderlyingSymbol VARCHAR(100),
Issuer VARCHAR(200),
CUSIP VARCHAR(200),
UnitCostLocal FLOAT,
TotalCostLocal FlOAT,
TotalCost FlOAT  ,
TypeOfTransaction Varchar(20),
IsSwapped Bit                  
)   


SELECT MAX(Taxlot_PK) As Taxlot_PK 
		InTo #TempTaxlotPK
					FROM PM_Taxlots PT
					inner Join #Funds F On F.FundID = PT.FundID
					Inner Join #TEMPWashSalePreference WS On WS.FundID = PT.FundID 
					WHERE DATEDIFF(D, PT.AUECModifiedDate, WS.WashSaleStartDate) >= 0
					GROUP BY TaxlotID                              
      
---- Get Open Positions                         
INSERT INTO #TempWashSaleOpenTaxlots                              
 SELECT 
  PT.TaxLotID AS TaxlotID,                              
  PT.Symbol AS Symbol, 
  PT.TaxlotOpenQty AS Quantity,
  PT.FundID AS FundID,                              
  CF.FundName AS FundName,                                    
  G.CurrencyID As CurrencyID,
  G.AssetID AS AssetID,
  G.OrderSideTagValue AS OrderSideID,
  G.OriginalPurchaseDate AS OriginalPurchaseDate,
  G.CounterPartyID  AS CounterPartyID,
  G.AUECLocalDate AS TradeDate,
  SM.BloombergSymbol AS BloombergSymbol, 
  SM.UnderlyingSymbol AS UnderlyingSymbol,
  (IsNull(UDA.Issuer,'Undefined')) As Issuer,
  SM.CUSIPSymbol AS CUSIP,
  (((PT.TaxLotOpenQty * PT.AvgPrice * IsNUll(SM.Multiplier,0)) + (PT.OpenTotalCommissionandFees * [dbo].[GetSideMultiplier](PT.OrderSideTagValue))) / (PT.TaxLotOpenQty * SM.Multiplier)) As UnitCostLocal,
   ((PT.TaxLotOpenQty * PT.AvgPrice * IsNUll(SM.Multiplier,0)) + (PT.OpenTotalCommissionandFees * [dbo].[GetSideMultiplier](PT.OrderSideTagValue))) As TotalCostLocal,
	CASE
		WHEN G.CurrencyID <> CF.LocalCurrency        
		THEN 
			CASE 
			WHEN IsNUll(PT.FXRate,0) > 0
			THEN 
				CASE
					WHEN (PT.FXConversionMethodOperator ='M')
					THEN ((PT.TaxLotOpenQty*PT.AvgPrice * IsNUll(SM.Multiplier,0)) + (PT.OpenTotalCommissionandFees * [dbo].[GetSideMultiplier](PT.OrderSideTagValue))) * PT.FXRate
					WHEN (PT.FXConversionMethodOperator ='D') AND PT.FXRate > 0
					THEN ((PT.TaxLotOpenQty*PT.AvgPrice*IsNUll(SM.Multiplier,0)) + (PT.OpenTotalCommissionandFees * [dbo].[GetSideMultiplier](PT.OrderSideTagValue))) / PT.FXRate
				END
			ELSE 0
		END
		ELSE ((PT.TaxLotOpenQty * PT.AvgPrice * IsNUll(SM.Multiplier,0)) + (PT.OpenTotalCommissionandFees * [dbo].[GetSideMultiplier](PT.OrderSideTagValue)))
	END as TotalCost,
   'Taxlot' As TypeOfTransaction,
   G.IsSwapped 
  FROM PM_Taxlots PT  
  Inner Join #TempTaxlotPK TPK On TPK.Taxlot_PK = PT.Taxlot_PK                  
  Inner Join T_Group G ON G.GroupID = PT.GroupID
  Inner Join #Temp_Asset ASS ON ASS.assetID = G.AssetID
  Inner Join #Temp_Currency CURR ON CURR.currID = G.CurrencyID
  Inner Join T_CompanyFunds CF on CF.CompanyFundID = PT.FundID  
  Inner Join #TEMPWashSalePreference WP ON WP.FundID = PT.FundID
  Inner Join V_SecMASterData SM  ON PT.Symbol = SM.TickerSymbol 
  Left Outer Join V_UDA_DynamicUDA UDA ON UDA.Symbol_PK = SM.Symbol_PK
  WHERE TaxLotOpenQty > 0

---- Get Transactions                      
INSERT INTO #TempWashSaleOpenTaxlots                              
 SELECT 
  VT.TaxLotID AS TaxlotID,                              
  VT.Symbol AS Symbol, 
  VT.TaxLotQty AS Quantity,
  VT.FundID AS FundID,                              
  CF.FundName AS FundName,                                    
  VT.CurrencyID As CurrencyID,
  VT.AssetID As AssetID,
  VT.OrderSideTagValue AS OrderSideID,
  VT.OriginalPurchaseDate AS OriginalPurchaseDate,
  VT.CounterPartyID  AS CounterPartyID,
  VT.AUECLocalDate AS TradeDate,
  SM.BloombergSymbol AS BloombergSymbol, 
  SM.UnderlyingSymbol AS UnderlyingSymbol,
  (IsNull(UDA.Issuer,'Undefined')) As Issuer,
  SM.CUSIPSymbol AS CUSIP,
  (((VT.TaxLotQty * VT.AvgPrice * IsNUll(SM.Multiplier,0)) + ((VT.TotalExpenses - VT.OptionPremiumAdjustment) * [dbo].[GetSideMultiplier](VT.OrderSideTagValue)))/ (VT.TaxLotQty*SM.Multiplier)) As UnitCostLocal,

  ((VT.TaxLotQty * VT.AvgPrice * IsNUll(SM.Multiplier,0)) + (VT.TotalExpenses - VT.OptionPremiumAdjustment) * [dbo].[GetSideMultiplier](VT.OrderSideTagValue)) As TotalCostLocal,

   CASE
		WHEN VT.CurrencyID <> CF.LocalCurrency        
		THEN 
		CASE 
			WHEN IsNUll(VT.FXRate,0) <> 0
				THEN 
				CASE
					WHEN (VT.FXConversionMethodOperator ='M')
						THEN ((VT.TaxLotQty*VT.AvgPrice*IsNUll(SM.Multiplier,0)) +  (VT.TotalExpenses - VT.OptionPremiumAdjustment) * [dbo].[GetSideMultiplier](VT.OrderSideTagValue)) * VT.FXRate
					WHEN (VT.FXConversionMethodOperator ='D') AND VT.FXRate > 0
						THEN ((VT.TaxLotQty*VT.AvgPrice*IsNUll(SM.Multiplier,0)) +  (VT.TotalExpenses - VT.OptionPremiumAdjustment) * [dbo].[GetSideMultiplier](VT.OrderSideTagValue)) / VT.FXRate
				END
			ELSE 0
		END
	ELSE ((VT.TaxLotQty * VT.AvgPrice * IsNUll(SM.Multiplier,0)) +  (VT.TotalExpenses - VT.OptionPremiumAdjustment) * [dbo].[GetSideMultiplier](VT.OrderSideTagValue))
	END As TotalCost,
    'Trade' As TypeOfTransaction,
	VT.IsSwapped 
  FROM V_TaxLots VT  
  Inner Join #funds F ON F.fundID = VT.FundID
  Inner Join #Temp_Currency CURR ON CURR.currID = VT.CurrencyID
  Inner Join #Temp_Asset ASS ON ASS.assetID = VT.AssetID
  Inner Join T_CompanyFunds CF on CF.CompanyFundID = VT.FundID  
  Inner Join #TEMPWashSalePreference WP ON WP.FundID = VT.FundID
  Inner Join V_SecMASterData SM ON VT.Symbol = SM.TickerSymbol 
  Left Outer Join V_UDA_DynamicUDA UDA ON UDA.Symbol_PK = SM.Symbol_PK
 Where DateDiff(Day,WP.TransactionStartDate, VT.AUECLocalDate) >= 0
	And DateDiff(Day, VT.AUECLocalDate,WP.TransactionEndDate) >= 0
	And VT.OrderSideTagValue In ('1','5','A','C')

------------------------------------------------------------------------------------------
--- Table to store final values and columns that are to be shown on Wash Sale UI grid------
CREATE TABLE #WashSaleTaxlots                                                                            
(                                      
TaxlotID VARCHAR(100) NOT NULL,
TypeOfTransaction VARCHAR(20) ,
TradeDate DATETIME ,
OriginalPurchaseDate DATETIME ,
Account VARCHAR(500) ,
Side VARCHAR(20) ,
Asset VARCHAR(50) ,
Currency VARCHAR(20) ,
[Broker] VARCHAR(200) ,
Symbol VARCHAR(200) ,
BloombergSymbol VARCHAR(200) ,
CUSIP VARCHAR(50) ,
Issuer VARCHAR(200) ,
UnderlyingSymbol VARCHAR(200) ,
Quantity FLOAT ,
UnitCostLocal FLOAT ,
TotalCostLocal FLOAT ,
TotalCost FLOAT ,
WashSaleAdjustedRealizedLoss VARCHAR(200) ,
WashSaleAdjustedHoldingsPeriod INT ,
WashSaleAdjustedCostBasis VARCHAR(200) ,
WashSaleAdjustedHoldingsStartDate DATETIME       
)

INSERT INTO #WashSaleTaxlots
(
TaxlotID,
TypeOfTransaction,
TradeDate  ,
OriginalPurchaseDate  ,
Account,
Side ,
Asset ,
Currency  ,
[Broker]  ,
Symbol  ,
BloombergSymbol  ,
CUSIP  ,
Issuer  ,
UnderlyingSymbol  ,
Quantity  ,
UnitCostLocal  ,
TotalCostLocal  ,
TotalCost  ,
WashSaleAdjustedRealizedLoss  ,
WashSaleAdjustedHoldingsPeriod  ,
WashSaleAdjustedCostBasis  ,
WashSaleAdjustedHoldingsStartDate
)
SELECT
TWS.TaxlotID, 
TWS.TypeOfTransaction As TypeOfTransaction,                        
TWS.TradeDate,
TWS.OriginalPurchaseDate,
TWS.FundName,
S.Side,
CASE       
	WHEN (TWS.AssetID = 1 And TWS.IsSwapped = 1)                  
	THEN 'EquitySwap'                
ELSE Asset.AssetName               
END AS  Asset,
TC.CurrencySymbol,
CP.ShortName,
TWS.Symbol,
TWS.BloombergSymbol,
TWS.CUSIP,
TWS.Issuer,
TWS.UnderlyingSymbol,
TWS.Quantity,
TWS.UnitCostLocal,
TWS.TotalCostLocal,  --TotalCostLocal
TWS.TotalCost,  --TotalCost
WS.WashSaleAdjustedRealizedLoss,
WS.WashSaleAdjustedHoldingsPeriod,
WS.WashSaleAdjustedCostBasis,
WS.WashSaleAdjustedHoldingsStartDate
FROM 
#TempWashSaleOpenTaxlots TWS
Inner Join T_Side S ON S.SideTagValue= TWS.OrderSideID     
Inner Join T_Asset Asset ON Asset.AssetId= TWS.AssetID 
Inner Join T_Currency TC ON TC.CurrencyID = TWS.CurrencyID  
Inner Join T_CounterParty CP ON CP.CounterPartyID = TWS.CounterPartyID 
Left Outer Join T_WashSaleOnBoarding WS ON WS.TaxlotID = TWS.TaxlotID
Inner Join #TEMPWashSalePreference WP ON WP.FundID = TWS.fundID
---------------------------------------------------------------------------
--Return table to Wash Sale UI Grid---

SELECT *,CAST(TaxlotID as FLOAT(53)) as TAXLOT FROM #WashSaleTaxlots
ORDER BY TAXLOT
--------------------------------------------------------------------------
---Drop all the temp tables-----------
Drop table #Funds
Drop table #Temp_Asset
Drop table #Temp_Currency
Drop table #TempWashSaleOpenTaxlots
Drop table #WashSaleTaxlots
Drop table #TEMPWashSalePreference
Drop Table #TempTaxlotPK
--Drop Table #Temp_TaxLotClosingId, #Temp_PositionalTaxlotID