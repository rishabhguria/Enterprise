/*
Author: <Sandeep Singh>
Create date: <25 March 2020>
Description: <Fetch Trades of a date and attributes>
https://jira.nirvanasolutions.com:8443/browse/ONB-642
EXEC P_ThirdParty_Meraki_PMEOD 5, '20,21,22', '06-01-2020', 2, '',0,0,1,
*/

CREATE PROCEDURE [dbo].[P_ThirdParty_Meraki_PMEOD] 
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
	
SET NOCOUNT ON;

--Declare @inputDate DateTime
--Declare @companyFundIDs VARCHAR(500)
--Set @inputDate = '06-01-2020'
--Set @companyFundIDs = '20,21,22'

Declare @PreviousBusinessDate DateTime
Set @PreviousBusinessDate = dbo.AdjustBusinessDays(@inputDate,-1,1) 


DECLARE @Fund TABLE (FundID INT)

INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',') 

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
Insert into #FXConversionRates Exec P_GetAllFXConversionRatesFundWiseForGivenDateRange @PreviousBusinessDate,@inputDate                  
                    
-- Adjusting FxRates based on the conversion method....                    
Update #FXConversionRates                    
Set RateValue = 1.0/RateValue                    
Where RateValue <> 0 and ConversionMethod = 1                    
                    
-- For Fund Zero              
SELECT * INTO #ZeroFundFxRate              
FROM #FXConversionRates              
WHERE fundID = 0 

Create Table #TempParentDetails
(
Symbol Varchar(200),
ParentClOrderID Varchar(200),
ParentOrderQty Float,
ExecutedQty Float,
RemainingQty Float,
Sub_StagedOrderID Varchar(200),
Staged_MsgType Varchar(20)
)

Insert InTo #TempParentDetails
EXEC P_GetParentOrderQtyForPMEOD_EOD @InputDate

Create Table #TempMarketValueAndNAV
(
Account Varchar(200),
Symbol Varchar(200),
MarketValueBase Float,
Asset Varchar(100),
IsSwapped Bit,
OpenPositions Float,
AccountNAV Float,
ClosingMark Float,
PxLast Float,
FxRate Float
)

Insert InTo #TempMarketValueAndNAV
EXEC P_GetMarketValAndAccountNAVForPMEOD_EOD @InputDate, @companyFundIDs

---- Calculate account percentage based on Open positions
Select Symbol, IsSwapped ,Sum(OpenPositions) As OpenPositions 
InTo #TempOpenQtyGroupBySymbol 
From #TempMarketValueAndNAV
--Where Symbol = '2590-TSE'
Group By Symbol, IsSwapped

Select Account, Symbol,
IsSwapped ,
Sum(OpenPositions) As OpenPositions 
Into #TempOpenQtyGroupByAccountSymbol 
From #TempMarketValueAndNAV
--Where Symbol = '2590-TSE'
Group By Account, Symbol, IsSwapped

Alter Table #TempOpenQtyGroupByAccountSymbol
Add TotalOpenPos Float,
OpenPosAccountPercent Float

Update #TempOpenQtyGroupByAccountSymbol
Set TotalOpenPos = 0, OpenPosAccountPercent = 0

Update T1
Set T1.TotalOpenPos = T.OpenPositions
From #TempOpenQtyGroupByAccountSymbol T1
Inner Join #TempOpenQtyGroupBySymbol T On T.Symbol = T1.Symbol

Update T1
Set T1.OpenPosAccountPercent = (T1.OpenPositions/TotalOpenPos) * 100
From #TempOpenQtyGroupByAccountSymbol T1
Where TotalOpenPos <> 0

--Select * from #TempOpenQtyGroupByAccountSymbol

Select Distinct
Replace(CONVERT(VARCHAR(10), G.ProcessDate, 111),'/','-') As ProcessDate, 
G.OrderSideTagValue,
G.AllocatedQty As [AllocatedQty],
G.Symbol As Symbol,
G.AvgPrice As AvgPrice,
G.InternalComments As Notes,
G.CurrencyID As CurrencyId,
L2.Commission + L2.OtherBrokerFees + L2.StampDuty + L2.TransactionLevy + L2.ClearingFee + L2.TaxOnCommissions + L2.MiscFees + L2.SecFee + L2.OccFee + L2.OrfFee + L2.ClearingBrokerFee + L2.SoftCommission + L2.OptionPremiumAdjustment AS TotalExpenses,
Case 
When L2.FXRate Is Not Null And L2.FXRate > 0
Then 
	Case 
		When IsNull(L2.FXConversionMethodOperator,'M') = 'M'
		Then L2.FXRate
		When L2.FXConversionMethodOperator = 'D' And L2.FXRate > 0
		Then 1 / L2.FXRate
		Else 0 
	End 
