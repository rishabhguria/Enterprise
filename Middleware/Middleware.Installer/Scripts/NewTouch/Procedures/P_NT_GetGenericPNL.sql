/****** Object:  StoredProcedure [dbo].[P_NT_GetGenericPNL]    Script Date: 05/13/2015 16:36:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
--Exec P_NT_GetGenericPNL '04/01/2015','05/01/2015'
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetGenericPNL]
-- Add the parameters for the stored procedure here
@FromDate DateTime,
@ToDate DateTime,
@FundIDSymbol xml = Null
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here
Create Table #FundIDSymbol
(FundID int,Fund varchar(Max),Symbol varchar(Max))
Insert Into #FundIDSymbol
(FundID,Fund,Symbol)
Select 
ref.value('FundID[1]','int') As FundID,
ref.value('Fund[1]','varchar(Max)') As Fund,
ref.value('Symbol[1]','varchar(Max)') As Symbol 
From @FundIDSymbol.nodes('/FundIDSymbol') xmlData(ref)

If Not Exists (Select FundID,Fund,Symbol From #FundIDSymbol)
Begin 
	Insert Into #FundIDSymbol 
	(FundID,Fund) 
	Select CompanyFundID,FundName From T_CompanyFunds 
End 

Select Distinct FundID,Fund
Into #Fund From #FundIDSymbol
--------------------------------------------------------------------------------------------------------- 
Declare @DefaultAUECID int
Select @DefaultAUECID = DefaultAUECID From T_Company

Declare @BaseCurrencyID int
Select @BaseCurrencyID= BaseCurrencyID From T_Company

Declare @BaseCurrencyName varchar(5)
Select @BaseCurrencyName = CurrencySymbol From T_currency Where CurrencyID = @BaseCurrencyID
--------------------------------------------------------------------------------------------------------- 
Create Table #SecMasterDataTempTable
(TickerSymbol varchar(Max),UnderlyingDelta float,
SectorName varchar(Max),SubSectorName varchar(Max),CountryName varchar(Max),CompanyName varchar(Max),BloombergSymbol varchar(Max),PutOrCall varchar(Max),ExpirationDate datetime,StrikePrice float)
Insert Into #SecMasterDataTempTable 
(TickerSymbol,UnderlyingDelta,
SectorName,SubSectorName,CountryName,CompanyName,BloombergSymbol,PutOrCall,ExpirationDate,StrikePrice) 
Select TickerSymbol,UnderlyingDelta,
SectorName,SubSectorName,CountryName,CompanyName,BloombergSymbol,PutOrCall,ExpirationDate,StrikePrice From V_SecMasterData 

Create Table #SplitTab 
(Symbol varchar(Max),EffectiveDate datetime,SplitFactor float) 
Insert Into #SplitTab 
(Symbol,EffectiveDate,SplitFactor)
Select Symbol,EffectiveDate,SplitFactor
From V_CorpActionData Where IsApplied = 1 And CorpActionTypeID = 6 

Create Table #CurrentGenericPNL
(AcctId int,AcctName varchar(Max),RunDate datetime,PreviousBusinessDate datetime,
Symbol varchar(Max),UnderlyingSymbol varchar(Max),TradeDate datetime,ClosingDate datetime,Open_CloseTag varchar(Max),
UdaSector varchar(Max),
UdaSubSector varchar(Max),
UdaCountry varchar(Max),
Strategy varchar(Max),
SymbolDescription varchar(Max),
UnderlyingSymbolDescription varchar(Max),
BloombergSymbol varchar(Max),
PutOrCall varchar(Max),
ExpirationDate datetime,
StrikePrice float,
Delta float,
Beta float,
ImpliedVol float,
Asset varchar(Max),SetupAsset varchar(Max),
CommissionAndFees bit,
FXPNL bit,
PriceMultiplier float,
DeltaAdjPosMultiplier bit,
ZeroOrEndingMVOrUnrealized int,
CouponRate bit,
BlackScholesOrBlack76 bit,
Side varchar(Max),
TradeCurrency varchar(Max),
OpeningFXRate float,
BeginningFXRate float,
EndingFXRate float,
TotalCostLocal float,
BeginningMarketValueLocal float,
EndingMarketValueLocal float,
TotalOpenCommissionAndFeesLocal float,
TotalClosedCommissionAndFeesLocal float,
DividendLocal float,
BeginningQuantity float,
EndingQuantity float,
Quantity float,
UnitCostLocal float,
BeginningPriceLocal float,
ClosingPriceLocal float,
EndingPriceLocal float,
UnderlyingSymbolPriceLocal float,
SideMultiplier float,
Multiplier float,
UnderlyingDelta float,
TaxlotID varchar(50),
TaxlotClosingID uniqueidentifier,
DaySplitFactor float,
TillSplitFactor float,
K0 float,
K1 float,
K2 float,
AveDaystoLiquidate float,
DaysToLiquidate float,
AveNDaysTradingVolume float,
AveNDaysTradingValueLocal float)

Insert Into #CurrentGenericPNL
(AcctId,AcctName,RunDate,PreviousBusinessDate,
Symbol,UnderlyingSymbol,TradeDate,ClosingDate,Open_CloseTag,
UdaSector,
UdaSubSector,
UdaCountry,
Strategy,
SymbolDescription,
UnderlyingSymbolDescription,
BloombergSymbol,
PutOrCall,
ExpirationDate,
StrikePrice,
Delta,
Beta,
ImpliedVol,
Asset,SetupAsset,
CommissionAndFees,
FXPNL,
PriceMultiplier,
DeltaAdjPosMultiplier,
ZeroOrEndingMVOrUnrealized,
CouponRate,
BlackScholesOrBlack76,
Side,
TradeCurrency,
OpeningFXRate,
BeginningFXRate,
EndingFXRate,
TotalCostLocal,
BeginningMarketValueLocal,
EndingMarketValueLocal,
TotalOpenCommissionAndFeesLocal,
TotalClosedCommissionAndFeesLocal,
DividendLocal,
BeginningQuantity,
EndingQuantity,
Quantity,
UnitCostLocal,
BeginningPriceLocal,
ClosingPriceLocal,
EndingPriceLocal,
UnderlyingSymbolPriceLocal,
SideMultiplier,
Multiplier,
UnderlyingDelta,
TaxlotID,
TaxlotClosingID,
DaySplitFactor,
TillSplitFactor,
K0,
K1,
K2,
AveDaystoLiquidate,
DaysToLiquidate,
AveNDaysTradingVolume,
AveNDaysTradingValueLocal)
Select FS.FundID As AcctId,FS.Fund As AcctName,RunDate,dbo.AdjustBusinessDays(RunDate,-1,@DefaultAUECID),
A.Symbol,IsNull(UnderlyingSymbol,'Undefined'),TradeDate,ClosingDate,Open_CloseTag,
UdaSector = IsNull(SM.SectorName,'Undefined'),
UdaSubSector = IsNull(SM.SubSectorName,'Undefined'),
UdaCountry = IsNull(SM.CountryName,'Undefined'),
A.Strategy,
SymbolDescription = IsNull(SM.CompanyName,A.Symbol),
UnderlyingSymbolDescription = IsNull(USM.CompanyName,IsNull(UnderlyingSymbol,'Undefined')),
BloombergSymbol = IsNull(SM.BloombergSymbol,A.Symbol),
PutOrCall = IsNull(SM.PutOrCall,''),
ExpirationDate = SM.ExpirationDate,
StrikePrice = IsNull(SM.StrikePrice,IsNull(UnderlyingSymbolPrice,0) * B.PriceMultiplier),
IsNull(A.Delta,1) As Delta,
IsNull(A.Beta,1) As Beta,
A.ImpliedVol,
A.Asset,B.Asset As SetupAsset,
B.CommissionAndFees,
B.FXPNL,
B.PriceMultiplier,
B.DeltaAdjPosMultiplier,
B.ZeroOrEndingMVOrUnrealized,
B.CouponRate,
B.BlackScholesOrBlack76,
Side,
TradeCurrency,
Case When TradeCurrency <> @BaseCurrencyName Then OpeningFXRate Else 1 End As OpeningFXRate,
Case When TradeCurrency <> @BaseCurrencyName Then BeginningFXRate Else 1 End As BeginningFXRate,
Case When TradeCurrency <> @BaseCurrencyName Then EndingFXRate Else 1 End As EndingFXRate,
Case  
When Open_CloseTag = 'O' Then UnitCostLocal * BeginningQuantity * IsNull(Multiplier,1) * IsNull(SideMultiplier,1) * B.PriceMultiplier  
When Open_CloseTag = 'C' Then UnitCostLocal * EndingQuantity * IsNull(Multiplier,1) * IsNull(SideMultiplier,1) * B.PriceMultiplier 
Else 0 End As TotalCostLocal,
Case  
When Open_CloseTag = 'O' Then (Case When DateDiff(d,Rundate,TradeDate) < 0 Then BeginningPriceLocal/DaySplitTab.SplitFactor * BeginningQuantity * IsNull(Multiplier,1) * IsNull(SideMultiplier,1) * B.PriceMultiplier Else 0 End) 
When Open_CloseTag = 'C' Then (Case When DateDiff(d,Rundate,TradeDate) < 0 Then BeginningPriceLocal/DaySplitTab.SplitFactor * EndingQuantity * IsNull(Multiplier,1) * IsNull(SideMultiplier,1) * B.PriceMultiplier Else 0 End)  
Else 0 End As BeginningMarketValueLocal,
Case  
When Open_CloseTag = 'O' Then EndingPriceLocal * BeginningQuantity * IsNull(Multiplier,1) * IsNull(SideMultiplier,1) * B.PriceMultiplier 
When Open_CloseTag = 'C' Then ClosingPriceLocal * EndingQuantity * IsNull(Multiplier,1) * IsNull(SideMultiplier,1) * B.PriceMultiplier 
Else 0 End As EndingMarketValueLocal,
Case 
When Open_CloseTag = 'O' Then TotalOpenCommissionAndFees_Local 
When Open_CloseTag = 'C' Then TotalOpenCommissionAndFees_Local 
Else 0 End As TotalOpenCommissionAndFeesLocal,
Case 
When Open_CloseTag = 'O' Then 0 
When Open_CloseTag = 'C' Then TotalClosedCommissionAndFees_Local 
Else 0 End As TotalClosedCommissionAndFeesLocal,
0 As DividendLocal,
BeginningQuantity,
EndingQuantity,
Case 
When Open_CloseTag = 'O' Then BeginningQuantity 
When Open_CloseTag = 'C' Then EndingQuantity 
Else 0 End As Quantity,
UnitCostLocal * B.PriceMultiplier As UnitCostLocal,
BeginningPriceLocal * B.PriceMultiplier As BeginningPriceLocal,
ClosingPriceLocal * B.PriceMultiplier As EndingPriceLocal,
EndingPriceLocal * B.PriceMultiplier As EndingPriceLocal,
IsNull(UnderlyingSymbolPrice,0) * B.PriceMultiplier As UnderlyingSymbolPriceLocal,
IsNull(SideMultiplier,1) As SideMultiplier,
IsNull(Multiplier,1) As Multiplier,
IsNull(SM.UnderlyingDelta,1) As UnderlyingDelta,
TaxlotID,
TaxlotClosingID,
IsNull(DaySplitTab.SplitFactor,1) As DaySplitFactor,
IsNull(TillSplitTab.SplitFactor,1) As TillSplitFactor,
Case When CouponRate = 1 And (Open_CloseTag = 'O' Or Open_CloseTag = 'C') Then 
(Case FormulaType 
When 0 Then Round((FaceValue * 365)/(365 +(((100 - UnitCostLocal) * DaysToMaturity)/100)),2)
When 1 Then Round((Round(((CouponRates.Coupon/2)* (1-Round(Power(Round(1/(1+(100 - UnitCostLocal)/200),8),CouponPayments),8)))/((100 - UnitCostLocal)/200),8) + 100 * Round(Power(Round(1/(1+(100 - UnitCostLocal)/200),8),CouponPayments),8)) * FaceValue,2)
When 2 Then Round((UnitCostLocal) * IsNull(Multiplier,1),2)
Else Null End) Else Null End As K0, 
Case When CouponRate = 1 And (Open_CloseTag = 'O' Or Open_CloseTag = 'C') Then 
(Case FormulaType 
When 0 Then Round((FaceValue * 365)/(365 +(((100 - BeginningPriceLocal) * DaysToMaturity)/100)),2)
When 1 Then Round((Round(((CouponRates.Coupon/2)* (1-Round(Power(Round(1/(1+(100 - BeginningPriceLocal)/200),8),CouponPayments),8)))/((100 - BeginningPriceLocal)/200),8) + 100 * Round(Power(Round(1/(1+(100 - BeginningPriceLocal)/200),8),CouponPayments),8)) * FaceValue,2)
When 2 Then Round((BeginningPriceLocal) * IsNull(Multiplier,1),2)
Else Null End) Else Null End As K1,
Case When CouponRate = 1 And (Open_CloseTag = 'O' Or Open_CloseTag = 'C') Then 
(Case FormulaType 
When 0 Then Round((FaceValue * 365)/(365 +(((100 - Case When Open_CloseTag = 'O' Then EndingPriceLocal When Open_CloseTag ='C' Then ClosingPriceLocal End) * DaysToMaturity)/100)),2)
When 1 Then Round((Round(((CouponRates.Coupon/2)* (1-Round(Power(Round(1/(1+(100 - Case When Open_CloseTag = 'O' Then EndingPriceLocal When Open_CloseTag ='C' Then ClosingPriceLocal End)/200),8),CouponPayments),8)))/((100 -    
Case When Open_CloseTag = 'O' Then EndingPriceLocal When Open_CloseTag ='C' Then ClosingPriceLocal End)/200),8) + 100 * Round(Power(Round(1/(1+(100 - Case When Open_CloseTag = 'O' Then EndingPriceLocal When Open_CloseTag ='C' Then ClosingPriceLocal End)/200),8),CouponPayments),8)) * FaceValue,2) 
When 2 Then Round((Case When Open_CloseTag = 'O' Then EndingPriceLocal When Open_CloseTag ='C' Then ClosingPriceLocal End) * IsNull(Multiplier,1),2)    
Else Null End) Else Null End As K2,
Abs(IsNull(AverageLiquidation,0)),
3*Abs(IsNull(AverageLiquidation,0)),
IsNull(AverageVolume,0),
IsNull(AverageVolume,0) * EndingPriceLocal * IsNull(Multiplier,1) * B.PriceMultiplier 
From T_MW_GenericPNL A 
Join #FundIDSymbol FS On (FS.Fund = A.Fund) And (FS.Symbol = A.Symbol Or FS.Symbol Is Null) 
Join T_NT_AssetwisePreferences B On 
(Case 
When A.Asset = 'FixedIncome' Then 'FixedIncome' 
When A.Asset = 'Equity' And A.IsSwapped = 0 Then 'Equity' 
When A.Asset in ('FX','FXForward') Then 'FX' 
When A.Asset = 'Future' Then 'Future' 
When A.Asset = 'Equity' And A.IsSwapped = 1 Then 'Swap' 
When A.Asset = 'FutureOption' And A.TradeCurrency <> @BaseCurrencyName Then 'InternationalFutureOption' 
When A.Asset = 'FutureOption' And A.TradeCurrency = @BaseCurrencyName Then 'LocalFutureOption'
Else 'Default' 
End) = B.Asset 
Left Outer Join #SecMasterDataTempTable SM On (A.Symbol = SM.TickerSymbol)
Left Outer Join #SecMasterDataTempTable USM On (A.UnderlyingSymbol = USM.TickerSymbol) 
Outer Apply (Select IsNull(Exp(Sum(Log(SplitFactor))),1) As SplitFactor From #SplitTab Where Symbol = A.Symbol And DateDiff(d,A.Rundate,EffectiveDate) = 0) DaySplitTab 
Outer Apply (Select IsNull(Exp(Sum(Log(SplitFactor))),1) As SplitFactor From #SplitTab Where Symbol = A.Symbol And DateDiff(d,A.Rundate,EffectiveDate) <= 0) TillSplitTab 
Left Outer Join T_MW_CouponRates CouponRates On A.Symbol = CouponRates.Symbol 
Where Not (A.Asset = 'Cash' Or Open_CloseTag = 'D') And (Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0) 

Insert Into #CurrentGenericPNL
(AcctId,AcctName,RunDate,PreviousBusinessDate,
Symbol,UnderlyingSymbol,TradeDate,ClosingDate,Open_CloseTag,
UdaSector,
UdaSubSector,
UdaCountry,
Strategy,
SymbolDescription,
UnderlyingSymbolDescription,
BloombergSymbol,
PutOrCall,
ExpirationDate,
StrikePrice,
Delta,
Beta,
ImpliedVol,
Asset,SetupAsset,
CommissionAndFees,
FXPNL,
PriceMultiplier,
DeltaAdjPosMultiplier,
ZeroOrEndingMVOrUnrealized,
CouponRate,
BlackScholesOrBlack76,
Side,
TradeCurrency,
OpeningFXRate,
BeginningFXRate,
EndingFXRate,
TotalCostLocal,
BeginningMarketValueLocal,
EndingMarketValueLocal,
TotalOpenCommissionAndFeesLocal,
TotalClosedCommissionAndFeesLocal,
DividendLocal,
BeginningQuantity,
EndingQuantity,
Quantity,
UnitCostLocal,
BeginningPriceLocal,
ClosingPriceLocal,
EndingPriceLocal,
UnderlyingSymbolPriceLocal,
SideMultiplier,
Multiplier,
UnderlyingDelta,
TaxlotID,
TaxlotClosingID,
DaySplitFactor,
TillSplitFactor,
K0,
K1,
K2,
AveDaystoLiquidate,
DaysToLiquidate,
AveNDaysTradingVolume,
AveNDaysTradingValueLocal)
Select FS.FundID As AcctId,FS.Fund As AcctName,RunDate,dbo.AdjustBusinessDays(RunDate,-1,@DefaultAUECID),
A.Symbol,IsNull(UnderlyingSymbol,'Undefined'),TradeDate,ClosingDate,Open_CloseTag,
UdaSector = IsNull(SM.SectorName,'Undefined'),
UdaSubSector = IsNull(SM.SubSectorName,'Undefined'),
UdaCountry = IsNull(SM.CountryName,'Undefined'),
A.Strategy,
SymbolDescription = IsNull(SM.CompanyName,A.Symbol),
UnderlyingSymbolDescription = IsNull(USM.CompanyName,IsNull(UnderlyingSymbol,'Undefined')),
BloombergSymbol = IsNull(SM.BloombergSymbol,A.Symbol),
PutOrCall = IsNull(SM.PutOrCall,''),
ExpirationDate = SM.ExpirationDate,
StrikePrice = IsNull(SM.StrikePrice,IsNull(UnderlyingSymbolPrice,0) * B.PriceMultiplier),
1 As Delta,
1 As Beta,
Null As ImpliedVol,
A.Asset,B.Asset As SetupAsset,
B.CommissionAndFees,
B.FXPNL,
B.PriceMultiplier,
B.DeltaAdjPosMultiplier,
B.ZeroOrEndingMVOrUnrealized,
B.CouponRate,
B.BlackScholesOrBlack76,
Side,
TradeCurrency,
Case When TradeCurrency <> @BaseCurrencyName Then OpeningFXRate Else 1 End As OpeningFXRate,
Case When TradeCurrency <> @BaseCurrencyName Then OpeningFXRate Else 1 End As BeginningFXRate,
Case When TradeCurrency <> @BaseCurrencyName Then OpeningFXRate Else 1 End As EndingFXRate,
0 As TotalCostLocal,
0 As BeginningMarketValueLocal,
0 As EndingMarketValueLocal,
0 As TotalOpenCommissionAndFeesLocal,
0 As TotalClosedCommissionAndFeesLocal,
DividendLocal As DividendLocal,
0 As BeginningQuantity,
0 As EndingQuantity,
0 As Quantity,
0 As UnitCostLocal,
0 As BeginningPriceLocal,
0 As EndingPriceLocal,
0 As EndingPriceLocal,
0 As UnderlyingSymbolPriceLocal,
1 As SideMultiplier,
1 As Multiplier,
1 As UnderlyingDelta,
'' As TaxlotID,
Null As TaxlotClosingID,
1 As DaySplitFactor,
1 As TillSplitFactor,
Null As K0, 
Null As K1,
Null As K2,
Null As AveDaystoLiquidate,
Null As DaysToLiquidate,
Null As AveNDaysTradingVolume,
Null As AveNDaysTradingValueLocal
From T_MW_GenericPNL A 
Join #Fund FS On A.Fund = FS.Fund 
Join T_NT_AssetwisePreferences B On 
(Case 
When A.Asset = 'FixedIncome' Then 'FixedIncome' 
When A.Asset = 'Equity' And A.IsSwapped = 0 Then 'Equity' 
When A.Asset in ('FX','FXForward') Then 'FX' 
When A.Asset = 'Future' Then 'Future' 
When A.Asset = 'Equity' And A.IsSwapped = 1 Then 'Swap' 
When A.Asset = 'FutureOption' And A.TradeCurrency <> @BaseCurrencyName Then 'InternationalFutureOption' 
When A.Asset = 'FutureOption' And A.TradeCurrency = @BaseCurrencyName Then 'LocalFutureOption'
Else 'Default' 
End) = B.Asset 
Left Outer Join #SecMasterDataTempTable SM On (A.Symbol = SM.TickerSymbol)
Left Outer Join #SecMasterDataTempTable USM On (A.UnderlyingSymbol = USM.TickerSymbol) 
Where (Open_CloseTag = 'D') And (Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0) 

Create Table #GenericPNL
(AcctId int,AcctName varchar(Max),RunDate datetime,
Symbol varchar(Max),UnderlyingSymbol varchar(Max),TradeDate datetime,ClosingDate datetime,Open_CloseTag varchar(Max),
UdaSector varchar(Max),
UdaSubSector varchar(Max),
UdaCountry varchar(Max),
Strategy varchar(Max),
SymbolDescription varchar(Max),
UnderlyingSymbolDescription varchar(Max),
BloombergSymbol varchar(Max),
PutOrCall varchar(Max),
ExpirationDate datetime,
StrikePrice float,
Delta float,
Beta float,
ImpliedVol float,
Asset varchar(Max),SetupAsset varchar(Max),
CommissionAndFees bit,
FXPNL bit,
PriceMultiplier float,
DeltaAdjPosMultiplier bit,
ZeroOrEndingMVOrUnrealized int,
CouponRate bit,
BlackScholesOrBlack76 bit,
Side varchar(Max),
TradeCurrency varchar(Max),
OpeningFXRate float,
BeginningFXRate float,
EndingFXRate float,
TotalCostLocal float,
BeginningMarketValueLocal float,
EndingMarketValueLocal float,
TotalOpenCommissionAndFeesLocal float,
TotalClosedCommissionAndFeesLocal float,
DividendLocal float,
BeginningQuantity float,
EndingQuantity float,
Quantity float,
UnitCostLocal float,
BeginningPriceLocal float,
ClosingPriceLocal float,
EndingPriceLocal float,
UnderlyingSymbolPriceLocal float,
SideMultiplier float,
Multiplier float,
UnderlyingDelta float,
TaxlotID varchar(50),
TaxlotClosingID uniqueidentifier,
DaySplitFactor float,
TillSplitFactor float,
K0 float,
K1 float,
K2 float,
AveDaystoLiquidate float,
DaysToLiquidate float,
AveNDaysTradingVolume float,
AveNDaysTradingValueLocal float,
BeginningDelta float,
BeginningUnderlyingSymbolPriceLocal float,PreviousBusinessDate datetime)

Insert Into #GenericPNL
(AcctId,AcctName,RunDate,
Symbol,UnderlyingSymbol,TradeDate,ClosingDate,Open_CloseTag,
UdaSector,
UdaSubSector,
UdaCountry,
Strategy,
SymbolDescription,
UnderlyingSymbolDescription,
BloombergSymbol,
PutOrCall,
ExpirationDate,
StrikePrice,
Delta,
Beta,
ImpliedVol,
Asset,SetupAsset,
CommissionAndFees,
FXPNL,
PriceMultiplier,
DeltaAdjPosMultiplier,
ZeroOrEndingMVOrUnrealized,
CouponRate,
BlackScholesOrBlack76,
Side,
TradeCurrency,
OpeningFXRate,
BeginningFXRate,
EndingFXRate,
TotalCostLocal,
BeginningMarketValueLocal,
EndingMarketValueLocal,
TotalOpenCommissionAndFeesLocal,
TotalClosedCommissionAndFeesLocal,
DividendLocal,
BeginningQuantity,
EndingQuantity,
Quantity,
UnitCostLocal,
BeginningPriceLocal,
ClosingPriceLocal,
EndingPriceLocal,
UnderlyingSymbolPriceLocal,
SideMultiplier,
Multiplier,
UnderlyingDelta,
TaxlotID,
TaxlotClosingID,
DaySplitFactor,
TillSplitFactor,
K0,
K1,
K2,
AveDaystoLiquidate,
DaysToLiquidate,
AveNDaysTradingVolume,
AveNDaysTradingValueLocal,
PreviousBusinessDate
--BeginningDelta,
--BeginningUnderlyingSymbolPriceLocal
)
Select 
A.AcctId,A.AcctName,A.RunDate,
A.Symbol,A.UnderlyingSymbol,A.TradeDate,A.ClosingDate,A.Open_CloseTag,
A.UdaSector,
A.UdaSubSector,
A.UdaCountry,
A.Strategy,
A.SymbolDescription,
A.UnderlyingSymbolDescription,
A.BloombergSymbol,
A.PutOrCall,
A.ExpirationDate,
A.StrikePrice,
A.Delta,
A.Beta,
A.ImpliedVol,
A.Asset,A.SetupAsset,
A.CommissionAndFees,
A.FXPNL,
A.PriceMultiplier,
A.DeltaAdjPosMultiplier,
A.ZeroOrEndingMVOrUnrealized,
A.CouponRate,
A.BlackScholesOrBlack76,
A.Side,
A.TradeCurrency,
A.OpeningFXRate,
A.BeginningFXRate,
A.EndingFXRate,
A.TotalCostLocal,
A.BeginningMarketValueLocal,
A.EndingMarketValueLocal,
A.TotalOpenCommissionAndFeesLocal,
A.TotalClosedCommissionAndFeesLocal,
A.DividendLocal,
A.BeginningQuantity,
A.EndingQuantity,
A.Quantity,
A.UnitCostLocal,
A.BeginningPriceLocal,
A.ClosingPriceLocal,
A.EndingPriceLocal,
A.UnderlyingSymbolPriceLocal,
A.SideMultiplier,
A.Multiplier,
A.UnderlyingDelta,
A.TaxlotID,
A.TaxlotClosingID,
A.DaySplitFactor,
A.TillSplitFactor,
A.K0,
A.K1,
A.K2,
A.AveDaystoLiquidate,
A.DaysToLiquidate,
A.AveNDaysTradingVolume,
A.AveNDaysTradingValueLocal,
A.PreviousBusinessDate
From #CurrentGenericPNL A

update A
set BeginningDelta =IsNull(B.Delta,1),
BeginningUnderlyingSymbolPriceLocal =IsNull(B.UnderlyingSymbolPrice,0) * A.PriceMultiplier 
From #GenericPNL  A Left Outer 
Join T_MW_GenericPNL B On 
B.Open_CloseTag = 'O' And A.PreviousBusinessDate = B.RunDate And A.TaxlotID = B.TaxlotID

Select AcctId,AcctName,RunDate,
Symbol,UnderlyingSymbol,TradeDate,ClosingDate,Open_CloseTag,
UdaSector,
UdaSubSector,
UdaCountry,
Strategy,
SymbolDescription,
UnderlyingSymbolDescription,
BloombergSymbol,
PutOrCall,
ExpirationDate,
StrikePrice,
Delta,
Beta,
ImpliedVol,
Asset,SetupAsset,
CommissionAndFees,
FXPNL,
PriceMultiplier,
DeltaAdjPosMultiplier,
ZeroOrEndingMVOrUnrealized,
CouponRate,
BlackScholesOrBlack76,
Side,
TradeCurrency,
OpeningFXRate,
BeginningFXRate,
EndingFXRate,
TotalCostLocal,
BeginningMarketValueLocal,
EndingMarketValueLocal,
TotalOpenCommissionAndFeesLocal,
TotalClosedCommissionAndFeesLocal,
DividendLocal,
BeginningQuantity,
EndingQuantity,
Quantity,
UnitCostLocal,
BeginningPriceLocal,
ClosingPriceLocal,
EndingPriceLocal,
UnderlyingSymbolPriceLocal,
SideMultiplier,
Multiplier,
UnderlyingDelta,
TaxlotID,
TaxlotClosingID,
DaySplitFactor,
TillSplitFactor,
K0,
K1,
K2,
AveDaystoLiquidate,
DaysToLiquidate,
AveNDaysTradingVolume,
AveNDaysTradingValueLocal,
BeginningDelta,
BeginningUnderlyingSymbolPriceLocal 
From #GenericPNL

Drop Table #CurrentGenericPNL,#GenericPNL,#SecMasterDataTempTable,#SplitTab
Drop Table #FundIDSymbol,#Fund

END


GO