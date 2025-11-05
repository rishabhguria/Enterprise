/*
Created By: Kuldeep Agrawal
CreationDate: Sep 30th, 2014.
Description: This script Imports Markprices and Betas from the specified file by using some custom rules.

Modified By: Narendra Kumar Jangir
CreationDate: Apr 24, 2015.
Description: Add us exchange in the symbol, remove header row and insert beta for the mentioned date.
*/
CREATE PROCEDURE [dbo].[P_ImportsMarkpricesAndBetas]
AS
--- Input Section ---
DECLARE @filePath VARCHAR(MAX)

SET @filePath = 'C:\LocalUser\GreenowlFTP\GreenOwl_Beta.csv'

--DECLARE @Date datetime
--set @Date='2015-04-29'
--drop table #tempMarkPriceDataTemp
CREATE TABLE #tempMarkPriceDataTemp (
	[Date1] VARCHAR(100)
	,[security] VARCHAR(100)
	,EQY_RAW_BETA_6M VARCHAR(100)
	)

DECLARE @bulkinsert NVARCHAR(2000)

--We have header in the file, so first inserting in the temp table to remove header
SET @bulkinsert = N'BULK INSERT #tempMarkPriceDataTemp FROM ''' + @filePath + N''' WITH (FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'

EXEC sp_executesql @bulkinsert

--Deleting header
DELETE TOP (1)
FROM #tempMarkPriceDataTemp

--remove leading and trailing spaces
UPDATE #tempMarkPriceDataTemp
SET [security] = RTRIM(LTRIM([security]))

CREATE TABLE #tempMarkPriceData (
	[Date1] DATETIME
	,[security] VARCHAR(100)
	,EQY_RAW_BETA_6M FLOAT
	)

--Inserting data in the table after deleting header
INSERT INTO #tempMarkPriceData
SELECT [Date1]
	,CASE 
		WHEN (LEN([security]) - LEN(REPLACE([security], ' ', ''))) < 2
			THEN Replace([security], ' ', ' US ')
		ELSE [security]
		END AS security
	,EQY_RAW_BETA_6M
FROM #tempMarkPriceDataTemp

--select * from #tempMarkPriceData
ALTER TABLE #tempMarkPriceData ADD TickerSymbol VARCHAR(50)
	,AssetID INT

---------------------insert Symbol and Asset IDs for securities----------------------------- 
UPDATE TEMP
SET TEMP.TickerSymbol = sec.TickerSymbol
	,TEMP.AssetID = sec.Assetid
FROM #tempMarkPriceData TEMP
INNER JOIN V_SecMasterData_WithUnderlying sec ON TEMP.security = sec.BloombergSymbol

DELETE
FROM #tempMarkPriceData
WHERE tickersymbol IS NULL

--select * from #tempMarkPriceData
--------------------------------Deleting Old Betas-----------------------------
DELETE PM_Dailybeta
FROM PM_Dailybeta
INNER JOIN #tempMarkPriceData ON DateDiff(d, #tempMarkPriceData.Date1, PM_Dailybeta.DATE) = 0
	AND #tempMarkPriceData.TickerSymbol = PM_Dailybeta.Symbol

------------------------------------Inserting New Betas----------------------------
INSERT INTO [dbo].[PM_Dailybeta] (
	[Date]
	,[Symbol]
	,[Beta]
	)
SELECT DISTINCT [Date1]
	,TickerSymbol
	,ISNULL(EQY_RAW_BETA_6M, 0) AS Beta
FROM #tempMarkPriceData

----------------------------------Dropping Temporary tables----------------------------
DROP TABLE #tempMarkPriceData
	,#tempMarkPriceDataTemp
