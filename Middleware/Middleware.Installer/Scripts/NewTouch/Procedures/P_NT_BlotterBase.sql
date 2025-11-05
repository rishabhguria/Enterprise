GO
/****** Object:  StoredProcedure [dbo].[P_NT_BlotterBase]    Script Date: 02/24/2016 02:08:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
--Exec [P_NT_BlotterBase]
-- =============================================
CREATE PROCEDURE [dbo].[P_NT_BlotterBase]
-- Add the parameters for the stored procedure here
AS
BEGIN

--Set NOCOUNT ON;
SET NOCOUNT OFF;
SET FMTONLY OFF;

Create Table #Blotter
([FundName] varchar(Max),
[Symbol] varchar(Max),
[Side] varchar(Max),
[Average Price] float,
[Executed Quantity] float,
[Working Quantity] float,
[Order Quantity] float,
[Broker] varchar(Max),
[Execution Time] datetime,
[Security Description] varchar(Max),
[Expiration Date] datetime,
[Put/Call] varchar(Max),
[Symbol Description] varchar(Max),
[Order ID] varchar(Max),
[Created By] varchar(Max))

Insert Into #Blotter
([FundName],
[Symbol],
[Side],
[Average Price],
[Executed Quantity],
[Working Quantity],
[Order Quantity],
[Broker],
[Execution Time],
[Symbol Description],
[Order ID],
[Created By]) 
Select 
F.[FundName],
T.[Symbol],
S.[Side],
Avg(IsNull(T.[AvgPrice],0)),
Sum(IsNull(T.[TaxLotQty],0)),
Avg(IsNull(T.[Quantity] - T.[CumQty],0)),
Avg(IsNull(T.[Quantity],0)),
C.[ShortName],
T.[AUECLocalDate], 
max(Sec.[CompanyName]),
max(Trade.[OrderID]),
max(U.[ShortName]) 
From [V_Taxlots] T 
Join [T_CompanyFunds] F on F.[CompanyFundID]=T.[FundID] 
Left Outer Join [T_Side] S on S.[SideTagValue]=T.[OrderSideTagValue]
Left Outer Join [T_CounterParty] C on C.[CounterPartyID]=T.[CounterPartyID]
left Outer Join [v_secmasterdata] Sec on Sec.[TickerSymbol]=T.[Symbol]
Left Outer Join [T_TradedOrders] Trade on Trade.[GroupId]=T.[GroupId]
Left Outer Join [T_CompanyUser] U on U.[UserID]=T.[UserID]
Where 
DateDiff(d,T.[AUECLocalDate],GetDate()) = 0 And 
--DateDiff(d,T.[AUECLocalDate],GetDate()) = 0 And 
T.[Symbol] Is Not Null And 
T.[Quantity] > 0 
Group By 
F.[FundName],
T.[Symbol],
S.[Side],
C.[ShortName],
T.[AUECLocalDate]

Select 
[FundName],
[Symbol],
[Side],
[Average Price],
[Executed Quantity],
[Working Quantity],
[Order Quantity],
[Broker],
[Execution Time],
[Symbol Description],
[Order ID],
[Created By] 
From #Blotter 
Order By [Execution Time] Desc

Drop Table #Blotter
END

GO