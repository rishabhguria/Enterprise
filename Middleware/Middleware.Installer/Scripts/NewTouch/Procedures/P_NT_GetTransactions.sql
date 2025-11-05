GO
/****** Object:  StoredProcedure [dbo].[P_NT_GetTransactions]    Script Date: 05/13/2015 16:36:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
--Delete From P_NT_GetTransactions
--Insert Into T_NT_Transactions
--Exec P_NT_GetTransactions '07/30/2015','07/30/2015'
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_GetTransactions] 
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
SectorName varchar(Max),SubSectorName varchar(Max),CountryName varchar(Max),CompanyName varchar(Max),BloombergSymbol varchar(Max),PutOrCall varchar(Max),ExpirationDate datetime)
Insert Into #SecMasterDataTempTable 
(TickerSymbol,UnderlyingDelta,
SectorName,SubSectorName,CountryName,CompanyName,BloombergSymbol,PutOrCall,ExpirationDate) 
Select TickerSymbol,UnderlyingDelta,
SectorName,SubSectorName,CountryName,CompanyName,BloombergSymbol,PutOrCall,ExpirationDate From V_SecMasterData 

Create Table #Transactions
(AcctId int,AcctName varchar(Max),RunDate datetime,
Symbol varchar(Max),UnderlyingSymbol varchar(Max),Open_CloseTag varchar(Max),AvgPrice float,Quantity float,Side varchar(Max),NetAmountBase float,Dividend float,TradeCurrency varchar(Max),
UdaSector varchar(Max),
UdaSubSector varchar(Max),
UdaCountry varchar(Max),
Strategy varchar(Max),
SymbolDescription varchar(Max),
UnderlyingSymbolDescription varchar(Max),
BloombergSymbol varchar(Max),
PutOrCall varchar(Max),
ExpirationDate datetime,
Asset varchar(Max),SetupAsset varchar(Max),
CommissionAndFees bit,
FXPNL bit,
PriceMultiplier float,
DeltaAdjPosMultiplier bit,
ZeroOrEndingMVOrUnrealized int,
CouponRate bit,
BlackScholesOrBlack76 bit,
GroupID varchar(50),
TransactionType varchar(100))

Insert Into #Transactions
(AcctId,AcctName,RunDate,
Symbol,UnderlyingSymbol,Open_CloseTag,AvgPrice,Quantity,Side,TradeCurrency,NetAmountBase,Dividend,
UdaSector,
UdaSubSector,
UdaCountry,
Strategy,
SymbolDescription,
UnderlyingSymbolDescription,
BloombergSymbol,
PutOrCall,
ExpirationDate,
Asset,SetupAsset,
CommissionAndFees,
FXPNL,
PriceMultiplier,
DeltaAdjPosMultiplier,
ZeroOrEndingMVOrUnrealized,
CouponRate,
BlackScholesOrBlack76,
GroupID,TransactionType)
Select FS.FundId As AcctId,FS.Fund As AcctName,RunDate,
A.Symbol,IsNull(UnderlyingSymbol,'Undefined'),Open_CloseTag,AvgPrice,Quantity,Side,TradeCurrency,NetAmountBase,Dividend,
UdaSector = IsNull(SM.SectorName,'Undefined'),
UdaSubSector = IsNull(SM.SubSectorName,'Undefined'),
UdaCountry = IsNull(SM.CountryName,'Undefined'),
A.Strategy,
SymbolDescription = IsNull(SM.CompanyName,A.Symbol),
UnderlyingSymbolDescription = IsNull(USM.CompanyName,IsNull(UnderlyingSymbol,'Undefined')),
BloombergSymbol = IsNull(SM.BloombergSymbol,A.Symbol),
PutOrCall = IsNull(SM.PutOrCall,''),
ExpirationDate = SM.ExpirationDate,
A.Asset,B.Asset As SetupAsset,
B.CommissionAndFees,
B.FXPNL,
B.PriceMultiplier,
B.DeltaAdjPosMultiplier,
B.ZeroOrEndingMVOrUnrealized,
B.CouponRate,
B.BlackScholesOrBlack76,
GroupID,
TransactionType
From T_MW_Transactions A 
Join #FundIDSymbol FS On
(FS.Fund = A.Fund) And 
(FS.Symbol = A.Symbol Or FS.Symbol Is Null) 
Join T_NT_AssetwisePreferences B On 
(Case 
When A.Asset = 'FixedIncome' Then 'FixedIncome' 
When A.Asset = 'Equity' And A.IsSwapped = 0 Then 'Equity' 
When A.Asset = 'FX' Then 'FX' 
When A.Asset = 'Future' Then 'Future' 
When A.Asset = 'Equity' And A.IsSwapped = 1 Then 'Swap' 
When A.Asset = 'FutureOption' And A.TradeCurrency <> @BaseCurrencyName Then 'InternationalFutureOption' 
When A.Asset = 'FutureOption' And A.TradeCurrency = @BaseCurrencyName Then 'LocalFutureOption'
Else 'Default' 
End) = 
B.Asset 
Left Outer Join #SecMasterDataTempTable SM On (A.Symbol = SM.TickerSymbol)
Left Outer Join #SecMasterDataTempTable USM On (A.UnderlyingSymbol = USM.TickerSymbol) 
Where  
(A.Asset <> 'Cash' And A.Open_CloseTag In ('O','C')) And 
(Datediff(d,@FromDate,RunDate) >= 0 and Datediff(d,RunDate,@ToDate) >= 0) 

Select AcctId,AcctName,RunDate,
Symbol,UnderlyingSymbol,Open_CloseTag,AvgPrice,Quantity,Side,TradeCurrency,NetAmountBase,Dividend,
UdaSector,
UdaSubSector,
UdaCountry,
Strategy,
SymbolDescription,
UnderlyingSymbolDescription,
BloombergSymbol,
PutOrCall,
ExpirationDate,
Asset,SetupAsset,
CommissionAndFees,
FXPNL,
PriceMultiplier,
DeltaAdjPosMultiplier,
ZeroOrEndingMVOrUnrealized,
CouponRate,
BlackScholesOrBlack76,
GroupID,TransactionType

From #Transactions 

Drop Table #Transactions,#SecMasterDataTempTable
Drop Table #FundIDSymbol

END

GO