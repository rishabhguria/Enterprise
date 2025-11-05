
CREATE Procedure [dbo].[P_GetOpenPos_NZS_Harbour_EOD]                            
(                                     
@ThirdPartyID int,                                                
@CompanyFundIDs varchar(max),                                                                                                                                                                              
@InputDate datetime,                                                                                                                                                                          
@CompanyID int,                                                                                                                                          
@AUECIDs varchar(max),                                                                                
@TypeID int,  
@DateType int,
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

--set @thirdPartyID=122
--set @companyFundIDs=N'4,1,5,8,3,7,6,12,15,13,14,11,10,9,2'
--set @inputDate='2023-12-08'
--set @companyID=7
--set @auecIDs=N'20,65,71,76,63,53,44,34,59,54,21,11,1,15,62,32,33'
--set @TypeID=0
--set @dateType=0
--set @fileFormatID=63

Set NoCount On

Set @companyFundIDs = '2'        

Declare @PreviousBusinessDate DateTime, @FundIds Varchar(500)  

Set @PreviousBusinessDate = dbo.AdjustBusinessDays(@inputDate,-1,1)
  
Declare @Fund Table                                                               
(                    
FundID int                          
)      
    
Insert into @Fund                                                                                                        
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',')     
 
Select * 
InTo #Fund_WithZeroFundId
From @Fund
Insert InTo #Fund_WithZeroFundId
Select 0

SELECT @FundIds = COALESCE(@FundIds + ',', '') + CAST(FundID AS VARCHAR(5)) FROM #Fund_WithZeroFundId

-- get Previous day Mark Price for End Date              
CREATE TABLE #DayMarkPriceForPreviousBusinessDate   
(    
  FinalMarkPrice FLOAT    
 ,Symbol VARCHAR(200)    
 ,FundID INT    
 )   
  
INSERT INTO #DayMarkPriceForPreviousBusinessDate   
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
WHERE DateDiff(Day,DMP.DATE,@PreviousBusinessDate) = 0

-- For Fund Zero
SELECT *
INTO #ZeroFundMarkPricePreviousBusinessDate
FROM #DayMarkPriceForPreviousBusinessDate
WHERE FundID = 0

Delete FROM #DayMarkPriceForPreviousBusinessDate WHERE FundID = 0
           
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

--select * from #MarkPriceForEndDate 

-- For Fund Zero
SELECT *
INTO #ZeroFundMarkPriceEndDate
FROM #MarkPriceForEndDate
WHERE FundID = 0

Delete FROM #MarkPriceForEndDate WHERE FundID = 0

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
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange_MW @PreviousBusinessDate ,@InputDate ,@FundIds

--Select * from #FXConversionRates

UPDATE #FXConversionRates
SET RateValue = 1.0 / RateValue
WHERE RateValue <> 0 AND ConversionMethod = 1

-- For FundID = 0 (Zero)
SELECT *
INTO #ZeroFundFxRate
FROM #FXConversionRates
WHERE FundID = 0 

Delete FROM #FXConversionRates WHERE FundID = 0 

Create Table #Temp_OpenPosition
(
FundID Int,
FundName Varchar(200),
Symbol Varchar(200),
PositionIndicator Varchar(20),
SEDOLSymbol Varchar(50),
CUSIPSymbol Varchar(50),
ISINSymbol Varchar(50),
LocalCurrency Varchar(10),
OpenPositions Float,
SecurityName Varchar(500),
AssetName Varchar(30),
PreviousDateFXRate Float,
EndDateFXRate Float,
FXRateWithTrade Float,
MarketValue_PreviousDay_Local Float,
MarketValue_PreviousDay_Base Float,
MarketValue_Ending_Local Float,
MarketValue_Ending_Base Float,
Netnotional_Local Float,
NetNotional_Base Float,
SideMultiplier Int,
Exchange Varchar(100),
PreviousDayEODCash Float,
P_E_Indicator Varchar(10),
CustomUDA9 varchar(200)
)

Create Table #Temp_SM
(
TickerSymbol Varchar(500),
CompanyName Varchar(1000),
AUECID Int,
ISINSymbol Varchar(50),
CUSIPSymbol Varchar(50),
SEDOLSymbol Varchar(50),
BloombergSymbol Varchar(100),
Multiplier Float,
CurrencyID Int,
AssetID Int,
CustomUDA9 Varchar(200)
)

