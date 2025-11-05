/*
Desc: Customized Postion Report for ACT Capital: https://jira.nirvanasolutions.com:8443/browse/TG-7598

EXEC [P_GetCustomPositionReport_ACTCap_EOD_Jeffery] 1,'1,2,3,4,5,6,7,8,9,10,11,12','2020-11-18',1,'1',0,0,0

*/

CREATE Procedure [dbo].[P_GetCustomPositionReport_ACTCap_EOD_Jeffery]                        
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

--Set @ThirdPartyID = 1                                            
--Set @CompanyFundIDs = '1,2,3,4,5,6,7,8,9,10,12'                                                                                                                                                                       
--Set @InputDate = '2020-10-22'                                                                                                                                                                    
--Set @CompanyID = 1                                                                                                                                     
--set @AUECIDs = '1'                                                                            
--Set @TypeID = 0
--Set @DateType = 0                                                                                                                                                                         
--Set @FileFormatID  = 0  

Declare @PreviousBusinessDate DateTime
Set @CompanyFundIDs = '1,2,3,4,5,6,7,8,9,10,11,12'

Declare @Fund Table                                                           
(                
FundID int                      
)  

Insert into @Fund     
Select Cast(Items as int) from dbo.Split(@companyFundIDs,',') 

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
FROM PM_DayMarkPrice DMP
--Inner Join @Fund F On F.FundID = DMP.FundID  
WHERE DateDiff(Day,DMP.DATE,@InputDate) = 0 And DMP.FundID = 0 

-- For Fund Zero
SELECT *
INTO #ZeroFundMarkPriceEndDate
FROM #MarkPriceForEndDate
WHERE fundID = 0

SELECT PT.Taxlot_PK    
InTo #TempTaxlotPK         
FROM PM_Taxlots PT          
Inner Join @Fund Fund on Fund.FundID = PT.FundID              
Where PT.Taxlot_PK in                                       
(                                                                                                
 Select Max(Taxlot_PK) from PM_Taxlots                                                                                   
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@InputDate) >= 0                                                                      
 group by TaxlotId                                                                      
)                                                                                          
And PT.TaxLotOpenQty > 0   

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
WHERE fundID = 0

Create Table #TempAccountNAV
(
Account Varchar(100),
NAV Float
)

Insert InTo #TempAccountNAV

SELECT 
Funds.FundName As Account,
Sum( SubBal.CloseDrBalBase - SubBal.CloseCrBalBase) As NAV

FROM T_SubAccountBalances SubBal WITH(NOLOCK)
Inner Join @Fund TempF On TempF.FundID = SubBal.FundID
INNER JOIN T_SubAccounts SubAcc WITH(NOLOCK) ON SubAcc.SubAccountID = SubBal.SubAccountID
INNER JOIN T_CompanyFunds Funds WITH(NOLOCK) ON Funds.CompanyFundID = SubBal.FundID
--Inner Join T_CompanyMasterFundSubAccountAssociation MFA On MFA.CompanyFundID = Funds.CompanyFundID 
--Inner Join T_CompanyMasterFunds MF On MF.CompanyMasterFundID = MFA.CompanyMasterFundID
INNER JOIN T_SubCategory SubCat WITH(NOLOCK) ON SubCat.SubCategoryID = SubAcc.SubCategoryID
INNER JOIN T_MasterCategory MastCat WITH(NOLOCK) ON MastCat.MasterCategoryID = SubCat.MasterCategoryID
INNER JOIN T_TransactionType AccType WITH(NOLOCK) ON AccType.TransactionTypeId = SubAcc.TransactionTypeId
INNER JOIN T_CashPreferences CashPref WITH(NOLOCK) ON CashPref.FundID = SubBal.FundID
WHERE DateDiff(d, TransactionDate, @InputDate) = 0
	AND DATEDIFF(d, SubBal.TransactionDate, CashPref.CashMgmtStartDate) <= 0
	And MastCat.MasterCategoryID In (1,2,6)
