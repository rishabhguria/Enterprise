/*
EXEC P_GenerateCommissionReport_EOD 1,'1,2','2020-05-28',1,'1,2',1,0,12,0
*/

CREATE Procedure [dbo].[P_GenerateCommissionReport_EOD]
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
As

SET NOCOUNT ON;

--Declare @inputDate DateTime
--Set @inputDate = '06-16-2020'

DECLARE @FirstDateofMonth DateTime
Declare @EndDate DateTime

SET @FirstDateofMonth = @inputDate

--Set @StartDate = '04/07/2020'
Set @EndDate = @inputDate

Create Table #FXConversionRates                    
(                    
FromCurrencyID int,                    
ToCurrencyID int,                    
RateValue float,                    
ConversionMethod int,                    
Date DateTime,                    
eSignalSymbol varchar(max),                      
 FundID int                                                                     
)   

--insert FX Rate for date ranges in the temp table                    
Insert into #FXConversionRates Exec P_GetAllFXConversionRatesFundWiseForGivenDateRange @FirstDateofMonth,@inputDate                    
                    
-- Adjusting FxRates based on the conversion method....                    
Update #FXConversionRates                    
Set RateValue = 1.0/RateValue                    
Where RateValue <> 0 and ConversionMethod = 1                    
                    
-- For Fund Zero              
SELECT * INTO #ZeroFundFxRate              
FROM #FXConversionRates              
WHERE fundID = 0 

Declare @CommissionRateMultiplier float
Set @CommissionRateMultiplier = 10000

Select 
MF.MasterFundName As Client,
Convert(varchar,VT.AUECLocalDate, 101) As TradeDate,
convert(varchar,VT.SettlementDate,101) As SettlementDate,
S.Side As TransactionType,
(VT.TaxLotQty) As Quantity,
SM.BloombergSymbol As Symbol,
(VT.AvgPrice) As Price,
TradeCurr.CurrencySymbol As Crcy,
--(IsNull(VT.AvgPrice * VT.TaxLotQty * [dbo].GetSideMultiplier(VT.OrderSideTagValue) * SM.Multiplier ,0)) As GrossMoney,
(IsNull(VT.AvgPrice * VT.TaxLotQty * SM.Multiplier ,0)) As GrossMoney,
CP.ShortName As BrokerCode,
A.AssetName As UserAssetClass, -- Asset Class
CP.FullName As BrokerName,--:Broker Full name
VT.Commission As HardCommission,
VT.SoftCommission As SoftCommission,
VT.OtherBrokerFees As MerakiCommission,--Other Broker Fee
VT.IsSwapped As IsSwap,
BaseCurr.CurrencySymbol As BaseCurrency,
Case 
When VT.FXRate_Taxlot Is Not Null And VT.FXRate_Taxlot > 0
Then 
	Case 
		When IsNull(VT.FXConversionMethodOperator_Taxlot,'M') = 'M'
		Then VT.FXRate_Taxlot
		When VT.FXConversionMethodOperator_Taxlot = 'D' And VT.FXRate_Taxlot > 0
		Then 1 / VT.FXRate_Taxlot
		Else 0 
	End 
When VT.FXRate Is Not Null And VT.FXRate > 0
Then 
	Case 
		When IsNull(VT.FXConversionMethodOperator,'M') = 'M'
		Then VT.FXRate
		When VT.FXConversionMethodOperator = 'D' And VT.FXRate > 0
		Then 1 / VT.FXRate
		Else 0 
	End 
Else IsNull(FXRatesForTradeDate.Val,0) 
End As FXRate,
U.UnderLyingName

InTo #TempCommissionReportBeforeGrouping	
From V_TaxLots VT With (NoLock)
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = VT.Symbol 
Inner Join T_Side S On S.SideTagValue = VT.OrderSideTagValue
inner Join T_Currency TradeCurr On TradeCurr.CurrencyID = VT.CurrencyID
Inner Join T_Asset A On A.AssetID = VT.AssetID
Inner Join T_CounterParty CP On CP.CounterPartyID = VT.CounterPartyID
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = VT.FundID 
Inner Join T_CompanyMasterFundSubAccountAssociation MFA On MFA.CompanyFundID = CF.CompanyFundID
Inner Join T_CompanyMasterFunds MF On MF.CompanyMasterFundID = MFA.CompanyMasterFundID
Inner Join T_Currency BaseCurr On BaseCurr.CurrencyID = CF.LocalCurrency
Inner Join T_UnderLying U On U.UnderLyingID = VT.UnderLyingID
-- Forex Price for Trade Date                    
 LEFT OUTER JOIN #FXConversionRates FXDayRatesForTradeDate ON (                
   FXDayRatesForTradeDate.FromCurrencyID = VT.CurrencyID                
AND FXDayRatesForTradeDate.ToCurrencyID = CF.LocalCurrency                                                                                                             
   AND DateDiff(d, VT.ProcessDate, FXDayRatesForTradeDate.DATE) = 0                
   AND FXDayRatesForTradeDate.FundID = VT.FundID              
    ) 
	 LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRateTradeDate ON (              
   ZeroFundFxRateTradeDate.FromCurrencyID = VT.CurrencyID               
   AND ZeroFundFxRateTradeDate.ToCurrencyID = CF.LocalCurrency              
   AND DateDiff(d, VT.ProcessDate, ZeroFundFxRateTradeDate.DATE) = 0              
   AND ZeroFundFxRateTradeDate.FundID = 0              
   )
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
Where DateDiff(Day,@FirstDateofMonth,VT.AUECLocalDate) >= 0
And DateDiff(Day,VT.AUECLocalDate,@EndDate) >= 0
And CP.ShortName <> 'Undefined'


