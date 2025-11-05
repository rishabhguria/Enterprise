/*
EXEC P_GetFIXFile_Multiday_EOD 1,1,'2020-08-28',1,1,1,1,1,1
*/

create PROC P_GetFIXFile_Multiday_EOD
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

--Declare @inputDate date
--Set @inputDate = '2020-08-28'


Select Distinct Sub.ParentClOrderID 
InTo #TempParentClOrderID 
from T_Sub Sub With (NoLock)
Inner Join T_Sub Staged With (NoLock) On Staged.StagedOrderID = Sub.ParentClOrderID
Where DATEDIFF(Day,Staged.AUECLocalDate,@inputDate)=0

--Select * From #TempParentClOrderID
--Delete From #TempParentClOrderID
--Where ParentClOrderID Not In ('2020050102525415')

Select 
'IsAnOrder' As IsAnOrder
,TOrd.Symbol As Symbol
,Curr.CurrencySymbol As Currency
,Ven.VenueName As venue
,Sub.ParentClOrderID As OrderRef
,Sub.TradeAttribute6 As PMID
,CP.ShortName As ParticipantCode
,CU.Login As TraderID
,Sub.ServerTime As DecisionTime
,case
when sub.MsgType='D' 
then Sub.InsertionTime 
end As ArrivalTime_QuoteTime
,'' As FirstFillTime_TradeTime
,'' As LastFillTime
,Sub.price As Price
,Sub.quantity As Quantity
,S.Side As Side
, 'FlowType' As FlowType
, 'CO' As MessageType
, '' As ParentOrderRef
,Sub.MsgType As ExecutionType
--,'FeeBasis1' As FeeBasis1
--,'FeeAmount1' As FeeAmount1
--,'FeeBasis2' As FeeBasis2
--, 'FeeAmount2' As FeeAmount2
,0.0 As FeeBasis1
,0.0 As FeeAmount1
,0.0 As FeeBasis2
,0.0 As FeeAmount2
,'' as actiontupe
, '' As ActionDateTime
,TIF.TimeInForce
,'ClientCategory' As ClientCategory
,'' as OrderType
,'' as OrderTypeChangeTime
,'' as OriginalOrderQuantity
,'' as Desk
,Sub.MsgType
,Sub.InsertionTime
,Sub.StagedOrderID

InTo #TempParentDay
from T_Sub Sub With (NoLock)
inner join #TempParentClOrderID TP On TP.ParentClOrderID=Sub.ParentClOrderID
inner join T_Order TOrd With (NoLock) On TOrd.ParentClOrderID=Sub.ParentClOrderID
Inner Join T_CompanyUser CU On CU.UserID = Sub.UserID
Inner Join T_Currency Curr On Curr.CurrencyID = TOrd.CurrencyID
Inner Join T_Side S On S.SideTagValue = TOrd.OrderSidetagValue
Left Outer Join T_CounterParty CP On CP.CounterPartyID = Sub.CounterPartyID
Left Outer Join T_Venue Ven On Ven.VenueID = Sub.VenueID
Left Outer Join T_TimeInForce TIF On TIF.TimeInForceTagValue = Sub.TimeInForce
Where Sub.IsHidden = 0

select * into #TempParentMultidayDay from #TempParentDay where DATEDIFF(d,ArrivalTime_QuoteTime,@inputDate) = 0 
select  * into #TempParent from #TempParentDay where OrderRef not in (select OrderRef from #TempParentMultidayDay)

DECLARE @ParentClOrder varchar(500), @Count INT, @ParentClOrderIdCount INT
SET @Count = 1
Set @ParentClOrderIdCount = 0
Select @ParentClOrderIdCount = Count(Distinct OrderRef) From #TempParent

Select Max(InsertionTime) As InsertionTime ,OrderRef InTo #TempUpdatedParentOrderDetails From #TempParent
group By OrderRef

