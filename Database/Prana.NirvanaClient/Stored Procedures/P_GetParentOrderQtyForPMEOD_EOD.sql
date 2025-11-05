/*
EXEC P_GetParentOrderQtyForPMEOD_EOD '06-17-2020'
*/
CREATE PROCEDURE P_GetParentOrderQtyForPMEOD_EOD
(
	@inputDate DateTime
)
As 

SET NOCOUNT ON;
--Declare @inputDate DateTime
--Set @inputDate =  '09-22-2020'

Select Distinct 
Sub.StagedOrderID As Sub_StagedOrderID 
,Staged.ParentClOrderID As ParentClOrderID 
,(Staged.Quantity) As ParentOrderQty
,Staged.MsgType As Staged_MsgType

InTo #TempSubParentDetails
from T_Sub Sub With (NoLock)
Inner Join T_Sub Staged With (NoLock) On Staged.ParentClOrderID = Sub.StagedOrderID 
	Where 
	Sub.ClOrderID  In
	(
		Select ClOrderID 
		from T_Sub With (NoLock)
		Where DateDiff(Day,AUECLocalDate,@inputDate) = 0
		And IsHidden = 0		
	)

--Select * From #TempSubParentDetails
--Where ParentClOrderID = '2020061618260752'

Delete Main
From #TempSubParentDetails Main
Inner Join #TempSubParentDetails Sub On Sub.ParentClOrderID = Main.ParentClOrderID
Where Main.Staged_MsgType = 'D' And Sub.Staged_MsgType = 'G'
--Select * From #TempSubParentDetails
Select Sub_StagedOrderID,ParentClOrderID, Max(ParentOrderQty) As ParentOrderQty, Staged_MsgType
InTo #TempParentDetails
From #TempSubParentDetails
Group By 
 Sub_StagedOrderID
,ParentClOrderID
,Staged_MsgType

Alter Table #TempParentDetails
Add ExecutedQty Float

Update #TempParentDetails
Set ExecutedQty = 0

--Select * From #TempParentDetails

Select StagedOrderID,ClOrderID,ParentClOrderID,OrigClOrderID, InsertionTime
InTo #TempSubDetails
From T_Sub With (NoLock)
Where 
(
	StagedOrderID  In
	(
		Select T.ParentClOrderID
		From #TempParentDetails T
	)
	Or 
	ParentClOrderID  In
	(
		Select T.ParentClOrderID
		From #TempParentDetails T
	)
)
And MsgType <> 'ROR' 
--And  ClOrderID Not In ('2020052519070333','2020052519070369','2020052717430295','2020052717430306')
--select * from #TempSubDetails
Delete Main
From #TempSubDetails Main
Inner Join #TempSubDetails Sub On Sub.OrigClOrderID = Main.ClOrderID

ALTER Table #TempSubDetails
Add ExecutedQty Float

Update #TempSubDetails
Set ExecutedQty = 0

Select F.ClOrderID, Sum(F.CumQty) As ExecutedQty 
InTo #TempFillsDetails
From T_Fills F With (NoLock)
Inner Join V_LatestFillByOrderSeqNumber T With (NoLock) On T.ClOrderID = F.ClOrderID And F.NirvanaSeqNumber = T.OrderSeqNumber
Where F.ClOrderID In
(
	Select ClOrderID from #TempSubDetails
)
Group By F.ClOrderID

Update Sub
Set Sub.ExecutedQty = F.ExecutedQty
From #TempSubDetails Sub
Inner Join #TempFillsDetails F On F.ClOrderID = Sub.ClOrderID


;WITH cte AS (
    SELECT 
        StagedOrderID, 
        ClOrderID, 
        ParentClOrderID, 
        OrigClOrderID,
		InsertionTime, 
		ExecutedQty,
        ROW_NUMBER() OVER (
            PARTITION BY 
                StagedOrderID, 
                ParentClOrderID, 
                OrigClOrderID
            ORDER BY 
                InsertionTime desc
        ) row_num
     FROM 
        #TempSubDetails
)
delete from cte
WHERE row_num > 1;
--select * from #TempSubDetails where StagedOrderID='2020091818090578'
Select StagedOrderID As ParentClOrderID, Sum(ExecutedQty) As ExecutedQty
InTo #Temp
From #TempSubDetails
Group By StagedOrderID
--select * from #Temp
Update P
Set P.ExecutedQty = S.ExecutedQty
From #TempParentDetails P
Inner Join #Temp S On S.ParentClOrderID = P.ParentClOrderID

Alter Table #TempParentDetails
Add RemainingQty Float

Update #TempParentDetails
Set RemainingQty = 0

Update #TempParentDetails
Set RemainingQty = ParentOrderQty - ExecutedQty

Select 
Ord.Symbol, 
T.ParentClOrderID As ParentClOrderID, 
T.ParentOrderQty, 
T.ExecutedQty, 
T.RemainingQty, 
T.Sub_StagedOrderID, 
T.Staged_MsgType
From #TempParentDetails T
Inner Join T_Order Ord On Ord.ParentClOrderID = T.ParentClOrderID
--Where Ord.Symbol = '4152-GTS'
--Where Sub_StagedOrderID  = '2020052018295736'

Drop Table #TempParentDetails,#TempSubDetails,#TempFillsDetails,#Temp,#TempSubParentDetails
