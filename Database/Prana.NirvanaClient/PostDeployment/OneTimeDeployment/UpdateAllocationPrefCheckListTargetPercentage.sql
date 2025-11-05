IF EXISTS (
		SELECT * FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_AL_AllocationPreferenceDef'
		AND COLUMN_NAME = 'Id'
)
AND EXISTS (
		SELECT * FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'T_AL_CheckList'
		AND COLUMN_NAME = 'CheckListId'
)
BEGIN

CREATE TABLE #TempT_Pref_CheckList_Join
(
	[PresetDefId]            INT NOT NULL,
	[CheckListId]            INT NOT NULL
)
--------------------------------------------------------------------------------------------------------------------------
---- Inserting values in Temporary Table #TempT_Pref_CheckList_Join ------------------------------------------------------
---- from join of Tables  T_AL_AllocationPreferenceDef and T_AL_CheckList-------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
INSERT INTO #TempT_Pref_CheckList_Join
SELECT [PresetDefId],[CheckListId]
FROM T_AL_AllocationPreferenceDef 
JOIN T_AL_CheckList 
	ON Id = PresetDefId 
	AND CheckListId NOT IN (SELECT CheckListId FROM T_AL_AccountCheckListValue)

--------------------------------------------------------------------------------------------------------------------------
---- Inserting values in Table T_AL_AccountCheckListValue from Table T_AL_AllocationPreferenceData -----------------------
--------------------------------------------------------------------------------------------------------------------------
INSERT INTO T_AL_AccountCheckListValue([CheckListId],[AccountId],[Value])
SELECT [CheckListId],[FundId],[Value]
FROM T_AL_AllocationPreferenceData 
JOIN #TempT_Pref_CheckList_Join
	ON T_AL_AllocationPreferenceData.PresetdefId = #TempT_Pref_CheckList_Join.PresetDefId

--------------------------------------------------------------------------------------------------------------------------
---- Inserting values in Table T_AL_StrategyChecklistValues from Table T_AL_StrategyValue --------------------------------
--------------------------------------------------------------------------------------------------------------------------
INSERT INTO T_AL_StrategyChecklistValues([AccountCheckListId],[StrategyId],[Value])
SELECT	TempAC.CheckListPercId,[StrategyId],[Value]
FROM	T_AL_StrategyValue 
JOIN (SELECT PD.Id as PercId,PD.FundId,TempPC.PresetDefId,TempPC.CheckListId,AC.Id as CheckListPercId,AC.AccountId
		FROM T_AL_AllocationPreferenceData PD
		JOIN #TempT_Pref_CheckList_Join TempPC
		ON PD.PresetDefId = TempPC.PresetDefId
		JOIN T_AL_AccountCheckListValue AC
		ON TempPC.CheckListId = AC.CheckListId
		AND AC.AccountId = PD.FundId) TempAC
	ON T_AL_StrategyValue.AllocationPrefDataId = TempAC.PercId

DROP TABLE #TempT_Pref_CheckList_Join

END