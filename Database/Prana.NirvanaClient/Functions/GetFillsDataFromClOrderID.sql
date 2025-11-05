CREATE Function DBO.GetFillsDataFromClOrderID(@ClOrder Varchar(500))
Returns table
As
Return(
 
Select 
'IsAnOrder' As IsAnOrder
,TOrd.Symbol As Symbol
,Curr.CurrencySymbol As Currency
,Ven.VenueName As venue
,Sub.ClOrderID As OrderRef
,Sub.TradeAttribute6 As PMID
,CP.ShortName As ParticipantCode
,CU.Login As TraderID
,'' As DecisionTime
,Sub.InsertionTime As ArrivalTime_QuoteTime
,Fills.TransactTime AS FirstFillTime_TradeTime
,'' As LastFillTime
,Fills.LastPx As Price
,Fills.LastShares As Quantity
,S.Side As Side
,'FlowType' As FlowType
,'F' AS MessageType
,Sub.ParentClOrderID AS ParentOrderRef
,Fills.exectype As ExecutionType
--,'FeeBasis1' As FeeBasis1
--,'FeeAmount1' As FeeAmount1
--,'FeeBasis2' As FeeBasis2
--, 'FeeAmount2' As FeeAmount2
,0.0 As FeeBasis1
,0.0 As FeeAmount1
,0.0 As FeeBasis2
, 0.0 As FeeAmount2
, '' As ActionType
, '' As ActionDateTime
,TIF.TimeInForce
,'ClientCategory' As ClientCategory
,'' as OrderType
,'' as OrderTypeChangeTime
,'' as OriginalOrderQuantity
,'' as Desk
,'' As MsgType
,'' As InsertionTime
,Sub.StagedOrderID
From T_Fills Fills With (NoLock)
inner Join T_Sub Sub With (NoLock) On Sub.ClOrderID = Fills.ClOrderID
inner join T_Order TOrd With (NoLock) On TOrd.ParentClOrderID = Sub.ParentClOrderID 
Inner Join T_CompanyUser CU On CU.UserID = Sub.UserID
Inner Join T_Currency Curr On Curr.CurrencyID = Tord.CurrencyID
Inner Join T_Side S On S.SideTagValue = Tord.OrderSidetagValue
Left Outer Join T_CounterParty CP On CP.CounterPartyID = Sub.CounterPartyID
Left Outer Join T_Venue Ven On Ven.VenueID = Sub.VenueID
Left Outer Join T_TimeInForce TIF On TIF.TimeInForceTagValue = Sub.TimeInForce
Where Fills.ClOrderID = @ClOrder And Sub.StagedOrderID <>''
);