Group By Funds.FundName
Order By Funds.FundName


    
    
Select  
Case 
	When (SM.AssetId In (2,4))
	Then 'Q' + SM.OSISymbol 
	Else SM.TickerSymbol 
End As Symbol,  
Case                      
	When dbo.GetSideMultiplier(PT.OrderSideTagValue) = 1                      
	Then 'LONG'                      
	Else 'SHORT'                      
End as PositionSide,               
CF.FundName As PortfolioName,  
CF.FundName As GsecAccount,       
SM.CompanyName As SecurityName,  
PT.TaxlotOpenQty As OpenQty,
IsNull(MPEndDate.Val, 0) As Price,
IsNull((PT.TaxlotOpenQty * IsNull(MPEndDate.Val, 0) * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)), 0) As MarketValue,
(PT.TaxlotOpenQty * PT.AvgPrice * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + (PT.OpenTotalCommissionAndFees) As CostBasis, 
Case 
	When (G.AssetID ='2' Or G.AssetID ='4')
	Then SM.ExpirationDate
Else '1/1/1900'
End As Expiration,
SM.StrikePrice As StrikePrice,
SM.Multiplier As ContractSize,
Case 
	When (SM.AssetId In (2,4))
	Then SM.PutOrCall
	Else '' 
End As OptionIndicator,
SM.CUSIPSymbol As UnderlyingCusip,
Case 
	When (SM.AssetId In (2,4))
	Then SMUnderlying.TickerSymbol
	Else '' 
End As UnderlyingTicker,
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
Curr.CurrencySymbol,
A.AssetName As AssetClass,
SM.UnderLyingSymbol

Into #TempOpenPositionsTable         
From PM_Taxlots PT
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK 
Inner Join V_SecMasterData SM On SM.TickerSymbol = PT.Symbol
Inner Join V_SecMasterData_WithUnderlying SMUnderlying On SMUnderlying.TickerSymbol = SM.UnderLyingSymbol
Inner Join T_Currency Curr On Curr.CurrencyID = SM.CurrencyID  
Inner Join T_CompanyFunds CF On CF.CompanyFundID = PT.FundID
Inner Join T_Asset A On A.AssetID = SM.AssetID
INNER JOIN T_Group G ON G.GroupID = PT.GroupID

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

Select    
Temp.Symbol,    
Temp.PositionSide As PositionSide,            
Temp.PortfolioName As PortfolioName,
Max(Temp.GsecAccount) As GsecAccount,              
Max(SecurityName) As SecurityName,
Sum(Temp.OpenQty * Temp.SideMultiplier) As OpenQty, 
Max(Price) As Price,
Sum(MarketValue) AS MarketValue, 
Sum(Temp.CostBasis) As CostBasis, 
Temp.Expiration,
Max(StrikePrice) As StrikePrice, 
Max(Temp.ContractSize) As ContractSize,      
Max(OptionIndicator) As OptionIndicator,
Max(UnderlyingCusip) As UnderlyingCusip,
Max(UnderlyingTicker) As UnderlyingTicker,
Max(SideMultiplier) As SideMultiplier,
Max(TradeFXRate) As TradeFXRate,
Max(EndDateFXRate) As EndDateFXRate,
Max(CurrencySymbol) As CurrencySymbol, 
Max(UnderlyingSymbol) As UnderlyingSymbol,
AssetClass As AssetClass
Into #TempTable              
From #TempOpenPositionsTable Temp  
Group By Temp.PortfolioName,Temp.Symbol,Temp.PositionSide,Temp.AssetClass,Temp.Expiration

Alter Table #TempTable      
Add UnitCost Float Null,
UnrealizedGainLoss Float Null,
PercentGainLoss Decimal(18,10) Null,
PercentEquity Decimal(18,10) Null
      
UPdate #TempTable      
Set UnitCost = 0.0,UnrealizedGainLoss = 0.0, PercentGainLoss = 0.0,
PercentEquity = 0.0    
      
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
	When IsNull(ACNAV.NAV,0) <> 0        
	Then ((OpenPos.MarketValue) / ACNAV.NAV ) * 100      
	Else 0        
End      
From #TempTable OpenPos
Inner Join #TempAccountNAV ACNAV On ACNAV.Account = OpenPos.PortfolioName     
      
Select 
Symbol,
PositionSide,
Case 
	When PortfolioName = 'ACT Capital Partners LP 43100955'
	Then 'ACT Capital Ptnrs LP'
	When PortfolioName = 'Whitethorne Fund LLC 43101260'
	Then 'Whitethorne Fund LLC'
	When PortfolioName = 'Gingko Fund LLC 43101259'
	Then 'Gingko Fund LLC'
	When PortfolioName = 'Ecker Family Partnership 43100963'
	Then 'Ecker Family Ptnrshp'
	When PortfolioName = '21st Century Ecker Family Partnership 43100964'
	Then '21st Century Ptnrshp'
	When PortfolioName = 'ACT Capital Mgt LLC 43100956'
	Then 'ACT Capital Mgt LLC'
	When PortfolioName = 'Amir L Ecker IRA 43100957'
	Then 'Amir L Ecker IRA 43100957'
	When PortfolioName = 'Amir L Ecker 43100958'
	Then 'Amir L Ecker 43100958'
	When PortfolioName = 'Amir L Ecker Roth 43100959'
	Then 'Amir L Ecker Roth 43100959'
	When PortfolioName = 'Maria T Ecker 43100960'
	Then 'Maria T Ecker 43100960'
	When PortfolioName = 'Maria T Ecker IRA 43100961'
	Then 'Maria T Ecker IRA 43100961'
	When PortfolioName = 'Ecker Joint 43100962'
	Then 'Ecker Joint 43100962'	
Else PortfolioName
End As PortfolioName,
Case 
	When GsecAccount = 'ACT Capital Partners LP 43100955'
	Then '43100955'
	When GsecAccount = 'Whitethorne Fund LLC 43101260'
	Then '43101260'
	When GsecAccount = 'Gingko Fund LLC 43101259'
	Then '43101259'
	When GsecAccount = 'Ecker Family Partnership 43100963'
	Then '43100963'
	When GsecAccount = '21st Century Ecker Family Partnership 43100964'
	Then '43100964'
	When GsecAccount = 'ACT Capital Mgt LLC 43100956'
	Then '43100956'
	When GsecAccount = 'Amir L Ecker IRA 43100957'
	Then '43100957'
	When GsecAccount = 'Amir L Ecker 43100958'
	Then '43100958'
	When GsecAccount = 'Amir L Ecker Roth 43100959'
	Then '43100959'
	When GsecAccount = 'Maria T Ecker 43100960'
	Then '43100960'
	When GsecAccount = 'Maria T Ecker IRA 43100961'
	Then '43100961'
	When GsecAccount = 'Ecker Joint 43100962'
	Then '43100962'	
Else GsecAccount
End As GsecAccount,
SecurityName,
OpenQty As Quantity,
Price,
MarketValue,
UnitCost,
CostBasis,
0 As LongTerm,
0 As ShortTerm,
UnrealizedGainLoss,
PercentGainLoss,
PercentEquity,
Convert(varchar,Expiration,101) As Expiration,
StrikePrice,
ContractSize,
OptionIndicator,
UnderlyingCusip,
UnderlyingTicker
From #TempTable 
Order By UnderLyingSymbol,AssetClass---- PortfolioName,AssetClass,Symbol,PositionSide 


Drop Table #TempOpenPositionsTable, #TempTable, #TempAccountNAV
Drop Table #TempTaxlotPK,#MarkPriceForEndDate, #FXConversionRates,#ZeroFundMarkPriceEndDate,#ZeroFundFxRate