When G.FXRate Is Not Null And G.FXRate > 0
Then 
	Case 
		When IsNull(G.FXConversionMethodOperator,'M') = 'M'
		Then G.FXRate
		When G.FXConversionMethodOperator = 'D' And G.FXRate > 0
		Then 1 / G.FXRate
		Else 0 
	End 
Else 0  
End As FXRate,
L2.TaxLotQty,
G.Quantity, 
L1.Percentage,
L1.FundID,
G.GroupID,
L2.TaxLotID,
G.TransactionSource,
G.OriginalAllocationPreferenceID
,PTTDetails.EndingPercentage 
,Staged.RemainingQty As RemainingQty
,Staged.ParentOrderQty As BlotterTotalQty
,Staged.ExecutedQty As BlotterExecutedQty
,G.InternalComments
,G.IsSwapped 
InTo #VT
From T_Group G With (NoLock)
Inner Join T_FundAllocation L1 With (NoLock) On L1.GroupID = G.GroupID
Inner Join T_Level2Allocation L2 With (NoLock) On L2.Level1AllocationID = L1.AllocationId
Inner Join @Fund FID On FID.FundID = L1.FundID 
Inner Join T_TradedOrders TOr With (NoLock) On TOr.GroupID = G.GroupID 
Inner Join T_Sub Sub With (NoLock) On Sub.ClOrderID = Tor.CLOrderID
--Inner Join T_Sub Staged With (NoLock) On Staged.CLOrderID = Sub.StagedOrderID
Inner Join #TempParentDetails Staged On Staged.ParentClOrderID = Sub.StagedOrderID 
Left Outer Join T_PTTDefinition PTTDef With (NoLock) On PTTDef.PTTId = G.OriginalAllocationPreferenceID And PTTDef.Symbol = G.Symbol 
Left Outer Join T_PTTDetails PTTDetails With (NoLock) On PTTDetails.PTTId = PTTDef.PTTId And PTTDetails.AccountId = L1.FundID  
Where DateDiff(Day,G.AUECLocalDate,@inputDate) = 0
And L2.TaxlotQty > 0

--Select * From #VT
--Where Symbol = '0257-HKG'

Select 
Replace(CONVERT(VARCHAR(10), VT.ProcessDate, 111),'/','-') As TradeDate, 
CF.FundName As AccountName,
S.Side,
VT.AllocatedQty As [AllocatedQty],
SM.CompanyName As [SecurityDescription],
VT.Symbol As Symbol,
SM.BloombergSymbol As BBGSymbol,
VT.AvgPrice As AvgPrice,
AccountNAV,
MarketValueBase,
T.OpenPositions,
ClosingMark,
PxLast,
VT.InternalComments As Notes,
TradedCurrency.CurrencySymbol As TradeCurrency,
BaseCurr.CurrencySymbol As BaseCurrency,
VT.TotalExpenses,
Case
	When VT.FXRate > 0
	Then VT.FXRate
	Else IsNull(FXRatesForTradeDate.Val,0) 
End As FXRate,
IsNull(VT.AvgPrice * VT.TaxLotQty * [dbo].GetSideMultiplier(VT.OrderSideTagValue) * SM.Multiplier ,0) As TradedQtyGrossNotional,
 Case
	When VT.Percentage = 100
	Then IsNull((T.PxLast * ((VT.RemainingQty * [dbo].GetSideMultiplier(VT.OrderSideTagValue)) + T.OpenPositions) * SM.Multiplier) ,0) 
	Else IsNull((T.PxLast * (((VT.RemainingQty * (T2.OpenPosAccountPercent/100) * [dbo].GetSideMultiplier(VT.OrderSideTagValue))) + T.OpenPositions) * SM.Multiplier),0) 
End As TargetQtyGrossNotional,
Case 
	When AccountNAV <> 0
	Then Round(((MarketValueBase /AccountNAV) * 100),4)
	Else 0
End As [CurrentPercent],
Case ---- 13 means PTT, if allocated by PTT, give prefernece to this one
	When VT.TransactionSource = 13 And EndingPercentage Is Not Null
	Then EndingPercentage
	Else 0.0
