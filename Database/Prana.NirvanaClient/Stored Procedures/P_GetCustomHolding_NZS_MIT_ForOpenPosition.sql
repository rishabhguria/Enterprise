
/*

EXEC [P_GetCustomHolding_NZS_Jupiter_ForOpenPosition] 1, '1,5', '03-09-2023',1,1,1,1,1

*/

CREATE Procedure [dbo].[P_GetCustomHolding_NZS_MIT_ForOpenPosition]                      
(                                 
@ThirdPartyID int,                                            
@CompanyFundIDs varchar(max),                                                                                                                                                                          
@InputDate datetime,                                                                                                                                                                      
@CompanyID int,                                                                                                                                      
@AUECIDs varchar(max),                                                                            
@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
@FileFormatID int                                  
)                                  
AS

--Declare @ThirdPartyID int,                                            
--@CompanyFundIDs varchar(max),                                                                                                                                                                          
--@InputDate datetime,                                                                                                                                                                      
--@CompanyID int,                                                                                                                                      
--@AUECIDs varchar(max),                                                                            
--@TypeID int,                                     
--@DateType int,
--@FileFormatID int  

--Set @ThirdPartyID = 1                                            
--Set @CompanyFundIDs = '1,5'                                                                                                                                                                   
--Set @InputDate =  '03-09-2023'		                                                                                                                                                                 
--Set @CompanyID = 1                                                                                                                                     
--set @AUECIDs = '1'                                                                            
--Set @TypeID = 0
--Set @DateType = 0                                                                                                                                                                         
--Set @FileFormatID = 0  

Declare @PreviousBusinessDate DateTime

Declare @Fund Table                                                                     
(                          
FundID int                                
)            
          
Insert into @Fund                                                                                                              
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')   

Create Table #FundDetails                                                           
(                
FundID int,
FundName Varchar(100),
LocalCurrency Int
                     
)  

Insert into #FundDetails 
Select 
CF.CompanyFundID,
CF.FundName,
CF.LocalCurrency

From T_CompanyFunds CF WITH(NOLOCK)
Inner Join @Fund F On F.FundID = CF.CompanyFundID
Inner Join T_CompanyMasterFundSubAccountAssociation MFA WITH(NOLOCK) On MFA.CompanyFundID = CF.CompanyFundID 


-- get Mark Price for End Date              
CREATE TABLE #MarkPriceForEndDate   
(    
  Finalmarkprice FLOAT    
 ,Symbol VARCHAR(200)    
 ,FundID INT    
 )   
  
INSERT INTO #MarkPriceForEndDate   
(    
 FinalMarkPrice    
 ,Symbol    
 ,FundID    
 )    
SELECT   
  DMP.FinalMarkPrice    
 ,DMP.Symbol    
 ,DMP.FundID    
FROM PM_DayMarkPrice DMP With (NoLock)
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0

-- For Fund Zero
SELECT *
INTO #ZeroFundMarkPriceEndDate
FROM #MarkPriceForEndDate
WHERE FundID = 0

Delete FROM #MarkPriceForEndDate WHERE FundID = 0

Create Table #TempTaxlotPK
( 
Taxlot_PK BigInt
)

Insert InTo #TempTaxlotPK
SELECT Distinct PT.Taxlot_PK 
FROM PM_Taxlots PT With (NoLock)          
Inner Join @Fund Fund on Fund.FundID = PT.FundID  
Where PT.Taxlot_PK in                                       
(                                                                                                
 Select Max(Taxlot_PK) from PM_Taxlots With (NoLock)                                                                                 
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                      
 Group by TaxlotId                                                                      
)                                                                                          
And PT.TaxLotOpenQty > 0 

Set @PreviousBusinessDate = (Select Min(G.AUECLocalDate) As MinTradeDate
FROM PM_Taxlots PT WITH(NOLOCK)
Inner Join #TempTaxlotPK T On T.Taxlot_PK = PT.Taxlot_PK
INNER JOIN T_Group G WITH(NOLOCK) ON PT.GroupID = G.GroupID)

