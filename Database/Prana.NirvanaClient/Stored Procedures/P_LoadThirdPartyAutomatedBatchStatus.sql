CREATE PROCEDURE [dbo].[P_LoadThirdPartyAutomatedBatchStatus]	
@currentEstTime DATETIME
AS
-- Create temporary tables for last and next run times
CREATE TABLE #LastBatchRunTime (
    FileFormatId INT NOT NULL,
    LastRunTime TIME
);

CREATE TABLE #NextBatchRunTime (
    FileFormatId INT NOT NULL,
    NextRunTime TIME
);

-- Populate temporary tables
INSERT INTO #LastBatchRunTime (FileFormatId, LastRunTime)
SELECT FileFormatId, MAX(BatchRunTime)
FROM T_ThirdPartyTimeBatches
WHERE IsPaused = 0 AND
CAST(BatchRunTime AS TIME) <= CAST(@currentEstTime AS TIME)
GROUP BY FileFormatId;

INSERT INTO #NextBatchRunTime (FileFormatId, NextRunTime)
SELECT FileFormatId, MIN(BatchRunTime)
FROM T_ThirdPartyTimeBatches
WHERE IsPaused = 0 AND
CAST(BatchRunTime AS TIME) > CAST(@currentEstTime AS TIME)
GROUP BY FileFormatId;

-- Create #BatchRunTimeDetails table
CREATE TABLE #BatchRunTimeDetails (
    FileFormatId INT NOT NULL,
    LastBatchRunTime TIME,
    NextBatchRunTime TIME,
    BatchSuccess BIT DEFAULT (0),
    LastRunDate DATETIME,
    TimeBatchId INT 
);

-- Populate #BatchRunTimeDetails
INSERT INTO #BatchRunTimeDetails (FileFormatId, LastBatchRunTime, NextBatchRunTime)
SELECT
    COALESCE(L.FileFormatId, N.FileFormatId) AS FileFormatId,
    L.LastRunTime,
    N.NextRunTime
FROM #LastBatchRunTime L
FULL OUTER JOIN #NextBatchRunTime N ON L.FileFormatId = N.FileFormatId;

-- Update batch success and last run date
UPDATE B
SET
    B.TimeBatchId = T.Id,
    B.BatchSuccess = T.BatchSuccess,
    B.LastRunDate = T.LastRunDate
FROM #BatchRunTimeDetails B
INNER JOIN T_ThirdPartyTimeBatches T ON B.FileFormatId = T.FileFormatId AND B.LastBatchRunTime = T.BatchRunTime;

-- Retrieve final results
SELECT
    T.ThirdPartyBatchId,
    B.LastBatchRunTime,
    B.NextBatchRunTime,
    B.BatchSuccess,
    B.LastRunDate,
    B.TimeBatchId
FROM #BatchRunTimeDetails B
INNER JOIN T_ThirdPartyBatch T ON B.FileFormatId = T.ThirdPartyFormatId;

-- Clean up temporary tables
DROP TABLE #LastBatchRunTime;
DROP TABLE #NextBatchRunTime;
DROP TABLE #BatchRunTimeDetails;



