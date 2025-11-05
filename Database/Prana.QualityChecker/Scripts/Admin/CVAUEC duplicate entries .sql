Declare @errormsg varchar(max)
set @errormsg=''

SELECT 
OT.CVAUECID,
OT.SideID,
PV.DisplayName AS [Counterparty Venue],
AU.DisplayName as [AUEC Display Name],
O.Side as [Side]
Into #TempData1
FROM T_CVAUECSide OT 
inner JOIN T_CVAUEC CV ON OT.CVAUECID = cv.CVAUECID
INNER JOIN T_CounterPartyVenue PV on PV.CounterPartyVenueID = CV.CounterPartyVenueID
INNER JOIN T_AUEC AU on AU.AUECID = CV.AUECID
INNER JOIN T_Side O on O.SideID = OT.SideID
--Where PV.DisplayName='Nasdaq' and O.OrderTypes='Market' and AU.DisplayName='NASDAQ'
--GROUP BY OT.CVAUECID,ot.OrderTypesID
--HAVING COUNT(*)>1 


;WITH CVAUEC_CTE([Counterparty Venue],[AUEC Display Name],[Side], Ranking)          
AS          
(          
SELECT          
 [Counterparty Venue],[AUEC Display Name],[Side],          
Ranking = DENSE_RANK() OVER(PARTITION BY CVAUECID, SideID ORDER BY NEWID() ASC)          
FROM #TempData1         
)          
Select
[Counterparty Venue],
[AUEC Display Name],
[Side],
Ranking AS [Duplicacy Number] 
Into #CorruptedCVAUECData
FROM CVAUEC_CTE
WHERE Ranking > 1

