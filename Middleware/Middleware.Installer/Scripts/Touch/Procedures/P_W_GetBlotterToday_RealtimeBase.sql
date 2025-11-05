/****** Object:  StoredProcedure [dbo].[P_W_GetBlotterToday_RealtimeBase]    Script Date: 02/27/2014 10:05:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- =============================================
CREATE PROCEDURE [dbo].[P_W_GetBlotterToday_RealtimeBase]
@GroupBy varchar(Max),@Client varchar(Max),@Entity varchar(Max)
AS
BEGIN

Set NOCOUNT ON;

Create Table #Funds(Fund varchar(Max))
If @GroupBy = 'Aggregate' 
Begin
	Insert Into #Funds(Fund)
	Select CF.FundName From T_W_Clients C Join T_W_ClientFundMapping M On C.ClientID = M.ClientID Join T_W_Funds F On M.TouchFundID = F.TouchFundID Join T_CompanyFunds CF On F.PranaFundID = CF.CompanyFundID 
	Where C.ClientName = @Client 
	
End
Else If @GroupBy = 'Fund' 
Begin 
	Insert Into #Funds(Fund) Select @Entity
End 

Create Table #Blotter
([Symbol] varchar(Max),
[Side] varchar(Max),
[Average Price] float,
[Executed Quantity] float,
[Working Quantity] float,
[Order Quantity] float,
[Broker] varchar(Max),
[Execution Time] datetime,
[Security Description] varchar(Max),
[Expiration Date] datetime,
[Put/Call] varchar(Max))

Insert Into #Blotter
([Symbol],
[Side],
[Average Price],
[Executed Quantity],
[Working Quantity],
[Order Quantity],
[Broker],
[Execution Time]) 
Select T.[Symbol],S.[Side],Avg(IsNull(T.[AvgPrice],0)) As [Average Price],Sum(IsNull(T.[TaxLotQty],0)) As [Executed Quantity],Avg(IsNull(T.[Quantity] - T.[CumQty],0)) As [Working Quantity],Avg(IsNull(T.[Quantity],0)) As [Order Quantity],C.[ShortName] As [Broker],T.[AUECLocalDate] As [Execution Time] 
From [V_Taxlots] T 
Join [T_CompanyFunds] F on F.[CompanyFundID]=T.[FundID] 
Join #Funds E On E.Fund = F.FundName 
Left Outer Join [T_Side] S on S.[SideTagValue]=T.[OrderSideTagValue]
Left Outer Join [T_CounterParty] C on C.[CounterPartyID]=T.[CounterPartyID]
Where 
DateDiff(d,T.[AUECLocalDate],GetDate()) = 0 And 
T.[Symbol] Is Not Null And T.[Quantity] > 0 
Group By T.[Symbol],S.[Side],C.[ShortName],T.[AUECLocalDate] 
Order By T.[AUECLocalDate] Asc 
--Select
--G.Symbol,
--S.Side,
--Avg(G.AvgPrice),
--Sum(G.CumQty),
--Case When Sum(G.Quantity - G.CumQty) < 0 Then 0 Else Sum(G.Quantity - G.CumQty) End,
--Sum(G.Quantity),
--C.ShortName,
--G.AUECLocalDate
--From T_Group G Left Outer Join T_Side S on S.SideTagValue = G.OrderSideTagValue Left Outer Join T_CounterParty C on C.CounterPartyID = G.CounterPartyID
--Where DateDiff(d,G.AUECLocalDate,GetDate()) = 0 And 
--G.Symbol Is Not Null And G.Quantity > 0 
--Group By G.Symbol,S.Side,C.ShortName,G.AUECLocalDate 
--Order By G.[AUECLocalDate] Asc

Select 
[Symbol],
[Security Description],
[Side],
[Average Price],
[Executed Quantity],
[Working Quantity],
[Order Quantity],
[Broker],
[Execution Time],
[Expiration Date],
[Put/Call] From #Blotter

Drop Table #Blotter,#Funds

END