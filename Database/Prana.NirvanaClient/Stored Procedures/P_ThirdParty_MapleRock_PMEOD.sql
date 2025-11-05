
/*
Author: <Sandeep Singh>
Create date: <30 April 2020>
Description: <Fetch Trades of a date and attributes. Here NAV is considered as Master Fund level.
Also if input date FX rate is missing, in this case, previous business date will be considered>
EXEC P_ThirdParty_MapleRock_PMEOD 5, '1213,1238,1266', '04-27-2020', 2, '',0,0,1,0
*/

CREATE PROCEDURE [dbo].[P_ThirdParty_MapleRock_PMEOD] 
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
--Set @inputDate = '04-27-2020'
--Set @companyFundIDs = '1213,1238,1266'

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

Create Table #TempMarketValueAndNAV
(
MasterFund Varchar(200),
Account Varchar(200),
Symbol Varchar(200),
AccountNAV Float,
OpenPositions Float,
MarketValueBase Float
)

Create Table #TempAccountwiseNAV
(
Account Varchar(200),
AccountNAV Float,
MasterFund Varchar(200)
)

Create Table #TempMasterFundwiseNAV
(
MasterFund Varchar(200),
MasterFundNAV Float
)

Insert Into #TempMarketValueAndNAV
Select
MF.MasterFundName As MasterFund,
DDump.Account,
DDump.Symbol,
Case 
	When ISNUMERIC(DDump.[NAV (Touch)]) = 1
	Then Cast(DDump.[NAV (Touch)] AS FLOAT)
	Else 0
End AS [AccountNAV],
Case 
	When ISNUMERIC(DDump.[Position]) = 1
	Then Cast(DDump.Position AS FLOAT)
	Else 0
End AS [OpenPositions],
Case 
	When ISNUMERIC(DDump.[Market Value (Base)]) = 1
	Then Cast(DDump.[Market Value (Base)] AS FLOAT)
	Else 0
End AS MarketValueBase
From T_PMDataDump DDump With (NoLock)
Inner Join T_CompanyFunds CF With (NoLock) On CF.FundName = DDump.Account 
Inner Join T_CompanyMasterFundSubAccountAssociation MFA On MFA.CompanyFundID = CF.CompanyFundID
Inner Join T_CompanyMasterFunds MF On MF.CompanyMasterFundID = MFA.CompanyMasterFundID
Where CreatedOn = (Select Max(CreatedOn) From T_PMDataDump)
And Account <> '' And Account <> 'Undefined'

---- Account wise NAV
Insert InTo #TempAccountwiseNAV(Account,AccountNAV,MasterFund)
Select Distinct 
Account,
AccountNAV,
MasterFund
From #TempMarketValueAndNAV

-- Master Fundwise NAV
Insert InTo #TempMasterFundwiseNAV(MasterFund,MasterFundNAV )
Select 
MasterFund, 
Sum(AccountNAV) As MasterFundNAV
From #TempAccountwiseNAV
Group By MasterFund

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
,Staged.Quantity As TargetQuantity
,G.InternalComments 
InTo #VT
From T_Group G With (NoLock)
Inner Join T_FundAllocation L1 With (NoLock) On L1.GroupID = G.GroupID
Inner Join T_Level2Allocation L2 With (NoLock) On L2.Level1AllocationID = L1.AllocationId
Inner Join @Fund FID On FID.FundID = L1.FundID 
Inner Join T_TradedOrders TOr With (NoLock) On TOr.GroupID = G.GroupID 
Inner Join T_Sub Sub With (NoLock) On Sub.ClOrderID = Tor.CLOrderID
Inner Join T_Sub Staged With (NoLock) On Staged.CLOrderID = Sub.StagedOrderID 
Left Outer Join T_PTTDefinition PTTDef With (NoLock) On PTTDef.PTTId = G.OriginalAllocationPreferenceID And PTTDef.Symbol = G.Symbol 
Left Outer Join T_PTTDetails PTTDetails With (NoLock) On PTTDetails.PTTId = PTTDef.PTTId And PTTDetails.AccountId = L1.FundID  
Where DateDiff(Day,G.AUECLocalDate,@inputDate) = 0
And L2.TaxlotQty > 0


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
MFNAV.MasterFundNAV,
MarketValueBase,
OpenPositions,
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
	When VT.TargetQuantity = VT.AllocatedQty  
	Then IsNull(VT.AvgPrice * VT.TaxLotQty * [dbo].GetSideMultiplier(VT.OrderSideTagValue) * SM.Multiplier ,0)
	Else IsNull((VT.AvgPrice * VT.TargetQuantity * [dbo].GetSideMultiplier(VT.OrderSideTagValue) * SM.Multiplier * VT.Percentage) / 100,0) 
End As TargetQtyGrossNotional,
Case 
	When MFNAV.MasterFundNAV <> 0
	Then Round(((MarketValueBase /MFNAV.MasterFundNAV) * 100),4)
	Else 0
End As [CurrentPercent],
Case
	When VT.TransactionSource = 13 And EndingPercentage Is Not Null
	Then EndingPercentage
	Else 0.0
End As TargetPercent,
VT.TransactionSource,
VT.TaxLotQty

InTo #TempPMEOD
From #VT VT
Inner Join T_CompanyFunds CF With (NoLock) On CF.CompanyFundID = VT.FundID 
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = VT.Symbol 
Inner Join T_Side S With (NoLock) On S.SideTagValue = VT.OrderSideTagValue 
Inner Join #TempMarketValueAndNAV T On T.Account = CF.FundName And VT.Symbol = T.Symbol
Inner Join #TempMasterFundwiseNAV MFNAV On MFNAV.MasterFund = T.MasterFund
Inner Join T_Currency TradedCurrency With (NoLock) On TradedCurrency.CurrencyId = VT.CurrencyID  
Inner Join T_Currency BaseCurr With (NoLock) ON BaseCurr.CurrencyID= CF.LocalCurrency  
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
Order By Account, Symbol 

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
Add 
  TradedPercent Float

  Update #TempPMEOD
  Set   
  TradedPercent = 0 

   Update #TempPMEOD
   Set 
   TradedPercent = 
	
Case 
	When MasterFundNAV <> 0
	Then Round(((TradedQtyGrossNotional /MasterFundNAV) * 100),4)
	Else 0
End ,
TargetPercent = 
 Case 
	When TargetPercent = 0 And MasterFundNAV <> 0
	Then Round(((TargetQtyGrossNotional /MasterFundNAV) * 100),4)
	When TargetPercent = 0 And MasterFundNAV = 0
	Then 0
	Else TargetPercent
End 

Update #TempPMEOD
Set 
TargetPercent = 	
	Case 
		When TransactionSource <> 13
		Then CurrentPercent - TradedPercent + TargetPercent
		Else TargetPercent
	End

Select * from #TempPMEOD

Drop Table #TempMarketValueAndNAV, #TempPMEOD, #TempAccountwiseNAV,#TempMasterFundwiseNAV
Drop Table #FXConversionRates,#ZeroFundFxRate,#VT