Delete From #TempParent
From #TempParent P
Inner join #TempUpdatedParentOrderDetails T On T.OrderRef = P.OrderRef
Where DateDiff(MS,P.InsertionTime,T.InsertionTime) > 0

--Select * from #TempParent

 CREATE TABLE #FinalData
(
ID Int Identity (1,1),
IsAnOrder varchar(50),
Symbol varchar(50),
Currency varchar(50),
Venue varchar(50),
ParentClOrderID Varchar(500),
PMID varchar(50),
Broker varchar(50),
TraderID varchar(50),
DecisionTime varchar(50),
ArrivalTime_QuoteTime datetime,
FirstFillTime_TradeTime varchar(50),
LastFillTime varchar(50),
Price FLOAT,
Quantity FLOAT,
Side Varchar(20),
FlowType Varchar(20),
MessageType varchar(50),
ParentOrderRef varchar(50),
ExecutionType varchar(50),
FeeBasis1 varchar(50),
FeeAmount1 varchar(50),
FeeBasis2 varchar(50),
FeeAmount2 varchar(50),
ActionType varchar(100),
ActionDateTime datetime,
TimeInForce varchar(50),
ClientCategory varchar(50)
,OrderType varchar(50)
,OrderTypeChangeTime varchar(50)
,OriginalOrderQuantity varchar(50)
,Desk varchar(50)
,MsgType Varchar(20)
,InsertionTime Varchar(50)
,StagedOrderID  Varchar(50)
)

 CREATE TABLE #TempSO
(
IsAnOrder varchar(50),
Symbol varchar(50),
Currency varchar(50),
Venue varchar(50),
ClOrderID Varchar(500),
PMID varchar(50),
Broker varchar(50),
TraderID varchar(50),
DecisionTime varchar(50),
ArrivalTime_QuoteTime Datetime,
FirstFillTime_TradeTime varchar(50),
LastFillTime varchar(50),
Price FLOAT,
Quantity FLOAT,
Side Varchar(20),
FlowType Varchar(20),
MessageType varchar(50),
ParentOrderRef varchar(50),
ExecutionType varchar(50),
FeeBasis1 varchar(50),
FeeAmount1 varchar(50),
FeeBasis2 varchar(50),
FeeAmount2 varchar(50),
ActionType varchar(100),
ActionDateTime datetime,
TimeInForce varchar(50),
ClientCategory varchar(50)
,OrderType varchar(50)
,OrderTypeChangeTime varchar(50)
,OriginalOrderQuantity varchar(50)
,Desk varchar(50)
,MsgType Varchar(20)
,InsertionTime Varchar(50)
,StagedOrderID  Varchar(50)
)

CREATE TABLE #TempLastFillExec
(
ClOrderID Varchar(50),
AveragePrice Float,
TransactTime Varchar(50),
Qauntity Float
)

CREATE TABLE #TempTableForTimeQtyUpdate
(
StagedOrderID Varchar(50),
ClOrderID Varchar(50),
ArrivalTime_QuoteTime Varchar(50)
)

CREATE TABLE #TempTableForFirstLastPrice
(
ParentClorderid Varchar(50),
ClOrderID Varchar(50),
Price float,
FirstFillTime_TradeTime varchar(50),
LastFillTime varchar(50),
FeeBasis1 varchar(50)
)