Set @PreviousBusinessDate = dbo.AdjustBusinessDays(@PreviousBusinessDate,-1,1)

Declare @Fund_WithZeroFundId Table
(
FundId Int
)
Insert InTo @Fund_WithZeroFundId
Select * 
From @Fund

Insert InTo @Fund_WithZeroFundId
Select 0

Declare @FundIds Varchar(3000)

Select @FundIds = COALESCE(@FundIds +  ',','') + Cast(FundID As Varchar(5)) From @Fund_WithZeroFundId

-- get forex rates for 2 date ranges          
CREATE TABLE #FXConversionRates 
(
	FromCurrencyID INT
	,ToCurrencyID INT
	,RateValue FLOAT
	,ConversionMethod INT
	,DATE DATETIME
	,eSignalSymbol VARCHAR(max)
	,FundID INT
)

INSERT INTO #FXConversionRates
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange_MW @PreviousBusinessDate ,@InputDate, @FundIds


UPDATE #FXConversionRates
SET RateValue = 1.0 / RateValue
WHERE RateValue <> 0
	AND ConversionMethod = 1

-- For FundID = 0 (Zero)
SELECT *
INTO #ZeroFundFxRate
FROM #FXConversionRates
WHERE FundID = 0 

Delete FROM #FXConversionRates WHERE FundID = 0    
    
Select  

CF.FundName As Account,  
A.AssetName As AssetClass,
SM.TickerSymbol As Symbol,  
Case  dbo.GetSideMultiplier(PT.OrderSideTagValue)                    
	When 1                      
	Then 'LONG'                      
Else 'SHORT'                      
End as PositionSide,        
SM.CompanyName As SecurityName,  
PT.TaxlotOpenQty As OpenQty,
IsNull(MPEndDate.Val, 0) As MarkPrice,
IsNull((PT.TaxlotOpenQty * IsNull(MPEndDate.Val, 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) As MarketValue_Local,
Cast(0.00 As Float) As MarketValue_Base,
IsNull((PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees), 0) As CostBasis_Local, 
Cast(0.00 As Float) As CostBasis_Base,
SM.Multiplier As ContractSize,

SM.ISINSymbol As ISINSymbol,

SM.SEDOLSymbol As SEDOLSymbol,
SM.OSISymbol As OSISymbol,


CASE 
	WHEN G.CurrencyID = CF.LocalCurrency 
	THEN 1
Else 
	CASE 
		WHEN ISNULL(PT.FXRate, 0) > 0
			THEN CASE ISNULL(PT.FXConversionMethodOperator, 'M')
					WHEN 'M'
						THEN PT.FXRate
					WHEN 'D'
						THEN 1 / PT.FXRate
					END
		WHEN ISNULL(G.FXrate, 0) > 0
			THEN CASE ISNULL(G.FXConversionMethodOperator, 'M')
					WHEN 'M'
						THEN G.FXRate
					WHEN 'D'
						THEN 1 / G.FXRate
					END
		ELSE ISNULL(FXRatesForTradeDate.Val, 0)
		END 
End AS TradeFXRate,
Case 
	When SM.CurrencyID = CF.LocalCurrency 
	Then 1
Else IsNull(FXRatesForEndDate.Val,0) 
End As EndDateFXRate,
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,
TradeCurr.CurrencySymbol As TradeCurrency,
PortfolioCurr.CurrencySymbol As PortfolioCurrency,
TC.CurrencySymbol As SettlementCurrency,
SM.BloombergSymbol As BloombergSymbol,
SM.UnderLyingSymbol





Into #TempOpenPositionsTable         
From PM_Taxlots PT With (NoLock)
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
Inner Join #FundDetails CF With (NoLock) On CF.FundID = PT.FundID
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol
Inner Join T_Currency TradeCurr With (NoLock) On TradeCurr.CurrencyID = SM.CurrencyID  
Inner Join T_Currency PortfolioCurr With (NoLock) On PortfolioCurr.CurrencyID = CF.LocalCurrency
Inner Join T_Currency TC With (NoLock) On TC.CurrencyID = PT.SettlCurrency
Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID
INNER JOIN T_Group G With (NoLock) ON G.GroupID = PT.GroupID

LEFT OUTER JOIN #MarkPriceForEndDate MPE ON (
		MPE.Symbol = PT.Symbol AND MPE.FundID = PT.FundID )
LEFT OUTER JOIN #ZeroFundMarkPriceEndDate MPZeroEndDate ON (
		PT.Symbol = MPZeroEndDate.Symbol AND MPZeroEndDate.FundID = 0)
