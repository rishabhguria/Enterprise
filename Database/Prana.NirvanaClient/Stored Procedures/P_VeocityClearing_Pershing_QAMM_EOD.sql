
/*
EXEC P_VeocityClearing_Pershing_QAMM_EOD 1,'1,2,3,4,5,67,8,9,10','2022-03-21',1,'1,2',1,0,12,0

*/

CREATE Procedure [dbo].[P_VeocityClearing_Pershing_QAMM_EOD]
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

--Declare @InputDate DateTime, @companyFundIDs VARCHAR(max)
--Set @InputDate = '2022-03-17'

--Set @companyFundIDs = ''

DECLARE @Fund TABLE (FundID INT)

If (@companyFundIDs = '')
	Begin
	INSERT INTO @Fund
		SELECT CompanyFundID
		FROM T_CompanyFunds
	End
Else
	Begin
		INSERT INTO @Fund
		SELECT Cast(Items AS INT)
		FROM dbo.Split(@companyFundIDs, ',')
	End


Create Table #Temp_Trades_Grouped
(
[TableID] [int] IDENTITY(1,1) NOT NULL,
LOCALREF Varchar(200),
Asset Varchar(20),
TIRORDERID Varchar(200),
TradeCurrency Varchar(10),
SettleCurrency Varchar(10),
AccountName Varchar(50),
Symbol Varchar(200),
Side Varchar(20),
TradeDate Varchar(20),
SettlementDate Varchar(20),
CounterParty Varchar(50),
Quantity Float,
AvgPrice Float,
CompanyName Varchar(200),
Commission Float,
ISIN Varchar(50),
CUSIP Varchar(50),
SEDOL Varchar(50),
OSIOptionSymbol Varchar(50),
FXRate_Taxlot Float,
FXConversionMethodOperator_Taxlot Varchar(10),
Symbol_PK BigInt,
BrokerID Int,
DefKey Varchar(200),
CustomOrdering Int
)

Insert InTo #Temp_Trades_Grouped

Select 
SUBSTRING (Replace(Max(convert(varchar, VT.ProcessDate, 11)),'/','') ,3 , 4 )  As LOCALREF,
A.AssetName As Asset,
Replace(Cast((convert(varchar, VT.ProcessDate, 10)  + Cast(Max(SM.Symbol_PK) As Varchar(20))) As Varchar(200)),'-','') As TIRORDERID,
TC.CurrencySymbol As TradeCurrency,
SC.CurrencySymbol As SettleCurrency,
Cast('' As Varchar(200)) As AccountName,
VT.Symbol,
S.Side As Side,
convert(varchar, VT.ProcessDate, 10) As TradeDate,
Max(convert(varchar, VT.SettlementDate, 10)) As SettlementDate,
CP.ShortName As [CounterParty],
Sum(VT.TaxLotQty) As Quantity,
Sum(VT.AvgPrice * VT.TaxLotQty) As AvgPrice,
Max(SM.CompanyName) As CompanyName,
Sum(VT.Commission + VT.SoftCommission) As Commission,
Max(SM.ISINSymbol) As ISIN,
Max(SM.CUSIPSymbol) As CUSIP,
Max(SM.SEDOLSymbol) As SEDOL,
Max(SM.OSISymbol) As OSIOptionSymbol,
Max(VT.FXRate_Taxlot) As FXRate_Taxlot,
Max(VT.FXConversionMethodOperator_Taxlot) As FXConversionMethodOperator_Taxlot,
Max(SM.Symbol_PK) As Symbol_PK,
Max(VT.CounterPartyID) As BrokerID,
Cast(convert(varchar, VT.ProcessDate, 10) + '_' + VT.Symbol + '_' + S.Side + '_' + CP.ShortName As varchar(500)) As DefKey,
1 As CustomOrdering

--InTo #Temp_Trades_Grouped

From V_TaxLots VT With (NoLock)
Inner Join @Fund F On F.FundID = VT.FundID
Inner Join T_CompanyFunds CF On CF.CompanyFundID = VT.FundID
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = VT.Symbol 
Inner Join T_Side S On S.SideTagValue = VT.OrderSideTagValue
Inner Join T_Asset A On A.AssetID = VT.AssetID
Inner Join T_Currency TC On TC.CurrencyID = VT.CurrencyID
Inner Join T_Currency SC On SC.CurrencyID = VT.SettlCurrency_Taxlot
Inner Join T_CounterParty CP On CP.CounterPartyID = VT.CounterPartyID

