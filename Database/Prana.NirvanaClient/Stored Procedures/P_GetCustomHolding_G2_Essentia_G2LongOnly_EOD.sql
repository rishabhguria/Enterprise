/*

EXEC P_GetCustomHolding_G2_Essentia_EOD 1, '1240,1239,1234,1238,1244,1245,1242,1243,1241', '09-15-2021',1,1,1,1,1

*/

CREATE Procedure [dbo].[P_GetCustomHolding_G2_Essentia_NEW_EOD]                        
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
--@TypeID int,  -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties                                            
--@DateType int, -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                                            
--@FileFormatID int  

-- set @thirdPartyID=36
-- set @companyFundIDs=N'1240,1239,1246,1247,1234,1238,1244,1245,1242,1243'
-- set @inputDate='2024-04-05 02:47:43'
-- set @companyID=7
-- set @auecIDs=N'63,44,34,43,59,31,54,18,161,61,74,103,1,15,11,62,73,12,32,81'
-- set @TypeID=0
-- set @dateType=0
-- set @fileFormatID=33
 
Declare @PreviousBusinessDate DateTime

Declare @varMasterFundIds Varchar(1000)

Set @varMasterFundIds = '27'

Declare @MasterFunds Table
(
MasterFundId Int,
MasterFundName Varchar(200)
)

Insert into @MasterFunds  
Select 
CompanyMasterFundID,
MasterFundName 
From T_CompanyMasterFunds
Where CompanyMasterFundID IN
(  
Select Cast(Items as int) from dbo.Split(@varMasterFundIds,',') 
) 

Declare @Fund Table                                                       
(                
FundID int                      
)  

Insert into @Fund 
Select Funds.CompanyFundID
From T_CompanyFunds Funds WITH(NOLOCK)
Inner Join T_CompanyMasterFundSubAccountAssociation MFA On MFA.CompanyFundID = Funds.CompanyFundID 
Inner Join T_CompanyMasterFunds MF On MF.CompanyMasterFundID = MFA.CompanyMasterFundID
Inner Join @MasterFunds T On T.MasterFundId = MF.CompanyMasterFundID

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
FROM PM_DayMarkPrice DMP  With (NoLock)
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0

-- For Fund Zero
SELECT *
INTO #ZeroFundMarkPriceEndDate
FROM #MarkPriceForEndDate
WHERE FundID = 0

Create Table #TempTaxlotPK
( 
Taxlot_PK BigInt
)

Insert InTo #TempTaxlotPK
SELECT Distinct PT.Taxlot_PK    

FROM PM_Taxlots PT With (NoLock)          
Inner Join @Fund Fund on Fund.FundID = PT.FundID  
Inner Join T_Group G On G.GroupID = PT.GroupID             
Where PT.Taxlot_PK in                                       
(                                                                                                
 Select Max(Taxlot_PK) from PM_Taxlots With (NoLock)                                                                                 
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                      
 Group by TaxlotId                                                                      
)                                                                                          
And Round(PT.TaxLotOpenQty,2) > 0 
And G.AssetID = 1
And PT.Symbol Not In ('FSMXX')   

Set @PreviousBusinessDate = (Select Min(G.AUECLocalDate) As MinTradeDate
FROM PM_Taxlots PT
Inner Join #TempTaxlotPK T On T.Taxlot_PK = PT.Taxlot_PK
INNER JOIN T_Group G ON PT.GroupID = G.GroupID)


Set @PreviousBusinessDate = dbo.AdjustBusinessDays(@PreviousBusinessDate,-1,1)

--Select @PreviousBusinessDate

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
EXEC P_GetAllFXConversionRatesForGivenDateRange @PreviousBusinessDate ,@InputDate

UPDATE #FXConversionRates
SET RateValue = 1.0 / RateValue
WHERE RateValue <> 0
	AND ConversionMethod = 1

-- For FundID = 0 (Zero)
SELECT *
INTO #ZeroFundFxRate
FROM #FXConversionRates
WHERE FundID = 0

Create Table #TempMasterFundNAV
(
MasterFund Varchar(100),
NAV Float
)

