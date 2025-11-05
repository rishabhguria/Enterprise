

CREATE Procedure [dbo].[P_GetGroupAllocationTradesDetails_ACK_EOD]
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

--declare
--@thirdPartyID INT
--	,@companyFundIDs VARCHAR(max)
--	,@inputDate DATETIME
--	,@companyID INT
--	,@auecIDs VARCHAR(max)
--	,@TypeID INT -- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties 
--	,@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                                                            
--	,@fileFormatID INT
--	,@includeSent INT = 0

--set @thirdPartyID=109
--set @companyFundIDs=N'1,9,2,10,5,11,6,8,7,3,4'
--set @inputDate='2023-11-01 09:18:19'
--set @companyID=7
--set @auecIDs=N'11,1,15,62,73,12,32'
--set @TypeID=0
--set @dateType=0
--set @fileFormatID=47

--Declare @InputDate DateTime
--Set @InputDate = '2021-09-21'

DECLARE @Fund TABLE (FundID INT)

INSERT INTO @Fund
SELECT Cast(Items AS INT)
FROM dbo.Split(@companyFundIDs, ',')


Create Table #Temp_Trades_Grouped
(
[TableID] [int] IDENTITY(1,1) NOT NULL,

Asset Varchar(20),
TradeCurrency Varchar(10),
AccountName Varchar(50),
Symbol Varchar(200),
Side Varchar(20),
TradeDate Varchar(20),
CounterParty Varchar(50),
BlockQuantity Float,
Quantity Float,
AvgPrice Float,
Commission Float,
ISIN Varchar(50),
CUSIP Varchar(50),
SEDOL Varchar(50),
OSIOptionSymbol Varchar(50),
Symbol_PK BigInt,
AllocationNo Int,
DefKey Varchar(200),
GroupID Varchar(200),
CustomOrdering Int
)

Insert InTo #Temp_Trades_Grouped

Select 

A.AssetName As Asset,
TC.CurrencySymbol As TradeCurrency,
Cast('' As Varchar(200)) As AccountName,
VT.Symbol AS Symbol,
S.Side As Side,
convert(varchar, VT.ProcessDate, 10) As TradeDate,
CP.ShortName As [CounterParty],
Sum(VT.TaxLotQty) As BlockQuantity,
0 As Quantity,
Sum(VT.AvgPrice * VT.TaxLotQty) As AvgPrice,
Sum(VT.Commission + VT.SoftCommission) As Commission,
Max(SM.ISINSymbol) As ISIN,
Max(SM.CUSIPSymbol) As CUSIP,
Max(SM.SEDOLSymbol) As SEDOL,
Max(SM.OSISymbol) As OSIOptionSymbol,
Max(SM.Symbol_PK) As Symbol_PK,
0 As AllocationNo,
Cast(convert(varchar, VT.ProcessDate, 10) + '_' + VT.Symbol + '_' + S.Side + '_' + CP.ShortName As varchar(500)) As DefKey,
max(Vt.GroupID),
1 As CustomOrdering


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
Group By convert(varchar, VT.ProcessDate, 10), VT.Symbol, S.Side,CP.ShortName,A.AssetName,TC.CurrencySymbol
Order By  convert(varchar, VT.ProcessDate, 10),VT.Symbol, S.Side

Update #Temp_Trades_Grouped
Set AvgPrice  = 
Case 
	When BlockQuantity <> 0
	Then AvgPrice / BlockQuantity
	Else 0
End

Insert Into #Temp_Trades_Grouped
Select 

A.AssetName As Asset,
TC.CurrencySymbol As TradeCurrency,
CF.FundName As AccountName,
VT.Symbol AS Symbol,
S.Side As Side,
Convert(varchar, VT.ProcessDate, 10) As TradeDate,
CP.ShortName As [CounterParty],
'' AS BlockQuantity,
(VT.TaxLotQty) As Quantity,
VT.AvgPrice As AvgPrice,
VT.Commission + VT.SoftCommission As Commission,
(SM.ISINSymbol) As ISIN,
(SM.CUSIPSymbol) As CUSIP,
(SM.SEDOLSymbol) As SEDOL,
SM.OSISymbol As OSIOptionSymbol,
SM.Symbol_PK As Symbol_PK,
0 As AllocationNo,
Cast(convert(varchar, VT.ProcessDate, 10) + '_' + VT.Symbol + '_' + S.Side + '_' + CP.ShortName As varchar(500)) As DefKey,
Vt.GroupID,
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

Select
GroupID,
Symbol,
Side,
CUSIP,
SEDOL,
OSIOptionSymbol,
TradeCurrency,
Asset,
[CounterParty],
BlockQuantity,
Quantity,
AvgPrice,
AccountName,
Commission,
CustomOrdering,
DENSE_RANK ( ) OVER(PARTITION BY GroupID, Symbol order by AccountName) AS AllocationNo
--ROW_NUMBER() OVER(PARTITION BY Symbol ORDER BY AccountName) AS AllocationNo

from #Temp_Trades_Grouped
--where Symbol='VC'
--Where CounterParty='WCHV'
Order By DefKey, CustomOrdering, AccountName, Symbol, Side

Drop Table #Temp_Trades_Grouped