IF EXISTS(Select * from #CorruptedCVAUECData)
Begin

set @errormsg='Corrupted Data in T_CVAUECSide |'

Select
[Counterparty Venue],
[AUEC Display Name],
[Side],
[Duplicacy Number] 
FROM #CorruptedCVAUECData
WHERE [Duplicacy Number] > 1

END

-----------------------------------------------------------------Section 2 -------------------------------------------------------------



SELECT 
OT.CVAUECID,
OT.ExecutionInstructionsID,
PV.DisplayName AS [Counterparty Venue],
AU.DisplayName as [AUEC Display Name],
O.ExecutionInstructions as [Execution Instructions]
Into #TempData2
FROM T_CVAUECExecutionInstructions OT 
inner JOIN T_CVAUEC CV ON OT.CVAUECID = cv.CVAUECID
INNER JOIN T_CounterPartyVenue PV on PV.CounterPartyVenueID = CV.CounterPartyVenueID
INNER JOIN T_AUEC AU on AU.AUECID = CV.AUECID
INNER JOIN T_ExecutionInstructions O on O.ExecutionInstructionsID = OT.ExecutionInstructionsID
--Where PV.DisplayName='Nasdaq' and O.OrderTypes='Market' and AU.DisplayName='NASDAQ'
--GROUP BY OT.CVAUECID,ot.OrderTypesID
--HAVING COUNT(*)>1 


;WITH CVAUEC_CTE([Counterparty Venue],[AUEC Display Name],[ExecutionInstructions], Ranking)          
AS          
(          
SELECT          
 [Counterparty Venue],[AUEC Display Name],[ExecutionInstructionsID],          
Ranking = DENSE_RANK() OVER(PARTITION BY CVAUECID, ExecutionInstructionsID ORDER BY NEWID() ASC)          
FROM #TempData2         
)          
Select
[Counterparty Venue],
[AUEC Display Name],
[ExecutionInstructions],
Ranking AS [Duplicacy Number] 
Into #CorruptedCVAUECData1
FROM CVAUEC_CTE
WHERE Ranking > 1

IF EXISTS(Select * from #CorruptedCVAUECData1)
Begin

set @errormsg=@errormsg+' Corrupted Data in T_CVAUECExecutionInstructions |'

Select
[Counterparty Venue],
[AUEC Display Name],
[ExecutionInstructions],
[Duplicacy Number] 
FROM #CorruptedCVAUECData1
WHERE [Duplicacy Number] > 1

END

-----------------------------------------------------------------Section 3 -------------------------------------------------------------
SELECT 
OT.CVAUECID,
OT.HandlingInstructionsID,
PV.DisplayName AS [Counterparty Venue],
AU.DisplayName as [AUEC Display Name],
O.HandlingInstructions as [Handling Instructions]
Into #TempData3
FROM T_CVAUECHandlingInstructions OT 
inner JOIN T_CVAUEC CV ON OT.CVAUECID = cv.CVAUECID
INNER JOIN T_CounterPartyVenue PV on PV.CounterPartyVenueID = CV.CounterPartyVenueID
INNER JOIN T_AUEC AU on AU.AUECID = CV.AUECID
INNER JOIN T_HandlingInstructions O on O.HandlingInstructionsID = OT.HandlingInstructionsID
--Where PV.DisplayName='Nasdaq' and O.OrderTypes='Market' and AU.DisplayName='NASDAQ'
--GROUP BY OT.CVAUECID,ot.OrderTypesID
--HAVING COUNT(*)>1 


;WITH CVAUEC_CTE([Counterparty Venue],[AUEC Display Name],[HandlingInstructions], Ranking)          
AS          
(          
SELECT          
 [Counterparty Venue],[AUEC Display Name],[HandlingInstructionsID],          
Ranking = DENSE_RANK() OVER(PARTITION BY CVAUECID, HandlingInstructionsID ORDER BY NEWID() ASC)          
FROM #TempData3         
)          
Select
[Counterparty Venue],
[AUEC Display Name],
[HandlingInstructions],
Ranking AS [Duplicacy Number] 
Into #CorruptedCVAUECData2
FROM CVAUEC_CTE
WHERE Ranking > 1

IF EXISTS(Select * from #CorruptedCVAUECData2)
Begin

set @errormsg=@errormsg+' Corrupted Data in T_CVAUECHandlingInstructions |'

Select
[Counterparty Venue],
[AUEC Display Name],
[HandlingInstructions],
[Duplicacy Number] 
FROM #CorruptedCVAUECData2
WHERE [Duplicacy Number] > 1

END

-----------------------------------------------------------------Section 4 -------------------------------------------------------------
SELECT 
OT.CVAUECID,
OT.TimeInForceID,
PV.DisplayName AS [Counterparty Venue],
AU.DisplayName as [AUEC Display Name],
O.TimeInForce as [TimeInForceTagValue]
Into #TempData4
FROM T_CVAUECTimeInForce OT 
inner JOIN T_CVAUEC CV ON OT.CVAUECID = cv.CVAUECID
INNER JOIN T_CounterPartyVenue PV on PV.CounterPartyVenueID = CV.CounterPartyVenueID
INNER JOIN T_AUEC AU on AU.AUECID = CV.AUECID
INNER JOIN T_TimeInForce O on O.TimeInForceID = OT.TimeInForceID
--Where PV.DisplayName='Nasdaq' and O.OrderTypes='Market' and AU.DisplayName='NASDAQ'
--GROUP BY OT.CVAUECID,ot.OrderTypesID
--HAVING COUNT(*)>1 


;WITH CVAUEC_CTE([Counterparty Venue],[AUEC Display Name],[TimeInForce], Ranking)          
AS          
(          
SELECT          
 [Counterparty Venue],[AUEC Display Name],[TimeInForceID],          
Ranking = DENSE_RANK() OVER(PARTITION BY CVAUECID, TimeInForceID ORDER BY NEWID() ASC)          
FROM #TempData4         
)          
Select
[Counterparty Venue],
[AUEC Display Name],
[TimeInForce],
Ranking AS [Duplicacy Number] 
Into #CorruptedCVAUECData3
FROM CVAUEC_CTE
WHERE Ranking > 1

IF EXISTS(Select * from #CorruptedCVAUECData3)
Begin

set @errormsg=@errormsg+' Corrupted Data in T_CVAUECTimeInForce |'

Select
[Counterparty Venue],
[AUEC Display Name],
[TimeInForce],
[Duplicacy Number] 
FROM #CorruptedCVAUECData3
WHERE [Duplicacy Number] > 1

END

-----------------------------------------------------------------Section 5 -------------------------------------------------------------
SELECT 
OT.CVAUECID,
OT.OrderTypesID,
PV.DisplayName AS [Counterparty Venue],
AU.DisplayName as [AUEC Display Name],
O.OrderTypes as [Order Type]
Into #TempData
FROM T_CVAUECOrderTypes OT 
inner JOIN T_CVAUEC CV ON OT.CVAUECID = cv.CVAUECID
INNER JOIN T_CounterPartyVenue PV on PV.CounterPartyVenueID = CV.CounterPartyVenueID
INNER JOIN T_AUEC AU on AU.AUECID = CV.AUECID
INNER JOIN T_OrderType O on O.OrderTypesID = OT.OrderTypesID
--Where PV.DisplayName='Nasdaq' and O.OrderTypes='Market' and AU.DisplayName='NASDAQ'
--GROUP BY OT.CVAUECID,ot.OrderTypesID
--HAVING COUNT(*)>1 


;WITH CVAUEC_CTE([Counterparty Venue],[AUEC Display Name],[Order Type], Ranking)          
AS          
(          
SELECT          
 [Counterparty Venue],[AUEC Display Name],[Order Type],          
Ranking = DENSE_RANK() OVER(PARTITION BY CVAUECID, OrderTypesID ORDER BY NEWID() ASC)          
FROM #TempData         
)          
Select
[Counterparty Venue],
[AUEC Display Name],
[Order Type],
Ranking AS [Duplicacy Number] 
Into #CorruptedCVAUECData4
FROM CVAUEC_CTE
WHERE Ranking > 1

IF EXISTS(Select * from #CorruptedCVAUECData4)
Begin

set @errormsg=@errormsg+' Corrupted Data in T_CVAUECOrderTypes.'

Select
[Counterparty Venue],
[AUEC Display Name],
[Order Type],
[Duplicacy Number] 
FROM #CorruptedCVAUECData4
WHERE [Duplicacy Number] > 1

END
-----------------------------------------------------------------We are done-------------------------------------------------------------

select @errormsg as ErrorMsg

Drop Table #TempData,#TempData1,#TempData2,#TempData3,#TempData4,#CorruptedCVAUECData,#CorruptedCVAUECData1,#CorruptedCVAUECData2,#CorruptedCVAUECData3,#CorruptedCVAUECData4