-- Handling folloing Corruptions
-- 1. Duplicate closing data in PM_TaxLotClosing -- Done
-- 2. Exact 2 rows in PM_Taxlot table for each closingID -- Done
-- 3. ClosingID Present in PM_Taxlots but not into PM_TaxLotClosing -- Done
-- 4. ClosingID Present in PM_TaxLotClosing but not into PM_Taxlots -- Done
-- 5. ClosingID Present in T_Group but not into PM_Taxlots, PM_TaxLotClosing -- Done
-- 6. TaxlotID present in PM_TaxLotClosing but not in PM_Taxlots -- Done
-- 7. Check the Quantity Properly closed -- Done
-- 8. Closing Id is not null and groupid is not present in T_group

DECLARE @FundIds VARCHAR(max)
DECLARE @FromDate DATETIME
DECLARE @ToDate DATETIME
DECLARE @errormsg VARCHAR(max)

SET @errormsg=''
SET @FromDate=''
SET @ToDate=''
SET @FundIds=''

CREATE TABLE #PositionanTaxLotId
(
	PositionalTaxlotId VARCHAR(50),
	ClosingTaxlotId VARCHAR(50)
)

-- Start of checking 1. Duplicate closing data in PM_TaxLotClosing---
-- Getting all the rows from PM_TaxLotClosing which have more then one occurence of same PositionalTaxlotId and ClosingTaxLotId

INSERT INTO #PositionanTaxLotId
SELECT PositionalTaxlotId, ClosingTaxLotId FROM PM_TaxLotClosing
GROUP BY PositionalTaxlotId, ClosingTaxLotId 
HAVING Count(*)>1

CREATE TABLE #TempClosingId
(
	TaxLotClosingId VARCHAR(50)
)

SELECT DISTINCT AUECLocalDate AS ClosingDate, Symbol, FundID, TaxLotClosingID_FK INTO #ClosingCourrption FROM PM_TaxLots
INNER JOIN PM_TaxLotClosing PTC ON TaxLotClosingID_FK = PTC.TaxlotclosingId
WHERE TaxLotClosingID_FK In 
(SELECT TaxLotClosingID  FROM PM_TaxLotClosing CL INNER JOIN #PositionanTaxLotId N 
ON CL.PositionalTaxlotId = N.PositionalTaxlotId AND CL.ClosingTaxLotId = N.ClosingTaxLotId)
Order by AUECLocalDate, Symbol

ALTER TABLE #ClosingCourrption
ADD Comments VARCHAR (max)

IF Exists( SELECT * FROM #ClosingCourrption)
BEGIN	
	SET @errormsg ='Closing Data is Corrupted.'

	UPDATE #ClosingCourrption SET Comments ='Closing Data is Corrupted: Duplicate PNL.'	
END

-- End of checking 1. Duplicate closing data---

-- Start of checking 2. Exact 2 rows in PM_Taxlot table for each closingID

SELECT Symbol, FundID, TaxLotClosingId_Fk INTO #tempCorruptedData1 FROM PM_Taxlots WHERE TaxLotClosingId_Fk IN (
SELECT TaxLotClosingId_Fk  FROM PM_Taxlots where TaxLotClosingId_Fk is not NULL
GROUP BY TaxLotClosingId_Fk having Count(TaxLotClosingId_Fk) <>2 )

ALTER TABLE #tempCorruptedData1
ADD Comments VARCHAR (max)

