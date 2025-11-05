CREATE PROCEDURE [dbo].[P_GetBloterExecuation_Benchmark_EOD] (
	@thirdPartyID INT
	,@companyFundIDs VARCHAR(max)
	,@inputDate DATETIME
	,@companyID INT
	,@auecIDs VARCHAR(max)
	,@TypeID INT
	,-- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties       
	@dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                      
	,@fileFormatID INT
	,@includeSent BIT = 1
	)
AS


--declare @thirdPartyID INT
--	declare @companyFundIDs VARCHAR(max)
--	declare @inputDate DATETIME
--	declare @companyID INT
--	declare @auecIDs VARCHAR(max)
--	declare @TypeID INT ---- 0 means for Company Third Party, 1 means for Executing broker or All Data Parties       
--	declare @dateType INT -- 0 means for Process Date and 1 means Trade Date.                                                                                                                      
--	declare @fileFormatID INT
--	declare @includeSent BIT =1
	

--set @thirdPartyID=71
--set @companyFundIDs=N'16,30,11,31,39,10,32,12,3,15,33,37,8,34,4,36,5,35,7,40,38,6,1'
--set @inputDate='2022-09-01'
--set @companyID=7
--set @auecIDs=N'18,180,1,15,62,73,12,90,98,86,114,16,17,85,100'
--set @TypeID=0
--set @dateType=0
--set @fileFormatID=109

create table #TempFills
(
APSID varchar(50),
Quantity varchar(100),
Side varchar(20),
LastShares varchar(100),
Symbol varchar(200),
ExpirationDate varchar(100),
Exchange varchar(100),
CompanyName varchar(500),
LastPrice varchar(100),
PutOrCall varchar(100),
StrikePrice varchar(100),
FullName varchar(100),
TransactionTime varchar(100),
FundName varchar(100),
CLOrderID varchar(50),
CumQty varchar(100),
OrderStatus varchar(20),
FundID varchar(50),
TradeID varchar(50),
TIRORDERID Varchar(50),
CustomOrderBy Int
)

Create Table #TempAccountID
(
CLOrderID varchar(50),
FundID varchar(50)
)

Select Distinct
GroupID
InTO #Temp_GroupID
From V_TaxLots
Where DATEDIFF(DAY,AUECLocalDate,@inputDate) = 0

Select 
CLOrderID,
Symbol,
ROW_Number() over (partition by symbol order by symbol ) as Id 
InTo #SymbolID 
From T_TradedOrders TOR
Inner Join #Temp_GroupID T On T.GroupID = TOR.GroupID 
Where NirvanaSeqNumber is Not Null

--Select *
--From #SymbolID
--Where Symbol = 'ES U22'-- 'RTY Z22'

Insert InTo #TempAccountID
SELECT 
CLOrderID, 
100+ROW_NUMBER() over (order by CLOrderID) as FundID  
FROM #symbolID  
--Where Symbol = 'ES U22'--'RTY Z22'
group by CLOrderID

--Select *
--From #TempAccountID

Insert InTo #TempFills
Select 
TempID.Id As APSID, 
Fills.Quantity, 
Side.Side, 
Fills.LastShares,
Fills.Symbol, 
SM.ExpirationDate, 
AUEC.DisplayName as Exchange,
SM.CompanyName,
Fills.LastPx As LastPrice,
SM.PutOrCall,
SM.StrikePrice,
CP.FullName as FullName,
Fills.TransactTime As TransactionTime,
Null As FundName,
TOR.CLOrderID,
Fills.CumQty,
Fills.OrderStatus,
'43119'+ #TempAccountID.FundID as FundID,
--G.GroupRefID as TradeID,
TOR.CLOrderID as TradeID,
--Concat(G.GroupID,L1.FundID) As TIRORDERID
TOR.CLOrderID As TIRORDERID,
1 As CustomOrderBy
from T_Fills Fills
Inner Join T_Sub Sub On Sub.ClOrderID = Fills.ClOrderID
Inner Join #symbolID TempID On TempID.CLOrderID = SUB.CLOrderID
Inner Join #TempAccountID On #TempAccountID.CLOrderID = SUB.CLOrderID
Inner Join V_SecMasterData SM On SM.TickerSymbol = Fills.Symbol
Inner Join T_AUEC AUEC On AUEC.AUECID = SM.AUECID
Inner Join T_Exchange TE On TE.ExchangeID=AUEC.ExchangeID
Inner Join T_Side Side On Side.SideTagValue = Fills.SideID
Inner Join T_TradedOrders TOR On TOR.CLOrderID= Fills.ClOrderID
Inner Join T_CounterParty CP On CP.CounterPartyID= Sub.CounterPartyID
--Where DATEDIFF(d,Sub.AUECLocalDate,@inputDate)=0 
--And G.CumQty > 0
Where Fills.LastShares > 0
--And SM.TickerSymbol = 'ES U22'--'RTY Z22'
Order By Fills.Fills_PK ASC

insert into #TempFills
select distinct 
Fills.APSID,
L1.AllocatedQty As Quantity,
Fills.Side,
null As LastShares,
Fills.Symbol,
Fills.ExpirationDate,
Fills.Exchange,
Fills.CompanyName,
cast(TOR.AveragePrice as decimal(10,5)) As LastPrice,
Fills.PutOrCall,
Fills.StrikePrice,
Fills.FullName As FullName,
TOR.TransactTime As TransactionTime,
CF.FundName,
TOR.CLOrderID,
TOR.CumQty,
null As OrderStatus,
null As FundID,
Fills.TradeID,
Fills.TIRORDERID,
2 As CustomOrderBy
from T_TradedOrders TOR 
right join #TempFills Fills On Fills.ClOrderID = TOR.CLOrderID
Inner Join T_Group G On G.GroupID= TOR.GroupID
Inner Join T_FundAllocation L1 On L1.GroupID = G.GroupID
Inner Join T_CompanyFunds CF On CF.CompanyFundID = L1.FundID
--Where  DATEDIFF(d,TOR.AUECLocalDate,@inputDate)=0 and NirvanaSeqNumber is not null
--And G.Symbol ='ES U22'--'RTY Z22'

Select * 
From #TempFills 
--Where Symbol = 'ES U22'--'RTY Z22'
Order by CLOrderID ASC, OrderStatus DESC, CustomOrderBy

Drop Table #TempFills, #symbolID
Drop Table #TempAccountID, #Temp_GroupID


--select * from T_FundAllocation Where GroupID='2105210151310'