End As TargetPercent,
VT.TransactionSource,
VT.TaxLotQty,
VT.RemainingQty,
VT.BlotterTotalQty,
VT.BlotterExecutedQty,
VT.Percentage As FundPercent,
(VT.RemainingQty * (T2.OpenPosAccountPercent/100) ) As RemainingQtyPercent,
T2.OpenPosAccountPercent
InTo #TempPMEOD
From #VT VT
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = VT.FundID 
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = VT.Symbol 
Inner Join T_Side S With (NoLock) On S.SideTagValue = VT.OrderSideTagValue 
Inner Join #TempMarketValueAndNAV T On T.Account = CF.FundName And VT.Symbol = T.Symbol And T.IsSwapped = VT.IsSwapped
Inner Join T_Currency TradedCurrency With (NoLock) On TradedCurrency.CurrencyId = VT.CurrencyID  
Inner Join T_Currency BaseCurr With (NoLock) ON BaseCurr.CurrencyID= CF.LocalCurrency
Inner Join #TempOpenQtyGroupByAccountSymbol T2 On T2.Account =  CF.FundName And VT.Symbol = T2.Symbol And VT.IsSwapped =  T2.IsSwapped 
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
    ---- Previous business date FX Rate
   LEFT OUTER JOIN #FXConversionRates FXDayRatesForPreviousToTradeDate ON (                
   FXDayRatesForPreviousToTradeDate.FromCurrencyID = VT.CurrencyID                
   AND FXDayRatesForPreviousToTradeDate.ToCurrencyID = CF.LocalCurrency                                                                                                             
   AND DateDiff(d, @PreviousBusinessDate, FXDayRatesForPreviousToTradeDate.DATE) = 0                
   AND FXDayRatesForPreviousToTradeDate.FundID = VT.FundID              
    ) 
	LEFT OUTER JOIN #ZeroFundFxRate ZeroFundFxRatePreviousToTradeDate ON (              
   ZeroFundFxRatePreviousToTradeDate.FromCurrencyID = VT.CurrencyID               
   AND ZeroFundFxRatePreviousToTradeDate.ToCurrencyID = CF.LocalCurrency              
   AND DateDiff(d, @PreviousBusinessDate, ZeroFundFxRatePreviousToTradeDate.DATE) = 0              
   AND ZeroFundFxRatePreviousToTradeDate.FundID = 0              
   )
    CROSS APPLY (              
  SELECT 
	CASE               
	WHEN FXDayRatesForTradeDate.RateValue IS NULL              
	THEN 
		CASE               
			WHEN ZeroFundFxRateTradeDate.RateValue IS NULL              
			THEN  
				Case
				When FXDayRatesForPreviousToTradeDate.RateValue IS NULL
				Then
					Case
						When ZeroFundFxRatePreviousToTradeDate.RateValue IS NULL
						Then 0
					Else ZeroFundFxRatePreviousToTradeDate.RateValue 
					End
				Else FXDayRatesForPreviousToTradeDate.RateValue
				End             
		ELSE ZeroFundFxRateTradeDate.RateValue              
		END              
	ELSE FXDayRatesForTradeDate.RateValue              
	END              
  ) AS FXRatesForTradeDate(Val)

Where DateDiff(Day,VT.ProcessDate,@inputDate) = 0
And VT.TaxlotQty > 0
Order By T.Account, Symbol 

 Update #TempPMEOD                    
Set TradedQtyGrossNotional =                    
	Case                    
		When TradeCurrency <> BaseCurrency                    
		Then FXRate * TradedQtyGrossNotional                    
		ELSE TradedQtyGrossNotional
	End,
TargetQtyGrossNotional =                    
	Case                    
		When TradeCurrency <> BaseCurrency                    
		Then FXRate * TargetQtyGrossNotional                    
		ELSE TargetQtyGrossNotional
	End

Alter Table #TempPMEOD  
Add TradedPercent Float

  Update #TempPMEOD
  Set TradedPercent = 0 

   Update #TempPMEOD
   Set TradedPercent = 	
Case 
	When AccountNAV <> 0
	Then Round(((TradedQtyGrossNotional /AccountNAV) * 100),4)
	Else 0
End ,
TargetPercent = 
 Case 
	When TransactionSource <> 13 And TargetPercent = 0 And AccountNAV <> 0
	Then Round(((TargetQtyGrossNotional /AccountNAV) * 100),4)
	When TargetPercent = 0 And AccountNAV = 0
	Then 0
	Else TargetPercent
End 

Select * from #TempPMEOD
----Where Symbol  = '2590-TSE'

Drop Table #TempMarketValueAndNAV,#TempPMEOD,#FXConversionRates,#ZeroFundFxRate,#VT
Drop Table #TempParentDetails, #TempOpenQtyGroupBySymbol, #TempOpenQtyGroupByAccountSymbol

GO