Insert InTo #TempMasterFundNAV
SELECT 
MF.MasterFundName As MasterFund,
Sum(SubBal.CloseDrBalBase - SubBal.CloseCrBalBase) As NAV
FROM T_SubAccountBalances SubBal WITH(NOLOCK)
Inner Join @Fund TempF On TempF.FundID = SubBal.FundID
INNER JOIN T_SubAccounts SubAcc WITH(NOLOCK) ON SubAcc.SubAccountID = SubBal.SubAccountID
INNER JOIN T_CompanyFunds Funds WITH(NOLOCK) ON Funds.CompanyFundID = SubBal.FundID
Inner Join T_CompanyMasterFundSubAccountAssociation MFA On MFA.CompanyFundID = Funds.CompanyFundID 
Inner Join T_CompanyMasterFunds MF On MF.CompanyMasterFundID = MFA.CompanyMasterFundID
INNER JOIN T_SubCategory SubCat WITH(NOLOCK) ON SubCat.SubCategoryID = SubAcc.SubCategoryID
INNER JOIN T_MasterCategory MastCat WITH(NOLOCK) ON MastCat.MasterCategoryID = SubCat.MasterCategoryID
INNER JOIN T_TransactionType AccType WITH(NOLOCK) ON AccType.TransactionTypeId = SubAcc.TransactionTypeId
INNER JOIN T_CashPreferences CashPref WITH(NOLOCK) ON CashPref.FundID = SubBal.FundID
WHERE DateDiff(d, TransactionDate, @InputDate) = 0
	AND DATEDIFF(d, SubBal.TransactionDate, CashPref.CashMgmtStartDate) <= 0
	And MastCat.MasterCategoryID In (1,2,6)
Group By MF.MasterFundName
Order By MF.MasterFundName


----Select * from #TempMasterFundNAV

    
    
Select  
Case 
	When (SM.AssetId In (2,4))
	Then SM.OSISymbol 
	Else SM.TickerSymbol 
End As Symbol,  
Case                      
	When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                      
	Then 'LONG'                      
	Else 'SHORT'                      