Insert InTo #Temp_SM
Select 
TickerSymbol,
CompanyName,
AUECID,
ISINSymbol,
CUSIPSymbol,
SEDOLSymbol,
BloombergSymbol,
Multiplier,
CurrencyID,
AssetID,
UDA.CustomUDA9
From V_SecMasterData SM With (NOLock)
Inner Join V_UDA_DynamicUDA UDA On SM.Symbol_PK = UDA.Symbol_PK 


Create Table #TempTaxlotPK_PreviousBusinessDay
( 
Taxlot_PK BigInt
)

Insert InTo #TempTaxlotPK_PreviousBusinessDay
SELECT Distinct PT.Taxlot_PK 
FROM PM_Taxlots PT With (NoLock)          
Inner Join @Fund Fund on Fund.FundID = PT.FundID  
Where PT.Taxlot_PK in                                       
(                                                                                                
 Select Max(Taxlot_PK) from PM_Taxlots With (NoLock)                                                                                 
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@PreviousBusinessDate) >= 0                                                                      
 Group by TaxlotId                                                                      
)                                                                                          
And PT.TaxLotOpenQty > 0 

Select GroupId, CurrencyID, AUECLocalDate, Symbol, FXRate,FXConversionMethodOperator
InTo #Temp_Group_PreviousDate
From T_Group With (NoLock)
Where GroupId In
(
	Select Distinct GroupID From PM_Taxlots With (NoLock) 
	Where Taxlot_PK IN
	(
		Select Taxlot_PK From #TempTaxlotPK_PreviousBusinessDay
	)
)

Create Table #TempTaxlotPK_EndDate
( 
Taxlot_PK BigInt
)

Insert InTo #TempTaxlotPK_EndDate
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


Select GroupId, CurrencyID, AUECLocalDate, Symbol, FXRate,FXConversionMethodOperator
InTo #Temp_Group_EndDate
From T_Group With (NoLock) 
Where GroupId In
(
	Select Distinct GroupID From PM_Taxlots With (NoLock) 
	Where Taxlot_PK IN
	(
		Select Taxlot_PK From #TempTaxlotPK_EndDate
	)
)
---- Previous Day Open Positions
Insert InTo #Temp_OpenPosition
Select  
CF.CompanyFundID As FundID,
CF.FundName,               
PT.Symbol, 
'' As PositionIndicator,
SM.SEDOLSymbol As SEDOLSymbol,
SM.CUSIPSymbol As CUSIPSymbol,    
SM.ISINSymbol As ISINSymbol,                     
Curr.CurrencySymbol As LocalCurrency,    
PT.TaxlotOpenQty As OpenPositions,     
SM.CompanyName AS SecurityName,
A.AssetName As AssetName,
Case 
	When SM.CurrencyID = CF.LocalCurrency 
	Then 1