IF Exists( SELECT * FROM #tempCorruptedData1)
BEGIN		
		SET @errormsg ='Closing Data is Corrupted'
		UPDATE #tempCorruptedData1 set Comments ='Closing Data is Corrupted.'
End

-- End of checking 2. Exact 2 rows in PM_Taxlot table for each closingID


-- Start of Checking 3. ClosingID Present in PM_Taxlots but not into PM_TaxLotClosing

SELECT AUECModifiedDate,Symbol, FundID, TaxLotClosingId_Fk INTO #tempCorruptedData3 FROM PM_Taxlots 
WHERE TaxLotClosingId_Fk NOT IN (SELECT TaxLotClosingID FROM PM_TaxlotClosing)
AND TaxLotClosingId_Fk IS NOT NULL

ALTER TABLE #tempCorruptedData3
ADD Comments VARCHAR (max)

IF Exists( SELECT * FROM #tempCorruptedData3)
BEGIN		
		SET @errormsg ='Closing Data is Corrupted'
		UPDATE #tempCorruptedData3 set Comments ='Closing Data is Corrupted. Closing Id Present in PM_TaxLos but not in PM_TaxLotClosing.'
End

-- End of Checking 3. ClosingID Present in PM_Taxlots but not into PM_TaxLotClosing

-- Start of checking 4. ClosingID Present in PM_TaxLotClosing but not into PM_Taxlots
CREATE TABLE #tempCorruptedData4
(
	ClosingDate DATETIME,
	Symbol VARCHAR(50),
	FundID VARCHAR(10),
	TaxLotClosingID_FK VARCHAR(100)
)
INSERT INTO #tempCorruptedData4
SELECT AUECLocalDate,'', '', TaxLotClosingID FROM PM_TaxlotClosing 
WHERE TaxLotClosingID NOT IN (SELECT TaxLotClosingID_FK FROM PM_Taxlots)

ALTER TABLE #tempCorruptedData4
ADD Comments VARCHAR (max)

IF Exists( SELECT * FROM #tempCorruptedData4)
BEGIN		
		SET @errormsg ='Closing Data is Corrupted'
		UPDATE #tempCorruptedData4 set Comments ='Closing Data is Corrupted. ClosingID Present in PM_TaxLotClosing but not into PM_Taxlots.'
End
-- End of checking 4. ClosingID Present in PM_TaxLotClosing but not into PM_Taxlots

-- Start of checking 5. ClosingID Present in T_Group but not into PM_Taxlots, PM_TaxLotClosing
CREATE TABLE #tempCorruptedData5
(
	ClosingDate DATETIME,
	Symbol VARCHAR(50),
	FundID VARCHAR(10),
	TaxLotClosingID_FK VARCHAR(100)
)
INSERT INTO #tempCorruptedData5
SELECT AUECLocalDate,Symbol, '', TaxLotClosingId_Fk FROM T_Group 
WHERE TaxLotClosingId_Fk NOT IN (SELECT TaxLotClosingID FROM PM_TaxlotClosing)
AND TaxLotClosingId_Fk IS NOT NULL
AND T_Group.StateID <>1
UNION
SELECT AUECLocalDate,Symbol, '', TaxLotClosingId_Fk FROM T_Group 
WHERE TaxLotClosingId_Fk NOT IN (SELECT TaxLotClosingID_FK FROM PM_Taxlots)
AND TaxLotClosingId_Fk IS NOT NULL
AND T_Group.StateID <>1

ALTER TABLE #tempCorruptedData5
ADD Comments VARCHAR (max)

IF Exists( SELECT * FROM #tempCorruptedData5)
BEGIN		
		SET @errormsg ='Closing Data is Corrupted'
		UPDATE #tempCorruptedData5 set Comments ='Closing Data is Corrupted. Closing Id Present in T_Group but not in PM_Taxlots,PM_TaxLotClosing.'
End

-- End of checking 5. ClosingID Present in T_Group but not into PM_Taxlots, PM_TaxLotClosing

-- Start of checking 6. TaxlotID present in PM_TaxLotClosing but not in PM_Taxlots
CREATE TABLE #tempCorruptedData6
(
	ClosingDate DATETIME,	
	TaxLotClosingID VARCHAR(100)
)
INSERT INTO #tempCorruptedData6
SELECT AUECLocalDate,TaxLotClosingID FROM PM_TaxlotClosing 
WHERE PositionalTaxlotId NOT IN (SELECT TaxLotID FROM PM_Taxlots)
UNION
SELECT AUECLocalDate,TaxLotClosingID FROM PM_TaxlotClosing 
WHERE ClosingTaxlotId NOT IN (SELECT TaxLotID FROM PM_Taxlots)

ALTER TABLE #tempCorruptedData6
ADD Comments VARCHAR (max)

IF Exists( SELECT * FROM #tempCorruptedData6)
BEGIN		
		SET @errormsg ='Closing Data is Corrupted'
		UPDATE #tempCorruptedData6 set Comments ='Closing Data is Corrupted. TaxlotID present in PM_TaxLotClosing but not in PM_Taxlots.'
End
-- End of checking 6. TaxlotID present in PM_TaxLotClosing but not in PM_Taxlots

-- Start of checking 7. Check the Quantity Properly closed
CREATE TABLE #TaxLotCloseAndOpenData
(
	TaxLotId VARCHAR(50),	
	ClosedQty Float
)
INSERT INTO #TaxLotCloseAndOpenData
SELECT PositionalTaxlotId, SUM(ClosedQty) FROM PM_TaxlotClosing
GROUP BY PositionalTaxlotId
ORDER BY PositionalTaxlotId

INSERT INTO #TaxLotCloseAndOpenData
SELECT ClosingTaxlotId, SUM(ClosedQty) FROM PM_TaxlotClosing
GROUP BY ClosingTaxlotId
ORDER BY ClosingTaxlotId

INSERT INTO #TaxLotCloseAndOpenData
Select DISTINCT TaxLotID, TaxLotOpenQty  
From PM_Taxlots  
Where TaxLotOpenQty<>0 and  
Taxlot_PK in                                                                                             
 (                                                                                                   
    Select max(Taxlot_PK) from PM_Taxlots                                                                                                                                                               
    where DateDiff(d,PM_Taxlots.AUECModifiedDate,getdate()) >=0                                                                                                                                      
    group by taxlotid                                                   
  )


CREATE TABLE #FINALDATA
(
	TaxLotId VARCHAR(50),	
	TotalQty Float
)

INSERT INTO #FINALDATA
SELECT TaxLotId, SUM(ClosedQty) 
FROM #TaxLotCloseAndOpenData
GROUP BY TaxLotId
ORDER BY TaxLotId



CREATE TABLE #tempCorruptedData7
(
	DiffQty Float,
	TaxLotId VARCHAR(50),
	Symbol VARCHAR(100),
	FundID Int,
	TradeDate DATETIME
)

INSERT INTO #tempCorruptedData7
SELECT (PM.TaxLotOpenQty - F.TotalQty), F.TaxLotId, PM.Symbol, PM.FundID, PM.AUECModifiedDate  
FROM #FINALDATA F INNER JOIN PM_Taxlots PM ON F.TaxLotId = PM.TaxLotId
WHERE TaxLotClosingId_Fk IS NULL AND ((PM.TaxLotOpenQty - F.TotalQty) >=1 OR (PM.TaxLotOpenQty - F.TotalQty) <=-1) AND (PM.TaxLotOpenQty - F.TotalQty)<>0 

ALTER TABLE #tempCorruptedData7
ADD Comments VARCHAR (max)

IF Exists( SELECT * FROM #tempCorruptedData7)
BEGIN		
		SET @errormsg ='Closing Data is Corrupted'
		UPDATE #tempCorruptedData7 set Comments ='Closing Data is Corrupted. Quantity Properly not closed.'
End
-- End of checking 7. Check the Quantity Properly closed

-- Start of checking 8. Closing ID is not null and GroupID is not present in T_group
CREATE TABLE #tempCorruptedData8
(
	ClosingDate DATETIME,
	Symbol VARCHAR(50),
	FundID VARCHAR(10),
	TaxLotClosingID_FK VARCHAR(100)
)

INSERT INTO #tempCorruptedData8
SELECT AUECModifiedDate, Symbol, FundId, TaxLotClosingId_Fk FROM PM_Taxlots
WHERE GroupID NOT IN (SELECT GroupID FROM T_Group)
AND TaxLotClosingId_Fk IS NOT NULL

ALTER TABLE #tempCorruptedData8
ADD Comments VARCHAR (max)

IF Exists( SELECT * FROM #tempCorruptedData8)
BEGIN		
		SET @errormsg ='Closing Data is Corrupted'
		UPDATE #tempCorruptedData8 set Comments ='Closing Data is Corrupted. GroupID is present in PM_Taxlots but not in T_Group.'
End
-- End of checking 8. Closing ID is not null and GroupID is not present in T_group

CREATE TABLE #FinalClosingCourrption
(
	ClosingDate DATETIME,
	Symbol VARCHAR(50),
	FundID VARCHAR(10),	
	TaxLotClosingID_FK VARCHAR(100),
	DifferenceQty VARCHAR(50),
	Comments VARCHAR(100)
)

INSERT INTO #FinalClosingCourrption
SELECT ClosingDate, Symbol, FundID, TaxLotClosingID_FK,'', Comments from #ClosingCourrption 
UNION ALL
SELECT ISNULL(PM.AUECLocalDate,PM_TaxLots.AUECModifiedDate)  AS 'ClosingDate', #tempCorruptedData1.Symbol,
 #tempCorruptedData1.FundID, #tempCorruptedData1.TaxLotClosingID_FK,'', Comments FROM #tempCorruptedData1
INNER JOIN PM_TaxLots ON PM_TaxLots.TaxLotClosingID_FK = #tempCorruptedData1.TaxLotClosingId_Fk
LEFT JOIN PM_TaxlotClosing PM ON PM.TaxLotClosingID =  #tempCorruptedData1.TaxLotClosingId_Fk
UNION ALL
SELECT AUECModifiedDate AS 'ClosingDate', Symbol, FundID,TaxLotClosingID_FK,'', Comments from #tempCorruptedData3 
UNION ALL
SELECT ClosingDate, Symbol, FundID, TaxLotClosingID_FK,'', Comments from #tempCorruptedData4 
UNION ALL
SELECT ClosingDate, Symbol, FundID, TaxLotClosingID_FK,'', Comments from #tempCorruptedData5 
UNION ALL
SELECT ClosingDate, '', '', TaxLotClosingID, '',Comments from #tempCorruptedData6
UNION ALL
SELECT TradeDate, Symbol, FundID, NULL,DiffQty, Comments from #tempCorruptedData7
UNION ALL
SELECT ClosingDate, Symbol, FundID, TaxLotClosingID_FK,'', Comments from #tempCorruptedData8

SELECT ClosingDate AS 'Closing OR Trade Date', Symbol, CP.FundShortName 'Account Name', TaxLotClosingID_FK AS 'TaxLotClosingID',DifferenceQty, Comments FROM #FinalClosingCourrption F
LEFT JOIN T_CompanyFunds CP ON CP.CompanyFundID = F.FundID

Select  @errormsg as ErrorMsg

DROP TABLE #TempClosingId, #PositionanTaxLotId,#ClosingCourrption, #FinalClosingCourrption,#tempCorruptedData1,#tempCorruptedData3,
#tempCorruptedData4,#tempCorruptedData5,#tempCorruptedData6,#tempCorruptedData7,#tempCorruptedData8,#TaxLotCloseAndOpenData,#FINALDATA