/* Forex Price for Trade Date */
LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON 
		(FXDayRatesForTradeDate.FromCurrencyID = G.CurrencyID
		AND FXDayRatesForTradeDate.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(d, G.ProcessDate, FXDayRatesForTradeDate.DATE) = 0
		AND FXDayRatesForTradeDate.FundID = PT.FundID)
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON 
		(ZeroFundFxRateTradeDate.FromCurrencyID = G.CurrencyID
		AND ZeroFundFxRateTradeDate.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(d, G.ProcessDate, ZeroFundFxRateTradeDate.DATE) = 0
		AND ZeroFundFxRateTradeDate.FundID = 0)
/* Forex Price for End Date */
LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON 
		(FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID
		AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(d, FXDayRatesForEndDate.DATE,@InputDate) = 0)
		AND FXDayRatesForEndDate.FundID = PT.FundID   
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON 
		(ZeroFundFxRateEndDate.FromCurrencyID = G.CurrencyID
		AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(d, @InputDate, ZeroFundFxRateEndDate.DATE) = 0
		AND ZeroFundFxRateEndDate.FundID = 0)
CROSS APPLY (
	SELECT CASE 
			WHEN MPE.FinalMarkPrice IS NULL
				THEN CASE 
						WHEN MPZeroEndDate.FinalMarkPrice IS NULL
						THEN 0
						ELSE MPZeroEndDate.FinalMarkPrice
					END
			ELSE MPE.Finalmarkprice
			END
	) AS MPEndDate(Val)
CROSS APPLY (
	SELECT CASE 
			WHEN FXDayRatesForTradeDate.RateValue IS NULL
				THEN CASE 
						WHEN ZeroFundFxRateTradeDate.RateValue IS NULL
							THEN 0
						ELSE ZeroFundFxRateTradeDate.RateValue
						END
			ELSE FXDayRatesForTradeDate.RateValue
			END
	) AS FXRatesForTradeDate(Val)
CROSS APPLY (
	SELECT CASE 
			WHEN FXDayRatesForEndDate.RateValue IS NULL
				THEN CASE 
						WHEN ZeroFundFxRateEndDate.RateValue IS NULL
							THEN 0
						ELSE ZeroFundFxRateEndDate.RateValue
						END
			ELSE FXDayRatesForEndDate.RateValue
			END
	) AS FXRatesForEndDate(Val)
Where PT.TaxlotOpenQty > 0

--- In base currency
Update #TempOpenPositionsTable
Set CostBasis_Base = CostBasis_Local * TradeFXRate,
MarketValue_Base = MarketValue_Local * EndDateFXRate

----Select * from #TempOpenPositionsTable

Select  

Temp.Account As Account, 
AssetClass As AssetClass,    
Temp.Symbol,       
Max(SecurityName) As SecurityName,
Sum(Temp.OpenQty * Temp.SideMultiplier) As OpenQty, 
Max(MarkPrice) As MarkPrice,
Sum(MarketValue_Local) AS MarketValue_Local,
Sum(MarketValue_Base) AS MarketValue_Base,
Sum(Temp.CostBasis_Local) As CostBasis_Local, 
Sum(Temp.CostBasis_Base) As CostBasis_Base, 
Max(Temp.ContractSize) As ContractSize,      
Max(ISINSymbol) As ISINSymbol,