WHILE @Count <= @ParentClOrderIdCount
	BEGIN

	Select @ParentClOrder = (Select Top 1 OrderRef From #TempParent)

	Insert Into #FinalData Select * From #TempParent Where (OrderRef=@ParentClOrder)
	
	Insert Into #TempSO Select * From DBO.GetSubOrderDetailsFromParentClOrder(@ParentClOrder) --Order By OrderRef

	DECLARE @SOCount INT
	Set @SOCount = 0
	Select @SOCount =  Count(Distinct ClOrderID) from #TempSO

	--Select S.StagedOrderID, S.ClOrderID,S.ParentOrderRef,S.MessageType,S.* 
	--From #TempSO S

	Insert InTo #TempTableForTimeQtyUpdate
	Select StagedOrderID, Max(ClOrderID) As ClOrderID, Max(ArrivalTime_QuoteTime) As ArrivalTime_QuoteTime
	 From #TempSO
	Group By StagedOrderID

	--Select * FRom #TempTableForTimeQtyUpdate

	DECLARE @Counter INT, @GrpedCLCount Int
	Set @GrpedCLCount = 0

	Select @GrpedCLCount = Count(ClOrderID) From #TempTableForTimeQtyUpdate
	SET @Counter = 1;


	While @Counter <= @GrpedCLCount
		Begin
				DECLARE @GpedClorderId Varchar(200)
				Select @GpedClorderId = (Select Top 1 ClOrderID from #TempTableForTimeQtyUpdate)

				Insert into #TempLastFillExec Select * From DBO.GetLastFillAvgPriceAndExecTime(@GpedClorderId)
				Update SO
				Set SO.Price = T.AveragePrice,
				SO.LastFillTime = T.TransactTime,
				SO.Quantity = T.Qauntity
				From #TempSO SO
				Inner Join #TempTableForTimeQtyUpdate Temp On Temp.StagedOrderID = SO.ClOrderID
				Inner Join #TempLastFillExec T On T.ClOrderID = Temp.ClOrderID						 
				And SO.MessageType = 'SO'
				--select @GpedClorderId
				

				SET @Counter = @Counter + 1;
				Delete #TempTableForTimeQtyUpdate where ClOrderID = @GpedClorderId
				Delete From #TempLastFillExec
		End
		

	SET @Counter = 1;

			While @Counter <= @SOCount
				begin

					DECLARE @SOClorderId Varchar(200)
					Select @SOClorderId = (Select Top 1 ClOrderID from #TempSO)

					insert into #FinalData select * from #TempSO where ClOrderID = @SOClorderId

					Update Temp
					set DecisionTime = TSUB.ServerTime, ArrivalTime_QuoteTime = TSUB.InsertionTime
					from #FinalData Temp
					inner join T_Sub TSUB With (NoLock) on TSUB.ParentClOrderID=temp.ParentClOrderID
					where TSUB.MsgType='D'	
				
					insert into #FinalData select * from DBO.GetFillsDataFromClOrderID(@SOClorderId) 
					
					 
				--select * from #FinalData
				--select * from #TempSO
				declare @AvgPrice Float;
				declare @lastfillTimeForCo varchar(50);
				declare @FirstfillTimeForCo varchar(50);
				set  @lastfillTimeForCo= (select LastFillTime from #TempSO where MessageType='SO' and ClOrderID=@SOClorderId)
				set  @FirstfillTimeForCo= (select FirstFillTime_TradeTime from #TempSO where MessageType='SO' and ClOrderID=@SOClorderId)
				set @AvgPrice= (select avg(price) from #TempSO where MessageType='SO' and ClOrderID=@SOClorderId)
				update  FD
						set FeeBasis1=TGP.Commission
						from #FinalData FD
						inner join T_TradedOrders TTO on TTO.ParentClOrderID=FD.ParentClOrderID
						Inner join T_Group TGP On TGP.GroupID=TTO.GroupID
						where MessageType='SO'

update  FD
						set FeeBasis1=NULLIF(TGP.Commission,0)/NULLIF(FD.Quantity,0)
						from #FinalData FD
						inner join T_TradedOrders TTO on TTO.ParentClOrderID=FD.ParentOrderRef
						Inner join T_Group TGP On TGP.GroupID=TTO.GroupID
						where FD.MessageType='F'
				
				declare @FeeBasis1 varchar(50);
					 set @FeeBasis1= (select  SUM(convert(float, FeeBasis1)) from #FinalData where MessageType='SO' and ParentClOrderID=@SOClorderId )
					
					insert into #TempTableForFirstLastPrice (ParentClorderid,ClOrderID,Price, FirstFillTime_TradeTime, LastFillTime,FeeBasis1)
					values (@ParentClOrder, @SOClorderId,@AvgPrice,@FirstfillTimeForCo,@lastfillTimeForCo,@FeeBasis1)
					
					
					SET @Counter = @Counter + 1;

					Delete #TempSO where ClOrderID = @SOClorderId
					Delete From #TempLastFillExec
				End

				Delete From #TempTableForTimeQtyUpdate 

	   SET @count = @count + 1;
	   Delete #TempParent where OrderRef=@ParentClOrder
	END;

	DECLARE @CounterPrice INT, @PriceCount Int
	Set @PriceCount = 0

	Select @PriceCount = Count(distinct ParentClorderid) From #TempTableForFirstLastPrice
	SET @CounterPrice = 1;
	--select @PriceCount
	while @CounterPrice<=@PriceCount
	begin
	DECLARE @parent Varchar(200)
					Select @parent = (Select Top 1 ParentClorderid from #TempTableForFirstLastPrice)
					update #FinalData
					set Price= (select AVG(price) from #TempTableForFirstLastPrice where ParentClorderid=@parent)
					 ,FirstFillTime_TradeTime = (select min(FirstFillTime_TradeTime )from #TempTableForFirstLastPrice where ParentClorderid=@parent),
					 LastFillTime= (select max(LastFillTime) from #TempTableForFirstLastPrice where ParentClorderid=@parent)
					 , FeeBasis1=(Select SUM(convert(float, FeeBasis1)) from #TempTableForFirstLastPrice where ParentClorderid=@parent)
					where MessageType='CO' and ParentClorderid=@parent
					SET @CounterPrice = @CounterPrice + 1;
					delete #TempTableForFirstLastPrice where ParentClorderid=@parent
					
	END
	
	
update #FinalData
set ExecutionType='D'
where MessageType='CO' 

select 
case
	when MessageType = 'CO' then 'TRUE'
	when MessageType = 'SO' then 'TRUE'
	when MessageType = 'F' then 'FALSE'
End as IsAnOrder,
Symbol,
Currency,
Venue,
ParentClOrderID,
PMID,
Broker,
TraderID,
DecisionTime,
convert(varchar, ArrivalTime_QuoteTime,112)+ '-' + convert(varchar, ArrivalTime_QuoteTime,108) as ArrivalTime_QuoteTime,
FirstFillTime_TradeTime,
LastFillTime,
Price,
Quantity,
Side,
FlowType,
MessageType,
ParentOrderRef,
CASE
	when ExecutionType='0' then 'New'
	when ExecutionType='1' then 'Partially filled'
	when ExecutionType='2' then 'Filled'
	when ExecutionType='3' then 'Done for day'
	when ExecutionType='4' then 'Canceled'
	when ExecutionType='5' then 'Replaced'
	when ExecutionType='6' then 'Pending Cancel'
	when ExecutionType='7' then 'Stopped'
	when ExecutionType='8' then 'Rejected'
	when ExecutionType='9' then 'Suspended'
	when ExecutionType='A' then 'Pending New'
	when ExecutionType='B' then 'Calculated'
	when ExecutionType='C' then 'Expired'
	when ExecutionType='D' then 'New'
	when ExecutionType='E' then 'Pending Replace'
	when ExecutionType='F' then 'Pending Cancel'
	when ExecutionType='G' then 'Pending Replace'
END as ExecutionType,
FeeBasis1,
FeeAmount1,
FeeBasis2,
FeeAmount2,
ActionType,
ActionDateTime,
TimeInForce,
ClientCategory  
, OrderType
, OrderTypeChangeTime
, OriginalOrderQuantity
, Desk 
From #FinalData
Order By ID


Drop Table #TempSO,#FinalData,#TempParentClOrderID,#TempLastFillExec,#TempTableForTimeQtyUpdate,#TempTableForFirstLastPrice
Drop Table #TempParentDay,#TempParent,#TempUpdatedParentOrderDetails,#TempParentMultidayDay