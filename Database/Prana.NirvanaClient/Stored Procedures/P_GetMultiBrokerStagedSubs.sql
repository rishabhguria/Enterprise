
CREATE procedure [dbo].[P_GetMultiBrokerStagedSubs] 
(
@AllAUECDatesString VARCHAR(max)
) 
as  
begin  
    
DECLARE @AUECDatesTable TABLE 
(
	AUECID INT,
	CurrentAUECDate DATETIME
)

INSERT INTO @AUECDatesTable
SELECT *
FROM dbo.GetAllAUECDatesFromString(@AllAUECDatesString)

declare @MultiChildParentSubIDs TABLE
(
    ClorderID VARCHAR(max)
)
																		  
-- Insert distinct values into @MultiChildParentSubIDs using a single INSERT statement with UNION
INSERT INTO @MultiChildParentSubIDs (ClorderID)
SELECT DISTINCT
    CASE 
        WHEN NirvanaMsgType = 14 THEN StagedOrderID 
        WHEN TimeInForce IN (1, 6) AND NirvanaMsgType IN (2, 10) THEN ParentClorderID 
    END
FROM 
    V_TradingDataFinal
WHERE 
    IsHidden = 0 AND ( NirvanaMsgType = 14 OR (TimeInForce IN (1, 6) AND NirvanaMsgType IN (2, 10)))

INSERT INTO @MultiChildParentSubIDs (ClorderID)
select DISTINCT ParentClorderID from V_TradingDataFinal where NirvanaMsgType = 14 AND  IsHidden = 0


select distinct ClOrderID  INTO #TempClOrderIDs from @MultiChildParentSubIDs
																																													  
CREATE TABLE #TradingDataFinal(
	[StagedOrderID] [varchar](50) NULL,
	[ParentClorderID] [varchar](50) NOT NULL,
	[FillMsgType] [varchar](10) NULL,
	[ClorderID] [varchar](50) NOT NULL,
	[OrderID] [varchar](500) NULL,
	[OrigClOrderID] [varchar](50) NULL,
	[AUECLocalDate] [datetime] NULL,
	[AUECID] [int] NULL,
	[CompanyAUECID] [int] NOT NULL,
	[TimeInForce] [varchar](3) NULL,
	[ExpireTime] [varchar](50) NULL,
	[CumQTY] [float] NOT NULL,
	[AveragePrice] [float] NOT NULL,
	[nirvanaMsgType] [int] NULL
) 


INSERT INTO #TradingDataFinal
           ([StagedOrderID]
           ,[ParentClorderID]
           ,[FillMsgType]
           ,[ClorderID]
           ,[OrderID]
           ,[OrigClOrderID]
           ,[AUECLocalDate]
           ,[AUECID]
           ,[CompanyAUECID]
           ,[TimeInForce]
           ,[ExpireTime]
           ,[CumQTY]
           ,[AveragePrice]
           ,[nirvanaMsgType])

Select 
		StagedOrderID,
		ParentClorderID,
		FillMsgType,
		ClorderID,
		OrderID,
		OrigClOrderID,
		AUECLocalDate,
		AUECID,
		CompanyAUECID,
		TimeInForce,
		ExpireTime,
		CumQTY,
		AveragePrice,
		nirvanaMsgType 
		from V_TradingDataFinal  where (ParentClOrderID in (select distinct ClOrderID from #TempClOrderIDs) )
		--from V_TradingDataFinal  A
		--INNER JOIN #TempClOrderIDs B
		--on A.ParentClOrderID = B.ClorderID
		

delete from #TradingDataFinal where FillMsgType in ('9','G','F') 

DECLARE @TradedSubs TABLE 
(
	ClOrderID VARCHAR(max),
	OrderID VARCHAR(max),
	NewClOrderID VARCHAR(max),
	NewOrderID VARCHAR(max),
    ParentClOrderID VARCHAR(max),
    OrigClOrderID VARCHAR(max),
	ChildParentClOrderID VARCHAR(max),
    ChildOrigClOrderID VARCHAR(max),
	AUECLocalDate Datetime,
	AUECID int,
	CompanyAUECID int,
	TimeInForce varchar(3),
	ExpireTime  VARCHAR(MAX),
	CumQty float,
	AveragePrice float	
)

SELECT CompanyAUECID, MAX(CUACTB.ClearanceTime) as MaxClearanceTime, MIN(CUACTB.ClearanceTime) as MinClearanceTime 
into #AuecIDwiseClearanceTime FROM [T_CompanyAUECClearanceTimeBlotter] CUACTB
GROUP BY CompanyAUECID

Insert INTO @TradedSubs
Select T1.ClorderID,T1.OrderID,T1.ClOrderID as NewClOrderID, T1.OrderID as NewOrderID,T1.ParentClOrderID, T1.OrigClOrderID,'' as ChildParentClOrderID, '' as ChildOrigClOrderID,
T1.AUECLocalDate,T1.AUECID,T1.CompanyAUECID ,t1.TimeInForce, T1.ExpireTime ,t1.CumQTY,t1.AveragePrice FROM #TradingDataFinal T1
--inner JOIN #TradingDataFinal T2
--on T1.StagedOrderID=T2.ClorderID
WHERE T1.nirvanaMsgType in (2,10) AND T1.TimeInForce IN (1,6)


Insert INTO @TradedSubs
Select T1.ClorderID,T1.OrderID,T2.ClOrderID as NewClOrderID, T2.OrderID as NewOrderID,T1.ParentClOrderID, T1.OrigClOrderID,T2.ParentClOrderID as ChildParentClOrderID, T2.OrigClOrderID as ChildOrigClOrderID, 
T2.AUECLocalDate,T2.AUECID,T2.CompanyAUECID ,t1.TimeInForce ,T1.ExpireTime,t2.CumQTY,t2.AveragePrice FROM #TradingDataFinal T1
inner JOIN #TradingDataFinal T2
on T1.ClorderID=T2.StagedOrderID
WHERE T1.nirvanaMsgType in (2,10) AND T1.TimeInForce IN (1,6)

Select * Into #TradedSubs from @TradedSubs 

Select 
ClOrderID ,
OrderID ,
NewClOrderID ,
NewOrderID ,
ParentClOrderID ,
OrigClOrderID ,
ChildParentClOrderID ,
ChildOrigClOrderID ,
AUECLocalDate,
#TradedSubs.AUECID,
#TradedSubs.CompanyAUECID,
CumQty,
AveragePrice 	 
from #TradedSubs
LEFT JOIN #AuecIDwiseClearanceTime AS clearance ON #TradedSubs.CompanyAUECID = clearance.CompanyAUECID
INNER JOIN T_AUEC AUEC ON AUEC.AUECID = #TradedSubs.AUECID
INNER JOIN @AUECDatesTable AS AUECTable ON AUECTable.AUECID = #TradedSubs.AUECID
order by NewClOrderID


Drop table #TradingDataFinal
Drop table #AuecIDwiseClearanceTime
Drop table #TradedSubs
end