Max(SEDOLSymbol) As SEDOLSymbol,
Max(OSISymbol) As OSISymbol,
Max(TradeFXRate) As TradeFXRate,
Max(EndDateFXRate) As EndDateFXRate,
Max(TradeCurrency) As TradeCurrency,
Max(PortfolioCurrency) As PortfolioCurrency,
Max(SettlementCurrency) As SettlementCurrency,
Max(BloombergSymbol) As BloombergSymbol,
Max(UnderlyingSymbol) As UnderlyingSymbol
Into #TempTable_Grouped              
From #TempOpenPositionsTable Temp  
Group By  Temp.Account,Temp.AssetClass,Temp.Symbol

Alter Table #TempTable_Grouped      
Add UnitCost Float Null

      
UPdate #TempTable_Grouped      
Set UnitCost = 0.0 
      
UPdate #TempTable_Grouped      
Set UnitCost =       
Case        
	When OpenQty <> 0 And ContractSize <> 0        
	Then (CostBasis_Local/OpenQty) /ContractSize        
	Else 0        
End


Select 
Convert(varchar, @InputDate,23) As HoldingDate,  
AssetClass,

Account,
Symbol,
SecurityName,
Round(OpenQty,0) As Quantity,
MarkPrice,
MarketValue_Local,
MarketValue_Base,
UnitCost,
CostBasis_Local As CostBasis_Local,
CostBasis_Base As CostBasis_Base,
ContractSize,
ISINSymbol,
SEDOLSymbol,
OSISymbol,
TradeCurrency,
PortfolioCurrency,
SettlementCurrency,
BloombergSymbol,
2 As CustomOrder,
--Convert(varchar, @PreviousBusinessDate,23) As TradeDate,
--@PreviousBusinessDate As TradeDate
FORMAT(CAST(@PreviousBusinessDate As Date),'yyyyMMdd') AS TradeDate

InTo #Temp_FinalTable
From #TempTable_Grouped T


Insert into #Temp_FinalTable        
select    
Convert(varchar, @InputDate,23) As HoldingDate,
'Cash' As AssetClass,  
 
CF.FundName As Account,                                 
'Cash-' + CurrencyLocal.CurrencySymbol As Symbol,
'Cash-' + CurrencyLocal.CurrencySymbol As SecurityName,                                  
Cash.CashValueLocal as Quantity,
0.0 As MarkPrice,
Cash.CashValueLocal As MarketValue_Local, 
Cash.CashValueBase As MarketValue_Base,    
0.0 As UnitCost,
0.0 As CostBasis_Local,
0.0 As CostBasis_Base,

1 As ContractSize,
'' As ISINSymbol,

'' As SEDOLSymbol,
'' As OSISymbol,
CurrencyLocal.CurrencySymbol As TradeCurrency,
CurrencyBase.CurrencySymbol As PortfolioCurrency ,
CurrencyLocal.CurrencySymbol As SettlementCurrency,
'' As BloombergSymbol,
1 As CustomOrder,
--FORMAT(CAST(@PreviousBusinessDate As Date),'yyyymmdd') AS TradeDate
--@PreviousBusinessDate As TradeDate
FORMAT(CAST(@PreviousBusinessDate As Date),'yyyyMMdd') AS TradeDate
                       
From PM_CompanyFundCashCurrencyValue Cash  With (NoLock)                     
Inner join #FundDetails CF On CF.FundId = Cash.FundID                      
Inner join T_Currency CurrencyLocal  With (NoLock) On CurrencyLocal.CurrencyId = Cash.LocalCurrencyID                                
Inner join T_Currency CurrencyBase  With (NoLock) On CurrencyBase.CurrencyId = Cash.BaseCurrencyID                                
Where DateDiff(Day, Cash.Date, @inputDate) = 0


Select *
From #Temp_FinalTable
Order By CustomOrder,Account,Symbol  
 

Drop Table #TempOpenPositionsTable, #TempTable_Grouped,#Temp_FinalTable, #FundDetails
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates,#ZeroFundMarkPriceEndDate,#ZeroFundFxRate