Where DateDiff(Day,VT.ProcessDate,@InputDate) = 0
Group By convert(varchar, VT.ProcessDate, 10), VT.Symbol, S.Side,CP.ShortName,A.AssetName,TC.CurrencySymbol,SC.CurrencySymbol
Order By  convert(varchar, VT.ProcessDate, 10),VT.Symbol, S.Side

Update #Temp_Trades_Grouped
Set AvgPrice  = 
Case 
	When Quantity <> 0
	Then AvgPrice / Quantity
	Else 0
End

Insert Into #Temp_Trades_Grouped
Select 
SUBSTRING (Replace(convert(varchar, VT.ProcessDate, 11),'/','') ,3 , 4 )  As LOCALREF,
A.AssetName As Asset,
Replace(Cast((convert(varchar, VT.ProcessDate, 10)  + Cast((SM.Symbol_PK) As Varchar(20))) As Varchar(200)),'-','') As TIRORDERID,
TC.CurrencySymbol As TradeCurrency,
SC.CurrencySymbol As SettleCurrency,
CF.FundName As AccountName,
VT.Symbol,
S.Side As Side,
Convert(varchar, VT.ProcessDate, 10) As TradeDate,
Convert(varchar, VT.SettlementDate, 10) As SettlementDate,
CP.ShortName As [CounterParty],
(VT.TaxLotQty) As Quantity,
VT.AvgPrice As AvgPrice,
SM.CompanyName As CompanyName,
VT.Commission + VT.SoftCommission As Commission,
(SM.ISINSymbol) As ISIN,
(SM.CUSIPSymbol) As CUSIP,
(SM.SEDOLSymbol) As SEDOL,
SM.OSISymbol As OSIOptionSymbol,
VT.FXRate_Taxlot,
VT.FXConversionMethodOperator_Taxlot As FXConversionMethodOperator_Taxlot,
SM.Symbol_PK As Symbol_PK,
VT.CounterPartyID As BrokerID,
Cast(convert(varchar, VT.ProcessDate, 10) + '_' + VT.Symbol + '_' + S.Side + '_' + CP.ShortName As varchar(500)) As DefKey,
2 As CustomOrdering

From V_TaxLots VT With (NoLock)
Inner Join @Fund F On F.FundID = VT.FundID
Inner Join T_CompanyFunds CF On CF.CompanyFundID = VT.FundID
Inner Join V_SecMasterData SM With (NoLock) On SM.TickerSymbol = VT.Symbol 
Inner Join T_Side S On S.SideTagValue = VT.OrderSideTagValue
Inner Join T_Asset A On A.AssetID = VT.AssetID
Inner Join T_Currency TC On TC.CurrencyID = VT.CurrencyID
Inner Join T_Currency SC On SC.CurrencyID = VT.SettlCurrency_Taxlot
Inner Join T_CounterParty CP On CP.CounterPartyID = VT.CounterPartyID
Where DateDiff(Day,VT.ProcessDate,@InputDate) = 0
Order By  convert(varchar, VT.ProcessDate, 10),VT.Symbol, S.Side,CF.FundName


Alter Table #Temp_Trades_Grouped
Add UniqueID Varchar(20)

Update #Temp_Trades_Grouped
set UniqueID = Replace(CONVERT(VARCHAR(19), GETDATE(), 10),'-','')  + Cast(TableID As Varchar(10))
Where AccountName = ''

Select *
InTo #Temp_Trades_Grouped_G
From #Temp_Trades_Grouped
Where AccountName = ''

update D
Set D.UniqueID  = G.UniqueID

From #Temp_Trades_Grouped_G G
Inner Join #Temp_Trades_Grouped D On D.Symbol= G.Symbol And D.Side = G.Side And D.BrokerID = G.BrokerID 

Update #Temp_Trades_Grouped
Set TradeDate = Replace(convert(varchar, Cast(TradeDate As Date), 23),'-',''),
SettlementDate = Replace(convert(varchar, Cast(SettlementDate As Date), 23),'-','')

Select * from #Temp_Trades_Grouped
Order By DefKey, CustomOrdering, AccountName, Symbol, Side

Drop Table #Temp_Trades_Grouped,#Temp_Trades_Grouped_G

