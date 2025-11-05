/*
EXEC P_GetMarketValAndAccountNAVForPMEOD_EOD '05-29-2020','20,21,22'
https://jira.nirvanasolutions.com:8443/browse/ONB-642
*/

CREATE Proc [dbo].[P_GetMarketValAndAccountNAVForPMEOD_EOD]
(
	@inputDate DateTime,
	@companyFundIDs VARCHAR(1000)
)
As 

SET NOCOUNT ON;
--Declare @inputDate DateTime
--Declare @companyFundIDs VARCHAR(500)
--Set @inputDate = '05-29-2020'
--Set @companyFundIDs = '20,21,22'

DECLARE @Fund TABLE (FundID INT)

INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',')

SELECT PT.Taxlot_PK  
InTo #TempTaxlotPK       
FROM PM_Taxlots PT With (NoLock)       
Inner Join @Fund Fund on Fund.FundID = PT.FundID            
Where PT.Taxlot_PK in                                     
(                                                                                              
 Select Max(Taxlot_PK) from PM_Taxlots  With (NoLock)                                                                                
 Where DateDiff(d, PM_Taxlots.AUECModifiedDate,@inputDate) >= 0                                                                    
 group by TaxlotId                                                                    
)                                                                                        
And PT.TaxLotOpenQty > 0 

Create Table #TempAccountNAV
(
Account Varchar(200),
Symbol Varchar(200),
AccountNAV Float,
ClosingMark Float,
PxLast Float,
FxRate Float,
PxSelectedFeedLocal Float
)

Insert Into #TempAccountNAV
Select
Account,
Symbol,
Case 
	When ISNUMERIC([NAV (Touch)]) = 1
	Then Cast([NAV (Touch)] AS FLOAT)
	Else 0
End AS [AccountNAV],
Case 
	When ISNUMERIC([Closing Mark]) = 1
	Then Cast([Closing Mark] AS FLOAT)
	Else 0
End AS ClosingMark,
Case 
	When ISNUMERIC([Px Last]) = 1
	Then Cast([Px Last] AS FLOAT)
	Else 0
End AS PxLast,
Case 
	When ISNUMERIC([FxRate]) = 1
	Then Cast([FxRate] AS FLOAT)
	Else 0
End AS FxRate,
Case 
	When ISNUMERIC([Px Selected Feed (Local)]) = 1
	Then Cast([Px Selected Feed (Local)] AS FLOAT)
	Else 0
End AS PxSelectedFeedLocal
From T_PMDataDump
Where CreatedOn = (Select Max(CreatedOn) From T_PMDataDump)
And Account <> '' And Account <> 'Undefined'

Update #TempAccountNAV
Set PxLast = PxSelectedFeedLocal
Where PxLast = 0 And PxSelectedFeedLocal <> 0

--Select * From #TempAccountNAV
--Where Symbol = '0257-HKG'

---- Get Open positions and Market value
Select 
Funds.FundName as Fund,                                                                                                                    
PT.Symbol as Symbol ,  
Asset.AssetName As Asset,
 G.IsSwapped,
 Sum(PT.TaxLotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue)) As OpenPositions,
 Sum(PT.TaxLotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue) * SM.Multiplier * T2.PxLast) As MarketValue_Local,
 Sum(
	 Case
		 When G.CurrencyID <> Funds.LocalCurrency
		 Then (PT.TaxLotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue) * SM.Multiplier * T2.PxLast) * IsNull(T2.FxRate,0)
		 Else (PT.TaxLotOpenQty * dbo.GetSideMultiplier(PT.OrderSideTagValue) * SM.Multiplier * T2.PxLast)
	 End
 ) As MarketValueBase , 
--Sum((PT.AvgPrice * TaxlotOpenQty * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) + IsNull(PT.OpenTotalCommissionAndFees,0) ) As TotalCost_Local,
Sum((PT.AvgPrice * TaxlotOpenQty * SM.Multiplier * dbo.GetSideMultiplier(PT.OrderSideTagValue)) ) As TotalCost_Local,
Max(IsNull(T2.FxRate,0)) As FXEndDate                                                                       
InTo #TempMarketValue                        
from PM_Taxlots PT With (NoLock)
Inner Join #TempTaxlotPK Temp On Temp.Taxlot_PK = PT.Taxlot_PK            
Inner join  T_Group G With (NoLock) on G.GroupID=PT.GroupID                                        
Inner join T_Currency TC on TC.CurrencyID = G.CurrencyID      
inner join T_Asset Asset on Asset.AssetId= G.AssetID                             
inner join T_Companyfunds Funds With (NoLock) on Funds.Companyfundid = PT.FundID                 
Inner JOIN V_SecMasterData AS SM ON SM.TickerSymbol = PT.Symbol  
Inner Join #TempAccountNAV T2 On T2.Account = Funds.FundName And T2.Symbol = PT.Symbol
--Where PT.Symbol = '0257-HKG'    
Group By Funds.FundName, PT.Symbol,Asset.AssetName,IsSwapped                
Order By  Funds.FundName, PT.Symbol

---- Merge Account NAV and Market value in a singh table
Select 
T1.Fund As Account,
T1.Symbol,
--Case
--	When T1.IsSwapped = 1
--	Then (T1.MarketValue_Local - T1.TotalCost_Local) * T2.FxRate
--	Else T1.MarketValueBase
--End As MarketValueBase,
T1.MarketValueBase As MarketValueBase,
T1.Asset,
T1.IsSwapped,
T1.OpenPositions,
T2.AccountNAV,
T2.ClosingMark,
T2.PxLast,
T2.FxRate
InTo #TempMarketValueAndNAV
From #TempMarketValue T1
Inner Join #TempAccountNAV T2 On T2.Account = T1.Fund And T2.Symbol  =T1.Symbol
--And T1.Symbol = '0257-HKG'

Select * From #TempMarketValueAndNAV

Drop Table #TempTaxlotPK, #TempAccountNAV, #TempMarketValue, #TempMarketValueAndNAV
GO