Else IsNull(FXRatesFoPreviousDate.Val,0) 
End As PreviousDateFXRate,
0 As EndDateFXRate,
0 As FXRateWithTrade,
IsNull((PT.TaxlotOpenQty * IsNull(MPPreviousDate.Val, 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) As MarketValue_PreviousDay_Local,
0 As MarketValue_PreviousDay_Base, 
0 As MarketValue_Ending_Local,
0 As MarketValue_Ending_Base, 
0 As Netnotional_Local, 
0 As NetNotional_Base,  
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,
AU.DisplayName As Exchange,
0 As PreviousDayEODCash,
'P' As P_E_Indicator,
SM.CustomUDA9
 
From PM_Taxlots PT With (NOLock)   
Inner Join #TempTaxlotPK_PreviousBusinessDay Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
INNER JOIN #Temp_Group_PreviousDate G ON G.GroupID = PT.GroupID 
Inner Join T_CompanyFunds CF With (NOLock) On CF.CompanyFundID = PT.FundID    
Inner Join #Temp_SM SM On SM.TickerSymbol = PT.Symbol   
Inner Join T_Currency Curr With (NOLock) On Curr.CurrencyID = SM.CurrencyID
INNER JOIN T_AUEC AU With (NoLock) ON AU.AUECID = SM.AUECID 
Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID
LEFT OUTER JOIN #DayMarkPriceForPreviousBusinessDate MP_Previous ON (MP_Previous.Symbol = PT.Symbol AND MP_Previous.FundID = PT.FundID )
LEFT OUTER JOIN #ZeroFundMarkPricePreviousBusinessDate MPZero_Previous ON (PT.Symbol = MPZero_Previous.Symbol AND MPZero_Previous.FundID = 0)
CROSS APPLY (
	SELECT CASE 
			WHEN MP_Previous.FinalMarkPrice IS NULL
				THEN 
					CASE 
						WHEN MPZero_Previous.FinalMarkPrice IS NULL
						THEN 0
						ELSE MPZero_Previous.FinalMarkPrice
					END
			ELSE MP_Previous.Finalmarkprice
			END
	) AS MPPreviousDate(Val) 

LEFT OUTER JOIN #FXConversionRates FXDayRatesForPreviousDate ON (                
	FXDayRatesForPreviousDate.FromCurrencyID = G.CurrencyID AND FXDayRatesForPreviousDate.ToCurrencyID = CF.LocalCurrency                                                                                                             
	AND DateDiff(d, @PreviousBusinessDate, FXDayRatesForPreviousDate.DATE) = 0  AND FXDayRatesForPreviousDate.FundID = PT.FundID ) 
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (     
   ZeroFundFxRateEndDate.FromCurrencyID = G.CurrencyID AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency 
   AND DateDiff(d, @PreviousBusinessDate, ZeroFundFxRateEndDate.DATE) = 0 AND ZeroFundFxRateEndDate.FundID = 0 )		
CROSS APPLY (              
SELECT 
	CASE               
	WHEN FXDayRatesForPreviousDate.RateValue IS NULL 
	THEN 
		CASE      
			WHEN ZeroFundFxRateEndDate.RateValue IS NULL              
			THEN 0        
		ELSE ZeroFundFxRateEndDate.RateValue  
		END              
	ELSE FXDayRatesForPreviousDate.RateValue              
	END              
  ) AS FXRatesFoPreviousDate(Val)   

Where PT.TaxlotOpenQty > 0

---- Ending Day Open Positions 
Insert InTo #Temp_OpenPosition      
Select  
CF.CompanyFundID As FundID,
CF.FundName,               
PT.Symbol, 
'' As PositionIndicator,
SM.SEDOLSymbol As SEDOLSymbol,
SM.CUSIPSymbol As CUSIPSymbol,    
SM.ISINSymbol As ISINSymbol,                     
Curr.CurrencySymbol As LocalCurrency,    
PT.TaxlotOpenQty As OpenPositions,     
SM.CompanyName AS SecurityName,
A.AssetName As AssetName,
0 As PreviousDateFXRate,
Case 
	When SM.CurrencyID = CF.LocalCurrency 
	Then 1
Else IsNull(FXRatesForEndDate.Val,0) 
End As EndDateFXRate,
0 As FXRateWithTrade,
0 As MarketValue_PreviousDay_Local,
0 As MarketValue_PreviousDay_Base, 
IsNull((PT.TaxlotOpenQty * IsNull(MPEndDate.Val, 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) As MarketValue_Ending_Local,
0 As MarketValue_Ending_Base,
0 As Netnotional_Local, 
0 As NetNotional_Base,  
dbo.GetSideMultiplier(PT.OrderSideTagValue) As SideMultiplier,
AU.DisplayName As Exchange,
0 As PreviousDayEODCash,
'E' As P_E_Indicator,
SM.CustomUDA9
             
From PM_Taxlots PT With (NOLock)   
Inner Join #TempTaxlotPK_EndDate Temp On Temp.Taxlot_PK = PT.Taxlot_PK     
Inner Join #Temp_SM SM On SM.TickerSymbol = PT.Symbol
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID    
Inner Join T_Currency Curr With (NOLock) On Curr.CurrencyID = SM.CurrencyID  
INNER JOIN #Temp_Group_EndDate G ON G.GroupID = PT.GroupID
Inner Join T_Asset A With (NoLock) On A.AssetID = SM.AssetID
INNER JOIN T_AUEC AU With (NoLock) ON AU.AUECID = SM.AUECID 
LEFT OUTER JOIN #MarkPriceForEndDate MPE With (NoLock) ON (MPE.Symbol = PT.Symbol AND MPE.FundID = PT.FundID )
LEFT OUTER JOIN #ZeroFundMarkPriceEndDate MPZeroEndDate ON (PT.Symbol = MPZeroEndDate.Symbol AND MPZeroEndDate.FundID = 0)
CROSS APPLY (
	SELECT CASE 
			WHEN MPE.FinalMarkPrice IS NULL
				THEN 
					CASE 
						WHEN MPZeroEndDate.FinalMarkPrice IS NULL
						THEN 0
						ELSE MPZeroEndDate.FinalMarkPrice
					END
			ELSE MPE.Finalmarkprice
			END
	) AS MPEndDate(Val)  

LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (                
	FXDayRatesForEndDate.FromCurrencyID = G.CurrencyID AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency                                                                                                             
	AND DateDiff(d, @InputDate, FXDayRatesForEndDate.DATE) = 0  AND FXDayRatesForEndDate.FundID = PT.FundID ) 
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (     
   ZeroFundFxRateEndDate.FromCurrencyID = G.CurrencyID AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency 
   AND DateDiff(d, @InputDate, ZeroFundFxRateEndDate.DATE) = 0 AND ZeroFundFxRateEndDate.FundID = 0 )		
CROSS APPLY (              
SELECT 
	CASE               
	WHEN FXDayRatesForEndDate.RateValue IS NULL 
	THEN 
		CASE      
			WHEN ZeroFundFxRateEndDate.RateValue IS NULL              
			THEN 0        
			ELSE ZeroFundFxRateEndDate.RateValue  
		END              
	ELSE FXDayRatesForEndDate.RateValue              
	END              
  ) AS FXRatesForEndDate(Val) 
Where PT.TaxlotOpenQty > 0


---- Today's trades

Select  
CF.CompanyFundID As FundID,
CF.FundName,               
VT.Symbol, 
SM.SEDOLSymbol As SEDOLSymbol,
SM.CUSIPSymbol As CUSIPSymbol,    
SM.ISINSymbol As ISINSymbol,                     
Curr.CurrencySymbol As LocalCurrency,    
VT.TaxLotQty As OpenPositions,     
SM.CompanyName AS SecurityName,
A.AssetName As AssetName,
Case 
	When VT.CurrencyID = CF.LocalCurrency 
	Then 1
	Else  
	Case
		When IsNull(VT.FXRate_Taxlot,0) > 0 And VT.FXConversionMethodOperator_Taxlot Is Not Null And VT.FXConversionMethodOperator_Taxlot = 'D'
		Then 1/VT.FXRate_Taxlot
		When IsNull(VT.FXRate_Taxlot,0) > 0 And IsNull(VT.FXConversionMethodOperator_Taxlot,'M') = 'M'
		Then IsNull(VT.FXRate_Taxlot,0)
		Else IsNull(FXRatesForEndDate.Val,0) 
	End
End As FXRateWithTrade, 
(((VT.TaxLotQty * VT.AvgPrice * SM.Multiplier) + (VT.TotalExpenses * VT.SideMultiplier)) *VT.SideMultiplier) As Netnotional_Local, 
Cast(0 As Float) As NetNotional_Base,  
VT.SideMultiplier As SideMultiplier,
AU.DisplayName As Exchange,
SM.CustomUDA9

InTo #Temp_CurrentDateTaxlots           
From V_Taxlots VT With (NOLock)
Inner Join @Fund Fund on Fund.FundID = VT.FundID  
Inner Join T_CompanyFunds CF On CF.CompanyFundID = VT.FundID    
Inner Join T_Currency Curr With (NOLock) On Curr.CurrencyID = VT.CurrencyID  
Inner Join T_Asset A With (NoLock) On A.AssetID = VT.AssetID
INNER JOIN T_AUEC AU With (NoLock) ON AU.AUECID = VT.AUECID 
Inner Join #Temp_SM SM On SM.TickerSymbol = VT.Symbol

LEFT OUTER JOIN #FXConversionRates FXDayRatesForEndDate ON (                
	FXDayRatesForEndDate.FromCurrencyID = VT.CurrencyID AND FXDayRatesForEndDate.ToCurrencyID = CF.LocalCurrency                                                                                                             
	AND DateDiff(d, @InputDate, FXDayRatesForEndDate.DATE) = 0  AND FXDayRatesForEndDate.FundID = VT.FundID ) 
LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateEndDate ON (     
   ZeroFundFxRateEndDate.FromCurrencyID = VT.CurrencyID AND ZeroFundFxRateEndDate.ToCurrencyID = CF.LocalCurrency 
   AND DateDiff(d, @InputDate, ZeroFundFxRateEndDate.DATE) = 0 AND ZeroFundFxRateEndDate.FundID = 0 )		
CROSS APPLY (              
SELECT 
	CASE               
	WHEN FXDayRatesForEndDate.RateValue IS NULL 
	THEN 
		CASE      
			WHEN ZeroFundFxRateEndDate.RateValue IS NULL              
			THEN 0        
			ELSE ZeroFundFxRateEndDate.RateValue  
		END              
	ELSE FXDayRatesForEndDate.RateValue              
	END              
  ) AS FXRatesForEndDate(Val) 
Where DateDiff(Day,VT.AUECLocalDate,@InputDate) = 0

Update #Temp_CurrentDateTaxlots
Set NetNotional_Base = Netnotional_Local * FXRateWithTrade

-- Currenct date Taxlots
Insert InTo #Temp_OpenPosition 
Select 
FundID, 
FundName, 
Symbol,
Cast('' As Varchar(10)) As PositionIndicator,
Max(SEDOLSymbol) As SEDOLSymbol,
Max(CUSIPSymbol) As CUSIPSymbol, 
Max(ISINSymbol) As ISINSymbol,
Max(LocalCurrency) As LocalCurrency,
0 As OpenPositions,
Max(SecurityName) As SecurityName,
Max(AssetName) As AssetName,
0 As PreviousDateFXRate,
0 As EndDateFXRate,
0 As FXRateWithTrade,
0 As MarketValue_PreviousDay_Local,
0 As MarketValue_PreviousDay_Base,
0 As MarketValue_Ending_Local,
0 As MarketValue_Ending_Base,
Sum(Netnotional_Local) As Netnotional_Local,
Sum(NetNotional_Base) as NetNotional_Base,
1 As SideMultiplier,
Max(Exchange) As Exchange,
0 As PreviousDayEODCash,
'TT' As P_E_Indicator,
Max(CustomUDA9) As CustomUDA9
From #Temp_CurrentDateTaxlots
Group By FundID,FundName,Symbol 

Update #Temp_OpenPosition 
Set PositionIndicator = 
Case 
When NetNotional_Base >= 0
Then 'BUY'
Else 'SELL'
End 
Where P_E_Indicator = 'TT'

--Select *
--from #Temp_OpenPosition
--Where P_E_Indicator = 'TT'


--Select 
-- FundID,
-- Sum(CashValueBase) As CashValueBase
-- InTo #Temp_EODCash_PreviousDate
-- From PM_CompanyFundCashCurrencyValue EODCash With (NOLock) 
-- Where DateDiff(DAY,EODCash.Date, @PreviousBusinessDate) = 0
-- Group By FundID

 --Update P
 --Set PreviousDayEODCash = EODCash.CashValueBase 
 --From  #Temp_OpenPosition P
 --Inner Join #Temp_EODCash_PreviousDate EODCash On EODCash.FundID = P.FundID
 --Where P_E_Indicator = 'P'
 
Declare  @TotalMarketValue_PreviousDay_Base Float, @TotalMarketValue_Ending_Base Float--@NAV_PreviousDay_Base Float,
Set @TotalMarketValue_PreviousDay_Base = 0
Set @TotalMarketValue_Ending_Base = 0

Update #Temp_OpenPosition
Set MarketValue_PreviousDay_Base = MarketValue_PreviousDay_Local * PreviousDateFXRate
Where P_E_Indicator = 'P'

--Select *
--From #Temp_OpenPosition
--Where P_E_Indicator = 'P'


Update #Temp_OpenPosition
Set MarketValue_Ending_Base = MarketValue_Ending_Local * EndDateFXRate,
NetNotional_Base = Netnotional_Local * FXRateWithTrade
Where P_E_Indicator = 'E'



Set @TotalMarketValue_PreviousDay_Base = (Select Sum(MarketValue_PreviousDay_Base) from #Temp_OpenPosition Where P_E_Indicator = 'P')

--Set @NAV_PreviousDay_Base = (Select Sum(MarketValue_PreviousDay_Base + PreviousDayEODCash) from #Temp_OpenPosition Where P_E_Indicator = 'P')

Set @TotalMarketValue_Ending_Base = (Select Sum(MarketValue_Ending_Base + PreviousDayEODCash) from #Temp_OpenPosition Where P_E_Indicator = 'E')

--Select @NAV_PreviousDay_Base As NAV_PREV, @TotalMarketValue_PreviousDay_Base As MKTVALPREV, @TotalMarketValue_Ending_Base AS MKTVALENDING

Select 
Symbol,
Sum(
	Case
	When P_E_Indicator = 'E'
	Then OpenPositions
	Else 0
	End) As OpenPositions,
MAX(
	Case
	When P_E_Indicator = 'TT'
	Then PositionIndicator
	Else ''
	End) As PositionIndicator,
Max(LocalCurrency) As LocalCurrency,
Max(ISINSymbol) As ISINSymbol,
Max(CUSIPSymbol) As CUSIPSymbol, 
Max(SEDOLSymbol) As SEDOLSymbol,
Max(SecurityName) As SecurityName,
Max(AssetName) As AssetName,
Max(Exchange) As Exchange,
Sum(MarketValue_Ending_Base) As MarketValue_Ending_Base,
Sum(MarketValue_PreviousDay_Base) As MarketValue_PreviousDay_Base,
Sum(Netnotional_Local) As Netnotional_Local,
Sum(NetNotional_Base) as NetNotional_Base,
Max(CustomUDA9) As CustomUDA9
InTo #Temp_OpenPosition_Grouped
From #Temp_OpenPosition
Group By FundName,Symbol 

Select 
Symbol,
OpenPositions,
PositionIndicator,
LocalCurrency,
ISINSymbol,
CUSIPSymbol, 
SEDOLSymbol,
SecurityName,
AssetName,
Exchange,
CONVERT(VARCHAR(10), @InputDate, 101) AS TradeDate,
MarketValue_Ending_Base,
MarketValue_PreviousDay_Base,
Netnotional_Local,
NetNotional_Base,
@TotalMarketValue_Ending_Base As TotalMarketValue_Ending_Base,
Case
	When @TotalMarketValue_Ending_Base Is Not Null And @TotalMarketValue_Ending_Base <> 0
	Then (MarketValue_Ending_Base / @TotalMarketValue_Ending_Base) * 100
	Else 0
End As CurrPortfolioWeight,
@TotalMarketValue_PreviousDay_Base As TotalMarketValue_PreviousDay_Base,
Case
	When @TotalMarketValue_PreviousDay_Base Is Not Null And @TotalMarketValue_PreviousDay_Base <> 0
	Then (MarketValue_PreviousDay_Base / @TotalMarketValue_PreviousDay_Base) * 100
	Else 0
End As PrevoiusPortfolioWeight,
@TotalMarketValue_PreviousDay_Base As NAV_PreviousDay_Base,
Case
	When @TotalMarketValue_PreviousDay_Base Is Not Null And @TotalMarketValue_PreviousDay_Base <> 0
	Then (NetNotional_Base / @TotalMarketValue_PreviousDay_Base)  * 100
	Else 0
End As TRADED_WEIGHT,
CustomUDA9
From #Temp_OpenPosition_Grouped
Order By Symbol
 

Drop Table #FXConversionRates, #ZeroFundFxRate, #Temp_OpenPosition,#Temp_OpenPosition_Grouped,#Fund_WithZeroFundId,#Temp_SM
Drop Table #TempTaxlotPK_PreviousBusinessDay,#DayMarkPriceForPreviousBusinessDate,#ZeroFundMarkPricePreviousBusinessDate,#Temp_Group_PreviousDate--,#Temp_EODCash_PreviousDate
Drop Table #TempTaxlotPK_EndDate, #MarkPriceForEndDate,#ZeroFundMarkPriceEndDate,#Temp_Group_EndDate
Drop Table #Temp_CurrentDateTaxlots