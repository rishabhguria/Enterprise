/*

Desc: This SP is used to get Start Day of NAV
EXEC P_GetAccountNAV_Calculated_EOD_Batch ''
*/

CREATE Procedure [dbo].[P_GetAccountNAV_Calculated_EOD_Batch]
(
@CompanyFundIDs  VARCHAR(1000),
@date date

)
As
SET NOCOUNT On  

--Declare @CompanyFundIDs VARCHAR(1000) 

Declare @CurrentBusinessDate DATE, @PreviousBusinessDate Date

Set @CurrentBusinessDate = DBO.AdjustBusinessDays(@date,0,1)

--Set @PreviousBusinessDate = DBO.AdjustBusinessDays(@CurrentBusinessDate,-1,1)

Create Table #FundIds 
(
FundID Int
)                                                      
 If (@CompanyFundIDs Is NULL Or @CompanyFundIDs = '')                                                      
 Insert InTo #FundIds                                                      
	 Select CompanyFundID as FundID 
	 From T_CompanyFunds Where IsActive=1                                                    
 Else                                                      
	 Insert into #FundIds                                                      
	 Select Items as FundID from dbo.Split(@CompanyFundIDs,',') 

SELECT  
	CF.CompanyFundID As FundId,   
    CF.FundName AS AccounName,
	CF.LocalCurrency
INTO #TempFunds
FROM #FundIds TF 
Inner Join T_CompanyFunds CF With (NoLock) on CF.CompanyFundID = TF.FundID

-- get Mark Price for previous business Date                        
CREATE TABLE #MarkPriceForBusinessDate 
(
	Finalmarkprice FLOAT
	,Symbol VARCHAR(max)
	,FundID INT
)

INSERT INTO #MarkPriceForBusinessDate 
	(
	Finalmarkprice
	,Symbol
	,FundID
	)
SELECT DISTINCT DMP.Finalmarkprice
	,DMP.Symbol
	,DMP.FundID
FROM PM_DayMarkPrice DMP With (NoLock)
	Where DateDiff(DAY, DMP.DATE, @CurrentBusinessDate) = 0

-- For Fund Zero                  
SELECT *
INTO #ZeroFundMarkPriceBusinessDate
FROM #MarkPriceForBusinessDate
WHERE FundID = 0

Delete From #MarkPriceForBusinessDate
Where FundID = 0

-- get forex rates for 2 date ranges                        
CREATE TABLE #FXConversionRates (
	FromCurrencyID INT
	,ToCurrencyID INT
	,RateValue FLOAT
	,ConversionMethod INT
	,DATE DATETIME
	,eSignalSymbol VARCHAR(max)
	,FundID INT
	)

--insert FX Rate for date ranges in the temp table                        
INSERT INTO #FXConversionRates
EXEC P_GetAllFXConversionRatesFundWiseForGivenDateRange @CurrentBusinessDate ,@CurrentBusinessDate
                        
UPDATE #FXConversionRates
SET RateValue = 1.0 / RateValue
WHERE RateValue <> 0 AND ConversionMethod = 1

-- For Fund Zero                  
SELECT *
INTO #ZeroFundFxRate
FROM #FXConversionRates
WHERE FundID = 0

Delete From #FXConversionRates
Where FundID = 0

Select Taxlot_PK
InTo #Temp_TaxlotPK
From PM_Taxlots PT With (NoLock)
INNER JOIN #TempFunds F ON F.FundID = PT.FundID
Where PT.Taxlot_PK IN (
		SELECT Max(Taxlot_PK) FROM PM_Taxlots With (NoLock)
		WHERE DateDiff(Day, AUECModifiedDate, @CurrentBusinessDate) >= 0
		GROUP BY TaxlotID
		)
And TaxLotOpenQty > 0

