CREATE PROCEDURE [dbo].[P_DeleteObsoleteAllocatinoPreference]
	AS
 BEGIN TRAN TRAN1  
  BEGIN TRY     
CREATE TABLE #TempAllocationPreferenceIDs (AllocationPreferenceID INT)

INSERT INTO #TempAllocationPreferenceIDs (AllocationPreferenceID)
SELECT DISTINCT id
FROM T_AL_AllocationPreferenceDef
WHERE T_AL_AllocationPreferenceDef.NAME LIKE '%*PTT#%' OR 
T_AL_AllocationPreferenceDef.NAME LIKE '%*Custom#%' OR
T_AL_AllocationPreferenceDef.NAME LIKE '%*WorkArea#%'

CREATE TABLE #TempAllocationPreferenceIDsToDelete (AllocationPreferenceID INT)

INSERT INTO #TempAllocationPreferenceIDsToDelete (AllocationPreferenceID)  
SELECT AllocationPreferenceID  
FROM #TempAllocationPreferenceIDs  
WHERE AllocationPreferenceID NOT IN 
  (
  SELECT distinct OriginalAllocationPreferenceID  
  FROM T_TradedOrders Where OriginalAllocationPreferenceID IS NOT NULL
  )

DELETE
FROM [T_PTTDetails]
WHERE [T_PTTDetails].[PTTId] IN (
		SELECT AllocationPreferenceID from #TempAllocationPreferenceIDsToDelete
		)
		AND PTTId NOT IN (SELECT PTTId FROM T_PTTAllocationMapping)
DELETE
FROM [T_PTTDefinition]
WHERE [PTTId] IN (
		SELECT AllocationPreferenceID from #TempAllocationPreferenceIDsToDelete
		)
		AND PTTId NOT IN (SELECT PTTId FROM T_PTTAllocationMapping)
-- Deleting from asset table  
DELETE
FROM T_AL_Asset
WHERE CheckListId IN (
		SELECT CheckListid
		FROM T_AL_CheckList
		WHERE PresetDefId IN (
				SELECT AllocationPreferenceID
				FROM #TempAllocationPreferenceIDsToDelete
				)
		)

-- Deleting from exchange table  
DELETE
FROM T_AL_Exchange
WHERE CheckListId IN (
		SELECT CheckListid
		FROM T_AL_CheckList
		WHERE PresetDefId IN (
				SELECT AllocationPreferenceID
				FROM #TempAllocationPreferenceIDsToDelete
				)
		)

		
-- Deleting from order side table  
DELETE
FROM T_AL_OrderSide
WHERE CheckListId IN (
		SELECT CheckListid
		FROM T_AL_CheckList
		WHERE PresetDefId IN (
				SELECT AllocationPreferenceID
				FROM #TempAllocationPreferenceIDsToDelete
				)
		)

-- Deleting from PR table  
DELETE
FROM T_AL_PR
WHERE CheckListId IN (
		SELECT CheckListid
		FROM T_AL_CheckList
		WHERE PresetDefId IN (
				SELECT AllocationPreferenceID
				FROM #TempAllocationPreferenceIDsToDelete
				)
		)

--Deleting from StrategyChecklistValues table
DELETE FROM T_AL_StrategyChecklistValues
where AccountCheckListId IN
(
	SELECT Id 
	FROM T_AL_AccountCheckListValue
	where CheckListId IN
	(
		SELECT CheckListId 
		FROM T_AL_CheckList
		WHERE PresetDefId IN (
				SELECT AllocationPreferenceID 
				FROM #TempAllocationPreferenceIDsToDelete
				)
	)
)

--Deleting from AccountCheckListValue table
 DELETE
 FROM T_AL_AccountCheckListValue 
 WHERE CheckListId IN (
		 SELECT CheckListid 
		 FROM T_AL_CheckList
		 WHERE PresetDefId IN (
				SELECT AllocationPreferenceID 
				FROM #TempAllocationPreferenceIDsToDelete
				)
		)
				

-- Deleting from CheckList table  
DELETE
FROM T_AL_CheckList
WHERE PresetDefId IN (
		SELECT AllocationPreferenceID
		FROM #TempAllocationPreferenceIDsToDelete
		) 

DELETE
FROM T_AL_StrategyValue
WHERE T_AL_StrategyValue.AllocationPrefDataId IN (
		SELECT DISTINCT T_AL_AllocationPreferenceData.Id
		FROM T_AL_AllocationPreferenceData
		WHERE T_AL_AllocationPreferenceData.PresetdefId IN (
				SELECT Id
				FROM T_AL_AllocationPreferenceDef
				WHERE Id IN (
						SELECT AllocationPreferenceID
						FROM #TempAllocationPreferenceIDsToDelete
						)
				)
		)

DELETE
FROM T_AL_AllocationPreferenceData
WHERE PresetdefId IN (
		SELECT Id
		FROM T_AL_AllocationPreferenceDef
		WHERE Id IN (
				SELECT AllocationPreferenceID
				FROM #TempAllocationPreferenceIDsToDelete
				)
		)

DELETE
FROM T_AL_AllocationPreferenceDef
WHERE Id IN (
		SELECT AllocationPreferenceID from #TempAllocationPreferenceIDsToDelete
		)

DROP TABLE #TempAllocationPreferenceIDs

DROP TABLE #TempAllocationPreferenceIDsToDelete

COMMIT TRANSACTION TRAN1  
  END TRY                                                                                                                                                          
  BEGIN CATCH 
	ROLLBACK TRANSACTION TRAN1                             
  END CATCH;
RETURN 0