Alter Table #TempCommissionReportBeforeGrouping
Add HardCommission_Base Decimal(18,4),
SoftCommission_Base Decimal(18,4),
MerakiCommission_Base Decimal(18,4)

Update #TempCommissionReportBeforeGrouping
Set HardCommission_Base = 0,
SoftCommission_Base = 0,
MerakiCommission_Base = 0

Update #TempCommissionReportBeforeGrouping
Set 
HardCommission_Base = 
	Case
		When Crcy <> BaseCurrency
		Then HardCommission * FXRate
		Else HardCommission
	End ,
SoftCommission_Base = 
	Case
		When Crcy <> BaseCurrency
		Then SoftCommission * FXRate
		Else SoftCommission
	End ,
MerakiCommission_Base = 
	Case
		When Crcy <> BaseCurrency
		Then MerakiCommission * FXRate
		Else MerakiCommission
	End

----Select * From #TempCommissionReportBeforeGrouping
----Where Crcy <> 'USD'

Select 
Client,
TradeDate,
SettlementDate,
TransactionType,
Sum(Quantity) As Quantity,
Symbol,
Sum(Price * Quantity) As Price,
--Avg(Price) As Price,
Crcy,
Sum(GrossMoney) As GrossMoney,
BrokerCode,
UserAssetClass, -- Asset Class
BrokerName,--:Broker Full name
Sum(HardCommission) As HardCommission,
Sum(SoftCommission) As SoftCommission,
Sum(MerakiCommission) As MerakiCommission,--Other Broker Fee
IsSwap,
BaseCurrency,
Sum(HardCommission_Base) As HardCommission_Base,
Sum(SoftCommission_Base) As SoftCommission_Base,
Sum(MerakiCommission_Base) As MerakiCommission_Base,
UnderLyingName
InTo #TempCommissionReport
From #TempCommissionReportBeforeGrouping
Where UserAssetClass <> 'Future'
Group By Client,TradeDate,SettlementDate,TransactionType,Symbol,
Crcy,BrokerCode,UserAssetClass,BrokerName,BaseCurrency,IsSwap,UnderLyingName

---- For Future asset category, grouping is not required
Insert InTo #TempCommissionReport
Select 
Client,
TradeDate,
SettlementDate,
TransactionType,
(Quantity) As Quantity,
Symbol,
(Price * Quantity) As Price,
Crcy,
(GrossMoney) As GrossMoney,
BrokerCode,
UserAssetClass, -- Asset Class
BrokerName,--:Broker Full name
(HardCommission) As HardCommission,
(SoftCommission) As SoftCommission,
(MerakiCommission) As MerakiCommission,--Other Broker Fee
IsSwap,
BaseCurrency,
(HardCommission_Base) As HardCommission_Base,
(SoftCommission_Base) As SoftCommission_Base,
(MerakiCommission_Base) As MerakiCommission_Base,
UnderLyingName
From #TempCommissionReportBeforeGrouping
Where UserAssetClass = 'Future'
--Group By Client,TradeDate,SettlementDate,TransactionType,
--Crcy,BrokerCode,UserAssetClass,BrokerName,BaseCurrency,IsSwap

Alter Table #TempCommissionReport
Add BrokerCommRate Decimal(18,4),
MerakiCommRate Decimal(18,4)

-- Commission + Soft per share for US and for Intl Per Gross Notional
--: Other Broker Fee per share for US and for Intl Per Gross Notional
Update #TempCommissionReport
Set BrokerCommRate = 0,
MerakiCommRate = 0

Update #TempCommissionReport
Set 
BrokerCommRate = 
	Case
	When UnderLyingName = 'US'
	Then 
		Case 
			When Quantity > 0
			Then (HardCommission + SoftCommission)/Quantity
		Else 0
		End
	Else 
		Case 
			When Abs(GrossMoney) > 0
			Then ((HardCommission + SoftCommission) * @CommissionRateMultiplier)/Abs(GrossMoney)
		Else 0
		End
	End ,
MerakiCommRate = 
	Case
	When UnderLyingName = 'US'
	Then 
		Case 
			When Quantity > 0
			Then (MerakiCommission)/Quantity
		Else 0
		End
	Else 
		Case 
			When Abs(GrossMoney) > 0
			Then ((MerakiCommission) * @CommissionRateMultiplier)/Abs(GrossMoney)
		Else 0
		End
	End,
	Price =Price/Quantity


Select 
Client,
TradeDate,
SettlementDate ,
TransactionType,
Quantity,
Symbol,
Price,
Crcy,
GrossMoney,
BrokerCode,
BrokerCommRate,
MerakiCommRate,
IsSwap,
UserAssetClass,
BrokerName,
HardCommission,
SoftCommission,
MerakiCommission,
BaseCurrency,
HardCommission_Base,
SoftCommission_Base,
MerakiCommission_Base,
UnderLyingName
From #TempCommissionReport
----Where BrokerCode <> 'Undefined'
----And Crcy <> 'USD'
Order By TradeDate,Client,BrokerCode,Symbol

Drop Table #TempCommissionReport,#TempCommissionReportBeforeGrouping, #FXConversionRates, #ZeroFundFxRate