Select
PT.Symbol,
PT.FundID,
CF.AccounName,
PT.TaxLotOpenQty,
CASE 
WHEN G.CurrencyID <> CF.LocalCurrency
Then IsNull(IsNull(MPForADate.Val, 0) * PT.TaxlotOpenQty * SM.Multiplier * DBO.GetSideMultiplier(PT.OrderSideTagValue) * IsNull(FXRatesForADate.Val, 0), 0)
Else IsNull(IsNull(MPForADate.Val, 0) * PT.TaxlotOpenQty * SM.Multiplier * DBO.GetSideMultiplier(PT.OrderSideTagValue), 0) 
End AS MarketValue_Base 

InTo #Temp_SymbolMarketValue

FROM PM_Taxlots PT With (NoLock)
Inner Join #Temp_TaxlotPK PK On PK.TaxLot_PK = PT.TaxLot_PK 
INNER JOIN #TempFunds CF ON CF.FundId = PT.FundID
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = PT.Symbol
Inner Join T_Group G With (NoLock) On G.GroupID = PT.GroupID
LEFT OUTER JOIN #MarkPriceForBusinessDate MP_Fund ON 
		(
		MP_Fund.Symbol = PT.Symbol And MP_Fund.FundID = PT.FundID
		)
LEFT OUTER JOIN #ZeroFundMarkPriceBusinessDate MP_ZeroFund ON 
		(
		PT.Symbol = MP_ZeroFund.Symbol And MP_ZeroFund.FundID = 0
		)
LEFT OUTER JOIN #FXConversionRates FXRates_Fund ON (
		FXRates_Fund.FromCurrencyID = G.CurrencyID
		AND FXRates_Fund.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(d, G.AUECLocalDate, FXRates_Fund.DATE) = 0
		AND FXRates_Fund.FundID = PT.FundID
		)
LEFT OUTER JOIN #ZeroFundFxRate FXRates_ZeroFund ON (
		FXRates_ZeroFund.FromCurrencyID = G.CurrencyID
		AND FXRates_ZeroFund.ToCurrencyID = CF.LocalCurrency
		AND DateDiff(d, G.AUECLocalDate, FXRates_ZeroFund.DATE) = 0
		AND FXRates_ZeroFund.FundID = 0
		)
CROSS APPLY (
	SELECT CASE 
			WHEN FXRates_Fund.RateValue IS NULL
				THEN 
					CASE 
						WHEN FXRates_ZeroFund.RateValue IS NULL
						THEN 0
					ELSE FXRates_ZeroFund.RateValue
					END
			ELSE FXRates_Fund.RateValue
			END
	) AS FXRatesForADate(Val)
CROSS APPLY (
	SELECT CASE 
			WHEN MP_Fund.Finalmarkprice IS NULL
				THEN 
					CASE 
						WHEN MP_ZeroFund.Finalmarkprice IS NULL
						THEN 0
					ELSE MP_ZeroFund.Finalmarkprice
					END
			ELSE MP_Fund.Finalmarkprice
			END
	) AS MPForADate(Val)

WHERE PT.TaxlotOpenQty > 0


Select
MV.AccounName As Account,
Sum(MV.MarketValue_Base) As AccountNAV
InTo #TempAccountNAV
From #Temp_SymbolMarketValue MV
Group By AccounName

Insert InTo #TempAccountNAV
Select 
CF.AccounName As Account,
Sum(EODCash.CashValueBase) As AccountNAV
From PM_CompanyFundCashCurrencyValue EODCash With (NoLock)
Inner Join #TempFunds CF On CF.FundId =  EODCash.FundID 
Where DateDiff(Day,EODCash.Date,@CurrentBusinessDate) = 0
Group By EODCash.FundID,CF.AccounName, EODCash.BaseCurrencyID


Select 
Account,
Sum(AccountNAV) As AccountNAV
From #TempAccountNAV
Group By Account
Order By Account


Drop Table #FundIds,#TempFunds,#FXConversionRates,#MarkPriceForBusinessDate,#Temp_TaxlotPK
Drop Table #Temp_SymbolMarketValue, #TempAccountNAV,#ZeroFundFxRate,#ZeroFundMarkPriceBusinessDate