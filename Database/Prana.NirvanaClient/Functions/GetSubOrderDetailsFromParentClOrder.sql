CREATE Function DBO.GetSubOrderDetailsFromParentClOrder (@ParentClOrder Varchar(500))
Returns Table
As

Return(
 
Select 'IsAnOrder' As IsAnOrder
,Ord.Symbol As Symbol
,Curr.CurrencySymbol As Currency
,Ven.VenueName As venue
,Sub.ClOrderID As OrderRef
,Sub.tradeattribute6 as PMID
,CP.ShortName As ParticipantCode
,CU.Login As TraderID
,'' As DecisionTime
,Sub.InsertionTime As ArrivalTime_QuoteTime
--,Fills.TransactTime As FirstFillTime_TradeTime
--,CASE
--	when Sub.msgtype='G' then Fills.TransactTime
--	when Sub.msgtype='F' then Fills.TransactTime
--	when Sub.msgtype='D' then Fills.TransactTime
--	else Fills.TransactTime
--END as FirstFillTime_TradeTime
--,CONVERT(varchar, Sub.InsertionTime, 108)  As FirstFillTime_TradeTime
,convert(varchar, Sub.InsertionTime,112)+ '-' + convert(varchar, sub.InsertionTime,108) as FirstFillTime_TradeTime
,'' As LastFillTime
,Sub.price As Price
,Sub.quantity as Quantity
,S.Side As Side
, 'FlowType' as FlowType
,CASE
	when Sub.msgtype='G' then 'F'
	when Sub.msgtype='F' then 'F' 
	Else 'SO'
END As MessageType

--, 'SO' AS MessageType
,CASE
	when Sub.msgtype='G' then Sub.OrigClOrderID
	when Sub.msgtype='F' then Sub.OrigClOrderID
	else Sub.StagedOrderID
END as ParentOrderRef
,Sub.MsgType As ExecutionType
--,TGO.Commission As FeeBasis1
--,'FeeBasis1' As FeeBasis1
--,'FeeAmount1' As FeeAmount1
--,'FeeBasis2' As FeeBasis2
--, 'FeeAmount2' As FeeAmount2
,0.0 As FeeBasis1
,0.0  As FeeAmount1
,0.0  As FeeBasis2
,0.0  As FeeAmount2
, '' As ActionType
, '' As ActionDateTime
,TIF.TimeInForce
,'ClientCategory' As ClientCategory
,'' as OrderType
,'' as OrderTypeChangeTime
,sub.Quantity as OriginalOrderQuantity
,'' as Desk
,'' As MsgType
,'' As InsertionTime
,Sub.ParentClOrderID
From T_Sub Sub With (NoLock)
Inner Join T_Fills Fills With (NoLock) On Fills.ClOrderID = @ParentClOrder
Inner Join T_Order Ord With (NoLock) On Ord.ParentClOrderID = Sub.ParentClOrderID
Inner Join T_CompanyUser CU On CU.UserID = Sub.UserID
Inner Join T_Currency Curr On Curr.CurrencyID = Ord.CurrencyID
Inner Join T_Side S On S.SideTagValue = Ord.OrderSidetagValue
Left Outer Join T_CounterParty CP On CP.CounterPartyID = Sub.CounterPartyID
Left Outer Join T_Venue Ven On Ven.VenueID = Sub.VenueID
Left Outer Join T_TimeInForce TIF On TIF.TimeInForceTagValue = Sub.TimeInForce
--inner join T_TradedOrders TDO on TDO.CLOrderID=sub.ClOrderID
--inner join T_Group TGO on TGO.GroupID = TDO.GroupID 
Where StagedOrderID = @ParentClOrder
And Sub.msgtype <> 'ROR'
);