End as PositionSide,               
CF.FundName As PortfolioName,        
SM.CompanyName As SecurityName,  
PT.TaxlotOpenQty As OpenQty,
IsNull(MPEndDate.Val, 0) As MarkPrice,
IsNull((PT.TaxlotOpenQty * IsNull(MPEndDate.Val, 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) As MarketValue,
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) As CostBasis, 
SM.Multiplier As ContractSize,

SM.ISINSymbol As ISINSymbol,
SM.CUSIPSymbol As CUSIPSymbol,
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
A.AssetName As AssetClass,
SM.UnderLyingSymbol,
MF.MasterFundName As MasterFund

Into #TempOpenPositionsTable         
From PM_Taxlots PT With (NoLock)
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol
Inner Join T_Currency TradeCurr With (NoLock) On TradeCurr.CurrencyID = SM.CurrencyID  
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = PT.FundID
Inner Join T_CompanyMasterFundSubAccountAssociation MFA On MFA.CompanyFundID = CF.CompanyFundID 
Inner Join T_CompanyMasterFunds MF On MF.CompanyMasterFundID = MFA.CompanyMasterFundID
Inner Join T_Currency PortfolioCurr With (NoLock) On PortfolioCurr.CurrencyID = CF.LocalCurrency
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

Update #TempOpenPositionsTable
Set CostBasis = CostBasis * TradeFXRate,
MarketValue = MarketValue * EndDateFXRate

----Select * from #TempOpenPositionsTable

Select  
Temp.Symbol,               
Temp.PortfolioName As PortfolioName,            
Max(SecurityName) As SecurityName,
Sum(Temp.OpenQty * Temp.SideMultiplier) As OpenQty, 
Max(MarkPrice) As MarkPrice,
Sum(MarketValue) AS MarketValue,
--Sum(ABS(MarketValue)) AS GrossMarketValue,  
Cast(0.0 As Float) AS GrossMarketValue, 
Sum(Temp.CostBasis) As CostBasis, 
Max(Temp.ContractSize) As ContractSize,      
Max(ISINSymbol) As ISINSymbol,
Max(CUSIPSymbol) As CUSIPSymbol,
Max(SEDOLSymbol) As SEDOLSymbol,
Max(OSISymbol) As OSISymbol,
Max(SideMultiplier) As SideMultiplier,
Max(TradeFXRate) As TradeFXRate,
Max(EndDateFXRate) As EndDateFXRate,
Max(TradeCurrency) As TradeCurrency,
Max(PortfolioCurrency) As PortfolioCurrency, 
Max(UnderlyingSymbol) As UnderlyingSymbol,
AssetClass As AssetClass,
MasterFund

Into #TempTable              
From #TempOpenPositionsTable Temp  
Group By Temp.MasterFund, Temp.PortfolioName,Temp.Symbol,Temp.AssetClass--,Temp.PositionSide


Update #TempTable
Set GrossMarketValue = ABS(MarketValue)

Alter Table #TempTable      
Add UnitCost Float Null,
UnrealizedGainLoss Float Null,
PercentGainLoss Decimal(18,10) Null,
PercentEquity Decimal(18,10) Null,
MasterFundNAV Float Null
      
UPdate #TempTable      
Set UnitCost = 0.0,UnrealizedGainLoss = 0.0, PercentGainLoss = 0.0,PercentEquity = 0.0    
      
UPdate #TempTable      
Set UnitCost =       
Case        
	When OpenQty <> 0 And ContractSize <> 0        
	Then (CostBasis/OpenQty) /ContractSize        
	Else 0        
End,
UnrealizedGainLoss = MarketValue - CostBasis ,
PercentGainLoss = 
Case 
	When CostBasis <> 0
	Then  (MarketValue - CostBasis)/ CostBasis
Else 0
End
--From #TempTable 

UPdate OpenPos      
Set PercentEquity =       
Case        
	When IsNull(MNAV.NAV,0) <> 0        
	Then ((OpenPos.MarketValue) / MNAV.NAV ) * 100      
	Else 0        
End ,
MasterFundNAV =  MNAV.NAV   
From #TempTable OpenPos
Inner Join #TempMasterFundNAV MNAV On MNAV.MasterFund = OpenPos.MasterFund   
     
	 
Create Table #MasterFundGrossMarket
(
MasterFund Varchar(200),
GrossMarketValue Float	  
)


Insert InTo #MasterFundGrossMarket
Select 
MasterFund,
Sum(GrossMarketValue) As AccountGrossMarketValue
From #TempTable
Group By MasterFund

--Select * from #MasterFundGrossMarket
--Order By MasterFund

Select 
Convert(varchar, @InputDate,23) As HoldingDate,  
Symbol,
PortfolioName,
SecurityName,
Round(OpenQty,0) As Quantity,
MarkPrice,
MarketValue,
MGMV.GrossMarketValue As GrossMarketValue,
Case
When MGMV.GrossMarketValue Is Not Null And MGMV.GrossMarketValue <> 0
Then MarketValue / MGMV.GrossMarketValue
Else 0
End As PortfolioWeight_1,
MasterFundNAV,
UnitCost,
CostBasis As TotalCost,
UnrealizedGainLoss,
PercentGainLoss,
PercentEquity,
ContractSize,
ISINSymbol,
CUSIPSymbol,
SEDOLSymbol,
OSISymbol,
TradeCurrency,
PortfolioCurrency,
AssetClass,
T.MasterFund
InTo #Temp_FinalTable
From #TempTable T
Inner Join #MasterFundGrossMarket MGMV On MGMV.MasterFund = T.MasterFund 
--Where Round(Abs(MarketValue),2) <> Round(GrossMarketValue,2)
Order By PortfolioName,Symbol--,UnderLyingSymbol,AssetClass

Alter Table #Temp_FinalTable
Add PortfolioWeight Decimal(18,15)

Update #Temp_FinalTable 
Set PortfolioWeight = 0.0

Update #Temp_FinalTable 
Set PortfolioWeight = Cast(PortfolioWeight_1 As decimal(18,15))

--Update #Temp_FinalTable
--Set PortfolioWeight = 0.0
--Where Round(Abs(PortfolioWeight),4)  = 0

Select *
From #Temp_FinalTable
--Where SEDOLSymbol = 'BQT3XY6'
Order By PortfolioName,Symbol

Drop Table #TempOpenPositionsTable, #TempTable, #TempMasterFundNAV,#MasterFundGrossMarket,#Temp_FinalTable
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates,#ZeroFundMarkPriceEndDate,#ZeroFundFxRate